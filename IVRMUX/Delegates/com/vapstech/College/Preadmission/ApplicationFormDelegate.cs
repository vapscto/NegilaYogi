using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.College.Admission;
using PreadmissionDTOs.com.vaps.College.Preadmission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Preadmission
{
    public class ApplicationFormDelegate
    {
        CommonDelegate<CollegePreadmissionstudnetDto, CollegePreadmissionstudnetDto> common = new CommonDelegate<CollegePreadmissionstudnetDto, CollegePreadmissionstudnetDto>();
        CommonDelegate<PaymentDetails, PaymentDetails> pay = new CommonDelegate<PaymentDetails, PaymentDetails>();
        public CollegePreadmissionstudnetDto Getdetails(CollegePreadmissionstudnetDto obj)
        {
            return common.CollegePOSTData(obj, "ApplicationFormFacade/Getdetails/");
        }
        public CollegePreadmissionstudnetDto getCourse(CollegePreadmissionstudnetDto data)
        {
            return common.CollegePOSTData(data, "ApplicationFormFacade/getCourse/");
        }
        public CollegePreadmissionstudnetDto getBranch(CollegePreadmissionstudnetDto dt)
        {
            return common.CollegePOSTData(dt, "ApplicationFormFacade/getBranch/");
        }

        public PaymentDetails.PAYTM getpaymentresponsepaytm(PaymentDetails.PAYTM rtnData)
        {
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:59069/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(rtnData);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/ApplicationFormFacade/getpaymentresponsePAYTM/", byteContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    rtnData = JsonConvert.DeserializeObject<PaymentDetails.PAYTM>(product, new JsonSerializerSettings
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
        public CollegePreadmissionstudnetDto Dashboarddetails(CollegePreadmissionstudnetDto ctry)
        {
            return common.CollegePOSTData(ctry, "ApplicationFormFacade/Dashboarddetails");
        }
        public CollegePreadmissionstudnetDto paynow(CollegePreadmissionstudnetDto dt)
        {

            return common.CollegePOSTData(dt, "ApplicationFormFacade/paynow/");

        }

        public PaymentDetails getpaymentresponse(PaymentDetails response)
        {

            return pay.CollegePOSTData(response, "ApplicationFormFacade/getpaymentresponse/");

        }
        public CollegePreadmissionstudnetDto getSemester(CollegePreadmissionstudnetDto dto)
        {
            return common.CollegePOSTData(dto, "ApplicationFormFacade/getSemester/");
        }
        public CollegePreadmissionstudnetDto getCaste(CollegePreadmissionstudnetDto dto)
        {
            return common.CollegePOSTData(dto, "ApplicationFormFacade/getCaste/");
        }
        //public AdmMasterCollegeStudentDTO getQuotaCategory(AdmMasterCollegeStudentDTO dto)
        //{
        //    return common.CollegePOSTData(dto, "ApplicationFormFacade/getQuotaCategory/");
        //}
        public CollegePreadmissionstudnetDto saveStudentDetails(CollegePreadmissionstudnetDto dto)
        {
            return common.CollegePOSTData(dto, "ApplicationFormFacade/saveStudentDetails/");
        }
        public CollegePreadmissionstudnetDto Edit(CollegePreadmissionstudnetDto data)
        {
            return common.CollegePOSTData(data, "ApplicationFormFacade/Edit/");
        }
        public CollegePreadmissionstudnetDto getprintdata(CollegePreadmissionstudnetDto data)
        {
            return common.CollegePOSTData(data, "ApplicationFormFacade/getprintdata/");
        }
        //public AdmMasterCollegeStudentDTO checkDuplicate(AdmMasterCollegeStudentDTO data)
        //{
        //    return common.CollegePOSTData(data, "ApplicationFormFacade/checkDuplicate/");
        //}
        public CollegePreadmissionstudnetDto getdpstate(CollegePreadmissionstudnetDto data)
        {
            return common.CollegePOSTData(data, "ApplicationFormFacade/getdpstate/");
        }
        public CollegePreadmissionstudnetDto saveAddress(CollegePreadmissionstudnetDto add)
        {
            return common.CollegePOSTData(add, "ApplicationFormFacade/saveAddress/");
        }
        public CollegePreadmissionstudnetDto saveParentsDetails(CollegePreadmissionstudnetDto ParentsData)
        {
            return common.CollegePOSTData(ParentsData, "ApplicationFormFacade/saveParentsDetails/");
        }
        public CollegePreadmissionstudnetDto StateByCountryName(CollegePreadmissionstudnetDto country)
        {
            return common.CollegePOSTData(country, "ApplicationFormFacade/StateByCountryName/");
        }
        public CollegePreadmissionstudnetDto saveOthersDetails(CollegePreadmissionstudnetDto others)
        {
            return common.CollegePOSTData(others, "ApplicationFormFacade/saveOthersDetails/");
        }
        public CollegePreadmissionstudnetDto saveDocuments(CollegePreadmissionstudnetDto docs)
        {
            return common.CollegePOSTData(docs, "ApplicationFormFacade/saveDocuments/");
        }
        //public AdmMasterCollegeStudentDTO SearchByColumn(AdmMasterCollegeStudentDTO docs)
        //{
        //    return common.CollegePOSTData(docs, "ApplicationFormFacade/SearchByColumn/");
        //}
        //public AdmMasterCollegeStudentDTO DeleteEntry(AdmMasterCollegeStudentDTO docs)
        //{
        //    return common.CollegePOSTData(docs, "ApplicationFormFacade/DeleteEntry/");
        //}

        //master competitive exam
        public CollegePreadmissionstudnetDto compExamName(CollegePreadmissionstudnetDto country)
        {
            return common.CollegePOSTData(country, "ApplicationFormFacade/compExamName/");
        }
        //Razor pay
        public PaymentDetails razorgetpaymentresponse(PaymentDetails response)
        {

            return pay.CollegePOSTData(response, "ApplicationFormFacade/razorgetpaymentresponse/");

        }
        //docs download
        public CollegePreadmissionstudnetDto Clgapplicationstudocs(CollegePreadmissionstudnetDto country)
        {
            return common.CollegePOSTData(country, "ApplicationFormFacade/Clgapplicationstudocs/");
        }
    }
}
