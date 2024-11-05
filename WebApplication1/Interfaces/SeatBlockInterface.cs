using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
    public interface SeatBlockInterface
    {
        Task<Preadmission_SeatBlocked_StudentDTO> saveSeatBlock(Preadmission_SeatBlocked_StudentDTO InstitDTO);
        Preadmission_SeatBlocked_StudentDTO AllDropdownList(int stu);
       Task<Preadmission_SeatBlocked_StudentDTO> deleterec(int id);
        Preadmission_SeatBlocked_StudentDTO getdetails(int id);
        Preadmission_SeatBlocked_StudentDTO getSeatConfirmedStud(Preadmission_SeatBlocked_StudentDTO stud);

    }
}
