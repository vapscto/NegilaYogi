using CommonLibrary;
using PreadmissionDTOs.com.vaps.Alumni;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Alumni
{
    public class Alumni_Interactions_Delegate
    {
        CommonDelegate<Alumni_School_Interactions_DTO, Alumni_School_Interactions_DTO> comm = new CommonDelegate<Alumni_School_Interactions_DTO, Alumni_School_Interactions_DTO>();
        public Alumni_School_Interactions_DTO getloaddata(Alumni_School_Interactions_DTO dto)
        {
            return comm.POSTDataAlumni(dto, "Alumni_InteractionsFacade/getloaddata/");
        }
        public Alumni_School_Interactions_DTO getdetails(Alumni_School_Interactions_DTO dto)
        {
            return comm.POSTDataAlumni(dto, "Alumni_InteractionsFacade/getdetails/");
        }
        public Alumni_School_Interactions_DTO savedetails(Alumni_School_Interactions_DTO dto)
        {
            return comm.POSTDataAlumni(dto, "Alumni_InteractionsFacade/savedetails/");
        }
        public Alumni_School_Interactions_DTO reply(Alumni_School_Interactions_DTO dto)
        {
            return comm.POSTDataAlumni(dto, "Alumni_InteractionsFacade/reply/");
        }
        public Alumni_School_Interactions_DTO savereply(Alumni_School_Interactions_DTO dto)
        {
            return comm.POSTDataAlumni(dto, "Alumni_InteractionsFacade/savereply/");
        }


    }
}
