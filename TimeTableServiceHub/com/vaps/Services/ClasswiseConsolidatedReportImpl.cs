using DataAccessMsSqlServerProvider.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Services
{
    public class ClasswiseConsolidatedReportImpl:Interfaces.ClasswiseConsolidatedReportInterface
    {

        public TTContext _ttcontext;

        public ClasswiseConsolidatedReportImpl(TTContext y)
        {
            _ttcontext = y;
        }
      public TT_ClasswiseConsolidatedReportDTO getalldetails(int id)
        {
            TT_ClasswiseConsolidatedReportDTO data = new TT_ClasswiseConsolidatedReportDTO();
            try
            {
                data.classlist = _ttcontext.School_M_Class.Where(t => t.MI_Id == id).Distinct().ToArray();
                data.sectionlist = _ttcontext.School_M_Section.Where(t => t.MI_Id == id).Distinct().ToArray();
                data.yearlist = _ttcontext.AcademicYear.Where(t => t.MI_Id == id).Distinct().ToArray();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
           
            
        }




        public TT_ClasswiseConsolidatedReportDTO Report(TT_ClasswiseConsolidatedReportDTO data)
        {
            try
            {

                data.subject = (from a in _ttcontext.TT_Final_Generation_DetailedDMO

                                from b in _ttcontext.TT_Final_GenerationDMO
                                from c in _ttcontext.IVRM_School_Master_SubjectsDMO
                                
                                where (b.TTFG_Id == a.TTFG_Id && a.ISMS_Id == c.ISMS_Id&&a.ASMCL_Id==data.ASMCL_Id&&a.ASMS_Id==data.ASMS_Id&&b.MI_Id==c.MI_Id&&b.MI_Id==data.MI_Id&&b.ASMAY_Id==data.ASMAY_Id)
                                select c).Distinct().ToArray();

                data.day = (from a in _ttcontext.TT_Master_Day_ClasswiseDMO
                            from b in _ttcontext.TT_Master_DayDMO
                          
                            where (a.TTMD_Id == b.TTMD_Id && b.MI_Id == data.MI_Id && b.TTMD_ActiveFlag == true && a.TTMDC_ActiveFlag == true&&a.ASMCL_Id==data.ASMCL_Id&& a.ASMAY_Id == data.ASMAY_Id/*&&a.ASMAY_Id==c.ASMAY_Id&&c.Is_Active==true&&c.MI_Id==d.MI_Id&&d.ASMCL_Id==a.ASMCL_Id*/) select b).Distinct().ToArray();
                

                              using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "[TT_School_Classwise_Consolidated_Report]";
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
                    SqlDbType.NVarChar)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                    SqlDbType.Char)
                    {
                        Value = data.ASMS_Id
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
                        data.getreportdata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public TT_ClasswiseConsolidatedReportDTO getabreport(TT_ClasswiseConsolidatedReportDTO data)
        {
            try
            {
            using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_ABBREVIATION_REPORT";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@TYPE",
                    SqlDbType.Char)
                    {
                        Value = data.tsallorindii
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
                        data.getreportdata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


    }
}
