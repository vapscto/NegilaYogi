using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Interfaces;
using PreadmissionDTOs;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class SeatBlockFacadeController : Controller
    {
        public SeatBlockInterface _Seat;
        public SeatBlockFacadeController(SeatBlockInterface SeatB)
        {
            _Seat = SeatB;
        }
        //// GET: api/values
        [HttpGet("{id:int}")]
        public Preadmission_SeatBlocked_StudentDTO Get(int id)
        {
            return _Seat.AllDropdownList(id);
        }
        [Route("getdetails/{id:int}")]
        //[Route("getenquirycontroller")]
        public Preadmission_SeatBlocked_StudentDTO getSeatBlockdet(int id)
        {
            // id = 12;
            return _Seat.getdetails(id);
        }
        // POST api/values
        [HttpPost]
        public async Task<Preadmission_SeatBlocked_StudentDTO> Post([FromBody] Preadmission_SeatBlocked_StudentDTO SeatBlock)
        {
            return await _Seat.saveSeatBlock(SeatBlock);
        }

        [Route("getdetailsById/{id:int}")]
        public Preadmission_SeatBlocked_StudentDTO getSeatBlockdetById(int id)
        {
            // id = 12;
            return _Seat.getdetails(id);
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public async Task<Preadmission_SeatBlocked_StudentDTO> Deleterec(int id)
        {
            return  await _Seat.deleterec(id);
        }
        [Route("getSeatConfirmedStud")]
        public Preadmission_SeatBlocked_StudentDTO getSeatConfirmedStud([FromBody]Preadmission_SeatBlocked_StudentDTO stud)
        {
            return _Seat.getSeatConfirmedStud(stud);
        }
    }
}
