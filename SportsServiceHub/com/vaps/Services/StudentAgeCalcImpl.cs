using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Sports;
using DomainModel.Model.com.vapstech.Sports;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Sports;
using SportsServiceHub.com.vaps.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Services
{
    public class StudentAgeCalcImpl : StudentAgeCalcInterface
    {
        DomainModelMsSqlServerContext _db;
        SportsContext _context;
        public StudentAgeCalcImpl(DomainModelMsSqlServerContext db, SportsContext context)
        {
            _db = db;
            _context = context;
        }

        public StudentAgeCalcDTO Getdetails(StudentAgeCalcDTO data)
        {
            try
            {
                data.academicYear = _context.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();
                data.classList = _db.School_M_Class.Where(d => d.MI_Id == data.MI_Id && d.ASMCL_ActiveFlag == true).ToArray();
                data.sectionList = _db.School_M_Section.Where(d => d.MI_Id == data.MI_Id && d.ASMC_ActiveFlag == 1).ToArray();

                var eventsStudentRecordList = (from a in _context.Adm_M_Student
                                               from b in _context.AcademicYear
                                               from c in _context.admissionClass
                                               from d in _context.masterSection
                                               from e in _context.StudentAgeCalcDMO
                                               where (a.AMST_Id == e.AMST_Id && e.ASMAY_Id == b.ASMAY_Id && e.ASMCL_Id == c.ASMCL_Id && e.ASMS_Id == d.ASMS_Id && e.MI_Id == data.MI_Id)
                                               select new StudentAgeCalcDTO
                                               {
                                                   ASMAY_Year = b.ASMAY_Year,
                                                   ASMCL_ClassName = c.ASMCL_ClassName,
                                                   ASMC_SectionName = d.ASMC_SectionName,
                                                   studentName = a.AMST_FirstName + (string.IsNullOrEmpty(a.AMST_MiddleName) ? "" : ' ' + a.AMST_MiddleName) + (string.IsNullOrEmpty(a.AMST_LastName) ? "" : ' ' + a.AMST_LastName),
                                                   Age_Years = e.Age_Years,
                                                   Age_Months = e.Age_Months,
                                                   Age_Days = e.Age_Days
                                               }).Distinct().OrderBy(t => t.AMST_Id).ToList();
                if (eventsStudentRecordList.Count > 0)
                {
                    data.eventsStudentRecordList = eventsStudentRecordList.ToArray();
                    data.count = eventsStudentRecordList.Count;
                }
                else
                {
                    data.count = 0;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public StudentAgeCalcDTO getStudents(StudentAgeCalcDTO data)
        {
            try
            {
                var student = (from m in _db.School_Adm_Y_StudentDMO
                               from n in _db.Adm_M_Student
                               where m.AMST_Id == n.AMST_Id && m.AMAY_ActiveFlag == 1 && n.AMST_ActiveFlag == 1 && m.ASMAY_Id == data.ASMAY_Id && n.MI_Id == data.MI_Id && n.AMST_SOL.Equals("S") && m.ASMCL_Id == data.ASMCL_Id && m.ASMS_Id == data.ASMS_Id
                               select new StudentAgeCalcDTO
                               {
                                   AMST_Id = m.AMST_Id,
                                   studentName = n.AMST_FirstName + (string.IsNullOrEmpty(n.AMST_MiddleName) ? "" : ' ' + n.AMST_MiddleName) + (string.IsNullOrEmpty(n.AMST_LastName) ? "" : ' ' + n.AMST_LastName),
                               }).Distinct().OrderBy(n => n.studentName).ToList();
                if (student.Count > 0)
                {
                    data.studentList = student.ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public StudentAgeCalcDTO saveRecord(StudentAgeCalcDTO obj)
        {
            try
            {
                for (int i = 0; i < obj.student.Length; i++)
                {
                    var checkduplicate = _context.StudentAgeCalcDMO.Where(d => d.MI_Id == obj.MI_Id && d.ASMAY_Id == obj.ASMAY_Id && d.ASMCL_Id.Equals(obj.ASMCL_Id) && d.SPCCAC_Id != obj.SPCCAC_Id && d.ASMS_Id.Equals(obj.ASMS_Id) && d.AMST_Id == obj.student[i].AMST_Id).ToList();
                    if (checkduplicate.Count > 0)
                    {
                        obj.returnVal = "duplicate";
                    }
                    else
                    {
                        var mapp = Mapper.Map<StudentAgeCalcDMO>(obj);
                        var AMST_DOB = _context.Adm_M_Student.Single(a => a.MI_Id == obj.MI_Id && a.AMST_Id == obj.student[i].AMST_Id).AMST_DOB;
                        DateTime dob = AMST_DOB;
                        DateTime Today = obj.Till_Date;
                        TimeSpan ts = Today - dob;
                        DateTime Age = DateTime.MinValue + ts;
                        obj.Age_Years = Age.Year - 1;
                        obj.Age_Months = Age.Month - 1;
                        obj.Age_Days = Age.Day - 1;

                        mapp.MI_Id = obj.MI_Id;
                        mapp.ASMAY_Id = obj.ASMAY_Id;
                        mapp.AMST_Id = obj.student[i].AMST_Id;
                        mapp.ASMCL_Id = obj.ASMCL_Id;
                        mapp.ASMS_Id = obj.ASMS_Id;
                        mapp.Age_Years = obj.Age_Years;
                        mapp.Age_Months = obj.Age_Months;
                        mapp.Age_Days = obj.Age_Days;
                        mapp.Till_Date = obj.Till_Date;

                        mapp.CreatedDate = DateTime.Now;
                        mapp.UpdatedDate = DateTime.Now;
                        _context.Add(mapp);
                        int s = _context.SaveChanges();
                        if (s > 0)
                        {
                            obj.returnVal = "saved";
                        }
                        else
                        {
                            obj.returnVal = "savingFailed";
                        }
                    }
                }


                for (int i = 0; i < obj.student.Length; i++)
                {
                    var std = obj.student[i].AMST_Id;

                    var datareport = (from a in _context.AcademicYear
                                      from b in _context.admissionClass
                                      from c in _context.masterSection
                                      from d in _context.Adm_M_Student
                                      from e in _context.StudentAgeCalcDMO
                                      where (e.MI_Id == obj.MI_Id && a.ASMAY_Id == e.ASMAY_Id && b.ASMCL_Id == e.ASMCL_Id && c.ASMS_Id == e.ASMS_Id && d.AMST_Id == e.AMST_Id && e.ASMAY_Id == obj.ASMAY_Id && e.ASMCL_Id == obj.ASMCL_Id && e.ASMS_Id == obj.ASMS_Id && e.AMST_Id == std)
                                      select new StudentAgeCalcDTO
                                      {
                                          ASMAY_Year = a.ASMAY_Year,
                                          AMST_AdmNo = d.AMST_AdmNo,
                                          studentName = d.AMST_FirstName + (string.IsNullOrEmpty(d.AMST_MiddleName) ? "" : ' ' + d.AMST_MiddleName) + (string.IsNullOrEmpty(d.AMST_LastName) ? "" : ' ' + d.AMST_LastName),
                                          ASMCL_ClassName = b.ASMCL_ClassName,
                                          ASMC_SectionName = c.ASMC_SectionName,
                                          Age_Years = e.Age_Years,
                                          Age_Months = e.Age_Months,
                                          Age_Days = e.Age_Days
                                      }
                                       ).ToList();
                    if (datareport.Count > 0)
                    {
                        obj.datareport = datareport.ToArray();
                        obj.count = datareport.Count;
                    }
                    else
                    {
                        obj.count = 0;
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return obj;
        }

        public StudentAgeCalcDTO report(StudentAgeCalcDTO obj)
        {
            try
            {
                obj.datareport = (from a in _context.AcademicYear
                                  from b in _context.admissionClass
                                  from c in _context.masterSection
                                  from d in _context.Adm_M_Student
                                  from e in _context.StudentAgeCalcDMO
                                  where (e.MI_Id == obj.MI_Id && a.ASMAY_Id == e.ASMAY_Id && b.ASMCL_Id == e.ASMCL_Id && c.ASMS_Id == e.ASMS_Id && d.AMST_Id == e.AMST_Id && e.ASMAY_Id == obj.ASMAY_Id && e.ASMCL_Id == obj.ASMCL_Id && e.ASMS_Id == obj.ASMS_Id)
                                  select new StudentAgeCalcDTO
                                  {
                                      ASMAY_Year = a.ASMAY_Year,
                                      AMST_AdmNo = d.AMST_AdmNo,
                                      studentName = d.AMST_FirstName + (string.IsNullOrEmpty(d.AMST_MiddleName) ? "" : ' ' + d.AMST_MiddleName) + (string.IsNullOrEmpty(d.AMST_LastName) ? "" : ' ' + d.AMST_LastName),
                                      ASMCL_ClassName = b.ASMCL_ClassName,
                                      ASMC_SectionName = c.ASMC_SectionName,
                                      AMST_DOB = d.AMST_DOB,
                                      AMST_Date = d.AMST_Date,
                                      Till_Date = e.Till_Date,
                                      Age_Years = e.Age_Years,
                                      Age_Months = e.Age_Months,
                                      Age_Days = e.Age_Days
                                  }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }

        public StudentAgeCalcDTO Get_Class_House(StudentAgeCalcDTO dto)
        {
            try
            {
                dto.classList = (from a in _context.admissionyearstudent
                                 from b in _context.admissionClass
                                 where (a.ASMAY_Id == dto.ASMAY_Id && a.AMAY_ActiveFlag == 1 && a.ASMCL_Id == b.ASMCL_Id && b.ASMCL_ActiveFlag == true && b.MI_Id == dto.MI_Id)
                                 select b).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();

                dto.houseList = (from t in _context.SportMasterHouseDMO
                                 from b in _context.SportStudentHouseDivisionDMO
                                 where (t.MI_Id == b.MI_Id && t.SPCCMH_Id == b.SPCCMH_Id && t.MI_Id == dto.MI_Id && b.ASMAY_Id == dto.ASMAY_Id && t.SPCCMH_ActiveFlag == true && b.SPCCMH_ActiveFlag == true)
                                 select t
                                 ).Distinct().OrderBy(t => t.SPCCMH_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dto;
        }

    }
}
