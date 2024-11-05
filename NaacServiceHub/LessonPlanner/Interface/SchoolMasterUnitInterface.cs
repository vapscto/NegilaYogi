using PreadmissionDTOs.com.vaps.Exam.LessonPlanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.com.vaps.LessonPlanner.Interface
{
    public interface SchoolMasterUnitInterface
    {
        SchoolMasterUnitDTO Getdetails(SchoolMasterUnitDTO data);
        SchoolMasterUnitDTO savedetails(SchoolMasterUnitDTO data);
        SchoolMasterUnitDTO editdeatils(SchoolMasterUnitDTO data);
        SchoolMasterUnitDTO deactivate(SchoolMasterUnitDTO data);
        SchoolMasterUnitDTO validateordernumber(SchoolMasterUnitDTO data);
        // Master Unit Topic Mapping
        SchoolMasterUnitDTO Getdetailsmapping(SchoolMasterUnitDTO data);
        SchoolMasterUnitDTO gettopicnames(SchoolMasterUnitDTO data);
        SchoolMasterUnitDTO savemappingdetails(SchoolMasterUnitDTO data);
        SchoolMasterUnitDTO deactivatemapping(SchoolMasterUnitDTO data);
        
    }
}
