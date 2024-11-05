using PreadmissionDTOs.com.vaps.Portals.Principal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Principal.Interfaces
{
    public interface NoticeInterface
    {
        Notice_DTO savedetail(Notice_DTO data);
        Notice_DTO Getdetails(Notice_DTO data);
        Notice_DTO deactivate(Notice_DTO data);
    }
}
