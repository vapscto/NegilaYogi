using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{
    public interface TimeTableGenerationInterface
    {
        TT_Final_GenerationDTO generate(TT_Final_GenerationDTO objcategory);
        TT_Final_GenerationDTO getdetails(TT_Final_GenerationDTO data);
        TT_Final_GenerationDTO get_catg(TT_Final_GenerationDTO objcategory);
        TT_Final_GenerationDTO get_count(TT_Final_GenerationDTO objcategory);
        TT_Final_GenerationDTO resetTT(TT_Final_GenerationDTO objcategory);
        TT_Final_GenerationDTO Get_temp_data(TT_Final_GenerationDTO objcategory);
        TT_Final_GenerationDTO getalldetailsviewrecords(TT_Final_GenerationDTO objcategory);
        TT_Final_GenerationDTO getreplacemntdetailsviewrecords(TT_Final_GenerationDTO objcategory);
        TT_Final_GenerationDTO saveTemptomain(TT_Final_GenerationDTO objcategory);
        TT_Final_GenerationDTO deactivate(TT_Final_GenerationDTO data);
    }
}
