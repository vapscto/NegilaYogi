
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

//PreadmissionDTOs.com.vaps.admission


namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface CoScholasticActivityAreasInterface
    {
        CoScholasticActivityAreasDTO savedetails(CoScholasticActivityAreasDTO data);
        CoScholasticActivityAreasDTO validateordernumber(CoScholasticActivityAreasDTO data);
        CoScholasticActivityAreasDTO deactivate(CoScholasticActivityAreasDTO data);

        CoScholasticActivityAreasDTO editdetails(int ID);

        CoScholasticActivityAreasDTO Getdetails(CoScholasticActivityAreasDTO data);
    }
}
