﻿using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.BirthDay;
//using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.BirthDay
{
    public class BirthDayDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<BirthDayDTO, BirthDayDTO> COMFRNT = new CommonDelegate<BirthDayDTO, BirthDayDTO>();
       
        //public BirthDayDTO getdata(int id)
        //{
        //    return COMFRNT.GetDataByIdFROFF(id, "BirthDayFacade/getdata/");

        //}
        public BirthDayDTO getdata(int id)
        {
            BirthDayDTO returnData = null;
            string product;
            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri("http://localhost:54009/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/BirthDayFacade/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    returnData = JsonConvert.DeserializeObject<BirthDayDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects,
                        DateFormatHandling = DateFormatHandling.IsoDateFormat
                    });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            // return output;
            return returnData;
        }

        public BirthDayDTO getlistthree(BirthDayDTO student)
        {

            return COMFRNT.POSTDataBirthday(student, "BirthDayFacade/getS/");

        }
        public BirthDayDTO staflist(BirthDayDTO staff)
        {

            return COMFRNT.POSTDataBirthday(staff, "BirthDayFacade/BindStaff/");
        }
        public BirthDayDTO staflist1(BirthDayDTO staff)
        {

            return COMFRNT.POSTDataBirthday(staff, "BirthDayFacade/Sendsms/");


        }
        public BirthDayDTO Sendmsg(BirthDayDTO msg)
        {

            return COMFRNT.POSTDataBirthday(msg, "BirthDayFacade/Sendmsg/");


        }
        public BirthDayDTO getReport(BirthDayDTO rpt)
        {
            return COMFRNT.POSTDataBirthday(rpt, "BirthDayFacade/getReport/");
        }

        public BirthDayDTO getEmailSMSCount(BirthDayDTO rpt)
        {
            return COMFRNT.POSTDataBirthday(rpt, "BirthDayFacade/getEmailSMSCount/");
        }
        public BirthDayDTO SearchByColumn(BirthDayDTO data)
        {
            return COMFRNT.POSTDataBirthday(data, "BirthDayFacade/SearchByColumn/");
        }

        public BirthDayDTO getmonthreport(BirthDayDTO rpt)
        {
            return COMFRNT.POSTDataBirthday(rpt, "BirthDayFacade/getmonthreport/");
        }

        public BirthDayDTO getdetails(BirthDayDTO data)
        {
            return COMFRNT.POSTDataBirthday(data, "BirthDayFacade/getstaffdetails/");
        }

        public BirthDayDTO Whatsapp(BirthDayDTO data)
        {
            return COMFRNT.POSTDataBirthday(data, "BirthDayFacade/Whatsapp");
        }

    }
}
