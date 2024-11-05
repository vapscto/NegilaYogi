using PreadmissionDTOs.com.vaps.Hostel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HostelServiceHub.Interface
{
    public interface CLGHostelVacantInterface
    {

        Task<CLGHostelVacantDTO> loaddata(CLGHostelVacantDTO data);
        CLGHostelVacantDTO edittab1(CLGHostelVacantDTO data);
        Task<CLGHostelVacantDTO> getalldetailsOnselectiontype(CLGHostelVacantDTO data);
        Task<CLGHostelVacantDTO> get_studentDetail(CLGHostelVacantDTO data);
        Task<CLGHostelVacantDTO> get_staffDetail(CLGHostelVacantDTO data);
        CLGHostelVacantDTO get_guestDetail(CLGHostelVacantDTO data);


    }
}
