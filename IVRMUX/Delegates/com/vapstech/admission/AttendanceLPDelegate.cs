using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.admission;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using CommonLibrary;
namespace corewebapi18072016.Delegates.com.vaps.admission
{
    public class AttendanceLPDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<AttendanceLPDTO, AttendanceLPDTO> COMMM = new CommonDelegate<AttendanceLPDTO, AttendanceLPDTO>();

        public AttendanceLPDTO getDataBySelectedType(AttendanceLPDTO dto)
        {

            return COMMM.POSTDataADM(dto, "AttendanceLPFacede/getdatabyselectedtype/");
        }

        public AttendanceLPDTO SaveData(AttendanceLPDTO attdo)
        {
            return COMMM.POSTDataADM(attdo, "AttendanceLPFacede/savedata/");
        }
        public AttendanceLPDTO getyear(AttendanceLPDTO attdo)
        {
            return COMMM.POSTDataADM(attdo, "AttendanceLPFacede/getyear/");
        }
        
        public AttendanceLPDTO LoadInitialData(long MIID)
        {
            return COMMM.GetDataByIdADM(Convert.ToInt32(MIID), "AttendanceLPFacede/getinitialdata/");
        }

        public AttendanceLPDTO GetEditData(AttendanceLPDTO attdto)
        {
            return COMMM.POSTDataADM(attdto, "AttendanceLPFacede/geteditdata/");
        }
           public AttendanceLPDTO staffwisegrid(AttendanceLPDTO attdto)
        {
            return COMMM.POSTDataADM(attdto, "AttendanceLPFacede/staffwisegrid/");
        }
        public AttendanceLPDTO deleteRecord(AttendanceLPDTO attobj)
        {
            return COMMM.POSTDataADM(attobj, "AttendanceLPFacede/deleteRecord/");
        }

    }
}
