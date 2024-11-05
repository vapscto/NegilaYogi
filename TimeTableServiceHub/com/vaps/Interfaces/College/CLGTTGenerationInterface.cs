using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{
    public interface CLGTTGenerationInterface
    {
        CLGTTGenerationDTO generate(CLGTTGenerationDTO objcategory);
        CLGTTGenerationDTO getdetails(CLGTTGenerationDTO data);
        CLGTTGenerationDTO get_catg(CLGTTGenerationDTO objcategory);
        CLGTTGenerationDTO get_count(CLGTTGenerationDTO objcategory);
        CLGTTGenerationDTO resetTT(CLGTTGenerationDTO objcategory);
        CLGTTGenerationDTO Get_temp_data(CLGTTGenerationDTO objcategory);
        CLGTTGenerationDTO getalldetailsviewrecords(CLGTTGenerationDTO objcategory);
        CLGTTGenerationDTO getreplacemntdetailsviewrecords(CLGTTGenerationDTO objcategory);
        CLGTTGenerationDTO saveTemptomain(CLGTTGenerationDTO objcategory);
        CLGTTGenerationDTO deactivate(CLGTTGenerationDTO data);
    }
}
