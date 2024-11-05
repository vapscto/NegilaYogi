using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.Fee;
using FeeServiceHub.com.vaps.interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using paytm.security;
using paytm.util;
using paytm.exception;
using Razorpay.Api;
using System.Text;
using System.Security.Cryptography;


namespace FeeServiceHub.com.vaps.services
{
    public class Student_SettlementImpl : Student_SettlementInterface
    {
        public FeeGroupContext _FeeGroupContext;
        public Student_SettlementImpl(FeeGroupContext FeeGroupContext)
        {
            _FeeGroupContext = FeeGroupContext;
        }
        public Student_SettlementDTO Getdetails(Student_SettlementDTO data)
        {
            try
            {
                var year = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t=>t.ASMAY_Order).ToList();
                data.yearlist = year.Distinct().ToArray();

                //var merchants = _FeeGroupContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == data.MI_Id && t.FPGD_PGActiveFlag == "1" && t.User_id==data.user_id).ToList();
                //data.merchantlist = merchants.Distinct().ToArray();

                // data.datelist = _FeeGroupContext.Fee_Payment_Overall_Settlement_DetailsDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id).Select(t => t.FYPPST_Settlement_Date).Distinct().ToArray();

                List<Student_SettlementDTO> list = new List<Student_SettlementDTO>();

                var list123 = (from a in _FeeGroupContext.FEeGroupLoginPreviledgeDMO
                               where (a.MI_ID == data.MI_Id && a.User_Id == data.user_id)
                               select new Student_SettlementDTO
                               {
                                   FMG_Id = a.FMG_ID
                               }
      ).Distinct().Select(t => t.FMG_Id).ToList();

                list = (from a in _FeeGroupContext.FeeGroupDMO
                        where (a.MI_Id == data.MI_Id && list123.Contains(a.FMG_Id))
                        select new Student_SettlementDTO
                        {
                            user_id = a.user_id
                        }
        ).Distinct().ToList();

                List<long> useridss = new List<long>();

                foreach (var x in list)
                {
                    useridss.Add(x.user_id);
                }


                //var merchants = _FeeGroupContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == data.MI_Id && t.FPGD_PGActiveFlag == "1" && useridss.Contains(t.User_id) && t.IMPG_Id == data.IMPG_Id).ToList();
                //data.merchantlist = merchants.Distinct().ToArray();


                data.savedlist = _FeeGroupContext.Fee_Payment_Overall_Settlement_DetailsDMO.Where(t => t.MI_Id == data.MI_Id && useridss.Contains(t.User_id)).Distinct().ToArray();

                var classes = _FeeGroupContext.School_M_Class.Where(t => t.MI_Id == data.MI_Id && t.ASMCL_ActiveFlag).Distinct().ToList();
                data.classlist = classes.ToArray();

                var sections = _FeeGroupContext.school_M_Section.Where(t => t.MI_Id == data.MI_Id && t.ASMC_ActiveFlag == 1).Distinct().ToList();
                data.sectionlist = sections.ToArray();

                var routes = _FeeGroupContext.MasterRouteDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMR_ActiveFlg).Distinct().ToList();
                data.routelist = routes.ToArray();

                data.paymentgatewaydet = (from a in _FeeGroupContext.PAYUDETAILS
                                          from b in _FeeGroupContext.Fee_PaymentGateway_Details
                                  where (a.IMPG_Id==b.IMPG_Id && b.MI_Id==data.MI_Id)
                                  select a).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public Student_SettlementDTO getdates(Student_SettlementDTO data)
        {
            try
            {
                data.datelist = _FeeGroupContext.Fee_Payment_Overall_Settlement_DetailsDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.User_id==data.user_id).Select(t => t.FYPPST_Settlement_Date).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public Student_SettlementDTO savedata(Student_SettlementDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);
                //DateTime indianTime = TimeZoneInfo.ConvertTime(data.Selected_Date, INDIAN_ZONE);

                List<Student_SettlementDTO> PAYMENTPARAMDETAILS = new List<Student_SettlementDTO>();
                PAYMENTPARAMDETAILS = (from a in _FeeGroupContext.PAYUDETAILS
                                       where (a.IMPG_Id == data.IMPG_Id)
                                       select new Student_SettlementDTO
                                       {
                                           IMPG_PGFlag = a.IMPG_PGFlag,
                                           IMPG_SettlementURL = a.IMPG_SettlementURL
                                       }
           ).ToList();


                //var SubMerchantKey = _FeeGroupContext.Fee_PaymentGateway_Details.Single(t => t.MI_Id == data.MI_Id && t.FPGD_Id == data.FPGD_Id).FPGD_SubMerchantKey;
                List<Student_SettlementDTO> paymentdet = new List<Student_SettlementDTO>();
                paymentdet = (from a in _FeeGroupContext.Fee_PaymentGateway_Details
                              where (a.MI_Id == data.MI_Id && a.FPGD_Id == data.FPGD_Id)
                              select new Student_SettlementDTO
                              {
                                  MID = a.FPGD_MerchantId,
                                  FPGD_SubMerchantKey = a.FPGD_SubMerchantKey,
                                  FPGD_AuthorizationHeader = a.FPGD_AuthorizationHeader,
                                  FPGD_AuthorisationKey = a.FPGD_AuthorisationKey,
                              }
           ).ToList();

                //var AuthorizationHeader = _FeeGroupContext.Fee_PaymentGateway_Details.Single(t => t.MI_Id == data.MI_Id && t.FPGD_Id == data.FPGD_Id).FPGD_AuthorizationHeader;

                var date = data.Selected_Date.ToString("yyyy-MM-dd");

                List<Student_SettlementDTO> list = new List<Student_SettlementDTO>();

                var list123 = (from a in _FeeGroupContext.FEeGroupLoginPreviledgeDMO
                               where (a.MI_ID == data.MI_Id && a.User_Id == data.user_id)
                               select new Student_SettlementDTO
                               {
                                   FMG_Id = a.FMG_ID
                               }
      ).Distinct().Select(t => t.FMG_Id).ToList();

                list = (from a in _FeeGroupContext.FeeGroupDMO
                        where (a.MI_Id == data.MI_Id && list123.Contains(a.FMG_Id))
                        select new Student_SettlementDTO
                        {
                            user_id = a.user_id
                        }
        ).Distinct().ToList();

                long useridss = 0;

                foreach (var x in list)
                {
                    useridss=  x.user_id;
                }

                //var values = new Dictionary<string, string>
                //        {
                //        { "Authorization", paymentdet.FirstOrDefault().FPGD_AuthorizationHeader },
                //        };

                string url = "";

                if (PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_PGFlag.Equals("PAYU"))
                {
                    //url = "https://www.payumoney.com/treasury/op/getSettlementDetailsByDate?merchantKey=SubMerchantKey&settlementDate=date";
                    url = PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_SettlementURL;

                    url = url.Replace("date", date);
                    url = url.Replace("SubMerchantKey", paymentdet.FirstOrDefault().FPGD_SubMerchantKey);

                    System.Net.WebRequest req = System.Net.WebRequest.Create(url);
                    req.Method = "GET";
                    req.Headers["Authorization"] = paymentdet.FirstOrDefault().FPGD_AuthorizationHeader;
                    // req.Proxy = new System.Net.WebProxy(ProxyString, true); //true means no proxy
                    System.Net.WebResponse resp = req.GetResponseAsync().Result;
                    System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                    string s = sr.ReadToEnd().Trim();
                    JObject joResponse1 = JObject.Parse(s);
                    JArray array1 = (JArray)joResponse1["result"];
                    if (array1.Count == 0)
                    {
                        data.settled_flag = false;
                    }
                    else if (array1.Count > 0)
                    {
                        data.settled_flag = true;
                    }

                    foreach (JObject root1 in array1)
                    {
                        JArray array2 = (JArray)root1["transaction"];

                        Fee_Payment_Overall_Settlement_DetailsDMO obj1 = new Fee_Payment_Overall_Settlement_DetailsDMO();
                        obj1.MI_Id = data.MI_Id;
                        obj1.ASMAY_Id = data.ASMAY_Id;
                        obj1.FYPPST_Settlement_Id = (String)root1["settlementId"];
                        obj1.FYPPST_Settlement_Date = DateTime.ParseExact((String)root1["settlementCompletedDate"], "yyyy-MM-dd", null);
                        obj1.FYPPST_Settlement_Amount = (Int64)root1["settlementAmount"];
                        obj1.FYPPST_UTR_No = (String)root1["utrnumber"];
                        //obj1.User_id = data.user_id;
                        obj1.User_id = Convert.ToInt32(useridss);

                        _FeeGroupContext.Add(obj1);

                        foreach (JObject root2 in array2)
                        {
                            Fee_Payment_Settlement_DetailsDMO obj2 = new Fee_Payment_Settlement_DetailsDMO();
                            obj2.FYPPSD_PAYU_Id = (String)root2["payuId"];
                            obj2.FYPPSD_Transaction_amount = (Int64)root2["transactionAmount"];
                            obj2.FYPPSD_Payment_Id = (string)root2["paymentId"];
                            obj2.FYPPSD_Payment_Mode = (String)root2["paymentMode"];
                            obj2.FYPPSD_Payment_Status = (String)root2["paymentStatus"];
                            obj2.FYPPSD_Transaction_Date = DateTime.ParseExact((String)root2["paymentAddedOn"], "yyyy-MM-dd", null);
                            obj2.FYPPSD_Payment_Amount = (Int64)root2["paymentAmount"];
                            obj2.FYPPST_Id = obj1.FYPPST_Id;

                            _FeeGroupContext.Add(obj2);
                        }
                    }
                    var exists = _FeeGroupContext.SaveChanges();
                    if (exists >= 1)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }

                if (PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_PGFlag.Equals("PAYTM"))
                {

                    string utrrno = "";
                    //url = "https://securegw.paytm.in/merchant-settlement-services/search/settlement";
                    url = PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_SettlementURL;

                    Dictionary<String, String> paytmParams = new Dictionary<String, String>();
                    paytmParams.Add("MID", paymentdet.FirstOrDefault().MID);
                    paytmParams.Add("date", date);
                    paytmParams.Add("pageNum", "1");
                    paytmParams.Add("pageSize", "100");

                    string paytmChecksum = generateCheckSum(paymentdet.FirstOrDefault().FPGD_AuthorisationKey, paytmParams);
                    paytmParams.Add("checksumHash", paytmChecksum);


                    var myContent = JsonConvert.SerializeObject(paytmParams);

                    //String postData = "JsonData=" + new JavaScriptSerializer().Serialize(paytmParams);
                    String postData = myContent;
                    HttpWebRequest connection = (HttpWebRequest)WebRequest.Create(url);
                    connection.ContentType = "application/json";
                    connection.MediaType = "application/json";
                    connection.Accept = "application/json";
                    
                    connection.Method = "POST";

                    using (StreamWriter requestWriter = new StreamWriter(connection.GetRequestStream()))
                    {
                        requestWriter.Write(postData);
                    }
                    string responseData = string.Empty;

                    using (StreamReader responseReader = new StreamReader(connection.GetResponse().GetResponseStream()))
                    {
                        responseData = responseReader.ReadToEnd();
                        
                        //var obj = JArray.Parse(responseData);
                        JObject joResponse1 = JObject.Parse(responseData);
                        JArray array1 = (JArray)joResponse1["settlementSearchResponseVO"]["settlementDetailList"];

                        string totalcount= joResponse1["settlementSearchResponseVO"]["totalCount"].ToString();
                        string settledAmount = joResponse1["settlementSearchResponseVO"]["settledAmount"].ToString();

                        settledAmount = settledAmount.Replace(".00", "");

                        if (array1.Count == 0)
                        {
                            data.settled_flag = false;
                        }
                        else if (array1.Count > 0)
                        {
                            data.settled_flag = true;
                        }

                        //foreach (JObject root1 in array1)
                        //{
                            Fee_Payment_Overall_Settlement_DetailsDMO obj1 = new Fee_Payment_Overall_Settlement_DetailsDMO();
                            obj1.MI_Id = data.MI_Id;
                            obj1.ASMAY_Id = data.ASMAY_Id;
                         

                            obj1.FYPPST_Settlement_Id =  indianTime.ToString("yyyyMMddHHmmss");

                            obj1.FYPPST_Settlement_Date = indianTime;

                            obj1.FYPPST_Settlement_Amount = Convert.ToInt64(settledAmount);

                            obj1.User_id = Convert.ToInt32(useridss);

                            utrrno = (String)(array1[0]["UTR"]);
                            obj1.FYPPST_UTR_No = utrrno;

                            _FeeGroupContext.Add(obj1);

                            foreach (JObject root2 in array1)
                            {
                                Fee_Payment_Settlement_DetailsDMO obj2 = new Fee_Payment_Settlement_DetailsDMO();
                                obj2.FYPPSD_PAYU_Id = (String)root2["TXNID"];
                                string FYPPSD_Transaction_amount = (String)(root2["TXNAMOUNT"]);

                                FYPPSD_Transaction_amount = FYPPSD_Transaction_amount.Replace(".00", "");

                                obj2.FYPPSD_Transaction_amount = Convert.ToInt64(FYPPSD_Transaction_amount);

                                obj2.FYPPSD_Payment_Id = "0";

                                obj2.FYPPSD_Payment_Mode = (String)root2["PAYMENTMODE"];
                                obj2.FYPPSD_Payment_Status = "Sucess";

                                string FYPPSD_Transaction_Date = (String)(root2["TXNDATE"]);
                                obj2.FYPPSD_Transaction_Date = Convert.ToDateTime(FYPPSD_Transaction_Date);

                                string FYPPSD_Settled_amount = (String)(root2["SETTLEDAMOUNT"]);
                                FYPPSD_Settled_amount = FYPPSD_Settled_amount.Replace(".00", "");

                                obj2.FYPPSD_Payment_Amount = Convert.ToInt64(FYPPSD_Settled_amount);
                                obj2.FYPPST_Id = obj1.FYPPST_Id;

                                utrrno = (String)(root2["UTR"]);

                                _FeeGroupContext.Add(obj2);
                            }

                            //obj1.User_id = utrrno;

                        //}


                        var exists = _FeeGroupContext.SaveChanges();
                        if (exists >= 1)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                    
                }

                if (PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_PGFlag.Equals("RAZORPAY"))
                {
                    //single account added on 17/12/2019

                    var accountvalidation = (from a in _FeeGroupContext.Fee_PaymentGateway_Details
                                             where (a.MI_Id == data.MI_Id && a.FPGD_PGName == "RAZORPAY")
                                             select new FeeStudentTransactionDTO
                                             {
                                                 FPGD_SubMerchantId = a.FPGD_SubMerchantId
                                             }).Distinct().ToArray();

                    //single account added on 17/12/2019
                    if (accountvalidation.Count() > 1)
                    {
                        var FYPPSD_Idmax = (from a in _FeeGroupContext.Fee_Payment_Settlement_DetailsDMO
                                            where (a.MI_Id == data.MI_Id)
                                            select new Student_SettlementDTO
                                            {
                                                FYPPSD_Id = a.FYPPSD_Id,
                                            }
                       ).Distinct().ToList();

                        string utrrno = "";
                        //url = "https://api.razorpay.com/v1/settlements/?count=10&skip=0&from=frmdate&to=todate";
                        url = PAYMENTPARAMDETAILS.FirstOrDefault().IMPG_SettlementURL;

                        //CURRENT DAY - 1 
                        //Int32 unixTimestampstart = (Int32)(DateTime.UtcNow.Date.AddDays(-18).AddHours(-5).AddMinutes(-30).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                        //Int32 unixTimestampend = (Int32)(DateTime.UtcNow.Date.AddDays(-18).AddHours(18).AddMinutes(29).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                        //CURRENT DAY - 1 

                        //CURRENT DAY
                        Int32 unixTimestampstart = (Int32)(DateTime.UtcNow.Date.AddHours(-5).AddMinutes(-30).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                        Int32 unixTimestampend = (Int32)(DateTime.UtcNow.Date.AddHours(18).AddMinutes(29).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                        //CURRENT DAY

                        url = url.Replace("frmdate", unixTimestampstart.ToString());
                        url = url.Replace("todate", unixTimestampend.ToString());

                        List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                        paymentdetails = _FeeGroupContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == data.MI_Id && t.FPGD_PGName == "RAZORPAY").Distinct().ToList();

                        list = (from a in _FeeGroupContext.FEE_RAZOR_TRANSFER_API_DETAILS
                                from b in _FeeGroupContext.FeePaymentDetailsDMO
                                where (a.ORDER_ID == b.fyp_transaction_id && a.MI_ID == data.MI_Id && a.SETTLEMENT_FLAG == "0" && b.FYP_OnlineChallanStatusFlag == "Sucessfull" && b.FYP_PayGatewayType == "RAZORPAY")
                                select new Student_SettlementDTO
                                {
                                    ORDER_ID = a.ORDER_ID,
                                    TRANSFER_ID = a.TRANSFER_ID,
                                    PAYMENT_ID = a.PAYMENT_ID,
                                }
         ).Distinct().ToList();

                        for (int r = 0; r < list.Count(); r++)
                        {
                            //url = "https://api.razorpay.com/v1/payments/paymentid/transfers";
                            //url = url.Replace("paymentid", list[r].PAYMENT_ID);
                            url = "https://api.razorpay.com/v1/transfers/TRANSFERID";
                            url = url.Replace("TRANSFERID", list[r].TRANSFER_ID);

                            string method1 = "GET";

                            HttpWebRequest requestsett = (HttpWebRequest)WebRequest.Create(url);
                            requestsett.Method = method1.ToString();
                            requestsett.ContentLength = 0;
                            requestsett.ContentType = "application/json";

                            string userAgent1 = string.Format("{0} {1}", RazorpayClient.Version, getAppDetailsUa());
                            requestsett.UserAgent = "razorpay-dot-net/" + userAgent1;

                            string authString1 = string.Format("{0}:{1}", paymentdetails.FirstOrDefault().FPGD_SaltKey, paymentdetails.FirstOrDefault().FPGD_AuthorisationKey);

                            requestsett.Headers["Authorization"] = "Basic " + Convert.ToBase64String(
                                Encoding.UTF8.GetBytes(authString1));

                            System.Net.WebResponse resp1 = requestsett.GetResponseAsync().Result;
                            System.IO.StreamReader sr1 = new System.IO.StreamReader(resp1.GetResponseStream());
                            string s1 = sr1.ReadToEnd().Trim();
                            JObject root2 = JObject.Parse(s1);
                            // JArray array2 = (JArray)joResponse2[""];
                            Fee_Payment_Settlement_DetailsDMO obj2 = new Fee_Payment_Settlement_DetailsDMO();
                            if ((String)root2["recipient_settlement_id"] != null)
                            {
                                var getconnid = (from a in _FeeGroupContext.FEE_RAZOR_TRANSFER_API_DETAILS
                                                 where (a.MI_ID == data.MI_Id && a.TRANSFER_ID == (String)root2["id"])
                                                 select new FeeStudentTransactionDTO
                                                 {
                                                     order_id = a.ORDER_ID,
                                                     payment_id = a.PAYMENT_ID
                                                 }
                             ).ToList();

                                //obj2.FYPPSD_PAYU_Id = getconnid[0].order_id.ToString();
                                obj2.FYPPSD_PAYU_Id = getconnid[0].payment_id.ToString();
                                string FYPPSD_Transaction_amount = (String)(root2["amount"]);
                                obj2.FYPPSD_Transaction_amount = Convert.ToInt32(FYPPSD_Transaction_amount) / 100;
                                //obj2.FYPPSD_Payment_Id = getconnid[0].payment_id.ToString();
                                obj2.FYPPSD_Payment_Id = getconnid[0].order_id.ToString();
                                obj2.FYPPSD_Payment_Mode = (String)root2["recipient_settlement_id"];
                                obj2.FYPPSD_Payment_Status = "Sucess";

                                // Format our new DateTime object to start at the UNIX Epoch
                                System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);

                                // Add the timestamp (number of seconds since the Epoch) to be converted
                                dateTime = dateTime.AddSeconds((double)root2["created_at"]);

                                obj2.FYPPSD_Transaction_Date = dateTime;
                                obj2.FYPPSD_Payment_Amount = Convert.ToInt32(FYPPSD_Transaction_amount) / 100; ;
                                obj2.MI_Id = data.MI_Id;
                                obj2.FYPPST_Id = 0;

                                _FeeGroupContext.Add(obj2);
                                //var existsRAZORPAY = _FeeGroupContext.SaveChanges();

                                var obj_status_stf = _FeeGroupContext.FEE_RAZOR_TRANSFER_API_DETAILS.Where(t => t.MI_ID == data.MI_Id && t.TRANSFER_ID == (String)root2["id"]).FirstOrDefault();

                                obj_status_stf.SETTLEMENT_FLAG = "1";
                                _FeeGroupContext.Update(obj_status_stf);

                                _FeeGroupContext.SaveChanges();
                            }


                        }

                        try
                        {
                            List<Student_SettlementDTO> ovrsett = new List<Student_SettlementDTO>();
                            using (var cmd1 = _FeeGroupContext.Database.GetDbConnection().CreateCommand())

                            {
                                cmd1.CommandText = "getsettlementsummation";
                                cmd1.CommandType = CommandType.StoredProcedure;

                                cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                                 SqlDbType.BigInt)
                                {
                                    Value = data.MI_Id
                                });

                                cmd1.Parameters.Add(new SqlParameter("@Asmay_Id",
                                SqlDbType.BigInt)
                                {
                                    Value = data.ASMAY_Id
                                });
                                if (FYPPSD_Idmax == null)
                                {
                                    cmd1.Parameters.Add(new SqlParameter("@FYPPSD_Idmax", SqlDbType.BigInt)
                                    {
                                        Value = 0
                                    });
                                }
                                else
                                {
                                    cmd1.Parameters.Add(new SqlParameter("@FYPPSD_Idmax", SqlDbType.BigInt)
                                    {
                                        Value = FYPPSD_Idmax.Max(x => x.FYPPSD_Id)
                                    });
                                }

                                cmd1.Parameters.Add(new SqlParameter("@User_Id", SqlDbType.BigInt)
                                {
                                    Value = data.user_id
                                });

                                if (cmd1.Connection.State != ConnectionState.Open)
                                    cmd1.Connection.Open();

                                var retObject1 = new List<dynamic>();

                                try
                                {
                                    using (var dataReader1 = cmd1.ExecuteReader())
                                    {
                                        while (dataReader1.Read())
                                        {
                                            ovrsett.Add(new Student_SettlementDTO
                                            {
                                                FYPPSD_Payment_Mode = Convert.ToString(dataReader1["FYPPSD_Payment_Mode"]),
                                                overallsettamt = Convert.ToInt32(dataReader1["SettmentAmount"]),
                                            });
                                        }
                                    }
                                }

                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }



                            foreach (var value in ovrsett)
                            //for (int q = 0; q < data.ovrsett.Length; q++)
                            {
                                Fee_Payment_Overall_Settlement_DetailsDMO overallsett = new Fee_Payment_Overall_Settlement_DetailsDMO();

                                url = "https://api.razorpay.com/v1/transfers?expand[]=recipient_settlement&recipient_settlement_id=receipient_sett_id";
                                url = url.Replace("receipient_sett_id", value.FYPPSD_Payment_Mode);

                                string method1 = "GET";

                                HttpWebRequest requestsett = (HttpWebRequest)WebRequest.Create(url);
                                requestsett.Method = method1.ToString();
                                requestsett.ContentLength = 0;
                                requestsett.ContentType = "application/json";

                                string userAgent1 = string.Format("{0} {1}", RazorpayClient.Version, getAppDetailsUa());
                                requestsett.UserAgent = "razorpay-dot-net/" + userAgent1;

                                string authString1 = string.Format("{0}:{1}", paymentdetails.FirstOrDefault().FPGD_SaltKey, paymentdetails.FirstOrDefault().FPGD_AuthorisationKey);

                                requestsett.Headers["Authorization"] = "Basic " + Convert.ToBase64String(
                                    Encoding.UTF8.GetBytes(authString1));

                                System.Net.WebResponse resp1 = requestsett.GetResponseAsync().Result;
                                System.IO.StreamReader sr1 = new System.IO.StreamReader(resp1.GetResponseStream());
                                string s1 = sr1.ReadToEnd().Trim();
                                JObject root3 = JObject.Parse(s1);

                                Fee_Payment_Overall_Settlement_DetailsDMO obj3 = new Fee_Payment_Overall_Settlement_DetailsDMO();

                                obj3.MI_Id = data.MI_Id;
                                obj3.ASMAY_Id = data.ASMAY_Id;
                                obj3.FYPPST_Settlement_Id = value.FYPPSD_Payment_Mode;

                                JArray array2 = (JArray)root3["items"];
                                //JArray array3 = (JArray)array2["recipient_settlement"];

                                obj3.FYPPST_UTR_No = (array2[0]["recipient_settlement"]["utr"].ToString());
                                obj3.FYPPST_Settlement_Amount = value.overallsettamt;

                                // Format our new DateTime object to start at the UNIX Epoch
                                System.DateTime dateTime1 = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);

                                // Add the timestamp (number of seconds since the Epoch) to be converted
                                obj3.FYPPST_Settlement_Date = dateTime1.AddSeconds((double)(array2[0]["recipient_settlement"]["created_at"]));
                                obj3.User_id = 0;

                                _FeeGroupContext.Add(obj3);
                                _FeeGroupContext.SaveChanges();

                                _FeeGroupContext.Database.ExecuteSqlCommand("updatesettlementid @p0,@p1,@p2", data.MI_Id, data.ASMAY_Id, value.FYPPSD_Payment_Mode);

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    else
                    {
                        Int32 unixTimestampstart = 0;
                        Int32 unixTimestampend = 0;

                        //string currdate = "1";

                        //if (currdate != null && currdate != "" && currdate != "0" && currdate != "1")
                        //{
                        //    CURRENT DAY -1
                        //    unixTimestampstart = (Int32)(DateTime.UtcNow.Date.AddDays(-(Convert.ToInt32(currdate))).AddHours(-5).AddMinutes(-30).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                        //    unixTimestampend = (Int32)(DateTime.UtcNow.Date.AddDays(-(Convert.ToInt32(currdate))).AddHours(18).AddMinutes(29).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                        //}
                        //else
                        //{
                        //    CURRENT DAY
                        //    unixTimestampstart = (Int32)(DateTime.UtcNow.Date.AddHours(-5).AddMinutes(-30).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                        //    unixTimestampend = (Int32)(DateTime.UtcNow.Date.AddHours(18).AddMinutes(29).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                        //}

                        unixTimestampstart = (Int32)(DateTime.UtcNow.Date.AddHours(-5).AddMinutes(-30).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                        unixTimestampend = (Int32)(DateTime.UtcNow.Date.AddHours(18).AddMinutes(29).Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                        //url = "https://api.razorpay.com/v1/settlements/?count=10&skip=0&from=1576434600&to=1576607340";

                        string urlupdated = "https://api.razorpay.com/v1/settlements/?count=10&skip=0&from=frmdate&to=todate";
                        urlupdated = urlupdated.Replace("frmdate", unixTimestampstart.ToString());
                        urlupdated = urlupdated.Replace("todate", unixTimestampend.ToString());

                        string method1 = "GET";

                        HttpWebRequest requestsett = (HttpWebRequest)WebRequest.Create(urlupdated);
                        requestsett.Method = method1.ToString();
                        requestsett.ContentLength = 0;
                        requestsett.ContentType = "application/json";

                        string userAgent1 = string.Format("{0} {1}", RazorpayClient.Version, getAppDetailsUa());
                        requestsett.UserAgent = "razorpay-dot-net/" + userAgent1;

                        List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
                        paymentdetails = _FeeGroupContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == data.MI_Id && t.FPGD_PGName == "RAZORPAY").Distinct().ToList();

                        string authString1 = string.Format("{0}:{1}", paymentdetails.FirstOrDefault().FPGD_SaltKey, paymentdetails.FirstOrDefault().FPGD_AuthorisationKey);

                        requestsett.Headers["Authorization"] = "Basic " + Convert.ToBase64String(
                            Encoding.UTF8.GetBytes(authString1));

                        System.Net.WebResponse resp1 = requestsett.GetResponseAsync().Result;
                        System.IO.StreamReader sr1 = new System.IO.StreamReader(resp1.GetResponseStream());
                        string s1 = sr1.ReadToEnd().Trim();
                        JObject root3 = JObject.Parse(s1);

                        Fee_Payment_Overall_Settlement_DetailsDMO obj3 = new Fee_Payment_Overall_Settlement_DetailsDMO();

                        JArray array2 = (JArray)root3["items"];

                        obj3.MI_Id = data.MI_Id;
                        obj3.ASMAY_Id = data.ASMAY_Id;
                        obj3.FYPPST_Settlement_Id = (array2[0]["id"].ToString());
                        obj3.FYPPST_UTR_No = (array2[0]["utr"].ToString());
                        obj3.FYPPST_Settlement_Amount = (Int32)(array2[0]["amount"])/100;

                        // Format our new DateTime object to start at the UNIX Epoch
                        System.DateTime dateTime1 = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);

                        // Add the timestamp (number of seconds since the Epoch) to be converted
                        obj3.FYPPST_Settlement_Date = dateTime1.AddSeconds((double)(array2[0]["created_at"]));
                        obj3.User_id = 0;

                        _FeeGroupContext.Add(obj3);
                        _FeeGroupContext.SaveChanges();

                        //second API

                        string unixTimestampstartDAY = DateTime.UtcNow.Day.ToString();
                        string unixTimestampstartMONTH = DateTime.UtcNow.Month.ToString();
                        string unixTimestampstartYEAR = DateTime.UtcNow.Year.ToString();

                        //url = "https://api.razorpay.com/v1/settlements/recon/combined?year=2019&month=12&day=16";

                        string secondurl = "https://api.razorpay.com/v1/settlements/recon/combined?year=YEARID&month=MONTHID&day=DAYID";
                        secondurl = secondurl.Replace("YEARID", unixTimestampstartYEAR.ToString());
                        secondurl = secondurl.Replace("MONTHID", unixTimestampstartMONTH.ToString());
                        secondurl = secondurl.Replace("DAYID", unixTimestampstartDAY.ToString());

                        string method2 = "GET";

                        HttpWebRequest requestsettI = (HttpWebRequest)WebRequest.Create(secondurl);
                        requestsettI.Method = method1.ToString();
                        requestsettI.ContentLength = 0;
                        requestsettI.ContentType = "application/json";

                        string userAgent2 = string.Format("{0} {1}", RazorpayClient.Version, getAppDetailsUa());
                        requestsettI.UserAgent = "razorpay-dot-net/" + userAgent2;

                        requestsettI.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(authString1));

                        System.Net.WebResponse resp2 = requestsettI.GetResponseAsync().Result;
                        System.IO.StreamReader sr2 = new System.IO.StreamReader(resp2.GetResponseStream());
                        string s2 = sr2.ReadToEnd().Trim();
                        JObject root4 = JObject.Parse(s2);

                        JArray array3 = (JArray)root4["items"];

                        for (int q = 0; q < array3.Count(); q++)
                        {
                            Fee_Payment_Settlement_DetailsDMO obj4 = new Fee_Payment_Settlement_DetailsDMO();
                            if (array3[q]["type"].ToString() == "payment")
                            {
                                obj4.FYPPSD_PAYU_Id = array3[q]["entity_id"].ToString();
                                obj4.FYPPSD_Transaction_amount = (Int32)array3[q]["credit"]/100;
                                obj4.FYPPSD_Payment_Id = array3[q]["order_id"].ToString();
                                obj4.FYPPSD_Payment_Mode = array3[q]["settlement_id"].ToString();
                                obj4.FYPPSD_Payment_Status = "Completed";

                                // Format our new DateTime object to start at the UNIX Epoch
                                System.DateTime dateTime2 = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);

                                // Add the timestamp (number of seconds since the Epoch) to be converted
                                obj4.FYPPSD_Transaction_Date = dateTime1.AddSeconds((double)(array2[0]["created_at"]));
                                obj4.FYPPSD_Payment_Amount = (Int32)array3[q]["credit"];
                                obj4.FYPPST_Id = obj3.FYPPST_Id;
                                obj4.MI_Id = data.MI_Id;

                                _FeeGroupContext.Add(obj4);

                                //var obj_status_stf = _FeeGroupContext.FEE_RAZOR_TRANSFER_API_DETAILS.Where(t => t.MI_ID == data.MI_Id && t.ORDER_ID == array3[q]["order_id"].ToString()).FirstOrDefault();

                                //obj_status_stf.SETTLEMENT_FLAG = "1";
                                //_FeeGroupContext.Update(obj_status_stf);

                                //_FeeGroupContext.SaveChanges();

                            }
                        }

                        _FeeGroupContext.SaveChanges();

                    }
                   
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        private static string getAppDetailsUa()
        {
            List<Dictionary<string, string>> appsDetails = RazorpayClient.AppsDetails;

            string appsDetailsUa = string.Empty;

            foreach (Dictionary<string, string> appsDetail in appsDetails)
            {
                string appUa = string.Empty;

                if (appsDetail.ContainsKey("title"))
                {
                    appUa = appsDetail["title"];

                    if (appsDetail.ContainsKey("version"))
                    {
                        appUa += appsDetail["version"];
                    }
                }

                appsDetailsUa += appUa;
            }

            return appsDetailsUa;
        }

        public Student_SettlementDTO viewrecords(Student_SettlementDTO data)
        {
            try
            {
                data.viewlist = _FeeGroupContext.Fee_Payment_Settlement_DetailsDMO.Where(t => t.FYPPST_Id == data.FYPPST_Id).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }
        public Student_SettlementDTO get_classes(Student_SettlementDTO data)
        {

            try
            {
                data.classlist = (from a in _FeeGroupContext.School_Adm_Y_StudentDMO
                                  from b in _FeeGroupContext.School_M_Class
                                  where (a.AMAY_ActiveFlag == 1 && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == b.ASMCL_Id && b.MI_Id == data.MI_Id && b.ASMCL_ActiveFlag)
                                  select b).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public Student_SettlementDTO get_sections(Student_SettlementDTO data)
        {

            try
            {

                //data.sectionlist = (from m in _FeeGroupContext.Masterclasscategory
                //                  from n in _FeeGroupContext.AdmSchoolMasterClassCatSec
                //                  from o in _FeeGroupContext.school_M_Section
                //                  where (m.ASMCC_Id == n.ASMCC_Id && n.ASMS_Id == o.ASMS_Id
                //                  && o.ASMC_ActiveFlag == 1 && o.MI_Id == data.MI_Id && m.Is_Active == true
                //                  && m.ASMCL_Id == data.ASMCL_Id && m.ASMAY_Id == data.ASMAY_Id && o.ASMC_ActiveFlag == 1)
                //                  select new School_M_Section
                //                  {
                //                      ASMS_Id = o.ASMS_Id,
                //                      ASMC_SectionName = o.ASMC_SectionName,
                //                      ASMC_SectionCode = o.ASMC_SectionCode,
                //                      ASMC_Order = o.ASMC_Order,
                //                      ASMC_MaxCapacity = o.ASMC_MaxCapacity,
                //                      ASMC_ActiveFlag = o.ASMC_ActiveFlag
                //                  }).Distinct().ToArray();

                data.sectionlist = (from a in _FeeGroupContext.School_Adm_Y_StudentDMO
                                    from b in _FeeGroupContext.school_M_Section
                                    where (a.AMAY_ActiveFlag == 1 && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && b.MI_Id == data.MI_Id && b.ASMC_ActiveFlag == 1 && b.ASMS_Id == a.ASMS_Id)
                                    select b).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public Student_SettlementDTO get_routes(Student_SettlementDTO data)
        {

            try
            {
                data.routelist = (from a in _FeeGroupContext.School_Adm_Y_StudentDMO
                                  from b in _FeeGroupContext.MasterRouteDMO
                                  from c in _FeeGroupContext.TR_Student_RouteDMO
                                  where (a.AMAY_ActiveFlag == 1 && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && b.MI_Id == data.MI_Id && b.TRMR_ActiveFlg && c.ASMAY_Id == data.ASMAY_Id && c.MI_Id == data.MI_Id && a.AMST_Id == c.AMST_Id && (b.TRMR_Id == c.TRMR_Id || b.TRMR_Id == c.TRMR_Drop_Route))
                                  select b).Distinct().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        public Student_SettlementDTO getreport(Student_SettlementDTO data)
        {
            try
            {
                if (data.allclass == "true" && data.allsection == "false")
                {
                    var reportlist = (from a in _FeeGroupContext.School_Adm_Y_StudentDMO
                                      from b in _FeeGroupContext.AdmissionStudentDMO
                                      from c in _FeeGroupContext.School_M_Class
                                      from d in _FeeGroupContext.school_M_Section
                                      from e in _FeeGroupContext.Fee_Payment_Overall_Settlement_DetailsDMO
                                      from f in _FeeGroupContext.Fee_Payment_Settlement_DetailsDMO
                                      from g in _FeeGroupContext.FeePaymentDetailsDMO
                                      from h in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                      where (a.AMAY_ActiveFlag == 1 && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.AMST_Id == b.AMST_Id && b.MI_Id == data.MI_Id && b.AMST_ActiveFlag == 1 && b.AMST_SOL == "S" && c.MI_Id == data.MI_Id && c.ASMCL_ActiveFlag && c.ASMCL_Id == a.ASMCL_Id && d.MI_Id == data.MI_Id && d.ASMC_ActiveFlag == 1 && d.ASMS_Id == a.ASMS_Id && e.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id && e.FYPPST_Id == f.FYPPST_Id && g.FYP_PaymentReference_Id == f.FYPPSD_PAYU_Id && g.MI_Id == data.MI_Id && g.ASMAY_ID == data.ASMAY_Id && h.FYP_Id == g.FYP_Id && h.AMST_Id == b.AMST_Id && e.FYPPST_Settlement_Date >= data.From_Date && e.FYPPST_Settlement_Date <= data.To_Date && e.User_id==data.user_id && e.User_id == data.user_id && g.user_id == data.user_id)
                                      select new Student_SettlementDTO
                                      {
                                          AMST_Id = b.AMST_Id,
                                          AMST_FirstName = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + " " + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                                          AMST_RegistrationNo = b.AMST_RegistrationNo,
                                          AMST_AdmNo = b.AMST_AdmNo == null ? "" : b.AMST_AdmNo,
                                          AMAY_RollNo = a.AMAY_RollNo,
                                          ASMCL_ClassName = c.ASMCL_ClassName,
                                          ASMC_SectionName = d.ASMC_SectionName,
                                          FYP_Id = g.FYP_Id,
                                          FYPPST_Id = e.FYPPST_Id,
                                          FYPPSD_Id = f.FYPPSD_Id,
                                          FYPPSD_Payment_Mode = f.FYPPSD_Payment_Mode,
                                          FYPPSD_Payment_Id = f.FYPPSD_Payment_Id,
                                          FYPPSD_PAYU_Id = f.FYPPSD_PAYU_Id,
                                          FYPPSD_Payment_Amount = f.FYPPSD_Payment_Amount,
                                          FYPPSD_Transaction_Date = f.FYPPSD_Transaction_Date,
                                          FYPPST_Settlement_Date = e.FYPPST_Settlement_Date,
                                          FYPPST_UTR_No = e.FYPPST_UTR_No,
                                          FYPPST_Settlement_Id = e.FYPPST_Settlement_Id
                                      }).Distinct().ToList();
                    data.reportlist = reportlist.ToArray();

                    if (data.TRMR_Id > 0)
                    {
                        data.reportlist = (from a in reportlist
                                           from b in _FeeGroupContext.TR_Student_RouteDMO
                                           where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMST_Id == a.AMST_Id && (b.TRMR_Drop_Route == data.TRMR_Id || b.TRMR_Id == data.TRMR_Id))
                                           select a).Distinct().ToArray();
                    }
                }

                if (data.allclass == "true" && data.allsection == "true")
                {
                    var reportlist = (from a in _FeeGroupContext.School_Adm_Y_StudentDMO
                                      from b in _FeeGroupContext.AdmissionStudentDMO
                                      from c in _FeeGroupContext.School_M_Class
                                      from d in _FeeGroupContext.school_M_Section
                                      from e in _FeeGroupContext.Fee_Payment_Overall_Settlement_DetailsDMO
                                      from f in _FeeGroupContext.Fee_Payment_Settlement_DetailsDMO
                                      from g in _FeeGroupContext.FeePaymentDetailsDMO
                                      from h in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                      where (a.AMAY_ActiveFlag == 1 && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && a.ASMS_Id == data.ASMS_Id && a.AMST_Id == b.AMST_Id && b.MI_Id == data.MI_Id && b.AMST_ActiveFlag == 1 && b.AMST_SOL == "S" && c.MI_Id == data.MI_Id && c.ASMCL_ActiveFlag && c.ASMCL_Id == a.ASMCL_Id && d.MI_Id == data.MI_Id && d.ASMC_ActiveFlag == 1 && d.ASMS_Id == a.ASMS_Id && e.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id && e.FYPPST_Id == f.FYPPST_Id && g.FYP_PaymentReference_Id == f.FYPPSD_PAYU_Id && g.MI_Id == data.MI_Id && g.ASMAY_ID == data.ASMAY_Id && h.FYP_Id == g.FYP_Id && h.AMST_Id == b.AMST_Id && e.FYPPST_Settlement_Date >= data.From_Date && e.FYPPST_Settlement_Date <= data.To_Date && e.User_id == data.user_id && g.user_id == data.user_id)
                                      select new Student_SettlementDTO
                                      {
                                          AMST_Id = b.AMST_Id,
                                          AMST_FirstName = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + " " + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                                          AMST_RegistrationNo = b.AMST_RegistrationNo,
                                          AMST_AdmNo = b.AMST_AdmNo == null ? "" : b.AMST_AdmNo,
                                          AMAY_RollNo = a.AMAY_RollNo,
                                          ASMCL_ClassName = c.ASMCL_ClassName,
                                          ASMC_SectionName = d.ASMC_SectionName,
                                          FYP_Id = g.FYP_Id,
                                          FYPPST_Id = e.FYPPST_Id,
                                          FYPPSD_Id = f.FYPPSD_Id,
                                          FYPPSD_Payment_Mode = f.FYPPSD_Payment_Mode,
                                          FYPPSD_Payment_Id = f.FYPPSD_Payment_Id,
                                          FYPPSD_PAYU_Id = f.FYPPSD_PAYU_Id,
                                          FYPPSD_Payment_Amount = f.FYPPSD_Payment_Amount,
                                          FYPPSD_Transaction_Date = f.FYPPSD_Transaction_Date,
                                          FYPPST_Settlement_Date = e.FYPPST_Settlement_Date,
                                          FYPPST_UTR_No = e.FYPPST_UTR_No,
                                          FYPPST_Settlement_Id = e.FYPPST_Settlement_Id
                                      }).Distinct().ToList();
                    data.reportlist = reportlist.ToArray();

                    if (data.TRMR_Id > 0)
                    {
                        data.reportlist = (from a in reportlist
                                           from b in _FeeGroupContext.TR_Student_RouteDMO
                                           where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMST_Id == a.AMST_Id && (b.TRMR_Drop_Route == data.TRMR_Id || b.TRMR_Id == data.TRMR_Id))
                                           select a).Distinct().ToArray();
                    }
                }

                if (data.allclass == "false" && data.allsection == "false")
                {
                    var reportlist = (from a in _FeeGroupContext.School_Adm_Y_StudentDMO
                                      from b in _FeeGroupContext.AdmissionStudentDMO
                                      from c in _FeeGroupContext.School_M_Class
                                      from d in _FeeGroupContext.school_M_Section
                                      from e in _FeeGroupContext.Fee_Payment_Overall_Settlement_DetailsDMO
                                      from f in _FeeGroupContext.Fee_Payment_Settlement_DetailsDMO
                                      from g in _FeeGroupContext.FeePaymentDetailsDMO
                                      from h in _FeeGroupContext.Fee_Y_Payment_School_StudentDMO
                                      where (a.AMAY_ActiveFlag == 1 && a.ASMAY_Id == data.ASMAY_Id && a.AMST_Id == b.AMST_Id && b.MI_Id == data.MI_Id && b.AMST_ActiveFlag == 1 && b.AMST_SOL == "S" && c.MI_Id == data.MI_Id && c.ASMCL_ActiveFlag && c.ASMCL_Id == a.ASMCL_Id && d.MI_Id == data.MI_Id && d.ASMC_ActiveFlag == 1 && d.ASMS_Id == a.ASMS_Id && e.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id && e.FYPPST_Id == f.FYPPST_Id && g.FYP_PaymentReference_Id == f.FYPPSD_PAYU_Id && g.MI_Id == data.MI_Id && g.ASMAY_ID == data.ASMAY_Id && h.ASMAY_Id==data.ASMAY_Id && h.FYP_Id == g.FYP_Id && h.AMST_Id == b.AMST_Id && e.FYPPST_Settlement_Date >= data.From_Date && e.FYPPST_Settlement_Date <= data.To_Date && e.User_id==data.user_id && g.user_id==data.user_id)
                                      select new Student_SettlementDTO
                                      {
                                          AMST_Id = b.AMST_Id,
                                          AMST_FirstName = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + " " + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + " " + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                                          AMST_RegistrationNo = b.AMST_RegistrationNo,
                                          AMST_AdmNo = b.AMST_AdmNo == null ? "" : b.AMST_AdmNo,
                                          AMAY_RollNo = a.AMAY_RollNo,
                                          ASMCL_ClassName = c.ASMCL_ClassName,
                                          ASMC_SectionName = d.ASMC_SectionName,
                                          FYP_Id = g.FYP_Id,
                                          FYPPST_Id = e.FYPPST_Id,
                                          FYPPSD_Id = f.FYPPSD_Id,
                                          FYPPSD_Payment_Mode = f.FYPPSD_Payment_Mode,
                                          FYPPSD_Payment_Id = f.FYPPSD_Payment_Id,
                                          FYPPSD_PAYU_Id = f.FYPPSD_PAYU_Id,
                                          FYPPSD_Payment_Amount = f.FYPPSD_Payment_Amount,
                                          FYPPSD_Transaction_Date = f.FYPPSD_Transaction_Date,
                                          FYPPST_Settlement_Date = e.FYPPST_Settlement_Date,
                                          FYPPST_UTR_No = e.FYPPST_UTR_No,
                                          FYPPST_Settlement_Id = e.FYPPST_Settlement_Id
                                      }).Distinct().ToList();
                    data.reportlist = reportlist.ToArray();

                    if (data.TRMR_Id > 0)
                    {
                        data.reportlist = (from a in reportlist
                                           from b in _FeeGroupContext.TR_Student_RouteDMO
                                           where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMST_Id == a.AMST_Id && (b.TRMR_Drop_Route == data.TRMR_Id || b.TRMR_Id == data.TRMR_Id))
                                           select a).Distinct().ToArray();
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public async Task<Student_SettlementDTO> getreport1(Student_SettlementDTO data)
        {

            List<Student_SettlementDTO> list = new List<Student_SettlementDTO>();

            var list123 = (from a in _FeeGroupContext.FEeGroupLoginPreviledgeDMO
                           where (a.MI_ID == data.MI_Id && a.User_Id == data.user_id)
                           select new Student_SettlementDTO
                           {
                               FMG_Id = a.FMG_ID
                           }
  ).Distinct().Select(t => t.FMG_Id).ToList();

            list = (from a in _FeeGroupContext.FeeGroupDMO
                    where (a.MI_Id == data.MI_Id && list123.Contains(a.FMG_Id))
                    select new Student_SettlementDTO
                    {
                        user_id = a.user_id
                    }
    ).Distinct().ToList();

            string useridss = "0";
            List<long> useridlst = new List<long>();

            foreach (var x in list)
            {
                useridss = useridss + "," + x.user_id;
                useridlst.Add(x.user_id);
            }

            data.alldata = (from a in _FeeGroupContext.FeeGroupDMO
                           from b in _FeeGroupContext.FeeHeadDMO
                           from c in _FeeGroupContext.FeeYearlygroupHeadMappingDMO
                           where (a.FMG_Id == c.FMG_Id && b.FMH_Id == c.FMH_Id && c.MI_Id == data.MI_Id && useridlst.Contains(a.user_id))
                           select new DailyCollectionReportDTO
                           {
                               FMH_Id = b.FMH_Id,
                               FMH_FeeName = b.FMH_FeeName,

                           }
                ).Distinct().OrderBy(h => h.FMH_FeeName).ToArray();


            data.studentlist = _FeeGroupContext.feespecialHead.Where(y => y.FMSFH_ActiceFlag == true && y.MI_Id == data.MI_Id).Distinct().ToArray();

            data.allgroupheaddata = (from a in _FeeGroupContext.feespecialHead
                                    from b in _FeeGroupContext.feeSGGG
                                    where (a.FMSFH_Id == b.FMSFH_Id && a.MI_Id == data.MI_Id && a.FMSFH_ActiceFlag == true && b.FMSFHFH_ActiceFlag == true)
                                    select new DailyCollectionReportDTO
                                    {
                                        FMSFHFH_Id = b.FMSFHFH_Id,
                                        FMH_Id = b.FMH_Id,
                                        FMSFH_Id = b.FMSFH_Id
                                    }
                   ).Distinct().ToArray();


            

            try
            {
                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Fee_ClassWise_Settlement_Report_userid";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                    {
                        Value = Convert.ToInt64(data.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                   SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@Fromdate",
                SqlDbType.DateTime)
                    {
                        Value = data.From_Date
                    });
                    cmd.Parameters.Add(new SqlParameter("@Todate",
               SqlDbType.DateTime)
                    {
                        Value = data.To_Date
                    });
                    cmd.Parameters.Add(new SqlParameter("@userid",
              SqlDbType.NVarChar)
                    {
                        //Value = data.user_id
                        Value = useridss
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
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

                        data.reportlist = retObject.ToArray();


                    }
                    catch (Exception ee)
                    {
                        Console.WriteLine(ee.Message);
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public Student_SettlementDTO fillmerchants(Student_SettlementDTO data)
        {
            try
            {

                List<Student_SettlementDTO> list = new List<Student_SettlementDTO>();

                var list123 = (from a in _FeeGroupContext.FEeGroupLoginPreviledgeDMO
                               where (a.MI_ID == data.MI_Id && a.User_Id == data.user_id)
                               select new Student_SettlementDTO
                               {
                                   FMG_Id = a.FMG_ID
                               }
      ).Distinct().Select(t => t.FMG_Id).ToList();

                list = (from a in _FeeGroupContext.FeeGroupDMO
                        where (a.MI_Id == data.MI_Id && list123.Contains(a.FMG_Id))
                        select new Student_SettlementDTO
                        {
                            user_id = a.user_id
                        }
        ).Distinct().ToList();

                List<long> useridss = new List<long>();

                foreach (var x in list)
                {
                    useridss.Add(x.user_id);
                }


                var merchants = _FeeGroupContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == data.MI_Id && t.FPGD_PGActiveFlag == "1" && useridss.Contains(t.User_id) && t.IMPG_Id==data.IMPG_Id).ToList();
                data.merchantlist = merchants.Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public static String generateCheckSum(String masterKey, Dictionary<String, String> parameters)
        {
            // Validate Input
            validateGenerateCheckSumInput(masterKey);
            Dictionary<string, string> parameter = new Dictionary<string, string>();
            try
            {
                foreach (string key in parameters.Keys)
                {
                    // below code snippet is mandatory, so that no one can use your checksumgeneration url for other purpose .
                    if (parameters[key].Trim().ToUpper().Contains("REFUND") || parameters[key].Trim().Contains("|"))
                    {
                        parameter.Add(key.Trim(), "");
                    }

                    //if (parameters.ContainsKey(key.Trim()))
                    //{
                    //    parameters[key.Trim()] = parameters[key].Trim();
                    //}
                    else
                    {
                        parameter.Add(key.Trim(), parameters[key].Trim());
                    }
                }

                String checkSumParams = SecurityUtils.createCheckSumString(parameter);
                String salt = StringUtils.generateRandomString(Constants.SALT_LENGTH);

                checkSumParams += salt;

                MessageConsole.WriteLine(); MessageConsole.WriteLine("Final CheckSum String:::: " + checkSumParams);
                String hashedCheckSum = SecurityUtils.getHashedString(checkSumParams);

                MessageConsole.WriteLine(); MessageConsole.WriteLine("HashedCheckSum String:::: " + hashedCheckSum);
                hashedCheckSum += salt;

                MessageConsole.WriteLine(); MessageConsole.WriteLine("HashedCheckSum String with Salt:::: " + hashedCheckSum);

                String checkSum = Crypto.Encrypt(hashedCheckSum, masterKey);

                return checkSum;
            }
            catch (Exception e)
            {
                throw new CryptoException("Exception occurred while generating CheckSum. " + e.Message);
            }
        }

        public static String generateCheckSumForRefund(String masterKey, Dictionary<String, String> parameters)
        {
            // Validate Input
            validateGenerateCheckSumInput(masterKey);

            try
            {
                String checkSumParams = SecurityUtils.createCheckSumString(parameters);
                String salt = StringUtils.generateRandomString(Constants.SALT_LENGTH);
                checkSumParams += salt;

                MessageConsole.WriteLine(); MessageConsole.WriteLine("Final CheckSum String:::: " + checkSumParams);
                String hashedCheckSum = SecurityUtils.getHashedString(checkSumParams);

                MessageConsole.WriteLine(); MessageConsole.WriteLine("HashedCheckSum String:::: " + hashedCheckSum);
                hashedCheckSum += salt;

                MessageConsole.WriteLine(); MessageConsole.WriteLine("HashedCheckSum String with Salt:::: " + hashedCheckSum);

                String checkSum = Crypto.Encrypt(hashedCheckSum, masterKey);
                return checkSum;
            }
            catch (Exception e)
            {
                throw new CryptoException("Exception occurred while generating CheckSum. " + e.Message);
            }

        }

        public static String generateCheckSumByJson(String masterKey, string json)
        {
            validateGenerateCheckSumInput(masterKey);

            try
            {
                String checkSumParams = json;
                String salt = StringUtils.generateRandomString(Constants.SALT_LENGTH);
                checkSumParams += Constants.VALUE_SEPARATOR_TOKEN;
                checkSumParams += salt;

                String hashedCheckSum = SecurityUtils.getHashedString(checkSumParams);

                hashedCheckSum += salt;

                String checkSum = Crypto.Encrypt(hashedCheckSum, masterKey);
                return checkSum;
            }
            catch (Exception e)
            {
                throw new CryptoException("Exception occurred while generating CheckSum. " + e.Message);
            }

        }

        private static void validateGenerateCheckSumInput(String masterKey)
        {
            if (masterKey == null)
            {
                //throw new ArgumentNullException("masterKey");
                throw new ArgumentNullException("Parameter cannot be null", "masterKey");
            }

        }

        private static void validateVerifyCheckSumInput(String masterKey, String checkSum)
        {
            if (masterKey == null)
            {
                //throw new ArgumentNullException("masterKey");
                throw new ArgumentNullException("Parameter cannot be null", "masterKey");
            }

            if (checkSum == null)
            {
                throw new ArgumentNullException("Parameter cannot be null", "checkSum");
            }
        }

        public static string Encrypt(String CardDetails, String masterKey)
        {
            return Crypto.Encrypt(CardDetails, masterKey);
        }

        public static String Decrypt(String carddetails, String masterKey)
        {
            return Crypto.Decrypt(carddetails, masterKey);
        }

        public Student_SettlementDTO WEBHOOKS(Student_SettlementDTO data)
        {
            try
            {
                List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();

                paymentdetails = _FeeGroupContext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == 4 && t.FPGD_PGName == "RAZORPAY").Distinct().ToList();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public string Generatehash512(string text)
        {

            byte[] message = Encoding.UTF8.GetBytes(text);

            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;

            var hashString = SHA256.Create();

            //SHA512 hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(message);

            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

        public Student_SettlementDTO paymentlogs(Student_SettlementDTO data)
        {
            try
            {
                List<FeeStudentTransactionDTO> paymentdet = new List<FeeStudentTransactionDTO>();
                paymentdet = (from a in _FeeGroupContext.Fee_M_Online_TransactionDMO
                              where (a.MI_Id == data.MI_Id && a.AMST_Id == data.AMST_Id && a.ASMAY_ID==data.ASMAY_Id)
                              select new FeeStudentTransactionDTO
                              {
                                 FMA_Amount=a.FMOT_Amount,
                                 trans_id = a.FMOT_Trans_Id
                              }
           ).ToList();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
