using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vaps.Fee;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.admission;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Services
{
    public class EmployeeStudentDetailsImpl:Interfaces.EmployeeStudentDetailsInterface
    {
        public StudentAttendanceReportContext _db;
        public ExamContext _exm;
        public EmployeeStudentDetailsImpl(ExamContext exm, StudentAttendanceReportContext db)
        {
            _db = db;
            _exm = exm;

        }

        public EmployeeDashboardDTO getdata(EmployeeDashboardDTO dto)
        {
            try
            {
                //dto.HRME_Id = _exm.Staff_User_Login.Single(c => c.Id == dto.userid && c.MI_Id == dto.MI_Id).Emp_Code;

                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _db.academicYear.Where(t => t.MI_Id == dto.MI_Id && t.Is_Active == true).OrderBy(a => a.ASMAY_Order).ToList();
                dto.academicList = allyear.ToArray();

                var classlist = (from a in _db.Adm_SchAttLoginUserClass
                                 from b in _db.Adm_SchAttLoginUser
                                 from c in _db.admissionClass
                                 where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id
                                 && b.MI_Id == dto.MI_Id && b.ASMAY_Id == dto.ASMAY_Id
                                 && c.ASMCL_ActiveFlag == true)
                                 select new StudentAttendanceReportDTO
                                 {
                                     ASMCL_Id = c.ASMCL_Id,
                                     asmcL_ClassName = c.ASMCL_ClassName,
                                 }
                                 ).Distinct().ToList();
                dto.classlist = classlist.ToArray();


                dto.SectionList = (from a in _db.admissionyearstudent
                                   from b in _db.masterSection
                                   from c in classlist
                                   where (a.ASMAY_Id == dto.ASMAY_Id && a.AMAY_ActiveFlag == 1 && a.ASMS_Id == b.ASMS_Id && b.ASMC_ActiveFlag == 1 && b.MI_Id == dto.MI_Id && a.ASMCL_Id == c.ASMCL_Id)
                                   select b).Distinct().ToArray();

                dto.studentList = _db.admissionStduent.Where(t => t.MI_Id == dto.MI_Id && t.AMST_ActiveFlag == 1 && t.AMST_SOL == "S").Distinct().ToArray();


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


                //dto.studentList = _db.admissionStduent.Where(t => t.MI_Id == dto.MI_Id && t.AMST_ActiveFlag == 1 && t.AMST_SOL == "S").Distinct().ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;

        }
    }
}
