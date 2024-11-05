using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs;
using Newtonsoft.Json;
using CommonLibrary;
namespace corewebapi18072016.Delegates
{
    public class MasterPageDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterPageDTO, MasterPageDTO> COMMM = new CommonDelegate<MasterPageDTO, MasterPageDTO>();
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

        public MasterPageDTO savedetails(MasterPageDTO maspage)
        {

            return COMMM.POSTData(maspage, "MasterPageFacade/");
        
        }

        public MasterPageDTO mobilesaveorgdet(MasterPageDTO maspage)
        {

            return COMMM.POSTData(maspage, "MasterPageFacade/mobilesaveorgdet/");

        }

        public MasterPageDTO deleterec(int id)
        {
            return COMMM.DeleteDataById(id, "MasterPageFacade/deletedetails/");

        }

        public MasterPageDTO mobiledeleterec(MasterPageDTO id)
        {
            return COMMM.POSTData(id, "MasterPageFacade/mobiledeleterec/");

        }

        public MasterPageDTO getdetails(int id)
        {

            return COMMM.GetDataById(id, "MasterPageFacade/getdetails/");


          
        }
     
        public MasterPageDTO getalldetailsmobile(int id)
        {

            return COMMM.GetDataById(id, "MasterPageFacade/getalldetailsmobile/");



        }

        public MasterPageDTO getpagedetails(int id)
        {

            return COMMM.GetDataById(id, "MasterPageFacade/getpagedetails/");


       
        }

        public MasterPageDTO mobilegetdetails(int id)
        {

            return COMMM.GetDataById(id, "MasterPageFacade/mobilegetdetails/");



        }

        public MasterPageDTO getsearchdata(int data, MasterPageDTO dataa)
        {

            return COMMM.GETSEarchData(data, dataa, "MasterPageFacade/1/");

        }

    }
}
