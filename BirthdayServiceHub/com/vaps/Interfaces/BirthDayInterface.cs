﻿using PreadmissionDTOs.com.vaps.BirthDay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BirthdayServiceHub.com.vaps.Interfaces
{
   public interface BirthDayInterface
    {
        BirthDayDTO getdata(int id);
        BirthDayDTO getlistthree(BirthDayDTO stu);
        BirthDayDTO staflist(BirthDayDTO stu1);
        BirthDayDTO staflist1(BirthDayDTO stu1);
        void Check_SMS_Mail_Status(int stu1);
        BirthDayDTO Sendmsg(BirthDayDTO msg);
        BirthDayDTO QueryContact_ApiCall(BirthDayDTO msg);
        BirthDayDTO getReport(BirthDayDTO rpt);
        BirthDayDTO getEmailSMSCount(BirthDayDTO rpt);
        BirthDayDTO SearchByColumn(BirthDayDTO data);
        BirthDayDTO getmonthreport(BirthDayDTO rpt);

        //Task<string> sendWhatsAppCall(BirthDayDTO data);
        Task<string> sendWhatsAppCall(BirthDayDTO data);

        Task<BirthDayDTO> getstaffdetails(BirthDayDTO data);


        
        void SMS_Schedulers(int smsSch);
    }
}
