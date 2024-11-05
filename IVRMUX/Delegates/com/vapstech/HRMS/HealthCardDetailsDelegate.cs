using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.HRMS
{
    public class HealthCardDetailsDelegate
    {
        CommonDelegate<HealthCardDetailsDTO, HealthCardDetailsDTO> _comm = new CommonDelegate<HealthCardDetailsDTO, HealthCardDetailsDTO>();
        CommonDelegate<HealthCardMasterDTO, HealthCardMasterDTO> _commm = new CommonDelegate<HealthCardMasterDTO, HealthCardMasterDTO>();

        public HealthCardDetailsDTO loaddata(HealthCardDetailsDTO data)
        {
            return _comm.POSTDataHRMS(data, "HealthCardDetailsFACADE/loaddata");
        }
        public HealthCardDetailsDTO SaveDetails(HealthCardDetailsDTO data)
        {
            return _comm.POSTDataHRMS(data, "HealthCardDetailsFACADE/SaveDetails");
        }
        //OnChangeEmployee
        public HealthCardDetailsDTO OnChangeEmployee(HealthCardDetailsDTO data)
        {
            return _comm.POSTDataHRMS(data, "HealthCardDetailsFACADE/OnChangeEmployee");
        }
        //Savemaster
        public HealthCardMasterDTO Savemaster(HealthCardMasterDTO data)
        {
            return _commm.POSTDataHRMS(data, "HealthCardDetailsFACADE/Savemaster");
        }
        //editmaster
        public HealthCardMasterDTO editmaster(HealthCardMasterDTO data)
        {
            return _commm.POSTDataHRMS(data, "HealthCardDetailsFACADE/editmaster");
        }

        //deactiveM
        public HealthCardMasterDTO deactiveM(HealthCardMasterDTO data)
        {
            return _commm.POSTDataHRMS(data, "HealthCardDetailsFACADE/deactiveM");
        }
    }
}
