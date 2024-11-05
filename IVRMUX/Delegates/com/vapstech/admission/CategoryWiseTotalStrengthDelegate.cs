using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace corewebapi18072016.Delegates.com.vapstech.admission
{
    public class CategoryWiseTotalStrengthDelegate
    {
        private readonly object resource;
        private readonly string serviceBaseUrl;
        private const String JsonContentType = "application/json; charset=utf-8";
        CommonDelegate<CategoryWiseTotalStrengthDTO, CategoryWiseTotalStrengthDTO> comm = new CommonDelegate<CategoryWiseTotalStrengthDTO, CategoryWiseTotalStrengthDTO>();
        public CategoryWiseTotalStrengthDTO getdetails(CategoryWiseTotalStrengthDTO id)
        {
            return comm.POSTDataADM(id, "CategoryWiseTotalStrengthFacade/getdetails/");
        }
        public CategoryWiseTotalStrengthDTO Getreportdetails(CategoryWiseTotalStrengthDTO data)
        {
            return comm.POSTDataADM(data, "CategoryWiseTotalStrengthFacade/Getreportdetails/");
        }
    }

}
