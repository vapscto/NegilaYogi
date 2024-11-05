using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;
using FeeServiceHub.com.vaps.interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FeeServiceHub.com.vaps.controllers
{
    [Route("api/[controller]")]
    public class Student_SettlementFacadeController : Controller
    {
        public Student_SettlementInterface _inter;
        public Student_SettlementFacadeController(Student_SettlementInterface inter)
        {
            _inter = inter;
        }
        [HttpPost]
        [Route("Getdetails")]
        public Student_SettlementDTO Getdetails([FromBody] Student_SettlementDTO data)
        {
            return _inter.Getdetails(data);
        }
        [Route("getdates")]
        public Student_SettlementDTO getdates([FromBody] Student_SettlementDTO data)
        {
            return _inter.getdates(data);
        }
        [Route("savedata")]
        public Student_SettlementDTO savedata([FromBody] Student_SettlementDTO data)
        {
            return _inter.savedata(data);
        }
        [Route("viewrecords")]
        public Student_SettlementDTO viewrecords([FromBody] Student_SettlementDTO data)
        {
            return _inter.viewrecords(data);
        }
        [Route("get_classes")]
        public Student_SettlementDTO get_classes([FromBody] Student_SettlementDTO data)
        {
            return _inter.get_classes(data);
        }
        [Route("get_sections")]
        public Student_SettlementDTO get_sections([FromBody] Student_SettlementDTO data)
        {
            return _inter.get_sections(data);
        }
        [Route("get_routes")]
        public Student_SettlementDTO get_routes([FromBody] Student_SettlementDTO data)
        {
            return _inter.get_routes(data);
        }
        [Route("getreport")]
        public Student_SettlementDTO getreport([FromBody] Student_SettlementDTO data)
        {
            return _inter.getreport(data);
        }

        [Route("getreport1")]
        public Task<Student_SettlementDTO> getreport1([FromBody] Student_SettlementDTO data)
        {
            return _inter.getreport1(data);
        }


        [Route("fillmerchants")]
        public Student_SettlementDTO fillmerchants([FromBody] Student_SettlementDTO data)
        {
            return _inter.fillmerchants(data);
        }

        [Route("paymentlogs")]
        public Student_SettlementDTO paymentlogs([FromBody] Student_SettlementDTO data)
        {
            return _inter.paymentlogs(data);
        }

    }
}
