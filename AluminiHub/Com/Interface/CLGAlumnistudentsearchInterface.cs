using PreadmissionDTOs.com.vaps.Alumni;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlumniHub.Com.Interface
{
    public interface CLGAlumnistudentsearchInterface 
    {
        CLGAlumniStudentDTO getData(CLGAlumniStudentDTO admDto);

        CLGAlumniStudentDTO getsemdata(CLGAlumniStudentDTO admDto);
    }
}
