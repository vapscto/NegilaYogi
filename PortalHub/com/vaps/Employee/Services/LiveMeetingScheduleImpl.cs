using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Portals.Employee;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs.com.vaps.Exam;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Concurrent;
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
using RestSharp;
using PreadmissionDTOs;
using System.IO;
using PreadmissionDTOs.com.vaps.IssueManager;
using System.Timers;
using System.Net.Http;
using System.Net.Http.Headers;

namespace PortalHub.com.vaps.Employee.Services
{
    public class LiveMeetingScheduleImpl : Interfaces.LiveMeetingScheduleInterface
    {
        public FeeGroupContext _fees;
        public ExamContext _exam;
        public  DomainModelMsSqlServerContext _db;       
        public  DomainModelMsSqlServerContext _newdb;       
        public ExamContext _examcontext;
        readonly ILogger<LiveMeetingScheduleImpl> _logger;  
        public long meetingid=0;
        public long localmiid=0;
        public Int32 count = 0;
        private System.Timers.Timer timer = new System.Timers.Timer();
        public LiveMeetingScheduleImpl(FeeGroupContext fees, ExamContext exm, DomainModelMsSqlServerContext db, ExamContext examcontext, ILogger<LiveMeetingScheduleImpl> log,DomainModelMsSqlServerContext newdb)
        {
            _fees = fees;
            _exam = exm;
            _db = db;
            _examcontext = examcontext;
            _logger = log;
            _newdb = newdb;
        }
        public LiveMeetingScheduleDTO getstudentdetails(LiveMeetingScheduleDTO data)
        {
            try
            {
                var VCCODE = _db.Institution.Where(c => c.MI_Id == data.MI_Id && (c.MI_VCStudentFlag != null && c.MI_VCStudentFlag != "")).ToList();
                if (VCCODE.Count() > 0)
                {
                    data.vcflag = VCCODE.FirstOrDefault().MI_VCStudentFlag;
                }
                else
                {
                    data.vcflag = "BBB";
                }
                var rolelist = _db.MasterRoleType.Where(t => t.IVRMRT_Id == data.RoleId).ToList();
                if (rolelist[0].IVRMRT_Role.Equals("Student"))
                {
                    data.staffstudentflag = true;
                }
                else
                {
                    data.staffstudentflag = false;
                }

                var fillstudentalldetails = (from a in _fees.AdmissionStudentDMO
                                             from b in _fees.School_Adm_Y_StudentDMO
                                             from c in _fees.admissioncls
                                             from d in _fees.school_M_Section
                                             from e in _fees.AcademicYear
                                             where (b.ASMCL_Id == c.ASMCL_Id && d.ASMS_Id == b.ASMS_Id && b.ASMAY_Id == e.ASMAY_Id
                                             && a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMST_Id == data.Amst_Id
                                             && a.AMST_Id == data.Amst_Id && b.AMAY_ActiveFlag == 1 && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1)
                                             select new LiveMeetingScheduleDTO
                                             {
                                                 amst_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),
                                                 amst_MiddleName = a.AMST_MiddleName,
                                                 amst_LastName = a.AMST_LastName,
                                                 amst_RegistrationNo = a.AMST_RegistrationNo,
                                                 amst_AdmNo = a.AMST_AdmNo,
                                                 amay_RollNo = b.AMAY_RollNo,
                                                 classname = c.ASMCL_ClassName,
                                                 sectionname = d.ASMC_SectionName,
                                                 fathername = a.AMST_FatherName,
                                                 mothername = a.AMST_MotherName,
                                                 bloodgroup = a.AMST_BloodGroup,
                                                 address1 = a.AMST_PerStreet,
                                                 address2 = a.AMST_PerArea,
                                                 address3 = a.AMST_PerCity,
                                                 fathermobileno = a.AMST_FatherMobleNo,
                                                 studentdob = a.AMST_DOB,
                                                 amst_mobile = a.AMST_MobileNo,
                                                 amst_sex = a.AMST_Sex,
                                                 amst_dob = a.AMST_DOB,
                                                 amst_emailid = a.AMST_emailId,
                                                 asma_year = e.ASMAY_Year,
                                                 AMST_Photoname = a.AMST_Photoname,
                                                 ASMCL_Id = b.ASMCL_Id,
                                                 ASMS_Id = b.ASMS_Id
                                             }
                    ).Distinct().ToList();

                data.fillstudentalldetails = fillstudentalldetails.ToArray();

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var joinedmeeting = (from a in _db.LMS_Live_Meeting_StudentDMO
                                     from b in _db.LMS_Live_MeetingDMO
                                     where b.MI_Id == data.MI_Id && a.LMSLMEET_Id == b.LMSLMEET_Id && a.AMST_Id == data.Amst_Id && (a.LMSLMEETSTD_LogoutTime == null || a.LMSLMEETSTD_LogoutTime == "") && (b.LMSLMEET_PlannedDate.Date == indianTime.Date || b.LMSLMEET_MeetingDate == indianTime.Date) && (b.LMSLMEET_EndTime == null || b.LMSLMEET_EndTime == "")
                                     select b).Distinct().ToList();
                data.joinedmeeting = joinedmeeting.ToArray();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GETStudent_Meeting_Profile";
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

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "studentonlineclassrecordings";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                    SqlDbType.BigInt)
                    {
                        Value = fillstudentalldetails.FirstOrDefault().ASMCL_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                    SqlDbType.BigInt)
                    {
                        Value = fillstudentalldetails.FirstOrDefault().ASMS_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
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


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public LiveMeetingScheduleDTO ondatechangestudent(LiveMeetingScheduleDTO data)
        {
            try
            {

                var joinedmeeting = (from a in _db.LMS_Live_Meeting_StudentDMO
                                     from b in _db.LMS_Live_MeetingDMO
                                     where b.MI_Id == data.MI_Id && a.LMSLMEET_Id == b.LMSLMEET_Id && a.AMST_Id == data.Amst_Id && (a.LMSLMEETSTD_LogoutTime == null || a.LMSLMEETSTD_LogoutTime == "") && (b.LMSLMEET_PlannedDate.Date == data.LMSLMEET_PlannedDate.Date || b.LMSLMEET_MeetingDate == data.LMSLMEET_PlannedDate.Date) && (b.LMSLMEET_EndTime == null || b.LMSLMEET_EndTime == "")
                                     select b).Distinct().ToList();
                data.joinedmeeting = joinedmeeting.ToArray();


                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GETStudent_Meeting_Profile";
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
                    SqlDbType.VarChar)
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
        public LiveMeetingScheduleDTO joinmeetingStudent(LiveMeetingScheduleDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");

                var VCCODE = _db.Institution.Where(c => c.MI_Id == data.MI_Id && (c.MI_VCStudentFlag == "BBB")).ToList();
                if (VCCODE.Count() > 0)
                {
                    var studentid = (from stud in _db.StudentAppUserLoginDMO where stud.STD_APP_ID == data.UserId select stud.AMST_ID);
                    long amstid = studentid.First();
                    var student = _db.Adm_M_Student.Where(c => c.MI_Id == data.MI_Id && c.AMST_Id == amstid).ToList();
                    string studentname = "student";
                    string duplicatestudent = "student";
                    if (student.Count() > 0)
                    {
                        studentname = ((student.FirstOrDefault().AMST_FirstName == null ? "" : student.FirstOrDefault().AMST_FirstName.ToUpper()) + " " + (student.FirstOrDefault().AMST_MiddleName == null ? "" : student.FirstOrDefault().AMST_MiddleName.ToUpper()) + " " + (student.FirstOrDefault().AMST_LastName == null ? "" : student.FirstOrDefault().AMST_LastName.ToUpper())).Trim().ToString();

                        var studentcurrent = (from a in _db.Adm_M_Student
                                              from b in _db.School_Adm_Y_StudentDMO
                                              where a.AMST_Id == b.AMST_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == amstid
                                              select new LiveMeetingScheduleDTO
                                              {
                                                  ASMCL_Id = b.ASMCL_Id,
                                                  ASMS_Id = b.ASMS_Id,
                                                  AMST_FirstName = a.AMST_FirstName,
                                                  AMST_MiddleName = a.AMST_MiddleName,
                                                  AMST_LastName = a.AMST_LastName,
                                                  amst_AdmNo = a.AMST_AdmNo
                                              }
                                         ).Distinct().ToList();

                        using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Duplicate_studentName_Check";
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
                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                              SqlDbType.BigInt)
                            {
                                Value = studentcurrent.FirstOrDefault().ASMCL_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                              SqlDbType.BigInt)
                            {
                                Value = studentcurrent.FirstOrDefault().ASMS_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@studentname",
                              SqlDbType.VarChar)
                            {
                                Value = studentcurrent.FirstOrDefault().AMST_FirstName.Trim() + studentcurrent.FirstOrDefault().AMST_MiddleName.Trim() + studentcurrent.FirstOrDefault().AMST_LastName.Trim()
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
                                data.duplicatestudentlist = retObject.ToArray();
                            }

                            catch (Exception ex)
                            {
                                Console.Write(ex.Message);
                            }
                        }

                        if (data.duplicatestudentlist.Length > 1)
                        {
                            studentname = studentname.Replace("  ", " ");
                            studentname = studentname + "-" + studentcurrent.FirstOrDefault().amst_AdmNo;
                            duplicatestudent = studentname.Trim();
                        }
                        else
                        {
                            studentname = studentname.Replace("  ", " ");
                            duplicatestudent = studentname.Trim();
                        }
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

                var Teamsurl = _db.Institution.Where(c => c.MI_Id == data.MI_Id && (c.MI_VCStudentFlag.Equals("Teams", StringComparison.OrdinalIgnoreCase))).ToList();
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

                LMS_Live_Meeting_StudentDMO ress = new LMS_Live_Meeting_StudentDMO();

                ress.AMST_Id = data.Amst_Id;
                ress.LMSLMEET_Id = data.LMSLMEET_Id;
                ress.LMSLMEETSTD_LoginTime = time;
                ress.LMSLMEETSTD_MACAddress = sMacAddress;
                ress.LMSLMEETSTD_IPAddress = myIP1;
                ress.LMSLMEETSTD_UpdatedBy = data.UserId;
                ress.LMSLMEETSTD_CreatedBy = data.UserId;
                ress.LMSLMEETSTD_CreatedDate = DateTime.Now;
                ress.LMSLMEETSTD_UpdatedDate = DateTime.Now;
                ress.LMSLMEETSTD_ActiveFlg = true;
                _db.Add(ress);
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
        public LiveMeetingScheduleDTO joinmeeting(LiveMeetingScheduleDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");

                if (data.HRME_Id == 0)
                {
                    var HRME_Idlist = _exam.Staff_User_Login.Where(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).ToList();
                    if (HRME_Idlist.Count > 0)
                    {
                        data.HRME_Id = HRME_Idlist.FirstOrDefault().Emp_Code;
                    }
                }

                var VCCODE = _db.Institution.Where(c => c.MI_Id == data.MI_Id && (c.MI_VCStudentFlag == "BBB")).ToList();
                if (VCCODE.Count() > 0)
                {
                    var studentid = (from stud in _db.Staff_User_Login where stud.Id == data.UserId select stud.Emp_Code);
                    long amstid = studentid.First();
                    var student = _db.HR_Master_Employee_DMO.Where(c => c.MI_Id == data.MI_Id && c.HRME_Id == amstid).ToList();
                    string studentname = "student";
                    string duplicatestudent = "student";
                    if (student.Count() > 0)
                    {
                        studentname = ((student.FirstOrDefault().HRME_EmployeeFirstName == null ? "" : student.FirstOrDefault().HRME_EmployeeFirstName.ToUpper()) + " " + (student.FirstOrDefault().HRME_EmployeeMiddleName == null ? "" : student.FirstOrDefault().HRME_EmployeeMiddleName.ToUpper()) + " " + (student.FirstOrDefault().HRME_EmployeeLastName == null ? "" : student.FirstOrDefault().HRME_EmployeeLastName.ToUpper())).Trim().ToString();

                        var studentcurrent = (from a in _db.Adm_M_Student
                                              from b in _db.School_Adm_Y_StudentDMO
                                              where a.AMST_Id == b.AMST_Id && b.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == amstid
                                              select new LiveMeetingScheduleDTO
                                              {
                                                  ASMCL_Id = b.ASMCL_Id,
                                                  ASMS_Id = b.ASMS_Id,
                                                  AMST_FirstName = a.AMST_FirstName,
                                                  AMST_MiddleName = a.AMST_MiddleName,
                                                  AMST_LastName = a.AMST_LastName,
                                                  amst_AdmNo = a.AMST_AdmNo
                                              }
                                         ).Distinct().ToList();

                        studentname = studentname.Replace("  ", " ");
                        duplicatestudent = studentname.Trim();
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

                var Teamsurl = _db.Institution.Where(c => c.MI_Id == data.MI_Id && (c.MI_VCStudentFlag.Equals("Teams", StringComparison.OrdinalIgnoreCase))).ToList();
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
        //STAFF PROFILE
        public LiveMeetingScheduleDTO getempdetails(LiveMeetingScheduleDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                if (data.HRME_Id == 0)
                {
                    var HRME_Idlist = _exam.Staff_User_Login.Where(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).ToList();
                    if (HRME_Idlist.Count > 0)
                    {
                        data.HRME_Id = HRME_Idlist.FirstOrDefault().Emp_Code;
                    }
                }
                var VCCODE = _db.Institution.Where(c => c.MI_Id == data.MI_Id && (c.MI_VCStudentFlag != null && c.MI_VCStudentFlag != "")).ToList();
                if (VCCODE.Count() > 0)
                {
                    data.vcflag = VCCODE.FirstOrDefault().MI_VCStudentFlag;
                }
                else
                {
                    data.vcflag = "BBB";
                }

                //data.stafflist = (from a in _db.HR_Master_Employee_DMO
                //                  from c in _db.HR_Master_Designation
                //                  from d in _db.HR_Master_Department
                //                  from e in _db.IVRM_Master_Gender
                //                  where (a.HRMDES_Id == c.HRMDES_Id && a.HRMD_Id == d.HRMD_Id && a.IVRMMG_Id == e.IVRMMG_Id && a.MI_Id.Equals(data.MI_Id) && a.HRME_Id == data.HRME_Id && a.HRME_TechNonTeachingFlg == "Teaching")
                //                  select new LiveMeetingScheduleDTO
                //                  {
                //                      HRME_Id = a.HRME_Id,
                //                      MI_Id = a.MI_Id,
                //                      HRMD_DepartmentName = d.HRMD_DepartmentName,
                //                      HRMDES_DesignationName = c.HRMDES_DesignationName,
                //                      HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "0" ? "" : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "0" ? "" : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "0" ? "" : a.HRME_EmployeeLastName)).Trim(),
                //                      HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName,
                //                      HRME_EmployeeLastName = a.HRME_EmployeeLastName,
                //                      HRME_EmployeeCode = a.HRME_EmployeeCode,
                //                      HRME_BiometricCode = a.HRME_BiometricCode,
                //                      HRME_PerStreet = a.HRME_PerStreet,
                //                      HRME_PerArea = a.HRME_PerArea,
                //                      HRME_PerCity = a.HRME_PerCity,
                //                      HRME_PerStateId = a.HRME_PerStateId,
                //                      HRME_PerCountryId = a.HRME_PerCountryId,
                //                      HRME_PerPincode = a.HRME_PerPincode,
                //                      HRME_LocStreet = a.HRME_LocStreet,
                //                      HRME_LocArea = a.HRME_LocArea,
                //                      HRME_LocCity = a.HRME_LocCity,
                //                      HRME_LocStateId = a.HRME_LocStateId,
                //                      HRME_LocCountryId = a.HRME_LocCountryId,
                //                      HRME_LocPincode = a.HRME_LocPincode,
                //                      IVRMMG_Id = a.IVRMMG_Id,
                //                      IVRMMG_GenderName = e.IVRMMG_GenderName,
                //                      CasteId = a.CasteId,
                //                      HRME_FatherName = a.HRME_FatherName,
                //                      HRME_MotherName = a.HRME_MotherName,
                //                      HRME_DOB = a.HRME_DOB,
                //                      HRME_DOJ = a.HRME_DOJ,
                //                      HRME_ExpectedRetirementDate = a.HRME_ExpectedRetirementDate,
                //                      HRME_PFDate = a.HRME_PFDate,
                //                      HRME_ESIDate = a.HRME_ESIDate,
                //                      HRME_MobileNo = a.HRME_Id,
                //                      HRME_EmailId = a.HRME_EmailId,
                //                      HRME_BloodGroup = a.HRME_BloodGroup,

                //                      HRME_Photo = a.HRME_Photo,

                //                  }).Distinct().ToArray();


                //data.joinmeetinglist = (from a in _db.LMS_Live_MeetingDMO
                //                        from b in _db.HR_Master_Employee_DMO
                //                        from c in _db.LMS_Live_Meeting_StaffOthersDMO
                //                        where a.HRME_Id == b.HRME_Id && (c.HRME_Id == data.HRME_Id || c.User_Id==data.UserId) && a.MI_Id == data.MI_Id && (a.LMSLMEET_PlannedDate == DateTime.Now.Date || a.LMSLMEET_MeetingDate == DateTime.Now.Date) && (a.LMSLMEET_EndTime == null || a.LMSLMEET_EndTime == "") && a.LMSLMEET_Id == c.LMSLMEET_Id && a.LMSLMEET_ActiveFlg == true && (c.LMSLMEETSTFOTH_LogoutTime == null || c.LMSLMEETSTFOTH_LogoutTime == "")
                //                        select new LiveMeetingScheduleDTO
                //                        {
                //                            HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "0" ? "" : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "0" ? "" : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "0" ? "" : b.HRME_EmployeeLastName)).Trim(),
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
                    cmd.CommandText = "Joinstaffonlineclasslista";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Hrme_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@date",
                        SqlDbType.DateTime)
                    {
                        Value = indianTime.Date
                    });
                    cmd.Parameters.Add(new SqlParameter("@userid",
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
                        data.joinmeetinglist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                var joinedmeeting = (from a in _db.LMS_Live_Meeting_StaffOthersDMO
                                     from b in _db.LMS_Live_MeetingDMO
                                     where b.MI_Id == data.MI_Id && a.LMSLMEET_Id == b.LMSLMEET_Id && (a.HRME_Id == data.HRME_Id || a.User_Id == data.UserId) && (a.LMSLMEETSTFOTH_LogoutTime == null || a.LMSLMEETSTFOTH_LogoutTime == "") && (b.LMSLMEET_PlannedDate.Date == indianTime.Date || b.LMSLMEET_MeetingDate == indianTime.Date) && (b.LMSLMEET_EndTime == null || b.LMSLMEET_EndTime == "")
                                     select b).Distinct().ToList();
                data.joinedmeeting = joinedmeeting.ToArray();

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "staffonlineclasslista";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Hrme_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
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
                    cmd.CommandText = "staffonlineclassrecordings";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Hrme_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
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

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }           
            return data;
        }
        public LiveMeetingScheduleDTO ondatechange(LiveMeetingScheduleDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                if (data.HRME_Id == 0)
                {
                    var HRME_Idlist = _exam.Staff_User_Login.Where(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).ToList();
                    if (HRME_Idlist.Count > 0)
                    {
                        data.HRME_Id = HRME_Idlist.FirstOrDefault().Emp_Code;
                    }
                }

                var roletyp = _db.MasterRoleType.Where(t => t.IVRMRT_Id == data.RoleId).ToList();
                data.roletype = roletyp.FirstOrDefault().IVRMRT_Role;
                if (data.roletype.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "staffonlineclasslista";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Hrme_Id",
                        SqlDbType.BigInt)
                        {
                            Value = data.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@date",
                        SqlDbType.DateTime)
                        {
                            Value = data.LMSLMEET_PlannedDate
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
                        cmd.CommandText = "GETStudent_Meeting_Profile";
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

                //data.joinmeetinglist = (from a in _db.LMS_Live_MeetingDMO
                //                        from b in _db.HR_Master_Employee_DMO
                //                        from c in _db.LMS_Live_Meeting_StaffOthersDMO
                //                        where a.HRME_Id == b.HRME_Id && (c.HRME_Id == data.HRME_Id || c.User_Id == data.UserId) && a.MI_Id == data.MI_Id && (a.LMSLMEET_PlannedDate == DateTime.Now.Date || a.LMSLMEET_MeetingDate == DateTime.Now.Date) && (a.LMSLMEET_EndTime == null || a.LMSLMEET_EndTime == "") && a.LMSLMEET_Id == c.LMSLMEET_Id && a.LMSLMEET_ActiveFlg == true && (c.LMSLMEETSTFOTH_LogoutTime == null || c.LMSLMEETSTFOTH_LogoutTime == "")
                //                        select new LiveMeetingScheduleDTO
                //                        {
                //                            HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "0" ? "" : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "0" ? "" : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "0" ? "" : b.HRME_EmployeeLastName)).Trim(),
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
                    cmd.CommandText = "Joinstaffonlineclasslista";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Hrme_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@date",
                        SqlDbType.DateTime)
                    {
                        Value = data.LMSLMEET_PlannedDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@userid",
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
                        data.joinmeetinglist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                var joinedmeeting = (from a in _db.LMS_Live_Meeting_StaffOthersDMO
                                     from b in _db.LMS_Live_MeetingDMO
                                     where b.MI_Id == data.MI_Id && a.LMSLMEET_Id == b.LMSLMEET_Id && (a.HRME_Id == data.HRME_Id || a.User_Id == data.UserId) && (a.LMSLMEETSTFOTH_LogoutTime == null || a.LMSLMEETSTFOTH_LogoutTime == "") && (b.LMSLMEET_PlannedDate.Date == indianTime.Date || b.LMSLMEET_MeetingDate == indianTime.Date) && (b.LMSLMEET_EndTime == null || b.LMSLMEET_EndTime == "")
                                     select b).Distinct().ToList();
                data.joinedmeeting = joinedmeeting.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public LiveMeetingScheduleDTO onstartmeeting(LiveMeetingScheduleDTO data)
        {
            try
            {
                bool meetingstarted = false;
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");
                string meetingidinternal = "";
                bool meetingstartedfirst = false;
                var checkstrated= _db.LMS_Live_MeetingDMO.Where(e => e.LMSLMEET_Id == data.LMSLMEET_Id).ToList();
                if(checkstrated.Count()>0)
                {
                    if(checkstrated.FirstOrDefault().LMSLMEET_StartedTime==null && checkstrated.FirstOrDefault().LMSLMEET_MeetingDate == null)
                    {
                        meetingstartedfirst = true;
                    }                    
                }

                bool start = false;
                //CREATE MEETING
                var VCCODECREATE = _db.Institution.Where(c => c.MI_Id == data.MI_Id && (c.MI_VCStudentFlag == "BBB")).ToList();
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
                        string logouturl = "http://localhost:57606/#/app/StudentMeetingProfile/1196";
                        //string callbackurl = "http://localhost:51263/api/LiveMeetingScheduleFacade/callback?meetingID=" + data.LMSLMEET_MeetingId;
                        //string recordcallbackurl = "http://localhost:51263/api/LiveMeetingScheduleFacade/recordcallback";
                        string callbackurl = "https://portalhub.azurewebsites.net/api/LiveMeetingScheduleFacade/callback?meetingID=" + data.LMSLMEET_MeetingId;
                        string recordcallbackurl = "https://portalhub.azurewebsites.net/api/LiveMeetingScheduleFacade/recordcallback";

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
                var VCCODE = _db.Institution.Where(c => c.MI_Id == data.MI_Id && (c.MI_VCStudentFlag == "BBB")).ToList();
                if (VCCODE.Count() > 0)
                {
                    var employee = _db.HR_Master_Employee_DMO.Where(c => c.MI_Id == data.MI_Id && c.HRME_Id == data.HRME_Id).ToList();
                    string empname = "staffname";
                    if (employee.Count() > 0)
                    {
                        empname = ((employee.FirstOrDefault().HRME_EmployeeFirstName == null ? "" : employee.FirstOrDefault().HRME_EmployeeFirstName.ToUpper()) + " " + (employee.FirstOrDefault().HRME_EmployeeMiddleName == null ? "" : employee.FirstOrDefault().HRME_EmployeeMiddleName.ToUpper()) + " " + (employee.FirstOrDefault().HRME_EmployeeLastName == null ? "" : employee.FirstOrDefault().HRME_EmployeeLastName.ToUpper())).Trim().ToString();
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
                if (ress.LMSLMEET_StartedTime == "" || ress.LMSLMEET_StartedTime == null)
                {
                    meetingstarted = true;
                }
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

                //Fetch MeetingURL
                data.LMSLMEET_MeetingURL = ress.LMSLMEET_MeetingURL;
                //Fetch MeetingURL

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
                if (data.returnval == true && meetingstarted == true)
                {
                    var devicelist = (from a in _db.HR_Master_Employee_DMO
                                      from b in _db.Staff_User_Login
                                      from c in _db.LMS_Live_Meeting_StaffOthersDMO
                                      where (a.MI_Id == data.MI_Id && a.HRME_Id == b.Emp_Code && b.Id == c.User_Id && c.LMSLMEET_Id == data.LMSLMEET_Id && a.HRME_AppDownloadedDeviceId != null && a.HRME_AppDownloadedDeviceId != "")
                                      select new IVRM_Homework_DTO
                                      {
                                          AMST_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId
                                      }).Distinct().ToList();


                    IVRM_Homework_DTO dto = new IVRM_Homework_DTO();
                    dto.devicelist12 = devicelist;
                    data.deviceArray = devicelist.ToArray();
                    var deviceidsnew = "";
                    var devicenew = "";
                    if (devicelist.Count > 0)
                    {
                        int k = 0;
                        foreach (var deviceid in devicelist)
                        {
                            if (k == 0)
                            {
                                deviceidsnew = '"' + deviceid.AMST_AppDownloadedDeviceId + '"';
                                k = 1;
                            }
                            else
                            {
                                deviceidsnew = deviceidsnew + "," + '"' + deviceid.AMST_AppDownloadedDeviceId + '"';
                            }
                        }
                        devicenew = "[" + deviceidsnew + "]";

                        callnotification(devicenew, data.LMSLMEET_Id, data.MI_Id, dto, "Start");
                    }
                }
              
                if(meetingstartedfirst==true)
                {
                    meetingid = data.LMSLMEET_Id;
                    localmiid = data.MI_Id;
                    Start();
                }
                
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public LiveMeetingScheduleDTO endmainmeetingstudent(LiveMeetingScheduleDTO data)
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


                var ress = _db.LMS_Live_Meeting_StudentDMO.Where(e => e.LMSLMEET_Id == data.LMSLMEET_Id && e.AMST_Id == data.Amst_Id).ToList();
                foreach (var item in ress)
                {
                    item.LMSLMEETSTD_LogoutTime = time;
                    item.LMSLMEETSTD_MACAddress = sMacAddress;
                    item.LMSLMEETSTD_IPAddress = myIP1;
                    item.LMSLMEETSTD_UpdatedBy = data.UserId;
                    item.LMSLMEETSTD_UpdatedDate = DateTime.Now;
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
        public LiveMeetingScheduleDTO endmainmeeting(LiveMeetingScheduleDTO data)
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

                if (data.HRML_LeaveType == "M")
                {
                    var ress = _db.LMS_Live_MeetingDMO.Single(e => e.LMSLMEET_Id == data.LMSLMEET_Id);
                    ress.LMSLMEET_EndTime = time;

                    ress.LMSLMEET_MACAddress = sMacAddress;
                    ress.LMSLMEET_IPAddress = myIP1;
                    ress.LMSLMEET_UpdatedBy = data.UserId;
                    ress.LMSLMEET_UpdatedDate = DateTime.Now;
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
                else if (data.HRML_LeaveType == "ST")
                {

                    //if (data.HRME_Id == 0)
                    //{
                    //    data.HRME_Id = _exam.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                    //}

                    var ress = _db.LMS_Live_Meeting_StaffOthersDMO.Single(e => e.LMSLMEET_Id == data.LMSLMEET_Id && e.User_Id == data.UserId);
                    ress.LMSLMEETSTFOTH_LogoutTime = time;
                    ress.LMSLMEETSTFOTH_MACAddress = sMacAddress;
                    ress.LMSLMEETSTFOTH_MACAddress = myIP1;
                    ress.LMSLMEETSTFOTH_UpdatedBy = data.UserId;
                    ress.LMSLMEETSTFOTH_UpdatedDate = DateTime.Now;
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
        //public LiveMeetingScheduleDTO joinmeeting(LiveMeetingScheduleDTO data)
        //{
        //    try
        //    {

        //        if (data.HRME_Id == 0)
        //        {
        //            data.HRME_Id = _exam.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
        //        }


        //        TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        //        DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
        //        var time = indianTime.ToString("hh:mm tt");



        //        //GET THE IP ADDRESS
        //        NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
        //        String sMacAddress = string.Empty;
        //        foreach (NetworkInterface adapter in nics)
        //        {
        //            if (sMacAddress == String.Empty)// only return MAC Address from first card
        //            {
        //                IPInterfaceProperties properties = adapter.GetIPProperties();
        //                sMacAddress = adapter.GetPhysicalAddress().ToString();
        //            }
        //        }
        //        var remoteIpAddress = "";
        //        //string netip = remoteIpAddress.ToString();

        //        string strHostName = "";
        //        strHostName = System.Net.Dns.GetHostName();

        //        IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

        //        IPAddress[] addr = ipEntry.AddressList;

        //        remoteIpAddress = addr[addr.Length - 1].ToString();

        //        string hostName = Dns.GetHostName();
        //        var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
        //        //  string myIP1 = ip_list.ToString();
        //        string myIP1 = addr[addr.Length - 2].ToString();

        //        var ress = _db.LMS_Live_Meeting_StaffOthersDMO.Single(e => e.LMSLMEET_Id == data.LMSLMEET_Id && e.HRME_Id == data.HRME_Id);
        //        ress.LMSLMEETSTFOTH_LoginTime = time;
        //        ress.LMSLMEETSTFOTH_MACAddress = sMacAddress;
        //        ress.LMSLMEETSTFOTH_IPAddress = myIP1;
        //        ress.LMSLMEETSTFOTH_UpdatedBy = data.UserId;
        //        ress.LMSLMEETSTFOTH_CreatedBy = data.UserId;
        //        ress.LMSLMEETSTFOTH_UpdatedDate = DateTime.Now;
        //        ress.LMSLMEETSTFOTH_CreatedDate = DateTime.Now;
        //        _db.Update(ress);
        //        int ss = _db.SaveChanges();
        //        if (ss > 0)
        //        {
        //            data.returnval = true;
        //        }
        //        else
        //        {
        //            data.returnval = true;
        //        }

        //    }



        //    catch (Exception ee)
        //    {
        //        Console.WriteLine(ee.Message);
        //    }
        //    return data;
        //}
        //MEETIN SCHEDULE
        public LiveMeetingScheduleDTO getdatastuacadgrp(LiveMeetingScheduleDTO data)
        {
            try
            {
                var roletyp = _db.MasterRoleType.Where(t => t.IVRMRT_Id == data.RoleId).ToList();
                data.roletype = roletyp.FirstOrDefault().IVRMRT_Role;

                if (data.roletype.Equals("staff", StringComparison.OrdinalIgnoreCase))
                {
                    data.HRME_Id = _exam.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                    data.adminstaffflag = false;

                    //data.meetinglist = (from a in _db.LMS_Live_MeetingDMO
                    //                    from b in _db.HR_Master_Employee_DMO
                    //                    where a.HRME_Id == b.HRME_Id && a.HRME_Id == data.HRME_Id && a.MI_Id == data.MI_Id
                    //                    select a).Distinct().OrderByDescending(w => w.LMSLMEET_PlannedDate).ToArray();

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Live_Meeting_List";
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
                        cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                          SqlDbType.BigInt)
                        {
                            Value = data.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@flag",
                        SqlDbType.VarChar)
                        {
                            Value = "S"
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
                            data.meetinglist = retObject.ToArray();
                        }

                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }

                    data.teacherslist = (from e in _examcontext.Staff_User_Login
                                         from f in _examcontext.Exm_Login_PrivilegeDMO
                                         from i in _examcontext.Exm_Login_Privilege_SubjectsDMO
                                         from g in _examcontext.HR_Master_Employee_DMO
                                         where (e.Emp_Code == g.HRME_Id && data.MI_Id == data.MI_Id && f.Login_Id == e.IVRMSTAUL_Id
                                         && f.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id && f.ELP_Id == i.ELP_Id && f.ELP_ActiveFlg == true
                                         && i.ELPs_ActiveFlg == true && g.HRME_ActiveFlag == true && e.Emp_Code == data.HRME_Id)
                                         select new LiveMeetingScheduleDTO
                                         {
                                             UserId = e.Id,
                                             HRME_Id = g.HRME_Id,
                                             HRME_EmployeeFirstName = ((g.HRME_EmployeeFirstName == null || g.HRME_EmployeeFirstName == "0" ? "" : g.HRME_EmployeeFirstName) + " " + (g.HRME_EmployeeMiddleName == null || g.HRME_EmployeeMiddleName == "0" ? "" : g.HRME_EmployeeMiddleName) + " " + (g.HRME_EmployeeLastName == null || g.HRME_EmployeeLastName == "0" ? "" : g.HRME_EmployeeLastName)).Trim()
                                         }).Distinct().OrderBy(r => r.HRME_EmployeeFirstName).ToArray();

                }
                else
                {
                    data.adminstaffflag = true;
                    //data.meetinglist = (from a in _db.LMS_Live_MeetingDMO
                    //                    from b in _db.HR_Master_Employee_DMO
                    //                    where a.MI_Id == data.MI_Id
                    //                    select a).Distinct().OrderByDescending(w => w.LMSLMEET_PlannedDate).ToArray();
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Live_Meeting_List";
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
                        cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                          SqlDbType.BigInt)
                        {
                            Value = data.HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@flag",
                        SqlDbType.VarChar)
                        {
                            Value = "A"
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
                            data.meetinglist = retObject.ToArray();
                        }

                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                }

                data.stafflist = (from a in _db.Staff_User_Login
                                  from b in _db.HR_Master_Employee_DMO
                                  where a.MI_Id == b.MI_Id && a.Emp_Code == b.HRME_Id && a.IVRMSTAUL_ActiveFlag == 1 && b.HRME_ActiveFlag == true
                                  && a.MI_Id == data.MI_Id && b.HRME_Id != data.HRME_Id && b.HRME_TechNonTeachingFlg == "Teaching"
                                  select new LiveMeetingScheduleDTO
                                  {
                                      UserId = a.Id,
                                      HRME_Id = b.HRME_Id,
                                      IVRMSTAUL_UserName = a.IVRMSTAUL_UserName,
                                      HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "0" ? "" : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "0" ? "" : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "0" ? "" : b.HRME_EmployeeLastName)).Trim(),
                                  }).Distinct().OrderBy(r => r.HRME_EmployeeFirstName).ToArray();

                data.allstafflist = (from e in _examcontext.Staff_User_Login
                                     from f in _examcontext.Exm_Login_PrivilegeDMO
                                     from i in _examcontext.Exm_Login_Privilege_SubjectsDMO
                                     from g in _examcontext.HR_Master_Employee_DMO
                                     where (e.Emp_Code == g.HRME_Id && data.MI_Id == data.MI_Id && f.Login_Id == e.IVRMSTAUL_Id && f.ASMAY_Id == data.ASMAY_Id
                                     && f.MI_Id == data.MI_Id && f.ELP_Id == i.ELP_Id && f.ELP_ActiveFlg == true
                                     && i.ELPs_ActiveFlg == true && g.HRME_ActiveFlag == true)
                                     select new LiveMeetingScheduleDTO
                                     {
                                         UserId = e.Id,
                                         HRME_Id = g.HRME_Id,
                                         HRME_EmployeeFirstName = ((g.HRME_EmployeeFirstName == null || g.HRME_EmployeeFirstName == "0" ? "" : g.HRME_EmployeeFirstName) + " " + (g.HRME_EmployeeMiddleName == null || g.HRME_EmployeeMiddleName == "0" ? "" : g.HRME_EmployeeMiddleName) + " " + (g.HRME_EmployeeLastName == null || g.HRME_EmployeeLastName == "0" ? "" : g.HRME_EmployeeLastName)).Trim()
                                     }).Distinct().OrderBy(r => r.HRME_EmployeeFirstName).ToArray();


                data.academicList = _exam.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public LiveMeetingScheduleDTO getclass(LiveMeetingScheduleDTO data)
        {

            try
            {
                //var roletyp = _db.MasterRoleType.Where(t => t.IVRMRT_Id == data.RoleId).ToList();
                //data.roletype = roletyp.FirstOrDefault().IVRMRT_Role;
                //if (data.roletype.Equals("staff", StringComparison.OrdinalIgnoreCase))
                //{

                var classexmid = (from e in _examcontext.Staff_User_Login
                                  from f in _examcontext.Exm_Login_PrivilegeDMO
                                  from i in _examcontext.Exm_Login_Privilege_SubjectsDMO
                                  from a in _examcontext.AdmissionClass
                                  where (e.Emp_Code == data.HRME_Id && data.MI_Id == data.MI_Id && a.ASMCL_Id == i.ASMCL_Id &&
                                    f.Login_Id == e.IVRMSTAUL_Id && f.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id
                                    && f.ELP_Id == i.ELP_Id && f.ELP_ActiveFlg == true && i.ELPs_ActiveFlg == true)
                                  select new MarksEntry_SDTO
                                  {
                                      ASMCL_Id = i.ASMCL_Id
                                  }).Distinct().Select(t => t.ASMCL_Id).ToArray();

                List<AdmissionClass> clist = new List<AdmissionClass>();
                clist = _examcontext.AdmissionClass.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true && classexmid.Contains(t.ASMCL_Id)).ToList();

                data.classlist = (from b in _db.admissioncls
                                  where (b.MI_Id == data.MI_Id && classexmid.Contains(b.ASMCL_Id) && b.ASMCL_ActiveFlag == true)
                                  select b).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();
                //}
                //else
                //{
                //    data.classlist = (from a in _db.Masterclasscategory
                //                      from b in _db.admissioncls
                //                      where (b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMCL_ActiveFlag == true && a.Is_Active == true)
                //                      select b
                //                      ).Distinct().OrderBy(t => t.ASMCL_Order).ToArray();
                //}
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public LiveMeetingScheduleDTO getsection(LiveMeetingScheduleDTO data)
        {

            try
            {
                List<long> clss_ids = new List<long>();

                //var roletyp = _db.MasterRoleType.Where(t => t.IVRMRT_Id == data.RoleId).ToList();
                //data.roletype = roletyp.FirstOrDefault().IVRMRT_Role;
                //if (data.roletype.Equals("staff", StringComparison.OrdinalIgnoreCase))
                //{
                data.SectionList = (from e in _examcontext.Staff_User_Login
                                    from f in _examcontext.Exm_Login_PrivilegeDMO
                                    from i in _examcontext.Exm_Login_Privilege_SubjectsDMO
                                    from a in _examcontext.AdmissionClass
                                    from c in _examcontext.School_M_Section
                                    where (e.Emp_Code == data.HRME_Id && data.MI_Id == data.MI_Id && a.ASMCL_Id == i.ASMCL_Id && c.ASMS_Id == i.ASMS_Id
                                    && f.Login_Id == e.IVRMSTAUL_Id && f.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id && f.ELP_Id == i.ELP_Id
                                    && f.ELP_ActiveFlg == true && i.ELPs_ActiveFlg == true && i.ASMCL_Id == data.ASMCL_Id)
                                    select c).Distinct().OrderBy(t => t.ASMC_Order).ToArray();

                //}
                //else
                //{
                //    data.SectionList = (from a in _db.Masterclasscategory
                //                        from b in _db.admissioncls
                //                        from c in _db.AdmSchoolMasterClassCatSec
                //                        from d in _db.Section
                //                        where (b.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && a.ASMCC_Id == c.ASMCC_Id
                //                        && c.ASMS_Id == d.ASMS_Id && b.ASMCL_ActiveFlag == true && a.Is_Active == true && a.ASMCL_Id == data.ASMCL_Id
                //                        && a.ASMAY_Id == data.ASMAY_Id && c.ASMCCS_ActiveFlg == true)
                //                        select d).Distinct().OrderBy(t => t.ASMC_Order).ToArray();
                //}
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public LiveMeetingScheduleDTO getsubject(LiveMeetingScheduleDTO data)
        {

            try
            {
                List<long> clss_ids = new List<long>();

                //var roletyp = _db.MasterRoleType.Where(t => t.IVRMRT_Id == data.RoleId).ToList();
                //data.roletype = roletyp.FirstOrDefault().IVRMRT_Role;

                //if (data.roletype.Equals("staff", StringComparison.OrdinalIgnoreCase))
                //{
                data.subjlist = (from e in _examcontext.Staff_User_Login
                                 from f in _examcontext.Exm_Login_PrivilegeDMO
                                 from i in _examcontext.Exm_Login_Privilege_SubjectsDMO
                                 from a in _examcontext.AdmissionClass
                                 from c in _examcontext.School_M_Section
                                 from d in _examcontext.IVRM_School_Master_SubjectsDMO
                                 where (e.Emp_Code == data.HRME_Id && data.MI_Id == data.MI_Id && a.ASMCL_Id == i.ASMCL_Id && c.ASMS_Id == i.ASMS_Id
                                 && i.ISMS_Id == d.ISMS_Id && f.Login_Id == e.IVRMSTAUL_Id && f.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id
                                 && f.ELP_Id == i.ELP_Id && i.ASMS_Id == data.ASMS_Id && f.ELP_ActiveFlg == true && i.ELPs_ActiveFlg == true
                                 && i.ASMCL_Id == data.ASMCL_Id)
                                 select d).Distinct().OrderBy(t => t.ISMS_OrderFlag).ToArray();

                //}
                //else
                //{
                //    data.subjlist = (from a in _examcontext.Exm_Category_ClassDMO
                //                     from b in _examcontext.Exm_Yearly_CategoryDMO
                //                     from c in _examcontext.Exm_Yearly_Category_GroupDMO
                //                     from d in _examcontext.Exm_Yearly_Category_Group_SubjectsDMO
                //                     from e in _examcontext.IVRM_School_Master_SubjectsDMO
                //                     where (a.EMCA_Id == b.EMCA_Id && b.EYC_Id == c.EYC_Id && c.EYCG_Id == d.EYCG_Id && d.ISMS_Id == e.ISMS_Id
                //                     && a.ASMAY_Id == data.ASMAY_Id && a.ASMS_Id == data.ASMS_Id && a.ASMCL_Id == data.ASMCL_Id
                //                     && b.ASMAY_Id == data.ASMAY_Id && a.ECAC_ActiveFlag == true && b.EYC_ActiveFlg == true && c.EYCG_ActiveFlg == true
                //                     && d.EYCGS_ActiveFlg == true)
                //                     select e).Distinct().OrderBy(t => t.ISMS_OrderFlag).ToArray();
                //}
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public LiveMeetingScheduleDTO checkduplicate(LiveMeetingScheduleDTO data)
        {
            try
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Duplicate_Meeting_Check_Emp";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                    SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@HRME_ID",
                        SqlDbType.BigInt)
                    {
                        Value = data.HRME_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@PLANNEDDATE",
                        SqlDbType.DateTime)
                    {
                        Value = data.LMSLMEET_PlannedDate
                    });
                    cmd.Parameters.Add(new SqlParameter("@STARTTIME",
                       SqlDbType.VarChar)
                    {
                        Value = data.LMSLMEET_PlannedStartTime
                    });
                    cmd.Parameters.Add(new SqlParameter("@ENDTIME",
                       SqlDbType.VarChar)
                    {
                        Value = data.LMSLMEET_PlannedEndTime
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
                        data.duplicatemeetingemp = retObject.ToArray();
                        //if(data.duplicatemeetingemp.Length>0)
                        //{
                        //    data.duplicatemeeting = true;
                        //}                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }



                if (data.saveaarylist.Length > 0)
                {
                    foreach (var cls in data.saveaarylist)
                    {
                        using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Duplicate_Meeting_Check_Class";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.BigInt)
                            {
                                Value = data.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                            SqlDbType.BigInt)
                            {
                                Value = data.ASMAY_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@PLANNEDDATE",
                                SqlDbType.DateTime)
                            {
                                Value = data.LMSLMEET_PlannedDate
                            });
                            cmd.Parameters.Add(new SqlParameter("@STARTTIME",
                               SqlDbType.VarChar)
                            {
                                Value = data.LMSLMEET_PlannedStartTime
                            });
                            cmd.Parameters.Add(new SqlParameter("@ENDTIME",
                               SqlDbType.VarChar)
                            {
                                Value = data.LMSLMEET_PlannedEndTime
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMCL_Id",
                               SqlDbType.BigInt)
                            {
                                Value = cls.ASMCL_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@ASMS_Id",
                               SqlDbType.BigInt)
                            {
                                Value = cls.ASMS_Id
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
                                data.duplicatemeetingclass = retObject.ToArray();
                                //if (data.duplicatemeetingclass.Length > 0)
                                //{
                                //    data.duplicatemeeting = true;                                    
                                //}                                
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public LiveMeetingScheduleDTO savedata(LiveMeetingScheduleDTO data)
        {
            try
            {
                string meetingid = "", meetingurl = "", accesstoken = "";
                meetingid = Getmetingid(data);
                var teamcredentials = _db.Institution.Where(c => c.MI_Id == data.MI_Id).ToList();

                if (teamcredentials.FirstOrDefault().MI_VCStudentFlag.Equals("Teams", StringComparison.OrdinalIgnoreCase))
                {
                    //MS Teams Meeting schedule Integration
                    var teamstaffcredentials = (from a in _db.HR_Master_Employee_DMO
                                                from b in _db.Staff_User_Login
                                                where (a.HRME_Id == b.Emp_Code && a.HRME_Id == data.HRME_Id && a.MI_Id == data.MI_Id)
                                                select a).Distinct().ToList();

                    string useraccesstokenurl = teamcredentials.FirstOrDefault().MI_MSTeamsUserAceessTockenURL;
                    useraccesstokenurl = useraccesstokenurl.Replace("TenantID", teamcredentials.FirstOrDefault().MI_MSTeamsTenentId);
                    var client = new RestClient(useraccesstokenurl);

                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

                    request.AddParameter("grant_type", teamcredentials.FirstOrDefault().MI_MSTeamsGrantType);
                    request.AddParameter("client_id", teamcredentials.FirstOrDefault().MI_MSTeamsClientId);
                    request.AddParameter("client_secret", teamcredentials.FirstOrDefault().MI_MSTemasClinetSecretCode);
                    request.AddParameter("scope", teamcredentials.FirstOrDefault().MI_MSTeamsScope);
                    //request.AddParameter("userName", teamcredentials.FirstOrDefault().MI_MSTeamsAdminUsername);
                    //request.AddParameter("password", teamcredentials.FirstOrDefault().MI_MSTeamsAdminPassword);

                    if ((teamstaffcredentials[0].HRME_MSTeamsEmailId != null && teamstaffcredentials[0].HRME_MSTeamsEmailId != "") && (teamstaffcredentials[0].HRME_MSTeamsPassword != null && teamstaffcredentials[0].HRME_MSTeamsPassword != ""))
                    {
                        request.AddParameter("userName", teamstaffcredentials[0].HRME_MSTeamsEmailId);
                        request.AddParameter("password", teamstaffcredentials[0].HRME_MSTeamsPassword);
                    }
                    else
                    {
                        data.returnvalue = "User";
                        return data;
                    }

                    try
                    {
                        IRestResponse response = client.Execute(request);
                        Console.WriteLine(response.Content);
                        JObject joResponse = JObject.Parse(response.Content);
                        accesstoken = (string)joResponse["access_token"];
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        _logger.LogError("Error in " + data.MI_Id + " - " + ex.InnerException);
                    }

                    var client1 = new RestClient(teamcredentials.FirstOrDefault().MI_MSTeamsMeetingScheduleURL);
                    client1.Timeout = -1;
                    var request1 = new RestRequest(Method.POST);
                    request1.AddHeader("Authorization", "Bearer " + accesstoken);
                    request1.AddHeader("Content-Type", "application/json");

                    string addparameter = "{\"startDateTime\":\"startdt\",\"endDateTime\":\"enddt\",\"subject\":\"sbjnme\",\"participants\":{\"organizer\":{\"identity\":{\"user\":{\"id\":\"usrid\"}}}}}";

                    DateTime dt = DateTime.UtcNow;
                    string plandate = data.LMSLMEET_PlannedDate.ToString("yyyy-MM-dd").Substring(0, 10);
                    string enddateTime = data.LMSLMEET_PlannedDate.ToString("yyyy-MM-dd").Substring(0, 10);
                    string[] houmins = data.LMSLMEET_PlannedStartTime.Split(":");
                    string[] houmine = data.LMSLMEET_PlannedEndTime.Split(":");

                    plandate = plandate + "T" + data.LMSLMEET_PlannedStartTime + ":00.0000000-07:00";
                    enddateTime = enddateTime + "T" + data.LMSLMEET_PlannedEndTime + ":00.0000000-07:00";

                    addparameter = addparameter.Replace("startdt", plandate);
                    addparameter = addparameter.Replace("enddt", enddateTime);
                    addparameter = addparameter.Replace("sbjnme", meetingid);
                    addparameter = addparameter.Replace("usrid", teamstaffcredentials[0].HRME_MSTeamsUserId);
                    request1.AddParameter("application/json", addparameter, ParameterType.RequestBody);
                    try
                    {
                        IRestResponse response1 = client1.Execute(request1);
                        Console.WriteLine(response1.Content);
                        JObject joResponse1 = JObject.Parse(response1.Content);
                        meetingurl = (string)joResponse1["joinWebUrl"];
                        if (meetingurl == null || meetingurl == "")
                        {
                            data.returnvalue = "User";
                            return data;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        _logger.LogError("Error in " + data.MI_Id + " - " + ex.InnerException);
                    }
                    //MS Teams Meeting schedule Integration
                }
                if (data.LMSLMEET_Id == 0)
                {
                    if (data.HRME_Id == 0)
                    {
                        data.HRME_Id = _exam.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                    }
                    else
                    {
                        data.staffuserid = _exam.Staff_User_Login.Single(c => c.Emp_Code == data.HRME_Id && c.MI_Id == data.MI_Id).Id;
                    }

                    //string meetingid = Getmetingid(data);

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

                    LMS_Live_MeetingDMO obj = new LMS_Live_MeetingDMO();
                    obj.MI_Id = data.MI_Id;
                    obj.LMSLMEET_MeetingId = meetingid;
                    obj.LMSLMEET_PlannedDate = data.LMSLMEET_PlannedDate;
                    obj.LMSLMEET_PlannedEndTime = data.LMSLMEET_PlannedEndTime;
                    obj.LMSLMEET_PlannedStartTime = data.LMSLMEET_PlannedStartTime;
                    obj.LMSLMEET_MeetingTopic = data.LMSLMEET_MeetingTopic;
                    obj.LMSLMEET_PMACAddress = sMacAddress;
                    obj.LMSLMEET_PIPAddress = myIP1;
                    obj.User_Id = data.UserId;
                    obj.HRME_Id = data.HRME_Id;
                    obj.HRME_Id = data.HRME_Id;
                    obj.LMSLMEET_CreatedBy = data.UserId;
                    obj.LMSLMEET_UpdatedBy = data.UserId;
                    obj.LMSLMEET_Remarks ="";
                    obj.LMSLMEET_Grade = "";
                    obj.LMSLMEET_CanOthersStartFlg = false;
                    obj.LMSLMEET_StartedByUserId = 0;
                    obj.LMSLMEET_MeetingURL = meetingurl;
                    obj.LMSLMEET_CreatedDate = DateTime.Now;
                    obj.LMSLMEET_UpdatedDate = DateTime.Now;
                    obj.LMSLMEET_ActiveFlg = true;
                    _db.Add(obj);

                    if (data.studflag == true)
                    {
                        if (data.saveaarylist.Length > 0)
                        {
                            foreach (var cls in data.saveaarylist)
                            {
                                LMS_Live_Meeting_ClassDMO obj1 = new LMS_Live_Meeting_ClassDMO();
                                obj1.LMSLMEET_Id = obj.LMSLMEET_Id;
                                obj1.ASMAY_Id = data.ASMAY_Id;
                                obj1.ASMCL_Id = cls.ASMCL_Id;
                                obj1.ASMS_Id = cls.ASMS_Id;
                                obj1.ISMS_Id = cls.ISMS_Id;
                                obj1.LMSLMEETCLS_ActiveFlg = true;
                                obj1.LMSLMEETCLS_CreatedDate = DateTime.Now;
                                obj1.LMSLMEETCLS_UpdatedDate = DateTime.Now;
                                obj1.LMSLMEETCLS_CreatedBy = data.UserId;
                                obj1.LMSLMEETCLS_UpdatedBy = data.UserId;
                                _db.Add(obj1);
                            }
                        }
                    }
                    if (data.stafflag == true)
                    {
                        if (data.stfids.Length > 0)
                        {
                            foreach (var item in data.stfids)
                            {
                                LMS_Live_Meeting_StaffOthersDMO sobj = new LMS_Live_Meeting_StaffOthersDMO();
                                sobj.LMSLMEET_Id = obj.LMSLMEET_Id;
                                sobj.User_Id = item.UserId;
                                sobj.HRME_Id = item.HRME_Id;
                                sobj.LMSLMEETSTFOTH_CreatedBy = data.UserId;
                                sobj.LMSLMEETSTFOTH_UpdatedBy = data.UserId;
                                sobj.LMSLMEETSTFOTH_CreatedDate = DateTime.Now;
                                sobj.LMSLMEETSTFOTH_UpdatedDate = DateTime.Now;
                                sobj.LMSLMEETSTFOTH_ActiveFlg = true;
                                _db.Add(sobj);
                            }
                        }
                    }
                    if (data.managerflg == true)
                    {
                        var users = (from c in _db.ApplicationUser
                                     from f in _db.appUserRole
                                     from d in _db.MasterRoleType
                                     from a in _db.UserRoleWithInstituteDMO
                                     where (a.Id == f.UserId && f.RoleTypeId == d.IVRMRT_Id && a.Id == c.Id && a.Id != data.staffuserid && c.Id == f.UserId && a.MI_Id == data.MI_Id && d.IVRMRT_Role.Equals("Manager", StringComparison.OrdinalIgnoreCase)
                                  )
                                     select new StaffLoginDTO
                                     {
                                         rolenamess = d.IVRMRT_Role,
                                         User_Name = c.UserName,
                                         Id = c.Id
                                     }
                                   ).Distinct().ToList();

                        if (users.Count > 0)
                        {
                            if (users.Count > 0)
                            {
                                foreach (var item in users)
                                {
                                    LMS_Live_Meeting_StaffOthersDMO sobj = new LMS_Live_Meeting_StaffOthersDMO();
                                    sobj.LMSLMEET_Id = obj.LMSLMEET_Id;
                                    sobj.User_Id = item.Id;
                                    sobj.HRME_Id = 0;
                                    sobj.LMSLMEETSTFOTH_CreatedBy = data.UserId;
                                    sobj.LMSLMEETSTFOTH_UpdatedBy = data.UserId;
                                    sobj.LMSLMEETSTFOTH_CreatedDate = DateTime.Now;
                                    sobj.LMSLMEETSTFOTH_UpdatedDate = DateTime.Now;
                                    sobj.LMSLMEETSTFOTH_ActiveFlg = true;
                                    _db.Add(sobj);
                                }
                            }
                        }

                    }
                    if (data.hodflg == true)
                    {
                        if (data.saveaarylist.Length > 0)
                        {
                            foreach (var cls in data.saveaarylist)
                            {
                                var users = (from a in _db.HOD_DMO
                                             from b in _db.IVRM_HOD_Class_DMO
                                             from c in _db.Staff_User_Login
                                             where (a.IHOD_Id == b.IHOD_Id && a.HRME_Id == c.Emp_Code && a.MI_Id == data.MI_Id && a.IHOD_Flg == "HOD" && a.IHOD_ActiveFlag == true && b.IHODC_ActiveFlag == true && b.ASMCL_Id == cls.ASMCL_Id && c.Id != data.staffuserid
                                          )
                                             select new StaffLoginDTO
                                             {
                                                 HRME_Id = a.HRME_Id,
                                                 Id = c.Id
                                             }
                                  ).Distinct().ToList();

                                if (users.Count > 0)
                                {
                                    foreach (var item in users)
                                    {
                                        LMS_Live_Meeting_StaffOthersDMO sobj = new LMS_Live_Meeting_StaffOthersDMO();
                                        sobj.LMSLMEET_Id = obj.LMSLMEET_Id;
                                        sobj.User_Id = item.Id;
                                        sobj.HRME_Id = item.HRME_Id;
                                        sobj.LMSLMEETSTFOTH_CreatedBy = data.UserId;
                                        sobj.LMSLMEETSTFOTH_UpdatedBy = data.UserId;
                                        sobj.LMSLMEETSTFOTH_CreatedDate = DateTime.Now;
                                        sobj.LMSLMEETSTFOTH_UpdatedDate = DateTime.Now;
                                        sobj.LMSLMEETSTFOTH_ActiveFlg = true;
                                        _db.Add(sobj);
                                    }
                                }
                            }
                        }
                    }
                    if (data.principalflg == true)
                    {
                        var users = (from c in _db.ApplicationUser
                                     from f in _db.appUserRole
                                     from d in _db.MasterRoleType
                                     from a in _db.UserRoleWithInstituteDMO
                                     where (a.Id == f.UserId && f.RoleTypeId == d.IVRMRT_Id && a.Id == c.Id && c.Id == f.UserId && a.Id != data.staffuserid && a.MI_Id == data.MI_Id && d.IVRMRT_Role.Equals("Principal", StringComparison.OrdinalIgnoreCase)
                                  )
                                     select new StaffLoginDTO
                                     {
                                         rolenamess = d.IVRMRT_Role,
                                         User_Name = c.UserName,
                                         Id = c.Id
                                     }
                                  ).Distinct().ToList();

                        if (users.Count > 0)
                        {
                            if (users.Count > 0)
                            {
                                foreach (var item in users)
                                {
                                    LMS_Live_Meeting_StaffOthersDMO sobj = new LMS_Live_Meeting_StaffOthersDMO();
                                    sobj.LMSLMEET_Id = obj.LMSLMEET_Id;
                                    sobj.User_Id = item.Id;
                                    sobj.HRME_Id = 0;
                                    sobj.LMSLMEETSTFOTH_CreatedBy = data.UserId;
                                    sobj.LMSLMEETSTFOTH_UpdatedBy = data.UserId;
                                    sobj.LMSLMEETSTFOTH_CreatedDate = DateTime.Now;
                                    sobj.LMSLMEETSTFOTH_UpdatedDate = DateTime.Now;
                                    sobj.LMSLMEETSTFOTH_ActiveFlg = true;
                                    _db.Add(sobj);
                                }
                            }
                        }
                    }
                    int res = _db.SaveChanges();
                    if (res > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = true;
                    }

                    if (data.returnval == true)
                    {
                        var devicelist = (from a in _db.HR_Master_Employee_DMO
                                          from b in _db.Staff_User_Login
                                          from c in _db.LMS_Live_Meeting_StaffOthersDMO                                          
                                          where (a.MI_Id == data.MI_Id && a.HRME_Id == b.Emp_Code && b.Id == c.User_Id && c.LMSLMEET_Id == obj.LMSLMEET_Id && a.HRME_AppDownloadedDeviceId != null && a.HRME_AppDownloadedDeviceId != "")
                                          select new IVRM_Homework_DTO
                                          {
                                              AMST_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId
                                          }).Distinct().ToList();
                        IVRM_Homework_DTO dto = new IVRM_Homework_DTO();
                        dto.devicelist12 = devicelist;
                        data.deviceArray = devicelist.ToArray();
                        var deviceidsnew = "";
                        var devicenew = "";
                        if (devicelist.Count > 0)
                        {
                            int k = 0;
                            foreach (var deviceid in devicelist)
                            {
                                if (k == 0)
                                {
                                    deviceidsnew = '"' + deviceid.AMST_AppDownloadedDeviceId + '"';
                                    k = 1;
                                }
                                else
                                {
                                    deviceidsnew = deviceidsnew + "," + '"' + deviceid.AMST_AppDownloadedDeviceId + '"';
                                }
                            }
                            devicenew = "[" + deviceidsnew + "]";

                            callnotification(devicenew, obj.LMSLMEET_Id, data.MI_Id, dto, "Schedule");
                        }
                    }

                    if (data.returnval == true)
                    {
                        var devicelist = (from a in _db.HR_Master_Employee_DMO
                                          from b in _db.Staff_User_Login
                                          from c in _db.LMS_Live_Meeting_StaffOthersDMO                                          
                                          where (a.MI_Id == data.MI_Id && a.HRME_Id == b.Emp_Code && b.Id == c.User_Id && c.LMSLMEET_Id == obj.LMSLMEET_Id)
                                          select new IVRM_Homework_DTO
                                          {
                                              HRME_Id = a.HRME_Id
                                          }).Distinct().ToList();
                        if (devicelist.Count > 0)
                        {
                            foreach (var deviceid in devicelist)
                            {
                                var Employee = (from a in _db.HR_Master_Employee_DMO
                                                from b in _db.Multiple_Email_DMO
                                                where a.HRME_Id == b.HRME_Id && a.HRME_Id == deviceid.HRME_Id && b.HRME_Id == deviceid.HRME_Id
                                                select new ISM_PlannerReportsDTO
                                                {
                                                    HRMEM_EmailId = b.HRMEM_EmailId 
                                                }).ToList();
                                if(Employee.Count()>0)
                                {
                                    Email Email = new Email(_db);
                                    Email.sendmail(data.MI_Id, Employee.FirstOrDefault().HRMEM_EmailId, "ONLINE_CLASS_MEETING_STAFF", obj.LMSLMEET_Id);
                                }

                                var Employeemobile = (from a in _db.HR_Master_Employee_DMO
                                                from b in _db.Multiple_Mobile_DMO
                                                where a.HRME_Id == b.HRME_Id && a.HRME_Id == deviceid.HRME_Id && b.HRME_Id == deviceid.HRME_Id
                                                select new ISM_PlannerReportsDTO
                                                {
                                                    HRMEMNO_MobileNo = b.HRMEMNO_MobileNo
                                                }).ToList();
                                if (Employeemobile.Count() > 0)
                                {
                                    SMS sms = new SMS(_db);
                                    sms.sendSms(data.MI_Id, Employeemobile.FirstOrDefault().HRMEMNO_MobileNo, "ONLINE_CLASS_MEETING_STAFF", obj.LMSLMEET_Id);
                                }
                            }
                        }      
                    }
                }
                else
                {
                    if (data.HRME_Id == 0)
                    {
                        data.HRME_Id = _exam.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                    }
                    else
                    {
                        data.staffuserid = _exam.Staff_User_Login.Single(c => c.Emp_Code == data.HRME_Id && c.MI_Id == data.MI_Id).Id;
                    }
                    //string meetingid = Getmetingid(data);
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
                    ress.LMSLMEET_PlannedDate = data.LMSLMEET_PlannedDate;
                    ress.LMSLMEET_PlannedEndTime = data.LMSLMEET_PlannedEndTime;
                    ress.LMSLMEET_PlannedStartTime = data.LMSLMEET_PlannedStartTime;
                    ress.LMSLMEET_MeetingTopic = data.LMSLMEET_MeetingTopic;
                    ress.LMSLMEET_PMACAddress = sMacAddress;
                    ress.LMSLMEET_PIPAddress = myIP1;
                    ress.User_Id = data.UserId;
                    ress.HRME_Id = data.HRME_Id;
                    ress.LMSLMEET_ActiveFlg = true;
                    ress.LMSLMEET_UpdatedBy = data.UserId;
                    ress.LMSLMEET_UpdatedDate = DateTime.Now;
                    _db.Update(ress);

                    var remove = _db.LMS_Live_Meeting_ClassDMO.Where(e => e.LMSLMEET_Id == data.LMSLMEET_Id).ToList();
                    if (remove.Count > 0)
                    {
                        foreach (var item in remove)
                        {
                            _db.Remove(item);
                        }
                    }
                    var removestf = _db.LMS_Live_Meeting_StaffOthersDMO.Where(e => e.LMSLMEET_Id == data.LMSLMEET_Id).ToList();
                    if (removestf.Count > 0)
                    {
                        foreach (var item in removestf)
                        {
                            _db.Remove(item);
                        }
                    }
                    if (data.studflag == true)
                    {
                        if (data.saveaarylist.Length > 0)
                        {
                            foreach (var cls in data.saveaarylist)
                            {
                                LMS_Live_Meeting_ClassDMO obj1 = new LMS_Live_Meeting_ClassDMO();
                                obj1.LMSLMEET_Id = data.LMSLMEET_Id;
                                obj1.ASMAY_Id = data.ASMAY_Id;
                                obj1.ASMCL_Id = cls.ASMCL_Id;
                                obj1.ASMS_Id = cls.ASMS_Id;
                                obj1.ISMS_Id = cls.ISMS_Id;
                                obj1.LMSLMEETCLS_ActiveFlg = true;
                                obj1.LMSLMEETCLS_CreatedDate = DateTime.Now;
                                obj1.LMSLMEETCLS_UpdatedDate = DateTime.Now;
                                obj1.LMSLMEETCLS_CreatedBy = data.UserId;
                                obj1.LMSLMEETCLS_UpdatedBy = data.UserId;
                                _db.Add(obj1);
                            }
                        }
                    }
                    if (data.stafflag == true)
                    {
                        if (data.stfids.Length > 0)
                        {
                            foreach (var item in data.stfids)
                            {
                                LMS_Live_Meeting_StaffOthersDMO sobj = new LMS_Live_Meeting_StaffOthersDMO();
                                sobj.LMSLMEET_Id = data.LMSLMEET_Id;
                                sobj.User_Id = item.UserId;
                                sobj.HRME_Id = item.HRME_Id;
                                sobj.LMSLMEETSTFOTH_CreatedBy = data.UserId;
                                sobj.LMSLMEETSTFOTH_UpdatedBy = data.UserId;
                                sobj.LMSLMEETSTFOTH_CreatedDate = DateTime.Now;
                                sobj.LMSLMEETSTFOTH_UpdatedDate = DateTime.Now;
                                sobj.LMSLMEETSTFOTH_ActiveFlg = true;
                                _db.Add(sobj);
                            }
                        }
                    }
                    if (data.managerflg == true)
                    {
                        var users = (from c in _db.ApplicationUser
                                     from f in _db.appUserRole
                                     from d in _db.MasterRoleType
                                     from a in _db.UserRoleWithInstituteDMO
                                     where (a.Id == f.UserId && f.RoleTypeId == d.IVRMRT_Id && a.Id == c.Id && a.Id != data.staffuserid && c.Id == f.UserId && a.MI_Id == data.MI_Id && d.IVRMRT_Role.Equals("Manager", StringComparison.OrdinalIgnoreCase)
                                  )
                                     select new StaffLoginDTO
                                     {
                                         rolenamess = d.IVRMRT_Role,
                                         User_Name = c.UserName,
                                         Id = c.Id
                                     }
                                   ).Distinct().ToList();

                        if (users.Count > 0)
                        {
                            if (users.Count > 0)
                            {
                                foreach (var item in users)
                                {
                                    LMS_Live_Meeting_StaffOthersDMO sobj = new LMS_Live_Meeting_StaffOthersDMO();
                                    sobj.LMSLMEET_Id = data.LMSLMEET_Id;
                                    sobj.User_Id = item.Id;
                                    sobj.HRME_Id = 0;
                                    sobj.LMSLMEETSTFOTH_CreatedBy = data.UserId;
                                    sobj.LMSLMEETSTFOTH_UpdatedBy = data.UserId;
                                    sobj.LMSLMEETSTFOTH_CreatedDate = DateTime.Now;
                                    sobj.LMSLMEETSTFOTH_UpdatedDate = DateTime.Now;
                                    sobj.LMSLMEETSTFOTH_ActiveFlg = true;
                                    _db.Add(sobj);
                                }
                            }
                        }

                    }
                    if (data.hodflg == true)
                    {
                        if (data.saveaarylist.Length > 0)
                        {
                            foreach (var cls in data.saveaarylist)
                            {
                                var users = (from a in _db.HOD_DMO
                                             from b in _db.IVRM_HOD_Class_DMO
                                             from c in _db.Staff_User_Login
                                             where (a.IHOD_Id == b.IHOD_Id && a.HRME_Id == c.Emp_Code && a.MI_Id == data.MI_Id && a.IHOD_Flg == "HOD" && a.IHOD_ActiveFlag == true && b.IHODC_ActiveFlag == true && b.ASMCL_Id == cls.ASMCL_Id && c.Id != data.staffuserid
                                          )
                                             select new StaffLoginDTO
                                             {
                                                 HRME_Id = a.HRME_Id,
                                                 Id = c.Id
                                             }
                                  ).Distinct().ToList();

                                if (users.Count > 0)
                                {
                                    foreach (var item in users)
                                    {
                                        LMS_Live_Meeting_StaffOthersDMO sobj = new LMS_Live_Meeting_StaffOthersDMO();
                                        sobj.LMSLMEET_Id = data.LMSLMEET_Id;
                                        sobj.User_Id = item.Id;
                                        sobj.HRME_Id = item.HRME_Id;
                                        sobj.LMSLMEETSTFOTH_CreatedBy = data.UserId;
                                        sobj.LMSLMEETSTFOTH_UpdatedBy = data.UserId;
                                        sobj.LMSLMEETSTFOTH_CreatedDate = DateTime.Now;
                                        sobj.LMSLMEETSTFOTH_UpdatedDate = DateTime.Now;
                                        sobj.LMSLMEETSTFOTH_ActiveFlg = true;
                                        _db.Add(sobj);
                                    }
                                }
                            }
                        }
                    }
                    if (data.principalflg == true)
                    {
                        var users = (from c in _db.ApplicationUser
                                     from f in _db.appUserRole
                                     from d in _db.MasterRoleType
                                     from a in _db.UserRoleWithInstituteDMO
                                     where (a.Id == f.UserId && f.RoleTypeId == d.IVRMRT_Id && a.Id == c.Id && c.Id == f.UserId && a.Id != data.staffuserid && a.MI_Id == data.MI_Id && d.IVRMRT_Role.Equals("Principal", StringComparison.OrdinalIgnoreCase)
                                  )
                                     select new StaffLoginDTO
                                     {
                                         rolenamess = d.IVRMRT_Role,
                                         User_Name = c.UserName,
                                         Id = c.Id
                                     }
                                  ).Distinct().ToList();

                        if (users.Count > 0)
                        {
                            if (users.Count > 0)
                            {
                                foreach (var item in users)
                                {
                                    LMS_Live_Meeting_StaffOthersDMO sobj = new LMS_Live_Meeting_StaffOthersDMO();
                                    sobj.LMSLMEET_Id = data.LMSLMEET_Id;
                                    sobj.User_Id = item.Id;
                                    sobj.HRME_Id = 0;
                                    sobj.LMSLMEETSTFOTH_CreatedBy = data.UserId;
                                    sobj.LMSLMEETSTFOTH_UpdatedBy = data.UserId;
                                    sobj.LMSLMEETSTFOTH_CreatedDate = DateTime.Now;
                                    sobj.LMSLMEETSTFOTH_UpdatedDate = DateTime.Now;
                                    sobj.LMSLMEETSTFOTH_ActiveFlg = true;
                                    _db.Add(sobj);
                                }
                            }
                        }
                    }
                    int res = _db.SaveChanges();
                    if (res > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = true;
                    }
                    if (data.returnval == true)
                    {
                       var devicelist = (from a in _db.HR_Master_Employee_DMO
                                          from b in _db.Staff_User_Login
                                          from c in _db.LMS_Live_Meeting_StaffOthersDMO                                          
                                          where (a.MI_Id == data.MI_Id && a.HRME_Id == b.Emp_Code && b.Id == c.User_Id && c.LMSLMEET_Id == data.LMSLMEET_Id && a.HRME_AppDownloadedDeviceId != null && a.HRME_AppDownloadedDeviceId != "")
                                          select new IVRM_Homework_DTO
                                          {
                                              AMST_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId
                                          }).Distinct().ToList();


                        IVRM_Homework_DTO dto = new IVRM_Homework_DTO();
                        dto.devicelist12 = devicelist;
                        data.deviceArray = devicelist.ToArray();
                        var deviceidsnew = "";
                        var devicenew = "";
                        if (devicelist.Count > 0)
                        {
                            int k = 0;
                            foreach (var deviceid in devicelist)
                            {
                                if (k == 0)
                                {
                                    deviceidsnew = '"' + deviceid.AMST_AppDownloadedDeviceId + '"';
                                    k = 1;
                                }
                                else
                                {
                                    deviceidsnew = deviceidsnew + "," + '"' + deviceid.AMST_AppDownloadedDeviceId + '"';
                                }
                            }
                            devicenew = "[" + deviceidsnew + "]";

                            callnotification(devicenew, data.LMSLMEET_Id, data.MI_Id, dto, "ReSchedule");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }

        public string callnotification(string devicenew, long LMSLMEET_Id, long mi_id, IVRM_Homework_DTO dto, string flg)
        {
            try
            {
                string notificationmsg = "";
                string subject = "";
                var key = _db.MobileApplAuthenticationDMO.Single(a => a.MI_Id == mi_id).MAAN_AuthenticationKey;

                var ress = _db.LMS_Live_MeetingDMO.Where(e => e.LMSLMEET_Id == LMSLMEET_Id).ToList();

                var devicelist = (from a in _db.HR_Master_Employee_DMO
                                  from b in _db.LMS_Live_MeetingDMO
                                  where (a.MI_Id == mi_id && a.HRME_Id == b.HRME_Id && b.LMSLMEET_Id == LMSLMEET_Id)
                                  select new LiveMeetingScheduleDTO
                                  {
                                      HRME_EmployeeFirstName = ((a.HRME_EmployeeFirstName == null || a.HRME_EmployeeFirstName == "0" ? "" : a.HRME_EmployeeFirstName) + " " + (a.HRME_EmployeeMiddleName == null || a.HRME_EmployeeMiddleName == "0" ? "" : a.HRME_EmployeeMiddleName) + " " + (a.HRME_EmployeeLastName == null || a.HRME_EmployeeLastName == "0" ? "" : a.HRME_EmployeeLastName)).Trim(),
                                      LMSLMEET_MeetingTopic = b.LMSLMEET_MeetingTopic,
                                      PlannedDate = b.LMSLMEET_PlannedDate.ToString("dd-MM-yyyy"),
                                      LMSLMEET_PlannedStartTime = b.LMSLMEET_PlannedStartTime,
                                      LMSLMEET_PlannedEndTime = b.LMSLMEET_PlannedEndTime,
                                      LMSLMEET_StartedTime = b.LMSLMEET_StartedTime

                                  }).Distinct().ToList();
                if (flg == "Schedule")
                {
                    subject = "Meeting Scheduled :";
                    notificationmsg = "Meeting is scheduled by " + devicelist.FirstOrDefault().HRME_EmployeeFirstName + " on " + devicelist.FirstOrDefault().PlannedDate + " at " + devicelist.FirstOrDefault().LMSLMEET_PlannedStartTime + "-" + devicelist.FirstOrDefault().LMSLMEET_PlannedEndTime + ". Topic: " + devicelist.FirstOrDefault().LMSLMEET_MeetingTopic;
                }
                else if (flg == "Start")
                {
                    subject = "Meeting Started : ";
                    notificationmsg = "Meeting is started by " + devicelist.FirstOrDefault().HRME_EmployeeFirstName + " for topic " + devicelist.FirstOrDefault().LMSLMEET_MeetingTopic + " at " + devicelist.FirstOrDefault().LMSLMEET_StartedTime + " so kindly join meeting.";
                }
                else if (flg == "ReSchedule")
                {
                    subject = "Meeting Re-Scheduled :";
                    notificationmsg = "Meeting is Re-Scheduled by " + devicelist.FirstOrDefault().HRME_EmployeeFirstName + " on " + devicelist.FirstOrDefault().PlannedDate + " at " + devicelist.FirstOrDefault().LMSLMEET_PlannedStartTime + "-" + devicelist.FirstOrDefault().LMSLMEET_PlannedEndTime + ". Topic: " + devicelist.FirstOrDefault().LMSLMEET_MeetingTopic;
                }


                string url = "";
                url = "https://fcm.googleapis.com/fcm/send";

                List<string> notificationparams = new List<string>();
                string daata = "";

                string sound = "default";
                string notId = "2";


                daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
              "" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + subject + '"' + " ,  " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "body" + '"' + ":" + '"' + notificationmsg + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";

                notificationparams.Add(daata.ToString());

                // var mycontent = JsonConvert.SerializeObject(notificationparams);
                var mycontent = notificationparams[0];
                string postdata = mycontent.ToString();
                HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(url);
                connection.ContentType = "application/json";
                connection.MediaType = "application/json";
                connection.Accept = "application/json";

                connection.Method = "post";
                connection.Headers["authorization"] = "key=" + key;

                using (StreamWriter requestwriter = new StreamWriter(connection.GetRequestStream()))
                {
                    requestwriter.Write(postdata);
                }
                string responsedata = string.Empty;

                using (StreamReader responsereader = new StreamReader(connection.GetResponse().GetResponseStream()))
                {
                    responsedata = responsereader.ReadToEnd();
                    JObject joresponse1 = JObject.Parse(responsedata);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";

        }

        public  string callnotificationstudents(string devicenew, long LMSLMEET_Id,long miid,string topic,string empname,string time)
        {
            try
            {
                string notificationmsg = "";
                string subject = "";
                var key = _db.MobileApplAuthenticationDMO.Single(a => a.MI_Id == miid).MAAN_AuthenticationKey;

                var ress = _db.LMS_Live_MeetingDMO.Where(e => e.LMSLMEET_Id == LMSLMEET_Id).ToList();              

                subject = "Online Class Started : ";
                notificationmsg = "Meeting is started by " + empname + " for topic " + topic + " at " + time + " so kindly join meeting.";               


                string url = "";
                url = "https://fcm.googleapis.com/fcm/send";

                List<string> notificationparams = new List<string>();
                string daata = "";

                string sound = "default";
                string notId = "2";


                daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
              "" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + subject + '"' + " ,  " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "body" + '"' + ":" + '"' + notificationmsg + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";

                notificationparams.Add(daata.ToString());

                // var mycontent = JsonConvert.SerializeObject(notificationparams);
                var mycontent = notificationparams[0];
                string postdata = mycontent.ToString();
                HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(url);
                connection.ContentType = "application/json";
                connection.MediaType = "application/json";
                connection.Accept = "application/json";

                connection.Method = "post";
                connection.Headers["authorization"] = "key=" + key;

                using (StreamWriter requestwriter = new StreamWriter(connection.GetRequestStream()))
                {
                    requestwriter.Write(postdata);
                }
                string responsedata = string.Empty;

                using (StreamReader responsereader = new StreamReader(connection.GetResponse().GetResponseStream()))
                {
                    responsedata = responsereader.ReadToEnd();
                    JObject joresponse1 = JObject.Parse(responsedata);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";

        }
        public LiveMeetingScheduleDTO editdata(LiveMeetingScheduleDTO data)
        {
            try
            {
                data.editlist = _db.LMS_Live_MeetingDMO.Where(w => w.LMSLMEET_Id == data.LMSLMEET_Id).ToArray();
                var editlist = _db.LMS_Live_MeetingDMO.Where(w => w.LMSLMEET_Id == data.LMSLMEET_Id).ToArray();

                var details = _db.LMS_Live_Meeting_ClassDMO.Where(w => w.LMSLMEET_Id == data.LMSLMEET_Id).ToList();

                data.Emp_punchDetails = details.ToArray();

                if (details.Count > 0)
                {
                    data.studflag = true;
                    data.ASMAY_Id = details[0].ASMAY_Id;
                    data.HRME_Id = editlist[0].HRME_Id;
                    data.classlist = getclass(data).classlist;

                    //data.classlist = (from a in _db.School_Adm_Y_StudentDMO
                    //                  from b in _db.admissioncls
                    //                  where (b.MI_Id == data.MI_Id && a.ASMAY_Id == details[0].ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && b.ASMCL_ActiveFlag == true && a.AMAY_ActiveFlag == 1)
                    //                  select new LiveMeetingScheduleDTO
                    //                  {
                    //                      ASMCL_ClassName = b.ASMCL_ClassName,
                    //                      ASMCL_Id = b.ASMCL_Id,
                    //                      ASMCL_ClassCode = b.ASMCL_ClassCode
                    //                  }).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();

                    //data.SectionList = (from a in _db.School_Adm_Y_StudentDMO
                    //                    from b in _db.admissioncls
                    //                    from d in _db.Section
                    //                    from e in _db.LMS_Live_Meeting_ClassDMO
                    //                    where (a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && a.ASMAY_Id == details[0].ASMAY_Id && b.ASMCL_Id == a.ASMCL_Id && d.ASMS_Id == a.ASMS_Id && e.ASMCL_Id == b.ASMCL_Id && e.LMSLMEET_Id == data.LMSLMEET_Id)
                    //                    select new LiveMeetingScheduleDTO
                    //                    {
                    //                        ASMC_SectionName = d.ASMC_SectionName,
                    //                        ASMC_SectionCode = d.ASMC_SectionCode,
                    //                        ASMS_Id = d.ASMS_Id
                    //                    }).Distinct().OrderBy(t => t.ASMS_Id).ToArray();

                }
                else
                {
                    data.studflag = false;
                }

                var stfdetails = _db.LMS_Live_Meeting_StaffOthersDMO.Where(w => w.LMSLMEET_Id == data.LMSLMEET_Id).ToList();

                data.empdetails = stfdetails.ToArray();

                if (stfdetails.Count > 0)
                {
                    data.stafflag = true;
                }
                else
                {
                    data.stafflag = false;
                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "EDIT_Live_meeting_data";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@meetingid",
                      SqlDbType.BigInt)
                    {
                        Value = data.LMSLMEET_Id
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
                        data.editedmeetinglist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return data;
        }
        public LiveMeetingScheduleDTO deactive(LiveMeetingScheduleDTO data)
        {

            try
            {
                var activelist = _db.LMS_Live_MeetingDMO.Single(w => w.LMSLMEET_Id == data.LMSLMEET_Id);
                var classlist = _db.LMS_Live_Meeting_ClassDMO.Where(w => w.LMSLMEET_Id == data.LMSLMEET_Id).ToList();
                var stafflist = _db.LMS_Live_Meeting_StaffOthersDMO.Where(w => w.LMSLMEET_Id == data.LMSLMEET_Id).ToList();

                if (activelist.LMSLMEET_ActiveFlg == true)
                {
                    activelist.LMSLMEET_ActiveFlg = false;


                    if (classlist.Count > 0)
                    {
                        foreach (var item in classlist)
                        {
                            item.LMSLMEETCLS_ActiveFlg = false;
                            item.LMSLMEETCLS_UpdatedBy = data.UserId;
                            item.LMSLMEETCLS_UpdatedDate = DateTime.Now;

                            _db.Update(item);
                        }
                    }
                    if (stafflist.Count > 0)
                    {
                        foreach (var item1 in stafflist)
                        {
                            item1.LMSLMEETSTFOTH_ActiveFlg = false;
                            item1.LMSLMEETSTFOTH_UpdatedBy = data.UserId;
                            item1.LMSLMEETSTFOTH_UpdatedDate = DateTime.Now;

                            _db.Update(item1);
                        }
                    }

                }
                else
                {
                    activelist.LMSLMEET_ActiveFlg = true;
                    if (classlist.Count > 0)
                    {
                        foreach (var item in classlist)
                        {
                            item.LMSLMEETCLS_ActiveFlg = true;
                            item.LMSLMEETCLS_UpdatedBy = data.UserId;
                            item.LMSLMEETCLS_UpdatedDate = DateTime.Now;
                            _db.Update(item);
                        }
                    }


                    if (stafflist.Count > 0)
                    {
                        foreach (var item1 in stafflist)
                        {
                            item1.LMSLMEETSTFOTH_ActiveFlg = true;
                            item1.LMSLMEETSTFOTH_UpdatedBy = data.UserId;
                            item1.LMSLMEETSTFOTH_UpdatedDate = DateTime.Now;

                            _db.Update(item1);
                        }
                    }
                }

                activelist.LMSLMEET_UpdatedBy = data.UserId;
                activelist.LMSLMEET_UpdatedDate = DateTime.Now;

                _db.Update(activelist);

                int res = _db.SaveChanges();
                if (res > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = true;
                }


            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }
        public String Getmetingid(LiveMeetingScheduleDTO data)
        {
            string meetingid = "";
            try
            {

                var MeetingId = "";

                if (data.HRME_Id == 0)
                {
                    data.HRME_Id = _exam.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                }
                var EmpCode = _exam.HR_Master_Employee_DMO.Single(e => e.HRME_Id == data.HRME_Id && e.MI_Id == data.MI_Id).HRME_EmployeeCode;
                if (EmpCode != null && EmpCode != "")
                {
                    MeetingId = EmpCode;
                }
                var date = String.Format("{0:m}", data.LMSLMEET_PlannedDate);

                if (data.selectedClasslist.Length > 0)
                {
                    var cnt = 0;
                    foreach (var item in data.selectedClasslist)
                    {
                        if (item.ASMCL_ClassCode != null && item.ASMCL_ClassCode != "")
                        {

                            if (cnt == 0)
                            {
                                if (MeetingId != "")
                                {
                                    MeetingId = MeetingId + "-" + item.ASMCL_ClassCode;
                                }
                                else
                                {
                                    MeetingId = item.ASMCL_ClassCode;
                                }

                                cnt += 1;
                            }


                        }
                    }



                }

                if (data.subids.Length > 0)
                {
                    var cnt = 0;
                    foreach (var item in data.subids)
                    {
                        if (item.ISMS_SubjectCode != null && item.ISMS_SubjectCode != "")
                        {

                            if (cnt == 0)
                            {
                                if (MeetingId != "")
                                {
                                    MeetingId = MeetingId + "-" + item.ISMS_SubjectCode;
                                }
                                else
                                {
                                    MeetingId = item.ISMS_SubjectCode;
                                }

                                cnt += 1;
                            }


                        }
                    }

                }

                if (date != null && date != "")
                {
                    if (MeetingId != "")
                    {
                        MeetingId = MeetingId + "-" + date;
                    }
                    else
                    {
                        MeetingId = date;
                    }
                }

                var Meetinglist = _db.LMS_Live_MeetingDMO.OrderByDescending(e => e.LMSLMEET_Id).Take(1).ToList();
                if (Meetinglist.Count > 0)
                {
                    var id = Meetinglist[0].LMSLMEET_Id + 1;
                    if (MeetingId != "")
                    {

                        MeetingId = MeetingId + "-" + id;
                    }
                    else
                    {
                        MeetingId = id.ToString();
                    }
                }
                else
                {
                    if (MeetingId != "")
                    {
                        MeetingId = MeetingId + "-" + "1";
                    }
                    else
                    {
                        MeetingId = "1";
                    }
                }


                meetingid = MeetingId.Replace(" ", String.Empty);

            }
            catch (Exception ex)
            {

                throw;
            }

            return meetingid;
        }
        //REPORT METHODS
        public LiveMeetingScheduleDTO getschrptdetails(LiveMeetingScheduleDTO data)
        {
            try
            {
                if (data.RoleId > 0)
                {
                    var roletyp = _db.MasterRoleType.Where(t => t.IVRMRT_Id == data.RoleId).ToList();
                    data.roletype = roletyp.FirstOrDefault().IVRMRT_Role;
                    if (data.roletype.Equals("staff", StringComparison.OrdinalIgnoreCase))
                    {


                        if (data.HRME_Id == 0)
                        {
                            data.HRME_Id = _exam.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                        }

                        data.stafflist = (from a in _db.LMS_Live_MeetingDMO
                                          from b in _db.HR_Master_Employee_DMO
                                          where a.HRME_Id == b.HRME_Id && a.HRME_Id == data.HRME_Id && a.MI_Id == data.MI_Id
                                          select new LiveMeetingScheduleDTO
                                          {
                                              HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "0" ? "" : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "0" ? "" : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "0" ? "" : b.HRME_EmployeeLastName)).Trim(),
                                              HRME_Id = b.HRME_Id,

                                          }
                                ).Distinct().OrderBy(w => w.HRME_EmployeeFirstName).ToArray();
                    }
                    else
                    {
                        data.stafflist = (from a in _db.LMS_Live_MeetingDMO
                                          from b in _db.HR_Master_Employee_DMO
                                          where a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id
                                          select new LiveMeetingScheduleDTO
                                          {
                                              HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "0" ? "" : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "0" ? "" : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "0" ? "" : b.HRME_EmployeeLastName)).Trim(),
                                              HRME_Id = b.HRME_Id,

                                          }
                              ).Distinct().OrderBy(w => w.HRME_EmployeeFirstName).ToArray();
                    }



                }
                data.academicList = _exam.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();

                data.classlist = _exam.AdmissionClass.Where(e => e.MI_Id == data.MI_Id && e.ASMCL_ActiveFlag == true).ToArray();

                data.SectionList = _exam.School_M_Section.Where(e => e.MI_Id == data.MI_Id && e.ASMC_ActiveFlag == 1).ToArray();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;

        }
        public LiveMeetingScheduleDTO getstudentprofiledata(LiveMeetingScheduleDTO data)
        {
            try
            {
                if (data.RoleId > 0)
                {
                    var roletyp = _db.MasterRoleType.Where(t => t.IVRMRT_Id == data.RoleId).ToList();
                    data.roletype = roletyp.FirstOrDefault().IVRMRT_Role;
                    if (data.roletype.Equals("student", StringComparison.OrdinalIgnoreCase))
                    {
                        data.fillstudentalldetails = (from a in _db.Adm_M_Student
                                                      from b in _db.School_Adm_Y_StudentDMO
                                                      from c in _db.LMS_Live_MeetingDMO
                                                      from d in _db.LMS_Live_Meeting_ClassDMO
                                                      where (a.AMST_Id == b.AMST_Id && b.ASMAY_Id == d.ASMAY_Id && b.ASMCL_Id == d.ASMCL_Id && b.ASMS_Id == d.ASMS_Id && c.LMSLMEET_Id == d.LMSLMEET_Id && c.LMSLMEET_Id == d.LMSLMEET_Id && b.AMST_Id == data.Amst_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id)
                                                      select new LiveMeetingScheduleDTO
                                                      {
                                                          amst_FirstName = ((a.AMST_FirstName == null ? " " : a.AMST_FirstName) + " " + (a.AMST_MiddleName == null ? " " : a.AMST_MiddleName) + " " + (a.AMST_LastName == null ? " " : a.AMST_LastName)).Trim(),
                                                          Amst_Id = a.AMST_Id,
                                                      }
                    ).Distinct().ToArray();

                        data.academicList = _exam.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(a => a.ASMAY_Order).ToArray();


                    }
                    else
                    {
                        //data.stafflist = (from a in _db.LMS_Live_MeetingDMO
                        //                  from b in _db.HR_Master_Employee_DMO
                        //                  where a.HRME_Id == b.HRME_Id && a.MI_Id == data.MI_Id
                        //                  select new LiveMeetingScheduleDTO
                        //                  {
                        //                      HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null ? " " : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null ? " " : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null ? " " : b.HRME_EmployeeLastName)).Trim(),
                        //                      HRME_Id = b.HRME_Id,

                        //                  }
                        //      ).Distinct().OrderBy(w => w.HRME_EmployeeFirstName).ToArray();
                    }



                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;

        }
        public LiveMeetingScheduleDTO getschedulereport(LiveMeetingScheduleDTO data)
        {

            try
            {
                string stafids = "";
                string classidss = "0";
                string secidss = "0";

                if (data.stfids != null)
                {
                    var cnt = 0;
                    foreach (var item in data.stfids)
                    {
                        if (cnt == 0)
                        {
                            stafids = item.HRME_Id.ToString();
                        }
                        else
                        {
                            stafids = stafids + "," + item.HRME_Id.ToString();
                        }
                        cnt += 1;
                    }
                }

                if (data.rtype2=="ALL" || data.rtype2 == "STD")
                {
                    if (data.rtype2 == "STD")
                    {

                        if (data.selectedClasslist !=null)
                        {
                            foreach (var item in data.selectedClasslist)
                            {
                                classidss = classidss + "," + item.ASMCL_Id.ToString();
                            }
                        }

                        if (data.secids != null)
                        {
                            foreach (var item in data.secids)
                            {
                                secidss = secidss + "," + item.ASMS_Id.ToString();
                            }
                        }



                        using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "STAFF_MEETING_SCHEDULE_CLASS_REPORT_CLASSWISE";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                           SqlDbType.BigInt)
                            {
                                Value = data.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@Staffid",
                              SqlDbType.VarChar)
                            {
                                Value = stafids
                            });
                            cmd.Parameters.Add(new SqlParameter("@FromDate",
                            SqlDbType.VarChar)
                            {
                                Value = data.FromDate.ToString("yyyy-MM-dd")
                            });
                            cmd.Parameters.Add(new SqlParameter("@ToDate",
                            SqlDbType.VarChar)
                            {
                                Value = data.ToDate.ToString("yyyy-MM-dd")
                            });

                            cmd.Parameters.Add(new SqlParameter("@TYPE",
                             SqlDbType.VarChar)
                            {
                                Value = data.rtype
                            });
                            cmd.Parameters.Add(new SqlParameter("@classids",
                             SqlDbType.VarChar)
                            {
                                Value = classidss
                            });
                            cmd.Parameters.Add(new SqlParameter("@sectionidss",
                             SqlDbType.VarChar)
                            {
                                Value = secidss
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
                                data.meetinglist = retObject.ToArray();
                            }

                            catch (Exception ex)
                            {
                                Console.Write(ex.Message);
                            }
                        }
                    }
                    else
                    {
                        using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Staff_Meeting_Schedule_Class_Report";
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add(new SqlParameter("@MI_Id",
                           SqlDbType.BigInt)
                            {
                                Value = data.MI_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@Staffid",
                              SqlDbType.VarChar)
                            {
                                Value = stafids
                            });
                            cmd.Parameters.Add(new SqlParameter("@FromDate",
                            SqlDbType.VarChar)
                            {
                                Value = data.FromDate.ToString("yyyy-MM-dd")
                            });
                            cmd.Parameters.Add(new SqlParameter("@ToDate",
                            SqlDbType.VarChar)
                            {
                                Value = data.ToDate.ToString("yyyy-MM-dd")
                            });

                            cmd.Parameters.Add(new SqlParameter("@TYPE",
                             SqlDbType.VarChar)
                            {
                                Value = data.rtype
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
                                data.meetinglist = retObject.ToArray();
                            }

                            catch (Exception ex)
                            {
                                Console.Write(ex.Message);
                            }
                        }
                    }

                   
                }
               

                if (data.rtype2 == "ALL" || data.rtype2 == "STF")
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "Staff_Meeting_Schedule_Staff_Report";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = data.MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Staffid",
                          SqlDbType.VarChar)
                        {
                            Value = stafids
                        });
                        cmd.Parameters.Add(new SqlParameter("@FromDate",
                        SqlDbType.VarChar)
                        {
                            Value = data.FromDate.ToString("yyyy-MM-dd")
                        });
                        cmd.Parameters.Add(new SqlParameter("@ToDate",
                        SqlDbType.VarChar)
                        {
                            Value = data.ToDate.ToString("yyyy-MM-dd")
                        });

                        cmd.Parameters.Add(new SqlParameter("@TYPE",
                         SqlDbType.VarChar)
                        {
                            Value = data.rtype
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
                            data.meetingliststaff = retObject.ToArray();
                        }

                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                        }
                    }
                }
               

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }
        public LiveMeetingScheduleDTO getschrptdetailsprofile(LiveMeetingScheduleDTO data)
        {
            try
            {
                if (data.RoleId > 0)
                {
                    var roletyp = _db.MasterRoleType.Where(t => t.IVRMRT_Id == data.RoleId).ToList();
                    data.roletype = roletyp.FirstOrDefault().IVRMRT_Role;
                    if (data.roletype.Equals("staff", StringComparison.OrdinalIgnoreCase))
                    {
                        if (data.HRME_Id == 0)
                        {
                            data.HRME_Id = _exam.Staff_User_Login.Single(c => c.Id == data.UserId && c.MI_Id == data.MI_Id).Emp_Code;
                        }

                        data.stafflist = (from a in _db.LMS_Live_Meeting_StaffOthersDMO
                                          from b in _db.HR_Master_Employee_DMO
                                          where a.HRME_Id == b.HRME_Id && a.HRME_Id == data.HRME_Id && b.MI_Id == data.MI_Id
                                          select new LiveMeetingScheduleDTO
                                          {
                                              HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "0" ? "" : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "0" ? "" : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "0" ? "" : b.HRME_EmployeeLastName)).Trim(),
                                              HRME_Id = b.HRME_Id,

                                          }
                                ).Distinct().OrderBy(w => w.HRME_EmployeeFirstName).ToArray();
                    }
                    else
                    {
                        data.stafflist = (from a in _db.LMS_Live_Meeting_StaffOthersDMO
                                          from b in _db.HR_Master_Employee_DMO
                                          where a.HRME_Id == b.HRME_Id && b.MI_Id == data.MI_Id
                                          select new LiveMeetingScheduleDTO
                                          {
                                              HRME_EmployeeFirstName = ((b.HRME_EmployeeFirstName == null || b.HRME_EmployeeFirstName == "0" ? "" : b.HRME_EmployeeFirstName) + " " + (b.HRME_EmployeeMiddleName == null || b.HRME_EmployeeMiddleName == "0" ? "" : b.HRME_EmployeeMiddleName) + " " + (b.HRME_EmployeeLastName == null || b.HRME_EmployeeLastName == "0" ? "" : b.HRME_EmployeeLastName)).Trim(),
                                              HRME_Id = b.HRME_Id,

                                          }
                              ).Distinct().OrderBy(w => w.HRME_EmployeeFirstName).ToArray();
                    }



                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;

        }
        public LiveMeetingScheduleDTO getstaffprofilereport(LiveMeetingScheduleDTO data)
        {

            try
            {
                string stafids = "";

                if (data.stfids != null)
                {
                    var cnt = 0;
                    foreach (var item in data.stfids)
                    {
                        if (cnt == 0)
                        {
                            stafids = item.HRME_Id.ToString();
                        }
                        else
                        {
                            stafids = stafids + "," + item.HRME_Id.ToString();
                        }
                        cnt += 1;
                    }
                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Staff_Meeting_Profile_Staff_Report";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                   SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Staffid",
                      SqlDbType.VarChar)
                    {
                        Value = stafids
                    });
                    cmd.Parameters.Add(new SqlParameter("@FromDate",
                    SqlDbType.VarChar)
                    {
                        Value = data.FromDate.ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@ToDate",
                    SqlDbType.VarChar)
                    {
                        Value = data.ToDate.ToString("yyyy-MM-dd")
                    });

                    cmd.Parameters.Add(new SqlParameter("@TYPE",
                     SqlDbType.VarChar)
                    {
                        Value = data.rtype
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
                        data.meetingliststaff = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }
        public LiveMeetingScheduleDTO getstudentprofilereport(LiveMeetingScheduleDTO data)
        {

            try
            {
                string stafids = "";

                if (data.stids != null)
                {
                    var cnt = 0;
                    foreach (var item in data.stids)
                    {
                        if (cnt == 0)
                        {
                            stafids = item.amst_Id.ToString();
                        }
                        else
                        {
                            stafids = stafids + "," + item.amst_Id.ToString();
                        }
                        cnt += 1;
                    }
                }

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Student_Meeting_Profile_Student_Report";
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
                    cmd.Parameters.Add(new SqlParameter("@Staffid",
                      SqlDbType.VarChar)
                    {
                        Value = stafids
                    });
                    cmd.Parameters.Add(new SqlParameter("@FromDate",
                    SqlDbType.VarChar)
                    {
                        Value = data.FromDate.ToString("yyyy-MM-dd")
                    });
                    cmd.Parameters.Add(new SqlParameter("@ToDate",
                    SqlDbType.VarChar)
                    {
                        Value = data.ToDate.ToString("yyyy-MM-dd")
                    });

                    cmd.Parameters.Add(new SqlParameter("@TYPE",
                     SqlDbType.VarChar)
                    {
                        Value = data.rtype
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
                        data.meetingliststaff = retObject.ToArray();
                    }

                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }

        public LiveMeetingScheduleDTO sendnotification(LiveMeetingScheduleDTO data)
        {           
            List<LiveMeetingScheduleDTO> empdevlist = new List<LiveMeetingScheduleDTO>();
            try
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GETSTUDENT_LIST_FOR_MEETING";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@meetingid",
                  SqlDbType.BigInt)
                    {
                        Value = data.LMSLMEET_Id
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
                                empdevlist.Add(new LiveMeetingScheduleDTO
                                {
                                    HRME_EmployeeFirstName = Convert.ToString(dataReader["EmpName"]),
                                    mobiledeviceid = Convert.ToString(dataReader["DeviceId"]),
                                    amst_emailid = Convert.ToString(dataReader["AMST_emailId"]),
                                    amst_mobile = Convert.ToInt64(dataReader["AMST_MobileNo"]),
                                    LMSLMEET_MeetingTopic = Convert.ToString(dataReader["ISMS_SubjectName"]),
                                    LMSLMEET_StartedTime = Convert.ToString(dataReader["LMSLMEET_StartedTime"])
                                });
                            }
                        }
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

            if (empdevlist.Count() > 0)
            {

                var deviceidsnew = "";
                var devicenew = "";
                if (empdevlist.Count > 0)
                {
                    int k = 0;
                    foreach (var deviceid in empdevlist)
                    {
                        if (deviceid.mobiledeviceid != null && deviceid.mobiledeviceid != "" && deviceid.mobiledeviceid != "0")
                        {
                            if (k == 0)
                            {
                                deviceidsnew = '"' + deviceid.mobiledeviceid + '"';
                                k = 1;
                            }
                            else
                            {
                                deviceidsnew = deviceidsnew + "," + '"' + deviceid.mobiledeviceid + '"';
                            }
                        }
                    }
                    devicenew = "[" + deviceidsnew + "]";

                    callnotificationstudents(devicenew, data.LMSLMEET_Id, data.MI_Id, empdevlist.FirstOrDefault().LMSLMEET_MeetingTopic, empdevlist.FirstOrDefault().HRME_EmployeeFirstName, empdevlist.FirstOrDefault().LMSLMEET_StartedTime);
                }
            }

            if(empdevlist.Count() > 0)
            {
                foreach (var deviceid in empdevlist)
                {                  
                   if(deviceid.amst_emailid!=null && deviceid.amst_emailid != "")
                    {
                        Email Email = new Email(_db);
                        Email.sendmail(data.MI_Id, deviceid.amst_emailid, "ONLINE_CLASS_MEETING_STUDENT", data.LMSLMEET_Id);

                    }
                    if (deviceid.amst_mobile != null && deviceid.amst_mobile != 0)
                    {
                        SMS sms = new SMS(_db);
                        sms.sendSms(data.MI_Id, deviceid.amst_mobile, "ONLINE_CLASS_MEETING_STUDENT", data.LMSLMEET_Id);
                    }
                }
            }          

            return data;
        }
        public async Task  Start()
        {
            long miid = localmiid;
            List<LiveMeetingScheduleDTO> empdevlist = new List<LiveMeetingScheduleDTO>();
            try
            {
                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "GET_MEETING_NOTIFICATION_INTERVAL";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@miid",
                  SqlDbType.BigInt)
                    {
                        Value = miid
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
                                empdevlist.Add(new LiveMeetingScheduleDTO
                                {
                                    interval = Convert.ToInt64(dataReader["Interval"])
                                  
                                });
                            }
                        }
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

            if(empdevlist.Count()>0)
            {
                timer.Interval = empdevlist.FirstOrDefault().interval; //300,000 miliseconds = 5 minutes
                timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
                timer.Start();
            }
            //else
            //{
            //    timer.Interval = 300000; //300,000 miliseconds = 5 minutes
            //    timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            //    timer.Start();
            //}           
        }

        public void Stop()
        {
            //If you need to stop it
            timer.Stop();
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            YourFunction();

        }

        public async Task YourFunction()
        {
            Stop();

            long tempmeeting = meetingid;
            long miid = localmiid;
            LiveMeetingScheduleDTO dd = new LiveMeetingScheduleDTO();
            LiveMeetingScheduleDTO vmspaymentsubsctiptiondto = new LiveMeetingScheduleDTO();
            vmspaymentsubsctiptiondto.LMSLMEET_Id = tempmeeting;
            vmspaymentsubsctiptiondto.MI_Id = miid;

            string stringurl = "http://localhost:51263/";
            HttpClient client1 = new HttpClient();
            client1.BaseAddress = new Uri(stringurl);
            client1.DefaultRequestHeaders.Accept.Clear();
            client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client1.PostAsJsonAsync("api/LiveMeetingScheduleFacade/sendnotification", vmspaymentsubsctiptiondto).Result;

            string description = string.Empty;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                description = result;

                dd = JsonConvert.DeserializeObject<LiveMeetingScheduleDTO>(description, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            }          
        }       
    }
}
