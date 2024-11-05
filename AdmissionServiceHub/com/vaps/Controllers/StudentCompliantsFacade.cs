using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdmissionServiceHub.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.admission;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdmissionServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class StudentCompliantsFacade : Controller
    {
        public StudentCompliantsInterface _inter;
      public StudentCompliantsFacade(StudentCompliantsInterface inter)
        {
            _inter = inter;
        }

        [Route("loaddata")]
        public Task<StudentCompliants_DTO> loaddata([FromBody] StudentCompliants_DTO data)
        {
            return _inter.loaddata(data);
        }
        [Route("getstudents")]
        public Task<StudentCompliants_DTO> getstudents([FromBody] StudentCompliants_DTO data)
        {
            return _inter.getstudents(data);
        }
        [Route("searchfilter")]
        public StudentCompliants_DTO searchfilter([FromBody] StudentCompliants_DTO data)
        {
            return _inter.searchfilter(data);
        }
        [Route("edittab1")]
        public StudentCompliants_DTO edittab1([FromBody] StudentCompliants_DTO data)
        {
            return _inter.edittab1(data);
        }
        [Route("getorganizationdata")]
        public StudentCompliants_DTO getorganizationdata([FromBody] StudentCompliants_DTO data)
        {
            return _inter.getorganizationdata(data);
        }

        [Route("getstudentdetails")]
        public StudentCompliants_DTO getstudentdetails([FromBody] StudentCompliants_DTO data)
        {
            return _inter.getstudentdetails(data);
        }

        [Route("deactive")]
        public StudentCompliants_DTO deactive([FromBody] StudentCompliants_DTO data)
        {
            return _inter.deactive(data);
        }

        [Route("save")]
        public StudentCompliants_DTO save([FromBody] StudentCompliants_DTO data)
        {
            return _inter.save(data);
        }

        [Route("report")]
        public StudentCompliants_DTO report([FromBody] StudentCompliants_DTO data)
        {
            return _inter.report(data);
        }
        [Route("deletedetailsY")]
        public StudentCompliants_DTO DeleterecY([FromBody] StudentCompliants_DTO data)
        {
            return _inter.deleterecY(data);
        }
        [Route("editdetails")]
        public StudentCompliants_DTO editdetails([FromBody] StudentCompliants_DTO data)
        {
            return _inter.editdetails(data);
        }

    }
}
