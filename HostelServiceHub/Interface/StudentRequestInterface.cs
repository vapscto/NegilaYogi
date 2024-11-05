using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HostelServiceHub.Interface
{
   public  interface StudentRequestInterface
    {
        StudentRequestDTO save(StudentRequestDTO data);
        Task<StudentRequestDTO> loaddata(StudentRequestDTO data);
        StudentRequestDTO edittab1(StudentRequestDTO data);
        StudentRequestDTO deactive(StudentRequestDTO data);
    }
}
