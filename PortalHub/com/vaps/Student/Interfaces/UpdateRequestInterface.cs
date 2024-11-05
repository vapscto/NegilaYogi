using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Portals.Student;

namespace PortalHub.com.vaps.Student.Interfaces
{
    public interface UpdateRequestInterface
    {
        Task<UpdateRequestDTO> getloaddata(UpdateRequestDTO data);
        Task<UpdateRequestDTO> getreploaddata(UpdateRequestDTO data);
        Task<UpdateRequestDTO> getreport(UpdateRequestDTO data);
        UpdateRequestDTO saverequest(UpdateRequestDTO data);
        UpdateRequestDTO savedataadmin(UpdateRequestDTO data);
        UpdateRequestDTO savereject(UpdateRequestDTO data);
        UpdateRequestDTO guardianDetails(UpdateRequestDTO data);

        Task<UpdateRequestDTO>  getstudata(UpdateRequestDTO sddto);

        Task<UpdateRequestDTO> getStateByCountry(int id);

        UpdateRequestDTO searchfilter(UpdateRequestDTO data);
    }
}
