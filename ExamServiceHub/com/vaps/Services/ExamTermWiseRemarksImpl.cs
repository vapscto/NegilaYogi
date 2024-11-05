using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model.com.vapstech.Exam;
using DomainModel.Model.com.vapstech.MobileApp;
using PreadmissionDTOs.com.vaps.Exam;
namespace ExamServiceHub.com.vaps.Services
{
    public class ExamTermWiseRemarksImpl : Interfaces.ExamTermWiseRemarksInterface
    {
        public ExamContext _context;
        DomainModelMsSqlServerContext _dbd;
        public ExamTermWiseRemarksImpl(ExamContext _cont, DomainModelMsSqlServerContext dbd)
        {
            _context = _cont;
            _dbd = dbd;
        }
        public ExamTermWiseRemarksDTO Getdetails(ExamTermWiseRemarksDTO data)
        {
            try
            {
                data.getyear = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.getdetails = (from a in _context.ExamTermWiseRemarksDMO
                                   from b in _context.CCE_Exam_M_TermsDMO
                                   from c in _context.AcademicYear
                                   from d in _context.AdmissionClass
                                   from e in _context.School_M_Section
                                   where (a.ECT_Id == b.ECT_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                   && a.ECTERE_ActiveFlag == true && a.MI_Id == data.MI_Id)
                                   select new ExamTermWiseRemarksDTO
                                   {
                                       ECT_Id = a.ECT_Id,
                                       ASMCL_Id = a.ASMCL_Id,
                                       ASMS_Id = a.ASMS_Id,
                                       ASMAY_Id = a.ASMAY_Id,
                                       ECTERE_Indi_OverAllFlag = a.ECTERE_Indi_OverAllFlag
                                   }).Distinct().ToArray();

                if (data.stringmobileorportal == "Mobile")
                {
                    List<IVRM_User_MobileApp_Login_Privileges> Staffmobileappprivileges = new List<IVRM_User_MobileApp_Login_Privileges>();
                    Staffmobileappprivileges = _dbd.IVRM_User_MobileApp_Login_Privileges.Where(t => t.IVRMUL_Id == data.User_Id && t.MI_Id == data.MI_Id).ToList();

                    if (Staffmobileappprivileges.Count() > 0)
                    {
                        data.Staffmobileappprivileges = (from Mobilepage in _dbd.IVRM_MobileApp_Page
                                                         from MobileRolePrivileges in _dbd.IVRM_Role_MobileApp_Privileges
                                                         from UserRolePrivileges in _dbd.IVRM_User_MobileApp_Login_Privileges
                                                         where (MobileRolePrivileges.MI_ID == UserRolePrivileges.MI_Id
                                                         && Mobilepage.IVRMMAP_Id == MobileRolePrivileges.IVRMMAP_Id
                                                         && Mobilepage.IVRMMAP_Id == UserRolePrivileges.IVRMMAP_Id
                                                         && MobileRolePrivileges.IVRMRT_Id == data.roleid
                                                         && MobileRolePrivileges.MI_ID == data.MI_Id && UserRolePrivileges.IVRMUL_Id == data.User_Id)
                                                         select new StudentTransactionDTO
                                                         {
                                                             Pagename = Mobilepage.IVRMMAP_AppPageName,
                                                             Pageicon = Mobilepage.IVRMMAP_AppPageDesc,
                                                             Pageurl = Mobilepage.IVRMMAP_AppPageURL,
                                                             IVRMRMAP_Id = MobileRolePrivileges.IVRMRMAP_Id,
                                                             IVRMMAP_AddFlg = UserRolePrivileges.IVRMUMALP_AddFlg,
                                                             IVRMMAP_UpdateFlg = UserRolePrivileges.IVRMUMALP_UpdateFlg,
                                                             IVRMMAP_DeleteFlg = UserRolePrivileges.IVRMUMALP_DeleteFlg
                                                         }).OrderBy(d => d.IVRMRMAP_Id).ToArray();

                        data.mobileprivileges = "true";
                    }
                    else
                    {
                        data.mobileprivileges = "false";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamTermWiseRemarksDTO get_class(ExamTermWiseRemarksDTO data)
        {
            try
            {
                var check_rolename = (from a in _context.MasterRoleType
                                      where (a.IVRMRT_Id == data.roleid)
                                      select new ExamTermWiseRemarksDTO
                                      {
                                          rolename = a.IVRMRT_Role.ToUpper(),
                                      }).ToList();

                var empcode_check = (from a in _context.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.Id.Equals(data.User_Id))
                                     select new ExamTermWiseRemarksDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                if (check_rolename.FirstOrDefault().rolename.Equals("STAFF"))
                {
                    data.getclass = (from a in _context.ClassTeacherMappingDMO
                                     from c in _context.AdmissionClass
                                     where (c.ASMCL_Id == a.ASMCL_Id
                                     && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                     && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                     && c.ASMCL_ActiveFlag == true && a.IMCT_ActiveFlag == true)
                                     select c).Distinct().OrderBy(c => c.ASMCL_Order).ToArray();
                }
                else
                {
                    data.getclass = _context.AdmissionClass.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true).OrderBy(a => a.ASMCL_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamTermWiseRemarksDTO get_section(ExamTermWiseRemarksDTO data)
        {
            try
            {
                var check_rolename = (from a in _context.MasterRoleType
                                      where (a.IVRMRT_Id == data.roleid)
                                      select new ExamTermWiseRemarksDTO
                                      {
                                          rolename = a.IVRMRT_Role.ToUpper(),
                                      }).ToList();

                var empcode_check = (from a in _context.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.Id.Equals(data.User_Id))
                                     select new ExamTermWiseRemarksDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                if (check_rolename.FirstOrDefault().rolename.Equals("STAFF"))
                {
                    data.getsection = (from a in _context.ClassTeacherMappingDMO
                                       from c in _context.AdmissionClass
                                       from d in _context.School_M_Section
                                       where (c.ASMCL_Id == a.ASMCL_Id && a.ASMS_Id == d.ASMS_Id
                                       && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                                       && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                       && c.ASMCL_ActiveFlag == true && a.IMCT_ActiveFlag == true)
                                       select d).Distinct().OrderBy(d => d.ASMC_Order).ToArray();
                }
                else
                {
                    data.getsection = _context.School_M_Section.Where(a => a.MI_Id == data.MI_Id && a.ASMC_ActiveFlag == 1).OrderBy(a => a.ASMC_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamTermWiseRemarksDTO get_term(ExamTermWiseRemarksDTO data)
        {
            try
            {
                var getemcaid = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).Distinct().ToArray();

                data.getterm = _context.CCE_Exam_M_TermsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ECT_ActiveFlag == true && a.EMCA_Id == getemcaid.FirstOrDefault().EMCA_Id).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamTermWiseRemarksDTO search_student(ExamTermWiseRemarksDTO data)
        {
            try
            {
                string order = "";
                var get_configuration = _context.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
                order = "studentname";
                if (get_configuration!= null && get_configuration.Count > 0)
                {
                    if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "Name")
                    {
                        order = "studentname";
                    }
                    else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "AdmNo")
                    {
                        order = "admno";
                    }
                    else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RollNo")
                    {
                        order = "rollno";
                    }
                    else if (get_configuration.FirstOrDefault().ExmConfig_Recordsearchtype == "RegNo")
                    {
                        order = "regno";
                    }                   
                }
                List<ExamTermWiseRemarksDTO> studentList = new List<ExamTermWiseRemarksDTO>();
                studentList = (from a in _context.School_Adm_Y_Student
                                          from b in _context.Adm_M_Student
                                          from c in _context.AcademicYear
                                          from d in _context.AdmissionClass
                                          from e in _context.School_M_Section
                                          where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                          && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1 && a.ASMAY_Id == data.ASMAY_Id
                                          && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id)
                                          select new ExamTermWiseRemarksDTO
                                          {
                                              AMST_Id = b.AMST_Id,
                                              admno = b.AMST_AdmNo,
                                              regno = b.AMST_AdmNo,
                                              rollno = a.AMAY_RollNo,
                                              studentname = ((b.AMST_FirstName == null || b.AMST_FirstName == "" ? "" : b.AMST_FirstName) +
                                              (b.AMST_MiddleName == null || b.AMST_MiddleName == "" ? "" : " " + b.AMST_MiddleName) +
                                              (b.AMST_LastName == null || b.AMST_LastName == "" ? " " : " " + b.AMST_LastName)).Trim()
                                          }).Distinct().OrderBy(t => order).ToList();


                var propertyInfo = typeof(ExamTermWiseRemarksDTO).GetProperty(order);
                data.getstudentdetails = studentList.OrderBy(x => propertyInfo.GetValue(x, null)).ToArray();

                if (data.indiorfinal == "IE")
                {
                    data.getsavedetails = _context.ExamTermWiseRemarksDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                    && a.ASMS_Id == data.ASMS_Id && a.MI_Id == data.MI_Id && a.ECT_Id == data.ECT_Id && a.ECTERE_ActiveFlag == true
                    && a.ECTERE_Indi_OverAllFlag == "IE").Distinct().ToArray();
                }
                else
                {
                    data.getsavedetails = _context.ExamTermWiseRemarksDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                    && a.ASMS_Id == data.ASMS_Id && a.MI_Id == data.MI_Id && a.ECTERE_ActiveFlag == true && a.ECTERE_Indi_OverAllFlag != "IE").Distinct().ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamTermWiseRemarksDTO save_details(ExamTermWiseRemarksDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                if (data.indiorfinal == "IE")
                {
                    if (data.saved_remarks_details.Length > 0)
                    {
                        foreach (var c in data.saved_remarks_details)
                        {
                            var checkdata = _context.ExamTermWiseRemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                            && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.AMST_Id == c.AMST_Id && a.ECT_Id == data.ECT_Id
                            && a.ECTERE_Indi_OverAllFlag == "IE").ToList();

                            if (checkdata.Count > 0)
                            {
                                var checkresult = _context.ExamTermWiseRemarksDMO.Single(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.AMST_Id == c.AMST_Id && a.ECT_Id == data.ECT_Id
                                && a.ECTERE_Indi_OverAllFlag == "IE");

                                checkresult.ECTERE_Remarks = c.ECTERE_Remarks;
                                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                                checkresult.UpdatedDate = indianTime;
                                checkresult.ECTERE_UpdatedBy = data.User_Id;
                                _context.Update(checkresult);
                            }
                            else
                            {
                                ExamTermWiseRemarksDMO dmo = new ExamTermWiseRemarksDMO();

                                dmo.MI_Id = data.MI_Id;
                                dmo.ASMAY_Id = data.ASMAY_Id;
                                dmo.ASMCL_Id = data.ASMCL_Id;
                                dmo.ASMS_Id = data.ASMS_Id;
                                dmo.ECT_Id = data.ECT_Id;
                                dmo.AMST_Id = c.AMST_Id;
                                dmo.ECTERE_Remarks = c.ECTERE_Remarks;
                                dmo.ECTERE_ActiveFlag = true;
                                dmo.ECTERE_Indi_OverAllFlag = "IE";
                                dmo.ECTERE_CreatedBy = data.User_Id;
                                dmo.ECTERE_UpdatedBy = data.User_Id;
                                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                                dmo.CreatedDate = indianTime;
                                dmo.UpdatedDate = indianTime;
                                _context.Add(dmo);
                            }
                        }
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else if (data.indiorfinal == "PE")
                {
                    if (data.saved_remarks_details.Length > 0)
                    {
                        foreach (var c in data.saved_remarks_details)
                        {
                            var checkdata = _context.ExamTermWiseRemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                            && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.AMST_Id == c.AMST_Id && a.ECTERE_Indi_OverAllFlag == "PE").ToList();

                            if (checkdata.Count > 0)
                            {
                                var checkresult = _context.ExamTermWiseRemarksDMO.Single(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.AMST_Id == c.AMST_Id && a.ECTERE_Indi_OverAllFlag == "PE");

                                checkresult.ECTERE_Remarks = c.ECTERE_Remarks;
                                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                                checkresult.UpdatedDate = indianTime;
                                checkresult.ECTERE_UpdatedBy = data.User_Id;
                                _context.Update(checkresult);
                            }
                            else
                            {
                                ExamTermWiseRemarksDMO dmo = new ExamTermWiseRemarksDMO();

                                dmo.MI_Id = data.MI_Id;
                                dmo.ASMAY_Id = data.ASMAY_Id;
                                dmo.ASMCL_Id = data.ASMCL_Id;
                                dmo.ASMS_Id = data.ASMS_Id;
                                dmo.ECT_Id = 0;
                                dmo.AMST_Id = c.AMST_Id;
                                dmo.ECTERE_Remarks = c.ECTERE_Remarks;
                                dmo.ECTERE_ActiveFlag = true;
                                dmo.ECTERE_Indi_OverAllFlag = "PE";
                                dmo.ECTERE_CreatedBy = data.User_Id;
                                dmo.ECTERE_UpdatedBy = data.User_Id;
                                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                                dmo.CreatedDate = indianTime;
                                dmo.UpdatedDate = indianTime;
                                _context.Add(dmo);
                            }
                        }
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = false;
            }
            return data;
        }
        public ExamTermWiseRemarksDTO save_Group_wise_details(ExamTermWiseRemarksDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                if (data.indiorfinal == "IE")
                {
                    if (data.saved_remarks_details.Length > 0)
                    {
                        foreach (var c in data.saved_remarks_details)
                        {
                            var checkdata = _context.ExamTermWiseRemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                            && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.AMST_Id == c.AMST_Id && a.ECT_Id == data.ECT_Id
                            && a.ECTERE_Indi_OverAllFlag == "IE").ToList();

                            if (checkdata.Count > 0)
                            {
                                var checkresult = _context.ExamTermWiseRemarksDMO.Single(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.AMST_Id == c.AMST_Id && a.ECT_Id == data.ECT_Id
                                && a.ECTERE_Indi_OverAllFlag == "IE");

                                checkresult.ECTERE_Remarks = c.ECTERE_Remarks;
                                checkresult.ECTERE_Conduct = c.ECTERE_Conduct;
                                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                                checkresult.UpdatedDate = indianTime;
                                checkresult.ECTERE_UpdatedBy = data.User_Id;
                                _context.Update(checkresult);
                            }
                            else
                            {
                                ExamTermWiseRemarksDMO dmo = new ExamTermWiseRemarksDMO();

                                dmo.MI_Id = data.MI_Id;
                                dmo.ASMAY_Id = data.ASMAY_Id;
                                dmo.ASMCL_Id = data.ASMCL_Id;
                                dmo.ASMS_Id = data.ASMS_Id;
                                dmo.ECT_Id = data.ECT_Id;
                                dmo.AMST_Id = c.AMST_Id;
                                dmo.ECTERE_Remarks = c.ECTERE_Remarks;
                                dmo.ECTERE_ActiveFlag = true;
                                dmo.ECTERE_Indi_OverAllFlag = "IE";
                                dmo.ECTERE_CreatedBy = data.User_Id;
                                dmo.ECTERE_UpdatedBy = data.User_Id;
                                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                                dmo.CreatedDate = indianTime;
                                dmo.UpdatedDate = indianTime;
                                dmo.ECTERE_Conduct = c.ECTERE_Conduct;
                                _context.Add(dmo);
                            }
                        }
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else if (data.indiorfinal == "PE")
                {
                    if (data.saved_remarks_details.Length > 0)
                    {
                        foreach (var c in data.saved_remarks_details)
                        {
                            var checkdata = _context.ExamTermWiseRemarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                            && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.AMST_Id == c.AMST_Id && a.ECTERE_Indi_OverAllFlag == "PE").ToList();

                            if (checkdata.Count > 0)
                            {
                                var checkresult = _context.ExamTermWiseRemarksDMO.Single(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.AMST_Id == c.AMST_Id && a.ECTERE_Indi_OverAllFlag == "PE");

                                checkresult.ECTERE_Remarks = c.ECTERE_Remarks;
                                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                                checkresult.UpdatedDate = indianTime;
                                checkresult.ECTERE_UpdatedBy = data.User_Id;
                                _context.Update(checkresult);
                            }
                            else
                            {
                                ExamTermWiseRemarksDMO dmo = new ExamTermWiseRemarksDMO();

                                dmo.MI_Id = data.MI_Id;
                                dmo.ASMAY_Id = data.ASMAY_Id;
                                dmo.ASMCL_Id = data.ASMCL_Id;
                                dmo.ASMS_Id = data.ASMS_Id;
                                dmo.ECT_Id = 0;
                                dmo.AMST_Id = c.AMST_Id;
                                dmo.ECTERE_Remarks = c.ECTERE_Remarks;
                                dmo.ECTERE_ActiveFlag = true;
                                dmo.ECTERE_Indi_OverAllFlag = "PE";
                                dmo.ECTERE_CreatedBy = data.User_Id;
                                dmo.ECTERE_UpdatedBy = data.User_Id;
                                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                                dmo.CreatedDate = indianTime;
                                dmo.UpdatedDate = indianTime;
                                _context.Add(dmo);
                            }
                        }
                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = false;
            }
            return data;
        }
        public ExamTermWiseRemarksDTO edit_details(ExamTermWiseRemarksDTO data)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Term Wise Participate
        public ExamTermWiseRemarksDTO Getdetails_Participate(ExamTermWiseRemarksDTO data)
        {
            try
            {
                data.getyear = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.getdetails = (from a in _context.Exm_Student_TermAchievementsDMO
                                   from b in _context.CCE_Exam_M_TermsDMO
                                   from c in _context.AcademicYear
                                   from d in _context.AdmissionClass
                                   from e in _context.School_M_Section
                                   where (a.ECT_Id == b.ECT_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                   && a.ESTTA_ActiveFlag == true && a.MI_Id == data.MI_Id)
                                   select new ExamTermWiseRemarksDTO
                                   {
                                       ECT_Id = a.ECT_Id,
                                       ASMCL_Id = a.ASMCL_Id,
                                       ASMS_Id = a.ASMS_Id,
                                       ASMAY_Id = a.ASMAY_Id,
                                       ECT_TermName = b.ECT_TermName,
                                       ASMAY_Year = c.ASMAY_Year,
                                       ASMCL_ClassName = d.ASMCL_ClassName,
                                       ASMS_SectionName = e.ASMC_SectionName

                                   }).Distinct().ToArray();

                if (data.stringmobileorportal == "Mobile")
                {
                    List<IVRM_User_MobileApp_Login_Privileges> Staffmobileappprivileges = new List<IVRM_User_MobileApp_Login_Privileges>();
                    Staffmobileappprivileges = _dbd.IVRM_User_MobileApp_Login_Privileges.Where(t => t.IVRMUL_Id == data.User_Id && t.MI_Id == data.MI_Id).ToList();

                    if (Staffmobileappprivileges.Count() > 0)
                    {
                        data.Staffmobileappprivileges = (from Mobilepage in _dbd.IVRM_MobileApp_Page
                                                         from MobileRolePrivileges in _dbd.IVRM_Role_MobileApp_Privileges
                                                         from UserRolePrivileges in _dbd.IVRM_User_MobileApp_Login_Privileges
                                                         where (MobileRolePrivileges.MI_ID == UserRolePrivileges.MI_Id
                                                         && Mobilepage.IVRMMAP_Id == MobileRolePrivileges.IVRMMAP_Id
                                                         && Mobilepage.IVRMMAP_Id == UserRolePrivileges.IVRMMAP_Id
                                                         && MobileRolePrivileges.IVRMRT_Id == data.roleid
                                                         && MobileRolePrivileges.MI_ID == data.MI_Id && UserRolePrivileges.IVRMUL_Id == data.User_Id)
                                                         select new StudentTransactionDTO
                                                         {
                                                             Pagename = Mobilepage.IVRMMAP_AppPageName,
                                                             Pageicon = Mobilepage.IVRMMAP_AppPageDesc,
                                                             Pageurl = Mobilepage.IVRMMAP_AppPageURL,
                                                             IVRMRMAP_Id = MobileRolePrivileges.IVRMRMAP_Id,
                                                             IVRMMAP_AddFlg = UserRolePrivileges.IVRMUMALP_AddFlg,
                                                             IVRMMAP_UpdateFlg = UserRolePrivileges.IVRMUMALP_UpdateFlg,
                                                             IVRMMAP_DeleteFlg = UserRolePrivileges.IVRMUMALP_DeleteFlg
                                                         }).OrderBy(d => d.IVRMRMAP_Id).ToArray();

                        data.mobileprivileges = "true";
                    }
                    else
                    {
                        data.mobileprivileges = "false";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamTermWiseRemarksDTO search_student_participate(ExamTermWiseRemarksDTO data)
        {
            try
            {
                data.getstudentdetails = (from a in _context.School_Adm_Y_Student
                                          from b in _context.Adm_M_Student
                                          from c in _context.AcademicYear
                                          from d in _context.AdmissionClass
                                          from e in _context.School_M_Section
                                          where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == e.ASMS_Id
                                          && a.AMAY_ActiveFlag == 1 && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1 && a.ASMAY_Id == data.ASMAY_Id
                                          && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id)
                                          select new ExamTermWiseRemarksDTO
                                          {
                                              AMST_Id = b.AMST_Id,
                                              admno = b.AMST_AdmNo,
                                              regno = b.AMST_AdmNo,
                                              rollno = a.AMAY_RollNo,
                                              studentname = ((b.AMST_FirstName == null || b.AMST_FirstName == "" ? "" : b.AMST_FirstName) +
                                              (b.AMST_MiddleName == null || b.AMST_MiddleName == "" ? "" : " " + b.AMST_MiddleName) +
                                              (b.AMST_LastName == null || b.AMST_LastName == "" ? " " : " " + b.AMST_LastName)).Trim()
                                          }).Distinct().OrderBy(a => a.rollno).ToArray();


                data.getsavedetails = _context.Exm_Student_TermAchievementsDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                && a.ASMS_Id == data.ASMS_Id && a.MI_Id == data.MI_Id && a.ESTTA_ActiveFlag == true && a.ECT_Id == data.ECT_Id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamTermWiseRemarksDTO save_participate_details(ExamTermWiseRemarksDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                if (data.saved_participate_details.Length > 0)
                {
                    List<long> ids = new List<long>();

                    foreach (var d in data.saved_participate_details)
                    {
                        ids.Add(d.AMST_Id);
                    }

                    var RemoveDetails = _context.Exm_Student_TermAchievementsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ECT_Id == data.ECT_Id && !ids.Contains(a.AMST_Id)).ToArray();

                    foreach (var e in RemoveDetails)
                    {
                        _context.Remove(e);
                    }

                    foreach (var c in data.saved_participate_details)
                    {
                        if (c.ESTTA_Id > 0)
                        {
                            var result = _context.Exm_Student_TermAchievementsDMO.Single(a => a.MI_Id == data.MI_Id && a.ESTTA_Id == c.ESTTA_Id);
                            result.ESTTA_Remarks = c.ESTTA_Remarks;
                            result.ESTTA_Updatedby = data.User_Id;
                            result.ESTTA_UpdatedDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                            _context.Update(result);
                        }
                        else
                        {
                            Exm_Student_TermAchievementsDMO exm_Student_TermAchievementsDMO = new Exm_Student_TermAchievementsDMO();
                            exm_Student_TermAchievementsDMO.MI_Id = data.MI_Id;
                            exm_Student_TermAchievementsDMO.ASMAY_Id = data.ASMAY_Id;
                            exm_Student_TermAchievementsDMO.ASMCL_Id = data.ASMCL_Id;
                            exm_Student_TermAchievementsDMO.ASMS_Id = data.ASMS_Id;
                            exm_Student_TermAchievementsDMO.ECT_Id = data.ECT_Id;
                            exm_Student_TermAchievementsDMO.AMST_Id = c.AMST_Id;
                            exm_Student_TermAchievementsDMO.ESTTA_Remarks = c.ESTTA_Remarks;
                            exm_Student_TermAchievementsDMO.ESTTA_ActiveFlag = true;
                            exm_Student_TermAchievementsDMO.ESTTA_Createdby = data.User_Id;
                            exm_Student_TermAchievementsDMO.ESTTA_CreatedDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                            exm_Student_TermAchievementsDMO.ESTTA_Updatedby = data.User_Id;
                            exm_Student_TermAchievementsDMO.ESTTA_UpdatedDate = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                            _context.Add(exm_Student_TermAchievementsDMO);
                        }
                    }

                    var i = _context.SaveChanges();
                    if (i > 0)
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
                data.returnval = false;
            }
            return data;
        }
        public ExamTermWiseRemarksDTO ViewStudentParticipateDetails(ExamTermWiseRemarksDTO data)
        {
            try
            {

                data.viewstudentdetails = (from a in _context.Exm_Student_TermAchievementsDMO
                                           from b in _context.Adm_M_Student
                                           where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id
                                           && a.MI_Id == data.MI_Id && a.ECT_Id == data.ECT_Id)
                                           select new ExamTermWiseRemarksDTO
                                           {
                                               AMST_Id = b.AMST_Id,
                                               admno = b.AMST_AdmNo,
                                               regno = b.AMST_AdmNo,
                                               studentname = ((b.AMST_FirstName == null || b.AMST_FirstName == "" ? "" : b.AMST_FirstName) +
                                              (b.AMST_MiddleName == null || b.AMST_MiddleName == "" ? "" : " " + b.AMST_MiddleName) +
                                              (b.AMST_LastName == null || b.AMST_LastName == "" ? " " : " " + b.AMST_LastName)).Trim(),
                                               ESTTA_Remarks = a.ESTTA_Remarks,
                                               ESTTA_Id = a.ESTTA_Id,
                                               ESTTA_ActiveFlag = a.ESTTA_ActiveFlag

                                           }).Distinct().OrderBy(a => a.studentname).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = false;
            }
            return data;
        }
    }
}