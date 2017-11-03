using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

namespace SDC
{
    public partial class SubmissionGetXML : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string transaction_id = Request.QueryString["transaction_id"];
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("select submit_form from sdc_submits where transaction_id = '" + transaction_id + "'");
                    cmd.Connection = con;
                    SqlDataAdapter ad = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    ad.Fill(dt);
                    if (dt.Rows.Count == 1)
                    {
                        string xml = dt.Rows[0][0].ToString();
                        rawxml.Value = XmlHelper.FormatXML( xml);

                    }

                }
            }
        }
    }
}