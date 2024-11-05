using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.admission;
using CommonLibrary;


namespace corewebapi18072016.Delegates
{
    public class StudentAdmissionDelegates
    {
        CommonDelegate<Adm_M_StudentDTO, Adm_M_StudentDTO> COMMM = new CommonDelegate<Adm_M_StudentDTO, Adm_M_StudentDTO>();
        CommonDelegate<StudentDocumentDTO, StudentDocumentDTO> COM = new CommonDelegate<StudentDocumentDTO, StudentDocumentDTO>();

        private const String JsonContentType = "application/json; charset=utf-8";

        public string getData(long resource)
        {
            string product = "";
            HttpClient client = new HttpClient();

            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // HTTP GET
            try
            {
                HttpResponseMessage response = client.GetAsync("api/StudentAdmissionFacade/" + resource).Result;
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
        public StudentDocumentDTO savestudentdetails(StudentDocumentDTO stu)
        {
            return COM.POSTData(stu, "StudentAdmissionFacade/doc");


        }
        public Adm_M_StudentDTO checkDuplicate(Adm_M_StudentDTO Adm_M_StudentDTO)
        {

            // StudentDetailsDTO DTO = new StudentDetailsDTO();

            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {

                var myContent = JsonConvert.SerializeObject(Adm_M_StudentDTO);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PostAsync("api/StudentAdmissionFacade/checkDuplicate/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    Adm_M_StudentDTO = JsonConvert.DeserializeObject<Adm_M_StudentDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                }
            }
            catch
            {

            }


            return Adm_M_StudentDTO;

        }
        public Adm_M_StudentDTO getCaste(Adm_M_StudentDTO Adm_M_StudentDTO)
        {

            // StudentDetailsDTO DTO = new StudentDetailsDTO();

            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {

                var myContent = JsonConvert.SerializeObject(Adm_M_StudentDTO);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PostAsync("api/StudentAdmissionFacade/getcaste/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    Adm_M_StudentDTO = JsonConvert.DeserializeObject<Adm_M_StudentDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                }
            }
            catch
            {

            }


            return Adm_M_StudentDTO;

        }
        public Adm_M_StudentDTO GetData(Adm_M_StudentDTO DTO)
        {
            Adm_M_StudentDTO dto_ret = null;
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {

                var myContent = JsonConvert.SerializeObject(DTO);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PostAsync("api/StudentAdmissionFacade/Getdetails/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    dto_ret = JsonConvert.DeserializeObject<Adm_M_StudentDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return dto_ret;
        }
        public Adm_M_StudentDTO getdpstate(int id)
        {
            Adm_M_StudentDTO DTO = new Adm_M_StudentDTO();
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {

                var myContent = JsonConvert.SerializeObject(DTO);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/StudentAdmissionFacade/getdpstate/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    DTO = JsonConvert.DeserializeObject<Adm_M_StudentDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return DTO;
        }

        public Adm_M_StudentDTO getdpdistrict(int id)
        {
            Adm_M_StudentDTO DTO = new Adm_M_StudentDTO();
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {

                var myContent = JsonConvert.SerializeObject(DTO);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/StudentAdmissionFacade/getdpdistrict/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    DTO = JsonConvert.DeserializeObject<Adm_M_StudentDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return DTO;
        }
        public Adm_M_StudentDTO onchangebithplacecountry(int id)
        {
            Adm_M_StudentDTO DTO = new Adm_M_StudentDTO();
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {

                var myContent = JsonConvert.SerializeObject(DTO);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/StudentAdmissionFacade/onchangebithplacecountry/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    DTO = JsonConvert.DeserializeObject<Adm_M_StudentDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return DTO;
        }
        public Adm_M_StudentDTO onchangenationality(int id)
        {
            Adm_M_StudentDTO DTO = new Adm_M_StudentDTO();
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {

                var myContent = JsonConvert.SerializeObject(DTO);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/StudentAdmissionFacade/onchangenationality/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    DTO = JsonConvert.DeserializeObject<Adm_M_StudentDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return DTO;
        }
        public Adm_M_StudentDTO getdpcities(int id)
        {
            Adm_M_StudentDTO DTO = new Adm_M_StudentDTO();
            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {

                var myContent = JsonConvert.SerializeObject(DTO);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var response = client.GetAsync("api/StudentAdmissionFacade/getdpcities/" + id).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    DTO = JsonConvert.DeserializeObject<Adm_M_StudentDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return DTO;
        }
        public Adm_M_StudentDTO GetSelectedRowDetails(Adm_M_StudentDTO Adm_M_StudentDTO)
        {
            // Adm_M_StudentDTO Adm_M_StudentDTO = new Adm_M_StudentDTO();
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {


                var myContent = JsonConvert.SerializeObject(Adm_M_StudentDTO);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PostAsync("api/StudentAdmissionFacade/GetSelectedRowDetails/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    Adm_M_StudentDTO = JsonConvert.DeserializeObject<Adm_M_StudentDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                }
            }
            catch
            {

            }


            return Adm_M_StudentDTO;

        }
        public Adm_M_StudentDTO SaveData(Adm_M_StudentDTO Adm_M_StudentDTO)
        {

            // StudentDetailsDTO DTO = new StudentDetailsDTO();

            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {

                var myContent = JsonConvert.SerializeObject(Adm_M_StudentDTO);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PostAsync("api/StudentAdmissionFacade/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    Adm_M_StudentDTO = JsonConvert.DeserializeObject<Adm_M_StudentDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                }
            }
            catch
            {

            }


            return Adm_M_StudentDTO;

        }
        public Adm_M_StudentDTO DeleteBondEntry(int ID)
        {
            Adm_M_StudentDTO Adm_M_StudentDTO = new Adm_M_StudentDTO();
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //HTTP POST
            try
            {

                var myContent = JsonConvert.SerializeObject(ID);//IVRMM_Id
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.DeleteAsync("api/StudentAdmissionFacade/DeleteBondEntry/" + ID).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);
                }
            }
            catch
            {

            }
            return Adm_M_StudentDTO;
        }
        public Adm_M_StudentDTO DeletedEntry(Adm_M_StudentDTO DTO)
        {
            return COMMM.POSTDataaADM(DTO, "StudentAdmissionFacade/DeleteEntry/");
        }

        //readmint details
        public Adm_M_StudentDTO yearwisetcstd(Adm_M_StudentDTO DTO)
        {

            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {

                var myContent = JsonConvert.SerializeObject(DTO);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PostAsync("api/StudentAdmissionFacade/yearwisetcstd/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    DTO = JsonConvert.DeserializeObject<Adm_M_StudentDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return DTO;
        }
        public Adm_M_StudentDTO addtocart(Adm_M_StudentDTO DTO)
        {

            string product;
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            try
            {

                var myContent = JsonConvert.SerializeObject(DTO);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PostAsync("api/StudentAdmissionFacade/addtocart/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    DTO = JsonConvert.DeserializeObject<Adm_M_StudentDTO>(product, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects
                    });
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return DTO;
        }
        public Adm_M_StudentDTO searchByClmn(Adm_M_StudentDTO dto)
        {

            return COMMM.POSTDataaADM(dto, "StudentAdmissionFacade/searchbycolumn/");

        }
        public Adm_M_StudentDTO StateByCountryName(Adm_M_StudentDTO ct)
        {
            return COMMM.POSTDataaADM(ct, "StudentAdmissionFacade/StateByCountryName/");
        }
        public Adm_M_StudentDTO getmaxminage(Adm_M_StudentDTO enqdto)
        {
            return COMMM.POSTDataaADM(enqdto, "StudentAdmissionFacade/classchangemaxminage");
        }
        public Adm_M_StudentDTO savefirsttab(Adm_M_StudentDTO enqdto)
        {
            return COMMM.POSTDataaADM(enqdto, "StudentAdmissionFacade/savefirsttab");
        }
        public Adm_M_StudentDTO savesecondtab(Adm_M_StudentDTO enqdto)
        {
            return COMMM.POSTDataaADM(enqdto, "StudentAdmissionFacade/savesecondtab");
        }
        public Adm_M_StudentDTO savesixthtab(Adm_M_StudentDTO enqdto)
        {
            return COMMM.POSTDataaADM(enqdto, "StudentAdmissionFacade/savesixthtab");
        }
        public Adm_M_StudentDTO savethirdtab(Adm_M_StudentDTO data)
        {
            return COMMM.POSTDataaADM(data, "StudentAdmissionFacade/savethirdtab");
        }
        public Adm_M_StudentDTO savefourthtab(Adm_M_StudentDTO data)
        {
            return COMMM.POSTDataaADM(data, "StudentAdmissionFacade/savefourthtab");
        }
        public Adm_M_StudentDTO savefinaltab(Adm_M_StudentDTO data)
        {
            return COMMM.POSTDataaADM(data, "StudentAdmissionFacade/savefinaltab");
        }
        public Adm_M_StudentDTO checkbiometriccode(Adm_M_StudentDTO data)
        {
            return COMMM.POSTDataaADM(data, "StudentAdmissionFacade/checkbiometriccode");
        }
        public Adm_M_StudentDTO checkrfcardduplicate(Adm_M_StudentDTO data)
        {
            return COMMM.POSTDataaADM(data, "StudentAdmissionFacade/checkrfcardduplicate");
        }
        public Adm_M_StudentDTO onchangefathernationality(Adm_M_StudentDTO data)
        {
            return COMMM.POSTDataaADM(data, "StudentAdmissionFacade/onchangefathernationality");
        }
        public Adm_M_StudentDTO onchangemothernationality(Adm_M_StudentDTO data)
        {
            return COMMM.POSTDataaADM(data, "StudentAdmissionFacade/onchangemothernationality");
        }

        // Admission Cancel OR Widthdraw
        public Adm_M_StudentDTO OnLoadAdmissionCancel(Adm_M_StudentDTO data)
        {
            return COMMM.POSTDataaADM(data, "StudentAdmissionFacade/OnLoadAdmissionCancel");
        }
        public Adm_M_StudentDTO OnChangeAdmissionCancelYear(Adm_M_StudentDTO data)
        {
            return COMMM.POSTDataaADM(data, "StudentAdmissionFacade/OnChangeAdmissionCancelYear");
        }
        public Adm_M_StudentDTO OnChangeAdmissionCancelStudent(Adm_M_StudentDTO data)
        {
            return COMMM.POSTDataaADM(data, "StudentAdmissionFacade/OnChangeAdmissionCancelStudent");
        }
        public Adm_M_StudentDTO SaveAdmissionCancelStudent(Adm_M_StudentDTO data)
        {
            return COMMM.POSTDataaADM(data, "StudentAdmissionFacade/SaveAdmissionCancelStudent");
        }
        public Adm_M_StudentDTO EditAdmissionCancelStudent(Adm_M_StudentDTO data)
        {
            return COMMM.POSTDataaADM(data, "StudentAdmissionFacade/EditAdmissionCancelStudent");
        }

        // Admisison Cancel Report
        public Adm_M_StudentDTO OnLoadAdmissionCancelReport(Adm_M_StudentDTO data)
        {
            return COMMM.POSTDataaADM(data, "StudentAdmissionFacade/OnLoadAdmissionCancelReport");
        }
        public Adm_M_StudentDTO OnChangeAdmissionCancelReportYear(Adm_M_StudentDTO data)
        {
            return COMMM.POSTDataaADM(data, "StudentAdmissionFacade/OnChangeAdmissionCancelReportYear");
        }
        public Adm_M_StudentDTO ViewStudentProfile(Adm_M_StudentDTO data)
        {
            return COMMM.POSTDataaADM(data, "StudentAdmissionFacade/ViewStudentProfile");
        }

        public Adm_M_StudentDTO GetSubjectsofinstitute(Adm_M_StudentDTO data)
        {
            return COMMM.POSTDataaADM(data, "StudentAdmissionFacade/GetSubjectsofinstitute");
        }
    }
}
