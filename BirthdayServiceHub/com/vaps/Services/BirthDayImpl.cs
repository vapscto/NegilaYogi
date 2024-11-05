using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.Birthday;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.admission;
using PreadmissionDTOs.com.vaps.BirthDay;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using CommonLibrary;
using SendGrid.Helpers.Mail;
using SendGrid;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using PreadmissionDTOs.com.vaps.HRMS;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System.Threading.Tasks;
using System.Data.Common;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace BirthdayServiceHub.com.vaps.Services
{
    public class BirthDayImpl : Interfaces.BirthDayInterface
    {
        int MI_ID = 0;
        private static ConcurrentDictionary<string, BirthDayDTO> _login =
          new ConcurrentDictionary<string, BirthDayDTO>();

        public DomainModelMsSqlServerContext _db;
        public FOContext _FOContext;

        public BirthDayImpl(DomainModelMsSqlServerContext db, FOContext fOContext)
        {
            _db = db;
            _FOContext = fOContext;

        }
        public BirthDayDTO getdata(int id)
        {
            BirthDayDTO dto = new BirthDayDTO();
            //  dto.yeardropdown = _db.AcademicYear.Where(d => d.MI_Id == id && d.Is_Active == true).ToArray();
            dto.classDrpDwn = (from m in _db.School_M_Class
                               where m.MI_Id == id && m.ASMCL_ActiveFlag == true
                               select new School_M_ClassDTO
                               {
                                   ASMCL_Id = m.ASMCL_Id,
                                   ASMCL_ClassName = m.ASMCL_ClassName
                               }).ToArray();
            dto.sectionDrpDwn = (from n in _db.School_M_Section
                                 where n.MI_Id == id && n.ASMC_ActiveFlag == 1
                                 select new School_M_Section
                                 {
                                     ASMS_Id = n.ASMS_Id,
                                     ASMC_SectionName = n.ASMC_SectionName
                                 }).ToArray();
            dto.fillmonth = _db.month.Where(R => R.Is_Active == true).Distinct().ToArray();




            dto.fillyear = (from a in _db.HR_Master_LeaveYearDMO
                            where (a.MI_Id == id && a.HRMLY_ActiveFlag == true)
                            select new HR_Master_LeaveYearDTO
                            {
                                HRMLY_Id = Convert.ToInt32(a.HRMLY_Id),
                                HRMLY_LeaveYear = a.HRMLY_LeaveYear,
                                ASMAY_From_Date = a.HRMLY_FromDate,
                                ASMAY_To_Date = a.HRMLY_ToDate
                            }).Distinct().OrderByDescending(t => t.HRMLY_LeaveYear).ToArray();

            return dto;


        }
        public BirthDayDTO getlistthree(BirthDayDTO stu)
        {
            BirthDayDTO acdmc = new BirthDayDTO();
            try
            {
                List<BirthDayDTO> classlist1 = new List<BirthDayDTO>();
                string createddate = Convert.ToDateTime(stu.amst_dob).ToString("yyyy-MM-dd h:mm tt");

                var acd_Id = _db.AcademicYear.Where(t => t.MI_Id.Equals(stu.MI_Id) && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();

                stu.studentDetails = (from a in _db.School_Adm_Y_StudentDMO
                                      from b in _db.Adm_M_Student
                                      from c in _db.School_M_Section
                                      where (a.AMST_Id == b.AMST_Id && b.MI_Id == stu.MI_Id && a.ASMCL_Id == stu.asmcL_Id && a.ASMS_Id == stu.sectionid && b.AMST_DOB.Date == Convert.ToDateTime(createddate) && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1)
                                      group b by b.AMST_Id into g
                                      select new Adm_M_StudentDTO
                                      {
                                          AMST_Id = g.FirstOrDefault().AMST_Id,
                                          AMST_FirstName = ((g.FirstOrDefault().AMST_FirstName == null ? " " : g.FirstOrDefault().AMST_FirstName) + (g.FirstOrDefault().AMST_MiddleName == null ? " " : g.FirstOrDefault().AMST_MiddleName) + (g.FirstOrDefault().AMST_LastName == null ? " " : g.FirstOrDefault().AMST_LastName)).Trim(),
                                          AMST_emailId = g.FirstOrDefault().AMST_emailId,
                                          AMST_MobileNo = g.FirstOrDefault().AMST_MobileNo
                                      }).Distinct().ToArray();

                if (stu.studentDetails.Length > 0)
                {
                    stu.count = stu.studentDetails.Length;
                }
                else
                {
                    stu.count = 0;
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stu;
        }
        public BirthDayDTO staflist(BirthDayDTO stu1)
        {
            try
            {


                if (stu1.rdbbutton.Equals("student", StringComparison.OrdinalIgnoreCase))
                {
                    if (stu1.Logintype == "KIOSK" && stu1.Logintype != "")
                    {

                        DateTime fromdatecon = DateTime.Now;
                        string condate = "";
                        //fromdatecon = Convert.ToDateTime(stu1.fromdatecon.Value.Date.ToString("yyyy-MM-dd"));
                        condate = fromdatecon.ToString("yyyy-MM-dd");

                        var mo_id = _db.Organisation.Where(t => t.MO_Id == stu1.MO_Id).Select(d => d.MO_Id).FirstOrDefault();

                        using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Student_Birthday_Report_kiosk";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@tdate", SqlDbType.VarChar)
                            {
                                Value = condate
                            });

                            cmd.Parameters.Add(new SqlParameter("@MO_Id", SqlDbType.BigInt)
                            {
                                Value = mo_id
                            });


                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject = new List<dynamic>();
                            //var data = cmd.ExecuteNonQuery();

                            try
                            {
                                // var data = cmd.ExecuteNonQuery();

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
                                    stu1.studentlist = retObject.ToArray();

                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }

                        if (stu1.studentlist.Length > 0)
                        {
                            stu1.count = stu1.studentlist.Length;
                        }
                        else
                        {
                            stu1.count = 0;
                        }
                    }
                    else
                    {
                        var acd_Id = _db.AcademicYear.Where(t => t.MI_Id.Equals(stu1.MI_Id) && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();
                        stu1.studentlist = (from a in _db.Adm_M_Student
                                            from b in _db.School_Adm_Y_StudentDMO
                                            from c in _db.School_M_Class
                                            from d in _db.School_M_Section
                                            where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == c.ASMCL_Id && d.ASMS_Id == b.ASMS_Id && a.MI_Id == stu1.MI_Id && b.ASMAY_Id== acd_Id &&
                                            a.AMST_ActiveFlag == 1 && a.AMST_SOL.Equals("S") && b.AMAY_ActiveFlag == 1 && a.AMST_DOB.Date.Day == DateTime.Now.Day && a.AMST_DOB.Date.Month == DateTime.Now.Month)
                                            //group a by a.HRME_Id into g
                                            select new BirthDayDTO
                                            {
                                                AMST_Id = a.AMST_Id,
                                                studentName = a.AMST_FirstName + (string.IsNullOrEmpty(a.AMST_MiddleName) ? "" : ' ' + a.AMST_MiddleName) + (string.IsNullOrEmpty(a.AMST_LastName) ? "" : ' ' + a.AMST_LastName),
                                                AMST_emailId = a.AMST_emailId == null || a.AMST_emailId == "" ? "" : a.AMST_emailId,
                                                AMST_MobileNo = a.AMST_MobileNo == 0 || a.AMST_MobileNo == null ? 0 : a.AMST_MobileNo,
                                                amst_dob = a.AMST_DOB,
                                                ASMCL_ClassName = c.ASMCL_ClassName,
                                                ASMC_SectionName = d.ASMC_SectionName,
                                                PhotoPath = (string.IsNullOrEmpty(a.AMST_Photoname) ? "" : a.AMST_Photoname)
                                            }).Distinct().ToArray();
                    }


                    if (stu1.studentlist.Length > 0)
                    {
                        stu1.count = stu1.studentlist.Length;
                    }
                    else
                    {
                        stu1.count = 0;
                    }
                }
                else if (stu1.rdbbutton.Equals("Staff", StringComparison.OrdinalIgnoreCase))
                {
                    stu1.staffList = (from a in _db.HR_Master_Employee_DMO
                                      from b in _db.Multiple_Email_DMO
                                      from c in _db.Multiple_Mobile_DMO
                                      where (a.HRME_Id == b.HRME_Id && b.HRME_Id == c.HRME_Id && b.HRMEM_DeFaultFlag.Equals("default") && c.HRMEMNO_DeFaultFlag.Equals("default") && a.MI_Id == stu1.MI_Id && a.HRME_ActiveFlag == true && a.HRME_DOB.Value.Date.Day == DateTime.Now.Day && a.HRME_DOB.Value.Date.Month == DateTime.Now.Month && a.HRME_LeftFlag == false)
                                      //group a by a.HRME_Id into g
                                      select new BirthDayDTO
                                      {
                                          HRME_Id = a.HRME_Id,
                                          employeeName = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                          HRME_EmailId = b.HRMEM_EmailId == null || b.HRMEM_EmailId == "" ? "" : b.HRMEM_EmailId,
                                          HRME_MobileNo = c.HRMEMNO_MobileNo == 0 || c.HRMEMNO_MobileNo == null ? 0 : c.HRMEMNO_MobileNo,
                                          HRME_DOB = a.HRME_DOB,
                                          PhotoPath = (string.IsNullOrEmpty(a.HRME_Photo) ? "" : a.HRME_Photo)
                                      }).Distinct().ToArray();

                    if (stu1.staffList.Length > 0)
                    {
                        stu1.count = stu1.staffList.Length;
                    }
                    else
                    {
                        stu1.count = 0;
                    }
                }
                if (stu1.rdbbutton.Equals("Alumni", StringComparison.OrdinalIgnoreCase))
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "ALUMNI_BirthdayList_Day";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = stu1.MI_Id });
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

                            stu1.studentlist = retObject.ToArray();
                            //dataReader.close();


                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }

                    if (stu1.studentlist.Length > 0)
                    {
                        stu1.count = stu1.studentlist.Length;
                    }
                    else
                    {
                        stu1.count = 0;
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stu1;
        }
        public BirthDayDTO staflist1(BirthDayDTO stu2)
        {
            BirthDayDTO acdmc = new BirthDayDTO();
            try
            {
                List<BirthDayDTO> stafflist1 = new List<BirthDayDTO>();
                //sendSMSManually();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return stu2;
        }
        public void Check_SMS_Mail_Status(int id) // scheduler for all client
        {
            try
            {
                MI_ID = id;

                SendEmail(MI_ID);
                Check_Mail_Staff(MI_ID);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
        }
        public string SendEmail(int MI_ID)
        {
            string re = "";
            long trnsno = 0;
            try
            {
                var acd_Id = _db.AcademicYear.Where(t => t.MI_Id.Equals(MI_ID) && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();

                var que1 = (from a in _db.Adm_M_Student
                            from b in _db.School_Adm_Y_StudentDMO
                            from c in _db.School_M_Class
                            from d in _db.School_M_Section
                            where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMAY_Id == acd_Id && d.ASMS_Id == b.ASMS_Id && a.MI_Id == MI_ID && a.AMST_ActiveFlag == 1 && a.AMST_SOL.Equals("S") && b.AMAY_ActiveFlag == 1 && a.AMST_DOB.Date.Day == DateTime.Now.Day && a.AMST_DOB.Date.Month == DateTime.Now.Month)
                            select new BirthDayDTO
                            {
                                AMST_Id = a.AMST_Id,
                                AMST_emailId = a.AMST_emailId,
                                AMST_MobileNo = a.AMST_MobileNo
                            }).Distinct().ToList();

                if (que1.Count > 0)
                {
                    //  SMS NEW TABLES CODE START


                    //SMS smstransno = new SMS(_db);
                    //  public async Task<string> sendSmsnew(long MI_Id, long mobileNo, string Template, long UserID, string sms)
                    // trnsno = smstransno.getsmsno(MI_ID);

                    //string studentempflag = "STUDENT";

                    // SMS NEW TABLES CODE END
                    for (int i = 0; i < que1.Count; i++)
                    {
                        try
                        {
                            long id = que1[i].AMST_Id;
                            string email = Convert.ToString(que1[i].AMST_emailId);
                            string mobileNo = Convert.ToString(que1[i].AMST_MobileNo);
                            // string email = "rakeshrd307@gmail.com";
                            // string mobileNo = "9686061628";
                            string Template = "BIRTHDAY";
                            string type = "Student";
                            //string val = sendmail(MI_ID, email, Template, id, type, trnsno);
                            // string x = sendSms(MI_ID, mobileNo, Template, id, type, trnsno);
                            if (email != "" && email != null)
                            {
                                string val = sendmail(MI_ID, email, Template, id, type);
                            }
                            if (mobileNo != "" && mobileNo != null)
                            {
                                string x = sendSms(MI_ID, mobileNo, Template, id, type);
                            }

                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return re;
        }
        public string Check_Mail_Staff(int MI_ID)
        {
            string ret = "";
            long trnsno = 0;
            try
            {
                var que2 = (from a in _db.HR_Master_Employee_DMO
                            from b in _db.Multiple_Email_DMO
                            from c in _db.Multiple_Mobile_DMO
                            where (a.HRME_Id == b.HRME_Id && a.HRME_Id == c.HRME_Id && b.HRMEM_DeFaultFlag.Equals("default") && c.HRMEMNO_DeFaultFlag.Equals("default") && a.MI_Id == MI_ID && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.HRME_DOB.Value.Date.Day == DateTime.Now.Day && a.HRME_DOB.Value.Date.Month == DateTime.Now.Month)
                            //group a by a.HRME_Id into g
                            select new BirthDayDTO
                            {
                                HRME_Id = a.HRME_Id,
                                HRME_EmailId = b.HRMEM_EmailId,
                                HRME_MobileNo = c.HRMEMNO_MobileNo
                            }).ToList();

                if (que2.Count > 0)
                {
                    //  SMS NEW TABLES CODE START


                    SMS smstransno = new SMS(_db);
                    //  public async Task<string> sendSmsnew(long MI_Id, long mobileNo, string Template, long UserID, string sms)
                    //  trnsno = smstransno.getsmsno(MI_ID);

                    //string studentempflag = "STUDENT";

                    // SMS NEW TABLES CODE END
                    for (int i = 0; i < que2.Count; i++)
                    {
                        try
                        {
                            long id = que2[i].HRME_Id;
                            string email = Convert.ToString(que2[i].HRME_EmailId);
                            string mob = Convert.ToString(que2[i].HRME_MobileNo);
                            string Template = "STAFFBIRTHDAY";
                            string type = "Employee";
                            // string val = sendmail(MI_ID, email, Template, id, type, trnsno);
                            // string x = sendSms(MI_ID, mob, Template, id, type, trnsno);
                            if (email != null && email != "")
                            {
                                string val = sendmail(MI_ID, email, Template, id, type);
                            }

                            if (mob != null && mob != "")
                            {
                                string x = sendSms(MI_ID, mob, Template, id, type);
                            }
                        }
                        catch (Exception e)
                        {
                            continue;
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return ret;
        }

        public string sendmail(long MI_Id, string Email, string Template, long UserID, string type)
        {
            try
            {

                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name.Equals(Template, StringComparison.OrdinalIgnoreCase) && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailmsg = template.FirstOrDefault().ISES_Mail_Message;
                string result = "";
                List<Match> variables = new List<Match>();
                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "BIRTHDAY_PARAMETER";
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
                    cmd.Parameters.Add(new SqlParameter("@type",
                        SqlDbType.VarChar)
                    {
                        Value = type
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


                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string mailcc = alldetails[0].IVRM_mailcc;

                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;

                    if (mailcc != null && mailcc != "")
                    {
                        string[] mail_id = alldetails[0].IVRM_mailcc.Split(',');

                        if (mail_id.Length > 0)
                        {
                            for (int i = 0; i < mail_id.Length; i++)
                            {
                                message.AddBcc(mail_id[i]);
                            }
                        }
                    }

                    //Email = "amanullah@vapstech.com";
                    message.AddTo(Email);

                    string name = "";
                    string date1 = "";
                    if (type.Equals("Employee", StringComparison.OrdinalIgnoreCase))
                    {
                        var query1 = _db.HR_Master_Employee_DMO.Where(d => d.HRME_Id == UserID).Select(d => new HR_Master_Employee_DMO { HRME_EmployeeFirstName = d.HRME_EmployeeFirstName, HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName, HRME_EmployeeLastName = d.HRME_EmployeeLastName }).ToList();
                        name = query1.FirstOrDefault().HRME_EmployeeFirstName + (string.IsNullOrEmpty(query1.FirstOrDefault().HRME_EmployeeMiddleName) ? "" : ' ' + query1.FirstOrDefault().HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(query1.FirstOrDefault().HRME_EmployeeLastName) ? "" : ' ' + query1.FirstOrDefault().HRME_EmployeeLastName);
                        date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");

                    }
                    else if (type.Equals("Student", StringComparison.OrdinalIgnoreCase))
                    {
                        var query1 = _db.Adm_M_Student.Single(d => d.AMST_Id == UserID);
                        name = query1.AMST_FirstName + (string.IsNullOrEmpty(query1.AMST_MiddleName) ? "" : ' ' + query1.AMST_MiddleName) + (string.IsNullOrEmpty(query1.AMST_LastName) ? "" : ' ' + query1.AMST_LastName);
                        date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");
                    }

                    if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                    {
                        message.HtmlContent = Regex.Replace(template.FirstOrDefault().ISES_MailHTMLTemplate, @"\bname\b", name,
                            RegexOptions.IgnoreCase);

                        message.HtmlContent = Regex.Replace(message.HtmlContent, @"\bdate1\b", date1, RegexOptions.IgnoreCase);
                    }

                    else
                    {
                        message.HtmlContent = "HAPPY BIRTHDAY DEAR" + " " + name + "<br/> No Template Found";
                    }

                    if (alldetails.FirstOrDefault().IVRM_sendgridkey != "" && alldetails.FirstOrDefault().IVRM_sendgridkey != null)
                    {
                        var client = new SendGridClient(alldetails.FirstOrDefault().IVRM_sendgridkey);
                        client.SendEmailAsync(message).Wait();
                        //string status=client.DeliverAsync(message).Status.ToString();
                    }

                    else
                    {
                        return "Sendgrid key is not available";
                    }

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing_1";
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
                            Value = "BIRTHDAY"
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@type",
                        SqlDbType.VarChar)
                        {
                            Value = type
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


        public string sendmailApiCall(string QCAC_Name, string QCAC_Email, string QCAC_Subject, string QCAC_Query, string QCAC_MobileNo)
        {
            try
            {
               
                string SendingEmail = "vapsregister1@gmail.com";
                string SendingEmailPassword = "vaps@123";
                string SendingEmailHostName = "smtp.gmail.com";
                Int32 PortNumber = 587;
                string Subject ="Test";
                string sengridkey = "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";
                
                var message = new SendGridMessage();
                message.From = new EmailAddress(SendingEmail);
                message.Subject = Subject;
                string ISES_MailBCCId = "rajkumar@vapstech.com";
                if (ISES_MailBCCId != null && ISES_MailBCCId != "")
                {
                    string[] ccmail = ISES_MailBCCId.Split(',');
                    foreach (var c in ccmail)
                    {
                        if (c != "")
                        {
                            message.AddCc(c);
                        }
                    }

                }
                
                message.Subject = Subject;
                string emailid = "sale.bharathielectricals@gmail.com";
                message.AddTo(emailid);

                message.HtmlContent =
                    "Customer Name" + " " +":" +" " + QCAC_Name + "<br/> " +
                    "Customer Email" + " " + ":" + " " + QCAC_Email + "<br/> " +
                    "Customer Subject" + " " + ":" + " " + QCAC_Subject + "<br/> " + 
                    "Customer Query " + " " + ":" + " " + QCAC_Query + "<br/> " + 
                    "Customer Mobile No" + " " + ":" + " " + QCAC_MobileNo + "<br/> "  ;
 
               
                var client = new SendGridClient(sengridkey);
                client.SendEmailAsync(message).Wait();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }
        public string sendSms(long MI_Id, string mobileNo, string Template, long UserID, string type)
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
                    cmd.CommandText = "BIRTHDAY_PARAMETER";
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
                    cmd.Parameters.Add(new SqlParameter("@type",
                      SqlDbType.VarChar)
                    {
                        Value = type
                    });
                    cmd.Parameters.Add(new SqlParameter("@coe_id",
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
                if (template.FirstOrDefault().ISES_MailFooter != null && template.FirstOrDefault().ISES_MailFooter != "")
                {
                    try
                    {
                        sms = sms + Environment.NewLine + template.FirstOrDefault().ISES_MailFooter + Environment.NewLine;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
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
                    // url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());
                    // url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);
                    if (url.Contains("entityid"))
                    {
                        if (insdeta[0].MI_EntityId.ToString() != null && insdeta[0].MI_EntityId.ToString() != "")
                        {
                            url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());
                        }
                    }

                    if (url.Contains("templateid"))
                    {
                        if (template.FirstOrDefault().ISES_TemplateId != null && template.FirstOrDefault().ISES_TemplateId != "")
                        {
                            url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);
                        }
                    }


                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();
                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);
                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;

                    try
                    {
                        IVRM_sms_sentBoxDMO dmo2 = new IVRM_sms_sentBoxDMO();
                        dmo2.CreatedDate = DateTime.Now;
                        dmo2.Datetime = DateTime.Now;
                        dmo2.Message = sms.ToString();
                        dmo2.Message_id = messageid;
                        dmo2.MI_Id = MI_Id;
                        dmo2.Mobile_no = PHNO;
                        dmo2.Module_Name = "BIRTHDAY";
                        dmo2.To_FLag = type;
                        dmo2.UpdatedDate = DateTime.Now;
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
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
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




        public async Task<string> sendWhatsAppCall(BirthDayDTO msg)
        {
            if (msg.rdbbutton.Equals("student"))
            {
                //if (msg.WhatsappFlag == null)
                //{
                    for (int i = 0; i < msg.selectedStudent.Length; i++)
                    {
                        //string s = sendmail(msg.MI_Id, msg.selectedStudent[i].AMST_emailId, "BIRTHDAY", msg.selectedStudent[i].AMST_Id, "Student", trnsno);
                        string s = await Whatsapp(msg.MI_Id, msg.selectedStudent[i].AMST_MobileNo.ToString(), "birthdaynew", msg.selectedStudent[i].AMST_Id, "Student");
                        if (s.Equals("success"))
                        {
                            msg.whatsappstatus = "success";
                        }
                    }
                //}
                //else
                //{

                //}
            }
            else
            {

            }
            return "success";
        }

        public async Task<string> Whatsapp(long MI_Id, string mobileNo, string Template, long UserID, string type)
        {
            try
            {
                ApiParm parm = new ApiParm();

                parm.messaging_product = "whatsapp";
                parm.to = mobileNo;
                parm.type = "template";
                string name = "";
                string InstituteName = "";
                var query2 = _db.Institution.Single(d => d.MI_Id == MI_Id);
                InstituteName = query2.MI_Name;


                if (type.Equals("Employee", StringComparison.OrdinalIgnoreCase))
                {
                    var query1 = _db.HR_Master_Employee_DMO.Where(d => d.HRME_Id == UserID).Select(d => new HR_Master_Employee_DMO { HRME_EmployeeFirstName = d.HRME_EmployeeFirstName, HRME_EmployeeMiddleName = d.HRME_EmployeeMiddleName, HRME_EmployeeLastName = d.HRME_EmployeeLastName }).ToList();
                    name = query1.FirstOrDefault().HRME_EmployeeFirstName + (string.IsNullOrEmpty(query1.FirstOrDefault().HRME_EmployeeMiddleName) ? "" : ' ' + query1.FirstOrDefault().HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(query1.FirstOrDefault().HRME_EmployeeLastName) ? "" : ' ' + query1.FirstOrDefault().HRME_EmployeeLastName);
                }
                else if (type.Equals("Student", StringComparison.OrdinalIgnoreCase))
                {
                    var query1 = _db.Adm_M_Student.Single(d => d.AMST_Id == UserID);
                    name = query1.AMST_FirstName + (string.IsNullOrEmpty(query1.AMST_MiddleName) ? "" : ' ' + query1.AMST_MiddleName) + (string.IsNullOrEmpty(query1.AMST_LastName) ? "" : ' ' + query1.AMST_LastName);
                }
                var datalist  = (from a in _db.smsEmailSetting
                               from b in _db.IVRM_SMS_Email_Setting_Parameter
                               from d in _db.SMS_MAIL_PARAMETER_DMO
                               where (a.MI_Id == MI_Id && a.ISES_Id == b.ISES_Id&& b.ISMP_Id == d.ISMP_ID && a.ISES_Template_Name == Template)

                               select new BirthDayDTO
                               {
                                   ISES_NAME=d.ISMP_NAME,
                                   ISES_WhatsAppTemplateId = a.ISES_WhatsAppTemplateId
                               }).Distinct().ToArray();

                List<parameter> p = new List<parameter>
                {
                    new parameter
                    {
                         type="text",
                         text= name


                    },
                    new parameter
                    {
                        type="text",
                        text= InstituteName
                    }
                };
                List<component> c = new List<component>
            {
                new component
                {
                   type="body",
                    parameters = p
                },
            };

                language l = new language();
                l.code = "en";
                templete t = new templete();
                t.name = datalist.FirstOrDefault().ISES_WhatsAppTemplateId;
                t.language = l;
                t.components = c;
                parm.template = t;

                try
                {
                    var client = new HttpClient();
                    var jsonContent = JsonConvert.SerializeObject(parm);
                    var requestnew = new HttpRequestMessage
                    {
                        Method = System.Net.Http.HttpMethod.Post,
                        RequestUri = new Uri("https://graph.facebook.com/v17.0/177948302058854/messages?access_token=EAALIaEGktRgBOxmK05MMqsi9jKwo4ZBSzps3ZAWckqVLvDMe0LZCSPZCDHEUkrsGjuKkR43wKzvBVZC4XnPNrESHgZC9ZADLVZAHLAAZCDZCY7mhy6hdqwt2FRTTAdjUUqrqNUrwasJwdOBEeXzFGDonc5ri5KZCts64szUfrhQ1kc0RLIdeQWjYZAx4Gi2SupDgg3aP2e8xsSfMt4QVVj7u"),
                        Content = new StringContent(jsonContent, Encoding.UTF8, "application/json")
                    };
                    using (var response123 = await client.SendAsync(requestnew))
                    {
                        response123.EnsureSuccessStatusCode();
                        var body = await response123.Content.ReadAsStringAsync();
                        Console.WriteLine(body);
                        JObject joResponse1 = JObject.Parse(body);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return ex.Message;
            }
            return "success";
        }

        public BirthDayDTO Sendmsg(BirthDayDTO msg) //Send SMS & Email from front end
        {
            long trnsno = 0;

            try
            {
                SMS smstransno = new SMS(_db);
                //  public async Task<string> sendSmsnew(long MI_Id, long mobileNo, string Template, long UserID, string sms)
                // trnsno = smstransno.getsmsno(msg.MI_Id);
                msg.smsStatus = "";
                msg.emailStatus = "";

                if (msg.rdbbutton.Equals("student"))
                {
                    if (msg.smsflag != null && msg.smsflag != "")
                    {
                        for (int i = 0; i < msg.selectedStudent.Length; i++)
                        {
                            //string s = sendSms(msg.MI_Id, msg.selectedStudent[i].AMST_MobileNo.ToString(), "BIRTHDAY", msg.selectedStudent[i].AMST_Id, "Student", trnsno);
                            string s = sendSms(msg.MI_Id, msg.selectedStudent[i].AMST_MobileNo.ToString(), "BIRTHDAY", msg.selectedStudent[i].AMST_Id, "Student");
                            if (s.Equals("success"))
                            {
                                msg.smsStatus = "SMS";
                            }
                        }
                    }
                    if (msg.emailflag != null && msg.emailflag != "")
                    {
                        for (int i = 0; i < msg.selectedStudent.Length; i++)
                        {
                            //string s = sendmail(msg.MI_Id, msg.selectedStudent[i].AMST_emailId, "BIRTHDAY", msg.selectedStudent[i].AMST_Id, "Student", trnsno);
                            string s = sendmail(msg.MI_Id, msg.selectedStudent[i].AMST_emailId, "BIRTHDAY", msg.selectedStudent[i].AMST_Id, "Student");
                            if (s.Equals("success"))
                            {
                                msg.emailStatus = "EMAIL";
                            }
                        }
                    }

                }
                else if (msg.rdbbutton.Equals("Staff"))
                {
                    if (msg.smsflag != null && msg.smsflag != "")
                    {
                        for (int i = 0; i < msg.selectedEmployees.Length; i++)
                        {
                            // string s = sendSms(msg.MI_Id, msg.selectedEmployees[i].HRME_MobileNo.ToString(), "BIRTHDAY", msg.selectedEmployees[i].HRME_Id, "Employee", trnsno);
                            string s = sendSms(msg.MI_Id, msg.selectedEmployees[i].HRME_MobileNo.ToString(), "STAFFBIRTHDAY", msg.selectedEmployees[i].HRME_Id, "Employee");
                            if (s.Equals("success"))
                            {
                                msg.smsStatus = "SMS";
                            }
                        }
                    }
                    if (msg.emailflag != null && msg.emailflag != "")
                    {
                        for (int i = 0; i < msg.selectedEmployees.Length; i++)
                        {
                            //string s = sendmail(msg.MI_Id, msg.selectedEmployees[i].HRME_EmailId, "BIRTHDAY", msg.selectedEmployees[i].HRME_Id, "Employee", trnsno);
                            string s = sendmail(msg.MI_Id, msg.selectedEmployees[i].HRME_EmailId, "STAFFBIRTHDAY", msg.selectedEmployees[i].HRME_Id, "Employee");
                            if (s.Equals("success"))
                            {
                                msg.emailStatus = "EMAIL";
                            }
                        }
                    }
                }
                else if (msg.rdbbutton.Equals("Alumni"))
                {
                    if (msg.smsflag != null && msg.smsflag != "")
                    {
                        for (int i = 0; i < msg.selectedStudent.Length; i++)
                        {
                            //string s = sendSms(msg.MI_Id, msg.selectedStudent[i].AMST_MobileNo.ToString(), "BIRTHDAY", msg.selectedStudent[i].AMST_Id, "Student", trnsno);
                            string s = sendSms(msg.MI_Id, msg.selectedStudent[i].AMST_MobileNo.ToString(), "BIRTHDAY", msg.selectedStudent[i].AMST_Id, "Alumni");
                            if (s.Equals("success"))
                            {
                                msg.smsStatus = "SMS";
                            }
                        }
                    }
                    if (msg.emailflag != null && msg.emailflag != "")
                    {
                        for (int i = 0; i < msg.selectedStudent.Length; i++)
                        {
                            //string s = sendmail(msg.MI_Id, msg.selectedStudent[i].AMST_emailId, "BIRTHDAY", msg.selectedStudent[i].AMST_Id, "Student", trnsno);
                            string s = sendmail(msg.MI_Id, msg.selectedStudent[i].AMST_emailId, "BIRTHDAY", msg.selectedStudent[i].AMST_Id, "Alumni");
                            if (s.Equals("success"))
                            {
                                msg.emailStatus = "EMAIL";
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return msg;
        }


        public BirthDayDTO QueryContact_ApiCall(BirthDayDTO msg) //Send SMS & Email from front end
        {
            

            try
            {
                var QueryContact = _db.Database.ExecuteSqlCommand("QueryContact_ApiCall_Insert @p0,@p1,@p2,@p3,@p4", msg.QCAC_Name, msg.QCAC_Email, msg.QCAC_Subject, msg.QCAC_Query, msg.QCAC_MobileNo);

                if (QueryContact > 0)
                {
                    string s = sendmailApiCall(msg.QCAC_Name, msg.QCAC_Email, msg.QCAC_Subject, msg.QCAC_Query, msg.QCAC_MobileNo);
                    if (s.Equals("success"))
                    {
                        msg.emailStatus = "EMAIL";
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return msg;
        }


        public BirthDayDTO getReport(BirthDayDTO rpt)
        {
            try
            {
                if (rpt.rdbbutton.Equals("student", StringComparison.OrdinalIgnoreCase))
                {
                    var acd_Id = _db.AcademicYear.Where(t => t.MI_Id.Equals(rpt.MI_Id) && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();

                    rpt.studentlist = (from a in _db.Adm_M_Student
                                       from b in _db.School_Adm_Y_StudentDMO
                                       from c in _db.School_M_Class
                                       from d in _db.School_M_Section
                                       where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id && a.MI_Id == rpt.MI_Id && a.AMST_ActiveFlag == 1 && a.AMST_SOL.Equals("S") && b.AMAY_ActiveFlag == 1 && a.AMST_DOB.DayOfYear >= rpt.start_date.Value.DayOfYear && a.AMST_DOB.DayOfYear <= rpt.end_date.Value.DayOfYear && a.AMST_DOB.Month >= rpt.start_date.Value.Month && a.AMST_DOB.Month <= rpt.end_date.Value.Month)
                                       select new BirthDayDTO
                                       {
                                           AMST_Id = a.AMST_Id,
                                           studentName = a.AMST_FirstName + (string.IsNullOrEmpty(a.AMST_MiddleName) ? "" : ' ' + a.AMST_MiddleName) + (string.IsNullOrEmpty(a.AMST_LastName) ? "" : ' ' + a.AMST_LastName),
                                           AMST_emailId = a.AMST_emailId == null || a.AMST_emailId == "" ? "" : a.AMST_emailId,
                                           AMST_MobileNo = a.AMST_MobileNo == 0 || a.AMST_MobileNo == null ? 0 : a.AMST_MobileNo,
                                           amst_dob = a.AMST_DOB,
                                           ASMCL_ClassName = c.ASMCL_ClassName,
                                           ASMC_SectionName = d.ASMC_SectionName
                                       }
                                ).ToArray();
                    if (rpt.studentlist.Length > 0)
                    {
                        rpt.count = rpt.studentlist.Length;
                    }
                    else
                    {
                        rpt.count = 0;
                    }

                }
                else if (rpt.rdbbutton.Equals("Staff", StringComparison.OrdinalIgnoreCase))
                {
                    rpt.staffList = (from a in _db.HR_Master_Employee_DMO
                                     from b in _db.Multiple_Email_DMO
                                     from c in _db.Multiple_Mobile_DMO
                                     where (a.HRME_Id == b.HRME_Id && a.HRME_Id == c.HRME_Id && b.HRMEM_DeFaultFlag.Equals("default") && c.HRMEMNO_DeFaultFlag.Equals("default") && a.MI_Id == rpt.MI_Id && a.HRME_ActiveFlag == true && a.HRME_DOB.Value.DayOfYear >= rpt.start_date.Value.DayOfYear && a.HRME_DOB.Value.DayOfYear <= rpt.end_date.Value.DayOfYear && a.HRME_DOB.Value.Month >= rpt.start_date.Value.Month && a.HRME_DOB.Value.Month <= rpt.end_date.Value.Month)
                                     select new BirthDayDTO
                                     {
                                         HRME_Id = a.HRME_Id,
                                         employeeName = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                         HRME_EmailId = b.HRMEM_EmailId == null || b.HRMEM_EmailId == "" ? "" : b.HRMEM_EmailId,
                                         HRME_MobileNo = c.HRMEMNO_MobileNo == 0 || c.HRMEMNO_MobileNo == null ? 0 : c.HRMEMNO_MobileNo,
                                         HRME_DOB = a.HRME_DOB
                                     }
                                 ).ToArray();
                    if (rpt.staffList.Length > 0)
                    {
                        rpt.count = rpt.staffList.Length;
                    }
                    else
                    {
                        rpt.count = 0;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return rpt;
        }
        public BirthDayDTO getEmailSMSCount(BirthDayDTO rpt)
        {
            try
            {
                string[] s1 = { "staff", "Employee" };
                List<IVRM_sms_sentBoxDMO> list = new List<IVRM_sms_sentBoxDMO>();
                List<ivrm_email_sentbox> list1 = new List<ivrm_email_sentbox>();
                if (rpt.rdbbutton == "smscount")
                {
                    if (rpt.studChecked == true && rpt.staffChecked == true)
                    {
                        string[] s = { "Student", "staff", "Employee", "Other" };

                        list = _db.IVRM_sms_sentBoxDMO.Where(d => d.MI_Id == rpt.MI_Id && d.Datetime.Date >= rpt.start_date.Value.Date && d.Datetime.Date <= rpt.end_date.Value.Date && s.Contains(d.To_FLag)).ToList();
                    }
                    else if (rpt.studChecked == true)
                    {
                        list = _db.IVRM_sms_sentBoxDMO.Where(d => d.MI_Id == rpt.MI_Id && d.Datetime.Date >= rpt.start_date.Value.Date && d.Datetime.Date <= rpt.end_date.Value.Date && d.To_FLag.Equals("Student", StringComparison.OrdinalIgnoreCase)).ToList();
                    }
                    else if (rpt.staffChecked == true)
                    {
                        list = _db.IVRM_sms_sentBoxDMO.Where(d => d.MI_Id == rpt.MI_Id && d.Datetime.Date >= rpt.start_date.Value.Date && d.Datetime.Date <= rpt.end_date.Value.Date && s1.Contains(d.To_FLag)).ToList();
                    }

                    rpt.sms_mail_count = list.ToArray();
                    rpt.count = rpt.sms_mail_count.Length;
                }
                else if (rpt.rdbbutton == "emailcount")
                {
                    if (rpt.studChecked == true && rpt.staffChecked == true)
                    {
                        string[] s = { "Student", "staff", "Employee", "Other" };
                        list1 = _db.ivrm_email_sentbox.Where(d => d.MI_Id == rpt.MI_Id && d.Datetime.Value.Date >= rpt.start_date.Value.Date && d.Datetime.Value.Date <= rpt.end_date.Value.Date && s.Contains(d.To_FLag)).ToList();
                    }
                    else if (rpt.studChecked == true)
                    {
                        string[] s = { "Student" };
                        // list1 = _db.ivrm_email_sentbox.Where(d => d.MI_Id == rpt.MI_Id && d.Datetime.Value.Date >= rpt.start_date.Value.Date && d.Datetime.Value.Date <= rpt.end_date.Value.Date && d.To_FLag.Equals("Student", StringComparison.OrdinalIgnoreCase)).ToList();
                        list1 = _db.ivrm_email_sentbox.Where(d => d.MI_Id == rpt.MI_Id && d.Datetime.Value.Date >= rpt.start_date.Value.Date && d.Datetime.Value.Date <= rpt.end_date.Value.Date && s.Contains(d.To_FLag)).ToList();
                    }
                    else if (rpt.staffChecked == true)
                    {
                        list1 = _db.ivrm_email_sentbox.Where(d => d.MI_Id == rpt.MI_Id && d.Datetime.Value.Date >= rpt.start_date.Value.Date && d.Datetime.Value.Date <= rpt.end_date.Value.Date && s1.Contains(d.To_FLag)).ToList();
                    }
                    rpt.mail_count_list = list1.ToArray();
                    rpt.count = rpt.mail_count_list.Length;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return rpt;
        }
        public BirthDayDTO SearchByColumn(BirthDayDTO search)
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
        public BirthDayDTO getmonthreport(BirthDayDTO rpt)
        {
            try
            {
                rpt.year = _db.AcademicYear.Where(t => t.MI_Id.Equals(rpt.MI_Id) && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();

                rpt.studentlist = (from a in _db.Adm_M_Student
                                   from b in _db.School_Adm_Y_StudentDMO
                                   from c in _db.School_M_Class
                                   from d in _db.School_M_Section

                                   where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id && a.MI_Id == rpt.MI_Id && a.AMST_ActiveFlag == 1 && a.AMST_SOL.Equals("S") && b.AMAY_ActiveFlag == 1 && a.AMST_DOB.Month == rpt.month && b.ASMAY_Id == rpt.year)
                                   select new BirthDayDTO
                                   {
                                       AMST_Id = a.AMST_Id,
                                       studentName = a.AMST_FirstName + (string.IsNullOrEmpty(a.AMST_MiddleName) ? "" : ' ' + a.AMST_MiddleName) + (string.IsNullOrEmpty(a.AMST_LastName) ? "" : ' ' + a.AMST_LastName),
                                       AMST_emailId = a.AMST_emailId == null || a.AMST_emailId == "" ? "" : a.AMST_emailId,
                                       AMST_MobileNo = a.AMST_MobileNo == 0 || a.AMST_MobileNo == null ? 0 : a.AMST_MobileNo,
                                       amst_dob = a.AMST_DOB,
                                       ASMCL_ClassName = c.ASMCL_ClassName,
                                       ASMC_SectionName = d.ASMC_SectionName
                                   }
                             ).ToArray();

                if (rpt.studentlist.Length > 0)
                {
                    rpt.count1 = rpt.studentlist.Length;
                }
                else
                {
                    rpt.count1 = 0;
                }
                rpt.staffList = (from a in _db.HR_Master_Employee_DMO
                                 where ( a.MI_Id == rpt.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.HRME_DOB.Value.Month == rpt.month)
                                 select new BirthDayDTO
                                 {
                                     HRME_Id = a.HRME_Id,
                                     employeeName = a.HRME_EmployeeFirstName + (string.IsNullOrEmpty(a.HRME_EmployeeMiddleName) ? "" : ' ' + a.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(a.HRME_EmployeeLastName) ? "" : ' ' + a.HRME_EmployeeLastName),
                                     HRME_DOB = a.HRME_DOB
                                 }
                             ).ToArray();
                if (rpt.staffList.Length > 0)
                {
                    rpt.count2 = rpt.staffList.Length;
                }
                else
                {
                    rpt.count2 = 0;
                }


                var quee1 = (from a in _db.AcademicYear
                             where (a.ASMAY_Id == rpt.year && a.MI_Id == rpt.MI_Id)
                             select new BirthDayDTO
                             {
                                 date11 = a.ASMAY_From_Date
                             }
                             ).ToArray();
                var quee2 = (from a in _db.AcademicYear
                             where (a.ASMAY_Id == rpt.year && a.MI_Id == rpt.MI_Id)
                             select new BirthDayDTO
                             {
                                 date12 = a.ASMAY_To_Date
                             }
                            ).ToArray();


                //new code added by roopa
                rpt.smsStatus1 = (from aw in _db.IVRM_sms_sentBoxDMO

                                  where (aw.MI_Id == rpt.MI_Id && aw.Module_Name == "BIRTHDAY" &&
                                  aw.Datetime.Year == rpt.ayear
                                  && aw.Datetime.Month == rpt.month && aw.To_FLag == "Student")
                                  select new BirthDayDTO
                                  {
                                      ssb1 = aw.IVRM_SSB_ID
                                  }).Count().ToString();

                rpt.smsStatus2 = (from aw in _db.IVRM_sms_sentBoxDMO

                                  where (aw.MI_Id == rpt.MI_Id && aw.Module_Name == "BIRTHDAY"
                                  && aw.Datetime.Year == rpt.ayear
                                  && aw.Datetime.Month == rpt.month && aw.To_FLag != "Student")
                                  select new BirthDayDTO
                                  {
                                      ssb2 = aw.IVRM_SSB_ID
                                  }).Count().ToString();

                rpt.AMST_emailId1 = (from aw2 in _db.ivrm_email_sentbox

                                     where (aw2.MI_Id == rpt.MI_Id && aw2.Module_Name == "BIRTHDAY" &&
                                     aw2.Datetime.Value.Year == rpt.ayear

                                     && aw2.Datetime.Value.Month == rpt.month
                                     && aw2.To_FLag == "Student")
                                     select new BirthDayDTO
                                     {
                                         esb1 = aw2.IVRMESB_ID
                                     }).Count().ToString();

                rpt.AMST_emailId2 = (from aw2 in _db.ivrm_email_sentbox

                                     where (aw2.MI_Id == rpt.MI_Id && aw2.Module_Name == "BIRTHDAY"
                                     && aw2.Datetime.Value.Year == rpt.ayear
                                     && aw2.Datetime.Value.Month == rpt.month
                                     && aw2.To_FLag != "Student")
                                     select new BirthDayDTO
                                     {
                                         esb2 = aw2.IVRMESB_ID
                                     }).Count().ToString();



            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return rpt;
        }
        public async Task<BirthDayDTO> getstaffdetails(BirthDayDTO data)
        {
            // id = 12;
            //return _dd.getdetails(data);

            DateTime fromdatecon = DateTime.Now;
            string confromdate = "";
            fromdatecon = Convert.ToDateTime(data.Fromdate.Value.Date.ToString("yyyy-MM-dd"));
            confromdate = fromdatecon.ToString("yyyy-MM-dd");

            DateTime todatecon = DateTime.Now;
            string contodate = "";
            todatecon = Convert.ToDateTime(data.Todate.Value.Date.ToString("yyyy-MM-dd"));
            contodate = todatecon.ToString("yyyy-MM-dd");


            if (data.Logintype != "" && data.Logintype == "KIOSK")
            {

                var mo_id = _db.Organisation.Where(t => t.MO_Id == data.MO_Id).FirstOrDefault();


                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Staff_Birthday_Report_kiosk";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@month",
                        SqlDbType.VarChar)
                    {
                        Value = Convert.ToString(data.months)
                    });

                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar)
                    {
                        Value = confromdate
                    });

                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar)
                    {
                        Value = contodate
                    });

                    cmd.Parameters.Add(new SqlParameter("@MO_Id", SqlDbType.BigInt)
                    {
                        Value = data.MO_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@all1", SqlDbType.Text)
                    {
                        Value = Convert.ToString(data.all1)
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();

                    try
                    {
                        // var data = cmd.ExecuteNonQuery();

                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? "N/A" : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.staffDetails = retObject.ToArray();
                        if (data.staffDetails.Length > 0)
                        {
                            data.count = data.staffDetails.Length;
                        }
                        else
                        {
                            data.count = 0;
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
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Staff_Birthday_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@month",
                        SqlDbType.VarChar)
                    {
                        Value = Convert.ToString(data.months)
                    });

                    cmd.Parameters.Add(new SqlParameter("@fromdate", SqlDbType.VarChar)
                    {
                        Value = confromdate
                    });

                    cmd.Parameters.Add(new SqlParameter("@todate", SqlDbType.VarChar)
                    {
                        Value = contodate
                    });

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.MI_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@all1", SqlDbType.Text)
                    {
                        Value = Convert.ToString(data.all1)
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();

                    try
                    {
                        // var data = cmd.ExecuteNonQuery();

                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? "N/A" : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.staffDetails = retObject.ToArray();
                        if (data.staffDetails.Length > 0)
                        {
                            data.count = data.staffDetails.Length;
                        }
                        else
                        {
                            data.count = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }


            return data;
        }




      





        public void SMS_Schedulers(int id) // scheduler for st.james
        {
            try
            {
                MI_ID = id;
                sendsmsforstudent(MI_ID);
                sendsmsforstaff(MI_ID);
                //sendsmsforAlumni(MI_ID);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
        }
        public string sendsmsforstudent(int MI_ID)
        {
            string re = "";
            long trnsno = 0;
            try
            {
                var acd_Id = _db.AcademicYear.Where(t => t.MI_Id.Equals(MI_ID) && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();

                var que1 = (from a in _db.Adm_M_Student
                            from b in _db.School_Adm_Y_StudentDMO
                            from c in _db.School_M_Class
                            from d in _db.School_M_Section
                            where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMAY_Id == acd_Id && d.ASMS_Id == b.ASMS_Id && a.MI_Id == MI_ID && a.AMST_ActiveFlag == 1 && a.AMST_SOL.Equals("S") && b.AMAY_ActiveFlag == 1 && a.AMST_DOB.Date.Day == DateTime.Now.Day && a.AMST_DOB.Date.Month == DateTime.Now.Month)
                            select new BirthDayDTO
                            {
                                AMST_Id = a.AMST_Id,
                                AMST_emailId = a.AMST_emailId,
                                AMST_MobileNo = a.AMST_MobileNo
                            }).Distinct().ToList();

                if (que1.Count > 0)
                {
                    for (int i = 0; i < que1.Count; i++)
                    {
                        try
                        {
                            long id = que1[i].AMST_Id;
                            string email = Convert.ToString(que1[i].AMST_emailId);
                            string mobileNo = Convert.ToString(que1[i].AMST_MobileNo);
                            string Template = "BIRTHDAY";
                            string type = "Student";
                            if (email != "" && email != null)
                            {
                                //string val = sendmail(MI_ID, email, Template, id, type);
                            }
                            if (mobileNo != "" && mobileNo != null)
                            {
                                string x = sendSms(MI_ID, mobileNo, Template, id, type);
                            }
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }

                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return re;
        }
        public string sendsmsforstaff(int MI_ID)
        {
            string ret = "";
            long trnsno = 0;
            try
            {

                var que2 = (from a in _db.HR_Master_Employee_DMO
                            from b in _db.Multiple_Email_DMO
                            from c in _db.Multiple_Mobile_DMO
                            where (a.HRME_Id == b.HRME_Id && a.HRME_Id == c.HRME_Id && b.HRMEM_DeFaultFlag.Equals("default") && c.HRMEMNO_DeFaultFlag.Equals("default") && a.MI_Id == MI_ID && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.HRME_DOB.Value.Date.Day == DateTime.Now.Day && a.HRME_DOB.Value.Date.Month == DateTime.Now.Month)
                            //group a by a.HRME_Id into g
                            select new BirthDayDTO
                            {
                                HRME_Id = a.HRME_Id,
                                HRME_EmailId = b.HRMEM_EmailId,
                                HRME_MobileNo = c.HRMEMNO_MobileNo
                            }).ToList();

                if (que2.Count > 0)
                {
                    SMS smstransno = new SMS(_db);

                    for (int i = 0; i < que2.Count; i++)
                    {
                        try
                        {
                            long id = que2[i].HRME_Id;
                            string email = Convert.ToString(que2[i].HRME_EmailId);
                            string mob = Convert.ToString(que2[i].HRME_MobileNo);
                            string Template = "STAFFBIRTHDAY";
                            string type = "Employee";
                            if (email != "" && email != null)
                            {
                                //string val = sendmail(MI_ID, email, Template, id, type);
                            }
                            if (mob != "" && mob != null)
                            {
                                string x = sendSms(MI_ID, mob, Template, id, type);
                            }
                        }
                        catch (Exception e)
                        {
                            continue;
                        }
                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return ret;
        }
        public string sendsmsforAlumni(int MI_ID)
        {
            string re = "";
            long trnsno = 0;
            try
            {
                var acd_Id = _db.AcademicYear.Where(t => t.MI_Id.Equals(MI_ID) && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();

                List<BirthDayDTO> studentlist = new List<BirthDayDTO>();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ALUMNI_BirthdayList_Day";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = MI_ID });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                studentlist.Add(new BirthDayDTO
                                {
                                    AMST_MobileNo = Convert.ToInt64(dataReader["amsT_MobileNo"]),
                                    studentName = (dataReader["studentName"]).ToString(),
                                    AMST_emailId = (dataReader["amsT_emailId"]).ToString(),
                                    AMST_Id = Convert.ToInt64(dataReader["AMST_Id"])
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                if (studentlist != null && studentlist.Count > 0)
                {
                    for (int i = 0; i < studentlist.Count; i++)
                    {
                        try
                        {
                            long id = studentlist[i].AMST_Id;
                            string email = Convert.ToString(studentlist[i].AMST_emailId);
                            string mobileNo = Convert.ToString(studentlist[i].AMST_MobileNo);
                            string Template = "BIRTHDAY";
                            string type = "Alumni";
                            if (email != "" && email != null)
                            {
                                //string val = sendmail(MI_ID, email, Template, id, type);
                            }
                            if (mobileNo != "" && mobileNo != null)
                            {
                                string x = sendSms(MI_ID, mobileNo, Template, id, type);
                            }
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return re;
        }
    }
}