using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs;
using corewebapi18072016.Delegates;

namespace corewebapi18072016.Controllers
{
    [ValidateAntiForgeryToken]
    [Route("api/[controller]")]
    public class SeatBlockController : Controller
    {

        SeatBlockDelegate SeatBlock = new SeatBlockDelegate();
        // POST api/values
        [HttpPost]
        public Preadmission_SeatBlocked_StudentDTO SaveSeatBlock([FromBody] Preadmission_SeatBlocked_StudentDTO SeatB)
        {
            return SeatBlock.saveSeatBlockdetails(SeatB);
        }
        // GET: api/values
        [Route("getalldetails/{id:int}")]
        public Preadmission_SeatBlocked_StudentDTO Get([FromQuery] int id)
        {
            id = Convert.ToInt32(HttpContext.Session.GetInt32("Session_MI_Id"));
            return SeatBlock.getSeatBlockdata(id);
        }


        [Route("getSeatBlockById/{id:int}")]
        public Preadmission_SeatBlocked_StudentDTO GetSeatBlockDetailsById(int id)
        {
            // id = 12;
            return SeatBlock.getSeatBlockDetailsbySeatBlockId(id);
        }

        // DELETE api/values/5
        [HttpDelete]
        [Route("deletedetails/{id:int}")]
        public Preadmission_SeatBlocked_StudentDTO Delete(int id)
        {
            return SeatBlock.deleterec(id);
        }
        [Route("getSeatConfirmedStud")]
        public Preadmission_SeatBlocked_StudentDTO getSeatConfirmedStud([FromBody] Preadmission_SeatBlocked_StudentDTO stud)
        {
            return SeatBlock.getSeatConfirmedStud(stud);
        }
    }
}