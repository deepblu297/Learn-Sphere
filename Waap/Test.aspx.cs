using System;
using System.Data.SqlClient;
using System.Configuration;

namespace Waap
{
    public partial class Test : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string conStr = ConfigurationManager
                .ConnectionStrings["LearnSphereDB"]
                .ConnectionString;

            try
            {
                using (SqlConnection con = new SqlConnection(conStr))
                {
                    con.Open();
                    Response.Write("<h2 style='color:green'>Database Connected Successfully!</h2>");
                }
            }
            catch (Exception ex)
            {
                Response.Write("<h2 style='color:red'>Connection Failed</h2>");
                Response.Write("<br/>" + ex.Message);
            }
        }
    }
}

