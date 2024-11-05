using PreadmissionDTOs.HealthManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthManagement.Interfaces
{
    public interface HM_MasterInterfaces
    {
        // Master Behaviour
        Master_HealthManagementDTO load_MB(Master_HealthManagementDTO dto);
        Master_HealthManagementDTO Save_MB(Master_HealthManagementDTO dto);
        Master_HealthManagementDTO Edit_MB(Master_HealthManagementDTO dto);
        Master_HealthManagementDTO ActiveDeactive_MB(Master_HealthManagementDTO dto);

        // Master Behaviour
        Master_HealthManagementDTO load_CL(Master_HealthManagementDTO dto);
        Master_HealthManagementDTO Save_CL(Master_HealthManagementDTO dto);
        Master_HealthManagementDTO Edit_CL(Master_HealthManagementDTO dto);
        Master_HealthManagementDTO ActiveDeactive_CL(Master_HealthManagementDTO dto);
        
        // Master Doctor
        Master_HealthManagementDTO load_DC(Master_HealthManagementDTO dto);
        Master_HealthManagementDTO Save_DC(Master_HealthManagementDTO dto);
        Master_HealthManagementDTO Edit_DC(Master_HealthManagementDTO dto);
        Master_HealthManagementDTO ActiveDeactive_DC(Master_HealthManagementDTO dto);

         // Master Examination
        Master_HealthManagementDTO load_EX(Master_HealthManagementDTO dto);
        Master_HealthManagementDTO Save_EX(Master_HealthManagementDTO dto);
        Master_HealthManagementDTO Edit_EX(Master_HealthManagementDTO dto);
        Master_HealthManagementDTO ActiveDeactive_EX(Master_HealthManagementDTO dto);

        // Master Observation
        Master_HealthManagementDTO load_OB(Master_HealthManagementDTO dto);
        Master_HealthManagementDTO Save_OB(Master_HealthManagementDTO dto);
        Master_HealthManagementDTO Edit_OB(Master_HealthManagementDTO dto);
        Master_HealthManagementDTO ActiveDeactive_OB(Master_HealthManagementDTO dto);

        // Master Observation
        Master_HealthManagementDTO Load_illness(Master_HealthManagementDTO dto);
        Master_HealthManagementDTO Save_illness(Master_HealthManagementDTO dto);
        Master_HealthManagementDTO Edit_illness(Master_HealthManagementDTO dto);
        Master_HealthManagementDTO ActiveDeactive_illness(Master_HealthManagementDTO dto);
    }
}
