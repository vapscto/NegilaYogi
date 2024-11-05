using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Impl
{
    public class CLG_PrincipleSMS_SendImpl : Interface.CLG_PrincipleSMS_SendInterface
    {
        public ClgAdmissionContext _ClgAdmissionContext;
        ILogger<CLG_PrincipleSMS_SendImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public CLG_PrincipleSMS_SendImpl(ClgAdmissionContext cpContext, DomainModelMsSqlServerContext db, ILogger<CLG_PrincipleSMS_SendImpl> _acdi)
        {
            _ClgAdmissionContext = cpContext;
            _db = db;
            _acdimpl = _acdi;
        }

        public async Task<SendSMSDTO> Getdetails(SendSMSDTO data)//int IVRMM_Id
        {
            {
                try
                {
                    var list = await _ClgAdmissionContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(e => e.ASMAY_Order).ToListAsync();//AcademicYear
                    data.yearlist = list.ToArray();

                    //var currYear = await _ClgAdmissionContext.AcademicYear.Where(d => d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id).OrderByDescending(e => e.ASMAY_Order).ToListAsync();//AcademicYear
                    //data.currentYear = currYear.ToArray();


                    var designationdropdown = await _ClgAdmissionContext.HR_Master_Designation.Where(t => t.MI_Id == data.MI_Id && t.HRMDES_ActiveFlag == true).ToListAsync();
                    data.designationdropdown = designationdropdown.ToArray();

                    var studentlist = await (from m in _ClgAdmissionContext.Adm_Master_College_StudentDMO
                                       from n in _db.Adm_College_Yearly_StudentDMO
                                       from o in _db.MasterCourseDMO
                                       from p in _db.ClgMasterBranchDMO
                                       from q in _db.CLG_Adm_Master_SemesterDMO
                                       where m.AMCST_Id == n.AMCST_Id && m.MI_Id == data.MI_Id && 
                                       n.ASMAY_Id == data.ASMAY_Id && m.AMCST_SOL.Equals("S") &&
                                       m.AMCST_ActiveFlag == true && n.ACYST_ActiveFlag == 1 
                                       && n.AMCO_Id == o.AMCO_Id && n.AMB_Id == p.AMB_Id && n.AMSE_Id == q.AMSE_Id
                                       && o.AMCO_ActiveFlag == true && p.AMB_ActiveFlag == true && q.AMSE_ActiveFlg == true
                                       select new SendSMSDTO
                                       {
                                           AMCST_Id = n.AMCST_Id,
                                           studentName = m.AMCST_FirstName + (string.IsNullOrEmpty(m.AMCST_MiddleName) || m.AMCST_MiddleName == "0" ? "" : ' ' + m.AMCST_MiddleName) + (string.IsNullOrEmpty(m.AMCST_LastName) || m.AMCST_LastName == "0" ? "" : ' ' + m.AMCST_LastName),
                                           AMCST_AdmNo = m.AMCST_AdmNo,
                                           AMCST_emailId = m.AMCST_emailId,
                                           AMCST_MobileNo = m.AMCST_MobileNo,
                                           AMCO_CourseName = o.AMCO_CourseName,
                                           AMB_BranchName = p.AMB_BranchName,
                                           AMSE_SEMName = q.AMSE_SEMName

                                       }).OrderBy(t => t.studentName).ToListAsync();

                    //var studentlist = await (from m in _db.Adm_M_Student
                    //                         from n in _db.School_Adm_Y_StudentDMO
                    //                         from o in _db.MasterCourseDMO
                    //                         from p in _db.ClgMasterBranchDMO
                    //                         from q in _db.CLG_Adm_Master_SemesterDMO
                    //                         where m.AMST_Id == n.AMST_Id && m.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && m.AMST_SOL.Equals("S") && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1 && m.MI_Id == o.MI_Id && m.MI_Id == p.MI_Id && m.MI_Id == q.MI_Id && o.AMCO_ActiveFlag == true && p.AMB_ActiveFlag == true && q.AMSE_ActiveFlg == true
                    //                         select new SendSMSDTO
                    //                         {
                    //                             AMST_Id = n.AMST_Id,
                    //                             studentName = m.AMST_FirstName + (string.IsNullOrEmpty(m.AMST_MiddleName) || m.AMST_MiddleName == "0" ? "" : ' ' + m.AMST_MiddleName) + (string.IsNullOrEmpty(m.AMST_LastName) || m.AMST_LastName == "0" ? "" : ' ' + m.AMST_LastName),
                    //                             AMST_AdmNo = m.AMST_AdmNo,
                    //                             AMST_emailId = m.AMST_emailId,
                    //                             AMST_MobileNo = m.AMST_MobileNo,
                    //                             AMCO_CourseName = o.AMCO_CourseName,
                    //                             AMB_BranchName = p.AMB_BranchName,
                    //                             AMSE_SEMName = q.AMSE_SEMName

                    //                         }).OrderBy(t => t.studentName).ToListAsync();
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
        public SendSMSDTO getCourse(SendSMSDTO data)
        {
            try
            {
                data.courselist = (from a in _ClgAdmissionContext.MasterCourseDMO
                                   from b in _ClgAdmissionContext.AcademicYear
                                   from c in _ClgAdmissionContext.CLG_Adm_College_AY_CourseDMO
                                   where (a.MI_Id == b.MI_Id && a.AMCO_ActiveFlag == true && c.ASMAY_Id == b.ASMAY_Id && c.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == c.AMCO_Id)
                                   select new SendSMSDTO
                                   {
                                       AMCO_Id = a.AMCO_Id,
                                       AMCO_CourseName = a.AMCO_CourseName,
                                   }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public SendSMSDTO getBranch(SendSMSDTO data)
        {
            try
            {
                data.branchlist = (from a in _ClgAdmissionContext.ClgMasterBranchDMO
                                   from b in _ClgAdmissionContext.CLG_Adm_College_AY_CourseDMO
                                   from c in _ClgAdmissionContext.CLG_Adm_College_AY_Course_BranchDMO
                                   from d in _ClgAdmissionContext.AcademicYear
                                   where (a.AMB_Id == c.AMB_Id && b.ACAYC_Id == c.ACAYC_Id && b.ASMAY_Id == d.ASMAY_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && b.AMCO_Id == data.AMCO_Id)
                                   select new SendSMSDTO
                                   {
                                       AMB_Id = a.AMB_Id,
                                       AMB_BranchName = a.AMB_BranchName,

                                   }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public SendSMSDTO getSemister(SendSMSDTO data)
        {
            try
            {
                data.semisterlist = (from a in _ClgAdmissionContext.CLG_Adm_Master_SemesterDMO
                                     from b in _ClgAdmissionContext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                     from c in _ClgAdmissionContext.CLG_Adm_College_AY_CourseDMO
                                     from d in _ClgAdmissionContext.CLG_Adm_College_AY_Course_BranchDMO
                                     where (b.AMSE_Id == a.AMSE_Id && a.MI_Id == data.MI_Id && c.ACAYC_Id == d.ACAYC_Id && b.ACAYCB_Id == d.ACAYCB_Id && c.AMCO_Id == data.AMCO_Id && d.AMB_Id == data.AMB_Id && c.ASMAY_Id == data.ASMAY_Id)
                                     select new SendSMSDTO
                                     {
                                         AMSE_Id = a.AMSE_Id,
                                         AMSE_SEMName = a.AMSE_SEMName,
                                     }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public SendSMSDTO GetEmployeeDetailsByLeaveYearAndMonth(SendSMSDTO data)
        {
            //List<MasterEmployee> employe = new List<MasterEmployee>();
            List<HR_Master_Employee_DMO> employe = new List<HR_Master_Employee_DMO>();

            try
            {
                employe = (from a in _ClgAdmissionContext.HR_Master_Employee_DMO//MasterEmployee

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
            var rolelist = _ClgAdmissionContext.MasterRoleType.Where(t => t.IVRMRT_Id == data.roleId).ToList();

            string rolecheck = "";

            if (rolelist.Count > 0)
            {
                if (rolelist.FirstOrDefault().IVRMRT_Role.Equals("College Principal", StringComparison.OrdinalIgnoreCase))
                {
                    rolecheck = "P";

                }
                if (rolelist.FirstOrDefault().IVRMRT_Role.Equals("College Manager", StringComparison.OrdinalIgnoreCase))
                {
                    rolecheck = "M";

                }
                if (data.radiotype == "General")
                {
                    SmsWithoutTemplate sms = new SmsWithoutTemplate(_db);
                    {
                        // data.Mobno = "9591081840";
                        string s = "";
                        if (rolecheck == "P")
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
                //var studentlist = await (from m in _db.Adm_M_Student
                //                         from n in _db.School_Adm_Y_StudentDMO
                //                         from o in _db.Adm_M_Student_MobileNo
                //                         from p in _db.MasterCourseDMO
                //                         from q in _db.ClgMasterBranchDMO
                //                         from r in _db.CLG_Adm_Master_SemesterDMO
                //                         where m.AMST_Id == n.AMST_Id && m.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && m.AMST_SOL.Equals("S") && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1 && p.AMCO_Id == data.AMCO_Id && q.AMB_Id == data.AMB_Id && r.AMSE_Id == data.AMSE_Id && m.AMST_Id == o.AMST_Id
                //                         select new SendSMSDTO
                //                         {
                //                             AMST_Id = n.AMST_Id,
                //                             studentName = m.AMST_FirstName + (string.IsNullOrEmpty(m.AMST_MiddleName) || m.AMST_MiddleName == "0" ? "" : ' ' + m.AMST_MiddleName) + (string.IsNullOrEmpty(m.AMST_LastName) || m.AMST_LastName == "0" ? "" : ' ' + m.AMST_LastName),
                //                             AMST_AdmNo = m.AMST_AdmNo,
                //                             AMST_emailId = m.AMST_emailId,
                //                             AMST_MobileNo = Convert.ToInt64(o.AMSTSMS_MobileNo)

                //                         }).OrderBy(f => f.studentName).ToListAsync();

                var studentlist = await (from m in _ClgAdmissionContext.Adm_Master_College_StudentDMO
                                         from n in _db.Adm_College_Yearly_StudentDMO
                                         from o in _db.MasterCourseDMO
                                         from p in _db.ClgMasterBranchDMO
                                         from q in _db.CLG_Adm_Master_SemesterDMO
                                         where m.AMCST_Id == n.AMCST_Id && m.MI_Id == data.MI_Id &&
                                         n.ASMAY_Id == data.ASMAY_Id && m.AMCST_SOL.Equals("S") &&
                                         m.AMCST_ActiveFlag == true && n.ACYST_ActiveFlag == 1 
                                        && n.AMCO_Id==o.AMCO_Id && n.AMB_Id==p.AMB_Id && n.AMSE_Id==q.AMSE_Id 
                                         && o.AMCO_ActiveFlag == true && p.AMB_ActiveFlag == true && q.AMSE_ActiveFlg == true
                                         select new SendSMSDTO
                                         {
                                             AMCST_Id = n.AMCST_Id,
                                             studentName = m.AMCST_FirstName + (string.IsNullOrEmpty(m.AMCST_MiddleName) || m.AMCST_MiddleName == "0" ? "" : ' ' + m.AMCST_MiddleName) + (string.IsNullOrEmpty(m.AMCST_LastName) || m.AMCST_LastName == "0" ? "" : ' ' + m.AMCST_LastName),
                                             AMCST_AdmNo = m.AMCST_AdmNo,
                                             AMCST_emailId = m.AMCST_emailId,
                                             AMCST_MobileNo = m.AMCST_MobileNo,
                                             AMCO_CourseName = o.AMCO_CourseName,
                                             AMB_BranchName = p.AMB_BranchName,
                                             AMSE_SEMName = q.AMSE_SEMName

                                         }).OrderBy(t => t.studentName).ToListAsync();
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
            try
            {
                data.departmentdropdown = (from a in _ClgAdmissionContext.HR_Master_Department
                                           from b in _ClgAdmissionContext.HR_Master_Employee_DMO
                                           where (a.HRMD_Id == b.HRMD_Id && a.HRMD_ActiveFlag == true && a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id)
                                           select new SendSMSDTO
                                           {
                                               HRMD_Id = a.HRMD_Id,
                                               HRMD_DepartmentName = a.HRMD_DepartmentName
                                           }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public SendSMSDTO get_designation(SendSMSDTO data)
        {
            data.designationdropdown = (from a in _ClgAdmissionContext.HR_Master_Employee_DMO//MasterEmployee
                                        from b in _ClgAdmissionContext.HR_Master_Designation
                                        from c in _ClgAdmissionContext.HR_Master_Department
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
            data.stafflist = (from a in _ClgAdmissionContext.HR_Master_Employee_DMO//MasterEmployee
                              from b in _ClgAdmissionContext.Multiple_Mobile_DMO
                              from c in _ClgAdmissionContext.Multiple_Email_DMO
                              where (a.MI_Id == data.MI_Id && data.multipledes.ToString().Contains(Convert.ToString(a.HRMDES_Id)) && data.multipledep.ToString().Contains(Convert.ToString(a.HRMD_Id)) && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false && a.HRME_Id == b.HRME_Id && a.HRME_Id == c.HRME_Id)
                              select new SendSMSDTO
                              {
                                  HRME_Id = a.HRME_Id,
                                  HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                  HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName ?? " ",
                                  HRME_EmployeeLastName = a.HRME_EmployeeLastName ?? " ",
                                  HRME_MobileNo = b.HRMEMNO_MobileNo,
                                  hrm_email = c.HRMEM_EmailId
                              }
                     ).Distinct().OrderBy(e => e.HRME_EmployeeFirstName).ToArray();
            return data;
        }
    }
}
