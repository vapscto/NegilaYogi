using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface ClgSeatDistributionInterface
    {
        ClgSeatDistributionDTO getalldetails(ClgSeatDistributionDTO data);
        ClgSeatDistributionDTO getCoursedata(ClgSeatDistributionDTO data);
        ClgSeatDistributionDTO getBranchdata(ClgSeatDistributionDTO data);
        ClgSeatDistributionDTO getSemesterdata(ClgSeatDistributionDTO data);
        ClgSeatDistributionDTO get_Category(ClgSeatDistributionDTO data);
        ClgSeatDistributionDTO savedata(ClgSeatDistributionDTO data);
        ClgSeatDistributionDTO get_Seattotal(ClgSeatDistributionDTO data);

        //master competitive exam
        Master_Competitive_AdmExamsClgDTO getexamdetails(Master_Competitive_AdmExamsClgDTO data);
        Master_Competitive_AdmExamsClgDTO saveExamDetails(Master_Competitive_AdmExamsClgDTO data);
        Master_Competitive_AdmExamsClgDTO saveExamMapDetails(Master_Competitive_AdmExamsClgDTO data);
        Master_Competitive_AdmExamsClgDTO getexamedit(int id);

        Master_Competitive_AdmExamsClgDTO deleterecord(int id);

        Master_Competitive_AdmExamsClgDTO getsubedit(int id);

        Master_Competitive_AdmExamsClgDTO deleterecordsub(int id);

    }

}
