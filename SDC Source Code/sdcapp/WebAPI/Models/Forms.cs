using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{

    public class Forms
    {
        public string type { get; set; }
        public CodedItem title { get; set; }
        public Source source { get; set; }
        public CodedItem status { get; set; }
        public Date date { get; set; }
        public List<Entry> entry { get; set; }
    }
    
    public class Entry
    {
        public Item item { get; set; }
    }
    public class Item
    {
        public string id { get; set; }
        public CodedItem reference { get; set; }
        public CodedItem display { get; set; }
    }

    public class Reference
    {
        public CodedItem reference { get; set; }
    }

    public class Source
    {
        public CodedItem reference { get; set; }
    }

    public class Date
    {
        public string value { get; set; }
    }
    
    
    public class CodedItem
    {
        public string value {get;set;}
    }

   

   
}