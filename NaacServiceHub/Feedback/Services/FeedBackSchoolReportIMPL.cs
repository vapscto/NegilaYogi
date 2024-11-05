using DataAccessMsSqlServerProvider.FeedBack;
using DomainModel.Model.com.vaps.admission;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.NAAC.FeedBack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Feedback.Services
{
    public class FeedBackSchoolReportIMPL : Interface.FeedBackSchoolReportInterface
    {
        public FeedBackContext _context;

        public FeedBackSchoolReportIMPL(FeedBackContext context)
        {
            _context = context;
        }

        public FeedBackSchoolReportDTO getdetails(FeedBackSchoolReportDTO data)
        {
            try
            {
                data.getyear = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).Distinct().OrderByDescending(a => a.ASMAY_Order).ToArray();
                data.feedbacktype = _context.FeedBackMasterTypeDMO.Where(a => a.MI_Id == data.MI_Id  && a.FMTY_ActiveFlag == true).OrderBy(a => a.FMTY_FTOrder).ToArray();
                data.classlist = (from a in _context.School_M_Class
                                  from b in _context.School_Adm_Y_StudentDMO
                                  from c in _context.AcademicYear
                                  where (a.MI_Id == c.MI_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMAY_Id == c.ASMAY_Id && a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true)
                                  select new FeedBackSchoolReportDTO
                                  {
                                      ASMCL_Id = b.ASMCL_Id,
                                      ASMCL_ClassName = a.ASMCL_ClassName,
                                      ASMCL_Order = a.ASMCL_Order
                                  }).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();
               
               

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FeedBackSchoolReportDTO onclass(FeedBackSchoolReportDTO data)
        {
            try
            {
                data.sectionlist = ( from a in _context.School_M_Section
                                    from b in _context.School_Adm_Y_StudentDMO
                                    where ( a.ASMS_Id == b.ASMS_Id && b.ASMCL_Id == data.ASMCL_Id && a.MI_Id == data.MI_Id && a.ASMC_ActiveFlag == 1)
                                  select new FeedBackSchoolReportDTO
                                  {
                                      ASMS_Id = a.ASMS_Id,
                                      ASMC_SectionName = a.ASMC_SectionName,
                                      ASMC_Order=a.ASMC_Order
                                  }).Distinct().OrderBy(t => t.ASMC_Order).ToArray();

               

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //onclass
        //count
        public FeedBackSchoolReportDTO count(FeedBackSchoolReportDTO data)
        {
            try
            {
               
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_School_FeedbackCountDetails";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FMTY_Id", SqlDbType.BigInt)
                    {
                        Value = data.FMTY_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt)
                    {
                        Value = data.ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.Bit)
                    {
                        Value = data.type
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
                        data.getcount = retObject.Distinct().ToArray();
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
        public FeedBackSchoolReportDTO getreport(FeedBackSchoolReportDTO data)
        {
            try
            {                
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Adm_School_FeedbackCount";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@FMTY_Id", SqlDbType.BigInt)
                    {
                        Value = data.FMTY_Id
                    });
                    //@ASMCL_Id
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.BigInt)
                    {
                        Value = data.ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.BigInt)
                    {
                        Value = data.ASMS_Id
                    });
                    //ASMS_Id
                    cmd.Parameters.Add(new SqlParameter("@optionflag", SqlDbType.VarChar)
                    {
                        Value = data.optionflag
                    });
                    //@optionflag
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
                        data.getReport = retObject.Distinct().ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                //data.getyear = _context.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
