using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
    public interface Institutioninterface
    {
        InstitutionDTO saveInstitute(InstitutionDTO InstitDTO);
        Task<InstitutionDTO> OnPageloadData(InstitutionDTO stu);
        StateDTO getStatedataByCountryID(int id);
        CityDTO getcity(int id);
        InstitutionDTO deleterec(int id);
        InstitutionDTO getdetails(int id);
        InstitutionDTO DuplicateData(InstitutionDTO InstitDTO);
        Master_Institution_SubscriptionValidityDTO SaveSubscriptionValidity(Master_Institution_SubscriptionValidityDTO InstitDTO);
        Master_Institution_SubscriptionValidityDTO deleteSubscriptionrec(int id);
        Task<InstitutionDTO> Institutionsearchdata(SortingPagingInfoDTO InstitDTO);
        Task<Master_Institution_SubscriptionValidityDTO> Subscriptionsearchdata(SortingPagingInfoDTO stu);
        InstitutionDTO OnClickSaveAutoMapping(InstitutionDTO stu);
    }
}