using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using corewebapi18072016.Delegates;
using DomainModel.Model;
using PreadmissionDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]

    public class EnquiryController : Controller
    {
        EnquiryDelegate enqu = new EnquiryDelegate();

     

        [HttpPost]
        [Route("getalldetails")]
        public Enq Get([FromBody] Enq en)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            en.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            en.Id = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            en.ASMAY_Id = ASMAY_Id;

            return enqu.getcountrydata(en);
            //return enq;
        }
        //[Route("getalldetails/{id:int}")]
        //public Enq getdetails(int id)
        //{
        //    return enqu.getalldetails(id);
        //}

        [HttpGet]
        [Route("getenquirycontroller/{id:int}")]
        public StateDTO Getstates(int id)
        {
            //int drpdata = en.countryid;
            //EnqDTO enq=new EnqDTO();
            return enqu.enqdatacountrydrp(id);
            //return enq;
        }

        [Route("getenquirystatecontroller/{id:int}")]
        public CityDTO getcity(int id)
        {
            return enqu.cityfill(id);
        }
        [Route("GetEnqdetails/{id:int}")]
        public Enq getdetail(int id)
        {
            //HttpContext.Session.SetString("enqid", id.ToString()); //Set
            // id = 12;
            return enqu.Editdetailss(id);
            //HttpContext.Session.SetString("institutionid","0"); //Set
        }

        [HttpPost]
        public Enq saveEnqdetail([FromBody] Enq en)
        {   
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            en.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            en.Id = UserId;

            //int enqid = 0;
            //if (HttpContext.Session.GetString("enqid") != null)
            //{
            //    enqid = Convert.ToInt32(HttpContext.Session.GetString("enqid"));//Get
            //    en.PASE_Id = enqid;
            //    HttpContext.Session.Remove("enqid");
            //}
            return enqu.saveEnqdetails(en);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }

        // DELETE api/values/5
        [HttpPost]
        [Route("DeleteDetails")]
        public Enq Delete([FromBody] Enq en)
        {
            return enqu.DeleteEnqrecord(en);
        }
        public Enq clear(int id)
        {
            return enqu.clearEnqdata(id);

        }


        //Dashboard Maping
        [HttpGet]
        [Route("getstoragedetails")]
        public dasAzure_StorageDTO getstoragedetails()
        {
            dasAzure_StorageDTO dto = new dasAzure_StorageDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            dto.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            dto.Userid = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            dto.ASMAY_Id = ASMAY_Id;

            return enqu.getstoragedata(dto);
            //return enq;
        }

        [Route("editstorage/{Id:int}")]
        public dasAzure_StorageDTO editstorage(int id)
        {
            return enqu.editstorage(id);
        }

        [HttpPost]
        [Route("saveStoragedetail")]
        public dasAzure_StorageDTO saveStoragedetail([FromBody] dasAzure_StorageDTO en)
        {
           
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            en.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            en.Userid = UserId;

          
            return enqu.saveStoragedetail(en);
        }

        [HttpPost]
        [Route("saveMappingDetails")]
        public dasMappingDTO saveMappingdetail([FromBody] dasMappingDTO en)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            en.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            en.Userid = UserId;


            return enqu.saveMappingdetail(en);
        }


        [HttpGet]
        [Route("getmappingedit/{Id:int}")]
        public dasMappingDTO getmappingedit(int id)
        {
            return enqu.getmappingedit(id);
        }

        [Route("getpremappingedit")]
        public dasMappingDTO getpremappingedit([FromBody] dasMappingDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Userid = UserId;
            return enqu.getpremappingedit(data);
        }
        [HttpGet]
        [Route("deletemappingrecord/{Id:int}")]
        public dasMappingDTO deletemappingrecord(int id)
        {
            return enqu.deletemappingrecord(id)
;
        }

        [Route("deletepremappingrecord")]
        public dasMappingDTO deletepremappingrecord([FromBody] dasMappingDTO data)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            data.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            data.Userid = UserId;
            return enqu.deletepremappingrecord(data);
        }

        //Rolewise institutuion mapping

        [HttpPost]
        [Route("deletegriddata")]
        public IVRM_User_Login_InstitutionwiseDTO deletegriddata([FromBody] IVRM_User_Login_InstitutionwiseDTO en)
        {
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            en.MI_Id = mid;
            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            en.Userid = UserId;
            return enqu.deletegriddata(en);
        }
        [HttpPost]
        [Route("getuser")]
        public IVRM_User_Login_InstitutionwiseDTO getuser([FromBody] IVRM_User_Login_InstitutionwiseDTO en)
        {
           // IVRM_User_Login_InstitutionwiseDTO dto = new IVRM_User_Login_InstitutionwiseDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            en.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            en.Userid = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            en.ASMAY_Id = ASMAY_Id;

            return enqu.getuser(en);
            //return enq;
        }


        [Route("getinstitution")]
        public IVRM_User_Login_InstitutionwiseDTO getinstitution([FromBody] IVRM_User_Login_InstitutionwiseDTO en)
        {
            // IVRM_User_Login_InstitutionwiseDTO dto = new IVRM_User_Login_InstitutionwiseDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            en.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            en.Userid = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            en.ASMAY_Id = ASMAY_Id;

            return enqu.getinstitution(en);
            //return enq;
        }

        [Route("getcartdata")]
        public IVRM_User_Login_InstitutionwiseDTO getcartdata([FromBody] IVRM_User_Login_InstitutionwiseDTO en)
        {
            // IVRM_User_Login_InstitutionwiseDTO dto = new IVRM_User_Login_InstitutionwiseDTO();
            int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            en.MI_Id = mid;

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            en.Userid = UserId;

            int ASMAY_Id = Convert.ToInt32(HttpContext.Session.GetInt32("ASMAY_Id"));
            en.ASMAY_Id = ASMAY_Id;

            return enqu.getcartdata(en);
            //return enq;
        }
        [HttpPost]
        [Route("saveThirdData")]
        public IVRM_User_Login_InstitutionwiseDTO saveThirdData([FromBody] IVRM_User_Login_InstitutionwiseDTO en)
        {
            //int mid = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            //en.MI_Id = mid;

            //int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            //en.Userid = UserId;


            return enqu.savethirdDetail(en);
        }

        [HttpPost]
        [Route("savepreadmissionmapping")]
        public dasMappingDTO savepreadmissionmapping([FromBody] dasMappingDTO en)
        {

            int UserId = Convert.ToInt32(HttpContext.Session.GetInt32("UserId"));
            en.Userid = UserId;


            return enqu.savepreadmissionDetail(en);
        }


    }
}
