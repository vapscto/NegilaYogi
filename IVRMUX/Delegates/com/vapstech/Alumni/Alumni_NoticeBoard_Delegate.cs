using CommonLibrary;
using PreadmissionDTOs.com.vaps.Alumni;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Alumni
{
    public class Alumni_NoticeBoard_Delegate
    {
        CommonDelegate<Alumni_NoticeBoard_DTO, Alumni_NoticeBoard_DTO> COMMM = new CommonDelegate<Alumni_NoticeBoard_DTO, Alumni_NoticeBoard_DTO>();
        public Alumni_NoticeBoard_DTO loaddata (Alumni_NoticeBoard_DTO dto)
        {
            return COMMM.POSTDataAlumni(dto, "Alumni_NoticeBoardFacade/loaddata/");
        }
        public Alumni_NoticeBoard_DTO savedetail(Alumni_NoticeBoard_DTO dto)
        {
            return COMMM.POSTDataAlumni(dto, "Alumni_NoticeBoardFacade/savedetail/");
        }
         public Alumni_NoticeBoard_DTO viewData(Alumni_NoticeBoard_DTO dto)
        {
            return COMMM.POSTDataAlumni(dto, "Alumni_NoticeBoardFacade/viewData/");
        }
         public Alumni_NoticeBoard_DTO deactivate(Alumni_NoticeBoard_DTO dto)
        {
            return COMMM.POSTDataAlumni(dto, "Alumni_NoticeBoardFacade/deactivate/");
        }
         public Alumni_NoticeBoard_DTO editdetails(Alumni_NoticeBoard_DTO dto)
        {
            return COMMM.POSTDataAlumni(dto, "Alumni_NoticeBoardFacade/editdetails/");
        }


    }
}
