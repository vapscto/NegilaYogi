using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.College.Admission;
using PreadmissionDTOs.com.vaps.College.Preadmission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegePreadmission.Interfaces
{
    public interface ApplicationFormInterface 
    {
        CollegePreadmissionstudnetDto Getdetails(CollegePreadmissionstudnetDto dto);
        CollegePreadmissionstudnetDto getCourse(CollegePreadmissionstudnetDto dto);
        CollegePreadmissionstudnetDto getBranch(CollegePreadmissionstudnetDto data);
        CollegePreadmissionstudnetDto Dashboarddetails(CollegePreadmissionstudnetDto stu);

        CollegePreadmissionstudnetDto paynow(CollegePreadmissionstudnetDto dt);

        PaymentDetails payuresponse(PaymentDetails stu);

        PaymentDetails.PAYTM paytmresponse(PaymentDetails.PAYTM stu);
        CollegePreadmissionstudnetDto getSemester(CollegePreadmissionstudnetDto dt);
        CollegePreadmissionstudnetDto getcaste(CollegePreadmissionstudnetDto dt);
        AdmMasterCollegeStudentDTO getQuotaCategory(AdmMasterCollegeStudentDTO dts);
        CollegePreadmissionstudnetDto saveStudentDetails(CollegePreadmissionstudnetDto data);
        CollegePreadmissionstudnetDto Edit(CollegePreadmissionstudnetDto Edata);
        CollegePreadmissionstudnetDto getprintdata(CollegePreadmissionstudnetDto Edata);
        AdmMasterCollegeStudentDTO checkDuplicate(AdmMasterCollegeStudentDTO check);
        CollegePreadmissionstudnetDto getdpstate(CollegePreadmissionstudnetDto check);
        CollegePreadmissionstudnetDto saveAddress(CollegePreadmissionstudnetDto adds);
        CollegePreadmissionstudnetDto saveParentsDetails(CollegePreadmissionstudnetDto ParentsData);
        CollegePreadmissionstudnetDto StateByCountryName(CollegePreadmissionstudnetDto country);
        CollegePreadmissionstudnetDto saveOthersDetails(CollegePreadmissionstudnetDto others);
        CollegePreadmissionstudnetDto saveDocuments(CollegePreadmissionstudnetDto docs);
        //AdmMasterCollegeStudentDTO SearchByColumn(AdmMasterCollegeStudentDTO docs);
        //AdmMasterCollegeStudentDTO DeleteEntry(AdmMasterCollegeStudentDTO docs);\

        //master competitive exam
        CollegePreadmissionstudnetDto compExamName(CollegePreadmissionstudnetDto country);
        CollegePreadmissionstudnetDto Clgapplicationstudocs(CollegePreadmissionstudnetDto country);
        
        PaymentDetails razorgetpaymentresponse(PaymentDetails stu);
    }
}
