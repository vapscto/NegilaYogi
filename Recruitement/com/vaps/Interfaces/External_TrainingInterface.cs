using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
   public interface External_TrainingInterface
    {
        External_TrainingDTO onloaddata(External_TrainingDTO data);
        External_TrainingDTO saverecord(External_TrainingDTO data);
        External_TrainingDTO deactiveY(External_TrainingDTO data);
        External_TrainingDTO Edit(External_TrainingDTO data);

    }
}
