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

        [WebMethod(EnableSession=true)]
        [SoapDocumentMethod(Action = "RetrieveFormRequest",RequestNamespace="urn:ihe:iti:rfd:2007")]
        public void RetrieveFormRequest(XmlElement prepopData, 
            XmlElement workflowData )
        {
            //unpack request
            
            string packageid = workflowData.InnerText;
            
              string xmlpackage = GetPackageContent(packageid);

            
            if(xmlpackage.Length>0)
            {
                //remove declarations if exists
                //xmlpackage = xmlpackage.Substring(xmlpackage.IndexOf("<FormDesign"));
                //if (xmlpackage.IndexOf("<SDCPackage") == -1)
                //{
                //    TraceSoapExtension.SDCExtension.TraceSoapExtension.ReturnMessage = "fault";
                //    throw new Exception("Element SDCPackage not found.");
                //}
                
                xmlpackage = xmlpackage.Substring(xmlpackage.IndexOf("<SDCPackage"));

                //add instance id, etc.
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(xmlpackage);
                XmlNamespaceManager mgr = new XmlNamespaceManager(xDoc.NameTable);
                mgr.AddNamespace("urn", "urn:ihe:iti:rfd:2007");
                mgr.AddNamespace("sdc", "urn:ihe:qrph:sdc:2016");
                mgr.AddNamespace("soapenv", "http://www.w3.org/2003/05/soap-envelope");
                mgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");
               
                //get formdesign
                XmlNode xNode = xDoc.SelectSingleNode("//sdc:FormDesign",mgr);
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
                XmlAttribute att = xDoc.CreateAttribute("formInstanceURI");
                att.Value = g.ToString();
                xNode.Attributes.Append(att);               

                string instanceid = xNode.Attributes["formInstanceURI"].Value;
                att = xDoc.CreateAttribute("formInstanceVersionURI");
                att.Value = instanceid + "/ver1";
                xNode.Attributes.Append(att);

                xmlpackage = xDoc.OuterXml;


//                xmlsdc = xDoc.OuterXml;

//                string xmlpackage = @"<SDCPackage xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'
//                     xmlns='urn:ihe:qrph:sdc:2016'
//                    xmlns:xsd='http://www.w3.org/2001/XMLSchema' packageID='" + packageid + "'><XMLPackage>";
//                if(xmldemog.Length>0)
//                {
//                    xmlpackage = xmlpackage + "\n\r" + xmldemog + "\n\r" ;
//                }
//                if(xmlsdc.Length>0)
//                {
//                    xmlpackage = xmlpackage + "\n\r" + xmlsdc + "\n\r";
//                }
//                xmlpackage = xmlpackage + "</XMLPackage></SDCPackage>";
                
              

                TraceSoapExtension.SDCExtension.TraceSoapExtension.ReturnMessage = CreateRetrieveResponse( xmlpackage);
                
            }
            else
            {
                
                string fault = @"<Code>
                                    <Value>Receiver</Value>
                                 </Code>
                                <Reason>
                                    <Text xml:lang='en-US'>Unknown packageID</Text>
                                </Reason>
                            ";
                TraceSoapExtension.SDCExtension.TraceSoapExtension.ResponseWriter = "CreateRetrieveFault";
                TraceSoapExtension.SDCExtension.TraceSoapExtension.ReturnMessage = fault;
            }
        }

        //private string GetPackageContent(string packageid)
        //{
        //    using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
        //    {
        //        SqlCommand cmd = new SqlCommand("select form_content from sdc_forms where form_id = '" + packageid + "'");
        //        cmd.Connection = con;
        //        SqlDataAdapter ad = new SqlDataAdapter(cmd);
        //        DataTable dt = new DataTable();
        //        ad.Fill(dt);
        //        if (dt.Rows.Count == 1)
        //        {
        //            string xml = dt.Rows[0][0].ToString();
        //            return xml;

        //        }

        //    }
        //    return "";
        //}

        private string GetPackageContent(string packageid)
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

        private string CreateRetrieveResponse(string msg)
        {
            XNamespace ns1 = "http://www.w3.org/2003/05/soap-envelope";
            XNamespace ns2 = "http://www.w3.org/2005/08/addressing";
            XNamespace ns3 = "urn:ihe:iti:rfd:2007";
            XNamespace ns4 = "urn:ihe:qrph:sdc:2016";

            //XElement response = new XElement("Envelope", new XAttribute(XNamespace.Xmlns + "soap", ns1)
            //    , new XAttribute(XNamespace.Xmlns + "wsa", ns2),
            //    new XElement(ns1 + "Header",
            //        new XElement(ns2 + "To", new XText("http://www.sharedapps.net/SDCShare/Services/FormManager")),
            //        new XElement(ns2 + "MessageID", new XText("urn:uuid:6d39ebfe-2db4-4ea9-b6f6-06c7962f6050")),
            //        new XElement(ns2 + "Action", new XAttribute(ns1 + "mustUnderstand", "1"), new XText("urn:ihe:iti:rfd:2007:SubmitFormResponse"))),
            //        new XElement(ns1 + "Body", new XElement(ns3 + "RetrieveFormResponse", new XElement(ns4 + "SubmissionRule",
            //            new XElement(ns4 + "Destination", new XText("czxvcnzv")),new XText("|msg|")))));

            XElement response = new XElement("Envelope", new XAttribute(XNamespace.Xmlns + "soap", ns1)
               , new XAttribute(XNamespace.Xmlns + "wsa", ns2),
               new XElement(ns1 + "Header",
                   new XElement(ns2 + "To", new XText("http://www.sharedapps.net/SDCShare/Services/FormManager")),
                   new XElement(ns2 + "MessageID", new XText("urn:uuid:6d39ebfe-2db4-4ea9-b6f6-06c7962f6050")),
                   new XElement(ns2 + "Action", new XAttribute(ns1 + "mustUnderstand", "1"), new XText("urn:ihe:iti:rfd:2007:RetrieveFormResponse"))),
                   new XElement(ns1 + "Body", new XElement(ns3 + "RetrieveFormResponse", new XText("|msg|"))));
            string retval = response.ToString();
            retval = retval.Replace("|msg|", "\r\n" + msg);
            XmlHelper.FormatXML(retval);
            return retval;
        }
        private class SDCForm
        {
            public string FormID { get; set; }
            public string FormName { get; set; }
            public string FormType { get; set; }

        }
    }
    
}
