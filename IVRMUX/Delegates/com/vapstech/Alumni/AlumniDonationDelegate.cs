using CommonLibrary;
using Newtonsoft.Json;
using PreadmissionDTOs.com.vaps.Alumni;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Alumni
{
    public class AlumniDonationDelegate
    {

        CommonDelegate<AlumniStudentDTO, AlumniStudentDTO> comm = new CommonDelegate<AlumniStudentDTO, AlumniStudentDTO>();

        public AlumniStudentDTO Pageload(AlumniStudentDTO data)
        {
            return comm.POSTDataAlumni(data, "AlumniDonationFacade/Pageload/");
        }
        public AlumniStudentDTO getamount(AlumniStudentDTO dto)
        {
            return comm.POSTDataAlumni(dto, "AlumniDonationFacade/getamount/");
        }
         public AlumniStudentDTO getpayment_details(AlumniStudentDTO dto)
        {
            return comm.POSTDataAlumni(dto, "AlumniDonationFacade/getpayment_details/");
        }
         public AlumniStudentDTO paymentsave(AlumniStudentDTO dto)
        {
            return comm.POSTDataAlumni(dto, "AlumniDonationFacade/paymentsave/");
        }
         public AlumniStudentDTO getdonationreport(AlumniStudentDTO dto)
        {
            return comm.POSTDataAlumni(dto, "AlumniDonationFacade/getdonationreport/");
        }

        //=============master donation 
        public AlumniStudentDTO getdata_donation(AlumniStudentDTO dto)
        {
            return comm.POSTDataAlumni(dto, "AlumniDonationFacade/getdata_donation/");
        }
        public AlumniStudentDTO save_donation(AlumniStudentDTO dto)
        {
            return comm.POSTDataAlumni(dto, "AlumniDonationFacade/save_donation/");
        }
        public AlumniStudentDTO deactive_donation(AlumniStudentDTO dto)
        {
            return comm.POSTDataAlumni(dto, "AlumniDonationFacade/deactive_donation/");
        }
        public AlumniStudentDTO edit_donation(AlumniStudentDTO dto)
        {
            return comm.POSTDataAlumni(dto, "AlumniDonationFacade/edit_donation/");
        }
         public AlumniStudentDTO alumnidetails(AlumniStudentDTO dto)
        {
            return comm.POSTDataAlumni(dto, "AlumniDonationFacade/alumnidetails/");
        }
        public AlumniStudentDTO getdonationprint(AlumniStudentDTO dto)
        {
            return comm.POSTDataAlumni(dto, "AlumniDonationFacade/getdonationprint/");
        }

    }
}
