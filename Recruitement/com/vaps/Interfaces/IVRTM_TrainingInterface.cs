using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
   public interface IVRTM_TrainingInterface
    {
        IVRTM_TrainingDTO onloaddata(IVRTM_TrainingDTO data);
        IVRTM_TrainingDTO saverecord(IVRTM_TrainingDTO data);
        IVRTM_TrainingDTO deactiveY(IVRTM_TrainingDTO data);
        IVRTM_TrainingDTO trainerfeedback(IVRTM_TrainingDTO data);
        IVRTM_TrainingDTO Edit(IVRTM_TrainingDTO data);
        IVRTM_TrainingDTO gettrainer(IVRTM_TrainingDTO data);
        IVRTM_TrainingDTO onloaddataRequest(IVRTM_TrainingDTO data);
        IVRTM_TrainingDTO saveData(IVRTM_TrainingDTO data);
        
        /////////////////////////////IVRM_Training_Assigning/////////////////////////////////////////////////////////////////////////////
        IVRTM_TrainingDTO assignonload(IVRTM_TrainingDTO data);
        IVRTM_TrainingDTO EditDetails(IVRTM_TrainingDTO data);
        IVRTM_TrainingDTO saveassign(IVRTM_TrainingDTO data);

        //====================report===============================////////////////
        IVRTM_TrainingDTO onloaddatareport(IVRTM_TrainingDTO data);
        IVRTM_TrainingDTO getreport(IVRTM_TrainingDTO data);

    }
}
