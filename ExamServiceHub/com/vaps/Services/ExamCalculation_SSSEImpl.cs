
using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.Exam;
using AutoMapper;
using PreadmissionDTOs.com.vaps.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.com.vaps.admission;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using DomainModel.Model.com.vapstech.Exam;
using CommonLibrary;
using System.Dynamic;

namespace ExamServiceHub.com.vaps.Services
{
    public class ExamCalculation_SSSEImpl : Interfaces.ExamCalculation_SSSEInterface
    {
        private static ConcurrentDictionary<string, ExamCalculation_SSSEDTO> _login =
         new ConcurrentDictionary<string, ExamCalculation_SSSEDTO>();

        public DomainModelMsSqlServerContext _db;
        public ExamContext _CumulativeReportContext;
        ILogger<ExamCalculation_SSSEImpl> _acdimpl;
        public ExamCalculation_SSSEImpl(ExamContext cpContext, ILogger<ExamCalculation_SSSEImpl> acdimpl, DomainModelMsSqlServerContext db)
        {
            _CumulativeReportContext = cpContext;
            _acdimpl = acdimpl;
            _db = db;
        }
        public ExamCalculation_SSSEDTO Getdetails(ExamCalculation_SSSEDTO data)//int IVRMM_Id
        {
            ExamCalculation_SSSEDTO getdata = new ExamCalculation_SSSEDTO();
            try
            {
                var empcode_check = (from a in _CumulativeReportContext.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.Id.Equals(data.User_Id))
                                     select new ExamCalculation_SSSEDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                List<MasterAcademic> list = new List<MasterAcademic>();
                list = _CumulativeReportContext.AcademicYear.AsNoTracking().Where(t => t.MI_Id == data.MI_Id 
                && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                getdata.yearlist = list.ToArray();

                List<exammasterDMO> esmp = new List<exammasterDMO>();
                esmp = _CumulativeReportContext.exammasterDMO.AsNoTracking().Where(t => t.MI_Id == data.MI_Id 
                && t.EME_ActiveFlag == true).OrderBy(t => t.EME_ExamOrder).ToList();
                getdata.exmstdlist = esmp.ToArray();

                getdata.Feetermlist = _CumulativeReportContext.FeeTermDMO.Where(a => a.MI_Id == data.MI_Id 
                && a.FMT_ActiveFlag == true).Distinct().OrderBy(a => a.FMT_Order).ToArray();

                getdata.Feegrouplist = _CumulativeReportContext.FeeGroupDMO.Where(a => a.MI_Id == data.MI_Id 
                && a.FMG_ActiceFlag == true).Distinct().OrderBy(a=>a.FMG_Order).ToArray();

            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return getdata;

        }
        public ExamCalculation_SSSEDTO get_classes(ExamCalculation_SSSEDTO data)
        {
            try
            {
                var empcode_check = (from a in _CumulativeReportContext.Staff_User_Login
                                     where (a.MI_Id == data.MI_Id && a.Id.Equals(data.User_Id))
                                     select new ExamCalculation_SSSEDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                if (empcode_check.Count() > 0)
                {
                    var check_classteacher = _CumulativeReportContext.ClassTeacherMappingDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.IMCT_ActiveFlag == true && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code).ToList();

                    if (check_classteacher.Count() > 0)
                    {

                        data.classlist = (from a in _CumulativeReportContext.ClassTeacherMappingDMO
                                          from b in _CumulativeReportContext.AdmissionClass
                                          from c in _CumulativeReportContext.AcademicYear
                                          where (a.ASMCL_Id == b.ASMCL_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id
                                          && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && a.IMCT_ActiveFlag == true && b.ASMCL_ActiveFlag == true)
                                          select new ExamCalculation_SSSEDTO
                                          {
                                              ASMCL_Id = b.ASMCL_Id,
                                              ASMCL_ClassName = b.ASMCL_ClassName,
                                              ASMCL_ClassOrder = b.ASMCL_Order
                                          }).Distinct().OrderBy(a => a.ASMCL_ClassOrder).ToArray();

                    }
                }
                else
                {
                    data.classlist = (from a in _CumulativeReportContext.Exm_Category_ClassDMO
                                      from b in _CumulativeReportContext.AdmissionClass
                                      where (a.ASMCL_Id == b.ASMCL_Id && a.ECAC_ActiveFlag == true && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                                      && b.ASMCL_ActiveFlag == true)
                                      select b).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public ExamCalculation_SSSEDTO get_cls_sections(ExamCalculation_SSSEDTO id)
        {
            try
            {
                var getuserid = _CumulativeReportContext.ApplUser.Where(a => a.UserName.Equals(id.UserName.Trim())).Select(a => a.Id);

                var empcode_check = (from a in _CumulativeReportContext.Staff_User_Login
                                     where (a.MI_Id == id.MI_Id && a.Id.Equals(id.User_Id))
                                     select new ExamCalculation_SSSEDTO
                                     {
                                         Emp_Code = a.Emp_Code,
                                     }).ToList();

                if (empcode_check.Count() > 0)
                {
                    var check_classteacher = _CumulativeReportContext.ClassTeacherMappingDMO.Where(a => a.MI_Id == id.MI_Id && a.ASMAY_Id == id.ASMAY_Id && a.IMCT_ActiveFlag == true && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code).ToList();

                    if (check_classteacher.Count() > 0)
                    {

                        id.seclist = (from a in _CumulativeReportContext.ClassTeacherMappingDMO
                                      from b in _CumulativeReportContext.School_M_Section
                                      from c in _CumulativeReportContext.AcademicYear
                                      from d in _CumulativeReportContext.AdmissionClass
                                      where (a.ASMCL_Id == d.ASMCL_Id && a.ASMS_Id == b.ASMS_Id && a.ASMAY_Id == c.ASMAY_Id && a.ASMAY_Id == id.ASMAY_Id
                                      && a.ASMCL_Id == id.ASMCL_Id && a.HRME_Id == empcode_check.FirstOrDefault().Emp_Code && a.IMCT_ActiveFlag == true && b.ASMC_ActiveFlag == 1)
                                      select new ExamCalculation_SSSEDTO
                                      {
                                          ASMS_Id = b.ASMS_Id,
                                          ASMC_SectionName = b.ASMC_SectionName,
                                          ASMC_SectionOrder = b.ASMC_Order
                                      }).Distinct().OrderBy(a => a.ASMC_SectionOrder).ToArray();

                    }
                }
                else
                {
                    id.seclist = (from a in _CumulativeReportContext.Exm_Category_ClassDMO
                                  from b in _CumulativeReportContext.AdmissionClass
                                  from c in _CumulativeReportContext.School_M_Section
                                  where (a.ASMCL_Id == b.ASMCL_Id && a.ASMS_Id == c.ASMS_Id && a.ECAC_ActiveFlag == true && a.MI_Id == id.MI_Id
                                  && a.ASMAY_Id == id.ASMAY_Id && b.ASMCL_ActiveFlag == true && c.ASMC_ActiveFlag == 1 && a.ASMCL_Id == id.ASMCL_Id)
                                  select c).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return id;
        }
        public ExamCalculation_SSSEDTO get_exams(ExamCalculation_SSSEDTO data)
        {
            //ExamMarksDTO data = new ExamMarksDTO();
            try
            {
                var catid = _CumulativeReportContext.Exm_Category_ClassDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.MI_Id == data.MI_Id && t.ECAC_ActiveFlag == true).Select(t => t.EMCA_Id).ToArray();

                var eycid = _CumulativeReportContext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && catid.Contains(t.EMCA_Id) && t.EYC_ActiveFlg == true).Select(t => t.EYC_Id).ToArray();

                var emeid = _CumulativeReportContext.Exm_Yearly_Category_ExamsDMO.Where(t => eycid.Contains(t.EYC_Id) && t.EYCE_ActiveFlg == true).Select(t => t.EME_Id).ToArray();

                List<exammasterDMO> examlist = new List<exammasterDMO>();
                examlist = _CumulativeReportContext.exammasterDMO.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true && emeid.Contains(t.EME_Id)).ToList();
                data.exmstdlist = examlist.Distinct().ToArray();

                List<int?> ids = new List<int?>();
                if (data.Left_Flag == true || data.Deactive_Flag == true)
                {
                    ids.Add(0);
                }

                ids.Add(1);

                List<string> sol = new List<string>();
                sol.Add("S");

                if (data.Left_Flag == true)
                {
                    sol.Add("L");
                }
                if (data.Deactive_Flag == true)
                {
                    sol.Add("D");
                }

                data.studentlist = (from a in _CumulativeReportContext.Adm_M_Student
                                    from b in _CumulativeReportContext.School_Adm_Y_Student
                                    where (a.AMST_Id == b.AMST_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && b.ASMAY_Id == data.ASMAY_Id
                                    && a.MI_Id == data.MI_Id && sol.Contains(a.AMST_SOL) && ids.Contains(a.AMST_ActiveFlag) && ids.Contains(b.AMAY_ActiveFlag))
                                    select new ProgressCardReportDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) +
                                        (a.AMST_MiddleName == null || a.AMST_MiddleName == "" ? "" : " " + a.AMST_MiddleName) +
                                        (a.AMST_LastName == null || a.AMST_LastName == "" ? "" : " " + a.AMST_LastName) +
                                        (a.AMST_AdmNo == null || a.AMST_AdmNo == "" ? "" : " : " + a.AMST_AdmNo)).Trim()
                                    }).Distinct().OrderBy(a => a.AMST_FirstName).ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public ExamCalculation_SSSEDTO onchangeexam(ExamCalculation_SSSEDTO data)
        {
            //ExamMarksDTO data = new ExamMarksDTO();
            try
            {
                data.editlist = _CumulativeReportContext.ExamMarksDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id
                && a.ASMS_Id == data.ASMS_Id && a.EME_Id == data.EME_Id).ToArray();

                var getemcaid = _CumulativeReportContext.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).ToList();

                var geteycid = _CumulativeReportContext.Exm_Yearly_CategoryDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.EYC_ActiveFlg == true && getemcaid.Contains(a.EMCA_Id)).Select(a => a.EYC_Id).ToList();

                var checkprocessdateisnull = _CumulativeReportContext.Exm_Yearly_Category_ExamsDMO.Where(a => a.EME_Id == data.EME_Id && a.EYCE_ActiveFlg == true && geteycid.Contains(a.EYC_Id)).Distinct().ToArray();

                data.savelist = checkprocessdateisnull;

                //if (checkprocessdateisnull.FirstOrDefault().EYCE_MarksProcessLastDate != null)
                //{
                //    var checklastdateforprocess = _CumulativeReportContext.Exm_Yearly_Category_ExamsDMO.Where(a => a.EME_Id == data.EME_Id
                //    && geteycid.Contains(a.EYC_Id) && Convert.ToDateTime(System.DateTime.Today.Date) <= Convert.ToDateTime(a.EYCE_MarksProcessLastDate)).ToList();

                //    data.lastdateprocess = checklastdateforprocess.Count();

                //    var checklastdatepublishmarksdate = _CumulativeReportContext.Exm_Yearly_Category_ExamsDMO.Where(a => a.EME_Id == data.EME_Id
                //   && geteycid.Contains(a.EYC_Id) && Convert.ToDateTime(System.DateTime.Today.Date) >= Convert.ToDateTime(a.EYCE_MarksPublishDate)).ToList();

                //    data.publishmarksdate = checklastdatepublishmarksdate.Count();
                //}
                //else
                //{
                //    data.lastdateprocess = 10000;
                //    data.publishmarksdate = 10000;
                //}

                //var checkpublishedornot = _CumulativeReportContext.ExmStudentMarksProcessDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_Id == data.EME_Id).ToList();

                //if (checkpublishedornot.Count() > 0)
                //{
                //    data.retuvrnvalflag = true;
                //}
                //else
                //{
                //    data.retuvrnvalflag = false;
                //}

                //if (checkprocessdateisnull.FirstOrDefault().EYCE_MarksPublishDate == null)
                //{
                //    var checkpublishedornotpublish = _CumulativeReportContext.ExmStudentMarksProcessDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_Id == data.EME_Id && a.ESTMP_PublishToStudentFlg == true).ToList();

                //    if (checkpublishedornotpublish.Count() > 0)
                //    {
                //        data.retuvrnvalpublishflag = true;
                //    }
                //    else
                //    {
                //        data.retuvrnvalpublishflag = false;
                //    }
                //}

                // CHECKING FOR PUBLISH MARKS 

                if (checkprocessdateisnull.FirstOrDefault().EYCE_MarksPublishDate == null)
                {
                    // CHECKING WHETHER MARKS ARE CALCULATED OR NOT FOR SELECTED DETAILS

                    var checkprocess = _CumulativeReportContext.ExmStudentMarksProcessDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_Id == data.EME_Id).ToList();

                    if (checkprocess.Count() > 0)
                    {
                        data.publishbtn = true;
                        data.retuvrnvalflag = true;
                    }
                    else
                    {
                        data.publishbtn = false;
                        data.retuvrnvalflag = false;
                    }

                    // CHECKING WHETHER MARKS ARE PUBLISHED (ESTMP_PublishToStudentFlg==true) OR NOT FOR SELECTED DETAILS

                    var checkprocesspublish = _CumulativeReportContext.ExmStudentMarksProcessDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                    && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_Id == data.EME_Id && a.ESTMP_PublishToStudentFlg == true).ToList();

                    if (checkprocesspublish.Count() > 0)
                    {
                        data.retuvrnvalpublishflag = true;
                        data.markscalculatedflag = true;
                    }
                    else
                    {
                        data.retuvrnvalpublishflag = false;
                        data.markscalculatedflag = false;
                    }
                }
                else
                {
                    var checklastdatepublishmarksdate = _CumulativeReportContext.Exm_Yearly_Category_ExamsDMO.Where(a => a.EME_Id == data.EME_Id
                    && geteycid.Contains(a.EYC_Id) && Convert.ToDateTime(System.DateTime.Today.Date) >= Convert.ToDateTime(a.EYCE_MarksPublishDate)).ToList();

                    if (checklastdatepublishmarksdate.Count > 0)
                    {
                        data.publishbtn = true;
                        data.publishmarksdate = checklastdatepublishmarksdate.Count();

                        var checkprocesspublish = _CumulativeReportContext.ExmStudentMarksProcessDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                        && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_Id == data.EME_Id).ToList();

                        if (checkprocesspublish.Count() > 0)
                        {
                            data.retuvrnvalpublishflag = true;
                            data.markscalculatedflag = true;
                        }
                        else
                        {
                            data.retuvrnvalpublishflag = false;
                            data.markscalculatedflag = false;
                        }
                    }
                    else
                    {
                        data.publishbtn = false;
                        data.publishmarksdate = 0;
                    }
                }

                if (checkprocessdateisnull.FirstOrDefault().EYCE_MarksProcessLastDate != null)
                {
                    var checklastdateforprocess = _CumulativeReportContext.Exm_Yearly_Category_ExamsDMO.Where(a => a.EME_Id == data.EME_Id
                    && geteycid.Contains(a.EYC_Id) && Convert.ToDateTime(System.DateTime.Today.Date) <= Convert.ToDateTime(a.EYCE_MarksProcessLastDate)).ToList();

                    if (checklastdateforprocess.Count() > 0)
                    {
                        data.lastdateprocess = checklastdateforprocess.Count();
                        data.markslastdateflag = true;
                    }
                    else
                    {
                        data.markslastdateflag = false;
                    }
                }
                using (var cmd = _CumulativeReportContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exm_Marks_Entry_Marks_Popup";
                    cmd.CommandTimeout = 1000000;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar)
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
                        data.datareport = retObject.ToArray();
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
        public ExamCalculation_SSSEDTO Calculation(ExamCalculation_SSSEDTO exm)
        {
            try
            {
                using (var cmd = _CumulativeReportContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "IndSubjects_SubsExmMarksCalculation";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 5000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.BigInt)
                    {
                        Value = exm.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                     SqlDbType.BigInt)
                    {
                        Value = exm.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                      SqlDbType.BigInt)
                    {
                        Value = exm.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                      SqlDbType.BigInt)
                    {
                        Value = exm.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
                  SqlDbType.Int)
                    {
                        Value = exm.EME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ESTMP_PublishToStudentFlg",
                 SqlDbType.Int)
                    {
                        Value = exm.ESTMP_PublishToStudentFlg
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        var a = cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogInformation("Exam Calculation New 1:" + exm.MI_Id + " " + ex.Message + "");
                        Console.WriteLine(ex.Message);
                    }
                    exm.returnval = true;
                }

                TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                Exm_Calculation_LogDMO dmo = new Exm_Calculation_LogDMO();

                dmo.MI_Id = exm.MI_Id;
                dmo.ASMAY_Id = exm.ASMAY_Id;
                dmo.ASMCL_Id = exm.ASMCL_Id;
                dmo.ASMS_Id = exm.ASMS_Id;
                dmo.EME_Id = exm.EME_Id;
                dmo.IVRMUL_Id = exm.User_Id;
                dmo.ECL_PublishFlag = 0;
                dmo.CreatedDate = indiantime0;
                dmo.UpdatedDate = indiantime0;

                _CumulativeReportContext.Add(dmo);
                try
                {
                    var i = _CumulativeReportContext.SaveChanges();
                    if (i > 0)
                    {
                        _acdimpl.LogInformation("Exam Calculation New Insert Into Log Success");
                        exm.returnval = true;
                    }
                    else
                    {
                        _acdimpl.LogInformation("Exam Calculation New Insert Into Log Failed");
                        exm.returnval = true;
                    }
                }
                catch (Exception ex)
                {
                    _acdimpl.LogInformation("Exam Calculation New Insert :" + exm.MI_Id + " " + ex.Message + "");
                    Console.WriteLine(ex.Message);
                    exm.returnval = true;
                }
            }
            catch (Exception ee)
            {
                _acdimpl.LogInformation("Exam Calculation New 2:" + exm.MI_Id + " " + ee.Message + "");
                Console.WriteLine(ee.Message);
                exm.returnval = false;
            }

            return exm;
        }
        public ExamCalculation_SSSEDTO saveapporvecal(ExamCalculation_SSSEDTO data)
        {
            //ExamMarksDTO data = new ExamMarksDTO();
            try
            {
                try
                {
                    var outputval = _CumulativeReportContext.Database.ExecuteSqlCommand("Exam_Marks_Approval_Process_To_Publis_Student_Portal @p0,@p1,@p2,@p3,@p4",
                   data.MI_Id, data.ASMAY_Id, data.ASMCL_Id, data.ASMS_Id, data.EME_Id);

                    if (outputval >= 1)
                    {
                        data.returnval = true;

                        TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                        DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                        Exm_Calculation_LogDMO dmo = new Exm_Calculation_LogDMO();

                        dmo.MI_Id = data.MI_Id;
                        dmo.ASMAY_Id = data.ASMAY_Id;
                        dmo.ASMCL_Id = data.ASMCL_Id;
                        dmo.ASMS_Id = data.ASMS_Id;
                        dmo.EME_Id = data.EME_Id;
                        dmo.IVRMUL_Id = data.User_Id;
                        dmo.ECL_PublishFlag = 1;
                        dmo.CreatedDate = indiantime0;
                        dmo.UpdatedDate = indiantime0;
                        _CumulativeReportContext.Add(dmo);

                        try
                        {
                            var i = _CumulativeReportContext.SaveChanges();
                            if (i > 0)
                            {
                                _acdimpl.LogInformation("Exam Calculation New Insert Publish Into Log Success");
                                data.returnval = true;
                            }
                            else
                            {
                                _acdimpl.LogInformation("Exam Calculation New Insert Publish Into Log Failed");
                                data.returnval = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            _acdimpl.LogInformation("Exam Calculation New Insert :" + data.MI_Id + " " + ex.Message + "");
                            Console.WriteLine(ex.Message);
                            data.returnval = true;
                        }
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
                catch (Exception ee)
                {
                    _acdimpl.LogInformation("Exam Calculation New 2:" + data.MI_Id + " " + ee.Message + "");
                    Console.WriteLine(ee.Message);
                    data.returnval = false;
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        // Student Wise Publish 
        public ExamCalculation_SSSEDTO ChangeOfSection(ExamCalculation_SSSEDTO data)
        {
            //ExamMarksDTO data = new ExamMarksDTO();
            try
            {
                var catid = _CumulativeReportContext.Exm_Category_ClassDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id && t.MI_Id == data.MI_Id && t.ECAC_ActiveFlag == true).Select(t => t.EMCA_Id).ToArray();

                var eycid = _CumulativeReportContext.Exm_Yearly_CategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && catid.Contains(t.EMCA_Id) && t.EYC_ActiveFlg == true).Select(t => t.EYC_Id).ToArray();

                if (data.radiotype == "Exam")
                {
                    var emeid = _CumulativeReportContext.Exm_Yearly_Category_ExamsDMO.Where(t => eycid.Contains(t.EYC_Id) && t.EYCE_ActiveFlg == true).Select(t => t.EME_Id).ToArray();

                    List<exammasterDMO> examlist = new List<exammasterDMO>();
                    examlist = _CumulativeReportContext.exammasterDMO.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true && emeid.Contains(t.EME_Id)).ToList();
                    data.exmstdlist = examlist.Distinct().ToArray();
                }
                else
                {
                    data.editlist = _CumulativeReportContext.Exm_Student_MP_PromotionDMO.Where(a => a.MI_Id == data.MI_Id
                    && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id).ToArray();
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public ExamCalculation_SSSEDTO CheckMarksCalculated(ExamCalculation_SSSEDTO data)
        {
            try
            {
                data.editlist = _CumulativeReportContext.ExmStudentMarksProcessDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.EME_Id == data.EME_Id).ToArray();

                var getemcaid = _CumulativeReportContext.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).Select(a => a.EMCA_Id).ToList();

                var geteycid = _CumulativeReportContext.Exm_Yearly_CategoryDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.EYC_ActiveFlg == true && getemcaid.Contains(a.EMCA_Id)).Select(a => a.EYC_Id).ToList();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public ExamCalculation_SSSEDTO SearchStudent(ExamCalculation_SSSEDTO data)
        {
            try
            {
                string FMGIDS = "0";

                if(data.TempGroupList!=null && data.TempGroupList.Length > 0)
                {
                    FMGIDS = string.Join(",", data.TempGroupList);
                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Exam_Get_StudentList_PublishMarks_Manual";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar){Value = data.MI_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar){Value = data.ASMAY_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar){Value = data.ASMCL_Id});
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar){Value = data.ASMS_Id});
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar){Value = data.EME_Id});
                    cmd.Parameters.Add(new SqlParameter("@radiotype", SqlDbType.VarChar){Value = data.radiotype });
                    cmd.Parameters.Add(new SqlParameter("@feeinstallmentcheckbox", SqlDbType.VarChar){Value = data.feeinstallmentcheckbox==true ? "1" : "0" });
                    cmd.Parameters.Add(new SqlParameter("@FMT_Id", SqlDbType.VarChar) { Value = data.FMT_Id });
                    cmd.Parameters.Add(new SqlParameter("@FMGIDS", SqlDbType.VarChar) { Value = FMGIDS });
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
                        data.datareport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }                
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public ExamCalculation_SSSEDTO SaveStudentStatus(ExamCalculation_SSSEDTO data)
        {
            try
            {
                if (data.radiotype == "Exam")
                {
                    foreach (var c in data.selectedstudentsmarks)
                    {
                        var checkresult = _CumulativeReportContext.ExmStudentMarksProcessDMO.Where(a => a.AMST_Id == c.AMST_Id && a.ESTMP_Id == c.ESTMP_Id).ToList();

                        if (checkresult.Count > 0)
                        {
                            var result = _CumulativeReportContext.ExmStudentMarksProcessDMO.Single(a => a.AMST_Id == c.AMST_Id && a.ESTMP_Id == c.ESTMP_Id);
                            result.ESTMP_PublishToStudentFlg = result.ESTMP_PublishToStudentFlg == true ? false : true;
                            result.UpdatedDate = DateTime.Now;
                            _CumulativeReportContext.Update(result);
                        }
                    }
                }
                else
                {
                    foreach (var c in data.selectedstudentsmarks)
                    {
                        var checkresult = _CumulativeReportContext.Exm_Student_MP_PromotionDMO.Where(a => a.AMST_Id == c.AMST_Id && a.ESTMPP_Id == c.ESTMP_Id).ToList();

                        if (checkresult.Count > 0)
                        {
                            var result = _CumulativeReportContext.Exm_Student_MP_PromotionDMO.Single(a => a.AMST_Id == c.AMST_Id && a.ESTMPP_Id == c.ESTMP_Id);
                            result.ESTMPP_PublishToStudentFlg = result.ESTMPP_PublishToStudentFlg == true ? false : true;
                            result.UpdatedDate = DateTime.Now;
                            _CumulativeReportContext.Update(result);
                        }
                    }
                }

                var i = _CumulativeReportContext.SaveChanges();
                if (i > 0)
                {
                    data.returnval = true;
                    var getexamname = "";
                    if (data.EME_Id > 0)
                    {
                        var getexamlist = _CumulativeReportContext.exammasterDMO.Where(a => a.MI_Id == data.MI_Id && a.EME_Id == data.EME_Id).ToList();
                        getexamname = getexamlist.FirstOrDefault().EME_ExamName;
                    }
                    else
                    {
                        getexamname = "Promotion";
                    }

                    if (data.SMSFLAG == true)
                    {
                        foreach (var c in data.selectedstudentsmarks)
                        {
                            if (c.ESTMP_PublishToStudentFlg == 0)
                            {
                                var Studentname = c.AMST_FirstName;
                                var Studentid = c.AMST_Id;
                                var EMEId = data.EME_Id;
                                var Templatename = "Exam_Individual_Exam_Publish";

                                var getstudentmobileno = _db.Adm_M_Student.Where(a => a.MI_Id == data.MI_Id && a.AMST_Id == Studentid).ToList();

                                try
                                {
                                    SMS sMS = new SMS(_db);
                                    var result = sMS.SendSMSExamMarksPublish(data.MI_Id, getstudentmobileno.FirstOrDefault().AMST_MobileNo, Templatename, Studentid, getexamname, data.ASMAY_Id, Studentname);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                    }

                    if (data.EMAILFLAG == true)
                    {
                        foreach (var c in data.selectedstudentsmarks)
                        {
                            if (c.ESTMP_PublishToStudentFlg == 0)
                            {
                                var Studentname = c.AMST_FirstName;
                                var Studentid = c.AMST_Id;
                                var EMEId = data.EME_Id;
                                var Templatename = "Exam_Individual_Promotion_Marks_Publish";
                                var getstudentmobileno = _db.Adm_M_Student.Where(a => a.MI_Id == data.MI_Id && a.AMST_Id == Studentid).ToList();
                                try
                                {
                                    Email email = new Email(_db);
                                    var result = email.SendMAILExamMarksPublish(data.MI_Id, getstudentmobileno.FirstOrDefault().AMST_emailId, Templatename, Studentid, getexamname, data.ASMAY_Id, Studentname);
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                        }
                    }
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

        // HHS PROMOTION CALCULATION
        public ExamCalculation_SSSEDTO onchangesection(ExamCalculation_SSSEDTO data)
        {
            try
            {
                var checkmarkscalculated = _CumulativeReportContext.Exm_Student_MP_PromotionDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id
                && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id).Distinct().ToList();

                if (checkmarkscalculated.Count() > 0)
                {
                    data.countcalculated = checkmarkscalculated.Count();
                }
                else
                {
                    data.countcalculated = 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ExamCalculation_SSSEDTO promotionsaveddata(ExamCalculation_SSSEDTO exm)
        {
            try
            {
                using (var cmd = _CumulativeReportContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "EXAM_WITHOUT_RULES_PROMOTION_Calculation";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = exm.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = exm.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = exm.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = exm.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@ESTMPP_PublishToStudentFlg", SqlDbType.Int) { Value = exm.ESTMPP_PublishToStudentFlg });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();
                    try
                    {
                        var a = cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogInformation("Exam  promotion Calculation New 1:" + exm.MI_Id + " " + ex.Message + "");
                        Console.WriteLine(ex.Message);
                    }


                    exm.returnval = true;
                }
            }
            catch (Exception ee)
            {
                _acdimpl.LogInformation("Exam Calculation New 2:" + exm.MI_Id + " " + ee.Message + "");
                Console.WriteLine(ee.Message);
                exm.returnval = false;
            }

            return exm;
        }
        public ExamCalculation_SSSEDTO publishtostudentportal(ExamCalculation_SSSEDTO data)
        {
            try
            {
                try
                {
                    TimeZoneInfo INDIAN_ZONE0 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                    DateTime indiantime0 = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE0);

                    var outputval = _CumulativeReportContext.Database.ExecuteSqlCommand("Exam_Promotion_Marks_Approval_Process_To_Publis_Student_Portal @p0,@p1,@p2,@p3", data.MI_Id, data.ASMAY_Id, data.ASMCL_Id, data.ASMS_Id);

                    if (outputval >= 1)
                    {
                        data.returnval = true;

                        Exm_Calculation_LogDMO dmo = new Exm_Calculation_LogDMO();

                        dmo.MI_Id = data.MI_Id;
                        dmo.ASMAY_Id = data.ASMAY_Id;
                        dmo.ASMCL_Id = data.ASMCL_Id;
                        dmo.ASMS_Id = data.ASMS_Id;
                        dmo.EME_Id = 0;
                        dmo.IVRMUL_Id = data.User_Id;
                        dmo.ECL_PublishFlag = 1;
                        dmo.CreatedDate = indiantime0;
                        dmo.UpdatedDate = indiantime0;
                        _CumulativeReportContext.Add(dmo);

                        try
                        {
                            var i = _CumulativeReportContext.SaveChanges();
                            if (i > 0)
                            {
                                _acdimpl.LogInformation("Exam Calculation New Insert Publish Into Log Success");
                                data.returnval = true;
                            }
                            else
                            {
                                _acdimpl.LogInformation("Exam Calculation New Insert Publish Into Log Failed");
                                data.returnval = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            _acdimpl.LogInformation("Exam Calculation New Insert :" + data.MI_Id + " " + ex.Message + "");
                            Console.WriteLine(ex.Message);
                            data.returnval = true;
                        }
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
                catch (Exception ee)
                {
                    _acdimpl.LogInformation("Exam Calculation New 2:" + data.MI_Id + " " + ee.Message + "");
                    Console.WriteLine(ee.Message);
                    data.returnval = false;
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