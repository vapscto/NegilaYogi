using CommonLibrary;
using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Hostel
{
    public class CLGHostelVacantDelegate
    {
        CommonDelegate<CLGHostelVacantDTO, CLGHostelVacantDTO> comm = new CommonDelegate<CLGHostelVacantDTO, CLGHostelVacantDTO>();
        //public StudentVacantDTO save(StudentVacantDTO data)
        //{
        //    return comm.Post_Hostel(data, "studentrequestfacade1/save/");
        //}
        public CLGHostelVacantDTO loaddata(CLGHostelVacantDTO data)
        {
            return comm.Post_Hostel(data, "CLGHostelVacantFacade/binddata/");
        }
        public CLGHostelVacantDTO edittab1(CLGHostelVacantDTO data)
        {
            return comm.Post_Hostel(data, "CLGHostelVacantFacade/edittab1");
        }
        public CLGHostelVacantDTO getalldetailsOnselectiontype(CLGHostelVacantDTO data)
        {
            return comm.Post_Hostel(data, "CLGHostelVacantFacade/getalldetailsOnselectiontype");
        }
        public CLGHostelVacantDTO get_staffDetail(CLGHostelVacantDTO data)
        {
            return comm.Post_Hostel(data, "CLGHostelVacantFacade/get_staffDetail");
        }
        public CLGHostelVacantDTO get_studentDetail(CLGHostelVacantDTO data)
        {
            return comm.Post_Hostel(data, "CLGHostelVacantFacade/get_studentDetail");
        }
        public CLGHostelVacantDTO get_guestDetail(CLGHostelVacantDTO data)
        {
            return comm.Post_Hostel(data, "CLGHostelVacantFacade/get_guestDetail");
        }

    }
}

