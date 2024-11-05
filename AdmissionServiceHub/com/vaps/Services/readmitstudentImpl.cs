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
using Microsoft.Extensions.Logging;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class readmitstudentImpl : Interfaces.readmitstudentInterface
    {
        private static ConcurrentDictionary<string, Adm_M_StudentDTO> _login =
        new ConcurrentDictionary<string, Adm_M_StudentDTO>();

        public DomainModelMsSqlServerContext _Context;
        public readmitstudentContext db;
        public AdmissionFormContext stu_;
        ILogger<readmitstudentImpl> _acdimpl;
        public readmitstudentImpl(DomainModelMsSqlServerContext Admissiondbcontext, readmitstudentContext readmitstudentContext, AdmissionFormContext ams, ILogger<readmitstudentImpl> acdimpl)
        {
            _Context = Admissiondbcontext;
            db = readmitstudentContext;
            stu_ = ams;
            _acdimpl = acdimpl;
        }
        public SchoolYearWiseStudentDTO GetDropDownList(SchoolYearWiseStudentDTO stu)
        {
            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _Context.AcademicYear.Where(t => t.MI_Id.Equals(stu.MI_Id) && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                stu.YearList = year.ToArray();

                List<School_M_Class> classList = new List<School_M_Class>();
                classList = _Context.School_M_Class.Where(t => t.MI_Id.Equals(stu.MI_Id) && t.ASMCL_ActiveFlag == true).OrderBy(c => c.ASMCL_Order).ToList();
                stu.classList = classList.ToArray();

                stu.sectionList = _Context.School_M_Section.Where(t => t.MI_Id == stu.MI_Id && t.ASMC_ActiveFlag == 1).OrderBy(s => s.ASMC_Order).ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return stu;
        }

        //student details by year

        public SchoolYearWiseStudentDTO GetStudentListByYear(long id)
        {
            SchoolYearWiseStudentDTO stu = new SchoolYearWiseStudentDTO();
            try
            {

                stu.StudentList = (from s in _Context.Adm_M_Student
                                   where !(from ys in _Context.SchoolYearWiseStudent
                                           where ys.AMAY_ActiveFlag == 0
                                           select ys.AMST_Id).Contains(s.AMST_Id) && s.ASMAY_Id == id
                                   select s).ToArray();

                //stu.StudentList = (from a in _Context.Student_TC
                //                   from b in _Context.Adm_M_Student
                //                   where (a.AMST_Id == b.AMST_Id && a.MI_Id == stu.MI_Id && a.ASTC_ActiveFlag.Equals('L') && a.ASMAY_Id == id)
                //                   select b).Distinct().ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return stu;
        }


        public readmitstudentDTO savereadmit_student(readmitstudentDTO secAllt)
        {
            try
            {
                using (var transaction = stu_.Database.BeginTransaction())
                {
                    try
                    {
                        readmitstudentDMO MM1 = new readmitstudentDMO
                        {
                            ASMAY_Id_OLd = secAllt.AMAY_ID_OLD,
                            ASMCL_Id_Old = secAllt.AMCL_ID_OLD,
                            AMST_Id_Old = secAllt.AMST_Id,
                            ASMCL_Id_New = secAllt.AMCL_Id_New,
                            ASMAY_Id_New = secAllt.AMAY_Id_New,
                            AMST_Id_New = secAllt.AMST_Id_New,
                            MI_Id = secAllt.MI_Id,
                            userid = secAllt.userid,
                            CreatedDate = DateTime.Now,
                            UpdatedDate = DateTime.Now
                        };

                        stu_.Add(MM1);
                        int n = stu_.SaveChanges();
                        if (n > 0)
                        {
                            transaction.Commit();
                            secAllt.returnval = true;
                        }
                        else
                        {
                            secAllt.returnval = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }           
            return secAllt;
        }

        // GetstudentdetailsbyYearandclass
        public SchoolYearWiseStudentDTO GetstudentdetailsbyYearandclass(SchoolYearWiseStudentDTO stu)
        {
            try
            {
                var checkflag = _Context.AdmissionStandardDMO.Where(a => a.MI_Id == stu.MI_Id).ToList();
                var checkflag1 = _Context.GenConfig.Where(a => a.MI_Id == stu.MI_Id).ToList();
                
                if (checkflag1.FirstOrDefault().IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag == "1")
                {
                    stu.StudentList = (from a in _Context.Student_TC
                                       from m in _Context.Adm_M_Student
                                       where (a.AMST_Id == m.AMST_Id && a.MI_Id == stu.MI_Id && a.ASTC_ActiveFlag.Equals("L") && a.ASMAY_Id == stu.AMAY_Id && a.ASMCL_Id == stu.ASMCL_Id && a.ASMS_Id == stu.ASMS_Id)
                                       select new SchoolYearWiseStudentDTO
                                       {
                                           AMST_Id = a.AMST_Id,
                                           AMST_FirstName = ((m.AMST_FirstName == null || m.AMST_FirstName == "" ? "" : m.AMST_FirstName) + 
                                           (m.AMST_MiddleName == null || m.AMST_MiddleName == "" ? "" : " " +m.AMST_MiddleName) + 
                                           (m.AMST_LastName == null || m.AMST_LastName == "" ? "" :" "+ m.AMST_LastName) +  
                                           (m.AMST_AdmNo == null || m.AMST_AdmNo == "" ? "" : " : " + m.AMST_AdmNo)).Trim(),

                                       }).Distinct().ToArray();
                }
                else if (checkflag1.FirstOrDefault().IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag == "2")
                {
                    stu.StudentList = (from a in _Context.Student_TC
                                       from m in _Context.Adm_M_Student
                                       where (a.AMST_Id == m.AMST_Id && a.MI_Id == stu.MI_Id && a.ASTC_ActiveFlag.Equals("L") && a.ASMAY_Id == stu.AMAY_Id && a.ASMCL_Id == stu.ASMCL_Id && a.ASMS_Id == stu.ASMS_Id)
                                       select new SchoolYearWiseStudentDTO
                                       {
                                           AMST_Id = m.AMST_Id,
                                           AMST_FirstName = ((m.AMST_RegistrationNo == null ? " " : m.AMST_RegistrationNo) + ':' + (m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName)).Trim(),
                                       }).Distinct().ToArray();
                }

                else if (checkflag1.FirstOrDefault().IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag == "3")
                {
                    stu.StudentList = (from a in _Context.Student_TC
                                       from m in _Context.Adm_M_Student
                                       where (a.AMST_Id == m.AMST_Id && a.MI_Id == stu.MI_Id && a.ASTC_ActiveFlag.Equals("L") && a.ASMAY_Id == stu.AMAY_Id && a.ASMCL_Id == stu.ASMCL_Id && a.ASMS_Id == stu.ASMS_Id)
                                       select new SchoolYearWiseStudentDTO
                                       {
                                           AMST_Id = m.AMST_Id,
                                           AMST_FirstName = ((m.AMST_AdmNo == null ? " " : m.AMST_AdmNo) + ':' + (m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName)).Trim(),

                                       }).Distinct().ToArray();
                }

                else if (checkflag1.FirstOrDefault().IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag == "4")
                {
                    stu.StudentList = (from a in _Context.Student_TC
                                       from m in _Context.Adm_M_Student
                                       where (a.AMST_Id == m.AMST_Id && a.MI_Id == stu.MI_Id && a.ASTC_ActiveFlag.Equals("L") && a.ASMAY_Id == stu.AMAY_Id && a.ASMCL_Id == stu.ASMCL_Id && a.ASMS_Id == stu.ASMS_Id)
                                       select new SchoolYearWiseStudentDTO
                                       {
                                           AMST_Id = m.AMST_Id,
                                           AMST_FirstName = ((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName) + ':' + (m.AMST_RegistrationNo == null ? " " : m.AMST_RegistrationNo)).Trim(),

                                       }).Distinct().ToArray();
                }
                else if (checkflag1.FirstOrDefault().IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag == "6")
                {
                    stu.StudentList = (from a in _Context.Student_TC
                                       from m in _Context.Adm_M_Student
                                       from n in _Context.School_Adm_Y_StudentDMO
                                       where (m.AMST_Id==n.AMST_Id && a.AMST_Id == m.AMST_Id && a.MI_Id == stu.MI_Id && a.ASTC_ActiveFlag.Equals("L") && a.ASMAY_Id == stu.AMAY_Id && a.ASMCL_Id == stu.ASMCL_Id && a.ASMS_Id == stu.ASMS_Id)
                                       select new SchoolYearWiseStudentDTO
                                       {
                                           AMST_Id = m.AMST_Id,
                                           AMST_FirstName = ((n.AMAY_RollNo.ToString() == null ? " " : n.AMAY_RollNo.ToString()) + ':' + (m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName)).Trim(),

                                       }).Distinct().ToArray();
                }

                else if (checkflag1.FirstOrDefault().IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag == "5")
                {
                    stu.StudentList = (from a in _Context.Student_TC
                                       from m in _Context.Adm_M_Student
                                       from n in _Context.School_Adm_Y_StudentDMO
                                       where (m.AMST_Id == n.AMST_Id &&  a.AMST_Id == m.AMST_Id && a.MI_Id == stu.MI_Id && a.ASTC_ActiveFlag.Equals("L") && a.ASMAY_Id == stu.AMAY_Id && a.ASMCL_Id == stu.ASMCL_Id && a.ASMS_Id == stu.ASMS_Id)
                                       select new SchoolYearWiseStudentDTO
                                       {
                                           AMST_Id = m.AMST_Id,
                                           AMST_FirstName = ((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName) + ':' + (n.AMAY_RollNo.ToString() == null ? " " : n.AMAY_RollNo.ToString())).Trim(),

                                       }).Distinct().ToArray();
                }

                else
                {
                    stu.StudentList = (from a in _Context.Student_TC
                                       from m in _Context.Adm_M_Student
                                       where (a.AMST_Id == m.AMST_Id && a.MI_Id == stu.MI_Id && a.ASTC_ActiveFlag.Equals("L") && a.ASMAY_Id == stu.AMAY_Id && a.ASMCL_Id == stu.ASMCL_Id && a.ASMS_Id == stu.ASMS_Id)
                                       select new SchoolYearWiseStudentDTO
                                       {
                                           AMST_Id = m.AMST_Id,
                                           AMST_FirstName = ((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName) + ':' + (m.AMST_RegistrationNo == null ? " " : m.AMST_RegistrationNo)).Trim(),

                                       }).Distinct().ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return stu;
        }
        public SchoolYearWiseStudentDTO getnewjoinlist(SchoolYearWiseStudentDTO data)
        {
            try
            {
                var checkflag = _Context.AdmissionStandardDMO.Where(a => a.MI_Id == data.MI_Id).ToList();
                if (checkflag.FirstOrDefault().ASC_AdmNo_RegNo_RollNo_DefaultFlag == "1")
                {
                    data.newstudlist = (from m in _Context.Adm_M_Student
                                        from b in _Context.School_Adm_Y_StudentDMO
                                        where (m.AMST_Id == b.AMST_Id && m.MI_Id == data.MI_Id && m.ASMAY_Id == data.AMAY_Id_New && m.ASMCL_Id == data.ASMCL_Id_New && b.ASMS_Id == data.ASMS_Id_New && m.AMST_SOL == "S" && m.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1)
                                        select new SchoolYearWiseStudentDTO
                                        {
                                            AMST_Id = m.AMST_Id,
                                            AMST_FirstName = ((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName) + ":" + (m.AMST_AdmNo == null ? " " : m.AMST_AdmNo)).Trim(),
                                        }).Distinct().ToArray();
                }

                else if (checkflag.FirstOrDefault().ASC_AdmNo_RegNo_RollNo_DefaultFlag == "2")
                {
                    data.newstudlist = (from m in _Context.Adm_M_Student
                                        from b in _Context.School_Adm_Y_StudentDMO
                                        where (m.AMST_Id == b.AMST_Id && m.MI_Id == data.MI_Id && m.ASMAY_Id == data.AMAY_Id_New && m.ASMCL_Id == data.ASMCL_Id_New && b.ASMS_Id == data.ASMS_Id_New && m.AMST_SOL == "S" && m.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1)
                                        select new SchoolYearWiseStudentDTO
                                        {
                                            AMST_Id = m.AMST_Id,
                                            AMST_FirstName = ((m.AMST_RegistrationNo == null ? " " : m.AMST_RegistrationNo) + ':' + (m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName)).Trim(),

                                        }).Distinct().ToArray();
                }

                else if (checkflag.FirstOrDefault().ASC_AdmNo_RegNo_RollNo_DefaultFlag == "3")
                {
                    data.newstudlist = (from m in _Context.Adm_M_Student
                                        from b in _Context.School_Adm_Y_StudentDMO
                                        where (m.AMST_Id == b.AMST_Id && m.MI_Id == data.MI_Id && m.ASMAY_Id == data.AMAY_Id_New && m.ASMCL_Id == data.ASMCL_Id_New && b.ASMS_Id == data.ASMS_Id_New && m.AMST_SOL == "S" && m.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1)
                                        select new SchoolYearWiseStudentDTO
                                        {
                                            AMST_Id = m.AMST_Id,
                                            AMST_FirstName = ((m.AMST_AdmNo == null ? " " : m.AMST_AdmNo) + ':' + (m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName)).Trim(),

                                        }).Distinct().ToArray();
                }

                else if (checkflag.FirstOrDefault().ASC_AdmNo_RegNo_RollNo_DefaultFlag == "4")
                {
                    data.newstudlist = (from m in _Context.Adm_M_Student
                                        from b in _Context.School_Adm_Y_StudentDMO
                                        where (m.AMST_Id == b.AMST_Id && m.MI_Id == data.MI_Id && m.ASMAY_Id == data.AMAY_Id_New && m.ASMCL_Id == data.ASMCL_Id_New && b.ASMS_Id == data.ASMS_Id_New && m.AMST_SOL == "S" && m.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1)
                                        select new SchoolYearWiseStudentDTO
                                        {
                                            AMST_Id = m.AMST_Id,
                                            AMST_FirstName = ((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName) + ':' + (m.AMST_RegistrationNo == null ? " " : m.AMST_RegistrationNo)).Trim(),

                                        }).Distinct().ToArray();
                }
                else if (checkflag.FirstOrDefault().ASC_AdmNo_RegNo_RollNo_DefaultFlag == "6")
                {
                    data.newstudlist = (from m in _Context.Adm_M_Student
                                        from n in _Context.School_Adm_Y_StudentDMO
                                        where (m.AMST_Id == n.AMST_Id && m.MI_Id == data.MI_Id && m.ASMAY_Id == data.AMAY_Id_New && m.ASMCL_Id == data.ASMCL_Id_New && n.ASMS_Id == data.ASMS_Id_New && m.AMST_SOL == "S" && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1)
                                        select new SchoolYearWiseStudentDTO
                                        {
                                            AMST_Id = m.AMST_Id,
                                            AMST_FirstName = ((n.AMAY_RollNo.ToString() == null ? " " : n.AMAY_RollNo.ToString()) + ':' + (m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName)).Trim(),

                                        }).Distinct().ToArray();
                }

                else if (checkflag.FirstOrDefault().ASC_AdmNo_RegNo_RollNo_DefaultFlag == "5")
                {
                    data.newstudlist = (from m in _Context.Adm_M_Student
                                        from n in _Context.School_Adm_Y_StudentDMO
                                        where (m.AMST_Id == n.AMST_Id && m.MI_Id == data.MI_Id && m.ASMAY_Id == data.AMAY_Id_New && m.ASMCL_Id == data.ASMCL_Id_New && n.ASMS_Id == data.ASMS_Id_New && m.AMST_SOL == "S" && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1)
                                        select new SchoolYearWiseStudentDTO
                                        {
                                            AMST_Id = m.AMST_Id,
                                            AMST_FirstName = ((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName) + ':' + (n.AMAY_RollNo.ToString() == null ? " " : n.AMAY_RollNo.ToString())).Trim(),

                                        }).Distinct().ToArray();
                }

                else
                {
                    data.newstudlist = (from m in _Context.Adm_M_Student
                                        from b in _Context.School_Adm_Y_StudentDMO
                                        where (m.AMST_Id == b.AMST_Id && m.MI_Id == data.MI_Id && m.ASMAY_Id == data.AMAY_Id_New && m.ASMCL_Id == data.ASMCL_Id_New && b.ASMS_Id == data.ASMS_Id_New && m.AMST_SOL == "S" && m.AMST_ActiveFlag == 1 && b.AMAY_ActiveFlag == 1)
                                        select new SchoolYearWiseStudentDTO
                                        {
                                            AMST_Id = m.AMST_Id,
                                            AMST_FirstName = ((m.AMST_FirstName == null ? " " : m.AMST_FirstName) + (m.AMST_MiddleName == null ? " " : m.AMST_MiddleName) + (m.AMST_LastName == null ? " " : m.AMST_LastName) + ':' + (m.AMST_RegistrationNo == null ? " " : m.AMST_RegistrationNo)).Trim(),
                                        }).Distinct().ToArray();
                }
            }
            catch (Exception ex)
            { 
                _acdimpl.LogInformation("new studentlist :'" + ex.Message + "'");
                data.returnVal = false;
            }
            return data;
        }

    }
}
