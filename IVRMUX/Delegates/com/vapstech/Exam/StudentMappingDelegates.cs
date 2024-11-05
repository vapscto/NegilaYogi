
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs.com.vaps.Exam;
using Newtonsoft.Json;
using CommonLibrary;


namespace corewebapi18072016.Delegates
{
    public class StudentMappingDelegates
    {
        CommonDelegate<StudentMappingDTO, StudentMappingDTO> _comm = new CommonDelegate<StudentMappingDTO, StudentMappingDTO>();

        private const String JsonContentType = "application/json; charset=utf-8";
        public StudentMappingDTO Getdetails(StudentMappingDTO data)
        {
            StudentMappingDTO DTO = new StudentMappingDTO();
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50257/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(data);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/StudentMappingFacade/Getdetails", byteContent).Result;
                              
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    DTO = JsonConvert.DeserializeObject<StudentMappingDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return DTO;
        }
        public StudentMappingDTO Studentdetails(StudentMappingDTO data)
        {
            StudentMappingDTO DTO = new StudentMappingDTO();
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50257/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(data);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/StudentMappingFacade/Studentdetails", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    DTO = JsonConvert.DeserializeObject<StudentMappingDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return DTO;
        }        
        public StudentMappingDTO getcategory(StudentMappingDTO data)
        {
            StudentMappingDTO StudentMappingDTO = new StudentMappingDTO();
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50257/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(data);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/StudentMappingFacade/getcategory", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    StudentMappingDTO = JsonConvert.DeserializeObject<StudentMappingDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetBaseException());
            }
            return StudentMappingDTO;

        }
        public StudentMappingDTO getclassid(StudentMappingDTO data)
        {
            StudentMappingDTO StudentMappingDTO = new StudentMappingDTO();
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50257/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(data);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/StudentMappingFacade/getclassid", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    StudentMappingDTO = JsonConvert.DeserializeObject<StudentMappingDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetBaseException());
            }
            return StudentMappingDTO;

        }        
        public StudentMappingDTO getsubject(StudentMappingDTO data)
        {
            StudentMappingDTO StudentMappingDTO = new StudentMappingDTO();
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50257/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(data);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/StudentMappingFacade/getsubject", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    StudentMappingDTO = JsonConvert.DeserializeObject<StudentMappingDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetBaseException());
            }
            return StudentMappingDTO;

        }
        public StudentMappingDTO editdetails(int ID)
        {
            StudentMappingDTO StudentMappingDTO = new StudentMappingDTO();
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50257/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");   
            try
            { 
                var myContent = JsonConvert.SerializeObject(ID);//AMA_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");       
                var response = client.GetAsync("api/StudentMappingFacade/editdetails/" + ID).Result;             
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);
                    StudentMappingDTO = JsonConvert.DeserializeObject<StudentMappingDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return StudentMappingDTO;
        }        
        public StudentMappingDTO getalldetailsviewrecords(int ID)
        {
            StudentMappingDTO StudentMappingDTO = new StudentMappingDTO();
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50257/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            try
            {
                var myContent = JsonConvert.SerializeObject(ID);//AMA_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/StudentMappingFacade/getalldetailsviewrecords/" + ID).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);
                    StudentMappingDTO = JsonConvert.DeserializeObject<StudentMappingDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return StudentMappingDTO;
        }
        public StudentMappingDTO savedetails(StudentMappingDTO data)
        {
            StudentMappingDTO DTO = new StudentMappingDTO();
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50257/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {              

                var myContent = JsonConvert.SerializeObject(data);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");            
                var response = client.PostAsync("api/StudentMappingFacade/savedetails", byteContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    DTO = JsonConvert.DeserializeObject<StudentMappingDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    //Console.WriteLine("", product);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return DTO;
        }
        public StudentMappingDTO deactivate(StudentMappingDTO data)
        {
            StudentMappingDTO DTO = new StudentMappingDTO();
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50257/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(data);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/StudentMappingFacade/deactivate", byteContent).Result;
                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    DTO = JsonConvert.DeserializeObject<StudentMappingDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    //Console.WriteLine("", product);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return DTO;

        }
        public StudentMappingDTO get_cls_sections(StudentMappingDTO categorypage)
        {
            StudentMappingDTO pageedit = null;
            string pagedetails;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50257/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(categorypage);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/StudentMappingFacade/get_cls_sections", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    pagedetails = response.Content.ReadAsStringAsync().Result;

                    pageedit = JsonConvert.DeserializeObject<StudentMappingDTO>(pagedetails, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return pageedit;
        }
        public StudentMappingDTO OnClickRemove(StudentMappingDTO categorypage)
        {
            StudentMappingDTO pageedit = null;
            string pagedetails;
            Array[] dropDownArray = new Array[2];
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50257/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {
                var myContent = JsonConvert.SerializeObject(categorypage);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.PostAsync("api/StudentMappingFacade/OnClickRemove", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    pagedetails = response.Content.ReadAsStringAsync().Result;

                    pageedit = JsonConvert.DeserializeObject<StudentMappingDTO>(pagedetails, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return pageedit;
        }

        //Student Wise Question Paper Type Mapping
        public StudentMappingDTO BindData_PT(StudentMappingDTO data)
        {
            return _comm.POSTDataExam(data, "StudentMappingFacade/BindData_PT");
        }
        public StudentMappingDTO OnChangeYear_GetClass_PT(StudentMappingDTO data)
        {
            return _comm.POSTDataExam(data, "StudentMappingFacade/OnChangeYear_GetClass_PT");
        }
        public StudentMappingDTO OnChangeClass_GetSection_PT(StudentMappingDTO data)
        {
            return _comm.POSTDataExam(data, "StudentMappingFacade/OnChangeClass_GetSection_PT");
        }
        public StudentMappingDTO OnChangeSection_GetExam_PT(StudentMappingDTO data)
        {
            return _comm.POSTDataExam(data, "StudentMappingFacade/OnChangeSection_GetExam_PT");
        }
        public StudentMappingDTO OnChangeExam_GetSubject_PT(StudentMappingDTO data)
        {
            return _comm.POSTDataExam(data, "StudentMappingFacade/OnChangeExam_GetSubject_PT");
        }
        public StudentMappingDTO OnSearch_PT(StudentMappingDTO data)
        {
            return _comm.POSTDataExam(data, "StudentMappingFacade/OnSearch_PT");
        }
        public StudentMappingDTO OnSave_PT(StudentMappingDTO data)
        {
            return _comm.POSTDataExam(data, "StudentMappingFacade/OnSave_PT");
        }
        public StudentMappingDTO OnClickRemove_PT(StudentMappingDTO data)
        {
            return _comm.POSTDataExam(data, "StudentMappingFacade/OnClickRemove_PT");
        }
    }
}
