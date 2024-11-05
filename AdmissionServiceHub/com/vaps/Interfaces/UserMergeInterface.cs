using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface UserMergeInterface
    {
        UserMergeDTO getalldetails(UserMergeDTO data);
        UserMergeDTO onstudentnamechange(UserMergeDTO data);
        UserMergeDTO mergeuserdetails(UserMergeDTO data);
    }
}
