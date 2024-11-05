using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;

namespace WebApplication1.Interfaces
{
   public interface MasterClassCategoryInterface
    {
        MasterClassCategoryDTO getDat(MasterClassCategoryDTO data);
        MasterClassCategoryDTO saveData(MasterClassCategoryDTO dto);
        MasterClassCategoryDTO Edit(MasterClassCategoryDTO dto);
        MasterClassCategoryDTO deleterec(int id);
        MasterClassCategoryDTO deactivate(MasterClassCategoryDTO clscat);
        MasterClassCategoryDTO searchByColumn(MasterClassCategoryDTO cls);
        MasterClassCategoryDTO viewrecordspopup(MasterClassCategoryDTO cls);
        MasterClassCategoryDTO deactivesection(MasterClassCategoryDTO cls);
    }
}
