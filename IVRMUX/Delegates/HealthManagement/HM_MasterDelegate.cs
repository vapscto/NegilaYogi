using CommonLibrary;
using PreadmissionDTOs.HealthManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.HealthManagement
{
    public class HM_MasterDelegate
    {
        CommonDelegate<Master_HealthManagementDTO, Master_HealthManagementDTO> COMM = new CommonDelegate<Master_HealthManagementDTO, Master_HealthManagementDTO>();

        // Master Behaviour
        public Master_HealthManagementDTO load_MB(Master_HealthManagementDTO dto)
        {
            return COMM.HealthManagementPOST(dto, "HM_MasterFacade/load_MB/");
        }
        public Master_HealthManagementDTO Save_MB(Master_HealthManagementDTO dto)
        {
            return COMM.HealthManagementPOST(dto, "HM_MasterFacade/Save_MB/");
        }
        public Master_HealthManagementDTO Edit_MB(Master_HealthManagementDTO dto)
        {
            return COMM.HealthManagementPOST(dto, "HM_MasterFacade/Edit_MB/");
        }
        public Master_HealthManagementDTO ActiveDeactive_MB(Master_HealthManagementDTO dto)
        {
            return COMM.HealthManagementPOST(dto, "HM_MasterFacade/ActiveDeactive_MB/");
        }

        // Master Cleanness
        public Master_HealthManagementDTO load_CL(Master_HealthManagementDTO dto)
        {
            return COMM.HealthManagementPOST(dto, "HM_MasterFacade/load_CL/");
        }
        public Master_HealthManagementDTO Save_CL(Master_HealthManagementDTO dto)
        {
            return COMM.HealthManagementPOST(dto, "HM_MasterFacade/Save_CL/");
        }
        public Master_HealthManagementDTO Edit_CL(Master_HealthManagementDTO dto)
        {
            return COMM.HealthManagementPOST(dto, "HM_MasterFacade/Edit_CL/");
        }
        public Master_HealthManagementDTO ActiveDeactive_CL(Master_HealthManagementDTO dto)
        {
            return COMM.HealthManagementPOST(dto, "HM_MasterFacade/ActiveDeactive_CL/");
        }

        // Master Doctor
        public Master_HealthManagementDTO load_DC(Master_HealthManagementDTO dto)
        {
            return COMM.HealthManagementPOST(dto, "HM_MasterFacade/load_DC/");
        }
        public Master_HealthManagementDTO Save_DC(Master_HealthManagementDTO dto)
        {
            return COMM.HealthManagementPOST(dto, "HM_MasterFacade/Save_DC/");
        }
        public Master_HealthManagementDTO Edit_DC(Master_HealthManagementDTO dto)
        {
            return COMM.HealthManagementPOST(dto, "HM_MasterFacade/Edit_DC/");
        }
        public Master_HealthManagementDTO ActiveDeactive_DC(Master_HealthManagementDTO dto)
        {
            return COMM.HealthManagementPOST(dto, "HM_MasterFacade/ActiveDeactive_DC/");
        }

        // Master Examination
        public Master_HealthManagementDTO load_EX(Master_HealthManagementDTO dto)
        {
            return COMM.HealthManagementPOST(dto, "HM_MasterFacade/load_EX/");
        }
        public Master_HealthManagementDTO Save_EX(Master_HealthManagementDTO dto)
        {
            return COMM.HealthManagementPOST(dto, "HM_MasterFacade/Save_EX/");
        }
        public Master_HealthManagementDTO Edit_EX(Master_HealthManagementDTO dto)
        {
            return COMM.HealthManagementPOST(dto, "HM_MasterFacade/Edit_EX/");
        }
        public Master_HealthManagementDTO ActiveDeactive_EX(Master_HealthManagementDTO dto)
        {
            return COMM.HealthManagementPOST(dto, "HM_MasterFacade/ActiveDeactive_EX/");
        }

        // Master Observation
        public Master_HealthManagementDTO load_OB(Master_HealthManagementDTO dto)
        {
            return COMM.HealthManagementPOST(dto, "HM_MasterFacade/load_OB/");
        }
        public Master_HealthManagementDTO Save_OB(Master_HealthManagementDTO dto)
        {
            return COMM.HealthManagementPOST(dto, "HM_MasterFacade/Save_OB/");
        }
        public Master_HealthManagementDTO Edit_OB(Master_HealthManagementDTO dto)
        {
            return COMM.HealthManagementPOST(dto, "HM_MasterFacade/Edit_OB/");
        }
        public Master_HealthManagementDTO ActiveDeactive_OB(Master_HealthManagementDTO dto)
        {
            return COMM.HealthManagementPOST(dto, "HM_MasterFacade/ActiveDeactive_OB/");
        }

        // Master Illness
        public Master_HealthManagementDTO Load_illness(Master_HealthManagementDTO dto)
        {
            return COMM.HealthManagementPOST(dto, "HM_MasterFacade/Load_illness/");
        }
        public Master_HealthManagementDTO Save_illness(Master_HealthManagementDTO dto)
        {
            return COMM.HealthManagementPOST(dto, "HM_MasterFacade/Save_illness/");
        }
        public Master_HealthManagementDTO Edit_illness(Master_HealthManagementDTO dto)
        {
            return COMM.HealthManagementPOST(dto, "HM_MasterFacade/Edit_illness/");
        }
        public Master_HealthManagementDTO ActiveDeactive_illness(Master_HealthManagementDTO dto)
        {
            return COMM.HealthManagementPOST(dto, "HM_MasterFacade/ActiveDeactive_illness/");
        }

    }
}
