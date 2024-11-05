using PreadmissionDTOs.com.vaps.ClubManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClubManagement.Interfaces
{
   public interface CMS_Master_InstallmentInterface
    {
        CMS_Master_InstallmentsDTO loaddata(int dto);
        CMS_Master_InstallmentsDTO savedata(CMS_Master_InstallmentsDTO data);
        //deactive
        CMS_Master_InstallmentsDTO deactive(CMS_Master_InstallmentsDTO data);
    }
}
