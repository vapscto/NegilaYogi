using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Interfaces
{
   public interface UserClassLibraryInterface
    {

        LIB_Library_Class_DTO getdetails(int id);

    }
}
