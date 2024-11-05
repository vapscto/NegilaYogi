using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model;
using DomainModel.Model.com.vaps.Exam;
using DomainModel.Model.com.vapstech.Portals.Employee;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.admission;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Services
{
    public class EmployeeStudentExamResultsImpl:Interfaces.EmployeeStudentExamResultsInterface 
    {
        private static ConcurrentDictionary<string, EmployeeDashboardDTO> _login =
       new ConcurrentDictionary<string, EmployeeDashboardDTO>();

        private readonly ExamContext _PCReportContext;
        private  PortalContext _PContext;

        ILogger<EmployeeStudentExamResultsImpl> _acdimpl;
        public StudentAttendanceReportContext _db;
        
        public EmployeeStudentExamResultsImpl(ExamContext PCReportContext, StudentAttendanceReportContext db, PortalContext PContext)
        {
            _db = db;
            _PCReportContext = PCReportContext;
            _PContext = PContext;
        }
        public EmployeeDashboardDTO getdata(EmployeeDashboardDTO dto)
        {
            try
            {
                //dto.HRME_Id = _exm.Staff_User_Login.Single(c => c.Id == dto.userid && c.MI_Id == dto.MI_Id).Emp_Code;

                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _db.academicYear.Where(t => t.MI_Id == dto.MI_Id && t.Is_Active == true).OrderBy(a => a.ASMAY_Order).ToList();
                dto.academicList = allyear.ToArray();

                var remarkslist = (from a in _PContext.EmployeeStudentExamResultDMO
                                   from b in _db.admissionClass
                                   from c in _db.masterSection
                                   from d in _db.admissionStduent
                                   from e in _PCReportContext.exammasterDMO
                                   where (a.ASMCL_Id == b.ASMCL_Id && a.ASMS_Id == c.ASMS_Id && a.AMST_Id == d.AMST_Id && a.EME_Id == e.EME_Id && a.ASMAY_Id ==dto.ASMAY_Id && a.EMER_ActiveFlag == true)
                                   select new EmployeeDashboardDTO
                                   {
                                      // EMER_Id = a.EMER_Id,
                                       Amst_Id = a.AMST_Id,
                                       ASMCL_Id = a.ASMCL_Id,
                                       ASMS_Id=a.ASMS_Id,
                                       ASMAY_Id = a.ASMAY_Id,
                                      
                                       studentName = d.AMST_FirstName + (string.IsNullOrEmpty(d.AMST_MiddleName) || d.AMST_MiddleName == "0" ? "" : ' ' + d.AMST_MiddleName) + (string.IsNullOrEmpty(d.AMST_LastName) || d.AMST_LastName == "0" ? "" : ' ' + d.AMST_LastName),
                                       ClassName =b.ASMCL_ClassName,
                                       SectionName=c.ASMC_SectionName,
                                       //exam_name =e.EME_ExamName,
                                       //TXTRemark =a.EMER_Remarks 
                                   }
                                 ).Distinct().ToList();
                dto.remarklist = remarkslist.ToArray();

                //var classlist = (from a in _db.Adm_SchAttLoginUserClass
                //                 from b in _db.Adm_SchAttLoginUser
                //                 from c in _db.admissionClass
                //                 where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id
                //                 && b.MI_Id == dto.MI_Id && b.ASMAY_Id == dto.ASMAY_Id
                //                 && c.ASMCL_ActiveFlag == true)
                //                 select new StudentAttendanceReportDTO
                //                 {
                //                     ASMCL_Id = c.ASMCL_Id,
                //                     asmcL_ClassName = c.ASMCL_ClassName,
                //                 }
                //                 ).Distinct().ToList();
                //dto.classlist = classlist.ToArray();


                //dto.SectionList = (from a in _db.admissionyearstudent
                //                   from b in _db.masterSection
                //                   from c in classlist
                //                   where (a.ASMAY_Id == dto.ASMAY_Id && a.AMAY_ActiveFlag == 1 && a.ASMS_Id == b.ASMS_Id && b.ASMC_ActiveFlag == 1 && b.MI_Id == dto.MI_Id && a.ASMCL_Id == c.ASMCL_Id)
                //                   select b).Distinct().ToArray();

                //dto.studentList = _db.admissionStduent.Where(t => t.MI_Id == dto.MI_Id && t.AMST_ActiveFlag == 1 && t.AMST_SOL == "S").Distinct().ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        public EmployeeDashboardDTO getremarkdetails(EmployeeDashboardDTO dto)
        {
            try
            {
                
                var remarkslist = (from a in _PContext.EmployeeStudentExamResultDMO
                                   from b in _PCReportContext.exammasterDMO
                                   where ( a.EME_Id == b.EME_Id && a.ASMAY_Id == dto.ASMAY_Id && a.ASMCL_Id == dto.ASMCL_Id && a.ASMS_Id == dto.ASMS_Id && a.AMST_Id==dto.Amst_Id && a.EMER_ActiveFlag == true)
                                   select new EmployeeDashboardDTO
                                   {
                                       //EMER_Id = a.EMER_Id,
                                       //EME_Id = a.EME_Id,
                                       exam_name = b.EME_ExamName,
                                       TXTRemark = a.EMER_Remarks
                                   }
                                 ).Distinct().ToList();
                dto.remarklistS = remarkslist.ToArray();
                
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        public EmployeeDashboardDTO get_class(EmployeeDashboardDTO dto)
        {
            try
            {
                dto.classlist = (from a in _db.admissionyearstudent
                                 from b in _db.admissionClass
                                 where (a.ASMAY_Id == dto.ASMAY_Id && a.AMAY_ActiveFlag == 1 && a.ASMCL_Id == b.ASMCL_Id && b.ASMCL_ActiveFlag == true && b.MI_Id == dto.MI_Id)
                                 select b).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        public EmployeeDashboardDTO get_section(EmployeeDashboardDTO dto)
        {
            try
            {
                dto.SectionList = (from a in _db.admissionyearstudent
                                   from b in _db.masterSection

                                   where (a.ASMAY_Id == dto.ASMAY_Id && a.AMAY_ActiveFlag == 1 && a.ASMS_Id == b.ASMS_Id && b.ASMC_ActiveFlag == 1 && b.MI_Id == dto.MI_Id && a.ASMCL_Id == dto.ASMCL_Id)
                                   select b).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        public EmployeeDashboardDTO get_student(EmployeeDashboardDTO dto)
        {
            try
            {
                dto.studentList = (from a in _db.admissionyearstudent
                                   from b in _db.admissionStduent

                                   where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == dto.ASMAY_Id && a.AMAY_ActiveFlag == 1 && a.ASMS_Id == dto.ASMS_Id && b.MI_Id == dto.MI_Id && a.ASMCL_Id == dto.ASMCL_Id && a.ASMS_Id == dto.ASMS_Id)
                                   select b).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
        public EmployeeDashboardDTO get_exam(EmployeeDashboardDTO dto)
        {
            try
            {
                var EQuery = _PCReportContext.ExmStudentMarksProcessDMO.Where(t => t.MI_Id == dto.MI_Id && t.ASMAY_Id == dto.ASMAY_Id && t.ASMCL_Id == dto.ASMCL_Id && t.ASMS_Id == dto.ASMS_Id && t.AMST_Id == dto.Amst_Id).Select(d => d.EME_Id).ToList();

                List<exammasterDMO> esmp = new List<exammasterDMO>();
                esmp = _PCReportContext.exammasterDMO.Where(t => t.MI_Id == dto.MI_Id && t.EME_ActiveFlag == true && EQuery.Contains(t.EME_Id)).ToList();
                dto.exmstdlist = esmp.ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }

        public EmployeeDashboardDTO saveRemark(EmployeeDashboardDTO dto)
        {
            EmployeeStudentExamResultDMO objpge = Mapper.Map<EmployeeStudentExamResultDMO>(dto);
            try
            {
                //if (objpge.EMER_Id > 0)
                //{
                var resultCount = _PContext.EmployeeStudentExamResultDMO.Where(t => t.MI_Id == objpge.MI_Id && t.ASMAY_Id == objpge.ASMAY_Id && t.ASMCL_Id== objpge.ASMCL_Id && t.ASMS_Id == objpge.ASMS_Id && t.EME_Id== objpge.EME_Id && t.AMST_Id== objpge.AMST_Id).Count();
                if (resultCount == 0)
                {
                    var emeid = Convert.ToInt32(dto.EME_Id);
                    objpge.MI_Id = dto.MI_Id;
                    objpge.ASMAY_Id = dto.ASMAY_Id;
                    objpge.ASMCL_Id = dto.ASMCL_Id;
                    objpge.ASMS_Id = dto.ASMS_Id;
                    objpge.EME_Id = emeid;
                    objpge.AMST_Id = dto.Amst_Id;
                    objpge.EMER_Remarks = dto.TXTRemark;
                    objpge.EMER_ActiveFlag = true;
                    objpge.CreatedDate = DateTime.Now;
                    objpge.UpdatedDate = DateTime.Now;

                    _PContext.Add(objpge);
                    var contactExists = _PContext.SaveChanges();
                    if (contactExists == 1)
                    {
                        dto.returnval = true;
                    }
                    else
                    {
                        dto.returnval = false;
                    }
                }else
                {
                    dto.returnduplicatestatus = "Duplicate";
                    dto.returnval = false;
                }
                //}


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }

        //public EmployeeDashboardDTO editdetails(EmployeeDashboardDTO dto)
        //{
           
        //    try
        //    {
        //        List<EmployeeStudentExamResultDMO> list = new List<EmployeeStudentExamResultDMO>();
        //        list = _PContext.EmployeeStudentExamResultDMO.Where(t => t.EME_Id == ).ToList();
        //        dto.editlist = list.ToArray();
        //    }
        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee);
        //    }
        //    return dto;
        //}
        public EmployeeDashboardDTO getdaily_data(EmployeeDashboardDTO dto)
        {
            try
            {
                dto.HRME_Id = _PCReportContext.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;
                
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }
    }
}
