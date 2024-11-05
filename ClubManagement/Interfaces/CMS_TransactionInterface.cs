using PreadmissionDTOs.com.vaps.ClubManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClubManagement.Interfaces
{
   public interface CMS_TransactionInterface
    {
        CMS_TransactionDTO loaddata(int dto);
        CMS_TransactionDTO savedata(CMS_TransactionDTO data);
       
        CMS_TransactionDTO deactive(CMS_TransactionDTO data);
    
        CMS_TransactionDTO edit(CMS_TransactionDTO data);

        //Transaction Details
        CMS_TransactionDetailsDTO loaddatatwo(int dto);
        CMS_TransactionDetailsDTO savedatatwo(CMS_TransactionDetailsDTO data);
        CMS_TransactionDetailsDTO deactivetwo(CMS_TransactionDetailsDTO data);
        CMS_TransactionDetailsDTO edittwo(CMS_TransactionDetailsDTO data);
    }
}
