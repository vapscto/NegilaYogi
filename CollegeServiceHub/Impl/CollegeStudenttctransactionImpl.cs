using CommonLibrary;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Admission;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Impl
{
    public class CollegeStudenttctransactionImpl : Interface.CollegeStudenttctransactionInterface
    {
        public ClgAdmissionContext _ClgAdmissionContext;

        public CollegeStudenttctransactionImpl(ClgAdmissionContext _clgad)
        {
            _ClgAdmissionContext = _clgad;
        }
        public CollegeStudenttctransactionDTO loaddata(CollegeStudenttctransactionDTO data)
        {
            try
            {
                data.getyear = _ClgAdmissionContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.getconfigurationdetails = _ClgAdmissionContext.AdmissionStandardDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();

                data.admTransNumSetting = _ClgAdmissionContext.Master_Numbering.Where(t => t.MI_Id == data.MI_Id && t.IMN_Flag.Equals("tcno")).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }
        public CollegeStudenttctransactionDTO onchangeyear(CollegeStudenttctransactionDTO data)
        {
            try
            {
                data.getcourse = (from a in _ClgAdmissionContext.MasterCourseDMO
                                  from b in _ClgAdmissionContext.CLG_Adm_College_AY_CourseDMO
                                  where (a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == a.AMCO_Id)
                                  select a).Distinct().OrderBy(t => t.AMCO_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }
        public CollegeStudenttctransactionDTO onchangecourse(CollegeStudenttctransactionDTO data)
        {
            try
            {
                var branchlist = (from a in _ClgAdmissionContext.ClgMasterBranchDMO
                                  from b in _ClgAdmissionContext.CLG_Adm_College_AY_CourseDMO
                                  from c in _ClgAdmissionContext.CLG_Adm_College_AY_Course_BranchDMO
                                  where (a.MI_Id == data.MI_Id && a.AMB_ActiveFlag && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ACAYC_ActiveFlag && b.AMCO_Id == data.AMCO_Id && c.ACAYC_Id == b.ACAYC_Id && c.MI_Id == data.MI_Id && c.AMB_Id == a.AMB_Id && c.ACAYCB_ActiveFlag)
                                  select new ClgMasterBranchDMO
                                  {
                                      AMB_Id = a.AMB_Id,
                                      AMB_BranchName = a.AMB_BranchName,
                                      AMB_BranchCode = a.AMB_BranchCode,
                                      AMB_BranchInfo = a.AMB_BranchInfo,
                                      AMB_BranchType = a.AMB_BranchType,
                                      AMB_StudentCapacity = a.AMB_StudentCapacity,
                                      AMB_Order = a.AMB_Order,
                                      AMB_AidedUnAided = a.AMB_AidedUnAided
                                  }).Distinct().ToList();
                data.getbranch = branchlist.OrderBy(t => t.AMB_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }
        public CollegeStudenttctransactionDTO onchangebranch(CollegeStudenttctransactionDTO data)
        {
            try
            {
                using (var cmd = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Get_Present_Semester_Details";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
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
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();

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
                        data.getsemester = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }
        public CollegeStudenttctransactionDTO onchangesemester(CollegeStudenttctransactionDTO data)
        {
            try
            {
                data.getssection = (from a in _ClgAdmissionContext.Adm_College_Yearly_StudentDMO
                                    from b in _ClgAdmissionContext.Adm_College_Master_SectionDMO
                                    where (a.ACMS_Id == b.ACMS_Id && a.AMCO_Id == data.AMCO_Id && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id
                                    && a.ASMAY_Id == data.ASMAY_Id)
                                    select b).Distinct().OrderBy(a => a.ACMS_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }
        public CollegeStudenttctransactionDTO onchangesection(CollegeStudenttctransactionDTO data)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }
        public CollegeStudenttctransactionDTO searchfilter(CollegeStudenttctransactionDTO data)
        {
            try
            {
                data.searchfilter = data.searchfilter.ToUpper();
                bool activeflag = false;
                int activeflagyear = 0;

                if (data.AMCST_SOL == "S")
                {
                    activeflag = true;
                    activeflagyear = 1;
                }
                else if (data.AMCST_SOL == "L")
                {
                    activeflag = false;
                    activeflagyear = 0;
                }
                else if (data.AMCST_SOL == "D")
                {
                    activeflag = true;
                    activeflagyear = 1;
                }
                else if (data.AMCST_SOL == "T")
                {
                    activeflag = true;
                    activeflagyear = 1;
                }



                if (data.allorindividual == "All")
                {
                    if (data.adm_num_flag == "A")
                    {
                        data.getstudentlist = (from a in _ClgAdmissionContext.Adm_Master_College_StudentDMO
                                               from b in _ClgAdmissionContext.Adm_College_Yearly_StudentDMO
                                               where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id 
                                               && a.AMCST_SOL.Equals(data.AMCST_SOL) && a.AMCST_ActiveFlag == activeflag && b.ACYST_ActiveFlag == activeflagyear 
                                               && (a.AMCST_AdmNo.StartsWith(data.searchfilter)))
                                               select new CollegeStudenttctransactionDTO
                                               {
                                                   AMCST_Id = a.AMCST_Id,
                                                   studentName = (a.AMCST_AdmNo + ':' + (a.AMCST_FirstName == null ? "" : a.AMCST_FirstName) +
                                                   (a.AMCST_MiddleName == null || a.AMCST_MiddleName == "" ? "" : " " + a.AMCST_MiddleName) +
                                                   (a.AMCST_LastName == null || a.AMCST_LastName == "" ? "" : " " + a.AMCST_LastName)).Trim(),
                                               }).Distinct().ToArray();


                    }
                    else
                    {
                        data.getstudentlist = (from a in _ClgAdmissionContext.Adm_Master_College_StudentDMO
                                               from b in _ClgAdmissionContext.Adm_College_Yearly_StudentDMO
                                               where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                               && a.AMCST_SOL.Equals(data.AMCST_SOL) && a.AMCST_ActiveFlag == activeflag && b.ACYST_ActiveFlag == activeflagyear
                                               && ((a.AMCST_FirstName.Trim().ToUpper() + ' ' + a.AMCST_MiddleName.Trim().ToUpper() + ' '
                                               + a.AMCST_LastName.Trim().ToUpper()).Contains(data.searchfilter)
                                               || a.AMCST_FirstName.Trim().ToUpper().StartsWith(data.searchfilter)
                                               || a.AMCST_MiddleName.Trim().ToUpper().StartsWith(data.searchfilter)
                                               || a.AMCST_LastName.Trim().ToUpper().StartsWith(data.searchfilter)))
                                               select new CollegeStudenttctransactionDTO
                                               {
                                                   AMCST_Id = a.AMCST_Id,
                                                   studentName = ((a.AMCST_FirstName == null ? "" : a.AMCST_FirstName) +
                                                   (a.AMCST_MiddleName == null || a.AMCST_MiddleName == "" ? "" : " " + a.AMCST_MiddleName) +
                                                   (a.AMCST_LastName == null || a.AMCST_LastName == "" ? "" : " " + a.AMCST_LastName) +
                                                    (a.AMCST_AdmNo == null || a.AMCST_AdmNo == "" ? "" : " : " + a.AMCST_AdmNo)).Trim(),
                                               }).Distinct().ToArray();

                    }
                }
                else
                {
                    if (data.adm_num_flag == "A")
                    {
                        data.getstudentlist = (from a in _ClgAdmissionContext.Adm_Master_College_StudentDMO
                                               from b in _ClgAdmissionContext.Adm_College_Yearly_StudentDMO
                                               where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                               && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && b.AMSE_Id == data.AMSE_Id
                                               && b.ACMS_Id == data.ACMS_Id && a.AMCST_SOL.Equals(data.AMCST_SOL) && a.AMCST_ActiveFlag == activeflag
                                               && b.ACYST_ActiveFlag == activeflagyear && (a.AMCST_AdmNo.StartsWith(data.searchfilter)))
                                               select new CollegeStudenttctransactionDTO
                                               {
                                                   AMCST_Id = a.AMCST_Id,
                                                   studentName = (a.AMCST_AdmNo + ':' + (a.AMCST_FirstName == null ? "" : a.AMCST_FirstName) +
                                                   (a.AMCST_MiddleName == null || a.AMCST_MiddleName == "" ? "" : " " + a.AMCST_MiddleName) +
                                                   (a.AMCST_LastName == null || a.AMCST_LastName == "" ? "" : " " + a.AMCST_LastName)).Trim(),
                                               }).Distinct().ToArray();


                    }
                    else
                    {
                        data.getstudentlist = (from a in _ClgAdmissionContext.Adm_Master_College_StudentDMO
                                               from b in _ClgAdmissionContext.Adm_College_Yearly_StudentDMO
                                               where (a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                               && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && b.AMSE_Id == data.AMSE_Id
                                               && b.ACMS_Id == data.ACMS_Id && a.AMCST_SOL.Equals(data.AMCST_SOL) && a.AMCST_ActiveFlag == activeflag
                                               && b.ACYST_ActiveFlag == activeflagyear
                                               && ((a.AMCST_FirstName.Trim().ToUpper() + ' ' + a.AMCST_MiddleName.Trim().ToUpper() + ' '
                                               + a.AMCST_LastName.Trim().ToUpper()).Contains(data.searchfilter)
                                               || a.AMCST_FirstName.Trim().ToUpper().StartsWith(data.searchfilter)
                                               || a.AMCST_MiddleName.Trim().ToUpper().StartsWith(data.searchfilter)
                                               || a.AMCST_LastName.Trim().ToUpper().StartsWith(data.searchfilter)))
                                               select new CollegeStudenttctransactionDTO
                                               {
                                                   AMCST_Id = a.AMCST_Id,
                                                   studentName = ((a.AMCST_FirstName == null ? "" : a.AMCST_FirstName) +
                                                   (a.AMCST_MiddleName == null || a.AMCST_MiddleName == "" ? "" : " " + a.AMCST_MiddleName) +
                                                   (a.AMCST_LastName == null || a.AMCST_LastName == "" ? "" : " " + a.AMCST_LastName) +
                                                    (a.AMCST_AdmNo == null || a.AMCST_AdmNo == "" ? "" : " : " + a.AMCST_AdmNo)).Trim(),
                                               }).Distinct().ToArray();

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeStudenttctransactionDTO onchangestudent(CollegeStudenttctransactionDTO data)
        {
            try
            {
                using (var cmd = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_Student_TC_Details";
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
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMCST_SOL
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMCST_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();

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
                        data.getstudentdetails = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                using (var cmd = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_student_TC_Attendance_Details";
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
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMCST_SOL
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMCST_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();

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
                        data.getstudentattendancedetails = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                using (var cmd = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_student_TC_Fee_Exam_Details";
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
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMCST_SOL
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMCST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAGNEW",
                   SqlDbType.VarChar)
                    {
                        Value = 'F'
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();

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
                        data.getstudentfeedetails = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }

                using (var cmd = _ClgAdmissionContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "College_student_TC_Fee_Exam_Details";
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
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMCST_SOL
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                       SqlDbType.VarChar)
                    {
                        Value = data.AMCST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAGNEW",
                   SqlDbType.VarChar)
                    {
                        Value = 'E'
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();

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
                        data.getstudentlanguagesubjects = retObject.ToArray();

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }
        public CollegeStudenttctransactionDTO chk_dup_tc(CollegeStudenttctransactionDTO data)
        {
            try
            {
                var checkduplicate = _ClgAdmissionContext.CollegeStudenttctransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.ACSTC_TCNO.Equals(data.ACSTC_TCNO)).Count();
                if (checkduplicate > 0)
                {
                    data.message = "Exists";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeStudenttctransactionDTO savetc(CollegeStudenttctransactionDTO data)
        {
            try
            {
                //var Tc_Amst_Count = 0;

                var checkcount = _ClgAdmissionContext.CollegeStudenttctransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id).Count();

                if (checkcount > 0)
                {
                    var result = _ClgAdmissionContext.CollegeStudenttctransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id).Single();

                    if (result.ACSTC_ActiveFlag == "T")
                    {
                        var result1 = _ClgAdmissionContext.CollegeStudenttctransactionDMO.Single(a => a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id);

                        result.ACSTC_ActiveFlag = "L";
                        result.ACSTC_TemporaryFlag = 0;
                        result.UpdatedDate = DateTime.Now;
                        result.ACSTC_AttendedDays = Convert.ToInt64(data.ACSTC_AttendedDays);
                        result.ACSTC_Conduct = data.ACSTC_Conduct;

                        result.ACSTC_WorkingDays = data.ACSTC_WorkingDays;
                        result.ACSTC_AttendedDays = data.ACSTC_AttendedDays;
                        result.ACSTC_TCNO = data.ACSTC_TCNO;
                        result.ACSTC_Scholarship = data.ACSTC_Scholarship;
                        result.ACSTC_LanguageStudied = data.ACSTC_LanguageStudied;
                        result.ACSTC_MediumOfINStruction = data.ACSTC_MediumOfINStruction;
                        result.ACSTC_ElectivesStudied = data.ACSTC_ElectivesStudied;
                        result.ACSTC_MedicallyExam = data.ACSTC_MedicallyExam;
                        result.ACSTC_Qual_PromotionFlag = data.ACSTC_Qual_PromotionFlag;
                        result.ACSTC_Qual_Course = data.ACSTC_Qual_Course;
                        result.ACSTC_FeePaid = data.ACSTC_FeePaid;
                        result.ACSTC_FeeConcession = data.ACSTC_FeeConcession;
                        result.ACSTC_Conduct = data.ACSTC_Conduct;
                        result.ACSTC_LeavingReason = data.ACSTC_LeavingReason;
                        result.ACSTC_Result = data.ACSTC_Result;
                        result.ACSTC_ResultDetails = data.ACSTC_ResultDetails;
                        result.ACSTC_NCCDetails = data.ACSTC_NCCDetails;
                        result.ACSTC_LastExamDetails = data.ACSTC_LastExamDetails;
                        result.ACSTC_ExtraActivities = data.ACSTC_ExtraActivities;
                        result.ACSTC_Remarks = data.ACSTC_Remarks;
                        result.Last_Course_Studied = data.Last_Course_Studied;
                        result.Caste = data.Caste;
                        result.Fee_Due_Amnt = data.Fee_Due_Amnt;
                        result.Library_Due_Amnt = data.Library_Due_Amnt;
                        result.Store_Canteen_Due = data.Store_Canteen_Due;
                        result.PDA_Due = data.PDA_Due;
                        result.Admission_Date = data.Admission_Date;
                        result.ACSTC_TCApplicationDate = data.ACSTC_TCApplicationDate;
                        result.ACSTC_TCDate = data.ACSTC_TCDate;
                        result.ACSTC_TCIssueDate = data.ACSTC_TCIssueDate;
                        result.ACSTC_LastAttendedDate = data.ACSTC_LastAttendedDate;
                        result.ACSTC_PromotionDate = data.ACSTC_PromotionDate;             

                        _ClgAdmissionContext.CollegeStudenttctransactionDMO.Update(result);


                        var update_student = _ClgAdmissionContext.Adm_Master_College_StudentDMO.Single(a => a.MI_Id == data.MI_Id && a.AMCST_Id == data.AMCST_Id);
                        update_student.AMCST_SOL = "L";
                        update_student.AMCST_ActiveFlag = false;
                        update_student.UpdatedDate = DateTime.UtcNow;
                        _ClgAdmissionContext.Update(update_student);

                        var update_ystudent = _ClgAdmissionContext.Adm_College_Yearly_StudentDMO.Single(a => a.AMCST_Id == data.AMCST_Id && a.ASMAY_Id == data.ASMAY_Id && a.ACYST_ActiveFlag == 1);

                        update_ystudent.ACYST_ActiveFlag = 0;
                        update_ystudent.UpdatedDate = DateTime.UtcNow;
                        _ClgAdmissionContext.Update(update_ystudent);

                        var k = _ClgAdmissionContext.SaveChanges();
                        if (k > 0)
                        {
                            data.message = "Update";
                            data.returnval = true;


                            var getstdappid = _ClgAdmissionContext.CollegeStudentlogin.Where(a => a.AMCST_Id == data.AMCST_Id).Select(a => a.IVRMUL_Id).ToList();

                            for (int dk = 0; dk < getstdappid.Count(); dk++)
                            {
                                var chckuser = _ClgAdmissionContext.UserRoleWithInstituteDMO.Where(a => a.MI_Id == data.MI_Id && a.Id == getstdappid[dk]).ToList();

                                if (chckuser.Count() > 0)
                                {
                                    var updatecheckuser = _ClgAdmissionContext.UserRoleWithInstituteDMO.Single(a => a.MI_Id == data.MI_Id && a.Id == getstdappid[dk]);

                                    updatecheckuser.Activeflag = 0;
                                    _ClgAdmissionContext.Update(updatecheckuser);
                                    var i = _ClgAdmissionContext.SaveChanges();
                                    if (i > 0)
                                    {
                                        data.message = "Update";
                                        data.returnval = true;
                                    }
                                    else
                                    {
                                        data.message = "Update";
                                        data.returnval = true;
                                    }
                                }
                            }

                            try
                            {
                                var outputval =  _ClgAdmissionContext.Database.ExecuteSqlCommand("CLG_Alumini_Students_Insert  @p0,@p1,@p2",
                                       data.MI_Id, data.ASMAY_Id, data.AMCST_Id);

                                if (outputval >= 1)
                                {
                                    data.returnval = true;
                                    data.message = "Update";
                                }
                                else
                                {
                                    data.returnval = true;
                                    data.message = "Update";
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        else
                        {
                            data.message = "Update";
                            data.returnval = false;
                        }
                    }

                    else
                    {
                        var update_tc1 = _ClgAdmissionContext.CollegeStudenttctransactionDMO.Single(d => d.ACSTC_Id == result.ACSTC_Id);

                        update_tc1.ACSTC_WorkingDays = data.ACSTC_WorkingDays;
                        update_tc1.ACSTC_AttendedDays = data.ACSTC_AttendedDays;
                        update_tc1.ACSTC_TCNO = data.ACSTC_TCNO;
                        update_tc1.ACSTC_Scholarship = data.ACSTC_Scholarship;
                        update_tc1.ACSTC_LanguageStudied = data.ACSTC_LanguageStudied;
                        update_tc1.ACSTC_MediumOfINStruction = data.ACSTC_MediumOfINStruction;
                        update_tc1.ACSTC_ElectivesStudied = data.ACSTC_ElectivesStudied;
                        update_tc1.ACSTC_MedicallyExam = data.ACSTC_MedicallyExam;
                        update_tc1.ACSTC_Qual_PromotionFlag = data.ACSTC_Qual_PromotionFlag;
                        update_tc1.ACSTC_Qual_Course = data.ACSTC_Qual_Course;
                        update_tc1.ACSTC_FeePaid = data.ACSTC_FeePaid;
                        update_tc1.ACSTC_FeeConcession = data.ACSTC_FeeConcession;
                        update_tc1.ACSTC_Conduct = data.ACSTC_Conduct;
                        update_tc1.ACSTC_LeavingReason = data.ACSTC_LeavingReason;
                        update_tc1.ACSTC_Result = data.ACSTC_Result;
                        update_tc1.ACSTC_ResultDetails = data.ACSTC_ResultDetails;
                        update_tc1.ACSTC_NCCDetails = data.ACSTC_NCCDetails;
                        update_tc1.ACSTC_LastExamDetails = data.ACSTC_LastExamDetails;
                        update_tc1.ACSTC_ExtraActivities = data.ACSTC_ExtraActivities;
                        update_tc1.ACSTC_Remarks = data.ACSTC_Remarks;
                        update_tc1.Last_Course_Studied = data.Last_Course_Studied;
                        update_tc1.Caste = data.Caste;
                        update_tc1.Fee_Due_Amnt = data.Fee_Due_Amnt;
                        update_tc1.Library_Due_Amnt = data.Library_Due_Amnt;
                        update_tc1.Store_Canteen_Due = data.Store_Canteen_Due;
                        update_tc1.PDA_Due = data.PDA_Due;
                        update_tc1.Admission_Date = data.Admission_Date;
                        update_tc1.ACSTC_TCApplicationDate = data.ACSTC_TCApplicationDate;
                        update_tc1.ACSTC_TCDate = data.ACSTC_TCDate;
                        update_tc1.ACSTC_TCIssueDate = data.ACSTC_TCIssueDate;
                        update_tc1.ACSTC_LastAttendedDate = data.ACSTC_LastAttendedDate;
                        update_tc1.ACSTC_PromotionDate = data.ACSTC_PromotionDate;
                        _ClgAdmissionContext.CollegeStudenttctransactionDMO.Update(update_tc1);

                        var update_flag1 = _ClgAdmissionContext.SaveChanges();

                        if (update_flag1 >= 1)
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
                    CollegeStudenttctransactionDMO tc_dmo = new CollegeStudenttctransactionDMO();

                    if (data.transnumconfigsettings.IMN_AutoManualFlag == "Auto")
                    {
                        GenerateTransactionNumbering a = new GenerateTransactionNumbering(_ClgAdmissionContext);
                        data.transnumconfigsettings.MI_Id = data.MI_Id;
                        data.transnumconfigsettings.ASMAY_Id = Convert.ToInt64(data.ASMAY_Id);
                        data.ACSTC_TCNO = a.GenerateNumber(data.transnumconfigsettings);
                    }

                    if (data.transnumconfigsettings.IMN_AutoManualFlag == "Manual")
                    {
                        var tcnoduplicate = _ClgAdmissionContext.CollegeStudenttctransactionDMO.Where(d => d.ACSTC_TCNO == data.ACSTC_TCNO && d.MI_Id == data.MI_Id).Count();
                        if (tcnoduplicate > 0)
                        {
                            data.message = "Exists";
                            return data;
                        }
                    }
                    var studDet = _ClgAdmissionContext.Adm_Master_College_StudentDMO.Where(d => d.AMCST_Id == data.AMCST_Id).ToList();
                    var section = _ClgAdmissionContext.Adm_College_Yearly_StudentDMO.Where(d => d.AMCST_Id == data.AMCST_Id && d.ASMAY_Id == data.ASMAY_Id && d.ACYST_ActiveFlag == 1).ToList();
                    if (section.Count == 0)
                    {
                        data.AMCO_Id = 0;
                        data.AMB_Id = 0;
                        data.AMSE_Id = 0;
                        data.ACMS_Id = 0;
                    }
                    else
                    {
                        for (int i = 0; i < section.Count; i++)
                        {
                            data.AMCO_Id = section[i].AMCO_Id;
                            data.AMB_Id = section[i].AMB_Id;
                            data.AMSE_Id = section[i].AMSE_Id;
                            data.ACMS_Id = section[i].ACMS_Id;
                        }
                    }

                    //tc_dmo.ASMAY_Id = studDet.FirstOrDefault().ASMAY_Id;
                    tc_dmo.ASMAY_Id = data.ASMAY_Id;
                    tc_dmo.IMC_Id = Convert.ToInt64(studDet.FirstOrDefault().IMC_Id);
                    tc_dmo.MI_Id = studDet.FirstOrDefault().MI_Id;
                    //saving the new records for active student/decative student(it will become either temp or left according to ASTC_TemporaryFlag) 
                    if (data.ACSTC_ActiveFlag == "S" || data.ACSTC_ActiveFlag == "D")
                    {
                        if (data.ACSTC_TemporaryFlag == 1)
                        {
                            tc_dmo.ACSTC_ActiveFlag = "T";
                            tc_dmo.ACSTC_TemporaryFlag = 1;
                            tc_dmo.CreatedDate = DateTime.Now;
                            tc_dmo.UpdatedDate = DateTime.Now;
                        }
                        else
                        {
                            tc_dmo.ACSTC_ActiveFlag = "L";
                            tc_dmo.ACSTC_TemporaryFlag = 0;
                            tc_dmo.CreatedDate = DateTime.Now;
                            tc_dmo.UpdatedDate = DateTime.Now;

                        }


                        tc_dmo.MI_Id = data.MI_Id;
                        tc_dmo.ASMAY_Id = data.ASMAY_Id;
                        tc_dmo.AMCO_Id = data.AMCO_Id;
                        tc_dmo.AMB_Id = data.AMB_Id;
                        tc_dmo.AMSE_Id = data.AMSE_Id;
                        tc_dmo.ACMS_Id = data.ACMS_Id;
                        tc_dmo.AMCST_Id = data.AMCST_Id;
                        tc_dmo.IMC_Id = data.IMC_Id;
                        tc_dmo.ACSTC_WorkingDays = data.ACSTC_WorkingDays;
                        tc_dmo.ACSTC_AttendedDays = data.ACSTC_AttendedDays;
                        tc_dmo.ACSTC_TCNO = data.ACSTC_TCNO;
                        tc_dmo.ACSTC_Scholarship = data.ACSTC_Scholarship;
                        tc_dmo.ACSTC_LanguageStudied = data.ACSTC_LanguageStudied;
                        tc_dmo.ACSTC_MediumOfINStruction = data.ACSTC_MediumOfINStruction;
                        tc_dmo.ACSTC_ElectivesStudied = data.ACSTC_ElectivesStudied;
                        tc_dmo.ACSTC_MedicallyExam = data.ACSTC_MedicallyExam;
                        tc_dmo.ACSTC_Qual_PromotionFlag = data.ACSTC_Qual_PromotionFlag;
                        tc_dmo.ACSTC_Qual_Course = data.ACSTC_Qual_Course;
                        tc_dmo.ACSTC_FeePaid = data.ACSTC_FeePaid;
                        tc_dmo.ACSTC_FeeConcession = data.ACSTC_FeeConcession;
                        tc_dmo.ACSTC_Conduct = data.ACSTC_Conduct;
                        tc_dmo.ACSTC_LeavingReason = data.ACSTC_LeavingReason;
                        tc_dmo.ACSTC_Result = data.ACSTC_Result;
                        tc_dmo.ACSTC_ResultDetails = data.ACSTC_ResultDetails;
                        tc_dmo.ACSTC_NCCDetails = data.ACSTC_NCCDetails;
                        tc_dmo.ACSTC_LastExamDetails = data.ACSTC_LastExamDetails;
                        tc_dmo.ACSTC_ExtraActivities = data.ACSTC_ExtraActivities;
                        tc_dmo.ACSTC_Remarks = data.ACSTC_Remarks;
                        tc_dmo.Last_Course_Studied = data.Last_Course_Studied;
                        tc_dmo.Caste = data.Caste;
                        tc_dmo.Fee_Due_Amnt = data.Fee_Due_Amnt;
                        tc_dmo.Library_Due_Amnt = data.Library_Due_Amnt;
                        tc_dmo.Store_Canteen_Due = data.Store_Canteen_Due;
                        tc_dmo.PDA_Due = data.PDA_Due;
                        tc_dmo.Admission_Date = data.Admission_Date;
                        tc_dmo.ACSTC_TCApplicationDate = data.ACSTC_TCApplicationDate;
                        tc_dmo.ACSTC_TCDate = data.ACSTC_TCDate;
                        tc_dmo.ACSTC_TCIssueDate = data.ACSTC_TCIssueDate;
                        tc_dmo.ACSTC_LastAttendedDate = data.ACSTC_LastAttendedDate;
                        tc_dmo.ACSTC_PromotionDate = data.ACSTC_PromotionDate;


                        _ClgAdmissionContext.Add(tc_dmo);

                        //updating the records for active student/decative student(it will become either temp or left according to ASTC_TemporaryFlag)                    


                        var update_ADM = _ClgAdmissionContext.Adm_Master_College_StudentDMO.FirstOrDefault(d => d.AMCST_Id == data.AMCST_Id);
                        var update_admy = _ClgAdmissionContext.Adm_College_Yearly_StudentDMO.FirstOrDefault(d => d.AMCST_Id == data.AMCST_Id && d.ACYST_ActiveFlag == 1
                        && d.ASMAY_Id == data.ASMAY_Id);

                        if (update_ADM.AMCST_SOL == "S" || update_ADM.AMCST_SOL == "D")
                        {
                            if (data.ACSTC_TemporaryFlag == 1)
                            {
                                update_ADM.AMCST_SOL = "T";
                                update_ADM.AMCST_ActiveFlag = true;
                                update_admy.ACYST_ActiveFlag = 1;
                            }
                            else
                            {
                                update_ADM.AMCST_SOL = "L";
                                update_ADM.AMCST_ActiveFlag = false;
                                update_admy.ACYST_ActiveFlag = 0;
                            }

                            update_ADM.UpdatedDate = DateTime.Now;
                            _ClgAdmissionContext.Adm_Master_College_StudentDMO.Update(update_ADM);
                            _ClgAdmissionContext.Adm_College_Yearly_StudentDMO.Update(update_admy);
                        }

                        var flag = _ClgAdmissionContext.SaveChanges();

                        if (flag > 0)
                        {
                            data.returnval = true;
                            try
                            {
                                var getstdappid = _ClgAdmissionContext.CollegeStudentlogin.Where(a => a.AMCST_Id == data.AMCST_Id).Select(a => a.IVRMUL_Id).ToList();

                                var chckuser = _ClgAdmissionContext.UserRoleWithInstituteDMO.Where(a => a.MI_Id == data.MI_Id && a.Id == getstdappid[0]).ToList();

                                if (chckuser.Count() > 0)
                                {
                                    var updatecheckuser = _ClgAdmissionContext.UserRoleWithInstituteDMO.Single(a => a.MI_Id == data.MI_Id && a.Id == getstdappid[0]);

                                    updatecheckuser.Activeflag = 0;
                                    _ClgAdmissionContext.Update(updatecheckuser);
                                    var i = _ClgAdmissionContext.SaveChanges();
                                    if (i > 0)
                                    {
                                        data.returnval = true;
                                    }
                                    else
                                    {
                                        data.returnval = true;
                                    }
                                }
                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }                          

                            try
                            {
                                if(data.ACSTC_TemporaryFlag == 0)
                                {
                                    var outputval = _ClgAdmissionContext.Database.ExecuteSqlCommand("CLG_Alumini_Students_Insert  @p0,@p1,@p2",
                                       data.MI_Id, data.ASMAY_Id, data.AMCST_Id);

                                    if (outputval >= 1)
                                    {
                                        data.returnval = true;
                                    }
                                    else
                                    {
                                        data.returnval = true;
                                    }
                                }                                

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
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
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
