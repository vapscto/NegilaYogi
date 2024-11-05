using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Principal.Services
{
    public class StudentSearchImpl : Interfaces.StudentSearchInterface
    {
        public FeeGroupContext _fees;
        public ExamContext _exam;

        public StudentSearchImpl(FeeGroupContext fees, ExamContext exm)
        {
            _fees = fees;
            _exam = exm;
        }
        public StudentSearchDTO getdatastuacadgrp(StudentSearchDTO data)
        {
            try
            {
                // data.HRME_Id = _exam.Staff_User_Login.Single(c => c.Id == data.userid && c.MI_Id == data.MI_Id).Emp_Code;

                var list = _fees.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(e => e.ASMAY_Order).ToList();//AcademicYear
                data.yearlist = list.ToArray();

                var currYear = _fees.AcademicYear.Where(d => d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id).ToList();//AcademicYear
                data.currentYear = currYear.ToArray();

                var classlist = _fees.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                data.classlist = classlist.ToArray();

                var sectionlist = _fees.school_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).ToList();
                data.sectionlist = sectionlist.ToArray();


                var studentlist = (from m in _fees.AdmissionStudentDMO
                                   from n in _fees.School_Adm_Y_StudentDMO
                                   where m.AMST_Id == n.AMST_Id && m.MI_Id == data.MI_Id && n.ASMAY_Id == currYear.FirstOrDefault().ASMAY_Id && m.AMST_SOL.Equals("S") && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1 && n.ASMCL_Id == classlist.FirstOrDefault().ASMCL_Id && n.ASMS_Id == sectionlist.FirstOrDefault().ASMS_Id
                                   select new StudentSearchDTO
                                   {
                                       Amst_Id = m.AMST_Id,
                                       AMST_MiddleName = m.AMST_MiddleName,
                                       AMST_LastName = m.AMST_LastName,
                                       admno = m.AMST_AdmNo,
                                       AMST_FirstName = ((m.AMST_FirstName == null || m.AMST_FirstName == "" ? "" : " " + m.AMST_FirstName) + (m.AMST_MiddleName == null || m.AMST_MiddleName == "" || m.AMST_MiddleName == "0" ? "" : " " + m.AMST_MiddleName) + (m.AMST_LastName == null || m.AMST_LastName == "" || m.AMST_LastName == "0" ? "" : " " + m.AMST_LastName)).Trim(),

                                   }).Distinct().OrderBy(f => f.AMST_FirstName).ToList();
                if (studentlist.Count > 0)
                {
                    data.fillstudent = studentlist.ToArray();
                    data.studentCount = studentlist.Count;
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public async Task<StudentSearchDTO> getstudentdetails(StudentSearchDTO data)
        {
            try
            {
                data.fillstudentalldetails = (from a in _fees.AdmissionStudentDMO
                                              from b in _fees.School_Adm_Y_StudentDMO
                                              from c in _fees.admissioncls
                                              from d in _fees.school_M_Section
                                              from e in _fees.AcademicYear
                                              where (b.ASMCL_Id == c.ASMCL_Id && d.ASMS_Id == b.ASMS_Id && a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id
                                              && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.Amst_Id && e.ASMAY_Id == data.ASMAY_Id)
                                              select new StudentSearchDTO
                                              {
                                                  // amst_Id = a.AMST_Id,
                                                  amst_FirstName = a.AMST_FirstName,
                                                  amst_MiddleName = a.AMST_MiddleName,
                                                  amst_LastName = a.AMST_LastName,
                                                  amst_RegistrationNo = a.AMST_RegistrationNo,
                                                  amst_AdmNo = a.AMST_AdmNo,
                                                  amay_RollNo = b.AMAY_RollNo,
                                                  classname = c.ASMCL_ClassName,
                                                  sectionname = d.ASMC_SectionName,
                                                  fathername = a.AMST_FatherName,
                                                  mothername = a.AMST_MotherName,
                                                  bloodgroup = a.AMST_BloodGroup,
                                                  address1 = a.AMST_PerStreet,
                                                  address2 = a.AMST_PerArea,
                                                  address3 = a.AMST_PerCity,
                                                  fathermobileno = a.AMST_FatherMobleNo,
                                                  studentdob = a.AMST_DOB,
                                                  amst_mobile = a.AMST_MobileNo,
                                                  amst_sex = a.AMST_Sex,
                                                  amst_dob = a.AMST_DOB,
                                                  amst_emailid = a.AMST_emailId,
                                                  asma_year = e.ASMAY_Year,
                                                  AMST_FatherOccupation = a.AMST_FatherOccupation == null ? "" : a.AMST_FatherOccupation,
                                                  AMST_MotherOccupation = a.AMST_MotherOccupation == null ? "" : a.AMST_MotherOccupation,
                                              }).Distinct().ToArray();

                data.examlist = (from a in _exam.ExmStudentMarksProcessDMO
                                 from b in _exam.exammasterDMO
                                 where (a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.Amst_Id
                                 && a.EME_Id == b.EME_Id && b.EME_ActiveFlag == true)
                                 select new StudentSearchDTO
                                 {
                                     EME_Id = a.EME_Id,
                                     exam_name = b.EME_ExamName,
                                     totalmarks = a.ESTMP_TotalMaxMarks,
                                     obtainmarks = a.ESTMP_TotalObtMarks,
                                     persentage = a.ESTMP_Percentage,
                                     result = a.ESTMP_Result
                                 }).Distinct().ToArray();


                using (var cmd = _fees.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PORTAL_STU_FEES_STATUS_YEARWISE";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@AMST_ID",
                    SqlDbType.BigInt)
                    {
                        Value = data.Amst_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    using (var dataReader = await cmd.ExecuteReaderAsync())
                    {
                        while (await dataReader.ReadAsync())
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
                    data.getfeedetails = retObject.ToArray();

                }

                using (var cmd = _fees.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PORTAL_STUDENT_MONTHLY_ATTENDANCE";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@asmay_id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@amst_id",
                    SqlDbType.BigInt)
                    {
                        Value = data.Amst_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();


                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        data.attendencelist = retObject.ToArray();

                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                using (var cmd = _fees.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_PRINCIPAL_TERMWISE_FEE";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.Amst_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();


                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        data.termwisefeelist = retObject.ToArray();

                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

                using (var cmd = _exam.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_StudentCompliants_View";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar)
                    {
                        Value = data.Amst_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        data.studentdivlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public async Task<StudentSearchDTO> showsectionGrid(StudentSearchDTO data)
        {

            try
            {
                using (var cmd = _fees.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PRINCIPAL_GET_SUBWISE_MARKS";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.Amst_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
                   SqlDbType.BigInt)
                    {
                        Value = data.EME_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();


                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
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
                        data.subwiseexmlist = retObject.ToArray();

                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }
        public async Task<StudentSearchDTO> GetStudentDetails1(StudentSearchDTO data)
        {
            try
            {
                var studentlist = (from m in _fees.AdmissionStudentDMO
                                   from n in _fees.School_Adm_Y_StudentDMO
                                   where m.AMST_Id == n.AMST_Id && m.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && m.AMST_SOL.Equals("S") && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1 && n.ASMCL_Id == data.ASMCL_Id && n.ASMS_Id == data.ASMS_Id
                                   select new StudentSearchDTO
                                   {
                                       Amst_Id = m.AMST_Id,
                                       AMST_MiddleName = m.AMST_MiddleName,
                                       AMST_LastName = m.AMST_LastName,
                                       admno = m.AMST_AdmNo,
                                       AMST_FirstName = ((m.AMST_FirstName == null || m.AMST_FirstName == "" ? "" : " " + m.AMST_FirstName) + (m.AMST_MiddleName == null || m.AMST_MiddleName == "" || m.AMST_MiddleName == "0" ? "" : " " + m.AMST_MiddleName) + (m.AMST_LastName == null || m.AMST_LastName == "" || m.AMST_LastName == "0" ? "" : " " + m.AMST_LastName)).Trim(),
                                   }).Distinct().OrderBy(f => f.AMST_FirstName).ToList();


                if (studentlist.Count > 0)
                {
                    data.fillstudent = studentlist.ToArray();
                    data.studentCount = studentlist.Count;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }

        public StudentSearchDTO GetStudentSearchByNameOrAdmno(StudentSearchDTO data)
        {
            try
            {
                data.searchfiltervalue = data.searchfiltervalue.ToUpper();

                if (data.searchbyflag == "Admo")
                {
                    data.studentlistsearch = (from a in _fees.Adm_M_Student
                                              from b in _fees.SchoolYearWiseStudent
                                              where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                              && a.AMST_SOL.Equals("S") && a.AMST_ActiveFlag == 1
                                              && b.AMAY_ActiveFlag == 1 && (a.AMST_AdmNo.Contains(data.searchfiltervalue)))
                                              select new StudentSearchDTO
                                              {
                                                  Amst_Id = a.AMST_Id,
                                                  amst_FirstName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) + " " +
                                                  (a.AMST_MiddleName == null ? "" : a.AMST_MiddleName) + " " +
                                                  (a.AMST_LastName == null ? "" : a.AMST_LastName) + ':' + a.AMST_AdmNo).Trim(),
                                              }).ToArray();
                }

                else if (data.searchbyflag == "Name")
                {
                    //data.studentlistsearch = (from a in _fees.Adm_M_Student
                    //                          from b in _fees.SchoolYearWiseStudent
                    //                          where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                    //                          && a.AMST_SOL.Equals("S") && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1
                    //                          && ((a.AMST_FirstName.Trim().ToUpper() + ' ' + a.AMST_MiddleName.Trim().ToUpper() + ' ' +
                    //                          a.AMST_LastName.Trim().ToUpper()).Contains(data.searchfiltervalue) ||
                    //                          a.AMST_FirstName.Trim().ToUpper().Contains(data.searchfiltervalue) ||
                    //                          a.AMST_MiddleName.Trim().ToUpper().Contains(data.searchfiltervalue) ||
                    //                          a.AMST_LastName.Trim().ToUpper().Contains(data.searchfiltervalue)))
                    //                          select new StudentSearchDTO
                    //                          {
                    //                              Amst_Id = a.AMST_Id,
                    //                              amst_FirstName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) + " " +
                    //                              (a.AMST_MiddleName == null ? "" : a.AMST_MiddleName) + " " +
                    //                              (a.AMST_LastName == null ? "" : a.AMST_LastName) + ':' + a.AMST_AdmNo).Trim(),
                    //                          }).ToArray();
                    string str = "";
                    
                    data.studentlistsearch = (from a in _fees.Adm_M_Student
                                              from b in _fees.SchoolYearWiseStudent
                                              where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                              && a.AMST_SOL.Equals("S") && a.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1
                                              && (((a.AMST_FirstName.ToUpper().Trim() + ' ' + (string.IsNullOrEmpty(a.AMST_MiddleName.ToUpper().Trim()) == true ?
                                                str : a.AMST_MiddleName.ToUpper().Trim())).Trim() + ' ' + (string.IsNullOrEmpty(a.AMST_LastName.ToUpper().Trim()) == true ?
                                                str : a.AMST_LastName.ToUpper().Trim())).Trim().Contains(data.searchfiltervalue)
                                                || a.AMST_FirstName.ToUpper().ToUpper().StartsWith(data.searchfiltervalue)
                                                || a.AMST_MiddleName.ToUpper().ToUpper().StartsWith(data.searchfiltervalue)
                                                || a.AMST_LastName.ToUpper().StartsWith(data.searchfiltervalue)))
                                              select new StudentSearchDTO
                                              {
                                                  Amst_Id = a.AMST_Id,
                                                  amst_FirstName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) + " " +
                                                  (a.AMST_MiddleName == null ? "" : a.AMST_MiddleName) + " " +
                                                  (a.AMST_LastName == null ? "" : a.AMST_LastName) + ':' + a.AMST_AdmNo).Trim(),
                                              }).ToArray();
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
