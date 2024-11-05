using CommonLibrary;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VisitorsManagement
{
    public class OutwardDelegate
    {
        CommonDelegate<OutwardDTO, OutwardDTO> COMVISITOR = new CommonDelegate<OutwardDTO, OutwardDTO>();

        public OutwardDTO saveData(OutwardDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "OutwardFacade/saveData/");
        }

        public OutwardDTO getDetails(OutwardDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "OutwardFacade/getDetails/");
        }

        public OutwardDTO EditDetails(OutwardDTO id)
        {
            return COMVISITOR.POSTDataVisitors(id, "OutwardFacade/EditDetails/");
        }

        public OutwardDTO deactivate(OutwardDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "OutwardFacade/deactivate/");
        }
    }
}
