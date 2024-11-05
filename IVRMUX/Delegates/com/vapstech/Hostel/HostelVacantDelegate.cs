using CommonLibrary;
using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Hostel
{
    public class StudentVacantDelegate
    {
        CommonDelegate<StudentVacantDTO, StudentVacantDTO> comm = new CommonDelegate<StudentVacantDTO, StudentVacantDTO>();
        //public StudentVacantDTO save(StudentVacantDTO data)
        //{
        //    return comm.Post_Hostel(data, "studentrequestfacade1/save/");
        //}
        public StudentVacantDTO loaddata(StudentVacantDTO data)
        {
            return comm.Post_Hostel(data, "HostelVacateFacade/binddata/");
        }
        public StudentVacantDTO edittab1(StudentVacantDTO data)
        {
            return comm.Post_Hostel(data, "HostelVacateFacade/edittab1");
        }
        public StudentVacantDTO getalldetailsOnselectiontype(StudentVacantDTO data)
        {
            return comm.Post_Hostel(data, "HostelVacateFacade/getalldetailsOnselectiontype");
        }
        public StudentVacantDTO get_staffDetail(StudentVacantDTO data)
        {
            return comm.Post_Hostel(data, "HostelVacateFacade/get_staffDetail");
        }
        public StudentVacantDTO get_studentDetail(StudentVacantDTO data)
        {
            return comm.Post_Hostel(data, "HostelVacateFacade/get_studentDetail");
        }
        public StudentVacantDTO get_guestDetail(StudentVacantDTO data)
        {
            return comm.Post_Hostel(data, "HostelVacateFacade/get_guestDetail");
        }

    }
}
