﻿using Newtonsoft.Json;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
namespace corewebapi18072016.Delegates
{
    public class EBSonlinepaymentDelegates
    {

        private static FacadeUrl fdu = new FacadeUrl();
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        public string getData(long resource)
        {
            string product = "";
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:57606/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // HTTP GET
            try
            {
                HttpResponseMessage response = client.GetAsync("api/FeeClassCategoryFacade/" + resource).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("{0}\t${1}\t{2}", product);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetBaseException());
            }
            return product;
        }
        public FeeStudentTransactionDTO getdetails(FeeStudentTransactionDTO id)
        {
            FeeStudentTransactionDTO orgdet = null;
            string product;
          
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:49540/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/EBSonlinepaymentFacade/getdetails/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    orgdet = JsonConvert.DeserializeObject<FeeStudentTransactionDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            // return output;
            return orgdet;
        }

        public PaymentDetails getpaymentresponse(PaymentDetails rtnData)
        {
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:49540/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(rtnData);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/EBSonlinepaymentFacade/getpaymentresponse/", byteContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    rtnData = JsonConvert.DeserializeObject<PaymentDetails>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return rtnData;
        }

    }
}