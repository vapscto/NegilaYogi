using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class NAACAlumniContributionDelegate
    {
        CommonDelegate<NAACAlumniContributionDTO, NAACAlumniContributionDTO> comm = new CommonDelegate<NAACAlumniContributionDTO, NAACAlumniContributionDTO>();
        public NAACAlumniContributionDTO loaddatahsu(NAACAlumniContributionDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACAlumniContributionFacade/loaddatahsu");
        }
        public NAACAlumniContributionDTO loaddata(NAACAlumniContributionDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACAlumniContributionFacade/loaddata");
        }
        public NAACAlumniContributionDTO save(NAACAlumniContributionDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACAlumniContributionFacade/save");
        }
        public NAACAlumniContributionDTO savehsu(NAACAlumniContributionDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACAlumniContributionFacade/savehsu");
        }
        public NAACAlumniContributionDTO deactiveStudent(NAACAlumniContributionDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACAlumniContributionFacade/deactiveStudent");
        }

        public NAACAlumniContributionDTO EditData(NAACAlumniContributionDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACAlumniContributionFacade/EditData");
        }
        public NAACAlumniContributionDTO viewuploadflies(NAACAlumniContributionDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACAlumniContributionFacade/viewuploadflies");
        }
        public NAACAlumniContributionDTO deleteuploadfile(NAACAlumniContributionDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACAlumniContributionFacade/deleteuploadfile");
        }

        public NAACAlumniContributionDTO savemedicaldatawisecomments(NAACAlumniContributionDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACAlumniContributionFacade/savemedicaldatawisecomments");
        }
        public NAACAlumniContributionDTO getcomment(NAACAlumniContributionDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACAlumniContributionFacade/getcomment");
        }
        public NAACAlumniContributionDTO getfilecomment(NAACAlumniContributionDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACAlumniContributionFacade/getfilecomment");
        }
        public NAACAlumniContributionDTO savefilewisecomments(NAACAlumniContributionDTO data)
        {
            return comm.naacdetailsbypost(data, "NAACAlumniContributionFacade/savefilewisecomments");
        }
    }
}
