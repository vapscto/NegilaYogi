using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace TransportServiceHub.Services
{
    public class RouteTermFeeDetailsImpl : Interfaces.RouteTermFeeDetailsInterface
    {
        private static ConcurrentDictionary<string, RouteTermFeeDetailsDTO> _login =
        new ConcurrentDictionary<string, RouteTermFeeDetailsDTO>();
        public DomainModelMsSqlServerContext _db;
        public TransportContext _context;
        ILogger<RouteTermFeeDetailsImpl> _areaimpl;
        public RouteTermFeeDetailsImpl(ILogger<RouteTermFeeDetailsImpl> areaimpl, TransportContext context, DomainModelMsSqlServerContext db)
        {

            _areaimpl = areaimpl;
            _context = context;
            _db = db;
        }

        public RouteTermFeeDetailsDTO getdata(int id)
        {
            RouteTermFeeDetailsDTO data = new RouteTermFeeDetailsDTO();
            data.MI_Id = id;
            try
            {
                List<AcademicYear> allyear = new List<AcademicYear>();
                allyear = _context.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == id && y.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.YearList = allyear.Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();


                //data.termlist = _context.FeeTerms.Where(t => t.MI_Id == data.MI_Id && t.FMT_ActiveFlag==true).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Transport Error Driver Char savedata" + ex.Message);
            }

            return data;
        }


        public RouteTermFeeDetailsDTO Getreportdetails(RouteTermFeeDetailsDTO data)

        {
            try
            {
                data.messagelist = (from b in _context.MasterRouteDMO
                                    where (b.MI_Id == data.MI_Id)
                                    select new RouteTermFeeDetailsDTO
                                    {
                                        TRMR_Id = b.TRMR_Id,
                                        TRMR_RouteName = b.TRMR_RouteName

                                    }).Distinct().ToArray();
                data.termlist = _context.FeeTerms.Where(t => t.MI_Id == data.MI_Id && t.FMT_ActiveFlag == true).ToArray();

                data.termlist = (from a in _context.FeeMasterTermHeadsDMO
                                 from b in _context.FeeTerms
                                 from c in _context.FeeHeadDMO
                                 where (a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && b.FMT_ActiveFlag == true && a.FMH_Id == c.FMH_Id && c.FMH_Flag == "T" && a.FMT_Id == b.FMT_Id)
                                 select b).Distinct().ToArray();


                //data.cdeposit = (from b in _context.feestudentstatus
                //                 from a in _context.TR_Student_RouteDMO
                //                 from c in _context.MasterRouteDMO
                //                 from d in _context.FeeHead
                //                 where (a.MI_Id == b.MI_Id && b.MI_Id == data.MI_Id && a.AMST_Id == b.AMST_Id && a.ASMAY_Id == b.ASMAY_Id && a.TRMR_Id == c.TRMR_Id && d.FMH_Id == b.FMH_Id && b.FMG_Id == a.FMG_Id)
                //                 group new { a, b, c, d }
                //                   by new { a.TRMR_Id } into g
                //                 select new RouteTermFeeDetailsDTO
                //                 {

                //                     TRMR_Id = g.FirstOrDefault().a.TRMR_Id,
                //                     TRMR_RouteName = g.FirstOrDefault().c.TRMR_RouteName,
                //                     FSS_ToBePaid = g.Sum(d => d.b.FSS_ToBePaid),
                //                     FSS_PaidAmount = g.Sum(d => d.b.FSS_PaidAmount) + g.Sum(d => d.b.FSS_ConcessionAmount),
                //                     FSS_TotalToBePaid = g.Sum(d => d.b.FSS_TotalToBePaid)

                //                 }).ToArray();

                List<RouteTermFeeDetailsDTO> result = new List<RouteTermFeeDetailsDTO>();
                List<RouteTermFeeDetailsDTO> result1 = new List<RouteTermFeeDetailsDTO>();
                if (data.checkdate == false)
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TRN_CD_FEE";

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



                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();

                        try
                        {

                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    result.Add(new RouteTermFeeDetailsDTO
                                    {
                                        TRMR_Id = Convert.ToInt32(dataReader["TRMR_Id"].ToString()),
                                        // TRMR_RouteName = ((dataReader["TRMR_RouteName"].ToString() == null ? " " : dataReader["TRMR_RouteName"].ToString())).Trim(),

                                        FSS_ToBePaid = Convert.ToInt64(dataReader["FSS_ToBePaid"].ToString()),
                                        FSS_PaidAmount = Convert.ToInt64(dataReader["FSS_PaidAmount"].ToString()),


                                        FSS_TotalToBePaid = Convert.ToInt64(dataReader["FSS_TotalToBePaid"].ToString()),


                                    });
                                    data.cdeposit = result.Distinct().ToArray();
                                    //data.savelist = result.Distinct().ToList();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "TRN_TERMWISE_FEE";

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



                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var retObject = new List<dynamic>();

                        try
                        {

                            using (var dataReader = cmd.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    result1.Add(new RouteTermFeeDetailsDTO
                                    {
                                        FMT_Id = Convert.ToInt32(dataReader["FMT_Id"].ToString()),
                                        TRMR_Id = Convert.ToInt32(dataReader["TRMR_Id"].ToString()),
                                        //  TRMR_RouteName = ((dataReader["TRMR_RouteName"].ToString() == null ? " " : dataReader["TRMR_RouteName"].ToString())).Trim(),

                                        FSS_ToBePaid = Convert.ToInt64(dataReader["FSS_ToBePaid"].ToString()),
                                        FSS_PaidAmount = Convert.ToInt64(dataReader["FSS_PaidAmount"].ToString()),


                                        FSS_TotalToBePaid = Convert.ToInt64(dataReader["FSS_TotalToBePaid"].ToString()),


                                    });
                                    data.griddata = result1.ToArray();
                                    //data.savelist = result.Distinct().ToList();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }



                if (data.checkdate == true)
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "DATEWISE_TRN_CD_FEE";

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
                                    SqlDbType.Date)
                        {
                            Value = data.frmdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@todate",
                                    SqlDbType.Date)
                        {
                            Value = data.todate
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
                                    result.Add(new RouteTermFeeDetailsDTO
                                    {
                                        TRMR_Id = Convert.ToInt32(dataReader["TRMR_Id"].ToString()),
                                        // TRMR_RouteName = ((dataReader["TRMR_RouteName"].ToString() == null ? " " : dataReader["TRMR_RouteName"].ToString())).Trim(),

                                        FSS_ToBePaid = Convert.ToInt64(dataReader["FSS_ToBePaid"].ToString()),
                                        FSS_PaidAmount = Convert.ToInt64(dataReader["FSS_PaidAmount"].ToString()),


                                        FSS_TotalToBePaid = Convert.ToInt64(dataReader["FSS_TotalToBePaid"].ToString()),


                                    });
                                    data.cdeposit = result.Distinct().ToArray();
                                    //data.savelist = result.Distinct().ToList();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "DATEWISE_TRN_TERMWISE_FEE";

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
                                    SqlDbType.Date)
                        {
                            Value = data.frmdate
                        });
                        cmd.Parameters.Add(new SqlParameter("@todate",
                                    SqlDbType.Date)
                        {
                            Value = data.todate
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
                                    result1.Add(new RouteTermFeeDetailsDTO
                                    {
                                        FMT_Id = Convert.ToInt32(dataReader["FMT_Id"].ToString()),
                                        TRMR_Id = Convert.ToInt32(dataReader["TRMR_Id"].ToString()),
                                        //  TRMR_RouteName = ((dataReader["TRMR_RouteName"].ToString() == null ? " " : dataReader["TRMR_RouteName"].ToString())).Trim(),

                                        FSS_ToBePaid = Convert.ToInt64(dataReader["FSS_ToBePaid"].ToString()),
                                        FSS_PaidAmount = Convert.ToInt64(dataReader["FSS_PaidAmount"].ToString()),


                                        FSS_TotalToBePaid = Convert.ToInt64(dataReader["FSS_TotalToBePaid"].ToString()),


                                    });
                                    data.griddata = result1.ToArray();
                                    //data.savelist = result.Distinct().ToList();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }







                //data.griddata = (from b in _context.feestudentstatus
                //                 from a in _context.TR_Student_RouteDMO
                //                 from c in _context.MasterRouteDMO
                //                 from d in _context.FeeHead
                //                 where (a.MI_Id == b.MI_Id && a.AMST_Id == b.AMST_Id && a.ASMAY_Id == b.ASMAY_Id && a.TRMR_Id == c.TRMR_Id && d.FMH_Id != b.FMH_Id && b.FMG_Id == a.FMG_Id)
                //                 group new { a, b, c, d }
                //                   by new { a.TRMR_Id, d.FMT_Id } into g
                //                 select new RouteTermFeeDetailsDTO
                //                 {
                //                     FMT_Id = g.FirstOrDefault().d.FMT_Id,
                //                     TRMR_Id = g.FirstOrDefault().a.TRMR_Id,
                //                     TRMR_RouteName = g.FirstOrDefault().c.TRMR_RouteName,
                //                     FSS_ToBePaid = g.Sum(d => d.b.FSS_ToBePaid),
                //                     FSS_PaidAmount = g.Sum(d => d.b.FSS_PaidAmount) + g.Sum(d => d.b.FSS_ConcessionAmount),
                //                     FSS_TotalToBePaid = g.Sum(d => d.b.FSS_TotalToBePaid),


                //                 }).ToArray();







            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}

