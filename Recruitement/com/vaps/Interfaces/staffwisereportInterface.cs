using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
    public interface staffwisereportInterface
    {
        staffwisereportDTO onloaddata(staffwisereportDTO data);
        staffwisereportDTO getreport(staffwisereportDTO data);

    }
}
