using CommonLibrary;
using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Hostel
{
    public class HostelAllotForStaffDelegate
    {

        CommonDelegate<HostelAllotForStaff_DTO, HostelAllotForStaff_DTO> _commnbranch = new CommonDelegate<HostelAllotForStaff_DTO, HostelAllotForStaff_DTO>();

        public HostelAllotForStaff_DTO loaddata(HostelAllotForStaff_DTO data)
        {
            return _commnbranch.Post_Hostel(data, "HostelAllotForStaffFacade/loaddata/");
        }
        public HostelAllotForStaff_DTO savedata(HostelAllotForStaff_DTO data)
        {
            return _commnbranch.Post_Hostel(data, "HostelAllotForStaffFacade/savedata/");
        }
        public HostelAllotForStaff_DTO get_studInfo(HostelAllotForStaff_DTO data)
        {
            return _commnbranch.Post_Hostel(data, "HostelAllotForStaffFacade/get_studInfo/");
        }
        public HostelAllotForStaff_DTO get_roomdetails(HostelAllotForStaff_DTO data)
        {
            return _commnbranch.Post_Hostel(data, "HostelAllotForStaffFacade/get_roomdetails/");
        }
        public HostelAllotForStaff_DTO editdata(HostelAllotForStaff_DTO data)
        {
            return _commnbranch.Post_Hostel(data, "HostelAllotForStaffFacade/editdata/");
        } public HostelAllotForStaff_DTO deactivYTab1(HostelAllotForStaff_DTO data)
        {
            return _commnbranch.Post_Hostel(data, "HostelAllotForStaffFacade/deactivYTab1/");
        }
        public HostelAllotForStaff_DTO getdesg(HostelAllotForStaff_DTO data)
        {
            return _commnbranch.Post_Hostel(data, "HostelAllotForStaffFacade/getdesg/");
        }
    }
}
