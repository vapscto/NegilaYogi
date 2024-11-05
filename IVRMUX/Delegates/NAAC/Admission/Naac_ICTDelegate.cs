using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class Naac_ICTDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<Naac_ICT_DTO, Naac_ICT_DTO> COMMM = new CommonDelegate<Naac_ICT_DTO, Naac_ICT_DTO>();
        public Naac_ICT_DTO loaddata(Naac_ICT_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_ICTfacade/loaddata/");
        }
        public Naac_ICT_DTO savedata(Naac_ICT_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_ICTfacade/savedata");
        }
        public Naac_ICT_DTO savefilewisecomments(Naac_ICT_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_ICTfacade/savefilewisecomments");
        }
        public Naac_ICT_DTO savemedicaldatawisecomments(Naac_ICT_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_ICTfacade/savemedicaldatawisecomments");
        }
        public Naac_ICT_DTO getfilecomment(Naac_ICT_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_ICTfacade/getfilecomment");
        }
        public Naac_ICT_DTO getcomment(Naac_ICT_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_ICTfacade/getcomment");
        }
        public Naac_ICT_DTO editdata(Naac_ICT_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_ICTfacade/editdata");
        }
        public Naac_ICT_DTO deactivRow(Naac_ICT_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_ICTfacade/deactivRow");
        }
        public Naac_ICT_DTO viewuploadflies(Naac_ICT_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_ICTfacade/viewuploadflies");
        }
        public Naac_ICT_DTO deleteuploadfile(Naac_ICT_DTO data)
        {
            return COMMM.naacdetailsbypost(data, "Naac_ICTfacade/deleteuploadfile");
        }
    }
}
