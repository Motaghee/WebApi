using Common.db;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Common.Actions
{
    public static class StatisticsActs
    {
        public static List<ProductStatistics> GetLiveProdStatistics(String _Type)
        {
            try
            {
                DateTime dtN = DateTime.Now;
                DateTime dtYD = DateTime.Now.AddDays(-1);
                DateTime dtO30D = DateTime.Now.AddDays(-30);
                DateTime dtO90D = DateTime.Now.AddDays(-90);
                DateTime dtO180D = DateTime.Now.AddDays(-180);
                PersianCalendar pc = new PersianCalendar();
                //YD_D,YD_M,YD_Y, O30D_D, O30D_M, O30D_Y, O90D_D, O90D_M, O90D_Y, O180D_D, O180D_M, O180D_Y,
                string Y, M, D, strDateCondition0 = "99999999", strDateCondition1 = "999999";
                Y = pc.GetYear(dtN).ToString();
                M = pc.GetMonth(dtN).ToString().PadLeft(2, '0');
                D = pc.GetDayOfMonth(dtN).ToString().PadLeft(2, '0');
                if (_Type == "Y")
                {
                    strDateCondition0 = Y + "0101";
                    strDateCondition1 = Y + "/01/01";
                }
                else if (_Type == "M")
                {
                    strDateCondition0 = Y + M + "01"; ;
                    strDateCondition1 = Y + "/" + M + "/01";
                }
                else if (_Type == "D")
                {
                    strDateCondition0 = Y + M + D; ;
                    strDateCondition1 = Y + "/" + M + "/" + D;
                }
                else if (_Type == "YD")
                {
                    Y = pc.GetYear(dtYD).ToString();
                    M = pc.GetMonth(dtYD).ToString().PadLeft(2, '0');
                    D = pc.GetDayOfMonth(dtYD).ToString().PadLeft(2, '0');
                    strDateCondition0 = Y + M + D; ;
                    strDateCondition1 = Y + "/" + M + "/" + D;
                }
                else if (_Type == "O30D")
                {
                    Y = pc.GetYear(dtO30D).ToString();
                    M = pc.GetMonth(dtO30D).ToString().PadLeft(2, '0');
                    D = pc.GetDayOfMonth(dtO30D).ToString().PadLeft(2, '0');
                    strDateCondition0 = Y + M + D; ;
                    strDateCondition1 = Y + "/" + M + "/" + D;
                }
                else if (_Type == "O90D")
                {
                    Y = pc.GetYear(dtO90D).ToString();
                    M = pc.GetMonth(dtO90D).ToString().PadLeft(2, '0');
                    D = pc.GetDayOfMonth(dtO90D).ToString().PadLeft(2, '0');
                    strDateCondition0 = Y + M + D; ;
                    strDateCondition1 = Y + "/" + M + "/" + D;
                }
                else if (_Type == "O180D")
                {
                    Y = pc.GetYear(dtO180D).ToString();
                    M = pc.GetMonth(dtO180D).ToString().PadLeft(2, '0');
                    D = pc.GetDayOfMonth(dtO180D).ToString().PadLeft(2, '0');
                    strDateCondition0 = Y + M + D; ;
                    strDateCondition1 = Y + "/" + M + "/" + D;
                }

                //string Curdate_fa = pc.GetYear(dtN).ToString() + "/" + pc.GetMonth(dtN).ToString().PadLeft(2, '0') + "/" + pc.GetDayOfMonth(dtN).ToString().PadLeft(2, '0');

                //LogManager.SetCommonLog("GetYearProdStatistics_ starting...");
                string commandtext = string.Format(@"select SYS_GUID() as Id,'{2}' as DateIntervalType ,z.*,sysdate as U_DateTime,TO_char(sysdate,'YYYY/MM/DD HH24:MI:SS','nls_calendar=persian') as U_DateTimeFa from 
                                                                        (-- Saipa
                                                                        select 82 as CompanyCode,'سایپا' as CompanyName,gs.name,bm.aliasname,case when bm.aliasname='تيبا 211' then 'تیبا2' when bm.aliasname='تيبا 212' then NAME else bm.aliasname end as CommonBodyModelName ,count(vin)as ProductCount 
                                                                                  from pt.VW_LFD_ProdJoin s
                                                                                    JOIN BODYMODEL bm on s.bdmdlcode=bm.bdmdlcode 
                                                                                    JOIN groupsproductsmsgroup gs on gs.smsgrpid=bm.smsgrpid
                                                                        where s.pProdDate>=({0}) and companycode=82  
                                                                        group by gs.name,bm.aliasname
                                                                        union
                                                                        -- diesel
                                                                        select 89 as CompanyCode,'سایپا دیزل' as CompanyName,'خودروهای سنگین' As NAME,'خودروهای سنگین' As ALIASNAME,'خودروهای سنگین' As CommonBodyModelName,sum(s.prod_qty) as ProductCount
                                                                         from   saipagroup.diesel_products_data s where s.prod_date >= ('{1}')
                                                                        union
                                                                        select 86 as CompanyCode,'زامیاد' as CompanyName,NAME,NAME as ALIASNAME,NAME as CommonBodyModelName,sum(prod_qty) as ProductCount from
                                                                        (select (case when s.pubrnd_id = 12 then 'ريچ' when s.pubrnd_id = 4 then 'کاميونت پادرا' when s.pubrnd_id = 1 then 'نيسان' else s.pubrnd_name end)
                                                                        as NAME,s.prod_qty
                                                                        from saipagroup.group_products_data s where comp_code=004 and   s.prod_date >= ('{1}') ) q
                                                                        group by q.NAME --004 Zamiad
                                                                        union
                                                                        --parskhodro
                                                                        select 81 as CompanyCode,'پارس خودرو' as CompanyName,NAME,NAME as ALIASNAME,NAME as CommonBodyModelName,sum(prod_qty) as ProductCount from 
                                                                        (select (case when s.pubrnd_id =36 then 'H220 برليانس' when s.pubrnd_id = 41 then 'کوئيک' when s.pubrnd_id = 35 then 'برليانس CROSS' when (s.pubrnd_id =  4)OR(s.pubrnd_id =  25) then 'تندر'  else s.pubrnd_name end)
                                                                        as name ,s.prod_qty,s.pubrnd_id
                                                                        from saipagroup.group_products_data s 
                                                                        where comp_code=002 and   s.prod_date >= ('{1}')) q
                                                                        group by q.NAME having sum(prod_qty)<>0  --002 parskhodro
                                                                        union
                                                                        --kashan
                                                                        select 83 as CompanyCode,'سایپا سیتروئن' as CompanyName,gs.name,bm.aliasname,NAME as CommonBodyModelName,count(vin)as ProductCount from pt.VW_LFD_ProdJoin s
                                                                                    JOIN BODYMODEL bm on s.bdmdlcode=bm.bdmdlcode 
                                                                                    JOIN groupsproductsmsgroup gs on gs.smsgrpid=bm.smsgrpid
                                                                        where s.pProdDate>=({0}) and companycode=83  
                                                                        group by gs.name,bm.aliasname
                                                                        -- BonRo
                                                                        union
                                                                        select 90 as CompanyCode,'بن رو' as CompanyName,gs.name,bm.aliasname,bm.aliasname as CommonBodyModelName,count(vin) as ProductCount from pt.VW_LFD_ProdJoin s
                                                                                    JOIN BODYMODEL bm on s.bdmdlcode=bm.bdmdlcode 
                                                                                    JOIN groupsproductsmsgroup gs on gs.smsgrpid=bm.smsgrpid
                                                                        where s.pProdDate>=({0}) and companycode=90  
                                                                        group by gs.name,bm.aliasname
                                                                        ) z order by companycode asc,ProductCount desc
                                                                        ", strDateCondition0, strDateCondition1, _Type);


                List<ProductStatistics> lst = new List<ProductStatistics>();
                lst = clsDBHelper.GetDBObjectByObj2(new ProductStatistics(), null, commandtext, "pt").Cast<ProductStatistics>().ToList();
                //LogManager.SetCommonLog("GetYearProdStatistics_ SuccessFull");
                //---

                return lst;
            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("GetLiveProdStatistics_ Error" + ex.Message.ToString());
                return null;
            }

        }


        public static List<ProductStatistics> GetArchiveProdStatistics()
        {
            try
            {
                string strDateCondition0 = "13970101";
                string strDateCondition1 = "1397/01/01";
                // create Archive commande
                string commandtext = string.Format(@"select SYS_GUID() as Id,'A' as DateIntervalType ,z.*,sysdate as U_DateTime,TO_char(sysdate,'YYYY/MM/DD HH24:MI:SS','nls_calendar=persian') as U_DateTimeFa from 
                                                    (
                                                    --brand saipa
                                                    select substr(s.pProdDate,0,4)||'/'||substr(s.pProdDate,5,2)||'/'||substr(s.pProdDate,7,2) as Prod_Date,s.companycode,case when companycode=82 then 'سایپا'  when companycode=83 then 'سایپا سیتروئن'   when companycode=90 then 'بن رو' when companycode=81 then 'پارس خودرو' else 'other' end as CompanyName,gs.name,bm.aliasname,case when bm.aliasname='تيبا 211' then 'تیبا2' when bm.aliasname='تيبا 212' then NAME else bm.aliasname end as CommonBodyModelName 
                                                    ,count(vin)as ProductCount 
                                                                from pt.VW_LFD_ProdJoin s
                                                                JOIN BODYMODEL bm on s.bdmdlcode=bm.bdmdlcode 
                                                                JOIN groupsproductsmsgroup gs on gs.smsgrpid=bm.smsgrpid 
                                                    where companycode not in (92,97)
                                                        group by companycode,gs.name,bm.aliasname,s.pProdDate having  s.pProdDate >=({0})

                                                    union
                                                    --parskhodro
                                                    select prod_date,companycode,CompanyName,NAME,NAME as ALIASNAME,NAME as CommonBodyModelName,sum(prod_qty) as cnt from 
                                                    (
                                                    select s.prod_date,case when comp_code=002 then 81 when comp_code=004 then 86 end as companycode,case when comp_code=002  then 'پارس خودرو' when comp_code=004 then 'زامیاد' else 'other' end as CompanyName
                                                    ,(case when s.pubrnd_id =36 then 'H220 برليانس' when s.pubrnd_id = 41 then 'کوئيک' when s.pubrnd_id = 35 then 'برليانس CROSS' when (s.pubrnd_id =  4)OR(s.pubrnd_id =  25) then 'تندر' when s.pubrnd_id = 12 then 'ريچ' when s.pubrnd_id = 4 then 'کاميونت پادرا' when s.pubrnd_id = 1 then 'نيسان' else s.pubrnd_name end)
                                                    as name ,s.prod_qty,s.pubrnd_id,s.comp_code
                                                    from saipagroup.group_products_data s 
                                                    where comp_code in (002,004) and   s.prod_date >= '{1}'
                                                    ) q
                                                    group by q.NAME,prod_date,companycode,CompanyName,NAME having sum(prod_qty)<>0 
                                                    union
                                                    -- diesel
                                                    select s.prod_date,89 as companycode,'سایپا دیزل' as CompanyName,'خودروهای سنگین' As NAME,'خودروهای سنگین' As ALIASNAME,'خودروهای سنگین' As CommonBodyModelName,sum(s.prod_qty) as cnt
                                                        from   saipagroup.diesel_products_data s 
                                                    group by s.prod_date  having s.prod_date >= '{1}' and sum(s.prod_qty) <>0
                                                    order by prod_date,companycode
                                                    ) z
                                                    ", strDateCondition0, strDateCondition1);

                List<ProductStatistics> lst = new List<ProductStatistics>();
                lst = clsDBHelper.GetDBObjectByObj2(new ProductStatistics(), null, commandtext, "pt").Cast<ProductStatistics>().ToList();
                return lst;
            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("GetArchiveProdStatistics_Error_" + ex.Message.ToString());
                return null;


            }
        }

    }
}