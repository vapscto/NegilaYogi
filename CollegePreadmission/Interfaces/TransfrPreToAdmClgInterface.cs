using PreadmissionDTOs.com.vaps.College.Preadmission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegePreadmission.Interfaces
{
  public  interface TransfrPreToAdmClgInterface
    {
       TransfrPreToAdmDTO onloadgetdetails(TransfrPreToAdmDTO dto);
        TransfrPreToAdmDTO get_branchs(TransfrPreToAdmDTO data);

        TransfrPreToAdmDTO get_semester(TransfrPreToAdmDTO data);
        TransfrPreToAdmDTO getserdata(TransfrPreToAdmDTO data);

        Task<TransfrPreToAdmDTO> expoadmi(TransfrPreToAdmDTO TransfrPreToAdmDTO);
    }
}
