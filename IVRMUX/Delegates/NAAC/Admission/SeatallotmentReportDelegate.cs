using CommonLibrary;
using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.NAAC.Admission
{
    public class SeatallotmentReportDelegate
    {
        CommonDelegate<SeatallotmentReportDTO, SeatallotmentReportDTO> _comm = new CommonDelegate<SeatallotmentReportDTO, SeatallotmentReportDTO>();

        public SeatallotmentReportDTO getdetails(SeatallotmentReportDTO data)
        {
            return _comm.naacdetailsbypost(data, "SeatallotmentReportFacade/getdetails");
        }
        public SeatallotmentReportDTO onreport(SeatallotmentReportDTO data)
        {
            return _comm.naacdetailsbypost(data, "SeatallotmentReportFacade/onreport");
        }
    }
}
