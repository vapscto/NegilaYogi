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
    public class Atten_Batch_MappingFacadeController : Controller
    {
        public Atten_Batch_MappingInterface _inter;
        public Atten_Batch_MappingFacadeController(Atten_Batch_MappingInterface inter)
        {
            _inter = inter;
        }

        [HttpPost]
        [Route("getalldetails")]
        public Atten_Batch_MappingDTO getalldetails([FromBody] Atten_Batch_MappingDTO data)
        {
            return _inter.getalldetails(data);
        }
        [Route("savedata1")]
        public Atten_Batch_MappingDTO savedata1([FromBody] Atten_Batch_MappingDTO data)
        {
            return _inter.savedata1(data);
        }
        [Route("get_courses")]
        public Atten_Batch_MappingDTO get_courses([FromBody] Atten_Batch_MappingDTO data)
        {
            return _inter.get_courses(data);
        }
        [Route("get_branches")]
        public Atten_Batch_MappingDTO get_branches([FromBody] Atten_Batch_MappingDTO data)
        {
            return _inter.get_branches(data);
        }
        [Route("get_semisters")]
        public Atten_Batch_MappingDTO get_semisters([FromBody] Atten_Batch_MappingDTO data)
        {
            return _inter.get_semisters(data);
        }
        [Route("get_students")]
        public Atten_Batch_MappingDTO get_students([FromBody] Atten_Batch_MappingDTO data)
        {
            return _inter.get_students(data);
        }
        [Route("savedata2")]
        public Atten_Batch_MappingDTO savedata2([FromBody] Atten_Batch_MappingDTO data)
        {
            return _inter.savedata2(data);
        }
        [Route("view_subjects")]
        public Atten_Batch_MappingDTO view_subjects([FromBody] Atten_Batch_MappingDTO data)
        {
            return _inter.view_subjects(data);
        }
        [Route("Deletedetails")]
        public Atten_Batch_MappingDTO Deletedetails([FromBody] Atten_Batch_MappingDTO data)
        {
            return _inter.Deletedetails(data);
        }
    }
}
