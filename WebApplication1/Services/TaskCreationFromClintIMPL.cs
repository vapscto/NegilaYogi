using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.IssueManager;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.IssueManager;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.IssueManager;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class TaskCreationFromClintIMPL : Interfaces.TaskCreationFromClintInterface
    {


        public DomainModelMsSqlServerContext _Context;
        public TaskCreationFromClintIMPL(DomainModelMsSqlServerContext db)
        {

            _Context = db;
        }
        public async Task<TaskCreationFromClintDTO> getdetails(TaskCreationFromClintDTO data)
        {
            try
            {
                var checkuserid = _Context.IVRM_Payment_User_MappingDMO.Where(a => a.User_Id == data.UserId).ToList();
                TaskCreationFromClintDTO dd = new TaskCreationFromClintDTO();

                var geturl = _Context.IVRM_Storage_path_Details.ToList();

                if (geturl.Count > 0 && geturl.FirstOrDefault().IVRM_VMS_Subscription_URL != null && geturl.FirstOrDefault().IVRM_VMS_Subscription_URL != "")
                {
                    if (checkuserid.Count > 0)
                    {
                        var stringurl = geturl.FirstOrDefault().IVRM_VMS_Subscription_URL;

                        var getinstitutioncode = _Context.VirtualSchool.Where(a => a.IVRM_MI_Id == data.MI_Id).ToList();

                        string institutioncode = getinstitutioncode.FirstOrDefault().IVRM_Sub_Domain_Name;

                        data.ISMMCLT_ClientCode = institutioncode;
               
                        HttpClient client1 = new HttpClient();
                        client1.BaseAddress = new Uri(stringurl);
                        client1.DefaultRequestHeaders.Accept.Clear();
                        client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = client1.PostAsJsonAsync("api/ISM_TaskCreationFacade/getdetails", data).Result;

                        string description = string.Empty;
                        if (response.IsSuccessStatusCode)
                        {
                            string result = response.Content.ReadAsStringAsync().Result;
                            description = result;

                            dd = JsonConvert.DeserializeObject<TaskCreationFromClintDTO>(description, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                        }
                    }
                }
                data.get_department = dd.get_department;
                data.get_priority = dd.get_priority;
                data.get_client = dd.get_client;
                data.get_project = dd.get_project;
                data.get_taskdetails = dd.get_taskdetails;
                data.get_days = dd.get_days;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public TaskCreationFromClintDTO savedata(TaskCreationFromClintDTO data)
        {
            try
            {
                var checkuserid = _Context.IVRM_Payment_User_MappingDMO.Where(a => a.User_Id == data.UserId).ToList();

                var geturl = _Context.IVRM_Storage_path_Details.ToList();

                if (geturl.Count > 0 && geturl.FirstOrDefault().IVRM_VMS_Subscription_URL != null && geturl.FirstOrDefault().IVRM_VMS_Subscription_URL != "")
                {
                    if (checkuserid.Count > 0)
                    {
                        var stringurl = geturl.FirstOrDefault().IVRM_VMS_Subscription_URL;

                        var getinstitutioncode = _Context.VirtualSchool.Where(a => a.IVRM_MI_Id == data.MI_Id).ToList();

                        string institutioncode = getinstitutioncode.FirstOrDefault().IVRM_Sub_Domain_Name;

                        data.ISMMCLT_ClientCode = institutioncode;


                        HttpClient client1 = new HttpClient();
                        client1.BaseAddress = new Uri(stringurl);
                        client1.DefaultRequestHeaders.Accept.Clear();
                        client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = client1.PostAsJsonAsync("api/ISM_TaskCreationFacade/Savedata", data).Result;

                        string description = string.Empty;
                        if (response.IsSuccessStatusCode)
                        {
                            string result = response.Content.ReadAsStringAsync().Result;
                            description = result;

                            data = JsonConvert.DeserializeObject<TaskCreationFromClintDTO>(description, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                        }
                    }
                }
                // return dd.getpaymentnotificationdetails;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
           
        }
        public string callnotification(string devicenew, long empid, long task_id, string titletext, long mi_id, string sound, string subject)
        {
            //  try
            //  {
            //      TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            //      DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
            //      string url = "";
            //      string utrrno = "";
            //      url = "https://fcm.googleapis.com/fcm/send";

            //      List<string> notificationparams = new List<string>();
            //      string daata = "";
            //      long notId = 1;

            //      //daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," + "" + '"' + "data" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "foreground" + '"' + ":" + '"' + true + '"' + " , " + '"' + "title" + '"' + ":" + '"' + titletext + '"' + " ,  " + '"' + "body" + '"' + ":" + '"' + indianTime + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#bd3f32" + '"' + " } }";

            //      daata = "{ " + '"' + "registration_ids" + '"' + ":" + devicenew + "," +
            //"" + '"' + "notification" + '"' + " : " + "{" + '"' + "icon" + '"' + "" + ":" + '"' + "ic_notification_icon" + '"' + " , " + '"' + "title" + '"' + ":" + '"' + subject + '"' + " ,  " + '"' + "sound" + '"' + "" + ":" + '"' + sound + '"' + " , " + '"' + "notId" + '"' + "" + ":" + '"' + notId + '"' + " , " + '"' + "body" + '"' + ":" + '"' + titletext + '"' + ", " + '"' + "iconcolor" + '"' + ":" + '"' + "#692a7b" + '"' + " } }";

            //      notificationparams.Add(daata.ToString());
            //      // var mycontent = JsonConvert.SerializeObject(notificationparams);
            //      var mycontent = notificationparams[0];
            //      string postdata = mycontent.ToString();
            //      HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(url);
            //      connection.ContentType = "application/json";
            //      connection.MediaType = "application/json";
            //      connection.Accept = "application/json";

            //      connection.Method = "post";
            //      connection.Headers["authorization"] = "key=AAAADrksgbk:APA91bGjurLMMB23AWc8SklzSksUUMoFt6zA_XY2TMkk0BxzDYFIkYuKNpNlhtYdVIWiQ8zjsQxXIlGdWI-Zrqb9UHhNpJf9DMM7qtAFxxgZPWbhenI4KWsnpZaaeWtM6O2qR_vIHXqS";
            //      using (StreamWriter requestwriter = new StreamWriter(connection.GetRequestStream()))
            //      {
            //          requestwriter.Write(postdata);
            //      }
            //      string responsedata = string.Empty;

            //      using (StreamReader responsereader = new StreamReader(connection.GetResponse().GetResponseStream()))
            //      {
            //          responsedata = responsereader.ReadToEnd();
            //          JObject joresponse1 = JObject.Parse(responsedata);
            //      }
            //  }
            //  catch (Exception ex)
            //  {
            //      Console.WriteLine(ex.Message);
            //      return ex.Message;
            //  }
            return "success";
        }
        public string Insertnotification(long MI_Id, long empid, long task_id, string titletext, long userid, string type)
        {
            //try
            //{
            //    TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            //    DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

            //    ISM_PlannerCreationDTO data = new ISM_PlannerCreationDTO();
            //    ISM_NotificationsDMO push = new ISM_NotificationsDMO();
            //    push.MI_Id = MI_Id;
            //    push.HRME_Id = empid;
            //    push.ISMNO_Notification = titletext;
            //    push.ISMNO_NoticationDate = indianTime;
            //    push.ISMNO_NotificationType = type;
            //    push.ISMNO_ReadFlg = false;
            //    push.ISMNO_MakeUnReadFlg = false;
            //    push.ISMNO_ActiveFlag = true;
            //    push.ISMNO_CreatedBy = userid;
            //    push.ISMNO_UpdatedBy = userid;
            //    push.CreatedDate = indianTime;
            //    push.UpdatedDate = indianTime;
            //    _IssueContext.Add(push);
            //    var contactExists = _IssueContext.SaveChanges();
            //    if (contactExists > 0)
            //    {
            //        data.returnval = true;
            //    }
            //    else
            //    {
            //        data.returnval = false;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //    return ex.Message;
            //}
            return "success";
        }
        public TaskCreationFromClintDTO deactive(TaskCreationFromClintDTO data)
        {
            //try
            //{
            //    var result = _IssueContext.ISM_TaskCreationDMO.Single(t => t.ISMTCR_Id == data.ISMTCR_Id);

            //    if (result.ISMTCR_ActiveFlg == true)
            //    {
            //        result.ISMTCR_ActiveFlg = false;
            //    }
            //    else if (result.ISMTCR_ActiveFlg == false)
            //    {
            //        result.ISMTCR_ActiveFlg = true;
            //    }
            //    result.UpdatedDate = DateTime.Now;
            //    _IssueContext.Update(result);
            //    var resultatt = _IssueContext.ISM_TaskCreation_AttachmentDMO.Where(a => a.ISMTCR_Id == data.ISMTCR_Id).Distinct().ToList();
            //    foreach (var at in resultatt)
            //    {
            //        var resultA = _IssueContext.ISM_TaskCreation_AttachmentDMO.Single(t => t.ISMTCRAT_Id == at.ISMTCRAT_Id);

            //        if (resultA.ISMTCRAT_ActiveFlg == true)
            //        {
            //            resultA.ISMTCRAT_ActiveFlg = false;
            //        }
            //        else if (resultA.ISMTCRAT_ActiveFlg == false)
            //        {
            //            resultA.ISMTCRAT_ActiveFlg = true;
            //        }
            //        resultA.UpdatedDate = DateTime.Now;
            //        _IssueContext.Update(resultA);
            //    }

            //    int returnval = _IssueContext.SaveChanges();
            //    if (returnval > 0)
            //    {
            //        data.returnval = true;
            //    }
            //    else
            //    {
            //        data.returnval = false;
            //    }
            //}
            //catch (Exception ee)
            //{
            //    Console.WriteLine(ee.Message);
            //}

            return data;
        }

    }
}