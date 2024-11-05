using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
  public  interface CLGMasterSemisterInterface
    {
        CLGMasterSemisterDTO activedeactivesem(CLGMasterSemisterDTO data);
        CLGMasterSemisterDTO savesem(CLGMasterSemisterDTO data);
        CLGMasterSemisterDTO editsem(CLGMasterSemisterDTO data);
        CLGMasterSemisterDTO getdata(CLGMasterSemisterDTO data);
        CLGMasterSemisterDTO getOrder(CLGMasterSemisterDTO data);
        
    }
}
