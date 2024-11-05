
using DomainModel.Model.com.vaps.admission;
using PreadmissionDTOs.com.vaps.admission;
using DataAccessMsSqlServerProvider;
using AutoMapper;
using DomainModel.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class BatchwiseStudentMappingImpl : Interfaces.BatchwiseStudentMappingInterface
    {
        public DomainModelMsSqlServerContext _Context;
        public MasterSubjectContext _subject;
        public BatchwiseStudentMappingImpl(DomainModelMsSqlServerContext Admissiondbcontext, MasterSubjectContext subject)
        {
            _Context = Admissiondbcontext;
            _subject = subject;
        }

        public AdmSchoolAttendanceSubjectBatchDTO GetDropdowndetailsbyYearandInstitute(AdmSchoolAttendanceSubjectBatchDTO stu)
        {
            try
            {
                if (stu.FormType == "onload")
                {
                    List<MasterAcademic> year = new List<MasterAcademic>();

                    year = _Context.AcademicYear.Where(t => t.MI_Id == stu.MI_Id && t.Is_Active == true).ToList();
                    stu.YearList = year.OrderByDescending(a => a.ASMAY_Order).ToArray();

                    List<School_M_Class> classList = new List<School_M_Class>();
                    classList = _Context.School_M_Class.Where(t => t.MI_Id == stu.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                    stu.classList = classList.OrderBy(c => c.ASMCL_Order).ToArray();

                    List<School_M_Section> School_M_SectionList = new List<School_M_Section>();
                    School_M_SectionList = _Context.School_M_Section.Where(t => t.MI_Id == stu.MI_Id && t.ASMC_ActiveFlag == 1).ToList();
                    stu.sectionList = School_M_SectionList.OrderBy(s => s.ASMC_Order).ToArray();

                    List<IVRM_Master_SubjectsDMO> MasterSubjectList = new List<IVRM_Master_SubjectsDMO>();
                    MasterSubjectList = _subject.IVRM_Master_SubjectsDMO.Where(t => t.MI_Id == stu.MI_Id && t.ISMS_ActiveFlag == 1 && t.ISMS_BatchAppl == 1).ToList();
                    stu.SubjectList = MasterSubjectList.ToArray();

                    List<AdmSchoolAttendanceSubjectBatch> Adm_Master_BatchList = new List<AdmSchoolAttendanceSubjectBatch>();
                    Adm_Master_BatchList = _Context.AdmSchoolAttendanceSubjectBatch.Where(t => t.MI_Id == stu.MI_Id).ToList();
                    stu.batchList = Adm_Master_BatchList.ToArray();




                    stu.SubjectBatchList = (from sb in _Context.AdmSchoolAttendanceSubjectBatch
                                            from sbs in _Context.AdmSchoolAttendanceSubjectBatchStudents
                                            from ms in _Context.Adm_M_Student
                                            from mi in _Context.Institute
                                            from y in _Context.AcademicYear
                                            from cl in _Context.School_M_Class
                                            from sec in _Context.School_M_Section
                                            from sub in _Context.MasterSubjectList

                                            where sbs.ASASB_Id == sb.ASASB_Id && sbs.AMST_Id == ms.AMST_Id
                                            && mi.MI_Id == sb.MI_Id && y.ASMAY_Id == sb.ASMAY_Id &&
                                            cl.ASMCL_Id == sb.ASMCL_Id && sec.ASMS_Id == sb.ASMS_Id
                                            && sub.ISMS_Id == sb.ISMS_Id
                                            && sb.MI_Id == stu.MI_Id && ms.AMST_ActiveFlag == 1 && ms.AMST_SOL == "S"
                                            select new AdmSchoolAttendanceSubjectBatchDTO
                                            {
                                                AMST_Id = sbs.AMST_Id,
                                                AMST_FirstName = ms.AMST_FirstName,
                                                AMST_MiddleName = ms.AMST_MiddleName,
                                                AMST_LastName = ms.AMST_LastName,
                                                MI_Id = sb.MI_Id,
                                                MI_Name = mi.MI_Name,
                                                ASMAY_Id = sb.ASMAY_Id,
                                                ASMAY_Year = y.ASMAY_Year,
                                                ASMCL_Id = sb.ASMCL_Id,
                                                ASMCL_ClassName = cl.ASMCL_ClassName,
                                                ASMS_Id = sb.ASMS_Id,
                                                ASMC_SectionName = sec.ASMC_SectionName,
                                                AMSU_Id = sb.ISMS_Id,
                                                AMSU_Name = sub.ISMS_SubjectName,
                                                ASASB_BatchName = sb.ASASB_BatchName,
                                                ASASB_Id = sb.ASASB_Id,
                                                ASASBS_Id = sbs.ASASBS_Id,
                                                ASASB_StudentStrenth = sb.ASASB_StudentStrenth

                                            }).ToArray();
                    if (stu.SubjectBatchList.Length > 0)
                    {
                        stu.batchcount = stu.SubjectBatchList.Length;
                    }
                    else
                    {
                        stu.batchcount = 0;
                    }



                    //stu.StudentList = (from s in _Context.M_Student
                    //                   where s.AMST_ActiveFlag == 1 && s.MI_Id == 2    // stu.MI_Id
                    //                   select s).ToArray();




                }
                else if (stu.FormType == "onselect")
                {
                    stu.StudentList = (from s in _Context.Adm_M_Student
                                       from sy in _Context.SchoolYearWiseStudent
                                       from c in _Context.School_M_Class
                                       from d in _Context.School_M_Section

                                       where c.ASMCL_Id == sy.ASMCL_Id && d.ASMS_Id == sy.ASMS_Id && sy.AMST_Id == s.AMST_Id && s.AMST_ActiveFlag == 1 && s.AMST_SOL == "S" && s.MI_Id == stu.MI_Id   // stu.MI_Id
                                       && sy.ASMS_Id == stu.ASMS_Id && sy.ASMCL_Id == stu.ASMCL_Id && sy.ASMAY_Id == stu.ASMAY_Id && sy.AMAY_ActiveFlag == 1
                                       select new AdmSchoolAttendanceSubjectBatchDTO
                                       {
                                           AMST_Id = s.AMST_Id,
                                           studentname = ((s.AMST_FirstName == null || s.AMST_FirstName == "" ? "" : " " + s.AMST_FirstName) + (s.AMST_MiddleName == null || s.AMST_MiddleName == "" || s.AMST_MiddleName == "0" ? "" : " " + s.AMST_MiddleName) + (s.AMST_LastName == null || s.AMST_LastName == "" || s.AMST_LastName == "0" ? "" : " " + s.AMST_LastName)).Trim(),

                                           amsT_AdmNo = s.AMST_AdmNo,
                                           amsT_RegistrationNo = s.AMST_RegistrationNo

                                       }).ToArray();
                    if (stu.StudentList.Length > 0)
                    {
                        stu.studentCount = stu.StudentList.Length;
                    }
                    else
                    {
                        stu.studentCount = 0;
                    }

                }
                else if (stu.FormType == "onedit")
                {
                    stu.StudentList = (from s in _Context.Adm_M_Student
                                       from sy in _Context.SchoolYearWiseStudent
                                       from c in _Context.School_M_Class
                                       from d in _Context.School_M_Section

                                       where c.ASMCL_Id == sy.ASMCL_Id && d.ASMS_Id == sy.ASMS_Id && sy.AMST_Id == s.AMST_Id && s.AMST_ActiveFlag == 1 && s.AMST_SOL == "S" && s.MI_Id == stu.MI_Id   // stu.MI_Id
                                       && sy.ASMS_Id == stu.ASMS_Id && sy.ASMCL_Id == stu.ASMCL_Id && sy.ASMAY_Id == stu.ASMAY_Id && sy.AMAY_ActiveFlag == 1
                                       select new AdmSchoolAttendanceSubjectBatchDTO
                                       {
                                           AMST_Id = s.AMST_Id,
                                           studentname = ((s.AMST_FirstName == null || s.AMST_FirstName == "" ? "" : " " + s.AMST_FirstName) + (s.AMST_MiddleName == null || s.AMST_MiddleName == "" || s.AMST_MiddleName == "0" ? "" : " " + s.AMST_MiddleName) + (s.AMST_LastName == null || s.AMST_LastName == "" || s.AMST_LastName == "0" ? "" : " " + s.AMST_LastName)).Trim(),

                                           amsT_AdmNo = s.AMST_AdmNo,
                                           amsT_RegistrationNo = s.AMST_RegistrationNo

                                       }).ToArray();

                    if (stu.StudentList.Length > 0)
                    {
                        stu.studentCount = stu.StudentList.Length;
                    }
                    else
                    {
                        stu.studentCount = 0;
                    }

                    stu.EditedStudentList = (from s in _Context.Adm_M_Student
                                             from sy in _Context.SchoolYearWiseStudent
                                             from c in _Context.School_M_Class
                                             from d in _Context.School_M_Section

                                             where c.ASMCL_Id == sy.ASMCL_Id && d.ASMS_Id == sy.ASMS_Id && sy.AMST_Id == s.AMST_Id && s.AMST_Id == stu.AMST_Id
                                             select new AdmSchoolAttendanceSubjectBatchDTO
                                             {
                                                 AMST_Id = s.AMST_Id,
                                                 studentname = ((s.AMST_FirstName == null || s.AMST_FirstName == "" ? "" : " " + s.AMST_FirstName) + (s.AMST_MiddleName == null || s.AMST_MiddleName == "" || s.AMST_MiddleName == "0" ? "" : " " + s.AMST_MiddleName) + (s.AMST_LastName == null || s.AMST_LastName == "" || s.AMST_LastName == "0" ? "" : " " + s.AMST_LastName)).Trim(),

                                                 amsT_AdmNo = s.AMST_AdmNo,
                                                 amsT_RegistrationNo = s.AMST_RegistrationNo

                                             }).ToArray();

                }
                else if (stu.FormType == "filter")
                {
                    var batch = _Context.AdmSchoolAttendanceSubjectBatch.Where(b => b.ASMAY_Id == stu.ASMAY_Id &&
                    b.ASMCL_Id == stu.ASMCL_Id && b.ASMS_Id == stu.ASMS_Id && b.ISMS_Id == stu.AMSU_Id).Select(b => b.ASASB_Id).ToArray();
                    if (batch != null)
                    {
                        var batchsub = _Context.AdmSchoolAttendanceSubjectBatchStudents.Where(b => b.ASASB_Id != 0 && batch.Contains(b.ASASB_Id)).Select(b => b.AMST_Id).ToArray();
                        if (batchsub != null)
                        {
                            stu.StudentList = (from s in _Context.Adm_M_Student
                                               from sy in _Context.SchoolYearWiseStudent
                                               from c in _Context.School_M_Class
                                               from d in _Context.School_M_Section

                                               where c.ASMCL_Id == sy.ASMCL_Id && d.ASMS_Id == sy.ASMS_Id && sy.AMST_Id == s.AMST_Id && s.AMST_ActiveFlag == 1 && s.AMST_SOL == "S" && s.MI_Id == stu.MI_Id   // stu.MI_Id
                                               && sy.ASMS_Id == stu.ASMS_Id && sy.ASMCL_Id == stu.ASMCL_Id && sy.ASMAY_Id == stu.ASMAY_Id && sy.AMAY_ActiveFlag == 1 && !batchsub.Contains(s.AMST_Id)
                                               select new AdmSchoolAttendanceSubjectBatchDTO
                                               {
                                                   AMST_Id = s.AMST_Id,
                                                   studentname = ((s.AMST_FirstName == null || s.AMST_FirstName == "" ? "" : " " + s.AMST_FirstName) + (s.AMST_MiddleName == null || s.AMST_MiddleName == "" || s.AMST_MiddleName == "0" ? "" : " " + s.AMST_MiddleName) + (s.AMST_LastName == null || s.AMST_LastName == "" || s.AMST_LastName == "0" ? "" : " " + s.AMST_LastName)).Trim(),

                                                   amsT_AdmNo = s.AMST_AdmNo,
                                                   amsT_RegistrationNo = s.AMST_RegistrationNo

                                               }).ToArray();
                        }
                        else
                        {
                            stu.StudentList = (from s in _Context.Adm_M_Student
                                               from sy in _Context.SchoolYearWiseStudent
                                               from c in _Context.School_M_Class
                                               from d in _Context.School_M_Section

                                               where c.ASMCL_Id == sy.ASMCL_Id && d.ASMS_Id == sy.ASMS_Id && sy.AMST_Id == s.AMST_Id && s.AMST_ActiveFlag == 1 && s.AMST_SOL == "S" && s.MI_Id == stu.MI_Id   // stu.MI_Id
                                               && sy.ASMS_Id == stu.ASMS_Id && sy.ASMCL_Id == stu.ASMCL_Id && sy.ASMAY_Id == stu.ASMAY_Id && sy.AMAY_ActiveFlag == 1
                                               select new AdmSchoolAttendanceSubjectBatchDTO
                                               {
                                                   AMST_Id = s.AMST_Id,
                                                   studentname = ((s.AMST_FirstName == null || s.AMST_FirstName == "" ? "" : " " + s.AMST_FirstName) + (s.AMST_MiddleName == null || s.AMST_MiddleName == "" || s.AMST_MiddleName == "0" ? "" : " " + s.AMST_MiddleName) + (s.AMST_LastName == null || s.AMST_LastName == "" || s.AMST_LastName == "0" ? "" : " " + s.AMST_LastName)).Trim(),

                                                   amsT_AdmNo = s.AMST_AdmNo,
                                                   amsT_RegistrationNo = s.AMST_RegistrationNo

                                               }).ToArray();
                        }
                    }
                    else
                    {
                        stu.StudentList = (from s in _Context.Adm_M_Student
                                           from sy in _Context.SchoolYearWiseStudent
                                           from c in _Context.School_M_Class
                                           from d in _Context.School_M_Section

                                           where c.ASMCL_Id==sy.ASMCL_Id && d.ASMS_Id==sy.ASMS_Id &&  sy.AMST_Id == s.AMST_Id && s.AMST_ActiveFlag == 1 && s.AMST_SOL == "S" && s.MI_Id == stu.MI_Id   // stu.MI_Id
                                           && sy.ASMS_Id == stu.ASMS_Id && sy.ASMCL_Id == stu.ASMCL_Id && sy.ASMAY_Id == stu.ASMAY_Id && sy.AMAY_ActiveFlag == 1
                                           select new AdmSchoolAttendanceSubjectBatchDTO
                                           {
                                               AMST_Id = s.AMST_Id,
                                               studentname = ((s.AMST_FirstName == null || s.AMST_FirstName == "" ? "" : " " + s.AMST_FirstName) + (s.AMST_MiddleName == null || s.AMST_MiddleName == "" || s.AMST_MiddleName == "0" ? "" : " " + s.AMST_MiddleName) + (s.AMST_LastName == null || s.AMST_LastName == "" || s.AMST_LastName == "0" ? "" : " " + s.AMST_LastName)).Trim(),

                                               amsT_AdmNo = s.AMST_AdmNo,
                                               amsT_RegistrationNo = s.AMST_RegistrationNo

                                           }).ToArray();
                    }
                }

                else if (stu.FormType == "deleterecord")
                {
                    List<AdmSchoolAttendanceSubjectBatchStudents> masters = new List<AdmSchoolAttendanceSubjectBatchStudents>();
                    masters = _Context.AdmSchoolAttendanceSubjectBatchStudents.Where(t => t.ASASBS_Id == stu.ASASBS_Id).ToList();
                    if (masters.Any())
                    {
                        _Context.Remove(masters.ElementAt(0));
                        var flag = _Context.SaveChanges();
                        if (flag > 0)
                        {
                            stu.returnVal = true;
                        }
                        else
                        {
                            stu.returnVal = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return stu;
        }




        public AdmSchoolAttendanceSubjectBatchDTO GetSelectedRowDetails(AdmSchoolAttendanceSubjectBatchDTO stu)
        {
            try
            {
                //stu.StudentList = (from s in _Context.Adm_M_Student
                //                   from ssy in _Context.AdmSchoolAttendanceSubjectBatchStudents
                //                   from ssst in _Context.AdmSchoolAttendanceSubjectBatch

                //                   where s.AMST_ActiveFlag == 1 && s.MI_Id == stu.MI_Id

                //                   && s.AMST_Id == stu.AMST_Id && ssy.AMST_Id == stu.AMST_Id


                //                   select new AdmSchoolAttendanceSubjectBatchDTO
                //                   {
                //                       AMAY_Id = s.ASMAY_Id,    //year
                //                       AMCL_Id= s.ASMCL_Id,    //class
                //                       ASASB_BatchName = ssst.ASASB_BatchName,   //batch_name
                //                       ASASBS_Id= ssy.ASASBS_Id,   //batch_id
                //                       AMS_Id = ssst.AMS_Id,     //section
                //                       ISMS_Id = ssst.ISMS_Id    //subject
                //                   }).ToArray();




                stu.StudentList = (from adm_stu in _Context.Adm_M_Student
                                   from sbs in _Context.AdmSchoolAttendanceSubjectBatchStudents
                                   from sb in _Context.AdmSchoolAttendanceSubjectBatch
                                   from cl in _Context.School_M_Class
                                   where adm_stu.AMST_ActiveFlag == 1 && adm_stu.MI_Id == stu.MI_Id && adm_stu.AMST_SOL == "S"
                                   && sbs.ASASBS_Id == stu.ASASBS_Id && cl.ASMCL_Id == stu.ASMCL_Id
                                   && adm_stu.AMST_Id == sbs.AMST_Id && sbs.ASASB_Id == sb.ASASB_Id

                                   select new AdmSchoolAttendanceSubjectBatchDTO
                                   {
                                       ASMAY_Id = adm_stu.ASMAY_Id,    //year
                                       ASMCL_Id = cl.ASMCL_Id,    //class
                                       ASMS_Id = sb.ASMS_Id,     //section
                                       AMSU_Id = sb.ISMS_Id,    //subject
                                       ASASB_BatchName = sb.ASASB_BatchName,   //batch_name
                                       ASASBS_Id = sbs.ASASBS_Id, //batch id
                                       AMST_Id = adm_stu.AMST_Id  //Student Id
                                   }).ToArray();
            }

            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return stu;
        }
        public AdmSchoolAttendanceSubjectBatchDTO saveAdmSchoolAttendanceSubjectBatch(AdmSchoolAttendanceSubjectBatchDTO secAllt)
        {
            //store procedure

            AdmSchoolAttendanceSubjectBatch asb = Mapper.Map<AdmSchoolAttendanceSubjectBatch>(secAllt);

            int count = 0;
            try
            {

                string str = "0";
                var message1 = "";
                foreach (AdmSchoolAttendanceSubjectBatchStudentsDTO actv in secAllt.selectedstudents)
                {
                    str = str + "," + actv.AMST_Id;
                }

                List<AdmSchoolAttendanceSubjectBatchStudentsDTO> result1 = new List<AdmSchoolAttendanceSubjectBatchStudentsDTO>();
                using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_Get_Batchwise_Student_ListFor_Update";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                    {
                        Value = secAllt.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                        SqlDbType.BigInt)
                    {
                        Value = secAllt.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id",
                       SqlDbType.BigInt)
                    {
                        Value = secAllt.AMSU_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                      SqlDbType.BigInt)
                    {
                        Value = secAllt.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                    SqlDbType.BigInt)
                    {
                        Value = secAllt.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASASB_Id",
                   SqlDbType.BigInt)
                    {
                        Value = secAllt.ASASB_Id1
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                       SqlDbType.VarChar)
                    {
                        Value = str
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();

                    try
                    {
                        // var data = cmd.ExecuteNonQuery();

                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result1.Add(new AdmSchoolAttendanceSubjectBatchStudentsDTO
                                {
                                    ASASBS_Id = Convert.ToInt64(dataReader["ASASBS_Id"])
                                });
                                secAllt.activityIds = result1.ToArray();
                            }
                        }

                        if (secAllt.activityIds != null)
                        {
                            foreach (AdmSchoolAttendanceSubjectBatchStudentsDTO act in secAllt.activityIds)
                            {
                                var Activityresult = _Context.AdmSchoolAttendanceSubjectBatchStudents.Where(t => t.ASASBS_Id == act.ASASBS_Id).ToList();
                                if (Activityresult.Any())
                                {
                                    _Context.Remove(Activityresult.ElementAt(0));
                                    _Context.SaveChanges();
                                    message1 = "Un Checked Records Deleted";
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                List<AdmSchoolAttendanceSubjectBatch> checkDuplicate = new List<AdmSchoolAttendanceSubjectBatch>();
                if (secAllt.ASASB_Id1 > 0)
                {
                    checkDuplicate = _Context.AdmSchoolAttendanceSubjectBatch.
                            Where(d => d.MI_Id == secAllt.MI_Id
                            && d.ASMAY_Id == secAllt.ASMAY_Id
                            && d.ASMCL_Id == secAllt.ASMCL_Id
                            && d.ASMS_Id == secAllt.ASMS_Id
                            && d.ISMS_Id == secAllt.AMSU_Id && d.ASASB_Id == secAllt.ASASB_Id1
                            ).ToList();
                }
                else
                {
                    checkDuplicate = _Context.AdmSchoolAttendanceSubjectBatch.
                           Where(d => d.MI_Id == secAllt.MI_Id
                           && d.ASMAY_Id == secAllt.ASMAY_Id
                           && d.ASMCL_Id == secAllt.ASMCL_Id
                           && d.ASMS_Id == secAllt.ASMS_Id
                           && d.ISMS_Id == secAllt.AMSU_Id
                           && d.ASASB_BatchName.Equals(secAllt.ASASB_BatchName)).ToList();
                }



                if (asb.ASASB_Id > 0)
                {
                    var result = _Context.AdmSchoolAttendanceSubjectBatch.Single(s => s.ASASB_Id == asb.ASASB_Id);
                    result.ASASB_BatchName = asb.ASASB_BatchName;
                    result.UpdatedDate = DateTime.Now;
                    _Context.Update(result);
                    count = _Context.SaveChanges();
                    secAllt.ASASB_Id = result.ASASB_Id;
                }
                else
                {
                    if (checkDuplicate.Count > 0)
                    {
                        secAllt.ASASB_Id = checkDuplicate.FirstOrDefault().ASASB_Id;
                    }
                    else
                    {
                        asb.CreatedDate = DateTime.Now;
                        asb.UpdatedDate = DateTime.Now;
                        asb.ISMS_Id = secAllt.AMSU_Id;
                        _Context.Add(asb);
                        count = _Context.SaveChanges();
                        secAllt.ASASB_Id = asb.ASASB_Id;
                    }
                }
                if (secAllt.ASASB_Id > 0)
                {
                    //add/update details.
                    if (secAllt.selectedstudents.Count() > 0)
                    {
                        for (int i = 0; i < secAllt.selectedstudents.Length; i++)
                        {
                            var duplicateCheck = (from m in _Context.AdmSchoolAttendanceSubjectBatch
                                                  from n in _Context.AdmSchoolAttendanceSubjectBatchStudents
                                                  where (m.ASASB_Id == n.ASASB_Id && n.AMST_Id == secAllt.selectedstudents[i].AMST_Id && m.ASMAY_Id == secAllt.ASMAY_Id
                                                  && m.ASMCL_Id == secAllt.ASMCL_Id && m.ASMS_Id == secAllt.ASMS_Id
                                                  && m.ISMS_Id == secAllt.AMSU_Id && m.MI_Id == secAllt.MI_Id)
                                                  select new AdmSchoolAttendanceSubjectBatchDTO
                                                  {
                                                      AMST_Id = n.AMST_Id
                                                  }).ToList();
                            if (secAllt.selectedstudents[i].ASASBS_Id > 0)
                            {
                                var MobileNoresult = _Context.AdmSchoolAttendanceSubjectBatchStudents.Single(s => s.ASASBS_Id == secAllt.selectedstudents[i].ASASBS_Id);
                                MobileNoresult.UpdatedDate = DateTime.Now;
                                _Context.Update(MobileNoresult);
                            }
                            else
                            {
                                AdmSchoolAttendanceSubjectBatchStudents stud = new AdmSchoolAttendanceSubjectBatchStudents();
                                if (duplicateCheck.Count > 0)
                                {
                                    secAllt.message = "Some Duplicate record exist......!!";
                                }
                                else
                                {
                                    stud.AMST_Id = secAllt.selectedstudents[i].AMST_Id;
                                    stud.ASASB_Id = secAllt.ASASB_Id;
                                    stud.CreatedDate = DateTime.Now;
                                    stud.UpdatedDate = DateTime.Now;
                                    _Context.Add(stud);
                                }

                            }
                            var flag = _Context.SaveChanges();
                            if (flag > 0)
                            {
                                secAllt.returnVal = true;
                            }
                            else
                            {
                                secAllt.returnVal = false;
                            }
                        }
                    }
                    if (message1 != "")
                    {
                        secAllt.message = secAllt.message + message1;
                    }
                    int StudentStrenth = (from m in _Context.Adm_M_Student
                                          from n in _Context.AdmSchoolAttendanceSubjectBatchStudents
                                          where m.AMST_Id == n.AMST_Id && m.AMST_SOL.Equals("S") && m.AMST_ActiveFlag == 1 && n.ASASB_Id == secAllt.ASASB_Id
                                          select n.AMST_Id).Count();

                    //   _Context.AdmSchoolAttendanceSubjectBatchStudents.Where(s => s.ASASB_Id == secAllt.ASASB_Id).Count();
                    if (StudentStrenth > 0)
                    {
                        var updateStudentStrenth = _Context.AdmSchoolAttendanceSubjectBatch.Single(s => s.ASASB_Id == secAllt.ASASB_Id);
                        updateStudentStrenth.ASASB_StudentStrenth = StudentStrenth;
                        _Context.Update(updateStudentStrenth);
                        int updatecount = _Context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return secAllt;
        }

        public AdmSchoolAttendanceSubjectBatchDTO getbatchwisestdlist(AdmSchoolAttendanceSubjectBatchDTO data)
        {
            try
            {
                data.batchwisestdlist = (from a in _Context.AdmSchoolAttendanceSubjectBatch
                                         from b in _Context.AdmSchoolAttendanceSubjectBatchStudents
                                         from c in _Context.Adm_M_Student
                                         from d in _Context.School_Adm_Y_StudentDMO
                                         where a.ASASB_Id == b.ASASB_Id && b.AMST_Id == d.AMST_Id && d.AMST_Id == c.AMST_Id && a.MI_Id == data.MI_Id
                                         && a.ISMS_Id == data.AMSU_Id && a.ASASB_Id == data.ASASB_Id && a.ASMAY_Id == d.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id
                                         select new AdmSchoolAttendanceSubjectBatchDTO
                                         {
                                             studentname = ((c.AMST_FirstName == null || c.AMST_FirstName == "" ? "" : " " + c.AMST_FirstName.Trim()) + (c.AMST_MiddleName == null || c.AMST_MiddleName == "" || c.AMST_MiddleName == "0" ? "" : " " + c.AMST_MiddleName.Trim()) + (c.AMST_LastName == null || c.AMST_LastName == "" || c.AMST_LastName == "0" ? "" : " " + c.AMST_LastName.Trim())),
                                             admno = c.AMST_AdmNo,
                                             regno = c.AMST_RegistrationNo,
                                             AMST_Id = c.AMST_Id

                                         }).ToArray();


                if (data.batchwisestdlist.Length > 0)
                {
                    data.countbatchlist = data.batchwisestdlist.Length;
                }
                else
                {
                    data.countbatchlist = 0;
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public AdmSchoolAttendanceSubjectBatchDTO getbatchname(AdmSchoolAttendanceSubjectBatchDTO data)
        {
            try
            {
                data.batchList = _Context.AdmSchoolAttendanceSubjectBatch.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.ISMS_Id == data.AMSU_Id).ToArray();
                if (data.batchList.Length > 0)
                {
                    data.countbatchlist = data.batchList.Length;
                }
                else
                {
                    data.countbatchlist = 0;
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
