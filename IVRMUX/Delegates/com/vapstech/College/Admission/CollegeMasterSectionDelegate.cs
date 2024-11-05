using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.College.Admission
{
    public class CollegeMasterSectionDelegate
    {
        CommonDelegate<CollegeMasterSectionDTO, CollegeMasterSectionDTO> _comm = new CommonDelegate<CollegeMasterSectionDTO, CollegeMasterSectionDTO>();

        public CollegeMasterSectionDTO getalldetails(int id)
        {
            return _comm.clgadmissionbyid(id, "CollegeMasterSectionFacade/getalldetails/");
        }
        public CollegeMasterSectionDTO Editdetails(CollegeMasterSectionDTO id)
        {
            return _comm.clgadmissionbypost(id, "CollegeMasterSectionFacade/Editdetails/");
        }
        public CollegeMasterSectionDTO saveMasterdata(CollegeMasterSectionDTO id)
        {
            return _comm.clgadmissionbypost(id, "CollegeMasterSectionFacade/saveMasterdata/");
        }
        public CollegeMasterSectionDTO saveorder(CollegeMasterSectionDTO id)
        {
            return _comm.clgadmissionbypost(id, "CollegeMasterSectionFacade/saveorder/");
        }
        public CollegeMasterSectionDTO Deletedetails(CollegeMasterSectionDTO id)
        {
            return _comm.clgadmissionbypost(id, "CollegeMasterSectionFacade/Deletedetails/");
        }
        

    }
}
