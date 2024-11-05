using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class Naac_MOUDelegate
    {
        CommonDelegate<Naac_MOU_DTO, Naac_MOU_DTO> comm = new CommonDelegate<Naac_MOU_DTO, Naac_MOU_DTO>();
        public Naac_MOU_DTO loaddata(Naac_MOU_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_MOUFacade/loaddata");
        }
        public Naac_MOU_DTO save(Naac_MOU_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_MOUFacade/save");
        }
        public Naac_MOU_DTO getcomment(Naac_MOU_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_MOUFacade/getcomment");
        }
        public Naac_MOU_DTO getfilecomment(Naac_MOU_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_MOUFacade/getfilecomment");
        }
        public Naac_MOU_DTO savemedicaldatawisecomments(Naac_MOU_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_MOUFacade/savemedicaldatawisecomments");
        }
        public Naac_MOU_DTO savefilewisecomments(Naac_MOU_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_MOUFacade/savefilewisecomments");
        }
        public Naac_MOU_DTO deactiveStudent(Naac_MOU_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_MOUFacade/deactiveStudent");
        }
        public Naac_MOU_DTO viewuploadflies(Naac_MOU_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_MOUFacade/viewuploadflies");
        }
        public Naac_MOU_DTO deleteuploadfile(Naac_MOU_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_MOUFacade/deleteuploadfile");
        }
        public Naac_MOU_DTO EditData(Naac_MOU_DTO data)
        {
            return comm.naacdetailsbypost(data, "Naac_MOUFacade/EditData");
        }
    }
}
