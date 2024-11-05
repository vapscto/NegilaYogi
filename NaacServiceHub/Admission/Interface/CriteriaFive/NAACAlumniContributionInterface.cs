using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
   public interface NAACAlumniContributionInterface
    {
        NAACAlumniContributionDTO loaddatahsu(NAACAlumniContributionDTO data);
        NAACAlumniContributionDTO loaddata(NAACAlumniContributionDTO data);
        NAACAlumniContributionDTO save(NAACAlumniContributionDTO data);
        NAACAlumniContributionDTO savehsu(NAACAlumniContributionDTO data);
        NAACAlumniContributionDTO deactiveStudent(NAACAlumniContributionDTO data);
        NAACAlumniContributionDTO EditData(NAACAlumniContributionDTO obj);
        NAACAlumniContributionDTO deleteuploadfile(NAACAlumniContributionDTO obj);
        NAACAlumniContributionDTO viewuploadflies(NAACAlumniContributionDTO obj);
        NAACAlumniContributionDTO savemedicaldatawisecomments(NAACAlumniContributionDTO obj);
        NAACAlumniContributionDTO getcomment(NAACAlumniContributionDTO obj);
        NAACAlumniContributionDTO getfilecomment(NAACAlumniContributionDTO obj);
        NAACAlumniContributionDTO savefilewisecomments(NAACAlumniContributionDTO obj);
    
}
}
