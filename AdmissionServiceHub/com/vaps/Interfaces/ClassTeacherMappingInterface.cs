using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
   public interface ClassTeacherMappingInterface 
    {
         ClassTeacherMappingDTO getdetails(int id);
        ClassTeacherMappingDTO save(ClassTeacherMappingDTO data);
        ClassTeacherMappingDTO GetSelectedRowDetails(ClassTeacherMappingDTO data);
        ClassTeacherMappingDTO onchangestaff1(ClassTeacherMappingDTO data);
        ClassTeacherMappingDTO onchangestaff2(ClassTeacherMappingDTO data);
        ClassTeacherMappingDTO exchangesave(ClassTeacherMappingDTO data);
        ClassTeacherMappingDTO deleterecord(ClassTeacherMappingDTO data);
    }
}
