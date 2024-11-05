using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model.com.vapstech.Fee;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface FeeActivityRequestInterface
    {
        Adm_Master_ActivitiesDTO getdata(Adm_Master_ActivitiesDTO data);
        Adm_Master_ActivitiesDTO savedata(Adm_Master_ActivitiesDTO data);
        Adm_Master_ActivitiesDTO deletedata(Adm_Master_ActivitiesDTO data);
        Adm_Master_ActivitiesDTO loadapproval(Adm_Master_ActivitiesDTO data);
        Adm_Master_ActivitiesDTO viewacaclslst(Adm_Master_ActivitiesDTO data);
        Adm_Master_ActivitiesDTO viewstudentlist(Adm_Master_ActivitiesDTO data);
        Adm_Master_ActivitiesDTO saveGroupdata(Adm_Master_ActivitiesDTO data);
        Adm_Master_ActivitiesDTO searching(Adm_Master_ActivitiesDTO data);
        
    }
}
