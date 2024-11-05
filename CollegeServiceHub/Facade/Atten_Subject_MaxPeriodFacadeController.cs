using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Admission;
using CollegeServiceHub.Interface;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeServiceHub.Facade
{
    [Route("api/[controller]")]
    public class Atten_Subject_MaxPeriodFacadeController : Controller
    {
        public Atten_Subject_MaxPeriodInterface _inter;
        public Atten_Subject_MaxPeriodFacadeController(Atten_Subject_MaxPeriodInterface inter)
        {
            _inter = inter;
        }

        [HttpPost]
        [Route("getalldetails")]
        public Atten_Subject_MaxPeriodDTO getalldetails([FromBody] Atten_Subject_MaxPeriodDTO data)
        {
            return _inter.getalldetails(data);
        }
        [Route("get_courses")]
        public Atten_Subject_MaxPeriodDTO get_courses([FromBody] Atten_Subject_MaxPeriodDTO data)
        {
            return _inter.get_courses(data);
        }
        [Route("get_branches")]
        public Atten_Subject_MaxPeriodDTO get_branches([FromBody] Atten_Subject_MaxPeriodDTO data)
        {
            return _inter.get_branches(data);
        }
        [Route("get_semisters")]
        public Atten_Subject_MaxPeriodDTO get_semisters([FromBody] Atten_Subject_MaxPeriodDTO data)
        {
            return _inter.get_semisters(data);
        }
        [Route("get_subjects")]
        public Atten_Subject_MaxPeriodDTO get_subjects([FromBody] Atten_Subject_MaxPeriodDTO data)
        {
            return _inter.get_subjects(data);
        }
        [Route("savedata")]
        public Atten_Subject_MaxPeriodDTO savedata([FromBody] Atten_Subject_MaxPeriodDTO data)
        {
            return _inter.savedata(data);
        }
        [Route("Deletedetails")]
        public Atten_Subject_MaxPeriodDTO Deletedetails([FromBody] Atten_Subject_MaxPeriodDTO data)
        {
            return _inter.Deletedetails(data);
        }
        [Route("showmodaldetails")]
        public Atten_Subject_MaxPeriodDTO showmodaldetails([FromBody] Atten_Subject_MaxPeriodDTO data)
        {
            return _inter.showmodaldetails(data);
        }
        [Route("deactivesem")]
        public Atten_Subject_MaxPeriodDTO deactivesem([FromBody] Atten_Subject_MaxPeriodDTO data)
        {
            return _inter.deactivesem(data);
        }
        
    }
}
