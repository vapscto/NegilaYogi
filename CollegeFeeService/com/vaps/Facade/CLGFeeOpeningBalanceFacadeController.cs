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
    public class CLGFeeOpeningBalanceFacadeController : Controller
    {
        public CLGFeeOpeningBalanceInterface _inter;
        public CLGFeeOpeningBalanceFacadeController(CLGFeeOpeningBalanceInterface inter)
        {
            _inter = inter;
        }

        [HttpPost]
        [Route("getalldetails")]
        public CLGFeeOpeningBalanceDTO getalldetails([FromBody] CLGFeeOpeningBalanceDTO data)
        {
            return _inter.getalldetails(data);
        }
        [Route("get_courses")]
        public CLGFeeOpeningBalanceDTO get_courses([FromBody] CLGFeeOpeningBalanceDTO data)
        {
            return _inter.get_courses(data);
        }
        [Route("get_branches")]
        public CLGFeeOpeningBalanceDTO get_branches([FromBody] CLGFeeOpeningBalanceDTO data)
        {
            return _inter.get_branches(data);
        }
        [Route("get_semisters")]
        public CLGFeeOpeningBalanceDTO get_semisters([FromBody] CLGFeeOpeningBalanceDTO data)
        {
            return _inter.get_semisters(data);
        }
        [Route("get_groups")]
        public CLGFeeOpeningBalanceDTO get_groups([FromBody] CLGFeeOpeningBalanceDTO data)
        {
            return _inter.get_groups(data);
        }
        [Route("get_heads")]
        public CLGFeeOpeningBalanceDTO get_heads([FromBody] CLGFeeOpeningBalanceDTO data)
        {
            return _inter.get_heads(data);
        }
        [Route("get_installments")]
        public CLGFeeOpeningBalanceDTO get_installments([FromBody] CLGFeeOpeningBalanceDTO data)
        {
            return _inter.get_installments(data);
        }
        [Route("get_students")]
        public CLGFeeOpeningBalanceDTO get_students([FromBody] CLGFeeOpeningBalanceDTO data)
        {
            return _inter.get_students(data);
        }
        [Route("savedata")]
        public CLGFeeOpeningBalanceDTO savedata([FromBody] CLGFeeOpeningBalanceDTO data)
        {
            return _inter.savedata(data);
        }
        [Route("Deletedetails")]
        public CLGFeeOpeningBalanceDTO Deletedetails([FromBody] CLGFeeOpeningBalanceDTO data)
        {
            return _inter.Deletedetails(data);
        }
    }
}
