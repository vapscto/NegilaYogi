using PreadmissionDTOs.NAAC.University;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.University.Interface
{
   public interface NAAC_HSU_StudentComplaints252Interface
    {
        NAAC_HSU_StudentComplaints252_DTO loaddata(NAAC_HSU_StudentComplaints252_DTO data);
        NAAC_HSU_StudentComplaints252_DTO save(NAAC_HSU_StudentComplaints252_DTO data);
        NAAC_HSU_StudentComplaints252_DTO deactive(NAAC_HSU_StudentComplaints252_DTO data);
        NAAC_HSU_StudentComplaints252_DTO EditData(NAAC_HSU_StudentComplaints252_DTO data);
        NAAC_HSU_StudentComplaints252_DTO viewuploadflies(NAAC_HSU_StudentComplaints252_DTO data);
        NAAC_HSU_StudentComplaints252_DTO deleteuploadfile(NAAC_HSU_StudentComplaints252_DTO obj);

    }
}
