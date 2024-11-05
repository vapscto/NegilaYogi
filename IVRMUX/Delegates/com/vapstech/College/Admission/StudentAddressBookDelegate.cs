using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Admission
{
    public class StudentAddressBookDelegate
    {
        CommonDelegate<StudentAddressBookDTO, StudentAddressBookDTO> comm = new CommonDelegate<StudentAddressBookDTO, StudentAddressBookDTO>();

        public StudentAddressBookDTO loaddata(StudentAddressBookDTO data)
        {
            return comm.clgadmissionbypost(data, "StudentAddressBookFacade/loaddata");
        }
        public StudentAddressBookDTO getcourse(StudentAddressBookDTO data)
        {
            return comm.clgadmissionbypost(data, "StudentAddressBookFacade/getcourse");
        }
        public StudentAddressBookDTO getbranch(StudentAddressBookDTO data)
        {
            return comm.clgadmissionbypost(data, "StudentAddressBookFacade/getbranch");
        }
        public StudentAddressBookDTO getsemester(StudentAddressBookDTO data)
        {
            return comm.clgadmissionbypost(data, "StudentAddressBookFacade/getsemester");
        }
        public StudentAddressBookDTO onselectBranch(StudentAddressBookDTO data)
        {
            return comm.clgadmissionbypost(data, "StudentAddressBookFacade/onselectBranch");
        }
        public StudentAddressBookDTO getsection(StudentAddressBookDTO data)
        {
            return comm.clgadmissionbypost(data, "StudentAddressBookFacade/getsection");
        }
        public StudentAddressBookDTO getstudent(StudentAddressBookDTO data)
        {
            return comm.clgadmissionbypost(data, "StudentAddressBookFacade/getstudent");
        }
        public StudentAddressBookDTO Report(StudentAddressBookDTO data)
        {
            return comm.clgadmissionbypost(data, "StudentAddressBookFacade/Report");
        }
        public StudentAddressBookDTO AddressBookFormat2(StudentAddressBookDTO data)
        {
            return comm.clgadmissionbypost(data, "StudentAddressBookFacade/AddressBookFormat2");
        }
    }
}
