using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DomainModel;
using PreadmissionDTOs;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model;
using DataAccessMsSqlServerProvider;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class StudentAchievementReportImpl : Interfaces.StudentAchievementReportInterface
    {
        private static ConcurrentDictionary<string, Adm_M_StudentDTO> _login =
            new ConcurrentDictionary<string, Adm_M_StudentDTO>();

        public DomainModelMsSqlServerContext _DomainModelMsSqlServerContext;
        public StudentAchievementReportImpl(DomainModelMsSqlServerContext DomainModelMsSqlServerContext)
        {
            _DomainModelMsSqlServerContext = DomainModelMsSqlServerContext;
        }

        public Adm_M_StudentDTO getdetails(Adm_M_StudentDTO stu)
        {
            Adm_M_StudentDTO acdmc = new Adm_M_StudentDTO();
            try
            {
                List<MasterAcademic> allacademic = new List<MasterAcademic>();
                allacademic = _DomainModelMsSqlServerContext.AcademicYear.Where(r => r.Is_Active == true && r.MI_Id == stu.MI_Id).ToList();
                acdmc.AllAcademicYear = allacademic.OrderByDescending(a => a.ASMAY_Order).ToArray();

                List<School_M_Class> classlist = new List<School_M_Class>();
                classlist = _DomainModelMsSqlServerContext.School_M_Class.Where(s => s.ASMCL_ActiveFlag == true && s.MI_Id == stu.MI_Id).ToList();
                acdmc.AllClass = classlist.OrderBy(c => c.ASMCL_Order).ToArray();

                List<School_M_Section> seclist = new List<School_M_Section>();
                seclist = _DomainModelMsSqlServerContext.Section.Where(a => a.ASMC_ActiveFlag == 1 && a.MI_Id == stu.MI_Id).ToList();
                acdmc.AllSection = seclist.OrderBy(s => s.ASMC_Order).ToArray();

                //List<StudentAchivementDMO> AchivementList = new List<StudentAchivementDMO>();
                //AchivementList = _DomainModelMsSqlServerContext.Achivement.Where(s => s.MI_Id == stu.MI_Id).ToList();
                //acdmc.AllAchivement = AchivementList.ToArray();

                //List<Adm_M_Student> adm_m_student = new List<Adm_M_Student>();
                //adm_m_student = _DomainModelMsSqlServerContext.Adm_M_Student.ToList();
                //adm_m_student = _DomainModelMsSqlServerContext.Adm_M_Student.Where(a => a == 1 && a.MI_Id == stu.MI_Id).ToList();
                //acdmc.adm_m_student = adm_m_student.ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return acdmc;
        }

        public async System.Threading.Tasks.Task<Adm_M_StudentDTO> getStudDetails(Adm_M_StudentDTO stuDTO)
        {
            //  Adm_M_StudentDTO StudDet = new Adm_M_StudentDTO();
            List<Adm_M_StudentDTO> HFClist = new List<Adm_M_StudentDTO>();
            try
            {

                using (var cmd = _DomainModelMsSqlServerContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_Student_Achievement_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    //   cmd.Parameters.Add(new SqlParameter("@mywhere", SqlDbType.VarChar) { Value = Convert.ToString(mywhere) });
                    //cmd.Parameters.Add(new SqlParameter("@SOL", SqlDbType.VarChar) { Value = stuDTO.AMST_SOL });
                    cmd.Parameters.Add(new SqlParameter("@yearId", SqlDbType.BigInt) { Value = stuDTO.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@classid", SqlDbType.BigInt) { Value = stuDTO.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@sectionid", SqlDbType.BigInt) { Value = stuDTO.ASMC_Id });
                    cmd.Parameters.Add(new SqlParameter("@studid", SqlDbType.BigInt) { Value = stuDTO.AMST_Id });
                    //cmd.Parameters.Add(new SqlParameter("@achiveid", SqlDbType.BigInt) { Value = stuDTO.AMSTEC_Id });
                    cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.BigInt) { Value = stuDTO.MI_Id });

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
                        stuDTO.studentlist = retObject.ToArray();

                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return stuDTO;
        }


        public async Task<Adm_M_StudentDTO> stdnamesyear(Adm_M_StudentDTO stu)
        {
            try
            {
                var flag = "S";
                var aciveflag = 1;
                var amayflag = 1;

                var checkflag = _DomainModelMsSqlServerContext.AdmissionStandardDMO.Where(a => a.MI_Id == stu.MI_Id).ToList();
                var checkflag1 = _DomainModelMsSqlServerContext.GenConfig.Where(a => a.MI_Id == stu.MI_Id).ToList();

                if (checkflag1.FirstOrDefault().IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag == "1")
                {
                    stu.studentlist123 = (from m in _DomainModelMsSqlServerContext.Adm_M_Student
                                          from n in _DomainModelMsSqlServerContext.School_Adm_Y_StudentDMO
                                          where m.AMST_Id == n.AMST_Id && n.ASMAY_Id == stu.ASMAY_Id && n.ASMCL_Id == stu.ASMCL_Id
                                          && n.ASMS_Id == stu.asms_id && m.MI_Id == stu.MI_Id && m.AMST_SOL == flag && m.AMST_ActiveFlag == aciveflag && n.AMAY_ActiveFlag == amayflag
                                          select new Adm_M_StudentDTO
                                          {
                                              AMST_Id = m.AMST_Id,
                                              Name = ((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName) + ":" + (m.AMST_AdmNo == null ? " " : m.AMST_AdmNo)).Trim(),

                                          }).ToArray();
                }

                else if (checkflag1.FirstOrDefault().IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag == "2")
                {
                    stu.studentlist123 = (from m in _DomainModelMsSqlServerContext.Adm_M_Student
                                          from n in _DomainModelMsSqlServerContext.School_Adm_Y_StudentDMO
                                          where m.AMST_Id == n.AMST_Id && n.ASMAY_Id == stu.ASMAY_Id && n.ASMCL_Id == stu.ASMCL_Id
                                          && n.ASMS_Id == stu.asms_id && m.MI_Id == stu.MI_Id && m.AMST_SOL == flag && m.AMST_ActiveFlag == aciveflag && n.AMAY_ActiveFlag == amayflag
                                          select new Adm_M_StudentDTO
                                          {
                                              AMST_Id = m.AMST_Id,
                                              Name = ((m.AMST_RegistrationNo == null ? " " : m.AMST_RegistrationNo) + ':' + (m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName)).Trim(),

                                          }).ToArray();
                }

                else if (checkflag1.FirstOrDefault().IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag == "3")
                {
                    stu.studentlist123 = (from m in _DomainModelMsSqlServerContext.Adm_M_Student
                                          from n in _DomainModelMsSqlServerContext.School_Adm_Y_StudentDMO
                                          where m.AMST_Id == n.AMST_Id && n.ASMAY_Id == stu.ASMAY_Id && n.ASMCL_Id == stu.ASMCL_Id
                                          && n.ASMS_Id == stu.asms_id && m.MI_Id == stu.MI_Id && m.AMST_SOL == flag && m.AMST_ActiveFlag == aciveflag && n.AMAY_ActiveFlag == amayflag
                                          select new Adm_M_StudentDTO
                                          {
                                              AMST_Id = m.AMST_Id,
                                              Name = ((m.AMST_AdmNo == null ? " " : m.AMST_AdmNo) + ':' + (m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName)).Trim(),

                                          }).ToArray();
                }

                else if (checkflag1.FirstOrDefault().IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag == "4")
                {
                    stu.studentlist123 = (from m in _DomainModelMsSqlServerContext.Adm_M_Student
                                          from n in _DomainModelMsSqlServerContext.School_Adm_Y_StudentDMO
                                          where m.AMST_Id == n.AMST_Id && n.ASMAY_Id == stu.ASMAY_Id && n.ASMCL_Id == stu.ASMCL_Id
                                          && n.ASMS_Id == stu.asms_id && m.MI_Id == stu.MI_Id && m.AMST_SOL == flag && m.AMST_ActiveFlag == aciveflag && n.AMAY_ActiveFlag == amayflag
                                          select new Adm_M_StudentDTO
                                          {
                                              AMST_Id = m.AMST_Id,
                                              Name = ((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName) + ':' + (m.AMST_RegistrationNo == null ? " " : m.AMST_RegistrationNo)).Trim(),

                                          }).ToArray();
                }
                else if (checkflag1.FirstOrDefault().IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag == "6")
                {
                    stu.studentlist123 = (from m in _DomainModelMsSqlServerContext.Adm_M_Student
                                          from n in _DomainModelMsSqlServerContext.School_Adm_Y_StudentDMO
                                          where m.AMST_Id == n.AMST_Id && n.ASMAY_Id == stu.ASMAY_Id && n.ASMCL_Id == stu.ASMCL_Id
                                          && n.ASMS_Id == stu.asms_id && m.MI_Id == stu.MI_Id && m.AMST_SOL == flag && m.AMST_ActiveFlag == aciveflag && n.AMAY_ActiveFlag == amayflag
                                          select new Adm_M_StudentDTO
                                          {
                                              AMST_Id = m.AMST_Id,
                                              Name = ((n.AMAY_RollNo.ToString() == null ? " " : n.AMAY_RollNo.ToString()) + ':' + (m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName)).Trim(),

                                          }).ToArray();
                }

                else if (checkflag1.FirstOrDefault().IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag == "5")
                {
                    stu.studentlist123 = (from m in _DomainModelMsSqlServerContext.Adm_M_Student
                                          from n in _DomainModelMsSqlServerContext.School_Adm_Y_StudentDMO
                                          where m.AMST_Id == n.AMST_Id && n.ASMAY_Id == stu.ASMAY_Id && n.ASMCL_Id == stu.ASMCL_Id
                                          && n.ASMS_Id == stu.asms_id && m.MI_Id == stu.MI_Id && m.AMST_SOL == flag && m.AMST_ActiveFlag == aciveflag && n.AMAY_ActiveFlag == amayflag
                                          select new Adm_M_StudentDTO
                                          {
                                              AMST_Id = m.AMST_Id,
                                              Name = ((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName) + ':' + (n.AMAY_RollNo.ToString() == null ? " " : n.AMAY_RollNo.ToString())).Trim(),

                                          }).ToArray();
                }

                else
                {
                    stu.studentlist123 = (from m in _DomainModelMsSqlServerContext.Adm_M_Student
                                          from n in _DomainModelMsSqlServerContext.School_Adm_Y_StudentDMO
                                          where m.AMST_Id == n.AMST_Id && n.ASMAY_Id == stu.ASMAY_Id && n.ASMCL_Id == stu.ASMCL_Id
                                          && n.ASMS_Id == stu.asms_id && m.MI_Id == stu.MI_Id && m.AMST_SOL == flag && m.AMST_ActiveFlag == aciveflag && n.AMAY_ActiveFlag == amayflag
                                          select new Adm_M_StudentDTO
                                          {
                                              AMST_Id = m.AMST_Id,
                                              Name = ((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName) + ':' + (m.AMST_RegistrationNo == null ? " " : m.AMST_RegistrationNo)).Trim(),

                                          }).ToArray();
                }



                //using (var cmd = _DomainModelMsSqlServerContext.Database.GetDbConnection().CreateCommand())
                //{
                //    cmd.CommandText = "Student_achement_report_namebinding";
                //    cmd.CommandType = CommandType.StoredProcedure;

                //    cmd.Parameters.Add(new SqlParameter("@year",
                //       SqlDbType.VarChar)
                //    {
                //        Value = stu.ASMAY_Id
                //    });

                //    cmd.Parameters.Add(new SqlParameter("@miid",
                //        SqlDbType.VarChar)
                //    {
                //        Value = stu.MI_Id
                //    });

                //    cmd.Parameters.Add(new SqlParameter("@classid",
                //        SqlDbType.VarChar)
                //    {
                //        Value = stu.ASMCL_Id
                //    });

                //    cmd.Parameters.Add(new SqlParameter("@secid",
                //        SqlDbType.VarChar)
                //    {
                //        Value = stu.asms_id
                //    });
                //    //cmd.Parameters.Add(new SqlParameter("@flag",
                //    // SqlDbType.VarChar)
                //    //{
                //    //    Value = stu.flag123
                //    //});

                //    //cmd.Parameters.Add(new SqlParameter("@amst_id",
                //    // SqlDbType.VarChar)
                //    //{
                //    //    Value = stu.AMST_Id
                //    //});



                //    if (cmd.Connection.State != ConnectionState.Open)
                //        cmd.Connection.Open();

                //    var retObject = new List<dynamic>();     

                //    try
                //    {
                //        using (var dataReader = await cmd.ExecuteReaderAsync())
                //        {
                //            while (await dataReader.ReadAsync())
                //            {
                //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                {
                //                    dataRow.Add(
                //                        dataReader.GetName(iFiled),
                //                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                //                    );
                //                }

                //                retObject.Add((ExpandoObject)dataRow);
                //            }
                //        }
                //        stu.studentlist123 = retObject.ToArray();

                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine(ex.Message);
                //    }
                //}
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return stu;
        }
    }
}
