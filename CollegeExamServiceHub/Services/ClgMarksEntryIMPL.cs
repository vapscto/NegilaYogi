using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.Exam;
using DomainModel.Model.com.vapstech.College.Exam;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Exam;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeExamServiceHub.Services
{
    public class ClgMarksEntryIMPL : Interfaces.ClgMarksEntryInterface
    {
        private static ConcurrentDictionary<string, ClgMarksEntryDTO> _login =
        new ConcurrentDictionary<string, ClgMarksEntryDTO>();


        public ClgExamContext _examcontext;
        public DomainModelMsSqlServerContext _db;
        readonly ILogger<ClgMarksEntryIMPL> _logger;
        public ClgMarksEntryIMPL(ClgExamContext ttcategory, ILogger<ClgMarksEntryIMPL> log, DomainModelMsSqlServerContext db)
        {
            _examcontext = ttcategory;
            _logger = log;
            _db = db;
        }
        public ClgMarksEntryDTO getdetails(ClgMarksEntryDTO TTMC)
        {
            try
            {
                var userid = getuserid(TTMC.username);

                TTMC.getyear = _examcontext.AcademicYear.Where(a => a.MI_Id == TTMC.MI_Id && a.Is_Active == true).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();


                var check_rolename = (from a in _examcontext.MasterRoleType
                                      where (a.IVRMRT_Id == TTMC.roleId)
                                      select new ClgMarksEntryDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }).ToList();

                var empcode_check = (from a in _examcontext.Staff_User_Login
                                     where (a.MI_Id == TTMC.MI_Id && a.Id.Equals(userid))
                                     select new ClgMarksEntryDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                if (empcode_check.Count > 0)
                {
                    TTMC.courseslist = (from a in _examcontext.Adm_College_Atten_Login_UserDMO
                                        from b in _examcontext.Adm_College_Atten_Login_DetailsDMO
                                        from c in _examcontext.AcademicYear
                                        from d in _examcontext.MasterCourseDMO
                                        where (a.ACALU_Id == b.ACALU_Id && a.ASMAY_Id == c.ASMAY_Id && b.AMCO_Id == d.AMCO_Id && a.MI_Id == TTMC.MI_Id && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && d.AMCO_ActiveFlag == true && b.ACALD_ActiveFlag == true)
                                        select d).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
                }
                else
                {
                    TTMC.message = "For This Staff There Is No Privileges To Enter Attendance.. Please Contact Administrator";
                }
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return TTMC;

        }
        public ClgMarksEntryDTO onchangeyear(ClgMarksEntryDTO TTMC)
        {
            try
            {
                var userid = getuserid(TTMC.username);

                var check_rolename = (from a in _examcontext.MasterRoleType
                                      where (a.IVRMRT_Id == TTMC.roleId)
                                      select new ClgMarksEntryDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }).ToList();

                var empcode_check = (from a in _examcontext.Staff_User_Login
                                     where (a.MI_Id == TTMC.MI_Id && a.Id.Equals(userid))
                                     select new ClgMarksEntryDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                if (empcode_check.Count > 0)
                {
                    TTMC.courseslist = (from a in _examcontext.Adm_College_Atten_Login_UserDMO
                                        from b in _examcontext.Adm_College_Atten_Login_DetailsDMO
                                        from c in _examcontext.AcademicYear
                                        from d in _examcontext.MasterCourseDMO
                                        where (a.ACALU_Id == b.ACALU_Id && a.ASMAY_Id == c.ASMAY_Id && b.AMCO_Id == d.AMCO_Id
                                        && a.MI_Id == TTMC.MI_Id && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && d.AMCO_ActiveFlag == true
                                        && b.ACALD_ActiveFlag == true && a.ASMAY_Id == TTMC.ASMAY_Id)
                                        select d).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
                }
                else
                {
                    TTMC.message = "For This Staff There Is No Privileges To Enter Attendance.. Please Contact Administrator";
                }
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return TTMC;

        }
        public ClgMarksEntryDTO onchangecourse(ClgMarksEntryDTO data)
        {
            try
            {
                var userid = getuserid(data.username);

                var empcode_check = (from a in _examcontext.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.Id.Equals(userid))
                                     select new ClgMarksEntryDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                data.branchlist = (from a in _examcontext.ClgMasterBranchDMO
                                   from d in _examcontext.AcademicYear
                                   from e in _examcontext.Adm_College_Atten_Login_DetailsDMO
                                   from f in _examcontext.Adm_College_Atten_Login_UserDMO
                                   where (a.AMB_Id == e.AMB_Id && f.ASMAY_Id == d.ASMAY_Id && e.ACALU_Id == f.ACALU_Id && a.MI_Id == data.MI_Id && e.AMCO_Id == data.AMCO_Id && f.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && f.ASMAY_Id == data.ASMAY_Id && e.ACALD_ActiveFlag == true)
                                   select a).Distinct().OrderBy(a => a.AMB_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgMarksEntryDTO onchangebranch(ClgMarksEntryDTO data)
        {
            try
            {
                List<long> bid = new List<long>();

                foreach (var c in data.marksbranch)
                {
                    bid.Add(c.AMB_Id);
                }

                var userid = getuserid(data.username);

                var empcode_check = (from a in _examcontext.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.Id.Equals(userid))
                                     select new ClgMarksEntryDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                data.semisters = (from a in _examcontext.CLG_Adm_Master_SemesterDMO
                                  from c in _examcontext.AcademicYear
                                  from d in _examcontext.Adm_College_Atten_Login_DetailsDMO
                                  from e in _examcontext.Adm_College_Atten_Login_UserDMO
                                  where (d.AMSE_Id == a.AMSE_Id && e.ACALU_Id == d.ACALU_Id && a.MI_Id == data.MI_Id
                                  && e.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && bid.Contains(d.AMB_Id) && d.AMCO_Id == data.AMCO_Id && e.ASMAY_Id == data.ASMAY_Id && d.ACALD_ActiveFlag == true)
                                  select a).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgMarksEntryDTO get_exams(ClgMarksEntryDTO data)
        {
            ClgMarksEntryDTO TTMC = new ClgMarksEntryDTO();
            try
            {
                List<long> bid = new List<long>();

                foreach (var c in data.marksbranch)
                {
                    bid.Add(c.AMB_Id);
                }

                var userid = getuserid(data.username);

                var empcode_check = (from a in _examcontext.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.Id.Equals(userid))
                                     select new ClgMarksEntryDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                TTMC.examlist = (from s in _examcontext.col_exammasterDMO
                                 from k in _examcontext.Exm_Col_Yearly_Scheme_ExamsDMO
                                 where (s.EME_Id == k.EME_Id && k.AMCO_Id == data.AMCO_Id && bid.Contains(k.AMB_Id) && s.MI_Id == data.MI_Id
                                 && k.AMSE_Id == data.AMSE_Id && k.ECYSE_ActiveFlg == true)
                                 select new ClgMarksEntryDTO
                                 {
                                     EME_Id = s.EME_Id,
                                     EME_ExamName = s.EME_ExamName,
                                     EME_ExamCode = s.EME_ExamCode,
                                     EME_ExamOrder = s.EME_ExamOrder
                                 }).Distinct().OrderBy(t => t.EME_ExamOrder).ToArray();

                TTMC.sectionlist = (from a in _examcontext.Adm_College_Master_SectionDMO
                                    from b in _examcontext.Adm_College_Atten_Login_DetailsDMO
                                    from f in _examcontext.Adm_College_Atten_Login_UserDMO
                                    from g in _examcontext.AcademicYear
                                    where (a.ACMS_Id == b.ACMS_Id && b.AMB_Id == b.AMB_Id && b.AMCO_Id == b.AMCO_Id && b.AMSE_Id == b.AMSE_Id
                                    && f.ASMAY_Id == g.ASMAY_Id && bid.Contains(b.AMB_Id) && b.AMCO_Id == data.AMCO_Id && b.ACALU_Id == f.ACALU_Id
                                    && b.AMSE_Id == data.AMSE_Id && a.MI_Id == data.MI_Id && f.HRME_Id == empcode_check.FirstOrDefault().Emp_Code
                                    && f.ASMAY_Id == data.ASMAY_Id && b.ACALD_ActiveFlag == true)
                                    select a).Distinct().OrderBy(a => a.ACMS_Order).ToArray();

                TTMC.subjectgrplist = (from a in _examcontext.Exm_Col_Yearly_SchemeDMO
                                       from b in _examcontext.Exm_Col_Yearly_Scheme_GroupDMO
                                       from c in _examcontext.Exm_Master_GroupDMO
                                       where (a.ECYS_Id == b.ECYS_Id && b.EMG_Id == c.EMG_Id && bid.Contains(a.AMB_Id) && a.AMCO_Id == data.AMCO_Id
                                       && a.AMSE_Id == data.AMSE_Id && a.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id && a.ECYS_ActiveFlag == true
                                       && b.ECYSG_ActiveFlag == true && c.EMG_ActiveFlag == true)
                                       select c).Distinct().OrderBy(a => a.EMG_GroupName).ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return TTMC;

        }
        public ClgMarksEntryDTO get_subjects(ClgMarksEntryDTO data)
        {
            try
            {
                List<long> bid = new List<long>();

                foreach (var c in data.marksbranch)
                {
                    bid.Add(c.AMB_Id);
                }

                List<long> secid = new List<long>();

                foreach (var c in data.markssection)
                {
                    secid.Add(c.ACMS_Id);
                }

                var userid = getuserid(data.username);

                var empcode_check = (from a in _examcontext.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.Id.Equals(userid))
                                     select new ClgMarksEntryDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                data.subjectgroups = (from a in _examcontext.IVRM_School_Master_SubjectsDMO
                                      from b in _examcontext.ClgMasterBranchDMO
                                      from c in _examcontext.MasterCourseDMO
                                      from d in _examcontext.CLG_Adm_Master_SemesterDMO
                                      from e in _examcontext.Adm_College_Atten_Login_DetailsDMO
                                      from f in _examcontext.Adm_College_Atten_Login_UserDMO
                                      from g in _examcontext.AcademicYear
                                      from h in _examcontext.Adm_College_Master_SectionDMO
                                      where (a.ISMS_Id == e.ISMS_Id && b.AMB_Id == e.AMB_Id && c.AMCO_Id == e.AMCO_Id && e.ACMS_Id == h.ACMS_Id
                                      && f.ASMAY_Id == g.ASMAY_Id && e.ACALU_Id == f.ACALU_Id && bid.Contains(e.AMB_Id) && e.AMCO_Id == data.AMCO_Id
                                      && secid.Contains(e.ACMS_Id) && e.AMSE_Id == data.AMSE_Id && a.MI_Id == data.MI_Id
                                      && f.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && f.ASMAY_Id == data.ASMAY_Id && e.ACALD_ActiveFlag == true)
                                      select a).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgMarksEntryDTO getsubjectscheme(ClgMarksEntryDTO data)
        {
            try
            {
                List<long> bid = new List<long>();

                foreach (var c in data.marksbranch)
                {
                    bid.Add(c.AMB_Id);
                }

                List<long> secid = new List<long>();

                foreach (var c in data.markssection)
                {
                    secid.Add(c.ACMS_Id);
                }

                var userid = getuserid(data.username);

                var empcode_check = (from a in _examcontext.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.Id.Equals(userid))
                                     select new ClgMarksEntryDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                data.getsubjectschemetype = (from a in _examcontext.AdmCollegeSubjectSchemeDMO
                                             from b in _examcontext.Exm_Col_Yearly_SchemeDMO
                                             from c in _examcontext.Exm_Col_Yearly_Scheme_ExamsDMO
                                             from d in _examcontext.Exm_Col_Yrly_Sch_Exams_SubwiseDMO
                                             from e in _examcontext.IVRM_School_Master_SubjectsDMO
                                             where (a.ACSS_Id == b.ACSS_Id && b.ECYS_Id == c.ECYS_Id && c.ECYSE_Id == d.ECYSE_Id && e.ISMS_Id == d.ISMS_Id
                                             && b.AMCO_Id == data.AMCO_Id && bid.Contains(b.AMB_Id) && b.AMSE_Id == data.AMSE_Id && b.ECYS_ActiveFlag == true
                                             && c.ECYSE_ActiveFlg == true && d.ECYSES_ActiveFlg == true && e.ISMS_ActiveFlag == 1 && d.ISMS_Id == data.ISMS_Id
                                             && c.EME_Id == data.EME_Id)
                                             select a).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgMarksEntryDTO getsubjectschemetype(ClgMarksEntryDTO data)
        {
            try
            {
                List<long> bid = new List<long>();

                foreach (var c in data.marksbranch)
                {
                    bid.Add(c.AMB_Id);
                }

                List<long> secid = new List<long>();

                foreach (var c in data.markssection)
                {
                    secid.Add(c.ACMS_Id);
                }

                var userid = getuserid(data.username);

                var empcode_check = (from a in _examcontext.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.Id.Equals(userid))
                                     select new ClgMarksEntryDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                data.getschemetype = (from a in _examcontext.AdmCollegeSubjectSchemeDMO
                                      from b in _examcontext.Exm_Col_Yearly_SchemeDMO
                                      from c in _examcontext.Exm_Col_Yearly_Scheme_ExamsDMO
                                      from d in _examcontext.Exm_Col_Yrly_Sch_Exams_SubwiseDMO
                                      from e in _examcontext.IVRM_School_Master_SubjectsDMO
                                      from f in _examcontext.AdmCollegeSchemeTypeDMO
                                      where (a.ACSS_Id == b.ACSS_Id && b.ACST_Id == f.ACST_Id && b.ECYS_Id == c.ECYS_Id && c.ECYSE_Id == d.ECYSE_Id
                                      && e.ISMS_Id == d.ISMS_Id && b.AMCO_Id == data.AMCO_Id && bid.Contains(b.AMB_Id) && b.AMSE_Id == data.AMSE_Id
                                      && b.ECYS_ActiveFlag == true && c.ECYSE_ActiveFlg == true && d.ECYSES_ActiveFlg == true && e.ISMS_ActiveFlag == 1
                                      && d.ISMS_Id == data.ISMS_Id && c.EME_Id == data.EME_Id && b.ACSS_Id == data.ACSS_Id)
                                      select f).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<ClgMarksEntryDTO> onsearch(ClgMarksEntryDTO id)
        {
            ClgMarksEntryDTO EM = new ClgMarksEntryDTO();
            try
            {
                EM.configuration = _examcontext.Exm_ConfigurationDMO.Where(a => a.MI_Id == id.MI_Id).ToArray();


                string branchid = "0";
                foreach (var c in id.marksbranch)
                {
                    branchid = branchid + "," + c.AMB_Id;
                }

                string sectionid = "0";
                foreach (var c in id.markssection)
                {
                    sectionid = sectionid + "," + c.ACMS_Id;
                }

                //MarksCalcReset MarksCalcReset = new MarksCalcReset(_examcontext);
                //EM.marksdeleteflag = MarksCalcReset.MarksCalculationResetFlag(id.ASMAY_Id, id.ASMCL_Id, id.ASMS_Id, id.MI_Id, id.EME_Id);
                var ecysid = _examcontext.Exm_Col_Yearly_SchemeDMO.Where(t => t.MI_Id == id.MI_Id && t.ECYS_ActiveFlag == true && t.ACSS_Id == id.ACSS_Id && t.ACST_Id == id.ACST_Id).Select(t => t.ECYS_Id).ToArray();

                var ecyseid = _examcontext.Exm_Col_Yearly_Scheme_ExamsDMO.Where(t => ecysid.Contains(t.ECYS_Id) && t.ACST_Id == id.ACST_Id && t.ACSS_Id == id.ACSS_Id).Select(t => t.ECYSE_Id).ToArray();

                var subid = _examcontext.Exm_Col_Yrly_Sch_Exams_SubwiseDMO.Where(t => ecyseid.Contains(t.ECYSE_Id)).ToArray();

                var subMorGFlag = _examcontext.Exm_Col_Yrly_Sch_Exams_SubwiseDMO.Where(t => ecyseid.Contains(t.ECYSE_Id)).ToArray();

                EM.subMorGFlag = subMorGFlag[0].ECYSES_MarksGradeEntryFlg;

                EM.EMGR_Id = subMorGFlag[0].EMGR_Id;

                if (EM.subMorGFlag == "G")
                {
                    EM.gradname = (from a in _examcontext.Exm_Master_GradeDMO
                                   from b in _examcontext.Exm_Master_Grade_DetailsDMO
                                   where (a.MI_Id == id.MI_Id && a.EMGR_Id == EM.EMGR_Id && b.EMGR_Id == EM.EMGR_Id)
                                   select new ClgMarksEntryDTO
                                   {
                                       grade = b.EMGD_Name
                                   }).Select(b => b.grade).ToArray();
                }

                List<ClgMarksEntryDTO> result = new List<ClgMarksEntryDTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Clg_Exam_get_Marks_Entry_New_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@AMCO_ID", SqlDbType.VarChar) { Value = id.AMCO_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMB_ID", SqlDbType.VarChar) { Value = branchid });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_ID", SqlDbType.VarChar) { Value = id.AMSE_Id });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = id.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = id.EME_Id });
                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = id.ISMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.VarChar) { Value = sectionid });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = id.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ACSS_Id", SqlDbType.VarChar) { Value = id.ACSS_Id });
                    cmd.Parameters.Add(new SqlParameter("@ACST_Id", SqlDbType.VarChar) { Value = id.ACST_Id });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                result.Add(new ClgMarksEntryDTO
                                {
                                    AMCST_Id = Convert.ToInt64(dataReader["AMCST_Id"].ToString()),
                                    courseid = Convert.ToInt64(dataReader["AMCO_Id"].ToString()),
                                    branchid = Convert.ToInt64(dataReader["AMB_Id"].ToString()),
                                    semesterid = Convert.ToInt64(dataReader["AMSE_Id"].ToString()),
                                    sectionid = Convert.ToInt64(dataReader["ACMS_Id"].ToString()),
                                    studentname = ((dataReader["AMCST_FirstName"].ToString() == null ? " " : dataReader["AMCST_FirstName"].ToString()) + " " + (dataReader["AMCST_MiddleName"].ToString() == null ? " " : dataReader["AMCST_MiddleName"].ToString()) + " " + (dataReader["AMCST_LastName"].ToString() == null ? " " : dataReader["AMCST_LastName"].ToString())).Trim(),
                                    amcsT_AdmNo = dataReader["AMCST_AdmNo"].ToString() == null ? "" : dataReader["AMCST_AdmNo"].ToString(),
                                    amcsT_RegistrationNo = dataReader["AMCST_RegistrationNo"].ToString() == null ? "" : dataReader["AMCST_RegistrationNo"].ToString(),
                                    acysT_RollNo = Convert.ToInt32(dataReader["ACYST_RollNo"].ToString()),
                                    SubjectName = dataReader["ISMS_SubjectName"].ToString(),
                                    TotalMarks = Convert.ToDecimal(dataReader["ECYSES_MaxMarks"].ToString()),
                                    MarksEnterFor = Convert.ToDecimal(dataReader["ECYSES_MarksEntryMax"].ToString()),
                                    MinMarks = Convert.ToDecimal(dataReader["ECYSES_MinMarks"].ToString()),
                                    obtainmarks = (dataReader["ECSTM_Flg"].ToString() == "" ? dataReader["ECSTM_Marks"].ToString() : dataReader["ECSTM_Flg"].ToString())
                                });
                                EM.studentList = result.Distinct().ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return EM;
        }
        public ClgMarksEntryDTO SaveMarks(ClgMarksEntryDTO id)
        {
            try
            {
                //MarksCalcReset MarksCalcReset = new MarksCalcReset(_examcontext);
                //id.marksdeleteflag = MarksCalcReset.MarksCalculationReset(id.ASMAY_Id, id.ASMCL_Id, id.ASMS_Id, id.MI_Id, id.EME_Id);

                var ecysid = _examcontext.Exm_Col_Yearly_SchemeDMO.Where(t => t.MI_Id == id.MI_Id && t.ECYS_ActiveFlag == true && t.ACSS_Id == id.ACSS_Id && t.ACST_Id == id.ACST_Id).Select(t => t.ECYS_Id).ToArray();

                var ecyseid = _examcontext.Exm_Col_Yearly_Scheme_ExamsDMO.Where(t => ecysid.Contains(t.ECYS_Id) && t.ACSS_Id == id.ACSS_Id && t.ACST_Id == id.ACST_Id).Select(t => t.ECYSE_Id).ToArray();

                var subid = _examcontext.Exm_Col_Yrly_Sch_Exams_SubwiseDMO.Where(t => ecyseid.Contains(t.ECYSE_Id)).ToArray();

                var subMorGFlag = _examcontext.Exm_Col_Yrly_Sch_Exams_SubwiseDMO.Where(t => ecyseid.Contains(t.ECYSE_Id)).ToArray();

                id.subMorGFlag = subMorGFlag[0].ECYSES_MarksGradeEntryFlg;

                id.EMGR_Id = subMorGFlag[0].EMGR_Id;

                //string msg = "";

                for (int i = 0; i < id.detailsList.Length; i++)
                {
                    var checkduplicates = _examcontext.Exm_Col_Student_MarksDMO.Where(d => d.AMCO_ID == id.AMCO_Id
                    && d.AMB_ID == Convert.ToInt64(id.detailsList[i].branchid) && d.AMSE_ID == id.AMSE_Id && d.AMCST_Id == id.detailsList[i].amcsT_Id
                    && d.EME_Id == id.EME_Id && d.ISMS_Id == id.ISMS_Id && d.ECSTM_ActiveFlg == true && d.MI_Id == id.MI_Id
                    && d.ACMS_Id == Convert.ToInt64(id.detailsList[i].sectionid)).ToList();

                    if (checkduplicates.Count > 0)
                    {
                        var result = _examcontext.Exm_Col_Student_MarksDMO.Single(d => d.AMCO_ID == id.AMCO_Id
                        && d.AMB_ID == Convert.ToInt64(id.detailsList[i].branchid) && d.AMSE_ID == id.AMSE_Id && d.AMCST_Id == id.detailsList[i].amcsT_Id
                        && d.EME_Id == id.EME_Id && d.ISMS_Id == id.ISMS_Id && d.ECSTM_ActiveFlg == true && d.MI_Id == id.MI_Id
                        && d.ACMS_Id == Convert.ToInt64(id.detailsList[i].sectionid));

                        result.MI_Id = id.MI_Id;
                        result.AMCO_ID = id.AMCO_Id;
                        result.AMB_ID = Convert.ToInt64(id.detailsList[i].branchid);
                        result.AMSE_ID = id.AMSE_Id;
                        result.ACMS_Id = Convert.ToInt64(id.detailsList[i].sectionid);
                        result.EME_Id = id.EME_Id;
                        result.ISMS_Id = id.ISMS_Id;
                        result.ASMAY_Id = id.ASMAY_Id;
                        result.AMCST_Id = id.detailsList[i].amcsT_Id;

                        if (id.detailsList[i].obtainmarks == "AB" || id.detailsList[i].obtainmarks == "ab" || id.detailsList[i].obtainmarks == "L"
                            || id.detailsList[i].obtainmarks == "l" || id.detailsList[i].obtainmarks == "M" || id.detailsList[i].obtainmarks == "m")
                        {
                            result.ECSTM_Flg = id.detailsList[i].obtainmarks;
                            result.ECSTM_Marks = Convert.ToDecimal("0");
                            result.ECSTM_Grade = "null";
                            if (id.subMorGFlag == "M")
                            {
                                result.ECSTM_MarksGradeFlg = "M";
                            }
                            else if (id.subMorGFlag == "G")
                            {
                                result.ECSTM_MarksGradeFlg = "G";
                            }
                        }
                        else
                        {
                            result.ECSTM_Flg = "";

                            if (id.subMorGFlag == "M")
                            {
                                result.ECSTM_MarksGradeFlg = "M";
                                result.ECSTM_Marks = Convert.ToDecimal(id.detailsList[i].obtainmarks);
                                result.ECSTM_Grade = "null";
                            }
                            else if (id.subMorGFlag == "G")
                            {
                                result.ECSTM_MarksGradeFlg = "G";
                                result.ECSTM_Marks = Convert.ToDecimal("0");
                                result.ECSTM_Grade = id.detailsList[i].obtainmarks; //id.ESTM_Grade;
                            }
                        }
                        result.UpdatedDate = DateTime.Now;
                        result.Login_Id = id.Id;
                        result.ECSTM_UpdatedBy = id.Id;
                        result.ECSTM_IP4Address = id.IP4;
                        result.ECSTM_LoginDate = DateTime.Now;
                        _examcontext.Update(result);
                    }
                    else
                    {
                        Exm_Col_Student_MarksDMO MM = Mapper.Map<Exm_Col_Student_MarksDMO>(id);
                        MM.MI_Id = id.MI_Id;
                        MM.AMCO_ID = id.AMCO_Id;
                        MM.AMB_ID = Convert.ToInt64(id.detailsList[i].branchid);
                        MM.AMSE_ID = id.AMSE_Id;
                        MM.ACMS_Id = Convert.ToInt64(id.detailsList[i].sectionid);
                        MM.EME_Id = id.EME_Id;
                        MM.ISMS_Id = id.ISMS_Id;
                        MM.ASMAY_Id = id.ASMAY_Id;
                        MM.AMCST_Id = id.detailsList[i].amcsT_Id;

                        if (id.detailsList[i].obtainmarks == "AB" || id.detailsList[i].obtainmarks == "ab" || id.detailsList[i].obtainmarks == "L"
                            || id.detailsList[i].obtainmarks == "l" || id.detailsList[i].obtainmarks == "M" || id.detailsList[i].obtainmarks == "m")
                        {
                            MM.ECSTM_Flg = id.detailsList[i].obtainmarks;
                            MM.ECSTM_Marks = Convert.ToDecimal("0");
                            MM.ECSTM_Grade = "null";
                            if (id.subMorGFlag == "M")
                            {
                                MM.ECSTM_MarksGradeFlg = "M";
                            }
                            else if (id.subMorGFlag == "G")
                            {
                                MM.ECSTM_MarksGradeFlg = "G";
                            }
                        }
                        else
                        {
                            MM.ECSTM_Flg = "";

                            if (id.subMorGFlag == "M")
                            {
                                MM.ECSTM_Marks = Convert.ToDecimal(id.detailsList[i].obtainmarks);
                                MM.ECSTM_MarksGradeFlg = "M";
                                MM.ECSTM_Grade = "null";
                            }
                            else if (id.subMorGFlag == "G")
                            {
                                MM.ECSTM_Marks = Convert.ToDecimal("0");
                                MM.ECSTM_MarksGradeFlg = "G";
                                MM.ECSTM_Grade = id.detailsList[i].obtainmarks; //id.ESTM_Grade;
                            }
                        }
                        MM.CreatedDate = DateTime.Now;
                        MM.UpdatedDate = DateTime.Now;
                        MM.Login_Id = id.Id;
                        MM.ECSTM_CreatedBy = id.Id;
                        MM.ECSTM_UpdatedBy = id.Id;
                        MM.ECSTM_IP4Address = id.IP4;
                        MM.ECSTM_ActiveFlg = true;
                        MM.ECSTM_LoginDate = DateTime.Now;
                        _examcontext.Add(MM);
                    }
                }

                if (id.detailsList.Length > 0)
                {
                    int flag = _examcontext.SaveChanges();
                    if (flag > 0)
                    {
                        id.messagesaveupdate = "true";
                        if (id.message == "Autocalculate")
                        {
                            using (var cmd = _examcontext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "CLG_IndSubjects_SubsExmMarksCalculation";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = id.MI_Id });
                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = id.ASMAY_Id });
                                cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.BigInt) { Value = id.AMCO_Id });
                                cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.BigInt) { Value = id.AMB_Id });
                                cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.BigInt) { Value = id.ACMS_Id });
                                cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.BigInt) { Value = id.AMSE_Id });
                                cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.Int) { Value = id.EME_Id });
                                if (cmd.Connection.State != ConnectionState.Open)
                                    cmd.Connection.Open();

                                var a = cmd.ExecuteNonQuery();

                                
                            }
                        }
                    }
                    else
                    {
                        id.messagesaveupdate = "false";
                    }
                }
                else
                {
                    id.messagesaveupdate = "false";
                }
            }
            catch (Exception ee)
            {
                id.messagesaveupdate = "false";
                Console.WriteLine(ee.Message);
            }
            return id;
        }
        public long getuserid(string data)
        {
            string username = data.ToString();
            //string id = "";
            var getuseridd = _examcontext.ApplUser.Where(a => a.UserName == data).FirstOrDefault().Id;
            return getuseridd;
        }

    }
}
