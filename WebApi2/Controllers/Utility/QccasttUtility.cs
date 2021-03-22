using Common.Actions;
using Common.db;
using Common.Models;
using Common.Models.Qccastt;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WebApi2.Models;

namespace WebApi2.Controllers.Utility
{
    public class QccasttUtility
    {

        public static List<Qccastt> GetCarDefect(Qccastt qccastt)
        {
            try
            {

                if ((qccastt != null)) // && (U.Macaddress == "48:13:7e:11:d7:1f"))
                {
                    qccastt.ValidFormat = CarUtility.CheckFormatVin(qccastt.Vin);
                    if (qccastt.ValidFormat)
                    {
                        qccastt.VinWithoutChar = CarUtility.GetVinWithoutChar(qccastt.Vin);
                        string commandtext = string.Format(@"select SYS_GUID() as Id,q.srl,'NAS'||q.vin as vin,q.vin as VinWithoutChar,q.qcmdult_srl,q.qcbadft_srl,
                                                               q.qcareat_srl,a.areacode,a.areatype,p.shopcode,
                                                               s.strenghtdesc,
                                                               m.modulecode,
                                                               d.defectcode,
                                                               m.modulename,
                                                               d.defectdesc,c.bdmdlcode,
                                                               bm.grpcode,
                                                               cg.grpname,
                                                               t.title,q.RecordOwner,q.CHECKLISTAREA_SRL,q.QCSTRGT_SRL,q.IsDefected,q.InUse,
                                                               q.inuse,a.areacode,
                                                               a.areacode||a.areadesc as AreaDesc,
                                                               u.lname as CreatedByDesc,q.CreatedBy,
                                                               ur.lname as RepairedByDesc,q.RepairedBy,
                                                               TO_char(q.createddate,'YYYY/MM/DD HH24:MI:SS','nls_calendar=persian') as createddateFa,
                                                               TO_char(q.repaireddate,'YYYY/MM/DD HH24:MI:SS','nls_calendar=persian') as repaireddateFa,
                                                               to_char(q.createddate,'yyyy/mm/dd','nls_calendar=persian') as CreatedDayFa,
                                                               q.isrepaired,
                                                               {1} as ActAreaSrl,{2} as ActBy
                                                          from qccastt q
                                                          join qcusert u on u.srl = q.createdby
                                                          left join qcusert ur on ur.srl = q.RepairedBy
                                                          join carid c on c.vin = q.vin 
                                                          join bodymodel bm on bm.bdmdlcode=c.bdmdlcode
                                                          join qcareat a
                                                            on q.qcareat_srl = a.srl
                                                     left join pcshopt p on p.srl = a.pcshopt_srl
                                                          join qcmdult m
                                                            on q.qcmdult_srl = m.srl
                                                          join qcbadft d
                                                            on q.qcbadft_srl = d.srl
                                                          join qcstrgt s
                                                            on q.qcstrgt_srl = s.srl
                                                          join cargroup cg on cg.grpcode=bm.grpcode
                                                          join qccabdt t on t.srl = d.qccabdt_srl
                                                         where q.inuse=1 and q.recordowner=1 and q.isdefected=1 and q.deletedby is null 
                                                         And q.vin= '{0}' order by q.createddate desc", qccastt.VinWithoutChar, qccastt.ActAreaSrl.ToString(), qccastt.ActBy.ToString());
                        //DataSet ds = clsDBHelper.ExecuteMyQueryIns(commandtext);
                        // --
                        //string jsonString = string.Empty;
                        //jsonString = JsonConvert.SerializeObject(ds.Tables[0]);
                        //return jsonString;
                        // --
                        List<Qccastt> FoundDefects = new List<Qccastt>();
                        FoundDefects = DBHelper.GetDBObjectByObj2_OnLive(new Qccastt(), null, commandtext, "inspector").Cast<Qccastt>().ToList();
                        //---
                        if (FoundDefects.Count > 0)
                        {
                            FoundDefects[0].ValidFormat = qccastt.ValidFormat;
                            FoundDefects[0].VinWithoutChar = qccastt.VinWithoutChar;
                            return FoundDefects;
                        }
                        else
                        {
                            List<Qccastt> q = new List<Qccastt>();
                            q.Add(qccastt);
                            return q;
                        }
                    }
                    else
                    {
                        List<Qccastt> q = new List<Qccastt>();
                        q.Add(qccastt);
                        return q;
                    }

                }
                else
                {
                    DBHelper.LogtxtToFile("z null");
                    return null;
                }
            }
            catch (Exception ex)
            {
                //string err = e.ToString() + e.InnerException.Message + e.Message.ToString();
                //clsDBHelper.LogFile(e);
                DBHelper.LogFile(ex);
                List<Qccastt> q = new List<Qccastt>();
                q.Add(qccastt);
                return q;
            }
        }

        public static List<CarImage> GetCarImages(CarImage carImage)
        {
            try
            {
                //clsDBHelper.LogtxtToFile("00000 GetCarImages" + car.Vin);
                string commandtext = "";
                if ((!carImage.Srl.Equals(null)) && (carImage.Srl != 0))
                {
                    commandtext = string.Format(@"select i.Srl,
                                                                    'NAS'||i.Vin as Vin,
                                                                    i.Title,
                                                                    i.ImageDesc,
                                                                    i.CreatedBy,
                                                                    i.QCAreatSrl,
                                                                    i.Image,i.Thumbnail,
                                                                    i.inuse,i.updatedby,
                                                                    u.fname || ' ' || u.lname as createdbydesc,
                                                                    i.qcareatsrl,
                                                                    a.areacode || ' ' || a.areadesc as areadesc,
                                                                    TO_char(i.createddate,
                                                                            'YYYY/MM/DD HH24:MI:SS',
                                                                            'nls_calendar=persian') as createddateFa
                                                                from qccarimgt i
                                                                left join qcusert u
                                                                on u.srl = i.createdby
                                                                left join qcareat a
                                                                on a.srl = i.qcareatsrl
                                                                where i.inuse = 1
                                                                and i.Srl = {0}
                                                            ", carImage.Srl);
                }
                else
                {
                    commandtext = string.Format(@"select i.Srl,
                                                                    'NAS'||i.Vin as Vin,
                                                                    i.Title,
                                                                    i.ImageDesc,
                                                                    i.CreatedBy,
                                                                    i.QCAreatSrl,
                                                                    null as Image,i.Thumbnail,
                                                                    i.inuse,i.updatedby,
                                                                    u.fname || ' ' || u.lname as createdbydesc,
                                                                    i.qcareatsrl,
                                                                    a.areacode || ' ' || a.areadesc as areadesc,
                                                                    TO_char(i.createddate,
                                                                            'YYYY/MM/DD HH24:MI:SS',
                                                                            'nls_calendar=persian') as createddateFa
                                                                from qccarimgt i
                                                                left join qcusert u
                                                                on u.srl = i.createdby
                                                                left join qcareat a
                                                                on a.srl = i.qcareatsrl
                                                                where i.inuse = 1
                                                                and i.vin = '{0}'
                                                            ", CarUtility.GetVinWithoutChar(carImage.Vin));
                }


                // --
                List<CarImage> FoundCarImages = new List<CarImage>();
                FoundCarImages = DBHelper.GetDBObjectByObj2(new CarImage(), null, commandtext, "inspector").Cast<CarImage>().ToList();
                //---
                if (FoundCarImages.Count > 0)
                {
                    return FoundCarImages;
                }
                else
                {
                    DBHelper.LogtxtToFile("count 0");
                    return null;
                }
            }
            catch (Exception e)
            {
                //string err = e.ToString() + e.InnerException.Message + e.Message.ToString();
                DBHelper.LogFile(e);
                return null;
            }
        }

        public static CarImage UpdateCarImage(CarImage carImage)
        {
            string errTrc = carImage.Vin + "_1_";
            try
            {

                OracleCommand cmd = new OracleCommand();
                if (carImage.Inuse == 1)
                {
                    if (carImage.Updated == false) //insert new image
                    {
                        if (carImage.Image == null)
                        {
                            errTrc += "null image_";
                        }


                        cmd.CommandText = string.Format(@"INSERT INTO qccarimgt (Srl,Vin,Title,ImageDesc,CreatedBy,CreatedDate,QCAreatSrl, Image,Thumbnail)
                        VALUES ({0},'{1}','{2}','{3}',{4},sysdate,{5},:blobImage,:blobThumbnail)"
                       , carImage.Srl, CarUtility.GetVinWithoutChar(carImage.Vin), carImage.Title, carImage.ImageDesc, carImage.CreatedBy,
                       carImage.QCAreatSrl);
                        OracleParameter blobImage = new OracleParameter();
                        blobImage.OracleDbType = OracleDbType.Blob;
                        blobImage.ParameterName = ":blobImage";
                        blobImage.Direction = ParameterDirection.Input;
                        byte[] bImage = Convert.FromBase64String(carImage.Image);
                        blobImage.Value = bImage;
                        cmd.Parameters.Add(blobImage);
                        //--
                        OracleParameter blobThumbnail = new OracleParameter();
                        blobThumbnail.OracleDbType = OracleDbType.Blob;
                        blobThumbnail.ParameterName = ":blobThumbnail";
                        blobThumbnail.Direction = ParameterDirection.Input;
                        byte[] bThumbnail = Convert.FromBase64String(carImage.Thumbnail);
                        blobThumbnail.Value = bThumbnail;
                        cmd.Parameters.Add(blobThumbnail);


                    }
                    else         //update desc of image
                    {
                        cmd.CommandText = string.Format(@"update qccarimgt i
                                                   set i.Title = {1},
                                                       i.ImageDesc={2}
                                                       i.updatedby = {3},
                                                       i.u_date= sysdate
                                                   where srl={0}
                                                ", carImage.Srl, carImage.Title, carImage.ImageDesc, carImage.UpdatedBy);

                    }
                }
                else
                {
                    cmd.CommandText = string.Format(@"update qccarimgt i
                                                   set i.inuse = 0,
                                                       i.updatedby = {1},
                                                       i.u_date= sysdate
                                                   where srl={0}
                                                ", carImage.Srl, carImage.UpdatedBy);
                }
                if (DBHelper.DBConnectionIns.State == ConnectionState.Closed)
                {
                    DBHelper.DBConnectionIns.ConnectionString = DBHelper.CnStrIns;
                    DBHelper.DBConnectionIns.Open();
                }
                cmd.Connection = DBHelper.DBConnectionIns;
                cmd.ExecuteNonQuery();
                // ---
                //cmd.Parameters.Clear();
                string commandtext = "";
                if (carImage.Inuse == 1)
                {
                    commandtext = string.Format(@"select i.Srl,
                                                            'NAS' || i.Vin as Vin,
                                                            i.Title,
                                                            i.ImageDesc,
                                                            i.CreatedBy,
                                                            i.QCAreatSrl,Thumbnail,
                                                            null as Image,i.Inuse,i.updatedby,
                                                            u.fname ||' '|| u.lname as createdbydesc,
                                                            a.areacode || ' ' || a.areadesc as areadesc,
                                                            TO_char(i.createddate,'YYYY/MM/DD HH24:MI:SS','nls_calendar=persian') as createddateFa
                                                        from qccarimgt i
                                                        left join qcusert u
                                                        on u.srl = i.createdby
                                                        left join qcareat a
                                                        on a.srl = i.qcareatsrl
                                                        Where i.Srl ={0}", carImage.Srl);
                }
                else
                {
                    commandtext = string.Format(@"select i.Srl,
                                                                'NAS' || i.Vin as Vin,
                                                                i.Title,
                                                                i.ImageDesc,
                                                                i.CreatedBy,
                                                                i.QCAreatSrl,null as Thumbnail,
                                                                null as Image,i.Inuse,i.updatedby,
                                                                u.fname ||' '|| u.lname as createdbydesc,
                                                                a.areacode || ' ' || a.areadesc as areadesc,
                                                                TO_char(i.createddate,'YYYY/MM/DD HH24:MI:SS','nls_calendar=persian') as createddateFa
                                                            from qccarimgt i
                                                            left join qcusert u
                                                            on u.srl = i.createdby
                                                            left join qcareat a
                                                            on a.srl = i.qcareatsrl
                                                            Where i.Srl ={0}", carImage.Srl);
                }
                CarImage InsertedCarImage = DBHelper.GetDBObjectByObj2(new CarImage(), null, commandtext, "inspector").Cast<CarImage>().ToList()[0];
                return InsertedCarImage;
            }
            catch (Exception e)
            {
                DBHelper.LogtxtToFile("err_UpdateCarImage_" + errTrc);
                DBHelper.LogFile(e);


                return null;
            }
        }

        //GetModuleList
        public static List<Module> GetBaseModuleList()
        {
            try
            {
                string commandtext = string.Format(@"select srl,modulename,modulecode from qcmdult m ");
                DataSet ds = DBHelper.ExecuteMyQueryIns(commandtext);
                List<Module> ModuleList = new List<Module>();
                ModuleList = DBHelper.GetDBObjectByObj2(new Module(), null, commandtext, "inspector").Cast<Module>().ToList();
                //---
                if (ModuleList.Count > 0)
                {
                    return ModuleList;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                DBHelper.LogFile(e);
                return null;
            }
        }


        public static List<Defect> GetBaseDefectList()
        {
            try
            {
                string commandtext = string.Format(@"select srl,defectcode,defectdesc from qcbadft d");
                DataSet ds = DBHelper.ExecuteMyQueryIns(commandtext);
                List<Defect> DefectList = new List<Defect>();
                DefectList = DBHelper.GetDBObjectByObj2(new Defect(), null, commandtext, "inspector").Cast<Defect>().ToList();
                //---
                if (DefectList.Count > 0)
                {
                    return DefectList;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                DBHelper.LogFile(ex);
                return null;
            }
        }

        public static List<Strength> GetBaseStrengthList()
        {
            try
            {
                string commandtext = string.Format(@"select  s.srl,s.strenghtcode,s.strenghtdesc from qcstrgt s");
                DataSet ds = DBHelper.ExecuteMyQueryIns(commandtext);
                List<Strength> StrengthList = new List<Strength>();
                StrengthList = DBHelper.GetDBObjectByObj2(new Strength(), null, commandtext, "inspector").Cast<Strength>().ToList();
                //---
                if (StrengthList.Count > 0)
                {
                    return StrengthList;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }
        }


        public static List<Qcdfctt> GetAreaCheckList(Area _area)
        {
            try
            {
                string commandtext = string.Format(@"select distinct q.srl ,q.qcmdult_srl,
                                                        q.qcbadft_srl,q.qcstrgt_srl,q.grpcode  from qcdfctt q 
				                                        where q.qcareat_srl = {0}",
                                                    _area.Srl);
                DataSet ds = DBHelper.ExecuteMyQueryIns(commandtext);
                List<Qcdfctt> CheckList = new List<Qcdfctt>();
                CheckList = DBHelper.GetDBObjectByObj2(new Qcdfctt(), null, commandtext, "inspector").Cast<Qcdfctt>().ToList();
                //---
                if (CheckList.Count > 0)
                {
                    return CheckList;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                DBHelper.LogFile(e);
                return null;
            }
        }



        public static List<Area> GetBaseAreaList()
        {
            try
            {
                string commandtext = string.Format(@"select  a.srl,a.areacode,a.areadesc,a.CheckDest,
                                        decode(a.areatype,35,(decode(p.shopcode,14,30,17,40)),a.areatype) as areatype,PCShopt_Srl
                                        from qcareat a join pcshopt p on p.srl = a.pcshopt_srl ");
                DataSet ds = DBHelper.ExecuteMyQueryIns(commandtext);
                List<Area> AreaList = new List<Area>();
                AreaList = DBHelper.GetDBObjectByObj2(new Area(), null, commandtext, "inspector").Cast<Area>().ToList();
                //---
                if (AreaList.Count > 0)
                {
                    return AreaList;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }
        }

        public static List<Shop> GetBaseShopList()
        {
            try
            {
                string commandtext = string.Format(@"select p.shopcode,p.shopname from pcshopt p join pt.shop s on s.shopcode=p.ptshopcode where companycode=82");
                DataSet ds = DBHelper.ExecuteMyQueryIns(commandtext);
                List<Shop> List = new List<Shop>();
                List = DBHelper.GetDBObjectByObj2(new Shop(), null, commandtext, "inspector").Cast<Shop>().ToList();
                //---
                if (List.Count > 0)
                {
                    return List;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }
        }

        public static List<CarGroup> GetBaseCarGroupList()
        {
            try
            {
                string commandtext = string.Format(@"select GrpCode,GrpName,SmsTitle from pt.cargroup cg");
                DataSet ds = DBHelper.ExecuteMyQueryIns(commandtext);
                List<CarGroup> List = new List<CarGroup>();
                List = DBHelper.GetDBObjectByObj2(new CarGroup(), null, commandtext, "inspector").Cast<CarGroup>().ToList();
                //---
                if (List.Count > 0)
                {
                    return List;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }
        }

        public static List<BodyModel> GetBaseBodyModelList()
        {
            try
            {
                string commandtext = string.Format(@"select bdmdlcode,grpcode,aliasname,case when bm.aliasname='تيبا 211' then 'تیبا2' when bm.aliasname='تيبا 212'  or bm.aliasname='تيبا 232'  then gs.name else bm.aliasname end  as CommonBodyModelName  from pt.bodymodel bm JOIN pt.groupsproductsmsgroup gs on gs.smsgrpid=bm.smsgrpid");
                DataSet ds = DBHelper.ExecuteMyQueryIns(commandtext);
                List<BodyModel> List = new List<BodyModel>();
                List = DBHelper.GetDBObjectByObj2(new BodyModel(), null, commandtext, "inspector").Cast<BodyModel>().ToList();
                //---
                if (List.Count > 0)
                {
                    return List;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }
        }

        public static List<SaleStatus> GetBaseSaleStatusList()
        {
            try
            {
                string commandtext = string.Format(@"select * from sale.car_status@saleguard_priprctl s where s.Status_Code in (select distinct Status_Code from sale.car_id@saleguard_priprctl p where p.prod_date> 980101)");
                DataSet ds = DBHelper.ExecuteMyQueryIns(commandtext);
                List<SaleStatus> List = new List<SaleStatus>();
                List = DBHelper.GetDBObjectByObj2(new SaleStatus(), null, commandtext, "inspector").Cast<SaleStatus>().ToList();
                //---
                if (List.Count > 0)
                {
                    return List;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }
        }

        public static List<FinalQC> GetBaseFinalQCList()
        {
            try
            {
                string commandtext = string.Format(@"select finqccode,finqcname from finalqc f where f.isactive=1");
                DataSet ds = DBHelper.ExecuteMyQueryIns(commandtext);
                List<FinalQC> List = new List<FinalQC>();
                List = DBHelper.GetDBObjectByObj2(new FinalQC(), null, commandtext, "inspector").Cast<FinalQC>().ToList();
                //---
                if (List.Count > 0)
                {
                    return List;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }
        }

        public static List<Qcdsart> GetQcdsart()
        {
            try
            {
                string commandtext = string.Format(@"select  d.fromareasrl,d.toareasrl,d.IsDefault,d.grpcode from Qcdsart d");
                DataSet ds = DBHelper.ExecuteMyQueryIns(commandtext);
                List<Qcdsart> PathList = new List<Qcdsart>();
                PathList = DBHelper.GetDBObjectByObj2(new Qcdsart(), null, commandtext, "inspector").Cast<Qcdsart>().ToList();
                //---
                if (PathList.Count > 0)
                {
                    return PathList;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }
        }


        public static ResultMsg InsertQcqctrt(CarSend _CarSend)
        {
            ResultMsg rm = new ResultMsg();
            try
            {
                if (DBHelper.DBConnectionIns.State == ConnectionState.Closed)
                {
                    DBHelper.DBConnectionIns.ConnectionString = DBHelper.CnStrIns;
                    DBHelper.DBConnectionIns.Open();
                }
                OracleCommand cmd = new OracleCommand();
                OracleDataAdapter da = new OracleDataAdapter();
                cmd.Connection = DBHelper.DBConnectionIns;
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "qcinsertqcqctrt";
                cmd.Parameters.Clear();
                cmd.Parameters.Add("pvin", OracleDbType.Varchar2).Value = CarUtility.GetVinWithoutChar(_CarSend.Vin);
                cmd.Parameters.Add("pfromareasrl", OracleDbType.Double).Value = _CarSend.FromAreaSrl;
                cmd.Parameters.Add("ptoareasrl", OracleDbType.Double).Value = _CarSend.ToAreaSrl;
                cmd.Parameters.Add("pcurteamwork", OracleDbType.Varchar2).Value = "";
                cmd.Parameters.Add("pstatuscode", OracleDbType.Double).Value = 1;
                cmd.Parameters.Add("puauser_srl", OracleDbType.Double).Value = _CarSend.QCUsertSrl;
                cmd.Parameters.Add("pErrorMessages", OracleDbType.Varchar2, 2048);
                cmd.Parameters["pErrorMessages"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string result = cmd.Parameters["pErrorMessages"].Value.ToString();
                rm.title = rm.Message = result;
                return rm;

            }
            catch (Exception ex)
            {
                rm.title = "error";
                rm.Message = ex.Message.ToString();
                return rm;
            }

        }

        public static ResultMsg Delete_QCCASTT(Qccastt qccastt)
        {
            //int QCCASTTSRL = Convert.ToInt32(qccastt.Srl);
            //int areaSRL = Convert.ToInt32(qccastt.ActAreaSrl);
            //int userSRL = Convert.ToInt32(qccastt.ActBy);
            //LogManager.SetCommonLog("Delete_QCCASTT" + qccastt.Srl.ToString() + "_" + qccastt.ActAreaSrl.ToString() + "_" + qccastt.ActBy.ToString());
            ResultMsg rm = new ResultMsg();
            try
            {
                LogManager.SetCommonLog("Delete_QCCASTT:" + qccastt.IsRepaired.ToString() + "_actby:" + qccastt.ActBy.ToString() + "_actareasrl:" + qccastt.ActAreaSrl.ToString());
                OracleCommand cmd = new OracleCommand();
                OracleDataAdapter da = new OracleDataAdapter();
                cmd.Connection = DBHelper.LiveDBConnectionIns;
                if (DBHelper.LiveDBConnectionIns.State != ConnectionState.Open)
                    DBHelper.LiveDBConnectionIns.Open();
                da.SelectCommand = cmd;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "QCP_QCCASTT_Delete";
                cmd.Parameters.Add("pQCCASTT_SRL", OracleDbType.Int32).Value = qccastt.Srl;
                cmd.Parameters.Add("pQCUSERT_SRL", OracleDbType.Int32).Value = qccastt.ActBy;
                cmd.Parameters.Add("pQCAREAT_SRL", OracleDbType.Int32).Value = qccastt.ActAreaSrl;
                cmd.Parameters.Add("pMessage", OracleDbType.Varchar2, 1000);
                cmd.Parameters["pMessage"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string result = cmd.Parameters["pMessage"].Value.ToString();
                rm.title = rm.Message = result;
                //-- translate result  msg --
                if (result.Contains("SUCCESSFUL"))
                    rm.MessageFa = "عیب مورد نظر با موفقیت حذف گردید";
                else if (result.Contains("NO_DATA_FOUND"))
                    rm.MessageFa = "عیب مورد نظر در سیستم یافت نشد";
                else
                    rm.MessageFa = "بروز خطا در حذف عیب";
                LogManager.SetCommonLog("Delete_QCCASTT:err" + result);
                //System.Threading.Thread.Sleep(1000);
                rm.lstQccastt = QccasttUtility.GetCarDefect(qccastt);
                
                
                // --
                return rm;
            }
            catch (Exception ex)
            {
                rm.title = "error";
                rm.Message = ex.Message.ToString();
                return rm;
            }

        }

        public static bool CheckConsistencyBetweenCheckListAndCarGroupCode(Qccastt qccastt)
        {
            try
            {
                string commandtext = string.Format(@"Select * from qcdfctt d where (d.grpcode={0} or  d.qcareat_srl=841 )
                                                        And d.qcbadft_srl={1} 
                                                        And d.qcareat_srl={2}
                                                        And d.qcmdult_srl={3}
                                                        ", qccastt.GrpCode, qccastt.QCBadft_Srl, qccastt.QCAreat_Srl, qccastt.QCMdult_Srl);
                DataSet ds = DBHelper.ExecuteMyQueryInsOnLive(commandtext);
                if ((ds.Tables[0] != null) && (ds.Tables[0].Rows.Count != 0))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static ResultMsg QCCASTT_DefectDetect(Qccastt qccastt)
        {
            ResultMsg rm = new ResultMsg();
            
            try
            {
                //LogManager.SetCommonLog("QCCASTT_DefectDetect_isrep:"+qccastt.IsRepaired.ToString()+"_actby:" + qccastt.ActBy.ToString() + "_actareasrl:" + qccastt.ActAreaSrl.ToString() + "_QCSTRGT_SRL:" + qccastt.QCSTRGT_SRL.ToString() + "_IsDefected:" + qccastt.IsDefected.ToString());//
                bool blnConsistency = false;
                if (qccastt.IsDefected == 1)
                {
                    blnConsistency =
                        CheckConsistencyBetweenCheckListAndCarGroupCode(qccastt);
                }
                if ((blnConsistency) || (qccastt.IsDefected == 0))
                {
                    OracleCommand cmd = new OracleCommand();
                    OracleDataAdapter da = new OracleDataAdapter();
                    cmd.Connection = DBHelper.LiveDBConnectionIns;
                    if (DBHelper.LiveDBConnectionIns.State != ConnectionState.Open)
                        DBHelper.LiveDBConnectionIns.Open();
                    da.SelectCommand = cmd;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "QCP_QCCASTT_Detect";
                    if (!qccastt.QCBadft_Srl.Equals(null))
                        cmd.Parameters.Add("PQCBADFT_SRL", OracleDbType.Int32).Value = qccastt.QCBadft_Srl;
                    else
                        cmd.Parameters.Add("PQCBADFT_SRL", OracleDbType.Int32).Value = DBNull.Value;
                    //-
                    if (!qccastt.QCMdult_Srl.Equals(null))
                        cmd.Parameters.Add("PQCMDULT_SRL", OracleDbType.Int32).Value = qccastt.QCMdult_Srl;
                    else
                        cmd.Parameters.Add("PQCMDULT_SRL", OracleDbType.Int32).Value = DBNull.Value;
                    //--
                    cmd.Parameters.Add("PVIN", OracleDbType.Varchar2).Value = qccastt.VinWithoutChar;
                    cmd.Parameters.Add("PQCAREAT_SRL", OracleDbType.Int32).Value = qccastt.ActAreaSrl;
                    // --
                    if (!qccastt.IsRepaired.Equals(null))
                        cmd.Parameters.Add("PISREPAIRED", OracleDbType.Int32).Value = qccastt.IsRepaired;
                    else
                        cmd.Parameters.Add("PISREPAIRED", OracleDbType.Int32).Value = DBNull.Value;

                    cmd.Parameters.Add("PQCUSERT_SRL", OracleDbType.Int32).Value = qccastt.ActBy;
                    //-
                    if (!qccastt.QCSTRGT_SRL.Equals(null))
                        cmd.Parameters.Add("PQCSTRGT_SRL", OracleDbType.Int32).Value = qccastt.QCSTRGT_SRL;
                    else
                        cmd.Parameters.Add("PQCSTRGT_SRL", OracleDbType.Int32).Value = DBNull.Value;
                    // --
                    if (!qccastt.CHECKLISTAREA_SRL.Equals(null))
                        cmd.Parameters.Add("pCheckListArea_SRL", OracleDbType.Int32).Value = qccastt.CHECKLISTAREA_SRL;
                    else
                        cmd.Parameters.Add("pCheckListArea_SRL", OracleDbType.Int32).Value = DBNull.Value;
                    // --
                    cmd.Parameters.Add("PINUSE", OracleDbType.Int32).Value = qccastt.InUse;
                    cmd.Parameters.Add("PISDEFECTED", OracleDbType.Int32).Value = qccastt.IsDefected;
                    // --
                    if (!qccastt.RecordOwner.Equals(null))
                        cmd.Parameters.Add("pRecordOwner", OracleDbType.Int32).Value = qccastt.RecordOwner;
                    else
                        cmd.Parameters.Add("pRecordOwner", OracleDbType.Int32).Value = DBNull.Value;
                    // --
                    cmd.Parameters.Add("pGrpCode", OracleDbType.Int32).Value = qccastt.GrpCode;
                    // --
                    cmd.Parameters.Add("pMessage", OracleDbType.Varchar2, 1000);
                    cmd.Parameters["PMESSAGE"].Direction = ParameterDirection.Output;
                    // --
                    cmd.ExecuteNonQuery();
                    string result = cmd.Parameters["PMESSAGE"].Value.ToString();
                    rm.title = rm.Message = result;
                    //-- translate result  msg --
                    if (result.Contains("SUCCESSFUL"))
                        rm.MessageFa = "عیب با موفقیت ثبت گردید";
                    else if (result.ToUpper().Trim().Equals("REPAIRED CHANGE") || result.ToUpper().Trim().Equals("EDIT_REPAIRED"))
                    {
                        if (qccastt.IsRepaired==1)
                            rm.MessageFa = "عیب رفع شده ثبت گردید";
                        else
                            rm.MessageFa = "عیب رفع نشده ثبت گردید";
                    }
                    else if (result.Contains("EDIT DEFECT"))
                        rm.MessageFa = "عیب ویرایش گردید";

                    else if (result.Contains("REPEATED DEFECT"))
                        rm.MessageFa = "این عیب قبلا در این ناحیه ثبت گردیده است";
                    else
                        rm.MessageFa = "خطایی رخ داده است"+ qccastt.ActBy.ToString() ;
                    // --
                    rm.lstQccastt= QccasttUtility.GetCarDefect(qccastt);
                    return rm;
                }
                else
                {
                    rm.title = rm.Message = rm.MessageFa = "عدم همخوانی عیب باخودرو";
                    return rm;
                }
            }
            catch (Exception ex)
            {
                rm.title = "error";
                rm.Message = ex.Message.ToString();
                LogManager.SetCommonLog("QCCASTT_DefectDetect_Err:" + ex.Message.ToString());
                return rm;
            }
        }

        public static ResultMsg QCCASTT_MultiDefectRepair3(List<Qccastt> LstQCcastt)
        {
            ResultMsg rm = new ResultMsg();
            try
            {
                for (int i = 0; i < LstQCcastt.Count; i++)
                {
                    bool blnConsistency = false;
                    if (LstQCcastt[i].IsDefected == 1)
                    {
                        blnConsistency =
                            CheckConsistencyBetweenCheckListAndCarGroupCode(LstQCcastt[i]);
                    }
                    if ((blnConsistency) || (LstQCcastt[i].IsDefected == 0))
                    {
                        OracleCommand cmd = new OracleCommand();
                        OracleDataAdapter da = new OracleDataAdapter();
                        cmd.Connection = DBHelper.LiveDBConnectionIns;
                        if (DBHelper.LiveDBConnectionIns.State != ConnectionState.Open)
                            DBHelper.LiveDBConnectionIns.Open();
                        da.SelectCommand = cmd;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "QCP_QCCASTT_Detect";
                        if (!LstQCcastt[i].QCBadft_Srl.Equals(null))
                            cmd.Parameters.Add("PQCBADFT_SRL", OracleDbType.Int32).Value = LstQCcastt[i].QCBadft_Srl;
                        else
                            cmd.Parameters.Add("PQCBADFT_SRL", OracleDbType.Int32).Value = DBNull.Value;
                        //-
                        if (!LstQCcastt[i].QCMdult_Srl.Equals(null))
                            cmd.Parameters.Add("PQCMDULT_SRL", OracleDbType.Int32).Value = LstQCcastt[i].QCMdult_Srl;
                        else
                            cmd.Parameters.Add("PQCMDULT_SRL", OracleDbType.Int32).Value = DBNull.Value;
                        //--
                        cmd.Parameters.Add("PVIN", OracleDbType.Varchar2).Value = LstQCcastt[i].VinWithoutChar;
                        cmd.Parameters.Add("PQCAREAT_SRL", OracleDbType.Int32).Value = LstQCcastt[i].ActAreaSrl;
                        // --
                        if (!LstQCcastt[i].IsRepaired.Equals(null))
                            cmd.Parameters.Add("PISREPAIRED", OracleDbType.Int32).Value = LstQCcastt[i].IsRepaired;
                        else
                            cmd.Parameters.Add("PISREPAIRED", OracleDbType.Int32).Value = DBNull.Value;

                        cmd.Parameters.Add("PQCUSERT_SRL", OracleDbType.Int32).Value = LstQCcastt[i].ActBy;
                        //-
                        if (!LstQCcastt[i].QCSTRGT_SRL.Equals(null))
                            cmd.Parameters.Add("PQCSTRGT_SRL", OracleDbType.Int32).Value = LstQCcastt[i].QCSTRGT_SRL;
                        else
                            cmd.Parameters.Add("PQCSTRGT_SRL", OracleDbType.Int32).Value = DBNull.Value;
                        // --
                        if (!LstQCcastt[i].CHECKLISTAREA_SRL.Equals(null))
                            cmd.Parameters.Add("pCheckListArea_SRL", OracleDbType.Int32).Value = LstQCcastt[i].CHECKLISTAREA_SRL;
                        else
                            cmd.Parameters.Add("pCheckListArea_SRL", OracleDbType.Int32).Value = DBNull.Value;
                        // --
                        cmd.Parameters.Add("PINUSE", OracleDbType.Int32).Value = LstQCcastt[i].InUse;
                        cmd.Parameters.Add("PISDEFECTED", OracleDbType.Int32).Value = LstQCcastt[i].IsDefected;
                        // --
                        if (!LstQCcastt[i].RecordOwner.Equals(null))
                            cmd.Parameters.Add("pRecordOwner", OracleDbType.Int32).Value = LstQCcastt[i].RecordOwner;
                        else
                            cmd.Parameters.Add("pRecordOwner", OracleDbType.Int32).Value = DBNull.Value;
                        // --
                        cmd.Parameters.Add("pGrpCode", OracleDbType.Int32).Value = LstQCcastt[i].GrpCode;
                        // --
                        cmd.Parameters.Add("pMessage", OracleDbType.Varchar2, 1000);
                        cmd.Parameters["PMESSAGE"].Direction = ParameterDirection.Output;
                        // --
                        cmd.ExecuteNonQuery();
                        string result = cmd.Parameters["PMESSAGE"].Value.ToString();
                        rm.Message += result;
                        //-- translate result  msg --
                        if (result.Contains("SUCCESSFUL"))
                            rm.MessageFa = "عیب با موفقیت ثبت گردید";
                        else if (result.ToUpper().Trim().Equals("REPAIRED CHANGE") || result.ToUpper().Trim().Equals("EDIT_REPAIRED"))
                        {
                            if (LstQCcastt[i].IsRepaired == 1)
                                rm.MessageFa += "عیب رفع شده ثبت گردید";
                            else
                                rm.MessageFa += "عیب رفع نشده ثبت گردید";
                        }
                        else if (result.Contains("EDIT DEFECT"))
                            rm.MessageFa += "عیب ویرایش گردید";

                        else if (result.Contains("REPEATED DEFECT"))
                            rm.MessageFa += "این عیب قبلا در این ناحیه ثبت گردیده است";
                        else
                            rm.MessageFa += "خطایی رخ داده است";
                        // --
                    }
                    else
                    {
                        //rm.title = rm.Message =
                        rm.MessageFa += "عدم همخوانی عیب باخودرو";
                        
                    }
                }
                rm.lstQccastt = QccasttUtility.GetCarDefect(LstQCcastt[0]);
                return rm;
            }
            catch (Exception ex)
            {
                rm.lstQccastt = null;
                rm.title = "error";
                rm.Message = ex.Message.ToString();
                return rm;
            }
        }
    }
}
