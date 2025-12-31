using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LearnSphere.Learner
{
    public partial class MyCourses : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["LearnSphereDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStudentName();
                LoadStatistics();
                LoadEnrolledCourses();
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

                    // Enrolled courses count
                    int enrolledCount = 0;
                    if (Session["UserID"] != null)
                    {
                        int studentId = Convert.ToInt32(Session["UserID"]);
                        string enrolledQuery = "SELECT COUNT(*) FROM Enrollments WHERE UserID = @UserID";
                        SqlCommand cmdEnrolled = new SqlCommand(enrolledQuery, conn);
                        cmdEnrolled.Parameters.AddWithValue("@UserID", studentId);
                        enrolledCount = (int)cmdEnrolled.ExecuteScalar();
                    }
                    lblEnrolledCount.Text = enrolledCount.ToString();

                    // Total available courses
                    string totalQuery = "SELECT COUNT(*) FROM Courses";
                    SqlCommand cmdTotal = new SqlCommand(totalQuery, conn);
                    int totalCourses = (int)cmdTotal.ExecuteScalar();
                    lblAvailableCount.Text = (totalCourses - enrolledCount).ToString();

                    // Latest enrollment date
                    if (Session["UserID"] != null && enrolledCount > 0)
                    {
                        int studentId = Convert.ToInt32(Session["UserID"]);
                        string latestQuery = "SELECT TOP 1 EnrolledDate FROM Enrollments WHERE UserID = @UserID ORDER BY EnrolledDate DESC";
                        SqlCommand cmdLatest = new SqlCommand(latestQuery, conn);
                        cmdLatest.Parameters.AddWithValue("@UserID", studentId);
                        object result = cmdLatest.ExecuteScalar();
                        if (result != null)
                        {
                            lblLatestEnrollment.Text = Convert.ToDateTime(result).ToString("MMM dd, yyyy");
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error loading statistics: " + ex.Message);
                }
            }
        }

        private void LoadEnrolledCourses()
        {
            if (Session["UserID"] == null)
            {
                pnlNoEnrollments.Visible = true;
                return;
            }

            int studentId = Convert.ToInt32(Session["UserID"]);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT 
                                    e.EnrollmentID,
                                    e.EnrolledDate,
                                    c.CourseID,
                                    c.CourseName,
                                    c.Description,
                                    c.ThumbnailPath,
                                    c.ResourcePath,
                                    u.FullName AS InstructorName
                                FROM Enrollments e
                                INNER JOIN Courses c ON e.CourseID = c.CourseID
                                LEFT JOIN Users u ON c.CreatedBy = u.UserID
                                WHERE e.UserID = @StudentID
                                ORDER BY e.EnrolledDate DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@StudentID", studentId);

                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        rptEnrolledCourses.DataSource = dt;
                        rptEnrolledCourses.DataBind();
                        pnlNoEnrollments.Visible = false;
                    }
                    else
                    {
                        rptEnrolledCourses.DataSource = null;
                        rptEnrolledCourses.DataBind();
                        pnlNoEnrollments.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage("Error loading enrolled courses: " + ex.Message, false);
                    pnlNoEnrollments.Visible = true;
                }
            }
        }

        protected void rptEnrolledCourses_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "ViewCourse")
            {
                int courseId = Convert.ToInt32(e.CommandArgument);
                Response.Redirect($"CourseDetails.aspx?CourseID={courseId}");
            }
            else if (e.CommandName == "Unenroll")
            {
                int enrollmentId = Convert.ToInt32(e.CommandArgument);
                UnenrollFromCourse(enrollmentId);
            }
        }

        private void UnenrollFromCourse(int enrollmentId)
        {
            if (Session["UserID"] == null)
            {
                ShowMessage("Please login to manage your enrollments.", false);
                return;
            }

            int studentId = Convert.ToInt32(Session["UserID"]);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Enrollments WHERE EnrollmentID = @EnrollmentID AND UserID = @UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@EnrollmentID", enrollmentId);
                cmd.Parameters.AddWithValue("@UserID", studentId);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        ShowMessage("Successfully unenrolled from the course.", true);
                        LoadStatistics();
                        LoadEnrolledCourses();
                    }
                    else
                    {
                        ShowMessage("Failed to unenroll. Please try again.", false);
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage("Error unenrolling from course: " + ex.Message, false);
                }
            }
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