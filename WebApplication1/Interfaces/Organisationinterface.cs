using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;


namespace WebApplication1.Interfaces
{
    public interface Organisationinterface
    {
        OrganisationDTO saveorgdet(OrganisationDTO org);
        Task<OrganisationDTO> countrydrp(OrganisationDTO stu);
        StateDTO enqdrpcountrydata(int id);
        CountryDTO getcity(int id);
        OrganisationDTO deleterec(int id);
        OrganisationDTO getdetails(int id);
        OrganisationDTO getcurrency(int id);
        OrganisationDTO getfilterdet(int filtype, OrganisationDTO orgdata);

        Task<OrganisationDTO> getorgSearchedDetails(SortingPagingInfoDTO data);
    }
}
