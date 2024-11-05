using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using DomainModel.Model;
using DataAccessMsSqlServerProvider;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Dynamic;
using System.Net;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Http;
using System.Web;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.Birthday;

namespace CommonLibrary
{
    public class SMS
    {
        public DomainModelMsSqlServerContext _db;
        private readonly ApplicationDBContext _ApplicationDBContext;

        public SMS(DomainModelMsSqlServerContext db)
        {
            _db = db;
        }
        // public Array Values { get; set; }

        public async Task<string> sendSmsStaff(long MI_Id, long UserID, long mobileno, string Template)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = sms;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (variables.Count == 0)
                {
                    sms = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VISITOR_SMS_Email_PARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                           SqlDbType.VarChar)
                        {
                            Value = MI_Id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }

                    }

                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                sms = result;
                            }
                        }
                    }

                    sms = result;
                }

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                List<Institution> insdeta = new List<Institution>();
                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileno.ToString();


                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());

                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;

                    var remoteIpAddress = "";
                    string strHostName = "";
                    strHostName = System.Net.Dns.GetHostName();
                    IPHostEntry ipEntry = await System.Net.Dns.GetHostEntryAsync(strHostName);
                    IPAddress[] addr = ipEntry.AddressList;
                    remoteIpAddress = addr[addr.Length - 1].ToString();

                    string hostName = Dns.GetHostName();
                    var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                    string myIP1 = addr[addr.Length - 2].ToString();

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

                    IVRM_sms_sentBoxDMO dmo2 = new IVRM_sms_sentBoxDMO();
                    dmo2.CreatedDate = DateTime.Now;
                    dmo2.Datetime = DateTime.Now;
                    dmo2.Message = sms.ToString();
                    dmo2.Message_id = messageid;
                    dmo2.MI_Id = MI_Id;
                    dmo2.Mobile_no = PHNO;
                    dmo2.Module_Name = "Visitors Management";
                    dmo2.To_FLag = "Visitor Appointment";
                    dmo2.UpdatedDate = DateTime.Now;
                    dmo2.System_Ip = remoteIpAddress;
                    dmo2.MacAddress_Ip = sMacAddress;
                    dmo2.network_Ip = myIP1;
                    if (messageid.Contains("GID") && messageid.Contains("ID"))
                    {
                        dmo2.statusofmessage = "Delivered";
                    }
                    else
                    {
                        dmo2.statusofmessage = "Delivered";
                    }

                    _db.Add(dmo2);
                    var flag = _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // return ex.Message;
            }
            return "success";
        }
        public async Task<string> sendSmsStaffReminder(long MI_Id, long UserID, long mobileno, string Template)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = sms;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (variables.Count == 0)
                {
                    sms = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VISITOR_SMS_EMAIL_PARAMETER_REMINDER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                           SqlDbType.VarChar)
                        {
                            Value = MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@Template",
                           SqlDbType.VarChar)
                        {
                            Value = Template
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }

                    }

                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                sms = result;
                            }
                        }
                    }

                    sms = result;
                }

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                List<Institution> insdeta = new List<Institution>();
                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();


                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileno.ToString();


                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());

                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);


                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;

                    var remoteIpAddress = "";
                    string strHostName = "";
                    strHostName = System.Net.Dns.GetHostName();
                    IPHostEntry ipEntry = await System.Net.Dns.GetHostEntryAsync(strHostName);
                    IPAddress[] addr = ipEntry.AddressList;
                    remoteIpAddress = addr[addr.Length - 1].ToString();

                    string hostName = Dns.GetHostName();
                    var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                    string myIP1 = addr[addr.Length - 2].ToString();

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

                    IVRM_sms_sentBoxDMO dmo2 = new IVRM_sms_sentBoxDMO();
                    dmo2.CreatedDate = DateTime.Now;
                    dmo2.Datetime = DateTime.Now;
                    dmo2.Message = sms.ToString();
                    dmo2.Message_id = messageid;
                    dmo2.MI_Id = MI_Id;
                    dmo2.Mobile_no = PHNO;
                    dmo2.Module_Name = "Visitors Management";
                    dmo2.To_FLag = "Visitor Appointment";
                    dmo2.UpdatedDate = DateTime.Now;
                    dmo2.System_Ip = remoteIpAddress;
                    dmo2.MacAddress_Ip = sMacAddress;
                    dmo2.network_Ip = myIP1;
                    if (messageid.Contains("GID") && messageid.Contains("ID"))
                    {
                        dmo2.statusofmessage = "Delivered";
                    }
                    else
                    {
                        dmo2.statusofmessage = "Delivered";
                    }

                    _db.Add(dmo2);
                    var flag = _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // return ex.Message;
            }
            return "success";
        }
        public async Task<string> sendSmsVisitorReminder(long MI_Id, long UserID, long mobileno, string Template)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = sms;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (variables.Count == 0)
                {
                    sms = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VISITOR_SMS_EMAIL_PARAMETER_REMINDER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                           SqlDbType.VarChar)
                        {
                            Value = MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@Template",
                           SqlDbType.VarChar)
                        {
                            Value = Template
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }

                    }

                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                sms = result;
                            }
                        }
                    }

                    sms = result;
                }

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                List<Institution> insdeta = new List<Institution>();
                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();


                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileno.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());

                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;

                    var remoteIpAddress = "";
                    string strHostName = "";
                    strHostName = System.Net.Dns.GetHostName();
                    IPHostEntry ipEntry = await System.Net.Dns.GetHostEntryAsync(strHostName);
                    IPAddress[] addr = ipEntry.AddressList;
                    remoteIpAddress = addr[addr.Length - 1].ToString();

                    string hostName = Dns.GetHostName();
                    var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                    string myIP1 = addr[addr.Length - 2].ToString();

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

                    IVRM_sms_sentBoxDMO dmo2 = new IVRM_sms_sentBoxDMO();
                    dmo2.CreatedDate = DateTime.Now;
                    dmo2.Datetime = DateTime.Now;
                    dmo2.Message = sms.ToString();
                    dmo2.Message_id = messageid;
                    dmo2.MI_Id = MI_Id;
                    dmo2.Mobile_no = PHNO;
                    dmo2.Module_Name = "Visitors Management";
                    dmo2.To_FLag = "Visitor Appointment";
                    dmo2.UpdatedDate = DateTime.Now;
                    dmo2.System_Ip = remoteIpAddress;
                    dmo2.MacAddress_Ip = sMacAddress;
                    dmo2.network_Ip = myIP1;
                    if (messageid.Contains("GID") && messageid.Contains("ID"))
                    {
                        dmo2.statusofmessage = "Delivered";
                    }
                    else
                    {
                        dmo2.statusofmessage = "Delivered";
                    }

                    _db.Add(dmo2);
                    var flag = _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // return ex.Message;
            }
            return "success";
        }
        public async Task<string> sendSmsVisitor(long MI_Id, long UserID, long mobileno, string Template)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = sms;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (variables.Count == 0)
                {
                    sms = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "VISITOR_SMS_Email_PARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                           SqlDbType.VarChar)
                        {
                            Value = MI_Id
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }

                    }

                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                sms = result;
                            }
                        }
                    }

                    sms = result;
                }

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                List<Institution> insdeta = new List<Institution>();
                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();


                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileno.ToString();


                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());

                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);


                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;

                    var remoteIpAddress = "";
                    string strHostName = "";
                    strHostName = System.Net.Dns.GetHostName();
                    IPHostEntry ipEntry = await System.Net.Dns.GetHostEntryAsync(strHostName);
                    IPAddress[] addr = ipEntry.AddressList;
                    remoteIpAddress = addr[addr.Length - 1].ToString();

                    string hostName = Dns.GetHostName();
                    var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                    string myIP1 = addr[addr.Length - 2].ToString();

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

                    IVRM_sms_sentBoxDMO dmo2 = new IVRM_sms_sentBoxDMO();
                    dmo2.CreatedDate = DateTime.Now;
                    dmo2.Datetime = DateTime.Now;
                    dmo2.Message = sms.ToString();
                    dmo2.Message_id = messageid;
                    dmo2.MI_Id = MI_Id;
                    dmo2.Mobile_no = PHNO;
                    dmo2.Module_Name = "Visitors Management";
                    dmo2.To_FLag = "Visitor Appointment";
                    dmo2.UpdatedDate = DateTime.Now;
                    dmo2.System_Ip = remoteIpAddress;
                    dmo2.MacAddress_Ip = sMacAddress;
                    dmo2.network_Ip = myIP1;
                    if (messageid.Contains("GID") && messageid.Contains("ID"))
                    {
                        dmo2.statusofmessage = "Delivered";
                    }
                    else
                    {
                        dmo2.statusofmessage = "Delivered";
                    }

                    _db.Add(dmo2);
                    var flag = _db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // return ex.Message;
            }
            return "success";
        }

        public async Task<string> sendSms_TT(long MI_Id, long mobileNo, string Template, long UserID)
        {

            try
            {

                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = sms;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                    sms = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {


                        cmd.CommandText = "SMSMAILPARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@template",
                           SqlDbType.VarChar)
                        {
                            Value = Template
                        });


                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }

                    }

                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                sms = result;
                            }
                        }
                    }

                    sms = result;
                }


                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                List<Institution> insdeta = new List<Institution>();
                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();


                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());

                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);


                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;




                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                            SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
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

                        cmd.Parameters.Add(new SqlParameter("@status",
                   SqlDbType.VarChar)
                        {
                            Value = "Delivered"
                        });

                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                SqlDbType.VarChar)
                        {
                            Value = messageid
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
        public async Task<string> sendSms1(long MI_Id, string studentName, string evename, string Template, long? AMVM_Contact_No)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = sms;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }


                for (int j = 0; j < ParamaetersName.Count; j++)
                {
                    if (ParamaetersName[j].ISMP_NAME == "[NAME]")
                    {
                        result = sms.Replace(ParamaetersName[j].ISMP_NAME, studentName);
                        sms = result;
                    }
                    if (ParamaetersName[j].ISMP_NAME == "[EVENT]")
                    {
                        result = sms.Replace(ParamaetersName[j].ISMP_NAME, evename);
                        sms = result;
                    }
                }
                sms = result;



                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                List<Institution> insdeta = new List<Institution>();
                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = AMVM_Contact_No.ToString();

                    // PHNO = "9771237044";
                    url = url.Replace("PHNO", PHNO);
                    url = url.Replace("MESSAGE", sms);
                    url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());
                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);


                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;
                    #region

                    var str = "";
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                            SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
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

                        cmd.Parameters.Add(new SqlParameter("@status",
                   SqlDbType.VarChar)
                        {
                            Value = "Delivered"
                        });

                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                SqlDbType.VarChar)
                        {
                            Value = messageid
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
                    #endregion

                    //_db.Dispose();

                    //_dbcontext.Database.GetDbConnection();
                    //IVRM_sms_sentBoxDMO dmo2 = new IVRM_sms_sentBoxDMO();
                    //dmo2.CreatedDate = DateTime.Now;
                    //dmo2.Datetime = DateTime.Now;
                    //dmo2.Message = sms.ToString();
                    //dmo2.Message_id = messageid;
                    //dmo2.MI_Id = MI_Id;
                    //dmo2.Mobile_no = PHNO;
                    //dmo2.Module_Name = "Sports";
                    ////dmo2.To_FLag = type;
                    //dmo2.UpdatedDate = DateTime.Now;
                    //if (messageid.Contains("GID") && messageid.Contains("ID"))
                    //{
                    //    dmo2.statusofmessage = "Delivered";
                    //}
                    //else
                    //{
                    //    dmo2.statusofmessage = messageid;
                    //}

                    //_dbcontext.Add(dmo2);
                    //var flag = _dbcontext.SaveChanges();

                    //methos(MI_Id, Template, PHNO, sms, messageid);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }
        public async Task<string> sendsmsforhirer(long MI_Id, long mobileNo, string Template, long UserID, long TRTOB_Id, long driverid)
        {
            try
            {

                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = sms;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                    sms = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {


                        cmd.CommandText = "TR_HirerMessageSend";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@TRTOB_Id",
                         SqlDbType.BigInt)
                        {
                            Value = TRTOB_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@TEMPL",
                         SqlDbType.VarChar)
                        {
                            Value = Template
                        });
                        cmd.Parameters.Add(new SqlParameter("@TRMD_Id",
                        SqlDbType.BigInt)
                        {
                            Value = driverid
                        });


                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }

                    }

                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                sms = result;
                            }
                        }
                    }

                    sms = result;
                }


                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                List<Institution> insdeta = new List<Institution>();
                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();


                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());

                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);


                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                            SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            //Value = modulename[0]
                            Value = "BUSHIRE"
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@status",
                   SqlDbType.VarChar)
                        {
                            Value = "Delivered"
                        });

                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                SqlDbType.VarChar)
                        {
                            Value = messageid
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
        public async Task<string> saveSmsnewwithouttemplete_newtable(long MI_Id, long mobileNo, string Template, long UserID, string sms, long trnsno, string studentempflag, long senderid)
        {

            try
            {

                var msgid = string.Empty;
                var mainstatus = string.Empty;
                var status = string.Empty;
                DateTime? delitime = null;
                string msg = string.Empty;
                var message = string.Empty;
                var appstatus = string.Empty;
                Dictionary<string, string> val = new Dictionary<string, string>();



                appstatus = "PENDING";
                string result = sms;

                //var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
                var remoteIpAddress = "";
                //string netip = remoteIpAddress.ToString();

                string strHostName = "";
                strHostName = System.Net.Dns.GetHostName();

                IPHostEntry ipEntry = await System.Net.Dns.GetHostEntryAsync(strHostName);

                IPAddress[] addr = ipEntry.AddressList;

                remoteIpAddress = addr[addr.Length - 1].ToString();

                string hostName = Dns.GetHostName();
                var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                //  string myIP1 = ip_list.ToString();
                string myIP1 = addr[addr.Length - 2].ToString();

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


                bool flag = false;


                // long transactionid = 0;


                //  string studentempflag = true;
                Boolean scheduleflag = false;

                using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                {

                    cmd.CommandText = "IVRM_SMS_Outgoing_Table_Insert";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@Message",
                       SqlDbType.NVarChar)
                    {
                        Value = sms
                    });
                    cmd.Parameters.Add(new SqlParameter("@SSD_HeaderName",
                     SqlDbType.NVarChar)
                    {
                        Value = Template
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                 SqlDbType.BigInt)
                    {
                        Value = MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@SSD_TransactionId",
                    SqlDbType.BigInt)
                    {
                        Value = trnsno
                    });
                    cmd.Parameters.Add(new SqlParameter("@SSD_ToFlag",
                   SqlDbType.VarChar)
                    {
                        Value = flag
                    });
                    cmd.Parameters.Add(new SqlParameter("@SSD_SystemIP",
             SqlDbType.VarChar)
                    {
                        Value = remoteIpAddress
                    });

                    cmd.Parameters.Add(new SqlParameter("@SSD_NetworkIP",
             SqlDbType.VarChar)
                    {
                        Value = myIP1
                    });

                    cmd.Parameters.Add(new SqlParameter("@SSD_MAACAddress",
             SqlDbType.VarChar)
                    {
                        Value = sMacAddress
                    });

                    cmd.Parameters.Add(new SqlParameter("@SSD_SchedulerFlag",
             SqlDbType.Bit)
                    {
                        Value = flag
                    });

                    cmd.Parameters.Add(new SqlParameter("@mobileno",
          SqlDbType.BigInt)
                    {
                        Value = mobileNo
                    });
                    cmd.Parameters.Add(new SqlParameter("@senderid",
         SqlDbType.BigInt)
                    {
                        Value = senderid
                    });
                    cmd.Parameters.Add(new SqlParameter("@Messgstatus",
         SqlDbType.VarChar)
                    {
                        Value = status
                    });
                    cmd.Parameters.Add(new SqlParameter("@SSDNO_SMSStatusId",
        SqlDbType.VarChar)
                    {
                        Value = msg
                    });

                    cmd.Parameters.Add(new SqlParameter("@APP_STATUS",
       SqlDbType.VarChar)
                    {
                        Value = appstatus
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "Success";
        }
        public async Task<string> saveapproval(long MI_Id, string Template, long UserID, long trnsno, bool otpapproveflag, bool approveflag)
        {
            string str = string.Empty;
            string genotp = "";
            try
            {

                var approvaldetails = _db.SMSMasterApprovalDMO.Where(t => t.MI_Id == MI_Id && t.SMA_ActiveFlag == true && t.SMA_HeaderName.Trim().ToLower() == Template.Trim().ToLower() && t.SMA_SMSMailCallFlag == "SMS").Distinct().OrderBy(t => t.SMA_Level).ToList();

                if (approvaldetails.Count == 0)
                {
                    str = "SETUSER";
                }
                else
                {

                    foreach (var item in approvaldetails)
                    {
                        SMSApprovalStatusDMO res = new SMSApprovalStatusDMO();
                        res.MI_Id = MI_Id;
                        res.IRMUL_Id = item.IVRMUL_Id;
                        res.SMA_Id = item.SMA_Id;
                        res.STAD_TransNo = trnsno;
                        res.STAD_ApprStatus = "PENDING";
                        res.CreatedDate = DateTime.Now;
                        res.UpdatedDate = DateTime.Now;
                        _db.Add(res);
                    }

                    var exist = _db.SaveChanges();
                    if (exist > 0)
                    {
                        CommonLibrary.generateOTP generate = new CommonLibrary.generateOTP();
                        genotp = generate.getOTP();
                        if (genotp.Length < 6)
                        {
                            genotp = generate.getOTP();
                        }


                        var smssendotp = (from a in _db.SMSMasterApprovalDMO
                                          from b in _db.Staff_User_Login
                                          from c in _db.HR_Master_Employee_DMO
                                          from d in _db.Multiple_Mobile_DMO
                                          where (b.MI_Id == a.MI_Id && a.MI_Id == MI_Id && a.IVRMUL_Id == b.Id && b.Emp_Code == c.HRME_Id && c.MI_Id == a.MI_Id && d.HRME_Id == c.HRME_Id && a.IVRMUL_Id == approvaldetails[0].IVRMUL_Id)
                                          select new hrme_mobile_email
                                          {
                                              mobile = d.HRMEMNO_MobileNo

                                          }).Distinct().ToList();



                        var emailsendotp = (from a in _db.SMSMasterApprovalDMO
                                            from b in _db.Staff_User_Login
                                            from c in _db.HR_Master_Employee_DMO
                                            from d in _db.Multiple_Email_DMO
                                            where (b.MI_Id == a.MI_Id && a.MI_Id == MI_Id && a.IVRMUL_Id == b.Id && b.Emp_Code == c.HRME_Id && c.MI_Id == a.MI_Id && d.HRME_Id == c.HRME_Id && a.IVRMUL_Id == approvaldetails[0].IVRMUL_Id)
                                            select new hrme_mobile_email
                                            {
                                                email = d.HRMEM_EmailId

                                            }).Distinct().ToList();


                        if (emailsendotp.Count > 0)
                        {
                            if (emailsendotp[0].email != "")
                            {


                                Email Email = new Email(_db);
                                if (otpapproveflag == true)
                                {
                                    string t = Email.sendmail_new(MI_Id, emailsendotp[0].email, "APPROVALOTP", Convert.ToInt64(genotp), trnsno);
                                }
                                if (approveflag == true)
                                {
                                    string t = Email.sendmail_new(MI_Id, emailsendotp[0].email, "APPROVAL", Convert.ToInt64(genotp), trnsno);
                                }

                            }
                        }

                        if (smssendotp.Count > 0)
                        {
                            if (smssendotp[0].mobile != 0)
                            {

                                if (otpapproveflag == true)
                                {
                                    string g = await sendSmsnewTemplet_new(MI_Id, smssendotp[0].mobile, "APPROVALOTP", UserID, trnsno, "", Convert.ToInt64(genotp));
                                }

                                if (approveflag == true)
                                {
                                    string g = await sendSmsnewTemplet_new(MI_Id, smssendotp[0].mobile, "APPROVAL", UserID, trnsno, "", Convert.ToInt64(genotp));
                                }

                            }
                        }

                        if (otpapproveflag == true)
                        {
                            var updateotp = _db.SMSApprovalStatusDMO.Single(t => t.IRMUL_Id == approvaldetails[0].IVRMUL_Id && t.STAD_TransNo == trnsno);
                            updateotp.STAD_OTP = Convert.ToInt64(genotp);
                            _db.Update(updateotp);
                            _db.SaveChanges();
                        }


                        str = "success";




                    }
                    else
                    {
                        str = "faild";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return str;
        }
        public async Task<string> saveapproval_nextlevel(long MI_Id, string Template, long UserID, long trnsno, bool otpapproveflag, bool approveflag, int level)
        {
            string str = string.Empty;
            string genotp = "";
            try
            {

                var approvaldetails = _db.SMSMasterApprovalDMO.Where(t => t.MI_Id == MI_Id && t.SMA_ActiveFlag == true && t.SMA_HeaderName == Template.Trim().ToLower() && t.SMA_SMSMailCallFlag == "SMS" && t.SMA_Level == level + 1).Distinct().OrderBy(t => t.SMA_Level).ToList();

                if (approvaldetails.Count == 0)
                {
                    str = "SETUSER";
                }
                else
                {
                    CommonLibrary.generateOTP generate = new CommonLibrary.generateOTP();
                    genotp = generate.getOTP();
                    if (genotp.Length < 6)
                    {
                        genotp = generate.getOTP();
                    }


                    var smssendotp = (from a in _db.SMSMasterApprovalDMO
                                      from b in _db.Staff_User_Login
                                      from c in _db.HR_Master_Employee_DMO
                                      from d in _db.Multiple_Mobile_DMO
                                      where (b.MI_Id == a.MI_Id && a.MI_Id == MI_Id && a.IVRMUL_Id == b.Id && b.Emp_Code == c.HRME_Id && c.MI_Id == a.MI_Id && d.HRME_Id == c.HRME_Id && a.IVRMUL_Id == approvaldetails[0].IVRMUL_Id)
                                      select new hrme_mobile_email
                                      {
                                          mobile = d.HRMEMNO_MobileNo

                                      }).Distinct().ToList();



                    var emailsendotp = (from a in _db.SMSMasterApprovalDMO
                                        from b in _db.Staff_User_Login
                                        from c in _db.HR_Master_Employee_DMO
                                        from d in _db.Multiple_Email_DMO
                                        where (b.MI_Id == a.MI_Id && a.MI_Id == MI_Id && a.IVRMUL_Id == b.Id && b.Emp_Code == c.HRME_Id && c.MI_Id == a.MI_Id && d.HRME_Id == c.HRME_Id && a.IVRMUL_Id == approvaldetails[0].IVRMUL_Id)
                                        select new hrme_mobile_email
                                        {
                                            email = d.HRMEM_EmailId

                                        }).Distinct().ToList();


                    if (emailsendotp.Count > 0)
                    {
                        if (emailsendotp[0].email != "")
                        {
                            Email Email = new Email(_db);
                            if (otpapproveflag == true)
                            {
                                string t = Email.sendmail_new(MI_Id, emailsendotp[0].email, "APPROVALOTP", Convert.ToInt64(genotp), trnsno);
                            }
                            if (approveflag == true)
                            {
                                string t = Email.sendmail_new(MI_Id, emailsendotp[0].email, "APPROVAL", Convert.ToInt64(genotp), trnsno);
                            }
                        }
                    }

                    if (smssendotp.Count > 0)
                    {
                        if (smssendotp[0].mobile != 0)
                        {

                            if (otpapproveflag == true)
                            {
                                string g = await sendSmsnewTemplet_new(MI_Id, smssendotp[0].mobile, "APPROVALOTP", UserID, trnsno, "", Convert.ToInt64(genotp));
                            }

                            if (approveflag == true)
                            {
                                string g = await sendSmsnewTemplet_new(MI_Id, smssendotp[0].mobile, "APPROVAL", UserID, trnsno, "", Convert.ToInt64(genotp));
                            }

                        }
                    }

                    if (otpapproveflag == true)
                    {
                        var updateotp = _db.SMSApprovalStatusDMO.Single(t => t.IRMUL_Id == approvaldetails[0].IVRMUL_Id && t.STAD_TransNo == trnsno);
                        updateotp.STAD_OTP = Convert.ToInt64(genotp);
                        _db.Update(updateotp);
                        _db.SaveChanges();
                    }
                    str = "success";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return str;
        }
        public async Task<string> bushiresendSms(long MI_Id, long mobileNo, string Template, long UserID, string msg)
        {

            try
            {

                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = sms;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                    sms = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {


                        cmd.CommandText = "SMSMAILPARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                        SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@template",
                        SqlDbType.VarChar)
                        {
                            Value = Template
                        });


                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }

                    }

                    //result = sms.Replace(ParamaetersName[j].ISMP_NAME, msg);
                    // sms = result;
                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        //for (int p = 0; p < val.Count; p++)
                        //{
                        // if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                        // {
                        // result = sms.Replace(ParamaetersName[j].ISMP_NAME, msg);
                        // sms = result;
                        // }
                        //}
                        result = sms.Replace(ParamaetersName[j].ISMP_NAME, msg);
                        sms = result;
                    }

                    sms = result;
                }


                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                List<Institution> insdeta = new List<Institution>();
                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();



                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());

                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);


                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;


                    //try
                    //{
                    // var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                    // var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                    // var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                    // IVRM_sms_sentBoxDMO smssentbox = new IVRM_sms_sentBoxDMO();

                    // smssentbox.MI_Id = MI_Id;
                    // smssentbox.Mobile_no = PHNO;
                    // smssentbox.Message = sms;
                    // smssentbox.Datetime = DateTime.Now;

                    // smssentbox.Message_id = messageid;
                    // smssentbox.Module_Name = modulename[0];
                    // smssentbox.CreatedDate = DateTime.Now;
                    // smssentbox.UpdatedDate = DateTime.Now;

                    // smssentbox.statusofmessage = "Deliverd";
                    // smssentbox.To_FLag = "Student";

                    // _db.IVRM_sms_sentBoxDMO.Add(smssentbox);
                    // var returnmessage=_db.SaveChanges();

                    //}
                    //catch(Exception ex)
                    //{
                    // Console.WriteLine(ex.Message);
                    //}

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                        SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                        SqlDbType.NVarChar)
                        {
                            Value = sms
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            //Value = modulename[0]
                            Value = "BUSHIRE"
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@status",
                        SqlDbType.VarChar)
                        {
                            Value = "Delivered"
                        });

                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                        SqlDbType.VarChar)
                        {
                            Value = messageid
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
        public async Task<string> sendSms(long MI_Id, long mobileNo, string Template, long UserID)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = sms;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                    sms = result;
                }
                else
                {

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "SMSMAILPARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@template",
                           SqlDbType.VarChar)
                        {
                            Value = Template
                        });


                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }

                    }



                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                sms = result;
                            }
                        }
                    }

                    sms = result;
                }


                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                List<Institution> insdeta = new List<Institution>();
                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    //URL http://alerts.kaleyra.com/api/v4/?method=sms&api_key={{api_key}}&message=MESSAGE&sender={{sender}}&to=PHNO&entity_id=entityid&template_id=templateid

                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);
                    url = url.Replace("MESSAGE", sms);
                    url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());
                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);
                    if (url.Contains("entityid"))
                    {
                        if (insdeta[0].MI_EntityId.ToString() != null && insdeta[0].MI_EntityId.ToString() != "")
                        {
                            url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());
                        }
                    }
                    if (url.Contains("templateid"))
                    {
                        if (template.FirstOrDefault().ISES_TemplateId != null && template.FirstOrDefault().ISES_TemplateId != "")
                        {
                            url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);
                        }
                    }
                    

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;


                    //try
                    //{
                    //    var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                    //    var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                    //    var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                    //    IVRM_sms_sentBoxDMO smssentbox = new IVRM_sms_sentBoxDMO();

                    //    smssentbox.MI_Id = MI_Id;
                    //    smssentbox.Mobile_no = PHNO;
                    //    smssentbox.Message = sms;
                    //    smssentbox.Datetime = DateTime.Now;

                    //    smssentbox.Message_id = messageid;
                    //    smssentbox.Module_Name = modulename[0];
                    //    smssentbox.CreatedDate = DateTime.Now;
                    //    smssentbox.UpdatedDate = DateTime.Now;

                    //    smssentbox.statusofmessage = "Deliverd";
                    //    smssentbox.To_FLag = "Student";

                    //    _db.IVRM_sms_sentBoxDMO.Add(smssentbox);
                    //    var returnmessage=_db.SaveChanges();

                    //}
                    //catch(Exception ex)
                    //{
                    //    Console.WriteLine(ex.Message);
                    //}

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                            SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
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

                        cmd.Parameters.Add(new SqlParameter("@status",
                   SqlDbType.VarChar)
                        {
                            Value = "Delivered"
                        });

                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                SqlDbType.VarChar)
                        {
                            Value = messageid
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


        public async Task<string> sendSms_dailytwice(long MI_Id, long mobileNo, string Template, long UserID,string flag, long asmay_id)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = sms;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                    sms = result;
                }
                else
                {

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "SMSMAILPARAMETER_FORATTENDANCE";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@template",
                           SqlDbType.VarChar)
                        {
                            Value = Template
                        });
                        cmd.Parameters.Add(new SqlParameter("@FLag",
                         SqlDbType.VarChar)
                        {
                            Value = flag
                        });
                        cmd.Parameters.Add(new SqlParameter("@Asmay_Id",
                         SqlDbType.VarChar)
                        {
                            Value = asmay_id
                        });


                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }

                    }



                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                sms = result;
                            }
                        }
                    }

                    sms = result;
                }


                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                List<Institution> insdeta = new List<Institution>();
                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    //URL http://alerts.kaleyra.com/api/v4/?method=sms&api_key={{api_key}}&message=MESSAGE&sender={{sender}}&to=PHNO&entity_id=entityid&template_id=templateid

                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);
                    url = url.Replace("MESSAGE", sms);
                    url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());
                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);
                    if (url.Contains("entityid"))
                    {
                        if (insdeta[0].MI_EntityId.ToString() != null && insdeta[0].MI_EntityId.ToString() != "")
                        {
                            url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());
                        }
                    }
                    if (url.Contains("templateid"))
                    {
                        if (template.FirstOrDefault().ISES_TemplateId != null && template.FirstOrDefault().ISES_TemplateId != "")
                        {
                            url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);
                        }
                    }


                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;


                    //try
                    //{
                    //    var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                    //    var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                    //    var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                    //    IVRM_sms_sentBoxDMO smssentbox = new IVRM_sms_sentBoxDMO();

                    //    smssentbox.MI_Id = MI_Id;
                    //    smssentbox.Mobile_no = PHNO;
                    //    smssentbox.Message = sms;
                    //    smssentbox.Datetime = DateTime.Now;

                    //    smssentbox.Message_id = messageid;
                    //    smssentbox.Module_Name = modulename[0];
                    //    smssentbox.CreatedDate = DateTime.Now;
                    //    smssentbox.UpdatedDate = DateTime.Now;

                    //    smssentbox.statusofmessage = "Deliverd";
                    //    smssentbox.To_FLag = "Student";

                    //    _db.IVRM_sms_sentBoxDMO.Add(smssentbox);
                    //    var returnmessage=_db.SaveChanges();

                    //}
                    //catch(Exception ex)
                    //{
                    //    Console.WriteLine(ex.Message);
                    //}

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                            SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
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

                        cmd.Parameters.Add(new SqlParameter("@status",
                   SqlDbType.VarChar)
                        {
                            Value = "Delivered"
                        });

                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                SqlDbType.VarChar)
                        {
                            Value = messageid
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
        public async Task<string> sendSmsreg(long MI_Id, long mobileNo, string Template, long UserID, string password)
        {
            string responseparameters = "";
            try
            {

                Dictionary<string, string> val = new Dictionary<string, string>();
                val.Add("[PWD]", password);

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms1 = template.FirstOrDefault().ISES_SMSMessage;
                string sms = sms1.Replace("[PWD]", password);
                string result = sms;



                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                    sms = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {


                        cmd.CommandText = "SMSMAILPARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@template",
                           SqlDbType.VarChar)
                        {
                            Value = Template
                        });


                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }

                    }

                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                //result = sms.Replace(ParamaetersName[j].ISMP_NAME, val[ParamaetersName[p].ISMP_NAME]);
                                result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                sms = result;
                            }
                        }
                    }

                    sms = result;
                }


                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                List<Institution> insdeta = new List<Institution>();
                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();



                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());

                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);


                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;
                    if (responseparameters != "Invalid Template matched")
                    {
                        using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                        {
                            var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                            var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                            var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                            cmd.CommandText = "IVRM_SMS_Outgoing";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MobileNo",
                                SqlDbType.NVarChar)
                            {
                                Value = PHNO
                            });
                            cmd.Parameters.Add(new SqlParameter("@Message",
                               SqlDbType.NVarChar)
                            {
                                Value = sms
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

                            cmd.Parameters.Add(new SqlParameter("@status",
                       SqlDbType.VarChar)
                            {
                                Value = "Delivered"
                            });

                            cmd.Parameters.Add(new SqlParameter("@Message_id",
                    SqlDbType.VarChar)
                            {
                                Value = messageid
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
                        responseparameters = "success";
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return responseparameters;
        }
        public async Task<string> sendSmsnewwithouttemplet(long MI_Id, long mobileNo, string Template, long UserID, string sms, long trnsno, string studentempflag, long senderid)
        {

            try
            {

                Dictionary<string, string> val = new Dictionary<string, string>();

                string result = sms;

                //var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
                var remoteIpAddress = "";
                //string netip = remoteIpAddress.ToString();

                string strHostName = "";
                strHostName = System.Net.Dns.GetHostName();

                IPHostEntry ipEntry = await System.Net.Dns.GetHostEntryAsync(strHostName);

                IPAddress[] addr = ipEntry.AddressList;

                remoteIpAddress = addr[addr.Length - 1].ToString();



                string hostName = Dns.GetHostName();
                var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                //  string myIP1 = ip_list.ToString();
                string myIP1 = addr[addr.Length - 2].ToString();

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

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();


                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);


                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;

                    // long transactionid = 0;


                    //  string studentempflag = true;
                    Boolean scheduleflag = false;

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        //var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        //var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        //var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_SMS_Outgoing_new_table";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
                        });
                        cmd.Parameters.Add(new SqlParameter("@SSD_HeaderName",
                         SqlDbType.NVarChar)
                        {
                            Value = Template
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                     SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@SSD_TransactionId",
                        SqlDbType.BigInt)
                        {
                            Value = trnsno
                        });
                        cmd.Parameters.Add(new SqlParameter("@SSD_ToFlag",
                       SqlDbType.VarChar)
                        {
                            Value = studentempflag
                        });
                        cmd.Parameters.Add(new SqlParameter("@SSD_SystemIP",
                 SqlDbType.VarChar)
                        {
                            Value = remoteIpAddress
                        });

                        cmd.Parameters.Add(new SqlParameter("@SSD_NetworkIP",
                 SqlDbType.VarChar)
                        {
                            Value = myIP1
                        });

                        cmd.Parameters.Add(new SqlParameter("@SSD_MAACAddress",
                 SqlDbType.VarChar)
                        {
                            Value = sMacAddress
                        });

                        cmd.Parameters.Add(new SqlParameter("@SSD_SchedulerFlag",
                 SqlDbType.Bit)
                        {
                            Value = scheduleflag
                        });

                        cmd.Parameters.Add(new SqlParameter("@mobileno",
              SqlDbType.BigInt)
                        {
                            Value = mobileNo
                        });
                        cmd.Parameters.Add(new SqlParameter("@senderid",
             SqlDbType.BigInt)
                        {
                            Value = senderid
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
        public async Task<string> sendSmsnewTemplet(long MI_Id, long mobileNo, string Template, long UserID, long trnsno, string studentempflag, long senderid)
        {

            try
            {

                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = sms;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                    sms = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {


                        cmd.CommandText = "SMSMAILPARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@template",
                           SqlDbType.VarChar)
                        {
                            Value = Template
                        });


                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }

                    }

                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                sms = result;
                            }
                        }
                    }

                    sms = result;
                }

                //var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
                var remoteIpAddress = "";
                //string netip = remoteIpAddress.ToString();

                string strHostName = "";
                strHostName = System.Net.Dns.GetHostName();

                IPHostEntry ipEntry = await System.Net.Dns.GetHostEntryAsync(strHostName);

                IPAddress[] addr = ipEntry.AddressList;

                remoteIpAddress = addr[addr.Length - 1].ToString();



                string hostName = Dns.GetHostName();
                var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                //  string myIP1 = ip_list.ToString();
                string myIP1 = addr[addr.Length - 2].ToString();

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

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();


                List<Institution> insdeta = new List<Institution>();
                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();



                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());

                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;

                    // long transactionid = 0;


                    //  string studentempflag = true;
                    Boolean scheduleflag = false;

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        //var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        //var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        //var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_SMS_Outgoing_new_table";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
                        });
                        cmd.Parameters.Add(new SqlParameter("@SSD_HeaderName",
                         SqlDbType.NVarChar)
                        {
                            Value = Template
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                     SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@SSD_TransactionId",
                        SqlDbType.BigInt)
                        {
                            Value = trnsno
                        });
                        cmd.Parameters.Add(new SqlParameter("@SSD_ToFlag",
                       SqlDbType.VarChar)
                        {
                            Value = studentempflag
                        });
                        cmd.Parameters.Add(new SqlParameter("@SSD_SystemIP",
                 SqlDbType.VarChar)
                        {
                            Value = remoteIpAddress
                        });

                        cmd.Parameters.Add(new SqlParameter("@SSD_NetworkIP",
                 SqlDbType.VarChar)
                        {
                            Value = myIP1
                        });

                        cmd.Parameters.Add(new SqlParameter("@SSD_MAACAddress",
                 SqlDbType.VarChar)
                        {
                            Value = sMacAddress
                        });

                        cmd.Parameters.Add(new SqlParameter("@SSD_SchedulerFlag",
                 SqlDbType.Bit)
                        {
                            Value = scheduleflag
                        });

                        cmd.Parameters.Add(new SqlParameter("@mobileno",
              SqlDbType.BigInt)
                        {
                            Value = mobileNo
                        });
                        cmd.Parameters.Add(new SqlParameter("@senderid",
             SqlDbType.BigInt)
                        {
                            Value = senderid
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
        public long getsmsno(long MI_Id)
        {

            long transactionid = 0;
            var acd_Id = _db.AcademicYear.Where(t => t.MI_Id.Equals(MI_Id) && Convert.ToDateTime(t.ASMAY_From_Date) <= Convert.ToDateTime(System.DateTime.Today.Date) && Convert.ToDateTime(t.ASMAY_To_Date) >= Convert.ToDateTime(System.DateTime.Today.Date)).Select(d => d.ASMAY_Id).FirstOrDefault();

            List<Master_Numbering> masnum = new List<Master_Numbering>();
            masnum = _db.Master_Numbering.Where(t => t.MI_Id == MI_Id && t.IMN_Flag == "SMSEMAIL").ToList();

            StudentApplicationDTO stu = new StudentApplicationDTO();
            Master_NumberingDTO trns = new Master_NumberingDTO();
            trns.MI_Id = MI_Id;
            stu.transnumconfigsettings = trns;
            stu.transnumconfigsettings.IMN_AutoManualFlag = masnum.FirstOrDefault().IMN_AutoManualFlag;
            stu.transnumconfigsettings.IMN_DuplicatesFlag = masnum.FirstOrDefault().IMN_DuplicatesFlag;
            stu.transnumconfigsettings.IMN_Flag = masnum.FirstOrDefault().IMN_Flag;
            stu.transnumconfigsettings.IMN_Id = masnum.FirstOrDefault().IMN_Id;
            stu.transnumconfigsettings.IMN_PrefixAcadYearCode = masnum.FirstOrDefault().IMN_PrefixAcadYearCode;
            stu.transnumconfigsettings.IMN_PrefixCalYearCode = masnum.FirstOrDefault().IMN_PrefixCalYearCode;
            stu.transnumconfigsettings.IMN_PrefixFinYearCode = masnum.FirstOrDefault().IMN_PrefixFinYearCode;
            stu.transnumconfigsettings.IMN_PrefixParticular = masnum.FirstOrDefault().IMN_PrefixParticular;
            stu.transnumconfigsettings.IMN_RestartNumFlag = masnum.FirstOrDefault().IMN_RestartNumFlag;
            stu.transnumconfigsettings.IMN_StartingNo = masnum.FirstOrDefault().IMN_StartingNo;
            stu.transnumconfigsettings.IMN_SuffixAcadYearCode = masnum.FirstOrDefault().IMN_SuffixAcadYearCode;
            stu.transnumconfigsettings.IMN_SuffixCalYearCode = masnum.FirstOrDefault().IMN_SuffixCalYearCode;
            stu.transnumconfigsettings.IMN_SuffixFinYearCode = masnum.FirstOrDefault().IMN_SuffixFinYearCode;
            stu.transnumconfigsettings.IMN_SuffixParticular = masnum.FirstOrDefault().IMN_SuffixParticular;
            stu.transnumconfigsettings.IMN_WidthNumeric = masnum.FirstOrDefault().IMN_WidthNumeric;
            stu.transnumconfigsettings.IMN_ZeroPrefixFlag = masnum.FirstOrDefault().IMN_ZeroPrefixFlag;
            stu.transnumconfigsettings.ASMAY_Id = acd_Id;
            stu.transnumconfigsettings.MI_Id = MI_Id;

            if (masnum.Count > 0)
            {
                if (masnum[0].IMN_Flag == "SMSEMAIL" && masnum[0].IMN_AutoManualFlag == "Auto")
                {
                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                    transactionid = Convert.ToInt64(a.GenerateNumber(stu.transnumconfigsettings));

                }
            }

            return transactionid;

        }
        public async Task<string> sendSms(long MI_Id, long mobileNo, long UserID, string modulename, string sms)
        {

            try
            {

                Dictionary<string, string> val = new Dictionary<string, string>();

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();


                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);


                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();

                    //List<SMSParameters> list = JsonConvert.DeserializeObject<List<SMSParameters>>(responseparameters);

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {

                        // var modulename = "InstitutionCreation";
                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                            SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@status",
                   SqlDbType.VarChar)
                        {
                            Value = responseparameters
                        });

                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                SqlDbType.VarChar)
                        {
                            Value = 0
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
        public async Task<string> sendsmssection(long MI_Id, long mobileNo, string Template, long UserID, long ASMAY_Id)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = sms;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                    sms = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "SMSMAILPARAMETER_SECTIONALLOTMENT";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@template",
                           SqlDbType.VarChar)
                        {
                            Value = Template
                        });
                        cmd.Parameters.Add(new SqlParameter("@asmayid",
                         SqlDbType.VarChar)
                        {
                            Value = ASMAY_Id
                        });


                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }

                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                sms = result;
                            }
                        }
                    }

                    sms = result;
                }


                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                List<Institution> insdeta = new List<Institution>();
                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());

                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                            SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
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

                        cmd.Parameters.Add(new SqlParameter("@status",
                   SqlDbType.VarChar)
                        {
                            Value = "Delivered"
                        });

                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                SqlDbType.VarChar)
                        {
                            Value = messageid
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
        //Praveen
        public async Task<string> sendSmsnewwithouttemplete_newtable(long MI_Id, long mobileNo, string Template, long UserID, string sms, long trnsno, string studentempflag, long senderid)
        {
            try
            {
                var msgid = string.Empty;
                var mainstatus = string.Empty;
                var status = string.Empty;
                DateTime? delitime = null;
                string msg = string.Empty;
                var message = string.Empty;

                Dictionary<string, string> val = new Dictionary<string, string>();

                string result = sms;

                //var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
                var remoteIpAddress = "";
                //string netip = remoteIpAddress.ToString();

                string strHostName = "";
                strHostName = System.Net.Dns.GetHostName();

                IPHostEntry ipEntry = await System.Net.Dns.GetHostEntryAsync(strHostName);

                IPAddress[] addr = ipEntry.AddressList;

                remoteIpAddress = addr[addr.Length - 1].ToString();


                string hostName = Dns.GetHostName();
                var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                //  string myIP1 = ip_list.ToString();
                string myIP1 = addr[addr.Length - 2].ToString();

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

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                List<Institution> insdeta = new List<Institution>();
                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();
                    //PHNO="695874215";
                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);


                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    //var obj = JsonConvert.DeserializeObject<RootObject>(myContent);
                    //var messsge_id = obj.ID;

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);

                    string messageid = responsedata;

                    if (messageid.Contains("="))
                    {
                        var query = HttpUtility.ParseQueryString(url);

                        var var2 = query.Get("workingkey");
                        string[] name = messageid.Split("=");
                        msg = name[2];
                        var url1 = "https://api-alerts.solutionsinfini.com/v4/?api_key=" + var2 + "&method=sms.status&id=" + msg + "";
                        System.Net.HttpWebRequest request1 = System.Net.WebRequest.Create(url1) as HttpWebRequest;
                        System.Net.HttpWebResponse response1 = await request1.GetResponseAsync() as System.Net.HttpWebResponse;
                        Stream stream1 = response1.GetResponseStream();

                        StreamReader readStream1 = new StreamReader(stream1, Encoding.UTF8);
                        string responseparameters1 = readStream1.ReadToEnd();
                        var myContent1 = JsonConvert.SerializeObject(responseparameters1);

                        dynamic responsedata1 = JsonConvert.DeserializeObject(myContent1);
                        var statusdetails = JsonConvert.DeserializeObject<stausdata>(responsedata1);

                        if (statusdetails.data.Length > 0)
                        {
                            status = statusdetails.data[0].status;
                            if (statusdetails.data[0].dlrtime != null)
                            {
                                delitime = Convert.ToDateTime(statusdetails.data[0].dlrtime);


                            }
                            mainstatus = statusdetails.status;
                            message = statusdetails.message;
                        }
                        else
                        {
                            status = message = statusdetails.message;
                        }



                    }
                    else
                    {
                        status = messageid;
                    }

                    bool flag = true;


                    // long transactionid = 0;


                    //  string studentempflag = true;
                    Boolean scheduleflag = false;

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        //var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        //var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        //var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_SMS_Outgoing_Table_Insert";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
                        });
                        cmd.Parameters.Add(new SqlParameter("@SSD_HeaderName",
                         SqlDbType.NVarChar)
                        {
                            Value = Template
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                     SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@SSD_TransactionId",
                        SqlDbType.BigInt)
                        {
                            Value = trnsno
                        });
                        cmd.Parameters.Add(new SqlParameter("@SSD_ToFlag",
                       SqlDbType.VarChar)
                        {
                            Value = flag
                        });
                        cmd.Parameters.Add(new SqlParameter("@SSD_SystemIP",
                 SqlDbType.VarChar)
                        {
                            Value = remoteIpAddress
                        });

                        cmd.Parameters.Add(new SqlParameter("@SSD_NetworkIP",
                 SqlDbType.VarChar)
                        {
                            Value = myIP1
                        });

                        cmd.Parameters.Add(new SqlParameter("@SSD_MAACAddress",
                 SqlDbType.VarChar)
                        {
                            Value = sMacAddress
                        });

                        cmd.Parameters.Add(new SqlParameter("@SSD_SchedulerFlag",
                 SqlDbType.Bit)
                        {
                            Value = flag
                        });

                        cmd.Parameters.Add(new SqlParameter("@mobileno",
              SqlDbType.BigInt)
                        {
                            Value = mobileNo
                        });
                        cmd.Parameters.Add(new SqlParameter("@senderid",
             SqlDbType.BigInt)
                        {
                            Value = senderid
                        });
                        cmd.Parameters.Add(new SqlParameter("@Messgstatus",
             SqlDbType.VarChar)
                        {
                            Value = status
                        });
                        cmd.Parameters.Add(new SqlParameter("@SSDNO_SMSStatusId",
            SqlDbType.VarChar)
                        {
                            Value = msg
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
            return "Success";
        }
        public async Task<string> sendSmsapproval_update_newtable(long MI_Id, long mobileNo, string Template, long UserID, string sms, long trnsno, string studentempflag, long senderid, long SSDNO_Id, long SSD_Id)
        {

            try
            {
                var msgid = string.Empty;
                var mainstatus = string.Empty;
                var status = string.Empty;
                DateTime? delitime = null;
                string msg = string.Empty;
                var message = string.Empty;

                Dictionary<string, string> val = new Dictionary<string, string>();

                string result = sms;

                //var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
                var remoteIpAddress = "";
                //string netip = remoteIpAddress.ToString();

                string strHostName = "";
                strHostName = System.Net.Dns.GetHostName();

                IPHostEntry ipEntry = await System.Net.Dns.GetHostEntryAsync(strHostName);

                IPAddress[] addr = ipEntry.AddressList;

                remoteIpAddress = addr[addr.Length - 1].ToString();

                string hostName = Dns.GetHostName();
                var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                //  string myIP1 = ip_list.ToString();
                string myIP1 = addr[addr.Length - 2].ToString();

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

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                List<Institution> insdeta = new List<Institution>();
                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();
                    //PHNO="695874215";
                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    //var obj = JsonConvert.DeserializeObject<RootObject>(myContent);
                    //var messsge_id = obj.ID;

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);

                    string messageid = responsedata;

                    if (messageid.Contains("="))
                    {
                        var query = HttpUtility.ParseQueryString(url);

                        var var2 = query.Get("workingkey");
                        string[] name = messageid.Split("=");
                        msg = name[2];
                        var url1 = "https://api-alerts.solutionsinfini.com/v4/?api_key=" + var2 + "&method=sms.status&id=" + msg + "";
                        System.Net.HttpWebRequest request1 = System.Net.WebRequest.Create(url1) as HttpWebRequest;
                        System.Net.HttpWebResponse response1 = await request1.GetResponseAsync() as System.Net.HttpWebResponse;
                        Stream stream1 = response1.GetResponseStream();

                        StreamReader readStream1 = new StreamReader(stream1, Encoding.UTF8);
                        string responseparameters1 = readStream1.ReadToEnd();
                        var myContent1 = JsonConvert.SerializeObject(responseparameters1);

                        dynamic responsedata1 = JsonConvert.DeserializeObject(myContent1);
                        var statusdetails = JsonConvert.DeserializeObject<stausdata>(responsedata1);

                        if (statusdetails.data.Length > 0)
                        {
                            status = statusdetails.data[0].status;
                            if (statusdetails.data[0].dlrtime != null)
                            {
                                delitime = Convert.ToDateTime(statusdetails.data[0].dlrtime);


                            }
                            mainstatus = statusdetails.status;
                            message = statusdetails.message;
                        }
                        else
                        {
                            status = message = statusdetails.message;
                        }



                    }
                    else
                    {
                        status = messageid;
                    }

                    bool flag = true;


                    // long transactionid = 0;


                    //  string studentempflag = true;
                    Boolean scheduleflag = false;

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        //var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        //var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        //var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_SMS_Outgoing_Table_update_approval";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
                        });
                        cmd.Parameters.Add(new SqlParameter("@SSD_HeaderName",
                         SqlDbType.NVarChar)
                        {
                            Value = Template
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                     SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@SSD_TransactionId",
                        SqlDbType.BigInt)
                        {
                            Value = trnsno
                        });
                        cmd.Parameters.Add(new SqlParameter("@SSD_ToFlag",
                       SqlDbType.VarChar)
                        {
                            Value = flag
                        });
                        cmd.Parameters.Add(new SqlParameter("@SSD_SystemIP",
                 SqlDbType.VarChar)
                        {
                            Value = remoteIpAddress
                        });

                        cmd.Parameters.Add(new SqlParameter("@SSD_NetworkIP",
                 SqlDbType.VarChar)
                        {
                            Value = myIP1
                        });

                        cmd.Parameters.Add(new SqlParameter("@SSD_MAACAddress",
                 SqlDbType.VarChar)
                        {
                            Value = sMacAddress
                        });

                        cmd.Parameters.Add(new SqlParameter("@SSD_SchedulerFlag",
                 SqlDbType.Bit)
                        {
                            Value = flag
                        });

                        cmd.Parameters.Add(new SqlParameter("@mobileno",
              SqlDbType.BigInt)
                        {
                            Value = mobileNo
                        });
                        cmd.Parameters.Add(new SqlParameter("@senderid",
             SqlDbType.BigInt)
                        {
                            Value = senderid
                        });
                        cmd.Parameters.Add(new SqlParameter("@Messgstatus",
             SqlDbType.VarChar)
                        {
                            Value = status
                        });
                        cmd.Parameters.Add(new SqlParameter("@SSDNO_SMSStatusId",
            SqlDbType.VarChar)
                        {
                            Value = msg
                        });

                        cmd.Parameters.Add(new SqlParameter("@SSDNO_Id",
                    SqlDbType.BigInt)
                        {
                            Value = SSDNO_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@SSD_Id",
                    SqlDbType.BigInt)
                        {
                            Value = SSD_Id
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
            return "Success";
        }
        public async Task<string> ResendsendSmsnewwithouttemplete_newtable(long MI_Id, long mobileNo, string sms, long trnsno, long senderid, long SSDNO_Id)
        {
            try
            {
                var msgid = string.Empty;
                var mainstatus = string.Empty;
                var status = string.Empty;
                DateTime? delitime = null;
                string msg = string.Empty;
                var message = string.Empty;

                Dictionary<string, string> val = new Dictionary<string, string>();

                string result = sms;

                //var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
                var remoteIpAddress = "";
                //string netip = remoteIpAddress.ToString();

                string strHostName = "";
                strHostName = System.Net.Dns.GetHostName();

                IPHostEntry ipEntry = await System.Net.Dns.GetHostEntryAsync(strHostName);

                IPAddress[] addr = ipEntry.AddressList;

                remoteIpAddress = addr[addr.Length - 1].ToString();



                string hostName = Dns.GetHostName();
                var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                //  string myIP1 = ip_list.ToString();
                string myIP1 = addr[addr.Length - 2].ToString();

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

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    //var obj = JsonConvert.DeserializeObject<RootObject>(myContent);
                    //var messsge_id = obj.ID;

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);

                    string messageid = responsedata;

                    if (messageid.Contains("="))
                    {
                        var query = HttpUtility.ParseQueryString(url);

                        var var2 = query.Get("workingkey");
                        string[] name = messageid.Split("=");
                        msg = name[2];
                        var url1 = "https://api-alerts.solutionsinfini.com/v4/?api_key=" + var2 + "&method=sms.status&id=" + msg + "";
                        System.Net.HttpWebRequest request1 = System.Net.WebRequest.Create(url1) as HttpWebRequest;
                        System.Net.HttpWebResponse response1 = await request1.GetResponseAsync() as System.Net.HttpWebResponse;
                        Stream stream1 = response1.GetResponseStream();

                        StreamReader readStream1 = new StreamReader(stream1, Encoding.UTF8);
                        string responseparameters1 = readStream1.ReadToEnd();
                        var myContent1 = JsonConvert.SerializeObject(responseparameters1);

                        dynamic responsedata1 = JsonConvert.DeserializeObject(myContent1);
                        var statusdetails = JsonConvert.DeserializeObject<stausdata>(responsedata1);

                        if (statusdetails.data.Length > 0)
                        {
                            status = statusdetails.data[0].status;
                            if (statusdetails.data[0].dlrtime != null)
                            {
                                delitime = Convert.ToDateTime(statusdetails.data[0].dlrtime);


                            }


                            mainstatus = statusdetails.status;
                            message = statusdetails.message;
                        }
                        else
                        {
                            status = message = statusdetails.message;
                        }



                    }
                    else
                    {
                        status = messageid;
                    }

                    bool flag = true;


                    // long transactionid = 0;


                    //  string studentempflag = true;
                    Boolean scheduleflag = false;

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRM_SMS_Outgoing_Table_Resend";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@SSDNO_Id",
                           SqlDbType.BigInt)
                        {
                            Value = SSDNO_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@MessStatusId",
             SqlDbType.VarChar)
                        {
                            Value = msg
                        });



                        if (delitime == null)
                        {
                            cmd.Parameters.Add(new SqlParameter("@deliverytime",
                     SqlDbType.DateTime)
                            {
                                Value = DBNull.Value
                            });
                        }
                        else
                        {
                            cmd.Parameters.Add(new SqlParameter("@deliverytime",
                SqlDbType.DateTime)
                            {
                                Value = delitime
                            });
                        }

                        cmd.Parameters.Add(new SqlParameter("@Messgstatus",
                 SqlDbType.VarChar)
                        {
                            Value = status
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
            return "Success";
        }
        ///end
        public async Task<string> sendSmsnewTemplet_new(long MI_Id, long mobileNo, string Template, long UserID, long trnsno, string studentempflag, long senderid)
        {
            try
            {
                var msgid = string.Empty;
                var mainstatus = string.Empty;
                var status = string.Empty;
                DateTime? delitime = null;
                string msg = string.Empty;
                var message = string.Empty;

                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = sms;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, senderid.ToString());
                    sms = result;
                }
                else if (Template == "APPROVALOTP")
                {
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, senderid.ToString()).Replace(ParamaetersName[1].ISMP_NAME, trnsno.ToString());
                }

                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "SMSMAILPARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@template",
                           SqlDbType.VarChar)
                        {
                            Value = Template
                        });


                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }

                    }

                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                sms = result;
                            }
                        }
                    }

                    sms = result;
                }

                //var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
                var remoteIpAddress = "";
                //string netip = remoteIpAddress.ToString();

                string strHostName = "";
                strHostName = System.Net.Dns.GetHostName();

                IPHostEntry ipEntry = await System.Net.Dns.GetHostEntryAsync(strHostName);

                IPAddress[] addr = ipEntry.AddressList;

                remoteIpAddress = addr[addr.Length - 1].ToString();



                string hostName = Dns.GetHostName();
                var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                //  string myIP1 = ip_list.ToString();
                string myIP1 = addr[addr.Length - 2].ToString();

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

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                List<Institution> insdeta = new List<Institution>();
                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();


                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());

                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);


                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;

                    if (messageid.Contains("="))
                    {
                        var query = HttpUtility.ParseQueryString(url);

                        var var2 = query.Get("workingkey");
                        string[] name = messageid.Split("=");
                        msg = name[2];
                        var url1 = "https://api-alerts.solutionsinfini.com/v4/?api_key=" + var2 + "&method=sms.status&id=" + msg + "";
                        System.Net.HttpWebRequest request1 = System.Net.WebRequest.Create(url1) as HttpWebRequest;
                        System.Net.HttpWebResponse response1 = await request1.GetResponseAsync() as System.Net.HttpWebResponse;
                        Stream stream1 = response1.GetResponseStream();

                        StreamReader readStream1 = new StreamReader(stream1, Encoding.UTF8);
                        string responseparameters1 = readStream1.ReadToEnd();
                        var myContent1 = JsonConvert.SerializeObject(responseparameters1);

                        dynamic responsedata1 = JsonConvert.DeserializeObject(myContent1);
                        var statusdetails = JsonConvert.DeserializeObject<stausdata>(responsedata1);

                        if (statusdetails.data.Length > 0)
                        {
                            status = statusdetails.data[0].status;
                            if (statusdetails.data[0].dlrtime != null)
                            {
                                delitime = Convert.ToDateTime(statusdetails.data[0].dlrtime);


                            }
                            mainstatus = statusdetails.status;
                            message = statusdetails.message;
                        }
                        else
                        {
                            status = message = statusdetails.message;
                        }
                    }

                    else
                    {
                        status = messageid;
                    }

                    bool flag = true;


                    //  string studentempflag = true;
                    Boolean scheduleflag = false;

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_SMS_Outgoing_Table_Insert";
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
                        });
                        cmd.Parameters.Add(new SqlParameter("@SSD_HeaderName",
                         SqlDbType.NVarChar)
                        {
                            Value = Template
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                     SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@SSD_TransactionId",
                        SqlDbType.BigInt)
                        {
                            Value = trnsno
                        });
                        cmd.Parameters.Add(new SqlParameter("@SSD_ToFlag",
                       SqlDbType.VarChar)
                        {
                            Value = flag
                        });
                        cmd.Parameters.Add(new SqlParameter("@SSD_SystemIP",
                 SqlDbType.VarChar)
                        {
                            Value = remoteIpAddress
                        });

                        cmd.Parameters.Add(new SqlParameter("@SSD_NetworkIP",
                 SqlDbType.VarChar)
                        {
                            Value = myIP1
                        });

                        cmd.Parameters.Add(new SqlParameter("@SSD_MAACAddress",
                 SqlDbType.VarChar)
                        {
                            Value = sMacAddress
                        });

                        cmd.Parameters.Add(new SqlParameter("@SSD_SchedulerFlag",
                 SqlDbType.Bit)
                        {
                            Value = flag
                        });

                        cmd.Parameters.Add(new SqlParameter("@mobileno",
              SqlDbType.BigInt)
                        {
                            Value = mobileNo
                        });
                        cmd.Parameters.Add(new SqlParameter("@senderid",
             SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@Messgstatus",
             SqlDbType.VarChar)
                        {
                            Value = status
                        });
                        cmd.Parameters.Add(new SqlParameter("@SSDNO_SMSStatusId",
            SqlDbType.VarChar)
                        {
                            Value = msg
                        });
                        cmd.Parameters.Add(new SqlParameter("@APP_STATUS",
            SqlDbType.VarChar)
                        {
                            Value = ""
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
        // Potal Certificate Request SMS 
        public async Task<string> sendCertificateSms(long MI_Id, long mobileNo, string Template, long UserID, long HRME_Id, long ASMAY_Id, long ASCA_Id)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = sms;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                    sms = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "PORTAL_SMSMAILPARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                           SqlDbType.BigInt)
                        {
                            Value = ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@employeeID",
                         SqlDbType.BigInt)
                        {
                            Value = HRME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASCA_Id",
                        SqlDbType.BigInt)
                        {
                            Value = ASCA_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@template",
                           SqlDbType.VarChar)
                        {
                            Value = Template
                        });


                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }

                    }

                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                sms = result;
                            }
                        }
                    }

                    sms = result;
                }




                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                List<Institution> insdeta = new List<Institution>();
                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();


                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());

                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);


                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;


                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                            SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
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

                        cmd.Parameters.Add(new SqlParameter("@status",
                   SqlDbType.VarChar)
                        {
                            Value = "Delivered"
                        });

                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                SqlDbType.VarChar)
                        {
                            Value = messageid
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
        //Inventory PI to Supplier SMS Notification
        public async Task<string> sendPItoSupplierSms(long MI_Id, long mobileNo, string Template, long UserID, long INVMS_Id, long INVMPI_Id)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = sms;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                    sms = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "INVPI_SMSMAILPARAMETER_NEW";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
                        });

                        cmd.Parameters.Add(new SqlParameter("@INVMS_Id",
                         SqlDbType.BigInt)
                        {
                            Value = INVMS_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@INVMPI_Id",
                        SqlDbType.BigInt)
                        {
                            Value = INVMPI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@template",
                           SqlDbType.VarChar)
                        {
                            Value = Template
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }

                    }

                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                sms = result;
                            }
                        }
                    }

                    sms = result;
                }

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                List<Institution> insdeta = new List<Institution>();
                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();


                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());

                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);


                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;


                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                            SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
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

                        cmd.Parameters.Add(new SqlParameter("@status",
                   SqlDbType.VarChar)
                        {
                            Value = "Delivered"
                        });

                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                SqlDbType.VarChar)
                        {
                            Value = messageid
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
        public async Task<string> sendSms_library(long MI_Id, long mobileNo, string Template, long asmay_id, long amst_id, long book_id)
        {

            try
            {

                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = sms;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, asmay_id.ToString());
                    sms = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {


                        cmd.CommandText = "LIB_SMSMAILPARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                          SqlDbType.BigInt)
                        {
                            Value = amst_id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                          SqlDbType.BigInt)
                        {
                            Value = asmay_id
                        });
                        cmd.Parameters.Add(new SqlParameter("@LMBANO_Id",
                          SqlDbType.BigInt)
                        {
                            Value = book_id
                        });

                        cmd.Parameters.Add(new SqlParameter("@TEMPLATE",
                           SqlDbType.VarChar)
                        {
                            Value = Template
                        });


                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }

                    }

                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                sms = result;
                            }
                        }
                    }

                    sms = result;
                }


                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                List<Institution> insdeta = new List<Institution>();
                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();


                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());

                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);


                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;




                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                            SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
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

                        cmd.Parameters.Add(new SqlParameter("@status",
                   SqlDbType.VarChar)
                        {
                            Value = "Delivered"
                        });

                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                SqlDbType.VarChar)
                        {
                            Value = messageid
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
        // ************************ SALES SMS 
        public async Task<string> sendSalesSms(long MI_Id, long? mobileNo, string Template, long SalePrice, string SaleDate)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                //var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();

                //if (template.Count == 0)
                //{
                //    return "SMS Template not Mapped to the selected Institution";
                //}
                //var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                //var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                //var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();
                //string sms = template.FirstOrDefault().ISES_SMSMessage;
                //string result = sms;
                string sms = Template;
                string result = Template;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    // result = sms.Replace(ParamaetersName[0].ISMP_NAME, MI_Id.ToString());
                    sms = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "INV_SMS_Sales";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                            SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@SalePrice",
                           SqlDbType.VarChar)
                        {
                            Value = SalePrice
                        });
                        cmd.Parameters.Add(new SqlParameter("@SaleDate",
                         SqlDbType.VarChar)
                        {
                            Value = SaleDate
                        });
                        cmd.Parameters.Add(new SqlParameter("@template",
                           SqlDbType.VarChar)
                        {
                            Value = Template
                        });
                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();
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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }

                    }

                    //for (int j = 0; j < ParamaetersName.Count; j++)
                    //{
                    //    for (int p = 0; p < val.Count; p++)
                    //    {
                    //        if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                    //        {
                    //            result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                    //            sms = result;
                    //        }
                    //    }
                    //}

                    sms = result;
                }

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;


                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                            SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
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

                        cmd.Parameters.Add(new SqlParameter("@status",
                   SqlDbType.VarChar)
                        {
                            Value = "Delivered"
                        });

                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                SqlDbType.VarChar)
                        {
                            Value = messageid
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
        public async Task<string> sendSms_TRNotApplied(long MI_Id, long mobileNo, string Template, string message, long amst_id)
        {

            try
            {

                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = sms;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, amst_id.ToString());
                    sms = result;
                }
                else if (Template == "TRNOTAPPLIED")
                {
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, message.ToString());
                    sms = result;
                }
                //else
                //{
                //    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                //    {


                //        cmd.CommandText = "LIB_SMSMAILPARAMETER";
                //        cmd.CommandType = CommandType.StoredProcedure;
                //        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                //            SqlDbType.BigInt)
                //        {
                //            Value = MI_Id
                //        });
                //        cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                //          SqlDbType.BigInt)
                //        {
                //            Value = amst_id
                //        });
                //        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                //          SqlDbType.BigInt)
                //        {
                //            Value = asmay_id
                //        });
                //        cmd.Parameters.Add(new SqlParameter("@LMBANO_Id",
                //          SqlDbType.BigInt)
                //        {
                //            Value = book_id
                //        });

                //        cmd.Parameters.Add(new SqlParameter("@TEMPLATE",
                //           SqlDbType.VarChar)
                //        {
                //            Value = Template
                //        });


                //        if (cmd.Connection.State != ConnectionState.Open)
                //            cmd.Connection.Open();

                //        try
                //        {
                //            using (var dataReader = cmd.ExecuteReader())
                //            {
                //                while (dataReader.Read())
                //                {
                //                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                    {
                //                        dataRow.Add(
                //                            dataReader.GetName(iFiled),
                //                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                //                        );
                //                        var datatype = dataReader.GetFieldType(iFiled);
                //                        if (datatype.Name == "DateTime")
                //                        {
                //                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                //                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                //                        }
                //                        else
                //                        {
                //                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                //                        }
                //                    }
                //                }

                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //            return ex.Message;
                //        }

                //    }

                //    for (int j = 0; j < ParamaetersName.Count; j++)
                //    {
                //        for (int p = 0; p < val.Count; p++)
                //        {
                //            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                //            {
                //                result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                //                sms = result;
                //            }
                //        }
                //    }

                //    sms = result;
                //}


                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();


                List<Institution> insdeta = new List<Institution>();
                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());

                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);


                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;




                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                            SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
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

                        cmd.Parameters.Add(new SqlParameter("@status",
                   SqlDbType.VarChar)
                        {
                            Value = "Delivered"
                        });

                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                SqlDbType.VarChar)
                        {
                            Value = messageid
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
        public async Task<string> SendSMSForMobile(long MI_Id, long mobileNo, string Template, long OTP, long UserID, long AMST_Id)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();
                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();
                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }
                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = sms;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (variables.Count == 0)
                {
                    sms = template.FirstOrDefault().ISES_SMSMessage + Environment.NewLine + template.FirstOrDefault().ISES_MailFooter + Environment.NewLine;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "STUDENT_GATEPASS_SMSMAILPARAMETER";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id",
                           SqlDbType.BigInt)
                        {
                            Value = AMST_Id
                        });
                        //cmd.Parameters.Add(new SqlParameter("@template",
                        //   SqlDbType.VarChar)
                        //{
                        //    Value = Template
                        //});
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
                                while (dataReader.Read())
                                {
                                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                    {
                                        dataRow.Add(
                                            dataReader.GetName(iFiled),
                                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled]
                                        );
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }
                    }
                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                sms = result;
                            }
                        }
                    }

                    sms = result;
                }

                #region

                //if (Template == "MOBILEOTP")
                //{
                //    result = sms.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                //    sms = result;
                //}
                //else
                //{
                //    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                //    {
                //        cmd.CommandText = "SMSMAILPARAMETER";
                //        cmd.CommandType = CommandType.StoredProcedure;
                //        cmd.Parameters.Add(new SqlParameter("@UserID",
                //            SqlDbType.BigInt)
                //        {
                //            Value = UserID
                //        });
                //        cmd.Parameters.Add(new SqlParameter("@template",
                //           SqlDbType.VarChar)
                //        {
                //            Value = Template
                //        });


                //        if (cmd.Connection.State != ConnectionState.Open)
                //            cmd.Connection.Open();

                //        try
                //        {
                //            using (var dataReader = cmd.ExecuteReader())
                //            {
                //                while (dataReader.Read())
                //                {
                //                    var dataRow = new ExpandoObject() as IDictionary<string, object>;
                //                    for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                //                    {
                //                        dataRow.Add(
                //                            dataReader.GetName(iFiled),
                //                            dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                //                        );
                //                        var datatype = dataReader.GetFieldType(iFiled);
                //                        if (datatype.Name == "DateTime")
                //                        {
                //                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                //                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                //                        }
                //                        else
                //                        {
                //                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                //                        }
                //                    }
                //                }

                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //            return ex.Message;
                //        }

                //    }

                //    for (int j = 0; j < ParamaetersName.Count; j++)
                //    {
                //        for (int p = 0; p < val.Count; p++)
                //        {
                //            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                //            {
                //                result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                //                sms = result;
                //            }
                //        }
                //    }

                //    sms = result;
                //}

                #endregion

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();


                List<Institution> insdeta = new List<Institution>();
                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());

                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;

                    var remoteIpAddress = "";
                    string strHostName = "";
                    strHostName = System.Net.Dns.GetHostName();
                    IPHostEntry ipEntry = await System.Net.Dns.GetHostEntryAsync(strHostName);
                    IPAddress[] addr = ipEntry.AddressList;
                    remoteIpAddress = addr[addr.Length - 1].ToString();

                    string hostName = Dns.GetHostName();
                    var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                    string myIP1 = addr[addr.Length - 2].ToString();

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

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_SMS_Outgoing_StudentGatePass";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                            SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = "Visitors Management"
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@status",
                   SqlDbType.VarChar)
                        {
                            Value = "Delivered"
                        });

                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                SqlDbType.VarChar)
                        {
                            Value = messageid
                        });
                        cmd.Parameters.Add(new SqlParameter("@To_Flag",
              SqlDbType.VarChar)
                        {
                            Value = "StudentGatePass"
                        });
                        cmd.Parameters.Add(new SqlParameter("@System_Ip",
                   SqlDbType.VarChar)
                        {
                            Value = remoteIpAddress
                        });

                        cmd.Parameters.Add(new SqlParameter("@network_Ip",
                SqlDbType.VarChar)
                        {
                            Value = sMacAddress
                        });
                        cmd.Parameters.Add(new SqlParameter("@MacAddress_Ip",
              SqlDbType.VarChar)
                        {
                            Value = myIP1
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
        public async Task<string> sendsmstopperlist(long MI_Id, long mobileNo, string Template, long UserID, long ASMAY_Id, long EME_Id, long ISMS_Id)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = sms;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                    sms = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "SMSMAILPARAMETER_TOPPERLIST_EXAM";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID", SqlDbType.VarChar)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@template", SqlDbType.VarChar)
                        {
                            Value = Template
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar)
                        {
                            Value = ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@EME_Id", SqlDbType.VarChar)
                        {
                            Value = EME_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar)
                        {
                            Value = ISMS_Id
                        });


                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }

                    }

                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                sms = result;
                            }
                        }
                    }

                    sms = result;
                }


                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                List<Institution> insdeta = new List<Institution>();
                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());

                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                            SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
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

                        cmd.Parameters.Add(new SqlParameter("@status",
                   SqlDbType.VarChar)
                        {
                            Value = "Delivered"
                        });

                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                SqlDbType.VarChar)
                        {
                            Value = messageid
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

        public async Task<string> SendSMSExamMarksPublish(long MI_Id, long mobileNo, string Template, long UserID, string ExamName, long ASMAY_Id, string StudentName)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = sms;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                    sms = result;
                }
                else
                {
                    sms = sms.Replace("[NAME]", StudentName);
                    sms = sms.Replace("[EXAM]", ExamName);
                }

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                List<Institution> insdeta = new List<Institution>();
                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());

                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo", SqlDbType.NVarChar) { Value = PHNO });
                        cmd.Parameters.Add(new SqlParameter("@Message", SqlDbType.NVarChar) { Value = sms });
                        cmd.Parameters.Add(new SqlParameter("@module", SqlDbType.VarChar) { Value = modulename[0] });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@status", SqlDbType.VarChar) { Value = "Delivered" });
                        cmd.Parameters.Add(new SqlParameter("@Message_id", SqlDbType.VarChar) { Value = messageid });
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

        // Illness Student SMS
        public async Task<string> Send_Student_Illness_SMS(long MI_Id, long mobileNo, string Template, long HMTILL_Id, long AMST_Id)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }


                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = sms;

                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, HMTILL_Id.ToString());
                    sms = result;
                }
                else
                {
                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {


                        cmd.CommandText = "SMSMAILPARAMETER_STUDENT_ILLNESS";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@HMTILL_Id", SqlDbType.BigInt) { Value = HMTILL_Id });
                        cmd.Parameters.Add(new SqlParameter("@template", SqlDbType.VarChar) { Value = Template });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.BigInt) { Value = MI_Id });
                        cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.BigInt) { Value = AMST_Id });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }

                    }

                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                sms = result;
                            }
                        }
                    }

                    sms = result;
                }

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                List<Institution> insdeta = new List<Institution>();
                insdeta = _db.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());

                    url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                    string messageid = responsedata;

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                            SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
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

                        cmd.Parameters.Add(new SqlParameter("@status",
                   SqlDbType.VarChar)
                        {
                            Value = "Delivered"
                        });

                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                SqlDbType.VarChar)
                        {
                            Value = messageid
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

        public async Task<string> sendSmsApprove1(long MI_Id, string mobileNo, long UserID, string modulename, string sms)
        {
            try
            {
                var msgid = string.Empty;
                var mainstatus = string.Empty;
                var status = string.Empty;
                DateTime? delitime = null;
                string msg = string.Empty;
                var message = string.Empty;

                Dictionary<string, string> val = new Dictionary<string, string>();

                string result = sms;

                //var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
                var remoteIpAddress = "";
                //string netip = remoteIpAddress.ToString();

                string strHostName = "";
                strHostName = System.Net.Dns.GetHostName();

                IPHostEntry ipEntry = await System.Net.Dns.GetHostEntryAsync(strHostName);

                IPAddress[] addr = ipEntry.AddressList;

                remoteIpAddress = addr[addr.Length - 1].ToString();


                string hostName = Dns.GetHostName();
                var ip_list = Dns.GetHostAddressesAsync(hostName).Result[1].ToString();
                //  string myIP1 = ip_list.ToString();
                string myIP1 = addr[addr.Length - 2].ToString();

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

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();
                    //PHNO="695874215";
                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();
                    var myContent = JsonConvert.SerializeObject(responseparameters);

                    //var obj = JsonConvert.DeserializeObject<RootObject>(myContent);
                    //var messsge_id = obj.ID;

                    dynamic responsedata = JsonConvert.DeserializeObject(myContent);

                    string messageid = responsedata;

                    if (messageid.Contains("="))
                    {
                        var query = HttpUtility.ParseQueryString(url);

                        var var2 = query.Get("workingkey");
                        string[] name = messageid.Split("=");
                        msg = name[2];
                        var url1 = "https://api-alerts.solutionsinfini.com/v4/?api_key=" + var2 + "&method=sms.status&id=" + msg + "";
                        System.Net.HttpWebRequest request1 = System.Net.WebRequest.Create(url1) as HttpWebRequest;
                        System.Net.HttpWebResponse response1 = await request1.GetResponseAsync() as System.Net.HttpWebResponse;
                        Stream stream1 = response1.GetResponseStream();

                        StreamReader readStream1 = new StreamReader(stream1, Encoding.UTF8);
                        string responseparameters1 = readStream1.ReadToEnd();
                        var myContent1 = JsonConvert.SerializeObject(responseparameters1);

                        dynamic responsedata1 = JsonConvert.DeserializeObject(myContent1);
                        var statusdetails = JsonConvert.DeserializeObject<stausdata>(responsedata1);

                        if (statusdetails.data.Length > 0)
                        {
                            status = statusdetails.data[0].status;
                            if (statusdetails.data[0].dlrtime != null)
                            {
                                delitime = Convert.ToDateTime(statusdetails.data[0].dlrtime);


                            }
                            mainstatus = statusdetails.status;
                            message = statusdetails.message;
                        }
                        else
                        {
                            status = message = statusdetails.message;
                        }



                    }
                    else
                    {
                        status = messageid;
                    }

                    bool flag = true;


                    // long transactionid = 0;


                    //  string studentempflag = true;
                    Boolean scheduleflag = false;

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {

                        // var modulename = "InstitutionCreation";
                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                            SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@status",
                   SqlDbType.VarChar)
                        {
                            Value = responseparameters
                        });

                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                SqlDbType.VarChar)
                        {
                            Value = 0
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
            return "Success";
        }
        public async Task<string> sendSmsApprove(long MI_Id, long mobileNo, long UserID, string modulename, string sms)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);

                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                            SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@status",
                   SqlDbType.VarChar)
                        {
                            Value = responseparameters
                        });

                        cmd.Parameters.Add(new SqlParameter("@Message_id",
                SqlDbType.VarChar)
                        {
                            Value = 0
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
        public async Task<string> sendSmsTransport(long MI_Id, long mobileNo, long UserID, string modulename, string sms)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                if (alldetails.Count > 0)
                {
                    string url = alldetails[0].IVRMSD_URL.ToString();

                    string PHNO = mobileNo.ToString();

                    url = url.Replace("PHNO", PHNO);

                    url = url.Replace("MESSAGE", sms);


                    System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                    System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                    Stream stream = response.GetResponseStream();

                    StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                    string responseparameters = readStream.ReadToEnd();

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "IVRM_SMS_Outgoing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@MobileNo",
                            SqlDbType.NVarChar)
                        {
                            Value = PHNO
                        });
                        cmd.Parameters.Add(new SqlParameter("@Message",
                           SqlDbType.NVarChar)
                        {
                            Value = sms
                        });
                        cmd.Parameters.Add(new SqlParameter("@module",
                        SqlDbType.VarChar)
                        {
                            Value = modulename
                        });
                        cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                        {
                            Value = MI_Id
                        });

                        cmd.Parameters.Add(new SqlParameter("@status",
                   SqlDbType.VarChar)
                        {
                            Value = responseparameters
                        });

                        cmd.Parameters.Add(new SqlParameter("@Message_id", SqlDbType.VarChar)
                        {
                            Value = 0
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

        //Absent Sms Call
        public async Task<string> sendAbsentSms(long MI_Id, long mobileNo, string Template, long UserID, long isms_ids, string asmcl_ids, string asms_ids, long ASMAY_Id, DateTime frmdate, DateTime todate, long MIId)
        {
            try
            {
                Dictionary<string, string> val = new Dictionary<string, string>();

                var template = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).ToList();

                if (template.Count == 0)
                {
                    return "SMS Template not Mapped to the selected Institution";
                }

                string FromDate = frmdate.ToString("yyyy-MM-dd");
                string ToDate = todate.Date.ToString("yyyy-MM-dd");
                var institutionName = _db.Institution.Where(i => i.MI_Id == MI_Id).ToList();

                var Paramaeters = _db.SMS_MAIL_SAVED_PARAMETER_DMO.Where(i => i.MI_Id == MI_Id && i.ISES_Id == template[0].ISES_Id && i.Flag == "S").Select(d => d.ISMP_ID).ToList();

                var ParamaetersName = _db.SMS_MAIL_PARAMETER_DMO.Where(i => Paramaeters.Contains(i.ISMP_ID)).ToList();

                string sms = template.FirstOrDefault().ISES_SMSMessage;

                string result = sms;


                int valuescnt = 0;
                List<Match> variables = new List<Match>();

                foreach (Match match in Regex.Matches(sms, @"\[.*?\]"))
                {
                    variables.Add(match);
                }

                if (Template == "EMAILOTP")
                {
                    result = sms.Replace(ParamaetersName[0].ISMP_NAME, UserID.ToString());
                    sms = result;
                }
                else
                {

                  

                    using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "SMSMAILPARAMETER_ABSENTSMS1";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@UserID",
                            SqlDbType.BigInt)
                        {
                            Value = UserID
                        });
                        cmd.Parameters.Add(new SqlParameter("@template",
                           SqlDbType.VarChar)
                        {
                            Value = Template
                        });
                        cmd.Parameters.Add(new SqlParameter("@Sub_Id",
                        SqlDbType.VarChar)
                        {
                            Value = isms_ids
                        });
                        cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                       SqlDbType.BigInt)
                        {
                            Value = ASMAY_Id
                        });
                        cmd.Parameters.Add(new SqlParameter("@FromDate",
                            SqlDbType.VarChar)
                        {
                            Value = frmdate
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

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
                                        var datatype = dataReader.GetFieldType(iFiled);
                                        if (datatype.Name == "DateTime")
                                        {
                                            var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                                        }
                                        else
                                        {
                                            val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                                        }
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message;
                        }

                    }



                    //using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                    //{

                    //    cmd.CommandText = "Adm_School_SubjectwiseAttendanceSMS_multipleParameter";
                    //    cmd.CommandType = CommandType.StoredProcedure;
                    //    cmd.Parameters.Add(new SqlParameter("@MI_Id", SqlDbType.VarChar) { Value = MIId });
                    //    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id", SqlDbType.VarChar) { Value = ASMAY_Id });
                    //    cmd.Parameters.Add(new SqlParameter("@ASMCL_Id", SqlDbType.VarChar) { Value = asmcl_ids });
                    //    cmd.Parameters.Add(new SqlParameter("@ASMS_Id", SqlDbType.VarChar) { Value = asms_ids });
                    //    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.VarChar) { Value = FromDate });
                    //    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.VarChar) { Value = ToDate });
                    //    cmd.Parameters.Add(new SqlParameter("@ISMS_Id", SqlDbType.VarChar) { Value = isms_ids });
                    //    cmd.Parameters.Add(new SqlParameter("@AMST_Id", SqlDbType.VarChar) { Value = UserID });
                    //    if (cmd.Connection.State != ConnectionState.Open)
                    //        cmd.Connection.Open();


                    //    var retObject = new List<dynamic>();
                    //    try
                    //    {
                    //        using (var dataReader = cmd.ExecuteReader())
                    //        {
                    //            while (dataReader.Read())
                    //            {
                    //                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                    //                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                    //                {
                    //                    dataRow.Add(
                    //                        dataReader.GetName(iFiled),
                    //                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                    //                    );
                    //                    var datatype = dataReader.GetFieldType(iFiled);
                    //                    if (datatype.Name == "DateTime")
                    //                    {
                    //                        var dateval = (Convert.ToDateTime(dataReader[iFiled]).Date).ToString("d");
                    //                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dateval);
                    //                    }
                    //                    else
                    //                    {
                    //                        val.Add(dataReader.GetName(iFiled), dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled].ToString());
                    //                    }
                    //                }
                    //            }

                    //        }

                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        return ex.Message;
                    //    }
                    //}
                     valuescnt = val.Count;
                    for (int j = 0; j < ParamaetersName.Count; j++)
                    {
                        for (int p = 0; p < val.Count; p++)
                        {
                            if (ParamaetersName[j].ISMP_NAME.Equals(val.Keys.ToArray()[p]))
                            {
                                result = sms.Replace(ParamaetersName[j].ISMP_NAME, val.Values.ToArray()[p]);
                                sms = result;
                            }
                        }
                    }

                    sms = result;
                }

                if(valuescnt>0)
                {
                    List<SMS_DETAILS_DMO> alldetails = new List<SMS_DETAILS_DMO>();
                    alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(MI_Id)).ToList();

                    List<Institution> insdeta = new List<Institution>();
                    insdeta = _db.Institution.Where(t => t.MI_Id.Equals(MI_Id)).ToList();

                    if (alldetails.Count > 0)
                    {
                        //URL http://alerts.kaleyra.com/api/v4/?method=sms&api_key={{api_key}}&message=MESSAGE&sender={{sender}}&to=PHNO&entity_id=entityid&template_id=templateid

                        string url = alldetails[0].IVRMSD_URL.ToString();

                        string PHNO = mobileNo.ToString();

                        url = url.Replace("PHNO", PHNO);
                        url = url.Replace("MESSAGE", sms);
                        url = url.Replace("entityid", insdeta[0].MI_EntityId.ToString());
                        url = url.Replace("templateid", template.FirstOrDefault().ISES_TemplateId);

                        System.Net.HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
                        System.Net.HttpWebResponse response = await request.GetResponseAsync() as System.Net.HttpWebResponse;
                        Stream stream = response.GetResponseStream();

                        StreamReader readStream = new StreamReader(stream, Encoding.UTF8);
                        string responseparameters = readStream.ReadToEnd();
                        var myContent = JsonConvert.SerializeObject(responseparameters);

                        dynamic responsedata = JsonConvert.DeserializeObject(myContent);
                        string messageid = responsedata;


                        //try
                        //{
                        //    var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                        //    var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                        //    var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                        //    IVRM_sms_sentBoxDMO smssentbox = new IVRM_sms_sentBoxDMO();

                        //    smssentbox.MI_Id = MI_Id;
                        //    smssentbox.Mobile_no = PHNO;
                        //    smssentbox.Message = sms;
                        //    smssentbox.Datetime = DateTime.Now;

                        //    smssentbox.Message_id = messageid;
                        //    smssentbox.Module_Name = modulename[0];
                        //    smssentbox.CreatedDate = DateTime.Now;
                        //    smssentbox.UpdatedDate = DateTime.Now;

                        //    smssentbox.statusofmessage = "Deliverd";
                        //    smssentbox.To_FLag = "Student";

                        //    _db.IVRM_sms_sentBoxDMO.Add(smssentbox);
                        //    var returnmessage=_db.SaveChanges();

                        //}
                        //catch(Exception ex)
                        //{
                        //    Console.WriteLine(ex.Message);
                        //}

                        using (var cmd = _db.Database.GetDbConnection().CreateCommand())
                        {
                            var template1010 = _db.smsEmailSetting.Where(e => e.MI_Id == MI_Id && e.ISES_Template_Name == Template && e.ISES_SMSActiveFlag == true).Select(d => d.IVRMIM_Id).ToList();

                            var moduleid = _db.Institution_Module.Where(i => template1010.Contains(i.IVRMIM_Id)).Select(d => d.IVRMM_Id).ToList();

                            var modulename = _db.masterModule.Where(i => moduleid.Contains(i.IVRMM_Id)).Select(d => d.IVRMM_ModuleName).ToList();

                            cmd.CommandText = "IVRM_SMS_Outgoing";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@MobileNo",
                                SqlDbType.NVarChar)
                            {
                                Value = PHNO
                            });
                            cmd.Parameters.Add(new SqlParameter("@Message",
                               SqlDbType.NVarChar)
                            {
                                Value = sms
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

                            cmd.Parameters.Add(new SqlParameter("@status",
                       SqlDbType.VarChar)
                            {
                                Value = "Delivered"
                            });

                            cmd.Parameters.Add(new SqlParameter("@Message_id",
                    SqlDbType.VarChar)
                            {
                                Value = messageid
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
               

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
            return "success";
        }
    }
    public class stausdata
    {
        public string status { get; set; }
        public string message { get; set; }
        public data1[] data { get; set; }
    }
    public class data1
    {
        public string id { get; set; }
        public string mobile { get; set; }
        public string status { get; set; }
        public string senttime { get; set; }
        public DateTime? dlrtime { get; set; }
        public string custom { get; set; }
        public string custom1 { get; set; }
        public string custom2 { get; set; }
        public string provider { get; set; }
        public string location { get; set; }
    }
    public class hrme_mobile_email
    {
        public long mobile { get; set; }
        public string email { get; set; }
    }
}
