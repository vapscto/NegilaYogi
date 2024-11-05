using CommonLibrary;
using DomainModel.Model.com.vapstech.Fee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Fees
{
    public class MasterActivityGroupHeadDelegate
    {
        CommonDelegate<Adm_Master_ActivitiesDTO, Adm_Master_ActivitiesDTO> comm = new CommonDelegate<Adm_Master_ActivitiesDTO, Adm_Master_ActivitiesDTO>();

        public Adm_Master_ActivitiesDTO loaddata(Adm_Master_ActivitiesDTO data)
        {
            return comm.POSTDatafee(data, "MasterActivityGroupHeadFacade/loaddata");
        }
        public Adm_Master_ActivitiesDTO gethead(Adm_Master_ActivitiesDTO data)
        {
            return comm.POSTDatafee(data, "MasterActivityGroupHeadFacade/gethead");

        }
        public Adm_Master_ActivitiesDTO savedata(Adm_Master_ActivitiesDTO data)
        {
            return comm.POSTDatafee(data, "MasterActivityGroupHeadFacade/savedata");
        }
        public Adm_Master_ActivitiesDTO masterDecative(Adm_Master_ActivitiesDTO data)
        {
            return comm.POSTDatafee(data, "MasterActivityGroupHeadFacade/masterDecative");
        }
        public Adm_Master_ActivitiesDTO deletedata(Adm_Master_ActivitiesDTO data)
        {
            return comm.POSTDatafee(data, "MasterActivityGroupHeadFacade/deletedata");
        }

    }
}
