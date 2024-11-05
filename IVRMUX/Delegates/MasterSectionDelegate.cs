using Newtonsoft.Json;
using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CommonLibrary;
namespace corewebapi18072016.Delegates
{
    public class MasterSectionDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterSectionDTO, MasterSectionDTO> COMMM = new CommonDelegate<MasterSectionDTO, MasterSectionDTO>();
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
        public MasterSectionDTO savedetails(MasterSectionDTO org)
        {
            return COMMM.POSTData(org, "MasterSectionFacade/");
        }
        public MasterSectionDTO Getmastersectiondetails(int id)
        {
            return COMMM.GetDataById(id, "MasterSectionFacade/getdata/");
        } 
        public MasterSectionDTO DeleteMasterRecord(MasterSectionDTO dto)
        {
            return COMMM.POSTData(dto, "MasterSectionFacade/Deletedetails/");
        }
        public MasterSectionDTO EditDetails(int id)
        {

            return COMMM.GetDataById(id, "MasterSectionFacade/Editdetails/");

          
        }
        public MasterSectionDTO getsearchdata(int data, MasterSectionDTO dataa)
        {
            return COMMM.GETSEarchData(data, dataa, "MasterSectionFacade/1/");         
        }
    }
}
