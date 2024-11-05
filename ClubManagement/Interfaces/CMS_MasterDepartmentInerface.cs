using PreadmissionDTOs.com.vaps.ClubManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClubManagement.Interfaces
{
   public interface CMS_MasterDepartmentInerface
    {
        CMS_MasterDepartmentDTO loaddata(int dto);
        CMS_MasterDepartmentDTO savedata(CMS_MasterDepartmentDTO data);
        //deactive
        CMS_MasterDepartmentDTO deactive(CMS_MasterDepartmentDTO data);
        //loaddataconfigure
        CMS_ConfigurationDTO loaddataconfigure(int data);
        //saveconfigure
        CMS_ConfigurationDTO saveconfigure(CMS_ConfigurationDTO data);
        //confdeactive
        CMS_ConfigurationDTO confdeactive(CMS_ConfigurationDTO data);
    }
}
