using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces
{
   public interface CategoryConcessionGroupMappingInterface
    {

        CategoryConcessionGroupMappingDTO loaddata(CategoryConcessionGroupMappingDTO data);
        CategoryConcessionGroupMappingDTO gethead(CategoryConcessionGroupMappingDTO data);
        CategoryConcessionGroupMappingDTO getconcession(CategoryConcessionGroupMappingDTO data);
        CategoryConcessionGroupMappingDTO save(CategoryConcessionGroupMappingDTO data);
        CategoryConcessionGroupMappingDTO deactiveStudent(CategoryConcessionGroupMappingDTO data);
        CategoryConcessionGroupMappingDTO EditData(CategoryConcessionGroupMappingDTO data);
        CategoryConcessionGroupMappingDTO getgroup(CategoryConcessionGroupMappingDTO data);
    }
}
