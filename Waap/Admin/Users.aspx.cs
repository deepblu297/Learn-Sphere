using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LearnSphere.Admin
{
    public partial class Users : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if user is logged in and is admin
            if (Session["UserID"] == null || Session["Role"]?.ToString() != "Admin")
            {
                Response.Redirect("~/Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                LoadUsers();
            }
        }


        private void LoadUsers(string searchTerm = "")
        {
            string connString = ConfigurationManager.ConnectionStrings["LearnSphereDB"].ConnectionString;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                string query = "SELECT UserID, FullName, Email, Role FROM Users";

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    query += " WHERE FullName LIKE @SearchTerm OR Email LIKE @SearchTerm";
                }

                query += " ORDER BY UserID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrWhiteSpace(searchTerm))
                    {
                        cmd.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
                    }

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    gvUsers.DataSource = dt;
                    gvUsers.DataBind();
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            LoadUsers(searchTerm);
        }

        protected void btnClearSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            LoadUsers();
        }

        protected void gvUsers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvUsers.EditIndex = e.NewEditIndex;
            LoadUsers(txtSearch.Text.Trim());
        }

        protected void gvUsers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvUsers.EditIndex = -1;
            LoadUsers(txtSearch.Text.Trim());
        }

        protected void gvUsers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                int userId = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);
                GridViewRow row = gvUsers.Rows[e.RowIndex];

                string fullName = ((TextBox)row.Cells[1].Controls[0]).Text;
                string email = ((TextBox)row.Cells[2].Controls[0]).Text;
                string role = ((DropDownList)row.FindControl("ddlEditRole")).SelectedValue;

                string connString = ConfigurationManager.ConnectionStrings["LearnSphereDB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    string query = @"UPDATE Users 
                                   SET FullName = @FullName, 
                                       Email = @Email, 
                                       Role = @Role 
                                   WHERE UserID = @UserID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.Parameters.AddWithValue("@FullName", fullName);
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Role", role);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                gvUsers.EditIndex = -1;
                LoadUsers(txtSearch.Text.Trim());
                ShowMessage("User updated successfully!", "success");
            }
            catch (Exception ex)
            {
                ShowMessage("Error updating user: " + ex.Message, "error");
            }
        }

        protected void gvUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int userId = Convert.ToInt32(gvUsers.DataKeys[e.RowIndex].Value);

                // Prevent admin from deleting themselves
                if (userId.ToString() == Session["UserID"]?.ToString())
                {
                    ShowMessage("You cannot delete your own account!", "error");
                    return;
                }

                string connString = ConfigurationManager.ConnectionStrings["LearnSphereDB"].ConnectionString;

                using (SqlConnection conn = new SqlConnection(connString))
                {
                    conn.Open();

                    // First delete related enrollments
                    string deleteEnrollments = "DELETE FROM Enrollments WHERE UserID = @UserID";
                    using (SqlCommand cmd = new SqlCommand(deleteEnrollments, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.ExecuteNonQuery();
                    }

                    // Then delete the user
                    string deleteUser = "DELETE FROM Users WHERE UserID = @UserID";
                    using (SqlCommand cmd = new SqlCommand(deleteUser, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        cmd.ExecuteNonQuery();
                    }
                }

                LoadUsers(txtSearch.Text.Trim());
                ShowMessage("User deleted successfully!", "success");
            }
            catch (Exception ex)
            {
                ShowMessage("Error deleting user: " + ex.Message, "error");
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("../Login.aspx");
        }

        private void ShowMessage(string message, string type)
        {
            pnlMessage.Visible = true;
            lblMessage.Text = message;
            pnlMessage.CssClass = "message " + type;
        }
    }
}