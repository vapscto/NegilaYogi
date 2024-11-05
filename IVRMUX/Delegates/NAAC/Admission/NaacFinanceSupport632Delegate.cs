using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class NaacFinanceSupport632Delegate
    {
        CommonDelegate<NAAC_AC_632_FinanceSupport_DTO, NAAC_AC_632_FinanceSupport_DTO> COMMM = new CommonDelegate<NAAC_AC_632_FinanceSupport_DTO, NAAC_AC_632_FinanceSupport_DTO>();
        public NAAC_AC_632_FinanceSupport_DTO loaddata(NAAC_AC_632_FinanceSupport_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacFinanceSupport632Facade/loaddata");
        }
        public NAAC_AC_632_FinanceSupport_DTO save(NAAC_AC_632_FinanceSupport_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacFinanceSupport632Facade/save");
        }
        public NAAC_AC_632_FinanceSupport_DTO deactive(NAAC_AC_632_FinanceSupport_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacFinanceSupport632Facade/deactive");
        }
        public NAAC_AC_632_FinanceSupport_DTO EditData(NAAC_AC_632_FinanceSupport_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacFinanceSupport632Facade/EditData");
        }
        public NAAC_AC_632_FinanceSupport_DTO viewuploadflies(NAAC_AC_632_FinanceSupport_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacFinanceSupport632Facade/viewuploadflies");
        }
        public NAAC_AC_632_FinanceSupport_DTO deleteuploadfile(NAAC_AC_632_FinanceSupport_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacFinanceSupport632Facade/deleteuploadfile");
        }


        public NAAC_AC_632_FinanceSupport_DTO savemedicaldatawisecomments(NAAC_AC_632_FinanceSupport_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacFinanceSupport632Facade/savemedicaldatawisecomments");
        }
        public NAAC_AC_632_FinanceSupport_DTO savefilewisecomments(NAAC_AC_632_FinanceSupport_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacFinanceSupport632Facade/savefilewisecomments");
        }
        public NAAC_AC_632_FinanceSupport_DTO getcomment(NAAC_AC_632_FinanceSupport_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacFinanceSupport632Facade/getcomment");
        }
        public NAAC_AC_632_FinanceSupport_DTO getfilecomment(NAAC_AC_632_FinanceSupport_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacFinanceSupport632Facade/getfilecomment");
        }


    }
}
