using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Admission
{
    public class BranchChangeDelegate
    {
        CommonDelegate<BranchChangeDTO, BranchChangeDTO> _commbranch = new CommonDelegate<BranchChangeDTO, BranchChangeDTO>();
        public BranchChangeDTO getdetails(BranchChangeDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "BranchChangeFacade/getdetails/");
        }
        public BranchChangeDTO Studentdetails(BranchChangeDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "BranchChangeFacade/Studentdetails/");
        }
        public BranchChangeDTO Savedetails(BranchChangeDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "BranchChangeFacade/Savedetails/");
        }
        public BranchChangeDTO deactive(BranchChangeDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "BranchChangeFacade/deactive/");
        }
    }
}
