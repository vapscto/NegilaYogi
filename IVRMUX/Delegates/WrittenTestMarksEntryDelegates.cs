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
    public class WrittenTestMarksEntryDelegates
    {

        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<WrittenTestMarksBindDataDTO, WrittenTestMarksBindDataDTO> COMMM = new CommonDelegate<WrittenTestMarksBindDataDTO, WrittenTestMarksBindDataDTO>();
        CommonDelegate<WirttenTestSubjectWiseMarksEntryDTO, WirttenTestSubjectWiseMarksEntryDTO> COMMMM = new CommonDelegate<WirttenTestSubjectWiseMarksEntryDTO, WirttenTestSubjectWiseMarksEntryDTO>();
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
                HttpResponseMessage response = client.GetAsync("api/WrittenTestMarksEntryFacadeController/" + resource).Result;
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

        public WrittenTestMarksBindDataDTO GetWrittenTestMarksEntryData(WrittenTestMarksBindDataDTO WrittenTestMarksBindDataDTO)
        {
            return COMMM.POSTData(WrittenTestMarksBindDataDTO, "WrittenTestMarksEntryFacade/Getdetails/");

 
        }

        public WirttenTestSubjectWiseMarksEntryDTO WrittenTestMarksEntryData(WirttenTestSubjectWiseMarksEntryDTO WirttenTestSubjectWiseMarksEntryDTO)
        {
            return COMMMM.POSTData(WirttenTestSubjectWiseMarksEntryDTO, "WrittenTestMarksEntryFacade/");



        }

        public WirttenTestSubjectWiseMarksEntryDTO GetdetailsOnSchedule(WirttenTestSubjectWiseMarksEntryDTO WirttenTestSubjectWiseMarksEntryDTO)
        {
            return COMMMM.POSTData(WirttenTestSubjectWiseMarksEntryDTO, "WrittenTestMarksEntryFacade/GetdetailsOnSchedule/");

        }

        public WrittenTestMarksBindDataDTO GetWrittenTestMarks(WrittenTestMarksBindDataDTO WrittenTestMarksBindDataDTO)
        {

            // StudentDetailsDTO DTO = new StudentDetailsDTO();
            WrittenTestMarksBindDataDTO temp = null;
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:65140/");
            client.DefaultRequestHeaders.Accept.Clear();
           client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            try
            {

                var myContent = JsonConvert.SerializeObject(WrittenTestMarksBindDataDTO);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = client.PostAsync("api/WrittenTestMarksEntryFacade/GetWrittenTestMarks/", byteContent).Result;

                if (response.IsSuccessStatusCode)
                {
                    product = response.Content.ReadAsStringAsync().Result;
                    Console.WriteLine("", product);

                    temp = JsonConvert.DeserializeObject<WrittenTestMarksBindDataDTO>(product, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });

                }
            }
            catch
            {

            }


            return temp;

        }
    }
}
