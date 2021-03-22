using Common.Models;
using Common.Models.Qccastt;
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
        [Route("api/Qccastt/DefectDetect2")]
        public ResultMsg DefectDetect2()
        {
            Qccastt qccastt = new Qccastt();
            qccastt.Srl = 26904286;
            qccastt.Vin = "NAS411100G1205277";
            qccastt.ActBy = 4314;
            qccastt.ActAreaSrl = 94;
            qccastt.QCMdult_Srl = 716;
            qccastt.QCBadft_Srl = 2024;
            qccastt.RecordOwner = 1;
            qccastt.IsRepaired = 1;
            qccastt.InUse = 1;
            qccastt.CreatedBy = 4314;
            qccastt.CHECKLISTAREA_SRL =94;

            return QccasttUtility.QCCASTT_DefectDetect(qccastt);
        }

        //[HttpGet]
        //[Route("api/Qccastt/DeleteQccastt2")]
        //public ResultMsg DeleteQccastt2()
        //{
        //    Qccastt qccastt = new Qccastt();
        //    qccastt.Srl = 26904272;
        //    qccastt.Vin = "NAS411100G1205277";
        //    qccastt.ActBy = 4314;
        //    qccastt.ActAreaSrl = 94;
        //    //qccastt.Vin = "NAS411100G1205277";
        //    return QccasttUtility.Delete_QCCASTT(qccastt);
        //}

        [HttpPost]
        [Route("api/Qccastt/CarSend")]
        public ResultMsg CarSend([FromBody] CarSend carsend)
        {
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
