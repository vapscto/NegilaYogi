
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

//PreadmissionDTOs.com.vaps.admission


namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface exammasterPersonalityInterface
    {
        // Master Personlity
        exammasterpersonalityDTO savedetails(exammasterpersonalityDTO data);
        exammasterpersonalityDTO validateordernumber(exammasterpersonalityDTO data);
        exammasterpersonalityDTO deactivate(exammasterpersonalityDTO data);
        exammasterpersonalityDTO editdetails(int ID);
        exammasterpersonalityDTO Getdetails(exammasterpersonalityDTO data);

        // Student Personlaity Mapping
        exammasterpersonalityDTO studentdataload(exammasterpersonalityDTO data);
        exammasterpersonalityDTO onchangeyear(exammasterpersonalityDTO data);
        exammasterpersonalityDTO onchangeclass(exammasterpersonalityDTO data);
        exammasterpersonalityDTO onchangesection(exammasterpersonalityDTO data);
        exammasterpersonalityDTO searchdata(exammasterpersonalityDTO data);
        exammasterpersonalityDTO savemapping(exammasterpersonalityDTO data);
        exammasterpersonalityDTO editmappingdetails(exammasterpersonalityDTO data);
        

    }
}
