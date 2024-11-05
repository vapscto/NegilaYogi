using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class JobListVMSDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_MRFRequisitionDTO, HR_MRFRequisitionDTO> COMMM = new CommonDelegate<HR_MRFRequisitionDTO, HR_MRFRequisitionDTO>();

        public HR_MRFRequisitionDTO onloadgetdetails(HR_MRFRequisitionDTO dto)
        {
            return COMMM.POSTVMS(dto, "JobListVMSFacade/onloadgetdetails");
        }

        public HR_MRFRequisitionDTO savedetails(HR_MRFRequisitionDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "AddJobVMSFacade/");
        }
        public HR_MRFRequisitionDTO getRecorddetailsById(HR_MRFRequisitionDTO dto)
        {
            return COMMM.POSTVMS(dto, "JobListVMSFacade/getRecordById/");
        }
        public HR_MRFRequisitionDTO deleterec(HR_MRFRequisitionDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "JobListVMSFacade/deactivateRecordById/");
        }
    }
}
