using CommonLibrary;
using PreadmissionDTOs.com.vaps.ClubManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.ClubManagement
{
    public class CMS_TransactionDelegate
    {
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<CMS_TransactionDTO, CMS_TransactionDTO> COMMM = new CommonDelegate<CMS_TransactionDTO, CMS_TransactionDTO>();

        CommonDelegate<CMS_TransactionDetailsDTO, CMS_TransactionDetailsDTO> COMMMT = new CommonDelegate<CMS_TransactionDetailsDTO, CMS_TransactionDetailsDTO>();
        public CMS_TransactionDTO loaddata(int id)
        {
            return COMMM.GetDataByClubManagement(id, "CMS_TransactionFacade/loaddata/");
        }
        //POSTDataClubManagement
        public CMS_TransactionDTO savedata(CMS_TransactionDTO data)
        {
            return COMMM.POSTDataClubManagement(data, "CMS_TransactionFacade/savedata/");
        }
        //deactive
        public CMS_TransactionDTO deactive(CMS_TransactionDTO data)
        {
            return COMMM.POSTDataClubManagement(data, "CMS_TransactionFacade/deactive/");
        }
        //edit
        public CMS_TransactionDTO edit(CMS_TransactionDTO data)
        {
            return COMMM.POSTDataClubManagement(data, "CMS_TransactionFacade/edit/");
        }
        //TrnasctionDetails
        public CMS_TransactionDetailsDTO loaddatatwo(int id)
        {
            return COMMMT.GetDataByClubManagement(id, "CMS_TransactionFacade/loaddatatwo/");
        }
        public CMS_TransactionDetailsDTO savedatatwo(CMS_TransactionDetailsDTO data)
        {
            return COMMMT.POSTDataClubManagement(data, "CMS_TransactionFacade/savedatatwo/");
        }
        //deactive
        public CMS_TransactionDetailsDTO deactivetwo(CMS_TransactionDetailsDTO data)
        {
            return COMMMT.POSTDataClubManagement(data, "CMS_TransactionFacade/deactivetwo/");
        }
        //edit
        public CMS_TransactionDetailsDTO edittwo(CMS_TransactionDetailsDTO data)
        {
            return COMMMT.POSTDataClubManagement(data, "CMS_TransactionFacade/edittwo/");
        }
    }
}
