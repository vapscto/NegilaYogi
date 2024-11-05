using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.College.Admission
{
    public class ClgMasterAcademicYearDelegate
    {
        CommonDelegate<ClgMasterAcademicYearDTO, ClgMasterAcademicYearDTO> _commyear = new CommonDelegate<ClgMasterAcademicYearDTO, ClgMasterAcademicYearDTO>();

        public ClgMasterAcademicYearDTO getalldetails(ClgMasterAcademicYearDTO data)
        {
            return _commyear.clgadmissionbypost(data, "ClgMasterAcademicYearFacade/getalldetails");
        }
        public ClgMasterAcademicYearDTO saveaccyear(ClgMasterAcademicYearDTO data)
        {
            return _commyear.clgadmissionbypost(data, "ClgMasterAcademicYearFacade/saveaccyear");
        }
        public ClgMasterAcademicYearDTO edit(ClgMasterAcademicYearDTO data)
        {
            return _commyear.clgadmissionbypost(data, "ClgMasterAcademicYearFacade/edit");
        }
        public ClgMasterAcademicYearDTO deactivate(ClgMasterAcademicYearDTO data)
        {
            return _commyear.clgadmissionbypost(data, "ClgMasterAcademicYearFacade/deactivate");
        }
        public ClgMasterAcademicYearDTO saveorder(ClgMasterAcademicYearDTO data)
        {
            return _commyear.clgadmissionbypost(data, "ClgMasterAcademicYearFacade/saveorder");
        }
        
    }
}
