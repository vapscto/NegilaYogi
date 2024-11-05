using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.College.Admission
{
    public class CategorySeatDistributionDelegates
    {

        CommonDelegate<CategorySeatDistributionDTO, CategorySeatDistributionDTO> _commbranch = new CommonDelegate<CategorySeatDistributionDTO, CategorySeatDistributionDTO>();

        public CategorySeatDistributionDTO getdetails(CategorySeatDistributionDTO data)
        {
            return _commbranch.clgadmissionbypost(data, "CategorySeatDistributionFacade/getdetails/");
        }
        public CategorySeatDistributionDTO onreport(CategorySeatDistributionDTO id)
        {
            return _commbranch.clgadmissionbypost(id, "CategorySeatDistributionFacade/onreport/");
        }

    }
}
