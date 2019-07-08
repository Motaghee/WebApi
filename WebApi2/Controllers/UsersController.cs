using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        new User { id = 1, name = "Hamed1"},
        new User { id = 2, name = "Hadi2"},
        new User { id = 3, name = "Reza3"},
        new User { id = 4, name = "Omid4"},
        new User { id = 5, name = "Saeed5"}
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
                u.name= ds.Tables[0].Rows[0][0].ToString();
            else
                u.name= "Not Found";
            //
            //u.id = 6;
            //u.name = "Ali";
            //users.Add(u);
            return u;
        }

        
}
}
