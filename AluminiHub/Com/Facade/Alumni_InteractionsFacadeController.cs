using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlumniHub.Com.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Alumni;

namespace AlumniHub.Com.Facade
{
   
    [Route("api/[controller]")]
    public class Alumni_InteractionsFacadeController : Controller
    {
        public Alumni_Interactions_Interface _inter;
        public Alumni_InteractionsFacadeController(Alumni_Interactions_Interface inter)
        {
            _inter = inter;
        }
        [Route("getloaddata")]
        public Alumni_School_Interactions_DTO getloaddata([FromBody]Alumni_School_Interactions_DTO dto)
        {
            return _inter.getloaddata(dto);
        }
        [Route("getdetails")]
        public Alumni_School_Interactions_DTO getdetails([FromBody]Alumni_School_Interactions_DTO dto)
        {
            return _inter.getdetails(dto);
        }
        [Route("savedetails")]
        public Alumni_School_Interactions_DTO savedetails([FromBody]Alumni_School_Interactions_DTO dto)
        {
            return _inter.savedetails(dto);
        }
        [Route("reply")]
        public Alumni_School_Interactions_DTO reply([FromBody]Alumni_School_Interactions_DTO dto)
        {
            return _inter.reply(dto);
        }
        [Route("savereply")]
        public Alumni_School_Interactions_DTO savereply([FromBody]Alumni_School_Interactions_DTO dto)
        {
            return _inter.savereply(dto);
        }

    }
}