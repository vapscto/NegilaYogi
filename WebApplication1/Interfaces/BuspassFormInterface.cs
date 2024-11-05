using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
    public interface BuspassFormInterface
    {
        StudentHelthcertificateDTO getloaddata(StudentHelthcertificateDTO data);
        Task<StudentHelthcertificateDTO> getstudata(StudentHelthcertificateDTO sddto);
        Task<StudentHelthcertificateDTO> getbuspassdata(StudentHelthcertificateDTO sddto);
        StudentHelthcertificateDTO getroutedata(StudentHelthcertificateDTO data);
        StudentHelthcertificateDTO getlocationdata(StudentHelthcertificateDTO data);
        StudentHelthcertificateDTO savedata(StudentHelthcertificateDTO data);

        StudentHelthcertificateDTO paynow (StudentHelthcertificateDTO data);

        PaymentDetails payuresponse(PaymentDetails stu);
    }

}
