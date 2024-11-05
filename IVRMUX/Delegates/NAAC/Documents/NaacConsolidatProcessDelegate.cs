using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using PreadmissionDTOs.NAAC.Documents;

namespace IVRMUX.Delegates.NAAC.Documents
{
    public class NaacConsolidatProcessDelegate
    {
        CommonDelegate<NaacConsolidatProcessDTO, NaacConsolidatProcessDTO> _comm = new CommonDelegate<NaacConsolidatProcessDTO, NaacConsolidatProcessDTO>();
        public NaacConsolidatProcessDTO onload(NaacConsolidatProcessDTO data)
        {
            return _comm.naacdetailsbypost(data, "NaacConsolidatProcessFacade/onload");
        }
        public NaacConsolidatProcessDTO search(NaacConsolidatProcessDTO data)
        {
            return _comm.naacdetailsbypost(data, "NaacConsolidatProcessFacade/search");
        }
        public NaacConsolidatProcessDTO getorganizationdata(NaacConsolidatProcessDTO data)
        {
            return _comm.naacdetailsbypost(data, "NaacConsolidatProcessFacade/getorganizationdata");
        }
        public NaacConsolidatProcessDTO onclickapproval(NaacConsolidatProcessDTO data)
        {
            return _comm.naacdetailsbypost(data, "NaacConsolidatProcessFacade/onclickapproval");
        }

        // AFFLIATED COLLEGE RELATED
        public NaacConsolidatProcessDTO getaffliateddata(NaacConsolidatProcessDTO data)
        {
            return _comm.naacdetailsbypost(data, "NaacConsolidatProcessFacade/getaffliateddata");
        }
        public NaacConsolidatProcessDTO savedatawisecomments(NaacConsolidatProcessDTO data)
        {
            return _comm.naacdetailsbypost(data, "NaacConsolidatProcessFacade/savedatawisecomments");
        }
        public NaacConsolidatProcessDTO viewdatawisecomments(NaacConsolidatProcessDTO data)
        {
            return _comm.naacdetailsbypost(data, "NaacConsolidatProcessFacade/viewdatawisecomments");
        }
        public NaacConsolidatProcessDTO savefilewisecomments(NaacConsolidatProcessDTO data)
        {
            return _comm.naacdetailsbypost(data, "NaacConsolidatProcessFacade/savefilewisecomments");
        }
        public NaacConsolidatProcessDTO viewfilewisecomments(NaacConsolidatProcessDTO data)
        {
            return _comm.naacdetailsbypost(data, "NaacConsolidatProcessFacade/viewfilewisecomments");
        }
        public NaacConsolidatProcessDTO approvedata(NaacConsolidatProcessDTO data)
        {
            return _comm.naacdetailsbypost(data, "NaacConsolidatProcessFacade/approvedata");
        }
        public NaacConsolidatProcessDTO approvedocuments(NaacConsolidatProcessDTO data)
        {
            return _comm.naacdetailsbypost(data, "NaacConsolidatProcessFacade/approvedocuments");
        }
        public NaacConsolidatProcessDTO getapproved(NaacConsolidatProcessDTO data)
        {
            return _comm.naacdetailsbypost(data, "NaacConsolidatProcessFacade/getapproved");
        }

        //****************** MEDICAL COLLEGE DATA ********************** //
        public NaacConsolidatProcessDTO getmedicalddata(NaacConsolidatProcessDTO data)
        {
            return _comm.naacdetailsbypost(data, "NaacConsolidatProcessFacade/getmedicalddata");
        }
        public NaacConsolidatProcessDTO getmedicalapproveddata(NaacConsolidatProcessDTO data)
        {
            return _comm.naacdetailsbypost(data, "NaacConsolidatProcessFacade/getmedicalapproveddata");
        }
        public NaacConsolidatProcessDTO savemedicaldatawisecomments(NaacConsolidatProcessDTO data)
        {
            return _comm.naacdetailsbypost(data, "NaacConsolidatProcessFacade/savemedicaldatawisecomments");
        }
        public NaacConsolidatProcessDTO viewmedicaldatawisecomments(NaacConsolidatProcessDTO data)
        {
            return _comm.naacdetailsbypost(data, "NaacConsolidatProcessFacade/viewmedicaldatawisecomments");
        }
        public NaacConsolidatProcessDTO approvemedicaldata(NaacConsolidatProcessDTO data)
        {
            return _comm.naacdetailsbypost(data, "NaacConsolidatProcessFacade/approvemedicaldata");
        }
        public NaacConsolidatProcessDTO savemedicalfilewisecomments(NaacConsolidatProcessDTO data)
        {
            return _comm.naacdetailsbypost(data, "NaacConsolidatProcessFacade/savemedicalfilewisecomments");
        }
        public NaacConsolidatProcessDTO viewmedicalfilewisecomments(NaacConsolidatProcessDTO data)
        {
            return _comm.naacdetailsbypost(data, "NaacConsolidatProcessFacade/viewmedicalfilewisecomments");
        }
        public NaacConsolidatProcessDTO approvemedicaldocuments(NaacConsolidatProcessDTO data)
        {
            return _comm.naacdetailsbypost(data, "NaacConsolidatProcessFacade/approvemedicaldocuments");
        }
    }
}
