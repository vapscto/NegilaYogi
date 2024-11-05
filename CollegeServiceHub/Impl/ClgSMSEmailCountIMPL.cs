using CollegeServiceHub.Interface;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.Birthday;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Impl
{
    public class ClgSMSEmailCountIMPL : ClgSMSEmailCountInterface
    {
        public ClgAdmissionContext _clgadmctxt;
        public DomainModelMsSqlServerContext _db;
        public ClgSMSEmailCountIMPL(ClgAdmissionContext obj, DomainModelMsSqlServerContext obj1)
        {
            _clgadmctxt = obj;
            _db = obj1;
        }
        public ClgSMSEmailCountDTO getdata(ClgSMSEmailCountDTO data)
        {
            try
            {
                using (var cmd = _clgadmctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ClgEmailSmsCount";
                    cmd.CommandType = CommandType.StoredProcedure;
                
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.Modulelist = retObject.ToArray();

                    }


                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public ClgSMSEmailCountDTO getreport(ClgSMSEmailCountDTO rpt)
        {
           
            try
            {
                List<string> IVRMM_ModuleName = new List<string>();
                if(rpt.modulenameslist !=null)
                {
                    foreach (var item in rpt.modulenameslist)
                    {
                        IVRMM_ModuleName.Add(item.IVRMM_ModuleName);
                    }
                       
                }
                List<string> To_FLag = new List<string>();
                if (rpt.To_FLagList != null)
                {
                    foreach (var rt in rpt.To_FLagList)
                    {
                        To_FLag.Add(rt.To_FLag);
                    }

                }                            
                if (rpt.rdbbutton == "smscount")
                {
                    rpt.sms_mail_count = _db.IVRM_sms_sentBoxDMO.Where(d => d.MI_Id == rpt.MI_Id && d.Datetime.Date >= rpt.start_date.Value.Date && d.Datetime.Date <= rpt.end_date.Value.Date && IVRMM_ModuleName.Contains(d.Module_Name) && To_FLag.Contains(d.To_FLag)).Distinct().ToArray();                    
                    rpt.count = rpt.sms_mail_count.Length;
                }
                else if (rpt.rdbbutton == "emailcount")
                {
                    rpt.mail_count_list = _db.ivrm_email_sentbox.Where(d => d.MI_Id == rpt.MI_Id && d.Datetime.Value.Date >= rpt.start_date.Value.Date && d.Datetime.Value.Date <= rpt.end_date.Value.Date && To_FLag.Contains(d.To_FLag) && IVRMM_ModuleName.Contains(d.Module_Name)).Distinct().ToArray();                   
                  
                    rpt.count = rpt.mail_count_list.Length;
                }
                //rptmonth
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return rpt;
        }

        public ClgSMSEmailCountDTO SearchByColumn(ClgSMSEmailCountDTO search)
        {
            try
            {
                if (search.SearchColumn == "" || search.SearchColumn == null)
                {
                    search.SearchColumn = "0";
                }
                switch (search.SearchColumn)
                {
                    case "0":
                        if (search.rdbbutton == "smscount")
                        {
                            var query1 = _db.IVRM_sms_sentBoxDMO.Where(d => d.MI_Id == search.MI_Id && d.Datetime.Date >= search.start_date.Value.Date && d.Datetime.Date <= search.end_date.Value.Date && d.Module_Name.Contains(search.EnteredData)).ToList();
                            if (query1.Count > 0)
                            {
                                search.sms_mail_count = query1.ToArray();
                                search.count = query1.Count;
                            }
                            else
                            {
                                search.count = 0;
                            }
                        }
                        else if (search.rdbbutton == "emailcount")
                        {
                            var query1 = _db.ivrm_email_sentbox.Where(d => d.MI_Id == search.MI_Id && d.Datetime.Value.Date >= search.start_date.Value.Date && d.Datetime.Value.Date <= search.end_date.Value.Date && d.Module_Name.Contains(search.EnteredData)).ToList();
                            if (query1.Count > 0)
                            {
                                search.mail_count_list = query1.ToArray();
                                search.count = query1.Count;

                            }
                            else
                            {
                                search.count = 0;
                            }

                        }
                        break;
                    case "1":

                        if (search.rdbbutton == "emailcount")
                        {
                            var query1 = _db.ivrm_email_sentbox.Where(d => d.MI_Id == search.MI_Id && d.Datetime.Value.Date >= search.start_date.Value.Date && d.Datetime.Value.Date <= search.end_date.Value.Date && d.Email_Id.Contains(search.EnteredData)).ToList();
                            if (query1.Count > 0)
                            {
                                search.mail_count_list = query1.ToArray();
                                search.count = query1.Count;
                            }
                            else
                            {
                                search.count = 0;
                            }

                        }
                        break;
                    case "2":
                        if (search.rdbbutton == "smscount")
                        {
                            var query1 = _db.IVRM_sms_sentBoxDMO.Where(d => d.MI_Id == search.MI_Id && d.Datetime.Date >= search.start_date.Value.Date && d.Datetime.Date <= search.end_date.Value.Date && d.Message.Contains(search.EnteredData)).ToList();
                            if (query1.Count > 0)
                            {
                                search.sms_mail_count = query1.ToArray();
                                search.count = query1.Count;
                            }
                            else
                            {
                                search.count = 0;
                            }
                        }
                        else if (search.rdbbutton == "emailcount")
                        {
                            var query1 = _db.ivrm_email_sentbox.Where(d => d.MI_Id == search.MI_Id && d.Datetime.Value.Date >= search.start_date.Value.Date && d.Datetime.Value.Date <= search.end_date.Value.Date && d.Message.Contains(search.EnteredData)).ToList();
                            if (query1.Count > 0)
                            {
                                search.mail_count_list = query1.ToArray();
                                search.count = query1.Count;
                            }
                            else
                            {
                                search.count = 0;
                            }

                        }
                        break;
                    case "3":
                        try
                        {
                            DateTime date = DateTime.ParseExact(search.EnteredData, "dd/MM/yyyy",
                                 CultureInfo.InvariantCulture);
                            if (search.rdbbutton == "smscount")
                            {
                                var query1 = _db.IVRM_sms_sentBoxDMO.Where(d => d.MI_Id == search.MI_Id && d.Datetime.Date >= search.start_date.Value.Date && d.Datetime.Date <= search.end_date.Value.Date && d.Datetime.Date == Convert.ToDateTime(date.ToString("yyyy-MM-dd"))).ToList();
                                if (query1.Count > 0)
                                {
                                    search.sms_mail_count = query1.ToArray();
                                    search.count = query1.Count;
                                }
                                else
                                {
                                    search.count = 0;
                                }
                            }
                            else if (search.rdbbutton == "emailcount")
                            {
                                var query1 = _db.ivrm_email_sentbox.Where(d => d.MI_Id == search.MI_Id && d.Datetime.Value.Date >= search.start_date.Value.Date && d.Datetime.Value.Date <= search.end_date.Value.Date && d.Datetime.Value.Date == Convert.ToDateTime(date.ToString("yyyy-MM-dd"))).ToList();
                                if (query1.Count > 0)
                                {
                                    search.mail_count_list = query1.ToArray();
                                    search.count = query1.Count;
                                }
                                else
                                {
                                    search.count = 0;
                                }

                            }

                        }
                        catch (Exception ex)
                        {
                            search.message = "Please Enter date in dd/MM/yyyy format";
                            Console.WriteLine(ex.Message);
                            if (search.rdbbutton == "smscount")
                            {
                                var query1 = _db.IVRM_sms_sentBoxDMO.Where(d => d.MI_Id == search.MI_Id && d.Datetime.Date >= search.start_date.Value.Date && d.Datetime.Date <= search.end_date.Value.Date).ToList();
                                if (query1.Count > 0)
                                {
                                    search.sms_mail_count = query1.ToArray();
                                    search.count = query1.Count;
                                }
                                else
                                {
                                    search.count = 0;
                                }
                            }
                            else if (search.rdbbutton == "emailcount")
                            {
                                var query1 = _db.ivrm_email_sentbox.Where(d => d.MI_Id == search.MI_Id && d.Datetime.Value.Date >= search.start_date.Value.Date && d.Datetime.Value.Date <= search.end_date.Value.Date).ToList();
                                if (query1.Count > 0)
                                {
                                    search.mail_count_list = query1.ToArray();
                                    search.count = query1.Count;
                                }
                                else
                                {
                                    search.count = 0;
                                }

                            }
                        }
                        break;
                    case "4":
                        if (search.rdbbutton == "smscount")
                        {
                            var query1 = _db.IVRM_sms_sentBoxDMO.Where(d => d.MI_Id == search.MI_Id && d.Datetime.Date >= search.start_date.Value.Date && d.Datetime.Date <= search.end_date.Value.Date && d.Mobile_no.Contains(search.EnteredData)).ToList();
                            if (query1.Count > 0)
                            {
                                search.sms_mail_count = query1.ToArray();
                                search.count = query1.Count;
                            }
                            else
                            {
                                search.count = 0;
                            }
                        }
                        break;
                    case "5":
                        if (search.rdbbutton == "smscount")
                        {
                            var query1 = _db.IVRM_sms_sentBoxDMO.Where(d => d.MI_Id == search.MI_Id && d.Datetime.Date >= search.start_date.Value.Date && d.Datetime.Date <= search.end_date.Value.Date && d.statusofmessage.Contains(search.EnteredData)).ToList();
                            if (query1.Count > 0)
                            {
                                search.sms_mail_count = query1.ToArray();
                                search.count = query1.Count;
                            }
                            else
                            {
                                search.count = 0;
                            }
                        }
                        break;
                    default:
                        if (search.rdbbutton == "smscount")
                        {
                            var query1 = _db.IVRM_sms_sentBoxDMO.Where(d => d.MI_Id == search.MI_Id && d.Datetime.Date >= search.start_date.Value.Date && d.Datetime.Date <= search.end_date.Value.Date).ToList();
                            if (query1.Count > 0)
                            {
                                search.sms_mail_count = query1.ToArray();
                                search.count = query1.Count;
                            }
                            else
                            {
                                search.count = 0;
                            }
                        }
                        else if (search.rdbbutton == "emailcount")
                        {
                            var query1 = _db.ivrm_email_sentbox.Where(d => d.MI_Id == search.MI_Id && d.Datetime.Value.Date >= search.start_date.Value.Date && d.Datetime.Value.Date <= search.end_date.Value.Date).ToList();
                            if (query1.Count > 0)
                            {
                                search.mail_count_list = query1.ToArray();
                                search.count = query1.Count;
                            }
                            else
                            {
                                search.count = 0;
                            }

                        }
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return search;
        }
    }
}
