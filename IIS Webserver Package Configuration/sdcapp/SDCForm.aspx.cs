using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;

namespace SDC
{
    public partial class SDCForm : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                LoadForm();
            }
           // else
            //{
             //   fillparameters();
            //}

        }

        //protected void fillparameters()
        //{
        //    string receivers = Request.QueryString["receivers"];
        //    string manager = Request.QueryString["manager"];
        //    string package_id = Request.QueryString["package"];
        //    string receivernames = "";
        //     //get end points
        //    using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
        //    {
        //        string retrieve_from = "";
        //        string submit_to = "";
        //        string transform_path = "";
        //        string name_space = "";
        //        string submit_action = "";
        //        string manager_name = "";
        //        bool scriptSubmit = true;

        //        SqlCommand cmd = new SqlCommand("select name, retrieve_endpoint, formlist_endpoint, transform from sdc_managers where id=" + manager);
        //        cmd.Connection = con;
        //        DataTable dt = new DataTable();
        //        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        //        ad.Fill(dt);
        //        if (dt.Rows.Count == 1)
        //        {
        //            retrieve_from = dt.Rows[0]["retrieve_endpoint"].ToString();
        //            transform_path = dt.Rows[0]["transform"].ToString();
        //            manager_name = dt.Rows[0]["name"].ToString();


        //            receivernames = GetSubmitParameters(receivers, ref submit_to, ref name_space, ref submit_action, ref scriptSubmit);
        //            submitnamespace.Value = name_space;
        //            submiturl.Value = submit_to;
        //            submitaction.Value = submit_action;
        //            scriptsubmit.Checked = scriptSubmit;

        //            xsltpath.Value = transform_path;
        //        }
        //    }
        //}

        protected void LoadForm()
        {
            string receivers = Request.QueryString["receivers"];
            string manager = Request.QueryString["manager"];
            string package_id = Request.QueryString["package"];
            string format = Request.QueryString["format"];

            string receivernames = "";

            if (package_id == null)
            {
                Response.Write("Package Id not supplied");
                return;
            }
            if (manager == null)
            {
                Response.Write("Form manager not supplied");
                return;
            }
            if (receivers == null)
            {
                Response.Write("Receivers not supplied");
                return;
            }

            //get end points
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
                    retrieve_from = dt.Rows[0]["retrieve_endpoint"].ToString();
                    transform_path = dt.Rows[0]["transform"].ToString();
                    manager_name = dt.Rows[0]["name"].ToString();


                    receivernames = GetSubmitParameters(receivers, ref submit_to, ref name_space, ref submit_action, ref scriptSubmit);
                    //submitnamespace.Value = name_space;
                    submiturl.Value = submit_to;
                    //submitaction.Value = submit_action;
                    scriptsubmit.Checked = scriptSubmit;

                    //xsltpath.Value = transform_path;
                    //retrieve form                           
                    string xml = getForm(package_id, retrieve_from, format);
                    if (xml != "error")
                    {
                        //remove soap envelope

                        XmlDocument xdoc = new XmlDocument();
                       

                        xdoc.LoadXml(xml);

                        XmlNamespaceManager mgr = new XmlNamespaceManager(xdoc.NameTable);

                        //get FormDesign element
                        XmlNode xNode = null;
                        if (manager_name.ToUpper() != "DCG")  //new xml
                        {

                            mgr.AddNamespace("urn", "urn:ihe:iti:rfd:2007");
                            mgr.AddNamespace("sdc", "urn:ihe:qrph:sdc:2016");
                            mgr.AddNamespace("soapenv", "http://www.w3.org/2003/05/soap-envelope");
                            mgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                            mgr.AddNamespace("def", "");

                            /*
                            IMPORTANT: since FormDesign is in default namespace urn, it is
                            required to select this node with urn = "urn:ihe:iti:rfd:2007" namespace
                             * 
                            Also when this node is selected the namespace declaration moves to this node
                            from higher node where this namespavce was declared
                             * 
                            When xNode is serialized xml will have xlmns = "urn:ihe:iti:rfd:2007"
                            xslt must deal with this default namespace
                                 
                            */
                            if (format == "html")
                            {
                                XmlNode html = xdoc.SelectSingleNode("//sdc:HTMLPackage", mgr);
                                if(html!=null)
                                {
                                    string encodedString = html.InnerText;

                                    content.InnerHtml = DecodeBase64(encodedString);
                                    content.Style.Add("display", "block");
                                    content.Style.Add("Color", "black");
                                    header.Style["display"] = "none";
                                    return;
                                }
                               

                            }

                            XmlNode xResponse = xdoc.SelectSingleNode("//urn:RetrieveFormResponse", mgr);
                            if (xResponse != null)
                            {
                                //add destination if not present
                                if (xdoc.SelectSingleNode("sdc:SubmissionRule/sdc:Destination/sdc:Endpoint", mgr) != null)
                                {
                                    submiturl.Value = xdoc.SelectSingleNode("sdc:SubmissionRule/sdc:Destination/sdc:Endpoint", mgr).Attributes["value"].ToString();
                                }
                                else
                                {
                                    string[] destinations = submit_to.Split('|');
                                    XmlNode xSDCPackage = xdoc.SelectSingleNode("//sdc:SDCPackage", mgr);
                                    XmlElement xSubmissionRule = xdoc.CreateElement("SubmissionRule", "urn:ihe:qrph:sdc:2016");

                                    foreach (string dest in destinations)
                                    {

                                        XmlElement xDestination = xdoc.CreateElement("Destination", "urn:ihe:qrph:sdc:2016");
                                        XmlElement xEndpoint = xdoc.CreateElement("Endpoint", "urn:ihe:qrph:sdc:2016");
                                        XmlAttribute att = xdoc.CreateAttribute("val");
                                        att.Value = dest;
                                        xEndpoint.Attributes.Append(att);

                                        xDestination.AppendChild(xEndpoint);

                                        xSubmissionRule.AppendChild(xDestination);

                                    }

                                    xResponse.InsertBefore(xSubmissionRule, xSDCPackage);

                                }




                                string formparms = "";
                                xNode = xdoc.SelectSingleNode("//sdc:FormDesign/@ID", mgr);
                                if (xNode != null)
                                {
                                    formparms = xNode.InnerText + " - ";
                                }
                                xNode = xdoc.SelectSingleNode("//sdc:FormDesign/sdc:Header/@title", mgr);
                                if (xNode != null)
                                {
                                    formparms = formparms + xNode.InnerText + "<br/>";
                                }

                                xNode = xdoc.SelectSingleNode("//sdc:DemogForm/@ID", mgr);
                                if (xNode != null)
                                {
                                    formparms = formparms + xNode.InnerText + "<br/>";
                                }


                                xNode = xdoc.SelectSingleNode("//sdc:DemogForm/sdc:Header/@title", mgr);
                                if (xNode != null)
                                {
                                    formparms = formparms + xNode.InnerText + "<br/>";
                                }

                                forms.Text = formparms;

                                xNode = xdoc.SelectSingleNode("//sdc:SDCPackage/@packageID", mgr);
                                if (xNode != null)
                                {
                                   packageid.Text = xNode.InnerText;
                                }

                                xNode = xResponse;

                            }
                            else
                            {
                                Response.Write(xml);
                                return;
                            }
                        }
                        else
                        {
                            /*
                             * In Old xml FormDesign is in http://healthIT.gov/sdc namespace
                             * */
                            mgr.AddNamespace("urn", "urn:ihe:iti:rfd:2007");
                            mgr.AddNamespace("sdc", "http://healthIT.gov/sdc");
                            mgr.AddNamespace("soapenv", "http://www.w3.org/2003/05/soap-envelope");
                            mgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
                            mgr.AddNamespace("def", "");

                            //xNode = xdoc.SelectSingleNode("//sdc:FormDesign", mgr);
                            xNode = xdoc.SelectSingleNode("//sdc:SDCPackage", mgr);
                            if (xNode == null)
                            {
                                Response.Write(xml);
                                return;
                            }

                        }



                        xdoc.LoadXml(xNode.OuterXml);

                        xml = XmlHelper.FormatXML(xdoc);


                        rawxml.InnerText = xml;


                        //submitnamespace.Value = name_space;
                        submiturl.Value = submit_to;
                        //submitaction.Value = submit_action;
                        retrieveurl.Value = retrieve_from;
                        //packageid.Text = "Package ID from RetrieveResponse";
                        //packagename.Text = "Package Name from RetrieveResponse";

                    
                        content.InnerHtml = GetHtml(transform_path, xml);;
                        content.Style.Add("display", "block");
                        content.Style.Add("Color", "black");
                    }
                    else
                    {
                        Response.Write("Form Manager  ID = " + manager + " not found.");
                    }
                }


                submitmsg.InnerHtml = "<p style='text-align:center;' >Submit Endpoints: <br/>"; 

                string[] names = receivernames.Split('|');
                int i = 0;
                foreach (string name in names)
                {
                    submitmsg.InnerHtml = submitmsg.InnerHtml +
                        "<span style='font-weight:bold; cursor:pointer' title='" + submiturl.Value.Split('|')[i] + "'>" + name + "</span> <br/>";
                    i++;
                }
                submitmsg.InnerHtml = submitmsg.InnerHtml + "</p>";
            }
        }

        private string GetSubmitParameters(string ids, ref string SubmitUrls, ref string SubmitNamespace, ref string SubmitAction, ref bool ScriptSubmit)
        {
            if (ids.Length == 0)
                return "";

            ScriptSubmit = true;  //initialize to true
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                
                string submit_to = "";
                string name_space = "";
                string submit_action = "";
                string names = "";
                SqlCommand cmd = new SqlCommand("select name, submit_endpoint, submit_action, submit_namespace, script_submit from sdc_receivers where id in (" + ids + ")");
                cmd.Connection = con;
                DataTable dt = new DataTable();
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                ad.Fill(dt);
                foreach(DataRow dr in dt.Rows)
                {
                    if(submit_to.Length==0)
                    {
                        submit_to = dr["submit_endpoint"].ToString();
                        names = dr["name"].ToString();
                    }
                    else
                    {
                        submit_to = submit_to + "|" + dr["submit_endpoint"].ToString();
                        names = names + "|" + dr["name"].ToString();
                    }
                    submit_action = dr["submit_action"].ToString();
                    name_space = dr["submit_namespace"].ToString();
                    if (bool.Parse(dr["script_submit"].ToString()) == false)
                    {
                        ScriptSubmit = false;  //if any is false, ScriptSubmit is false
                    }
                    //ScriptSubmit = bool.Parse(dt.Rows[0]["script_submit"].ToString()) == true ? true : false;
                }

                SubmitUrls = submit_to;
                SubmitNamespace = name_space;
                SubmitAction = submit_action;
                return names;
            }
        }

        /*not prepoluting guids on first occurrence of an element, ID will be unique*/
        private static void addGuid(XmlNode node)
        {
            if (node.HasChildNodes)
            {
                foreach (XmlNode child in node.ChildNodes)
                {
                    if (child.NodeType == XmlNodeType.Element && (child.LocalName == "Section" || child.LocalName == "Question"))
                    {

                        ((XmlElement)child).SetAttribute("instanceGuid", Guid.NewGuid().ToString());
                        XmlNode p = XmlHelper.ClosestParentElement(child, "Section,Question");
                        if (p != null)
                            ((XmlElement)child).SetAttribute("parentGuid", p.Attributes["instanceGuid"].Value);

                    }
                    addGuid(child);

                }
            }
            else
            {


            }

        }



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

        public string createRetrieveFormRequestSoap1(string formid)
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

        public string createRetrieveFormRequestSoap2(string formid, string format)
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

        [WebMethod()]
        public static string submitform(string xml, string urls)
        {
            string data = xml;
            string responsetext = "";
            

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(data);
            XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);
            mgr.AddNamespace("urn", "urn:ihe:iti:rfd:2007");
            mgr.AddNamespace("sdc", "urn:ihe:qrph:sdc:2016");
            mgr.AddNamespace("soapenv", "http://www.w3.org/2003/05/soap-envelope");
            mgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            mgr.AddNamespace("def", "");

            XmlDocument newdoc = new XmlDocument();
            XmlNode form = doc.SelectSingleNode("//sdc:FormDesign", mgr);
            XmlNode demog = doc.SelectSingleNode("//sdc:DemogFormDesign", mgr);           
           

            XmlNode xPackage = newdoc.CreateNode("element", "SDCSubmissionPackage", "urn:ihe:qrph:sdc:2016");
            xPackage.AppendChild(xPackage.OwnerDocument.ImportNode(form, true));
            
            
            if(demog!=null)
                xPackage.OwnerDocument.ImportNode(demog, true);

            newdoc.AppendChild(xPackage);

            data = newdoc.OuterXml;

            string soapRequest =	                @"<soap:Envelope xmlns:soap='http://www.w3.org/2003/05/soap-envelope' 
													 xmlns:urn='urn:ihe:iti:rfd:2007'>
													<soap:Header/>
														<soap:Body>
																 <urn:SubmitFormRequest>" +
																 data +
																@"</urn:SubmitFormRequest>
														 </soap:Body>
													 </soap:Envelope>";

            //HttpContext.Current.Session["soaprequest"] = soapRequest;
            byte[] postData = Encoding.UTF8.GetBytes(soapRequest);
            string[] receivers=urls.Split('|');
            foreach(string url in receivers)
            {
                HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
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

                     try
                    {
                        HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                            result = reader.ReadToEnd();
                        
                      
                        responsetext = responsetext + "Response from " + url + ":\n\r" + result + "\n\r";
                        //responsetext = result;
                    }
                     catch (WebException ex)
                     {
                         HttpWebResponse response = ex.Response as HttpWebResponse;
                         if (null == response)
                             throw new ArgumentNullException("Response was null");
                         using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                         {
                             string error = reader.ReadToEnd();
                             
                             responsetext = responsetext + "Response from " + url + ":\n\r" + error + "\n\r";                         
                             

                         }
                     }

                }

            }

            return  responsetext + "#!#2#3" + XmlHelper.FormatXML(soapRequest);
            
        }
        

        public string getForm(string formid, string endpoint, string format)
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
                    content.InnerHtml=error;
                    content.Style.Add("Color", "red");
                    return "error";         
                 

                }
            }
        }

        protected string DecodeBase64(string encoded)
        {
            byte[] data = Convert.FromBase64String(encoded);
            string decodedString = Encoding.UTF8.GetString(data);
            return decodedString;
        }

        protected void btnServerSubmit_Click(object sender, EventArgs e)
        {
            var data = rawxml.Value;

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(data);
            XmlNamespaceManager mgr = new XmlNamespaceManager(doc.NameTable);
            mgr.AddNamespace("urn", "urn:ihe:iti:rfd:2007");
            mgr.AddNamespace("sdc", "urn:ihe:qrph:sdc:2016");
            mgr.AddNamespace("soapenv", "http://www.w3.org/2003/05/soap-envelope");
            mgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            mgr.AddNamespace("def", "");


            XmlDocument newdoc = new XmlDocument();
            XmlNode form = doc.SelectSingleNode("//sdc:FormDesign", mgr);
            XmlNode demog = doc.SelectSingleNode("//sdc:DemogFormDesign", mgr);

           
            

            XmlNode xPackage = newdoc.CreateNode("element", "SDCSubmissionPackage", "urn:ihe:qrph:sdc:2016");
            xPackage.AppendChild(xPackage.OwnerDocument.ImportNode(form, true));
            
            
            if(demog!=null)
                xPackage.OwnerDocument.ImportNode(demog, true);

            newdoc.AppendChild(xPackage);

            data = newdoc.OuterXml;

            string soapRequest =	                @"<soap:Envelope xmlns:soap='http://www.w3.org/2003/05/soap-envelope' 
													 xmlns:urn='urn:ihe:iti:rfd:2007'>
													<soap:Header/>
														<soap:Body>
																 <urn:SubmitFormRequest>" +
																 data +
																@"</urn:SubmitFormRequest>
														 </soap:Body>
													 </soap:Envelope>";

            //serversubmitresponse.InnerText = "";
            byte[] postData = Encoding.UTF8.GetBytes(soapRequest);
            string[] receivers=submiturl.Value.Split('|');
            foreach(string url in receivers)
            {
                HttpWebRequest req = WebRequest.Create(url) as HttpWebRequest;
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

                     try
                    {
                        HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                            result = reader.ReadToEnd();
                        result = XmlHelper.FormatXML(result);
                        //serversubmitresponse.Style.Add("white-space", "pre");
                        //serversubmitresponse.Style.Add("background-color", "yellow");
                        //serversubmitresponse.Style.Add("color", "black");
                        //serversubmitresponse.InnerText= serversubmitresponse.InnerText + "Response from " + url + ":\n\r" + result + "\n\r";
                        
                    }
                     catch (WebException ex)
                     {
                         HttpWebResponse response = ex.Response as HttpWebResponse;
                         if (null == response)
                             throw new ArgumentNullException("Response was null");
                         using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                         {
                             string error = reader.ReadToEnd();
                             error = XmlHelper.FormatXML(error);
                             //serversubmitresponse.Style.Add("white-space", "pre");
                             //serversubmitresponse.Style.Add("background-color", "yellow");
                             //serversubmitresponse.Style.Add("color", "red");
                             //serversubmitresponse.InnerText = serversubmitresponse.InnerText + "Response from " + url + ":\n\r" + error + "\n\r";
                             

                         }
                     }

                }

            }
            
        }
    }
}