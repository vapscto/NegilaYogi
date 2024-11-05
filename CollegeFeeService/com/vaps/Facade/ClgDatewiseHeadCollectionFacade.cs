using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CollegeFeeService.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.College.Fee;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class ClgDatewiseHeadCollectionFacade : Controller
    {
        public ClgDatewiseHeadCollectionInterface objInterface;

        public ClgDatewiseHeadCollectionFacade(ClgDatewiseHeadCollectionInterface bdInterface)
        {
            objInterface = bdInterface;
        }
        // GET: api/values

        [HttpGet]
        [Route("GetYearList/{id:int}")]
        public ClgDatewiseHeadCollectionDTO GetYearList(int id)
        {
            return objInterface.GetYearList(id);
        }

        [HttpPost]
        [Route("get_feegroups")]
        public ClgDatewiseHeadCollectionDTO get_feegroups([FromBody]ClgDatewiseHeadCollectionDTO id)
        {
            return objInterface.get_feegroups(id);
        }

        ////[HttpPost]
        [Route("get_heads")]
        public ClgDatewiseHeadCollectionDTO get_heads([FromBody]ClgDatewiseHeadCollectionDTO id)
        {
            return objInterface.get_heads(id);
        }
        ////[HttpPost]
        [Route("get_semisters")]
        public ClgDatewiseHeadCollectionDTO get_semisters([FromBody] ClgDatewiseHeadCollectionDTO data)
        {
            return objInterface.get_semisters(data);
        }
        ////[HttpPost]
        [Route("get_report")]
        public ClgDatewiseHeadCollectionDTO get_report([FromBody] ClgDatewiseHeadCollectionDTO data)
        {
            return objInterface.get_report(data);
        }

        [Route("savedata")]
        public ClgDatewiseHeadCollectionDTO savedata([FromBody] ClgDatewiseHeadCollectionDTO data)
        {
            return objInterface.savedata(data);
        }

        [Route("editdata")]
        public ClgDatewiseHeadCollectionDTO editdata([FromBody]ClgDatewiseHeadCollectionDTO data)
        {
            return objInterface.editdata(data);
        }

        [Route("DeleteRecord")]
        public ClgDatewiseHeadCollectionDTO DeleteRecord([FromBody] ClgDatewiseHeadCollectionDTO data)
        {
            return objInterface.DeleteRecord(data);
        }
    }
}
