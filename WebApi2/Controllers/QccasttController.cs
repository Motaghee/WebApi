using System;
using System.Collections.Generic;
using System.Web.Http;
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
        public List<Defect> GetBaseDefectList()
        {
            return QccasttUtility.GetBaseDefectList();
        }

        [HttpGet]
        [Route("api/Qccastt/GetBaseStrengthList")]
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
        public List<Qcdsart> GetQcdsart()
        {
            return QccasttUtility.GetQcdsart();
        }

        [HttpGet]
        [Route("api/Qccastt/GetBaseAreaList")]
        public List<Area> GetArea()
        {
            return QccasttUtility.GetBaseAreaList();
        }





    }
}
