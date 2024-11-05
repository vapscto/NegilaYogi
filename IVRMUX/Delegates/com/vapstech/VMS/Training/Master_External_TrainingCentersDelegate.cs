using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;
using PreadmissionDTOs.com.vaps.VMS.Training;

namespace IVRMUX.Delegates.com.vapstech.VMS.Training
{
    public class Master_External_TrainingCentersDelegate
    {
        CommonDelegate<Master_External_TrainingCentersDTO, Master_External_TrainingCentersDTO> _com = new CommonDelegate<Master_External_TrainingCentersDTO, Master_External_TrainingCentersDTO>();

        public Master_External_TrainingCentersDTO onloaddata(Master_External_TrainingCentersDTO data)
        {
            return _com.POSTVMS(data, "Master_External_TrainingCentersFacade/onloaddata");
        }
        public Master_External_TrainingCentersDTO saverecord(Master_External_TrainingCentersDTO data)
        {
            return _com.POSTVMS(data, "Master_External_TrainingCentersFacade/saverecord");
        }
        public Master_External_TrainingCentersDTO deactiveY(Master_External_TrainingCentersDTO data)
        {
            return _com.POSTVMS(data, "Master_External_TrainingCentersFacade/deactiveY");
        }
    }
}
