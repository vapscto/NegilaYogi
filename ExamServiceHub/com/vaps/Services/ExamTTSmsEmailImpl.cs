using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using ExamServiceHub.com.vaps.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Exam;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace ExamServiceHub.com.vaps.Services
{
    public class ExamTTSmsEmailImpl : ExamTTSmsEmailInterface
    {
        public ExamContext _examctxt;
        private readonly subjectmasterContext _subctxt;
        private readonly IHostingEnvironment _hostingEnvironment;
        public DomainModelMsSqlServerContext _db;
        public ExamTTSmsEmailImpl(ExamContext obj, subjectmasterContext obj1, DomainModelMsSqlServerContext obj3, IHostingEnvironment hostingEnvironment)
        {
            _examctxt = obj;
            _subctxt = obj1;
            _db = obj3;
            _hostingEnvironment = hostingEnvironment;

        }
        public ExamTTSmsEmailDTO getdetails(ExamTTSmsEmailDTO data)
        {
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _examctxt.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.Acdlist = list.ToArray();

                data.examlist = _examctxt.masterexam.Where(z => z.MI_Id == data.MI_Id && z.EME_ActiveFlag == true).OrderBy(t => t.EME_ExamOrder).ToArray();

                List<AdmissionClass> classes = new List<AdmissionClass>();
                classes = _examctxt.AdmissionClass.Where(c => c.MI_Id == data.MI_Id && c.ASMCL_ActiveFlag == true).OrderBy(t => t.ASMCL_Order).ToList();
                data.ctlist = classes.Distinct().ToArray();

                List<School_M_Section> sections = new List<School_M_Section>();
                sections = _examctxt.School_M_Section.Where(c => c.MI_Id == data.MI_Id && c.ASMC_ActiveFlag == 1).OrderBy(t => t.ASMC_Order).ToList();
                data.seclist = sections.Distinct().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public ExamTTSmsEmailDTO onselectAcdYear(ExamTTSmsEmailDTO data)
        {
            try
            {
                data.ctlist = (from a in _examctxt.AdmissionClass
                               from b in _examctxt.Exm_Category_ClassDMO
                               from c in _examctxt.AcademicYear
                               where (b.MI_Id == data.MI_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMAY_Id == c.ASMAY_Id && a.ASMCL_ActiveFlag == true && b.ASMAY_Id == data.ASMAY_Id)
                               select new ExamTTSmsEmailDTO
                               {
                                   ASMCL_Id = a.ASMCL_Id,
                                   ASMCL_ClassName = a.ASMCL_ClassName,
                                   ASMCL_Order = a.ASMCL_Order
                               }).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public ExamTTSmsEmailDTO onselectclass(ExamTTSmsEmailDTO data)
        {
            try
            {
                data.seclist = (from a in _examctxt.School_M_Section
                                from b in _examctxt.Exm_Category_ClassDMO
                                from c in _examctxt.AdmissionClass
                                from d in _examctxt.AcademicYear
                                where (a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == b.ASMS_Id && c.ASMCL_Id == b.ASMCL_Id && d.ASMAY_Id == b.ASMAY_Id && a.ASMC_ActiveFlag == 1)
                                select new ExamTTSmsEmailDTO
                                {
                                    ASMS_Id = a.ASMS_Id,
                                    ASMC_SectionName = a.ASMC_SectionName,
                                    ASMC_Order = a.ASMC_Order
                                }
                                 ).Distinct().OrderBy(t => t.ASMC_Order).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public ExamTTSmsEmailDTO onselectSection(ExamTTSmsEmailDTO data)
        {
            try
            {
                var EMCA_Id = _examctxt.Exm_Category_ClassDMO.SingleOrDefault(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.ECAC_ActiveFlag == true).EMCA_Id;
                var EYC_Id = _examctxt.Exm_Yearly_CategoryDMO.SingleOrDefault(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.EMCA_Id == EMCA_Id && t.EYC_ActiveFlg == true).EYC_Id;
                var examlist = (from a in _examctxt.masterexam
                                from b in _examctxt.Exm_Yearly_Category_ExamsDMO
                                where (a.MI_Id == data.MI_Id && a.EME_ActiveFlag == true && a.EME_Id == b.EME_Id && b.EYC_Id == EYC_Id && b.EYCE_ActiveFlg == true)
                                select a).Distinct().OrderBy(t => t.EME_ExamOrder).ToList();
                data.examlist = examlist.Distinct().OrderBy(t => t.EME_ExamOrder).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public ExamTTSmsEmailDTO getStudentsTeachers(ExamTTSmsEmailDTO data)
        {
            try
            {
                data.studentlist = (from a in _examctxt.Adm_M_Student
                                    from b in _examctxt.School_Adm_Y_StudentDMO
                                    where (a.MI_Id == data.MI_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1 && b.AMST_Id == a.AMST_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMAY_ActiveFlag == 1 && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id)
                                    select new ExamTTSmsEmailDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),
                                        AMST_AdmNo = a.AMST_AdmNo == null ? "" : a.AMST_AdmNo,
                                        AMST_MobileNo = a.AMST_MobileNo,
                                        AMST_emailId = a.AMST_emailId
                                    }).Distinct().ToArray();

                data.teacherlist = (from a in _examctxt.ClassTeacherMappingDMO
                                    from b in _examctxt.HR_Master_Employee_DMO
                                    from c in _examctxt.Multiple_Email_DMO
                                    from d in _examctxt.Multiple_Mobile_DMO
                                    where (a.MI_Id == data.MI_Id && c.HRMEM_DeFaultFlag == "default" && d.HRMEMNO_DeFaultFlag == "default" && b.HRME_Id == c.HRME_Id
                                    && b.HRME_Id == d.HRME_Id && a.IMCT_ActiveFlag == true && b.HRME_Id == a.HRME_Id && a.ASMAY_Id == data.ASMAY_Id && b.HRME_ActiveFlag == true && b.HRME_LeftFlag == false && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id)
                                    select new ExamTTSmsEmailDTO
                                    {
                                        HRME_Id = b.HRME_Id,
                                        HRME_EmployeeCode = b.HRME_EmployeeCode,
                                        HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                        HRME_MobileNo = d.HRMEMNO_MobileNo,
                                        HRME_EmailId = c.HRMEM_EmailId

                                    }).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public ExamTTSmsEmailDTO generate(ExamTTSmsEmailDTO data)
        {
            try
            {

                data.generateTT = (from a in _examctxt.Exm_TimeTableDMO
                                   from b in _examctxt.Exm_TimeTable_SubjectsDMO
                                   from e in _examctxt.School_M_Section
                                   from f in _examctxt.AcademicYear
                                   from k in _examctxt.exammasterDMO
                                   from l in _subctxt.subjectmasterDMO
                                   from m in _examctxt.Exm_TT_M_SessionDMO
                                   where (a.EXTT_Id == b.EXTT_Id && a.ASMS_Id == e.ASMS_Id && f.ASMAY_Id == a.ASMAY_Id && k.EME_Id == a.EME_Id 
                                   && l.ISMS_Id == b.ISMS_Id && m.ETTS_Id == b.ETTS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id 
                                   && a.ASMCL_Id == data.ASMCL_Id && a.EME_Id == data.EME_Id && a.ASMS_Id == data.ASMS_Id)
                                   select new ExamTTSmsEmailDTO
                                   {
                                       ISMS_SubjectName = l.ISMS_SubjectName,
                                       ETTS_SessionName = m.ETTS_SessionName,
                                       EXTTS_Date = b.EXTTS_Date,
                                       ETTS_StartTime = m.ETTS_StartTime,
                                       ETTS_EndTime = m.ETTS_EndTime,
                                       EME_ExamName = k.EME_ExamName
                                   }).Distinct().OrderBy(a => Convert.ToDateTime(a.EXTTS_Date).Year).ThenBy(a => Convert.ToDateTime(a.EXTTS_Date).Month).ThenBy(a => Convert.ToDateTime(a.EXTTS_Date).Date).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public ExamTTSmsEmailDTO getsubjectgroup(ExamTTSmsEmailDTO data)
        {
            try
            {

                data.subject_group = (from a in _examctxt.Exm_Category_ClassDMO
                                      from b in _examctxt.Exm_Yearly_CategoryDMO
                                      from c in _examctxt.Exm_Yearly_Category_GroupDMO
                                      from d in _examctxt.Exm_Master_GroupDMO
                                      where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true && b.MI_Id == a.MI_Id && b.ASMAY_Id == a.ASMAY_Id && b.EYC_ActiveFlg == true && b.EMCA_Id == a.EMCA_Id && c.EYC_Id == b.EYC_Id && c.EYCG_ActiveFlg == true && d.MI_Id == a.MI_Id && d.EMG_ActiveFlag == true && d.EMG_Id == c.EMG_Id)
                                      select new ExamTTTransSettingsDTO
                                      {
                                          EMG_Id = d.EMG_Id,
                                          EMG_GroupName = d.EMG_GroupName

                                      }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamTTSmsEmailDTO sendmail(ExamTTSmsEmailDTO data)
        {
            try
            {
                string str = "";
                string accdetails = "";
                string datetime = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy");

                var examname = _examctxt.exammasterDMO.Where(t => t.EME_Id == data.EME_Id && t.MI_Id == data.MI_Id).Select(t => t.EME_ExamName).FirstOrDefault();

                var classname = _examctxt.AdmissionClass.Where(t => t.ASMCL_Id == data.ASMCL_Id && t.MI_Id == data.MI_Id).Select(t => t.ASMCL_ClassName).FirstOrDefault();

                var sectionname = _examctxt.School_M_Section.Where(t => t.ASMS_Id == data.ASMS_Id && t.MI_Id == data.MI_Id).Select(t => t.ASMC_SectionName).FirstOrDefault();

                var yearname = _examctxt.AcademicYear.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.MI_Id == data.MI_Id).Select(t => t.ASMAY_Year).FirstOrDefault();

                accdetails = "Year : " + yearname + " Class : " + classname + "  Section :" + sectionname;

                var get_details = (from a in _examctxt.Exm_TimeTableDMO
                                   from b in _examctxt.Exm_TimeTable_SubjectsDMO
                                   from d in _examctxt.exammasterDMO
                                   from e in _examctxt.AdmissionClass
                                   from f in _examctxt.School_M_Section
                                   from k in _examctxt.AcademicYear
                                   from l in _subctxt.subjectmasterDMO
                                   from m in _examctxt.Exm_TT_M_SessionDMO
                                   where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.EME_Id == data.EME_Id
                                   && a.ASMS_Id == data.ASMS_Id && a.EXTT_Id == b.EXTT_Id && a.EME_Id == d.EME_Id && a.ASMCL_Id == e.ASMCL_Id && a.ASMS_Id == f.ASMS_Id
                                   && a.ASMAY_Id == k.ASMAY_Id && l.ISMS_Id == b.ISMS_Id && m.ETTS_Id == b.ETTS_Id)
                                   select new ExamTTSmsEmailDTO
                                   {
                                       ISMS_SubjectName = l.ISMS_SubjectName,
                                       ETTS_SessionName = m.ETTS_SessionName,
                                       dateexam = Convert.ToDateTime(b.EXTTS_Date).ToString("dd/MM/yyyy"),
                                       EXTTS_Date = b.EXTTS_Date,
                                       ETTS_StartTime = m.ETTS_StartTime,
                                       ETTS_EndTime = m.ETTS_EndTime,
                                       EME_ExamName = d.EME_ExamName
                                   }).Distinct().OrderBy(a => Convert.ToDateTime(a.EXTTS_Date).Year).ThenBy(a => Convert.ToDateTime(a.EXTTS_Date).Month).ThenBy(a => Convert.ToDateTime(a.EXTTS_Date).Date).ToList();

                if (get_details.Count > 0)
                {
                    str += "<html xmlns=" + "http://www.w3.org/1999/xhtml" + ">  <head> <meta http-equiv=" + "Content-Type" + " content=" + "text/html;charset =utf-8" + "/><style > th, td {padding:8px;}tr:nth-child(even) {background-color:#ffe0b3;}</style> </head> <body>" +
                           "<table width='100%' border= '2'style=" + "border-collapse:collapse;border-width:2px" + "><tr align='center' width='100%'>" +
                            "<td colspan = 5> Exam Time Table For  " + examname + " : " + accdetails + "  <b>Date : " + datetime + " </b> </td > " +
                            "</tr> <tr style = 'background-color:cornflowerblue ; color:white;' ><td align=" + "center" + "> Subject Name </td> " +
                            "<td align=" + "center" + "> Session Name </td><td align=" + "center" + "> Exam Date </td> <td align=" + "center" + "> Start Time </td><td  align=" + "center" + "> End Time </td> </tr> ";

                    for (int i = 0; i < get_details.Count; i++)
                    {
                        str += "<tr><td align=" + "left" + " style=" + "padding-left:10px" + ">" + get_details[i].ISMS_SubjectName + "</td><td align=" + "left" + " style=" + "padding-left:10px" + ">" + get_details[i].ETTS_SessionName + "</td><td align=" + "center" + ">" + get_details[i].dateexam + "</td> <td align=" + "center" + ">" + get_details[i].ETTS_StartTime + "</td> <td align=" + "center" + ">" + get_details[i].ETTS_EndTime + "</td> </tr>";
                    }
                    str += "</table> </body> </html> ";

                }



                if (str == null || str.Length == 0)
                {

                }

                var examdate = (from a in _examctxt.Exm_TimeTableDMO
                                from b in _examctxt.Exm_TimeTable_SubjectsDMO
                                where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.EME_Id == data.EME_Id && a.ASMS_Id == data.ASMS_Id && a.EXTT_Id == b.EXTT_Id)
                                select new ExamTTSmsEmailDTO
                                {
                                    EXTTS_Date = b.EXTTS_Date,
                                }).Distinct().ToArray();



                for (int i = 0; i < data.check_save_studdto.Count(); i++)
                {
                    try
                    {
                        sendmailexamtt(data.MI_Id, data.check_save_studdto[i].AMST_emailId, "ExamTimeTable", data.check_save_studdto[i].AMST_Id, data.check_save_studdto[i].AMST_FirstName, examname, examdate.FirstOrDefault().EXTTS_Date, str);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    try
                    {
                        sendSms_examtt(data.MI_Id, data.check_save_studdto[i].AMST_MobileNo, "ExamTimeTable", data.check_save_studdto[i].AMST_Id, data.check_save_studdto[i].AMST_FirstName, examname);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                for (int j = 0; j < data.check_save_teachdto.Count(); j++)
                {
                    try
                    {
                        sendmailexamtt(data.MI_Id, data.check_save_teachdto[j].HRME_EmailId, "ExamTimeTable", 0, data.check_save_teachdto[j].HRME_EmployeeFirstName, examname, examdate[j].EXTTS_Date, str);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    try
                    {
                        sendSms_examtt(data.MI_Id, data.check_save_teachdto[j].HRME_MobileNo, "ExamTimeTable", data.check_save_teachdto[j].HRME_Id, data.check_save_teachdto[j].HRME_EmployeeFirstName, examname);
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
        public string sendSms_examtt(long MI_Id, long MobileNo, string Template, long Id, string name, string EME_ExamName)
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


                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    if (ParamaetersName[j].ISMP_NAME == "[NAME]")
                    {
                        result = sms.Replace(ParamaetersName[j].ISMP_NAME, name);
                        sms = result;
                    }
                    else if (ParamaetersName[j].ISMP_NAME == "[EXAMNAME]")
                    {
                        result = sms.Replace(ParamaetersName[j].ISMP_NAME, EME_ExamName);
                        sms = result;
                    }

                }
                sms = result;

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                List<Institution> insdeta = new List<Institution>();
                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = MobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    url = url.Replace("entity_id", insdeta[0].MI_EntityId.ToString());

                    url = url.Replace("template_id", template.FirstOrDefault().ISES_TemplateId);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = request.GetResponse() as System.Net.HttpWebResponse;
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
                // return ex.Message;
            }
            return "success";
        }
        public string sendmailexamtt(long MI_Id, string mail_id, string Template, long AMST_Id, string FirstName, string EME_ExamName, DateTime? EXTTS_Date, string url)
        {

            Dictionary<string, string> val = new Dictionary<string, string>();

            var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "ExamTimeTable" && e.ISES_MailActiveFlag == true).ToList();

            var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();
            var institutionName_email = _db.Institution_EmailId.Where(i => i.MI_Id == MI_Id).ToList();

            var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(i => i.ISMP_ID).ToList();

            var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

            string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;

            string result_value = Mailmsg;

            List<Match> variables = new List<Match>();

            foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
            {
                variables.Add(match);
            }


            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "Email_Exam_TimeTable";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add(new SqlParameter("@FirstName",
                      SqlDbType.VarChar)
                {
                    Value = FirstName
                });
                cmd.Parameters.Add(new SqlParameter("@EME_ExamName",
                              SqlDbType.VarChar)
                {
                    Value = EME_ExamName
                });
                cmd.Parameters.Add(new SqlParameter("@EXTTS_Date",
                             SqlDbType.DateTime)
                {
                    Value = EXTTS_Date
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
                    Console.WriteLine(ex.Message);
                }

            }

            for (int j = 0; j < ParamaetersName.Count; j++)
            {
                for (int p = 0; p < val.Count; p++)
                {
                    if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                    {
                        result_value = Mailmsg.Replace(ParamaetersName[j].ISMP_NAME, val[val.Keys.ToArray()[p]]);
                        Mailmsg = result_value;
                    }
                }
            }
            Mailmsg = result_value + url;

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
                if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                {
                    mailcc = alldetails[0].IVRM_mailcc.ToString();
                }


                //Sending mail using SendGrid API.
                //Date:07-02-2017.
                try
                {


                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
                    message.AddTo(mail_id);

                    if (mailcc != null && mailcc != "")
                    {
                        message.AddCc(mailcc);
                    }


                    // message.AddTo(d.amstemail);

                    message.HtmlContent = Mailmsg;

                    var client = new SendGridClient(sengridkey);

                    client.SendEmailAsync(message).Wait();
                }
                catch (AggregateException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (FileNotFoundException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (ArgumentNullException z)
                {
                    Console.WriteLine(z.Message);
                }
                catch (NotSupportedException z)
                {
                    Console.WriteLine(z.Message);
                }


                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == "ExamTimeTable" && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                    var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                    var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                    cmd.CommandText = "IVRM_Email_Outgoing";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@EmailId",
                        SqlDbType.NVarChar)
                    {
                        Value = mail_id
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
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return "success";
        }
    }


}
