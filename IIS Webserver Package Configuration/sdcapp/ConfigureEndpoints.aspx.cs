using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SDC
{
    public partial class ConfigureEndpoints : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                BindManagerData();
                BindReceiverData();
            }
            
        }

        protected void formmanager_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string name = ((TextBox)formmanager.Rows[e.RowIndex].FindControl("txtFormManager")).Text;
            string retrieve = ((TextBox)formmanager.Rows[e.RowIndex].FindControl("txtRetrieve")).Text;
            string formlist = ((TextBox)formmanager.Rows[e.RowIndex].FindControl("txtFormList")).Text;
            string id = ((Label)formmanager.Rows[e.RowIndex].FindControl("lblID")).Text;

            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(@"update sdc_managers set name = @name, formlist_endpoint = @formlist,
                                                Retrieve_endpoint = @retrieve
                                                where id=" + id);

                cmd.Parameters.AddWithValue("name", name);
                cmd.Parameters.AddWithValue("formlist", formlist);
                cmd.Parameters.AddWithValue("retrieve", retrieve);
                

                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            e.Cancel = true;
            formmanager.EditIndex = -1;
            BindManagerData();
        }

        protected void formmanager_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            
            e.Cancel = true;
            formmanager.EditIndex = -1;
            BindManagerData();
        }

        protected void formmanager_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string id = ((Label)formmanager.Rows[e.RowIndex].FindControl("lblID")).Text;
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("delete from sdc_managers where id=" + id);
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            e.Cancel = true;
            formmanager.EditIndex = -1;
            BindManagerData();
        }

        private void BindManagerData()
        {
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select id, name, formlist_endpoint, retrieve_endpoint from sdc_managers union select null, null, null, null;");
                cmd.Connection = con;
                DataTable dt = new DataTable();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(dt);
                formmanager.DataSource = dt;
                formmanager.DataBind();
            }
        }

        private void BindReceiverData()
        {
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select id, name, submit_endpoint, script_submit from sdc_receivers union select null, null, null, 0;");
                cmd.Connection = con;
                DataTable dt = new DataTable();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(dt);
                formreceivers.DataSource = dt;
                formreceivers.DataBind();
            }
        }

        protected void AddNewManager(object sender, EventArgs e)
        {

            string name = ((TextBox)formmanager.FooterRow.FindControl("txtFormManager")).Text;
            string retrieve = ((TextBox)formmanager.FooterRow.FindControl("txtRetrieve")).Text;
            string formlist = ((TextBox)formmanager.FooterRow.FindControl("txtFormList")).Text;

           


            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(@"insert into sdc_managers(name, formlist_endpoint, Retrieve_endpoint)
                                                values (@name, @formlist, @retrieve)");

                cmd.Parameters.AddWithValue("name", name);
                cmd.Parameters.AddWithValue("formlist", formlist);
                cmd.Parameters.AddWithValue("retrieve", retrieve);              
                
                //cmd.Parameters.AddWithValue("submitnamespace", "urn:ihe:iti:rfd:2007");
                

                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            formmanager.EditIndex = -1;
            BindManagerData();
        }

        protected void formmanager_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
           
        }

        protected void formmanager_RowEditing(object sender, GridViewEditEventArgs e)
        {
            formmanager.EditIndex = e.NewEditIndex;
            BindManagerData();
        }

        protected void formmanager_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataRowView drview = e.Row.DataItem as DataRowView;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if(drview["id"]== System.DBNull.Value )
                {
                    e.Row.Visible = false;
                }
            }
        }

        protected void formreceivers_RowEditing(object sender, GridViewEditEventArgs e)
        {
            formreceivers.EditIndex = e.NewEditIndex;
            BindReceiverData();
        }

        protected void formreceivers_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string name = ((TextBox)formreceivers.Rows[e.RowIndex].FindControl("txtFormReceiver")).Text;
            string submit = ((TextBox)formreceivers.Rows[e.RowIndex].FindControl("txtSubmit")).Text;
            string id = ((Label)formreceivers.Rows[e.RowIndex].FindControl("lblID")).Text;
            bool scriptsubmit=((CheckBox)formreceivers.Rows[e.RowIndex].FindControl("chkCORS")).Checked;

            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(@"update sdc_receivers set name = @name, submit_endpoint = @submit, script_submit = @scriptsubmit
                                                where id=" + id);

                cmd.Parameters.AddWithValue("name", name);
                cmd.Parameters.AddWithValue("submit", submit);
                cmd.Parameters.AddWithValue("scriptsubmit", scriptsubmit);
               
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            e.Cancel = true;
            formreceivers.EditIndex = -1;
            BindReceiverData();
        }

        protected void formreceivers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            e.Cancel = true;
            formreceivers.EditIndex = -1;
            BindReceiverData();
        }

        protected void formreceivers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            string id = ((Label)formreceivers.Rows[e.RowIndex].FindControl("lblID")).Text;
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("delete from sdc_receivers where id=" + id);
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            e.Cancel = true;
            formreceivers.EditIndex = -1;
            BindReceiverData();
        }

        protected void formreceivers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            DataRowView drview = e.Row.DataItem as DataRowView;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (drview["id"] == System.DBNull.Value)
                {
                    e.Row.Visible = false;
                }
            }
        }

        protected void AddNewReceiver(object sender, EventArgs e)
        {

            string name = ((TextBox)formreceivers.FooterRow.FindControl("txtFormReceiver")).Text;
            string submit = ((TextBox)formreceivers.FooterRow.FindControl("txtSubmit")).Text;
            bool scriptsubmit = ((CheckBox)formreceivers.FooterRow.FindControl("chkCORS")).Checked;

            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(@"insert into sdc_receivers(name, submit_endpoint, script_submit)
                                                values (@name, @submit, @scriptsubmit)");

                cmd.Parameters.AddWithValue("name", name);
                cmd.Parameters.AddWithValue("submit", submit);
                cmd.Parameters.AddWithValue("scriptsubmit", scriptsubmit);

                //cmd.Parameters.AddWithValue("submitnamespace", "urn:ihe:iti:rfd:2007");


                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            formreceivers.EditIndex = -1;
            BindReceiverData();
        }
    }
}