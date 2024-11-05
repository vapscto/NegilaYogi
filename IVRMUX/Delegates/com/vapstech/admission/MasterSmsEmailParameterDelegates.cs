using System;
using System.Net.Http;
using PreadmissionDTOs.com.vaps.admission;
using CommonLibrary;

namespace corewebapi18072016.Delegates
{
    public class MasterSmsEmailParameterDelegates
    {

        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<MasterSmsEmailParameterDTO, MasterSmsEmailParameterDTO> COMMM = new CommonDelegate<MasterSmsEmailParameterDTO, MasterSmsEmailParameterDTO>();
     
        public MasterSmsEmailParameterDTO GetcastecategoryData(MasterSmsEmailParameterDTO lo)
        {
            return COMMM.GETDataADm(lo, "MasterSmsEmailParameterFacade/Getdetails");
          
        }

        public MasterSmsEmailParameterDTO edit(MasterSmsEmailParameterDTO data)
        {
            return COMMM.POSTDataADM(data, "MasterSmsEmailParameterFacade/edit/");

          
        }

        public MasterSmsEmailParameterDTO Savedata(MasterSmsEmailParameterDTO MasterSmsEmailParameterDTO)
        {
            return COMMM.POSTDataADM(MasterSmsEmailParameterDTO, "MasterSmsEmailParameterFacade/Savedata");


        
        }

        public MasterSmsEmailParameterDTO deletedata(int ID)
        {

            return COMMM.DeleteDataByIdADM(ID, "MasterSmsEmailParameterFacade/deletedata/");


        }

        ///HTMLTEMPLATE
        ///
        public MasterSmsEmailParameterDTO htmlGetcastecategoryData(MasterSmsEmailParameterDTO lo)
        {
            return COMMM.POSTDataADM(lo, "MasterSmsEmailParameterFacade/htmlGetdetails");

        }

        public MasterSmsEmailParameterDTO htmledit(MasterSmsEmailParameterDTO data)
        {
            return COMMM.POSTDataADM(data, "MasterSmsEmailParameterFacade/htmledit/");


        }

        public MasterSmsEmailParameterDTO htmlSavedata(MasterSmsEmailParameterDTO MasterSmsEmailParameterDTO)
        {
            return COMMM.POSTDataADM(MasterSmsEmailParameterDTO, "MasterSmsEmailParameterFacade/htmlSavedata");



        }

        public MasterSmsEmailParameterDTO htmldeletedata(MasterSmsEmailParameterDTO ID)
        {

            return COMMM.POSTDataADM(ID, "MasterSmsEmailParameterFacade/htmldeletedata/");


        }

    }
}
