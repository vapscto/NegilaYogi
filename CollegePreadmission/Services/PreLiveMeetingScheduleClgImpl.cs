using CommonLibrary;
using DataAccessMsSqlServerProvider;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs.com.vaps.College.Preadmission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace CollegePreadmission.Services
{
    public class PreLiveMeetingScheduleClgImpl : Interfaces.PreLiveMeetingScheduleClgInterface
    {
        public FeeGroupContext _fees;
       
        public DomainModelMsSqlServerContext _db;
    
        readonly ILogger<PreLiveMeetingScheduleClgImpl> _logger;

        public PreLiveMeetingScheduleClgImpl(FeeGroupContext fees, DomainModelMsSqlServerContext db, ILogger<PreLiveMeetingScheduleClgImpl> log)
        {
            _fees = fees;
         
            _db = db;
     
            _logger = log;
        }

        public PreLiveMeetingScheduleClgDTO getempdetails(PreLiveMeetingScheduleClgDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                var VCCODE = _db.Institution.Where(c => c.MI_Id == data.MI_Id && (c.MI_VCOthersFlag != null && c.MI_VCOthersFlag != "")).ToList();
                if (VCCODE.Count() > 0)
                {
                    data.vcflag = VCCODE.FirstOrDefault().MI_VCOthersFlag;
                }
                else
                {
                    data.vcflag = "JITSI";
                }

                //data.joinmeetinglist = (from a in _db.LMS_Live_MeetingDMO
                //                        from b in _db.HR_Master_Employee_DMO
                //                        from c in _db.LMS_Live_Meeting_StaffOthersDMO
                //                        where a.HRME_Id == b.HRME_Id && c.User_Id == data.UserId && a.MI_Id == data.MI_Id && (a.LMSLMEET_PlannedDate == DateTime.Now.Date || a.LMSLMEET_MeetingDate == DateTime.Now.Date) && (a.LMSLMEET_EndTime == null || a.LMSLMEET_EndTime == "") && a.LMSLMEET_Id == c.LMSLMEET_Id && a.LMSLMEET_ActiveFlg == true && (c.LMSLMEETSTFOTH_LogoutTime == null || c.LMSLMEETSTFOTH_LogoutTime == "")
                //                        select new LiveMeetingScheduleDTO
                //                        {
                //                            HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                //                            HRME_Id = b.HRME_Id,
                //                            LMSLMEET_Id = a.LMSLMEET_Id,
                //                            LMSLMEET_PlannedDate = a.LMSLMEET_PlannedDate,
                //                            LMSLMEET_PlannedStartTime = a.LMSLMEET_PlannedStartTime,
                //                            LMSLMEET_PlannedEndTime = a.LMSLMEET_PlannedEndTime,
                //                            LMSLMEET_MeetingDate = a.LMSLMEET_MeetingDate,
                //                            LMSLMEET_EndTime = a.LMSLMEET_EndTime,
                //                            LMSLMEET_StartedTime = a.LMSLMEET_StartedTime,
                //                            LMSLMEET_MeetingId = a.LMSLMEET_MeetingId,
                //                            LMSLMEET_MeetingTopic = a.LMSLMEET_MeetingTopic,
                //                        }
                //                 ).Distinct().OrderByDescending(w => w.LMSLMEET_PlannedDate).ToArray();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PreAdmmeetingschedulelist_College";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@userid",
                    SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@date",
                        SqlDbType.DateTime)
                    {
                        Value = indianTime.Date
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.meetinglist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "staffonlinecourserecordings";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Hrme_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.UserId
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.recordedmeetinglist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Pre_Totalmeetingsscheduled_college";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@userid",
                     SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@date",
                        SqlDbType.DateTime)
                    {
                        Value = indianTime.Date
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
                                       dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                    );
                                }
                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.totalmeetingsofday = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public PreLiveMeetingScheduleClgDTO ondatechangestudent(PreLiveMeetingScheduleClgDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PreStudent_Meeting_Profile_college";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@userid",
                       SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@date",
                    SqlDbType.Date)
                    {
                        Value = data.LMSLMEET_PlannedDate.ToString("yyyy-MM-dd")
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
                        data.joinmeetinglist = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }

        public PreLiveMeetingScheduleClgDTO onschedulecahnge(PreLiveMeetingScheduleClgDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);


                var roletyp = _db.MasterRoleType.Where(t => t.IVRMRT_Id == data.RoleId).ToList();
                data.roletype = roletyp.FirstOrDefault().IVRMRT_Role;
                if (!data.roletype.Equals("OnlinePreadmissionUser", StringComparison.OrdinalIgnoreCase))
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "PreAdmmeetingschedulelist_onChangeCollege";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@userid",
                        SqlDbType.BigInt)
                        {
                            Value = data.UserId
                        });
                        cmd.Parameters.Add(new SqlParameter("@date",
                        SqlDbType.DateTime)
                        {
                            Value = data.LMSLMEET_PlannedDate
                        });
                        cmd.Parameters.Add(new SqlParameter("@name",
                       SqlDbType.VarChar)
                        {
                            Value = data.LMSLMEET_MeetingTopic
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
                                           dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                        );
                                    }
                                    retObject.Add((ExpandoObject)dataRow);
                                }
                            }
                            data.meetinglist = retObject.ToArray();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "GETStudent_Meeting_Profile_College";
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
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                          SqlDbType.BigInt)
                        {
                            Value = data.Amst_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@MeetingDate",
                        SqlDbType.Date)
                        {
                            Value = indianTime.Date.ToString("yyyy-MM-dd")
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
                            data.joinmeetinglist = retObject.ToArray();
                        }

                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                }

                data.joinmeetinglist = (from a in _db.LMS_Live_MeetingDMO
                                        from b in _db.HR_Master_Employee_DMO
                                        from c in _db.LMS_Live_Meeting_StaffOthersDMO
                                        where a.HRME_Id == b.HRME_Id && c.HRME_Id == data.HRME_Id && a.MI_Id == data.MI_Id && (a.LMSLMEET_PlannedDate == data.LMSLMEET_PlannedDate.Date || a.LMSLMEET_MeetingDate == data.LMSLMEET_PlannedDate.Date) && (a.LMSLMEET_EndTime == null || a.LMSLMEET_EndTime == "") && a.LMSLMEET_Id == c.LMSLMEET_Id && a.LMSLMEET_ActiveFlg == true
                                        select new PreLiveMeetingScheduleClgDTO
                                        {
                                            HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                                            HRME_Id = b.HRME_Id,
                                            LMSLMEET_Id = a.LMSLMEET_Id,
                                            LMSLMEET_PlannedDate = a.LMSLMEET_PlannedDate,
                                            LMSLMEET_PlannedStartTime = a.LMSLMEET_PlannedStartTime,
                                            LMSLMEET_PlannedEndTime = a.LMSLMEET_PlannedEndTime,
                                            LMSLMEET_MeetingDate = a.LMSLMEET_MeetingDate,
                                            LMSLMEET_EndTime = a.LMSLMEET_EndTime,
                                            LMSLMEET_StartedTime = a.LMSLMEET_StartedTime
                                        }
                                  ).Distinct().OrderByDescending(w => w.LMSLMEET_PlannedDate).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public PreLiveMeetingScheduleClgDTO endmainmeetingstudent(PreLiveMeetingScheduleClgDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");



                //GET THE IP ADDRESS
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                String sMacAddress = string.Empty;
                foreach (NetworkInterface adapter in nics)
                {
                    if (sMacAddress == String.Empty)// only return MAC Address from first card
                    {
                        IPInterfaceProperties properties = adapter.GetIPProperties();
                        sMacAddress = adapter.GetPhysicalAddress().ToString();
                    }
                }
                var remoteIpAddress = "";
                //string netip = remoteIpAddress.ToString();

                string strHostName = "";
                strHostName = System.Net.Dns.GetHostName();

                IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

                IPAddress[] addr = ipEntry.AddressList;

                remoteIpAddress = addr[addr.Length - 1].ToString();

                string hostName = Dns.GetHostName();
                var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                //  string myIP1 = ip_list.ToString();
                string myIP1 = addr[addr.Length - 2].ToString();


                var ress = _db.LMS_Live_Meeting_PAStudent_CollegeDMO.Where(e => e.LMSLMEET_Id == data.LMSLMEET_Id).ToList();
                foreach (var item in ress)
                {
                    item.LMSLMEETPASTDC_LogoutTime = time;
                    item.LMSLMEETPASTDC_MACAddress = sMacAddress;
                    item.LMSLMEETPASTDC_IPAddress = myIP1;
                    item.LMSLMEETPASTDC_UpdatedBy = data.UserId;
                    item.LMSLMEETPASTDC_UpdatedDate = DateTime.Now;
                    _db.Update(item);
                }



                int ss = _db.SaveChanges();
                if (ss > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = true;
                }

            }




            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }


        public PreLiveMeetingScheduleClgDTO onstartmeeting(PreLiveMeetingScheduleClgDTO data)
        {
            try
            {
                data.createduser = false;
                var checkmeetingstartuser = _db.LMS_Live_MeetingDMO.Where(e => e.LMSLMEET_Id == data.LMSLMEET_Id && e.LMSLMEET_StartedByUserId > 0).ToList();
                if (checkmeetingstartuser.Count() > 0)
                {
                    var checkmeetingstartuserby = _db.LMS_Live_MeetingDMO.Where(e => e.LMSLMEET_Id == data.LMSLMEET_Id && e.LMSLMEET_StartedByUserId == data.UserId).ToList();
                    if (checkmeetingstartuserby.Count() > 0)
                    {
                        data.createduser = true;
                    }
                }
                else
                {
                    data.createduser = true;
                }

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");
                string meetingidinternal = "";
                bool start = false;
                //CREATE MEETING
                var VCCODECREATE = _db.Institution.Where(c => c.MI_Id == data.MI_Id && (c.MI_VCOthersFlag == "BBB")).ToList();
                if (VCCODECREATE.Count() > 0)
                {
                    var CheckDuplicateMeeting = _db.LMS_Live_MeetingDMO.Where(c => c.LMSLMEET_MeetingId == data.LMSLMEET_MeetingId && c.LMSLMEET_MeetingDate == null && (c.LMSLMEET_StartedTime == null || c.LMSLMEET_StartedTime == "")).ToList();
                    if (CheckDuplicateMeeting.Count() == 1)
                    {
                        start = true;
                        // BIGBLUE BUTTON START
                        Payment pay = new Payment(_db);
                        string moderatorpassword = "mp";
                        string attendeepassword = "ap";
                        string rnamee = data.LMSLMEET_MeetingTopic;
                        rnamee = rnamee.Replace(" ", "+");
                        string generatedchecksum = "";
                        string logouturl = "http://localhost:57606/#/app/PreStudentMeetingProfile/1196";
                        string callbackurl = "http://localhost:65140/api/PreLiveMeetingScheduleFacade/callback?meetingID=" + data.LMSLMEET_MeetingId;
                        string recordcallbackurl = "http://localhost:65140/api/PreLiveMeetingScheduleFacade/recordcallback";
                        //string callbackurl = "https://portalhub.azurewebsites.net/api/PreLiveMeetingScheduleFacade/callback?meetingID=" + data.LMSLMEET_MeetingId;
                        //string recordcallbackurl = "https://portalhub.azurewebsites.net/api/PreLiveMeetingScheduleFacade/recordcallback";

                        logouturl = HttpUtility.UrlEncode(logouturl);
                        callbackurl = HttpUtility.UrlEncode(callbackurl);
                        recordcallbackurl = HttpUtility.UrlEncode(recordcallbackurl);

                        string hash_string = "name=randomname&attendeePW=Apwd&meetingID=Mid&moderatorPW=Mpwd&logoutURL=lgout&meta_endCallbackUrl=callback&record=true&meta_bbb-recording-ready-url=recordurl&lockSettingsDisablePrivateChat=true";

                        hash_string = hash_string.Replace("randomname", rnamee);
                        hash_string = hash_string.Replace("Apwd", attendeepassword);
                        hash_string = hash_string.Replace("Mid", data.LMSLMEET_MeetingId);
                        hash_string = hash_string.Replace("Mpwd", moderatorpassword);
                        hash_string = hash_string.Replace("callback", callbackurl);
                        hash_string = hash_string.Replace("lgout", logouturl);
                        hash_string = hash_string.Replace("recordurl", recordcallbackurl);

                        generatedchecksum = pay.Generatehash256("create" + hash_string + "f8dKsKlHmpsQEK3Q5zHnNe7dqpeQDRqyG8Pv1j9C5k").ToLower();

                        string urlBB = "https://conference.vapssmartecampus.com/bigbluebutton/api/create?" + hash_string + "&checksum=" + generatedchecksum;

                        System.Net.WebRequest req = System.Net.WebRequest.Create(urlBB);
                        req.Method = "GET";
                        // req.Proxy = new System.Net.WebProxy(ProxyString, true); //true means no proxy
                        System.Net.WebResponse resp = req.GetResponseAsync().Result;
                        System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                        string s = sr.ReadToEnd().Trim();
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(s);
                        string jsonText = JsonConvert.SerializeXmlNode(doc);
                        var joResponse1 = JObject.Parse(jsonText);
                        meetingidinternal = Convert.ToString(joResponse1["response"]["internalMeetingID"]);
                        // BIGBLUE BUTTON END
                    }
                }

                //START MEETING MY MODERATER
                var VCCODE = _db.Institution.Where(c => c.MI_Id == data.MI_Id && (c.MI_VCOthersFlag == "BBB")).ToList();
                if (VCCODE.Count() > 0)
                {
                    var employee = _db.ApplicationUser.Where(c => c.Id == data.UserId).ToList();
                    string empname = "staffname";
                    if (employee.Count() > 0)
                    {
                        empname = employee.FirstOrDefault().UserName;
                    }
                    empname = empname.Trim();
                    empname = empname.Replace(" ", "+");
                    // join moderator url
                    Payment pay = new Payment(_db);

                    string hash_string = "fullName=" + empname + "&meetingID=" + data.LMSLMEET_MeetingId + "&password=mp&redirect=true";

                    string generatedchecksum = pay.Generatehash256("join" + hash_string + "f8dKsKlHmpsQEK3Q5zHnNe7dqpeQDRqyG8Pv1j9C5k").ToLower();

                    data.moderatorurl = "https://conference.vapssmartecampus.com/bigbluebutton/api/join?" + hash_string + "&checksum=" + generatedchecksum;

                    // join moderator url
                }

                if (data.createduser == true)
                {
                    //GET THE IP ADDRESS
                    NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                    String sMacAddress = string.Empty;
                    foreach (NetworkInterface adapter in nics)
                    {
                        if (sMacAddress == String.Empty)// only return MAC Address from first card
                        {
                            IPInterfaceProperties properties = adapter.GetIPProperties();
                            sMacAddress = adapter.GetPhysicalAddress().ToString();
                        }
                    }
                    var remoteIpAddress = "";
                    //string netip = remoteIpAddress.ToString();

                    string strHostName = "";
                    strHostName = System.Net.Dns.GetHostName();

                    IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

                    IPAddress[] addr = ipEntry.AddressList;

                    remoteIpAddress = addr[addr.Length - 1].ToString();

                    string hostName = Dns.GetHostName();
                    var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                    //  string myIP1 = ip_list.ToString();
                    string myIP1 = addr[addr.Length - 2].ToString();
                    var ress = _db.LMS_Live_MeetingDMO.Single(e => e.LMSLMEET_Id == data.LMSLMEET_Id);
                    ress.LMSLMEET_MeetingDate = indianTime.Date;
                    ress.LMSLMEET_StartedTime = time;
                    if (start == true)
                    {
                        ress.LMSLMEET_internalMeetingID = meetingidinternal;
                    }
                    ress.LMSLMEET_MACAddress = sMacAddress;
                    ress.LMSLMEET_IPAddress = myIP1;
                    ress.LMSLMEET_UpdatedBy = data.UserId;
                    ress.LMSLMEET_UpdatedDate = DateTime.Now;
                    ress.LMSLMEET_StartedByUserId = data.UserId;
                    //Fetch MeetingURL
                    data.LMSLMEET_MeetingURL = ress.LMSLMEET_MeetingURL;
                    //Fetch MeetingURL

                    _db.Update(ress);
                    int ss = _db.SaveChanges();
                    if (ss > 0)
                    {
                        data.returnval = true;

                        var student = _db.LMS_Live_Meeting_PAStudent_CollegeDMO.Where(e => e.LMSLMEET_Id == data.LMSLMEET_Id).ToList();
                        if (student.Count() > 0)
                        {
                            var pastudent = _db.PA_College_Application.Where(e => e.PACA_Id == student.FirstOrDefault().PACA_Id).ToList();
                            if (pastudent.Count > 0)
                            {
                                //Email Email = new Email(_db);
                                //string m = Email.sendmail(pastudent.FirstOrDefault().MI_Id, pastudent.FirstOrDefault().PASR_emailId, "VC_SCHEDULE_START", pastudent.FirstOrDefault().pasr_id);
                                if (data.createduser == true)
                                {
                                    SMS sms = new SMS(_db);
                                    sms.sendSms(pastudent.FirstOrDefault().MI_Id, pastudent.FirstOrDefault().PACA_MobileNo, "VC_SCHEDULE_START", pastudent.FirstOrDefault().PACA_Id);
                                }
                            }
                        }
                    }
                    else
                    {
                        data.returnval = true;
                    }
                }
                joinmeeting(data);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public PreLiveMeetingScheduleClgDTO joinmeeting(PreLiveMeetingScheduleClgDTO data)
        {
            try
            {

                if (data.HRME_Id == 0)
                {
                    var staff = _db.Staff_User_Login.Where(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).ToList();
                    if (staff.Count > 0)
                    {
                        data.HRME_Id = staff.FirstOrDefault().Emp_Code;
                    }
                }


                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");



                //GET THE IP ADDRESS
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                String sMacAddress = string.Empty;
                foreach (NetworkInterface adapter in nics)
                {
                    if (sMacAddress == String.Empty)// only return MAC Address from first card
                    {
                        IPInterfaceProperties properties = adapter.GetIPProperties();
                        sMacAddress = adapter.GetPhysicalAddress().ToString();
                    }
                }
                var remoteIpAddress = "";
                //string netip = remoteIpAddress.ToString();

                string strHostName = "";
                strHostName = System.Net.Dns.GetHostName();

                IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

                IPAddress[] addr = ipEntry.AddressList;

                remoteIpAddress = addr[addr.Length - 1].ToString();

                string hostName = Dns.GetHostName();
                var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                //  string myIP1 = ip_list.ToString();
                string myIP1 = addr[addr.Length - 2].ToString();

                var ress = _db.LMS_Live_Meeting_StaffOthersDMO.Single(e => e.LMSLMEET_Id == data.LMSLMEET_Id && e.User_Id == data.UserId);
                ress.LMSLMEETSTFOTH_LoginTime = time;
                ress.LMSLMEETSTFOTH_MACAddress = sMacAddress;
                ress.LMSLMEETSTFOTH_IPAddress = myIP1;
                ress.LMSLMEETSTFOTH_UpdatedBy = data.UserId;
                ress.LMSLMEETSTFOTH_CreatedBy = data.UserId;
                ress.LMSLMEETSTFOTH_UpdatedDate = DateTime.Now;
                ress.LMSLMEETSTFOTH_CreatedDate = DateTime.Now;
                _db.Update(ress);
                int ss = _db.SaveChanges();
                if (ss > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = true;
                }

            }



            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public PreLiveMeetingScheduleClgDTO saveremarks(PreLiveMeetingScheduleClgDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");

                var mainress = _db.LMS_Live_MeetingDMO.Where(e => e.LMSLMEET_Id == data.LMSLMEET_Id && e.User_Id == data.UserId).ToList();
                if (mainress.Count() > 0)
                {
                    var update = _db.LMS_Live_MeetingDMO.Single(e => e.LMSLMEET_Id == data.LMSLMEET_Id && e.User_Id == data.UserId);
                    update.LMSLMEET_Remarks = data.remarks;
                    update.LMSLMEET_Grade = data.grade;
                    update.LMSLMEET_UpdatedBy = data.UserId;
                    update.LMSLMEET_UpdatedDate = indianTime;
                    _db.Update(update);
                    int ups = _db.SaveChanges();
                    if (ups > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = true;
                    }
                }
                else
                {
                    var ress = _db.LMS_Live_Meeting_StaffOthersDMO.Single(e => e.LMSLMEET_Id == data.LMSLMEET_Id && e.User_Id == data.UserId);
                    ress.LMSLMEETSTFOTH_Remarks = data.remarks;
                    ress.LMSLMEETSTFOTH_Grade = data.grade;
                    ress.LMSLMEETSTFOTH_UpdatedBy = data.UserId;
                    _db.Update(ress);
                    int ss = _db.SaveChanges();
                    if (ss > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = true;
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public PreLiveMeetingScheduleClgDTO getstudentdetails(PreLiveMeetingScheduleClgDTO data)
        {
            try
            {
                var VCCODE = _db.Institution.Where(c => c.MI_Id == data.MI_Id && (c.MI_VCOthersFlag != null && c.MI_VCOthersFlag != "")).ToList();
                if (VCCODE.Count() > 0)
                {
                    data.vcflag = VCCODE.FirstOrDefault().MI_VCOthersFlag;
                }
                else
                {
                    data.vcflag = "JITSI";
                }

                var rolelist = _db.MasterRoleType.Where(t => t.IVRMRT_Id == data.RoleId).ToList();

                if (rolelist[0].IVRMRT_Role.Equals("OnlinePreadmissionUser", StringComparison.OrdinalIgnoreCase))
                {
                    data.staffstudentflag = true;
                }
                else
                {
                    data.staffstudentflag = false;
                }
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var joinedmeeting = (from a in _db.LMS_Live_Meeting_PAStudent_CollegeDMO
                                     from b in _db.LMS_Live_MeetingDMO
                                     from c in _db.PA_College_Application
                                     where b.MI_Id == data.MI_Id && a.LMSLMEET_Id == b.LMSLMEET_Id && a.PACA_Id == c.PACA_Id && c.ID == data.UserId && (a.LMSLMEETPASTDC_LogoutTime == null || a.LMSLMEETPASTDC_LogoutTime == "") && (a.LMSLMEETPASTDC_LogoutTime == null || a.LMSLMEETPASTDC_LogoutTime == "") && (b.LMSLMEET_PlannedDate.Date == indianTime.Date || b.LMSLMEET_MeetingDate == indianTime.Date) && (b.LMSLMEET_EndTime == null || b.LMSLMEET_EndTime == "")
                                     select b).Distinct().ToList();

                data.joinedmeeting = joinedmeeting.ToArray();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "PreStudent_Meeting_Profile_College";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@userid",
                      SqlDbType.BigInt)
                    {
                        Value = data.UserId
                    });
                    cmd.Parameters.Add(new SqlParameter("@date",
                    SqlDbType.Date)
                    {
                        Value = indianTime.Date.ToString("yyyy-MM-dd")
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
                        data.joinmeetinglist = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public PreLiveMeetingScheduleClgDTO joinmeetingStudent(PreLiveMeetingScheduleClgDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");

                var VCCODE = _db.Institution.Where(c => c.MI_Id == data.MI_Id && (c.MI_VCOthersFlag == "BBB")).ToList();
                if (VCCODE.Count() > 0)
                {

                    var student = _db.PA_College_Application.Where(c => c.MI_Id == data.MI_Id && c.PACA_Id == data.PACA_Id).ToList();
                    string studentname = "student";
                    string duplicatestudent = "student";
                    if (student.Count() > 0)
                    {
                        studentname = ((student.FirstOrDefault().PACA_FirstName == null ? "" : student.FirstOrDefault().PACA_FirstName.ToUpper()) + " " + (student.FirstOrDefault().PACA_MiddleName == null ? "" : student.FirstOrDefault().PACA_MiddleName.ToUpper()) + " " + (student.FirstOrDefault().PACA_LastName == null ? "" : student.FirstOrDefault().PACA_LastName.ToUpper())).Trim().ToString();
                    }
                    studentname = studentname.Trim();
                    studentname = studentname.Replace(" ", "+");

                    Payment meetingstatus = new Payment(_db);
                    string hash_string_check_status = "meetingID=" + data.LMSLMEET_MeetingId;
                    string checkstatus = meetingstatus.Generatehash256("isMeetingRunning" + hash_string_check_status + "f8dKsKlHmpsQEK3Q5zHnNe7dqpeQDRqyG8Pv1j9C5k").ToLower();
                    string meetingstatusurl = "https://conference.vapssmartecampus.com/bigbluebutton/api/isMeetingRunning?" + hash_string_check_status + "&checksum=" + checkstatus;

                    System.Net.WebRequest reqcheck = System.Net.WebRequest.Create(meetingstatusurl);
                    reqcheck.Method = "GET";
                    System.Net.WebResponse respcheck = reqcheck.GetResponseAsync().Result;
                    System.IO.StreamReader srcheck = new System.IO.StreamReader(respcheck.GetResponseStream());
                    string scheck = srcheck.ReadToEnd().Trim();
                    XmlDocument doccheck = new XmlDocument();
                    doccheck.LoadXml(scheck);
                    string jsonTextcheck = JsonConvert.SerializeXmlNode(doccheck);
                    JObject joResponse1check = JObject.Parse(jsonTextcheck);
                    data.meetingstatus = (bool)(joResponse1check["response"]["running"]);
                    if (data.meetingstatus == true)
                    {
                        // Check User In the Running Meeting
                        Payment checkjoined = new Payment(_db);
                        string hash_string_check = "meetingID=" + data.LMSLMEET_MeetingId + "&password=mp";
                        string generatedchecksumjoined = checkjoined.Generatehash256("getMeetingInfo" + hash_string_check + "f8dKsKlHmpsQEK3Q5zHnNe7dqpeQDRqyG8Pv1j9C5k").ToLower();
                        string duplicatecheckurl = "https://conference.vapssmartecampus.com/bigbluebutton/api/getMeetingInfo?" + hash_string_check + "&checksum=" + generatedchecksumjoined;

                        System.Net.WebRequest req = System.Net.WebRequest.Create(duplicatecheckurl);
                        req.Method = "GET";
                        System.Net.WebResponse resp = req.GetResponseAsync().Result;
                        System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                        string s = sr.ReadToEnd().Trim();
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(s);
                        string jsonText = JsonConvert.SerializeXmlNode(doc);
                        JObject joResponse1 = JObject.Parse(jsonText);
                        //List<string>  = Convert.ToString(joResponse1["response"]["attendees"]);
                        JObject array = (JObject)joResponse1["response"];
                        try
                        {
                            JObject array1 = (JObject)array["attendees"];
                            JArray array2 = (JArray)array1["attendee"];
                            foreach (JToken page in array2)
                            {
                                string key = page["fullName"].ToString();
                                if (key == duplicatestudent)
                                {
                                    data.joined = true;
                                }
                            }
                        }
                        catch (Exception ee)
                        {
                            if (ee.Message == null || ee.Message == "Unable to cast object of type 'Newtonsoft.Json.Linq.JValue' to type 'Newtonsoft.Json.Linq.JObject'.")
                            {
                                data.joined = false;
                            }
                        }

                        if (data.joined == false)
                        {
                            // join moderator url
                            Payment pay = new Payment(_db);
                            string hash_string = "fullName=" + studentname + "&meetingID=" + data.LMSLMEET_MeetingId + "&password=ap&redirect=true";
                            string generatedchecksum = pay.Generatehash256("join" + hash_string + "f8dKsKlHmpsQEK3Q5zHnNe7dqpeQDRqyG8Pv1j9C5k").ToLower();
                            data.moderatorurl = "https://conference.vapssmartecampus.com/bigbluebutton/api/join?" + hash_string + "&checksum=" + generatedchecksum;
                            // join moderator url
                        }
                    }
                }

                var Teamsurl = _db.Institution.Where(c => c.MI_Id == data.MI_Id && (c.MI_VCStudentFlag == "Teams")).ToList();
                if (Teamsurl.Count() > 0)
                {
                    var geturl = _db.LMS_Live_MeetingDMO.Single(e => e.LMSLMEET_Id == data.LMSLMEET_Id);

                    //Fetch MeetingURL
                    data.LMSLMEET_MeetingURL = geturl.LMSLMEET_MeetingURL;
                }

                //GET THE IP ADDRESS
                NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
                String sMacAddress = string.Empty;
                foreach (NetworkInterface adapter in nics)
                {
                    if (sMacAddress == String.Empty)// only return MAC Address from first card
                    {
                        IPInterfaceProperties properties = adapter.GetIPProperties();
                        sMacAddress = adapter.GetPhysicalAddress().ToString();
                    }
                }
                var remoteIpAddress = "";
                //string netip = remoteIpAddress.ToString();

                string strHostName = "";
                strHostName = System.Net.Dns.GetHostName();

                IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

                IPAddress[] addr = ipEntry.AddressList;

                remoteIpAddress = addr[addr.Length - 1].ToString();

                string hostName = Dns.GetHostName();
                var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                //  string myIP1 = ip_list.ToString();
                string myIP1 = addr[addr.Length - 2].ToString();

                var joinedmeeting = (from a in _db.LMS_Live_Meeting_PAStudent_CollegeDMO
                                     from b in _db.LMS_Live_MeetingDMO
                                     from c in _db.PA_College_Application
                                     where b.MI_Id == data.MI_Id && a.LMSLMEET_Id == b.LMSLMEET_Id && a.PACA_Id == c.PACA_Id && c.ID == data.UserId && (a.LMSLMEETPASTDC_LogoutTime == null || a.LMSLMEETPASTDC_LogoutTime == "") && (b.LMSLMEET_PlannedDate.Date == indianTime.Date || b.LMSLMEET_MeetingDate == indianTime.Date) && (b.LMSLMEET_EndTime == null || b.LMSLMEET_EndTime == "")
                                     select b).Distinct().ToList();

                data.joinedmeeting = joinedmeeting.ToArray();

                var ress = _db.LMS_Live_Meeting_PAStudent_CollegeDMO.Single(e => e.LMSLMEET_Id == data.LMSLMEET_Id && e.PACA_Id == data.PACA_Id);
                ress.LMSLMEETPASTDC_LoginTime = time;
                ress.LMSLMEETPASTDC_MACAddress = sMacAddress;
                ress.LMSLMEETPASTDC_IPAddress = myIP1;
                ress.LMSLMEETPASTDC_UpdatedBy = data.UserId;
                ress.LMSLMEETPASTDC_UpdatedDate = DateTime.Now;
                _db.Update(ress);
                int ss = _db.SaveChanges();
                if (ss > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = true;
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
    }
}
