using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface ExamTTTransSettingsInterface
    {
        ExamTTTransSettingsDTO deactivate(ExamTTTransSettingsDTO id);
        ExamTTTransSettingsDTO editgetdetails(ExamTTTransSettingsDTO data);
        ExamTTTransSettingsDTO getdetails(ExamTTTransSettingsDTO data);
        ExamTTTransSettingsDTO onselectAcdYear(ExamTTTransSettingsDTO data);
        ExamTTTransSettingsDTO onselectclass(ExamTTTransSettingsDTO data);
        ExamTTTransSettingsDTO onselectSection(ExamTTTransSettingsDTO data);
        ExamTTTransSettingsDTO onselectSubject(ExamTTTransSettingsDTO data);
        ExamTTTransSettingsDTO onselectSubSubject(ExamTTTransSettingsDTO data);
        ExamTTTransSettingsDTO savedetail(ExamTTTransSettingsDTO data);
        ExamTTTransSettingsDTO getalldetailsviewrecords(ExamTTTransSettingsDTO acdm);

    }
}
