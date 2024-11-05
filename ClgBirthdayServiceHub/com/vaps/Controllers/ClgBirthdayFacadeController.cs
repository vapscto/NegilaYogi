using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.BirthDay;
using ClgBirthdayServiceHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.College.BirthDay;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace ClgBirthdayServiceHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class ClgBirthdayFacadeController : Controller
    {

        public ClgBirthdayInterface _clgbirthday;
        public  ClgBirthdayFacadeController(ClgBirthdayInterface clgbirthday)
        {
            _clgbirthday = clgbirthday;
        }
        [Route("getloaddata")]
        public ClgBirthDayDTO getloaddata([FromBody] ClgBirthDayDTO data)
        {
            return _clgbirthday.getloaddata(data);
        }
        [Route("radiochange")]
        public Task<ClgBirthDayDTO> radiochange([FromBody] ClgBirthDayDTO data)
        {
            return _clgbirthday.radiochange(data);
        }
        [Route("sendmsg")]
        public Task<ClgBirthDayDTO> sendmsg([FromBody] ClgBirthDayDTO data)
        {
            return _clgbirthday.sendmsg(data);
        }
        [Route("BindStaff")]
        public ClgBirthDayDTO BindStaff([FromBody] ClgBirthDayDTO data)
        {
            return _clgbirthday.staflist(data);
        }

        [Route("clg_getBirthday/{id:int}")]
        public void clg_getBirthday(int id)
        {
            _clgbirthday.clg_getBirthday(id);
        }


    }
}
