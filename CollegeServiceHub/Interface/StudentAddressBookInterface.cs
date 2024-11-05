using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface StudentAddressBookInterface
    {
        StudentAddressBookDTO loaddata(StudentAddressBookDTO data);
        StudentAddressBookDTO getcourse(StudentAddressBookDTO data);
        StudentAddressBookDTO getbranch(StudentAddressBookDTO data);
        StudentAddressBookDTO getsemester(StudentAddressBookDTO data);
        StudentAddressBookDTO onselectBranch(StudentAddressBookDTO data);
        StudentAddressBookDTO getsection(StudentAddressBookDTO data);
        StudentAddressBookDTO getstudent(StudentAddressBookDTO data);
        StudentAddressBookDTO Report(StudentAddressBookDTO data);
        StudentAddressBookDTO AddressBookFormat2(StudentAddressBookDTO data);
    }
}
