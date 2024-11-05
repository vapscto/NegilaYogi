using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;

namespace WebApplication1.Interfaces
{
    public interface AdmissionStatusInterface
    {
        AdmissionStatusDTO getallDetails(AdmissionStatusDTO data);

        AdmissionStatusDTO editdetail(int id);
        AdmissionStatusDTO savedataa(AdmissionStatusDTO data);
        AdmissionStatusDTO deletedata(AdmissionStatusDTO dta);
    }
}
