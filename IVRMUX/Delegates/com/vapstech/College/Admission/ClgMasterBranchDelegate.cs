using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.College.Admission
{
    public class ClgMasterBranchDelegate
    {
        CommonDelegate<ClgMasterBranchDTO, ClgMasterBranchDTO> _commbranch = new CommonDelegate<ClgMasterBranchDTO, ClgMasterBranchDTO>();

        public ClgMasterBranchDTO getalldetails(ClgMasterBranchDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgMasterBranchFacade/getalldetails/");
        }
        public ClgMasterBranchDTO savebranch(ClgMasterBranchDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgMasterBranchFacade/savebranch/");
        }
        public ClgMasterBranchDTO editbranch(ClgMasterBranchDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgMasterBranchFacade/editbranch/");
        }
        public ClgMasterBranchDTO activedeactivebranch(ClgMasterBranchDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgMasterBranchFacade/activedeactivebranch/");
        }
        public ClgMasterBranchDTO saveorder(ClgMasterBranchDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgMasterBranchFacade/saveorder/");
        }
        
    }
}
