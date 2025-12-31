using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace LearnSphere.Instructor
{
    public partial class InstructorDashboard : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["LearnSphereDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check if instructor is logged in
                if (Session["UserID"] == null || Session["Role"] == null || Session["Role"].ToString() != "Instructor")
                {
                    Response.Redirect("~/Login.aspx");
                    return;
                }

                LoadDashboardData();
            }
        }

        private void LoadDashboardData()
        {
            int instructorId = Convert.ToInt32(Session["UserID"]);

            // Load instructor name
            LoadInstructorName(instructorId);

            // Load statistics
            LoadStatistics(instructorId);

            // Load instructor's courses
            LoadMyCourses(instructorId);

            // Load recent enrollments
            LoadRecentEnrollments(instructorId);
        }

        private void LoadInstructorName(int instructorId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT FullName FROM Users WHERE UserID = @UserID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@UserID", instructorId);

                try
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        lblInstructorName.Text = result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    // Log error
                    System.Diagnostics.Debug.WriteLine("Error loading instructor name: " + ex.Message);
                }
            }
        }

        private void LoadStatistics(int instructorId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Get total courses count
                    string coursesQuery = "SELECT COUNT(*) FROM Courses WHERE CreatedBy = @InstructorID";
                    SqlCommand cmdCourses = new SqlCommand(coursesQuery, conn);
                    cmdCourses.Parameters.AddWithValue("@InstructorID", instructorId);
                    int totalCourses = (int)cmdCourses.ExecuteScalar();
                    lblMyCourses.Text = totalCourses.ToString();

                    // Get total students enrolled in instructor's courses
                    string studentsQuery = @"SELECT COUNT(DISTINCT e.UserID) 
                                           FROM Enrollments e
                                           INNER JOIN Courses c ON e.CourseID = c.CourseID
                                           WHERE c.CreatedBy = @InstructorID";
                    SqlCommand cmdStudents = new SqlCommand(studentsQuery, conn);
                    cmdStudents.Parameters.AddWithValue("@InstructorID", instructorId);
                    object studentsResult = cmdStudents.ExecuteScalar();
                    int totalStudents = studentsResult != DBNull.Value ? Convert.ToInt32(studentsResult) : 0;
                    lblTotalStudents.Text = totalStudents.ToString();

                    // Get most popular course
                    string popularCourseQuery = @"SELECT TOP 1 c.CourseName
                                                FROM Courses c
                                                LEFT JOIN Enrollments e ON c.CourseID = e.CourseID
                                                WHERE c.CreatedBy = @InstructorID
                                                GROUP BY c.CourseID, c.CourseName
                                                ORDER BY COUNT(e.EnrollmentID) DESC";
                    SqlCommand cmdPopular = new SqlCommand(popularCourseQuery, conn);
                    cmdPopular.Parameters.AddWithValue("@InstructorID", instructorId);
                    object popularCourse = cmdPopular.ExecuteScalar();
                    lblPopularCourse.Text = popularCourse != null ? popularCourse.ToString() : "N/A";
                }
                catch (Exception ex)
                {
                    // Log error
                    System.Diagnostics.Debug.WriteLine("Error loading statistics: " + ex.Message);
                    lblMyCourses.Text = "0";
                    lblTotalStudents.Text = "0";
                    lblPopularCourse.Text = "N/A";
                }
            }
        }

        private void LoadMyCourses(int instructorId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT 
                                    c.CourseID,
                                    c.CourseName,
                                    c.Description,
                                    COUNT(e.EnrollmentID) AS EnrollmentCount,
                                    c.CreatedDate
                                FROM Courses c
                                LEFT JOIN Enrollments e ON c.CourseID = e.CourseID
                                WHERE c.CreatedBy = @InstructorID
                                GROUP BY c.CourseID, c.CourseName, c.Description, c.CreatedDate
                                ORDER BY c.CreatedDate DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@InstructorID", instructorId);

                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvMyCourses.DataSource = dt;
                    gvMyCourses.DataBind();
                }
                catch (Exception ex)
                {
                    // Log error
                    System.Diagnostics.Debug.WriteLine("Error loading courses: " + ex.Message);
                }
            }
        }

        private void LoadRecentEnrollments(int instructorId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT TOP 10
                                    u.FullName,
                                    u.Email,
                                    c.CourseName,
                                    e.EnrolledDate
                                FROM Enrollments e
                                INNER JOIN Courses c ON e.CourseID = c.CourseID
                                INNER JOIN Users u ON e.UserID = u.UserID
                                WHERE c.CreatedBy = @InstructorID
                                ORDER BY e.EnrolledDate DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@InstructorID", instructorId);

                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvRecentEnrollments.DataSource = dt;
                    gvRecentEnrollments.DataBind();
                }
                catch (Exception ex)
                {
                    // Log error
                    System.Diagnostics.Debug.WriteLine("Error loading recent enrollments: " + ex.Message);
                }
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Clear all session variables
            Session.Clear();
            Session.Abandon();

            // Redirect to login page
            Response.Redirect("~/Login.aspx");
        }
    }
}