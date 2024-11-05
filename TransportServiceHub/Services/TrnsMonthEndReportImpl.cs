using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model.com.vapstech.Transport;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Services
{
    public class TrnsMonthEndReportImpl : Interfaces.TrnsMonthEndReportInterface
    {
        public TransportContext _context;
        ILogger<TrnsMonthEndReportImpl> _areaimpl;
        //      public DomainModelMsSqlServerContext _db;


        // parameterized constructor
        public TrnsMonthEndReportImpl(ILogger<TrnsMonthEndReportImpl> areaimpl, TransportContext context)
        {

            _areaimpl = areaimpl;
            _context = context;

        }


        public TrnsMonthEndReportDTO getdata1(int id)
        {
            TrnsMonthEndReportDTO data = new TrnsMonthEndReportDTO();
            try
            {
                var list = _context.AcademicYearDMO.Where(t => t.MI_Id == id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.Acdlist = list.ToArray();

                data.monthlist = _context.MonthDMO.Where(a => a.Is_Active == true).ToArray();
            }
            catch (Exception ex)
            {
                _areaimpl.LogInformation("Transport Error Master Area getdata" + ex.Message);
            }
            return data;
        }
        public TrnsMonthEndReportDTO savedata1(TrnsMonthEndReportDTO data)
        {
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_TRN_MONTH_END_REPORT";

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 8000000;

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

                    cmd.Parameters.Add(new SqlParameter("@frmdate",
                                SqlDbType.VarChar)
                    {
                        Value = data.year
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
                                SqlDbType.VarChar)
                    {
                        Value = data.IVRM_Month_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@type",
                                SqlDbType.VarChar)
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
        public TrnsMonthEndReportDTO getdata(int id)
        {
            TrnsMonthEndReportDTO data = new TrnsMonthEndReportDTO();
            try
            {
              
              var  list = _context.AcademicYearDMO.Where(t => t.MI_Id == id && t.Is_Active == true && t.ASMAY_ActiveFlag==1).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.Acdlist = list.ToArray();

                data.monthlist = _context.MonthDMO.Where(a => a.Is_Active == true).ToArray();
            }
            catch (Exception ex)
            {
                _areaimpl.LogInformation("Transport Error Master Area getdata" + ex.Message);
            }
            return data;
        }
        public TrnsMonthEndReportDTO savedata(TrnsMonthEndReportDTO data)
        {
            try
            {
                #region STUDENT STRENGTH
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TRN_Month_End_Report";
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
                        data.Fillstudentstrenth = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                _areaimpl.LogInformation("Transport Error Master Area savedata" + ex.Message);
            }
            return data;
        }

        public TrnsMonthEndReportDTO geteditdata(TrnsMonthEndReportDTO data)
        {
            try
            {
              
            }
            catch (Exception ex)
            {
                _areaimpl.LogInformation("Transport Error Master Area geteditdata " + ex.Message);
            }
            return data;
        }

        public TrnsMonthEndReportDTO activedeactive(TrnsMonthEndReportDTO data)
        {
            try
            {
             
                 
              
            }
            catch (Exception ex)
            {
                data.message = "You Can Not Deactivate This Record Its Already Mapped";
                _areaimpl.LogInformation("Transport Error Master Area activedeactive" + ex.Message);
            }
            return data;
        }
    }
}
