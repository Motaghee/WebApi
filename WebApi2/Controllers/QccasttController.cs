﻿using Common.Models;
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

        //[HttpGet]
        //[Route("api/Qccastt/CarDefect2")]
        //public List<Qccastt> CarDefect2()
        //{
        //    Qccastt qccastt = new Qccastt();
        //    qccastt.Vin = "NAS111100M1000870";
        //    return QccasttUtility.GetCarDefect(qccastt);
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
