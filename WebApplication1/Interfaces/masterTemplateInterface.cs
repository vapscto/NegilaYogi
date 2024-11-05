using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
     public  interface masterTemplateInterface
    {
        MasterTemplateDTO saveTempldet(MasterTemplateDTO tmpl);
        MasterTemplateDTO getAllDetails(MasterTemplateDTO stu);
        MasterTemplateDTO getSaletypes(int stu);

        MasterTemplateDTO deleterec(int id);
        MasterTemplateDTO getdetails(int id);
        SatRegistrationDTO satregistration(SatRegistrationDTO dta);
    }
}
