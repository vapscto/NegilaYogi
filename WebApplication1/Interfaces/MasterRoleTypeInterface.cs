using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;

namespace WebApplication1.Interfaces
{
    public interface MasterRoleTypeInterface
    {
        MasterRoleTypeDTO saveorgdet(MasterRoleTypeDTO org);
        MasterRoleTypeDTO deleterec(int id);

        MasterRoleTypeDTO getdetails(int id);
        MasterRoleTypeDTO getpageedit(int id);

        MasterRoleTypeDTO getsearchdata(int id, MasterRoleTypeDTO org);
    }
}
