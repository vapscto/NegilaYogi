using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.ClubManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.ClubManagement
{
    public class CMS_TransactionTypeDelegate
    {

        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<CMS_TrasanctionTypeDTO, CMS_TrasanctionTypeDTO> COMMM = new CommonDelegate<CMS_TrasanctionTypeDTO, CMS_TrasanctionTypeDTO>();
        CommonDelegate<CMS_TransactionsTypeInstallmentsDTO, CMS_TransactionsTypeInstallmentsDTO> COMMMR = new CommonDelegate<CMS_TransactionsTypeInstallmentsDTO, CMS_TransactionsTypeInstallmentsDTO>();
        
              CommonDelegate<CMS_TransactionsTypeTaxDTO, CMS_TransactionsTypeTaxDTO> COMMMT = new CommonDelegate<CMS_TransactionsTypeTaxDTO, CMS_TransactionsTypeTaxDTO>();

        public CMS_TrasanctionTypeDTO Getdetails(int id)
        {
            return COMMM.GetDataByClubManagement(id, "CMS_TrasanctionTypeFacade/GetDetails/");
        }
        //POSTDataClubManagement
        public CMS_TrasanctionTypeDTO saveDetails(CMS_TrasanctionTypeDTO data)
        {
            return COMMM.POSTDataClubManagement(data, "CMS_TrasanctionTypeFacade/saveDetails/");
        }
        //deactive
        
        public CMS_TrasanctionTypeDTO editDetails(CMS_TrasanctionTypeDTO data)
        {
            return COMMM.POSTDataClubManagement(data, "CMS_TrasanctionTypeFacade/editDetails/");
        }
        public CMS_TrasanctionTypeDTO deleteDetails(CMS_TrasanctionTypeDTO data)
        {
            return COMMM.POSTDataClubManagement(data, "CMS_TrasanctionTypeFacade/deleteDetails/");
        }
        //intallment
        public CMS_TransactionsTypeInstallmentsDTO GetInitialData(int id)
        {
            return COMMMR.GetDataByClubManagement(id, "CMS_TrasanctionTypeFacade/GetInitialData/");
        }

        public CMS_TransactionsTypeInstallmentsDTO editInsDetails(CMS_TransactionsTypeInstallmentsDTO data)
        {
            return COMMMR.POSTDataClubManagement(data, "CMS_TrasanctionTypeFacade/editInsDetails/");
        }
        public CMS_TransactionsTypeInstallmentsDTO saveInsDetails(CMS_TransactionsTypeInstallmentsDTO data)
        {
            return COMMMR.POSTDataClubManagement(data, "CMS_TrasanctionTypeFacade/saveInsDetails/");
        }

        public CMS_TransactionsTypeInstallmentsDTO deleteInsDetails(CMS_TransactionsTypeInstallmentsDTO data)
        {
            return COMMMR.POSTDataClubManagement(data, "CMS_TrasanctionTypeFacade/deleteInsDetails/");
        }
        //transaction tax
        public CMS_TransactionsTypeTaxDTO GetTaxInitialData(int id)
        {
            return COMMMT.GetDataByClubManagement(id, "CMS_TrasanctionTypeFacade/GetTaxInitialData/");
        }
        public CMS_TransactionsTypeTaxDTO deleteTaxDetails(CMS_TransactionsTypeTaxDTO data)
        {
            return COMMMT.POSTDataClubManagement(data, "CMS_TrasanctionTypeFacade/deleteTaxDetails/");
        }

        public CMS_TransactionsTypeTaxDTO editTaxDetails(CMS_TransactionsTypeTaxDTO data)
        {
            return COMMMT.POSTDataClubManagement(data, "CMS_TrasanctionTypeFacade/editTaxDetails/");
        }

        public CMS_TransactionsTypeTaxDTO saveTaxDetails(CMS_TransactionsTypeTaxDTO data)
        {
            return COMMMT.POSTDataClubManagement(data, "CMS_TrasanctionTypeFacade/saveTaxDetails/");
        }
    }
}


