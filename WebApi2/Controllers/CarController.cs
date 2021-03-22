using System;
using System.Web.Http;
using WebApi2.Controllers.Utility;
using WebApi2.Models;

namespace WebApi2.Controllers
{
    public class CarController : ApiController
    {


        // POST
        [HttpPost]
        [Route("api/car/GetCarInfo")]
        public Car GetCarInfo([FromBody] Car car)
        {
            return CarUtility.GetCarInfo(car);
        }
        [HttpGet]
        [Route("api/car/GetCarInfotst")]
        public Car GetCarInfotst()
        {
            try
            {
                Car car = new Car();
                car.Vin = "NAS411100K1146021";
                car.ActBy = 4314;
                car.ActAreaSrl = 94;
                return CarUtility.GetCarInfo(car);
            }
            catch (Exception ex)
            {
                return null;
            }

        }


    }


}
