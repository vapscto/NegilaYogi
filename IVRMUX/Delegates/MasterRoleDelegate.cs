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
    public class MasterRoleDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterRoleDTO, MasterRoleDTO> COMMM = new CommonDelegate<MasterRoleDTO, MasterRoleDTO>();
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

        public MasterRoleDTO savedetails(MasterRoleDTO maspage)
        {

            return COMMM.POSTData(maspage, "MasterRoleFacade/");

        }

        public MasterRoleDTO deleterec(int id)
        {
            return COMMM.DeleteDataById(id, "MasterRoleFacade/deletedetails/");

        }

        public MasterRoleDTO getdetails(int id)
        {
            return COMMM.GetDataById(id, "MasterRoleFacade/getdetails/");

        }

        public MasterRoleDTO getpagedetails(int id)
        {
            return COMMM.GetDataById(id, "MasterRoleFacade/getpagedetails/");

        }

        public MasterRoleDTO getsearchdata(int data, MasterRoleDTO dataa)
        {
            return COMMM.GETSEarchData(data, dataa, "MasterRoleFacade/1/");

          
        }
    }
}
