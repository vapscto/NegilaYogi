using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs;

using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using CommonLibrary;
namespace corewebapi18072016.Delegates
{
    public class OrganisationDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<OrganisationDTO, OrganisationDTO> COMMM = new CommonDelegate<OrganisationDTO, OrganisationDTO>();
        CommonDelegate<OrganisationDTO, SortingPagingInfoDTO> CO = new CommonDelegate<OrganisationDTO, SortingPagingInfoDTO>();
        CommonDelegate<StateDTO, StateDTO> COMMMM = new CommonDelegate<StateDTO, StateDTO>();
        CommonDelegate<CountryDTO, CountryDTO> COMMMMM = new CommonDelegate<CountryDTO, CountryDTO>();
           // CommonDelegate<SortingPagingInfoDTO> COMM = new CommonDelegate<SortingPagingInfoDTO>();

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

        public OrganisationDTO savedetails(OrganisationDTO org)
        {

            return COMMM.POSTData(org, "OrganisationFacade/");
        }

        public OrganisationDTO getcountrydata(OrganisationDTO id)
        {
            return COMMM.POSTData(id, "OrganisationFacade/getalldetails/");
            
        }

        public StateDTO enqdatacountrydrp(int id)
        {
            return COMMMM.GetDataById(id, "OrganisationFacade/getorganisationcontroller/");
        }

        public CountryDTO cityfill(int id)
        {

            return COMMMMM.GetDataById(id, "OrganisationFacade/getorganisationstatecontroller/");
        }

        public OrganisationDTO getcurrency(int id)
        {
            return COMMM.GetDataById(id, "OrganisationFacade/getcurrencydetails/");
        }


        public OrganisationDTO deleterec(int id)
        {

            return COMMM.DeleteDataById(id, "OrganisationFacade/deletedetails/");
        }

        public OrganisationDTO orgdet(int id)
        {
            return COMMM.GetDataById(id, "OrganisationFacade/getdetails/");
            
        }


        public OrganisationDTO getfilterde(int data, OrganisationDTO dataa)
        {

            return COMMM.GETSEarchData(data, dataa, "OrganisationFacade/1/");
            
        }

        public OrganisationDTO getorgSearchedDetails(SortingPagingInfoDTO searchdata)
        {

            return CO.POSTDataa(searchdata, "OrganisationFacade/getOrganisationSearchedDetails/");

            //OrganisationDTO instute = new OrganisationDTO();
            //string product = "";
            //HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("http://localhost:65140/");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            ////HTTP POST
            //try
            //{
            //    var myContent = JsonConvert.SerializeObject(searchdata);
            //    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


            //    var byteContent = new ByteArrayContent(buffer);
            //    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //    var response = client.PostAsync("api/OrganisationFacade/getOrganisationSearchedDetails/", byteContent).Result;

            //    if (response.IsSuccessStatusCode)
            //    {
            //        product = response.Content.ReadAsStringAsync().Result;
            //        Console.WriteLine("", product);
            //        instute = JsonConvert.DeserializeObject<OrganisationDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            //    }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e.Message);
            //}
            //return instute;
        }


    }
}
