using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
  public  interface MasterCategoryInterface
    {
        MasterCategoryDTO saveCategorydet(MasterCategoryDTO cat);
        MasterCategoryDTO getAllDetails(int id);
        MasterCategoryDTO deleterec(int id);
        MasterCategoryDTO getdetails(MasterCategoryDTO id);
        MasterCategoryDTO deactivate(MasterCategoryDTO data);
    }
}
