using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Hostel;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace HostelServiceHub.Services
{
    public class CLGStudentRequestReportIMPL : Interface.CLGStudentRequestReportInterface
    {
        public HostelContext _HostelContext;
        public DomainModelMsSqlServerContext _dbcontext;
        public CLGStudentRequestReportIMPL(HostelContext para1, DomainModelMsSqlServerContext para2)
        {
            _HostelContext = para1;
            _dbcontext = para2;
        }
        public CLGStudentReportDTO getdata(CLGStudentReportDTO data)
        {
            var list = _HostelContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
            data.yearlist = list.ToArray();
            data.hostellist = _HostelContext.HL_Master_Hostel_DMO.Where(a => a.MI_Id == data.MI_Id && a.HLMH_ActiveFlag == true).Distinct().ToArray();
            return data;
        }
        public async Task<CLGStudentReportDTO> getreport(CLGStudentReportDTO data)
        {
            try
            {
                if (data.type.Length > 0)
                {
                    using (var cmd = _HostelContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "CLG_HOSTEL_STUDENT_ALLOTMENT_REPORT";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@type",
                       SqlDbType.VarChar)
                        {
                            Value = data.type
                        });
                        cmd.Parameters.Add(new SqlParameter("@frmdate",
                   SqlDbType.VarChar)
                        {
                            Value = data.frmdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@todate",
                   SqlDbType.VarChar)
                        {
                            Value = data.todate
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                       SqlDbType.BigInt)
                        {
                            Value = data.ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@HLMH_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.HLMH_Id
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
                            data.griddata = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    using (var cmd = _HostelContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "CLG_HOSTEL_STUDENT_REQUEST_REPORT";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@issuertype1",
                       SqlDbType.VarChar)
                        {
                            Value = data.issuertype1
                        });
                        cmd.Parameters.Add(new SqlParameter("@frmdate",
                   SqlDbType.VarChar)
                        {
                            Value = data.frmdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@todate",
                   SqlDbType.VarChar)
                        {
                            Value = data.todate
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
                            data.griddata = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<CLGStudentReportDTO> getconfirmreport(CLGStudentReportDTO data)
        {
            try
            {
                using (var cmd = _HostelContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG_HOSTEL_STUDENT_CONFIRM_REPORT";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
             SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@ctype",
                   SqlDbType.VarChar)
                    {
                        Value = data.ctype
                    });

                    cmd.Parameters.Add(new SqlParameter("@frmdate",
               SqlDbType.VarChar)
                    {
                        Value = data.frmdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
               SqlDbType.VarChar)
                    {
                        Value = data.todate
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
                        data.griddata = retObject.ToArray();
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
