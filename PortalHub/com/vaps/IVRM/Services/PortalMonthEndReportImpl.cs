using PreadmissionDTOs.com.vaps.Portals.Student;
using System;
using System.Linq;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using DomainModel.Model.com.vapstech.Portals.Student;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using PreadmissionDTOs;
using AutoMapper;
using PreadmissionDTOs.com.vaps.Portals.IVRM;
using DomainModel.Model.com.vapstech.Portals.IVRM;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.admission;

namespace PortalHub.com.vaps.IVRM.Services
{
    public class PortalMonthEndReportImpl : Interfaces.PortalMonthEndReportInterface
    {
        private static ConcurrentDictionary<string, PortalMonthEndReportDTO> _login =
           new ConcurrentDictionary<string, PortalMonthEndReportDTO>();
        private PortalContext _PortalContext;
        public DomainModelMsSqlServerContext _context;
        public ClgAdmissionContext _ClgAdmissionContext;

        public PortalMonthEndReportImpl(PortalContext PortalContext, DomainModelMsSqlServerContext context, ClgAdmissionContext clgAdmissionContext)
        {
            _PortalContext = PortalContext;
            _context = context;
            _ClgAdmissionContext = clgAdmissionContext;
        }



        public PortalMonthEndReportDTO getloaddata(PortalMonthEndReportDTO data)
        {
            try
            {
                //List<AcademicYear> year = new List<AcademicYear>();
                //year = _PortalContext.AcademicYearDMO.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                //data.acayear = year.ToArray();
                //List<Month> mnth = new List<Month>();
                //mnth = _ClgAdmissionContext.mnth.Where(t => t.Is_Active == true).ToList();
                //data.Month_array = mnth.ToArray();

                var list = _PortalContext.AcademicYearDMO.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToList();
                data.acayear = list.ToArray();

                data.Month_array = _ClgAdmissionContext.mnth.Where(a => a.Is_Active == true).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<PortalMonthEndReportDTO> getmonthreport(PortalMonthEndReportDTO data)
        {
            try
            {
                //Portal_Month_End_Report_01 previous stored procedure
                var roleid = _PortalContext.IVRM_Role_Type.Single(r => r.IVRMRT_Role == data.roleflag || r.IVRMRT_RoleFlag == data.roleflag).IVRMRT_Id;
                using (var cmd = _PortalContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Portal_Month_End_Report_count";
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

                    cmd.Parameters.Add(new SqlParameter("@month",
                  SqlDbType.VarChar)
                    {
                        Value = data.month
                    });

                    cmd.Parameters.Add(new SqlParameter("@year",
                  SqlDbType.VarChar)
                    {
                        Value = data.year
                    });
                    cmd.Parameters.Add(new SqlParameter("@RoleId",
                SqlDbType.VarChar)
                    {
                        Value = roleid
                    });
                    cmd.Parameters.Add(new SqlParameter("@countflaguc",
               SqlDbType.VarChar)
                    {
                        Value = data.countflaguc
                    });
                    cmd.Parameters.Add(new SqlParameter("@countflaglc",
             SqlDbType.VarChar)
                    {
                        Value = data.countflaglc
                    });
                    cmd.Parameters.Add(new SqlParameter("@Moduleflag",
               SqlDbType.VarChar)
                    {
                        Value = data.Moduleflag
                    });
                    cmd.Parameters.Add(new SqlParameter("@Roleflag",
                SqlDbType.VarChar)
                    {
                        Value = data.roleflag
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
                        data.get_monthendreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
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
