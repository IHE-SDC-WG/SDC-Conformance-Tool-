using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Data.SqlClient;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Xsl;
using System.Web.Services.Description;
using System.Web.Script.Services;
using System.Data;
using System.Xml.Schema;
using System.Xml.Linq;
using System.Xml.Serialization;

using System.ServiceModel.Web;

namespace SDC.Services
{
    /// <summary>
    /// Summary description for FormManager
    /// </summary>
    [WebService(Namespace = "urn:ihe:iti:rfd:2007")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class FormManager : System.Web.Services.WebService
    {
        public SoapUnknownHeader[] unknownHeaders;
        [WebMethod(EnableSession=true), SoapHeader("unknownHeaders")]
        [SoapDocumentMethod(Action = "RetrieveFormRequest",RequestNamespace="urn:ihe:iti:rfd:2007")]
        public string RetrieveFormRequest(XmlElement prepopData, 
          [XmlAnyElement]   XmlElement workflowData )
        {
            //unpack request
            foreach (SoapUnknownHeader header in unknownHeaders)
            {
                header.DidUnderstand = true;
            }

            
            XmlDocument xRequest = new XmlDocument();
            XmlNamespaceManager reqmgr = new XmlNamespaceManager(xRequest.NameTable);
            reqmgr.AddNamespace("urn", "urn:ihe:iti:rfd:2007");
            reqmgr.AddNamespace("sdc", "urn:ihe:qrph:sdc:2016");
            reqmgr.AddNamespace("soapenv", "http://www.w3.org/2003/05/soap-envelope");
            reqmgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            xRequest.LoadXml(workflowData.OuterXml);
            string packageid = xRequest.SelectSingleNode("//urn:formID", reqmgr).InnerText;
            string format = "";
            XmlNode xEncodedResponse = xRequest.SelectSingleNode("//urn:encodedResponse", reqmgr);

           
            
            string replyto = HttpContext.Current.Request.UserHostAddress;
            string prepop = "";
            
            //is prepop data available?
            
            if (prepopData!=null && prepopData.OuterXml != "")
            {
                prepop = prepopData.OuterXml;
              

            }
            
            if(xEncodedResponse.Attributes["responseContentType"]!=null)
            {
                 format = xEncodedResponse.Attributes["responseContentType"].Value;


            }

            //get xml of form from the db
            string xmlpackage = GetXMLContent(packageid);

            //if response type is urn, create urn response and return 
            if(xEncodedResponse.InnerText=="false")
            {    
                string uri = GetUri(packageid);
                uri = Server.UrlEncode(uri);
                //uri = Base64Encode(uri);
                //uri = Server.UrlEncode(uri);
                TraceSoapExtension.SDCExtension.TraceSoapExtension.ResponseWriter = "";
                TraceSoapExtension.SDCExtension.TraceSoapExtension.ReturnMessage = CreateRetrieveResponse(uri, "uri", packageid, prepop, xmlpackage, replyto);
                return "Success";               
            }

            //if response type is xml or html continue
            
            XmlDocument xResponse = new XmlDocument();
            xResponse.LoadXml(xmlpackage);
            XmlNamespaceManager mgr = new XmlNamespaceManager(xResponse.NameTable);
            mgr.AddNamespace("urn", "urn:ihe:iti:rfd:2007");
            mgr.AddNamespace("sdc", "urn:ihe:qrph:sdc:2016");
            mgr.AddNamespace("soapenv", "http://www.w3.org/2003/05/soap-envelope");
            mgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
               
            //ADD instance id, destinations, etc if 
            

            if(xmlpackage.Length>0)
            {
               if (format=="text/html+sdc")
                {
                    //create html on the fly
                    /*
                    string test = GetHtml("Transforms/Ver3", xmlpackage);
                    string search = "<body align=\"left\">";
                    int index = test.IndexOf(search) + search.Length;
                    System.Text.StringBuilder bldr = new System.Text.StringBuilder(test);
                    string insert = @"<textarea name='rawxml' id='rawxml' cols='200' style='display:none;margin-left:80px;margin-right:80px;background-color:wheat' rows='15'>";
                    insert = insert + xmlpackage + "</textarea>";
                    bldr.Insert(index + 6, insert);
                    test = bldr.ToString();
                    test = Base64Encode(test);                    
                    
                    */
                    string encodedhtml = Base64Encode(GetHTMLContent(packageid));
                    TraceSoapExtension.SDCExtension.TraceSoapExtension.ResponseWriter = "";
                    //XmlDocument tempdoc = new XmlDocument();
                    //tempdoc.LoadXml(CreateRetrieveResponse(encodedhtml, "html"));

                    TraceSoapExtension.SDCExtension.TraceSoapExtension.ReturnMessage = CreateRetrieveResponse(encodedhtml,"html",packageid,prepop,xmlpackage,replyto);
                    Context.Response.StatusCode = 200;
                    return "Success";
                }

                //add instance id, etc.
               
                //
               
                //get formdesign element and add instance attributes
                XmlNode xNode = xResponse.SelectSingleNode("//sdc:FormDesign", mgr);
                if (xNode.Attributes["formInstanceURI"]!=null)
                {
                    xNode.Attributes.Remove(xNode.Attributes["formInstanceURI"]);                   

                }

                if (xNode.Attributes["formInstanceVersionURI"] != null)
                {
                    xNode.Attributes.Remove(xNode.Attributes["formInstanceURI"]);

                }

                if (xNode.Attributes["formPreviousInstanceVersionURI"] != null)
                {
                    xNode.Attributes.Remove(xNode.Attributes["formPreviousInstanceVersionURI"]);

                }

                Guid g;
                g = Guid.NewGuid();
                XmlAttribute att = xResponse.CreateAttribute("formInstanceURI");
                att.Value = g.ToString();
                xNode.Attributes.Append(att);               

                string instanceid = xNode.Attributes["formInstanceURI"].Value;
                att = xResponse.CreateAttribute("formInstanceVersionURI");
                att.Value = instanceid + "/ver1";
                xNode.Attributes.Append(att);

                XmlNode package = xResponse.SelectSingleNode("//sdc:SDCPackage", mgr);
                xmlpackage = package.OuterXml;


                TraceSoapExtension.SDCExtension.TraceSoapExtension.ResponseWriter = "";
                TraceSoapExtension.SDCExtension.TraceSoapExtension.ReturnMessage = CreateRetrieveResponse( xmlpackage,"xml",packageid,prepop,xmlpackage,replyto);
                Context.Response.StatusCode = 200;
            }
            else
            {
                
                string fault = @"<Code>
                                    <Value>Receiver</Value>
                                 </Code>
                                <Reason>
                                    <Text xml:lang='en-US'>Unknown packageID = " + packageid + @"</Text>
                                </Reason>
                            ";
                TraceSoapExtension.SDCExtension.TraceSoapExtension.ResponseWriter = "CreateRetrieveFault";
                TraceSoapExtension.SDCExtension.TraceSoapExtension.ReturnMessage = fault;
                return "Success";
            }
            return "Success";
        }

       [WebMethod(EnableSession = true), SoapHeader("unknownHeaders")]
        public string GetFormHtml(string MessageId)
        {
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(@"select form from SDC_FormRequestsQ where TransactionID = '" + MessageId + "'");
                cmd.Connection = con;
                con.Open();
                string retval = cmd.ExecuteScalar().ToString();
                con.Close();
                return retval;
                
            }
        }

        public string GetHtml(string transform_path, string xml)
        {
            string xsltPath = Server.MapPath(transform_path)+ "\\sdctemplate.xslt";
            string csspath = Server.MapPath(transform_path) + "\\sdctemplate.css";
            //3/10/2016 - change encoding to unicode 
            System.IO.MemoryStream stream = new System.IO.MemoryStream(System.Text.UnicodeEncoding.ASCII.GetBytes(xml));
            System.Xml.XPath.XPathDocument document = new System.Xml.XPath.XPathDocument(stream);
            System.IO.StringWriter writer = new System.IO.StringWriter();
            System.Xml.Xsl.XslCompiledTransform transform = new System.Xml.Xsl.XslCompiledTransform();

            System.Xml.Xsl.XsltSettings settings = new System.Xml.Xsl.XsltSettings(true, true);

            System.Xml.XmlSecureResolver resolver = new System.Xml.XmlSecureResolver(new System.Xml.XmlUrlResolver(), csspath);
            try
            {
                transform.Load(xsltPath, settings, resolver);
                transform.Transform(document, null, writer);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
            return writer.ToString();
        }

        private string GetXMLContent(string packageid)
        {
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select package_content from sdc_packages where package_id = '" + packageid + "'");
                cmd.Connection = con;
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                if (dt.Rows.Count == 1)
                {
                    string xml = dt.Rows[0][0].ToString();
                    return xml;

                }

            }
            return "";
        }

        private string GetHTMLContent(string packageid)
        {
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select html_content from sdc_packages where package_id = '" + packageid + "'");
                cmd.Connection = con;
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                if (dt.Rows.Count == 1)
                {
                    string xml = dt.Rows[0][0].ToString();
                    return xml;

                }

            }
            return "";
        }

        private List<SDCForm> GetForms(string packageid)
        {
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(@"select a.form_id, form_type, form_name from sdc_package_forms a 
                                    join sdc_forms b on a.form_id = b.form_id where package_id = '" + packageid + "'");
                cmd.Connection = con;
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                List<SDCForm> forms = new List<SDCForm>();

                foreach (DataRow dr in dt.Rows)
                {
                    SDCForm form = new SDCForm();
                    form.FormID = dr["form_id"].ToString();
                    form.FormName = dr["form_name"].ToString();
                    form.FormType = dr["form_type"].ToString();
                    forms.Add(form);
                }
                return forms;
            }
        }

        private string GetUri(string packageid)
        {

            //return formmanager address
            return Server.MapPath("UriRequestHandler.aspx");
          
        }


        
      
        private string CreateRetrieveResponse(string msg, string format, string packageid, string prepop, string formxml, string replyto)
        {          
            
           string html = "";
           
            
           html = GenerateForm(formxml, prepop, format);
                              
           
            string MessageId = Guid.NewGuid().ToString().ToLower();
            SaveFormInQ(formxml, html, MessageId, replyto, packageid);

            XNamespace ns1 = "http://www.w3.org/2003/05/soap-envelope";
            XNamespace ns2 = "http://www.w3.org/2005/08/addressing";
            XNamespace ns3 = "urn:ihe:iti:rfd:2007";
            XNamespace ns4 = "urn:ihe:qrph:sdc:2016";


            if (format == "uri")
            {
                string url = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path);
                url = url.Substring(0, url.LastIndexOf("/")) + "/UriRequestHandler.aspx";
                msg = url + "?MessageId=" + MessageId;
            }

            XElement response=null;
            if(format=="xml")
            {
            
                response = new XElement(ns1+"Envelope", new XAttribute(XNamespace.Xmlns + "soap", ns1)
             , new XAttribute(XNamespace.Xmlns + "wsa", ns2),
             new XElement(ns1 + "Header",
                 new XElement(ns2 + "To", new XText(replyto)),
                 //new XElement(ns2 + "MessageID", new XText("urn:uuid:6d39ebfe-2db4-4ea9-b6f6-06c7962f6050")),
                 new XElement(ns2 + "MessageID", new XText("urn:uuid:" + MessageId)),
                 new XElement(ns2 + "Action", new XAttribute(ns1 + "mustUnderstand", "1"), new XText("urn:ihe:iti:rfd:2007:RetrieveFormResponse"))),
                 new XElement(ns1 + "Body", new XElement(ns3 + "RetrieveFormResponse",
                     new XElement(ns3+"form" , new XElement(ns3+"Structured",  new XText("|msg|"))),
                     new XElement(ns3+"contentType",new XText("application/xml+sdc")),
                     new XElement(ns3+"responseCode",new XText("Request succeeded")))));
            
            
            }
            else if(format=="html")
            {
          

                response = new XElement(ns1+"Envelope", new XAttribute(XNamespace.Xmlns + "soap", ns1)
             , new XAttribute(XNamespace.Xmlns + "wsa", ns2),
             new XElement(ns1 + "Header",
                 //new XElement(ns2 + "To", new XText("")),
                 //new XElement(ns2 + "MessageID", new XText("urn:uuid:6d39ebfe-2db4-4ea9-b6f6-06c7962f6050")),
                 new XElement(ns2 + "MessageID", new XText("urn:uuid:" + MessageId)),
                 new XElement(ns2 + "Action", new XAttribute(ns1 + "mustUnderstand", "1"), new XText("urn:ihe:iti:rfd:2007:RetrieveFormResponse"))),
                 new XElement(ns1 + "Body", new XElement(ns3 + "RetrieveFormResponse",
                     new XElement(ns3 + "form", new XElement(ns3 + "Structured",
                         new XElement(ns4+"SDCPackage", new XAttribute("packageID",packageid), 
                         new XElement(ns4+"HTMLPackage", new XText("|msg|"))))),
                         new XElement(ns3+"contentType",new XText("text/html+sdc")),
                         new XElement(ns3+"responseCode",new XText("Request succeded")))));
            }
            else if (format=="uri")
            {

                response = new XElement(ns1 + "Envelope", new XAttribute(XNamespace.Xmlns + "soap", ns1)
             , new XAttribute(XNamespace.Xmlns + "wsa", ns2),
             new XElement(ns1 + "Header",
                 //new XElement(ns2 + "To", new XText("")),
                 //new XElement(ns2 + "MessageID", new XText("urn:uuid:6d39ebfe-2db4-4ea9-b6f6-06c7962f6050")),
                 new XElement(ns2 + "MessageID", new XText("urn:uuid:" + MessageId)),
                 new XElement(ns2 + "Action", new XAttribute(ns1 + "mustUnderstand", "1"), new XText("urn:ihe:iti:rfd:2007:RetrieveFormResponse"))),
                 new XElement(ns1 + "Body", new XElement(ns3 + "RetrieveFormResponse",
                     new XElement(ns3+"form" , new XElement(ns3+"URL",  new XText("|msg|"))),
                     new XElement(ns3+"contentType",new XText("URL")),
                     new XElement(ns3+"responseCode",new XText("Request succeeded")))));
            }
            string retval = response.ToString();
            retval = retval.Replace("|msg|", "\r\n" + msg);

           

            if(format=="xml")
                XmlHelper.FormatXML(retval);
            

            return retval;
        }

        private string GenerateForm(string xml, string prepop, string format)
        {
            //parse prepop

            xml = Prepop(xml, prepop);
            //XmlDocument cda = new XmlDocument();
            //cda.LoadXml(prepop);
            ////find elements to prepop
            //XmlNamespaceManager ns = new XmlNamespaceManager(cda.NameTable);
            // ns.AddNamespace("urn", "urn:hl7-org:v3");
            //ns.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            //ns.AddNamespace("rfd", "urn:ihe:iti:rfd:2007");
            //ns.AddNamespace("sdtc", "urn:hl7-org:sdtc");
            //ns.AddNamespace("fn", " http://www.w3.org/2005/xpath-functions");

            ////
            if (format == "xml")
                return xml;

            string html = GetHtml("transforms/ver3",xml);
            return html;
        }

        private string Prepop(string xml, string prepop)
        {
            XmlDocument xmlpackage = new XmlDocument();
            xmlpackage.LoadXml(xml);

            if (prepop == "")
                return xml;

            XmlDocument cda = new XmlDocument();
            cda.LoadXml(prepop);

            //find elements to prepop
            XmlNamespaceManager cdans = new XmlNamespaceManager(cda.NameTable);
            cdans.AddNamespace("urn", "urn:hl7-org:v3");
            cdans.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            cdans.AddNamespace("rfd", "urn:ihe:iti:rfd:2007");
            cdans.AddNamespace("sdtc", "urn:hl7-org:sdtc");
            cdans.AddNamespace("fn", " http://www.w3.org/2005/xpath-functions");

            XmlNamespaceManager packagemgr = new XmlNamespaceManager(xmlpackage.NameTable);
            packagemgr.AddNamespace("urn", "urn:ihe:iti:rfd:2007");
            packagemgr.AddNamespace("sdc", "urn:ihe:qrph:sdc:2016");
            packagemgr.AddNamespace("soapenv", "http://www.w3.org/2003/05/soap-envelope");
            packagemgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(@"select * from sdc_prepopmap",con);
                DataTable dt = new DataTable();
                SqlDataAdapter ad = new SqlDataAdapter();
                ad.SelectCommand = cmd;
                ad.Fill(dt);
                foreach(DataRow dr in dt.Rows)
                {
                    string xpath = dr["cdapath"].ToString().Replace("\r\n","");
                    decimal ckey = decimal.Parse(dr["sdcid"].ToString());
                    xpath = DecorateXPathWithNameSpace(xpath, "urn");
                    XmlNodeList xNodeList = cda.SelectNodes(xpath,cdans);
                    if(xNodeList.Count>0)
                    {
                         xpath = @"//*[@ID='" + ckey.ToString() + "']";
                        
                        //find it in xmlpackage
                         XmlNode xNode = xmlpackage.SelectSingleNode(xpath,packagemgr);
                        if(xNode!=null)
                        {
                            string value = xNodeList[0].InnerText;
                            if (xNode.LocalName=="ListItem")
                            {
                                XmlAttribute attr = xmlpackage.CreateAttribute("selected");
                                attr.Value="true";
                                xNode.Attributes.Append(attr);

                            }
                            else
                            {
                                //free response
                                XmlNode xValue = xNode.SelectSingleNode("sdc:ResponseField/sdc:Response/sdc:string",packagemgr);
                                if(xValue!=null) //string
                                {
                                    XmlAttribute attr = xmlpackage.CreateAttribute("val");
                                    attr.Value = value;
                                    xValue.Attributes.Append(attr);
                                }
                                else
                                {
                                    xValue = xNode.SelectSingleNode("sdc:ResponseField/sdc:Response/sdc:integer", packagemgr);
                                    if (xValue != null)  //integer
                                    {
                                        XmlAttribute attr = xmlpackage.CreateAttribute("val");
                                        attr.Value = value;
                                        xValue.Attributes.Append(attr);
                                    }
                                }
                                
                            }
                        }
                    }
                }
               
            }
           



            return xmlpackage.OuterXml;
        }

        private void SaveFormInQ(string xml, string html, string messageid, string replyto, string packageid)
        {

            //packageid = Server.UrlDecode(packageid);
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(@"insert into SDC_FormRequestsQ (TransactionID, PackageID, Requestor, xml, html, DateCreated) values (@TransactionID, @PackageID, @Requestor, @Xml, @Html, @DateCreated)");
                cmd.Connection = con;
                con.Open();

                cmd.Parameters.AddWithValue("TransactionID", messageid);
                cmd.Parameters.AddWithValue("PackageID", packageid);
                cmd.Parameters.AddWithValue("Requestor", replyto);
                cmd.Parameters.AddWithValue("Xml", xml);
                cmd.Parameters.AddWithValue("Html", html);
                cmd.Parameters.AddWithValue("DateCreated", DateTime.Now);

                cmd.ExecuteNonQuery();
                con.Close();

                cmd.Dispose();

            }
        }

        private class SDCForm
        {
            public string FormID { get; set; }
            public string FormName { get; set; }
            public string FormType { get; set; }

        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }


        public static string DecorateXPathWithNameSpace(string XPath, string NS)
        {
            if(XPath.Length>0)
            {
                string[] temp = XPath.Split('/');
                int i = 0;
                foreach (string test in temp)
                {
                    if(test.Length>0)
                    {
                        if(test.IndexOf("sdtc:")==-1 && test.IndexOf("sdc")==-1)
                        {
                            if(test.Substring(0,1)!="@")
                            {
                                temp[i]= NS + ":" + test;
                            }
                        }

                    }
                    i++;
                }

                XPath=String.Join("/",temp);
            }

            return XPath;
        }

    }
    
}
