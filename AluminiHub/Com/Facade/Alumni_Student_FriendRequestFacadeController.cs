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
    public class Alumni_Student_FriendRequestFacadeController : Controller
    {
        Alumni_Student_FriendRequestInterface _inter;
        public Alumni_Student_FriendRequestFacadeController(Alumni_Student_FriendRequestInterface intr)
        {
            _inter = intr;
        }
        //Alumnim Request send
        [Route("getdata")]
        public Alumni_FriendRequestDTO getdata([FromBody] Alumni_FriendRequestDTO dto)
        {
            return _inter.getdata(dto);
        }
        [Route("getsearch_data")]
        public Alumni_FriendRequestDTO getsearch_data([FromBody] Alumni_FriendRequestDTO dto)
        {
            return _inter.getsearch_data(dto);
        }
         [Route("sendrequest")]
        public Alumni_FriendRequestDTO sendrequest([FromBody] Alumni_FriendRequestDTO dto)
        {
            return _inter.sendrequest(dto);
        }
        [Route("viewprofile")]
        public Alumni_FriendRequestDTO viewprofile([FromBody] Alumni_FriendRequestDTO dto)
        {
            return _inter.viewprofile(dto);
        }

        //Alumnim Request accept

        [Route("getdata_request")]
        public Alumni_FriendRequestDTO getdata_request([FromBody] Alumni_FriendRequestDTO dto)
        {
            return _inter.getdata_request(dto);
        }
        [Route("FriendrequestAccept")]
        public Alumni_FriendRequestDTO FriendrequestAccept([FromBody] Alumni_FriendRequestDTO dto)
        {
            return _inter.FriendrequestAccept(dto);
        }

    }
}