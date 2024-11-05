using PreadmissionDTOs.com.vaps.Alumni;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlumniHub.Com.Interface
{
    public interface AlumniDonationInterface
    {
        AlumniStudentDTO Pageload(AlumniStudentDTO clswisedailyattDTO);
        AlumniStudentDTO getamount(AlumniStudentDTO clswisedailyattDTO);
        AlumniStudentDTO getpayment_details(AlumniStudentDTO clswisedailyattDTO);
        AlumniStudentDTO paymentsave(AlumniStudentDTO clswisedailyattDTO);
        AlumniStudentDTO getdonationreport(AlumniStudentDTO clswisedailyattDTO);
        //=============master donation 
        AlumniStudentDTO getdata_donation(AlumniStudentDTO clswisedailyattDTO);
        AlumniStudentDTO save_donation(AlumniStudentDTO clswisedailyattDTO);
        AlumniStudentDTO edit_donation(AlumniStudentDTO clswisedailyattDTO);
        AlumniStudentDTO alumnidetails(AlumniStudentDTO clswisedailyattDTO);
        AlumniStudentDTO getdonationprint(AlumniStudentDTO clswisedailyattDTO);
        AlumniStudentDTO deactive_donation(AlumniStudentDTO clswisedailyattDTO);
        
    }
}
