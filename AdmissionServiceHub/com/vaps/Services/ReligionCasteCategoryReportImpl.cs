using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class ReligionCasteCategoryReportImpl:Interfaces.ReligionCasteCategoryReportInterface
    {
        public AdmissionFormContext _context;

        public ReligionCasteCategoryReportImpl(AdmissionFormContext _form)
        {
            _context = _form;
        }
        public ReligionCasteCategoryReport_DTO loaddata(ReligionCasteCategoryReport_DTO data)
        {
            try
            {
                data.yearlist = _context.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).Distinct().ToArray();
                data.classlist = _context.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).Distinct().ToArray();
                data.castecategorylist = _context.CasteCategory.Distinct().ToArray();
                data.religionlist = _context.Religion.Where(t=>t.Is_Active==true).Distinct().ToArray();
                
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public async Task<ReligionCasteCategoryReport_DTO> showdetails(ReligionCasteCategoryReport_DTO data)
        {
            try
            {

                string year_ids = "0";
                List<long> year1_ids = new List<long>();
                foreach (var item in data.selectedclasslist)
                {
                    year1_ids.Add(item.ASMCL_Id);
                }
                for (int s = 0; s < year1_ids.Count(); s++)
                {
                    year_ids = year_ids + ',' + year1_ids[s].ToString();
                }
                // type
                string type_ids = "0";
                List<long> type1_ids = new List<long>();
                foreach (var item in data.selectedcastecategorylist)
                {
                    type1_ids.Add(item.IMCC_Id);
                }
                for (int s = 0; s < type1_ids.Count(); s++)
                {
                    type_ids = type_ids + ',' + type1_ids[s].ToString();
                }
                //activity
                string activity_ids = "0";
                List<long> activity1_ids = new List<long>();
                foreach (var item in data.selectedreligionlist)
                {
                    activity1_ids.Add(item.IVRMMR_Id);
                }
                for (int s = 0; s < activity1_ids.Count(); s++)
                {
                    activity_ids = activity_ids + ',' + activity1_ids[s].ToString();
                }
                if (data.selectedcastecategorylist.Length > 0)
                {
                    data.flag = "flag1";
                    data.ZERO = "";
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Admission_ReligionCasteCategory_Report";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@flag",
                       SqlDbType.VarChar)
                        {
                            Value = data.flag
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                         SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                        {
                            Value = year_ids
                        });
                        cmd.Parameters.Add(new SqlParameter("@castecategory",
                        SqlDbType.VarChar)
                        {
                            Value = type_ids
                        });
                        cmd.Parameters.Add(new SqlParameter("@religion",
                        SqlDbType.VarChar)
                        {
                            Value = data.ZERO
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();
                        try
                        {
                            using (var dataReader1 = await cmd.ExecuteReaderAsync())
                            {
                                while (await dataReader1.ReadAsync())
                                {
                                    var dataRow1 = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled = 0; iFiled < dataReader1.FieldCount; iFiled++)
                                    {
                                        dataRow1.Add(
                                            dataReader1.GetName(iFiled),
                                            dataReader1.IsDBNull(iFiled) ? null : dataReader1[iFiled] // use null instead of {}
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow1);
                                }
                            }
                            data.reportlist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                if (data.selectedreligionlist.Length > 0)
                {
                    data.flag = "flag2";
                    data.ZERO = "";
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Admission_ReligionCasteCategory_Report";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@flag",
                        SqlDbType.VarChar)
                        {
                            Value = data.flag
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                         SqlDbType.VarChar)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                        {
                            Value = year_ids
                        });
                        cmd.Parameters.Add(new SqlParameter("@castecategory",
                        SqlDbType.VarChar)
                        {
                            Value = data.ZERO
                        });
                        cmd.Parameters.Add(new SqlParameter("@religion",
                        SqlDbType.VarChar)
                        {
                            Value = activity_ids
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
                            data.reportlist2 = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
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