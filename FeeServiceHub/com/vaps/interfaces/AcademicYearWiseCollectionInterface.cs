using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace FeeServiceHub.com.vaps.interfaces
{
    public interface AcademicYearWiseCollectionInterface
    {
        CategoryWiseFeeCollectionDTO getdetails(CategoryWiseFeeCollectionDTO data);



        Task<CategoryWiseFeeCollectionDTO> radiobtndata(CategoryWiseFeeCollectionDTO temp);
        Task<CategoryWiseFeeCollectionDTO> onchangeacademic(CategoryWiseFeeCollectionDTO temp);
    }
}
