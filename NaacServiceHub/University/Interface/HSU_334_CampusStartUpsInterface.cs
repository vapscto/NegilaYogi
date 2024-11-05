using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.University.Interface
{
  public  interface HSU_334_CampusStartUpsInterface
    {
        HSU_334_CampusStartUpsDTO loaddata(HSU_334_CampusStartUpsDTO data);
        HSU_334_CampusStartUpsDTO save(HSU_334_CampusStartUpsDTO data);
        HSU_334_CampusStartUpsDTO deactive(HSU_334_CampusStartUpsDTO data);
        HSU_334_CampusStartUpsDTO EditData(HSU_334_CampusStartUpsDTO data);
        HSU_334_CampusStartUpsDTO viewuploadflies(HSU_334_CampusStartUpsDTO data);
        HSU_334_CampusStartUpsDTO deleteuploadfile(HSU_334_CampusStartUpsDTO data);
    }
}
