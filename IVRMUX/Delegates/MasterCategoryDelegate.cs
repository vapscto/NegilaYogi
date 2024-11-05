using Newtonsoft.Json;
using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class MasterCategoryDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterCategoryDTO, MasterCategoryDTO> COMMM = new CommonDelegate<MasterCategoryDTO, MasterCategoryDTO>();
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
                HttpResponseMessage response = client.GetAsync("api/MasterCategoryFacade/" + resource).Result;
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
        public MasterCategoryDTO savedetails(MasterCategoryDTO mstcatgry)
        {
            return COMMM.POSTData(mstcatgry, "MasterCategoryFacade/");
        }
        public MasterCategoryDTO getAll(int id)
        {
            return COMMM.GetDataById(id, "MasterCategoryFacade/getdata/");
        }

      

        public MasterCategoryDTO deleterec(int id)
        {
            return COMMM.DeleteDataById(id, "MasterCategoryFacade/deletedetails/");
        }

        public MasterCategoryDTO categoryDet(MasterCategoryDTO data)
        {

            return COMMM.POSTData(data, "MasterCategoryFacade/getdetails/");
        }
        public MasterCategoryDTO deactivate(MasterCategoryDTO data)
        {
            return COMMM.POSTData(data, "MasterCategoryFacade/deactivate/");
        }
    }
}
