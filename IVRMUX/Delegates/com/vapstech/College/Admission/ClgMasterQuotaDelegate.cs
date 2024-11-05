using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.College.Admission
{
    public class ClgMasterQuotaDelegate
    {
        CommonDelegate<ClgQuotaDTO, ClgQuotaDTO> _commbranch = new CommonDelegate<ClgQuotaDTO, ClgQuotaDTO>();

        public ClgQuotaDTO getalldetails(ClgQuotaDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgMasterQuotaFacade/getalldetails/");
        }

        //------------------------------------------Master Quota
        public ClgQuotaDTO savedetails(ClgQuotaDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgMasterQuotaFacade/savedetails/");
        }
        public ClgQuotaDTO activedeactiveQuota(ClgQuotaDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgMasterQuotaFacade/activedeactiveQuota/");
        }       
        public ClgQuotaDTO editdetails(ClgQuotaDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgMasterQuotaFacade/editdetails/");
        }
        //------------------------------------------Master Quota Category
        public ClgQuotaDTO savedetails1(ClgQuotaDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgMasterQuotaFacade/savedetails1/");
        }
        public ClgQuotaDTO activedeactiveQuota1(ClgQuotaDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgMasterQuotaFacade/activedeactiveQuota1/");
        }
        public ClgQuotaDTO editdetails1(ClgQuotaDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgMasterQuotaFacade/editdetails1/");
        }
        //------------------------------------------Master Quota Category Mapping
        public ClgQuotaDTO savedetails2(ClgQuotaDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgMasterQuotaFacade/savedetails2/");
        }
        public ClgQuotaDTO activedeactiveQuota2(ClgQuotaDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "ClgMasterQuotaFacade/activedeactiveQuota2/");
        }

    }
}
