using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.admission;
using AutoMapper;
using PreadmissionDTOs.com.vaps.admission;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;

using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DomainModel.Model.com.vapstech.TT;
using CommonLibrary;
using SendGrid.Helpers.Mail;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using SendGrid;
using System.Net;
using Newtonsoft.Json;
using System.Net.NetworkInformation;
using System.Web;


namespace AdmissionServiceHub.com.vaps.Services
{
    public class SMSGenaralImpl : Interfaces.SMSGenaralInterface
    {
        private readonly AdmissionFormContext _smsContext;
       
        public DomainModelMsSqlServerContext _db;
        public SMSGenaralImpl(AdmissionFormContext cpContext, DomainModelMsSqlServerContext db)
        {
            _smsContext = cpContext;
            _db = db;
        }
        public async Task<SMSGenaralDTO> Getdetails(SMSGenaralDTO data)//int IVRMM_Id
        {
            {
                try
                {
                    var list = await _smsContext.year.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).ToListAsync();//AcademicYear
                    data.yearlist = list.ToArray();

                    var currYear = await _smsContext.year.Where(d => d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id).ToListAsync();//AcademicYear
                    data.currentYear = currYear.ToArray();

                    var classlist = await _db.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToListAsync();
                    data.classlist = classlist.ToArray();

                    var sectionlist = await _db.School_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).ToListAsync();
                    data.sectionlist = sectionlist.ToArray();

                    var designationdropdown = await _smsContext.HR_Master_Designation.Where(t => t.MI_Id == data.MI_Id && t.HRMDES_ActiveFlag == true).ToListAsync();
                    data.designationdropdown = designationdropdown.ToArray();



                    var studentlist = await (from m in _db.Adm_M_Student
                                             from n in _db.School_Adm_Y_StudentDMO
                                             where m.AMST_Id == n.AMST_Id && m.MI_Id == data.MI_Id && n.ASMAY_Id == currYear.FirstOrDefault().ASMAY_Id && m.AMST_SOL.Equals("S") && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1 && n.ASMCL_Id == classlist.FirstOrDefault().ASMCL_Id && n.ASMS_Id == sectionlist.FirstOrDefault().ASMS_Id
                                             select new SMSGenaralDTO
                                             {
                                                 AMST_Id = n.AMST_Id,
                                                 studentName = m.AMST_FirstName + (string.IsNullOrEmpty(m.AMST_MiddleName) || m.AMST_MiddleName == "0" ? "" : ' ' + m.AMST_MiddleName) + (string.IsNullOrEmpty(m.AMST_LastName) || m.AMST_LastName == "0" ? "" : ' ' + m.AMST_LastName),
                                                 AMST_AdmNo = m.AMST_AdmNo,
                                                 AMST_emailId = m.AMST_emailId,
                                                 AMST_MobileNo = m.AMST_MobileNo

                                             }).ToListAsync();
                    if (studentlist.Count > 0)
                    {
                        data.studentlist = studentlist.ToArray();
                        data.studentCount = studentlist.Count;
                    }
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }

                return data;
            }

        }
        public SMSGenaralDTO Getexam(SMSGenaralDTO data)
        {
            try
            {
                var Cat_Id1 = _smsContext.Exm_Category_ClassDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMAY_Id == data.ASMAY_Id && t.ECAC_ActiveFlag == true).ToList();
                if (Cat_Id1.Count > 0)
                {
                    var Cat_Id = _smsContext.Exm_Category_ClassDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMAY_Id == data.ASMAY_Id && t.ECAC_ActiveFlag == true).Select(t => t.EMCA_Id).First();
                    var year_cat_id = _smsContext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.EYC_ActiveFlg == true && t.EMCA_Id == Cat_Id).Select(t => t.EYC_Id).First();

                    data.exmstdlist = (from a in _smsContext.masterexam
                                       from b in _smsContext.Exm_Yearly_Category_ExamsDMO
                                       where (a.MI_Id == data.MI_Id && a.EME_ActiveFlag == true && a.EME_Id == b.EME_Id && b.EYC_Id == year_cat_id)
                                       select a).Distinct().ToArray();
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }
        public SMSGenaralDTO GetEmployeeDetailsByLeaveYearAndMonth(SMSGenaralDTO data)
        {
            //List<MasterEmployee> employe = new List<MasterEmployee>();
            List<HR_Master_Employee_DMO> employe = new List<HR_Master_Employee_DMO>();

            try
            {
                employe = (from a in _smsContext.HR_Master_Employee_DMO//MasterEmployee

                           where (a.MI_Id.Equals(data.MI_Id)) && a.HRME_ActiveFlag == true
                           select a).Distinct().ToList();

                if (employe.Count > 0)
                {
                    employe = employe.Where(a => a.HRME_LeftFlag == false).ToList();


                    if (data.hrmdeS_IdList.Count() > 0 && data.hrmD_IdList.Count() > 0)
                    {
                        //employee
                        employe = employe.Where(t => t.MI_Id.Equals(data.MI_Id) && data.hrmdeS_IdList.Contains(t.HRMDES_Id) && data.hrmD_IdList.Contains(t.HRMD_Id)).ToList();

                    }
                    else if (data.hrmdeS_IdList.Count() > 0 && data.hrmD_IdList.Count() > 0)
                    {
                        //employee
                        employe = employe.Where(t => t.MI_Id.Equals(data.MI_Id) && data.hrmdeS_IdList.Contains(t.HRMDES_Id) && data.hrmD_IdList.Contains(t.HRMD_Id)).ToList();
                    }
                    else if (data.hrmdeS_IdList.Count() > 0 && data.hrmD_IdList.Count() == 0)
                    {
                        //employee
                        employe = employe.Where(t => t.MI_Id.Equals(data.MI_Id) && data.hrmdeS_IdList.Contains(t.HRMDES_Id)).ToList();
                    }
                    else if (data.hrmdeS_IdList.Count() == 0 && data.hrmD_IdList.Count() > 0)
                    {
                        //employee
                        employe = employe.Where(t => t.MI_Id.Equals(data.MI_Id) && data.hrmD_IdList.Contains(t.HRMD_Id)).ToList();
                    }
                    else if (data.hrmdeS_IdList.Count() > 0 && data.hrmD_IdList.Count() == 0)
                    {
                        //employee
                        employe = employe.Where(t => t.MI_Id.Equals(data.MI_Id) && data.hrmdeS_IdList.Contains(t.HRMDES_Id)).ToList();
                    }
                    else if (data.hrmdeS_IdList.Count() == 0 && data.hrmD_IdList.Count() > 0)
                    {
                        //employee
                        employe = employe.Where(t => t.MI_Id.Equals(data.MI_Id) && data.hrmD_IdList.Contains(t.HRMD_Id)).ToList();
                    }

                    else if (data.hrmdeS_IdList.Count() == 0 && data.hrmD_IdList.Count() == 0)
                    {
                        //employee
                        employe = employe.Where(t => t.MI_Id.Equals(data.MI_Id)).ToList();
                    }
                    data.employeedropdown = employe.ToArray();
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public async Task<SMSGenaralDTO> savedetail(SMSGenaralDTO data)
        {
            long trnsno = 0;
            SMS sms1 = new SMS(_db);
            trnsno = sms1.getsmsno(data.MI_Id);
            if (data.radiotype == "General")
            {
                //SmsWithoutTemplate sms = new SmsWithoutTemplate(_db);


                //string s = await sms.sendsmsfromPortal(data.MI_Id, Convert.ToInt64(data.Mobno), data.mes);
                if (data.otpapproveflag==true || data.approveflag==true)
                {
                    data.otpapproveflag = false;
                    data.approveflag = true;
                    string y = await sms1.saveapproval(data.MI_Id, data.template, data.Userid, trnsno, data.otpapproveflag, data.approveflag);
                    if (y.Equals("SETUSER"))
                    {
                        data.smsStatus = "user";
                    }
                    else if (y.Equals("success"))
                    {
                        string z = await sms1.saveSmsnewwithouttemplete_newtable(data.MI_Id, Convert.ToInt64(data.Mobno), data.template, data.Userid, data.mes, trnsno, "test", data.Userid);
                        if (z.Equals("Success"))
                        {
                            data.smsStatus = "approve";
                        }
                        else
                        {
                            data.smsStatus = "failed";
                        }
                    }
                    else
                    {
                        data.smsStatus = "failed";
                    }
                    
                }
                else
                {
                    string s = await sms1.sendSmsnewwithouttemplete_newtable(data.MI_Id, Convert.ToInt64(data.Mobno), data.template, data.Userid, data.mes, trnsno, "test", data.Userid);
                    if (s.Equals("Success"))
                    {
                        data.smsStatus = "sent";
                    }
                    else
                    {
                        data.smsStatus = "failed";
                    }
                }
                   
                
            }


            else if (data.radiotype == "Student")
            {
                if (data.studentlistdto.Length > 0)
                {
                    SmsWithoutTemplate sms = new SmsWithoutTemplate(_db);
                    EmailWithoutTemplate email = new EmailWithoutTemplate(_db);
                    string e = string.Empty;
                    string z = string.Empty;

                    if (data.otpapproveflag == true || data.approveflag == true)
                    {
                        data.otpapproveflag = false;
                        data.approveflag = true;


                        string y = await sms1.saveapproval(data.MI_Id, data.template, data.Userid, trnsno, data.otpapproveflag, data.approveflag);
                        if (y.Equals("SETUSER"))
                        {
                            data.smsStatus = "user";
                        }
                        else if (y.Equals("success"))
                        {

                            for (int i = 0; i < data.studentlistdto.Length; i++)
                            {
                                string emailtext = data.SmsMailText;
                                
                                    if (data.studentlistdto[i].AMST_MobileNo != 0)
                                    {
                                      
                                         z = await sms1.saveSmsnewwithouttemplete_newtable(data.MI_Id, data.studentlistdto[i].AMST_MobileNo, data.template, data.Userid, data.SmsMailText, trnsno, "test", data.Userid);
                                    }
                               
                                
                                    if (data.studentlistdto[i].AMST_emailId != "")

                                    {

                                    
                                        //e = Sendgenemail(data.MI_Id, data.studentlistdto[i].studentName, data.studentlistdto[i].AMST_emailId, emailtext);


                                   
                                }

                            }


                          
                            if (z.Equals("Success"))
                            {
                                data.smsStatus = "approve";
                            }
                            else
                            {
                                data.smsStatus = "failed";
                            }
                        }
                        else
                        {
                            data.smsStatus = "failed";
                        }
                    }
                    else
                    {
                        for (int i = 0; i < data.studentlistdto.Length; i++)
                        {
                            string emailtext = data.SmsMailText;
                            if (data.snd_sms == true)
                            {
                                if (data.studentlistdto[i].AMST_MobileNo != 0)
                                {
                                  
                                    e = await sms1.sendSmsnewwithouttemplete_newtable(data.MI_Id, Convert.ToInt64(data.studentlistdto[i].AMST_MobileNo), data.template, data.Userid, data.SmsMailText, trnsno, "test", data.Userid);
                                }
                            }
                            if (data.snd_email == true)
                            {
                                if (data.studentlistdto[i].AMST_emailId != "")

                                {

                                   
                                    e = Sendgenemail(data.MI_Id, data.studentlistdto[i].studentName, data.studentlistdto[i].AMST_emailId, emailtext);


                                }
                            }

                        }
                        if (e.Equals("Success"))
                        {
                            data.emailStatus = "sent";
                        }
                        else
                        {
                            data.emailStatus = "failed";
                        }
                    }
                   
                }
            }

            else if (data.radiotype == "Staff")
            {
                string z = string.Empty;
                if (data.otpapproveflag == true || data.approveflag == true)
                {
                    data.otpapproveflag = false;
                    data.approveflag = true;


                    string y = await sms1.saveapproval(data.MI_Id, data.template, data.Userid, trnsno, data.otpapproveflag, data.approveflag);
                    if (y.Equals("SETUSER"))
                    {
                        data.smsStatus = "user";
                    }
                    else if (y.Equals("success"))
                    {

                        for (int i = 0; i < data.studentlistdto.Length; i++)
                        {
                            string emailtext = data.SmsMailText;
                           
                                if (data.studentlistdto[i].HRME_MobileNo != 0)
                                {
                                   ;
                                    z = await sms1.saveSmsnewwithouttemplete_newtable(data.MI_Id, Convert.ToInt64(data.studentlistdto[i].HRME_MobileNo), data.template, data.Userid, data.SmsMailText, trnsno, "test", data.Userid);
                                }


                            if (data.studentlistdto[i].hrm_email != "")

                            {

                                
                                //e = Sendgenemail(data.MI_Id, data.studentlistdto[i].studentName, data.studentlistdto[i].hrm_email, emailtext);


                            }

                        }



                        if (z.Equals("Success"))
                        {
                            data.smsStatus = "approve";
                        }
                        else
                        {
                            data.smsStatus = "failed";
                        }
                    }
                    else
                    {
                        data.smsStatus = "failed";
                    }
                }
                else
                {
                    string s = string.Empty;
                    for (int i = 0; i < data.studentlistdto.Length; i++)
                    {
                        string stfemltext = string.Empty;
                        stfemltext = data.SmsMailText;
                        if (data.stfsnd_sms == true)
                        {
                           
                            if (data.studentlistdto[i].HRME_MobileNo != 0)
                            {
                                //data.SmsMailText = data.studentlistdto[i].HRME_EmployeeFirstName + "," + data.SmsMailText;
                                data.SmsMailText = "" + data.SmsMailText;

                                s = await sms1.sendSmsnewwithouttemplete_newtable(data.MI_Id, Convert.ToInt64(data.studentlistdto[i].HRME_MobileNo), data.template, data.Userid, data.SmsMailText, trnsno, "Test", data.Userid);


                            }
                        }
                        if (data.stfsnd_email == true)
                        {
                            if (data.studentlistdto[i].hrm_email != "")
                            {
                              
                                s = Sendgenemail(data.MI_Id, data.studentlistdto[i].HRME_EmployeeFirstName, data.studentlistdto[i].hrm_email, stfemltext);
                            }
                        }

                    }
                    if (s.Equals("Success"))
                    {
                        data.smsStatus = "sent";
                    }
                    else
                    {
                        data.smsStatus = "failed";
                    }
                }
                

            }
            return data;
        }
        public string Sendgenemail(long mi_id, string name, string emailid, string msg)
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
                    message.From = new EmailAddress(SendingEmail, institutionName.FirstOrDefault().MI_Name);
                    message.Subject = Subject;
                    message.AddTo(emailid);
                    string body = "<div>" + msg + "</div>";
                    string footer = "<br />" + " Thanks and Regards" + "<br />" + "<div>" + institutionName.FirstOrDefault().MI_Name + "</div>";
                    message.HtmlContent = body + footer;
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
                            Value = "Admission"
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
        public async Task<string> sendgeneralsms(long MI_Id, long mobileNo, string message)
        {

            try
            {
                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();
                Dictionary<string, string> val = new Dictionary<string, string>();

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                string url = alldetails[0].IVRMSD_URL.ToString();

                string PHNO = mobileNo.ToString();

                url = url.Replace("PHNO", PHNO);

                url = url.Replace("MESSAGE", message);

                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                Stream stream = response.GetResponseStream();

                StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                string responseparameters = readStream.ReadToEnd();

                if (responseparameters != null)
                {
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
                            Value = message
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = "Admission"
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
                            Value = responseparameters
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
            catch (Exception e)
            {
                return e.Message;
            }
            return "Success";
        }
        public async Task<SMSGenaralDTO> GetStudentDetails(SMSGenaralDTO data)
        {
            try
            {
                if (data.ASMS_Id == 0)
                {
                    var studentlist = await (from m in _db.Adm_M_Student
                                             from n in _db.School_Adm_Y_StudentDMO
                                             where m.AMST_Id == n.AMST_Id && m.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && m.AMST_SOL.Equals("S") && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1 && n.ASMCL_Id == data.ASMCL_Id
                                             //&& n.ASMS_Id == data.ASMS_Id
                                             select new SMSGenaralDTO
                                             {
                                                 AMST_Id = n.AMST_Id,
                                                 studentName = m.AMST_FirstName + (string.IsNullOrEmpty(m.AMST_MiddleName) || m.AMST_MiddleName == "0" ? "" : ' ' + m.AMST_MiddleName) + (string.IsNullOrEmpty(m.AMST_LastName) || m.AMST_LastName == "0" ? "" : ' ' + m.AMST_LastName),
                                                 AMST_AdmNo = m.AMST_AdmNo,
                                                 AMST_emailId = m.AMST_emailId,
                                                 AMST_MobileNo = m.AMST_MobileNo

                                             }).Distinct().ToListAsync();
                    if (studentlist.Count > 0)
                    {
                        data.studentlist = studentlist.ToArray();
                        data.studentCount = studentlist.Count;
                    }
                }
                else
                {
                    var studentlist = await (from m in _db.Adm_M_Student
                                             from n in _db.School_Adm_Y_StudentDMO
                                             where m.AMST_Id == n.AMST_Id && m.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && m.AMST_SOL.Equals("S") && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1 && n.ASMCL_Id == data.ASMCL_Id && n.ASMS_Id == data.ASMS_Id
                                             select new SMSGenaralDTO
                                             {
                                                 AMST_Id = n.AMST_Id,
                                                 studentName = m.AMST_FirstName + (string.IsNullOrEmpty(m.AMST_MiddleName) || m.AMST_MiddleName == "0" ? "" : ' ' + m.AMST_MiddleName) + (string.IsNullOrEmpty(m.AMST_LastName) || m.AMST_LastName == "0" ? "" : ' ' + m.AMST_LastName),
                                                 AMST_AdmNo = m.AMST_AdmNo,
                                                 AMST_emailId = m.AMST_emailId,
                                                 AMST_MobileNo = m.AMST_MobileNo

                                             }).Distinct().ToListAsync();
                    if (studentlist.Count > 0)
                    {
                        data.studentlist = studentlist.ToArray();
                        data.studentCount = studentlist.Count;
                    }
                }



                if (data.selradioval == "exam")
                {
                    List<SMSGenaralDTO> result1 = new List<SMSGenaralDTO>();
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Exam_Send_generalSMS";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                         SqlDbType.BigInt)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                          SqlDbType.BigInt)
                        {
                            Value = data.ASMCL_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.ASMS_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@EME_Id",
                       SqlDbType.BigInt)
                        {
                            Value = data.EME_Id
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
                                    result1.Add(new SMSGenaralDTO
                                    {
                                        AMST_Id = Convert.ToInt64(dataReader["AMST_Id"].ToString()),
                                        studentName = dataReader["AMST_Name"].ToString(),
                                        AMST_AdmNo = dataReader["AMST_AdmNo"].ToString(),
                                        AMST_emailId = dataReader["AMST_emailId"].ToString(),
                                        AMST_MobileNo = Convert.ToInt64(dataReader["AMST_MobileNo"].ToString()),
                                        MarksDetails = dataReader["MarksDetails"].ToString(),
                                        GradeDetails = dataReader["GradeDetails"].ToString(),
                                        TotalMarks = dataReader["TotalMarks"].ToString(),
                                        TotalGrade = dataReader["TotalGrade"].ToString(),
                                        result = dataReader["result"].ToString(),
                                        ESTMPS_PassFailFlg = dataReader["ESTMPS_PassFailFlg"].ToString(),



                                    });
                                    data.stumarkdetails = result1.ToArray();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }



                }

                if (data.selradioval == "attendance")
                {

                    if (data.attend_radioval == "current")
                    {
                        data.fr_date = data.crnt_date;
                        data.to_date = data.crnt_date;
                    }



                    List<SMSGenaralDTO> result = new List<SMSGenaralDTO>();
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "SMS_StudentAttendance_P";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                          SqlDbType.VarChar)
                        {
                            Value = data.MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                     SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                            SqlDbType.VarChar)
                        {
                            Value = data.ASMCL_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                            SqlDbType.VarChar)
                        {
                            Value = data.ASMS_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@from",
                       SqlDbType.Date)
                        {
                            Value = data.fr_date
                        });
                        cmd.Parameters.Add(new SqlParameter("@to",
                       SqlDbType.Date)
                        {
                            Value = data.to_date
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
                                    result.Add(new SMSGenaralDTO
                                    {
                                        AMST_Id = Convert.ToInt64(dataReader["AMST_Id"].ToString()),
                                        totalpresentday = (dataReader["TotalPresentDays"].ToString()),
                                        totalworkingday = (dataReader["TotalSchoolDays"].ToString()),
                                        attper = (dataReader["Total_Percentage"].ToString()),
                                    });
                                }
                            }
                            data.attdetails = result.ToArray();

                        }
                        catch (Exception ex)
                        {
                            throw ex;

                        }
                    }

                }


            }
            catch (Exception)
            {

                throw;
            }

            return data;
        }
        public SMSGenaralDTO Getdepartment(SMSGenaralDTO data)
        {
            var departmentdropdown = _smsContext.HR_Master_Department.Where(t => t.MI_Id == data.MI_Id && t.HRMD_ActiveFlag == true).ToList();
            data.departmentdropdown = departmentdropdown.ToArray();
            return data;
        }
        public SMSGenaralDTO get_designation(SMSGenaralDTO data)
        {
            data.designationdropdown = (from a in _smsContext.HR_Master_Employee_DMO//MasterEmployee
                                        from b in _smsContext.HR_Master_Designation
                                        from c in _smsContext.HR_Master_Department
                                        where (a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id == c.HRMD_Id && c.HRMD_ActiveFlag == true
                                        && b.HRMDES_ActiveFlag == true && a.HRME_ActiveFlag == true
                                        && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && data.multipledep.ToString().Contains(Convert.ToString(c.HRMD_Id)))
                                        select new SMSGenaralDTO
                                        {
                                            HRMDES_Id = b.HRMDES_Id,
                                            HRMDES_DesignationName = b.HRMDES_DesignationName,
                                        }
                     ).Distinct().ToArray();

            return data;
        }
        public SMSGenaralDTO get_employee(SMSGenaralDTO data)
        {
            data.stafflist = (from a in _smsContext.HR_Master_Employee_DMO
                              from b in _smsContext.Multiple_Mobile_DMO
                              from c in _smsContext.Multiple_Email_DMO//MasterEmployee
                              where (a.MI_Id == data.MI_Id && a.HRME_Id == b.HRME_Id && a.HRME_Id == c.HRME_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && b.HRMEMNO_DeFaultFlag == "default" && c.HRMEM_DeFaultFlag == "default" && data.multipledes.ToString().Contains(Convert.ToString(a.HRMDES_Id)) && data.multipledep.ToString().Contains(Convert.ToString(a.HRMD_Id)))
                              select new SMSGenaralDTO
                              {
                                  HRME_Id = a.HRME_Id,
                                  HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                  HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName ?? " ",
                                  HRME_EmployeeLastName = a.HRME_EmployeeLastName ?? " ",
                                  HRME_MobileNo = b.HRMEMNO_MobileNo,
                                  hrm_email = c.HRMEM_EmailId
                              }
                     ).Distinct().ToArray();
            return data;
        }

       



    }

 
}
