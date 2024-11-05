using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
   public interface Adm_School_Master_CEInterface
    {
        Adm_School_Master_CE_DTO getdata(Adm_School_Master_CE_DTO data);
        Adm_School_Master_CE_DTO savedata(Adm_School_Master_CE_DTO data);
        Adm_School_Master_CE_DTO editdata(Adm_School_Master_CE_DTO data);
        Adm_School_Master_CE_DTO activedeactive(Adm_School_Master_CE_DTO data);
        Adm_School_Master_CE_DTO savedata2(Adm_School_Master_CE_DTO data);
       
        Adm_School_Master_CE_DTO deactive2(Adm_School_Master_CE_DTO data);
        Adm_School_Master_CE_DTO edit2(Adm_School_Master_CE_DTO data);

    }
}
