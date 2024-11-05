using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Portals.Student;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;

namespace PortalHub.com.vaps.HOD.Services
{
    public class HODStudentSearchImpl : Interfaces.HODStudentSearchInterface
    {
        public FeeGroupContext _fees;
        public ExamContext _exam;
        private PortalContext _Examcontext;
        public DomainModelMsSqlServerContext _db;
        public HODStudentSearchImpl(FeeGroupContext fees, ExamContext exm, PortalContext Examcontext, DomainModelMsSqlServerContext db)
        {
            _fees = fees;
            _exam = exm;
            _Examcontext = Examcontext;
            _db = db;
        }
        public StudentSearchDTO getdatastuacadgrp(StudentSearchDTO data)
        {
            try
            {
                // data.HRME_Id = _exam.Staff_User_Login.Single(c => c.Id == data.userid && c.MI_Id == data.MI_Id).Emp_Code;

                data.fillstudent = (from a in _fees.AdmissionStudentDMO
                                    from b in _fees.School_Adm_Y_StudentDMO
                                    from c in _fees.admissioncls
                                    from d in _fees.school_M_Section
                                    where (b.ASMCL_Id == c.ASMCL_Id && d.ASMS_Id == b.ASMS_Id && a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id)
                                    select new StudentSearchDTO
                                    {
                                        Amst_Id = a.AMST_Id,
                                        //AMST_FirstName = a.AMST_FirstName,
                                        AMST_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),
                                        AMST_MiddleName = a.AMST_MiddleName,
                                        AMST_LastName = a.AMST_LastName,
                                    }
                   ).Distinct().ToArray();

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
                                              where (b.ASMCL_Id == c.ASMCL_Id && d.ASMS_Id == b.ASMS_Id && a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == e.ASMAY_Id && e.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.Amst_Id)
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
                                                  asma_year = e.ASMAY_Year
                                              }
                    ).Distinct().ToArray();

                data.examlist = (from a in _exam.ExmStudentMarksProcessDMO
                                 from b in _exam.exammasterDMO
                                 where (a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.Amst_Id && a.EME_Id == b.EME_Id && b.EME_ActiveFlag == true)
                                 select new StudentSearchDTO
                                 {
                                     exam_name = b.EME_ExamName,
                                     totalmarks = a.ESTMP_TotalMaxMarks,
                                     obtainmarks = a.ESTMP_TotalObtMarks,
                                     persentage = a.ESTMP_Percentage,
                                     result = a.ESTMP_Result
                                 }
                               ).Distinct().ToArray();


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

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

    }
}
