using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdmissionServiceHub.com.vaps.Interfaces
{
  public interface QRCode_Generation_Interface
    {

        QRCode_GenerationDTO Getdetails(QRCode_GenerationDTO data);
        QRCode_GenerationDTO SaveQR_Code(QRCode_GenerationDTO data);
        QRCode_GenerationDTO STAFFSaveQR_Code(QRCode_GenerationDTO data);
        QRCode_GenerationDTO get_classes(QRCode_GenerationDTO data);
        QRCode_GenerationDTO get_cls_sections(QRCode_GenerationDTO data);
        QRCode_GenerationDTO GetStudents(QRCode_GenerationDTO data);
        QRCode_GenerationDTO QRReportDetails(QRCode_GenerationDTO data);
        QRCode_GenerationDTO StaffGetdetails(QRCode_GenerationDTO data);




        QRCode_GenerationDTO getBasicData(QRCode_GenerationDTO dto);


        QRCode_GenerationDTO get_depts(QRCode_GenerationDTO dto);

        QRCode_GenerationDTO get_desig(QRCode_GenerationDTO dto);

        QRCode_GenerationDTO FilterEmployeedetailsBySelection(QRCode_GenerationDTO dto);
        QRCode_GenerationDTO QRcodegeneration(QRCode_GenerationDTO dto);
        QRCode_GenerationDTO StudentQRCode(QRCode_GenerationDTO dto);
    }
}
