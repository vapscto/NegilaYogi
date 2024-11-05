using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace CommonLibrary
{
    public class SubscriptionPaymentNotification
    {
        public PortalContext _PortalContext;
        public SubscriptionPaymentNotification(PortalContext db)
        {
            _PortalContext = db;
        }
        public Array getnotificationdetails(long MI_Id, long UserId)
        {
            var checkuserid = _PortalContext.IVRM_Payment_User_MappingDMO.Where(a => a.User_Id == UserId).ToList();
            vmspaymentsubsctiptiondto dd = new vmspaymentsubsctiptiondto();

            var geturl = _PortalContext.IVRM_Storage_path_Details.ToList();

            if (geturl.Count > 0 && geturl.FirstOrDefault().IVRM_VMS_Subscription_URL != null && geturl.FirstOrDefault().IVRM_VMS_Subscription_URL != "")
            {
                if (checkuserid.Count > 0)
                {
                    var stringurl = geturl.FirstOrDefault().IVRM_VMS_Subscription_URL;

                    var getinstitutioncode = _PortalContext.VirtualSchool.Where(a => a.IVRM_MI_Id == MI_Id).ToList();

                    string institutioncode = getinstitutioncode.FirstOrDefault().IVRM_Sub_Domain_Name;

                    vmspaymentsubsctiptiondto vmspaymentsubsctiptiondto = new vmspaymentsubsctiptiondto();
                    vmspaymentsubsctiptiondto.IVRM_MI_Id = MI_Id;
                    vmspaymentsubsctiptiondto.ISMMCLT_ClientCode = institutioncode;

                    HttpClient client1 = new HttpClient();
                    client1.BaseAddress = new Uri(stringurl);
                    client1.DefaultRequestHeaders.Accept.Clear();
                    client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client1.PostAsJsonAsync("api/ISM_Master_ClientProject_PaymentFacade/paymentnotification", vmspaymentsubsctiptiondto).Result;

                    string description = string.Empty;
                    if (response.IsSuccessStatusCode)
                    {
                        string result = response.Content.ReadAsStringAsync().Result;
                        description = result;

                        dd = JsonConvert.DeserializeObject<vmspaymentsubsctiptiondto>(description, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    }
                }
            }
            return dd.getpaymentnotificationdetails;
        }

        public Array getpaymentnotificationdetails(long MI_Id, long UserId)
        {

            vmspaymentsubsctiptiondto dd = new vmspaymentsubsctiptiondto();

            var geturl = _PortalContext.IVRM_Storage_path_Details.ToList();

            if (geturl.Count > 0 && geturl.FirstOrDefault().IVRM_VMS_Subscription_URL != null && geturl.FirstOrDefault().IVRM_VMS_Subscription_URL != "")
            {

                var stringurl = geturl.FirstOrDefault().IVRM_VMS_Subscription_URL;

                var getinstitutioncode = _PortalContext.VirtualSchool.Where(a => a.IVRM_MI_Id == MI_Id).ToList();

                string institutioncode = getinstitutioncode.FirstOrDefault().IVRM_Sub_Domain_Name;

                vmspaymentsubsctiptiondto vmspaymentsubsctiptiondto = new vmspaymentsubsctiptiondto();
                vmspaymentsubsctiptiondto.IVRM_MI_Id = MI_Id;
                vmspaymentsubsctiptiondto.ISMMCLT_ClientCode = institutioncode;

                HttpClient client1 = new HttpClient();
                client1.BaseAddress = new Uri(stringurl);
                client1.DefaultRequestHeaders.Accept.Clear();
                client1.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client1.PostAsJsonAsync("api/ISM_Master_ClientProject_PaymentFacade/paymentnotification", vmspaymentsubsctiptiondto).Result;

                string description = string.Empty;
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    description = result;

                    dd = JsonConvert.DeserializeObject<vmspaymentsubsctiptiondto>(description, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }

            }
            return dd.getpaymentnotificationdetails;
        }
    }
}