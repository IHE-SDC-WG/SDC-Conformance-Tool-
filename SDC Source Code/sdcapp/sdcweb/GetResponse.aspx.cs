using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SDC
{
    public partial class GetResponse : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ClearHeaders();
            Response.AddHeader("Content-Type", "text/xml");
            Response.Write(@"<?xml version=""1.0"" encoding=""ISO-8859-1""?><response><name>john doe</name><name>jane doe</name></response>");
            Response.End();
            return;
            
            
        }

        

    }
}