using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.Portals.Employee
{
    public class EmployeeForm12BBDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<Employee12BBDTO, Employee12BBDTO> COMMM = new CommonDelegate<Employee12BBDTO, Employee12BBDTO>();

        public Employee12BBDTO getalldetails(Employee12BBDTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeForm12BBFacade/getdata/");
        }

        //public Employee12BBDTO getyear(Employee12BBDTO data)
        //{
        //    return COMMM.POSTPORTALData(data, "EmployeeForm12BBFacade/getyear/");
        //}
        public Employee12BBDTO getdaily_data(Employee12BBDTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeForm12BBFacade/getdaily_data/");
        }

        public Employee12BBDTO getsalaryalldetails(Employee12BBDTO data)
        {
            return COMMM.POSTPORTALData(data, "EmployeeForm12BBFacade/getsalaryalldetails/");
        }

        //public EmployeeDashboardDTO getalldetails(EmployeeDashboardDTO data)
        //{
        //    string product;
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("http://localhost:51263/");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    //HTTP POST
        //    try
        //    {
        //        var myContent = JsonConvert.SerializeObject(data);
        //        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
        //        var byteContent = new ByteArrayContent(buffer);
        //        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //        var response = client.PostAsync("api/EmployeeSalaryDetailsFacade/getdata/", byteContent).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            product = response.Content.ReadAsStringAsync().Result;
        //            Console.WriteLine("", product);
        //            data = JsonConvert.DeserializeObject<EmployeeDashboardDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return data;

        //}

        //public EmployeeDashboardDTO getdaily_data(EmployeeDashboardDTO data)
        //{
        //    string product;
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("http://localhost:51263/");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    //HTTP POST
        //    try
        //    {
        //        var myContent = JsonConvert.SerializeObject(data);
        //        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
        //        var byteContent = new ByteArrayContent(buffer);
        //        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //        var response = client.PostAsync("api/EmployeeSalaryDetailsFacade/getdaily_data/", byteContent).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            product = response.Content.ReadAsStringAsync().Result;
        //            Console.WriteLine("", product);
        //            data = JsonConvert.DeserializeObject<EmployeeDashboardDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return data;
        //}

        //public EmployeeDashboardDTO getsalaryalldetails(EmployeeDashboardDTO data)
        //{
        //    string product;
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("http://localhost:51263/");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    //HTTP POST
        //    try
        //    {
        //        var myContent = JsonConvert.SerializeObject(data);
        //        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
        //        var byteContent = new ByteArrayContent(buffer);
        //        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //        var response = client.PostAsync("api/EmployeeSalaryDetailsFacade/getsalaryalldetails/", byteContent).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            product = response.Content.ReadAsStringAsync().Result;
        //            Console.WriteLine("", product);
        //            data = JsonConvert.DeserializeObject<EmployeeDashboardDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return data;
        //}
    }
}
