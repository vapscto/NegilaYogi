using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.OnlineProgram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.OnlineProgram.Interfaces
{
    public interface GuestDetailsInterface
    {
        OnlineProgramDTO getloaddata(OnlineProgramDTO data);
        OnlineProgramDTO savedetail(OnlineProgramDTO data);

        OnlineProgramDTO getalldetailsviewrecords(OnlineProgramDTO data);

        OnlineProgramDTO getdetails(OnlineProgramDTO data);

        OnlineProgramDTO deleterecord(OnlineProgramDTO data);
    }
}
