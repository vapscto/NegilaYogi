using PreadmissionDTOs.com.vaps.Portals.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.Employee.Interfaces
{
    public interface IVRM_DocsUploadInterface
    {

        IVRM_DocsUploadDTO Getdetails(IVRM_DocsUploadDTO data);
        IVRM_DocsUploadDTO savedetail(IVRM_DocsUploadDTO data);
        Task<IVRM_DocsUploadDTO> get_classes(IVRM_DocsUploadDTO data);
        IVRM_DocsUploadDTO getsectiondata(IVRM_DocsUploadDTO data);
        IVRM_DocsUploadDTO deactivate(IVRM_DocsUploadDTO data);
        IVRM_DocsUploadDTO editData(IVRM_DocsUploadDTO data);

    }
}
