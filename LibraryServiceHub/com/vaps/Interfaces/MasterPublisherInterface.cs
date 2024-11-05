using LibraryServiceHub.com.vaps.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Library;

namespace LibraryServiceHub.com.vaps.Interfaces
{
  public interface MasterPublisherInterface
    {
        MasterPublisherDTO Savedata(MasterPublisherDTO data);
        MasterPublisherDTO getdetails(int id);
        MasterPublisherDTO deactiveY(MasterPublisherDTO data);
    }
}
