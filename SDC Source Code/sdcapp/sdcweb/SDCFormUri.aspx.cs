using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.IO;
using System.Text;
using System.Xml;

namespace SDC
{
    public partial class GetFormUri : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string formid = Request.Params["packageid"];
            string manager = Request.Params["manager"];
            string prepop = Request.Params["prepop"];
            string url = "";
            string uri = "";

            
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                

                SqlCommand cmd = new SqlCommand("select name, retrieve_endpoint, formlist_endpoint, transform from sdc_managers where id=" + manager);
                cmd.Connection = con;
                DataTable dt = new DataTable();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(dt);
                if (dt.Rows.Count == 1)
                {
                    url = dt.Rows[0]["retrieve_endpoint"].ToString();
                }
                string xml = getUri(formid, url, "urn", prepop);
                XmlDocument xdoc = new XmlDocument();
                XmlNamespaceManager mgr = new XmlNamespaceManager(xdoc.NameTable);
                mgr.AddNamespace("urn", "urn:ihe:iti:rfd:2007");
                mgr.AddNamespace("sdc", "urn:ihe:qrph:sdc:2016");
                mgr.AddNamespace("soapenv", "http://www.w3.org/2003/05/soap-envelope");
                mgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                mgr.AddNamespace("def", "");
                xdoc.LoadXml(xml);

                XmlNode urlnode = xdoc.SelectSingleNode("//urn:URL", mgr);

                

                if (urlnode != null)
                {                    
                    uri = urlnode.InnerText;
                    uri = Server.UrlDecode(uri);
                    //Response.Write("Visit: <a href='" + uri + "'>" + uri + "</a>");
                }
                else
                {
                    Page.Controls.Add(new LiteralControl("<textarea cols=120 rows=10>" + xml + "</textarea>"));
                   
                }
                  
                

            }

            Response.Write("Visit: <a href='" + uri + "'>" + uri + "</a>");
        }

        public static string createRetrieveFormRequestSoap2(string formid, string format, string prepop)
        {
            //prepop = "";
            return @"<?xml version='1.0' encoding='utf - 8'?>
                            <soap:Envelope xmlns:soap = 'http://www.w3.org/2003/05/soap-envelope' 
                                           xmlns:urn = 'urn:ihe:iti:rfd:2007'>
                            <soap:Header/>
                                <soap:Body>
                                    <urn:RetrieveFormRequest>
                                        <urn:prepopData>" + prepop +
                                        @"</urn:prepopData>
                                        <urn:workflowData>
                                            <urn:formID>" + formid + @"</urn:formID >
                                            <urn:encodedResponse>false</urn:encodedResponse >
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

        public string getPackageRequestMessage(string formid)
        {
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {


                SqlCommand cmd = new SqlCommand("select formrequest from sdc_packages where package_id = '" + formid + "'");
                cmd.Connection = con;
                DataTable dt = new DataTable();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(dt);
                if (dt.Rows.Count == 1)
                {
                   if(dt.Rows[0][0]!=System.DBNull.Value)
                   {
                       return dt.Rows[0][0].ToString();
                   }
                }
                return "";
            }
        }

        public  string getUri(string formid, string endpoint, string format, string prepop)
        {
            string data = "";

            //if request is saved in the table just get that and send
            //data = getPackageRequestMessage(formid);

           if (data=="")
            {
                data = createRetrieveFormRequestSoap2(formid, format, prepop);
            }

            //write data to file
            try
            {
                System.IO.StreamWriter wr = new StreamWriter(Server.MapPath(@"Logs\RetrieveRequest.xml"));
                wr.Write(data);
                wr.Close();
            }
            catch (Exception ex)
            {
                Response.Write("Error writing log: " + ex.Message);

            }

            HttpWebRequest req = WebRequest.Create(endpoint) as HttpWebRequest;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
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
    }
}