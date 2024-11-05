using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.admission;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface AdditionalFieldInterface
    {
       AdditionalFieldDTO Save(AdditionalFieldDTO cdto);
      
        AdditionalFieldDTO getBasicData(int additional_field);
        void deactivate(int id);
        AdditionalFieldDTO editData(int edit_field);
        //AdditionalFieldDTO getBasicDatanew(AdditionalFieldDTO enqo);
    }
}
