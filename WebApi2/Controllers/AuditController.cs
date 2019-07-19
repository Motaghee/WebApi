﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi2.Models;
using WebApi2.Models.utility;

namespace WebApi2.Controllers
{
    public class AuditController : ApiController
    {


 
        [HttpGet]
        [Obsolete]
        public Car get()
        {
            Car car = new Car();
            try
            {
                
                car.VIN = "";
                if ((car != null)) // && (U.Macaddress == "48:13:7e:11:d7:1f"))
                {
                    car.VALIDFORMAT = CarUtility.CheckFormatVin(car.VIN);
                    car.AUDITEDITABLE = false;
                    string commandtext = string.Format(@"update svaauditcar a set a.editabledefectorigin=1 ,a.editablemoduledefect=1 where a.vin in ('{0}')", CarUtility.GetVinWithoutChar(car.VIN));
                    int intResult = clsDBHelper.ExecuteQueryScalar(commandtext, true);
                    if (intResult > 0)
                    {
                        car.AUDITEDITABLE = true;
                        return car;
                    }
                    else
                    {
                        return car;
                    }
                }
                else
                {
                    return car;
                }
            }
            catch (Exception e)
            {
                return car;
            }
            //return users;
        }

        // GET: api/Users/5

        // POST: api/Audit
        [HttpPost]
        [Obsolete]
        public Car Post([FromBody] Car car)
        {
            try
            {
                if ((car != null)) // && (U.Macaddress == "48:13:7e:11:d7:1f"))
                {
                    car.VALIDFORMAT= CarUtility.CheckFormatVin(car.VIN);
                    car.AUDITEDITABLE = false;
                    string commandtext = string.Format(@"update svaauditcar a set a.editabledefectorigin=1 ,a.editablemoduledefect=1 where a.vin in ('{0}')", CarUtility.GetVinWithoutChar(car.VIN));
                    int intResult= clsDBHelper.ExecuteQueryScalar(commandtext,true);
                    if (intResult > 0)
                    {
                        car.AUDITEDITABLE = true;
                        return car;
                    }
                    else
                    {
                        return car;
                    }
                }
                else
                {
                    return car;
                }
            }
            catch (Exception e)
            {
                car.MSG = "exc1:" + e.Message;
                return car;
            }

        }

    }
}
