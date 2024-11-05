using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vaps.Exam;
using DomainModel.Model.com.vapstech.MobileApp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Services
{
    public class ExamWiseTermReportImpl : Interfaces.ExamWiseTermReportInterface
    {
        private static ConcurrentDictionary<string, ExamWiseTermReport_DTO> _login =
     new ConcurrentDictionary<string, ExamWiseTermReport_DTO>();

        private readonly ExamContext _PCReportContext;
        public StudentAttendanceReportContext _db;
        ILogger<ExamWiseTermReportImpl> _acdimpl;
        DomainModelMsSqlServerContext _dbd;
        public ExamWiseTermReportImpl(ExamContext cpContext, StudentAttendanceReportContext db, DomainModelMsSqlServerContext dbd, ILogger<ExamWiseTermReportImpl> _acdim)
        {
            _PCReportContext = cpContext;
            _db = db;
            _dbd = dbd;
            _acdimpl = _acdim;
        }

        public async Task<ExamWiseTermReport_DTO> Getdetails(ExamWiseTermReport_DTO data)//int IVRMM_Id
        {
            ExamWiseTermReport_DTO getdata = new ExamWiseTermReport_DTO();
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _PCReportContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                getdata.yearlist = list.ToArray();

                List<School_M_Section> seclist = new List<School_M_Section>();
                seclist = _PCReportContext.School_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).OrderBy(t => t.ASMC_Order).ToList();
                getdata.sectionlist = seclist.ToArray();

                List<AdmissionClass> admlist = new List<AdmissionClass>();
                admlist = _PCReportContext.AdmissionClass.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).OrderBy(t => t.ASMCL_Order).ToList();
                getdata.classlist = admlist.ToArray();

                List<exammasterDMO> esmp = new List<exammasterDMO>();
                esmp = _PCReportContext.exammasterDMO.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true).OrderBy(t => t.EME_ExamOrder).ToList();
                getdata.examlist = esmp.ToArray();

                if (getdata.stringmobileorportal == "Mobile")
                {
                    List<IVRM_User_MobileApp_Login_Privileges> Staffmobileappprivileges = new List<IVRM_User_MobileApp_Login_Privileges>();
                    Staffmobileappprivileges = _dbd.IVRM_User_MobileApp_Login_Privileges.Where(t => t.IVRMUL_Id == getdata.Userid && t.MI_Id == getdata.MI_Id).ToList();

                    if (Staffmobileappprivileges.Count() > 0)
                    {
                        getdata.Staffmobileappprivileges = (from Mobilepage in _dbd.IVRM_MobileApp_Page
                                                            from MobileRolePrivileges in _dbd.IVRM_Role_MobileApp_Privileges
                                                            from UserRolePrivileges in _dbd.IVRM_User_MobileApp_Login_Privileges
                                                            where (MobileRolePrivileges.MI_ID == UserRolePrivileges.MI_Id
                                                            && Mobilepage.IVRMMAP_Id == MobileRolePrivileges.IVRMMAP_Id
                                                            && Mobilepage.IVRMMAP_Id == UserRolePrivileges.IVRMMAP_Id
                                                            && MobileRolePrivileges.IVRMRT_Id == getdata.Roleid
                                                            && MobileRolePrivileges.MI_ID == getdata.MI_Id && UserRolePrivileges.IVRMUL_Id == getdata.Userid)
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

                        getdata.mobileprivileges = "true";
                    }
                    else
                    {
                        getdata.mobileprivileges = "false";
                    }

                }
            }

            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return getdata;

        }

        public ExamWiseTermReport_DTO onchangeyear(ExamWiseTermReport_DTO data)
        {
            try
            {
                data.classlist = (from a in _PCReportContext.AcademicYear
                                  from b in _PCReportContext.Exm_Category_ClassDMO
                                  from c in _PCReportContext.AdmissionClass
                                  where (a.ASMAY_Id == b.ASMAY_Id && b.ASMCL_Id == c.ASMCL_Id && a.MI_Id == data.MI_Id && a.Is_Active == true && b.ASMAY_Id == data.ASMAY_Id && b.ECAC_ActiveFlag == true && b.MI_Id == data.MI_Id)
                                  select c).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public ExamWiseTermReport_DTO onchangeclass(ExamWiseTermReport_DTO data)
        {
            try
            {
                data.sectionlist = (from a in _PCReportContext.AcademicYear
                                    from b in _PCReportContext.Exm_Category_ClassDMO
                                    from c in _PCReportContext.AdmissionClass
                                    from d in _PCReportContext.School_M_Section
                                    where (a.ASMAY_Id == b.ASMAY_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id
                                    && a.MI_Id == data.MI_Id && a.Is_Active == true && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && b.ECAC_ActiveFlag == true && b.MI_Id == data.MI_Id)
                                    select d).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public ExamWiseTermReport_DTO onchangesection(ExamWiseTermReport_DTO data)
        {
            try
            {
                data.exmstdlist = (from a in _PCReportContext.Exm_Master_CategoryDMO
                                   from b in _PCReportContext.Exm_Category_ClassDMO
                                   from c in _PCReportContext.Exm_Yearly_CategoryDMO
                                   from d in _PCReportContext.Exm_Yearly_Category_ExamsDMO
                                   from e in _PCReportContext.AcademicYear
                                   from f in _PCReportContext.AdmissionClass
                                   from g in _PCReportContext.School_M_Section
                                   from h in _PCReportContext.masterexam
                                   where (a.EMCA_Id == b.EMCA_Id && c.EMCA_Id == a.EMCA_Id && c.EYC_Id == d.EYC_Id && e.ASMAY_Id == c.ASMAY_Id
                                   && b.ASMAY_Id == e.ASMAY_Id && b.ASMCL_Id == f.ASMCL_Id && b.ASMS_Id == g.ASMS_Id && d.EME_Id == h.EME_Id
                                   && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && a.MI_Id == data.MI_Id
                                   && c.ASMAY_Id == data.ASMAY_Id && b.ECAC_ActiveFlag == true && c.EYC_ActiveFlg == true && d.EYCE_ActiveFlg == true
                                   && h.EME_ActiveFlag == true)
                                   select h).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }

    }
}
