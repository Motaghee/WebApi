using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using WebApi2.Controllers.Utility;
using Common.db;
using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using Common.Models;
using Common.Models.General;
using Common.Models.QccasttModels;
using Common.Utility;
using Common.Models.Protection;

namespace WebApi2.Controllers
{
    public class ProtectionController : ApiController
    {
        [HttpPost]
        [Route("api/Protections/Insert")]
        public QCProT Insert([FromBody]  QCProT _qCProT)
        {
            return ProtectionUtility.InsertQCProT(_qCProT);
        }
        [HttpGet]
        [Route("api/Protections/Inserttst")]
        public QCProT InsertTST()
        {
            QCProT _qCProT = new QCProT();
            _qCProT.Vin = "NAS411100G1205275";
            _qCProT.CreatedBy = 6017;
            return ProtectionUtility.InsertQCProT(_qCProT);
        }

    }
}
