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


    }


}
