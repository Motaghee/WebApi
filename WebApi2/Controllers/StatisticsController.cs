using Common.CacheManager;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;
using WebApi.OutputCache.V2;

namespace WebApi2.Controllers
{
    public class StatisticsController : ApiController
    {
        // POST: api/qccastt
        [HttpGet]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [Route("api/Statistics/GetYearProdStatistics")]
        public List<ProductStatistics> GetYearProdStatistics()//[FromBody] ProductStatistics _ps
        {
            try
            {
                List<ProductStatistics> ListPS;
                MemoryCacher mc = new MemoryCacher();
                // ---
                ListPS = (List<ProductStatistics>)mc.GetValue("YearProductStatistics");
                if (ListPS == null)
                {
                    ListPS = ldbFetch.GetLiveLdbProductStatistics("Y");
                    if (ListPS == null)
                        return null; //ListPS=StatisticsActs.GetYearProdStatistics("Y");
                    mc.Add("YearProductStatistics", ListPS, DateTimeOffset.Now.AddMinutes(2));
                }
                // ---
                return ListPS;
            }
            catch (Exception e)
            {
                return null;
            }

        }
        [HttpGet]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [Route("api/Statistics/GetMonthProdStatistics")]
        public List<ProductStatistics> GetMonthProdStatistics()//[FromBody] ProductStatistics _ps
        {
            try
            {
                List<ProductStatistics> ListPS;
                MemoryCacher mc = new MemoryCacher();
                // ---
                ListPS = (List<ProductStatistics>)mc.GetValue("MonthProductStatistics");
                if (ListPS == null)
                {
                    ListPS = ldbFetch.GetLiveLdbProductStatistics("M");
                    if (ListPS == null)
                        return null; //ListPS = StatisticsActs.GetYearProdStatistics("M");

                    mc.Add("MonthProductStatistics", ListPS, DateTimeOffset.Now.AddMinutes(2));
                }
                // ---
                return ListPS;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [Route("api/Statistics/GetDayProdStatistics")]
        public  List<ProductStatistics> GetDayProdStatistics()//[FromBody] ProductStatistics _ps
        {
            try
            {
                List<ProductStatistics> ListPS;
                MemoryCacher mc = new MemoryCacher();
                // ---
                ListPS = (List<ProductStatistics>)mc.GetValue("DayProductStatistics");
                if (ListPS == null)
                {
                    ListPS = ldbFetch.GetLiveLdbProductStatistics("D");
                    if (ListPS == null)
                        return null; // ListPS = StatisticsActs.GetYearProdStatistics("D");
                    mc.Add("DayProductStatistics", ListPS, DateTimeOffset.Now.AddMinutes(2));
                }
                // ---
                return ListPS;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [Route("api/Statistics/GetYDProdStatistics")]
        public List<ProductStatistics> GetYDProdStatistics()//[FromBody] ProductStatistics _ps
        {
            try
            {
                List<ProductStatistics> ListPS;
                MemoryCacher mc = new MemoryCacher();
                // ---
                ListPS = (List<ProductStatistics>)mc.GetValue("YDProductStatistics");
                if (ListPS == null)
                {
                    ListPS = ldbFetch.GetLiveLdbProductStatistics("YD");
                    if (ListPS == null)
                        return null; // ListPS = StatisticsActs.GetYearProdStatistics("D");
                    mc.Add("YDProductStatistics", ListPS, DateTimeOffset.Now.AddMinutes(2));
                }
                // ---
                return ListPS;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [Route("api/Statistics/GetO30DProdStatistics")]
        public List<ProductStatistics> GetO30DProdStatistics()//[FromBody] ProductStatistics _ps
        {
            try
            {
                List<ProductStatistics> ListPS;
                MemoryCacher mc = new MemoryCacher();
                // ---
                ListPS = (List<ProductStatistics>)mc.GetValue("O30DProductStatistics");
                if (ListPS == null)
                {
                    ListPS = ldbFetch.GetLiveLdbProductStatistics("O30D");
                    if (ListPS == null)
                        return null; // ListPS = StatisticsActs.GetYearProdStatistics("D");
                    mc.Add("O30DProductStatistics", ListPS, DateTimeOffset.Now.AddMinutes(2));
                }
                // ---
                return ListPS;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [Route("api/Statistics/GetO90DProdStatistics")]
        public List<ProductStatistics> GetO90DProdStatistics()//[FromBody] ProductStatistics _ps
        {
            try
            {
                List<ProductStatistics> ListPS;
                MemoryCacher mc = new MemoryCacher();
                // ---
                ListPS = (List<ProductStatistics>)mc.GetValue("O90DProductStatistics");
                if (ListPS == null)
                {
                    ListPS = ldbFetch.GetLiveLdbProductStatistics("O90D");
                    if (ListPS == null)
                        return null; // ListPS = StatisticsActs.GetYearProdStatistics("D");
                    mc.Add("O90DProductStatistics", ListPS, DateTimeOffset.Now.AddMinutes(2));
                }
                // ---
                return ListPS;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [Route("api/Statistics/GetO180DProdStatistics")]
        public List<ProductStatistics> GetO180DProdStatistics()//[FromBody] ProductStatistics _ps
        {
            try
            {
                List<ProductStatistics> ListPS;
                MemoryCacher mc = new MemoryCacher();
                // ---
                ListPS = (List<ProductStatistics>)mc.GetValue("O180DProductStatistics");
                if (ListPS == null)
                {
                    ListPS = ldbFetch.GetLiveLdbProductStatistics("O180D");
                    if (ListPS == null)
                        return null; // ListPS = StatisticsActs.GetYearProdStatistics("D");
                    mc.Add("O180DProductStatistics", ListPS, DateTimeOffset.Now.AddMinutes(2));
                }
                // ---
                return ListPS;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        [Route("api/Statistics/GetArchiveProdStatistics")]
        public List<ProductStatistics> GetArchiveProdStatistics()//[FromBody] ProductStatistics _ps
        {
            try
            {
                List<ProductStatistics> ListPS;
                MemoryCacher mc = new MemoryCacher();
                // ---
                ListPS = (List<ProductStatistics>)mc.GetValue("ArchiveProductStatistics");
                if (ListPS == null)
                {
                    ListPS = ldbFetch.GetArchiveLdbProductStatistics();
                    if (ListPS == null)
                        return null; // ListPS = StatisticsActs.GetArchiveProdStatistics("");
                    mc.Add("ArchiveProductStatistics", ListPS, DateTimeOffset.Now.AddMinutes(60));
                }
                // ---
                return ListPS;
            }
            catch (Exception e)
            {
                return null;
            }

        }



        [HttpGet]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [Route("api/Statistics/GetProdStatistics")]
        public List<ProductStatistics> GetProdStatistics()//[FromBody] ProductStatistics _ps
        {
            try
            {
                List<ProductStatistics> List = new List<ProductStatistics>();
                List<ProductStatistics> ListPSD = GetDayProdStatistics();
                List<ProductStatistics> ListPSM = GetMonthProdStatistics();
                List<ProductStatistics> ListPSY = GetYearProdStatistics(); ;
                List<ProductStatistics> ListPYD = GetYDProdStatistics();
                List<ProductStatistics> ListPO30D = GetO30DProdStatistics();
                List<ProductStatistics> ListPO90D = GetO90DProdStatistics();
                List<ProductStatistics> ListPO180D = GetO180DProdStatistics();
                if ((ListPSD != null) && (ListPSD.Count != 0))
                    List.AddRange(ListPSD);
                if ((ListPSM != null) && (ListPSM.Count != 0))
                    List.AddRange(ListPSM);
                if ((ListPSY != null) && (ListPSY.Count != 0))
                    List.AddRange(ListPSY);
                if ((ListPYD != null) && (ListPYD.Count != 0))
                    List.AddRange(ListPYD);
                if ((ListPO30D != null) && (ListPO30D.Count != 0))
                    List.AddRange(ListPO30D);
                if ((ListPO90D != null) && (ListPO90D.Count != 0))
                    List.AddRange(ListPO90D);
                if ((ListPO180D != null) && (ListPO180D.Count != 0))
                    List.AddRange(ListPO180D);
                return List;
            }
            catch (Exception ex)
            {
                return null;
            }
        }






    }
}
