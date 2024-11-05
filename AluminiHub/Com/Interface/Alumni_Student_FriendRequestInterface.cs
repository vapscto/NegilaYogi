using PreadmissionDTOs.com.vaps.Alumni;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlumniHub.Com.Interface
{
    public interface Alumni_Student_FriendRequestInterface
    {
        //Alumnim Request send
         Alumni_FriendRequestDTO getdata(Alumni_FriendRequestDTO dto);
         Alumni_FriendRequestDTO getsearch_data(Alumni_FriendRequestDTO dto);
         Alumni_FriendRequestDTO sendrequest(Alumni_FriendRequestDTO dto);
         Alumni_FriendRequestDTO viewprofile(Alumni_FriendRequestDTO dto);


        //Alumnim Request accept
        Alumni_FriendRequestDTO getdata_request(Alumni_FriendRequestDTO dto);
        Alumni_FriendRequestDTO FriendrequestAccept(Alumni_FriendRequestDTO dto);
    }
}
