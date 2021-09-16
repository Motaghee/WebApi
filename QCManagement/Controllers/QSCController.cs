using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Common.db;
using Common.Models.Car;
using Common.Models.General;
using Common.Models.QccasttModels;
using Common.Models.QSC;
using Common.Utility;
using QCManagement.Models;
using Stimulsoft.Report;
using Stimulsoft.Report.BarCodes;
using Stimulsoft.Report.Components;
using Stimulsoft.Report.Mvc;

namespace QCManagement.Controllers
{
    public class QSCController : Controller
    {
        [Authorize]
        public ActionResult Qccard(CarModels cm)
        {
            if ((cm.Vin == null) || (cm.Vin == ""))
            {
                cm.Vin = "NAS";
            }
            return View(cm);
        }

        [Authorize]
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Back(CarModels cm)
        {
            //return RedirectToAction("Qccard", "Qccastt", cm);
            return View("Qccard", cm);
        }
        [Authorize]
        public ActionResult ReportShow(CarModels cm)
        {
            return View(cm);
        }
        public ActionResult ReportShow2(qscreqt cm)
        {
            return View(cm);
        }

        #region ReportLoad Methods
        [Authorize]
        public ActionResult FromLoadFileReport()
        {
            var formValues = StiMvcViewer.GetFormValues(this.HttpContext);
            CarModels cm = new CarModels();
            if (formValues.Count != 0)
                cm.Vin = formValues.GetValues("vin").GetValue(0).ToString();
            else
                cm.Vin = "NAS0";
            try
            {

                Qccastt qccastt = new Qccastt();
                qccastt.Vin = cm.GetVin();
                DataSet dsCardInfo = new DataSet();
                if (!string.IsNullOrEmpty(cm.Vin))
                {
                    Object[] obj=null;
                    dsCardInfo = QccasttUtility.GetCarDefect(qccastt,out obj);
                    dsCardInfo.Tables[0].TableName = "CarDefects";
                }

                StiReport report = new StiReport();
                report.Load(Server.MapPath("/Content/ReportsFile/QCCard2.mrt"));
                StiText stitxtVin = (StiText)report.GetComponentByName("txtVin");
                stitxtVin.Text = cm.GetVin();
                // BCVin
                StiBarCode stiBCVin = (StiBarCode)report.GetComponentByName("BCVin");
                stiBCVin.Code = new StiBarCodeExpression(cm.GetVin());

                //report.RegBusinessObject("driverReport", dsCardInfo.Tables[0]);
                //int i = dsCardInfo.Tables[0].Rows.Count;
                //report.Compile();

                report.RegData(dsCardInfo);
                return StiMvcViewer.GetReportSnapshotResult(HttpContext, report);
            }
            catch (Exception ex)
            {
                // lblVinMessage.Text = "خطا در دریافت اطلاعات";
                return View();
            }
        }


        public ActionResult ViewerEvent()
        {
            return StiMvcViewer.ViewerEventResult();
        }

        public ActionResult PrintReport()
        {
            var report = StiMvcViewer.GetReportObject();

            // Some actions with report when printing

            return StiMvcViewer.PrintReportResult(report);
        }

        public ActionResult ExportReport()
        {
            //var report = StiMvcViewer.GetReportObject();
            //var parameters = StiMvcViewer.GetRequestParams();

            //if (parameters.ExportFormat == StiExportFormat.Pdf)
            //{
            //    // Some actions with report when exporting to PDF
            //}
            return StiMvcViewer.ExportReportResult(this.HttpContext);
        }

        #endregion

        //[Authorize]
        public ActionResult QSCRequest(qscreqt _qscreqt)
        {
            //if ((cm.Vin == null) || (cm.Vin == ""))
            //{
            //    cm.Vin = "NAS";
            //}
            //_qccastt.SelectedItemIds = new[] { 2, 3 };
            //_qccastt.SelectedItemIds2 = new[] { 2, 3 };
            _qscreqt.LstQscapunt = qscapunt.Get().Cast<qscapunt>().ToList();
            _qscreqt.LstQscreft = qscreft.Get().Cast<qscreft>().ToList();
            _qscreqt.LstQSCDtstt = qscdtstt.Get().Cast<qscdtstt>().ToList();
            _qscreqt.LstQscrqrsnt = qscrsnt.Get().Cast<qscrsnt>().ToList();
            _qscreqt.LstQscscpt = qscscpt.Get().Cast<qscscpt>().ToList();
            _qscreqt.LstQscrqtyt = qscrqtyt.Get().Cast<qscrqtyt>().ToList();
            _qscreqt.LstQscUsers = Users.GetQSCUsers().Cast<Users>().ToList();
            _qscreqt.LstQscordet = qscordet.Get().Cast<qscordet>().ToList();
            _qscreqt.LstQscdebat = qscdebat.Get().Cast<qscdebat>().ToList();
            _qscreqt.LstQscbdmdt = qscbdmdt.Get().Cast<qscbdmdt>().ToList();
            _qscreqt.LstQscrqfistt = qscrqfistt.Get().Cast<qscrqfistt>().ToList();
            _qscreqt.LstQsccocet = qsccocet.Get().Cast<qsccocet>().ToList();
            _qscreqt.Vin = "NAS411100G1205277";
            _qscreqt.FinQCCode = 85;
            //_qccastt.Lstshop = QccasttUtility.GetShop().Cast<Pcshopt>().ToList();
            //_qccastt.LstArea = QccasttUtility.GetArea("").Cast<Area>().ToList();
            //_qccastt.LstCarGroup = CarUtility.GetBaseCarGroupList().Cast<CarGroup>().ToList();
            //_qccastt.LstBodyModel = CarUtility.GetBaseBodyModelList().Cast<BodyModel>().ToList();
            //_qccastt.LstStrength = QccasttUtility.GetBaseStrength().Cast<Strength>().ToList();
            //_qccastt.LstBodyStyle = CarUtility.GetBodyStyle("", "").Cast<BodyStyle>().ToList();
            //_qccastt.LstShift = new List<Shift>();
            //_qccastt.LstShift.Add(new Shift { ShiftName = "A" });
            //_qccastt.LstShift.Add(new Shift { ShiftName = "B" });
            //_qccastt.LstShift.Add(new Shift { ShiftName = "C" });
            return View(_qscreqt);
        }

        [HttpPost]
        public ActionResult RequestInsert(qscreqt qcm)
        {
            qcm.CreatedBy= Convert.ToInt32(Session["SRL"].ToString());
            QSCUtility.InsertQscreqt(qcm);
            return View(qcm);
        }





        #region ComboFillFromJavaCode

        [HttpPost]
        public ActionResult GetJCarInfo(string _Vin)
        {
            Car c = new Car();
            c.Vin = _Vin;
            c=CarUtility.GetCarInfo(c);
            return Json(c, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetArea(string[] QCAreatSrls)
        {
            string Srls = "";
            if (QCAreatSrls != null)
                Srls = string.Join(",", QCAreatSrls);
            SelectList list = CommonUtility.ToSelectList(QccasttUtility.GetdsArea(Srls).Tables[0], "SRL", "AREADESC", false);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetBodyModel(string[] _GrpCode)
        {
            string GrpCode = "";
            if (_GrpCode != null)
                GrpCode = string.Join(",", _GrpCode);
            SelectList list = CommonUtility.ToSelectList(CarUtility.GetdsBaseBodyModelList(GrpCode).Tables[0], "Bdmdlcode", "CommonBodyModelName", false);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetBodyStyle(string[] _GrpCode, string[] _BdmdlCode)
        {
            string GrpCode = "", BdmdlCode = "";
            if (_GrpCode != null)
                GrpCode = string.Join(",", _GrpCode);
            if (_BdmdlCode != null)
                BdmdlCode = string.Join(",", _BdmdlCode);
            SelectList list = CommonUtility.ToSelectList(CarUtility.GetdsBodyStyle(GrpCode, BdmdlCode).Tables[0], "Bdstlcode", "AliasName", false);
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult GetJCarDef(string _Vin)
        {
            Qccastt q = new Qccastt();
            q.Vin = _Vin;
            List<QccasttLight> lstQccasttLight = QccasttUtility.GetCarDefectLight(_Vin);
            //SelectList list = CommonUtility.ToSelectList(QccasttUtility.GetCarDefect(q,).Tables[0], "Bdstlcode", "AliasName", false);
            return Json(lstQccasttLight, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public string SendEmails(string[] SelectedValues)
        //{
        //    string Srls = "";
        //    if (SelectedValues != null)
        //        Srls = string.Join(",", SelectedValues);
        //    return "your selected:"+ Srls;
        //}
        #endregion


        //[Authorize]
        //public ActionResult ReportShow2(CarModels cm)
        //{
        //    return View(cm);
        //}


    }
}