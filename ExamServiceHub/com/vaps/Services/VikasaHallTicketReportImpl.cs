using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model;
using ExamServiceHub.com.vaps.Interfaces;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Services
{
    public class VikasaHallTicketReportImpl : VikasaHallTicketReportInterface
    {
        public ExamContext _examctxt;
        public subjectmasterContext _subctxt;
        public DomainModelMsSqlServerContext _mictxt;
        public VikasaHallTicketReportImpl(ExamContext obj, subjectmasterContext obj1, DomainModelMsSqlServerContext obj2)
        {
            _examctxt = obj;
            _subctxt = obj1;
            _mictxt = obj2;

        }
        public VikasaHallTicketReportDTO report(VikasaHallTicketReportDTO data)
        {
            try
            {
                string amstids = "0";
                List<long> studentid = new List<long>();

                if (data.studentlist.Count() > 0)
                {

                    //for (int k = 0; k < data.studentlist.Count(); k++)
                    //{
                    //    studentid.Add(data.studentlist[k].AMST_Id);

                    //} //added by adarsh
                    foreach (var X in (data.studentlist))
                    {
                        studentid.Add(X.AMST_Id);
                        amstids = amstids + "," + X.AMST_Id;
                    }

                    data.datareport = (from a in _examctxt.Exm_HallTicketDMO
                                       from b in _examctxt.Adm_M_Student
                                       from c in _examctxt.AdmissionClass
                                       from d in _examctxt.School_M_Section
                                       from g in _examctxt.School_Adm_Y_Student
                                       from e in _examctxt.AcademicYear
                                       from f in _examctxt.exammasterDMO
                                       where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                                       && a.ASMS_Id == data.ASMS_Id && a.EME_Id == data.EME_Id && a.AMST_Id == b.AMST_Id && a.ASMCL_Id == c.ASMCL_Id
                                       && a.ASMS_Id == d.ASMS_Id && a.ASMAY_Id == e.ASMAY_Id && a.EME_Id == f.EME_Id && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1
                                       && g.AMST_Id == b.AMST_Id && g.AMAY_ActiveFlag == 1 && g.ASMAY_Id == data.ASMAY_Id && g.ASMCL_Id == data.ASMCL_Id
                                       && g.ASMS_Id == data.ASMS_Id && studentid.Contains(g.AMST_Id) && a.EHT_ActiveFlag == true)
                                       select new VikasaHallTicketReportDTO
                                       {
                                           AMST_Id = g.AMST_Id,
                                           AMST_FirstName = ((b.AMST_FirstName == null || b.AMST_FirstName == "" ? "" : " " + b.AMST_FirstName) + (b.AMST_MiddleName == null || b.AMST_MiddleName == "" || b.AMST_MiddleName == "0" ? "" : " " + b.AMST_MiddleName) + (b.AMST_LastName == null || b.AMST_LastName == "" || b.AMST_LastName == "0" ? "" : " " + b.AMST_LastName)).Trim(),
                                           AMST_AdmNo = b.AMST_AdmNo,
                                           AMAY_Rollno = g.AMAY_RollNo,
                                           ASMCL_ClassName = c.ASMCL_ClassName,
                                           ASMC_SectionName = d.ASMC_SectionName,
                                           EHT_HallTicketNo = a.EHT_HallTicketNo,
                                           AMST_Photoname = b.AMST_Photoname,
                                           ETTS_SessionName = b.AMST_RegistrationNo,
                                           AMST_FatherName = ((b.AMST_FatherName == null || b.AMST_FatherName == "" ? "" : " " + b.AMST_FatherName)
                                           + (b.AMST_FatherSurname == null || b.AMST_FatherSurname == "" || b.AMST_FatherSurname == "0" ? "" : " " + b.AMST_FatherSurname)).Trim(),
                                       }).Distinct().OrderBy(a => a.AMST_FirstName).ToArray();

                    using (var cmd = _examctxt.Database.GetDbConnection().CreateCommand()) //added by adarsh
                    {
                        cmd.CommandText = "Exam_Hallticket_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 9000000;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                        cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = data.EME_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = amstids });
                        
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

                            data.subarray = retObject.ToArray();
                        }
                        catch (Exception ee)
                        {
                            Console.WriteLine(ee.Message);
                        }
                    }
                    //data.subarray = (from a in _examctxt.Exm_TimeTable_SubjectsDMO
                    //                 from b in _examctxt.Exm_TimeTableDMO
                    //                 from c in _subctxt.subjectmasterDMO
                    //                 from d in _examctxt.exammasterDMO
                    //                 from e in _examctxt.Exm_TT_M_SessionDMO
                    //                 from f in _examctxt.StudentMappingDMO
                    //                 where (a.EXTT_Id == b.EXTT_Id && c.ISMS_Id == a.ISMS_Id && f.ISMS_Id == c.ISMS_Id && f.EMG_Id == b.EMG_Id
                    //                 && b.EME_Id == d.EME_Id && e.ETTS_Id == a.ETTS_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id
                    //                 && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && b.EME_Id == data.EME_Id && f.ASMAY_Id == data.ASMAY_Id
                    //                 && f.ASMCL_Id == data.ASMCL_Id && f.ASMS_Id == data.ASMS_Id && f.MI_Id == data.MI_Id && f.ESTSU_ActiveFlg == true
                    //                 && studentid.Contains(f.AMST_Id))
                    //                 select new VikasaHallTicketReportDTO
                    //                 {
                    //                     AMST_Id = f.AMST_Id,
                    //                     ISMS_SubjectName = c.ISMS_SubjectName,
                    //                     EXTTS_Date = a.EXTTS_Date,
                    //                     EME_ExamName = d.EME_ExamName,
                    //                     ETTS_SessionName = e.ETTS_SessionName,
                    //                     subjectorder = c.ISMS_OrderFlag,
                    //                     ETTS_StartTime = e.ETTS_StartTime,
                    //                     ETTS_EndTime = e.ETTS_EndTime
                    //                 }).Distinct().OrderBy(a => a.EXTTS_Date).ThenBy(a => a.subjectorder).ToArray();

                    data.studarray = (from a in _examctxt.Adm_M_Student
                                      from b in _examctxt.Exm_HallTicketDMO
                                      from c in _examctxt.AdmissionClass
                                      from d in _examctxt.School_M_Section
                                      from g in _examctxt.School_Adm_Y_Student
                                      where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id
                                      && b.EME_Id == data.EME_Id && a.AMST_Id == b.AMST_Id && c.ASMCL_Id == a.ASMCL_Id && d.ASMS_Id == b.ASMS_Id
                                      && g.AMST_Id == a.AMST_Id && g.AMAY_ActiveFlag == 1 && g.ASMAY_Id == data.ASMAY_Id && g.ASMCL_Id == data.ASMCL_Id
                                      && g.ASMS_Id == data.ASMS_Id && studentid.Contains(g.AMST_Id) && b.EHT_ActiveFlag == true)
                                      select new VikasaHallTicketReportDTO
                                      {
                                          AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "" ? "" : " " + a.AMST_FirstName) + (a.AMST_MiddleName == null || a.AMST_MiddleName == "" || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) + (a.AMST_LastName == null || a.AMST_LastName == "" || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim(),
                                          AMST_Photoname = a.AMST_Photoname,
                                          EHT_HallTicketNo = b.EHT_HallTicketNo,
                                          ASMCL_ClassName = c.ASMCL_ClassName,
                                          ASMC_SectionName = d.ASMC_SectionName
                                      }).Distinct().OrderBy(a => a.AMST_FirstName).ToArray();
                }


                data.institute = _mictxt.Institution.Where(t => t.MI_Id == data.MI_Id).ToArray();

                data.configuraion = _mictxt.GenConfig.Where(t => t.MI_Id == data.MI_Id).ToArray();

                string accountname = "";
                string accesskey = "";
                ReadTemplateFromAzure obj = new ReadTemplateFromAzure();
                var datatstu = _examctxt.IVRM_Storage_path_Details.ToList();
                if (datatstu.Count() > 0)
                {
                    accountname = datatstu.FirstOrDefault().IVRM_SD_Access_Name;
                    accesskey = datatstu.FirstOrDefault().IVRM_SD_Access_Key;
                }

                string html = "";
                try
                {
                    html = obj.getHtmlContentFromAzure(accountname, accesskey, "files/" + data.MI_Id, "HallticketReport.html", 0);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    html = "";
                }
                data.htmldata = html;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public VikasaHallTicketReportDTO getdetails(VikasaHallTicketReportDTO data)
        {
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _examctxt.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.Acdlist = list.ToArray();

                data.examlist = _examctxt.masterexam.Where(z => z.MI_Id == data.MI_Id && z.EME_ActiveFlag == true).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public VikasaHallTicketReportDTO onselectAcdYear(VikasaHallTicketReportDTO data)
        {
            try
            {
                data.ctlist = (from c in _examctxt.AdmissionClass
                               from d in _examctxt.Exm_Category_ClassDMO
                               where (c.MI_Id == data.MI_Id && c.ASMCL_Id == d.ASMCL_Id && c.ASMCL_ActiveFlag == true && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && d.ASMCL_Id == c.ASMCL_Id && d.ECAC_ActiveFlag == true)
                               select c).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public VikasaHallTicketReportDTO onselectclass(VikasaHallTicketReportDTO data)
        {
            try
            {
                data.seclist = (from c in _examctxt.School_M_Section
                                    //from e in _examctxt.AdmissionClass
                                from d in _examctxt.Exm_Category_ClassDMO
                                where (d.ASMCL_Id == data.ASMCL_Id && c.MI_Id == data.MI_Id && c.ASMS_Id == d.ASMS_Id && c.ASMC_ActiveFlag == 1 && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && d.ECAC_ActiveFlag == true && c.ASMS_Id == d.ASMS_Id)
                                select c).Distinct().OrderBy(t => t.ASMC_Order).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public VikasaHallTicketReportDTO onselectSection(VikasaHallTicketReportDTO data)
        {
            try
            {
                var EMCA_Id = _examctxt.Exm_Category_ClassDMO.SingleOrDefault(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.ECAC_ActiveFlag == true).EMCA_Id;
                var EYC_Id = _examctxt.Exm_Yearly_CategoryDMO.SingleOrDefault(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.EMCA_Id == EMCA_Id && t.EYC_ActiveFlg == true).EYC_Id;
                var examlist = (from a in _examctxt.masterexam
                                from b in _examctxt.Exm_Yearly_Category_ExamsDMO
                                where (a.MI_Id == data.MI_Id && a.EME_ActiveFlag == true && a.EME_Id == b.EME_Id && b.EYC_Id == EYC_Id && b.EYCE_ActiveFlg == true)
                                select a).Distinct().OrderBy(t => t.EME_ExamOrder).ToList();
                data.examlist = examlist.Distinct().OrderBy(t => t.EME_ExamOrder).ToArray();


                data.getstudentlist = (from b in _examctxt.Adm_M_Student
                                       from c in _examctxt.AdmissionClass
                                       from d in _examctxt.School_M_Section
                                       from g in _examctxt.School_Adm_Y_Student
                                       from e in _examctxt.AcademicYear
                                       where (g.AMST_Id == b.AMST_Id && g.ASMAY_Id == e.ASMAY_Id && g.ASMCL_Id == c.ASMCL_Id && g.ASMS_Id == d.ASMS_Id
                                       && g.ASMAY_Id == data.ASMAY_Id && g.ASMCL_Id == data.ASMCL_Id && g.ASMS_Id == data.ASMS_Id
                                       && b.MI_Id == data.MI_Id && b.AMST_SOL == "S" && b.AMST_ActiveFlag == 1 && g.AMAY_ActiveFlag == 1)
                                       select new VikasaHallTicketReportDTO
                                       {
                                           AMST_Id = g.AMST_Id,
                                           AMST_FirstName = ((b.AMST_FirstName == null || b.AMST_FirstName == "" ? "" : " " + b.AMST_FirstName) +
                                           (b.AMST_MiddleName == null || b.AMST_MiddleName == "" || b.AMST_MiddleName == "0" ? "" : " " + b.AMST_MiddleName) +
                                           (b.AMST_LastName == null || b.AMST_LastName == "" || b.AMST_LastName == "0" ? "" : " " + b.AMST_LastName)).Trim()

                                       }).Distinct().OrderBy(a => a.AMST_FirstName).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
    }
}
