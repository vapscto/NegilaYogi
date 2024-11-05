using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Interfaces
{

    public  interface SubjectwiseInterface
    {
        TT_SubjectwiseDTO savedetail(TT_SubjectwiseDTO objcategory);
        TT_SubjectwiseDTO getdetails(int id);
      

    }
}
