using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface CategorySeatDistributionInterface
    {
        CategorySeatDistributionDTO getdetails(CategorySeatDistributionDTO data);
        CategorySeatDistributionDTO onreport(CategorySeatDistributionDTO data);
    }
}
