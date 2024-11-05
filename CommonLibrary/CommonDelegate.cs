using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

using Newtonsoft.Json;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Extensions.Options;

namespace CommonLibrary
{
    public class CommonDelegate<T, E> where T : class where E : class
    {

        //private readonly object resource;
        //private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        //private readonly corewebapi18072016.FacadeUrl _config;
        // private static FacadeUrl fdu = new FacadeUrl();

        // empty Constructor, dnt delete please
        public CommonDelegate() { }
        string URLPREADMISSION = "http://localhost:65140/";

        string ALUMNIURL  = "http://localhost:54176/";

        string URLADMISSION = "http://localhost:53497/";

        string URLHRMS = "http://localhost:59390/";

        string URLFEE = "http://localhost:49540/";

        string URLCOE = "http://localhost:60039/";

        string URLBirthday = "http://localhost:54009/";

        string URLFO = "http://localhost:50642/";

        string URLLEAVE = "http://localhost:57234/";

        string URLExam = "http://localhost:50257/";

        string URL_Transport = "http://localhost:54363/";

        string URL_Mobilecommonservice = "http://localhost:59441/";

        string url_portal = "http://localhost:51263/";

        string URL_SPORTS = "http://localhost:52230/";

        string CollegePreadmission  = "http://localhost:59069/";

        string URL_Clgadmission = "http://localhost:50790/";

        string URL_ClgFee = "http://localhost:55570/";
      
        string URL_Visitors = "http://localhost:50198/";

        string URL_Library = "http://localhost:50588/";
  
         string URL_ClgExam = "http://localhost:55516/";

        string URL_Inventory = "http://localhost:50639/";

        string URL_ClgBirthday = "http://localhost:54007/";

        string URL_ClgCOE = "http://localhost:60037/";

        string URL_Clgportals = "http://localhost:65159/";

        string URL_AssetsTracking = "http://localhost:52447/";

        string URL_Naacdetails = "http://localhost:50006/";

        string URL_Pda = "http://localhost:54217/";

        string URL_Timetable = "http://localhost:52949/";

        string URL_MobielApp = "http://localhost:53805/";

        string URL_HOSTEL = "http://localhost:61875/";

        string URLRecruitment = "http://localhost:18123/";

        string URLSeatingArrangment = "http://localhost:59571/";

        string URL_HealthManagement = "http://localhost:58286/";

        string URL_Clubmanagement = "http://localhost:13028/";
       
        string URL_VidyaBharathi= "http://localhost:54386/";

        string PlacementServiceHub = "http://localhost:54000/";

        string URL_Canteen = "http://localhost:60289/";
        public T GETData(T ctry, string facadeurl)
        {
            T enqdto = null;
            string product;
            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri(URLPREADMISSION);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {


                var myContent = JsonConvert.SerializeObject(ctry);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/" + facadeurl).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }
        //

        // Get Data by single Id
        public T GetDataById(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(URLPREADMISSION);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {


                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.GetAsync("api/" + facadeurl + id).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stuDTO;
        }
        //
        public T GetDataByIdNo(int id, T dto, string facadeurl)
        {
            T SeatBlockdto = null;
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URLPREADMISSION);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/" + facadeurl).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    SeatBlockdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects,
                        DateFormatHandling = DateFormatHandling.IsoDateFormat
                    });

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            // return output;
            return SeatBlockdto;
        }
        public T GetDataByIdFROFF(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(URLFO);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {


                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.GetAsync("api/" + facadeurl + id).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stuDTO;
        }
        public T DeleteDataByIdFROFF(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URLFO);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {

                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.DeleteAsync("api/" + facadeurl + id).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }
            }
            catch
            {

            }


            return stuDTO;
        }
        public T DeleteDataByIdCOE(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URLCOE);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {

                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.DeleteAsync("api/" + facadeurl + id).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }
            }
            catch
            {

            }


            return stuDTO;
        }

        // Delete Data by id
        public T DeleteDataById(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URLPREADMISSION);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {

                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.DeleteAsync("api/" + facadeurl + id).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }
            }
            catch
            {

            }


            return stuDTO;
        }
        //        
        public T POSTData(T ctry, string facadeurl)
        {
            T enqdto = default(T);
            string product;
            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri(URLPREADMISSION);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {


                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                    Console.WriteLine("", product);


                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }
        public T POSTDataa(E ctry, string facadeurl)
        {
            T enqdto = default(T);
            string product;
            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri(URLPREADMISSION);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {


                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                    Console.WriteLine("", product);


                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }
        public T GETSEarchData(int id, T dto, string facadeurl)
        {
            string pagedetails;
            T enqdto = null;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URLPREADMISSION);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(dto);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    pagedetails = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", pagedetails);
                    enqdto = JsonConvert.DeserializeObject<T>(pagedetails, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            // return output;
            return enqdto;
        }

        public T GETDataADm(T ctry, string facadeurl)
        {
            T enqdto = null;
            string product;
            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri(URLADMISSION);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {

                var myContent = JsonConvert.SerializeObject(ctry);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/" + facadeurl).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }
        //

        // Get Data by single Id
        public T GetDataByIdADM(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(URLADMISSION);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {


                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.GetAsync("api/" + facadeurl + id).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stuDTO;
        }
        //

        // Delete Data by id

        public T GetDataByIdNoADM(int id, T dto, string facadeurl)
        {
            T SeatBlockdto = null;
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URLADMISSION);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/" + facadeurl).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    SeatBlockdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects,
                        DateFormatHandling = DateFormatHandling.IsoDateFormat
                    });

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            // return output;
            return SeatBlockdto;
        }
        public T DeleteDataByIdADM(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URLADMISSION);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {

                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.DeleteAsync("api/" + facadeurl + id).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }
            }
            catch
            {

            }


            return stuDTO;
        }
        //


        public T POSTDataADM(T ctry, string facadeurl)
        {
            T enqdto = null;
            string product;
            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri(URLADMISSION);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {


                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }


        public T POSTDatafee(T ctry, string facadeurl)
        {
            T enqdto = null;
            string product;
            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri(URLFEE);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {


                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }

        public T POSTDataaADM(E ctry, string facadeurl)
        {
            T enqdto = null;
            string product;
            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri(URLADMISSION);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {


                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }

        public T GetData(string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(URLPREADMISSION);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var response = client.GetAsync("api/" + facadeurl).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects,
                        DateFormatHandling = DateFormatHandling.IsoDateFormat
                    });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stuDTO;
        }

        //

        //HRMS Common delegtate Functions


        // Get Data by single Id
        public T GetDataByIdHRMS(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(URLHRMS);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {


                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.GetAsync("api/" + facadeurl + id).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stuDTO;
        }
        //

        public T GetDataByIdNoHRMS(int id, T dto, string facadeurl)
        {
            T SeatBlockdto = null;
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URLHRMS);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/" + facadeurl).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    SeatBlockdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects,
                        DateFormatHandling = DateFormatHandling.IsoDateFormat
                    });

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            // return output;
            return SeatBlockdto;
        }
        public T DeleteDataByIdHRMS(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URLHRMS);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {

                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.DeleteAsync("api/" + facadeurl + id).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }
            }
            catch
            {

            }


            return stuDTO;
        }
        //


        public T POSTDataHRMS(T ctry, string facadeurl)
        {
            T enqdto = null;
            string product;
            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri(URLHRMS);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {


                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }

        public T POSTDataaHRMS(E ctry, string facadeurl)
        {
            T enqdto = null;
            string product;
            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri(URLHRMS);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {


                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }



        public T GetDataByIdfee(T ctry, string facadeurl)
        {
            T enqdto = null;
            string product;
            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri(URLADMISSION);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {

                var myContent = JsonConvert.SerializeObject(ctry);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/" + facadeurl).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });

                }

            }


            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }

        public T GetDataByIdCOE(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(URLCOE);


            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id

                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.GetAsync("api/" + facadeurl + id).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);


                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stuDTO;
        }
        public T POSTDataCOE(T ctry, string facadeurl)
        {
            T enqdto = null;
            string product;
            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri(URLCOE);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {


                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }

        public T POSTDataBirthday(T ctry, string facadeurl)
        {
            T enqdto = null;
            string product;
            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri(URLBirthday);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {


                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }

        //Holiday Report


        public T POSTDataHolidayReport(T ctry, string facadeurl)
        {
            T enqdto = null;
            string product;
            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri(URLFO);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {

                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }


        //online leave application
        public T POSTDataOnlineLeave(T ctry, string facadeurl)
        {
            T enqdto = null;
            string product;
            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri(URLLEAVE);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {


                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }
        public T POSTDataOnlineTC(T ctry, string facadeurl)
        {
            T enqdto = null;
            string product;
            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri(url_portal);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {


                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }
        public T POSTDataExam(T ctry, string facadeurl)
        {
            T enqdto = default(T);
            string product;
            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri(URLExam);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {


                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                    Console.WriteLine("", product);


                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }

        public T GetDataByIdExam(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(URLExam);


            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id

                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.GetAsync("api/" + facadeurl + id).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);


                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stuDTO;
        }
        //Transport
        public T POSTDataTransport(T ctry, string facadeurl)
        {
            T enqdto = default(T);
            string product;
            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri(URL_Transport);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }

        public T GetDataByIdTransport(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(URL_Transport);


            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id

                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.GetAsync("api/" + facadeurl + id).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);


                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stuDTO;
        }


        public T POSTPORTALData(T ctry, string facadeurl)
        {
            T enqdto = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(url_portal);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }

        public T GETPORTALData(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(url_portal);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.GetAsync("api/" + facadeurl + id).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stuDTO;
        }

        public T POSTDataInventory(T ctry, string facadeurl)
        {
            T enqdto = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(URL_Inventory);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }

        public T GETDataInventory(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(URL_Inventory);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.GetAsync("api/" + facadeurl + id).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stuDTO;
        }
        public T POSTDataAssetsTracking(T ctry, string facadeurl)
        {
            T enqdto = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(URL_AssetsTracking);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }

        public T GETDataAssetsTracking(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(URL_AssetsTracking);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.GetAsync("api/" + facadeurl + id).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stuDTO;
        }

        public T GetDataByIdSports(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(URL_SPORTS);


            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id

                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.GetAsync("api/" + facadeurl + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);


                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stuDTO;
        }
        public T POSTDataSports(T ctry, string facadeurl)
        {
            T enqdto = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(URL_SPORTS);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }

        //Visitor management

        public T GetDataByIdVisitors(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(URL_Visitors);


            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id

                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.GetAsync("api/" + facadeurl + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);


                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stuDTO;
        }
        public T POSTDataVisitors(T ctry, string facadeurl)
        {
            T enqdto = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(URL_Visitors);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }



        //end of visitor management

        //College Admission
        public T clgadmissionbypost(T ctry, string facadeurl)
        {
            T enqdto = default(T);
            string product;
            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri(URL_Clgadmission);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }

        public T clgadmissionbyid(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(URL_Clgadmission);


            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id

                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.GetAsync("api/" + facadeurl + id).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);


                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stuDTO;
        }

        public T GETClgFee(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(URL_ClgFee);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.GetAsync("api/" + facadeurl + id).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stuDTO;
        }

        public T PostClgFee(T ctry, string facadeurl)
        {
            T enqdto = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(URL_ClgFee);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }

        public T POSTDataCollfee(T ctry, string facadeurl)
        {
            T enqdto = null;
            string product;
            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri(URL_ClgFee);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {


                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }

        public T GETLibrary(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(URL_Library);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.GetAsync("api/" + facadeurl + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stuDTO;
        }

        public T PostLibrary(T ctry, string facadeurl)
        {
            T enqdto = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(URL_Library);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }


        //col exam
        public T GETexam(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL_ClgExam);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP GET
            try
            {
                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/" + facadeurl + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);
                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stuDTO;
        }


        public T POSTcolExam(T ctry, string facadeurl)
        {
            T enqdto = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(URL_ClgExam);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }

        public T GETDataAlumni(T ctry, string facadeurl)
        {
            T enqdto = null;
            string product;
            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri(ALUMNIURL);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {


                var myContent = JsonConvert.SerializeObject(ctry);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/" + facadeurl).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }
        //

        // Get Data by single Id
        public T GetDataByIdAlumni(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(ALUMNIURL);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {


                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.GetAsync("api/" + facadeurl + id).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stuDTO;
        }
        //
        public T GetDataByIdNoAlumni(int id, T dto, string facadeurl)
        {
            T SeatBlockdto = null;
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ALUMNIURL);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/" + facadeurl).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    SeatBlockdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects,
                        DateFormatHandling = DateFormatHandling.IsoDateFormat
                    });

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            // return output;
            return SeatBlockdto;
        }

        // Delete Data by id
        public T DeleteDataByIdAlumni(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ALUMNIURL);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {

                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.DeleteAsync("api/" + facadeurl + id).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }
            }
            catch
            {

            }


            return stuDTO;
        }
        //
        public T POSTDataAlumni(T ctry, string facadeurl)
        {
            T enqdto = default(T);
            string product;
            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri(ALUMNIURL);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {


                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                    Console.WriteLine("", product);


                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }

        public T POSTDataaAlumni(E ctry, string facadeurl)
        {
            T enqdto = default(T);
            string product;
            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri(ALUMNIURL);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {


                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                    Console.WriteLine("", product);


                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }

        public T GETSEarchDataAlumni(int id, T dto, string facadeurl)
        {
            string pagedetails;
            T enqdto = null;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(ALUMNIURL);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(dto);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    pagedetails = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", pagedetails);
                    enqdto = JsonConvert.DeserializeObject<T>(pagedetails, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            // return output;
            return enqdto;
        }

        public T GetDataAlumni(string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(ALUMNIURL);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var response = client.GetAsync("api/" + facadeurl).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects,
                        DateFormatHandling = DateFormatHandling.IsoDateFormat
                    });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stuDTO;
        }


        //=============College Birthday
        public T GETClgBirthday(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL_ClgBirthday);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP GET
            try
            {
                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/" + facadeurl + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);
                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stuDTO;
        }
        public T POSTClgBirthday(T ctry, string facadeurl)
        {
            T enqdto = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(URL_ClgBirthday);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }
        //=============College COE
        public T GETClgCOE(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL_ClgCOE);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP GET
            try
            {
                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/" + facadeurl + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);
                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stuDTO;
        }
        public T POSTClgCOE(T ctry, string facadeurl)
        {
            T enqdto = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(URL_ClgCOE);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }
        public T DeleteClgCOE(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URLCOE);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {

                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.DeleteAsync("api/" + facadeurl + id).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }
            }
            catch
            {

            }
            return stuDTO;
        }
        //============================================

        //===================College Portal    
        public T CLGPortalPOSTData(T ctry, string facadeurl)
        {
            T enqdto = default(T);
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL_Clgportals);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;
                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;
                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                    Console.WriteLine("", product);
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }

        public T CLGPortalPOSTDataa(E ctry, string facadeurl)
        {
            T enqdto = default(T);
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL_Clgportals);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                    Console.WriteLine("", product);
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }
        //============================================



        //---------------COLLEGE PREADMISSION----------------------//



        public T CollegeGETData(T ctry, string facadeurl)
        {
            T enqdto = null;
            string product;
            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri(URLPREADMISSION);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {


                var myContent = JsonConvert.SerializeObject(ctry);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/" + facadeurl).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }
        //

        // Get Data by single Id
        public T CollegeGetDataById(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(CollegePreadmission);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {


                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.GetAsync("api/" + facadeurl + id).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stuDTO;
        }
        //
        public T CollegeGetDataByIdNo(int id, T dto, string facadeurl)
        {
            T SeatBlockdto = null;
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(CollegePreadmission);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/" + facadeurl).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    SeatBlockdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects,
                        DateFormatHandling = DateFormatHandling.IsoDateFormat
                    });

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            // return output;
            return SeatBlockdto;
        }

        public T CollegeDeleteDataById(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(CollegePreadmission);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {

                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.DeleteAsync("api/" + facadeurl + id).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }
            }
            catch
            {

            }


            return stuDTO;
        }
        //


        public T CollegePOSTData(T ctry, string facadeurl)
        {
            T enqdto = default(T);
            string product;
            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri(CollegePreadmission);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {


                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                    Console.WriteLine("", product);


                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }

        public T CollegePOSTDataa(E ctry, string facadeurl)
        {
            T enqdto = default(T);
            string product;
            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri(CollegePreadmission);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {


                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                    Console.WriteLine("", product);


                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }




        public T CollegeGETSEarchData(int id, T dto, string facadeurl)
        {
            string pagedetails;
            T enqdto = null;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(CollegePreadmission);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(dto);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    pagedetails = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", pagedetails);
                    enqdto = JsonConvert.DeserializeObject<T>(pagedetails, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            // return output;
            return enqdto;
        }
        public T CollegeGetData(string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(CollegePreadmission);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var response = client.GetAsync("api/" + facadeurl).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects,
                        DateFormatHandling = DateFormatHandling.IsoDateFormat
                    });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stuDTO;
        }

        //--------------------------END OF COLLEGE PREADMISSION-----------------//

        //=====================Hostel
        public T Get_Hostel(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(URL_HOSTEL);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.GetAsync("api/" + facadeurl + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stuDTO;
        }

        public T Post_Hostel(T ctry, string facadeurl)
        {
            T enqdto = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(URL_HOSTEL);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }
        //===================End Hostel


        public T naacdetailsbypost(T ctry, string facadeurl)
        {
            T enqdto = default(T);
            string product;
            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri(URL_Naacdetails);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }

        public T naacdetailsbyid(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(URL_Naacdetails);


            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id

                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.GetAsync("api/" + facadeurl + id).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);


                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stuDTO;
        }

        ///Timetable      
        public T POSTDataTimeTable(T ctry, string facadeurl)
        {
            T enqdto = default(T);
            string product;
            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri(URL_Timetable);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }
        public T GetDataByIdTimeTable(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(URL_Timetable);


            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);

                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/" + facadeurl + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);


                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stuDTO;
        }

        //RECRUITMENT
        public T GetVMS(int id, string facadeurl)
        {
            T SeatBlockdto = null;
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URLRecruitment);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/" + facadeurl + id).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    SeatBlockdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects,
                        DateFormatHandling = DateFormatHandling.IsoDateFormat
                    });

                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            // return output;
            return SeatBlockdto;
        }
        public T POSTVMS(E ctry, string facadeurl)
        {
            T enqdto = null;
            string product;
            HttpClient client = new HttpClient();


            client.BaseAddress = new Uri(URLRecruitment);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {


                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }

        //Seating Arrangement
        public T SeatingArrangmentGet(int id, string facadeurl)
        {
            T SeatBlockdto = null;
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URLSeatingArrangment);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");            
            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/" + facadeurl + id).Result;                
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    SeatBlockdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects,
                        DateFormatHandling = DateFormatHandling.IsoDateFormat
                    });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            } 
            return SeatBlockdto;
        }
        public T SeatingArrangmentPOST(E ctry, string facadeurl)
        {
            T enqdto = null;
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URLSeatingArrangment);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
           
            try
            {
                var myContent = JsonConvert.SerializeObject(ctry);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }

        //Health Management
        public T HealthManagementGet(int id, string facadeurl)
        {
            T SeatBlockdto = null;
            string product;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL_HealthManagement);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            try
            {
                var myContent = JsonConvert.SerializeObject(id);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/" + facadeurl + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;

                    SeatBlockdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects,
                        DateFormatHandling = DateFormatHandling.IsoDateFormat
                    });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return SeatBlockdto;
        }
        public T HealthManagementPOST(E ctry, string facadeurl)
        {
            T enqdto = null;
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL_HealthManagement);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

            try
            {
                var myContent = JsonConvert.SerializeObject(ctry);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }


        //club mangement
        public T GetDataByClubManagement(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(URL_Clubmanagement);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {


                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.GetAsync("api/" + facadeurl + id).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stuDTO;
        }
        //
        public T POSTDataClubManagement(T ctry, string facadeurl)
        {
            T enqdto = null;
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL_Clubmanagement);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
               var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");               
                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;               
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }
        //VidyaBharathi
        public T GetDataByVidyaBharathi(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL_VidyaBharathi);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");               
                var response = client.GetAsync("api/" + facadeurl + id).Result;              
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stuDTO;
        }

        public T POSTDataVidyaBharathi(T ctry, string facadeurl)
        {
            T enqdto = null;
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL_VidyaBharathi);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }

        //PlacementModule
        public T GetDataByPlacement(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(PlacementServiceHub);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/" + facadeurl + id).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stuDTO;
        }

        public T POSTDataPlacement(T ctry, string facadeurl)
        {
            T enqdto = null;
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(PlacementServiceHub);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    Console.WriteLine("", product);
                }

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return enqdto;
        }


        public T GETFees(int id, string facadeurl)
        {
            T stuDTO = null;
            string product;
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri(URLFEE);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.GetAsync("api/" + facadeurl + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return stuDTO;
        }


        //POSTDataByCanteen


        public T POSTDataByCanteen(T ctry, string facadeurl)        {            T enqdto = null;            string product;            HttpClient client = new HttpClient();            client.BaseAddress = new Uri(URL_Canteen);            client.DefaultRequestHeaders.Accept.Clear();            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try            {                var myContent = JsonConvert.SerializeObject(ctry);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);                var byteContent = new ByteArrayContent(buffer);                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");                var response = client.PostAsync("api/" + facadeurl, byteContent).Result;                if (response.IsSuccessStatusCode)                {                    product = response.Content.ReadAsStringAsync().Result;                    enqdto = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });                    Console.WriteLine("", product);                }            }            catch (Exception ee)            {                Console.WriteLine(ee.Message);            }            return enqdto;        }

        public T GetDataByCanteen(int id, string facadeurl)        {            T stuDTO = null;            string product;            HttpClient client = new HttpClient();            client.BaseAddress = new Uri(URL_Canteen);            client.DefaultRequestHeaders.Accept.Clear();            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try            {                var myContent = JsonConvert.SerializeObject(id);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);                var byteContent = new ByteArrayContent(buffer);                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                // var response = client.PostAsync("api/registrationFacade/", byteContent).Result;

                var response = client.GetAsync("api/" + facadeurl + id).Result;

                // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                if (response.IsSuccessStatusCode)                {                    product = response.Content.ReadAsStringAsync().Result;                    Console.WriteLine("", product);                    stuDTO = JsonConvert.DeserializeObject<T>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });                }            }            catch (Exception ee)            {                Console.WriteLine(ee.Message);            }            return stuDTO;        }

    }
}