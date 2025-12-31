using System;
using System.Web.UI;

namespace LearnSphere
{
    public partial class Site : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // This runs every time a page using this master page is loaded
            if (!IsPostBack)
            {
                // Any code that needs to run only once per page load
            }
        }
    }
}
