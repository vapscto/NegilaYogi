using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.College.Exam;
using DomainModel.Model.com.vapstech.College.Exam;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Exam;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.Services
{
    public class ClgcoursebranchmappingIMPL : Interfaces.ClgcoursebranchmappingInterface
    {
        private static ConcurrentDictionary<string, Exm_Col_CourseBranchDTO> _login =
                new ConcurrentDictionary<string, Exm_Col_CourseBranchDTO>();


        public ClgExamContext _examcontext;
        readonly ILogger<ClgcoursebranchmappingIMPL> _logger;
        public ClgcoursebranchmappingIMPL(ClgExamContext ttcategory, ILogger<ClgcoursebranchmappingIMPL> log)
        {
            _examcontext = ttcategory;
            _logger = log;
        }
        public Exm_Col_CourseBranchDTO getdetails(int id)
        {
            Exm_Col_CourseBranchDTO TTMC = new Exm_Col_CourseBranchDTO();
            try
            {
                TTMC.courseslist = _examcontext.MasterCourseDMO.Where(c => c.MI_Id == id && c.AMCO_ActiveFlag == true).ToList().Distinct().ToArray();
                TTMC.subjectshemalist = _examcontext.AdmCollegeSubjectSchemeDMO.Where(c => c.MI_Id == id && c.ACST_ActiveFlg == true).ToList().Distinct().ToArray();
                TTMC.subjectgrplist = _examcontext.col_Exm_Master_GroupDMO.Where(c => c.MI_Id == id && c.EMG_ActiveFlag == true).ToList().Distinct().ToArray();
                TTMC.branchlist = _examcontext.ClgMasterBranchDMO.Where(c => c.MI_Id == id && c.AMB_ActiveFlag == true).ToList().Distinct().ToArray();
                TTMC.schmetypelist = _examcontext.AdmCollegeSchemeTypeDMO.Where(c => c.MI_Id == id && c.ACST_ActiveFlg == true).ToList().Distinct().ToArray();
                TTMC.semisters = _examcontext.CLG_Adm_Master_SemesterDMO.Where(c => c.MI_Id == id && c.AMSE_ActiveFlg == true).ToList().Distinct().ToArray();

                TTMC.alllist = (from a in _examcontext.Exm_Col_Yearly_SchemeDMO
                                from b in _examcontext.AdmCollegeSubjectSchemeDMO
                                from c in _examcontext.AdmCollegeSchemeTypeDMO
                                from d in _examcontext.MasterCourseDMO
                                from f in _examcontext.ClgMasterBranchDMO
                                from e in _examcontext.CLG_Adm_Master_SemesterDMO
                                from g in _examcontext.Exm_Col_Yearly_Scheme_GroupDMO
                                from h in _examcontext.col_Exm_Master_GroupDMO
                                where (a.ACSS_Id == b.ACSS_Id && a.ACST_Id == c.ACST_Id && a.AMCO_Id == d.AMCO_Id
                                && a.AMB_Id == f.AMB_Id && a.AMSE_Id == e.AMSE_Id && a.ECYS_Id == g.ECYS_Id && g.EMG_Id == h.EMG_Id && a.MI_Id == id)
                                select new Exm_Col_CourseBranchDTO
                                {
                                    ECYS_Id = a.ECYS_Id,
                                    AMCO_Id = d.AMCO_Id,
                                    ACSS_Id = b.ACSS_Id,
                                    AMB_Id = f.AMB_Id,
                                    AMSE_Id = e.AMSE_Id,
                                    ACST_Id = c.ACST_Id,
                                    ACSS_SchmeName = b.ACSS_SchmeName,
                                    AMCO_CourseName = d.AMCO_CourseName,
                                    AMB_BranchName = f.AMB_BranchName,
                                    AMSE_SEMName = e.AMSE_SEMName,
                                    subgroupname = h.EMG_GroupName,
                                    ACST_SchmeType = c.ACST_SchmeType,
                                    ECYS_ActiveFlag = a.ECYS_ActiveFlag,
                                    EMG_Id = g.EMG_Id
                                }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return TTMC;

        }
        public Exm_Col_CourseBranchDTO getbranch(Exm_Col_CourseBranchDTO id)
        {
            try
            {
                id.branchlist = (from a in _examcontext.MasterCourseDMO
                                 from b in _examcontext.ClgMasterCourseBranchMap
                                 from d in _examcontext.ClgMasterBranchDMO
                                 where (a.AMCO_Id == b.AMCO_Id && b.AMB_Id == d.AMB_Id && a.AMCO_ActiveFlag == true && b.AMCOBM_ActiveFlg == true && d.AMB_ActiveFlag == true && a.MI_Id == id.MI_Id && b.AMCO_Id == id.AMCO_Id)
                                 select d).Distinct().OrderBy(a => a.AMB_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return id;
        }

        public Exm_Col_CourseBranchDTO savedetail2(Exm_Col_CourseBranchDTO id)
        {
            try
            {
                if (id.ECYS_Id > 0)
                {
                    for (int i = 0; i < id.selected_semis.Length; i++)
                    {
                        var result = (from t in _examcontext.Exm_Col_Yearly_SchemeDMO
                                      from g in _examcontext.Exm_Col_Yearly_Scheme_GroupDMO
                                      where (t.ECYS_Id == g.ECYS_Id && t.MI_Id == id.MI_Id && t.AMCO_Id == id.AMCO_Id && t.AMB_Id == id.AMB_Id
                                      && t.AMSE_Id == id.selected_semis[i].AMSE_Id && t.ACSS_Id == id.ACSS_Id && t.ACST_Id == id.ACST_Id
                                      && t.ECYS_Id != id.ECYS_Id && g.EMG_Id == id.EMG_Id)
                                      select new { }).Count();
                        if (result > 0)
                        {
                            id.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {
                            var result1 = _examcontext.Exm_Col_Yearly_SchemeDMO.Single(t => t.MI_Id == id.MI_Id && t.ECYS_Id == id.ECYS_Id);
                            result1.AMCO_Id = id.AMCO_Id;
                            result1.AMB_Id = id.AMB_Id;
                            result1.ACSS_Id = id.ACSS_Id;
                            result1.ACST_Id = id.ACST_Id;
                            result1.AMSE_Id = id.selected_semis[i].AMSE_Id;
                            result1.ECYS_ActiveFlag = true;
                            result1.UpdatedDate = DateTime.Now;
                            _examcontext.Update(result1);
                            var contactExists = _examcontext.SaveChanges();
                            if (contactExists >= 1)
                            {
                                id.returnval = true;
                                var result2 = _examcontext.Exm_Col_Yearly_Scheme_GroupDMO.Single(t => t.ECYS_Id == id.ECYS_Id);
                                //if (result2.EMG_Id != id.EMG_Id)
                                //{
                                result2.EMG_Id = id.EMG_Id;
                                result2.ECYSG_ActiveFlag = true;
                                result2.UpdatedDate = DateTime.Now;
                                _examcontext.Update(result2);
                                _examcontext.SaveChanges();
                                var result1234 = _examcontext.Exm_Col_Yearly_Scheme_GroupDMO.Where(t => t.ECYS_Id == id.ECYS_Id).ToList();
                                for (int ih = 0; ih < result1234.Count; ih++)
                                {
                                    List<Exm_Col_Yearly_Scheme_Group_SubjectsDMO> lorg2 = new List<Exm_Col_Yearly_Scheme_Group_SubjectsDMO>();
                                    lorg2 = _examcontext.Exm_Col_Yearly_Scheme_Group_SubjectsDMO.Where(t => t.ECYSG_Id == result1234[i].ECYSG_Id).ToList();
                                    if (lorg2.Any())
                                    {
                                        for (int ih1 = 0; ih1 < lorg2.Count; ih1++)
                                        {
                                            _examcontext.Remove(lorg2.ElementAt(ih1));
                                            var flag1010 = _examcontext.SaveChanges();
                                        }
                                    }
                                }

                                for (int kk = 0; kk < id.selected_subgrps.Length; kk++)
                                {
                                    Exm_Col_Yearly_Scheme_Group_SubjectsDMO obj_ccs12 = new Exm_Col_Yearly_Scheme_Group_SubjectsDMO();
                                    obj_ccs12.ECYSG_Id = result1234.Select(h => h.ECYSG_Id).FirstOrDefault();
                                    obj_ccs12.ISMS_Id = id.selected_subgrps[kk].ISMS_Id; ;
                                    obj_ccs12.ECYSGS_ActiveFlag = true;
                                    obj_ccs12.CreatedDate = DateTime.Now;
                                    obj_ccs12.UpdatedDate = DateTime.Now;
                                    _examcontext.Add(obj_ccs12);
                                    _examcontext.SaveChanges();
                                    id.returnval = true;
                                }
                                //}
                            }
                            else
                            {
                                id.returnval = false;
                            }
                        }
                    }
                    id.ECYS_Id = id.ECYS_Id;
                }
                else
                {
                    for (int i = 0; i < id.selected_semis.Length; i++)
                    {
                        var result = (from t in _examcontext.Exm_Col_Yearly_SchemeDMO
                                      from g in _examcontext.Exm_Col_Yearly_Scheme_GroupDMO
                                      where (t.ECYS_Id == g.ECYS_Id && t.MI_Id == id.MI_Id && t.AMCO_Id == id.AMCO_Id && t.AMB_Id == id.AMB_Id
                                      && t.AMSE_Id == id.selected_semis[i].AMSE_Id && t.ACSS_Id == id.ACSS_Id && t.ACST_Id == id.ACST_Id
                                      && g.EMG_Id == id.EMG_Id)
                                      select new { }).Count();
                        if (result > 0)
                        {
                            id.returnduplicatestatus = "Duplicate";
                        }
                        else
                        {
                            Exm_Col_Yearly_SchemeDMO obj_ccs = new Exm_Col_Yearly_SchemeDMO();
                            obj_ccs.MI_Id = Convert.ToInt64(id.MI_Id);
                            obj_ccs.AMCO_Id = id.AMCO_Id;
                            obj_ccs.AMB_Id = id.AMB_Id;
                            obj_ccs.ACSS_Id = id.ACSS_Id;
                            obj_ccs.ACST_Id = id.ACST_Id;
                            obj_ccs.AMSE_Id = id.selected_semis[i].AMSE_Id;
                            obj_ccs.ECYS_ActiveFlag = true;
                            obj_ccs.CreatedDate = DateTime.Now;
                            obj_ccs.UpdatedDate = DateTime.Now;
                            _examcontext.Add(obj_ccs);
                            var contactExists = _examcontext.SaveChanges();
                            var result123 = _examcontext.Exm_Col_Yearly_SchemeDMO.Max(t => t.ECYS_Id);
                            if (contactExists >= 1)
                            {
                                Exm_Col_Yearly_Scheme_GroupDMO obj_ccs1 = new Exm_Col_Yearly_Scheme_GroupDMO();
                                obj_ccs1.ECYS_Id = result123;
                                obj_ccs1.EMG_Id = id.EMG_Id;
                                obj_ccs1.ECYSG_ActiveFlag = true;
                                obj_ccs1.CreatedDate = DateTime.Now;
                                obj_ccs1.UpdatedDate = DateTime.Now;
                                _examcontext.Add(obj_ccs1);
                                var exw = _examcontext.SaveChanges();
                                if (exw >= 1)
                                {
                                    var result1234 = _examcontext.Exm_Col_Yearly_Scheme_GroupDMO.Max(t => t.ECYSG_Id);
                                    for (int kk = 0; kk < id.selected_subgrps.Length; kk++)
                                    {
                                        Exm_Col_Yearly_Scheme_Group_SubjectsDMO obj_ccs12 = new Exm_Col_Yearly_Scheme_Group_SubjectsDMO();
                                        obj_ccs12.ECYSG_Id = result1234;
                                        obj_ccs12.ISMS_Id = id.selected_subgrps[kk].ISMS_Id; ;
                                        obj_ccs12.ECYSGS_ActiveFlag = true;
                                        obj_ccs12.CreatedDate = DateTime.Now;
                                        obj_ccs12.UpdatedDate = DateTime.Now;
                                        _examcontext.Add(obj_ccs12);
                                        var exw1 = _examcontext.SaveChanges();
                                        if (exw1 >= 1)
                                        {
                                            id.returnval = true;
                                        }
                                        else
                                        {
                                            var result11 = _examcontext.Exm_Col_Yearly_Scheme_GroupDMO.Where(t => t.ECYSG_Id.Equals(result1234)).ToList();
                                            for (int ih = 0; ih < result11.Count; ih++)
                                            {
                                                List<Exm_Col_Yearly_Scheme_GroupDMO> lorg2 = new List<Exm_Col_Yearly_Scheme_GroupDMO>();
                                                lorg2 = _examcontext.Exm_Col_Yearly_Scheme_GroupDMO.Where(t => t.ECYSG_Id == result11[i].ECYSG_Id).ToList();
                                                if (lorg2.Any())
                                                {
                                                    for (int ih1 = 0; ih1 < lorg2.Count; ih1++)
                                                    {
                                                        _examcontext.Remove(lorg2.ElementAt(ih1));
                                                        var flag1010 = _examcontext.SaveChanges();
                                                    }
                                                }
                                            }
                                            var result1 = _examcontext.Exm_Col_Yearly_SchemeDMO.Where(t => t.ECYS_Id.Equals(result123)).ToList();
                                            for (int ih = 0; ih < result1.Count; ih++)
                                            {
                                                List<Exm_Col_Yearly_SchemeDMO> lorg = new List<Exm_Col_Yearly_SchemeDMO>();
                                                lorg = _examcontext.Exm_Col_Yearly_SchemeDMO.Where(t => t.ECYS_Id == result1[i].ECYS_Id).ToList();
                                                if (lorg.Any())
                                                {
                                                    for (int ih1 = 0; ih1 < lorg.Count; ih1++)
                                                    {
                                                        _examcontext.Remove(lorg.ElementAt(ih1));
                                                        var flag1010 = _examcontext.SaveChanges();
                                                    }
                                                }
                                            }
                                            id.returnval = false;
                                        }
                                    }
                                }
                                else
                                {
                                    id.returnval = false;
                                    var result1 = _examcontext.Exm_Col_Yearly_SchemeDMO.Where(t => t.ECYS_Id.Equals(result123)).ToList();
                                    for (int ih = 0; ih < result1.Count; ih++)
                                    {
                                        List<Exm_Col_Yearly_SchemeDMO> lorg = new List<Exm_Col_Yearly_SchemeDMO>();
                                        lorg = _examcontext.Exm_Col_Yearly_SchemeDMO.Where(t => t.ECYS_Id == result1[i].ECYS_Id).ToList();
                                        if (lorg.Any())
                                        {
                                            for (int ih1 = 0; ih1 < result1.Count; ih1++)
                                            {
                                                _examcontext.Remove(lorg.ElementAt(ih1));
                                                var flag1010 = _examcontext.SaveChanges();
                                            }
                                        }
                                    }
                                }

                            }
                            else
                            {
                                id.returnval = false;
                            }
                        }

                    }
                }
                id.alllist = (from a in _examcontext.Exm_Col_Yearly_SchemeDMO
                              from b in _examcontext.AdmCollegeSubjectSchemeDMO
                              from c in _examcontext.AdmCollegeSchemeTypeDMO
                              from d in _examcontext.MasterCourseDMO
                              from f in _examcontext.ClgMasterBranchDMO
                              from e in _examcontext.CLG_Adm_Master_SemesterDMO
                              from g in _examcontext.Exm_Col_Yearly_Scheme_GroupDMO
                              from h in _examcontext.col_Exm_Master_GroupDMO
                              where (a.MI_Id == id.MI_Id && a.ACSS_Id == b.ACSS_Id && a.ACST_Id == c.ACST_Id && a.AMCO_Id == d.AMCO_Id && a.AMB_Id == f.AMB_Id && a.AMSE_Id == e.AMSE_Id && a.ECYS_Id == g.ECYS_Id && g.EMG_Id == h.EMG_Id)
                              select new Exm_Col_CourseBranchDTO
                              {
                                  ACSS_SchmeName = b.ACSS_SchmeName,
                                  AMCO_CourseName = d.AMCO_CourseName,
                                  AMB_BranchName = f.AMB_BranchName,
                                  AMSE_SEMName = e.AMSE_SEMName,
                                  subgroupname = h.EMG_GroupName,
                                  ACST_SchmeType = c.ACST_SchmeType,
                                  ECYS_ActiveFlag = a.ECYS_ActiveFlag
                              }).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return id;
        }
        public Exm_Col_CourseBranchDTO get_subjects(Exm_Col_CourseBranchDTO data)
        {
            Exm_Col_CourseBranchDTO TTMC = new Exm_Col_CourseBranchDTO();
            try
            {
                TTMC.subjectgroups = (from s in _examcontext.Exm_Master_GroupDMO
                                      from k in _examcontext.Exm_Col_Master_Group_SubjectsDMO
                                      from j in _examcontext.IVRM_Master_SubjectsDMO
                                      where (s.EMG_Id == k.EMG_Id && k.ISMS_Id == j.ISMS_Id && k.EMG_Id == data.EMG_Id && s.MI_Id == data.MI_Id && s.EMG_ActiveFlag == true && k.ECMGS_ActiveFlag == true)
                                      select new Exm_Col_CourseBranchDTO
                                      {
                                          ISMS_Id = k.ISMS_Id,
                                          ISMS_SubjectName = j.ISMS_SubjectName,
                                          ISMS_SubjectCode = j.ISMS_SubjectCode,
                                          ISMS_Max_Marks = j.ISMS_Max_Marks,
                                          ISMS_Min_Marks = j.ISMS_Min_Marks,
                                          ISMS_OrderFlag = j.ISMS_OrderFlag,
                                      }).Distinct().OrderBy(t => t.ISMS_OrderFlag).ToArray();

                TTMC.examlist = _examcontext.col_exammasterDMO.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true).OrderBy(t => t.EME_ExamOrder).ToList().ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return TTMC;

        }
        public Exm_Col_CourseBranchDTO getalldetailsviewrecords(Exm_Col_CourseBranchDTO data)
        {
            try
            {
                data.viewrecords = (from a in _examcontext.Exm_Col_Yearly_SchemeDMO
                                    from re in _examcontext.Exm_Col_Yearly_Scheme_GroupDMO
                                    from rr in _examcontext.Exm_Col_Yearly_Scheme_Group_SubjectsDMO
                                    from b in _examcontext.AdmCollegeSubjectSchemeDMO
                                    from c in _examcontext.AdmCollegeSchemeTypeDMO
                                    from d in _examcontext.MasterCourseDMO
                                    from f in _examcontext.ClgMasterBranchDMO
                                    from e in _examcontext.CLG_Adm_Master_SemesterDMO
                                    from r in _examcontext.IVRM_Master_SubjectsDMO
                                    where (a.ACSS_Id == b.ACSS_Id && a.ACST_Id == c.ACST_Id && a.AMCO_Id == d.AMCO_Id
                                    && a.AMB_Id == f.AMB_Id && a.AMSE_Id == e.AMSE_Id && a.ECYS_Id == re.ECYS_Id && re.ECYSG_Id == rr.ECYSG_Id
                                    && rr.ISMS_Id == r.ISMS_Id && a.ACSS_Id == b.ACSS_Id && a.AMB_Id == f.AMB_Id && a.AMSE_Id == e.AMSE_Id
                                    && a.ACST_Id == c.ACST_Id && a.MI_Id == data.MI_Id && a.AMSE_Id == data.AMSE_Id && a.AMCO_Id == data.AMCO_Id
                                    && a.ACST_Id == data.ACST_Id && a.AMB_Id == data.AMB_Id && a.ACSS_Id == data.ACSS_Id && a.ECYS_Id == data.ECYS_Id
                                    && re.ECYS_Id == data.ECYS_Id && re.EMG_Id == data.EMG_Id)
                                    select new Exm_Col_CourseBranchDTO
                                    {
                                        ECYS_Id = a.ECYS_Id,
                                        ACSS_SchmeName = b.ACSS_SchmeName,
                                        AMCO_CourseName = d.AMCO_CourseName,
                                        AMB_BranchName = f.AMB_BranchName,
                                        AMSE_SEMName = e.AMSE_SEMName,
                                        ACST_SchmeType = c.ACST_SchmeType,
                                        ISMS_SubjectName = r.ISMS_SubjectName

                                    }).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public Exm_Col_CourseBranchDTO deactivate(Exm_Col_CourseBranchDTO page)
        {
            List<Exm_Col_Yearly_SchemeDMO> lorg = new List<Exm_Col_Yearly_SchemeDMO>();
            List<Exm_Col_Yearly_Scheme_GroupDMO> lorg1 = new List<Exm_Col_Yearly_Scheme_GroupDMO>();
            List<Exm_Col_Yearly_Scheme_Group_SubjectsDMO> lorg2 = new List<Exm_Col_Yearly_Scheme_Group_SubjectsDMO>();
            try
            {
                lorg = _examcontext.Exm_Col_Yearly_SchemeDMO.Where(t => t.AMCO_Id == page.AMCO_Id && t.AMB_Id == page.AMB_Id && t.ACSS_Id == page.ACSS_Id && t.ACST_Id == page.ACST_Id && t.AMSE_Id == page.AMSE_Id).ToList();
                if (lorg.Count() > 0)
                {

                    for (int i = 0; i < lorg.Count(); i++)
                    {
                        var result = _examcontext.Exm_Col_Yearly_SchemeDMO.Single(t => t.MI_Id == page.MI_Id && t.ECYS_Id == lorg[i].ECYS_Id);
                        if (result.ECYS_ActiveFlag == true)
                        {
                            result.ECYS_ActiveFlag = false;
                        }
                        else
                        {
                            result.ECYS_ActiveFlag = true;
                        }
                        result.UpdatedDate = DateTime.Now;
                        _examcontext.Update(result);
                        var flag = _examcontext.SaveChanges();
                        if (flag == 1)
                        {
                            lorg1 = _examcontext.Exm_Col_Yearly_Scheme_GroupDMO.Where(t => t.ECYS_Id == lorg[i].ECYS_Id).ToList();
                            if (lorg1.Count() > 0)
                            {

                                for (int ii = 0; ii < lorg1.Count(); ii++)
                                {
                                    var result1 = _examcontext.Exm_Col_Yearly_Scheme_GroupDMO.Single(t => t.ECYSG_Id == lorg1[ii].ECYSG_Id);
                                    if (result1.ECYSG_ActiveFlag == true)
                                    {
                                        result1.ECYSG_ActiveFlag = false;
                                    }
                                    else
                                    {
                                        result1.ECYSG_ActiveFlag = true;
                                    }
                                    result1.UpdatedDate = DateTime.Now;
                                    _examcontext.Update(result1);
                                    _examcontext.SaveChanges();
                                    lorg2 = _examcontext.Exm_Col_Yearly_Scheme_Group_SubjectsDMO.Where(t => t.ECYSG_Id == lorg1[ii].ECYSG_Id).ToList();
                                    if (lorg2.Count() > 0)
                                    {
                                        for (int iii = 0; iii < lorg2.Count(); iii++)
                                        {
                                            var result2 = _examcontext.Exm_Col_Yearly_Scheme_Group_SubjectsDMO.Single(t => t.ECYSG_Id == lorg1[iii].ECYSG_Id);
                                            if (result2.ECYSGS_ActiveFlag == true)
                                            {
                                                result2.ECYSGS_ActiveFlag = false;
                                            }
                                            else
                                            {
                                                result2.ECYSGS_ActiveFlag = true;
                                            }
                                            result2.UpdatedDate = DateTime.Now;
                                            _examcontext.Update(result2);
                                            _examcontext.SaveChanges();
                                            page.returnval = true;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            page.returnval = false;
                        }
                    }
                }
                else
                {
                    page.returnval = false;
                }
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
            }
            return page;
        }

        public Exm_Col_CourseBranchDTO editdeatils(int ID)
        {
            Exm_Col_CourseBranchDTO edit = new Exm_Col_CourseBranchDTO();
            try
            {
                edit.editlist = _examcontext.Exm_Col_Yearly_SchemeDMO.Where(e => e.ECYS_Id == ID).ToArray();
                var emg = _examcontext.Exm_Col_Yearly_Scheme_GroupDMO.Where(e => e.ECYS_Id == ID).Select(g => g.EMG_Id).Distinct().FirstOrDefault();
                edit.EMG_Id = emg;
                var miid = _examcontext.Exm_Col_Yearly_SchemeDMO.Where(e => e.ECYS_Id == ID).Select(g => g.MI_Id).Distinct().FirstOrDefault();
                edit.editlist2 = (from s in _examcontext.Exm_Master_GroupDMO
                                  from k in _examcontext.Exm_Col_Master_Group_SubjectsDMO
                                  from j in _examcontext.IVRM_Master_SubjectsDMO
                                  where (s.EMG_Id == k.EMG_Id && k.ISMS_Id == j.ISMS_Id && k.EMG_Id == emg && s.MI_Id == miid && s.EMG_ActiveFlag == true && k.ECMGS_ActiveFlag == true)
                                  select new Exm_Col_CourseBranchDTO
                                  {
                                      ISMS_Id = k.ISMS_Id,
                                      ISMS_SubjectName = j.ISMS_SubjectName,
                                      ISMS_SubjectCode = j.ISMS_SubjectCode,
                                      ISMS_Max_Marks = j.ISMS_Max_Marks,
                                      ISMS_Min_Marks = j.ISMS_Min_Marks,
                                      ISMS_OrderFlag = j.ISMS_OrderFlag,
                                  }).Distinct().OrderBy(t => t.ISMS_OrderFlag).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return edit;
        }


    }

}
