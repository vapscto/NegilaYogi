using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
    public interface InstitutionUserMappingInterface
    {
        InstitutionUserMappingDTO loaddata(InstitutionUserMappingDTO data);
        InstitutionUserMappingDTO onchangeinst(InstitutionUserMappingDTO data);
        InstitutionUserMappingDTO savedetails(InstitutionUserMappingDTO data);
        InstitutionUserMappingDTO viewdetails(InstitutionUserMappingDTO data);

        InstitutionUserMappingDTO savepaymentremarks(InstitutionUserMappingDTO data);
        InstitutionUserMappingDTO getvmspaymentsubsctiptionreport(InstitutionUserMappingDTO data);
    }
}
