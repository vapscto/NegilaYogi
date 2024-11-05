using CollegeExamServiceHub.Interfaces;
using DataAccessMsSqlServerProvider.com.vapstech.College.Exam;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.College.Exam;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using DomainModel.Model.com.vapstech.College.Exam;

namespace CollegeExamServiceHub.Services
{
    public class HallTicketGenerationCollegeIMPL : HallTicketGenerationCollege
    {
        public ClgExamContext _examctxt;
        public HallTicketGenerationCollegeIMPL(ClgExamContext obj)
        {
            _examctxt = obj;
        }

        public HallTicketGenerationCollegeDTO getdetails(HallTicketGenerationCollegeDTO data)
        {
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _examctxt.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.Acdlist = list.ToArray();
                data.examlist = _examctxt.col_exammasterDMO.Where(z => z.MI_Id == data.MI_Id && z.EME_ActiveFlag == true).OrderBy(t => t.EME_ExamOrder).ToArray();
                data.courseslist = _examctxt.MasterCourseDMO.Where(z => z.MI_Id == data.MI_Id && z.AMCO_ActiveFlag == true).Distinct().ToArray();
                //datalist 
                using (var cmd = _examctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_HallTicket_Generation_DATA";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
            SqlDbType.VarChar)
                    {
                        Value = "1"
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
            SqlDbType.BigInt)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
            SqlDbType.BigInt)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id",
           SqlDbType.BigInt)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
         SqlDbType.BigInt)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id",
        SqlDbType.BigInt)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
       SqlDbType.BigInt)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
     SqlDbType.VarChar)
                    {
                        Value = "0"
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
                        data.Alldata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //Exam_TimeTable_Generation
                using (var cmd = _examctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_TimeTable_Generation";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
            SqlDbType.VarChar)
                    {
                        Value = "1"
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
            SqlDbType.BigInt)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
            SqlDbType.BigInt)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id",
           SqlDbType.BigInt)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
         SqlDbType.BigInt)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id",
        SqlDbType.BigInt)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
       SqlDbType.BigInt)
                    {
                        Value = 0
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
     SqlDbType.VarChar)
                    {
                        Value = "0"
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
                        data.AllDataTimeTable = retObject.ToArray();
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
        public HallTicketGenerationCollegeDTO onselectAcdYear(HallTicketGenerationCollegeDTO data)
        {
            try
            {

                data.semisters = (from a in _examctxt.MasterCourseDMO
                                  from b in _examctxt.ClgMasterBranchDMO
                                  from c in _examctxt.Adm_Course_Branch_MappingDMO
                                  from d in _examctxt.AdmCourseBranchSemesterMappingDMO
                                  from e in _examctxt.CLG_Adm_Master_SemesterDMO
                                  where (a.AMCO_Id == c.AMCO_Id && b.AMB_Id == c.AMB_Id && e.AMSE_Id == d.AMSE_Id
                                  && a.MI_Id == data.MI_Id && c.AMCOBM_ActiveFlg == true && d.AMCOBM_Id == c.AMCOBM_Id
                                  && c.AMCO_Id == data.AMCO_Id && a.AMCO_Id == data.AMCO_Id && a.AMCO_ActiveFlag == true
                                  && b.AMB_ActiveFlag == true && d.AMCOBMS_ActiveFlg == true && e.AMSE_ActiveFlg == true && c.AMB_Id == data.AMB_Id)
                                  select e).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public HallTicketGenerationCollegeDTO onselectclass(HallTicketGenerationCollegeDTO data)
        {
            try
            {
                data.branchlist = (from a in _examctxt.MasterCourseDMO
                                   from b in _examctxt.ClgMasterBranchDMO
                                   from c in _examctxt.Adm_Course_Branch_MappingDMO
                                   where (a.AMCO_Id == c.AMCO_Id && b.AMB_Id == c.AMB_Id && a.MI_Id == data.MI_Id && c.AMCOBM_ActiveFlg == true
                                   && c.AMCO_Id == data.AMCO_Id && a.AMCO_Id == data.AMCO_Id && a.AMCO_ActiveFlag == true && b.AMB_ActiveFlag == true)
                                   select b).Distinct().OrderBy(a => a.AMB_Order).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public HallTicketGenerationCollegeDTO onselectsection(HallTicketGenerationCollegeDTO data)
        {
            try
            {
                data.examlist = (from s in _examctxt.col_exammasterDMO
                                 from k in _examctxt.Exm_Col_Yearly_Scheme_ExamsDMO
                                 where (s.EME_Id == k.EME_Id && k.AMCO_Id == data.AMCO_Id && k.AMB_Id == data.AMB_Id && s.MI_Id == data.MI_Id
                                 && k.AMSE_Id == data.AMSE_Id && k.ECYSE_ActiveFlg == true)
                                 select new CollegeMarksEntryDTO
                                 {
                                     EME_Id = s.EME_Id,
                                     EME_ExamName = s.EME_ExamName,
                                     EME_ExamCode = s.EME_ExamCode,
                                     EME_ExamOrder = s.EME_ExamOrder
                                 }).Distinct().OrderBy(t => t.EME_ExamOrder).ToArray();

                data.sectionlist = _examctxt.Adm_College_Master_SectionDMO.Where(R => R.MI_Id == data.MI_Id && R.ACMS_ActiveFlag == true).Distinct().ToArray();

                data.subjectshemalist = (from a in _examctxt.Exm_Col_Yearly_SchemeDMO
                                         from b in _examctxt.AdmCollegeSubjectSchemeDMO
                                         where (a.ACSS_Id == b.ACSS_Id && a.MI_Id == data.MI_Id && a.AMCO_Id == data.AMCO_Id
                                         && a.AMB_Id == data.AMB_Id && a.AMSE_Id == data.AMSE_Id && a.ECYS_ActiveFlag == true && b.ACST_ActiveFlg == true)
                                         select b).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public HallTicketGenerationCollegeDTO savedetail(HallTicketGenerationCollegeDTO data)
        {
            try
            {
                string secidget = "";
                for (int i = 0; i < data.section_Array.Length; i++)
                {

                    string Id = data.section_Array[i].ACMS_Id.ToString();
                    if (Id != "0" && Id != null)
                    {

                        secidget = Id + "," + secidget;
                    }
                }
                string coloumns = secidget.Remove(secidget.Length - 1);

                var contactExists = _examctxt.Database.ExecuteSqlCommand("Exam_HallTicket_Generation_College @p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10", data.MI_Id, data.ASMAY_Id, data.AMCO_Id, coloumns, data.EME_Id, data.prefixstr, data.startno, data.increment, data.leadingzeros, data.AMB_Id, data.AMSE_Id);
                if (contactExists > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public HallTicketGenerationCollegeDTO ViewStudentDetails(HallTicketGenerationCollegeDTO data)
        {
            try
            {
                using (var cmd = _examctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_HallTicket_Generation_DATA";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Flag",
            SqlDbType.VarChar)
                    {
                        Value = "2"
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
          SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
            SqlDbType.BigInt)
                    {
                        Value = data.AMCO_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id",
           SqlDbType.BigInt)
                    {
                        Value = data.AMB_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
         SqlDbType.BigInt)
                    {
                        Value = data.AMSE_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ACMS_Id",
        SqlDbType.BigInt)
                    {
                        Value = data.ACMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
     SqlDbType.VarChar)
                    {
                        Value = "0"
                    });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
       SqlDbType.BigInt)
                    {
                        Value = data.EME_Id
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
                        data.GetStudent = retObject.ToArray();
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
        public HallTicketGenerationCollegeDTO SaveStudentStatus(HallTicketGenerationCollegeDTO data)
        {
            try
            {
                if (data.section_Array.Length > 0)
                {
                    foreach (var c in data.section_Array)
                    {
                        var checkresult = _examctxt.Exm_HallTicketCollegeDMO.Where(a => a.AMCST_Id == c.AMCST_Id && a.EHTC_Id == c.EHTC_Id && a.MI_Id == data.MI_Id).ToList();

                        if (checkresult.Count > 0)
                        {
                            var result = _examctxt.Exm_HallTicketCollegeDMO.Single(a => a.AMCST_Id == c.AMCST_Id && a.EHTC_Id == c.EHTC_Id);
                            result.EHTC_PublishFlg = result.EHTC_PublishFlg == true ? false : true;
                            result.EHTC_UpdatedDate = DateTime.Now;
                            _examctxt.Update(result);
                        }
                    }

                    var i = _examctxt.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        //ExamReport
        public HallTicketGenerationCollegeDTO ExamReport(HallTicketGenerationCollegeDTO data)
        {
            try
            {
                string AMCST_Id = "0";
                //AMCST_Id
                if (data.AMCST_Id > 0)
                {
                    var list = _examctxt.Adm_College_Yearly_StudentDMO.Where(R => R.AMCST_Id == data.AMCST_Id && R.ASMAY_Id == data.ASMAY_Id && R.ACYST_ActiveFlag == 1).ToList();
                    data.AMCO_Id = list.FirstOrDefault().AMCO_Id;
                    data.AMB_Id = list.FirstOrDefault().AMB_Id;
                    data.AMSE_Id = list.FirstOrDefault().AMSE_Id;
                    data.ACMS_Id = list.FirstOrDefault().ACMS_Id;
                    AMCST_Id = data.AMCST_Id.ToString();
                    data.FlagReport = "btwdates";
                }
                else
                {
                    if (data.section_Array != null && data.section_Array.Length > 0)
                    {
                        foreach (var d in data.section_Array)
                        {
                            AMCST_Id = AMCST_Id + "," + d.AMCST_Id;
                        }
                    }
                }

                if (data.FlagReport == "btwdates")
                {
                    using (var cmd = _examctxt.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Exam_TimeTable_Generation";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                SqlDbType.VarChar)
                        {
                            Value = "2"
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
              SqlDbType.BigInt)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                SqlDbType.BigInt)
                        {
                            Value = data.AMCO_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMB_Id",
               SqlDbType.BigInt)
                        {
                            Value = data.AMB_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
             SqlDbType.BigInt)
                        {
                            Value = data.AMSE_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ACMS_Id",
            SqlDbType.BigInt)
                        {
                            Value = data.ACMS_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@EME_Id",
           SqlDbType.BigInt)
                        {
                            Value = data.EME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
          SqlDbType.VarChar)
                        {
                            Value = AMCST_Id
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
                            data.GetStudent = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                }
                else
                {
                    using (var cmd = _examctxt.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Exam_HallTicket_Generation_DATA";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Flag",
                SqlDbType.VarChar)
                        {
                            Value = "3"
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
              SqlDbType.BigInt)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                SqlDbType.BigInt)
                        {
                            Value = data.AMCO_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMB_Id",
               SqlDbType.BigInt)
                        {
                            Value = data.AMB_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
             SqlDbType.BigInt)
                        {
                            Value = data.AMSE_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ACMS_Id",
            SqlDbType.BigInt)
                        {
                            Value = data.ACMS_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@EME_Id",
           SqlDbType.BigInt)
                        {
                            Value = data.EME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
          SqlDbType.VarChar)
                        {
                            Value = AMCST_Id
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
                            data.GetStudent = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                }


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
        //HalticketSubject
        public HallTicketGenerationCollegeDTO HalticketSubject(HallTicketGenerationCollegeDTO data)
        {
            try
            {
                var ECYS_IdS = _examctxt.Exm_Col_Yearly_SchemeDMO.Where(R => R.MI_Id == data.MI_Id && R.AMCO_Id == data.AMCO_Id
                  && R.AMB_Id == data.AMB_Id && R.AMSE_Id == data.AMSE_Id && R.ACSS_Id == data.ACSS_Id && R.ACST_Id == data.ACST_Id && R.ECYS_ActiveFlag == true).ToList();

                var ECYSE_IdS = _examctxt.Exm_Col_Yearly_Scheme_ExamsDMO.Where(R => R.AMCO_Id == data.AMCO_Id
               && R.AMB_Id == data.AMB_Id && R.AMSE_Id == data.AMSE_Id && R.ACSS_Id == data.ACSS_Id && R.ACST_Id == data.ACST_Id
               && R.ECYSE_ActiveFlg == true && R.ECYS_Id == ECYS_IdS.FirstOrDefault().ECYS_Id).ToList();

                //data.view_exam_subjects = (from a in _examctxt.Exm_Col_Yrly_Sch_Exams_SubwiseDMO
                //                           from c in _examctxt.col_exammasterDMO
                //                           from e in _examctxt.Exm_Col_Yearly_Scheme_ExamsDMO
                //                           from f in _examctxt.Exm_Master_GradeDMO
                //                           from g in _examctxt.IVRM_School_Master_SubjectsDMO
                //                           where (a.ECYSE_Id == e.ECYSE_Id && a.ISMS_Id == g.ISMS_Id && a.EMGR_Id == f.EMGR_Id && a.ECYSE_Id == ECYSE_IdS.FirstOrDefault().ECYSE_Id && e.EME_Id == c.EME_Id)
                //                           select new ClgSubjectWizardDTO
                //                           {
                //                               ECYSES_Id = a.ECYSES_Id,
                //                               ECYSE_Id = a.ECYSE_Id,
                //                               ISMS_Id = a.ISMS_Id,
                //                               EME_ExamName = c.EME_ExamName,
                //                               EMGR_GradeName = f.EMGR_GradeName,
                //                               ISMS_SubjectName = g.ISMS_SubjectName,
                //                               ISMS_SubjectCode = g.ISMS_SubjectCode,
                //                               ECYSES_MaxMarks = a.ECYSES_MaxMarks,
                //                               ECYSES_MinMarks = a.ECYSES_MinMarks,
                //                               ECYSES_MarksEntryMax = a.ECYSES_MarksEntryMax,
                //                               ECYSES_SubExamFlg = a.ECYSES_SubExamFlg,
                //                               ECYSES_SubSubjectFlg = a.ECYSES_SubSubjectFlg,
                //                               ECYSES_MarksGradeEntryFlg = a.ECYSES_MarksGradeEntryFlg,
                //                               ECYSES_MarksDisplayFlg = a.ECYSES_MarksDisplayFlg,
                //                               ECYSES_GradeDisplayFlg = a.ECYSES_GradeDisplayFlg,
                //                               ECYSES_AplResultFlg = a.ECYSES_AplResultFlg,
                //                               ECYSES_SubjectOrder = a.ECYSES_SubjectOrder,
                //                               ECYSES_ActiveFlg = a.ECYSES_ActiveFlg
                //                           }).Distinct().OrderBy(x => x.ECYSES_SubjectOrder).ToArray();

                data.view_exam_subjects = (from s in _examctxt.Exm_Col_Yearly_SchemeDMO
                                           from k in _examctxt.Exm_Col_Yearly_Scheme_GroupDMO
                                           from l in _examctxt.Exm_Col_Yearly_Scheme_Group_SubjectsDMO
                                           from j in _examctxt.IVRM_Master_SubjectsDMO
                                           where (s.ECYS_Id == k.ECYS_Id && k.ECYSG_Id == l.ECYSG_Id && l.ISMS_Id == j.ISMS_Id
                                           && s.AMCO_Id == data.AMCO_Id && s.AMB_Id == data.AMB_Id && s.AMSE_Id == data.AMSE_Id && s.ACSS_Id == data.ACSS_Id
                                           && s.ACST_Id == data.ACST_Id && s.MI_Id == data.MI_Id && s.ECYS_ActiveFlag == true && k.ECYSG_ActiveFlag == true
                                           && l.ECYSGS_ActiveFlag == true)
                                           select j).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();
                data.time_slot = _examctxt.Exm_TT_M_SessionDMO.Where(t => t.MI_Id == data.MI_Id && t.ETTS_Activeflag == true).ToArray();

                //data.OnEditSubjects = _examctxt.Exm_TimeTable_College_SubjectsDMO.Where(R => R.EXTTC_Id == data.EXTTC_Id).Distinct().ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        //savedetailHalticket
        public HallTicketGenerationCollegeDTO savedetailHalticket(HallTicketGenerationCollegeDTO data)
        {


            try
            {
                if (data.EXTTC_Id > 0)
                {
                    var res = _examctxt.Exm_TimeTable_CollegeDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id
                    && t.AMCO_Id == data.AMCO_Id && t.AMB_Id == data.AMB_Id && t.EME_Id == data.EME_Id
                    && t.AMSE_Id == data.AMSE_Id && t.EXTTC_Id != data.EXTTC_Id).ToList();

                    if (res.Count() > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var objpge1 = _examctxt.Exm_TimeTable_CollegeDMO.Single(t => t.EXTTC_Id == data.EXTTC_Id);
                        objpge1.MI_Id = data.MI_Id;
                        objpge1.ASMAY_Id = data.ASMAY_Id;
                        objpge1.AMCO_Id = data.AMCO_Id;
                        objpge1.AMB_Id = data.AMB_Id;
                        objpge1.AMSE_Id = data.AMSE_Id;
                        objpge1.ACST_Id = data.ACST_Id;
                        objpge1.ACSS_Id = data.ACSS_Id;
                        objpge1.EME_Id = data.EME_Id;
                        objpge1.EXTTC_ActiveFlag = true;
                        objpge1.EXTTC_FromDateTime = data.EXTTC_FromDateTime;
                        objpge1.EXTTC_ToDateTime = data.EXTTC_ToDateTime;
                        //objpge1.EMG_Id = data.EMG_Id;
                        objpge1.EXTTC_UpdatedDate = DateTime.Now;
                        _examctxt.Update(objpge1);
                        var contactExists = _examctxt.SaveChanges();
                        var result = data.EXTTC_Id;
                        if (contactExists > 0)
                        {
                            if (data.TempararyArrayList != null && data.TempararyArrayList.Length > 0)
                            {
                                var listCount = _examctxt.Exm_TimeTable_College_SubjectsDMO.Where(R => R.EXTTC_Id == data.EXTTC_Id).ToList();
                                if (listCount.Count > 0)
                                {
                                    foreach (var d in listCount)
                                    {
                                        _examctxt.Remove(d);
                                    }
                                }

                                for (int i = 0; i < data.TempararyArrayList.Count(); i++)
                                {
                                    Exm_TimeTable_College_SubjectsDMO objpge2 = new Exm_TimeTable_College_SubjectsDMO();
                                    objpge2.EXTTC_Id = data.EXTTC_Id;
                                    objpge2.ISMS_Id = data.TempararyArrayList[i].ISMS_Id;
                                    objpge2.EMTTSC_Id = data.TempararyArrayList[i].EMTTSC_Id;
                                    objpge2.EXTTSC_Date = data.TempararyArrayList[i].EXTTSC_Date;
                                    objpge2.EXTTC_ExaminationCenter = data.TempararyArrayList[i].EXTTC_ExaminationCenter;
                                    objpge2.EXTTC_ReportingTime = data.TempararyArrayList[i].EXTTC_ReportingTime;
                                    objpge2.EXTTCS_CreatedDate = DateTime.Now;
                                    objpge2.EXTTCS_UpdatedDate = DateTime.Now;
                                    objpge2.EXTTSC_ActiveFlag = true;
                                    _examctxt.Add(objpge2);
                                }

                                var contactExists1 = _examctxt.SaveChanges();
                                if (contactExists1 > 0)
                                {
                                    data.returnval = true;
                                }
                                else
                                {
                                    data.returnval = false;
                                }
                            }

                        }
                    }
                }
                else
                {
                    var Duplicate = _examctxt.Exm_TimeTable_CollegeDMO.Where(R => R.MI_Id == data.MI_Id
                    && R.ASMAY_Id == data.ASMAY_Id && R.AMCO_Id == data.AMCO_Id && R.AMB_Id == data.AMB_Id && R.AMSE_Id == data.AMSE_Id && R.EME_Id == data.EME_Id).ToList();
                    if (Duplicate.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        Exm_TimeTable_CollegeDMO objpge = new Exm_TimeTable_CollegeDMO();
                        objpge.MI_Id = data.MI_Id;
                        objpge.ASMAY_Id = data.ASMAY_Id;
                        objpge.AMCO_Id = data.AMCO_Id;
                        objpge.AMB_Id = data.AMB_Id;
                        objpge.EME_Id = data.EME_Id;
                        objpge.AMSE_Id = data.AMSE_Id;
                        objpge.ACST_Id = data.ACST_Id;
                        objpge.ACSS_Id = data.ACSS_Id;
                        objpge.EXTTC_ActiveFlag = true;
                        objpge.EXTTC_FromDateTime = data.EXTTC_FromDateTime;
                        objpge.EXTTC_ToDateTime = data.EXTTC_ToDateTime;
                        objpge.EXTTC_CreatedDate = DateTime.Now;
                        objpge.EXTTC_UpdatedDate = DateTime.Now;
                        _examctxt.Add(objpge);

                        for (int i = 0; i < data.TempararyArrayList.Count(); i++)
                        {
                            Exm_TimeTable_College_SubjectsDMO objpge1 = new Exm_TimeTable_College_SubjectsDMO();
                            objpge1.EXTTC_Id = objpge.EXTTC_Id;
                            objpge1.ISMS_Id = data.TempararyArrayList[i].ISMS_Id;
                            objpge1.EMTTSC_Id = data.TempararyArrayList[i].EMTTSC_Id;
                            objpge1.EXTTSC_Date = data.TempararyArrayList[i].EXTTSC_Date;
                            objpge1.EXTTC_ExaminationCenter = data.TempararyArrayList[i].EXTTC_ExaminationCenter;
                            objpge1.EXTTC_ReportingTime = data.TempararyArrayList[i].EXTTC_ReportingTime;
                            objpge1.EXTTCS_CreatedDate = DateTime.Now;
                            objpge1.EXTTCS_UpdatedDate = DateTime.Now;
                            objpge1.EXTTSC_ActiveFlag = true;
                            _examctxt.Add(objpge1);
                        }

                        var contactExists1 = _examctxt.SaveChanges();
                        if (contactExists1 > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        //onedit
        public HallTicketGenerationCollegeDTO onedit(HallTicketGenerationCollegeDTO data)
        {


            try
            {
                if (data.EXTTC_Id > 0)
                {
                    var ECYS_IdS = _examctxt.Exm_Col_Yearly_SchemeDMO.Where(R => R.MI_Id == data.MI_Id && R.AMCO_Id == data.AMCO_Id
                 && R.AMB_Id == data.AMB_Id && R.AMSE_Id == data.AMSE_Id && R.ACSS_Id == data.ACSS_Id && R.ACST_Id == data.ACST_Id && R.ECYS_ActiveFlag == true).ToList();

                    var ECYSE_IdS = _examctxt.Exm_Col_Yearly_Scheme_ExamsDMO.Where(R => R.AMCO_Id == data.AMCO_Id
                   && R.AMB_Id == data.AMB_Id && R.AMSE_Id == data.AMSE_Id && R.ACSS_Id == data.ACSS_Id && R.ACST_Id == data.ACST_Id
                   && R.ECYSE_ActiveFlg == true && R.ECYS_Id == ECYS_IdS.FirstOrDefault().ECYS_Id).ToList();


                    data.view_exam_subjects = (from s in _examctxt.Exm_Col_Yearly_SchemeDMO
                                               from k in _examctxt.Exm_Col_Yearly_Scheme_GroupDMO
                                               from l in _examctxt.Exm_Col_Yearly_Scheme_Group_SubjectsDMO
                                               from j in _examctxt.IVRM_Master_SubjectsDMO
                                               where (s.ECYS_Id == k.ECYS_Id && k.ECYSG_Id == l.ECYSG_Id && l.ISMS_Id == j.ISMS_Id
                                               && s.AMCO_Id == data.AMCO_Id && s.AMB_Id == data.AMB_Id && s.AMSE_Id == data.AMSE_Id && s.ACSS_Id == data.ACSS_Id
                                               && s.ACST_Id == data.ACST_Id && s.MI_Id == data.MI_Id && s.ECYS_ActiveFlag == true && k.ECYSG_ActiveFlag == true
                                               && l.ECYSGS_ActiveFlag == true)
                                               select j).Distinct().OrderBy(a => a.ISMS_OrderFlag).ToArray();
                    data.time_slot = _examctxt.Exm_TT_M_SessionDMO.Where(t => t.MI_Id == data.MI_Id && t.ETTS_Activeflag == true).ToArray();
          
                    data.OnEditSubjects = _examctxt.Exm_TimeTable_College_SubjectsDMO.Where(R => R.EXTTC_Id == data.EXTTC_Id).Distinct().ToArray();

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        //ondelete 
        public HallTicketGenerationCollegeDTO ondelete(HallTicketGenerationCollegeDTO data)
        {


            try
            {
                if (data.EXTTC_Id > 0)
                {

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
