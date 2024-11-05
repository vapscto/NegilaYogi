using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.University.Interface
{
   public interface NAAC_HSU_345_TeacherResearchPapersInterface
    {

        NAAC_HSU_345_TeacherResearchPapers_DTO loaddata(NAAC_HSU_345_TeacherResearchPapers_DTO data);
        NAAC_HSU_345_TeacherResearchPapers_DTO save(NAAC_HSU_345_TeacherResearchPapers_DTO data);
        NAAC_HSU_345_TeacherResearchPapers_DTO deactive(NAAC_HSU_345_TeacherResearchPapers_DTO data);
        NAAC_HSU_345_TeacherResearchPapers_DTO EditData(NAAC_HSU_345_TeacherResearchPapers_DTO data);
        NAAC_HSU_345_TeacherResearchPapers_DTO viewuploadflies(NAAC_HSU_345_TeacherResearchPapers_DTO data);
        NAAC_HSU_345_TeacherResearchPapers_DTO deleteuploadfile(NAAC_HSU_345_TeacherResearchPapers_DTO obj);
    }
}
