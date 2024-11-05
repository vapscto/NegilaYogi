using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;
using PreadmissionDTOs.com.vaps.VMS.Training;

namespace IVRMUX.Delegates.com.vapstech.VMS.Training
{
    public class Master_External_TrainingTypeDelegate
    {
        CommonDelegate<Master_External_TrainingTypeDTO, Master_External_TrainingTypeDTO> _com = new CommonDelegate<Master_External_TrainingTypeDTO, Master_External_TrainingTypeDTO>();

        public Master_External_TrainingTypeDTO onloaddata(Master_External_TrainingTypeDTO data)
        {
            return _com.POSTVMS(data, "Master_External_TrainingTypeFacade/onloaddata");
        }
        public Master_External_TrainingTypeDTO saverecord(Master_External_TrainingTypeDTO data)
        {
            return _com.POSTVMS(data, "Master_External_TrainingTypeFacade/saverecord");
        }
        public Master_External_TrainingTypeDTO deactiveY(Master_External_TrainingTypeDTO data)
        {
            return _com.POSTVMS(data, "Master_External_TrainingTypeFacade/deactiveY");
        }
    }
}
