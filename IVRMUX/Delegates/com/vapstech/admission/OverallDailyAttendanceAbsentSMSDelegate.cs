using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;

namespace corewebapi18072016.Delegates.com.vapstech.admission
{
    public class OverallDailyAttendanceAbsentSMSDelegate
    {
        CommonDelegate<OveralldailyattendanceabsentsmsDTO, OveralldailyattendanceabsentsmsDTO> comm = new CommonDelegate<OveralldailyattendanceabsentsmsDTO, OveralldailyattendanceabsentsmsDTO>();

        public OveralldailyattendanceabsentsmsDTO getinitialdata(OveralldailyattendanceabsentsmsDTO data)
        {
            return comm.POSTDataADM(data, "OveralldailyattendanceabsentsmsFacade/getinitialdata/");
        }

        public OveralldailyattendanceabsentsmsDTO getserdata(OveralldailyattendanceabsentsmsDTO data)
        {
            return comm.POSTDataADM(data, "OveralldailyattendanceabsentsmsFacade/getserdata/");
        }
        public OveralldailyattendanceabsentsmsDTO getstudentDet(OveralldailyattendanceabsentsmsDTO data)
        {
            return comm.POSTDataADM(data, "OveralldailyattendanceabsentsmsFacade/getstudentDet/");
        }
        public OveralldailyattendanceabsentsmsDTO sendsms(OveralldailyattendanceabsentsmsDTO data)
        {
            return comm.POSTDataADM(data, "OveralldailyattendanceabsentsmsFacade/sendsms/");
        }
        public OveralldailyattendanceabsentsmsDTO sendemail(OveralldailyattendanceabsentsmsDTO data)
        {
            return comm.POSTDataADM(data, "OveralldailyattendanceabsentsmsFacade/sendemail/");
        }
        public OveralldailyattendanceabsentsmsDTO smartcardatt(OveralldailyattendanceabsentsmsDTO data)
        {
            return comm.POSTDataADM(data, "OveralldailyattendanceabsentsmsFacade/smartcardatt/");
        }
        public OveralldailyattendanceabsentsmsDTO createuser(OveralldailyattendanceabsentsmsDTO data)
        {
            return comm.POSTDataADM(data, "OveralldailyattendanceabsentsmsFacade/createuser/");
        }
        

    }
}
