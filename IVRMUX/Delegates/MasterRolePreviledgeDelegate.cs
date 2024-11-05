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
    public class MasterRolePreviledgeDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterRolePreviledgeDTO, MasterRolePreviledgeDTO> COMMM = new CommonDelegate<MasterRolePreviledgeDTO, MasterRolePreviledgeDTO>();
        public MasterRolePreviledgeDTO getmoduledet(int id)
        {
            MasterRolePreviledgeDTO pgmod = null;
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/MasterRolePreviledgeFacade/getmoduledetails/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    pgmod = JsonConvert.DeserializeObject<MasterRolePreviledgeDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return pgmod;
        }


        public MasterRolePreviledgeDTO mobilegetalldetails(int id)
        {
            MasterRolePreviledgeDTO pgmod = null;
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/MasterRolePreviledgeFacade/mobilegetalldetails/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    pgmod = JsonConvert.DeserializeObject<MasterRolePreviledgeDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return pgmod;
        }

        public MasterRolePreviledgeDTO getselectedpg(int id)
        {
            return COMMM.GetDataById(id, "MasterRolePreviledgeFacade/getmoduledetails/");
        }
        public MasterRolePreviledgeDTO mobilegetmodulepages(MasterRolePreviledgeDTO id)
        {
            return COMMM.POSTData(id, "MasterRolePreviledgeFacade/mobilegetmodulepages/");
        }

        public MasterRolePreviledgeDTO savedetails(MasterRolePreviledgeDTO pgmod)
        {
            return COMMM.POSTData(pgmod, "MasterRolePreviledgeFacade/");
        }

        public MasterRolePreviledgeDTO mobilesaveorgdet(MasterRolePreviledgeDTO pgmod)
        {
            return COMMM.POSTData(pgmod, "MasterRolePreviledgeFacade/mobilesaveorgdet/");
        }

        public MasterRolePreviledgeDTO deleterec(int id)
        {
            return COMMM.DeleteDataById(id, "MasterRolePreviledgeFacade/deletemodpages/");
        }

        public MasterRolePreviledgeDTO mobiledeletemodpages(MasterRolePreviledgeDTO dto)
        {
            return COMMM.POSTData(dto, "MasterRolePreviledgeFacade/mobiledeletemodpages/");
        }

        public MasterRolePreviledgeDTO getmodulepagedata(MasterRolePreviledgeDTO id)
        {
            return COMMM.POSTData(id, "MasterRolePreviledgeFacade/getmodulepages/");
        }

        public MasterRolePreviledgeDTO getsearchdata(int data, MasterRolePreviledgeDTO dataa)
        {
            return COMMM.GETSEarchData(data, dataa, "MasterRolePreviledgeFacade/1/");
        }
    }
}
