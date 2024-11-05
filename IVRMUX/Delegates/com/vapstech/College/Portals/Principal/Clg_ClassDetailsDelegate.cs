using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Portals.Chairman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Portals.Principal
{
    public class Clg_ClassDetailsDelegate
    {

        CommonDelegate<Clg_ClassDetails_DTO, Clg_ClassDetails_DTO> comm = new CommonDelegate<Clg_ClassDetails_DTO, Clg_ClassDetails_DTO>();

        public Clg_ClassDetails_DTO loaddata(Clg_ClassDetails_DTO data)
        {
            return comm.CLGPortalPOSTData(data, "Clg_ClassDetailsFacade/loaddata");
        }
        public Clg_ClassDetails_DTO getcourse(Clg_ClassDetails_DTO data)
        {
            return comm.CLGPortalPOSTData(data, "Clg_ClassDetailsFacade/getcourse");
        }
        public Clg_ClassDetails_DTO report(Clg_ClassDetails_DTO data)
        {
            return comm.CLGPortalPOSTData(data, "Clg_ClassDetailsFacade/report");
        }

    }
}
