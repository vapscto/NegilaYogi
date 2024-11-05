using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{
    public interface TT_Master_Subject_AbbreviationInterface
    {
        TT_Master_Subject_AbbreviationDTO savedetail(TT_Master_Subject_AbbreviationDTO objcategory);
        TT_Master_Subject_AbbreviationDTO getdetails(int id);
        TT_Master_Subject_AbbreviationDTO getpageedit(int id);
        TT_Master_Subject_AbbreviationDTO deleterec(int id);

        TT_Master_Subject_AbbreviationDTO deactivate(TT_Master_Subject_AbbreviationDTO id);
    }
}
