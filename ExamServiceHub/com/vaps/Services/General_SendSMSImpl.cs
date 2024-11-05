using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.Exam;
using AutoMapper;
using PreadmissionDTOs.com.vaps.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.com.vaps.admission;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CommonLibrary;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Mail;
using SendGrid;
using SendGrid.Helpers.Mail;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Dynamic;

namespace ExamServiceHub.com.vaps.Services
{
    public class General_SendSMSImpl : Interfaces.General_SendSMSInterface
    {
        private static ConcurrentDictionary<string, ExamCalculation_SSSEDTO> _login =
         new ConcurrentDictionary<string, ExamCalculation_SSSEDTO>();

        private readonly ExamContext _smsContext;
        ILogger<ExamCalculation_SSSEImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public General_SendSMSImpl(ExamContext cpContext, DomainModelMsSqlServerContext db, ILogger<ExamCalculation_SSSEImpl> _acdi)
        {
            _smsContext = cpContext;
            _db = db;
            _acdimpl = _acdi;
        }
        public async Task<General_SendSMSDTO> Getdetails(General_SendSMSDTO data)//int IVRMM_Id
        {
            {
                try
                {
                    //data.templatelist = (from a in _smsContext.SMSEmailSetting
                    //                     where a.MI_Id == data.MI_Id && a.ISES_SMSActiveFlag == true
                    //                     select new General_SendSMSDTO
                    //                     {
                    //                         ISES_Id = a.ISES_Id,
                    //                         ISES_Template_Name = a.ISES_Template_Name,
                    //                     }).Distinct().OrderBy(e => e.ISES_Template_Name).ToArray();

                    data.templatelist = (from a in _smsContext.SMSEmailSetting
                                         from b in _smsContext.Institution_Module_Page
                                         from c in _smsContext.MasterPage
                                         where a.MI_Id == data.MI_Id && a.IVRMIMP_Id == b.IVRMIMP_Id && b.IVRMP_Id == c.IVRMP_Id
                                         && c.IVRMP_PageURL == "app.GeneralSendSMS"
                                         select new General_SendSMSDTO
                                         {
                                             ISES_Id = a.ISES_Id,
                                             ISES_Template_Name = a.ISES_Template_Name,
                                         }).Distinct().OrderBy(e => e.ISES_Template_Name).ToArray();


                    var list = await _smsContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id
                    && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToListAsync();//AcademicYear
                    data.yearlist = list.ToArray();

                    var currYear = await _smsContext.AcademicYear.Where(d => d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id).ToListAsync();//AcademicYear
                    data.currentYear = currYear.ToArray();

                    var classlist = await _db.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToListAsync();
                    data.classlist = classlist.ToArray();

                    var sectionlist = await _db.School_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).ToListAsync();
                    data.sectionlist = sectionlist.ToArray();

                    var designationdropdown = await _smsContext.HR_Master_Designation.Where(t => t.MI_Id == data.MI_Id && t.HRMDES_ActiveFlag == true).ToListAsync();
                    data.designationdropdown = designationdropdown.ToArray();

                    data.routelist = _db.MasterRouteDMO.Where(t => t.MI_Id == data.MI_Id).ToArray();

                    data.studentroutelist = (from a in _db.TR_Student_RouteDMO
                                             from b in _db.Adm_M_Student
                                             where ( a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                             select new General_SendSMSDTO
                                             {
                                                 TRMR_Id = a.TRMR_Id,
                                                 AMST_Id = b.AMST_Id,
                                                 studentName = b.AMST_FirstName + (string.IsNullOrEmpty(b.AMST_MiddleName) || b.AMST_MiddleName == "0" ? "" : ' ' + b.AMST_MiddleName) + (string.IsNullOrEmpty(b.AMST_LastName) || b.AMST_LastName == "0" ? "" : ' ' + b.AMST_LastName),
                                                 AMST_AdmNo = b.AMST_AdmNo,
                                                 AMST_MobileNo = b.AMST_MobileNo,
                                             }).ToArray();

                    var studentlist = await (from m in _db.Adm_M_Student
                                             from n in _db.School_Adm_Y_StudentDMO
                                             where m.AMST_Id == n.AMST_Id && m.MI_Id == data.MI_Id && n.ASMAY_Id == currYear.FirstOrDefault().ASMAY_Id && m.AMST_SOL.Equals("S") && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1 && n.ASMCL_Id == classlist.FirstOrDefault().ASMCL_Id && n.ASMS_Id == sectionlist.FirstOrDefault().ASMS_Id
                                             select new General_SendSMSDTO
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

        public async Task<General_SendSMSDTO> SrkvsSerach(General_SendSMSDTO data)
        {
            try
            {
                //stfsnd_email
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ADM_General_SMS_SRKVS";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@GRADE", SqlDbType.VarChar) { Value = data.TotalGrade });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                {
                                    dataRow1.Add(dataReader.GetName(iFiled1), dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1]);
                                }
                                retObject.Add((ExpandoObject)dataRow1);
                            }
                        }
                        data.studentlist = retObject.ToArray();
                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }


            }
            catch (Exception)
            {

                throw;
            }

            return data;
        }
        public General_SendSMSDTO Getexam(General_SendSMSDTO data)
        {
            try
            {
                List<long> classid = new List<long>();
                List<long> sectionid = new List<long>();

                List<AdmissionClass> getclassid = new List<AdmissionClass>();
                List<School_M_Section> getsectionid = new List<School_M_Section>();

                if (data.ASMCL_Id > 0)
                {
                    getclassid = _smsContext.AdmissionClass.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true && a.ASMCL_Id == data.ASMCL_Id).ToList();
                    for (int k = 0; k < getclassid.Count(); k++)
                    {
                        classid.Add(getclassid[k].ASMCL_Id);
                    }
                }
                else
                {
                    getclassid = _smsContext.AdmissionClass.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true).ToList();
                    for (int k = 0; k < getclassid.Count(); k++)
                    {
                        classid.Add(getclassid[k].ASMCL_Id);
                    }
                }

                if (data.ASMS_Id > 0)
                {

                    getsectionid = _smsContext.School_M_Section.Where(a => a.MI_Id == data.MI_Id && a.ASMC_ActiveFlag == 1 && a.ASMS_Id == data.ASMS_Id).ToList();
                    for (int k = 0; k < getsectionid.Count(); k++)
                    {
                        sectionid.Add(getsectionid[k].ASMS_Id);
                    }
                }
                else
                {
                    getsectionid = _smsContext.School_M_Section.Where(a => a.MI_Id == data.MI_Id && a.ASMC_ActiveFlag == 1).ToList();
                    for (int k = 0; k < getsectionid.Count(); k++)
                    {
                        sectionid.Add(getsectionid[k].ASMS_Id);
                    }
                }

                var Cat_Id1 = _smsContext.Exm_Category_ClassDMO.Where(t => t.MI_Id == data.MI_Id && classid.Contains(t.ASMCL_Id) && t.ASMAY_Id == data.ASMAY_Id
                && t.ECAC_ActiveFlag == true && sectionid.Contains(t.ASMS_Id)).ToList();

                if (Cat_Id1.Count > 0)
                {
                    var Cat_Id = _smsContext.Exm_Category_ClassDMO.Where(t => t.MI_Id == data.MI_Id && classid.Contains(t.ASMCL_Id) && t.ASMAY_Id == data.ASMAY_Id
                    && t.ECAC_ActiveFlag == true && sectionid.Contains(t.ASMS_Id)).Select(t => t.EMCA_Id);


                    var year_cat_id = _smsContext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.EYC_ActiveFlg == true
                    && Cat_Id.Contains(t.EMCA_Id)).Select(t => t.EYC_Id);

                    data.exmstdlist = (from a in _smsContext.masterexam
                                       from b in _smsContext.Exm_Yearly_Category_ExamsDMO
                                       where (a.MI_Id == data.MI_Id && a.EME_ActiveFlag == true && a.EME_Id == b.EME_Id &&  year_cat_id.Contains(b.EYC_Id))
                                       select a).Distinct().ToArray();
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }
        public General_SendSMSDTO GetEmployeeDetailsByLeaveYearAndMonth(General_SendSMSDTO data)
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
        public async Task<General_SendSMSDTO> savedetail(General_SendSMSDTO data)
        {
            if (data.radiotype == "General")
            {
                SmsWithoutTemplate sms = new SmsWithoutTemplate(_db);
                {
                    if (data.mobilenolist.Length>0)
                    {
                        foreach (var item in data.mobilenolist)
                        {
                            if (data.sms_flag == true)
                            {
                                string s = await sms.sendsmsfromPortal(data.MI_Id, Convert.ToInt64(item.hrmemnO_MobileNo), data.mes);
                            }
                            if (data.whatsapp_flag == true)
                            {
                                string s = await sms.SendwhatsappfromPortal(data.MI_Id, Convert.ToInt64(item.hrmemnO_MobileNo), data.mes, data.fileattachementforwhatsapp , data.whatsapp_filetype);
                            }
                        }
                    }

                    data.smsStatus = "sent";

                    //if (s.Equals("Success"))
                    //{
                    //    data.smsStatus = "sent";
                    //}
                    //else
                    //{
                    //    data.smsStatus = "failed";
                    //}
                }
            }

            else if (data.radiotype == "Gemail")
            {
                EmailWithoutTemplate email = new EmailWithoutTemplate(_db);
                {

                    foreach (var item in data.emaillist)
                    {
                        string s = sendmailWT(data.MI_Id,item.hrmeM_EmailId,data.mes,7,data.esubject,data.footer,data.filelist,data.Header, "",data.atchflag,data.fhead);

                    }
                }

                data.smsStatus = "sent";



            }

            else if (data.radiotype == "Student")
            {

                if (data.selradioval == "StdTemplate")
                {
                    if (data.studentlistdto.Length > 0)
                    {

                        var templatelist = _smsContext.SMSEmailSetting.Where(e => e.MI_Id == data.MI_Id && e.ISES_Id == data.ISES_Id).ToList();

                        if (templatelist.Count>0)
                        {
                            SmsWithoutTemplate sms = new SmsWithoutTemplate(_db);
                            EmailWithoutTemplate email = new EmailWithoutTemplate(_db);
                            string e = string.Empty;
                            for (int i = 0; i < data.studentlistdto.Length; i++)
                            {
                                if (data.snd_sms == true)
                                {
                                    if (data.studentlistdto[i].AMST_MobileNo != 0)
                                    {
                                        e = await templatesendsms(data.MI_Id, data.studentlistdto[i].AMST_MobileNo, templatelist[0].ISES_Template_Name.Trim(), data.studentlistdto[i].AMST_Id, data.ASMAY_Id, data.User_Id, data.SmsMailText);
                                    }
                                }
                                if (data.snd_email == true)
                                {
                                    if (data.studentlistdto[i].AMST_emailId != "")
                                    {
                                        e = templatesendmail(data.MI_Id, data.studentlistdto[i].AMST_emailId, templatelist[0].ISES_Template_Name.Trim(), data.studentlistdto[i].AMST_Id,data.ASMAY_Id,data.User_Id, data.SmsMailText);
                                    }
                                }

                            }
                            if (e.Equals("success") || e.Equals("Success"))
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
                else
                {
                    if (data.studentlistdto.Length > 0)
                    {
                        SmsWithoutTemplate sms = new SmsWithoutTemplate(_db);
                        EmailWithoutTemplate email = new EmailWithoutTemplate(_db);
                        string e = string.Empty;
                        for (int i = 0; i < data.studentlistdto.Length; i++)
                        {
                            string emailtext = data.SmsMailText;
                            if (data.selradioval == "exam" && data.exm_radioval == "mark")
                            {
                                var AMST_Id = data.studentlistdto[i].AMST_Id.ToString();
                                var MobileNo = data.studentlistdto[i].AMST_MobileNo;
                                var Email = data.studentlistdto[i].AMST_emailId;

                                if (data.snd_sms == true)
                                {
                                    if (MobileNo != 0)
                                    {
                                        e = await sendexamgeneralsms(data.MI_Id, MobileNo, "Exam_SMS_Marks_Details", AMST_Id, data.ASMAY_Id.ToString(), data.ASMCL_Id.ToString(), data.ASMS_Id.ToString(), data.EME_Id.ToString());
                                    }
                                }
                                if (data.snd_email == true)
                                {
                                    if (data.studentlistdto[i].AMST_emailId != "")
                                    {
                                        e = await sendexamgeneralemail(data.MI_Id, MobileNo, "Exam_SMS_Marks_Details", AMST_Id, data.ASMAY_Id.ToString(), data.ASMCL_Id.ToString(), data.ASMS_Id.ToString(), data.EME_Id.ToString(), Email);

                                    }
                                }
                            }

                            else if (data.selradioval == "exam" && data.exm_radioval == "grade")
                            {
                                var AMST_Id = data.studentlistdto[i].AMST_Id.ToString();
                                var MobileNo = data.studentlistdto[i].AMST_MobileNo;
                                var Email = data.studentlistdto[i].AMST_emailId;

                                if (data.snd_sms == true)
                                {
                                    if (MobileNo != 0)
                                    {
                                        e = await sendexamgeneralgradesms(data.MI_Id, MobileNo, "Exam_SMS_Grade_Details", AMST_Id, data.ASMAY_Id.ToString(), data.ASMCL_Id.ToString(), data.ASMS_Id.ToString(), data.EME_Id.ToString());
                                    }
                                }
                                if (data.snd_email == true)
                                {
                                    if (data.studentlistdto[i].AMST_emailId != "")
                                    {
                                        e = await sendexamgeneralgradeemail(data.MI_Id, MobileNo, "Exam_SMS_Grade_Details", AMST_Id, data.ASMAY_Id.ToString(), data.ASMCL_Id.ToString(), data.ASMS_Id.ToString(), data.EME_Id.ToString(), Email);

                                    }
                                }
                            }

                            else if (data.selradioval == "attendance")
                            {
                                var AMST_Id = data.studentlistdto[i].AMST_Id.ToString();
                                var MobileNo = data.studentlistdto[i].AMST_MobileNo;
                                var Email = data.studentlistdto[i].AMST_emailId;

                                DateTime fromdatecon = DateTime.Now;
                                string confromdate = "";
                                try
                                {
                                    fromdatecon = Convert.ToDateTime(data.fr_date.Value.ToString("yyyy-MM-dd"));
                                    confromdate = fromdatecon.ToString("yyyy-MM-dd");
                                }

                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                string contodate = "";
                                try
                                {
                                    fromdatecon = Convert.ToDateTime(data.to_date.Value.ToString("yyyy-MM-dd"));
                                    contodate = fromdatecon.ToString("yyyy-MM-dd");
                                }

                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                                string condate = "";
                                try
                                {
                                    fromdatecon = Convert.ToDateTime(data.crnt_date.Value.ToString("yyyy-MM-dd"));
                                    condate = fromdatecon.ToString("yyyy-MM-dd");
                                }

                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }


                                if (data.snd_sms == true)
                                {
                                    if (MobileNo != 0)
                                    {
                                        e = await sendgeneralattendancesms(data.MI_Id, MobileNo, "General_SMS_Attendance_Details", AMST_Id, data.ASMAY_Id.ToString(), data.ASMCL_Id.ToString(), data.ASMS_Id.ToString(), confromdate, contodate, condate, data.attend_radioval);
                                    }
                                }
                                if (data.snd_email == true)
                                {
                                    if (data.studentlistdto[i].AMST_emailId != "")
                                    {
                                        e = await sendgeneralattendanceemail(data.MI_Id, MobileNo, "General_SMS_Attendance_Details", AMST_Id, data.ASMAY_Id.ToString(), data.ASMCL_Id.ToString(), data.ASMS_Id.ToString(), confromdate, contodate, condate, data.attend_radioval, Email);

                                    }
                                }
                            }
                            else
                            {
                                if (data.snd_sms == true)
                                {
                                    if (data.studentlistdto[i].AMST_MobileNo != 0)
                                    {
                                        e = await sendgeneralsms(data.MI_Id, Convert.ToInt64(data.studentlistdto[i].AMST_MobileNo), data.SmsMailText);
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
                        }
                        if (e.Equals("success") || e.Equals("Success"))
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
                string s = string.Empty;
                for (int i = 0; i < data.studentlistdto.Length; i++)
                {
                    string stfemltext = string.Empty;
                    stfemltext = data.SmsMailText;
                    if (data.stfsnd_sms == true)
                    {
                        ;
                        if (data.studentlistdto[i].HRME_MobileNo != 0)
                        {

                            data.SmsMailText = "" + data.SmsMailText;

                            s = await sendgeneralsms(data.MI_Id, Convert.ToInt64(data.studentlistdto[i].HRME_MobileNo), data.SmsMailText);


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
            else if (data.radiotype == "SRkvs")
            {
                if (data.studentlistdto != null)
                {
                  
                    data.smsStatus = "sent";
                    //added
                    if (data.studentlistdto.Length > 0)
                    {
                        SmsWithoutTemplate sms = new SmsWithoutTemplate(_db);
                        EmailWithoutTemplate email = new EmailWithoutTemplate(_db);
                        string e = string.Empty;
                        for (int i = 0; i < data.studentlistdto.Length; i++)
                        {
                            string emailtext = "Hi";
                            var AMST_Id = data.studentlistdto[i].AMST_Id.ToString();
                            var MobileNo = data.studentlistdto[i].AMST_MobileNo;
                            var Email = data.studentlistdto[i].AMST_emailId;
                            string mes = data.studentlistdto[i].mes;
                            string SmsMailText = data.studentlistdto[i].SmsMailText;
                            if (MobileNo != 0 && data.snd_sms == true)
                            {
                                e = await sendexamgeneralSRKVS(data.MI_Id, MobileNo, "Exam_SMS_Marks_Details", AMST_Id, data.ASMAY_Id.ToString(), data.ASMCL_Id.ToString(), data.ASMS_Id.ToString(), data.EME_Id.ToString(), mes);
                            }
                            if (data.snd_email == true && Email !="")
                            {
                                e = await sendexamgeneralemailSRKVS(data.MI_Id, MobileNo, "Exam_SMS_Marks_Details", AMST_Id, data.ASMAY_Id.ToString(), data.ASMCL_Id.ToString(), data.ASMS_Id.ToString(), data.EME_Id.ToString(), Email, SmsMailText);

                            }
                           
                        }

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
                message = message.Replace("&", "%26");
                message = message.Replace("#", "%23");
                message = message.Replace("'", "%27");
                message = message.Replace("$", "%24");
                message = message.Replace("+", "%2B");
                message = message.Replace("@", "%40");
                message = message.Replace("?", "%3F");
                message = message.Replace(">", "%3E");
                message = message.Replace("=", "%3D");
                message = message.Replace(";", "%3B");
                message = message.Replace("<", "%3C");

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
        public async Task<General_SendSMSDTO> GetStudentDetails(General_SendSMSDTO data)
        {
            try
            {


                //data.templatelist = (from a in _smsContext.SMSEmailSetting
                //                     where a.MI_Id == data.MI_Id && a.ISES_SMSActiveFlag == true
                //                     select new General_SendSMSDTO
                //                     {
                //                         ISES_Id = a.ISES_Id,
                //                         ISES_Template_Name = a.ISES_Template_Name,
                //                     }).Distinct().OrderBy(e => e.ISES_Template_Name).ToArray();

                data.templatelist = (from a in _smsContext.SMSEmailSetting
                                     from b in _smsContext.Institution_Module_Page
                                     from c in _smsContext.MasterPage
                                     where a.MI_Id == data.MI_Id && a.IVRMIMP_Id == b.IVRMIMP_Id && b.IVRMP_Id == c.IVRMP_Id && c.IVRMP_PageURL == "app.GeneralSendSMS"
                                     select new General_SendSMSDTO
                                     {
                                         ISES_Id = a.ISES_Id,
                                         ISES_Template_Name = a.ISES_Template_Name,
                                     }).Distinct().OrderBy(e => e.ISES_Template_Name).ToArray();

                if (data.neworregular == "New")
                {
                    if (data.ASMCL_Id == 0)
                    {
                        var studentlist = await (from m in _db.Adm_M_Student
                                                 where m.MI_Id == data.MI_Id && m.ASMAY_Id == data.ASMAY_Id && m.AMST_SOL.Equals("S")
                                                 && m.AMST_ActiveFlag == 1
                                                 select new General_SendSMSDTO
                                                 {
                                                     AMST_Id = m.AMST_Id,
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
                                                 where m.MI_Id == data.MI_Id && m.ASMAY_Id == data.ASMAY_Id && m.AMST_SOL.Equals("S")
                                                 && m.AMST_ActiveFlag == 1 && m.ASMCL_Id == data.ASMCL_Id
                                                 select new General_SendSMSDTO
                                                 {
                                                     AMST_Id = m.AMST_Id,
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
                }
                else
                {
                    if (data.ASMCL_Id == 0)
                    {
                        if (data.ASMS_Id == 0)
                        {
                            var studentlist = await (from m in _db.Adm_M_Student
                                                     from n in _db.School_Adm_Y_StudentDMO
                                                     where m.AMST_Id == n.AMST_Id && m.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && m.AMST_SOL.Equals("S")
                                                     && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1
                                                     //&& n.ASMS_Id == data.ASMS_Id && n.ASMCL_Id == data.ASMCL_Id
                                                     select new General_SendSMSDTO
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
                                                     where m.AMST_Id == n.AMST_Id && m.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && m.AMST_SOL.Equals("S")
                                                     && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1 && n.ASMS_Id == data.ASMS_Id
                                                     //&& n.ASMCL_Id == data.ASMCL_Id
                                                     select new General_SendSMSDTO
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
                    }

                    else
                    {
                        if (data.ASMS_Id == 0)
                        {
                            var studentlist = await (from m in _db.Adm_M_Student
                                                     from n in _db.School_Adm_Y_StudentDMO
                                                     where m.AMST_Id == n.AMST_Id && m.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && m.AMST_SOL.Equals("S")
                                                     && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1 && n.ASMCL_Id == data.ASMCL_Id
                                                     //&& n.ASMS_Id == data.ASMS_Id 
                                                     select new General_SendSMSDTO
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
                                                     where m.AMST_Id == n.AMST_Id && m.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && m.AMST_SOL.Equals("S")
                                                     && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1 && n.ASMS_Id == data.ASMS_Id && n.ASMCL_Id == data.ASMCL_Id
                                                     select new General_SendSMSDTO
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
                    }
                }

                if (data.selradioval == "exam")
                {
                    List<General_SendSMSDTO> result1 = new List<General_SendSMSDTO>();
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
                                    result1.Add(new General_SendSMSDTO
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



                    List<General_SendSMSDTO> result = new List<General_SendSMSDTO>();
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
                                    result.Add(new General_SendSMSDTO
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
                            _acdimpl.LogError(ex.Message);
                            _acdimpl.LogDebug(ex.Message);
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
        public General_SendSMSDTO Getdepartment(General_SendSMSDTO data)
        {
            var departmentdropdown = _smsContext.HR_Master_Department.Where(t => t.MI_Id == data.MI_Id && t.HRMD_ActiveFlag == true).ToList();
            data.departmentdropdown = departmentdropdown.ToArray();
            return data;
        }
        public General_SendSMSDTO get_designation(General_SendSMSDTO data)
        {
            data.designationdropdown = (from a in _smsContext.HR_Master_Employee_DMO//MasterEmployee
                                        from b in _smsContext.HR_Master_Designation
                                        from c in _smsContext.HR_Master_Department
                                        where (a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id == c.HRMD_Id && c.HRMD_ActiveFlag == true
                                        && b.HRMDES_ActiveFlag == true && a.HRME_ActiveFlag == true
                                        && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && data.multipledep.ToString().Contains(Convert.ToString(c.HRMD_Id)))
                                        select new General_SendSMSDTO
                                        {
                                            HRMDES_Id = b.HRMDES_Id,
                                            HRMDES_DesignationName = b.HRMDES_DesignationName,
                                        }
                     ).Distinct().ToArray();

            return data;
        }
        public General_SendSMSDTO get_employee(General_SendSMSDTO data)
        {
            try
            {
                List<General_SendSMSDTO> staffdetails = new List<General_SendSMSDTO>();

                var getmobileno = (from a in _smsContext.HR_Master_Employee_DMO
                                   from b in _smsContext.Multiple_Mobile_DMO
                                   where (a.MI_Id == data.MI_Id && a.HRME_Id == b.HRME_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false
                                   && b.HRMEMNO_DeFaultFlag == "default" && data.multipledes.ToString().Contains(Convert.ToString(a.HRMDES_Id))
                                   && data.multipledep.ToString().Contains(Convert.ToString(a.HRMD_Id)))
                                   select new General_SendSMSDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                       HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName ?? " ",
                                       HRME_EmployeeLastName = a.HRME_EmployeeLastName ?? " ",
                                       HRME_MobileNo = b.HRMEMNO_MobileNo
                                   }).Distinct().ToList();

                var getemailids = (from a in _smsContext.HR_Master_Employee_DMO
                                   from b in _smsContext.Multiple_Email_DMO
                                   where (a.MI_Id == data.MI_Id && a.HRME_Id == b.HRME_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false
                                   && b.HRMEM_DeFaultFlag == "default" && data.multipledes.ToString().Contains(Convert.ToString(a.HRMDES_Id))
                                   && data.multipledep.ToString().Contains(Convert.ToString(a.HRMD_Id)))
                                   select new General_SendSMSDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                       HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName ?? " ",
                                       HRME_EmployeeLastName = a.HRME_EmployeeLastName ?? " ",
                                       hrm_email = b.HRMEM_EmailId
                                   }).Distinct().ToList();

                var getstaffdetails = (from a in _smsContext.HR_Master_Employee_DMO
                                       where (a.MI_Id == data.MI_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false
                                       && data.multipledes.ToString().Contains(Convert.ToString(a.HRMDES_Id))
                                       && data.multipledep.ToString().Contains(Convert.ToString(a.HRMD_Id)))
                                       select new General_SendSMSDTO
                                       {
                                           HRME_Id = a.HRME_Id,
                                           HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                           HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName ?? " ",
                                           HRME_EmployeeLastName = a.HRME_EmployeeLastName ?? " "
                                       }).Distinct().ToList();

                if (getstaffdetails.Count() > 0)
                {
                    for (int k = 0; k < getstaffdetails.Count(); k++)
                    {
                        for (int j = 0; j < getmobileno.Count; j++)
                        {
                            if (getmobileno[j].HRME_Id == getstaffdetails[k].HRME_Id)
                            {
                                getstaffdetails[k].HRME_MobileNo = getmobileno[j].HRME_MobileNo;
                            }
                        }
                    }

                    for (int k = 0; k < getstaffdetails.Count(); k++)
                    {
                        for (int j = 0; j < getemailids.Count; j++)
                        {
                            if (getemailids[j].HRME_Id == getstaffdetails[k].HRME_Id)
                            {
                                getstaffdetails[k].hrm_email = getemailids[j].hrm_email;
                            }
                        }
                    }
                }

                data.stafflist = getstaffdetails.ToArray();


                //data.stafflist = (from a in _smsContext.HR_Master_Employee_DMO
                //                  from b in _smsContext.Multiple_Mobile_DMO
                //                  from c in _smsContext.Multiple_Email_DMO//MasterEmployee
                //                  where (a.MI_Id == data.MI_Id && a.HRME_Id == b.HRME_Id && a.HRME_Id == c.HRME_Id && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && b.HRMEMNO_DeFaultFlag == "default" && c.HRMEM_DeFaultFlag == "default" && data.multipledes.ToString().Contains(Convert.ToString(a.HRMDES_Id)) && data.multipledep.ToString().Contains(Convert.ToString(a.HRMD_Id)))
                //                  select new General_SendSMSDTO
                //                  {
                //                      HRME_Id = a.HRME_Id,
                //                      HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                //                      HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName ?? " ",
                //                      HRME_EmployeeLastName = a.HRME_EmployeeLastName ?? " ",
                //                      HRME_MobileNo = b.HRMEMNO_MobileNo,
                //                      hrm_email = c.HRMEM_EmailId
                //                  }
                //    ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }

        // ****** Exam Marks SMS **************//
        public async Task<string> sendexamgeneralsms(long MI_Id, long mobileNo, string Template, string AMST_Id, string ASMAY_Id, string ASMCL_Id, string ASMS_Id, string EME_Id)
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
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, AMST_Id.ToString());
                    sms = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Exam_SMS_Marks_Details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToString(ASMAY_Id) });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = Convert.ToString(ASMCL_Id) });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = Convert.ToString(ASMS_Id) });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(MI_Id) });
                        cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = Convert.ToString(EME_Id) });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = Convert.ToString(AMST_Id) });


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
        public async Task<string> sendexamgeneralemail(long MI_Id, long mobileNo, string Template, string AMST_Id, string ASMAY_Id, string ASMCL_Id, string ASMS_Id, string EME_Id, string Email)
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
                    result = Mailmsg.Replace(ParamaetersName[0].ISMP_NAME, AMST_Id.ToString());
                    Mailmsg = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Exam_SMS_Marks_Details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToString(ASMAY_Id) });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = Convert.ToString(ASMCL_Id) });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = Convert.ToString(ASMS_Id) });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(MI_Id) });
                        cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = Convert.ToString(EME_Id) });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = Convert.ToString(AMST_Id) });


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

                string Attechement = "";
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
                        Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }


                    //Sending mail using SendGrid API.
                    //Date:07-02-2017.

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
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

        // ********* Exam Grade SMS *************//
        public async Task<string> sendexamgeneralgradesms(long MI_Id, long mobileNo, string Template, string AMST_Id, string ASMAY_Id, string ASMCL_Id, string ASMS_Id, string EME_Id)
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
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, AMST_Id.ToString());
                    sms = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Exam_SMS_Grade_Details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToString(ASMAY_Id) });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = Convert.ToString(ASMCL_Id) });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = Convert.ToString(ASMS_Id) });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(MI_Id) });
                        cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = Convert.ToString(EME_Id) });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = Convert.ToString(AMST_Id) });


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
        public async Task<string> sendexamgeneralgradeemail(long MI_Id, long mobileNo, string Template, string AMST_Id, string ASMAY_Id, string ASMCL_Id, string ASMS_Id, string EME_Id, string Email)
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
                    result = Mailmsg.Replace(ParamaetersName[0].ISMP_NAME, AMST_Id.ToString());
                    Mailmsg = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Exam_SMS_Grade_Details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToString(ASMAY_Id) });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = Convert.ToString(ASMCL_Id) });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = Convert.ToString(ASMS_Id) });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(MI_Id) });
                        cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = Convert.ToString(EME_Id) });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = Convert.ToString(AMST_Id) });


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

                string Attechement = "";
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
                        Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }


                    //Sending mail using SendGrid API.
                    //Date:07-02-2017.

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
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

        // **********  Attendance SMS *************//
        public async Task<string> sendgeneralattendancesms(long MI_Id, long mobileNo, string Template, string AMST_Id, string ASMAY_Id, string ASMCL_Id, string ASMS_Id, string confromdate, string contodate, string condate, string flag)
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
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, AMST_Id.ToString());
                    sms = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "General_SMS_Attendance_Details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(MI_Id) });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToString(ASMAY_Id) });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = Convert.ToString(ASMCL_Id) });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = Convert.ToString(ASMS_Id) });
                        cmd.Parameters.Add(new SqlParameter("@from", SqlDbType.VarChar) { Value = Convert.ToString(confromdate) });
                        cmd.Parameters.Add(new SqlParameter("@to", SqlDbType.VarChar) { Value = Convert.ToString(contodate) });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = Convert.ToString(AMST_Id) });
                        cmd.Parameters.Add(new SqlParameter("@date", SqlDbType.VarChar) { Value = Convert.ToString(condate) });
                        cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.VarChar) { Value = Convert.ToString(flag) });


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
        public async Task<string> sendgeneralattendanceemail(long MI_Id, long mobileNo, string Template, string AMST_Id, string ASMAY_Id, string ASMCL_Id, string ASMS_Id, string confromdate, string contodate, string condate, string flag, string Email)
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
                    result = Mailmsg.Replace(ParamaetersName[0].ISMP_NAME, AMST_Id.ToString());
                    Mailmsg = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "General_SMS_Attendance_Details";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = Convert.ToString(MI_Id) });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = Convert.ToString(ASMAY_Id) });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = Convert.ToString(ASMCL_Id) });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = Convert.ToString(ASMS_Id) });
                        cmd.Parameters.Add(new SqlParameter("@from", SqlDbType.VarChar) { Value = Convert.ToString(confromdate) });
                        cmd.Parameters.Add(new SqlParameter("@to", SqlDbType.VarChar) { Value = Convert.ToString(contodate) });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = Convert.ToString(AMST_Id) });
                        cmd.Parameters.Add(new SqlParameter("@date", SqlDbType.VarChar) { Value = Convert.ToString(condate) });
                        cmd.Parameters.Add(new SqlParameter("@flag", SqlDbType.VarChar) { Value = Convert.ToString(flag) });

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

                string Attechement = "";
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
                        Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }


                    //Sending mail using SendGrid API.
                    //Date:07-02-2017.

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
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
        public async Task<General_SendSMSDTO> savedetailworking(General_SendSMSDTO data)
        {
            if (data.radiotype == "General")
            {
                SmsWithoutTemplate sms = new SmsWithoutTemplate(_db);
                {
                    string s = await sms.sendsmsfromPortal(data.MI_Id, Convert.ToInt64(data.Mobno), data.mes);
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
                    for (int i = 0; i < data.studentlistdto.Length; i++)
                    {
                        string emailtext = data.SmsMailText;
                        if (data.selradioval == "exam" && data.exm_radioval == "mark")
                        {


                            // data.SmsMailText = "Dear" + " " + data.studentlistdto[i].studentName + "," + "  " + "Your" + " " + data.exmname + "  " + "Marks";
                            data.SmsMailText = "  " + "Your" + " " + data.exmname + "  " + "Marks";

                            data.SmsMailText = data.SmsMailText + "--" + data.studentlistdto[i].MarksDetails;

                            emailtext = "Your" + " " + data.exmname + "  " + "Marks" + "--" + data.studentlistdto[i].MarksDetails;
                        }
                        if (data.selradioval == "exam" && data.exm_radioval == "grade")
                        {
                            //data.SmsMailText = "Dear" + " " + data.studentlistdto[i].studentName + "," + "  " + "Your" + " " + data.exmname + "  " + "Grade";
                            data.SmsMailText = "  " + "Your" + " " + data.exmname + "  " + "Grade";
                            data.SmsMailText = data.SmsMailText + "--" + data.studentlistdto[i].GradeDetails;

                            emailtext = "Your" + " " + data.exmname + "  " + "Grade" + "--" + data.studentlistdto[i].GradeDetails;
                        }
                        if (data.selradioval == "attendance")
                        {
                            // data.SmsMailText = "Dear" + " " + data.studentlistdto[i].studentName + ",";
                            data.SmsMailText = "";

                            data.SmsMailText = data.SmsMailText + data.studentlistdto[i].atndetails;

                            emailtext = data.studentlistdto[i].atndetails;
                        }
                        if (data.snd_sms == true)
                        {
                            if (data.studentlistdto[i].AMST_MobileNo != 0)
                            {

                                e = await sendgeneralsms(data.MI_Id, Convert.ToInt64(data.studentlistdto[i].AMST_MobileNo), data.SmsMailText);
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

            else if (data.radiotype == "Staff")
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

                            s = await sendgeneralsms(data.MI_Id, Convert.ToInt64(data.studentlistdto[i].HRME_MobileNo), data.SmsMailText);


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
            return data;
        }
        public string sendmailWT(long MI_Id, string Email, string sms, long UserID, string sub, string footer, filedtosms[] files, string header, string lname, bool aflag, string fhead)
        {
            try
            {
                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                string Mailcontent = sms;
                string Mailmsg = sms;
                Mailcontent = header + "<br />";
                Mailmsg = header  + "<br /><br />";
                // string Resultsms = Mailcontent;
                // string result = Mailmsg;
                Mailcontent = Mailcontent + sms;
                Mailmsg = Mailmsg + sms;
                if (fhead != null && fhead != "")
                {
                    Mailcontent = Mailcontent + "<br /><br />" + fhead + "<br />";
                    Mailmsg = Mailmsg + "<br /><br />" + fhead + "<br />";
                }
                if (footer != "" && footer != null)
                {
                    Mailcontent = Mailcontent + footer;
                    Mailmsg = Mailmsg+ footer;
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


                        var message = new SendGridMessage();
                        message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                        message.Subject = Subject;
                        message.AddTo(Email);


                        if (aflag == true)
                        {
                            if (files.Length > 0)
                            {
                                foreach (var item in files)
                                {
                                    if (item.cfilepath != null && item.cfilepath != "")
                                    {
                                        var webClient = new WebClient();
                                        byte[] imageBytes = webClient.DownloadData(item.cfilepath);

                                        string fileContentsAsBase64 = Convert.ToBase64String(imageBytes);

                                        message.AddAttachment(item.cfilepath, fileContentsAsBase64, null, null, null);
                                    }
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
                            Value = "Sales"
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
        public string templatesendmail(long MI_Id, string Email, string Template, long STDEMP_Id, long ASMAY_Id, long User_Id, string SmsMailText)
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
                    result = Mailmsg.Replace(ParamaetersName[0].ISMP_NAME, User_Id.ToString());
                    Mailmsg = result;
                    Mailcontent = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = "SMSMAILPARAMETER_GENERALSMSNEW";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = User_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@template",
                           SqlDbType.VarChar)
                        {
                            Value = Template
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                           SqlDbType.BigInt)
                        {
                            Value = ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@STDOREMPID",
                           SqlDbType.BigInt)
                        {
                            Value = STDEMP_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@@MI_Idnew",
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


                        var message = new SendGridMessage();
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

                        if (mailcc != "" && mailcc != null)
                        {
                            message.AddCc(mailcc);
                        }

                        if (mailbcc != "" && mailbcc != null)
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
        public async Task<string> templatesendsms(long MI_Id, long mobileNo, string Template, long STDEMP_Id, long ASMAY_Id, long User_Id, string SmsMailText)
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
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, User_Id.ToString());
                    sms = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {

                        cmd.CommandText = "SMSMAILPARAMETER_GENERALSMSNEW";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = User_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@template",
                           SqlDbType.VarChar)
                        {
                            Value = Template
                        });

                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                           SqlDbType.BigInt)
                        {
                            Value = ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@STDOREMPID",
                           SqlDbType.BigInt)
                        {
                            Value = STDEMP_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@@MI_Idnew",
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
        public async Task<string> sendexamgeneralSRKVS(long MI_Id, long mobileNo, string Template, string AMST_Id, string ASMAY_Id, string ASMCL_Id, string ASMS_Id, string EME_Id, string message)
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

                string result = message;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, AMST_Id.ToString());
                    sms = result;
                }
                else
                {
                    //sms = "Hi";

                    sms = message;
                }

                sms = sms.Replace("+", "%2B");
       

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
                    //url = url.Replace("entity_id", "1201159824698906966");

                    //url = url.Replace("template_id", "1207162079874197777");
                    //url = url.Replace("entity_id", "1201159824698906966");

                    //url = url.Replace("template_id", "1207162079874197777");

                    //url = url.Replace("entity_id", insdeta[0].MI_EntityId.ToString());

                    //url = url.Replace("template_id", template.FirstOrDefault().ISES_TemplateId);

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
        //added By sanjeev
        public async Task<string> sendexamgeneralemailSRKVS(long MI_Id, long mobileNo, string Template, string AMST_Id, string ASMAY_Id, string ASMCL_Id, string ASMS_Id, string EME_Id, string Email, string messa)
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
                string Mailmsg = template.FirstOrDefault().ISES_MailHTMLTemplate;
                string result = Mailmsg;                             
                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();
                //messa = messa.Replace("+", "%2B");
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                   // string SendingEmail = "noreply@vapstech.com";
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    //string SendingEmailPassword = "vaps@123";
                   string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    //string SendingEmailHostName = "smtp.gmail.com";
                   Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    //Int32 PortNumber = 587;
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    //string sengridkey = "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";
                   
                   
                    var message = new SendGridMessage();
                    message.HtmlContent = messa;
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
                    

                    message.AddTo(Email);
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
