using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Exam;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Services
{
    public class EmployeeStudentSearchImpl : Interfaces.EmployeeStudentSearchInterface
    {
        public FeeGroupContext _fees;
        public ExamContext _exam;
        public DomainModelMsSqlServerContext _db;

        public EmployeeStudentSearchImpl(FeeGroupContext fees, ExamContext exm, DomainModelMsSqlServerContext db)
        {
            _fees = fees;
            _exam = exm;
            _db = db;
        }

        public EmployeeDashboardDTO getdatastuacadgrp(EmployeeDashboardDTO data)
        {
            try
            {
                data.HRME_Id = _exam.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;

                var loginData = _db.Staff_User_Login.Where(d => d.Id == data.UserId).ToList();

                var searchlog = _db.HOD_DMO.Where(a => a.HRME_Id == data.HRME_Id).ToList();
                if (searchlog.Count > 0)
                {
                    var Fillstudentstrenth = (from a in _db.School_Adm_Y_StudentDMO
                                              from b in _db.admissioncls
                                              from c in _db.Adm_M_Student
                                              from d in _db.IVRM_HOD_Class_DMO
                                              from e in _db.HOD_DMO
                                              where (a.AMST_Id == c.AMST_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                              && a.ASMAY_Id == c.ASMAY_Id && c.AMST_SOL.Equals("S") && c.AMST_ActiveFlag == 1 && a.AMAY_ActiveFlag == 1
                                              && a.ASMCL_Id == d.ASMCL_Id && d.IHOD_Id == e.IHOD_Id && e.HRME_Id == loginData.FirstOrDefault().Emp_Code
                                              && e.IHOD_Flg == "HOD" && e.IHOD_ActiveFlag == true)
                                              group new { a, b } by a.ASMCL_Id into g
                                              select new ADMClassSectionStrengthDTO
                                              {
                                                  ASMAY_Id = g.FirstOrDefault().a.ASMAY_Id,
                                                  asmcL_Id = g.FirstOrDefault().a.ASMCL_Id,
                                                  asmS_Id = g.FirstOrDefault().a.ASMS_Id,
                                                  Class_Name = g.FirstOrDefault().b.ASMCL_ClassName,
                                                  stud_count = g.Count()
                                              }).Distinct().ToList();

                    List<long> classsids = new List<long>();
                    List<long> sectionsids = new List<long>();
                    foreach (var classs in Fillstudentstrenth)
                    {
                        classsids.Add(classs.asmcL_Id);
                    }
                    foreach (var sectins in Fillstudentstrenth)
                    {
                        sectionsids.Add(sectins.asmS_Id);
                    }

                    data.fillstudent = (from a in _fees.AdmissionStudentDMO
                                        from b in _fees.School_Adm_Y_StudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S"
                                        && b.AMAY_ActiveFlag == 1 && classsids.Contains(b.ASMCL_Id) && sectionsids.Contains(b.ASMS_Id) && a.ASMAY_Id == b.ASMAY_Id)
                                        select new EmployeeDashboardDTO
                                        {
                                            Amst_Id = a.AMST_Id,                                            
                                            amst_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "" ? "" : a.AMST_FirstName) +
                                            (a.AMST_MiddleName == null || a.AMST_MiddleName == "" ? "" : " " + a.AMST_MiddleName) +
                                            (a.AMST_LastName == null || a.AMST_LastName == "" ? "" : " " + a.AMST_LastName) +
                                            (a.AMST_AdmNo == null || a.AMST_AdmNo == "" ? "" : " : " + a.AMST_AdmNo)).Trim(),
                                            AMST_MiddleName = a.AMST_MiddleName,
                                            AMST_LastName = a.AMST_LastName,
                                            studentnameorder = ((a.AMST_FirstName == null || a.AMST_FirstName == "" ? "" : a.AMST_FirstName) +
                                           (a.AMST_MiddleName == null || a.AMST_MiddleName == "" || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) +
                                           (a.AMST_LastName == null || a.AMST_LastName == "" || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim(),
                                        }).Distinct().OrderBy(t => t.studentnameorder).ToArray();

                }

                else
                {
                    var Fillstudentstrenth = (from a in _db.School_Adm_Y_StudentDMO
                                              from b in _db.admissioncls
                                              from c in _db.Adm_M_Student
                                              where (a.AMST_Id == c.AMST_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id 
                                              && a.ASMAY_Id == c.ASMAY_Id && c.AMST_SOL.Equals("S") && c.AMST_ActiveFlag == 1 && a.AMAY_ActiveFlag == 1)
                                              group new { a, b } by a.ASMCL_Id into g
                                              select new ADMClassSectionStrengthDTO
                                              {
                                                  ASMAY_Id = g.FirstOrDefault().a.ASMAY_Id,
                                                  asmcL_Id = g.FirstOrDefault().a.ASMCL_Id,
                                                  asmS_Id = g.FirstOrDefault().a.ASMS_Id,
                                                  Class_Name = g.FirstOrDefault().b.ASMCL_ClassName,
                                                  stud_count = g.Count()
                                              }).Distinct().ToList();

                    List<long> classsids = new List<long>();
                    List<long> sectionsids = new List<long>();

                    foreach (var classs in Fillstudentstrenth)
                    {
                        classsids.Add(classs.asmcL_Id);
                    }
                    foreach (var sectins in Fillstudentstrenth)
                    {
                        sectionsids.Add(sectins.asmS_Id);
                    }

                    data.fillstudent = (from a in _fees.School_Adm_Y_StudentDMO
                                        from b in _fees.ClassTeacherMappingDMO
                                        from c in _fees.AdmissionStudentDMO
                                        where (a.ASMCL_Id == b.ASMCL_Id && a.ASMS_Id == b.ASMS_Id && b.HRME_Id == data.HRME_Id && c.AMST_Id == a.AMST_Id
                                        && c.AMST_SOL == "S" && a.AMAY_ActiveFlag == 1 && a.ASMAY_Id == data.ASMAY_Id && b.ASMAY_Id == data.ASMAY_Id
                                        && b.MI_Id == data.MI_Id && b.IMCT_ActiveFlag == true)
                                        select new EmployeeDashboardDTO
                                        {
                                            Amst_Id = a.AMST_Id,                                            
                                            amst_FirstName = ((c.AMST_FirstName == null || c.AMST_FirstName == "" ? "" : c.AMST_FirstName) +
                                            (c.AMST_MiddleName == null || c.AMST_MiddleName == "" ? "" : " " + c.AMST_MiddleName) +
                                            (c.AMST_LastName == null || c.AMST_LastName == "" ? "" : " " + c.AMST_LastName) +
                                            (c.AMST_AdmNo == null || c.AMST_AdmNo == "" ? "" : " : " + c.AMST_AdmNo)).Trim(),
                                            AMST_MiddleName = c.AMST_MiddleName,
                                            AMST_LastName = c.AMST_LastName,
                                            studentnameorder = ((c.AMST_FirstName == null || c.AMST_FirstName == "" ? "" : c.AMST_FirstName) +
                                           (c.AMST_MiddleName == null || c.AMST_MiddleName == "" || c.AMST_MiddleName == "0" ? "" : " " + c.AMST_MiddleName) +
                                           (c.AMST_LastName == null || c.AMST_LastName == "" || c.AMST_LastName == "0" ? "" : " " + c.AMST_LastName)).Trim(),
                                        }).Distinct().OrderBy(t => t.studentnameorder).ToArray();
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public EmployeeDashboardDTO getstudentdetails(EmployeeDashboardDTO data)
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
                                              select new EmployeeDashboardDTO
                                              {                                                 
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
                                                  AMST_Photoname = a.AMST_Photoname,
                                              }).Distinct().ToArray();

                data.examlist = (from a in _exam.ExmStudentMarksProcessDMO
                                 from b in _exam.exammasterDMO
                                 where (a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.Amst_Id
                                 && a.EME_Id == b.EME_Id && b.EME_ActiveFlag == true)
                                 select new EmployeeDashboardDTO
                                 {
                                     exam_name = b.EME_ExamName,
                                     totalmarks = a.ESTMP_TotalMaxMarks,
                                     obtainmarks = a.ESTMP_TotalObtMarks,
                                     persentage = a.ESTMP_Percentage,
                                     result = a.ESTMP_Result
                                 }).Distinct().ToArray();

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
    }
}