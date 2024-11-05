using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class NaacExpenditure424Delegate
    {
        CommonDelegate<NaacExpenditure424DTO, NaacExpenditure424DTO> COMMM = new CommonDelegate<NaacExpenditure424DTO, NaacExpenditure424DTO>();
        public NaacExpenditure424DTO save(NaacExpenditure424DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacExpenditure424Facade/save");
        }
        public NaacExpenditure424DTO loaddata(NaacExpenditure424DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacExpenditure424Facade/loaddata");
        }
        public NaacExpenditure424DTO deactiveStudent(NaacExpenditure424DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacExpenditure424Facade/deactiveStudent");
        }
        public NaacExpenditure424DTO getcomment(NaacExpenditure424DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacExpenditure424Facade/getcomment");
        }
        public NaacExpenditure424DTO savefilewisecomments(NaacExpenditure424DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacExpenditure424Facade/savefilewisecomments");
        }
        public NaacExpenditure424DTO savemedicaldatawisecomments(NaacExpenditure424DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacExpenditure424Facade/savemedicaldatawisecomments");
        }
        public NaacExpenditure424DTO getfilecomment(NaacExpenditure424DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacExpenditure424Facade/getfilecomment");
        }
        public NaacExpenditure424DTO EditData(NaacExpenditure424DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacExpenditure424Facade/EditData");
        }
        public NaacExpenditure424DTO viewuploadflies(NaacExpenditure424DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacExpenditure424Facade/viewuploadflies");
        }
        public NaacExpenditure424DTO deleteuploadfile(NaacExpenditure424DTO data)
        {
            return COMMM.naacdetailsbypost(data, "NaacExpenditure424Facade/deleteuploadfile");
        }

    }
}
