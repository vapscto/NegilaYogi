
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
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.com.vaps.admission;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using Microsoft.AspNetCore.Hosting;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;
using DomainModel.Model.com.vapstech.HRMS;

namespace PortalHub.com.vaps.Principal.Services
{
    public class SendSMSImpl : Interfaces.SendSMSInterface
    {
        int MI_ID = 0;
        private readonly IHostingEnvironment _hostingEnvironment;

        private static ConcurrentDictionary<string, SendSMSDTO> _login =
         new ConcurrentDictionary<string, SendSMSDTO>();

        private readonly PortalContext _PrincipalDashboardContext;
        ILogger<SendSMSImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public SendSMSImpl(PortalContext cpContext, DomainModelMsSqlServerContext db, IHostingEnvironment hostingEnvironment)
        {
            _PrincipalDashboardContext = cpContext;
            _db = db;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<SendSMSDTO> Getdetails(SendSMSDTO data)//int IVRMM_Id
        {
            {
                try
                {
                    var list = await _PrincipalDashboardContext.AcademicYearDMO.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(e=>e.ASMAY_Order).ToListAsync();//AcademicYear
                    data.yearlist = list.ToArray();

                    var currYear = await _PrincipalDashboardContext.AcademicYearDMO.Where(d => d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id).OrderByDescending(e => e.ASMAY_Order).ToListAsync();//AcademicYear
                    data.currentYear = currYear.ToArray();

                    var classlist =await _db.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToListAsync();
                    data.classlist = classlist.ToArray();

                    var sectionlist =await _db.School_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).ToListAsync();
                    data.sectionlist = sectionlist.ToArray();
             
                    var designationdropdown =await _PrincipalDashboardContext.HR_Master_Designation.Where(t => t.MI_Id == data.MI_Id && t.HRMDES_ActiveFlag == true).ToListAsync();
                    data.designationdropdown = designationdropdown.ToArray();



                    var studentlist =await (from m in _db.Adm_M_Student
                                       from n in _db.School_Adm_Y_StudentDMO
                                       where m.AMST_Id == n.AMST_Id && m.MI_Id == data.MI_Id && n.ASMAY_Id == currYear.FirstOrDefault().ASMAY_Id && m.AMST_SOL.Equals("S") && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1 && n.ASMCL_Id == classlist.FirstOrDefault().ASMCL_Id && n.ASMS_Id == sectionlist.FirstOrDefault().ASMS_Id
                                       select new SendSMSDTO
                                       {
                                           AMCST_Id = n.AMST_Id,
                                           studentName = m.AMST_FirstName + (string.IsNullOrEmpty(m.AMST_MiddleName)  || m.AMST_MiddleName=="0" ? "" : ' ' + m.AMST_MiddleName) + (string.IsNullOrEmpty(m.AMST_LastName) || m.AMST_LastName=="0" ? "" : ' ' + m.AMST_LastName),
                                           AMCST_AdmNo = m.AMST_AdmNo,
                                           AMCST_emailId = m.AMST_emailId,
                                           AMCST_MobileNo = m.AMST_MobileNo

                                       }).OrderBy(t=>t.studentName).ToListAsync();
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
        public SendSMSDTO GetEmployeeDetailsByLeaveYearAndMonth(SendSMSDTO data)
        {
            //List<MasterEmployee> employe = new List<MasterEmployee>();
            List<HR_Master_Employee_DMO> employe = new List<HR_Master_Employee_DMO>();

            try
            {
                employe = (from a in _PrincipalDashboardContext.HR_Master_Employee_DMO//MasterEmployee

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


        public async Task<SendSMSDTO> savedetail(SendSMSDTO data)
        {
            var rolelist = _PrincipalDashboardContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.roleId).ToList();

            string rolecheck = "";

            if (rolelist.Count >0)
            {
                if (rolelist.FirstOrDefault().IVRMRT_Role.Equals("Principal", StringComparison.OrdinalIgnoreCase))
                {
                    rolecheck = "P";

                }
                if (rolelist.FirstOrDefault().IVRMRT_Role.Equals("Manager", StringComparison.OrdinalIgnoreCase))
                {
                    rolecheck = "M";

                }
                if (data.radiotype == "General")
                {
                    SmsWithoutTemplate sms = new SmsWithoutTemplate(_db);
                    {
                       // data.Mobno = "9591081840";
                        string s = "";
                        if (rolecheck =="P")
                        {
                            s = await sms.sendsmsfromPortal_new(data.MI_Id, Convert.ToInt64(data.Mobno), data.mes, "PRINCIPAL", "PRINCIPAL DASHBOARD");
                        }
                        else if (rolecheck == "M")
                        {
                            s = await sms.sendsmsfromPortal_new(data.MI_Id, Convert.ToInt64(data.Mobno), data.mes, "MANAGER", "MANAGER PORTAL");
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


                else if (data.radiotype == "Student")
                {
                    if (data.studentlistdto.Length > 0)
                    {
                        SmsWithoutTemplate sms = new SmsWithoutTemplate(_db);
                        EmailWithoutTemplate email = new EmailWithoutTemplate(_db);

                        for (int i = 0; i < data.studentlistdto.Length; i++)
                        {
                            if (data.studentlistdto[i].AMCST_MobileNo != 0)
                            {
                               // data.studentlistdto[i].AMST_MobileNo = 9591081840;

                                if (rolecheck == "P")
                                {
                                    await sms.sendsmsfromPortal_new(data.MI_Id, Convert.ToInt64(data.studentlistdto[i].AMCST_MobileNo), data.SmsMailText, "PRINCIPAL", "PRINCIPAL DASHBOARD");
                                }
                                else if (rolecheck == "M")
                                {
                                    await sms.sendsmsfromPortal_new(data.MI_Id, Convert.ToInt64(data.studentlistdto[i].AMCST_MobileNo), data.SmsMailText, "MANAGER", "MANAGER PORTAL");
                                }
                                   
                            }
                            if (data.studentlistdto[i].AMCST_emailId != "")

                            {
                               // data.studentlistdto[i].AMST_emailId = "praveenishwar@vapstech.com";
                                string e = "";

                                if (rolecheck == "P")
                                {
                                    e = email.PortalEmailWthtTmp_new(data.MI_Id, data.studentlistdto[i].studentName, data.studentlistdto[i].AMCST_emailId, data.SmsMailText, "PRINCIPAL", "PRINCIPAL DASHBOARD");
                                }
                                else if (rolecheck == "M")
                                {
                                    e = email.PortalEmailWthtTmp_new(data.MI_Id, data.studentlistdto[i].studentName, data.studentlistdto[i].AMCST_emailId, data.SmsMailText, "MANAGER", "MANAGER PORTAL");
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
                }

                else if (data.radiotype == "Staff")
                {
                    SmsWithoutTemplate sms = new SmsWithoutTemplate(_db);
                    EmailWithoutTemplate email = new EmailWithoutTemplate(_db);
                    for (int i = 0; i < data.studentlistdto.Length; i++)
                    {

                        // data.studentlistdto[i].HRME_MobileNo = 9591081840;
                        //   data.studentlistdto[i].hrm_email = "praveenishwar@vapstech.com";

                        if (data.studentlistdto[i].HRME_MobileNo != 0)
                        {
                            string s = "";
                            if (rolecheck == "P")
                            {
                                s = await sms.sendsmsfromPortal_new(data.MI_Id, Convert.ToInt64(data.studentlistdto[i].HRME_MobileNo), data.SmsMailText, "PRINCIPAL", "PRINCIPAL DASHBOARD");
                            }
                            else if (rolecheck == "M")
                            {
                                s = await sms.sendsmsfromPortal_new(data.MI_Id, Convert.ToInt64(data.studentlistdto[i].HRME_MobileNo), data.SmsMailText, "MANAGER", "MANAGER PORTAL");
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

                        if (data.studentlistdto[i].hrm_email != "" && data.studentlistdto[i].hrm_email != null)
                        {

                            string e = "";
                            if (rolecheck == "P")
                            {
                                e = email.PortalEmailWthtTmp_new(data.MI_Id, data.studentlistdto[i].studentName, data.studentlistdto[i].AMCST_emailId, data.SmsMailText, "PRINCIPAL", "PRINCIPAL DASHBOARD");
                            }
                            else if (rolecheck == "M")
                            {
                                e = email.PortalEmailWthtTmp_new(data.MI_Id, data.studentlistdto[i].studentName, data.studentlistdto[i].AMCST_emailId, data.SmsMailText, "MANAGER", "MANAGER PORTAL");
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
            }

          
            return data;
        }


        public async Task<SendSMSDTO> GetStudentDetails(SendSMSDTO data)
        {
            try
            {
                var studentlist =await (from m in _db.Adm_M_Student
                                   from n in _db.School_Adm_Y_StudentDMO
                                   from o in _db.Adm_M_Student_MobileNo
                                   where m.AMST_Id == n.AMST_Id && m.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && m.AMST_SOL.Equals("S") && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1 && n.ASMCL_Id == data.ASMCL_Id && n.ASMS_Id == data.ASMS_Id && m.AMST_Id==o.AMST_Id  
                                   select new SendSMSDTO
                                   {
                                       AMCST_Id = n.AMST_Id,
                                       studentName = m.AMST_FirstName + (string.IsNullOrEmpty(m.AMST_MiddleName) || m.AMST_MiddleName == "0" ? "" : ' ' + m.AMST_MiddleName) + (string.IsNullOrEmpty(m.AMST_LastName) || m.AMST_LastName == "0" ? "" : ' ' + m.AMST_LastName),
                                       AMCST_AdmNo = m.AMST_AdmNo,
                                       AMCST_emailId = m.AMST_emailId,
                                       AMCST_MobileNo = Convert.ToInt64(o.AMSTSMS_MobileNo)

                                   }).OrderBy(f=>f.studentName).ToListAsync();
                if (studentlist.Count > 0)
                {
                    data.studentlist = studentlist.ToArray();
                    data.studentCount = studentlist.Count;
                }

            }
            catch (Exception)
            {

                throw;
            }

            return data;
        }


        public SendSMSDTO Getdepartment(SendSMSDTO data)
        {
            var departmentdropdown =  _PrincipalDashboardContext.HR_Master_Department.Where(t => t.MI_Id == data.MI_Id && t.HRMD_ActiveFlag == true).ToList();
            data.departmentdropdown = departmentdropdown.ToArray();
            return data;
        }


        public SendSMSDTO get_designation(SendSMSDTO data)
        {
            data.designationdropdown = ( from a in _PrincipalDashboardContext.HR_Master_Employee_DMO//MasterEmployee
                                         from b in _PrincipalDashboardContext.HR_Master_Designation
                                         from c in _PrincipalDashboardContext.HR_Master_Department
                                    where (a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id == c.HRMD_Id && c.HRMD_ActiveFlag == true
                                    && b.HRMDES_ActiveFlag == true && a.HRME_ActiveFlag == true
                                    && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && data.multipledep.ToString().Contains(Convert.ToString(c.HRMD_Id)))
                                    select new SendSMSDTO
                                    {
                                        HRMDES_Id = b.HRMDES_Id,
                                        HRMDES_DesignationName = b.HRMDES_DesignationName,
                                    }
                     ).Distinct().ToArray();

            return data;
        }
       
        public SendSMSDTO get_employee(SendSMSDTO data)
        {
            data.stafflist = (from a in _PrincipalDashboardContext.HR_Master_Employee_DMO//MasterEmployee
                              from b in _PrincipalDashboardContext.Multiple_Mobile_DMO
                              from c in _PrincipalDashboardContext.Multiple_Email_DMO
                              where (a.MI_Id == data.MI_Id && data.multipledes.ToString().Contains(Convert.ToString(a.HRMDES_Id))  && data.multipledep.ToString().Contains(Convert.ToString(a.HRMD_Id)) && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false  && a.HRME_Id==b.HRME_Id && a.HRME_Id==c.HRME_Id)
                                 select new SendSMSDTO
                                 {
                                     HRME_Id = a.HRME_Id,
                                     HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                     HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName ?? " ",
                                     HRME_EmployeeLastName = a.HRME_EmployeeLastName ?? " ",
                                     HRME_MobileNo = b.HRMEMNO_MobileNo,
                                     hrm_email = c.HRMEM_EmailId
                                 }
                     ).Distinct().OrderBy(e=>e.HRME_EmployeeFirstName).ToArray();
            return data;
        }
    }
}
