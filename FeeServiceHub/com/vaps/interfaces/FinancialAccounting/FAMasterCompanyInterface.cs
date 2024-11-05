using System;
using PreadmissionDTOs.com.vaps.Fees.FinancialAccounting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces.FinancialAccounting
{
    public interface FAMasterCompanyInterface
    {
        FAMasterCompanyDTO saveDetails(FAMasterCompanyDTO data);
        FAMasterCompanyDTO editDetails(int id);
        FAMasterCompanyDTO deleteDetails(FAMasterCompanyDTO data);
        FAMasterCompanyDTO Getdetails(FAMasterCompanyDTO data);

        FAUserComapnyMappingDTO GetCompany(FAUserComapnyMappingDTO data);

        FAUserComapnyMappingDTO saveUserDetails(FAUserComapnyMappingDTO data);

        FAUserComapnyMappingDTO editUserDetails(int id);

        FAUserComapnyMappingDTO deleteUserDetails(FAUserComapnyMappingDTO data);

        FACompanyFYMappingDTO GetInitialData(FACompanyFYMappingDTO data);

        FACompanyFYMappingDTO saveFYDetails(FACompanyFYMappingDTO data);

        FACompanyFYMappingDTO editFYDetails(int id);

        FACompanyFYMappingDTO deleteFYDetails(FACompanyFYMappingDTO data);
    }
}
