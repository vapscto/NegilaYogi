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
    public class MasterHolidayDelegate
    {       
        CommonDelegate<MasterHolidayDTO, MasterHolidayDTO> COMFRNT = new CommonDelegate<MasterHolidayDTO, MasterHolidayDTO>();
        public MasterHolidayDTO getdata(int id)
        {
            return COMFRNT.GetDataByIdFROFF(id, "MasterHolidayFacade/getdata/");
        }

        public MasterHolidayDTO delete_data(MasterHolidayDTO categorypage)
        {
            return COMFRNT.POSTDataHolidayReport(categorypage, "MasterHolidayFacade/delete/");
        }

        public MasterHolidayDTO savedata(MasterHolidayDTO categorypage)
        {
            return COMFRNT.POSTDataHolidayReport(categorypage, "MasterHolidayFacade/savedetail/");
        }

        public MasterHolidayDTO Change(MasterHolidayDTO categorypage)
        {
            return COMFRNT.POSTDataHolidayReport(categorypage, "MasterHolidayFacade/Change/");
        }

        public MasterHolidayDTO saveadvmasterHolidaydata(MasterHolidayDTO categorypage)
        {
            return COMFRNT.POSTDataHolidayReport(categorypage, "MasterHolidayFacade/saveadvmasterHolidaydata/");
        }

        public MasterHolidayDTO advloaddata(MasterHolidayDTO obj)
        {
            return COMFRNT.POSTDataHolidayReport(obj, "MasterHolidayFacade/advloaddata/");
        }

        public MasterHolidayDTO advdelete(int id)
        {
            return COMFRNT.GetDataByIdFROFF(id, "MasterHolidayFacade/advdelete/");
        }

        public MasterHolidayDTO editadvmasterHoliday(MasterHolidayDTO obj)
        {
            return COMFRNT.POSTDataHolidayReport(obj, "MasterHolidayFacade/editadvmasterHoliday/");
        }
    }
}
