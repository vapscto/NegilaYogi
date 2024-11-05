using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.Portals.Student;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Student.Services
{
    public class OnlineLeaveAppImpl : Interfaces.OnlineLeaveAppInterface
    {
        private PortalContext _PortalContext;
        public DomainModelMsSqlServerContext _db;

        public OnlineLeaveAppImpl(PortalContext para, DomainModelMsSqlServerContext db)
        {
            _PortalContext = para;
            _db = db;
        }

        public OnlineLeaveApp_DTO getdetails(OnlineLeaveApp_DTO data)
        {
            try
            {
                #region flag Details
                if (data.flag != "" && data.flag != null)
                {
                    var roletyp = _PortalContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.IVRMRT_Id).ToList();
                    data.roletype = roletyp.FirstOrDefault().IVRMRT_Role;
                }

                if (data.roletype.Equals("Student", StringComparison.OrdinalIgnoreCase))
                {
                    data.flag_Type = data.roletype;

                    data.studentdetails = (from a in _PortalContext.Adm_M_Student
                                           from b in _PortalContext.School_Adm_Y_StudentDMO
                                           from c in _PortalContext.School_M_Class
                                           from s in _PortalContext.School_M_Section
                                           where (b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == s.ASMS_Id && a.AMST_Id == b.AMST_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMST_Id == data.AMST_Id && a.AMST_SOL == "S")
                                           select new OnlineLeaveApp_DTO
                                           {
                                               ASMCL_Id = c.ASMCL_Id,
                                               ASMCL_ClassName = c.ASMCL_ClassName,
                                               ASMS_Id = b.ASMS_Id,
                                               ASMC_SectionName = s.ASMC_SectionName,
                                               AMST_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),
                                               AMST_RegistrationNo = a.AMST_RegistrationNo,
                                               AMST_MobileNo = a.AMST_MobileNo,
                                               AMST_emailId = a.AMST_emailId,
                                               AMST_Id = a.AMST_Id,
                                           }).Distinct().ToArray();

                    data.allstuddata = (from a in _PortalContext.Adm_M_Student
                                        from d in _PortalContext.Adm_Students_Leave_Apply_DMO
                                        from b in _PortalContext.School_Adm_Y_StudentDMO
                                        from c in _PortalContext.School_M_Class
                                        from s in _PortalContext.School_M_Section
                                        where (b.AMST_Id == d.AMST_Id && b.AMST_Id == a.AMST_Id && b.ASMCL_Id == c.ASMCL_Id && d.ASMAY_Id == b.ASMAY_Id && b.ASMS_Id == s.ASMS_Id && a.MI_Id == c.MI_Id && d.MI_Id == data.MI_Id && d.AMST_Id == data.AMST_Id && a.AMST_SOL == "S")
                                        select new OnlineLeaveApp_DTO
                                        {
                                            ASMCL_Id = c.ASMCL_Id,
                                            AMST_Id = a.AMST_Id,
                                            ASMCL_ClassName = c.ASMCL_ClassName,
                                            ASMS_Id = b.ASMS_Id,
                                            ASMC_SectionName = s.ASMC_SectionName,
                                            AMST_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),
                                            ASLA_Status = d.ASLA_Status,
                                            ASLA_ApplyDate = d.ASLA_ApplyDate,
                                            ASLA_FromDate = d.ASLA_FromDate,
                                            ASLA_ToDate = d.ASLA_ToDate,
                                            ASLA_Flag = d.ASLA_Flag,
                                            ASLA_ActiveFlag = d.ASLA_ActiveFlag,
                                            ASLA_Id = d.ASLA_Id,
                                        }).Distinct().OrderBy(t => t.ASLA_ApplyDate).ToArray();

                }
                else if (data.roletype.Equals("Staff", StringComparison.OrdinalIgnoreCase))
                {
                    data.flag_Type = data.roletype;
                    var hrme = _PortalContext.Staff_User_Login.Where(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Distinct().ToList();
                    if (hrme.Count > 0)
                    {
                        data.HRME_Id = hrme.FirstOrDefault().Emp_Code;
                    }

                    //data.HRME_Id = _PortalContext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                    //var clstchname = (from a in _db.Adm_SchAttLoginUserClass
                    //                  from b in _db.Adm_SchAttLoginUser
                    //                  from c in _db.School_M_Class
                    //                  where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id
                    //                  && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                    //                  && b.HRME_Id == data.HRME_Id
                    //                  && c.ASMCL_ActiveFlag == true)
                    //                  select new OnlineLeaveApp_DTO
                    //                  {
                    //                      ASMCL_Id = c.ASMCL_Id,
                    //                      ASMCL_ClassName = c.ASMCL_ClassName,
                    //                      ASMS_Id = a.ASMS_Id,
                    //                  }
                    //                ).Distinct().ToArray();
                    data.pendingleave = (from a in _PortalContext.AdmissionStudentDMO
                                         from b in _PortalContext.School_Adm_Y_StudentDMO
                                         from c in _PortalContext.Adm_Students_Leave_Apply_DMO
                                         from d in _PortalContext.Adm_Students_Leave_Approval_DMO
                                         from cl in _PortalContext.School_M_Class
                                         from sc in _PortalContext.School_M_Section
                                         where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == b.ASMAY_Id && b.ASMCL_Id == cl.ASMCL_Id && b.ASMS_Id == sc.ASMS_Id && b.AMST_Id == c.AMST_Id && c.ASLA_Id == d.ASLA_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && c.ASLA_ActiveFlag == true && c.ASLA_Status == "Pending")
                                         select new OnlineLeaveApp_DTO
                                         {
                                             AMST_Id = a.AMST_Id,
                                             AMST_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),
                                             ASMS_Id = sc.ASMS_Id,
                                             ASMC_SectionName = sc.ASMC_SectionName,
                                             ASMCL_ClassName = cl.ASMCL_ClassName,
                                             ASMCL_Id = cl.ASMCL_Id,
                                             ASLA_Status = c.ASLA_Status,
                                             ASLA_LeaveId = c.ASLA_LeaveId,
                                             ASLA_Reason = c.ASLA_Reason,
                                             ASLA_ApplyDate = c.ASLA_ApplyDate,
                                             ASLA_FromDate = c.ASLA_FromDate,
                                             ASLA_ToDate = c.ASLA_ToDate,
                                             ASLA_Flag = c.ASLA_Flag,
                                             ASLA_Id = c.ASLA_Id,
                                             MI_Id = c.MI_Id,
                                             ASMAY_Id = c.ASMAY_Id,
                                             AMST_RegistrationNo = a.AMST_RegistrationNo,
                                             AMST_emailId = a.AMST_emailId,
                                             AMST_MobileNo = a.AMST_MobileNo,
                                         }).OrderBy(t => t.ASLA_ApplyDate).ToArray();

                }
                else if (data.roletype.Equals("Principal", StringComparison.OrdinalIgnoreCase))
                {
                    data.flag_Type = data.roletype;
                    var hrme = _PortalContext.Staff_User_Login.Where(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Distinct().ToList();
                    if (hrme.Count > 0)
                    {
                        data.HRME_Id = hrme.FirstOrDefault().Emp_Code;
                    }

                    //var clstchname = (from a in _db.Adm_SchAttLoginUserClass
                    //                  from b in _db.Adm_SchAttLoginUser
                    //                  from c in _db.School_M_Class
                    //                  where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id
                    //                  && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                    //                  && b.HRME_Id == data.HRME_Id
                    //                  && c.ASMCL_ActiveFlag == true)
                    //                  select new OnlineLeaveApp_DTO
                    //                  {
                    //                      ASMCL_Id = c.ASMCL_Id,
                    //                      ASMCL_ClassName = c.ASMCL_ClassName,
                    //                      ASMS_Id = a.ASMS_Id,
                    //                  }
                    //                ).Distinct().ToArray();
                    data.pendingleave = (from a in _PortalContext.AdmissionStudentDMO
                                         from b in _PortalContext.School_Adm_Y_StudentDMO
                                         from c in _PortalContext.Adm_Students_Leave_Apply_DMO
                                         from d in _PortalContext.Adm_Students_Leave_Approval_DMO
                                         from cl in _PortalContext.School_M_Class
                                         from sc in _PortalContext.School_M_Section
                                         where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == b.ASMAY_Id && b.ASMCL_Id == cl.ASMCL_Id && b.ASMS_Id == sc.ASMS_Id && b.AMST_Id == c.AMST_Id && c.ASLA_Id == d.ASLA_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.AMST_SOL == "S" && b.AMAY_ActiveFlag == 1 && c.ASLA_ActiveFlag == true && c.ASLA_Status == "Pending")
                                         select new OnlineLeaveApp_DTO
                                         {
                                             AMST_Id = a.AMST_Id,
                                             AMST_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),
                                             ASMS_Id = sc.ASMS_Id,
                                             ASMC_SectionName = sc.ASMC_SectionName,
                                             ASMCL_ClassName = cl.ASMCL_ClassName,
                                             ASMCL_Id = cl.ASMCL_Id,
                                             ASLA_Status = c.ASLA_Status,
                                             ASLA_LeaveId = c.ASLA_LeaveId,
                                             ASLA_Reason = c.ASLA_Reason,
                                             ASLA_ApplyDate = c.ASLA_ApplyDate,
                                             ASLA_FromDate = c.ASLA_FromDate,
                                             ASLA_ToDate = c.ASLA_ToDate,
                                             ASLA_Flag = c.ASLA_Flag,
                                             ASLA_Id = c.ASLA_Id,
                                             MI_Id = c.MI_Id,
                                             ASMAY_Id = c.ASMAY_Id,
                                             AMST_RegistrationNo = a.AMST_RegistrationNo,
                                             AMST_emailId = a.AMST_emailId,
                                             AMST_MobileNo = a.AMST_MobileNo,
                                         }).OrderBy(t => t.ASLA_ApplyDate).ToArray();

                }
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }

        public async Task<OnlineLeaveApp_DTO> leaveapply(OnlineLeaveApp_DTO data)
        {
            try
            {
                string s = "";
                string m = "";
                if (data.ASLA_Id > 0)
                {
                    var dupliacte = _PortalContext.Adm_Students_Leave_Apply_DMO.Where(t => t.MI_Id == data.MI_Id && t.ASLA_Id != data.ASLA_Id && t.ASLA_ApplyDate == data.ASLA_ApplyDate && t.ASLA_FromDate == data.ASLA_FromDate && t.ASLA_ToDate == data.ASLA_ToDate).ToList();

                    if (dupliacte.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var upadte = _PortalContext.Adm_Students_Leave_Apply_DMO.Where(t => t.ASLA_Id == data.ASLA_Id && t.MI_Id == data.MI_Id).SingleOrDefault();

                        upadte.AMST_Id = data.AMST_Id;
                        upadte.ASLA_Reason = data.ASLA_Reason;
                        upadte.ASLA_ApplyDate = data.ASLA_ApplyDate;
                        upadte.ASLA_FromDate = data.ASLA_FromDate;
                        upadte.ASLA_ToDate = data.ASLA_ToDate;
                        upadte.ASLA_Status = "Pending";
                        upadte.ASLA_Flag = data.ASLA_Flag;
                        upadte.UpdatedDate = DateTime.Now;

                        _PortalContext.Update(upadte);

                        var update2 = _PortalContext.Adm_Students_Leave_Approval_DMO.Where(t => t.MI_Id == data.MI_Id && t.ASLA_Id == upadte.ASLA_Id).SingleOrDefault();
                        update2.UpdatedDate = DateTime.Now;
                        _PortalContext.Update(update2);

                        int rowAffected = _PortalContext.SaveChanges();
                        if (rowAffected > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }

                }
                else
                {
                    var dupliacte = _PortalContext.Adm_Students_Leave_Apply_DMO.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == data.AMST_Id && t.ASLA_LeaveId == data.ASLA_LeaveId && t.ASLA_Status == "Pending" && t.ASLA_FromDate == data.ASLA_FromDate && t.ASLA_ToDate == data.ASLA_ToDate).ToList();

                    if (dupliacte.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        Adm_Students_Leave_Apply_DMO obj = new Adm_Students_Leave_Apply_DMO();

                        obj.MI_Id = data.MI_Id;
                        obj.ASMAY_Id = data.ASMAY_Id;
                        obj.AMST_Id = data.AMST_Id;
                        obj.ASLA_LeaveId = data.ASLA_LeaveId;
                        obj.ASLA_Reason = data.ASLA_Reason;
                        obj.ASLA_ApplyDate = data.ASLA_ApplyDate;
                        obj.ASLA_FromDate = data.ASLA_FromDate;
                        obj.ASLA_ToDate = data.ASLA_ToDate;
                        obj.ASLA_Status = "Pending";
                        obj.ASLA_Flag = data.ASLA_Flag;
                        obj.ASLA_ActiveFlag = true;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;

                        _PortalContext.Add(obj);

                        Adm_Students_Leave_Approval_DMO obj2 = new Adm_Students_Leave_Approval_DMO();

                        obj2.MI_Id = data.MI_Id;
                        obj2.ASLA_Id = obj.ASLA_Id;
                        obj2.ASLAP_AppRejDate = null;
                        obj2.ASLAP_AppFromDate = null;
                        obj2.ASLAP_AppToDate = null;
                        obj2.ASLAP_LeaveStatus = "Pending";
                        obj2.ASALP_RejectReason = "";
                        obj2.IVRMALU_Id = null;
                        obj2.CreatedDate = DateTime.Now;
                        obj2.UpdatedDate = DateTime.Now;

                        _PortalContext.Add(obj2);

                        int rowAffected = _PortalContext.SaveChanges();
                        if (rowAffected > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }

                        if (rowAffected > 0)
                        {
                            var clsid = (from d in _PortalContext.AcademicYearDMO
                                         from a in _PortalContext.School_M_Class
                                         from c in _PortalContext.School_Adm_Y_StudentDMO
                                         where (a.ASMCL_Id == c.ASMCL_Id && d.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.AMST_Id == data.AMST_Id)
                                         select new ExamDTO
                                         {
                                             ASMCL_Id = c.ASMCL_Id,
                                             ASMCL_ClassName = a.ASMCL_ClassName,
                                         }
                                ).Distinct().ToList();

                            var hrmeids = (from a in _PortalContext.School_M_Class
                                           from b in _PortalContext.HR_Master_Employee_DMO
                                           from c in _PortalContext.IVRM_PrincipalDMO
                                           from d in _PortalContext.IVRM_Principal_ClassDMO
                                           where (a.ASMCL_Id == d.ASMCL_Id && b.HRME_Id == c.IVRMUL_Id && a.MI_Id == c.MI_Id && c.MI_Id == data.MI_Id && a.ASMCL_Id == clsid.FirstOrDefault().ASMCL_Id)
                                           select new TransferCertificate_DTO
                                           {
                                               HRME_Id = c.IVRMUL_Id
                                           }
                                          ).Distinct().ToList();
                            if (hrmeids.Count > 0)
                            {
                                var hrmeidp = hrmeids.FirstOrDefault().HRME_Id;

                                long mobileno = 0;
                                var emailid = "";
                                var mobile = _PortalContext.Multiple_Mobile_DMO.Where(p => p.HRME_Id == hrmeidp && p.HRMEMNO_DeFaultFlag == "default").ToList();
                                var email = _PortalContext.Multiple_Email_DMO.Where(e => e.HRME_Id == hrmeidp && e.HRMEM_DeFaultFlag == "default").ToList();

                                if (mobile.Count > 0)
                                {
                                    var mobilenumber = _PortalContext.Multiple_Mobile_DMO.Where(p => p.HRME_Id == hrmeidp && p.HRMEMNO_DeFaultFlag == "default").Distinct().ToList();
                                    mobileno = mobilenumber.FirstOrDefault().HRMEMNO_MobileNo;
                                    SMS sms = new SMS(_db);
                                    s = await sms.sendCertificateSms(data.MI_Id, mobileno, "LEAVEREQUEST", data.AMST_Id, hrmeidp, data.ASMAY_Id, obj.ASLA_Id);


                                }
                                if (email.Count > 0)
                                {
                                    var mailid = _PortalContext.Multiple_Email_DMO.Where(e => e.HRME_Id == hrmeidp && e.HRMEM_DeFaultFlag == "default").Distinct().ToList();

                                    emailid = mailid.FirstOrDefault().HRMEM_EmailId;

                                    Email Email = new Email(_db);
                                    m = Email.sendCertificatemail(data.MI_Id, emailid, "LEAVEREQUEST", data.AMST_Id, hrmeidp, data.ASMAY_Id, obj.ASLA_Id);
                                }
                            }

                            long mobilenoT = 0;
                            var emailidT = "";
                            var clsecids = (from d in _PortalContext.AcademicYearDMO
                                            from a in _PortalContext.School_M_Class
                                            from b in _PortalContext.School_M_Section
                                            from c in _PortalContext.School_Adm_Y_StudentDMO
                                            where (a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.AMST_Id == data.AMST_Id)
                                            select new ExamDTO
                                            {
                                                ASMCL_Id = c.ASMCL_Id,
                                                ASMCL_ClassName = a.ASMCL_ClassName,
                                                ASMS_Id = c.ASMS_Id,
                                            }
                       ).Distinct().ToList();
                            if (clsecids.Count > 0)
                            {
                                var classteacherid = (from a in _PortalContext.ClassTeacherMappingDMO
                                                      from b in _PortalContext.HR_Master_Employee_DMO
                                                      from c in _PortalContext.School_M_Class
                                                      from d in _PortalContext.School_M_Section
                                                      where (a.MI_Id == c.MI_Id && a.HRME_Id == b.HRME_Id && a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == d.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == clsecids.FirstOrDefault().ASMCL_Id && a.ASMS_Id == clsecids.FirstOrDefault().ASMS_Id && a.IMCT_ActiveFlag == true && b.HRME_LeftFlag == false && c.ASMCL_ActiveFlag == true && d.ASMC_ActiveFlag == 1)
                                                      select new TransferCertificate_DTO
                                                      {
                                                          HRME_Id = a.HRME_Id
                                                      }
                                   ).Distinct().ToList();

                                if (classteacherid.Count > 0)
                                {
                                    var hrme_Id = classteacherid.FirstOrDefault().HRME_Id;

                                    var mobile1 = _PortalContext.Multiple_Mobile_DMO.Where(p => p.HRME_Id == hrme_Id && p.HRMEMNO_DeFaultFlag == "default").ToList();
                                    if (mobile1.Count > 0)
                                    {
                                        //  var mobilenumber1 = _PortalContext.Multiple_Mobile_DMO.Where(p => p.HRME_Id == data.HRME_Id && p.HRMEMNO_DeFaultFlag == "default").Distinct().ToList();
                                        mobilenoT = mobile1.FirstOrDefault().HRMEMNO_MobileNo;
                                        SMS sms = new SMS(_db);
                                        s = await sms.sendCertificateSms(data.MI_Id, mobilenoT, "LEAVEREQUEST", data.AMST_Id, hrme_Id, data.ASMAY_Id, obj.ASLA_Id);
                                    }
                                    var email1 = _PortalContext.Multiple_Email_DMO.Where(e => e.HRME_Id == hrme_Id && e.HRMEM_DeFaultFlag == "default").ToList();
                                    if (email1.Count > 0)
                                    {
                                        //var mailid1 = _PortalContext.Multiple_Email_DMO.Where(e => e.HRME_Id == data.HRME_Id && e.HRMEM_DeFaultFlag == "default").Distinct().ToList();
                                        emailidT = email1.FirstOrDefault().HRMEM_EmailId;
                                        Email Email = new Email(_db);
                                        m = Email.sendCertificatemail(data.MI_Id, emailidT, "LEAVEREQUEST", data.AMST_Id, hrme_Id, data.ASMAY_Id, obj.ASLA_Id);
                                    }
                                }
                            }

                            if (s == "success" || m == "success")
                            {
                                //data.returnval = true;
                            }
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public OnlineLeaveApp_DTO editdata(OnlineLeaveApp_DTO data)
        {
            try
            {
                data.editlist = _PortalContext.Adm_Students_Leave_Apply_DMO.Where(t => t.ASLA_Id == data.ASLA_Id && t.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public async Task<OnlineLeaveApp_DTO> leaveApproved(OnlineLeaveApp_DTO data)
        {
            try
            {
                string s = "";
                string m = "";
                if (data.ASLA_Id > 0)
                {
                    var aprovd = _PortalContext.Adm_Students_Leave_Apply_DMO.Where(t => t.ASLA_Id == data.ASLA_Id && t.MI_Id == data.MI_Id).SingleOrDefault();

                    aprovd.ASLA_ApprovedFromDate = data.ASLA_ApprovedFromDate;
                    aprovd.ASLA_ApprovedToDate = data.ASLA_ApprovedToDate;
                    aprovd.ASLA_Status = "Approved";
                    aprovd.UpdatedDate = DateTime.Now;
                    _PortalContext.Add(aprovd);

                    var update = _PortalContext.Adm_Students_Leave_Approval_DMO.Where(t => t.ASLA_Id == data.ASLA_Id && t.MI_Id == data.MI_Id).SingleOrDefault();

                    update.ASLA_Id = aprovd.ASLA_Id;
                    update.ASLAP_LeaveStatus = "Approved";
                    update.IVRMALU_Id = data.UserId;
                    update.ASLAP_AppRejDate = data.ASLAP_AppRejDate;
                    update.ASALP_RejectReason = data.ASALP_RejectReason;
                    update.ASLAP_AppFromDate = data.ASLAP_AppFromDate;
                    update.ASLAP_AppToDate = data.ASLAP_AppToDate;
                    update.UpdatedDate = DateTime.Now;
                    _PortalContext.Add(update);

                    Adm_Students_Approval_Process_DMO obj = new Adm_Students_Approval_Process_DMO();

                    obj.MI_Id = data.MI_Id;
                    obj.ASAP_ApprovalBy = Convert.ToString(data.UserId);
                    obj.ASAP_ActiveFlg = true;
                    obj.ASAP_ApprovalDate = data.ASAP_ApprovalDate;
                    obj.CreatedDate = DateTime.Now;
                    obj.UpdatedDate = DateTime.Now;
                    _PortalContext.Add(obj);

                    Adm_Students_Approval_Process_ClassSec_DMO obj2 = new Adm_Students_Approval_Process_ClassSec_DMO();

                    obj2.ASAP_Id = obj.ASAP_Id;
                    obj2.MI_Id = data.MI_Id;
                    obj2.ASMCL_Id = data.ASMCL_Id;
                    obj2.ASMS_Id = data.ASMS_Id;
                    obj2.ASAPCS_ActiveFlg = true;
                    obj2.CreatedDate = DateTime.Now;
                    obj2.UpdatedDate = DateTime.Now;
                    _PortalContext.Add(obj2);

                    _PortalContext.Update(aprovd);
                    _PortalContext.Update(update);

                    int rowAffected = _PortalContext.SaveChanges();

                    if (rowAffected > 0)
                    {
                        var asmstid = _PortalContext.Adm_Students_Leave_Apply_DMO.Single(a => a.ASLA_Id == data.ASLA_Id).AMST_Id;
                        var mobilemail = _PortalContext.Adm_M_Student.Where(a => a.MI_Id == data.MI_Id && a.AMST_Id == asmstid && a.AMST_ActiveFlag == 1 && a.AMST_SOL == "S").Distinct().ToList();

                        long mobileno = 0;
                        var emailid = "";
                        if (mobilemail.Count > 0)
                        {
                            mobileno = mobilemail.FirstOrDefault().AMST_MobileNo;
                            SMS sms = new SMS(_db);
                            s = await sms.sendCertificateSms(data.MI_Id, mobileno, "LEAVESTATUS", asmstid, data.HRME_Id, data.ASMAY_Id, data.ASLA_Id);
                            if (s == "success")
                            {
                                var clsecids = (from d in _PortalContext.AcademicYearDMO
                                                from a in _PortalContext.School_M_Class
                                                from b in _PortalContext.School_M_Section
                                                from c in _PortalContext.School_Adm_Y_StudentDMO
                                                where (a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.AMST_Id == asmstid)
                                                select new ExamDTO
                                                {
                                                    ASMCL_Id = c.ASMCL_Id,
                                                    ASMCL_ClassName = a.ASMCL_ClassName,
                                                    ASMS_Id = c.ASMS_Id,
                                                }
                           ).Distinct().ToList();

                                var classteacherid = (from a in _PortalContext.ClassTeacherMappingDMO
                                                      from b in _PortalContext.HR_Master_Employee_DMO
                                                      from c in _PortalContext.School_M_Class
                                                      from d in _PortalContext.School_M_Section
                                                      where (a.MI_Id == c.MI_Id && a.HRME_Id == b.HRME_Id && a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == d.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == clsecids.FirstOrDefault().ASMCL_Id && a.ASMS_Id == clsecids.FirstOrDefault().ASMS_Id && a.IMCT_ActiveFlag == true && b.HRME_LeftFlag == false && c.ASMCL_ActiveFlag == true && d.ASMC_ActiveFlag == 1)
                                                      select new TransferCertificate_DTO
                                                      {
                                                          HRME_Id = a.HRME_Id
                                                      }
                                   ).Distinct().ToList();
                                if (classteacherid.Count > 0)
                                {
                                    var hrme_Id = classteacherid.FirstOrDefault().HRME_Id;

                                    var mobile1 = _PortalContext.Multiple_Mobile_DMO.Where(p => p.HRME_Id == hrme_Id && p.HRMEMNO_DeFaultFlag == "default").ToList();
                                    if (mobile1.Count > 0)
                                    {
                                        mobileno = _PortalContext.Multiple_Mobile_DMO.Single(p => p.HRME_Id == hrme_Id && p.HRMEMNO_DeFaultFlag == "default").HRMEMNO_MobileNo;
                                        s = await sms.sendCertificateSms(data.MI_Id, mobileno, "LEAVESTATUS", asmstid, hrme_Id, data.ASMAY_Id, data.ASLA_Id);
                                    }
                                }

                            }
                        }
                        if (mobilemail.Count > 0)
                        {
                            emailid = mobilemail.FirstOrDefault().AMST_emailId;
                            Email Email = new Email(_db);
                            m = Email.sendCertificatemail(data.MI_Id, emailid, "LEAVESTATUS", asmstid, data.HRME_Id, data.ASMAY_Id, data.ASLA_Id);

                            if (m == "success")
                            {
                                var clsecids = (from d in _PortalContext.AcademicYearDMO
                                                from a in _PortalContext.School_M_Class
                                                from b in _PortalContext.School_M_Section
                                                from c in _PortalContext.School_Adm_Y_StudentDMO
                                                where (a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.AMST_Id == asmstid)
                                                select new ExamDTO
                                                {
                                                    ASMCL_Id = c.ASMCL_Id,
                                                    ASMCL_ClassName = a.ASMCL_ClassName,
                                                    ASMS_Id = c.ASMS_Id,
                                                }
                             ).Distinct().ToList();

                                var classteacherid = (from a in _PortalContext.ClassTeacherMappingDMO
                                                      from b in _PortalContext.HR_Master_Employee_DMO
                                                      from c in _PortalContext.School_M_Class
                                                      from d in _PortalContext.School_M_Section
                                                      where (a.MI_Id == c.MI_Id && a.HRME_Id == b.HRME_Id && a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == d.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == clsecids.FirstOrDefault().ASMCL_Id && a.ASMS_Id == clsecids.FirstOrDefault().ASMS_Id && a.IMCT_ActiveFlag == true && b.HRME_LeftFlag == false && c.ASMCL_ActiveFlag == true && d.ASMC_ActiveFlag == 1)
                                                      select new TransferCertificate_DTO
                                                      {
                                                          HRME_Id = a.HRME_Id
                                                      }
                                   ).Distinct().ToList();
                                if (classteacherid.Count > 0)
                                {
                                    var hrme_Id = classteacherid.FirstOrDefault().HRME_Id;

                                    var email1 = _PortalContext.Multiple_Email_DMO.Where(e => e.HRME_Id == hrme_Id && e.HRMEM_DeFaultFlag == "default").ToList();
                                    if (email1.Count > 0)
                                    {
                                        emailid = _PortalContext.Multiple_Email_DMO.Single(e => e.HRME_Id == hrme_Id && e.HRMEM_DeFaultFlag == "default").HRMEM_EmailId;
                                        m = Email.sendCertificatemail(data.MI_Id, emailid, "LEAVESTATUS", asmstid, hrme_Id, data.ASMAY_Id, data.ASLA_Id);
                                    }
                                }
                            }
                        }
                        if (s == "success" || m == "success")
                        {
                            data.returnval = true;
                        }
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<OnlineLeaveApp_DTO> leaveRejected(OnlineLeaveApp_DTO data)
        {
            try
            {
                string s = "";
                string m = "";
                if (data.ASLA_Id > 0)
                {
                    var aprovd = _PortalContext.Adm_Students_Leave_Apply_DMO.Where(t => t.ASLA_Id == data.ASLA_Id && t.MI_Id == data.MI_Id).SingleOrDefault();

                    aprovd.ASLA_Status = "Rejected";
                    aprovd.UpdatedDate = DateTime.Now;
                    _PortalContext.Update(aprovd);

                    var update = _PortalContext.Adm_Students_Leave_Approval_DMO.Where(t => t.ASLA_Id == data.ASLA_Id && t.MI_Id == data.MI_Id).SingleOrDefault();

                    update.ASLA_Id = aprovd.ASLA_Id;
                    update.ASLAP_AppRejDate = data.ASLAP_AppRejDate;
                    update.ASALP_RejectReason = data.ASALP_RejectReason;
                    update.ASLAP_AppFromDate = data.ASLAP_AppFromDate;
                    update.ASLAP_AppToDate = data.ASLAP_AppToDate;
                    update.ASLAP_LeaveStatus = "Rejected";
                    update.IVRMALU_Id = data.UserId;
                    update.UpdatedDate = DateTime.Now;
                    _PortalContext.Update(update);

                    int rowAffected = _PortalContext.SaveChanges();

                    if (rowAffected > 0)
                    {
                        var asmstid = _PortalContext.Adm_Students_Leave_Apply_DMO.Single(a => a.ASLA_Id == data.ASLA_Id).AMST_Id;
                        var mobilemail = _PortalContext.Adm_M_Student.Where(a => a.MI_Id == data.MI_Id && a.AMST_Id == asmstid && a.AMST_ActiveFlag == 1 && a.AMST_SOL == "S").Distinct().ToList();

                        long mobileno = 0;
                        var emailid = "";
                        if (mobilemail.Count > 0)
                        {
                            mobileno = mobilemail.FirstOrDefault().AMST_MobileNo;
                            SMS sms = new SMS(_db);
                            s = await sms.sendCertificateSms(data.MI_Id, mobileno, "LEAVESTATUS", asmstid, data.HRME_Id, data.ASMAY_Id, data.ASLA_Id);
                            if (s == "success")
                            {
                                var clsecids = (from d in _PortalContext.AcademicYearDMO
                                                from a in _PortalContext.School_M_Class
                                                from b in _PortalContext.School_M_Section
                                                from c in _PortalContext.School_Adm_Y_StudentDMO
                                                where (a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.AMST_Id == asmstid)
                                                select new ExamDTO
                                                {
                                                    ASMCL_Id = c.ASMCL_Id,
                                                    ASMCL_ClassName = a.ASMCL_ClassName,
                                                    ASMS_Id = c.ASMS_Id,
                                                }
                           ).Distinct().ToList();

                                var classteacherid = (from a in _PortalContext.ClassTeacherMappingDMO
                                                      from b in _PortalContext.HR_Master_Employee_DMO
                                                      from c in _PortalContext.School_M_Class
                                                      from d in _PortalContext.School_M_Section
                                                      where (a.MI_Id == c.MI_Id && a.HRME_Id == b.HRME_Id && a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == d.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == clsecids.FirstOrDefault().ASMCL_Id && a.ASMS_Id == clsecids.FirstOrDefault().ASMS_Id && a.IMCT_ActiveFlag == true && b.HRME_LeftFlag == false && c.ASMCL_ActiveFlag == true && d.ASMC_ActiveFlag == 1)
                                                      select new TransferCertificate_DTO
                                                      {
                                                          HRME_Id = a.HRME_Id
                                                      }
                                   ).Distinct().ToList();
                                if (classteacherid.Count > 0)
                                {
                                    var hrme_Id = classteacherid.FirstOrDefault().HRME_Id;

                                    var mobile1 = _PortalContext.Multiple_Mobile_DMO.Where(p => p.HRME_Id == hrme_Id && p.HRMEMNO_DeFaultFlag == "default").ToList();
                                    if (mobile1.Count > 0)
                                    {
                                        var mobilenumber = _PortalContext.Multiple_Mobile_DMO.Where(p => p.HRME_Id == data.HRME_Id && p.HRMEMNO_DeFaultFlag == "default").Distinct().ToList();
                                        mobileno = mobilenumber.FirstOrDefault().HRMEMNO_MobileNo;
                                        s = await sms.sendCertificateSms(data.MI_Id, mobileno, "LEAVESTATUS", asmstid, data.HRME_Id, data.ASMAY_Id, data.ASLA_Id);
                                    }
                                }

                            }
                        }
                        if (mobilemail.Count > 0)
                        {
                            emailid = mobilemail.FirstOrDefault().AMST_emailId;
                            Email Email = new Email(_db);
                            m = Email.sendCertificatemail(data.MI_Id, emailid, "LEAVESTATUS", asmstid, data.HRME_Id, data.ASMAY_Id, data.ASLA_Id);

                            if (m == "success")
                            {
                                var clsecids = (from d in _PortalContext.AcademicYearDMO
                                                from a in _PortalContext.School_M_Class
                                                from b in _PortalContext.School_M_Section
                                                from c in _PortalContext.School_Adm_Y_StudentDMO
                                                where (a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.AMST_Id == asmstid)
                                                select new ExamDTO
                                                {
                                                    ASMCL_Id = c.ASMCL_Id,
                                                    ASMCL_ClassName = a.ASMCL_ClassName,
                                                    ASMS_Id = c.ASMS_Id,
                                                }
                             ).Distinct().ToList();

                                var classteacherid = (from a in _PortalContext.ClassTeacherMappingDMO
                                                      from b in _PortalContext.HR_Master_Employee_DMO
                                                      from c in _PortalContext.School_M_Class
                                                      from d in _PortalContext.School_M_Section
                                                      where (a.MI_Id == c.MI_Id && a.HRME_Id == b.HRME_Id && a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == d.ASMS_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == clsecids.FirstOrDefault().ASMCL_Id && a.ASMS_Id == clsecids.FirstOrDefault().ASMS_Id && a.IMCT_ActiveFlag == true && b.HRME_LeftFlag == false && c.ASMCL_ActiveFlag == true && d.ASMC_ActiveFlag == 1)
                                                      select new TransferCertificate_DTO
                                                      {
                                                          HRME_Id = a.HRME_Id
                                                      }
                                   ).Distinct().ToList();
                                if (classteacherid.Count > 0)
                                {
                                    var hrme_Id = classteacherid.FirstOrDefault().HRME_Id;

                                    var email1 = _PortalContext.Multiple_Email_DMO.Where(e => e.HRME_Id == hrme_Id && e.HRMEM_DeFaultFlag == "default").ToList();
                                    if (email1.Count > 0)
                                    {
                                        var mailid1 = _PortalContext.Multiple_Email_DMO.Where(e => e.HRME_Id == data.HRME_Id && e.HRMEM_DeFaultFlag == "default").Distinct().ToList();
                                        emailid = mailid1.FirstOrDefault().HRMEM_EmailId;
                                        m = Email.sendCertificatemail(data.MI_Id, emailid, "LEAVESTATUS", asmstid, hrme_Id, data.ASMAY_Id, data.ASLA_Id);
                                    }
                                }
                            }
                        }
                        if (s == "success" && m == "success")
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public OnlineLeaveApp_DTO deactiveY(OnlineLeaveApp_DTO data)
        {
            try
            {
                var result = _PortalContext.Adm_Students_Leave_Apply_DMO.Single(t => t.MI_Id == data.MI_Id && t.ASLA_Id == data.ASLA_Id);

                if (result.ASLA_ActiveFlag == true)
                {
                    result.ASLA_ActiveFlag = false;
                }
                else if (result.ASLA_ActiveFlag == false)
                {
                    result.ASLA_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;
                _PortalContext.Update(result);
                int rowAffected = _PortalContext.SaveChanges();
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

        public OnlineLeaveApp_DTO cancellationRecord(OnlineLeaveApp_DTO data)
        {
            try
            {
                if (data.ASLA_Id > 0)
                {
                    var cancel = _PortalContext.Adm_Students_Leave_Apply_DMO.Where(t => t.ASLA_Id == data.ASLA_Id && t.MI_Id == data.MI_Id).SingleOrDefault();

                    cancel.AMST_Id = data.AMST_Id;
                    cancel.ASLA_Status = "Cancelled";
                    cancel.ASLA_Flag = data.ASLA_Flag;
                    cancel.UpdatedDate = DateTime.Now;

                    _PortalContext.Update(cancel);

                    var update2 = _PortalContext.Adm_Students_Leave_Approval_DMO.Where(t => t.MI_Id == data.MI_Id && t.ASLA_Id == cancel.ASLA_Id).SingleOrDefault();
                    update2.ASLAP_LeaveStatus = "Cancelled";
                    update2.UpdatedDate = DateTime.Now;
                    _PortalContext.Update(update2);

                    int rowAffected = _PortalContext.SaveChanges();
                    if (rowAffected > 0)
                    {
                        data.returnval = true;

                        var apply_id = _PortalContext.Adm_Students_Leave_Approval_DMO.Where(t => t.MI_Id == data.MI_Id && t.ASLA_Id == data.ASLA_Id).SingleOrDefault();
                        long apply_idss = apply_id.ASLA_Id;
                        deactiverecord(apply_idss);

                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public OnlineLeaveApp_DTO getdate_sla(OnlineLeaveApp_DTO dto)
        {
            try
            {

                dto.class_s_list = _PortalContext.School_M_Class.Where(a => a.MI_Id == dto.MI_Id && a.ASMCL_ActiveFlag == true).ToArray();

               

              
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public OnlineLeaveApp_DTO getsection(OnlineLeaveApp_DTO dto)
        {
            try
            {
                dto.section_s_list = (from a in _PortalContext.School_M_Section
                                      from b in _PortalContext.School_Adm_Y_StudentDMO
                                      where (a.MI_Id == dto.MI_Id && a.ASMS_Id == b.ASMS_Id && b.ASMCL_Id == dto.ASMCL_Id && a.MI_Id == dto.MI_Id /*&& b.ASMAY_Id==dto.ASMAY_Id*/)
                                      select new OnlineLeaveApp_DTO
                                      {
                                          ASMC_SectionName = a.ASMC_SectionName,
                                          ASMS_Id = a.ASMS_Id
                                      }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public OnlineLeaveApp_DTO getstudent(OnlineLeaveApp_DTO dto)
        {
            try
            {
                dto.student_s_list = (from a in _PortalContext.Adm_M_Student
                                      from b in _PortalContext.School_Adm_Y_StudentDMO
                                      where (a.MI_Id == dto.MI_Id && a.AMST_Id == b.AMST_Id && b.ASMCL_Id == dto.ASMCL_Id && b.ASMS_Id==dto.ASMS_Id  /*&& b.ASMAY_Id==dto.ASMAY_Id && a.ASMAY_Id==dto.ASMAY_Id*/)
                                      select new OnlineLeaveApp_DTO
                                      {
                                          AMST_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),
                                          AMST_Id = a.AMST_Id
                                      }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }
        public OnlineLeaveApp_DTO get_leave_Report(OnlineLeaveApp_DTO dto)
        {
            try
            {
                string stu_id = "0";

                List<long> stu_ids = new List<long>();


                foreach (var item in dto.student_id_list)
                {
                    stu_ids.Add(item.AMST_Id);

                }


                for (int s = 0; s < stu_ids.Count(); s++)
                {
                    stu_id = stu_id + ',' + stu_ids[s].ToString();
                }



                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_Student_leave_report_proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.ASMCL_Id)
                    });
                    
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.ASMS_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@stu_id", SqlDbType.VarChar)
                    {
                        Value = stu_id
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
                        dto.student_leave_list = retObject.ToArray();

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
            return dto;
        }

        public TransferCertificate_DTO get_TC_Report(TransferCertificate_DTO dto)
        {
            try
            {
                string stu_id = "0";

                List<long> stu_ids = new List<long>();


                foreach (var item in dto.student_id_list)
                {
                    stu_ids.Add(item.AMST_Id);

                }


                for (int s = 0; s < stu_ids.Count(); s++)
                {
                    stu_id = stu_id + ',' + stu_ids[s].ToString();
                }



                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "INV_Student_tc_report_proc";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.ASMCL_Id)
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(dto.ASMS_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@stu_id", SqlDbType.VarChar)
                    {
                        Value = stu_id
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
                        dto.student_tc_list = retObject.ToArray();

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
            return dto;
        }
        private void deactiverecord(long apply_idss)
        {

            var result = _PortalContext.Adm_Students_Leave_Apply_DMO.Single(t => t.ASLA_Id == apply_idss);

            if (result.ASLA_ActiveFlag == true)
            {
                result.ASLA_ActiveFlag = false;
            }
            else if (result.ASLA_ActiveFlag == false)
            {
                // result.ASLA_ActiveFlag = true;
            }
            result.UpdatedDate = DateTime.Now;
            _PortalContext.Update(result);
            int rowAffected = _PortalContext.SaveChanges();
            if (rowAffected > 0)
            {
                //data.returnval = true;
            }
            else
            {
                // data.returnval = false;
            }
            //throw new NotImplementedException();
        }
    }
}
