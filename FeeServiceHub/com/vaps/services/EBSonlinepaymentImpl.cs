using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using FeeServiceHub.com.vaps.interfaces;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.IO;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;
using MimeKit.Cryptography;
using System.Security.Cryptography;
using static PreadmissionDTOs.PaymentDetails;
using CommonLibrary;

namespace FeeServiceHub.com.vaps.services
{
    public class EBSonlinepaymentImpl : interfaces.EBSonlinepaymentInterface
    {

        public DomainModelMsSqlServerContext _context;
        private static ConcurrentDictionary<string, FeeClassCategoryDTO> _login =
        new ConcurrentDictionary<string, FeeClassCategoryDTO>();

        private static ConcurrentDictionary<string, FeeYearlyClassCategoryDTO> _login1 =
             new ConcurrentDictionary<string, FeeYearlyClassCategoryDTO>();

        public FeeGroupContext _FeeGroupContext;
        readonly ILogger<FeeGroupImplimentation> _logger;
        public EBSonlinepaymentImpl(FeeGroupContext frgContext, ILogger<FeeGroupImplimentation> log, DomainModelMsSqlServerContext context)
        {
            _FeeGroupContext = frgContext;
            _logger = log;
            _context = context;
        }
       
        public FeeStudentTransactionDTO getdetails(FeeStudentTransactionDTO org)
        {
            org.paydet = paymentPart(org, org.topayamount);
            return org;

        }
        public Array paymentPart(FeeStudentTransactionDTO enq, long totamount)
        {


            EBSpayment paymentd = new EBSpayment();
            paymentd.channel = 0;
            paymentd.account_id = 25914;
            paymentd.reference_no = "12347";
            paymentd.amount = "12.34";
            paymentd.ba_B25914CA4 = "";
            paymentd.ba_B259148DC = "6";
            paymentd.split_profile_code = "";

            paymentd.mode = "TEST";
            paymentd.currency = "INR";
            paymentd.description = "FIRST TEST TRANSACTION";
            paymentd.return_url = "https://stagingcampusux.azurewebsites.net/EBSonlinepayment/paymentresponse";
            //paymentd.return_url = "http://localhost:57606/FeeOnlinePayment/paymentresponse";
            paymentd.name = "KAVITHA";
            paymentd.address = "BANGALORE";
            paymentd.city = "BANGALORE";
            paymentd.state = "KARNATAKA";
            paymentd.country = "IND";
            paymentd.postal_code = "123456";
            paymentd.phone = 9080448914;
            paymentd.email = "KAVITHA@VAPSTECH.COM";
            paymentd.ship_name = "KAVITHA";
            paymentd.ship_address = "BANGALORE";
            paymentd.ship_city = "BANGALORE";
            paymentd.ship_state = "KARNATAKA";
            paymentd.ship_country = "IND";
            paymentd.ship_postal_code = "123456";
            paymentd.ship_phone = 9080448914;
            // paymentd.algo = "SHA512";

            // paymentd.name_on_card = "TEST";
            //  paymentd.card_number = 4111111111111111;
            // paymentd.card_expiry = 0716;
            // paymentd.card_cvv = 123;

            string hashValue = "a79a25f201354535a085129481eda537";
            string V3URL = "https://secure.ebs.in/pg/ma/payment/request";
            string HashData = hashValue;

            List<KeyValuePair<string, string>> postparamslist = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("account_id", paymentd.account_id.ToString()),
                new KeyValuePair<string, string>("channel", paymentd.channel.ToString()),
                new KeyValuePair<string, string>("currency", paymentd.currency),
                new KeyValuePair<string, string>("reference_no", paymentd.reference_no),
                new KeyValuePair<string, string>("amount",Convert.ToString(paymentd.amount)),
              //  new KeyValuePair<string, string>("ba_B25914CA4",paymentd.ba_B25914CA4),
                new KeyValuePair<string, string>("ba_B259148DC",paymentd.ba_B259148DC),
            //    new KeyValuePair<string, string>("split_profile_code",paymentd.split_profile_code),
                new KeyValuePair<string, string>("description", paymentd.description),
                new KeyValuePair<string, string>("name",paymentd.name),
                new KeyValuePair<string, string>("address", paymentd.address),
                new KeyValuePair<string, string>("city",paymentd.city),
                new KeyValuePair<string, string>("state", paymentd.state),
                new KeyValuePair<string, string>("postal_code", paymentd.postal_code),
                new KeyValuePair<string, string>("country", paymentd.country),
                new KeyValuePair<string, string>("email",paymentd.email),
                new KeyValuePair<string, string>("phone",Convert.ToString(paymentd.phone)),
                new KeyValuePair<string, string>("mode", paymentd.mode),
                new KeyValuePair<string, string>("return_url", paymentd.return_url),
                new KeyValuePair<string, string>("ship_name", paymentd.ship_name),
                new KeyValuePair<string, string>("ship_address", paymentd.ship_address),
                new KeyValuePair<string, string>("ship_city", paymentd.ship_city),
                new KeyValuePair<string, string>("ship_state", paymentd.ship_state),
                new KeyValuePair<string, string>("ship_country", paymentd.ship_country),
                new KeyValuePair<string, string>("ship_phone",Convert.ToString(paymentd.ship_phone)),
               // new KeyValuePair<string, string>("algo", paymentd.algo),
                new KeyValuePair<string, string>("ship_postal_code", paymentd.ship_postal_code),
              //  new KeyValuePair<string, string>("name_on_card", paymentd.name_on_card),
              //  new KeyValuePair<string, string>("card_number",Convert.ToString(paymentd.card_number)),
               // new KeyValuePair<string, string>("card_expiry",Convert.ToString(paymentd.card_expiry)),
              //  new KeyValuePair<string, string>("card_cvv",Convert.ToString(paymentd.card_cvv))
            };

            postparamslist.Sort(Compare1);

            foreach (KeyValuePair<string, string> param in postparamslist)
            {
                HashData += "|" + param.Value;
            }

            string hashedvalue = "";

            if (hashValue.Length > 0)
            {
                hashedvalue += computeHash(HashData, "SHA512");
            }
            //var secure_hash = hashedvalue;

            //  postparamslist.Add(new KeyValuePair<string, string>("secure_hash", hashedvalue));
            // postparamslist.Add(new KeyValuePair<string, string>("V3URL", V3URL));
            paymentd.secure_hash = hashedvalue;
            paymentd.V3URL = V3URL;
            List<EBSpayment> pds = new List<EBSpayment>();
            pds.Add(paymentd);
            return pds.ToArray();

        }
        static int Compare1(KeyValuePair<string, string> a, KeyValuePair<string, string> b)
        {
            return a.Key.CompareTo(b.Key);
        }
        static string computeHash(string input, string name)
        {

            byte[] data = null;
            switch (name)
            {

                //case "MD5":
                //    data = MD5.Create().ComputeHash(Encoding.ASCII.GetBytes(input));
                //    break;
                case "SHA1":
                    data = SHA1.Create().ComputeHash(Encoding.ASCII.GetBytes(input));
                    break;
                case "SHA512":
                    data = SHA512.Create().ComputeHash(Encoding.ASCII.GetBytes(input));
                    break;
                default:
                    break;
            }

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString().ToUpper();

        }
        public PaymentDetails payuresponse(PaymentDetails response)
        {
            //if (response.status == "success")
            //{

            //}
            //    if (Request.HttpMethod == "GET")
            //{
            //    List<KeyValuePair<string, string>> postparamslist = new List<KeyValuePair<string, string>>();

            //    for (int i = 0; i < Request.QueryString.Keys.Count; i++)
            //    {
            //        KeyValuePair<string, string> postparam = new KeyValuePair<string, string>(Request.QueryString.Keys[i], Request.QueryString[i]);


            //        postparamslist.Add(postparam);
            //    }


            //    Response.Write("<div style='background-color:white; box-shadow:5px 5px 5px 5px'>");
            //    Response.Write("<center><div style='background-color:lightblue'><h1> Response Details</h1><br></div><table width=600px>");

            //    foreach (KeyValuePair<string, string> param in postparamslist)
            //    {

            //        Response.Write("<tr><td>" + param.Key + "</td><td>" + param.Value + "</td></tr>");

            //    }

            //    Response.Write("</table></center>");

            //}
            //else
            //{



            //    List<KeyValuePair<string, string>> postparamslist = new List<KeyValuePair<string, string>>();

            //    for (int i = 0; i < Request.Form.Keys.Count; i++)
            //    {
            //        KeyValuePair<string, string> postparam = new KeyValuePair<string, string>(Request.Form.Keys[i], Request.Form[i]);


            //        postparamslist.Add(postparam);
            //    }


            //    Response.Write("<div style='background-color:white; box-shadow:5px 5px 5px 5px'>");
            //    Response.Write("<center><div style='background-color:lightblue'><h1> Response Details</h1><br></div><table width=600px>");

            //    foreach (KeyValuePair<string, string> param in postparamslist)
            //    {

            //        Response.Write("<tr><td>" + param.Key + "</td><td>" + param.Value + "</td></tr>");

            //    }

            //    Response.Write("</table></center>");

            //}
            return response;
        }
        
    }
}
