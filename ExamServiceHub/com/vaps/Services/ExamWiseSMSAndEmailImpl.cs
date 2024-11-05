using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using DomainModel.Model.com.vaps.admission;
using System.Text.RegularExpressions;
using DomainModel.Model;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ExamServiceHub.com.vaps.Services
{
    public class ExamWiseSMSAndEmailImpl : Interfaces.ExamWiseSMSAndEmailInterface
    {
        public ExamContext _examcontext;
        public DomainModelMsSqlServerContext _db;
        public ExamWiseSMSAndEmailImpl(ExamContext exm, DomainModelMsSqlServerContext db)
        {
            _db = db;
            _examcontext = exm;
        }
        public ExamWiseSMSAndEmailDTO loaddata(ExamWiseSMSAndEmailDTO data)
        {
            try
            {
                data.allacademicyear = _examcontext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamWiseSMSAndEmailDTO getclass(ExamWiseSMSAndEmailDTO data)
        {
            try
            {

                data.allclasslist = (from a in _examcontext.Exm_Category_ClassDMO
                                     from b in _examcontext.AdmissionClass
                                     from c in _examcontext.AcademicYear
                                     where (a.ASMAY_Id == data.ASMAY_Id && a.ASMAY_Id == c.ASMAY_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.ASMCL_Id == b.ASMCL_Id && a.ECAC_ActiveFlag == true)
                                     select new PromotionSmsAndEmailDetailsReport_DTO
                                     {
                                         ASMCL_Id = b.ASMCL_Id,
                                         ASMCL_ClassName = b.ASMCL_ClassName,
                                     }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }
        public ExamWiseSMSAndEmailDTO getsection(ExamWiseSMSAndEmailDTO data)
        {
            try
            {

                data.allsectionlist = (from a in _examcontext.Exm_Category_ClassDMO
                                       from b in _examcontext.School_M_Section
                                       from c in _examcontext.AcademicYear
                                       from d in _examcontext.AdmissionClass
                                       where (a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == b.ASMS_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id
                                       && a.ASMCL_Id == data.ASMCL_Id && a.ECAC_ActiveFlag == true && b.ASMC_ActiveFlag == 1 && a.ASMCL_Id == data.ASMCL_Id)
                                       select b).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamWiseSMSAndEmailDTO getexam(ExamWiseSMSAndEmailDTO data)
        {
            try
            {
                List<long> emcaid = new List<long>();
                List<long> eycid = new List<long>();
                if (data.ASMS_Id == 0)
                {
                    var getemcaid1 = _examcontext.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.ASMCL_Id == data.ASMCL_Id && a.ECAC_ActiveFlag == true).ToList();

                    foreach (var c in getemcaid1)
                    {
                        emcaid.Add(c.EMCA_Id);
                    }

                    var geteycid1 = _examcontext.Exm_Yearly_CategoryDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && emcaid.Contains(a.EMCA_Id) && a.EYC_ActiveFlg == true).ToList();

                    foreach (var c in geteycid1)
                    {
                        eycid.Add(c.EYC_Id);
                    }
                }
                else
                {
                    var getemcaid = _examcontext.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).ToList();

                    foreach (var c in getemcaid)
                    {
                        emcaid.Add(c.EMCA_Id);
                    }

                    var geteycid = _examcontext.Exm_Yearly_CategoryDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && emcaid.Contains(a.EMCA_Id) && a.EYC_ActiveFlg == true).ToList();

                    foreach (var c in geteycid)
                    {
                        eycid.Add(c.EYC_Id);
                    }
                }

                data.allexamlist = (from a in _examcontext.Exm_Yearly_Category_ExamsDMO
                                    from b in _examcontext.exammasterDMO
                                    where (a.EME_Id == b.EME_Id && a.EYCE_ActiveFlg == true && eycid.Contains(a.EYC_Id))
                                    select b).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }
        public ExamWiseSMSAndEmailDTO searchDetails(ExamWiseSMSAndEmailDTO data)
        {

            string asmsid = "0";
            List<School_M_Section> sectionlist = new List<School_M_Section>();

            if (data.ASMS_Id == 0)
            {
                sectionlist = (from a in _examcontext.Exm_Category_ClassDMO
                               from b in _examcontext.School_M_Section
                               from c in _examcontext.AcademicYear
                               from d in _examcontext.AdmissionClass
                               where (a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == b.ASMS_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id
                               && a.ASMCL_Id == data.ASMCL_Id && a.ECAC_ActiveFlag == true && b.ASMC_ActiveFlag == 1 && a.ASMCL_Id == data.ASMCL_Id)
                               select b).Distinct().OrderBy(a => a.ASMC_Order).ToList();
            }
            else
            {
                sectionlist = (from a in _examcontext.Exm_Category_ClassDMO
                               from b in _examcontext.School_M_Section
                               from c in _examcontext.AcademicYear
                               from d in _examcontext.AdmissionClass
                               where (a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == b.ASMS_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id
                               && a.ASMCL_Id == data.ASMCL_Id && a.ECAC_ActiveFlag == true && b.ASMC_ActiveFlag == 1 && a.ASMCL_Id == data.ASMCL_Id
                               && a.ASMS_Id == data.ASMS_Id)
                               select b).Distinct().OrderBy(a => a.ASMC_Order).ToList();
            }

            foreach (var c in sectionlist)
            {
                asmsid = asmsid + "," + c.ASMS_Id;
            }


            try
            {
                using (var cmd = _examcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Wise_SMS_Email_Sending";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = asmsid
                    });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar)
                    {
                        Value = data.EME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@typeformat", SqlDbType.VarChar)
                    {
                        Value = data.typeformat
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
                        data.studentdetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }
        public ExamWiseSMSAndEmailDTO SendSmsEmail(ExamWiseSMSAndEmailDTO data)
        {
            try
            {
                SMS sms = new SMS(_db);
                Email email = new Email(_db);

                //int countsms = 0;
                //int countemail = 0;

                for (int i = 0; i < data.finalstudentlist.Length; i++)
                {
                    var amst_id = data.finalstudentlist[i].AMST_Id;
                    long MobileNo = Convert.ToInt64(data.finalstudentlist[i].MOBILENO);
                    var Email = data.finalstudentlist[i].EMAILID;

                    var e = "";
                    string f = "";
                    if (data.sms == true)
                    {
                        try
                        {
                            if (data.typeformat == "Marks")
                            {
                                e = sendsmstopperlist(data.MI_Id, MobileNo, "EXAM_WISE_SMS_EMAIL_DETAILS", amst_id, data.ASMAY_Id, data.EME_Id, 0).ToString();
                            }
                            else if (data.typeformat == "Progress")
                            {
                                e = sendsmstopperlist(data.MI_Id, MobileNo, "Exam_SMS_Marks_Details", amst_id, data.ASMAY_Id, data.EME_Id, 0).ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                    if (data.email == true)
                    {
                        try
                        {
                            if (data.typeformat == "Marks")
                            {
                                f = sendmailtopperlist(data.MI_Id, Email, "EXAM_WISE_SMS_EMAIL_DETAILS", amst_id, data.ASMAY_Id, data.EME_Id, 0);
                            }
                            else if (data.typeformat == "Progress")
                            {
                                f = sendmailtopperlist(data.MI_Id, Email, "Exam_SMS_Marks_Details", amst_id, data.ASMAY_Id, data.EME_Id, 0);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                data.message = "true";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        //SMS AND EMAIL
        public async Task<string> sendsmstopperlist(long MI_Id, long mobileNo, string Template, long UserID, long ASMAY_Id, long EME_Id, long ISMS_Id)
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
                        cmd.CommandText = "SMSMAILPARAMETER_TOPPERLIST_EXAM";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@template", SqlDbType.VarChar)
                        {
                            Value = Template
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                        {
                            Value = ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar)
                        {
                            Value = EME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar)
                        {
                            Value = ISMS_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                        {
                            Value = MI_Id
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
                }


                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                List<Institution> insdeta = new List<Institution>();
                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    url = url.Replace("entity_id", insdeta[0].MI_EntityId.ToString());

                    url = url.Replace("template_id", template.FirstOrDefault().ISES_TemplateId);

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
        public string sendmailtopperlist(long MI_Id, string Email, string Template, long UserID, long ASMAY_Id, long EME_Id, long ISMS_Id)
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


                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

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
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = "SMSMAILPARAMETER_TOPPERLIST_EXAM";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@template", SqlDbType.VarChar)
                        {
                            Value = Template
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                        {
                            Value = ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar)
                        {
                            Value = EME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar)
                        {
                            Value = ISMS_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                        {
                            Value = MI_Id
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
                }

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
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
                        string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }


                    //Sending mail using SendGrid API.
                    //Date:07-02-2017.

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
                    message.AddTo(Email);
                    if (mailcc != null && mailcc != "")
                    {
                        message.AddCc(mailcc);
                    }
                    if (mailbcc != null && mailbcc != "")
                    {
                        message.AddBcc(mailbcc);
                    }


                    // var client = new Web("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");

                    //if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                    //{
                    //    message.AddAttachment(template.FirstOrDefault().ISES_MailHTMLTemplate);

                    //}
                    message.HtmlContent = Mailmsg;

                    var client = new SendGridClient(sengridkey);

                    client.SendEmailAsync(message).Wait();


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
                            Value = Mailmsg
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
