using PreadmissionDTOs.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface StudentTransactionInterface
    {
        StudentTransactionDTO getDetails(StudentTransactionDTO dto);
        StudentTransactionDTO onchangeyear(StudentTransactionDTO dto);
        StudentTransactionDTO onchangeclass(StudentTransactionDTO dto);
        StudentTransactionDTO onchangesection(StudentTransactionDTO dto);
        StudentTransactionDTO onchangeskills(StudentTransactionDTO dto);
        StudentTransactionDTO onchangeactivites(StudentTransactionDTO dto);
        StudentTransactionDTO getStudentList(StudentTransactionDTO dto);
        StudentTransactionDTO save(StudentTransactionDTO obj);
        StudentTransactionDTO onchangeactivitesskillflag(StudentTransactionDTO obj);
        //getmobiletList
        StudentTransactionDTO getmobiletList(StudentTransactionDTO obj);

    }
}
