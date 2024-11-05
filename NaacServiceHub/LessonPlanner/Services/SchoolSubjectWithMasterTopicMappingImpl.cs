using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model.com.vapstech.Exam.LessonPlanner;
using DomainModel.Model.NAAC.LessonPlanner;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Exam.LessonPlanner;
using PreadmissionDTOs.NAAC.LessonPlanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.com.vaps.LessonPlanner.Services
{
    public class SchoolSubjectWithMasterTopicMappingImpl : Interface.SchoolSubjectWithMasterTopicMappingInterface
    {
        public LessonplannerContext _context;
        ILogger<SchoolSubjectWithMasterTopicMappingImpl> _logger;

        public SchoolSubjectWithMasterTopicMappingImpl(LessonplannerContext context, ILogger<SchoolSubjectWithMasterTopicMappingImpl> _log)
        {
            _context = context;
            _logger = _log;
        }

        public SchoolSubjectWithMasterTopicMappingDTO Getdetails(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            try
            {
                data.getyear = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                var getuserdetails = _context.Staff_User_Login.Where(a => a.MI_Id == data.MI_Id && a.Id == data.Userid).ToList();

                var getinstitutiontype = _context.Institution.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToList();

                List<long> subjectid = new List<long>();

                if (getuserdetails.Count > 0)
                {
                    var getschoolorcollege = getinstitutiontype.FirstOrDefault().MI_SchoolCollegeFlag;
                    var getempcode = getuserdetails.FirstOrDefault().Emp_Code;
                    var loginid = getuserdetails.FirstOrDefault().IVRMSTAUL_Id;

                    List<SchoolSubjectWithMasterTopicMappingDTO> getsubjects = new List<SchoolSubjectWithMasterTopicMappingDTO>();

                    if (getschoolorcollege == "C")
                    {
                        getsubjects = (from a in _context.Adm_College_Atten_Login_DetailsDMO
                                       from c in _context.Adm_College_Atten_Login_UserDMO
                                       from d in _context.IVRM_School_Master_SubjectsDMO
                                       where (a.ACALU_Id == c.ACALU_Id && a.ISMS_Id == d.ISMS_Id && a.ACALD_ActiveFlag == true && d.ISMS_ActiveFlag == 1
                                       && c.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && c.HRME_Id == getempcode)
                                       select new SchoolSubjectWithMasterTopicMappingDTO
                                       {
                                           ISMS_Id = a.ISMS_Id
                                       }).Distinct().ToList();
                    }
                    else
                    {
                        getsubjects = (from a in _context.Exm_Login_PrivilegeDMO
                                       from c in _context.Exm_Login_Privilege_SubjectsDMO
                                       from d in _context.IVRM_School_Master_SubjectsDMO
                                       from e in _context.Staff_User_Login
                                       where (a.ELP_Id == c.ELP_Id && e.IVRMSTAUL_Id == a.Login_Id && c.ISMS_Id == d.ISMS_Id && a.ELP_ActiveFlg == true && d.ISMS_ActiveFlag == 1
                                       && a.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && c.ELPs_ActiveFlg == true && a.Login_Id == loginid && e.Emp_Code == getempcode)
                                       select new SchoolSubjectWithMasterTopicMappingDTO
                                       {
                                           ISMS_Id = c.ISMS_Id
                                       }).Distinct().ToList();
                    }

                    if (getsubjects.Count > 0)
                    {
                        foreach (var c in getsubjects)
                        {
                            subjectid.Add(c.ISMS_Id);
                        }
                    }

                    data.getsubjectlist = _context.IVRM_School_Master_SubjectsDMO.Where(a => a.MI_Id == data.MI_Id && a.ISMS_ActiveFlag == 1
                    && subjectid.Contains(a.ISMS_Id)).OrderBy(a => a.ISMS_OrderFlag).ToArray();

                    data.getdetails = (from a in _context.SchoolSubjectWithMasterTopicMapping
                                       from b in _context.MasterSchoolTopicDMO
                                       from c in _context.IVRM_School_Master_SubjectsDMO
                                       from d in _context.SchoolMasterUnitDMO
                                       from e in _context.AcademicYear
                                       from f in _context.AdmissionClass
                                       where (b.ISMS_Id == c.ISMS_Id && a.LPMMT_Id == b.LPMMT_Id && b.ISMS_Id == c.ISMS_Id && d.LPMU_Id == b.LPMU_Id
                                       && c.ISMS_ActiveFlag == 1 && b.ASMAY_Id == e.ASMAY_Id && b.ASMCL_Id == f.ASMCL_Id && b.LPMMT_ActiveFlag == true
                                       && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id && subjectid.Contains(c.ISMS_Id))
                                       select new SchoolSubjectWithMasterTopicMappingDTO
                                       {
                                           ISMS_SubjectName = c.ISMS_SubjectName,
                                           LPMMT_TopicName = b.LPMMT_TopicName,
                                           LPMT_TopicName = a.LPMT_TopicName,
                                           LPMT_TotalHrs = a.LPMT_TotalHrs,
                                           LPMT_LessonPlan = a.LPMT_LessonPlan,
                                           LPMT_TotalPeriods = a.LPMT_TotalPeriods,
                                           ISMS_Id = b.ISMS_Id,
                                           LPMT_Id = a.LPMT_Id,
                                           LPMMT_Id = a.LPMMT_Id,
                                           LPMU_UnitName = d.LPMU_UnitName,
                                           LPMU_Id = b.LPMU_Id,
                                           LPMT_ActiveFlag = a.LPMT_ActiveFlag,
                                           ASMAY_Year = e.ASMAY_Year,
                                           ASMCL_ClassName = f.ASMCL_ClassName,
                                           ASMAY_Order = e.ASMAY_Order,
                                           ASMAY_Id = b.ASMAY_Id,
                                           ASMCL_Id = b.ASMCL_Id
                                       }).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();
                }
                else
                {
                    data.getsubjectlist = _context.IVRM_School_Master_SubjectsDMO.Where(a => a.MI_Id == data.MI_Id && a.ISMS_ActiveFlag == 1).OrderBy(a => a.ISMS_OrderFlag).ToArray();

                    data.getdetails = (from a in _context.SchoolSubjectWithMasterTopicMapping
                                       from b in _context.MasterSchoolTopicDMO
                                       from c in _context.IVRM_School_Master_SubjectsDMO
                                       from d in _context.SchoolMasterUnitDMO
                                       from e in _context.AcademicYear
                                       from f in _context.AdmissionClass
                                       where (b.ISMS_Id == c.ISMS_Id && a.LPMMT_Id == b.LPMMT_Id && b.ISMS_Id == c.ISMS_Id && d.LPMU_Id == b.LPMU_Id
                                       && b.ASMAY_Id == e.ASMAY_Id && b.ASMCL_Id == f.ASMCL_Id && c.ISMS_ActiveFlag == 1 && b.LPMMT_ActiveFlag == true
                                       && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id)
                                       select new SchoolSubjectWithMasterTopicMappingDTO
                                       {
                                           ISMS_SubjectName = c.ISMS_SubjectName,
                                           LPMMT_TopicName = b.LPMMT_TopicName,
                                           LPMT_TopicName = a.LPMT_TopicName,
                                           LPMT_TotalHrs = a.LPMT_TotalHrs,
                                           LPMT_LessonPlan = a.LPMT_LessonPlan,
                                           LPMT_TotalPeriods = a.LPMT_TotalPeriods,
                                           ISMS_Id = b.ISMS_Id,
                                           LPMT_Id = a.LPMT_Id,
                                           LPMMT_Id = a.LPMMT_Id,
                                           LPMU_UnitName = d.LPMU_UnitName,
                                           LPMU_Id = b.LPMU_Id,
                                           LPMT_ActiveFlag = a.LPMT_ActiveFlag,
                                           ASMAY_Year = e.ASMAY_Year,
                                           ASMCL_ClassName = f.ASMCL_ClassName,
                                           ASMAY_Order = e.ASMAY_Order,
                                           ASMAY_Id = b.ASMAY_Id,
                                           ASMCL_Id = b.ASMCL_Id
                                       }).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SchoolSubjectWithMasterTopicMappingDTO onchangeyear(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            try
            {
                var getuserdetails = _context.Staff_User_Login.Where(a => a.MI_Id == data.MI_Id && a.Id == data.Userid).ToList();

                var getinstitutiontype = _context.Institution.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToList();

                List<long> classid = new List<long>();

                if (data.Flag == "1")
                {
                    if (getuserdetails.Count > 0)
                    {
                        var getschoolorcollege = getinstitutiontype.FirstOrDefault().MI_SchoolCollegeFlag;
                        var getempcode = getuserdetails.FirstOrDefault().Emp_Code;
                        var loginid = getuserdetails.FirstOrDefault().IVRMSTAUL_Id;

                        List<SchoolSubjectWithMasterTopicMappingDTO> getclasse = new List<SchoolSubjectWithMasterTopicMappingDTO>();

                        getclasse = (from a in _context.Exm_Login_PrivilegeDMO
                                     from c in _context.Exm_Login_Privilege_SubjectsDMO
                                     from d in _context.IVRM_School_Master_SubjectsDMO
                                     from e in _context.Staff_User_Login
                                     from f in _context.MasterSchoolTopicDMO
                                     from g in _context.SchoolSubjectWithMasterTopicMapping
                                     where (a.ELP_Id == c.ELP_Id && f.LPMMT_Id == g.LPMMT_Id && e.IVRMSTAUL_Id == a.Login_Id && c.ISMS_Id == d.ISMS_Id
                                     && a.ELP_ActiveFlg == true && d.ISMS_ActiveFlag == 1 && a.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id
                                     && c.ELPs_ActiveFlg == true && a.Login_Id == loginid && e.Emp_Code == getempcode && a.ASMAY_Id == data.ASMAY_Id
                                     && f.ASMAY_Id == data.ASMAY_Id)
                                     select new SchoolSubjectWithMasterTopicMappingDTO
                                     {
                                         ASMCL_Id = c.ASMCL_Id
                                     }).Distinct().ToList();

                        if (getclasse.Count > 0)
                        {
                            foreach (var c in getclasse)
                            {
                                classid.Add(c.ASMCL_Id);
                            }
                        }

                        data.getclass = _context.AdmissionClass.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true && classid.Contains(a.ASMCL_Id)).OrderBy(a => a.ASMCL_Order).ToArray();
                    }
                    else
                    {
                        data.getclass = (from a in _context.Masterclasscategory
                                         from b in _context.AcademicYear
                                         from c in _context.AdmissionClass
                                         from f in _context.MasterSchoolTopicDMO
                                         from g in _context.SchoolSubjectWithMasterTopicMapping
                                         where (a.ASMCL_Id == c.ASMCL_Id && f.LPMMT_Id == g.LPMMT_Id && c.ASMCL_Id == f.ASMCL_Id && a.ASMAY_Id == b.ASMAY_Id 
                                         && a.Is_Active == true && a.ASMAY_Id == data.ASMAY_Id && f.ASMAY_Id == data.ASMAY_Id)
                                         select c).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                    }
                }
                else
                {
                    if (getuserdetails.Count > 0)
                    {
                        var getschoolorcollege = getinstitutiontype.FirstOrDefault().MI_SchoolCollegeFlag;
                        var getempcode = getuserdetails.FirstOrDefault().Emp_Code;
                        var loginid = getuserdetails.FirstOrDefault().IVRMSTAUL_Id;

                        List<SchoolSubjectWithMasterTopicMappingDTO> getclasse = new List<SchoolSubjectWithMasterTopicMappingDTO>();

                        getclasse = (from a in _context.Exm_Login_PrivilegeDMO
                                     from c in _context.Exm_Login_Privilege_SubjectsDMO
                                     from d in _context.IVRM_School_Master_SubjectsDMO
                                     from e in _context.Staff_User_Login
                                     where (a.ELP_Id == c.ELP_Id && e.IVRMSTAUL_Id == a.Login_Id && c.ISMS_Id == d.ISMS_Id && a.ELP_ActiveFlg == true
                                     && d.ISMS_ActiveFlag == 1 && a.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && c.ELPs_ActiveFlg == true && a.Login_Id == loginid
                                     && e.Emp_Code == getempcode)
                                     select new SchoolSubjectWithMasterTopicMappingDTO
                                     {
                                         ASMCL_Id = c.ASMCL_Id
                                     }).Distinct().ToList();

                        if (getclasse.Count > 0)
                        {
                            foreach (var c in getclasse)
                            {
                                classid.Add(c.ASMCL_Id);
                            }
                        }

                        data.getclass = _context.AdmissionClass.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true && classid.Contains(a.ASMCL_Id)).OrderBy(a => a.ASMCL_Order).ToArray();
                    }
                    else
                    {
                        data.getclass = (from a in _context.Masterclasscategory
                                         from b in _context.AcademicYear
                                         from c in _context.AdmissionClass
                                         where (a.ASMCL_Id == c.ASMCL_Id && a.ASMAY_Id == b.ASMAY_Id && a.Is_Active == true && a.ASMAY_Id == data.ASMAY_Id)
                                         select c).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SchoolSubjectWithMasterTopicMappingDTO onchangeclass(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            try
            {
                var getuserdetails = _context.Staff_User_Login.Where(a => a.MI_Id == data.MI_Id && a.Id == data.Userid).ToList();

                var getinstitutiontype = _context.Institution.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToList();

                List<long> subjectid = new List<long>();

                if (data.Flag == "1")
                {
                    if (getuserdetails.Count > 0)
                    {
                        var getschoolorcollege = getinstitutiontype.FirstOrDefault().MI_SchoolCollegeFlag;
                        var getempcode = getuserdetails.FirstOrDefault().Emp_Code;
                        var loginid = getuserdetails.FirstOrDefault().IVRMSTAUL_Id;

                        List<SchoolSubjectWithMasterTopicMappingDTO> getsubjects = new List<SchoolSubjectWithMasterTopicMappingDTO>();

                        getsubjects = (from a in _context.Exm_Login_PrivilegeDMO
                                       from c in _context.Exm_Login_Privilege_SubjectsDMO
                                       from d in _context.IVRM_School_Master_SubjectsDMO
                                       from e in _context.Staff_User_Login
                                       from f in _context.MasterSchoolTopicDMO
                                       from g in _context.SchoolSubjectWithMasterTopicMapping
                                       where (a.ELP_Id == c.ELP_Id && f.LPMMT_Id == g.LPMMT_Id && f.ISMS_Id == d.ISMS_Id && e.IVRMSTAUL_Id == a.Login_Id
                                       && c.ISMS_Id == d.ISMS_Id && a.ELP_ActiveFlg == true && d.ISMS_ActiveFlag == 1 && a.MI_Id == data.MI_Id
                                       && d.MI_Id == data.MI_Id && c.ELPs_ActiveFlg == true && a.Login_Id == loginid && e.Emp_Code == getempcode
                                       && c.ASMCL_Id == data.ASMCL_Id && a.ASMAY_Id == data.ASMAY_Id && f.ASMCL_Id == data.ASMCL_Id && f.ASMAY_Id == data.ASMAY_Id)
                                       select new SchoolSubjectWithMasterTopicMappingDTO
                                       {
                                           ISMS_Id = c.ISMS_Id
                                       }).Distinct().ToList();

                        if (getsubjects.Count > 0)
                        {
                            foreach (var c in getsubjects)
                            {
                                subjectid.Add(c.ISMS_Id);
                            }
                        }

                        data.getsubjectlist = _context.IVRM_School_Master_SubjectsDMO.Where(a => a.MI_Id == data.MI_Id && a.ISMS_ActiveFlag == 1
                        && subjectid.Contains(a.ISMS_Id)).OrderBy(a => a.ISMS_OrderFlag).ToArray();
                    }
                    else
                    {
                        data.getsubjectlist = (from a in _context.Exm_Category_ClassDMO
                                               from b in _context.Exm_Yearly_CategoryDMO
                                               from c in _context.Exm_Yearly_Category_ExamsDMO
                                               from d in _context.Exm_Yrly_Cat_Exams_SubwiseDMO
                                               from e in _context.IVRM_School_Master_SubjectsDMO
                                               from f in _context.MasterSchoolTopicDMO
                                               from g in _context.SchoolSubjectWithMasterTopicMapping
                                               where (a.EMCA_Id == b.EMCA_Id && b.EYC_Id == c.EYC_Id && c.EYCE_Id == d.EYCE_Id && d.ISMS_Id == e.ISMS_Id
                                               && f.LPMMT_Id == g.LPMMT_Id && f.ISMS_Id == d.ISMS_Id && a.ECAC_ActiveFlag == true && b.EYC_ActiveFlg == true
                                               && c.EYCE_ActiveFlg == true && d.EYCES_ActiveFlg == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                               && a.ASMCL_Id == data.ASMCL_Id && b.ASMAY_Id == data.ASMAY_Id && f.ASMAY_Id == data.ASMAY_Id
                                               && f.ASMCL_Id == data.ASMCL_Id)
                                               select e).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();

                    }
                }
                else
                {
                    if (getuserdetails.Count > 0)
                    {
                        var getschoolorcollege = getinstitutiontype.FirstOrDefault().MI_SchoolCollegeFlag;
                        var getempcode = getuserdetails.FirstOrDefault().Emp_Code;
                        var loginid = getuserdetails.FirstOrDefault().IVRMSTAUL_Id;

                        List<SchoolSubjectWithMasterTopicMappingDTO> getsubjects = new List<SchoolSubjectWithMasterTopicMappingDTO>();

                        getsubjects = (from a in _context.Exm_Login_PrivilegeDMO
                                       from c in _context.Exm_Login_Privilege_SubjectsDMO
                                       from d in _context.IVRM_School_Master_SubjectsDMO
                                       from e in _context.Staff_User_Login
                                       where (a.ELP_Id == c.ELP_Id && e.IVRMSTAUL_Id == a.Login_Id && c.ISMS_Id == d.ISMS_Id && a.ELP_ActiveFlg == true
                                       && d.ISMS_ActiveFlag == 1 && a.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && c.ELPs_ActiveFlg == true && a.Login_Id == loginid
                                       && e.Emp_Code == getempcode && c.ASMCL_Id == data.ASMCL_Id && a.ASMAY_Id == data.ASMAY_Id)
                                       select new SchoolSubjectWithMasterTopicMappingDTO
                                       {
                                           ISMS_Id = c.ISMS_Id
                                       }).Distinct().ToList();

                        if (getsubjects.Count > 0)
                        {
                            foreach (var c in getsubjects)
                            {
                                subjectid.Add(c.ISMS_Id);
                            }
                        }

                        data.getsubjectlist = _context.IVRM_School_Master_SubjectsDMO.Where(a => a.MI_Id == data.MI_Id && a.ISMS_ActiveFlag == 1
                        && subjectid.Contains(a.ISMS_Id)).OrderBy(a => a.ISMS_OrderFlag).ToArray();
                    }
                    else
                    {
                        data.getsubjectlist = (from a in _context.Exm_Category_ClassDMO
                                               from b in _context.Exm_Yearly_CategoryDMO
                                               from c in _context.Exm_Yearly_Category_ExamsDMO
                                               from d in _context.Exm_Yrly_Cat_Exams_SubwiseDMO
                                               from e in _context.IVRM_School_Master_SubjectsDMO
                                               where (a.EMCA_Id == b.EMCA_Id && b.EYC_Id == c.EYC_Id && c.EYCE_Id == d.EYCE_Id && d.ISMS_Id == e.ISMS_Id
                                               && a.ECAC_ActiveFlag == true && b.EYC_ActiveFlg == true && c.EYCE_ActiveFlg == true && d.EYCES_ActiveFlg == true
                                               && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && b.ASMAY_Id == data.ASMAY_Id)
                                               select e).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SchoolSubjectWithMasterTopicMappingDTO onchangesubject(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            try
            {
                List<long> getid = new List<long>();

                if (data.Flag == "1")
                {
                    data.unitdetails = (from a in _context.IVRM_School_Master_SubjectsDMO
                                        from b in _context.MasterSchoolTopicDMO
                                        from c in _context.SchoolMasterUnitDMO
                                        from f in _context.MasterSchoolTopicDMO
                                        from g in _context.SchoolSubjectWithMasterTopicMapping
                                        where (c.LPMU_Id == b.LPMU_Id && f.LPMMT_Id == g.LPMMT_Id && a.ISMS_Id == b.ISMS_Id && a.ISMS_ActiveFlag == 1
                                        && b.LPMMT_ActiveFlag == true && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.ISMS_Id == data.ISMS_Id
                                        && b.ISMS_Id == data.ISMS_Id && b.ASMAY_Id == data.ASMAY_Id & b.ASMCL_Id == data.ASMCL_Id
                                        && f.ISMS_Id == data.ISMS_Id && f.ASMAY_Id == data.ASMAY_Id & f.ASMCL_Id == data.ASMCL_Id)
                                        select new SchoolSubjectWithMasterTopicMappingDTO
                                        {
                                            LPMU_UnitName = c.LPMU_UnitName,
                                            LPMU_Id = b.LPMU_Id,
                                            LPMU_Order = c.LPMU_Order
                                        }).Distinct().OrderBy(a => a.LPMU_Order).ToArray();
                }
                else
                {
                    data.unitdetails = (from a in _context.IVRM_School_Master_SubjectsDMO
                                        from b in _context.MasterSchoolTopicDMO
                                        from c in _context.SchoolMasterUnitDMO
                                        where (c.LPMU_Id == b.LPMU_Id && a.ISMS_Id == b.ISMS_Id && a.ISMS_ActiveFlag == 1 && b.LPMMT_ActiveFlag == true
                                        && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.ISMS_Id == data.ISMS_Id && b.ISMS_Id == data.ISMS_Id
                                        && b.ASMAY_Id == data.ASMAY_Id & b.ASMCL_Id == data.ASMCL_Id)
                                        select new SchoolSubjectWithMasterTopicMappingDTO
                                        {
                                            LPMU_UnitName = c.LPMU_UnitName,
                                            LPMU_Id = b.LPMU_Id,
                                            LPMU_Order = c.LPMU_Order
                                        }).Distinct().OrderBy(a => a.LPMU_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SchoolSubjectWithMasterTopicMappingDTO onchangeunit(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            try
            {
                List<long> getid = new List<long>();
                if (data.Flag == "1")
                {
                    data.topicdetails = (from a in _context.IVRM_School_Master_SubjectsDMO
                                         from b in _context.MasterSchoolTopicDMO
                                         from g in _context.SchoolSubjectWithMasterTopicMapping
                                         where (a.ISMS_Id == b.ISMS_Id && b.LPMMT_Id == g.LPMMT_Id && a.ISMS_ActiveFlag == 1 && b.LPMMT_ActiveFlag == true
                                         && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.ISMS_Id == data.ISMS_Id && b.ISMS_Id == data.ISMS_Id
                                         && b.LPMU_Id == data.LPMU_Id && b.ASMAY_Id == data.ASMAY_Id & b.ASMCL_Id == data.ASMCL_Id)
                                         select new SchoolSubjectWithMasterTopicMappingDTO
                                         {
                                             LPMMT_TopicName = b.LPMMT_TopicName,
                                             LPMMT_Id = b.LPMMT_Id,
                                             LPMMT_Order = b.LPMMT_Order
                                         }).Distinct().OrderBy(a => a.LPMMT_Order).ToArray();
                }
                else
                {
                    data.topicdetails = (from a in _context.IVRM_School_Master_SubjectsDMO
                                         from b in _context.MasterSchoolTopicDMO
                                         where (a.ISMS_Id == b.ISMS_Id && a.ISMS_ActiveFlag == 1 && b.LPMMT_ActiveFlag == true && a.MI_Id == data.MI_Id
                                         && b.MI_Id == data.MI_Id && a.ISMS_Id == data.ISMS_Id && b.ISMS_Id == data.ISMS_Id && b.LPMU_Id == data.LPMU_Id
                                         && b.ASMAY_Id == data.ASMAY_Id & b.ASMCL_Id == data.ASMCL_Id)
                                         select new SchoolSubjectWithMasterTopicMappingDTO
                                         {
                                             LPMMT_TopicName = b.LPMMT_TopicName,
                                             LPMMT_Id = b.LPMMT_Id,
                                             LPMMT_Order = b.LPMMT_Order
                                         }).Distinct().OrderBy(a => a.LPMMT_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SchoolSubjectWithMasterTopicMappingDTO savedetails(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            try
            {
                if (data.LPMT_Id > 0)
                {
                    var checkduplicate = _context.SchoolSubjectWithMasterTopicMapping.Where(a => a.MI_Id == data.MI_Id && a.LPMMT_Id == data.LPMMT_Id && a.LPMT_TopicName.Equals(data.LPMT_TopicName) && a.LPMT_Id != data.LPMT_Id).ToList();

                    if (checkduplicate.Count() > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        var checkduplicate1 = _context.SchoolSubjectWithMasterTopicMapping.Single(a => a.MI_Id == data.MI_Id && a.LPMMT_Id == data.LPMMT_Id
                        && a.LPMT_Id == data.LPMT_Id);
                        checkduplicate1.LPMT_TopicName = data.LPMT_TopicName;
                        checkduplicate1.LPMT_LessonPlan = data.LPMT_LessonPlan;
                        checkduplicate1.LPMT_TotalHrs = data.LPMT_TotalHrs;
                        checkduplicate1.LPMT_TotalPeriods = data.LPMT_TotalPeriods;
                        checkduplicate1.LPMT_TeacherGuide = data.LPMT_TeacherGuide;
                        checkduplicate1.LPMT_StudentGuide = data.LPMT_StudentGuide;
                        checkduplicate1.LPMT_MaterialNeeded = data.LPMT_MaterialNeeded;
                        checkduplicate1.LPMT_References = data.LPMT_References;
                        checkduplicate1.LPMT_Homework = data.LPMT_Homework;
                        checkduplicate1.LPMT_UpdatedBy = data.Userid;
                        checkduplicate1.UpdatedDate = DateTime.Now;
                        _context.Update(checkduplicate1);


                        if (data.TeacherGuideUploadDTO.Length > 0)
                        {
                            foreach (var t in data.TeacherGuideUploadDTO)
                            {
                                if (t.LPMTR_Resources != null && t.LPMTR_Resources != "")
                                {
                                    School_Topic_Resource_MappingDMO dmoteacher = new School_Topic_Resource_MappingDMO();

                                    dmoteacher.LPMT_Id = data.LPMT_Id;
                                    dmoteacher.LPMTR_FileName = t.LPMTR_FileName;
                                    dmoteacher.LPMTR_Resources = t.LPMTR_Resources;
                                    dmoteacher.LPMTR_ResourceType = "Teacher Guide";
                                    dmoteacher.CreatedDate = DateTime.Now;
                                    dmoteacher.UpdatedDate = DateTime.Now;
                                    dmoteacher.LPMTR_CreatedBy = data.Userid;
                                    dmoteacher.LPMTR_UpdatedBy = data.Userid;
                                    _context.Add(dmoteacher);
                                }
                            }
                        }
                        if (data.StudnetGuideUploadDTO.Length > 0)
                        {
                            foreach (var t in data.StudnetGuideUploadDTO)
                            {
                                if (t.LPMTR_Resources != null && t.LPMTR_Resources != "")
                                {
                                    School_Topic_Resource_MappingDMO dmostudent = new School_Topic_Resource_MappingDMO();

                                    dmostudent.LPMT_Id = data.LPMT_Id;
                                    dmostudent.LPMTR_FileName = t.LPMTR_FileName;
                                    dmostudent.LPMTR_Resources = t.LPMTR_Resources;
                                    dmostudent.LPMTR_ResourceType = "Student Guide";
                                    dmostudent.CreatedDate = DateTime.Now;
                                    dmostudent.UpdatedDate = DateTime.Now;
                                    dmostudent.LPMTR_CreatedBy = data.Userid;
                                    dmostudent.LPMTR_UpdatedBy = data.Userid;
                                    _context.Add(dmostudent);
                                }
                            }
                        }
                        if (data.MateralGuideUploadDTO.Length > 0)
                        {
                            foreach (var t in data.MateralGuideUploadDTO)
                            {
                                if (t.LPMTR_Resources != null && t.LPMTR_Resources != "")
                                {
                                    School_Topic_Resource_MappingDMO dmomaterial = new School_Topic_Resource_MappingDMO();

                                    dmomaterial.LPMT_Id = data.LPMT_Id;
                                    dmomaterial.LPMTR_FileName = t.LPMTR_FileName;
                                    dmomaterial.LPMTR_Resources = t.LPMTR_Resources;
                                    dmomaterial.LPMTR_ResourceType = "Materials Needed";
                                    dmomaterial.CreatedDate = DateTime.Now;
                                    dmomaterial.UpdatedDate = DateTime.Now;
                                    dmomaterial.LPMTR_CreatedBy = data.Userid;
                                    dmomaterial.LPMTR_UpdatedBy = data.Userid;
                                    _context.Add(dmomaterial);
                                }
                            }
                        }
                        if (data.ReferenceGuideUploadDTO.Length > 0)
                        {
                            foreach (var t in data.ReferenceGuideUploadDTO)
                            {
                                if (t.LPMTR_Resources != null && t.LPMTR_Resources != "")
                                {
                                    School_Topic_Resource_MappingDMO dmorefrence = new School_Topic_Resource_MappingDMO();

                                    dmorefrence.LPMT_Id = data.LPMT_Id;
                                    dmorefrence.LPMTR_FileName = t.LPMTR_FileName;
                                    dmorefrence.LPMTR_Resources = t.LPMTR_Resources;
                                    dmorefrence.LPMTR_ResourceType = "References";
                                    dmorefrence.CreatedDate = DateTime.Now;
                                    dmorefrence.UpdatedDate = DateTime.Now;
                                    dmorefrence.LPMTR_CreatedBy = data.Userid;
                                    dmorefrence.LPMTR_UpdatedBy = data.Userid;
                                    _context.Add(dmorefrence);
                                }

                            }
                        }

                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.message = "Update";
                            data.returnval = true;
                        }
                        else
                        {
                            data.message = "Update";
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                    var checkduplicate = _context.SchoolSubjectWithMasterTopicMapping.Where(a => a.MI_Id == data.MI_Id && a.LPMMT_Id == data.LPMMT_Id
                    && a.LPMT_TopicName.Equals(data.LPMT_TopicName)).ToList();

                    if (checkduplicate.Count() > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        var chekrowcount = _context.SchoolSubjectWithMasterTopicMapping.Where(a => a.MI_Id == data.MI_Id && a.LPMMT_Id == data.LPMMT_Id).Count();
                        int k = chekrowcount + 1;

                        SchoolSubjectWithMasterTopicMappingDMO dmo = new SchoolSubjectWithMasterTopicMappingDMO();
                        dmo.LPMT_TopicName = data.LPMT_TopicName;
                        dmo.MI_Id = data.MI_Id;
                        dmo.LPMMT_Id = data.LPMMT_Id;
                        dmo.LPMT_LessonPlan = data.LPMT_LessonPlan;
                        dmo.LPMT_TotalHrs = data.LPMT_TotalHrs;
                        dmo.LPMT_TotalPeriods = data.LPMT_TotalPeriods;
                        dmo.LPMT_TeacherGuide = data.LPMT_TeacherGuide;
                        dmo.LPMT_StudentGuide = data.LPMT_StudentGuide;
                        dmo.LPMT_MaterialNeeded = data.LPMT_MaterialNeeded;
                        dmo.LPMT_References = data.LPMT_References;
                        dmo.LPMT_Homework = data.LPMT_Homework;
                        dmo.LPMT_UpdatedBy = data.Userid;
                        dmo.UpdatedDate = DateTime.Now;
                        dmo.LPMT_CreatedBy = data.Userid;
                        dmo.CreatedDate = DateTime.Now;
                        dmo.LPMT_TopicOrder = k;
                        dmo.LPMT_ActiveFlag = true;
                        _context.Add(dmo);

                        if (data.TeacherGuideUploadDTO.Length > 0)
                        {
                            foreach (var t in data.TeacherGuideUploadDTO)
                            {
                                if (t.LPMTR_Resources != null && t.LPMTR_Resources != "")
                                {
                                    School_Topic_Resource_MappingDMO dmoteacher = new School_Topic_Resource_MappingDMO();

                                    dmoteacher.LPMT_Id = dmo.LPMT_Id;
                                    dmoteacher.LPMTR_FileName = t.LPMTR_FileName;
                                    dmoteacher.LPMTR_Resources = t.LPMTR_Resources;
                                    dmoteacher.LPMTR_ResourceType = "Teacher Guide";
                                    dmoteacher.CreatedDate = DateTime.Now;
                                    dmoteacher.UpdatedDate = DateTime.Now;
                                    dmoteacher.LPMTR_CreatedBy = data.Userid;
                                    dmoteacher.LPMTR_UpdatedBy = data.Userid;
                                    _context.Add(dmoteacher);
                                }
                            }
                        }
                        if (data.StudnetGuideUploadDTO.Length > 0)
                        {
                            foreach (var t in data.StudnetGuideUploadDTO)
                            {
                                if (t.LPMTR_Resources != null && t.LPMTR_Resources != "")
                                {
                                    School_Topic_Resource_MappingDMO dmostudent = new School_Topic_Resource_MappingDMO();

                                    dmostudent.LPMT_Id = dmo.LPMT_Id;
                                    dmostudent.LPMTR_FileName = t.LPMTR_FileName;
                                    dmostudent.LPMTR_Resources = t.LPMTR_Resources;
                                    dmostudent.LPMTR_ResourceType = "Student Guide";
                                    dmostudent.CreatedDate = DateTime.Now;
                                    dmostudent.UpdatedDate = DateTime.Now;
                                    dmostudent.LPMTR_CreatedBy = data.Userid;
                                    dmostudent.LPMTR_UpdatedBy = data.Userid;
                                    _context.Add(dmostudent);
                                }
                            }
                        }
                        if (data.MateralGuideUploadDTO.Length > 0)
                        {
                            foreach (var t in data.MateralGuideUploadDTO)
                            {
                                if (t.LPMTR_Resources != null && t.LPMTR_Resources != "")
                                {
                                    School_Topic_Resource_MappingDMO dmomaterial = new School_Topic_Resource_MappingDMO();

                                    dmomaterial.LPMT_Id = dmo.LPMT_Id;
                                    dmomaterial.LPMTR_FileName = t.LPMTR_FileName;
                                    dmomaterial.LPMTR_Resources = t.LPMTR_Resources;
                                    dmomaterial.LPMTR_ResourceType = "Materials Needed";
                                    dmomaterial.CreatedDate = DateTime.Now;
                                    dmomaterial.UpdatedDate = DateTime.Now;
                                    dmomaterial.LPMTR_CreatedBy = data.Userid;
                                    dmomaterial.LPMTR_UpdatedBy = data.Userid;
                                    _context.Add(dmomaterial);
                                }
                            }
                        }
                        if (data.ReferenceGuideUploadDTO.Length > 0)
                        {
                            foreach (var t in data.ReferenceGuideUploadDTO)
                            {
                                if (t.LPMTR_Resources != null && t.LPMTR_Resources != "")
                                {
                                    School_Topic_Resource_MappingDMO dmorefrence = new School_Topic_Resource_MappingDMO();

                                    dmorefrence.LPMT_Id = dmo.LPMT_Id;
                                    dmorefrence.LPMTR_FileName = t.LPMTR_FileName;
                                    dmorefrence.LPMTR_Resources = t.LPMTR_Resources;
                                    dmorefrence.LPMTR_ResourceType = "References";
                                    dmorefrence.CreatedDate = DateTime.Now;
                                    dmorefrence.UpdatedDate = DateTime.Now;
                                    dmorefrence.LPMTR_CreatedBy = data.Userid;
                                    dmorefrence.LPMTR_UpdatedBy = data.Userid;
                                    _context.Add(dmorefrence);
                                }
                            }
                        }

                        var i = _context.SaveChanges();
                        if (i > 0)
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
                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                data.message = "Error";
                _logger.LogInformation("Lesson Planner Subject Unit Topic Sub Topic : " + ex.Message);
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SchoolSubjectWithMasterTopicMappingDTO deactivate(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            try
            {
                var result = _context.SchoolSubjectWithMasterTopicMapping.Single(a => a.MI_Id == data.MI_Id && a.LPMT_Id == data.LPMT_Id);
                if (result.LPMT_ActiveFlag == true)
                {
                    result.LPMT_ActiveFlag = false;
                }
                else
                {
                    result.LPMT_ActiveFlag = true;
                }
                result.UpdatedDate = DateTime.Now;
                result.LPMT_UpdatedBy = data.Userid;
                _context.Update(result);
                int i = _context.SaveChanges();
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
                data.returnval = false;
            }
            return data;
        }
        public SchoolSubjectWithMasterTopicMappingDTO validateordernumber(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            try
            {
                if (data.SchoolSubjectWithMasterTopicMappingTemporderDTO.Count() > 0)
                {
                    int i = 0;
                    for (int k = 0; k < data.SchoolSubjectWithMasterTopicMappingTemporderDTO.Count(); k++)
                    {
                        i = i + 1;
                        var checkresult = _context.SchoolSubjectWithMasterTopicMapping.Single(a => a.MI_Id == data.MI_Id && a.LPMT_Id == data.SchoolSubjectWithMasterTopicMappingTemporderDTO[k].LPMT_Id && a.LPMMT_Id == data.SchoolSubjectWithMasterTopicMappingTemporderDTO[k].LPMMT_Id);
                        checkresult.LPMT_TopicOrder = i;
                        checkresult.LPMT_UpdatedBy = data.Userid;
                        checkresult.UpdatedDate = DateTime.Now;
                        _context.Update(checkresult);
                    }
                    var j = _context.SaveChanges();
                    if (j > 0)
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
        public SchoolSubjectWithMasterTopicMappingDTO editdeatils(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            try
            {
                data.geteditdetials = (from a in _context.SchoolSubjectWithMasterTopicMapping
                                       from b in _context.MasterSchoolTopicDMO
                                       from c in _context.IVRM_School_Master_SubjectsDMO
                                       where (a.LPMMT_Id == b.LPMMT_Id && b.ISMS_Id == c.ISMS_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id
                                       && c.MI_Id == data.MI_Id && a.LPMT_Id == data.LPMT_Id && a.LPMMT_Id == data.LPMMT_Id && b.ISMS_Id == data.ISMS_Id
                                       && c.ISMS_Id == data.ISMS_Id)
                                       select new SchoolSubjectWithMasterTopicMappingDTO
                                       {
                                           ISMS_SubjectName = c.ISMS_SubjectName,
                                           LPMMT_TopicName = b.LPMMT_TopicName,
                                           LPMT_TopicName = a.LPMT_TopicName,
                                           LPMT_TotalHrs = a.LPMT_TotalHrs,
                                           LPMT_TotalPeriods = a.LPMT_TotalPeriods,
                                           ISMS_Id = b.ISMS_Id,
                                           LPMT_Id = a.LPMT_Id,
                                           LPMMT_Id = a.LPMMT_Id,
                                           LPMT_TeacherGuide = a.LPMT_TeacherGuide,
                                           LPMT_StudentGuide = a.LPMT_StudentGuide,
                                           LPMT_MaterialNeeded = a.LPMT_MaterialNeeded,
                                           LPMT_References = a.LPMT_References,
                                           LPMT_Homework = a.LPMT_Homework,
                                           LPMT_LessonPlan = a.LPMT_LessonPlan,
                                           LPMU_Id = b.LPMU_Id,
                                           ASMAY_Id = b.ASMAY_Id,
                                           ASMCL_Id = b.ASMCL_Id
                                       }).Distinct().ToArray();

                data.topicdetails = (from a in _context.IVRM_School_Master_SubjectsDMO
                                     from b in _context.MasterSchoolTopicDMO
                                     where (a.ISMS_Id == b.ISMS_Id && a.ISMS_ActiveFlag == 1 && b.LPMMT_ActiveFlag == true && a.MI_Id == data.MI_Id
                                     && b.MI_Id == data.MI_Id && a.ISMS_Id == data.ISMS_Id && b.ISMS_Id == data.ISMS_Id && b.LPMU_Id == data.LPMU_Id)
                                     select new SchoolSubjectWithMasterTopicMappingDTO
                                     {
                                         LPMMT_TopicName = b.LPMMT_TopicName,
                                         LPMMT_Id = b.LPMMT_Id,
                                         LPMMT_Order = b.LPMMT_Order
                                     }).Distinct().OrderBy(a => a.LPMMT_Order).ToArray();


                data.unitdetails = (from a in _context.IVRM_School_Master_SubjectsDMO
                                    from b in _context.MasterSchoolTopicDMO
                                    from c in _context.SchoolMasterUnitDMO
                                    where (c.LPMU_Id == b.LPMU_Id && a.ISMS_Id == b.ISMS_Id && a.ISMS_ActiveFlag == 1 && b.LPMMT_ActiveFlag == true
                                    && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.ISMS_Id == data.ISMS_Id && b.ISMS_Id == data.ISMS_Id)
                                    select new SchoolSubjectWithMasterTopicMappingDTO
                                    {
                                        LPMU_UnitName = c.LPMU_UnitName,
                                        LPMU_Id = b.LPMU_Id,
                                        LPMU_Order = c.LPMU_Order
                                    }).Distinct().OrderBy(a => a.LPMU_Order).ToArray();



                var getuserdetails = _context.Staff_User_Login.Where(a => a.MI_Id == data.MI_Id && a.Id == data.Userid).ToList();

                var getinstitutiontype = _context.Institution.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToList();

                List<long> classid = new List<long>();

                if (getuserdetails.Count > 0)
                {
                    var getschoolorcollege = getinstitutiontype.FirstOrDefault().MI_SchoolCollegeFlag;
                    var getempcode = getuserdetails.FirstOrDefault().Emp_Code;
                    var loginid = getuserdetails.FirstOrDefault().IVRMSTAUL_Id;

                    List<SchoolSubjectWithMasterTopicMappingDTO> getclasse = new List<SchoolSubjectWithMasterTopicMappingDTO>();

                    getclasse = (from a in _context.Exm_Login_PrivilegeDMO
                                 from c in _context.Exm_Login_Privilege_SubjectsDMO
                                 from d in _context.IVRM_School_Master_SubjectsDMO
                                 from e in _context.Staff_User_Login
                                 where (a.ELP_Id == c.ELP_Id && e.IVRMSTAUL_Id == a.Login_Id && c.ISMS_Id == d.ISMS_Id && a.ELP_ActiveFlg == true
                                 && d.ISMS_ActiveFlag == 1 && a.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && c.ELPs_ActiveFlg == true && a.Login_Id == loginid
                                 && e.Emp_Code == getempcode)
                                 select new SchoolSubjectWithMasterTopicMappingDTO
                                 {
                                     ASMCL_Id = c.ASMCL_Id
                                 }).Distinct().ToList();

                    if (getclasse.Count > 0)
                    {
                        foreach (var c in getclasse)
                        {
                            classid.Add(c.ASMCL_Id);
                        }
                    }

                    data.getclass = _context.AdmissionClass.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true && classid.Contains(a.ASMCL_Id)).OrderBy(a => a.ASMCL_Order).ToArray();
                }
                else
                {
                    data.getclass = (from a in _context.Masterclasscategory
                                     from b in _context.AcademicYear
                                     from c in _context.AdmissionClass
                                     where (a.ASMCL_Id == c.ASMCL_Id && a.ASMAY_Id == b.ASMAY_Id && a.Is_Active == true && a.ASMAY_Id == data.ASMAY_Id)
                                     select c).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = false;
            }
            return data;
        }
        public SchoolSubjectWithMasterTopicMappingDTO onselecttopic(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            try
            {
                data.gettopicdetailsorder = _context.SchoolSubjectWithMasterTopicMapping.Where(a => a.MI_Id == data.MI_Id && a.LPMMT_Id == data.LPMMT_Id).OrderBy(a => a.LPMT_TopicOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SchoolSubjectWithMasterTopicMappingDTO viewuploadflies(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            try
            {
                data.uploadfiles = (from a in _context.SchoolSubjectWithMasterTopicMapping
                                    from b in _context.MasterSchoolTopicDMO
                                    from c in _context.IVRM_School_Master_SubjectsDMO
                                    from d in _context.School_Topic_Resource_MappingDMO
                                    where (b.ISMS_Id == c.ISMS_Id && a.LPMMT_Id == b.LPMMT_Id && b.ISMS_Id == c.ISMS_Id
                                    && c.ISMS_ActiveFlag == 1 && b.LPMMT_ActiveFlag == true && a.LPMT_Id == d.LPMT_Id
                                    && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id && d.LPMT_Id == data.LPMT_Id
                                    && d.LPMTR_ResourceType == data.LPMTR_ResourceType && b.ISMS_Id == data.ISMS_Id && b.LPMU_Id == data.LPMU_Id
                                    && a.LPMT_Id == data.LPMT_Id)
                                    select new SchoolSubjectWithMasterTopicMappingDTO
                                    {
                                        ISMS_SubjectName = c.ISMS_SubjectName,
                                        LPMMT_TopicName = b.LPMMT_TopicName,
                                        LPMT_TopicName = a.LPMT_TopicName,
                                        ISMS_Id = b.ISMS_Id,
                                        LPMT_Id = a.LPMT_Id,
                                        LPMMT_Id = a.LPMMT_Id,
                                        LPMTR_Resources = d.LPMTR_Resources,
                                        LPMTR_FileName = d.LPMTR_FileName,
                                        LPMTR_Id = d.LPMTR_Id,
                                        LPMU_Id = b.LPMU_Id
                                    }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SchoolSubjectWithMasterTopicMappingDTO deleteuploadfile(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            try
            {
                var checkdata = _context.School_Topic_Resource_MappingDMO.Where(a => a.LPMTR_Id == data.LPMTR_Id).ToList();
                if (checkdata.Count > 0)
                {
                    var result = _context.School_Topic_Resource_MappingDMO.Single(a => a.LPMTR_Id == data.LPMTR_Id);

                    _context.Remove(result);

                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }

                    data.uploadfiles = (from a in _context.SchoolSubjectWithMasterTopicMapping
                                        from b in _context.MasterSchoolTopicDMO
                                        from c in _context.IVRM_School_Master_SubjectsDMO
                                        from d in _context.School_Topic_Resource_MappingDMO
                                        where (b.ISMS_Id == c.ISMS_Id && a.LPMMT_Id == b.LPMMT_Id && b.ISMS_Id == c.ISMS_Id
                                        && c.ISMS_ActiveFlag == 1 && b.LPMMT_ActiveFlag == true && a.LPMT_Id == d.LPMT_Id
                                        && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id && d.LPMT_Id == data.LPMT_Id
                                        && d.LPMTR_ResourceType == data.LPMTR_ResourceType && b.ISMS_Id == data.ISMS_Id && b.LPMU_Id == data.LPMU_Id
                                        && a.LPMT_Id == data.LPMT_Id)
                                        select new SchoolSubjectWithMasterTopicMappingDTO
                                        {
                                            ISMS_SubjectName = c.ISMS_SubjectName,
                                            LPMMT_TopicName = b.LPMMT_TopicName,
                                            LPMT_TopicName = a.LPMT_TopicName,
                                            ISMS_Id = b.ISMS_Id,
                                            LPMT_Id = a.LPMT_Id,
                                            LPMMT_Id = a.LPMMT_Id,
                                            LPMTR_Resources = d.LPMTR_Resources,
                                            LPMTR_FileName = d.LPMTR_FileName,
                                            LPMTR_Id = d.LPMTR_Id
                                        }).Distinct().ToArray();
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

        // Topic Resource Mapping
        public SchoolSubjectWithMasterTopicMappingDTO Getdetailsmapping(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            try
            {
                data.getsubjectlist = _context.IVRM_School_Master_SubjectsDMO.Where(a => a.MI_Id == data.MI_Id && a.ISMS_ActiveFlag == 1).OrderBy(a => a.ISMS_OrderFlag).ToArray();

                data.getdetails = (from a in _context.SchoolSubjectWithMasterTopicMapping
                                   from b in _context.MasterSchoolTopicDMO
                                   from c in _context.IVRM_School_Master_SubjectsDMO
                                   from d in _context.School_Topic_Resource_MappingDMO
                                   where (b.ISMS_Id == c.ISMS_Id && a.LPMMT_Id == b.LPMMT_Id && b.ISMS_Id == c.ISMS_Id
                                   && c.ISMS_ActiveFlag == 1 && b.LPMMT_ActiveFlag == true && a.LPMT_Id == d.LPMT_Id
                                   && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id)
                                   select new SchoolSubjectWithMasterTopicMappingDTO
                                   {
                                       ISMS_SubjectName = c.ISMS_SubjectName,
                                       LPMMT_TopicName = b.LPMMT_TopicName,
                                       LPMT_TopicName = a.LPMT_TopicName,
                                       ISMS_Id = b.ISMS_Id,
                                       LPMT_Id = a.LPMT_Id,
                                       LPMMT_Id = a.LPMMT_Id,
                                       LPMTR_Resources = d.LPMTR_Resources,
                                       LPMTR_Id = d.LPMTR_Id
                                   }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SchoolSubjectWithMasterTopicMappingDTO onchangetopic(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            try
            {
                data.subtopicdetails = (from a in _context.SchoolSubjectWithMasterTopicMapping
                                        from b in _context.MasterSchoolTopicDMO
                                        from c in _context.IVRM_School_Master_SubjectsDMO
                                        where (b.ISMS_Id == c.ISMS_Id && a.LPMMT_Id == b.LPMMT_Id && b.ISMS_Id == c.ISMS_Id
                                        && c.ISMS_ActiveFlag == 1 && b.LPMMT_ActiveFlag == true
                                        && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id
                                        && b.ISMS_Id == data.ISMS_Id && a.LPMMT_Id == data.LPMMT_Id)
                                        select a
                                   ).Distinct().OrderBy(a => a.LPMT_TopicOrder).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SchoolSubjectWithMasterTopicMappingDTO onchangesubtopic(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            try
            {
                data.savedetails = _context.School_Topic_Resource_MappingDMO.Where(a => a.LPMT_Id == data.LPMT_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SchoolSubjectWithMasterTopicMappingDTO savemapping(SchoolSubjectWithMasterTopicMappingDTO data)
        {
            try
            {
                if (data.SchoolSubjectWithMasterTopicResourceMappingTempDTO.Count() > 0)
                {
                    List<long> temparr = new List<long>();
                    List<long> temparr1 = new List<long>();

                    foreach (SchoolSubjectWithMasterTopicResourceMappingTempDTO ph in data.SchoolSubjectWithMasterTopicResourceMappingTempDTO)
                    {
                        temparr.Add(ph.LPMTR_Id);
                    }

                    Array Phone_Noresultremove = _context.School_Topic_Resource_MappingDMO.Where(t => !temparr.Contains(t.LPMTR_Id) && t.LPMT_Id == data.LPMT_Id).ToArray();

                    foreach (School_Topic_Resource_MappingDMO ph1 in Phone_Noresultremove)
                    {
                        _context.Remove(ph1);
                    }


                    for (int k = 0; k < data.SchoolSubjectWithMasterTopicResourceMappingTempDTO.Count(); k++)
                    {
                        var checkdetails = _context.School_Topic_Resource_MappingDMO.Where(a => a.LPMT_Id == data.LPMT_Id && a.LPMTR_Id == data.SchoolSubjectWithMasterTopicResourceMappingTempDTO[k].LPMTR_Id).Count();

                        if (checkdetails > 0)
                        {
                            var result = _context.School_Topic_Resource_MappingDMO.Single(a => a.LPMT_Id == data.LPMT_Id && a.LPMTR_Id == data.SchoolSubjectWithMasterTopicResourceMappingTempDTO[k].LPMTR_Id);
                            result.LPMTR_Resources = data.SchoolSubjectWithMasterTopicResourceMappingTempDTO[k].LPMTR_Resources;
                            result.LPMTR_UpdatedBy = data.Userid;
                            result.UpdatedDate = DateTime.Now;
                            _context.Update(result);
                        }
                        else
                        {
                            School_Topic_Resource_MappingDMO dmo = new School_Topic_Resource_MappingDMO();
                            dmo.LPMT_Id = data.LPMT_Id;
                            dmo.LPMTR_Resources = data.SchoolSubjectWithMasterTopicResourceMappingTempDTO[k].LPMTR_Resources;
                            dmo.LPMTR_CreatedBy = data.Userid;
                            dmo.LPMTR_UpdatedBy = data.Userid;
                            dmo.CreatedDate = DateTime.Now;
                            dmo.UpdatedDate = DateTime.Now;
                            _context.Add(dmo);

                        }

                    }
                    var i = _context.SaveChanges();
                    if (i > 0)
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        // College 
        public CollegeSubjTopicMappingDTO Getcollegedetails(CollegeSubjTopicMappingDTO data)
        {
            try
            {
                data.getyear = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                var getuserdetails = _context.Staff_User_Login.Where(a => a.MI_Id == data.MI_Id && a.Id == data.LPMTC_CreatedBy).ToList();

                var getinstitutiontype = _context.Institution.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToList();

                List<long> subjectid = new List<long>();

                if (getuserdetails.Count > 0)
                {
                    var getschoolorcollege = getinstitutiontype.FirstOrDefault().MI_SchoolCollegeFlag;
                    var getempcode = getuserdetails.FirstOrDefault().Emp_Code;
                    var loginid = getuserdetails.FirstOrDefault().IVRMSTAUL_Id;

                    List<CollegeSubjTopicMappingDTO> getsubjects = new List<CollegeSubjTopicMappingDTO>();

                    if (getschoolorcollege == "C")
                    {
                        getsubjects = (from a in _context.Adm_College_Atten_Login_DetailsDMO
                                       from c in _context.Adm_College_Atten_Login_UserDMO
                                       from d in _context.IVRM_School_Master_SubjectsDMO
                                       where (a.ACALU_Id == c.ACALU_Id && a.ISMS_Id == d.ISMS_Id && a.ACALD_ActiveFlag == true && d.ISMS_ActiveFlag == 1
                                       && c.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && c.HRME_Id == getempcode)
                                       select new CollegeSubjTopicMappingDTO
                                       {
                                           ISMS_Id = a.ISMS_Id
                                       }).Distinct().ToList();
                    }
                    else
                    {
                        getsubjects = (from a in _context.Exm_Login_PrivilegeDMO
                                       from c in _context.Exm_Login_Privilege_SubjectsDMO
                                       from d in _context.IVRM_School_Master_SubjectsDMO
                                       from e in _context.Staff_User_Login
                                       where (a.ELP_Id == c.ELP_Id && e.IVRMSTAUL_Id == a.Login_Id && c.ISMS_Id == d.ISMS_Id && a.ELP_ActiveFlg == true && d.ISMS_ActiveFlag == 1
                                       && a.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && c.ELPs_ActiveFlg == true && a.Login_Id == loginid && e.Emp_Code == getempcode)
                                       select new CollegeSubjTopicMappingDTO
                                       {
                                           ISMS_Id = c.ISMS_Id
                                       }).Distinct().ToList();
                    }

                    if (getsubjects.Count > 0)
                    {
                        foreach (var c in getsubjects)
                        {
                            subjectid.Add(c.ISMS_Id);
                        }
                    }

                    data.getsubjectlist = _context.IVRM_School_Master_SubjectsDMO.Where(a => a.MI_Id == data.MI_Id && a.ISMS_ActiveFlag == 1
                    && subjectid.Contains(a.ISMS_Id)).OrderBy(a => a.ISMS_OrderFlag).ToArray();

                    data.getdetails = (from a in _context.LP_Master_Topic_CollegeDMO
                                       from b in _context.LP_Master_MainTopic_CollegeDMO
                                       from c in _context.IVRM_School_Master_SubjectsDMO
                                       from d in _context.SchoolMasterUnitDMO
                                       from e in _context.AcademicYear
                                       from f in _context.MasterCourseDMO
                                       from g in _context.ClgMasterBranchDMO
                                       from h in _context.CLG_Adm_Master_SemesterDMO
                                       where (b.ISMS_Id == c.ISMS_Id && a.LPMMTC_Id == b.LPMMTC_Id && b.ISMS_Id == c.ISMS_Id && d.LPMU_Id == b.LPMU_Id
                                       && c.ISMS_ActiveFlag == 1 && b.ASMAY_Id == e.ASMAY_Id && b.AMCO_Id == f.AMCO_Id && b.AMB_Id == g.AMB_Id
                                       && b.AMSE_Id == h.AMSE_Id && b.LPMMTC_ActiveFlg == true
                                       && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id && subjectid.Contains(c.ISMS_Id))
                                       select new CollegeSubjTopicMappingDTO
                                       {
                                           ISMS_SubjectName = c.ISMS_SubjectName,
                                           LPMMTC_TopicName = b.LPMMTC_TopicName,
                                           LPMTC_TopicName = a.LPMTC_TopicName,
                                           LPMTC_TotalHrs = a.LPMTC_TotalHrs,
                                           LPMTC_LessonPlan = a.LPMTC_LessonPlan,
                                           LPMTC_TotalPeriods = a.LPMTC_TotalPeriods,
                                           ISMS_Id = b.ISMS_Id,
                                           LPMTC_Id = a.LPMTC_Id,
                                           LPMMTC_Id = a.LPMMTC_Id,
                                           LPMU_UnitName = d.LPMU_UnitName,
                                           LPMU_Id = b.LPMU_Id,
                                           LPMTC_Activefalg = a.LPMTC_Activefalg,
                                           ASMAY_Year = e.ASMAY_Year,
                                           AMCO_CourseName = f.AMCO_CourseName,
                                           AMB_BranchName = g.AMB_BranchName,
                                           AMSE_SEMName = h.AMSE_SEMName,
                                           ASMAY_Order = e.ASMAY_Order,
                                           ASMAY_Id = b.ASMAY_Id,
                                           AMCO_Id = b.AMCO_Id,
                                           AMB_Id = b.AMB_Id,
                                           AMSE_Id = b.AMSE_Id
                                       }).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();
                }
                else
                {
                    data.getsubjectlist = _context.IVRM_School_Master_SubjectsDMO.Where(a => a.MI_Id == data.MI_Id && a.ISMS_ActiveFlag == 1).OrderBy(a => a.ISMS_OrderFlag).ToArray();

                    data.getdetails = (from a in _context.LP_Master_Topic_CollegeDMO
                                       from b in _context.LP_Master_MainTopic_CollegeDMO
                                       from c in _context.IVRM_School_Master_SubjectsDMO
                                       from d in _context.SchoolMasterUnitDMO
                                       from e in _context.AcademicYear
                                       from f in _context.MasterCourseDMO
                                       from g in _context.ClgMasterBranchDMO
                                       from h in _context.CLG_Adm_Master_SemesterDMO
                                       where (b.ISMS_Id == c.ISMS_Id && a.LPMMTC_Id == b.LPMMTC_Id && b.ISMS_Id == c.ISMS_Id && d.LPMU_Id == b.LPMU_Id
                                       && b.ASMAY_Id == e.ASMAY_Id && b.AMCO_Id == f.AMCO_Id && c.ISMS_ActiveFlag == 1 && b.LPMMTC_ActiveFlg == true
                                       && b.AMB_Id == g.AMB_Id && b.AMSE_Id == h.AMSE_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id)
                                       select new CollegeSubjTopicMappingDTO
                                       {
                                           ISMS_SubjectName = c.ISMS_SubjectName,
                                           LPMMTC_TopicName = b.LPMMTC_TopicName,
                                           LPMTC_TopicName = a.LPMTC_TopicName,
                                           LPMTC_TotalHrs = a.LPMTC_TotalHrs,
                                           LPMTC_LessonPlan = a.LPMTC_LessonPlan,
                                           LPMTC_TotalPeriods = a.LPMTC_TotalPeriods,
                                           ISMS_Id = b.ISMS_Id,
                                           LPMTC_Id = a.LPMTC_Id,
                                           LPMMTC_Id = a.LPMMTC_Id,
                                           LPMU_UnitName = d.LPMU_UnitName,
                                           LPMU_Id = b.LPMU_Id,
                                           LPMTC_Activefalg = a.LPMTC_Activefalg,
                                           ASMAY_Year = e.ASMAY_Year,
                                           AMCO_CourseName = f.AMCO_CourseName,
                                           AMB_BranchName = g.AMB_BranchName,
                                           AMSE_SEMName = h.AMSE_SEMName,
                                           ASMAY_Order = e.ASMAY_Order,
                                           ASMAY_Id = b.ASMAY_Id,
                                           AMCO_Id = b.AMCO_Id,
                                           AMB_Id = b.AMB_Id,
                                           AMSE_Id = b.AMSE_Id
                                       }).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeSubjTopicMappingDTO collegeonchangeyear(CollegeSubjTopicMappingDTO data)
        {
            try
            {
                var getuserdetails = _context.Staff_User_Login.Where(a => a.MI_Id == data.MI_Id && a.Id == data.LPMTC_CreatedBy).ToList();

                var getinstitutiontype = _context.Institution.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToList();

                if (data.Flag == "1")
                {
                    if (getuserdetails.Count > 0)
                    {
                        data.getcourse = (from a in _context.Adm_College_Atten_Login_UserDMO
                                          from b in _context.MasterCourseDMO
                                          from c in _context.AcademicYear
                                          from d in _context.Adm_College_Atten_Login_DetailsDMO
                                          from e in _context.LP_Master_MainTopic_CollegeDMO
                                          from f in _context.LP_Master_Topic_CollegeDMO
                                          where (a.ACALU_Id == d.ACALU_Id && d.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && d.ACALD_ActiveFlag == true
                                          && e.LPMMTC_Id == f.LPMMTC_Id && e.AMCO_Id == b.AMCO_Id && e.ASMAY_Id == data.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id
                                          && a.MI_Id == data.MI_Id && a.HRME_Id == getuserdetails.FirstOrDefault().Emp_Code)
                                          select b).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
                    }
                    else
                    {
                        data.getcourse = (from a in _context.CLG_Adm_College_AY_CourseDMO
                                          from b in _context.MasterCourseDMO
                                          from c in _context.AcademicYear
                                          from e in _context.LP_Master_MainTopic_CollegeDMO
                                          from f in _context.LP_Master_Topic_CollegeDMO
                                          where (a.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && a.ACAYC_ActiveFlag == true && e.LPMMTC_Id == f.LPMMTC_Id
                                          && e.AMCO_Id == b.AMCO_Id && e.ASMAY_Id == data.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)
                                          select b).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
                    }
                }
                else
                {
                    if (getuserdetails.Count > 0)
                    {
                        data.getcourse = (from a in _context.Adm_College_Atten_Login_UserDMO
                                          from b in _context.MasterCourseDMO
                                          from c in _context.AcademicYear
                                          from d in _context.Adm_College_Atten_Login_DetailsDMO
                                          where (a.ACALU_Id == d.ACALU_Id && d.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && d.ACALD_ActiveFlag == true
                                          && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.HRME_Id == getuserdetails.FirstOrDefault().Emp_Code)
                                          select b).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
                    }
                    else
                    {
                        data.getcourse = (from a in _context.CLG_Adm_College_AY_CourseDMO
                                          from b in _context.MasterCourseDMO
                                          from c in _context.AcademicYear
                                          where (a.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && a.ACAYC_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id
                                          && a.MI_Id == data.MI_Id)
                                          select b).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeSubjTopicMappingDTO collegeonchangecourse(CollegeSubjTopicMappingDTO data)
        {
            try
            {
                var getuserdetails = _context.Staff_User_Login.Where(a => a.MI_Id == data.MI_Id && a.Id == data.LPMTC_CreatedBy).ToList();

                var getinstitutiontype = _context.Institution.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToList();

                if (data.Flag == "1")
                {
                    if (getuserdetails.Count > 0)
                    {
                        data.getbranch = (from a in _context.Adm_College_Atten_Login_UserDMO
                                          from b in _context.MasterCourseDMO
                                          from c in _context.AcademicYear
                                          from d in _context.Adm_College_Atten_Login_DetailsDMO
                                          from e in _context.ClgMasterBranchDMO
                                          from f in _context.LP_Master_MainTopic_CollegeDMO
                                          from g in _context.LP_Master_Topic_CollegeDMO
                                          where (a.ACALU_Id == d.ACALU_Id && d.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && d.AMB_Id == e.AMB_Id
                                          && d.ACALD_ActiveFlag == true && f.LPMMTC_Id == g.LPMMTC_Id && f.AMCO_Id == b.AMCO_Id && e.AMB_Id == f.AMB_Id
                                          && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && d.AMCO_Id == data.AMCO_Id && f.ASMAY_Id == data.ASMAY_Id
                                          && f.AMCO_Id == data.AMCO_Id && a.HRME_Id == getuserdetails.FirstOrDefault().Emp_Code)
                                          select e).Distinct().OrderBy(a => a.AMB_Order).ToArray();
                    }
                    else
                    {
                        data.getbranch = (from a in _context.CLG_Adm_College_AY_CourseDMO
                                          from b in _context.MasterCourseDMO
                                          from c in _context.AcademicYear
                                          from d in _context.CLG_Adm_College_AY_Course_BranchDMO
                                          from e in _context.ClgMasterBranchDMO
                                          from f in _context.LP_Master_MainTopic_CollegeDMO
                                          from g in _context.LP_Master_Topic_CollegeDMO
                                          where (a.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && a.ACAYC_Id == d.ACAYC_Id && d.AMB_Id == e.AMB_Id
                                          && a.ACAYC_ActiveFlag == true && f.LPMMTC_Id == g.LPMMTC_Id && f.AMCO_Id == b.AMCO_Id && e.AMB_Id == f.AMB_Id
                                          && d.ACAYCB_ActiveFlag == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                          && a.AMCO_Id == data.AMCO_Id && f.ASMAY_Id == data.ASMAY_Id && f.AMCO_Id == data.AMCO_Id)
                                          select e).Distinct().OrderBy(a => a.AMB_Order).ToArray();
                    }
                }
                else
                {
                    if (getuserdetails.Count > 0)
                    {
                        data.getbranch = (from a in _context.Adm_College_Atten_Login_UserDMO
                                          from b in _context.MasterCourseDMO
                                          from c in _context.AcademicYear
                                          from d in _context.Adm_College_Atten_Login_DetailsDMO
                                          from e in _context.ClgMasterBranchDMO
                                          where (a.ACALU_Id == d.ACALU_Id && d.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && d.AMB_Id == e.AMB_Id
                                          && d.ACALD_ActiveFlag == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && d.AMCO_Id == data.AMCO_Id
                                          && a.HRME_Id == getuserdetails.FirstOrDefault().Emp_Code)
                                          select e).Distinct().OrderBy(a => a.AMB_Order).ToArray();
                    }
                    else
                    {
                        data.getbranch = (from a in _context.CLG_Adm_College_AY_CourseDMO
                                          from b in _context.MasterCourseDMO
                                          from c in _context.AcademicYear
                                          from d in _context.CLG_Adm_College_AY_Course_BranchDMO
                                          from e in _context.ClgMasterBranchDMO
                                          where (a.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && a.ACAYC_Id == d.ACAYC_Id && d.AMB_Id == e.AMB_Id
                                          && a.ACAYC_ActiveFlag == true && d.ACAYCB_ActiveFlag == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                          && a.AMCO_Id == data.AMCO_Id)
                                          select e).Distinct().OrderBy(a => a.AMB_Order).ToArray();
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeSubjTopicMappingDTO collegeonchangebranch(CollegeSubjTopicMappingDTO data)
        {
            try
            {
                var getuserdetails = _context.Staff_User_Login.Where(a => a.MI_Id == data.MI_Id && a.Id == data.LPMTC_CreatedBy).ToList();

                var getinstitutiontype = _context.Institution.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToList();

                if (data.Flag == "1")
                {
                    if (getuserdetails.Count > 0)
                    {
                        data.getsemester = (from a in _context.Adm_College_Atten_Login_UserDMO
                                            from b in _context.MasterCourseDMO
                                            from c in _context.AcademicYear
                                            from d in _context.Adm_College_Atten_Login_DetailsDMO
                                            from e in _context.ClgMasterBranchDMO
                                            from g in _context.CLG_Adm_Master_SemesterDMO
                                            from h in _context.LP_Master_MainTopic_CollegeDMO
                                            from i in _context.LP_Master_Topic_CollegeDMO
                                            where (a.ACALU_Id == d.ACALU_Id && d.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && d.AMB_Id == e.AMB_Id
                                            && g.AMSE_Id == d.AMSE_Id && h.LPMMTC_Id == i.LPMMTC_Id && h.AMCO_Id == b.AMCO_Id && h.AMB_Id == e.AMB_Id
                                            && g.AMSE_Id == h.AMSE_Id && a.MI_Id == data.MI_Id && d.ACALD_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id
                                            && d.AMCO_Id == data.AMCO_Id && d.AMB_Id == data.AMB_Id && h.ASMAY_Id == data.ASMAY_Id
                                            && h.AMCO_Id == data.AMCO_Id && h.AMB_Id == data.AMB_Id && a.HRME_Id == getuserdetails.FirstOrDefault().Emp_Code)
                                            select g).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();

                    }
                    else
                    {
                        data.getsemester = (from a in _context.CLG_Adm_College_AY_CourseDMO
                                            from b in _context.MasterCourseDMO
                                            from c in _context.AcademicYear
                                            from d in _context.CLG_Adm_College_AY_Course_BranchDMO
                                            from e in _context.ClgMasterBranchDMO
                                            from f in _context.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                            from g in _context.CLG_Adm_Master_SemesterDMO
                                            from h in _context.LP_Master_MainTopic_CollegeDMO
                                            from i in _context.LP_Master_Topic_CollegeDMO
                                            where (a.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && a.ACAYC_Id == d.ACAYC_Id && d.AMB_Id == e.AMB_Id
                                            && d.ACAYCB_Id == f.ACAYCB_Id && f.AMSE_Id == g.AMSE_Id && f.ACAYCBS_ActiveFlag == true && h.LPMMTC_Id == i.LPMMTC_Id
                                            && h.AMCO_Id == b.AMCO_Id && h.AMB_Id == e.AMB_Id && g.AMSE_Id == h.AMSE_Id && a.MI_Id == data.MI_Id
                                            && a.ACAYC_ActiveFlag == true && d.ACAYCB_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id
                                            && h.ASMAY_Id == data.ASMAY_Id && h.AMCO_Id == data.AMCO_Id && h.AMB_Id == data.AMB_Id && d.AMB_Id == data.AMB_Id)
                                            select g).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();
                    }

                }
                else
                {
                    if (getuserdetails.Count > 0)
                    {
                        data.getsemester = (from a in _context.Adm_College_Atten_Login_UserDMO
                                            from b in _context.MasterCourseDMO
                                            from c in _context.AcademicYear
                                            from d in _context.Adm_College_Atten_Login_DetailsDMO
                                            from e in _context.ClgMasterBranchDMO
                                            from g in _context.CLG_Adm_Master_SemesterDMO
                                            where (a.ACALU_Id == d.ACALU_Id && d.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && d.AMB_Id == e.AMB_Id
                                            && g.AMSE_Id == d.AMSE_Id && a.MI_Id == data.MI_Id && d.ACALD_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id
                                            && d.AMCO_Id == data.AMCO_Id && d.AMB_Id == data.AMB_Id && a.HRME_Id == getuserdetails.FirstOrDefault().Emp_Code)
                                            select g).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();

                    }
                    else
                    {
                        data.getsemester = (from a in _context.CLG_Adm_College_AY_CourseDMO
                                            from b in _context.MasterCourseDMO
                                            from c in _context.AcademicYear
                                            from d in _context.CLG_Adm_College_AY_Course_BranchDMO
                                            from e in _context.ClgMasterBranchDMO
                                            from f in _context.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                            from g in _context.CLG_Adm_Master_SemesterDMO
                                            where (a.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && a.ACAYC_Id == d.ACAYC_Id && d.AMB_Id == e.AMB_Id
                                            && d.ACAYCB_Id == f.ACAYCB_Id && f.AMSE_Id == g.AMSE_Id && f.ACAYCBS_ActiveFlag == true && a.MI_Id == data.MI_Id
                                            && a.ACAYC_ActiveFlag == true && d.ACAYCB_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id
                                            && d.AMB_Id == data.AMB_Id)
                                            select g).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeSubjTopicMappingDTO collegeonchangesemester(CollegeSubjTopicMappingDTO data)
        {
            try
            {
                var getuserdetails = _context.Staff_User_Login.Where(a => a.MI_Id == data.MI_Id && a.Id == data.LPMTC_CreatedBy).ToList();

                var getinstitutiontype = _context.Institution.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToList();

                if (data.Flag == "1")
                {
                    if (getuserdetails.Count > 0)
                    {
                        data.getsubjectlist = (from a in _context.Adm_College_Atten_Login_UserDMO
                                               from b in _context.MasterCourseDMO
                                               from c in _context.AcademicYear
                                               from d in _context.Adm_College_Atten_Login_DetailsDMO
                                               from e in _context.ClgMasterBranchDMO
                                               from g in _context.CLG_Adm_Master_SemesterDMO
                                               from i in _context.IVRM_School_Master_SubjectsDMO
                                               from j in _context.LP_Master_MainTopic_CollegeDMO
                                               from k in _context.LP_Master_Topic_CollegeDMO
                                               where (a.ACALU_Id == d.ACALU_Id && d.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && d.AMB_Id == e.AMB_Id
                                               && g.AMSE_Id == d.AMSE_Id && i.ISMS_Id == d.ISMS_Id && j.LPMMTC_Id == k.LPMMTC_Id && j.AMCO_Id == b.AMCO_Id
                                               && j.AMB_Id == e.AMB_Id && j.AMSE_Id == g.AMSE_Id && j.ISMS_Id == i.ISMS_Id && a.MI_Id == data.MI_Id
                                               && d.ACALD_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id && d.AMCO_Id == data.AMCO_Id && d.AMB_Id == data.AMB_Id
                                               && d.AMSE_Id == data.AMSE_Id && j.ASMAY_Id == data.ASMAY_Id && j.AMCO_Id == data.AMCO_Id && j.AMB_Id == data.AMB_Id
                                               && j.AMSE_Id == data.AMSE_Id && a.HRME_Id == getuserdetails.FirstOrDefault().Emp_Code)
                                               select i).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();
                    }
                    else
                    {
                        data.getsubjectlist = (from j in _context.LP_Master_MainTopic_CollegeDMO
                                               from k in _context.LP_Master_Topic_CollegeDMO
                                               from l in _context.IVRM_School_Master_SubjectsDMO
                                               where (j.LPMMTC_Id == k.LPMMTC_Id && j.ISMS_Id == l.ISMS_Id && j.ASMAY_Id == data.ASMAY_Id
                                               && j.AMCO_Id == data.AMCO_Id && j.AMB_Id == data.AMB_Id && j.AMSE_Id == data.AMSE_Id && l.MI_Id == data.MI_Id
                                               && l.ISMS_ActiveFlag == 1)
                                               select l).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();
                    }

                }
                else
                {
                    if (getuserdetails.Count > 0)
                    {
                        data.getsubjectlist = (from a in _context.Adm_College_Atten_Login_UserDMO
                                               from b in _context.MasterCourseDMO
                                               from c in _context.AcademicYear
                                               from d in _context.Adm_College_Atten_Login_DetailsDMO
                                               from e in _context.ClgMasterBranchDMO
                                               from g in _context.CLG_Adm_Master_SemesterDMO
                                               from i in _context.IVRM_School_Master_SubjectsDMO
                                               where (a.ACALU_Id == d.ACALU_Id && d.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && d.AMB_Id == e.AMB_Id
                                               && g.AMSE_Id == d.AMSE_Id && i.ISMS_Id == d.ISMS_Id && a.MI_Id == data.MI_Id && d.ACALD_ActiveFlag == true
                                               && a.ASMAY_Id == data.ASMAY_Id && d.AMCO_Id == data.AMCO_Id && d.AMB_Id == data.AMB_Id && d.AMSE_Id == data.AMSE_Id
                                               && a.HRME_Id == getuserdetails.FirstOrDefault().Emp_Code)
                                               select i).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();
                    }
                    else
                    {
                        data.getsubjectlist = _context.IVRM_School_Master_SubjectsDMO.Where(a => a.MI_Id == data.MI_Id && a.ISMS_ActiveFlag == 1).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeSubjTopicMappingDTO onchangecollegesubject(CollegeSubjTopicMappingDTO data)
        {

            try
            {
                List<long> getid = new List<long>();

                if (data.Flag == "1")
                {
                    data.unitdetails = (from a in _context.IVRM_School_Master_SubjectsDMO
                                        from b in _context.LP_Master_MainTopic_CollegeDMO
                                        from c in _context.SchoolMasterUnitDMO
                                        from d in _context.LP_Master_Topic_CollegeDMO
                                        where (c.LPMU_Id == b.LPMU_Id && b.LPMMTC_Id == d.LPMMTC_Id && a.ISMS_Id == b.ISMS_Id && a.ISMS_ActiveFlag == 1
                                        && b.LPMMTC_ActiveFlg == true && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.ISMS_Id == data.ISMS_Id
                                        && b.ISMS_Id == data.ISMS_Id && b.ASMAY_Id == data.ASMAY_Id & b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id
                                        && b.AMSE_Id == data.AMSE_Id)
                                        select new CollegeSubjTopicMappingDTO
                                        {
                                            LPMU_UnitName = c.LPMU_UnitName,
                                            LPMU_Id = b.LPMU_Id,
                                            LPMU_Order = c.LPMU_Order
                                        }).Distinct().OrderBy(a => a.LPMU_Order).ToArray();
                }
                else
                {
                    data.unitdetails = (from a in _context.IVRM_School_Master_SubjectsDMO
                                        from b in _context.LP_Master_MainTopic_CollegeDMO
                                        from c in _context.SchoolMasterUnitDMO
                                        where (c.LPMU_Id == b.LPMU_Id && a.ISMS_Id == b.ISMS_Id && a.ISMS_ActiveFlag == 1 && b.LPMMTC_ActiveFlg == true
                                        && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.ISMS_Id == data.ISMS_Id && b.ISMS_Id == data.ISMS_Id
                                        && b.ASMAY_Id == data.ASMAY_Id & b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && b.AMSE_Id == data.AMSE_Id)
                                        select new CollegeSubjTopicMappingDTO
                                        {
                                            LPMU_UnitName = c.LPMU_UnitName,
                                            LPMU_Id = b.LPMU_Id,
                                            LPMU_Order = c.LPMU_Order
                                        }).Distinct().OrderBy(a => a.LPMU_Order).ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeSubjTopicMappingDTO onchangecollegeunit(CollegeSubjTopicMappingDTO data)
        {

            try
            {
                List<long> getid = new List<long>();

                if (data.Flag == "1")
                {
                    data.topicdetails = (from a in _context.IVRM_School_Master_SubjectsDMO
                                         from b in _context.LP_Master_MainTopic_CollegeDMO
                                         from c in _context.LP_Master_Topic_CollegeDMO
                                         where (a.ISMS_Id == b.ISMS_Id && b.LPMMTC_Id == c.LPMMTC_Id && a.ISMS_ActiveFlag == 1 && b.LPMMTC_ActiveFlg == true
                                         && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.ISMS_Id == data.ISMS_Id && b.ISMS_Id == data.ISMS_Id
                                         && b.LPMU_Id == data.LPMU_Id && b.ASMAY_Id == data.ASMAY_Id & b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id
                                         && b.AMSE_Id == data.AMSE_Id)
                                         select new CollegeSubjTopicMappingDTO
                                         {
                                             LPMMTC_TopicName = b.LPMMTC_TopicName,
                                             LPMMTC_Id = b.LPMMTC_Id,
                                             LPMMTC_Order = b.LPMMTC_Order
                                         }).Distinct().OrderBy(a => a.LPMMTC_Order).ToArray();
                }
                else
                {
                    data.topicdetails = (from a in _context.IVRM_School_Master_SubjectsDMO
                                         from b in _context.LP_Master_MainTopic_CollegeDMO
                                         where (a.ISMS_Id == b.ISMS_Id && a.ISMS_ActiveFlag == 1 && b.LPMMTC_ActiveFlg == true && a.MI_Id == data.MI_Id
                                         && b.MI_Id == data.MI_Id && a.ISMS_Id == data.ISMS_Id && b.ISMS_Id == data.ISMS_Id && b.LPMU_Id == data.LPMU_Id
                                         && b.ASMAY_Id == data.ASMAY_Id & b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && b.AMSE_Id == data.AMSE_Id)
                                         select new CollegeSubjTopicMappingDTO
                                         {
                                             LPMMTC_TopicName = b.LPMMTC_TopicName,
                                             LPMMTC_Id = b.LPMMTC_Id,
                                             LPMMTC_Order = b.LPMMTC_Order
                                         }).Distinct().OrderBy(a => a.LPMMTC_Order).ToArray();
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeSubjTopicMappingDTO savecollegedetails(CollegeSubjTopicMappingDTO data)
        {

            try
            {
                if (data.LPMTC_Id > 0)
                {
                    var checkduplicate = _context.LP_Master_Topic_CollegeDMO.Where(a => a.MI_Id == data.MI_Id && a.LPMMTC_Id == data.LPMMTC_Id
                    && a.LPMTC_TopicName.Equals(data.LPMTC_TopicName) && a.LPMTC_Id != data.LPMTC_Id).ToList();

                    if (checkduplicate.Count() > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        var checkduplicate1 = _context.LP_Master_Topic_CollegeDMO.Single(a => a.MI_Id == data.MI_Id && a.LPMMTC_Id == data.LPMMTC_Id
                        && a.LPMTC_Id == data.LPMTC_Id);
                        checkduplicate1.LPMTC_TopicName = data.LPMTC_TopicName;
                        checkduplicate1.LPMTC_LessonPlan = data.LPMTC_LessonPlan;
                        checkduplicate1.LPMTC_TotalHrs = data.LPMTC_TotalHrs;
                        checkduplicate1.LPMTC_TotalPeriods = data.LPMTC_TotalPeriods;
                        checkduplicate1.LPMTC_TeacherGuide = data.LPMTC_TeacherGuide;
                        checkduplicate1.LPMTC_StudentGuide = data.LPMTC_StudentGuide;
                        checkduplicate1.LPMTC_MaterialNeeded = data.LPMTC_MaterialNeeded;
                        checkduplicate1.LPMTC_References = data.LPMTC_References;
                        checkduplicate1.LPMTC_Homework = data.LPMTC_Homework;
                        checkduplicate1.UpdatedDate = DateTime.Now;
                        _context.Update(checkduplicate1);

                        if (data.TeacherGuideUploadDTO.Length > 0)
                        {
                            foreach (var t in data.TeacherGuideUploadDTO)
                            {
                                if (t.LPMTRC_Resources != null && t.LPMTRC_Resources != "")
                                {
                                    LP_Master_Topic_Resources_CollegeDMO dmoteacher = new LP_Master_Topic_Resources_CollegeDMO();

                                    dmoteacher.LPMTC_Id = data.LPMTC_Id;
                                    dmoteacher.LPMTRC_FileName = t.LPMTRC_FileName;
                                    dmoteacher.LPMTRC_Resources = t.LPMTRC_Resources;
                                    dmoteacher.LPMTRC_ResourceType = "Teacher Guide";
                                    dmoteacher.CreatedDate = DateTime.Now;
                                    dmoteacher.UpdatedDate = DateTime.Now;
                                    dmoteacher.LPMTRC_CreatedBy = data.LPMTC_CreatedBy;
                                    dmoteacher.LPMTRC_UpdatedBy = data.LPMTC_CreatedBy;
                                    _context.Add(dmoteacher);
                                }
                            }
                        }
                        if (data.StudnetGuideUploadDTO.Length > 0)
                        {
                            foreach (var t in data.StudnetGuideUploadDTO)
                            {
                                if (t.LPMTRC_Resources != null && t.LPMTRC_Resources != "")
                                {
                                    LP_Master_Topic_Resources_CollegeDMO dmostudent = new LP_Master_Topic_Resources_CollegeDMO();

                                    dmostudent.LPMTC_Id = data.LPMTC_Id;
                                    dmostudent.LPMTRC_FileName = t.LPMTRC_FileName;
                                    dmostudent.LPMTRC_Resources = t.LPMTRC_Resources;
                                    dmostudent.LPMTRC_ResourceType = "Student Guide";
                                    dmostudent.CreatedDate = DateTime.Now;
                                    dmostudent.UpdatedDate = DateTime.Now;
                                    dmostudent.LPMTRC_CreatedBy = data.LPMTC_CreatedBy;
                                    dmostudent.LPMTRC_UpdatedBy = data.LPMTC_CreatedBy;
                                    _context.Add(dmostudent);
                                }
                            }
                        }
                        if (data.MateralGuideUploadDTO.Length > 0)
                        {
                            foreach (var t in data.MateralGuideUploadDTO)
                            {
                                if (t.LPMTRC_Resources != null && t.LPMTRC_Resources != "")
                                {
                                    LP_Master_Topic_Resources_CollegeDMO dmomaterial = new LP_Master_Topic_Resources_CollegeDMO();

                                    dmomaterial.LPMTC_Id = data.LPMTC_Id;
                                    dmomaterial.LPMTRC_FileName = t.LPMTRC_FileName;
                                    dmomaterial.LPMTRC_Resources = t.LPMTRC_Resources;
                                    dmomaterial.LPMTRC_ResourceType = "Materials Needed";
                                    dmomaterial.CreatedDate = DateTime.Now;
                                    dmomaterial.UpdatedDate = DateTime.Now;
                                    dmomaterial.LPMTRC_CreatedBy = data.LPMTC_CreatedBy;
                                    dmomaterial.LPMTRC_UpdatedBy = data.LPMTC_CreatedBy;
                                    _context.Add(dmomaterial);
                                }
                            }
                        }
                        if (data.ReferenceGuideUploadDTO.Length > 0)
                        {
                            foreach (var t in data.ReferenceGuideUploadDTO)
                            {
                                if (t.LPMTRC_Resources != null && t.LPMTRC_Resources != "")
                                {
                                    LP_Master_Topic_Resources_CollegeDMO dmorefrence = new LP_Master_Topic_Resources_CollegeDMO();

                                    dmorefrence.LPMTC_Id = data.LPMTC_Id;
                                    dmorefrence.LPMTRC_FileName = t.LPMTRC_FileName;
                                    dmorefrence.LPMTRC_Resources = t.LPMTRC_Resources;
                                    dmorefrence.LPMTRC_ResourceType = "References";
                                    dmorefrence.CreatedDate = DateTime.Now;
                                    dmorefrence.UpdatedDate = DateTime.Now;
                                    dmorefrence.LPMTRC_CreatedBy = data.LPMTC_CreatedBy;
                                    dmorefrence.LPMTRC_UpdatedBy = data.LPMTC_CreatedBy;
                                    _context.Add(dmorefrence);
                                }

                            }
                        }

                        var i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.message = "Update";
                            data.returnval = true;
                        }
                        else
                        {
                            data.message = "Update";
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                    var checkduplicate = _context.LP_Master_Topic_CollegeDMO.Where(a => a.MI_Id == data.MI_Id && a.LPMMTC_Id == data.LPMMTC_Id
                    && a.LPMTC_TopicName.Equals(data.LPMTC_TopicName)).ToList();

                    if (checkduplicate.Count() > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        var chekrowcount = _context.LP_Master_Topic_CollegeDMO.Where(a => a.MI_Id == data.MI_Id && a.LPMMTC_Id == data.LPMMTC_Id).Count();
                        int k = chekrowcount + 1;

                        LP_Master_Topic_CollegeDMO dmo = new LP_Master_Topic_CollegeDMO();
                        dmo.LPMTC_TopicName = data.LPMTC_TopicName;
                        dmo.MI_Id = data.MI_Id;
                        dmo.LPMMTC_Id = data.LPMMTC_Id;
                        dmo.LPMTC_LessonPlan = data.LPMTC_LessonPlan;
                        dmo.LPMTC_TotalHrs = data.LPMTC_TotalHrs;
                        dmo.LPMTC_TotalPeriods = data.LPMTC_TotalPeriods;
                        dmo.LPMTC_TeacherGuide = data.LPMTC_TeacherGuide;
                        dmo.LPMTC_StudentGuide = data.LPMTC_StudentGuide;
                        dmo.LPMTC_MaterialNeeded = data.LPMTC_MaterialNeeded;
                        dmo.LPMTC_References = data.LPMTC_References;
                        dmo.LPMTC_Homework = data.LPMTC_Homework;
                        dmo.LPMTC_UpdatedBy = data.LPMTC_CreatedBy;
                        dmo.UpdatedDate = DateTime.Now;
                        dmo.LPMTC_CreatedBy = data.LPMTC_CreatedBy;
                        dmo.CreatedDate = DateTime.Now;
                        dmo.LPMTC_TopicOrder = k;
                        dmo.LPMTC_Activefalg = true;
                        _context.Add(dmo);

                        if (data.TeacherGuideUploadDTO.Length > 0)
                        {
                            foreach (var t in data.TeacherGuideUploadDTO)
                            {
                                if (t.LPMTRC_Resources != null && t.LPMTRC_Resources != "")
                                {
                                    LP_Master_Topic_Resources_CollegeDMO dmoteacher = new LP_Master_Topic_Resources_CollegeDMO();

                                    dmoteacher.LPMTC_Id = dmo.LPMTC_Id;
                                    dmoteacher.LPMTRC_FileName = t.LPMTRC_FileName;
                                    dmoteacher.LPMTRC_Resources = t.LPMTRC_Resources;
                                    dmoteacher.LPMTRC_ResourceType = "Teacher Guide";
                                    dmoteacher.CreatedDate = DateTime.Now;
                                    dmoteacher.UpdatedDate = DateTime.Now;
                                    dmoteacher.LPMTRC_CreatedBy = data.LPMTC_CreatedBy;
                                    dmoteacher.LPMTRC_UpdatedBy = data.LPMTC_CreatedBy;
                                    _context.Add(dmoteacher);
                                }
                            }
                        }
                        if (data.StudnetGuideUploadDTO.Length > 0)
                        {
                            foreach (var t in data.StudnetGuideUploadDTO)
                            {
                                if (t.LPMTRC_Resources != null && t.LPMTRC_Resources != "")
                                {
                                    LP_Master_Topic_Resources_CollegeDMO dmostudent = new LP_Master_Topic_Resources_CollegeDMO();

                                    dmostudent.LPMTC_Id = dmo.LPMTC_Id;
                                    dmostudent.LPMTRC_FileName = t.LPMTRC_FileName;
                                    dmostudent.LPMTRC_Resources = t.LPMTRC_Resources;
                                    dmostudent.LPMTRC_ResourceType = "Student Guide";
                                    dmostudent.CreatedDate = DateTime.Now;
                                    dmostudent.UpdatedDate = DateTime.Now;
                                    dmostudent.LPMTRC_CreatedBy = data.LPMTC_CreatedBy;
                                    dmostudent.LPMTRC_UpdatedBy = data.LPMTC_CreatedBy;
                                    _context.Add(dmostudent);
                                }
                            }
                        }
                        if (data.MateralGuideUploadDTO.Length > 0)
                        {
                            foreach (var t in data.MateralGuideUploadDTO)
                            {
                                if (t.LPMTRC_Resources != null && t.LPMTRC_Resources != "")
                                {
                                    LP_Master_Topic_Resources_CollegeDMO dmomaterial = new LP_Master_Topic_Resources_CollegeDMO();

                                    dmomaterial.LPMTC_Id = dmo.LPMTC_Id;
                                    dmomaterial.LPMTRC_FileName = t.LPMTRC_FileName;
                                    dmomaterial.LPMTRC_Resources = t.LPMTRC_Resources;
                                    dmomaterial.LPMTRC_ResourceType = "Materials Needed";
                                    dmomaterial.CreatedDate = DateTime.Now;
                                    dmomaterial.UpdatedDate = DateTime.Now;
                                    dmomaterial.LPMTRC_CreatedBy = data.LPMTC_CreatedBy;
                                    dmomaterial.LPMTRC_UpdatedBy = data.LPMTC_CreatedBy;
                                    _context.Add(dmomaterial);
                                }
                            }
                        }
                        if (data.ReferenceGuideUploadDTO.Length > 0)
                        {
                            foreach (var t in data.ReferenceGuideUploadDTO)
                            {
                                if (t.LPMTRC_Resources != null && t.LPMTRC_Resources != "")
                                {
                                    LP_Master_Topic_Resources_CollegeDMO dmorefrence = new LP_Master_Topic_Resources_CollegeDMO();

                                    dmorefrence.LPMTC_Id = dmo.LPMTC_Id;
                                    dmorefrence.LPMTRC_FileName = t.LPMTRC_FileName;
                                    dmorefrence.LPMTRC_Resources = t.LPMTRC_Resources;
                                    dmorefrence.LPMTRC_ResourceType = "References";
                                    dmorefrence.CreatedDate = DateTime.Now;
                                    dmorefrence.UpdatedDate = DateTime.Now;
                                    dmorefrence.LPMTRC_CreatedBy = data.LPMTC_CreatedBy;
                                    dmorefrence.LPMTRC_UpdatedBy = data.LPMTC_UpdatedBy;
                                    _context.Add(dmorefrence);
                                }
                            }
                        }

                        var i = _context.SaveChanges();
                        if (i > 0)
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
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeSubjTopicMappingDTO editcollegedeatils(CollegeSubjTopicMappingDTO data)
        {
            try
            {
                data.geteditdetials = (from a in _context.LP_Master_Topic_CollegeDMO
                                       from b in _context.LP_Master_MainTopic_CollegeDMO
                                       from c in _context.IVRM_School_Master_SubjectsDMO
                                       where (a.LPMMTC_Id == b.LPMMTC_Id && b.ISMS_Id == c.ISMS_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id
                                       && c.MI_Id == data.MI_Id && a.LPMTC_Id == data.LPMTC_Id && a.LPMMTC_Id == data.LPMMTC_Id && b.ISMS_Id == data.ISMS_Id
                                       && c.ISMS_Id == data.ISMS_Id && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && b.AMSE_Id == data.AMSE_Id
                                       && b.ASMAY_Id == data.ASMAY_Id)
                                       select new CollegeSubjTopicMappingDTO
                                       {
                                           ISMS_SubjectName = c.ISMS_SubjectName,
                                           LPMMTC_TopicName = b.LPMMTC_TopicName,
                                           LPMTC_TopicName = a.LPMTC_TopicName,
                                           LPMTC_TotalHrs = a.LPMTC_TotalHrs,
                                           LPMTC_TotalPeriods = a.LPMTC_TotalPeriods,
                                           ISMS_Id = b.ISMS_Id,
                                           LPMTC_Id = a.LPMTC_Id,
                                           LPMMTC_Id = a.LPMMTC_Id,
                                           LPMTC_TeacherGuide = a.LPMTC_TeacherGuide,
                                           LPMTC_StudentGuide = a.LPMTC_StudentGuide,
                                           LPMTC_MaterialNeeded = a.LPMTC_MaterialNeeded,
                                           LPMTC_References = a.LPMTC_References,
                                           LPMTC_Homework = a.LPMTC_Homework,
                                           LPMTC_LessonPlan = a.LPMTC_LessonPlan,
                                           LPMU_Id = b.LPMU_Id,
                                           ASMAY_Id = b.ASMAY_Id,
                                           AMCO_Id = b.AMCO_Id,
                                           AMB_Id = b.AMB_Id,
                                           AMSE_Id = b.AMSE_Id
                                       }).Distinct().ToArray();

                data.topicdetails = (from a in _context.IVRM_School_Master_SubjectsDMO
                                     from b in _context.LP_Master_MainTopic_CollegeDMO
                                     where (a.ISMS_Id == b.ISMS_Id && a.ISMS_ActiveFlag == 1 && b.LPMMTC_ActiveFlg == true && a.MI_Id == data.MI_Id
                                     && b.MI_Id == data.MI_Id && a.ISMS_Id == data.ISMS_Id && b.ISMS_Id == data.ISMS_Id && b.LPMU_Id == data.LPMU_Id
                                     && b.ASMAY_Id == data.ASMAY_Id & b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && b.AMSE_Id == data.AMSE_Id)
                                     select new CollegeSubjTopicMappingDTO
                                     {
                                         LPMMTC_TopicName = b.LPMMTC_TopicName,
                                         LPMMTC_Id = b.LPMMTC_Id,
                                         LPMMTC_Order = b.LPMMTC_Order
                                     }).Distinct().OrderBy(a => a.LPMMTC_Order).ToArray();


                data.unitdetails = (from a in _context.IVRM_School_Master_SubjectsDMO
                                    from b in _context.LP_Master_MainTopic_CollegeDMO
                                    from c in _context.SchoolMasterUnitDMO
                                    where (c.LPMU_Id == b.LPMU_Id && a.ISMS_Id == b.ISMS_Id && a.ISMS_ActiveFlag == 1 && b.LPMMTC_ActiveFlg == true
                                    && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.ISMS_Id == data.ISMS_Id && b.ISMS_Id == data.ISMS_Id
                                    && b.ASMAY_Id == data.ASMAY_Id & b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && b.AMSE_Id == data.AMSE_Id)
                                    select new CollegeSubjTopicMappingDTO
                                    {
                                        LPMU_UnitName = c.LPMU_UnitName,
                                        LPMU_Id = b.LPMU_Id,
                                        LPMU_Order = c.LPMU_Order
                                    }).Distinct().OrderBy(a => a.LPMU_Order).ToArray();



                var getuserdetails = _context.Staff_User_Login.Where(a => a.MI_Id == data.MI_Id && a.Id == data.LPMTC_CreatedBy).ToList();

                var getinstitutiontype = _context.Institution.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToList();

                List<long> classid = new List<long>();


                if (getuserdetails.Count > 0)
                {
                    data.getcourse = (from a in _context.Adm_College_Atten_Login_UserDMO
                                      from b in _context.MasterCourseDMO
                                      from c in _context.AcademicYear
                                      from d in _context.Adm_College_Atten_Login_DetailsDMO
                                      where (a.ACALU_Id == d.ACALU_Id && d.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && d.ACALD_ActiveFlag == true
                                      && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && a.HRME_Id == getuserdetails.FirstOrDefault().Emp_Code)
                                      select b).Distinct().OrderBy(a => a.AMCO_Order).ToArray();

                    data.getsemester = (from a in _context.Adm_College_Atten_Login_UserDMO
                                        from b in _context.MasterCourseDMO
                                        from c in _context.AcademicYear
                                        from d in _context.Adm_College_Atten_Login_DetailsDMO
                                        from e in _context.ClgMasterBranchDMO
                                        from g in _context.CLG_Adm_Master_SemesterDMO
                                        where (a.ACALU_Id == d.ACALU_Id && d.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && d.AMB_Id == e.AMB_Id
                                        && g.AMSE_Id == d.AMSE_Id && a.MI_Id == data.MI_Id && d.ACALD_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id
                                        && d.AMCO_Id == data.AMCO_Id && d.AMB_Id == data.AMB_Id && a.HRME_Id == getuserdetails.FirstOrDefault().Emp_Code)
                                        select g).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();

                    data.getbranch = (from a in _context.Adm_College_Atten_Login_UserDMO
                                      from b in _context.MasterCourseDMO
                                      from c in _context.AcademicYear
                                      from d in _context.Adm_College_Atten_Login_DetailsDMO
                                      from e in _context.ClgMasterBranchDMO
                                      where (a.ACALU_Id == d.ACALU_Id && d.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && d.AMB_Id == e.AMB_Id
                                      && d.ACALD_ActiveFlag == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && d.AMCO_Id == data.AMCO_Id
                                      && a.HRME_Id == getuserdetails.FirstOrDefault().Emp_Code)
                                      select e).Distinct().OrderBy(a => a.AMB_Order).ToArray();

                    data.getsubjectlist = (from a in _context.Adm_College_Atten_Login_UserDMO
                                           from b in _context.MasterCourseDMO
                                           from c in _context.AcademicYear
                                           from d in _context.Adm_College_Atten_Login_DetailsDMO
                                           from e in _context.ClgMasterBranchDMO
                                           from g in _context.CLG_Adm_Master_SemesterDMO
                                           from i in _context.IVRM_School_Master_SubjectsDMO
                                           where (a.ACALU_Id == d.ACALU_Id && d.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && d.AMB_Id == e.AMB_Id
                                           && g.AMSE_Id == d.AMSE_Id && i.ISMS_Id == d.ISMS_Id && a.MI_Id == data.MI_Id && d.ACALD_ActiveFlag == true
                                           && a.ASMAY_Id == data.ASMAY_Id && d.AMCO_Id == data.AMCO_Id && d.AMB_Id == data.AMB_Id
                                           && a.HRME_Id == getuserdetails.FirstOrDefault().Emp_Code)
                                           select i).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();
                }

                else
                {
                    data.getcourse = (from a in _context.CLG_Adm_College_AY_CourseDMO
                                      from b in _context.MasterCourseDMO
                                      from c in _context.AcademicYear
                                      where (a.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && a.ACAYC_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id
                                      && a.MI_Id == data.MI_Id)
                                      select b).Distinct().OrderBy(a => a.AMCO_Order).ToArray();

                    data.getbranch = (from a in _context.CLG_Adm_College_AY_CourseDMO
                                      from b in _context.MasterCourseDMO
                                      from c in _context.AcademicYear
                                      from d in _context.CLG_Adm_College_AY_Course_BranchDMO
                                      from e in _context.ClgMasterBranchDMO
                                      where (a.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && a.ACAYC_Id == d.ACAYC_Id && d.AMB_Id == e.AMB_Id
                                      && a.ACAYC_ActiveFlag == true && d.ACAYCB_ActiveFlag == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                      && a.AMCO_Id == data.AMCO_Id)
                                      select e).Distinct().OrderBy(a => a.AMB_Order).ToArray();

                    data.getsemester = (from a in _context.CLG_Adm_College_AY_CourseDMO
                                        from b in _context.MasterCourseDMO
                                        from c in _context.AcademicYear
                                        from d in _context.CLG_Adm_College_AY_Course_BranchDMO
                                        from e in _context.ClgMasterBranchDMO
                                        from f in _context.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                        from g in _context.CLG_Adm_Master_SemesterDMO
                                        where (a.AMCO_Id == b.AMCO_Id && a.ASMAY_Id == c.ASMAY_Id && a.ACAYC_Id == d.ACAYC_Id && d.AMB_Id == e.AMB_Id
                                        && d.ACAYCB_Id == f.ACAYCB_Id && f.AMSE_Id == g.AMSE_Id && f.ACAYCBS_ActiveFlag == true && a.MI_Id == data.MI_Id
                                        && a.ACAYC_ActiveFlag == true && d.ACAYCB_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id)
                                        select g).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();

                    data.getsubjectlist = _context.IVRM_School_Master_SubjectsDMO.Where(a => a.MI_Id == data.MI_Id && a.ISMS_ActiveFlag == 1).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeSubjTopicMappingDTO collegedeactivate(CollegeSubjTopicMappingDTO data)
        {
            try
            {
                var result = _context.LP_Master_Topic_CollegeDMO.Single(a => a.MI_Id == data.MI_Id && a.LPMTC_Id == data.LPMTC_Id);
                if (result.LPMTC_Activefalg == true)
                {
                    result.LPMTC_Activefalg = false;
                }
                else
                {
                    result.LPMTC_Activefalg = true;
                }
                result.UpdatedDate = DateTime.Now;
                result.LPMTC_UpdatedBy = data.LPMTC_CreatedBy;
                _context.Update(result);
                int i = _context.SaveChanges();
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
        public CollegeSubjTopicMappingDTO oncollegeselecttopic(CollegeSubjTopicMappingDTO data)
        {
            try
            {
                data.gettopicdetailsorder = _context.LP_Master_Topic_CollegeDMO.Where(a => a.MI_Id == data.MI_Id && a.LPMMTC_Id == data.LPMMTC_Id).OrderBy(a => a.LPMTC_TopicOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeSubjTopicMappingDTO validatecollegeordernumber(CollegeSubjTopicMappingDTO data)
        {
            try
            {
                if (data.CollegeSubjectTopicMappingTemporderDTO.Count() > 0)
                {
                    int i = 0;
                    for (int k = 0; k < data.CollegeSubjectTopicMappingTemporderDTO.Count(); k++)
                    {
                        i = i + 1;
                        var checkresult = _context.LP_Master_Topic_CollegeDMO.Single(a => a.MI_Id == data.MI_Id
                        && a.LPMTC_Id == data.CollegeSubjectTopicMappingTemporderDTO[k].LPMTC_Id
                        && a.LPMMTC_Id == data.CollegeSubjectTopicMappingTemporderDTO[k].LPMMTC_Id);
                        checkresult.LPMTC_TopicOrder = i;
                        checkresult.LPMTC_UpdatedBy = data.LPMTC_CreatedBy;
                        checkresult.UpdatedDate = DateTime.Now;
                        _context.Update(checkresult);
                    }
                    var j = _context.SaveChanges();
                    if (j > 0)
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
        public CollegeSubjTopicMappingDTO viewcollegeuploadflies(CollegeSubjTopicMappingDTO data)
        {
            try
            {
                data.uploadfiles = (from a in _context.LP_Master_Topic_CollegeDMO
                                    from b in _context.LP_Master_MainTopic_CollegeDMO
                                    from c in _context.IVRM_School_Master_SubjectsDMO
                                    from d in _context.LP_Master_Topic_Resources_CollegeDMO
                                    where (b.ISMS_Id == c.ISMS_Id && a.LPMMTC_Id == b.LPMMTC_Id && b.ISMS_Id == c.ISMS_Id
                                    && c.ISMS_ActiveFlag == 1 && b.LPMMTC_ActiveFlg == true && a.LPMTC_Id == d.LPMTC_Id
                                    && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id && d.LPMTC_Id == data.LPMTC_Id
                                    && d.LPMTRC_ResourceType == data.LPMTRC_ResourceType && b.ISMS_Id == data.ISMS_Id && b.LPMU_Id == data.LPMU_Id
                                    && a.LPMTC_Id == data.LPMTC_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id
                                    && b.AMSE_Id == data.AMSE_Id)
                                    select new CollegeSubjTopicMappingDTO
                                    {
                                        ISMS_SubjectName = c.ISMS_SubjectName,
                                        LPMMTC_TopicName = b.LPMMTC_TopicName,
                                        LPMTC_TopicName = a.LPMTC_TopicName,
                                        ISMS_Id = b.ISMS_Id,
                                        LPMTC_Id = a.LPMTC_Id,
                                        LPMMTC_Id = a.LPMMTC_Id,
                                        LPMTRC_Resources = d.LPMTRC_Resources,
                                        LPMTRC_FileName = d.LPMTRC_FileName,
                                        LPMTRC_Id = d.LPMTRC_Id,
                                        LPMU_Id = b.LPMU_Id
                                    }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeSubjTopicMappingDTO deletecollegeuploadfile(CollegeSubjTopicMappingDTO data)
        {
            try
            {
                var checkdata = _context.LP_Master_Topic_Resources_CollegeDMO.Where(a => a.LPMTRC_Id == data.LPMTRC_Id).ToList();
                if (checkdata.Count > 0)
                {
                    var result = _context.LP_Master_Topic_Resources_CollegeDMO.Single(a => a.LPMTRC_Id == data.LPMTRC_Id);

                    _context.Remove(result);

                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }

                    data.uploadfiles = (from a in _context.LP_Master_Topic_CollegeDMO
                                        from b in _context.LP_Master_MainTopic_CollegeDMO
                                        from c in _context.IVRM_School_Master_SubjectsDMO
                                        from d in _context.LP_Master_Topic_Resources_CollegeDMO
                                        where (b.ISMS_Id == c.ISMS_Id && a.LPMMTC_Id == b.LPMMTC_Id && b.ISMS_Id == c.ISMS_Id
                                        && c.ISMS_ActiveFlag == 1 && b.LPMMTC_ActiveFlg == true && a.LPMTC_Id == d.LPMTC_Id
                                        && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id && d.LPMTC_Id == data.LPMTC_Id
                                        && d.LPMTRC_ResourceType == data.LPMTRC_ResourceType && b.ISMS_Id == data.ISMS_Id && b.LPMU_Id == data.LPMU_Id
                                        && a.LPMTC_Id == data.LPMTC_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMCO_Id
                                        && b.AMSE_Id == data.AMSE_Id)
                                        select new CollegeSubjTopicMappingDTO
                                        {
                                            ISMS_SubjectName = c.ISMS_SubjectName,
                                            LPMMTC_TopicName = b.LPMMTC_TopicName,
                                            LPMTC_TopicName = a.LPMTC_TopicName,
                                            ISMS_Id = b.ISMS_Id,
                                            LPMTC_Id = a.LPMTC_Id,
                                            LPMMTC_Id = a.LPMMTC_Id,
                                            LPMTRC_Resources = d.LPMTRC_Resources,
                                            LPMTRC_FileName = d.LPMTRC_FileName,
                                            LPMTRC_Id = d.LPMTRC_Id,
                                            LPMU_Id = b.LPMU_Id
                                        }).Distinct().ToArray();
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
    }
}
