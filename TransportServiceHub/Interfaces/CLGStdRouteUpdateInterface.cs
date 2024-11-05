using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Interfaces
{
   public interface CLGStdRouteUpdateInterface
    {

        CLGStdRouteUpdateDTO getloaddata(CLGStdRouteUpdateDTO data);

        CLGStdRouteUpdateDTO getloaddataintruction(CLGStdRouteUpdateDTO data);
        CLGStdRouteUpdateDTO getstudata(CLGStdRouteUpdateDTO sddto);
        Task<CLGStdRouteUpdateDTO> getbuspassdata(CLGStdRouteUpdateDTO sddto);

        Task<CLGStdRouteUpdateDTO> getbuspassdataupdate(CLGStdRouteUpdateDTO sddto);
        CLGStdRouteUpdateDTO getroutedata(CLGStdRouteUpdateDTO data);
        CLGStdRouteUpdateDTO getlocationdata(CLGStdRouteUpdateDTO data);
        CLGStdRouteUpdateDTO getlocationdataonly(CLGStdRouteUpdateDTO data);
        CLGStdRouteUpdateDTO savedata(CLGStdRouteUpdateDTO data);


        CLGStdRouteUpdateDTO paynow(CLGStdRouteUpdateDTO data);

        //PaymentDetails payuresponse(PaymentDetails stu);

        CLGStdRouteUpdateDTO searchfilter(CLGStdRouteUpdateDTO data);

    }
}
