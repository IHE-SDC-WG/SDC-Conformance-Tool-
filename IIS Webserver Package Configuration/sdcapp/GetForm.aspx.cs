using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SDC
{
    public partial class GetForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                DataTable dt = GetFormManagers();

                lstManagers.DataSource = dt;
                lstManagers.DataTextField = "Name";
                lstManagers.DataValueField = "ID";
                lstManagers.DataBind();

                foreach (ListItem item in lstManagers.Items)
                {
                    string formlist = GetFieldValue("Formlist_endpoint", int.Parse(item.Value), dt);
                    string retrieve = GetFieldValue("Retrieve_endpoint", int.Parse(item.Value), dt);
                    item.Attributes.Add("Title", formlist + "|" + retrievelist);
                }
            
                DataTable dtReceivers = GetFormReceivers();


                lstReceivers.DataSource = dtReceivers;
                lstReceivers.DataTextField = "Name";
                lstReceivers.DataValueField = "id";
                lstReceivers.DataBind();

                foreach (ListItem item in lstReceivers.Items)
                {
                    string submit = GetFieldValue("submit_endpoint", int.Parse(item.Value), dtReceivers);
                    item.Attributes.Add("Title", submit);

                }

               
            }
           
            
        }

      private string GetFieldValue(string FieldName, int ID, DataTable dt)
        {
            return dt.Select("ID=" + ID)[0][FieldName].ToString();

        }

       private DataTable GetFormManagers()
        {
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select ID, Name, Retrieve_endpoint, coalesce(formlist_endpoint,'') as formlist_endpoint, formlist_endpoint + '|' + retrieve_endpoint AS endpoints from sdc_managers", con);
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                ad.Fill(dt);

                return dt;

            }
        }

        private DataTable GetFormReceivers()
       {
           using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
           {
               SqlCommand cmd = new SqlCommand("select * from sdc_receivers", con);
               SqlDataAdapter ad = new SqlDataAdapter(cmd);
               DataTable dt = new DataTable();
               ad.Fill(dt);

               return dt;

           }
       }

        //private DataTable GetPackages()
        //{

        //}



        public string GetHtml(string transform_path, string xml)
        {
            string xsltPath = Server.MapPath(transform_path) + "\\sdctemplate.xslt";
            string csspath = Server.MapPath(transform_path) + "\\sdctemplate.css";
            //3/10/2016 - change encoding to unicode 
            System.IO.MemoryStream stream = new System.IO.MemoryStream(System.Text.UnicodeEncoding.ASCII.GetBytes(xml));
            System.Xml.XPath.XPathDocument document = new System.Xml.XPath.XPathDocument(stream);
            System.IO.StringWriter writer = new System.IO.StringWriter();
            System.Xml.Xsl.XslCompiledTransform transform = new System.Xml.Xsl.XslCompiledTransform();

            System.Xml.Xsl.XsltSettings settings = new System.Xml.Xsl.XsltSettings(true, true);

            System.Xml.XmlSecureResolver resolver = new System.Xml.XmlSecureResolver(new System.Xml.XmlUrlResolver(), csspath);
            transform.Load(xsltPath, settings, resolver);
            transform.Transform(document, null, writer);
            return writer.ToString();
        }

        public string createRetrieveFormRequestSoap(string formid)
        {
            return @"<?xml version='1.0' encoding='utf - 8'?>
                            <soap:Envelope xmlns:soap = 'http://www.w3.org/2003/05/soap-envelope' 
                                           xmlns:urn = 'urn:ihe:iti:rfd:2007'>
                            <soap:Header/>
                                <soap:Body>
                                    <urn:RetrieveFormRequest>
                                        <urn:prepopData>
                                        </urn:prepopData>
                                        <urn:workflowData>
                                            <urn:formID>" + formid + @"</urn:formID >
                                            <urn:encodedResponse>true</urn:encodedResponse >
                                            <urn:archiveURL>?</urn:archiveURL>
                                            <urn:context></urn:context>
                                            <urn:instanceID>?</urn:instanceID>
                                       </urn:workflowData>
                                    </urn:RetrieveFormRequest>
                                </soap:Body>
                            </soap:Envelope>";
        }

        public string getForm(string formid, string endpoint)
        {
            
            string data = createRetrieveFormRequestSoap(formid);
            HttpWebRequest req = WebRequest.Create(endpoint) as HttpWebRequest;
            byte[] postData = Encoding.UTF8.GetBytes(data);
            string result;
            if (null != req)
            {
                req.Method = "POST";
                req.ContentType = "application/soap+xml; charset=utf-8";
                req.ContentLength = postData.Length;
                using (Stream stream = req.GetRequestStream())
                {
                    stream.Write(postData, 0, postData.Length);
                    stream.Flush();
                    stream.Close();
                }
            }

            try
            {
                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    result = reader.ReadToEnd();


                return result;
            }

            catch (WebException ex)
            {
                HttpWebResponse response = ex.Response as HttpWebResponse;
                if (null == response)
                    throw new ArgumentNullException("Response was null");
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string error = reader.ReadToEnd();
                    throw new Exception("Fault from RetrieveFormResponse: " + error);

                }
            }
        }

        protected void Select_Click(object sender, EventArgs e)
        {
            
        }

       

        protected void Unnamed_Click1(object sender, EventArgs e)
        {

        }

        protected void retrievelist_Click(object sender, EventArgs e)
        {

            DataTable dtManagers = GetFormManagers();
            DataTable dtReceivers = GetFormReceivers();

            if (lstManagers.SelectedValue.Length==0)
            {
                //packages.Controls.Clear();
                packages.Controls.Add(new LiteralControl("<p>Please select one of the Form Managers and one or more Form Receivers.</p>"));
                return;
            }
            int managerid = int.Parse(lstManagers.SelectedValue);
            //populate the hidden field

            formmanager.Value = managerid.ToString();

                     

            string selectedreceivers = "";
            foreach(ListItem itm in lstReceivers.Items)
            {
                if (itm.Selected)
                {
                    if (selectedreceivers.Length == 0)
                    {
                        selectedreceivers = lstReceivers.SelectedValue;  // GetFieldValue("submit_endpoint", int.Parse(itm.Value), dtReceivers);
                    }
                    else
                    {
                        selectedreceivers = selectedreceivers + "," + itm.Value;  // GetFieldValue("submit_endpoint", int.Parse(itm.Value), dtReceivers);
                    }
                }
            }

            //populate the hidden field
            receivers.Value = selectedreceivers;

            string formlisturl = GetFieldValue("formlist_endpoint", int.Parse(lstManagers.SelectedValue), dtManagers);
            if (formlisturl.Length == 0)
            {
                packages.Controls.Add(new LiteralControl("<p>Selected form manager does not provide endpoint to get packagelist. Please enter package id above and click Retrieve</p>"));
                packagelist.Items.Clear();
                divlist.Style["display"] = "none";
                packagelist.Visible = false;
                return;
            }
            else
            {
                //getpackage.Visible = false;
                //getpackage.Style["display"] = "none";
            }

            //get package list from selected form manager

            if(formlisturl.Length==0)
            {
                packages.InnerHtml = "Please select a form manager.";
                return;
            }
            else
            {
               

                string url = formlisturl;
                Uri myUri = new Uri(url, UriKind.Absolute);
                 try
                 {
                     string result = "";
                    
                     HttpWebRequest req = WebRequest.Create(myUri) as HttpWebRequest;
                    
                     HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                     
                     using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                         result = reader.ReadToEnd();
                    
                     JObject o = JObject.Parse(result);

                     var entry = o["entry"];

                     packagelist.Items.Clear();
                     foreach (var item in entry)
                     {

                         var id = item["item"]["id"].ToString();
                         var name = item["item"]["display"]["value"].ToString();                        

                         packagelist.Items.Add(new ListItem(id, id));

                     }

                     if (packagelist.Items.Count>0)
                     {
                         divlist.Style["display"] = "block";
                         packagelist.Visible = true;
                         packagelist.Attributes.Add("onchange", "setpackageid(this)");
                         //btnRetrieveSelected.Visible = true;
                     }
                     else
                     {
                         divlist.Style["display"] = "none";
                         packagelist.Visible = false;
                     }

                 }
                 catch (Exception ex)
                 {
                     packages.InnerHtml = "Error calling REST API: " + url + ", " + ex.Message;
                     packages.Style.Add("Color", "red");
                     
                 }
            }
        }

        protected void btnRetrierve_Click(object sender, EventArgs e)
        {
            string package = txtPackageid.Text;
            int managerid = int.Parse(lstManagers.SelectedValue);
            //populate the hidden field

            formmanager.Value = managerid.ToString();
            
                         
        }

        protected string DecodeBase64(string encoded)
        {
            byte[] data = Convert.FromBase64String(encoded);
            string decodedString = Encoding.UTF8.GetString(data);
            return decodedString;
        }

        protected void btnRetrieveSelected_Click(object sender, EventArgs e)
        {

        }

        }
    }
