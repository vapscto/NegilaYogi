using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vaps.Exam;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Exam;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Services
{
    public class EmployeeStudentReportCardImpl : Interfaces.EmployeeStudentReportCardInterface
    {
        private static ConcurrentDictionary<string, EmployeeDashboardDTO> _login =
        new ConcurrentDictionary<string, EmployeeDashboardDTO>();

        private readonly ExamContext _PCReportContext;
        public StudentAttendanceReportContext _db;
        public FeeGroupContext _fees;
        ILogger<EmployeeStudentReportCardImpl> _acdimpl;
        public EmployeeStudentReportCardImpl(ExamContext cpContext, StudentAttendanceReportContext db, FeeGroupContext fee)
        {
            _PCReportContext = cpContext;
            _db = db;
            _fees = fee;
        }


        public EmployeeDashboardDTO Getdetails(EmployeeDashboardDTO data)//int IVRMM_Id
        {
            EmployeeDashboardDTO getdata = new EmployeeDashboardDTO();
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _PCReportContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_ActiveFlag == 1).ToList();
                getdata.yearlist = list.ToArray();


                //List<School_M_Section> seclist = new List<School_M_Section>();
                //seclist = _PCReportContext.School_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).ToList();
                //getdata.SectionList = seclist.ToArray();

                //List<AdmissionClass> admlist = new List<AdmissionClass>();
                //admlist = _PCReportContext.AdmissionClass.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                //getdata.classlist = admlist.ToArray();


                //List<exammasterDMO> esmp = new List<exammasterDMO>();
                //esmp = _PCReportContext.exammasterDMO.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true).ToList();
                //getdata.exmstdlist = esmp.ToArray();

                //getdata.studentList = _db.admissionStduent.Where(t => t.MI_Id == data.MI_Id && t.AMST_ActiveFlag == 1 && t.AMST_SOL == "S").Distinct().ToArray();


            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return getdata;

        }

        public EmployeeDashboardDTO showdetails(EmployeeDashboardDTO data)
        {


            try
            {
                data.clstchname = (from a in _PCReportContext.ClassTeacherMappingDMO
                                   from b in _PCReportContext.HR_Master_Employee_DMO
                                   where (a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id)
                                   select new BaldwinAllReportDTO
                                   {
                                       HRME_Id = a.HRME_Id,
                                       HRME_EmployeeFirstName = b.HRME_EmployeeFirstName + (string.IsNullOrEmpty(b.HRME_EmployeeMiddleName) || b.HRME_EmployeeMiddleName == "0" ? "" : ' ' + b.HRME_EmployeeMiddleName) + (string.IsNullOrEmpty(b.HRME_EmployeeLastName) || b.HRME_EmployeeLastName == "0" ? "" : ' ' + b.HRME_EmployeeLastName),
                                   }).Distinct().ToArray();

                data.savelist = (from a in _PCReportContext.ExmStudentMarksProcessSubjectwiseDMO
                                 from b in _PCReportContext.AdmissionClass
                                 from c in _PCReportContext.exammasterDMO
                                 from d in _PCReportContext.IVRM_School_Master_SubjectsDMO
                                 from e in _PCReportContext.School_M_Section
                                 from f in _PCReportContext.Adm_M_Student
                                     //    from g in _PCReportContext.ExmStudentMarksProcessDMO
                                 from h in _PCReportContext.School_Adm_Y_Student
                                 where (a.ASMCL_Id == b.ASMCL_Id && a.ASMCL_Id == data.ASMCL_Id && a.EME_Id == c.EME_Id && a.ISMS_Id == d.ISMS_Id && a.ASMS_Id == e.ASMS_Id && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == f.AMST_Id && a.MI_Id == data.MI_Id && a.EME_Id == data.EME_Id && h.ASMAY_Id == data.ASMAY_Id && h.ASMCL_Id == data.ASMCL_Id && h.ASMS_Id == data.ASMS_Id && h.AMST_Id == a.AMST_Id && a.AMST_Id == data.Amst_Id)
                                 select new ProgressCardReportDTO
                                 {
                                     ESTMPS_ObtainedMarks = a.ESTMPS_ObtainedMarks,
                                     ESTMPS_ObtainedGrade = a.ESTMPS_ObtainedGrade,
                                     ESTMPS_PassFailFlg = a.ESTMPS_PassFailFlg,
                                     EME_ExamName = c.EME_ExamName,
                                     ASMCL_ClassName = b.ASMCL_ClassName,
                                     ASMC_SectionName = e.ASMC_SectionName,
                                     AMST_Id = f.AMST_Id,
                                     //AMST_FirstName = ((f.AMST_FirstName == null ? " " : f.AMST_FirstName) + (f.AMST_MiddleName == null ? " " : f.AMST_MiddleName) + (f.AMST_LastName == null ? " " : f.AMST_LastName)).Trim(),
                                     AMST_FirstName = f.AMST_FirstName + (string.IsNullOrEmpty(f.AMST_MiddleName) || f.AMST_MiddleName == "0" ? "" : ' ' + f.AMST_MiddleName) + (string.IsNullOrEmpty(f.AMST_LastName) || f.AMST_LastName == "0" ? "" : ' ' + f.AMST_LastName),
                                     AMST_DOB = f.AMST_DOB,
                                     AMAY_RollNo = h.AMAY_RollNo,
                                     AMST_AdmNo = f.AMST_AdmNo,
                                     ISMS_Id = d.ISMS_Id,
                                     ISMS_SubjectName = d.ISMS_SubjectName,
                                     ESTMPS_MaxMarks = a.ESTMPS_MaxMarks,
                                     //  ESTMP_Percentage = g.ESTMP_Percentage,
                                 }).Distinct().ToArray();

                data.savelisttot = _PCReportContext.ExmStudentMarksProcessDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.MI_Id == data.MI_Id && t.ASMS_Id == data.ASMS_Id && t.EME_Id == data.EME_Id && t.AMST_Id == data.Amst_Id).Distinct().ToArray();


                data.subjlist = (from a in _PCReportContext.Exm_Category_ClassDMO
                                 from b in _PCReportContext.Exm_Yearly_CategoryDMO
                                 from c in _PCReportContext.Exm_Yearly_Category_ExamsDMO
                                 from d in _PCReportContext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                 from e in _PCReportContext.IVRM_School_Master_SubjectsDMO
                                 where (a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EMCA_Id == b.EMCA_Id && b.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && c.EYC_Id == b.EYC_Id && c.EME_Id == data.EME_Id && d.EYCE_Id == c.EYCE_Id && d.EYCES_ActiveFlg == true && e.MI_Id == data.MI_Id && e.ISMS_Id == d.ISMS_Id)
                                 select new ProgressCardReportDTO
                                 {
                                     ISMS_Id = d.ISMS_Id,
                                     ISMS_SubjectName = e.ISMS_SubjectName,
                                     ISMS_SubjectCode = e.ISMS_SubjectCode,
                                     EYCES_AplResultFlg = d.EYCES_AplResultFlg,
                                     EYCES_MaxMarks = d.EYCES_MaxMarks,
                                     EYCES_MinMarks = d.EYCES_MinMarks,
                                     EMGR_Id = d.EMGR_Id,


                                 }
                               ).Distinct().ToArray();
                List<int> grade = new List<int>();
                foreach (ProgressCardReportDTO x in data.subjlist)
                {
                    grade.Add(x.EMGR_Id);
                }

                data.grade_details = (from a in _PCReportContext.Exm_Master_GradeDMO
                                      from b in _PCReportContext.Exm_Master_Grade_DetailsDMO
                                      where (a.MI_Id == data.MI_Id && grade.Contains(a.EMGR_Id) && a.EMGR_Id == b.EMGR_Id)
                                      select b
                                     ).Distinct().ToArray();



            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }

            return data;
        }

        public EmployeeDashboardDTO get_class(EmployeeDashboardDTO dto)
        {
            try
            {
                dto.HRME_Id = _PCReportContext.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;

                //var EQuery = (from a in _PCReportContext.HR_Master_Employee_DMO
                //              from b in _PCReportContext.Staff_User_Login
                //              from c in _PCReportContext.Exm_Login_PrivilegeDMO
                //              from d in _PCReportContext.Exm_Login_Privilege_SubjectsDMO
                //              where (a.HRME_Id == b.Emp_Code && b.IVRMSTAUL_Id == c.Login_Id && c.ELP_Id == d.ELP_Id && a.HRME_Id == dto.HRME_Id && c.MI_Id == dto.MI_Id)
                //              select d.ASMCL_Id).Distinct().ToList();


                dto.classlist = (from a in _db.Adm_SchAttLoginUserClass
                                 from b in _db.Adm_SchAttLoginUser
                                 from c in _db.admissionClass
                                 where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id
                                 && b.MI_Id == dto.MI_Id && b.ASMAY_Id == dto.ASMAY_Id
                                 && b.HRME_Id == dto.HRME_Id
                                 && c.ASMCL_ActiveFlag == true)
                                 select new EmployeeDashboardDTO
                                 {
                                     ASMCL_Id = c.ASMCL_Id,
                                     ASMCL_ClassName = c.ASMCL_ClassName,
                                     //ASMS_Id = a.ASMS_Id,
                                 }).Distinct().ToArray();

                //dto.classlist = (from a in _db.admissionyearstudent
                //                 from b in _db.admissionClass
                //                 where (a.ASMAY_Id == dto.ASMAY_Id && a.AMAY_ActiveFlag == 1 && a.ASMCL_Id == b.ASMCL_Id && b.ASMCL_ActiveFlag == true && b.MI_Id == dto.MI_Id && EQuery.Contains(b.ASMCL_Id))
                //                 select b).Distinct().ToArray();

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
                dto.HRME_Id = _PCReportContext.Staff_User_Login.Single(c => c.Id == dto.UserId && c.MI_Id == dto.MI_Id).Emp_Code;

                //var EQuery1 = (from a in _PCReportContext.HR_Master_Employee_DMO
                //              from b in _PCReportContext.Staff_User_Login
                //              from c in _PCReportContext.Exm_Login_PrivilegeDMO
                //              from d in _PCReportContext.Exm_Login_Privilege_SubjectsDMO
                //              where (a.HRME_Id == b.Emp_Code && b.IVRMSTAUL_Id == c.Login_Id && c.ELP_Id == d.ELP_Id && a.HRME_Id == dto.HRME_Id && c.MI_Id == dto.MI_Id && d.ASMCL_Id ==dto.ASMCL_Id)
                //              select d.ASMS_Id).Distinct().ToList();

                //dto.SectionList = (from a in _db.admissionyearstudent
                //                   from b in _db.masterSection

                //                   where (a.ASMAY_Id == dto.ASMAY_Id && a.AMAY_ActiveFlag == 1 && a.ASMS_Id == b.ASMS_Id && b.ASMC_ActiveFlag == 1 && b.MI_Id == dto.MI_Id && a.ASMCL_Id == dto.ASMCL_Id && EQuery1.Contains(b.ASMS_Id))
                //                   select b).Distinct().ToArray();

                dto.SectionList = (from a in _db.Adm_SchAttLoginUserClass
                                   from b in _db.Adm_SchAttLoginUser
                                   from c in _db.admissionClass
                                   from d in _db.masterSection
                                   where (a.ASALU_Id == b.ASALU_Id && c.ASMCL_Id == a.ASMCL_Id && a.ASMS_Id == d.ASMS_Id
                                 && b.MI_Id == dto.MI_Id && b.ASMAY_Id == dto.ASMAY_Id
                                 && b.HRME_Id == dto.HRME_Id
                                 && c.ASMCL_ActiveFlag == true && d.ASMC_ActiveFlag == 1)
                                   select new EmployeeDashboardDTO
                                   {
                                       ASMS_Id = a.ASMS_Id,
                                       ASMC_SectionName = d.ASMC_SectionName,
                                   }).Distinct().ToArray();

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
                                   select new EmployeeDashboardDTO
                                   {
                                       Amst_Id = b.AMST_Id,
                                       AMST_FirstName = b.AMST_FirstName,
                                       AMST_MiddleName = b.AMST_MiddleName,
                                       AMST_LastName = b.AMST_LastName,
                                       studentnameorder = ((b.AMST_FirstName == null || b.AMST_FirstName == "" ? "" : " " + b.AMST_FirstName) +
                                       (b.AMST_MiddleName == null || b.AMST_MiddleName == "" || b.AMST_MiddleName == "0" ? "" : " " + b.AMST_MiddleName) +
                                       (b.AMST_LastName == null || b.AMST_LastName == "" || b.AMST_LastName == "0" ? "" : " " + b.AMST_LastName)).Trim(),
                                   }).Distinct().OrderBy(a => a.studentnameorder).ToArray();


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

      

    }
}
