using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface ClassCategoryReportInterface
    {
        ClassCategoryReportDTO getInitailData(int id);
        ClassCategoryReportDTO SearchData(ClassCategoryReportDTO Clscatag);
    }
}
