using PreadmissionDTOs.com.vaps.Portals.Chirman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.HOD.Interfaces
{
    public interface HODStudentStrengthInterface
    {
        ADMClassSectionStrengthDTO Getdetails(ADMClassSectionStrengthDTO data);
        ADMClassSectionStrengthDTO getclass(ADMClassSectionStrengthDTO data);
        ADMClassSectionStrengthDTO Getsection(ADMClassSectionStrengthDTO data);
        ADMClassSectionStrengthDTO Getsectioncount(ADMClassSectionStrengthDTO data);
    }
}
