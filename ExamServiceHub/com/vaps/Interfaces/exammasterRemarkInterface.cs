
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

//PreadmissionDTOs.com.vaps.admission


namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface exammasterRemarkInterface
    {
        exammasterRemarkDTO savedetails(exammasterRemarkDTO data);
        exammasterRemarkDTO validateordernumber(exammasterRemarkDTO data);
        exammasterRemarkDTO deactivate(exammasterRemarkDTO data);
        exammasterRemarkDTO editdetails(int ID);
        exammasterRemarkDTO Getdetails(exammasterRemarkDTO data);

        // Student Personlaity Mapping
        exammasterRemarkDTO studentdataload(exammasterRemarkDTO data);
        exammasterRemarkDTO onchangeyear(exammasterRemarkDTO data);
        exammasterRemarkDTO onchangeclass(exammasterRemarkDTO data);
        exammasterRemarkDTO onchangesection(exammasterRemarkDTO data);
        exammasterRemarkDTO searchdata(exammasterRemarkDTO data);
        exammasterRemarkDTO savemapping(exammasterRemarkDTO data);
        exammasterRemarkDTO editmappingdetails(exammasterRemarkDTO data);
        exammasterRemarkDTO ViewSubjectWiseRemarks(exammasterRemarkDTO data);

        //Subject Wise Remarks
        exammasterRemarkDTO Subjectwise_studentdataload(exammasterRemarkDTO data);
        exammasterRemarkDTO Subjectwise_onchangeyear(exammasterRemarkDTO data);
        exammasterRemarkDTO Subjectwise_onchangeclass(exammasterRemarkDTO data);
        exammasterRemarkDTO Subjectwise_onchangesection(exammasterRemarkDTO data);
        exammasterRemarkDTO Subjectwise_onchangeexam(exammasterRemarkDTO data);
        exammasterRemarkDTO Subjectwise_searchdata(exammasterRemarkDTO data);
        exammasterRemarkDTO SubjectWise_savemapping(exammasterRemarkDTO data);
        exammasterRemarkDTO SubjectWise_editmappingdetails(exammasterRemarkDTO data);
    }
}
