using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
   public interface prospectus
    {
        Task<ProspectusDTO> saveProsdet(ProspectusDTO pros);
        ProspectusDTO countrydrp(ProspectusDTO stu);
        StateDTO enqdrpcountrydata(int id);
        CityDTO getcity(int id);
        ProspectusDTO deleterec(ProspectusDTO stu);
        ProspectusDTO getdetails(int id);
        Enq getEnqdetails(searchEnquiryDTO id);
        ProspectusDTO searchByColumn(ProspectusDTO pros);

        ProspectusDTO getfilePath(int id);

        PaymentDetails payuresponse(PaymentDetails response);
    }
}
