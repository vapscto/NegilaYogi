using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.admission;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class StudentSmartCardLogReportImpl : Interfaces.StudentSmartCardLogReportInterface
    {
        public StudentYearLossReportContext _StudentTcReportContext;
        string IVRM_CLM_coloumn = "";
        public ILogger<StudentSmartCardLogReportImpl> _smartcard;
        public StudentSmartCardLogReportImpl(StudentYearLossReportContext frgContext, ILogger<StudentSmartCardLogReportImpl> _smart)
        {
            _StudentTcReportContext = frgContext;
            _smartcard = _smart;
        }
        public async Task<StudentSmartCardLogReportDTO> getdetails(StudentSmartCardLogReportDTO data)
        {
            try
            {
                string coloumns = "";
                coloumns = IVRM_CLM_coloumn.Remove(IVRM_CLM_coloumn.Length - 1);

                using (var cmd = _StudentTcReportContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Admission_SmartCard_LogReport_namebind";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@tableparam",
                        SqlDbType.VarChar)
                    {
                        Value = coloumns
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    //var data = cmd.ExecuteNonQuery();

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
                    data.alldatagridreport = retObject.ToArray();

                }

            }
            catch (Exception e)
            {
                _smartcard.LogInformation("Smart Card Log Report Error :'" + e.Message + "'");

            }
            return data;
        }


        public StudentSmartCardLogReportDTO getstuddet(StudentSmartCardLogReportDTO data)
        {
            try
            {
                if (data.regornamedetails == "regno")
                {
                    data.studentlist = (from a in _StudentTcReportContext.AdmissionStudent
                                        from b in _StudentTcReportContext.SchoolAdmYStudent
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmay_id && a.AMST_SOL == "S")
                                        select new StudentSmartCardLogReportDTO
                                        {
                                            Amst_Id = a.AMST_Id,
                                            AMST_FirstName = a.AMST_RegistrationNo + "-" + a.AMST_FirstName,
                                            AMST_MiddleName = a.AMST_MiddleName,
                                            AMST_LastName = a.AMST_LastName,

                                        }
             ).ToArray();
                }

                else if (data.regornamedetails == "stdname")
                {
                    data.studentlist = (from a in _StudentTcReportContext.AdmissionStudent
                                        from b in _StudentTcReportContext.SchoolAdmYStudent
                                        where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_ID && b.ASMAY_Id == data.asmay_id && a.AMST_SOL == "S")
                                        select new StudentSmartCardLogReportDTO
                                        {
                                            Amst_Id = a.AMST_Id,
                                            AMST_FirstName = a.AMST_FirstName,
                                            AMST_MiddleName = a.AMST_MiddleName,
                                            AMST_LastName = a.AMST_LastName + "-" + a.AMST_RegistrationNo,
                                        }
             ).ToArray();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
            return data;
        }

        public async Task<StudentSmartCardLogReportDTO> Getreportdetails(StudentSmartCardLogReportDTO data)
        {
            try
            {
                DateTime fromdatecon = DateTime.Now;
                string confromdate = "";

                DateTime todatecon = DateTime.Now;
                string contodate = "";

                DateTime dailydate = DateTime.Now;
                string condailydate = "";
                try
                {
                    fromdatecon = Convert.ToDateTime(data.fromdate.Value.Date.ToString("yyyy-MM-dd"));
                    confromdate = fromdatecon.ToString("yyyy-MM-dd");

                    todatecon = Convert.ToDateTime(data.todate.Value.Date.ToString("yyyy-MM-dd"));
                    contodate = todatecon.ToString("yyyy-MM-dd");

                    dailydate = Convert.ToDateTime(data.dailydate.Value.Date.ToString("yyyy-MM-dd"));
                    condailydate = dailydate.ToString("yyyy-MM-dd");
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }



                using (var cmd = _StudentTcReportContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Admission_SmartCard_LogReport";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@allorindivflag",
                        SqlDbType.VarChar)
                    {
                        Value = data.allorindiv
                    });
                    cmd.Parameters.Add(new SqlParameter("@mallorindivflag",
                      SqlDbType.VarChar)
                    {
                        Value = data.mallorindi
                    });
                    cmd.Parameters.Add(new SqlParameter("@dailyordatewiseflag",
                     SqlDbType.VarChar)
                    {
                        Value = data.dailybtedates
                    });
                    cmd.Parameters.Add(new SqlParameter("@dailydate",
                     SqlDbType.VarChar)
                    {
                        Value = condailydate
                    });
                    cmd.Parameters.Add(new SqlParameter("@fromdate",
                   SqlDbType.VarChar)
                    {
                        Value = confromdate
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",
                   SqlDbType.VarChar)
                    {
                        Value = contodate
                    });
                    cmd.Parameters.Add(new SqlParameter("@amstid",
                 SqlDbType.VarChar)
                    {
                        Value = data.Amst_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@modulename",
                 SqlDbType.VarChar)
                    {
                        Value = data.ASMODULE

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
                        data.alldatagridreport = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        _smartcard.LogInformation("Smart Card Log Report Error Sp :'" + ex.Message + "'");
                        data.alldatagridreport = null;
                    }
                }
            }
            catch (Exception e)
            {
                _smartcard.LogInformation("Smart Card Log Report Error :'" + e.Message + "'");
            }
            return data;
        }


    }
}
