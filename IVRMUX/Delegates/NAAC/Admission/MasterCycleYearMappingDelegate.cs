using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
namespace IVRMUX.Delegates.NAAC.Admission
{
    public class MasterCycleYearMappingDelegate
    {
        CommonDelegate<MasterCycleYearMappingDTO, MasterCycleYearMappingDTO> _comm = new CommonDelegate<MasterCycleYearMappingDTO, MasterCycleYearMappingDTO>();

        public MasterCycleYearMappingDTO getalldetails(MasterCycleYearMappingDTO data)
        {
            return _comm.naacdetailsbypost(data, "MasterCycleYearMappingFacade/getalldetails");
        }
        public MasterCycleYearMappingDTO savedetails(MasterCycleYearMappingDTO data)
        {
            return _comm.naacdetailsbypost(data, "MasterCycleYearMappingFacade/savedetails");
        }
        public MasterCycleYearMappingDTO activedeactivedetails(MasterCycleYearMappingDTO data)
        {
            return _comm.naacdetailsbypost(data, "MasterCycleYearMappingFacade/activedeactivedetails");
        }
        public MasterCycleYearMappingDTO editdetails(MasterCycleYearMappingDTO data)
        {
            return _comm.naacdetailsbypost(data, "MasterCycleYearMappingFacade/editdetails");
        }
        public MasterCycleYearMappingDTO getOrder(MasterCycleYearMappingDTO data)
        {
            return _comm.naacdetailsbypost(data, "MasterCycleYearMappingFacade/getOrder");
        }

        // Master Cycle Year Mapping  
        public MasterCycleYearMappingDTO onchangecycle(MasterCycleYearMappingDTO data)
        {
            return _comm.naacdetailsbypost(data, "MasterCycleYearMappingFacade/onchangecycle");
        }
        public MasterCycleYearMappingDTO savedetails1(MasterCycleYearMappingDTO data)
        {
            return _comm.naacdetailsbypost(data, "MasterCycleYearMappingFacade/savedetails1");
        }
        public MasterCycleYearMappingDTO viewdetails(MasterCycleYearMappingDTO data)
        {
            return _comm.naacdetailsbypost(data, "MasterCycleYearMappingFacade/viewdetails");
        }
        public MasterCycleYearMappingDTO deactivesem(MasterCycleYearMappingDTO data)
        {
            return _comm.naacdetailsbypost(data, "MasterCycleYearMappingFacade/deactivesem");
        }
        public MasterCycleYearMappingDTO delete(MasterCycleYearMappingDTO data)
        {
            return _comm.naacdetailsbypost(data, "MasterCycleYearMappingFacade/delete");
        }
    }

}
