using PreadmissionDTOs.com.vaps.College.Preadmission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegePreadmission.Interfaces
{
    public interface DocumentViewClgInterface
    {
        CollegePreadmissionstudnetDto getInitailData(CollegePreadmissionstudnetDto id);
        CollegePreadmissionstudnetDto getclgstudata(CollegePreadmissionstudnetDto id);
        CollegePreadmissionstudnetDto getdocksonly(CollegePreadmissionstudnetDto id);

        CollegePreadmissionstudnetDto getbranch(CollegePreadmissionstudnetDto id);
        CollegePreadmissionstudnetDto getsemester(CollegePreadmissionstudnetDto id);
    }
}
