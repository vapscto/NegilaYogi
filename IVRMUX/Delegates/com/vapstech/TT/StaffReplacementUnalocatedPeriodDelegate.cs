﻿using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.TT
{
    public class StaffReplacementUnalocatedPeriodDelegate
    {
        public StaffReplacementUnalocatedPeriodDTO getdetails(int id)
        {
            StaffReplacementUnalocatedPeriodDTO orgdet = null;
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:52949/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/StaffReplacementUnalocatedPeriodFacade/getdetails/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    orgdet = JsonConvert.DeserializeObject<StaffReplacementUnalocatedPeriodDTO>(product, new JsonSerializerSettings
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
        public StaffReplacementUnalocatedPeriodDTO get_catg(StaffReplacementUnalocatedPeriodDTO categorypage)
        {
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:52949/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(categorypage);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/StaffReplacementUnalocatedPeriodFacade/get_catg", byteContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);
                    categorypage = JsonConvert.DeserializeObject<StaffReplacementUnalocatedPeriodDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch
            {

            }
            return categorypage;
        }
        public StaffReplacementUnalocatedPeriodDTO getrpt(StaffReplacementUnalocatedPeriodDTO categorypage)
        {
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:52949/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(categorypage);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/StaffReplacementUnalocatedPeriodFacade/getrpt", byteContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);
                    categorypage = JsonConvert.DeserializeObject<StaffReplacementUnalocatedPeriodDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch
            {

            }
            return categorypage;
        }
    }
}