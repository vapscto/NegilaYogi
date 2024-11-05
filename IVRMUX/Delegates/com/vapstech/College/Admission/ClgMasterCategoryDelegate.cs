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
    public class ClgMasterCategoryDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<ClgMasterCategoryDTO, ClgMasterCategoryDTO> COMMM = new CommonDelegate<ClgMasterCategoryDTO, ClgMasterCategoryDTO>();


        public ClgMasterCategoryDTO Savedetails(ClgMasterCategoryDTO dt)
        {
            return COMMM.clgadmissionbypost(dt, "ClgMasterCategoryFacade/Savedetails");
        }
        public ClgMasterCategoryDTO getalldetails(int id)
        {
            return COMMM.clgadmissionbyid(id, "ClgMasterCategoryFacade/getalldetails/");
        }
        public ClgMasterCategoryDTO Deletedetails(ClgMasterCategoryDTO id)
        {
            return COMMM.clgadmissionbypost(id, "ClgMasterCategoryFacade/Deletedetails");
        }
        public ClgMasterCategoryDTO deactivate(ClgMasterCategoryDTO id)
        {
            return COMMM.clgadmissionbypost(id, "ClgMasterCategoryFacade/deactivate");
        }
        
    }
}