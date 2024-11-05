using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model;
using PortalHub.com.vaps.HOD.Interfaces;
using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.HOD.Services
{
    public class StudentHODImpl : StudentHODInterface
    {
        private static ConcurrentDictionary<string, ExamDTO> _login =
          new ConcurrentDictionary<string, ExamDTO>();
        private PortalContext _Examcontext;

        public DomainModelMsSqlServerContext _db;
        public StudentHODImpl(PortalContext Feecontext, DomainModelMsSqlServerContext db)
        {
            _Examcontext = Feecontext;
            _db = db;
        }
        public ExamDTO getloaddata(ExamDTO data)
        {
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _db.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_ActiveFlag == 1).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.studetiallist = list.ToArray();
                //data.studetiallist = (from d in _Examcontext.AcademicYearDMO
                //                      where (d.MI_Id == data.MI_Id && d.ASMAY_ActiveFlag == 1)
                //                      select new ExamDTO
                //                      {
                //                          ASMAY_Id = d.ASMAY_Id,
                //                          ASMAY_Year = d.ASMAY_Year
                //                      }
                //             ).Distinct().OrderBy(t=>t.ASMAY_Order).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ExamDTO get_classes(ExamDTO data)
        {
            try
            {
                var loginData = _db.Staff_User_Login.Where(d => d.Id == data.user_id).ToList();
                data.classlist = (from c in _Examcontext.School_M_Class
                                      //from e in _db.Masterclasscategory
                                      //from e in _Examcontext.Masterclasscategory
                                  from f in _Examcontext.IVRM_HOD_Class_DMO
                                      //from g in _Examcontext.IVRM_HOD_Staff_DMO
                                  from h in _Examcontext.HOD_DMO
                                  from z in _Examcontext.School_Adm_Y_StudentDMO
                                  from y in _Examcontext.AcademicYearDMO
                                  where (h.IHOD_Id == f.IHOD_Id && f.ASMCL_Id == c.ASMCL_Id && z.ASMAY_Id == y.ASMAY_Id && z.ASMCL_Id == c.ASMCL_Id && z.ASMAY_Id == data.ASMAY_Id && h.IHOD_ActiveFlag == true && h.MI_Id == data.MI_Id && h.HRME_Id == loginData.FirstOrDefault().Emp_Code)
                                  select new ExamDTO
                                  {
                                      ASMCL_Id = c.ASMCL_Id,
                                      ASMCL_ClassName = c.ASMCL_ClassName
                                  }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ExamDTO getsectiondata(ExamDTO data)
        {
            try
            {
                //data.sectionlist = (from d in _db.School_M_Section
                //                    from e in _db.Masterclasscategory
                //                    from f in _db.AdmSchoolMasterClassCatSec

                //                    where (d.MI_Id == data.MI_Id && d.ASMC_ActiveFlag == 1 && d.ASMS_Id == f.ASMS_Id && f.ASMCC_Id == e.ASMCC_Id && e.ASMAY_Id == data.ASMAY_Id && e.ASMCL_Id == data.ASMCL_Id)
                //                    select new ExamDTO
                //                    {
                //                        ASMS_Id = d.ASMS_Id,
                //                        ASMC_SectionName = d.ASMC_SectionName
                //                    }
                //             ).Distinct().ToArray();

                data.sectionlist = (from a in _db.School_M_Class
                                    from b in _db.School_Adm_Y_StudentDMO
                                    from c in _db.School_M_Section
                                    where (a.ASMCL_Id == b.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && a.ASMCL_ActiveFlag == true && b.AMAY_ActiveFlag == 1 && c.ASMC_ActiveFlag == 1)
                                    select new ExamDTO
                                    {
                                        ASMC_SectionName = c.ASMC_SectionName,
                                        ASMS_Id = c.ASMS_Id
                                    }).Distinct().OrderBy(t => t.ASMS_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public ExamDTO getstudentdata(ExamDTO data)
        {
            try
            {
                data.fillstudent = (from a in _db.Adm_M_Student
                                    from b in _db.School_Adm_Y_StudentDMO
                                    where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && a.AMST_SOL == "S")
                                    select new ExamDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        //AMST_FirstName = a.AMST_FirstName,
                                        AMST_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),
                                        AMST_MiddleName = a.AMST_MiddleName,
                                        AMST_LastName = a.AMST_LastName,
                                    }
                ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamDTO getexamdata(ExamDTO data)
        {
            try
            {
                data.examlist = (from a in _Examcontext.ExmStudentMarksProcessSubjectwiseDMO
                                 from b in _Examcontext.exammasterDMO
                                 where (a.EME_Id == b.EME_Id && a.MI_Id == data.MI_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && b.MI_Id == data.MI_Id)
                                 select new ExamDTO
                                 {
                                     EME_Id = b.EME_Id,
                                     EME_ExamName = b.EME_ExamName
                                 }
                     ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public ExamDTO getexamdetails(ExamDTO data)
        {
            try
            {

                data.examsubjdetails = (from a in _Examcontext.ExmStudentMarksProcessSubjectwiseDMO
                                        from b in _Examcontext.exammasterDMO
                                        from c in _Examcontext.AcademicYearDMO
                                        from d in _Examcontext.IVRM_Master_SubjectsDMO
                                        where (d.ISMS_Id == a.ISMS_Id && a.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.AMST_Id == data.AMST_Id && b.EME_Id == a.EME_Id && a.EME_Id == data.EME_Id)
                                        select new ExamDTO
                                        {
                                            EME_Id = a.EME_Id,
                                            ISMS_SubjectName = d.ISMS_SubjectName,
                                            ESTMPS_ObtainedGrade = a.ESTMPS_ObtainedGrade,
                                            ESTMPS_ObtainedMarks = a.ESTMPS_ObtainedMarks,
                                            ESTMPS_MaxMarks = a.ESTMPS_MaxMarks,
                                            ASMAY_Year = c.ASMAY_Year
                                        }
                            ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
