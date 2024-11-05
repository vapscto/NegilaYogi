using CommonLibrary;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.HRMS
{
    public class GroupDeptDessgDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<HRGroupDeptDessgDTO, HRGroupDeptDessgDTO> COMMM = new CommonDelegate<HRGroupDeptDessgDTO, HRGroupDeptDessgDTO>();

        public HRGroupDeptDessgDTO loaddata(HRGroupDeptDessgDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "GroupDeptDessgFacade/loaddata");
        }
        public HRGroupDeptDessgDTO savedata(HRGroupDeptDessgDTO dto)
        {
            return COMMM.POSTDataHRMS(dto, "GroupDeptDessgFacade/savedata");
        }
        public HRGroupDeptDessgDTO Editdata(HRGroupDeptDessgDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "GroupDeptDessgFacade/Editdata/");
        }
        public HRGroupDeptDessgDTO masterDecative(HRGroupDeptDessgDTO maspage)
        {
            return COMMM.POSTDataHRMS(maspage, "GroupDeptDessgFacade/masterDecative/");
        }
    }
}
