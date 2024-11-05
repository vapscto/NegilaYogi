using PreadmissionDTOs.NAAC.Admission.Criteria7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface.Criteria7
{
  public  interface LocalCommunityInterface
    {
        Task<LocalCommunityDTO> loaddata(LocalCommunityDTO data);
        LocalCommunityDTO getdata(LocalCommunityDTO data);
        LocalCommunityDTO savedatatab1(LocalCommunityDTO data);
        LocalCommunityDTO edittab1(LocalCommunityDTO data);
        LocalCommunityDTO deactivYTab1(LocalCommunityDTO data);
        LocalCommunityDTO deleteuploadfile(LocalCommunityDTO data);
        LocalCommunityDTO viewuploadflies(LocalCommunityDTO data);
        LocalCommunityDTO getcomment(LocalCommunityDTO data);
        LocalCommunityDTO getfilecomment(LocalCommunityDTO data);
        LocalCommunityDTO savecomments(LocalCommunityDTO data);
        LocalCommunityDTO savefilewisecomments(LocalCommunityDTO data);
    }
}
