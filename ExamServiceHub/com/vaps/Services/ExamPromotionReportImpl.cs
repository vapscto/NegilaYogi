using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Services
{
    public class ExamPromotionReportImpl : Interfaces.ExamPromotionReportInterface
    {
        private static ConcurrentDictionary<string, ExamPromotionReport_DTO> _login =
       new ConcurrentDictionary<string, ExamPromotionReport_DTO>();

        public ExamContext _context;
        ILogger<ExamPromotionReportImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public ExamPromotionReportImpl(ExamContext cpContext, DomainModelMsSqlServerContext db, ILogger<ExamPromotionReportImpl> _acdimp)
        {
            _context = cpContext;
            _db = db;
            _acdimpl = _acdimp;
        }

        public ExamPromotionReport_DTO Getdetails(ExamPromotionReport_DTO data)
        {
            try
            {
                data.getyear = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;

        }

        public ExamPromotionReport_DTO get_class(ExamPromotionReport_DTO data)
        {
            try
            {
                data.getclass = (from a in _context.AcademicYear
                                 from b in _context.AdmissionClass
                                 from c in _context.Exm_Category_ClassDMO
                                 where (a.ASMAY_Id == c.ASMAY_Id && b.ASMCL_Id == c.ASMCL_Id && c.ECAC_ActiveFlag == true && c.MI_Id == data.MI_Id
                                 && c.ASMAY_Id == data.ASMAY_Id)
                                 select b).Distinct().OrderBy(a => a.ASMCL_Order).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ExamPromotionReport_DTO get_section(ExamPromotionReport_DTO data)
        {
            try
            {
                data.getsection = (from a in _context.AcademicYear
                                   from b in _context.AdmissionClass
                                   from c in _context.Exm_Category_ClassDMO
                                   from d in _context.School_M_Section
                                   where (a.ASMAY_Id == c.ASMAY_Id && b.ASMCL_Id == c.ASMCL_Id && c.ASMS_Id == d.ASMS_Id && c.ECAC_ActiveFlag == true
                                   && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.ASMCL_Id == data.ASMCL_Id)
                                   select d).Distinct().OrderBy(a => a.ASMC_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public ExamPromotionReport_DTO get_exam(ExamPromotionReport_DTO data)
        {
            try
            {
                var get_emcaid = _context.Exm_Category_ClassDMO.Where(a => a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.ECAC_ActiveFlag == true).ToList();

                var get_eycid = _context.Exm_Yearly_CategoryDMO.Where(a => a.EMCA_Id == get_emcaid[0].EMCA_Id && a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id).ToList();

                var get_examid = (from a in _context.Exm_Yearly_Category_ExamsDMO
                                  from b in _context.exammasterDMO
                                  where (a.EME_Id == b.EME_Id && a.EYCE_ActiveFlg == true && a.EYC_Id == get_eycid[0].EYC_Id && b.MI_Id == data.MI_Id && b.EME_ActiveFlag == true)
                                  select b).Distinct().OrderBy(c => c.EME_ExamOrder).ToArray();

                data.getexam = get_examid;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _acdimpl.LogDebug("get_exam() in Exam Promotion Remaks form" + ex.Message);
                _acdimpl.LogError("get_exam() in Exam Promotion Remaks form" + ex.Message);
            }
            return data;
        }

        public async Task<ExamPromotionReport_DTO> get_reports(ExamPromotionReport_DTO data)
        {
            try
            {
                string emi_id = "";
                if (data.examtype == "1")
                {
                    emi_id = "";
                }
                else if (data.examtype == "0")
                {
                    emi_id = Convert.ToString(data.EME_Id);
                }
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "get_Exam_Promotion_Report";
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
                    cmd.Parameters.Add(new SqlParameter("@examtype",
                    SqlDbType.VarChar)
                    {
                        Value = data.examtype
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
                    cmd.Parameters.Add(new SqlParameter("@EME_Id",
                    SqlDbType.VarChar)
                    {
                        Value = emi_id
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
                        data.get_result = retObject.ToArray();
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
