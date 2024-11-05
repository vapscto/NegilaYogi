using PreadmissionDTOs.com.vaps.College.Fee;

namespace CollegeFeeService.com.vaps.Interfaces
{
    public interface CollegeYearlyStatusReportInterface
    {
        CollegeYearlyStatusReportDTO GetYearList(int id);

        CollegeYearlyStatusReportDTO savedata(CollegeYearlyStatusReportDTO data);

        CollegeYearlyStatusReportDTO get_group(CollegeYearlyStatusReportDTO data);
    }
}