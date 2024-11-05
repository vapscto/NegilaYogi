using PreadmissionDTOs.com.vaps.Alumni;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlumniHub.Com.Interface
{
    public interface Alumni_NoticeBoard_Interface
    {
        Alumni_NoticeBoard_DTO loaddata(Alumni_NoticeBoard_DTO dto);
        Alumni_NoticeBoard_DTO savedetail(Alumni_NoticeBoard_DTO dto);
        Alumni_NoticeBoard_DTO viewData(Alumni_NoticeBoard_DTO dto);
        Alumni_NoticeBoard_DTO deactivate(Alumni_NoticeBoard_DTO dto);
        Alumni_NoticeBoard_DTO editdetails(Alumni_NoticeBoard_DTO dto);
    }
}
