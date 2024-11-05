
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

//PreadmissionDTOs.com.vaps.admission


namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface exammasterPointInterface
    {
        exammasterpointDTO savedetails(exammasterpointDTO data);
        exammasterpointDTO validateordernumber(exammasterpointDTO data);
        exammasterpointDTO deactivate(exammasterpointDTO data);

        exammasterpointDTO editdetails(int ID);

        exammasterpointDTO Getdetails(exammasterpointDTO data);
    }
}
