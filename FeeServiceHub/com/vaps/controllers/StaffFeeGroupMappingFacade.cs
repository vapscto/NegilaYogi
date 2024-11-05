using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using PreadmissionDTOs.com.vaps.Fees;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class StaffFeeGroupMappingFacade : Controller
    {
        public StaffFeeGroupMappingInterface _org;

        public StaffFeeGroupMappingFacade(StaffFeeGroupMappingInterface orga)
        {
            _org = orga;
        }


        [Route("getalldetails")]
        public FeeStudentGroupMappingDTO Getdet([FromBody] FeeStudentGroupMappingDTO data)
        {
            return _org.getdata(data);
        }
        [Route("Editdetails/{id:int}")]
        public FeeStudentGroupMappingDTO Getmasterdetails(int id)
        {
            return _org.EditMasterscetionDetails(id);
        }

        [Route("getgroupmappedheads")]
        public FeeStudentGroupMappingDTO getstudentclass([FromBody] FeeStudentGroupMappingDTO data)
        {
            return _org.getstucls(data);
        }


        [HttpPost]
        public FeeStudentGroupMappingDTO savedata([FromBody] FeeStudentGroupMappingDTO pgmodu)
        {
            //int trustid = 0;
            //if (HttpContext.Session.GetString("pagemoduleid") != null)
            //{
            //    trustid = Convert.ToInt32(HttpContext.Session.GetString("pagemoduleid"));//Get
            //}

            //pgmodu.IVRMMP_Id = trustid;
            //HttpContext.Session.Remove("pagemoduleid");
            return _org.savedetails(pgmodu);
        }
        [HttpPost]
        [Route("saveeditdata")]
        public FeeStudentGroupMappingDTO saveeditdata([FromBody] FeeStudentGroupMappingDTO pgmodu)
        {
            //int trustid = 0;
            //if (HttpContext.Session.GetString("pagemoduleid") != null)
            //{
            //    trustid = Convert.ToInt32(HttpContext.Session.GetString("pagemoduleid"));//Get
            //}

            //pgmodu.IVRMMP_Id = trustid;
            //HttpContext.Session.Remove("pagemoduleid");
            return _org.saveeditdata(pgmodu);
        }
        [HttpPost]
        [Route("getclassoncatselect")]
        public FeeStudentGroupMappingDTO getdataaspercat([FromBody]FeeStudentGroupMappingDTO value)
        {
            return _org.getdataaspercategory(value);
        }


        [Route("studentsavedgroup")]
        public FeeStudentGroupMappingDTO studentsavedgroupfac([FromBody]FeeStudentGroupMappingDTO value)
        {
            return _org.studentsavedgroupfacfun(value);
        }


        [HttpPost("{id}")]
        public FeeStudentGroupMappingDTO Put(int id, [FromBody]FeeStudentGroupMappingDTO value)
        {
            return _org.getsearchdata(id, value);
        }

        [HttpPost]
        [Route("radiobtndata")]
        public FeeStudentGroupMappingDTO getradiodata([FromBody]FeeStudentGroupMappingDTO value)
        {
            return _org.getradiofiltereddata(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {

        }


        [Route("deletemodpages")]
        public FeeStudentGroupMappingDTO Delete([FromBody] FeeStudentGroupMappingDTO data)
        {
            return _org.deleterec(data);
        }

        [HttpPost]
        [Route("searching")]
        public FeeStudentGroupMappingDTO searching([FromBody] FeeStudentGroupMappingDTO data)
        {
            return _org.searching(data);
        }

        [Route("searchingstud")]
        public FeeStudentGroupMappingDTO searchingstud([FromBody] FeeStudentGroupMappingDTO data)
        {
            return _org.searchingstu(data);
        }

        [Route("geteditdata")]
        public FeeStudentGroupMappingDTO editstudentdata([FromBody] FeeStudentGroupMappingDTO data)
        {
            return _org.editstudata(data);
        }
    }
}
