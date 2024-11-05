using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Interfaces
{
    public interface PDCReportInterface
    {
        PDC_EntryFormDTO getdetails(PDC_EntryFormDTO dt);


        PDC_EntryFormDTO get_courses(PDC_EntryFormDTO data);
        PDC_EntryFormDTO get_branches(PDC_EntryFormDTO data);
        PDC_EntryFormDTO get_semisters(PDC_EntryFormDTO data);
        PDC_EntryFormDTO get_semisters_new(PDC_EntryFormDTO data);
        PDC_EntryFormDTO getgroupmappedheads(PDC_EntryFormDTO feedto);

        PDC_EntryFormDTO getgroupheadsid(PDC_EntryFormDTO feedtohead);

        Task<PDC_EntryFormDTO> Getreportdetails(PDC_EntryFormDTO feedtoget);

        PDC_EntryFormDTO getdata(PDC_EntryFormDTO feedtohead);
    }
}
