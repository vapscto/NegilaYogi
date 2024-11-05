using CommonLibrary;
using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.admission
{
    public class QRCode_GenerationDelegate
    {
        CommonDelegate<QRCode_GenerationDTO, QRCode_GenerationDTO> COMM = new CommonDelegate<QRCode_GenerationDTO, QRCode_GenerationDTO>();


        public QRCode_GenerationDTO Getdetails(QRCode_GenerationDTO data)
        {
            return COMM.POSTDataaADM(data, "QRCode_GenerationFacade/Getdetails/");
        }

        public QRCode_GenerationDTO SaveQR_Code(QRCode_GenerationDTO data)
        {
            return COMM.POSTDataaADM(data, "QRCode_GenerationFacade/SaveQR_Code/");
        }
        public QRCode_GenerationDTO STAFFSaveQR_Code(QRCode_GenerationDTO data)
        {
            return COMM.POSTDataaADM(data, "QRCode_GenerationFacade/STAFFSaveQR_Code/");
        }
        public QRCode_GenerationDTO get_classes(QRCode_GenerationDTO data)
        {
            return COMM.POSTDataaADM(data, "QRCode_GenerationFacade/get_classes/");
        }

        public QRCode_GenerationDTO get_cls_sections(QRCode_GenerationDTO data)
        {
            return COMM.POSTDataaADM(data, "QRCode_GenerationFacade/get_cls_sections/");
        }
        public QRCode_GenerationDTO GetStudents(QRCode_GenerationDTO data)
        {
            return COMM.POSTDataaADM(data, "QRCode_GenerationFacade/GetStudents/");
        }
        public QRCode_GenerationDTO QRReportDetails(QRCode_GenerationDTO data)
        {
            return COMM.POSTDataaADM(data, "QRCode_GenerationFacade/QRReportDetails/");
        }
        public QRCode_GenerationDTO StaffGetdetails(QRCode_GenerationDTO data)
        {
            return COMM.POSTDataaADM(data, "QRCode_GenerationFacade/StaffGetdetails/");
        }

        public QRCode_GenerationDTO onloadgetdetails(QRCode_GenerationDTO dto)
        {
            return COMM.POSTDataaADM(dto, "QRCode_GenerationFacade/onloadgetdetails");
        }


        public QRCode_GenerationDTO getEmployeedetailsBySelection(QRCode_GenerationDTO maspage)
        {
            return COMM.POSTDataaADM(maspage, "QRCode_GenerationFacade/getEmployeedetailsBySelection/");
        }


        public QRCode_GenerationDTO get_depts(QRCode_GenerationDTO maspage)
        {
            return COMM.POSTDataaADM(maspage, "QRCode_GenerationFacade/get_depts/");
        }

        public QRCode_GenerationDTO get_desig(QRCode_GenerationDTO maspage)
        {
            return COMM.POSTDataaADM(maspage, "QRCode_GenerationFacade/get_desig/");
        }
        public QRCode_GenerationDTO filterEmployeedetailsBySelection(QRCode_GenerationDTO dto)
        {
            return COMM.POSTDataaADM(dto, "QRCode_GenerationFacade/filterEmployeedetailsBySelection");
        }

        public QRCode_GenerationDTO QRcodegeneration(QRCode_GenerationDTO dto)
        {
            return COMM.POSTDataaADM(dto, "QRCode_GenerationFacade/QRcodegeneration");
        }
        public QRCode_GenerationDTO StudentQRCode(QRCode_GenerationDTO dto)
        {
            return COMM.POSTDataaADM(dto, "QRCode_GenerationFacade/StudentQRCode");
        }
    }
}
