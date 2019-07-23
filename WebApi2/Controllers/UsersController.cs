﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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
        new User { SRL = 1, FNAME = "Hamed1"},
        new User { SRL = 2, FNAME = "Hadi2"},
        new User { SRL = 3, FNAME = "Reza3"},
        new User { SRL = 4, FNAME = "Omid4"},
        new User { SRL = 5, FNAME = "Saeed5"}
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
            try
            {
                if ((id != null)) // && (U.Macaddress == "48:13:7e:11:d7:1f"))
                {
                    //string commandtext0 = string.Format(@"select vin from carid c where c.prodno ={0}", id.ToString());
                    string commandtext = string.Format(@"select * from QCUSERT Where srl=4314");
                    //DataSet ds = clsDBHelper.ExecuteMyQuery(commandtext);
                    List<User> FoundUser = new List<User>();
                    FoundUser = clsDBHelper.GetDBObjectByObj(new User(), null, commandtext).Cast<User>().ToList();
                    if (FoundUser.Count == 1)
                    {
                        FoundUser[0].USERATHENTICATION = true;
                        byte[] userByte = Encoding.UTF8.GetBytes("1000861");
                        FoundUser[0].PSW = clsDBHelper.Cryptographer.CreateHash("8585", "MD5", userByte);
                        return FoundUser[0];
                    }
                    else
                        return null;
                    
                    
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        // POST: api/Login
        [HttpPost]
        public User Post([FromBody] User user)
        {
            try
            {
                if ((user != null)) // && (U.Macaddress == "48:13:7e:11:d7:1f"))
                {
                    user.USERATHENTICATION = false;
                    byte[] userByte = Encoding.UTF8.GetBytes(user.USERNAME);
                    string strHashPSW = clsDBHelper.Cryptographer.CreateHash(user.PSW,"MD5", userByte);
                    string commandtext = string.Format(@"select srl,fname,lname,username,psw from QCUSERT Where USERName='{0}'
                                                    and PSW ='{1}' and (InUse=1 or InUse=-2)", user.USERNAME, strHashPSW);
                    //DataSet ds = clsDBHelper.ExecuteMyQuery(commandtext);
                    List<User> FoundUser = new List<User>();
                    FoundUser = clsDBHelper.GetDBObjectByObj(new User(), null, commandtext).Cast<User>().ToList();
                    //---
                    user.MACISVALID        = FoundUser[0].MACISVALID        = true;
                    user.CLIENTVERISVALID  = FoundUser[0].CLIENTVERISVALID  = true;
                    if ((FoundUser[0].USERNAME == "1000861") || (FoundUser[0].USERNAME == "257923"))
                        FoundUser[0].USERAUTHORIZATION = user.USERAUTHORIZATION = true;
                    else
                        FoundUser[0].USERAUTHORIZATION = user.USERAUTHORIZATION = false;

                    //---
                    if (FoundUser.Count == 1)
                    {
                        user.USERATHENTICATION = FoundUser[0].USERATHENTICATION = true;
                        clsDBHelper.LogtLoginUser(FoundUser[0].USERNAME+"_"+ FoundUser[0].LNAME + "__"+user.MACADDRESS+ "_AppVer:"+user.USERAPPVER);
                        return FoundUser[0];
                    }
                    else
                    {
                        return user;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }



    }
}
