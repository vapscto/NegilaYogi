using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DomainModel;
using PreadmissionDTOs;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model;
using DataAccessMsSqlServerProvider;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using DomainModel.Model.com.vapstech.admission;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class ActivateDeactivateStudentImpl : Interfaces.ActivateDeactivateStudentInterface
    {
        private static ConcurrentDictionary<string, ActivateDeactivateStudentDTO> _login =
            new ConcurrentDictionary<string, ActivateDeactivateStudentDTO>();

        public ActivateDeactivateContext _ActivateDeactivateContext;
        private readonly UserManager<ApplicationUser> _UserManager;
        public ActivateDeactivateStudentImpl(ActivateDeactivateContext ActivateDeactivateContext, UserManager<ApplicationUser> UserManager)
        {
            _ActivateDeactivateContext = ActivateDeactivateContext;
            _UserManager = UserManager;
        }

        public ActivateDeactivateStudentDTO getdetails(int id)
        {
            ActivateDeactivateStudentDTO acdmc = new ActivateDeactivateStudentDTO();
            try
            {
                List<MasterAcademic> allacademic = new List<MasterAcademic>();

                allacademic = _ActivateDeactivateContext.academicYear.Where(y => y.Is_Active == true && y.MI_Id == id).OrderByDescending(y => y.ASMAY_Order).ToList();

                acdmc.yearfilllist = allacademic.Distinct().ToArray();

                List<School_M_Class> classlist = new List<School_M_Class>();
                classlist = _ActivateDeactivateContext.admissionClass.Where(c => c.MI_Id == id && c.ASMCL_ActiveFlag == true).ToList();
                acdmc.classfilllist = classlist.OrderBy(c => c.ASMCL_Order).Distinct().ToArray();

                List<School_M_Section> seclist = new List<School_M_Section>();

                seclist = _ActivateDeactivateContext.masterSection.Where(s => s.MI_Id == id && s.ASMC_ActiveFlag == 1).ToList();

                acdmc.sectionfilllist = seclist.OrderBy(s => s.ASMC_Order).Distinct().ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return acdmc;
        }
        public ActivateDeactivateStudentDTO getlistone(ActivateDeactivateStudentDTO acdmc)
        {
            // ActivateDeactivateStudentDTO acdmc = new ActivateDeactivateStudentDTO();
            try
            {
                List<School_M_Class> classlist = new List<School_M_Class>();
                acdmc.classfilllist = (from a in _ActivateDeactivateContext.masterclasscategory
                                       from b in _ActivateDeactivateContext.admissionClass

                                       where (a.ASMCL_Id == b.ASMCL_Id && a.Is_Active == true && b.ASMCL_ActiveFlag == true && a.ASMAY_Id == acdmc.yearid && a.MI_Id == acdmc.MI_Id)

                                       select new ActivateDeactivateStudentDTO
                                       {
                                           asmcL_Id = b.ASMCL_Id,
                                           asmcL_ClassName = b.ASMCL_ClassName,
                                           ASMCL_order = b.ASMCL_Order
                                       }).Distinct().OrderBy(a => a.ASMCL_order).ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return acdmc;
        }
        public ActivateDeactivateStudentDTO getlistthree(ActivateDeactivateStudentDTO acdmc)
        {
            // ActivateDeactivateStudentDTO acdmc = new ActivateDeactivateStudentDTO();
            try
            {
                List<ActivateDeactivateStudentDTO> classlist = new List<ActivateDeactivateStudentDTO>();
                acdmc.studentlist = (from a in _ActivateDeactivateContext.school_Adm_Y_StudentDMO
                                     from e in _ActivateDeactivateContext.ActivateDeactivateStudentDMO
                                     where (a.AMST_Id == e.AMST_Id && a.ASMAY_Id == acdmc.yearid
                                     && a.ASMCL_Id == acdmc.asmcL_Id && a.ASMS_Id == acdmc.sectionid
                                     && e.AMST_SOL == acdmc.AMST_SOL && a.AMAY_ActiveFlag == 1 && e.MI_Id == acdmc.MI_Id)
                                     select new ActivateDeactivateStudentDTO
                                     {
                                         AMST_Id = a.AMST_Id,
                                         name = ((e.AMST_FirstName == null || e.AMST_FirstName == "" ? "" : " " + e.AMST_FirstName) + (e.AMST_MiddleName == null || e.AMST_MiddleName == "" || e.AMST_MiddleName == "0" ? "" : " " + e.AMST_MiddleName) + (e.AMST_LastName == null || e.AMST_LastName == "" || e.AMST_LastName == "0" ? "" : " " + e.AMST_LastName)).Trim(),
                                         regno = e.AMST_RegistrationNo,
                                         admno = e.AMST_AdmNo
                                     }).Distinct().ToArray();
                if (acdmc.studentlist.Length > 0)
                {
                    acdmc.count = acdmc.studentlist.Length;
                }
                else
                {
                    acdmc.count = 0;
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return acdmc;
        }
        public async Task<ActivateDeactivateStudentDTO> getlisttwo(ActivateDeactivateStudentDTO stu)
        {
            ActivateDeactivateStudentDTO acdmc = new ActivateDeactivateStudentDTO();
            try
            {
                if (stu.savetmpdata.Count() > 0)
                {
                    foreach (ActivateDeactivateStudentDTO ph in stu.savetmpdata)
                    {
                        try
                        {
                            if (ph.checkedvalue == true)
                            {
                                var Phone_Noresult = _ActivateDeactivateContext.ActivateDeactivateStudentDMO.Single(t => t.AMST_Id == ph.AMST_Id);
                                Phone_Noresult.AMST_SOL = stu.AMST_SOL_activate;
                                _ActivateDeactivateContext.Update(Phone_Noresult);

                                var check_alreadyexists = _ActivateDeactivateContext.Adm_Student_Deactivate_Active_ReasonDMO.Where(a => a.MI_Id == stu.MI_Id &&
                                a.ASMAY_Id == stu.yearid && a.ASMCL_Id == stu.asmcL_Id && a.ASMS_Id == stu.sectionid && a.AMST_Id == ph.AMST_Id && a.ASDE_ActivedFlg == false && a.ASDE_ActiveFlag == true).ToList();

                                if (check_alreadyexists.Count() > 0)
                                {
                                    var check_alreadyexistsupdate = _ActivateDeactivateContext.Adm_Student_Deactivate_Active_ReasonDMO.Single(a => a.MI_Id == stu.MI_Id &&
                                a.ASMAY_Id == stu.yearid && a.ASMCL_Id == stu.asmcL_Id && a.ASMS_Id == stu.sectionid && a.AMST_Id == ph.AMST_Id && a.ASDE_ActivedFlg == false && a.ASDE_ActiveFlag == true);

                                    check_alreadyexistsupdate.ASDE_ActivatedReason = ph.reason+"\n"+ph.remarks;
                                    check_alreadyexistsupdate.ASDE_ActivatedDate = DateTime.Now;
                                    check_alreadyexistsupdate.ASDE_UpdatedBy = stu.userid;
                                    check_alreadyexistsupdate.UpdatedDate = DateTime.Now;
                                    check_alreadyexistsupdate.ASDE_ActivedFlg = true;
                                    _ActivateDeactivateContext.Update(check_alreadyexistsupdate);
                                }
                                else
                                {
                                    Adm_Student_Deactivate_Active_ReasonDMO rema = new Adm_Student_Deactivate_Active_ReasonDMO();
                                    rema.MI_Id = stu.MI_Id;
                                    rema.ASMAY_Id = stu.yearid;
                                    rema.ASMCL_Id = stu.asmcL_Id;
                                    rema.ASMS_Id = stu.sectionid;
                                    rema.AMST_Id = ph.AMST_Id;
                                    if (stu.AMST_SOL_activate == "D")
                                    {
                                        rema.ASDE_DeactivatedReason = ph.remarks;
                                        rema.ASDE_DeactivatedDate = DateTime.Now;
                                        rema.ASDE_ActivatedReason = "";
                                        rema.ASDE_ActivedFlg = false;
                                    }
                                    else
                                    {
                                        rema.ASDE_ActivatedReason = ph.remarks;
                                        rema.ASDE_ActivatedDate = DateTime.Now;
                                        rema.ASDE_DeactivatedReason = "";
                                        rema.ASDE_ActivedFlg = true;
                                    }
                                    rema.ASDE_ActiveFlag = true;
                                    rema.ASDE_CreatedBy = stu.userid;
                                    rema.ASDE_UpdatedBy = stu.userid;
                                    rema.CreatedDate = DateTime.Now;
                                    rema.UpdatedDate = DateTime.Now;

                                    _ActivateDeactivateContext.Add(rema);
                                }



                                var contactExists = _ActivateDeactivateContext.SaveChanges();

                                if (contactExists > 0)
                                {
                                    stu.returnval = true;

                                    var getstdappid = _ActivateDeactivateContext.StudentAppUserLoginDMO.Where(a => a.AMST_ID == ph.AMST_Id).Select(a => a.STD_APP_ID).ToList();

                                    var chckuser = _ActivateDeactivateContext.UserRoleWithInstituteDMO.Where(a => a.Id == getstdappid[0]).ToList();

                                    if (chckuser.Count() > 0)
                                    {
                                        var updatecheckuser = _ActivateDeactivateContext.UserRoleWithInstituteDMO.Single(a => a.Id == getstdappid[0]);

                                        if (stu.AMST_SOL_activate == "D")
                                        {
                                            updatecheckuser.Activeflag = 0;
                                        }
                                        else
                                        {
                                            updatecheckuser.Activeflag = 1;
                                        }

                                        _ActivateDeactivateContext.Update(updatecheckuser);
                                        var i = _ActivateDeactivateContext.SaveChanges();
                                        if (i > 0)
                                        {
                                            stu.returnval = true;
                                        }
                                        else
                                        {
                                            stu.returnval = true;
                                        }
                                    }
                                }
                                else
                                {
                                    stu.returnval = false;
                                }
                            }
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }                        
                    }
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return stu;
        }
    }
}
