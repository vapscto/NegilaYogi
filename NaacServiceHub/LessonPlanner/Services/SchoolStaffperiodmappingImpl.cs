using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model.com.vapstech.Exam.LessonPlanner;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Exam.LessonPlanner;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.com.vaps.LessonPlanner.Services
{
    public class SchoolStaffperiodmappingImpl : Interface.SchoolStaffperiodmappingInterface
    {
        public LessonplannerContext _context;

        public SchoolStaffperiodmappingImpl(LessonplannerContext context)
        {
            _context = context;
        }

        public SchoolStaffperiodmappingDTO Getdetails(SchoolStaffperiodmappingDTO data)
        {
            try
            {
                data.masteryear = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();


                data.employeedetails = (from e in _context.HR_Master_Employee_DMO
                                        where (e.MI_Id == data.MI_Id && e.HRME_LeftFlag == false && e.HRME_ActiveFlag == true)
                                        select new SchoolStaffperiodmappingDTO
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
        public SchoolStaffperiodmappingDTO getemployeedetails(SchoolStaffperiodmappingDTO data)
        {
            try
            {
                data.masterclass = (from a in _context.AcademicYear
                                    from b in _context.AdmissionClass
                                    from c in _context.HR_Master_Employee_DMO
                                    from d in _context.Staff_User_Login
                                    from e in _context.Exm_Login_PrivilegeDMO
                                    from f in _context.Exm_Login_Privilege_SubjectsDMO
                                    where (a.ASMAY_Id == e.ASMAY_Id && b.ASMCL_Id == f.ASMCL_Id
                                    && c.HRME_Id == d.Emp_Code && e.Login_Id == d.IVRMSTAUL_Id
                                    && e.ELP_Id == f.ELP_Id && a.MI_Id == data.MI_Id
                                    && e.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id
                                    && d.Emp_Code == data.HRME_Id && e.ELP_ActiveFlg == true
                                    && f.ELPs_ActiveFlg == true)
                                    select b).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SchoolStaffperiodmappingDTO onchangeclass(SchoolStaffperiodmappingDTO data)
        {
            try
            {
                data.mastersection = (from a in _context.AcademicYear
                                      from b in _context.AdmissionClass
                                      from c in _context.HR_Master_Employee_DMO
                                      from d in _context.Staff_User_Login
                                      from e in _context.Exm_Login_PrivilegeDMO
                                      from f in _context.Exm_Login_Privilege_SubjectsDMO
                                      from g in _context.School_M_Section
                                      where (a.ASMAY_Id == e.ASMAY_Id && b.ASMCL_Id == f.ASMCL_Id
                                      && c.HRME_Id == d.Emp_Code && e.Login_Id == d.IVRMSTAUL_Id
                                      && e.ELP_Id == f.ELP_Id && g.ASMS_Id == f.ASMS_Id
                                      && f.ASMCL_Id == data.ASMCL_Id && a.MI_Id == data.MI_Id
                                      && e.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id
                                      && d.Emp_Code == data.HRME_Id && e.ELP_ActiveFlg == true
                                      && f.ELPs_ActiveFlg == true)
                                      select g).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SchoolStaffperiodmappingDTO onchangesection(SchoolStaffperiodmappingDTO data)
        {
            try
            {
                data.mastersubjects = (from a in _context.AcademicYear
                                       from b in _context.AdmissionClass
                                       from c in _context.HR_Master_Employee_DMO
                                       from d in _context.Staff_User_Login
                                       from e in _context.Exm_Login_PrivilegeDMO
                                       from f in _context.Exm_Login_Privilege_SubjectsDMO
                                       from g in _context.School_M_Section
                                       from h in _context.IVRM_School_Master_SubjectsDMO
                                       where (a.ASMAY_Id == e.ASMAY_Id && b.ASMCL_Id == f.ASMCL_Id
                                       && c.HRME_Id == d.Emp_Code && e.Login_Id == d.IVRMSTAUL_Id
                                       && e.ELP_Id == f.ELP_Id && g.ASMS_Id == f.ASMS_Id
                                       && h.ISMS_Id == f.ISMS_Id && h.ISMS_ActiveFlag == 1
                                       && f.ASMCL_Id == data.ASMCL_Id && a.MI_Id == data.MI_Id
                                       && e.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id
                                       && d.Emp_Code == data.HRME_Id && f.ASMS_Id == data.ASMS_Id
                                       && e.ELP_ActiveFlg == true && f.ELPs_ActiveFlg == true)
                                       select h).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SchoolStaffperiodmappingDTO getsearchdetails(SchoolStaffperiodmappingDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "School_Lesson_Planner_Get_All_Semester_Dates";
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

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
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


                data.gettopicdetails = (from a in _context.MasterSchoolTopicDMO
                                        from b in _context.SchoolSubjectWithMasterTopicMapping
                                        from c in _context.IVRM_School_Master_SubjectsDMO
                                        where (a.ISMS_Id == c.ISMS_Id && a.LPMMT_Id == b.LPMMT_Id && a.ISMS_Id == data.ISMS_Id
                                        && a.LPMMT_ActiveFlag == true && b.LPMT_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id)
                                        select new SchoolStaffperiodmappingDTO
                                        {
                                            LPMT_Id = b.LPMT_Id,
                                            LPMT_TopicName = b.LPMT_TopicName,
                                            LPMT_TopicOrder = b.LPMT_TopicOrder,
                                            LPMMT_Order = a.LPMMT_Order
                                        }).Distinct().OrderBy(a => a.LPMMT_Order).ThenBy(b => b.LPMT_TopicOrder).ToArray();

                data.getsavedetails = (from a in _context.SchoolStaffperiodmappingDMO
                                       where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                       && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id
                                       && a.ISMS_Id == data.ISMS_Id && a.HRME_Id == data.HRME_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id)
                                       select new SchoolStaffperiodmappingDTO
                                       {
                                           LPMT_Id = a.LPMT_Id,
                                           LPLP_LPDate = a.LPLP_LPDate,
                                           LPLP_Id = a.LPLP_Id,
                                           LPLP_ClassTakenFlg = a.LPLP_ClassTakenFlg
                                       }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SchoolStaffperiodmappingDTO savedata(SchoolStaffperiodmappingDTO data)
        {
            try
            {
                if (data.SchoolStaffPeriodMappingTempDTO.Count() > 0)
                {
                    List<long> LPLPId = new List<long>();
                    for (int j1 = 0; j1 < data.SchoolStaffPeriodMappingTempDTO.Count(); j1++)
                    {
                        LPLPId.Add(data.SchoolStaffPeriodMappingTempDTO[j1].LPLP_Id);
                    }

                    var reomvedetails = _context.SchoolStaffperiodmappingDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.HRME_Id == data.HRME_Id && !LPLPId.Contains(a.LPLP_Id)).ToList();

                    for (int k1 = 0; k1 < reomvedetails.Count; k1++)
                    {
                        var removeresult = _context.SchoolStaffperiodmappingDMO.Single(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id 
                        && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.HRME_Id == data.HRME_Id && a.LPLP_Id == reomvedetails[k1].LPLP_Id);
                        _context.Remove(removeresult);
                    }


                    for (int j = 0; j < data.SchoolStaffPeriodMappingTempDTO.Count(); j++)
                    {

                        if (data.SchoolStaffPeriodMappingTempDTO[j].LPLP_Id != 0)
                        {
                            var resultcheck = _context.SchoolStaffperiodmappingDMO.Single(a => a.MI_Id == data.MI_Id && a.LPLP_Id == data.SchoolStaffPeriodMappingTempDTO[j].LPLP_Id);
                            resultcheck.LPMT_Id = Convert.ToInt64(data.SchoolStaffPeriodMappingTempDTO[j].LPMT_Id);
                            resultcheck.UpdatedDate = DateTime.Now;
                            resultcheck.LPMT_UpdatedBy = data.Userid;
                            _context.Update(resultcheck);
                        }
                        else
                        {
                            SchoolStaffperiodmappingDMO dmo = new SchoolStaffperiodmappingDMO();

                            dmo.MI_Id = data.MI_Id;
                            dmo.ASMAY_Id = data.ASMAY_Id;
                            dmo.ASMCL_Id = data.ASMCL_Id;
                            dmo.ASMS_Id = data.ASMS_Id;
                            dmo.HRME_Id = data.HRME_Id;
                            dmo.LPLP_LPDate = Convert.ToDateTime(data.SchoolStaffPeriodMappingTempDTO[j].LPLP_LPDate);
                            dmo.LPLP_CTDate = DateTime.Now;
                            dmo.LPMT_Id = Convert.ToInt64(data.SchoolStaffPeriodMappingTempDTO[j].LPMT_Id);
                            dmo.ISMS_Id = data.ISMS_Id;
                            dmo.LPLP_ClassTakenFlg = false;
                            dmo.LPLP_ActiveFlag = true;
                            dmo.CreatedDate = DateTime.Now;
                            dmo.UpdatedDate = DateTime.Now;
                            dmo.LPMT_CreatedBy = data.Userid;
                            dmo.LPMT_UpdatedBy = data.Userid;

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
            }
            return data;
        }

        public SchoolStaffperiodmappingDTO Getdetailstransaction(SchoolStaffperiodmappingDTO data)
        {
            try
            {
                data.masteryear = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                var check_rolename = (from a in _context.MasterRoleType
                                      where (a.IVRMRT_Id == data.roleId)
                                      select new SchoolStaffperiodmappingDTO
                                      {
                                          rolename = a.IVRMRT_Role,
                                      }).ToList();

                var empcode_check = (from a in _context.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.IVRMSTAUL_UserName.Equals(data.username.Trim()))
                                     select new SchoolStaffperiodmappingDTO
                                     {
                                         HRME_Id = a.Emp_Code,
                                     }).ToList();


                data.employeedetails = (from e in _context.HR_Master_Employee_DMO
                                        where (e.MI_Id == data.MI_Id && e.HRME_LeftFlag == false && e.HRME_ActiveFlag == true && e.HRME_Id == empcode_check.FirstOrDefault().HRME_Id)
                                        select new SchoolStaffperiodmappingDTO
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
        public SchoolStaffperiodmappingDTO getsearchdetailstransaction(SchoolStaffperiodmappingDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "School_Lesson_Planner_Get_All_Semester_Dates_Transaction";
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

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
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

                data.gettopicdetails = (from a in _context.MasterSchoolTopicDMO
                                        from b in _context.SchoolSubjectWithMasterTopicMapping
                                        from c in _context.IVRM_School_Master_SubjectsDMO
                                        where (a.ISMS_Id == c.ISMS_Id && a.LPMMT_Id == b.LPMMT_Id && a.ISMS_Id == data.ISMS_Id
                                        && a.LPMMT_ActiveFlag == true && b.LPMT_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id)
                                        select new SchoolStaffperiodmappingDTO
                                        {
                                            LPMT_Id = b.LPMT_Id,
                                            LPMT_TopicName = b.LPMT_TopicName,
                                            LPMT_TopicOrder = b.LPMT_TopicOrder
                                        }).Distinct().OrderBy(a => a.LPMT_TopicOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SchoolStaffperiodmappingDTO savedatatransaction(SchoolStaffperiodmappingDTO data)
        {
            try
            {
                if (data.SchoolStaffPeriodMappingsavingTempDTO.Count() > 0)
                {
                    for (int k = 0; k < data.SchoolStaffPeriodMappingsavingTempDTO.Count(); k++)
                    {
                        var result = _context.SchoolStaffperiodmappingDMO.Single(a => a.MI_Id == data.MI_Id && a.LPLP_Id == data.SchoolStaffPeriodMappingsavingTempDTO[k].LPLP_Id);
                        result.LPLP_CTDate = Convert.ToDateTime(data.SchoolStaffPeriodMappingsavingTempDTO[k].LPLP_CTDate);
                        result.LPMT_UpdatedBy = data.Userid;
                        result.LPLP_ClassTakenFlg = true;
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
