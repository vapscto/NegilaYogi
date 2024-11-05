using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission.Criteria8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission.Criteria8
{
    public class NC_818_EmpCommitteesDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<NC_818_EmpCommitteesDTO, NC_818_EmpCommitteesDTO> COMMM = new CommonDelegate<NC_818_EmpCommitteesDTO, NC_818_EmpCommitteesDTO>();

        public NC_818_EmpCommitteesDTO loaddata(NC_818_EmpCommitteesDTO data)
        {
            return COMMM.naacdetailsbypost(data, "NC_818_EmpCommitteesFacade/loaddata/");
        }
        public NC_818_EmpCommitteesDTO savedata(NC_818_EmpCommitteesDTO data)
        {
            return COMMM.naacdetailsbypost(data, "NC_818_EmpCommitteesFacade/savedata/");
        }
        public NC_818_EmpCommitteesDTO editdata(NC_818_EmpCommitteesDTO data)
        {
            return COMMM.naacdetailsbypost(data, "NC_818_EmpCommitteesFacade/editdata/");
        }
        public NC_818_EmpCommitteesDTO deactivY(NC_818_EmpCommitteesDTO data)
        {
            return COMMM.naacdetailsbypost(data, "NC_818_EmpCommitteesFacade/deactivY/");
        }
        public NC_818_EmpCommitteesDTO viewuploadflies(NC_818_EmpCommitteesDTO data)
        {
            return COMMM.naacdetailsbypost(data, "NC_818_EmpCommitteesFacade/viewuploadflies");
        }
        public NC_818_EmpCommitteesDTO deleteuploadfile(NC_818_EmpCommitteesDTO data)
        {
            return COMMM.naacdetailsbypost(data, "NC_818_EmpCommitteesFacade/deleteuploadfile");
        }
        public NC_818_EmpCommitteesDTO getcomment(NC_818_EmpCommitteesDTO data)
        {
            return COMMM.naacdetailsbypost(data, "NC_818_EmpCommitteesFacade/getcomment");
        }
        public NC_818_EmpCommitteesDTO getfilecomment(NC_818_EmpCommitteesDTO data)
        {
            return COMMM.naacdetailsbypost(data, "NC_818_EmpCommitteesFacade/getfilecomment");
        }
        public NC_818_EmpCommitteesDTO savecomments(NC_818_EmpCommitteesDTO data)
        {
            return COMMM.naacdetailsbypost(data, "NC_818_EmpCommitteesFacade/savecomments");
        }
        public NC_818_EmpCommitteesDTO savefilewisecomments(NC_818_EmpCommitteesDTO data)
        {
            return COMMM.naacdetailsbypost(data, "NC_818_EmpCommitteesFacade/savefilewisecomments");
        }
    }
}
