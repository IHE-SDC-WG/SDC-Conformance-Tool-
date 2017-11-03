using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SDC
{
    public partial class download : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "application/xml";
            Response.AppendHeader("Content-Disposition", "attachment; filename=result.xml");

            //can either use TransmitFile or plain Write
            Response.Write("<greeting>Hello</greeting>");
            //Response.TransmitFile(Server.MapPath("~/newforms/Breast_Invasive_M3.xml"));
            
            Response.End();
        }
    }
}