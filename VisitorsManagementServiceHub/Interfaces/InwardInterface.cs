using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisitorsManagementServiceHub.Interfaces
{
    public interface InwardInterface
    {
        InwardDTO getDetails(InwardDTO data);
        InwardDTO saveData(InwardDTO data);
        InwardDTO EditDetails(InwardDTO id);
        InwardDTO deactivate(InwardDTO data);
        InwardDTO searchfilter(InwardDTO data);
        InwardDTO get_empdetails(InwardDTO data);
        InwardDTO searchfilter2(InwardDTO data);
        InwardDTO get_empdetails2(InwardDTO data);


    }
}
