﻿
using Common.Actions;
using Common.db;
using Common.Models.General;
using Common.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Http;
using WebApi2.Models;

namespace WebApi2.Controllers
{
    public class MessageController : ApiController
    {
        // POST: api/qccastt
        [HttpPost]
        [Route("api/Message/GetSMS")]
        public List<Message> GetSMS([FromBody] Message message)
        {
            try
            {
                string commandtext = "";
                if ((message != null)) // && (U.Macaddress == "48:13:7e:11:d7:1f"))
                {
                    string Command = "";
                    string strTypeDesc = "";
                    if ((message.MessageType != null) && (message.MessageType != ""))
                    {
                        if (message.MessageType == "1")
                        {
                            Command = " and s.userid = 'QCUser' and s.message like '%رخداد%' ";
                            strTypeDesc = "واحد صدور: کیفیت";
                        }
                        else if (message.MessageType == "2")
                        {
                            Command = " and s.userid = 'QCUser' and s.message not like '%رخداد%' ";
                            strTypeDesc = "واحد صدور: آدیت";
                        }
                        else if (message.MessageType == "3")
                        {
                            Command = " and s.userid = 'admin' ";
                            strTypeDesc = "واحد صدور: تولید";
                        }
                        else if (message.MessageType == "4")
                        {
                            Command = " and s.userid is null ";
                            strTypeDesc = "واحد صدور: توقفات";
                        }
                        else
                            return null;
                        //clsDBHelper.LogtxtToFile("********"+message.MessageType+"---"+ message.UserId);
                        commandtext = string.Format(@"select distinct s.message,s.insdate,
                                                                TO_char(s.insdate,
                                                                        'YYYY/MM/DD HH24:MI',
                                                                        'nls_calendar=persian') as InsDateFa,
                                                                TO_char(s.insdate,'YYYY/MM/DD','nls_calendar=persian') as InsDayFa,
                                                                s.userid,
                                                                '{1}' as MessageType,'{2}' as MessageTypeDesc
                                                  from sp_sms s
                                                 where  TO_char(s.insdate,'YYYY/MM/DD','nls_calendar=persian')='{3}'
                                                         and s.message not like  '%یک بار%'
                                                         and {0} order by s.insdate desc
                                                 ", Command, message.MessageType, strTypeDesc, message.InsDayFa);
                    }
                    else
                    {
                        commandtext = string.Format(@"select distinct s.message,
                                                        s.insdate,
                                                        TO_char(s.insdate,
                                                                'YYYY/MM/DD HH24:MI',
                                                                'nls_calendar=persian') as InsDateFa,
                                                        TO_char(s.insdate, 'YYYY/MM/DD', 'nls_calendar=persian') as InsDayFa,
                                                        s.userid,
                                                        Case WHEN userid='QCUser' and instr(s.message,'رخداد')   
                                                          <> 0
                                                        THEN '1'
                                                          WHEN userid='QCUser' and instr(s.message,'رخداد')   
                                                            =0 
                                                            THEN '2'
                                                         WHEN userid='admin' then '3'
                                                         WHEN userid is null then '4'
                                                         END MessageType,
                                                        Case WHEN userid='QCUser' and instr(s.message,'رخداد')   
                                                          <> 0
                                                        THEN 'واحد صدور: کیفیت'
                                                          WHEN userid='QCUser' and instr(s.message,'رخداد')   
                                                            =0 
                                                            THEN 'واحد صدور: آدیت'
                                                         WHEN userid='admin' then 'واحد صدور: تولید'
                                                         WHEN userid is null then 'واحد صدور: توقفات'
                                                         END  MessageTypeDesc
                                          from sp_sms s
                                         where TO_char(s.insdate, 'YYYY/MM/DD', 'nls_calendar=persian') ='{0}'
                                                and s.message not like  '%یک بار%'
                                         order by s.insdate desc
                                                         ", message.InsDayFa);
                    }
                    // userid, message.MessageType
                    //DataSet ds = clsDBHelper.ExecuteMyQueryStp(commandtext);
                    // --
                    List<Message> lst = new List<Message>();
                    lst = DBHelper.GetDBObjectByObj2(new Message(), null, commandtext, "stopage").Cast<Message>().ToList();
                    //---
                    if (lst.Count > 0)
                    {
                        return lst;
                    }
                    else
                    {
                        List<Message> q = new List<Message>();
                        message.message = "اطلاعاتی یافت نشد";
                        //q.Add(message);
                        return null;
                    }
                }
                else
                {
                    List<Message> q = new List<Message>();
                    message.message = "نوع پیام درخواستی یافت نشد";
                    //q.Add(message);
                    return q;
                }


            }
            catch (Exception e)
            {
                //string err = e.ToString() + e.InnerException.Message + e.Message.ToString();
                //clsDBHelper.LogFile(e);
                List<Message> q = new List<Message>();
                message.message = e.Message;
                //q.Add(message);
                return null;
            }

        }

        [HttpPost]
        [Authorize]
        [Route("api/Message/SendSMessage")]
        public SMessage SendSMessage([FromBody] SMessage message) //[FromBody] SMessage message
        {
            NowDateTime ndt = new NowDateTime();
            try
            {
                //SMessage message = new SMessage();
                //message.SenderUserId = 493619;
                //message.SendToUserId = 0;
                //message.SenderUserDesc = "محمد رضا ابراهیمی";
                message.CreatedDateTime = ndt.Now;
                message.CreatedDateTimeFa = ndt.NowDateTimeFa;
                message.Id = Guid.NewGuid().ToString();
                //message.Message = "شماره شاسی صحیح نیست "+(new Random()).Next();
                return SMessageUtility.SendSMessage(message);
            }
            catch (Exception e)
            {
                LogManager.SetCommonLog(e.Message.ToString());
                return null;
            }
        }

        [HttpPost]
        [Authorize]
        [Route("api/Message/DeleteSMessage")]
        public SMessage DeleteSMessage([FromBody] SMessage message) //[FromBody] SMessage message
        {
            try
            {
                
                return SMessageUtility.DeleteSMessage(message);
            }
            catch (Exception e)
            {
                LogManager.SetCommonLog(e.Message.ToString());
                return null;
            }
        }

        //[HttpGet]
        //[Route("api/Message/tstDeleteSMessage")]
        //public List<SMessage> DeleteSMessage() //[FromBody] SMessage message
        //{
        //    try
        //    {
        //        SMessage message = new SMessage();
        //        message.Id = "e4cf84ef-b640-437c-bea4-5f9ab1e51f0a";
        //        message.SenderUserId = 1000861;
        //        return SMessageUtility.DeleteSMessage(message);
        //    }
        //    catch (Exception e)
        //    {
        //        LogManager.SetCommonLog(e.Message.ToString());
        //        return null;
        //    }
        //}

        [HttpGet]
        [Authorize]
        [Route("api/Message/GetSMessages")]
        public List<SMessage> GetSMessages() //[FromBody] SMessage message
        {
            NowDateTime ndt = new NowDateTime();
            try
            {
                return SMessageUtility.GetSMessages();
            }
            catch (Exception e)
            {
                LogManager.SetCommonLog(e.Message.ToString());
                return null;
            }
        }



    }
}
