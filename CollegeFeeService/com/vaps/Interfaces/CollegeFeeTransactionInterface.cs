using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CollegeFeeService.com.vaps.interfaces
{
    public interface CollegeFeeTransactionInterface
    {
        CollegeFeeTransactionDTO getdata(CollegeFeeTransactionDTO data);
        CollegeFeeTransactionDTO getstuddet(CollegeFeeTransactionDTO data);

        CollegeFeeTransactionDTO getstuddetnew(CollegeFeeTransactionDTO data);
        CollegeFeeTransactionDTO getdatastuacad(CollegeFeeTransactionDTO data);
        CollegeFeeTransactionDTO getdatastuacadgrp(CollegeFeeTransactionDTO id);
        Task<CollegeFeeTransactionDTO> savedetails(CollegeFeeTransactionDTO data);
        CollegeFeeTransactionDTO delrec(CollegeFeeTransactionDTO data);
        Task<CollegeFeeTransactionDTO> printreceipt(CollegeFeeTransactionDTO data);
        CollegeFeeTransactionDTO duplicaterecept(CollegeFeeTransactionDTO data);

        CollegeFeeTransactionDTO get_grp_reptno(CollegeFeeTransactionDTO data);


        CollegeFeeTransactionDTO getsearchfilter(CollegeFeeTransactionDTO data);

        CollegeFeeTransactionDTO searching(CollegeFeeTransactionDTO data);

        CollegeFeeTransactionDTO edittra(CollegeFeeTransactionDTO data);

        Task<CollegeFeeTransactionDTO> printreceiptnew(CollegeFeeTransactionDTO data);

        CollegeFeeTransactionDTO Search_Chaln_No(CollegeFeeTransactionDTO data);
        CollegeFeeTransactionDTO Save_Chaln_No(CollegeFeeTransactionDTO data);
        CollegeFeeTransactionDTO dynamicfinecalculation(CollegeFeeTransactionDTO data);

        CollegeFeeTransactionDTO viewstatus(CollegeFeeTransactionDTO data);
    }
}
