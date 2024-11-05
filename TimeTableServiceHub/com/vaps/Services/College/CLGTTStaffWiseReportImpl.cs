using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using TimeTableServiceHub.com.vaps.Interfaces;

namespace TimeTableServiceHub.com.vaps.Services
{
    public class CLGTTStaffWiseReportImpl : Interfaces.CLGTTStaffWiseReportInterface
    {

        public TTContext _ttcontext;
        public DomainModelMsSqlServerContext _db;

        public CLGTTStaffWiseReportImpl(TTContext ttcntx)
        {
            _ttcontext = ttcntx;
        }

        public CLGTTStaffWiseReportDTO getdetails(CLGTTStaffWiseReportDTO data)
        {

            try
            {
                data.acayear = _ttcontext.AcademicYear.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).ToList().Distinct().OrderByDescending(rr => rr.ASMAY_Order).ToArray();
                //data.categorylist = _ttcontext.TTMasterCategoryDMO.AsNoTracking().Where(t => t.MI_Id == data.MI_Id && t.TTMC_ActiveFlag == true).ToList().Distinct().ToArray();


                data.periodslst = _ttcontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id == data.MI_Id).ToList().ToArray();

                data.gridweeks = _ttcontext.TT_Master_DayDMO.Where(t => t.MI_Id == data.MI_Id && t.TTMD_ActiveFlag == true).ToArray();

                data.stafflist = (from b in _ttcontext.HR_Master_Employee_DMO
                                  from a in _ttcontext.TT_Master_Staff_AbbreviationDMO
                                  from c in _ttcontext.CLGTT_Final_Generation_DetailedDMO
                                  from d in _ttcontext.TT_Final_GenerationDMO
                                  where (b.MI_Id == data.MI_Id && b.HRME_ActiveFlag == true && a.HRME_Id == b.HRME_Id && a.TTMSAB_ActiveFlag == true && c.HRME_Id == a.HRME_Id && c.TTFG_Id == d.TTFG_Id && d.TTFG_ActiveFlag == true)
                                  select new CLGTTStaffWiseReportDTO
                                  {
                                      HRME_Id = b.HRME_Id,
                                      staffName = b.HRME_EmployeeFirstName + " " + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == " " || b.HRME_EmployeeMiddleName == "0" ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == " " || b.HRME_EmployeeLastName == "0" ? " " : b.HRME_EmployeeLastName),
                                  }).Distinct().OrderBy(i => i.staffName).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;

        }
        public CLGTTStaffWiseReportDTO getreport(CLGTTStaffWiseReportDTO data)
        {
            try
            {
                
                string groupidss = "0";
                

                for (int d = 0; d < data.staffarray.Count(); d++)
                {
                    groupidss = groupidss + ',' + data.staffarray[d].HRME_Id;
                }
                
                using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_GET_STAFFWISE_REPORT_ROOM";
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
                

                    cmd.Parameters.Add(new SqlParameter("@HRME_Ids",
                      SqlDbType.NVarChar)
                    {
                        Value = groupidss
                    });
                    cmd.Parameters.Add(new SqlParameter("@TYPE",
                      SqlDbType.Char)
                    {
                        Value = data.TYPE
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",
                      SqlDbType.Bit)
                    {
                        Value = data.RMFLG
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
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
      
        public async Task<CLGTTStaffWiseReportDTO> GetStaffDetails(CLGTTStaffWiseReportDTO data)
        {
            try
            {
                data.HRME_Id = _ttcontext.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                data.periodslst = _ttcontext.TT_Master_PeriodDMO.Where(c => c.TTMP_ActiveFlag.Equals(true) && c.MI_Id == data.MI_Id).ToList().ToArray();
                data.gridweeks = _ttcontext.TT_Master_DayDMO.Where(t => t.MI_Id == data.MI_Id && t.TTMD_ActiveFlag == true).ToArray();

                #region Employee Details
                using (var cmd1 = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd1.CommandText = "CLG_PORTAL_EMPLOYEEDETAILS";
                    cmd1.CommandType = CommandType.StoredProcedure;

                    cmd1.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd1.Parameters.Add(new SqlParameter("@HRME_Id",SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });

                    if (cmd1.Connection.State != ConnectionState.Open)
                        cmd1.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd1.ExecuteReaderAsync())
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
                        data.employeedetails = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.empmobileno = _ttcontext.Emp_MobileNo.Where(t => t.HRME_Id == data.HRME_Id && t.HRMEMNO_DeFaultFlag == "default").Select(t => t.HRMEMNO_MobileNo.ToString()).FirstOrDefault();

                data.empemailid = _ttcontext.Emp_Email_Id.Where(t => t.HRME_Id == data.HRME_Id && t.HRMEM_DeFaultFlag == "default").Select(t => t.HRMEM_EmailId).FirstOrDefault();
                #endregion

                using (var cmd = _ttcontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TT_CLG_GET_STAFFWISE_REPORT_ROOM";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_Ids",SqlDbType.NVarChar)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TYPE",SqlDbType.Char)
                    {
                        Value = data.TYPE
                    });
                    cmd.Parameters.Add(new SqlParameter("@FLAG",SqlDbType.Bit)
                    {
                        Value = data.RMFLG
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
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
    }
}
