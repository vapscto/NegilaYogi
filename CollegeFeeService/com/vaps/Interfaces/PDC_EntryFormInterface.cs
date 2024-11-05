using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Interfaces
{
    public interface PDC_EntryFormInterface
    {
        PDC_EntryFormDTO SaveGroupData(PDC_EntryFormDTO org);
        PDC_EntryFormDTO EditgroupDetails(int id);
        PDC_EntryFormDTO getdetails(PDC_EntryFormDTO data);
        PDC_EntryFormDTO GetGroupSearchData(PDC_EntryFormDTO mas);
        PDC_EntryFormDTO getpageedit(int id);

        PDC_EntryFormDTO deactivate(PDC_EntryFormDTO id);
        PDC_EntryFormDTO showdata(PDC_EntryFormDTO data);

        PDC_EntryFormDTO PDCRemainder(PDC_EntryFormDTO data);
        PDC_EntryFormDTO getbranchdetails(PDC_EntryFormDTO data);
        PDC_EntryFormDTO getsemesterdetails(PDC_EntryFormDTO data);
        PDC_EntryFormDTO selectstudent(PDC_EntryFormDTO data);
        College_Student_SettlementDTO Settlement_data(College_Student_SettlementDTO data);

    }
}
