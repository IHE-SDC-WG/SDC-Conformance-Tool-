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
    public partial class NewIndex : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadPackages();
        
        }

        protected void LoadPackages()
        {
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select package_id, package_name from sdc_packages");
                cmd.Connection = con;
                DataTable dt = new DataTable();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(dt);
                

                foreach(DataRow dr in dt.Rows)
                {
                    Label package = new Label();
                    package.Text = dr["package_id"].ToString() + " - " + dr["package_name"].ToString();
                    packages.Controls.Add(package);

                    SqlCommand cmd1 = new SqlCommand("select package_id, sdc_forms.form_id, form_name, form_type from sdc_package_forms join sdc_forms on sdc_package_forms.form_id = sdc_forms.form_id where package_id = '" + dr["package_id"].ToString() + "'");
                    cmd1.Connection = con;
                    DataTable dt1 = new DataTable();
                    SqlDataAdapter ad1 = new SqlDataAdapter(cmd1);
                    ad1.Fill(dt1);

                    Table tblForms = new Table();
                    TableRow tr;
                    TableCell tc;
                    foreach(DataRow dr1 in dt1.Rows)
                    {                        
                        tr = new TableRow();
                        tc = new TableCell();
                        tc.Text = dr1["form_id"].ToString();
                        tr.Cells.Add(tc);

                        tc = new TableCell();
                        tc.Text = dr1["form_name"].ToString();
                        tr.Cells.Add(tc);

                        tc = new TableCell();
                        tc.Text = dr1["form_type"].ToString();
                        tr.Cells.Add(tc);

                        LinkButton remove = new LinkButton();
                        remove.Text = "Remove";
                        remove.ID = "lnk" + dr1["form_id"].ToString();
                        //remove.Attributes.Add("OnClick", "confirm('Delete " + dr1["form_id"].ToString() + "?')");
                        remove.Click += remove_Click;
                        
                        tc = new TableCell();
                        tc.Controls.Add(remove);
                        tr.Cells.Add(tc);
                        tblForms.Rows.Add(tr);
                    }

                    packages.Controls.Add(tblForms);
                }
            }
        }

        void remove_Click(object sender, EventArgs e)
        {
            var button = sender as LinkButton;
            string form_id = button.ID.Substring(3);
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("delete from sdc_package_forms where form_id '" + form_id + "'");

                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            
        }

        protected void Unnamed1_RowDeleted(object sender, GridViewDeletedEventArgs e)
        {
           
        }
        private void DeleteForm(string formid)
        {
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("delete from sdc_forms where form_id = @form_id");
                cmd.Connection = con;
                con.Open();
                cmd.Parameters.AddWithValue("form_id", formid);
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        protected void Unnamed1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
           
                DeleteForm(e.Keys[0].ToString());            
                forms.DataBind();
                e.Cancel=true;
                
            
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
               
        

        
    }   
    
}