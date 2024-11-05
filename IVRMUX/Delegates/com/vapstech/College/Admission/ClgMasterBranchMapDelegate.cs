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
    public class ClgMasterBranchMapDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ClgMasterCourseBranchMapDTO, ClgMasterCourseBranchMapDTO> COMMM = new CommonDelegate<ClgMasterCourseBranchMapDTO, ClgMasterCourseBranchMapDTO>();

       
        public ClgMasterCourseBranchMapDTO Savedetails(ClgMasterCourseBranchMapDTO dt)
        {
            return COMMM.clgadmissionbypost(dt, "ClgCourseBranchMapFacade/Savedetails");
        }
        public ClgMasterCourseBranchMapDTO getalldetails(ClgMasterCourseBranchMapDTO id)
        {
            return COMMM.clgadmissionbypost(id, "ClgCourseBranchMapFacade/getalldetails/");
        }
        public ClgMasterCourseBranchMapDTO Deletedetails(ClgMasterCourseBranchMapDTO id)
        {
            return COMMM.clgadmissionbypost(id, "ClgCourseBranchMapFacade/Deletedetails");
        }

        public ClgMasterCourseBranchMapDTO showmodaldetails(ClgMasterCourseBranchMapDTO id)
        {
            return COMMM.clgadmissionbypost(id, "ClgCourseBranchMapFacade/showmodaldetails");
        }
        public ClgMasterCourseBranchMapDTO deactivesem(ClgMasterCourseBranchMapDTO id)
        {
            return COMMM.clgadmissionbypost(id, "ClgCourseBranchMapFacade/deactivesem");
        }
        public ClgMasterCourseBranchMapDTO edit(ClgMasterCourseBranchMapDTO id)
        {
            return COMMM.clgadmissionbypost(id, "ClgCourseBranchMapFacade/edit");
        }
        

    }
}
