using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;
using PreadmissionDTOs;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface StudentAddressBook1Interface
    {

        Task<StudentAddressBook1DTO> getInitailData(int id);
        Task<StudentAddressBook1DTO> yearchange(StudentAddressBook1DTO id);
        Task<StudentAddressBook1DTO> getclass(StudentAddressBook1DTO id);
        Task<StudentAddressBook1DTO> getyear(StudentAddressBook1DTO id);
        Task<StudentAddressBook1DTO> getsection(StudentAddressBook1DTO id);
        Task<StudentAddressBook1DTO> getdetails(StudentAddressBook1DTO resource);
        Task<StudentAddressBook1DTO> sectinchange(StudentAddressBook1DTO dto);
      
    }
}
