using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class MarksReportImpl : Interfaces.MarksReportInterface
    {
        //public DomainModelMsSqlServerContext _Context;

        public MarksReportContext _MarksReportContext;
        public MarksReportImpl(MarksReportContext DomainModelContext)
        {
            _MarksReportContext = DomainModelContext;
        }
        public MarksReportDTO getdetails(MarksReportDTO data)
        {
            try
            {




            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public MarksReportDTO schedulelist(MarksReportDTO data)
        {
            try
            {
                var Acdemic_preadmission = _MarksReportContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.mid).Select(d => d.ASMAY_Id).FirstOrDefault();
                data.asmay_id = Acdemic_preadmission;
                List<CasteCategory> allcc = new List<CasteCategory>();
                allcc = _MarksReportContext.castecategory.ToList();
                data.admissioncatdrpall = allcc.ToArray();

                if (data.oralwrittenscheduleflag == "oral")
                {
                    List<OralTestScheduleDMO> oral = new List<OralTestScheduleDMO>();
                    data.writentestlist = (from a in _MarksReportContext.oraltest
                                           where (a.MI_Id == data.mid && a.ASMAY_Id == data.asmay_id)
                                           select new MarksReportDTO
                                           {
                                               disid = a.PAOTS_Id,
                                               disname = a.PAOTS_ScheduleName
                                           }).ToArray();

                    List<School_M_Class> classname = new List<School_M_Class>();
                    classname = _MarksReportContext.admissioncls.Where(t => t.MI_Id == data.mid && t.ASMCL_ActiveFlag == true).ToList();
                    data.fillclass = classname.ToArray();
                }
                else
                {
                    List<WrittenTestScheduleDMO> written = new List<WrittenTestScheduleDMO>();
                    data.writentestlist = (from a in _MarksReportContext.writentest
                                           where (a.MI_Id == data.mid && a.ASMAY_Id == data.asmay_id)
                                           select new MarksReportDTO
                                           {
                                               disid = a.PAWTS_Id,
                                               disname = a.PAWTS_ScheduleName
                                           }).ToArray();
                    List<IVRM_Master_SubjectsDMO> sublist = new List<IVRM_Master_SubjectsDMO>();
                    sublist = _MarksReportContext.allSubject.Where(t => t.MI_Id == data.mid && t.ISMS_ActiveFlag == 1 && t.ISMS_PreadmFlag == 1).ToList();
                    data.fillsub = sublist.ToArray();

                    List<School_M_Class> classname = new List<School_M_Class>();
                    classname = _MarksReportContext.admissioncls.Where(t => t.MI_Id == data.mid && t.ASMCL_ActiveFlag == true).ToList();
                    data.fillclass = classname.ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public MarksReportDTO Getreportdetails(MarksReportDTO data)
        {
            try
            {
                var Acdemic_preadmission = _MarksReportContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.mid).Select(d => d.ASMAY_Id).FirstOrDefault();

                data.yearid = Convert.ToInt32(Acdemic_preadmission);

                if (data.flagows == "oral")
                {
                    data.fillhead = (from a in _MarksReportContext.MasterConfiguration
                                     where (a.MI_Id == data.mid && a.ASMAY_Id == data.yearid)
                                     select new MarksReportDTO
                                     {
                                         hid = Convert.ToInt64(a.ISPAC_OralTestBy),
                                         //hhead = a.PAMSU_SubjectName,
                                         hmaxmarks = a.ISPAC_OralByMax_Marks
                                     }).OrderByDescending(t => t.asmay_id).ToArray();
                    if (data.schids == 0)
                    {
                        data.allreports = (from a in _MarksReportContext.OralTestStudentWiseMarksDMO
                                           from b in _MarksReportContext.StudentDetailsDMO
                                           from c in _MarksReportContext.OralTestScheduleStudentInsertDMO
                                           from d in _MarksReportContext.admissioncls
                                           from e in _MarksReportContext.oraltest
                                           where (a.PASR_Id == b.PASR_Id && b.PASR_Id == c.PASR_Id && a.PASR_Id == c.PASR_Id && e.PAOTS_Id == c.PAOTS_Id && b.MI_Id == data.mid && b.ASMCL_Id == d.ASMCL_Id && e.ASMAY_Id == data.yearid)
                                           select new MarksReportDTO
                                           {
                                               PASR_Id = b.PASR_Id,
                                               PASR_FirstName = b.PASR_FirstName,
                                               PASR_MiddleName = b.PASR_MiddleName,
                                               PASR_LastName = b.PASR_LastName,
                                               // name = b.PASR_FirstName + " " + b.PASR_MiddleName + " " + b.PASR_LastName,
                                               regno = b.PASR_RegistrationNo,
                                               ISMS_Id = a.PAOTM_Id,
                                               PASWMS_MarksScored = a.PAOTMS_Marks,
                                               classname = d.ASMCL_ClassName

                                           }).OrderBy(t => t.PASR_Id).ToArray();
                    }
                    else
                    {
                        data.allreports = (from a in _MarksReportContext.OralTestStudentWiseMarksDMO
                                           from b in _MarksReportContext.StudentDetailsDMO
                                           from c in _MarksReportContext.OralTestScheduleStudentInsertDMO
                                           from d in _MarksReportContext.admissioncls
                                           where (a.PASR_Id == b.PASR_Id && b.PASR_Id == c.PASR_Id && a.PASR_Id == c.PASR_Id && b.MI_Id == data.mid && c.PAOTS_Id == data.schids && b.ASMCL_Id == d.ASMCL_Id)
                                           select new MarksReportDTO
                                           {
                                               PASR_Id = b.PASR_Id,
                                               PASR_FirstName = b.PASR_FirstName,
                                               PASR_MiddleName = b.PASR_MiddleName,
                                               PASR_LastName = b.PASR_LastName,
                                               // name = b.PASR_FirstName + " " + b.PASR_MiddleName + " " + b.PASR_LastName,
                                               regno = b.PASR_RegistrationNo,
                                               ISMS_Id = a.PAOTM_Id,
                                               PASWMS_MarksScored = a.PAOTMS_Marks,
                                               classname = d.ASMCL_ClassName
                                           }).OrderBy(t => t.PASR_Id).ToArray();

                    }
                }
                else
                {
                    data.fillhead = (from a in _MarksReportContext.allSubject
                                     where (a.MI_Id == data.mid && a.ISMS_PreadmFlag == 1 && a.ISMS_ActiveFlag == 1)
                                     select new MarksReportDTO
                                     {
                                         hid = a.ISMS_Id,
                                         hhead = a.ISMS_SubjectName,
                                         hmaxmarks = a.ISMS_Max_Marks
                                     }).ToArray();
                    if (data.schids == 0)
                    {
                        if (data.ISMS_Id == 0)
                        {
                            data.allreports = (from a in _MarksReportContext.WrittenTestStudentSubjectWiseMarksDMO
                                               from b in _MarksReportContext.StudentDetailsDMO
                                               from c in _MarksReportContext.WrittenTestScheduleStudentInsertDMO
                                               from d in _MarksReportContext.WIrttenTestSubjectWiseMarksDMO
                                               from e in _MarksReportContext.admissioncls
                                               from f in _MarksReportContext.writentest
                                               where (a.PASWM_Id == d.PASWM_Id && a.PASR_Id == b.PASR_Id && c.PASR_Id == b.PASR_Id && f.PAWTS_Id == c.PAWTS_Id && b.MI_Id == data.mid && b.ASMCL_Id == e.ASMCL_Id && f.ASMAY_Id == data.yearid)
                                               select new MarksReportDTO
                                               {
                                                   ISMS_Id = d.ISMS_ID,
                                                   PASR_Id = b.PASR_Id,
                                                   PASR_FirstName = b.PASR_FirstName,
                                                   PASR_MiddleName = b.PASR_MiddleName,
                                                   PASR_LastName = b.PASR_LastName,
                                                   // name = b.PASR_FirstName + " " + b.PASR_MiddleName + " " + b.PASR_LastName,
                                                   regno = b.PASR_RegistrationNo,
                                                   PASWMS_MarksScored = a.PASWMS_MarksScored,
                                                   classname = e.ASMCL_ClassName
                                               }).OrderBy(t => t.PASR_Id).ToArray();
                        }
                        else
                        {

                            data.allreports = (from a in _MarksReportContext.WrittenTestStudentSubjectWiseMarksDMO
                                               from b in _MarksReportContext.StudentDetailsDMO
                                               from c in _MarksReportContext.WrittenTestScheduleStudentInsertDMO
                                               from d in _MarksReportContext.WIrttenTestSubjectWiseMarksDMO
                                               from e in _MarksReportContext.admissioncls
                                               from f in _MarksReportContext.writentest
                                               where (a.PASWM_Id == d.PASWM_Id && a.PASR_Id == b.PASR_Id && c.PASR_Id == b.PASR_Id && b.MI_Id == data.mid && d.ISMS_ID == data.ISMS_Id && b.ASMCL_Id == e.ASMCL_Id && f.PAWTS_Id == c.PAWTS_Id && f.ASMAY_Id == data.yearid)
                                               select new MarksReportDTO
                                               {
                                                   ISMS_Id = d.ISMS_ID,
                                                   PASR_Id = b.PASR_Id,
                                                   PASR_FirstName = b.PASR_FirstName,
                                                   PASR_MiddleName = b.PASR_MiddleName,
                                                   PASR_LastName = b.PASR_LastName,
                                                   // name = b.PASR_FirstName + " " + b.PASR_MiddleName + " " + b.PASR_LastName,
                                                   regno = b.PASR_RegistrationNo,
                                                   PASWMS_MarksScored = a.PASWMS_MarksScored,
                                                   classname = e.ASMCL_ClassName
                                               }).OrderBy(t => t.PASR_Id).ToArray();
                        }

                    }
                    else
                    {
                        if (data.ISMS_Id == 0)
                        {

                            //      Preadmission_Subjectwise_Written_Marks_Students(WrittenTestStudentSubjectWiseMarksDMO)INNER JOIN
                            //Preadmission_Subjectwise_Written_Marks ON
                            //Preadmission_Subjectwise_Written_Marks_Students.PASWM_Id = Preadmission_Subjectwise_Written_Marks.PASWM_Id INNER JOIN
                            //Preadmission_School_Registration(StudentDetailsDMO) ON Preadmission_Subjectwise_Written_Marks_Students.PASR_Id = Preadmission_School_Registration.PASR_Id INNER JOIN
                            //Preadmission_WrittenTest_Schedule_Student(WrittenTestScheduleStudentInsertDMO) ON Preadmission_School_Registration.PASR_Id = Preadmission_WrittenTest_Schedule_Student.PASR_Id


                            data.allreports = (from a in _MarksReportContext.WrittenTestStudentSubjectWiseMarksDMO
                                               from b in _MarksReportContext.StudentDetailsDMO
                                               from c in _MarksReportContext.WrittenTestScheduleStudentInsertDMO
                                               from d in _MarksReportContext.WIrttenTestSubjectWiseMarksDMO
                                               from e in _MarksReportContext.admissioncls
                                               where (a.PASWM_Id == d.PASWM_Id && a.PASR_Id == b.PASR_Id && c.PASR_Id == b.PASR_Id && b.MI_Id == data.mid && c.PAWTS_Id == data.schids && b.ASMCL_Id == e.ASMCL_Id)
                                               select new MarksReportDTO
                                               {
                                                   ISMS_Id = d.ISMS_ID,
                                                   PASR_Id = b.PASR_Id,
                                                   PASR_FirstName = b.PASR_FirstName,
                                                   PASR_MiddleName = b.PASR_MiddleName,
                                                   PASR_LastName = b.PASR_LastName,
                                                   regno = b.PASR_RegistrationNo,
                                                   PASWMS_MarksScored = a.PASWMS_MarksScored,
                                                   classname = e.ASMCL_ClassName
                                               }).OrderBy(t => t.PASR_Id).OrderBy(t => t.PASWMS_Id).ToArray();
                        }
                        else
                        {
                            data.allreports = (from a in _MarksReportContext.WrittenTestStudentSubjectWiseMarksDMO
                                               from b in _MarksReportContext.StudentDetailsDMO
                                               from c in _MarksReportContext.WrittenTestScheduleStudentInsertDMO
                                               from d in _MarksReportContext.WIrttenTestSubjectWiseMarksDMO
                                               from e in _MarksReportContext.admissioncls
                                               where (a.PASWM_Id == d.PASWM_Id && a.PASR_Id == b.PASR_Id && c.PASR_Id == b.PASR_Id && b.MI_Id == data.mid && c.PAWTS_Id == data.schids && d.ISMS_ID == data.ISMS_Id && b.ASMCL_Id == e.ASMCL_Id)
                                               select new MarksReportDTO
                                               {
                                                   ISMS_Id = d.ISMS_ID,
                                                   PASR_Id = b.PASR_Id,
                                                   PASR_FirstName = b.PASR_FirstName,
                                                   PASR_MiddleName = b.PASR_MiddleName,
                                                   PASR_LastName = b.PASR_LastName,
                                                   regno = b.PASR_RegistrationNo,
                                                   PASWMS_MarksScored = a.PASWMS_MarksScored,
                                                   classname = e.ASMCL_ClassName
                                               }).OrderBy(t => t.PASR_Id).ToArray();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public MarksReportDTO Getreportdetailssrkvs(MarksReportDTO data)
        {
            try
            {
                var Acdemic_preadmission = _MarksReportContext.AcademicYear.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.mid).Select(d => d.ASMAY_Id).FirstOrDefault();
                List<MarksReportDTO> studentlist = new List<MarksReportDTO>();
                data.yearid = Convert.ToInt32(Acdemic_preadmission);
                if (data.flagows != "oral")
                {
                    data.fillhead = (from a in _MarksReportContext.allSubject
                                     where (a.MI_Id == data.mid && a.ISMS_PreadmFlag == 1 && a.ISMS_ActiveFlag == 1)
                                     select new MarksReportDTO
                                     {
                                         hid = a.ISMS_Id,
                                         hhead = a.ISMS_SubjectName,
                                         hmaxmarks = a.ISMS_Max_Marks
                                     }).ToArray();

                    if (data.schids == 0)
                    {
                        if (data.ISMS_Id == 0)
                        {
                            if (data.ordertype == "Name" || data.ordertype == "Rank")
                            {
                                if (data.CasteCategory_Id == 0)
                                {
                                    data.allreports = (from a in _MarksReportContext.WrittenTestStudentSubjectWiseMarksDMO
                                                       from b in _MarksReportContext.StudentDetailsDMO
                                                       from c in _MarksReportContext.WrittenTestScheduleStudentInsertDMO
                                                       from d in _MarksReportContext.WIrttenTestSubjectWiseMarksDMO
                                                       from e in _MarksReportContext.admissioncls
                                                       from f in _MarksReportContext.writentest
                                                       from g in _MarksReportContext.castecategory
                                                       where (a.PASWM_Id == d.PASWM_Id && a.PASR_Id == b.PASR_Id && c.PASR_Id == b.PASR_Id && f.PAWTS_Id == c.PAWTS_Id && b.CasteCategory_Id == g.IMCC_Id && b.MI_Id == data.mid && b.ASMCL_Id == e.ASMCL_Id && f.ASMAY_Id == data.yearid)
                                                       select new MarksReportDTO
                                                       {
                                                           ISMS_Id = d.ISMS_ID,
                                                           PASR_Id = b.PASR_Id,
                                                           PASR_FirstName = b.PASR_FirstName,
                                                           PASR_MiddleName = b.PASR_MiddleName,
                                                           PASR_LastName = b.PASR_LastName,
                                                           name = ((b.PASR_FirstName == null || b.PASR_FirstName == "0" ? "" : b.PASR_FirstName) + " " + (b.PASR_MiddleName == null || b.PASR_MiddleName == "0" ? "" : b.PASR_MiddleName) + " " + (b.PASR_LastName == null || b.PASR_LastName == "0" ? "" : b.PASR_LastName)),
                                                           regno = b.PASR_RegistrationNo,
                                                           PASWMS_MarksScored = a.PASWMS_MarksScored,
                                                           classname = e.ASMCL_ClassName,
                                                           PASR_Age = b.PASR_Age,
                                                           PASR_Medium = b.PASR_Medium,
                                                           caste = g.IMCC_CategoryName,
                                                           Remark = b.Remark,
                                                           scheduleddate=f.PAWTS_ScheduleDate,
                                                           PASR_ConDistrict = b.PASR_ConDistrict
                                                       }).OrderBy(t => t.name).ToArray();
                                }
                                else
                                {
                                    data.allreports = (from a in _MarksReportContext.WrittenTestStudentSubjectWiseMarksDMO
                                                       from b in _MarksReportContext.StudentDetailsDMO
                                                       from c in _MarksReportContext.WrittenTestScheduleStudentInsertDMO
                                                       from d in _MarksReportContext.WIrttenTestSubjectWiseMarksDMO
                                                       from e in _MarksReportContext.admissioncls
                                                       from f in _MarksReportContext.writentest
                                                       from g in _MarksReportContext.castecategory
                                                       where (a.PASWM_Id == d.PASWM_Id && a.PASR_Id == b.PASR_Id && c.PASR_Id == b.PASR_Id && f.PAWTS_Id == c.PAWTS_Id && b.CasteCategory_Id == g.IMCC_Id && b.MI_Id == data.mid && b.ASMCL_Id == e.ASMCL_Id && f.ASMAY_Id == data.yearid && b.CasteCategory_Id == data.CasteCategory_Id)
                                                       select new MarksReportDTO
                                                       {
                                                           ISMS_Id = d.ISMS_ID,
                                                           PASR_Id = b.PASR_Id,
                                                           PASR_FirstName = b.PASR_FirstName,
                                                           PASR_MiddleName = b.PASR_MiddleName,
                                                           PASR_LastName = b.PASR_LastName,
                                                           name = ((b.PASR_FirstName == null || b.PASR_FirstName == "0" ? "" : b.PASR_FirstName) + " " + (b.PASR_MiddleName == null || b.PASR_MiddleName == "0" ? "" : b.PASR_MiddleName) + " " + (b.PASR_LastName == null || b.PASR_LastName == "0" ? "" : b.PASR_LastName)),
                                                           regno = b.PASR_RegistrationNo,
                                                           PASWMS_MarksScored = a.PASWMS_MarksScored,
                                                           classname = e.ASMCL_ClassName,
                                                           PASR_Age = b.PASR_Age,
                                                           PASR_Medium = b.PASR_Medium,
                                                           caste = g.IMCC_CategoryName,
                                                           Remark = b.Remark,
                                                           scheduleddate = f.PAWTS_ScheduleDate,
                                                           PASR_ConDistrict = b.PASR_ConDistrict
                                                       }).OrderBy(t => t.name).ToArray();
                                }

                            }
                            else if (data.ordertype == "ApplNo")
                            {
                                if (data.CasteCategory_Id == 0)
                                {
                                    data.allreports = (from a in _MarksReportContext.WrittenTestStudentSubjectWiseMarksDMO
                                                       from b in _MarksReportContext.StudentDetailsDMO
                                                       from c in _MarksReportContext.WrittenTestScheduleStudentInsertDMO
                                                       from d in _MarksReportContext.WIrttenTestSubjectWiseMarksDMO
                                                       from e in _MarksReportContext.admissioncls
                                                       from f in _MarksReportContext.writentest
                                                       from g in _MarksReportContext.castecategory
                                                       where (a.PASWM_Id == d.PASWM_Id && a.PASR_Id == b.PASR_Id && c.PASR_Id == b.PASR_Id && f.PAWTS_Id == c.PAWTS_Id && b.CasteCategory_Id == g.IMCC_Id && b.MI_Id == data.mid && b.ASMCL_Id == e.ASMCL_Id && f.ASMAY_Id == data.yearid)
                                                       select new MarksReportDTO
                                                       {
                                                           ISMS_Id = d.ISMS_ID,
                                                           PASR_Id = b.PASR_Id,
                                                           PASR_FirstName = b.PASR_FirstName,
                                                           PASR_MiddleName = b.PASR_MiddleName,
                                                           PASR_LastName = b.PASR_LastName,
                                                           // name = b.PASR_FirstName + " " + b.PASR_MiddleName + " " + b.PASR_LastName,
                                                           regno = b.PASR_RegistrationNo,
                                                           PASWMS_MarksScored = a.PASWMS_MarksScored,
                                                           classname = e.ASMCL_ClassName,
                                                           PASR_Age = b.PASR_Age,
                                                           PASR_Medium = b.PASR_Medium,
                                                           caste = g.IMCC_CategoryName,
                                                           Remark = b.Remark,
                                                           scheduleddate = f.PAWTS_ScheduleDate,
                                                           PASR_ConDistrict = b.PASR_ConDistrict
                                                       }).OrderBy(t => t.regno).ToArray();
                                }
                                else
                                {
                                    data.allreports = (from a in _MarksReportContext.WrittenTestStudentSubjectWiseMarksDMO
                                                       from b in _MarksReportContext.StudentDetailsDMO
                                                       from c in _MarksReportContext.WrittenTestScheduleStudentInsertDMO
                                                       from d in _MarksReportContext.WIrttenTestSubjectWiseMarksDMO
                                                       from e in _MarksReportContext.admissioncls
                                                       from f in _MarksReportContext.writentest
                                                       from g in _MarksReportContext.castecategory
                                                       where (a.PASWM_Id == d.PASWM_Id && a.PASR_Id == b.PASR_Id && c.PASR_Id == b.PASR_Id && f.PAWTS_Id == c.PAWTS_Id && b.CasteCategory_Id == g.IMCC_Id && b.MI_Id == data.mid && b.ASMCL_Id == e.ASMCL_Id && f.ASMAY_Id == data.yearid && b.CasteCategory_Id == data.CasteCategory_Id)
                                                       select new MarksReportDTO
                                                       {
                                                           ISMS_Id = d.ISMS_ID,
                                                           PASR_Id = b.PASR_Id,
                                                           PASR_FirstName = b.PASR_FirstName,
                                                           PASR_MiddleName = b.PASR_MiddleName,
                                                           PASR_LastName = b.PASR_LastName,
                                                           // name = b.PASR_FirstName + " " + b.PASR_MiddleName + " " + b.PASR_LastName,
                                                           regno = b.PASR_RegistrationNo,
                                                           PASWMS_MarksScored = a.PASWMS_MarksScored,
                                                           classname = e.ASMCL_ClassName,
                                                           PASR_Age = b.PASR_Age,
                                                           PASR_Medium = b.PASR_Medium,
                                                           caste = g.IMCC_CategoryName,
                                                           Remark = b.Remark,
                                                           scheduleddate = f.PAWTS_ScheduleDate,
                                                           PASR_ConDistrict = b.PASR_ConDistrict
                                                       }).OrderBy(t => t.regno).ToArray();
                                }

                            }
                            if (data.allreports.Length > 0)
                            {
                                studentlist = (from a in _MarksReportContext.WrittenTestStudentSubjectWiseMarksDMO
                                               from b in _MarksReportContext.StudentDetailsDMO
                                               from c in _MarksReportContext.WrittenTestScheduleStudentInsertDMO
                                               from d in _MarksReportContext.WIrttenTestSubjectWiseMarksDMO
                                               from e in _MarksReportContext.admissioncls
                                               from f in _MarksReportContext.writentest
                                               from g in _MarksReportContext.castecategory
                                               where (a.PASWM_Id == d.PASWM_Id && a.PASR_Id == b.PASR_Id && c.PASR_Id == b.PASR_Id && b.CasteCategory_Id == g.IMCC_Id && f.PAWTS_Id == c.PAWTS_Id && b.MI_Id == data.mid && b.ASMCL_Id == e.ASMCL_Id && f.ASMAY_Id == data.yearid)
                                               select new MarksReportDTO
                                               {
                                                   ISMS_Id = d.ISMS_ID,
                                                   PASR_Id = b.PASR_Id,
                                                   PASR_FirstName = b.PASR_FirstName,
                                                   PASR_MiddleName = b.PASR_MiddleName,
                                                   PASR_LastName = b.PASR_LastName,
                                                   // name = b.PASR_FirstName + " " + b.PASR_MiddleName + " " + b.PASR_LastName,
                                                   regno = b.PASR_RegistrationNo,
                                                   PASWMS_MarksScored = a.PASWMS_MarksScored,
                                                   classname = e.ASMCL_ClassName,
                                                   PASR_Age = b.PASR_Age,
                                                   PASR_Medium = b.PASR_Medium,
                                                   caste = g.IMCC_CategoryName,
                                                   Remark = b.Remark,
                                                   scheduleddate = f.PAWTS_ScheduleDate,
                                                   PASR_ConDistrict = b.PASR_ConDistrict
                                               }).OrderBy(t => t.PASR_Id).ToList();
                            }
                        }
                        else
                        {

                            if (data.ordertype == "Name" || data.ordertype == "Rank")
                            {
                                if (data.CasteCategory_Id == 0)
                                {
                                    data.allreports = (from a in _MarksReportContext.WrittenTestStudentSubjectWiseMarksDMO
                                                       from b in _MarksReportContext.StudentDetailsDMO
                                                       from c in _MarksReportContext.WrittenTestScheduleStudentInsertDMO
                                                       from d in _MarksReportContext.WIrttenTestSubjectWiseMarksDMO
                                                       from e in _MarksReportContext.admissioncls
                                                       from f in _MarksReportContext.writentest
                                                       from g in _MarksReportContext.castecategory
                                                       where (a.PASWM_Id == d.PASWM_Id && a.PASR_Id == b.PASR_Id && c.PASR_Id == b.PASR_Id && b.MI_Id == data.mid && d.ISMS_ID == data.ISMS_Id && b.ASMCL_Id == e.ASMCL_Id && f.PAWTS_Id == c.PAWTS_Id && b.CasteCategory_Id == g.IMCC_Id && f.ASMAY_Id == data.yearid)
                                                       select new MarksReportDTO
                                                       {
                                                           ISMS_Id = d.ISMS_ID,
                                                           PASR_Id = b.PASR_Id,
                                                           PASR_FirstName = b.PASR_FirstName,
                                                           PASR_MiddleName = b.PASR_MiddleName,
                                                           PASR_LastName = b.PASR_LastName,
                                                           // name = b.PASR_FirstName + " " + b.PASR_MiddleName + " " + b.PASR_LastName,
                                                           regno = b.PASR_RegistrationNo,
                                                           name = ((b.PASR_FirstName == null || b.PASR_FirstName == "0" ? "" : b.PASR_FirstName) + " " + (b.PASR_MiddleName == null || b.PASR_MiddleName == "0" ? "" : b.PASR_MiddleName) + " " + (b.PASR_LastName == null || b.PASR_LastName == "0" ? "" : b.PASR_LastName)),
                                                           PASWMS_MarksScored = a.PASWMS_MarksScored,
                                                           classname = e.ASMCL_ClassName,
                                                           PASR_Age = b.PASR_Age,
                                                           PASR_Medium = b.PASR_Medium,
                                                           caste = g.IMCC_CategoryName,
                                                           Remark = b.Remark,
                                                           scheduleddate = f.PAWTS_ScheduleDate,
                                                           PASR_ConDistrict = b.PASR_ConDistrict
                                                       }).OrderBy(t => t.name).ToArray();
                                }
                                else
                                {
                                    data.allreports = (from a in _MarksReportContext.WrittenTestStudentSubjectWiseMarksDMO
                                                       from b in _MarksReportContext.StudentDetailsDMO
                                                       from c in _MarksReportContext.WrittenTestScheduleStudentInsertDMO
                                                       from d in _MarksReportContext.WIrttenTestSubjectWiseMarksDMO
                                                       from e in _MarksReportContext.admissioncls
                                                       from f in _MarksReportContext.writentest
                                                       from g in _MarksReportContext.castecategory
                                                       where (a.PASWM_Id == d.PASWM_Id && a.PASR_Id == b.PASR_Id && c.PASR_Id == b.PASR_Id && b.MI_Id == data.mid && d.ISMS_ID == data.ISMS_Id && b.ASMCL_Id == e.ASMCL_Id && f.PAWTS_Id == c.PAWTS_Id && b.CasteCategory_Id == g.IMCC_Id && f.ASMAY_Id == data.yearid && b.CasteCategory_Id == data.CasteCategory_Id)
                                                       select new MarksReportDTO
                                                       {
                                                           ISMS_Id = d.ISMS_ID,
                                                           PASR_Id = b.PASR_Id,
                                                           PASR_FirstName = b.PASR_FirstName,
                                                           PASR_MiddleName = b.PASR_MiddleName,
                                                           PASR_LastName = b.PASR_LastName,
                                                           // name = b.PASR_FirstName + " " + b.PASR_MiddleName + " " + b.PASR_LastName,
                                                           regno = b.PASR_RegistrationNo,
                                                           name = ((b.PASR_FirstName == null || b.PASR_FirstName == "0" ? "" : b.PASR_FirstName) + " " + (b.PASR_MiddleName == null || b.PASR_MiddleName == "0" ? "" : b.PASR_MiddleName) + " " + (b.PASR_LastName == null || b.PASR_LastName == "0" ? "" : b.PASR_LastName)),
                                                           PASWMS_MarksScored = a.PASWMS_MarksScored,
                                                           classname = e.ASMCL_ClassName,
                                                           PASR_Age = b.PASR_Age,
                                                           PASR_Medium = b.PASR_Medium,
                                                           caste = g.IMCC_CategoryName,
                                                           Remark = b.Remark,
                                                           scheduleddate = f.PAWTS_ScheduleDate,
                                                           PASR_ConDistrict = b.PASR_ConDistrict
                                                       }).OrderBy(t => t.name).ToArray();

                                }

                            }
                            else if (data.ordertype == "ApplNo")
                            {
                                if (data.CasteCategory_Id == 0)
                                {
                                    data.allreports = (from a in _MarksReportContext.WrittenTestStudentSubjectWiseMarksDMO
                                                       from b in _MarksReportContext.StudentDetailsDMO
                                                       from c in _MarksReportContext.WrittenTestScheduleStudentInsertDMO
                                                       from d in _MarksReportContext.WIrttenTestSubjectWiseMarksDMO
                                                       from e in _MarksReportContext.admissioncls
                                                       from f in _MarksReportContext.writentest
                                                       from g in _MarksReportContext.castecategory
                                                       where (a.PASWM_Id == d.PASWM_Id && a.PASR_Id == b.PASR_Id && c.PASR_Id == b.PASR_Id && b.MI_Id == data.mid && d.ISMS_ID == data.ISMS_Id && b.ASMCL_Id == e.ASMCL_Id && f.PAWTS_Id == c.PAWTS_Id && b.CasteCategory_Id == g.IMCC_Id && f.ASMAY_Id == data.yearid)
                                                       select new MarksReportDTO
                                                       {
                                                           ISMS_Id = d.ISMS_ID,
                                                           PASR_Id = b.PASR_Id,
                                                           PASR_FirstName = b.PASR_FirstName,
                                                           PASR_MiddleName = b.PASR_MiddleName,
                                                           PASR_LastName = b.PASR_LastName,
                                                           // name = b.PASR_FirstName + " " + b.PASR_MiddleName + " " + b.PASR_LastName,
                                                           regno = b.PASR_RegistrationNo,
                                                           PASWMS_MarksScored = a.PASWMS_MarksScored,
                                                           classname = e.ASMCL_ClassName,
                                                           PASR_Age = b.PASR_Age,
                                                           PASR_Medium = b.PASR_Medium,
                                                           caste = g.IMCC_CategoryName,
                                                           Remark = b.Remark,
                                                           scheduleddate = f.PAWTS_ScheduleDate,
                                                           PASR_ConDistrict = b.PASR_ConDistrict
                                                       }).OrderBy(t => t.regno).ToArray();
                                }
                                else
                                {
                                    data.allreports = (from a in _MarksReportContext.WrittenTestStudentSubjectWiseMarksDMO
                                                       from b in _MarksReportContext.StudentDetailsDMO
                                                       from c in _MarksReportContext.WrittenTestScheduleStudentInsertDMO
                                                       from d in _MarksReportContext.WIrttenTestSubjectWiseMarksDMO
                                                       from e in _MarksReportContext.admissioncls
                                                       from f in _MarksReportContext.writentest
                                                       from g in _MarksReportContext.castecategory
                                                       where (a.PASWM_Id == d.PASWM_Id && a.PASR_Id == b.PASR_Id && c.PASR_Id == b.PASR_Id && b.MI_Id == data.mid && d.ISMS_ID == data.ISMS_Id && b.ASMCL_Id == e.ASMCL_Id && f.PAWTS_Id == c.PAWTS_Id && b.CasteCategory_Id == g.IMCC_Id && f.ASMAY_Id == data.yearid && b.CasteCategory_Id == data.CasteCategory_Id)
                                                       select new MarksReportDTO
                                                       {
                                                           ISMS_Id = d.ISMS_ID,
                                                           PASR_Id = b.PASR_Id,
                                                           PASR_FirstName = b.PASR_FirstName,
                                                           PASR_MiddleName = b.PASR_MiddleName,
                                                           PASR_LastName = b.PASR_LastName,
                                                           // name = b.PASR_FirstName + " " + b.PASR_MiddleName + " " + b.PASR_LastName,
                                                           regno = b.PASR_RegistrationNo,
                                                           PASWMS_MarksScored = a.PASWMS_MarksScored,
                                                           classname = e.ASMCL_ClassName,
                                                           PASR_Age = b.PASR_Age,
                                                           PASR_Medium = b.PASR_Medium,
                                                           caste = g.IMCC_CategoryName,
                                                           Remark = b.Remark,
                                                           scheduleddate = f.PAWTS_ScheduleDate,
                                                           PASR_ConDistrict = b.PASR_ConDistrict
                                                       }).OrderBy(t => t.regno).ToArray();
                                }

                            }
                            if (data.allreports.Length > 0)
                            {
                                studentlist = (from a in _MarksReportContext.WrittenTestStudentSubjectWiseMarksDMO
                                               from b in _MarksReportContext.StudentDetailsDMO
                                               from c in _MarksReportContext.WrittenTestScheduleStudentInsertDMO
                                               from d in _MarksReportContext.WIrttenTestSubjectWiseMarksDMO
                                               from e in _MarksReportContext.admissioncls
                                               from f in _MarksReportContext.writentest
                                               from g in _MarksReportContext.castecategory
                                               where (a.PASWM_Id == d.PASWM_Id && a.PASR_Id == b.PASR_Id && c.PASR_Id == b.PASR_Id && b.MI_Id == data.mid && d.ISMS_ID == data.ISMS_Id && b.ASMCL_Id == e.ASMCL_Id && f.PAWTS_Id == c.PAWTS_Id && f.ASMAY_Id == data.yearid)
                                               select new MarksReportDTO
                                               {
                                                   ISMS_Id = d.ISMS_ID,
                                                   PASR_Id = b.PASR_Id,
                                                   PASR_FirstName = b.PASR_FirstName,
                                                   PASR_MiddleName = b.PASR_MiddleName,
                                                   PASR_LastName = b.PASR_LastName,
                                                   // name = b.PASR_FirstName + " " + b.PASR_MiddleName + " " + b.PASR_LastName,
                                                   regno = b.PASR_RegistrationNo,
                                                   PASWMS_MarksScored = a.PASWMS_MarksScored,
                                                   classname = e.ASMCL_ClassName,
                                                   PASR_Age = b.PASR_Age,
                                                   PASR_Medium = b.PASR_Medium,
                                                   caste = g.IMCC_CategoryName,
                                                   Remark = b.Remark,
                                                   scheduleddate = f.PAWTS_ScheduleDate,
                                                   PASR_ConDistrict = b.PASR_ConDistrict
                                               }).OrderBy(t => t.PASR_Id).ToList();
                            }
                        }
                    }
                    else
                    {
                        if (data.ISMS_Id == 0)
                        {

                            if (data.ordertype == "Name" || data.ordertype == "Rank")
                            {
                                if (data.CasteCategory_Id == 0)
                                {
                                    data.allreports = (from a in _MarksReportContext.WrittenTestStudentSubjectWiseMarksDMO
                                                       from b in _MarksReportContext.StudentDetailsDMO
                                                       from c in _MarksReportContext.WrittenTestScheduleStudentInsertDMO
                                                       from d in _MarksReportContext.WIrttenTestSubjectWiseMarksDMO
                                                       from e in _MarksReportContext.admissioncls
                                                       from f in _MarksReportContext.writentest
                                                       from g in _MarksReportContext.castecategory
                                                       where (a.PASWM_Id == d.PASWM_Id && a.PASR_Id == b.PASR_Id && b.CasteCategory_Id == g.IMCC_Id &&  f.PAWTS_Id == c.PAWTS_Id && c.PASR_Id == b.PASR_Id && b.MI_Id == data.mid && c.PAWTS_Id == data.schids && b.ASMCL_Id == e.ASMCL_Id && b.CasteCategory_Id == g.IMCC_Id)
                                                       select new MarksReportDTO
                                                       {
                                                           ISMS_Id = d.ISMS_ID,
                                                           PASR_Id = b.PASR_Id,
                                                           PASR_FirstName = b.PASR_FirstName,
                                                           PASR_MiddleName = b.PASR_MiddleName,
                                                           PASR_LastName = b.PASR_LastName,
                                                           regno = b.PASR_RegistrationNo,
                                                           name = ((b.PASR_FirstName == null || b.PASR_FirstName == "0" ? "" : b.PASR_FirstName) + " " + (b.PASR_MiddleName == null || b.PASR_MiddleName == "0" ? "" : b.PASR_MiddleName) + " " + (b.PASR_LastName == null || b.PASR_LastName == "0" ? "" : b.PASR_LastName)),
                                                           PASWMS_MarksScored = a.PASWMS_MarksScored,
                                                           classname = e.ASMCL_ClassName,
                                                           PASR_Age = b.PASR_Age,
                                                           PASR_Medium = b.PASR_Medium,
                                                           caste = g.IMCC_CategoryName,
                                                           Remark = b.Remark,
                                                           scheduleddate = f.PAWTS_ScheduleDate,
                                                           PASR_ConDistrict = b.PASR_ConDistrict
                                                       }).OrderBy(t => t.name).OrderBy(t => t.PASWMS_Id).ToArray();
                                }
                                else
                                {
                                    data.allreports = (from a in _MarksReportContext.WrittenTestStudentSubjectWiseMarksDMO
                                                       from b in _MarksReportContext.StudentDetailsDMO
                                                       from c in _MarksReportContext.WrittenTestScheduleStudentInsertDMO
                                                       from d in _MarksReportContext.WIrttenTestSubjectWiseMarksDMO
                                                       from e in _MarksReportContext.admissioncls
                                                       from f in _MarksReportContext.writentest
                                                       from g in _MarksReportContext.castecategory
                                                       where (a.PASWM_Id == d.PASWM_Id && a.PASR_Id == b.PASR_Id && c.PASR_Id == b.PASR_Id &&  f.PAWTS_Id == c.PAWTS_Id && b.MI_Id == data.mid && c.PAWTS_Id == data.schids && b.ASMCL_Id == e.ASMCL_Id && b.CasteCategory_Id == g.IMCC_Id && b.CasteCategory_Id == data.CasteCategory_Id)
                                                       select new MarksReportDTO
                                                       {
                                                           ISMS_Id = d.ISMS_ID,
                                                           PASR_Id = b.PASR_Id,
                                                           PASR_FirstName = b.PASR_FirstName,
                                                           PASR_MiddleName = b.PASR_MiddleName,
                                                           PASR_LastName = b.PASR_LastName,
                                                           regno = b.PASR_RegistrationNo,
                                                           name = ((b.PASR_FirstName == null || b.PASR_FirstName == "0" ? "" : b.PASR_FirstName) + " " + (b.PASR_MiddleName == null || b.PASR_MiddleName == "0" ? "" : b.PASR_MiddleName) + " " + (b.PASR_LastName == null || b.PASR_LastName == "0" ? "" : b.PASR_LastName)),
                                                           PASWMS_MarksScored = a.PASWMS_MarksScored,
                                                           classname = e.ASMCL_ClassName,
                                                           PASR_Age = b.PASR_Age,
                                                           PASR_Medium = b.PASR_Medium,
                                                           caste = g.IMCC_CategoryName,
                                                           Remark = b.Remark,
                                                           scheduleddate = f.PAWTS_ScheduleDate,
                                                           PASR_ConDistrict = b.PASR_ConDistrict
                                                       }).OrderBy(t => t.name).OrderBy(t => t.PASWMS_Id).ToArray();
                                }

                            }
                            else if (data.ordertype == "ApplNo")
                            {
                                if (data.CasteCategory_Id == 0)
                                {
                                    data.allreports = (from a in _MarksReportContext.WrittenTestStudentSubjectWiseMarksDMO
                                                       from b in _MarksReportContext.StudentDetailsDMO
                                                       from c in _MarksReportContext.WrittenTestScheduleStudentInsertDMO
                                                       from d in _MarksReportContext.WIrttenTestSubjectWiseMarksDMO
                                                       from e in _MarksReportContext.admissioncls
                                                       from f in _MarksReportContext.writentest
                                                       from g in _MarksReportContext.castecategory
                                                       where (a.PASWM_Id == d.PASWM_Id && a.PASR_Id == b.PASR_Id && c.PASR_Id == b.PASR_Id && f.PAWTS_Id == c.PAWTS_Id && b.MI_Id == data.mid && c.PAWTS_Id == data.schids && b.ASMCL_Id == e.ASMCL_Id && b.CasteCategory_Id == g.IMCC_Id)
                                                       select new MarksReportDTO
                                                       {
                                                           ISMS_Id = d.ISMS_ID,
                                                           PASR_Id = b.PASR_Id,
                                                           PASR_FirstName = b.PASR_FirstName,
                                                           PASR_MiddleName = b.PASR_MiddleName,
                                                           PASR_LastName = b.PASR_LastName,
                                                           regno = b.PASR_RegistrationNo,
                                                           PASWMS_MarksScored = a.PASWMS_MarksScored,
                                                           classname = e.ASMCL_ClassName
                                                       }).OrderBy(t => t.regno).OrderBy(t => t.PASWMS_Id).ToArray();
                                }
                                else
                                {
                                    data.allreports = (from a in _MarksReportContext.WrittenTestStudentSubjectWiseMarksDMO
                                                       from b in _MarksReportContext.StudentDetailsDMO
                                                       from c in _MarksReportContext.WrittenTestScheduleStudentInsertDMO
                                                       from d in _MarksReportContext.WIrttenTestSubjectWiseMarksDMO
                                                       from f in _MarksReportContext.writentest
                                                       from e in _MarksReportContext.admissioncls
                                                       from g in _MarksReportContext.castecategory
                                                       where (a.PASWM_Id == d.PASWM_Id && a.PASR_Id == b.PASR_Id && c.PASR_Id == b.PASR_Id && f.PAWTS_Id == c.PAWTS_Id  && b.MI_Id == data.mid && c.PAWTS_Id == data.schids && b.ASMCL_Id == e.ASMCL_Id && b.CasteCategory_Id == g.IMCC_Id && b.CasteCategory_Id == data.CasteCategory_Id)
                                                       select new MarksReportDTO
                                                       {
                                                           ISMS_Id = d.ISMS_ID,
                                                           PASR_Id = b.PASR_Id,
                                                           PASR_FirstName = b.PASR_FirstName,
                                                           PASR_MiddleName = b.PASR_MiddleName,
                                                           PASR_LastName = b.PASR_LastName,
                                                           regno = b.PASR_RegistrationNo,
                                                           PASWMS_MarksScored = a.PASWMS_MarksScored,
                                                           classname = e.ASMCL_ClassName
                                                       }).OrderBy(t => t.regno).OrderBy(t => t.PASWMS_Id).ToArray();

                                }


                            }

                            if (data.allreports.Length > 0)
                            {
                                studentlist = (from a in _MarksReportContext.WrittenTestStudentSubjectWiseMarksDMO
                                               from b in _MarksReportContext.StudentDetailsDMO
                                               from c in _MarksReportContext.WrittenTestScheduleStudentInsertDMO
                                               from d in _MarksReportContext.WIrttenTestSubjectWiseMarksDMO
                                               from f in _MarksReportContext.writentest
                                               from e in _MarksReportContext.admissioncls
                                               where (a.PASWM_Id == d.PASWM_Id && a.PASR_Id == b.PASR_Id && c.PASR_Id == b.PASR_Id && f.PAWTS_Id == c.PAWTS_Id && b.MI_Id == data.mid && c.PAWTS_Id == data.schids && b.ASMCL_Id == e.ASMCL_Id)
                                               select new MarksReportDTO
                                               {
                                                   ISMS_Id = d.ISMS_ID,
                                                   PASR_Id = b.PASR_Id,
                                                   PASR_FirstName = b.PASR_FirstName,
                                                   PASR_MiddleName = b.PASR_MiddleName,
                                                   PASR_LastName = b.PASR_LastName,
                                                   regno = b.PASR_RegistrationNo,
                                                   PASWMS_MarksScored = a.PASWMS_MarksScored,
                                                   classname = e.ASMCL_ClassName
                                               }).OrderBy(t => t.PASR_Id).OrderBy(t => t.PASWMS_Id).ToList();
                            }
                        }
                        else
                        {
                            if (data.ordertype == "Name" || data.ordertype == "Rank")
                            {
                                if (data.CasteCategory_Id == 0)
                                {
                                    data.allreports = (from a in _MarksReportContext.WrittenTestStudentSubjectWiseMarksDMO
                                                       from b in _MarksReportContext.StudentDetailsDMO
                                                       from c in _MarksReportContext.WrittenTestScheduleStudentInsertDMO
                                                       from d in _MarksReportContext.WIrttenTestSubjectWiseMarksDMO
                                                       from e in _MarksReportContext.admissioncls
                                                       from f in _MarksReportContext.writentest
                                                       from g in _MarksReportContext.castecategory
                                                       where (a.PASWM_Id == d.PASWM_Id && a.PASR_Id == b.PASR_Id && c.PASR_Id == b.PASR_Id && f.PAWTS_Id == c.PAWTS_Id && b.MI_Id == data.mid && c.PAWTS_Id == data.schids && d.ISMS_ID == data.ISMS_Id && b.ASMCL_Id == e.ASMCL_Id && b.CasteCategory_Id == g.IMCC_Id)
                                                       select new MarksReportDTO
                                                       {
                                                           ISMS_Id = d.ISMS_ID,
                                                           PASR_Id = b.PASR_Id,
                                                           PASR_FirstName = b.PASR_FirstName,
                                                           PASR_MiddleName = b.PASR_MiddleName,
                                                           PASR_LastName = b.PASR_LastName,
                                                           name = ((b.PASR_FirstName == null || b.PASR_FirstName == "0" ? "" : b.PASR_FirstName) + " " + (b.PASR_MiddleName == null || b.PASR_MiddleName == "0" ? "" : b.PASR_MiddleName) + " " + (b.PASR_LastName == null || b.PASR_LastName == "0" ? "" : b.PASR_LastName)),
                                                           regno = b.PASR_RegistrationNo,
                                                           PASWMS_MarksScored = a.PASWMS_MarksScored,
                                                           classname = e.ASMCL_ClassName
                                                       }).OrderBy(t => t.name).ToArray();
                                }
                                else
                                {
                                    data.allreports = (from a in _MarksReportContext.WrittenTestStudentSubjectWiseMarksDMO
                                                       from b in _MarksReportContext.StudentDetailsDMO
                                                       from c in _MarksReportContext.WrittenTestScheduleStudentInsertDMO
                                                       from d in _MarksReportContext.WIrttenTestSubjectWiseMarksDMO
                                                       from e in _MarksReportContext.admissioncls
                                                       from f in _MarksReportContext.writentest
                                                       from g in _MarksReportContext.castecategory
                                                       where (a.PASWM_Id == d.PASWM_Id && a.PASR_Id == b.PASR_Id && c.PASR_Id == b.PASR_Id &&  f.PAWTS_Id == c.PAWTS_Id && b.MI_Id == data.mid && c.PAWTS_Id == data.schids && d.ISMS_ID == data.ISMS_Id && b.ASMCL_Id == e.ASMCL_Id && b.CasteCategory_Id == g.IMCC_Id && b.CasteCategory_Id == data.CasteCategory_Id)
                                                       select new MarksReportDTO
                                                       {
                                                           ISMS_Id = d.ISMS_ID,
                                                           PASR_Id = b.PASR_Id,
                                                           PASR_FirstName = b.PASR_FirstName,
                                                           PASR_MiddleName = b.PASR_MiddleName,
                                                           PASR_LastName = b.PASR_LastName,
                                                           name = ((b.PASR_FirstName == null || b.PASR_FirstName == "0" ? "" : b.PASR_FirstName) + " " + (b.PASR_MiddleName == null || b.PASR_MiddleName == "0" ? "" : b.PASR_MiddleName) + " " + (b.PASR_LastName == null || b.PASR_LastName == "0" ? "" : b.PASR_LastName)),
                                                           regno = b.PASR_RegistrationNo,
                                                           PASWMS_MarksScored = a.PASWMS_MarksScored,
                                                           classname = e.ASMCL_ClassName
                                                       }).OrderBy(t => t.name).ToArray();

                                }

                            }
                            else if (data.ordertype == "ApplNo")
                            {
                                if (data.CasteCategory_Id == 0)
                                {
                                    data.allreports = (from a in _MarksReportContext.WrittenTestStudentSubjectWiseMarksDMO
                                                       from b in _MarksReportContext.StudentDetailsDMO
                                                       from c in _MarksReportContext.WrittenTestScheduleStudentInsertDMO
                                                       from d in _MarksReportContext.WIrttenTestSubjectWiseMarksDMO
                                                       from e in _MarksReportContext.admissioncls
                                                       from f in _MarksReportContext.writentest
                                                       from g in _MarksReportContext.castecategory
                                                       where (a.PASWM_Id == d.PASWM_Id && a.PASR_Id == b.PASR_Id && c.PASR_Id == b.PASR_Id && f.PAWTS_Id == c.PAWTS_Id && b.MI_Id == data.mid && c.PAWTS_Id == data.schids && d.ISMS_ID == data.ISMS_Id && b.ASMCL_Id == e.ASMCL_Id && b.CasteCategory_Id == g.IMCC_Id)
                                                       select new MarksReportDTO
                                                       {
                                                           ISMS_Id = d.ISMS_ID,
                                                           PASR_Id = b.PASR_Id,
                                                           PASR_FirstName = b.PASR_FirstName,
                                                           PASR_MiddleName = b.PASR_MiddleName,
                                                           PASR_LastName = b.PASR_LastName,
                                                           regno = b.PASR_RegistrationNo,
                                                           PASWMS_MarksScored = a.PASWMS_MarksScored,
                                                           classname = e.ASMCL_ClassName
                                                       }).OrderBy(t => t.regno).ToArray();
                                }
                                else
                                {
                                    data.allreports = (from a in _MarksReportContext.WrittenTestStudentSubjectWiseMarksDMO
                                                       from b in _MarksReportContext.StudentDetailsDMO
                                                       from c in _MarksReportContext.WrittenTestScheduleStudentInsertDMO
                                                       from d in _MarksReportContext.WIrttenTestSubjectWiseMarksDMO
                                                       from e in _MarksReportContext.admissioncls
                                                       from f in _MarksReportContext.writentest
                                                       from g in _MarksReportContext.castecategory
                                                       where (a.PASWM_Id == d.PASWM_Id && a.PASR_Id == b.PASR_Id && c.PASR_Id == b.PASR_Id && f.PAWTS_Id == c.PAWTS_Id && b.MI_Id == data.mid && c.PAWTS_Id == data.schids && d.ISMS_ID == data.ISMS_Id && b.ASMCL_Id == e.ASMCL_Id && b.CasteCategory_Id == g.IMCC_Id && b.CasteCategory_Id == data.CasteCategory_Id)
                                                       select new MarksReportDTO
                                                       {
                                                           ISMS_Id = d.ISMS_ID,
                                                           PASR_Id = b.PASR_Id,
                                                           PASR_FirstName = b.PASR_FirstName,
                                                           PASR_MiddleName = b.PASR_MiddleName,
                                                           PASR_LastName = b.PASR_LastName,
                                                           regno = b.PASR_RegistrationNo,
                                                           PASWMS_MarksScored = a.PASWMS_MarksScored,
                                                           classname = e.ASMCL_ClassName
                                                       }).OrderBy(t => t.regno).ToArray();
                                }

                            }
                            if (data.allreports.Length > 0)
                            {
                                studentlist = (from a in _MarksReportContext.WrittenTestStudentSubjectWiseMarksDMO
                                               from b in _MarksReportContext.StudentDetailsDMO
                                               from c in _MarksReportContext.WrittenTestScheduleStudentInsertDMO
                                               from d in _MarksReportContext.WIrttenTestSubjectWiseMarksDMO
                                               from f in _MarksReportContext.writentest
                                               from e in _MarksReportContext.admissioncls
                                               where (a.PASWM_Id == d.PASWM_Id && a.PASR_Id == b.PASR_Id && c.PASR_Id == b.PASR_Id && f.PAWTS_Id == c.PAWTS_Id && b.MI_Id == data.mid && c.PAWTS_Id == data.schids && d.ISMS_ID == data.ISMS_Id && b.ASMCL_Id == e.ASMCL_Id)
                                               select new MarksReportDTO
                                               {
                                                   ISMS_Id = d.ISMS_ID,
                                                   PASR_Id = b.PASR_Id,
                                                   PASR_FirstName = b.PASR_FirstName,
                                                   PASR_MiddleName = b.PASR_MiddleName,
                                                   PASR_LastName = b.PASR_LastName,
                                                   regno = b.PASR_RegistrationNo,
                                                   PASWMS_MarksScored = a.PASWMS_MarksScored,
                                                   classname = e.ASMCL_ClassName
                                               }).OrderBy(t => t.PASR_Id).ToList();
                            }
                        }
                    }

                    if (data.allreports.Length > 0)
                    {
                        string studentids = "0";
                        if (studentlist.Count > 0)
                        {
                            foreach (var ue in studentlist)
                            {
                                studentids = studentids + "," + ue.PASR_Id;
                            }
                        }

                        using (var cmd = _MarksReportContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Preadmission_StudentRnk_PASR";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@studentids",
                              SqlDbType.VarChar)
                            {
                                Value = studentids
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
                                        var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                        {
                                            dataRow1.Add(
                                                dataReader.GetName(iFiled1),
                                                dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                            );
                                        }
                                        retObject.Add((ExpandoObject)dataRow1);
                                    }
                                }
                                data.ranklist = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.Write(ex.Message);
                            }
                        }
                    }
                }
                else
                {
                    data.fillhead = (from a in _MarksReportContext.allSubject
                                     where (a.MI_Id == data.mid && a.ISMS_PreadmFlag == 1 && a.ISMS_ActiveFlag == 1)
                                     select new MarksReportDTO
                                     {
                                         hid = a.ISMS_Id,
                                         hhead = a.ISMS_SubjectName,
                                         hmaxmarks = a.ISMS_Max_Marks
                                     }).ToArray();

                    if (data.schids == 0)
                    {

                        if (data.ordertype == "Name" || data.ordertype == "Rank")
                        {

                            data.allreports = (from a in _MarksReportContext.OralTestStudentWiseMarksDMO
                                               from b in _MarksReportContext.StudentDetailsDMO
                                               from c in _MarksReportContext.OralTestScheduleStudentInsertDMO
                                               from d in _MarksReportContext.admissioncls
                                               from e in _MarksReportContext.oraltest
                                               where (a.PASR_Id == b.PASR_Id && b.PASR_Id == c.PASR_Id && a.PASR_Id == c.PASR_Id && e.PAOTS_Id == c.PAOTS_Id && b.MI_Id == data.mid && b.ASMCL_Id == d.ASMCL_Id && e.ASMAY_Id == data.yearid)
                                               select new MarksReportDTO
                                               {
                                                   PASR_Id = b.PASR_Id,
                                                   PASR_FirstName = b.PASR_FirstName,
                                                   PASR_MiddleName = b.PASR_MiddleName,
                                                   PASR_LastName = b.PASR_LastName,
                                                   // name = b.PASR_FirstName + " " + b.PASR_MiddleName + " " + b.PASR_LastName,
                                                   regno = b.PASR_RegistrationNo,
                                                   ISMS_Id = a.PAOTM_Id,
                                                   PASWMS_MarksScored = a.PAOTMS_Marks,
                                                   classname = d.ASMCL_ClassName,
                                                   PASR_Age = b.PASR_Age,
                                                   PASR_Medium = b.PASR_Medium,
                                                   Remark = b.Remark,
                                                   PASR_ConDistrict = b.PASR_ConDistrict

                                               }).OrderBy(t => t.name).ToArray();
                            //data.allreports = (from a in _MarksReportContext.OralTestStudentWiseMarksDMO
                            //                   from b in _MarksReportContext.StudentDetailsDMO
                            //                   from e in _MarksReportContext.OralTestScheduleStudentInsertDMO
                            //                   from c in _MarksReportContext.oraltest
                            //                   from d in _MarksReportContext.admissioncls
                            //                   where (a.PASR_Id == b.PASR_Id && a.PASR_Id == e.PASR_Id && c.PAOTS_Id == e.PAOTS_Id && b.ASMCL_Id == d.ASMCL_Id && c.ASMAY_Id == data.yearid)
                            //                   select new MarksReportDTO
                            //                   {
                            //                       PASR_Id = b.PASR_Id,
                            //                       PASR_FirstName = b.PASR_FirstName,
                            //                       PASR_MiddleName = b.PASR_MiddleName,
                            //                       PASR_LastName = b.PASR_LastName,
                            //                       name = ((b.PASR_FirstName == null || b.PASR_FirstName == "0" ? "" : b.PASR_FirstName) + " " + (b.PASR_MiddleName == null || b.PASR_MiddleName == "0" ? "" : b.PASR_MiddleName) + " " + (b.PASR_LastName == null || b.PASR_LastName == "0" ? "" : b.PASR_LastName)),
                            //                       regno = b.PASR_RegistrationNo,
                            //                       PASWMS_MarksScored = a.PAOTMS_Marks,
                            //                       classname = d.ASMCL_ClassName,
                            //                       PASR_Age = b.PASR_Age,
                            //                       PASR_Medium = b.PASR_Medium,
                            //                       Remark = b.Remark,
                            //                       PASR_ConDistrict = b.PASR_ConDistrict
                            //                   }).OrderBy(t => t.name).ToArray();


                        }
                        else if (data.ordertype == "ApplNo")
                        {


                            data.allreports = (from a in _MarksReportContext.OralTestStudentWiseMarksDMO
                                               from b in _MarksReportContext.StudentDetailsDMO
                                               from e in _MarksReportContext.OralTestScheduleStudentInsertDMO
                                               from c in _MarksReportContext.oraltest
                                               from d in _MarksReportContext.admissioncls
                                               where (a.PASR_Id == b.PASR_Id && a.PASR_Id == e.PASR_Id && c.PAOTS_Id == e.PAOTS_Id && b.ASMCL_Id == d.ASMCL_Id && c.ASMAY_Id == data.yearid)
                                               select new MarksReportDTO
                                               {
                                                   PASR_Id = b.PASR_Id,
                                                   PASR_FirstName = b.PASR_FirstName,
                                                   PASR_MiddleName = b.PASR_MiddleName,
                                                   PASR_LastName = b.PASR_LastName,
                                                   name = ((b.PASR_FirstName == null || b.PASR_FirstName == "0" ? "" : b.PASR_FirstName) + " " + (b.PASR_MiddleName == null || b.PASR_MiddleName == "0" ? "" : b.PASR_MiddleName) + " " + (b.PASR_LastName == null || b.PASR_LastName == "0" ? "" : b.PASR_LastName)),
                                                   regno = b.PASR_RegistrationNo,
                                                   PASWMS_MarksScored = a.PAOTMS_Marks,
                                                   classname = d.ASMCL_ClassName,
                                                   PASR_Age = b.PASR_Age,
                                                   PASR_Medium = b.PASR_Medium,
                                                   Remark = b.Remark,
                                                   PASR_ConDistrict = b.PASR_ConDistrict
                                               }).OrderBy(t => t.regno).ToArray();


                        }
                        if (data.allreports.Length > 0)
                        {
                            //data.allreports = (from a in _MarksReportContext.OralTestStudentWiseMarksDMO
                            //                   from b in _MarksReportContext.StudentDetailsDMO
                            //                   from c in _MarksReportContext.OralTestScheduleStudentInsertDMO

                            //                   from e in _MarksReportContext.admissioncls
                            //                   from f in _MarksReportContext.oraltest
                            //                   from g in _MarksReportContext.castecategory
                            //                   where (a.PASR_Id == b.PASR_Id && c.PASR_Id == b.PASR_Id && f.PAOTS_Id == c.PAOTS_Id && b.CasteCategory_Id == g.IMCC_Id && b.MI_Id == data.mid && b.ASMCL_Id == e.ASMCL_Id && f.ASMAY_Id == data.yearid)
                            //                   select new MarksReportDTO
                            //                   {
                            //                       PASR_Id = b.PASR_Id,
                            //                       PASR_FirstName = b.PASR_FirstName,
                            //                       PASR_MiddleName = b.PASR_MiddleName,
                            //                       PASR_LastName = b.PASR_LastName,
                            //                       name = ((b.PASR_FirstName == null || b.PASR_FirstName == "0" ? "" : b.PASR_FirstName) + " " + (b.PASR_MiddleName == null || b.PASR_MiddleName == "0" ? "" : b.PASR_MiddleName) + " " + (b.PASR_LastName == null || b.PASR_LastName == "0" ? "" : b.PASR_LastName)),
                            //                       regno = b.PASR_RegistrationNo,
                            //                       PASWMS_MarksScored = a.PAOTMS_Marks,
                            //                       classname = e.ASMCL_ClassName,
                            //                       PASR_Age = b.PASR_Age,
                            //                       PASR_Medium = b.PASR_Medium,
                            //                       caste = g.IMCC_CategoryName,
                            //                       Remark = b.Remark,
                            //                       PASR_ConDistrict = b.PASR_ConDistrict
                            //                   }).OrderBy(t => t.name).ToArray();


                            data.allreports = (from a in _MarksReportContext.OralTestStudentWiseMarksDMO
                                               from b in _MarksReportContext.StudentDetailsDMO
                                               from e in _MarksReportContext.OralTestScheduleStudentInsertDMO
                                               from c in _MarksReportContext.oraltest
                                               from d in _MarksReportContext.admissioncls
                                               where (a.PASR_Id == b.PASR_Id && a.PASR_Id == e.PASR_Id && c.PAOTS_Id == e.PAOTS_Id && b.ASMCL_Id == d.ASMCL_Id && c.ASMAY_Id == data.yearid)
                                               select new MarksReportDTO
                                               {
                                                   PASR_Id = b.PASR_Id,
                                                   PASR_FirstName = b.PASR_FirstName,
                                                   PASR_MiddleName = b.PASR_MiddleName,
                                                   PASR_LastName = b.PASR_LastName,
                                                   name = ((b.PASR_FirstName == null || b.PASR_FirstName == "0" ? "" : b.PASR_FirstName) + " " + (b.PASR_MiddleName == null || b.PASR_MiddleName == "0" ? "" : b.PASR_MiddleName) + " " + (b.PASR_LastName == null || b.PASR_LastName == "0" ? "" : b.PASR_LastName)),
                                                   regno = b.PASR_RegistrationNo,
                                                   PASWMS_MarksScored = a.PAOTMS_Marks,
                                                   classname = d.ASMCL_ClassName,
                                                   PASR_Age = b.PASR_Age,
                                                   PASR_Medium = b.PASR_Medium,
                                                   Remark = b.Remark,
                                                   PASR_ConDistrict = b.PASR_ConDistrict
                                               }).OrderBy(t => t.regno).ToArray();

                        }


                    }
                    else
                    {


                        if (data.ordertype == "Name" || data.ordertype == "Rank")
                        {


                            data.allreports = (from a in _MarksReportContext.OralTestStudentWiseMarksDMO
                                               from b in _MarksReportContext.StudentDetailsDMO
                                               from e in _MarksReportContext.OralTestScheduleStudentInsertDMO
                                               from c in _MarksReportContext.oraltest
                                               from d in _MarksReportContext.admissioncls
                                               where (a.PASR_Id == b.PASR_Id && a.PASR_Id == e.PASR_Id && c.PAOTS_Id == e.PAOTS_Id && b.ASMCL_Id == d.ASMCL_Id && c.ASMAY_Id == data.yearid)
                                               select new MarksReportDTO
                                               {
                                                   PASR_Id = b.PASR_Id,
                                                   PASR_FirstName = b.PASR_FirstName,
                                                   PASR_MiddleName = b.PASR_MiddleName,
                                                   PASR_LastName = b.PASR_LastName,
                                                   name = ((b.PASR_FirstName == null || b.PASR_FirstName == "0" ? "" : b.PASR_FirstName) + " " + (b.PASR_MiddleName == null || b.PASR_MiddleName == "0" ? "" : b.PASR_MiddleName) + " " + (b.PASR_LastName == null || b.PASR_LastName == "0" ? "" : b.PASR_LastName)),
                                                   regno = b.PASR_RegistrationNo,
                                                   PASWMS_MarksScored = a.PAOTMS_Marks,
                                                   classname = d.ASMCL_ClassName,
                                                   PASR_Age = b.PASR_Age,
                                                   PASR_Medium = b.PASR_Medium,
                                                   Remark = b.Remark,
                                                   scheduleddate=c.PAOTS_ScheduleDate,
                                                   PASR_ConDistrict = b.PASR_ConDistrict
                                               }).OrderBy(t => t.name).ToArray();
                        }


                        else if (data.ordertype == "ApplNo")
                        {


                            data.allreports = (from a in _MarksReportContext.OralTestStudentWiseMarksDMO
                                               from b in _MarksReportContext.StudentDetailsDMO
                                               from e in _MarksReportContext.OralTestScheduleStudentInsertDMO
                                               from c in _MarksReportContext.oraltest
                                               from d in _MarksReportContext.admissioncls
                                               where (a.PASR_Id == b.PASR_Id && a.PASR_Id == e.PASR_Id && c.PAOTS_Id == e.PAOTS_Id && b.ASMCL_Id == d.ASMCL_Id && c.ASMAY_Id == data.yearid)
                                               select new MarksReportDTO
                                               {
                                                   PASR_Id = b.PASR_Id,
                                                   PASR_FirstName = b.PASR_FirstName,
                                                   PASR_MiddleName = b.PASR_MiddleName,
                                                   PASR_LastName = b.PASR_LastName,
                                                   name = ((b.PASR_FirstName == null || b.PASR_FirstName == "0" ? "" : b.PASR_FirstName) + " " + (b.PASR_MiddleName == null || b.PASR_MiddleName == "0" ? "" : b.PASR_MiddleName) + " " + (b.PASR_LastName == null || b.PASR_LastName == "0" ? "" : b.PASR_LastName)),
                                                   regno = b.PASR_RegistrationNo,
                                                   PASWMS_MarksScored = a.PAOTMS_Marks,
                                                   classname = d.ASMCL_ClassName,
                                                   PASR_Age = b.PASR_Age,
                                                   PASR_Medium = b.PASR_Medium,
                                                   Remark = b.Remark,
                                                   scheduleddate = c.PAOTS_ScheduleDate,
                                                   PASR_ConDistrict = b.PASR_ConDistrict
                                               }).OrderBy(t => t.regno).ToArray();
                        }
                        else
                        {

                            data.allreports = (from a in _MarksReportContext.OralTestStudentWiseMarksDMO
                                               from b in _MarksReportContext.StudentDetailsDMO
                                               from e in _MarksReportContext.OralTestScheduleStudentInsertDMO
                                               from c in _MarksReportContext.oraltest
                                               from d in _MarksReportContext.admissioncls
                                               where (a.PASR_Id == b.PASR_Id && a.PASR_Id == e.PASR_Id && c.PAOTS_Id == e.PAOTS_Id && b.ASMCL_Id == d.ASMCL_Id && c.ASMAY_Id == data.yearid)
                                               select new MarksReportDTO
                                               {
                                                   PASR_Id = b.PASR_Id,
                                                   PASR_FirstName = b.PASR_FirstName,
                                                   PASR_MiddleName = b.PASR_MiddleName,
                                                   PASR_LastName = b.PASR_LastName,
                                                   name = ((b.PASR_FirstName == null || b.PASR_FirstName == "0" ? "" : b.PASR_FirstName) + " " + (b.PASR_MiddleName == null || b.PASR_MiddleName == "0" ? "" : b.PASR_MiddleName) + " " + (b.PASR_LastName == null || b.PASR_LastName == "0" ? "" : b.PASR_LastName)),
                                                   regno = b.PASR_RegistrationNo,
                                                   PASWMS_MarksScored = a.PAOTMS_Marks,
                                                   classname = d.ASMCL_ClassName,
                                                   PASR_Age = b.PASR_Age,
                                                   PASR_Medium = b.PASR_Medium,
                                                   Remark = b.Remark,
                                                   scheduleddate = c.PAOTS_ScheduleDate,
                                                   PASR_ConDistrict = b.PASR_ConDistrict
                                               }).OrderBy(t => t.name).ToArray();
                        }


                        if (data.allreports.Length > 0)
                        {

                            data.allreports = (from a in _MarksReportContext.OralTestStudentWiseMarksDMO
                                               from b in _MarksReportContext.StudentDetailsDMO
                                               from e in _MarksReportContext.OralTestScheduleStudentInsertDMO
                                               from c in _MarksReportContext.oraltest
                                               from d in _MarksReportContext.admissioncls
                                               where (a.PASR_Id == b.PASR_Id && a.PASR_Id == e.PASR_Id && c.PAOTS_Id == e.PAOTS_Id && b.ASMCL_Id == d.ASMCL_Id && c.ASMAY_Id == data.yearid)
                                               select new MarksReportDTO
                                               {
                                                   PASR_Id = b.PASR_Id,
                                                   PASR_FirstName = b.PASR_FirstName,
                                                   PASR_MiddleName = b.PASR_MiddleName,
                                                   PASR_LastName = b.PASR_LastName,
                                                   name = ((b.PASR_FirstName == null || b.PASR_FirstName == "0" ? "" : b.PASR_FirstName) + " " + (b.PASR_MiddleName == null || b.PASR_MiddleName == "0" ? "" : b.PASR_MiddleName) + " " + (b.PASR_LastName == null || b.PASR_LastName == "0" ? "" : b.PASR_LastName)),
                                                   regno = b.PASR_RegistrationNo,
                                                   PASWMS_MarksScored = a.PAOTMS_Marks,
                                                   classname = d.ASMCL_ClassName,
                                                   PASR_Age = b.PASR_Age,
                                                   PASR_Medium = b.PASR_Medium,
                                                   Remark = b.Remark,
                                                   scheduleddate = c.PAOTS_ScheduleDate,
                                                   PASR_ConDistrict = b.PASR_ConDistrict
                                               }).OrderBy(t => t.name).ToArray();
                        }


                    }

                    if (data.allreports.Length > 0)
                    {
                        string studentids = "0";
                        if (studentlist.Count > 0)
                        {
                            foreach (var ue in studentlist)
                            {
                                studentids = studentids + "," + ue.PASR_Id;
                            }
                        }

                        using (var cmd = _MarksReportContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Preadmission_StudentRnk_PASR";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@studentids",
                              SqlDbType.VarChar)
                            {
                                Value = studentids
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
                                        var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                        for (var iFiled1 = 0; iFiled1 < dataReader.FieldCount; iFiled1++)
                                        {
                                            dataRow1.Add(
                                                dataReader.GetName(iFiled1),
                                                dataReader.IsDBNull(iFiled1) ? 0 : dataReader[iFiled1] // use null instead of {}
                                            );
                                        }
                                        retObject.Add((ExpandoObject)dataRow1);
                                    }
                                }
                                data.ranklist = retObject.ToArray();
                            }
                            catch (Exception ex)
                            {
                                Console.Write(ex.Message);
                            }
                        }
                    }
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
