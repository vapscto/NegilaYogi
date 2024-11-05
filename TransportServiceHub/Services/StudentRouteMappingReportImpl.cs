using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.admission;
using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.Birthday;
using Microsoft.AspNetCore.Identity;
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
using System.Threading.Tasks;

namespace TransportServiceHub.Services
{
    public class StudentRouteMappingReportImpl : Interfaces.StudentRouteMappingReportInterface
    {
        private static ConcurrentDictionary<string, StudentRouteMappingReportDTO> _login =
        new ConcurrentDictionary<string, StudentRouteMappingReportDTO>();

        public TransportContext _context;
        ILogger<StudentRouteMappingReportImpl> _areaimpl;
        public StudentRouteMappingReportImpl(ILogger<StudentRouteMappingReportImpl> areaimpl, TransportContext context)
        {

            _areaimpl = areaimpl;
            _context = context;

        }

        public StudentRouteMappingReportDTO getdata(int id)
        {
            StudentRouteMappingReportDTO data = new StudentRouteMappingReportDTO();
            data.MI_Id = id;
            try
            {
                List<AcademicYear> allyear = new List<AcademicYear>();
                allyear = _context.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == id ).OrderByDescending(t=>t.ASMAY_Order).ToList();
                data.YearList = allyear.Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                data.routename = _context.MasterRouteDMO.Where(a => a.MI_Id == id && a.TRMR_ActiveFlg==true).OrderBy(d=>d.TRMR_order).ToArray();

                data.seclist = _context.School_M_Section.Where(t => t.MI_Id == id && t.ASMC_ActiveFlag == 1).Distinct().OrderBy(t => t.ASMC_Order).ToArray();


                data.classlist = _context.School_M_Class.Where(t => t.MI_Id == id && t.ASMCL_ActiveFlag == true).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();

                data.sessionlist = _context.MsterSessionDMO.Where(t => t.MI_Id == id && t.TRMS_ActiveFlg == true).Distinct().OrderByDescending(t => t.TRMS_Flag).ToArray();




                data.grouplist = (from a in _context.FeeHeadDMO
                                  from b in _context.FeeYearlygroupHeadMappingDMO
                                  from c in _context.FeeGroupDMO
                                  where (a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && a.FMH_ActiveFlag == true && a.FMH_Flag == "T" && a.FMH_Id == b.FMH_Id && b.FMG_Id == c.FMG_Id && c.FMG_ActiceFlag == true && b.FYGHM_ActiveFlag == "1")
                                  select c).Distinct().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Transport Error Driver Char savedata" + ex.Message);
            }

            return data;
        }
        public StudentRouteMappingReportDTO Getreportdetails(StudentRouteMappingReportDTO data)
        {

            try
            {
                string classids = "0";
                string secids = "0";
                string grpids = "0";
                string routeds = "0";
                if (data.TRMR_Id > 0)
                {
                    routeds = data.TRMR_Id.ToString();
                }
                else
                {
                  
                    var routelist = _context.MasterRouteDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMR_ActiveFlg == true).Select( d=>new StudentRouteMappingReportDTO {TRMR_Id= d.TRMR_Id
                    } ).Distinct().ToList();

                    for (int d = 0; d < routelist.Count(); d++)
                    {
                        routeds = routeds + ',' + routelist[d].TRMR_Id;
                    }



                }

                if (data.ASMCL_Id > 0)
                {
                    classids = data.ASMCL_Id.ToString();
                }
                else
                {
                   
                    var classsellist = _context.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true ).Select(d => new StudentRouteMappingReportDTO
                    {
                        ASMCL_Id = d.ASMCL_Id
                    }).Distinct().ToList();

                    for (int d = 0; d < classsellist.Count(); d++)
                    {
                        classids = classids + ',' + classsellist[d].ASMCL_Id;
                    }

                }

                if (data.ASMS_Id > 0)
                {
                    secids = data.ASMS_Id.ToString();
                }
                else
                {
                   
                    var secsellist = _context.School_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).Select(d => new StudentRouteMappingReportDTO
                    {
                        ASMS_Id = d.ASMS_Id
                    }).Distinct().ToList();

                    for (int d = 0; d < secsellist.Count(); d++)
                    {
                        secids = secids + ',' + secsellist[d].ASMS_Id;
                    }

                }

                if (data.feeflag==true)
                {
                    if (data.FMG_Id > 0)
                    {
                        grpids = data.FMG_Id.ToString();
                    }
                    else
                    {

                        var grouplist1 = (from a in _context.FeeHeadDMO
                                          from b in _context.FeeYearlygroupHeadMappingDMO
                                          from c in _context.FeeGroupDMO
                                          where (a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && a.FMH_ActiveFlag == true && a.FMH_Flag == "T" && a.FMH_Id == b.FMH_Id && b.FMG_Id == c.FMG_Id && c.FMG_ActiceFlag == true && b.FYGHM_ActiveFlag == "1" && b.ASMAY_Id == data.ASMAY_Id)
                                          select new StudentRouteMappingReportDTO
                                          {
                                              FMG_Id = c.FMG_Id
                                          }).Distinct().ToList();


                        for (int d = 0; d < grouplist1.Count(); d++)
                        {
                            grpids = grpids + ',' + grouplist1[d].FMG_Id;
                        }

                    }

                }

               

                List<TransportStatusReportDTO> result3 = new List<TransportStatusReportDTO>();
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TR_STUDENTROUTEMAPPINGREPORT";
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
                    cmd.Parameters.Add(new SqlParameter("@TRMR_Id",
                     SqlDbType.VarChar)
                    {
                        Value = routeds
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                     SqlDbType.VarChar)
                    {
                        Value = classids
                    });

                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                     SqlDbType.VarChar)
                    {
                        Value = secids
                    });
                    cmd.Parameters.Add(new SqlParameter("@AppStatus",
                     SqlDbType.VarChar)
                    {
                        Value = data.ASTA_ApplStatus
                    });
                    cmd.Parameters.Add(new SqlParameter("@feeflag",
                     SqlDbType.Bit)
                    {
                        Value = data.feeflag
                    });
                    cmd.Parameters.Add(new SqlParameter("@FMG_Id",
                     SqlDbType.VarChar)
                    {
                        Value = grpids
                    });
                    cmd.Parameters.Add(new SqlParameter("@TRMS_Id",
                     SqlDbType.VarChar)
                    {
                        Value = data.TRMS_Id
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
                        data.messagelist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            
          

            return data;
        }

    }
}
