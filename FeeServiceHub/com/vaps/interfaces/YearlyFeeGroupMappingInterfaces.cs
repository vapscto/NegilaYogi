using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface YearlyFeeGroupMappingInterfaces
    {
        FeeYearlygroupHeadMappingDTO getdata(FeeYearlygroupHeadMappingDTO data);
        FeeYearlygroupHeadMappingDTO savedetails(FeeYearlygroupHeadMappingDTO pgmod);
        FeeYearlygroupHeadMappingDTO deleterec(int id);
        FeeYearlygroupHeadMappingDTO getsearchdata(int id, FeeYearlygroupHeadMappingDTO org);
        FeeYearlygroupHeadMappingDTO EditMasterscetionDetails(FeeYearlygroupHeadMappingDTO data);

        FeeYearlygroupHeadMappingDTO getdataongroup(FeeYearlygroupHeadMappingDTO data);

        FeeYearlygroupHeadMappingDTO selectacade(FeeYearlygroupHeadMappingDTO data);
        
    }
}