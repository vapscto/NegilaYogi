using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.interfaces
{
    public interface Student_SettlementInterface
    {
        Student_SettlementDTO Getdetails(Student_SettlementDTO data);
        Student_SettlementDTO getdates(Student_SettlementDTO data);
        Student_SettlementDTO savedata(Student_SettlementDTO data);
        Student_SettlementDTO viewrecords(Student_SettlementDTO data);
        Student_SettlementDTO get_classes(Student_SettlementDTO data);
        Student_SettlementDTO get_sections(Student_SettlementDTO data);
        Student_SettlementDTO get_routes(Student_SettlementDTO data);
        Student_SettlementDTO getreport(Student_SettlementDTO data);
        Task<Student_SettlementDTO> getreport1(Student_SettlementDTO data);
        Student_SettlementDTO fillmerchants(Student_SettlementDTO data);
        Student_SettlementDTO paymentlogs(Student_SettlementDTO data);
        
    }
}
