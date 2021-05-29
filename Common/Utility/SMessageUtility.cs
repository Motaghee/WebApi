using Common.db;
using Common.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using LiteDB;
using Common.CacheManager;

namespace Common.Utility
{
    public static class SMessageUtility
    {
        public static List<SMessage>  SendSMessage(SMessage _smessage)
        {
            LiteDatabase db = null;
            try
            {
                NowDateTime _ndt = new NowDateTime();
                _smessage.CreatedDateTimeFa = _ndt.NowDateTimeFa;
                _smessage.CreatedDateTime = _ndt.NowTime;
                ConnectionString cn = ldbConfig.ldbSMessageConnectionString;
                db = new LiteDatabase(cn);
                LiteCollection<SMessage> dbSMessage = db.GetCollection<SMessage>("SMessage");
                dbSMessage.Insert(_smessage);
                List <SMessage> lst = dbSMessage.FindAll().Where(x => x.IsDeleted != 1).ToList<SMessage>();
                return lst;
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }

        }

        public static List<SMessage> GetSMessages()
        {
            LiteDatabase db = null;
            try
            {
                ConnectionString cn = ldbConfig.ldbSMessageConnectionString;
                db = new LiteDatabase(cn);
                LiteCollection<SMessage> dbSMessage = db.GetCollection<SMessage>("SMessage");
                List<SMessage> lst = dbSMessage.FindAll().Where(x => x.IsDeleted != 1).ToList<SMessage>();
                return lst;
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }
        }

        public static List<SMessage> DeleteSMessage(SMessage smessage)
        {
            LiteDatabase db = null;
            try
            {
                ConnectionString cn = ldbConfig.ldbSMessageConnectionString;
                db = new LiteDatabase(cn);
                LiteCollection<SMessage> dbSMessage = db.GetCollection<SMessage>("SMessage");
                //dbSMessage.Delete(Query.EQ("Id", smessage.Id));
                //LiteCollection<SMessage> lst=dbSMessage.Where(x => x.Id == smessage.Id).ToList<SMessage>();
                SMessage sm = dbSMessage.FindById(smessage.Id);
                if (sm != null)
                {
                    sm.IsDeleted = 1;
                }
                dbSMessage.Update(sm);
                List<SMessage> lst = dbSMessage.FindAll().Where(x => x.IsDeleted != 1).ToList<SMessage>();
                return lst;
            }
            catch (Exception ex)
            {
                DBHelper.LogFile(ex);
                return null;
            }
        }
    }
}