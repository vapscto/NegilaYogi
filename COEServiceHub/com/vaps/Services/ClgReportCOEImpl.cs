using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.College.COE;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.College.COE;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace COEServiceHub.com.vaps.Services
{
    public class ClgReportCOEImpl : Interfaces.ClgReportCOEInterface
    {
        private static ConcurrentDictionary<string, ClgMasterCOEDTO> _login =
        new ConcurrentDictionary<string, ClgMasterCOEDTO>();


        public ClgCOEContext _ClgCOEContext;
        public DomainModelMsSqlServerContext _db;
        public ClgReportCOEImpl(ClgCOEContext coe, DomainModelMsSqlServerContext db)
        {
            _db = db;
            _ClgCOEContext = coe;
        }

        public ClgMasterCOEDTO getinitialData(int id)
        {
            ClgMasterCOEDTO data = new ClgMasterCOEDTO();
            try
            {


                var q = (from a in _db.month
                         where (a.Is_Active == true)
                         select new
                         {
                             monthid = a.IVRM_Month_Id,
                             monthname = a.IVRM_Month_Name,
                         }).Distinct().ToArray();

                var query = q.Distinct().ToArray();
                data.fillmonth = (from a in query
                                  select new ClgMasterCOEDTO
                                  {
                                      monthid = Convert.ToInt32(a.monthid),
                                      monthname = a.monthname
                                  }).Distinct().OrderBy(t => t.monthid).ToArray();

                data.fillyear = (from a in _ClgCOEContext.AcademicYear
                                 where (a.MI_Id == id && a.ASMAY_ActiveFlag == 1)
                                 select new ClgMasterCOEDTO
                                 {
                                     ASMAY_Id = Convert.ToInt32(a.ASMAY_Id),
                                     ASMAY_Year = a.ASMAY_Year
                                 }
       ).Distinct().ToArray();

                data.course_rp = _ClgCOEContext.MasterCourseDMO.Where(d => d.MI_Id == id && d.AMCO_ActiveFlag == true).Distinct().OrderBy(c => c.AMCO_Order).ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public async Task<ClgMasterCOEDTO> getReport(ClgMasterCOEDTO data)
        {
            try
            {
                string ids = "0";
                if (data.AMCO_Ids != null)
                {
                    foreach (var g in data.AMCO_Ids)
                    {
                        ids = ids + "," + g.AMCO_Id;
                    }
                }
                using (var cmd = _ClgCOEContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "CLG_COEREPORT";
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
                    cmd.Parameters.Add(new SqlParameter("@AMCO_Ids",
                   SqlDbType.VarChar)
                    {
                        Value = ids
                    });
                    cmd.Parameters.Add(new SqlParameter("@month",
                    SqlDbType.BigInt)
                    {
                        Value = data.monthid
                    });
                    cmd.Parameters.Add(new SqlParameter("@typeflag",
                  SqlDbType.VarChar)
                    {
                        Value = data.typeflag
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
                        data.coereport = retObject.ToArray();
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

        public ClgMasterCOEDTO mothreport(ClgMasterCOEDTO data)
        {
            try
            {
                data.coereport = (from m in _ClgCOEContext.COE_Master_EventsDMO
                                  from n in _ClgCOEContext.COE_EventsDMO
                                  where m.COEME_Id == n.COEME_Id && n.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && n.COEE_EStartDate.Value.Month == data.monthid
                                  select new ClgMasterCOEDTO
                                  {
                                      COEME_EventName = m.COEME_EventName,
                                      COEME_EventDesc = m.COEME_EventDesc,
                                      COEE_EStartDate = n.COEE_EStartDate,
                                      COEE_EEndDate = n.COEE_EEndDate
                                  }).ToArray();
                if (data.coereport.Length > 0)
                {
                    data.count = data.coereport.Length;
                }
                else
                {
                    data.count = 0;
                }

                data.eventDesc = _db.IVRM_Email_sentBox.Where(q => q.MI_Id == data.MI_Id && q.Datetime.Value.Month == data.monthid && q.Module_Name == "College COE").Count().ToString();
                data.eventName = _db.IVRM_sms_sentBoxDMO.Where(w => w.Datetime.Month == data.monthid && w.MI_Id == data.MI_Id && w.Module_Name == "College COE").Count().ToString();
                data.yearName = _db.Institution.Where(s => s.MI_Id == data.MI_Id).Select(d => d.MI_Logo).FirstOrDefault();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

    }
}
