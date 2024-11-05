using PreadmissionDTOs.com.vaps.VMS.Training;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruitment.com.vaps.Interfaces
{
    public interface TrainingtypewisereportInterface
    {
        TrainingtypewisereportDTO onloaddata(TrainingtypewisereportDTO data);
        TrainingtypewisereportDTO getreport(TrainingtypewisereportDTO data);
    }
}
