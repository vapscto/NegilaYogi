using DataAccessMsSqlServerProvider.com.vapstech.Exam;
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
    public class SlabWiseExamReportImpl : Interfaces.SlabWiseExamReportinterface
    {
        public ExamContext _context;
        public SlabWiseExamReportImpl(ExamContext _cont)
        {
            _context = _cont;
        }
        public SlabWiseExamReportDTO getsubjects(SlabWiseExamReportDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "Exam_Subject";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar) { Value = data.EME_Id });
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
                        data.subjectlist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }
        public SlabWiseExamReportDTO getslabreport(SlabWiseExamReportDTO data)
        {

            decimal frommarks = Convert.ToDecimal(data.FROMMARKS);
            decimal tomarks = Convert.ToDecimal(data.TOMARKS);
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandTimeout = 0;
                    cmd.CommandText = "Exam_cumulative_Subwise_Report_Modify";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt) { Value = data.ASMAY_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt) { Value = data.ASMCL_Id });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt) { Value = data.ASMS_Id });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = data.MI_Id });
                    cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.BigInt) { Value = data.EME_Id });
                    cmd.Parameters.Add(new SqlParameter("@ISMS_ID", SqlDbType.BigInt) { Value = data.ISMS_ID });
                    cmd.Parameters.Add(new SqlParameter("@FROMMARKS", SqlDbType.Decimal) { Value = frommarks });
                    cmd.Parameters.Add(new SqlParameter("@TOMARKS", SqlDbType.Decimal) { Value = tomarks });
                    cmd.Parameters.Add(new SqlParameter("@reporttype", SqlDbType.VarChar) { Value = data.reporttype });
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
                                    var columnName = dataReader.GetName(iFiled);
                                    dataRow.Add(
                                    dataReader.GetName(iFiled),
                                    dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.getslabreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return data;
        }

    }
}
