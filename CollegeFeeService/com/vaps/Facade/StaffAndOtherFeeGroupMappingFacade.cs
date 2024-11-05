using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeFeeService.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fee;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class StaffAndOtherFeeGroupMappingFacade : Controller
    {
        public StaffAndOtherFeeGroupMappingInterface _org;

        public StaffAndOtherFeeGroupMappingFacade(StaffAndOtherFeeGroupMappingInterface orga)
        {
            _org = orga;
        }


        [Route("getalldetails")]
        public Clg_StudentFeeGroupMapping_DTO Getdet([FromBody] Clg_StudentFeeGroupMapping_DTO data)
        {
            return _org.getdata(data);
        }
        [Route("Editdetails/{id:int}")]
        public Clg_StudentFeeGroupMapping_DTO Getmasterdetails(int id)
        {
            return _org.EditMasterscetionDetails(id);
        }

        //[Route("getgroupmappedheads")]
        //public Clg_StudentFeeGroupMapping_DTO getstudentclass([FromBody] Clg_StudentFeeGroupMapping_DTO data)
        //{
        //    return _org.getstucls(data);
        //}


      
        [Route("savedetails_s")]
        public Clg_StudentFeeGroupMapping_DTO savedata_s([FromBody] Clg_StudentFeeGroupMapping_DTO pgmodu)
        {

            return _org.savedetails_s(pgmodu);
        }
        [Route("savedetails_o")]
        public Clg_StudentFeeGroupMapping_DTO savedata_o([FromBody] Clg_StudentFeeGroupMapping_DTO pgmodu)
        {

            return _org.savedetails_o(pgmodu);
        }
        //[HttpPost]
        //[Route("saveeditdata")]
        //public Clg_StudentFeeGroupMapping_DTO saveeditdata([FromBody] Clg_StudentFeeGroupMapping_DTO pgmodu)
        //{
        //    //int trustid = 0;
        //    //if (HttpContext.Session.GetString("pagemoduleid") != null)
        //    //{
        //    //    trustid = Convert.ToInt32(HttpContext.Session.GetString("pagemoduleid"));//Get
        //    //}

        //    //pgmodu.IVRMMP_Id = trustid;
        //    //HttpContext.Session.Remove("pagemoduleid");
        //    return _org.saveeditdata(pgmodu);
        //}
        [HttpPost]
        [Route("getclassoncatselect")]
        public Clg_StudentFeeGroupMapping_DTO getdataaspercat([FromBody]Clg_StudentFeeGroupMapping_DTO value)
        {
            return _org.getdataaspercategory(value);
        }


        [Route("studentsavedgroup")]
        public Clg_StudentFeeGroupMapping_DTO studentsavedgroupfac([FromBody]Clg_StudentFeeGroupMapping_DTO value)
        {
            return _org.studentsavedgroupfacfun(value);
        }


        //[HttpPost("{id}")]
        //public Clg_StudentFeeGroupMapping_DTO Put(int id, [FromBody]Clg_StudentFeeGroupMapping_DTO value)
        //{
        //    return _org.getsearchdata(id, value);
        //}

        [HttpPost]
        [Route("radiobtndata")]
        public Clg_StudentFeeGroupMapping_DTO getradiodata([FromBody]Clg_StudentFeeGroupMapping_DTO value)
        {
            return _org.getradiofiltereddata(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }


        
        [Route("deletemodpages_s")]
        public Clg_StudentFeeGroupMapping_DTO Delete_s([FromBody] Clg_StudentFeeGroupMapping_DTO data)
        {
            return _org.deleterec_s(data);
        }
        [Route("deletemodpages_o")]
        public Clg_StudentFeeGroupMapping_DTO Delete_o([FromBody] Clg_StudentFeeGroupMapping_DTO data)
        {
            return _org.deleterec_o(data);
        }

        //[HttpPost]
        //[Route("searching")]
        //public Clg_StudentFeeGroupMapping_DTO searching([FromBody] Clg_StudentFeeGroupMapping_DTO data)
        //{
        //    return _org.searching(data);
        //}
        [Route("searching_s")]
        public Clg_StudentFeeGroupMapping_DTO searching_s([FromBody] Clg_StudentFeeGroupMapping_DTO data)
        {
            return _org.searching_s(data);
        }
        [Route("searching_o")]
        public Clg_StudentFeeGroupMapping_DTO searching_o([FromBody] Clg_StudentFeeGroupMapping_DTO data)
        {
            return _org.searching_o(data);
        }

        [Route("searchingstud")]
        public Clg_StudentFeeGroupMapping_DTO searchingstud([FromBody] Clg_StudentFeeGroupMapping_DTO data)
        {
            return _org.searchingstu(data);
        }
        [Route("searchingstaff")]
        public Clg_StudentFeeGroupMapping_DTO searchingstaff([FromBody] Clg_StudentFeeGroupMapping_DTO data)
        {
            return _org.searchingstaff(data);
        }
        [Route("searchingothers")]
        public Clg_StudentFeeGroupMapping_DTO searchingothers([FromBody] Clg_StudentFeeGroupMapping_DTO data)
        {
            return _org.searchingothers(data);
        }

        [Route("geteditdata")]
        public Clg_StudentFeeGroupMapping_DTO editstudentdata([FromBody] Clg_StudentFeeGroupMapping_DTO data)
        {
            return _org.editstudata(data);
        }

        [Route("getacademicyear")]
        public Clg_StudentFeeGroupMapping_DTO getacademicye([FromBody] Clg_StudentFeeGroupMapping_DTO data)
        {
            return _org.getacademicyr(data);
        }

        //[Route("fillstudentsroute")]
        //public Clg_StudentFeeGroupMapping_DTO fillstudentsroute([FromBody] Clg_StudentFeeGroupMapping_DTO data)
        //{
        //    return _org.fillstudentsroute(data);
        //}

        [Route("geteditdatastaffothers")]
        public Clg_StudentFeeGroupMapping_DTO geteditdatastaffothers([FromBody] Clg_StudentFeeGroupMapping_DTO data)
        {
            return _org.geteditdatastaffothers(data);
        }

        [Route("saveeditdataothers")]
        public Clg_StudentFeeGroupMapping_DTO saveeditdataothers([FromBody] Clg_StudentFeeGroupMapping_DTO data)
        {
            return _org.saveeditdataothers(data);
        }



        [Route("saveeditdatastaff")]
        public Clg_StudentFeeGroupMapping_DTO saveeditdatastaff([FromBody] Clg_StudentFeeGroupMapping_DTO data)
        {
            return _org.saveeditdatastaff(data);
        }


    }
}
