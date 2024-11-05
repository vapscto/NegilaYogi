
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

//PreadmissionDTOs.com.vaps.admission


namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface MasterScholasticActivityInterface
    {
        MasterScholasticActivityDTO savedata(MasterScholasticActivityDTO data);
        MasterScholasticActivityDTO deactivate(MasterScholasticActivityDTO data);
        MasterScholasticActivityDTO editdetails(int ID);
        MasterScholasticActivityDTO Getdetails(MasterScholasticActivityDTO data);
        MasterScholasticActivityDTO validateordernumber(MasterScholasticActivityDTO data);
    }
}
