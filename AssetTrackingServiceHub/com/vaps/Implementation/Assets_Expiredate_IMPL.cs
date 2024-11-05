using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.AssetTracking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs.com.vaps.AssetTracking;
using PreadmissionDTOs.com.vaps.AssetTracking.AssetTag;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AssetTrackingServiceHub.com.vaps.Implementation
{
    public class Assets_Expiredate_IMPL : Interface.Assets_Expiredate_Interface
    {
        public AssetTrackingContext _ATContext;
        private readonly DomainModelMsSqlServerContext _db;

        public Assets_Expiredate_IMPL(AssetTrackingContext ATContext, DomainModelMsSqlServerContext db)
        {
            _ATContext = ATContext;
            _db = db;

        }
        public Asset_Expiredate_DTO get_expdata(Asset_Expiredate_DTO dto)
        {
            try
            {
                var key = _ATContext.MobileApplAuthenticationDMO.Single(a => a.MI_Id == dto.MI_Id).MAAN_AuthenticationKey;
                var date_count = _ATContext.INV_ConfigurationDMO.Where(a => a.MI_Id == dto.MI_Id).ToList().FirstOrDefault();
                DateTime dt = DateTime.Now;
                DateTime today = dt.Date;
                DateTime addate = today.AddDays(date_count.INVC_AlertsBeforeDays);
                var getitem = (from a in _ATContext.INV_Asset_AssetTagDMO 
                               from b in _ATContext.INV_Master_StoreDMO
                               from c in _ATContext.INV_Master_ItemDMO
                               from d in _ATContext.MasterEmployee
                               where (a.INVMI_Id == c.INVMI_Id && a.INVMST_Id == b.INVMST_Id && a.MI_Id == dto.MI_Id && b.HRME_Id == d.HRME_Id && a.INVAAT_WarantyExpiryDate >= today &&  a.INVAAT_WarantyExpiryDate <= addate && a.INVAAT_ActiveFlg==true)
                               select new Asset_Expiredate_DTO
                               {
                                   HRME_Id = d.HRME_Id,
                                   HRME_AppDownloadedDeviceId = d.HRME_AppDownloadedDeviceId,
                                   INVAAT_WarantyExpiryDate = a.INVAAT_WarantyExpiryDate,
                                    INVMS_StoreName = b.INVMS_StoreName,
                                   INVMI_ItemName = c.INVMI_ItemName,
                                   employeename = ((d.HRME_EmployeeFirstName == null || d.HRME_EmployeeFirstName == "" ? "" : d.HRME_EmployeeFirstName)
                                                      + (d.HRME_EmployeeMiddleName == null || d.HRME_EmployeeMiddleName == "" ? "" : " " + d.HRME_EmployeeMiddleName)
                                                      + (d.HRME_EmployeeLastName == null || d.HRME_EmployeeLastName == "" ? "" : " " + d.HRME_EmployeeLastName)).Trim(),
                                   HRME_MobileNo =Convert.ToInt64(d.HRME_MobileNo)
                               }).ToList();
                foreach (var item in getitem)
                {
                    
                    callnotification(item.HRME_AppDownloadedDeviceId, item.INVMI_ItemName, item.INVAAT_WarantyExpiryDate, item.employeename, dto.MI_Id, item.HRME_MobileNo,item.HRME_Id,  key);
                }
                 
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public string callnotification(string HRME_AppDownloadedDeviceId, string INVMI_ItemName, DateTime? INVAAT_WarantyExpiryDate, string employeename, long MI_Id, long HRME_MobileNo, long HRME_Id, string key)
        {
           
            try
            {
               
                var sound = "";
                if (sound == "")
                {
                    sound = "default";
                }


                string url = "";
                    url = "https://fcm.googleapis.com/fcm/send";

                    List<string> notificationparams = new List<string>();
                    string daata = "";
                    long notId = 1;
                DateTime date = DateTime.Now;
                DateTime tdate = date.Date;
                DateTime date2 = Convert.ToDateTime(INVAAT_WarantyExpiryDate);
                var dif = date2.Subtract(tdate).Days.ToString();

                string message = "";
                if(dif=="0")
                {
                    message = "Your Item " + INVMI_ItemName + " Expired ToDay";
                }
                else
                {
                    message = "Your Item " + INVMI_ItemName + " Expired Within " + dif + " Day";
                }
                
               
               
                var deviceidsnew = "";
                var devicenew = "";
                deviceidsnew = '"' + HRME_AppDownloadedDeviceId + '"';
                 devicenew = "[" + deviceidsnew + "]";

                daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," + "" + '"' + "data" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "foreground" + '"' + ":" + '"' + true + '"' + " , " + '"' + "title" + '"' + ":" + '"' + employeename + '"' + " , " + '"' + "body" + '"' + ":" + '"' + message + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#bd3f32" + '"' + " } }";


                    notificationparams.Add(daata.ToString());
                    //var mycontent = JsonConvert.SerializeObject(notificationparams);
                    var mycontent = notificationparams[0];
                    string postdata = mycontent.ToString();
                    HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(url);
                    connection.ContentType = "application/json";
                    connection.MediaType = "application/json";
                    connection.Accept = "application/json";

                    connection.Method = "post";
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
                    }


                    PushNotification push_noti = new PushNotification(_db);

                    push_noti.Insert_PushNotification_asset_tagging(MI_Id, employeename, message, HRME_MobileNo, HRME_AppDownloadedDeviceId, HRME_Id);


               

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
