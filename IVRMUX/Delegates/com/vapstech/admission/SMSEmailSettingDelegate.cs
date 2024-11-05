using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.admission;
using PreadmissionDTOs;
using CommonLibrary;
namespace corewebapi18072016.com.vaps.admission.Delegates
{
    public class SMSEmailSettingDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<SMSEmailSettingDTO, SMSEmailSettingDTO> COMMM = new CommonDelegate<SMSEmailSettingDTO, SMSEmailSettingDTO>();
        CommonDelegate<SmsEmailDTO, SmsEmailDTO> COMMMM = new CommonDelegate<SmsEmailDTO, SmsEmailDTO>();
        CommonDelegate<SMS_MAIL_PARAMETER_DTO, SMS_MAIL_PARAMETER_DTO> COMMMMN = new CommonDelegate<SMS_MAIL_PARAMETER_DTO, SMS_MAIL_PARAMETER_DTO>();

        public string getData(long resource)
        {
            string product = "";
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // HTTP GET
            try
            {
                HttpResponseMessage response = client.GetAsync("api/SMSEmailSettingFacade/" + resource).Result;
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
        public SMSEmailSettingDTO getSmsEmailSetting(SMSEmailSettingDTO smsDto)
        {

            return COMMM.POSTDataADM(smsDto, "SMSEmailSettingFacade/getSmsEmailSetting/");

        }
        public SMSEmailSettingDTO getmodulePage(SMSEmailSettingDTO smsDto)
        {
            return COMMM.POSTDataADM(smsDto, "SMSEmailSettingFacade/getmodulePage/");

        }
        public SMSEmailSettingDTO getHeader(SMSEmailSettingDTO sms)
        {

            return COMMM.POSTDataADM(sms, "SMSEmailSettingFacade/");

        }
        public SmsEmailDTO saveSmsEmailSettings(SmsEmailDTO smsemail)
        {

            return COMMMM.POSTDataADM(smsemail, "SMSEmailSettingFacade/saveEmailSetting/");

        }
        public SmsEmailDTO deleterec(int id)
        {
           
            return COMMMM.DeleteDataByIdADM(id, "SMSEmailSettingFacade/deletedetails/");
            

        }

        public SmsEmailDTO editDetails(int id)
        {
            return COMMMM.GetDataByIdADM(id, "SMSEmailSettingFacade/getdetails/");
            
        }
        public SMS_MAIL_PARAMETER_DTO parameter(int id)
        {
            return COMMMMN.GetDataByIdADM(id, "SMSEmailSettingFacade/parameter/");

        }
        public SmsEmailDTO activedeactivesms(SmsEmailDTO data)
        {
            return COMMMM.POSTDataADM(data, "SMSEmailSettingFacade/activedeactivesms/"); 
        }
        public SmsEmailDTO activedeactiveemail(SmsEmailDTO data)
        {
            return COMMMM.POSTDataADM(data, "SMSEmailSettingFacade/activedeactiveemail/");
        }
        public SmsEmailDTO viewtempate(SmsEmailDTO data)
        {
            return COMMMM.POSTDataADM(data, "SMSEmailSettingFacade/viewtempate/");
        }
        
    }
}
