using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.Portals.IVRS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.IVRS.Services
{
    public class CallDashboardImpl:Interfaces.CallDashboardInterface
    {
        public DomainModelMsSqlServerContext _db;
        public PortalContext _ivrs;

        public CallDashboardImpl(DomainModelMsSqlServerContext a, PortalContext b)
        {
            _db = a;
            _ivrs = b;
        }
        public async Task<CallDashboardDTO> loadData(CallDashboardDTO data)
        {
            try
            {
                var sub1_name = (from a in _db.VirtualSchool
                                from b in _db.IVRS_ConfigurationDMO
                                where (a.IVRM_MI_Id == b.IIVRSC_MI_Id && a.IVRM_MI_Id == data.MI_Id)
                                select new CallDashboardDTO
                                {
                                    IVRM_Sub_Domain_Name = a.IVRM_Sub_Domain_Name,

                                }).Distinct().ToList();
                data.IVRM_Sub_Domain_Name = sub1_name[0].IVRM_Sub_Domain_Name;
                var sub2_name = (from a in _db.VirtualSchool
                                 from b in _db.IVRS_ConfigurationDMO
                                 where (a.IVRM_MI_Id == b.IIVRSC_MI_Id && a.IVRM_MI_Id == data.MI_Id&&a.IVRM_Sub_Domain_Name==b.IVRSC_IVRM_Sub_Domain_Name&&b.IVRSC_IVRM_Sub_Domain_Name==data.IVRM_Sub_Domain_Name)
                                 select new CallDashboardDTO
                                 {
                                     IIVRSC_VirtualNo = b.IIVRSC_VirtualNo
                                 }).Distinct().ToList();

                if (sub2_name.Count>0)
                {
                    data.flag = "valid";
                    data.IIVRSC_VirtualNo = sub2_name[0].IIVRSC_VirtualNo;
                    data.reportlist = (from a in _ivrs.IVRS_Call_StatusDMO
                                       from b in _ivrs.IVRM_IVRS_ConfigurationDMO
                                       where (a.IMCS_MI_Id == data.MI_Id && a.IMCS_VirtualNo == b.IIVRSC_VirtualNo && b.IVRSC_IVRM_Sub_Domain_Name == data.IVRM_Sub_Domain_Name)
                                       select new CallDashboardDTO
                                       {
                                           IMCS_AssignedCall = a.IMCS_AssignedCall,
                                           IMCS_InboundCalls = a.IMCS_InboundCalls,
                                           IMCS_OutboundCalls = a.IMCS_OutboundCalls,
                                           IMCS_AvailableCalls = a.IMCS_AvailableCalls,
                                       }).ToArray();
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRS_CALLDASHBOARD";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt) { Value = data.MI_Id });


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
                            data.reportdatelist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRS_CALLDASHBOARD2";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@mi_id", SqlDbType.BigInt) { Value = data.MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@virtualno", SqlDbType.VarChar) { Value = data.IIVRSC_VirtualNo });
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
                            data.reportdatelist2 = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    return data;
                }
                if (sub2_name.Count==0)
                {
                    data.flag = "invalid";
                    data.IMCS_AssignedCall = 0;
                    data.IMCS_AvailableCalls = 0;
                    data.IMCS_InboundCalls = 0;
                    data.IMCS_OutboundCalls = 0;
                    data.received = 0;
                    data.ConnectedCount = 0;
                    data.NotConnectedCount = 0;
                    data.sum = 0;
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
