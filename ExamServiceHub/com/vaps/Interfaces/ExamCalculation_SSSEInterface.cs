
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

//PreadmissionDTOs.com.vaps.admission


namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface ExamCalculation_SSSEInterface
    {

        ExamCalculation_SSSEDTO Getdetails(ExamCalculation_SSSEDTO data);
        ExamCalculation_SSSEDTO get_cls_sections(ExamCalculation_SSSEDTO data);
        ExamCalculation_SSSEDTO Calculation(ExamCalculation_SSSEDTO data);
        ExamCalculation_SSSEDTO get_classes(ExamCalculation_SSSEDTO id);
        ExamCalculation_SSSEDTO get_exams(ExamCalculation_SSSEDTO id);
        ExamCalculation_SSSEDTO onchangeexam(ExamCalculation_SSSEDTO id);       
        ExamCalculation_SSSEDTO saveapporvecal(ExamCalculation_SSSEDTO id);

        //Student Wise Publish
        ExamCalculation_SSSEDTO ChangeOfSection(ExamCalculation_SSSEDTO id);
        ExamCalculation_SSSEDTO CheckMarksCalculated(ExamCalculation_SSSEDTO id);
        ExamCalculation_SSSEDTO SearchStudent(ExamCalculation_SSSEDTO id);
        ExamCalculation_SSSEDTO SaveStudentStatus(ExamCalculation_SSSEDTO id);

        //Promotion
        ExamCalculation_SSSEDTO onchangesection(ExamCalculation_SSSEDTO data);
        ExamCalculation_SSSEDTO promotionsaveddata(ExamCalculation_SSSEDTO id);
        ExamCalculation_SSSEDTO publishtostudentportal(ExamCalculation_SSSEDTO id);
    }
}
