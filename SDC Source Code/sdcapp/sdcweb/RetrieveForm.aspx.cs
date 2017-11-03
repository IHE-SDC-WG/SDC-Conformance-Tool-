using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Net;
using System.IO;
using System.Text;


namespace SDC
{
    public partial class RetrieveForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        [WebMethod()]
        public static string getxmlpackage(string formid, string url)
        {
            return getForm(formid, url, "xml");
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
    }
}