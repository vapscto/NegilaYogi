using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Interfaces
{
    public interface Master_CovidVaccineTypeInterface
    {
        CovidVaccineDTO onloaddata(CovidVaccineDTO data);
        CovidVaccineDTO saverecord(CovidVaccineDTO data);
        CovidVaccineDTO deactiveY(CovidVaccineDTO data);
    }

}
