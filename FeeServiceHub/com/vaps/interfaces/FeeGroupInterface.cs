using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface FeeGroupInterface
    {
       
        FeeGroupDTO SaveGroupData(FeeGroupDTO org);
        FeeGroupDTO EditgroupDetails(int id);
        FeeGroupDTO getdetails(FeeGroupDTO data);
        FeeGroupDTO GetGroupSearchData(FeeGroupDTO mas);
        FeeGroupDTO getpageedit(int id);
        FeeGroupDTO deleterec(int id);
        FeeGroupDTO deactivate(FeeGroupDTO id);

        //for yearly 

        Task<FeeGroupDTO> getIndependentDropDowns(FeeGroupDTO yrs);


      //  Task<FeeYearlyGroupDTO> getIndependentDrop(FeeYearlyGroupDTO yrs);
        FeeYearlyGroupDTO SaveYearlyGroupData(int id,FeeYearlyGroupDTO org);

        FeeYearlyGroupDTO getdetailsY(int id);
        FeeYearlyGroupDTO deactivateY(FeeYearlyGroupDTO id);
        FeeYearlyGroupDTO getpageeditY(int id);


        FeeYearlyGroupDTO deleterecY(int id);

        FeeYearlyGroupDTO selectacade(FeeYearlyGroupDTO data);
        //savedataFTally
        Fee_FeeGroup_CompanyMappingDTO savedataFTally(Fee_FeeGroup_CompanyMappingDTO data);
        //deletedataYYY
        Fee_FeeGroup_CompanyMappingDTO deletedataYYY(Fee_FeeGroup_CompanyMappingDTO data);
    }
}
