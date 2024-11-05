
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

//PreadmissionDTOs.com.vaps.admission


namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface mastersubsubjectInterface
    {
        mastersubsubjectDTO deactivate(mastersubsubjectDTO data);

        mastersubsubjectDTO editdeatils(int ID);
        mastersubsubjectDTO savedetails(mastersubsubjectDTO data);
        mastersubsubjectDTO validateordernumber(mastersubsubjectDTO data);
        
        mastersubsubjectDTO Getdetails(mastersubsubjectDTO data);
    }
}
