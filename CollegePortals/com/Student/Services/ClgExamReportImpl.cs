using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Linq;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using DomainModel.Model.com.vaps.Exam;
using PreadmissionDTOs.com.vaps.Exam;
using PreadmissionDTOs.com.vaps.College.Portals.Student;
using DataAccessMsSqlServerProvider.com.vapstech.College.Portal;
using DataAccessMsSqlServerProvider.com.vapstech.College.Exam;

namespace CollegePortals.com.Student.Services
{
    public class ClgExamReportImpl : Interfaces.ClgExamReportInterface
    {
        private static ConcurrentDictionary<string, ClgExamDTO> _login = new ConcurrentDictionary<string, ClgExamDTO>();
        private CollegeportalContext _Portalcontext;
        private ClgExamContext _Examcontext;

        public ClgExamReportImpl(CollegeportalContext Feecontext)
        {
            _Portalcontext = Feecontext;
        }
        public ClgExamDTO getloaddata(ClgExamDTO data)
        {
            try
            {
                data.studetiallist = (from d in _Portalcontext.academicYearDMO
                                      from e in _Portalcontext.CLG_Exm_Col_Student_Marks_ProcessDMO
                                      from f in _Portalcontext.Adm_Master_College_StudentDMO
                                      where (d.ASMAY_Id == e.ASMAY_Id && e.AMCST_Id == f.AMCST_Id && d.ASMAY_ActiveFlag == 1 && f.AMCST_ActiveFlag == true &&
                                      e.AMCST_Id == data.AMCST_Id && e.MI_Id == data.MI_Id && f.AMCST_Id == data.AMCST_Id)
                                      select new ClgExamDTO
                                      {
                                          ASMAY_Id = d.ASMAY_Id,
                                          ASMAY_Year = d.ASMAY_Year
                                      }
                             ).Distinct().ToArray();
                //
                List<long> ASMAY_Ids = new List<long>();
                var Yearlist = _Portalcontext.Adm_College_Yearly_StudentDMO.Where(R => R.AMCST_Id == data.AMCST_Id).Distinct().ToList();
                if (Yearlist.Count > 0)
                {
                    foreach (var d in Yearlist)
                    {
                        ASMAY_Ids.Add(d.ASMAY_Id);
                    }
                }
                data.examReportList = _Portalcontext.academicYearDMO.Where(R => R.MI_Id == data.MI_Id && ASMAY_Ids.Contains(R.ASMAY_Id)).Distinct().ToArray();
                //  
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<ClgExamDTO> getexamdata(ClgExamDTO data)
        {
            try
            {
                long asmCOid = 0;
                long asmBRid = 0;
                long asmSEMid = 0;
                var classSectionData = (from d in _Portalcontext.academicYearDMO
                                        from a in _Portalcontext.MasterCourseDMO
                                        from b in _Portalcontext.ClgMasterBranchDMO
                                        from c in _Portalcontext.CLG_Adm_Master_SemesterDMO
                                        from e in _Portalcontext.Adm_Master_College_StudentDMO
                                        from f in _Portalcontext.CLG_Exm_Col_Student_Marks_ProcessDMO
                                        where (e.AMCST_Id == data.AMCST_Id && a.AMCO_Id == f.AMCO_Id && b.AMB_Id == f.AMB_Id && c.AMSE_Id == f.AMSE_Id && e.MI_Id == data.MI_Id && d.ASMAY_Id == f.ASMAY_Id && f.MI_Id == data.MI_Id && f.ASMAY_Id == data.ASMAY_Id && f.AMCST_Id == data.AMCST_Id)
                                        select new ClgExamDTO
                                        {
                                            AMCO_CourseName = a.AMCO_CourseName,
                                            AMCO_Id = a.AMCO_Id,
                                            AMB_BranchName = b.AMB_BranchName,
                                            AMB_Id = b.AMB_Id,
                                            AMSE_SEMName = c.AMSE_SEMName,
                                            AMSE_Id = c.AMSE_Id,
                                            ASMAY_Year = d.ASMAY_Year
                                        }).Distinct().ToList();

                if (classSectionData.Count > 0)
                {
                    asmCOid = classSectionData.FirstOrDefault().AMCO_Id;
                    asmBRid = classSectionData.FirstOrDefault().AMB_Id;
                    asmSEMid = classSectionData.FirstOrDefault().AMSE_Id;
                }
                else
                {
                    asmCOid = 0;
                    asmBRid = 0;
                    asmSEMid = 0;
                }

                if (data.Type == "Overall")
                {
                    await StudentExamDetails(data);
                }
                else if (data.Type == "SWAE")
                {
                    data.subjectlist = (from a in _Portalcontext.Adm_Master_College_StudentDMO
                                        from b in _Portalcontext.IVRM_Master_SubjectsDMO
                                        from c in _Portalcontext.CLG_Exm_Col_Student_Marks_Process_SubjectwiseDMO
                                        where (a.AMCST_Id == c.AMCST_Id && b.ISMS_Id == c.ISMS_Id && a.AMCST_Id == data.AMCST_Id && a.AMCST_ActiveFlag == true && b.ISMS_ActiveFlag == 1 && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id)
                                        select new ProgressCardReportDTO
                                        {
                                            ISMS_Id = b.ISMS_Id,
                                            ISMS_SubjectName = b.ISMS_SubjectName,
                                            ISMS_SubjectCode = b.ISMS_SubjectCode
                                        }).Distinct().OrderBy(a => a.ISMS_Id).ToArray();
                }
                else if (data.Type == "Halt_Ticket")
                {
                    using (var cmd = _Portalcontext.Database.GetDbConnection().CreateCommand())
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
                            Value = "4"
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                SqlDbType.BigInt)
                        {
                            Value = data.ASMAY_Id
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
                            Value = data.AMCST_Id
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
                            data.examlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    using (var cmd = _Portalcontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "collegeFeeBalanceCheck";
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
                  
                        cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
         SqlDbType.VarChar)
                        {
                            Value = data.AMCST_Id
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
                            data.stuyearlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {

                    List<long> emeidnew = new List<long>();
                    var getexamlist = (from a in _Portalcontext.Adm_Master_College_StudentDMO
                                       from b in _Portalcontext.col_exammasterDMO
                                       from c in _Portalcontext.CLG_Exm_Col_Student_Marks_Process_SubjectwiseDMO
                                       where (a.AMCST_Id == c.AMCST_Id && b.EME_Id == c.EME_Id && a.AMCST_Id == data.AMCST_Id && a.AMCST_ActiveFlag == true && b.EME_ActiveFlag == true && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id)
                                       select new exammasterDMO
                                       {
                                           EME_Id = b.EME_Id
                                       }).Distinct().ToList();

                    foreach (var e in getexamlist)
                    {
                        emeidnew.Add(e.EME_Id);
                    }

                    List<exammasterDMO> esmp = new List<exammasterDMO>();

                    esmp = (from a in _Portalcontext.Adm_Master_College_StudentDMO
                            from b in _Portalcontext.col_exammasterDMO
                            from c in _Portalcontext.CLG_Exm_Col_Student_Marks_ProcessDMO
                            where (a.AMCST_Id == c.AMCST_Id && b.EME_Id == c.EME_Id && a.AMCST_Id == data.AMCST_Id && a.AMCST_ActiveFlag == true && b.EME_ActiveFlag == true && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id)
                            select new exammasterDMO
                            {
                                EME_Id = b.EME_Id,
                                EME_ExamName = b.EME_ExamName,
                                EME_ExamOrder = b.EME_ExamOrder
                            }).Distinct().ToList();
                    data.examlist = esmp.OrderBy(a => a.EME_ExamOrder).Distinct().ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ClgExamDTO getSubjects(ClgExamDTO data)
        {
            try
            {
                long asmCOid = 0;
                long asmBRid = 0;
                long asmSEMid = 0;

                var classSectionData = (from d in _Portalcontext.academicYearDMO
                                        from a in _Portalcontext.MasterCourseDMO
                                        from b in _Portalcontext.ClgMasterBranchDMO
                                        from c in _Portalcontext.CLG_Adm_Master_SemesterDMO
                                        from e in _Portalcontext.Adm_Master_College_StudentDMO
                                        from f in _Portalcontext.CLG_Exm_Col_Student_Marks_ProcessDMO
                                        where (e.AMCST_Id == data.AMCST_Id && a.AMCO_Id == f.AMCO_Id && b.AMB_Id == f.AMB_Id && c.AMSE_Id == f.AMSE_Id && e.MI_Id == data.MI_Id && d.ASMAY_Id == f.ASMAY_Id && f.MI_Id == data.MI_Id && f.ASMAY_Id == data.ASMAY_Id && f.AMCST_Id == data.AMCST_Id)
                                        select new ClgExamDTO
                                        {
                                            AMCO_CourseName = a.AMCO_CourseName,
                                            AMCO_Id = a.AMCO_Id,
                                            AMB_BranchName = b.AMB_BranchName,
                                            AMB_Id = b.AMB_Id,
                                            AMSE_SEMName = c.AMSE_SEMName,
                                            AMSE_Id = c.AMSE_Id,
                                            ASMAY_Year = d.ASMAY_Year
                                        }).Distinct().ToList();

                if (classSectionData.Count > 0)
                {
                    asmCOid = classSectionData.FirstOrDefault().AMCO_Id;
                    asmBRid = classSectionData.FirstOrDefault().AMB_Id;
                    asmSEMid = classSectionData.FirstOrDefault().AMSE_Id;
                }
                else
                {
                    asmCOid = 0;
                    asmBRid = 0;
                    asmSEMid = 0;
                }

                data.subjectlist = (from a in _Portalcontext.Adm_Master_College_StudentDMO
                                    from b in _Portalcontext.IVRM_Master_SubjectsDMO
                                    from c in _Portalcontext.CLG_Exm_Col_Student_Marks_Process_SubjectwiseDMO
                                    where (a.AMCST_Id == c.AMCST_Id && b.ISMS_Id == c.ISMS_Id && a.AMCST_Id == data.AMCST_Id && a.AMCST_ActiveFlag == true && b.ISMS_ActiveFlag == 1 && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && c.AMCST_Id == data.AMCST_Id && c.ASMAY_Id == data.ASMAY_Id)
                                    select new ProgressCardReportDTO
                                    {
                                        ISMS_Id = b.ISMS_Id,
                                        ISMS_SubjectName = b.ISMS_SubjectName,
                                        ISMS_SubjectCode = b.ISMS_SubjectCode
                                    }).Distinct().OrderBy(a => a.ISMS_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<ClgExamDTO> StudentExamDetails(ClgExamDTO data)
        {
            try
            {
                long asmCOid = 0;
                long asmBRid = 0;
                long asmSEMid = 0;

                var classSectionData = (from d in _Portalcontext.academicYearDMO
                                        from a in _Portalcontext.MasterCourseDMO
                                        from b in _Portalcontext.ClgMasterBranchDMO
                                        from c in _Portalcontext.CLG_Adm_Master_SemesterDMO
                                        from e in _Portalcontext.Adm_Master_College_StudentDMO
                                        from f in _Portalcontext.CLG_Exm_Col_Student_Marks_ProcessDMO
                                        where (e.AMCST_Id == data.AMCST_Id && a.AMCO_Id == f.AMCO_Id && b.AMB_Id == f.AMB_Id && c.AMSE_Id == f.AMSE_Id && e.MI_Id == data.MI_Id && d.ASMAY_Id == f.ASMAY_Id && f.MI_Id == data.MI_Id && f.ASMAY_Id == data.ASMAY_Id && f.AMCST_Id == data.AMCST_Id)
                                        select new ClgExamDTO
                                        {
                                            AMCO_CourseName = a.AMCO_CourseName,
                                            AMCO_Id = a.AMCO_Id,
                                            AMB_BranchName = b.AMB_BranchName,
                                            AMB_Id = b.AMB_Id,
                                            AMSE_SEMName = c.AMSE_SEMName,
                                            AMSE_Id = c.AMSE_Id,
                                            ASMAY_Year = d.ASMAY_Year
                                        }).Distinct().ToList();

                if (classSectionData.Count > 0)
                {
                    asmCOid = classSectionData.FirstOrDefault().AMCO_Id;
                    asmBRid = classSectionData.FirstOrDefault().AMB_Id;
                    asmSEMid = classSectionData.FirstOrDefault().AMSE_Id;
                }
                else
                {
                    asmCOid = 0;
                    asmBRid = 0;
                    asmSEMid = 0;
                }

                using (var cmd = _Portalcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG_PORTAL_EXAMREPORT_Modify_New";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@AMCO_Id", SqlDbType.BigInt)
                    {
                        Value = asmCOid
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMB_Id", SqlDbType.BigInt)
                    {
                        Value = asmBRid
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id", SqlDbType.BigInt)
                    {
                        Value = asmSEMid
                    });
                    cmd.Parameters.Add(new SqlParameter("@AMCST_Id", SqlDbType.BigInt)
                    {
                        Value = data.AMCST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.BigInt)
                    {
                        Value = data.EME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.BigInt)
                    {
                        Value = data.ISMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.VarChar)
                    {
                        Value = data.Type
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
                        data.examReportList = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
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
