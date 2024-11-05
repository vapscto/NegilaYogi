using PreadmissionDTOs.com.vaps.ClubManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClubManagement.Interfaces
{
   public interface CMS_Master_InstallmentTypeINTERFACE
    {
        CMS_Master_InstallmentTypeDTO loaddata(int dto);
        CMS_Master_InstallmentTypeDTO savedata(CMS_Master_InstallmentTypeDTO data);
        //deactive
        CMS_Master_InstallmentTypeDTO deactive(CMS_Master_InstallmentTypeDTO data);
    }
}
