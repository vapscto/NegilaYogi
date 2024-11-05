using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Portals.Student;

namespace PortalHub.com.vaps.Student.Interfaces
{
    public interface ParentsSmartCardInterface
    {
        Task<ParentSmartCardDTO> getloaddata(ParentSmartCardDTO data);
        ParentSmartCardDTO savedata(ParentSmartCardDTO data);
        ParentSmartCardDTO savedataadmin(ParentSmartCardDTO data);
        ParentSmartCardDTO guardianDetails(ParentSmartCardDTO data);

        Task<ParentSmartCardDTO>  getstudata(ParentSmartCardDTO sddto);

        Task<ParentSmartCardDTO> getStateByCountry(int id);

        ParentSmartCardDTO searchfilter(ParentSmartCardDTO data);
        ParentSmartCardDTO getreport(ParentSmartCardDTO data);
    }
}
