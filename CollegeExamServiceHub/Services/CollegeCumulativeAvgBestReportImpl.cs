using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.com.vapstech.College.Exam;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Exam;

namespace CollegeExamServiceHub.Services
{
    public class CollegeCumulativeAvgBestReportImpl : Interfaces.CollegeCumulativeAvgBestReportInterface
    {
        public ClgExamContext _examcontext;
        readonly ILogger<CollegeCumulativeAvgBestReportDTO> _logger;

        public CollegeCumulativeAvgBestReportImpl(ClgExamContext _exam, ILogger<CollegeCumulativeAvgBestReportDTO> _log)
        {
            _examcontext = _exam;
            _logger = _log;
        }
        public CollegeCumulativeAvgBestReportDTO Getdetails(CollegeCumulativeAvgBestReportDTO data)
        {
            try
            {
                data.getyear = _examcontext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeCumulativeAvgBestReportDTO onchangeyear(CollegeCumulativeAvgBestReportDTO data)
        {
            try
            {
                var getempcode = _examcontext.Staff_User_Login.Where(a => a.Id == data.Userid).ToList();


                if (getempcode.Count > 0)
                {
                    data.getcourse = (from a in _examcontext.Adm_College_Atten_Login_UserDMO
                                      from b in _examcontext.Adm_College_Atten_Login_DetailsDMO
                                      from c in _examcontext.AcademicYear
                                      from d in _examcontext.MasterCourseDMO
                                      where (a.ACALU_Id == b.ACALU_Id && a.ASMAY_Id == c.ASMAY_Id && b.AMCO_Id == d.AMCO_Id && a.MI_Id == data.MI_Id
                                      && a.HRME_Id == getempcode.FirstOrDefault().Emp_Code && d.AMCO_ActiveFlag == true && b.ACALD_ActiveFlag == true
                                      && a.ASMAY_Id == data.ASMAY_Id)
                                      select d).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
                }
                else
                {
                    data.getcourse = (from a in _examcontext.CLG_Adm_College_AY_CourseDMO
                                      from b in _examcontext.MasterCourseDMO
                                      where (a.AMCO_Id == b.AMCO_Id && a.ACAYC_ActiveFlag == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                      select b).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeCumulativeAvgBestReportDTO onchangecourse(CollegeCumulativeAvgBestReportDTO data)
        {
            try
            {
                var getempcode = _examcontext.Staff_User_Login.Where(a => a.Id == data.Userid).ToList();


                if (getempcode.Count > 0)
                {
                    data.getbranch = (from a in _examcontext.ClgMasterBranchDMO
                                      from c in _examcontext.AcademicYear
                                      from d in _examcontext.Adm_College_Atten_Login_DetailsDMO
                                      from e in _examcontext.Adm_College_Atten_Login_UserDMO
                                      where (d.AMB_Id == a.AMB_Id && e.ACALU_Id == d.ACALU_Id && a.MI_Id == data.MI_Id
                                      && e.HRME_Id == getempcode.FirstOrDefault().Emp_Code && d.AMCO_Id == data.AMCO_Id
                                      && e.ASMAY_Id == data.ASMAY_Id && d.ACALD_ActiveFlag == true)
                                      select a).Distinct().OrderBy(a => a.AMB_Order).ToArray();
                }
                else
                {
                    data.getbranch = (from a in _examcontext.CLG_Adm_College_AY_CourseDMO
                                      from b in _examcontext.MasterCourseDMO
                                      from c in _examcontext.CLG_Adm_College_AY_Course_BranchDMO
                                      from d in _examcontext.ClgMasterBranchDMO
                                      where (a.AMCO_Id == b.AMCO_Id && a.ACAYC_Id == c.ACAYC_Id && c.AMB_Id == d.AMB_Id && a.ACAYC_ActiveFlag == true
                                      && c.ACAYCB_ActiveFlag == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                      && a.AMCO_Id == data.AMCO_Id)
                                      select d).Distinct().OrderBy(a => a.AMB_Order).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeCumulativeAvgBestReportDTO onchangebranch(CollegeCumulativeAvgBestReportDTO data)
        {
            try
            {
                var getempcode = _examcontext.Staff_User_Login.Where(a => a.Id == data.Userid).ToList();


                if (getempcode.Count > 0)
                {
                    data.getsemester = (from a in _examcontext.ClgMasterBranchDMO
                                        from d in _examcontext.AcademicYear
                                        from e in _examcontext.Adm_College_Atten_Login_DetailsDMO
                                        from f in _examcontext.Adm_College_Atten_Login_UserDMO
                                        from g in _examcontext.CLG_Adm_Master_SemesterDMO
                                        where (a.AMB_Id == e.AMB_Id && f.ASMAY_Id == d.ASMAY_Id && e.ACALU_Id == f.ACALU_Id && e.AMSE_Id == g.AMSE_Id
                                        && a.MI_Id == data.MI_Id && e.AMCO_Id == data.AMCO_Id && f.HRME_Id == getempcode.FirstOrDefault().Emp_Code
                                        && f.ASMAY_Id == data.ASMAY_Id && e.ACALD_ActiveFlag == true && e.AMB_Id == data.AMB_Id)
                                        select g).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();
                }
                else
                {
                    data.getsemester = (from a in _examcontext.CLG_Adm_College_AY_CourseDMO
                                        from b in _examcontext.MasterCourseDMO
                                        from c in _examcontext.CLG_Adm_College_AY_Course_BranchDMO
                                        from d in _examcontext.ClgMasterBranchDMO
                                        from e in _examcontext.CLG_Adm_College_AY_Course_Branch_SemesterDMO
                                        from f in _examcontext.CLG_Adm_Master_SemesterDMO
                                        where (a.AMCO_Id == b.AMCO_Id && a.ACAYC_Id == c.ACAYC_Id && c.ACAYCB_Id == e.ACAYCB_Id
                                        && c.AMB_Id == d.AMB_Id && e.AMSE_Id == f.AMSE_Id && e.ACAYCBS_ActiveFlag == true && a.ACAYC_ActiveFlag == true
                                        && c.ACAYCB_ActiveFlag == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id
                                        && c.AMB_Id == data.AMB_Id)
                                        select f).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeCumulativeAvgBestReportDTO onchangesemester(CollegeCumulativeAvgBestReportDTO data)
        {
            try
            {
                var getempcode = _examcontext.Staff_User_Login.Where(a => a.Id == data.Userid).ToList();


                data.getsection = _examcontext.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMS_ActiveFlag == true).Distinct().OrderBy(a => a.ACMS_Order).ToArray();

                data.subjectshemalist = (from a in _examcontext.Exm_Col_Yearly_SchemeDMO
                                         from b in _examcontext.AdmCollegeSubjectSchemeDMO
                                         where (a.ACSS_Id == b.ACSS_Id && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id
                                         && a.AMSE_Id == data.AMSE_Id && a.ECYS_ActiveFlag == true && b.ACST_ActiveFlg == true)
                                         select b).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeCumulativeAvgBestReportDTO onchangesubjectscheme(CollegeCumulativeAvgBestReportDTO data)
        {
            try
            {
                var getempcode = _examcontext.Staff_User_Login.Where(a => a.Id == data.Userid).ToList();


                data.schmetypelist = (from a in _examcontext.Exm_Col_Yearly_SchemeDMO
                                      from b in _examcontext.AdmCollegeSubjectSchemeDMO
                                      from c in _examcontext.AdmCollegeSchemeTypeDMO
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
        public CollegeCumulativeAvgBestReportDTO onchangeschemetype(CollegeCumulativeAvgBestReportDTO data)
        {
            try
            {
                data.getexam = (from a in _examcontext.Exm_Col_Yearly_SchemeDMO
                                from b in _examcontext.Exm_Col_Yearly_Scheme_ExamsDMO
                                from c in _examcontext.col_exammasterDMO
                                where (a.ECYS_Id == b.ECYS_Id && b.EME_Id == c.EME_Id && c.MI_Id == data.MI_Id && a.MI_Id == data.MI_Id
                                && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id
                                && a.AMSE_Id == data.AMSE_Id && a.ACSS_Id == data.ACSS_Id && a.ACST_Id == data.ACST_Id && a.ECYS_ActiveFlag == true
                                && b.ECYSE_ActiveFlg == true && c.EME_ActiveFlag == true)
                                select c).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();


                var empcode_check = (from a in _examcontext.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.Id.Equals(data.Userid))
                                     select new ClgMarksEntryDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                if (empcode_check.Count > 0)
                {
                    data.getsubject = (from a in _examcontext.IVRM_School_Master_SubjectsDMO
                                       from b in _examcontext.ClgMasterBranchDMO
                                       from c in _examcontext.MasterCourseDMO
                                       from d in _examcontext.CLG_Adm_Master_SemesterDMO
                                       from e in _examcontext.Adm_College_Atten_Login_DetailsDMO
                                       from f in _examcontext.Adm_College_Atten_Login_UserDMO
                                       from g in _examcontext.AcademicYear
                                       from h in _examcontext.Adm_College_Master_SectionDMO
                                       where (a.ISMS_Id == e.ISMS_Id && b.AMB_Id == e.AMB_Id && c.AMCO_Id == e.AMCO_Id && e.ACMS_Id == h.ACMS_Id
                                       && f.ASMAY_Id == g.ASMAY_Id && e.ACALU_Id == f.ACALU_Id && e.AMB_Id == data.AMB_Id && e.AMCO_Id == data.AMCO_Id
                                       && e.ACMS_Id == data.ACMS_Id && e.AMSE_Id == data.AMSE_Id && a.MI_Id == data.MI_Id
                                       && f.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && f.ASMAY_Id == data.ASMAY_Id)
                                       select a).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();
                }
                else
                {
                    data.getsubject = (from a in _examcontext.Exm_Col_Yearly_SchemeDMO
                                       from b in _examcontext.Exm_Col_Yearly_Scheme_GroupDMO
                                       from c in _examcontext.Exm_Col_Yearly_Scheme_Group_SubjectsDMO
                                       from d in _examcontext.IVRM_School_Master_SubjectsDMO
                                       where (a.ECYS_Id == b.ECYS_Id && b.ECYSG_Id == c.ECYSG_Id && d.ISMS_Id == c.ISMS_Id && a.ECYS_ActiveFlag == true
                                       && b.ECYSG_ActiveFlag == true && c.ECYSGS_ActiveFlag == true && d.ISMS_ActiveFlag == 1 && a.AMCO_Id == data.AMCO_Id
                                       && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id && a.ACSS_Id == data.ACSS_Id && a.ACST_Id == data.ACST_Id)
                                       select d).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();
                }





            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CollegeCumulativeAvgBestReportDTO Getcmreport(CollegeCumulativeAvgBestReportDTO data)
        {
            try
            {
                data.configuration = _examcontext.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();

                data.instname = _examcontext.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();

                string emeid = "0";

                foreach (var c in data.examids)
                {
                    emeid = emeid + "," + c.EME_Id;
                }

                using (var cmd = _examcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "College_Exam_Cumulative_Avg_Best_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = data.AMB_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_ID", SqlDbType.VarChar) { Value = data.AMSE_Id });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.VarChar) { Value = data.ACMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = data.ISMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@FLAG", SqlDbType.VarChar) { Value = data.Flag });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = emeid });
                    cmd.Parameters.Add(new SqlParameter("@ACSS_Id", SqlDbType.VarChar) { Value = data.ACSS_Id });
                    cmd.Parameters.Add(new SqlParameter("@ACST_Id", SqlDbType.VarChar) { Value = data.ACST_Id });
                    cmd.Parameters.Add(new SqlParameter("@BEST", SqlDbType.VarChar) { Value = data.bestcount });

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

                        data.reportdata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        _logger.LogDebug(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeCumulativeAvgBestReportDTO getindreport(CollegeCumulativeAvgBestReportDTO data)
        {
            try
            {
                data.configuration = _examcontext.Exm_ConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();

                data.instname = _examcontext.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();

                //string emeid = "0";              

                using (var cmd = _examcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "College_Exam_Indi_SubjectWise_Cumulative_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.VarChar) { Value = data.AMCO_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.VarChar) { Value = data.AMB_Id });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_ID", SqlDbType.VarChar) { Value = data.AMSE_Id });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id", SqlDbType.VarChar) { Value = data.ACMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = data.ISMS_Id });                    
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = data.EME_Id });
                    cmd.Parameters.Add(new SqlParameter("@ACSS_Id", SqlDbType.VarChar) { Value = data.ACSS_Id });
                    cmd.Parameters.Add(new SqlParameter("@ACST_Id", SqlDbType.VarChar) { Value = data.ACST_Id });                  

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

                        data.reportinddata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        _logger.LogDebug(ex.Message);
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
