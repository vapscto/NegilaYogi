using CommonLibrary;
using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.College.Fees
{
    public class ColStudentConcessionReportDelegate
    {
        private const String JsonContentType = "application/json;charset=utf-8";

        CommonDelegate<CollegeConcessionDTO, CollegeConcessionDTO> COMMM = new CommonDelegate<CollegeConcessionDTO, CollegeConcessionDTO>();
        public CollegeConcessionDTO getalldetails(CollegeConcessionDTO data)
        {

            return COMMM.POSTDataCollfee(data, "ColStudentConcessionReportFacade/getalldetails/");

        }


    }
}
