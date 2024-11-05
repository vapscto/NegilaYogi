using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.College.Admission
{
    public class ClgyearlycoursemappingDelegate
    {
        CommonDelegate<ClgyearlycoursemappingDTO, ClgyearlycoursemappingDTO> _commbranch = new CommonDelegate<ClgyearlycoursemappingDTO, ClgyearlycoursemappingDTO>();

        public ClgyearlycoursemappingDTO getalldetails(ClgyearlycoursemappingDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgyearlycoursemappingFacade/getalldetails");
        }
        public ClgyearlycoursemappingDTO getbranches(ClgyearlycoursemappingDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgyearlycoursemappingFacade/getbranches");
        }
        public ClgyearlycoursemappingDTO getsemisters(ClgyearlycoursemappingDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgyearlycoursemappingFacade/getsemisters");
        }
        public ClgyearlycoursemappingDTO savedata(ClgyearlycoursemappingDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgyearlycoursemappingFacade/savedata");
        }
        public ClgyearlycoursemappingDTO searchdata(ClgyearlycoursemappingDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgyearlycoursemappingFacade/searchdata");
        }
        public ClgyearlycoursemappingDTO viewrecordspopup(ClgyearlycoursemappingDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgyearlycoursemappingFacade/viewrecordspopup");
        }
        

    }
}
