using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

namespace LearnSphere
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Check if user is already logged in
                if (Session["UserID"] != null)
                {
                    RedirectToDashboard();
                }

                // Check for remember me cookie
                if (Request.Cookies["LearnSphereEmail"] != null)
                {
                    txtEmail.Text = Request.Cookies["LearnSphereEmail"].Value;
                    chkRememberMe.Checked = true;
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    string email = txtEmail.Text.Trim().ToLower();
                    string password = txtPassword.Text;

                    // Authenticate user
                    DataRow user = AuthenticateUser(email, password);

                    if (user != null)
                    {
                        // Store user information in session
                        Session["UserID"] = user["UserID"];
                        Session["FullName"] = user["FullName"];
                        Session["Email"] = user["Email"];
                        Session["Role"] = user["Role"];

                        // Handle remember me
                        if (chkRememberMe.Checked)
                        {
                            HttpCookie emailCookie = new HttpCookie("LearnSphereEmail");
                            emailCookie.Value = email;
                            emailCookie.Expires = DateTime.Now.AddDays(30);
                            Response.Cookies.Add(emailCookie);
                        }
                        else
                        {
                            // Remove cookie if unchecked
                            if (Request.Cookies["LearnSphereEmail"] != null)
                            {
                                HttpCookie emailCookie = new HttpCookie("LearnSphereEmail");
                                emailCookie.Expires = DateTime.Now.AddDays(-1);
                                Response.Cookies.Add(emailCookie);
                            }
                        }

                        // Redirect to appropriate dashboard
                        RedirectToDashboard();
                    }
                    else
                    {
                        ShowMessage("Invalid email or password. Please try again.", "error");
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage("An error occurred: " + ex.Message, "error");
                }
            }
        }

        private DataRow AuthenticateUser(string email, string password)
        {
            string connString = ConfigurationManager.ConnectionStrings["LearnSphereDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"SELECT UserID, FullName, Email, Role 
                               FROM Users 
                               WHERE Email = @Email AND Password = @Password";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        return dt.Rows[0];
                    }

                    return null;
                }
            }
        }

        private void RedirectToDashboard()
        {
            string role = Session["Role"]?.ToString();

            if (role == "Admin")
            {
                Response.Redirect("Admin/AdminDashboard.aspx");
            }
            else if (role == "Instructor")
            {
                Response.Redirect("Instructor/InstructorDashboard.aspx");
            }
            else if (role == "Learner")
            {
                Response.Redirect("Dashboard.aspx");
            }
            else
            {
                Response.Redirect("Dashboard.aspx");
            }
        }

        private void ShowMessage(string message, string type)
        {
            pnlMessage.Visible = true;
            lblMessage.Text = message;

            if (type == "success")
            {
                pnlMessage.CssClass = "message success";
            }
            else
            {
                pnlMessage.CssClass = "message error";
            }
        }
    }
}