using PreadmissionDTOs.com.vaps.COE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoeServiceHub.com.vaps.Interfaces
{
    public interface COEMailSMSInterface
    {
        void getdata(int id);
        COEReportDTO getdatanew(COEReportDTO data);

    }
}
