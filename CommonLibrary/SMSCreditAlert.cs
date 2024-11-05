using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using Newtonsoft.Json;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace CommonLibrary
{
    public class SMSCreditAlert
    {
        public DomainModelMsSqlServerContext _db;
        public SMSCreditAlert(DomainModelMsSqlServerContext db)
        {
            _db = db;
        }
        public CommonDTO getalertcount(CommonDTO data)
        {

            try
            {
                long credit = 0;
                long Rcredit = 0;
                string WORKINGKEY = "";
                var instiutionlist = _db.Institution.Where(e => e.MI_Id == data.IVRM_MI_Id).Distinct().ToList();

                if (instiutionlist.Count > 0)
                {
                        if (instiutionlist.FirstOrDefault().MI_SMSCountAlert == null)
                        {
                            credit = 0;
                        }
                        else
                        {
                            credit = Convert.ToInt64(instiutionlist.FirstOrDefault().MI_SMSCountAlert);
                        }
                        var alldetails = _db.SMS_DETAILS_DMO.Where(t => t.MI_ID.Equals(data.IVRM_MI_Id)).ToList();
                        if (alldetails.Count > 0)
                        {

                            if (alldetails.FirstOrDefault().IVRMSD_WORKINGKEY != null && alldetails.FirstOrDefault().IVRMSD_WORKINGKEY != "")
                            {
                                WORKINGKEY = alldetails.FirstOrDefault().IVRMSD_WORKINGKEY;


                                var url1 = "https://api-alerts.kaleyra.com/v4/?api_key=" + WORKINGKEY + "&method=account.credits";
                                System.Net.HttpWebRequest request1 = System.Net.WebRequest.Create(url1) as HttpWebRequest;
                                System.Net.HttpWebResponse response1 =  request1.GetResponse() as System.Net.HttpWebResponse;
                                Stream stream1 = response1.GetResponseStream();

                                StreamReader readStream1 = new StreamReader(stream1, Encoding.UTF8);
                                string responseparameters1 = readStream1.ReadToEnd();
                                var myContent1 = JsonConvert.SerializeObject(responseparameters1);

                                dynamic responsedata1 = JsonConvert.DeserializeObject(myContent1);
                                var statusdetails = JsonConvert.DeserializeObject<logincreditstausdata>(responsedata1);

                                if (statusdetails.status == "OK")
                                {
                                    var Rcredit1 = statusdetails.data.credits;

                                    Rcredit = Convert.ToInt64(Rcredit1);
                                    if (Rcredit <= credit)
                                    {
                                    data.smsalrtflag = true;
                                    data.Rcredit = Rcredit;
                                }

                                }
                            else
                            {
                                data.smsalrtflag = false;
                            }

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
    }



    public class logincreditstausdata
    {
        public string status { get; set; }
        public string message { get; set; }
        public string code { get; set; }
        public string credits { get; set; }

        // public string data { get; set; }
        public logincredit data { get; set; }
    }

    public class logincredit
    {
        public string credits { get; set; }

    }

}