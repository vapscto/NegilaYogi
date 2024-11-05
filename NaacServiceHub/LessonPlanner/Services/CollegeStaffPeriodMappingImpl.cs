using DataAccessMsSqlServerProvider.com.vapstech.College.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model.com.vapstech.College.Exam.LessonPlanner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Exam.LessonPlanner;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.LessonPlanner.Services
{
    public class CollegeStaffPeriodMappingImpl : Interface.CollegeStaffPeriodMappingInterface
    {
        public LessonplannerContext _context;
        ILogger<CollegeStaffPeriodMappingImpl> _logg;

        public CollegeStaffPeriodMappingImpl(LessonplannerContext _contxt, ILogger<CollegeStaffPeriodMappingImpl> _log)
        {
            _context = _contxt;
            _logg = _log;
        }

        public CollegeStaffPeriodMappingDTO Getdetails(CollegeStaffPeriodMappingDTO data)
        {
            try
            {
                data.masteryear = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.employeedetails = (from e in _context.HR_Master_Employee_DMO
                                        where (e.MI_Id == data.MI_Id && e.HRME_LeftFlag == false && e.HRME_ActiveFlag == true)
                                        select new CollegeStaffPeriodMappingDTO
                                        {
                                            HRME_Id = e.HRME_Id,
                                            employeename = ((e.HRME_EmployeeFirstName == null ? " " : e.HRME_EmployeeFirstName) +
                                            (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == "" ? "" : " " + e.HRME_EmployeeMiddleName) +
                                            (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == "" ? "" : " : " + e.HRME_EmployeeCode)).Trim()
                                        }).Distinct().OrderBy(a => a.employeename).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeStaffPeriodMappingDTO getemployeedetails(CollegeStaffPeriodMappingDTO data)
        {
            try
            {
                data.mastercourse = (from a in _context.Adm_College_Atten_Login_UserDMO
                                     from b in _context.Adm_College_Atten_Login_DetailsDMO
                                     from c in _context.AcademicYear
                                     from d in _context.MasterCourseDMO
                                     where (a.ACALU_Id == b.ACALU_Id && a.ASMAY_Id == c.ASMAY_Id && b.AMCO_Id == d.AMCO_Id && a.MI_Id == data.MI_Id &&
                                     a.HRME_Id == data.HRME_Id && a.ASMAY_Id == data.ASMAY_Id && d.AMCO_ActiveFlag == true && b.ACALD_ActiveFlag == true)
                                     select d).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeStaffPeriodMappingDTO onchangecourse(CollegeStaffPeriodMappingDTO data)
        {
            try
            {
                data.masterbranch = (from a in _context.ClgMasterBranchDMO
                                     from d in _context.AcademicYear
                                     from e in _context.Adm_College_Atten_Login_DetailsDMO
                                     from f in _context.Adm_College_Atten_Login_UserDMO
                                     where (a.AMB_Id == e.AMB_Id && f.ASMAY_Id == d.ASMAY_Id && e.ACALU_Id == f.ACALU_Id && d.ASMAY_Id == data.ASMAY_Id
                                     && a.MI_Id == data.MI_Id && e.AMCO_Id == data.AMCO_Id && f.HRME_Id == data.HRME_Id)
                                     select a).Distinct().OrderBy(a => a.AMB_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeStaffPeriodMappingDTO onchangebranch(CollegeStaffPeriodMappingDTO data)
        {
            try
            {
                List<long> ambid = new List<long>();
                if (data.temp_branch_id != null && data.temp_branch_id.Length > 0)
                {
                    foreach (var c in data.temp_branch_id)
                    {
                        ambid.Add(c.AMB_Id);
                    }
                }
                else
                {
                    ambid.Add(data.AMB_Id);
                }

                data.mastersemester = (from a in _context.CLG_Adm_Master_SemesterDMO
                                       from c in _context.AcademicYear
                                       from d in _context.Adm_College_Atten_Login_DetailsDMO
                                       from e in _context.Adm_College_Atten_Login_UserDMO
                                       where (d.AMSE_Id == a.AMSE_Id && e.ACALU_Id == d.ACALU_Id && a.MI_Id == data.MI_Id
                                       && e.ASMAY_Id == data.ASMAY_Id && e.HRME_Id == data.HRME_Id && ambid.Contains(d.AMB_Id)
                                       && d.AMCO_Id == data.AMCO_Id)
                                       select a).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeStaffPeriodMappingDTO onchangesemster(CollegeStaffPeriodMappingDTO data)
        {
            try
            {
                List<long> ambid = new List<long>();

                if (data.temp_branch_id != null && data.temp_branch_id.Length > 0)
                {
                    foreach (var c in data.temp_branch_id)
                    {
                        ambid.Add(c.AMB_Id);
                    }
                }
                else
                {
                    ambid.Add(data.AMB_Id);
                }

                data.mastersection = (from a in _context.Adm_College_Master_SectionDMO
                                      from b in _context.Adm_College_Atten_Login_DetailsDMO
                                      from f in _context.Adm_College_Atten_Login_UserDMO
                                      from g in _context.AcademicYear
                                      where (a.ACMS_Id == b.ACMS_Id && b.AMB_Id == b.AMB_Id && b.AMCO_Id == b.AMCO_Id && b.AMSE_Id == b.AMSE_Id
                                      && f.ASMAY_Id == g.ASMAY_Id && ambid.Contains(b.AMB_Id) && b.AMCO_Id == data.AMCO_Id && b.ACALU_Id == f.ACALU_Id
                                      && b.AMSE_Id == data.AMSE_Id && a.MI_Id == data.MI_Id && g.ASMAY_Id == data.ASMAY_Id
                                      && f.HRME_Id == data.HRME_Id)
                                      select a).Distinct().OrderBy(a => a.ACMS_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeStaffPeriodMappingDTO onchangesection(CollegeStaffPeriodMappingDTO data)
        {
            try
            {
                List<long> ambid = new List<long>();

                if (data.temp_branch_id != null && data.temp_branch_id.Length > 0)
                {
                    foreach (var c in data.temp_branch_id)
                    {
                        ambid.Add(c.AMB_Id);
                    }
                }
                else
                {
                    ambid.Add(data.AMB_Id);
                }

                List<long> acmsid = new List<long>();

                if (data.temp_section_id != null && data.temp_section_id.Length > 0)
                {
                    foreach (var c in data.temp_section_id)
                    {
                        acmsid.Add(c.ACMS_Id);
                    }
                }
                else
                {
                    acmsid.Add(data.ACMS_Id);
                }

                data.mastersubjects = (from a in _context.IVRM_School_Master_SubjectsDMO
                                       from b in _context.ClgMasterBranchDMO
                                       from c in _context.MasterCourseDMO
                                       from d in _context.CLG_Adm_Master_SemesterDMO
                                       from e in _context.Adm_College_Atten_Login_DetailsDMO
                                       from f in _context.Adm_College_Atten_Login_UserDMO
                                       from g in _context.AcademicYear
                                       from h in _context.Adm_College_Master_SectionDMO
                                       where (a.ISMS_Id == e.ISMS_Id && b.AMB_Id == e.AMB_Id && c.AMCO_Id == e.AMCO_Id && e.ACMS_Id == h.ACMS_Id
                                       && f.ASMAY_Id == g.ASMAY_Id && e.ACALU_Id == f.ACALU_Id
                                       && ambid.Contains(e.AMB_Id) && e.AMCO_Id == data.AMCO_Id && acmsid.Contains(e.ACMS_Id)
                                       && e.AMSE_Id == data.AMSE_Id && a.MI_Id == data.MI_Id && f.ASMAY_Id == data.ASMAY_Id
                                       && f.HRME_Id == data.HRME_Id)
                                       select a).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeStaffPeriodMappingDTO getsearchdetails(CollegeStaffPeriodMappingDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Lesson_Planner_Get_All_Semester_Dates_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMCO_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMB_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMSE_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.ISMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id",
                    SqlDbType.VarChar)
                    {
                        Value = data.ACMS_Id
                    });

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
                        data.getalldates = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.gettopicdetails = (from a in _context.LP_Master_MainTopic_CollegeDMO
                                        from b in _context.LP_Master_Topic_CollegeDMO
                                        from c in _context.IVRM_School_Master_SubjectsDMO
                                        where (a.ISMS_Id == c.ISMS_Id && a.LPMMTC_Id == b.LPMMTC_Id && a.ISMS_Id == data.ISMS_Id
                                        && a.LPMMTC_ActiveFlg == true && b.LPMTC_Activefalg == true && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id
                                        && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id)
                                        select new CollegeStaffPeriodMappingDTO
                                        {
                                            LPMT_Id = b.LPMTC_Id,
                                            LPMT_TopicName = b.LPMTC_TopicName,
                                            LPMT_TopicOrder = b.LPMTC_TopicOrder
                                        }).Distinct().OrderBy(a => a.LPMT_TopicOrder).ToArray();


                data.getsavedetails = (from a in _context.CollegeStaffPeriodMappingDMO
                                       where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id
                                       && a.AMSE_Id == data.AMSE_Id && a.ACMS_Id == data.ACMS_Id && a.ISMS_Id == data.ISMS_Id && a.HRME_Id == data.HRME_Id)
                                       select new CollegeStaffPeriodMappingDTO
                                       {
                                           LPMT_Id = a.LPMTC_Id,
                                           LPLPC_LPDate = a.LPLPC_LPDate,
                                           LPLPC_Id = a.LPLPC_Id,
                                           LPLPC_ClassTakenFlg = a.LPLPC_ClassTakenFlg
                                       }).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeStaffPeriodMappingDTO savedata(CollegeStaffPeriodMappingDTO data)
        {
            try
            {

                if (data.CollegeStaffPeriodMappingTempDTO.Count() > 0)
                {
                    List<long> LPLPId = new List<long>();
                    for (int j1 = 0; j1 < data.CollegeStaffPeriodMappingTempDTO.Count(); j1++)
                    {
                        LPLPId.Add(data.CollegeStaffPeriodMappingTempDTO[j1].LPLPC_Id);
                    }

                    var reomvedetails = _context.CollegeStaffPeriodMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id && a.ACMS_Id == data.ACMS_Id && a.HRME_Id == data.HRME_Id
                    && !LPLPId.Contains(a.LPLPC_Id)).ToList();

                    for (int k1 = 0; k1 < reomvedetails.Count; k1++)
                    {
                        var removeresult = _context.CollegeStaffPeriodMappingDMO.Single(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                        && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id && a.ACMS_Id == data.ACMS_Id
                        && a.HRME_Id == data.HRME_Id && a.LPLPC_Id == reomvedetails[k1].LPLPC_Id);
                        _context.Remove(removeresult);
                    }

                    for (int j = 0; j < data.CollegeStaffPeriodMappingTempDTO.Count(); j++)
                    {
                        if (data.CollegeStaffPeriodMappingTempDTO[j].LPLPC_Id != 0)
                        {
                            var resultcheck = _context.CollegeStaffPeriodMappingDMO.Single(a => a.MI_Id == data.MI_Id
                            && a.LPLPC_Id == data.CollegeStaffPeriodMappingTempDTO[j].LPLPC_Id);
                            resultcheck.LPMTC_Id = Convert.ToInt64(data.CollegeStaffPeriodMappingTempDTO[j].LPMT_Id);
                            resultcheck.UpdatedDate = DateTime.Now;
                            resultcheck.LPMTC_UpdatedBy = data.LPCSWA_UpdatedBy;
                            _context.Update(resultcheck);
                        }
                        else
                        {
                            CollegeStaffPeriodMappingDMO dmo = new CollegeStaffPeriodMappingDMO();

                            dmo.MI_Id = data.MI_Id;
                            dmo.ASMAY_Id = data.ASMAY_Id;
                            dmo.AMCO_Id = data.AMCO_Id;
                            dmo.AMB_Id = data.AMB_Id;
                            dmo.AMSE_Id = data.AMSE_Id;
                            dmo.ACMS_Id = data.ACMS_Id;
                            dmo.HRME_Id = data.HRME_Id;
                            dmo.LPLPC_LPDate = Convert.ToDateTime(data.CollegeStaffPeriodMappingTempDTO[j].LPLPC_LPDate);
                            dmo.LPLPC_CTDate = DateTime.Now;
                            dmo.LPMTC_Id = Convert.ToInt64(data.CollegeStaffPeriodMappingTempDTO[j].LPMT_Id);
                            dmo.ISMS_Id = data.ISMS_Id;
                            dmo.LPLPC_ClassTakenFlg = false;
                            dmo.LPLPC_ActiveFlag = true;
                            dmo.CreatedDate = DateTime.Now;
                            dmo.UpdatedDate = DateTime.Now;
                            dmo.LPMTC_CreatedBy = data.Userid;
                            dmo.LPMTC_UpdatedBy = data.Userid;

                            _context.Add(dmo);
                        }
                    }

                    int k = _context.SaveChanges();
                    if (k > 0)
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

        // Staff Transaction 
        public CollegeStaffPeriodMappingDTO Getdetailstransaction(CollegeStaffPeriodMappingDTO data)
        {
            try
            {
                data.masteryear = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                var check_rolename = (from a in _context.MasterRoleType
                                      where (a.IVRMRT_Id == data.roleId)
                                      select new CollegeStaffPeriodMappingDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }).ToList();

                var empcode_check = (from a in _context.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.IVRMSTAUL_UserName.Equals(data.username.Trim()))
                                     select new CollegeStaffPeriodMappingDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();


                data.employeedetails = (from e in _context.HR_Master_Employee_DMO
                                        where (e.MI_Id == data.MI_Id && e.HRME_LeftFlag == false && e.HRME_ActiveFlag == true && e.HRME_Id == empcode_check.FirstOrDefault().Emp_Code)
                                        select new CollegeStaffPeriodMappingDTO
                                        {
                                            HRME_Id = e.HRME_Id,
                                            employeename = ((e.HRME_EmployeeFirstName == null ? " " : e.HRME_EmployeeFirstName) +
                                            (e.HRME_EmployeeMiddleName == null || e.HRME_EmployeeMiddleName == "" ? "" : " " + e.HRME_EmployeeMiddleName) +
                                            (e.HRME_EmployeeLastName == null || e.HRME_EmployeeLastName == "" ? "" : " : " + e.HRME_EmployeeCode)).Trim()
                                        }).Distinct().OrderBy(a => a.employeename).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeStaffPeriodMappingDTO getsearchdetailstransaction(CollegeStaffPeriodMappingDTO data)
        {
            try
            {
                string ambid = "0";

                if (data.temp_branch_id.Length > 0)
                {
                    foreach (var c in data.temp_branch_id)
                    {
                        ambid = ambid + "," + c.AMB_Id;
                    }
                }

                string acmsid = "0";

                if (data.temp_section_id.Length > 0)
                {
                    foreach (var c in data.temp_section_id)
                    {
                        acmsid = acmsid + "," + c.ACMS_Id;
                    }
                }

                List<long> ambidnew = new List<long>();

                if (data.temp_branch_id.Length > 0)
                {
                    foreach (var c in data.temp_branch_id)
                    {
                        ambidnew.Add(c.AMB_Id);
                    }
                }

                List<long> acmsidnew = new List<long>();

                if (data.temp_section_id.Length > 0)
                {
                    foreach (var c in data.temp_section_id)
                    {
                        acmsidnew.Add(c.ACMS_Id);
                    }
                }

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Lesson_Planner_Get_All_Semester_Dates_Transaction_New";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMCO_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                       SqlDbType.VarChar)
                    {
                        Value = ambid
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMSE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id",
                       SqlDbType.VarChar)
                    {
                        Value = acmsid
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Id",
                     SqlDbType.VarChar)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id",
                   SqlDbType.VarChar)
                    {
                        Value = data.ISMS_Id
                    });

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
                        data.getalldates = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.gettopicdetails = (from a in _context.LP_Master_MainTopic_CollegeDMO
                                        from b in _context.LP_Master_Topic_CollegeDMO
                                        from c in _context.IVRM_School_Master_SubjectsDMO
                                        where (a.ISMS_Id == c.ISMS_Id && a.LPMMTC_Id == b.LPMMTC_Id && a.ISMS_Id == data.ISMS_Id
                                        && a.LPMMTC_ActiveFlg == true && b.LPMTC_Activefalg == true && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id
                                        && ambidnew.Contains(a.AMB_Id) && a.AMSE_Id == data.AMSE_Id)
                                        select new CollegeStaffPeriodMappingDTO
                                        {
                                            LPMT_Id = b.LPMTC_Id,
                                            LPMT_TopicName = b.LPMTC_TopicName,
                                            LPMT_TopicOrder = b.LPMTC_TopicOrder
                                        }).Distinct().OrderBy(a => a.LPMT_TopicOrder).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeStaffPeriodMappingDTO savedatatransaction(CollegeStaffPeriodMappingDTO data)
        {
            try
            {
                if (data.CollegeStaffPeriodMappingsavingTempDTO.Count() > 0)
                {
                    for (int k = 0; k < data.CollegeStaffPeriodMappingsavingTempDTO.Count(); k++)
                    {
                        var result = _context.CollegeStaffPeriodMappingDMO.Single(a => a.MI_Id == data.MI_Id && a.LPLPC_Id == data.CollegeStaffPeriodMappingsavingTempDTO[k].LPLPC_Id);
                        result.LPLPC_CTDate = Convert.ToDateTime(data.CollegeStaffPeriodMappingsavingTempDTO[k].LPLPC_CTDate);
                        result.LPMTC_UpdatedBy = data.Userid;
                        result.LPLPC_ClassTakenFlg = true;
                        result.UpdatedDate = DateTime.Now;
                        _context.Update(result);
                    }
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
            }
            catch (Exception ex)
            {
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}
