using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

namespace LearnSphere.Admin
{
    public partial class AdminDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is logged in and is Admin
            if (Session["UserID"] == null || Session["Role"]?.ToString() != "Admin")
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                // Set admin name in master page
                Label lblAdmin = (Label)Master.FindControl("lblAdminName");
                if (lblAdmin != null)
                    lblAdmin.Text = Session["FullName"]?.ToString();

                LoadDashboardData();
            }
        }

        private void LoadDashboardData()
        {
            string connStr = ConfigurationManager.ConnectionStrings["LearnSphereDB"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();

                // Total Users
                SqlCommand cmdUsers = new SqlCommand("SELECT COUNT(*) FROM Users", con);
                lblTotalUsers.Text = cmdUsers.ExecuteScalar().ToString();

                // Total Courses
                SqlCommand cmdCourses = new SqlCommand("SELECT COUNT(*) FROM Courses", con);
                lblTotalCourses.Text = cmdCourses.ExecuteScalar().ToString();

                // Total Enrollments
                SqlCommand cmdEnroll = new SqlCommand("SELECT COUNT(*) FROM Enrollments", con);
                lblTotalEnrollments.Text = cmdEnroll.ExecuteScalar().ToString();

                // Total Instructors
                SqlCommand cmdInstructors = new SqlCommand("SELECT COUNT(*) FROM Users WHERE Role='Instructor'", con);
                lblTotalInstructors.Text = cmdInstructors.ExecuteScalar().ToString();

                // Recent Users
                SqlDataAdapter daUsers = new SqlDataAdapter("SELECT TOP 5 UserID, FullName, Email, Role FROM Users ORDER BY UserID DESC", con);
                DataTable dtUsers = new DataTable();
                daUsers.Fill(dtUsers);
                gvRecentUsers.DataSource = dtUsers;
                gvRecentUsers.DataBind();

                // Recent Courses
                SqlDataAdapter daCourses = new SqlDataAdapter("SELECT TOP 5 CourseID, CourseName, CreatedDate FROM Courses ORDER BY CourseID DESC", con);
                DataTable dtCourses = new DataTable();
                daCourses.Fill(dtCourses);
                gvRecentCourses.DataSource = dtCourses;
                gvRecentCourses.DataBind();
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("~/Login.aspx");
        }
    }
}
