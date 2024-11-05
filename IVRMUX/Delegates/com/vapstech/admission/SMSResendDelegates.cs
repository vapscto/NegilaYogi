using System;
using System.Net.Http;
using PreadmissionDTOs.com.vaps.admission;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class SMSResendDelegates
    {

        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<SMSResendDTO, SMSResendDTO> COMMM = new CommonDelegate<SMSResendDTO, SMSResendDTO>();
        public string getData(long resource)
        {
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:53497/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");

            // HTTP GET
            try
            {
                HttpResponseMessage response = client.GetAsync("api/SMSResendFacadeController/" + resource).Result;
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

        public SMSResendDTO Getdetailsstatus(SMSResendDTO lo)
        {
            return COMMM.POSTDataADM(lo, "SMSResendFacade/Getdetailsstatus");

           
        }
        public SMSResendDTO Gettransnostatus(SMSResendDTO lo)
        {
            return COMMM.POSTDataADM(lo, "SMSResendFacade/Gettransnostatus");

           
        }
        public SMSResendDTO getstatusreport(SMSResendDTO lo)
        {
            return COMMM.POSTDataADM(lo, "SMSResendFacade/getstatusreport");

           
        }

        public SMSResendDTO Getdetails(SMSResendDTO lo)
        {
            return COMMM.POSTDataADM(lo, "SMSResendFacade/Getdetails");

           
        }

        public SMSResendDTO Gettransno(SMSResendDTO lo)
        {
            return COMMM.POSTDataADM(lo, "SMSResendFacade/Gettransno");


        }
        public SMSResendDTO showdata(SMSResendDTO lo)
        {
            return COMMM.POSTDataADM(lo, "SMSResendFacade/showdata");


        }
        public SMSResendDTO resendMsg(SMSResendDTO lo)
        {
            return COMMM.POSTDataADM(lo, "SMSResendFacade/resendMsg");


        }


        
        public SMSResendDTO GetSelectedRowDetails(int ID)//Int32 AMA_Id
        {

            return COMMM.GetDataByIdADM(ID, "SMSResendFacade/GetSelectedRowDetails/");

          
        }

        
        public SMSResendDTO mastercasteData(SMSResendDTO SMSResendDTO)//Int32 IVRMM_Id
        {

            return COMMM.POSTDataADM(SMSResendDTO, "SMSResendFacade/");

           // string product = "";
           // HttpClient client = new HttpClient();
           // client.BaseAddress = new Uri("http://localhost:53497/");
           // client.DefaultRequestHeaders.Accept.Clear();
           //client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
           // //HTTP POST
           // try
           // {              

           //     var myContent = JsonConvert.SerializeObject(SMSResendDTO);//IVRMM_Id
           //     var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
           //     var byteContent = new ByteArrayContent(buffer);
           //     byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");            
           //     var response = client.PostAsync("api/SMSResendFacade/", byteContent).Result;
           //     if (response.IsSuccessStatusCode)
           //     {
           //         product = response.Content.ReadAsStringAsync().Result;
           //         Console.WriteLine("", product);
           //     }
           // }
           // catch
           // {

           // }


           // return SMSResendDTO;
        
        }

        public SMSResendDTO MasterDeleteModulesData(int ID)//Int32 IVRMM_Id
        {

            return COMMM.DeleteDataByIdADM(ID, "SMSResendFacade/MasterDeleteModulesDATA/");

           // SMSResendDTO SMSResendDTO = new SMSResendDTO();
           // string product = "";
           // HttpClient client = new HttpClient();
           // client.BaseAddress = new Uri("http://localhost:53497/");
           // client.DefaultRequestHeaders.Accept.Clear();
           //client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
           // //HTTP POST
           // try
           // {
               

           //     var myContent = JsonConvert.SerializeObject(ID);//IVRMM_Id
           //     var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);


           //     var byteContent = new ByteArrayContent(buffer);
           //     byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json"); 
           //     var response = client.DeleteAsync("api/SMSResendFacade/MasterDeleteModulesDATA/" + ID).Result;     
           //     if (response.IsSuccessStatusCode)
           //     {
           //         product = response.Content.ReadAsStringAsync().Result;
           //         Console.WriteLine("", product);
           //     }
           // }
           // catch
           // {

           // }


           // return SMSResendDTO;

        }

    }
}
