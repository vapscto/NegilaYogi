using PreadmissionDTOs.com.vaps.Alumni;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlumniHub.Com.Interface
{
    public interface Alumni_Interactions_Interface
    {
        Alumni_School_Interactions_DTO getloaddata(Alumni_School_Interactions_DTO dto);
        Alumni_School_Interactions_DTO getdetails(Alumni_School_Interactions_DTO dto);
        Alumni_School_Interactions_DTO savedetails(Alumni_School_Interactions_DTO dto);
        Alumni_School_Interactions_DTO reply(Alumni_School_Interactions_DTO dto);
        Alumni_School_Interactions_DTO savereply(Alumni_School_Interactions_DTO dto);
    }
}
