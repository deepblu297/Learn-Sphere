using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace LearnSphere.Admin
{
    public partial class Courses : System.Web.UI.Page
    {
        string connectionString =
            ConfigurationManager.ConnectionStrings["LearnSphereDB"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Admin security check
            if (Session["UserID"] == null || Session["Role"]?.ToString() != "Admin")
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadCourses();
            }
        }

        private void LoadCourses()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT CourseID, CourseName, Description, Content, CreatedDate
                                 FROM Courses
                                 ORDER BY CreatedDate DESC";

                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                gvCourses.DataSource = dt;
                gvCourses.DataBind();
            }
        }

        protected void btnAddCourse_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCourseName.Text) ||
                string.IsNullOrWhiteSpace(txtDescription.Text) ||
                string.IsNullOrWhiteSpace(txtContent.Text))
            {
                ShowMessage("All fields are required.", false);
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Courses
                                (CourseName, Description, Content, CreatedBy)
                                 VALUES (@CourseName, @Description, @Content, @CreatedBy)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CourseName", txtCourseName.Text.Trim());
                cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
                cmd.Parameters.AddWithValue("@Content", txtContent.Text.Trim());
                cmd.Parameters.AddWithValue("@CreatedBy", Convert.ToInt32(Session["UserID"]));

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            ShowMessage("Course added successfully!", true);

            txtCourseName.Text = "";
            txtDescription.Text = "";
            txtContent.Text = "";

            LoadCourses();
        }

        protected void gvCourses_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvCourses.EditIndex = e.NewEditIndex;
            LoadCourses();
        }

        protected void gvCourses_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvCourses.EditIndex = -1;
            LoadCourses();
        }

        protected void gvCourses_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int courseId = Convert.ToInt32(gvCourses.DataKeys[e.RowIndex].Value);

            string courseName = ((TextBox)gvCourses.Rows[e.RowIndex].Cells[1].Controls[0]).Text;
            string description = ((TextBox)gvCourses.Rows[e.RowIndex].Cells[2].Controls[0]).Text;
            string content = ((TextBox)gvCourses.Rows[e.RowIndex].Cells[3].Controls[0]).Text;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE Courses
                                 SET CourseName=@CourseName,
                                     Description=@Description,
                                     Content=@Content
                                 WHERE CourseID=@CourseID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CourseName", courseName);
                cmd.Parameters.AddWithValue("@Description", description);
                cmd.Parameters.AddWithValue("@Content", content);
                cmd.Parameters.AddWithValue("@CourseID", courseId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            gvCourses.EditIndex = -1;
            ShowMessage("Course updated successfully!", true);
            LoadCourses();
        }

        protected void gvCourses_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int courseId = Convert.ToInt32(gvCourses.DataKeys[e.RowIndex].Value);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Courses WHERE CourseID=@CourseID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CourseID", courseId);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            ShowMessage("Course deleted successfully!", true);
            LoadCourses();
        }

        private void ShowMessage(string message, bool success)
        {
            pnlMessage.Visible = true;
            pnlMessage.CssClass = success ? "alert alert-success" : "alert alert-danger";
            lblMessage.Text = message;
        }
    }
}
