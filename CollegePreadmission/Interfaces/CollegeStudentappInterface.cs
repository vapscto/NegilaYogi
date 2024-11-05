using PreadmissionDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegePreadmission.Interfaces
{
    public  interface CollegeStudentappInterface
    {
        Task<StudentApplicationDTO> studdet(StudentApplicationDTO stu); // Added on 9-11-2016
        Task<CountryDTO> countrydrp(CountryDTO stu);

        // Added on 19-9-2016
        Task<CountryDTO> getIndependentDropDowns(CountryDTO stu);

        CountryDTO ActivateDactivate(CountryDTO stu);

        Task<CountryDTO> Getcountofstudents(CountryDTO stu);

        Task<CountryDTO> Dashboarddetails(CountryDTO stu);

        StudentApplicationDTO getmaxminage(StudentApplicationDTO stu);

        Task<StudentApplicationDTO> GetSubjectsofinstitute(StudentApplicationDTO stu);



        Task<CityDTO> getCityByCountry(int id);
        Task<StateDTO> getStateByCountry(int id);

        Task<StateDTO> getroutes(int id);

        Task<StateDTO> getrouteslocation(int id);

        Task<StateDTO> getdprospectusdetails(int id);

        Task<StateDTO> getdpstatesubcatse(int id);

        Task<StateDTO> getdpstatesubcatsefather(int id);

        StudentApplicationDTO getapplicationhtml(StudentApplicationDTO id);

        Task<StateDTO> getdpstatesubcatsemother(int id);

        Task<CityDTO> getCityByState(int id);
        StudentApplicationDTO getStudentEditData(StudentApplicationDTO dt);

        StudentApplicationDTO paynow(StudentApplicationDTO dt);
        StudentApplicationDTO getstudentprintData(StudentApplicationDTO dt);

        StudentApplicationDTO getdashboardpage(StudentApplicationDTO dt);
        StudentApplicationDTO searchdata(StudentApplicationDTO stu);

        PaymentDetails payuresponse(PaymentDetails stu);
        void deleterec(int id);


        Task<StudentHelthcertificateDTO> savehealthcertificatedetail(StudentHelthcertificateDTO stu);
        Task<StudentHelthcertificateDTO> getstudata(StudentHelthcertificateDTO stu);
        StudentHelthcertificateDTO getEdithelthData(StudentHelthcertificateDTO dt);
        StudentHelthcertificateDTO deletehelthdetails(StudentHelthcertificateDTO id);
        StudentHelthcertificateDTO printgethelthData(StudentHelthcertificateDTO dt);

        StudentApplicationDTO fill_prospectus(StudentApplicationDTO data);
    }
}
