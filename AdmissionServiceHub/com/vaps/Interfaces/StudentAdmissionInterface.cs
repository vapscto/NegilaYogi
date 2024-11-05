using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.admission;


namespace AdmissionServiceHub.com.vaps.Interfaces
{
    public interface StudentAdmissionInterface
    {
        Task<Adm_M_StudentDTO> GetData(Adm_M_StudentDTO Adm_M_StudentDTO);
        Adm_M_StudentDTO getdpstate(int id);
        Adm_M_StudentDTO getdpdistrict(int id);
        
        Adm_M_StudentDTO onchangebithplacecountry(int id);
        Adm_M_StudentDTO onchangenationality(int id);
        Adm_M_StudentDTO getdpcities(int id);
        Adm_M_StudentDTO GetSelectedRowDetails(Adm_M_StudentDTO ID);
        Adm_M_StudentDTO SaveData(Adm_M_StudentDTO mas);
        Adm_M_StudentDTO checkDuplicate(Adm_M_StudentDTO mas);
        Adm_M_StudentDTO getcaste(Adm_M_StudentDTO mas);

        // StudentDocumentDTO stud_doc_upload(StudentDocumentDTO stu);
        Adm_M_StudentDTO DeleteBondEntry(int ID);
        Adm_M_StudentDTO DeleteEntry(Adm_M_StudentDTO data);
        Adm_M_StudentDTO yearwisetcstd(Adm_M_StudentDTO data);
        Adm_M_StudentDTO addtocart(Adm_M_StudentDTO data);
        Adm_M_StudentDTO searchByColumn(Adm_M_StudentDTO adm);
        Adm_M_StudentDTO StateByCountryName(Adm_M_StudentDTO ct);
        Adm_M_StudentDTO getmaxminage(Adm_M_StudentDTO stu);
        Task<Adm_M_StudentDTO> savefirsttab(Adm_M_StudentDTO stu);
        Adm_M_StudentDTO savesecondtab(Adm_M_StudentDTO stu);
        Task<Adm_M_StudentDTO> savethirdtab(Adm_M_StudentDTO stu);
        Adm_M_StudentDTO savefourthtab(Adm_M_StudentDTO stu);
        Adm_M_StudentDTO savesixthtab(Adm_M_StudentDTO stu);
        Adm_M_StudentDTO savefinaltab(Adm_M_StudentDTO stu);
        Adm_M_StudentDTO checkbiometriccode(Adm_M_StudentDTO stu);
        Adm_M_StudentDTO checkrfcardduplicate(Adm_M_StudentDTO stu);
        Adm_M_StudentDTO onchangefathernationality(Adm_M_StudentDTO stu);
        Adm_M_StudentDTO onchangemothernationality(Adm_M_StudentDTO stu);

        /*  Admission Cancel OR WithDraw   */
        Adm_M_StudentDTO OnLoadAdmissionCancel(Adm_M_StudentDTO stu);
        Adm_M_StudentDTO OnChangeAdmissionCancelYear(Adm_M_StudentDTO stu);
        Adm_M_StudentDTO OnChangeAdmissionCancelStudent(Adm_M_StudentDTO stu);
        Adm_M_StudentDTO SaveAdmissionCancelStudent(Adm_M_StudentDTO stu);
        Adm_M_StudentDTO EditAdmissionCancelStudent(Adm_M_StudentDTO stu);

        // Admission Cancel Report
        Adm_M_StudentDTO OnLoadAdmissionCancelReport(Adm_M_StudentDTO stu);
        Adm_M_StudentDTO OnChangeAdmissionCancelReportYear(Adm_M_StudentDTO stu);
        Adm_M_StudentDTO ViewStudentProfile(Adm_M_StudentDTO stu);
        Task<Adm_M_StudentDTO> GetSubjectsofinstitute(Adm_M_StudentDTO stu);
    }
}
