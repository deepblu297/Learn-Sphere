using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace LearnSphere
{
    public partial class Default : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["LearnSphereDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCourses();
            }
        }

        private void LoadCourses()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT TOP 4 
                        c.CourseID, c.CourseName, c.Description, c.ThumbnailPath,
                        u.FullName AS InstructorName
                    FROM Courses c
                    INNER JOIN Users u ON c.CreatedBy = u.UserID
                    WHERE u.Role = 'Instructor'
                    ORDER BY c.CreatedDate DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                try
                {
                    conn.Open();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        rptHomeCourses.DataSource = dt;
                        rptHomeCourses.DataBind();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Error loading courses: " + ex.Message);
                }
            }
        }
    }
}
