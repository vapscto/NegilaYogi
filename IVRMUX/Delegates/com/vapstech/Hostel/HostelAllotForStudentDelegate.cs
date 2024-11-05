using CommonLibrary;
using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Hostel
{
    public class HostelAllotForStudentDelegate
    {
        CommonDelegate<HostelAllotForStudent_DTO, HostelAllotForStudent_DTO> _commnbranch = new CommonDelegate<HostelAllotForStudent_DTO, HostelAllotForStudent_DTO>();

        public HostelAllotForStudent_DTO loaddata(HostelAllotForStudent_DTO data)
        {
            return _commnbranch.Post_Hostel(data, "HostelAllotForStudentFacade/loaddata/");
        }
        public HostelAllotForStudent_DTO savedata(HostelAllotForStudent_DTO data)
        {
            return _commnbranch.Post_Hostel(data, "HostelAllotForStudentFacade/savedata/");
        }
        public HostelAllotForStudent_DTO get_studInfo(HostelAllotForStudent_DTO data)
        {
            return _commnbranch.Post_Hostel(data, "HostelAllotForStudentFacade/get_studInfo/");
        }
        public HostelAllotForStudent_DTO get_roomdetails(HostelAllotForStudent_DTO data)
        {
            return _commnbranch.Post_Hostel(data, "HostelAllotForStudentFacade/get_roomdetails/");
        }
        public HostelAllotForStudent_DTO editdata(HostelAllotForStudent_DTO data)
        {
            return _commnbranch.Post_Hostel(data, "HostelAllotForStudentFacade/editdata/");
        }

    }
}
