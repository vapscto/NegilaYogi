
using System.Collections.Generic;
using System.Linq;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using System.Collections.Concurrent;
using System;
using DomainModel.Model.com.vaps.Exam;
using AutoMapper;
using PreadmissionDTOs.com.vaps.Exam;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model.com.vapstech.TT;
using DomainModel.Model.com.vaps.admission;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using PreadmissionDTOs.com.vaps.Portals.Principal;
using Microsoft.AspNetCore.Hosting;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;
using DomainModel.Model.com.vapstech.HRMS;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Net.NetworkInformation;
using Newtonsoft.Json;

namespace PortalHub.com.vaps.Principal.Services
{
    public class PushNotifyImpl : Interfaces.PushNotifyInterface
    {
        int MI_ID = 0;
        private readonly IHostingEnvironment _hostingEnvironment;

        private static ConcurrentDictionary<string, PushNotifyDTO> _login =
         new ConcurrentDictionary<string, PushNotifyDTO>();

        private readonly PortalContext _PrincipalDashboardContext;
        ILogger<SendSMSImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public PushNotifyImpl(PortalContext cpContext, DomainModelMsSqlServerContext db, IHostingEnvironment hostingEnvironment)
        {
            _PrincipalDashboardContext = cpContext;
            _db = db;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<PushNotifyDTO> Getdetails(PushNotifyDTO data)//int IVRMM_Id
        {
            {
                try
                {
                    var list = await _PrincipalDashboardContext.AcademicYearDMO.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(e=>e.ASMAY_Order).ToListAsync();//AcademicYear
                    data.yearlist = list.ToArray();

                    var currYear = await _PrincipalDashboardContext.AcademicYearDMO.Where(d => d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id).OrderByDescending(e => e.ASMAY_Order).ToListAsync();//AcademicYear
                    data.currentYear = currYear.ToArray();

                    var classlist =await _db.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag == true).ToListAsync();
                    data.classlist = classlist.ToArray();

                    var sectionlist =await _db.School_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).ToListAsync();
                    data.sectionlist = sectionlist.ToArray();
             
                    var designationdropdown =await _PrincipalDashboardContext.HR_Master_Designation.Where(t => t.MI_Id == data.MI_Id && t.HRMDES_ActiveFlag == true).ToListAsync();
                    data.designationdropdown = designationdropdown.ToArray();



                    var studentlist =await (from m in _db.Adm_M_Student
                                       from n in _db.School_Adm_Y_StudentDMO
                                       where m.AMST_Id == n.AMST_Id && m.MI_Id == data.MI_Id && n.ASMAY_Id == currYear.FirstOrDefault().ASMAY_Id && m.AMST_SOL.Equals("S") && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1 && n.ASMCL_Id == classlist.FirstOrDefault().ASMCL_Id && n.ASMS_Id == sectionlist.FirstOrDefault().ASMS_Id
                                       select new PushNotifyDTO
                                       {
                                           AMST_Id = n.AMST_Id,
                                           studentName = m.AMST_FirstName + (string.IsNullOrEmpty(m.AMST_MiddleName)  || m.AMST_MiddleName=="0" ? "" : ' ' + m.AMST_MiddleName) + (string.IsNullOrEmpty(m.AMST_LastName) || m.AMST_LastName=="0" ? "" : ' ' + m.AMST_LastName),
                                           AMST_AdmNo = m.AMST_AdmNo,
                                           AMST_emailId = m.AMST_emailId,
                                           AMST_MobileNo = m.AMST_MobileNo,
                                           AMST_AppDownloadedDeviceId = m.AMST_AppDownloadedDeviceId

                                       }).OrderBy(t=>t.studentName).ToListAsync();
                    if (studentlist.Count > 0)
                    {
                        data.studentlist = studentlist.ToArray();
                        data.studentCount = studentlist.Count;
                    }
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }

                return data;
            }

        }
        public PushNotifyDTO GetEmployeeDetailsByLeaveYearAndMonth(PushNotifyDTO data)
        {
            //List<MasterEmployee> employe = new List<MasterEmployee>();
            List<HR_Master_Employee_DMO> employe = new List<HR_Master_Employee_DMO>();

            try
            {
                employe = (from a in _PrincipalDashboardContext.HR_Master_Employee_DMO//MasterEmployee

                           where (a.MI_Id.Equals(data.MI_Id)) && a.HRME_ActiveFlag == true
                           select a).Distinct().ToList();

                if (employe.Count > 0)
                {
                    employe = employe.Where(a => a.HRME_LeftFlag == false).ToList();


                    if (data.hrmdeS_IdList.Count() > 0 && data.hrmD_IdList.Count() > 0)
                    {
                        //employee
                        employe = employe.Where(t => t.MI_Id.Equals(data.MI_Id) && data.hrmdeS_IdList.Contains(t.HRMDES_Id) && data.hrmD_IdList.Contains(t.HRMD_Id)).ToList();

                    }
                    else if (data.hrmdeS_IdList.Count() > 0 && data.hrmD_IdList.Count() > 0)
                    {
                        //employee
                        employe = employe.Where(t => t.MI_Id.Equals(data.MI_Id) && data.hrmdeS_IdList.Contains(t.HRMDES_Id) && data.hrmD_IdList.Contains(t.HRMD_Id)).ToList();
                    }
                    else if (data.hrmdeS_IdList.Count() > 0 && data.hrmD_IdList.Count() == 0)
                    {
                        //employee
                        employe = employe.Where(t => t.MI_Id.Equals(data.MI_Id) && data.hrmdeS_IdList.Contains(t.HRMDES_Id)).ToList();
                    }
                    else if (data.hrmdeS_IdList.Count() == 0 && data.hrmD_IdList.Count() > 0)
                    {
                        //employee
                        employe = employe.Where(t => t.MI_Id.Equals(data.MI_Id) && data.hrmD_IdList.Contains(t.HRMD_Id)).ToList();
                    }
                    else if (data.hrmdeS_IdList.Count() > 0 && data.hrmD_IdList.Count() == 0)
                    {
                        //employee
                        employe = employe.Where(t => t.MI_Id.Equals(data.MI_Id) && data.hrmdeS_IdList.Contains(t.HRMDES_Id)).ToList();
                    }
                    else if (data.hrmdeS_IdList.Count() == 0 && data.hrmD_IdList.Count() > 0)
                    {
                        //employee
                        employe = employe.Where(t => t.MI_Id.Equals(data.MI_Id) && data.hrmD_IdList.Contains(t.HRMD_Id)).ToList();
                    }

                    else if (data.hrmdeS_IdList.Count() == 0 && data.hrmD_IdList.Count() == 0)
                    {
                        //employee
                        employe = employe.Where(t => t.MI_Id.Equals(data.MI_Id)).ToList();
                    }
                    data.employeedropdown = employe.ToArray();
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }


        public async Task<PushNotifyDTO> savedetail(PushNotifyDTO data)
        {
            data.scnt = 0;
            data.fcnt = 0;

            var rolelist = _PrincipalDashboardContext.IVRM_Role_Type.Where(t => t.IVRMRT_Id == data.roleId).ToList();

            string rolecheck = "";


            var keylist = _PrincipalDashboardContext.MobileApplAuthenticationDMO.Where(t => t.MI_Id == data.MI_Id).Distinct().ToList();

            if (keylist.Count>0)
            {
                if (rolelist.Count > 0)
                {


                    rolecheck = rolelist.FirstOrDefault().IVRMRT_Role;


                    if (data.radiotype == "Student")
                    {


                        if (data.studentlistdto.Length > 0)
                        {
                            for (int i = 0; i < data.studentlistdto.Length; i++)
                            {

                                //data.studentlistdto[i].AMST_AppDownloadedDeviceId = "d7-btrlmxp4:APA91bEUHe6DNP9kt26XLxQVxfnsKvzi7KEvcL0uGjTHoiAz-bcEaRZyxxPc1T8tgkuUOELSxI8Jh1xqGd8RfbVZSZAJEMRjggW1ANM1Y3IaggWH8RpnAH5BcWTnYceYP7AZV_sU86Mc";


                             int e  =callnotification(data.studentlistdto[i].AMST_Id, data.studentlistdto[i].AMST_AppDownloadedDeviceId, data.studentlistdto[i].AMST_MobileNo, data.MI_Id, rolecheck, data.SmsMailText, keylist[0].MAAN_AuthenticationKey, data.radiotype);

                                if (e==1)
                                {
                                    data.scnt += 1;
                                }
                                else
                                {
                                    data.fcnt += 1;
                                }

                            }
                            

                        }
                    }

                    else if (data.radiotype == "Staff")
                    {
                        SmsWithoutTemplate sms = new SmsWithoutTemplate(_db);
                        EmailWithoutTemplate email = new EmailWithoutTemplate(_db);
                        for (int i = 0; i < data.studentlistdto.Length; i++)
                        {
                            long moble = 0;
                            if (data.studentlistdto[i].HRME_MobileNo ==null)
                            {
                                moble = 0;
                            }
                            else
                            {
                                moble = Convert.ToInt64(data.studentlistdto[i].HRME_MobileNo);
                            }


                            //data.studentlistdto[i].HRME_AppDownloadedDeviceId = "d7-btrlmxp4:APA91bEUHe6DNP9kt26XLxQVxfnsKvzi7KEvcL0uGjTHoiAz-bcEaRZyxxPc1T8tgkuUOELSxI8Jh1xqGd8RfbVZSZAJEMRjggW1ANM1Y3IaggWH8RpnAH5BcWTnYceYP7AZV_sU86Mc";
                            int e = callnotification(data.studentlistdto[i].HRME_Id, data.studentlistdto[i].HRME_AppDownloadedDeviceId, moble, data.MI_Id, rolecheck, data.SmsMailText, keylist[0].MAAN_AuthenticationKey, data.radiotype);

                            if (e == 1)
                            {
                                data.scnt += 1;
                            }
                            else
                            {
                                data.fcnt += 1;
                            }
                        }

                    }
                }
            }
            else
            {
                data.msg = "key";
            }

           

          
            return data;
        }


        public async Task<PushNotifyDTO> GetStudentDetails(PushNotifyDTO data)
        {
            try
            {
                var studentlist =await (from m in _db.Adm_M_Student
                                   from n in _db.School_Adm_Y_StudentDMO
                                   where m.AMST_Id == n.AMST_Id && m.MI_Id == data.MI_Id && n.ASMAY_Id == data.ASMAY_Id && m.AMST_SOL.Equals("S") && m.AMST_ActiveFlag == 1 && n.AMAY_ActiveFlag == 1 && n.ASMCL_Id == data.ASMCL_Id && n.ASMS_Id == data.ASMS_Id 
                                   select new PushNotifyDTO
                                   {
                                       AMST_Id = n.AMST_Id,
                                       studentName = m.AMST_FirstName + (string.IsNullOrEmpty(m.AMST_MiddleName) || m.AMST_MiddleName == "0" ? "" : ' ' + m.AMST_MiddleName) + (string.IsNullOrEmpty(m.AMST_LastName) || m.AMST_LastName == "0" ? "" : ' ' + m.AMST_LastName),
                                       AMST_AdmNo = m.AMST_AdmNo,
                                       AMST_emailId = m.AMST_emailId,
                                       AMST_MobileNo = m.AMST_MobileNo,
                                         AMST_AppDownloadedDeviceId = m.AMST_AppDownloadedDeviceId

                                   }).OrderBy(f=>f.studentName).ToListAsync();
                if (studentlist.Count > 0)
                {
                    data.studentlist = studentlist.ToArray();
                    data.studentCount = studentlist.Count;
                }

            }
            catch (Exception)
            {

                throw;
            }

            return data;
        }


        public PushNotifyDTO Getdepartment(PushNotifyDTO data)
        {
            var departmentdropdown =  _PrincipalDashboardContext.HR_Master_Department.Where(t => t.MI_Id == data.MI_Id && t.HRMD_ActiveFlag == true).ToList();
            data.departmentdropdown = departmentdropdown.ToArray();
            return data;
        }


        public PushNotifyDTO get_designation(PushNotifyDTO data)
        {
            data.designationdropdown = ( from a in _PrincipalDashboardContext.HR_Master_Employee_DMO//MasterEmployee
                                         from b in _PrincipalDashboardContext.HR_Master_Designation
                                         from c in _PrincipalDashboardContext.HR_Master_Department
                                    where (a.HRMDES_Id == b.HRMDES_Id && a.HRMD_Id == c.HRMD_Id && c.HRMD_ActiveFlag == true
                                    && b.HRMDES_ActiveFlag == true && a.HRME_ActiveFlag == true
                                    && a.MI_Id == b.MI_Id && c.MI_Id == a.MI_Id && a.MI_Id == data.MI_Id && data.multipledep.ToString().Contains(Convert.ToString(c.HRMD_Id)))
                                    select new PushNotifyDTO
                                    {
                                        HRMDES_Id = b.HRMDES_Id,
                                        HRMDES_DesignationName = b.HRMDES_DesignationName,
                                    }
                     ).Distinct().ToArray();

            return data;
        }
       
        public PushNotifyDTO get_employee(PushNotifyDTO data)
        {
            data.stafflist = (from a in _PrincipalDashboardContext.HR_Master_Employee_DMO
                              from b in _PrincipalDashboardContext.Multiple_Mobile_DMO
                              where (a.MI_Id == data.MI_Id && data.multipledes.ToString().Contains(Convert.ToString(a.HRMDES_Id))  && data.multipledep.ToString().Contains(Convert.ToString(a.HRMD_Id)) && a.HRME_ActiveFlag == true && a.HRME_LeftFlag == false  && a.HRME_Id==b.HRME_Id && b.HRMEMNO_DeFaultFlag== "default")
                                 select new PushNotifyDTO
                                 {
                                     HRME_Id = a.HRME_Id,
                                     HRME_EmployeeFirstName = a.HRME_EmployeeFirstName,
                                     HRME_EmployeeMiddleName = a.HRME_EmployeeMiddleName ?? " ",
                                     HRME_EmployeeLastName = a.HRME_EmployeeLastName ?? " ",
                                     HRME_EmployeeCode = a.HRME_EmployeeCode,
                                     HRME_AppDownloadedDeviceId = a.HRME_AppDownloadedDeviceId,
                                     HRME_MobileNo=b.HRMEMNO_MobileNo
                                 }
                     ).Distinct().OrderBy(e=>e.HRME_EmployeeFirstName).ToArray();
            return data;
        }

        

        public int callnotification(long AMST_Id, string devicenew, long mobileno, long mi_id, string headername, string text, string key,string type)
        {

            string transid = "";
            string result = "";
            int cnt = 0;
            try
            {


                var head = "NOTIFICATION FROM  " + headername.ToUpper();


               string devicenew1 = '"' + devicenew + '"';

                devicenew1 = "[" + devicenew1 + "]";

                string url = "";
                url = "https://fcm.googleapis.com/fcm/send";

                List<string> notificationparams = new List<string>();
                string daata = "";

                //daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
                // "" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + classwork.FirstOrDefault().ICW_Topic + '"' + " , " + '"' + "body" + '"' + ":" + '"' + classwork.FirstOrDefault().ICW_Content + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";


                string sound = "default";
                //string notId = "4";
                //daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
                // "" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + classwork.FirstOrDefault().ICW_Topic + '"' + " , " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' +
                // +'"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , "
                // + '"' + "body" + '"' + ":" + '"' + classwork.FirstOrDefault().ICW_Content + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";

                daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew1 + "," +
                "" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + head + '"' + " , " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "body" + '"' + ":" + '"' + text + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";

                notificationparams.Add(daata.ToString());

                // var mycontent = JsonConvert.SerializeObject(notificationparams);
                var mycontent = notificationparams[0];
                string postdata = mycontent.ToString();
                HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(url);
                connection.ContentType = "application/json";
                connection.MediaType = "application/json";
                connection.Accept = "application/json";

                connection.Method = "post";
                //connection.Headers["authorization"] = "key=AAAAvDDD0Rs:APA91bEFpdVjbc7oDFoFR2LIagSffKZmmu17NowfggiE752rEo45Hgl1kNX2_AWVxHqBcAUJOTvo5CApdlHNwNFHKBjIFqhVEwiQC9PVYdba_SRCAHC2tMVTVzV2WBaWcKIGGwAOGo4I";

                connection.Headers["authorization"] = "key="+ key;

                using (StreamWriter requestwriter = new StreamWriter(connection.GetRequestStream()))
                {
                    requestwriter.Write(postdata);
                }


                string responsedata = string.Empty;



                using (StreamReader responsereader = new StreamReader(connection.GetResponse().GetResponseStream()))
                {
                    responsedata = responsereader.ReadToEnd();
                    JObject joresponse1 = JObject.Parse(responsedata);

                    transid = (string)joresponse1["multicast_id"];
                    result = (string)joresponse1["success"];


                }



                Insert_PushNotification(mi_id, mobileno, devicenew, head, text, type, AMST_Id, transid, result);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                //return ex.Message;
            }

            if (result=="0")
            {
                cnt = 0;
            }
            else if (result == "1")
            {
                cnt = 1;
            }
            else
            {
                cnt = 0;
            }

            return cnt;

        }


        public string Insert_PushNotification(long mi_id, long mobileno, string devicenew,string header, string msgdetails,string type,long appdownloaderid,string transid, string result)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                var time = indianTime.ToString("hh:mm tt");

                var remoteIpAddress = "";
                //string netip = remoteIpAddress.ToString();

                string strHostName = "";
                strHostName = System.Net.Dns.GetHostName();

                IPHostEntry ipEntry =  System.Net.Dns.GetHostEntry(strHostName);

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
                

                PN_Sent_Details_DMO sdd = new PN_Sent_Details_DMO();
                sdd.MI_Id = mi_id;
                sdd.PNSD_HeaderName = header;
                sdd.PNSD_SentDate = indianTime;
                sdd.PNSD_SentTime = time;
                sdd.PNSD_ToFlag = type;
                sdd.PNSD_SMSMessage = msgdetails;
                sdd.PNSD_OutboxFlag = true;
                sdd.PNSD_SchedulerFlag = true;
                sdd.PNSD_SystemIP = remoteIpAddress;
                sdd.PNSD_NetworkIP = myIP1;
                sdd.PNSD_MAACAddress = sMacAddress;
                sdd.PNSD_TransactionId = transid;
                sdd.PNSD_ApprovalLevel = "0";
               
                _PrincipalDashboardContext.Add(sdd);
                
               
                    PN_Sent_Details_Devicewise_DMO ss = new PN_Sent_Details_Devicewise_DMO();

                    ss.PNSD_Id = sdd.PNSD_Id;
                    ss.PNSDDE_MobileNo = mobileno;
                    ss.PNSDDE_DeviceId = devicenew;
                    ss.PNSDDE_ReadStatus = result;
                    ss.PNSDDE_DeliveryDate = indianTime;
                    ss.PNSDDE_DeliveryTime = time;
                    ss.PNSDDE_MakeUnreadFlg = false;
                    ss.PNSDDE_ApprovalLevel = "0";
                _PrincipalDashboardContext.Add(ss);


                if (type=="Student")
                {
                    PN_Sent_Details_Student_DMO sds = new PN_Sent_Details_Student_DMO();
                    sds.PNSD_Id = sdd.PNSD_Id;
                    sds.AMST_Id = appdownloaderid;
                    _PrincipalDashboardContext.Add(sds);
                }


                if (type == "Staff")
                {
                    PN_Sent_Details_Staff_DMO sds = new PN_Sent_Details_Staff_DMO();
                    sds.PNSD_Id = sdd.PNSD_Id;
                    sds.HRME_Id = appdownloaderid;
                    _PrincipalDashboardContext.Add(sds);
                }



                _PrincipalDashboardContext.SaveChanges();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "err";

            }
            return "success";
        }

    }

}
