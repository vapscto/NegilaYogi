using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.VMS.HRMS
{
    public class masterJobsDelegates
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_Master_JobsDTO, HR_Master_JobsDTO> COMMM = new CommonDelegate<HR_Master_JobsDTO, HR_Master_JobsDTO>();

        public HR_Master_JobsDTO onloadgetdetails(HR_Master_JobsDTO dto)
        {
            return COMMM.POSTVMS(dto, "MasterJobFacade/onloadgetdetails");
        }

        public HR_Master_JobsDTO savedetails(HR_Master_JobsDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "MasterJobFacade/");
        }
        public HR_Master_JobsDTO getRecorddetailsById(int id)
        {
            return COMMM.GetVMS(id, "MasterJobFacade/getRecordById/");
        }
        public HR_Master_JobsDTO deleterec(HR_Master_JobsDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "MasterJobFacade/deactivateRecordById/");
        }


    }
}
