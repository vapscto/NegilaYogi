using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.VisitorsManagement;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Birthday;
using DomainModel.Model.com.vapstech.VisitorsManagement;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
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
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VisitorsManagementServiceHub.Services
{
    public class LateInStudentImpl:Interfaces.LateInStudentInterface
    {

        public VisitorsManagementContext visctxt;
        public DomainModelMsSqlServerContext _db;
        public LateInStudentImpl(VisitorsManagementContext context, DomainModelMsSqlServerContext _db123)
        {
            visctxt = context;
            _db = _db123;
        }

       
        public async Task<LateInStudent_DTO> loaddata(LateInStudent_DTO data)
        {
            try
            {

                data.academicYear = _db.AcademicYear.Where(d => d.MI_Id == data.MI_Id && d.ASMAY_ActiveFlag == 1).Distinct().ToArray();

                using (var cmd = visctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Get_Student_LateInd_Record";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                  

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        data.getstudentlist = retObject.ToArray();

                    }


                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public LateInStudent_DTO get_class(LateInStudent_DTO data)
        {
            try
            {
                data.classList = (from a in _db.SchoolYearWiseStudent
                                  from b in _db.School_M_Class
                                 where (a.ASMAY_Id == data.ASMAY_Id && a.AMAY_ActiveFlag == 1 && a.ASMCL_Id == b.ASMCL_Id && b.ASMCL_ActiveFlag == true && b.MI_Id == data.MI_Id)
                                 select b).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public LateInStudent_DTO get_section(LateInStudent_DTO data)
        {
            try
            {
                data.sectionList = (from a in _db.SchoolYearWiseStudent
                                   from b in _db.School_M_Section
                                   where (a.ASMAY_Id == data.ASMAY_Id && a.AMAY_ActiveFlag == 1 && a.ASMS_Id == b.ASMS_Id && b.ASMC_ActiveFlag == 1 && b.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id)
                                   select b).Distinct().OrderBy(t => t.ASMS_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public LateInStudent_DTO get_student(LateInStudent_DTO data)
        {
            try
            {
                data.studentlist = (from a in _db.SchoolYearWiseStudent
                                    from b in _db.School_M_Section
                                    from c in _db.School_M_Class
                                    from d in _db.Adm_M_Student
                                    where (a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == b.ASMS_Id && a.AMST_Id == d.AMST_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && d.AMST_SOL == "S" && a.AMAY_ActiveFlag == 1 && d.AMST_ActiveFlag == 1)
                                    select new LateInStudent_DTO {
                                        AMST_Id = d.AMST_Id,
                                        studentName = d.AMST_FirstName + (string.IsNullOrEmpty(d.AMST_MiddleName) ? "" : ' ' + d.AMST_MiddleName) + (string.IsNullOrEmpty(d.AMST_LastName) ? "" : ' ' + d.AMST_LastName),
                                    }).Distinct().OrderBy(t => t.ASMS_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<LateInStudent_DTO> savedata(LateInStudent_DTO data)
        {
            try
            {
                //var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
                var remoteIpAddress = "";
                //string netip = remoteIpAddress.ToString();

                string strHostName = "";
                strHostName = System.Net.Dns.GetHostName();
                IPHostEntry ipEntry =await System.Net.Dns.GetHostEntryAsync(strHostName);
                IPAddress[] addr = ipEntry.AddressList;
                remoteIpAddress = addr[addr.Length - 1].ToString();

                string hostName = Dns.GetHostName();
                var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                string myIP1 = addr[addr.Length - 2].ToString();

                if (data.ALIEOS_Id==0)
                {
                    if (data.studetdatalist.Length>0)
                    {
                       foreach(var amst in data.studetdatalist)
                        {
                            LateInStudent_DMO obj = new LateInStudent_DMO();

                            obj.ALIEOS_Id = data.ALIEOS_Id;
                            obj.MI_Id = data.MI_Id;
                            obj.ASMAY_Id = data.ASMAY_Id;
                            obj.ASMCL_Id = data.ASMCL_Id;
                            obj.ASMS_Id = data.ASMS_Id;
                            obj.AMST_Id = amst.AMST_Id;                            
                            obj.ALIEOS_AttendanceDate = data.ALIEOS_AttendanceDate;
                            obj.ALIEOS_PunchDate = data.ALIEOS_PunchDate;
                            obj.ALIEOS_PunchTime = data.ALIEOS_PunchTime;
                            obj.ALIEOS_Reason = data.ALIEOS_Reason;
                            obj.ALIEOS_SystemIP = remoteIpAddress;
                            obj.ALIEOS_NetworkIP = myIP1; /*data.ALIEOS_NetworkIP;*/
                            obj.ALIEOS_AactiveFlag = true;

                            visctxt.Add(obj);
                           
                        }
                        int a = visctxt.SaveChanges();
                        if (a > 0)
                        {
                            ////data.returnval = true;

                            for (int i = 0; i < data.studetdatalist.Length; i++)
                            {
                                var temp_usr = data.studetdatalist[i].AMST_Id;

                                var contactno1 = visctxt.Adm_M_Student.Single(t => t.AMST_Id == temp_usr).AMST_FatherMobleNo;
                               // var contactno = contactno1.Value;
                                string contactno = contactno1.Value.ToString();

                                var mailId = visctxt.Adm_M_Student.Single(t => t.AMST_Id == temp_usr).AMST_FatheremailId;
                                var fname = visctxt.Adm_M_Student.Single(t => t.AMST_Id == temp_usr).AMST_FirstName;
                                var mname = visctxt.Adm_M_Student.Single(t => t.AMST_Id == temp_usr).AMST_MiddleName;
                                var lname = visctxt.Adm_M_Student.Single(t => t.AMST_Id == temp_usr).AMST_LastName;
                                var admno = visctxt.Adm_M_Student.Single(t => t.AMST_Id == temp_usr).AMST_FatheremailId;
                                string Template = "StudentLateIn";

                                string x = await sendSms(data.MI_Id, contactno, Template, temp_usr);


                                string y = sendmail(data.MI_Id, mailId, Template, temp_usr);
                            }

                            data.returnval = true;

                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                    
                }
                else if (data.ALIEOS_Id > 0)
                {
                   
                        foreach (var item in data.studetdatalist)
                        {
                            var result = visctxt.LateInStudent_DMO.Where(t => t.MI_Id == data.MI_Id && t.ALIEOS_Id == data.ALIEOS_Id).Single();

                            result.ASMAY_Id = data.ASMAY_Id;
                            result.ASMCL_Id = data.ASMCL_Id;
                            result.ASMS_Id = data.ASMS_Id;
                            result.AMST_Id = item.AMST_Id;
                            result.ALIEOS_AttendanceDate = data.ALIEOS_AttendanceDate;
                            result.ALIEOS_PunchDate = data.ALIEOS_PunchDate;
                            result.ALIEOS_PunchTime = data.ALIEOS_PunchTime;
                            result.ALIEOS_Reason = data.ALIEOS_Reason;

                            visctxt.Update(result);
                        }
                  
                    
                    int a = visctxt.SaveChanges();
                    if (a > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public LateInStudent_DTO editdata(LateInStudent_DTO data)
        {
            try
            {
               
               data.editlist = visctxt.LateInStudent_DMO.Where(t => t.MI_Id == data.MI_Id && t.ALIEOS_Id == data.ALIEOS_Id).ToArray();

                data.classList = (from a in _db.SchoolYearWiseStudent
                                  from b in _db.School_M_Class
                                  where (a.ASMAY_Id == data.ASMAY_Id && a.AMAY_ActiveFlag == 1 && a.ASMCL_Id == b.ASMCL_Id && b.ASMCL_ActiveFlag == true && b.MI_Id == data.MI_Id)
                                  select b).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();

                data.sectionList = (from a in _db.SchoolYearWiseStudent
                                    from b in _db.School_M_Section
                                    where (a.ASMAY_Id == data.ASMAY_Id && a.AMAY_ActiveFlag == 1 && a.ASMS_Id == b.ASMS_Id && b.ASMC_ActiveFlag == 1 && b.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id)
                                    select b).Distinct().OrderBy(t => t.ASMS_Id).ToArray();

                data.studentlist = (from a in _db.SchoolYearWiseStudent
                                    from b in _db.School_M_Section
                                    from c in _db.School_M_Class
                                    from d in _db.Adm_M_Student
                                    where (a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == b.ASMS_Id && a.AMST_Id == d.AMST_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && d.AMST_SOL == "S" && a.AMAY_ActiveFlag == 1 && d.AMST_ActiveFlag == 1)
                                    select new LateInStudent_DTO
                                    {
                                        AMST_Id = d.AMST_Id,
                                        studentName = d.AMST_FirstName + (string.IsNullOrEmpty(d.AMST_MiddleName) ? "" : ' ' + d.AMST_MiddleName) + (string.IsNullOrEmpty(d.AMST_LastName) ? "" : ' ' + d.AMST_LastName),
                                    }).Distinct().OrderBy(t => t.ASMS_Id).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public LateInStudent_DTO deactive(LateInStudent_DTO data)
        {
            try
            {

                var result = visctxt.LateInStudent_DMO.Single(t => t.ALIEOS_Id == data.ALIEOS_Id);

                if (result.ALIEOS_AactiveFlag == true)
                {
                    result.ALIEOS_AactiveFlag = false;
                }
                else if (result.ALIEOS_AactiveFlag == false)
                {
                    result.ALIEOS_AactiveFlag = true;
                }
                visctxt.Update(result);
                int rowAffected = visctxt.SaveChanges();
                if (rowAffected > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public async Task<string> sendSms(long MI_Id, string mobileNo, string Template, long UserID)
        {

            try
            {

                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name.Equals(Template, StringComparison.OrdinalIgnoreCase) && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = "";

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
                            result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                            sms = result;
                        }
                    }
                }

                sms = result;
                //sms = sms + Environment.NewLine + template.FirstOrDefault().ISES_MailFooter + Environment.NewLine;



                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();
                   // PHNO = "9771237044";
                    url = url.Replace("PHNO", PHNO);
                    url = url.Replace("MESSAGE", sms);
                    url = url.Replace("entityid", institutionName[0].MI_EntityId.ToString());
                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;

                    Stream stream = response.GetResponseStream();
                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);
                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;


                    var remoteIpAddress = "";   
                    string strHostName = "";
                    strHostName = System.Net.Dns.GetHostName();
                    IPHostEntry ipEntry = await System.Net.Dns.GetHostEntryAsync(strHostName);
                    IPAddress[] addr = ipEntry.AddressList;
                    remoteIpAddress = addr[addr.Length - 1].ToString();

                    string hostName = Dns.GetHostName();
                    var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                   string myIP1 = addr[addr.Length - 2].ToString();

                    NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                    String sMacAddress = string.Empty;
                    foreach (NetworkInterface adapter in nics)
                    {
                        if (sMacAddress == String.Empty)// only return MAC Address from first card
                        {
                            IPInterfaceProperties properties = adapter.GetIPProperties();
                            sMacAddress = adapter.GetPhysicalAddress().ToString();
                        }
                    }
                  

                    IVRM_sms_sentBoxDMO dmo2 = new IVRM_sms_sentBoxDMO();
                    dmo2.CreatedDate = DateTime.Now;
                    dmo2.Datetime = DateTime.Now;
                    dmo2.Message = sms.ToString();
                    dmo2.Message_id = messageid;
                    dmo2.MI_Id = MI_Id;
                    dmo2.Mobile_no = PHNO;
                    dmo2.Module_Name = "Visitors Management";
                    dmo2.To_FLag = "";
                    dmo2.UpdatedDate = DateTime.Now;
                    dmo2.System_Ip = remoteIpAddress;
                    dmo2.MacAddress_Ip = sMacAddress;
                    dmo2.network_Ip = myIP1;
                    if (messageid.Contains("GID") && messageid.Contains("ID"))
                    {
                        dmo2.statusofmessage = "Delivered";
                    }
                    else
                    {
                        dmo2.statusofmessage = messageid;
                    }

                    _db.Add(dmo2);
                    var flag = _db.SaveChanges();
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
                    return "Email Template not Mapped to the selected Institution";
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
                                result = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
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
                                Resultsms = Mailcontent.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
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


                        //Sending mail using SendGrid API.
                        //Date:07-02-2017.

                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                       // Email = "amanullah@vapstech.com";
                        message.AddTo(Email);

                        if (Attechement.Equals("1"))
                        {
                            var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                            if (img.Count > 0)
                            {
                                for (int i = 0; i < img.Count; i++)
                                {
                                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;
                                    System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                    Stream stream = response.GetResponseStream();
                                    message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);
                                }
                            }
                        }

                        if (mailcc != null && mailcc != "")
                        {
                            message.AddCc(mailcc);
                        }
                        if (mailbcc != null && mailbcc != "")
                        {
                            message.AddBcc(mailbcc);
                        }

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
                            Value = modulename[0]
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


    }
}
