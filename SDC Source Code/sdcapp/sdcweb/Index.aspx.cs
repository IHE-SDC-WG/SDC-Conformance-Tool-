using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Xml;

namespace SDC
{
    public partial class GetForms : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //int CurrentPage;
            //forms.DataBind();
                forms.DataSource = GetPackageData();
                forms.DataBind();
            
        }

      
        private void DeleteForm(string formid)
        {
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("delete from sdc_packages where package_id = @package_id");
                cmd.Connection = con;
                con.Open();
                cmd.Parameters.AddWithValue("package_id", formid);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

       
        protected void update_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("update sdc_endpoints set form_receiver = '" + Request.Form["receiver"] + "', form_manager = '" + Request.Form["manager"] + "'");
                cmd.Connection = con;
                con.Open();               
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

       
        protected void grdPackages_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

        }

       
        protected void grdPackages_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void forms_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType==DataControlRowType.DataRow)
            {
                //e.Row.cells
            }
        }

        protected DataTable GetPackageData()
        {
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select * from sdc_packages");
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
                dt.Columns.Add("Encoded_ID");

                foreach(DataRow dr in dt.Rows)
                {
                    string xml = dr["package_content"].ToString();
                    dr["Encoded_ID"] = Server.UrlEncode(dr["Package_ID"].ToString());
                    try
                    {
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
                    catch(Exception ex)
                    {
                        dr["FORM_ID"] = "Error parsing XML. Please click on XML link to view detailed error.";
                        dr["FORM_NAME"] = "Error parsing XML. Please click on XML link to view detailed error.";
                    }
                    
                    
                    
                    
                }
                dt.AcceptChanges();
                
                return dt;
            }
        }

        protected void forms_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            forms.PageIndex = e.NewPageIndex;
            //Bind grid
            forms.DataBind();
        }

        protected void DeleteButton_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton btn = (ImageButton)sender;

            DeleteForm(btn.CommandArgument);
            forms.DataBind();
           
        }

       

       
    }   
   
}