using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;
using PreadmissionDTOs.com.vaps.VMS.Training;

namespace IVRMUX.Delegates.com.vapstech.VMS.Training
{
    public class External_Training_Delegate
    {
        CommonDelegate<External_TrainingDTO, External_TrainingDTO> _com = new CommonDelegate<External_TrainingDTO, External_TrainingDTO>();

        public External_TrainingDTO onloaddata(External_TrainingDTO data)
        {
            return _com.POSTVMS(data, "External_TrainingFacade/onloaddata");
        }
        public External_TrainingDTO saverecord(External_TrainingDTO data)
        {
            return _com.POSTVMS(data, "External_TrainingFacade/saverecord");
        }
        public External_TrainingDTO deactiveY(External_TrainingDTO data)
        {
            return _com.POSTVMS(data, "External_TrainingFacade/deactiveY");
        }
        public External_TrainingDTO Edit(External_TrainingDTO data)
        {
            return _com.POSTVMS(data, "External_TrainingFacade/Edit");
        }
    }
}
