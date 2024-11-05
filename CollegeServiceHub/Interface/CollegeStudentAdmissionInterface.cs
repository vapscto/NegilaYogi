using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Interface
{
    public interface CollegeStudentAdmissionInterface
    {
        AdmMasterCollegeStudentDTO Getdetails(AdmMasterCollegeStudentDTO dto);
        AdmMasterCollegeStudentDTO getCourse(AdmMasterCollegeStudentDTO dto);
        AdmMasterCollegeStudentDTO getBranch(AdmMasterCollegeStudentDTO data);
        AdmMasterCollegeStudentDTO getSemester(AdmMasterCollegeStudentDTO dt);
        AdmMasterCollegeStudentDTO getcaste(AdmMasterCollegeStudentDTO dt);
        AdmMasterCollegeStudentDTO getQuotaCategory(AdmMasterCollegeStudentDTO dts);
        Task<save_firsttab_details> saveStudentDetails(save_firsttab_details data);
        AdmMasterCollegeStudentDTO Edit(AdmMasterCollegeStudentDTO Edata);
        AdmMasterCollegeStudentDTO checkDuplicate(AdmMasterCollegeStudentDTO check);
        AdmMasterCollegeStudentDTO getdpstate(AdmMasterCollegeStudentDTO check);
        AdmMasterCollegeStudentDTO saveAddress(AdmMasterCollegeStudentDTO adds);
        AdmMasterCollegeStudentDTO saveParentsDetails(AdmMasterCollegeStudentDTO ParentsData);
        AdmMasterCollegeStudentDTO StateByCountryName(AdmMasterCollegeStudentDTO country);
        AdmMasterCollegeStudentDTO saveOthersDetails(AdmMasterCollegeStudentDTO others);
        AdmMasterCollegeStudentDTO saveDocuments(AdmMasterCollegeStudentDTO docs);
        AdmMasterCollegeStudentDTO SearchByColumn(AdmMasterCollegeStudentDTO docs);
        AdmMasterCollegeStudentDTO DeleteEntry(AdmMasterCollegeStudentDTO docs);
        AdmMasterCollegeStudentDTO ViewStudentProfile(AdmMasterCollegeStudentDTO stu);
        //master competitive exam
        AdmMasterCollegeStudentDTO compExamName(AdmMasterCollegeStudentDTO country);
        //document view
        AdmMasterCollegeStudentDTO getprintdata(AdmMasterCollegeStudentDTO country);

        AdmMasterCollegeStudentDTO checkbiometriccode(AdmMasterCollegeStudentDTO stu);
        AdmMasterCollegeStudentDTO checkrfcardduplicate(AdmMasterCollegeStudentDTO stu);
    }
}
