using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using WebAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;

namespace WebAPI.Controllers
{
    public class FormsController : ApiController
    {
        // GET: api/Forms
        public object Get()
        {
            Forms f = new Forms();
            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select package_id, package_name from sdc_packages");
                cmd.Connection = con;
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                
                f.type = "List";
                CodedItem title = new CodedItem();
                title.value = "All Packages Available In This Form Manager";
                Source source = new Source();

                source.reference = new CodedItem();
                source.reference.value="CAP Form Manager";
                CodedItem status = new CodedItem();
                status.value = "current";
                Date date = new Date();
                date.value = DateTime.Now.ToString();

                f.title = title;
                f.source = source;
                f.status = status;
                f.date = date;
                f.entry = new List<Entry>();
                


                foreach(DataRow dr in dt.Rows)
                {
                    
                    Entry entry = new Entry();
                    entry.item = new Item();
                    entry.item.id = dr["package_id"].ToString();
                    CodedItem display = new CodedItem();
                    display.value=dr["package_name"].ToString();
                    entry.item.display = display;
                    CodedItem refer = new CodedItem();
                    refer.value = "";
                    entry.item.reference = new CodedItem();
                    entry.item.reference.value = "";
                   
                    f.entry.Add(entry);
                }
                
               

            }

            return f;
            //var json = JsonConvert.SerializeObject(f);
            //var json = new JavaScriptSerializer().Serialize(f);

            //return json;
        }

        // GET: api/Forms/5
        public HttpResponseMessage Get(string id)
        {
            var response = new HttpResponseMessage();
            

            using (SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["sdcdb"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand("select html_content from sdc_packages where package_id = '" + id + "'");
                cmd.Connection = con;
                SqlDataAdapter ad = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                ad.Fill(dt);
                if(dt.Rows.Count>0)
                {
                    if (dt.Rows[0][0]!=System.DBNull.Value)
                    {
                        string html = dt.Rows[0][0].ToString();
                        response.Content = new StringContent(html);
                        response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/html");
                    }
                    
                }
            }
            
            return response;
        }

        // POST: api/Forms
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Forms/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Forms/5
        public void Delete(int id)
        {
        }
    }
}
