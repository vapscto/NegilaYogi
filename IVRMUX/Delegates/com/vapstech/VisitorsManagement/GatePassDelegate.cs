using CommonLibrary;
using PreadmissionDTOs.com.vaps.VisitorsManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.VisitorsManagement
{
    public class GatePassDelegate
    {
        CommonDelegate<GatePassDTO, GatePassDTO> COMVISITOR = new CommonDelegate<GatePassDTO, GatePassDTO>();
        public GatePassDTO getDetails(GatePassDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "GatePassFacade/getDetails/");
        }
        public GatePassDTO saveData(GatePassDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "GatePassFacade/saveData/");
        }
        public GatePassDTO getStudentDetails(GatePassDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "GatePassFacade/getStudentDetails/");
        }
        public GatePassDTO sendmail(GatePassDTO obj)
        {
            return COMVISITOR.POSTDataVisitors(obj, "GatePassFacade/sendmail/");
        }

    }
}
