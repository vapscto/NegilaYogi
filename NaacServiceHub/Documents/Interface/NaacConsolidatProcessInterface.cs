using PreadmissionDTOs.NAAC.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Documents.Interface
{
    public interface NaacConsolidatProcessInterface
    {
        NaacConsolidatProcessDTO onload(NaacConsolidatProcessDTO data);
        NaacConsolidatProcessDTO search(NaacConsolidatProcessDTO data);
        NaacConsolidatProcessDTO getorganizationdata(NaacConsolidatProcessDTO data);
        NaacConsolidatProcessDTO onclickapproval(NaacConsolidatProcessDTO data);

        //AFFLIATED COLLEGE RELATED 
        NaacConsolidatProcessDTO getaffliateddata(NaacConsolidatProcessDTO data);
        NaacConsolidatProcessDTO savedatawisecomments(NaacConsolidatProcessDTO data);
        NaacConsolidatProcessDTO viewdatawisecomments(NaacConsolidatProcessDTO data);
        NaacConsolidatProcessDTO savefilewisecomments(NaacConsolidatProcessDTO data);
        NaacConsolidatProcessDTO viewfilewisecomments(NaacConsolidatProcessDTO data);
        NaacConsolidatProcessDTO approvedata(NaacConsolidatProcessDTO data);
        NaacConsolidatProcessDTO approvedocuments(NaacConsolidatProcessDTO data);
        NaacConsolidatProcessDTO getapproved(NaacConsolidatProcessDTO data);

        // ************ MEDICAL COLLEGE DATA ****************** //
        NaacConsolidatProcessDTO getmedicalddata(NaacConsolidatProcessDTO data);
        NaacConsolidatProcessDTO getmedicalapproveddata(NaacConsolidatProcessDTO data);
        NaacConsolidatProcessDTO savemedicaldatawisecomments(NaacConsolidatProcessDTO data);
        NaacConsolidatProcessDTO viewmedicaldatawisecomments(NaacConsolidatProcessDTO data);
        NaacConsolidatProcessDTO approvemedicaldata(NaacConsolidatProcessDTO data);
        NaacConsolidatProcessDTO savemedicalfilewisecomments(NaacConsolidatProcessDTO data);
        NaacConsolidatProcessDTO viewmedicalfilewisecomments(NaacConsolidatProcessDTO data);
        NaacConsolidatProcessDTO approvemedicaldocuments(NaacConsolidatProcessDTO data);
    }
}
