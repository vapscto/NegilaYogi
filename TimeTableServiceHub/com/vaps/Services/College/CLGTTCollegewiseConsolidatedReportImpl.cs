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
    public class CLGTTCollegewiseConsolidatedReportImpl:Interfaces.CLGTTCollegewiseConsolidatedReportInterface
    {
        public TTContext _context;
        public CLGTTCollegewiseConsolidatedReportImpl(TTContext r)
        {
            _context = r;
        }
        public CLGTTCollegewiseConsolidatedReportDTO loaddata(CLGTTCollegewiseConsolidatedReportDTO data)
        {
            try
            {
                data.yearlist = _context.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().OrderByDescending(t=>t.ASMAY_Order).ToArray();
                data.categorylist = _context.TTMasterCategoryDMO.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.TTMC_ActiveFlag == true).ToArray();
                data.sectionlist = _context.Adm_College_Master_SectionDMO.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.ACMS_ActiveFlag == true).ToArray();
                //data.courselist = _context.MasterCourseDMO.Where(t => t.MI_Id == data.MI_Id && t.AMCO_ActiveFlag == true).Distinct().OrderByDescending(t=>t.AMCO_Order).ToArray();
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }

            return data;
        }
     
      public CLGTTCollegewiseConsolidatedReportDTO report(CLGTTCollegewiseConsolidatedReportDTO data)
        {
            try
            {





                data.subject = (from a in _context.CLGTT_Final_Generation_DetailedDMO

                                from b in _context.TT_Final_GenerationDMO
                                from c in _context.IVRM_School_Master_SubjectsDMO

                                where (b.TTFG_Id == a.TTFG_Id && a.ISMS_Id == c.ISMS_Id && b.MI_Id == c.MI_Id && b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id&&a.AMCO_Id==data.AMCO_Id&&a.AMB_Id==data.AMB_Id&&a.AMSE_Id==data.AMSE_Id&&a.ACMS_Id==data.ACMS_Id&&b.TTMC_Id==data.TTMC_Id)
                                select c).Distinct().ToArray();

                data.day = (from a in _context.CLGTT_Master_Day_CourseBranchDMO
                            from b in _context.TT_Master_DayDMO

                            where (a.TTMD_Id == b.TTMD_Id && b.MI_Id == data.MI_Id && b.TTMD_ActiveFlag == true && a.TTMDC_ActiveFlag == true && a.ASMAY_Id == data.ASMAY_Id &&a.AMCO_Id==data.AMCO_Id&&a.AMB_Id==data.AMB_Id&&a.AMSE_Id==data.AMSE_Id) /*&&a.ASMAY_Id==c.ASMAY_Id&&c.Is_Active==true&&c.MI_Id==d.MI_Id&&d.ASMCL_Id==a.ASMCL_Id*/
                            select b).Distinct().ToArray();


                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "[CLGTT_College_Coursewise_Consolidated_Report]";
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


                    cmd.Parameters.Add(new SqlParameter("@TTMC_Id",
                    SqlDbType.NVarChar)
                    {
                        Value = data.TTMC_Id
                    });
                    
                         cmd.Parameters.Add(new SqlParameter("@AMCO_Id",
                    SqlDbType.NVarChar)
                         {
                             Value = data.AMCO_Id
                         });
                    
                         cmd.Parameters.Add(new SqlParameter("@AMB_Id",
                    SqlDbType.NVarChar)
                         {
                             Value = data.AMB_Id
                         });

                    cmd.Parameters.Add(new SqlParameter("@AMSE_Id",
                    SqlDbType.Char)
                    {
                        Value = data.AMSE_Id
                    });
                    
                          cmd.Parameters.Add(new SqlParameter("@ACMS_Id",
                    SqlDbType.Char)
                          {
                              Value = data.ACMS_Id
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
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }




    }
}
