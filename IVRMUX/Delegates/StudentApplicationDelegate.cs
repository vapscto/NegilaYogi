using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs;

using Newtonsoft.Json;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Extensions.Options;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class StudentApplicationDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        private readonly FacadeUrl _config;
        private static FacadeUrl fdu = new FacadeUrl();
        CommonDelegate<StudentApplicationDTO, StudentApplicationDTO> COM = new CommonDelegate<StudentApplicationDTO, StudentApplicationDTO>();
        CommonDelegate<CountryDTO, CountryDTO> COMM = new CommonDelegate<CountryDTO, CountryDTO>();
        CommonDelegate<CityDTO, CityDTO> COMMM = new CommonDelegate<CityDTO, CityDTO>();
        CommonDelegate<StateDTO, StateDTO> COMMMM = new CommonDelegate<StateDTO, StateDTO>();
        CommonDelegate<DistrictDTO, DistrictDTO> COMMMMM = new CommonDelegate<DistrictDTO, DistrictDTO>();
        CommonDelegate<PaymentDetails, PaymentDetails> pay = new CommonDelegate<PaymentDetails, PaymentDetails>();
        CommonDelegate<StudentHelthcertificateDTO, StudentHelthcertificateDTO> COMHEL = new CommonDelegate<StudentHelthcertificateDTO, StudentHelthcertificateDTO>();
        // empty Constructor, dnt delete please
        public StudentApplicationDelegate() { }

        public StudentApplicationDelegate(FacadeUrl config)
        {
            _config = config;
            fdu = config;
        }

        public string getData(long resource)
        {
            string product = "";
            HttpClient client = new HttpClient();
            
            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

            // HTTP GET
            try
            {
                HttpResponseMessage response = client.GetAsync("api/PreadmissionFacade/" + resource).Result;
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

        public StudentApplicationDTO searchData(StudentApplicationDTO stu)
        {

            return COM.POSTData(stu, "StudentApplicationFacade/searchdata");

        }

        public StudentApplicationDTO savestudentdetails(StudentApplicationDTO stu)
        {
            return COM.POSTData(stu, "StudentApplicationFacade/");

        
        }

        public CountryDTO getcountrydata(int id)
        {
            CountryDTO dto = null;
            return COMM.GetDataByIdNo(id, dto, "StudentApplicationFacade/");

           
        }

        // Added on 19-9-2016
        public StudentApplicationDTO getStudentEditData(StudentApplicationDTO dt)
        {

            return COM.POSTData(dt, "StudentApplicationFacade/getStudentEditDataFacades/");

        }

        public StudentApplicationDTO paynow (StudentApplicationDTO dt)
        {

            return COM.POSTData(dt, "StudentApplicationFacade/paynow/");

        }
        public StudentApplicationDTO getstudentprintdata(StudentApplicationDTO dt)
        {

            return COM.POSTData(dt, "StudentApplicationFacade/getstudentprintDataFacades/");

        }

        public StudentApplicationDTO getdashboardpage(StudentApplicationDTO dt)
        {

            return COM.POSTData(dt, "StudentApplicationFacade/getdashboardpage/");

        }
        public StudentApplicationDTO getemployees(StudentApplicationDTO dt)
        {

            return COM.POSTData(dt, "StudentApplicationFacade/getemployees/");

        }
        public StudentApplicationDTO classchange(StudentApplicationDTO dt)
        {

            return COM.POSTData(dt, "StudentApplicationFacade/classchange/");

        }
        
        public PaymentDetails getpaymentresponse(PaymentDetails response)
        {

            return pay.POSTData(response, "StudentApplicationFacade/getpaymentresponse/");

        }
        public PaymentDetails razorgetpaymentresponse(PaymentDetails response)
        {

            return pay.POSTData(response, "StudentApplicationFacade/razorgetpaymentresponse/");

        }

        public PaymentDetails.PAYTM getpaymentresponsepaytm(PaymentDetails.PAYTM rtnData)
        {
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(rtnData);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/StudentApplicationFacade/getpaymentresponsePAYTM/", byteContent).Result;
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

        public PaymentDetails.easybuzz getpaymentresponseeasybuzz(PaymentDetails.easybuzz rtnData)
        {

            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(rtnData);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/StudentApplicationFacade/getpaymentresponseeasybuzz/", byteContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    rtnData = JsonConvert.DeserializeObject<PaymentDetails.easybuzz>(product, new JsonSerializerSettings
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


        public void deleterec(int id)
        {
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.DeleteAsync("api/StudentApplicationFacade/deletedetails/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    //enqdto = JsonConvert.DeserializeObject<InstitutionDTO>(product, new JsonSerializerSettings
                    //{
                    //    TypeNameHandling = TypeNameHandling.Objects
                    //});

                    //enqdto = JsonConvert.DeserializeObject<InstitutionDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            // return output;
            //return enqdto;
        }
        public CountryDTO getIndependentDropDowns(CountryDTO ctry)
        {
            return COMM.POSTData(ctry, "StudentApplicationFacade/country");

           
        }

        public CountryDTO ActivateDactivate(CountryDTO ctry)
        {
            return COMM.POSTData(ctry, "StudentApplicationFacade/ActivateDactivate");


        }

        public CountryDTO Getcountofstudents(CountryDTO ctry)
        {
            return COMM.POSTData(ctry, "StudentApplicationFacade/Getcountofstudents");


        }

        public CountryDTO Dashboarddetails(CountryDTO ctry)
        {
            return COMM.POSTData(ctry, "StudentApplicationFacade/Dashboarddetails");


        }

        public CityDTO getCityByState(int id)
        {
            return COMMM.GetDataById(id, "StudentApplicationFacade/getdpcities/");

           
        }


        public CityDTO getCityByCountry(int id)
        {

            return COMMM.GetDataById(id, "StudentApplicationFacade/getdpcity/");

           
        }

        //routes
        public StateDTO getroutes(int id)
        {

            return COMMMM.GetDataById(id, "StudentApplicationFacade/getroutes/");


        }

        public StateDTO getrouteslocation(int id)
        {

            return COMMMM.GetDataById(id, "StudentApplicationFacade/getrouteslocation/");


        }

        //State
        public StateDTO getStateByCountry(int id)
        {

            return COMMMM.GetDataById(id, "StudentApplicationFacade/getdpstate/");

           
        }
        //district
        public DistrictDTO getDistrictByState(int id)
        {

            return COMMMMM.GetDataById(id, "StudentApplicationFacade/getdpdistrict/");


        }

        

        public StateDTO getdprospectusdetails(int id)
        {

            return COMMMM.GetDataById(id, "StudentApplicationFacade/getdprospectusdetails/");


        }

        public StateDTO getdpstatesubcatse(int id)
        {

            return COMMMM.GetDataById(id, "StudentApplicationFacade/getdpstatesubcatse/");


        }

        public StateDTO getdpstatesubcatsefather(int id)
        {

            return COMMMM.GetDataById(id, "StudentApplicationFacade/getdpstatesubcatsefather/");


        }

        public StudentApplicationDTO getapplicationhtml(StudentApplicationDTO enqdto)
        {
            return COM.POSTData(enqdto, "StudentApplicationFacade/getapplicationhtml");
        }

        public StateDTO getdpstatesubcatsemother(int id)
        {

            return COMMMM.GetDataById(id, "StudentApplicationFacade/getdpstatesubcatsemother/");


        }

        public StudentApplicationDTO getmaxminage(StudentApplicationDTO enqdto)
        {
            return COM.POSTData(enqdto, "StudentApplicationFacade/classchangemaxminage");

          
        }
        public StudentApplicationDTO classoverunderage(StudentApplicationDTO enqdto)
        {
            return COM.POSTData(enqdto, "StudentApplicationFacade/classoverunderage");


        }

        public StudentApplicationDTO GetSubjectsofinstitute(StudentApplicationDTO enqdto)
        {
            return COM.POSTData(enqdto, "StudentApplicationFacade/GetSubjectsofinstitute");
        }

        public StudentApplicationDTO getstreams(StudentApplicationDTO enqdto)
        {
            return COM.POSTData(enqdto, "StudentApplicationFacade/getstreams");
        }


        public StudentHelthcertificateDTO savehealthcertificatedetail(StudentHelthcertificateDTO enqdto)
        {
            return COMHEL.POSTData(enqdto, "StudentApplicationFacade/savehealthcertificatedetail");


        }
        public StudentHelthcertificateDTO getstudata(StudentHelthcertificateDTO ctry)
        {
            return COMHEL.POSTData(ctry, "StudentApplicationFacade/getstudata");


        }
        public StudentHelthcertificateDTO getEdithelthData(StudentHelthcertificateDTO dt)
        {

            return COMHEL.POSTData(dt, "StudentApplicationFacade/getEdithelthData/");

        }
        public StudentHelthcertificateDTO deletehelthdetails(StudentHelthcertificateDTO dt)
        {

            return COMHEL.POSTData(dt, "StudentApplicationFacade/deletehelthdetails/");

        }
        public StudentHelthcertificateDTO printgethelthData(StudentHelthcertificateDTO dt)
        {

            return COMHEL.POSTData(dt, "StudentApplicationFacade/printgethelthData/");

        }

        public StudentApplicationDTO fill_prospectus(StudentApplicationDTO data)
        {
            return COM.POSTData(data, "StudentApplicationFacade/fill_prospectus/");
        }



    }
}
