﻿using CommonLibrary;
using PreadmissionDTOs.com.vaps.VMS.HRMS;
using System;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class AddJobVMSDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HR_MRFRequisitionDTO, HR_MRFRequisitionDTO> COMMM = new CommonDelegate<HR_MRFRequisitionDTO, HR_MRFRequisitionDTO>();

        public HR_MRFRequisitionDTO onloadgetdetails(HR_MRFRequisitionDTO dto)
        {
            return COMMM.POSTVMS(dto, "AddJobVMSFacade/onloadgetdetails");
        }

        public HR_MRFRequisitionDTO savedetails(HR_MRFRequisitionDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "AddJobVMSFacade/");
        }
        public HR_MRFRequisitionDTO getRecorddetailsById(int id)
        {
            return COMMM.GetVMS(id, "AddJobVMSFacade/getRecordById/");
        }
        public HR_MRFRequisitionDTO deleterec(HR_MRFRequisitionDTO maspage)
        {
            return COMMM.POSTVMS(maspage, "AddJobVMSFacade/deactivateRecordById/");
        }

       

       

    }
}
