using CommonLibrary;
using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Hostel
{
    public class CLGStudentRequestDelegatecs
    {
        CommonDelegate<CLGStudentRequest_DTO, CLGStudentRequest_DTO> comm = new CommonDelegate<CLGStudentRequest_DTO, CLGStudentRequest_DTO>();
        public CLGStudentRequest_DTO save(CLGStudentRequest_DTO data)
        {
            return comm.Post_Hostel(data, "CLGStudentRequestFacade/save/");
        }
        public CLGStudentRequest_DTO loaddata(CLGStudentRequest_DTO data)
        {
            return comm.Post_Hostel(data, "CLGStudentRequestFacade/loaddata/");
        }
        public CLGStudentRequest_DTO edittab1(CLGStudentRequest_DTO data)
        {
            return comm.Post_Hostel(data, "CLGStudentRequestFacade/edittab1");
        }
        public CLGStudentRequest_DTO roomdetails(CLGStudentRequest_DTO data)
        {
            return comm.Post_Hostel(data, "CLGStudentRequestFacade/roomdetails");
        }
        public CLGStudentRequest_DTO Catgory(CLGStudentRequest_DTO data)
        {
            return comm.Post_Hostel(data, "CLGStudentRequestFacade/Catgory");
        }
        public CLGStudentRequest_DTO getPdetails(CLGStudentRequest_DTO data)
        {
            return comm.Post_Hostel(data, "CLGStudentRequestFacade/getPdetails");
        }
        public CLGStudentRequest_DTO deactive(CLGStudentRequest_DTO data)
        {
            return comm.Post_Hostel(data, "CLGStudentRequestFacade/deactive");
        }

    }
}
