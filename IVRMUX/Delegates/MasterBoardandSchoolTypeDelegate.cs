using Newtonsoft.Json;
using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CommonLibrary;
namespace corewebapi18072016.Delegates
{
    public class MasterBoardandSchoolTypeDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterBoardDTO, MasterBoardDTO> COMMM = new CommonDelegate<MasterBoardDTO, MasterBoardDTO>();
        CommonDelegate<MasterSchoolTypeDTO, MasterSchoolTypeDTO> COMMMM = new CommonDelegate<MasterSchoolTypeDTO, MasterSchoolTypeDTO>();
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
                HttpResponseMessage response = client.GetAsync("api/MasterBoardandSchoolTypeFacade/" + resource).Result;
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
        public MasterBoardDTO savedetails(MasterBoardDTO mstbrd)
        {
            return COMMM.POSTData(mstbrd, "MasterBoardandSchoolTypeFacade/");
        }
        public MasterSchoolTypeDTO saveschlTypetails(MasterSchoolTypeDTO schltype)
        {

            return COMMMM.POSTData(schltype, "MasterBoardandSchoolTypeFacade/savedschlTypetails");
        }
        public MasterBoardDTO getAll(int id)
        {
            return COMMM.GetDataById(id, "MasterBoardandSchoolTypeFacade/getBoardDet/");
        }
        public MasterSchoolTypeDTO getAllSchoolType(int id)
        {
            MasterSchoolTypeDTO dto = null;
            return COMMMM.GetDataByIdNo(id, dto, "MasterBoardandSchoolTypeFacade/getAllSchoolType/");
        }


        public MasterBoardDTO deleterec(int id)
        {
            return COMMM.DeleteDataById(id, "MasterBoardandSchoolTypeFacade/deletedetails/");
        }
        public MasterSchoolTypeDTO deleteSchoolTyperec(int id)
        {
            return COMMMM.DeleteDataById(id, "MasterBoardandSchoolTypeFacade/deleteSchoolTyperec/");
        }

        public MasterBoardDTO boardDet(int id)
        {
            return COMMM.GetDataById(id, "MasterBoardandSchoolTypeFacade/getdetails/");
        }
        public MasterSchoolTypeDTO schoolTypeDet(int id)
        {
            return COMMMM.GetDataById(id, "MasterBoardandSchoolTypeFacade/schoolTypeDet/");
        }

    }
}
