using CommonLibrary;
using PreadmissionDTOs.NAAC.Medical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Medical
{
    public class MC_Programs_112Delegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MC_Programs_112_DTO, MC_Programs_112_DTO> COMMM = new CommonDelegate<MC_Programs_112_DTO, MC_Programs_112_DTO>();

        public MC_Programs_112_DTO loaddata(MC_Programs_112_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "MC_Programs_112Facade/loaddata/");
        }
        public MC_Programs_112_DTO savedata(MC_Programs_112_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "MC_Programs_112Facade/savedata/");
        }
        public MC_Programs_112_DTO editdata(MC_Programs_112_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "MC_Programs_112Facade/editdata/");
        }
        public MC_Programs_112_DTO deactive_Y(MC_Programs_112_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "MC_Programs_112Facade/deactive_Y/");
        }
        public MC_Programs_112_DTO viewuploadflies(MC_Programs_112_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "MC_Programs_112Facade/viewuploadflies/");
        }
        public MC_Programs_112_DTO deleteuploadfile(MC_Programs_112_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "MC_Programs_112Facade/deleteuploadfile/");
        }
        public MC_Programs_112_DTO StaffList_Boss(MC_Programs_112_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "MC_Programs_112Facade/StaffList_Boss/");
        }
        public MC_Programs_112_DTO StaffList_Council(MC_Programs_112_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "MC_Programs_112Facade/StaffList_Council/");
        }
    }
}
