using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NaacServiceHub.University.Interface;
using PreadmissionDTOs.NAAC.University;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NaacServiceHub.University.FacadeController
{
    [Route("api/[controller]")]
    public class HSU_316_Dept_AwardsFacade : Controller
    {
        public HSU_316_Dept_AwardsInterface _inter;
        public HSU_316_Dept_AwardsFacade(HSU_316_Dept_AwardsInterface i)
        {
            _inter = i;
        }

        [HttpPost]
        [Route("loaddata")]
        public HSU_316_Dept_AwardsDTO loaddata([FromBody] HSU_316_Dept_AwardsDTO data)
        {
            return _inter.loaddata(data);
        }
        [Route("save")]
        public HSU_316_Dept_AwardsDTO save([FromBody] HSU_316_Dept_AwardsDTO data)
        {
            return _inter.save(data);
        }
        [Route("deactive")]
        public HSU_316_Dept_AwardsDTO deactive([FromBody] HSU_316_Dept_AwardsDTO data)
        {
            return _inter.deactive(data);
        }
        [Route("EditData")]
        public HSU_316_Dept_AwardsDTO EditData([FromBody] HSU_316_Dept_AwardsDTO data)
        {
            return _inter.EditData(data);
        }

        [Route("deleteuploadfile")]
        public HSU_316_Dept_AwardsDTO deleteuploadfile([FromBody] HSU_316_Dept_AwardsDTO data)
        {
            return _inter.deleteuploadfile(data);
        }

        [Route("viewuploadflies")]
        public HSU_316_Dept_AwardsDTO viewuploadflies([FromBody] HSU_316_Dept_AwardsDTO data)
        {
            return _inter.viewuploadflies(data);
        }
    }
}
