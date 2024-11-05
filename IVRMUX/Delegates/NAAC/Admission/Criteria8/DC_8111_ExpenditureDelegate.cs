using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission.Criteria8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission.Criteria8
{
    public class DC_8111_ExpenditureDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<DC_8111_ExpenditureDTO, DC_8111_ExpenditureDTO> COMMM = new CommonDelegate<DC_8111_ExpenditureDTO, DC_8111_ExpenditureDTO>();

        public DC_8111_ExpenditureDTO loaddata(DC_8111_ExpenditureDTO data)
        {
            return COMMM.naacdetailsbypost(data, "DC_8111_ExpenditureFacade/loaddata/");
        }
        public DC_8111_ExpenditureDTO savedata(DC_8111_ExpenditureDTO data)
        {
            return COMMM.naacdetailsbypost(data, "DC_8111_ExpenditureFacade/savedata/");
        }
        public DC_8111_ExpenditureDTO editdata(DC_8111_ExpenditureDTO data)
        {
            return COMMM.naacdetailsbypost(data, "DC_8111_ExpenditureFacade/editdata/");
        }
        public DC_8111_ExpenditureDTO deactivY(DC_8111_ExpenditureDTO data)
        {
            return COMMM.naacdetailsbypost(data, "DC_8111_ExpenditureFacade/deactivY/");
        }
        public DC_8111_ExpenditureDTO viewuploadflies(DC_8111_ExpenditureDTO data)
        {
            return COMMM.naacdetailsbypost(data, "DC_8111_ExpenditureFacade/viewuploadflies");
        }
        public DC_8111_ExpenditureDTO deleteuploadfile(DC_8111_ExpenditureDTO data)
        {
            return COMMM.naacdetailsbypost(data, "DC_8111_ExpenditureFacade/deleteuploadfile");
        }
        public DC_8111_ExpenditureDTO getcomment(DC_8111_ExpenditureDTO data)
        {
            return COMMM.naacdetailsbypost(data, "DC_8111_ExpenditureFacade/getcomment");
        }
        public DC_8111_ExpenditureDTO getfilecomment(DC_8111_ExpenditureDTO data)
        {
            return COMMM.naacdetailsbypost(data, "DC_8111_ExpenditureFacade/getfilecomment");
        }
        public DC_8111_ExpenditureDTO savecomments(DC_8111_ExpenditureDTO data)
        {
            return COMMM.naacdetailsbypost(data, "DC_8111_ExpenditureFacade/savecomments");
        }
        public DC_8111_ExpenditureDTO savefilewisecomments(DC_8111_ExpenditureDTO data)
        {
            return COMMM.naacdetailsbypost(data, "DC_8111_ExpenditureFacade/savefilewisecomments");
        }
    }
}
