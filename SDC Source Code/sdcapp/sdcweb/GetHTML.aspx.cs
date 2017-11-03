using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace SDC
{
    public partial class GetHTML : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string packageid = Request.QueryString["packageid"];
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand("select package_content from sdc_packages where package_id = '" + packageid + "'");
                    cmd.Connection = con;
                    SqlDataAdapter ad = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    ad.Fill(dt);
                    if (dt.Rows.Count == 1)
                    {
                        string xml = dt.Rows[0][0].ToString();

                        //update receiver end point
                        string receiver = GetReceiverEndpoint();
                        if (receiver == "")
                            receiver = "http://www.sharedapps.net/SDCReceiver/service1.asmx";                                               

                        rawxml.Value = xml;
                        content.InnerHtml = GetHtml(Server.MapPath("Transforms/ver3/sdctemplate.xslt"), xml).Replace("%receiver%", receiver);
                        
                    }

                }
            }
        }

        public string GetReceiverEndpoint()
        {
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select submit_endpoint from sdc_parameters");
                cmd.Connection = con;
                con.Open();
                string retval = cmd.ExecuteScalar() == null ? "" : cmd.ExecuteScalar().ToString();
                con.Close();
                return retval;
            }
        }

        public string GetHtml(string xsltPath, string xml)
        {           

            //3/10/2016 - change encoding to unicode 
            System.IO.MemoryStream stream = new System.IO.MemoryStream(System.Text.UnicodeEncoding.ASCII.GetBytes(xml));
            System.Xml.XPath.XPathDocument document = new System.Xml.XPath.XPathDocument(stream);
            System.IO.StringWriter writer = new System.IO.StringWriter();
            System.Xml.Xsl.XslCompiledTransform transform = new System.Xml.Xsl.XslCompiledTransform();

            System.Xml.Xsl.XsltSettings settings = new System.Xml.Xsl.XsltSettings(true, true);

            System.Xml.XmlSecureResolver resolver = new System.Xml.XmlSecureResolver(new System.Xml.XmlUrlResolver(), Server.MapPath("Transforms/ver3/sdctemplate.css"));
            transform.Load(xsltPath, settings, resolver);
            transform.Transform(document, null, writer);
            return writer.ToString();
        }
    }
}