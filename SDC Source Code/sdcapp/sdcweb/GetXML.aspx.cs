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
    public partial class GetXML : System.Web.UI.Page
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
                        rawxml.Value = xml;

                        //validate xml
                        XmlDocument doc = new XmlDocument();
                        msg.Text = "";
                        try
                        {
                            doc.LoadXml(rawxml.Value);
                            msg.Text = "XML is valid.";
                            msg.ForeColor = System.Drawing.Color.Black;

                        }
                        catch (Exception ex)
                        {
                            msg.Text = ex.Message;
                            msg.ForeColor = System.Drawing.Color.Red;
                        }
                       
                    }

                }
            }
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            //update xml
            string packageid = Request.QueryString["packageid"];
            //make sure it is a valid xml before saving it
            XmlDocument doc = new XmlDocument();
            msg.Text = "";
            try
            {
                doc.LoadXml(rawxml.Value);
                UpdateForm(rawxml.Value, packageid);
                msg.Text = "Data Saved!";
                msg.ForeColor = System.Drawing.Color.Black;
                
            }
            catch(Exception ex)
            {
                msg.Text = ex.Message;
                msg.ForeColor = System.Drawing.Color.Red;
            }
           
        }

        private void UpdateForm(string xml, string packageid)
        {
            //convert binary to string
            string data = xml;
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(@"update SDC_PACKAGES set  
                                            package_content = @package_content, 
                                            date_updated = @date_updated 
                                            where package_id = @package_id");

                cmd.Parameters.AddWithValue("package_id", packageid);
                cmd.Parameters.AddWithValue("date_updated", DateTime.Now);
                cmd.Parameters.AddWithValue("package_content", data);
                
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();


            }
        }
    }
}