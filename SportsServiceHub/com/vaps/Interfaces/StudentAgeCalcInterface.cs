using PreadmissionDTOs.com.vaps.Sports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsServiceHub.com.vaps.Interfaces
{
    public interface StudentAgeCalcInterface
    {
        StudentAgeCalcDTO Getdetails(StudentAgeCalcDTO data);
        StudentAgeCalcDTO saveRecord(StudentAgeCalcDTO data);
        StudentAgeCalcDTO getStudents(StudentAgeCalcDTO data); 
        StudentAgeCalcDTO report(StudentAgeCalcDTO data);
        StudentAgeCalcDTO Get_Class_House(StudentAgeCalcDTO data);
    }
}
