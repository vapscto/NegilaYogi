using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Fees
{
    public class ClgQuotaFeeGroupDelegate
    {
            public ClgQuotaFeeGroupDTO savedetails(ClgQuotaFeeGroupDTO Grouppage)
            {
                string product;
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:55570/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HTTP POST
                try
                {
                    var myContent = JsonConvert.SerializeObject(Grouppage);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var response = client.PostAsync("api/ClgQuotaFeeGroupFacade/", byteContent).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        product = response.Content.ReadAsStringAsync().Result;
                        Console.WriteLine("", product);
                        Grouppage = JsonConvert.DeserializeObject<ClgQuotaFeeGroupDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    }
                }
                catch
                {

                }
                return Grouppage;
            }


         

            public string getData(long resource)
            {
                string product = "";
                HttpClient client = new HttpClient();

                client.BaseAddress = new Uri("http://localhost:57606/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // HTTP GET
                try
                {
                    HttpResponseMessage response = client.GetAsync("api/ClgQuotaFeeGroupFacade/" + resource).Result;
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
            public ClgQuotaFeeGroupDTO getdetails(ClgQuotaFeeGroupDTO orgdet)
            {
                //ClgQuotaFeeGroupDTO orgdet = null;
                string product;
                Array[] dropDownArray = new Array[2];
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:55570/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HTTP POST
                try
                {
                    var myContent = JsonConvert.SerializeObject(orgdet);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var response = client.PostAsync("api/ClgQuotaFeeGroupFacade/getdetails", byteContent).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        product = response.Content.ReadAsStringAsync().Result;

                        orgdet = JsonConvert.DeserializeObject<ClgQuotaFeeGroupDTO>(product, new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.Objects
                        });
                    }
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }
                // return output;
                return orgdet;
            }

            public ClgQuotaFeeGroupDTO EditDetails(int id)
            {
                ClgQuotaFeeGroupDTO orgdet = null;
                string product;
                Array[] dropDownArray = new Array[2];
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:55570/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var myContent = JsonConvert.SerializeObject(id);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var response = client.GetAsync("api/ClgQuotaFeeGroupFacade/Editdetails/" + id).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        product = response.Content.ReadAsStringAsync().Result;

                        orgdet = JsonConvert.DeserializeObject<ClgQuotaFeeGroupDTO>(product, new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.Objects
                        });
                    }
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }

                return orgdet;
            }

            public ClgQuotaFeeGroupDTO getpagedetails(int id)
            {
                ClgQuotaFeeGroupDTO pageedit = null;
                string pagedetails;
                Array[] dropDownArray = new Array[2];
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:55570/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HTTP POST
                try
                {
                    var myContent = JsonConvert.SerializeObject(id);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var response = client.GetAsync("api/ClgQuotaFeeGroupFacade/getpagedetails/" + id).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        pagedetails = response.Content.ReadAsStringAsync().Result;

                        pageedit = JsonConvert.DeserializeObject<ClgQuotaFeeGroupDTO>(pagedetails, new JsonSerializerSettings
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

            public ClgQuotaFeeGroupDTO getsearchdata(int data, ClgQuotaFeeGroupDTO dataa)
            {
                //   MasterSectionDTO pageedit = null;
                string pagedetails;
                Array[] dropDownArray = new Array[2];
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:55570/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HTTP POST
                try
                {
                    var myContent = JsonConvert.SerializeObject(dataa);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var response = client.PostAsync("api/ClgQuotaFeeGroupFacade/", byteContent).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        pagedetails = response.Content.ReadAsStringAsync().Result;
                        Console.WriteLine("", pagedetails);
                        dataa = JsonConvert.DeserializeObject<ClgQuotaFeeGroupDTO>(pagedetails, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    }
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }
                // return output;
                return dataa;
            }

            public ClgQuotaFeeGroupDTO deleterec(int id)
            {
                ClgQuotaFeeGroupDTO enqdto = null;
                string product;
                Array[] dropDownArray = new Array[2];
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:55570/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HTTP POST
                try
                {
                    var myContent = JsonConvert.SerializeObject(id);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var response = client.DeleteAsync("api/ClgQuotaFeeGroupFacade/deletedetails/" + id).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        product = response.Content.ReadAsStringAsync().Result;

                        enqdto = JsonConvert.DeserializeObject<ClgQuotaFeeGroupDTO>(product, new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.Objects
                        });

                        enqdto = JsonConvert.DeserializeObject<ClgQuotaFeeGroupDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
                    }
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }
                // return output;
                return enqdto;
            }

            public ClgQuotaFeeGroupDTO deactivateAcademicYear(ClgQuotaFeeGroupDTO id)
            {
                ClgQuotaFeeGroupDTO enqdet = null;
                string product;
                Array[] dropDownArray = new Array[2];
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:55570/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HTTP POST
                try
                {
                    var myContent = JsonConvert.SerializeObject(id);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var response = client.PostAsync("api/ClgQuotaFeeGroupFacade/deactivate/", byteContent).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        product = response.Content.ReadAsStringAsync().Result;

                        using (var sr = new StringReader(product))
                        using (var reader = new JsonTextReader(sr))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            serializer.DateParseHandling = DateParseHandling.DateTime;
                            serializer.TypeNameHandling = TypeNameHandling.Auto;
                            enqdet = serializer.Deserialize<ClgQuotaFeeGroupDTO>(reader);
                        }

                        // enqdet = JsonConvert.DeserializeObject<Enq>(product, jsonConve);
                        enqdet = JsonConvert.DeserializeObject<ClgQuotaFeeGroupDTO>(product, new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.Objects,
                            DateParseHandling = DateParseHandling.DateTime
                        });
                    }
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.Message);
                }
                // return output;
                return enqdet;
            }

            // for yearly group 

            public ClgQuotaFeeGroupDTO getIndependentDropDowns(ClgQuotaFeeGroupDTO yrs)
            {
                ClgQuotaFeeGroupDTO enqdto = null;
                string product;
                Array[] dropDownArray = new Array[2];
                HttpClient client = new HttpClient();

                // string str = _config.StudentFacadeUrlTwo;

                client.BaseAddress = new Uri("http://localhost:55570/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //HTTP POST
                try
                {
                    var myContent = JsonConvert.SerializeObject(yrs);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);

                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    var response = client.PostAsync("api/ClgQuotaFeeGroupFacade/yearsbind", byteContent).Result;

                    // HttpResponseMessage response = client.PostAsync("api/PreadmissionFacade/", lo);
                    if (response.IsSuccessStatusCode)
                    {
                        product = response.Content.ReadAsStringAsync().Result;
                        //List<StudentApplicationPartOneDTO> stu = new List<StudentApplicationPartOneDTO>();

                        //var stus = JsonConvert.DeserializeAnonymousType(product).Last();

                        enqdto = JsonConvert.DeserializeObject<ClgQuotaFeeGroupDTO>(product, new JsonSerializerSettings
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
                return enqdto;
            }
            

        
    }
}
