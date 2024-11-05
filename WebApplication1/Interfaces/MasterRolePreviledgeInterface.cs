using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;

namespace WebApplication1.Interfaces
{
    public interface MasterRolePreviledgeInterface
    {
        MasterRolePreviledgeDTO getmoduledet(int id);

        MasterRolePreviledgeDTO mobilegetmodulepages(MasterRolePreviledgeDTO id);

        MasterRolePreviledgeDTO mobilegetalldetails(int id);
        MasterRolePreviledgeDTO saveorgdet(MasterRolePreviledgeDTO pgmod);
        MasterRolePreviledgeDTO mobilesaveorgdet(MasterRolePreviledgeDTO pgmod);
        MasterRolePreviledgeDTO deleterec(int id);

        MasterRolePreviledgeDTO mobiledeletemodpages(MasterRolePreviledgeDTO dTO);

        MasterRolePreviledgeDTO getmodulepagedata(MasterRolePreviledgeDTO id);
        MasterRolePreviledgeDTO getsearchdata(int id, MasterRolePreviledgeDTO org);
    }
}
