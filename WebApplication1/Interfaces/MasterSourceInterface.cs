using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;

namespace WebApplication1.Interfaces
{
    public interface MasterSourceInterface
    {
        MasterSourceDTO saveorgdet(MasterSourceDTO org);
        MasterSourceDTO deleterec(int id);

        MasterSourceDTO getdetails(int id);
        MasterSourceDTO getpageedit(int id);

        MasterSourceDTO getsearchdata(int id, MasterSourceDTO org);
    }
}
