using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi2.Models;
using WebApi2.Models.utility;

namespace WebApi2.Controllers
{
    public class CarController : ApiController
    {
        [HttpGet]
        [Obsolete]
        public Car get()
        {

            return null;
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
                    car.VALIDFORMAT = CarUtility.CheckFormatVin(car.VIN);
                    car.VINWITHOUTCHAR= CarUtility.GetVinWithoutChar(car.VIN);
                    if (car.VALIDFORMAT)
                    {
                        List<Car> carinfo = new List<Car>();
                        string commandtext = string.Format(@"select z.*,case 
                                                             when (z.joinaryTeam is null) then
                                                              'عدم تجاري'
                                                             when (z.joinaryTeam ='Z') then
                                                               'Other'
                                                             when (z.shopcode = 14) then
                                                              JoinaryTeam
                                                             when (z.shopcode = 15) then
                                                              JoinaryTeam || '1'
                                                             when (z.shopcode = 23) then  
                                                               z.JoinaryTeam
                                                             else
                                                              'Other'
                                                             end as JoinaryTeamDesc,
                                                             'NAS'||vin as nasvin
                                               from 
                                            (select c.vin,c.prodno,c.joinerydate,c.bdmdlcode,c.bdstlcode ,c.fitypecode,c.finqccode,c.clrcode,
                                                   pt.FNI_GetAsmProdShopCodeByVin (c.vin) as shopCode
                                                   ,pt.FNI_GetAsmProdShopByVin (c.vin) as shopname,
                                                   pt.FNI_GetStationTeamWorkByVin(c.vin, 'T',8) as JoinaryTeam,pt.FNI_GetStationTeamWorkByVin(c.vin, 'A',1) as assmteamwork,
                                                   c.assemblytypecode,bds.gearboxtypecode,f.forexport,bm.grpcode,bm.aliasname,cg.grpname,
                                                   pt.FNI_GetAsmProdCompanyByVin(c.vin) as companyName,pt.FNI_GetAsmProdCompanyCodeByVin(c.vin) as companycode
                                                   ,ft.fitypename,co.clralias,gbt.gearboxtypedesc,
                                                     c.prodenddate,
                                                     TO_char(prodenddate,'YYYY/MM/DD HH24:MI:SS','nls_calendar=persian') as prodenddate_Fa,
                                                     TO_char(joinerydate,'YYYY/MM/DD HH24:MI:SS','nls_calendar=persian') as joinerydate_fa
                                            from
                                                   carid c 
                                                   left join bodymodel bm on c.bdmdlcode = bm.bdmdlcode
                                                   left join pt.bodystyle bds on bds.bdstlcode = c.bdstlcode
                                                   left join finalqc f  on c.finqccode = f.finqccode
                                                   left join cargroup cg on cg.grpcode=bm.GRPCODE
                                                   left join pt.fueltype ft on c.fitypecode = ft.fitypecode
                                                   left join color co on co.clrcode = c.clrcode
                                                   join pt.gearboxtype gbt on gbt.gearboxtypecode=bds.gearboxtypecode
                                                   where c.vin ='{0}') z", car.VINWITHOUTCHAR);
                        // 
                        carinfo = clsDBHelper.GetDBObjectByObj(new Car(), null, commandtext).Cast<Car>().ToList();
                        if (carinfo.Count == 1)
                        {
                            carinfo[0].VALIDFORMAT = car.VALIDFORMAT;
                            car.VINWITHOUTCHAR = car.VINWITHOUTCHAR;
                            return carinfo[0];
                        }
                        else
                        {
                            car.MSG ="car not found";
                            return car;
                        }

                    }
                    else
                    {
                        car.MSG = "vin is not valid format";
                        return car;
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
