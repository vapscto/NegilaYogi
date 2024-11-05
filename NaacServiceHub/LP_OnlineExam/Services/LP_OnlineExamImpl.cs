using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model.NAAC.LP_OnlineExam;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.NAAC.LP_OnlineExam;

namespace NaacServiceHub.LP_OnlineExam.Services
{
    public class LP_OnlineExamImpl : Interface.LP_OnlineExamInterface
    {
        public LessonplannerContext _context;

        public LP_OnlineExamImpl(LessonplannerContext _cont)
        {
            _context = _cont;
        }

        // LP ONLINE EXAM CONFIG
        public LP_OnlineExamDTO getconfigloaddata(LP_OnlineExamDTO data)
        {
            try
            {
                data.getconfigloaddataarray = _context.LP_Master_OE_SettingDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineExamDTO saveconfigdata(LP_OnlineExamDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.LPMOES_Id > 0)
                {
                    var result = _context.LP_Master_OE_SettingDMO.Single(a => a.MI_Id == data.MI_Id && a.LPMOES_Id == data.LPMOES_Id);
                    result.LPMOES_NoofQns = data.LPMOES_NoofQns;
                    result.LPMOES_TotalMarks = data.LPMOES_TotalMarks;
                    result.LPMOES_TotalDuration = data.LPMOES_TotalDuration;
                    result.LPMOES_EachQnsMarks = data.LPMOES_EachQnsMarks;
                    result.LPMOES_EachQnsDuration = data.LPMOES_EachQnsDuration;
                    result.LPMOES_NoOfOptions = data.LPMOES_NoOfOptions;
                    result.LPMOES_UpdatedBy = data.Userid;
                    result.UpdatedDate = indiantime0;

                    _context.Update(result);

                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                        data.message = "Update";
                    }
                    else
                    {
                        data.returnval = false;
                        data.message = "Update";
                    }
                }
                else
                {
                    LP_Master_OE_SettingDMO lP_Master_OE_SettingDMO = new LP_Master_OE_SettingDMO();

                    lP_Master_OE_SettingDMO.MI_Id = data.MI_Id;
                    lP_Master_OE_SettingDMO.LPMOES_NoofQns = data.LPMOES_NoofQns;
                    lP_Master_OE_SettingDMO.LPMOES_TotalMarks = data.LPMOES_TotalMarks;
                    lP_Master_OE_SettingDMO.LPMOES_TotalDuration = data.LPMOES_TotalDuration;
                    lP_Master_OE_SettingDMO.LPMOES_EachQnsMarks = data.LPMOES_EachQnsMarks;
                    lP_Master_OE_SettingDMO.LPMOES_EachQnsDuration = data.LPMOES_EachQnsDuration;
                    lP_Master_OE_SettingDMO.LPMOES_NoOfOptions = data.LPMOES_NoOfOptions;
                    lP_Master_OE_SettingDMO.LPMOES_ActiveFlg = true;
                    lP_Master_OE_SettingDMO.LPMOES_CreatedBy = data.Userid;
                    lP_Master_OE_SettingDMO.LPMOES_UpdatedBy = data.Userid;
                    lP_Master_OE_SettingDMO.CreatedDate = indiantime0;
                    lP_Master_OE_SettingDMO.UpdatedDate = indiantime0;

                    _context.Add(lP_Master_OE_SettingDMO);

                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                        data.message = "Add";
                    }
                    else
                    {
                        data.returnval = false;
                        data.message = "Add";
                    }
                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //LP SCHOOL ONLINE MASTER QUESTION 
        public LP_OnlineExamDTO getmasterquestionloaddata(LP_OnlineExamDTO data)
        {
            try
            {
                data.getConfigurationSettings = _context.LP_Master_OE_SettingDMO.Where(a => a.MI_Id == data.MI_Id && a.LPMOES_ActiveFlg == true).ToArray();

                data.getyearlist = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.getarratcomplexities = _context.LP_Master_ComplexitiesDMO.Where(a => a.LPMCOMP_ActiveFlg == true).ToArray();

                var getuserdetails = _context.Staff_User_Login.Where(a => a.MI_Id == data.MI_Id && a.Id == data.Userid).ToList();

                var getMasterQuestiondetails = (from a in _context.LP_Master_OE_QuestionsDMO
                                                from c in _context.AdmissionClass
                                                from d in _context.IVRM_School_Master_SubjectsDMO
                                                from e in _context.SchoolSubjectWithMasterTopicMapping
                                                from f in _context.LP_Master_ComplexitiesDMO
                                                where (a.ASMCL_Id == c.ASMCL_Id && a.ISMS_Id == d.ISMS_Id && a.LPMT_Id == e.LPMT_Id
                                                && a.LPMCOMP_Id == f.LPMCOMP_Id && a.MI_Id == data.MI_Id)
                                                select new LP_OnlineExamDTO
                                                {
                                                    LPMOEQ_Id = a.LPMOEQ_Id,
                                                    LPMOEQ_Question = a.LPMOEQ_Question,
                                                    LPMOEQ_QuestionDesc = a.LPMOEQ_QuestionDesc,
                                                    LPMOEQ_ActiveFlg = a.LPMOEQ_ActiveFlg,
                                                    ASMCL_Id = a.ASMCL_Id,
                                                    ISMS_Id = a.ISMS_Id,
                                                    LPMT_Id = a.LPMT_Id,
                                                    ASMCL_ClassName = c.ASMCL_ClassName,
                                                    ISMS_SubjectName = d.ISMS_SubjectName,
                                                    LPMT_TopicName = e.LPMT_TopicName,
                                                    countdoc = _context.LP_Master_OE_Questions_FilesDMO.Where(b => b.LPMOEQ_Id == a.LPMOEQ_Id).Count(),
                                                    countoption = _context.LP_Master_OE_QNS_OptionsDMO.Where(b => b.LPMOEQ_Id == a.LPMOEQ_Id).Count(),
                                                    CreatedDate = a.CreatedDate,
                                                    LPMOEQ_SubjectiveFlg = a.LPMOEQ_SubjectiveFlg,
                                                    LPMOEQ_StructuralFlg = a.LPMOEQ_StructuralFlg,
                                                    LPMOEQ_MatchTheFollowingFlg = a.LPMOEQ_MatchTheFollowingFlg,
                                                    LPMOEQ_Answer = a.LPMOEQ_Answer,
                                                    LPMOEQ_Marks = a.LPMOEQ_Marks,
                                                    LPMCOMP_ComplexityName = f.LPMCOMP_ComplexityName
                                                }).Distinct().OrderByDescending(a => a.CreatedDate).ToList();

                var getclasslist = _context.AdmissionClass.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true).Distinct().OrderBy(a => a.ASMCL_Order).ToList();

                List<LP_OnlineExamDTO> getsubjects = new List<LP_OnlineExamDTO>();
                List<long> subjectid = new List<long>();

                List<LP_OnlineExamDTO> getclass = new List<LP_OnlineExamDTO>();
                List<long> classid = new List<long>();

                if (getuserdetails.Count > 0)
                {
                    var getempcode = getuserdetails.FirstOrDefault().Emp_Code;
                    var loginid = getuserdetails.FirstOrDefault().IVRMSTAUL_Id;

                    getsubjects = (from a in _context.Exm_Login_PrivilegeDMO
                                   from c in _context.Exm_Login_Privilege_SubjectsDMO
                                   from d in _context.IVRM_School_Master_SubjectsDMO
                                   from e in _context.Staff_User_Login
                                   where (a.ELP_Id == c.ELP_Id && e.IVRMSTAUL_Id == a.Login_Id && c.ISMS_Id == d.ISMS_Id && a.ELP_ActiveFlg == true
                                   && d.ISMS_ActiveFlag == 1 && a.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && c.ELPs_ActiveFlg == true
                                   && a.Login_Id == loginid && e.Emp_Code == getempcode && a.ASMAY_Id == data.ASMAY_Id)
                                   select new LP_OnlineExamDTO
                                   {
                                       ISMS_Id = c.ISMS_Id
                                   }).Distinct().ToList();

                    foreach (var c in getsubjects)
                    {
                        subjectid.Add(c.ISMS_Id);
                    }

                    getclass = (from a in _context.Exm_Login_PrivilegeDMO
                                from c in _context.Exm_Login_Privilege_SubjectsDMO
                                from d in _context.AdmissionClass
                                from e in _context.Staff_User_Login
                                where (a.ELP_Id == c.ELP_Id && e.IVRMSTAUL_Id == a.Login_Id && c.ASMCL_Id == d.ASMCL_Id && a.ELP_ActiveFlg == true
                                && a.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && c.ELPs_ActiveFlg == true && a.Login_Id == loginid
                                && e.Emp_Code == getempcode && a.ASMAY_Id == data.ASMAY_Id)
                                select new LP_OnlineExamDTO
                                {
                                    ASMCL_Id = c.ASMCL_Id
                                }).Distinct().ToList();

                    foreach (var c in getclass)
                    {
                        classid.Add(c.ASMCL_Id);
                    }

                    getMasterQuestiondetails = getMasterQuestiondetails.Where(a => subjectid.Contains(a.ISMS_Id) && classid.Contains(a.ASMCL_Id)).ToList();

                    getclasslist = getclasslist.Where(a => classid.Contains(a.ASMCL_Id)).Distinct().OrderBy(a => a.ASMCL_Order).ToList();
                }


                data.getMasterQuestiondetails = getMasterQuestiondetails.ToArray();

                data.getclasslist = getclasslist.ToArray();

                var getmastercomplexitites = _context.LP_Master_ComplexitiesDMO.Where(a => a.LPMCOMP_DefaultFlg == true).ToList();

                data.LPMCOMP_Id = getmastercomplexitites.FirstOrDefault().LPMCOMP_Id;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineExamDTO getclasslist(LP_OnlineExamDTO data)
        {
            try
            {
                var getuserdetails = _context.Staff_User_Login.Where(a => a.MI_Id == data.MI_Id && a.Id == data.Userid).ToList();

                var getinstitutiontype = _context.Institution.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToList();

                List<LP_OnlineExamDTO> getclass = new List<LP_OnlineExamDTO>();
                List<long> classid = new List<long>();

                if (getuserdetails.Count > 0)
                {
                    var getempcode = getuserdetails.FirstOrDefault().Emp_Code;
                    var loginid = getuserdetails.FirstOrDefault().IVRMSTAUL_Id;

                    getclass = (from a in _context.Exm_Login_PrivilegeDMO
                                from c in _context.Exm_Login_Privilege_SubjectsDMO
                                from d in _context.AdmissionClass
                                from e in _context.Staff_User_Login
                                where (a.ELP_Id == c.ELP_Id && e.IVRMSTAUL_Id == a.Login_Id && c.ASMCL_Id == d.ASMCL_Id && a.ELP_ActiveFlg == true
                                && a.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && c.ELPs_ActiveFlg == true && a.Login_Id == loginid
                                && e.Emp_Code == getempcode)
                                select new LP_OnlineExamDTO
                                {
                                    ASMCL_Id = c.ASMCL_Id
                                }).Distinct().ToList();


                    if (getclass.Count > 0)
                    {
                        foreach (var c in getclass)
                        {
                            classid.Add(c.ASMCL_Id);
                        }

                        data.getclasslist = _context.AdmissionClass.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true
                        && classid.Contains(a.ASMCL_Id)).OrderBy(a => a.ASMCL_Order).ToArray();
                    }
                }
                else
                {
                    data.getclasslist = (from a in _context.Masterclasscategory
                                         from b in _context.AcademicYear
                                         from c in _context.AdmissionClass
                                         where (a.ASMCL_Id == c.ASMCL_Id && a.ASMAY_Id == b.ASMAY_Id && a.Is_Active == true)
                                         select c).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineExamDTO getsubjectlist(LP_OnlineExamDTO data)
        {
            try
            {
                var getuserdetails = _context.Staff_User_Login.Where(a => a.MI_Id == data.MI_Id && a.Id == data.Userid).ToList();

                var getinstitutiontype = _context.Institution.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToList();

                List<long> subjectid = new List<long>();
                List<LP_OnlineExamDTO> getsubjects = new List<LP_OnlineExamDTO>();

                if (getuserdetails.Count > 0)
                {
                    var getschoolorcollege = getinstitutiontype.FirstOrDefault().MI_SchoolCollegeFlag;
                    var getempcode = getuserdetails.FirstOrDefault().Emp_Code;
                    var loginid = getuserdetails.FirstOrDefault().IVRMSTAUL_Id;

                    getsubjects = (from a in _context.Exm_Login_PrivilegeDMO
                                   from c in _context.Exm_Login_Privilege_SubjectsDMO
                                   from d in _context.IVRM_School_Master_SubjectsDMO
                                   from e in _context.Staff_User_Login
                                   where (a.ELP_Id == c.ELP_Id && e.IVRMSTAUL_Id == a.Login_Id && c.ISMS_Id == d.ISMS_Id && a.ELP_ActiveFlg == true
                                   && d.ISMS_ActiveFlag == 1 && a.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && c.ELPs_ActiveFlg == true && a.Login_Id == loginid
                                   && e.Emp_Code == getempcode && c.ASMCL_Id == data.ASMCL_Id && a.ASMAY_Id == data.ASMAY_Id)
                                   select new LP_OnlineExamDTO
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
                                           from c in _context.Exm_Yearly_Category_GroupDMO
                                           from d in _context.Exm_Yearly_Category_Group_SubjectsDMO
                                           from e in _context.IVRM_School_Master_SubjectsDMO
                                           where (a.EMCA_Id == b.EMCA_Id && b.EYC_Id == c.EYC_Id && c.EYCG_Id == d.EYCG_Id && d.ISMS_Id == e.ISMS_Id
                                           && a.ECAC_ActiveFlag == true && b.EYC_ActiveFlg == true && c.EYCG_ActiveFlg == true && d.EYCGS_ActiveFlg == true
                                           && a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id)
                                           select e).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineExamDTO gettopiclist(LP_OnlineExamDTO data)
        {
            try
            {
                var getuserdetails = _context.Staff_User_Login.Where(a => a.MI_Id == data.MI_Id && a.Id == data.Userid).ToList();

                var getinstitutiontype = _context.Institution.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToList();

                List<long> subjectid = new List<long>();
                List<LP_OnlineExamDTO> getsubjects = new List<LP_OnlineExamDTO>();

                if (getuserdetails.Count > 0)
                {
                    var getschoolorcollege = getinstitutiontype.FirstOrDefault().MI_SchoolCollegeFlag;
                    var getempcode = getuserdetails.FirstOrDefault().Emp_Code;
                    var loginid = getuserdetails.FirstOrDefault().IVRMSTAUL_Id;

                    data.gettopiclist = (from a in _context.MasterSchoolTopicDMO
                                         from b in _context.SchoolSubjectWithMasterTopicMapping
                                         where (a.LPMMT_Id == b.LPMMT_Id && a.LPMMT_ActiveFlag == true && b.LPMT_ActiveFlag == true
                                         && a.ISMS_Id == data.ISMS_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMAY_Id == data.ASMAY_Id)
                                         select b).Distinct().ToArray();
                }
                else
                {
                    data.gettopiclist = (from a in _context.MasterSchoolTopicDMO
                                         from b in _context.SchoolSubjectWithMasterTopicMapping
                                         where (a.LPMMT_Id == b.LPMMT_Id && a.LPMMT_ActiveFlag == true && b.LPMT_ActiveFlag == true
                                         && a.ISMS_Id == data.ISMS_Id && a.ASMCL_Id == data.ASMCL_Id)
                                         select b).Distinct().ToArray();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineExamDTO SaveMasterQuestionDetails(LP_OnlineExamDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.LPMOEQ_Id > 0)
                {
                    var resultduplicate = _context.LP_Master_OE_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                    && a.LPMT_Id == data.LPMT_Id && a.LPMOEQ_Id != data.LPMOEQ_Id && a.LPMOEQ_Question.Equals(data.LPMOEQ_Question)
                    && a.LPMCOMP_Id == data.LPMCOMP_Id).ToList();

                    List<long> filesid = new List<long>();

                    foreach (var c in data.tempfilesDTO)
                    {
                        filesid.Add(c.LPMOEQF_Id);
                    }

                    Array previous_Noresultremove = _context.LP_Master_OE_Questions_FilesDMO.Where(t => !filesid.Contains(t.LPMOEQF_Id)
                    && t.LPMOEQ_Id == data.LPMOEQ_Id).ToArray();

                    foreach (LP_Master_OE_Questions_FilesDMO ph1 in previous_Noresultremove)
                    {
                        _context.Remove(ph1);
                    }

                    var result = _context.LP_Master_OE_QuestionsDMO.Single(t => t.LPMOEQ_Id == data.LPMOEQ_Id && t.MI_Id == data.MI_Id);
                    result.LPMOEQ_Question = data.LPMOEQ_Question;
                    result.LPMOEQ_QuestionDesc = data.LPMOEQ_QuestionDesc;
                    result.LPMOEQ_SubjectiveFlg = data.LPMOEQ_SubjectiveFlg;
                    result.LPMOEQ_StructuralFlg = data.LPMOEQ_StructuralFlg;
                    result.LPMOEQ_MatchTheFollowingFlg = data.LPMOEQ_MatchTheFollowingFlg;
                    result.LPMOEQ_Answer = data.LPMOEQ_Answer;
                    result.LPMOEQ_NoOfOptions = data.LPMOEQ_NoOfOptions;
                    result.LPMOEQ_MFRowCount = data.LPMOEQ_MFRowCount;
                    result.LPMOEQ_MFColumnCount = data.LPMOEQ_MFColumnCount;
                    result.LPMCOMP_Id = data.LPMCOMP_Id;
                    result.LPMOEQ_Marks = data.LPMOEQ_Marks;
                    result.LPMOEQ_UpdatedBy = data.Userid;
                    result.UpdatedDate = indiantime0;

                    _context.Update(result);

                    if (data.tempfilesDTO != null && data.tempfilesDTO.Length > 0)
                    {
                        foreach (var c in data.tempfilesDTO)
                        {
                            if (c.LPMOEQF_Id > 0)
                            {
                                if (c.LPMOEQF_FilePath != null && c.LPMOEQF_FilePath != "")
                                {
                                    var resultfiles = _context.LP_Master_OE_Questions_FilesDMO.Single(a => a.LPMOEQF_Id == c.LPMOEQF_Id
                                    && a.LPMOEQ_Id == data.LPMOEQ_Id);
                                    resultfiles.LPMOEQF_FilePath = c.LPMOEQF_FilePath;
                                    resultfiles.LPMOEQF_FileName = c.LPMOEQF_FileName;
                                    resultfiles.UpdatedDate = indiantime0;
                                    _context.Update(resultfiles);
                                }
                            }
                            else
                            {
                                if (c.LPMOEQF_FilePath != null && c.LPMOEQF_FilePath != "")
                                {
                                    LP_Master_OE_Questions_FilesDMO lP_Master_OE_Questions_FilesDMO = new LP_Master_OE_Questions_FilesDMO();
                                    lP_Master_OE_Questions_FilesDMO.LPMOEQ_Id = data.LPMOEQ_Id;
                                    lP_Master_OE_Questions_FilesDMO.LPMOEQF_FileName = c.LPMOEQF_FileName;
                                    lP_Master_OE_Questions_FilesDMO.LPMOEQF_FilePath = c.LPMOEQF_FilePath;
                                    lP_Master_OE_Questions_FilesDMO.LPMOEQF_ActiveFlag = true;
                                    lP_Master_OE_Questions_FilesDMO.CreatedDate = indiantime0;
                                    lP_Master_OE_Questions_FilesDMO.UpdatedDate = indiantime0;

                                    _context.Add(lP_Master_OE_Questions_FilesDMO);
                                }
                            }
                        }
                    }

                    if (data.tempoptionsDTO.Length > 0 && data.LPMOEQ_SubjectiveFlg == false)
                    {
                        List<long> optionids = new List<long>();

                        foreach (var d in data.tempoptionsDTO)
                        {
                            optionids.Add(d.LPMOEQOA_Id);
                        }

                        var get_optionlist = _context.LP_Master_OE_QNS_OptionsDMO.Where(a => a.LPMOEQ_Id == data.LPMOEQ_Id
                        && !optionids.Contains(a.LPMOEQOA_Id)).ToList();

                        List<long> remove_option_files = new List<long>();

                        foreach (var e in get_optionlist)
                        {
                            remove_option_files.Add(e.LPMOEQOA_Id);
                        }

                        Array get_option_files_list = _context.LP_Master_OE_QNS_Options_FilesDMO.Where(a => remove_option_files.Contains(a.LPMOEQOA_Id)).ToArray();

                        foreach (LP_Master_OE_QNS_Options_FilesDMO ph2 in get_option_files_list)
                        {
                            _context.Remove(ph2);
                        }

                        if (data.LPMOEQ_MatchTheFollowingFlg == true)
                        {
                            Array get_option_MF_list = _context.LP_Master_OE_QNS_Options_MFDMO.Where(a => remove_option_files.Contains(a.LPMOEQOA_Id)).ToArray();

                            foreach (LP_Master_OE_QNS_Options_MFDMO ph3 in get_option_MF_list)
                            {
                                _context.Remove(ph3);
                            }
                        }

                        Array get_remove_optionlist = get_optionlist.ToArray();

                        foreach (LP_Master_OE_QNS_OptionsDMO ph23 in get_remove_optionlist)
                        {
                            _context.Remove(ph23);
                        }

                        foreach (var c in data.tempoptionsDTO)
                        {
                            long optionid = 0;

                            var checkresultoptions = _context.LP_Master_OE_QNS_OptionsDMO.Where(a => a.LPMOEQOA_Id == c.LPMOEQOA_Id
                            && a.LPMOEQ_Id == data.LPMOEQ_Id).ToList();

                            if (checkresultoptions.Count > 0)
                            {
                                optionid = c.LPMOEQOA_Id;
                                var resultoptions = _context.LP_Master_OE_QNS_OptionsDMO.Single(a => a.LPMOEQOA_Id == c.LPMOEQOA_Id
                                && a.LPMOEQ_Id == data.LPMOEQ_Id);
                                resultoptions.LPMOEQOA_Option = c.LPMOEQOA_Option;
                                resultoptions.LPMOEQOA_OptionCode = c.LPMOEQOA_OptionCode;
                                resultoptions.LPMOEQOA_AnswerFlag = c.LPMOEQOA_AnswerFlag;
                                resultoptions.LPMOEQOA_Marks = c.LPMOEQOA_Marks == null ? null : c.LPMOEQOA_Marks;
                                resultoptions.UpdatedDate = indiantime0;
                                _context.Update(resultoptions);
                            }

                            else
                            {
                                if (c.LPMOEQOA_Option != null && c.LPMOEQOA_Option != "")
                                {
                                    LP_Master_OE_QNS_OptionsDMO lP_Master_OE_QNS_OptionsDMO = new LP_Master_OE_QNS_OptionsDMO
                                    {
                                        LPMOEQ_Id = data.LPMOEQ_Id,
                                        LPMOEQOA_Option = c.LPMOEQOA_Option,
                                        LPMOEQOA_OptionCode = c.LPMOEQOA_OptionCode,
                                        LPMOEQOA_AnswerFlag = c.LPMOEQOA_AnswerFlag,
                                        LPMOEQOA_Marks = c.LPMOEQOA_Marks == null ? null : c.LPMOEQOA_Marks,
                                        LPMOEQOA_ActiveFlg = true,
                                        CreatedDate = indiantime0,
                                        UpdatedDate = indiantime0
                                    };
                                    lP_Master_OE_QNS_OptionsDMO.UpdatedDate = indiantime0;
                                    _context.Add(lP_Master_OE_QNS_OptionsDMO);
                                    optionid = lP_Master_OE_QNS_OptionsDMO.LPMOEQOA_Id;
                                }
                            }

                            if (c.tempoptionsfiles != null && c.tempoptionsfiles.Length > 0)
                            {
                                List<long> ids = new List<long>();
                                foreach (var d in c.tempoptionsfiles)
                                {
                                    ids.Add(d.LPMOEQOAF_Id);
                                }

                                Array previous_options_files = _context.LP_Master_OE_QNS_Options_FilesDMO.Where(t => t.LPMOEQOA_Id == c.LPMOEQOA_Id
                                && !ids.Contains(t.LPMOEQOAF_Id)).ToArray();

                                foreach (LP_Master_OE_QNS_Options_FilesDMO ph2 in previous_options_files)
                                {
                                    _context.Remove(ph2);
                                }

                                foreach (var e in c.tempoptionsfiles)
                                {
                                    if (e.LPMOEQOAF_Id > 0)
                                    {
                                        if (e.LPMOEQOAF_FilePath != null && e.LPMOEQOAF_FilePath != "")
                                        {
                                            var resultoptsfiles = _context.LP_Master_OE_QNS_Options_FilesDMO.Single(a => a.LPMOEQOAF_Id == e.LPMOEQOAF_Id);
                                            resultoptsfiles.LPMOEQOAF_FileName = e.LPMOEQOAF_FileName;
                                            resultoptsfiles.LPMOEQOAF_FilePath = e.LPMOEQOAF_FilePath;
                                            resultoptsfiles.UpdatedDate = indiantime0;
                                            _context.Update(resultoptsfiles);
                                        }
                                    }
                                    else
                                    {
                                        if (e.LPMOEQOAF_FilePath != null && e.LPMOEQOAF_FilePath != "")
                                        {
                                            LP_Master_OE_QNS_Options_FilesDMO lP_Master_OE_QNS_Options_FilesDMO = new LP_Master_OE_QNS_Options_FilesDMO();
                                            //lP_Master_OE_QNS_Options_FilesDMO.LPMOEQOA_Id = c.LPMOEQOA_Id;
                                            lP_Master_OE_QNS_Options_FilesDMO.LPMOEQOA_Id = optionid;
                                            lP_Master_OE_QNS_Options_FilesDMO.LPMOEQOAF_FileName = e.LPMOEQOAF_FileName;
                                            lP_Master_OE_QNS_Options_FilesDMO.LPMOEQOAF_FilePath = e.LPMOEQOAF_FilePath;
                                            lP_Master_OE_QNS_Options_FilesDMO.LPMOEQOAF_ActiveFlag = true;
                                            lP_Master_OE_QNS_Options_FilesDMO.CreatedDate = indiantime0;
                                            lP_Master_OE_QNS_Options_FilesDMO.UpdatedDate = indiantime0;
                                            _context.Add(lP_Master_OE_QNS_Options_FilesDMO);
                                        }
                                    }
                                }
                            }

                            if (c.Temp_MF_OptionsDTO != null && c.Temp_MF_OptionsDTO.Length > 0 && data.LPMOEQ_MatchTheFollowingFlg == true)
                            {
                                List<long> id_mfs = new List<long>();

                                foreach (var mf in c.Temp_MF_OptionsDTO)
                                {
                                    id_mfs.Add(mf.LPMOEQOAMF_Id);
                                }

                                Array previous_options_Mf = _context.LP_Master_OE_QNS_Options_MFDMO.Where(t => t.LPMOEQOA_Id == c.LPMOEQOA_Id
                                && !id_mfs.Contains(t.LPMOEQOAMF_Id)).ToArray();

                                foreach (LP_Master_OE_QNS_Options_MFDMO ph_mf in previous_options_Mf)
                                {
                                    _context.Remove(ph_mf);
                                }

                                foreach (var mf_option in c.Temp_MF_OptionsDTO)
                                {
                                    if (mf_option.LPMOEQOAMF_Id > 0)
                                    {
                                        var result_mf = _context.LP_Master_OE_QNS_Options_MFDMO.Single(a => a.LPMOEQOAMF_Id == mf_option.LPMOEQOAMF_Id);
                                        result_mf.LPMOEQOAMF_MatchtheFollowing = mf_option.LPMOEQOAMF_MatchtheFollowing;
                                        result_mf.LPMOEQOAMF_Answer_LPMOEQOA_Id = null;
                                        result_mf.LPMOEQOAMF_Order = mf_option.LPMOEQOAMF_Order;
                                        result_mf.LPMOEQOAMF_UpdatedBy = data.Userid;
                                        result_mf.LPMOEQOAMF_UpdatedDate = indiantime0;
                                        result_mf.LPMOEQOAMF_AnswerFlag = mf_option.LPMOEQOAMF_AnswerFlag;
                                        _context.Update(result_mf);
                                    }
                                    else
                                    {
                                        LP_Master_OE_QNS_Options_MFDMO lP_Master_OE_QNS_Options_MFDMO = new LP_Master_OE_QNS_Options_MFDMO
                                        {
                                            LPMOEQOA_Id = optionid,
                                            LPMOEQOAMF_MatchtheFollowing = mf_option.LPMOEQOAMF_MatchtheFollowing,
                                            LPMOEQOAMF_Answer_LPMOEQOA_Id = null,
                                            LPMOEQOAMF_ActiveFlg = true,
                                            LPMOEQOAMF_CreatedBy = data.Userid,
                                            LPMOEQOAMF_CreatedDate = indiantime0,
                                            LPMOEQOAMF_UpdatedBy = data.Userid,
                                            LPMOEQOAMF_UpdatedDate = indiantime0,
                                            LPMOEQOAMF_AnswerFlag = mf_option.LPMOEQOAMF_AnswerFlag,
                                            LPMOEQOAMF_Order = mf_option.LPMOEQOAMF_Order
                                        };
                                        _context.Add(lP_Master_OE_QNS_Options_MFDMO);
                                    }
                                }
                            }
                        }
                    }
                    //else
                    //{
                    //    Array previous_options = _context.LP_Master_OE_QNS_OptionsDMO.Where(t => t.LPMOEQ_Id == data.LPMOEQ_Id).ToArray();

                    //    foreach (LP_Master_OE_QNS_OptionsDMO ph2 in previous_options)
                    //    {
                    //        _context.Remove(ph2);
                    //    }
                    //}

                    var contactExists = _context.SaveChanges();
                    if (contactExists > 0)
                    {
                        data.returnval = true;
                        data.message = "Update";
                    }
                    else
                    {
                        data.returnval = false;
                        data.message = "Update";
                    }
                }
                else
                {
                    LP_Master_OE_QuestionsDMO lP_Master_OE_QuestionsDMO = new LP_Master_OE_QuestionsDMO();

                    var resultduplicate = _context.LP_Master_OE_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                    && a.LPMT_Id == data.LPMT_Id && a.LPMOEQ_Question.Equals(data.LPMOEQ_Question) && a.LPMCOMP_Id == data.LPMCOMP_Id).ToList();

                    lP_Master_OE_QuestionsDMO.MI_Id = data.MI_Id;
                    lP_Master_OE_QuestionsDMO.ASMCL_Id = data.ASMCL_Id;
                    lP_Master_OE_QuestionsDMO.ISMS_Id = data.ISMS_Id;
                    lP_Master_OE_QuestionsDMO.LPMT_Id = data.LPMT_Id;
                    lP_Master_OE_QuestionsDMO.LPMCOMP_Id = data.LPMCOMP_Id;
                    lP_Master_OE_QuestionsDMO.LPMOEQ_Question = data.LPMOEQ_Question;
                    lP_Master_OE_QuestionsDMO.LPMOEQ_QuestionDesc = data.LPMOEQ_QuestionDesc;
                    lP_Master_OE_QuestionsDMO.LPMOEQ_SubjectiveFlg = data.LPMOEQ_SubjectiveFlg;
                    lP_Master_OE_QuestionsDMO.LPMOEQ_MatchTheFollowingFlg = data.LPMOEQ_MatchTheFollowingFlg;
                    lP_Master_OE_QuestionsDMO.LPMOEQ_StructuralFlg = data.LPMOEQ_StructuralFlg;
                    lP_Master_OE_QuestionsDMO.LPMOEQ_Answer = data.LPMOEQ_Answer;
                    lP_Master_OE_QuestionsDMO.LPMOEQ_NoOfOptions = data.LPMOEQ_NoOfOptions;
                    lP_Master_OE_QuestionsDMO.LPMOEQ_MFRowCount = data.LPMOEQ_MFRowCount;
                    lP_Master_OE_QuestionsDMO.LPMOEQ_MFColumnCount = data.LPMOEQ_MFColumnCount;
                    lP_Master_OE_QuestionsDMO.LPMOEQ_Marks = data.LPMOEQ_Marks;
                    lP_Master_OE_QuestionsDMO.LPMOEQ_ActiveFlg = true;
                    lP_Master_OE_QuestionsDMO.LPMOEQ_CreatedBy = data.Userid;
                    lP_Master_OE_QuestionsDMO.LPMOEQ_UpdatedBy = data.Userid;
                    lP_Master_OE_QuestionsDMO.CreatedDate = indiantime0;
                    lP_Master_OE_QuestionsDMO.UpdatedDate = indiantime0;

                    _context.Add(lP_Master_OE_QuestionsDMO);

                    if (data.tempfilesDTO != null && data.tempfilesDTO.Length > 0)
                    {
                        foreach (var c in data.tempfilesDTO)
                        {
                            if (c.LPMOEQF_FilePath != null && c.LPMOEQF_FilePath != "")
                            {
                                LP_Master_OE_Questions_FilesDMO lP_Master_OE_Questions_FilesDMO = new LP_Master_OE_Questions_FilesDMO
                                {
                                    LPMOEQ_Id = lP_Master_OE_QuestionsDMO.LPMOEQ_Id,
                                    LPMOEQF_FileName = c.LPMOEQF_FileName,
                                    LPMOEQF_FilePath = c.LPMOEQF_FilePath,
                                    LPMOEQF_ActiveFlag = true,
                                    CreatedDate = indiantime0,
                                    UpdatedDate = indiantime0
                                };

                                _context.Add(lP_Master_OE_Questions_FilesDMO);
                            }
                        }
                    }

                    if (data.tempoptionsDTO != null && data.tempoptionsDTO.Length > 0 && data.LPMOEQ_SubjectiveFlg == false)
                    {
                        foreach (var c in data.tempoptionsDTO)
                        {
                            if (c.LPMOEQOA_Option != null && c.LPMOEQOA_Option != "")
                            {
                                LP_Master_OE_QNS_OptionsDMO lP_Master_OE_QNS_OptionsDMO = new LP_Master_OE_QNS_OptionsDMO
                                {
                                    LPMOEQ_Id = lP_Master_OE_QuestionsDMO.LPMOEQ_Id,
                                    LPMOEQOA_Option = c.LPMOEQOA_Option,
                                    LPMOEQOA_OptionCode = c.LPMOEQOA_OptionCode,
                                    LPMOEQOA_AnswerFlag = c.LPMOEQOA_AnswerFlag,
                                    LPMOEQOA_Marks = c.LPMOEQOA_Marks == null ? null : c.LPMOEQOA_Marks,
                                    LPMOEQOA_ActiveFlg = true,
                                    CreatedDate = indiantime0,
                                    UpdatedDate = indiantime0
                                };

                                _context.Add(lP_Master_OE_QNS_OptionsDMO);

                                if (c.tempoptionsfiles != null && c.tempoptionsfiles.Length > 0)
                                {
                                    foreach (var d in c.tempoptionsfiles)
                                    {
                                        if (d.LPMOEQOAF_FilePath != null && d.LPMOEQOAF_FilePath != "")
                                        {
                                            LP_Master_OE_QNS_Options_FilesDMO lP_Master_OE_QNS_Options_FilesDMO = new LP_Master_OE_QNS_Options_FilesDMO();
                                            lP_Master_OE_QNS_Options_FilesDMO.LPMOEQOA_Id = lP_Master_OE_QNS_OptionsDMO.LPMOEQOA_Id;
                                            lP_Master_OE_QNS_Options_FilesDMO.LPMOEQOAF_FileName = d.LPMOEQOAF_FileName;
                                            lP_Master_OE_QNS_Options_FilesDMO.LPMOEQOAF_FilePath = d.LPMOEQOAF_FilePath;
                                            lP_Master_OE_QNS_Options_FilesDMO.LPMOEQOAF_ActiveFlag = true;
                                            lP_Master_OE_QNS_Options_FilesDMO.CreatedDate = indiantime0;
                                            lP_Master_OE_QNS_Options_FilesDMO.UpdatedDate = indiantime0;
                                            _context.Add(lP_Master_OE_QNS_Options_FilesDMO);
                                        }
                                    }
                                }

                                if (c.Temp_MF_OptionsDTO != null && c.Temp_MF_OptionsDTO.Length > 0 && data.LPMOEQ_SubjectiveFlg == false)
                                {
                                    foreach (var mf in c.Temp_MF_OptionsDTO)
                                    {
                                        LP_Master_OE_QNS_Options_MFDMO lP_Master_OE_QNS_Options_MFDMO = new LP_Master_OE_QNS_Options_MFDMO
                                        {

                                            LPMOEQOA_Id = lP_Master_OE_QNS_OptionsDMO.LPMOEQOA_Id,
                                            LPMOEQOAMF_MatchtheFollowing = mf.LPMOEQOAMF_MatchtheFollowing,
                                            LPMOEQOAMF_Answer_LPMOEQOA_Id = null,
                                            LPMOEQOAMF_ActiveFlg = true,
                                            LPMOEQOAMF_CreatedBy = data.Userid,
                                            LPMOEQOAMF_CreatedDate = indiantime0,
                                            LPMOEQOAMF_UpdatedBy = data.Userid,
                                            LPMOEQOAMF_UpdatedDate = indiantime0,
                                            LPMOEQOAMF_Order = mf.LPMOEQOAMF_Order,
                                            LPMOEQOAMF_AnswerFlag = mf.LPMOEQOAMF_AnswerFlag
                                        };
                                        _context.Add(lP_Master_OE_QNS_Options_MFDMO);
                                    }
                                }
                            }
                        }
                    }
                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                        data.message = "Add";
                    }
                    else
                    {
                        data.returnval = false;
                        data.message = "Add";
                    }
                }
            }
            catch (Exception ex)
            {
                data.returnval = false;
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineExamDTO EditMasterQuestion(LP_OnlineExamDTO data)
        {
            try
            {
                var getquestiondetails = _context.LP_Master_OE_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id && a.LPMOEQ_Id == data.LPMOEQ_Id).ToList();

                data.geteditmasterquestion = getquestiondetails.ToArray();
                data.ASMCL_Id = getquestiondetails.FirstOrDefault().ASMCL_Id;
                data.ISMS_Id = getquestiondetails.FirstOrDefault().ISMS_Id;

                data.geteditdocuments = _context.LP_Master_OE_Questions_FilesDMO.Where(a => a.LPMOEQ_Id == data.LPMOEQ_Id).ToArray();

                var getuserdetails = _context.Staff_User_Login.Where(a => a.MI_Id == data.MI_Id && a.Id == data.Userid).ToList();

                var getinstitutiontype = _context.Institution.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToList();

                List<LP_OnlineExamDTO> getclass = new List<LP_OnlineExamDTO>();
                List<long> classid = new List<long>();

                List<long> subjectid = new List<long>();
                List<LP_OnlineExamDTO> getsubjects = new List<LP_OnlineExamDTO>();

                // Staff Login With Default Year
                if (getuserdetails.Count > 0)
                {
                    var getempcode = getuserdetails.FirstOrDefault().Emp_Code;
                    var loginid = getuserdetails.FirstOrDefault().IVRMSTAUL_Id;

                    // GETTING CLASS LIST FOR THAT STAFF

                    getclass = (from a in _context.Exm_Login_PrivilegeDMO
                                from c in _context.Exm_Login_Privilege_SubjectsDMO
                                from d in _context.AdmissionClass
                                from e in _context.Staff_User_Login
                                where (a.ELP_Id == c.ELP_Id && e.IVRMSTAUL_Id == a.Login_Id && c.ASMCL_Id == d.ASMCL_Id && a.ELP_ActiveFlg == true
                                && a.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && c.ELPs_ActiveFlg == true && a.Login_Id == loginid
                                && e.Emp_Code == getempcode && a.ASMAY_Id == data.ASMAY_Id)
                                select new LP_OnlineExamDTO
                                {
                                    ASMCL_Id = c.ASMCL_Id
                                }).Distinct().ToList();


                    if (getclass.Count > 0)
                    {
                        foreach (var c in getclass)
                        {
                            classid.Add(c.ASMCL_Id);
                        }

                        data.geteditclasslist = _context.AdmissionClass.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true
                        && classid.Contains(a.ASMCL_Id)).OrderBy(a => a.ASMCL_Order).ToArray();
                    }

                    // GETTING SUBJECT LIST FOR THAT CLASS 
                    getsubjects = (from a in _context.Exm_Login_PrivilegeDMO
                                   from c in _context.Exm_Login_Privilege_SubjectsDMO
                                   from d in _context.IVRM_School_Master_SubjectsDMO
                                   from e in _context.Staff_User_Login
                                   where (a.ELP_Id == c.ELP_Id && e.IVRMSTAUL_Id == a.Login_Id && c.ISMS_Id == d.ISMS_Id && a.ELP_ActiveFlg == true
                                   && d.ISMS_ActiveFlag == 1 && a.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && c.ELPs_ActiveFlg == true && a.Login_Id == loginid
                                   && e.Emp_Code == getempcode && c.ASMCL_Id == data.ASMCL_Id && a.ASMAY_Id == data.ASMAY_Id)
                                   select new LP_OnlineExamDTO
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

                    data.geteditsubjectlist = _context.IVRM_School_Master_SubjectsDMO.Where(a => a.MI_Id == data.MI_Id && a.ISMS_ActiveFlag == 1
                    && subjectid.Contains(a.ISMS_Id)).OrderBy(a => a.ISMS_OrderFlag).ToArray();

                    data.getedittopiclist = (from a in _context.MasterSchoolTopicDMO
                                             from b in _context.SchoolSubjectWithMasterTopicMapping
                                             where (a.LPMMT_Id == b.LPMMT_Id && a.LPMMT_ActiveFlag == true && b.LPMT_ActiveFlag == true
                                             && a.ISMS_Id == data.ISMS_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMAY_Id == data.ASMAY_Id)
                                             select b).Distinct().ToArray();
                }
                // Admin Login With All Years
                else
                {
                    data.geteditclasslist = (from a in _context.Masterclasscategory
                                             from b in _context.AcademicYear
                                             from c in _context.AdmissionClass
                                             where (a.ASMCL_Id == c.ASMCL_Id && a.ASMAY_Id == b.ASMAY_Id && a.Is_Active == true)
                                             select c).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();

                    data.geteditsubjectlist = (from a in _context.Exm_Category_ClassDMO
                                               from b in _context.Exm_Yearly_CategoryDMO
                                               from c in _context.Exm_Yearly_Category_ExamsDMO
                                               from d in _context.Exm_Yrly_Cat_Exams_SubwiseDMO
                                               from e in _context.IVRM_School_Master_SubjectsDMO
                                               where (a.EMCA_Id == b.EMCA_Id && b.EYC_Id == c.EYC_Id && c.EYCE_Id == d.EYCE_Id && d.ISMS_Id == e.ISMS_Id
                                               && a.ECAC_ActiveFlag == true && b.EYC_ActiveFlg == true && c.EYCE_ActiveFlg == true && d.EYCES_ActiveFlg == true
                                               && a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id)
                                               select e).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();

                    data.getedittopiclist = (from a in _context.MasterSchoolTopicDMO
                                             from b in _context.SchoolSubjectWithMasterTopicMapping
                                             where (a.LPMMT_Id == b.LPMMT_Id && a.LPMMT_ActiveFlag == true && b.LPMT_ActiveFlag == true
                                             && a.ISMS_Id == data.ISMS_Id && a.ASMCL_Id == data.ASMCL_Id)
                                             select b).Distinct().ToArray();

                }

                data.getSavedOptions = _context.LP_Master_OE_QNS_OptionsDMO.Where(a => a.LPMOEQ_Id == data.LPMOEQ_Id).ToArray();

                data.getViewSavedOptionsFiles = (from a in _context.LP_Master_OE_QNS_Options_FilesDMO
                                                 from b in _context.LP_Master_OE_QNS_OptionsDMO
                                                 where (a.LPMOEQOA_Id == b.LPMOEQOA_Id && b.LPMOEQ_Id == data.LPMOEQ_Id)
                                                 select a).Distinct().ToArray();

                var get_optiontype_questionid = _context.LP_Students_Exam_AnswerDMO.Where(a => a.LPMOEQ_Id == data.LPMOEQ_Id && a.LPSTUEXANS_ActiveFlg == true).ToList();

                var get_subjective_questionid = _context.LP_Students_Exam_SubjectiveAnswerDMO.Where(a => a.LPMOEQ_Id == data.LPMOEQ_Id && a.LPSTUEXANS_ActiveFlg == true).ToList();

                if (getquestiondetails.FirstOrDefault().LPMOEQ_MatchTheFollowingFlg == true)
                {
                    data.getviewsavedmfoptions = (from a in _context.LP_Master_OE_QuestionsDMO
                                                  from b in _context.LP_Master_OE_QNS_OptionsDMO
                                                  from c in _context.LP_Master_OE_QNS_Options_MFDMO
                                                  where (a.LPMOEQ_Id == b.LPMOEQ_Id && b.LPMOEQOA_Id == c.LPMOEQOA_Id && a.LPMOEQ_Id == data.LPMOEQ_Id
                                                  && a.LPMOEQ_MatchTheFollowingFlg == true)
                                                  select new LP_OnlineExamDTO
                                                  {
                                                      LPMOEQ_Id = a.LPMOEQ_Id,
                                                      LPMOEQOA_Id = b.LPMOEQOA_Id,
                                                      LPMOEQOAMF_Id = c.LPMOEQOAMF_Id,
                                                      LPMOEQOAMF_MatchtheFollowing = c.LPMOEQOAMF_MatchtheFollowing,
                                                      LPMOEQOAMF_AnswerFlag = c.LPMOEQOAMF_AnswerFlag,
                                                      LPMOEQOAMF_Order = c.LPMOEQOAMF_Order,
                                                  }).Distinct().OrderBy(a => a.LPMOEQOAMF_Order).ToArray();
                }

                data.examconducted_count = get_optiontype_questionid.Count() + get_subjective_questionid.Count();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineExamDTO ViewMasterQuesDoc(LP_OnlineExamDTO data)
        {
            try
            {
                data.getviedocarray = _context.LP_Master_OE_Questions_FilesDMO.Where(a => a.LPMOEQ_Id == data.LPMOEQ_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineExamDTO DeactivateActivateQuestion(LP_OnlineExamDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var checkresult = _context.LP_Master_OE_QuestionsDMO.Where(a => a.LPMOEQ_Id == data.LPMOEQ_Id).ToList();

                var get_optiontype_questionid = _context.LP_Students_Exam_AnswerDMO.Where(a => a.LPMOEQ_Id == data.LPMOEQ_Id && a.LPSTUEXANS_ActiveFlg == true).ToList();

                var get_subjective_questionid = _context.LP_Students_Exam_SubjectiveAnswerDMO.Where(a => a.LPMOEQ_Id == data.LPMOEQ_Id && a.LPSTUEXANS_ActiveFlg == true).ToList();

                var count = 0;

                var get_exam_questionid = (from a in _context.LP_Master_OE_ExamDMO
                                           from b in _context.LP_Master_OE_Exam_LevelsDMO
                                           from c in _context.LP_Master_OE_Exam_QuestionsDMO
                                           where (a.LPMOEEX_Id == b.LPMOEEX_Id && b.LPMOEEXLVL_Id == c.LPMOEEXLVL_Id && c.LPMOEQ_Id == data.LPMOEQ_Id
                                           && a.MI_Id == data.MI_Id && a.LPMOEEX_ActiveFlg == true && b.LPMOEEXLVL_ActiveFlg == true && c.LPMOEEXQNS_ActiveFlg == true)
                                           select c).Distinct().ToList();

                var examconducted_count = get_optiontype_questionid.Count() + get_subjective_questionid.Count() + get_exam_questionid.Count();

                if (checkresult.FirstOrDefault().LPMOEQ_ActiveFlg == true)
                {
                    if (examconducted_count > 0)
                    {
                        count = 1;
                        data.message = "Mapped";
                    }
                }

                if (checkresult.Count > 0 && count == 0)
                {
                    var result = _context.LP_Master_OE_QuestionsDMO.Single(a => a.LPMOEQ_Id == data.LPMOEQ_Id);

                    if (result.LPMOEQ_ActiveFlg == true)
                    {
                        result.LPMOEQ_ActiveFlg = false;
                    }
                    else
                    {
                        result.LPMOEQ_ActiveFlg = true;
                    }
                    result.UpdatedDate = indiantime0;
                    _context.Update(result);

                    var s = _context.SaveChanges();

                    if (s > 0)
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
            }
            return data;
        }
        public LP_OnlineExamDTO DeactivateActivateDocument(LP_OnlineExamDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var checkresult = _context.LP_Master_OE_Questions_FilesDMO.Where(a => a.LPMOEQF_Id == data.LPMOEQF_Id).ToList();

                if (checkresult.Count > 0)
                {
                    var result = _context.LP_Master_OE_Questions_FilesDMO.Single(a => a.LPMOEQF_Id == data.LPMOEQF_Id);

                    if (result.LPMOEQF_ActiveFlag == true)
                    {
                        result.LPMOEQF_ActiveFlag = false;
                    }
                    else
                    {
                        result.LPMOEQF_ActiveFlag = true;
                    }
                    result.UpdatedDate = indiantime0;
                    _context.Update(result);

                    var s = _context.SaveChanges();

                    if (s > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }

                data.getviedocarray = _context.LP_Master_OE_Questions_FilesDMO.Where(a => a.LPMOEQ_Id == data.LPMOEQ_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineExamDTO ViewMasterQuesOptions(LP_OnlineExamDTO data)
        {
            try
            {
                data.getViewSavedOptions = (from a in _context.LP_Master_OE_QNS_OptionsDMO
                                            where (a.LPMOEQ_Id == data.LPMOEQ_Id)
                                            select new LP_OnlineExamDTO
                                            {
                                                LPMOEQOA_Id = a.LPMOEQOA_Id,
                                                LPMOEQ_Id = a.LPMOEQ_Id,
                                                LPMOEQOA_Option = a.LPMOEQOA_Option,
                                                LPMOEQOA_OptionCode = a.LPMOEQOA_OptionCode,
                                                LPMOEQOA_AnswerFlag = a.LPMOEQOA_AnswerFlag,
                                                LPMOEQOA_ActiveFlg = a.LPMOEQOA_ActiveFlg,
                                                countoptionfiles = _context.LP_Master_OE_QNS_Options_FilesDMO.Where(b => b.LPMOEQOA_Id == a.LPMOEQOA_Id).Count(),
                                            }).Distinct().ToArray();

                if (data.LPMOEQ_MatchTheFollowingFlg == true)
                {
                    data.getviewsavedmfoptions = (from a in _context.LP_Master_OE_QuestionsDMO
                                                  from b in _context.LP_Master_OE_QNS_OptionsDMO
                                                  from c in _context.LP_Master_OE_QNS_Options_MFDMO
                                                  where (a.LPMOEQ_Id == b.LPMOEQ_Id && b.LPMOEQOA_Id == c.LPMOEQOA_Id && a.LPMOEQ_Id == data.LPMOEQ_Id
                                                  && a.LPMOEQ_MatchTheFollowingFlg == true)
                                                  select new LP_OnlineExamDTO
                                                  {
                                                      LPMOEQ_Id = a.LPMOEQ_Id,
                                                      LPMOEQOA_Id = b.LPMOEQOA_Id,
                                                      LPMOEQOAMF_Id = c.LPMOEQOAMF_Id,
                                                      LPMOEQOAMF_MatchtheFollowing = c.LPMOEQOAMF_MatchtheFollowing,
                                                      LPMOEQOAMF_AnswerFlag = c.LPMOEQOAMF_AnswerFlag,
                                                      LPMOEQOAMF_Order = c.LPMOEQOAMF_Order,
                                                  }).Distinct().OrderBy(a => a.LPMOEQOAMF_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineExamDTO ViewUploadOptionFiles(LP_OnlineExamDTO data)
        {
            try
            {
                data.getViewSavedOptionsFiles = _context.LP_Master_OE_QNS_Options_FilesDMO.Where(a => a.LPMOEQOA_Id == data.LPMOEQOA_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineExamDTO DeactivateActivateQuesOption(LP_OnlineExamDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var checkresult = _context.LP_Master_OE_QNS_OptionsDMO.Where(a => a.LPMOEQOA_Id == data.LPMOEQOA_Id).ToList();

                if (checkresult.Count > 0)
                {
                    var result = _context.LP_Master_OE_QNS_OptionsDMO.Single(a => a.LPMOEQOA_Id == data.LPMOEQOA_Id);

                    if (result.LPMOEQOA_ActiveFlg == true)
                    {
                        result.LPMOEQOA_ActiveFlg = false;
                    }
                    else
                    {
                        result.LPMOEQOA_ActiveFlg = true;
                    }
                    result.UpdatedDate = indiantime0;
                    _context.Update(result);

                    var s = _context.SaveChanges();

                    if (s > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }

                data.getViewSavedOptions = _context.LP_Master_OE_QNS_OptionsDMO.Where(a => a.LPMOEQ_Id == data.LPMOEQ_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineExamDTO DeactivateActivateOptionsDocument(LP_OnlineExamDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var checkresult = _context.LP_Master_OE_QNS_Options_FilesDMO.Where(a => a.LPMOEQOA_Id == data.LPMOEQOA_Id
                && a.LPMOEQOAF_Id == data.LPMOEQOAF_Id).ToList();

                if (checkresult.Count > 0)
                {
                    var result = _context.LP_Master_OE_QNS_Options_FilesDMO.Single(a => a.LPMOEQOAF_Id == data.LPMOEQOAF_Id);

                    if (result.LPMOEQOAF_ActiveFlag == true)
                    {
                        result.LPMOEQOAF_ActiveFlag = false;
                    }
                    else
                    {
                        result.LPMOEQOAF_ActiveFlag = true;
                    }
                    result.UpdatedDate = indiantime0;
                    _context.Update(result);

                    var s = _context.SaveChanges();

                    if (s > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }

                data.getViewSavedOptionsFiles = _context.LP_Master_OE_QNS_Options_FilesDMO.Where(a => a.LPMOEQOA_Id == data.LPMOEQOA_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //********** LP SCHOOL ONLINE EXAM MASTER EXAM  *****************//
        public LP_OnlineExamDTO getexammasterload(LP_OnlineExamDTO data)
        {
            try
            {
                data.getConfigurationSettings = _context.LP_Master_OE_SettingDMO.Where(a => a.MI_Id == data.MI_Id && a.LPMOES_ActiveFlg == true).ToArray();

                data.getyearlist = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                var getuserdetails = _context.Staff_User_Login.Where(a => a.MI_Id == data.MI_Id && a.Id == data.Userid).ToList();

                data.getarratcomplexities = _context.LP_Master_ComplexitiesDMO.Where(a => a.LPMCOMP_ActiveFlg == true).ToArray();

                var getMasterExamQuestiondetails = (from a in _context.LP_Master_OE_ExamDMO
                                                    from c in _context.AdmissionClass
                                                    from d in _context.IVRM_School_Master_SubjectsDMO
                                                    from e in _context.AcademicYear
                                                    from f in _context.School_M_Section
                                                    where (a.ISMS_Id == d.ISMS_Id && a.ASMCL_Id == c.ASMCL_Id && a.ASMS_Id == f.ASMS_Id
                                                    && a.ASMAY_Id == e.ASMAY_Id && a.MI_Id == data.MI_Id)
                                                    select new LP_OnlineExamDTO
                                                    {
                                                        LPMOEEX_Id = a.LPMOEEX_Id,
                                                        LPMOEEX_ExamName = a.LPMOEEX_ExamName,
                                                        LPMOEEX_NoOfQuestion = a.LPMOEEX_NoOfQuestion,
                                                        LPMOEEX_ActiveFlg = a.LPMOEEX_ActiveFlg,
                                                        LPMOEEX_RandomFlg = a.LPMOEEX_RandomFlg,
                                                        LPMOEEX_UploadExamPaperFlg = a.LPMOEEX_UploadExamPaperFlg,
                                                        LPMOEEX_ExamDuration = a.LPMOEEX_ExamDuration,
                                                        LPMOEEX_TotalMarks = a.LPMOEEX_TotalMarks,
                                                        ASMCL_Id = a.ASMCL_Id,
                                                        ISMS_Id = a.ISMS_Id,
                                                        ASMAY_Year = e.ASMAY_Year,
                                                        ASMCL_ClassName = c.ASMCL_ClassName,
                                                        ASMC_SectionName = f.ASMC_SectionName,
                                                        ISMS_SubjectName = d.ISMS_SubjectName,
                                                        countoption = _context.LP_Master_OE_Exam_LevelsDMO.Where(b => b.LPMOEEX_Id == a.LPMOEEX_Id).Count(),
                                                        counttopics = _context.LP_Master_OE_Exam_TopicsDMO.Where(b => b.LPMOEEX_Id == a.LPMOEEX_Id).Count(),
                                                        CreatedDate = a.LPMOEEX_CreatedDate,
                                                        LPMOEEX_AnswerPapeFileName = a.LPMOEEX_AnswerPapeFileName,
                                                        LPMOEEX_QuestionPapeFileName = a.LPMOEEX_QuestionPapeFileName,
                                                        LPMOEEX_QuestionPaper = a.LPMOEEX_QuestionPaper,
                                                        LPMOEEX_AnswerSheet = a.LPMOEEX_AnswerSheet,
                                                        LPMOEEX_FromDateTime = a.LPMOEEX_FromDateTime,
                                                        LPMOEEX_ToDateTime = a.LPMOEEX_ToDateTime,
                                                        LPMOEEX_AutoPublishFlg = a.LPMOEEX_AutoPublishFlg,
                                                        editcount = _context.LP_Students_ExamDMO.Where(b => b.LPMOEEX_Id == a.LPMOEEX_Id).Count(),
                                                        examname = a.EME_Id > 0 ? _context.exammasterDMO.Where(z => z.MI_Id == data.MI_Id
                                                        && z.EME_Id == a.EME_Id).Select(v => v.EME_ExamName).FirstOrDefault() : ""
                                                    }).Distinct().OrderByDescending(a => a.CreatedDate).ToArray();

                if (getuserdetails.Count > 0)
                {
                    var getempcode = getuserdetails.FirstOrDefault().Emp_Code;
                    var loginid = getuserdetails.FirstOrDefault().IVRMSTAUL_Id;

                    List<LP_OnlineExamDTO> getsubjects = new List<LP_OnlineExamDTO>();
                    List<long> subjectid = new List<long>();

                    List<LP_OnlineExamDTO> getclass = new List<LP_OnlineExamDTO>();
                    List<long> classid = new List<long>();

                    getsubjects = (from a in _context.Exm_Login_PrivilegeDMO
                                   from c in _context.Exm_Login_Privilege_SubjectsDMO
                                   from d in _context.IVRM_School_Master_SubjectsDMO
                                   from e in _context.Staff_User_Login
                                   where (a.ELP_Id == c.ELP_Id && e.IVRMSTAUL_Id == a.Login_Id && c.ISMS_Id == d.ISMS_Id && a.ELP_ActiveFlg == true
                                   && d.ISMS_ActiveFlag == 1 && a.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && c.ELPs_ActiveFlg == true && a.Login_Id == loginid
                                   && e.Emp_Code == getempcode)
                                   select new LP_OnlineExamDTO
                                   {
                                       ISMS_Id = c.ISMS_Id
                                   }).Distinct().ToList();

                    foreach (var c in getsubjects)
                    {
                        subjectid.Add(c.ISMS_Id);
                    }

                    getclass = (from a in _context.Exm_Login_PrivilegeDMO
                                from c in _context.Exm_Login_Privilege_SubjectsDMO
                                from d in _context.AdmissionClass
                                from e in _context.Staff_User_Login
                                where (a.ELP_Id == c.ELP_Id && e.IVRMSTAUL_Id == a.Login_Id && c.ASMCL_Id == d.ASMCL_Id && a.ELP_ActiveFlg == true
                                && a.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && c.ELPs_ActiveFlg == true && a.Login_Id == loginid
                                && e.Emp_Code == getempcode)
                                select new LP_OnlineExamDTO
                                {
                                    ASMCL_Id = c.ASMCL_Id
                                }).Distinct().ToList();

                    foreach (var c in getclass)
                    {
                        classid.Add(c.ASMCL_Id);
                    }

                    getMasterExamQuestiondetails = getMasterExamQuestiondetails.Where(a => subjectid.Contains(a.ISMS_Id)
                    && classid.Contains(a.ASMCL_Id)).ToArray();
                }

                data.getMasterExamQuestiondetails = getMasterExamQuestiondetails;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineExamDTO getexamclasslist(LP_OnlineExamDTO data)
        {
            try
            {
                var getuserdetails = _context.Staff_User_Login.Where(a => a.MI_Id == data.MI_Id && a.Id == data.Userid).ToList();

                var getinstitutiontype = _context.Institution.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToList();

                List<LP_OnlineExamDTO> getclass = new List<LP_OnlineExamDTO>();
                List<long> classid = new List<long>();

                if (getuserdetails.Count > 0)
                {
                    var getempcode = getuserdetails.FirstOrDefault().Emp_Code;
                    var loginid = getuserdetails.FirstOrDefault().IVRMSTAUL_Id;

                    getclass = (from a in _context.Exm_Login_PrivilegeDMO
                                from c in _context.Exm_Login_Privilege_SubjectsDMO
                                from d in _context.AdmissionClass
                                from e in _context.Staff_User_Login
                                where (a.ELP_Id == c.ELP_Id && e.IVRMSTAUL_Id == a.Login_Id && c.ASMCL_Id == d.ASMCL_Id && a.ELP_ActiveFlg == true
                                && a.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && c.ELPs_ActiveFlg == true && a.Login_Id == loginid
                                && e.Emp_Code == getempcode && a.ASMAY_Id == data.ASMAY_Id)
                                select new LP_OnlineExamDTO
                                {
                                    ASMCL_Id = c.ASMCL_Id
                                }).Distinct().ToList();


                    if (getclass.Count > 0)
                    {
                        foreach (var c in getclass)
                        {
                            classid.Add(c.ASMCL_Id);
                        }

                        data.getclasslist = _context.AdmissionClass.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true
                        && classid.Contains(a.ASMCL_Id)).OrderBy(a => a.ASMCL_Order).ToArray();
                    }
                }
                else
                {
                    data.getclasslist = (from a in _context.Masterclasscategory
                                         from b in _context.AcademicYear
                                         from c in _context.AdmissionClass
                                         where (a.ASMCL_Id == c.ASMCL_Id && a.ASMAY_Id == b.ASMAY_Id && a.Is_Active == true && a.ASMAY_Id == data.ASMAY_Id)
                                         select c).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineExamDTO getexamsectionslist(LP_OnlineExamDTO data)
        {
            try
            {
                var getuserdetails = _context.Staff_User_Login.Where(a => a.MI_Id == data.MI_Id && a.Id == data.Userid).ToList();

                var getinstitutiontype = _context.Institution.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToList();

                List<LP_OnlineExamDTO> getsection = new List<LP_OnlineExamDTO>();
                List<long> classsecid = new List<long>();

                if (getuserdetails.Count > 0)
                {
                    var getempcode = getuserdetails.FirstOrDefault().Emp_Code;
                    var loginid = getuserdetails.FirstOrDefault().IVRMSTAUL_Id;

                    getsection = (from a in _context.Exm_Login_PrivilegeDMO
                                  from c in _context.Exm_Login_Privilege_SubjectsDMO
                                  from d in _context.AdmissionClass
                                  from e in _context.Staff_User_Login
                                  from f in _context.School_M_Section
                                  where (a.ELP_Id == c.ELP_Id && e.IVRMSTAUL_Id == a.Login_Id && c.ASMCL_Id == d.ASMCL_Id && f.ASMS_Id == c.ASMS_Id
                                  && a.ELP_ActiveFlg == true && a.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && c.ELPs_ActiveFlg == true && a.Login_Id == loginid
                                  && e.Emp_Code == getempcode && a.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == data.ASMCL_Id)
                                  select new LP_OnlineExamDTO
                                  {
                                      ASMS_Id = c.ASMS_Id
                                  }).Distinct().ToList();


                    if (getsection.Count > 0)
                    {
                        foreach (var c in getsection)
                        {
                            classsecid.Add(c.ASMS_Id);
                        }

                        data.getsectionlist = _context.School_M_Section.Where(a => a.MI_Id == data.MI_Id && a.ASMC_ActiveFlag == 1
                        && classsecid.Contains(a.ASMS_Id)).OrderBy(a => a.ASMC_Order).ToArray();
                    }
                }
                else
                {
                    data.getsectionlist = (from a in _context.Masterclasscategory
                                           from b in _context.AcademicYear
                                           from c in _context.AdmissionClass
                                           from d in _context.School_M_Section
                                           from f in _context.AdmSchoolMasterClassCatSec
                                           where (a.ASMCL_Id == c.ASMCL_Id && a.ASMCC_Id == f.ASMCC_Id && d.ASMS_Id == f.ASMS_Id && a.ASMAY_Id == b.ASMAY_Id
                                           && f.ASMCCS_ActiveFlg == true && a.Is_Active == true && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id)
                                           select d).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineExamDTO getexamsubjectlist(LP_OnlineExamDTO data)
        {
            try
            {
                List<long?> sectionids = new List<long?>();

                if (data.editflag == "Edit")
                {
                    sectionids.Add(data.ASMS_Id);
                }
                else
                {
                    foreach (var c in data.sectiondetailslist)
                    {
                        sectionids.Add(c.ASMS_Id);
                    }
                }

                var getuserdetails = _context.Staff_User_Login.Where(a => a.MI_Id == data.MI_Id && a.Id == data.Userid).ToList();

                var getinstitutiontype = _context.Institution.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToList();

                List<long> subjectid = new List<long>();
                List<LP_OnlineExamDTO> getsubjects = new List<LP_OnlineExamDTO>();

                if (getuserdetails.Count > 0)
                {
                    var getschoolorcollege = getinstitutiontype.FirstOrDefault().MI_SchoolCollegeFlag;
                    var getempcode = getuserdetails.FirstOrDefault().Emp_Code;
                    var loginid = getuserdetails.FirstOrDefault().IVRMSTAUL_Id;

                    getsubjects = (from a in _context.Exm_Login_PrivilegeDMO
                                   from c in _context.Exm_Login_Privilege_SubjectsDMO
                                   from d in _context.IVRM_School_Master_SubjectsDMO
                                   from e in _context.Staff_User_Login
                                   where (a.ELP_Id == c.ELP_Id && e.IVRMSTAUL_Id == a.Login_Id && c.ISMS_Id == d.ISMS_Id && a.ELP_ActiveFlg == true
                                   && d.ISMS_ActiveFlag == 1 && a.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && c.ELPs_ActiveFlg == true && a.Login_Id == loginid
                                   && e.Emp_Code == getempcode && c.ASMCL_Id == data.ASMCL_Id && a.ASMAY_Id == data.ASMAY_Id && sectionids.Contains(c.ASMS_Id))
                                   select new LP_OnlineExamDTO
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
                                           from c in _context.Exm_Yearly_Category_GroupDMO
                                           from d in _context.Exm_Yearly_Category_Group_SubjectsDMO
                                           from e in _context.IVRM_School_Master_SubjectsDMO
                                           where (a.EMCA_Id == b.EMCA_Id && b.EYC_Id == c.EYC_Id && c.EYCG_Id == d.EYCG_Id && d.ISMS_Id == e.ISMS_Id
                                           && a.ECAC_ActiveFlag == true && b.EYC_ActiveFlg == true && c.EYCG_ActiveFlg == true && d.EYCGS_ActiveFlg == true
                                           && a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMAY_Id == data.ASMAY_Id && b.ASMAY_Id == data.ASMAY_Id
                                           && sectionids.Contains(a.ASMS_Id))
                                           select e).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();
                }

                var getmappedexam_subject = _context.LP_Master_OE_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == data.ASMCL_Id && sectionids.Contains(a.ASMS_Id) && a.ISMS_Id == data.ISMS_Id && a.LPMOEEX_ActiveFlg == true).ToList();

                List<long?> emeids_new = new List<long?>();

                if (data.editflag != "Edit")
                {
                    foreach (var c in getmappedexam_subject)
                    {
                        emeids_new.Add(c.EME_Id);
                    }
                }


                var emcaids = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                && sectionids.Contains(a.ASMS_Id) && a.ECAC_ActiveFlag == true).Distinct().Select(a => a.EMCA_Id).ToList();

                var eycids = _context.Exm_Yearly_CategoryDMO.Where(a => a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && emcaids.Contains(a.EMCA_Id)
                 && a.EYC_ActiveFlg == true).Distinct().Select(a => a.EYC_Id).ToList();

                var emeids = _context.Exm_Yearly_Category_ExamsDMO.Where(a => eycids.Contains(a.EYC_Id)
                && a.EYCE_ActiveFlg == true && !emeids_new.Contains(a.EME_Id)).Distinct().Select(a => a.EME_Id).ToList();

                data.getmasterexamdetails = _context.exammasterDMO.Where(a => a.MI_Id == data.MI_Id && a.EME_ActiveFlag == true
                && emeids.Contains(a.EME_Id)).OrderBy(a => a.EME_ExamOrder).ToArray();

                //data.getmasterexamdetails = _context.exammasterDMO.Where(a => a.MI_Id == data.MI_Id && a.EME_ActiveFlag == true
                //&& emeids.Contains(a.EME_Id)).OrderBy(a => a.EME_ExamOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineExamDTO GetSearchTopics(LP_OnlineExamDTO data)
        {
            try
            {
                List<long> sectionids = new List<long>();

                if (data.editflag == "Edit")
                {
                    sectionids.Add(data.ASMS_Id);
                }
                else
                {
                    foreach (var c in data.sectiondetailslist)
                    {
                        sectionids.Add(c.ASMS_Id);
                    }
                }

                //data.gettopiclist = (from a in _context.LP_Master_OE_QuestionsDMO
                //                     from b in _context.SchoolSubjectWithMasterTopicMapping
                //                     where (a.LPMT_Id == b.LPMT_Id && a.ASMCL_Id == data.ASMCL_Id && a.ISMS_Id == data.ISMS_Id)
                //                     select b).Distinct().ToArray();


                data.gettopiclist = (from a in _context.MasterSchoolTopicDMO
                                     from b in _context.SchoolSubjectWithMasterTopicMapping
                                     where (a.LPMMT_Id == b.LPMMT_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMAY_Id == data.ASMAY_Id && a.ISMS_Id == data.ISMS_Id)
                                     select b).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineExamDTO SearchQuestions(LP_OnlineExamDTO data)
        {
            try
            {
                List<long> sectionids = new List<long>();

                if (data.editflag == "Edit")
                {
                    sectionids.Add(data.ASMS_Id);
                }
                else
                {
                    foreach (var c in data.sectiondetailslist)
                    {
                        sectionids.Add(c.ASMS_Id);
                    }
                }

                List<long> ids = new List<long>();

                if (data.editflag == "Edit")
                {
                    var topicslist = _context.LP_Master_OE_Exam_TopicsDMO.Where(a => a.LPMOEEX_Id == data.LPMOEEX_Id).ToList();
                    if (topicslist.Count > 0)
                    {
                        foreach (var c in topicslist)
                        {
                            ids.Add(c.LPMT_Id);
                        }
                    }
                }
                else
                {
                    foreach (var c in data.temptopics)
                    {
                        ids.Add(c.LPMT_Id);
                    }
                }

                List<LP_Master_OE_QuestionsDMO> LP_Master_OE_QuestionsDMO = new List<LP_Master_OE_QuestionsDMO>();

                if (data.LPMOEEX_RandomFlg == false)
                {
                    data.getquestionlist = _context.LP_Master_OE_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                    && a.ISMS_Id == data.ISMS_Id && a.LPMOEQ_ActiveFlg == true && ids.Contains(a.LPMT_Id) && a.LPMCOMP_Id != null).Distinct().ToArray();
                }
                else
                {
                    var query = _context.LP_Master_OE_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                    && a.ISMS_Id == data.ISMS_Id && a.LPMOEQ_ActiveFlg == true && ids.Contains(a.LPMT_Id) && a.LPMCOMP_Id != null).Distinct().ToList();
                    int count = Convert.ToInt32(data.LPMOEEX_NoOfQuestion);

                    LP_Master_OE_QuestionsDMO = query.OrderBy(x => Guid.NewGuid()).Take(count).ToList();

                    data.getquestionlist = LP_Master_OE_QuestionsDMO.ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineExamDTO SaveMasterExamQuestionDetails(LP_OnlineExamDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                string message = "";

                DateTime fromdate = new DateTime();
                string confromdate = "";

                fromdate = Convert.ToDateTime(data.LPMOEEX_FromDateTime.Value.AddHours(data.fhrors).AddMinutes(data.fminutes).AddSeconds(data.fsec).
                    ToString("yyyy-MM-dd HH:mm:ss"));
                confromdate = fromdate.ToString("yyyy-MM-dd HH:mm:ss");

                DateTime todate = new DateTime();
                string contodate = "";

                todate = Convert.ToDateTime(data.LPMOEEX_ToDateTime.Value.AddHours(data.thrors).AddMinutes(data.tminutes).AddSeconds(data.tsec).
                    ToString("yyyy-MM-dd HH:mm:ss"));
                contodate = todate.ToString("yyyy-MM-dd HH:mm:ss");

                if (data.LPMOEEX_Id > 0)
                {
                    var asms_id = data.sectiondetailslist[0].ASMS_Id;

                    var checkduplicate = _context.LP_Master_OE_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                    && a.ISMS_Id == data.ISMS_Id && a.ASMS_Id == asms_id && a.LPMOEEX_ExamName.Equals(data.LPMOEEX_ExamName)
                    && a.LPMOEEX_Id != data.LPMOEEX_Id && a.LPMOEEX_ActiveFlg == true).ToList();

                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        data.message = "Update";

                        var checkresult = _context.LP_Master_OE_ExamDMO.Single(a => a.LPMOEEX_Id == data.LPMOEEX_Id);
                        checkresult.LPMOEEX_ExamName = data.LPMOEEX_ExamName;
                        checkresult.LPMOEEX_NoOfQuestion = data.LPMOEEX_NoOfQuestion;
                        checkresult.LPMOEEX_RandomFlg = data.LPMOEEX_RandomFlg;
                        checkresult.LPMOEEX_UploadExamPaperFlg = data.LPMOEEX_UploadExamPaperFlg;
                        checkresult.LPMOEEX_ExamDuration = data.LPMOEEX_ExamDuration;
                        checkresult.LPMOEEX_TotalMarks = data.LPMOEEX_TotalMarks;

                        checkresult.LPMOEEX_AnswerPapeFileName = data.LPMOEEX_AnswerPapeFileName;
                        checkresult.LPMOEEX_AnswerSheet = data.LPMOEEX_AnswerSheet;
                        checkresult.LPMOEEX_AutoPublishFlg = data.LPMOEEX_AutoPublishFlg;

                        checkresult.LPMOEEX_QuestionPapeFileName = data.LPMOEEX_QuestionPapeFileName;
                        checkresult.LPMOEEX_QuestionPaper = data.LPMOEEX_QuestionPaper;
                        checkresult.EME_Id = data.EME_Id > 0 ? data.EME_Id : null;
                        checkresult.LPMOEEX_UpdatedBy = data.Userid;
                        checkresult.LPMOEEX_UpdatedDate = indiantime0;
                        checkresult.LPMOEEX_FromDateTime = data.LPMOEEX_FromDateTime.Value.AddHours(data.fhrors).AddMinutes(data.fminutes).AddSeconds(data.fsec);
                        checkresult.LPMOEEX_ToDateTime = data.LPMOEEX_ToDateTime.Value.AddHours(data.thrors).AddMinutes(data.tminutes).AddSeconds(data.tsec);

                        checkresult.LPMOEEX_AllowDownloadQnsPaperBeforeExamFlg = data.LPMOEEX_AllowDownloadQnsPaperBeforeExamFlg;
                        checkresult.LPMOEEX_Duration = data.LPMOEEX_Duration;
                        checkresult.LPMOEEX_DurationFlag = data.LPMOEEX_DurationFlag;
                        checkresult.LPMOEEX_NotLinkedToQnsBankFlg = data.LPMOEEX_NotLinkedToQnsBankFlg;

                        _context.Update(checkresult);

                        List<long> topicids = new List<long>();

                        if (data.temptopicDTO != null && data.temptopicDTO.Length > 0)
                        {
                            foreach (var c in data.temptopicDTO)
                            {
                                topicids.Add(c.LPMOEEXTOP_Id);
                            }

                            Array gettopics_Id = _context.LP_Master_OE_Exam_TopicsDMO.Where(a => a.LPMOEEX_Id == data.LPMOEEX_Id
                            && !topicids.Contains(a.LPMOEEXTOP_Id)).ToArray();

                            foreach (LP_Master_OE_Exam_TopicsDMO ph1 in gettopics_Id)
                            {
                                _context.Remove(ph1);
                            }

                            foreach (var c in data.temptopicDTO)
                            {
                                if (c.LPMOEEXTOP_Id > 0)
                                {
                                    var checkresulttopic = _context.LP_Master_OE_Exam_TopicsDMO.Single(a => a.LPMOEEXTOP_Id == c.LPMOEEXTOP_Id
                                    && a.LPMOEEX_Id == data.LPMOEEX_Id);
                                    checkresulttopic.LPMT_Id = c.LPMT_Id;
                                    checkresulttopic.LPMOEEXQNS_UpdatedBy = data.Userid;
                                    checkresulttopic.LPMOEEXQNS_UpdatedDate = indiantime0;
                                    _context.Update(checkresulttopic);
                                }
                                else
                                {
                                    LP_Master_OE_Exam_TopicsDMO lP_Master_OE_Exam_TopicsDMO = new LP_Master_OE_Exam_TopicsDMO();
                                    lP_Master_OE_Exam_TopicsDMO.LPMT_Id = c.LPMT_Id;
                                    lP_Master_OE_Exam_TopicsDMO.LPMOEEX_Id = data.LPMOEEX_Id;
                                    lP_Master_OE_Exam_TopicsDMO.LPMOEEXTOP_ActiveFlg = true;
                                    lP_Master_OE_Exam_TopicsDMO.LPMOEEXQNS_UpdatedBy = data.Userid;
                                    lP_Master_OE_Exam_TopicsDMO.LPMOEEXQNS_CreatedBy = data.Userid;
                                    lP_Master_OE_Exam_TopicsDMO.LPMOEEXQNS_UpdatedDate = indiantime0;
                                    lP_Master_OE_Exam_TopicsDMO.LPMOEEXQNS_CreatedDate = indiantime0;
                                    _context.Add(lP_Master_OE_Exam_TopicsDMO);
                                }
                            }
                        }

                        List<long?> remove_levelids = new List<long?>();
                        List<long> levelids = new List<long>();
                        List<long> quesid = new List<long>();
                        List<long> quesid_files = new List<long>();
                        List<long> level_ques_choose = new List<long>();
                        List<long> level_ques_mf = new List<long>();

                        if (data.ExamLevelDetails != null && data.ExamLevelDetails.Length > 0)
                        {
                            foreach (var l in data.ExamLevelDetails)
                            {
                                levelids.Add(l.LPMOEEXLVL_Id);
                            }

                            var get_examlevelids = _context.LP_Master_OE_Exam_LevelsDMO.Where(a => a.LPMOEEX_Id == data.LPMOEEX_Id
                            && !levelids.Contains(a.LPMOEEXLVL_Id)).ToList();

                            foreach (var lq in get_examlevelids)
                            {
                                remove_levelids.Add(lq.LPMOEEXLVL_Id);
                            }

                            var getlevel_question = _context.LP_Master_OE_Exam_QuestionsDMO.Where(a => remove_levelids.Contains(a.LPMOEEXLVL_Id)).ToList();

                            if (data.LPMOEEX_NotLinkedToQnsBankFlg == true)
                            {
                                foreach (var ques in getlevel_question)
                                {
                                    quesid.Add(ques.LPMOEEXQNS_Id);
                                }

                                // Removing Manual Question Files
                                var getlevelques_files = _context.LP_Master_OE_Exam_Questions_FilesDMO.Where(a => quesid.Contains(a.LPMOEEXQNS_Id)).ToList();
                                foreach (var ques_files in getlevelques_files)
                                {
                                    _context.Remove(ques_files);
                                }

                                var getlevelques_options = _context.LP_Master_OE_Exam_Questions_OptionsDMO.Where(a => quesid.Contains(a.LPMOEEXQNS_Id)).ToList();

                                foreach (var ques_options in getlevelques_options)
                                {
                                    level_ques_choose.Add(ques_options.LPMOEEXQNSOPT_Id);
                                }

                                var getlevelques_options_files = _context.LP_Master_OE_Exam_Questions_Options_FilesDMO.Where(a => level_ques_choose.Contains(a.LPMOEEXQNSOPT_Id)).ToList();

                                var getlevelques_options_Mf = _context.LP_Master_OE_Exam_Questions_Options_MFDMO.Where(a => level_ques_choose.Contains(a.LPMOEEXQNSOPT_Id)).ToList();

                                // Removing Manual Question Option MF
                                foreach (var ques_options_mf in getlevelques_options_Mf)
                                {
                                    _context.Remove(ques_options_mf);
                                }
                                // Removing Manual Question Option Files
                                foreach (var ques_options_files in getlevelques_options_files)
                                {
                                    _context.Remove(ques_options_files);
                                }
                                // Removing Manual Question Option
                                foreach (var ques_options in getlevelques_options)
                                {
                                    _context.Remove(ques_options);
                                }
                            }

                            foreach (var lques in getlevel_question)
                            {
                                _context.Remove(lques);
                            }

                            foreach (var lq in get_examlevelids)
                            {
                                _context.Remove(lq);
                            }

                            foreach (var d in data.ExamLevelDetails)
                            {
                                if (d.LPMOEEXLVL_Id > 0)
                                {
                                    var level_result = _context.LP_Master_OE_Exam_LevelsDMO.Single(a => a.LPMOEEXLVL_Id == d.LPMOEEXLVL_Id);

                                    level_result.LPMOEEXLVL_LevelDesc = d.LPMOEEXLVL_LevelDesc;
                                    level_result.LPMOEEXLVL_TotalNoOfQns = d.LPMOEEXLVL_TotalNoOfQns;
                                    level_result.LPMOEEXLVL_MaxQns = d.LPMOEEXLVL_MaxQns;
                                    level_result.LPMOEEXLVL_LevelTotalMarks = d.LPMOEEXLVL_LevelTotalMarks;
                                    level_result.LPMOEEXLVL_MarksPerQns = d.LPMOEEXLVL_MarksPerQns;
                                    level_result.LPMOEEXLVL_UpdatedBy = data.Userid;
                                    level_result.LPMOEEXLVL_UpdatedDate = indiantime0;
                                    level_result.LPMOEEXLVL_LevelOrder = d.LPMOEEXLVL_LevelOrder;
                                    _context.Update(level_result);

                                    foreach (var e in d.questionlist)
                                    {
                                        if (e.LPMOEEXQNS_Id > 0)
                                        {
                                            var ques_result = _context.LP_Master_OE_Exam_QuestionsDMO.Single(a => a.LPMOEEXQNS_Id == e.LPMOEEXQNS_Id);
                                            ques_result.LPMOEQ_Id = e.LPMOEQ_Id;
                                            ques_result.LPMOEEXQNS_UpdatedBy = data.Userid;
                                            ques_result.LPMOEEXQNS_UpdatedDate = indiantime0;
                                            ques_result.LPMOEEXQNS_Marks = e.LPMOEEXQNS_Marks;
                                            ques_result.LPMOEEXQNS_QnsOrder = e.LPMOEEXQNS_QnsOrder;

                                            ques_result.LPMOEEXQNS_Question = data.LPMOEEX_NotLinkedToQnsBankFlg == true ? e.LPMOEEXQNS_Question : null;
                                            ques_result.LPMOEEXQNS_SubjectiveFlg = data.LPMOEEX_NotLinkedToQnsBankFlg == true ? e.LPMOEEXQNS_SubjectiveFlg : null;
                                            ques_result.LPMOEEXQNS_Answer = data.LPMOEEX_NotLinkedToQnsBankFlg == true ? e.LPMOEEXQNS_Answer : null;
                                            ques_result.LPMOEEXQNS_MatchTheFollowingFlg = data.LPMOEEX_NotLinkedToQnsBankFlg == true ? e.LPMOEEXQNS_MatchTheFollowingFlg : null;
                                            ques_result.LPMOEEXQNS_NoOfOptions = data.LPMOEEX_NotLinkedToQnsBankFlg == true ? e.LPMOEEXQNS_NoOfOptions : null;
                                            ques_result.LPMOEEXQNS_NoOfRows = data.LPMOEEX_NotLinkedToQnsBankFlg == true ? e.LPMOEEXQNS_NoOfRows : null;
                                            ques_result.LPMOEEXQNS_NoOfColumns = data.LPMOEEX_NotLinkedToQnsBankFlg == true ? e.LPMOEEXQNS_NoOfColumns : null;
                                            ques_result.LPMOEEXQNS_QuestionType = data.LPMOEEX_NotLinkedToQnsBankFlg == true ? e.LPMOEEXQNS_QuestionType : null;

                                            _context.Update(ques_result);
                                        }

                                        if (data.LPMOEEX_NotLinkedToQnsBankFlg == true && e.Temp_Manual_Ques_Options != null && e.Temp_Manual_Ques_Options.Length > 0)
                                        {
                                            // Updating Manual Questions Files
                                            if (e.Temp_Manual_Ques_Files != null && e.Temp_Manual_Ques_Files.Length > 0)
                                            {
                                                foreach (var ques_files in e.Temp_Manual_Ques_Files)
                                                {
                                                    if (ques_files.LPMOEEXQNSF_Id > 0)
                                                    {
                                                        var ques_files_result = _context.LP_Master_OE_Exam_Questions_FilesDMO.Single(a => a.LPMOEEXQNSF_Id == ques_files.LPMOEEXQNSF_Id);
                                                        ques_files_result.LPMOEEXQNSF_FileName = ques_files.LPMOEEXQNSF_FileName;
                                                        ques_files_result.LPMOEEXQNSF_FilePath = ques_files.LPMOEEXQNSF_FilePath;
                                                        ques_files_result.LPMOEEXQNSF_UpdatedDate = indiantime0;
                                                        ques_files_result.LPMOEEXQNSF_UpdatedBy = data.Userid;
                                                        _context.Update(ques_files_result);
                                                    }
                                                }
                                            }

                                            // Updating Manual Questions Wise Options
                                            foreach (var f in e.Temp_Manual_Ques_Options)
                                            {
                                                if (f.LPMOEEXQNSOPT_Id > 0)
                                                {
                                                    var ques_option_result = _context.LP_Master_OE_Exam_Questions_OptionsDMO.Single(a => a.LPMOEEXQNSOPT_Id == f.LPMOEEXQNSOPT_Id);

                                                    ques_option_result.LPMOEEXQNSOPT_Option = f.LPMOEEXQNSOPT_Option;
                                                    ques_option_result.LPMOEEXQNSOPT_OptionCode = f.LPMOEEXQNSOPT_OptionCode;
                                                    ques_option_result.LPMOEEXQNSOPT_OptionImage = null;
                                                    ques_option_result.LPMOEEXQNSOPT_AnswerFlag = f.LPMOEEXQNSOPT_AnswerFlag;
                                                    ques_option_result.LPMOEEXQNSOPT_AnswerDesc = null;
                                                    ques_option_result.LPMOEEXQNSOPT_UpdatedBy = data.Userid;
                                                    ques_option_result.LPMOEEXQNSOPT_UpdatedDate = indiantime0;
                                                    ques_option_result.LPMOEEXQNSOPT_Marks = e.LPMOEEXQNS_MatchTheFollowingFlg == true ? f.LPMOEEXQNSOPT_Marks : null;
                                                    _context.Update(ques_option_result);
                                                }

                                                // Updating Manual Questions Option Wise Files
                                                if (f.Temp_Manual_Ques_Opts_Files != null && f.Temp_Manual_Ques_Opts_Files.Length > 0)
                                                {
                                                    foreach (var ques_opts_files in f.Temp_Manual_Ques_Opts_Files)
                                                    {
                                                        if (ques_opts_files.LPMOEEXQNSOPTF_Id > 0)
                                                        {
                                                            var ques_opts_files_result = _context.LP_Master_OE_Exam_Questions_Options_FilesDMO.Single(a => a.LPMOEEXQNSOPTF_Id == ques_opts_files.LPMOEEXQNSOPTF_Id);
                                                            ques_opts_files_result.LPMOEEXQNSOPTF_FileName = ques_opts_files.LPMOEEXQNSOPTF_FileName;
                                                            ques_opts_files_result.LPMOEEXQNSOPTF_FilePath = ques_opts_files.LPMOEEXQNSOPTF_FilePath;
                                                            ques_opts_files_result.LPMOEEXQNSOPTF_UpdatedDate = indiantime0;
                                                            ques_opts_files_result.LPMOEEXQNSOPTF_UpdatedBy = data.Userid;
                                                            _context.Update(ques_opts_files_result);
                                                        }
                                                    }
                                                }

                                                // Updating Manual Questions Option Wise Match The Following
                                                if (f.Temp_Manual_Ques_Options_Mf != null && f.Temp_Manual_Ques_Options_Mf.Length > 0)
                                                {
                                                    foreach (var g in f.Temp_Manual_Ques_Options_Mf)
                                                    {
                                                        if (g.LPMOEEXQNSOPTMF_Id > 0)
                                                        {
                                                            var ques_option_mf_result = _context.LP_Master_OE_Exam_Questions_Options_MFDMO.Single(a => a.LPMOEEXQNSOPTMF_Id == g.LPMOEEXQNSOPTMF_Id);

                                                            ques_option_mf_result.LPMOEEXQNSOPTMF_MatchtheFollowing = g.LPMOEEXQNSOPTMF_MatchtheFollowing;
                                                            ques_option_mf_result.LPMOEEXQNSOPTMF_Answer_LPMOEEXQNSOPT = null;
                                                            ques_option_mf_result.LPMOEEXQNSOPTMF_UpdatedBy = data.Userid;
                                                            ques_option_mf_result.LPMOEEXQNSOPTMF_UpdatedDate = indiantime0;
                                                            ques_option_mf_result.LPMOEEXQNSOPTMF_Answer_Flg = g.LPMOEEXQNSOPTMF_Answer_Flg;
                                                            ques_option_mf_result.LPMOEEXQNSOPTMF_Order = g.LPMOEEXQNSOPTMF_Order;
                                                            _context.Update(ques_option_mf_result);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                else
                                {
                                    LP_Master_OE_Exam_LevelsDMO lP_Master_OE_Exam_LevelsDMO = new LP_Master_OE_Exam_LevelsDMO
                                    {
                                        LPMOEEX_Id = data.LPMOEEX_Id,
                                        LPMOEEXLVL_LevelDesc = d.LPMOEEXLVL_LevelDesc,
                                        LPMOEEXLVL_TotalNoOfQns = d.LPMOEEXLVL_TotalNoOfQns,
                                        LPMOEEXLVL_MaxQns = d.LPMOEEXLVL_MaxQns,
                                        LPMOEEXLVL_LevelTotalMarks = d.LPMOEEXLVL_LevelTotalMarks,
                                        LPMOEEXLVL_MarksPerQns = d.LPMOEEXLVL_MarksPerQns,
                                        LPMOEEXLVL_ActiveFlg = true,
                                        LPMOEEXLVL_CreatedBy = data.Userid,
                                        LPMOEEXLVL_CreatedDate = indiantime0,
                                        LPMOEEXLVL_UpdatedBy = data.Userid,
                                        LPMOEEXLVL_UpdatedDate = indiantime0,
                                        LPMOEEXLVL_LevelOrder = d.LPMOEEXLVL_LevelOrder
                                    };
                                    _context.Add(lP_Master_OE_Exam_LevelsDMO);

                                    foreach (var e in d.questionlist)
                                    {
                                        LP_Master_OE_Exam_QuestionsDMO lP_Master_OE_Exam_QuestionsDMO = new LP_Master_OE_Exam_QuestionsDMO
                                        {
                                            LPMOEEXLVL_Id = lP_Master_OE_Exam_LevelsDMO.LPMOEEXLVL_Id,
                                            LPMOEQ_Id = e.LPMOEQ_Id,
                                            LPMOEEXQNS_Marks = e.LPMOEEXQNS_Marks,
                                            LPMOEEXQNS_ActiveFlg = true,
                                            LPMOEEXQNS_CreatedBy = data.Userid,
                                            LPMOEEXQNS_UpdatedBy = data.Userid,
                                            LPMOEEXQNS_CreatedDate = indiantime0,
                                            LPMOEEXQNS_UpdatedDate = indiantime0,
                                            LPMOEEXQNS_QnsOrder = e.LPMOEEXQNS_QnsOrder,

                                            LPMOEEXQNS_Question = data.LPMOEEX_NotLinkedToQnsBankFlg == true ? e.LPMOEEXQNS_Question : null,
                                            LPMOEEXQNS_SubjectiveFlg = data.LPMOEEX_NotLinkedToQnsBankFlg == true ? e.LPMOEEXQNS_SubjectiveFlg : null,
                                            LPMOEEXQNS_Answer = data.LPMOEEX_NotLinkedToQnsBankFlg == true ? e.LPMOEEXQNS_Answer : null,
                                            LPMOEEXQNS_MatchTheFollowingFlg = data.LPMOEEX_NotLinkedToQnsBankFlg == true ? e.LPMOEEXQNS_MatchTheFollowingFlg : null,
                                            LPMOEEXQNS_NoOfOptions = data.LPMOEEX_NotLinkedToQnsBankFlg == true ? e.LPMOEEXQNS_NoOfOptions : null,
                                            LPMOEEXQNS_NoOfRows = data.LPMOEEX_NotLinkedToQnsBankFlg == true ? e.LPMOEEXQNS_NoOfRows : null,
                                            LPMOEEXQNS_NoOfColumns = data.LPMOEEX_NotLinkedToQnsBankFlg == true ? e.LPMOEEXQNS_NoOfColumns : null,
                                            LPMOEEXQNS_QuestionType = data.LPMOEEX_NotLinkedToQnsBankFlg == true ? e.LPMOEEXQNS_QuestionType : null,
                                        };
                                        _context.Add(lP_Master_OE_Exam_QuestionsDMO);

                                        if (data.LPMOEEX_NotLinkedToQnsBankFlg == true)
                                        {
                                            // Adding Manual Questions Files
                                            if (e.Temp_Manual_Ques_Files != null && e.Temp_Manual_Ques_Files.Length > 0)
                                            {
                                                foreach (var ques_files in e.Temp_Manual_Ques_Files)
                                                {
                                                    LP_Master_OE_Exam_Questions_FilesDMO lP_Master_OE_Exam_Questions_FilesDMO = new LP_Master_OE_Exam_Questions_FilesDMO
                                                    {
                                                        LPMOEEXQNS_Id = lP_Master_OE_Exam_QuestionsDMO.LPMOEEXQNS_Id,
                                                        LPMOEEXQNSF_FileName = ques_files.LPMOEEXQNSF_FileName,
                                                        LPMOEEXQNSF_FilePath = ques_files.LPMOEEXQNSF_FilePath,
                                                        LPMOEEXQNSF_ActiveFlag = true,
                                                        LPMOEEXQNSF_CreatedDate = indiantime0,
                                                        LPMOEEXQNSF_UpdatedDate = indiantime0,
                                                        LPMOEEXQNSF_CreatedBy = data.Userid,
                                                        LPMOEEXQNSF_UpdatedBy = data.Userid
                                                    };
                                                    _context.Add(lP_Master_OE_Exam_Questions_FilesDMO);
                                                }
                                            }

                                            // Adding Manual Questions Wise Options
                                            if (e.LPMOEEXQNS_SubjectiveFlg != true && e.Temp_Manual_Ques_Options != null && e.Temp_Manual_Ques_Options.Length > 0)
                                            {
                                                foreach (var f in e.Temp_Manual_Ques_Options)
                                                {
                                                    LP_Master_OE_Exam_Questions_OptionsDMO lP_Master_OE_Exam_Questions_OptionsDMO =
                                                        new LP_Master_OE_Exam_Questions_OptionsDMO
                                                        {
                                                            LPMOEEXQNS_Id = lP_Master_OE_Exam_QuestionsDMO.LPMOEEXQNS_Id,
                                                            LPMOEEXQNSOPT_Option = f.LPMOEEXQNSOPT_Option,
                                                            LPMOEEXQNSOPT_OptionCode = f.LPMOEEXQNSOPT_OptionCode,
                                                            LPMOEEXQNSOPT_OptionImage = null,
                                                            LPMOEEXQNSOPT_AnswerFlag = f.LPMOEEXQNSOPT_AnswerFlag,
                                                            LPMOEEXQNSOPT_AnswerDesc = null,
                                                            LPMOEEXQNSOPT_ActiveFlg = true,
                                                            LPMOEEXQNSOPT_CreatedBy = data.Userid,
                                                            LPMOEEXQNSOPT_CreatedDate = indiantime0,
                                                            LPMOEEXQNSOPT_UpdatedBy = data.Userid,
                                                            LPMOEEXQNSOPT_UpdatedDate = indiantime0,
                                                            LPMOEEXQNSOPT_Marks = e.LPMOEEXQNS_MatchTheFollowingFlg == true ? f.LPMOEEXQNSOPT_Marks : null,
                                                        };
                                                    _context.Add(lP_Master_OE_Exam_Questions_OptionsDMO);

                                                    // Adding Manual Questions Options File
                                                    if (f.Temp_Manual_Ques_Opts_Files != null && f.Temp_Manual_Ques_Opts_Files.Length > 0)
                                                    {
                                                        foreach (var ques_opts_files in f.Temp_Manual_Ques_Opts_Files)
                                                        {
                                                            LP_Master_OE_Exam_Questions_Options_FilesDMO lP_Master_OE_Exam_Questions_Options_FilesDMO = new LP_Master_OE_Exam_Questions_Options_FilesDMO
                                                            {
                                                                LPMOEEXQNSOPT_Id = lP_Master_OE_Exam_Questions_OptionsDMO.LPMOEEXQNSOPT_Id,
                                                                LPMOEEXQNSOPTF_FileName = ques_opts_files.LPMOEEXQNSOPTF_FileName,
                                                                LPMOEEXQNSOPTF_FilePath = ques_opts_files.LPMOEEXQNSOPTF_FilePath,
                                                                LPMOEEXQNSOPTF_ActiveFlag = true,
                                                                LPMOEEXQNSOPTF_CreatedDate = indiantime0,
                                                                LPMOEEXQNSOPTF_UpdatedDate = indiantime0,
                                                                LPMOEEXQNSOPTF_CreatedBy = data.Userid,
                                                                LPMOEEXQNSOPTF_UpdatedBy = data.Userid
                                                            };
                                                            _context.Add(lP_Master_OE_Exam_Questions_Options_FilesDMO);
                                                        }
                                                    }

                                                    // Adding Manual Questions Options Wise Match The Following
                                                    if (e.LPMOEEXQNS_MatchTheFollowingFlg == true && f.Temp_Manual_Ques_Options_Mf != null
                                                        && f.Temp_Manual_Ques_Options_Mf.Length > 0)
                                                    {
                                                        foreach (var g in f.Temp_Manual_Ques_Options_Mf)
                                                        {
                                                            LP_Master_OE_Exam_Questions_Options_MFDMO lP_Master_OE_Exam_Questions_Options_MFDMO =
                                                                new LP_Master_OE_Exam_Questions_Options_MFDMO
                                                                {
                                                                    LPMOEEXQNSOPT_Id = lP_Master_OE_Exam_Questions_OptionsDMO.LPMOEEXQNSOPT_Id,
                                                                    LPMOEEXQNSOPTMF_MatchtheFollowing = g.LPMOEEXQNSOPTMF_MatchtheFollowing,
                                                                    LPMOEEXQNSOPTMF_Answer_LPMOEEXQNSOPT = null,
                                                                    LPMOEEXQNSOPTMF_ActiveFlg = true,
                                                                    LPMOEEXQNSOPTMF_CreatedBy = data.Userid,
                                                                    LPMOEEXQNSOPTMF_CreatedDate = indiantime0,
                                                                    LPMOEEXQNSOPTMF_UpdatedBy = data.Userid,
                                                                    LPMOEEXQNSOPTMF_UpdatedDate = indiantime0,
                                                                    LPMOEEXQNSOPTMF_Answer_Flg = g.LPMOEEXQNSOPTMF_Answer_Flg,
                                                                    LPMOEEXQNSOPTMF_Order = g.LPMOEEXQNSOPTMF_Order
                                                                };

                                                            _context.Add(lP_Master_OE_Exam_Questions_Options_MFDMO);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
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
                else
                {
                    foreach (var cd in data.sectiondetailslist)
                    {
                        List<LP_OnlineExamDTO> result = new List<LP_OnlineExamDTO>();

                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "LP_OE_Schedule_Records_Insert_21Oct2020";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 300000;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                            cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = data.ISMS_Id });
                            cmd.Parameters.Add(new SqlParameter("@Fromdate", SqlDbType.VarChar) { Value = confromdate });
                            cmd.Parameters.Add(new SqlParameter("@Todate", SqlDbType.VarChar) { Value = contodate });
                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = cd.ASMS_Id });
                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var retObject = new List<dynamic>();
                            try
                            {
                                using (var dataReader = cmd.ExecuteReader())
                                {
                                    while (dataReader.Read())
                                    {
                                        result.Add(new LP_OnlineExamDTO
                                        {
                                            countclass = Convert.ToInt64(dataReader["IdDuplicate"]),
                                        });
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }

                        if (result.FirstOrDefault().countclass == 0)
                        {
                            data.message = "Add";

                            var checkduplicate = _context.LP_Master_OE_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                            && a.ASMCL_Id == data.ASMCL_Id && a.ISMS_Id == data.ISMS_Id && a.LPMOEEX_ExamName.Equals(data.LPMOEEX_ExamName)
                            && a.LPMOEEX_ActiveFlg == true && a.ASMS_Id == cd.ASMS_Id).ToList();

                            //LP_Master_OE_ExamDMO lP_Master_OE_ExamDMO = new LP_Master_OE_ExamDMO();

                            LP_Master_OE_ExamDMO lP_Master_OE_ExamDMO = new LP_Master_OE_ExamDMO
                            {
                                MI_Id = data.MI_Id,
                                ASMAY_Id = data.ASMAY_Id,
                                ASMCL_Id = data.ASMCL_Id,
                                ISMS_Id = data.ISMS_Id,
                                ASMS_Id = cd.ASMS_Id,
                                LPMOEEX_ExamName = data.LPMOEEX_ExamName,
                                LPMOEEX_NoOfQuestion = data.LPMOEEX_NoOfQuestion,
                                LPMOEEX_RandomFlg = data.LPMOEEX_RandomFlg,
                                LPMOEEX_UploadExamPaperFlg = data.LPMOEEX_UploadExamPaperFlg,
                                LPMOEEX_ExamDuration = data.LPMOEEX_ExamDuration,
                                LPMOEEX_TotalMarks = data.LPMOEEX_TotalMarks,
                                LPMOEEX_AnswerPapeFileName = data.LPMOEEX_AnswerPapeFileName,
                                LPMOEEX_AnswerSheet = data.LPMOEEX_AnswerSheet,
                                LPMOEEX_AutoPublishFlg = data.LPMOEEX_AutoPublishFlg,
                                LPMOEEX_QuestionPapeFileName = data.LPMOEEX_QuestionPapeFileName,
                                LPMOEEX_QuestionPaper = data.LPMOEEX_QuestionPaper,
                                LPMOEEX_AllowDownloadQnsPaperBeforeExamFlg = data.LPMOEEX_AllowDownloadQnsPaperBeforeExamFlg,
                                LPMOEEX_Duration = data.LPMOEEX_Duration,
                                LPMOEEX_DurationFlag = data.LPMOEEX_DurationFlag,
                                LPMOEEX_ActiveFlg = true,
                                LPMOEEX_CreatedBy = data.Userid,
                                LPMOEEX_UpdatedBy = data.Userid,
                                LPMOEEX_CreatedDate = indiantime0,
                                LPMOEEX_UpdatedDate = indiantime0,
                                LPMOEEX_FromDateTime = data.LPMOEEX_FromDateTime.Value.AddHours(data.fhrors).AddMinutes(data.fminutes).AddSeconds(data.fsec),
                                LPMOEEX_ToDateTime = data.LPMOEEX_ToDateTime.Value.AddHours(data.thrors).AddMinutes(data.tminutes).AddSeconds(data.tsec),
                                LPMOEEX_NotLinkedToQnsBankFlg = data.LPMOEEX_NotLinkedToQnsBankFlg,
                                EME_Id = data.EME_Id > 0 ? data.EME_Id : null
                            };

                            _context.Add(lP_Master_OE_ExamDMO);

                            if (data.ExamLevelDetails != null && data.ExamLevelDetails.Length > 0)
                            {
                                foreach (var c in data.ExamLevelDetails)
                                {
                                    LP_Master_OE_Exam_LevelsDMO lP_Master_OE_Exam_LevelsDMO = new LP_Master_OE_Exam_LevelsDMO
                                    {
                                        LPMOEEX_Id = lP_Master_OE_ExamDMO.LPMOEEX_Id,
                                        LPMOEEXLVL_LevelDesc = c.LPMOEEXLVL_LevelDesc,
                                        LPMOEEXLVL_TotalNoOfQns = c.LPMOEEXLVL_TotalNoOfQns,
                                        LPMOEEXLVL_MaxQns = c.LPMOEEXLVL_MaxQns,
                                        LPMOEEXLVL_LevelTotalMarks = c.LPMOEEXLVL_LevelTotalMarks,
                                        LPMOEEXLVL_MarksPerQns = c.LPMOEEXLVL_MarksPerQns,
                                        LPMOEEXLVL_ActiveFlg = true,
                                        LPMOEEXLVL_CreatedBy = data.Userid,
                                        LPMOEEXLVL_CreatedDate = indiantime0,
                                        LPMOEEXLVL_UpdatedBy = data.Userid,
                                        LPMOEEXLVL_UpdatedDate = indiantime0,
                                        LPMOEEXLVL_LevelOrder = c.LPMOEEXLVL_LevelOrder
                                    };
                                    _context.Add(lP_Master_OE_Exam_LevelsDMO);

                                    foreach (var d in c.questionlist)
                                    {
                                        LP_Master_OE_Exam_QuestionsDMO lP_Master_OE_Exam_QuestionsDMO = new LP_Master_OE_Exam_QuestionsDMO
                                        {
                                            LPMOEEXLVL_Id = lP_Master_OE_Exam_LevelsDMO.LPMOEEXLVL_Id,
                                            LPMOEQ_Id = data.LPMOEEX_NotLinkedToQnsBankFlg == false ? d.LPMOEQ_Id : null,
                                            LPMOEEXQNS_Marks = d.LPMOEEXQNS_Marks,
                                            LPMOEEXQNS_ActiveFlg = true,
                                            LPMOEEXQNS_CreatedBy = data.Userid,
                                            LPMOEEXQNS_UpdatedBy = data.Userid,
                                            LPMOEEXQNS_CreatedDate = indiantime0,
                                            LPMOEEXQNS_UpdatedDate = indiantime0,
                                            LPMOEEXQNS_QnsOrder = d.LPMOEEXQNS_QnsOrder,

                                            LPMOEEXQNS_Question = data.LPMOEEX_NotLinkedToQnsBankFlg == true ? d.LPMOEEXQNS_Question : null,
                                            LPMOEEXQNS_SubjectiveFlg = data.LPMOEEX_NotLinkedToQnsBankFlg == true ? d.LPMOEEXQNS_SubjectiveFlg : null,
                                            LPMOEEXQNS_Answer = data.LPMOEEX_NotLinkedToQnsBankFlg == true ? d.LPMOEEXQNS_Answer : null,
                                            LPMOEEXQNS_MatchTheFollowingFlg = data.LPMOEEX_NotLinkedToQnsBankFlg == true ? d.LPMOEEXQNS_MatchTheFollowingFlg : null,
                                            LPMOEEXQNS_NoOfOptions = data.LPMOEEX_NotLinkedToQnsBankFlg == true ? d.LPMOEEXQNS_NoOfOptions : null,
                                            LPMOEEXQNS_NoOfRows = data.LPMOEEX_NotLinkedToQnsBankFlg == true ? d.LPMOEEXQNS_NoOfRows : null,
                                            LPMOEEXQNS_NoOfColumns = data.LPMOEEX_NotLinkedToQnsBankFlg == true ? d.LPMOEEXQNS_NoOfColumns : null,
                                            LPMOEEXQNS_QuestionType = data.LPMOEEX_NotLinkedToQnsBankFlg == true ? d.LPMOEEXQNS_QuestionType : null,

                                        };
                                        _context.Add(lP_Master_OE_Exam_QuestionsDMO);

                                        if (data.LPMOEEX_NotLinkedToQnsBankFlg == true)
                                        {
                                            // Adding Manual Questions Files
                                            if (d.Temp_Manual_Ques_Files != null && d.Temp_Manual_Ques_Files.Length > 0)
                                            {
                                                foreach (var ques_files in d.Temp_Manual_Ques_Files)
                                                {
                                                    LP_Master_OE_Exam_Questions_FilesDMO lP_Master_OE_Exam_Questions_FilesDMO = new LP_Master_OE_Exam_Questions_FilesDMO
                                                    {
                                                        LPMOEEXQNS_Id = lP_Master_OE_Exam_QuestionsDMO.LPMOEEXQNS_Id,
                                                        LPMOEEXQNSF_FileName = ques_files.LPMOEEXQNSF_FileName,
                                                        LPMOEEXQNSF_FilePath = ques_files.LPMOEEXQNSF_FilePath,
                                                        LPMOEEXQNSF_ActiveFlag = true,
                                                        LPMOEEXQNSF_CreatedDate = indiantime0,
                                                        LPMOEEXQNSF_UpdatedDate = indiantime0,
                                                        LPMOEEXQNSF_CreatedBy = data.Userid,
                                                        LPMOEEXQNSF_UpdatedBy = data.Userid
                                                    };
                                                    _context.Add(lP_Master_OE_Exam_Questions_FilesDMO);
                                                }
                                            }

                                            // Adding Manual Questions Wise Options
                                            if (d.LPMOEEXQNS_SubjectiveFlg != true && d.Temp_Manual_Ques_Options != null && d.Temp_Manual_Ques_Options.Length > 0)
                                            {
                                                foreach (var e in d.Temp_Manual_Ques_Options)
                                                {
                                                    LP_Master_OE_Exam_Questions_OptionsDMO lP_Master_OE_Exam_Questions_OptionsDMO =
                                                        new LP_Master_OE_Exam_Questions_OptionsDMO
                                                        {
                                                            LPMOEEXQNS_Id = lP_Master_OE_Exam_QuestionsDMO.LPMOEEXQNS_Id,
                                                            LPMOEEXQNSOPT_Option = e.LPMOEEXQNSOPT_Option,
                                                            LPMOEEXQNSOPT_OptionCode = e.LPMOEEXQNSOPT_OptionCode,
                                                            LPMOEEXQNSOPT_OptionImage = null,
                                                            LPMOEEXQNSOPT_AnswerFlag = e.LPMOEEXQNSOPT_AnswerFlag,
                                                            LPMOEEXQNSOPT_AnswerDesc = null,
                                                            LPMOEEXQNSOPT_ActiveFlg = true,
                                                            LPMOEEXQNSOPT_CreatedBy = data.Userid,
                                                            LPMOEEXQNSOPT_CreatedDate = indiantime0,
                                                            LPMOEEXQNSOPT_UpdatedBy = data.Userid,
                                                            LPMOEEXQNSOPT_UpdatedDate = indiantime0,
                                                            LPMOEEXQNSOPT_Marks = d.LPMOEEXQNS_MatchTheFollowingFlg == true ? e.LPMOEEXQNSOPT_Marks : null,
                                                        };
                                                    _context.Add(lP_Master_OE_Exam_Questions_OptionsDMO);

                                                    // Adding Manual Questions Option Files
                                                    if (e.Temp_Manual_Ques_Opts_Files != null && e.Temp_Manual_Ques_Opts_Files.Length > 0)
                                                    {
                                                        foreach (var ques_opts_files in e.Temp_Manual_Ques_Opts_Files)
                                                        {
                                                            LP_Master_OE_Exam_Questions_Options_FilesDMO lP_Master_OE_Exam_Questions_Options_FilesDMO = new LP_Master_OE_Exam_Questions_Options_FilesDMO
                                                            {
                                                                LPMOEEXQNSOPT_Id = lP_Master_OE_Exam_Questions_OptionsDMO.LPMOEEXQNSOPT_Id,
                                                                LPMOEEXQNSOPTF_FileName = ques_opts_files.LPMOEEXQNSOPTF_FileName,
                                                                LPMOEEXQNSOPTF_FilePath = ques_opts_files.LPMOEEXQNSOPTF_FilePath,
                                                                LPMOEEXQNSOPTF_ActiveFlag = true,
                                                                LPMOEEXQNSOPTF_CreatedDate = indiantime0,
                                                                LPMOEEXQNSOPTF_UpdatedDate = indiantime0,
                                                                LPMOEEXQNSOPTF_CreatedBy = data.Userid,
                                                                LPMOEEXQNSOPTF_UpdatedBy = data.Userid
                                                            };
                                                            _context.Add(lP_Master_OE_Exam_Questions_Options_FilesDMO);
                                                        }
                                                    }

                                                    // Adding Manual Questions Options Wise Match The Following
                                                    if (d.LPMOEEXQNS_MatchTheFollowingFlg == true && e.Temp_Manual_Ques_Options_Mf != null
                                                        && e.Temp_Manual_Ques_Options_Mf.Length > 0)
                                                    {
                                                        foreach (var f in e.Temp_Manual_Ques_Options_Mf)
                                                        {
                                                            LP_Master_OE_Exam_Questions_Options_MFDMO lP_Master_OE_Exam_Questions_Options_MFDMO =
                                                                new LP_Master_OE_Exam_Questions_Options_MFDMO
                                                                {
                                                                    LPMOEEXQNSOPT_Id = lP_Master_OE_Exam_Questions_OptionsDMO.LPMOEEXQNSOPT_Id,
                                                                    LPMOEEXQNSOPTMF_MatchtheFollowing = f.LPMOEEXQNSOPTMF_MatchtheFollowing,
                                                                    LPMOEEXQNSOPTMF_Answer_LPMOEEXQNSOPT = null,
                                                                    LPMOEEXQNSOPTMF_ActiveFlg = true,
                                                                    LPMOEEXQNSOPTMF_CreatedBy = data.Userid,
                                                                    LPMOEEXQNSOPTMF_CreatedDate = indiantime0,
                                                                    LPMOEEXQNSOPTMF_UpdatedBy = data.Userid,
                                                                    LPMOEEXQNSOPTMF_UpdatedDate = indiantime0,
                                                                    LPMOEEXQNSOPTMF_Answer_Flg = f.LPMOEEXQNSOPTMF_Answer_Flg,
                                                                    LPMOEEXQNSOPTMF_Order = f.LPMOEEXQNSOPTMF_Order
                                                                };

                                                            _context.Add(lP_Master_OE_Exam_Questions_Options_MFDMO);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            // Adding Topics For Exam
                            if (data.temptopicDTO != null && data.temptopicDTO.Length > 0)
                            {
                                foreach (var c in data.temptopicDTO)
                                {
                                    LP_Master_OE_Exam_TopicsDMO lP_Master_OE_Exam_TopicsDMO = new LP_Master_OE_Exam_TopicsDMO
                                    {
                                        LPMT_Id = c.LPMT_Id,
                                        LPMOEEX_Id = lP_Master_OE_ExamDMO.LPMOEEX_Id,
                                        LPMOEEXTOP_ActiveFlg = true,
                                        LPMOEEXQNS_UpdatedBy = data.Userid,
                                        LPMOEEXQNS_CreatedBy = data.Userid,
                                        LPMOEEXQNS_UpdatedDate = indiantime0,
                                        LPMOEEXQNS_CreatedDate = indiantime0
                                    };
                                    _context.Add(lP_Master_OE_Exam_TopicsDMO);
                                }
                            }
                        }
                        else
                        {
                            var getsectionname = _context.School_M_Section.Where(a => a.ASMS_Id == cd.ASMS_Id).FirstOrDefault().ASMC_SectionName;
                            if (message == "")
                            {
                                message = getsectionname;
                            }
                            else
                            {
                                message = message + " , " + getsectionname;
                            }
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

                    data.duplicatemessage = message;
                }
            }
            catch (Exception ex)
            {
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineExamDTO EditMasterExamQuestion(LP_OnlineExamDTO data)
        {
            try
            {
                var geteditdetails = _context.LP_Master_OE_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.LPMOEEX_Id == data.LPMOEEX_Id).ToList();

                data.geteditmasteroeexam = geteditdetails.ToArray();

                data.ASMAY_Id = geteditdetails.FirstOrDefault().ASMAY_Id;
                data.ASMCL_Id = geteditdetails.FirstOrDefault().ASMCL_Id;
                data.ASMS_Id = Convert.ToInt64(geteditdetails.FirstOrDefault().ASMS_Id);
                data.ISMS_Id = geteditdetails.FirstOrDefault().ISMS_Id;
                data.EME_Id = geteditdetails.FirstOrDefault().EME_Id;
                data.LPMOEEX_RandomFlg = geteditdetails.FirstOrDefault().LPMOEEX_RandomFlg;
                data.LPMOEEX_UploadExamPaperFlg = geteditdetails.FirstOrDefault().LPMOEEX_UploadExamPaperFlg;
                data.LPMOEEX_NotLinkedToQnsBankFlg = geteditdetails.FirstOrDefault().LPMOEEX_NotLinkedToQnsBankFlg;

                LP_OnlineExamDTO alldetails = new LP_OnlineExamDTO();
                // Get Class List Based On ASMAY_Id
                alldetails = getexamclasslist(data);
                var getclasslist = alldetails.getclasslist;

                // Get Section List Based On ASMAY_Id, ASMCL_Id
                data.editflag = "Edit";
                alldetails = getexamsectionslist(data);
                var getsectionlist = alldetails.getsectionlist;

                // Get Subject List , Exam List Based On ASMAY_Id, ASMCL_Id , ASMS_Id
                alldetails = getexamsubjectlist(data);
                var getsubjectlist = alldetails.getsubjectlist;
                var getmasterexamdetails = alldetails.getmasterexamdetails;

                // Get Totpic Based On ASMAY_Id, ASMCL_Id , ASMS_Id , ISMS_Id
                alldetails = GetSearchTopics(data);
                var gettopiclist = alldetails.gettopiclist;

                // Get Questoion Based On ASMAY_Id, ASMCL_Id , ASMS_Id , ISMS_Id
                alldetails = SearchQuestions(data);
                var getquestionlist = alldetails.getquestionlist;

                data.getclasslist = getclasslist;
                data.getsectionlist = getsectionlist;
                data.getsubjectlist = getsubjectlist;
                data.getmasterexamdetails = getmasterexamdetails;
                data.gettopiclist = gettopiclist;
                data.getquestionlist = getquestionlist;

                data.geteditexamtopiclist = _context.LP_Master_OE_Exam_TopicsDMO.Where(a => a.LPMOEEX_Id == data.LPMOEEX_Id).ToArray();

                data.getediteleveldetails = _context.LP_Master_OE_Exam_LevelsDMO.Where(a => a.LPMOEEX_Id == data.LPMOEEX_Id).ToArray();


                if (data.LPMOEEX_NotLinkedToQnsBankFlg == true)
                {
                    data.geteditelevelquestions = (from a in _context.LP_Master_OE_ExamDMO
                                                   from b in _context.LP_Master_OE_Exam_LevelsDMO
                                                   from c in _context.LP_Master_OE_Exam_QuestionsDMO
                                                   where (a.LPMOEEX_Id == b.LPMOEEX_Id && b.LPMOEEXLVL_Id == c.LPMOEEXLVL_Id
                                                   && a.LPMOEEX_Id == data.LPMOEEX_Id && b.LPMOEEX_Id == data.LPMOEEX_Id && a.LPMOEEX_NotLinkedToQnsBankFlg == true)
                                                   select new LP_OnlineExamDTO
                                                   {
                                                       LPMOEEXQNS_Question = c.LPMOEEXQNS_Question,
                                                       LPMOEEXQNS_SubjectiveFlg = c.LPMOEEXQNS_SubjectiveFlg,
                                                       LPMOEEXQNS_MatchTheFollowingFlg = c.LPMOEEXQNS_MatchTheFollowingFlg,
                                                       LPMOEEXQNS_NoOfOptions = c.LPMOEEXQNS_NoOfOptions,
                                                       LPMOEEXQNS_NoOfRows = c.LPMOEEXQNS_NoOfRows,
                                                       LPMOEEXQNS_NoOfColumns = c.LPMOEEXQNS_NoOfColumns,
                                                       LPMOEEXQNS_QuestionType = c.LPMOEEXQNS_QuestionType,
                                                       LPMOEEXLVL_Id = c.LPMOEEXLVL_Id,
                                                       LPMOEEXQNS_Id = c.LPMOEEXQNS_Id,
                                                       LPMOEEXQNS_QnsOrder = c.LPMOEEXQNS_QnsOrder,
                                                       LPMOEEXQNS_Marks = c.LPMOEEXQNS_Marks,
                                                   }).Distinct().ToArray();

                    data.geteditelevelquestionsfiles = (from a in _context.LP_Master_OE_ExamDMO
                                                        from b in _context.LP_Master_OE_Exam_LevelsDMO
                                                        from c in _context.LP_Master_OE_Exam_QuestionsDMO
                                                        from d in _context.LP_Master_OE_Exam_Questions_FilesDMO
                                                        where (a.LPMOEEX_Id == b.LPMOEEX_Id && b.LPMOEEXLVL_Id == c.LPMOEEXLVL_Id
                                                        && c.LPMOEEXQNS_Id == d.LPMOEEXQNS_Id && a.LPMOEEX_Id == data.LPMOEEX_Id && b.LPMOEEX_Id == data.LPMOEEX_Id
                                                        && a.LPMOEEX_NotLinkedToQnsBankFlg == true)
                                                        select new LP_OnlineExamDTO
                                                        {
                                                            LPMOEEXLVL_Id = c.LPMOEEXLVL_Id,
                                                            LPMOEEXQNS_Id = c.LPMOEEXQNS_Id,
                                                            LPMOEEXQNSF_Id = d.LPMOEEXQNSF_Id,
                                                            LPMOEEXQNSF_FileName = d.LPMOEEXQNSF_FileName,
                                                            LPMOEEXQNSF_FilePath = d.LPMOEEXQNSF_FilePath,
                                                            LPMOEEXQNSF_ActiveFlag = d.LPMOEEXQNSF_ActiveFlag,
                                                        }).Distinct().ToArray();

                    data.geteditelevelquestionsoptions = (from a in _context.LP_Master_OE_ExamDMO
                                                          from b in _context.LP_Master_OE_Exam_LevelsDMO
                                                          from c in _context.LP_Master_OE_Exam_QuestionsDMO
                                                          from d in _context.LP_Master_OE_Exam_Questions_OptionsDMO
                                                          where (a.LPMOEEX_Id == b.LPMOEEX_Id && b.LPMOEEXLVL_Id == c.LPMOEEXLVL_Id
                                                          && c.LPMOEEXQNS_Id == d.LPMOEEXQNS_Id && a.LPMOEEX_Id == data.LPMOEEX_Id
                                                          && b.LPMOEEX_Id == data.LPMOEEX_Id && a.LPMOEEX_NotLinkedToQnsBankFlg == true)
                                                          select new LP_OnlineExamDTO
                                                          {
                                                              LPMOEEXLVL_Id = c.LPMOEEXLVL_Id,
                                                              LPMOEEXQNS_Id = c.LPMOEEXQNS_Id,
                                                              LPMOEEXQNSOPT_Option = d.LPMOEEXQNSOPT_Option,
                                                              LPMOEEXQNSOPT_OptionCode = d.LPMOEEXQNSOPT_OptionCode,
                                                              LPMOEEXQNSOPT_OptionImage = d.LPMOEEXQNSOPT_OptionImage,
                                                              LPMOEEXQNSOPT_AnswerFlag = d.LPMOEEXQNSOPT_AnswerFlag,
                                                              LPMOEEXQNSOPT_AnswerDesc = d.LPMOEEXQNSOPT_AnswerDesc,
                                                              LPMOEEXQNSOPT_Marks = d.LPMOEEXQNSOPT_Marks,
                                                              LPMOEEXQNSOPT_ActiveFlg = d.LPMOEEXQNSOPT_ActiveFlg,
                                                              LPMOEEXQNSOPT_Id = d.LPMOEEXQNSOPT_Id,
                                                          }).Distinct().ToArray();

                    data.geteditelevelquestionsoptionsfiles = (from a in _context.LP_Master_OE_ExamDMO
                                                               from b in _context.LP_Master_OE_Exam_LevelsDMO
                                                               from c in _context.LP_Master_OE_Exam_QuestionsDMO
                                                               from d in _context.LP_Master_OE_Exam_Questions_OptionsDMO
                                                               from e in _context.LP_Master_OE_Exam_Questions_Options_FilesDMO
                                                               where (a.LPMOEEX_Id == b.LPMOEEX_Id && b.LPMOEEXLVL_Id == c.LPMOEEXLVL_Id
                                                               && c.LPMOEEXQNS_Id == d.LPMOEEXQNS_Id && d.LPMOEEXQNSOPT_Id == e.LPMOEEXQNSOPT_Id
                                                               && a.LPMOEEX_Id == data.LPMOEEX_Id && b.LPMOEEX_Id == data.LPMOEEX_Id
                                                               && a.LPMOEEX_NotLinkedToQnsBankFlg == true)
                                                               select new LP_OnlineExamDTO
                                                               {
                                                                   LPMOEEXLVL_Id = c.LPMOEEXLVL_Id,
                                                                   LPMOEEXQNS_Id = c.LPMOEEXQNS_Id,
                                                                   LPMOEEXQNSOPT_Id = d.LPMOEEXQNSOPT_Id,
                                                                   LPMOEEXQNSOPTF_Id = e.LPMOEEXQNSOPTF_Id,
                                                                   LPMOEEXQNSOPTF_FileName = e.LPMOEEXQNSOPTF_FileName,
                                                                   LPMOEEXQNSOPTF_FilePath = e.LPMOEEXQNSOPTF_FilePath,
                                                                   LPMOEEXQNSOPTF_ActiveFlag = e.LPMOEEXQNSOPTF_ActiveFlag,
                                                               }).Distinct().ToArray();

                    data.geteditelevelquestionsoptionsmf = (from a in _context.LP_Master_OE_ExamDMO
                                                            from b in _context.LP_Master_OE_Exam_LevelsDMO
                                                            from c in _context.LP_Master_OE_Exam_QuestionsDMO
                                                            from d in _context.LP_Master_OE_Exam_Questions_OptionsDMO
                                                            from e in _context.LP_Master_OE_Exam_Questions_Options_MFDMO
                                                            where (a.LPMOEEX_Id == b.LPMOEEX_Id && b.LPMOEEXLVL_Id == c.LPMOEEXLVL_Id
                                                            && c.LPMOEEXQNS_Id == d.LPMOEEXQNS_Id && d.LPMOEEXQNSOPT_Id == e.LPMOEEXQNSOPT_Id
                                                            && a.LPMOEEX_Id == data.LPMOEEX_Id && b.LPMOEEX_Id == data.LPMOEEX_Id
                                                            && a.LPMOEEX_NotLinkedToQnsBankFlg == true)
                                                            select new LP_OnlineExamDTO
                                                            {
                                                                LPMOEEXLVL_Id = c.LPMOEEXLVL_Id,
                                                                LPMOEEXQNS_Id = c.LPMOEEXQNS_Id,
                                                                LPMOEEXQNSOPT_Id = d.LPMOEEXQNSOPT_Id,
                                                                LPMOEEXQNSOPTMF_Id = e.LPMOEEXQNSOPTMF_Id,
                                                                LPMOEEXQNSOPTMF_MatchtheFollowing = e.LPMOEEXQNSOPTMF_MatchtheFollowing,
                                                                LPMOEEXQNSOPTMF_Answer_LPMOEEXQNSOPT = e.LPMOEEXQNSOPTMF_Answer_LPMOEEXQNSOPT,
                                                                LPMOEEXQNSOPTMF_ActiveFlg = e.LPMOEEXQNSOPTMF_ActiveFlg,
                                                                LPMOEEXQNSOPTMF_Answer_Flg = e.LPMOEEXQNSOPTMF_Answer_Flg,
                                                                LPMOEEXQNSOPTMF_Order = e.LPMOEEXQNSOPTMF_Order,
                                                            }).Distinct().OrderBy(a => a.LPMOEEXQNSOPTMF_Order).ToArray();
                }
                else
                {
                    data.geteditelevelquestions = (from a in _context.LP_Master_OE_ExamDMO
                                                   from b in _context.LP_Master_OE_Exam_LevelsDMO
                                                   from c in _context.LP_Master_OE_Exam_QuestionsDMO
                                                   from d in _context.LP_Master_OE_QuestionsDMO
                                                   where (a.LPMOEEX_Id == b.LPMOEEX_Id && b.LPMOEEXLVL_Id == c.LPMOEEXLVL_Id && c.LPMOEQ_Id == d.LPMOEQ_Id
                                                   && a.LPMOEEX_Id == data.LPMOEEX_Id && b.LPMOEEX_Id == data.LPMOEEX_Id)
                                                   select new LP_OnlineExamDTO
                                                   {
                                                       LPMOEQ_Question = d.LPMOEQ_Question,
                                                       LPMOEQ_QuestionDesc = d.LPMOEQ_QuestionDesc,
                                                       LPMOEQ_Id = d.LPMOEQ_Id,
                                                       LPMOEQ_StructuralFlg = d.LPMOEQ_StructuralFlg,
                                                       LPMOEEXLVL_Id = c.LPMOEEXLVL_Id,
                                                       LPMOEEXQNS_Id = c.LPMOEEXQNS_Id,
                                                       LPMOEEXQNS_QnsOrder = c.LPMOEEXQNS_QnsOrder,
                                                       LPMOEEXQNS_Marks = c.LPMOEEXQNS_Marks,
                                                   }).Distinct().ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineExamDTO ViewMasterExamQuesOptions(LP_OnlineExamDTO data)
        {
            try
            {
                data.getviewexamquestiondetails = (from a in _context.LP_Master_OE_Exam_QuestionsDMO
                                                   from b in _context.LP_Master_OE_QuestionsDMO
                                                   from c in _context.LP_Master_OE_ExamDMO
                                                   from d in _context.LP_Master_OE_Exam_LevelsDMO
                                                   where (d.LPMOEEX_Id == c.LPMOEEX_Id && a.LPMOEEXLVL_Id == d.LPMOEEXLVL_Id && a.LPMOEQ_Id == b.LPMOEQ_Id
                                                   && d.LPMOEEX_Id == data.LPMOEEX_Id
                                                   && b.MI_Id == data.MI_Id)
                                                   select new LP_OnlineExamDTO
                                                   {
                                                       LPMOEQ_Id = a.LPMOEQ_Id,
                                                       LPMOEEXQNS_Id = a.LPMOEEXQNS_Id,
                                                       LPMOEEX_Id = d.LPMOEEX_Id,
                                                       LPMOEEXQNS_ActiveFlg = a.LPMOEEXQNS_ActiveFlg,
                                                       LPMOEQ_Question = b.LPMOEQ_Question,
                                                       LPMOEEXQNS_Marks = a.LPMOEEXQNS_Marks,
                                                   }).Distinct().ToArray();

                data.getexamhappenedcount = _context.LP_Students_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.LPMOEEX_Id == data.LPMOEEX_Id).Count();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineExamDTO ViewMasterExamLevelDetails(LP_OnlineExamDTO data)
        {
            try
            {
                data.getarrayofleveldetails = _context.LP_Master_OE_Exam_LevelsDMO.Where(a => a.LPMOEEX_Id == data.LPMOEEX_Id).OrderBy(a => a.LPMOEEXLVL_LevelOrder).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineExamDTO ViewSavedLevelQuestons(LP_OnlineExamDTO data)
        {
            try
            {
                var getexamid = _context.LP_Master_OE_Exam_LevelsDMO.Where(a => a.LPMOEEXLVL_Id == data.LPMOEEXLVL_Id).ToList();

                var getexamdetails = _context.LP_Master_OE_ExamDMO.Where(a => a.LPMOEEX_Id == getexamid.FirstOrDefault().LPMOEEX_Id).ToList();

                data.LPMOEEX_NotLinkedToQnsBankFlg = getexamdetails.FirstOrDefault().LPMOEEX_NotLinkedToQnsBankFlg;
                data.LPMOEEX_Id = getexamdetails.FirstOrDefault().LPMOEEX_Id;

                if (data.LPMOEEX_NotLinkedToQnsBankFlg == true)
                {
                    data.getarrayoflevelquestiondetails = (from a in _context.LP_Master_OE_ExamDMO
                                                           from b in _context.LP_Master_OE_Exam_LevelsDMO
                                                           from c in _context.LP_Master_OE_Exam_QuestionsDMO
                                                           where (a.LPMOEEX_Id == b.LPMOEEX_Id && b.LPMOEEXLVL_Id == c.LPMOEEXLVL_Id
                                                           && a.LPMOEEX_Id == data.LPMOEEX_Id && b.LPMOEEX_Id == data.LPMOEEX_Id
                                                           && a.LPMOEEX_NotLinkedToQnsBankFlg == true && b.LPMOEEXLVL_Id == data.LPMOEEXLVL_Id)
                                                           select new LP_OnlineExamDTO
                                                           {
                                                               LPMOEEXQNS_Question = c.LPMOEEXQNS_Question,
                                                               LPMOEEXQNS_SubjectiveFlg = c.LPMOEEXQNS_SubjectiveFlg,
                                                               LPMOEEXQNS_Answer = c.LPMOEEXQNS_Answer,
                                                               LPMOEEXQNS_MatchTheFollowingFlg = c.LPMOEEXQNS_MatchTheFollowingFlg,
                                                               LPMOEEXQNS_NoOfOptions = c.LPMOEEXQNS_NoOfOptions,
                                                               LPMOEEXQNS_NoOfRows = c.LPMOEEXQNS_NoOfRows,
                                                               LPMOEEXQNS_NoOfColumns = c.LPMOEEXQNS_NoOfColumns,
                                                               LPMOEEXQNS_QuestionType = c.LPMOEEXQNS_QuestionType,
                                                               LPMOEEXLVL_Id = c.LPMOEEXLVL_Id,
                                                               LPMOEEXQNS_Id = c.LPMOEEXQNS_Id,
                                                               LPMOEEXQNS_QnsOrder = c.LPMOEEXQNS_QnsOrder,
                                                               LPMOEEXQNS_Marks = c.LPMOEEXQNS_Marks,
                                                           }).Distinct().ToArray();


                    data.getarrayoflevelquestiondetailsfiles = (from a in _context.LP_Master_OE_ExamDMO
                                                                from b in _context.LP_Master_OE_Exam_LevelsDMO
                                                                from c in _context.LP_Master_OE_Exam_QuestionsDMO
                                                                from d in _context.LP_Master_OE_Exam_Questions_FilesDMO
                                                                where (a.LPMOEEX_Id == b.LPMOEEX_Id && b.LPMOEEXLVL_Id == c.LPMOEEXLVL_Id
                                                                && c.LPMOEEXQNS_Id == d.LPMOEEXQNS_Id && a.LPMOEEX_Id == data.LPMOEEX_Id
                                                                && b.LPMOEEX_Id == data.LPMOEEX_Id && a.LPMOEEX_NotLinkedToQnsBankFlg == true
                                                                && b.LPMOEEXLVL_Id == data.LPMOEEXLVL_Id)
                                                                select new LP_OnlineExamDTO
                                                                {
                                                                    LPMOEEXLVL_Id = c.LPMOEEXLVL_Id,
                                                                    LPMOEEXQNS_Id = c.LPMOEEXQNS_Id,
                                                                    LPMOEEXQNSF_Id = d.LPMOEEXQNSF_Id,
                                                                    LPMOEEXQNSF_FileName = d.LPMOEEXQNSF_FileName,
                                                                    LPMOEEXQNSF_FilePath = d.LPMOEEXQNSF_FilePath,
                                                                    LPMOEEXQNSF_ActiveFlag = d.LPMOEEXQNSF_ActiveFlag,
                                                                }).Distinct().ToArray();


                    data.getarrayoflevelquestionoptiondetails = (from a in _context.LP_Master_OE_ExamDMO
                                                                 from b in _context.LP_Master_OE_Exam_LevelsDMO
                                                                 from c in _context.LP_Master_OE_Exam_QuestionsDMO
                                                                 from d in _context.LP_Master_OE_Exam_Questions_OptionsDMO
                                                                 where (a.LPMOEEX_Id == b.LPMOEEX_Id && b.LPMOEEXLVL_Id == c.LPMOEEXLVL_Id
                                                                 && c.LPMOEEXQNS_Id == d.LPMOEEXQNS_Id && a.LPMOEEX_Id == data.LPMOEEX_Id
                                                                 && b.LPMOEEX_Id == data.LPMOEEX_Id && a.LPMOEEX_NotLinkedToQnsBankFlg == true
                                                                  && b.LPMOEEXLVL_Id == data.LPMOEEXLVL_Id)
                                                                 select new LP_OnlineExamDTO
                                                                 {
                                                                     LPMOEEXLVL_Id = c.LPMOEEXLVL_Id,
                                                                     LPMOEEXQNS_Id = c.LPMOEEXQNS_Id,
                                                                     LPMOEEXQNSOPT_Option = d.LPMOEEXQNSOPT_Option,
                                                                     LPMOEEXQNSOPT_OptionCode = d.LPMOEEXQNSOPT_OptionCode,
                                                                     LPMOEEXQNSOPT_OptionImage = d.LPMOEEXQNSOPT_OptionImage,
                                                                     LPMOEEXQNSOPT_AnswerFlag = d.LPMOEEXQNSOPT_AnswerFlag,
                                                                     LPMOEEXQNSOPT_AnswerDesc = d.LPMOEEXQNSOPT_AnswerDesc,
                                                                     LPMOEEXQNSOPT_Marks = d.LPMOEEXQNSOPT_Marks,
                                                                     LPMOEEXQNSOPT_ActiveFlg = d.LPMOEEXQNSOPT_ActiveFlg,
                                                                     LPMOEEXQNSOPT_Id = d.LPMOEEXQNSOPT_Id,
                                                                 }).Distinct().ToArray();


                    data.getarrayoflevelquestionoptiondetailsfiles = (from a in _context.LP_Master_OE_ExamDMO
                                                                      from b in _context.LP_Master_OE_Exam_LevelsDMO
                                                                      from c in _context.LP_Master_OE_Exam_QuestionsDMO
                                                                      from d in _context.LP_Master_OE_Exam_Questions_OptionsDMO
                                                                      from e in _context.LP_Master_OE_Exam_Questions_Options_FilesDMO
                                                                      where (a.LPMOEEX_Id == b.LPMOEEX_Id && b.LPMOEEXLVL_Id == c.LPMOEEXLVL_Id
                                                                      && c.LPMOEEXQNS_Id == d.LPMOEEXQNS_Id && d.LPMOEEXQNSOPT_Id == e.LPMOEEXQNSOPT_Id
                                                                      && a.LPMOEEX_Id == data.LPMOEEX_Id && b.LPMOEEX_Id == data.LPMOEEX_Id
                                                                      && a.LPMOEEX_NotLinkedToQnsBankFlg == true && b.LPMOEEXLVL_Id == data.LPMOEEXLVL_Id)
                                                                      select new LP_OnlineExamDTO
                                                                      {
                                                                          LPMOEEXLVL_Id = c.LPMOEEXLVL_Id,
                                                                          LPMOEEXQNS_Id = c.LPMOEEXQNS_Id,
                                                                          LPMOEEXQNSOPT_Id = d.LPMOEEXQNSOPT_Id,
                                                                          LPMOEEXQNSOPTF_Id = e.LPMOEEXQNSOPTF_Id,
                                                                          LPMOEEXQNSOPTF_FileName = e.LPMOEEXQNSOPTF_FileName,
                                                                          LPMOEEXQNSOPTF_FilePath = e.LPMOEEXQNSOPTF_FilePath,
                                                                          LPMOEEXQNSOPTF_ActiveFlag = e.LPMOEEXQNSOPTF_ActiveFlag,
                                                                      }).Distinct().ToArray();


                    data.getarrayoflevelquestionoptionmfdetails = (from a in _context.LP_Master_OE_ExamDMO
                                                                   from b in _context.LP_Master_OE_Exam_LevelsDMO
                                                                   from c in _context.LP_Master_OE_Exam_QuestionsDMO
                                                                   from d in _context.LP_Master_OE_Exam_Questions_OptionsDMO
                                                                   from e in _context.LP_Master_OE_Exam_Questions_Options_MFDMO
                                                                   where (a.LPMOEEX_Id == b.LPMOEEX_Id && b.LPMOEEXLVL_Id == c.LPMOEEXLVL_Id
                                                                   && c.LPMOEEXQNS_Id == d.LPMOEEXQNS_Id && d.LPMOEEXQNSOPT_Id == e.LPMOEEXQNSOPT_Id
                                                                   && a.LPMOEEX_Id == data.LPMOEEX_Id && b.LPMOEEX_Id == data.LPMOEEX_Id
                                                                   && a.LPMOEEX_NotLinkedToQnsBankFlg == true && b.LPMOEEXLVL_Id == data.LPMOEEXLVL_Id)
                                                                   select new LP_OnlineExamDTO
                                                                   {
                                                                       LPMOEEXLVL_Id = c.LPMOEEXLVL_Id,
                                                                       LPMOEEXQNS_Id = c.LPMOEEXQNS_Id,
                                                                       LPMOEEXQNSOPT_Id = d.LPMOEEXQNSOPT_Id,
                                                                       LPMOEEXQNSOPTMF_Id = e.LPMOEEXQNSOPTMF_Id,
                                                                       LPMOEEXQNSOPTMF_MatchtheFollowing = e.LPMOEEXQNSOPTMF_MatchtheFollowing,
                                                                       LPMOEEXQNSOPTMF_Answer_LPMOEEXQNSOPT = e.LPMOEEXQNSOPTMF_Answer_LPMOEEXQNSOPT,
                                                                       LPMOEEXQNSOPTMF_ActiveFlg = e.LPMOEEXQNSOPTMF_ActiveFlg,
                                                                       LPMOEEXQNSOPTMF_Answer_Flg = e.LPMOEEXQNSOPTMF_Answer_Flg,
                                                                       LPMOEEXQNSOPTMF_Order = e.LPMOEEXQNSOPTMF_Order,
                                                                   }).Distinct().OrderBy(a => a.LPMOEEXQNSOPTMF_Order).ToArray();
                }
                else
                {
                    data.getarrayoflevelquestiondetails = (from a in _context.LP_Master_OE_Exam_LevelsDMO
                                                           from b in _context.LP_Master_OE_Exam_QuestionsDMO
                                                           from c in _context.LP_Master_OE_QuestionsDMO
                                                           where (a.LPMOEEXLVL_Id == b.LPMOEEXLVL_Id && b.LPMOEQ_Id == c.LPMOEQ_Id
                                                           && a.LPMOEEXLVL_Id == data.LPMOEEXLVL_Id && b.LPMOEEXLVL_Id == data.LPMOEEXLVL_Id)
                                                           select new LP_OnlineExamDTO
                                                           {
                                                               LPMOEQ_Id = b.LPMOEQ_Id,
                                                               LPMOEEXQNS_Id = b.LPMOEEXQNS_Id,
                                                               LPMOEEX_Id = a.LPMOEEX_Id,
                                                               LPMOEEXQNS_ActiveFlg = b.LPMOEEXQNS_ActiveFlg,
                                                               LPMOEQ_Question = c.LPMOEQ_Question,
                                                               LPMOEEXQNS_Marks = b.LPMOEEXQNS_Marks,
                                                               LPMOEEXQNS_QnsOrder = b.LPMOEEXQNS_QnsOrder
                                                           }).Distinct().OrderBy(a => a.LPMOEEXQNS_QnsOrder).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineExamDTO ViewMasterQuestionExamTopic(LP_OnlineExamDTO data)
        {
            try
            {
                data.getviewexamquestiontopicdetails = (from c in _context.LP_Master_OE_ExamDMO
                                                        from d in _context.LP_Master_OE_Exam_TopicsDMO
                                                        from e in _context.SchoolSubjectWithMasterTopicMapping
                                                        where (c.LPMOEEX_Id == d.LPMOEEX_Id && d.LPMT_Id == e.LPMT_Id && c.LPMOEEX_Id == data.LPMOEEX_Id
                                                        && c.MI_Id == data.MI_Id)
                                                        select new LP_OnlineExamDTO
                                                        {
                                                            LPMOEEX_Id = c.LPMOEEX_Id,
                                                            LPMOEEXTOP_Id = d.LPMOEEXTOP_Id,
                                                            LPMOEEXTOP_ActiveFlg = d.LPMOEEXTOP_ActiveFlg,
                                                            LPMT_TopicName = e.LPMT_TopicName,
                                                            CreatedDate = d.LPMOEEXQNS_CreatedDate
                                                        }).Distinct().OrderBy(a => a.CreatedDate).ToArray();

                data.getexamhappenedcount = _context.LP_Students_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.LPMOEEX_Id == data.LPMOEEX_Id).Count();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineExamDTO ViewQuestionPaper(LP_OnlineExamDTO data)
        {
            try
            {
                var getexamdetails = _context.LP_Master_OE_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.LPMOEEX_Id == data.LPMOEEX_Id).ToList();
                data.getexamdetails = getexamdetails.ToArray();

                if (getexamdetails.Count > 0)
                {
                    var getrandomflag = getexamdetails.FirstOrDefault().LPMOEEX_RandomFlg;
                    var getnoofquestion = getexamdetails.FirstOrDefault().LPMOEEX_NoOfQuestion;
                    var getuploadflag = getexamdetails.FirstOrDefault().LPMOEEX_UploadExamPaperFlg;
                    var LPMOEEX_NotLinkedToQnsBankFlg = getexamdetails.FirstOrDefault().LPMOEEX_NotLinkedToQnsBankFlg;

                    if (getuploadflag == false)
                    {
                        data.getexamleveldetails = _context.LP_Master_OE_Exam_LevelsDMO.Where(a => a.LPMOEEX_Id == data.LPMOEEX_Id).OrderBy(a => a.LPMOEEXLVL_LevelOrder).ToArray();

                        if (LPMOEEX_NotLinkedToQnsBankFlg == true)
                        {
                            var getexamquestionlist = (from a in _context.LP_Master_OE_Exam_QuestionsDMO
                                                       from b in _context.LP_Master_OE_ExamDMO
                                                       from d in _context.LP_Master_OE_Exam_LevelsDMO
                                                       where (d.LPMOEEX_Id == b.LPMOEEX_Id && d.LPMOEEXLVL_Id == a.LPMOEEXLVL_Id
                                                       && a.LPMOEEXQNS_ActiveFlg == true && d.LPMOEEX_Id == data.LPMOEEX_Id && b.LPMOEEX_Id == data.LPMOEEX_Id)
                                                       select new LP_OnlineExamDTO
                                                       {
                                                           LPMOEQ_Id = a.LPMOEEXQNS_Id,
                                                           LPMOEQ_Question = a.LPMOEEXQNS_Question,
                                                           LPMOEEXQNS_Id = a.LPMOEEXQNS_Id,
                                                           LPMOEQ_SubjectiveFlg = a.LPMOEEXQNS_SubjectiveFlg,
                                                           LPMOEEXQNS_Answer = a.LPMOEEXQNS_Answer,
                                                           LPMOEQ_MatchTheFollowingFlg = a.LPMOEEXQNS_MatchTheFollowingFlg,
                                                           LPMOEEXQNS_QnsOrder = a.LPMOEEXQNS_QnsOrder,
                                                           LPMOEEXLVL_Id = a.LPMOEEXLVL_Id,
                                                           LPMOEQ_StructuralFlg = a.LPMOEEXQNS_QuestionType
                                                       }).Distinct().OrderBy(a => a.LPMOEEXQNS_QnsOrder).ToList();

                            List<long?> questionids = new List<long?>();

                            foreach (var c in getexamquestionlist)
                            {
                                questionids.Add(c.LPMOEEXQNS_Id);
                            }

                            data.getexamquestionlist = getexamquestionlist.ToArray();


                            data.getquestiondoclist = (from a in _context.LP_Master_OE_Exam_QuestionsDMO
                                                       from b in _context.LP_Master_OE_Exam_Questions_FilesDMO
                                                       where (a.LPMOEEXQNS_Id == b.LPMOEEXQNS_Id && a.LPMOEEXQNS_ActiveFlg == true && b.LPMOEEXQNSF_ActiveFlag == true
                                                        && questionids.Contains(b.LPMOEEXQNS_Id))
                                                       select new LP_OnlineStudentExamDTO
                                                       {
                                                           LPMOEQ_Id = a.LPMOEEXQNS_Id,
                                                           LPMOEQF_FileName = b.LPMOEEXQNSF_FileName,
                                                           LPMOEQF_FilePath = b.LPMOEEXQNSF_FilePath
                                                       }).Distinct().ToArray();

                            data.getquestionoptionlist = (from a in _context.LP_Master_OE_Exam_QuestionsDMO
                                                          from b in _context.LP_Master_OE_ExamDMO
                                                          from d in _context.LP_Master_OE_Exam_LevelsDMO
                                                          from e in _context.LP_Master_OE_Exam_Questions_OptionsDMO
                                                          where (d.LPMOEEX_Id == b.LPMOEEX_Id && d.LPMOEEXLVL_Id == a.LPMOEEXLVL_Id
                                                          && a.LPMOEEXQNS_Id == e.LPMOEEXQNS_Id && a.LPMOEEXQNS_ActiveFlg == true
                                                          && d.LPMOEEX_Id == data.LPMOEEX_Id && b.LPMOEEX_Id == data.LPMOEEX_Id
                                                          && questionids.Contains(e.LPMOEEXQNS_Id))
                                                          select new LP_OnlineExamDTO
                                                          {
                                                              LPMOEQ_Id = a.LPMOEEXQNS_Id,
                                                              LPMOEQOA_Id = e.LPMOEEXQNSOPT_Id,
                                                              LPMOEQOA_Option = e.LPMOEEXQNSOPT_Option,
                                                              LPMOEQOA_OptionCode = e.LPMOEEXQNSOPT_OptionCode,
                                                              LPMOEQOA_AnswerFlag = e.LPMOEEXQNSOPT_AnswerFlag,
                                                          }).Distinct().OrderBy(a => a.LPMOEQ_Id).ToArray();

                            data.getoptionwisefiles = (from a in _context.LP_Master_OE_Exam_QuestionsDMO
                                                       from c in _context.LP_Master_OE_Exam_Questions_OptionsDMO
                                                       from d in _context.LP_Master_OE_Exam_Questions_Options_FilesDMO
                                                       where (a.LPMOEEXQNS_Id == c.LPMOEEXQNS_Id && c.LPMOEEXQNSOPT_Id == d.LPMOEEXQNSOPT_Id
                                                       && a.LPMOEEXQNS_ActiveFlg == true && c.LPMOEEXQNSOPT_ActiveFlg == true && questionids.Contains(c.LPMOEEXQNS_Id))
                                                       select new LP_OnlineStudentExamDTO
                                                       {
                                                           LPMOEQOA_Id = c.LPMOEEXQNSOPT_Id,
                                                           LPMOEQOAF_FileName = d.LPMOEEXQNSOPTF_FileName,
                                                           LPMOEQOAF_FilePath = d.LPMOEEXQNSOPTF_FilePath
                                                       }).Distinct().ToArray();

                            data.getquestionmfoptionlist = (from a in _context.LP_Master_OE_Exam_QuestionsDMO
                                                            from b in _context.LP_Master_OE_ExamDMO
                                                            from d in _context.LP_Master_OE_Exam_LevelsDMO
                                                            from e in _context.LP_Master_OE_Exam_Questions_OptionsDMO
                                                            from f in _context.LP_Master_OE_Exam_Questions_Options_MFDMO
                                                            where (d.LPMOEEX_Id == b.LPMOEEX_Id && d.LPMOEEXLVL_Id == a.LPMOEEXLVL_Id
                                                            && a.LPMOEEXQNS_Id == e.LPMOEEXQNS_Id && a.LPMOEEXQNS_ActiveFlg == true
                                                            && d.LPMOEEX_Id == data.LPMOEEX_Id && b.LPMOEEX_Id == data.LPMOEEX_Id
                                                            && e.LPMOEEXQNSOPT_Id == f.LPMOEEXQNSOPT_Id && a.LPMOEEXQNS_MatchTheFollowingFlg == true
                                                            && questionids.Contains(e.LPMOEEXQNS_Id))
                                                            select new LP_OnlineExamDTO
                                                            {
                                                                LPMOEQ_Id = a.LPMOEEXQNS_Id,
                                                                LPMOEQOA_Id = e.LPMOEEXQNSOPT_Id,
                                                                LPMOEQOAMF_Id = f.LPMOEEXQNSOPTMF_Id,
                                                                LPMOEQOAMF_MatchtheFollowing = f.LPMOEEXQNSOPTMF_MatchtheFollowing,
                                                                LPMOEQOAMF_AnswerFlag = f.LPMOEEXQNSOPTMF_Answer_Flg,
                                                                LPMOEQOAMF_Order = f.LPMOEEXQNSOPTMF_Order,
                                                            }).Distinct().OrderBy(a => a.LPMOEQOAMF_Order).ToArray();
                        }
                        else
                        {
                            var getexamquestionlist = (from a in _context.LP_Master_OE_Exam_QuestionsDMO
                                                       from b in _context.LP_Master_OE_ExamDMO
                                                       from c in _context.LP_Master_OE_QuestionsDMO
                                                       from d in _context.LP_Master_OE_Exam_LevelsDMO
                                                       where (d.LPMOEEX_Id == b.LPMOEEX_Id && d.LPMOEEXLVL_Id == a.LPMOEEXLVL_Id && a.LPMOEQ_Id == c.LPMOEQ_Id
                                                       && a.LPMOEEXQNS_ActiveFlg == true && d.LPMOEEX_Id == data.LPMOEEX_Id && b.LPMOEEX_Id == data.LPMOEEX_Id)
                                                       select new LP_OnlineExamDTO
                                                       {
                                                           LPMOEQ_Id = a.LPMOEQ_Id,
                                                           LPMOEQ_Question = c.LPMOEQ_Question,
                                                           LPMOEEXQNS_Id = a.LPMOEEXQNS_Id,
                                                           LPMOEQ_SubjectiveFlg = c.LPMOEQ_SubjectiveFlg,
                                                           LPMOEQ_MatchTheFollowingFlg = c.LPMOEQ_MatchTheFollowingFlg,
                                                           LPMOEEXQNS_QnsOrder = a.LPMOEEXQNS_QnsOrder,
                                                           LPMOEEXLVL_Id = a.LPMOEEXLVL_Id,
                                                           LPMOEQ_StructuralFlg = c.LPMOEQ_StructuralFlg
                                                       }).Distinct().OrderBy(a => a.LPMOEEXQNS_QnsOrder).ToList();

                            List<long?> questionids = new List<long?>();

                            foreach (var c in getexamquestionlist)
                            {
                                questionids.Add(c.LPMOEQ_Id);
                            }

                            data.getexamquestionlist = getexamquestionlist.ToArray();

                            data.getquestionoptionlist = (from a in _context.LP_Master_OE_QuestionsDMO
                                                          from b in _context.LP_Master_OE_QNS_OptionsDMO
                                                          where (a.LPMOEQ_Id == b.LPMOEQ_Id && a.LPMOEQ_ActiveFlg == true && b.LPMOEQOA_ActiveFlg == true
                                                           && questionids.Contains(b.LPMOEQ_Id) && a.MI_Id == data.MI_Id)
                                                          select new LP_OnlineStudentExamDTO
                                                          {
                                                              LPMOEQ_Id = a.LPMOEQ_Id,
                                                              LPMOEQOA_Id = b.LPMOEQOA_Id,
                                                              LPMOEQOA_Option = b.LPMOEQOA_Option,
                                                              LPMOEQOA_OptionCode = b.LPMOEQOA_OptionCode,
                                                              LPMOEQOA_AnswerFlag = b.LPMOEQOA_AnswerFlag,
                                                          }).Distinct().OrderBy(a => a.LPMOEQ_Id).ToArray();

                            data.getquestionmfoptionlist = (from a in _context.LP_Master_OE_QuestionsDMO
                                                            from b in _context.LP_Master_OE_QNS_OptionsDMO
                                                            from c in _context.LP_Master_OE_QNS_Options_MFDMO
                                                            where (a.LPMOEQ_Id == b.LPMOEQ_Id && b.LPMOEQOA_Id == c.LPMOEQOA_Id
                                                            && a.LPMOEQ_ActiveFlg == true && b.LPMOEQOA_ActiveFlg == true
                                                            && questionids.Contains(a.LPMOEQ_Id) && a.MI_Id == data.MI_Id
                                                            && a.LPMOEQ_MatchTheFollowingFlg == true)
                                                            select new LP_OnlineStudentExamDTO
                                                            {
                                                                LPMOEQ_Id = a.LPMOEQ_Id,
                                                                LPMOEQOA_Id = b.LPMOEQOA_Id,
                                                                LPMOEQOAMF_Id = c.LPMOEQOAMF_Id,
                                                                LPMOEQOAMF_MatchtheFollowing = c.LPMOEQOAMF_MatchtheFollowing,
                                                                LPMOEQOAMF_AnswerFlag = c.LPMOEQOAMF_AnswerFlag,
                                                                LPMOEQOAMF_Order = c.LPMOEQOAMF_Order,

                                                            }).Distinct().OrderBy(a => a.LPMOEQOAMF_Order).ToArray();

                            data.getquestiondoclist = (from a in _context.LP_Master_OE_QuestionsDMO
                                                       from b in _context.LP_Master_OE_Questions_FilesDMO
                                                       where (a.LPMOEQ_Id == b.LPMOEQ_Id && a.LPMOEQ_ActiveFlg == true && b.LPMOEQF_ActiveFlag == true
                                                        && questionids.Contains(b.LPMOEQ_Id) && a.MI_Id == data.MI_Id)
                                                       select new LP_OnlineStudentExamDTO
                                                       {
                                                           LPMOEQ_Id = a.LPMOEQ_Id,
                                                           LPMOEQF_FileName = b.LPMOEQF_FileName,
                                                           LPMOEQF_FilePath = b.LPMOEQF_FilePath
                                                       }).Distinct().ToArray();


                            data.getoptionwisefiles = (from a in _context.LP_Master_OE_QuestionsDMO
                                                       from b in _context.LP_Master_OE_QNS_OptionsDMO
                                                       from c in _context.LP_Master_OE_QNS_Options_FilesDMO
                                                       where (a.LPMOEQ_Id == b.LPMOEQ_Id && b.LPMOEQOA_Id == c.LPMOEQOA_Id && a.LPMOEQ_ActiveFlg == true
                                                       && b.LPMOEQOA_ActiveFlg == true && c.LPMOEQOAF_ActiveFlag == true
                                                        && questionids.Contains(b.LPMOEQ_Id) && a.MI_Id == data.MI_Id)
                                                       select new LP_OnlineStudentExamDTO
                                                       {
                                                           LPMOEQOA_Id = b.LPMOEQOA_Id,
                                                           LPMOEQOAF_FileName = c.LPMOEQOAF_FileName,
                                                           LPMOEQOAF_FilePath = c.LPMOEQOAF_FilePath
                                                       }).Distinct().ToArray();
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
        public LP_OnlineExamDTO DeactivateActivateMasterExam(LP_OnlineExamDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var checkresult = _context.LP_Master_OE_ExamDMO.Single(a => a.MI_Id == data.MI_Id && a.LPMOEEX_Id == data.LPMOEEX_Id);
                data.ASMCL_Id = checkresult.ASMCL_Id;
                data.ASMAY_Id = checkresult.ASMAY_Id;
                data.ISMS_Id = checkresult.ISMS_Id;
                data.ASMS_Id = Convert.ToInt64(checkresult.ASMS_Id);
                data.LPMOEEX_ExamName = checkresult.LPMOEEX_ExamName;

                if (checkresult.LPMOEEX_ActiveFlg == false)
                {
                    var checkduplicate = _context.LP_Master_OE_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.ASMCL_Id == data.ASMCL_Id && a.ISMS_Id == data.ISMS_Id && a.LPMOEEX_ExamName.Equals(data.LPMOEEX_ExamName) && a.ASMS_Id == data.ASMS_Id
                    && a.LPMOEEX_ActiveFlg == true && a.LPMOEEX_Id != data.LPMOEEX_Id).ToList();

                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Mapped";
                    }
                    else
                    {
                        checkresult.LPMOEEX_ActiveFlg = true;
                        checkresult.LPMOEEX_UpdatedBy = data.Userid;
                        checkresult.LPMOEEX_UpdatedDate = indiantime0;
                        _context.Update(checkresult);
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
                else
                {
                    checkresult.LPMOEEX_ActiveFlg = checkresult.LPMOEEX_ActiveFlg == true ? false : true;
                    checkresult.LPMOEEX_UpdatedBy = data.Userid;
                    checkresult.LPMOEEX_UpdatedDate = indiantime0;
                    _context.Update(checkresult);
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
            }
            return data;
        }
        public LP_OnlineExamDTO DeactivateActivateExamQues(LP_OnlineExamDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var checkresult = _context.LP_Master_OE_Exam_QuestionsDMO.Single(a => a.LPMOEEXQNS_Id == data.LPMOEEXQNS_Id);

                if (checkresult.LPMOEEXQNS_ActiveFlg == true)
                {
                    var checkquestionmapped = _context.LP_Students_Exam_AnswerDMO.Where(a => a.LPMOEQ_Id == checkresult.LPMOEQ_Id).ToList();

                    if (checkquestionmapped.Count > 0)
                    {
                        data.message = "Mapped";
                    }
                    else
                    {
                        checkresult.LPMOEEXQNS_ActiveFlg = false;
                    }
                }
                else
                {
                    checkresult.LPMOEEXQNS_ActiveFlg = true;
                }

                if (data.message != "Mapped")
                {
                    checkresult.LPMOEEXQNS_UpdatedBy = data.Userid;
                    checkresult.LPMOEEXQNS_UpdatedDate = indiantime0;
                    _context.Update(checkresult);
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


                data.getviewexamquestiondetails = (from a in _context.LP_Master_OE_Exam_QuestionsDMO
                                                   from b in _context.LP_Master_OE_QuestionsDMO
                                                   from c in _context.LP_Master_OE_ExamDMO
                                                   from d in _context.LP_Master_OE_Exam_LevelsDMO
                                                   where (d.LPMOEEX_Id == c.LPMOEEX_Id && a.LPMOEEXLVL_Id == d.LPMOEEXLVL_Id && a.LPMOEQ_Id == b.LPMOEQ_Id
                                                   && d.LPMOEEX_Id == data.LPMOEEX_Id
                                                   && b.MI_Id == data.MI_Id)
                                                   select new LP_OnlineExamDTO
                                                   {
                                                       LPMOEQ_Id = a.LPMOEQ_Id,
                                                       LPMOEEXQNS_Id = a.LPMOEEXQNS_Id,
                                                       LPMOEEX_Id = d.LPMOEEX_Id,
                                                       LPMOEEXQNS_ActiveFlg = a.LPMOEEXQNS_ActiveFlg,
                                                       LPMOEQ_Question = b.LPMOEQ_Question,
                                                       LPMOEEXQNS_Marks = a.LPMOEEXQNS_Marks,
                                                   }).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineExamDTO DeactivateActivateExamQuesTopic(LP_OnlineExamDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var checkresult = _context.LP_Master_OE_Exam_TopicsDMO.Single(a => a.LPMOEEX_Id == data.LPMOEEX_Id && a.LPMOEEXTOP_Id == data.LPMOEEXTOP_Id);

                if (checkresult.LPMOEEXTOP_ActiveFlg == true)
                {

                    checkresult.LPMOEEXTOP_ActiveFlg = false;

                }
                else
                {
                    checkresult.LPMOEEXTOP_ActiveFlg = true;
                }

                checkresult.LPMOEEXQNS_UpdatedBy = data.Userid;
                checkresult.LPMOEEXQNS_UpdatedDate = indiantime0;
                _context.Update(checkresult);
                var i = _context.SaveChanges();
                if (i > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }



                data.getviewexamquestiontopicdetails = (from c in _context.LP_Master_OE_ExamDMO
                                                        from d in _context.LP_Master_OE_Exam_TopicsDMO
                                                        from e in _context.SchoolSubjectWithMasterTopicMapping
                                                        where (c.LPMOEEX_Id == d.LPMOEEX_Id && d.LPMT_Id == e.LPMT_Id && c.LPMOEEX_Id == data.LPMOEEX_Id
                                                        && c.MI_Id == data.MI_Id)
                                                        select new LP_OnlineExamDTO
                                                        {
                                                            LPMOEEX_Id = c.LPMOEEX_Id,
                                                            LPMOEEXTOP_Id = d.LPMOEEXTOP_Id,
                                                            LPMOEEXTOP_ActiveFlg = d.LPMOEEXTOP_ActiveFlg,
                                                            LPMT_TopicName = e.LPMT_TopicName,
                                                            CreatedDate = d.LPMOEEXQNS_CreatedDate
                                                        }).Distinct().OrderBy(a => a.CreatedDate).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineExamDTO SearchQuestionfilter(LP_OnlineExamDTO data)
        {
            try
            {
                List<long> sectionids = new List<long>();

                foreach (var c in data.sectiondetailslist)
                {
                    sectionids.Add(c.ASMS_Id);
                }

                List<long> ids = new List<long>();

                foreach (var c in data.temptopics)
                {
                    ids.Add(c.LPMT_Id);
                }

                List<long?> complexities_ids = new List<long?>();

                if (data.tempcomplexitites.Length > 0)
                {
                    foreach (var c in data.tempcomplexitites)
                    {
                        complexities_ids.Add(c.LPMCOMP_Id);
                    }
                }
                else
                {
                    var getcomplexities = _context.LP_Master_ComplexitiesDMO.ToList();

                    foreach (var c in getcomplexities)
                    {
                        complexities_ids.Add(c.LPMCOMP_Id);
                    }
                }

                List<LP_Master_OE_QuestionsDMO> LP_Master_OE_QuestionsDMO = new List<LP_Master_OE_QuestionsDMO>();

                if (data.LPMOEEX_RandomFlg == false)
                {
                    if (data.subjectiveflag == true)
                    {
                        data.getquestionlist = _context.LP_Master_OE_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                        && a.ISMS_Id == data.ISMS_Id && a.LPMOEQ_ActiveFlg == true && ids.Contains(a.LPMT_Id)
                        && a.LPMCOMP_Id != null && complexities_ids.Contains(a.LPMCOMP_Id) && a.LPMOEQ_SubjectiveFlg == true).Distinct().ToArray();
                    }
                    else
                    {
                        data.getquestionlist = _context.LP_Master_OE_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                        && a.ISMS_Id == data.ISMS_Id && a.LPMOEQ_ActiveFlg == true && ids.Contains(a.LPMT_Id)
                        && a.LPMCOMP_Id != null && complexities_ids.Contains(a.LPMCOMP_Id)).Distinct().ToArray();
                    }
                }
                else
                {
                    int count = Convert.ToInt32(data.LPMOEEX_NoOfQuestion);

                    if (data.subjectiveflag == true)
                    {
                        var query = _context.LP_Master_OE_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                        && a.ISMS_Id == data.ISMS_Id && a.LPMOEQ_ActiveFlg == true && ids.Contains(a.LPMT_Id)
                        && a.LPMCOMP_Id != null && complexities_ids.Contains(a.LPMCOMP_Id) && a.LPMOEQ_SubjectiveFlg == true).Distinct().ToList();
                        LP_Master_OE_QuestionsDMO = query.OrderBy(x => Guid.NewGuid()).Take(count).ToList();
                    }
                    else
                    {
                        var query1 = _context.LP_Master_OE_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                        && a.ISMS_Id == data.ISMS_Id && a.LPMOEQ_ActiveFlg == true && ids.Contains(a.LPMT_Id)
                        && a.LPMCOMP_Id != null && complexities_ids.Contains(a.LPMCOMP_Id)).Distinct().ToList();
                        LP_Master_OE_QuestionsDMO = query1.OrderBy(x => Guid.NewGuid()).Take(count).ToList();
                    }

                    data.getquestionlist = LP_Master_OE_QuestionsDMO.ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineExamDTO OnChangeMasterExam(LP_OnlineExamDTO data)
        {
            try
            {
                List<long?> sectionids = new List<long?>();

                foreach (var c in data.sectiondetailslist)
                {
                    sectionids.Add(c.ASMS_Id);
                }

                List<long> ids = new List<long>();

                foreach (var c in data.temptopics)
                {
                    ids.Add(c.LPMT_Id);
                }
                List<LP_Master_OE_ExamDMO> getmappedexam_subject = new List<LP_Master_OE_ExamDMO>();
                if (data.LPMOEEX_Id > 0)
                {
                    getmappedexam_subject = _context.LP_Master_OE_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.ASMCL_Id == data.ASMCL_Id && sectionids.Contains(a.ASMS_Id) && a.ISMS_Id == data.ISMS_Id && a.LPMOEEX_ActiveFlg == true
                    && a.EME_Id == data.EME_Id && a.LPMOEEX_Id != data.LPMOEEX_Id).ToList();
                }
                else
                {
                    getmappedexam_subject = _context.LP_Master_OE_ExamDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.ASMCL_Id == data.ASMCL_Id && sectionids.Contains(a.ASMS_Id) && a.ISMS_Id == data.ISMS_Id && a.LPMOEEX_ActiveFlg == true
                    && a.EME_Id == data.EME_Id).ToList();
                }

                if (getmappedexam_subject.Count > 0)
                {
                    data.message = "Duplicate";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineExamDTO SaveLevelQuestionOrder(LP_OnlineExamDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var Order_Count = 0;
                if (data.Order_Flag == "Question_Order")
                {
                    if (data.temporderquestiondto != null && data.temporderquestiondto.Length > 0)
                    {
                        foreach (var c in data.temporderquestiondto)
                        {
                            Order_Count += 1;

                            var QuestionResult = _context.LP_Master_OE_Exam_QuestionsDMO.Single(a => a.LPMOEEXQNS_Id == c.LPMOEEXQNS_Id);
                            QuestionResult.LPMOEEXQNS_QnsOrder = Order_Count;
                            QuestionResult.LPMOEEXQNS_UpdatedBy = data.Userid;
                            QuestionResult.LPMOEEXQNS_UpdatedDate = indiantime0;
                            _context.Update(QuestionResult);
                        }
                    }
                }
                else if (data.Order_Flag == "Level_Order")
                {
                    if (data.ExamOrderLevelDetails != null && data.ExamOrderLevelDetails.Length > 0)
                    {
                        foreach (var c in data.ExamOrderLevelDetails)
                        {
                            Order_Count += 1;

                            var LevelResult = _context.LP_Master_OE_Exam_LevelsDMO.Single(a => a.LPMOEEXLVL_Id == c.LPMOEEXLVL_Id);
                            LevelResult.LPMOEEXLVL_LevelOrder = Order_Count;
                            LevelResult.LPMOEEXLVL_UpdatedBy = data.Userid;
                            LevelResult.LPMOEEXLVL_UpdatedDate = indiantime0;
                            _context.Update(LevelResult);
                        }
                    }
                }

                var i = _context.SaveChanges();
                if (i > 0)
                {
                    data.message = "Update";
                }

                if (data.Order_Flag == "Level_Order")
                {
                    data.getarrayofleveldetails = _context.LP_Master_OE_Exam_LevelsDMO.Where(a => a.LPMOEEX_Id == data.LPMOEEX_Id).OrderBy(a => a.LPMOEEXLVL_LevelOrder).ToArray();
                }
                else if (data.Order_Flag == "Question_Order")
                {
                    data.getarrayoflevelquestiondetails = (from a in _context.LP_Master_OE_Exam_LevelsDMO
                                                           from b in _context.LP_Master_OE_Exam_QuestionsDMO
                                                           from c in _context.LP_Master_OE_QuestionsDMO
                                                           where (a.LPMOEEXLVL_Id == b.LPMOEEXLVL_Id && b.LPMOEQ_Id == c.LPMOEQ_Id
                                                           && a.LPMOEEXLVL_Id == data.LPMOEEXLVL_Id && b.LPMOEEXLVL_Id == data.LPMOEEXLVL_Id)
                                                           select new LP_OnlineExamDTO
                                                           {
                                                               LPMOEQ_Id = b.LPMOEQ_Id,
                                                               LPMOEEXQNS_Id = b.LPMOEEXQNS_Id,
                                                               LPMOEEX_Id = a.LPMOEEX_Id,
                                                               LPMOEEXQNS_ActiveFlg = b.LPMOEEXQNS_ActiveFlg,
                                                               LPMOEQ_Question = c.LPMOEQ_Question,
                                                               LPMOEEXQNS_Marks = b.LPMOEEXQNS_Marks,
                                                               LPMOEEXQNS_QnsOrder = b.LPMOEEXQNS_QnsOrder
                                                           }).Distinct().OrderBy(a => a.LPMOEEXQNS_QnsOrder).ToArray();
                }

            }
            catch (Exception ex)
            {
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        //Master Question Deactivate All
        public LP_OnlineExamDTO loaddatadeactivate(LP_OnlineExamDTO data)
        {
            try
            {
                var getuserdetails = _context.Staff_User_Login.Where(a => a.MI_Id == data.MI_Id && a.Id == data.Userid).ToList();

                if (getuserdetails.Count > 0)
                {
                    var getempcode = getuserdetails.FirstOrDefault().Emp_Code;
                    var loginid = getuserdetails.FirstOrDefault().IVRMSTAUL_Id;

                    List<LP_OnlineExamDTO> getsubjects = new List<LP_OnlineExamDTO>();
                    List<long> subjectid = new List<long>();

                    List<LP_OnlineExamDTO> getclass = new List<LP_OnlineExamDTO>();
                    List<long> classid = new List<long>();

                    getsubjects = (from a in _context.Exm_Login_PrivilegeDMO
                                   from c in _context.Exm_Login_Privilege_SubjectsDMO
                                   from d in _context.IVRM_School_Master_SubjectsDMO
                                   from e in _context.Staff_User_Login
                                   where (a.ELP_Id == c.ELP_Id && e.IVRMSTAUL_Id == a.Login_Id && c.ISMS_Id == d.ISMS_Id && a.ELP_ActiveFlg == true
                                   && d.ISMS_ActiveFlag == 1 && a.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && c.ELPs_ActiveFlg == true
                                   && a.Login_Id == loginid && e.Emp_Code == getempcode && a.ASMAY_Id == data.ASMAY_Id)
                                   select new LP_OnlineExamDTO
                                   {
                                       ISMS_Id = c.ISMS_Id
                                   }).Distinct().ToList();

                    foreach (var c in getsubjects)
                    {
                        subjectid.Add(c.ISMS_Id);
                    }

                    getclass = (from a in _context.Exm_Login_PrivilegeDMO
                                from c in _context.Exm_Login_Privilege_SubjectsDMO
                                from d in _context.AdmissionClass
                                from e in _context.Staff_User_Login
                                where (a.ELP_Id == c.ELP_Id && e.IVRMSTAUL_Id == a.Login_Id && c.ASMCL_Id == d.ASMCL_Id && a.ELP_ActiveFlg == true
                                && a.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && c.ELPs_ActiveFlg == true && a.Login_Id == loginid
                                && e.Emp_Code == getempcode && a.ASMAY_Id == data.ASMAY_Id)
                                select new LP_OnlineExamDTO
                                {
                                    ASMCL_Id = c.ASMCL_Id
                                }).Distinct().ToList();

                    foreach (var c in getclass)
                    {
                        classid.Add(c.ASMCL_Id);
                    }


                    data.getMasterQuestiondetails = (from a in _context.LP_Master_OE_QuestionsDMO
                                                     from c in _context.AdmissionClass
                                                     from d in _context.IVRM_School_Master_SubjectsDMO
                                                     from e in _context.SchoolSubjectWithMasterTopicMapping
                                                     where (a.ISMS_Id == d.ISMS_Id && a.LPMT_Id == e.LPMT_Id && subjectid.Contains(a.ISMS_Id)
                                                     && classid.Contains(a.ASMCL_Id) && a.MI_Id == data.MI_Id)
                                                     select new LP_OnlineExamDTO
                                                     {
                                                         ASMCL_Id = a.ASMCL_Id,
                                                         ISMS_Id = a.ISMS_Id,
                                                         ASMCL_ClassName = c.ASMCL_ClassName,
                                                         ISMS_SubjectName = d.ISMS_SubjectName,
                                                     }).Distinct().ToArray();

                    data.getclasslist = _context.AdmissionClass.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true
                    && classid.Contains(a.ASMCL_Id)).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                }

                else
                {
                    data.getMasterQuestiondetails = (from a in _context.LP_Master_OE_QuestionsDMO
                                                     from c in _context.AdmissionClass
                                                     from d in _context.IVRM_School_Master_SubjectsDMO
                                                     where (a.ASMCL_Id == c.ASMCL_Id && a.ISMS_Id == d.ISMS_Id && a.MI_Id == data.MI_Id)
                                                     select new LP_OnlineExamDTO
                                                     {
                                                         ASMCL_Id = a.ASMCL_Id,
                                                         ISMS_Id = a.ISMS_Id,
                                                         ASMCL_ClassName = c.ASMCL_ClassName,
                                                         ISMS_SubjectName = d.ISMS_SubjectName,
                                                     }).Distinct().ToArray();

                    data.getclasslist = _context.AdmissionClass.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineExamDTO getclasslistdeactivate(LP_OnlineExamDTO data)
        {
            try
            {
                var getuserdetails = _context.Staff_User_Login.Where(a => a.MI_Id == data.MI_Id && a.Id == data.Userid).ToList();

                if (getuserdetails.Count > 0)
                {
                    var getempcode = getuserdetails.FirstOrDefault().Emp_Code;
                    var loginid = getuserdetails.FirstOrDefault().IVRMSTAUL_Id;

                    List<LP_OnlineExamDTO> getsubjects = new List<LP_OnlineExamDTO>();
                    List<long> subjectid = new List<long>();

                    List<LP_OnlineExamDTO> getclass = new List<LP_OnlineExamDTO>();
                    List<long> classid = new List<long>();

                    getsubjects = (from a in _context.Exm_Login_PrivilegeDMO
                                   from c in _context.Exm_Login_Privilege_SubjectsDMO
                                   from d in _context.IVRM_School_Master_SubjectsDMO
                                   from e in _context.Staff_User_Login
                                   where (a.ELP_Id == c.ELP_Id && e.IVRMSTAUL_Id == a.Login_Id && c.ISMS_Id == d.ISMS_Id && a.ELP_ActiveFlg == true
                                   && d.ISMS_ActiveFlag == 1 && a.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && c.ELPs_ActiveFlg == true
                                   && a.Login_Id == loginid && e.Emp_Code == getempcode && a.ASMAY_Id == data.ASMAY_Id)
                                   select new LP_OnlineExamDTO
                                   {
                                       ISMS_Id = c.ISMS_Id
                                   }).Distinct().ToList();

                    foreach (var c in getsubjects)
                    {
                        subjectid.Add(c.ISMS_Id);
                    }

                    getclass = (from a in _context.Exm_Login_PrivilegeDMO
                                from c in _context.Exm_Login_Privilege_SubjectsDMO
                                from d in _context.AdmissionClass
                                from e in _context.Staff_User_Login
                                where (a.ELP_Id == c.ELP_Id && e.IVRMSTAUL_Id == a.Login_Id && c.ASMCL_Id == d.ASMCL_Id && a.ELP_ActiveFlg == true
                                && a.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && c.ELPs_ActiveFlg == true && a.Login_Id == loginid
                                && e.Emp_Code == getempcode && a.ASMAY_Id == data.ASMAY_Id)
                                select new LP_OnlineExamDTO
                                {
                                    ASMCL_Id = c.ASMCL_Id
                                }).Distinct().ToList();

                    foreach (var c in getclass)
                    {
                        classid.Add(c.ASMCL_Id);
                    }


                    data.getMasterQuestiondetails = (from a in _context.LP_Master_OE_QuestionsDMO
                                                     from c in _context.AdmissionClass
                                                     from d in _context.IVRM_School_Master_SubjectsDMO
                                                     from e in _context.SchoolSubjectWithMasterTopicMapping
                                                     where (a.ISMS_Id == d.ISMS_Id && a.LPMT_Id == e.LPMT_Id && subjectid.Contains(a.ISMS_Id)
                                                     && classid.Contains(a.ASMCL_Id) && a.MI_Id == data.MI_Id)
                                                     select new LP_OnlineExamDTO
                                                     {
                                                         ASMCL_Id = a.ASMCL_Id,
                                                         ISMS_Id = a.ISMS_Id,
                                                         ASMCL_ClassName = c.ASMCL_ClassName,
                                                         ISMS_SubjectName = d.ISMS_SubjectName,
                                                     }).Distinct().ToArray();

                    data.getclasslist = _context.AdmissionClass.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true
                    && classid.Contains(a.ASMCL_Id)).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                }

                else
                {
                    data.getMasterQuestiondetails = (from a in _context.LP_Master_OE_QuestionsDMO
                                                     from c in _context.AdmissionClass
                                                     from d in _context.IVRM_School_Master_SubjectsDMO
                                                     from e in _context.SchoolSubjectWithMasterTopicMapping
                                                     where (a.ASMCL_Id == c.ASMCL_Id && a.ISMS_Id == d.ISMS_Id && a.LPMT_Id == e.LPMT_Id
                                                     && a.MI_Id == data.MI_Id)
                                                     select new LP_OnlineExamDTO
                                                     {
                                                         ASMCL_Id = a.ASMCL_Id,
                                                         ISMS_Id = a.ISMS_Id,
                                                         ASMCL_ClassName = c.ASMCL_ClassName,
                                                         ISMS_SubjectName = d.ISMS_SubjectName,
                                                     }).Distinct().ToArray();

                    data.getclasslist = _context.AdmissionClass.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineExamDTO getsubjectlistdeactivate(LP_OnlineExamDTO data)
        {
            try
            {
                var getuserdetails = _context.Staff_User_Login.Where(a => a.MI_Id == data.MI_Id && a.Id == data.Userid).ToList();

                var getinstitutiontype = _context.Institution.Where(a => a.MI_Id == data.MI_Id && a.MI_ActiveFlag == 1).ToList();


                data.getsubjectlist = (from a in _context.LP_Master_OE_QuestionsDMO
                                       from b in _context.IVRM_School_Master_SubjectsDMO
                                       where (a.ISMS_Id == b.ISMS_Id && a.ASMCL_Id == data.ASMCL_Id)
                                       select b).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineExamDTO GetQuestionList(LP_OnlineExamDTO data)
        {
            try
            {
                data.getquestionlist = _context.LP_Master_OE_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id
                && a.ISMS_Id == data.ISMS_Id && a.LPMOEQ_ActiveFlg == true
                && (a.CreatedDate.Date >= data.FromDate.Value.Date && a.CreatedDate.Date <= data.ToDate.Value.Date)).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineExamDTO SaveDeactiveQuestionDetails(LP_OnlineExamDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.tempquestiondto.Length > 0)
                {
                    foreach (var c in data.tempquestiondto)
                    {
                        var result = _context.LP_Master_OE_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id && a.LPMOEQ_Id == c.LPMOEQ_Id).ToList();

                        if (result.Count > 0)
                        {
                            var checkresult = _context.LP_Master_OE_QuestionsDMO.Single(a => a.MI_Id == data.MI_Id && a.LPMOEQ_Id == c.LPMOEQ_Id);
                            checkresult.LPMOEQ_ActiveFlg = false;
                            checkresult.UpdatedDate = indiantime0;
                            checkresult.LPMOEQ_UpdatedBy = data.Userid;
                            _context.Update(checkresult);
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
            }
            return data;
        }

        // Master Complexities
        public LP_OnlineExamDTO getmastercompliexities(LP_OnlineExamDTO data)
        {
            try
            {
                data.getMasterComplexitiesdetails = _context.LP_Master_ComplexitiesDMO.ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }
        public LP_OnlineExamDTO SaveMasterComplexity(LP_OnlineExamDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                if (data.LPMCOMP_Id > 0)
                {
                    data.message = "Update";
                    var checkduplicate = _context.LP_Master_ComplexitiesDMO.Where(a => a.LPMCOMP_Id != data.LPMCOMP_Id
                     && a.LPMCOMP_ComplexityName == data.LPMCOMP_ComplexityName).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        var checkresult = _context.LP_Master_ComplexitiesDMO.Single(a => a.LPMCOMP_Id == data.LPMCOMP_Id);
                        checkresult.LPMCOMP_ComplexityName = data.LPMCOMP_ComplexityName;
                        checkresult.LPMCOMP_ComplexityDesc = data.LPMCOMP_ComplexityDesc;
                        checkresult.LPMCOMP_DefaultFlg = data.LPMCOMP_DefaultFlg;
                        checkresult.LPMCOMP_UpdatedDate = indiantime0;
                        checkresult.LPMCOMP_UpdatedBy = data.Userid;
                        _context.Update(checkresult);
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
                else
                {

                    var checkduplicate = _context.LP_Master_ComplexitiesDMO.Where(a => a.LPMCOMP_ComplexityName == data.LPMCOMP_ComplexityName).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        data.message = "Add";
                        LP_Master_ComplexitiesDMO lP_Master_ComplexitiesDMO = new LP_Master_ComplexitiesDMO();
                        lP_Master_ComplexitiesDMO.LPMCOMP_ComplexityName = data.LPMCOMP_ComplexityName;
                        lP_Master_ComplexitiesDMO.LPMCOMP_ComplexityDesc = data.LPMCOMP_ComplexityDesc;
                        lP_Master_ComplexitiesDMO.LPMCOMP_DefaultFlg = data.LPMCOMP_DefaultFlg;
                        lP_Master_ComplexitiesDMO.LPMCOMP_ActiveFlg = true;
                        lP_Master_ComplexitiesDMO.LPMCOMP_UpdatedDate = indiantime0;
                        lP_Master_ComplexitiesDMO.LPMCOMP_CreatedDate = indiantime0;
                        lP_Master_ComplexitiesDMO.LPMCOMP_UpdatedBy = data.Userid;
                        lP_Master_ComplexitiesDMO.LPMCOMP_CreatedBy = data.Userid;
                        _context.Add(lP_Master_ComplexitiesDMO);
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
                data.message = "Error";
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineExamDTO DeactivateActivateComplexities(LP_OnlineExamDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                var checkresult = _context.LP_Master_ComplexitiesDMO.Single(a => a.LPMCOMP_Id == data.LPMCOMP_Id);
                checkresult.LPMCOMP_ActiveFlg = checkresult.LPMCOMP_ActiveFlg == true ? false : true;
                checkresult.LPMCOMP_UpdatedDate = indiantime0;
                checkresult.LPMCOMP_UpdatedBy = data.Userid;
                _context.Update(checkresult);
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

        // Report
        public LP_OnlineExamDTO LoadReport(LP_OnlineExamDTO data)
        {
            try
            {
                data.getclasslist = _context.AdmissionClass.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true).OrderBy(a => a.ASMCL_Order).ToArray();

                data.getyearlist = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderBy(a => a.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public LP_OnlineExamDTO GetReport(LP_OnlineExamDTO data)
        {
            try
            {
                if (data.classlistarray != null && data.classlistarray.Length > 0)
                {
                    List<long> sec = new List<long>();

                    string cls_Id = "0";
                    if (data.classlistarray != null && data.classlistarray.Length > 0)
                    {
                        foreach (var c in data.classlistarray)
                        {
                            cls_Id = cls_Id + "," + c.columnID.ToString();
                        }
                    }

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "LP_OnlineExam_MasterQuestion_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 300000;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.Report_Type == "OverAll" ? "0" : cls_Id });
                        cmd.Parameters.Add(new SqlParameter("@Report_Type", SqlDbType.VarChar) { Value = data.Report_Type });
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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.GetReport = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
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
        public LP_OnlineExamDTO GetStaffWiseExamReport(LP_OnlineExamDTO data)
        {
            try
            {
                if (data.classlistarray != null && data.classlistarray.Length > 0)
                {
                    List<long> sec = new List<long>();

                    string cls_Id = "0";
                    if (data.classlistarray != null && data.classlistarray.Length > 0)
                    {
                        foreach (var c in data.classlistarray)
                        {
                            cls_Id = cls_Id + "," + c.columnID.ToString();
                        }
                    }

                    DateTime fromdate = new DateTime();
                    string confromdate = "";

                    fromdate = Convert.ToDateTime(data.FromDate.Value.Date.ToString("yyyy-MM-dd"));
                    confromdate = fromdate.ToString("yyyy-MM-dd");

                    DateTime todate = new DateTime();
                    string contodate = "";

                    todate = Convert.ToDateTime(data.ToDate.Value.Date.ToString("yyyy-MM-dd"));
                    contodate = todate.ToString("yyyy-MM-dd");

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "LP_OnlineExam_StaffWiseCount";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 300000;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = cls_Id });
                        cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = confromdate });
                        cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar) { Value = contodate });
                        cmd.Parameters.Add(new SqlParameter("@Report_Type", SqlDbType.VarChar) { Value = data.Report_Type });
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
                                        dataRow.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]);
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.GetReport = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
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
    }
}