using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using PreadmissionDTOs.com.vaps.Portals.Student;

namespace PortalHub.com.vaps.Student.Services
{
    public class StudentHallticketImpl : Interfaces.StudentHallticketInterface
    {
        public PortalContext _context;
        public StudentHallticketImpl(PortalContext context)
        {
            _context = context;
        }
        public StudentHallticketDTO GetLoadData(StudentHallticketDTO data)
        {
            try
            {
                data.getyearlist = _context.AcademicYearDMO.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.getcurrentyearlist = _context.AcademicYearDMO.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true
                && a.ASMAY_Id == data.ASMAY_Id).OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.getexamlist = (from a in _context.Exm_HallTicketDMO
                                    from b in _context.exammasterDMO
                                    where (a.EME_Id == b.EME_Id && a.EHT_PublishFlg == true && a.AMST_Id == data.AMST_Id && a.ASMAY_Id == data.ASMAY_Id)
                                    select b).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentHallticketDTO GetExamDetails(StudentHallticketDTO data)
        {
            try
            {
                data.getexamlist = (from a in _context.Exm_HallTicketDMO
                                    from b in _context.exammasterDMO
                                    where (a.EME_Id == b.EME_Id && a.EHT_PublishFlg == true && a.AMST_Id == data.AMST_Id && a.ASMAY_Id == data.ASMAY_Id)
                                    select b).Distinct().OrderBy(a => a.EME_ExamOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentHallticketDTO GetReport(StudentHallticketDTO data)
        {
            try
            {
                var getstudentdetails = (from a in _context.School_Adm_Y_StudentDMO
                                         from b in _context.Adm_M_Student
                                         where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == data.AMST_Id && a.AMAY_ActiveFlag == 1
                                         && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1)
                                         select new StudentHallticketDTO
                                         {
                                             ASMCL_Id = a.ASMCL_Id,
                                             ASMS_Id = a.ASMS_Id

                                         }).Distinct().ToList();

                if (getstudentdetails.Count > 0)
                {
                    data.ASMCL_Id = getstudentdetails.FirstOrDefault().ASMCL_Id;
                    data.ASMS_Id = getstudentdetails.FirstOrDefault().ASMS_Id;
                }


                data.datareport = (from a in _context.Exm_HallTicketDMO
                                   from b in _context.Adm_M_Student
                                   from c in _context.School_M_Class
                                   from d in _context.School_M_Section
                                   from g in _context.School_Adm_Y_StudentDMO
                                   from e in _context.AcademicYearDMO
                                   from f in _context.exammasterDMO
                                   where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                                   && a.ASMS_Id == data.ASMS_Id && a.EME_Id == data.EME_Id && a.AMST_Id == b.AMST_Id && a.ASMCL_Id == c.ASMCL_Id
                                   && a.ASMS_Id == d.ASMS_Id && a.ASMAY_Id == e.ASMAY_Id && a.EME_Id == f.EME_Id && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1
                                   && g.AMST_Id == b.AMST_Id && g.AMAY_ActiveFlag == 1 && g.ASMAY_Id == data.ASMAY_Id && g.ASMCL_Id == data.ASMCL_Id
                                   && g.ASMS_Id == data.ASMS_Id && g.AMST_Id == data.AMST_Id && a.EHT_PublishFlg == true)
                                   select new StudentHallticketDTO
                                   {
                                       AMST_Id = g.AMST_Id,
                                       AMST_FirstName = ((b.AMST_FirstName == null || b.AMST_FirstName == "" ? "" : " " + b.AMST_FirstName) + (b.AMST_MiddleName == null || b.AMST_MiddleName == "" || b.AMST_MiddleName == "0" ? "" : " " + b.AMST_MiddleName) + (b.AMST_LastName == null || b.AMST_LastName == "" || b.AMST_LastName == "0" ? "" : " " + b.AMST_LastName)).Trim(),
                                       AMST_AdmNo = b.AMST_AdmNo,
                                       AMAY_Rollno = g.AMAY_RollNo,
                                       ASMCL_ClassName = c.ASMCL_ClassName,
                                       ASMC_SectionName = d.ASMC_SectionName,
                                       EHT_HallTicketNo = a.EHT_HallTicketNo,
                                       AMST_Photoname = b.AMST_Photoname,
                                       AMST_FatherName = ((b.AMST_FatherName == null || b.AMST_FatherName == "" ? "" : " " + b.AMST_FatherName)
                                       + (b.AMST_FatherSurname == null || b.AMST_FatherSurname == "" || b.AMST_FatherSurname == "0" ? "" : " " + b.AMST_FatherSurname)).Trim(),
                                   }).Distinct().OrderBy(a => a.AMST_FirstName).ToArray();


                data.subarray = (from a in _context.Exm_TimeTable_SubjectsDMO
                                 from b in _context.Exm_TimeTableDMO
                                 from c in _context.IVRM_Master_SubjectsDMO
                                 from d in _context.exammasterDMO
                                 from e in _context.Exm_TT_M_SessionDMO
                                 from f in _context.StudentMappingDMO
                                 where (a.EXTT_Id == b.EXTT_Id && c.ISMS_Id == a.ISMS_Id && f.ISMS_Id == c.ISMS_Id && f.EMG_Id == b.EMG_Id
                                 && b.EME_Id == d.EME_Id && e.ETTS_Id == a.ETTS_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                                 && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && b.EME_Id == data.EME_Id && f.ASMAY_Id == data.ASMAY_Id
                                 && f.ASMCL_Id == data.ASMCL_Id && f.ASMS_Id == data.ASMS_Id && f.MI_Id == data.MI_Id && f.ESTSU_ActiveFlg == true
                                 && f.AMST_Id == data.AMST_Id)
                                 select new StudentHallticketDTO
                                 {
                                     AMST_Id = f.AMST_Id,
                                     ISMS_SubjectName = c.ISMS_SubjectName,
                                     EXTTS_Date = a.EXTTS_Date,
                                     EME_ExamName = d.EME_ExamName,
                                     ETTS_SessionName = e.ETTS_SessionName,
                                     subjectorder = c.ISMS_OrderFlag,
                                     ETTS_StartTime = e.ETTS_StartTime,
                                     ETTS_EndTime = e.ETTS_EndTime
                                 }).Distinct().OrderBy(a => a.EXTTS_Date).ThenBy(a => a.subjectorder).ToArray();

                data.studarray = (from a in _context.Adm_M_Student
                                  from b in _context.Exm_HallTicketDMO
                                  from c in _context.School_M_Class
                                  from d in _context.School_M_Section
                                  from g in _context.School_Adm_Y_StudentDMO
                                  where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id
                                  && b.EME_Id == data.EME_Id && a.AMST_Id == b.AMST_Id && c.ASMCL_Id == a.ASMCL_Id && d.ASMS_Id == b.ASMS_Id
                                  && g.AMST_Id == a.AMST_Id && g.AMAY_ActiveFlag == 1 && g.ASMAY_Id == data.ASMAY_Id && g.ASMCL_Id == data.ASMCL_Id
                                  && g.ASMS_Id == data.ASMS_Id && g.AMST_Id == data.AMST_Id && b.EHT_PublishFlg == true)
                                  select new StudentHallticketDTO
                                  {
                                      AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "" ? "" : " " + a.AMST_FirstName) + (a.AMST_MiddleName == null || a.AMST_MiddleName == "" || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) + (a.AMST_LastName == null || a.AMST_LastName == "" || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim(),
                                      AMST_Photoname = a.AMST_Photoname,
                                      EHT_HallTicketNo = b.EHT_HallTicketNo,
                                      ASMCL_ClassName = c.ASMCL_ClassName,
                                      ASMC_SectionName = d.ASMC_SectionName
                                  }).Distinct().OrderBy(a => a.AMST_FirstName).ToArray();


                data.institute = _context.Institute.Where(t => t.MI_Id == data.MI_Id).ToArray();

                data.configuraion = _context.GeneralConfigDMO.Where(t => t.MI_Id == data.MI_Id).ToArray();

                string accountname = "";
                string accesskey = "";

                ReadTemplateFromAzure obj = new ReadTemplateFromAzure();
                var datatstu = _context.IVRM_Storage_path_Details.ToList();
                if (datatstu.Count() > 0)
                {
                    accountname = datatstu.FirstOrDefault().IVRM_SD_Access_Name;
                    accesskey = datatstu.FirstOrDefault().IVRM_SD_Access_Key;
                }

                string html = obj.getHtmlContentFromAzure(accountname, accesskey, "files/" + data.MI_Id, "HallticketReport.html", 0);
                data.htmldata = html;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
