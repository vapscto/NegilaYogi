using PreadmissionDTOs.com.vaps.MobileApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalHub.com.vaps.MobileApp.Interfaces
{
    public interface AdmissionInterface
    {
        AdmissionDTO.getAcademicyear getAcademicYear(AdmissionDTO.getAcademicyear data);
        AdmissionDTO.getClass getclass(AdmissionDTO.getClass data);
        AdmissionDTO.getSection getsection(AdmissionDTO.getSection data);
        AdmissionDTO.getClass AcademicyearwiseClass(AdmissionDTO.getClass data);
        AdmissionDTO.getSection AcademicyearwiseClassSection(AdmissionDTO.getSection data);
        AdmissionDTO.getstudent AcademicyearwiseClassSectionStudent(AdmissionDTO.getstudent data);

        AdmissionDTO.getCertificateApply getOnloadCertificateapply(AdmissionDTO.getCertificateApply data);
        Task<AdmissionDTO.saveCertificateApply> applyCertifateApplySave(AdmissionDTO.saveCertificateApply data);
        AdmissionDTO.getCertificateDetails getCertificateDetails(AdmissionDTO.getCertificateDetails data);

        AdmissionDTO.getloadFeedbackdata getloadFeedbackdata(AdmissionDTO.getloadFeedbackdata data);
        AdmissionDTO.saveFeedbackFormDTO savefeedback(AdmissionDTO.saveFeedbackFormDTO data);

        //Interaction
        Task<AdmissionDTO.OnloadInteractionsDTO> getInteractionloaddata(AdmissionDTO.OnloadInteractionsDTO data);
        Task<AdmissionDTO.replyInteractionsDTO> intractionreply(AdmissionDTO.replyInteractionsDTO data);
        AdmissionDTO.replysaveInteractionsDTO intractionreplysave(AdmissionDTO.replysaveInteractionsDTO data);      
        Task<AdmissionDTO.ComposeOnloadInteractionsDTO> intractioncomposeOnload(AdmissionDTO.ComposeOnloadInteractionsDTO data);
        Task<AdmissionDTO.composeOnselectOFTeacher> composeOnselectOFTeacher(AdmissionDTO.composeOnselectOFTeacher data);
        AdmissionDTO.composeOnsubmitOFStudent ComposeStudentSubmit(AdmissionDTO.composeOnsubmitOFStudent data);


        //added by kavita
        AdmissionDTO.ttLoadData ttgetloaddata(AdmissionDTO.ttLoadData data);
        AdmissionDTO.ttGetStudent getStudentTT(AdmissionDTO.ttGetStudent data);
        AdmissionDTO.stdDashboardExam stdDashboardExam(AdmissionDTO.stdDashboardExam data);



        //added by roopa

        AdmissionDTO.onclick_Homework_load onclick_Homework_load(AdmissionDTO.onclick_Homework_load data);
        AdmissionDTO.onclickClasswork onclick_Classwork_load(AdmissionDTO.onclickClasswork data);
        AdmissionDTO.onclick_notice onclick_notice(AdmissionDTO.onclick_notice data);
        AdmissionDTO.getClassworksave savecls_doc(AdmissionDTO.getClassworksave data);
        AdmissionDTO.gethomeworksave savehome_doc(AdmissionDTO.gethomeworksave data);

        //class wise timetable
        AdmissionDTO.attGetLoadData Attgetloaddata(AdmissionDTO.attGetLoadData data);
        AdmissionDTO.attGetdetails attGetdetails(AdmissionDTO.attGetdetails data);

        //added by sanjeev
        AdmissionDTO.onclick_LIB onclick_LIB(AdmissionDTO.onclick_LIB data);
        //onclick_LIBstaff
        AdmissionDTO.onclick_LIB onclick_LIBstaff(AdmissionDTO.onclick_LIB data);
        AdmissionDTO.getCoedata getloaddataCoe(AdmissionDTO.getCoedata data);
        AdmissionDTO.getCoedata getcoedata(AdmissionDTO.getCoedata data);
        AdmissionDTO.stdDashboardLoad stdDashboardDet(AdmissionDTO.stdDashboardLoad data);

        AdmissionDTO.getstudent stdFeeDue(AdmissionDTO.getstudent data);
        AdmissionDTO.daywiseTimetable daywiseTimetable(AdmissionDTO.daywiseTimetable data);


        // Dashboard
        AdmissionDTO.userDashboardLoad UserDashboardDetails(AdmissionDTO.userDashboardLoad data); 
        AdmissionDTO.UserProfileDetailsDTO UserProfileDetails(AdmissionDTO.UserProfileDetailsDTO data); 
        
        //Staff Dashboard
        AdmissionDTO.staffDashboardLoad staffDashboardDetails(AdmissionDTO.staffDashboardLoad data);
        //Manager Dashboard
        AdmissionDTO.ManagerDashboard ManagerDashboardDetails(AdmissionDTO.ManagerDashboard data);
        //PushNotification
        AdmissionDTO.PushNotification PushNotification(AdmissionDTO.PushNotification data);

        Task<AdmissionDTO.ShortageOFAttandence> shortageOfAttendanceAlert(AdmissionDTO.ShortageOFAttandence data);


        AdmissionDTO.staffProfileDTO staffProfile(AdmissionDTO.staffProfileDTO data);

        AdmissionDTO.PushNotificationonload PushNotificationonload(AdmissionDTO.PushNotificationonload data);
        AdmissionDTO.PushNotificationonload NotificationonloadRead(AdmissionDTO.PushNotificationonload data);

        AdmissionDTO.AcademicFeesData AcademicwiseFeesDetails(AdmissionDTO.AcademicFeesData data);
        AdmissionDTO.AcademicFeesData AcademicwiseClassFeesDetails(AdmissionDTO.AcademicFeesData data);
        AdmissionDTO.versiondetails Mobileversion_control(AdmissionDTO.versiondetails data);

    }
}
