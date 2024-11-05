using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.FrontOffice;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace corewebapi18072016.Delegates.com.vapstech.FrontOffice
{
    public class ShiftSettingMasterDelegate
    {

        CommonDelegate<MasterShiftsTimingsDTO, MasterShiftsTimingsDTO> COMFRNT = new CommonDelegate<MasterShiftsTimingsDTO, MasterShiftsTimingsDTO>();

        public MasterShiftsTimingsDTO getdata(MasterShiftsTimingsDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "ShiftSettingMasterFacade/getalldetails/");
        }
        public MasterShiftsTimingsDTO savedatadelegate(MasterShiftsTimingsDTO data)
        {
            return COMFRNT.POSTDataHolidayReport(data, "ShiftSettingMasterFacade/savedata/");
        }

        public MasterShiftsTimingsDTO EditDetails(int id)
        {
            return COMFRNT.GetDataByIdFROFF(id, "ShiftSettingMasterFacade/getpagedetails/");
        }
        public MasterShiftsTimingsDTO deactivate(MasterShiftsTimingsDTO id)
        {
            return COMFRNT.POSTDataHolidayReport(id, "ShiftSettingMasterFacade/deactivate/");
        }
        public MasterShiftsTimingsDTO deleterec(int id)
        {
            return COMFRNT.DeleteDataByIdFROFF(id, "ShiftSettingMasterFacade/deletedetails/");
        }
        public MasterShiftsTimingsDTO getalldetailsviewrecords1(int id)
        {
            return COMFRNT.GetDataByIdFROFF(id, "ShiftSettingMasterFacade/getalldetailsviewrecords1/");
        }

    }
}
