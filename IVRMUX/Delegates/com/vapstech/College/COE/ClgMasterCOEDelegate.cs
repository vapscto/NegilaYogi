using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.COE;
using PreadmissionDTOs.com.vaps.College.COE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.College.COE
{
    public class ClgMasterCOEDelegate
    {
        CommonDelegate<ClgMasterCOEDTO, ClgMasterCOEDTO> COMMM = new CommonDelegate<ClgMasterCOEDTO, ClgMasterCOEDTO>();
        public ClgMasterCOEDTO savedetail1(ClgMasterCOEDTO categorypage)
        {
            return COMMM.POSTDataCOE(categorypage, "ClgCOEMasterFacade/savedetail1");
        }
        public ClgMasterCOEDTO courseselect(ClgMasterCOEDTO categorypage)
        {
            return COMMM.POSTDataCOE(categorypage, "ClgCOEMasterFacade/courseselect");
        }
        public ClgMasterCOEDTO branchselect(ClgMasterCOEDTO categorypage)
        {
            return COMMM.POSTDataCOE(categorypage, "ClgCOEMasterFacade/branchselect");
        }
       
        public ClgMasterCOEDTO savedetail2(ClgMasterCOEDTO categorypage)
        {
            return COMMM.POSTDataCOE(categorypage, "ClgCOEMasterFacade/savedetail2");
        }

        public ClgMasterCOEDTO deactivate1(ClgMasterCOEDTO categorypage)
        {
            return COMMM.POSTDataCOE(categorypage, "ClgCOEMasterFacade/deactivate1");
        }

        public ClgMasterCOEDTO deactivate2(ClgMasterCOEDTO categorypage)
        {
            return COMMM.POSTDataCOE(categorypage, "ClgCOEMasterFacade/deactivate2");
        }

        public ClgMasterCOEDTO getdetails(ClgMasterCOEDTO categorypage)
        {
            return COMMM.POSTDataCOE(categorypage, "ClgCOEMasterFacade/getdetails");
        }

        //public ClgMasterCOEDTO getdetails(int id)
        //{
        //    return COMMM.GetDataByIdCOE(id, "ClgCOEMasterFacade/getdetails/");
        //}

        public ClgMasterCOEDTO geteventdetails(ClgMasterCOEDTO categorypage)
        {
            return COMMM.POSTDataCOE(categorypage, "ClgCOEMasterFacade/geteventdetails");
        }


        public ClgMasterCOEDTO getalldetailsviewrecords1(int id)
        {
            return COMMM.GetDataByIdCOE(id, "ClgCOEMasterFacade/getalldetailsviewrecords1/");

        }

        public ClgMasterCOEDTO getalldetailsviewrecords2(int id)
        {
            return COMMM.GetDataByIdCOE(id, "ClgCOEMasterFacade/getalldetailsviewrecords2/");
        }

        public ClgMasterCOEDTO getpagedetails1(int id)
        {
            return COMMM.GetDataByIdCOE(id, "ClgCOEMasterFacade/getpagedetails1/");
        }

        public ClgMasterCOEDTO getpagedetails2(int id)
        {
            return COMMM.GetDataByIdCOE(id, "ClgCOEMasterFacade/getpagedetails2/");
        }
        public ClgMasterCOEDTO deleterec(int id)
        {
            return COMMM.DeleteClgCOE(id, "ClgCOEMasterFacade/deletedetails/");
        }
    }
}
