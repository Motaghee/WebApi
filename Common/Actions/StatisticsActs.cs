using Common.db;
using Common.Models;
using Common.Models.QccasttModels;
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
            string commandtext = "";
            try
            {
                
                DateTime dtN = DateTime.Now;
                DateTime dtYD = DateTime.Now.AddDays(-1);
                DateTime dtO30D = DateTime.Now.AddDays(-30);
                DateTime dtO90D = DateTime.Now.AddDays(-90);
                DateTime dtO180D = DateTime.Now.AddDays(-180);

                PersianCalendar pc = new PersianCalendar();
                string Y, M, D, strDateCondition = "", strLFDDateCondition = "";
                Y = pc.GetYear(dtN).ToString();
                M = pc.GetMonth(dtN).ToString().PadLeft(2, '0');
                D = pc.GetDayOfMonth(dtN).ToString().PadLeft(2, '0');

                if (_Type == "Y")
                {
                    strDateCondition=string.Format(@"s.Prod_date>='{0}/01/01'", Y);
                    strLFDDateCondition = string.Format(@"s.prodenddate > to_date('{0}/01/01','yyyy/mm/dd','nls_calendar=persian')", Y);

                }
                else if (_Type == "M")
                {
                    strDateCondition = string.Format(@"s.Prod_date>='{0}/{1}/01'", Y,M);
                    strLFDDateCondition = string.Format(@"s.prodenddate > to_date('{0}/{1}/01','yyyy/mm/dd','nls_calendar=persian')", Y, M);

                }
                else if (_Type == "D")
                {
                    strDateCondition = string.Format(@"s.Prod_date='{0}/{1}/{2}'", Y, M,D);
                    strLFDDateCondition= string.Format(@"trunc(s.prodenddate) = to_date('{0}/{1}/{2}','yyyy/mm/dd','nls_calendar=persian')", Y, M, D);

                }
                else if (_Type == "YD")
                {
                    Y = pc.GetYear(dtYD).ToString();
                    M = pc.GetMonth(dtYD).ToString().PadLeft(2, '0');
                    D = pc.GetDayOfMonth(dtYD).ToString().PadLeft(2, '0');
                    strDateCondition = string.Format(@"s.Prod_date='{0}/{1}/{2}'", Y, M, D);
                    strLFDDateCondition = string.Format(@" trunc(s.prodenddate) = to_date('{0}/{1}/{2}','yyyy/mm/dd','nls_calendar=persian') ", Y, M, D);

                }
                else if (_Type == "O30D")
                {
                    Y = pc.GetYear(dtO30D).ToString();
                    M = pc.GetMonth(dtO30D).ToString().PadLeft(2, '0');
                    D = pc.GetDayOfMonth(dtO30D).ToString().PadLeft(2, '0');
                    strDateCondition = string.Format(@"s.Prod_date>='{0}/{1}/{2}'", Y, M,D);
                    strLFDDateCondition = string.Format(@"s.prodenddate > to_date('{0}/{1}/{2}','yyyy/mm/dd','nls_calendar=persian')", Y, M, D);
                }
                else if (_Type == "O90D")
                {
                    Y = pc.GetYear(dtO90D).ToString();
                    M = pc.GetMonth(dtO90D).ToString().PadLeft(2, '0');
                    D = pc.GetDayOfMonth(dtO90D).ToString().PadLeft(2, '0');
                    strDateCondition = string.Format(@"s.Prod_date>='{0}/{1}/{2}'", Y, M, D);
                    strLFDDateCondition = string.Format(@"s.prodenddate > to_date('{0}/{1}/{2}','yyyy/mm/dd','nls_calendar=persian')", Y, M, D);
                }
                else if (_Type == "O180D")
                {
                    Y = pc.GetYear(dtO180D).ToString();
                    M = pc.GetMonth(dtO180D).ToString().PadLeft(2, '0');
                    D = pc.GetDayOfMonth(dtO180D).ToString().PadLeft(2, '0');
                    strDateCondition = string.Format(@"s.Prod_date>='{0}/{1}/{2}'", Y, M, D);
                    strLFDDateCondition = string.Format(@"s.prodenddate > to_date('{0}/{1}/{2}','yyyy/mm/dd','nls_calendar=persian')", Y, M, D);
                }

                commandtext = string.Format(@"select SYS_GUID() as Id,'{2}' as ProdDateFa,'{2}' as DateIntervalType,z.*,sysdate as U_DateTime,TO_char(sysdate,'YYYY/MM/DD HH24:MI:SS','nls_calendar=persian') as U_DateTimeFa from 
                                                                        (-- Saipa bonro kashan
                                                                        select s.companycode,s.comanyname as CompanyName,gs.name,bm.aliasname,case when bm.aliasname='تيبا 211' then 'تیبا2' when bm.aliasname='تيبا 212'  or bm.aliasname='تيبا 232'  then NAME else bm.aliasname end as CommonBodyModelName ,count(vin)as ProductCount 
                                                                                  from pt.VW_LFD_ProdJoin s
                                                                                    JOIN BODYMODEL bm on s.bdmdlcode=bm.bdmdlcode 
                                                                                    JOIN groupsproductsmsgroup gs on gs.smsgrpid=bm.smsgrpid
                                                                        where {1} and companycode not in (92,97)
                                                                        group by s.comanyname,s.companycode,gs.name,bm.aliasname
                                                                        union
                                                                        -- diesel
                                                                        select 89 as CompanyCode,'سایپا دیزل' as CompanyName,'خودروهای سنگین' As NAME,'خودروهای سنگین' As ALIASNAME,'خودروهای سنگین' As CommonBodyModelName,sum(s.prod_qty) as ProductCount
                                                                         from   saipagroup.diesel_products_data s where {0}
                                                                        union
                                                                         --parskhodro,zamiad
                                                                        select CompanyCode,CompanyName,NAME,NAME as ALIASNAME,NAME as CommonBodyModelName,sum(prod_qty) as ProductCount from 
                                                                        (select (case when comp_code=002 then 'پارس خودرو' when comp_code=004 then 'زامیاد' else comp_code end)as CompanyName,(case when comp_code=002 then 81 when comp_code=004 then 86 else 0 end)as CompanyCode,(case when s.pubrnd_id =36 then 'H220 برليانس' when s.pubrnd_id = 41 then 'کوئيک' when s.pubrnd_id = 35 then 'برليانس CROSS' when ((comp_code=002)and((s.pubrnd_id =  4)OR(s.pubrnd_id =  25))) then 'تندر' when s.pubrnd_id = 12 then 'ريچ' when (s.pubrnd_id = 4 and comp_code=002 ) then 'کاميونت پادرا' when s.pubrnd_id = 1 then 'نيسان' else s.pubrnd_name end)
                                                                        as name ,s.prod_qty,s.pubrnd_id
                                                                        from saipagroup.group_products_data s 
                                                                        where comp_code in (002,004)  and {0} ) q
                                                                        group by CompanyCode,CompanyName,q.NAME having sum(prod_qty)<>0  --002,004 parskhodro,zamiad
                                                                        ) z order by companycode asc,ProductCount desc
                                                                        ", strDateCondition, strLFDDateCondition, _Type);


                List<ProductStatistics> lst = new List<ProductStatistics>();
                object[] result = DBHelper.GetDBObjectByObj2(new ProductStatistics(), null, commandtext, "pt");
                if (result != null)
                {
                    lst = result.Cast<ProductStatistics>().ToList();
                    return lst;
                }
                else
                    return null;
                //LogManager.SetCommonLog("GetYearProdStatistics_ SuccessFull");
                //---

                
            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("GetLiveProdStatistics_ Error" + ex.Message.ToString()+ " "+commandtext+" ");
                return null;
            }

        }

        public static List<BodyModelProductStatistics> GetLiveBodyModelProdStatistics(String _Type)
        {
            try
            {
                DateTime dtN = DateTime.Now;
                DateTime dtYD = DateTime.Now.AddDays(-1);
                DateTime dtO30D = DateTime.Now.AddDays(-30);
                DateTime dtO90D = DateTime.Now.AddDays(-90);
                DateTime dtO180D = DateTime.Now.AddDays(-180);

                PersianCalendar pc = new PersianCalendar();
                string Y, M, D, strDateCondition = "", strLFDDateCondition = "";
                Y = pc.GetYear(dtN).ToString();
                M = pc.GetMonth(dtN).ToString().PadLeft(2, '0');
                D = pc.GetDayOfMonth(dtN).ToString().PadLeft(2, '0');

                if (_Type == "Y")
                {
                    strDateCondition = string.Format(@"s.Prod_date>='{0}/01/01'", Y);
                    strLFDDateCondition = string.Format(@"s.prodenddate > to_date('{0}/01/01','yyyy/mm/dd','nls_calendar=persian')", Y);

                }
                else if (_Type == "M")
                {
                    strDateCondition = string.Format(@"s.Prod_date>='{0}/{1}/01'", Y, M);
                    strLFDDateCondition = string.Format(@"s.prodenddate > to_date('{0}/{1}/01','yyyy/mm/dd','nls_calendar=persian')", Y, M);

                }
                else if (_Type == "D")
                {
                    strDateCondition = string.Format(@"s.Prod_date='{0}/{1}/{2}'", Y, M, D);
                    strLFDDateCondition = string.Format(@"trunc(s.prodenddate) = to_date('{0}/{1}/{2}','yyyy/mm/dd','nls_calendar=persian')", Y, M, D);

                }
                else if (_Type == "YD")
                {
                    Y = pc.GetYear(dtYD).ToString();
                    M = pc.GetMonth(dtYD).ToString().PadLeft(2, '0');
                    D = pc.GetDayOfMonth(dtYD).ToString().PadLeft(2, '0');
                    strDateCondition = string.Format(@"s.Prod_date='{0}/{1}/{2}'", Y, M, D);
                    strLFDDateCondition = string.Format(@" trunc(s.prodenddate) = to_date('{0}/{1}/{2}','yyyy/mm/dd','nls_calendar=persian') ", Y, M, D);

                }
                else if (_Type == "O30D")
                {
                    Y = pc.GetYear(dtO30D).ToString();
                    M = pc.GetMonth(dtO30D).ToString().PadLeft(2, '0');
                    D = pc.GetDayOfMonth(dtO30D).ToString().PadLeft(2, '0');
                    strDateCondition = string.Format(@"s.Prod_date>='{0}/{1}/{2}'", Y, M, D);
                    strLFDDateCondition = string.Format(@"s.prodenddate > to_date('{0}/{1}/{2}','yyyy/mm/dd','nls_calendar=persian')", Y, M, D);
                }
                else if (_Type == "O90D")
                {
                    Y = pc.GetYear(dtO90D).ToString();
                    M = pc.GetMonth(dtO90D).ToString().PadLeft(2, '0');
                    D = pc.GetDayOfMonth(dtO90D).ToString().PadLeft(2, '0');
                    strDateCondition = string.Format(@"s.Prod_date>='{0}/{1}/{2}'", Y, M, D);
                    strLFDDateCondition = string.Format(@"s.prodenddate > to_date('{0}/{1}/{2}','yyyy/mm/dd','nls_calendar=persian')", Y, M, D);
                }
                else if (_Type == "O180D")
                {
                    Y = pc.GetYear(dtO180D).ToString();
                    M = pc.GetMonth(dtO180D).ToString().PadLeft(2, '0');
                    D = pc.GetDayOfMonth(dtO180D).ToString().PadLeft(2, '0');
                    strDateCondition = string.Format(@"s.Prod_date>='{0}/{1}/{2}'", Y, M, D);
                    strLFDDateCondition = string.Format(@"s.prodenddate > to_date('{0}/{1}/{2}','yyyy/mm/dd','nls_calendar=persian')", Y, M, D);
                }

                string commandtext = string.Format(@"select SYS_GUID() as Id,'{2}' as ProdDateFa,'{2}' as DateIntervalType,CommonBodyModelName,sum(ProductCount) as ProductCount,sysdate as U_DateTime,TO_char(sysdate,'YYYY/MM/DD HH24:MI:SS','nls_calendar=persian') as U_DateTimeFa from 
                                                                        (-- Saipa bonro kashan
                                                                        select s.companycode,s.comanyname as CompanyName,gs.name,bm.aliasname,case when bm.aliasname='تيبا 211' then 'تیبا2' when bm.aliasname='تيبا 212'  or bm.aliasname='تيبا 232'  then NAME else bm.aliasname end as CommonBodyModelName ,count(vin)as ProductCount 
                                                                                  from pt.VW_LFD_ProdJoin s
                                                                                    JOIN BODYMODEL bm on s.bdmdlcode=bm.bdmdlcode 
                                                                                    JOIN groupsproductsmsgroup gs on gs.smsgrpid=bm.smsgrpid
                                                                        where {1} and companycode not in (92,97)
                                                                        group by s.comanyname,s.companycode,gs.name,bm.aliasname
                                                                        union
                                                                        -- diesel
                                                                        select 89 as CompanyCode,'سایپا دیزل' as CompanyName,'خودروهای سنگین' As NAME,'خودروهای سنگین' As ALIASNAME,'خودروهای سنگین' As CommonBodyModelName,sum(s.prod_qty) as ProductCount
                                                                         from   saipagroup.diesel_products_data s where {0}
                                                                        union
                                                                         --parskhodro,zamiad
                                                                        select CompanyCode,CompanyName,NAME,NAME as ALIASNAME,NAME as CommonBodyModelName,sum(prod_qty) as ProductCount from 
                                                                        (select (case when comp_code=002 then 'پارس خودرو' when comp_code=004 then 'زامیاد' else comp_code end)as CompanyName,(case when comp_code=002 then 81 when comp_code=004 then 86 else 0 end)as CompanyCode,(case when s.pubrnd_id =36 then 'H220 برليانس' when s.pubrnd_id = 41 then 'کوئيک' when s.pubrnd_id = 35 then 'برليانس CROSS' when ((comp_code=002)and((s.pubrnd_id =  4)OR(s.pubrnd_id =  25))) then 'تندر' when s.pubrnd_id = 12 then 'ريچ' when (s.pubrnd_id = 4 and comp_code=002 ) then 'کاميونت پادرا' when s.pubrnd_id = 1 then 'نيسان' else s.pubrnd_name end)
                                                                        as name ,s.prod_qty,s.pubrnd_id
                                                                        from saipagroup.group_products_data s 
                                                                        where comp_code in (002,004)  and {0} ) q
                                                                        group by CompanyCode,CompanyName,q.NAME having sum(prod_qty)<>0  --002,004 parskhodro,zamiad
                                                                        ) z group by CommonBodyModelName
                                                                        ", strDateCondition, strLFDDateCondition, _Type);


                List<BodyModelProductStatistics> lst = new List<BodyModelProductStatistics>();
                lst = DBHelper.GetDBObjectByObj2(new BodyModelProductStatistics(), null, commandtext, "pt").Cast<BodyModelProductStatistics>().ToList();
                //LogManager.SetCommonLog("GetYearProdStatistics_ SuccessFull");
                //---

                return lst;
            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("GetLiveBodyModelProdStatistics_ Error" + ex.Message.ToString());
                return null;
            }

        }


        public static List<CompanyProductStatistics> GetArchiveCompanyProdStatistics()
        {
            try
            {
                string strFromDate = "1398/01/01";
                PersianCalendar pc = new PersianCalendar();
                DateTime dtN = DateTime.Now;
                string Y = pc.GetYear(dtN).ToString();
                string M = pc.GetMonth(dtN).ToString().PadLeft(2, '0');
                string D = pc.GetDayOfMonth(dtN).ToString().PadLeft(2, '0');
                string strToDate = Y + "/" + M + "/" + D;

                // ---
                string strDateCondition = string.Format(@" s.prod_date >= '{0}' and s.prod_date < '{1}' ", strFromDate, strToDate);
                string strLFDDateCondition = string.Format(@" s.prodenddate > to_date('{0}','yyyy/mm/dd','nls_calendar=persian') and s.prodenddate < to_date('{1}','yyyy/mm/dd','nls_calendar=persian') ", strFromDate, strToDate);
                // create Archive commande
                string commandtext = string.Format(@"select SYS_GUID() as Id,ProdDateFa,companycode,companyname,sum(ProductCount) as ProductCount,sysdate as U_DateTime,TO_char(sysdate,'YYYY/MM/DD HH24:MI:SS','nls_calendar=persian') as U_DateTimeFa 
                                                    from 
                                                    (
                                                    --brand saipa
                                                    select to_char(trunc(s.prodenddate),'yyyy/mm/dd','nls_calendar=persian') as ProdDateFa,s.companycode,case when companycode=82 then 'سایپا'  when companycode=83 then 'سایپا سیتروئن'   when companycode=90 then 'بن رو' when companycode=81 then 'پارس خودرو' else 'other' end as CompanyName,gs.name,bm.aliasname,case when bm.aliasname='تيبا 211' then 'تیبا2' when bm.aliasname='تيبا 212'  or bm.aliasname='تيبا 232' then NAME else bm.aliasname end as CommonBodyModelName 
                                                    ,count(vin)as ProductCount 
                                                                from pt.VW_LFD_ProdJoin s
                                                                JOIN BODYMODEL bm on s.bdmdlcode=bm.bdmdlcode 
                                                                JOIN groupsproductsmsgroup gs on gs.smsgrpid=bm.smsgrpid 
                                                    where companycode not in (92,97) and {1}
                                                        group by companycode,gs.name,bm.aliasname,trunc(s.prodenddate)  

                                                    union
                                                    --parskhodro
                                                    select prod_date as ProdDateFa,companycode,CompanyName,NAME,NAME as ALIASNAME,NAME as CommonBodyModelName,sum(prod_qty) as cnt from 
                                                    (
                                                    select s.prod_date,case when comp_code=002 then 81 when comp_code=004 then 86 end as companycode,case when comp_code=002  then 'پارس خودرو' when comp_code=004 then 'زامیاد' else 'other' end as CompanyName
                                                    ,(case when s.pubrnd_id =36 then 'H220 برليانس' when s.pubrnd_id = 41 then 'کوئيک' when s.pubrnd_id = 35 then 'برليانس CROSS' when ((comp_code=002)and((s.pubrnd_id =  4)OR(s.pubrnd_id =  25))) then 'تندر' when s.pubrnd_id = 12 then 'ريچ' when (s.pubrnd_id = 4 and comp_code=002 ) then 'کاميونت پادرا' when s.pubrnd_id = 1 then 'نيسان' else s.pubrnd_name end)
                                                    as name ,s.prod_qty,s.pubrnd_id,s.comp_code
                                                    from saipagroup.group_products_data s 
                                                    where comp_code in (002,004) and {0}
                                                    ) q
                                                    group by q.NAME,prod_date,companycode,CompanyName,NAME having sum(prod_qty)<>0 
                                                    union
                                                    -- diesel
                                                    select s.prod_date as ProdDateFa,89 as companycode,'سایپا دیزل' as CompanyName,'خودروهای سنگین' As NAME,'خودروهای سنگین' As ALIASNAME,'خودروهای سنگین' As CommonBodyModelName,sum(s.prod_qty) as cnt
                                                        from   saipagroup.diesel_products_data s 
                                                    group by s.prod_date  having {0}  and sum(s.prod_qty) <>0
                                                    order by ProdDateFa,companycode
                                                    ) z 
                                                    group by ProdDateFa,companycode,companyname
                                                    order by ProdDateFa,companycode
                                                    ", strDateCondition, strLFDDateCondition);

                List<CompanyProductStatistics> lst = new List<CompanyProductStatistics>();
                lst = DBHelper.GetDBObjectByObj2(new CompanyProductStatistics(), null, commandtext, "pt").Cast<CompanyProductStatistics>().ToList();
                return lst;
            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("GetArchiveCompanyProdStatistics_Error_" + ex.Message.ToString());
                return null;


            }
        }

        public static List<GroupProductStatistics> GetArchiveGroupProdStatistics()
        {
            try
            {
                string strFromDate = "1398/01/01";
                PersianCalendar pc = new PersianCalendar();
                DateTime dtN = DateTime.Now;
                string Y = pc.GetYear(dtN).ToString();
                string M = pc.GetMonth(dtN).ToString().PadLeft(2, '0');
                string D = pc.GetDayOfMonth(dtN).ToString().PadLeft(2, '0');
                //string strTodayCondition0 = Y + M + D; ;
                string strToDate = Y + "/" + M + "/" + D;

                // ---
                string strDateCondition = string.Format(@" s.prod_date >= '{0}' and s.prod_date < '{1}' ", strFromDate, strToDate);
                string strLFDDateCondition = string.Format(@" s.prodenddate > to_date('{0}','yyyy/mm/dd','nls_calendar=persian') and s.prodenddate < to_date('{1}','yyyy/mm/dd','nls_calendar=persian') ", strFromDate, strToDate);
                //string strDateCondition0 = "13980101";

                // create Archive commande
                string commandtext = string.Format(@"select SYS_GUID() as Id,ProdDateFa,sum(ProductCount) as ProductCount,sysdate as U_DateTime,TO_char(sysdate,'YYYY/MM/DD HH24:MI:SS','nls_calendar=persian') as U_DateTimeFa 
                                                    from 
                                                    (
                                                    --brand saipa
                                                    select to_char(trunc(s.prodenddate),'yyyy/mm/dd','nls_calendar=persian') as ProdDateFa,s.companycode,case when companycode=82 then 'سایپا'  when companycode=83 then 'سایپا سیتروئن'   when companycode=90 then 'بن رو' when companycode=81 then 'پارس خودرو' else 'other' end as CompanyName,gs.name,bm.aliasname,case when bm.aliasname='تيبا 211' then 'تیبا2' when bm.aliasname='تيبا 212'  or bm.aliasname='تيبا 232' then NAME else bm.aliasname end as CommonBodyModelName 
                                                    ,count(vin)as ProductCount 
                                                                from pt.VW_LFD_ProdJoin s
                                                                JOIN BODYMODEL bm on s.bdmdlcode=bm.bdmdlcode 
                                                                JOIN groupsproductsmsgroup gs on gs.smsgrpid=bm.smsgrpid 
                                                    where companycode not in (92,97) and {1}
                                                        group by companycode,gs.name,bm.aliasname,trunc(s.prodenddate) 
                                                    union
                                                    --parskhodro
                                                    select prod_date as ProdDateFa,companycode,CompanyName,NAME,NAME as ALIASNAME,NAME as CommonBodyModelName,sum(prod_qty) as cnt from 
                                                    (
                                                    select s.prod_date,case when comp_code=002 then 81 when comp_code=004 then 86 end as companycode,case when comp_code=002  then 'پارس خودرو' when comp_code=004 then 'زامیاد' else 'other' end as CompanyName
                                                    ,(case when s.pubrnd_id =36 then 'H220 برليانس' when s.pubrnd_id = 41 then 'کوئيک' when s.pubrnd_id = 35 then 'برليانس CROSS' when ((comp_code=002)and((s.pubrnd_id =  4)OR(s.pubrnd_id =  25))) then 'تندر' when s.pubrnd_id = 12 then 'ريچ' when (s.pubrnd_id = 4 and comp_code=002 ) then 'کاميونت پادرا' when s.pubrnd_id = 1 then 'نيسان' else s.pubrnd_name end)
                                                    as name ,s.prod_qty,s.pubrnd_id,s.comp_code
                                                    from saipagroup.group_products_data s 
                                                    where comp_code in (002,004) and   {0}
                                                    ) q
                                                    group by q.NAME,prod_date,companycode,CompanyName,NAME having sum(prod_qty)<>0 
                                                    union
                                                    -- diesel
                                                    select s.prod_date as ProdDateFa,89 as companycode,'سایپا دیزل' as CompanyName,'خودروهای سنگین' As NAME,'خودروهای سنگین' As ALIASNAME,'خودروهای سنگین' As CommonBodyModelName,sum(s.prod_qty) as cnt
                                                        from   saipagroup.diesel_products_data s 
                                                    group by s.prod_date  having {0}  and sum(s.prod_qty) <>0
                                                    order by ProdDateFa,companycode
                                                    ) z 
                                                    group by ProdDateFa
                                                    order by ProdDateFa
                                                    ", strDateCondition, strLFDDateCondition);

                List<GroupProductStatistics> lst = new List<GroupProductStatistics>();
                
                lst = DBHelper.GetDBObjectByObj2(new GroupProductStatistics(), null, commandtext, "pt").Cast<GroupProductStatistics>().ToList();
                return lst;
            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("GetArchiveGroupProdStatistics" + ex.Message.ToString());
                return null;


            }
        }


        public static List<QCStatistics> GetArchiveQCStatistics(bool JustTodayStatistic)
        {
            try
            {
                string strFromDate = "1398/01/01";
                PersianCalendar pc = new PersianCalendar();
                DateTime dtN = DateTime.Now;
                string Y = pc.GetYear(dtN).ToString();
                string M = pc.GetMonth(dtN).ToString().PadLeft(2, '0');
                string D = pc.GetDayOfMonth(dtN).ToString().PadLeft(2, '0');
                //string strTodayCondition0 = Y + M + D; ;
                string strToDate = Y + "/" + M + "/" + D;
                string strDateCondition = "";
                // ---
                if (JustTodayStatistic)
                    strDateCondition= string.Format(@" trunc(q.CreatedDate) = to_date('{0}/{1}/{2}','yyyy/mm/dd','nls_calendar=persian') ", Y, M, D);
                else
                    strDateCondition = string.Format(@" q.CreatedDate >= to_date('{0}','yyyy/mm/dd','nls_calendar=persian') and q.CreatedDate < to_date('{1}','yyyy/mm/dd','nls_calendar=persian') ", strFromDate, strToDate);
                //string strDateCondition0 = "13980101";
                // create Archive commande
                string commandtext = string.Format(@"select SYS_GUID() as Id
                                                    --,TO_char(sysdate,'YYYY/MM/DD HH24:MI:SS','nls_calendar=persian') as U_DateTimeFa 
                                                    --,trunc(q.CreatedDate) as CreatedDate
                                                    --,to_char(trunc(q.createddate),'yyyy/mm/dd','nls_calendar=persian') as createddateFa
                                                    ,to_number(to_char(trunc(q.createddate),'yyyymmdd','nls_calendar=persian')) as CreatedDateFaNum
                                                    ,cg.grpcode,bm.bdmdlcode
                                                    ,s.companycode
                                                    ,decode(a.areatype,35,(decode(p.shopcode,14,30,17,40)),a.areatype) as areatype
                                                    ,p.shopcode,q.qcareat_srl
                                                    --,q.qcmdult_srl,q.qcbadft_srl
                                                    ,count(distinct q.vin) as DetectCarCount,
                                                    count(distinct (decode(q.isdefected,1,q.vin,null))) as DefCarCount,
                                                    count(distinct (decode(q.isdefected,0,q.vin,null))) as StrCarCount,
                                                    count( (decode(q.isdefected,1,q.vin,null))) as RegDefCount,
                                                    count( (decode(q.isdefected,0,q.vin,null))) as StrCount,
                                                    count(distinct (decode(q.qcstrgt_srl,62,q.vin,42,q.vin,101,q.vin,null))) as ASPCarCount,
                                                    count( (decode(q.qcstrgt_srl,62,q.vin,42,q.vin,101,q.vin,null))) as ASPRegDefCount,
                                                    count(distinct (decode(q.qcstrgt_srl,62,q.vin,42,q.vin,101,q.vin,63,q.vin,null))) as BPlusCarCount,
                                                    count( (decode(q.qcstrgt_srl,62,q.vin,42,q.vin,101,q.vin,63,q.vin,null))) as BPlusRegDefCount
                                                    from qccastt q join carid c on c.vin = q.vin join bodymodel bm on bm.bdmdlcode=c.bdmdlcode 
                                                    --join pt.groupsproductsmsgroup gs on gs.smsgrpid=bm.smsgrpid
                                                    join cargroup cg on cg.grpcode=bm.grpcode
                                                    join qcareat a on a.srl = q.qcareat_srl 
                                                    join pcshopt p on p.srl = a.pcshopt_srl
                                                    join pt.shop s on s.shopcode=p.ptshopcode
                                                    where q.recordowner=1 and q.inuse=1 and s.companycode = 82 
                                                    and a.areacode not in (1000,905) 
                                                    and a.areatype2 <>3 and a.isauditarea=0 
                                                    and {0} 
                                                    group by trunc(q.createddate),cg.grpcode,s.companycode,bm.bdmdlcode,p.shopcode,a.areatype,q.qcareat_srl
                                                             --,q.qcmdult_srl,q.qcbadft_srl
                                                    "
                    , strDateCondition);
                List<QCStatistics> lst = new List<QCStatistics>();

                lst = DBHelper.GetDBObjectByObj2(new QCStatistics(), null, commandtext, "ins").Cast<QCStatistics>().ToList();
                return lst;
            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("GetArchiveQCStatistics_Err: " + ex.Message.ToString());
                return null;


            }
        }


        public static List<Qccastt> GetASPQccastt(bool JustCurMonthStatistic)
        {
            try
            {
                string strFromDate = "1399/01/01";
                PersianCalendar pc = new PersianCalendar();
                DateTime dtN = DateTime.Now;
                string Y = pc.GetYear(dtN).ToString();
                string M = pc.GetMonth(dtN).ToString().PadLeft(2, '0');
                string D = pc.GetDayOfMonth(dtN).ToString().PadLeft(2, '0');
                //string strTodayCondition0 = Y + M + D; ;
                string strCurMonthDate = Y + "/" + M + "/" + "01";
                string strToDate = Y + "/" + M + "/" + D;
                string strDateCondition = "";
                // ---
                if (JustCurMonthStatistic)
                    //strDateCondition = string.Format(@" trunc(q.CreatedDate) = to_date('{0}/{1}/{2}','yyyy/mm/dd','nls_calendar=persian') ", Y, M, D);
                    strDateCondition = string.Format(@" trunc(q.CreatedDate) >= trunc(to_date('{0}','yyyy/mm/dd','nls_calendar=persian')) ", strCurMonthDate);
                else
                    strDateCondition = string.Format(@" q.CreatedDate >= to_date('{0}','yyyy/mm/dd','nls_calendar=persian') and q.CreatedDate < to_date('{1}','yyyy/mm/dd','nls_calendar=persian') ", strFromDate, strToDate);
                //string strDateCondition0 = "13980101";
                // create Archive commande
                string commandtext = string.Format(@"select SYS_GUID() as Id,q.srl,q.vin,
                                                               a.areacode,
                                                               s.strenghtdesc,
                                                               m.modulecode,
                                                               d.defectcode,q.qcbadft_srl,
                                                               m.modulename,q.qcmdult_srl,
                                                               d.defectdesc,
                                                               bm.grpcode,
                                                               cg.grpname,
                                                               t.title,
                                                               q.inuse,
                                                               a.areacode||' '||a.areadesc as AreaDesc,
                                                               u.lname as CreatedByDesc,q.CreatedBy,
                                                               ur.lname as RepairedByDesc,q.RepairedBy,
                                                               TO_char(q.createddate,'YYYY/MM/DD HH24:MI:SS','nls_calendar=persian') as createddateFa,
                                                               to_char(q.createddate,'yyyy/mm/dd','nls_calendar=persian') as CreatedDayFa,
                                                               q.isrepaired,c.bdmdlcode,a.areatype,bm.grpcode,p.shopcode,q.qcareat_srl
                                                          from qccastt q
                                                          join qcusert u on u.srl = q.createdby
                                                          left join qcusert ur on ur.srl = q.RepairedBy
                                                          join carid c on c.vin = q.vin 
                                                          join bodymodel bm on bm.bdmdlcode=c.bdmdlcode
                                                          join qcareat a
                                                            on q.qcareat_srl = a.srl
                                                          left join pcshopt p on p.srl = a.pcshopt_srl
                                                          left join pt.shop ss on ss.shopcode=p.ptshopcode
                                                          join qcmdult m
                                                            on q.qcmdult_srl = m.srl
                                                          join qcbadft d
                                                            on q.qcbadft_srl = d.srl
                                                          join qcstrgt s
                                                            on q.qcstrgt_srl = s.srl
                                                          join cargroup cg on cg.grpcode=bm.grpcode
                                                          join qccabdt t on t.srl = d.qccabdt_srl
                                                         where q.inuse=1 and q.recordowner=1 and q.isdefected=1
                                                         and (ss.companycode = 82 or q.qcareat_srl in (441,641,94,101) )
                                                         and a.areacode not in (1000,905) 
                                                         and s.srl in (42,62,101)
                                                         And {0}
                                                         order by q.createddate desc
                                                    "
                    , strDateCondition);
                List<Qccastt> lst = new List<Qccastt>();

                lst = DBHelper.GetDBObjectByObj2(new Qccastt(), null, commandtext, "ins").Cast<Qccastt>().ToList();
                return lst;
            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("GetASPQccastt_Err: " + ex.Message.ToString());
                return null;


            }
        }

        public static List<QCHStatistics> GetHQCStatistics()
        {
            try
            {

                string commandtext = string.Format(@"select SYS_GUID() as Id,
                                                   TO_CHAR(sysdate,'MM/DD','nls_calendar=persian') ||' ' || lpad(h.hour, 2, '0') as hour,
                                                   cg.grpcode,bm.bdmdlcode,s.companycode,decode(a.areatype,35,(decode(p.shopcode, 14, 30, 17, 40)),a.areatype) as areatype,
                                                   p.shopcode,q.qcareat_srl,count(distinct q.vin) as DetectCarCount,count(distinct (decode(q.isdefected,1,q.vin,null))) as DefCarCount,
                                                   count( (decode(q.isdefected,1,q.vin,null))) as RegDefCount
                                              from Hours h
                                              left join qccastt q on lpad(h.hour, 2, '0') = TO_CHAR(q.createddate, 'HH24')
                                              left join carid c on c.vin = q.vin left join bodymodel bm on bm.bdmdlcode=c.bdmdlcode 
                                              left join cargroup cg on cg.grpcode=bm.grpcode
                                              left join qcareat a on a.srl = q.qcareat_srl 
                                              left join pcshopt p on p.srl = a.pcshopt_srl
                                              left join pt.shop s on s.shopcode=p.ptshopcode
                                             where trunc(q.createddate) > trunc(sysdate - 1)
                                                   and q.recordowner=1 and q.inuse=1 and (s.companycode = 82 or q.qcareat_srl in (441,641,94,101) ) and a.areacode not in (1000,905) 
                                                   and a.areatype2 <>3 and a.isauditarea=0 
                                             group by lpad(h.hour, 2, '0'),cg.grpcode,s.companycode,bm.bdmdlcode,p.shopcode,a.areatype,q.qcareat_srl
                                             order by lpad(h.hour, 2, '0')");
                List<QCHStatistics> lst = new List<QCHStatistics>();

                lst = DBHelper.GetDBObjectByObj2(new QCHStatistics(), null, commandtext, "ins").Cast<QCHStatistics>().ToList();
                return lst;
            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("GetHQCStatistics_Err: " + ex.Message.ToString());
                return null;


            }
        }

        public static List<CarStatus> GetCarStatus()
        {
            try
            {
                string strFromDate = "1398/01/01";

                string strDateCondition = "";
                strDateCondition = string.Format(@" c.joinerydate >= to_date('{0}','yyyy/mm/dd','nls_calendar=persian') ", strFromDate);
                // ---
                string commandtext = string.Format(@"select z.*,c.comanyname as CompanyName from (select SYS_GUID() as Id, bm.grpcode,c.bdmdlcode,c.vin,c.finqccode,sci.status_code,pt.FNI_GetAsmProdCompanyCodeByVin(c.vin) as companycode,to_number(TO_Char(c.joinerydate,'YYYYMMDD','nls_calendar=persian')) as JoineryDateFaNum,TO_Char(c.joinerydate,'YYYY/MM/DD','nls_calendar=persian') as JoineryDateFa
                                                        from pt.carid c left join sale.car_id@saleguard_priprctl sci on sci.prod_no = c.prodno
                                                        join bodymodel bm on bm.bdmdlcode=c.bdmdlcode
                                                        join cargroup cg on cg.grpcode=bm.grpcode
                                                        where  {0}) z join pt.companies c on z.companycode =c.companycode "
                                            , strDateCondition);
                List<CarStatus> lst = new List<CarStatus>();
                lst = DBHelper.GetDBObjectByObj2(new CarStatus(), null, commandtext, "ins").Cast<CarStatus>().ToList();
                return lst;
            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("GetCarStatus_Err: " + ex.Message.ToString());
                return null;


            }
        }

        public static List<AuditStatistics> GetArchiveAuditStatistics(bool JustTodayStatistic,String _AreaCodes)
        {
            try
            {
                string strFromDate = "1398/01/01";
                PersianCalendar pc = new PersianCalendar();
                DateTime dtN = DateTime.Now;
                string Y = pc.GetYear(dtN).ToString();
                string M = pc.GetMonth(dtN).ToString().PadLeft(2, '0');
                string D = pc.GetDayOfMonth(dtN).ToString().PadLeft(2, '0');
                //string strTodayCondition0 = Y + M + D; ;
                string strToDate = Y + "/" + M + "/" + D;
                string strDateCondition = "";
                // ---
                if (JustTodayStatistic)
                    strDateCondition = string.Format(@" trunc(a.AuditDate) = to_date('{0}/{1}/{2}','yyyy/mm/dd','nls_calendar=persian') ", Y, M, D);
                else
                    strDateCondition = string.Format(@" a.AuditDate >= to_date('{0}','yyyy/mm/dd','nls_calendar=persian') and a.AuditDate < to_date('{1}','yyyy/mm/dd','nls_calendar=persian') ", strFromDate, strToDate);
                //string strDateCondition0 = "13980101";
                // create Archive commande
                string commandtext = string.Format(@"select SYS_GUID() as Id,
                                                           a.AUDITDATE_FA,
                                                           to_number(to_char(trunc(a.AUDITDATE),'yyyymmdd','nls_calendar=persian')) as AuditDateFaNum,
                                                           a.QCAREAT_SRL,a.AREACODE,a.GRPCODE,a.BDMDLCODE,a.BDSTLCODE,
                                                           sum(a.COUNTOFMODULEDEFECT)as COUNTOFMODULEDEFECT,
                                                           sum(a.COUNTOFADEFECT) as COUNTOFADEFECT,
                                                           sum(a.COUNTOFBDEFECT) as COUNTOFBDEFECT,
                                                           sum(a.COUNTOFCDEFECT) as COUNTOFCDEFECT,
                                                           sum(a.COUNTOFSDEFECT) as COUNTOFSDEFECT,
                                                           sum(a.COUNTOFPDEFECT) as COUNTOFPDEFECT,
                                                           sum(a.COUNTOFBPLUSDEFECT) as COUNTOFBPLUSDEFECT,
                                                           sum(a.SUMOFNEGATIVESCOREVALUE) as SUMOFNEGATIVESCOREVALUE,
                                                           count(*) as AuditCarCount 
                                                    from sva_v_auditcar a 
                                                    where  a.SVAAUDITVART_SRL=1 and a.SVAAUDITTYPE_SRL = 1 and a.AreaCode in ({0}) 
                                                           And {1}
                                                    group by a.AUDITDATE_FA,a.AUDITDATE,a.QCAREAT_SRL,a.AREACODE
	                                                        ,a.GRPCODE,a.BDMDLCODE,a.BDSTLCODE
                                                    "
                    , _AreaCodes, strDateCondition);
                List<AuditStatistics> lst = new List<AuditStatistics>();

                lst = DBHelper.GetDBObjectByObj2(new AuditStatistics(), null, commandtext, "ins").Cast<AuditStatistics>().ToList();
                return lst;
            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("GetArchiveAuditStatistics: " + ex.Message.ToString());
                return null;


            }
        }

        public static List<AuditStatisticsMDTrend> GetArchiveMDTrendAuditStatistics(String _QCAreatSrls)
        {
            try
            {
                //string strFromDate = "1398/01/01";
                //PersianCalendar pc = new PersianCalendar();
                //DateTime dtN = DateTime.Now;
                //string Y = pc.GetYear(dtN).ToString();
                //string M = pc.GetMonth(dtN).ToString().PadLeft(2, '0');
                //string D = pc.GetDayOfMonth(dtN).ToString().PadLeft(2, '0');
                ////string strTodayCondition0 = Y + M + D; ;
                //string strToDate = Y + "/" + M + "/" + D;
                //string strDateCondition = "";
                //// ---
                //if (JustTodayStatistic)
                //    strDateCondition = string.Format(@" trunc(a.AuditDate) = to_date('{0}/{1}/{2}','yyyy/mm/dd','nls_calendar=persian') ", Y, M, D);
                //else
                //    strDateCondition = string.Format(@" a.AuditDate >= to_date('{0}','yyyy/mm/dd','nls_calendar=persian') and a.AuditDate < to_date('{1}','yyyy/mm/dd','nls_calendar=persian') ", strFromDate, strToDate);
                //string strDateCondition0 = "13980101";
                // create Archive commande
                string commandtext = string.Format(@"select SYS_GUID() as Id,z.*,round(WeekNScoreSum/AuditCarCount,2) as AvgNScore
                                                         from 
                                                        (select to_number(a.YearWeekNo)as yearweekno,a.qcareat_srl,a.GRPCODE,a.BDMDLCODE,a.BDSTLCODE,d.qcmdult_srl,d.qcbadft_srl,t.WEEKDetectMDCOUNT,round(t.MDREGR,3) as MDREGR
                                                               ,sum(d.negativescorevalue) as WeekNScoreSum
                                                               ,(select sum(ac.auditedcarcount) from svaauditcarcnt ac where ac.YEARWEEKNO=a.YEARWEEKNO and ac.QCAREAT_SRL=a.QCAREAT_SRL and ac.GRPCODE=a.GRPCODE and ac.BDMDLCODE=a.BDMDLCODE and ac.BDSTLCODE=a.BDSTLCODE) as AuditCarCount
                                                          from svaauditcardetail d
                                                          join sva_v_auditcar a
                                                            on a.srl = d.svaauditcar_srl
                                                            join sva_v_audittopregrmd t on t.QCMDULT_SRL = d.qcmdult_srl and t.QCBADFT_SRL=d.qcbadft_srl and t.QCAREAT_SRL=a.QCAREAT_SRL  
                                                           right join svaywt w
                                                            on w.yearweekno = a.YearWeekNo
                                                         Where w.inuse = 1 and 
                                                           a.YEARWEEKNO >= (TO_char(sysdate-84, 'YY', 'nls_calendar=persian') || fn_getweekofyearsaipa2(sysdate-84))
                                                             and a.YEARWEEKNO < (TO_char(sysdate, 'YY', 'nls_calendar=persian') || fn_getweekofyearsaipa2(sysdate))
                                                             and a.qcareat_srl in ({0})
      
                                                         group by a.YearWeekNo,a.qcareat_srl,a.GRPCODE,a.BDMDLCODE,a.BDSTLCODE,d.qcmdult_srl,d.qcbadft_srl,t.MDREGR,t.WEEKDetectMDCOUNT
                                                         order by a.QCAREAT_SRL,yearweekno,WEEKDetectMDCOUNT desc , MDREGR desc
                                                        ) z ", _QCAreatSrls); //, strDateCondition
                List<AuditStatisticsMDTrend> lst = new List<AuditStatisticsMDTrend>();

                lst = DBHelper.GetDBObjectByObj3(new AuditStatisticsMDTrend(), null, commandtext, "ins").Cast<AuditStatisticsMDTrend>().ToList();
                return lst;
            }
            catch (Exception ex)
            {
                LogManager.SetCommonLog("GetArchiveMDTrendAuditStatistics: " + ex.Message.ToString());
                return null;
            }
        }

    }
}