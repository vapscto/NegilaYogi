using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
    public interface ClientWise_Module_Feature_Interface
    {
        ClientWise_Module_Feature_DTO getmodule(ClientWise_Module_Feature_DTO data);
        ClientWise_Module_Feature_DTO getreport(ClientWise_Module_Feature_DTO data);
    }
}
