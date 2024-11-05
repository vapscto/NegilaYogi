
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

//PreadmissionDTOs.com.vaps.admission


namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface mastersubexamInterface
    {
        mastersubexamDTO savedetails(mastersubexamDTO data);
        mastersubexamDTO validateordernumber(mastersubexamDTO data);
        
        mastersubexamDTO deactivate(mastersubexamDTO data);

        mastersubexamDTO editdeatils(int ID);

        mastersubexamDTO Getdetails(mastersubexamDTO data);
    }
}
