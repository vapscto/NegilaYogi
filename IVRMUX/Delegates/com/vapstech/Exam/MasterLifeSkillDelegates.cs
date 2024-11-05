
using System;
using System.Net.Http;
using PreadmissionDTOs.com.vaps.Exam;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class MasterLifeSkillDelegates
    {

        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterLifeSkillDTO, MasterLifeSkillDTO> _comm = new CommonDelegate<MasterLifeSkillDTO, MasterLifeSkillDTO>();


        public string getData(long resource)
        {
            string product = "";
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:50257/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
            // HTTP GET
            try
            {
                HttpResponseMessage response = client.GetAsync("api/MasterLifeSkillFacadeController/" + resource).Result;
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

        public MasterLifeSkillDTO Getdetails(MasterLifeSkillDTO data)
        {
            return _comm.POSTDataExam(data, "MasterLifeSkillFacade/Getdetails/");
        }
        public MasterLifeSkillDTO editdetails(MasterLifeSkillDTO data)
        {
            return _comm.POSTDataExam(data, "MasterLifeSkillFacade/editdetails/");
        }
        public MasterLifeSkillDTO savedata(MasterLifeSkillDTO data)
        {
            return _comm.POSTDataExam(data, "MasterLifeSkillFacade/savedata/");
        }     
        public MasterLifeSkillDTO deactivate(MasterLifeSkillDTO data)
        {
            return _comm.POSTDataExam(data, "MasterLifeSkillFacade/deactivate/");
        }

        //Master Life Skill Area

        public MasterLifeSkillDTO Savedataarea(MasterLifeSkillDTO data)//Int32 IVRMM_Id
        {
            return _comm.POSTDataExam(data, "MasterLifeSkillFacade/Savedataarea/");
        }
        public MasterLifeSkillDTO editdetailsarea(MasterLifeSkillDTO data)//Int32 IVRMM_Id
        {
            return _comm.POSTDataExam(data, "MasterLifeSkillFacade/editdetailsarea/");
        }
        public MasterLifeSkillDTO deactivatearea(MasterLifeSkillDTO data)//Int32 IVRMM_Id
        {
            return _comm.POSTDataExam(data, "MasterLifeSkillFacade/deactivatearea/");
        }
        public MasterLifeSkillDTO validateordernumber(MasterLifeSkillDTO data)//Int32 IVRMM_Id
        {
            return _comm.POSTDataExam(data, "MasterLifeSkillFacade/validateordernumber/");
        }

        //Master Life Skill Area Mapping

        public MasterLifeSkillDTO Savedataareamapping(MasterLifeSkillDTO data)//Int32 IVRMM_Id
        {
            return _comm.POSTDataExam(data, "MasterLifeSkillFacade/Savedataareamapping/");
        }
        public MasterLifeSkillDTO editdetailsareamapping(MasterLifeSkillDTO data)//Int32 IVRMM_Id
        {
            return _comm.POSTDataExam(data, "MasterLifeSkillFacade/editdetailsareamapping/");
        }
        public MasterLifeSkillDTO deactivateareamapping(MasterLifeSkillDTO data)//Int32 IVRMM_Id
        {
            return _comm.POSTDataExam(data, "MasterLifeSkillFacade/deactivateareamapping/");
        }
        public MasterLifeSkillDTO getgrade(MasterLifeSkillDTO data)//Int32 IVRMM_Id
        {
            return _comm.POSTDataExam(data, "MasterLifeSkillFacade/getgrade/");
        }

        
    }

}
