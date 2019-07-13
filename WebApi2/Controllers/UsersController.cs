using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi2.Models;
using WebApi2.Models.utility;

namespace WebApi2.Controllers
{
    public class UsersController : ApiController
    {
        private List<User> users = new List<User>
    {
        new User { id = 1, Fname = "Hamed1"},
        new User { id = 2, Fname = "Hadi2"},
        new User { id = 3, Fname = "Reza3"},
        new User { id = 4, Fname = "Omid4"},
        new User { id = 5, Fname = "Saeed5"}
    };
        // GET: api/Users
        //[ResponseType(typeof(IEnumerable<User>))]

        //public string get()
        //{
        //    return "hello world";
        //}

        [HttpGet]
        public List<User> get()
        {
            return users;
        }

        // GET: api/Users/5
        [HttpGet]
        [Obsolete]
        public User Get(decimal id)
        {
            User userObject = (from user in users
                               where user.id == id
                               select user).SingleOrDefault();
            string commandtext = string.Format(@"select vin from carid c where c.prodno ={0}",id.ToString()) ;
            DataSet ds= clsDBHelper.ExecuteMyQuery(commandtext);
            User u = new User();
            u.id = id;
            if ((ds != null) && (ds.Tables[0] != null) && (ds.Tables[0].Rows.Count > 0)) 
                u.Fname= ds.Tables[0].Rows[0][0].ToString();
            else
                u.Fname= "Not Found";
            //
            //u.id = 6;
            //u.name = "Ali";
            //users.Add(u);
            return u;
        }
        // POST: api/Login
        [HttpPost]
        public User Post([FromBody] User U)
        {
            // int i =U.id;
            if ((U != null) && (U.Macaddress == "48:13:7e:11:d7:1f"))
            {

                User U2 = new User();
                U2.Username = "1000861";
                U2.id = 4314;
                U2.Fname = "hamiReza";
                U2.Lname = "MT";
                return U2;
            }
            else
            {
                User U2 = new User();
                U2.Username = "InvalidMac";
                U2.id = 0;
                U2.Fname = "InvalidMac";
                U2.Lname = "InvalidMac";
                return U2;
            }
        }



    }
}
