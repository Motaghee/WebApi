using Common.db;
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
                        string commandtext = string.Format(@"select q.srl,q.vin,
                                                               a.areacode,
                                                               s.strenghtdesc,
                                                               m.modulecode,
                                                               d.defectcode,
                                                               m.modulename,
                                                               d.defectdesc,
                                                               bm.grpcode,
                                                               cg.grpname,
                                                               t.title,
                                                               q.inuse,
                                                               a.areacode||a.areadesc as AreaDesc,
                                                               u.lname as CreatedByDesc,q.CreatedBy,
                                                               ur.lname as RepairedByDesc,q.RepairedBy,
                                                               TO_char(q.createddate,'YYYY/MM/DD HH24:MI:SS','nls_calendar=persian') as createddateFa
                                                               ,q.isrepaired
                                                          from qccastt q
                                                          join qcusert u on u.srl = q.createdby
                                                          left join qcusert ur on ur.srl = q.RepairedBy
                                                          join carid c on c.vin = q.vin 
                                                          join bodymodel bm on bm.bdmdlcode=c.bdmdlcode
                                                          join qcareat a
                                                            on q.qcareat_srl = a.srl
                                                          join qcmdult m
                                                            on q.qcmdult_srl = m.srl
                                                          join qcbadft d
                                                            on q.qcbadft_srl = d.srl
                                                          join qcstrgt s
                                                            on q.qcstrgt_srl = s.srl
                                                          join cargroup cg on cg.grpcode=bm.grpcode
                                                          join qccabdt t on t.srl = d.qccabdt_srl
                                                         where q.inuse=1 and q.recordowner=1 and q.isdefected=1  
                                                         And q.vin= '{0}' order by q.createddate desc", qccastt.VinWithoutChar);
                        //DataSet ds = clsDBHelper.ExecuteMyQueryIns(commandtext);
                        // --
                        //string jsonString = string.Empty;
                        //jsonString = JsonConvert.SerializeObject(ds.Tables[0]);
                        //return jsonString;
                        // --
                        List<Qccastt> FoundDefects = new List<Qccastt>();
                        FoundDefects = clsDBHelper.GetDBObjectByObj2(new Qccastt(), null, commandtext, "inspector").Cast<Qccastt>().ToList();
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
                    clsDBHelper.LogtxtToFile("z null");
                    return null;
                }
            }
            catch (Exception e)
            {
                //string err = e.ToString() + e.InnerException.Message + e.Message.ToString();
                //clsDBHelper.LogFile(e);
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
                FoundCarImages = clsDBHelper.GetDBObjectByObj2(new CarImage(), null, commandtext, "inspector").Cast<CarImage>().ToList();
                //---
                if (FoundCarImages.Count > 0)
                {
                    return FoundCarImages;
                }
                else
                {
                    clsDBHelper.LogtxtToFile("count 0");
                    return null;
                }
            }
            catch (Exception e)
            {
                //string err = e.ToString() + e.InnerException.Message + e.Message.ToString();
                clsDBHelper.LogFile(e);
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
                if (clsDBHelper.DBConnectionIns.State == ConnectionState.Closed)
                {
                    clsDBHelper.DBConnectionIns.ConnectionString = clsDBHelper.CnStrIns;
                    clsDBHelper.DBConnectionIns.Open();
                }
                cmd.Connection = clsDBHelper.DBConnectionIns;
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
                CarImage InsertedCarImage = clsDBHelper.GetDBObjectByObj2(new CarImage(), null, commandtext, "inspector").Cast<CarImage>().ToList()[0];
                return InsertedCarImage;
            }
            catch (Exception e)
            {
                clsDBHelper.LogtxtToFile("err_UpdateCarImage_" + errTrc);
                clsDBHelper.LogFile(e);


                return null;
            }
        }

        //GetModuleList
        public static List<Module> GetBaseModuleList()
        {
            try
            {
                string commandtext = string.Format(@"select srl,modulename,modulecode from qcmdult m ");
                DataSet ds = clsDBHelper.ExecuteMyQueryIns(commandtext);
                List<Module> ModuleList = new List<Module>();
                ModuleList = clsDBHelper.GetDBObjectByObj2(new Module(), null, commandtext, "inspector").Cast<Module>().ToList();
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
                clsDBHelper.LogFile(e);
                return null;
            }
        }


        public static List<Defect> GetBaseDefectList()
        {
            try
            {
                string commandtext = string.Format(@"select srl,defectcode,defectdesc from qcbadft d");
                DataSet ds = clsDBHelper.ExecuteMyQueryIns(commandtext);
                List<Defect> DefectList = new List<Defect>();
                DefectList = clsDBHelper.GetDBObjectByObj2(new Defect(), null, commandtext, "inspector").Cast<Defect>().ToList();
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
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<Strength> GetBaseStrengthList()
        {
            try
            {
                string commandtext = string.Format(@"select  s.srl,s.strenghtcode,s.strenghtdesc from qcstrgt s");
                DataSet ds = clsDBHelper.ExecuteMyQueryIns(commandtext);
                List<Strength> StrengthList = new List<Strength>();
                StrengthList = clsDBHelper.GetDBObjectByObj2(new Strength(), null, commandtext, "inspector").Cast<Strength>().ToList();
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
            catch (Exception e)
            {
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
                DataSet ds = clsDBHelper.ExecuteMyQueryIns(commandtext);
                List<Qcdfctt> CheckList = new List<Qcdfctt>();
                CheckList = clsDBHelper.GetDBObjectByObj2(new Qcdfctt(), null, commandtext, "inspector").Cast<Qcdfctt>().ToList();
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
                clsDBHelper.LogFile(e);
                return null;
            }
        }



        public static List<Area> GetBaseAreaList()
        {
            try
            {
                string commandtext = string.Format(@"select  a.srl,a.areacode,a.areadesc,a.CheckDest from qcareat a");
                DataSet ds = clsDBHelper.ExecuteMyQueryIns(commandtext);
                List<Area> AreaList = new List<Area>();
                AreaList = clsDBHelper.GetDBObjectByObj2(new Area(), null, commandtext, "inspector").Cast<Area>().ToList();
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
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<Qcdsart> GetQcdsart()
        {
            try
            {
                string commandtext = string.Format(@"select  d.fromareasrl,d.toareasrl,d.IsDefault,d.grpcode from Qcdsart d");
                DataSet ds = clsDBHelper.ExecuteMyQueryIns(commandtext);
                List<Qcdsart> PathList = new List<Qcdsart>();
                PathList = clsDBHelper.GetDBObjectByObj2(new Qcdsart(), null, commandtext, "inspector").Cast<Qcdsart>().ToList();
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
            catch (Exception e)
            {
                return null;
            }
        }


        public static ResultMsg InsertQcqctrt(CarSend _CarSend)
        {
            ResultMsg rm = new ResultMsg();
            try
            {
                if (clsDBHelper.DBConnectionIns.State == ConnectionState.Closed)
                {
                    clsDBHelper.DBConnectionIns.ConnectionString = clsDBHelper.CnStrIns;
                    clsDBHelper.DBConnectionIns.Open();
                }
                OracleCommand cmd = new OracleCommand();
                OracleDataAdapter da = new OracleDataAdapter();
                cmd.Connection = clsDBHelper.DBConnectionIns;
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

    }
}