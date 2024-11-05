using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.VMS.HRMS;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.IssueManager;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using PreadmissionDTOs.com.vaps.VMS.Training;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace Recruitment.com.vaps.Services
{
    public class SalesSMSEMAILService : Interfaces.SalesSMSEMAILInterface
    {

        public VMSContext _VMSContext;
        public DomainModelMsSqlServerContext _db;
        public SalesSMSEMAILService(VMSContext VMSContext, DomainModelMsSqlServerContext Context)
        {
            _VMSContext = VMSContext;
            _db = Context;
        }

        public SalesSMSEMAILDTO getBasicData(SalesSMSEMAILDTO data)
        {
         
            try
            {
                data.categorylist = _VMSContext.ISM_Sales_Master_Category_DMO_con.Where(a => a.MI_Id == data.MI_Id && a.ISMSMCA_ActiveFlag == true).Distinct().OrderBy(e => e.ISMSMCA_CategoryName).ToArray();

                data.sourcelist = _VMSContext.ISM_Sales_Master_Source_DMO_con.Where(a => a.MI_Id == data.MI_Id && a.ISMSMSO_ActiveFlag == true).Distinct().OrderBy(e => e.ISMSMSO_SourceName).ToArray();


                data.statuslist = _VMSContext.ISM_Sales_Master_Status_DMO_con.Where(a => a.MI_Id == data.MI_Id && a.ISMSMST_ActiveFlag == true).Distinct().OrderBy(e => e.ISMSMST_StatusName).ToArray();

                data.productlist = _VMSContext.ISM_Sales_Master_Product_DMO_con.Where(a => a.MI_Id == data.MI_Id && a.ISMSMPR_ActiveFlag == true).Distinct().OrderBy(e => e.ISMSMPR_ProductName).ToArray();

                data.countrylist = _VMSContext.IVRM_Master_Country.Distinct().OrderBy(f => f.IVRMMC_CountryName).ToArray();



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                data.retrunMsg = "Error occured";
            }
            return data;
        }

        public SalesSMSEMAILDTO sendsmsemail(SalesSMSEMAILDTO data)
        {
            
            try
            {
                if (data.type=="TPL" || data.type == "GREET")
                {

                    if (data.snd_sms==true)
                    {
                        if (data.selected.Length>0)
                        {
                            foreach (var item in data.selected)
                            {
                                if (item.ISMSLE_ContactNo!=0)
                                {
                                   
                                    string a = sendSms(data.MI_Id, item.ISMSLE_ContactNo, data.template, item.ISMSLE_Id).Result;
                                }
                            }


                        }
                    }


                    if (data.snd_email == true)
                    {
                        if (data.selected.Length > 0)
                        {
                            foreach (var item in data.selected)
                            {
                                if (item.ISMSLE_EmailId != "" && item.ISMSLE_EmailId !=null)
                                {

                                    if (item.ISMSLE_EmailId.Trim().ToLower()!="test@gmail.com")
                                    {
                                        string a = sendmail(data.MI_Id, item.ISMSLE_EmailId, data.template, item.ISMSLE_Id);
                                    }

                                 
                                }
                            }


                        }
                    }
                }
                else if (data.type == "NONTPL")
                {
                    if (data.snd_sms == true)
                    {
                        if (data.selected.Length > 0)
                        {
                            foreach (var item in data.selected)
                            {
                                if (item.ISMSLE_ContactNo != 0)
                                {
                                    
                                    string a = sendSmsWT(data.MI_Id, item.ISMSLE_ContactNo,  item.ISMSLE_Id,data.smsmsg, item.ISMSLE_ContactPerson).Result;
                                }
                            }


                        }
                    }


                    if (data.snd_email == true)
                    {
                        if (data.selected.Length > 0)
                        {
                            foreach (var item in data.selected)
                            {
                                if (item.ISMSLE_EmailId != "" && item.ISMSLE_EmailId != null)
                                {
                                    if (item.ISMSLE_EmailId.Trim().ToLower() != "test@gmail.com")
                                    {
                                        string a = sendmailWT(data.MI_Id, item.ISMSLE_EmailId, data.msg, item.ISMSLE_Id,data.esubject,data.Footer,data.filelist,item.ISMSLE_ContactPerson,item.ISMSLE_LeadName,data.athflag,data.FHEAD);
                                }

                                }
                            }


                        }
                    }
                }

                data.retrunMsg = "true";
            }


          
            catch (Exception ee)
            {
                data.retrunMsg = "false";
                Console.WriteLine(ee.Message);
            }

            return data;
        }

        public SalesSMSEMAILDTO editData(int id)
        {

            SalesSMSEMAILDTO dto = new SalesSMSEMAILDTO();
            dto.retrunMsg = "";
            try
            {
                List<HR_Master_PriorityDMO> lorg = new List<HR_Master_PriorityDMO>();
                lorg = _VMSContext.HR_Master_PriorityDMO.AsNoTracking().Where(t => t.HRMPR_Id.Equals(id)).ToList();
                dto.PriorityList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }
        public SalesSMSEMAILDTO get_state(SalesSMSEMAILDTO dto)
        {
            try
            {
                if (dto.stateids.Length>0)
                {
                    List<long> ids = new List<long>();
                    foreach (var item in dto.stateids)
                    {
                        ids.Add(item.ivrmmC_Id);
                    }


                    dto.statelist = _VMSContext.IVRM_Master_State.Where(a => ids.Contains(a.IVRMMC_Id)).Distinct().OrderBy(f => f.IVRMMS_Name).ToArray(); ;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
               
          
            return dto;
        }
        public SalesSMSEMAILDTO getrpt(SalesSMSEMAILDTO data)
        {
            try
            {
                data.templatelist = (from a in _VMSContext.SMSEmailSetting
                                     from b in _VMSContext.Institution_Module_Page
                                     from c in _VMSContext.MasterPage
                                     from d in _VMSContext.ISM_Sales_Master_Source_DMO
                                     where a.MI_Id == data.MI_Id && a.IVRMIMP_Id == b.IVRMIMP_Id && b.IVRMP_Id == c.IVRMP_Id && c.IVRMP_PageURL == "app.SalesSMSEMAIL" && d.ISMSMSO_Templet==a.ISES_Template_Name
                                     select new SalesSMSEMAILDTO
                                     {
                                         ISES_Id = a.ISES_Id,
                                         ISES_Template_Name = a.ISES_Template_Name,
                                         ISMSMSO_SourceName=d.ISMSMSO_SourceName
                                     }).Distinct().ToArray();

                if (data.catIds==null)
                {
                    data.catIds = "";
                }
                if (data.soursIds == null)
                {
                    data.soursIds = "";
                }
                if (data.prodidss == null)
                {
                    data.prodidss = "";
                }
                if (data.statussidss == null)
                {
                    data.statussidss = "";
                }
                if (data.contryidss == null)
                {
                    data.contryidss = "";
                }
                if (data.statidss == null)
                {
                    data.statidss = "";
                }
                   if (data.searchstring == null)
                {
                    data.searchstring = "";
                }

                if (data.contactname == null)
                {
                    data.contactname = "";
                }
                if (data.mobilesearch == null)
                {
                    data.mobilesearch = "";
                }
                if (data.emailsearch == null)
                {
                    data.emailsearch = "";
                }
                using (var cmd = _VMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_SALES_LEAD_DATA_SMS_EMAIL";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@catIds",
                      SqlDbType.VarChar)
                    {
                        Value = data.catIds
                    });

                    cmd.Parameters.Add(new SqlParameter("@soursIds",
                      SqlDbType.VarChar)
                    {
                        Value = data.soursIds
                    });
                    cmd.Parameters.Add(new SqlParameter("@prodidss",
                      SqlDbType.VarChar)
                    {
                        Value = data.prodidss
                    });
                    cmd.Parameters.Add(new SqlParameter("@statussidss",
                    SqlDbType.VarChar)
                    {
                        Value = data.statussidss
                    });
                    cmd.Parameters.Add(new SqlParameter("@contryidss",
                    SqlDbType.VarChar)
                    {
                        Value = data.contryidss
                    });
                    cmd.Parameters.Add(new SqlParameter("@stateids",
                    SqlDbType.VarChar)
                    {
                        Value = data.statidss
                    });
                    cmd.Parameters.Add(new SqlParameter("@searchstring",
                    SqlDbType.VarChar)
                    {
                        Value = data.searchstring
                    });
                    cmd.Parameters.Add(new SqlParameter("@contactname",
                    SqlDbType.VarChar)
                    {
                        Value = data.contactname
                    });
                    cmd.Parameters.Add(new SqlParameter("@mobilesearch",
                    SqlDbType.VarChar)
                    {
                        Value = data.mobilesearch
                    });
                    cmd.Parameters.Add(new SqlParameter("@emailsearch",
                    SqlDbType.VarChar)
                    {
                        Value = data.emailsearch
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
                        data.leadlist = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
               
          
            return data;
        }
        public SalesSMSEMAILDTO getrpt_lead(SalesSMSEMAILDTO data)
        {
            try
            {
                data.templatelist = (from a in _VMSContext.SMSEmailSetting
                                     from b in _VMSContext.Institution_Module_Page
                                     from c in _VMSContext.MasterPage
                                     from d in _VMSContext.ISM_Sales_Master_Source_DMO
                                     where a.MI_Id == data.MI_Id && a.IVRMIMP_Id == b.IVRMIMP_Id && b.IVRMP_Id == c.IVRMP_Id && c.IVRMP_PageURL == "app.SalesSMSEMAIL" && d.ISMSMSO_Templet == a.ISES_Template_Name
                                     select new SalesSMSEMAILDTO
                                     {
                                         ISES_Id = a.ISES_Id,
                                         ISES_Template_Name = a.ISES_Template_Name,
                                         ISMSMSO_SourceName = d.ISMSMSO_SourceName
                                     }).Distinct().ToArray();

                if (data.catIds == null)
                {
                    data.catIds = "";
                }
                if (data.soursIds == null)
                {
                    data.soursIds = "";
                }
                if (data.prodidss == null)
                {
                    data.prodidss = "";
                }
                if (data.statussidss == null)
                {
                    data.statussidss = "";
                }
                if (data.contryidss == null)
                {
                    data.contryidss = "";
                }
                if (data.statidss == null)
                {
                    data.statidss = "";
                }
                if (data.searchstring == null)
                {
                    data.searchstring = "";
                }

                if (data.contactname == null)
                {
                    data.contactname = "";
                }
                if (data.mobilesearch == null)
                {
                    data.mobilesearch = "";
                }
                if (data.emailsearch == null)
                {
                    data.emailsearch = "";
                }

                var startdate = "";
                var enddate = "";
                if (data.start_Date != null)
                {
                    startdate = data.start_Date.ToString();
                }
                if (data.end_Date != null)
                {
                    enddate = data.end_Date.ToString();
                }
                using (var cmd = _VMSContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_SALES_LEAD_Report_Proc";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@catIds",
                      SqlDbType.VarChar)
                    {
                        Value = data.catIds
                    });

                    cmd.Parameters.Add(new SqlParameter("@soursIds",
                      SqlDbType.VarChar)
                    {
                        Value = data.soursIds
                    });
                    cmd.Parameters.Add(new SqlParameter("@prodidss",
                      SqlDbType.VarChar)
                    {
                        Value = data.prodidss
                    });
                    cmd.Parameters.Add(new SqlParameter("@statussidss",
                    SqlDbType.VarChar)
                    {
                        Value = data.statussidss
                    });
                    cmd.Parameters.Add(new SqlParameter("@contryidss",
                    SqlDbType.VarChar)
                    {
                        Value = data.contryidss
                    });
                    cmd.Parameters.Add(new SqlParameter("@stateids",
                    SqlDbType.VarChar)
                    {
                        Value = data.statidss
                    });
                    cmd.Parameters.Add(new SqlParameter("@searchstring",
                    SqlDbType.VarChar)
                    {
                        Value = data.searchstring
                    });
                    cmd.Parameters.Add(new SqlParameter("@contactname",
                    SqlDbType.VarChar)
                    {
                        Value = data.contactname
                    });
                    cmd.Parameters.Add(new SqlParameter("@mobilesearch",
                    SqlDbType.VarChar)
                    {
                        Value = data.mobilesearch
                    });
                    cmd.Parameters.Add(new SqlParameter("@emailsearch",
                    SqlDbType.VarChar)
                    {
                        Value = data.emailsearch
                    });
                    cmd.Parameters.Add(new SqlParameter("@StartDate",
                   SqlDbType.VarChar)
                    {                       
                        Value = startdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@EndDate",
                   SqlDbType.VarChar)
                    {
                        Value =enddate
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
                        data.leadlist = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }


            return data;
        }


        public SalesSMSEMAILDTO GetAllDropdownAndDatatableDetails(SalesSMSEMAILDTO dto)
        {
            List<HR_Master_PriorityDMO> datalist = new List<HR_Master_PriorityDMO>();
            try
            {
                datalist = _VMSContext.HR_Master_PriorityDMO.Where(t=>t.MI_Id == dto.MI_Id).ToList();
                dto.PriorityList = datalist.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

        public SalesSMSEMAILDTO loadtemplate(SalesSMSEMAILDTO data)
        {
            
            try
            {
                if (data.type== "TPL")
                {
                    data.templatelist = (from a in _VMSContext.SMSEmailSetting
                                         from b in _VMSContext.Institution_Module_Page
                                         from c in _VMSContext.MasterPage
                                         from d in _VMSContext.ISM_Sales_Master_Source_DMO
                                         where a.MI_Id == data.MI_Id && a.IVRMIMP_Id == b.IVRMIMP_Id && b.IVRMP_Id == c.IVRMP_Id && c.IVRMP_PageURL == "app.SalesSMSEMAIL" && d.ISMSMSO_Templet == a.ISES_Template_Name
                                         select new SalesSMSEMAILDTO
                                         {
                                             ISES_Id = a.ISES_Id,
                                             ISES_Template_Name = a.ISES_Template_Name,
                                             ISMSMSO_SourceName = d.ISMSMSO_SourceName
                                         }).Distinct().ToArray();
                }
                else if (data.type == "GREET")
                {
                    using (var cmd = _VMSContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "SalesGreeting_Template";
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
                            data.templatelist = retObject.ToArray();
                        }

                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public SalesSMSEMAILDTO viewtemplatedetails(SalesSMSEMAILDTO data)
        {
            
            try
            {
               
                    data.templatelist = (from a in _VMSContext.SMSEmailSetting
                                         where a.MI_Id == data.MI_Id && a.ISES_Id == data.ISES_Id 
                                         select new SalesSMSEMAILDTO
                                         {
                                             ISES_Id = a.ISES_Id,
                                             ISES_MailHTMLTemplate = a.ISES_MailHTMLTemplate,
                                             ISES_SMSMessage = a.ISES_SMSMessage,
                                         }).Distinct().ToArray();
               
               
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }



        public async Task<string> sendSms(long MI_Id, long mobileNo, string Template, long UserID)
        {

            try
            {

                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = sms;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                    sms = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {


                        cmd.CommandText = "SMSMAILPARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@template",
                           SqlDbType.VarChar)
                        {
                            Value = Template
                        });


                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }

                    }

                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = sms.Replace(ParamaetersName[j].ISMP_NAME, val[ParamaetersName[p].ISMP_NAME]);
                                sms = result;
                            }
                        }
                    }

                    sms = result;
                }


                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;


                 

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                            SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@status",
                   SqlDbType.VarChar)
                        {
                            Value = "Delivered"
                        });

                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                SqlDbType.VarChar)
                        {
                            Value = messageid
                        });


                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }


        public async Task<string> sendSmsWT(long MI_Id, long mobileNo,long UserID,string sms,string name)
        {

            try
            {


                string msg = "Dear " + name +"," + Environment.NewLine; 


                string result = msg+sms;

              

                sms = result;



                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;




                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {


                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                            SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = "Sales"
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@status",
                   SqlDbType.VarChar)
                        {
                            Value = "Delivered"
                        });

                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                SqlDbType.VarChar)
                        {
                            Value = messageid
                        });


                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }
        public string sendmail(long MI_Id, string Email, string Template, long UserID)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string Mailcontent = template.FirstOrDefault().ISES_SMSMessage;
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

                string Resultsms = Mailcontent;
                string result = Mailmsg;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = Mailmsg.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                    Mailmsg = result;
                    Mailcontent = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = "SMSMAILPARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@template",
                           SqlDbType.VarChar)
                        {
                            Value = Template
                        });


                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }

                    }
                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val[val.Keys.ToArray()[p]]);
                                Mailmsg = result;
                            }
                        }
                    }
                    Mailmsg = result;
                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val[val.Keys.ToArray()[p]]);
                                Mailcontent = Resultsms;
                            }
                        }
                    }
                    Mailcontent = Resultsms;
                }

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _db.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {
                        //string mailcc = "";
                        //string mailbcc = "";
                        //if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                        //{
                        //    string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                        //    mailcc = ccmail[0].ToString();

                        //    if (ccmail.Length > 1)
                        //    {
                        //        if (ccmail[1] != null || ccmail[1] != "")
                        //        {
                        //            mailbcc = ccmail[1].ToString();
                        //        }
                        //    }

                        //}
                        //if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        //{
                        //    Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        //}
                        string mailcc = "";
                        string mailcc1 = "";
                        string mailcc2 = "";
                        string mailbcc = "";

                        //   mailcc = "siddesh@vapstech.com";
                        ///   mailcc1 = "pstomd@vapstech.com";
                        ///   mailcc2 = "mktgcoord@vapstech.com";
                        ///    mailbcc = "rakesh.reddy@vapstech.com";
                        var message = new SendGridMessage();
                        try
                        {
                            List<ISM_PlannerReportsDTO> dtonewtemp = new List<ISM_PlannerReportsDTO>();
                            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "FETCH_GETGREETINGCCBCC";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                                SqlDbType.BigInt)
                                {
                                    Value = 1
                                });
                                cmd.Parameters.Add(new SqlParameter("@Template",
                               SqlDbType.Char)
                                {
                                    Value = Template
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
                                            dtonewtemp.Add(new ISM_PlannerReportsDTO
                                            {
                                                MailCc = Convert.ToString(dataReader["cc"]),
                                                MailBCc = Convert.ToString(dataReader["bcc"])
                                            });
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                            if (dtonewtemp.Count > 0)
                            {
                                for (int j = 0; j < dtonewtemp.Count; j++)
                                {
                                    mailcc = dtonewtemp[j].MailCc;
                                    mailbcc = dtonewtemp[j].MailBCc;
                                }
                                if (mailcc != null && mailcc != "")
                                {
                                    string[] ccmaildetails = mailcc.Split(',');

                                    foreach (var c in ccmaildetails)
                                    {
                                        if (c != Email)
                                        {
                                            message.AddCc(c);
                                        }
                                    }
                                }
                                if (mailbcc != null && mailbcc != "")
                                {
                                    string[] bccmaildetails = mailbcc.Split(',');

                                    foreach (var c in bccmaildetails)
                                    {
                                        if (c != Email)
                                        {
                                            message.AddBcc(c);
                                        }
                                    }
                                }
                            }

                        }
                        catch (Exception error)
                        {
                            //
                        }


                        //Sending mail using SendGrid API.
                        //Date:07-02-2017.

                     
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(Email);


                        var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();
                        if (img.Count > 0)
                        {
                            for (int i = 0; i < img.Count; i++)
                            {
                                if (img[i].IVRM_Att_Path != null && img[i].IVRM_Att_Path != "")
                                {
                                    var webClient = new WebClient();
                                    byte[] imageBytes = webClient.DownloadData(img[i].IVRM_Att_Path);
                                    string fileContentsAsBase64 = Convert.ToBase64String(imageBytes);
                                    message.AddAttachment(img[i].IVRM_Att_Name, fileContentsAsBase64, null, null, null);
                                }
                            }
                        }

                        //message.AddCc(mailcc);
                        //message.AddCc(mailcc1);
                        //message.AddCc(mailcc2);
                        //message.AddBcc(mailbcc);

                        message.HtmlContent = Mailmsg;
                        // var client = new Web("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");

                        //if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                        //{
                        //    message.AddAttachment(template.FirstOrDefault().ISES_MailHTMLTemplate);

                        //}
                        var client = new SendGridClient(sengridkey);

                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {
                        string mailcc = "";
                        string mailbcc = "";
                        if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                        {
                            string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                            mailcc = ccmail[0].ToString();

                            if (ccmail.Length > 1)
                            {
                                if (ccmail[1] != null || ccmail[1] != "")
                                {
                                    mailbcc = ccmail[1].ToString();
                                }
                            }

                        }
                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }


                        using (var clientsmtp = new SmtpClient())
                        {
                            var credential = new NetworkCredential
                            {
                                UserName = smstpdetails.FirstOrDefault().IVRMGC_emailUserName,
                                Password = smstpdetails.FirstOrDefault().IVRMGC_emailPassword
                            };

                            clientsmtp.Credentials = credential;
                            clientsmtp.Host = smstpdetails.FirstOrDefault().IVRMGC_HostName;
                            clientsmtp.Port = smstpdetails.FirstOrDefault().IVRMGC_PortNo;
                            clientsmtp.EnableSsl = true;

                            using (var emailMessage = new MailMessage())
                            {


                                emailMessage.To.Add(new MailAddress(Email));
                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;


                                if (Attechement.Equals("1"))
                                {
                                    var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                                    if (img.Count > 0)
                                    {
                                        for (int i = 0; i < img.Count; i++)
                                        {

                                            foreach (var attache in img.ToList())
                                            {

                                                //emailMessage.Attachments.Add(new System.Net.Mail.Attachment("https://bdcampusstrg.blob.core.windows.net/files/4/Prospects Ver 03.pdf"));

                                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(attache.IVRM_Att_Path) as HttpWebRequest;
                                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                                Stream stream = response.GetResponseStream();
                                                emailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, attache.IVRM_Att_Name));
                                            }


                                            //var attachment = new MimePart("image", "gif")
                                            //{
                                            //    ContentObject = new ContentObject(File.OpenRead(img[i].IVRM_Att_Path), ContentEncoding.Default),
                                            //    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                                            //    ContentTransferEncoding = ContentEncoding.Base64,
                                            //    FileName = Path.GetFileName(img[i].IVRM_Att_Path)
                                            //};
                                        }
                                    }
                                }


                                if (mailcc != null && mailcc != "")
                                {
                                    emailMessage.CC.Add(mailcc);
                                }
                                if (mailbcc != null && mailbcc != "")
                                {
                                    emailMessage.Bcc.Add(mailbcc);
                                }
                                clientsmtp.Send(emailMessage);
                            }
                        }

                    }


                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = Email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailcontent
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = "PRE-SALES"
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }




        public string sendmailWT(long MI_Id, string Email, string sms, long UserID,string sub,string footer, NAACCriteriaFivefileDTO[] files,string cname,string lname,bool aflag,string fhead)
        {
            try
            {
                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                string Mailcontent = sms;
                string Mailmsg = sms;
                Mailcontent = "Dear " + cname + "," + "<br />";
                Mailmsg = "Dear " + cname + "," + "<br />";
                // string Resultsms = Mailcontent;
                // string result = Mailmsg;
                Mailcontent = Mailcontent + sms;
                Mailmsg = Mailmsg + sms;
                if (fhead !=null && fhead !="")
                {
                    Mailcontent = Mailcontent + "<br />" + fhead + "<br />";
                    Mailmsg = Mailmsg + "<br />" + fhead + "<br />";
                }
                if (footer!="" && footer !=null)
                {
                    Mailcontent = Mailcontent + "<br />" + footer;
                    Mailmsg = Mailmsg + "<br />" + footer;
                }
                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = sub;
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    List<GeneralConfigDMO> smstpdetails = new List<GeneralConfigDMO>();
                    smstpdetails = _db.GenConfig.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                    if (smstpdetails.FirstOrDefault().IVRMGC_APIOrSMTPFlg == "API")
                    {
                        //string mailcc = "";
                        //string mailbcc = "";
                        //if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                        //{
                        //    string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                        //    mailcc = ccmail[0].ToString();

                        //    if (ccmail.Length > 1)
                        //    {
                        //        if (ccmail[1] != null || ccmail[1] != "")
                        //        {
                        //            mailbcc = ccmail[1].ToString();
                        //        }
                        //    }

                        //}
                        //if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        //{
                        //    Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        //}
                        string mailcc = "";
                        string mailcc1 = "";
                        string mailcc2 = "";
                        string mailbcc = "";

                        //mailcc = "siddesh@vapstech.com";
                        //mailcc1 = "pstomd@vapstech.com";
                        //mailcc2 = "mktgcoord@vapstech.com";
                        //mailbcc = "rakesh.reddy@vapstech.com";

                        //Sending mail using SendGrid API.
                        //Date:07-02-2017.

                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(Email);
                        //string path = "https://bdcampusstrg.blob.core.windows.net/files/4/Prospects Ver 03.pdf";
                        //string path1 = "https://bdcampusstrg.blob.core.windows.net/files/4/Prospects Ver 03.pdf";
                        //message.AddAttachment(path,"path",null,null,null);
                        //message.AddAttachment(path1,"path",null,null,null);

                        if (aflag==true)
                        {
                            if (files.Length>0)
                            {
                                foreach (var item in files)
                                {
                                    if (item.cfilepath != null && item.cfilepath != "")
                                    {
                                        var webClient = new WebClient();
                                        byte[] imageBytes = webClient.DownloadData(item.cfilepath);
                                        // byte[] bytes = File.ReadAllBytes(a);
                                        string fileContentsAsBase64 = Convert.ToBase64String(imageBytes);
                                        //message.AddAttachment(item.cfilepath, "", null, null, null);
                                        message.AddAttachment(item.cfilepath, fileContentsAsBase64, null, null, null);
                                    }
                                }
                            }
                        }
                        //if (Attechement.Equals("1"))
                        //{
                        //    //var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                        //    //if (img.Count > 0)
                        //    //{
                        //    //for (int i = 0; i < img.Count; i++)
                        //    //{

                        //    //emailMessage.Attachments.Add(new System.Net.Mail.Attachment("https://bdcampusstrg.blob.core.windows.net/files/4/Prospects Ver 03.pdf"));
                        //    //string path = "https://bdcampusstrg.blob.core.windows.net/files/4/Prospects Ver 03.pdf";
                        //    //string name1 = "Ver 03.pdf";

                        //    //System.Net.HttpWebRequest request = System.Net.WebRequest.Create(path) as HttpWebRequest;
                        //    //        System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                        //    //        Stream stream = response.GetResponseStream();
                        //    //        message.AddAttachment(stream.ToString(), name1);
                        //       // }
                        //    //}
                        //}

                        //if (mailcc != null && mailcc != "")
                        //{
                        //    message.AddCc(mailcc);
                        //}
                        //if (mailbcc != null && mailbcc != "")
                        //{
                        //    message.AddBcc(mailbcc);
                        //}
                        try
                        {
                            List<ISM_PlannerReportsDTO> dtonewtemp = new List<ISM_PlannerReportsDTO>();
                            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "FETCH_GETGREETINGCCBCC";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                                SqlDbType.BigInt)
                                {
                                    Value = 1
                                });
                                cmd.Parameters.Add(new SqlParameter("@Template",
                               SqlDbType.Char)
                                {
                                    Value = "SALESWITHOUTTEMPL"
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
                                            dtonewtemp.Add(new ISM_PlannerReportsDTO
                                            {
                                                MailCc = Convert.ToString(dataReader["cc"]),
                                                MailBCc = Convert.ToString(dataReader["bcc"])
                                            });
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                            if (dtonewtemp.Count > 0)
                            {
                                for (int j = 0; j < dtonewtemp.Count; j++)
                                {
                                    mailcc = dtonewtemp[j].MailCc;
                                    mailbcc = dtonewtemp[j].MailBCc;
                                }
                                if (mailcc != null && mailcc != "")
                                {
                                    string[] ccmaildetails = mailcc.Split(',');

                                    foreach (var c in ccmaildetails)
                                    {
                                        if (c != Email)
                                        {
                                            message.AddCc(c);
                                        }
                                    }
                                }
                                if (mailbcc != null && mailbcc != "")
                                {
                                    string[] bccmaildetails = mailbcc.Split(',');

                                    foreach (var c in bccmaildetails)
                                    {
                                        if (c != Email)
                                        {
                                            message.AddBcc(c);
                                        }
                                    }
                                }
                            }

                        }
                        catch (Exception error)
                        {
                            //
                        }


                        message.HtmlContent = Mailmsg;
                        
                        var client = new SendGridClient(sengridkey);

                        client.SendEmailAsync(message).Wait();
                    }
                    else
                    {
                        string mailcc = "";
                        string mailbcc = "";
                        if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                        {
                            string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                            mailcc = ccmail[0].ToString();

                            if (ccmail.Length > 1)
                            {
                                if (ccmail[1] != null || ccmail[1] != "")
                                {
                                    mailbcc = ccmail[1].ToString();
                                }
                            }

                        }
                        if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                        {
                            Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                        }
                        
                        using (var clientsmtp = new SmtpClient())
                        {
                            var credential = new NetworkCredential
                            {
                                UserName = smstpdetails.FirstOrDefault().IVRMGC_emailUserName,
                                Password = smstpdetails.FirstOrDefault().IVRMGC_emailPassword
                            };
                            clientsmtp.Credentials = credential;
                            clientsmtp.Host = smstpdetails.FirstOrDefault().IVRMGC_HostName;
                            clientsmtp.Port = smstpdetails.FirstOrDefault().IVRMGC_PortNo;
                            clientsmtp.EnableSsl = true;

                            using (var emailMessage = new MailMessage())
                            {
                                emailMessage.To.Add(new MailAddress(Email));
                                emailMessage.From = new MailAddress(smstpdetails.FirstOrDefault().IVRMGC_emailUserName);
                                emailMessage.Subject = Subject;
                                emailMessage.Body = Mailmsg;
                                emailMessage.IsBodyHtml = true;
                                //Attechement = "1";

                                //if (Attechement.Equals("1"))
                                //{
                                //    string path = "https://bdcampusstrg.blob.core.windows.net/files/4/Prospects Ver 03.pdf";
                                //    string name1 = "Ver 03.pdf";

                                //    emailMessage.Attachments.Add(new System.Net.Mail.Attachment("https://bdcampusstrg.blob.core.windows.net/files/4/Prospects Ver 03.pdf"));


                                //    //var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                                //    //if (img.Count > 0)
                                //    //{
                                //    //for (int i = 0; i < img.Count; i++)
                                //    //{

                                //    //  foreach (var attache in img.ToList())
                                //    //  {



                                //    //System.Net.HttpWebRequest request = System.Net.WebRequest.Create(path) as HttpWebRequest;
                                //    //System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                //    //Stream stream = response.GetResponseStream();
                                //    //emailMessage.Attachments.Add(new System.Net.Mail.Attachment(stream, name1));
                                //    //}


                                //    //}
                                //    // }
                                //}


                                if (mailcc != null && mailcc != "")
                                {
                                    emailMessage.CC.Add(mailcc);
                                }
                                if (mailbcc != null && mailbcc != "")
                                {
                                    emailMessage.Bcc.Add(mailbcc);
                                }
                                clientsmtp.Send(emailMessage);
                            }
                        }

                    }


                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                       

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = Email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailcontent
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = "PRE-SALES"
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }




        public string PortalEmailWthtTmp(long mi_id, string name, string emailid, string msg)
        {

            try
            {
                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID == mi_id).ToList();
                var institutionName = _db.Institution.Where(m => m.MI_Id == mi_id).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    string Subject = "Email From " + institutionName.FirstOrDefault().MI_Name;

                    //Sending mail using SendGrid API.
                    //Date:07-02-2017.

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
                    message.AddTo(emailid);
                    string body = "<br />" + "<div>" + msg + "</div>";
                    string footer = "<br />" + " Thanks and Regards" + "<br />" + "<div>" + institutionName.FirstOrDefault().MI_Name + "</div>";
                    message.HtmlContent = "Dear " + name + body + footer;
                    var client = new SendGridClient(sengridkey);
                    client.SendEmailAsync(message).Wait();

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = emailid
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = msg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = "PRINCIPAL DASHBOARD"
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = mi_id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return e.Message;

            }
            return "Success";
        }

    }
}
