using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HostelServiceHub.Interface
{
   public  interface MasterMess_MessCategoryInterface
    {
        MasterMess_MessCategoryDTO get_Mmessdata(MasterMess_MessCategoryDTO data);
        HL_Master_Mess_DTO save_Mmessdata(HL_Master_Mess_DTO data);
        MasterMess_MessCategoryDTO edit_Mmessdata(MasterMess_MessCategoryDTO data);
        MasterMess_MessCategoryDTO deactiveY_Mmessdata(MasterMess_MessCategoryDTO data);

    }
}
