using CommonLibrary;
using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Fees
{
    public class StudentRequestDelegate
    {
        CommonDelegate<StudentRequestDTO, StudentRequestDTO> comm = new CommonDelegate<StudentRequestDTO, StudentRequestDTO>();
        public StudentRequestDTO save(StudentRequestDTO data)
        {
            return comm.Post_Hostel(data, "studentrequestfacade1/save/");
        }
        public StudentRequestDTO loaddata(StudentRequestDTO data)
        {
            return comm.Post_Hostel(data, "studentrequestfacade1/loaddata/");
        }
        public StudentRequestDTO edittab1(StudentRequestDTO data)
        {
            return comm.Post_Hostel(data, "studentrequestfacade1/edittab1");
        }
        public StudentRequestDTO deactive(StudentRequestDTO data)
        {
            return comm.Post_Hostel(data, "studentrequestfacade1/deactive");
        }
    }
}
