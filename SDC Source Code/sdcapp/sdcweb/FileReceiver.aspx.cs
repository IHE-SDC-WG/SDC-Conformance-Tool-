using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Specialized;
using System.Data.SqlClient;

namespace SDC
{
    public partial class FileReceiver : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpFileCollection  files = Request.Files;

           
            for (int i = 0; i < files.Count; i++ )
            {
                System.IO.Stream rdr = files[i].InputStream;

                //read file content to write to DB
                byte[] fileData = new byte[files[i].ContentLength];
                rdr.Read(fileData, 0, files[i].ContentLength);
                string packageid = Request.Form["packageid"].ToString();
                string packagename = Request.Form["packagename"];

                if(isPackageImported(packageid))
                {
                    Response.Write("This form is already uploaded. Please delete the form first.");
                    return;
                }

                InsertForm(fileData, packageid, packagename);
                //files[i].SaveAs(Server.MapPath("App_Data") + "\\" + files[i].FileName);
            }

            // If there are any form variables, get them here:
            //string summary = string.Empty;
            //summary += "<p>Form Variables:</p><ol>";

            //Load Form variables into NameValueCollection variable.
            //NameValueCollection coll = Request.Form;

            //foreach(string key in Request.Form )
            //{
            //    summary += "<li>" + key + ": " + Request.Form[key] + "</li>";
            //}
           
            //summary += "</ol>";

            //Response.Write(summary);

            Response.Write("Form-ID:" + Request.Form["packageid"] + " was uploaded.");
            //Response.Redirect("GetForms.aspx",true);
        }

        private void InsertForm(byte[] filedata, string packageid, string packagename)
        {
            //convert binary to string
            string data = System.Text.Encoding.Default.GetString(filedata);
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand( "insert into SDC_PACKAGES(package_id, package_name, package_content, date_updated) values (@package_id, @package_name, @package_content, @date_updated)");
                cmd.Parameters.AddWithValue("package_id", packageid);
                cmd.Parameters.AddWithValue("package_name", packagename);
                cmd.Parameters.AddWithValue("date_updated", DateTime.Now);
                //cmd.Parameters.AddWithValue("loaded_by", "");
                cmd.Parameters.AddWithValue("package_content", data);
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();


            }
        }

        private void UpdateForm(byte[] filedata, string formid, string formname)
        {
            //convert binary to string
            string data = System.Text.Encoding.Default.GetString(filedata);
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(@"update SDC_FORMS set  
                                            form_content = @form_content, 
                                            form_name = @form_name, 
                                            loaded_by = @loaded_by,
                                            date_uploaded = @date_uploaded 
                                            where form_id = @form_id");

                cmd.Parameters.AddWithValue("form_id", formid);
                cmd.Parameters.AddWithValue("form_name", formname);
                cmd.Parameters.AddWithValue("date_updated", DateTime.Now);
                cmd.Parameters.AddWithValue("form_content", data);
                cmd.Parameters.AddWithValue("loaded_by", "");
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();


            }
        }

        private void DeletePackage(string packageid)
        {
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("delete from sdc_packages where package_id = @package_id");
                cmd.Connection = con;
                con.Open();
                cmd.Parameters.Add("package_id", packageid);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        private bool isPackageImported(string package)
        {
             using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand( "select count(*) from SDC_PACKAGES where package_id = @id");
                cmd.Parameters.AddWithValue("id", package);
                cmd.Connection = con;
                con.Open();
                int retval = int.Parse(cmd.ExecuteScalar().ToString());
                con.Close();
                if(retval>0)
                {
                    return true;
                }
                return false;
            }
        }
    }
}