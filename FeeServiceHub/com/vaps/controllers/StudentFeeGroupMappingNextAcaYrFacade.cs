using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FeeServiceHub.com.vaps.interfaces;
using FeeServiceHub.com.vaps.services;
using PreadmissionDTOs.com.vaps.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class StudentFeeGroupMappingNextAcaYrFacade : Controller
    {
        public StudentFeeGroupMappingNextAcaYrInterface _org;
        public StudentFeeGroupMappingNextAcaYrFacade(StudentFeeGroupMappingNextAcaYrInterface orga)
        {
            _org = orga;
        }


        [Route("getalldetails")]
        public FeeStudentGroupMappingDTO Getdet([FromBody] FeeStudentGroupMappingDTO data)
        {
            return _org.getdata(data);
        }
        [Route("getgroupmappedheads")]
        public FeeStudentGroupMappingDTO getgroupmappedheads([FromBody] FeeStudentGroupMappingDTO data)
        {
            return _org.getgroupmappedheads(data);
        }

        [Route("savedetails")]
        public FeeStudentGroupMappingDTO savedetails([FromBody] FeeStudentGroupMappingDTO data)
        {
            return _org.savedetails(data);
        }
        [Route("searching")]
        public FeeStudentGroupMappingDTO searching([FromBody] FeeStudentGroupMappingDTO data)
        {
            return _org.searching(data);
        }

        [Route("Deletedetails")]
        public FeeStudentGroupMappingDTO Deletedetails([FromBody] FeeStudentGroupMappingDTO data)
        {
            return _org.Deletedetails(data);
        }
        
    }
}
