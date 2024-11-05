using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TimeTableServiceHub.Services
{
    public class BifurcationImpl : Interfaces.BifurcationInterface
    {
        private static ConcurrentDictionary<string, TT_Bifurcation_DTO> _login =
             new ConcurrentDictionary<string, TT_Bifurcation_DTO>();

        public TTContext _AcademicContext;
        ILogger<CategoryClassMappImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public BifurcationImpl(TTContext academiccontext, ILogger<CategoryClassMappImpl> acdimpl, DomainModelMsSqlServerContext db)
        {
            _AcademicContext = academiccontext;
            _acdimpl = acdimpl;
            _db = db;
        }
        public TT_Bifurcation_DTO getallDetails(TT_Bifurcation_DTO acdmc)
        {
            try
            {
 acdmc.categorylist = _AcademicContext.TTMasterCategoryDMO.AsNoTracking().Where(t => t.MI_Id.Equals(acdmc.MI_Id) && t.TTMC_ActiveFlag == true).ToList().ToArray();
 acdmc.acdlist = _AcademicContext.AcademicYear.AsNoTracking().Where(t => t.MI_Id.Equals(acdmc.MI_Id) && t.Is_Active == true).OrderByDescending(o=>o.ASMAY_Order).ToList().ToArray();

 acdmc.classlist = _AcademicContext.School_M_Class.AsNoTracking().Where(t => t.MI_Id.Equals(acdmc.MI_Id) && t.ASMCL_ActiveFlag == true).ToList().ToArray();

 acdmc.sectionlist = _AcademicContext.School_M_Section.AsNoTracking().Where(t => t.MI_Id.Equals(acdmc.MI_Id)).ToList().ToArray();

 acdmc.subjectlist = _AcademicContext.IVRM_School_Master_SubjectsDMO.AsNoTracking().Where(t => t.MI_Id.Equals(acdmc.MI_Id)).ToList().ToArray();

acdmc.periodlist = _AcademicContext.TT_Master_PeriodDMO.AsNoTracking().Where(t => t.MI_Id.Equals(acdmc.MI_Id) && t.TTMP_ActiveFlag == true).ToList().ToArray();

                acdmc.stafflist = (from e in _AcademicContext.HR_Master_Employee_DMO
                                   from g in _AcademicContext.TT_Final_Period_DistributionDMO
                                   where (e.MI_Id == g.MI_Id && e.HRME_Id == g.HRME_Id && g.MI_Id == acdmc.MI_Id)
                                   select new TTClassMasterDTO
                                   {
                                       HRME_Id = e.HRME_Id,
                                       staffName = (e.HRME_EmployeeFirstName + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName)).Trim(),

                                   }
                    ).Distinct().OrderBy(x => x.staffName).ToArray();

                acdmc.detailslist = (from m in _AcademicContext.TT_Bifurcation_DMO
                                     from q in _AcademicContext.AcademicYear
                                     from s in _AcademicContext.TTMasterCategoryDMO
                                     where (m.MI_Id == acdmc.MI_Id && m.ASMAY_Id == q.ASMAY_Id && m.TTMC_Id == s.TTMC_Id)
                                     select new TT_Bifurcation_DTO
                                     {
                                         AcdYear = q.ASMAY_Year,
                                         categoryName = s.TTMC_CategoryName,
                                         bifricationName = m.TTB_BifurcationName,
                                         periodname = m.TTB_NoOfPeriods.ToString(),
                                         TTB_Id = m.TTB_Id,
                                         TTB_ActiveFlag = m.TTB_ActiveFlag

                                     }).OrderByDescending(d => d.UpdatedDate).ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return acdmc;
        }

        public TT_Bifurcation_DTO getdetails(TT_Bifurcation_DTO acdmc)
        {

            try
            {


                acdmc.stafflist = (from e in _AcademicContext.HR_Master_Employee_DMO
                                   from g in _AcademicContext.TT_Final_Period_DistributionDMO
                                   where (e.MI_Id == g.MI_Id && e.HRME_Id == g.HRME_Id && g.MI_Id == acdmc.MI_Id)
                                   select new TTClassMasterDTO
                                   {
                                       HRME_Id = e.HRME_Id,
                                       staffName = (e.HRME_EmployeeFirstName + (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == " " || e.HRME_EmployeeMiddleName == "0" ? " " : e.HRME_EmployeeMiddleName) + (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == " " || e.HRME_EmployeeLastName == "0" ? " " : e.HRME_EmployeeLastName)).Trim(),

                                   }
                    ).Distinct().OrderBy(x => x.staffName).ToArray();

                acdmc.editdetailslist = (from m in _AcademicContext.TT_Bifurcation_DMO
                                         from n in _AcademicContext.TT_Bifurcation_Details_DMO
                                         from p in _AcademicContext.School_M_Class
                                         from t in _AcademicContext.HR_Master_Employee_DMO
                                         from u in _AcademicContext.School_M_Section
                                         from v in _AcademicContext.IVRM_School_Master_SubjectsDMO
                                         where (m.MI_Id == acdmc.MI_Id && m.TTB_Id == n.TTB_Id && n.ASMCL_Id == p.ASMCL_Id && n.ASMS_Id == u.ASMS_Id && n.HRME_Id == t.HRME_Id && n.ISMS_Id == v.ISMS_Id && m.TTB_Id == acdmc.TTB_Id)
                                         select new TT_Bifurcation_DTO
                                         {
                                             className = p.ASMCL_ClassName,
                                             sectioname = u.ASMC_SectionName,
                                             staffname = (t.HRME_EmployeeFirstName + (t.HRME_EmployeeMiddleName == null || t.HRME_EmployeeMiddleName == " " || t.HRME_EmployeeMiddleName == "0" ? " " : t.HRME_EmployeeMiddleName) + (t.HRME_EmployeeLastName == null || t.HRME_EmployeeLastName == " " || t.HRME_EmployeeLastName == "0" ? " " : t.HRME_EmployeeLastName)).Trim(),
                                             subjectname = v.ISMS_SubjectName,
                                             TTBD_Id = n.TTBD_Id

                                         }).OrderByDescending(d => d.UpdatedDate).ToArray();              

    acdmc.detailslist = _AcademicContext.TT_Bifurcation_DMO.Where(t => t.TTB_Id.Equals(acdmc.TTB_Id) && t.MI_Id == acdmc.MI_Id).ToList().ToArray();

                var ttmc_id_get = _AcademicContext.TT_Bifurcation_DMO.Where(t => t.TTB_Id.Equals(acdmc.TTB_Id) && t.MI_Id == acdmc.MI_Id).Distinct().Select(f => f.TTMC_Id).FirstOrDefault();

                var ids = _AcademicContext.TT_Category_Class_DMO.AsNoTracking().Where(t => t.MI_Id.Equals(acdmc.MI_Id) && t.TTMC_Id.Equals(ttmc_id_get)).Select(t => t.ASMCL_Id).ToList();

    acdmc.classlist = _AcademicContext.School_M_Class.AsNoTracking().Where(t => ids.Contains(t.ASMCL_Id)).ToList().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return acdmc;
        }

        public TT_Bifurcation_DTO getalldetailsviewrecords(TT_Bifurcation_DTO acdmc)
        {

            try
            {

                acdmc.viewdata = (from m in _AcademicContext.TT_Bifurcation_DMO
                                  from n in _AcademicContext.TT_Bifurcation_Details_DMO
                                  from p in _AcademicContext.School_M_Class
                                  from t in _AcademicContext.HR_Master_Employee_DMO
                                  from u in _AcademicContext.School_M_Section
                                  from v in _AcademicContext.IVRM_School_Master_SubjectsDMO
                                  where (m.MI_Id == acdmc.MI_Id && m.TTB_Id == n.TTB_Id && n.ASMCL_Id == p.ASMCL_Id && n.ASMS_Id == u.ASMS_Id && n.HRME_Id == t.HRME_Id && n.ISMS_Id == v.ISMS_Id && m.TTB_Id == acdmc.TTB_Id)
                                  select new TT_Bifurcation_DTO
                                  {
                                      TTB_BifurcationName = m.TTB_BifurcationName,
                                      className = p.ASMCL_ClassName,
                                      sectioname = u.ASMC_SectionName,
                                      staffname = (t.HRME_EmployeeFirstName + (t.HRME_EmployeeMiddleName == null || t.HRME_EmployeeMiddleName == " " || t.HRME_EmployeeMiddleName == "0" ? " " : t.HRME_EmployeeMiddleName) + (t.HRME_EmployeeLastName == null || t.HRME_EmployeeLastName == " " || t.HRME_EmployeeLastName == "0" ? " " : t.HRME_EmployeeLastName)).Trim(),
                                      subjectname = v.ISMS_SubjectName,
                                      TTBD_Id = n.TTBD_Id

                                  }).OrderByDescending(d => d.UpdatedDate).ToArray();

   acdmc.detailslist = _AcademicContext.TT_Bifurcation_DMO.Where(t => t.TTB_Id.Equals(acdmc.TTB_Id) && t.MI_Id == acdmc.MI_Id).ToList().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return acdmc;
        }
        public TT_Bifurcation_DTO saveProsdet(TT_Bifurcation_DTO acd)
        {
            try
            {

                if (acd.TTB_Id > 0)
                {

                    TT_Bifurcation_DMO enq = Mapper.Map<TT_Bifurcation_DMO>(acd);
                    var res = _AcademicContext.TT_Bifurcation_DMO.Where(t => t.MI_Id == enq.MI_Id && (t.TTB_BifurcationName == enq.TTB_BifurcationName) && t.TTB_Id !=acd.TTB_Id).ToList();
                    if (res.Count() > 0)
                    {
                        acd.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _AcademicContext.TT_Bifurcation_DMO.Single(t => t.TTB_Id.Equals(acd.TTB_Id));

                        result.TTB_Id = result.TTB_Id;
                        result.MI_Id = enq.MI_Id;
                        result.ASMAY_Id = enq.ASMAY_Id;
                        result.TTMC_Id = enq.TTMC_Id;
                        result.TTB_BifurcationName = enq.TTB_BifurcationName;
                        result.TTB_NoOfPeriods = enq.TTB_NoOfPeriods;
                        result.TTB_ConsecutiveFlag = enq.TTB_ConsecutiveFlag;
                        if (enq.TTB_ConsecutiveFlag == 1)
                        {
                            result.TTB_NoOfConPeriods = enq.TTB_NoOfConPeriods;
                            result.TTB_NoOfConDays = enq.TTB_NoOfConDays;
                        }
                        else
                        {
                            result.TTB_NoOfConPeriods = 0;
                            result.TTB_NoOfConDays = 0;
                        }
                        result.TTB_BefAftApplFlag = enq.TTB_BefAftApplFlag;
                        if (enq.TTB_BefAftApplFlag == 1)
                        {
                            result.TTB_BefAftFalg = enq.TTB_BefAftFalg;
                        }
                        else
                        {
                            result.TTB_BefAftFalg = "";
                        }
                        result.TTMP_Id = enq.TTMP_Id;
                        result.TTB_ActiveFlag = true;

                        _AcademicContext.Update(result);
                        var flag = _AcademicContext.SaveChanges();
                        if (flag == 1)
                        {
                            var result1 = _AcademicContext.TT_Bifurcation_Details_DMO.Where(t => t.TTB_Id.Equals(acd.TTB_Id)).ToList().ToList();

                            for (int i = 0; i < result1.Count; i++)
                            {
                                List<TT_Bifurcation_Details_DMO> lorg1 = new List<TT_Bifurcation_Details_DMO>();

                                lorg1 = _AcademicContext.TT_Bifurcation_Details_DMO.Where(t => t.TTBD_Id == result1[i].TTBD_Id).ToList();

                                if (lorg1.Any())
                                {
                                    _AcademicContext.Remove(lorg1.ElementAt(0));
                                    var flag1010 = _AcademicContext.SaveChanges();
                                }

                            }

                            for (int j = 0; j < acd.combinationlist.Length; j++)
                            {
                                TT_Bifurcation_Details_DMO enq1 = Mapper.Map<TT_Bifurcation_Details_DMO>(acd.combinationlist[j]);

                                enq1.TTB_Id = enq.TTB_Id;
                                enq1.ASMCL_Id = acd.combinationlist[j].ASMCL_Id;
                                enq1.ASMS_Id = acd.combinationlist[j].ASMS_Id;
                                enq1.HRME_Id = acd.combinationlist[j].HRME_Id;
                                enq1.ISMS_Id = acd.combinationlist[j].ISMS_Id;
                                enq1.TTBD_ActiveFlag = true;

                                _AcademicContext.Add(enq1);
                                var flag1 = _AcademicContext.SaveChanges();
                                if (flag1 == 1)
                                {
                                    acd.returnMsg = "update";
                                }
                            }

                            acd.detailslist = (from m in _AcademicContext.TT_Bifurcation_DMO
                                               from q in _AcademicContext.AcademicYear
                                               from s in _AcademicContext.TTMasterCategoryDMO
                                               where (m.MI_Id == acd.MI_Id && m.ASMAY_Id == q.ASMAY_Id && m.TTMC_Id == s.TTMC_Id)
                                               select new TT_Bifurcation_DTO
                                               {
                                                   AcdYear = q.ASMAY_Year,
                                                   categoryName = s.TTMC_CategoryName,
                                                   bifricationName = m.TTB_BifurcationName,
                                                   periodname = m.TTB_NoOfPeriods.ToString(),
                                                   TTB_Id = m.TTB_Id,
                                                   TTB_ActiveFlag = m.TTB_ActiveFlag

                                               }).OrderByDescending(d => d.UpdatedDate).ToArray();

                        }
                        else
                        {
                            acd.returnMsg = "";
                        }
                    }
                }
                else
                {
                    TT_Bifurcation_DMO enq = Mapper.Map<TT_Bifurcation_DMO>(acd);
                    var res = _AcademicContext.TT_Bifurcation_DMO.Where(t => t.MI_Id == enq.MI_Id && (t.TTB_BifurcationName == enq.TTB_BifurcationName)).ToList();
                    if (res.Count() > 0)
                    {
                        acd.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {

                        enq.TTB_Id = 0;
                        enq.MI_Id = enq.MI_Id;
                        enq.ASMAY_Id = enq.ASMAY_Id;
                        enq.TTMC_Id = enq.TTMC_Id;
                        enq.TTB_BifurcationName = enq.TTB_BifurcationName;
                        enq.TTB_NoOfPeriods = enq.TTB_NoOfPeriods;
                        enq.TTB_RemPeriods = enq.TTB_NoOfPeriods;
                        enq.TTB_ConsecutiveFlag = enq.TTB_ConsecutiveFlag;
                        if (enq.TTB_ConsecutiveFlag == 1)
                        {
                            enq.TTB_NoOfConPeriods = enq.TTB_NoOfConPeriods;
                            enq.TTB_NoOfConDays = enq.TTB_NoOfConDays;
                        }
                        else
                        {
                            enq.TTB_NoOfConPeriods = 0;
                            enq.TTB_NoOfConDays = 0;
                        }
                        enq.TTB_BefAftApplFlag = enq.TTB_BefAftApplFlag;
                        if (enq.TTB_BefAftApplFlag == 1)
                        {
                            enq.TTB_BefAftFalg = enq.TTB_BefAftFalg;
                        }
                        else
                        {
                            enq.TTB_BefAftFalg = "";
                        }
                        enq.TTMP_Id = enq.TTMP_Id;
                        enq.TTB_AllotedFlag = "No";
                        enq.TTB_ActiveFlag = true;
                        enq.CreatedDate = DateTime.Now;
                        enq.UpdatedDate = DateTime.Now;

                        _AcademicContext.Add(enq);
                        var flag = _AcademicContext.SaveChanges();
                        if (flag == 1)
                        {
                            enq.TTB_Id = enq.TTB_Id;

                            for (int j = 0; j < acd.combinationlist.Length; j++)
                            {
                                TT_Bifurcation_Details_DMO enq1 = Mapper.Map<TT_Bifurcation_Details_DMO>(acd.combinationlist[j]);

                                enq1.TTB_Id = enq.TTB_Id;
                                enq1.ASMCL_Id = acd.combinationlist[j].ASMCL_Id;
                                enq1.ASMS_Id = acd.combinationlist[j].ASMS_Id;
                                enq1.HRME_Id = acd.combinationlist[j].HRME_Id;
                                enq1.ISMS_Id = acd.combinationlist[j].ISMS_Id;
                                enq1.TTBD_ActiveFlag = true;

                                _AcademicContext.Add(enq1);
                                var flag1 = _AcademicContext.SaveChanges();
                                if (flag1 == 1)
                                {
                                    acd.returnMsg = "Add";
                                }
                            }

                            acd.detailslist = (from m in _AcademicContext.TT_Bifurcation_DMO
                                               from q in _AcademicContext.AcademicYear
                                               from s in _AcademicContext.TTMasterCategoryDMO
                                               where (m.MI_Id == acd.MI_Id && m.ASMAY_Id == q.ASMAY_Id && m.TTMC_Id == s.TTMC_Id)
                                               select new TT_Bifurcation_DTO
                                               {
                                                   AcdYear = q.ASMAY_Year,
                                                   categoryName = s.TTMC_CategoryName,
                                                   bifricationName = m.TTB_BifurcationName,
                                                   periodname = m.TTB_NoOfPeriods.ToString(),
                                                   TTB_Id = m.TTB_Id,
                                                   TTB_ActiveFlag = m.TTB_ActiveFlag

                                               }).OrderByDescending(d => d.UpdatedDate).ToArray();

                        }
                        else
                        {
                            acd.returnMsg = "";
                        }

                    }
                }

            }
            catch (Exception ee)
            {
                _acdimpl.LogError(ee.Message);
                _acdimpl.LogDebug(ee.Message);
                Console.WriteLine(ee.Message);
            }

            return acd;
        }

        public TT_Bifurcation_DTO getClassdetails(TT_Bifurcation_DTO acd)
        {
            try
            {


                var ids = _AcademicContext.TT_Category_Class_DMO.AsNoTracking().Where(t => t.MI_Id.Equals(acd.MI_Id) && t.TTMC_Id.Equals(acd.TTMC_Id)).Select(t => t.ASMCL_Id).ToList();

                acd.classlist = _AcademicContext.School_M_Class.AsNoTracking().Where(t => ids.Contains(t.ASMCL_Id)).ToList().ToArray();

            }
            catch (Exception ee)
            {
                _acdimpl.LogError(ee.Message);
                _acdimpl.LogDebug(ee.Message);
                Console.WriteLine(ee.Message);
            }

            return acd;
        }

        public TT_Bifurcation_DTO deleterec(TT_Bifurcation_DTO org)
        {

            List<TT_Bifurcation_DMO> lorg = new List<TT_Bifurcation_DMO>();


            try
            {

               

                lorg = _AcademicContext.TT_Bifurcation_DMO.Where(t => t.TTB_Id == org.TTB_Id).ToList();

                if (lorg.Any())
                {
                    var result = _AcademicContext.TT_Bifurcation_DMO.Single(t => t.TTB_Id.Equals(org.TTB_Id));

                    if (result.TTB_ActiveFlag == true)
                    {
                        result.TTB_ActiveFlag = false;

                        var result1 = _AcademicContext.TT_Bifurcation_Details_DMO.Where(t => t.TTB_Id.Equals(org.TTB_Id)).ToList().ToList();

                        for (int i = 0; i < result1.Count; i++)
                        {
                            List<TT_Bifurcation_Details_DMO> lorg1 = new List<TT_Bifurcation_Details_DMO>();

                            lorg1 = _AcademicContext.TT_Bifurcation_Details_DMO.Where(t => t.TTBD_Id == result1[i].TTBD_Id).ToList();

                            if (lorg1.Any())
                            {
                                lorg1.ElementAt(0).TTBD_ActiveFlag = false;

                                _AcademicContext.Update(lorg1.ElementAt(0));
                                
                                
                            }
                        }

                    }
                    else
                    {
                        result.TTB_ActiveFlag = true;


                        var result1 = _AcademicContext.TT_Bifurcation_Details_DMO.Where(t => t.TTB_Id.Equals(org.TTB_Id)).ToList().ToList();

                        for (int i = 0; i < result1.Count; i++)
                        {
                            List<TT_Bifurcation_Details_DMO> lorg1 = new List<TT_Bifurcation_Details_DMO>();

                            lorg1 = _AcademicContext.TT_Bifurcation_Details_DMO.Where(t => t.TTBD_Id == result1[i].TTBD_Id).ToList();

                            if (lorg1.Any())
                            {
                                lorg1.ElementAt(0).TTBD_ActiveFlag = true;

                                _AcademicContext.Update(lorg1.ElementAt(0));


                            }
                        }
                    }
                    _AcademicContext.Update(result);
                    var flag = _AcademicContext.SaveChanges();
                    if (flag >0)
                    {
                        org.returnMsg = "true";
                    }
                    else
                    {
                        org.returnMsg = "false";
                    }
                }

              



                org.detailslist = (from m in _AcademicContext.TT_Bifurcation_DMO
                                   from q in _AcademicContext.AcademicYear
                                   from s in _AcademicContext.TTMasterCategoryDMO
                                   where (m.MI_Id == org.MI_Id  && m.ASMAY_Id == q.ASMAY_Id && m.TTMC_Id == s.TTMC_Id)
                                   select new TT_Bifurcation_DTO
                                   {
                                       AcdYear = q.ASMAY_Year,
                                       categoryName = s.TTMC_CategoryName,
                                       bifricationName = m.TTB_BifurcationName,
                                       periodname = m.TTB_NoOfPeriods.ToString(),
                                       TTB_Id = m.TTB_Id,
                                       TTB_ActiveFlag = m.TTB_ActiveFlag

                                   }).OrderByDescending(d => d.UpdatedDate).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return org;
        }

    }
}
