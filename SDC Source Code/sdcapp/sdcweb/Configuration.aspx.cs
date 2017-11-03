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
    public partial class Configuration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindData();
                
            }
        }

        private void BindData()
        {
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select * from sdc_parameters;");
                cmd.Connection = con;
                DataTable dt = new DataTable();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(dt);
                parameters.DataSource = dt;
                parameters.DataBind();
            }
        }

        
        protected void AddNewParameter(object sender, EventArgs e)
        {
            
            string formlist = ((TextBox)parameters.FooterRow.FindControl("txtFormList")).Text;
            string retrieve = ((TextBox)parameters.FooterRow.FindControl("txtRetrieve")).Text;
            string submiturl = ((TextBox)parameters.FooterRow.FindControl("txtSubmit")).Text;
            string xslt = ((TextBox)parameters.FooterRow.FindControl("txtXSLT")).Text;
          

            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(@"insert into sdc_parameters(formlist_endpoint, Retrieve_endpoint, 
                                                Submit_endpoint, submit_form_action, submit_form_namespace, transform_path)
                                                values (@formlist, @retrieve, @submiturl, @submitaction, @submitnamespace, @xslt)"
                                                );

                cmd.Parameters.AddWithValue("formlist", formlist);
                cmd.Parameters.AddWithValue("retrieve", retrieve);
                cmd.Parameters.AddWithValue("submiturl", submiturl);
                cmd.Parameters.AddWithValue("submitaction", "SubmitFormRequest");
                cmd.Parameters.AddWithValue("submitnamespace", "urn:ihe:iti:rfd:2007");
                cmd.Parameters.AddWithValue("xslt", xslt);

                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            
            parameters.EditIndex = -1;
            BindData();
        }

        protected void DeleteParameter(object sender, EventArgs e)
        {
        
        }
        protected void EditParameter(object sender, GridViewEditEventArgs e)
        {
            parameters.EditIndex = e.NewEditIndex;
            BindData();
        }
        
        protected void UpdateParameter(object sender, GridViewUpdateEventArgs e)
        {

            string configid = ((Label)parameters.Rows[e.RowIndex].FindControl("lblConfigID")).Text;
            string formlist = ((TextBox)parameters.Rows[e.RowIndex].FindControl("txtFormList")).Text;
            string retrieve = ((TextBox)parameters.Rows[e.RowIndex].FindControl("txtRetrieve")).Text;
            string submiturl = ((TextBox)parameters.Rows[e.RowIndex].FindControl("txtSubmit")).Text;
            string xslt = ((TextBox)parameters.Rows[e.RowIndex].FindControl("txtXSLT")).Text;
            
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(@"update sdc_parameters set formlist_endpoint = @formlist,
                                                Retrieve_endpoint = @retrieve,
                                                Submit_endpoint=@submiturl,
                                                transform_path=@xslt where config_id=" + configid);
                
                cmd.Parameters.AddWithValue("formlist", formlist);
                cmd.Parameters.AddWithValue("retrieve", retrieve);
                cmd.Parameters.AddWithValue("submiturl", submiturl);
                cmd.Parameters.AddWithValue("xslt", xslt);

                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            e.Cancel = true;
            parameters.EditIndex = -1;
            BindData();
        }
        
        protected void OnPaging(object sender, GridViewPageEventArgs e)
        {
            BindData();
            parameters.PageIndex = e.NewPageIndex;
            parameters.DataBind();
        }

        protected void parameters_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string configid = ((Label)parameters.Rows[e.RowIndex].FindControl("lblConfigID")).Text;
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("delete from sdc_parameters where config_id=" + configid);
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            e.Cancel = true;
            parameters.EditIndex = -1;
            BindData();
        }

        protected void parameters_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            parameters.EditIndex = -1;
            BindData();
        }
    }
}