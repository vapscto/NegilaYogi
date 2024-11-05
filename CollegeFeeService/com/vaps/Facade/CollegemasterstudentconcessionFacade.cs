using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fees;
using CollegeFeeService.com.vaps.Interfaces;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class CollegemasterstudentconcessionFacade : Controller
    {
        public CollegemasterstudentconcessionInterface _ecsimport;

        public CollegemasterstudentconcessionFacade(CollegemasterstudentconcessionInterface maspag)
        {
            _ecsimport = maspag;
        }


        [HttpPost]
        [Route("getdata")]
        public CollegeConcessionDTO getdata([FromBody] CollegeConcessionDTO data)
        {
            return _ecsimport.getdata(data);
        }
        [HttpPost]
        [Route("get_courses")]
        public CollegeConcessionDTO get_courses([FromBody] CollegeConcessionDTO data)
        {
            return _ecsimport.get_courses(data);
        }
        [HttpPost]
        [Route("get_branches")]
        public CollegeConcessionDTO get_branches([FromBody] CollegeConcessionDTO data)
        {
            return _ecsimport.get_branches(data);
        }
        [HttpPost]
        [Route("get_semisters")]
        public CollegeConcessionDTO get_semisters([FromBody] CollegeConcessionDTO data)
        {
            return _ecsimport.get_semisters(data);
        }
        [HttpPost]
        [Route("get_student")]
        public CollegeConcessionDTO get_student([FromBody] CollegeConcessionDTO data)
        {
            return _ecsimport.get_student(data);
        }

        [HttpPost]
        [Route("fillamount")]
        public CollegeConcessionDTO fillamount([FromBody] CollegeConcessionDTO data)
        {
            return _ecsimport.fillamount(data);
        }

        [HttpPost]
        [Route("fillheaddetailsss")]
        public CollegeConcessionDTO fillheaddetailsss([FromBody] CollegeConcessionDTO data)
        {
            return _ecsimport.fillheaddetailsss(data);
        }

        [HttpPost]
        [Route("savedata")]
        public CollegeConcessionDTO savedata([FromBody] CollegeConcessionDTO data)
        {
            return _ecsimport.savedata(data);
        }
        [HttpPost]
        [Route("DeletRecord")]
        public CollegeConcessionDTO DeletRecord([FromBody] CollegeConcessionDTO data)
        {
            return _ecsimport.DeletRecord(data);
        }
    }
    

}
