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
using System.Dynamic;
using System.Linq;

namespace TransportServiceHub.Services
{
    public class CLGRouteSessionStrengthImpl : Interfaces.CLGRouteSessionStrengthInterface
    {
        private static ConcurrentDictionary<string, CLGRouteSessionStrengthDTO> _login =
      new ConcurrentDictionary<string, CLGRouteSessionStrengthDTO>();

        public TransportContext _context;
        ILogger<RouteSessionTotalStrengthImpl> _areaimpl;
        public CLGRouteSessionStrengthImpl(ILogger<RouteSessionTotalStrengthImpl> areaimpl, TransportContext context)
        {

            _areaimpl = areaimpl;
            _context = context;

        }

        public CLGRouteSessionStrengthDTO getdata(int id)
        {
            CLGRouteSessionStrengthDTO data = new CLGRouteSessionStrengthDTO();
            data.MI_Id = id;
            try
            {
                List<AcademicYear> allyear = new List<AcademicYear>();
                allyear = _context.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == id ).Distinct().OrderByDescending(t=>t.ASMAY_Order).ToList();
                data.YearList = allyear.Distinct().ToArray();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Transport Error" + ex.Message);
            }

            return data;
        }


        public CLGRouteSessionStrengthDTO Getreportdetails(CLGRouteSessionStrengthDTO data)

        {
            try
            {
                data.messagelist = (from b in _context.MasterRouteDMO
                                    where (b.MI_Id == data.MI_Id && b.TRMR_ActiveFlg==true)
                                    select new CLGRouteSessionStrengthDTO
                                    {
                                        TRMR_Id = b.TRMR_Id,
                                        TRMR_RouteName = b.TRMR_RouteName,
                                        TRMR_order = b.TRMR_order
                                    }).Distinct().OrderBy(t=>t.TRMR_order).ToArray();


                data.schedultime = _context.MsterSessionDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMS_ActiveFlg == true).ToArray();
                

                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TR_CLG_SESSIONWISE_BUSROUTE_COUNT";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                       SqlDbType.VarChar)
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
                        data.griddata = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



                var totaltrcount1 = (from a in _context.CLGStudentRouteMappingDMO
                                     from b in _context.Adm_Master_College_StudentDMO
                                     from c in _context.Adm_College_Yearly_StudentDMO
                                     where (a.MI_Id == data.MI_Id && a.TRRSCO_ActiveFlg == true
                                     && a.ASMAY_Id == data.ASMAY_Id && a.AMCST_Id==b.AMCST_Id && a.AMCST_Id == c.AMCST_Id && a.MI_Id==b.MI_Id && c.ASMAY_Id==a.ASMAY_Id && b.AMCST_ActiveFlag==true && b.AMCST_SOL=="S" && c.ACYST_ActiveFlag==1 )
                                     select new CLGRouteSessionStrengthDTO
                                     {
                                         AMCST_Id = a.AMCST_Id
                                     }
                               ).Distinct().ToList();

                data.totaltrcount = totaltrcount1.Count();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Transport Error" + ex.Message);
            }
            return data;
        }
        }
}
