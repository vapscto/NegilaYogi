using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using WebApplication1.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Model;
using AutoMapper;
using System.Collections.Concurrent;
using CommonLibrary;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Dynamic;
using Newtonsoft.Json;

namespace WebApplication1.Services
{
    public class StatusImpl : Interfaces.StatusInterface
    {
        private static ConcurrentDictionary<string, StudentApplicationDTO> _login =
              new ConcurrentDictionary<string, StudentApplicationDTO>();
        public DomainModelMsSqlServerContext _db;
        public StatusImpl(DomainModelMsSqlServerContext db)
        {
            _db = db;
        }
        // get initial dropdown data
        public async Task<CommonDTO> getInitailData(int mi_id)
        {
            CommonDTO ctdo = new CommonDTO();
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = (from a in _db.AcademicYear
                              where (a.MI_Id == mi_id && a.ASMAY_Pre_ActiveFlag == 1 && a.Is_Active == true && a.MI_Id == mi_id)
                              select new MasterAcademic
                              {
                                  ASMAY_Id = a.ASMAY_Id,
                                  ASMAY_Year = a.ASMAY_Year
                              }
                      ).ToList();
                ctdo.AcademicList = allyear.ToArray();
                List<School_M_Class> allclass = new List<School_M_Class>();
                allclass = _db.School_M_Class.Where(t => t.MI_Id == mi_id && t.ASMCL_ActiveFlag == true).ToList();
                ctdo.classlist = allclass.ToArray();

                List<AdmissionStatus> status = new List<AdmissionStatus>();
                status = _db.status.Where(t => t.MI_Id == mi_id).ToList();
                ctdo.statuslist = status.ToArray();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return ctdo;
        }
        public CommonDTO getdataonsearchfilter(CommonDTO cdto)
        {
            List<StudentApplicationDTO> stulist = new List<StudentApplicationDTO>();
            try
            {
                List<StudentApplicationDTO> result = new List<StudentApplicationDTO>();
                //to get data according to search criteria.
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Get_Student_Status";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Ids", SqlDbType.Int) { Value = cdto.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Ids", SqlDbType.Int) { Value = cdto.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@PAMST_Ids", SqlDbType.Int) { Value = cdto.PAMST_Id });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.Int) { Value = cdto.IVRM_MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@type_", SqlDbType.VarChar) { Value = cdto.status_type });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new StudentApplicationDTO
                                {
                                    pasR_Id = Convert.ToInt64(dataReader["PASR_Id"]),
                                    PAMST_Id = Convert.ToInt64(dataReader["PAMS_Id"]),
                                    remark = (dataReader["Remark"]).ToString(),
                                    PASR_FirstName = (dataReader["PASR_FirstName"]).ToString(),
                                    PASR_MiddleName= (Convert.IsDBNull(dataReader["PASR_MiddleName"])).ToString(),
                                    PASR_LastName = Convert.ToString(dataReader["PASR_LastName"]),
                                    ASMCL_Id = Convert.ToInt64(dataReader["ASMCL_Id"]),
                                    className = dataReader["ASMCL_ClassName"].ToString(),
                                    statusName = dataReader["PAMST_Status"].ToString(),
                                    statusFlag = dataReader["PAMST_StatusFlag"].ToString(),
                                    PASR_Sex = dataReader["PASR_Sex"].ToString(),
                                    PASR_RegistrationNo = dataReader["PASR_RegistrationNo"].ToString(),
                                    PASR_emailId =dataReader["PASR_emailId"].ToString(),
                                    PASR_MobileNo = Convert.ToInt64(dataReader["PASR_MobileNo"]),
                                    PASR_FatherName= Convert.IsDBNull(dataReader["PASR_FatherName"]).ToString()+' '+ Convert.IsDBNull(dataReader["PASR_FatherSurname"]).ToString(),
                                    PASR_DOBWords =dataReader["PASR_DOB"].ToString(),
                                    PASR_ConCity = Convert.IsDBNull(dataReader["PASR_ConCity"]).ToString(),
                                    PASR_ConStreet = Convert.IsDBNull(dataReader["PASR_ConStreet"]).ToString(),
                                    PASR_ConArea = Convert.IsDBNull(dataReader["PASR_ConArea"]).ToString(),
                                    PASR_ConPincode = Convert.ToInt32(Convert.IsDBNull(dataReader["PASR_ConPincode"])),
                                    Repeat_Class_Id = Convert.ToInt64(dataReader["Repeat_Class_Id"])
                                });
                                cdto.studentlist = result.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return cdto;
        }
        public CommonDTO saveData(CommonDTO cdto)
        {
            try
            {
                Dictionary<string, string> smsemail = new Dictionary<string, string>();
                smsemail.Add("header", cdto.header);
                smsemail.Add("Subject", cdto.Subject);
                smsemail.Add("Message", cdto.Message);
                smsemail.Add("Footer", cdto.Footer);
                cdto.smsemailarry = smsemail.ToArray();
                long trnsno = 0;
                long senderid = cdto.userId;
                // SMS NEW TABLES CODE START
                //if (cdto.data_array.Count()>0)
                //{
                //SMS smstransno = new SMS(_db);
                ////  public async Task<string> sendSmsnew(long MI_Id, long mobileNo, string Template, long UserID, string sms)
                // trnsno= smstransno.getsmsno(cdto.mi_id);
                //}
                //string studentempflag = "STUDENT";
                // SMS NEW TABLES CODE END
                if (cdto.data_array.Count() > 0)
                {
                    if (cdto._type == "Appsts")
                    {
                        for (int i = 0; i < cdto.data_array.Count(); i++)
                        {
                            try
                            {
                                var changedStudentData = _db.StudentApplication.Single(d => d.pasr_id == cdto.data_array[i].pasR_Id);
                                if (cdto.data_array[i].remarks != null)
                                {
                                    if (cdto.data_array[i].remarks.ToString() != "")
                                    {
                                        changedStudentData.Remark = cdto.data_array[i].remarks.ToString();
                                    }
                                }



                                changedStudentData.PASRAPS_ID = cdto.data_array[i].PAMST_Id;
                                _db.StudentApplication.Update(changedStudentData);
                                int cnt = _db.SaveChanges();
                                if (cnt == 1)
                                {
                                    if(cdto.defaultsmsemail==false)
                                    {
                                        if (cdto.emailcheck == true)
                                        {
                                            try
                                            {
                                                Email Email = new Email(_db);
                                                Email.sendmailschedule(changedStudentData.MI_Id, "STATUS", smsemail, changedStudentData.PASR_emailId, "Preadmission App Status");
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                        }

                                        if (cdto.smscheck == true)
                                        {

                                            try
                                            {

                                                Dictionary<string, string> val = new Dictionary<string, string>();

                                                var institutionName = _db.Institution.Where(g => g.MI_Id == changedStudentData.MI_Id).ToList();


                                                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                                                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(changedStudentData.MI_Id)).ToList();

                                                List<Institution> insdeta = new List<Institution>();
                                                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(changedStudentData.MI_Id)).ToList();

                                                var template = _db.smsEmailSetting.Where(e => e.MI_Id == changedStudentData.MI_Id && e.ISES_SMSActiveFlag == true).ToList();

                                                if (alldetails.Count > 0)
                                                {
                                                    string url = alldetails[0].IVRMSD_URL.ToString();

                                                    string PHNO = changedStudentData.PASR_MobileNo.ToString();

                                                    url = url.Replace("PHNO", PHNO);

                                                    url = url.Replace("MESSAGE", cdto.smscontent);
                                                    url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());

                                                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);


                                                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                                                    System.Net.HttpWebResponse response = request.GetResponse() as System.Net.HttpWebResponse;
                                                    Stream stream = response.GetResponseStream();

                                                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                                                    string responseparameters = readStream.ReadToEnd();

                                                    //List<SMSParameters> list = JsonConvert.DeserializeObject<List<SMSParameters>>(responseparameters);

                                                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                                    {

                                                        // var modulename = "InstitutionCreation";
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
                                                            Value = cdto.smscontent
                                                        });
                                                        cmd.Parameters.Add(new SqlParameter("@module",
                                                        SqlDbType.VarChar)
                                                        {
                                                            Value = "Preadmission Status"
                                                        });
                                                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                                       SqlDbType.BigInt)
                                                        {
                                                            Value = changedStudentData.MI_Id
                                                        });

                                                        cmd.Parameters.Add(new SqlParameter("@status",
                                                   SqlDbType.VarChar)
                                                        {
                                                            Value = responseparameters
                                                        });

                                                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                                                SqlDbType.VarChar)
                                                        {
                                                            Value = 0
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
                                                            //return ex.Message;
                                                        }
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                                //return ex.Message;
                                            }
                                            //}
                                            //catch (Exception ex)
                                            //{

                                            //}
                                        }
                                    }
                                    else
                                    {
                                        Email Email = new Email(_db);
                                        string m = Email.sendmail(changedStudentData.MI_Id, changedStudentData.PASR_emailId, "STATUS_CONFIRM", changedStudentData.pasr_id);
                                        
                                        try
                                        {

                                            Dictionary<string, string> val = new Dictionary<string, string>();

                                            var template = _db.smsEmailSetting.Where(e => e.MI_Id == changedStudentData.MI_Id && e.ISES_Template_Name == "STATUS_CONFIRM" && e.ISES_SMSActiveFlag == true).ToList();


                                            var institutionName = _db.Institution.Where(j => j.MI_Id == changedStudentData.MI_Id).ToList();

                                            var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(j => j.MI_Id == changedStudentData.MI_Id && j.ISES_Id == template[0].ISES_Id && j.Flag == "S").Select(d => d.ISMP_ID).ToList();

                                            var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(j => Paramaeters.Contains(j.ISMP_ID)).ToList();

                                            string sms = template.FirstOrDefault().ISES_SMSMessage;

                                            string result = sms;

                                            List<Match> variables = new List<Match>();

                                            foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                                            {
                                                variables.Add(match);
                                            }
                                                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                                {


                                                    cmd.CommandText = "SMSMAILPARAMETER";
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.Add(new SqlParameter("@UserID",
                                                        SqlDbType.BigInt)
                                                    {
                                                        Value = changedStudentData.pasr_id
                                                    });
                                                    cmd.Parameters.Add(new SqlParameter("@template",
                                                       SqlDbType.VarChar)
                                                    {
                                                        Value = "STATUS_CONFIRM"
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
                                                       
                                                    }

                                                }

                                                for (int j = 0; j < ParamaetersName.Count; j++)
                                                {
                                                    for (int p = 0; p < val.Count; p++)
                                                    {
                                                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                                                        {
                                                            result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                                            sms = result;
                                                        }
                                                    }
                                                }

                                                sms = result;
                                            


                                            List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                                            alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(changedStudentData.MI_Id)).ToList();

                                            List<Institution> insdeta = new List<Institution>();
                                            insdeta = _db.Institution.Where(t => t.MI_Id.Equals(changedStudentData.MI_Id)).ToList();                                          

                                            if (alldetails.Count > 0)
                                            {
                                                string url = alldetails[0].IVRMSD_URL.ToString();

                                                string PHNO = changedStudentData.PASR_MobileNo.ToString();

                                                url = url.Replace("PHNO", PHNO);

                                                url = url.Replace("MESSAGE", sms);
                                                url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());
                                                url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);

                                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                                                System.Net.HttpWebResponse response =  request.GetResponse() as System.Net.HttpWebResponse;
                                                Stream stream = response.GetResponseStream();

                                                StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                                                string responseparameters = readStream.ReadToEnd();
                                                var myContent = JsonConvert.SerializeObject(responseparameters);

                                                dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                                                string messageid = responsedata;

                                               
                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                            
                                        }
                                    }
                              


                                }
                                else
                                {
                                    cdto.count = 0;
                                }
                                //  save student status change to history table
                                StudentStatusHistory ssh = new StudentStatusHistory();
                                ssh.PASR_Id = changedStudentData.pasr_id;
                                ssh.PASSH_Status = Convert.ToString(changedStudentData.PAMS_Id);
                                ssh.PASSH_Date = DateTime.Now;


                                //added by 02/02/2017
                                ssh.CreatedDate = DateTime.Now;
                                ssh.UpdatedDate = DateTime.Now;
                                _db.studentstatushistory.Add(ssh);
                                _db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < cdto.data_array.Count(); i++)
                        {
                            try
                            {
                                string _status_id = cdto.data_array[i].PAMST_Id.ToString();
                                if (cdto.data_array[i].remarks != null)
                                {
                                    string _remarks = cdto.data_array[i].remarks.ToString();
                                }
                                var changedStudentData = _db.StudentApplication.Single(d => d.pasr_id == cdto.data_array[i].pasR_Id);
                                if (cdto.data_array[i].remarks != null)
                                {
                                    if (cdto.data_array[i].remarks.ToString() != "")
                                    {
                                        changedStudentData.Remark = cdto.data_array[i].remarks.ToString();
                                    }
                                }
                                changedStudentData.PAMS_Id = cdto.data_array[i].PAMST_Id;
                                _db.StudentApplication.Update(changedStudentData);
                                int cnt = _db.SaveChanges();
                                if (cnt == 1)
                                {                                    
                                    cdto.count = 1;
                                    if (cdto.defaultsmsemail == false)
                                    {
                                        if (cdto.emailcheck == true)
                                        {
                                            Email Email = new Email(_db);
                                            Email.sendmailschedule(changedStudentData.MI_Id, "DEFAULT", smsemail, changedStudentData.PASR_emailId, "Preadmission Admission Status");
                                        }
                                        if (cdto.smscheck == true)
                                        {
                                            try
                                            {
                                                Dictionary<string, string> val = new Dictionary<string, string>();
                                                var institutionName = _db.Institution.Where(g => g.MI_Id == changedStudentData.MI_Id).ToList();
                                                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                                                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(changedStudentData.MI_Id)).ToList();

                                                List<Institution> insdeta = new List<Institution>();
                                                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(changedStudentData.MI_Id)).ToList();

                                                var template = _db.smsEmailSetting.Where(e => e.MI_Id == changedStudentData.MI_Id && e.ISES_Template_Name == "DEFAULT" && e.ISES_SMSActiveFlag == true).ToList();
                                                if (alldetails.Count > 0)
                                                {
                                                    string url = alldetails[0].IVRMSD_URL.ToString();
                                                    string PHNO = changedStudentData.PASR_MobileNo.ToString();
                                                    url = url.Replace("PHNO", PHNO);
                                                    url = url.Replace("MESSAGE", cdto.smscontent);
                                                    url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());
                                                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);

                                                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                                                    System.Net.HttpWebResponse response = request.GetResponse() as System.Net.HttpWebResponse;
                                                    Stream stream = response.GetResponseStream();
                                                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                                                    string responseparameters = readStream.ReadToEnd();
                                                    //List<SMSParameters> list = JsonConvert.DeserializeObject<List<SMSParameters>>(responseparameters);
                                                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                                    {
                                                        // var modulename = "InstitutionCreation";
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
                                                            Value = cdto.smscontent
                                                        });
                                                        cmd.Parameters.Add(new SqlParameter("@module",
                                                        SqlDbType.VarChar)
                                                        {
                                                            Value = "Preadmission Status"
                                                        });
                                                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                                       SqlDbType.BigInt)
                                                        {
                                                            Value = changedStudentData.MI_Id
                                                        });

                                                        cmd.Parameters.Add(new SqlParameter("@status",
                                                   SqlDbType.VarChar)
                                                        {
                                                            Value = responseparameters
                                                        });

                                                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                                                SqlDbType.VarChar)
                                                        {
                                                            Value = 0
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
                                                            //return ex.Message;
                                                        }
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                                //return ex.Message;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var statusflag = _db.status.Single(d => d.PAMST_Id == cdto.data_array[i].PAMST_Id).PAMST_StatusFlag;
                                        if(statusflag=="CNF")
                                        {
                                            Email Email = new Email(_db);
                                            string m = Email.sendmail(changedStudentData.MI_Id, changedStudentData.PASR_emailId, "STATUS_CONFIRM", changedStudentData.pasr_id);
                                            try
                                            {
                                                Dictionary<string, string> val = new Dictionary<string, string>();
                                                var template = _db.smsEmailSetting.Where(e => e.MI_Id == changedStudentData.MI_Id && e.ISES_Template_Name == "STATUS_CONFIRM" && e.ISES_SMSActiveFlag == true).ToList();
                                                var institutionName = _db.Institution.Where(j => j.MI_Id == changedStudentData.MI_Id).ToList();
                                                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(j => j.MI_Id == changedStudentData.MI_Id && j.ISES_Id == template[0].ISES_Id && j.Flag == "S").Select(d => d.ISMP_ID).ToList();
                                                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(j => Paramaeters.Contains(j.ISMP_ID)).ToList();
                                                string sms = template.FirstOrDefault().ISES_SMSMessage;
                                                string result = sms;
                                                List<Match> variables = new List<Match>();
                                                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                                                {
                                                    variables.Add(match);
                                                }
                                                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                                {
                                                    cmd.CommandText = "SMSMAILPARAMETER";
                                                    cmd.CommandType = CommandType.StoredProcedure;
                                                    cmd.Parameters.Add(new SqlParameter("@UserID",
                                                        SqlDbType.BigInt)
                                                    {
                                                        Value = changedStudentData.pasr_id
                                                    });
                                                    cmd.Parameters.Add(new SqlParameter("@template",
                                                       SqlDbType.VarChar)
                                                    {
                                                        Value = "STATUS_CONFIRM"
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

                                                    }
                                                }
                                                for (int j = 0; j < ParamaetersName.Count; j++)
                                                {
                                                    for (int p = 0; p < val.Count; p++)
                                                    {
                                                        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                                                        {
                                                            result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                                            sms = result;
                                                        }
                                                    }
                                                }
                                                sms = result;
                                                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                                                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(changedStudentData.MI_Id)).ToList();

                                                List<Institution> insdeta = new List<Institution>();
                                                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(changedStudentData.MI_Id)).ToList();                                              
                                                if (alldetails.Count > 0)
                                                {
                                                    string url = alldetails[0].IVRMSD_URL.ToString();
                                                    string PHNO = changedStudentData.PASR_MobileNo.ToString();
                                                    url = url.Replace("PHNO", PHNO);
                                                    url = url.Replace("MESSAGE", sms);
                                                    url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());
                                                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);
                                                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                                                    System.Net.HttpWebResponse response = request.GetResponse() as System.Net.HttpWebResponse;
                                                    Stream stream = response.GetResponseStream();
                                                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                                                    string responseparameters = readStream.ReadToEnd();
                                                    var myContent = JsonConvert.SerializeObject(responseparameters);
                                                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                                                    string messageid = responsedata;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);
                                            }
                                        }
                                    }                                       
                                }
                                else
                                {
                                    cdto.count = 0;
                                }
                                //  save student status change to history table
                                StudentStatusHistory ssh = new StudentStatusHistory();
                                ssh.PASR_Id = changedStudentData.pasr_id;
                                ssh.PASSH_Status = Convert.ToString(changedStudentData.PAMS_Id);
                                ssh.PASSH_Date = DateTime.Now;
                                //added by 02/02/2017
                                ssh.CreatedDate = DateTime.Now;
                                ssh.UpdatedDate = DateTime.Now;
                                _db.studentstatushistory.Add(ssh);
                                _db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return cdto;
        }
    }
}
