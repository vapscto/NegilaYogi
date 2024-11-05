using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.College.Preadmission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegePreadmission.Interfaces
{
   public interface TotalCountClgReportInterface
    {
        Task <CollegePreadmissionstudnetDto> Get_Intial_data(CollegePreadmissionstudnetDto data);

        Task <CollegePreadmissionstudnetDto> Getdetails(CollegePreadmissionstudnetDto data);

        //preadmission status

        Task<CommonDTO> getstatusdata(int id);
        CommonDTO getdataonsearchfilter(CommonDTO cdto);
        CommonDTO saveData(CommonDTO cdto);
        CollegePreadmissionstudnetDto Clgapplicationstudocs(CollegePreadmissionstudnetDto cdto);

        CollegePreadmissionstudnetDto Clgapplicationsturemarks(CollegePreadmissionstudnetDto cdto);

    }
}
