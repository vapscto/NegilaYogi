using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Admission
{
    public class ClgSMSEmailCountDelegate
    {
        CommonDelegate<ClgSMSEmailCountDTO, ClgSMSEmailCountDTO> _commbranch = new CommonDelegate<ClgSMSEmailCountDTO, ClgSMSEmailCountDTO>();
        public ClgSMSEmailCountDTO getdata(ClgSMSEmailCountDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "ClgSMSEmailCountFacade/getdata/");
        }
        public ClgSMSEmailCountDTO getreport(ClgSMSEmailCountDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "ClgSMSEmailCountFacade/getreport/");
        }
        //
        public ClgSMSEmailCountDTO SearchByColumn(ClgSMSEmailCountDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "ClgSMSEmailCountFacade/SearchByColumn/");
        }
    }
}
