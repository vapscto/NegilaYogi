using CommonLibrary;
using PreadmissionDTOs.com.vaps.Alumni;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Alumni
{
    public class Alumni_Student_FriendRequestDelegate
    {
        CommonDelegate<Alumni_FriendRequestDTO, Alumni_FriendRequestDTO> COMMM = new CommonDelegate<Alumni_FriendRequestDTO, Alumni_FriendRequestDTO>();

        //Alumnim Request send
        public Alumni_FriendRequestDTO getdata (Alumni_FriendRequestDTO dto)
        {
            return COMMM.POSTDataAlumni(dto, "Alumni_Student_FriendRequestFacade/getdata/");
        }
        public Alumni_FriendRequestDTO getsearch_data(Alumni_FriendRequestDTO dto)
        {
            return COMMM.POSTDataAlumni(dto, "Alumni_Student_FriendRequestFacade/getsearch_data/");
        }
        public Alumni_FriendRequestDTO sendrequest(Alumni_FriendRequestDTO dto)
        {
            return COMMM.POSTDataAlumni(dto, "Alumni_Student_FriendRequestFacade/sendrequest/");
        }
        public Alumni_FriendRequestDTO viewprofile(Alumni_FriendRequestDTO dto)
        {
            return COMMM.POSTDataAlumni(dto, "Alumni_Student_FriendRequestFacade/viewprofile/");
        }


        //Alumnim Request accept
        public Alumni_FriendRequestDTO getdata_request(Alumni_FriendRequestDTO dto)
        {
            return COMMM.POSTDataAlumni(dto, "Alumni_Student_FriendRequestFacade/getdata_request/");
        }
        public Alumni_FriendRequestDTO FriendrequestAccept(Alumni_FriendRequestDTO dto)
        {
            return COMMM.POSTDataAlumni(dto, "Alumni_Student_FriendRequestFacade/FriendrequestAccept/");
        }

    }
}
