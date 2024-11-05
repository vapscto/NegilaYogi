using CommonLibrary;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VisitorsManagement
{
    public class InwardDelegate
    {
        CommonDelegate<InwardDTO, InwardDTO> COMVISITOR = new CommonDelegate<InwardDTO, InwardDTO>();

        public InwardDTO saveData(InwardDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "InwardFacade/saveData/");
        }

        public InwardDTO getDetails(InwardDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "InwardFacade/getDetails/");
        }

        public InwardDTO EditDetails(InwardDTO id)
        {
            return COMVISITOR.POSTDataVisitors(id, "InwardFacade/EditDetails/");
        }
        public InwardDTO deactivate(InwardDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "InwardFacade/deactivate/");
        }
        public InwardDTO searchfilter(InwardDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "InwardFacade/searchfilter/");
        }

        public InwardDTO get_empdetails(InwardDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "InwardFacade/get_empdetails/");
        }

        public InwardDTO searchfilter2(InwardDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "InwardFacade/searchfilter2/");
        }

        public InwardDTO get_empdetails2(InwardDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "InwardFacade/get_empdetails2/");
        }
        
    }
}
