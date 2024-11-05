using PreadmissionDTOs.com.vaps.ClubManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClubManagement.Interfaces
{
  public  interface CMS_TrasanctionTypeInterface
    {
        CMS_TrasanctionTypeDTO Getdetails(int data);

        CMS_TrasanctionTypeDTO saveDetails(CMS_TrasanctionTypeDTO data);

        CMS_TrasanctionTypeDTO editDetails(CMS_TrasanctionTypeDTO id);

        CMS_TrasanctionTypeDTO deleteDetails(CMS_TrasanctionTypeDTO data);

        //inatallment
        CMS_TransactionsTypeInstallmentsDTO GetInitialData(int id);

        CMS_TransactionsTypeInstallmentsDTO editInsDetails(CMS_TransactionsTypeInstallmentsDTO id);

        CMS_TransactionsTypeInstallmentsDTO saveInsDetails(CMS_TransactionsTypeInstallmentsDTO data);

        CMS_TransactionsTypeInstallmentsDTO deleteInsDetails(CMS_TransactionsTypeInstallmentsDTO data);

        //transaction tax
        CMS_TransactionsTypeTaxDTO GetTaxInitialData(int id);
        CMS_TransactionsTypeTaxDTO deleteTaxDetails(CMS_TransactionsTypeTaxDTO data);

        CMS_TransactionsTypeTaxDTO editTaxDetails(CMS_TransactionsTypeTaxDTO id);
        CMS_TransactionsTypeTaxDTO saveTaxDetails(CMS_TransactionsTypeTaxDTO id);
    }
}
