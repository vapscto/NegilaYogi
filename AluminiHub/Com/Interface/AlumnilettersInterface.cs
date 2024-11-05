using PreadmissionDTOs.com.vaps.Alumni;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlumniHub.Com.Interface
{
   public interface AlumnilettersInterface
    {
        AlumnilettersDTO BindData(AlumnilettersDTO clswisedailyattDTO);
        AlumnilettersDTO ShowReport(AlumnilettersDTO data);
        AlumnilettersDTO letterReport(AlumnilettersDTO data);
    }
}
