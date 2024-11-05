using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.Transport;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Server;
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
    public class BusRoutesDetailsImpl : Interfaces.BusRoutesDetailsInterface
    {
        public TransportContext _context;
        ILogger<BusRoutesDetailsImpl> _areaimpl;
        public BusRoutesDetailsImpl(ILogger<BusRoutesDetailsImpl> areaimpl, TransportContext context)
        {

            _areaimpl = areaimpl;
            _context = context;

        }

        public BusRoutesDetailsDTO getdata(int id)
        {
            BusRoutesDetailsDTO data = new BusRoutesDetailsDTO();
            data.MI_Id = id;
            try
            {
                List<AcademicYear> allyear = new List<AcademicYear>();
                allyear = _context.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == id ).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.YearList = allyear.Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

               
                data.seclist =  _context.School_M_Section.Where(t => t.MI_Id == id && t.ASMC_ActiveFlag == 1).Distinct().OrderBy(t=>t.ASMC_Order).ToArray();


                data.classlist =  _context.School_M_Class.Where(t => t.MI_Id == id && t.ASMCL_ActiveFlag == true).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();



            }
            catch (Exception ex)
            {
                Console.WriteLine("Transport Error Driver Char savedata" + ex.Message);
            }

            return data;
        }


        public BusRoutesDetailsDTO Getreportdetails(BusRoutesDetailsDTO data)

        {
            try
            {
                data.messagelist = (from b in _context.MasterRouteDMO
                                    where (b.MI_Id == data.MI_Id)
                                    select new BusRoutesDetailsDTO
                                    {
                                        TRMR_Id = b.TRMR_Id,
                                        TRMR_RouteName = b.TRMR_RouteName

                                    }).Distinct().ToArray();


                data.classdata = _context.School_M_Class.Where(t => t.MI_Id == data.MI_Id).ToArray();

                //data.griddata = (from a in _context.School_Adm_Y_StudentDMO
                //                 from b in _context.Adm_Student_Transport_ApplicationDMO
                //                 where (b.MI_Id == data.MI_Id && a.AMST_Id == b.AMST_Id && a.ASMAY_Id == b.ASTA_FutureAY && b.ASTA_FutureAY==data.ASMAY_Id)
                //                 select new
                //                 {
                //                     TRMR_Id =,
                //                     classid = a.ASMCL_Id,
                //                     stud_count = a.AMST_Id
                //                 }).Distinct().GroupBy(id => new { id.classid, id.TRMR_Id }).Select(g => new BusRoutesDetailsDTO
                //                 { ASMCL_Id = g.Key.classid, stud_count = g.Count(), TRMR_Id = g.Key.TRMR_Id }).ToArray();

                if (data.type == "stdcount")
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Transport_Route_ClassWise_Count_Report_new";
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
                        cmd.Parameters.Add(new SqlParameter("@flag",
                         SqlDbType.VarChar)
                        {
                            Value = data.flag
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


                if (data.type == "stdstddetails")
                {


                    List<long> secid1 = new List<long>();
                    List<long> clsid1 = new List<long>();
                    foreach (var item in data.secidlist)
                    {
                        secid1.Add(item.ASMS_Id);
                        
                    }
                    foreach (var item in data.clsidlist)
                    {
                        clsid1.Add(item.ASMCL_Id);
                       
                    }



                    string clsidss = "0";
                    string secidss = "0";
                    //foreach (var item in grps)
                    //{
                    //    grpid.Add(item.FMG_Id);
                    //}

                    for (int r = 0; r < clsid1.Count(); r++)
                    {
                        clsidss = clsidss + ',' + clsid1[r];
                    }
                    
                    for (int y = 0; y < secid1.Count(); y++)
                    {
                        secidss = secidss + ',' + secid1[y];
                    }



                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Transport_Route_ClassWise_Count_Report_Details";
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
                        cmd.Parameters.Add(new SqlParameter("@flag",
                         SqlDbType.VarChar)
                        {
                            Value = data.flag
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                        {
                            Value = clsidss
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                        SqlDbType.VarChar)
                        {
                            Value = secidss
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
                            data.studentgriddata = retObject.ToArray();
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
    }
}
