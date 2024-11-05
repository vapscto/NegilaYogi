using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;

namespace WebApplication1.Interfaces
{
    public interface MasterPageModuleMappingInterface
    {
        MasterPageModuleMappingDTO getmoduledet(int id);
        MasterPageModuleMappingDTO saveorgdet(MasterPageModuleMappingDTO pgmod);

        MasterPageModuleMappingDTO deleterec(int id);

        MasterPageModuleMappingDTO getsaveddata(int id);
    }
}
