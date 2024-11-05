using System;
using System.Collections.Generic;
using PreadmissionDTOs;
using DomainModel.Model;
using DataAccessMsSqlServerProvider;
using MailKit.Net.Smtp;
using MimeKit;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace CommonLibrary
{

    public class Payment
    {
        private readonly DomainModelMsSqlServerContext _db;
        private readonly ApplicationDBContext _ApplicationDBContext;
        public string action1 = string.Empty;
        public string hash1 = string.Empty;
       // string transaction_response_url = "";

        public Payment(DomainModelMsSqlServerContext db)
        {
            _db = db;
        }

        public Array Values { get; set; }
        //public Array OnlinePayment(string MI_Id, string trans_id, string amount, string MARCHANT_ID, string Seq, string productinfo, string firstname, string email, string AmountSymbol, string SaltKey, string payu_URL, long  mobile,long pasp_id)
        public Array OnlinePayment(PaymentDetails dto)
        {
          
            PaymentDetails temp = new PaymentDetails();
            try
            {
            
                string[] hashVarsSeq;
                string hash_string = string.Empty;

                List<SplitAggregator> paymentParts = new List<SplitAggregator>();
                var dict = new Dictionary<String, SplitAggregator>();


                //for (int i = 0; i < 1; i++)
                //{
                //    dict.Add("splitAgr" + i.ToString(), new SplitAggregator());
                //}
                //int count = 0;
                //foreach (var x in dict.Values)
                //{

                //    x.name = "Online_Pyament_" + dto.trans_id.Trim(); 
                //    x.description = "Online_Pyament_Desc_" + dto.trans_id.Trim();
                //    x.value =Convert.ToString( dto.amount);
                //    x.merchantId = dto.MARCHANT_ID.ToString();
                //    paymentParts.Add(x);
                //    count = count + 1;
                //}

                //string splitjson = JsonConvert.SerializeObject(paymentParts);


                hashVarsSeq = dto.Seq.ToString().Split('|');
                hash_string = "";
                foreach (string hash_var in hashVarsSeq)
                {
                    if (hash_var == "key")
                    {
                        hash_string = hash_string + dto.MARCHANT_ID.ToString();
                        hash_string = hash_string + '|';
                    }
                    else if (hash_var == "txnid")
                    {
                        hash_string = hash_string + dto.trans_id.Trim(); 
                        hash_string = hash_string + '|';
                    }
                    else if (hash_var == "amount")
                    {
                        hash_string = hash_string + Convert.ToDecimal(dto.amount).ToString("g29");
                        hash_string = hash_string + '|';
                    }

                    else if (hash_var == "productinfo")
                    {
                        hash_string = hash_string + (dto.productinfo.Trim());
                        hash_string = hash_string + '|';
                    }

                    //else if (hash_var == "productinfo")
                    //{
                    //    hash_string = hash_string + "{\"paymentParts\":" + JsonConvert.SerializeObject(paymentParts) + "}";
                    //    hash_string = hash_string + '|';
                    //}
                    else if (hash_var == "firstname")
                    {
                        hash_string = hash_string + (dto.firstname.Trim());
                        hash_string = hash_string + '|';
                    }
                    else if (hash_var == "email")
                    {
                        hash_string = hash_string + (dto.email.Trim());
                        hash_string = hash_string + '|';
                    }
                   
                    else if (hash_var == "udf1")
                    {
                        hash_string = hash_string + dto.udf1;
                        hash_string = hash_string + '|';
                    }
                    else if (hash_var == "udf2")
                    {
                        hash_string = hash_string + dto.udf2;
                        hash_string = hash_string + '|';
                    }
                    else if (hash_var == "udf3")
                    {
                        hash_string = hash_string + dto.udf3;
                        hash_string = hash_string + '|';
                    }
                    else if (hash_var == "udf4")
                    {
                        hash_string = hash_string + dto.udf4;
                        hash_string = hash_string + '|';
                    }

                    else if (hash_var == "udf5")
                    {
                        hash_string = hash_string + dto.udf5;
                        hash_string = hash_string + '|';
                    }
                    else if (hash_var == "udf6")
                    {
                        hash_string = hash_string + dto.udf6;
                        hash_string = hash_string + '|';
                    }
                    else
                    {
                        if ( hash_var == "udf7" || hash_var == "udf8" || hash_var == "udf9" || hash_var == "udf10")
                        {
                            hash_string = hash_string + "";
                            hash_string = hash_string + '|';
                        }
                        else
                        {
                            hash_string = hash_string + (hash_var != null ? hash_var : "");
                            hash_string = hash_string + '|';
                        }
                    }
                }

                hash_string += dto.SaltKey.ToString();// appending SALT

                hash1 = Generatehash512(hash_string).ToLower();
                action1 = dto.payu_URL + "/_payment";

                if (!string.IsNullOrEmpty(hash1))
                {
                    temp.SaltKey = dto.SaltKey;
                    temp.hash_string = hash_string;
                    temp.hash = hash1;
                    temp.status = "success";
                    temp.trans_id = dto.trans_id.Trim();
                    temp.amount = Convert.ToDecimal((dto.amount).ToString("g29"));
                    temp.MARCHANT_ID = dto.MARCHANT_ID;
                    temp.udf1 = dto.udf1.Trim();
                    temp.udf2 = dto.udf2.Trim();
                    temp.udf3 = dto.udf3.Trim();
                    temp.udf4 = dto.udf4.Trim();
                    temp.udf5 = dto.udf5.Trim();
                    temp.udf6 = dto.udf6.Trim();
                    temp.firstname = dto.firstname.Trim();
                    temp.email = dto.email.Trim();
                    temp.phone = Convert.ToInt64(dto.phone);
                    temp.productinfo = dto.productinfo.Trim();
                    //temp.productinfo = System.Net.WebUtility.HtmlEncode("{\"paymentParts\":" + JsonConvert.SerializeObject(paymentParts) + "}");
                    temp.surl = dto.transaction_response_url.Trim();
                    temp.furl = dto.transaction_response_url.Trim();
                    temp.service_provider = dto.service_provider.Trim();
                    temp.payu_URL = dto.payu_URL.Trim();

                    //
                    List<PaymentDetails> paydet = new List<PaymentDetails>();
                    paydet.Add(temp);
                    dto.PaymentDetailsList = paydet.ToArray();
                }
                else
                {
                    dto.returnvalue = "";
                }
            }
            catch (Exception ex)
            {
                dto.returnvalue = "";
            }
            return dto.PaymentDetailsList;
        }
      
        public string Generatehash512(string text)
        {

            byte[] message = Encoding.UTF8.GetBytes(text);

            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;

            var hashString = SHA512.Create();

            //SHA512 hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(message);

            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;


        }
        public string Generatehash256(string text)
        {
            byte[] message = Encoding.UTF8.GetBytes(text);

            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;

            var hashString = SHA1.Create();

            string hex = "";
            hashValue = hashString.ComputeHash(message);

            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;


        }

    }
    public class SplitAggregator
    {
        public string name;
        public string description;
        public string value;
        public string merchantId;
    }


}
