using Common.db;
using Common.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Utility
{
    public static class MessageUtility
    {
        public static MessageCount GetSmsCountByDate(string InsDateFa)
        {
            
            MessageCount msc = new MessageCount();
            msc.InsDateFa = InsDateFa;
            try
            {
                string commandtext = string.Format(@"select MessageType, count(*) as Cnt
                                    from (select distinct s.message,
                                                        Case
                                                            WHEN userid = 'QCUser' and
                                                                instr(s.message, 'رخداد') <> 0 THEN
                                                            '1'
                                                            WHEN userid = 'QCUser' and
                                                                instr(s.message, 'رخداد') = 0 THEN
                                                            '2'
                                                            WHEN userid = 'admin' then
                                                            '3'
                                                            WHEN userid is null then
                                                            '4'
                                                        END MessageType
                                            from sp_sms s
                                            where  s.message not like  '%یک بار%' and TO_char(s.insdate, 'YYYY/MM/DD', 'nls_calendar=persian') =
                                                '{0}')a 
                                    group by MessageType
                                ", InsDateFa);

                List<MsgStatistics> lst = new List<MsgStatistics>();
                lst = DBHelper.GetDBObjectByObj2(new MsgStatistics(), null, commandtext, "stopage").Cast<MsgStatistics>().ToList();
                //---
                if ((lst != null) && (lst.Count > 0))
                {
                    if (lst.Exists(x => x.MessageType == "1"))
                        msc.QCMsgCnt = lst.Where(x => x.MessageType == "1").First().Cnt;
                    if (lst.Exists(x => x.MessageType == "2"))
                        msc.AuditMsgCnt = lst.Where(x => x.MessageType == "2").First().Cnt;
                    if (lst.Exists(x => x.MessageType == "3"))
                        msc.PTMsgCnt = lst.Where(x => x.MessageType == "3").First().Cnt;
                    if (lst.Exists(x => x.MessageType == "4"))
                        msc.SPMsgCnt = lst.Where(x => x.MessageType == "4").First().Cnt;
                }
                return msc;
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return msc;
            }

        }
    }
}