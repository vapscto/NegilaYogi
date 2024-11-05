using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FrontOfficeHub.com.vaps.Interfaces;
using PreadmissionDTOs.com.vaps.FrontOffice;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace FrontOfficeHub.com.vaps.Controllers
{
    [Route("api/[controller]")]
    public class MasterHolidayFacade : Controller
    {
        MasterHolidayInterface _msthldy;
        public MasterHolidayFacade(MasterHolidayInterface mhi)
        {
            _msthldy = mhi;
        }

        [Route("getdata/{id:int}")]
        public MasterHolidayDTO getdata(int id)
        {
            return _msthldy.getdata(id);
        }

        [Route("savedetail")]
        public MasterHolidayDTO savedetail([FromBody] MasterHolidayDTO categorypage)
        {
            return _msthldy.save_details(categorypage);
        }

        [Route("Change")]
        public Task<MasterHolidayDTO> Change([FromBody] MasterHolidayDTO categorypage)
        {
            return _msthldy.Change(categorypage);
        }

        [Route("getdetails/{id:int}")]
        public MasterHolidayDTO getorgdet(int id)
        {
            return _msthldy.getdetails(id);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        [Route("delete")]
        public MasterHolidayDTO delete_date([FromBody] MasterHolidayDTO obj)
        {
            return _msthldy.delete_data(obj);
        }

        [Route("advloaddata")]
        public MasterHolidayDTO advloaddata([FromBody] MasterHolidayDTO obj)
        {
            return _msthldy.advloaddata(obj);
        }

        [Route("saveadvmasterHolidaydata")]
        public MasterHolidayDTO saveadvmasterHolidaydata([FromBody] MasterHolidayDTO obj)
        {
            return _msthldy.saveadvmasterHolidaydata(obj);
        }
        
        [Route("advdelete/{id:int}")]
        public MasterHolidayDTO advdelete(int id)
        {
            return _msthldy.advdelete(id);
        }

        [Route("editadvmasterHoliday")]
        public MasterHolidayDTO editadvmasterHoliday([FromBody] MasterHolidayDTO obj)
        {
            return _msthldy.editadvmasterHoliday(obj);
        }
    }
}
