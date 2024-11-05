using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.NAAC.Admission;

namespace NaacServiceHub.Admission.Interface
{
    public interface MasterCycleYearMappingInterface
    {
        MasterCycleYearMappingDTO getalldetails(MasterCycleYearMappingDTO data);
        MasterCycleYearMappingDTO savedetails(MasterCycleYearMappingDTO data);
        MasterCycleYearMappingDTO activedeactivedetails(MasterCycleYearMappingDTO data);
        MasterCycleYearMappingDTO editdetails(MasterCycleYearMappingDTO data);
        MasterCycleYearMappingDTO getOrder(MasterCycleYearMappingDTO data);

        // Master Cycle Year Mapping
        MasterCycleYearMappingDTO onchangecycle(MasterCycleYearMappingDTO data);
        MasterCycleYearMappingDTO savedetails1(MasterCycleYearMappingDTO data);
        MasterCycleYearMappingDTO viewdetails(MasterCycleYearMappingDTO data);
        MasterCycleYearMappingDTO deactivesem(MasterCycleYearMappingDTO data);
        MasterCycleYearMappingDTO delete(MasterCycleYearMappingDTO data);

    }
}
