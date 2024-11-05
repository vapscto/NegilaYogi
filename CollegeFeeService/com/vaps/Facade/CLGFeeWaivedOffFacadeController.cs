using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CollegeFeeService.com.vaps.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.College.Fees;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CollegeFeeService.com.vaps.Facade
{
    [Route("api/[controller]")]
    public class CLGFeeWaivedOffFacadeController : Controller
    {
       public CLGFeeWaivedOffInterface _inter;
        public CLGFeeWaivedOffFacadeController(CLGFeeWaivedOffInterface inf)
        {
            _inter = inf;
        }
        
        [HttpPost]
        [Route("getalldetails")]
        public CLGFeeWaivedOffDTO getalldetails([FromBody] CLGFeeWaivedOffDTO data)
        {
            return _inter.getalldetails(data);
        }
        [Route("get_students")]
        public CLGFeeWaivedOffDTO get_students([FromBody] CLGFeeWaivedOffDTO data)
        {           
            return _inter.get_students(data);
        }
        [Route("get_groups")]
        public CLGFeeWaivedOffDTO get_groups([FromBody] CLGFeeWaivedOffDTO data)
        {
            return _inter.get_groups(data);
        }
        [Route("get_heads")]
        public CLGFeeWaivedOffDTO get_heads([FromBody] CLGFeeWaivedOffDTO data)
        {
            return _inter.get_heads(data);
        }
        [Route("savedata")]
        public CLGFeeWaivedOffDTO savedata([FromBody] CLGFeeWaivedOffDTO data)
        {
            return _inter.savedata(data);
        }
        [Route("EditDetails")]
        public CLGFeeWaivedOffDTO EditDetails([FromBody] CLGFeeWaivedOffDTO data)
        {
            return _inter.EditDetails(data);
        }
        [Route("DeletRecord")]
        public CLGFeeWaivedOffDTO DeletRecord([FromBody] CLGFeeWaivedOffDTO data)
        {
            return _inter.DeletRecord(data);
        }
    }
}
