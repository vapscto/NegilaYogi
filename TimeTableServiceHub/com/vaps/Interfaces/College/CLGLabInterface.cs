using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.Interfaces
{
    public interface CLGLabInterface
    { 
        CLGLabDTO deactivate(CLGLabDTO data);
    
        CLGLabDTO getalldetails(CLGLabDTO data);
        CLGLabDTO editlab(CLGLabDTO data);
       
        CLGLabDTO savedetail(CLGLabDTO data);
        CLGLabDTO viewrecordspopup(CLGLabDTO data);
       
    }
}
