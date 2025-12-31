using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LearnSphere.Instructor
{
    public partial class ManageCourses : System.Web.UI.Page
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

                LoadInstructorName();
                LoadCourses();
            }
        }

        private void LoadInstructorName()
        {
            int instructorId = Convert.ToInt32(Session["UserID"]);
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
                    System.Diagnostics.Debug.WriteLine("Error loading instructor name: " + ex.Message);
                }
            }
        }

        private void LoadCourses()
        {
            int instructorId = Convert.ToInt32(Session["UserID"]);
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT 
                                    c.CourseID,
                                    c.CourseName,
                                    c.Description,
                                    c.ThumbnailPath,
                                    c.ResourcePath,
                                    COUNT(e.EnrollmentID) AS EnrollmentCount,
                                    c.CreatedDate
                                FROM Courses c
                                LEFT JOIN Enrollments e ON c.CourseID = e.CourseID
                                WHERE c.CreatedBy = @InstructorID
                                GROUP BY c.CourseID, c.CourseName, c.Description, c.ThumbnailPath, c.ResourcePath, c.CreatedDate
                                ORDER BY c.CreatedDate DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@InstructorID", instructorId);

                try
                {
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    gvCourses.DataSource = dt;
                    gvCourses.DataBind();
                }
                catch (Exception ex)
                {
                    ShowMessage("Error loading courses: " + ex.Message, false);
                }
            }
        }

        protected void btnSaveCourse_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            int instructorId = Convert.ToInt32(Session["UserID"]);
            int courseId = Convert.ToInt32(hfCourseID.Value);

            string thumbnailPath = null;
            string resourcePath = null;

            // Handle thumbnail upload
            if (fuThumbnail.HasFile)
            {
                try
                {
                    string fileName = Path.GetFileName(fuThumbnail.FileName);
                    string fileExtension = Path.GetExtension(fileName).ToLower();

                    // Validate image file
                    if (fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".png" || fileExtension == ".gif")
                    {
                        // Create unique filename
                        string uniqueFileName = "thumb_" + DateTime.Now.Ticks + fileExtension;
                        string thumbnailFolder = Server.MapPath("~/Thumbnail/");

                        // Create folder if doesn't exist
                        if (!Directory.Exists(thumbnailFolder))
                        {
                            Directory.CreateDirectory(thumbnailFolder);
                        }

                        string filePath = Path.Combine(thumbnailFolder, uniqueFileName);
                        fuThumbnail.SaveAs(filePath);
                        thumbnailPath = "Thumbnail/" + uniqueFileName;
                    }
                    else
                    {
                        ShowMessage("Please upload a valid image file (JPG, PNG, GIF)", false);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage("Error uploading thumbnail: " + ex.Message, false);
                    return;
                }
            }

            // Handle resource upload
            if (fuResource.HasFile)
            {
                try
                {
                    string fileName = Path.GetFileName(fuResource.FileName);
                    string fileExtension = Path.GetExtension(fileName).ToLower();

                    // Validate document file
                    if (fileExtension == ".pdf" || fileExtension == ".doc" || fileExtension == ".docx")
                    {
                        // Create unique filename
                        string uniqueFileName = "res_" + DateTime.Now.Ticks + fileExtension;
                        string resourceFolder = Server.MapPath("~/Resources/");

                        // Create folder if doesn't exist
                        if (!Directory.Exists(resourceFolder))
                        {
                            Directory.CreateDirectory(resourceFolder);
                        }

                        string filePath = Path.Combine(resourceFolder, uniqueFileName);
                        fuResource.SaveAs(filePath);
                        resourcePath = "Resources/" + uniqueFileName;
                    }
                    else
                    {
                        ShowMessage("Please upload a valid document file (PDF, DOC, DOCX)", false);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage("Error uploading resource: " + ex.Message, false);
                    return;
                }
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query;
                if (courseId == 0)
                {
                    // Insert new course
                    query = @"INSERT INTO Courses (CourseName, Description, Content, ThumbnailPath, ResourcePath, CreatedBy, CreatedDate) 
                             VALUES (@CourseName, @Description, @Content, @ThumbnailPath, @ResourcePath, @CreatedBy, GETDATE())";
                }
                else
                {
                    // Update existing course
                    if (thumbnailPath != null && resourcePath != null)
                    {
                        query = @"UPDATE Courses 
                                 SET CourseName = @CourseName, 
                                     Description = @Description, 
                                     Content = @Content,
                                     ThumbnailPath = @ThumbnailPath,
                                     ResourcePath = @ResourcePath
                                 WHERE CourseID = @CourseID AND CreatedBy = @CreatedBy";
                    }
                    else if (thumbnailPath != null)
                    {
                        query = @"UPDATE Courses 
                                 SET CourseName = @CourseName, 
                                     Description = @Description, 
                                     Content = @Content,
                                     ThumbnailPath = @ThumbnailPath
                                 WHERE CourseID = @CourseID AND CreatedBy = @CreatedBy";
                    }
                    else if (resourcePath != null)
                    {
                        query = @"UPDATE Courses 
                                 SET CourseName = @CourseName, 
                                     Description = @Description, 
                                     Content = @Content,
                                     ResourcePath = @ResourcePath
                                 WHERE CourseID = @CourseID AND CreatedBy = @CreatedBy";
                    }
                    else
                    {
                        query = @"UPDATE Courses 
                                 SET CourseName = @CourseName, 
                                     Description = @Description, 
                                     Content = @Content
                                 WHERE CourseID = @CourseID AND CreatedBy = @CreatedBy";
                    }
                }

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CourseName", txtCourseName.Text.Trim());
                cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
                cmd.Parameters.AddWithValue("@Content", txtContent.Text.Trim());
                cmd.Parameters.AddWithValue("@CreatedBy", instructorId);

                if (thumbnailPath != null)
                {
                    cmd.Parameters.AddWithValue("@ThumbnailPath", thumbnailPath);
                }
                else if (courseId == 0)
                {
                    cmd.Parameters.AddWithValue("@ThumbnailPath", DBNull.Value);
                }

                if (resourcePath != null)
                {
                    cmd.Parameters.AddWithValue("@ResourcePath", resourcePath);
                }
                else if (courseId == 0)
                {
                    cmd.Parameters.AddWithValue("@ResourcePath", DBNull.Value);
                }

                if (courseId != 0)
                {
                    cmd.Parameters.AddWithValue("@CourseID", courseId);
                }

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        string message = courseId == 0 ? "Course added successfully!" : "Course updated successfully!";
                        ShowMessage(message, true);
                        ClearForm();
                        LoadCourses();
                    }
                    else
                    {
                        ShowMessage("Failed to save course. Please try again.", false);
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage("Error saving course: " + ex.Message, false);
                }
            }
        }

        protected void gvCourses_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int courseId = Convert.ToInt32(e.CommandArgument);

            if (e.CommandName == "EditCourse")
            {
                LoadCourseForEdit(courseId);
            }
            else if (e.CommandName == "DeleteCourse")
            {
                DeleteCourse(courseId);
            }
        }

        private void LoadCourseForEdit(int courseId)
        {
            int instructorId = Convert.ToInt32(Session["UserID"]);
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT CourseID, CourseName, Description, Content, ThumbnailPath, ResourcePath 
                               FROM Courses 
                               WHERE CourseID = @CourseID AND CreatedBy = @InstructorID";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CourseID", courseId);
                cmd.Parameters.AddWithValue("@InstructorID", instructorId);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        hfCourseID.Value = reader["CourseID"].ToString();
                        txtCourseName.Text = reader["CourseName"].ToString();
                        txtDescription.Text = reader["Description"].ToString();
                        txtContent.Text = reader["Content"].ToString();

                        // Show current thumbnail info
                        if (!reader.IsDBNull(reader.GetOrdinal("ThumbnailPath")))
                        {
                            lblCurrentThumbnail.Text = "Current: " + Path.GetFileName(reader["ThumbnailPath"].ToString());
                            lblCurrentThumbnail.Visible = true;
                        }

                        // Show current resource info
                        if (!reader.IsDBNull(reader.GetOrdinal("ResourcePath")))
                        {
                            lblCurrentResource.Text = "Current: " + Path.GetFileName(reader["ResourcePath"].ToString());
                            lblCurrentResource.Visible = true;
                        }

                        lblFormTitle.Text = "Edit Course";
                        btnCancelEdit.Visible = true;
                        btnSaveCourse.Text = "Update Course";

                        // Scroll to form
                        ScriptManager.RegisterStartupScript(this, GetType(), "ScrollToForm",
                            "window.scrollTo(0, 0);", true);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    ShowMessage("Error loading course: " + ex.Message, false);
                }
            }
        }

        private void DeleteCourse(int courseId)
        {
            int instructorId = Convert.ToInt32(Session["UserID"]);
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    // Get file paths before deletion
                    string getThumbnailQuery = "SELECT ThumbnailPath, ResourcePath FROM Courses WHERE CourseID = @CourseID";
                    SqlCommand cmdGetFiles = new SqlCommand(getThumbnailQuery, conn, transaction);
                    cmdGetFiles.Parameters.AddWithValue("@CourseID", courseId);
                    SqlDataReader reader = cmdGetFiles.ExecuteReader();

                    string thumbnailPath = null;
                    string resourcePath = null;

                    if (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                            thumbnailPath = reader.GetString(0);
                        if (!reader.IsDBNull(1))
                            resourcePath = reader.GetString(1);
                    }
                    reader.Close();

                    // First delete enrollments
                    string deleteEnrollments = "DELETE FROM Enrollments WHERE CourseID = @CourseID";
                    SqlCommand cmdEnrollments = new SqlCommand(deleteEnrollments, conn, transaction);
                    cmdEnrollments.Parameters.AddWithValue("@CourseID", courseId);
                    cmdEnrollments.ExecuteNonQuery();

                    // Then delete course
                    string deleteCourse = "DELETE FROM Courses WHERE CourseID = @CourseID AND CreatedBy = @InstructorID";
                    SqlCommand cmdCourse = new SqlCommand(deleteCourse, conn, transaction);
                    cmdCourse.Parameters.AddWithValue("@CourseID", courseId);
                    cmdCourse.Parameters.AddWithValue("@InstructorID", instructorId);
                    int rowsAffected = cmdCourse.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        transaction.Commit();

                        // Delete physical files
                        if (!string.IsNullOrEmpty(thumbnailPath))
                        {
                            string fullThumbnailPath = Server.MapPath("~/" + thumbnailPath);
                            if (File.Exists(fullThumbnailPath))
                            {
                                File.Delete(fullThumbnailPath);
                            }
                        }

                        if (!string.IsNullOrEmpty(resourcePath))
                        {
                            string fullResourcePath = Server.MapPath("~/" + resourcePath);
                            if (File.Exists(fullResourcePath))
                            {
                                File.Delete(fullResourcePath);
                            }
                        }

                        ShowMessage("Course deleted successfully!", true);
                        LoadCourses();
                    }
                    else
                    {
                        transaction.Rollback();
                        ShowMessage("Failed to delete course.", false);
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ShowMessage("Error deleting course: " + ex.Message, false);
                }
            }
        }

        protected void btnClearForm_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        protected void btnCancelEdit_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            hfCourseID.Value = "0";
            txtCourseName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtContent.Text = string.Empty;
            lblCurrentThumbnail.Visible = false;
            lblCurrentResource.Visible = false;

            lblFormTitle.Text = "Add New Course";
            btnCancelEdit.Visible = false;
            btnSaveCourse.Text = "Save Course";
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