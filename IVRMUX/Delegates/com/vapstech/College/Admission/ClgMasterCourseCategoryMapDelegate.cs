using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.College.Admission
{
    public class ClgMasterCourseCategoryMapDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ClgMasterCourseCategoryMapDTO, ClgMasterCourseCategoryMapDTO> COMMM = new CommonDelegate<ClgMasterCourseCategoryMapDTO, ClgMasterCourseCategoryMapDTO>();

       
        public ClgMasterCourseCategoryMapDTO Savedetails(ClgMasterCourseCategoryMapDTO dt)
        {
            return COMMM.clgadmissionbypost(dt, "ClgMasterCourseCategoryFacade/Savedetails");
        }
        public ClgMasterCourseCategoryMapDTO getalldetails(ClgMasterCourseCategoryMapDTO id)
        {
            return COMMM.clgadmissionbypost(id, "ClgMasterCourseCategoryFacade/getalldetails/");
        }
        public ClgMasterCourseCategoryMapDTO deactive(ClgMasterCourseCategoryMapDTO id)
        {
            return COMMM.clgadmissionbypost(id, "ClgMasterCourseCategoryFacade/deactive");
        }
       
    }
}
