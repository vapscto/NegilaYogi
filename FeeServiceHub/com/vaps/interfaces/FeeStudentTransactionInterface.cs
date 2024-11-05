using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Fees;


namespace FeeServiceHub.com.vaps.interfaces
{
    public interface FeeStudentTransactionInterface
    {
        FeeStudentTransactionDTO getdata(FeeStudentTransactionDTO data);
        FeeStudentTransactionDTO getstuddet(FeeStudentTransactionDTO data);

        FeeStudentTransactionDTO getstuddetnew(FeeStudentTransactionDTO data);
        FeeStudentTransactionDTO getdatastuacad(FeeStudentTransactionDTO data);
        FeeStudentTransactionDTO getdatastuacadgrp(FeeStudentTransactionDTO id);
        Task<FeeStudentTransactionDTO> savedetails(FeeStudentTransactionDTO data);
        FeeStudentTransactionDTO delrec(FeeStudentTransactionDTO data);
        Task<FeeStudentTransactionDTO> printreceipt(FeeStudentTransactionDTO data);
        FeeStudentTransactionDTO duplicaterecept(FeeStudentTransactionDTO data);

        FeeStudentTransactionDTO get_grp_reptno(FeeStudentTransactionDTO data);


        FeeStudentTransactionDTO getsearchfilter(FeeStudentTransactionDTO data);

        FeeStudentTransactionDTO searching(FeeStudentTransactionDTO data);

        FeeStudentTransactionDTO edittra(FeeStudentTransactionDTO data);

        Task<FeeStudentTransactionDTO> printreceiptnew(FeeStudentTransactionDTO data);

        FeeStudentTransactionDTO Search_Chaln_No(FeeStudentTransactionDTO data);
        FeeStudentTransactionDTO Save_Chaln_No(FeeStudentTransactionDTO data);

        FeeStudentTransactionDTO SendEmail(FeeStudentTransactionDTO data);

        FeeStudentTransactionDTO getduedates(FeeStudentTransactionDTO data);
        FeeStudentTransactionDTO getheadwisedetails(FeeStudentTransactionDTO data);

        FeeStudentTransactionDTO viewstatus(FeeStudentTransactionDTO data);
        FeeStudentTransactionDTO viewpaydetails(FeeStudentTransactionDTO data);
        FeeStudentTransactionDTO viewpayexcessdetails(FeeStudentTransactionDTO data);

        FeeStudentTransactionDTO OBTransfer(FeeStudentTransactionDTO data);

        //Rebate Apply
        Task<FeeStudentTransactionDTO> Rebateapplyandsave(FeeStudentTransactionDTO data);

        FeeStudentTransactionDTO rebateamountcalc(FeeStudentTransactionDTO data);
        FeeStudentTransactionDTO Readminssioninsertionfees(FeeStudentTransactionDTO data);

    }
}
