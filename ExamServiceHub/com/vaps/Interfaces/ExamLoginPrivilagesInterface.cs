
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Exam;

//PreadmissionDTOs.com.vaps.admission


namespace ExamServiceHub.com.vaps.Interfaces
{
    public interface ExamLoginPrivilagesInterface
    {

        Exm_Login_PrivilegeDTO savedetails(Exm_Login_PrivilegeDTO data);
        Exm_Login_PrivilegeDTO deactivate(Exm_Login_PrivilegeDTO data);
        string deleteRecord(Exm_Login_PrivilegeDTO data);
        Exm_Login_PrivilegeDTO editdetails(Exm_Login_PrivilegeDTO data); 
        Exm_Login_PrivilegeDTO getalldetailsviewrecords(Exm_Login_PrivilegeDTO data);
        Exm_Login_PrivilegeDTO getcategory(Exm_Login_PrivilegeDTO data);
        Exm_Login_PrivilegeDTO getclassid(Exm_Login_PrivilegeDTO data); 
        Exm_Login_PrivilegeDTO getclstechdetails(Exm_Login_PrivilegeDTO data);
        Exm_Login_PrivilegeDTO Getdetails(Exm_Login_PrivilegeDTO data); 
        Exm_Login_PrivilegeDTO Studentdetails(Exm_Login_PrivilegeDTO data);
        Exm_Login_PrivilegeDTO OnAcdyear(Exm_Login_PrivilegeDTO data);
        
    }
}
