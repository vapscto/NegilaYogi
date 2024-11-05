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
using corewebapi18072016;

namespace IVRMUX.Delegates.com.vapstech.College.Preadmission
{
    public class CollegeStudentappDelegate 
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
        CommonDelegate<PaymentDetails, PaymentDetails> pay = new CommonDelegate<PaymentDetails, PaymentDetails>();
        CommonDelegate<StudentHelthcertificateDTO, StudentHelthcertificateDTO> COMHEL = new CommonDelegate<StudentHelthcertificateDTO, StudentHelthcertificateDTO>();
        // empty Constructor, dnt delete please
        public CollegeStudentappDelegate() { }

        public CollegeStudentappDelegate(FacadeUrl config)
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

            return COM.CollegePOSTData(stu, "CollegeStudentappFacade/searchdata");

        }

        public StudentApplicationDTO savestudentdetails(StudentApplicationDTO stu)
        {
            return COM.CollegePOSTData(stu, "CollegeStudentappFacade/");


        }

        public CountryDTO getcountrydata(int id)
        {
            CountryDTO dto = null;
            return COMM.CollegeGetDataByIdNo(id, dto, "CollegeStudentappFacade/");


        }

        // Added on 19-9-2016
        public StudentApplicationDTO getStudentEditData(StudentApplicationDTO dt)
        {

            return COM.CollegePOSTData(dt, "CollegeStudentappFacade/getStudentEditDataFacades/");

        }

        public StudentApplicationDTO paynow(StudentApplicationDTO dt)
        {

            return COM.CollegePOSTData(dt, "CollegeStudentappFacade/paynow/");

        }
        public StudentApplicationDTO getstudentprintdata(StudentApplicationDTO dt)
        {

            return COM.CollegePOSTData(dt, "CollegeStudentappFacade/getstudentprintDataFacades/");

        }

        public StudentApplicationDTO getdashboardpage(StudentApplicationDTO dt)
        {

            return COM.CollegePOSTData(dt, "CollegeStudentappFacade/getdashboardpage/");

        }
        public PaymentDetails getpaymentresponse(PaymentDetails response)
        {

            return pay.CollegePOSTData(response, "CollegeStudentappFacade/getpaymentresponse/");

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
                var response = client.DeleteAsync("api/CollegeStudentappFacade/deletedetails/" + id).Result;

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
            return COMM.CollegePOSTData(ctry, "CollegeStudentappFacade/country");


        }

        public CountryDTO ActivateDactivate(CountryDTO ctry)
        {
            return COMM.CollegePOSTData(ctry, "CollegeStudentappFacade/ActivateDactivate");


        }

        public CountryDTO Getcountofstudents(CountryDTO ctry)
        {
            return COMM.CollegePOSTData(ctry, "CollegeStudentappFacade/Getcountofstudents");


        }

        public CountryDTO Dashboarddetails(CountryDTO ctry)
        {
            return COMM.CollegePOSTData(ctry, "CollegeStudentappFacade/Dashboarddetails");


        }

        public CityDTO getCityByState(int id)
        {
            return COMMM.CollegeGetDataById(id, "CollegeStudentappFacade/getdpcities/");


        }


        public CityDTO getCityByCountry(int id)
        {

            return COMMM.CollegeGetDataById(id, "CollegeStudentappFacade/getdpcity/");


        }

        //routes
        public StateDTO getroutes(int id)
        {

            return COMMMM.CollegeGetDataById(id, "CollegeStudentappFacade/getroutes/");


        }

        public StateDTO getrouteslocation(int id)
        {

            return COMMMM.CollegeGetDataById(id, "CollegeStudentappFacade/getrouteslocation/");


        }

        //State
        public StateDTO getStateByCountry(int id)
        {

            return COMMMM.CollegeGetDataById(id, "CollegeStudentappFacade/getdpstate/");


        }

        public StateDTO getdprospectusdetails(int id)
        {

            return COMMMM.CollegeGetDataById(id, "CollegeStudentappFacade/getdprospectusdetails/");


        }

        public StateDTO getdpstatesubcatse(int id)
        {

            return COMMMM.CollegeGetDataById(id, "CollegeStudentappFacade/getdpstatesubcatse/");


        }

        public StateDTO getdpstatesubcatsefather(int id)
        {

            return COMMMM.CollegeGetDataById(id, "CollegeStudentappFacade/getdpstatesubcatsefather/");


        }

        public StudentApplicationDTO getapplicationhtml(StudentApplicationDTO enqdto)
        {
            return COM.CollegePOSTData(enqdto, "CollegeStudentappFacade/getapplicationhtml");
        }

        public StateDTO getdpstatesubcatsemother(int id)
        {

            return COMMMM.CollegeGetDataById(id, "CollegeStudentappFacade/getdpstatesubcatsemother/");


        }

        public StudentApplicationDTO getmaxminage(StudentApplicationDTO enqdto)
        {
            return COM.CollegePOSTData(enqdto, "CollegeStudentappFacade/classchangemaxminage");


        }

        public StudentApplicationDTO GetSubjectsofinstitute(StudentApplicationDTO enqdto)
        {
            return COM.CollegePOSTData(enqdto, "CollegeStudentappFacade/GetSubjectsofinstitute");
        }


        public StudentHelthcertificateDTO savehealthcertificatedetail(StudentHelthcertificateDTO enqdto)
        {
            return COMHEL.CollegePOSTData(enqdto, "CollegeStudentappFacade/savehealthcertificatedetail");


        }
        public StudentHelthcertificateDTO getstudata(StudentHelthcertificateDTO ctry)
        {
            return COMHEL.CollegePOSTData(ctry, "CollegeStudentappFacade/getstudata");


        }
        public StudentHelthcertificateDTO getEdithelthData(StudentHelthcertificateDTO dt)
        {

            return COMHEL.CollegePOSTData(dt, "CollegeStudentappFacade/getEdithelthData/");

        }
        public StudentHelthcertificateDTO deletehelthdetails(StudentHelthcertificateDTO dt)
        {

            return COMHEL.CollegePOSTData(dt, "CollegeStudentappFacade/deletehelthdetails/");

        }
        public StudentHelthcertificateDTO printgethelthData(StudentHelthcertificateDTO dt)
        {

            return COMHEL.CollegePOSTData(dt, "CollegeStudentappFacade/printgethelthData/");

        }

        public StudentApplicationDTO fill_prospectus(StudentApplicationDTO data)
        {
            return COM.CollegePOSTData(data, "CollegeStudentappFacade/fill_prospectus/");
        }
    }
}
