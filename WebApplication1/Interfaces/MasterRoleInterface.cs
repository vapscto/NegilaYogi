using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;

namespace WebApplication1.Interfaces
{
    public interface MasterRoleInterface
    {
        MasterRoleDTO saveorgdet(MasterRoleDTO org);
        MasterRoleDTO deleterec(int id);

        MasterRoleDTO getdetails(int id);
        MasterRoleDTO getpageedit(int id);

        MasterRoleDTO getsearchdata(int id, MasterRoleDTO org);
    }
}
