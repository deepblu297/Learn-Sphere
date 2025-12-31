using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LearnSphere.Learner
{
    public partial class Dashboard : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["LearnSphereDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStudentName();
                LoadStatistics();
                LoadCourses();
            }
        }

        private void LoadStudentName()
        {
            if (Session["UserID"] != null)
            {
                int studentId = Convert.ToInt32(Session["UserID"]);
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    string query = "SELECT FullName FROM Users WHERE UserID = @UserID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@UserID", studentId);

                    try
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            lblStudentName.Text = result.ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Error loading student name: " + ex.Message);
                    }
                }
            }
        }

        private void LoadStatistics()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Total available courses
                    string totalQuery = "SELECT COUNT(*) FROM Courses";
                    SqlCommand cmdTotal = new SqlCommand(totalQuery, conn);
                    int totalCourses = (int)cmdTotal.ExecuteScalar();
                    lblTotalCourses.Text = totalCourses.ToString();

                    // Enrolled courses
                    int enrolledCount = 0;
                    if (Session["UserID"] != null)
                    {
                        int studentId = Convert.ToInt32(Session["UserID"]);
                        string enrolledQuery = "SELECT COUNT(*) FROM Enrollments WHERE UserID = @UserID";
                        SqlCommand cmdEnrolled = new SqlCommand(enrolledQuery, conn);
                        cmdEnrolled.Parameters.AddWithValue("@UserID", studentId);
                        enrolledCount = (int)cmdEnrolled.ExecuteScalar();
                    }
                    lblEnrolledCourses.Text = enrolledCount.ToString();

                    // Available to enroll
                    lblAvailableToEnroll.Text = (totalCourses - enrolledCount).ToString();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error loading statistics: " + ex.Message);
                }
            }
        }

        private void LoadCourses(string searchTerm = "")
        {
            int studentId = Session["UserID"] != null ? Convert.ToInt32(Session["UserID"]) : 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT 
                                    c.CourseID,
                                    c.CourseName,
                                    c.Description,
                                    c.ThumbnailPath,
                                    c.CreatedDate,
                                    u.FullName AS InstructorName,
                                    COUNT(DISTINCT e.EnrollmentID) AS EnrollmentCount,
                                    CASE 
                                        WHEN EXISTS (SELECT 1 FROM Enrollments WHERE CourseID = c.CourseID AND UserID = @StudentID)
                                        THEN 1 
                                        ELSE 0 
                                    END AS IsEnrolled
                                FROM Courses c
                                LEFT JOIN Users u ON c.CreatedBy = u.UserID
                                LEFT JOIN Enrollments e ON c.CourseID = e.CourseID
                                WHERE (@SearchTerm = '' OR c.CourseName LIKE '%' + @SearchTerm + '%' OR c.Description LIKE '%' + @SearchTerm + '%')
                                GROUP BY c.CourseID, c.CourseName, c.Description, c.ThumbnailPath, c.CreatedDate, u.FullName
                                ORDER BY c.CreatedDate DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@StudentID", studentId);
                cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);

                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        rptCourses.DataSource = dt;
                        rptCourses.DataBind();
                        pnlNoCourses.Visible = false;
                    }
                    else
                    {
                        rptCourses.DataSource = null;
                        rptCourses.DataBind();
                        pnlNoCourses.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage("Error loading courses: " + ex.Message, false);
                }
            }
        }


        protected void rptCourses_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int courseId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "ViewDetails")
            {
                Response.Redirect($"CourseDetails.aspx?CourseID={courseId}");
            }
            else if (e.CommandName == "EnrollCourse")
            {
                EnrollInCourse(courseId);
            }
        }

        private void EnrollInCourse(int courseId)
        {
            if (Session["UserID"] == null)
            {
                ShowMessage("Please login to enroll in courses.", false);
                return;
            }

            int studentId = Convert.ToInt32(Session["UserID"]);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                // Check if already enrolled
                string checkQuery = "SELECT COUNT(*) FROM Enrollments WHERE UserID = @UserID AND CourseID = @CourseID";
                SqlCommand cmdCheck = new SqlCommand(checkQuery, conn);
                cmdCheck.Parameters.AddWithValue("@UserID", studentId);
                cmdCheck.Parameters.AddWithValue("@CourseID", courseId);

                try
                {
                    conn.Open();
                    int count = (int)cmdCheck.ExecuteScalar();

                    if (count > 0)
                    {
                        ShowMessage("You are already enrolled in this course!", false);
                        return;
                    }

                    // Enroll student
                    string enrollQuery = "INSERT INTO Enrollments (UserID, CourseID, EnrolledDate) VALUES (@UserID, @CourseID, GETDATE())";
                    SqlCommand cmdEnroll = new SqlCommand(enrollQuery, conn);
                    cmdEnroll.Parameters.AddWithValue("@UserID", studentId);
                    cmdEnroll.Parameters.AddWithValue("@CourseID", courseId);

                    int rowsAffected = cmdEnroll.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        ShowMessage("🎉 Successfully enrolled in the course!", true);
                        LoadStatistics();
                        LoadCourses(txtSearch.Text.Trim());
                    }
                    else
                    {
                        ShowMessage("Failed to enroll. Please try again.", false);
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage("Error enrolling in course: " + ex.Message, false);
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadCourses(txtSearch.Text.Trim());
        }

        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            LoadCourses();
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            pnlMessage.Visible = true;
            lblMessage.Text = message;
            pnlMessage.CssClass = isSuccess ? "alert alert-success" : "alert alert-error";
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }
    }
}