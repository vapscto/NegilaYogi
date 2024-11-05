using CommonLibrary;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.TT.College
{
    public class ClgPeriodAllocationDelegate
    {
        CommonDelegate<ClgPeriodAllocation_DTO, ClgPeriodAllocation_DTO> comm = new CommonDelegate<ClgPeriodAllocation_DTO, ClgPeriodAllocation_DTO>();
        public ClgPeriodAllocation_DTO save_period(ClgPeriodAllocation_DTO data)
        {
            return comm.POSTDataTimeTable(data, "ClgPeriodAllocationFacade/save_period");
        }
        //public ClgPeriodAllocation_DTO getcategories(ClgPeriodAllocation_DTO data)
        //{
        //    return comm.POSTDataTimeTable(data, "ClgPeriodAllocationFacade/getcategories");
        //}
        public ClgPeriodAllocation_DTO getdetails(ClgPeriodAllocation_DTO data)
        {

            return comm.POSTDataTimeTable(data, "ClgPeriodAllocationFacade/getdetails");
        }
        public ClgPeriodAllocation_DTO deactivate(ClgPeriodAllocation_DTO data)
        {
            return comm.POSTDataTimeTable(data, "ClgPeriodAllocationFacade/deactivate");
        }
        public ClgPeriodAllocation_DTO deactivate1(ClgPeriodAllocation_DTO data)
        {
            return comm.POSTDataTimeTable(data, "ClgPeriodAllocationFacade/deactivate1");
        }


        public ClgPeriodAllocation_DTO getcategories(ClgPeriodAllocation_DTO data)
        {

            return comm.POSTDataTimeTable(data, "ClgPeriodAllocationFacade/getcategories");
        }

        public ClgPeriodAllocation_DTO getperiod_class(ClgPeriodAllocation_DTO data)
        {

            return comm.POSTDataTimeTable(data, "ClgPeriodAllocationFacade/getperiod_class");
        }
        public ClgPeriodAllocation_DTO savedetail(ClgPeriodAllocation_DTO data)
        {
            return comm.POSTDataTimeTable(data, "ClgPeriodAllocationFacade/savedetail");
        }

    }
}
