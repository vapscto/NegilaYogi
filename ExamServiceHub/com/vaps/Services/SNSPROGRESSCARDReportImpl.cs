
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
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;

namespace ExamServiceHub.com.vaps.Services
{
    public class SNSPROGRESSCARDReportImpl : Interfaces.SNSPROGRESSCARDReportInterface
    {
        private static ConcurrentDictionary<string, SNSPROGRESSCARDReportDTO> _login =
         new ConcurrentDictionary<string, SNSPROGRESSCARDReportDTO>();

        private readonly ExamContext _HHSAllReportContext;
        ILogger<SNSPROGRESSCARDReportImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public SNSPROGRESSCARDReportImpl(ExamContext cpContext, DomainModelMsSqlServerContext db)
        {
            _HHSAllReportContext = cpContext;
            _db = db;
        }

        public async Task<SNSPROGRESSCARDReportDTO> Getdetails(SNSPROGRESSCARDReportDTO data)//int IVRMM_Id
        {
            SNSPROGRESSCARDReportDTO getdata = new SNSPROGRESSCARDReportDTO();
            try
            {
                List<MasterAcademic> list = new List<MasterAcademic>();
                list = await _HHSAllReportContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToListAsync();
                getdata.yearlist = list.ToArray();

                List<School_M_Section> seclist = new List<School_M_Section>();
                seclist = await _HHSAllReportContext.School_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).OrderBy(t=>t.ASMC_Order).ToListAsync();
                getdata.seclist = seclist.ToArray();

                List<AdmissionClass> admlist = new List<AdmissionClass>();
                admlist = await _HHSAllReportContext.AdmissionClass.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).OrderBy(t=>t.ASMCL_Order).ToListAsync();
                getdata.classlist = admlist.ToArray();

                List<exammasterDMO> esmp = new List<exammasterDMO>();
                esmp = await _HHSAllReportContext.exammasterDMO.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true).OrderBy(t=>t.EME_ExamOrder).ToListAsync();
                getdata.exmstdlist = esmp.ToArray();
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }
            return getdata;

        }

        public async Task<SNSPROGRESSCARDReportDTO> savedetails(SNSPROGRESSCARDReportDTO data)
        {
            try
            {
                List<int?> ids = new List<int?>();
                ids.Add(0);
                ids.Add(1);

                List<string> sol = new List<string>();
                sol.Add("S");
                sol.Add("L");
                sol.Add("D");

                List<exammasterDMO> esmp = new List<exammasterDMO>();
                esmp = await _HHSAllReportContext.exammasterDMO.Where(t => t.MI_Id == data.MI_Id && t.EME_ActiveFlag == true).ToListAsync();
                data.exmstdlist = (from a in _HHSAllReportContext.exammasterDMO
                                   from b in _HHSAllReportContext.ExmStudentMarksProcessSubjectwiseDMO
                                   where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.EME_Id == b.EME_Id && b.ASMS_Id == data.ASMS_Id && b.ASMCL_Id == data.ASMCL_Id)
                                   select a).Distinct().ToArray();

                data.instname = _HHSAllReportContext.Institution_master.Where(t => t.MI_Id == data.MI_Id).ToArray();


                data.studlist = (from a in _HHSAllReportContext.Adm_M_Student
                                 from b in _HHSAllReportContext.School_Adm_Y_StudentDMO
                                 from c in _HHSAllReportContext.AcademicYear
                                 from d in _HHSAllReportContext.AdmissionClass
                                 from e in _HHSAllReportContext.School_M_Section
                                 where (a.AMST_Id==b.AMST_Id && b.ASMAY_Id==c.ASMAY_Id && b.ASMCL_Id==d.ASMCL_Id && b.ASMS_Id==e.ASMS_Id &&   
                                  a.MI_Id == data.MI_Id  && b.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id 
                                  && sol.Contains(a.AMST_SOL) && ids.Contains(b.AMAY_ActiveFlag) && ids.Contains(a.AMST_ActiveFlag))
                                 select new SNSPROGRESSCARDReportDTO
                                 {
                                     AMST_Id = a.AMST_Id,                                     
                                     AMST_FirstName = ((a.AMST_FirstName == null || a.AMST_FirstName == "" ? "" : " " + a.AMST_FirstName) + (a.AMST_MiddleName == null || a.AMST_MiddleName == "" || a.AMST_MiddleName == "0" ? "" : " " + a.AMST_MiddleName) + (a.AMST_LastName == null || a.AMST_LastName == "" || a.AMST_LastName == "0" ? "" : " " + a.AMST_LastName)).Trim(),
                                     AMST_FatherName=a.AMST_FatherName,
                                     AMST_AdmNo = a.AMST_AdmNo,
                                     AMAY_RollNo = b.AMAY_RollNo
                                 }).Distinct().OrderBy(t => t.AMAY_RollNo).ToArray();
               
               var list1 = _HHSAllReportContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id==data.ASMAY_Id).ToList();
                if (list1.Count > 0)
                {
                    data.to_date = list1[0].ASMAY_To_Date;
                    data.fr_date = list1[0].ASMAY_From_Date;
                }

                //get attendance
                List<SNSPROGRESSCARDReportDTO> result12 = new List<SNSPROGRESSCARDReportDTO>();
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "SMS_StudentAttendance_P";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.VarChar)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@from",
                   SqlDbType.Date)
                    {
                        Value = data.fr_date
                    });
                    cmd.Parameters.Add(new SqlParameter("@to",
                   SqlDbType.Date)
                    {
                        Value = data.to_date
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                result12.Add(new SNSPROGRESSCARDReportDTO
                                {
                                    AMST_Id = Convert.ToInt64(dataReader["AMST_Id"].ToString()),
                                    totalpresentday = Convert.ToDecimal((dataReader["TotalPresentDays"].ToString())),
                                    totalworkingday = Convert.ToDecimal((dataReader["TotalSchoolDays"].ToString())),
                                    attper = (dataReader["Total_Percentage"].ToString()),
                                });
                            }
                        }
                        data.attdetails = result12.ToArray();

                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogError(ex.Message);
                        _acdimpl.LogDebug(ex.Message);
                    }
                }
                //get_subjectlist sub exam marks 
                List<SNSPROGRESSCARDReportDTO> result = new List<SNSPROGRESSCARDReportDTO>();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_SREMRS_PROGRESSCARD_REPORT";

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 8000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.BigInt)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.BigInt)
                    {
                        Value = data.ASMS_Id
                    });


                   

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {

                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                result.Add(new SNSPROGRESSCARDReportDTO
                                {
                                    AMST_Id = Convert.ToInt32(dataReader["AMST_Id"].ToString()),
                                    AMST_FirstName = ((dataReader["stname"].ToString() == null ? " " : dataReader["stname"].ToString())).Trim(),
                                    AMST_AdmNo = (dataReader["AMST_AdmNo"].ToString() == null || dataReader["AMST_AdmNo"].ToString() == "" ? "" : dataReader["AMST_AdmNo"].ToString()),
                                    AMAY_RollNo = Convert.ToInt32(dataReader["AMAY_RollNo"].ToString() == null || dataReader["AMAY_RollNo"].ToString() == "" ? "" : dataReader["AMAY_RollNo"].ToString()),

                                    EME_Id = Convert.ToInt32(dataReader["EME_Id"].ToString()),
                                    ISMS_Id = Convert.ToInt32(dataReader["ISMS_Id"].ToString()),
                                    EMSE_Id = Convert.ToInt32(dataReader["EMSE_Id"].ToString()),
                                    EMSE_SubExamName = (dataReader["EMSE_SubExamName"].ToString() == null || dataReader["EMSE_SubExamName"].ToString() == "" ? "" : dataReader["EMSE_SubExamName"].ToString()),

                                    ESTMPS_ObtainedMarks = Convert.ToDecimal(dataReader["ESTMPS_ObtainedMarks"].ToString() == null || dataReader["ESTMPS_ObtainedMarks"].ToString() == "" ? "0" : dataReader["ESTMPS_ObtainedMarks"].ToString()),
                                    ESTMPS_MaxMarks = Convert.ToDecimal(dataReader["ESTMPS_MaxMarks"].ToString() == null || dataReader["ESTMPS_MaxMarks"].ToString() == "" ? "0" : dataReader["ESTMPS_MaxMarks"].ToString()),
                                    ESTMPS_PassFailFlg = (dataReader["ESTMPS_PassFailFlg"].ToString() == null || dataReader["ESTMPS_PassFailFlg"].ToString() == "" ? "" : dataReader["ESTMPS_PassFailFlg"].ToString()),
                                });

                                data.savelist = result.Distinct().OrderBy(t => t.ISMS_Id).ThenBy(t => t.EMSE_Id).ToList();
                                //data.savelist = result.Distinct().ToList();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogError(ex.Message);
                        _acdimpl.LogDebug(ex.Message);
                    }
                }

                //get sub exams
                List<SNSPROGRESSCARDReportDTO> result1 = new List<SNSPROGRESSCARDReportDTO>();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_SREMRS_SUB_SUBEXAM";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                 SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.BigInt)
                    {
                        Value = data.ASMCL_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.BigInt)
                    {
                        Value = data.ASMS_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {

                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                result1.Add(new SNSPROGRESSCARDReportDTO
                                {
                                    EME_Id = Convert.ToInt32(dataReader["EME_Id"].ToString()),
                                    ISMS_Id = Convert.ToInt32(dataReader["ISMS_Id"].ToString()),
                                    ISMS_SubjectName = (dataReader["ISMS_SubjectName"].ToString() == null || dataReader["ISMS_SubjectName"].ToString() == "" ? "" : dataReader["ISMS_SubjectName"].ToString()),


                                    EMSE_Id = Convert.ToInt32(dataReader["EMSE_Id"].ToString()),
                                    EMSE_SubExamName = (dataReader["EMSE_SubExamName"].ToString() == null || dataReader["EMSE_SubExamName"].ToString() == "" ? "" : dataReader["EMSE_SubExamName"].ToString()),


                                    EYCES_AplResultFlg = Convert.ToBoolean(dataReader["EYCES_AplResultFlg"].ToString()),


                                });
                                data.subwithsubexm = result1.Distinct().ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _acdimpl.LogError(ex.Message);
                        _acdimpl.LogDebug(ex.Message);
                    }
                }

                var exmrank = _HHSAllReportContext.ExmStudentMarksProcessDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.ASMCL_Id == data.ASMCL_Id && t.ASMS_Id == data.ASMS_Id).ToList();
                data.exmrank = exmrank.ToArray();

                data.finalexam = _HHSAllReportContext.exammasterDMO.Where(t => t.MI_Id == data.MI_Id && t.EME_FinalExamFlag == true).ToArray();

                var electivemarks1 = (from a in _HHSAllReportContext.Exm_Yearly_CategoryDMO
                                      from b in _HHSAllReportContext.Exm_Category_ClassDMO
                                      from c in _HHSAllReportContext.Exm_Yearly_Category_ExamsDMO
                                      from d in _HHSAllReportContext.Exm_Yrly_Cat_Exams_SubwiseDMO
                                      from e in _HHSAllReportContext.ExmStudentMarksProcessSubjectwiseDMO
                                      where (a.MI_Id == b.MI_Id && a.MI_Id == e.MI_Id && a.MI_Id == data.MI_Id &&
                                      a.ASMAY_Id == b.ASMAY_Id && a.ASMAY_Id == e.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id &&
                                      a.EMCA_Id == b.EMCA_Id && a.EYC_Id == c.EYC_Id && d.EYCE_Id == c.EYCE_Id && d.EYCES_AplResultFlg == false && d.ISMS_Id == e.ISMS_Id && b.ASMCL_Id == e.ASMCL_Id && b.ASMS_Id == e.ASMS_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id 
                                      )
                                      select e
                                     ).Distinct().ToList();
                data.electivemarks = electivemarks1.ToArray();
            }
            catch (Exception ex)
            {
                _acdimpl.LogError(ex.Message);
                _acdimpl.LogDebug(ex.Message);
            }

            return data;
        }


        public SNSPROGRESSCARDReportDTO yearchange(SNSPROGRESSCARDReportDTO data)
        {
            try
            {
                //List<AdmissionClass> admlist = new List<AdmissionClass>();
                //admlist = _HHSAllReportContext.AdmissionClass.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToList();
                //data.classlist = admlist.ToArray();

                data.classlist = (from a in _HHSAllReportContext.School_Adm_Y_StudentDMO
                                  from b in _HHSAllReportContext.AdmissionClass
                                  where (b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMCL_ActiveFlag == true && a.AMAY_ActiveFlag == 1)

                                  select b
                                  
                                  ).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SNSPROGRESSCARDReportDTO classchange(SNSPROGRESSCARDReportDTO data)
        {
            try
            {
                //List<School_M_Section> seclist = new List<School_M_Section>();
                //seclist = _HHSAllReportContext.School_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).ToList();
                //data.seclist = seclist.ToArray();

                data.seclist = (from a in _HHSAllReportContext.School_Adm_Y_StudentDMO
                                    from b in _HHSAllReportContext.AdmissionClass
                                    from c in _HHSAllReportContext.Adm_M_Student
                                    from d in _HHSAllReportContext.School_M_Section
                                where (a.AMST_Id == c.AMST_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && b.ASMCL_Id == a.ASMCL_Id && b.ASMCL_Id == c.ASMCL_Id && a.ASMCL_Id == data.ASMCL_Id && d.ASMS_Id == a.ASMS_Id)
                                    select d).Distinct().OrderBy(t => t.ASMS_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SNSPROGRESSCARDReportDTO sectionchange(SNSPROGRESSCARDReportDTO data)
        {
            //    try
            //    {
            //        data.studentlist = (from a in _HHSAllReportContext.Adm_M_Student
            //                            from b in _HHSAllReportContext.School_Adm_Y_StudentDMO
            //                            from c in _HHSAllReportContext.AdmissionClass
            //                            from d in _HHSAllReportContext.School_M_Section
            //                            from e in _HHSAllReportContext.AcademicYear
            //                            where (a.MI_Id == c.MI_Id && d.MI_Id == e.MI_Id && b.AMST_Id == a.AMST_Id && b.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == d.ASMS_Id && b.ASMAY_Id == e.ASMAY_Id && b.ASMCL_Id == data.ASMCL_Id && b.ASMS_Id == data.ASMS_Id && b.ASMAY_Id == data.ASMAY_Id)
            //                            select new SNSPROGRESSCARDReportDTO
            //                            {
            //                                AMST_Id = a.AMST_Id,
            //                                AMST_FirstName = a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName
            //                            }).Distinct().ToArray();
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //    }
            return data;
        }


    }

   
}
