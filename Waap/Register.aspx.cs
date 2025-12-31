using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;

namespace LearnSphere
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Initialize page if needed
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    // Get form values
                    string fullName = txtFullName.Text.Trim();
                    string email = txtEmail.Text.Trim().ToLower();
                    string password = txtPassword.Text;
                    string role = ddlRole.SelectedValue;

                    // Check if email already exists
                    if (EmailExists(email))
                    {
                        ShowMessage("This email is already registered. Please use a different email or login.", "error");
                        return;
                    }

                    // Insert user into database
                    if (InsertUser(fullName, email, password, role))
                    {
                        ShowMessage("Registration successful! Redirecting to login page...", "success");

                        // Redirect to login page after 2 seconds
                        Response.AddHeader("REFRESH", "2;URL=Login.aspx");
                    }
                    else
                    {
                        ShowMessage("Registration failed. Please try again.", "error");
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage("An error occurred: " + ex.Message, "error");
                }
            }
        }

        private bool EmailExists(string email)
        {
            string connString = ConfigurationManager.ConnectionStrings["LearnSphereDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);

                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();

                    return count > 0;
                }
            }
        }

        private bool InsertUser(string fullName, string email, string password, string role)
        {
            string connString = ConfigurationManager.ConnectionStrings["LearnSphereDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = @"INSERT INTO Users (FullName, Email, Password, Role) 
                                VALUES (@FullName, @Email, @Password, @Role)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@FullName", fullName);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@Role", role);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    return rowsAffected > 0;
                }
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