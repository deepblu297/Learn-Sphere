using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace LearnSphere
{
    public partial class Explore : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["LearnSphereDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCourses();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadCourses(txtSearch.Text.Trim());
        }

        private void LoadCourses(string searchTerm = "")
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT c.CourseID, c.CourseName, c.Description, c.ThumbnailPath,
                           u.FullName AS InstructorName
                    FROM Courses c
                    INNER JOIN Users u ON c.CreatedBy = u.UserID
                    WHERE u.Role='Instructor' AND 
                          (@SearchTerm='' OR c.CourseName LIKE '%' + @SearchTerm + '%' OR c.Description LIKE '%' + @SearchTerm + '%')
                    ORDER BY c.CreatedDate DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SearchTerm", searchTerm);

                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                try
                {
                    conn.Open();
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
                    System.Diagnostics.Debug.WriteLine("Error loading courses: " + ex.Message);
                }
            }
        }

        protected string GetThumbnailHtml(object thumbnailPath)
        {
            if (thumbnailPath == null || string.IsNullOrEmpty(thumbnailPath.ToString()))
            {
                return "<div class='placeholder-thumbnail'>📚</div>";
            }
            else
            {
                return $"<img src='{ResolveUrl("~/Thumbnail/" + thumbnailPath)}' class='course-thumbnail' alt='Course Thumbnail' />";
            }
        }
    }
}
