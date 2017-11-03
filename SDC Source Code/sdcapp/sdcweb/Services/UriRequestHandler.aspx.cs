using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Serialization;
using System.Xml.Linq;

namespace SDC
{
    public partial class UriRequestHandler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string msgid = Request.QueryString["MessageId"];
            if(!Page.IsPostBack)
            {
                using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
                {
                    SqlCommand cmd = new SqlCommand(@"select html from SDC_FormRequestsQ where transactionid = '" + msgid + "'");
                    cmd.Connection = con;
                    con.Open();
                    string retval = cmd.ExecuteScalar().ToString();
                    con.Close();

                    //build xml
                    rawxml.Value = BuildXml(msgid);

                    formcontent.InnerHtml = retval;

                    //Response.Write(retval);
                }
            }
            
        }

        private string GetXml(string MessageID, ref string PackageID)
        {
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(@"select xml from SDC_FormRequestsQ where transactionid = '" + MessageID + "'");
                cmd.Connection = con;
                con.Open();
                string retval = cmd.ExecuteScalar().ToString();
                cmd.CommandText = "select packageid from SDC_FormRequestsQ where transactionid = '" + MessageID + "'";
                PackageID = cmd.ExecuteScalar().ToString();
                con.Close();

                //remove declaration
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(retval);

                foreach (XmlNode node in xdoc)
                {
                    if (node.NodeType == XmlNodeType.XmlDeclaration)
                    {
                        xdoc.RemoveChild(node);
                    }
                }
                return xdoc.OuterXml;
            }
        }

        private string BuildXml(string MessageID)
        {
            string packageid = "";
            string xml = GetXml(MessageID, ref packageid);
            XElement response = null;
            XNamespace ns3 = "urn:ihe:iti:rfd:2007";
            response = new XElement(ns3 + "RetrieveFormResponse",
                     new XElement(ns3 + "form", new XElement(ns3 + "Structured", new XText("|msg|"))),
                     new XElement(ns3 + "contentType", new XText("application/xml+sdc")));
            string temp = response.ToString();
            temp = temp.Replace("|msg|",xml);
            temp = InsertDestination(temp, packageid);
            temp = XmlHelper.FormatXML(temp);
            return temp.ToString();
            
        }

        private string InsertDestination(string xml, string packageid)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.LoadXml(xml);
            XmlNamespaceManager mgr = new XmlNamespaceManager(xdoc.NameTable);
            mgr.AddNamespace("urn", "urn:ihe:iti:rfd:2007");
            mgr.AddNamespace("sdc", "urn:ihe:qrph:sdc:2016");
            mgr.AddNamespace("soapenv", "http://www.w3.org/2003/05/soap-envelope");
            mgr.AddNamespace("xsi", "http://www.w3.org/2001/XMLSchema-instance");

            XmlNode xResponse = xdoc.SelectSingleNode("//urn:RetrieveFormResponse", mgr);
            if (xResponse != null)
            {

                //read default destination from 

                string[] destinations = GetDefaultDestinations(packageid).Split('|');
                XmlNode xSDCPackage = xdoc.SelectSingleNode("//sdc:SDCPackage", mgr);

                XmlNode xXMLPackage = xdoc.SelectSingleNode("//sdc:XMLPackage", mgr);
                XmlElement xSubmissionRule = xdoc.CreateElement("SubmissionRule", "urn:ihe:qrph:sdc:2016");
                XmlAttribute xRuleID = xdoc.CreateAttribute("ruleID");
                xRuleID.Value = "www.CAP.org/eCC";
                xSubmissionRule.Attributes.Append(xRuleID);
                foreach (string dest in destinations)
                {

                    XmlElement xDestination = xdoc.CreateElement("Destination", "urn:ihe:qrph:sdc:2016");
                    XmlElement xEndpoint = xdoc.CreateElement("Endpoint", "urn:ihe:qrph:sdc:2016");
                    XmlAttribute att = xdoc.CreateAttribute("val");
                    att.Value = dest;
                    xEndpoint.Attributes.Append(att);

                    XmlElement xDesc = xdoc.CreateElement("EndpointDescription", "urn:ihe:qrph:sdc:2016");
                    XmlAttribute att1 = xdoc.CreateAttribute("val");
                    att1.Value = "Endpoint description here";
                    xDesc.Attributes.Append(att1);

                    xDestination.AppendChild(xEndpoint);
                    xDestination.AppendChild(xDesc);
                    xSubmissionRule.AppendChild(xDestination);

                }

                xSDCPackage.InsertBefore(xSubmissionRule, xXMLPackage);


            }
            return xdoc.OuterXml;
        }

        private string GetDefaultDestinations(string packageid)
        {
            string url = "";
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(@"select default_receiver from sdc_packages where package_id = '" + packageid + "'", con);
                cmd.Connection = con;
                con.Open();
                string ids = cmd.ExecuteScalar().ToString();
                string[] IDS = ids.Split(',');
                DataTable dt = new DataTable();
                SqlDataAdapter ad = new SqlDataAdapter();
                ad.SelectCommand = cmd;
                ad.Fill(dt);
                foreach (string id in IDS)
                {
                    
                    cmd.CommandText = "select submit_endpoint from sdc_receivers where id = " + id;
                    string retval = cmd.ExecuteScalar().ToString();
                    if (retval.Length > 0)
                    {
                        if (url.Length > 0)
                            url = url + "|" + retval;
                        else
                            url = retval;
                    }


                }
                con.Close();
            }

            return url;
        }

    }
}