using AutoMapper;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Portals;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vaps.Fee;
using DomainModel.Model.com.vapstech.Fee;
using DomainModel.Model.com.vapstech.Transport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace PortalHub.com.vaps.Student.Services
{
    public class StudentRoueLocationUpdateImpl : Interfaces.StudentRoueLocationUpdateInterface
    {
        private static ConcurrentDictionary<string, StudentBuspassFormDTO> _login =
             new ConcurrentDictionary<string, StudentBuspassFormDTO>();
        //private readonly UserManager<ApplicationUser> _UserManager;
        private readonly DomainModelMsSqlServerContext _db;
        public PortalContext _buspasscontext;
        public FeeGroupContext _feecontext;
        public ProspectusContext _ProspectusContext;

        readonly ILogger<StudentRoueLocationUpdateImpl> Buspasss;
        //  public StudentApplicationContext _StudentApplicationContext;

        public StudentRoueLocationUpdateImpl(PortalContext buspassContext, DomainModelMsSqlServerContext db, FeeGroupContext feecontext, ProspectusContext ProspectusContext, ILogger<StudentRoueLocationUpdateImpl> _Buspasss)
        {
            //_StudentApplicationContext = StudentApplicationContext;
            _buspasscontext = buspassContext;
            _feecontext = feecontext;
            _ProspectusContext = ProspectusContext;
            _db = db;
            Buspasss = _Buspasss;
        }

        public StudentBuspassFormDTO getloaddata(StudentBuspassFormDTO data)
        {
            try
            {
                string rolename = _buspasscontext.IVRM_Role_Type.FirstOrDefault(t => t.IVRMRT_Id == data.roleId).IVRMRT_Role;

                var Acdemic_preadmission = _buspasscontext.AcademicYearDMO.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();


                data.transportyear = Acdemic_preadmission;

                //  data.prospectusPaymentlist = _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYPPA_Type == "T").ToArray();

                //     data.prospectusPaymentlist = (from a in _feecontext.FeeAmountEntryDMO
                //                                   from b in _feecontext.FeeTransactionPaymentDMO
                //                                   from d in _feecontext.FeeGroupDMO
                //                                   from f in _feecontext.FeeHeadDMO
                //                                   from g in _feecontext.feeYCCC
                //                                   from h in _feecontext.feeYCC
                //                                   from i in _feecontext.Fee_Y_Payment_School_StudentDMO
                //                                   from j in _feecontext.Adm_Student_Transport_ApplicationDMO
                //                                   where (a.FMG_Id == d.FMG_Id && a.FMH_Id == f.FMH_Id && a.FMCC_Id == h.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMA_Id == a.FMA_Id && h.ASMAY_Id == a.ASMAY_Id && j.AMST_Id==i.AMST_Id && i.FYP_Id == b.FYP_Id && f.FMH_Flag == "T" && d.FMG_CompulsoryFlag == "T" && a.ASMAY_Id==data.transportyear && j.AMST_Id==data.AMST_Id && j.ASTA_FutureAY == data.transportyear)
                //                                   select new StudentBuspassFormDTO
                //                                   {
                //                                       AMST_Id = i.AMST_Id,
                //                                       ASTA_Id=j.ASTA_Id

                //                                   }
                //).ToArray();


                data.prospectusPaymentlist = (from a in _feecontext.FeeYearlygroupHeadMappingDMO
                                              from b in _feecontext.FeeStudentTransactionDMO
                                              from d in _feecontext.FeeGroupDMO
                                              from f in _feecontext.FeeHeadDMO
                                              from j in _feecontext.Adm_Student_Transport_ApplicationDMO
                                              where (f.FMH_Flag == "T" && d.FMG_CompulsoryFlag == "T" && a.ASMAY_Id == data.transportyear && j.AMST_Id == data.AMST_Id && j.ASTA_FutureAY == data.transportyear && a.FMG_Id == d.FMG_Id && d.FMG_Id == b.FMG_Id && a.FMH_Id == f.FMH_Id && b.FMH_Id == f.FMH_Id && b.AMST_Id == j.AMST_Id && b.MI_Id == data.MI_Id)
                                              select new StudentBuspassFormDTO
                                              {
                                                  AMST_Id = j.AMST_Id,
                                                  ASTA_Id = j.ASTA_Id

                                              }
       ).ToArray();

                if (rolename == "Student")
                {
                    data.stu_name = (from a in _buspasscontext.Adm_M_Student
                                     where (a.AMST_Id == data.AMST_Id && a.MI_Id == data.MI_Id)
                                     select new StudentBuspassFormDTO
                                     {
                                         AMST_Id = a.AMST_Id,
                                         AMST_FirstName = a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName,
                                     }
              ).ToList().ToArray();

                    data.routeDetails = (from a in _buspasscontext.Adm_Student_Transport_ApplicationDMO
                                         from d in _buspasscontext.Adm_M_Student

                                         where (a.AMST_Id == d.AMST_Id && d.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && d.AMST_ActiveFlag == 1 && d.AMST_SOL == "S" && a.AMST_Id == data.AMST_Id)
                                         select new StudentBuspassFormDTO
                                         {
                                             AMST_Id = a.AMST_Id,
                                             ASTA_Id = a.ASTA_Id,
                                             AMST_FirstName = d.AMST_FirstName + ' ' + d.AMST_MiddleName + ' ' + d.AMST_LastName,
                                             ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
                                             TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _buspasscontext.MasterLocationDMO.Where(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).FirstOrDefault().TRML_LocationName : "--",

                                             ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
                                             TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _buspasscontext.MasterLocationDMO.Where(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).FirstOrDefault().TRML_LocationName : "--",

                                             TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
                                             TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _buspasscontext.MasterRouteDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",

                                             TRMR_Idd = a.ASTA_Drop_TRMR_Id,
                                             TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _buspasscontext.MasterRouteDMO.Where(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",
                                             ASTA_ApplStatus = a.ASTA_ApplStatus,
                                             ASMAY_Year = data.ASMAY_Id != 0 ? _buspasscontext.AcademicYearDMO.Where(td => td.MI_Id == data.MI_Id && td.ASMAY_Id == a.ASTA_FutureAY).FirstOrDefault().ASMAY_Year : "--",
                                         }
                        ).Distinct().ToArray();

                    var regularnewff = (from f in _feecontext.Adm_Student_Transport_ApplicationDMO
                                        where (f.AMST_Id == data.AMST_Id)
                                        select new StudentBuspassFormDTO
                                        {
                                            classnextid = f.ASTA_FutureAY,
                                            ASTA_ApplStatus = f.ASTA_ApplStatus
                                        }
            ).ToList();

                    //var newasmay = regularnewff.FirstOrDefault().classnextid;
                    if (regularnewff.Count != 0)
                    {
                        data.approvenot = regularnewff.FirstOrDefault().ASTA_ApplStatus;
                    }




                }

                else
                {
                    data.stu_name = (from a in _buspasscontext.Adm_M_Student
                                     where (a.MI_Id == data.MI_Id)
                                     select new StudentBuspassFormDTO
                                     {
                                         AMST_Id = a.AMST_Id,
                                         AMST_FirstName = a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName,
                                     }
           ).ToList().ToArray();

                    data.routeDetails = (from a in _buspasscontext.Adm_Student_Transport_ApplicationDMO
                                         from d in _buspasscontext.Adm_M_Student
                                         where (a.AMST_Id == d.AMST_Id && d.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && d.AMST_ActiveFlag == 1 && d.AMST_SOL == "S")
                                         select new StudentBuspassFormDTO
                                         {
                                             AMST_Id = a.AMST_Id,
                                             ASTA_Id = a.ASTA_Id,
                                             AMST_FirstName = d.AMST_FirstName + ' ' + d.AMST_MiddleName + ' ' + d.AMST_LastName,
                                             ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
                                             TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _buspasscontext.MasterLocationDMO.Where(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).FirstOrDefault().TRML_LocationName : "--",

                                             ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
                                             TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _buspasscontext.MasterLocationDMO.Where(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).FirstOrDefault().TRML_LocationName : "--",

                                             TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
                                             TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _buspasscontext.MasterRouteDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",

                                             TRMR_Idd = a.ASTA_Drop_TRMR_Id,
                                             TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _buspasscontext.MasterRouteDMO.Where(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",
                                             ASTA_ApplStatus = a.ASTA_ApplStatus,
                                             ASMAY_Year = data.ASMAY_Id != 0 ? _buspasscontext.AcademicYearDMO.Where(td => td.MI_Id == data.MI_Id && td.ASMAY_Id == a.ASTA_FutureAY).FirstOrDefault().ASMAY_Year : "--",


                                         }
                          ).Distinct().ToArray();



                }
                data.logoheader = (from a in _buspasscontext.FeeMasterConfigurationDMO
                                   where (a.MI_Id == data.MI_Id && a.userid == 364)
                                   select new StudentBuspassFormDTO
                                   {
                                       logopath = a.MI_Logo,
                                   }
        ).ToList().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public StudentBuspassFormDTO getloaddataintruction(StudentBuspassFormDTO data)
        {
            List<AcademicYear> curracayr = new List<AcademicYear>();
                curracayr = _buspasscontext.AcademicYearDMO.Where(r => r.MI_Id == data.MI_Id && r.ASMAY_ActiveFlag == 1).OrderByDescending(t=>t.ASMAY_Order).ToList();
                data.currfillyear = curracayr.ToArray();

            data.routeDetails = (from a in _buspasscontext.Adm_Student_Transport_ApplicationDMO
                                 from d in _buspasscontext.Adm_M_Student
                                 where (a.AMST_Id == d.AMST_Id && d.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && d.AMST_ActiveFlag == 1 && d.AMST_SOL == "S")
                                 select new StudentBuspassFormDTO
                                 {
                                     AMST_Id = a.AMST_Id,
                                     ASTA_Id = a.ASTA_Id,
                                     AMST_FirstName = d.AMST_FirstName + ' ' + d.AMST_MiddleName + ' ' + d.AMST_LastName,
                                     ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
                                     TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _buspasscontext.MasterLocationDMO.Where(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).FirstOrDefault().TRML_LocationName : "--",

                                     ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
                                     TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _buspasscontext.MasterLocationDMO.Where(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).FirstOrDefault().TRML_LocationName : "--",

                                     TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
                                     TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _buspasscontext.MasterRouteDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",

                                     TRMR_Idd = a.ASTA_Drop_TRMR_Id,
                                     TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _buspasscontext.MasterRouteDMO.Where(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",
                                     ASTA_ApplStatus = a.ASTA_ApplStatus,
                                     ASMAY_Year = data.ASMAY_Id != 0 ? _buspasscontext.AcademicYearDMO.Where(td => td.MI_Id == data.MI_Id && td.ASMAY_Id == a.ASTA_FutureAY).FirstOrDefault().ASMAY_Year : "--",


                                 }
                          ).Distinct().ToArray();

            data.logoheader = (from a in _buspasscontext.FeeMasterConfigurationDMO
                               where (a.MI_Id == data.MI_Id && a.userid == 364)
                               select new StudentBuspassFormDTO
                               {
                                   logopath = a.MI_Logo,
                               }
      ).ToList().ToArray();


            return data;
        }

        public StudentBuspassFormDTO getstudata(StudentBuspassFormDTO data)
        {
            try
            {
                var studentcurrentyear = (from a in _buspasscontext.School_Adm_Y_StudentDMO
                                          where (a.AMST_Id == data.AMST_Id)
                                          select a
              ).ToList().OrderByDescending(d => d.ASYST_Id).ToArray();

                if (studentcurrentyear.Length > 0)
                {
                    if (studentcurrentyear.Length == 1)
                    {
                        if (studentcurrentyear.FirstOrDefault().ASMAY_Id == data.ASMAY_Id)
                        {
                            data.studentaccyear = studentcurrentyear.FirstOrDefault().ASMAY_Id;
                            data.studentstatus = "AdmissionNew";
                        }
                        else
                        {
                            data.studentaccyear = studentcurrentyear.FirstOrDefault().ASMAY_Id;
                            data.studentstatus = "AdmissionRegular";
                        }
                    }
                    else if (studentcurrentyear.Length > 1)
                    {
                        if (studentcurrentyear.FirstOrDefault().ASMAY_Id != data.ASMAY_Id)
                        {
                            data.studentaccyear = studentcurrentyear.FirstOrDefault().ASMAY_Id;

                        }
                        else
                        {
                            data.studentaccyear = studentcurrentyear.FirstOrDefault().ASMAY_Id;

                        }
                        data.studentstatus = "AdmissionRegular";
                    }




                }

                else
                {
                    data.studentstatus = "AdmissionNew";
                    var studentadmityear = (from a in _buspasscontext.Adm_M_Student
                                            where (a.AMST_Id == data.AMST_Id)
                                            select a
               ).ToList().ToArray();

                    data.studentaccyear = studentadmityear.FirstOrDefault().ASMAY_Id;

                }
                data.regularnew = (from a in _feecontext.FeeGroupDMO
                                   from b in _feecontext.FeeYearlygroupHeadMappingDMO
                                   from c in _feecontext.FeeStudentTransactionDMO
                                   from d in _feecontext.FeeHeadDMO
                                   where (b.FMG_Id == a.FMG_Id && b.FMH_Id == d.FMH_Id && c.FMG_Id == b.FMG_Id && c.FMH_Id == b.FMH_Id && c.FMG_Id == a.FMG_Id && c.FMH_Id == d.FMH_Id && c.ASMAY_Id == b.ASMAY_Id && d.FMH_Flag == "T" && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.studentaccyear && c.FSS_CurrentYrCharges > 0 && c.AMST_Id == data.AMST_Id)
                                   select new StudentBuspassFormDTO
                                   {
                                       AMST_Id = c.AMST_Id

                                   }
).ToList().ToArray();


                if (data.regularnew.Length > 0)
                {
                    data.studentTrstatus = "TrRegular";
                }
                else
                {
                    data.studentTrstatus = "TrNew";
                }









                //if (data.transappfillAdmissionNew == true || data.transappfillTrNew == true || data.transappfillTrRegular == true)
                //{
                    //data.trnsportcutoffdate = "True";
                    data.countryid = _buspasscontext.country.ToArray();

                    List<MasterAreaDMO> saa = new List<MasterAreaDMO>();
                    saa = _buspasscontext.MasterAreaDMO.Where(r => r.MI_Id == data.MI_Id && r.TRMA_ActiveFlg==true).ToList();
                    data.areaList = saa.ToArray();
                    //route
                    List<MasterRouteDMO> rout = new List<MasterRouteDMO>();
                    rout = _buspasscontext.MasterRouteDMO.Where(r => r.MI_Id == data.MI_Id && r.TRMR_ActiveFlg==true).OrderBy(l=>l.TRMR_order).ToList();
                    data.routeList = rout.ToArray();
                    // location
                    List<MasterLocationDMO> locat = new List<MasterLocationDMO>();
                    locat = _buspasscontext.MasterLocationDMO.Where(r => r.MI_Id == data.MI_Id && r.TRML_ActiveFlg==true).ToList();
                    data.locaList = locat.ToArray();

                    List<Adm_Student_Transport_ApplicationDMO> trans = new List<Adm_Student_Transport_ApplicationDMO>();
                    trans = _buspasscontext.Adm_Student_Transport_ApplicationDMO.Where(r => r.MI_Id == data.MI_Id && r.AMST_Id == data.AMST_Id && r.ASTA_FutureAY == data.ASMAY_Id).ToList();
                    var trans_amstid = trans.ToArray();

                    if (trans.Count() > 0)
                    {
                        var studentcurrentyearr = (from a in _buspasscontext.School_Adm_Y_StudentDMO
                                                   where (a.AMST_Id == data.AMST_Id)
                                                   select a
                   ).ToList().OrderByDescending(d => d.ASYST_Id).ToArray();
                        if (studentcurrentyearr.Length > 0)
                        {
                            if (trans[0].ASTA_PickUp_TRMR_Id != 0 && trans[0].ASTA_Drop_TRMR_Id == 0)
                            {
                                data.studentDetails = (from d in _buspasscontext.AcademicYearDMO
                                                       from a in _buspasscontext.School_M_Class
                                                       from b in _buspasscontext.School_M_Section
                                                       from c in _buspasscontext.School_Adm_Y_StudentDMO
                                                       from f in _buspasscontext.Adm_M_Student
                                                       from g in _buspasscontext.country
                                                       from h in _buspasscontext.state
                                                       from i in _buspasscontext.MasterRouteDMO
                                                       from j in _buspasscontext.MasterLocationDMO
                                                       from k in _buspasscontext.Adm_Student_Transport_ApplicationDMO
                                                       where (
                                                       f.AMST_Id == c.AMST_Id && d.ASMAY_Id == c.ASMAY_Id && c.ASMCL_Id == a.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && g.IVRMMC_Id == f.AMST_ConCountry && h.IVRMMS_Id == f.AMST_ConState && h.IVRMMC_Id == g.IVRMMC_Id && k.AMST_Id == f.AMST_Id && (i.TRMR_Id == k.ASTA_PickUp_TRMR_Id) && (j.TRML_Id == k.ASTA_PickUp_TRML_Id) && k.AMST_Id == data.AMST_Id && k.ASTA_FutureAY == data.ASMAY_Id && f.MI_Id == data.MI_Id && c.ASMAY_Id==data.ASMAY_Id )
                                                       select new StudentBuspassFormDTO
                                                       {
                                                           AMST_FirstName = f.AMST_FirstName + ' ' + f.AMST_MiddleName + ' ' + f.AMST_LastName,
                                                           ASMCL_Id = c.ASMCL_Id,
                                                           ASMCL_ClassName = a.ASMCL_ClassName,
                                                           ASMS_Id = c.ASMS_Id,
                                                           AMST_BloodGroup = f.AMST_BloodGroup,
                                                           ASMC_SectionName = b.ASMC_SectionName,
                                                           ASMAY_Id = c.ASMAY_Id,
                                                           ASMAY_Year = d.ASMAY_Year,
                                                           AMST_AdmNo = f.AMST_AdmNo,
                                                           AMST_DOB = f.AMST_DOB,
                                                           AMST_emailId = f.AMST_emailId,
                                                           AMST_MobileNo = f.AMST_MobileNo,
                                                           AMST_PerStreet = f.AMST_ConStreet,
                                                           AMST_PerCity = f.AMST_ConCity,
                                                           AMST_PerArea = f.AMST_ConArea,
                                                           AMST_PerPincode = f.AMST_ConPincode,
                                                           IVRMMC_CountryName = g.IVRMMC_CountryName,
                                                           IVRMMS_Name = h.IVRMMS_Name,
                                                           AMST_FatherName = f.AMST_FatherName,
                                                           AMST_MotherName = f.AMST_MotherName,
                                                           AMST_FatherMobleNo = k.ASTA_FatherMobileNo,
                                                           AMST_MotherMobileNo = k.ASTA_MotherMobileNo,
                                                           AMST_FatherOfficeAdd = f.AMST_FatherOfficeAdd,
                                                           ASTA_AreaZoneName = k.ASTA_AreaZoneName,
                                                           TRMA_Id = k.TRMA_Id,
                                                           ASTA_PickUp_TRML_Id = k.ASTA_PickUp_TRML_Id,
                                                           ASTA_Drop_TRML_Id = k.ASTA_Drop_TRML_Id,
                                                           ASTA_PickUp_TRMR_Id = k.ASTA_PickUp_TRMR_Id,
                                                           ASTA_Drop_TRMR_Id = k.ASTA_Drop_TRMR_Id,
                                                           ASTA_Landmark = k.ASTA_Landmark,
                                                           ASTA_Phoneoff = k.ASTA_Phoneoff,
                                                           ASTA_PhoneRes = k.ASTA_PhoneRes,
                                                           AMST_Photoname = f.AMST_Photoname,
                                                           IVRMMS_Id = h.IVRMMS_Id,
                                                           ASTA_FutureAY = k.ASTA_FutureAY,
                                                           ASTA_Id = k.ASTA_Id

                                                       }
                        ).Distinct().ToArray();
                            }
                            else if (trans[0].ASTA_PickUp_TRMR_Id == 0 && trans[0].ASTA_Drop_TRMR_Id != 0)
                            {
                                data.studentDetails = (from d in _buspasscontext.AcademicYearDMO
                                                       from a in _buspasscontext.School_M_Class
                                                       from b in _buspasscontext.School_M_Section
                                                       from c in _buspasscontext.School_Adm_Y_StudentDMO
                                                       from f in _buspasscontext.Adm_M_Student
                                                       from g in _buspasscontext.country
                                                       from h in _buspasscontext.state
                                                       from i in _buspasscontext.MasterRouteDMO
                                                       from j in _buspasscontext.MasterLocationDMO
                                                       from k in _buspasscontext.Adm_Student_Transport_ApplicationDMO

                                                       where (f.AMST_Id == c.AMST_Id && d.ASMAY_Id == c.ASMAY_Id && c.ASMCL_Id == a.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && g.IVRMMC_Id == f.AMST_ConCountry && h.IVRMMS_Id == f.AMST_ConState && h.IVRMMC_Id == g.IVRMMC_Id && k.AMST_Id == f.AMST_Id && (i.TRMR_Id == k.ASTA_Drop_TRMR_Id) && (j.TRML_Id == k.ASTA_Drop_TRML_Id) && k.AMST_Id == data.AMST_Id && k.ASTA_FutureAY == data.ASMAY_Id && f.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id)
                                                       select new StudentBuspassFormDTO
                                                       {
                                                           AMST_FirstName = f.AMST_FirstName + ' ' + f.AMST_MiddleName + ' ' + f.AMST_LastName,
                                                           ASMCL_Id = c.ASMCL_Id,
                                                           ASMCL_ClassName = a.ASMCL_ClassName,
                                                           ASMS_Id = c.ASMS_Id,
                                                           AMST_BloodGroup = f.AMST_BloodGroup,
                                                           ASMC_SectionName = b.ASMC_SectionName,
                                                           ASMAY_Id = c.ASMAY_Id,
                                                           ASMAY_Year = d.ASMAY_Year,
                                                           AMST_AdmNo = f.AMST_AdmNo,
                                                           AMST_DOB = f.AMST_DOB,
                                                           AMST_emailId = f.AMST_emailId,
                                                           AMST_MobileNo = f.AMST_MobileNo,
                                                           AMST_PerStreet = f.AMST_ConStreet,
                                                           AMST_PerCity = f.AMST_ConCity,
                                                           AMST_PerArea = f.AMST_ConArea,
                                                           AMST_PerPincode = f.AMST_ConPincode,
                                                           IVRMMC_CountryName = g.IVRMMC_CountryName,
                                                           IVRMMS_Name = h.IVRMMS_Name,
                                                           AMST_FatherName = f.AMST_FatherName,
                                                           AMST_MotherName = f.AMST_MotherName,
                                                           AMST_FatherMobleNo = k.ASTA_FatherMobileNo,
                                                           AMST_MotherMobileNo = k.ASTA_MotherMobileNo,
                                                           AMST_FatherOfficeAdd = f.AMST_FatherOfficeAdd,
                                                           ASTA_AreaZoneName = k.ASTA_AreaZoneName,
                                                           TRMA_Id = k.TRMA_Id,
                                                           ASTA_PickUp_TRML_Id = k.ASTA_PickUp_TRML_Id,
                                                           ASTA_Drop_TRML_Id = k.ASTA_Drop_TRML_Id,
                                                           ASTA_PickUp_TRMR_Id = k.ASTA_PickUp_TRMR_Id,
                                                           ASTA_Drop_TRMR_Id = k.ASTA_Drop_TRMR_Id,
                                                           ASTA_Landmark = k.ASTA_Landmark,
                                                           ASTA_Phoneoff = k.ASTA_Phoneoff,
                                                           ASTA_PhoneRes = k.ASTA_PhoneRes,
                                                           AMST_Photoname = f.AMST_Photoname,
                                                           IVRMMS_Id = h.IVRMMS_Id,
                                                           ASTA_FutureAY = k.ASTA_FutureAY,
                                                           ASTA_Id = k.ASTA_Id

                                                       }
                        ).Distinct().ToArray();
                            }
                            else if (trans[0].ASTA_PickUp_TRMR_Id != 0 && trans[0].ASTA_Drop_TRMR_Id != 0)
                            {
                                data.studentDetails = (from d in _buspasscontext.AcademicYearDMO
                                                       from a in _buspasscontext.School_M_Class
                                                       from b in _buspasscontext.School_M_Section
                                                       from c in _buspasscontext.School_Adm_Y_StudentDMO
                                                       from f in _buspasscontext.Adm_M_Student
                                                       from g in _buspasscontext.country
                                                       from h in _buspasscontext.state
                                                       from i in _buspasscontext.MasterRouteDMO
                                                       from j in _buspasscontext.MasterLocationDMO
                                                       from k in _buspasscontext.Adm_Student_Transport_ApplicationDMO
                                                       where (

                                                       f.AMST_Id == c.AMST_Id && d.ASMAY_Id == c.ASMAY_Id && c.ASMCL_Id == a.ASMCL_Id && c.ASMS_Id == b.ASMS_Id && g.IVRMMC_Id == h.IVRMMC_Id && g.IVRMMC_Id == f.AMST_ConCountry && h.IVRMMS_Id == f.AMST_ConState && c.AMST_Id == k.AMST_Id &&
                                                       (i.TRMR_Id == k.ASTA_PickUp_TRMR_Id || i.TRMR_Id == k.ASTA_Drop_TRMR_Id) && (j.TRML_Id == k.ASTA_Drop_TRML_Id || j.TRML_Id == k.ASTA_PickUp_TRML_Id) && k.AMST_Id == data.AMST_Id && k.ASTA_FutureAY == data.ASMAY_Id && f.AMST_Id == data.AMST_Id && c.ASMAY_Id == data.ASMAY_Id



                                                    /*   f.AMST_Id == c.AMST_Id && d.ASMAY_Id == f.ASMAY_Id && c.ASMCL_Id == a.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && g.IVRMMC_Id == f.AMST_ConCountry && h.IVRMMS_Id == f.AMST_ConState && h.IVRMMC_Id == g.IVRMMC_Id && k.AMST_Id == f.AMST_Id && (i.TRMR_Id == k.ASTA_PickUp_TRMR_Id || i.TRMR_Id == k.ASTA_Drop_TRMR_Id) && (j.TRML_Id == k.ASTA_Drop_TRML_Id || j.TRML_Id == k.ASTA_PickUp_TRML_Id) && k.AMST_Id == data.AMST_Id && c.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id*/
                                                    )
                                                       select new StudentBuspassFormDTO
                                                       {
                                                           AMST_FirstName = f.AMST_FirstName + ' ' + f.AMST_MiddleName + ' ' + f.AMST_LastName,
                                                           ASMCL_Id = c.ASMCL_Id,
                                                           ASMCL_ClassName = a.ASMCL_ClassName,
                                                           ASMS_Id = c.ASMS_Id,
                                                           AMST_BloodGroup = f.AMST_BloodGroup,
                                                           ASMC_SectionName = b.ASMC_SectionName,
                                                           ASMAY_Id = c.ASMAY_Id,
                                                           ASMAY_Year = d.ASMAY_Year,
                                                           AMST_AdmNo = f.AMST_AdmNo,
                                                           AMST_DOB = f.AMST_DOB,
                                                           AMST_emailId = f.AMST_emailId,
                                                           AMST_MobileNo = f.AMST_MobileNo,
                                                           AMST_PerStreet = f.AMST_ConStreet,
                                                           AMST_PerCity = f.AMST_ConCity,
                                                           AMST_PerArea = f.AMST_ConArea,
                                                           AMST_PerPincode = f.AMST_ConPincode,
                                                           IVRMMC_CountryName = g.IVRMMC_CountryName,
                                                           IVRMMS_Name = h.IVRMMS_Name,
                                                           AMST_FatherName = f.AMST_FatherName,
                                                           AMST_MotherName = f.AMST_MotherName,
                                                           AMST_FatherMobleNo = k.ASTA_FatherMobileNo,
                                                           AMST_MotherMobileNo = k.ASTA_MotherMobileNo,
                                                           AMST_FatherOfficeAdd = f.AMST_FatherOfficeAdd,
                                                           ASTA_AreaZoneName = k.ASTA_AreaZoneName,
                                                           TRMA_Id = k.TRMA_Id,
                                                           ASTA_PickUp_TRML_Id = k.ASTA_PickUp_TRML_Id,
                                                           ASTA_Drop_TRML_Id = k.ASTA_Drop_TRML_Id,
                                                           ASTA_PickUp_TRMR_Id = k.ASTA_PickUp_TRMR_Id,
                                                           ASTA_Drop_TRMR_Id = k.ASTA_Drop_TRMR_Id,
                                                           ASTA_Landmark = k.ASTA_Landmark,
                                                           ASTA_Phoneoff = k.ASTA_Phoneoff,
                                                           ASTA_PhoneRes = k.ASTA_PhoneRes,
                                                           AMST_Photoname = f.AMST_Photoname,
                                                           IVRMMS_Id = h.IVRMMS_Id,
                                                           ASTA_FutureAY = k.ASTA_FutureAY,
                                                           ASTA_Id = k.ASTA_Id

                                                       }
                         ).Distinct().ToArray();
                            }

                        }
                        else
                        {
                            if (trans[0].ASTA_PickUp_TRMR_Id != 0 && trans[0].ASTA_Drop_TRMR_Id == 0)
                            {
                                data.studentDetails = (from d in _buspasscontext.AcademicYearDMO
                                                       from a in _buspasscontext.School_M_Class
                                                       from f in _buspasscontext.Adm_M_Student
                                                       from g in _buspasscontext.country
                                                       from h in _buspasscontext.state
                                                       from i in _buspasscontext.MasterRouteDMO
                                                       from j in _buspasscontext.MasterLocationDMO
                                                       from k in _buspasscontext.Adm_Student_Transport_ApplicationDMO
                                                       where (
                                                        f.ASMAY_Id == d.ASMAY_Id && f.ASMCL_Id == a.ASMCL_Id && g.IVRMMC_Id == f.AMST_ConCountry && h.IVRMMS_Id == f.AMST_ConState && h.IVRMMC_Id == g.IVRMMC_Id && k.AMST_Id == f.AMST_Id && (i.TRMR_Id == k.ASTA_PickUp_TRMR_Id) && (j.TRML_Id == k.ASTA_PickUp_TRML_Id) && k.AMST_Id == data.AMST_Id && k.ASTA_FutureAY == data.ASMAY_Id && f.MI_Id == data.MI_Id)
                                                       select new StudentBuspassFormDTO
                                                       {
                                                           AMST_FirstName = f.AMST_FirstName + ' ' + f.AMST_MiddleName + ' ' + f.AMST_LastName,
                                                           ASMCL_Id = f.ASMCL_Id,
                                                           ASMCL_ClassName = a.ASMCL_ClassName,

                                                           AMST_BloodGroup = f.AMST_BloodGroup,

                                                           ASMAY_Id = f.ASMAY_Id,
                                                           ASMAY_Year = d.ASMAY_Year,
                                                           AMST_AdmNo = f.AMST_AdmNo,
                                                           AMST_DOB = f.AMST_DOB,
                                                           AMST_emailId = f.AMST_emailId,
                                                           AMST_MobileNo = f.AMST_MobileNo,
                                                           AMST_PerStreet = f.AMST_ConStreet,
                                                           AMST_PerCity = f.AMST_ConCity,
                                                           AMST_PerArea = f.AMST_ConArea,
                                                           AMST_PerPincode = f.AMST_ConPincode,
                                                           IVRMMC_CountryName = g.IVRMMC_CountryName,
                                                           IVRMMS_Name = h.IVRMMS_Name,
                                                           AMST_FatherName = f.AMST_FatherName,
                                                           AMST_MotherName = f.AMST_MotherName,
                                                           AMST_FatherMobleNo = k.ASTA_FatherMobileNo,
                                                           AMST_MotherMobileNo = k.ASTA_MotherMobileNo,
                                                           AMST_FatherOfficeAdd = f.AMST_FatherOfficeAdd,
                                                           ASTA_AreaZoneName = k.ASTA_AreaZoneName,
                                                           TRMA_Id = k.TRMA_Id,
                                                           ASTA_PickUp_TRML_Id = k.ASTA_PickUp_TRML_Id,
                                                           ASTA_Drop_TRML_Id = k.ASTA_Drop_TRML_Id,
                                                           ASTA_PickUp_TRMR_Id = k.ASTA_PickUp_TRMR_Id,
                                                           ASTA_Drop_TRMR_Id = k.ASTA_Drop_TRMR_Id,
                                                           ASTA_Landmark = k.ASTA_Landmark,
                                                           ASTA_Phoneoff = k.ASTA_Phoneoff,
                                                           ASTA_PhoneRes = k.ASTA_PhoneRes,
                                                           AMST_Photoname = f.AMST_Photoname,
                                                           IVRMMS_Id = h.IVRMMS_Id,
                                                           ASTA_FutureAY = k.ASTA_FutureAY,
                                                           ASTA_Id = k.ASTA_Id

                                                       }
                        ).Distinct().ToArray();
                            }
                            else if (trans[0].ASTA_PickUp_TRMR_Id == 0 && trans[0].ASTA_Drop_TRMR_Id != 0)
                            {
                                data.studentDetails = (from d in _buspasscontext.AcademicYearDMO
                                                       from a in _buspasscontext.School_M_Class
                                                       from f in _buspasscontext.Adm_M_Student
                                                       from g in _buspasscontext.country
                                                       from h in _buspasscontext.state
                                                       from i in _buspasscontext.MasterRouteDMO
                                                       from j in _buspasscontext.MasterLocationDMO
                                                       from k in _buspasscontext.Adm_Student_Transport_ApplicationDMO

                                                       where (f.ASMAY_Id == d.ASMAY_Id && f.ASMCL_Id == a.ASMCL_Id && g.IVRMMC_Id == f.AMST_ConCountry && h.IVRMMS_Id == f.AMST_ConState && h.IVRMMC_Id == g.IVRMMC_Id && k.AMST_Id == f.AMST_Id && (i.TRMR_Id == k.ASTA_Drop_TRMR_Id) && (j.TRML_Id == k.ASTA_Drop_TRML_Id) && k.AMST_Id == data.AMST_Id && k.ASTA_FutureAY == data.ASMAY_Id && f.MI_Id == data.MI_Id)
                                                       select new StudentBuspassFormDTO
                                                       {
                                                           AMST_FirstName = f.AMST_FirstName + ' ' + f.AMST_MiddleName + ' ' + f.AMST_LastName,
                                                           ASMCL_Id = f.ASMCL_Id,
                                                           ASMCL_ClassName = a.ASMCL_ClassName,

                                                           AMST_BloodGroup = f.AMST_BloodGroup,

                                                           ASMAY_Id = f.ASMAY_Id,
                                                           ASMAY_Year = d.ASMAY_Year,
                                                           AMST_AdmNo = f.AMST_AdmNo,
                                                           AMST_DOB = f.AMST_DOB,
                                                           AMST_emailId = f.AMST_emailId,
                                                           AMST_MobileNo = f.AMST_MobileNo,
                                                           AMST_PerStreet = f.AMST_ConStreet,
                                                           AMST_PerCity = f.AMST_ConCity,
                                                           AMST_PerArea = f.AMST_ConArea,
                                                           AMST_PerPincode = f.AMST_ConPincode,
                                                           IVRMMC_CountryName = g.IVRMMC_CountryName,
                                                           IVRMMS_Name = h.IVRMMS_Name,
                                                           AMST_FatherName = f.AMST_FatherName,
                                                           AMST_MotherName = f.AMST_MotherName,
                                                           AMST_FatherMobleNo = k.ASTA_FatherMobileNo,
                                                           AMST_MotherMobileNo = k.ASTA_MotherMobileNo,
                                                           AMST_FatherOfficeAdd = f.AMST_FatherOfficeAdd,
                                                           ASTA_AreaZoneName = k.ASTA_AreaZoneName,
                                                           TRMA_Id = k.TRMA_Id,
                                                           ASTA_PickUp_TRML_Id = k.ASTA_PickUp_TRML_Id,
                                                           ASTA_Drop_TRML_Id = k.ASTA_Drop_TRML_Id,
                                                           ASTA_PickUp_TRMR_Id = k.ASTA_PickUp_TRMR_Id,
                                                           ASTA_Drop_TRMR_Id = k.ASTA_Drop_TRMR_Id,
                                                           ASTA_Landmark = k.ASTA_Landmark,
                                                           ASTA_Phoneoff = k.ASTA_Phoneoff,
                                                           ASTA_PhoneRes = k.ASTA_PhoneRes,
                                                           AMST_Photoname = f.AMST_Photoname,
                                                           IVRMMS_Id = h.IVRMMS_Id,
                                                           ASTA_FutureAY = k.ASTA_FutureAY,
                                                           ASTA_Id = k.ASTA_Id

                                                       }
                        ).Distinct().ToArray();
                            }
                            else if (trans[0].ASTA_PickUp_TRMR_Id != 0 && trans[0].ASTA_Drop_TRMR_Id != 0)
                            {
                                data.studentDetails = (from d in _buspasscontext.AcademicYearDMO
                                                       from a in _buspasscontext.School_M_Class

                                                       from f in _buspasscontext.Adm_M_Student
                                                       from g in _buspasscontext.country
                                                       from h in _buspasscontext.state
                                                       from i in _buspasscontext.MasterRouteDMO
                                                       from j in _buspasscontext.MasterLocationDMO
                                                       from k in _buspasscontext.Adm_Student_Transport_ApplicationDMO
                                                       where (f.ASMAY_Id == f.ASMAY_Id && f.ASMCL_Id == a.ASMCL_Id && g.IVRMMC_Id == f.AMST_ConCountry && h.IVRMMS_Id == f.AMST_ConState && h.IVRMMC_Id == g.IVRMMC_Id && k.AMST_Id == f.AMST_Id && (i.TRMR_Id == k.ASTA_PickUp_TRMR_Id || i.TRMR_Id == k.ASTA_Drop_TRMR_Id) && (j.TRML_Id == k.ASTA_Drop_TRML_Id || j.TRML_Id == k.ASTA_PickUp_TRML_Id) && k.AMST_Id == data.AMST_Id && k.ASTA_FutureAY == data.ASMAY_Id && f.MI_Id == data.MI_Id)
                                                       select new StudentBuspassFormDTO
                                                       {
                                                           AMST_FirstName = f.AMST_FirstName + ' ' + f.AMST_MiddleName + ' ' + f.AMST_LastName,
                                                           ASMCL_Id = f.ASMCL_Id,
                                                           ASMCL_ClassName = a.ASMCL_ClassName,

                                                           AMST_BloodGroup = f.AMST_BloodGroup,

                                                           ASMAY_Id = f.ASMAY_Id,
                                                           ASMAY_Year = d.ASMAY_Year,
                                                           AMST_AdmNo = f.AMST_AdmNo,
                                                           AMST_DOB = f.AMST_DOB,
                                                           AMST_emailId = f.AMST_emailId,
                                                           AMST_MobileNo = f.AMST_MobileNo,
                                                           AMST_PerStreet = f.AMST_ConStreet,
                                                           AMST_PerCity = f.AMST_ConCity,
                                                           AMST_PerArea = f.AMST_ConArea,
                                                           AMST_PerPincode = f.AMST_ConPincode,
                                                           IVRMMC_CountryName = g.IVRMMC_CountryName,
                                                           IVRMMS_Name = h.IVRMMS_Name,
                                                           AMST_FatherName = f.AMST_FatherName,
                                                           AMST_MotherName = f.AMST_MotherName,
                                                           AMST_FatherMobleNo = k.ASTA_FatherMobileNo,
                                                           AMST_MotherMobileNo = k.ASTA_MotherMobileNo,
                                                           AMST_FatherOfficeAdd = f.AMST_FatherOfficeAdd,
                                                           ASTA_AreaZoneName = k.ASTA_AreaZoneName,
                                                           TRMA_Id = k.TRMA_Id,
                                                           ASTA_PickUp_TRML_Id = k.ASTA_PickUp_TRML_Id,
                                                           ASTA_Drop_TRML_Id = k.ASTA_Drop_TRML_Id,
                                                           ASTA_PickUp_TRMR_Id = k.ASTA_PickUp_TRMR_Id,
                                                           ASTA_Drop_TRMR_Id = k.ASTA_Drop_TRMR_Id,
                                                           ASTA_Landmark = k.ASTA_Landmark,
                                                           ASTA_Phoneoff = k.ASTA_Phoneoff,
                                                           ASTA_PhoneRes = k.ASTA_PhoneRes,
                                                           AMST_Photoname = f.AMST_Photoname,
                                                           IVRMMS_Id = h.IVRMMS_Id,
                                                           ASTA_FutureAY = k.ASTA_FutureAY,
                                                           ASTA_Id = k.ASTA_Id

                                                       }
                         ).Distinct().ToArray();
                            }

                        }




                    }
                    else
                    {

                        var studentcurrentyearr = (from a in _buspasscontext.School_Adm_Y_StudentDMO
                                                   where (a.AMST_Id == data.AMST_Id)
                                                   select a
                    ).ToList().OrderByDescending(d => d.ASYST_Id).ToArray();
                        if (studentcurrentyearr.Length > 0)
                        {

                            data.studentDetails = (from d in _buspasscontext.AcademicYearDMO
                                                   from a in _buspasscontext.School_M_Class
                                                   from b in _buspasscontext.School_M_Section
                                                   from c in _buspasscontext.School_Adm_Y_StudentDMO
                                                   from f in _buspasscontext.Adm_M_Student
                                                   from g in _buspasscontext.country
                                                   from h in _buspasscontext.state

                                                   where (f.AMST_Id == c.AMST_Id && c.ASMCL_Id == a.ASMCL_Id && d.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == c.ASMCL_Id && g.IVRMMC_Id == h.IVRMMC_Id && g.IVRMMC_Id == f.AMST_ConCountry && h.IVRMMS_Id == f.AMST_ConState && f.MI_Id == data.MI_Id && c.AMST_Id == data.AMST_Id && c.ASMAY_Id == data.studentaccyear && c.ASMAY_Id == data.studentaccyear
                                               //    f.AMST_Id == c.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id
                                               //    && g.IVRMMC_Id == f.AMST_PerCountry && h.IVRMMS_Id == f.AMST_PerState && g.IVRMMC_Id == h.IVRMMC_Id &&

                                               // a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id &&
                                               //c.AMST_Id == data.AMST_Id
                                               )
                                                   select new StudentBuspassFormDTO
                                                   {
                                                       AMST_FirstName = f.AMST_FirstName + ' ' + f.AMST_MiddleName + ' ' + f.AMST_LastName,
                                                       ASMCL_Id = c.ASMCL_Id,
                                                       ASMCL_ClassName = a.ASMCL_ClassName,
                                                       ASMS_Id = c.ASMS_Id,
                                                       ASMC_SectionName = b.ASMC_SectionName,
                                                       ASMAY_Id = c.ASMAY_Id,
                                                       ASMAY_Year = d.ASMAY_Year,
                                                       AMST_AdmNo = f.AMST_AdmNo,
                                                       AMST_DOB = f.AMST_DOB,
                                                       AMST_emailId = f.AMST_emailId,
                                                       AMST_MobileNo = f.AMST_MobileNo,
                                                       AMST_PerStreet = f.AMST_ConStreet,
                                                       AMST_PerCity = f.AMST_ConCity,
                                                       AMST_PerArea = f.AMST_ConArea,
                                                       AMST_PerPincode = f.AMST_ConPincode,
                                                       IVRMMC_CountryName = g.IVRMMC_CountryName,
                                                       IVRMMS_Name = h.IVRMMS_Name,
                                                       AMST_FatherName = f.AMST_FatherName,
                                                       AMST_MotherName = f.AMST_MotherName,
                                                       AMST_Photoname = f.AMST_Photoname,
                                                       AMST_FatherOfficeAdd = f.AMST_FatherOfficeAdd,
                                                       IVRMMS_Id = h.IVRMMS_Id
                                                   }
                    ).Distinct().ToArray();
                        }
                        else
                        {
                            data.studentDetails = (from d in _buspasscontext.AcademicYearDMO
                                                   from a in _buspasscontext.School_M_Class
                                                   from f in _buspasscontext.Adm_M_Student
                                                   from g in _buspasscontext.country
                                                   from h in _buspasscontext.state
                                                   where (
                                                   f.ASMAY_Id == d.ASMAY_Id && f.ASMCL_Id == a.ASMCL_Id && f.AMST_ConCountry == g.IVRMMC_Id && f.AMST_ConState == h.IVRMMS_Id && g.IVRMMC_Id == h.IVRMMC_Id && f.AMST_Id == data.AMST_Id && f.MI_Id == data.MI_Id


                                               //    a.ASMCL_Id == f.ASMCL_Id &&  d.ASMAY_Id == f.ASMAY_Id
                                               //    && g.IVRMMC_Id == f.AMST_PerCountry && h.IVRMMS_Id == f.AMST_PerState && g.IVRMMC_Id == h.IVRMMC_Id &&

                                               // a.MI_Id == data.MI_Id && f.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id &&
                                               //f.AMST_Id == data.AMST_Id
                                               )
                                                   select new StudentBuspassFormDTO
                                                   {
                                                       AMST_FirstName = f.AMST_FirstName + ' ' + f.AMST_MiddleName + ' ' + f.AMST_LastName,
                                                       ASMCL_Id = f.ASMCL_Id,
                                                       ASMCL_ClassName = a.ASMCL_ClassName,
                                                       ASMAY_Id = f.ASMAY_Id,
                                                       ASMAY_Year = d.ASMAY_Year,
                                                       AMST_AdmNo = f.AMST_AdmNo,
                                                       AMST_DOB = f.AMST_DOB,
                                                       AMST_emailId = f.AMST_emailId,
                                                       AMST_MobileNo = f.AMST_MobileNo,
                                                       AMST_PerStreet = f.AMST_ConStreet,
                                                       AMST_PerCity = f.AMST_ConCity,
                                                       AMST_PerArea = f.AMST_ConArea,
                                                       AMST_PerPincode = f.AMST_ConPincode,
                                                       IVRMMC_CountryName = g.IVRMMC_CountryName,
                                                       IVRMMS_Name = h.IVRMMS_Name,
                                                       AMST_FatherName = f.AMST_FatherName,
                                                       AMST_MotherName = f.AMST_MotherName,
                                                       AMST_Photoname = f.AMST_Photoname,
                                                       AMST_FatherOfficeAdd = f.AMST_FatherOfficeAdd,
                                                       IVRMMS_Id = h.IVRMMS_Id
                                                   }
                    ).Distinct().ToArray();

                        }

                    }


                    data.routeDetails = (from a in _buspasscontext.Adm_Student_Transport_ApplicationDMO
                                         from d in _buspasscontext.Adm_M_Student
                                         where (a.AMST_Id == d.AMST_Id && d.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && d.AMST_ActiveFlag == 1 && d.AMST_SOL == "S" && a.AMST_Id == data.AMST_Id)
                                         select new StudentBuspassFormDTO
                                         {
                                             AMST_Id = a.AMST_Id,
                                             ASTA_Id = a.ASTA_Id,
                                             AMST_FirstName = d.AMST_FirstName + ' ' + d.AMST_MiddleName + ' ' + d.AMST_LastName,
                                             ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
                                             TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _buspasscontext.MasterLocationDMO.Single(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).TRML_LocationName : "--",

                                             ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
                                             TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _buspasscontext.MasterLocationDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).TRML_LocationName : "--",

                                             TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
                                             TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _buspasscontext.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).TRMR_RouteName : "--",

                                             TRMR_Idd = a.ASTA_Drop_TRMR_Id,
                                             TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _buspasscontext.MasterRouteDMO.Single(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).TRMR_RouteName : "--"
                                         }
                           ).Distinct().ToArray();

                //}
                //else
                //{
                //    data.trnsportcutoffdate = "False";
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public StudentBuspassFormDTO getstudata1(StudentBuspassFormDTO data)
        {
            try
            {
                data.countryid = _buspasscontext.country.ToArray();

                List<MasterAreaDMO> saa = new List<MasterAreaDMO>();
                saa = _buspasscontext.MasterAreaDMO.Where(r => r.MI_Id == data.MI_Id).ToList();
                data.areaList = saa.ToArray();
                //route
                List<MasterRouteDMO> rout = new List<MasterRouteDMO>();
                rout = _buspasscontext.MasterRouteDMO.Where(r => r.MI_Id == data.MI_Id).ToList();
                data.routeList = rout.ToArray();
                // location
                List<MasterLocationDMO> locat = new List<MasterLocationDMO>();
                locat = _buspasscontext.MasterLocationDMO.Where(r => r.MI_Id == data.MI_Id).ToList();
                data.locaList = locat.ToArray();

                string rolename = _buspasscontext.IVRM_Role_Type.FirstOrDefault(t => t.IVRMRT_Id == data.roleId).IVRMRT_Role;

                List<Adm_Student_Transport_ApplicationDMO> trans = new List<Adm_Student_Transport_ApplicationDMO>();
                trans = _buspasscontext.Adm_Student_Transport_ApplicationDMO.Where(r => r.MI_Id == data.MI_Id && r.AMST_Id == data.AMST_Id).ToList();
                var trans_amstid = trans.ToArray();

                if (rolename == "Student")
                {
                    if (trans_amstid.Length > 0)
                    {
                        data.studentDetails = (from d in _buspasscontext.AcademicYearDMO
                                               from a in _buspasscontext.School_M_Class
                                               from b in _buspasscontext.School_M_Section
                                               from c in _buspasscontext.School_Adm_Y_StudentDMO
                                               from f in _buspasscontext.Adm_M_Student
                                               from g in _buspasscontext.country
                                               from h in _buspasscontext.state
                                               from i in _buspasscontext.MasterRouteDMO
                                               from j in _buspasscontext.MasterLocationDMO
                                               from k in _buspasscontext.Adm_Student_Transport_ApplicationDMO

                                               where (d.ASMAY_Id == f.ASMAY_Id && a.ASMCL_Id == f.ASMCL_Id && c.AMST_Id == f.AMST_Id && c.ASMS_Id == b.ASMS_Id
                                               && g.IVRMMC_Id == f.AMST_ConCountry && h.IVRMMS_Id == f.AMST_ConState && g.IVRMMC_Id == h.IVRMMC_Id &&
                                           i.TRMR_Id == k.ASTA_Drop_TRMR_Id && i.TRMR_Id == k.ASTA_PickUp_TRMR_Id && j.TRML_Id == k.ASTA_PickUp_TRML_Id && j.TRML_Id == k.ASTA_Drop_TRML_Id && f.MI_Id == data.MI_Id && f.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id &&
                                           f.AMST_Id == data.AMST_Id)
                                               select new StudentBuspassFormDTO
                                               {
                                                   AMST_FirstName = f.AMST_FirstName + ' ' + f.AMST_MiddleName + ' ' + f.AMST_LastName,
                                                   ASMCL_Id = c.ASMCL_Id,
                                                   ASMCL_ClassName = a.ASMCL_ClassName,
                                                   ASMS_Id = c.ASMS_Id,
                                                   AMST_BloodGroup = f.AMST_BloodGroup,
                                                   ASMC_SectionName = b.ASMC_SectionName,
                                                   ASMAY_Id = c.ASMAY_Id,
                                                   ASMAY_Year = d.ASMAY_Year,
                                                   AMST_AdmNo = f.AMST_AdmNo,
                                                   AMST_DOB = f.AMST_DOB,
                                                   AMST_emailId = f.AMST_emailId,
                                                   AMST_MobileNo = f.AMST_MobileNo,
                                                   AMST_PerStreet = f.AMST_ConStreet,
                                                   AMST_PerCity = f.AMST_ConCity,
                                                   AMST_PerArea = f.AMST_ConArea,
                                                   AMST_PerPincode = f.AMST_ConPincode,
                                                   IVRMMC_CountryName = g.IVRMMC_CountryName,
                                                   IVRMMS_Name = h.IVRMMS_Name,
                                                   AMST_FatherName = f.AMST_FatherName,
                                                   AMST_MotherName = f.AMST_MotherName,
                                                   AMST_FatherMobleNo = k.ASTA_FatherMobileNo,
                                                   AMST_MotherMobileNo = k.ASTA_MotherMobileNo,
                                                   AMST_FatherOfficeAdd = f.AMST_FatherOfficeAdd,
                                                   ASTA_AreaZoneName = k.ASTA_AreaZoneName,
                                                   ASTA_PickUp_TRML_Id = k.ASTA_PickUp_TRML_Id,
                                                   ASTA_Drop_TRML_Id = k.ASTA_Drop_TRML_Id,
                                                   ASTA_PickUp_TRMR_Id = k.ASTA_PickUp_TRMR_Id,
                                                   ASTA_Drop_TRMR_Id = k.ASTA_Drop_TRMR_Id,
                                                   ASTA_Landmark = k.ASTA_Landmark,
                                                   ASTA_Phoneoff = k.ASTA_Phoneoff,
                                                   ASTA_PhoneRes = k.ASTA_PhoneRes
                                               }
                ).Distinct().ToArray();

                        var mobilenos = (from a in _buspasscontext.Adm_Student_Transport_ApplicationDMO
                                         from b in _buspasscontext.Adm_M_Student
                                         where (a.AMST_Id == data.AMST_Id && a.AMST_Id == b.AMST_Id && a.ASTA_CurrentAY == data.ASMAY_Id)
                                         select new StudentBuspassFormDTO
                                         {
                                             AMST_FatherMobleNo = a.ASTA_FatherMobileNo,
                                             AMST_MotherMobileNo = a.ASTA_MotherMobileNo,
                                             ASTA_Phoneoff = a.ASTA_Phoneoff,
                                             ASTA_PhoneRes = a.ASTA_PhoneRes,
                                             ASTA_Landmark = a.ASTA_Landmark,
                                             AMST_Photoname = b.AMST_Photoname
                                         }
      ).ToList().ToArray();

                        data.AMST_MotherMobileNo = mobilenos.FirstOrDefault().AMST_MotherMobileNo;
                        data.AMST_FatherMobleNo = mobilenos.FirstOrDefault().AMST_FatherMobleNo;
                        data.ASTA_Phoneoff = mobilenos.FirstOrDefault().ASTA_Phoneoff;
                        data.ASTA_PhoneRes = mobilenos.FirstOrDefault().ASTA_PhoneRes;
                        data.ASTA_Landmark = mobilenos.FirstOrDefault().ASTA_Landmark;
                        data.AMST_Photoname = mobilenos.FirstOrDefault().AMST_Photoname;

                        var studentdetails = (from d in _buspasscontext.AcademicYearDMO
                                              from a in _buspasscontext.School_M_Class
                                              from b in _buspasscontext.School_M_Section
                                              from c in _buspasscontext.School_Adm_Y_StudentDMO
                                              from f in _buspasscontext.Adm_M_Student
                                              from g in _buspasscontext.country
                                              from h in _buspasscontext.state
                                              from i in _buspasscontext.MasterRouteDMO
                                              from j in _buspasscontext.MasterLocationDMO
                                              from k in _buspasscontext.Adm_Student_Transport_ApplicationDMO
                                              where (d.ASMAY_Id == f.ASMAY_Id && a.ASMCL_Id == f.ASMCL_Id && c.AMST_Id == f.AMST_Id && c.ASMS_Id == b.ASMS_Id
                                               && g.IVRMMC_Id == f.AMST_ConCountry && h.IVRMMS_Id == f.AMST_ConState && g.IVRMMC_Id == h.IVRMMC_Id &&
                                           i.TRMR_Id == k.ASTA_Drop_TRMR_Id && i.TRMR_Id == k.ASTA_PickUp_TRMR_Id && j.TRML_Id == k.ASTA_PickUp_TRML_Id && j.TRML_Id == k.ASTA_Drop_TRML_Id && f.MI_Id == data.MI_Id && f.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id &&
                                          c.AMST_Id == data.AMST_Id)
                                              select new StudentBuspassFormDTO
                                              {
                                                  AMST_FirstName = f.AMST_FirstName + ' ' + f.AMST_MiddleName + ' ' + f.AMST_LastName,
                                                  ASMCL_Id = c.ASMCL_Id,
                                                  ASMCL_ClassName = a.ASMCL_ClassName,
                                                  ASMS_Id = c.ASMS_Id,
                                                  ASMC_SectionName = b.ASMC_SectionName,
                                                  ASMAY_Id = c.ASMAY_Id,
                                                  ASMAY_Year = d.ASMAY_Year,
                                                  AMST_AdmNo = f.AMST_AdmNo,
                                                  AMST_DOB = f.AMST_DOB,
                                                  AMST_emailId = f.AMST_emailId,
                                                  AMST_MobileNo = f.AMST_MobileNo,
                                                  AMST_PerStreet = f.AMST_ConStreet,
                                                  AMST_PerCity = f.AMST_ConCity,
                                                  AMST_PerArea = f.AMST_ConArea,
                                                  AMST_PerPincode = f.AMST_ConPincode,
                                                  IVRMMC_CountryName = g.IVRMMC_CountryName,
                                                  IVRMMS_Name = h.IVRMMS_Name,
                                                  AMST_FatherName = f.AMST_FatherName,
                                                  AMST_MotherName = f.AMST_MotherName,
                                                  AMST_FatherMobleNo = k.ASTA_FatherMobileNo,
                                                  AMST_MotherMobileNo = k.ASTA_MotherMobileNo,
                                                  AMST_FatherOfficeAdd = f.AMST_FatherOfficeAdd,
                                                  AMST_BloodGroup = f.AMST_BloodGroup,
                                                  ASTA_AreaZoneName = k.ASTA_AreaZoneName,
                                                  ASTA_PickUp_TRML_Id = k.ASTA_PickUp_TRML_Id,
                                                  ASTA_Drop_TRML_Id = k.ASTA_Drop_TRML_Id,
                                                  ASTA_PickUp_TRMR_Id = k.ASTA_PickUp_TRMR_Id,
                                                  ASTA_Drop_TRMR_Id = k.ASTA_Drop_TRMR_Id,
                                                  ASTA_Landmark = k.ASTA_Landmark,
                                                  ASTA_Phoneoff = k.ASTA_Phoneoff,
                                                  ASTA_PhoneRes = k.ASTA_PhoneRes
                                              }
                                   ).Distinct().ToArray();



                        data.studentconstate = (from b in _buspasscontext.state
                                                from c in _buspasscontext.country
                                                where (c.IVRMMC_Id == b.IVRMMC_Id && c.IVRMMC_CountryName == studentdetails[0].IVRMMC_CountryName)
                                                select new StudentBuspassFormDTO
                                                {
                                                    IVRMMS_Id = b.IVRMMS_Id,
                                                    IVRMMS_Name = b.IVRMMS_Name
                                                }).Distinct().ToArray();
                    }
                    else
                    {
                        data.studentDetails = (from d in _buspasscontext.AcademicYearDMO
                                               from a in _buspasscontext.School_M_Class
                                               from b in _buspasscontext.School_M_Section
                                               from c in _buspasscontext.School_Adm_Y_StudentDMO
                                               from f in _buspasscontext.Adm_M_Student
                                               from g in _buspasscontext.country
                                               from h in _buspasscontext.state

                                               where (f.AMST_Id == c.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id
                                               && g.IVRMMC_Id == f.AMST_PerCountry && h.IVRMMS_Id == f.AMST_ConState && g.IVRMMC_Id == h.IVRMMC_Id &&

                                            a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id &&
                                           c.AMST_Id == data.AMST_Id)
                                               select new StudentBuspassFormDTO
                                               {
                                                   AMST_FirstName = f.AMST_FirstName + ' ' + f.AMST_MiddleName + ' ' + f.AMST_LastName,
                                                   ASMCL_Id = c.ASMCL_Id,
                                                   ASMCL_ClassName = a.ASMCL_ClassName,
                                                   ASMS_Id = c.ASMS_Id,
                                                   ASMC_SectionName = b.ASMC_SectionName,
                                                   ASMAY_Id = c.ASMAY_Id,
                                                   ASMAY_Year = d.ASMAY_Year,
                                                   AMST_AdmNo = f.AMST_AdmNo,
                                                   AMST_DOB = f.AMST_DOB,
                                                   AMST_emailId = f.AMST_emailId,
                                                   AMST_MobileNo = f.AMST_MobileNo,
                                                   AMST_PerStreet = f.AMST_ConStreet,
                                                   AMST_PerCity = f.AMST_ConCity,
                                                   AMST_PerArea = f.AMST_ConArea,
                                                   AMST_PerPincode = f.AMST_ConPincode,
                                                   IVRMMC_CountryName = g.IVRMMC_CountryName,
                                                   IVRMMS_Name = h.IVRMMS_Name,
                                                   AMST_FatherName = f.AMST_FatherName,
                                                   AMST_MotherName = f.AMST_MotherName,

                                                   AMST_FatherOfficeAdd = f.AMST_FatherOfficeAdd,
                                               }
            ).Distinct().ToArray();


                        var mobilenos1 = (from a in _buspasscontext.Adm_M_Student
                                          where (a.AMST_Id == data.AMST_Id && a.MI_Id == data.MI_Id)
                                          select new StudentBuspassFormDTO
                                          {
                                              AMST_Photoname = a.AMST_Photoname
                                          }
    ).ToList().ToArray();

                        data.AMST_Photoname = mobilenos1.FirstOrDefault().AMST_Photoname;

                        var studentdetails = (from d in _buspasscontext.AcademicYearDMO
                                              from a in _buspasscontext.School_M_Class
                                              from b in _buspasscontext.School_M_Section
                                              from c in _buspasscontext.School_Adm_Y_StudentDMO
                                              from f in _buspasscontext.Adm_M_Student
                                              from g in _buspasscontext.country
                                              from h in _buspasscontext.state

                                              where (f.AMST_Id == c.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id
                                              && g.IVRMMC_Id == f.AMST_PerCountry && h.IVRMMS_Id == f.AMST_ConState && g.IVRMMC_Id == h.IVRMMC_Id &&

                                           a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id &&
                                          c.AMST_Id == data.AMST_Id)
                                              select new StudentBuspassFormDTO
                                              {
                                                  AMST_FirstName = f.AMST_FirstName + ' ' + f.AMST_MiddleName + ' ' + f.AMST_LastName,
                                                  ASMCL_Id = c.ASMCL_Id,
                                                  ASMCL_ClassName = a.ASMCL_ClassName,
                                                  ASMS_Id = c.ASMS_Id,
                                                  ASMC_SectionName = b.ASMC_SectionName,
                                                  ASMAY_Id = c.ASMAY_Id,
                                                  ASMAY_Year = d.ASMAY_Year,
                                                  AMST_AdmNo = f.AMST_AdmNo,
                                                  AMST_DOB = f.AMST_DOB,
                                                  AMST_emailId = f.AMST_emailId,
                                                  AMST_MobileNo = f.AMST_MobileNo,
                                                  AMST_PerStreet = f.AMST_ConStreet,
                                                  AMST_PerCity = f.AMST_ConCity,
                                                  AMST_PerArea = f.AMST_ConArea,
                                                  AMST_PerPincode = f.AMST_ConPincode,
                                                  IVRMMC_CountryName = g.IVRMMC_CountryName,
                                                  IVRMMS_Name = h.IVRMMS_Name,
                                                  AMST_FatherName = f.AMST_FatherName,
                                                  AMST_MotherName = f.AMST_MotherName,

                                                  AMST_FatherOfficeAdd = f.AMST_FatherOfficeAdd,
                                                  AMST_BloodGroup = f.AMST_BloodGroup,
                                              }
                                   ).Distinct().ToArray();

                        //  data.stutransapp = _buspasscontext.Adm_Student_Transport_ApplicationDMO.Where(r => r.MI_Id == data.MI_Id).ToArray();

                        data.studentconstate = (from b in _buspasscontext.state
                                                from c in _buspasscontext.country
                                                where (c.IVRMMC_Id == b.IVRMMC_Id && c.IVRMMC_CountryName == studentdetails[0].IVRMMC_CountryName)
                                                select new StudentBuspassFormDTO
                                                {
                                                    IVRMMS_Id = b.IVRMMS_Id,
                                                    IVRMMS_Name = b.IVRMMS_Name
                                                }).Distinct().ToArray();
                    }
                }
                else
                {
                    if (trans_amstid.Length > 0)
                    {
                        data.studentDetails = (from d in _buspasscontext.AcademicYearDMO
                                               from a in _buspasscontext.School_M_Class
                                               from b in _buspasscontext.School_M_Section
                                               from c in _buspasscontext.School_Adm_Y_StudentDMO
                                               from f in _buspasscontext.Adm_M_Student
                                               from g in _buspasscontext.country
                                               from h in _buspasscontext.state
                                               from i in _buspasscontext.MasterRouteDMO
                                               from j in _buspasscontext.MasterLocationDMO
                                               from k in _buspasscontext.Adm_Student_Transport_ApplicationDMO

                                               where (d.ASMAY_Id == f.ASMAY_Id && a.ASMCL_Id == f.ASMCL_Id && c.AMST_Id == f.AMST_Id && c.ASMS_Id == b.ASMS_Id
                                               && g.IVRMMC_Id == f.AMST_ConCountry && h.IVRMMS_Id == f.AMST_ConState && g.IVRMMC_Id == h.IVRMMC_Id &&
                                           i.TRMR_Id == k.ASTA_Drop_TRMR_Id && i.TRMR_Id == k.ASTA_PickUp_TRMR_Id && j.TRML_Id == k.ASTA_PickUp_TRML_Id && j.TRML_Id == k.ASTA_Drop_TRML_Id && f.MI_Id == data.MI_Id && f.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id)
                                               select new StudentBuspassFormDTO
                                               {
                                                   AMST_FirstName = f.AMST_FirstName + ' ' + f.AMST_MiddleName + ' ' + f.AMST_LastName,
                                                   ASMCL_Id = c.ASMCL_Id,
                                                   ASMCL_ClassName = a.ASMCL_ClassName,
                                                   ASMS_Id = c.ASMS_Id,
                                                   ASMC_SectionName = b.ASMC_SectionName,
                                                   ASMAY_Id = c.ASMAY_Id,
                                                   ASMAY_Year = d.ASMAY_Year,
                                                   AMST_AdmNo = f.AMST_AdmNo,
                                                   AMST_DOB = f.AMST_DOB,
                                                   AMST_emailId = f.AMST_emailId,
                                                   AMST_MobileNo = f.AMST_MobileNo,
                                                   AMST_PerStreet = f.AMST_ConStreet,
                                                   AMST_PerCity = f.AMST_ConCity,
                                                   AMST_PerArea = f.AMST_ConArea,
                                                   AMST_PerPincode = f.AMST_ConPincode,
                                                   IVRMMC_CountryName = g.IVRMMC_CountryName,
                                                   IVRMMS_Name = h.IVRMMS_Name,
                                                   AMST_FatherName = f.AMST_FatherName,
                                                   AMST_MotherName = f.AMST_MotherName,
                                                   AMST_FatherMobleNo = k.ASTA_FatherMobileNo,
                                                   AMST_MotherMobileNo = k.ASTA_MotherMobileNo,
                                                   AMST_FatherOfficeAdd = f.AMST_FatherOfficeAdd,
                                                   ASTA_AreaZoneName = k.ASTA_AreaZoneName,
                                                   ASTA_PickUp_TRML_Id = k.ASTA_PickUp_TRML_Id,
                                                   ASTA_Drop_TRML_Id = k.ASTA_Drop_TRML_Id,
                                                   ASTA_PickUp_TRMR_Id = k.ASTA_PickUp_TRMR_Id,
                                                   ASTA_Drop_TRMR_Id = k.ASTA_Drop_TRMR_Id,
                                                   ASTA_Landmark = k.ASTA_Landmark,
                                                   ASTA_Phoneoff = k.ASTA_Phoneoff,
                                                   ASTA_PhoneRes = k.ASTA_PhoneRes
                                               }
                       ).Distinct().ToArray();

                        var mobilenos = (from a in _buspasscontext.Adm_Student_Transport_ApplicationDMO
                                         where (a.AMST_Id == data.AMST_Id && a.ASTA_CurrentAY == data.ASMAY_Id)
                                         select new StudentBuspassFormDTO
                                         {
                                             AMST_FatherMobleNo = a.ASTA_FatherMobileNo,
                                             AMST_MotherMobileNo = a.ASTA_MotherMobileNo,
                                             ASTA_PhoneRes = a.ASTA_PhoneRes,
                                             ASTA_Landmark = a.ASTA_Landmark

                                         }
      ).ToList().ToArray();

                        data.AMST_MotherMobileNo = mobilenos.FirstOrDefault().AMST_MotherMobileNo;
                        data.AMST_FatherMobleNo = mobilenos.FirstOrDefault().AMST_FatherMobleNo;
                        data.ASTA_Phoneoff = mobilenos.FirstOrDefault().ASTA_Phoneoff;
                        data.ASTA_PhoneRes = mobilenos.FirstOrDefault().ASTA_PhoneRes;
                        data.ASTA_Landmark = mobilenos.FirstOrDefault().ASTA_Landmark;

                        var studentdetails = (from d in _buspasscontext.AcademicYearDMO
                                              from a in _buspasscontext.School_M_Class
                                              from b in _buspasscontext.School_M_Section
                                              from c in _buspasscontext.School_Adm_Y_StudentDMO
                                              from f in _buspasscontext.Adm_M_Student
                                              from g in _buspasscontext.country
                                              from h in _buspasscontext.state
                                              from i in _buspasscontext.MasterRouteDMO
                                              from j in _buspasscontext.MasterLocationDMO
                                              from k in _buspasscontext.Adm_Student_Transport_ApplicationDMO
                                              where (d.ASMAY_Id == f.ASMAY_Id && a.ASMCL_Id == f.ASMCL_Id && c.AMST_Id == f.AMST_Id && c.ASMS_Id == b.ASMS_Id
                                               && g.IVRMMC_Id == f.AMST_ConCountry && h.IVRMMS_Id == f.AMST_ConState && g.IVRMMC_Id == h.IVRMMC_Id &&
                                           i.TRMR_Id == k.ASTA_Drop_TRMR_Id && i.TRMR_Id == k.ASTA_PickUp_TRMR_Id && j.TRML_Id == k.ASTA_PickUp_TRML_Id && j.TRML_Id == k.ASTA_Drop_TRML_Id && f.MI_Id == data.MI_Id && f.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id
                                         )
                                              select new StudentBuspassFormDTO
                                              {
                                                  AMST_FirstName = f.AMST_FirstName + ' ' + f.AMST_MiddleName + ' ' + f.AMST_LastName,
                                                  ASMCL_Id = c.ASMCL_Id,
                                                  ASMCL_ClassName = a.ASMCL_ClassName,
                                                  ASMS_Id = c.ASMS_Id,
                                                  ASMC_SectionName = b.ASMC_SectionName,
                                                  ASMAY_Id = c.ASMAY_Id,
                                                  ASMAY_Year = d.ASMAY_Year,
                                                  AMST_AdmNo = f.AMST_AdmNo,
                                                  AMST_DOB = f.AMST_DOB,
                                                  AMST_emailId = f.AMST_emailId,
                                                  AMST_MobileNo = f.AMST_MobileNo,
                                                  AMST_PerStreet = f.AMST_ConStreet,
                                                  AMST_PerCity = f.AMST_ConCity,
                                                  AMST_PerArea = f.AMST_ConArea,
                                                  AMST_PerPincode = f.AMST_ConPincode,
                                                  IVRMMC_CountryName = g.IVRMMC_CountryName,
                                                  IVRMMS_Name = h.IVRMMS_Name,
                                                  AMST_FatherName = f.AMST_FatherName,
                                                  AMST_MotherName = f.AMST_MotherName,
                                                  AMST_FatherMobleNo = k.ASTA_FatherMobileNo,
                                                  AMST_MotherMobileNo = k.ASTA_MotherMobileNo,
                                                  AMST_FatherOfficeAdd = f.AMST_FatherOfficeAdd,
                                                  AMST_BloodGroup = f.AMST_BloodGroup,
                                                  ASTA_AreaZoneName = k.ASTA_AreaZoneName,
                                                  ASTA_PickUp_TRML_Id = k.ASTA_PickUp_TRML_Id,
                                                  ASTA_Drop_TRML_Id = k.ASTA_Drop_TRML_Id,
                                                  ASTA_PickUp_TRMR_Id = k.ASTA_PickUp_TRMR_Id,
                                                  ASTA_Drop_TRMR_Id = k.ASTA_Drop_TRMR_Id,
                                                  ASTA_Landmark = k.ASTA_Landmark,
                                                  ASTA_Phoneoff = k.ASTA_Phoneoff,
                                                  ASTA_PhoneRes = k.ASTA_PhoneRes
                                              }
                                   ).Distinct().ToArray();

                        //  data.stutransapp = _buspasscontext.Adm_Student_Transport_ApplicationDMO.Where(r => r.MI_Id == data.MI_Id).ToArray();

                        data.studentconstate = (from b in _buspasscontext.state
                                                from c in _buspasscontext.country
                                                where (c.IVRMMC_Id == b.IVRMMC_Id && c.IVRMMC_CountryName == studentdetails[0].IVRMMC_CountryName)
                                                select new StudentBuspassFormDTO
                                                {
                                                    IVRMMS_Id = b.IVRMMS_Id,
                                                    IVRMMS_Name = b.IVRMMS_Name
                                                }).Distinct().ToArray();
                    }
                    else
                    {
                        data.studentDetails = (from d in _buspasscontext.AcademicYearDMO
                                               from a in _buspasscontext.School_M_Class
                                               from b in _buspasscontext.School_M_Section
                                               from c in _buspasscontext.School_Adm_Y_StudentDMO
                                               from f in _buspasscontext.Adm_M_Student
                                               from g in _buspasscontext.country
                                               from h in _buspasscontext.state

                                               where (f.AMST_Id == c.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id
                                               && g.IVRMMC_Id == f.AMST_PerCountry && h.IVRMMS_Id == f.AMST_ConState && g.IVRMMC_Id == h.IVRMMC_Id &&

                                            a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id &&
                                           c.AMST_Id == data.AMST_Id)
                                               select new StudentBuspassFormDTO
                                               {
                                                   AMST_FirstName = f.AMST_FirstName + ' ' + f.AMST_MiddleName + ' ' + f.AMST_LastName,
                                                   ASMCL_Id = c.ASMCL_Id,
                                                   ASMCL_ClassName = a.ASMCL_ClassName,
                                                   ASMS_Id = c.ASMS_Id,
                                                   ASMC_SectionName = b.ASMC_SectionName,
                                                   ASMAY_Id = c.ASMAY_Id,
                                                   ASMAY_Year = d.ASMAY_Year,
                                                   AMST_AdmNo = f.AMST_AdmNo,
                                                   AMST_DOB = f.AMST_DOB,
                                                   AMST_emailId = f.AMST_emailId,
                                                   AMST_MobileNo = f.AMST_MobileNo,
                                                   AMST_PerStreet = f.AMST_ConStreet,
                                                   AMST_PerCity = f.AMST_ConCity,
                                                   AMST_PerArea = f.AMST_ConArea,
                                                   AMST_PerPincode = f.AMST_ConPincode,
                                                   IVRMMC_CountryName = g.IVRMMC_CountryName,
                                                   IVRMMS_Name = h.IVRMMS_Name,
                                                   AMST_FatherName = f.AMST_FatherName,
                                                   AMST_MotherName = f.AMST_MotherName,

                                                   AMST_FatherOfficeAdd = f.AMST_FatherOfficeAdd,
                                               }
            ).Distinct().ToArray();

                        var studentdetails = (from d in _buspasscontext.AcademicYearDMO
                                              from a in _buspasscontext.School_M_Class
                                              from b in _buspasscontext.School_M_Section
                                              from c in _buspasscontext.School_Adm_Y_StudentDMO
                                              from f in _buspasscontext.Adm_M_Student
                                              from g in _buspasscontext.country
                                              from h in _buspasscontext.state

                                              where (f.AMST_Id == c.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id
                                              && g.IVRMMC_Id == f.AMST_PerCountry && h.IVRMMS_Id == f.AMST_ConState && g.IVRMMC_Id == h.IVRMMC_Id &&

                                           a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id &&
                                          c.AMST_Id == data.AMST_Id)
                                              select new StudentBuspassFormDTO
                                              {
                                                  AMST_FirstName = f.AMST_FirstName + ' ' + f.AMST_MiddleName + ' ' + f.AMST_LastName,
                                                  ASMCL_Id = c.ASMCL_Id,
                                                  ASMCL_ClassName = a.ASMCL_ClassName,
                                                  ASMS_Id = c.ASMS_Id,
                                                  ASMC_SectionName = b.ASMC_SectionName,
                                                  ASMAY_Id = c.ASMAY_Id,
                                                  ASMAY_Year = d.ASMAY_Year,
                                                  AMST_AdmNo = f.AMST_AdmNo,
                                                  AMST_DOB = f.AMST_DOB,
                                                  AMST_emailId = f.AMST_emailId,
                                                  AMST_MobileNo = f.AMST_MobileNo,
                                                  AMST_PerStreet = f.AMST_ConStreet,
                                                  AMST_PerCity = f.AMST_ConCity,
                                                  AMST_PerArea = f.AMST_ConArea,
                                                  AMST_PerPincode = f.AMST_ConPincode,
                                                  IVRMMC_CountryName = g.IVRMMC_CountryName,
                                                  IVRMMS_Name = h.IVRMMS_Name,
                                                  AMST_FatherName = f.AMST_FatherName,
                                                  AMST_MotherName = f.AMST_MotherName,
                                                  AMST_FatherOfficeAdd = f.AMST_FatherOfficeAdd,
                                                  AMST_BloodGroup = f.AMST_BloodGroup,
                                              }
                                   ).Distinct().ToArray();

                        //  data.stutransapp = _buspasscontext.Adm_Student_Transport_ApplicationDMO.Where(r => r.MI_Id == data.MI_Id).ToArray();

                        data.studentconstate = (from b in _buspasscontext.state
                                                from c in _buspasscontext.country
                                                where (c.IVRMMC_Id == b.IVRMMC_Id && c.IVRMMC_CountryName == studentdetails[0].IVRMMC_CountryName)
                                                select new StudentBuspassFormDTO
                                                {
                                                    IVRMMS_Id = b.IVRMMS_Id,
                                                    IVRMMS_Name = b.IVRMMS_Name
                                                }).Distinct().ToArray();
                    }
                }

                data.routeDetails = (from a in _buspasscontext.Adm_Student_Transport_ApplicationDMO
                                     from d in _buspasscontext.Adm_M_Student
                                     where (a.AMST_Id == d.AMST_Id && d.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && d.AMST_ActiveFlag == 1 && d.AMST_SOL == "S" && a.AMST_Id == data.AMST_Id)
                                     select new StudentBuspassFormDTO
                                     {
                                         AMST_Id = a.AMST_Id,
                                         ASTA_Id = a.ASTA_Id,
                                         AMST_FirstName = d.AMST_FirstName + ' ' + d.AMST_MiddleName + ' ' + d.AMST_LastName,
                                         ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
                                         TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _buspasscontext.MasterLocationDMO.Single(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).TRML_LocationName : "--",

                                         ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
                                         TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _buspasscontext.MasterLocationDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).TRML_LocationName : "--",

                                         TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
                                         TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _buspasscontext.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).TRMR_RouteName : "--",

                                         TRMR_Idd = a.ASTA_Drop_TRMR_Id,
                                         TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _buspasscontext.MasterRouteDMO.Single(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).TRMR_RouteName : "--"
                                     }
                       ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentBuspassFormDTO getroutedata(StudentBuspassFormDTO data)
        {
            try
            {
                data.routelist = (from a in _buspasscontext.MasterAreaDMO
                                  from b in _buspasscontext.MasterRouteDMO
                                  where (a.TRMA_Id == b.TRMA_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && b.TRMR_ActiveFlg==true)
                                  select new StudentBuspassFormDTO
                                  {
                                      TRMR_Id = b.TRMR_Id,
                                      TRMR_RouteName = b.TRMR_RouteName,
                                      TRMR_RouteNo = b.TRMR_RouteNo,
                                      TRMR_order = b.TRMR_order,
                                  }
                 ).ToList().OrderBy(f=>f.TRMR_order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentBuspassFormDTO getlocationdata(StudentBuspassFormDTO data)
        {
            try
            {
                data.locationlist = (from a in _buspasscontext.Route_Location
                                     from b in _buspasscontext.MasterRouteDMO
                                     from c in _buspasscontext.MasterLocationDMO
                                     where (a.TRMR_Id == b.TRMR_Id && a.TRML_Id == c.TRML_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id && a.TRMRL_ActiveFlag==true && b.TRMR_ActiveFlg==true  && c.TRML_ActiveFlg==true)
                                     select new StudentBuspassFormDTO
                                     {
                                         TRML_Id = c.TRML_Id,
                                         TRML_LocationName = c.TRML_LocationName
                                     }
                 ).ToList().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public StudentBuspassFormDTO getlocationdataonly(StudentBuspassFormDTO data)
        {
            try
            {
                data.locationlist = (from a in _buspasscontext.Route_Location
                                     from b in _buspasscontext.MasterRouteDMO
                                     from c in _buspasscontext.MasterLocationDMO
                                     where (a.TRMR_Id == b.TRMR_Id && a.TRML_Id == c.TRML_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id && a.TRMR_Id == data.TRMR_Id && a.TRMRL_ActiveFlag==true && c.TRML_ActiveFlg==true && b.TRMR_ActiveFlg==true)
                                     select new StudentBuspassFormDTO
                                     {
                                         TRML_Id = c.TRML_Id,
                                         TRML_LocationName = c.TRML_LocationName
                                     }
                 ).ToList().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<StudentBuspassFormDTO> getbuspassdata(StudentBuspassFormDTO data)
        {
            try
            {
                var studentcurrentyear = (from a in _buspasscontext.School_Adm_Y_StudentDMO
                                          where (a.AMST_Id == data.AMST_Id)
                                          select a
       ).ToList().OrderByDescending(d => d.ASYST_Id).ToArray();

                if (studentcurrentyear.Length > 0)
                {
                    if (studentcurrentyear.FirstOrDefault().ASMAY_Id != data.ASMAY_Id)
                    {
                        data.studentaccyear = studentcurrentyear.FirstOrDefault().ASMAY_Id;
                    }
                    else
                    {
                        data.studentaccyear = data.ASMAY_Id;

                    }

                }

                else
                {
                    var studentadmityear = (from a in _buspasscontext.Adm_M_Student
                                            where (a.AMST_Id == data.AMST_Id)
                                            select a
                 ).ToList().ToArray();


                    if (studentadmityear.FirstOrDefault().ASMAY_Id != data.ASMAY_Id)
                    {
                        data.studentaccyear = studentadmityear.FirstOrDefault().ASMAY_Id;
                    }
                    else
                    {
                        data.studentaccyear = data.ASMAY_Id;
                    }
                }

                using (var cmd = _buspasscontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Buspass_Form_details";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@amst",
                SqlDbType.VarChar)
                    {
                        Value = data.AMST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@asta",
               SqlDbType.VarChar)
                    {
                        Value = data.ASTA_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
              SqlDbType.VarChar)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@asmay_id",
           SqlDbType.BigInt)
                    {
                        Value = data.studentaccyear
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = await cmd.ExecuteReaderAsync())
                        {
                            while (await dataReader.ReadAsync())
                            {
                                var dataRow = new ExpandoObject() as IDictionary<string, object>;
                                for (var iFiled = 0; iFiled < dataReader.FieldCount; iFiled++)
                                {
                                    dataRow.Add(
                                        dataReader.GetName(iFiled),
                                        dataReader.IsDBNull(iFiled) ? null : dataReader[iFiled] // use null instead of {}
                                    );
                                }

                                retObject.Add((ExpandoObject)dataRow);
                            }
                        }
                        data.buspassdatalist = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                var regularnewff = (from f in _feecontext.Adm_Student_Transport_ApplicationDMO
                                    where (f.AMST_Id == data.AMST_Id && f.ASTA_CurrentAY == data.ASMAY_Id)
                                    select new StudentBuspassFormDTO
                                    {
                                        classnextid = f.ASTA_FutureAY,
                                        ASTA_ApplStatus = f.ASTA_ApplStatus
                                    }
        ).ToList();



                List<AcademicYear> acayr = new List<AcademicYear>();
                acayr = _buspasscontext.AcademicYearDMO.Where(r => r.MI_Id == data.MI_Id && r.ASMAY_ActiveFlag == 1).ToList();
                data.fillyear = acayr.ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            //string html = "";
            //string path = "D:\\IVRMCODE\\25FEB2019\\IVRMUX\\wwwroot\\htmlpage.html";
            /////  html = "";
            //html = File.ReadAllText(path);
            //data.htmldata = html;
            return data;
        }

        public async Task<StudentBuspassFormDTO> getbuspassdataupdate(StudentBuspassFormDTO data)
        {
            try
            {

                List<StudentBuspassFormDTO> details = new List<StudentBuspassFormDTO>();

                details = (from a in _buspasscontext.Adm_Student_Transport_ApplicationDMO
                                 where (a.MI_Id == data.MI_Id  && a.ASTA_FutureAY == data.ASMAY_Id)
                                 select new StudentBuspassFormDTO
                                 {
                                     AMST_Id = a.AMST_Id,
                                    
                                 }
          ).ToList();

                List<long> amstids = new List<long>();
                foreach (var item in details)
                {
                    amstids.Add(item.AMST_Id);
                }
                

                data.stu_name = (from a in _buspasscontext.Adm_M_Student
                                 from b in _buspasscontext.Adm_Student_Transport_ApplicationDMO
                                 where (a.MI_Id == data.MI_Id && b.AMST_Id==a.AMST_Id && b.ASTA_FutureAY==data.ASMAY_Id &&  amstids.Contains(b.AMST_Id))
                                 select new StudentBuspassFormDTO
                                 {
                                     AMST_Id = a.AMST_Id,
                                     AMST_FirstName = a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName,
                                 }
          ).ToList().ToArray();

                data.fillstudent = (from a in _buspasscontext.Adm_M_Student
                                    from b in _buspasscontext.Adm_Student_Transport_ApplicationDMO
                                    where (a.MI_Id ==70000)
                                    select new StudentBuspassFormDTO
                                    {
                                        AMST_Id = a.AMST_Id,
                                        AMST_FirstName = a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName,
                                    }
          ).ToList().ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentBuspassFormDTO savedata(StudentBuspassFormDTO data)
        {
            try 
            {

                //Academic Year 
                var studentcurrentyear = (from a in _buspasscontext.School_Adm_Y_StudentDMO
                                          where (a.AMST_Id == data.studentid)
                                          select a
                  ).ToList().OrderByDescending(d => d.ASYST_Id).ToArray();

                if (studentcurrentyear.Length > 0)
                {
                    if (studentcurrentyear.FirstOrDefault().ASMAY_Id != data.transportyear)
                    {
                        data.studentaccyear = studentcurrentyear.FirstOrDefault().ASMAY_Id;
                        data.studentclass = studentcurrentyear.FirstOrDefault().ASMCL_Id;

                        var cls_orderid = (from a in _buspasscontext.School_M_Class
                                           where (a.ASMCL_Id == data.studentclass && a.MI_Id == data.MI_Id)
                                           select new StudentBuspassFormDTO
                                           {
                                               cls_Order = a.ASMCL_Order + 1
                                           }
             ).ToList().ToArray();

                        var class_Id = (from a in _buspasscontext.School_M_Class
                                        where (a.ASMCL_Order == cls_orderid[0].cls_Order && a.MI_Id == data.MI_Id)
                                        select new StudentBuspassFormDTO
                                        {
                                            cls_Id = a.ASMCL_Id
                                        }
                         ).ToList().ToArray();

                        data.studentfutureclass = class_Id.FirstOrDefault().cls_Id;
                    }
                    else
                    {
                        data.studentaccyear = data.transportyear;
                        data.studentclass = studentcurrentyear.FirstOrDefault().ASMCL_Id;
                        data.studentfutureclass = studentcurrentyear.FirstOrDefault().ASMCL_Id;
                    }

                }

                else
                {
                    var studentadmityear = (from a in _buspasscontext.Adm_M_Student
                                            where (a.AMST_Id == data.studentid)
                                            select a
                 ).ToList().ToArray();


                    if (studentadmityear.FirstOrDefault().ASMAY_Id != data.transportyear)
                    {
                        data.studentaccyear = studentadmityear.FirstOrDefault().ASMAY_Id;
                        data.studentclass = studentadmityear.FirstOrDefault().ASMCL_Id;
                        var cls_orderid = (from a in _buspasscontext.School_M_Class
                                           where (a.ASMCL_Id == data.studentclass && a.MI_Id == data.MI_Id)
                                           select new StudentBuspassFormDTO
                                           {
                                               cls_Order = a.ASMCL_Order + 1
                                           }
               ).ToList().ToArray();

                        var class_Id = (from a in _buspasscontext.School_M_Class
                                        where (a.ASMCL_Order == cls_orderid[0].cls_Order && a.MI_Id == data.MI_Id)
                                        select new StudentBuspassFormDTO
                                        {
                                            cls_Id = a.ASMCL_Id
                                        }
                         ).ToList().ToArray();

                        data.studentfutureclass = class_Id.FirstOrDefault().cls_Id;
                    }
                    else
                    {
                        data.studentaccyear = data.transportyear;
                        data.studentclass = studentadmityear.FirstOrDefault().ASMCL_Id;
                        data.studentfutureclass = studentadmityear.FirstOrDefault().ASMCL_Id;
                    }
                }





                var duplicatecount = _buspasscontext.Adm_Student_Transport_ApplicationDMO.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == data.studentid
                && t.ASTA_FutureAY == data.ASMAY_Id).Count();
                if (duplicatecount == 0)
                {
                    if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                    {
                        GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                        data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                        data.transnumbconfigurationsettingsss.ASMAY_Id = data.transportyear;
                        data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                    }



                    data.regularnew = (from a in _feecontext.FeeGroupDMO
                                       from b in _feecontext.FeeYearlygroupHeadMappingDMO
                                       from c in _feecontext.FeeStudentTransactionDMO
                                       from d in _feecontext.FeeHeadDMO
                                       where (b.FMG_Id == a.FMG_Id && b.FMH_Id == d.FMH_Id && c.FMG_Id == b.FMG_Id && c.FMH_Id == b.FMH_Id && c.FMG_Id == a.FMG_Id && c.FMH_Id == d.FMH_Id && c.ASMAY_Id == b.ASMAY_Id && d.FMH_Flag == "T" && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.ASMAY_Id && c.FSS_CurrentYrCharges > 0 && c.AMST_Id == data.studentid)
                                       select new StudentBuspassFormDTO
                                       {
                                           AMST_Id = c.AMST_Id

                                       }
             ).ToList().ToArray();

                    if (data.regularnew.Length > 0)
                    {
                        data.newregular = "Regular";
                    }
                    else
                    {
                        data.newregular = "New";
                    }

                    Adm_Student_Transport_ApplicationDMO admiss = Mapper.Map<Adm_Student_Transport_ApplicationDMO>(data);
                    admiss.ASTA_FutureAY = data.transportyear;
                    admiss.ASTA_CurrentAY = Convert.ToInt64(data.studentaccyear);
                    admiss.ASTA_CurrentClass = Convert.ToInt64(data.studentclass);
                    admiss.ASTA_FutureClass = Convert.ToInt64(data.studentfutureclass);
                    admiss.ASTA_ApplicationDate = DateTime.Now;
                    admiss.ASTA_ActiveFlag = true;
                    admiss.ASTA_Landmark = data.ASTA_Landmark;
                    admiss.ASTA_ApplStatus = "Waiting";
                    admiss.ASTA_Regnew = data.newregular;
                    admiss.ASTA_AreaZoneName = data.TRMA_AreaName;
                    admiss.ASTA_FatherMobileNo = data.ASTA_FatherMobileNo;
                    admiss.ASTA_MotherMobileNo = data.ASTA_MotherMobileNo;
                    admiss.ASTA_PickUp_TRML_Id = Convert.ToInt64(data.TRML_Idp);
                    admiss.ASTA_Drop_TRML_Id = Convert.ToInt64(data.TRML_Idd);
                    admiss.ASTA_Drop_TRMR_Id = Convert.ToInt64(data.TRMR_Idd);
                    admiss.ASTA_PickUp_TRMR_Id = Convert.ToInt64(data.TRMR_Idp);
                    admiss.ASTA_ApplicationNo = data.trans_id;
                    admiss.ASTA_PhoneRes = data.ASTA_PhoneRes;
                    admiss.ASTA_Phoneoff = data.ASTA_Phoneoff;

                    admiss.CreatedDate = DateTime.Now;
                    admiss.UpdatedDate = DateTime.Now;

                    _buspasscontext.Add(admiss);
                    List<Adm_M_Student> studentdetails = new List<Adm_M_Student>();
                    studentdetails = _buspasscontext.Adm_M_Student.Where(t => t.AMST_Id == data.studentid).ToList();


                    if (studentdetails.Count() > 0)
                    {
                        // data.ASMCL_Id = studentdetails.FirstOrDefault().ASMCL_Id;
                        data.ASMAY_Id = studentdetails.FirstOrDefault().ASMAY_Id;
                        data.AMST_FatherName = studentdetails.FirstOrDefault().AMST_FirstName;
                        data.AMST_MobileNo = studentdetails.FirstOrDefault().AMST_MobileNo;
                        data.AMST_emailId = studentdetails.FirstOrDefault().AMST_emailId;

                        data.paymentapplicable = "Pay";
                        data.payementcheck = (from a in _feecontext.FeeAmountEntryDMO
                                              from b in _feecontext.FeeTransactionPaymentDMO
                                              from d in _feecontext.FeeGroupDMO
                                              from f in _feecontext.FeeHeadDMO
                                              from g in _feecontext.feeYCCC
                                              from h in _feecontext.feeYCC
                                              from i in _feecontext.Fee_Y_Payment_School_StudentDMO
                                              where (a.FMG_Id == d.FMG_Id && a.FMH_Id == f.FMH_Id && a.FMCC_Id == h.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMA_Id == a.FMA_Id && h.ASMAY_Id == a.ASMAY_Id && i.FYP_Id == b.FYP_Id && f.FMH_Flag == "T" && d.FMG_CompulsoryFlag == "T" && i.AMST_Id == data.studentid)
                                              select new FeeAmountEntryDMO
                                              {
                                                  FMA_Id = a.FMA_Id,
                                                  FMA_Amount = a.FMA_Amount
                                              }
           ).Count();

                        //if (data.payementcheck == 0)
                        //{
                        //    data.paydet = paymentPart(data);
                        //}

                    }

                    int contactExists = _buspasscontext.SaveChanges();
                    if (contactExists > 0)
                    {


                        data.returnval = "true";
                        var admissphoto = _buspasscontext.Adm_M_Student.Single(t => t.MI_Id == data.MI_Id && t.AMST_Id == data.studentid);

                        admissphoto.AMST_Photoname = data.AMST_Photoname;
                        _buspasscontext.Update(admissphoto);




                       
                        int contactExistsphoto = _buspasscontext.SaveChanges();
                    }
                    else
                    {
                        data.returnval = "false";
                    }
                }
                else if (duplicatecount > 0)
                {

                    //var admissstatus = _buspasscontext.Adm_Student_Transport_ApplicationDMO.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == data.studentid && t.ASTA_ApplStatus == "Approved" && t.ASTA_FutureAY == data.ASMAY_Id).Count();
                    //if (admissstatus == 0)
                    //{.
                   List<Adm_Student_Transport_ApplicationDMO> duplicatedata = new List<Adm_Student_Transport_ApplicationDMO>();
                     duplicatedata = _buspasscontext.Adm_Student_Transport_ApplicationDMO.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == data.studentid
                     && t.ASTA_FutureAY == data.ASMAY_Id).ToList();

                    if(duplicatedata.FirstOrDefault().ASTA_Drop_TRMR_Id==0 || duplicatedata.FirstOrDefault().ASTA_PickUp_TRMR_Id==0)
                    {
                        data.oneortwoway = "1";
                    }
                    else
                    {
                        data.oneortwoway = "2";
                    }


                    if (data.TRMR_Idd == 0 || data.TRMR_Idp == 0)
                    {
                        data.oneortwowayupdate = "1";
                    }
                    else
                    {
                        data.oneortwowayupdate = "2";
                    }

                    string studentupdatedway = data.oneortwoway + "TO" + data.oneortwowayupdate;


                    Adm_Student_Transport_Application_UpdateDMO updateapplication = new Adm_Student_Transport_Application_UpdateDMO();

                    updateapplication.ASTA_Id = duplicatedata.FirstOrDefault().ASTA_Id;
                    updateapplication.TRMA_Id = duplicatedata.FirstOrDefault().TRMA_Id;
                    updateapplication.ASTA_ApplicationDate = Convert.ToDateTime(duplicatedata.FirstOrDefault().UpdatedDate);
                    updateapplication.ASTA_Drop_TRMR_Id = duplicatedata.FirstOrDefault().ASTA_Drop_TRMR_Id;
                    updateapplication.ASTA_Drop_TRML_Id = duplicatedata.FirstOrDefault().ASTA_Drop_TRML_Id;
                    updateapplication.ASTA_PickUp_TRMR_Id = duplicatedata.FirstOrDefault().ASTA_PickUp_TRMR_Id;
                    updateapplication.ASTA_PickUp_TRML_Id = duplicatedata.FirstOrDefault().ASTA_PickUp_TRML_Id;
                    updateapplication.ASTA_Landmark = duplicatedata.FirstOrDefault().ASTA_Landmark;
                    updateapplication.TRMAU_Id = duplicatedata.FirstOrDefault().TRMA_Id;
                    updateapplication.ASTAU_UpdateDate = DateTime.Now;
                    updateapplication.ASTAU_Drop_TRMR_Id = Convert.ToInt64(data.TRMR_Idd);
                    updateapplication.ASTAU_Drop_TRML_Id = Convert.ToInt64(data.TRML_Idd);
                    updateapplication.ASTAU_PickUp_TRMR_Id = Convert.ToInt64(data.TRMR_Idp);
                    updateapplication.ASTAU_PickUp_TRML_Id = Convert.ToInt64(data.TRML_Idp);
                    updateapplication.ASTAU_Landmark = data.ASTA_Landmark;
                    updateapplication.ASTAU_Type = studentupdatedway;
                    _buspasscontext.Add(updateapplication);


                    data.message = "Duplicate";
                        //Adm_Student_Transport_ApplicationDMO admiss = Mapper.Map<Adm_Student_Transport_ApplicationDMO>(data);
                        var admiss = _buspasscontext.Adm_Student_Transport_ApplicationDMO.Single(t => t.MI_Id == data.MI_Id && t.AMST_Id == data.studentid && t.ASTA_FutureAY == data.ASMAY_Id);

                        //data.ASTA_ApplStatus = admiss.ASTA_ApplStatus;
                        //data.ASTA_ActiveFlag = admiss.ASTA_ActiveFlag;
                        admiss.ASTA_FutureAY = data.transportyear;
                        admiss.ASTA_CurrentAY = Convert.ToInt64(data.studentaccyear);
                        admiss.ASTA_CurrentClass = Convert.ToInt64(data.studentclass);
                        admiss.ASTA_FutureClass = Convert.ToInt64(data.studentfutureclass);
                        //admiss.ASTA_ApplicationDate = DateTime.Now;
                        //admiss.ASTA_ActiveFlag = data.ASTA_ActiveFlag;
                        admiss.ASTA_Landmark = data.ASTA_Landmark;
                       //admiss.ASTA_ApplStatus = data.ASTA_ApplStatus;
                        admiss.ASTA_AreaZoneName = data.TRMA_AreaName;
                        admiss.ASTA_FatherMobileNo = data.ASTA_FatherMobileNo;
                        admiss.ASTA_MotherMobileNo = data.ASTA_MotherMobileNo;
                        admiss.ASTA_PickUp_TRML_Id = Convert.ToInt64(data.TRML_Idp);
                        admiss.ASTA_Drop_TRML_Id = Convert.ToInt64(data.TRML_Idd);
                        admiss.ASTA_Drop_TRMR_Id = Convert.ToInt64(data.TRMR_Idd);
                        admiss.ASTA_PickUp_TRMR_Id = Convert.ToInt64(data.TRMR_Idp);
                        admiss.ASTA_PhoneRes = data.ASTA_PhoneRes;
                        admiss.ASTA_Phoneoff = data.ASTA_Phoneoff;
                        // admiss.CreatedDate = DateTime.Now;
                        admiss.UpdatedDate = DateTime.Now;

                        _buspasscontext.Update(admiss);


                  

                    List<Adm_M_Student> studentdetails = new List<Adm_M_Student>();
                        studentdetails = _buspasscontext.Adm_M_Student.Where(t => t.AMST_Id == data.studentid).ToList();




               //         if (studentdetails.Count() > 0)
               //         {
               //             // data.ASMCL_Id = studentdetails.FirstOrDefault().ASMCL_Id;
               //             data.ASMAY_Id = studentdetails.FirstOrDefault().ASMAY_Id;
               //             data.AMST_FirstName = studentdetails.FirstOrDefault().AMST_FirstName;
               //             data.AMST_MobileNo = studentdetails.FirstOrDefault().AMST_MobileNo;
               //             data.AMST_emailId = studentdetails.FirstOrDefault().AMST_emailId;

               //             data.paymentapplicable = "Pay";
               //             data.payementcheck = (from a in _feecontext.FeeAmountEntryDMO
               //                                   from b in _feecontext.FeeTransactionPaymentDMO
               //                                   from d in _feecontext.FeeGroupDMO
               //                                   from f in _feecontext.FeeHeadDMO
               //                                   from g in _feecontext.feeYCCC
               //                                   from h in _feecontext.feeYCC
               //                                   from i in _feecontext.Fee_Y_Payment_School_StudentDMO
               //                                   where (a.FMG_Id == d.FMG_Id && a.FMH_Id == f.FMH_Id && a.FMCC_Id == h.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMA_Id == a.FMA_Id && h.ASMAY_Id == a.ASMAY_Id && i.FYP_Id == b.FYP_Id && f.FMH_Flag == "T" && d.FMG_CompulsoryFlag == "T" && i.AMST_Id == data.studentid)
               //                                   select new FeeAmountEntryDMO
               //                                   {
               //                                       FMA_Id = a.FMA_Id,
               //                                       FMA_Amount = a.FMA_Amount
               //                                   }
               //).Count();

               //             //if (data.payementcheck == 0)
               //             //{
               //             //    data.paydet = paymentPart(data);
               //             //}

               //         }


                        int contactExists = _buspasscontext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = "true";
                            var admissphoto = _buspasscontext.Adm_M_Student.Single(t => t.MI_Id == data.MI_Id && t.AMST_Id == data.studentid);

                            admissphoto.AMST_Photoname = data.AMST_Photoname;
                            _buspasscontext.Update(admissphoto);
                            int contactExistsphoto = _buspasscontext.SaveChanges();


                       

                        var update1 = _buspasscontext.Adm_Student_Transport_ApplicationDMO.Single(a => a.MI_Id == data.MI_Id && a.AMST_Id == data.studentid && a.ASTA_FutureAY == data.ASMAY_Id);

                        var fmgidlist = (from a in _buspasscontext.FeeGroupDMO
                                         from b in _buspasscontext.FeeStudentGroupMappingDMO
                                         where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.FMG_ActiceFlag == true && a.FMG_CompulsoryFlag == "T" && a.FMG_Id == b.FMG_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMST_Id == data.studentid
                                         select new StudentBuspassFormDTO
                                         {
                                             FMG_Id = a.FMG_Id
                                         }).Distinct().ToList();




                        var stdlatest = _buspasscontext.Adm_Student_Transport_ApplicationDMO.Where(a => a.MI_Id == data.MI_Id && a.AMST_Id == data.studentid && a.ASTA_FutureAY == data.ASMAY_Id && a.ASTA_ApplStatus == "Approved").ToList();
                        if (stdlatest.Count>0)
                        {
                            var confirmstatus1 = _buspasscontext.Database.ExecuteSqlCommand("transportfeemapping_Delete_BALDWINS @p0,@p1,@p2",
                             data.MI_Id, data.ASMAY_Id, data.studentid);


                            var confirmstatus = _buspasscontext.Database.ExecuteSqlCommand("Auto_Fee_Group_mapping_Transport @p0,@p1,@p2,@p3",
                                   data.MI_Id, data.ASMAY_Id, data.studentid, data.UserId);


                            var stu_rec_list = _buspasscontext.TR_Student_RouteDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMST_Id == data.studentid).ToList();
                            if (stu_rec_list.Count > 0)
                            {
                                var feegrplist = _buspasscontext.TR_Student_Route_FeeGroupDMO.Where(t => t.TRSR_Id == stu_rec_list[0].TRSR_Id).ToList();
                                foreach (var delff in feegrplist)
                                {
                                    _buspasscontext.Remove(delff);
                                }

                                foreach (var del_stu in stu_rec_list)
                                {
                                    _buspasscontext.Remove(del_stu);
                                }

                                _buspasscontext.SaveChanges();
                            }



                            TR_Student_RouteDMO object123 = new TR_Student_RouteDMO();
                            object123.MI_Id = data.MI_Id;
                            object123.ASMAY_Id = update1.ASTA_FutureAY;
                            object123.AMST_Id = update1.AMST_Id;
                            object123.TRSR_Date = DateTime.Now.Date;
                            //object123.FMG_Id = x.TRML_Id;
                            object123.TRMR_Id = update1.ASTA_PickUp_TRMR_Id;
                            // object123.TRSR_PickupSchedule = stu.savetmpdata[i].TRSR_PickupSchedule;
                            object123.TRSR_PickUpLocation = update1.ASTA_PickUp_TRML_Id;

                            object123.TRSR_PickUpMobileNo = update1.ASTA_FatherMobileNo.GetValueOrDefault();


                            object123.TRMR_Drop_Route = update1.ASTA_Drop_TRMR_Id;
                            // object123.TRSR_DropSchedule = stu.savetmpdata[i].TRSR_DropSchedule;
                            object123.TRSR_DropLocation = update1.ASTA_Drop_TRML_Id;
                            object123.TRSR_DropMobileNo = update1.ASTA_FatherMobileNo.GetValueOrDefault();
                            object123.TRSR_ApplicationNo = Convert.ToInt64(update1.ASTA_ApplicationNo);
                            object123.TRSR_PickupSession = update1.ASTA_PickUp_TRMS_Id.GetValueOrDefault();
                            object123.TRSR_DropSession = update1.ASTA_Drop_TRMS_Id.GetValueOrDefault();
                            object123.TRSR_ActiveFlg = true;
                            object123.CreatedDate = DateTime.Now;
                            object123.UpdatedDate = DateTime.Now;
                            object123.ASTA_Id = update1.ASTA_Id;
                            _buspasscontext.Add(object123);
                            _buspasscontext.SaveChanges();
                            foreach (var x in fmgidlist)
                            {
                                TR_Student_Route_FeeGroupDMO oobj = new TR_Student_Route_FeeGroupDMO();
                                oobj.TRSR_Id = object123.TRSR_Id;
                                oobj.FMG_Id = x.FMG_Id;
                                oobj.TRSRFG_ActiveFlg = true;
                                _buspasscontext.Add(oobj);
                            }


                            _buspasscontext.SaveChanges();


                        }
                        
                    }
                        else
                        {
                            data.returnval = "false";
                        }


                    //}
                    //else
                    //{
                    //    data.returnval = "Y";
                    //}

                }


            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        public Array paymentPart(StudentBuspassFormDTO enq)
        {
            Payment pay = new Payment(_db);
            ProspectusDTO data = new ProspectusDTO();
            List<Prospepaymentamount> paymentdetails = new List<Prospepaymentamount>();
            PaymentDetails PaymentDetailsDto = new PaymentDetails();
            int autoinc = 1, totpayableamount = 0;

            List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();
            //enq.ASMAY_Id = 7;
            try
            {
                paymentdetails = _ProspectusContext.Prospepaymentamount.Where(t => t.IVRMOP_MIID == enq.MI_Id).ToList();
                // ProspectusDTO ProspectusDTO = new ProspectusDTO();
                var FeeAmountresult = (from a in _feecontext.feeYCC

                                       from c in _feecontext.feeYCCC
                                       from d in _feecontext.FeeAmountEntryDMO

                                       from g in _feecontext.FeeHeadDMO
                                       from e in _feecontext.FeeGroupDMO
                                       where (d.FMH_Id == g.FMH_Id && d.FMCC_Id == a.FMCC_Id && a.ASMAY_Id == d.ASMAY_Id && a.FYCC_Id == c.FYCC_Id && d.ASMAY_Id == enq.ASMAY_Id && d.MI_Id == enq.MI_Id && d.FMG_Id == e.FMG_Id && g.FMH_Flag == "T" && e.FMG_CompulsoryFlag == "T" && c.ASMCL_Id == enq.ASMCL_Id)
                                       select new FeeAmountEntryDMO
                                       {
                                           FMA_Id = d.FMA_Id,
                                           FMA_Amount = d.FMA_Amount
                                       }
            ).FirstOrDefault();

                try
                {
                    // string ids = enq.ftiidss;

                    using (var cmd1 = _feecontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd1.CommandText = "Admission_Transport_Split_Payment_Registration";
                        cmd1.CommandType = CommandType.StoredProcedure;

                        cmd1.Parameters.Add(new SqlParameter("@MI_Id",
                         SqlDbType.BigInt)
                        {
                            Value = enq.MI_Id
                        });

                        cmd1.Parameters.Add(new SqlParameter("@Asmay_Id",
                        SqlDbType.BigInt)
                        {
                            Value = enq.ASMAY_Id
                        });

                        cmd1.Parameters.Add(new SqlParameter("@Amst_Id",
                        SqlDbType.VarChar)
                        {
                            Value = enq.AMST_Id
                        });



                        if (cmd1.Connection.State != ConnectionState.Open)
                            cmd1.Connection.Open();

                        try
                        {
                            using (var dataReader = cmd1.ExecuteReader())
                            {
                                while (dataReader.Read())
                                {
                                    result.Add(new FeeSlplitOnlinePayment
                                    {
                                        name = "splitId" + autoinc.ToString(),
                                        merchantId = dataReader["FPGD_MerchantId"].ToString(),
                                        value = dataReader["balance"].ToString(),
                                        commission = "0",
                                        description = "Online Payment",
                                    });

                                    autoinc = autoinc + 1;
                                }
                            }
                        }

                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


                if (enq.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                {
                    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                    enq.transnumbconfigurationsettingsss.MI_Id = Convert.ToInt64(enq.MI_Id);
                    enq.transnumbconfigurationsettingsss.ASMAY_Id = Convert.ToInt64(enq.ASMAY_Id);
                    PaymentDetailsDto.trans_id = a.GenerateNumber(enq.transnumbconfigurationsettingsss);
                }

                if (FeeAmountresult != null)
                {


                    PaymentDetailsDto.Seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";

                    foreach (FeeSlplitOnlinePayment x in result)
                    {
                        totpayableamount = totpayableamount + Convert.ToInt32(x.value);
                    }

                    var item = new
                    {
                        paymentParts = result
                    };

                    string payinfo = JsonConvert.SerializeObject(item);

                    PaymentDetailsDto.productinfo = payinfo;
                    PaymentDetailsDto.amount = Convert.ToDecimal(totpayableamount);
                    PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().IVRMOP_MERCHANT_KEY;
                    PaymentDetailsDto.firstname = enq.AMST_FirstName;


                    PaymentDetailsDto.email = enq.AMST_emailId;

                    PaymentDetailsDto.SaltKey = paymentdetails.FirstOrDefault().IVRMOP_SALT;
                    PaymentDetailsDto.payu_URL = paymentdetails.FirstOrDefault().IVRMOP_BASE_URL;
                    PaymentDetailsDto.phone = Convert.ToInt64(enq.AMST_MobileNo);
                    PaymentDetailsDto.udf1 = Convert.ToString(enq.ASMAY_Id);
                    PaymentDetailsDto.udf2 = Convert.ToString(enq.AMST_Id);
                    PaymentDetailsDto.udf3 = enq.MI_Id.ToString();
                    PaymentDetailsDto.udf4 = enq.ASMCL_Id.ToString();
                    PaymentDetailsDto.udf5 = enq.ASMAY_Id.ToString();
                    PaymentDetailsDto.udf6 = enq.ASMCL_Id.ToString();
                    // PaymentDetailsDto.transaction_response_url = "";
                    PaymentDetailsDto.transaction_response_url = "http://localhost:57606/api/StudentRoueLocationUpdate/paymentresponse/";
                    PaymentDetailsDto.status = "success";
                    PaymentDetailsDto.service_provider = "payu_paisa";

                    PaymentDetailsDto.PaymentDetailsList = pay.OnlinePayment(PaymentDetailsDto);



                    //FeePaymentDetailsDMO feepaydet = new FeePaymentDetailsDMO();
                    //feepaydet.MI_Id = Convert.ToInt64(enq.MI_Id);
                    //feepaydet.ASMAY_ID = Convert.ToInt64(enq.ASMAY_Id);

                    //feepaydet.FTCU_Id = 1;
                    //feepaydet.FYP_Receipt_No = PaymentDetailsDto.trans_id;
                    //feepaydet.FYP_Bank_Name = "";
                    //feepaydet.FYP_Bank_Or_Cash = "O";
                    //feepaydet.FYP_DD_Cheque_No = "";
                    //feepaydet.FYP_DD_Cheque_Date = DateTime.Now;
                    //feepaydet.FYP_Date = DateTime.Now;
                    //feepaydet.FYP_Tot_Amount = PaymentDetailsDto.amount;
                    //feepaydet.FYP_Tot_Waived_Amt = 0;
                    //feepaydet.FYP_Tot_Fine_Amt = 0;
                    //feepaydet.FYP_Tot_Concession_Amt = 0;
                    //feepaydet.FYP_Remarks = "Transport Registration";
                    //feepaydet.FYP_Chq_Bounce = "CL";
                    //feepaydet.DOE = DateTime.Now;
                    //feepaydet.CreatedDate = DateTime.Now;
                    //feepaydet.UpdatedDate = DateTime.Now;
                    //feepaydet.user_id = Convert.ToInt64(enq.Id);
                    //feepaydet.fyp_transaction_id = PaymentDetailsDto.trans_id;
                    //feepaydet.FYP_OnlineChallanStatusFlag = "Payment Initiated";
                    //feepaydet.FYP_PaymentReference_Id = "";

                    //_feecontext.FeePaymentDetailsDMO.Add(feepaydet);
                    //_feecontext.SaveChanges();



                    Fee_M_Online_TransactionDMO onlinemtrans = new Fee_M_Online_TransactionDMO();

                    onlinemtrans.FMOT_Trans_Id = PaymentDetailsDto.trans_id;
                    onlinemtrans.FMOT_Amount = PaymentDetailsDto.amount;
                    onlinemtrans.FMOT_Date = DateTime.Now;
                    onlinemtrans.FMOT_Flag = "T";
                    onlinemtrans.PASR_Id = 0;
                    onlinemtrans.AMST_Id = enq.AMST_Id;
                    onlinemtrans.FMOT_Receipt_no = "";
                    onlinemtrans.ASMAY_ID = Convert.ToInt64(enq.ASMAY_Id);
                    onlinemtrans.MI_Id = Convert.ToInt64(enq.MI_Id);

                    _feecontext.Fee_M_Online_TransactionDMO.Add(onlinemtrans);


                    Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
                    onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
                    onlinettrans.FMA_Id = FeeAmountresult.FMA_Id;
                    onlinettrans.FTOT_Amount = PaymentDetailsDto.amount;
                    onlinettrans.FTOT_Created_date = DateTime.Now;
                    onlinettrans.FTOT_Updated_date = DateTime.Now;
                    onlinettrans.FTOT_Concession = 0;
                    onlinettrans.FTOT_Fine = 0;

                    _feecontext.Fee_T_Online_TransactionDMO.Add(onlinettrans);


                    var contactexisttransaction = 0;

                    using (var dbCtxTxn = _feecontext.Database.BeginTransaction())
                    {
                        try
                        {
                            contactexisttransaction = _feecontext.SaveChanges();
                            dbCtxTxn.Commit();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                            dbCtxTxn.Rollback();
                        }
                    }








                    PaymentDetailsDto.paymentdetails = "True";

                }
                else
                {
                    PaymentDetailsDto.paymentdetails = "false";
                }
            }
            catch (Exception e)
            {

            }

            return PaymentDetailsDto.PaymentDetailsList;

        }
        public PaymentDetails payuresponse(PaymentDetails response)
        {
            PaymentDetails dto = new PaymentDetails();
            StudentApplicationDTO stu = new StudentApplicationDTO();
            FeeStudentTransactionDTO data = new FeeStudentTransactionDTO();
            //   FeePaymentDetailsDMO feeypayment = Mapper.Map<FeePaymentDetailsDMO>(response);
            if (response.status == "success")
            {
                string termid = "1";


                //   var orderid = (from a in _buspasscontext.AcademicYearDMO
                //                  where (a.ASMAY_Id ==Convert.ToInt64(response.udf5) && a.MI_Id ==Convert.ToInt64(response.udf3))
                //                  select new StudentBuspassFormDTO
                //                  {
                //                      year_Order = a.ASMAY_Order + 1
                //                  }
                //).ToList().ToArray();

                //   var asmay_Id = (from a in _buspasscontext.AcademicYearDMO
                //                   where (a.ASMAY_Order == orderid[0].year_Order && a.MI_Id == Convert.ToInt64(response.udf3))
                //                   select new StudentBuspassFormDTO
                //                   {
                //                       year_Id = a.ASMAY_Id
                //                   }
                //    ).ToList().ToArray();
                //   string yutherid = asmay_Id.FirstOrDefault().year_Id.ToString();

                stu.MI_Id = Convert.ToInt64(response.udf3);
                stu.PASR_MobileNo = response.phone;
                stu.pasR_Id = Convert.ToInt64(response.udf2);
                stu.PASR_emailId = response.email;
                stu.ASMAY_Id = Convert.ToInt64(response.udf5);


                data.MI_Id = Convert.ToInt64(response.udf3);
                data.ASMCL_ID = Convert.ToInt64(response.udf4);
                data.ASMAY_Id = Convert.ToInt64(response.udf5);

                try
                {
                    var mgs = insertdatainfeetables(response.udf3, termid, response.udf2, response.udf4, response.amount, response.txnid, response.mihpayid, response.udf5);
                    if (Convert.ToInt32(mgs) > 0)
                    {

                        List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
                        mstConfig = _db.mstConfig.Where(d => d.MI_Id.Equals(stu.MI_Id) && d.ASMAY_Id.Equals(stu.ASMAY_Id)).ToList();

                        var studDet = _db.Adm_Student_Transport_ApplicationDMO.Where(t => t.MI_Id == data.MI_Id && t.ASTA_FutureAY == data.ASMAY_Id && t.AMST_Id == stu.pasR_Id).ToList();

                        SMS sms = new SMS(_db);
                        string s = sms.sendSms(data.MI_Id, Convert.ToInt64(studDet.FirstOrDefault().ASTA_FatherMobileNo), "TRANSPORT_REG", stu.pasR_Id).Result;

                        var studDett = _db.Adm_M_Student.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == stu.pasR_Id).ToList();

                        Email Email = new Email(_db);
                        string m = Email.sendmail(data.MI_Id, studDett.FirstOrDefault().AMST_emailId, "TRANSPORT_REG", stu.pasR_Id);
                    }
                }
                catch (Exception ex)
                {
                    Buspasss.LogError("Error in " + ex.InnerException);
                    Buspasss.LogInformation("Buspass Exception" + ex.Message);
                }






            }
            else
            {
                dto.status = response.status;
            }

            return response;
        }

        public string get_grp_reptno(FeeStudentTransactionDTO data)
        {
            try
            {

                List<FeeMasterConfigurationDMO> feemasnum = new List<FeeMasterConfigurationDMO>();
                feemasnum = _db.FeeMasterConfigurationDMO.Where(t => t.MI_Id == data.MI_Id).ToList();
                data.feeconfiglist = feemasnum.ToArray();

                List<long> temparr = new List<long>();
                for (int i = 0; i < feemasnum.Count; i++)
                {
                    data.auto_receipt_flag = feemasnum[i].FMC_AutoReceiptFeeGroupFlag;
                }

                if (data.auto_receipt_flag == 1)
                {

                    var FeeAmountresult = (from a in _feecontext.feeYCC

                                           from c in _feecontext.feeYCCC
                                           from d in _feecontext.FeeAmountEntryDMO

                                           from g in _feecontext.FeeHeadDMO
                                           from e in _feecontext.FeeGroupDMO
                                           where (d.FMH_Id == g.FMH_Id && d.FMCC_Id == a.FMCC_Id && a.ASMAY_Id == d.ASMAY_Id && a.FYCC_Id == c.FYCC_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.FMG_Id == e.FMG_Id && g.FMH_Flag == "T" && e.FMG_CompulsoryFlag == "T" && c.ASMCL_Id == data.ASMCL_ID)
                                           select new FeeStudentTransactionDTO
                                           {
                                               FMH_Id = d.FMH_Id,
                                           }
           ).ToList();

                    List<long> HeadId = new List<long>();
                    foreach (var item in FeeAmountresult)
                    {
                        HeadId.Add(item.FMH_Id);
                    }

                    List<FeeStudentTransactionDTO> grps = new List<FeeStudentTransactionDTO>();
                    grps = (from b in _feecontext.FeeYearlygroupHeadMappingDMO

                            where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FYGHM_ActiveFlag == "1" && HeadId.Contains(b.FMH_Id))

                            select new FeeStudentTransactionDTO
                            {
                                FMG_Id = b.FMG_Id
                            }
                           ).Distinct().ToList();

                    List<long> grpid = new List<long>();
                    string groupidss = "0";
                    foreach (var item in grps)
                    {
                        grpid.Add(item.FMG_Id);
                    }

                    for (int r = 0; r < grpid.Count(); r++)
                    {
                        groupidss = groupidss + ',' + grpid[r];
                    }

                    var final_rept_no = "";
                    List<FeeStudentTransactionDTO> list_all = new List<FeeStudentTransactionDTO>();
                    List<FeeStudentTransactionDTO> list_repts = new List<FeeStudentTransactionDTO>();

                    list_all = (from b in _feecontext.Fee_Groupwise_AutoReceiptDMO
                                from c in _feecontext.Fee_Groupwise_AutoReceipt_GroupsDMO
                                where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(c.FMG_Id) && b.FGAR_Id == c.FGAR_Id)

                                select new FeeStudentTransactionDTO
                                {
                                    FGAR_PrefixName = b.FGAR_PrefixName,
                                    FGAR_SuffixName = b.FGAR_SuffixName,
                                    //FGAR_Name = b.FGAR_Name,
                                    //FMG_Id = c.FMG_Id
                                }
                         ).Distinct().ToList();

                    data.grp_count = list_all.Count();

                    if (data.grp_count == 1)
                    {


                        using (var cmd = _feecontext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "receiptnogeneration";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@mi_id",
                                SqlDbType.VarChar, 100)
                            {
                                Value = data.MI_Id
                            });

                            cmd.Parameters.Add(new SqlParameter("@asmayid",
                               SqlDbType.NVarChar, 100)
                            {
                                Value = data.ASMAY_Id
                            });
                            cmd.Parameters.Add(new SqlParameter("@fmgid",
                           SqlDbType.NVarChar, 100)
                            {
                                Value = groupidss
                            });

                            cmd.Parameters.Add(new SqlParameter("@receiptno",
                SqlDbType.NVarChar, 500)
                            {
                                Direction = ParameterDirection.Output
                            });

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var data1 = cmd.ExecuteNonQuery();

                            data.FYP_Receipt_No = cmd.Parameters["@receiptno"].Value.ToString();

                        }

                        //data.auto_FYP_Receipt_No = final_rept_no;

                        //data.FYP_Receipt_No = final_rept_no;
                    }
                }
                else
                {
                    data.FYP_Receipt_No = "0";
                }

                //else if (data.automanualreceiptno == "Auto")
                //{
                //    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                //    data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                //    data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                //    data.FYP_Receipt_No = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                //}

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data.FYP_Receipt_No;
        }
        public StudentBuspassFormDTO paynow(StudentBuspassFormDTO dt)
        {

            try
            {
                var alreadyExistEmailId = _buspasscontext.Adm_Student_Transport_ApplicationDMO.Where(d => d.ASTA_Id == dt.ASTA_Id).ToList();

                var studentdetails = _buspasscontext.Adm_M_Student.Where(d => d.AMST_Id == dt.PASR_Id).ToList();


                dt.ASMCL_Id = alreadyExistEmailId.FirstOrDefault().ASTA_FutureClass;
                dt.ASMAY_Id = alreadyExistEmailId.FirstOrDefault().ASTA_FutureAY;
                dt.AMST_FirstName = studentdetails.FirstOrDefault().AMST_FirstName;
                dt.AMST_emailId = studentdetails.FirstOrDefault().AMST_emailId;
                dt.AMST_MobileNo = studentdetails.FirstOrDefault().AMST_MobileNo;
                dt.AMST_Id = dt.PASR_Id;



                //dt.payementcheck = _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYPPA_Type == "T" && t.PASA_Id == dt.PASR_Id).Count();
                dt.payementcheck = (from a in _feecontext.FeeAmountEntryDMO
                                    from b in _feecontext.FeeTransactionPaymentDMO
                                    from d in _feecontext.FeeGroupDMO
                                    from f in _feecontext.FeeHeadDMO
                                    from g in _feecontext.feeYCCC
                                    from h in _feecontext.feeYCC
                                    from i in _feecontext.Fee_Y_Payment_School_StudentDMO
                                    where (a.FMG_Id == d.FMG_Id && a.FMH_Id == f.FMH_Id && a.FMCC_Id == h.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMA_Id == a.FMA_Id && h.ASMAY_Id == a.ASMAY_Id && i.FYP_Id == b.FYP_Id && f.FMH_Flag == "T" && d.FMG_CompulsoryFlag == "T" && i.AMST_Id == dt.PASR_Id)
                                    select new FeeAmountEntryDMO
                                    {
                                        FMA_Id = a.FMA_Id,
                                        FMA_Amount = a.FMA_Amount
                                    }
          ).Count();

                if (dt.payementcheck == 0)
                {
                    dt.paydet = paymentPart(dt);
                }


            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }

            return dt;
        }

        public string insertdatainfeetables(string miid, string termid, string studentid, string classid, decimal amount, string transid, long refid, string yearid)
        {
            var contactexisttransaction = 0;
            try
            {
                var confirmstatus = _feecontext.Database.ExecuteSqlCommand("Transport_Application_online_Registration_Mapping @p0,@p1,@p2,@p3", miid, yearid, studentid, classid);

                string recnoen = "";
                var fetchfmhotid = (from a in _feecontext.Fee_M_Online_TransactionDMO
                                    where (a.AMST_Id == Convert.ToInt64(studentid) && a.FMOT_Trans_Id == transid.ToString())
                                    select new FeeStudentTransactionDTO
                                    {
                                        FMHOT_Id = a.FMOT_Id,
                                        FMA_Amount = a.FMOT_Amount
                                    }).ToArray();

                for (int r = 0; r < fetchfmhotid.Count(); r++)
                {
                    var fethchfmgids = (from a in _feecontext.Fee_T_Online_TransactionDMO
                                        from b in _feecontext.FeeAmountEntryDMO
                                        from c in _feecontext.Fee_OnlinePayment_Mapping
                                        where (c.FMH_Id == b.FMH_Id && c.fti_id == b.FTI_Id && c.fmg_id == b.FMG_Id && a.FMA_Id == b.FMA_Id && a.FMOT_Id == fetchfmhotid[r].FMHOT_Id && b.MI_Id == Convert.ToInt64(miid) && b.ASMAY_Id == Convert.ToInt64(yearid))
                                        select new FeeStudentTransactionDTO
                                        {
                                            FMG_Id = b.FMG_Id
                                        }).Distinct().ToArray();

                    List<long> grpid = new List<long>();
                    string groupidss = "0";
                    foreach (var item in fethchfmgids)
                    {
                        grpid.Add(item.FMG_Id);
                    }

                    for (int d = 0; d < fethchfmgids.Count(); d++)
                    {
                        groupidss = groupidss + ',' + fethchfmgids[d].FMG_Id;
                    }


                    List<FeeStudentTransactionDTO> list_all = new List<FeeStudentTransactionDTO>();
                    List<FeeStudentTransactionDTO> list_repts = new List<FeeStudentTransactionDTO>();
                    list_all = (from b in _feecontext.Fee_Groupwise_AutoReceiptDMO
                                from c in _feecontext.Fee_Groupwise_AutoReceipt_GroupsDMO
                                where (b.MI_Id == Convert.ToInt64(miid) && b.ASMAY_Id == Convert.ToInt64(yearid) && grpid.Contains(c.FMG_Id) && b.FGAR_Id == c.FGAR_Id)

                                select new FeeStudentTransactionDTO
                                {
                                    FGAR_PrefixName = b.FGAR_PrefixName,
                                    FGAR_SuffixName = b.FGAR_SuffixName,
                                    FGAR_Id = c.FGAR_Id,
                                }
                         ).Distinct().ToList();

                    using (var cmd = _feecontext.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.CommandText = "receiptnogeneration";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@mi_id",
                            SqlDbType.VarChar, 100)
                        {
                            Value = Convert.ToInt32(miid)
                        });

                        cmd.Parameters.Add(new SqlParameter("@asmayid",
                           SqlDbType.NVarChar, 100)
                        {
                            Value = Convert.ToInt32(yearid)
                        });
                        cmd.Parameters.Add(new SqlParameter("@fmgid",
                       SqlDbType.NVarChar, 100)
                        {
                            Value = groupidss
                        });

                        cmd.Parameters.Add(new SqlParameter("@receiptno",
            SqlDbType.NVarChar, 500)
                        {
                            Direction = ParameterDirection.Output
                        });

                        if (cmd.Connection.State != ConnectionState.Open)
                            cmd.Connection.Open();

                        var data1 = cmd.ExecuteNonQuery();

                        recnoen = cmd.Parameters["@receiptno"].Value.ToString();

                        var groupwisefmaids = (from a in _feecontext.Fee_T_Online_TransactionDMO
                                               from c in _feecontext.Fee_M_Online_TransactionDMO
                                               where (a.FMOT_Id == c.FMOT_Id && a.FMOT_Id == fetchfmhotid[r].FMHOT_Id && c.AMST_Id == Convert.ToInt64(studentid))
                                               select new FeeStudentTransactionDTO
                                               {
                                                   FMA_Id = a.FMA_Id,
                                                   FSS_ToBePaid = Convert.ToInt64(a.FTOT_Amount)
                                               }
                             ).ToArray();

                        FeePaymentDetailsDMO onlinemtrans = new FeePaymentDetailsDMO();

                        onlinemtrans.ASMAY_ID = Convert.ToInt64(yearid);
                        onlinemtrans.FTCU_Id = 1;
                        onlinemtrans.FYP_Receipt_No = recnoen;
                        onlinemtrans.FYP_Bank_Name = "";
                        onlinemtrans.FYP_Bank_Or_Cash = "O";
                        onlinemtrans.FYP_DD_Cheque_No = "";
                        onlinemtrans.FYP_DD_Cheque_Date = DateTime.Now;

                        onlinemtrans.FYP_Date = DateTime.Now;
                        onlinemtrans.FYP_Tot_Amount = Convert.ToDecimal(fetchfmhotid[r].FMA_Amount);
                        onlinemtrans.FYP_Tot_Waived_Amt = 0;
                        onlinemtrans.FYP_Tot_Fine_Amt = 0;
                        onlinemtrans.FYP_Tot_Concession_Amt = 0;
                        onlinemtrans.FYP_Remarks = "Online Transport Payment";
                        // onlinemtrans.IVRMSTAUL_ID = Convert.ToInt64(studentid);

                        onlinemtrans.FYP_Chq_Bounce = "CL";
                        onlinemtrans.MI_Id = Convert.ToInt64(miid);
                        onlinemtrans.DOE = DateTime.Now;
                        onlinemtrans.CreatedDate = DateTime.Now;
                        onlinemtrans.UpdatedDate = DateTime.Now;
                        onlinemtrans.user_id = Convert.ToInt64(studentid);
                        onlinemtrans.fyp_transaction_id = transid;

                        onlinemtrans.FYP_OnlineChallanStatusFlag = "Sucessfull";
                        onlinemtrans.FYP_PaymentReference_Id = refid.ToString();
                        onlinemtrans.FYP_ChallanNo = "";

                        _feecontext.FeePaymentDetailsDMO.Add(onlinemtrans);

                        Fee_Y_Payment_School_StudentDMO onlinestuapp = new Fee_Y_Payment_School_StudentDMO();

                        onlinestuapp.FYP_Id = onlinemtrans.FYP_Id;
                        onlinestuapp.AMST_Id = Convert.ToInt64(studentid);
                        onlinestuapp.ASMAY_Id = Convert.ToInt64(yearid);
                        onlinestuapp.FTP_TotalPaidAmount = Convert.ToDecimal(fetchfmhotid[r].FMA_Amount);
                        onlinestuapp.FTP_TotalConcessionAmount = 0;
                        onlinestuapp.FTP_TotalFineAmount = 0;
                        onlinestuapp.FTP_TotalWaivedAmount = 0;

                        _feecontext.Fee_Y_Payment_School_StudentDMO.Add(onlinestuapp);

                        //var obj_status_stftrans = _feecontext.Adm_Student_Transport_ApplicationDMO.Where(t => t.MI_Id == Convert.ToInt64(miid) && t.ASTA_FutureAY == Convert.ToInt64(yearid) && t.AMST_Id == Convert.ToInt64(studentid)).FirstOrDefault();
                        //obj_status_stftrans.ASTA_PaymentDate = DateTime.Now;
                        //obj_status_stftrans.ASTA_Amount = Convert.ToDecimal(fetchfmhotid[r].FMA_Amount);
                        //obj_status_stftrans.ASTA_ReceiptNo = recnoen;
                        //_feecontext.Update(obj_status_stftrans);



                        for (int s = 0; s < groupwisefmaids.Count(); s++)
                        {
                            FeeTransactionPaymentDMO onlinettrans = new FeeTransactionPaymentDMO();
                            onlinettrans.FYP_Id = onlinemtrans.FYP_Id;
                            onlinettrans.FMA_Id = groupwisefmaids[s].FMA_Id;
                            onlinettrans.FTP_Paid_Amt = groupwisefmaids[s].FSS_ToBePaid;
                            onlinettrans.FTP_Fine_Amt = 0;
                            onlinettrans.FTP_Concession_Amt = 0;
                            onlinettrans.FTP_Waived_Amt = 0;
                            onlinettrans.ftp_remarks = "Online Transport Payment";

                            _feecontext.FeeTransactionPaymentDMO.Add(onlinettrans);

                            var obj_status_stf = _feecontext.FeeStudentTransactionDMO.Where(t => t.MI_Id == Convert.ToInt64(miid) && t.ASMAY_Id == Convert.ToInt64(yearid) && t.AMST_Id == Convert.ToInt64(studentid) && t.FMA_Id == groupwisefmaids[s].FMA_Id && t.FSS_ActiveFlag == true).FirstOrDefault();

                            obj_status_stf.FSS_PaidAmount = obj_status_stf.FSS_PaidAmount + groupwisefmaids[s].FSS_ToBePaid;

                            if (obj_status_stf.FSS_NetAmount != 0)
                            {
                                obj_status_stf.FSS_ToBePaid = obj_status_stf.FSS_ToBePaid - groupwisefmaids[s].FSS_ToBePaid;
                            }
                            else
                            {
                                obj_status_stf.FSS_ToBePaid = 0;
                            }

                            _feecontext.Update(obj_status_stf);



                        }

                        groupidss = "0";
                    }

                }

                using (var dbCtxTxn = _feecontext.Database.BeginTransaction())
                {
                    try
                    {
                        contactexisttransaction = _feecontext.SaveChanges();
                        dbCtxTxn.Commit();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        dbCtxTxn.Rollback();
                    }
                }

                var confirmstatusss = _feecontext.Database.ExecuteSqlCommand("Transport_Application_online_Registration_update @p0,@p1,@p2,@p3,@p4", miid, yearid, studentid, classid, recnoen);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Buspasss.LogError("Error in " + ex.InnerException);
                Buspasss.LogInformation("Buspass Exception" + ex.Message);
            }

            return contactexisttransaction.ToString();
        }

        public StudentBuspassFormDTO searchfilter(StudentBuspassFormDTO data)
        {
            try
            {


                List<StudentBuspassFormDTO> result = new List<StudentBuspassFormDTO>();
                using (var cmd = _feecontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TR_RUOTEUPDATE_NAME_SEARCH";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@Mi_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });

                    cmd.Parameters.Add(new SqlParameter("@searchtext",
                                 SqlDbType.VarChar)
                    {
                        Value = data.searchfilter
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                                          SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });



                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();

                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new StudentBuspassFormDTO
                                {
                                    AMST_Id = Convert.ToInt64(dataReader["AMST_Id"].ToString()),
                                    AMST_FirstName = dataReader["AMST_FirstName"].ToString(),
                                  

                                });
                            }
                        }
                        data.fillstudent = result.ToArray();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }


           //     data.fillstudent = (from a in _buspasscontext.Adm_M_Student
           //                         from b in _buspasscontext.Adm_Student_Transport_ApplicationDMO
           //                         where (a.AMST_Id == b.AMST_Id && a.MI_Id == data.MI_Id && b.ASTA_FutureAY == data.ASMAY_Id && a.AMST_SOL == "S"  && a.AMST_ActiveFlag == 1 && ((a.AMST_FirstName.Trim() + ' ' + a.AMST_MiddleName.Trim() + ' ' + a.AMST_LastName.Trim()).Contains(data.searchfilter) || (a.AMST_FirstName.Trim() + a.AMST_MiddleName.Trim() + ' ' + a.AMST_LastName.Trim()).Contains(data.searchfilter) || a.AMST_FirstName.Contains(data.searchfilter) || a.AMST_MiddleName.Contains(data.searchfilter) || a.AMST_LastName.Contains(data.searchfilter)))
           //                         select new StudentBuspassFormDTO
           //                         {
           //                             AMST_Id = a.AMST_Id,
           //                             AMST_FirstName = a.AMST_FirstName + ' ' + a.AMST_MiddleName + ' ' + a.AMST_LastName,
           //                         }
           //).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}







