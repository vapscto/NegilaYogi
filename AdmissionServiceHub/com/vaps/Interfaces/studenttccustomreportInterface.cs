using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
   public interface studenttccustomreportInterface
    {
        studenttccustomreportDTO getinitialdata(int id);
        studenttccustomreportDTO changeyear(studenttccustomreportDTO data);
        studenttccustomreportDTO changeclass(studenttccustomreportDTO data);
        studenttccustomreportDTO changesection(studenttccustomreportDTO data);
        studenttccustomreportDTO getTCdata(studenttccustomreportDTO data);
    }
}
