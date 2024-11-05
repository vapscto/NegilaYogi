using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission.Criteria7
{
    public class NaacCodeOfCoduct7112Delegate
    {
        CommonDelegate<NAAC_AC_7112_CodeOfCoduct_DTO, NAAC_AC_7112_CodeOfCoduct_DTO> comm = new CommonDelegate<NAAC_AC_7112_CodeOfCoduct_DTO, NAAC_AC_7112_CodeOfCoduct_DTO>();
        public NAAC_AC_7112_CodeOfCoduct_DTO loaddata(NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCodeOfCoduct7112Facade/loaddata");
        }
        public NAAC_AC_7112_CodeOfCoduct_DTO save(NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCodeOfCoduct7112Facade/save");
        }
        public NAAC_AC_7112_CodeOfCoduct_DTO getcomment(NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCodeOfCoduct7112Facade/getcomment");
        }
        public NAAC_AC_7112_CodeOfCoduct_DTO savefilewisecomments(NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCodeOfCoduct7112Facade/savefilewisecomments");
        }
        public NAAC_AC_7112_CodeOfCoduct_DTO getfilecomment(NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCodeOfCoduct7112Facade/getfilecomment");
        }
        public NAAC_AC_7112_CodeOfCoduct_DTO savemedicaldatawisecomments(NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCodeOfCoduct7112Facade/savemedicaldatawisecomments");
        }
        public NAAC_AC_7112_CodeOfCoduct_DTO deactivate(NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCodeOfCoduct7112Facade/deactivate");
        }
        public NAAC_AC_7112_CodeOfCoduct_DTO EditData(NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCodeOfCoduct7112Facade/EditData");
        }
        public NAAC_AC_7112_CodeOfCoduct_DTO viewuploadflies(NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCodeOfCoduct7112Facade/viewuploadflies");
        }
        public NAAC_AC_7112_CodeOfCoduct_DTO deleteuploadfile(NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCodeOfCoduct7112Facade/deleteuploadfile");
        }

        public NAAC_AC_7112_CodeOfCoduct_DTO getData(NAAC_AC_7112_CodeOfCoduct_DTO data)
        {
            return comm.naacdetailsbypost(data, "NaacCodeOfCoduct7112Facade/getData");
        }
    }
}
