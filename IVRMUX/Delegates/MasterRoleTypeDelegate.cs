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
    public class MasterRoleTypeDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterRoleTypeDTO, MasterRoleTypeDTO> COMMM = new CommonDelegate<MasterRoleTypeDTO, MasterRoleTypeDTO>();
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

        public MasterRoleTypeDTO savedetails(MasterRoleTypeDTO maspage)
        {
            return COMMM.POSTData(maspage, "MasterRoleTypeFacade/");
        }

        public MasterRoleTypeDTO deleterec(int id)
        {
            return COMMM.DeleteDataById(id, "MasterRoleTypeFacade/deletedetails/");
        }

        public MasterRoleTypeDTO getdetails(int id)
        {
            return COMMM.GetDataById(id, "MasterRoleTypeFacade/getdetails/");
        }

        public MasterRoleTypeDTO getpagedetails(int id)
        {
            return COMMM.GetDataById(id, "MasterRoleTypeFacade/getpagedetails/");
        }

        public MasterRoleTypeDTO getsearchdata(int data, MasterRoleTypeDTO dataa)
        {
            return COMMM.GETSEarchData(data, dataa, "MasterRoleTypeFacade/1/");
        }
    }
}
