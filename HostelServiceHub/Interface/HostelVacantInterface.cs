using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HostelServiceHub.Interface
{
    public interface StudentVacantInterface
    {

        Task<StudentVacantDTO> loaddata(StudentVacantDTO data);
        StudentVacantDTO edittab1(StudentVacantDTO data);
        Task<StudentVacantDTO> getalldetailsOnselectiontype(StudentVacantDTO data);
        Task<StudentVacantDTO> get_studentDetail(StudentVacantDTO data);
        Task<StudentVacantDTO> get_staffDetail(StudentVacantDTO data);
        StudentVacantDTO get_guestDetail(StudentVacantDTO data);


    }
}
