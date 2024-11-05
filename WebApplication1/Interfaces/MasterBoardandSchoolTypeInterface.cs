using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
    public interface MasterBoardandSchoolTypeInterface
    {
        MasterBoardDTO savedet(MasterBoardDTO mb);
        MasterSchoolTypeDTO saveSchoolTypeDet(MasterSchoolTypeDTO st);
        MasterBoardDTO getAllDetails(int brdlist);
        MasterSchoolTypeDTO getAllSchoolTypeDetails(MasterSchoolTypeDTO stlist);
        MasterBoardDTO deleterec(int id);
        MasterSchoolTypeDTO deleteSchoolTyperec(int id);
        MasterBoardDTO getdetails(int id);
        MasterSchoolTypeDTO getSchoolTypedetails(int id);

    }
}
