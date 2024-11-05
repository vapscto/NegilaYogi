using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.Interfaces
{
    public interface CLGCategoryMappingInterface
    {
        CLGCategoryMappingDTO savedetails(CLGCategoryMappingDTO data);
        CLGCategoryMappingDTO deleterec(CLGCategoryMappingDTO data);
        CLGCategoryMappingDTO getalldetails(CLGCategoryMappingDTO data);
        CLGCategoryMappingDTO getBranch(CLGCategoryMappingDTO data);
       
    }
}
