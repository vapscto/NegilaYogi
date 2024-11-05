using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs;
using Newtonsoft.Json;
using System.Collections;
using CommonLibrary;
namespace corewebapi18072016.Delegates
{
    public class OralTestMarksEntryDelegates
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<OralTestMarksBindDataDTO, OralTestMarksBindDataDTO> COMMM = new CommonDelegate<OralTestMarksBindDataDTO, OralTestMarksBindDataDTO>();
        CommonDelegate<OralTestOralByMarksDTO, OralTestOralByMarksDTO> COMMMM = new CommonDelegate<OralTestOralByMarksDTO, OralTestOralByMarksDTO>();

        CommonDelegate<OralTestMarksBindDataDTO, OralTestOralByMarksDTO> COMM = new CommonDelegate<OralTestMarksBindDataDTO, OralTestOralByMarksDTO>();

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
                HttpResponseMessage response = client.GetAsync("api/OralTestMarksEntryFacadeController/" + resource).Result;
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

        public OralTestMarksBindDataDTO GetOralTestMarksEntryData(OralTestOralByMarksDTO OralTestOralByMarksDTO)
        {

            return COMM.POSTDataa(OralTestOralByMarksDTO, "OralTestMarksEntryFacade/Getdetails/");

          
        }


        public OralTestOralByMarksDTO GetdetailsOnSchedule(OralTestOralByMarksDTO OralTestOralByMarksDTO)
        {
            return COMMMM.POSTData(OralTestOralByMarksDTO, "OralTestMarksEntryFacade/GetdetailsOnSchedule/");
        
        }

        public OralTestOralByMarksDTO OralTestMarksEntryData(OralTestOralByMarksDTO OralTestOralByMarksDTO)
        {


            return COMMMM.POSTData(OralTestOralByMarksDTO, "OralTestMarksEntryFacade/");
            
        }

        public OralTestMarksBindDataDTO[] GetOralTestMarks(OralTestMarksBindDataDTO OralTestMarksBindDataDTO)
        {

        //    return COMMM.POSTData(OralTestMarksBindDataDTO, "OralTestMarksEntryFacade/");

            // StudentDetailsDTO DTO = new StudentDetailsDTO();
            OralTestMarksBindDataDTO[] temp = null;
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            try
            {

                var myContent = JsonConvert.SerializeObject(OralTestMarksBindDataDTO);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PostAsync("api/OralTestMarksEntryFacade/GetOralTestMarks/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    temp = JsonConvert.DeserializeObject<OralTestMarksBindDataDTO[]>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                }
            }
            catch
            {

            }


            return temp;

        }
    }
}
