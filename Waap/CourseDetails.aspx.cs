using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI;

namespace LearnSphere.Learner
{
    public partial class CourseDetails : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["LearnSphereDB"].ConnectionString;
        private int courseId = 0;
        private string resourcePath = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Get CourseID from query string
                if (Request.QueryString["CourseID"] != null && int.TryParse(Request.QueryString["CourseID"], out courseId))
                {
                    LoadCourseDetails();
                }
                else
                {
                    ShowNotFound();
                }
            }
        }

        private void LoadCourseDetails()
        {
            int studentId = Session["UserID"] != null ? Convert.ToInt32(Session["UserID"]) : 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT 
                                    c.CourseID,
                                    c.CourseName,
                                    c.Description,
                                    c.Content,
                                    c.CreatedDate,
                                    c.ThumbnailPath,
                                    c.ResourcePath,
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
                                WHERE c.CourseID = @CourseID
                                GROUP BY c.CourseID, c.CourseName, c.Description, c.Content, c.CreatedDate, c.ThumbnailPath, c.ResourcePath, u.FullName";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@CourseID", courseId);
                cmd.Parameters.AddWithValue("@StudentID", studentId);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // Set course details
                        lblCourseName.Text = reader["CourseName"].ToString();
                        lblDescription.Text = reader["Description"].ToString();
                        lblContent.Text = reader["Content"].ToString();
                        lblInstructor.Text = reader["InstructorName"].ToString();
                        lblEnrollmentCount.Text = reader["EnrollmentCount"].ToString();
                        lblCreatedDate.Text = Convert.ToDateTime(reader["CreatedDate"]).ToString("MMMM dd, yyyy");

                        bool isEnrolled = Convert.ToBoolean(reader["IsEnrolled"]);

                        pnlEnrollButton.Visible = !isEnrolled;
                        pnlEnrolledBadge.Visible = isEnrolled;

                        // Thumbnail logic
                        string thumbnail = reader["ThumbnailPath"] != DBNull.Value ? reader["ThumbnailPath"].ToString() : "";
                        if (!string.IsNullOrEmpty(thumbnail))
                        {
                            imgCourseThumbnail.ImageUrl = "~/" + thumbnail;
                            imgCourseThumbnail.Visible = true;
                            thumbnailPlaceholder.Visible = false;
                        }
                        else
                        {
                            imgCourseThumbnail.Visible = false;
                            thumbnailPlaceholder.Visible = true;
                        }

                        // Resource Download Section (Only for enrolled students)
                        resourcePath = reader["ResourcePath"] != DBNull.Value ? reader["ResourcePath"].ToString() : "";

                        if (isEnrolled)
                        {
                            if (!string.IsNullOrEmpty(resourcePath))
                            {
                                // Show download section
                                pnlResourceDownload.Visible = true;
                                pnlNoResource.Visible = false;

                                // Store resource path in ViewState for download
                                ViewState["ResourcePath"] = resourcePath;

                                // Display resource file name
                                string fileName = Path.GetFileName(resourcePath);
                                lblResourceName.Text = "File: " + fileName;
                            }
                            else
                            {
                                // Show no resource message
                                pnlResourceDownload.Visible = false;
                                pnlNoResource.Visible = true;
                            }
                        }
                        else
                        {
                            // Not enrolled - hide both panels
                            pnlResourceDownload.Visible = false;
                            pnlNoResource.Visible = false;
                        }

                        pnlCourseDetails.Visible = true;
                        pnlNotFound.Visible = false;
                    }
                    else
                    {
                        ShowNotFound();
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    ShowMessage("Error loading course details: " + ex.Message, false);
                    ShowNotFound();
                }
            }
        }

        protected void btnEnroll_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["CourseID"] == null)
            {
                ShowMessage("Invalid course ID.", false);
                return;
            }

            int courseId = Convert.ToInt32(Request.QueryString["CourseID"]);

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
                        LoadCourseDetails();
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
                        ShowMessage("🎉 Successfully enrolled in the course! You can now access all course materials.", true);
                        LoadCourseDetails();
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

        protected void btnDownloadResource_Click(object sender, EventArgs e)
        {
            // Check if user is logged in
            if (Session["UserID"] == null)
            {
                ShowMessage("Please login to download resources.", false);
                return;
            }

            // Get resource path from ViewState
            string resourcePath = ViewState["ResourcePath"] != null ? ViewState["ResourcePath"].ToString() : "";

            if (string.IsNullOrEmpty(resourcePath))
            {
                ShowMessage("No resource file available for download.", false);
                return;
            }

            // Get the physical path
            string filePath = Server.MapPath("~/" + resourcePath);

            // Check if file exists
            if (!File.Exists(filePath))
            {
                ShowMessage("Resource file not found on server. Please contact the instructor.", false);
                return;
            }

            try
            {
                // Get file info
                FileInfo fileInfo = new FileInfo(filePath);
                string fileName = fileInfo.Name;

                // Clear response
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();

                // Set content type based on file extension
                Response.ContentType = GetContentType(fileName);

                // Force download
                Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                Response.AddHeader("Content-Length", fileInfo.Length.ToString());

                // Write file to response
                Response.TransmitFile(filePath);
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                ShowMessage("Error downloading file: " + ex.Message, false);
            }
        }

        private string GetContentType(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLower();

            switch (extension)
            {
                case ".pdf":
                    return "application/pdf";
                case ".doc":
                    return "application/msword";
                case ".docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case ".xls":
                    return "application/vnd.ms-excel";
                case ".xlsx":
                    return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                case ".ppt":
                    return "application/vnd.ms-powerpoint";
                case ".pptx":
                    return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
                case ".zip":
                    return "application/zip";
                case ".txt":
                    return "text/plain";
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                default:
                    return "application/octet-stream";
            }
        }

        private void ShowNotFound()
        {
            pnlCourseDetails.Visible = false;
            pnlNotFound.Visible = true;
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            pnlMessage.Visible = true;
            lblMessage.Text = message;
            pnlMessage.CssClass = isSuccess ? "alert alert-success" : "alert alert-danger";
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }
    }
}