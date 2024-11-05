using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public   interface VikasaAdmissionReportInterface
    {
        VikasaAdmissionreportDTO getdetails(VikasaAdmissionreportDTO stu);
        VikasaAdmissionreportDTO getStudDatabyclass(VikasaAdmissionreportDTO data);        
        Task<VikasaAdmissionreportDTO> getStudDetails(VikasaAdmissionreportDTO studData);
        VikasaAdmissionreportDTO onacademicyearchange(VikasaAdmissionreportDTO data);
        VikasaAdmissionreportDTO searchfilter(VikasaAdmissionreportDTO data);
        VikasaAdmissionreportDTO ShowReport(VikasaAdmissionreportDTO data);
        VikasaAdmissionreportDTO ShowReport1(VikasaAdmissionreportDTO data);
        
    }
}
