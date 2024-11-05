using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.FrontOffice;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.FrontOffice;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.FrontOffice;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FrontOfficeHub.com.vaps.Services
{
    public class BiometricImpl : Interfaces.BiometricInterface
    {
        public FOContext _FOContext;

        public DomainModelMsSqlServerContext _db;
        private readonly ILogger<BiometricImpl> _log;

        public BiometricImpl(FOContext ttcntx, ILogger<BiometricImpl> loggerFactor, DomainModelMsSqlServerContext biomet)
        {
            _FOContext = ttcntx;
            _log = loggerFactor;
            _db = biomet;
        }
        public FO_Emp_PunchDTO punchdata(FO_Emp_PunchDTO data)
        {
            try
            {
                    List<object> arrtest = new List<object>();
                    for (int i = 0; i < data.temp1.Length; i++)
                    {
                        //writer.WriteLine(data.temp1[i].MI_Id + " " + data.temp1[i].HRME_BiometricCode + " "+ data.temp1[i].FOEP_PunchDate+" "+ data.temp1[i].FOEPD_PunchTime);
                       
                        object B_DTO = new FO_Emp_PunchDTO2
                        {
                            MI_Id = data.temp1[i].MI_Id.ToString(),
                            HRME_BiometricCode = data.temp1[i].HRME_BiometricCode.ToString(),
                            FOEP_PunchDate = data.temp1[i].FOEP_PunchDate.ToString(),
                            FOEPD_PunchTime = Convert.ToDateTime(data.temp1[i].FOEPD_PunchTime).ToString("HH:mm")
                        };
                        arrtest.Add(B_DTO);

                    }
                    var item = new
                    {
                        temp1 = arrtest
                    };

                    var result = JsonConvert.SerializeObject(item);
                string str_result = result.ToString();
                data.MI_Id = data.temp1[0].MI_Id;


                //17-11-2018 code starts
                string name = DateTime.Now.ToString("dd-MM-yyyy-hh-mm-tt") + "_" + data.MI_Id;
                UploadAsync(name, data.MI_Id, str_result);

                //17-11-2018 code ends

                //System.IO.File.WriteAllText(@"D:\Biometric_Logs\Biometric_Log_"+ name + ".json", result.ToString());


                if (data.temp1 != null)
                {
                    for (int i = 0; i < data.temp1.Length; i++)
                    {


                       

                        DateTime punchdate = data.temp1[i].FOEP_PunchDate.Value.Date;
                        string punchtime = Convert.ToDateTime(data.temp1[i].FOEPD_PunchTime).ToString("HH:mm");
               

                        var currTemperature = "";
                        var biometriccode = data.temp1[i].HRME_BiometricCode;
                        if (biometriccode != null && biometriccode.Length > 0)
                        {
                            var API_URL = "";

                            var connectionstring = "";
                            List<FO_Emp_PunchDTO> punchdata = new List<FO_Emp_PunchDTO>();
                            if (Convert.ToInt64(biometriccode) > 19000)
                            {
                                //Only For Vaps Employee
                                connectionstring = "Data Source=dcampus.database.windows.net,1433;Initial Catalog=VMS;Persist Security Info=False;User ID=decampus;Password=Digit@lc@mpu$@1;Connection Timeout=30;";


                                try
                                {

                                    using (SqlConnection connection = new SqlConnection(connectionstring))
                                    {
                                        try
                                        {
                                           

                                            using (var cmd = connection.CreateCommand())
                                            {
                                                cmd.CommandTimeout = 300;
                                                cmd.CommandText = "FO_Punch_Punch_Insert";
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                                                {
                                                    Value = Convert.ToInt64(data.MI_Id)
                                                });
                                                cmd.Parameters.Add(new SqlParameter("@Biometric_Id", SqlDbType.BigInt)
                                                {
                                                    Value = Convert.ToInt64(biometriccode)
                                                });
                                                cmd.Parameters.Add(new SqlParameter("@PunchDate", SqlDbType.DateTime)
                                                {
                                                    Value = Convert.ToDateTime(punchdate)
                                                });
                                                cmd.Parameters.Add(new SqlParameter("@PunchTime", SqlDbType.VarChar)
                                                {
                                                    Value = Convert.ToString(punchtime)
                                                });
                                                cmd.Parameters.Add(new SqlParameter("@Temparature", SqlDbType.VarChar)
                                                {
                                                    Value = Convert.ToString(currTemperature)
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
                                                            punchdata.Add(new FO_Emp_PunchDTO
                                                            {
                                                                HRME_BiometricCode = dataReader["Biometric_Id"].ToString(),
                                                            });
                                                        }
                                                    }
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.Write(ex.Message);
                                                }
                                            }





                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine("Error: " + ex.Message);
                                        }
                                    }



                                }
                                catch (Exception ex)
                                {
                                    //continue;
                                }

                            }
                            else
                            {

                                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                                {
                                    cmd.CommandTimeout = 300;
                                    cmd.CommandText = "FO_Punch_Punch_Insert";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                                    {
                                        Value = Convert.ToInt64(data.MI_Id)
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@Biometric_Id", SqlDbType.VarChar)
                                    {
                                        Value = Convert.ToString(biometriccode)
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@PunchDate", SqlDbType.DateTime)
                                    {
                                        Value = Convert.ToDateTime(punchdate)
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@PunchTime", SqlDbType.VarChar)
                                    {
                                        Value = Convert.ToString(punchtime)
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@Temparature", SqlDbType.VarChar)
                                    {
                                        Value = Convert.ToString(currTemperature)
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
                                                punchdata.Add(new FO_Emp_PunchDTO
                                                {
                                                    HRME_BiometricCode = dataReader["Biometric_Id"].ToString(),
                                                });
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.Write(ex.Message);
                                    }
                                }


                            }
                          
                        }                        
                   
                    }
                    using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "FO_PunchDetailsUpdationAfterDownLoad";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.Date)
                        {
                            Value = DateTime.Now.Date
                        });
                        cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.Date)
                        {
                            Value = DateTime.Now.Date
                        });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                    }
                }
            }
            catch (Exception e)
            {
                _log.LogInformation("Biometric impl");
                _log.LogDebug(e.Message);
            }
            return data;
        }

        public FO_Emp_PunchDTO punchdataTemparature(FO_Emp_PunchDTO data)
        {
            try
            {
                List<object> arrtest = new List<object>();
                for (int i = 0; i < data.temp1.Length; i++)
                {
                    //writer.WriteLine(data.temp1[i].MI_Id + " " + data.temp1[i].HRME_BiometricCode + " "+ data.temp1[i].FOEP_PunchDate+" "+ data.temp1[i].FOEPD_PunchTime);
                    if (data.temp1[i].FOEPD_Temperature !=null)
                    {
                        object B_DTO = new FO_Emp_PunchDTO3
                        {
                            MI_Id = data.temp1[i].MI_Id.ToString(),
                            HRME_BiometricCode = data.temp1[i].HRME_BiometricCode.ToString(),
                            FOEP_PunchDate = data.temp1[i].FOEP_PunchDate.ToString(),
                            FOEPD_PunchTime = Convert.ToDateTime(data.temp1[i].FOEPD_PunchTime).ToString("HH:mm"),
                            FOEPD_Temperature = data.temp1[i].FOEPD_Temperature.ToString()
                        };
                        arrtest.Add(B_DTO);
                    }
                    else
                    {
                        object B_DTO1 = new FO_Emp_PunchDTO2
                        {
                            MI_Id = data.temp1[i].MI_Id.ToString(),
                            HRME_BiometricCode = data.temp1[i].HRME_BiometricCode.ToString(),
                            FOEP_PunchDate = data.temp1[i].FOEP_PunchDate.ToString(),
                            FOEPD_PunchTime = Convert.ToDateTime(data.temp1[i].FOEPD_PunchTime).ToString("HH:mm")
                        };
                        arrtest.Add(B_DTO1);
                    }
                    
                    

                }
                var item = new
                {
                    temp1 = arrtest
                };

                var result = JsonConvert.SerializeObject(item);
                string str_result = result.ToString();
                data.MI_Id = data.temp1[0].MI_Id;


                //17-11-2018 code starts
                string name = DateTime.Now.ToString("dd-MM-yyyy-hh-mm-tt") + "_" + data.MI_Id;
                UploadAsync(name, data.MI_Id, str_result);




                if (data.temp1 != null)
                {
                    for (int i = 0; i < data.temp1.Length; i++)
                    {

                        var query = _FOContext.HR_Master_Employee_DMO.Where(d => d.HRME_BiometricCode == data.temp1[i].HRME_BiometricCode && d.HRME_ActiveFlag == true && d.HRME_LeftFlag==false && d.MI_Id == data.temp1[i].MI_Id).Select(t => t.HRME_Id).FirstOrDefault();
                        if (query > 0)
                        {
                            var query2 = _FOContext.FO_Emp_Punch.Where(d => d.HRME_Id == query && d.FOEP_PunchDate.Value.Date == data.temp1[i].FOEP_PunchDate.Value.Date && d.MI_Id == data.temp1[i].MI_Id).ToList(); //query.FirstOrDefault().HRME_Id
                            if (query2.Count == 0)
                            {
                                FO_Emp_PunchDMO dmo = new FO_Emp_PunchDMO();
                                dmo.CreatedDate = DateTime.Now;
                                dmo.FOEP_Flag = true;
                                var query3 = (from m in _FOContext.holidaydate
                                              from n in _FOContext.holidayWorkingDayType
                                              where m.FOHWDT_Id == n.FOHWDT_Id && n.FOHTWD_HolidayFlag == true && data.temp1[i].FOEP_PunchDate.Value.Date >= m.FOMHWDD_FromDate.Value.Date && data.temp1[i].FOEP_PunchDate.Value.Date <= m.FOMHWDD_ToDate.Value.Date && n.MI_Id == data.temp1[i].MI_Id

                                              select m).ToList();
                                if (query3.Count > 0)
                                {
                                    dmo.FOEP_HolidayPunchFlg = true;
                                }
                                else
                                {
                                    dmo.FOEP_HolidayPunchFlg = false;
                                }

                                dmo.FOEP_PunchDate = data.temp1[i].FOEP_PunchDate;
                                //dmo.HRME_Id = query.FirstOrDefault().HRME_Id;
                                dmo.HRME_Id = query;
                                dmo.MI_Id = data.temp1[i].MI_Id;
                                dmo.UpdatedDate = DateTime.Now;
                                _FOContext.Add(dmo);

                                FO_Emp_Punch_DetailsDMO dmo2 = new FO_Emp_Punch_DetailsDMO();
                                dmo2.CreatedDate = DateTime.Now;
                                dmo2.MI_Id = data.temp1[i].MI_Id;
                                dmo2.FOEP_Id = dmo.FOEP_Id;
                                dmo2.FOEPD_Flag = "1";
                                dmo2.FOEPD_InOutFlg = "I";
                                dmo2.FOEPD_PunchTime = Convert.ToDateTime(data.temp1[i].FOEPD_PunchTime).ToString("HH:mm");
                                dmo2.FOEPD_Temperature = data.temp1[i].FOEPD_Temperature;
                                dmo2.UpdatedDate = DateTime.Now;
                                _FOContext.Add(dmo2);

                                var flag = _FOContext.SaveChanges();
                                if (flag > 0)
                                {
                                    data.returnval = true;
                                }
                                else
                                {
                                    data.returnval = false;
                                }

                            }
                            else if (query2.Count > 0)
                            {

                                List<FO_Emp_PunchDTO> punchdata = new List<FO_Emp_PunchDTO>();
                                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                                {
                                    cmd.CommandTimeout = 300;
                                    cmd.CommandText = "FO_getPunchDetails";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@FOEP_PunchDate", SqlDbType.DateTime)
                                    {
                                        Value = Convert.ToDateTime(data.temp1[i].FOEP_PunchDate)
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@FOEPD_PunchTime", SqlDbType.VarChar)
                                    {
                                        Value = Convert.ToString(data.temp1[i].FOEPD_PunchTime)
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                                    {
                                        Value = Convert.ToInt64(query)
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                                    {
                                        Value = Convert.ToInt64(data.temp1[i].MI_Id)
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
                                                punchdata.Add(new FO_Emp_PunchDTO
                                                {
                                                    FOEPD_Id = Convert.ToInt64(dataReader["FOEPD_Id"]),
                                                    FOEPD_InOutFlg = Convert.ToString(dataReader["FOEPD_InOutFlg"]),
                                                    FOEPD_PunchTime = Convert.ToString(dataReader["FOEPD_PunchTime"]),
                                                });
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.Write(ex.Message);
                                    }
                                }

                                if (punchdata.Count == 0)
                                {
                                    var query5 = _FOContext.FO_Emp_Punch_Details.Where(d => d.FOEP_Id == query2.FirstOrDefault().FOEP_Id && d.MI_Id == data.temp1[i].MI_Id).OrderByDescending(t => t.FOEPD_Id).ToList();

                                    if (query5.Count > 0)
                                    {
                                        DateTime lastlog = Convert.ToDateTime(query5.FirstOrDefault().FOEPD_PunchTime);
                                        DateTime stime1 = Convert.ToDateTime(data.temp1[i].FOEPD_PunchTime);
                                        // string tim = stime1.TimeOfDay.ToString("HH:mm");
                                        TimeSpan diff = stime1.Subtract(lastlog);
                                        double totalMinutes = diff.TotalMinutes;
                                        if (totalMinutes > 3.0 || totalMinutes < 0)
                                        {
                                            if (query5.FirstOrDefault().FOEPD_InOutFlg == "I")
                                            {
                                                var query15 = _FOContext.FO_Emp_Punch_Details.Where(d => d.FOEP_Id == query2.FirstOrDefault().FOEP_Id && d.MI_Id == data.temp1[i].MI_Id && d.FOEPD_PunchTime == Convert.ToDateTime(data.temp1[i].FOEPD_PunchTime).ToString("HH:mm")).ToList();
                                                if (query15.Count == 0)
                                                {
                                                    FO_Emp_Punch_DetailsDMO dmo2 = new FO_Emp_Punch_DetailsDMO();
                                                    dmo2.CreatedDate = DateTime.Now;
                                                    dmo2.MI_Id = data.temp1[i].MI_Id;
                                                    dmo2.FOEP_Id = query5.FirstOrDefault().FOEP_Id;
                                                    dmo2.FOEPD_Flag = "1";
                                                    dmo2.FOEPD_InOutFlg = "O";
                                                    dmo2.FOEPD_Temperature = data.temp1[i].FOEPD_Temperature;
                                                    dmo2.FOEPD_PunchTime = Convert.ToDateTime(data.temp1[i].FOEPD_PunchTime).ToString("HH:mm");
                                                    dmo2.UpdatedDate = DateTime.Now;
                                                    _FOContext.Add(dmo2);
                                                }

                                                var flag = _FOContext.SaveChanges();
                                                if (flag > 0)
                                                {
                                                    data.returnval = true;
                                                }
                                                else
                                                {
                                                    data.returnval = false;
                                                }
                                            }
                                            else if (query5.FirstOrDefault().FOEPD_InOutFlg == "O")
                                            {
                                                var query16 = _FOContext.FO_Emp_Punch_Details.Where(d => d.FOEP_Id == query2.FirstOrDefault().FOEP_Id && d.MI_Id == data.temp1[i].MI_Id && d.FOEPD_PunchTime == Convert.ToDateTime(data.temp1[i].FOEPD_PunchTime).ToString("HH:mm")).ToList();
                                                if (query16.Count == 0)
                                                {
                                                    FO_Emp_Punch_DetailsDMO dmo2 = new FO_Emp_Punch_DetailsDMO();
                                                    dmo2.CreatedDate = DateTime.Now;
                                                    dmo2.MI_Id = data.temp1[i].MI_Id;
                                                    dmo2.FOEP_Id = query5.FirstOrDefault().FOEP_Id;
                                                    dmo2.FOEPD_Flag = "1";
                                                    dmo2.FOEPD_InOutFlg = "I";
                                                    dmo2.FOEPD_Temperature = data.temp1[i].FOEPD_Temperature;
                                                    dmo2.FOEPD_PunchTime = Convert.ToDateTime(data.temp1[i].FOEPD_PunchTime).ToString("HH:mm");
                                                    dmo2.UpdatedDate = DateTime.Now;
                                                    _FOContext.Add(dmo2);

                                                    var flag = _FOContext.SaveChanges();
                                                    if (flag > 0)
                                                    {
                                                        data.returnval = true;
                                                    }
                                                    else
                                                    {
                                                        data.returnval = false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }


                            }
                        }
                    }
                    using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "FO_PunchDetailsUpdationAfterDownLoad";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.Date)
                        {
                            Value = DateTime.Now.Date
                        });
                        cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.Date)
                        {
                            Value = DateTime.Now.Date
                        });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
                    }
                }
            }
            catch (Exception e)
            {
                _log.LogInformation("Biometric impl");
                _log.LogDebug(e.Message);
            }
            return data;
        }


        public async Task UploadAsync(string name, long MI_Id,string str_result)
        {
            try
            {
                var datatstu = _db.IVRM_Storage_path_Details.ToList();
                StorageCredentials cre = new StorageCredentials(datatstu.FirstOrDefault().IVRM_SD_Access_Name, datatstu.FirstOrDefault().IVRM_SD_Access_Key);
                CloudStorageAccount acc = new CloudStorageAccount(cre, useHttps: true);
                CloudBlobClient blobClient = acc.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("biometriclogs/" + MI_Id + "/" + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(DateTime.Now.Month));
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(name + ".txt");
              
                var options = new BlobRequestOptions()
                {
                    ServerTimeout = TimeSpan.FromMinutes(10)
                };
                using (var stream = new MemoryStream(Encoding.Default.GetBytes(str_result), false))
                {
                    await blockBlob.UploadFromStreamAsync(stream);
                }

            }
            catch(Exception ex)
            {
               // ex.Message;
            }
           
           
        }
        public FO_Emp_PunchDTO Latedata(FO_Emp_PunchDTO data)
        {
            FO_Emp_PunchDTO obj = new FO_Emp_PunchDTO();
            string str = "";
            try
            {
                List<FO_Emp_PunchDTO> result = new List<FO_Emp_PunchDTO>();
                //to get data according to search criteria.
                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FO_SP_MG_PAYCARE_LATEIN_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.Int) { Value = data.MI_Id });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new FO_Emp_PunchDTO
                                {
                                    EmployeeName = dataReader["EMPLOYEE NAME"].ToString(),
                                    InTime = dataReader["IN TIME"].ToString(),
                                    EntryTime = dataReader["ENTRY TIME"].ToString(),
                                    LateBy = dataReader["LATE BY"].ToString()
                                });
                            }
                        }
                        if (result.Count > 0)
                        {

                            str += "<table width='100%' border= '1'><tr align='center' width='100%'>LATE IN DETAIL   <b>Date : date1 </b></tr> <tr style = 'background-color:cornflowerblue' ><td> NAME </td><td> IN TIME</td><td> LATE BY </td></tr> ";

                            for (int i = 0; i < result.Count; i++)
                            {
                                str += "<tr><td>" + result[i].EmployeeName + "</td><td>" + result[i].InTime + "</td><td>" + result[i].LateBy + "</td></tr>";
                            }
                            str += "</table>";
                        }
                        else
                        {
                            str += "\r\n" + "LATE IN DETAIL   <b>Date : date1 </b><br />";
                            str += "\r\n" + "NO One Came Late Today!!!";
                        }

                        var query = (from m in _FOContext.holidaydate
                                     from n in _FOContext.holidayWorkingDayType
                                     where m.FOHWDT_Id == n.FOHWDT_Id && n.FOHTWD_HolidayFlag == true && m.FOMHWDD_FromDate.Value.Date == DateTime.Now.Date && n.MI_Id == data.MI_Id
                                     select m).ToList();
                        if (query.Count > 0)
                        {
                            return obj;
                        }
                        else
                        {
                            string[] mail_id = data.emailId.Split(',');
                            if (mail_id.Length == 1)
                            {
                                SendEmail(data.emailId, "Late-In details", str, data.MI_Id);
                            }
                            else if (mail_id.Length > 1)
                            {
                                for (int j = 0; j < mail_id.Length; j++)
                                {
                                    SendEmail(mail_id[j], "Late-In details", str, data.MI_Id);
                                }
                            }
                        }
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }

            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return obj;
        }

        public FO_Emp_PunchDTO LunchLatedata(FO_Emp_PunchDTO data)
        {
            FO_Emp_PunchDTO obj = new FO_Emp_PunchDTO();
            string str = "";
            try
            {
                List<FO_Emp_PunchDTO> result = new List<FO_Emp_PunchDTO>();
                //to get data according to search criteria.
                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FO_sp_mg_paycare_latein_details_LUNCH";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.Int) { Value = data.MI_Id });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new FO_Emp_PunchDTO
                                {
                                    EmployeeName = dataReader["EMPLOYEE NAME"].ToString(),
                                    InTime = dataReader["IN TIME"].ToString(),
                                    EntryTime = dataReader["ENTRY TIME"].ToString(),
                                    LateBy = dataReader["LATE BY"].ToString()
                                });
                            }
                        }
                        if (result.Count > 0)
                        {

                            str += "<table width='100%' border= '1'><tr align='center' width='100%'>LUNCH LATE IN DETAIL   <b>Date : date1 </b></tr> <tr style = 'background-color:cornflowerblue' ><td> NAME </td><td> IN TIME</td><td> LATE BY </td></tr> ";

                            for (int i = 0; i < result.Count; i++)
                            {
                                str += "<tr><td>" + result[i].EmployeeName + "</td><td>" + result[i].InTime + "</td><td>" + result[i].LateBy + "</td></tr>";
                            }
                            str += "</table>";
                        }
                        else
                        {
                            str += "\r\n" + "LUNCH LATE IN DETAIL   <b>Date : date1 </b><br />";
                            str += "\r\n" + "NO One Came Late Today!!!";
                        }

                        var query = (from m in _FOContext.holidaydate
                                     from n in _FOContext.holidayWorkingDayType
                                     where m.FOHWDT_Id == n.FOHWDT_Id && n.FOHTWD_HolidayFlag == true && m.FOMHWDD_FromDate.Value.Date == DateTime.Now.Date && n.MI_Id == data.MI_Id
                                     select m).ToList();
                        if (query.Count > 0)
                        {
                            return obj;
                        }
                        else
                        {
                            string[] mail_id = data.emailId.Split(',');
                            if (mail_id.Length == 1)
                            {
                                SendEmail(data.emailId, "Lunch Late-In details", str, data.MI_Id);
                            }
                            else if (mail_id.Length > 1)
                            {
                                for (int j = 0; j < mail_id.Length; j++)
                                {
                                    SendEmail(mail_id[j], "Lunch Late-In details", str, data.MI_Id);
                                }
                            }
                        }
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }

            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return obj;
        }

        public FO_Emp_PunchDTO LateInAbs_Email(FO_Emp_PunchDTO data)
        {
            FO_Emp_PunchDTO obj = new FO_Emp_PunchDTO();
            string str = "";
            try
            {
                List<FO_Emp_PunchDTO> result = new List<FO_Emp_PunchDTO>();
                //to get data according to search criteria.
                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FO_sp_mg_paycare_latein_details_Mail";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.Int) { Value = data.MI_Id });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new FO_Emp_PunchDTO
                                {
                                    EmployeeName = dataReader["EMPLOYEE NAME"].ToString(),
                                    HRME_Id = (long)dataReader["HRME_Id"],
                                    emailId = dataReader["EmailId"].ToString(),
                                    MobileNo = dataReader["MobileNo"].ToString() == "" ? 0 : (long)dataReader["MobileNo"],
                                    InTime = dataReader["IN TIME"].ToString(),
                                    EntryTime = dataReader["ENTRY TIME"].ToString(),
                                    LateBy = dataReader["LATE BY"].ToString(),
                                    OutTime = dataReader["LogoutTime"].ToString(),
                                    EarlyBy = dataReader["earlyby"].ToString(),
                                });
                            }
                        }
                        if (result.Count > 0)
                        {
                            for (int counter = 0; counter < result.Count; counter++)
                            {
                                if (result[counter].LateBy != null)
                                {
                                    string Emailbody = _FOContext.smsEmailSetting.Where(e => e.MI_Id == data.MI_Id && e.ISES_Template_Name == "FO_StaffLateIn").Select(m => m.ISES_MailBody).FirstOrDefault();
                                    Emailbody = Emailbody.Replace("[NAME]", result[counter].EmployeeName);
                                    Emailbody = Emailbody.Replace("[MINUTES]", result[counter].LateBy);
                                    SendEmail(result[counter].emailId, "Late-In Alert message", Emailbody, data.MI_Id);
                                }
                            }
                        }
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
                List<FO_Emp_PunchDTO> resultAbs = new List<FO_Emp_PunchDTO>();
                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "EmpAbsentDetails";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.Int) { Value = data.MI_Id });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                resultAbs.Add(new FO_Emp_PunchDTO
                                {
                                    EmployeeName = dataReader["name"].ToString(),
                                    HRME_Id = (long)dataReader["HRME_Id"],
                                    emailId = dataReader["email"].ToString(),
                                });
                            }
                        }
                        if (resultAbs.Count > 0)
                        {
                            for (int counter = 0; counter < resultAbs.Count; counter++)
                            {
                                string Emailbody = _FOContext.smsEmailSetting.Where(e => e.MI_Id == data.MI_Id && e.ISES_Template_Name == "FO_StaffAbsent").Select(m => m.ISES_MailBody).FirstOrDefault();
                                Emailbody = Emailbody.Replace("[NAME]", resultAbs[counter].EmployeeName);
                                SendEmail(resultAbs[counter].emailId, "Absent/Not Punched Alert message", Emailbody, data.MI_Id);
                            }
                        }
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }

            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return obj;
        }
        public FO_Emp_PunchDTO EarlyOut_Email(FO_Emp_PunchDTO data)
        {
            FO_Emp_PunchDTO obj = new FO_Emp_PunchDTO();
            string str = "";
            try
            {
                List<FO_Emp_PunchDTO> result = new List<FO_Emp_PunchDTO>();
                //to get data according to search criteria.
                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FO_sp_mg_paycare_earlyout_details_Mail";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.Int) { Value = data.MI_Id });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new FO_Emp_PunchDTO
                                {
                                    EmployeeName = dataReader["EMPLOYEE NAME"].ToString(),
                                    emailId = dataReader["HRMEM_EmailId"].ToString(),
                                    MobileNo = dataReader["HRMEMNO_MobileNo"].ToString() == "" ? 0 : (long)dataReader["HRMEMNO_MobileNo"],
                                    LateBy = dataReader["OUT TIME"].ToString(),
                                    OutTime = dataReader["EXIT TIME"].ToString(),
                                    EarlyBy = dataReader["EARLY BY"].ToString(),
                                });
                            }
                        }
                        if (result.Count > 0)
                        {
                            for (int counter = 0; counter < result.Count; counter++)
                            {
                                if (result[counter].EarlyBy != null)
                                {
                                    string Emailbody = _FOContext.smsEmailSetting.Where(e => e.MI_Id == data.MI_Id && e.ISES_Template_Name == "FO_StaffEarlyOut").Select(m => m.ISES_MailBody).FirstOrDefault();
                                    Emailbody = Emailbody.Replace("[NAME]", result[counter].EmployeeName);
                                    Emailbody = Emailbody.Replace("[MINUTES]", result[counter].EarlyBy);
                                    SendEmail(result[counter].emailId, "Early-Out Alert message", Emailbody, data.MI_Id);
                                }
                            }
                        }
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }

            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return obj;
        }

        public void SendEmail(string mailid, string subject, string body, long id)
        {
            //mailid = "goutamkumar@vapstech.com";
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _db.smsEmailSetting.Where(e => e.MI_Id == id && e.ISES_Template_Name.Equals("FO", StringComparison.OrdinalIgnoreCase) && e.ISES_MailActiveFlag == true).ToList();
                var institutionName = _db.Institution.Where(i => i.MI_Id == id).ToList();

                string Mailmsg = body;

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(id)).ToList();

                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    string mailcc = alldetails[0].IVRM_mailcc;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = subject;
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        string Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }

                    string date1 = DateTime.Now.Date.ToString("dd-MM-yyyy");

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;

                    if (mailcc != null && mailcc != "")
                    {
                        string[] mail_id = alldetails[0].IVRM_mailcc.Split(',');

                        if (mail_id.Length > 0)
                        {
                            for (int i = 0; i < mail_id.Length; i++)
                            {
                                message.AddBcc(mail_id[i]);
                            }

                        }
                    }

                    message.AddTo(mailid);

                    if (template.FirstOrDefault().ISES_MailHTMLTemplate != "" && template.FirstOrDefault().ISES_MailHTMLTemplate != null)
                    {
                        message.HtmlContent = Regex.Replace(template.FirstOrDefault().ISES_MailHTMLTemplate, @"\bMailmsg\b", Mailmsg, RegexOptions.IgnoreCase);

                        message.HtmlContent = Regex.Replace(message.HtmlContent, @"\bdate1\b", date1, RegexOptions.IgnoreCase);
                    }
                    else
                    {
                        message.HtmlContent = "<div style='height:100%; margin:0 auto; border:1px solid #333;'><table border='1' style='background:#CCE6FF;;'><tr><b><u>" + subject + "</u></b></tr> " + Mailmsg + "</table></div>";
                    }


                    if (alldetails.FirstOrDefault().IVRM_sendgridkey != "" && alldetails.FirstOrDefault().IVRM_sendgridkey != null)
                    {
                        var client = new SendGridClient(alldetails.FirstOrDefault().IVRM_sendgridkey);
                        client.SendEmailAsync(message).Wait();

                    }
                    else
                    {
                        // return "Sendgrid key is not available";
                    }


                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == id && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing_1";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = mailid
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = subject
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = id
                        });
                        cmd.Parameters.Add(new SqlParameter("@type",
                        SqlDbType.VarChar)
                        {
                            Value = "Staff"
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            //return ex.Message;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // return ex.Message;
            }
            // return "success";
        }
        public FO_Emp_PunchDTO Earlydata(FO_Emp_PunchDTO data)
        {
            FO_Emp_PunchDTO obj = new FO_Emp_PunchDTO();
            string str = "";
            try
            {
                List<FO_Emp_PunchDTO> result = new List<FO_Emp_PunchDTO>();
                //to get data according to search criteria.
                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FO_SP_MG_PAYCARE_EARLY_OUT_DETAILS";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.Int) { Value = data.MI_Id });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new FO_Emp_PunchDTO
                                {
                                    EmployeeName = dataReader["EMPLOYEE NAME"].ToString(),
                                    OutTime = dataReader["OUT TIME"].ToString(),
                                    ExitTime = dataReader["EXIT TIME"].ToString(),
                                    EarlyBy = dataReader["EARLY BY"].ToString()
                                });
                            }
                        }
                        if (result.Count > 0)
                        {
                            str += "<table width='100%' border= '1'><tr align='center'>EARLY OUT DETAIL   <b>Date : date1 </b></tr> <tr style = 'background-color:cornflowerblue' ><td> NAME </td><td> OUT TIME</td><td> EARLY BY </td></tr> ";

                            for (int i = 0; i < result.Count; i++)
                            {
                                str += "<tr><td>" + result[i].EmployeeName + "</td><td>" + result[i].OutTime + "</td><td>" + result[i].EarlyBy + "</td></tr>";
                            }
                            str += "</table>";
                        }
                        else
                        {
                            str += "\r\n" + "Early Out  DETAIL   <b>Date : date1 </b><br />";
                            str += "\r\n" + "NO One Went Early Today!!!";
                        }

                        var query = (from m in _FOContext.holidaydate
                                     from n in _FOContext.holidayWorkingDayType
                                     where m.FOHWDT_Id == n.FOHWDT_Id && n.FOHTWD_HolidayFlag == true && m.FOMHWDD_FromDate.Value.Date == DateTime.Now.Date && n.MI_Id == data.MI_Id
                                     select m).ToList();
                        if (query.Count > 0)
                        {
                            return obj;
                        }
                        else
                        {
                            string[] mail_id = data.emailId.Split(',');
                            if (mail_id.Length == 1)
                            {
                                SendEmail(data.emailId, "Early-Out details", str, data.MI_Id);
                            }
                            else if (mail_id.Length > 1)
                            {
                                for (int j = 0; j < mail_id.Length; j++)
                                {
                                    SendEmail(mail_id[j], "Early-Out details", str, data.MI_Id);
                                }
                            }

                        }
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return obj;
        }

        public FO_Emp_PunchDTO LunchEarlydata(FO_Emp_PunchDTO data)
        {
            FO_Emp_PunchDTO obj = new FO_Emp_PunchDTO();
            string str = "";
            try
            {
                List<FO_Emp_PunchDTO> result = new List<FO_Emp_PunchDTO>();
                //to get data according to search criteria.
                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FO_SP_MG_PAYCARE_EARLY_OUT_DETAILS_LUNCH";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@miid", SqlDbType.Int) { Value = data.MI_Id });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new FO_Emp_PunchDTO
                                {
                                    EmployeeName = dataReader["EMPLOYEE NAME"].ToString(),
                                    OutTime = dataReader["OUT TIME"].ToString(),
                                    ExitTime = dataReader["EXIT TIME"].ToString(),
                                    EarlyBy = dataReader["EARLY BY"].ToString()
                                });
                            }
                        }
                        if (result.Count > 0)
                        {
                            str += "<table width='100%' border= '1'><tr align='center'>LUNCH EARLY OUT DETAIL   <b>Date : date1 </b></tr> <tr style = 'background-color:cornflowerblue' ><td> NAME </td><td> OUT TIME</td><td> EARLY BY </td></tr> ";

                            for (int i = 0; i < result.Count; i++)
                            {
                                str += "<tr><td>" + result[i].EmployeeName + "</td><td>" + result[i].OutTime + "</td><td>" + result[i].EarlyBy + "</td></tr>";
                            }
                            str += "</table>";
                        }
                        else
                        {
                            str += "\r\n" + "LUNCH Early Out  DETAIL   <b>Date : date1 </b><br />";
                            str += "\r\n" + "NO One Went Early Today!!!";
                        }

                        var query = (from m in _FOContext.holidaydate
                                     from n in _FOContext.holidayWorkingDayType
                                     where m.FOHWDT_Id == n.FOHWDT_Id && n.FOHTWD_HolidayFlag == true && m.FOMHWDD_FromDate.Value.Date == DateTime.Now.Date && n.MI_Id == data.MI_Id
                                     select m).ToList();
                        if (query.Count > 0)
                        {
                            return obj;
                        }
                        else
                        {
                            string[] mail_id = data.emailId.Split(',');
                            if (mail_id.Length == 1)
                            {
                                SendEmail(data.emailId, "Lunch Early-Out details", str, data.MI_Id);
                            }
                            else if (mail_id.Length > 1)
                            {
                                for (int j = 0; j < mail_id.Length; j++)
                                {
                                    SendEmail(mail_id[j], "Lunch Early-Out details", str, data.MI_Id);
                                }
                            }

                        }
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return obj;
        }

        public FO_Biometric_VAPS_IEMapping_DTO vapsdata(FO_Biometric_VAPS_IEMapping_DTO data)
        {
            FO_Biometric_VAPS_IEMapping_DTO obj = new FO_Biometric_VAPS_IEMapping_DTO();
            string str = "";
            try
            {
               // List<FO_Emp_PunchDTO> result = new List<FO_Emp_PunchDTO>();
                //to get data according to search criteria.
                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    var query = _FOContext.FO_Biometric_VAPS_IEMapping_DMO.Where(t=>t.FOBVIEM_BiometricId==data.HRME_BiometricCode && t.FOBVIEM_ActiveFlg==true && t.MI_Id==data.MI_Id).Select(d => d.FOBVIEM_Insert_MI_Id).ToArray();
                    obj.filltypes = query.ToArray();
                }

            }
            catch(Exception ex)
            {

            }
                        
            return obj;
        }

        public class FO_Emp_PunchDTO2
        {
            public string MI_Id { get; set; }
            public string FOEP_PunchDate { get; set; }
            public string FOEPD_PunchTime { get; set; }
            public string HRME_BiometricCode { get; set; }
            public string HRME_RFCardId { get; set; }
           // public FO_Emp_PunchDTO2[] punch_details { get; set; }
        }

        public class FO_Emp_PunchDTO3
        {
            public string MI_Id { get; set; }
            public string FOEP_PunchDate { get; set; }
            public string FOEPD_PunchTime { get; set; }
            public string HRME_BiometricCode { get; set; }
            public string FOEPD_Temperature { get; set; }
            public string HRME_RFCardId { get; set; }
            // public FO_Emp_PunchDTO2[] punch_details { get; set; }
        }


        public class FO_Student_PunchDTO2
        {
            public string MI_Id { get; set; }
            public string ASPU_PunchDate { get; set; }
            public string ASPUD_PunchTime { get; set; }
            public string AMST_BiometricId { get; set; }
        }

        public FO_Emp_PunchDTO punchdata_vaps(FO_Emp_PunchDTO data)
        {
            try
            {

                List<object> arrtest = new List<object>();
                for (int i = 0; i < data.temp1.Length; i++)
                {
                    //writer.WriteLine(data.temp1[i].MI_Id + " " + data.temp1[i].HRME_BiometricCode + " "+ data.temp1[i].FOEP_PunchDate+" "+ data.temp1[i].FOEPD_PunchTime);

                    object B_DTO = new FO_Emp_PunchDTO2
                    {
                        MI_Id = data.temp1[i].MI_Id.ToString(),
                        HRME_BiometricCode = data.temp1[i].HRME_BiometricCode.ToString(),
                        FOEP_PunchDate = data.temp1[i].FOEP_PunchDate.ToString(),
                        FOEPD_PunchTime = data.temp1[i].FOEPD_PunchTime.ToString()
                    };
                    arrtest.Add(B_DTO);

                }
                var item = new
                {
                    temp1 = arrtest
                };

                var result = JsonConvert.SerializeObject(item);
                string str_result = result.ToString();


                //17-11-2018 code starts
                string name = DateTime.Now.ToString("dd-MM-yyyy-hh-mm-tt") + "_" + data.MI_Id;
               // UploadAsync(name);

                //17-11-2018 code ends

                //System.IO.File.WriteAllText(@"D:\Biometric_Logs\Biometric_Log_" + name + ".json", result.ToString());


                //string date = DateTime.Now.Date.ToString("dd-MM-yyyy");
                //string time = DateTime.Now.TimeOfDay.ToString();
                //FO_FileDownloadedLogsDMO dmo28 = new FO_FileDownloadedLogsDMO();
                //dmo28.CreatedDate = DateTime.Now;
                //dmo28.UpdatedDate = DateTime.Now;
                //dmo28.MI_Id = data.temp1[0].MI_Id;
                //dmo28.FODLL_Date = date;
                //dmo28.FODLL_time = time;
                //dmo28.FODLL_JSONData = str_result;
                //_FOContext.Add(dmo28);
                //_FOContext.SaveChanges();

                if (data.temp1 != null)
                {
                    for (int i = 0; i < data.temp1.Length; i++)
                    {
                        var query25 = _FOContext.FO_Biometric_VAPS_IEMapping_DMO.Where(t => t.FOBVIEM_BiometricId == data.temp1[i].HRME_BiometricCode && t.FOBVIEM_ActiveFlg == true && t.FOBVIEM_Insert_MI_Id == data.temp1[i].MI_Id).Select(d => d.MI_Id).FirstOrDefault();
                       if(query25 > 0)
                        {
                            var query = _FOContext.HR_Master_Employee_DMO.Where(d => d.HRME_BiometricCode == data.temp1[i].HRME_BiometricCode && d.HRME_ActiveFlag == true && d.MI_Id == query25).Select(t => t.HRME_Id).FirstOrDefault();
                            if (query > 0)
                            {
                                var query2 = _FOContext.FO_Emp_Punch.Where(d => d.HRME_Id == query && d.FOEP_PunchDate.Value.Date == data.temp1[i].FOEP_PunchDate.Value.Date && d.MI_Id == query25).ToList(); //query.FirstOrDefault().HRME_Id
                                if (query2.Count == 0)
                                {
                                    FO_Emp_PunchDMO dmo = new FO_Emp_PunchDMO();
                                    dmo.CreatedDate = DateTime.Now;
                                    dmo.FOEP_Flag = true;
                                    var query3 = (from m in _FOContext.holidaydate
                                                  from n in _FOContext.holidayWorkingDayType
                                                  where m.FOHWDT_Id == n.FOHWDT_Id && n.FOHTWD_HolidayFlag == true && data.temp1[i].FOEP_PunchDate.Value.Date >= m.FOMHWDD_FromDate.Value.Date && data.temp1[i].FOEP_PunchDate.Value.Date <= m.FOMHWDD_ToDate.Value.Date && n.MI_Id == query25

                                                  select m).ToList();
                                    if (query3.Count > 0)
                                    {
                                        dmo.FOEP_HolidayPunchFlg = true;
                                    }
                                    else
                                    {
                                        dmo.FOEP_HolidayPunchFlg = false;
                                    }

                                    dmo.FOEP_PunchDate = data.temp1[i].FOEP_PunchDate;
                                    //dmo.HRME_Id = query.FirstOrDefault().HRME_Id;
                                    dmo.HRME_Id = query;
                                    dmo.MI_Id = query25;
                                    dmo.UpdatedDate = DateTime.Now;
                                    _FOContext.Add(dmo);

                                    FO_Emp_Punch_DetailsDMO dmo2 = new FO_Emp_Punch_DetailsDMO();
                                    dmo2.CreatedDate = DateTime.Now;
                                    dmo2.MI_Id = query25;
                                    dmo2.FOEP_Id = dmo.FOEP_Id;
                                    dmo2.FOEPD_Flag = "1";
                                    dmo2.FOEPD_InOutFlg = "I";
                                    dmo2.FOEPD_PunchTime = Convert.ToDateTime(data.temp1[i].FOEPD_PunchTime).ToString("HH:mm");
                                    dmo2.UpdatedDate = DateTime.Now;
                                    _FOContext.Add(dmo2);

                                    var flag = _FOContext.SaveChanges();
                                    if (flag > 0)
                                    {
                                        data.returnval = true;
                                    }
                                    else
                                    {
                                        data.returnval = false;
                                    }

                                }

                                else if (query2.Count > 0)
                                {

                                    List<FO_Emp_PunchDTO> punchdata = new List<FO_Emp_PunchDTO>();
                                    using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                                    {
                                        cmd.CommandTimeout = 300;
                                        cmd.CommandText = "FO_getPunchDetails";
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add(new SqlParameter("@FOEP_PunchDate", SqlDbType.DateTime)
                                        {
                                            Value = Convert.ToDateTime(data.temp1[i].FOEP_PunchDate)
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@FOEPD_PunchTime", SqlDbType.VarChar)
                                        {
                                            Value = Convert.ToString(data.temp1[i].FOEPD_PunchTime)
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                                        {
                                            Value = Convert.ToInt64(query)
                                        });
                                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                                        {
                                            Value = Convert.ToInt64(query25)
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
                                                    punchdata.Add(new FO_Emp_PunchDTO
                                                    {
                                                        FOEPD_Id = Convert.ToInt64(dataReader["FOEPD_Id"]),
                                                        FOEPD_InOutFlg = Convert.ToString(dataReader["FOEPD_InOutFlg"]),
                                                        FOEPD_PunchTime = Convert.ToString(dataReader["FOEPD_PunchTime"]),
                                                    });
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.Write(ex.Message);
                                        }
                                    }

                                    if (punchdata.Count == 0)
                                    {
                                        var query5 = _FOContext.FO_Emp_Punch_Details.Where(d => d.FOEP_Id == query2.FirstOrDefault().FOEP_Id && d.MI_Id == query25).OrderByDescending(t => t.FOEPD_Id).ToList();

                                        if (query5.Count > 0)
                                        {

                                            DateTime lastlog = Convert.ToDateTime(query5.FirstOrDefault().FOEPD_PunchTime);
                                            DateTime stime1 = Convert.ToDateTime(data.temp1[i].FOEPD_PunchTime);
                                            // string tim = stime1.TimeOfDay.ToString("HH:mm");
                                            TimeSpan diff = stime1.Subtract(lastlog);
                                            double totalMinutes = diff.TotalMinutes;
                                            if (totalMinutes > 3.0)
                                            {
                                                if (query5.FirstOrDefault().FOEPD_InOutFlg == "I")
                                                {
                                                    var query15 = _FOContext.FO_Emp_Punch_Details.Where(d => d.FOEP_Id == query2.FirstOrDefault().FOEP_Id && d.MI_Id == query25 && d.FOEPD_PunchTime == data.temp1[i].FOEPD_PunchTime.Substring(0, 5)).ToList();
                                                    if (query15.Count == 0)
                                                    {
                                                        FO_Emp_Punch_DetailsDMO dmo2 = new FO_Emp_Punch_DetailsDMO();
                                                        dmo2.CreatedDate = DateTime.Now;
                                                        dmo2.MI_Id = query25;
                                                        dmo2.FOEP_Id = query5.FirstOrDefault().FOEP_Id;
                                                        dmo2.FOEPD_Flag = "1";
                                                        dmo2.FOEPD_InOutFlg = "O";
                                                        dmo2.FOEPD_PunchTime = Convert.ToDateTime(data.temp1[i].FOEPD_PunchTime).ToString("HH:mm");
                                                        dmo2.UpdatedDate = DateTime.Now;
                                                        _FOContext.Add(dmo2);
                                                    }

                                                    var flag = _FOContext.SaveChanges();
                                                    if (flag > 0)
                                                    {
                                                        data.returnval = true;
                                                    }
                                                    else
                                                    {
                                                        data.returnval = false;
                                                    }
                                                }
                                                else if (query5.FirstOrDefault().FOEPD_InOutFlg == "O")
                                                {

                                                    FO_Emp_Punch_DetailsDMO dmo2 = new FO_Emp_Punch_DetailsDMO();
                                                    dmo2.CreatedDate = DateTime.Now;
                                                    dmo2.MI_Id = query25;
                                                    dmo2.FOEP_Id = query5.FirstOrDefault().FOEP_Id;
                                                    dmo2.FOEPD_Flag = "1";
                                                    dmo2.FOEPD_InOutFlg = "I";
                                                    dmo2.FOEPD_PunchTime = Convert.ToDateTime(data.temp1[i].FOEPD_PunchTime).ToString("HH:mm");
                                                    dmo2.UpdatedDate = DateTime.Now;
                                                    _FOContext.Add(dmo2);

                                                    var flag = _FOContext.SaveChanges();
                                                    if (flag > 0)
                                                    {
                                                        data.returnval = true;
                                                    }
                                                    else
                                                    {
                                                        data.returnval = false;
                                                    }
                                                }
                                            }
                                        }

                                    }

                                    //27092018-Goutam
                                    //var Rechkquery = _FOContext.FO_Emp_Punch_Details.Where(d => d.FOEP_Id == query2.FirstOrDefault().FOEP_Id && d.MI_Id == data.temp1[i].MI_Id).OrderByDescending(t => t.FOEPD_PunchTime).ToList();
                                    //int Updatecounter = 0;
                                    //if (Rechkquery.Count > 0)
                                    //{
                                    //    for (int count = 0; count < Rechkquery.Count; count++)
                                    //    {
                                    //        var fdmo = _FOContext.FO_Emp_Punch_Details.Where(d => d.FOEP_Id == Rechkquery[count].FOEP_Id && d.MI_Id == data.temp1[i].MI_Id && d.FOEPD_PunchTime == Rechkquery[count].FOEPD_PunchTime).FirstOrDefault();
                                    //        if (Updatecounter == 0)
                                    //        {
                                    //            fdmo.FOEPD_InOutFlg = "I";
                                    //            Updatecounter++;
                                    //        }
                                    //        else
                                    //        {
                                    //            fdmo.FOEPD_InOutFlg = "O";
                                    //            Updatecounter = 0;
                                    //        }
                                    //        _FOContext.Update(fdmo);
                                    //        _FOContext.SaveChanges();
                                    //    }
                                    //}
                                    //27092018-Goutam

                                }
                            }
                        }
                       
                    }
                }
            }
            catch (Exception e)
            {
                _log.LogInformation("Biometric impl");
                _log.LogDebug(e.Message);
            }
            return data;
        }

        public FO_Emp_PunchDTO punchdata_Student(FO_Emp_PunchDTO data)
        {
            try
            {
                List<object> arrtest = new List<object>();
                for (int i = 0; i < data.temp1.Length; i++)
                {
                    object B_DTO = new FO_Student_PunchDTO2
                    {
                        MI_Id = data.temp1[i].MI_Id.ToString(),
                        AMST_BiometricId = data.temp1[i].AMST_BiometricId.ToString(),
                        ASPU_PunchDate = data.temp1[i].ASPU_PunchDate.ToString(),
                        ASPUD_PunchTime = data.temp1[i].ASPUD_PunchTime.ToString()
                    };
                    arrtest.Add(B_DTO);

                }
                var item = new
                {
                    temp1 = arrtest
                };

                var result = JsonConvert.SerializeObject(item);
                string str_result = result.ToString();
                data.MI_Id = data.temp1[0].MI_Id;

                string name = DateTime.Now.ToString("dd-MM-yyyy-hh-mm-tt") + "Stud_" + data.MI_Id;
                //UploadAsync(name, data.MI_Id, str_result);

                if (data.temp1 != null)
                {
                    for (int i = 0; i < data.temp1.Length; i++)
                    {

                        var query = _FOContext.Adm_M_Student.Where(d => d.AMST_BiometricId == data.temp1[i].AMST_BiometricId && d.AMST_ActiveFlag == 1 && d.MI_Id == data.temp1[i].MI_Id).Select(t => t.AMST_Id).FirstOrDefault();
                        if (query > 0)
                        {
                            var query2 = _FOContext.Adm_Student_PunchDMO.Where(d => d.AMST_Id == query && d.ASPU_PunchDate.Value.Date == data.temp1[i].ASPU_PunchDate.Value.Date && d.MI_Id == data.temp1[i].MI_Id).ToList(); //query.FirstOrDefault().HRME_Id
                            if (query2.Count == 0)
                            {
                                Adm_Student_PunchDMO dmo = new Adm_Student_PunchDMO();
                                dmo.CreatedDate = DateTime.Now;
                                dmo.ASPU_ActiveFlg = true;
                                dmo.ASPU_PunchDate = data.temp1[i].ASPU_PunchDate;
                                dmo.AMST_Id = query;
                                dmo.MI_Id = data.temp1[i].MI_Id;
                                _FOContext.Add(dmo);
                                _FOContext.SaveChanges();

                                Adm_Student_Punch_DetailsDMO dmo2 = new Adm_Student_Punch_DetailsDMO();
                                dmo2.MI_Id = data.temp1[i].MI_Id;
                                dmo2.ASPU_Id = dmo.ASPU_Id;
                                dmo2.ASPUD_InOutFlg = "I";
                                dmo2.ASPUD_PunchTime = Convert.ToDateTime(data.temp1[i].ASPUD_PunchTime).ToString("HH:mm");
                                _FOContext.Add(dmo2);
                                var flag = _FOContext.SaveChanges();
                                if (flag > 0)
                                {
                                    data.returnval = true;
                                }
                                else
                                {
                                    data.returnval = false;
                                }
                            }

                            else if (query2.Count > 0)
                            {
                                List<Adm_Student_PunchDTO> punchdata = new List<Adm_Student_PunchDTO>();
                                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                                {
                                    cmd.CommandTimeout = 300;
                                    cmd.CommandText = "FO_getStudentPunchDetails";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@ASPU_PunchDate", SqlDbType.DateTime)
                                    {
                                        Value = Convert.ToDateTime(data.temp1[i].ASPU_PunchDate)
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@ASPUD_PunchTime", SqlDbType.VarChar)
                                    {
                                        Value = Convert.ToString(data.temp1[i].ASPUD_PunchTime)
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt)
                                    {
                                        Value = Convert.ToInt64(query)
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                                    {
                                        Value = Convert.ToInt64(data.temp1[i].MI_Id)
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
                                                punchdata.Add(new Adm_Student_PunchDTO
                                                {
                                                    ASPUD_Id = Convert.ToInt64(dataReader["ASPUD_Id"]),
                                                    ASPUD_InOutFlg = Convert.ToString(dataReader["ASPUD_InOutFlg"]),
                                                    ASPUD_PunchTime = Convert.ToString(dataReader["ASPUD_PunchTime"]),
                                                });
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.Write(ex.Message);
                                    }
                                }

                                if (punchdata.Count == 0)
                                {
                                    var query5 = _FOContext.Adm_Student_Punch_DetailsDMO.Where(d => d.ASPU_Id == query2.FirstOrDefault().ASPU_Id && d.MI_Id == data.temp1[i].MI_Id).OrderByDescending(t => t.ASPUD_Id).ToList();

                                    if (query5.Count > 0)
                                    {
                                        DateTime lastlog = Convert.ToDateTime(query5.FirstOrDefault().ASPUD_PunchTime);
                                        DateTime stime1 = Convert.ToDateTime(data.temp1[i].ASPUD_PunchTime);
                                        TimeSpan diff = stime1.Subtract(lastlog);
                                        double totalMinutes = diff.TotalMinutes;
                                        if (totalMinutes > 3.0)
                                        {
                                            if (query5.FirstOrDefault().ASPUD_InOutFlg == "I")
                                            {
                                                var query15 = _FOContext.Adm_Student_Punch_DetailsDMO.Where(d => d.ASPU_Id == query2.FirstOrDefault().ASPU_Id && d.MI_Id == data.temp1[i].MI_Id && d.ASPUD_PunchTime == data.temp1[i].ASPUD_PunchTime.Substring(0, 5)).ToList();
                                                if (query15.Count == 0)
                                                {
                                                    Adm_Student_Punch_DetailsDMO dmo2 = new Adm_Student_Punch_DetailsDMO();
                                                    dmo2.MI_Id = data.temp1[i].MI_Id;
                                                    dmo2.ASPU_Id = query5.FirstOrDefault().ASPU_Id;
                                                    dmo2.ASPUD_InOutFlg = "O";
                                                    dmo2.ASPUD_PunchTime = Convert.ToDateTime(data.temp1[i].ASPUD_PunchTime).ToString("HH:mm");
                                                    _FOContext.Add(dmo2);
                                                }

                                                var flag = _FOContext.SaveChanges();
                                                if (flag > 0)
                                                {
                                                    data.returnval = true;
                                                }
                                                else
                                                {
                                                    data.returnval = false;
                                                }
                                            }
                                            else if (query5.FirstOrDefault().ASPUD_InOutFlg == "O")
                                            {
                                                Adm_Student_Punch_DetailsDMO dmo2 = new Adm_Student_Punch_DetailsDMO();
                                                dmo2.MI_Id = data.temp1[i].MI_Id;
                                                dmo2.ASPU_Id = query5.FirstOrDefault().ASPU_Id;
                                                dmo2.ASPUD_InOutFlg = "I";
                                                dmo2.ASPUD_PunchTime = Convert.ToDateTime(data.temp1[i].ASPUD_PunchTime).ToString("HH:mm");
                                                _FOContext.Add(dmo2);
                                                var flag = _FOContext.SaveChanges();
                                                if (flag > 0)
                                                {
                                                    data.returnval = true;
                                                }
                                                else
                                                {
                                                    data.returnval = false;
                                                }
                                            }
                                        }
                                    }

                                }

                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _log.LogInformation("Biometric impl");
                _log.LogDebug(e.Message);
            }
            return data;
        }

        public FO_Emp_PunchDTO Getbiometricdetails(FO_Emp_PunchDTO data)
        {
            data.biometricdevicedetails = _FOContext.FO_Biometric_DeviceDMO.Where(t => t.MI_Id == data.temp1[0].MI_Id && t.FOBD_ActiveFlg == true).ToArray();
            return data;
        }

        public FO_Emp_PunchDTO RFCardpunchdata(FO_Emp_PunchDTO data)
        {
            try
            {
                List<object> arrtest = new List<object>();
                for (int i = 0; i < data.temp1.Length; i++)
                {
                    object B_DTO = new FO_Emp_PunchDTO2
                    {
                        MI_Id = data.temp1[i].MI_Id.ToString(),
                        HRME_RFCardId = data.temp1[i].HRME_RFCardId.ToString(),
                        FOEP_PunchDate = data.temp1[i].FOEP_PunchDate.ToString(),
                        FOEPD_PunchTime = data.temp1[i].FOEPD_PunchTime.ToString()
                    };
                    arrtest.Add(B_DTO);
                }
                var item = new
                {
                    temp1 = arrtest
                };

                var result = JsonConvert.SerializeObject(item);
                string str_result = result.ToString();
                data.MI_Id = data.temp1[0].MI_Id;

                //string name = DateTime.Now.ToString("dd-MM-yyyy-hh-mm-tt") + "_" + data.MI_Id;
                //UploadAsync(name, data.MI_Id, str_result);

                if (data.temp1 != null)
                {
                    for (int i = 0; i < data.temp1.Length; i++)
                    {
                        var query = _FOContext.HR_Master_Employee_DMO.Where(d => d.HRME_RFCardId == data.temp1[i].HRME_RFCardId && d.HRME_ActiveFlag == true && d.MI_Id == data.temp1[i].MI_Id).Select(t => t.HRME_Id).FirstOrDefault();
                        if (query > 0)
                        {
                            var query2 = _FOContext.FO_Emp_Punch.Where(d => d.HRME_Id == query && d.FOEP_PunchDate.Value.Date == data.temp1[i].FOEP_PunchDate.Value.Date && d.MI_Id == data.temp1[i].MI_Id).ToList();
                            if (query2.Count == 0)
                            {
                                FO_Emp_PunchDMO dmo = new FO_Emp_PunchDMO();
                                dmo.CreatedDate = DateTime.Now;
                                dmo.FOEP_Flag = true;
                                var query3 = (from m in _FOContext.holidaydate
                                              from n in _FOContext.holidayWorkingDayType
                                              where m.FOHWDT_Id == n.FOHWDT_Id && n.FOHTWD_HolidayFlag == true && data.temp1[i].FOEP_PunchDate.Value.Date >= m.FOMHWDD_FromDate.Value.Date && data.temp1[i].FOEP_PunchDate.Value.Date <= m.FOMHWDD_ToDate.Value.Date && n.MI_Id == data.temp1[i].MI_Id

                                              select m).ToList();
                                if (query3.Count > 0)
                                {
                                    dmo.FOEP_HolidayPunchFlg = true;
                                }
                                else
                                {
                                    dmo.FOEP_HolidayPunchFlg = false;
                                }

                                dmo.FOEP_PunchDate = data.temp1[i].FOEP_PunchDate;
                                //dmo.HRME_Id = query.FirstOrDefault().HRME_Id;
                                dmo.HRME_Id = query;
                                dmo.MI_Id = data.temp1[i].MI_Id;
                                dmo.UpdatedDate = DateTime.Now;
                                _FOContext.Add(dmo);

                                FO_Emp_Punch_DetailsDMO dmo2 = new FO_Emp_Punch_DetailsDMO();
                                dmo2.CreatedDate = DateTime.Now;
                                dmo2.MI_Id = data.temp1[i].MI_Id;
                                dmo2.FOEP_Id = dmo.FOEP_Id;
                                dmo2.FOEPD_Flag = "1";
                                dmo2.FOEPD_InOutFlg = "I";
                                dmo2.FOEPD_PunchTime = Convert.ToDateTime(data.temp1[i].FOEPD_PunchTime).ToString("HH:mm");
                                dmo2.UpdatedDate = DateTime.Now;
                                _FOContext.Add(dmo2);

                                var flag = _FOContext.SaveChanges();
                                if (flag > 0)
                                {
                                    data.returnval = true;
                                }
                                else
                                {
                                    data.returnval = false;
                                }

                            }

                            else if (query2.Count > 0)
                            {
                                List<FO_Emp_PunchDTO> punchdata = new List<FO_Emp_PunchDTO>();
                                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                                {
                                    cmd.CommandTimeout = 300;
                                    cmd.CommandText = "FO_getPunchDetails";
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add(new SqlParameter("@FOEP_PunchDate", SqlDbType.DateTime)
                                    {
                                        Value = Convert.ToDateTime(data.temp1[i].FOEP_PunchDate)
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@FOEPD_PunchTime", SqlDbType.VarChar)
                                    {
                                        Value = Convert.ToString(data.temp1[i].FOEPD_PunchTime)
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@HRME_Id", SqlDbType.BigInt)
                                    {
                                        Value = Convert.ToInt64(query)
                                    });
                                    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
                                    {
                                        Value = Convert.ToInt64(data.temp1[i].MI_Id)
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
                                                punchdata.Add(new FO_Emp_PunchDTO
                                                {
                                                    FOEPD_Id = Convert.ToInt64(dataReader["FOEPD_Id"]),
                                                    FOEPD_InOutFlg = Convert.ToString(dataReader["FOEPD_InOutFlg"]),
                                                    FOEPD_PunchTime = Convert.ToString(dataReader["FOEPD_PunchTime"]),
                                                });
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.Write(ex.Message);
                                    }
                                }

                                if (punchdata.Count == 0)
                                {
                                    var query5 = _FOContext.FO_Emp_Punch_Details.Where(d => d.FOEP_Id == query2.FirstOrDefault().FOEP_Id && d.MI_Id == data.temp1[i].MI_Id).OrderByDescending(t => t.FOEPD_Id).ToList();

                                    if (query5.Count > 0)
                                    {

                                        DateTime lastlog = Convert.ToDateTime(query5.FirstOrDefault().FOEPD_PunchTime);
                                        DateTime stime1 = Convert.ToDateTime(data.temp1[i].FOEPD_PunchTime);
                                        TimeSpan diff = stime1.Subtract(lastlog);
                                        double totalMinutes = diff.TotalMinutes;
                                        if (totalMinutes > 3.0)
                                        {
                                            if (query5.FirstOrDefault().FOEPD_InOutFlg == "I")
                                            {
                                                var query15 = _FOContext.FO_Emp_Punch_Details.Where(d => d.FOEP_Id == query2.FirstOrDefault().FOEP_Id && d.MI_Id == data.temp1[i].MI_Id && d.FOEPD_PunchTime == data.temp1[i].FOEPD_PunchTime.Substring(0, 5)).ToList();
                                                if (query15.Count == 0)
                                                {
                                                    FO_Emp_Punch_DetailsDMO dmo2 = new FO_Emp_Punch_DetailsDMO();
                                                    dmo2.CreatedDate = DateTime.Now;
                                                    dmo2.MI_Id = data.temp1[i].MI_Id;
                                                    dmo2.FOEP_Id = query5.FirstOrDefault().FOEP_Id;
                                                    dmo2.FOEPD_Flag = "1";
                                                    dmo2.FOEPD_InOutFlg = "O";
                                                    dmo2.FOEPD_PunchTime = Convert.ToDateTime(data.temp1[i].FOEPD_PunchTime).ToString("HH:mm");
                                                    dmo2.UpdatedDate = DateTime.Now;
                                                    _FOContext.Add(dmo2);
                                                }

                                                var flag = _FOContext.SaveChanges();
                                                if (flag > 0)
                                                {
                                                    data.returnval = true;
                                                }
                                                else
                                                {
                                                    data.returnval = false;
                                                }
                                            }
                                            else if (query5.FirstOrDefault().FOEPD_InOutFlg == "O")
                                            {

                                                FO_Emp_Punch_DetailsDMO dmo2 = new FO_Emp_Punch_DetailsDMO();
                                                dmo2.CreatedDate = DateTime.Now;
                                                dmo2.MI_Id = data.temp1[i].MI_Id;
                                                dmo2.FOEP_Id = query5.FirstOrDefault().FOEP_Id;
                                                dmo2.FOEPD_Flag = "1";
                                                dmo2.FOEPD_InOutFlg = "I";
                                                dmo2.FOEPD_PunchTime = Convert.ToDateTime(data.temp1[i].FOEPD_PunchTime).ToString("HH:mm");
                                                dmo2.UpdatedDate = DateTime.Now;
                                                _FOContext.Add(dmo2);

                                                var flag = _FOContext.SaveChanges();
                                                if (flag > 0)
                                                {
                                                    data.returnval = true;
                                                }
                                                else
                                                {
                                                    data.returnval = false;
                                                }
                                            }
                                        }
                                    }

                                }

                                //27092018-Goutam
                                //var Rechkquery = _FOContext.FO_Emp_Punch_Details.Where(d => d.FOEP_Id == query2.FirstOrDefault().FOEP_Id && d.MI_Id == data.temp1[i].MI_Id).OrderByDescending(t => t.FOEPD_PunchTime).ToList();
                                //int Updatecounter = 0;
                                //if (Rechkquery.Count > 0)
                                //{
                                //    for (int count = 0; count < Rechkquery.Count; count++)
                                //    {
                                //        var fdmo = _FOContext.FO_Emp_Punch_Details.Where(d => d.FOEP_Id == Rechkquery[count].FOEP_Id && d.MI_Id == data.temp1[i].MI_Id && d.FOEPD_PunchTime == Rechkquery[count].FOEPD_PunchTime).FirstOrDefault();
                                //        if (Updatecounter == 0)
                                //        {
                                //            fdmo.FOEPD_InOutFlg = "I";
                                //            Updatecounter++;
                                //        }
                                //        else
                                //        {
                                //            fdmo.FOEPD_InOutFlg = "O";
                                //            Updatecounter = 0;
                                //        }
                                //        _FOContext.Update(fdmo);
                                //        _FOContext.SaveChanges();
                                //    }
                                //}
                                //27092018-Goutam

                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                _log.LogInformation("Biometric impl");
                _log.LogDebug(e.Message);
            }
            return data;
        }

        public FO_Emp_PunchDTO AutoAbsent(FO_Emp_PunchDTO data)
        {
            FO_Emp_PunchDTO obj = new FO_Emp_PunchDTO();
            string multiplehrmeid = "";
            string str = "";
            try
            {
                var multi = _FOContext.HR_Master_Employee_DMO.Where(a => a.HRME_ActiveFlag == true && a.MI_Id == data.MI_Id && a.HRME_LeftFlag == false).Select(t => t.HRME_Id).ToArray();
                if (multi.Length > 0)
                {
                    for (int i = 0; i < multi.Length; i++)
                    {
                        if (i == 0) { multiplehrmeid = multi[i].ToString(); }
                        else { multiplehrmeid = multiplehrmeid + "," + multi[i].ToString(); }
                    }
                }

                List<FO_Emp_PunchDTO> result = new List<FO_Emp_PunchDTO>();
                using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FO_Emp_Monthly_yearly_Report_New";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@fromdate",SqlDbType.VarChar)
                    {
                        Value = DateTime.Today.ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@todate",SqlDbType.VarChar)
                    {
                        Value = DateTime.Today.ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@multiplehrmeid",SqlDbType.NVarChar)
                    {
                        Value = multiplehrmeid
                    });
                    cmd.Parameters.Add(new SqlParameter("@miid",SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@type",SqlDbType.VarChar)
                    {
                        Value = "absent"
                    });
                    cmd.Parameters.Add(new SqlParameter("@cols",SqlDbType.NVarChar, 2000)
                    {
                        Direction = ParameterDirection.Output
                    });
                    cmd.Parameters.Add(new SqlParameter("@totalpresent",SqlDbType.VarChar, 10)
                    {
                        Direction = ParameterDirection.Output
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
                                result.Add(new FO_Emp_PunchDTO
                                {
                                    EmployeeName = dataReader["ename"].ToString(),
                                    HRME_BiometricCode = dataReader["ecode"].ToString(),
                                    HRME_RFCardId = dataReader["HRMDES_DesignationName"].ToString(),
                                    json = dataReader["absentdays"].ToString()
                                });
                            }
                        }
                        if (result.Count > 0)
                        {
                            str += "<table width='100%' border= '1'><tr align='center' width='100%'> ABSENT EMPLOYEE LIST <b>Date : " + DateTime.Today.ToString("dd - MM - yyyy") + "</b></tr> <tr style = 'background-color:cornflowerblue' ><td> NAME </td><td> EMP CODE </td><td> DESIGNATION </td></tr> ";

                            for (int i = 0; i < result.Count; i++)
                            {
                                if (result[i].json == "1")
                                {
                                    str += "<tr><td>" + result[i].EmployeeName + "</td><td>" + result[i].HRME_BiometricCode + "</td><td>" + result[i].HRME_RFCardId + "</td></tr>";
                                }
                            }
                            str += "</table>";
                        }
                        else
                        {
                            str += "\r\n" + "ABSENT EMPLOYEE LIST <b>Date :" + DateTime.Today.ToString("dd-MM-yyyy") + "</b><br />";
                            str += "\r\n" + "NO One is Absent Today!!!";
                        }

                        var query = (from m in _FOContext.holidaydate
                                     from n in _FOContext.holidayWorkingDayType
                                     where m.FOHWDT_Id == n.FOHWDT_Id && n.FOHTWD_HolidayFlag == true && m.FOMHWDD_FromDate.Value.Date == DateTime.Now.Date && n.MI_Id == data.MI_Id
                                     select m).ToList();
                        if (query.Count > 0)
                        {
                            return obj;
                        }
                        else
                        {
                            string[] mail_id = data.emailId.Split(',');
                            if (mail_id.Length == 1)
                            {
                                SendEmail(data.emailId, "Absent Employee List", str, data.MI_Id);
                            }
                            else if (mail_id.Length > 1)
                            {
                                for (int j = 0; j < mail_id.Length; j++)
                                {
                                    SendEmail(mail_id[j], "Absent Employee List", str, data.MI_Id);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }

            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return obj;
        }


        public FO_Emp_PunchDTO Studentpunchdata(FO_Emp_PunchDTO data)
        {
            try
            {
                List<object> arrtest = new List<object>();
                for (int i = 0; i < data.temp1.Length; i++)
                {
                    object B_DTO = new FO_Student_PunchDTO2
                    {
                        MI_Id = data.temp1[i].MI_Id.ToString(),
                        AMST_BiometricId = data.temp1[i].HRME_BiometricCode.ToString(),
                        ASPU_PunchDate = data.temp1[i].FOEP_PunchDate.ToString(),
                        ASPUD_PunchTime = data.temp1[i].FOEPD_PunchTime.ToString()
                    };
                    arrtest.Add(B_DTO);

                }

                var item = new
                {
                    temp1 = arrtest
                };

                var result = JsonConvert.SerializeObject(item);
                string str_result = result.ToString();
                data.MI_Id = data.temp1[0].MI_Id;


                //17-11-2018 code starts
                string name = DateTime.Now.ToString("dd-MM-yyyy-hh-mm-tt") + "Stud_" + data.MI_Id;
                UploadAsync(name, data.MI_Id, str_result);
                if (data.temp1 != null)
                {
                    for (int i = 0; i < data.temp1.Length; i++)
                    {
                        var query = _FOContext.Adm_M_Student.Where(d => d.AMST_BiometricId == data.temp1[i].HRME_BiometricCode && d.AMST_ActiveFlag == 1 && d.MI_Id == data.temp1[i].MI_Id).Select(t => t.AMST_Id).FirstOrDefault();
                        if (query > 0)
                        {
                            var query2 = _FOContext.Adm_Student_PunchDMO.Where(d => d.AMST_Id == query && d.ASPU_PunchDate.Value.ToString("dd/MM/yyyy") == data.temp1[i].FOEP_PunchDate.Value.ToString("dd/MM/yyyy") && d.MI_Id == data.temp1[i].MI_Id).ToList(); //query.FirstOrDefault().HRME_Id
                            if (query2.Count == 0)
                            {
                                Adm_Student_PunchDMO dmo = new Adm_Student_PunchDMO();
                                dmo.CreatedDate = DateTime.Now;
                                dmo.ASPU_ActiveFlg = true;
                                dmo.ASPU_PunchDate = data.temp1[i].FOEP_PunchDate;
                                dmo.AMST_Id = query;
                                dmo.MI_Id = data.temp1[i].MI_Id;
                                _FOContext.Add(dmo);
                                _FOContext.SaveChanges();

                                Adm_Student_Punch_DetailsDMO dmo2 = new Adm_Student_Punch_DetailsDMO();
                                dmo2.MI_Id = data.temp1[i].MI_Id;
                                dmo2.ASPU_Id = dmo.ASPU_Id;
                                dmo2.ASPUD_InOutFlg = "I";
                                dmo2.ASPUD_PunchTime = Convert.ToDateTime(data.temp1[i].FOEPD_PunchTime).ToString("HH:mm");
                                _FOContext.Add(dmo2);
                                var flag = _FOContext.SaveChanges();
                                if (flag > 0)
                                {
                                    data.returnval = true;
                                }
                                else
                                {
                                    data.returnval = false;
                                }

                            }

                            
                        }
                    }
                   
                }
            }
            catch (Exception e)
            {
                _log.LogInformation("Biometric impl");
                _log.LogDebug(e.Message);
            }
            return data;
        }


        //public FO_Emp_PunchDTO Studentpunchdata(FO_Emp_PunchDTO data)
        //{
        //    try
        //    {
        //        List<object> arrtest = new List<object>();
        //        for (int i = 0; i < data.temp1.Length; i++)
        //        {
        //            object B_DTO = new FO_Student_PunchDTO2
        //            {
        //                MI_Id = data.temp1[i].MI_Id.ToString(),
        //                AMST_BiometricId = data.temp1[i].HRME_BiometricCode.ToString(),
        //                ASPU_PunchDate = data.temp1[i].FOEP_PunchDate.ToString(),
        //                ASPUD_PunchTime = data.temp1[i].FOEPD_PunchTime.ToString()
        //            };
        //            arrtest.Add(B_DTO);

        //        }
        //        var item = new
        //        {
        //            temp1 = arrtest
        //        };

        //        var result = JsonConvert.SerializeObject(item);
        //        string str_result = result.ToString();
        //        data.MI_Id = data.temp1[0].MI_Id;

        //        string name = DateTime.Now.ToString("dd-MM-yyyy-hh-mm-tt") + "Stud_" + data.MI_Id;
        //        //UploadAsync(name, data.MI_Id, str_result);

        //        if (data.temp1 != null)
        //        {
        //            for (int i = 0; i < data.temp1.Length; i++)
        //            {

        //                var query = _FOContext.Adm_M_Student.Where(d => d.AMST_BiometricId == data.temp1[i].HRME_BiometricCode && d.AMST_ActiveFlag == 1 && d.MI_Id == data.temp1[i].MI_Id).Select(t => t.AMST_Id).FirstOrDefault();
        //                if (query > 0)
        //                {
        //                    var query2 = _FOContext.Adm_Student_PunchDMO.Where(d => d.AMST_Id == query && d.ASPU_PunchDate.Value.Date == data.temp1[i].FOEP_PunchDate.Value.Date && d.MI_Id == data.temp1[i].MI_Id).ToList(); //query.FirstOrDefault().HRME_Id
        //                    if (query2.Count == 0)
        //                    {
        //                        Adm_Student_PunchDMO dmo = new Adm_Student_PunchDMO();
        //                        dmo.CreatedDate = DateTime.Now;
        //                        dmo.ASPU_ActiveFlg = true;
        //                        dmo.ASPU_PunchDate = data.temp1[i].FOEP_PunchDate;
        //                        dmo.AMST_Id = query;
        //                        dmo.MI_Id = data.temp1[i].MI_Id;
        //                        _FOContext.Add(dmo);
        //                        _FOContext.SaveChanges();

        //                        Adm_Student_Punch_DetailsDMO dmo2 = new Adm_Student_Punch_DetailsDMO();
        //                        dmo2.MI_Id = data.temp1[i].MI_Id;
        //                        dmo2.ASPU_Id = dmo.ASPU_Id;
        //                        dmo2.ASPUD_InOutFlg = "I";
        //                        dmo2.ASPUD_PunchTime = Convert.ToDateTime(data.temp1[i].FOEPD_PunchTime).ToString("HH:mm");
        //                        _FOContext.Add(dmo2);
        //                        var flag = _FOContext.SaveChanges();
        //                        if (flag > 0)
        //                        {
        //                            data.returnval = true;
        //                        }
        //                        else
        //                        {
        //                            data.returnval = false;
        //                        }
        //                    }

        //                    else if (query2.Count > 0)
        //                    {
        //                        List<Adm_Student_PunchDTO> punchdata = new List<Adm_Student_PunchDTO>();
        //                        using (var cmd = _FOContext.Database.GetDbConnection().CreateCommand())
        //                        {
        //                            cmd.CommandTimeout = 300;
        //                            cmd.CommandText = "FO_getStudentPunchDetails";
        //                            cmd.CommandType = CommandType.StoredProcedure;
        //                            cmd.Parameters.Add(new SqlParameter("@ASPU_PunchDate", SqlDbType.DateTime)
        //                            {
        //                                Value = Convert.ToDateTime(data.temp1[i].FOEP_PunchDate)
        //                            });
        //                            cmd.Parameters.Add(new SqlParameter("@ASPUD_PunchTime", SqlDbType.VarChar)
        //                            {
        //                                Value = Convert.ToString(data.temp1[i].FOEPD_PunchTime)
        //                            });
        //                            cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt)
        //                            {
        //                                Value = Convert.ToInt64(query)
        //                            });
        //                            cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt)
        //                            {
        //                                Value = Convert.ToInt64(data.temp1[i].MI_Id)
        //                            });

        //                            if (cmd.Connection.State != ConnectionState.Open)
        //                                cmd.Connection.Open();

        //                            var retObject = new List<dynamic>();
        //                            try
        //                            {
        //                                using (var dataReader = cmd.ExecuteReader())
        //                                {
        //                                    while (dataReader.Read())
        //                                    {
        //                                        punchdata.Add(new Adm_Student_PunchDTO
        //                                        {
        //                                            ASPUD_Id = Convert.ToInt64(dataReader["ASPUD_Id"]),
        //                                            ASPUD_InOutFlg = Convert.ToString(dataReader["ASPUD_InOutFlg"]),
        //                                            ASPUD_PunchTime = Convert.ToString(dataReader["ASPUD_PunchTime"]),
        //                                        });
        //                                    }
        //                                }
        //                            }
        //                            catch (Exception ex)
        //                            {
        //                                Console.Write(ex.Message);
        //                            }
        //                        }

        //                        if (punchdata.Count == 0)
        //                        {
        //                            var query5 = _FOContext.Adm_Student_Punch_DetailsDMO.Where(d => d.ASPU_Id == query2[0].ASPU_Id && d.MI_Id == data.temp1[i].MI_Id).OrderByDescending(t => t.ASPUD_Id).ToList();

        //                            if (query5.Count > 0)
        //                            {
        //                                DateTime lastlog = Convert.ToDateTime(query5.FirstOrDefault().ASPUD_PunchTime);
        //                                DateTime stime1 = Convert.ToDateTime(data.temp1[i].FOEPD_PunchTime);
        //                                TimeSpan diff = stime1.Subtract(lastlog);
        //                                double totalMinutes = diff.TotalMinutes;
        //                                if (totalMinutes > 3.0)
        //                                {
        //                                    if (query5.FirstOrDefault().ASPUD_InOutFlg == "I")
        //                                    {
        //                                        var query15 = _FOContext.Adm_Student_Punch_DetailsDMO.Where(d => d.ASPU_Id == query2[0].ASPU_Id && d.MI_Id == data.temp1[i].MI_Id && d.ASPUD_PunchTime == data.temp1[i].FOEPD_PunchTime.Substring(0, 5).ToString()).ToList();
        //                                        if (query15.Count == 0)
        //                                        {
        //                                            Adm_Student_Punch_DetailsDMO dmo2 = new Adm_Student_Punch_DetailsDMO();
        //                                            dmo2.MI_Id = data.temp1[i].MI_Id;
        //                                            dmo2.ASPU_Id = query5.FirstOrDefault().ASPU_Id;
        //                                            dmo2.ASPUD_InOutFlg = "O";
        //                                            dmo2.ASPUD_PunchTime = Convert.ToDateTime(data.temp1[i].FOEPD_PunchTime).ToString("HH:mm");
        //                                            _FOContext.Add(dmo2);
        //                                        }

        //                                        var flag = _FOContext.SaveChanges();
        //                                        if (flag > 0)
        //                                        {
        //                                            data.returnval = true;
        //                                        }
        //                                        else
        //                                        {
        //                                            data.returnval = false;
        //                                        }
        //                                    }
        //                                    else if (query5.FirstOrDefault().ASPUD_InOutFlg == "O")
        //                                    {
        //                                        Adm_Student_Punch_DetailsDMO dmo2 = new Adm_Student_Punch_DetailsDMO();
        //                                        dmo2.MI_Id = data.temp1[i].MI_Id;
        //                                        dmo2.ASPU_Id = query5.FirstOrDefault().ASPU_Id;
        //                                        dmo2.ASPUD_InOutFlg = "I";
        //                                        dmo2.ASPUD_PunchTime = Convert.ToDateTime(data.temp1[i].FOEPD_PunchTime).ToString("HH:mm");
        //                                        _FOContext.Add(dmo2);
        //                                        var flag = _FOContext.SaveChanges();
        //                                        if (flag > 0)
        //                                        {
        //                                            data.returnval = true;
        //                                        }
        //                                        else
        //                                        {
        //                                            data.returnval = false;
        //                                        }
        //                                    }
        //                                }
        //                            }

        //                        }

        //                    }
        //                }
        //                //var stdclass = "";
        //                //var stdsection = "";
        //                //var EMPDEPT = "";
        //                //var EMPDES = "";
        //                //var code = "";

        //                //sendmail(data.temp1[i].MI_Id, "Praveenraikar124@gmail.com", "StudentPunchDetails", data.temp1[i].FOEP_PunchDate, data.temp1[i].FOEPD_PunchTime,"Praveenkumar Raikar",stdclass,stdsection, EMPDEPT, EMPDES,code);


        //            }
        //        }


                


        //    }
        //    catch (Exception e)
        //    {
        //        _log.LogInformation("Biometric impl");
        //        _log.LogDebug(e.Message);
        //    }
        //    return data;
        //}



        public string sendmail(long MI_Id, string Email, string Template, DateTime? LeaveDate, string Time, string Name,string stdclass,string stdsection,string EMPDEPT,string EMPDES,string code)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_MailActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "Email Template not Mapped to the selected Institution";
                }
                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();
                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "M").Select(d => d.ISMP_ID).ToList();
                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                string Mailmsg = template.FirstOrDefault().ISES_MailBody;
                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(Mailmsg, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                  Mailmsg = Mailmsg.Replace("[NAME]", Name);
                  Mailmsg = Mailmsg.Replace("[CLASS]", stdclass);
                  Mailmsg = Mailmsg.Replace("[SECTION]", stdsection);
                  Mailmsg = Mailmsg.Replace("[ADMNO]", code);
                  Mailmsg = Mailmsg.Replace("[DEPARTMENT]", EMPDEPT);
                  Mailmsg = Mailmsg.Replace("[DESIGNATION]", EMPDES);
                  Mailmsg = Mailmsg.Replace("[TIME]", Time);
                DateTime date = DateTime.Parse(LeaveDate.ToString());
                Mailmsg = Mailmsg.Replace("[DATE]", date.ToString("dd/MM/yyyy"));

                List<EMAIL_DETAILS_DMO> alldetails = new List<EMAIL_DETAILS_DMO>();
                alldetails = _db.EMAIL_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                string Attechement = "";
                if (alldetails.Count > 0)
                {
                    string SendingEmail = alldetails[0].IVRMMD_Mail_ID;
                    string SendingEmailPassword = alldetails[0].IVRMMD_Mail_PASSWORD;
                    string SendingEmailHostName = alldetails[0].IVRMMD_HOSTNAME;
                    Int32 PortNumber = Convert.ToInt32(alldetails[0].IVRMMD_PORTNO);
                    string Subject = template[0].ISES_MailSubject.ToString();
                    string sengridkey = alldetails[0].IVRM_sendgridkey.ToString();
                    string mailcc = "";
                    string mailbcc = "";
                    if (alldetails[0].IVRM_mailcc != null && alldetails[0].IVRM_mailcc != "")
                    {
                        string[] ccmail = alldetails[0].IVRM_mailcc.Split(',');

                        mailcc = ccmail[0].ToString();

                        if (ccmail.Length > 1)
                        {
                            if (ccmail[1] != null || ccmail[1] != "")
                            {
                                mailbcc = ccmail[1].ToString();
                            }
                        }

                    }
                    if (alldetails[0].IVRMMD_Attechement != null && alldetails[0].IVRMMD_Attechement != "")
                    {
                        Attechement = alldetails[0].IVRMMD_Attechement.ToString();
                    }

                    var message = new SendGridMessage();
                    message.From = new EmailAddress(SendingEmail, institutionName[0].MI_Name);
                    message.Subject = Subject;
                    message.AddTo(Email);

                    if (Attechement.Equals("1"))
                    {
                        var img = _db.IVRM_EMAIL_ATT_DMO.Where(i => i.ISES_Id == template[0].ISES_Id).ToList();

                        if (img.Count > 0)
                        {
                            for (int i = 0; i < img.Count; i++)
                            {
                                System.Net.HttpWebRequest request = System.Net.WebRequest.Create(img[i].IVRM_Att_Path) as HttpWebRequest;
                                System.Net.HttpWebResponse response = request.GetResponseAsync().Result as System.Net.HttpWebResponse;
                                Stream stream = response.GetResponseStream();
                                message.AddAttachment(stream.ToString(), img[i].IVRM_Att_Name);
                            }
                        }
                    }

                    if (mailcc != null && mailcc != "")
                    {
                        message.AddCc(mailcc);
                    }
                    if (mailbcc != null && mailbcc != "")
                    {
                        message.AddBcc(mailbcc);
                    }
                    message.HtmlContent = Mailmsg;
                    var client = new SendGridClient(sengridkey);
                    client.SendEmailAsync(message).Wait();
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_Email_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@EmailId",
                            SqlDbType.NVarChar)
                        {
                            Value = Email
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = Mailmsg
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename[0]
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd.ExecuteReader())
                            {
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }


    }
}




