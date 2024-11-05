using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DomainModel;
using PreadmissionDTOs;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model;
using DataAccessMsSqlServerProvider;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

using DomainModel.Model.com.vapstech.admission;
using CommonLibrary;
using System.Text.RegularExpressions;
using System.Globalization;

namespace AdmissionServiceHub.com.vaps.Services
{
    public class AdmissionImportImpl : Interfaces.AdmissionImportInterface
    {

        public DomainModelMsSqlServerContext _DomainModelMsSqlServerContext;
        ILogger<AdmissionImportImpl> _acdimpl;
        private readonly AdmissionFormContext _AdmissionFormContext;

        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly DomainModelMsSqlServerContext _db;

        private readonly ILogger _log;

        public AdmissionImportImpl(ILogger<AdmissionImportImpl> acdimpl, DomainModelMsSqlServerContext DomainModelMsSqlServerContext, AdmissionFormContext AdmissionFormContext, DomainModelMsSqlServerContext db, ILogger<StudentAdmissionImp> loggerFactor, UserManager<ApplicationUser> UserManager)
        {
            _DomainModelMsSqlServerContext = DomainModelMsSqlServerContext;
            _acdimpl = acdimpl;


            _AdmissionFormContext = AdmissionFormContext;
            _db = db;
            _log = loggerFactor;
            _UserManager = UserManager;
        }
        public ImportStudentWrapperDTO getdetails1(ImportStudentWrapperDTO stu)
        {

            try
            {
                // Adm_M_Student enq1 = Mapper.Map<Adm_M_Student>(stu);
                _acdimpl.LogInformation("entered try block");
                int sucesscount = 0;
                int failcount = 0;
                string failnames = "";
                for (int i = 0; i < stu.newlstget.Count; i++)
                {
                    _acdimpl.LogInformation("entered for loop");
                    Adm_M_Student enq = new Adm_M_Student();

                    var IVRMMRId1 = _DomainModelMsSqlServerContext.School_M_Class.Where(t => t.ASMCL_ClassName.Equals(stu.newlstget[i].Class.TrimEnd().TrimStart()) && t.MI_Id == stu.MI_Id).FirstOrDefault();
                    enq.ASMCL_Id = IVRMMRId1.ASMCL_Id;

                    _acdimpl.LogInformation("entered class id");
                    enq.AMST_FirstName = stu.newlstget[i].FirstName;
                    enq.AMST_MiddleName = stu.newlstget[i].MiddleName;
                    enq.AMST_LastName = stu.newlstget[i].LastName;

                    // DateTime dt = DateTime.Parse("24/01/2013", new System.Globalization.CultureInfo("en-CA"));
                    enq.AMST_Date = Convert.ToDateTime(stu.newlstget[i].amstdate);
                    _acdimpl.LogInformation("entered amstdate");
                    enq.AMST_RegistrationNo = stu.newlstget[i].AMSTRegistrationNo;
                    enq.AMST_AdmNo = stu.newlstget[i].AMSTAdmNo;
                    enq.MI_Id = stu.MI_Id;

                    enq.AMC_Id = 1; //not passing

                    enq.AMST_Sex = stu.newlstget[i].Gender;
                    enq.AMST_DOB = Convert.ToDateTime(stu.newlstget[i].DOB);
                    _acdimpl.LogInformation("entered dob");
                    enq.AMST_DOB_Words = stu.newlstget[i].AMSTDOBWords;
                    int age;
                    age = Convert.ToInt32(DateTime.Now.Year - Convert.ToDateTime(stu.newlstget[i].DOB).Year);

                    if (age > 0)
                    {
                        age -= Convert.ToInt32(DateTime.Now < Convert.ToDateTime(stu.newlstget[i].DOB).Date.AddYears(age));
                    }
                    else
                    {
                        age = 0;
                    }
                    enq.PASR_Age = age; //not passing

                    enq.AMST_BloodGroup = stu.newlstget[i].BloodGroup;
                    enq.AMST_MotherTongue = stu.newlstget[i].MotherTongue;

                    var year = _DomainModelMsSqlServerContext.AcademicYear.Where(t => t.ASMAY_Year.Equals(stu.newlstget[i].year.TrimEnd().TrimStart()) && t.MI_Id == stu.MI_Id).FirstOrDefault();
                    enq.ASMAY_Id = year.ASMAY_Id;
                    _acdimpl.LogInformation("entered year proc");
                    //enq.IVRMMR_Id = 1;
                    //enq.IMCC_Id = 1;
                    //enq.IC_Id = 1;

                    _acdimpl.LogInformation("entered permanent address");
                    enq.AMST_PerStreet = stu.newlstget[i].PermanentStreet;
                    enq.AMST_PerArea = stu.newlstget[i].PermanentArea;
                    enq.AMST_PerCity = stu.newlstget[i].PermanentCity;
                    enq.AMST_PerAdd3 = stu.newlstget[i].Permanentadd3;
                    _acdimpl.LogInformation("entered permanent state");
                    var perstate = _DomainModelMsSqlServerContext.State.Where(t => t.IVRMMS_Name == stu.newlstget[i].Permanentstate.TrimEnd().TrimStart()).FirstOrDefault();
                    enq.AMST_PerState = perstate.IVRMMS_Id;
                    _acdimpl.LogInformation("entered permanent country");
                    var percountry = _DomainModelMsSqlServerContext.country.Where(t => t.IVRMMC_CountryName == stu.newlstget[i].PermanentCountry.TrimEnd().TrimStart()).FirstOrDefault();
                    // enq.AMST_PerCountry = percountry.IVRMMC_Id;
                    enq.AMST_PerPincode = Convert.ToInt32(stu.newlstget[i].PermanentPincode);
                    _acdimpl.LogInformation("out permanent address");

                    _acdimpl.LogInformation("entered contact address");
                    enq.AMST_ConStreet = stu.newlstget[i].PresentStreet;
                    enq.AMST_ConArea = stu.newlstget[i].PresentArea;
                    enq.AMST_ConCity = stu.newlstget[i].persentcity;
                    _acdimpl.LogInformation("entered contact address");
                    var constate = _DomainModelMsSqlServerContext.State.Where(t => t.IVRMMS_Name == stu.newlstget[i].PresentState.TrimEnd().TrimStart()).FirstOrDefault();
                    enq.AMST_ConState = constate.IVRMMS_Id;
                    _acdimpl.LogInformation("entered contact address");
                    var concountry = _DomainModelMsSqlServerContext.country.Where(t => t.IVRMMC_CountryName == stu.newlstget[i].PresentCountry.TrimEnd().TrimStart()).FirstOrDefault();
                    enq.AMST_ConCountry = concountry.IVRMMC_Id;
                    enq.AMST_ConPincode = Convert.ToInt32(stu.newlstget[i].PresentPincode);
                    enq.AMST_AadharNo = Convert.ToInt64(stu.newlstget[i].AadharNo);
                    enq.AMST_StuBankAccNo = stu.newlstget[i].BankAccountNo;
                    enq.AMST_StuBankIFSC_Code = stu.newlstget[i].IFSCCode;
                    enq.AMST_StuCasteCertiNo = stu.newlstget[i].CasteCertificateNo;
                    enq.AMST_MobileNo = Convert.ToInt64(stu.newlstget[i].MobileNo);
                    enq.AMST_emailId = stu.newlstget[i].EmailID;

                    _acdimpl.LogInformation("entered father active condititon flag active 1");
                    if (stu.newlstget[i].FatherAlive == "1")
                    {
                        _acdimpl.LogInformation("active 1 yes");

                        enq.AMST_FatherAliveFlag = stu.newlstget[i].FatherAlive;
                        enq.AMST_FatherName = stu.newlstget[i].FatherName;
                        enq.AMST_FatherAadharNo = Convert.ToInt64(stu.newlstget[i].FatherAadharNo);
                        enq.AMST_FatherSurname = stu.newlstget[i].FatherSurname;
                        enq.AMST_FatherEducation = stu.newlstget[i].FatherEducation;   //not passing
                        enq.AMST_FatherOccupation = stu.newlstget[i].FatherOccupation;
                        enq.AMST_FatherOfficeAdd = stu.newlstget[i].FatherOfficeAddress;
                        enq.AMST_FatherDesignation = stu.newlstget[i].FatherDesignation;
                        enq.AMST_FatherMonIncome = Convert.ToDecimal(stu.newlstget[i].FatherMonthlyIncome);
                        enq.AMST_FatherAnnIncome = Convert.ToDecimal(stu.newlstget[i].FatherAnnualIncome);
                        var fathernationality = _DomainModelMsSqlServerContext.country.Where(t => t.IVRMMC_CountryName == stu.newlstget[i].FatherNationality.TrimEnd().TrimStart()).FirstOrDefault();
                        enq.AMST_FatherNationality = Convert.ToInt64(stu.newlstget[i].FatherNationality);
                        enq.AMST_FatherMobleNo = Convert.ToInt64(stu.newlstget[i].Fathermobileno);
                        enq.AMST_FatheremailId = stu.newlstget[i].FatherEmailId;
                        enq.AMST_FatherBankAccNo = stu.newlstget[i].FatherAccountNo;
                        enq.AMST_FatherBankIFSC_Code = stu.newlstget[i].FatherIFSCcode;
                        enq.AMST_FatherCasteCertiNo = stu.newlstget[i].FathercastecertificateNo;
                        enq.ANST_FatherPhoto = stu.newlstget[i].FatherPhoto;
                    }
                    else
                    {
                        _acdimpl.LogInformation("active 0 no");
                        enq.AMST_FatherAliveFlag = stu.newlstget[i].FatherAlive;
                        enq.AMST_FatherName = stu.newlstget[i].FatherName;
                        enq.AMST_FatherAadharNo = Convert.ToInt64(stu.newlstget[i].FatherAadharNo);
                        enq.AMST_FatherSurname = stu.newlstget[i].FatherSurname;
                        enq.AMST_FatherEducation = stu.newlstget[i].FatherEducation;   //not passing
                        enq.AMST_FatherOccupation = stu.newlstget[i].FatherOccupation;
                        enq.AMST_FatherOfficeAdd = stu.newlstget[i].FatherOfficeAddress;
                        enq.AMST_FatherDesignation = stu.newlstget[i].FatherDesignation;
                        enq.AMST_FatherMonIncome = Convert.ToDecimal(stu.newlstget[i].FatherMonthlyIncome);
                        enq.AMST_FatherAnnIncome = Convert.ToDecimal(stu.newlstget[i].FatherAnnualIncome);
                        var fathernationality1 = _DomainModelMsSqlServerContext.country.Where(t => t.IVRMMC_CountryName == stu.newlstget[i].FatherNationality.TrimEnd().TrimStart()).FirstOrDefault();
                        enq.AMST_FatherNationality = fathernationality1.IVRMMC_Id;
                        enq.AMST_FatherMobleNo = Convert.ToInt64(stu.newlstget[i].Fathermobileno);
                        enq.AMST_FatheremailId = stu.newlstget[i].FatherEmailId;
                        enq.AMST_FatherBankAccNo = stu.newlstget[i].FatherAccountNo;
                        enq.AMST_FatherBankIFSC_Code = stu.newlstget[i].FatherIFSCcode;
                        enq.AMST_FatherCasteCertiNo = stu.newlstget[i].FathercastecertificateNo;
                        enq.ANST_FatherPhoto = stu.newlstget[i].FatherPhoto;
                    }


                    //mother active flag checking 
                    _acdimpl.LogInformation("entered mother active condititon flag active 1");
                    if (stu.newlstget[i].MotherAlive == "1")
                    {
                        _acdimpl.LogInformation("active flag 1 yes");
                        enq.AMST_MotherAliveFlag = stu.newlstget[i].MotherAlive;
                        enq.AMST_MotherName = stu.newlstget[i].MotherName;
                        enq.AMST_MotherAadharNo = Convert.ToInt64(stu.newlstget[i].MotherAadharNo);
                        enq.AMST_MotherSurname = stu.newlstget[i].MotherSurname;
                        enq.AMST_MotherEducation = stu.newlstget[i].MotherEducation;
                        enq.AMST_MotherOccupation = stu.newlstget[i].MotherOcupation;
                        enq.AMST_MotherOfficeAdd = stu.newlstget[i].MotherOfficesAddress;
                        enq.AMST_MotherDesignation = stu.newlstget[i].MotherDesignation;
                        enq.AMST_MotherMonIncome = Convert.ToDecimal(stu.newlstget[i].MotherMonthlyIncome);
                        enq.AMST_MotherAnnIncome = Convert.ToDecimal(stu.newlstget[i].MotherAnnualIncome);
                        var mothernationality = _DomainModelMsSqlServerContext.country.Where(t => t.IVRMMC_CountryName == stu.newlstget[i].MotherNationality.TrimEnd().TrimStart()).FirstOrDefault();
                        enq.AMST_MotherNationality = mothernationality.IVRMMC_Id;
                        enq.AMST_MotherMobileNo = Convert.ToInt64(stu.newlstget[i].MotherMobileNo);
                        enq.AMST_MotherEmailId = stu.newlstget[i].MotherEmailId;
                        enq.AMST_MotherBankAccNo = stu.newlstget[i].MotherBankAccountNo;
                        enq.AMST_MotherBankIFSC_Code = stu.newlstget[i].MotherIFSCcode;
                        enq.AMST_MotherCasteCertiNo = stu.newlstget[i].MotherCasteCertificateNo;
                        enq.AMST_TotalIncome = Convert.ToDecimal(stu.newlstget[i].TotalIncome);
                        enq.ANST_MotherPhoto = stu.newlstget[i].MotherPhoto;
                    }
                    else
                    {
                        _acdimpl.LogInformation("active flag 0 yes");
                        enq.AMST_MotherAliveFlag = stu.newlstget[i].MotherAlive;
                        enq.AMST_MotherName = stu.newlstget[i].MotherName;
                        enq.AMST_MotherAadharNo = Convert.ToInt64(stu.newlstget[i].MotherAadharNo);
                        enq.AMST_MotherSurname = stu.newlstget[i].MotherSurname;
                        enq.AMST_MotherEducation = stu.newlstget[i].MotherEducation;
                        enq.AMST_MotherOccupation = stu.newlstget[i].MotherOcupation;
                        enq.AMST_MotherOfficeAdd = stu.newlstget[i].MotherOfficesAddress;
                        enq.AMST_MotherDesignation = stu.newlstget[i].MotherDesignation;
                        enq.AMST_MotherMonIncome = Convert.ToDecimal(stu.newlstget[i].MotherMonthlyIncome);
                        enq.AMST_MotherAnnIncome = Convert.ToDecimal(stu.newlstget[i].MotherAnnualIncome);
                        var mothernationality1 = _DomainModelMsSqlServerContext.country.Where(t => t.IVRMMC_CountryName == stu.newlstget[i].MotherNationality.TrimEnd().TrimStart()).FirstOrDefault();
                        enq.AMST_MotherNationality = mothernationality1.IVRMMC_Id;
                        enq.AMST_MotherMobileNo = Convert.ToInt64(stu.newlstget[i].MotherMobileNo);
                        enq.AMST_MotherEmailId = stu.newlstget[i].MotherEmailId;
                        enq.AMST_MotherBankAccNo = stu.newlstget[i].MotherBankAccountNo;
                        enq.AMST_MotherBankIFSC_Code = stu.newlstget[i].MotherIFSCcode;
                        enq.AMST_MotherCasteCertiNo = stu.newlstget[i].MotherCasteCertificateNo;
                        enq.AMST_TotalIncome = Convert.ToDecimal(stu.newlstget[i].TotalIncome);
                        enq.ANST_MotherPhoto = stu.newlstget[i].MotherPhoto;
                    }

                    _acdimpl.LogInformation("student details");
                    enq.AMST_BirthPlace = stu.newlstget[i].StudentBirthPlace;
                    var studnationality = _DomainModelMsSqlServerContext.country.Where(t => t.IVRMMC_CountryName == stu.newlstget[i].StudentNationality.TrimEnd().TrimStart()).FirstOrDefault();
                    enq.AMST_Nationality = studnationality.IVRMMC_Id;
                    enq.AMST_BPLCardFlag = Convert.ToInt32(stu.newlstget[i].BPLCardFlag);
                    enq.AMST_BPLCardNo = stu.newlstget[i].BPLCardNo;

                    enq.AMST_HostelReqdFlag = Convert.ToInt32(stu.newlstget[i].HostelFacility);
                    enq.AMST_TransportReqdFlag = Convert.ToInt32(stu.newlstget[i].TransportFacility);
                    enq.AMST_GymReqdFlag = Convert.ToInt32(stu.newlstget[i].GymFacility);
                    enq.AMST_ECSFlag = Convert.ToInt32(stu.newlstget[i].ECSFlag);
                    enq.AMST_PaymentFlag = Convert.ToInt32(stu.newlstget[i].PaymentFlag);
                    enq.AMST_AmountPaid = Convert.ToInt32(stu.newlstget[i].AmountPaid);
                    enq.AMST_PaymentType = stu.newlstget[i].PaymentType;
                    enq.AMST_PaymentDate = Convert.ToDateTime(stu.newlstget[i].PaymentDate);
                    enq.AMST_ReceiptNo = stu.newlstget[i].ReceiptNo;
                    enq.AMST_ActiveFlag = Convert.ToInt32(stu.newlstget[i].ActiveFlag);
                    enq.AMST_ApplStatus = stu.newlstget[i].Applicationstatus;
                    enq.AMST_FinalpaymentFlag = Convert.ToInt32(stu.newlstget[i].FinalPaymentFlag);
                    enq.AMST_Noofsisters = Convert.ToInt32(stu.newlstget[i].Noofsisters);
                    enq.AMST_Noofbrothers = Convert.ToInt32(stu.newlstget[i].NoOfBrothers);
                    _acdimpl.LogInformation("Religion");
                    var religion1 = _DomainModelMsSqlServerContext.masterReligion.Where(t => t.IVRMMR_Name == stu.newlstget[i].Religion.TrimEnd().TrimStart()).FirstOrDefault();
                    enq.IVRMMR_Id = religion1.IVRMMR_Id;
                    _acdimpl.LogInformation("caste");
                    //var caste1 = _DomainModelMsSqlServerContext.Where(t => t.IVRMMR_Name == stu.newlstget[i].Religion.TrimEnd().TrimStart()).FirstOrDefault();
                    enq.CreatedDate = DateTime.Now;
                    enq.UpdatedDate = DateTime.Now;
                    enq.AMST_Photoname = stu.newlstget[i].PhotoName;
                    enq.AMST_SOL = "S";

                    _DomainModelMsSqlServerContext.Add(enq);
                    var flag = _DomainModelMsSqlServerContext.SaveChanges();
                    if (flag >= 1)
                    {
                        stu.stuStatus = "true";
                        sucesscount = sucesscount + 1;
                    }
                    else
                    {
                        stu.stuStatus = "false";
                        failcount = failcount + 1;
                        failnames = failnames + "," + failnames;
                    }
                }
            }
            catch (Exception e)
            {
                stu.stuStatus = "false";
                Console.WriteLine(e.Message);
            }

            return stu;
        }

        public async Task<ImportStudentWrapperDTO> getdetails_working(ImportStudentWrapperDTO stu)
        {

            // Adm_M_Student enq1 = Mapper.Map<Adm_M_Student>(stu);
            _acdimpl.LogInformation("entered try block");
            int sucesscount = 0;
            int failcount = 0;
            string failnames = "";
            string finalnames = "";
            try
            {
                for (int i = 0; i < stu.newlstget.Count; i++)
                {
                    try
                    {
                        _acdimpl.LogInformation("entered for loop");
                        Adm_M_Student enq = new Adm_M_Student();

                        // var IVRMMRId1 = _DomainModelMsSqlServerContext.School_M_Class.Where(t => t.ASMCL_ClassName.Equals(stu.newlstget[i].Class.TrimEnd().TrimStart()) && t.MI_Id == stu.MI_Id).FirstOrDefault();

                        enq.AMST_ActiveFlag = 1;
                        enq.ASMCL_Id = Convert.ToInt64(stu.newlstget[i].Class);
                        _acdimpl.LogInformation("entered class id");
                        enq.AMST_FirstName = stu.newlstget[i].FirstName;
                        failnames = stu.newlstget[i].FirstName;
                        enq.AMST_MiddleName = stu.newlstget[i].MiddleName;
                        enq.AMST_LastName = stu.newlstget[i].LastName;

                        // DateTime dt = DateTime.Parse("24/01/2013", new System.Globalization.CultureInfo("en-CA"));
                        enq.AMST_Date = Convert.ToDateTime(stu.newlstget[i].amstdate);
                        _acdimpl.LogInformation("entered amstdate");
                        enq.AMST_RegistrationNo = stu.newlstget[i].AMSTRegistrationNo;
                        enq.AMST_AdmNo = stu.newlstget[i].AMSTAdmNo;
                        enq.MI_Id = stu.MI_Id;

                        enq.AMC_Id = 1; //not passing

                        enq.AMST_Sex = stu.newlstget[i].Gender;
                        string tempDate = stu.newlstget[i].DOB.TrimEnd().TrimStart().ToString();
                        enq.AMST_DOB = Convert.ToDateTime(tempDate);
                        _acdimpl.LogInformation("entered dob");
                        enq.AMST_DOB_Words = stu.newlstget[i].AMSTDOBWords;
                        int age;
                        age = Convert.ToInt32(DateTime.Now.Year - Convert.ToDateTime(stu.newlstget[i].DOB).Year);

                        if (age > 0)
                        {
                            age -= Convert.ToInt32(DateTime.Now < Convert.ToDateTime(stu.newlstget[i].DOB).Date.AddYears(age));
                        }
                        else
                        {
                            age = 0;
                        }
                        enq.PASR_Age = age; //not passing

                        enq.AMST_BloodGroup = stu.newlstget[i].BloodGroup;
                        enq.AMST_MotherTongue = stu.newlstget[i].MotherTongue;

                        //   var year = _DomainModelMsSqlServerContext.AcademicYear.Where(t => t.ASMAY_Year.Equals(stu.newlstget[i].year.TrimEnd().TrimStart()) && t.MI_Id == stu.MI_Id).FirstOrDefault();
                        enq.ASMAY_Id = Convert.ToInt64(stu.newlstget[i].year);
                        _acdimpl.LogInformation("entered year proc");
                        enq.IVRMMR_Id = 1;
                        enq.IMCC_Id = 1;
                        enq.IC_Id = 1;

                        enq.AMST_PerStreet = stu.newlstget[i].PermanentStreet;
                        enq.AMST_PerArea = stu.newlstget[i].PermanentArea;
                        enq.AMST_PerCity = stu.newlstget[i].PermanentCity;
                        enq.AMST_PerAdd3 = stu.newlstget[i].Permanentadd3;
                        //var perstate = _DomainModelMsSqlServerContext.State.Where(t => t.IVRMMS_Name == stu.newlstget[i].Permanentstate.TrimEnd().TrimStart()).FirstOrDefault();
                        enq.AMST_PerState = Convert.ToInt64(stu.newlstget[i].Permanentstate);

                        //var percountry = _DomainModelMsSqlServerContext.country.Where(t => t.IVRMMC_CountryName == stu.newlstget[i].PermanentCountry.TrimEnd().TrimStart()).FirstOrDefault();
                        //enq.AMST_PerCountry = Convert.ToInt64(stu.newlstget[i].PermanentCountry);
                        enq.AMST_PerPincode = Convert.ToInt32(stu.newlstget[i].PermanentPincode);



                        enq.AMST_ConStreet = stu.newlstget[i].PresentStreet;
                        enq.AMST_ConArea = stu.newlstget[i].PresentArea;
                        enq.AMST_ConCity = stu.newlstget[i].persentcity;

                        //var constate = _DomainModelMsSqlServerContext.State.Where(t => t.IVRMMS_Name == stu.newlstget[i].PresentState.TrimEnd().TrimStart()).FirstOrDefault();
                        enq.AMST_ConState = Convert.ToUInt16(stu.newlstget[i].PresentState);

                        //var concountry = _DomainModelMsSqlServerContext.country.Where(t => t.IVRMMC_CountryName == stu.newlstget[i].PresentCountry.TrimEnd().TrimStart()).FirstOrDefault();

                        enq.AMST_ConCountry = Convert.ToInt64(stu.newlstget[i].PresentCountry);
                        enq.AMST_ConPincode = Convert.ToInt32(stu.newlstget[i].PresentPincode);

                        enq.AMST_AadharNo = Convert.ToInt64(stu.newlstget[i].AadharNo);
                        enq.AMST_StuBankAccNo = stu.newlstget[i].BankAccountNo;
                        enq.AMST_StuBankIFSC_Code = stu.newlstget[i].IFSCCode;
                        enq.AMST_StuCasteCertiNo = stu.newlstget[i].CasteCertificateNo;
                        enq.AMST_MobileNo = Convert.ToInt64(stu.newlstget[i].MobileNo);
                        enq.AMST_emailId = stu.newlstget[i].EmailID;

                        enq.AMST_FatherAliveFlag = stu.newlstget[i].FatherAlive;
                        enq.AMST_FatherName = stu.newlstget[i].FatherName;
                        enq.AMST_FatherAadharNo = Convert.ToInt64(stu.newlstget[i].FatherAadharNo);
                        enq.AMST_FatherSurname = stu.newlstget[i].FatherSurname;
                        enq.AMST_FatherEducation = stu.newlstget[i].FatherEducation;
                        enq.AMST_FatherOccupation = stu.newlstget[i].FatherOccupation;
                        enq.AMST_FatherOfficeAdd = stu.newlstget[i].FatherOfficeAddress;
                        enq.AMST_FatherDesignation = stu.newlstget[i].FatherDesignation;
                        enq.AMST_FatherMonIncome = Convert.ToDecimal(stu.newlstget[i].FatherMonthlyIncome);
                        enq.AMST_FatherAnnIncome = Convert.ToDecimal(stu.newlstget[i].FatherAnnualIncome);
                        enq.AMST_FatherNationality = Convert.ToInt64(stu.newlstget[i].FatherNationality);
                        enq.AMST_FatherMobleNo = Convert.ToInt64(stu.newlstget[i].Fathermobileno);
                        enq.AMST_FatheremailId = stu.newlstget[i].FatherEmailId;
                        enq.AMST_FatherBankAccNo = stu.newlstget[i].FatherAccountNo;
                        enq.AMST_FatherBankIFSC_Code = stu.newlstget[i].FatherIFSCcode;
                        enq.AMST_FatherCasteCertiNo = stu.newlstget[i].FathercastecertificateNo;
                        enq.ANST_FatherPhoto = stu.newlstget[i].FatherPhoto;

                        enq.AMST_MotherAliveFlag = stu.newlstget[i].MotherAlive;
                        enq.AMST_MotherName = stu.newlstget[i].MotherName;
                        enq.AMST_MotherAadharNo = Convert.ToInt64(stu.newlstget[i].MotherAadharNo);
                        enq.AMST_MotherSurname = stu.newlstget[i].MotherSurname;
                        enq.AMST_MotherEducation = stu.newlstget[i].MotherEducation;
                        enq.AMST_MotherOccupation = stu.newlstget[i].MotherOcupation;
                        enq.AMST_MotherOfficeAdd = stu.newlstget[i].MotherOfficesAddress;
                        enq.AMST_MotherDesignation = stu.newlstget[i].MotherDesignation;
                        enq.AMST_MotherMonIncome = Convert.ToDecimal(stu.newlstget[i].MotherMonthlyIncome);
                        enq.AMST_MotherAnnIncome = Convert.ToDecimal(stu.newlstget[i].MotherAnnualIncome);
                        enq.AMST_MotherNationality = Convert.ToInt64(stu.newlstget[i].MotherNationality);
                        enq.AMST_MotherMobileNo = Convert.ToInt64(stu.newlstget[i].MotherMobileNo);
                        enq.AMST_MotherEmailId = stu.newlstget[i].MotherEmailId;
                        enq.AMST_MotherBankAccNo = stu.newlstget[i].MotherBankAccountNo;
                        enq.AMST_MotherBankIFSC_Code = stu.newlstget[i].MotherIFSCcode;
                        enq.AMST_MotherCasteCertiNo = stu.newlstget[i].MotherCasteCertificateNo;
                        enq.AMST_TotalIncome = Convert.ToDecimal(stu.newlstget[i].TotalIncome);
                        enq.ANST_MotherPhoto = stu.newlstget[i].MotherPhoto;

                        enq.AMST_BirthPlace = stu.newlstget[i].StudentBirthPlace;
                        enq.AMST_Nationality = Convert.ToInt32(stu.newlstget[i].StudentNationality);
                        enq.AMST_BPLCardFlag = Convert.ToInt32(stu.newlstget[i].BPLCardFlag);
                        enq.AMST_BPLCardNo = stu.newlstget[i].BPLCardFlag;

                        enq.AMST_HostelReqdFlag = Convert.ToInt32(stu.newlstget[i].HostelFacility);
                        enq.AMST_TransportReqdFlag = Convert.ToInt32(stu.newlstget[i].TransportFacility);
                        enq.AMST_GymReqdFlag = Convert.ToInt32(stu.newlstget[i].GymFacility);
                        enq.AMST_ECSFlag = Convert.ToInt32(stu.newlstget[i].ECSFlag);
                        enq.AMST_PaymentFlag = Convert.ToInt32(stu.newlstget[i].PaymentFlag);
                        enq.AMST_AmountPaid = Convert.ToInt32(stu.newlstget[i].AmountPaid);
                        enq.AMST_PaymentType = stu.newlstget[i].PaymentType;
                        enq.AMST_PaymentDate = Convert.ToDateTime(stu.newlstget[i].PaymentDate);
                        enq.AMST_ReceiptNo = stu.newlstget[i].ReceiptNo;
                        enq.AMST_ActiveFlag = Convert.ToInt32(stu.newlstget[i].ActiveFlag);
                        enq.AMST_ApplStatus = stu.newlstget[i].Applicationstatus;
                        enq.AMST_FinalpaymentFlag = Convert.ToInt32(stu.newlstget[i].FinalPaymentFlag);
                        enq.AMST_Noofsisters = Convert.ToInt32(stu.newlstget[i].Noofsisters);
                        enq.AMST_Noofbrothers = Convert.ToInt32(stu.newlstget[i].NoOfBrothers);
                        enq.CreatedDate = DateTime.Now;
                        enq.UpdatedDate = DateTime.Now;
                        enq.AMST_Photoname = stu.newlstget[i].PhotoName;
                        enq.AMST_SOL = "S";

                        _DomainModelMsSqlServerContext.Add(enq);
                        var flag = _DomainModelMsSqlServerContext.SaveChanges();
                        if (flag >= 1)
                        {
                            stu.stuStatus = "true";
                            sucesscount = sucesscount + 1;
                            generateOTP otp = new generateOTP();
                            if (enq.AMST_emailId != "" && enq.AMST_emailId != null)
                            {
                                string studotp = otp.getFourDigitOTP();
                                string StudentName = "";
                                if (enq.AMST_FirstName.Length > 4)
                                {
                                    StudentName = enq.AMST_FirstName.Substring(0, 3) + studotp;
                                }
                                else
                                {
                                    StudentName = enq.AMST_FirstName + studotp;

                                }

                                StudentName = Regex.Replace(StudentName, @"\s+", "");
                                string response = await Createlogins(enq.AMST_emailId, StudentName, enq.MI_Id, "Student", "student", enq.AMST_MobileNo.ToString());
                                bool val = AddStudentUserLogin(enq.MI_Id, StudentName, enq.AMST_Id);
                                if (response == "Success" && val == true && !enq.AMST_emailId.Equals("N"))
                                {
                                    if (enq.AMST_MobileNo.ToString() != "" && enq.AMST_MobileNo.ToString() != null)
                                    {
                                        SmsWithoutTemplate sms = new SmsWithoutTemplate(_db);
                                        string s = await sms.sendSms(enq.MI_Id, Convert.ToInt64(enq.AMST_MobileNo), StudentName, "Password@123");
                                    }
                                    EmailWithoutTemplate Email = new EmailWithoutTemplate(_db);
                                    string m = Email.EmailWthtTmp(enq.MI_Id, StudentName, enq.AMST_emailId, "Password@123");
                                }



                                studotp = "";
                            }
                            if (enq.AMST_FatheremailId != null)
                            {
                                string fathrotp = otp.getFourDigitOTPFather();
                                string FatherName = enq.AMST_FatherName.Substring(0, 4) + fathrotp;
                                FatherName = Regex.Replace(FatherName, @"\s+", "");
                                string response = await Createlogins(enq.AMST_FatheremailId, FatherName, enq.MI_Id, "PARENTS", "PARENTS", enq.AMST_FatherMobleNo.ToString());
                                bool val = AddStudentUserLogin(enq.MI_Id, FatherName, enq.AMST_Id);
                                if (response == "Success" && val == true && !enq.AMST_MotherEmailId.Equals("N"))
                                {
                                    if (enq.AMST_FatherMobleNo.ToString() != "" && enq.AMST_FatheremailId.ToString() != null)
                                    {
                                        SmsWithoutTemplate sms = new SmsWithoutTemplate(_db);
                                        string s = await sms.sendSms(enq.MI_Id, Convert.ToInt64(enq.AMST_FatherMobleNo), FatherName, "Password@123");
                                    }
                                    EmailWithoutTemplate Email = new EmailWithoutTemplate(_db);
                                    string m = Email.EmailWthtTmp(enq.MI_Id, FatherName, enq.AMST_FatheremailId, "Password@123");

                                }

                                fathrotp = "";
                            }
                            if (enq.AMST_MotherEmailId != null)
                            {
                                string motherotp = otp.getFourDigitOTPMother();
                                string MotherName = enq.AMST_MotherName.Substring(0, 4) + motherotp;
                                MotherName = Regex.Replace(MotherName, @"\s+", "");
                                string response = await Createlogins(enq.AMST_MotherEmailId, MotherName, enq.MI_Id, "PARENTS", "PARENTS", enq.AMST_MotherMobileNo.ToString());
                                bool val = AddStudentUserLogin(enq.MI_Id, MotherName, enq.AMST_Id);
                                if (response == "Success" && val == true && !enq.AMST_MotherEmailId.Equals("N"))
                                {
                                    if (enq.AMST_MotherMobileNo.ToString() != "" && enq.AMST_MotherMobileNo.ToString() != null)
                                    {
                                        SmsWithoutTemplate sms = new SmsWithoutTemplate(_db);
                                        string s = await sms.sendSms(enq.MI_Id, Convert.ToInt64(enq.AMST_MotherMobileNo), MotherName, "Password@123");
                                    }
                                    EmailWithoutTemplate Email = new EmailWithoutTemplate(_db);
                                    string m = Email.EmailWthtTmp(enq.MI_Id, MotherName, enq.AMST_MotherEmailId, "Password@123");

                                }
                                motherotp = "";
                            }
                        }
                        else
                        {
                            stu.stuStatus = "false";
                            failcount = failcount + 1;
                            string name = failnames;
                            finalnames += name + ",";


                        }

                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                        failcount = failcount + 1;
                        string name = failnames;
                        finalnames += name + ",";
                        //failnames = failnames + "," + failnames;
                        continue;

                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                stu.stuStatus = ex.Message;

            }

            stu.returnMsg += "Total Record Insert :'" + sucesscount + "'  , Total Records Failed : '" + failcount + "' And Failed List Names :' " + finalnames + "'";
            //catch (Exception e)
            //{
            //    stu.stuStatus = "false";
            //    Console.WriteLine(e.Message);
            //}

            return stu;
        }
        public async Task<string> Createlogins(string emailid, string name, long mi_id, string roles, string roletype, string mobile)
        {
            string resp = "";
            //Creating Student and parents login as well as Sending user name and password code starts.
            try
            {
                ApplicationUser user = new ApplicationUser();
                user = await _UserManager.FindByNameAsync(name);
                if (user == null)
                {
                    user = new ApplicationUser { UserName = name, Email = emailid, PhoneNumber = mobile };
                    user.Entry_Date = DateTime.Now;
                    user.EmailConfirmed = true;
                    var result = await _UserManager.CreateAsync(user, "Password@123");
                    if (result.Succeeded)
                    {
                        // Student Roles
                        string studentRole = roles;
                        var id = _db.applicationRole.Single(d => d.Name == studentRole);
                        //

                        // Student Role Type
                        string studentRoleType = roletype;
                        var id2 = _db.MasterRoleType.Where(d => d.IVRMRT_Role == studentRoleType);
                        //

                        // Save role
                        var role = new DataAccessMsSqlServerProvider.ApplicationUserRole { RoleId = Convert.ToInt32(id.Id), UserId = user.Id, RoleTypeId = Convert.ToInt64(id2.FirstOrDefault().IVRMRT_Id) };
                        role.CreatedDate = DateTime.Now;
                        role.UpdatedDate = DateTime.Now;
                        _db.appUserRole.Add(role);
                        _db.SaveChanges();

                        UserRoleWithInstituteDMO mas1 = new UserRoleWithInstituteDMO();
                        mas1.Id = user.Id;
                        mas1.MI_Id = mi_id;
                        _db.Add(mas1);
                        _db.SaveChanges();
                        resp = "Success";

                    }
                    else
                    {
                        resp = result.Errors.FirstOrDefault().Description.ToString();
                    }
                }
            }
            catch (Exception e)
            {
                _log.LogInformation("Student Admission form error");
                _log.LogDebug(e.Message);

            }
            return resp;

            //Creating Student and parents login as well as Sending user name and password code Ends.
        }
        public bool AddStudentUserLogin(long mi_id, string username, long amstId)
        {
            StudentUserLoginDMO dmo = new StudentUserLoginDMO();
            dmo.AMST_Id = amstId;
            dmo.CreatedDate = DateTime.Now;
            dmo.IVRMSTUUL_ActiveFlag = 1;
            dmo.IVRMSTUUL_Password = "Password@123";
            dmo.IVRMSTUUL_UserName = username;
            dmo.MI_Id = mi_id;
            dmo.UpdatedDate = DateTime.Now;
            _AdmissionFormContext.Add(dmo);
            var flag = _AdmissionFormContext.SaveChanges();
            if (flag > 0)
            {
                StudentUserLogin_Institutionwise inst = new StudentUserLogin_Institutionwise();
                inst.AMST_Id = amstId;
                inst.CreatedDate = DateTime.Now;
                inst.IVRMSTUULI_ActiveFlag = 1;
                inst.IVRMSTUUL_Id = dmo.IVRMSTUUL_Id;
                inst.UpdatedDate = DateTime.Now;
                _AdmissionFormContext.Add(inst);
                var flag1 = _AdmissionFormContext.SaveChanges();
                if (flag1 > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public async Task<ImportStudentWrapperDTO> getdetails(ImportStudentWrapperDTO stu)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            _acdimpl.LogInformation("entered try block");
            int sucesscount = 0;
            int failcount = 0;
            string failnames = "";
            string finalnames = "";
            string studentadmno = "Student Admission Number";
            int age = 0;


            try
            {
                List<ImportStudentDTO> failedlist = new List<ImportStudentDTO>();
                if (stu.newlstget1 != null && stu.newlstget1.Length > 0)
                {
                    for (int i = 0; i < stu.newlstget1.Length; i++)
                    {
                        try
                        {
                            if (stu.newlstget1[i].Status == "S")
                            {


                                _acdimpl.LogInformation("entered for loop");
                                Adm_M_Student enq = new Adm_M_Student();
                                enq.MI_Id = stu.MI_Id;
                                enq.AMST_ActiveFlag = 1;

                                var checkadmno = _AdmissionFormContext.Adm_M_Student.Where(a => a.MI_Id == stu.MI_Id
                                && a.AMST_AdmNo == stu.newlstget1[i].AMSTAdmNo).ToList();

                                if (checkadmno.Count == 0)
                                {
                                    if (stu.newlstget1[i].AMSTAdmNo != null && stu.newlstget1[i].AMSTAdmNo != "")
                                    {
                                        if ((Convert.ToString(stu.newlstget1[i].AMSTAdmNo.TrimEnd().TrimStart()) != null))
                                        {
                                            if ((Regex.IsMatch(Convert.ToString(stu.newlstget1[i].AMSTAdmNo.TrimEnd().TrimStart()), @"^[a-zA-Z0-9/\-@]*$")))
                                            {
                                                enq.AMST_AdmNo = stu.newlstget1[i].AMSTAdmNo;
                                            }
                                            else
                                            {
                                                stu.stuStatus = "Student Admission Number is Not Valid as It Should Contain Only Alphanumeric Characters " + stu.newlstget1[i].AMSTAdmNo;
                                                return stu;
                                            }
                                        }
                                        else
                                        {
                                            stu.stuStatus = "Student Admission Number can not be null or Empty";
                                            return stu;
                                        }
                                    }


                                    long yearid = 0;
                                    try
                                    {
                                        var year = "";
                                        if (stu.newlstget1[i].year != null && stu.newlstget1[i].year != "")
                                        {
                                            year = stu.newlstget1[i].year.Trim().ToString();
                                            if ((year != null))
                                            {
                                                if ((Regex.IsMatch(Convert.ToString(year), @"^[0-9\s-]*$")))
                                                {
                                                    var check_yearid = _AdmissionFormContext.year.Where(t => t.ASMAY_Year.Equals(year) && t.MI_Id == stu.MI_Id).ToList();
                                                    if (check_yearid.Count() > 0)
                                                    {
                                                        enq.ASMAY_Id = check_yearid.FirstOrDefault().ASMAY_Id;
                                                        yearid = check_yearid.FirstOrDefault().ASMAY_Id;
                                                    }
                                                    else
                                                    {
                                                        stu.stuStatus = "Year is Not Available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                                        return stu;
                                                    }
                                                }
                                                else
                                                {
                                                    stu.stuStatus = "Student Year is Not Valid as It Should Contain Only numeric with -";
                                                    return stu;
                                                }
                                            }
                                            else
                                            {
                                                stu.stuStatus = "Student Year can not be null or Empty For   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                                return stu;
                                            }
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        stu.stuStatus = "StudentYear can not be null or Empty   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                        return stu;
                                    }

                                    //--------Register Number ------------//
                                    try
                                    {
                                        var regno = "";
                                        if (stu.newlstget1[i].AMSTRegistrationNo != null && stu.newlstget1[i].AMSTRegistrationNo != "")
                                        {
                                            regno = stu.newlstget1[i].AMSTRegistrationNo.Trim().ToString();
                                            if ((regno != null))
                                            {
                                                if ((Regex.IsMatch(Convert.ToString(regno), @"^[a-zA-Z0-9/\-@]*$")))
                                                {
                                                    enq.AMST_RegistrationNo = stu.newlstget1[i].AMSTRegistrationNo;
                                                }

                                                else
                                                {
                                                    stu.stuStatus = "Student Registration Number is Not Valid as It Should Contain Only Alphanumeric Characters";
                                                    return stu;
                                                }
                                            }
                                            else
                                            {
                                                stu.stuStatus = "Student Registration Number can not be null or Empty For   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                                return stu;
                                            }
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        stu.stuStatus = "Student Registration Number can not be null or Empty For   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                        return stu;
                                    }
                                    if (stu.newlstget1[i].FirstName != null && stu.newlstget1[i].FirstName != "")
                                    {
                                        if ((stu.newlstget1[i].FirstName.TrimEnd().TrimStart() != null) && (stu.newlstget1[i].FirstName.TrimEnd().TrimStart() != ""))
                                        {
                                            if (Regex.IsMatch(stu.newlstget1[i].FirstName.TrimEnd().TrimStart(), @"^[a-zA-Z.\s]+$"))
                                            {
                                                enq.AMST_FirstName = stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                            }

                                            else
                                            {
                                                stu.stuStatus = "Student First Name is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + "Student Admission Number";
                                                return stu;
                                            }
                                        }
                                        else
                                        {
                                            stu.stuStatus = "Student First Name can not be null for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + "Student Admission Number";
                                            return stu;
                                        }
                                    }

                                    var middlename = "";
                                    if (stu.newlstget1[i].MiddleName != null && stu.newlstget1[i].MiddleName != "")
                                    {
                                        middlename = stu.newlstget1[i].MiddleName == null || stu.newlstget1[i].MiddleName.ToUpper() == "NULL" ? "" :
                                       stu.newlstget1[i].MiddleName.TrimEnd().TrimStart();

                                        enq.AMST_MiddleName = "";

                                        if ((Regex.IsMatch(middlename, @"^[a-zA-Z.\s]+$")))
                                        {
                                            enq.AMST_MiddleName = stu.newlstget1[i].MiddleName.TrimEnd().TrimStart();
                                        }
                                        else
                                        {
                                            enq.AMST_MiddleName = stu.newlstget1[i].MiddleName.TrimEnd().TrimStart();
                                        }
                                    }


                                    var lastname = "";
                                    if (stu.newlstget1[i].LastName != null && stu.newlstget1[i].LastName != "")
                                    {
                                        lastname = stu.newlstget1[i].LastName == null || stu.newlstget1[i].LastName.ToUpper() == "NULL" ? "" :
                                    stu.newlstget1[i].LastName.TrimEnd().TrimStart();
                                        enq.AMST_LastName = "";

                                        if (((Regex.IsMatch(lastname, @"^[a-zA-Z.\s]+$"))))
                                        {
                                            enq.AMST_LastName = stu.newlstget1[i].LastName.TrimEnd().TrimStart();
                                        }
                                    }


                                    //---Date Of Admissiom----//

                                    var dateFormats = new[] { "dd.MM.yyyy", "dd-MM-yyyy", "dd/MM/yyyy" };

                                    DateTime scheduleDate_DOB;
                                    DateTime scheduleDate_DOJ;
                                    string readAddMeeting = "";
                                    bool validDate = false;
                                    if (stu.newlstget1[i].amstdate != null && stu.newlstget1[i].amstdate != "")
                                    {
                                        readAddMeeting = stu.newlstget1[i].amstdate.TrimEnd().TrimStart();

                                        validDate = DateTime.TryParseExact(readAddMeeting, dateFormats, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out scheduleDate_DOJ);
                                        if (validDate)
                                        {
                                            enq.AMST_Date = scheduleDate_DOJ;
                                        }
                                        else
                                        {
                                            stu.stuStatus = "Date Of Join is not Valid, Please Enter in dd/MM/yyyy format for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                            return stu;
                                        }
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Date Of Join is Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }


                                    //----gender---//
                                    string gender = "";
                                    if (stu.newlstget1[i].Gender != null && stu.newlstget1[i].Gender != "")
                                    {
                                        gender = stu.newlstget1[i].Gender.TrimEnd().TrimStart();

                                        if (gender != null && gender != "")
                                        {
                                            if (((Regex.IsMatch(gender, @"^([a-zA-Z/s])*$")) && (gender.Equals("Male") || gender.Equals("Female") || gender.Equals("Other") || (gender.Equals("MALE") || gender.Equals("FEMALE"))) && gender.Length <= 6))
                                            {
                                                enq.AMST_Sex = gender;
                                            }
                                            else
                                            {
                                                stu.stuStatus = "Sudent Gender is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                                return stu;
                                            }
                                        }
                                        else
                                        {
                                            stu.stuStatus = "Sudent Gender is required for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                            return stu;
                                        }
                                    }


                                    //--Date Of Birth--//
                                    if (stu.newlstget1[i].DOB != null && stu.newlstget1[i].DOB != "")
                                    {
                                        string us = "";
                                        //readAddMeeting = stu.newlstget1[i].DOB.TrimEnd().TrimStart();
                                        //validDate = DateTime.TryParseExact(
                                        //   readAddMeeting,
                                        //   dateFormats,
                                        //   DateTimeFormatInfo.InvariantInfo,
                                        //   DateTimeStyles.None,
                                        //   out scheduleDate_DOB);
                                        //if (validDate)
                                        //{
                                        //    //enq.AMST_DOB = DateTime.ParseExact(scheduleDate_DOB.ToString(), "dd/MM/yyyy", null); 

                                        //    //enq.AMST_DOB = stu.newlstget1[i].DOB;



                                        //}
                                        //else
                                        //{
                                        //    stu.stuStatus = "Date Of Birth is not Valid, Please Enter in dd/MM/yyyy format for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        //    return stu;
                                        //}
                                        //enq.AMST_DOB = Convert.ToDateTime(stu.newlstget1[i].DOB.ToString());
                                        readAddMeeting = stu.newlstget1[i].DOB.TrimEnd().TrimStart();
                                        validDate = DateTime.TryParseExact(readAddMeeting, dateFormats, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out scheduleDate_DOB);
                                        if (validDate)
                                        {
                                            enq.AMST_DOB = scheduleDate_DOB;
                                        }
                                        else
                                        {
                                            stu.stuStatus = "Date Of Birth is not Valid, Please Enter in dd/MM/yyyy format for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                            return stu;
                                        }
                                        //enq.AMST_DOB = DateTime.ParseExact(stu.newlstget1[i].DOB, "dd/MM/yyyy", null);

                                    }



                                    //--DOB Words --//
                                    if (stu.newlstget1[i].AMSTDOBWords != null && stu.newlstget1[i].AMSTDOBWords != "")
                                    {
                                        if (stu.newlstget1[i].AMSTDOBWords.TrimEnd().TrimStart() != null && stu.newlstget1[i].AMSTDOBWords.TrimEnd().TrimStart() != "")
                                        {
                                            enq.AMST_DOB_Words = stu.newlstget1[i].AMSTDOBWords.TrimEnd().TrimStart();
                                        }
                                        else
                                        {
                                            enq.AMST_DOB_Words = stu.newlstget1[i].AMSTDOBWords.TrimEnd().TrimStart();
                                        }
                                    }


                                    DateTime fromdatecon = DateTime.Now;
                                    string confromdate = "";
                                    if (stu.newlstget1[i].DOB != null && stu.newlstget1[i].DOB != "")
                                    {
                                        fromdatecon = enq.AMST_DOB;
                                        confromdate = fromdatecon.ToString("yyyy-MM-dd");
                                    }


                                    //--Age calculation--//
                                    if (confromdate != null && confromdate != "")
                                    {
                                        age = Convert.ToInt32(DateTime.Now.Year - Convert.ToDateTime(confromdate).Year);
                                        if (age > 0)
                                        {
                                            age -= Convert.ToInt32(DateTime.Now < Convert.ToDateTime(confromdate).Date.AddYears(age));
                                        }
                                        else
                                        {
                                            age = 0;
                                        }

                                        enq.PASR_Age = age;
                                    }

                                    long classid;

                                    //--getting class id from class name --//

                                    var check_class = _AdmissionFormContext.School_M_Class.Where(t => t.MI_Id == stu.MI_Id
                                    && t.ASMCL_ClassName.TrimEnd().TrimStart().ToLower() == stu.newlstget1[i].Class.TrimEnd().TrimStart().ToLower()).ToList();
                                    if (check_class.Count > 0)
                                    {
                                        var class_id = _AdmissionFormContext.School_M_Class.Where(t => t.MI_Id == stu.MI_Id
                                        && t.ASMCL_ClassName.TrimEnd().TrimStart().ToLower() == stu.newlstget1[i].Class.TrimEnd().TrimStart().ToLower()).FirstOrDefault();
                                        enq.ASMCL_Id = class_id.ASMCL_Id;
                                        classid = class_id.ASMCL_Id;
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Class is Not Available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }


                                    //----getting class category id---//                       
                                    var get_class_category = (from a in _AdmissionFormContext.Masterclasscategory
                                                              from b in _AdmissionFormContext.Adm_M_Stu_Cat
                                                              where (a.AMC_Id == b.AMC_Id && a.MI_Id == stu.MI_Id && a.ASMAY_Id == yearid && a.ASMCL_Id == classid)
                                                              select new
                                                              {
                                                                  asmcc_id = a.ASMCC_Id
                                                              }).ToList();

                                    if (get_class_category.Count > 0)
                                    {
                                        enq.AMC_Id = get_class_category.FirstOrDefault().asmcc_id;
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Class Category is Not Available for this acadmic year" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    }
                                    if (stu.newlstget1[i].BloodGroup != null && stu.newlstget1[i].BloodGroup != "")
                                    {
                                        enq.AMST_BloodGroup = stu.newlstget1[i].BloodGroup;
                                    }



                                    //----Mother Tongue----//
                                    if (stu.newlstget1[i].MotherTongue != null && stu.newlstget1[i].MotherTongue != "")
                                    {
                                        if (((Regex.IsMatch(stu.newlstget1[i].MotherTongue.TrimEnd().TrimStart(), @"^[a-zA-Z\s]+$"))
                                       || (stu.newlstget1[i].MotherTongue.TrimEnd().TrimStart() == null) || (stu.newlstget1[i].MotherTongue.TrimEnd().TrimStart() == "")))
                                        {
                                            enq.AMST_MotherTongue = stu.newlstget1[i].MotherTongue.TrimEnd().TrimStart();
                                        }

                                    }

                                    //---Religion---//
                                    if (stu.newlstget1[i].Religion != null && stu.newlstget1[i].Religion != "")
                                    {
                                        if (((Regex.IsMatch(stu.newlstget1[i].Religion, @"^[a-zA-Z\s]+$"))))
                                        {
                                            var get_religionid = _AdmissionFormContext.Religion.Where(t => t.IVRMMR_Name.Equals(stu.newlstget1[i].Religion.TrimEnd().TrimStart())).ToList();
                                            if (get_religionid.Count() > 0)
                                            {
                                                enq.IVRMMR_Id = get_religionid.FirstOrDefault().IVRMMR_Id;
                                            }
                                            else
                                            {
                                                stu.stuStatus = "Student Religion is not available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                                return stu;
                                            }
                                        }
                                        else
                                        {
                                            stu.stuStatus = "Student Religion is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                            return stu;
                                        }

                                    }



                                    //--caste --//
                                    long get_casteid1 = 0;
                                    if (stu.newlstget1[i].Caste != null && stu.newlstget1[i].Caste != "")
                                    {
                                        if (((Regex.IsMatch(stu.newlstget1[i].Caste, @"^[a-zA-Z\s]+$"))))
                                        {
                                            var get_casteid = _AdmissionFormContext.Caste.Where(t => t.IMC_CasteName.Equals(stu.newlstget1[i].Caste.TrimEnd().TrimStart()) && t.MI_Id == stu.MI_Id).ToList();
                                            if (get_casteid.Count() > 0)
                                            {
                                                enq.IC_Id = get_casteid.FirstOrDefault().IMC_Id;
                                                get_casteid1 = get_casteid.FirstOrDefault().IMC_Id;
                                            }
                                            else
                                            {
                                                stu.stuStatus = "Student caste is not available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                                return stu;
                                            }
                                        }
                                        else
                                        {
                                            stu.stuStatus = "Student caste is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                            return stu;
                                        }
                                    }


                                    //geting caste category id
                                    var castecategoryid = "";
                                    if (stu.newlstget1[i].Caste != null && stu.newlstget1[i].Caste != "")
                                    {
                                        if (((Regex.IsMatch(stu.newlstget1[i].Caste, @"^[a-zA-Z0-9\s]+$"))))
                                        {
                                            var get_castecategoryid = (from a in _AdmissionFormContext.CasteCategory
                                                                       from b in _AdmissionFormContext.Caste
                                                                       where (a.IMCC_Id == b.IMCC_Id && b.MI_Id == stu.MI_Id && b.IMC_Id == Convert.ToInt64(get_casteid1))
                                                                       select new
                                                                       {
                                                                           castecategoryid = a.IMCC_Id
                                                                       }).ToList();
                                            if (get_castecategoryid.Count() > 0)
                                            {
                                                enq.IMCC_Id = get_castecategoryid.FirstOrDefault().castecategoryid;
                                            }
                                            else
                                            {
                                                stu.stuStatus = "Student caste category is not available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                                return stu;
                                            }
                                        }
                                        else
                                        {
                                            stu.stuStatus = "Student caste category is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                            return stu;
                                        }

                                    }

                                    //----------------Permanent Address--------------------//

                                    if (stu.newlstget1[i].PermanentStreet != null && stu.newlstget1[i].PermanentStreet != "")
                                    {
                                        enq.AMST_PerStreet = stu.newlstget1[i].PermanentStreet.TrimEnd().TrimStart();
                                    }
                                    if (stu.newlstget1[i].PermanentArea != null && stu.newlstget1[i].PermanentArea != "")
                                    {
                                        enq.AMST_PerArea = stu.newlstget1[i].PermanentArea.TrimEnd().TrimStart();
                                    }
                                    if (stu.newlstget1[i].PermanentCity != null && stu.newlstget1[i].PermanentCity != "")
                                    {
                                        enq.AMST_PerCity = stu.newlstget1[i].PermanentCity.TrimEnd().TrimStart();
                                    }



                                    // enq.AMST_PerCountry = null;
                                    enq.AMST_PerState = null;
                                    enq.AMST_PerPincode = null;

                                    //permanent country
                                    long countryid = 0;
                                    if (stu.newlstget1[i].PermanentCountry != null && stu.newlstget1[i].PermanentCountry != "")
                                    {
                                        if (stu.newlstget1[i].PermanentCountry.TrimEnd().TrimStart() != null && stu.newlstget1[i].PermanentCountry.TrimEnd().TrimStart() != "")
                                        {
                                            if (Regex.IsMatch(stu.newlstget1[i].PermanentCountry.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\.\:\,/])*$"))
                                            {
                                                var get_countryid = _AdmissionFormContext.Country.Where(t => t.IVRMMC_CountryName.Equals(stu.newlstget1[i].PermanentCountry.TrimEnd().TrimStart()));
                                                if (get_countryid.Count() > 0)
                                                {
                                                    // enq.AMST_PerCountry = get_countryid.FirstOrDefault().IVRMMC_Id;
                                                    countryid = get_countryid.FirstOrDefault().IVRMMC_Id;
                                                }
                                                else
                                                {
                                                    stu.stuStatus = "Permanent Country input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                                    return stu;
                                                }
                                            }
                                        }
                                    }


                                    //permanent state

                                    if (stu.newlstget1[i].Permanentstate != null && stu.newlstget1[i].Permanentstate != "" && countryid > 0)
                                    {
                                        if (stu.newlstget1[i].Permanentstate.TrimEnd().TrimStart() != null && stu.newlstget1[i].Permanentstate.TrimEnd().TrimStart() != "")
                                        {
                                            if (((Regex.IsMatch(stu.newlstget1[i].Permanentstate.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\.\:\,/])*$")) && ((stu.newlstget1[i].Permanentstate.TrimEnd().TrimStart()).Length <= 100)))
                                            {
                                                var get_countryid = _AdmissionFormContext.State.Where(t => t.IVRMMS_Name.Equals(stu.newlstget1[i].Permanentstate.TrimEnd().TrimStart()) && t.IVRMMC_Id == countryid);
                                                if (get_countryid.Count() > 0)
                                                {
                                                    enq.AMST_PerState = get_countryid.FirstOrDefault().IVRMMS_Id;
                                                }
                                                else
                                                {
                                                    stu.stuStatus = "Permanent state input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                                    return stu;
                                                }
                                            }
                                            else
                                            {
                                                stu.stuStatus = "Permanent state input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                                return stu;
                                            }
                                        }

                                    }



                                    //Permanent Pincode 

                                    if (stu.newlstget1[i].PermanentPincode != null && stu.newlstget1[i].PermanentPincode != "")
                                    {
                                        if (Convert.ToString(stu.newlstget1[i].PermanentPincode.TrimEnd().TrimStart()) != null && Convert.ToString(stu.newlstget1[i].PermanentPincode.TrimEnd().TrimStart()) != "")
                                        {
                                            if (((Regex.IsMatch(Convert.ToString(stu.newlstget1[i].PermanentPincode.TrimEnd().TrimStart()), @"^([0-9])*$"))
                                                && (Convert.ToString(stu.newlstget1[i].PermanentPincode.TrimEnd().TrimStart()).Length == 6)))
                                            {
                                                enq.AMST_PerPincode = Convert.ToInt32(stu.newlstget1[i].PermanentPincode.TrimEnd().TrimStart());
                                            }
                                            else
                                            {
                                                stu.stuStatus = "Permanent Pincode is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                                return stu;
                                            }
                                        }
                                    }

                                    //---End of the permanent address------//


                                    //-------------------------Communication Address--------------------------//
                                    if (stu.newlstget1[i].PresentStreet != null && stu.newlstget1[i].PresentStreet != "")
                                    {
                                        enq.AMST_ConStreet = stu.newlstget1[i].PresentStreet.TrimEnd().TrimStart();
                                    }
                                    if (stu.newlstget1[i].PresentArea != null && stu.newlstget1[i].PresentArea != "")
                                    {
                                        enq.AMST_ConArea = stu.newlstget1[i].PresentArea.TrimEnd().TrimStart();
                                    }
                                    if (stu.newlstget1[i].PresentCity != null && stu.newlstget1[i].PresentCity != "")
                                    {
                                        enq.AMST_ConCity = stu.newlstget1[i].PresentCity.TrimEnd().TrimStart();

                                    }



                                    enq.AMST_ConCountry = null;
                                    enq.AMST_ConState = null;
                                    enq.AMST_ConPincode = null;

                                    //persent country
                                    long countryid1 = 0;
                                    if (stu.newlstget1[i].PresentCountry != null && stu.newlstget1[i].PresentCountry != "")
                                    {
                                        if (stu.newlstget1[i].PresentCountry.TrimEnd().TrimStart() != null && stu.newlstget1[i].PresentCountry.TrimEnd().TrimStart() != "")
                                        {
                                            if (Regex.IsMatch(stu.newlstget1[i].PresentCountry.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\.\:\,/])*$"))
                                            {
                                                var get_countryid = _AdmissionFormContext.Country.Where(t => t.IVRMMC_CountryName.Equals(stu.newlstget1[i].PresentCountry.TrimEnd().TrimStart()));
                                                if (get_countryid.Count() > 0)
                                                {
                                                    enq.AMST_ConCountry = get_countryid.FirstOrDefault().IVRMMC_Id;
                                                    countryid1 = get_countryid.FirstOrDefault().IVRMMC_Id;
                                                }
                                                else
                                                {
                                                    stu.stuStatus = "Present Country input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                                    return stu;
                                                }
                                            }
                                        }
                                    }


                                    //present state
                                    if (stu.newlstget1[i].PresentState != null && stu.newlstget1[i].PresentState != "")
                                    {
                                        if (stu.newlstget1[i].PresentState.TrimEnd().TrimStart() != null && stu.newlstget1[i].PresentState.TrimEnd().TrimStart() != "")
                                        {
                                            if (((Regex.IsMatch(stu.newlstget1[i].PresentState.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\.\:\,/])*$")) && ((stu.newlstget1[i].PresentState.TrimEnd().TrimStart()).Length <= 100)))
                                            {
                                                var get_countryid = _AdmissionFormContext.State.Where(t => t.IVRMMS_Name.Equals(stu.newlstget1[i].PresentState.TrimEnd().TrimStart()) && t.IVRMMC_Id == countryid1);
                                                if (get_countryid.Count() > 0)
                                                {
                                                    enq.AMST_ConState = get_countryid.FirstOrDefault().IVRMMS_Id;
                                                }
                                                else
                                                {
                                                    stu.stuStatus = "present state input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                                    return stu;
                                                }
                                            }
                                        }
                                    }


                                    //present Pincode 
                                    if (stu.newlstget1[i].PresentPincode != null && stu.newlstget1[i].PresentPincode != "")
                                    {
                                        if (Convert.ToString(stu.newlstget1[i].PresentPincode.TrimEnd().TrimStart()) != null
                                       && Convert.ToString(stu.newlstget1[i].PresentPincode.TrimEnd().TrimStart()) != "")
                                        {
                                            if (((Regex.IsMatch(Convert.ToString(stu.newlstget1[i].PresentPincode.TrimEnd().TrimStart()), @"^([0-9])*$")) && (Convert.ToString(stu.newlstget1[i].PresentPincode.TrimEnd().TrimStart()).Length == 6)))
                                            {
                                                enq.AMST_ConPincode = Convert.ToInt32(stu.newlstget1[i].PresentPincode.TrimEnd().TrimStart());
                                            }
                                            else
                                            {
                                                stu.stuStatus = "Present Pincode is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                                return stu;
                                            }
                                        }
                                    }


                                    //---End of the Communication address------//

                                    //-------------Aadhar Number -----------//
                                    enq.AMST_AadharNo = null;
                                    if (stu.newlstget1[i].AadharNo != null && stu.newlstget1[i].AadharNo != "")
                                    {
                                        if (stu.newlstget1[i].AadharNo.TrimEnd().TrimStart() != null && stu.newlstget1[i].AadharNo.TrimEnd().TrimStart() != "")
                                        {
                                            if (((Regex.IsMatch((stu.newlstget1[i].AadharNo.TrimEnd().TrimStart()), @"^([0-9])*$"))
                                                && ((stu.newlstget1[i].AadharNo.TrimEnd().TrimStart()).Length >= 0))
                                                && ((stu.newlstget1[i].AadharNo.TrimEnd().TrimStart()).Length <= 12))
                                            {
                                                enq.AMST_AadharNo = Convert.ToInt64(stu.newlstget1[i].AadharNo.TrimEnd().TrimStart());
                                            }
                                        }
                                    }


                                    //--- Student Mobile Number --//
                                    if (stu.newlstget1[i].MobileNo != null && stu.newlstget1[i].MobileNo != "")
                                    {
                                        if ((Convert.ToString(stu.newlstget1[i].MobileNo.TrimEnd().TrimStart()) != null)
                                  && (Convert.ToString(stu.newlstget1[i].MobileNo.TrimEnd().TrimStart()) != ""))
                                        {
                                            string MatchPhoneNumberPattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
                                            if (Regex.IsMatch((Convert.ToString(stu.newlstget1[i].MobileNo.TrimEnd().TrimStart())), MatchPhoneNumberPattern))
                                            {
                                                enq.AMST_MobileNo = Convert.ToInt64(stu.newlstget1[i].MobileNo.TrimEnd().TrimStart());
                                            }
                                            else
                                            {
                                                stu.stuStatus = "Mobile Number is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                                return stu;
                                            }
                                        }
                                        else
                                        {
                                            stu.stuStatus = "Mobile Number can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                            return stu;
                                        }
                                    }


                                    //--- Student Email id--//
                                    if (stu.newlstget1[i].EmailID != null && stu.newlstget1[i].EmailID != "")
                                    {
                                        if ((Convert.ToString(stu.newlstget1[i].EmailID.TrimEnd().TrimStart()) != null) && (Convert.ToString(stu.newlstget1[i].EmailID.TrimEnd().TrimStart()) != ""))
                                        {
                                            Match match = regex.Match(stu.newlstget1[i].EmailID.TrimEnd().TrimStart());
                                            if (match.Success)
                                            {
                                                enq.AMST_emailId = stu.newlstget1[i].EmailID.TrimEnd().TrimStart();
                                            }
                                            else
                                            {
                                                stu.stuStatus = "Email Id is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                                return stu;
                                            }
                                        }
                                        else
                                        {
                                            stu.stuStatus = "Email Id can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                            return stu;
                                        }
                                    }


                                    //----------------Father Details-------------//

                                    enq.AMST_FatherAliveFlag = "1";
                                    if (stu.newlstget1[i].FatherAlive != null && stu.newlstget1[i].FatherAlive != "")
                                    {
                                        enq.AMST_FatherAliveFlag = stu.newlstget1[i].FatherAlive;
                                    }
                                    if (stu.newlstget1[i].FatherName != null && stu.newlstget1[i].FatherName != "")
                                    {
                                        enq.AMST_FatherName = stu.newlstget1[i].FatherName;
                                    }
                                    if (stu.newlstget1[i].FatherAadharNo != null && stu.newlstget1[i].FatherAadharNo != "")
                                    {
                                        enq.AMST_FatherAadharNo = Convert.ToInt64(stu.newlstget1[i].FatherAadharNo);
                                    }
                                    if (stu.newlstget1[i].FatherSurname != null && stu.newlstget1[i].FatherSurname != "")
                                    {

                                        enq.AMST_FatherSurname = stu.newlstget1[i].FatherSurname;
                                    }
                                    if (stu.newlstget1[i].FatherEducation != null && stu.newlstget1[i].FatherEducation != "")
                                    {
                                        enq.AMST_FatherEducation = stu.newlstget1[i].FatherEducation;
                                    }
                                    if (stu.newlstget1[i].FatherOccupation != null && stu.newlstget1[i].FatherOccupation != "")
                                    {
                                        enq.AMST_FatherOccupation = stu.newlstget1[i].FatherOccupation;
                                    }
                                    if (stu.newlstget1[i].FatherOfficeAdd != null && stu.newlstget1[i].FatherOfficeAdd != "")
                                    {
                                        enq.AMST_FatherOfficeAdd = stu.newlstget1[i].FatherOfficeAdd;

                                    }
                                    if (stu.newlstget1[i].FatherDesignation != null && stu.newlstget1[i].FatherDesignation != "")
                                    {
                                        enq.AMST_FatherDesignation = stu.newlstget1[i].FatherDesignation;

                                    }
                                    if (stu.newlstget1[i].FatherMonthlyIncome != null && stu.newlstget1[i].FatherMonthlyIncome != "")
                                    {
                                        enq.AMST_FatherMonIncome = Convert.ToDecimal(stu.newlstget1[i].FatherMonthlyIncome);

                                    }
                                    if (stu.newlstget1[i].FatherAnnualIncome != null && stu.newlstget1[i].FatherAnnualIncome != "")
                                    {
                                        enq.AMST_FatherAnnIncome = Convert.ToDecimal(stu.newlstget1[i].FatherAnnualIncome);

                                    }


                                    if (stu.newlstget1[i].FatherNationality != null)
                                    {
                                        var fathernationality1 = _AdmissionFormContext.Country.Where(t => t.IVRMMC_CountryName == stu.newlstget1[i].FatherNationality.TrimEnd().TrimStart()).ToList();
                                        enq.AMST_FatherNationality = fathernationality1.Count > 0 ? fathernationality1.FirstOrDefault().IVRMMC_Id : (long?)null;
                                    }

                                    if (stu.newlstget1[i].FatherBankAccNo != null && stu.newlstget1[i].FatherBankAccNo != "")
                                    {
                                        enq.AMST_FatherBankAccNo = stu.newlstget1[i].FatherBankAccNo;

                                    }
                                    if (stu.newlstget1[i].FatherBankIFSC_Code != null && stu.newlstget1[i].FatherBankIFSC_Code != "")
                                    {
                                        enq.AMST_FatherBankIFSC_Code = stu.newlstget1[i].FatherBankIFSC_Code;

                                    }
                                    if (stu.newlstget1[i].FatherCasteCertiNo != null && stu.newlstget1[i].FatherCasteCertiNo != "")
                                    {
                                        enq.AMST_FatherCasteCertiNo = stu.newlstget1[i].FatherCasteCertiNo;

                                    }
                                    if (stu.newlstget1[i].FatherPhoto != null && stu.newlstget1[i].FatherPhoto != "")
                                    {
                                        enq.ANST_FatherPhoto = stu.newlstget1[i].FatherPhoto;

                                    }

                                    if (stu.newlstget1[i].FatherPANNo != null && stu.newlstget1[i].FatherPANNo != "")
                                    {

                                        enq.AMST_FatherPANNo = stu.newlstget1[i].FatherPANNo;

                                    }


                                    if (stu.newlstget1[i].FatherReligion != null)
                                    {
                                        var fatherreligion = _AdmissionFormContext.Religion.Where(t => t.IVRMMR_Name == stu.newlstget1[i].FatherReligion.TrimEnd().TrimStart()).ToList();

                                        enq.AMST_FatherReligion = fatherreligion.Count > 0 ? fatherreligion.FirstOrDefault().IVRMMR_Id.ToString() : null;

                                    }
                                    if (stu.newlstget1[i].FatherCaste != null)
                                    {
                                        var fathercaste = _AdmissionFormContext.Caste.Where(t => t.IMC_CasteName == stu.newlstget1[i].FatherCaste.TrimEnd().TrimStart()).ToList();
                                        enq.AMST_FatherCaste = fathercaste.Count > 0 ? fathercaste.FirstOrDefault().IMC_Id.ToString() : null;

                                    }
                                    if (stu.newlstget1[i].FatherSubCaste != null)
                                    {
                                        enq.AMST_FatherSubCaste = stu.newlstget1[i].FatherSubCaste;

                                    }





                                    if (stu.newlstget1[i].Fathermobileno != null && stu.newlstget1[i].Fathermobileno != "")
                                    {
                                        if ((Convert.ToString(stu.newlstget1[i].Fathermobileno.TrimEnd().TrimStart()) != null) && (Convert.ToString(stu.newlstget1[i].Fathermobileno.TrimEnd().TrimStart()) != ""))
                                        {
                                            string MatchPhoneNumberPattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
                                            if (Regex.IsMatch((Convert.ToString(stu.newlstget1[i].Fathermobileno.TrimEnd().TrimStart())), MatchPhoneNumberPattern))
                                            {
                                                enq.AMST_FatherMobleNo = Convert.ToInt64(stu.newlstget1[i].Fathermobileno.TrimEnd().TrimStart());
                                            }
                                            else
                                            {
                                                stu.stuStatus = "Father Mobile Number is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                                return stu;
                                            }
                                        }
                                    }

                                    if (stu.newlstget1[i].FatherEmailId != null && stu.newlstget1[i].FatherEmailId != "")
                                    {
                                        if ((Convert.ToString(stu.newlstget1[i].FatherEmailId.TrimEnd().TrimStart()) != null) && (Convert.ToString(stu.newlstget1[i].FatherEmailId.TrimEnd().TrimStart()) != ""))
                                        {
                                            Match match = regex.Match(stu.newlstget1[i].FatherEmailId.TrimEnd().TrimStart());
                                            if (match.Success)
                                            {
                                                enq.AMST_FatheremailId = stu.newlstget1[i].FatherEmailId.TrimEnd().TrimStart();
                                            }
                                            else
                                            {
                                                stu.stuStatus = "Father Email Id is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                                return stu;
                                            }
                                        }
                                    }


                                    //-------Mother Details ----------//

                                    enq.AMST_MotherAliveFlag = "1";
                                    if (stu.newlstget1[i].MotherName != null && stu.newlstget1[i].MotherName != "")
                                    {
                                        enq.AMST_MotherName = stu.newlstget1[i].MotherName.TrimEnd().TrimStart();
                                    }
                                    if (stu.newlstget1[i].MotherAadharNo != null && stu.newlstget1[i].MotherAadharNo != "")
                                    {
                                        enq.AMST_MotherAadharNo = Convert.ToInt64(stu.newlstget1[i].MotherAadharNo);
                                    }
                                    if (stu.newlstget1[i].MotherSurname != null && stu.newlstget1[i].MotherSurname != "")
                                    {
                                        enq.AMST_MotherSurname = stu.newlstget1[i].MotherSurname;
                                    }
                                    if (stu.newlstget1[i].MotherEducation != null && stu.newlstget1[i].MotherEducation != "")
                                    {
                                        enq.AMST_MotherEducation = stu.newlstget1[i].MotherEducation;
                                    }
                                    if (stu.newlstget1[i].MotherOccupation != null && stu.newlstget1[i].MotherOccupation != "")
                                    {
                                        enq.AMST_MotherOccupation = stu.newlstget1[i].MotherOccupation;
                                    }
                                    if (stu.newlstget1[i].MotherOfficeAddress != null && stu.newlstget1[i].MotherOfficeAddress != "")
                                    {
                                        enq.AMST_MotherOfficeAdd = stu.newlstget1[i].MotherOfficeAddress;
                                    }
                                    if (stu.newlstget1[i].MotherDesignation != null && stu.newlstget1[i].MotherDesignation != "")
                                    {
                                        enq.AMST_MotherDesignation = stu.newlstget1[i].MotherDesignation;
                                    }
                                    if (stu.newlstget1[i].MotherMonthlyIncome != null && stu.newlstget1[i].MotherMonthlyIncome != "")
                                    {
                                        enq.AMST_MotherMonIncome = Convert.ToDecimal(stu.newlstget1[i].MotherMonthlyIncome);
                                    }
                                    if (stu.newlstget1[i].MotherAnnualIncome != null && stu.newlstget1[i].MotherAnnualIncome != "")
                                    {
                                        enq.AMST_MotherAnnIncome = Convert.ToDecimal(stu.newlstget1[i].MotherAnnualIncome);
                                    }


                                    if (stu.newlstget1[i].MotherNationality != null)
                                    {
                                        var mothernationality = _AdmissionFormContext.Country.Where(t => t.IVRMMC_CountryName == stu.newlstget1[i].MotherNationality.TrimEnd().TrimStart()).ToList();
                                        enq.AMST_MotherNationality = mothernationality.Count > 0 ? mothernationality.FirstOrDefault().IVRMMC_Id : (long?)null;


                                    }

                                    if (stu.newlstget1[i].MotherBankAccNo != null && stu.newlstget1[i].MotherBankAccNo != "")
                                    {
                                        enq.AMST_MotherBankAccNo = stu.newlstget1[i].MotherBankAccNo;
                                    }
                                    if (stu.newlstget1[i].MotherBankIFSC_Code != null && stu.newlstget1[i].MotherBankIFSC_Code != "")
                                    {
                                        enq.AMST_MotherBankIFSC_Code = stu.newlstget1[i].MotherBankIFSC_Code;
                                    }
                                    if (stu.newlstget1[i].MotherCasteCertiNo != null && stu.newlstget1[i].MotherCasteCertiNo != "")
                                    {
                                        enq.AMST_MotherCasteCertiNo = stu.newlstget1[i].MotherCasteCertiNo;
                                    }
                                    if (stu.newlstget1[i].MotherPhoto != null && stu.newlstget1[i].MotherPhoto != "")
                                    {
                                        enq.ANST_MotherPhoto = stu.newlstget1[i].MotherPhoto;
                                    }
                                    if (stu.newlstget1[i].MotherPANNo != null && stu.newlstget1[i].MotherPANNo != "")
                                    {
                                        enq.AMST_MotherPANNo = stu.newlstget1[i].MotherPANNo;
                                    }






                                    if (stu.newlstget1[i].MotherReligion != null)
                                    {
                                        var motherreligion = _AdmissionFormContext.Religion.Where(t => t.IVRMMR_Name == stu.newlstget1[i].MotherReligion.TrimEnd().TrimStart()).ToList();

                                        enq.AMST_MotherReligion = motherreligion.Count > 0 ? motherreligion.FirstOrDefault().IVRMMR_Id.ToString() : null;

                                    }
                                    if (stu.newlstget1[i].MotherCaste != null)
                                    {
                                        var mothercaste = _AdmissionFormContext.Caste.Where(t => t.IMC_CasteName == stu.newlstget1[i].MotherCaste.TrimEnd().TrimStart()).ToList();
                                        enq.AMST_MotherCaste = mothercaste.Count > 0 ? mothercaste.FirstOrDefault().IMC_Id.ToString() : null;

                                    }

                                    if (stu.newlstget1[i].MotherSubCaste != null && stu.newlstget1[i].MotherSubCaste != "")
                                    {
                                        enq.AMST_MotherSubCaste = stu.newlstget1[i].MotherSubCaste;
                                    }

                                    if (stu.newlstget1[i].MotherMobileNo != null && stu.newlstget1[i].MotherMobileNo != "")
                                    {
                                        if ((Convert.ToString(stu.newlstget1[i].MotherMobileNo.TrimEnd().TrimStart()) != null) && (Convert.ToString(stu.newlstget1[i].MotherMobileNo.TrimEnd().TrimStart()) != ""))
                                        {
                                            string MatchPhoneNumberPattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
                                            if (Regex.IsMatch((Convert.ToString(stu.newlstget1[i].MotherMobileNo.TrimEnd().TrimStart())), MatchPhoneNumberPattern))
                                            {
                                                enq.AMST_MotherMobileNo = Convert.ToInt64(stu.newlstget1[i].MotherMobileNo.TrimEnd().TrimStart());
                                            }
                                            else
                                            {
                                                stu.stuStatus = "Mother Mobile Number is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                                return stu;
                                            }
                                        }

                                    }
                                    if (stu.newlstget1[i].MotherEmailId != null && stu.newlstget1[i].MotherEmailId != "")
                                    {
                                        if ((Convert.ToString(stu.newlstget1[i].MotherEmailId.TrimEnd().TrimStart()) != null)
                                        && (Convert.ToString(stu.newlstget1[i].MotherEmailId.TrimEnd().TrimStart()) != ""))
                                        {

                                            Match match = regex.Match(stu.newlstget1[i].MotherEmailId.TrimEnd().TrimStart());
                                            if (match.Success)
                                            {
                                                enq.AMST_MotherEmailId = stu.newlstget1[i].MotherEmailId.TrimEnd().TrimStart();
                                            }
                                            else
                                            {
                                                stu.stuStatus = "Mother Email Id is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                                return stu;
                                            }
                                        }
                                        else
                                        {
                                            stu.stuStatus = "Mother Email Id can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                            return stu;
                                        }
                                    }



                                    //student nationality
                                    if (stu.newlstget1[i].StudentNationality != null && stu.newlstget1[i].StudentNationality != "")
                                    {
                                        if (stu.newlstget1[i].StudentNationality.TrimEnd().TrimStart() != null
                                      && stu.newlstget1[i].StudentNationality.TrimEnd().TrimStart() != "")
                                        {
                                            if (Regex.IsMatch(stu.newlstget1[i].StudentNationality.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\:\,])*$"))
                                            {
                                                var get_countryid = _AdmissionFormContext.Country.Where(t => t.IVRMMC_CountryName.Equals(stu.newlstget1[i].StudentNationality.TrimEnd().TrimStart()));
                                                if (get_countryid.Count() > 0)
                                                {
                                                    enq.AMST_Nationality = get_countryid.FirstOrDefault().IVRMMC_Id;
                                                    countryid1 = get_countryid.FirstOrDefault().IVRMMC_Id;
                                                }
                                                else
                                                {
                                                    stu.stuStatus = "student Nationality input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                                    return stu;
                                                }
                                            }
                                            else
                                            {
                                                stu.stuStatus = "student Nationality input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                                return stu;
                                            }
                                        }
                                        else
                                        {
                                            stu.stuStatus = "student Nationality Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                            return stu;
                                        }
                                    }


                                    var concession = _AdmissionFormContext.Fee_Master_ConcessionDMO.Where(a => a.MI_Id == stu.MI_Id && a.FMCC_ConcessionName.Equals("General") && a.FMCC_ActiveFlag == true).ToList();
                                    if (concession.Count > 0)
                                    {
                                        enq.AMST_Concession_Type = concession.FirstOrDefault().FMCC_Id;
                                    }
                                    else
                                    {
                                        stu.stuStatus = "student Concession Type Is Not There Please Map The Fee Concession Type For Student And Concession Type Should Be 'General' Concession Type";
                                        return stu;
                                    }
                                    if (stu.newlstget1[i].BirthCertNO != null && stu.newlstget1[i].BirthCertNO != "")
                                    {
                                        enq.AMST_BirthCertNO = stu.newlstget1[i].BirthCertNO;
                                    }
                                    if (stu.newlstget1[i].StudentBankAccNo != null && stu.newlstget1[i].StudentBankAccNo != "")
                                    {
                                        enq.AMST_StuBankAccNo = stu.newlstget1[i].StudentBankAccNo;
                                    }
                                    if (stu.newlstget1[i].StudentBankIFSC_Code != null && stu.newlstget1[i].StudentBankIFSC_Code != "")
                                    {
                                        enq.AMST_StuBankIFSC_Code = stu.newlstget1[i].StudentBankIFSC_Code;
                                    }
                                    if (stu.newlstget1[i].StuCasteCertiNo != null && stu.newlstget1[i].StuCasteCertiNo != "")
                                    {
                                        enq.AMST_StuCasteCertiNo = stu.newlstget1[i].StuCasteCertiNo;
                                    }
                                    if (stu.newlstget1[i].BirthPlace != null && stu.newlstget1[i].BirthPlace != "")
                                    {
                                        enq.AMST_BirthPlace = stu.newlstget1[i].BirthPlace;
                                    }
                                    if (stu.newlstget1[i].StudentSubCaste != null && stu.newlstget1[i].StudentSubCaste != "")
                                    {
                                        enq.AMST_SubCasteIMC_Id = stu.newlstget1[i].StudentSubCaste;
                                    }
                                    if (stu.newlstget1[i].StudentPANNo != null && stu.newlstget1[i].StudentPANNo != "")
                                    {
                                        enq.AMST_StudentPANNo = stu.newlstget1[i].StudentPANNo;
                                    }
                                    if (stu.newlstget1[i].HostelReqdFlag != null && stu.newlstget1[i].HostelReqdFlag != "")
                                    {
                                        enq.AMST_HostelReqdFlag = stu.newlstget1[i].HostelReqdFlag == "Yes" ? 1 : 0;
                                    }
                                    if (stu.newlstget1[i].HostelReqdFlag != null && stu.newlstget1[i].HostelReqdFlag != "")
                                    {
                                        enq.AMST_HostelReqdFlag = stu.newlstget1[i].HostelReqdFlag == "Yes" ? 1 : 0;
                                    }

                                    if (stu.newlstget1[i].TransportReqdFlag != null && stu.newlstget1[i].TransportReqdFlag != "")
                                    {

                                        enq.AMST_TransportReqdFlag = stu.newlstget1[i].TransportReqdFlag == "Yes" ? 1 : 0;
                                    }
                                    if (stu.newlstget1[i].GymReqdFlag != null && stu.newlstget1[i].GymReqdFlag != "")
                                    {

                                        enq.AMST_GymReqdFlag = stu.newlstget1[i].GymReqdFlag == "Yes" ? 1 : 0;
                                    }
                                    if (stu.newlstget1[i].ECSFlag != null && stu.newlstget1[i].ECSFlag != "")
                                    {

                                        enq.AMST_ECSFlag = stu.newlstget1[i].ECSFlag == "Yes" ? 1 : 0;
                                    }
                                    if (stu.newlstget1[i].BPLCardNo != null && stu.newlstget1[i].BPLCardNo != "")
                                    {
                                        enq.AMST_BPLCardNo = stu.newlstget1[i].BPLCardNo;
                                    }
                                    if (stu.newlstget1[i].LanguageSpoken != null && stu.newlstget1[i].LanguageSpoken != "")
                                    {
                                        enq.AMST_LanguageSpoken = stu.newlstget1[i].LanguageSpoken;
                                    }
                                    if (stu.newlstget1[i].Noofsisters != null && stu.newlstget1[i].Noofsisters != "")
                                    {
                                        enq.AMST_Noofsisters = Convert.ToInt32(stu.newlstget1[i].Noofsisters);
                                    }
                                    if (stu.newlstget1[i].NoOfBrothers != null && stu.newlstget1[i].NoOfBrothers != "")
                                    {
                                        enq.AMST_Noofbrothers = Convert.ToInt32(stu.newlstget1[i].NoOfBrothers);
                                    }
                                    if (stu.newlstget1[i].NoOfElderSisters != null && stu.newlstget1[i].NoOfElderSisters != "")
                                    {
                                        enq.AMST_NoOfElderSisters = Convert.ToInt32(stu.newlstget1[i].NoOfElderSisters);
                                    }
                                    if (stu.newlstget1[i].NoOfElderBrothers != null && stu.newlstget1[i].NoOfElderBrothers != "")
                                    {
                                        enq.AMST_NoOfElderBrothers = Convert.ToInt32(stu.newlstget1[i].NoOfElderBrothers);
                                    }
                                    if (stu.newlstget1[i].NoOfYoungerSisters != null && stu.newlstget1[i].NoOfYoungerSisters != "")
                                    {
                                        enq.AMST_NoOfYoungerSisters = Convert.ToInt32(stu.newlstget1[i].NoOfYoungerSisters);
                                    }
                                    if (stu.newlstget1[i].NoOfYoungerBrothers != null && stu.newlstget1[i].NoOfYoungerBrothers != "")
                                    {
                                        enq.AMST_NoOfYoungerBrothers = Convert.ToInt32(stu.newlstget1[i].NoOfYoungerBrothers);
                                    }
                                    if (stu.newlstget1[i].Status != null && stu.newlstget1[i].Status != "")
                                    {
                                        enq.AMST_SOL = stu.newlstget1[i].Status;

                                        enq.AMST_ActiveFlag = stu.newlstget1[i].Status == "S" ? 1 : 0;
                                    }

                                    enq.CreatedDate = DateTime.Now;
                                    enq.UpdatedDate = DateTime.Now;
                                    enq.AMST_UpdatedBy = stu.User_Id;
                                    enq.AMST_CreatedBy = stu.User_Id;

                                    _DomainModelMsSqlServerContext.Add(enq);
                                    //var flag = _DomainModelMsSqlServerContext.SaveChanges();
                                    //if (flag >= 1)
                                    //{

                                    if (enq.AMST_FatheremailId != null && enq.AMST_FatheremailId != "")
                                    {
                                        try
                                        {
                                            Adm_Master_Father_Email enq_faheremail = new Adm_Master_Father_Email
                                            {
                                                AMST_FatheremailId = enq.AMST_FatheremailId,
                                                MI_Id = stu.MI_Id,
                                                AMST_Id = enq.AMST_Id,
                                                CreatedDate = DateTime.Now,
                                                UpdatedDate = DateTime.Now
                                            };
                                            _DomainModelMsSqlServerContext.Add(enq_faheremail);

                                            // int n = _DomainModelMsSqlServerContext.SaveChanges();
                                        }
                                        catch (Exception ex)
                                        {
                                            _acdimpl.LogInformation("Father emailid saving in new table" + ex.Message);
                                            stu.stuStatus = ex.Message;
                                        }
                                    }



                                    if (enq.AMST_MotherEmailId != null && enq.AMST_MotherEmailId != "")
                                    {
                                        try
                                        {
                                            Adm_M_Mother_Emailid enq_motheemail = new Adm_M_Mother_Emailid
                                            {
                                                AMST_MotheremailId = enq.AMST_MotherEmailId,
                                                AMST_Id = enq.AMST_Id,
                                                MI_Id = stu.MI_Id,
                                                CreatedDate = DateTime.Now,
                                                UpdatedDate = DateTime.Now
                                            };
                                            _DomainModelMsSqlServerContext.Add(enq_motheemail);
                                            // int n1 = _DomainModelMsSqlServerContext.SaveChanges();
                                        }
                                        catch (Exception ex)
                                        {
                                            _acdimpl.LogInformation("Mother emailid saving in new table" + ex.Message);
                                            stu.stuStatus = ex.Message;
                                        }
                                    }



                                    if (enq.AMST_MotherMobileNo != null)
                                    {
                                        try
                                        {
                                            Adm_M_Mother_MobileNo enq_mothermobile = new Adm_M_Mother_MobileNo
                                            {
                                                AMST_MotherMobileNo = enq.AMST_MotherMobileNo,
                                                AMST_Id = enq.AMST_Id,
                                                MI_Id = stu.MI_Id,
                                                CreatedDate = DateTime.Now,
                                                UpdatedDate = DateTime.Now
                                            };
                                            _DomainModelMsSqlServerContext.Add(enq_mothermobile);
                                            // int n2 = _DomainModelMsSqlServerContext.SaveChanges();
                                        }
                                        catch (Exception ex)
                                        {
                                            _acdimpl.LogInformation("Mother Mobileno saving in new table" + ex.Message);
                                            stu.stuStatus = ex.Message;
                                        }
                                    }




                                    if (enq.AMST_FatherMobleNo != null)
                                    {
                                        try
                                        {
                                            Adm_M_Student_FatherMobileNo enq_fathermobile = new Adm_M_Student_FatherMobileNo
                                            {
                                                AMST_Id = enq.AMST_Id,
                                                MI_Id = stu.MI_Id,
                                                AMST_FatherMobile_No = Convert.ToInt64(enq.AMST_FatherMobleNo),
                                                CreatedDate = DateTime.Now,
                                                UpdatedDate = DateTime.Now
                                            };
                                            _DomainModelMsSqlServerContext.Add(enq_fathermobile);
                                            // int n3 = _DomainModelMsSqlServerContext.SaveChanges();
                                        }
                                        catch (Exception ex)
                                        {
                                            _acdimpl.LogInformation("Father Mobileno saving in new table" + ex.Message);
                                            stu.stuStatus = ex.Message;
                                        }
                                    }




                                    if (stu.newlstget1[i].MobileNo != null && stu.newlstget1[i].MobileNo != "")
                                    {
                                        try
                                        {
                                            Adm_M_Student_MobileNo enq_studentmobile = new Adm_M_Student_MobileNo
                                            {
                                                AMST_Id = enq.AMST_Id,
                                                AMSTSMS_MobileNo = enq.AMST_MobileNo.ToString(),
                                                CreatedDate = DateTime.Now,
                                                UpdatedDate = DateTime.Now
                                            };
                                            _DomainModelMsSqlServerContext.Add(enq_studentmobile);
                                            // int n4 = _DomainModelMsSqlServerContext.SaveChanges();
                                        }
                                        catch (Exception ex)
                                        {
                                            _acdimpl.LogInformation("Student Mobileno saving in new table" + ex.Message);
                                            stu.stuStatus = ex.Message;
                                        }
                                    }




                                    if (enq.AMST_emailId != null && enq.AMST_emailId != "")
                                    {
                                        try
                                        {
                                            Adm_M_Student_Email_Id enq_studentemail = new Adm_M_Student_Email_Id
                                            {
                                                AMST_Id = enq.AMST_Id,
                                                AMSTE_EmailId = enq.AMST_emailId,
                                                CreatedDate = DateTime.Now,
                                                UpdatedDate = DateTime.Now
                                            };
                                            _DomainModelMsSqlServerContext.Add(enq_studentemail);
                                            //int n5 = _DomainModelMsSqlServerContext.SaveChanges();
                                        }
                                        catch (Exception ex)
                                        {
                                            _acdimpl.LogInformation("Student Email saving in new table" + ex.Message);
                                            stu.stuStatus = ex.Message;
                                        }
                                    }



                                    long yearid_current = 0;
                                    long classid_current = 0;
                                    long section_current = 0;
                                    try
                                    {
                                        var year = "";
                                        if (stu.newlstget1[i].CurrentYear != null && stu.newlstget1[i].CurrentYear != "")
                                        {
                                            year = stu.newlstget1[i].CurrentYear.Trim().ToString();

                                            if ((year != null))
                                            {
                                                if ((Regex.IsMatch(Convert.ToString(year), @"^[0-9\s-]*$")))
                                                {
                                                    var check_yearid = _AdmissionFormContext.year.Where(t => t.ASMAY_Year.Equals(year) && t.MI_Id == stu.MI_Id).ToList();
                                                    if (check_yearid.Count() > 0)
                                                    {
                                                        yearid_current = check_yearid.FirstOrDefault().ASMAY_Id;
                                                    }
                                                    else
                                                    {
                                                        stu.stuStatus = "Student Current Year is Not Available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                                        return stu;
                                                    }
                                                }
                                                else
                                                {
                                                    stu.stuStatus = "Student Year is Not Valid as It Should Contain Only numeric with -";
                                                    return stu;
                                                }
                                            }
                                            else
                                            {
                                                stu.stuStatus = "Student Current Year can not be null or Empty For   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                                return stu;
                                            }
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        stu.stuStatus = "Student Current Year can not be null or Empty   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                        return stu;
                                    }

                                    try
                                    {
                                        var current_class = "";
                                        if (stu.newlstget1[i].CurrentClass != null && stu.newlstget1[i].CurrentClass != "")
                                        {
                                            current_class = stu.newlstget1[i].CurrentClass.Trim().ToString();

                                            if (current_class != null)
                                            {
                                                var check_classid = _AdmissionFormContext.School_M_Class.Where(t => t.ASMCL_ClassName.Equals(current_class)
                                                && t.MI_Id == stu.MI_Id).ToList();
                                                if (check_classid.Count() > 0)
                                                {
                                                    classid_current = check_classid.FirstOrDefault().ASMCL_Id;
                                                }
                                                else
                                                {
                                                    stu.stuStatus = "Student Current Class is Not Available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                                    return stu;
                                                }
                                            }
                                            else
                                            {
                                                stu.stuStatus = "Student Current Class can not be null or Empty For   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                                return stu;
                                            }
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        stu.stuStatus = "Student Current Class can not be null or Empty   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                        return stu;
                                    }

                                    try
                                    {
                                        var current_section = "";
                                        if (stu.newlstget1[i].CurrentSection != null && stu.newlstget1[i].CurrentSection != "")
                                        {
                                            current_section = stu.newlstget1[i].CurrentSection.Trim().ToString();

                                            if (current_section != null)
                                            {
                                                var check_sectionid = _AdmissionFormContext.AdmSection.Where(t => t.ASMC_SectionName.Equals(current_section)
                                                && t.MI_Id == stu.MI_Id).ToList();
                                                if (check_sectionid.Count() > 0)
                                                {
                                                    section_current = check_sectionid.FirstOrDefault().ASMS_Id;
                                                }
                                                else
                                                {
                                                    stu.stuStatus = "Student Current Section is Not Available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                                    return stu;
                                                }
                                            }
                                            else
                                            {
                                                stu.stuStatus = "Student Current Section can not be null or Empty For   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                                return stu;
                                            }
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                        stu.stuStatus = "Student Current Section can not be null or Empty   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                        return stu;
                                    }

                                    if (yearid_current > 0 && classid_current > 0 && section_current > 0)
                                    {
                                        School_Adm_Y_StudentDMO school_Adm_Y_StudentDMO = new School_Adm_Y_StudentDMO();
                                        school_Adm_Y_StudentDMO.ASMAY_Id = yearid_current;
                                        school_Adm_Y_StudentDMO.ASMCL_Id = classid_current;
                                        school_Adm_Y_StudentDMO.ASMS_Id = section_current;
                                        school_Adm_Y_StudentDMO.AMST_Id = enq.AMST_Id;
                                        school_Adm_Y_StudentDMO.AMAY_RollNo = 1;
                                        school_Adm_Y_StudentDMO.AMAY_ActiveFlag = 1;
                                        school_Adm_Y_StudentDMO.LoginId = stu.User_Id;
                                        school_Adm_Y_StudentDMO.ASYST_CreatedBy = stu.User_Id;
                                        school_Adm_Y_StudentDMO.ASYST_UpdatedBy = stu.User_Id;
                                        school_Adm_Y_StudentDMO.AMAY_DateTime = DateTime.Now;
                                        school_Adm_Y_StudentDMO.CreatedDate = DateTime.Now;
                                        school_Adm_Y_StudentDMO.UpdatedDate = DateTime.Now;
                                        _DomainModelMsSqlServerContext.Add(school_Adm_Y_StudentDMO);

                                        sucesscount = sucesscount + 1;

                                        //var id+=1;
                                        //var i1 = _AdmissionFormContext.SaveChanges();
                                        //var id = _DomainModelMsSqlServerContext.SaveChanges();
                                        stu.stuStatus = "true";
                                    }


                                    if (sucesscount > 0)
                                    {
                                        if (stu.stuStatus == "true")
                                        {
                                            int id = _DomainModelMsSqlServerContext.SaveChanges();
                                        }


                                        //if (id > 0)
                                        //{
                                        //    int id = _DomainModelMsSqlServerContext.SaveChanges();
                                        //    stu.stuStatus = "true";
                                        //}
                                        //else
                                        //{
                                        //    stu.stuStatus = "fail";
                                        //}

                                    }


                                    //}
                                    //else
                                    //{
                                    //    stu.stuStatus = "false";
                                    //    failcount = failcount + 1;
                                    //    string name = failnames;
                                    //    finalnames += name + ",";
                                    //    failedlist.Add(stu.newlstget1[i]);
                                    //}
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Status header is not matching or not mentioned in the file";
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.Write(ex.Message);
                            failcount = failcount + 1;
                            string name = failnames;
                            finalnames += name + ",";
                            failedlist.Add(stu.newlstget1[i]);
                            stu.stuStatus = ex.Message;
                            continue;
                        }
                    }
                }


                stu.failedlist = failedlist.ToArray();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                stu.stuStatus = ex.Message + sucesscount;
            }

            if (finalnames != "" && failcount > 0)
            {
                stu.returnMsg += "Total Record Insert :'" + sucesscount + "'  , Total Records Failed : '" + failcount + "' And Failed List Names :' " + finalnames + "'";
            }
            else
            {
                stu.returnMsg += "Total Record Insert :'" + sucesscount + "'  , Total Records Failed : '" + 0 + "'";
            }

            return stu;
        }
        //public async Task<ImportStudentWrapperDTO> getdetails(ImportStudentWrapperDTO stu)
        //{
        //    Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        //    _acdimpl.LogInformation("entered try block");
        //    int sucesscount = 0;
        //    int failcount = 0;
        //    string failnames = "";
        //    string finalnames = "";
        //    string studentadmno = "Student Admission Number";
        //    int age = 0;
        //    try
        //    {
        //        List<ImportStudentDTO> failedlist = new List<ImportStudentDTO>();
        //        if (stu.newlstget1 != null && stu.newlstget1.Length > 0)
        //        {
        //            for (int i = 0; i < stu.newlstget1.Length; i++)
        //            {
        //                try
        //                {
        //                    _acdimpl.LogInformation("entered for loop");
        //                    Adm_M_Student enq = new Adm_M_Student();
        //                    enq.MI_Id = stu.MI_Id;
        //                    enq.AMST_ActiveFlag = 1;

        //                    var checkadmno = _AdmissionFormContext.Adm_M_Student.Where(a => a.MI_Id == stu.MI_Id
        //                    && a.AMST_AdmNo == stu.newlstget1[i].AMSTAdmNo).ToList();

        //                    if (checkadmno.Count == 0)
        //                    {
        //                        if ((Convert.ToString(stu.newlstget1[i].AMSTAdmNo.TrimEnd().TrimStart()) != null))
        //                        {
        //                            if ((Regex.IsMatch(Convert.ToString(stu.newlstget1[i].AMSTAdmNo.TrimEnd().TrimStart()), @"^[a-zA-Z0-9/\-@]*$")))
        //                            {
        //                                enq.AMST_AdmNo = stu.newlstget1[i].AMSTAdmNo;
        //                            }
        //                            else
        //                            {
        //                                stu.stuStatus = "Student Admission Number is Not Valid as It Should Contain Only Alphanumeric Characters " + stu.newlstget1[i].AMSTAdmNo;
        //                                return stu;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            stu.stuStatus = "Student Admission Number can not be null or Empty";
        //                            return stu;
        //                        }

        //                        long yearid = 0;
        //                        try
        //                        {
        //                            var year = "";
        //                            year = stu.newlstget1[i].year.Trim().ToString();
        //                            if ((year != null))
        //                            {
        //                                if ((Regex.IsMatch(Convert.ToString(year), @"^[0-9\s-]*$")))
        //                                {
        //                                    var check_yearid = _AdmissionFormContext.year.Where(t => t.ASMAY_Year.Equals(year) && t.MI_Id == stu.MI_Id).ToList();
        //                                    if (check_yearid.Count() > 0)
        //                                    {
        //                                        enq.ASMAY_Id = check_yearid.FirstOrDefault().ASMAY_Id;
        //                                        yearid = check_yearid.FirstOrDefault().ASMAY_Id;
        //                                    }
        //                                    else
        //                                    {
        //                                        stu.stuStatus = "Year is Not Available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
        //                                        return stu;
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    stu.stuStatus = "Student Year is Not Valid as It Should Contain Only numeric with -";
        //                                    return stu;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                stu.stuStatus = "Student Year can not be null or Empty For   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
        //                                return stu;
        //                            }
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            Console.WriteLine(ex.Message);
        //                            stu.stuStatus = "StudentYear can not be null or Empty   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
        //                            return stu;
        //                        }

        //                        //--------Register Number ------------//
        //                        try
        //                        {
        //                            var regno = "";
        //                            regno = stu.newlstget1[i].AMSTRegistrationNo.Trim().ToString();
        //                            if ((regno != null))
        //                            {
        //                                if ((Regex.IsMatch(Convert.ToString(regno), @"^[a-zA-Z0-9/\-@]*$")))
        //                                {
        //                                    enq.AMST_RegistrationNo = stu.newlstget1[i].AMSTRegistrationNo;
        //                                }

        //                                else
        //                                {
        //                                    stu.stuStatus = "Student Registration Number is Not Valid as It Should Contain Only Alphanumeric Characters";
        //                                    return stu;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                stu.stuStatus = "Student Registration Number can not be null or Empty For   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
        //                                return stu;
        //                            }
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            Console.WriteLine(ex.Message);
        //                            stu.stuStatus = "Student Registration Number can not be null or Empty For   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
        //                            return stu;
        //                        }

        //                        if ((stu.newlstget1[i].FirstName.TrimEnd().TrimStart() != null) && (stu.newlstget1[i].FirstName.TrimEnd().TrimStart() != ""))
        //                        {
        //                            if (Regex.IsMatch(stu.newlstget1[i].FirstName.TrimEnd().TrimStart(), @"^[a-zA-Z.\s]+$"))
        //                            {
        //                                enq.AMST_FirstName = stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
        //                            }

        //                            else
        //                            {
        //                                stu.stuStatus = "Student First Name is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + "Student Admission Number";
        //                                return stu;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            stu.stuStatus = "Student First Name can not be null for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + "Student Admission Number";
        //                            return stu;
        //                        }

        //                        var middlename = stu.newlstget1[i].MiddleName == null || stu.newlstget1[i].MiddleName.ToUpper() == "NULL" ? "" :
        //                            stu.newlstget1[i].MiddleName.TrimEnd().TrimStart();

        //                        enq.AMST_MiddleName = "";

        //                        if ((Regex.IsMatch(middlename, @"^[a-zA-Z.\s]+$")))
        //                        {
        //                            enq.AMST_MiddleName = stu.newlstget1[i].MiddleName.TrimEnd().TrimStart();
        //                        }


        //                        var lastname = stu.newlstget1[i].LastName == null || stu.newlstget1[i].LastName.ToUpper() == "NULL" ? "" :
        //                            stu.newlstget1[i].LastName.TrimEnd().TrimStart();
        //                        enq.AMST_LastName = "";

        //                        if (((Regex.IsMatch(lastname, @"^[a-zA-Z.\s]+$"))))
        //                        {
        //                            enq.AMST_LastName = stu.newlstget1[i].LastName.TrimEnd().TrimStart();
        //                        }

        //                        //---Date Of Admissiom----//

        //                        var dateFormats = new[] { "dd.MM.yyyy", "dd-MM-yyyy", "dd/MM/yyyy" };

        //                        DateTime scheduleDate_DOB;
        //                        DateTime scheduleDate_DOJ;

        //                        string readAddMeeting = stu.newlstget1[i].amstdate.TrimEnd().TrimStart();

        //                        bool validDate = DateTime.TryParseExact(readAddMeeting, dateFormats, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out scheduleDate_DOJ);
        //                        if (validDate)
        //                        {
        //                            enq.AMST_Date = scheduleDate_DOJ;
        //                        }
        //                        else
        //                        {
        //                            stu.stuStatus = "Date Of Join is not Valid, Please Enter in dd/MM/yyyy format for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
        //                            return stu;
        //                        }

        //                        //----gender---//
        //                        string gender = stu.newlstget1[i].Gender.TrimEnd().TrimStart();

        //                        if (gender != null && gender != "")
        //                        {
        //                            if (((Regex.IsMatch(gender, @"^([a-zA-Z/s])*$")) && (gender.Equals("Male") || gender.Equals("Female") || gender.Equals("Other")) && gender.Length <= 6))
        //                            {
        //                                enq.AMST_Sex = gender;
        //                            }
        //                            else
        //                            {
        //                                stu.stuStatus = "Sudent Gender is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
        //                                return stu;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            stu.stuStatus = "Sudent Gender is required for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
        //                            return stu;
        //                        }

        //                        //--Date Of Birth--//
        //                        readAddMeeting = stu.newlstget1[i].DOB.TrimEnd().TrimStart();
        //                        validDate = DateTime.TryParseExact(
        //                           readAddMeeting,
        //                           dateFormats,
        //                           DateTimeFormatInfo.InvariantInfo,
        //                           DateTimeStyles.None,
        //                           out scheduleDate_DOB);
        //                        if (validDate)
        //                        {
        //                            enq.AMST_DOB = scheduleDate_DOB;
        //                        }
        //                        else
        //                        {
        //                            stu.stuStatus = "Date Of Birth is not Valid, Please Enter in dd/MM/yyyy format for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
        //                            return stu;
        //                        }


        //                        //--DOB Words --//
        //                        if (stu.newlstget1[i].AMSTDOBWords.TrimEnd().TrimStart() != null && stu.newlstget1[i].AMSTDOBWords.TrimEnd().TrimStart() != "")
        //                        {
        //                            enq.AMST_DOB_Words = stu.newlstget1[i].AMSTDOBWords.TrimEnd().TrimStart();
        //                        }
        //                        else
        //                        {
        //                            enq.AMST_DOB_Words = stu.newlstget1[i].AMSTDOBWords.TrimEnd().TrimStart();
        //                        }

        //                        DateTime fromdatecon = DateTime.Now;
        //                        string confromdate = "";
        //                        fromdatecon = Convert.ToDateTime(stu.newlstget1[i].DOB);
        //                        confromdate = fromdatecon.ToString("yyyy-MM-dd");

        //                        //--Age calculation--//
        //                        age = Convert.ToInt32(DateTime.Now.Year - Convert.ToDateTime(confromdate).Year);
        //                        if (age > 0)
        //                        {
        //                            age -= Convert.ToInt32(DateTime.Now < Convert.ToDateTime(confromdate).Date.AddYears(age));
        //                        }
        //                        else
        //                        {
        //                            age = 0;
        //                        }

        //                        enq.PASR_Age = age;
        //                        long classid;

        //                        //--getting class id from class name --//
        //                        var check_class = _AdmissionFormContext.School_M_Class.Where(t => t.MI_Id == stu.MI_Id
        //                        && t.ASMCL_ClassName.TrimEnd().TrimStart().ToLower() == stu.newlstget1[i].Class.TrimEnd().TrimStart().ToLower()).ToList();
        //                        if (check_class.Count > 0)
        //                        {
        //                            var class_id = _AdmissionFormContext.School_M_Class.Where(t => t.MI_Id == stu.MI_Id
        //                            && t.ASMCL_ClassName.TrimEnd().TrimStart().ToLower() == stu.newlstget1[i].Class.TrimEnd().TrimStart().ToLower()).FirstOrDefault();
        //                            enq.ASMCL_Id = class_id.ASMCL_Id;
        //                            classid = class_id.ASMCL_Id;
        //                        }
        //                        else
        //                        {
        //                            stu.stuStatus = "Class is Not Available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
        //                            return stu;
        //                        }


        //                        //----getting class category id---//                       
        //                        var get_class_category = (from a in _AdmissionFormContext.Masterclasscategory
        //                                                  from b in _AdmissionFormContext.Adm_M_Stu_Cat
        //                                                  where (a.AMC_Id == b.AMC_Id && a.MI_Id == stu.MI_Id && a.ASMAY_Id == yearid && a.ASMCL_Id == classid)
        //                                                  select new
        //                                                  {
        //                                                      asmcc_id = a.ASMCC_Id
        //                                                  }).ToList();

        //                        if (get_class_category.Count > 0)
        //                        {
        //                            enq.AMC_Id = get_class_category.FirstOrDefault().asmcc_id;
        //                        }
        //                        else
        //                        {
        //                            stu.stuStatus = "Class Category is Not Available for this acadmic year" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
        //                        }

        //                        enq.AMST_BloodGroup = stu.newlstget1[i].BloodGroup;


        //                        //----Mother Tongue----//
        //                        if (((Regex.IsMatch(stu.newlstget1[i].MotherTongue.TrimEnd().TrimStart(), @"^[a-zA-Z\s]+$"))
        //                            || (stu.newlstget1[i].MotherTongue.TrimEnd().TrimStart() == null) || (stu.newlstget1[i].MotherTongue.TrimEnd().TrimStart() == "")))
        //                        {
        //                            enq.AMST_MotherTongue = stu.newlstget1[i].MotherTongue.TrimEnd().TrimStart();
        //                        }

        //                        //---Religion---//
        //                        if (((Regex.IsMatch(stu.newlstget1[i].Religion, @"^[a-zA-Z\s]+$"))))
        //                        {
        //                            var get_religionid = _AdmissionFormContext.Religion.Where(t => t.IVRMMR_Name.Equals(stu.newlstget1[i].Religion.TrimEnd().TrimStart())).ToList();
        //                            if (get_religionid.Count() > 0)
        //                            {
        //                                enq.IVRMMR_Id = get_religionid.FirstOrDefault().IVRMMR_Id;
        //                            }
        //                            else
        //                            {
        //                                stu.stuStatus = "Student Religion is not available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
        //                                return stu;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            stu.stuStatus = "Student Religion is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
        //                            return stu;
        //                        }


        //                        //--caste --//
        //                        long get_casteid1 = 0;
        //                        if (((Regex.IsMatch(stu.newlstget1[i].Caste, @"^[a-zA-Z\s]+$"))))
        //                        {
        //                            var get_casteid = _AdmissionFormContext.Caste.Where(t => t.IMC_CasteName.Equals(stu.newlstget1[i].Caste.TrimEnd().TrimStart()) && t.MI_Id == stu.MI_Id).ToList();
        //                            if (get_casteid.Count() > 0)
        //                            {
        //                                enq.IC_Id = get_casteid.FirstOrDefault().IMC_Id;
        //                                get_casteid1 = get_casteid.FirstOrDefault().IMC_Id;
        //                            }
        //                            else
        //                            {
        //                                stu.stuStatus = "Student caste is not available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
        //                                return stu;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            stu.stuStatus = "Student caste is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
        //                            return stu;
        //                        }

        //                        //geting caste category id
        //                        var castecategoryid = "";
        //                        if (((Regex.IsMatch(stu.newlstget1[i].Caste, @"^[a-zA-Z0-9\s]+$"))))
        //                        {
        //                            var get_castecategoryid = (from a in _AdmissionFormContext.CasteCategory
        //                                                       from b in _AdmissionFormContext.Caste
        //                                                       where (a.IMCC_Id == b.IMCC_Id && b.MI_Id == stu.MI_Id && b.IMC_Id == Convert.ToInt64(get_casteid1))
        //                                                       select new
        //                                                       {
        //                                                           castecategoryid = a.IMCC_Id
        //                                                       }).ToList();
        //                            if (get_castecategoryid.Count() > 0)
        //                            {
        //                                enq.IMCC_Id = get_castecategoryid.FirstOrDefault().castecategoryid;
        //                            }
        //                            else
        //                            {
        //                                stu.stuStatus = "Student caste category is not available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
        //                                return stu;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            stu.stuStatus = "Student caste category is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
        //                            return stu;
        //                        }

        //                        //----------------Permanent Address--------------------//
        //                        enq.AMST_PerStreet = stu.newlstget1[i].PermanentStreet.TrimEnd().TrimStart();
        //                        enq.AMST_PerArea = stu.newlstget1[i].PermanentArea.TrimEnd().TrimStart();
        //                        enq.AMST_PerCity = stu.newlstget1[i].PermanentCity.TrimEnd().TrimStart();
        //                        enq.AMST_PerCountry = null;
        //                        enq.AMST_PerState = null;
        //                        enq.AMST_PerPincode = null;

        //                        //permanent country
        //                        long countryid = 0;
        //                        if (stu.newlstget1[i].PermanentCountry.TrimEnd().TrimStart() != null && stu.newlstget1[i].PermanentCountry.TrimEnd().TrimStart() != "")
        //                        {
        //                            if (Regex.IsMatch(stu.newlstget1[i].PermanentCountry.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\.\:\,/])*$"))
        //                            {
        //                                var get_countryid = _AdmissionFormContext.Country.Where(t => t.IVRMMC_CountryName.Equals(stu.newlstget1[i].PermanentCountry.TrimEnd().TrimStart()));
        //                                if (get_countryid.Count() > 0)
        //                                {
        //                                    enq.AMST_PerCountry = get_countryid.FirstOrDefault().IVRMMC_Id;
        //                                    countryid = get_countryid.FirstOrDefault().IVRMMC_Id;
        //                                }
        //                                else
        //                                {
        //                                    stu.stuStatus = "Permanent Country input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
        //                                    return stu;
        //                                }
        //                            }
        //                        }

        //                        //permanent state
        //                        if (stu.newlstget1[i].Permanentstate.TrimEnd().TrimStart() != null && stu.newlstget1[i].Permanentstate.TrimEnd().TrimStart() != "")
        //                        {
        //                            if (((Regex.IsMatch(stu.newlstget1[i].Permanentstate.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\.\:\,/])*$")) && ((stu.newlstget1[i].Permanentstate.TrimEnd().TrimStart()).Length <= 100)))
        //                            {
        //                                var get_countryid = _AdmissionFormContext.State.Where(t => t.IVRMMS_Name.Equals(stu.newlstget1[i].Permanentstate.TrimEnd().TrimStart()) && t.IVRMMC_Id == countryid);
        //                                if (get_countryid.Count() > 0)
        //                                {
        //                                    enq.AMST_PerState = get_countryid.FirstOrDefault().IVRMMS_Id;
        //                                }
        //                                else
        //                                {
        //                                    stu.stuStatus = "Permanent state input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
        //                                    return stu;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                stu.stuStatus = "Permanent state input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
        //                                return stu;
        //                            }
        //                        }


        //                        //Permanent Pincode 
        //                        if (Convert.ToString(stu.newlstget1[i].PermanentPincode.TrimEnd().TrimStart()) != null && Convert.ToString(stu.newlstget1[i].PermanentPincode.TrimEnd().TrimStart()) != "")
        //                        {
        //                            if (((Regex.IsMatch(Convert.ToString(stu.newlstget1[i].PermanentPincode.TrimEnd().TrimStart()), @"^([0-9])*$"))
        //                                && (Convert.ToString(stu.newlstget1[i].PermanentPincode.TrimEnd().TrimStart()).Length == 6)))
        //                            {
        //                                enq.AMST_PerPincode = Convert.ToInt32(stu.newlstget1[i].PermanentPincode.TrimEnd().TrimStart());
        //                            }
        //                            else
        //                            {
        //                                stu.stuStatus = "Permanent Pincode is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
        //                                return stu;
        //                            }
        //                        }
        //                        //---End of the permanent address------//


        //                        //-------------------------Communication Address--------------------------//

        //                        enq.AMST_ConStreet = stu.newlstget1[i].PresentStreet.TrimEnd().TrimStart();
        //                        enq.AMST_ConArea = stu.newlstget1[i].PresentArea.TrimEnd().TrimStart();
        //                        enq.AMST_ConCity = stu.newlstget1[i].PresentCity.TrimEnd().TrimStart();
        //                        enq.AMST_ConCountry = null;
        //                        enq.AMST_ConState = null;
        //                        enq.AMST_ConPincode = null;

        //                        //persent country
        //                        long countryid1 = 0;
        //                        if (stu.newlstget1[i].PresentCountry.TrimEnd().TrimStart() != null && stu.newlstget1[i].PresentCountry.TrimEnd().TrimStart() != "")
        //                        {
        //                            if (Regex.IsMatch(stu.newlstget1[i].PresentCountry.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\.\:\,/])*$"))
        //                            {
        //                                var get_countryid = _AdmissionFormContext.Country.Where(t => t.IVRMMC_CountryName.Equals(stu.newlstget1[i].PresentCountry.TrimEnd().TrimStart()));
        //                                if (get_countryid.Count() > 0)
        //                                {
        //                                    enq.AMST_ConCountry = get_countryid.FirstOrDefault().IVRMMC_Id;
        //                                    countryid1 = get_countryid.FirstOrDefault().IVRMMC_Id;
        //                                }
        //                                else
        //                                {
        //                                    stu.stuStatus = "Present Country input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
        //                                    return stu;
        //                                }
        //                            }
        //                        }

        //                        //present state
        //                        if (stu.newlstget1[i].PresentState.TrimEnd().TrimStart() != null && stu.newlstget1[i].PresentState.TrimEnd().TrimStart() != "")
        //                        {
        //                            if (((Regex.IsMatch(stu.newlstget1[i].PresentState.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\.\:\,/])*$")) && ((stu.newlstget1[i].PresentState.TrimEnd().TrimStart()).Length <= 100)))
        //                            {
        //                                var get_countryid = _AdmissionFormContext.State.Where(t => t.IVRMMS_Name.Equals(stu.newlstget1[i].PresentState.TrimEnd().TrimStart()) && t.IVRMMC_Id == countryid1);
        //                                if (get_countryid.Count() > 0)
        //                                {
        //                                    enq.AMST_ConState = get_countryid.FirstOrDefault().IVRMMS_Id;
        //                                }
        //                                else
        //                                {
        //                                    stu.stuStatus = "present state input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
        //                                    return stu;
        //                                }
        //                            }
        //                        }

        //                        //present Pincode 
        //                        if (Convert.ToString(stu.newlstget1[i].PresentPincode.TrimEnd().TrimStart()) != null
        //                            && Convert.ToString(stu.newlstget1[i].PresentPincode.TrimEnd().TrimStart()) != "")
        //                        {
        //                            if (((Regex.IsMatch(Convert.ToString(stu.newlstget1[i].PresentPincode.TrimEnd().TrimStart()), @"^([0-9])*$")) && (Convert.ToString(stu.newlstget1[i].PresentPincode.TrimEnd().TrimStart()).Length == 6)))
        //                            {
        //                                enq.AMST_ConPincode = Convert.ToInt32(stu.newlstget1[i].PresentPincode.TrimEnd().TrimStart());
        //                            }
        //                            else
        //                            {
        //                                stu.stuStatus = "Present Pincode is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
        //                                return stu;
        //                            }
        //                        }

        //                        //---End of the Communication address------//

        //                        //-------------Aadhar Number -----------//
        //                        enq.AMST_AadharNo = null;

        //                        if (stu.newlstget1[i].AadharNo.TrimEnd().TrimStart() != null && stu.newlstget1[i].AadharNo.TrimEnd().TrimStart() != "")
        //                        {
        //                            if (((Regex.IsMatch((stu.newlstget1[i].AadharNo.TrimEnd().TrimStart()), @"^([0-9])*$"))
        //                                && ((stu.newlstget1[i].AadharNo.TrimEnd().TrimStart()).Length >= 0))
        //                                && ((stu.newlstget1[i].AadharNo.TrimEnd().TrimStart()).Length <= 12))
        //                            {
        //                                enq.AMST_AadharNo = Convert.ToInt64(stu.newlstget1[i].AadharNo.TrimEnd().TrimStart());
        //                            }
        //                        }

        //                        //--- Student Mobile Number --//

        //                        if ((Convert.ToString(stu.newlstget1[i].MobileNo.TrimEnd().TrimStart()) != null)
        //                            && (Convert.ToString(stu.newlstget1[i].MobileNo.TrimEnd().TrimStart()) != ""))
        //                        {
        //                            string MatchPhoneNumberPattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
        //                            if (Regex.IsMatch((Convert.ToString(stu.newlstget1[i].MobileNo.TrimEnd().TrimStart())), MatchPhoneNumberPattern))
        //                            {
        //                                enq.AMST_MobileNo = Convert.ToInt64(stu.newlstget1[i].MobileNo.TrimEnd().TrimStart());
        //                            }
        //                            else
        //                            {
        //                                stu.stuStatus = "Mobile Number is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
        //                                return stu;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            stu.stuStatus = "Mobile Number can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
        //                            return stu;
        //                        }

        //                        //--- Student Email id--//

        //                        if ((Convert.ToString(stu.newlstget1[i].EmailID.TrimEnd().TrimStart()) != null) && (Convert.ToString(stu.newlstget1[i].EmailID.TrimEnd().TrimStart()) != ""))
        //                        {
        //                            Match match = regex.Match(stu.newlstget1[i].EmailID.TrimEnd().TrimStart());
        //                            if (match.Success)
        //                            {
        //                                enq.AMST_emailId = stu.newlstget1[i].EmailID.TrimEnd().TrimStart();
        //                            }
        //                            else
        //                            {
        //                                stu.stuStatus = "Email Id is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
        //                                return stu;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            stu.stuStatus = "Email Id can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
        //                            return stu;
        //                        }

        //                        //----------------Father Details-------------//

        //                        enq.AMST_FatherAliveFlag = "1";
        //                        if(stu.newlstget !=null)
        //                        {
        //                            enq.AMST_FatherAliveFlag = stu.newlstget[i].FatherAlive;
        //                            enq.AMST_FatherName = stu.newlstget[i].FatherName;
        //                            enq.AMST_FatherAadharNo = Convert.ToInt64(stu.newlstget[i].FatherAadharNo);
        //                            enq.AMST_FatherSurname = stu.newlstget[i].FatherSurname;
        //                            enq.AMST_FatherEducation = stu.newlstget[i].FatherEducation;
        //                            enq.AMST_FatherOccupation = stu.newlstget[i].FatherOccupation;
        //                            enq.AMST_FatherOfficeAdd = stu.newlstget[i].FatherOfficeAdd;
        //                            enq.AMST_FatherDesignation = stu.newlstget[i].FatherDesignation;
        //                            enq.AMST_FatherMonIncome = Convert.ToDecimal(stu.newlstget[i].FatherMonthlyIncome);
        //                            enq.AMST_FatherAnnIncome = Convert.ToDecimal(stu.newlstget[i].FatherAnnualIncome);
        //                            var fathernationality1 = _AdmissionFormContext.Country.Where(t => t.IVRMMC_CountryName == stu.newlstget[i].FatherNationality.TrimEnd().TrimStart()).ToList();
        //                            enq.AMST_FatherNationality = fathernationality1.Count > 0 ? fathernationality1.FirstOrDefault().IVRMMC_Id : (long?)null;
        //                            enq.AMST_FatherBankAccNo = stu.newlstget[i].FatherBankAccNo;
        //                            enq.AMST_FatherBankIFSC_Code = stu.newlstget[i].FatherBankIFSC_Code;
        //                            enq.AMST_FatherCasteCertiNo = stu.newlstget[i].FatherCasteCertiNo;
        //                            enq.ANST_FatherPhoto = stu.newlstget[i].FatherPhoto;
        //                            enq.AMST_FatherPANNo = stu.newlstget[i].FatherPANNo;

        //                            var fatherreligion = _AdmissionFormContext.Religion.Where(t => t.IVRMMR_Name == stu.newlstget[i].FatherReligion.TrimEnd().TrimStart()).ToList();

        //                            enq.AMST_FatherReligion = fatherreligion.Count > 0 ? fatherreligion.FirstOrDefault().IVRMMR_Id.ToString() : null;

        //                            var fathercaste = _AdmissionFormContext.Caste.Where(t => t.IMC_CasteName == stu.newlstget[i].FatherCaste.TrimEnd().TrimStart()).ToList();
        //                            enq.AMST_FatherCaste = fathercaste.Count > 0 ? fathercaste.FirstOrDefault().IMC_Id.ToString() : null;

        //                            enq.AMST_FatherSubCaste = stu.newlstget[i].FatherSubCaste;

        //                        }


        //                        if ((Convert.ToString(stu.newlstget1[i].Fathermobileno.TrimEnd().TrimStart()) != null) && (Convert.ToString(stu.newlstget1[i].Fathermobileno.TrimEnd().TrimStart()) != ""))
        //                        {
        //                            string MatchPhoneNumberPattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
        //                            if (Regex.IsMatch((Convert.ToString(stu.newlstget1[i].Fathermobileno.TrimEnd().TrimStart())), MatchPhoneNumberPattern))
        //                            {
        //                                enq.AMST_FatherMobleNo = Convert.ToInt64(stu.newlstget1[i].Fathermobileno.TrimEnd().TrimStart());
        //                            }
        //                            else
        //                            {
        //                                stu.stuStatus = "Father Mobile Number is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
        //                                return stu;
        //                            }
        //                        }

        //                        if ((Convert.ToString(stu.newlstget1[i].FatherEmailId.TrimEnd().TrimStart()) != null) && (Convert.ToString(stu.newlstget1[i].FatherEmailId.TrimEnd().TrimStart()) != ""))
        //                        {
        //                            Match match = regex.Match(stu.newlstget1[i].FatherEmailId.TrimEnd().TrimStart());
        //                            if (match.Success)
        //                            {
        //                                enq.AMST_FatheremailId = stu.newlstget1[i].FatherEmailId.TrimEnd().TrimStart();
        //                            }
        //                            else
        //                            {
        //                                stu.stuStatus = "Father Email Id is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
        //                                return stu;
        //                            }
        //                        }

        //                        //-------Mother Details ----------//

        //                        enq.AMST_MotherAliveFlag = "1";
        //                        enq.AMST_MotherName = stu.newlstget1[i].MotherName.TrimEnd().TrimStart();
        //                        enq.AMST_MotherAadharNo = Convert.ToInt64(stu.newlstget[i].MotherAadharNo);
        //                        enq.AMST_MotherSurname = stu.newlstget[i].MotherSurname;
        //                        enq.AMST_MotherEducation = stu.newlstget[i].MotherEducation;
        //                        enq.AMST_MotherOccupation = stu.newlstget[i].MotherOccupation;
        //                        enq.AMST_MotherOfficeAdd = stu.newlstget[i].MotherOfficeAddress;
        //                        enq.AMST_MotherDesignation = stu.newlstget[i].MotherDesignation;
        //                        enq.AMST_MotherMonIncome = Convert.ToDecimal(stu.newlstget[i].MotherMonthlyIncome);
        //                        enq.AMST_MotherAnnIncome = Convert.ToDecimal(stu.newlstget[i].MotherAnnualIncome);
        //                        var mothernationality = _AdmissionFormContext.Country.Where(t => t.IVRMMC_CountryName == stu.newlstget[i].MotherNationality.TrimEnd().TrimStart()).ToList();
        //                        enq.AMST_MotherNationality = mothernationality.Count > 0 ? mothernationality.FirstOrDefault().IVRMMC_Id : (long?)null;
        //                        enq.AMST_MotherBankAccNo = stu.newlstget[i].MotherBankAccNo;
        //                        enq.AMST_MotherBankIFSC_Code = stu.newlstget[i].MotherBankIFSC_Code;
        //                        enq.AMST_MotherCasteCertiNo = stu.newlstget[i].MotherCasteCertiNo;
        //                        enq.AMST_TotalIncome = Convert.ToDecimal(stu.newlstget[i].TotalIncome);
        //                        enq.ANST_MotherPhoto = stu.newlstget[i].MotherPhoto;
        //                        enq.AMST_MotherPANNo = stu.newlstget[i].MotherPANNo;

        //                        var motherreligion = _AdmissionFormContext.Religion.Where(t => t.IVRMMR_Name == stu.newlstget[i].MotherReligion.TrimEnd().TrimStart()).ToList();

        //                        enq.AMST_MotherReligion = motherreligion.Count > 0 ? motherreligion.FirstOrDefault().IVRMMR_Id.ToString() : null;

        //                        var mothercaste = _AdmissionFormContext.Caste.Where(t => t.IMC_CasteName == stu.newlstget[i].MotherCaste.TrimEnd().TrimStart()).ToList();
        //                        enq.AMST_MotherCaste = mothercaste.Count > 0 ? mothercaste.FirstOrDefault().IMC_Id.ToString() : null;
        //                        enq.AMST_MotherSubCaste = stu.newlstget[i].MotherSubCaste;

        //                        if ((Convert.ToString(stu.newlstget1[i].MotherMobileNo.TrimEnd().TrimStart()) != null) && (Convert.ToString(stu.newlstget1[i].MotherMobileNo.TrimEnd().TrimStart()) != ""))
        //                        {
        //                            string MatchPhoneNumberPattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
        //                            if (Regex.IsMatch((Convert.ToString(stu.newlstget1[i].MotherMobileNo.TrimEnd().TrimStart())), MatchPhoneNumberPattern))
        //                            {
        //                                enq.AMST_MotherMobileNo = Convert.ToInt64(stu.newlstget1[i].MotherMobileNo.TrimEnd().TrimStart());
        //                            }
        //                            else
        //                            {
        //                                stu.stuStatus = "Mother Mobile Number is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
        //                                return stu;
        //                            }
        //                        }

        //                        if ((Convert.ToString(stu.newlstget1[i].MotherEmailId.TrimEnd().TrimStart()) != null)
        //                            && (Convert.ToString(stu.newlstget1[i].MotherEmailId.TrimEnd().TrimStart()) != ""))
        //                        {

        //                            Match match = regex.Match(stu.newlstget1[i].MotherEmailId.TrimEnd().TrimStart());
        //                            if (match.Success)
        //                            {
        //                                enq.AMST_MotherEmailId = stu.newlstget1[i].MotherEmailId.TrimEnd().TrimStart();
        //                            }
        //                            else
        //                            {
        //                                stu.stuStatus = "Mother Email Id is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
        //                                return stu;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            stu.stuStatus = "Mother Email Id can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
        //                            return stu;
        //                        }

        //                        //student nationality
        //                        if (stu.newlstget1[i].StudentNationality.TrimEnd().TrimStart() != null
        //                            && stu.newlstget1[i].StudentNationality.TrimEnd().TrimStart() != "")
        //                        {
        //                            if (Regex.IsMatch(stu.newlstget1[i].StudentNationality.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\:\,])*$"))
        //                            {
        //                                var get_countryid = _AdmissionFormContext.Country.Where(t => t.IVRMMC_CountryName.Equals(stu.newlstget1[i].StudentNationality.TrimEnd().TrimStart()));
        //                                if (get_countryid.Count() > 0)
        //                                {
        //                                    enq.AMST_Nationality = get_countryid.FirstOrDefault().IVRMMC_Id;
        //                                    countryid1 = get_countryid.FirstOrDefault().IVRMMC_Id;
        //                                }
        //                                else
        //                                {
        //                                    stu.stuStatus = "student Nationality input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
        //                                    return stu;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                stu.stuStatus = "student Nationality input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
        //                                return stu;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            stu.stuStatus = "student Nationality Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
        //                            return stu;
        //                        }

        //                        var concession = _AdmissionFormContext.Fee_Master_ConcessionDMO.Where(a => a.MI_Id == stu.MI_Id && a.FMCC_ConcessionName.Equals("General") && a.FMCC_ActiveFlag == true).ToList();
        //                        if (concession.Count > 0)
        //                        {
        //                            enq.AMST_Concession_Type = concession.FirstOrDefault().FMCC_Id;
        //                        }
        //                        else
        //                        {
        //                            stu.stuStatus = "student Concession Type Is Not There Please Map The Fee Concession Type For Student And Concession Type Should Be 'General' Concession Type";
        //                            return stu;
        //                        }

        //                        enq.AMST_BirthCertNO = stu.newlstget1[i].BirthCertNO;

        //                        enq.AMST_StuBankAccNo = stu.newlstget1[i].StudentBankAccNo;
        //                        enq.AMST_StuBankIFSC_Code = stu.newlstget1[i].StudentBankIFSC_Code;
        //                        enq.AMST_StuCasteCertiNo = stu.newlstget1[i].StuCasteCertiNo;
        //                        enq.AMST_BirthPlace = stu.newlstget1[i].BirthPlace;
        //                        enq.AMST_SubCasteIMC_Id = stu.newlstget1[i].StudentSubCaste;
        //                        enq.AMST_StudentPANNo = stu.newlstget1[i].StudentPANNo;

        //                        enq.AMST_HostelReqdFlag = stu.newlstget1[i].HostelReqdFlag == "Yes" ? 1 : 0;
        //                        enq.AMST_TransportReqdFlag = stu.newlstget1[i].TransportReqdFlag == "Yes" ? 1 : 0;
        //                        enq.AMST_GymReqdFlag = stu.newlstget1[i].GymReqdFlag == "Yes" ? 1 : 0;
        //                        enq.AMST_ECSFlag = stu.newlstget1[i].ECSFlag == "Yes" ? 1 : 0;
        //                        enq.AMST_BPLCardNo = stu.newlstget1[i].BPLCardNo;
        //                        enq.AMST_LanguageSpoken = stu.newlstget1[i].LanguageSpoken;

        //                        enq.AMST_Noofsisters = Convert.ToInt32(stu.newlstget[i].Noofsisters);
        //                        enq.AMST_Noofbrothers = Convert.ToInt32(stu.newlstget[i].NoOfBrothers);

        //                        enq.AMST_NoOfElderSisters = Convert.ToInt32(stu.newlstget[i].NoOfElderSisters);
        //                        enq.AMST_NoOfElderBrothers = Convert.ToInt32(stu.newlstget[i].NoOfElderBrothers);

        //                        enq.AMST_NoOfYoungerSisters = Convert.ToInt32(stu.newlstget[i].NoOfYoungerSisters);
        //                        enq.AMST_NoOfYoungerBrothers = Convert.ToInt32(stu.newlstget[i].NoOfYoungerBrothers);

        //                        enq.AMST_SOL = stu.newlstget1[i].Status;
        //                        enq.AMST_ActiveFlag = stu.newlstget1[i].Status == "S" ? 1 : 0;
        //                        enq.CreatedDate = DateTime.Now;
        //                        enq.UpdatedDate = DateTime.Now;
        //                        enq.AMST_UpdatedBy = stu.User_Id;
        //                        enq.AMST_CreatedBy = stu.User_Id;

        //                        _DomainModelMsSqlServerContext.Add(enq);
        //                        //var flag = _DomainModelMsSqlServerContext.SaveChanges();
        //                        //if (flag >= 1)
        //                        //{
        //                        try
        //                        {
        //                            if (enq.AMST_FatheremailId != null && enq.AMST_FatheremailId != "")
        //                            {
        //                                Adm_Master_Father_Email enq_faheremail = new Adm_Master_Father_Email
        //                                {
        //                                    AMST_FatheremailId = enq.AMST_FatheremailId,
        //                                    MI_Id = stu.MI_Id,
        //                                    AMST_Id = enq.AMST_Id,
        //                                    CreatedDate = DateTime.Now,
        //                                    UpdatedDate = DateTime.Now
        //                                };
        //                                _DomainModelMsSqlServerContext.Add(enq_faheremail);
        //                                // int n = _DomainModelMsSqlServerContext.SaveChanges();
        //                            }
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            _acdimpl.LogInformation("Father emailid saving in new table" + ex.Message);
        //                        }

        //                        try
        //                        {
        //                            if (enq.AMST_MotherEmailId != null && enq.AMST_MotherEmailId != "")
        //                            {
        //                                Adm_M_Mother_Emailid enq_motheemail = new Adm_M_Mother_Emailid
        //                                {
        //                                    AMST_MotheremailId = enq.AMST_MotherEmailId,
        //                                    AMST_Id = enq.AMST_Id,
        //                                    MI_Id = stu.MI_Id,
        //                                    CreatedDate = DateTime.Now,
        //                                    UpdatedDate = DateTime.Now
        //                                };
        //                                _DomainModelMsSqlServerContext.Add(enq_motheemail);
        //                                // int n1 = _DomainModelMsSqlServerContext.SaveChanges();
        //                            }
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            _acdimpl.LogInformation("Mother emailid saving in new table" + ex.Message);
        //                        }

        //                        try
        //                        {
        //                            if (enq.AMST_MotherMobileNo != null)
        //                            {
        //                                Adm_M_Mother_MobileNo enq_mothermobile = new Adm_M_Mother_MobileNo
        //                                {
        //                                    AMST_MotherMobileNo = enq.AMST_MotherMobileNo,
        //                                    AMST_Id = enq.AMST_Id,
        //                                    MI_Id = stu.MI_Id,
        //                                    CreatedDate = DateTime.Now,
        //                                    UpdatedDate = DateTime.Now
        //                                };
        //                                _DomainModelMsSqlServerContext.Add(enq_mothermobile);
        //                                // int n2 = _DomainModelMsSqlServerContext.SaveChanges();
        //                            }

        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            _acdimpl.LogInformation("Mother Mobileno saving in new table" + ex.Message);
        //                        }

        //                        try
        //                        {
        //                            if (enq.AMST_FatherMobleNo != null)
        //                            {
        //                                Adm_M_Student_FatherMobileNo enq_fathermobile = new Adm_M_Student_FatherMobileNo
        //                                {
        //                                    AMST_Id = enq.AMST_Id,
        //                                    MI_Id = stu.MI_Id,
        //                                    AMST_FatherMobile_No = Convert.ToInt64(enq.AMST_FatherMobleNo),
        //                                    CreatedDate = DateTime.Now,
        //                                    UpdatedDate = DateTime.Now
        //                                };
        //                                _DomainModelMsSqlServerContext.Add(enq_fathermobile);
        //                                // int n3 = _DomainModelMsSqlServerContext.SaveChanges();
        //                            }

        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            _acdimpl.LogInformation("Father Mobileno saving in new table" + ex.Message);
        //                        }

        //                        try
        //                        {
        //                            if (stu.newlstget[i].MobileNo != null && stu.newlstget[i].MobileNo != "")
        //                            {
        //                                Adm_M_Student_MobileNo enq_studentmobile = new Adm_M_Student_MobileNo
        //                                {
        //                                    AMST_Id = enq.AMST_Id,
        //                                    AMSTSMS_MobileNo = enq.AMST_MobileNo.ToString(),
        //                                    CreatedDate = DateTime.Now,
        //                                    UpdatedDate = DateTime.Now
        //                                };
        //                                _DomainModelMsSqlServerContext.Add(enq_studentmobile);
        //                                // int n4 = _DomainModelMsSqlServerContext.SaveChanges();
        //                            }

        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            _acdimpl.LogInformation("Student Mobileno saving in new table" + ex.Message);
        //                        }

        //                        try
        //                        {
        //                            if (enq.AMST_emailId != null && enq.AMST_emailId != "")
        //                            {
        //                                Adm_M_Student_Email_Id enq_studentemail = new Adm_M_Student_Email_Id
        //                                {
        //                                    AMST_Id = enq.AMST_Id,
        //                                    AMSTE_EmailId = enq.AMST_emailId,
        //                                    CreatedDate = DateTime.Now,
        //                                    UpdatedDate = DateTime.Now
        //                                };
        //                                _DomainModelMsSqlServerContext.Add(enq_studentemail);
        //                                //int n5 = _DomainModelMsSqlServerContext.SaveChanges();
        //                            }

        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            _acdimpl.LogInformation("Student Email saving in new table" + ex.Message);
        //                        }

        //                        long yearid_current = 0;
        //                        long classid_current = 0;
        //                        long section_current = 0;
        //                        try
        //                        {
        //                            var year = "";
        //                            year = stu.newlstget1[i].CurrentYear.Trim().ToString();

        //                            if ((year != null))
        //                            {
        //                                if ((Regex.IsMatch(Convert.ToString(year), @"^[0-9\s-]*$")))
        //                                {
        //                                    var check_yearid = _AdmissionFormContext.year.Where(t => t.ASMAY_Year.Equals(year) && t.MI_Id == stu.MI_Id).ToList();
        //                                    if (check_yearid.Count() > 0)
        //                                    {
        //                                        yearid_current = check_yearid.FirstOrDefault().ASMAY_Id;
        //                                    }
        //                                    else
        //                                    {
        //                                        stu.stuStatus = "Student Current Year is Not Available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
        //                                        return stu;
        //                                    }
        //                                }
        //                                else
        //                                {
        //                                    stu.stuStatus = "Student Year is Not Valid as It Should Contain Only numeric with -";
        //                                    return stu;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                stu.stuStatus = "Student Current Year can not be null or Empty For   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
        //                                return stu;
        //                            }
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            Console.WriteLine(ex.Message);
        //                            stu.stuStatus = "Student Current Year can not be null or Empty   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
        //                            return stu;
        //                        }

        //                        try
        //                        {
        //                            var current_class = "";
        //                            current_class = stu.newlstget1[i].CurrentClass.Trim().ToString();

        //                            if (current_class != null)
        //                            {
        //                                var check_classid = _AdmissionFormContext.School_M_Class.Where(t => t.ASMCL_ClassName.Equals(current_class)
        //                                && t.MI_Id == stu.MI_Id).ToList();
        //                                if (check_classid.Count() > 0)
        //                                {
        //                                    classid_current = check_classid.FirstOrDefault().ASMCL_Id;
        //                                }
        //                                else
        //                                {
        //                                    stu.stuStatus = "Student Current Class is Not Available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
        //                                    return stu;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                stu.stuStatus = "Student Current Class can not be null or Empty For   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
        //                                return stu;
        //                            }
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            Console.WriteLine(ex.Message);
        //                            stu.stuStatus = "Student Current Class can not be null or Empty   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
        //                            return stu;
        //                        }

        //                        try
        //                        {
        //                            var current_section = "";
        //                            current_section = stu.newlstget1[i].CurrentSection.Trim().ToString();

        //                            if (current_section != null)
        //                            {
        //                                var check_sectionid = _AdmissionFormContext.AdmSection.Where(t => t.ASMC_SectionName.Equals(current_section)
        //                                && t.MI_Id == stu.MI_Id).ToList();
        //                                if (check_sectionid.Count() > 0)
        //                                {
        //                                    section_current = check_sectionid.FirstOrDefault().ASMS_Id;
        //                                }
        //                                else
        //                                {
        //                                    stu.stuStatus = "Student Current Section is Not Available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
        //                                    return stu;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                stu.stuStatus = "Student Current Section can not be null or Empty For   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
        //                                return stu;
        //                            }
        //                        }
        //                        catch (Exception ex)
        //                        {
        //                            Console.WriteLine(ex.Message);
        //                            stu.stuStatus = "Student Current Section can not be null or Empty   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
        //                            return stu;
        //                        }

        //                        if (yearid_current > 0 && classid_current > 0 && section_current > 0)
        //                        {
        //                            School_Adm_Y_StudentDMO school_Adm_Y_StudentDMO = new School_Adm_Y_StudentDMO();
        //                            school_Adm_Y_StudentDMO.ASMAY_Id = yearid_current;
        //                            school_Adm_Y_StudentDMO.ASMCL_Id = classid_current;
        //                            school_Adm_Y_StudentDMO.ASMS_Id = section_current;
        //                            school_Adm_Y_StudentDMO.AMST_Id = enq.AMST_Id;
        //                            school_Adm_Y_StudentDMO.AMAY_RollNo = 1;
        //                            school_Adm_Y_StudentDMO.AMAY_ActiveFlag = 1;
        //                            school_Adm_Y_StudentDMO.LoginId = stu.User_Id;
        //                            school_Adm_Y_StudentDMO.ASYST_CreatedBy = stu.User_Id;
        //                            school_Adm_Y_StudentDMO.ASYST_UpdatedBy = stu.User_Id;
        //                            school_Adm_Y_StudentDMO.AMAY_DateTime = DateTime.Now;
        //                            school_Adm_Y_StudentDMO.CreatedDate = DateTime.Now;
        //                            school_Adm_Y_StudentDMO.UpdatedDate = DateTime.Now;
        //                            _AdmissionFormContext.Add(school_Adm_Y_StudentDMO);

        //                            //var i1 = _AdmissionFormContext.SaveChanges();
        //                        }

        //                        var id = _DomainModelMsSqlServerContext.SaveChanges();

        //                        stu.stuStatus = "true";
        //                        sucesscount = sucesscount + 1;
        //                        //}
        //                        //else
        //                        //{
        //                        //    stu.stuStatus = "false";
        //                        //    failcount = failcount + 1;
        //                        //    string name = failnames;
        //                        //    finalnames += name + ",";
        //                        //    failedlist.Add(stu.newlstget1[i]);
        //                        //}
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.Write(ex.Message);
        //                    failcount = failcount + 1;
        //                    string name = failnames;
        //                    finalnames += name + ",";
        //                    failedlist.Add(stu.newlstget1[i]);
        //                    continue;
        //                }
        //            }
        //        }
        //        stu.failedlist = failedlist.ToArray();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Write(ex.Message);
        //    }

        //    if (finalnames != "" && failcount > 0)
        //    {
        //        stu.returnMsg += "Total Record Insert :'" + sucesscount + "'  , Total Records Failed : '" + failcount + "' And Failed List Names :' " + finalnames + "'";
        //    }
        //    else
        //    {
        //        stu.returnMsg += "Total Record Insert :'" + sucesscount + "'  , Total Records Failed : '" + 0 + "'";
        //    }

        //    return stu;
        //}

        public async Task<ImportStudentWrapperDTO> checkvalidation(ImportStudentWrapperDTO stu)
        {
            //int sucesscount = 0;
            int failcount = 0;
            string failnames = "";
            string finalnames = "";
            string studentadmno = "Student Admission Number";
            int age = 0;
            try
            {
                for (int i = 0; i < stu.newlstget1.Length; i++)
                {
                    try
                    {
                        _acdimpl.LogInformation("entered for loop");
                        Adm_M_Student enq = new Adm_M_Student();

                        // var IVRMMRId1 = _DomainModelMsSqlServerContext.School_M_Class.Where(t => t.ASMCL_ClassName.Equals(stu.newlstget[i].Class.TrimEnd().TrimStart()) && t.MI_Id == stu.MI_Id).FirstOrDefault();
                        try
                        {
                            var admno = "";
                            if (stu.newlstget1[i].AMSTAdmNo != null && stu.newlstget1[i].AMSTAdmNo != "")
                            {
                                admno = stu.newlstget1[i].AMSTAdmNo.Trim().ToString();
                                if ((admno != null))
                                {
                                    if ((Regex.IsMatch(Convert.ToString(admno), @"^[a-zA-Z0-9/\-@]*$")))
                                    {
                                        var checkduplicate = _AdmissionFormContext.Adm_M_Student.Where(a => a.MI_Id == stu.MI_Id && a.AMST_AdmNo.Equals(admno)).ToList();

                                        if (checkduplicate.Count > 0)
                                        {
                                            stu.stuStatus = "Student Admission Number is Already Exists " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                            return stu;
                                        }

                                        enq.AMST_AdmNo = stu.newlstget1[i].AMSTAdmNo;
                                    }

                                    else
                                    {
                                        stu.stuStatus = "Student Admission Number is Not Valid as It Should Contain Only Alphanumeric Characters " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Student Admission Number can not be null or Empty For   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                    return stu;
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            stu.stuStatus = "Student Admission Number can not be null or Empty For   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                            return stu;
                        }

                        //--------Register Number ------------//
                        try
                        {
                            var regno = "";
                            if (stu.newlstget1[i].AMSTRegistrationNo != null && stu.newlstget1[i].AMSTRegistrationNo != "")
                            {
                                regno = stu.newlstget1[i].AMSTRegistrationNo.Trim().ToString();
                                if ((regno != null))
                                {
                                    if ((Regex.IsMatch(Convert.ToString(regno), @"^[a-zA-Z0-9/\-@]*$")))
                                    {
                                        var checkduplicate1 = _AdmissionFormContext.Adm_M_Student.Where(a => a.MI_Id == stu.MI_Id && a.AMST_RegistrationNo.Equals(regno)).ToList();

                                        if (checkduplicate1.Count > 0)
                                        {
                                            stu.stuStatus = "Student Registration Number is Already Exists " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                            return stu;
                                        }


                                        enq.AMST_RegistrationNo = stu.newlstget1[i].AMSTRegistrationNo;
                                    }

                                    else
                                    {
                                        stu.stuStatus = "Student Registration Number is Not Valid as It Should Contain Only Alphanumeric Characters";
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Student Registration Number can not be null or Empty For   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                    return stu;
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            stu.stuStatus = "Student Registration Number can not be null or Empty For   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                            return stu;
                        }

                        //---------academic year----------//
                        long yearid = 0;
                        try
                        {
                            var year = "";
                            if (stu.newlstget1[i].year != null && stu.newlstget1[i].year != "")
                            {
                                year = stu.newlstget1[i].year.Trim().ToString();
                                if ((year != null))
                                {
                                    if ((Regex.IsMatch(Convert.ToString(year), @"^[0-9\s-]*$")))
                                    {
                                        var check_yearid = _AdmissionFormContext.year.Where(t => t.ASMAY_Year.Equals(year) && t.MI_Id == stu.MI_Id).ToList();
                                        if (check_yearid.Count() > 0)
                                        {
                                            enq.ASMAY_Id = check_yearid.FirstOrDefault().ASMAY_Id;
                                            yearid = check_yearid.FirstOrDefault().ASMAY_Id;
                                        }
                                        else
                                        {
                                            stu.stuStatus = "Year is Not Available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                            return stu;
                                        }

                                    }
                                    else
                                    {
                                        stu.stuStatus = "Student Year is Not Valid as It Should Contain Only numeric with -";
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Student Year can not be null or Empty For   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                    return stu;
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            stu.stuStatus = "StudentYear can not be null or Empty   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                            return stu;
                        }

                        //----------First Name---------//
                        try
                        {
                            if (stu.newlstget1[i].FirstName != null && stu.newlstget1[i].FirstName != "")
                            {
                                if ((stu.newlstget1[i].FirstName.TrimEnd().TrimStart() != null) && (stu.newlstget1[i].FirstName.TrimEnd().TrimStart() != ""))
                                {
                                    if ((Regex.IsMatch(stu.newlstget1[i].FirstName.TrimEnd().TrimStart(), @"^[a-zA-Z.\s]+$")) && ((stu.newlstget1[i].FirstName.TrimEnd().TrimStart()).Length <= 50))
                                    {
                                        enq.AMST_FirstName = stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                    }

                                    else
                                    {
                                        stu.stuStatus = "Student First Name is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + "Student Admission Number";
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Student First Name can not be null for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + "Student Admission Number";
                                    return stu;
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            stu.stuStatus = "Student First Name Header Is Changed , Header Should Be FirstName";
                            return stu;
                        }
                        var middlename = "";
                        var middlename1 = "";
                        if (stu.newlstget1[i].MiddleName != null)
                        {
                            if (stu.newlstget1[i].MiddleName != "")
                            {
                                middlename = stu.newlstget1[i].MiddleName.TrimEnd().TrimStart();
                                middlename1 = stu.newlstget1[i].MiddleName.Trim();

                                if ((Regex.IsMatch(middlename, @"^[a-zA-Z.\s]+$")))
                                {
                                    enq.AMST_MiddleName = stu.newlstget1[i].MiddleName.TrimEnd().TrimStart();
                                }
                                else
                                {
                                    enq.AMST_MiddleName = stu.newlstget1[i].MiddleName.TrimEnd().TrimStart();
                                }
                            }

                        }


                        if (stu.newlstget1[i].LastName != null && stu.newlstget1[i].LastName != "")
                        {
                            if (((Regex.IsMatch(stu.newlstget1[i].LastName.TrimEnd().TrimStart(), @"^[a-zA-Z.\s]+$"))))
                            {
                                enq.AMST_LastName = stu.newlstget1[i].LastName.TrimEnd().TrimStart();
                            }
                            else
                            {
                                enq.AMST_LastName = stu.newlstget1[i].LastName.TrimEnd().TrimStart();
                            }
                        }




                        //---Date Of Admissiom----//
                        string readAddMeeting = "";
                        bool validDate = false;
                        var dateFormats = new[] { "dd.MM.yyyy", "dd-MM-yyyy", "dd/MM/yyyy" };

                        DateTime scheduleDate_DOB;
                        DateTime scheduleDate_DOJ;

                        if (stu.newlstget1[i].amstdate != null)
                        {
                            readAddMeeting = stu.newlstget1[i].amstdate.TrimEnd().TrimStart();

                            validDate = DateTime.TryParseExact(
                               readAddMeeting,
                               dateFormats,
                               DateTimeFormatInfo.InvariantInfo,
                               DateTimeStyles.None,
                               out scheduleDate_DOJ);
                            if (validDate)
                            {
                                enq.AMST_Date = scheduleDate_DOJ;
                            }
                            else
                            {
                                stu.stuStatus = "Date Of Join is not Valid, Please Enter in dd/MM/yyyy format for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }
                        else
                        {
                            stu.stuStatus = "Date Of Join is Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                            return stu;
                        }



                        //----gender---//
                        if (stu.newlstget1[i].Gender != null && stu.newlstget1[i].Gender != "")
                        {
                            string gender = stu.newlstget1[i].Gender.TrimEnd().TrimStart();

                            if (gender != null && gender != "")
                            {
                                if (((Regex.IsMatch(gender, @"^([a-zA-Z/s])*$")) && (gender.Equals("Male") || gender.Equals("Female") || gender.Equals("Other") || (gender.Equals("MALE") || gender.Equals("FEMALE"))) && gender.Length <= 6))
                                {
                                    enq.AMST_Sex = gender;
                                }
                                else
                                {
                                    stu.stuStatus = "Sudent Gender is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Sudent Gender is required for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }

                        }


                        //--Date Of Birth--//
                        DateTime fromdatecon = DateTime.Now;
                        string confromdate = "";
                        if (stu.newlstget1[i].DOB != null)
                        {

                            readAddMeeting = stu.newlstget1[i].DOB.TrimEnd().TrimStart();


                            validDate = DateTime.TryParseExact(
                         readAddMeeting,
                         dateFormats,
                         DateTimeFormatInfo.InvariantInfo,
                         DateTimeStyles.None,
                         out scheduleDate_DOB);



                            if (validDate)
                            {
                                enq.AMST_DOB = scheduleDate_DOB;
                                fromdatecon = Convert.ToDateTime(scheduleDate_DOB);
                                //fromdatecon = Convert.ToDateTime(stu.newlstget1[i].DOB);
                                //confromdate = fromdatecon.ToString();
                                confromdate = fromdatecon.ToString("yyyy-MM-dd");
                            }


                            else
                            {
                                stu.stuStatus = "Date Of Birth is not Valid, Please Enter in dd/MM/yyyy format for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }

                        }


                        //--DOB Words --//

                        if (stu.newlstget1[i].AMSTDOBWords != null)
                        {
                            if (stu.newlstget1[i].AMSTDOBWords.TrimEnd().TrimStart() != null && stu.newlstget1[i].AMSTDOBWords.TrimEnd().TrimStart() != "")
                            {
                                enq.AMST_DOB_Words = stu.newlstget1[i].AMSTDOBWords.TrimEnd().TrimStart();
                            }
                            else
                            {
                                enq.AMST_DOB_Words = stu.newlstget1[i].AMSTDOBWords.TrimEnd().TrimStart();
                            }
                        }


                        //--Age calculation--//
                        if (confromdate != null && confromdate != "")
                        {
                            age = Convert.ToInt32(DateTime.Now.Year - Convert.ToDateTime(confromdate).Year);

                            if (age > 0)
                            {
                                age -= Convert.ToInt32(DateTime.Now < Convert.ToDateTime(confromdate).Date.AddYears(age));
                            }
                            else
                            {
                                age = 0;
                            }
                            enq.PASR_Age = age;
                        }

                        long classid = 0;
                        string classname = "";
                        if (stu.newlstget1[i].Class != null && stu.newlstget1[i].Class != "")
                        {
                            classname = stu.newlstget1[i].Class.TrimEnd();
                            //--getting class id from class name --//
                            var check_class = _AdmissionFormContext.School_M_Class.Where(t => t.MI_Id == stu.MI_Id && t.ASMCL_ClassName.TrimEnd().TrimStart().ToLower() == stu.newlstget1[i].Class.TrimEnd().TrimStart().ToLower()).ToList();
                            if (check_class.Count > 0)
                            {
                                var class_id = _AdmissionFormContext.School_M_Class.Where(t => t.MI_Id == stu.MI_Id && t.ASMCL_ClassName.TrimEnd().TrimStart().ToLower() == stu.newlstget1[i].Class.TrimEnd().TrimStart().ToLower()).FirstOrDefault();
                                enq.ASMCL_Id = class_id.ASMCL_Id;
                                classid = class_id.ASMCL_Id;
                            }
                            else
                            {
                                stu.stuStatus = "Class is Not Available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }



                        //----getting class category id---//                        
                        var get_class_category = (from a in _AdmissionFormContext.Masterclasscategory
                                                  from b in _AdmissionFormContext.Adm_M_Stu_Cat
                                                  where (a.AMC_Id == b.AMC_Id && a.MI_Id == stu.MI_Id && a.ASMAY_Id == yearid && a.ASMCL_Id == classid)
                                                  select new
                                                  {
                                                      asmcc_id = a.ASMCC_Id
                                                  }).ToList();

                        if (get_class_category.Count > 0)
                        {
                            enq.AMC_Id = get_class_category.FirstOrDefault().asmcc_id;
                        }
                        else
                        {
                            stu.stuStatus = "Class Category is Not Available for this acadmic year" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                            return stu;
                        }
                        if (stu.newlstget1[i].BloodGroup != null && stu.newlstget1[i].BloodGroup != "")
                        {
                            enq.AMST_BloodGroup = stu.newlstget1[i].BloodGroup;
                        }



                        //----Mother Tongue----//
                        if (stu.newlstget1[i].MotherTongue != null && stu.newlstget1[i].MotherTongue != "")
                        {
                            if (((Regex.IsMatch(stu.newlstget1[i].MotherTongue.TrimEnd().TrimStart(), @"^[a-zA-Z\s]+$")) || (stu.newlstget1[i].MotherTongue.TrimEnd().TrimStart() == null) || (stu.newlstget1[i].MotherTongue.TrimEnd().TrimStart() == "")))
                            {
                                enq.AMST_MotherTongue = stu.newlstget1[i].MotherTongue.TrimEnd().TrimStart();
                            }
                            else
                            {
                                stu.stuStatus = "Student MotherTongue is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }


                        //---Religion---//
                        if (stu.newlstget1[i].Religion != null)
                        {
                            if (((Regex.IsMatch(stu.newlstget1[i].Religion, @"^[a-zA-Z\s]+$"))))
                            {
                                var get_religionid = _AdmissionFormContext.Religion.Where(t => t.IVRMMR_Name.Equals(stu.newlstget1[i].Religion.TrimEnd().TrimStart())).ToList();
                                if (get_religionid.Count() > 0)
                                {
                                    enq.IVRMMR_Id = get_religionid.FirstOrDefault().IVRMMR_Id;
                                }
                                else
                                {
                                    stu.stuStatus = "Student Religion is not available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }

                            }
                            else
                            {
                                stu.stuStatus = "Student Religion is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }


                        //--caste --//
                        long get_casteid1 = 0;
                        if (stu.newlstget1[i].Caste != null && stu.newlstget1[i].Caste != "")
                        {
                            if (((Regex.IsMatch(stu.newlstget1[i].Caste, @"^[a-zA-Z0-9\s]+$"))))
                            {
                                //@"^[a-zA-Z\s]+$"
                                var get_casteid = _AdmissionFormContext.Caste.Where(t => t.IMC_CasteName.Equals(stu.newlstget1[i].Caste.TrimEnd().TrimStart()) && t.MI_Id == stu.MI_Id).ToList();
                                if (get_casteid.Count() > 0)
                                {
                                    enq.IC_Id = get_casteid.FirstOrDefault().IMC_Id;
                                    get_casteid1 = get_casteid.FirstOrDefault().IMC_Id;
                                }
                                else
                                {
                                    stu.stuStatus = "Student caste is not available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }

                            }
                            else
                            {
                                stu.stuStatus = "Student caste is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }

                            //geting caste category id
                            // var castecategoryid = "";

                            if (((Regex.IsMatch(stu.newlstget1[i].Caste, @"^[a-zA-Z0-9\s]+$"))))
                            {
                                var get_castecategoryid = (from a in _AdmissionFormContext.CasteCategory
                                                           from b in _AdmissionFormContext.Caste
                                                           where (a.IMCC_Id == b.IMCC_Id && b.MI_Id == stu.MI_Id && b.IMC_Id == Convert.ToInt64(get_casteid1))
                                                           select new
                                                           {
                                                               castecategoryid = a.IMCC_Id
                                                           }).ToList();
                                if (get_castecategoryid.Count() > 0)
                                {
                                    enq.IMCC_Id = get_castecategoryid.FirstOrDefault().castecategoryid;
                                }
                                else
                                {
                                    stu.stuStatus = "Student caste category is not available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno + " " + "Please Map The Caste  Category to" + stu.newlstget1[i].Caste;
                                    return stu;
                                }

                            }
                            else
                            {
                                stu.stuStatus = "Student caste category is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }

                        }




                        //----------------Permanent Address--------------------//

                        //street
                        var PermanentStreet = "";


                        if (stu.newlstget1[i].PermanentStreet != null && stu.newlstget1[i].PermanentStreet != "")

                        {
                            PermanentStreet = stu.newlstget1[i].PermanentStreet.Trim();
                            if (((Regex.IsMatch(PermanentStreet, @"^([a-zA-Z0-9\s\-\,\.\;\():\,\'/&#])*$")) && ((PermanentStreet).Length <= 100)))

                            {
                                enq.AMST_PerStreet = stu.newlstget1[i].PermanentStreet.TrimEnd().TrimStart();
                            }
                            else
                            {
                                stu.stuStatus = "Permanent Street's input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }
                        else
                        {
                            stu.stuStatus = "Permanent Street Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                            return stu;
                        }

                        //area
                        if (stu.newlstget1[i].PermanentArea != null && stu.newlstget1[i].PermanentArea != "")
                        {
                            if (stu.newlstget1[i].PermanentArea.TrimEnd().TrimStart() != null && stu.newlstget1[i].PermanentArea.TrimEnd().TrimStart() != "")
                            {
                                if (((Regex.IsMatch(stu.newlstget1[i].PermanentArea.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,()\'\.\;\:\,/&#])*$")) && ((stu.newlstget1[i].PermanentArea.TrimEnd().TrimStart()).Length <= 100)))

                                {
                                    enq.AMST_PerArea = stu.newlstget1[i].PermanentArea.TrimEnd().TrimStart();
                                }
                                else
                                {
                                    stu.stuStatus = "Permanent Area's input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Permanent Area Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }

                        //city
                        if (stu.newlstget1[i].PermanentCity != null && stu.newlstget1[i].PermanentCity != "")
                        {
                            if (stu.newlstget1[i].PermanentCity.TrimEnd().TrimStart() != null && stu.newlstget1[i].PermanentCity.TrimEnd().TrimStart() != "")
                            {
                                if (((Regex.IsMatch(stu.newlstget1[i].PermanentCity.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\'\.\;\:\,/#&])*$")) && ((stu.newlstget1[i].PermanentCity.TrimEnd().TrimStart()).Length <= 100)))

                                {
                                    enq.AMST_PerCity = stu.newlstget1[i].PermanentCity.TrimEnd().TrimStart();
                                }
                                else
                                {
                                    stu.stuStatus = "Permanent city's input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Permanent Area Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }


                        //permanent country
                        long countryid = 0;
                        if (stu.newlstget1[i].PermanentCountry != null && stu.newlstget1[i].PermanentCountry != "")
                        {
                            if (stu.newlstget1[i].PermanentCountry.TrimEnd().TrimStart() != null && stu.newlstget1[i].PermanentCountry.TrimEnd().TrimStart() != "")
                            {
                                if (((Regex.IsMatch(stu.newlstget1[i].PermanentCountry.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\.\:\,/#&])*$")) && ((stu.newlstget1[i].PermanentCountry.TrimEnd().TrimStart()).Length <= 50)))
                                {
                                    var get_countryid = _AdmissionFormContext.Country.Where(t => t.IVRMMC_CountryName.Equals(stu.newlstget1[i].PermanentCountry.TrimEnd().TrimStart()));
                                    if (get_countryid.Count() > 0)
                                    {
                                        //enq.AMST_PerCountry = get_countryid.FirstOrDefault().IVRMMC_Id;
                                        countryid = get_countryid.FirstOrDefault().IVRMMC_Id;
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Permanent Country input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Permanent Country's input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Permanent Country Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }



                        //permanent state
                        if (stu.newlstget1[i].Permanentstate != null && stu.newlstget1[i].Permanentstate != "" && countryid != 0)
                        {
                            if (stu.newlstget1[i].Permanentstate.TrimEnd().TrimStart() != null && stu.newlstget1[i].Permanentstate.TrimEnd().TrimStart() != "")
                            {
                                if (((Regex.IsMatch(stu.newlstget1[i].Permanentstate.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,()\.\:\,/#&])*$")) && ((stu.newlstget1[i].Permanentstate.TrimEnd().TrimStart()).Length <= 100)))
                                {
                                    var get_countryid = _AdmissionFormContext.State.Where(t => t.IVRMMS_Name.Equals(stu.newlstget1[i].Permanentstate.TrimEnd().TrimStart()) && t.IVRMMC_Id == countryid);
                                    if (get_countryid.Count() > 0)
                                    {
                                        enq.AMST_PerState = get_countryid.FirstOrDefault().IVRMMS_Id;
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Permanent state input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Permanent state input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Permanent state Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }


                        //Permanent Pincode 
                        if (stu.newlstget1[i].PermanentPincode != null && stu.newlstget1[i].PermanentPincode != "")
                        {
                            if (Convert.ToString(stu.newlstget1[i].PermanentPincode.TrimEnd().TrimStart()) != null && Convert.ToString(stu.newlstget1[i].PermanentPincode.TrimEnd().TrimStart()) != "")
                            {
                                if (((Regex.IsMatch(Convert.ToString(stu.newlstget1[i].PermanentPincode.TrimEnd().TrimStart()), @"^([0-9])*$")) && (Convert.ToString(stu.newlstget1[i].PermanentPincode.TrimEnd().TrimStart()).Length == 6)))
                                {
                                    enq.AMST_PerPincode = Convert.ToInt32(stu.newlstget1[i].PermanentPincode.TrimEnd().TrimStart());
                                }
                                else
                                {
                                    stu.stuStatus = "Permanent Pincode is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Permanent Pincode is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }
                        //---End of the permanent address------//


                        //-------------------------Communication Address--------------------------//
                        //street
                        if (stu.newlstget1[i].PresentStreet != null && stu.newlstget1[i].PresentStreet != "")
                        {
                            if (stu.newlstget1[i].PresentStreet.TrimEnd().TrimStart() != null && stu.newlstget1[i].PresentStreet.TrimEnd().TrimStart() != "")
                            {
                                if (((Regex.IsMatch(stu.newlstget1[i].PresentStreet.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\(),\'\.\;\:\,/#&])*$")) && ((stu.newlstget1[i].PresentStreet.TrimEnd().TrimStart()).Length <= 100)))

                                {
                                    enq.AMST_ConStreet = stu.newlstget1[i].PresentStreet.TrimEnd().TrimStart();
                                }
                                else
                                {
                                    stu.stuStatus = "Present Street's input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Present Street Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }

                        }

                        //area
                        if (stu.newlstget1[i].PresentArea != null && stu.newlstget1[i].PresentArea != "")
                        {
                            if (stu.newlstget1[i].PresentArea.TrimEnd().TrimStart() != null && stu.newlstget1[i].PresentArea.TrimEnd().TrimStart() != "")
                            {
                                if (((Regex.IsMatch(stu.newlstget1[i].PresentArea.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\(),\'\;\.\:\,/#&])*$")) && ((stu.newlstget1[i].PresentArea.TrimEnd().TrimStart()).Length <= 100)))
                                {
                                    enq.AMST_ConArea = stu.newlstget1[i].PresentArea.TrimEnd().TrimStart();
                                }
                                else
                                {
                                    stu.stuStatus = "Present Area's input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Present Area Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }
                        //city
                        if (stu.newlstget1[i].PresentCity != null && stu.newlstget1[i].PresentCity != "")
                        {
                            if (stu.newlstget1[i].PresentCity.TrimEnd().TrimStart() != null && stu.newlstget1[i].PresentCity.TrimEnd().TrimStart() != "")
                            {
                                if (((Regex.IsMatch(stu.newlstget1[i].PresentCity.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\;\'\.()\:\,/#&])*$")) && ((stu.newlstget1[i].PresentCity.TrimEnd().TrimStart()).Length <= 100)))

                                {
                                    enq.AMST_ConCity = stu.newlstget1[i].PresentCity.TrimEnd().TrimStart();
                                }
                                else
                                {
                                    stu.stuStatus = "present city's input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "present Area Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }

                        //permanent country
                        long countryid1 = 0;
                        if (stu.newlstget1[i].PresentCountry != null && stu.newlstget1[i].PresentCountry != "")
                        {
                            if (stu.newlstget1[i].PresentCountry.TrimEnd().TrimStart() != null && stu.newlstget1[i].PresentCountry.TrimEnd().TrimStart() != "")
                            {
                                if (((Regex.IsMatch(stu.newlstget1[i].PresentCountry.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\.\:\,/])*$")) && ((stu.newlstget1[i].PresentCountry.TrimEnd().TrimStart()).Length <= 100)))
                                {
                                    var get_countryid = _AdmissionFormContext.Country.Where(t => t.IVRMMC_CountryName.Equals(stu.newlstget1[i].PresentCountry.TrimEnd().TrimStart()));
                                    if (get_countryid.Count() > 0)
                                    {
                                        enq.AMST_ConCountry = get_countryid.FirstOrDefault().IVRMMC_Id;
                                        countryid1 = get_countryid.FirstOrDefault().IVRMMC_Id;
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Present Country input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Present Country's input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Present Country Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }

                        }


                        //permanent state
                        if (stu.newlstget1[i].PresentState != null && stu.newlstget1[i].PresentState != "")
                        {
                            if (stu.newlstget1[i].PresentState.TrimEnd().TrimStart() != null && stu.newlstget1[i].PresentState.TrimEnd().TrimStart() != "")
                            {
                                if (((Regex.IsMatch(stu.newlstget1[i].PresentState.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\.\:\,/])*$")) && ((stu.newlstget1[i].PresentState.TrimEnd().TrimStart()).Length <= 100)))
                                {
                                    var get_countryid = _AdmissionFormContext.State.Where(t => t.IVRMMS_Name.Equals(stu.newlstget1[i].PresentState.TrimEnd().TrimStart()) && t.IVRMMC_Id == countryid1);
                                    if (get_countryid.Count() > 0)
                                    {
                                        enq.AMST_PerState = get_countryid.FirstOrDefault().IVRMMS_Id;
                                    }
                                    else
                                    {
                                        stu.stuStatus = "present state input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "present state input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "present state Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }
                        //Permanent Pincode 
                        if (stu.newlstget1[i].PresentPincode != null && stu.newlstget1[i].PresentPincode != "")
                        {
                            if (Convert.ToString(stu.newlstget1[i].PresentPincode.TrimEnd().TrimStart()) != null && Convert.ToString(stu.newlstget1[i].PresentPincode.TrimEnd().TrimStart()) != "")
                            {
                                if (((Regex.IsMatch(Convert.ToString(stu.newlstget1[i].PresentPincode.TrimEnd().TrimStart()), @"^([0-9])*$")) && (Convert.ToString(stu.newlstget1[i].PresentPincode.TrimEnd().TrimStart()).Length == 6)))
                                {
                                    enq.AMST_ConPincode = Convert.ToInt32(stu.newlstget1[i].PresentPincode.TrimEnd().TrimStart());
                                }
                                else
                                {
                                    stu.stuStatus = "Present Pincode is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Permanent Pincode is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }

                        }
                        //---End of the Communication address------//

                        ///-------------Aadhar Number -----------
                        if (stu.newlstget1[i].AadharNo != null && stu.newlstget1[i].AadharNo != "")
                        {
                            if (stu.newlstget1[i].AadharNo.TrimEnd().TrimStart() != null && stu.newlstget1[i].AadharNo.TrimEnd().TrimStart() != "")
                            {
                                //if (((Regex.IsMatch((stu.newlstget1[i].AadharNo.TrimEnd().TrimStart()), @"^([0-9])*$")) && ((stu.newlstget1[i].AadharNo.TrimEnd().TrimStart()).Length >= 0)) && ((stu.newlstget1[i].AadharNo.TrimEnd().TrimStart()).Length <= 12))
                                //{

                                if (((Regex.IsMatch((stu.newlstget1[i].AadharNo.TrimEnd().TrimStart()), @"^([0-9])*$"))))
                                //@"^([0-9])*$"
                                {
                                    if (((stu.newlstget1[i].AadharNo.TrimEnd().TrimStart()).Length >= 0))
                                    {
                                        if (((stu.newlstget1[i].AadharNo.TrimEnd().TrimStart()).Length <= 12))
                                        {
                                            enq.AMST_AadharNo = Convert.ToInt64(stu.newlstget1[i].AadharNo.TrimEnd().TrimStart());
                                        }
                                    }
                                }

                                // }
                                else
                                {
                                    stu.stuStatus = "Aadhar Number is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Aadhar Number can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }

                        //--- Student Mobile Number --//
                        if (stu.newlstget1[i].MobileNo != null && stu.newlstget1[i].MobileNo != "")
                        {
                            if ((Convert.ToString(stu.newlstget1[i].MobileNo.TrimEnd().TrimStart()) != null) && (Convert.ToString(stu.newlstget1[i].MobileNo.TrimEnd().TrimStart()) != ""))
                            {
                                string MatchPhoneNumberPattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
                                if (Regex.IsMatch((Convert.ToString(stu.newlstget1[i].MobileNo.TrimEnd().TrimStart())), MatchPhoneNumberPattern))
                                {
                                    enq.AMST_MobileNo = Convert.ToInt64(stu.newlstget1[i].MobileNo.TrimEnd().TrimStart());
                                }
                                else
                                {
                                    stu.stuStatus = "Mobile Number is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Mobile Number can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }

                        //--- Student Email id--//
                        if (stu.newlstget1[i].EmailID != null && stu.newlstget1[i].EmailID.TrimEnd() != "")
                        {
                            if ((Convert.ToString(stu.newlstget1[i].EmailID.TrimEnd().TrimStart()) != null) && (Convert.ToString(stu.newlstget1[i].EmailID.TrimEnd().TrimStart()) != ""))
                            {
                                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                                Match match = regex.Match(stu.newlstget1[i].EmailID.TrimEnd().TrimStart());
                                if (match.Success)
                                {
                                    enq.AMST_emailId = stu.newlstget1[i].EmailID.TrimEnd().TrimStart();
                                }
                                else
                                {
                                    stu.stuStatus = "Email Id is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Email Id can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }
                        //----------------Father Details-------------//
                        enq.AMST_FatherAliveFlag = "1";
                        if (stu.newlstget1[i].FatherName != null && stu.newlstget1[i].FatherName != "")
                        {
                            if ((stu.newlstget1[i].FatherName.TrimEnd().TrimStart() != null) && (stu.newlstget1[i].FatherName.TrimEnd().TrimStart() != ""))
                            {
                                if ((Regex.IsMatch(stu.newlstget1[i].FatherName.TrimEnd().TrimStart(), @"^[a-zA-Z.\-\s]+$")) && ((stu.newlstget1[i].FatherName.TrimEnd().TrimStart()).Length <= 50))
                                {
                                    enq.AMST_FatherName = stu.newlstget1[i].FatherName.TrimEnd().TrimStart();
                                }

                                else
                                {
                                    stu.stuStatus = "Father Name is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + "Student Admission Number";
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Father Name is can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + "Student Admission Number";
                                return stu;
                            }
                        }



                        //Father Mobile no
                        if (stu.newlstget1[i].Fathermobileno != null && stu.newlstget1[i].Fathermobileno != "")
                        {
                            if ((Convert.ToString(stu.newlstget1[i].Fathermobileno.TrimEnd().TrimStart()) != null) && (Convert.ToString(stu.newlstget1[i].Fathermobileno.TrimEnd().TrimStart()) != ""))
                            {
                                string MatchPhoneNumberPattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
                                if (Regex.IsMatch((Convert.ToString(stu.newlstget1[i].Fathermobileno.TrimEnd().TrimStart())), MatchPhoneNumberPattern))
                                {
                                    enq.AMST_FatherMobleNo = Convert.ToInt64(stu.newlstget1[i].Fathermobileno.TrimEnd().TrimStart());
                                }
                                else
                                {
                                    stu.stuStatus = "Father Mobile Number is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Father Mobile Number can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }
                        //Father Email id
                        if (stu.newlstget1[i].FatherEmailId != null && stu.newlstget1[i].FatherEmailId != "")
                        {
                            if ((Convert.ToString(stu.newlstget1[i].FatherEmailId.TrimEnd().TrimStart()) != null) && (Convert.ToString(stu.newlstget1[i].FatherEmailId.TrimEnd().TrimStart()) != ""))
                            {
                                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                                Match match = regex.Match(stu.newlstget1[i].FatherEmailId.TrimEnd().TrimStart());
                                if (match.Success)
                                {
                                    enq.AMST_FatheremailId = stu.newlstget1[i].FatherEmailId.TrimEnd().TrimStart();
                                }
                                else
                                {
                                    stu.stuStatus = "Father Email Id is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Father Email Id can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }
                        //-------Mother Details ----------//

                        enq.AMST_MotherAliveFlag = "1";
                        if (stu.newlstget1[i].MotherName != null && stu.newlstget1[i].MotherName != "")
                        {
                            if ((stu.newlstget1[i].MotherName.TrimEnd().TrimStart() != null) && (stu.newlstget1[i].MotherName.TrimEnd().TrimStart() != ""))
                            {
                                if ((Regex.IsMatch(stu.newlstget1[i].MotherName.TrimEnd().TrimStart(), @"^[a-zA-Z.\-\s]+$")) && ((stu.newlstget1[i].MotherName.TrimEnd().TrimStart()).Length <= 50))
                                {
                                    enq.AMST_MotherName = stu.newlstget1[i].MotherName.TrimEnd().TrimStart();
                                }

                                else
                                {
                                    stu.stuStatus = "Mother Name is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + "Student Admission Number";
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Mother Name is can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + "Student Admission Number";
                                return stu;
                            }
                        }
                        //Father Mobile no
                        if (stu.newlstget1[i].MotherMobileNo != null && stu.newlstget1[i].MotherMobileNo != "")
                        {
                            if ((Convert.ToString(stu.newlstget1[i].MotherMobileNo.TrimEnd().TrimStart()) != null) && (Convert.ToString(stu.newlstget1[i].MotherMobileNo.TrimEnd().TrimStart()) != ""))
                            {
                                string MatchPhoneNumberPattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
                                if (Regex.IsMatch((Convert.ToString(stu.newlstget1[i].MotherMobileNo.TrimEnd().TrimStart())), MatchPhoneNumberPattern))
                                {
                                    enq.AMST_MotherMobileNo = Convert.ToInt64(stu.newlstget1[i].MotherMobileNo.TrimEnd().TrimStart());
                                }
                                else
                                {
                                    stu.stuStatus = "Mother Mobile Number is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Mother Mobile Number can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }
                        //Father Email id
                        if (stu.newlstget1[i].MotherEmailId != null && stu.newlstget1[i].MotherEmailId != "")
                        {
                            if ((Convert.ToString(stu.newlstget1[i].MotherEmailId.TrimEnd().TrimStart()) != null) && (Convert.ToString(stu.newlstget1[i].MotherEmailId.TrimEnd().TrimStart()) != ""))
                            {
                                Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                                Match match = regex.Match(stu.newlstget1[i].MotherEmailId.TrimEnd().TrimStart());
                                if (match.Success)
                                {
                                    enq.AMST_MotherEmailId = stu.newlstget1[i].MotherEmailId.TrimEnd().TrimStart();
                                }
                                else
                                {
                                    stu.stuStatus = "Mother Email Id is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Mother Email Id can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }
                        //student nationality
                        //long countryid12 = 0;
                        if (stu.newlstget1[i].StudentNationality != null && stu.newlstget1[i].StudentNationality != "")
                        {
                            if (stu.newlstget1[i].StudentNationality.TrimEnd().TrimStart() != null && stu.newlstget1[i].StudentNationality.TrimEnd().TrimStart() != "")
                            {
                                if (((Regex.IsMatch(stu.newlstget1[i].StudentNationality.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\:\,])*$")) && ((stu.newlstget1[i].StudentNationality.TrimEnd().TrimStart()).Length <= 50)))
                                {
                                    var get_countryid = _AdmissionFormContext.Country.Where(t => t.IVRMMC_CountryName.Equals(stu.newlstget1[i].StudentNationality.TrimEnd().TrimStart()));
                                    if (get_countryid.Count() > 0)
                                    {
                                        enq.AMST_Nationality = get_countryid.FirstOrDefault().IVRMMC_Id;
                                        countryid1 = get_countryid.FirstOrDefault().IVRMMC_Id;
                                    }
                                    else
                                    {
                                        stu.stuStatus = "student Nationality input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "student Nationality input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "student Nationality Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                        }
                        var concession = _AdmissionFormContext.Fee_Master_ConcessionDMO.Where(a => a.MI_Id == stu.MI_Id && a.FMCC_ConcessionName.Equals("General") && a.FMCC_ActiveFlag == true).ToList();
                        if (concession.Count > 0)
                        {
                            enq.AMST_Concession_Type = concession.FirstOrDefault().FMCC_Id;
                        }
                        else
                        {
                            stu.stuStatus = "student Concession Type Is Not There Please Map The Fee Concession Type For Student And Concession Type Should Be 'General' Concession Type";
                            return stu;
                        }
                        enq.AMST_SOL = "S";
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                        failcount = failcount + 1;
                        string name = failnames;
                        finalnames += name + ",";
                        stu.stuStatus = "Select Proper Excel Sheet";
                        return stu;

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _acdimpl.LogInformation("Admission Import Checking Validation" + ex.Message);
            }
            return stu;
        }

        public async Task<ImportStudentWrapperDTO> checkvalidationimport(ImportStudentWrapperDTO stu)
        {
            int sucesscount = 0;
            int failcount = 0;
            string failnames = "";
            string finalnames = "";
            int counttt = 0;
            string studentadmno = "Student Admission Number";
            int age = 0;
            try
            {
                for (int i = 0; i < stu.newlstget1.Length; i++)
                {
                    counttt = 0;
                    try
                    {
                        var checkadmnoexists = _AdmissionFormContext.Adm_M_Student.Where(t => t.MI_Id == stu.MI_Id && t.AMST_AdmNo.Trim() == stu.newlstget1[i].AMSTAdmNo.Trim()).ToList();
                        if (checkadmnoexists.Count > 0)
                        {

                        }
                        else
                        {
                            _acdimpl.LogInformation("entered for loop");
                            Adm_M_Student enq = new Adm_M_Student();
                            try
                            {
                                var admno = "";
                                if (stu.newlstget1[i].AMSTAdmNo != null && stu.newlstget1[i].AMSTAdmNo != "")
                                {
                                    admno = stu.newlstget1[i].AMSTAdmNo.Trim().ToString();
                                    if ((admno != null))
                                    {
                                        if ((Regex.IsMatch(Convert.ToString(admno), @"^[a-zA-Z0-9/\-@]*$")))
                                        {
                                            enq.AMST_AdmNo = stu.newlstget1[i].AMSTAdmNo.Trim();
                                            counttt = counttt + 1;
                                        }

                                        else
                                        {
                                            stu.stuStatus = "Student Admission Number is Not Valid as It Should Contain Only Alphanumeric Characters";
                                            return stu;
                                        }
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Student Admission Number can not be null or Empty For   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                        return stu;
                                    }

                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                stu.stuStatus = "Student Admission Number can not be null or Empty For   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                return stu;
                            }

                            //--------Register Number ------------//
                            try
                            {
                                var regno = "";
                                if (stu.newlstget1[i].AMSTRegistrationNo != null && stu.newlstget1[i].AMSTRegistrationNo != "")
                                {
                                    regno = stu.newlstget1[i].AMSTRegistrationNo.Trim().ToString();
                                    if ((regno != null))
                                    {
                                        if ((Regex.IsMatch(Convert.ToString(regno), @"^[a-zA-Z0-9/\-@]*$")))
                                        {
                                            enq.AMST_RegistrationNo = stu.newlstget1[i].AMSTRegistrationNo.Trim();
                                            counttt = counttt + 1;
                                        }

                                        else
                                        {
                                            stu.stuStatus = "Student Registration Number is Not Valid as It Should Contain Only Alphanumeric Characters";
                                            return stu;
                                        }
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Student Registration Number can not be null or Empty For   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                        return stu;
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                stu.stuStatus = "Student Registration Number can not be null or Empty For   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                return stu;
                            }

                            //---------academic year----------//
                            long yearid = 0;
                            try
                            {
                                var year = "";
                                if (stu.newlstget1[i].year != null && stu.newlstget1[i].year != "")
                                {
                                    year = stu.newlstget1[i].year.Trim().ToString();
                                    if ((year != null))
                                    {
                                        if ((Regex.IsMatch(Convert.ToString(year), @"^[0-9\s-]*$")))
                                        {
                                            var check_yearid = _AdmissionFormContext.year.Where(t => t.ASMAY_Year.Equals(year) && t.MI_Id == stu.MI_Id).ToList();
                                            if (check_yearid.Count() > 0)
                                            {
                                                enq.ASMAY_Id = check_yearid.FirstOrDefault().ASMAY_Id;
                                                yearid = check_yearid.FirstOrDefault().ASMAY_Id;
                                                counttt = counttt + 1;
                                            }
                                            else
                                            {
                                                stu.stuStatus = "Year is Not Available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                                return stu;
                                            }

                                        }
                                        else
                                        {
                                            stu.stuStatus = "Student Year is Not Valid as It Should Contain Only numeric with -";
                                            return stu;
                                        }
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Student Year can not be null or Empty For   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                        return stu;
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                stu.stuStatus = "StudentYear can not be null or Empty   " + stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                return stu;
                            }

                            //----------First Name---------//
                            try
                            {
                                if (stu.newlstget1[i].FirstName != null && stu.newlstget1[i].FirstName != "")
                                {
                                    if ((stu.newlstget1[i].FirstName.TrimEnd().TrimStart() != null) && (stu.newlstget1[i].FirstName.TrimEnd().TrimStart() != ""))
                                    {
                                        if ((Regex.IsMatch(stu.newlstget1[i].FirstName.TrimEnd().TrimStart(), @"^[a-zA-Z.\s]+$")) && ((stu.newlstget1[i].FirstName.TrimEnd().TrimStart()).Length <= 50))
                                        {
                                            enq.AMST_FirstName = stu.newlstget1[i].FirstName.TrimEnd().TrimStart();
                                            counttt = counttt + 1;
                                        }

                                        else
                                        {
                                            stu.stuStatus = "Student First Name is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + "Student Admission Number";
                                            return stu;
                                        }
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Student First Name can not be null for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + "Student Admission Number";
                                        return stu;
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                                stu.stuStatus = "Student First Name Header Is Changed , Header Should Be FirstName";
                                return stu;
                            }
                            var middlename = "";
                            var middlename1 = "";
                            if (stu.newlstget1[i].MiddleName != null && stu.newlstget1[i].MiddleName != "")
                            {
                                middlename = stu.newlstget1[i].MiddleName.TrimEnd().TrimStart();
                                middlename1 = stu.newlstget1[i].MiddleName.Trim();


                                if (middlename != "Null" && middlename != "NULL")
                                {
                                    if ((Regex.IsMatch(middlename, @"^[a-zA-Z.\s]+$")))
                                    {
                                        enq.AMST_MiddleName = stu.newlstget1[i].MiddleName.TrimEnd().TrimStart();
                                        counttt = counttt + 1;
                                    }
                                    else
                                    {
                                        enq.AMST_MiddleName = stu.newlstget1[i].MiddleName.TrimEnd().TrimStart();
                                        counttt = counttt + 1;
                                    }
                                }
                                else
                                {
                                    enq.AMST_MiddleName = "";
                                    counttt = counttt + 1;
                                }
                            }




                            if (stu.newlstget1[i].LastName != null && stu.newlstget1[i].LastName != "")
                            {
                                if (stu.newlstget1[i].LastName.TrimEnd().TrimStart() != "Null" && stu.newlstget1[i].LastName.TrimEnd().TrimStart() != "NULL")
                                {
                                    if (((Regex.IsMatch(stu.newlstget1[i].LastName.TrimEnd().TrimStart(), @"^[a-zA-Z.\s]+$"))))
                                    {
                                        enq.AMST_LastName = stu.newlstget1[i].LastName.TrimEnd().TrimStart();
                                        counttt = counttt + 1;
                                    }
                                    else
                                    {
                                        enq.AMST_LastName = stu.newlstget1[i].LastName.TrimEnd().TrimStart();
                                        counttt = counttt + 1;
                                    }
                                }
                                else
                                {
                                    enq.AMST_LastName = "";
                                    counttt = counttt + 1;
                                }
                            }





                            //---Date Of Admissiom----//

                            var dateFormats = new[] { "dd.MM.yyyy", "dd-MM-yyyy", "dd/MM/yyyy" };
                            string readAddMeeting = "";
                            DateTime scheduleDate_DOB;
                            DateTime scheduleDate_DOJ;
                            bool validDate = false;
                            if (stu.newlstget1[i].amstdate != null && stu.newlstget1[i].amstdate != "")
                            {
                                readAddMeeting = stu.newlstget1[i].amstdate.TrimEnd().TrimStart();

                                validDate = DateTime.TryParseExact(
                                   readAddMeeting,
                                   dateFormats,
                                   DateTimeFormatInfo.InvariantInfo,
                                   DateTimeStyles.None,
                                   out scheduleDate_DOJ);
                                if (validDate)
                                {
                                    enq.AMST_Date = scheduleDate_DOJ;
                                    counttt = counttt + 1;
                                }
                                else
                                {
                                    stu.stuStatus = "Date Of Join is not Valid, Please Enter in dd/MM/yyyy format for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }
                            else
                            {
                                stu.stuStatus = "Date Of Join is Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }


                            //----gender---//
                            string gender = "";
                            if (stu.newlstget1[i].Gender != null && stu.newlstget1[i].Gender != "")
                            {
                                gender = stu.newlstget1[i].Gender.TrimEnd().TrimStart();

                                if (gender != null && gender != "")
                                {
                                    if (((Regex.IsMatch(gender, @"^([a-zA-Z/s])*$")) &&
                                        (gender.Equals("Male") || gender.Equals("Female") || gender.Equals("F") || gender.Equals("M") || gender.Equals("Other") || (gender.Equals("MALE") || gender.Equals("FEMALE"))) && (gender.Length <= 6 || gender.Length <= 1)))
                                    {
                                        if (gender == "M")
                                        {
                                            enq.AMST_Sex = "Male";
                                            counttt = counttt + 1;
                                        }
                                        else if (gender == "F")
                                        {
                                            enq.AMST_Sex = "Female";
                                            counttt = counttt + 1;
                                        }
                                        else
                                        {
                                            enq.AMST_Sex = gender;
                                            counttt = counttt + 1;
                                        }
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Sudent Gender is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Sudent Gender is required for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }


                            //--Date Of Birth--//
                            if (stu.newlstget1[i].DOB != null && stu.newlstget1[i].DOB != "")
                            {
                                readAddMeeting = stu.newlstget1[i].DOB.TrimEnd().TrimStart();
                                validDate = DateTime.TryParseExact(
                                   readAddMeeting,
                                   dateFormats,
                                   DateTimeFormatInfo.InvariantInfo,
                                   DateTimeStyles.None,
                                   out scheduleDate_DOB);
                                if (validDate)
                                {
                                    enq.AMST_DOB = scheduleDate_DOB;
                                    counttt = counttt + 1;
                                }
                                else
                                {
                                    stu.stuStatus = "Date Of Birth is not Valid, Please Enter in dd/MM/yyyy format for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }



                            //--DOB Words --//
                            if (stu.newlstget1[i].AMSTDOBWords != null && stu.newlstget1[i].AMSTDOBWords != "")
                            {
                                if (stu.newlstget1[i].AMSTDOBWords.TrimEnd().TrimStart() != null && stu.newlstget1[i].AMSTDOBWords.TrimEnd().TrimStart() != "")
                                {
                                    enq.AMST_DOB_Words = stu.newlstget1[i].AMSTDOBWords.TrimEnd().TrimStart();
                                    counttt = counttt + 1;
                                }
                                else
                                {
                                    enq.AMST_DOB_Words = stu.newlstget1[i].AMSTDOBWords.TrimEnd().TrimStart();
                                    counttt = counttt + 1;
                                }
                            }


                            //--Age calculation--//
                            if (stu.newlstget1[i].DOB != null && stu.newlstget1[i].DOB != "")
                            {
                                age = Convert.ToInt32(DateTime.Now.Year - Convert.ToDateTime(stu.newlstget1[i].DOB.TrimEnd().TrimStart()).Year);

                                if (age > 0)
                                {
                                    age -= Convert.ToInt32(DateTime.Now < Convert.ToDateTime(stu.newlstget1[i].DOB.TrimEnd().TrimStart()).Date.AddYears(age));
                                    counttt = counttt + 1;
                                }
                                else
                                {
                                    age = 0;
                                    counttt = counttt + 1;
                                }
                                enq.PASR_Age = age;
                            }

                            long classid = 0;
                            string classname = "";
                            if (stu.newlstget1[i].Class != null && stu.newlstget1[i].Class != "")
                            {
                                classname = stu.newlstget1[i].Class.TrimEnd();
                                //--getting class id from class name --//
                                var check_class = _AdmissionFormContext.School_M_Class.Where(t => t.MI_Id == stu.MI_Id && t.ASMCL_ClassName.TrimEnd().TrimStart().ToLower() == stu.newlstget1[i].Class.TrimEnd().TrimStart().ToLower()).ToList();
                                if (check_class.Count > 0)
                                {
                                    var class_id = _AdmissionFormContext.School_M_Class.Where(t => t.MI_Id == stu.MI_Id && t.ASMCL_ClassName.TrimEnd().TrimStart().ToLower() == stu.newlstget1[i].Class.TrimEnd().TrimStart().ToLower()).FirstOrDefault();
                                    enq.ASMCL_Id = class_id.ASMCL_Id;
                                    classid = class_id.ASMCL_Id;
                                    counttt = counttt + 1;
                                }
                                else
                                {
                                    stu.stuStatus = "Class is Not Available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }

                            }


                            //----getting class category id---//                           
                            var get_class_category = (from a in _AdmissionFormContext.Masterclasscategory
                                                      from b in _AdmissionFormContext.Adm_M_Stu_Cat
                                                      where (a.AMC_Id == b.AMC_Id && a.MI_Id == stu.MI_Id && a.ASMAY_Id == yearid && a.ASMCL_Id == classid)
                                                      select new
                                                      {
                                                          asmcc_id = a.ASMCC_Id
                                                      }).ToList();

                            if (get_class_category.Count > 0)
                            {
                                enq.AMC_Id = get_class_category.FirstOrDefault().asmcc_id;
                                counttt = counttt + 1;
                            }
                            else
                            {
                                stu.stuStatus = "Class Category is Not Available for this acadmic year" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                return stu;
                            }
                            if (stu.newlstget1[i].BloodGroup != null && stu.newlstget1[i].BloodGroup != "")
                            {
                                enq.AMST_BloodGroup = stu.newlstget1[i].BloodGroup;
                            }

                            counttt = counttt + 1;

                            //----Mother Tongue----//
                            if (stu.newlstget1[i].MotherTongue != null && stu.newlstget1[i].MotherTongue != "")
                            {
                                if (((Regex.IsMatch(stu.newlstget1[i].MotherTongue.TrimEnd().TrimStart(), @"^[a-zA-Z\s]+$")) || (stu.newlstget1[i].MotherTongue.TrimEnd().TrimStart() == null) || (stu.newlstget1[i].MotherTongue.TrimEnd().TrimStart() == "")))
                                {
                                    enq.AMST_MotherTongue = stu.newlstget1[i].MotherTongue.TrimEnd().TrimStart();
                                    counttt = counttt + 1;
                                }
                                else
                                {
                                    stu.stuStatus = "Student MotherTongue is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }


                            //---Religion---//
                            if (stu.newlstget1[i].Religion != null && stu.newlstget1[i].Religion != "")
                            {
                                if (((Regex.IsMatch(stu.newlstget1[i].Religion, @"^[a-zA-Z\s]+$"))))
                                {
                                    var get_religionid = _AdmissionFormContext.Religion.Where(t => t.IVRMMR_Name.Equals(stu.newlstget1[i].Religion.TrimEnd().TrimStart())).ToList();
                                    if (get_religionid.Count() > 0)
                                    {
                                        enq.IVRMMR_Id = get_religionid.FirstOrDefault().IVRMMR_Id;
                                        counttt = counttt + 1;
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Student Religion is not available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }

                                }
                                else
                                {
                                    stu.stuStatus = "Student Religion is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }


                            //--caste --//
                            long get_casteid1 = 0;
                            if (stu.newlstget1[i].Caste != null && stu.newlstget1[i].Caste != "")
                            {
                                if (((Regex.IsMatch(stu.newlstget1[i].Caste, @"^[a-zA-Z\s]+$"))))
                                {
                                    var get_casteid = _AdmissionFormContext.Caste.Where(t => t.IMC_CasteName.Equals(stu.newlstget1[i].Caste.TrimEnd().TrimStart()) && t.MI_Id == stu.MI_Id).ToList();
                                    if (get_casteid.Count() > 0)
                                    {
                                        enq.IC_Id = get_casteid.FirstOrDefault().IMC_Id;
                                        get_casteid1 = get_casteid.FirstOrDefault().IMC_Id;
                                        counttt = counttt + 1;
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Student caste is not available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }

                                }
                                else
                                {
                                    stu.stuStatus = "Student caste is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }



                            //geting caste category id
                            // var castecategoryid = "";
                            if (stu.newlstget1[i].Caste != null && stu.newlstget1[i].Caste != "")
                            {
                                if (((Regex.IsMatch(stu.newlstget1[i].Caste, @"^[a-zA-Z0-9\s]+$"))))
                                {
                                    var get_castecategoryid = (from a in _AdmissionFormContext.CasteCategory
                                                               from b in _AdmissionFormContext.Caste
                                                               where (a.IMCC_Id == b.IMCC_Id && b.MI_Id == stu.MI_Id && b.IMC_Id == Convert.ToInt64(get_casteid1))
                                                               select new
                                                               {
                                                                   castecategoryid = a.IMCC_Id
                                                               }).ToList();
                                    if (get_castecategoryid.Count() > 0)
                                    {
                                        enq.IMCC_Id = get_castecategoryid.FirstOrDefault().castecategoryid;
                                        counttt = counttt + 1;
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Student caste category is not available for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno + " " + "Please Map The Caste  Category to" + stu.newlstget1[i].Caste;
                                        return stu;
                                    }

                                }
                                else
                                {
                                    stu.stuStatus = "Student caste category is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }


                            //----------------Permanent Address--------------------//

                            //street
                            var PermanentStreet = "";
                            if (stu.newlstget1[i].PermanentStreet != null && stu.newlstget1[i].PermanentStreet != "")
                            {
                                PermanentStreet = stu.newlstget1[i].PermanentStreet.Trim();
                                if (PermanentStreet != null && PermanentStreet != "")
                                {
                                    if (((Regex.IsMatch(PermanentStreet, @"^([a-zA-Z0-9\s\-\,\.\;\():\,\'/&#])*$")) && ((PermanentStreet).Length <= 100)))

                                    {
                                        enq.AMST_PerStreet = stu.newlstget1[i].PermanentStreet.TrimEnd().TrimStart();
                                        counttt = counttt + 1;
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Permanent Street's input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Permanent Street Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }


                            //area
                            if (stu.newlstget1[i].PermanentArea != null && stu.newlstget1[i].PermanentArea != "")
                            {
                                if (stu.newlstget1[i].PermanentArea.TrimEnd().TrimStart() != null && stu.newlstget1[i].PermanentArea.TrimEnd().TrimStart() != "")
                                {
                                    if (((Regex.IsMatch(stu.newlstget1[i].PermanentArea.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,()\'\.\;\:\,/&#])*$")) && ((stu.newlstget1[i].PermanentArea.TrimEnd().TrimStart()).Length <= 100)))

                                    {
                                        enq.AMST_PerArea = stu.newlstget1[i].PermanentArea.TrimEnd().TrimStart();
                                        counttt = counttt + 1;
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Permanent Area's input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Permanent Area Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }

                            //city
                            if (stu.newlstget1[i].PermanentCity != null && stu.newlstget1[i].PermanentCity != "")
                            {
                                if (stu.newlstget1[i].PermanentCity.TrimEnd().TrimStart() != null && stu.newlstget1[i].PermanentCity.TrimEnd().TrimStart() != "")
                                {
                                    if (((Regex.IsMatch(stu.newlstget1[i].PermanentCity.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\'\.\;\:\,/#&])*$")) && ((stu.newlstget1[i].PermanentCity.TrimEnd().TrimStart()).Length <= 100)))

                                    {
                                        enq.AMST_PerCity = stu.newlstget1[i].PermanentCity.TrimEnd().TrimStart();
                                        counttt = counttt + 1;
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Permanent city's input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Permanent Area Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }


                            //permanent country
                            long countryid = 0;
                            if (stu.newlstget1[i].PermanentCountry != null && stu.newlstget1[i].PermanentCountry != "")
                            {
                                if (stu.newlstget1[i].PermanentCountry.TrimEnd().TrimStart() != null && stu.newlstget1[i].PermanentCountry.TrimEnd().TrimStart() != "")
                                {
                                    if (((Regex.IsMatch(stu.newlstget1[i].PermanentCountry.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\.\:\,/#&])*$")) && ((stu.newlstget1[i].PermanentCountry.TrimEnd().TrimStart()).Length <= 50)))
                                    {
                                        var get_countryid = _AdmissionFormContext.Country.Where(t => t.IVRMMC_CountryName.Equals(stu.newlstget1[i].PermanentCountry.TrimEnd().TrimStart()));
                                        if (get_countryid.Count() > 0)
                                        {
                                            //enq.AMST_PerCountry = get_countryid.FirstOrDefault().IVRMMC_Id;
                                            countryid = get_countryid.FirstOrDefault().IVRMMC_Id;
                                            counttt = counttt + 1;
                                        }
                                        else
                                        {
                                            stu.stuStatus = "Permanent Country input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                            return stu;
                                        }
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Permanent Country's input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Permanent Country Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }



                            //permanent state
                            if (stu.newlstget1[i].Permanentstate != null && stu.newlstget1[i].Permanentstate != "")
                            {
                                if (stu.newlstget1[i].Permanentstate.TrimEnd().TrimStart() != null && stu.newlstget1[i].Permanentstate.TrimEnd().TrimStart() != "")
                                {
                                    if (((Regex.IsMatch(stu.newlstget1[i].Permanentstate.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,()\.\:\,/#&])*$")) && ((stu.newlstget1[i].Permanentstate.TrimEnd().TrimStart()).Length <= 100)))
                                    {
                                        var get_countryid = _AdmissionFormContext.State.Where(t => t.IVRMMS_Name.Equals(stu.newlstget1[i].Permanentstate.TrimEnd().TrimStart()) && t.IVRMMC_Id == countryid);
                                        if (get_countryid.Count() > 0)
                                        {
                                            enq.AMST_PerState = get_countryid.FirstOrDefault().IVRMMS_Id;
                                            counttt = counttt + 1;
                                        }
                                        else
                                        {
                                            stu.stuStatus = "Permanent state input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                            return stu;
                                        }
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Permanent state input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Permanent state Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }


                            //Permanent Pincode 
                            if (stu.newlstget1[i].PermanentPincode != null && stu.newlstget1[i].PermanentPincode != "")
                            {
                                if (Convert.ToString(stu.newlstget1[i].PermanentPincode.TrimEnd().TrimStart()) != null && Convert.ToString(stu.newlstget1[i].PermanentPincode.TrimEnd().TrimStart()) != "")
                                {
                                    if (((Regex.IsMatch(Convert.ToString(stu.newlstget1[i].PermanentPincode.TrimEnd().TrimStart()), @"^([0-9])*$")) && (Convert.ToString(stu.newlstget1[i].PermanentPincode.TrimEnd().TrimStart()).Length == 6)))
                                    {
                                        enq.AMST_PerPincode = Convert.ToInt32(stu.newlstget1[i].PermanentPincode.TrimEnd().TrimStart());
                                        counttt = counttt + 1;
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Permanent Pincode is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Permanent Pincode is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }

                            //---End of the permanent address------//


                            //-------------------------Communication Address--------------------------//
                            //street
                            if (stu.newlstget1[i].PresentStreet != null && stu.newlstget1[i].PresentStreet != "")
                            {
                                if (stu.newlstget1[i].PresentStreet.TrimEnd().TrimStart() != null && stu.newlstget1[i].PresentStreet.TrimEnd().TrimStart() != "")
                                {
                                    if (((Regex.IsMatch(stu.newlstget1[i].PresentStreet.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\(),\'\.\;\:\,/#&])*$")) && ((stu.newlstget1[i].PresentStreet.TrimEnd().TrimStart()).Length <= 100)))

                                    {
                                        enq.AMST_ConStreet = stu.newlstget1[i].PresentStreet.TrimEnd().TrimStart();
                                        counttt = counttt + 1;
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Present Street's input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Present Street Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }

                            }

                            //area
                            if (stu.newlstget1[i].PresentArea != null && stu.newlstget1[i].PresentArea != "")
                            {
                                if (stu.newlstget1[i].PresentArea.TrimEnd().TrimStart() != null && stu.newlstget1[i].PresentArea.TrimEnd().TrimStart() != "")
                                {
                                    if (((Regex.IsMatch(stu.newlstget1[i].PresentArea.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\(),\'\;\.\:\,/#&])*$")) && ((stu.newlstget1[i].PresentArea.TrimEnd().TrimStart()).Length <= 100)))
                                    {
                                        enq.AMST_ConArea = stu.newlstget1[i].PresentArea.TrimEnd().TrimStart();
                                        counttt = counttt + 1;
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Present Area's input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Present Area Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }

                            //city
                            if (stu.newlstget1[i].PresentCity != null && stu.newlstget1[i].PresentCity != "")
                            {
                                if (stu.newlstget1[i].PresentCity.TrimEnd().TrimStart() != null && stu.newlstget1[i].PresentCity.TrimEnd().TrimStart() != "")
                                {
                                    if (((Regex.IsMatch(stu.newlstget1[i].PresentCity.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\;\'\.()\:\,/#&])*$")) && ((stu.newlstget1[i].PresentCity.TrimEnd().TrimStart()).Length <= 100)))

                                    {
                                        enq.AMST_ConCity = stu.newlstget1[i].PresentCity.TrimEnd().TrimStart();
                                        counttt = counttt + 1;
                                    }
                                    else
                                    {
                                        stu.stuStatus = "present city's input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "present Area Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }


                            //permanent country
                            long countryid1 = 0;
                            if (stu.newlstget1[i].PresentCountry != null && stu.newlstget1[i].PresentCountry != "")
                            {
                                if (stu.newlstget1[i].PresentCountry.TrimEnd().TrimStart() != null && stu.newlstget1[i].PresentCountry.TrimEnd().TrimStart() != "")
                                {
                                    if (((Regex.IsMatch(stu.newlstget1[i].PresentCountry.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\.\:\,/])*$")) && ((stu.newlstget1[i].PresentCountry.TrimEnd().TrimStart()).Length <= 100)))
                                    {
                                        var get_countryid = _AdmissionFormContext.Country.Where(t => t.IVRMMC_CountryName.Equals(stu.newlstget1[i].PresentCountry.TrimEnd().TrimStart()));
                                        if (get_countryid.Count() > 0)
                                        {
                                            enq.AMST_ConCountry = get_countryid.FirstOrDefault().IVRMMC_Id;
                                            countryid1 = get_countryid.FirstOrDefault().IVRMMC_Id;
                                            counttt = counttt + 1;
                                        }
                                        else
                                        {
                                            stu.stuStatus = "Present Country input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                            return stu;
                                        }
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Present Country's input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Present Country Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }



                            //permanent state
                            if (stu.newlstget1[i].PresentState != null && stu.newlstget1[i].PresentState != "")
                            {
                                if (stu.newlstget1[i].PresentState.TrimEnd().TrimStart() != null && stu.newlstget1[i].PresentState.TrimEnd().TrimStart() != "")
                                {
                                    if (((Regex.IsMatch(stu.newlstget1[i].PresentState.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\.\:\,/])*$")) && ((stu.newlstget1[i].PresentState.TrimEnd().TrimStart()).Length <= 100)))
                                    {
                                        var get_countryid = _AdmissionFormContext.State.Where(t => t.IVRMMS_Name.Equals(stu.newlstget1[i].PresentState.TrimEnd().TrimStart()) && t.IVRMMC_Id == countryid1);
                                        if (get_countryid.Count() > 0)
                                        {
                                            enq.AMST_PerState = get_countryid.FirstOrDefault().IVRMMS_Id;
                                            counttt = counttt + 1;
                                        }
                                        else
                                        {
                                            stu.stuStatus = "present state input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                            return stu;
                                        }
                                    }
                                    else
                                    {
                                        stu.stuStatus = "present state input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "present state Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }


                            //Permanent Pincode 
                            if (stu.newlstget1[i].PresentPincode != null && stu.newlstget1[i].PresentPincode != "")
                            {
                                if (Convert.ToString(stu.newlstget1[i].PresentPincode.TrimEnd().TrimStart()) != null && Convert.ToString(stu.newlstget1[i].PresentPincode.TrimEnd().TrimStart()) != "")
                                {
                                    if (((Regex.IsMatch(Convert.ToString(stu.newlstget1[i].PresentPincode.TrimEnd().TrimStart()), @"^([0-9])*$")) && (Convert.ToString(stu.newlstget1[i].PresentPincode.TrimEnd().TrimStart()).Length == 6)))
                                    {
                                        enq.AMST_ConPincode = Convert.ToInt32(stu.newlstget1[i].PresentPincode.TrimEnd().TrimStart());
                                        counttt = counttt + 1;
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Present Pincode is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Permanent Pincode is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }

                            //---End of the Communication address------//

                            ///-------------Aadhar Number -----------
                            if (stu.newlstget1[i].AadharNo != null && stu.newlstget1[i].AadharNo != "")
                            {
                                if (stu.newlstget1[i].AadharNo.TrimEnd().TrimStart() != null && stu.newlstget1[i].AadharNo.TrimEnd().TrimStart() != "")
                                {
                                    if (((Regex.IsMatch((stu.newlstget1[i].AadharNo.TrimEnd().TrimStart()), @"^([0-9])*$")) && ((stu.newlstget1[i].AadharNo.TrimEnd().TrimStart()).Length >= 0)) && ((stu.newlstget1[i].AadharNo.TrimEnd().TrimStart()).Length <= 12))
                                    {
                                        enq.AMST_AadharNo = Convert.ToInt64(stu.newlstget1[i].AadharNo.TrimEnd().TrimStart());
                                        counttt = counttt + 1;
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Aadhar Number is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Aadhar Number can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }


                            //--- Student Mobile Number --//
                            if (stu.newlstget1[i].MobileNo != null && stu.newlstget1[i].MobileNo != "")
                            {
                                if ((Convert.ToString(stu.newlstget1[i].MobileNo.TrimEnd().TrimStart()) != null) && (Convert.ToString(stu.newlstget1[i].MobileNo.TrimEnd().TrimStart()) != ""))
                                {
                                    string MatchPhoneNumberPattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
                                    if (Regex.IsMatch((Convert.ToString(stu.newlstget1[i].MobileNo.TrimEnd().TrimStart())), MatchPhoneNumberPattern))
                                    {
                                        enq.AMST_MobileNo = Convert.ToInt64(stu.newlstget1[i].MobileNo.TrimEnd().TrimStart());
                                        counttt = counttt + 1;
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Mobile Number is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Mobile Number can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }


                            //--- Student Email id--//
                            if (stu.newlstget1[i].EmailID != null && stu.newlstget1[i].EmailID != "")
                            {
                                if ((Convert.ToString(stu.newlstget1[i].EmailID.TrimEnd().TrimStart()) != null) && (Convert.ToString(stu.newlstget1[i].EmailID.TrimEnd().TrimStart()) != ""))
                                {
                                    Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                                    Match match = regex.Match(stu.newlstget1[i].EmailID.TrimEnd().TrimStart());
                                    if (match.Success)
                                    {
                                        enq.AMST_emailId = stu.newlstget1[i].EmailID.TrimEnd().TrimStart();
                                        counttt = counttt + 1;
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Email Id is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Email Id can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }


                            //----------------Father Details-------------//

                            enq.AMST_FatherAliveFlag = "1";
                            if (stu.newlstget1[i].FatherName != null && stu.newlstget1[i].FatherName != "")
                            {
                                if ((stu.newlstget1[i].FatherName.TrimEnd().TrimStart() != null) && (stu.newlstget1[i].FatherName.TrimEnd().TrimStart() != ""))
                                {
                                    if ((Regex.IsMatch(stu.newlstget1[i].FatherName.TrimEnd().TrimStart(), @"^[a-zA-Z.\-\s]+$")) && ((stu.newlstget1[i].FatherName.TrimEnd().TrimStart()).Length <= 50))
                                    {
                                        enq.AMST_FatherName = stu.newlstget1[i].FatherName.TrimEnd().TrimStart();
                                        counttt = counttt + 1;
                                    }

                                    else
                                    {
                                        stu.stuStatus = "Father Name is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + "Student Admission Number";
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Father Name is can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + "Student Admission Number";
                                    return stu;
                                }
                            }


                            //Father Mobile no
                            if (stu.newlstget1[i].Fathermobileno != null && stu.newlstget1[i].Fathermobileno != "")
                            {
                                if ((Convert.ToString(stu.newlstget1[i].Fathermobileno.TrimEnd().TrimStart()) != null) && (Convert.ToString(stu.newlstget1[i].Fathermobileno.TrimEnd().TrimStart()) != ""))
                                {
                                    string MatchPhoneNumberPattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
                                    if (Regex.IsMatch((Convert.ToString(stu.newlstget1[i].Fathermobileno.TrimEnd().TrimStart())), MatchPhoneNumberPattern))
                                    {
                                        enq.AMST_FatherMobleNo = Convert.ToInt64(stu.newlstget1[i].Fathermobileno.TrimEnd().TrimStart());
                                        counttt = counttt + 1;
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Father Mobile Number is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Father Mobile Number can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }


                            //Father Email id
                            if (stu.newlstget1[i].FatherEmailId != null && stu.newlstget1[i].FatherEmailId != "")
                            {
                                if ((Convert.ToString(stu.newlstget1[i].FatherEmailId.TrimEnd().TrimStart()) != null) && (Convert.ToString(stu.newlstget1[i].FatherEmailId.TrimEnd().TrimStart()) != ""))
                                {
                                    Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                                    Match match = regex.Match(stu.newlstget1[i].FatherEmailId.TrimEnd().TrimStart());
                                    if (match.Success)
                                    {
                                        enq.AMST_FatheremailId = stu.newlstget1[i].FatherEmailId.TrimEnd().TrimStart();
                                        counttt = counttt + 1;
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Father Email Id is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Father Email Id can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }


                            //-------Mother Details ----------//

                            enq.AMST_MotherAliveFlag = "1";
                            if (stu.newlstget1[i].MotherName != null && stu.newlstget1[i].MotherName != "")
                            {
                                if ((stu.newlstget1[i].MotherName.TrimEnd().TrimStart() != null) && (stu.newlstget1[i].MotherName.TrimEnd().TrimStart() != ""))
                                {
                                    if ((Regex.IsMatch(stu.newlstget1[i].MotherName.TrimEnd().TrimStart(), @"^[a-zA-Z.\-\s]+$")) && ((stu.newlstget1[i].MotherName.TrimEnd().TrimStart()).Length <= 50))
                                    {
                                        enq.AMST_MotherName = stu.newlstget1[i].MotherName.TrimEnd().TrimStart();
                                        counttt = counttt + 1;
                                    }

                                    else
                                    {
                                        stu.stuStatus = "Mother Name is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + "Student Admission Number";
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Mother Name is can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + "Student Admission Number";
                                    return stu;
                                }
                            }
                            if (stu.newlstget1[i].MotherSurname != null && stu.newlstget1[i].MotherSurname != "")
                            {
                                if ((stu.newlstget1[i].MotherSurname.TrimEnd().TrimStart() != null) && (stu.newlstget1[i].MotherSurname.TrimEnd().TrimStart() != ""))
                                {
                                    if ((Regex.IsMatch(stu.newlstget1[i].MotherSurname.TrimEnd().TrimStart(), @"^[a-zA-Z.\-\s]+$")) && ((stu.newlstget1[i].MotherSurname.TrimEnd().TrimStart()).Length <= 50))
                                    {
                                        enq.AMST_MotherSurname = stu.newlstget1[i].MotherSurname.TrimEnd().TrimStart();
                                        counttt = counttt + 1;
                                    }

                                    else
                                    {
                                        stu.stuStatus = "Mother Surname is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + "Student Admission Number";
                                        return stu;
                                    }
                                }
                                else
                                {
                                    enq.AMST_MotherSurname = stu.newlstget1[i].MotherSurname.TrimEnd().TrimStart();
                                    counttt = counttt + 1;
                                }
                            }


                            //Father Mobile no
                            if (stu.newlstget1[i].MotherMobileNo != null && stu.newlstget1[i].MotherMobileNo != "")
                            {
                                if ((Convert.ToString(stu.newlstget1[i].MotherMobileNo.TrimEnd().TrimStart()) != null) && (Convert.ToString(stu.newlstget1[i].MotherMobileNo.TrimEnd().TrimStart()) != ""))
                                {
                                    string MatchPhoneNumberPattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
                                    if (Regex.IsMatch((Convert.ToString(stu.newlstget1[i].MotherMobileNo.TrimEnd().TrimStart())), MatchPhoneNumberPattern))
                                    {
                                        enq.AMST_MotherMobileNo = Convert.ToInt64(stu.newlstget1[i].MotherMobileNo.TrimEnd().TrimStart());
                                        counttt = counttt + 1;
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Mother Mobile Number is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Mother Mobile Number can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }


                            //Father Email id
                            if (stu.newlstget1[i].MotherEmailId != null && stu.newlstget1[i].MotherEmailId != "")
                            {
                                if ((Convert.ToString(stu.newlstget1[i].MotherEmailId.TrimEnd().TrimStart()) != null) && (Convert.ToString(stu.newlstget1[i].MotherEmailId.TrimEnd().TrimStart()) != ""))
                                {
                                    Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                                    Match match = regex.Match(stu.newlstget1[i].MotherEmailId.TrimEnd().TrimStart());
                                    if (match.Success)
                                    {
                                        enq.AMST_MotherEmailId = stu.newlstget1[i].MotherEmailId.TrimEnd().TrimStart();
                                        counttt = counttt + 1;
                                    }
                                    else
                                    {
                                        stu.stuStatus = "Mother Email Id is not Valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "Mother Email Id can not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }


                            //student nationality
                            //long countryid12 = 0;
                            if (stu.newlstget1[i].StudentNationality != null && stu.newlstget1[i].StudentNationality != "")
                            {
                                if (stu.newlstget1[i].StudentNationality.TrimEnd().TrimStart() != null && stu.newlstget1[i].StudentNationality.TrimEnd().TrimStart() != "")
                                {
                                    if (((Regex.IsMatch(stu.newlstget1[i].StudentNationality.TrimEnd().TrimStart(), @"^([a-zA-Z0-9\s\-\,\:\,])*$")) && ((stu.newlstget1[i].StudentNationality.TrimEnd().TrimStart()).Length <= 50)))
                                    {
                                        var get_countryid = _AdmissionFormContext.Country.Where(t => t.IVRMMC_Nationality.Equals(stu.newlstget1[i].StudentNationality.TrimEnd().TrimStart()));
                                        if (get_countryid.Count() > 0)
                                        {
                                            enq.AMST_Nationality = get_countryid.FirstOrDefault().IVRMMC_Id;
                                            countryid1 = get_countryid.FirstOrDefault().IVRMMC_Id;
                                            counttt = counttt + 1;
                                        }
                                        else
                                        {
                                            stu.stuStatus = "student Nationality input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                            return stu;
                                        }
                                    }
                                    else
                                    {
                                        stu.stuStatus = "student Nationality input is not valid for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                        return stu;
                                    }
                                }
                                else
                                {
                                    stu.stuStatus = "student Nationality Can Not be Empty for" + " " + stu.newlstget1[i].AMSTAdmNo + " " + studentadmno;
                                    return stu;
                                }
                            }


                            var concession = _AdmissionFormContext.Fee_Master_ConcessionDMO.Where(a => a.MI_Id == stu.MI_Id && a.FMCC_ConcessionName.Equals("General") && a.FMCC_ActiveFlag == true).ToList();
                            if (concession.Count > 0)
                            {
                                enq.AMST_Concession_Type = concession.FirstOrDefault().FMCC_Id;
                                counttt = counttt + 1;
                            }
                            else
                            {
                                stu.stuStatus = "student Concession Type Is Not There Please Map The Fee Concession Type For Student And Concession Type Should Be 'General' Concession Type";
                                return stu;
                            }
                            enq.MI_Id = stu.MI_Id;
                            counttt = counttt + 1;
                            enq.AMST_SOL = "S";
                            counttt = counttt + 1;
                            enq.AMST_ActiveFlag = 1;
                            counttt = counttt + 1;


                            if (counttt == 45)
                            {
                                enq.CreatedDate = DateTime.Now;
                                enq.UpdatedDate = DateTime.Now;

                                _DomainModelMsSqlServerContext.Add(enq);
                                var flag = _DomainModelMsSqlServerContext.SaveChanges();
                                if (flag >= 1)
                                {
                                    try
                                    {
                                        Adm_Master_Father_Email enq_faheremail = new Adm_Master_Father_Email();
                                        enq_faheremail.AMST_FatheremailId = enq.AMST_FatheremailId;
                                        enq_faheremail.MI_Id = stu.MI_Id;
                                        enq_faheremail.AMST_Id = enq.AMST_Id;
                                        enq_faheremail.CreatedDate = DateTime.Now;
                                        enq_faheremail.UpdatedDate = DateTime.Now;
                                        _DomainModelMsSqlServerContext.Add(enq_faheremail);
                                        int n = _DomainModelMsSqlServerContext.SaveChanges();
                                    }
                                    catch (Exception ex)
                                    {
                                        _acdimpl.LogInformation("Father emailid saving in new table" + ex.Message);
                                    }

                                    try
                                    {
                                        Adm_M_Mother_Emailid enq_motheemail = new Adm_M_Mother_Emailid();
                                        enq_motheemail.AMST_MotheremailId = enq.AMST_MotherEmailId;
                                        enq_motheemail.AMST_Id = enq.AMST_Id;
                                        enq_motheemail.MI_Id = stu.MI_Id;
                                        enq_motheemail.CreatedDate = DateTime.Now;
                                        enq_motheemail.UpdatedDate = DateTime.Now;
                                        _DomainModelMsSqlServerContext.Add(enq_motheemail);
                                        int n1 = _DomainModelMsSqlServerContext.SaveChanges();
                                    }
                                    catch (Exception ex)
                                    {
                                        _acdimpl.LogInformation("Mother emailid saving in new table" + ex.Message);
                                    }

                                    try
                                    {
                                        Adm_M_Mother_MobileNo enq_mothermobile = new Adm_M_Mother_MobileNo();
                                        enq_mothermobile.AMST_MotherMobileNo = enq.AMST_MotherMobileNo;
                                        enq_mothermobile.AMST_Id = enq.AMST_Id;
                                        enq_mothermobile.MI_Id = stu.MI_Id;
                                        enq_mothermobile.CreatedDate = DateTime.Now;
                                        enq_mothermobile.UpdatedDate = DateTime.Now;
                                        _DomainModelMsSqlServerContext.Add(enq_mothermobile);
                                        int n2 = _DomainModelMsSqlServerContext.SaveChanges();
                                    }
                                    catch (Exception ex)
                                    {
                                        _acdimpl.LogInformation("Mother Mobileno saving in new table" + ex.Message);
                                    }

                                    try
                                    {
                                        Adm_M_Student_FatherMobileNo enq_fathermobile = new Adm_M_Student_FatherMobileNo();
                                        enq_fathermobile.AMST_Id = enq.AMST_Id;
                                        enq_fathermobile.MI_Id = stu.MI_Id;
                                        enq_fathermobile.AMST_FatherMobile_No = Convert.ToInt64(enq.AMST_FatherMobleNo);
                                        enq_fathermobile.CreatedDate = DateTime.Now;
                                        enq_fathermobile.UpdatedDate = DateTime.Now;
                                        _DomainModelMsSqlServerContext.Add(enq_fathermobile);
                                        int n3 = _DomainModelMsSqlServerContext.SaveChanges();
                                    }
                                    catch (Exception ex)
                                    {
                                        _acdimpl.LogInformation("Father Mobileno saving in new table" + ex.Message);
                                    }
                                    try
                                    {
                                        Adm_M_Student_MobileNo enq_studentmobile = new Adm_M_Student_MobileNo();
                                        enq_studentmobile.AMST_Id = enq.AMST_Id;
                                        enq_studentmobile.AMSTSMS_MobileNo = enq.AMST_MobileNo.ToString();
                                        enq_studentmobile.CreatedDate = DateTime.Now;
                                        enq_studentmobile.UpdatedDate = DateTime.Now;
                                        _DomainModelMsSqlServerContext.Add(enq_studentmobile);
                                        int n4 = _DomainModelMsSqlServerContext.SaveChanges();
                                    }
                                    catch (Exception ex)
                                    {
                                        _acdimpl.LogInformation("Student Mobileno saving in new table" + ex.Message);
                                    }
                                    try
                                    {
                                        Adm_M_Student_Email_Id enq_studentemail = new Adm_M_Student_Email_Id();
                                        enq_studentemail.AMST_Id = enq.AMST_Id;
                                        enq_studentemail.AMSTE_EmailId = enq.AMST_emailId;
                                        enq_studentemail.CreatedDate = DateTime.Now;
                                        enq_studentemail.UpdatedDate = DateTime.Now;
                                        _DomainModelMsSqlServerContext.Add(enq_studentemail);
                                        int n5 = _DomainModelMsSqlServerContext.SaveChanges();
                                    }
                                    catch (Exception ex)
                                    {
                                        _acdimpl.LogInformation("Student Email saving in new table" + ex.Message);

                                    }

                                    stu.stuStatus = "true";
                                    sucesscount = sucesscount + 1;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                        failcount = failcount + 1;
                        string name = failnames;
                        finalnames += name + ",";
                        stu.stuStatus = "Please Sheet Proper Excel Sheet";
                        return stu;

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _acdimpl.LogInformation("Admission Import Checking Validation" + ex.Message);
            }
            return stu;
        }
    }
}
