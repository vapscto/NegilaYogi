using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
    public interface NaacExpAcaFacility441Interface
    {
        NaacExpAcaFacility441DTO loaddata(NaacExpAcaFacility441DTO data);
        NaacExpAcaFacility441DTO save(NaacExpAcaFacility441DTO data);
        NaacExpAcaFacility441DTO deactiveStudent(NaacExpAcaFacility441DTO data);
        NaacExpAcaFacility441DTO getfilecomment(NaacExpAcaFacility441DTO data);
        NaacExpAcaFacility441DTO savefilewisecomments(NaacExpAcaFacility441DTO data);
        NaacExpAcaFacility441DTO savemedicaldatawisecomments(NaacExpAcaFacility441DTO data);
        NaacExpAcaFacility441DTO getcomment(NaacExpAcaFacility441DTO data);
        NaacExpAcaFacility441DTO EditData(NaacExpAcaFacility441DTO data);
        NaacExpAcaFacility441DTO viewuploadflies(NaacExpAcaFacility441DTO data);
        NaacExpAcaFacility441DTO deleteuploadfile(NaacExpAcaFacility441DTO data);
    }
}
