using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.VisitorsManagement;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorsManagementServiceHub.Services
{
    public class V_AppointmentApprovalReportImpl:Interfaces.V_AppointmentApprovalReportInterface
    {
        public VisitorsManagementContext _visctxt;
        public DomainModelMsSqlServerContext _db;
        public V_AppointmentApprovalReportImpl(VisitorsManagementContext context, DomainModelMsSqlServerContext db)
        {
            _visctxt = context;
            _db = db;
        }
        public async Task<V_AppointmentApprovalReport_DTO> report(V_AppointmentApprovalReport_DTO data)
        {
            try
            {
                string mi_ids = "0";
                List<long> mid = new List<long>();


                mi_ids = data.MI_Id.ToString();
                if (data.all1 == "1")
                {
                    data.month_id = "";
                }
                else
                {
                    data.fromdate = "";
                    data.todate = "";
                }

                using (var cmd = _visctxt.Database.GetDbConnection().CreateCommand())
                {                    
                    cmd.CommandText = "APPOINTMENT_APPROVAL_STATUS_REPORT_IVRM";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.VarChar)
                    {
                        Value = mi_ids
                    });                   
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                  SqlDbType.VarChar)
                    {
                        Value = data.fromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
                  SqlDbType.VarChar)
                    {
                        Value = data.todate
                    });
                    cmd.Parameters.Add(new SqlParameter("@months",
                 SqlDbType.VarChar)
                    {
                        Value = data.month_id
                    });
                    cmd.Parameters.Add(new SqlParameter("@radiotype",
                 SqlDbType.VarChar)
                    {
                        Value = data.radiotype
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
                        data.viewlist = retObject.ToArray();

                    }


                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                data.yarname = (from z in _visctxt.AcademicYear
                                where (z.ASMAY_Id == data.ASMAY_Id && z.MI_Id == data.MI_Id)
                                select new V_AppointmentApprovalReport_DTO
                                {
                                    ASMAY_Year = z.ASMAY_Year
                                }).Distinct().ToArray();

            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public V_AppointmentApprovalReport_DTO loaddata(V_AppointmentApprovalReport_DTO data)
        {
            try
            {
                //var institutionlist = (from a in _visctxt.Institute
                //                       from b in _visctxt.UserRoleWithInstituteDMO
                //                       where (b.Id == data.UserId && b.MI_Id == a.MI_Id && b.Activeflag == 1 && a.MI_ActiveFlag == 1)
                //                       select a).Distinct().OrderBy(t => t.MI_Id).ToList();
                //data.institutionlist = institutionlist.ToArray();
                //if (data.MI_Id == 0)
                //{
                //    if (institutionlist.Count > 0)
                //    {
                //        data.MI_Id = institutionlist.FirstOrDefault().MI_Id;
                //    }
                //}

                long roletypeid = 0;
                string roletype = "";

                if (data.UserId > 0)
                {
                    //roletypeid = _db.appUserRole.Where(t => t.UserId == data.UserId).Distinct().FirstOrDefault().RoleTypeId;
                    //if (roletypeid > 0)
                    //{
                    //    roletype = _db.applicationRole.Where(t => t.Id == roletypeid).Distinct().FirstOrDefault().roleType;
                    //}
                    //if (roletype != "" && roletype != null)
                    //{
                    //    if (roletype.Equals("COORDINATOR", StringComparison.OrdinalIgnoreCase))
                    //    {
                    //        var institutionlist1 = (from a in _visctxt.Institute
                    //                                from b in _visctxt.UserRoleWithInstituteDMO
                    //                                where (b.Id == data.UserId && b.Activeflag == 1 && a.MI_ActiveFlag == 1)
                    //                                select a).Distinct().OrderBy(t => t.MI_Id).ToList();
                    //        data.institutionlist = institutionlist1.ToArray();

                    //        if (data.MI_Id == 0)
                    //        {
                    //            if (institutionlist1.Count > 0)
                    //            {
                    //                data.MI_Id = institutionlist1.FirstOrDefault().MI_Id;
                    //            }
                    //        }
                    //    }
                    //    else if (roletype.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                    //    {
                    //        var institutionlist2 = (from a in _visctxt.Institute
                    //                                from b in _visctxt.UserRoleWithInstituteDMO
                    //                                where (b.Id == data.UserId && b.Activeflag == 1 && a.MI_ActiveFlag == 1)
                    //                                select a).Distinct().OrderBy(t => t.MI_Id).ToList();
                    //        data.institutionlist = institutionlist2.ToArray();

                    //        if (data.MI_Id == 0)
                    //        {
                    //            if (institutionlist2.Count > 0)
                    //            {
                    //                data.MI_Id = institutionlist2.FirstOrDefault().MI_Id;
                    //            }
                    //        }
                    //    }
                    //    else if (roletype.Equals("HR", StringComparison.OrdinalIgnoreCase))
                    //    {
                    //        var institutionlist3 = (from a in _visctxt.Institute
                    //                                from b in _visctxt.UserRoleWithInstituteDMO
                    //                                where (b.Id == data.UserId && b.Activeflag == 1 && a.MI_ActiveFlag == 1)
                    //                                select a).Distinct().OrderBy(t => t.MI_Id).ToList();
                    //        data.institutionlist = institutionlist3.ToArray();

                    //        if (data.MI_Id == 0)
                    //        {
                    //            if (institutionlist3.Count > 0)
                    //            {
                    //                data.MI_Id = institutionlist3.FirstOrDefault().MI_Id;
                    //            }
                    //        }
                    //    }
                    //    else
                    //    {
                    //        var institutionlist = (from a in _visctxt.Institute
                    //                               from b in _visctxt.UserRoleWithInstituteDMO
                    //                               where (b.Id == data.UserId && b.MI_Id == a.MI_Id && b.Activeflag == 1 && a.MI_ActiveFlag == 1)
                    //                               select a).Distinct().OrderBy(t => t.MI_Id).ToList();
                    //        data.institutionlist = institutionlist.ToArray();
                    //        if (data.MI_Id == 0)
                    //        {
                    //            if (institutionlist.Count > 0)
                    //            {
                    //                data.MI_Id = institutionlist.FirstOrDefault().MI_Id;
                    //            }
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    var institutionlist = (from a in _visctxt.Institute
                    //                           from b in _visctxt.UserRoleWithInstituteDMO
                    //                           where (b.Id == data.UserId && b.MI_Id == a.MI_Id && b.Activeflag == 1 && a.MI_ActiveFlag == 1)
                    //                           select a).Distinct().OrderBy(t => t.MI_Id).ToList();
                    //    data.institutionlist = institutionlist.ToArray();
                    //    if (data.MI_Id == 0)
                    //    {
                    //        if (institutionlist.Count > 0)
                    //        {
                    //            data.MI_Id = institutionlist.FirstOrDefault().MI_Id;
                    //        }
                    //    }
                    //}
                }


                var q = (from a in _visctxt.holidaydate
                         where (a.MI_Id == data.MI_Id && a.FOMHWD_ActiveFlg == true)
                         select new
                         {
                             monthid = a.FOMHWDD_FromDate.Value.Month,
                             monthname = Convert.ToDateTime(a.FOMHWDD_FromDate).ToString("MMMMM").ToString()
                         }).Distinct().ToArray();

                var query = q.Distinct().ToArray();
                data.month_list = (from a in query
                                   select new V_AppointmentApprovalReport_DTO
                                   {
                                       monthid = a.monthid,
                                       monthname = a.monthname
                                   }).Distinct().OrderBy(t => t.monthid).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return data;
        }
        public V_AppointmentApprovalReport_DTO loaddatatoday(V_AppointmentApprovalReport_DTO data)
        {
            try
            {
                

                using (var cmd = _visctxt.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TODAYS_APPOINTMENT_LIST_IVRM";
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                  SqlDbType.Date)
                    {
                        Value = data.VMAP_MeetingDateTime
                    });
                    cmd.Parameters.Add(new SqlParameter("@UserId",
                  SqlDbType.VarChar)
                    {
                        Value = data.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                  SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                   
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader =  cmd.ExecuteReader())
                        {
                            while ( dataReader.Read())
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
                        data.viewlist = retObject.ToArray();

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
