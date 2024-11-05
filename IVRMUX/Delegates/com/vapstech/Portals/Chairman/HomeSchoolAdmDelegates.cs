
using System;
using PreadmissionDTOs.com.vaps.Portals.Chirman;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class HomeSchoolAdmDelegates
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HomeSchoolAdmDTO, HomeSchoolAdmDTO> COMMM = new CommonDelegate<HomeSchoolAdmDTO, HomeSchoolAdmDTO>();
        public HomeSchoolAdmDTO Getdetails(HomeSchoolAdmDTO data)
        {
            return COMMM.POSTPORTALData(data,"HomeSchoolAdmFacade/Getdetails/");
        }
  //public string getData(long resource)
        //{

        //    string product = "";
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("http://localhost:50257/");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
        //    // HTTP GET
        //    try
        //    {
        //        HttpResponseMessage response = client.GetAsync("api/BaldwinAllReportFacadeController/" + resource).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            product = response.Content.ReadAsStringAsync().Result;
        //            Console.WriteLine("{0}\t${1}\t{2}", product);   
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.GetBaseException());
        //    }
        //    return product;
        //}

        //public ChairmanDashboardDTO Getdetails(ChairmanDashboardDTO data)
        //{
            
        //      string product;
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
        //        var response = client.PostAsync("api/ChairmanDashboardFacade/Getdetails", byteContent).Result;
        //        if (response.IsSuccessStatusCode)
        //        {
        //            product = response.Content.ReadAsStringAsync().Result;
        //            Console.WriteLine("", product);
        //            data = JsonConvert.DeserializeObject<ChairmanDashboardDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
        //        }
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return data;
        //}

        

        //public BaldwinAllReportDTO savedetails(BaldwinAllReportDTO data)//Int32 IVRMM_Id
        //{
        //    BaldwinAllReportDTO DTO = new BaldwinAllReportDTO();
        //    string product = "";
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("http://localhost:50257/");
        //    client.DefaultRequestHeaders.Accept.Clear();
        //   client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
        //    //HTTP POST
        //    try
        //    {         

        //        var myContent = JsonConvert.SerializeObject(data);//IVRMM_Id
        //        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
        //        var byteContent = new ByteArrayContent(buffer);
        //        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");            
        //        var response = client.PostAsync("api/BaldwinAllReportFacade/savedetails", byteContent).Result;

        //        if (response.IsSuccessStatusCode)
        //        {
        //            product = response.Content.ReadAsStringAsync().Result;
        //            Console.WriteLine("", product);

        //            DTO = JsonConvert.DeserializeObject<BaldwinAllReportDTO>(product, new JsonSerializerSettings
        //            {
        //                TypeNameHandling = TypeNameHandling.Objects
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex);
        //    }
        //    return DTO;
        
        //}

    }
}
