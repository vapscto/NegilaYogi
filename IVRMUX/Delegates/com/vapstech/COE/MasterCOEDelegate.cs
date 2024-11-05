using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.COE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.COE
{
    public class MasterCOEDelegate
    {
        CommonDelegate<MasterCOEDTO, MasterCOEDTO> COMMM = new CommonDelegate<MasterCOEDTO, MasterCOEDTO>();
        public MasterCOEDTO savedetail1(MasterCOEDTO categorypage)
        {
            return COMMM.POSTDataCOE(categorypage, "MasterCOEFacade/savedetail1");
        }

        public MasterCOEDTO savedetail2(MasterCOEDTO categorypage)
        {
            return COMMM.POSTDataCOE(categorypage, "MasterCOEFacade/savedetail2");
        }

        public MasterCOEDTO deactivate1(MasterCOEDTO categorypage)
        {
            return COMMM.POSTDataCOE(categorypage, "MasterCOEFacade/deactivate1");
        }

        public MasterCOEDTO deactivate2(MasterCOEDTO categorypage)
        {
            return COMMM.POSTDataCOE(categorypage, "MasterCOEFacade/deactivate2");
        }

        public MasterCOEDTO getdetails(int id)
        {
            return COMMM.GetDataByIdCOE(id, "MasterCOEFacade/getdetails/");
        }

        public MasterCOEDTO geteventdetails(MasterCOEDTO categorypage)
        {
            return COMMM.POSTDataCOE(categorypage, "MasterCOEFacade/geteventdetails");
        }


        public MasterCOEDTO getalldetailsviewrecords1(int id)
        {
            return COMMM.GetDataByIdCOE(id, "MasterCOEFacade/getalldetailsviewrecords1/");

        }

        public MasterCOEDTO getalldetailsviewrecords2(int id)
        {
            return COMMM.GetDataByIdCOE(id, "MasterCOEFacade/getalldetailsviewrecords2/");
        }

        public MasterCOEDTO getpagedetails1(int id)
        {
            return COMMM.GetDataByIdCOE(id, "MasterCOEFacade/getpagedetails1/");
        }

        public MasterCOEDTO getpagedetails2(int id)
        {
            return COMMM.GetDataByIdCOE(id, "MasterCOEFacade/getpagedetails2/");
        }
        public MasterCOEDTO deleterec(int id)
        {
            return COMMM.DeleteDataByIdCOE(id, "MasterCOEFacade/deletedetails/");
        }
    }
}
