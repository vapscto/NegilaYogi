using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Interfaces
{
    public interface ConcessionApprovalInterface
    {
        Preadmission_School_Registration_CatergoryDTO loadta(Preadmission_School_Registration_CatergoryDTO id);
        Preadmission_School_Registration_CatergoryDTO getstudentdetails(Preadmission_School_Registration_CatergoryDTO dta);
        Preadmission_School_Registration_CatergoryDTO oncheckgetstudentdetails(Preadmission_School_Registration_CatergoryDTO data);

        Preadmission_School_Registration_CatergoryDTO confirmdta(Preadmission_School_Registration_CatergoryDTO data);
    }
}
