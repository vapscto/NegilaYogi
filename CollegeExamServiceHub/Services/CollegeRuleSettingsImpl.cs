using DataAccessMsSqlServerProvider.com.vapstech.College.Exam;
using DomainModel.Model.com.vapstech.College.Exam;
using PreadmissionDTOs.com.vaps.College.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.Services
{
    public class CollegeRuleSettingsImpl : Interfaces.CollegeRuleSettingsInterface
    {
        public ClgExamContext _context;

        public CollegeRuleSettingsImpl(ClgExamContext _conte)
        {
            _context = _conte;
        }

        public CollegeRuleSettingsDTO getalldetails(CollegeRuleSettingsDTO data)
        {
            try
            {
                data.getcourse = _context.MasterCourseDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag == true).OrderBy(a => a.AMCO_Order).ToArray();


                data.getsaveddetails = (from a in _context.Exm_Col_Yearly_SchemeDMO
                                        from b in _context.Clg_Exm_M_RuleSettingDMO
                                        from c in _context.MasterCourseDMO
                                        from d in _context.ClgMasterBranchDMO
                                        from e in _context.CLG_Adm_Master_SemesterDMO
                                        from f in _context.AdmCollegeSubjectSchemeDMO
                                        from g in _context.AdmCollegeSchemeTypeDMO
                                        where (a.ECYS_Id == b.ECYS_Id && a.AMCO_Id == c.AMCO_Id && a.AMB_Id == d.AMB_Id && a.AMSE_Id == e.AMSE_Id
                                        && a.ACSS_Id == f.ACSS_Id && a.ACST_Id == g.ACST_Id && a.ECYS_ActiveFlag == true && c.AMCO_ActiveFlag == true
                                        && d.AMB_ActiveFlag == true && e.AMSE_ActiveFlg == true && f.ACST_ActiveFlg == true && g.ACST_ActiveFlg == true
                                        && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id)
                                        select new CollegeRuleSettingsDTO
                                        {
                                            EMRS_Id = b.EMRS_Id,
                                            ECYS_Id = b.ECYS_Id,
                                            AMCO_Id = a.AMCO_Id,
                                            AMB_Id = a.AMB_Id,
                                            AMSE_Id = a.AMSE_Id,
                                            ACSS_Id = a.ACSS_Id,
                                            ACST_Id = a.ACST_Id,
                                            ACST_SchmeType = g.ACST_SchmeType,
                                            ACSS_SchmeName = f.ACSS_SchmeName,
                                            AMCO_CourseName = c.AMCO_CourseName,
                                            AMB_BranchName = d.AMB_BranchName,
                                            AMSE_SEMName = e.AMSE_SEMName,
                                            EMRS_ActiveFlag = b.EMRS_ActiveFlag
                                        }).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeRuleSettingsDTO getbranch(CollegeRuleSettingsDTO data)
        {
            try
            {
                data.getbranch = (from a in _context.MasterCourseDMO
                                  from b in _context.ClgMasterCourseBranchMap
                                  from c in _context.ClgMasterBranchDMO
                                  where (a.AMCO_Id == b.AMCO_Id && b.AMB_Id == c.AMB_Id && a.AMCO_ActiveFlag == true
                                  && b.AMCOBM_ActiveFlg == true && c.AMB_ActiveFlag == true && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id
                                  && c.MI_Id == data.MI_Id && b.AMCO_Id == data.AMCO_Id)
                                  select c).Distinct().OrderBy(a => a.AMB_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeRuleSettingsDTO get_semesters(CollegeRuleSettingsDTO data)
        {
            try
            {
                data.getsemester = (from a in _context.MasterCourseDMO
                                    from b in _context.ClgMasterCourseBranchMap
                                    from c in _context.ClgMasterBranchDMO
                                    from d in _context.AdmCourseBranchSemesterMappingDMO
                                    from e in _context.CLG_Adm_Master_SemesterDMO
                                    where (a.AMCO_Id == b.AMCO_Id && b.AMB_Id == c.AMB_Id && d.AMCOBM_Id == b.AMCOBM_Id && d.AMSE_Id == e.AMSE_Id
                                    && a.AMCO_ActiveFlag == true && b.AMCOBM_ActiveFlg == true && c.AMB_ActiveFlag == true && d.AMCOBMS_ActiveFlg == true
                                    && e.AMSE_ActiveFlg == true && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id && e.MI_Id == data.MI_Id
                                    && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id)
                                    select e).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeRuleSettingsDTO get_subjectscheme(CollegeRuleSettingsDTO data)
        {
            try
            {
                data.getsubjectscheme = (from a in _context.Exm_Col_Yearly_SchemeDMO
                                         from b in _context.AdmCollegeSubjectSchemeDMO
                                         where (a.ACSS_Id == b.ACSS_Id && a.MI_Id == data.MI_Id && a.AMCO_Id == data.AMCO_Id
                                         && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id && a.ECYS_ActiveFlag == true && b.ACST_ActiveFlg == true)
                                         select b).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeRuleSettingsDTO get_schemetype(CollegeRuleSettingsDTO data)
        {
            try
            {
                data.getschemetype = (from a in _context.Exm_Col_Yearly_SchemeDMO
                                      from b in _context.AdmCollegeSubjectSchemeDMO
                                      from c in _context.AdmCollegeSchemeTypeDMO
                                      where (a.ACSS_Id == b.ACSS_Id && a.ACST_Id == c.ACST_Id && a.MI_Id == data.MI_Id && a.AMCO_Id == data.AMCO_Id
                                      && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id && a.ACSS_Id == data.ACSS_Id)
                                      select c).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeRuleSettingsDTO get_subjects(CollegeRuleSettingsDTO data)
        {
            try
            {
                data.getsubject = (from a in _context.Exm_Col_Yearly_SchemeDMO
                                   from b in _context.Exm_Col_Yearly_Scheme_GroupDMO
                                   from c in _context.Exm_Col_Yearly_Scheme_Group_SubjectsDMO
                                   from d in _context.IVRM_School_Master_SubjectsDMO
                                   where (a.ECYS_Id == b.ECYS_Id && b.ECYSG_Id == c.ECYSG_Id && d.ISMS_Id == c.ISMS_Id && a.ECYS_ActiveFlag == true
                                   && b.ECYSG_ActiveFlag == true && c.ECYSGS_ActiveFlag == true && d.ISMS_ActiveFlag == 1 && a.AMCO_Id == data.AMCO_Id
                                   && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id && a.ACSS_Id == data.ACSS_Id && a.ACST_Id == data.ACST_Id)
                                   select d).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();

                data.getexam = (from a in _context.Exm_Col_Yearly_Scheme_ExamsDMO
                                from b in _context.col_exammasterDMO
                                where (a.EME_Id == b.EME_Id && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id && a.ACSS_Id == data.ACSS_Id
                                && a.ACST_Id == data.ACST_Id && a.ECYSE_ActiveFlg == true && b.EME_ActiveFlag == true)
                                select b).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();

                data.getgrade = _context.col_Exm_Master_GradeDMO.Where(a => a.MI_Id == data.MI_Id && a.EMGR_ActiveFlag == true).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Save Data
        public CollegeRuleSettingsDTO saveddata(CollegeRuleSettingsDTO data)
        {
            try
            {
                var geteycid = _context.Exm_Col_Yearly_SchemeDMO.Where(ad => ad.MI_Id == data.MI_Id && ad.AMCO_Id == data.AMCO_Id && ad.AMB_Id == data.AMB_Id
                 && ad.AMSE_Id == data.AMSE_Id && ad.ACSS_Id == data.ACSS_Id && ad.ACST_Id == data.ACST_Id && ad.ECYS_ActiveFlag == true).Select(b => b.ECYS_Id).FirstOrDefault();


                var checkeycid = (from dd in _context.Clg_Exm_M_RuleSettingDMO
                                  where (dd.MI_Id == data.MI_Id && dd.ECYS_Id == geteycid)
                                  select new CollegeRuleSettingsDTO
                                  {
                                      EMRS_Id = dd.EMRS_Id
                                  }).Distinct().ToList();


                if (checkeycid.Count() > 0)
                {
                    var checkeycidresult = _context.Clg_Exm_M_RuleSettingDMO.Single(d => d.MI_Id == data.MI_Id && d.ECYS_Id == geteycid);

                    checkeycidresult.UpdatedDate = DateTime.UtcNow;
                    _context.Update(checkeycidresult);

                    foreach (var x in data.Temp_Subject_ListDTO)
                    {
                        var checksubjectid = _context.Clg_Exm_M_RuleSetting_SubjectsDMO.Where(a1 => a1.EMRS_Id == checkeycid.FirstOrDefault().EMRS_Id && a1.ISMS_Id == x.ISMS_Id).ToList();

                        if (checksubjectid.Count() > 0)
                        {
                            CollegeRuleSettingsDTO req = new CollegeRuleSettingsDTO();
                            req = delete_records_of_pro_cat(checksubjectid.FirstOrDefault().EMRSS_Id);

                            if (req.returnval == true)
                            {
                                Clg_Exm_M_RuleSetting_SubjectsDMO subdmo = new Clg_Exm_M_RuleSetting_SubjectsDMO();

                                subdmo.EMRS_Id = checkeycid.FirstOrDefault().EMRS_Id;
                                subdmo.ISMS_Id = x.ISMS_Id;
                                subdmo.EMGR_Id = x.EMGR_Id;
                                subdmo.EMRSS_MaxMarks = x.EMRSS_MaxMarks;
                                subdmo.EMRSS_MinMarks = x.EMRSS_MinMarks;
                                subdmo.EMRSS_ConvertForMarks = x.EMRSS_ConvertForMarks;
                                if (x.EMRSS_AppToResultFlg != true)
                                {
                                    subdmo.EMRSS_AppToResultFlg = false;
                                }
                                else
                                {
                                    subdmo.EMRSS_AppToResultFlg = x.EMRSS_AppToResultFlg;
                                }
                                subdmo.EMRSS_ActiveFlag = true;
                                subdmo.CreatedDate = DateTime.UtcNow;
                                subdmo.UpdatedDate = DateTime.UtcNow;
                                _context.Add(subdmo);


                                foreach (var y in data.Temp_Group_ListDTO)
                                {
                                    if (y.ISMS_Id == x.ISMS_Id)
                                    {
                                        Clg_Exm_M_RS_Subj_GroupDMO subgroup = new Clg_Exm_M_RS_Subj_GroupDMO();
                                        subgroup.EMRSS_Id = subdmo.EMRSS_Id;
                                        subgroup.EMRSSG_GroupName = y.EMRSSG_GroupName;
                                        subgroup.EMRSSG_DisplayName = y.EMRSSG_DisplayName;
                                        subgroup.EMRSSG_PercentValue = y.EMRSSG_PercentValue;
                                        subgroup.EMRSSG_MarksValue = y.EMRSSG_MarksValue;
                                        subgroup.EMRSSG_MaxOff = y.EMRSSG_MaxOff;
                                        subgroup.EMRSSG_BestOff = y.EMRSSG_BestOff;
                                        subgroup.EMRSSG_ActiveFlag = true;
                                        subgroup.CreatedDate = DateTime.UtcNow;
                                        subgroup.UpdatedDate = DateTime.UtcNow;
                                        _context.Add(subgroup);

                                        foreach (var z in y.Exm_M_Prom_Subj_Group_Exams_master)
                                        {
                                            Clg_Exm_M_RS_Subj_Group_ExamsDMO subgroupexam = new Clg_Exm_M_RS_Subj_Group_ExamsDMO();

                                            subgroupexam.EMRSSG_Id = subgroup.EMRSSG_Id;
                                            subgroupexam.EME_Id = z.EME_Id;
                                            subgroupexam.EMRSSGE_ActiveFlg = true;
                                            subgroupexam.CreatedDate = DateTime.UtcNow;
                                            subgroupexam.UpdatedDate = DateTime.UtcNow;
                                            _context.Add(subgroupexam);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            Clg_Exm_M_RuleSetting_SubjectsDMO subdmo = new Clg_Exm_M_RuleSetting_SubjectsDMO();

                            subdmo.EMRS_Id = checkeycid.FirstOrDefault().EMRS_Id;
                            subdmo.ISMS_Id = x.ISMS_Id;
                            subdmo.EMGR_Id = x.EMGR_Id;
                            subdmo.EMRSS_MaxMarks = x.EMRSS_MaxMarks;
                            subdmo.EMRSS_MinMarks = x.EMRSS_MinMarks;
                            subdmo.EMRSS_ConvertForMarks = x.EMRSS_ConvertForMarks;
                            if (x.EMRSS_AppToResultFlg != true)
                            {
                                subdmo.EMRSS_AppToResultFlg = false;
                            }
                            else
                            {
                                subdmo.EMRSS_AppToResultFlg = x.EMRSS_AppToResultFlg;
                            }
                            subdmo.EMRSS_ActiveFlag = true;
                            subdmo.CreatedDate = DateTime.UtcNow;
                            subdmo.UpdatedDate = DateTime.UtcNow;
                            _context.Add(subdmo);


                            foreach (var y in data.Temp_Group_ListDTO)
                            {
                                if (y.ISMS_Id == x.ISMS_Id)
                                {
                                    Clg_Exm_M_RS_Subj_GroupDMO subgroup = new Clg_Exm_M_RS_Subj_GroupDMO();
                                    subgroup.EMRSS_Id = subdmo.EMRSS_Id;
                                    subgroup.EMRSSG_GroupName = y.EMRSSG_GroupName;
                                    subgroup.EMRSSG_DisplayName = y.EMRSSG_DisplayName;
                                    subgroup.EMRSSG_PercentValue = y.EMRSSG_PercentValue;
                                    subgroup.EMRSSG_MarksValue = y.EMRSSG_MarksValue;
                                    subgroup.EMRSSG_MaxOff = y.EMRSSG_MaxOff;
                                    subgroup.EMRSSG_BestOff = y.EMRSSG_BestOff;
                                    subgroup.EMRSSG_ActiveFlag = true;
                                    subgroup.CreatedDate = DateTime.UtcNow;
                                    subgroup.UpdatedDate = DateTime.UtcNow;
                                    _context.Add(subgroup);

                                    foreach (var z in y.Exm_M_Prom_Subj_Group_Exams_master)
                                    {
                                        Clg_Exm_M_RS_Subj_Group_ExamsDMO subgroupexam = new Clg_Exm_M_RS_Subj_Group_ExamsDMO();

                                        subgroupexam.EMRSSG_Id = subgroup.EMRSSG_Id;
                                        subgroupexam.EME_Id = z.EME_Id;
                                        subgroupexam.EMRSSGE_ActiveFlg = true;
                                        subgroupexam.CreatedDate = DateTime.UtcNow;
                                        subgroupexam.UpdatedDate = DateTime.UtcNow;
                                        _context.Add(subgroupexam);
                                    }
                                }
                            }

                        }
                    }
                }
                else
                {
                    Clg_Exm_M_RuleSettingDMO rule = new Clg_Exm_M_RuleSettingDMO();

                    rule.MI_Id = data.MI_Id;
                    rule.ECYS_Id = Convert.ToInt64(geteycid);
                    rule.EMGR_Id = data.EMGR_Id;
                    rule.EMRS_MarksPerFlg = data.EMRS_MarksPerFlg;
                    rule.EMRS_ActiveFlag = true;
                    rule.CreatedDate = DateTime.UtcNow;
                    rule.UpdatedDate = DateTime.UtcNow;
                    _context.Add(rule);

                    foreach (var x in data.Temp_Subject_ListDTO)
                    {
                        Clg_Exm_M_RuleSetting_SubjectsDMO subdmo = new Clg_Exm_M_RuleSetting_SubjectsDMO();

                        subdmo.EMRS_Id = rule.EMRS_Id;
                        subdmo.ISMS_Id = x.ISMS_Id;
                        subdmo.EMGR_Id = x.EMGR_Id;
                        subdmo.EMRSS_MaxMarks = x.EMRSS_MaxMarks;
                        subdmo.EMRSS_MinMarks = x.EMRSS_MinMarks;
                        subdmo.EMRSS_ConvertForMarks = x.EMRSS_ConvertForMarks;
                        if (x.EMRSS_AppToResultFlg != true)
                        {
                            subdmo.EMRSS_AppToResultFlg = false;
                        }
                        else
                        {
                            subdmo.EMRSS_AppToResultFlg = x.EMRSS_AppToResultFlg;
                        }
                        subdmo.EMRSS_ActiveFlag = true;
                        subdmo.CreatedDate = DateTime.UtcNow;
                        subdmo.UpdatedDate = DateTime.UtcNow;
                        _context.Add(subdmo);


                        foreach (var y in data.Temp_Group_ListDTO)
                        {
                            if (y.ISMS_Id == x.ISMS_Id)
                            {
                                Clg_Exm_M_RS_Subj_GroupDMO subgroup = new Clg_Exm_M_RS_Subj_GroupDMO();
                                subgroup.EMRSS_Id = subdmo.EMRSS_Id;
                                subgroup.EMRSSG_GroupName = y.EMRSSG_GroupName;
                                subgroup.EMRSSG_DisplayName = y.EMRSSG_DisplayName;
                                subgroup.EMRSSG_PercentValue = y.EMRSSG_PercentValue;
                                subgroup.EMRSSG_MarksValue = y.EMRSSG_MarksValue;
                                subgroup.EMRSSG_MaxOff = y.EMRSSG_MaxOff;
                                subgroup.EMRSSG_BestOff = y.EMRSSG_BestOff;
                                subgroup.EMRSSG_ActiveFlag = true;
                                subgroup.CreatedDate = DateTime.UtcNow;
                                subgroup.UpdatedDate = DateTime.UtcNow;
                                _context.Add(subgroup);

                                foreach (var z in y.Exm_M_Prom_Subj_Group_Exams_master)
                                {
                                    Clg_Exm_M_RS_Subj_Group_ExamsDMO subgroupexam = new Clg_Exm_M_RS_Subj_Group_ExamsDMO();

                                    subgroupexam.EMRSSG_Id = subgroup.EMRSSG_Id;
                                    subgroupexam.EME_Id = z.EME_Id;
                                    subgroupexam.EMRSSGE_ActiveFlg = true;
                                    subgroupexam.CreatedDate = DateTime.UtcNow;
                                    subgroupexam.UpdatedDate = DateTime.UtcNow;
                                    _context.Add(subgroupexam);
                                }
                            }
                        }
                    }
                }


                var a = _context.SaveChanges();
                if (a > 0)
                {
                    data.message = "Add";
                    data.returnval = true;
                }
                else
                {
                    data.message = "Add";
                    data.returnval = false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //Edit
        public CollegeRuleSettingsDTO editdeatils(CollegeRuleSettingsDTO data)
        {
            try
            {
                data.geteditsubjectlist = (from a in _context.Clg_Exm_M_RuleSettingDMO
                                           from b in _context.Clg_Exm_M_RuleSetting_SubjectsDMO
                                           from c in _context.Exm_Col_Yearly_SchemeDMO
                                           from d in _context.IVRM_School_Master_SubjectsDMO
                                           from e in _context.MasterCourseDMO
                                           from f in _context.ClgMasterBranchDMO
                                           from g in _context.CLG_Adm_Master_SemesterDMO
                                           from h in _context.AdmCollegeSchemeTypeDMO
                                           from i in _context.AdmCollegeSubjectSchemeDMO
                                           from j in _context.col_Exm_Master_GradeDMO
                                           where (a.EMRS_Id == b.EMRS_Id && a.ECYS_Id == c.ECYS_Id && b.ISMS_Id == d.ISMS_Id && c.AMCO_Id == e.AMCO_Id
                                           && c.AMB_Id == f.AMB_Id && c.AMSE_Id == g.AMSE_Id && c.ACST_Id == h.ACST_Id && c.ACSS_Id == i.ACSS_Id
                                           && j.EMGR_Id == b.EMGR_Id && c.ECYS_ActiveFlag == true && d.ISMS_ActiveFlag == 1 && a.MI_Id == data.MI_Id
                                           && a.EMRS_Id == data.EMRS_Id && b.EMRS_Id == data.EMRS_Id)
                                           select new CollegeRuleSettingsDTO
                                           {
                                               EMRS_Id = b.EMRS_Id,
                                               ECYS_Id = a.ECYS_Id,
                                               AMCO_Id = c.AMCO_Id,
                                               AMB_Id = c.AMB_Id,
                                               AMSE_Id = c.AMSE_Id,
                                               ACSS_Id = c.ACSS_Id,
                                               ACST_Id = c.ACST_Id,
                                               ACST_SchmeType = h.ACST_SchmeType,
                                               ACSS_SchmeName = i.ACSS_SchmeName,
                                               AMCO_CourseName = e.AMCO_CourseName,
                                               AMB_BranchName = f.AMB_BranchName,
                                               AMSE_SEMName = g.AMSE_SEMName,
                                               ISMS_SubjectName = d.ISMS_SubjectName,
                                               ISMS_SubjectCode = d.ISMS_SubjectCode,
                                               EMRSS_MaxMarks = b.EMRSS_MaxMarks,
                                               EMRSS_MinMarks = b.EMRSS_MinMarks,
                                               EMRSS_ConvertForMarks = b.EMRSS_ConvertForMarks,
                                               EMRSS_AppToResultFlg = b.EMRSS_AppToResultFlg,
                                               EMRSS_ActiveFlag = b.EMRSS_AppToResultFlg,
                                               EMGR_GradeName = j.EMGR_GradeName,
                                               EMRSS_Id = b.EMRSS_Id
                                           }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // View Subject Data
        public CollegeRuleSettingsDTO getalldetailsviewrecords(CollegeRuleSettingsDTO data)
        {
            try
            {
                data.getviewsubjects = (from a in _context.Clg_Exm_M_RuleSettingDMO
                                        from b in _context.Clg_Exm_M_RuleSetting_SubjectsDMO
                                        from c in _context.Exm_Col_Yearly_SchemeDMO
                                        from d in _context.IVRM_School_Master_SubjectsDMO
                                        from e in _context.MasterCourseDMO
                                        from f in _context.ClgMasterBranchDMO
                                        from g in _context.CLG_Adm_Master_SemesterDMO
                                        from h in _context.AdmCollegeSchemeTypeDMO
                                        from i in _context.AdmCollegeSubjectSchemeDMO
                                        from j in _context.col_Exm_Master_GradeDMO
                                        where (a.EMRS_Id == b.EMRS_Id && a.ECYS_Id == c.ECYS_Id && b.ISMS_Id == d.ISMS_Id && c.AMCO_Id == e.AMCO_Id
                                        && c.AMB_Id == f.AMB_Id && c.AMSE_Id == g.AMSE_Id && c.ACST_Id == h.ACST_Id && c.ACSS_Id == i.ACSS_Id
                                        && j.EMGR_Id == b.EMGR_Id && c.ECYS_ActiveFlag == true && d.ISMS_ActiveFlag == 1 && a.MI_Id == data.MI_Id
                                        && a.EMRS_Id == data.EMRS_Id && b.EMRS_Id == data.EMRS_Id)
                                        select new CollegeRuleSettingsDTO
                                        {
                                            EMRS_Id = b.EMRS_Id,
                                            ECYS_Id = a.ECYS_Id,
                                            AMCO_Id = c.AMCO_Id,
                                            AMB_Id = c.AMB_Id,
                                            AMSE_Id = c.AMSE_Id,
                                            ACSS_Id = c.ACSS_Id,
                                            ACST_Id = c.ACST_Id,
                                            ACST_SchmeType = h.ACST_SchmeType,
                                            ACSS_SchmeName = i.ACSS_SchmeName,
                                            AMCO_CourseName = e.AMCO_CourseName,
                                            AMB_BranchName = f.AMB_BranchName,
                                            AMSE_SEMName = g.AMSE_SEMName,
                                            ISMS_SubjectName = d.ISMS_SubjectName,
                                            ISMS_SubjectCode = d.ISMS_SubjectCode,
                                            EMRSS_MaxMarks = b.EMRSS_MaxMarks,
                                            EMRSS_MinMarks = b.EMRSS_MinMarks,
                                            EMRSS_ConvertForMarks = b.EMRSS_ConvertForMarks,
                                            EMRSS_AppToResultFlg = b.EMRSS_AppToResultFlg,
                                            EMRSS_ActiveFlag = b.EMRSS_ActiveFlag,
                                            EMGR_GradeName = j.EMGR_GradeName,
                                            EMRSS_Id = b.EMRSS_Id


                                        }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // View Group data from subject
        public CollegeRuleSettingsDTO viewrecordspopup_subgrps(CollegeRuleSettingsDTO data)
        {
            try
            {
                data.getviewgroupdetails = (from a in _context.Clg_Exm_M_RuleSettingDMO
                                            from b in _context.Clg_Exm_M_RuleSetting_SubjectsDMO
                                            from c in _context.Exm_Col_Yearly_SchemeDMO
                                            from d in _context.IVRM_School_Master_SubjectsDMO
                                            from e in _context.MasterCourseDMO
                                            from f in _context.ClgMasterBranchDMO
                                            from g in _context.CLG_Adm_Master_SemesterDMO
                                            from h in _context.AdmCollegeSchemeTypeDMO
                                            from i in _context.AdmCollegeSubjectSchemeDMO
                                            from j in _context.col_Exm_Master_GradeDMO
                                            from k in _context.Clg_Exm_M_RS_Subj_GroupDMO
                                            where (a.EMRS_Id == b.EMRS_Id && a.ECYS_Id == c.ECYS_Id && b.ISMS_Id == d.ISMS_Id && c.AMCO_Id == e.AMCO_Id
                                            && k.EMRSS_Id == b.EMRSS_Id && c.AMB_Id == f.AMB_Id && c.AMSE_Id == g.AMSE_Id && c.ACST_Id == h.ACST_Id
                                            && c.ACSS_Id == i.ACSS_Id && j.EMGR_Id == b.EMGR_Id && c.ECYS_ActiveFlag == true && d.ISMS_ActiveFlag == 1
                                            && a.MI_Id == data.MI_Id && a.EMRS_Id == data.EMRS_Id && b.EMRS_Id == data.EMRS_Id && k.EMRSS_Id == data.EMRSS_Id)
                                            select new CollegeRuleSettingsDTO
                                            {
                                                EMRSSG_GroupName = k.EMRSSG_GroupName,
                                                EMRSSG_DisplayName = k.EMRSSG_DisplayName,
                                                EMRSSG_MaxOff = k.EMRSSG_MaxOff,
                                                EMRSSG_BestOff = k.EMRSSG_BestOff,
                                                EMRSSG_PercentValue = k.EMRSSG_PercentValue,
                                                EMRSSG_MarksValue = k.EMRSSG_MarksValue,
                                                EMRSSG_ActiveFlag = k.EMRSSG_ActiveFlag,
                                                EMRSSG_Id = k.EMRSSG_Id,
                                                ISMS_SubjectName = d.ISMS_SubjectName,
                                                EMRSS_Id = b.EMRSS_Id,
                                                EMRS_Id = b.EMRS_Id
                                            }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // View Exam data from subject group
        public CollegeRuleSettingsDTO getalldetailsviewrecords_sub_grp_exms(CollegeRuleSettingsDTO data)
        {
            try
            {
                data.getviewexamdetails = (from a in _context.Clg_Exm_M_RuleSettingDMO
                                           from b in _context.Clg_Exm_M_RuleSetting_SubjectsDMO
                                           from c in _context.Exm_Col_Yearly_SchemeDMO
                                           from d in _context.IVRM_School_Master_SubjectsDMO
                                           from e in _context.MasterCourseDMO
                                           from f in _context.ClgMasterBranchDMO
                                           from g in _context.CLG_Adm_Master_SemesterDMO
                                           from h in _context.AdmCollegeSchemeTypeDMO
                                           from i in _context.AdmCollegeSubjectSchemeDMO
                                           from j in _context.col_Exm_Master_GradeDMO
                                           from k in _context.Clg_Exm_M_RS_Subj_GroupDMO
                                           from l in _context.col_exammasterDMO
                                           from m in _context.Clg_Exm_M_RS_Subj_Group_ExamsDMO
                                           where (a.EMRS_Id == b.EMRS_Id && a.ECYS_Id == c.ECYS_Id && b.ISMS_Id == d.ISMS_Id && c.AMCO_Id == e.AMCO_Id
                                           && k.EMRSS_Id == b.EMRSS_Id && c.AMB_Id == f.AMB_Id && c.AMSE_Id == g.AMSE_Id && c.ACST_Id == h.ACST_Id
                                           && l.EME_Id == m.EME_Id && m.EMRSSG_Id == k.EMRSSG_Id && c.ACSS_Id == i.ACSS_Id && j.EMGR_Id == b.EMGR_Id
                                           && c.ECYS_ActiveFlag == true && d.ISMS_ActiveFlag == 1 && a.MI_Id == data.MI_Id && k.EMRSSG_Id == data.EMRSSG_Id
                                           && a.EMRS_Id == data.EMRS_Id && b.EMRSS_Id == data.EMRSS_Id)
                                           select new CollegeRuleSettingsDTO
                                           {
                                               EME_Id = l.EME_Id,
                                               EME_ExamName = l.EME_ExamName,
                                               EME_Order = l.EME_ExamOrder,
                                               EMRSSGE_Id = m.EMRSSGE_Id,
                                               EMRSSGE_ActiveFlg = m.EMRSSGE_ActiveFlg,
                                               EME_ExamCode = l.EME_ExamCode,
                                               EMRSSG_GroupName = k.EMRSSG_GroupName,
                                               EMRSS_Id = b.EMRSS_Id,
                                               EMRS_Id = b.EMRS_Id,
                                               EMRSSG_Id=k.EMRSSG_Id

                                           }).Distinct().OrderBy(a => a.EME_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Deactivate 
        public CollegeRuleSettingsDTO deactivate(CollegeRuleSettingsDTO data)
        {
            try
            {
                var result = _context.Clg_Exm_M_RuleSettingDMO.Single(a => a.MI_Id == data.MI_Id && a.EMRS_Id == data.EMRS_Id);
                if (result.EMRS_ActiveFlag == true)
                {
                    result.EMRS_ActiveFlag = false;
                }
                else
                {
                    result.EMRS_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.UtcNow;
                _context.Update(result);
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
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CollegeRuleSettingsDTO deactivatesubject(CollegeRuleSettingsDTO data)
        {
            try
            {
                var result = _context.Clg_Exm_M_RuleSetting_SubjectsDMO.Single(a => a.EMRSS_Id == data.EMRSS_Id);
                if (result.EMRSS_ActiveFlag == true)
                {
                    result.EMRSS_ActiveFlag = false;
                }
                else
                {
                    result.EMRSS_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.UtcNow;
                _context.Update(result);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CollegeRuleSettingsDTO deactivategroup(CollegeRuleSettingsDTO data)
        {
            try
            {
                var result = _context.Clg_Exm_M_RS_Subj_GroupDMO.Single(a => a.EMRSSG_Id == data.EMRSSG_Id);
                if (result.EMRSSG_ActiveFlag == true)
                {
                    result.EMRSSG_ActiveFlag = false;
                }
                else
                {
                    result.EMRSSG_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.UtcNow;
                _context.Update(result);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CollegeRuleSettingsDTO deactivateexam(CollegeRuleSettingsDTO data)
        {
            try
            {
                var result = _context.Clg_Exm_M_RS_Subj_Group_ExamsDMO.Single(a => a.EMRSSGE_Id == data.EMRSSGE_Id);
                if (result.EMRSSGE_ActiveFlg == true)
                {
                    result.EMRSSGE_ActiveFlg = false;
                }
                else
                {
                    result.EMRSSGE_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.UtcNow;
                _context.Update(result);
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        // Delete While saving
        public CollegeRuleSettingsDTO delete_records_of_pro_cat(long id)
        {
            CollegeRuleSettingsDTO pagert = new CollegeRuleSettingsDTO();



            var subjectdetails = _context.Clg_Exm_M_RuleSetting_SubjectsDMO.Where(a => a.EMRSS_Id == id).ToList();

            List<long> grpid = new List<long>();

            var getsubjectgroup = _context.Clg_Exm_M_RS_Subj_GroupDMO.Where(a => a.EMRSS_Id == id).ToList();

            foreach (var c in getsubjectgroup)
            {
                grpid.Add(c.EMRSSG_Id);
            }

            List<long> grpexamid = new List<long>();
            var getsubjectgroupexam = _context.Clg_Exm_M_RS_Subj_Group_ExamsDMO.Where(a => grpid.Contains(a.EMRSSG_Id)).ToList();

            foreach (var d in getsubjectgroupexam)
            {
                grpexamid.Add(d.EMRSSGE_Id);
            }

            if (getsubjectgroupexam.Any())
            {
                for (int i = 0; getsubjectgroupexam.Count > i; i++)
                {
                    _context.Remove(getsubjectgroupexam.ElementAt(i));
                }
            }

            if (getsubjectgroup.Any())
            {
                for (int i = 0; getsubjectgroup.Count > i; i++)
                {
                    _context.Remove(getsubjectgroup.ElementAt(i));
                }
            }
            if (subjectdetails.Any())
            {
                for (int i = 0; subjectdetails.Count > i; i++)
                {
                    _context.Remove(subjectdetails.ElementAt(i));
                }
            }
            var contactExists = _context.SaveChanges();
            if (contactExists > 0)
            {
                pagert.returnval = true;
            }
            else
            {
                pagert.returnval = false;
            }

            return pagert;
        }
    }
}
