using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;

namespace WebApplication1.Interfaces
{
    public interface MasterPageInterface
    {
        MasterPageDTO saveorgdet(MasterPageDTO org);

        MasterPageDTO mobilesaveorgdet(MasterPageDTO org);
        MasterPageDTO deleterec(int id);

        MasterPageDTO mobiledeleterec(MasterPageDTO id);

        MasterPageDTO getdetails(int id);

        MasterPageDTO getalldetailsmobile(int id);
        MasterPageDTO getpageedit(int id);

        MasterPageDTO mobilegetdetails(int id);

        MasterPageDTO getsearchdata(int id, MasterPageDTO org);
    }
}
