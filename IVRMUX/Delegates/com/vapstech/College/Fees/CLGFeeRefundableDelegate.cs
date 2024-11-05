using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using PreadmissionDTOs.com.vaps.College.Fees;
using Newtonsoft.Json;
using System.Collections;
using PreadmissionDTOs.com.vaps.admission;
using CommonLibrary;

namespace corewebapi18072016.Delegates.com.vapstech.College.Fees
{
    public class CLGFeeRefundableDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<CLGFeeRefundableDTO, CLGFeeRefundableDTO> COMMM = new CommonDelegate<CLGFeeRefundableDTO, CLGFeeRefundableDTO>();
        public CLGFeeRefundableDTO getdata(CLGFeeRefundableDTO enqdto)
        {
            return COMMM.PostClgFee(enqdto, "CLGFeeRefundableFacade/getalldetails/");
        }

        public CLGFeeRefundableDTO getdatastuacadgrp(CLGFeeRefundableDTO enqdto)
        {
            return COMMM.PostClgFee(enqdto, "CLGFeeRefundableFacade/getstudlistgroup/");           
        }

        public CLGFeeRefundableDTO getdatastuacad(CLGFeeRefundableDTO enqdto)
        {
            return COMMM.PostClgFee(enqdto, "CLGFeeRefundableFacade/getacademicyear/");           
        }

        public CLGFeeRefundableDTO getstuddet(CLGFeeRefundableDTO pgmod)
        {
            return COMMM.PostClgFee(pgmod, "CLGFeeRefundableFacade/getgroupmappedheads/");           
        }

        public CLGFeeRefundableDTO savedetails(CLGFeeRefundableDTO pgmod)
        {
            return COMMM.PostClgFee(pgmod, "CLGFeeRefundableFacade/");
        }

        public CLGFeeRefundableDTO deleterec(CLGFeeRefundableDTO enqdto)
        {
            return COMMM.PostClgFee(enqdto, "CLGFeeRefundableFacade/Deletedetails/");           
        }
        public CLGFeeRefundableDTO getStudentdataByYear(CLGFeeRefundableDTO id)
        {
            return COMMM.PostClgFee(id, "CLGFeeRefundableFacade/getStudentdetailsByYear/");
        }

        public CLGFeeRefundableDTO GetSection(CLGFeeRefundableDTO Section)
        {
            return COMMM.PostClgFee(Section, "CLGFeeRefundableFacade/GetSection");            
        }
        public CLGFeeRefundableDTO get_semisters(CLGFeeRefundableDTO Section)
        {
            return COMMM.PostClgFee(Section, "CLGFeeRefundableFacade/get_semisters");
        }
        public CLGFeeRefundableDTO GetStudent(CLGFeeRefundableDTO Section)
        {
            return COMMM.PostClgFee(Section, "CLGFeeRefundableFacade/GetStudent");
        }

        public CLGFeeRefundableDTO GetStudentListByamst(CLGFeeRefundableDTO Section)
        {
            return COMMM.PostClgFee(Section, "CLGFeeRefundableFacade/GetStudentListByamst");
        }

        public CLGFeeRefundableDTO getdataclawisestude(CLGFeeRefundableDTO enqdto)
        {
            return COMMM.PostClgFee(enqdto, "CLGFeeRefundableFacade/editdetails/");           
        }

        public CLGFeeRefundableDTO getgroupheaddetails(CLGFeeRefundableDTO pgmod)
        {
             return COMMM.PostClgFee(pgmod, "CLGFeeRefundableFacade/onselectgroup/");            
        }


        public CLGFeeRefundableDTO getmodeofpaymentdata(CLGFeeRefundableDTO pgmod)
        {
            return COMMM.PostClgFee(pgmod, "CLGFeeRefundableFacade/modeofpayment/");            
        }
        public CLGFeeRefundableDTO searching(CLGFeeRefundableDTO id)
        {
            return COMMM.PostClgFee(id, "CLGFeeRefundableFacade/searching");
        }

    }
}
