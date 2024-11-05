using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.COE;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.FrontOffice;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace CoeServiceHub.com.vaps.Services
{
    public class COEReportImpl : Interfaces.COEReportInterface
    {
        public DomainModelMsSqlServerContext _db;
        public COEContext _coe;
        public FOContext _FOContext;
        public COEReportImpl(DomainModelMsSqlServerContext db, COEContext coe, FOContext FOContext)
        {
            _db = db;
            _coe = coe;
            _FOContext = FOContext;

        }
        public async Task<COEReportDTO> getinitialData(int id)
        {
            COEReportDTO dto = new COEReportDTO();
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
                dto.fillmonth = (from a in query
                                 select new EmployeeLogReportDTO
                                 {
                                     monthid = Convert.ToInt32(a.monthid),
                                     monthname = a.monthname
                                 }).Distinct().OrderBy(t => t.monthid).ToArray();


                dto.fillyear = (from a in _db.AcademicYear
                                where (a.MI_Id == id && a.ASMAY_ActiveFlag == 1)
                                select new HR_Master_LeaveYearDTO
                                {
                                    HRMLY_Id = Convert.ToInt32(a.ASMAY_Id),
                                    HRMLY_LeaveYear = a.ASMAY_Year,
                                    ASMAY_From_Date = a.ASMAY_From_Date,
                                    ASMAY_To_Date = a.ASMAY_To_Date
                                }).Distinct().OrderByDescending(t => t.HRMLY_LeaveYear).ToArray();

                dto.ClassList = _db.School_M_Class.Where(d => d.MI_Id == id && d.ASMCL_ActiveFlag == true).
               Select(d => new AdmissionClass
               {
                   ASMCL_Id = d.ASMCL_Id,
                   ASMCL_ClassName = d.ASMCL_ClassName
               }).ToArray();

                return dto;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dto;
        }
        public COEReportDTO getReport(COEReportDTO data)
        {
            try
            {
                if (data.type.Equals("1"))
                {
                    //data.coereport = (from m in _coe.COE_Master_EventsDMO
                    //                  from n in _coe.COE_EventsDMO
                    //                  where m.COEME_Id == n.COEME_Id && n.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && n.COEE_EStartDate.Value.Month == data.month && n.COEE_ActiveFlag == true && m.COEME_ActiveFlag == true
                    //                  select new COEReportDTO
                    //                  {
                    //                      eventName = m.COEME_EventName,
                    //                      eventDesc = m.COEME_EventDesc,
                    //                      COEE_EStartDate = n.COEE_EStartDate,
                    //                      COEE_EEndDate = n.COEE_EEndDate
                    //                  }).Distinct().OrderBy(t => t.COEE_EStartDate).ToArray();



                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "StaffCOEEventDetails";
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
                        cmd.Parameters.Add(new SqlParameter("@MonthId",
                        SqlDbType.VarChar)
                        {
                            Value = data.month

                        });
                        cmd.Parameters.Add(new SqlParameter("@type",
                        SqlDbType.VarChar)
                        {
                            Value = "staff"

                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                        SqlDbType.VarChar)
                        {
                            Value = 0
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
                            data.coereport = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    if (data.coereport.Length > 0)
                    {
                        data.count = data.coereport.Length;
                    }
                    else
                    {
                        data.count = 0;
                    }

                }
                else if (data.type.Equals("2"))
                {
                    data.coereport = (from m in _coe.COE_Master_EventsDMO
                                      from n in _coe.COE_EventsDMO
                                      from o in _coe.COE_Events_ClassesDMO
                                      where m.COEME_Id == n.COEME_Id && n.COEE_Id == o.COEE_Id && n.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && o.ASMCL_Id == data.ASMCL_Id && m.COEME_ActiveFlag == true && n.COEE_ActiveFlag == true
                                      select new COEReportDTO
                                      {
                                          eventName = m.COEME_EventName,
                                          eventDesc = m.COEME_EventDesc,
                                          COEE_EStartDate = n.COEE_EStartDate,
                                          COEE_EEndDate = n.COEE_EEndDate
                                      }).Distinct().OrderBy(t => t.COEE_EStartDate).ToArray();
                    if (data.coereport.Length > 0)
                    {
                        data.count = data.coereport.Length;
                    }
                    else
                    {
                        data.count = 0;
                    }
                }
                else if (data.type.Equals("3"))
                {
                    data.coereport = (from m in _coe.COE_Master_EventsDMO
                                      from n in _coe.COE_EventsDMO
                                      where m.COEME_Id == n.COEME_Id && n.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && n.COEE_EStartDate.Value.Date >= Convert.ToDateTime(data.COEE_EStartDate) && n.COEE_EEndDate.Value.Date <= Convert.ToDateTime(data.COEE_EEndDate) && m.COEME_ActiveFlag == true && n.COEE_ActiveFlag == true
                                      select new COEReportDTO
                                      {
                                          eventName = m.COEME_EventName,
                                          eventDesc = m.COEME_EventDesc,
                                          COEE_EStartDate = n.COEE_EStartDate,
                                          COEE_EEndDate = n.COEE_EEndDate
                                      }).Distinct().OrderBy(t => t.COEE_EStartDate).ToArray();

                    if (data.coereport.Length > 0)
                    {
                        data.count = data.coereport.Length;
                    }
                    else
                    {
                        data.count = 0;
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public COEReportDTO mothreport(COEReportDTO data)
        {
            try
            {
                data.coereport = (from m in _coe.COE_Master_EventsDMO
                                  from n in _coe.COE_EventsDMO
                                  where m.COEME_Id == n.COEME_Id && n.MI_Id == data.MI_Id && n.ASMAY_Id == data.year && n.COEE_EStartDate.Value.Month == data.month && m.COEME_ActiveFlag == true && n.COEE_ActiveFlag == true
                                  select new COEReportDTO
                                  {
                                      eventName = m.COEME_EventName,
                                      eventDesc = m.COEME_EventDesc,
                                      COEE_EStartDate = n.COEE_EStartDate,
                                      COEE_EEndDate = n.COEE_EEndDate
                                  }).Distinct().OrderBy(t => t.COEE_EStartDate).ToArray();

                if (data.coereport.Length > 0)
                {
                    data.count = data.coereport.Length;
                }
                else
                {
                    data.count = 0;
                }

                data.eventDesc = _db.IVRM_Email_sentBox.Where(q => q.MI_Id == data.MI_Id && q.Datetime.Value.Month == data.month && q.Module_Name == "Calendar of Event").Count().ToString();

                data.eventName = _db.IVRM_sms_sentBoxDMO.Where(w => w.Datetime.Month == data.month && w.MI_Id == data.MI_Id && w.Module_Name == "Calendar of Event").Count().ToString();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

    }
}
