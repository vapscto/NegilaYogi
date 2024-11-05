using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Medical.Interface
{
  public  interface MC_819_Accredition_ClinicallabInterface
    {
        MC_819_Accredition_ClinicallabDTO loaddata(MC_819_Accredition_ClinicallabDTO data);
        MC_819_Accredition_ClinicallabDTO savedata(MC_819_Accredition_ClinicallabDTO data);
        MC_819_Accredition_ClinicallabDTO savedata1(MC_819_Accredition_ClinicallabDTO data);
        MC_819_Accredition_ClinicallabDTO savedata2(MC_819_Accredition_ClinicallabDTO data);
        MC_819_Accredition_ClinicallabDTO savedata3(MC_819_Accredition_ClinicallabDTO data);
        MC_819_Accredition_ClinicallabDTO editdata(MC_819_Accredition_ClinicallabDTO data);
        MC_819_Accredition_ClinicallabDTO deactivate(MC_819_Accredition_ClinicallabDTO data);
        MC_819_Accredition_ClinicallabDTO getcomment(MC_819_Accredition_ClinicallabDTO data);
        MC_819_Accredition_ClinicallabDTO savecomments(MC_819_Accredition_ClinicallabDTO data);
    }
}
