using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Xml;

namespace SDC
{
    public partial class SDCFormHTML : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //1. get html content, 
            //2. get xml content, 
            //3. write htmlcontent in the content area, write xml content in rawxml
            string formid = Request.QueryString["package"];
            string manager = Request.QueryString["manager"];
            string url = "";

            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                string retrieve_from = "";
                string submit_to = "";
                string transform_path = "";
                string name_space = "";
                string submit_action = "";
                string manager_name = "";
                bool scriptSubmit = true;

                SqlCommand cmd = new SqlCommand("select name, retrieve_endpoint, formlist_endpoint, transform from sdc_managers where id=" + manager);
                cmd.Connection = con;
                DataTable dt = new DataTable();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(dt);
                if (dt.Rows.Count == 1)
                {
                    url = dt.Rows[0]["retrieve_endpoint"].ToString();
                }
            }

            string html = getForm(formid, url, "html");
            
            XmlDocument xdoc = new XmlDocument();
            XmlNamespaceManager mgr = new XmlNamespaceManager(xdoc.NameTable);
            mgr.AddNamespace("urn", "urn:ihe:iti:rfd:2007");
            mgr.AddNamespace("sdc", "urn:ihe:qrph:sdc:2016");
            mgr.AddNamespace("soapenv", "http://www.w3.org/2003/05/soap-envelope");
            mgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            mgr.AddNamespace("def", "");
            xdoc.LoadXml(html);
            XmlNode htmlnode = xdoc.SelectSingleNode("//sdc:HTMLPackage", mgr);
            if (htmlnode != null)
            {
                string encodedString = htmlnode.InnerText;

                content.InnerHtml = DecodeBase64(encodedString);
                content.Style.Add("display", "block");
                content.Style.Add("Color", "black");

                return;
            }

          
            
        }

        public static string getForm(string formid, string endpoint, string format)
        {

            string data = createRetrieveFormRequestSoap2(formid, format);
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

                    return "error";


                }
            }
        }

        public static string createRetrieveFormRequestSoap2(string formid, string format)
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
                                            <urn:encodedResponse responseContentType='application/" + format + @"+sdc'>true</urn:encodedResponse >
                                            <urn:archiveURL>?</urn:archiveURL>
                                            <urn:context></urn:context>
                                            <urn:instanceID>?</urn:instanceID>
                                       </urn:workflowData>
                                    </urn:RetrieveFormRequest>
                                </soap:Body>
                            </soap:Envelope>";
        }

        protected string DecodeBase64(string encoded)
        {
            byte[] data = Convert.FromBase64String(encoded);
            string decodedString = Encoding.UTF8.GetString(data);
            return decodedString;
        }
    }
}