using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.College.Admission;
using PreadmissionDTOs.com.vaps.College.Preadmission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegePreadmission.Interfaces
{
    public interface CLGStatusInterface  
    {
        CollegePreadmissionstudnetDto Getdetails(CollegePreadmissionstudnetDto dto);
        CollegePreadmissionstudnetDto getCourse(CollegePreadmissionstudnetDto dto);
        CollegePreadmissionstudnetDto getBranch(CollegePreadmissionstudnetDto data);
        CollegePreadmissionstudnetDto SearchData(CollegePreadmissionstudnetDto data);

        //master competitive exam
        Master_Competitive_ExamsClgDTO getexamdetails(Master_Competitive_ExamsClgDTO data);
        Master_Competitive_ExamsClgDTO saveExamDetails(Master_Competitive_ExamsClgDTO data);
        Master_Competitive_ExamsClgDTO saveExamMapDetails(Master_Competitive_ExamsClgDTO data);
        Master_Competitive_ExamsClgDTO getexamedit(int id);

        Master_Competitive_ExamsClgDTO deleterecord(int id);

        Master_Competitive_ExamsClgDTO getsubedit(int id);

        Master_Competitive_ExamsClgDTO deleterecordsub(int id);
    }
}
