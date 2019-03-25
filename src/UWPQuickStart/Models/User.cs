using Newtonsoft.Json;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWPQuickStart.Models
{
    public class User
    {
        [PrimaryKey]
        public string UserName { get; set; }
        public string id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        internal static object FromJson(string jsonText)
        {
            User profile = JsonConvert.DeserializeObject<User>(jsonText);
            return profile;
        }
    }
    
}
