using CommonLibrary;
using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Placement
{
    public class PL_CI_StudentStatusDelegate
    {
        CommonDelegate<PL_CI_StudentStatusDTO, PL_CI_StudentStatusDTO> _comm = new CommonDelegate<PL_CI_StudentStatusDTO, PL_CI_StudentStatusDTO>();
        public PL_CI_StudentStatusDTO loaddata(PL_CI_StudentStatusDTO data)
        {
            return _comm.POSTDataPlacement(data, "PL_CI_StudentStatusFacade/loaddata/");
        }

        public PL_CI_StudentStatusDTO savedetails(PL_CI_StudentStatusDTO data)
        {
            return _comm.POSTDataPlacement(data, "PL_CI_StudentStatusFacade/savedetails/");
        }

        public PL_CI_StudentStatusDTO editdetails(PL_CI_StudentStatusDTO data)
        {
            return _comm.POSTDataPlacement(data, "PL_CI_StudentStatusFacade/editdetails");
        }
        public PL_CI_StudentStatusDTO deactive(PL_CI_StudentStatusDTO data)
        {
            return _comm.POSTDataPlacement(data, "PL_CI_StudentStatusFacade/deactive/");
        }

    }
}
