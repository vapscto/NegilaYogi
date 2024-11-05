using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class StudentFeeGroupMappingGroupDeletionFacade : Controller
    {
        public StudentFeeGroupMappingGroupDeletionInterface _org;
        public StudentFeeGroupMappingGroupDeletionFacade(StudentFeeGroupMappingGroupDeletionInterface orga)
        {
            _org = orga;
        }

        //[HttpPost]
        [Route("getalldetails")]
        public FeeStudentGroupMappingDTO Getdet([FromBody] FeeStudentGroupMappingDTO data)
        {
            return _org.getdata(data);
        }
        [Route("deletemodpages")]
        public FeeStudentGroupMappingDTO Delete([FromBody] FeeStudentGroupMappingDTO data)
        {
            return _org.deleterecord(data);
        }
        [Route("Getreport")]
        public FeeStudentGroupMappingDTO onclickAcademic([FromBody] FeeStudentGroupMappingDTO data)
        {
            return _org.Getreport(data);
        }
        [Route("onclickClass")]
        public FeeStudentGroupMappingDTO onclickClass([FromBody] FeeStudentGroupMappingDTO data)
        {
            return _org.onclickClass(data);
        }


    }
}
