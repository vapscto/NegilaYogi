using DomainModel.Model.com.vapstech.Fee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces
{
   public interface MasterActivityGroupHeadInterface
    {
        Adm_Master_ActivitiesDTO loaddata(Adm_Master_ActivitiesDTO data);
        Adm_Master_ActivitiesDTO gethead(Adm_Master_ActivitiesDTO data);
        Adm_Master_ActivitiesDTO savedata(Adm_Master_ActivitiesDTO data);
        Adm_Master_ActivitiesDTO masterDecative(Adm_Master_ActivitiesDTO data);
        Adm_Master_ActivitiesDTO deletedata(Adm_Master_ActivitiesDTO data);
    }

}
