using PreadmissionDTOs.com.vaps.College.Fee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Interfaces
{
  public  interface Clg_YearlyFeeGroupMappingInterface
    {
        CLG_YearlyFeeGroupHeadMapping_DTO getdata(CLG_YearlyFeeGroupHeadMapping_DTO data);
        CLG_YearlyFeeGroupHeadMapping_DTO savedetails(CLG_YearlyFeeGroupHeadMapping_DTO pgmod);
        CLG_YearlyFeeGroupHeadMapping_DTO deleterec(int id);
        CLG_YearlyFeeGroupHeadMapping_DTO getsearchdata(int id, CLG_YearlyFeeGroupHeadMapping_DTO org);
        CLG_YearlyFeeGroupHeadMapping_DTO EditMasterscetionDetails(CLG_YearlyFeeGroupHeadMapping_DTO data);

        CLG_YearlyFeeGroupHeadMapping_DTO getdataongroup(CLG_YearlyFeeGroupHeadMapping_DTO data);

        CLG_YearlyFeeGroupHeadMapping_DTO selectacade(CLG_YearlyFeeGroupHeadMapping_DTO data);
    }
}
