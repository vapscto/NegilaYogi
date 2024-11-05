using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.IssueManager.PettyCash;
using PreadmissionDTOs.com.vaps.VMS.Training;

namespace IVRMUX.Delegates.com.vapstech.VMS.Training
{
    public class IVRTM_Training_Delegate
    {
        CommonDelegate<IVRTM_TrainingDTO, IVRTM_TrainingDTO> _com = new CommonDelegate<IVRTM_TrainingDTO, IVRTM_TrainingDTO>();

        public IVRTM_TrainingDTO onloaddata(IVRTM_TrainingDTO data)
        {
            return _com.POSTVMS(data, "IVRTM_TrainingFacade/onloaddata");
        }

        public IVRTM_TrainingDTO saverecord(IVRTM_TrainingDTO data)
        {
            return _com.POSTVMS(data, "IVRTM_TrainingFacade/saverecord");
        }
        public IVRTM_TrainingDTO deactiveY(IVRTM_TrainingDTO data)
        {
            return _com.POSTVMS(data, "IVRTM_TrainingFacade/deactiveY");
        }
        public IVRTM_TrainingDTO Edit(IVRTM_TrainingDTO data)
        {
            return _com.POSTVMS(data, "IVRTM_TrainingFacade/Edit");
        }
        public IVRTM_TrainingDTO gettrainer(IVRTM_TrainingDTO data)
        {
            return _com.POSTVMS(data, "IVRTM_TrainingFacade/gettrainer");
        }


        public IVRTM_TrainingDTO onloaddataRequest(IVRTM_TrainingDTO data)
        {
            return _com.POSTVMS(data, "IVRTM_TrainingFacade/onloaddataRequest");
        }
        
        public IVRTM_TrainingDTO saveData(IVRTM_TrainingDTO data)
        {
            return _com.POSTVMS(data, "IVRTM_TrainingFacade/saveData");
        }
        public IVRTM_TrainingDTO trainerfeedback(IVRTM_TrainingDTO data)
        {
            return _com.POSTVMS(data, "IVRTM_TrainingFacade/trainerfeedback");
        }


        /////////////////////////////IVRM_Training_Assigning/////////////////////////////////////////////////////////////////////////////
        public IVRTM_TrainingDTO assignonload(IVRTM_TrainingDTO data)
        {
            return _com.POSTVMS(data, "IVRTM_TrainingFacade/assignonload");
        }
        public IVRTM_TrainingDTO EditDetails(IVRTM_TrainingDTO data)
        {
            return _com.POSTVMS(data, "IVRTM_TrainingFacade/EditDetails");
        }
        public IVRTM_TrainingDTO saveassign(IVRTM_TrainingDTO data)
        {
            return _com.POSTVMS(data, "IVRTM_TrainingFacade/saveassign");
        }
        ///////////////=================report====================////////////////////////////////////////////

        public IVRTM_TrainingDTO onloaddatareport(IVRTM_TrainingDTO data)
        {
            return _com.POSTVMS(data, "IVRTM_TrainingFacade/onloaddatareport");
        }
        public IVRTM_TrainingDTO getreport(IVRTM_TrainingDTO data)
        {
            return _com.POSTVMS(data, "IVRTM_TrainingFacade/getreport");
        }


    }
}
