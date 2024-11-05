using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Recruitment.com.vaps.Interfaces
{
   public interface Master_External_TrainingCentersInterface
    {

        Master_External_TrainingCentersDTO onloaddata(Master_External_TrainingCentersDTO data);
        Master_External_TrainingCentersDTO saverecord(Master_External_TrainingCentersDTO data);
        Master_External_TrainingCentersDTO deactiveY(Master_External_TrainingCentersDTO data);
    }
}


