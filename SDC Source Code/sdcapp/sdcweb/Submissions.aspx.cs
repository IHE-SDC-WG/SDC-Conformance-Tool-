using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace SDC
{
    public partial class Submissions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GridView1.DataSource = GetSubmissionData();
            GridView1.DataBind();
        }

        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected DataTable GetSubmissionData()
        {
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select * from sdc_submits order by submit_time desc");
                cmd.Connection = con;
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                XmlDocument xdoc = new XmlDocument();
                XmlNamespaceManager mgr = new XmlNamespaceManager(xdoc.NameTable);

                mgr.AddNamespace("urn", "urn:ihe:iti:rfd:2007");
                mgr.AddNamespace("sdc", "urn:ihe:qrph:sdc:2016");
                mgr.AddNamespace("soapenv", "http://www.w3.org/2003/05/soap-envelope");
                mgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                mgr.AddNamespace("def", "");

                dt.Columns.Add("FORM_ID");
                dt.Columns.Add("FORM_NAME");
                foreach (DataRow dr in dt.Rows)
                {
                    string xml = dr["submit_form"].ToString();
                    xdoc.LoadXml(xml);

                    XmlNode xNode = xdoc.SelectSingleNode("//sdc:FormDesign/@ID", mgr);
                    if (xNode != null)
                    {
                        dr["FORM_ID"] = xNode.InnerText;
                    }
                    xNode = xdoc.SelectSingleNode("//sdc:Header/@title", mgr);
                    if (xNode != null)
                    {
                        dr["FORM_NAME"] = xNode.InnerText;
                    }

                }
                dt.AcceptChanges();

                return dt;
            }

            
        }

        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridView1.PageIndex = e.NewPageIndex;
            //Bind grid
            GridView1.DataBind();
        }

    }
}