using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
    public interface Master_External_TrainingTypeInterface
    {
        Master_External_TrainingTypeDTO onloaddata(Master_External_TrainingTypeDTO data);
        Master_External_TrainingTypeDTO saverecord(Master_External_TrainingTypeDTO data);
        Master_External_TrainingTypeDTO deactiveY(Master_External_TrainingTypeDTO data);
    }
}
