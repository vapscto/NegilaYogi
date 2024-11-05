using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Student.Interfaces
{
    public interface StudentBuspassFormInterface
    {
        StudentBuspassFormDTO getloaddata(StudentBuspassFormDTO data);

        StudentBuspassFormDTO getloaddataintruction(StudentBuspassFormDTO data);
        StudentBuspassFormDTO getstudata(StudentBuspassFormDTO sddto);
        Task<StudentBuspassFormDTO> getbuspassdata(StudentBuspassFormDTO sddto);
        StudentBuspassFormDTO getroutedata(StudentBuspassFormDTO data);
        StudentBuspassFormDTO academicload(StudentBuspassFormDTO data);
        StudentBuspassFormDTO getlocationdata(StudentBuspassFormDTO data);
        StudentBuspassFormDTO getlocationdataonly(StudentBuspassFormDTO data);
        StudentBuspassFormDTO savedata(StudentBuspassFormDTO data);


        StudentBuspassFormDTO paynow(StudentBuspassFormDTO data);
        StudentBuspassFormDTO paynow1(StudentBuspassFormDTO data);
        StudentBuspassFormDTO paynow2(StudentBuspassFormDTO data);

        PaymentDetails payuresponse(PaymentDetails stu);
        PaymentDetails Razorpaypaymentresponse(PaymentDetails stu);
    }
}
