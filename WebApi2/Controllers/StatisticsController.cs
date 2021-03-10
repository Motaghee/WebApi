using Common.Actions;
using Common.CacheManager;
using Common.Models;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Web.Http;
using WebApi.OutputCache.V2;
using Common;
using Common.Utility;
using WebApi2.Controllers.Utility;
using Common.db;
using Common.Models.General;
using System.Globalization;

namespace WebApi2.Controllers
{
    public class StatisticsController : ApiController
    {
        
        // POST: api/qccastt
        [HttpGet]
        [Authorize]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [Route("api/Statistics/GetYearProdStatistics")]
        public List<ProductStatistics> GetYearProdStatistics()//[FromBody] ProductStatistics _ps
        {
            try
            {
                List<ProductStatistics> ListPS;
                // ---
                ListPS = (List<ProductStatistics>)MemoryCacher.GetValue("YearProductStatistics");
                
                if (ListPS == null)
                {
                    ListPS = ldbFetch.GetLiveLdbProductStatistics("Y");
                    if (ListPS == null)
                        return null; //ListPS=StatisticsActs.GetYearProdStatistics("Y");
                    MemoryCacher.Add("YearProductStatistics", ListPS, DateTimeOffset.Now.AddMinutes(1));
                }
                // ---
                return ListPS;
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }

        }
        [HttpGet]
        [Authorize]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [Route("api/Statistics/GetMonthProdStatistics")]
        public List<ProductStatistics> GetMonthProdStatistics()//[FromBody] ProductStatistics _ps
        {
            try
            {
                List<ProductStatistics> ListPS;
                // ---
                ListPS = (List<ProductStatistics>)MemoryCacher.GetValue("MonthProductStatistics");
                if (ListPS == null)
                {
                    ListPS = ldbFetch.GetLiveLdbProductStatistics("M");
                    if (ListPS == null)
                        return null; //ListPS = StatisticsActs.GetYearProdStatistics("M");

                    MemoryCacher.Add("MonthProductStatistics", ListPS, DateTimeOffset.Now.AddMinutes(1));
                }
                // ---
                return ListPS;
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }

        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [Route("api/Statistics/GetDayProdStatistics")]
        [Authorize]
        public  List<ProductStatistics> GetDayProdStatistics()//[FromBody] ProductStatistics _ps
        {
            try
            {
                List<ProductStatistics> ListPS;
                // ---
                ListPS = (List<ProductStatistics>)MemoryCacher.GetValue("DayProductStatistics");
                if (ListPS == null)
                {
                    //LogManager.SetCommonLog("api/Statistics/GetDayProdStatistics_request catch Day is null");
                    ListPS = StatisticsActs.GetLiveProdStatistics("D");
                    if (ListPS == null)
                        return null; // ListPS = StatisticsActs.GetYearProdStatistics("D");
                    MemoryCacher.Add("DayProductStatistics", ListPS, DateTimeOffset.Now.AddMinutes(1));
                }
                //else
                    //LogManager.SetCommonLog("api/Statistics/GetDayProdStatistics_request catch DayNot Null,isCount ="+ ListPS.ToString());
                // ---
                return ListPS;
            }
            catch (Exception e)
            {
                LogManager.SetCommonLog("api/Statistics/GetDayProdStatistics_request Error=" + e.Message.ToString());
                return null;
            }

        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [Route("api/Statistics/GetYDProdStatistics")]
        [Authorize]
        public List<ProductStatistics> GetYDProdStatistics()//[FromBody] ProductStatistics _ps
        {
            try
            {
                List<ProductStatistics> ListPS;
                // ---
                ListPS = (List<ProductStatistics>)MemoryCacher.GetValue("YDProductStatistics");
                if (ListPS == null)
                {
                    ListPS = ldbFetch.GetLiveLdbProductStatistics("YD");
                    if (ListPS == null)
                        return null; // ListPS = StatisticsActs.GetYearProdStatistics("D");
                    MemoryCacher.Add("YDProductStatistics", ListPS, DateTimeOffset.Now.AddMinutes(1));
                }
                // ---
                return ListPS;
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }

        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [Route("api/Statistics/GetO30DProdStatistics")]
        [Authorize]
        public List<ProductStatistics> GetO30DProdStatistics()//[FromBody] ProductStatistics _ps
        {
            try
            {
                List<ProductStatistics> ListPS;
                // ---
                ListPS = (List<ProductStatistics>)MemoryCacher.GetValue("O30DProductStatistics");
                if (ListPS == null)
                {
                    ListPS = ldbFetch.GetLiveLdbProductStatistics("O30D");
                    if (ListPS == null)
                        return null; // ListPS = StatisticsActs.GetYearProdStatistics("D");
                    MemoryCacher.Add("O30DProductStatistics", ListPS, DateTimeOffset.Now.AddMinutes(1));
                }
                // ---
                return ListPS;
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }

        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [Route("api/Statistics/GetO90DProdStatistics")]
        [Authorize]
        public List<ProductStatistics> GetO90DProdStatistics()//[FromBody] ProductStatistics _ps
        {
            try
            {
                List<ProductStatistics> ListPS;
                // ---
                ListPS = (List<ProductStatistics>)MemoryCacher.GetValue("O90DProductStatistics");
                if (ListPS == null)
                {
                    ListPS = ldbFetch.GetLiveLdbProductStatistics("O90D");
                    if (ListPS == null)
                        return null; // ListPS = StatisticsActs.GetYearProdStatistics("D");
                    MemoryCacher.Add("O90DProductStatistics", ListPS, DateTimeOffset.Now.AddMinutes(1));
                }
                // ---
                return ListPS;
            }
            catch (Exception e)
            {
                DBHelper.LogFile(e);
                return null;
            }

        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [Route("api/Statistics/GetO180DProdStatistics")]
        [Authorize]
        public List<ProductStatistics> GetO180DProdStatistics()//[FromBody] ProductStatistics _ps
        {
            try
            {
                List<ProductStatistics> ListPS;
                // ---
                ListPS = (List<ProductStatistics>)MemoryCacher.GetValue("O180DProductStatistics");
                if (ListPS == null)
                {
                    ListPS = ldbFetch.GetLiveLdbProductStatistics("O180D");
                    if (ListPS == null)
                        return null; // ListPS = StatisticsActs.GetYearProdStatistics("D");
                    MemoryCacher.Add("O180DProductStatistics", ListPS, DateTimeOffset.Now.AddMinutes(1));
                }
                // ---
                return ListPS;
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }

        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [Route("api/Statistics/GetArchiveProdStatistics")]
        [Authorize]
        public List<ProductStatistics> GetArchiveProdStatistics()//[FromBody] ProductStatistics _ps
        {
            try
            {
                List<ProductStatistics> ListPS;
                // ---
                ListPS = (List<ProductStatistics>)MemoryCacher.GetValue("ArchiveProductStatistics");
                if (ListPS == null)
                {
                    ListPS = ldbFetch.GetArchiveLdbProductStatistics(365);
                    if (ListPS == null)
                        return null; // ListPS = StatisticsActs.GetArchiveProdStatistics("");
                    MemoryCacher.Add("ArchiveProductStatistics", ListPS, DateTimeOffset.Now.AddMinutes(1));
                }
                // ---
                return ListPS;
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }

        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [Route("api/Statistics/GetArchiveO30DProdStatistics")]
        [Authorize]
        public List<ProductStatistics> GetArchiveO30DProdStatistics()//[FromBody] ProductStatistics _ps
        {
            try
            {
                List<ProductStatistics> ListPS;
                // ---
                ListPS = (List<ProductStatistics>)MemoryCacher.GetValue("ArchiveO30DProductStatistics");
                if (ListPS == null)
                {
                    ListPS = ldbFetch.GetArchiveLdbProductStatistics(30);
                    if (ListPS == null)
                        return null; // ListPS = StatisticsActs.GetArchiveProdStatistics("");
                    MemoryCacher.Add("ArchiveO30DProductStatistics", ListPS, DateTimeOffset.Now.AddMinutes(1));
                }
                // ---
                return ListPS;
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }

        }


        [HttpGet]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [Authorize]
        [Route("api/Statistics/GetCompanyArchiveProdStatistics")]
        public List<CompanyProductStatistics> GetCompanyArchiveProdStatistics()//[FromBody] ProductStatistics _ps
        {
            try
            {
                List<CompanyProductStatistics> ListPS;
                // ---
                ListPS = (List<CompanyProductStatistics>)MemoryCacher.GetValue("CompanyArchiveProductStatistics");
                if (ListPS == null)
                {
                    ListPS = ldbFetch.GetArchiveLdbCompanyProductStatistics(365);
                    if (ListPS == null)
                        return null; // ListPS = StatisticsActs.GetArchiveProdStatistics("");
                    MemoryCacher.Add("CompanyArchiveProductStatistics", ListPS, DateTimeOffset.Now.AddMinutes(1));
                }
                // ---
                return ListPS;
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }

        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [Authorize]
        [Route("api/Statistics/GetGroupArchiveProdStatistics")]
        public List<GroupProductStatistics> GetGroupArchiveProdStatistics()//[FromBody] ProductStatistics _ps
        {
            try
            {
                List<GroupProductStatistics> ListPS;
                // ---
                ListPS = (List<GroupProductStatistics>)MemoryCacher.GetValue("GroupArchiveProductStatistics");
                if (ListPS == null)
                {
                    ListPS = ldbFetch.GetArchiveLdbGroupProductStatistics(365);
                    if (ListPS == null)
                        return null; // ListPS = StatisticsActs.GetArchiveProdStatistics("");
                    MemoryCacher.Add("GroupArchiveProductStatistics", ListPS, DateTimeOffset.Now.AddMinutes(1));
                }
                // ---
                return ListPS;
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }

        }


        [HttpGet]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [Authorize]
        [Route("api/Statistics/GetCompanyArchiveO30DProdStatistics")]
        public List<CompanyProductStatistics> GetCompanyArchiveO30DProdStatistics()//[FromBody] ProductStatistics _ps
        {
            try
            {
                List<CompanyProductStatistics> ListPS;
                // ---
                ListPS = (List<CompanyProductStatistics>)MemoryCacher.GetValue("CompanyArchiveO30DProductStatistics");
                if (ListPS == null)
                {
                    ListPS = ldbFetch.GetArchiveLdbCompanyProductStatistics(30);
                    if (ListPS == null)
                        return null; // ListPS = StatisticsActs.GetArchiveProdStatistics("");
                    MemoryCacher.Add("CompanyArchiveO30DProductStatistics", ListPS, DateTimeOffset.Now.AddMinutes(1));
                }
                // ---
                return ListPS;
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }

        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [Authorize]
        [Route("api/Statistics/GetGroupArchiveO30DProdStatistics")]
        public List<GroupProductStatistics> GetGroupArchiveO30DProdStatistics()//[FromBody] ProductStatistics _ps
        {
            try
            {
                List<GroupProductStatistics> ListPS;
                // ---
                ListPS = (List<GroupProductStatistics>)MemoryCacher.GetValue("GroupArchiveO30DProductStatistics");
                if (ListPS == null)
                {
                    ListPS = ldbFetch.GetArchiveLdbGroupProductStatistics(30);
                    if (ListPS == null)
                        return null; // ListPS = StatisticsActs.GetArchiveProdStatistics("");
                    MemoryCacher.Add("GroupArchiveO30DProductStatistics", ListPS, DateTimeOffset.Now.AddMinutes(1));
                }
                // ---
                return ListPS;
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }

        }



        [HttpGet]
        [Authorize]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [Route("api/Statistics/GetLiveProdStatistics")]
        public List<ProductStatistics> GetLiveProdStatistics()//[FromBody] ProductStatistics _ps
        {
            try
            {
                //LogManager.SetCommonLog("api/Statistics/GetLiveProdStatistics_request start");
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
                //LogManager.SetCommonLog("api/Statistics/GetLiveProdStatistics_request end");
                // ---
                return List;
            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("api/Statistics/GetLiveProdStatistics_ERROR "+ex.Message.ToString());
                return null;
            }
        }


        [HttpGet]
        [Authorize]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [Route("api/Statistics/GetLiveBodyModelProdStatistics")]
        public List<BodyModelProductStatistics> GetLiveBodyModelProdStatistics()//[FromBody] ProductStatistics _ps
        {
            try
            {
                //LogManager.SetCommonLog("api/Statistics/GetLiveBodyModelProdStatistics_request start");
                List<BodyModelProductStatistics> List = new List<BodyModelProductStatistics>();
                //--
                List<BodyModelProductStatistics> ListPSD = ldbFetch.GetLiveLdbBodyModelProductStatistics("D");
                List<BodyModelProductStatistics> ListPSM = ldbFetch.GetLiveLdbBodyModelProductStatistics("M");
                List<BodyModelProductStatistics> ListPSY = ldbFetch.GetLiveLdbBodyModelProductStatistics("Y");
                List<BodyModelProductStatistics> ListPYD = ldbFetch.GetLiveLdbBodyModelProductStatistics("YD");
                List<BodyModelProductStatistics> ListPO30D = ldbFetch.GetLiveLdbBodyModelProductStatistics("O30D");
                List<BodyModelProductStatistics> ListPO90D = ldbFetch.GetLiveLdbBodyModelProductStatistics("O90D");
                List<BodyModelProductStatistics> ListPO180D = ldbFetch.GetLiveLdbBodyModelProductStatistics("O180D");
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
                //LogManager.SetCommonLog("api/Statistics/GetLiveBodyModelProdStatistics_request end");
                // ---
                return List;
            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("api/Statistics/GetLiveBodyModelProdStatistics " + ex.Message.ToString());
                return null;
            }
        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        [Authorize]
        [Route("api/Statistics/GetQCArchiveBodyStatistics")]
        public object GetQCArchiveBodyStatistics()//[FromBody] ProductStatistics _ps
        {
            BsonArray bTypeAreaArray = new BsonArray();
            bTypeAreaArray.Add(10);
            return ldbFetch.GetQCAreaStatistics(-1,bTypeAreaArray);
        }


        [HttpGet]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [Authorize]
        [Route("api/Statistics/GetQCTodayBodyStatistics")]
        public object GetQCTodayBodyStatistics()//[FromBody] ProductStatistics _ps
        {
            BsonArray bTypeAreaArray = new BsonArray();
            bTypeAreaArray.Add(10);
            return ldbFetch.GetQCAreaStatistics(0, bTypeAreaArray);
        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        [Authorize]
        [Route("api/Statistics/GetQCArchivePaintStatistics")]
        public object GetQCArchivePaintStatistics()//[FromBody] ProductStatistics _ps
        {
            BsonArray bTypeAreaArray = new BsonArray();
            bTypeAreaArray.Add(20);
            return ldbFetch.GetQCAreaStatistics(-1, bTypeAreaArray);
        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [Authorize]
        [Route("api/Statistics/GetQCTodayPaintStatistics")]
        public object GetQCTodayPaintStatistics()//[FromBody] ProductStatistics _ps
        {
            BsonArray bTypeAreaArray = new BsonArray();
            bTypeAreaArray.Add(20);
            return ldbFetch.GetQCAreaStatistics(0, bTypeAreaArray);
        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        [Authorize]
        [Route("api/Statistics/GetQCArchiveAsmStatistics")]
        public object GetQCArchiveAsmStatistics()//[FromBody] ProductStatistics _ps
        {
            BsonArray bTypeAreaArray = new BsonArray();
            bTypeAreaArray.Add(30);
            return ldbFetch.GetQCAreaStatistics(-1, bTypeAreaArray);
        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [Authorize]
        [Route("api/Statistics/GetQCTodayAsmStatistics")]
        public object GetQCTodayAsmStatistics()//[FromBody] ProductStatistics _ps
        {
            BsonArray bTypeAreaArray = new BsonArray();
            bTypeAreaArray.Add(30);
            return ldbFetch.GetQCAreaStatistics(0, bTypeAreaArray);
        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        [Authorize]
        [Route("api/Statistics/GetQCArchiveTouchUpStatistics")]
        public object GetQCArchiveTouchUpStatistics()//[FromBody] ProductStatistics _ps
        {
            BsonArray bTypeAreaArray = new BsonArray();
            bTypeAreaArray.Add(40);
            return ldbFetch.GetQCAreaStatistics(-1, bTypeAreaArray);
        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [Authorize]
        [Route("api/Statistics/GetQCTodayTouchUpStatistics")]
        public object GetQCTodayTouchUpStatistics()//[FromBody] ProductStatistics _ps
        {
            BsonArray bTypeAreaArray = new BsonArray();
            bTypeAreaArray.Add(40);
            return ldbFetch.GetQCAreaStatistics(0, bTypeAreaArray);
        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        [Authorize]
        [Route("api/Statistics/GetQCArchivePDIStatistics")]
        public object GetQCArchivePDIStatistics()//[FromBody] ProductStatistics _ps
        {
            BsonArray bTypeAreaArray = new BsonArray();
            bTypeAreaArray.Add(50);
            return ldbFetch.GetQCAreaStatistics(-1, bTypeAreaArray);
        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [Authorize]
        [Route("api/Statistics/GetQCTodayPDIStatistics")]
        public object GetQCTodayPDIStatistics()//[FromBody] ProductStatistics _ps
        {
            BsonArray bTypeAreaArray = new BsonArray();
            bTypeAreaArray.Add(50);
            return ldbFetch.GetQCAreaStatistics(0, bTypeAreaArray);
        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [Authorize]
        [Route("api/Statistics/GetQCHAreaStatistics")]
        public object GetQCHStatistics()//[FromBody] ProductStatistics _ps
        {
            return ldbFetch.GetQCHAreaStatistics();
        }


        [HttpGet]
        [Authorize]
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [Route("api/Statistics/GetTodayASPCarDef")]
        public object GetTodayASPCarDef()//[FromBody] ProductStatistics _ps
        {
            return ldbFetch.GetTodayASPCarDef();
        }

        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        [Authorize]
        [Route("api/Statistics/GetSaipaAuditArchiveStatistics")]
        public object GetSaipaAuditArchiveStatistics()//[FromBody] ProductStatistics _ps
        {
            BsonArray bTypeAreaArray = new BsonArray();
            bTypeAreaArray.Add(1000);
            return ldbFetch.GetAuditStatistics(-1, bTypeAreaArray);
        }

        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        [Authorize]
        [Route("api/Statistics/GetParskhodroAuditArchiveStatistics")]
        public object GetParskhodroAuditArchiveStatistics()//[FromBody] ProductStatistics _ps
        {
            BsonArray bTypeAreaArray = new BsonArray();
            bTypeAreaArray.Add(1001);
            return ldbFetch.GetAuditStatistics(-1, bTypeAreaArray);
        }

        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        [Authorize]
        [Route("api/Statistics/GetCitroenAuditArchiveStatistics")]
        public object GetCitroenAuditArchiveStatistics()//[FromBody] ProductStatistics _ps
        {
            BsonArray bTypeAreaArray = new BsonArray();
            bTypeAreaArray.Add(1002);
            return ldbFetch.GetAuditStatistics(-1, bTypeAreaArray);
        }

        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        [Authorize]
        [Route("api/Statistics/GetBonroAuditArchiveStatistics")]
        public object GetBonroAuditArchiveStatistics()//[FromBody] ProductStatistics _ps
        {
            BsonArray bTypeAreaArray = new BsonArray();
            bTypeAreaArray.Add(1003);
            return ldbFetch.GetAuditStatistics(-1, bTypeAreaArray);
        }

        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [Authorize]
        [Route("api/Statistics/GetSaipaAuditTodayStatistics")]
        public object GetSaipaAuditTodayStatistics()//[FromBody] ProductStatistics _ps
        {
            BsonArray bTypeAreaArray = new BsonArray();
            bTypeAreaArray.Add(1000);
            return ldbFetch.GetAuditStatistics(0, bTypeAreaArray);
        }

        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [Authorize]
        [Route("api/Statistics/GetParskhodroAuditTodayStatistics")]
        public object GetParskhodroAuditTodayStatistics()//[FromBody] ProductStatistics _ps
        {
            BsonArray bTypeAreaArray = new BsonArray();
            bTypeAreaArray.Add(1001);
            return ldbFetch.GetAuditStatistics(0, bTypeAreaArray);
        }
        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [Authorize]
        [Route("api/Statistics/GetCitroenAuditTodayStatistics")]
        public object GetCitroenAuditTodayStatistics()//[FromBody] ProductStatistics _ps
        {
            BsonArray bTypeAreaArray = new BsonArray();
            bTypeAreaArray.Add(1002);
            return ldbFetch.GetAuditStatistics(0, bTypeAreaArray);
        }

        [CacheOutput(ClientTimeSpan = 60, ServerTimeSpan = 60)]
        [Authorize]
        [Route("api/Statistics/GetBonroAuditTodayStatistics")]
        public object GetBonroAuditTodayStatistics()//[FromBody] ProductStatistics _ps
        {
            BsonArray bTypeAreaArray = new BsonArray();
            bTypeAreaArray.Add(1003);
            return ldbFetch.GetAuditStatistics(0, bTypeAreaArray);
        }


        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        [Authorize]
        [Route("api/Statistics/GetSaipaAuditStatisticsMDTrend")]
        public object GetSaipaAuditStatisticsMDTrend()//[FromBody] ProductStatistics _ps
        {
            BsonArray bTypeAreaArray = new BsonArray();
            bTypeAreaArray.Add(841);
            return ldbFetch.GetAuditStatisticsMDTrend(bTypeAreaArray);
        }

        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        [Authorize]
        [Route("api/Statistics/GetParskhodroAuditStatisticsMDTrend")]
        public object GetParskhodroAuditStatisticsMDTrend()//[FromBody] ProductStatistics _ps
        {
            BsonArray bTypeAreaArray = new BsonArray();
            bTypeAreaArray.Add(961);
            return ldbFetch.GetAuditStatisticsMDTrend(bTypeAreaArray);
        }
        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        [Authorize]
        [Route("api/Statistics/GetCitroenAuditStatisticsMDTrend")]
        public object GetCitroenAuditStatisticsMDTrend()//[FromBody] ProductStatistics _ps
        {
            BsonArray bTypeAreaArray = new BsonArray();
            bTypeAreaArray.Add(962);
            return ldbFetch.GetAuditStatisticsMDTrend(bTypeAreaArray);
        }

        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        [Authorize]
        [Route("api/Statistics/GetBonroAuditStatisticsMDTrend")]
        public object GetBonroAuditStatisticsMDTrend()//[FromBody] ProductStatistics _ps
        {
            BsonArray bTypeAreaArray = new BsonArray();
            bTypeAreaArray.Add(963);
            return ldbFetch.GetAuditStatisticsMDTrend(bTypeAreaArray);
        }

        [HttpPost]
        [Route("api/Statistics/GetCarStatus")]
        public object GetCarStatusByFilters([FromBody] FilterCarStatus _filterCarStatus)// _ps
        {
            return ldbFetch.GetCarStatus(_filterCarStatus);
        }

        [HttpGet]
        [Route("api/Statistics/GetTest")]
        public object GetTest()//
        {
            try
            {
                //string strToday = CommonUtility.GetNowDateFaNum();
                //bool l= ldbRefresh.RefreshLdbAuditStatistics(true);
                //l.ArchiveCompanyProductStatistics_UDate = strToday;
                //l.ArchiveGroupProductStatistics_UDate = strToday;
                //l.ArchiveQCStatistics_UDate = strToday;
                // l.ArchiveASPQCCASTT = strToday;
                // l.ArchiveGenerateQCmdDPU = strToday;
                //bool result=ldbRefresh.SetLdbUpdateStatus(l);
                //return ldbRefresh.GenerateQCCaridDetailsNonBrand();
                DateTime Tthen = DateTime.Now;
                //do
                //{

                //} while (Tthen.AddSeconds(10) > DateTime.Now);
                //Thread.Sleep(5000);
                //return GetDBObjectByObj22(_Obj, _ds, _CommandText, strSchema);
                //bool RefQccasttArchive = ldbRefresh.RefreshLdbASPQCCASTT(true);
                //StatisticsActs.GetArchiveMDTrendAuditStatistics("841"); 
                return Tthen.ToString()+ " " +DateTime.Now.ToString();
                //GetLiveBodyModelProdStatistics()
                //string[] s = new string[1];
                //s[0] = "1000861";
                //int i = CommonUtility.SendAutomationAtachExcel("subtst","contenttst",s,s, DBHelper.ToDataTable(UserManager.SeedData()),"tstfileName");
                //return RefQCToday;// (5,82);
                //return l;
                //return GetBonroAuditArchiveStatistics();

            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return false;
            }
        }




    }

   
}
