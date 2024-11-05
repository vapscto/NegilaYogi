using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs;
using Newtonsoft.Json;

namespace corewebapi18072016.Delegates
{
    public class MasterMenuPageMappingInstitutionwiseDelegate
    {
        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO loaddata(IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO MasterModulesDTO)
        {
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            try
            {

                var myContent = JsonConvert.SerializeObject(MasterModulesDTO);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PostAsync("api/MasterMenuPageMappingInstitutionwiseFacade/getalldetails/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    MasterModulesDTO = JsonConvert.DeserializeObject<IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch
            {

            }
            return MasterModulesDTO;
        }

        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO modchangedata(IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO MasterModulesDTO)
        {
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            try
            {

                var myContent = JsonConvert.SerializeObject(MasterModulesDTO);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PostAsync("api/MasterMenuPageMappingInstitutionwiseFacade/modulechange/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    MasterModulesDTO = JsonConvert.DeserializeObject<IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch
            {

            }
            return MasterModulesDTO;
        }

        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO institutionchan(int ID)
        {
            IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO MasterModulesDTO = new IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO();
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            try
            {

                var myContent = JsonConvert.SerializeObject(ID);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.GetAsync("api/MasterMenuPageMappingInstitutionwiseFacade/institutionchange/" + ID).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    MasterModulesDTO = JsonConvert.DeserializeObject<IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch
            {

            }
            return MasterModulesDTO;
        }

        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO mainmenuchangedata(IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO MasterModulesDTO)
        {
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            try
            {

                var myContent = JsonConvert.SerializeObject(MasterModulesDTO);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PostAsync("api/MasterMenuPageMappingInstitutionwiseFacade/mainmenuchange/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    MasterModulesDTO = JsonConvert.DeserializeObject<IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch
            {

            }
            return MasterModulesDTO;
        }

        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO submenuchangedata(IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO data)
        {
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            try
            {

                var myContent = JsonConvert.SerializeObject(data);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PostAsync("api/MasterMenuPageMappingInstitutionwiseFacade/submenuchange/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    data = JsonConvert.DeserializeObject<IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch
            {

            }
            return data;
        }


        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO Onchangedetails(IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO data)
        {
            IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO InstituteMainMenuDTO = new IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO();
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            try
            {

                var myContent = JsonConvert.SerializeObject(data);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PostAsync("api/MasterMenuPageMappingInstitutionwiseFacade/Onchangedetails/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    InstituteMainMenuDTO = JsonConvert.DeserializeObject<IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch
            {

            }


            return InstituteMainMenuDTO;

        }

        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO savdata(IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO data)
        {
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            try
            {

                var myContent = JsonConvert.SerializeObject(data);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PostAsync("api/MasterMenuPageMappingInstitutionwiseFacade/savemenudata/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    data = JsonConvert.DeserializeObject<IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch
            {

            }
            return data;
        }


        public IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO deletemasterdataa(IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO ID)
        {
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            try
            {

                var myContent = JsonConvert.SerializeObject(ID);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PostAsync("api/MasterMenuPageMappingInstitutionwiseFacade/deletemasterdata/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    ID = JsonConvert.DeserializeObject<IVRM_Master_Menu_Page_Mapping_InstitutionwiseDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch
            {

            }
            return ID;
        }
    }
}
