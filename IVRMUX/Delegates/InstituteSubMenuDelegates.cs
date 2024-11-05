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
    public class InstituteSubMenuDelegates
    {


        private const String JsonContentType = "application/json; charset=utf-8";

        public string getData(long resource)
        {
            string product = "";
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

            try
            {
                HttpResponseMessage response = client.GetAsync("api/InstituteSubMenuFacadeController/" + resource).Result;
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

        public InstituteMainMenuDTO getmoduledetails(InstituteMainMenuDTO data)
        {
            // InstituteMainMenuDTO InstituteMainMenuDTO = new InstituteMainMenuDTO();
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

                var response = client.PostAsync("api/InstituteSubMenuFacade/getmoduledetails/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    data = JsonConvert.DeserializeObject<InstituteMainMenuDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch
            {

            }

            return data;

        }

        public InstituteMainMenuDTO GetMasterSubMenuData(InstituteMainMenuDTO lo)
        {
            InstituteMainMenuDTO DTO = new InstituteMainMenuDTO();
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

            try
            {

                var myContent = JsonConvert.SerializeObject(lo);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                //var response = client.GetAsync("api/InstituteSubMenuFacade/Getdetails").Result;
                var response = client.PostAsync("api/InstituteSubMenuFacade/Getdetails", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    DTO = JsonConvert.DeserializeObject<InstituteMainMenuDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch
            {

            }
            return DTO;
        }

        public InstituteMainMenuDTO GetSelectedRowDetails(int ID)
        {
            InstituteMainMenuDTO InstituteMainMenuDTO = new InstituteMainMenuDTO();
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

                var response = client.GetAsync("api/InstituteSubMenuFacade/GetSelectedRowDetails/" + ID).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    InstituteMainMenuDTO = JsonConvert.DeserializeObject<InstituteMainMenuDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch
            {

            }


            return InstituteMainMenuDTO;

        }

        //public InstituteMainMenuDTO getMenudetailsByModuleId(InstituteMainMenuDTO ID)
        //{
        //    InstituteMainMenuDTO InstituteMainMenuDTO = new InstituteMainMenuDTO();
        //    string product = "";
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("http://localhost:65140/");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
        //    try
        //    {

        //        var myContent = JsonConvert.SerializeObject(ID);
        //        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


        //        var byteContent = new ByteArrayContent(buffer);
        //        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        //        var response = client.GetAsync("api/InstituteSubMenuFacade/getMenudetailsByModuleId/").Result;

        //        if (response.IsSuccessStatusCode)
        //        {
        //            product = response.Content.ReadAsStringAsync().Result;
        //            Console.WriteLine("", product);

        //            InstituteMainMenuDTO = JsonConvert.DeserializeObject<InstituteMainMenuDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        //        }
        //    }
        //    catch
        //    {

        //    }


        //    return InstituteMainMenuDTO;

        //}


        public InstituteMainMenuDTO getMenudetailsByModuleId(InstituteMainMenuDTO data)
        {
            InstituteMainMenuDTO InstituteMainMenuDTO = new InstituteMainMenuDTO();
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

                var response = client.PostAsync("api/InstituteSubMenuFacade/getMenudetailsByModuleId/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    InstituteMainMenuDTO = JsonConvert.DeserializeObject<InstituteMainMenuDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch
            {

            }


            return InstituteMainMenuDTO;

        }

        public InstituteMainMenuDTO getSubMenudetailsByMainMenuId(InstituteMainMenuDTO data)
        {
            InstituteMainMenuDTO InstituteMainMenuDTO = new InstituteMainMenuDTO();
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

                var response = client.PostAsync("api/InstituteSubMenuFacade/getSubMenudetailsByMainMenuId/" ,byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    InstituteMainMenuDTO = JsonConvert.DeserializeObject<InstituteMainMenuDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch
            {

            }


            return InstituteMainMenuDTO;

        }
        public InstituteMainMenuDTO MasterMainMenuDTO(InstituteMainMenuDTO InstituteMainMenuDTO)//Int32 IVRMM_Id
        {

            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {

                var myContent = JsonConvert.SerializeObject(InstituteMainMenuDTO);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PostAsync("api/InstituteSubMenuFacade/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    InstituteMainMenuDTO = JsonConvert.DeserializeObject<InstituteMainMenuDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }
            }
            catch
            {

            }


            return InstituteMainMenuDTO;
        
        }

        public InstituteMainMenuDTO MasterDeleteMainMenuDTO(int ID)
        {
            InstituteMainMenuDTO InstituteMainMenuDTO = new InstituteMainMenuDTO();
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

                var response = client.DeleteAsync("api/InstituteSubMenuFacade/MasterDeleteMainMenuDTO/" + ID).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    InstituteMainMenuDTO = JsonConvert.DeserializeObject<InstituteMainMenuDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }
            }
            catch
            {

            }


            return InstituteMainMenuDTO;

        }


        public InstituteMainMenuDTO Onchangedetails(InstituteMainMenuDTO data)
        {
            InstituteMainMenuDTO InstituteMainMenuDTO = new InstituteMainMenuDTO();
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

                var response = client.PostAsync("api/InstituteSubMenuFacade/Onchangedetails/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    InstituteMainMenuDTO = JsonConvert.DeserializeObject<InstituteMainMenuDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch
            {

            }


            return InstituteMainMenuDTO;

        }

    }
}
