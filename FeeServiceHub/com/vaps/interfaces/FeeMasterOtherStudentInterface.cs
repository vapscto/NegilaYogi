using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces
{
   public interface FeeMasterOtherStudentInterface
    {
        FeeMasterOtherStudentDTO getdetails(int id);
        FeeMasterOtherStudentDTO save(FeeMasterOtherStudentDTO data);
        FeeMasterOtherStudentDTO edit(int id);
        FeeMasterOtherStudentDTO delete(int id);

    }
}
