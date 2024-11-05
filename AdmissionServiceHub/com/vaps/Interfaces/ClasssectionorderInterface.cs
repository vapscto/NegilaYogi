using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface ClasssectionorderInterface
    {
        ClasssectionorderDTO getdetails(int id);
        ClasssectionorderDTO save(ClasssectionorderDTO data);


    }
}
