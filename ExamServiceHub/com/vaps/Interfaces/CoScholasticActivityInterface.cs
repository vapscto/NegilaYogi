using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface CoScholasticActivityInterface
    {
        CoScholasticActivityDTO Getdetails(CoScholasticActivityDTO data);

        CoScholasticActivityDTO savedetail(CoScholasticActivityDTO data);
        CoScholasticActivityDTO deactivate(CoScholasticActivityDTO data);          
        CoScholasticActivityDTO editdetails(int ID);
        //------------------------ Activites Area ----------------------------------//
        CoScholasticActivityDTO savedetail1(CoScholasticActivityDTO data);
        CoScholasticActivityDTO deactivate1(CoScholasticActivityDTO data);
        CoScholasticActivityDTO validateordernumber(CoScholasticActivityDTO data);
        CoScholasticActivityDTO editdetails1(int ID);

        //---------- Activites Area Mapping -----------------//
        CoScholasticActivityDTO savedetail2(CoScholasticActivityDTO data);
        CoScholasticActivityDTO deactivate2(CoScholasticActivityDTO data);
        CoScholasticActivityDTO editdetails2(int ID);



        CoScholasticActivityDTO get_exam(CoScholasticActivityDTO data);
        CoScholasticActivityDTO getexampopup(int id);
   

    

    }
}
