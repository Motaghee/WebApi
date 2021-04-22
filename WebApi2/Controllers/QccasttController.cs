using Common.Models;
using Common.Models.Car;
using Common.Models.General;
using Common.Models.PT;
using Common.Models.QccasttModels;
using Common.Utility;
using System;
using System.Collections.Generic;
using System.Web.Http;
using WebApi.OutputCache.V2;
using WebApi2.Controllers.Utility;
using WebApi2.Models;

namespace WebApi2.Controllers
{
    public class QccasttController : ApiController
    {


        [HttpPost]
        [Route("api/Qccastt/CarDefect")]
        public List<Qccastt> CarDefect([FromBody] Qccastt qccastt)
        {
            return QccasttUtility.GetCarDefect(qccastt);
        }

        [HttpGet]
        [Route("api/Qccastt/tstCarDefect")]
        public List<Qccastt> tstCarDefect()
        {
            Qccastt q = new Qccastt();
            q.Vin = "NAS411100G1205277";
            //q.deletedby = 2621;
            //q.DateType = "M";
            q.ActAreaSrl = 94;
            q.ActBy = 4314;
            return QccasttUtility.GetCarDefect(q);
        }


        [HttpPost]
        [Route("api/Qccastt/DeleteQccastt")]
        public ResultMsg DeleteQccastt([FromBody] Qccastt qccastt)
        {
            return QccasttUtility.Delete_QCCASTT(qccastt);
        }

        [HttpPost]
        [Route("api/Qccastt/DefectDetect")]
        public ResultMsg DefectDetect([FromBody] Qccastt qccastt)
        {
            return QccasttUtility.QCCASTT_DefectDetect(qccastt);
        }

        [HttpPost]
        [Route("api/Qccastt/UserSammary")]
        public List<Summary> UserSammary([FromBody] Qccastt qccastt)
        {
            return QccasttUtility.GetUserSammary(qccastt.ActAreaSrl, qccastt.ActAreaSrl, 0, qccastt.RepairedByDesc);
        }

        [HttpGet]
        [Route("api/Qccastt/TSTUserSammary")]
        public List<Summary> UserSammary2()
        {
            return QccasttUtility.GetUserSammary(94, 4314, 0,"D");
        }

        [HttpPost]
        [Route("api/Qccastt/CarSend")]
        public ResultMsg CarSend([FromBody] CarSend carsend)
        {
            return QccasttUtility.InsertQcqctrt(carsend);
        }

        [HttpPost]
        [Route("api/Qccastt/PDIConfirm")]
        public ResultMsg PDIConfirm([FromBody] Qccastt qccastt)
        {
            return QccasttUtility.PDIConfirm(qccastt);
        }

        [HttpGet]
        [Route("api/Qccastt/CarSendtest")]
        public ResultMsg CarSendtest()
        {
            CarSend carsend = new CarSend();
            carsend.UserId = 1000861;
            carsend.Vin = "NAS411100G1205277";
            carsend.QCUsertSrl = 4314;
            carsend.FromAreaSrl = 461;
            carsend.ToAreaSrl = 94;
            return QccasttUtility.InsertQcqctrt(carsend);
        }

        [HttpPost]
        [Route("api/Qccastt/UpdateCarImage")]
        public CarImage UpdateCarImage([FromBody] CarImage carImage)
        {
            return QccasttUtility.UpdateCarImage(carImage);
        }

        [HttpPost]
        [Route("api/Qccastt/GetCarImages")]
        public List<CarImage> GetCarImages([FromBody] CarImage car)
        {
            return QccasttUtility.GetCarImages(car);
        }

        [HttpGet]
        [Route("api/Qccastt/GetBaseModuleList")]
        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        public List<Module> GetBaseModuleList()
        {
            List<Module> lstM = new List<Module>();
            try
            {
                lstM = QccasttUtility.GetBaseModuleList();
                return lstM;
            }
            catch (Exception e)
            {
                Module m = new Module();
                m.ModuleName = e.Message + "__" + e.InnerException.Message.ToString();
                m.ModuleCode = 0;
                m.Srl = 0;
                lstM.Add(m);
                return lstM;
            }
        }

        [HttpGet]
        [Route("api/Qccastt/GetBaseDefectList")]
        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        public List<Defect> GetBaseDefectList()
        {
            return QccasttUtility.GetBaseDefectList();
        }

        [HttpGet]
        [Route("api/Qccastt/GetBaseStrengthList")]
        [CacheOutput(ClientTimeSpan = 86400, ServerTimeSpan = 86400)]
        public List<Strength> GetBaseStrengthList()
        {
            return QccasttUtility.GetBaseStrengthList();
        }


        [HttpPost]
        [Route("api/Qccastt/GetAreaCheckList")]
        public List<Qcdfctt> GetAreaCheckList([FromBody]Area _area)
        {
            return QccasttUtility.GetAreaCheckList(_area);
        }

        [HttpGet]
        [Route("api/Qccastt/GetQcdsart")]
        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        public List<Qcdsart> GetQcdsart()
        {
            return QccasttUtility.GetQcdsart();
        }

        [HttpGet]
        [Route("api/Qccastt/GetBaseAreaList")]
        [CacheOutput(ClientTimeSpan = 86400, ServerTimeSpan = 86400)]
        public List<Area> GetArea()
        {
            return QccasttUtility.GetBaseAreaList();
        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 86400, ServerTimeSpan = 86400)]
        [Route("api/Qccastt/GetBaseShopList")]
        public List<Shop> GetShop()
        {
            return QccasttUtility.GetBaseShopList();
        }

        
        [HttpGet]
        [CacheOutput(ClientTimeSpan = 86400, ServerTimeSpan = 86400)]
        [Route("api/Qccastt/GetBaseCarGroupList")]
        public List<CarGroup> GetCarGroup()
        {
            return QccasttUtility.GetBaseCarGroupList();
        }

        [HttpGet]
        [Route("api/Qccastt/GetBaseBodyModelList")]
        [CacheOutput(ClientTimeSpan = 86400, ServerTimeSpan = 86400)]
        public List<BodyModel> GetBodyModel()
        {
            return QccasttUtility.GetBaseBodyModelList();
        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 86400, ServerTimeSpan = 86400)]
        [Route("api/Qccastt/GetBaseSaleStatusList")]
        public List<SaleStatus> GetSaleStatus()
        {
            return QccasttUtility.GetBaseSaleStatusList();
        }

        [HttpGet]
        [CacheOutput(ClientTimeSpan = 86400, ServerTimeSpan = 86400)]
        [Route("api/Qccastt/GetBaseFinalQCList")]
        public List<FinalQC> GetFinalQC()
        {
            return QccasttUtility.GetBaseFinalQCList();
        }





    }
}
