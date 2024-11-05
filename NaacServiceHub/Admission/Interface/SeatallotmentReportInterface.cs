using PreadmissionDTOs.NAAC.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.Admission.Interface
{
    public interface SeatallotmentReportInterface
    {
        SeatallotmentReportDTO getdetails (SeatallotmentReportDTO data);
        SeatallotmentReportDTO onreport(SeatallotmentReportDTO data);
    }
}
