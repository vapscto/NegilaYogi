using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;

namespace WebApplication1.Interfaces
{
    public interface StaffLoginInterface
    {
        StaffLoginDTO getmoduledet(StaffLoginDTO data);
        Task<StaffLoginDTO> getpagedetails(StaffLoginDTO pgmod);

        Task<StaffLoginDTO> updateusername(StaffLoginDTO pgmod);
        Task<StaffLoginDTO> saveorgdet(StaffLoginDTO pgmod);
        StaffLoginDTO deleterec(StaffLoginDTO id);

        StaffLoginDTO searchfilter(StaffLoginDTO data);

        StaffLoginDTO getstudata(StaffLoginDTO sddto);

        Task<StaffLoginDTO> onchangeuser (StaffLoginDTO sddto);

        Task<StaffLoginDTO> multionchangeuser (StaffLoginDTO sddto);

        Task<StaffLoginDTO> multiuserdeletpages(StaffLoginDTO sddto);
        StaffLoginDTO getmoduleroledetails(int id);
        StaffLoginDTO changeinstitu(StaffLoginDTO data);
        StaffLoginDTO checkusernmedup(StaffLoginDTO orgdata);
        StaffLoginDTO getfilterdet(int filtype, StaffLoginDTO orgdata);

        StaffLoginDTO checktrustfunction(StaffLoginDTO orgdata);
        StaffLoginDTO getstaffmobilepages(StaffLoginDTO orgdata);
    }
}
