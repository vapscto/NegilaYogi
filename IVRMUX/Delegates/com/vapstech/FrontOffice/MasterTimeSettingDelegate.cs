using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace corewebapi18072016.Delegates.com.vapstech.FrontOffice
{
    public class MasterTimeSettingDelegate
    {
        CommonDelegate<MasterTimeSettingDTO, MasterTimeSettingDTO> COMFRNT = new CommonDelegate<MasterTimeSettingDTO, MasterTimeSettingDTO>();
        public MasterTimeSettingDTO savedetail(MasterTimeSettingDTO categorypage)
        {
            return COMFRNT.POSTDataHolidayReport(categorypage, "MasterTimeSettingFacade/savedetail");

        }
               
        public MasterTimeSettingDTO getdetails(int id)
        {

            return COMFRNT.GetDataByIdFROFF(id, "MasterTimeSettingFacade/getdetails/");
        }

   
        public MasterTimeSettingDTO deleterec(int id)
        {
            return COMFRNT.DeleteDataByIdFROFF(id, "PeriodTimeSettingFacade/deletedetails/");
        }
    }
}
