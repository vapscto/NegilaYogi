using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;


namespace FeeServiceHub.com.vaps.interfaces
{
 public   interface FeeClassCategoryInterface
    {

         FeeClassCategoryDTO SaveGroupData(FeeClassCategoryDTO org);
        FeeClassCategoryDTO EditgroupDetails(int id);
        FeeClassCategoryDTO getdetails(FeeClassCategoryDTO org);
        FeeClassCategoryDTO GetGroupSearchData(FeeClassCategoryDTO mas);
        FeeClassCategoryDTO getpageedit(int id);
        FeeClassCategoryDTO deleterec(int id);
        FeeClassCategoryDTO deactivate(FeeClassCategoryDTO id);

        //for yearly 

        Task<FeeClassCategoryDTO> getIndependentDropDowns(FeeClassCategoryDTO yrs);


        ////  Task<FeeYearlyGroupDTO> getIndependentDrop(FeeYearlyGroupDTO yrs);
        FeeYearlyClassCategoryDTO SaveYearlyGroupData(int id, FeeYearlyClassCategoryDTO org);

        FeeYearlyClassCategoryDTO getdetailsY(int id);
        FeeYearlyClassCategoryDTO deactivateY(FeeYearlyClassCategoryDTO id);
        FeeYearlyClassCategoryDTO getpageeditY(int id);
        FeeYearlyClassCategoryDTO deleterecY(int id);
        FeeYearlyClassCategoryDTO loaddata(FeeYearlyClassCategoryDTO data);
    }
}
