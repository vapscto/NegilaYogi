using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
namespace WebApplication1.Services
{
    public class ScheduleReportImpl : Interfaces.ScheduleReportInterface
    {
        //public DomainModelMsSqlServerContext _Context;
        public ScheduleReportContext _ScheduleReportContext;
        private readonly DomainModelMsSqlServerContext _db;
        public ScheduleReportImpl(ScheduleReportContext DomainModelContext, DomainModelMsSqlServerContext db)
        {
            _ScheduleReportContext = DomainModelContext;
            _db = db;
        }
        public ScheduleReportDTO getdetails(ScheduleReportDTO data)
        {
            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _ScheduleReportContext.AcademicYear.Where(t => t.MI_Id == data.mid && t.Is_Active == true).OrderByDescending(d => d.ASMAY_Order).ToList();
                data.fillyear = year.ToArray();

                List<School_M_Class> classname = new List<School_M_Class>();
                classname = _ScheduleReportContext.admissioncls.Where(t => t.MI_Id == data.mid && t.ASMCL_ActiveFlag == true).ToList();
                data.fillclass = classname.ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public ScheduleReportDTO schedulelist(ScheduleReportDTO data)
        {
            try
            {
                if (data.oralwrittenscheduleflag == "oral")
                {
                    if (data.yearorbtwndates == "yearwise")
                    {
                        List<OralTestScheduleDMO> oral = new List<OralTestScheduleDMO>();

                        data.writentestlist = (from a in _ScheduleReportContext.oraltest
                                               from b in _ScheduleReportContext.oralstudents
                                               where (a.ASMAY_Id == data.asmay_id && a.PAOTS_Id == b.PAOTS_Id && a.MI_Id == data.mid && b.PAOTSS_StatusFlag == 1)
                                               select new ScheduleReportDTO
                                               {
                                                   disid = a.PAOTS_Id,
                                                   disname = a.PAOTS_ScheduleName
                                               }).Distinct().ToArray();
                    }
                    else
                    {
                        // List<OralTestScheduleDMO> oral = new List<OralTestScheduleDMO>();

                        data.writentestlist = (from a in _ScheduleReportContext.oraltest
                                               from b in _ScheduleReportContext.oralstudents
                                               where (a.ASMAY_Id == data.yearid && a.PAOTS_Id == b.PAOTS_Id && a.MI_Id == data.mid && b.PAOTSS_StatusFlag == 1)
                                               select new ScheduleReportDTO
                                               {
                                                   disid = a.PAOTS_Id,
                                                   disname = a.PAOTS_ScheduleName
                                               }).Distinct().ToArray();
                    }



                }
                else if (data.oralwrittenscheduleflag == "written")
                {

                    if (data.yearorbtwndates == "yearwise")
                    {
                        //  List<WrittenTestScheduleDMO> written = new List<WrittenTestScheduleDMO>();

                        data.writentestlist = (from a in _ScheduleReportContext.writentest
                                               where (a.ASMAY_Id == data.asmay_id && a.MI_Id == data.mid)
                                               select new ScheduleReportDTO
                                               {
                                                   disid = a.PAWTS_Id,
                                                   disname = a.PAWTS_ScheduleName

                                               }).Distinct().ToArray();
                    }
                    else
                    {
                        // List<WrittenTestScheduleDMO> written = new List<WrittenTestScheduleDMO>();

                        data.writentestlist = (from a in _ScheduleReportContext.writentest
                                               where (a.ASMAY_Id == data.yearid && a.MI_Id == data.mid)
                                               select new ScheduleReportDTO
                                               {
                                                   disid = a.PAWTS_Id,
                                                   disname = a.PAWTS_ScheduleName

                                               }).Distinct().ToArray();
                    }
                }
                else if (data.oralwrittenscheduleflag == "statusschedule")
                {
                    if (data.yearorbtwndates == "yearwise")
                    {
                        List<OralTestScheduleDMO> oral = new List<OralTestScheduleDMO>();

                        data.writentestlist = (from a in _ScheduleReportContext.oraltest
                                               from b in _ScheduleReportContext.oralstudents
                                               where (a.ASMAY_Id == data.asmay_id && a.PAOTS_Id == b.PAOTS_Id && a.MI_Id == data.mid && b.PAOTSS_StatusFlag == 3)
                                               select new ScheduleReportDTO
                                               {
                                                   disid = a.PAOTS_Id,
                                                   disname = a.PAOTS_ScheduleName
                                               }).Distinct().ToArray();
                    }
                    else
                    {
                        List<OralTestScheduleDMO> oral = new List<OralTestScheduleDMO>();

                        data.writentestlist = (from a in _ScheduleReportContext.oraltest
                                               from b in _ScheduleReportContext.oralstudents
                                               where (a.ASMAY_Id == data.yearid && a.PAOTS_Id == b.PAOTS_Id && a.MI_Id == data.mid && b.PAOTSS_StatusFlag == 3)
                                               select new ScheduleReportDTO
                                               {
                                                   disid = a.PAOTS_Id,
                                                   disname = a.PAOTS_ScheduleName
                                               }).Distinct().ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public async Task<ScheduleReportDTO> Getreportdetails(ScheduleReportDTO data)
        {
            try
            {
                string name = "";
                string IVRM_CLM_coloumn = "";

                using (var cmd = _ScheduleReportContext.Database.GetDbConnection().CreateCommand())
                {
                    if (data.stype == 2)
                    {
                        cmd.CommandText = "Preadmission_reschedulewise_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                    }
                    else if (data.stype == 1)
                    {
                        cmd.CommandText = "Preadmission_schedulewise_Report";
                        cmd.CommandType = CommandType.StoredProcedure;
                    }

                    if (data.fromdate == null || data.fromdate == "")
                    {
                        data.fromdate = DateTime.Now.ToString();
                    }
                    if (data.todate == null || data.todate == "")
                    {
                        data.todate = DateTime.Now.ToString();
                    }

                    cmd.Parameters.Add(new SqlParameter("@year",
                             SqlDbType.VarChar)
                    {
                        Value = data.asmayid
                    });

                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                               SqlDbType.DateTime)
                    {
                        Value = data.fromdate
                    });

                    cmd.Parameters.Add(new SqlParameter("@todate",
                            SqlDbType.DateTime)
                    {
                        Value = data.todate
                    });

                    cmd.Parameters.Add(new SqlParameter("@classid",
                        SqlDbType.VarChar)
                    {
                        Value = data.asmclid
                    });
                    cmd.Parameters.Add(new SqlParameter("@flag",
                   SqlDbType.VarChar)
                    {
                        Value = data.flagows
                    });
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
           SqlDbType.VarChar)
                    {
                        Value = data.mid
                    });

                    cmd.Parameters.Add(new SqlParameter("@schids",
         SqlDbType.VarChar)
                    {
                        Value = data.schids
                    });



                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();
                    try
                    {
                        // var data = cmd.ExecuteNonQuery();

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
                        data.allreports = retObject.ToArray();


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
        public async Task<ScheduleReportDTO> scheduleGetreportdetails(ScheduleReportDTO data)
        {
            try
            {
                using (var cmd = _ScheduleReportContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ViewPreadmission_schedulewise_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@schids",
                    SqlDbType.VarChar)
                    {
                        Value = data.PAOTS_Id 
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
                        data.schedulelist = retObject.ToArray();
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
        public async Task<ScheduleReportDTO> remarksGetreportdetails(ScheduleReportDTO data)
        {
            try
            {
                using (var cmd = _ScheduleReportContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "ViewPreadmission_Remarkswise_Report";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@schids",
                    SqlDbType.VarChar)
                    {
                        Value = data.PAOTS_Id 
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
                        data.remarkschedulelist = retObject.ToArray();
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
        public async Task<ScheduleReportDTO> sendmail(ScheduleReportDTO data)
        {
            List<ScheduleReportDTO> abc = new List<ScheduleReportDTO>();
            //foreach (ScheduleReportDTO mob in data.SmsMailStudentDetailst)
            //{

            for (int i = 0; i < data.SmsMailStudentDetailst.Count(); i++)
            {


                abc.Add(data.SmsMailStudentDetailst[i]);

                var regmo = abc[0].regno;
                var stuid = _ScheduleReportContext.student_registration.Where(d => d.PASR_RegistrationNo == regmo && d.MI_Id == data.mid).ToList();

                string emailid = stuid.FirstOrDefault().PASR_emailId;
                long stuidd = stuid.FirstOrDefault().Id;


                data.studetaarray = abc.ToArray();

                //Email.sendmailschedule(data.mid,"ORAL_TEST_SCHEDULE", data.studetaarray, emailid);

                Email Email = new Email(_db);

                Email.sendmail(data.mid, emailid, "ORAL_TEST_SCHEDULE", stuid.FirstOrDefault().pasr_id);

                SMS sms = new SMS(_db);
                await sms.sendSms(data.mid, stuid.FirstOrDefault().PASR_MobileNo, "ORAL_TEST_SCHEDULE", stuid.FirstOrDefault().pasr_id);
                abc.Clear();
            }
            return data;
        }
    }
}
