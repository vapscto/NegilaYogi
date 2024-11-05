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
    public class Hostel_Request_ReportImpl : Interface.Hostel_Request_ReportInterface
    {
        public HostelContext _HostelContext;
        public DomainModelMsSqlServerContext _dbcontext;
        public Hostel_Request_ReportImpl(HostelContext para1, DomainModelMsSqlServerContext para2)
        {
            _HostelContext = para1;
            _dbcontext = para2;
        }
        public Hostel_Request_ReportDTO getdata(Hostel_Request_ReportDTO data)
        {            
            return data;
        }
        public async Task<Hostel_Request_ReportDTO> getreport(Hostel_Request_ReportDTO data)
        {
            try
            {
                using (var cmd = _HostelContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HOSTEL_STUDENT_REQUEST_REPORT";
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<Hostel_Request_ReportDTO> getconfirmreport(Hostel_Request_ReportDTO data)
        {
            try
            {
                using (var cmd = _HostelContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "HOSTEL_STUDENT_CONFIRM_REPORT";
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
