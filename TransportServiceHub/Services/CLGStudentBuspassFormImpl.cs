using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model.com.vapstech.Transport;
using DomainModel.Model.com.vapstech.College.Admission;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;

using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Dynamic;
using CommonLibrary;
using PreadmissionDTOs.com.vaps.Fees;
using Newtonsoft.Json;
using DomainModel.Model.com.vapstech.Fee;
using Payment = CommonLibrary.Payment;

namespace TransportServiceHub.Services
{
    public class CLGStudentBuspassFormImpl : Interfaces.CLGStudentBuspassFormInterface
    {
        public TransportContext _buspasscontext;
        ILogger<CLGStudentBuspassFormImpl> _areaimpl;
        public DomainModelMsSqlServerContext _db;


        // parameterized constructor
        public CLGStudentBuspassFormImpl(ILogger<CLGStudentBuspassFormImpl> areaimpl, TransportContext context, DomainModelMsSqlServerContext db)
        {

            _areaimpl = areaimpl;
            _buspasscontext = context;
            _db = db;

        }

        public CLGStudentBuspassFormDTO academicload(CLGStudentBuspassFormDTO data)
        {
            try
            {
                var aaa = _buspasscontext.Adm_Master_College_StudentDMO.Single(f => f.AMCST_Id == data.AMCST_Id).ASMAY_Id;

                List<AcademicYear> acayr = new List<AcademicYear>();
                var acayrorder = _buspasscontext.AcademicYearDMO.Single(r => r.MI_Id == data.MI_Id && r.Is_Active == true && r.ASMAY_Id == data.ASMAY_Id).ASMAY_Order;

                var acayrorder1 = _buspasscontext.AcademicYearDMO.Single(r => r.MI_Id == data.MI_Id && r.Is_Active == true && r.ASMAY_Id == aaa).ASMAY_Order;

                var appliedlist = _buspasscontext.CLGAdm_Std_Transport_ApplicationDMO.Where(d => d.MI_Id == data.MI_Id && d.AMCST_Id == data.AMCST_Id && d.ASTACO_ActiveFlag == true).ToList();

                if (appliedlist.Count > 0)
                {
                    var appliedcurrlist = _buspasscontext.CLGAdm_Std_Transport_ApplicationDMO.Where(d => d.MI_Id == data.MI_Id && d.AMCST_Id == data.AMCST_Id && d.ASTACO_ActiveFlag == true && d.ASTACO_ForAY == data.ASMAY_Id).ToList();

                    if (appliedcurrlist.Count > 0)
                    {
                        acayr = _buspasscontext.AcademicYearDMO.Where(r => r.MI_Id == data.MI_Id && r.Is_Active == true && r.ASMAY_Order >= acayrorder && r.ASMAY_Order >= acayrorder1).ToList();
                        data.fillyear = acayr.OrderByDescending(r => r.ASMAY_Order).ToArray();
                    }
                    else
                    {
                        acayr = _buspasscontext.AcademicYearDMO.Where(r => r.MI_Id == data.MI_Id && r.Is_Active == true && r.ASMAY_Order >= acayrorder && r.ASMAY_Order >= acayrorder1).ToList();
                        data.fillyear = acayr.OrderByDescending(r => r.ASMAY_Order).ToArray();
                    }
                }
                else
                {
                    acayr = _buspasscontext.AcademicYearDMO.Where(r => r.MI_Id == data.MI_Id && r.Is_Active == true && r.ASMAY_Order >= acayrorder && r.ASMAY_Order >= acayrorder1).ToList();
                    data.fillyear = acayr.OrderByDescending(r => r.ASMAY_Order).ToArray();
                }

                data.locationlist = (from a in _buspasscontext.Route_Location
                                     from b in _buspasscontext.MasterRouteDMO
                                     from c in _buspasscontext.MasterLocationDMO
                                     where (a.TRMR_Id == b.TRMR_Id && a.TRML_Id == c.TRML_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id && a.TRMRL_ActiveFlag == true && b.TRMR_ActiveFlg == true && c.TRML_ActiveFlg == true)
                                     select new CLGStudentBuspassFormDTO
                                     {
                                         TRML_Id = c.TRML_Id,
                                         TRML_LocationName = c.TRML_LocationName
                                     }
                ).ToList().ToArray();
            }
            catch (Exception ex)
            {

                throw ex;
            }


            return data;
        }
        public CLGStudentBuspassFormDTO getloaddata(CLGStudentBuspassFormDTO data)
        {
            try
            {
                string rolename = _buspasscontext.IVRM_Role_Type.FirstOrDefault(t => t.IVRMRT_Id == data.roleId).IVRMRT_Role;



                //Praveen commented
                //var Acdemic_preadmission = _buspasscontext.AcademicYearDMO.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();
                //End Comment

                //Praveen ADDED
                var Acdemic_preadmission = _buspasscontext.AcademicYearDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.Is_Active == true && t.MI_Id == data.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();
                //End
                data.transportyear = Acdemic_preadmission;



                var studentcurrentyear = (from a in _buspasscontext.Adm_College_Yearly_StudentDMO
                                          where (a.AMCST_Id == data.AMCST_Id)
                                          select a
             ).ToList().OrderByDescending(d => d.ACYST_Id).ToArray();

                if (studentcurrentyear.Length > 0)
                {
                    if (studentcurrentyear.Length == 1)
                    {
                        if (studentcurrentyear.FirstOrDefault().ASMAY_Id == data.transportyear)
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
                        if (studentcurrentyear.FirstOrDefault().ASMAY_Id != data.transportyear)
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
                    var studentadmityear = (from a in _buspasscontext.Adm_Master_College_StudentDMO
                                            where (a.AMCST_Id == data.AMCST_Id)
                                            select a
               ).ToList().ToArray();

                    data.studentaccyear = studentadmityear.FirstOrDefault().ASMAY_Id;

                }
                data.regularnew = (from a in _buspasscontext.FeeGroupDMO
                                   from b in _buspasscontext.FeeYearlygroupHeadMappingDMO
                                   from c in _buspasscontext.Fee_College_Student_StatusDMO
                                   from d in _buspasscontext.FeeHeadDMO
                                   where (b.FMG_Id == a.FMG_Id && b.FMH_Id == d.FMH_Id && c.FMG_Id == b.FMG_Id && c.FMH_Id == b.FMH_Id && c.FMG_Id == a.FMG_Id && c.FMH_Id == d.FMH_Id && c.ASMAY_Id == b.ASMAY_Id && d.FMH_Flag == "T" && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.studentaccyear && c.FCSS_CurrentYrCharges > 0 && c.AMCST_Id == data.AMCST_Id)
                                   select new CLGStudentBuspassFormDTO
                                   {
                                       AMCST_Id = c.AMCST_Id

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


                //Praveen Commented
                //var trnsconfig = (from a in _buspasscontext.TRMasterconfigurationDMO
                //                  where (a.MI_ID == data.MI_Id && a.ASMAY_ID == data.transportyear)
                //                  select a).ToList();
                //Comment End


                var trnsconfig = (from a in _buspasscontext.AcademicYearDMO
                                  where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.transportyear)
                                  select a).ToList();

                data.transappfillAdmissionNew = false;
                data.transappfillTrNew = false;
                data.transappfillTrRegular = false;


                if (trnsconfig.FirstOrDefault().ASMAY_NewFlg == true)
                {
                    if (data.studentTrstatus == "TrNew")
                    {
                        data.transappfillTrNew = true;
                    }
                    else
                    {
                        data.transappfillTrNew = false;
                    }
                }


                if (trnsconfig.FirstOrDefault().ASMAY_ReggularFlg == true)
                {
                    if (data.studentTrstatus == "TrRegular")
                    {
                        data.transappfillTrRegular = true;
                    }
                    else
                    {
                        data.transappfillTrRegular = false;
                    }
                }


                if (trnsconfig.FirstOrDefault().ASMAY_NewAdmissionFlg == true)
                {
                    if (data.studentstatus == "AdmissionNew")
                    {
                        data.transappfillAdmissionNew = true;
                    }
                    else
                    {
                        data.transappfillAdmissionNew = false;
                    }

                }








                if (data.transappfillAdmissionNew == true || data.transappfillTrNew == true || data.transappfillTrRegular == true)
                {
                    data.trnsportcutoffdate = "True";
                }
                else
                {
                    data.trnsportcutoffdate = "False";
                }



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
                //                                   select new CLGStudentBuspassFormDTO
                //                                   {
                //                                       AMST_Id = i.AMST_Id,
                //                                       ASTA_Id=j.ASTA_Id

                //                                   }
                //).ToArray();


                data.prospectusPaymentlist = (from j in _buspasscontext.CLGAdm_Std_Transport_ApplicationDMO
                                              where (j.AMCST_Id == data.AMCST_Id && j.ASTACO_ForAY == data.transportyear && j.ASTACO_Amount > 0)
                                              select new CLGStudentBuspassFormDTO
                                              {
                                                  AMCST_Id = j.AMCST_Id,
                                                  ASTACO_Id = j.ASTACO_Id,
                                                  PASTA_Amount = j.ASTACO_Amount

                                              }
 ).ToArray();

                if (rolename == "Student")
                {
                    data.stu_name = (from a in _buspasscontext.Adm_Master_College_StudentDMO
                                     where (a.AMCST_Id == data.AMCST_Id && a.MI_Id == data.MI_Id)
                                     select new CLGStudentBuspassFormDTO
                                     {
                                         AMCST_Id = a.AMCST_Id,
                                         AMCST_FirstName = ((a.AMCST_FirstName == null || a.AMCST_FirstName == "0" ? "" : a.AMCST_FirstName) + " " + (a.AMCST_MiddleName == null || a.AMCST_MiddleName == "0" ? "" : a.AMCST_MiddleName) + " " + (a.AMCST_LastName == null || a.AMCST_LastName == "0" ? "" : a.AMCST_LastName)).Trim()

                                     }
              ).ToList().ToArray();

                    data.routeDetails = (from a in _buspasscontext.CLGAdm_Std_Transport_ApplicationDMO
                                         from d in _buspasscontext.Adm_Master_College_StudentDMO

                                         where (a.AMCST_Id == d.AMCST_Id && d.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && d.AMCST_ActiveFlag == true && d.AMCST_SOL == "S" && a.AMCST_Id == data.AMCST_Id && a.ASTACO_ForAY == data.transportyear)
                                         select new CLGStudentBuspassFormDTO
                                         {
                                             AMCST_Id = a.AMCST_Id,
                                             ASTACO_Id = a.ASTACO_Id,
                                             AMCST_FirstName = ((d.AMCST_FirstName == null || d.AMCST_FirstName == "0" ? "" : d.AMCST_FirstName) + " " + (d.AMCST_MiddleName == null || d.AMCST_MiddleName == "0" ? "" : d.AMCST_MiddleName) + " " + (d.AMCST_LastName == null || d.AMCST_LastName == "0" ? "" : d.AMCST_LastName)).Trim(),
                                             ASTA_PickUp_TRML_Id = a.ASTACO_PickUp_TRML_Id,
                                             TRML_PickLocationName = a.ASTACO_PickUp_TRML_Id != 0 ? _buspasscontext.MasterLocationDMO.Where(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTACO_PickUp_TRML_Id).FirstOrDefault().TRML_LocationName : "--",
                                             ASTA_Landmark = a.ASTACO_Landmark,
                                             ASTA_Phoneoff = a.ASTACO_Phoneoff,
                                             ASTA_PhoneRes = a.ASTACO_PhoneRes,
                                             ASTA_Drop_TRML_Id = a.ASTACO_Drop_TRML_Id,
                                             TRML_DropLocationName = a.ASTACO_Drop_TRML_Id != 0 ? _buspasscontext.MasterLocationDMO.Where(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTACO_Drop_TRML_Id).FirstOrDefault().TRML_LocationName : "--",

                                             TRMR_Idp = a.ASTACO_PickUp_TRMR_Id,
                                             TRMR_PickRouteName = a.ASTACO_PickUp_TRMR_Id != 0 ? _buspasscontext.MasterRouteDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTACO_PickUp_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",

                                             TRMR_Idd = a.ASTACO_Drop_TRMR_Id,
                                             TRMR_DropRouteName = a.ASTACO_Drop_TRMR_Id != 0 ? _buspasscontext.MasterRouteDMO.Where(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTACO_Drop_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",
                                             ASTA_ApplStatus = a.ASTACO_ApplStatus,
                                             ASTACO_ApplicationDate = a.ASTACO_ApplicationDate,
                                             ASMAY_Year = data.ASMAY_Id != 0 ? _buspasscontext.AcademicYearDMO.Where(td => td.MI_Id == data.MI_Id && td.ASMAY_Id == a.ASTACO_ForAY).FirstOrDefault().ASMAY_Year : "--",
                                             ASMAY_Id = a.ASTACO_ForAY
                                         }
                        ).Distinct().ToArray();

                    var regularnewff = (from f in _buspasscontext.CLGAdm_Std_Transport_ApplicationDMO
                                        where (f.AMCST_Id == data.AMCST_Id)
                                        select new CLGStudentBuspassFormDTO
                                        {
                                            classnextid = f.ASTACO_ForAY,
                                            ASTA_ApplStatus = f.ASTACO_ApplStatus
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
                    data.stu_name = (from a in _buspasscontext.Adm_Master_College_StudentDMO
                                     where (a.MI_Id == data.MI_Id)
                                     select new CLGStudentBuspassFormDTO
                                     {
                                         AMCST_Id = a.AMCST_Id,
                                         AMCST_FirstName = ((a.AMCST_FirstName == null || a.AMCST_FirstName == "0" ? "" : a.AMCST_FirstName) + " " + (a.AMCST_MiddleName == null || a.AMCST_MiddleName == "0" ? "" : a.AMCST_MiddleName) + " " + (a.AMCST_LastName == null || a.AMCST_LastName == "0" ? "" : a.AMCST_LastName)).Trim()

                                     }
           ).ToList().ToArray();

                    data.routeDetails = (from a in _buspasscontext.CLGAdm_Std_Transport_ApplicationDMO
                                         from d in _buspasscontext.Adm_Master_College_StudentDMO
                                         where (a.AMCST_Id == d.AMCST_Id && d.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && d.AMCST_ActiveFlag == true && d.AMCST_SOL == "S")
                                         select new CLGStudentBuspassFormDTO
                                         {
                                             AMCST_Id = a.AMCST_Id,
                                             ASTACO_Id = a.ASTACO_Id,
                                             AMCST_FirstName = ((d.AMCST_FirstName == null || d.AMCST_FirstName == "0" ? "" : d.AMCST_FirstName) + " " + (d.AMCST_MiddleName == null || d.AMCST_MiddleName == "0" ? "" : d.AMCST_MiddleName) + " " + (d.AMCST_LastName == null || d.AMCST_LastName == "0" ? "" : d.AMCST_LastName)).Trim(),
                                             ASTA_PickUp_TRML_Id = a.ASTACO_PickUp_TRML_Id,
                                             ASTA_Landmark = a.ASTACO_Landmark,
                                             ASTA_Phoneoff = a.ASTACO_Phoneoff,
                                             ASTA_PhoneRes = a.ASTACO_PhoneRes,
                                             TRML_PickLocationName = a.ASTACO_PickUp_TRML_Id != 0 ? _buspasscontext.MasterLocationDMO.Where(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTACO_PickUp_TRML_Id).FirstOrDefault().TRML_LocationName : "--",

                                             ASTA_Drop_TRML_Id = a.ASTACO_Drop_TRML_Id,
                                             TRML_DropLocationName = a.ASTACO_Drop_TRML_Id != 0 ? _buspasscontext.MasterLocationDMO.Where(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTACO_Drop_TRML_Id).FirstOrDefault().TRML_LocationName : "--",

                                             TRMR_Idp = a.ASTACO_PickUp_TRMR_Id,
                                             TRMR_PickRouteName = a.ASTACO_PickUp_TRMR_Id != 0 ? _buspasscontext.MasterRouteDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTACO_PickUp_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",

                                             TRMR_Idd = a.ASTACO_Drop_TRMR_Id,
                                             TRMR_DropRouteName = a.ASTACO_Drop_TRMR_Id != 0 ? _buspasscontext.MasterRouteDMO.Where(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTACO_Drop_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",
                                             ASTA_ApplStatus = a.ASTACO_ApplStatus,
                                             ASMAY_Year = data.ASMAY_Id != 0 ? _buspasscontext.AcademicYearDMO.Where(td => td.MI_Id == data.MI_Id && td.ASMAY_Id == a.ASTACO_ForAY).FirstOrDefault().ASMAY_Year : "--",


                                         }
                          ).Distinct().ToArray();



                }
                data.logoheader = (from a in _buspasscontext.FeeMasterConfigurationDMO
                                   where (a.MI_Id == data.MI_Id && a.userid == 364)
                                   select new CLGStudentBuspassFormDTO
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

        public CLGStudentBuspassFormDTO getloaddataintruction(CLGStudentBuspassFormDTO data)
        {
            try
            {

                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                List<MasterAcademic> allyear = new List<MasterAcademic>();
                List<MasterAcademic> allyearget = new List<MasterAcademic>();

                //Commented Praveen
                ////  allyearget = (from a in _feecontext.AcademicYear
                //                where (a.MI_Id == data.MI_Id && a.ASMAY_Pre_ActiveFlag == 1 && a.Is_Active == true && a.MI_Id == data.MI_Id)
                //                select new MasterAcademic
                //                {
                //                    ASMAY_Id = a.ASMAY_Id,
                //                    ASMAY_Year = a.ASMAY_Year
                //                }
                //   ).ToList();
                //comment end

                // Praveen added
                allyearget = (from a in _buspasscontext.AcademicYear
                              where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id && a.Is_Active == true && a.MI_Id == data.MI_Id)
                              select new MasterAcademic
                              {
                                  ASMAY_Id = a.ASMAY_Id,
                                  ASMAY_Year = a.ASMAY_Year
                              }
              ).ToList();
                // End


                allyear = (from a in _buspasscontext.AcademicYearDMO
                           where (a.MI_Id == data.MI_Id && a.ASMAY_TransportSDate <= indianTime && a.ASMAY_TransportEDate >= indianTime && a.ASMAY_Id == allyearget.FirstOrDefault().ASMAY_Id)
                           select new MasterAcademic
                           {
                               ASMAY_Id = a.ASMAY_Id
                           }
               ).ToList();

                if (allyear.Count > 0)
                {

                    //commented Praveen
                    // var Acdemic_preadmission = _buspasscontext.AcademicYearDMO.Where(t => t.ASMAY_Pre_ActiveFlag == 1 && t.Is_Active == true && t.MI_Id == data.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();
                    //comment end
                    // Praveen added
                    var Acdemic_preadmission = _buspasscontext.AcademicYearDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.Is_Active == true && t.MI_Id == data.MI_Id).Select(d => d.ASMAY_Id).FirstOrDefault();
                    //end

                    data.transportyear = Acdemic_preadmission;

                    var studentcurrentyear = (from a in _buspasscontext.Adm_College_Yearly_StudentDMO
                                              where (a.AMCST_Id == data.AMCST_Id)
                                              select a
                    ).ToList().OrderByDescending(d => d.ACYST_Id).ToArray();

                    if (studentcurrentyear.Length > 0)
                    {
                        if (studentcurrentyear.FirstOrDefault().ASMAY_Id != data.transportyear)
                        {
                            data.studentaccyear = studentcurrentyear.FirstOrDefault().ASMAY_Id;


                            List<CLGStudentBuspassFormDTO> temp_group = new List<CLGStudentBuspassFormDTO>();
                            temp_group = (from a in _buspasscontext.FeeGroupDMO
                                          from b in _buspasscontext.FeeHeadDMO
                                          from c in _buspasscontext.FeeYearlygroupHeadMappingDMO
                                          where (a.FMG_Id == c.FMG_Id && b.FMH_Id == c.FMH_Id && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.studentaccyear && (a.FMG_CompulsoryFlag == "T" || a.FMG_CompulsoryFlag == "N" || b.FMH_Flag == "T"))
                                          select new CLGStudentBuspassFormDTO
                                          {
                                              FMG_Id = a.FMG_Id
                                          }).Distinct().ToList();

                            List<long> grp_ids = new List<long>();
                            foreach (var item in temp_group)
                            {
                                grp_ids.Add(item.FMG_Id);
                            }



                            var openingbal = (from a in _buspasscontext.Fee_College_Student_StatusDMO
                                              where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.studentaccyear && grp_ids.Contains(a.FMG_Id) && a.AMCST_Id == data.AMCST_Id && a.FCSS_ToBePaid > 0)
                                              select new CLGStudentBuspassFormDTO
                                              {
                                                  openingbalance = a.FCSS_ToBePaid
                                              }).ToList().ToArray().Count();

                            if (openingbal == 0)
                            {
                                data.routeDetails = (from a in _buspasscontext.CLGAdm_Std_Transport_ApplicationDMO
                                                     from d in _buspasscontext.Adm_Master_College_StudentDMO
                                                     where (a.AMCST_Id == d.AMCST_Id && d.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && a.AMCST_Id == data.AMCST_Id)
                                                     select new CLGStudentBuspassFormDTO
                                                     {
                                                         AMCST_Id = a.AMCST_Id

                                                     }
                                    ).Distinct().ToArray();

                                data.openba = false;

                            }
                            else
                            {
                                data.openba = true;
                            }


                        }
                        else
                        {
                            data.studentaccyear = studentcurrentyear.FirstOrDefault().ASMAY_Id;
                            data.routeDetails = (from a in _buspasscontext.CLGAdm_Std_Transport_ApplicationDMO
                                                 from d in _buspasscontext.Adm_Master_College_StudentDMO
                                                 where (a.AMCST_Id == d.AMCST_Id && d.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && a.AMCST_Id == data.AMCST_Id)
                                                 select new CLGStudentBuspassFormDTO
                                                 {
                                                     AMCST_Id = a.AMCST_Id

                                                 }
                                    ).Distinct().ToArray();

                            data.openba = false;
                        }

                    }

                    else
                    {
                        var studentadmityear = (from a in _buspasscontext.Adm_Master_College_StudentDMO
                                                where (a.AMCST_Id == data.AMCST_Id)
                                                select a
                   ).ToList().ToArray();

                        data.studentaccyear = studentadmityear.FirstOrDefault().ASMAY_Id;

                        data.routeDetails = (from a in _buspasscontext.CLGAdm_Std_Transport_ApplicationDMO
                                             from d in _buspasscontext.Adm_Master_College_StudentDMO
                                             where (a.AMCST_Id == d.AMCST_Id && d.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && a.AMCST_Id == data.AMCST_Id)
                                             select new CLGStudentBuspassFormDTO
                                             {
                                                 AMCST_Id = a.AMCST_Id

                                             }
                                    ).Distinct().ToArray();
                        data.openba = false;
                    }

                    var acayrorder = (from a in _buspasscontext.AcademicYearDMO
                                      where (a.MI_Id == data.MI_Id && a.Is_Active == true && a.ASMAY_Id == data.studentaccyear)
                                      select new CLGStudentBuspassFormDTO
                                      {
                                          ASMAY_Order = a.ASMAY_Order,
                                      }
         ).Distinct().ToArray();

                    int acaorder = 0;
                    for (int s = 0; s < acayrorder.Count(); s++)
                    {
                        acaorder = Convert.ToInt16(acayrorder[s].ASMAY_Order);
                    }

                    List<AcademicYear> acayr = new List<AcademicYear>();
                    acayr = _buspasscontext.AcademicYearDMO.Where(r => r.MI_Id == data.MI_Id && r.Is_Active == true && r.ASMAY_Order >= acaorder).ToList();
                    data.fillyear = acayr.ToArray();

                    List<AcademicYear> curracayr = new List<AcademicYear>();
                    curracayr = _buspasscontext.AcademicYearDMO.Where(r => r.MI_Id == data.MI_Id && r.ASMAY_Id == data.transportyear).ToList();
                    data.currfillyear = curracayr.ToArray();

                    data.trnsportcutoffdate = "True";
                }
                else
                {
                    data.trnsportcutoffdate = "False";
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public CLGStudentBuspassFormDTO getstudata(CLGStudentBuspassFormDTO data)
        {
            try
            {

                var studentcurrentyear = (from a in _buspasscontext.Adm_College_Yearly_StudentDMO
                                          where (a.AMCST_Id == data.AMCST_Id)
                                          select a
              ).ToList().OrderByDescending(d => d.ACYST_Id).ToArray();

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
                    var studentadmityear = (from a in _buspasscontext.Adm_Master_College_StudentDMO
                                            where (a.AMCST_Id == data.AMCST_Id)
                                            select a
               ).ToList().ToArray();

                    data.studentaccyear = studentadmityear.FirstOrDefault().ASMAY_Id;

                }
                data.regularnew = (from a in _buspasscontext.FeeGroupDMO
                                   from b in _buspasscontext.FeeYearlygroupHeadMappingDMO
                                   from c in _buspasscontext.Fee_College_Student_StatusDMO
                                   from d in _buspasscontext.FeeHeadDMO
                                   where (b.FMG_Id == a.FMG_Id && b.FMH_Id == d.FMH_Id && c.FMG_Id == b.FMG_Id && c.FMH_Id == b.FMH_Id && c.FMG_Id == a.FMG_Id && c.FMH_Id == d.FMH_Id && c.ASMAY_Id == b.ASMAY_Id && d.FMH_Flag == "T" && c.MI_Id == data.MI_Id && c.ASMAY_Id == data.studentaccyear && c.FCSS_CurrentYrCharges > 0 && c.AMCST_Id == data.AMCST_Id)
                                   select new CLGStudentBuspassFormDTO
                                   {
                                       AMCST_Id = c.AMCST_Id

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


                //pravven added
                var trnsconfig = (from a in _buspasscontext.AcademicYearDMO
                                  where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)
                                  select a).ToList();

                data.transappfillAdmissionNew = false;
                data.transappfillTrNew = false;
                data.transappfillTrRegular = false;


                if (trnsconfig.FirstOrDefault().ASMAY_NewFlg == true)
                {
                    if (data.studentTrstatus == "TrNew")
                    {
                        data.transappfillTrNew = true;
                    }
                    else
                    {
                        data.transappfillTrNew = false;
                    }
                }


                if (trnsconfig.FirstOrDefault().ASMAY_ReggularFlg == true)
                {
                    if (data.studentTrstatus == "TrRegular")
                    {
                        data.transappfillTrRegular = true;
                    }
                    else
                    {
                        data.transappfillTrRegular = false;
                    }
                }


                if (trnsconfig.FirstOrDefault().ASMAY_NewAdmissionFlg == true)
                {
                    if (data.studentstatus == "AdmissionNew")
                    {
                        data.transappfillAdmissionNew = true;
                    }
                    else
                    {
                        data.transappfillAdmissionNew = false;
                    }

                }








                if (data.transappfillAdmissionNew == true || data.transappfillTrNew == true || data.transappfillTrRegular == true)
                {
                    data.trnsportcutoffdate = "True";
                    data.countryid = _buspasscontext.country.ToArray();

                    List<MasterAreaDMO> saa = new List<MasterAreaDMO>();
                    saa = _buspasscontext.MasterAreaDMO.Where(r => r.MI_Id == data.MI_Id && r.TRMA_ActiveFlg == true).ToList();
                    data.areaList = saa.ToArray();
                    //route
                    List<MasterRouteDMO> rout = new List<MasterRouteDMO>();
                    rout = _buspasscontext.MasterRouteDMO.Where(r => r.MI_Id == data.MI_Id && r.TRMR_ActiveFlg == true).OrderBy(t => t.TRMR_order).ToList();
                    data.routeList = rout.ToArray();
                    // location
                    List<MasterLocationDMO> locat = new List<MasterLocationDMO>();
                    locat = _buspasscontext.MasterLocationDMO.Where(r => r.MI_Id == data.MI_Id && r.TRML_ActiveFlg == true).ToList();
                    data.locaList = locat.ToArray();

                    List<CLGAdm_Std_Transport_ApplicationDMO> trans = new List<CLGAdm_Std_Transport_ApplicationDMO>();
                    trans = _buspasscontext.CLGAdm_Std_Transport_ApplicationDMO.Where(r => r.MI_Id == data.MI_Id && r.AMCST_Id == data.AMCST_Id && r.ASTACO_ForAY == data.ASMAY_Id).ToList();
                    var trans_amstid = trans.ToArray();

                    if (trans.Count() > 0)
                    {
                        var studentcurrentyearr = (from a in _buspasscontext.Adm_College_Yearly_StudentDMO
                                                   where (a.AMCST_Id == data.AMCST_Id)
                                                   select a
                   ).ToList().OrderByDescending(d => d.ACYST_Id).ToArray();
                        if (studentcurrentyearr.Length > 0)
                        {

                            using (var cmd = _buspasscontext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "CLG_TR_GET_TRNAPPLN_STUDENT_ROUTE_DETAILS";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                    SqlDbType.BigInt)
                                {
                                    Value = data.MI_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                                  SqlDbType.BigInt)
                                {
                                    Value = data.AMCST_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                   SqlDbType.BigInt)
                                {
                                    Value = data.ASMAY_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@FLAG",
                                  SqlDbType.VarChar)
                                {
                                    Value = "S"
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
                                    data.studentDetails = retObject.ToArray();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                            //    if (trans[0].ASTACO_PickUp_TRMR_Id != 0 && trans[0].ASTACO_Drop_TRMR_Id == 0)
                            //    {
                            //        data.studentDetails = (from d in _buspasscontext.AcademicYearDMO
                            //                               from a in _buspasscontext.School_M_Class
                            //                               from b in _buspasscontext.School_M_Section
                            //                               from c in _buspasscontext.School_Adm_Y_StudentDMO
                            //                               from f in _buspasscontext.Adm_M_Student
                            //                               from g in _buspasscontext.country
                            //                               from h in _buspasscontext.state
                            //                               from i in _buspasscontext.MasterRouteDMO
                            //                               from j in _buspasscontext.MasterLocationDMO
                            //                               from k in _buspasscontext.Adm_Student_Transport_ApplicationDMO
                            //                               where (
                            //                               f.AMST_Id == c.AMST_Id && d.ASMAY_Id == f.ASMAY_Id && c.ASMCL_Id == a.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && g.IVRMMC_Id == f.AMST_ConCountry && h.IVRMMS_Id == f.AMST_ConState && h.IVRMMC_Id == g.IVRMMC_Id && k.AMST_Id == f.AMST_Id && (i.TRMR_Id == k.ASTA_PickUp_TRMR_Id) && (j.TRML_Id == k.ASTA_PickUp_TRML_Id) && k.AMST_Id == data.AMST_Id && k.ASTA_FutureAY == data.ASMAY_Id && f.MI_Id == data.MI_Id)
                            //                               select new CLGStudentBuspassFormDTO
                            //                               {
                            //                                   AMST_FirstName = ((f.AMST_FirstName == null || f.AMST_FirstName == "0" ? "" : f.AMST_FirstName) + " " + (f.AMST_MiddleName == null || f.AMST_MiddleName == "0" ? "" : f.AMST_MiddleName) + " " + (f.AMST_LastName == null || f.AMST_LastName == "0" ? "" : f.AMST_LastName)).Trim(),
                            //                                   ASMCL_Id = c.ASMCL_Id,
                            //                                   ASMCL_ClassName = a.ASMCL_ClassName,
                            //                                   ASMS_Id = c.ASMS_Id,
                            //                                   AMST_BloodGroup = f.AMST_BloodGroup,
                            //                                   ASMC_SectionName = b.ASMC_SectionName,
                            //                                   ASMAY_Id = c.ASMAY_Id,
                            //                                   ASMAY_Year = d.ASMAY_Year,
                            //                                   AMST_AdmNo = f.AMST_AdmNo,
                            //                                   AMST_DOB = f.AMST_DOB,
                            //                                   AMST_emailId = f.AMST_emailId,
                            //                                   AMST_MobileNo = f.AMST_MobileNo,
                            //                                   AMST_PerStreet = f.AMST_ConStreet,
                            //                                   AMST_PerCity = f.AMST_ConCity,
                            //                                   AMST_PerArea = f.AMST_ConArea,
                            //                                   AMST_PerPincode = f.AMST_ConPincode,
                            //                                   IVRMMC_CountryName = g.IVRMMC_CountryName,
                            //                                   IVRMMS_Name = h.IVRMMS_Name,
                            //                                   AMST_FatherName = f.AMST_FatherName,
                            //                                   AMST_MotherName = f.AMST_MotherName,
                            //                                   AMST_FatherMobleNo = k.ASTA_FatherMobileNo,
                            //                                   AMST_MotherMobileNo = k.ASTA_MotherMobileNo,
                            //                                   AMST_FatherOfficeAdd = f.AMST_FatherOfficeAdd,
                            //                                   ASTA_AreaZoneName = k.ASTA_AreaZoneName,
                            //                                   TRMA_Id = k.TRMA_Id,
                            //                                   ASTA_PickUp_TRML_Id = k.ASTA_PickUp_TRML_Id,
                            //                                   ASTA_Drop_TRML_Id = k.ASTA_Drop_TRML_Id,
                            //                                   ASTA_PickUp_TRMR_Id = k.ASTA_PickUp_TRMR_Id,
                            //                                   ASTA_Drop_TRMR_Id = k.ASTA_Drop_TRMR_Id,
                            //                                   ASTA_Landmark = k.ASTA_Landmark,
                            //                                   ASTA_Phoneoff = k.ASTA_Phoneoff,
                            //                                   ASTA_PhoneRes = k.ASTA_PhoneRes,
                            //                                   AMST_Photoname = f.AMST_Photoname,
                            //                                   IVRMMS_Id = h.IVRMMS_Id,
                            //                                   ASTA_FutureAY = k.ASTA_FutureAY,
                            //                                   ASTA_Id = k.ASTA_Id,
                            //                                   ASMAY_Order = a.ASMCL_Order,

                            //                               }
                            //).Distinct().OrderByDescending(f => f.ASMAY_Order).ToArray();
                            //    }
                            //    else if (trans[0].ASTA_PickUp_TRMR_Id == 0 && trans[0].ASTA_Drop_TRMR_Id != 0)
                            //    {
                            //        data.studentDetails = (from d in _buspasscontext.AcademicYearDMO
                            //                               from a in _buspasscontext.School_M_Class
                            //                               from b in _buspasscontext.School_M_Section
                            //                               from c in _buspasscontext.School_Adm_Y_StudentDMO
                            //                               from f in _buspasscontext.Adm_M_Student
                            //                               from g in _buspasscontext.country
                            //                               from h in _buspasscontext.state
                            //                               from i in _buspasscontext.MasterRouteDMO
                            //                               from j in _buspasscontext.MasterLocationDMO
                            //                               from k in _buspasscontext.Adm_Student_Transport_ApplicationDMO

                            //                               where (f.AMST_Id == c.AMST_Id && d.ASMAY_Id == f.ASMAY_Id && c.ASMCL_Id == a.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && g.IVRMMC_Id == f.AMST_ConCountry && h.IVRMMS_Id == f.AMST_ConState && h.IVRMMC_Id == g.IVRMMC_Id && k.AMST_Id == f.AMST_Id && (i.TRMR_Id == k.ASTA_Drop_TRMR_Id) && (j.TRML_Id == k.ASTA_Drop_TRML_Id) && k.AMST_Id == data.AMST_Id && k.ASTA_FutureAY == data.ASMAY_Id && f.MI_Id == data.MI_Id)
                            //                               select new CLGStudentBuspassFormDTO
                            //                               {
                            //                                   AMST_FirstName = ((f.AMST_FirstName == null || f.AMST_FirstName == "0" ? "" : f.AMST_FirstName) + " " + (f.AMST_MiddleName == null || f.AMST_MiddleName == "0" ? "" : f.AMST_MiddleName) + " " + (f.AMST_LastName == null || f.AMST_LastName == "0" ? "" : f.AMST_LastName)).Trim(),
                            //                                   ASMCL_Id = c.ASMCL_Id,
                            //                                   ASMCL_ClassName = a.ASMCL_ClassName,
                            //                                   ASMS_Id = c.ASMS_Id,
                            //                                   AMST_BloodGroup = f.AMST_BloodGroup,
                            //                                   ASMC_SectionName = b.ASMC_SectionName,
                            //                                   ASMAY_Id = c.ASMAY_Id,
                            //                                   ASMAY_Year = d.ASMAY_Year,
                            //                                   AMST_AdmNo = f.AMST_AdmNo,
                            //                                   AMST_DOB = f.AMST_DOB,
                            //                                   AMST_emailId = f.AMST_emailId,
                            //                                   AMST_MobileNo = f.AMST_MobileNo,
                            //                                   AMST_PerStreet = f.AMST_ConStreet,
                            //                                   AMST_PerCity = f.AMST_ConCity,
                            //                                   AMST_PerArea = f.AMST_ConArea,
                            //                                   AMST_PerPincode = f.AMST_ConPincode,
                            //                                   IVRMMC_CountryName = g.IVRMMC_CountryName,
                            //                                   IVRMMS_Name = h.IVRMMS_Name,
                            //                                   AMST_FatherName = f.AMST_FatherName,
                            //                                   AMST_MotherName = f.AMST_MotherName,
                            //                                   AMST_FatherMobleNo = k.ASTA_FatherMobileNo,
                            //                                   AMST_MotherMobileNo = k.ASTA_MotherMobileNo,
                            //                                   AMST_FatherOfficeAdd = f.AMST_FatherOfficeAdd,
                            //                                   ASTA_AreaZoneName = k.ASTA_AreaZoneName,
                            //                                   TRMA_Id = k.TRMA_Id,
                            //                                   ASTA_PickUp_TRML_Id = k.ASTA_PickUp_TRML_Id,
                            //                                   ASTA_Drop_TRML_Id = k.ASTA_Drop_TRML_Id,
                            //                                   ASTA_PickUp_TRMR_Id = k.ASTA_PickUp_TRMR_Id,
                            //                                   ASTA_Drop_TRMR_Id = k.ASTA_Drop_TRMR_Id,
                            //                                   ASTA_Landmark = k.ASTA_Landmark,
                            //                                   ASTA_Phoneoff = k.ASTA_Phoneoff,
                            //                                   ASTA_PhoneRes = k.ASTA_PhoneRes,
                            //                                   AMST_Photoname = f.AMST_Photoname,
                            //                                   IVRMMS_Id = h.IVRMMS_Id,
                            //                                   ASTA_FutureAY = k.ASTA_FutureAY,
                            //                                   ASTA_Id = k.ASTA_Id,
                            //                                   ASMAY_Order = a.ASMCL_Order,

                            //                               }
                            //).Distinct().OrderByDescending(f => f.ASMAY_Order).ToArray();
                            //    }
                            //    else if (trans[0].ASTA_PickUp_TRMR_Id != 0 && trans[0].ASTA_Drop_TRMR_Id != 0)
                            //    {
                            //        data.studentDetails = (from d in _buspasscontext.AcademicYearDMO
                            //                               from a in _buspasscontext.School_M_Class
                            //                               from b in _buspasscontext.School_M_Section
                            //                               from c in _buspasscontext.School_Adm_Y_StudentDMO
                            //                               from f in _buspasscontext.Adm_M_Student
                            //                               from g in _buspasscontext.country
                            //                               from h in _buspasscontext.state
                            //                               from i in _buspasscontext.MasterRouteDMO
                            //                               from j in _buspasscontext.MasterLocationDMO
                            //                               from k in _buspasscontext.Adm_Student_Transport_ApplicationDMO
                            //                               where (

                            //                               f.AMST_Id == c.AMST_Id && d.ASMAY_Id == c.ASMAY_Id && c.ASMCL_Id == a.ASMCL_Id && c.ASMS_Id == b.ASMS_Id && g.IVRMMC_Id == h.IVRMMC_Id && g.IVRMMC_Id == f.AMST_ConCountry && h.IVRMMS_Id == f.AMST_ConState && c.AMST_Id == k.AMST_Id &&
                            //                               (i.TRMR_Id == k.ASTA_PickUp_TRMR_Id || i.TRMR_Id == k.ASTA_Drop_TRMR_Id) && (j.TRML_Id == k.ASTA_Drop_TRML_Id || j.TRML_Id == k.ASTA_PickUp_TRML_Id) && k.AMST_Id == data.AMST_Id && k.ASTA_FutureAY == data.ASMAY_Id && f.AMST_Id == data.AMST_Id



                            //                            /*   f.AMST_Id == c.AMST_Id && d.ASMAY_Id == f.ASMAY_Id && c.ASMCL_Id == a.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && g.IVRMMC_Id == f.AMST_ConCountry && h.IVRMMS_Id == f.AMST_ConState && h.IVRMMC_Id == g.IVRMMC_Id && k.AMST_Id == f.AMST_Id && (i.TRMR_Id == k.ASTA_PickUp_TRMR_Id || i.TRMR_Id == k.ASTA_Drop_TRMR_Id) && (j.TRML_Id == k.ASTA_Drop_TRML_Id || j.TRML_Id == k.ASTA_PickUp_TRML_Id) && k.AMST_Id == data.AMST_Id && c.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id*/
                            //                            )
                            //                               select new CLGStudentBuspassFormDTO
                            //                               {
                            //                                   AMST_FirstName = ((f.AMST_FirstName == null || f.AMST_FirstName == "0" ? "" : f.AMST_FirstName) + " " + (f.AMST_MiddleName == null || f.AMST_MiddleName == "0" ? "" : f.AMST_MiddleName) + " " + (f.AMST_LastName == null || f.AMST_LastName == "0" ? "" : f.AMST_LastName)).Trim(),
                            //                                   ASMCL_Id = c.ASMCL_Id,
                            //                                   ASMCL_ClassName = a.ASMCL_ClassName,
                            //                                   ASMS_Id = c.ASMS_Id,
                            //                                   AMST_BloodGroup = f.AMST_BloodGroup,
                            //                                   ASMC_SectionName = b.ASMC_SectionName,
                            //                                   ASMAY_Id = c.ASMAY_Id,
                            //                                   ASMAY_Year = d.ASMAY_Year,
                            //                                   AMST_AdmNo = f.AMST_AdmNo,
                            //                                   AMST_DOB = f.AMST_DOB,
                            //                                   AMST_emailId = f.AMST_emailId,
                            //                                   AMST_MobileNo = f.AMST_MobileNo,
                            //                                   AMST_PerStreet = f.AMST_ConStreet,
                            //                                   AMST_PerCity = f.AMST_ConCity,
                            //                                   AMST_PerArea = f.AMST_ConArea,
                            //                                   AMST_PerPincode = f.AMST_ConPincode,
                            //                                   IVRMMC_CountryName = g.IVRMMC_CountryName,
                            //                                   IVRMMS_Name = h.IVRMMS_Name,
                            //                                   AMST_FatherName = f.AMST_FatherName,
                            //                                   AMST_MotherName = f.AMST_MotherName,
                            //                                   AMST_FatherMobleNo = k.ASTA_FatherMobileNo,
                            //                                   AMST_MotherMobileNo = k.ASTA_MotherMobileNo,
                            //                                   AMST_FatherOfficeAdd = f.AMST_FatherOfficeAdd,
                            //                                   ASTA_AreaZoneName = k.ASTA_AreaZoneName,
                            //                                   TRMA_Id = k.TRMA_Id,
                            //                                   ASTA_PickUp_TRML_Id = k.ASTA_PickUp_TRML_Id,
                            //                                   ASTA_Drop_TRML_Id = k.ASTA_Drop_TRML_Id,
                            //                                   ASTA_PickUp_TRMR_Id = k.ASTA_PickUp_TRMR_Id,
                            //                                   ASTA_Drop_TRMR_Id = k.ASTA_Drop_TRMR_Id,
                            //                                   ASTA_Landmark = k.ASTA_Landmark,
                            //                                   ASTA_Phoneoff = k.ASTA_Phoneoff,
                            //                                   ASTA_PhoneRes = k.ASTA_PhoneRes,
                            //                                   AMST_Photoname = f.AMST_Photoname,
                            //                                   IVRMMS_Id = h.IVRMMS_Id,
                            //                                   ASTA_FutureAY = k.ASTA_FutureAY,
                            //                                   ASTA_Id = k.ASTA_Id,
                            //                                   ASMAY_Order = a.ASMCL_Order,

                            //                               }
                            // ).Distinct().OrderByDescending(f => f.ASMAY_Order).ToArray();
                            //    }

                        }
                        else
                        {



                            using (var cmd = _buspasscontext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "CLG_TR_GET_TRNAPPLN_STUDENT_ROUTE_DETAILS";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                    SqlDbType.BigInt)
                                {
                                    Value = data.MI_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                                  SqlDbType.BigInt)
                                {
                                    Value = data.AMCST_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                   SqlDbType.BigInt)
                                {
                                    Value = data.ASMAY_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@FLAG",
                                  SqlDbType.VarChar)
                                {
                                    Value = "N"
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
                                    data.studentDetails = retObject.ToArray();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }



                            //    if (trans[0].ASTA_PickUp_TRMR_Id != 0 && trans[0].ASTA_Drop_TRMR_Id == 0)
                            //    {
                            //        data.studentDetails = (from d in _buspasscontext.AcademicYearDMO
                            //                               from a in _buspasscontext.School_M_Class
                            //                               from f in _buspasscontext.Adm_M_Student
                            //                               from g in _buspasscontext.country
                            //                               from h in _buspasscontext.state
                            //                               from i in _buspasscontext.MasterRouteDMO
                            //                               from j in _buspasscontext.MasterLocationDMO
                            //                               from k in _buspasscontext.Adm_Student_Transport_ApplicationDMO
                            //                               where (
                            //                                f.ASMAY_Id == d.ASMAY_Id && f.ASMCL_Id == a.ASMCL_Id && g.IVRMMC_Id == f.AMST_ConCountry && h.IVRMMS_Id == f.AMST_ConState && h.IVRMMC_Id == g.IVRMMC_Id && k.AMST_Id == f.AMST_Id && (i.TRMR_Id == k.ASTA_PickUp_TRMR_Id) && (j.TRML_Id == k.ASTA_PickUp_TRML_Id) && k.AMST_Id == data.AMST_Id && k.ASTA_FutureAY == data.ASMAY_Id && f.MI_Id == data.MI_Id)
                            //                               select new CLGStudentBuspassFormDTO
                            //                               {
                            //                                   AMST_FirstName = ((f.AMST_FirstName == null || f.AMST_FirstName == "0" ? "" : f.AMST_FirstName) + " " + (f.AMST_MiddleName == null || f.AMST_MiddleName == "0" ? "" : f.AMST_MiddleName) + " " + (f.AMST_LastName == null || f.AMST_LastName == "0" ? "" : f.AMST_LastName)).Trim(),
                            //                                   ASMCL_Id = f.ASMCL_Id,
                            //                                   ASMCL_ClassName = a.ASMCL_ClassName,

                            //                                   AMST_BloodGroup = f.AMST_BloodGroup,

                            //                                   ASMAY_Id = f.ASMAY_Id,
                            //                                   ASMAY_Year = d.ASMAY_Year,
                            //                                   AMST_AdmNo = f.AMST_AdmNo,
                            //                                   AMST_DOB = f.AMST_DOB,
                            //                                   AMST_emailId = f.AMST_emailId,
                            //                                   AMST_MobileNo = f.AMST_MobileNo,
                            //                                   AMST_PerStreet = f.AMST_ConStreet,
                            //                                   AMST_PerCity = f.AMST_ConCity,
                            //                                   AMST_PerArea = f.AMST_ConArea,
                            //                                   AMST_PerPincode = f.AMST_ConPincode,
                            //                                   IVRMMC_CountryName = g.IVRMMC_CountryName,
                            //                                   IVRMMS_Name = h.IVRMMS_Name,
                            //                                   AMST_FatherName = f.AMST_FatherName,
                            //                                   AMST_MotherName = f.AMST_MotherName,
                            //                                   AMST_FatherMobleNo = k.ASTA_FatherMobileNo,
                            //                                   AMST_MotherMobileNo = k.ASTA_MotherMobileNo,
                            //                                   AMST_FatherOfficeAdd = f.AMST_FatherOfficeAdd,
                            //                                   ASTA_AreaZoneName = k.ASTA_AreaZoneName,
                            //                                   TRMA_Id = k.TRMA_Id,
                            //                                   ASTA_PickUp_TRML_Id = k.ASTA_PickUp_TRML_Id,
                            //                                   ASTA_Drop_TRML_Id = k.ASTA_Drop_TRML_Id,
                            //                                   ASTA_PickUp_TRMR_Id = k.ASTA_PickUp_TRMR_Id,
                            //                                   ASTA_Drop_TRMR_Id = k.ASTA_Drop_TRMR_Id,
                            //                                   ASTA_Landmark = k.ASTA_Landmark,
                            //                                   ASTA_Phoneoff = k.ASTA_Phoneoff,
                            //                                   ASTA_PhoneRes = k.ASTA_PhoneRes,
                            //                                   AMST_Photoname = f.AMST_Photoname,
                            //                                   IVRMMS_Id = h.IVRMMS_Id,
                            //                                   ASTA_FutureAY = k.ASTA_FutureAY,
                            //                                   ASTA_Id = k.ASTA_Id,
                            //                                   ASMAY_Order = a.ASMCL_Order,

                            //                               }
                            //).Distinct().OrderByDescending(f => f.ASMAY_Order).ToArray();
                            //    }
                            //    else if (trans[0].ASTA_PickUp_TRMR_Id == 0 && trans[0].ASTA_Drop_TRMR_Id != 0)
                            //    {
                            //        data.studentDetails = (from d in _buspasscontext.AcademicYearDMO
                            //                               from a in _buspasscontext.School_M_Class
                            //                               from f in _buspasscontext.Adm_M_Student
                            //                               from g in _buspasscontext.country
                            //                               from h in _buspasscontext.state
                            //                               from i in _buspasscontext.MasterRouteDMO
                            //                               from j in _buspasscontext.MasterLocationDMO
                            //                               from k in _buspasscontext.Adm_Student_Transport_ApplicationDMO

                            //                               where (f.ASMAY_Id == d.ASMAY_Id && f.ASMCL_Id == a.ASMCL_Id && g.IVRMMC_Id == f.AMST_ConCountry && h.IVRMMS_Id == f.AMST_ConState && h.IVRMMC_Id == g.IVRMMC_Id && k.AMST_Id == f.AMST_Id && (i.TRMR_Id == k.ASTA_Drop_TRMR_Id) && (j.TRML_Id == k.ASTA_Drop_TRML_Id) && k.AMST_Id == data.AMST_Id && k.ASTA_FutureAY == data.ASMAY_Id && f.MI_Id == data.MI_Id)
                            //                               select new CLGStudentBuspassFormDTO
                            //                               {
                            //                                   AMST_FirstName = ((f.AMST_FirstName == null || f.AMST_FirstName == "0" ? "" : f.AMST_FirstName) + " " + (f.AMST_MiddleName == null || f.AMST_MiddleName == "0" ? "" : f.AMST_MiddleName) + " " + (f.AMST_LastName == null || f.AMST_LastName == "0" ? "" : f.AMST_LastName)).Trim(),
                            //                                   ASMCL_Id = f.ASMCL_Id,
                            //                                   ASMCL_ClassName = a.ASMCL_ClassName,

                            //                                   AMST_BloodGroup = f.AMST_BloodGroup,

                            //                                   ASMAY_Id = f.ASMAY_Id,
                            //                                   ASMAY_Year = d.ASMAY_Year,
                            //                                   AMST_AdmNo = f.AMST_AdmNo,
                            //                                   AMST_DOB = f.AMST_DOB,
                            //                                   AMST_emailId = f.AMST_emailId,
                            //                                   AMST_MobileNo = f.AMST_MobileNo,
                            //                                   AMST_PerStreet = f.AMST_ConStreet,
                            //                                   AMST_PerCity = f.AMST_ConCity,
                            //                                   AMST_PerArea = f.AMST_ConArea,
                            //                                   AMST_PerPincode = f.AMST_ConPincode,
                            //                                   IVRMMC_CountryName = g.IVRMMC_CountryName,
                            //                                   IVRMMS_Name = h.IVRMMS_Name,
                            //                                   AMST_FatherName = f.AMST_FatherName,
                            //                                   AMST_MotherName = f.AMST_MotherName,
                            //                                   AMST_FatherMobleNo = k.ASTA_FatherMobileNo,
                            //                                   AMST_MotherMobileNo = k.ASTA_MotherMobileNo,
                            //                                   AMST_FatherOfficeAdd = f.AMST_FatherOfficeAdd,
                            //                                   ASTA_AreaZoneName = k.ASTA_AreaZoneName,
                            //                                   TRMA_Id = k.TRMA_Id,
                            //                                   ASTA_PickUp_TRML_Id = k.ASTA_PickUp_TRML_Id,
                            //                                   ASTA_Drop_TRML_Id = k.ASTA_Drop_TRML_Id,
                            //                                   ASTA_PickUp_TRMR_Id = k.ASTA_PickUp_TRMR_Id,
                            //                                   ASTA_Drop_TRMR_Id = k.ASTA_Drop_TRMR_Id,
                            //                                   ASTA_Landmark = k.ASTA_Landmark,
                            //                                   ASTA_Phoneoff = k.ASTA_Phoneoff,
                            //                                   ASTA_PhoneRes = k.ASTA_PhoneRes,
                            //                                   AMST_Photoname = f.AMST_Photoname,
                            //                                   IVRMMS_Id = h.IVRMMS_Id,
                            //                                   ASTA_FutureAY = k.ASTA_FutureAY,
                            //                                   ASTA_Id = k.ASTA_Id,
                            //                                   ASMAY_Order = a.ASMCL_Order,

                            //                               }
                            //).Distinct().OrderByDescending(f => f.ASMAY_Order).ToArray();
                            //    }
                            //    else if (trans[0].ASTA_PickUp_TRMR_Id != 0 && trans[0].ASTA_Drop_TRMR_Id != 0)
                            //    {
                            //        data.studentDetails = (from d in _buspasscontext.AcademicYearDMO
                            //                               from a in _buspasscontext.School_M_Class

                            //                               from f in _buspasscontext.Adm_M_Student
                            //                               from g in _buspasscontext.country
                            //                               from h in _buspasscontext.state
                            //                               from i in _buspasscontext.MasterRouteDMO
                            //                               from j in _buspasscontext.MasterLocationDMO
                            //                               from k in _buspasscontext.Adm_Student_Transport_ApplicationDMO
                            //                               where (f.ASMAY_Id == f.ASMAY_Id && f.ASMCL_Id == a.ASMCL_Id && g.IVRMMC_Id == f.AMST_ConCountry && h.IVRMMS_Id == f.AMST_ConState && h.IVRMMC_Id == g.IVRMMC_Id && k.AMST_Id == f.AMST_Id && (i.TRMR_Id == k.ASTA_PickUp_TRMR_Id || i.TRMR_Id == k.ASTA_Drop_TRMR_Id) && (j.TRML_Id == k.ASTA_Drop_TRML_Id || j.TRML_Id == k.ASTA_PickUp_TRML_Id) && k.AMST_Id == data.AMST_Id && k.ASTA_FutureAY == data.ASMAY_Id && f.MI_Id == data.MI_Id)
                            //                               select new CLGStudentBuspassFormDTO
                            //                               {
                            //                                   AMST_FirstName = ((f.AMST_FirstName == null || f.AMST_FirstName == "0" ? "" : f.AMST_FirstName) + " " + (f.AMST_MiddleName == null || f.AMST_MiddleName == "0" ? "" : f.AMST_MiddleName) + " " + (f.AMST_LastName == null || f.AMST_LastName == "0" ? "" : f.AMST_LastName)).Trim(),
                            //                                   ASMCL_Id = f.ASMCL_Id,
                            //                                   ASMCL_ClassName = a.ASMCL_ClassName,

                            //                                   AMST_BloodGroup = f.AMST_BloodGroup,

                            //                                   ASMAY_Id = f.ASMAY_Id,
                            //                                   ASMAY_Year = d.ASMAY_Year,
                            //                                   AMST_AdmNo = f.AMST_AdmNo,
                            //                                   AMST_DOB = f.AMST_DOB,
                            //                                   AMST_emailId = f.AMST_emailId,
                            //                                   AMST_MobileNo = f.AMST_MobileNo,
                            //                                   AMST_PerStreet = f.AMST_ConStreet,
                            //                                   AMST_PerCity = f.AMST_ConCity,
                            //                                   AMST_PerArea = f.AMST_ConArea,
                            //                                   AMST_PerPincode = f.AMST_ConPincode,
                            //                                   IVRMMC_CountryName = g.IVRMMC_CountryName,
                            //                                   IVRMMS_Name = h.IVRMMS_Name,
                            //                                   AMST_FatherName = f.AMST_FatherName,
                            //                                   AMST_MotherName = f.AMST_MotherName,
                            //                                   AMST_FatherMobleNo = k.ASTA_FatherMobileNo,
                            //                                   AMST_MotherMobileNo = k.ASTA_MotherMobileNo,
                            //                                   AMST_FatherOfficeAdd = f.AMST_FatherOfficeAdd,
                            //                                   ASTA_AreaZoneName = k.ASTA_AreaZoneName,
                            //                                   TRMA_Id = k.TRMA_Id,
                            //                                   ASTA_PickUp_TRML_Id = k.ASTA_PickUp_TRML_Id,
                            //                                   ASTA_Drop_TRML_Id = k.ASTA_Drop_TRML_Id,
                            //                                   ASTA_PickUp_TRMR_Id = k.ASTA_PickUp_TRMR_Id,
                            //                                   ASTA_Drop_TRMR_Id = k.ASTA_Drop_TRMR_Id,
                            //                                   ASTA_Landmark = k.ASTA_Landmark,
                            //                                   ASTA_Phoneoff = k.ASTA_Phoneoff,
                            //                                   ASTA_PhoneRes = k.ASTA_PhoneRes,
                            //                                   AMST_Photoname = f.AMST_Photoname,
                            //                                   IVRMMS_Id = h.IVRMMS_Id,
                            //                                   ASTA_FutureAY = k.ASTA_FutureAY,
                            //                                   ASTA_Id = k.ASTA_Id,
                            //                                   ASMAY_Order = a.ASMCL_Order,

                            //                               }
                            // ).Distinct().OrderByDescending(f => f.ASMAY_Order).ToArray();
                            //    }

                        }




                    }
                    else
                    {

                        var studentcurrentyearr = (from a in _buspasscontext.Adm_College_Yearly_StudentDMO
                                                   where (a.AMCST_Id == data.AMCST_Id)
                                                   select a
                    ).ToList().OrderByDescending(d => d.ACYST_Id).ToArray();
                        if (studentcurrentyearr.Length > 0)
                        {


                            using (var cmd = _buspasscontext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "CLG_TR_GET_TRNAPPLN_STUDENT_DETAILS";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                    SqlDbType.BigInt)
                                {
                                    Value = data.MI_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                                  SqlDbType.BigInt)
                                {
                                    Value = data.AMCST_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                   SqlDbType.BigInt)
                                {
                                    Value = data.studentaccyear
                                });
                                cmd.Parameters.Add(new SqlParameter("@FLAG",
                                  SqlDbType.VarChar)
                                {
                                    Value = "S"
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
                                    data.studentDetails = retObject.ToArray();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }







                            //        data.studentDetails = (from d in _buspasscontext.AcademicYearDMO
                            //                               from a in _buspasscontext.School_M_Class
                            //                               from b in _buspasscontext.School_M_Section
                            //                               from c in _buspasscontext.School_Adm_Y_StudentDMO
                            //                               from f in _buspasscontext.Adm_M_Student
                            //                               from g in _buspasscontext.country
                            //                               from h in _buspasscontext.state

                            //                               where (f.AMST_Id == c.AMST_Id && c.ASMCL_Id == a.ASMCL_Id && d.ASMAY_Id == c.ASMAY_Id && a.ASMCL_Id == c.ASMCL_Id && g.IVRMMC_Id == h.IVRMMC_Id && g.IVRMMC_Id == f.AMST_ConCountry && h.IVRMMS_Id == f.AMST_ConState && f.MI_Id == data.MI_Id && c.AMST_Id == data.AMST_Id && c.ASMAY_Id == data.studentaccyear
                            //                           //    f.AMST_Id == c.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id
                            //                           //    && g.IVRMMC_Id == f.AMST_PerCountry && h.IVRMMS_Id == f.AMST_PerState && g.IVRMMC_Id == h.IVRMMC_Id &&

                            //                           // a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id &&
                            //                           //c.AMST_Id == data.AMST_Id
                            //                           )
                            //                               select new CLGStudentBuspassFormDTO
                            //                               {
                            //                                   AMST_FirstName = ((f.AMST_FirstName == null || f.AMST_FirstName == "0" ? "" : f.AMST_FirstName) + " " + (f.AMST_MiddleName == null || f.AMST_MiddleName == "0" ? "" : f.AMST_MiddleName) + " " + (f.AMST_LastName == null || f.AMST_LastName == "0" ? "" : f.AMST_LastName)).Trim(),
                            //                                   ASMCL_Id = c.ASMCL_Id,
                            //                                   ASMCL_ClassName = a.ASMCL_ClassName,
                            //                                   ASMS_Id = c.ASMS_Id,
                            //                                   ASMC_SectionName = b.ASMC_SectionName,
                            //                                   ASMAY_Id = c.ASMAY_Id,
                            //                                   ASMAY_Year = d.ASMAY_Year,
                            //                                   AMST_AdmNo = f.AMST_AdmNo,
                            //                                   AMST_DOB = f.AMST_DOB,
                            //                                   AMST_emailId = f.AMST_emailId,
                            //                                   AMST_MobileNo = f.AMST_MobileNo,
                            //                                   AMST_PerStreet = f.AMST_ConStreet,
                            //                                   AMST_PerCity = f.AMST_ConCity,
                            //                                   AMST_PerArea = f.AMST_ConArea,
                            //                                   AMST_PerPincode = f.AMST_ConPincode,
                            //                                   IVRMMC_CountryName = g.IVRMMC_CountryName,
                            //                                   IVRMMS_Name = h.IVRMMS_Name,
                            //                                   AMST_FatherName = f.AMST_FatherName,
                            //                                   AMST_MotherName = f.AMST_MotherName,
                            //                                   AMST_Photoname = f.AMST_Photoname,
                            //                                   AMST_FatherOfficeAdd = f.AMST_FatherOfficeAdd,
                            //                                   IVRMMS_Id = h.IVRMMS_Id,
                            //                                   AMST_BloodGroup = f.AMST_BloodGroup,

                            //                               }
                            //).Distinct().ToArray();
                        }
                        else
                        {

                            using (var cmd = _buspasscontext.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.CommandText = "CLG_TR_GET_TRNAPPLN_STUDENT_DETAILS";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@MI_Id",
                                    SqlDbType.BigInt)
                                {
                                    Value = data.MI_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@AMCST_Id",
                                  SqlDbType.BigInt)
                                {
                                    Value = data.AMCST_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                                   SqlDbType.BigInt)
                                {
                                    Value = data.ASMAY_Id
                                });
                                cmd.Parameters.Add(new SqlParameter("@FLAG",
                                  SqlDbType.VarChar)
                                {
                                    Value = "N"
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
                                    data.studentDetails = retObject.ToArray();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }
                            //        data.studentDetails = (from d in _buspasscontext.AcademicYearDMO
                            //                               from a in _buspasscontext.School_M_Class
                            //                               from f in _buspasscontext.Adm_M_Student
                            //                               from g in _buspasscontext.country
                            //                               from h in _buspasscontext.state
                            //                               where (
                            //                               f.ASMAY_Id == d.ASMAY_Id && f.ASMCL_Id == a.ASMCL_Id && f.AMST_ConCountry == g.IVRMMC_Id && f.AMST_ConState == h.IVRMMS_Id && g.IVRMMC_Id == h.IVRMMC_Id && f.AMST_Id == data.AMST_Id && f.MI_Id == data.MI_Id


                            //                           //    a.ASMCL_Id == f.ASMCL_Id &&  d.ASMAY_Id == f.ASMAY_Id
                            //                           //    && g.IVRMMC_Id == f.AMST_PerCountry && h.IVRMMS_Id == f.AMST_PerState && g.IVRMMC_Id == h.IVRMMC_Id &&

                            //                           // a.MI_Id == data.MI_Id && f.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id &&
                            //                           //f.AMST_Id == data.AMST_Id
                            //                           )
                            //                               select new CLGStudentBuspassFormDTO
                            //                               {
                            //                                   AMST_FirstName = ((f.AMST_FirstName == null || f.AMST_FirstName == "0" ? "" : f.AMST_FirstName) + " " + (f.AMST_MiddleName == null || f.AMST_MiddleName == "0" ? "" : f.AMST_MiddleName) + " " + (f.AMST_LastName == null || f.AMST_LastName == "0" ? "" : f.AMST_LastName)).Trim(),
                            //                                   ASMCL_Id = f.ASMCL_Id,
                            //                                   ASMCL_ClassName = a.ASMCL_ClassName,
                            //                                   ASMAY_Id = f.ASMAY_Id,
                            //                                   ASMAY_Year = d.ASMAY_Year,
                            //                                   AMST_AdmNo = f.AMST_AdmNo,
                            //                                   AMST_DOB = f.AMST_DOB,
                            //                                   AMST_emailId = f.AMST_emailId,
                            //                                   AMST_MobileNo = f.AMST_MobileNo,
                            //                                   AMST_PerStreet = f.AMST_ConStreet,
                            //                                   AMST_PerCity = f.AMST_ConCity,
                            //                                   AMST_PerArea = f.AMST_ConArea,
                            //                                   AMST_PerPincode = f.AMST_ConPincode,
                            //                                   IVRMMC_CountryName = g.IVRMMC_CountryName,
                            //                                   IVRMMS_Name = h.IVRMMS_Name,
                            //                                   AMST_FatherName = f.AMST_FatherName,
                            //                                   AMST_MotherName = f.AMST_MotherName,
                            //                                   AMST_Photoname = f.AMST_Photoname,
                            //                                   AMST_FatherOfficeAdd = f.AMST_FatherOfficeAdd,
                            //                                   IVRMMS_Id = h.IVRMMS_Id,
                            //                                   AMST_BloodGroup = f.AMST_BloodGroup,
                            //                               }
                            //).Distinct().ToArray();

                        }

                    }


                    data.routeDetails = (from a in _buspasscontext.CLGAdm_Std_Transport_ApplicationDMO
                                         from d in _buspasscontext.Adm_Master_College_StudentDMO
                                         where (a.AMCST_Id == d.AMCST_Id && d.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && d.AMCST_ActiveFlag == true && d.AMCST_SOL == "S" && a.AMCST_Id == data.AMCST_Id)
                                         select new CLGStudentBuspassFormDTO
                                         {
                                             AMCST_Id = a.AMCST_Id,
                                             ASTACO_Id = a.ASTACO_Id,
                                             AMCST_FirstName = ((d.AMCST_FirstName == null || d.AMCST_FirstName == "0" ? "" : d.AMCST_FirstName) + " " + (d.AMCST_MiddleName == null || d.AMCST_MiddleName == "0" ? "" : d.AMCST_MiddleName) + " " + (d.AMCST_LastName == null || d.AMCST_LastName == "0" ? "" : d.AMCST_LastName)).Trim(),
                                             ASTA_PickUp_TRML_Id = a.ASTACO_PickUp_TRML_Id,
                                             TRML_PickLocationName = a.ASTACO_PickUp_TRML_Id != 0 ? _buspasscontext.MasterLocationDMO.Single(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTACO_PickUp_TRML_Id).TRML_LocationName : "--",

                                             ASTA_Drop_TRML_Id = a.ASTACO_Drop_TRML_Id,
                                             TRML_DropLocationName = a.ASTACO_Drop_TRML_Id != 0 ? _buspasscontext.MasterLocationDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTACO_Drop_TRML_Id).TRML_LocationName : "--",

                                             TRMR_Idp = a.ASTACO_PickUp_TRMR_Id,
                                             TRMR_PickRouteName = a.ASTACO_PickUp_TRMR_Id != 0 ? _buspasscontext.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTACO_PickUp_TRMR_Id).TRMR_RouteName : "--",

                                             TRMR_Idd = a.ASTACO_Drop_TRMR_Id,
                                             TRMR_DropRouteName = a.ASTACO_Drop_TRMR_Id != 0 ? _buspasscontext.MasterRouteDMO.Single(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTACO_Drop_TRMR_Id).TRMR_RouteName : "--"
                                         }
                           ).Distinct().ToArray();

                }
                else
                {
                    data.trnsportcutoffdate = "False";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public CLGStudentBuspassFormDTO getstudata1(CLGStudentBuspassFormDTO data)
        {
            //        try
            //        {
            //            data.countryid = _buspasscontext.country.ToArray();

            //            List<MasterAreaDMO> saa = new List<MasterAreaDMO>();
            //            saa = _buspasscontext.MasterAreaDMO.Where(r => r.MI_Id == data.MI_Id && r.TRMA_ActiveFlg == true).ToList();
            //            data.areaList = saa.ToArray();
            //            //route
            //            List<MasterRouteDMO> rout = new List<MasterRouteDMO>();
            //            rout = _buspasscontext.MasterRouteDMO.Where(r => r.MI_Id == data.MI_Id && r.TRMR_ActiveFlg == true).OrderBy(r => r.TRMR_order).ToList();
            //            data.routeList = rout.ToArray();
            //            // location
            //            List<MasterLocationDMO> locat = new List<MasterLocationDMO>();
            //            locat = _buspasscontext.MasterLocationDMO.Where(r => r.MI_Id == data.MI_Id && r.TRML_ActiveFlg == true).ToList();
            //            data.locaList = locat.ToArray();

            //            string rolename = _buspasscontext.IVRM_Role_Type.FirstOrDefault(t => t.IVRMRT_Id == data.roleId).IVRMRT_Role;

            //            List<Adm_Student_Transport_ApplicationDMO> trans = new List<Adm_Student_Transport_ApplicationDMO>();
            //            trans = _buspasscontext.Adm_Student_Transport_ApplicationDMO.Where(r => r.MI_Id == data.MI_Id && r.AMST_Id == data.AMST_Id).ToList();
            //            var trans_amstid = trans.ToArray();

            //            if (rolename == "Student")
            //            {
            //                if (trans_amstid.Length > 0)
            //                {
            //                    data.studentDetails = (from d in _buspasscontext.AcademicYearDMO
            //                                           from a in _buspasscontext.School_M_Class
            //                                           from b in _buspasscontext.School_M_Section
            //                                           from c in _buspasscontext.School_Adm_Y_StudentDMO
            //                                           from f in _buspasscontext.Adm_M_Student
            //                                           from g in _buspasscontext.country
            //                                           from h in _buspasscontext.state
            //                                           from i in _buspasscontext.MasterRouteDMO
            //                                           from j in _buspasscontext.MasterLocationDMO
            //                                           from k in _buspasscontext.Adm_Student_Transport_ApplicationDMO

            //                                           where (d.ASMAY_Id == f.ASMAY_Id && a.ASMCL_Id == f.ASMCL_Id && c.AMST_Id == f.AMST_Id && c.ASMS_Id == b.ASMS_Id
            //                                           && g.IVRMMC_Id == f.AMST_ConCountry && h.IVRMMS_Id == f.AMST_ConState && g.IVRMMC_Id == h.IVRMMC_Id &&
            //                                       i.TRMR_Id == k.ASTA_Drop_TRMR_Id && i.TRMR_Id == k.ASTA_PickUp_TRMR_Id && j.TRML_Id == k.ASTA_PickUp_TRML_Id && j.TRML_Id == k.ASTA_Drop_TRML_Id && f.MI_Id == data.MI_Id && f.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id &&
            //                                       f.AMST_Id == data.AMST_Id)
            //                                           select new CLGStudentBuspassFormDTO
            //                                           {
            //                                               AMST_FirstName = ((f.AMST_FirstName == null || f.AMST_FirstName == "0" ? "" : f.AMST_FirstName) + " " + (f.AMST_MiddleName == null || f.AMST_MiddleName == "0" ? "" : f.AMST_MiddleName) + " " + (f.AMST_LastName == null || f.AMST_LastName == "0" ? "" : f.AMST_LastName)).Trim(),
            //                                               ASMCL_Id = c.ASMCL_Id,
            //                                               ASMCL_ClassName = a.ASMCL_ClassName,
            //                                               ASMS_Id = c.ASMS_Id,
            //                                               AMST_BloodGroup = f.AMST_BloodGroup,
            //                                               ASMC_SectionName = b.ASMC_SectionName,
            //                                               ASMAY_Id = c.ASMAY_Id,
            //                                               ASMAY_Year = d.ASMAY_Year,
            //                                               AMST_AdmNo = f.AMST_AdmNo,
            //                                               AMST_DOB = f.AMST_DOB,
            //                                               AMST_emailId = f.AMST_emailId,
            //                                               AMST_MobileNo = f.AMST_MobileNo,
            //                                               AMST_PerStreet = f.AMST_ConStreet,
            //                                               AMST_PerCity = f.AMST_ConCity,
            //                                               AMST_PerArea = f.AMST_ConArea,
            //                                               AMST_PerPincode = f.AMST_ConPincode,
            //                                               IVRMMC_CountryName = g.IVRMMC_CountryName,
            //                                               IVRMMS_Name = h.IVRMMS_Name,
            //                                               AMST_FatherName = f.AMST_FatherName,
            //                                               AMST_MotherName = f.AMST_MotherName,
            //                                               AMST_FatherMobleNo = k.ASTA_FatherMobileNo,
            //                                               AMST_MotherMobileNo = k.ASTA_MotherMobileNo,
            //                                               AMST_FatherOfficeAdd = f.AMST_FatherOfficeAdd,
            //                                               ASTA_AreaZoneName = k.ASTA_AreaZoneName,
            //                                               ASTA_PickUp_TRML_Id = k.ASTA_PickUp_TRML_Id,
            //                                               ASTA_Drop_TRML_Id = k.ASTA_Drop_TRML_Id,
            //                                               ASTA_PickUp_TRMR_Id = k.ASTA_PickUp_TRMR_Id,
            //                                               ASTA_Drop_TRMR_Id = k.ASTA_Drop_TRMR_Id,
            //                                               ASTA_Landmark = k.ASTA_Landmark,
            //                                               ASTA_Phoneoff = k.ASTA_Phoneoff,
            //                                               ASTA_PhoneRes = k.ASTA_PhoneRes
            //                                           }
            //            ).Distinct().ToArray();

            //                    var mobilenos = (from a in _buspasscontext.Adm_Student_Transport_ApplicationDMO
            //                                     from b in _buspasscontext.Adm_M_Student
            //                                     where (a.AMST_Id == data.AMST_Id && a.AMST_Id == b.AMST_Id && a.ASTA_CurrentAY == data.ASMAY_Id)
            //                                     select new CLGStudentBuspassFormDTO
            //                                     {
            //                                         AMST_FatherMobleNo = a.ASTA_FatherMobileNo,
            //                                         AMST_MotherMobileNo = a.ASTA_MotherMobileNo,
            //                                         ASTA_Phoneoff = a.ASTA_Phoneoff,
            //                                         ASTA_PhoneRes = a.ASTA_PhoneRes,
            //                                         ASTA_Landmark = a.ASTA_Landmark,
            //                                         AMST_Photoname = b.AMST_Photoname
            //                                     }
            //  ).ToList().ToArray();

            //                    data.AMST_MotherMobileNo = mobilenos.FirstOrDefault().AMST_MotherMobileNo;
            //                    data.AMST_FatherMobleNo = mobilenos.FirstOrDefault().AMST_FatherMobleNo;
            //                    data.ASTA_Phoneoff = mobilenos.FirstOrDefault().ASTA_Phoneoff;
            //                    data.ASTA_PhoneRes = mobilenos.FirstOrDefault().ASTA_PhoneRes;
            //                    data.ASTA_Landmark = mobilenos.FirstOrDefault().ASTA_Landmark;
            //                    data.AMST_Photoname = mobilenos.FirstOrDefault().AMST_Photoname;

            //                    var studentdetails = (from d in _buspasscontext.AcademicYearDMO
            //                                          from a in _buspasscontext.School_M_Class
            //                                          from b in _buspasscontext.School_M_Section
            //                                          from c in _buspasscontext.School_Adm_Y_StudentDMO
            //                                          from f in _buspasscontext.Adm_M_Student
            //                                          from g in _buspasscontext.country
            //                                          from h in _buspasscontext.state
            //                                          from i in _buspasscontext.MasterRouteDMO
            //                                          from j in _buspasscontext.MasterLocationDMO
            //                                          from k in _buspasscontext.Adm_Student_Transport_ApplicationDMO
            //                                          where (d.ASMAY_Id == f.ASMAY_Id && a.ASMCL_Id == f.ASMCL_Id && c.AMST_Id == f.AMST_Id && c.ASMS_Id == b.ASMS_Id
            //                                           && g.IVRMMC_Id == f.AMST_ConCountry && h.IVRMMS_Id == f.AMST_ConState && g.IVRMMC_Id == h.IVRMMC_Id &&
            //                                       i.TRMR_Id == k.ASTA_Drop_TRMR_Id && i.TRMR_Id == k.ASTA_PickUp_TRMR_Id && j.TRML_Id == k.ASTA_PickUp_TRML_Id && j.TRML_Id == k.ASTA_Drop_TRML_Id && f.MI_Id == data.MI_Id && f.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id &&
            //                                      c.AMST_Id == data.AMST_Id)
            //                                          select new CLGStudentBuspassFormDTO
            //                                          {
            //                                              AMST_FirstName = ((f.AMST_FirstName == null || f.AMST_FirstName == "0" ? "" : f.AMST_FirstName) + " " + (f.AMST_MiddleName == null || f.AMST_MiddleName == "0" ? "" : f.AMST_MiddleName) + " " + (f.AMST_LastName == null || f.AMST_LastName == "0" ? "" : f.AMST_LastName)).Trim(),
            //                                              ASMCL_Id = c.ASMCL_Id,
            //                                              ASMCL_ClassName = a.ASMCL_ClassName,
            //                                              ASMS_Id = c.ASMS_Id,
            //                                              ASMC_SectionName = b.ASMC_SectionName,
            //                                              ASMAY_Id = c.ASMAY_Id,
            //                                              ASMAY_Year = d.ASMAY_Year,
            //                                              AMST_AdmNo = f.AMST_AdmNo,
            //                                              AMST_DOB = f.AMST_DOB,
            //                                              AMST_emailId = f.AMST_emailId,
            //                                              AMST_MobileNo = f.AMST_MobileNo,
            //                                              AMST_PerStreet = f.AMST_ConStreet,
            //                                              AMST_PerCity = f.AMST_ConCity,
            //                                              AMST_PerArea = f.AMST_ConArea,
            //                                              AMST_PerPincode = f.AMST_ConPincode,
            //                                              IVRMMC_CountryName = g.IVRMMC_CountryName,
            //                                              IVRMMS_Name = h.IVRMMS_Name,
            //                                              AMST_FatherName = f.AMST_FatherName,
            //                                              AMST_MotherName = f.AMST_MotherName,
            //                                              AMST_FatherMobleNo = k.ASTA_FatherMobileNo,
            //                                              AMST_MotherMobileNo = k.ASTA_MotherMobileNo,
            //                                              AMST_FatherOfficeAdd = f.AMST_FatherOfficeAdd,
            //                                              AMST_BloodGroup = f.AMST_BloodGroup,
            //                                              ASTA_AreaZoneName = k.ASTA_AreaZoneName,
            //                                              ASTA_PickUp_TRML_Id = k.ASTA_PickUp_TRML_Id,
            //                                              ASTA_Drop_TRML_Id = k.ASTA_Drop_TRML_Id,
            //                                              ASTA_PickUp_TRMR_Id = k.ASTA_PickUp_TRMR_Id,
            //                                              ASTA_Drop_TRMR_Id = k.ASTA_Drop_TRMR_Id,
            //                                              ASTA_Landmark = k.ASTA_Landmark,
            //                                              ASTA_Phoneoff = k.ASTA_Phoneoff,
            //                                              ASTA_PhoneRes = k.ASTA_PhoneRes
            //                                          }
            //                               ).Distinct().ToArray();



            //                    data.studentconstate = (from b in _buspasscontext.state
            //                                            from c in _buspasscontext.country
            //                                            where (c.IVRMMC_Id == b.IVRMMC_Id && c.IVRMMC_CountryName == studentdetails[0].IVRMMC_CountryName)
            //                                            select new CLGStudentBuspassFormDTO
            //                                            {
            //                                                IVRMMS_Id = b.IVRMMS_Id,
            //                                                IVRMMS_Name = b.IVRMMS_Name
            //                                            }).Distinct().ToArray();
            //                }
            //                else
            //                {
            //                    data.studentDetails = (from d in _buspasscontext.AcademicYearDMO
            //                                           from a in _buspasscontext.School_M_Class
            //                                           from b in _buspasscontext.School_M_Section
            //                                           from c in _buspasscontext.School_Adm_Y_StudentDMO
            //                                           from f in _buspasscontext.Adm_M_Student
            //                                           from g in _buspasscontext.country
            //                                           from h in _buspasscontext.state

            //                                           where (f.AMST_Id == c.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id
            //                                           && g.IVRMMC_Id == f.AMST_ConCountry && h.IVRMMS_Id == f.AMST_ConState && g.IVRMMC_Id == h.IVRMMC_Id &&

            //                                        a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id &&
            //                                       c.AMST_Id == data.AMST_Id)
            //                                           select new CLGStudentBuspassFormDTO
            //                                           {
            //                                               AMST_FirstName = ((f.AMST_FirstName == null || f.AMST_FirstName == "0" ? "" : f.AMST_FirstName) + " " + (f.AMST_MiddleName == null || f.AMST_MiddleName == "0" ? "" : f.AMST_MiddleName) + " " + (f.AMST_LastName == null || f.AMST_LastName == "0" ? "" : f.AMST_LastName)).Trim(),
            //                                               ASMCL_Id = c.ASMCL_Id,
            //                                               ASMCL_ClassName = a.ASMCL_ClassName,
            //                                               ASMS_Id = c.ASMS_Id,
            //                                               ASMC_SectionName = b.ASMC_SectionName,
            //                                               ASMAY_Id = c.ASMAY_Id,
            //                                               ASMAY_Year = d.ASMAY_Year,
            //                                               AMST_AdmNo = f.AMST_AdmNo,
            //                                               AMST_DOB = f.AMST_DOB,
            //                                               AMST_emailId = f.AMST_emailId,
            //                                               AMST_MobileNo = f.AMST_MobileNo,
            //                                               AMST_PerStreet = f.AMST_ConStreet,
            //                                               AMST_PerCity = f.AMST_ConCity,
            //                                               AMST_PerArea = f.AMST_ConArea,
            //                                               AMST_PerPincode = f.AMST_ConPincode,
            //                                               IVRMMC_CountryName = g.IVRMMC_CountryName,
            //                                               IVRMMS_Name = h.IVRMMS_Name,
            //                                               AMST_FatherName = f.AMST_FatherName,
            //                                               AMST_MotherName = f.AMST_MotherName,
            //                                               AMST_BloodGroup = f.AMST_BloodGroup,
            //                                               AMST_FatherOfficeAdd = f.AMST_FatherOfficeAdd,
            //                                           }
            //        ).Distinct().ToArray();


            //                    var mobilenos1 = (from a in _buspasscontext.Adm_M_Student
            //                                      where (a.AMST_Id == data.AMST_Id && a.MI_Id == data.MI_Id)
            //                                      select new CLGStudentBuspassFormDTO
            //                                      {
            //                                          AMST_Photoname = a.AMST_Photoname
            //                                      }
            //).ToList().ToArray();

            //                    data.AMST_Photoname = mobilenos1.FirstOrDefault().AMST_Photoname;

            //                    var studentdetails = (from d in _buspasscontext.AcademicYearDMO
            //                                          from a in _buspasscontext.School_M_Class
            //                                          from b in _buspasscontext.School_M_Section
            //                                          from c in _buspasscontext.School_Adm_Y_StudentDMO
            //                                          from f in _buspasscontext.Adm_M_Student
            //                                          from g in _buspasscontext.country
            //                                          from h in _buspasscontext.state

            //                                          where (f.AMST_Id == c.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id
            //                                          && g.IVRMMC_Id == f.AMST_ConCountry && h.IVRMMS_Id == f.AMST_ConState && g.IVRMMC_Id == h.IVRMMC_Id &&

            //                                       a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id &&
            //                                      c.AMST_Id == data.AMST_Id)
            //                                          select new CLGStudentBuspassFormDTO
            //                                          {
            //                                              AMST_FirstName = ((f.AMST_FirstName == null || f.AMST_FirstName == "0" ? "" : f.AMST_FirstName) + " " + (f.AMST_MiddleName == null || f.AMST_MiddleName == "0" ? "" : f.AMST_MiddleName) + " " + (f.AMST_LastName == null || f.AMST_LastName == "0" ? "" : f.AMST_LastName)).Trim(),
            //                                              ASMCL_Id = c.ASMCL_Id,
            //                                              ASMCL_ClassName = a.ASMCL_ClassName,
            //                                              ASMS_Id = c.ASMS_Id,
            //                                              ASMC_SectionName = b.ASMC_SectionName,
            //                                              ASMAY_Id = c.ASMAY_Id,
            //                                              ASMAY_Year = d.ASMAY_Year,
            //                                              AMST_AdmNo = f.AMST_AdmNo,
            //                                              AMST_DOB = f.AMST_DOB,
            //                                              AMST_emailId = f.AMST_emailId,
            //                                              AMST_MobileNo = f.AMST_MobileNo,
            //                                              AMST_PerStreet = f.AMST_ConStreet,
            //                                              AMST_PerCity = f.AMST_ConCity,
            //                                              AMST_PerArea = f.AMST_ConArea,
            //                                              AMST_PerPincode = f.AMST_ConPincode,
            //                                              IVRMMC_CountryName = g.IVRMMC_CountryName,
            //                                              IVRMMS_Name = h.IVRMMS_Name,
            //                                              AMST_FatherName = f.AMST_FatherName,
            //                                              AMST_MotherName = f.AMST_MotherName,

            //                                              AMST_FatherOfficeAdd = f.AMST_FatherOfficeAdd,
            //                                              AMST_BloodGroup = f.AMST_BloodGroup,
            //                                          }
            //                               ).Distinct().ToArray();

            //                    //  data.stutransapp = _buspasscontext.Adm_Student_Transport_ApplicationDMO.Where(r => r.MI_Id == data.MI_Id).ToArray();

            //                    data.studentconstate = (from b in _buspasscontext.state
            //                                            from c in _buspasscontext.country
            //                                            where (c.IVRMMC_Id == b.IVRMMC_Id && c.IVRMMC_CountryName == studentdetails[0].IVRMMC_CountryName)
            //                                            select new CLGStudentBuspassFormDTO
            //                                            {
            //                                                IVRMMS_Id = b.IVRMMS_Id,
            //                                                IVRMMS_Name = b.IVRMMS_Name
            //                                            }).Distinct().ToArray();
            //                }
            //            }
            //            else
            //            {
            //                if (trans_amstid.Length > 0)
            //                {
            //                    data.studentDetails = (from d in _buspasscontext.AcademicYearDMO
            //                                           from a in _buspasscontext.School_M_Class
            //                                           from b in _buspasscontext.School_M_Section
            //                                           from c in _buspasscontext.School_Adm_Y_StudentDMO
            //                                           from f in _buspasscontext.Adm_M_Student
            //                                           from g in _buspasscontext.country
            //                                           from h in _buspasscontext.state
            //                                           from i in _buspasscontext.MasterRouteDMO
            //                                           from j in _buspasscontext.MasterLocationDMO
            //                                           from k in _buspasscontext.Adm_Student_Transport_ApplicationDMO

            //                                           where (d.ASMAY_Id == f.ASMAY_Id && a.ASMCL_Id == f.ASMCL_Id && c.AMST_Id == f.AMST_Id && c.ASMS_Id == b.ASMS_Id
            //                                           && g.IVRMMC_Id == f.AMST_ConCountry && h.IVRMMS_Id == f.AMST_ConState && g.IVRMMC_Id == h.IVRMMC_Id &&
            //                                       i.TRMR_Id == k.ASTA_Drop_TRMR_Id && i.TRMR_Id == k.ASTA_PickUp_TRMR_Id && j.TRML_Id == k.ASTA_PickUp_TRML_Id && j.TRML_Id == k.ASTA_Drop_TRML_Id && f.MI_Id == data.MI_Id && f.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id)
            //                                           select new CLGStudentBuspassFormDTO
            //                                           {
            //                                               AMST_FirstName = ((f.AMST_FirstName == null || f.AMST_FirstName == "0" ? "" : f.AMST_FirstName) + " " + (f.AMST_MiddleName == null || f.AMST_MiddleName == "0" ? "" : f.AMST_MiddleName) + " " + (f.AMST_LastName == null || f.AMST_LastName == "0" ? "" : f.AMST_LastName)).Trim(),
            //                                               ASMCL_Id = c.ASMCL_Id,
            //                                               ASMCL_ClassName = a.ASMCL_ClassName,
            //                                               ASMS_Id = c.ASMS_Id,
            //                                               ASMC_SectionName = b.ASMC_SectionName,
            //                                               ASMAY_Id = c.ASMAY_Id,
            //                                               ASMAY_Year = d.ASMAY_Year,
            //                                               AMST_AdmNo = f.AMST_AdmNo,
            //                                               AMST_DOB = f.AMST_DOB,
            //                                               AMST_emailId = f.AMST_emailId,
            //                                               AMST_MobileNo = f.AMST_MobileNo,
            //                                               AMST_PerStreet = f.AMST_ConStreet,
            //                                               AMST_PerCity = f.AMST_ConCity,
            //                                               AMST_PerArea = f.AMST_ConArea,
            //                                               AMST_PerPincode = f.AMST_ConPincode,
            //                                               IVRMMC_CountryName = g.IVRMMC_CountryName,
            //                                               IVRMMS_Name = h.IVRMMS_Name,
            //                                               AMST_FatherName = f.AMST_FatherName,
            //                                               AMST_MotherName = f.AMST_MotherName,
            //                                               AMST_FatherMobleNo = k.ASTA_FatherMobileNo,
            //                                               AMST_MotherMobileNo = k.ASTA_MotherMobileNo,
            //                                               AMST_FatherOfficeAdd = f.AMST_FatherOfficeAdd,
            //                                               ASTA_AreaZoneName = k.ASTA_AreaZoneName,
            //                                               ASTA_PickUp_TRML_Id = k.ASTA_PickUp_TRML_Id,
            //                                               ASTA_Drop_TRML_Id = k.ASTA_Drop_TRML_Id,
            //                                               ASTA_PickUp_TRMR_Id = k.ASTA_PickUp_TRMR_Id,
            //                                               ASTA_Drop_TRMR_Id = k.ASTA_Drop_TRMR_Id,
            //                                               ASTA_Landmark = k.ASTA_Landmark,
            //                                               ASTA_Phoneoff = k.ASTA_Phoneoff,
            //                                               ASTA_PhoneRes = k.ASTA_PhoneRes,
            //                                               AMST_BloodGroup = f.AMST_BloodGroup,
            //                                           }
            //                   ).Distinct().ToArray();

            //                    var mobilenos = (from a in _buspasscontext.Adm_Student_Transport_ApplicationDMO
            //                                     where (a.AMST_Id == data.AMST_Id && a.ASTA_CurrentAY == data.ASMAY_Id)
            //                                     select new CLGStudentBuspassFormDTO
            //                                     {
            //                                         AMST_FatherMobleNo = a.ASTA_FatherMobileNo,
            //                                         AMST_MotherMobileNo = a.ASTA_MotherMobileNo,
            //                                         ASTA_PhoneRes = a.ASTA_PhoneRes,
            //                                         ASTA_Landmark = a.ASTA_Landmark

            //                                     }
            //  ).ToList().ToArray();

            //                    data.AMST_MotherMobileNo = mobilenos.FirstOrDefault().AMST_MotherMobileNo;
            //                    data.AMST_FatherMobleNo = mobilenos.FirstOrDefault().AMST_FatherMobleNo;
            //                    data.ASTA_Phoneoff = mobilenos.FirstOrDefault().ASTA_Phoneoff;
            //                    data.ASTA_PhoneRes = mobilenos.FirstOrDefault().ASTA_PhoneRes;
            //                    data.ASTA_Landmark = mobilenos.FirstOrDefault().ASTA_Landmark;

            //                    var studentdetails = (from d in _buspasscontext.AcademicYearDMO
            //                                          from a in _buspasscontext.School_M_Class
            //                                          from b in _buspasscontext.School_M_Section
            //                                          from c in _buspasscontext.School_Adm_Y_StudentDMO
            //                                          from f in _buspasscontext.Adm_M_Student
            //                                          from g in _buspasscontext.country
            //                                          from h in _buspasscontext.state
            //                                          from i in _buspasscontext.MasterRouteDMO
            //                                          from j in _buspasscontext.MasterLocationDMO
            //                                          from k in _buspasscontext.Adm_Student_Transport_ApplicationDMO
            //                                          where (d.ASMAY_Id == f.ASMAY_Id && a.ASMCL_Id == f.ASMCL_Id && c.AMST_Id == f.AMST_Id && c.ASMS_Id == b.ASMS_Id
            //                                           && g.IVRMMC_Id == f.AMST_ConCountry && h.IVRMMS_Id == f.AMST_ConState && g.IVRMMC_Id == h.IVRMMC_Id &&
            //                                       i.TRMR_Id == k.ASTA_Drop_TRMR_Id && i.TRMR_Id == k.ASTA_PickUp_TRMR_Id && j.TRML_Id == k.ASTA_PickUp_TRML_Id && j.TRML_Id == k.ASTA_Drop_TRML_Id && f.MI_Id == data.MI_Id && f.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id
            //                                     )
            //                                          select new CLGStudentBuspassFormDTO
            //                                          {
            //                                              AMST_FirstName = ((f.AMST_FirstName == null || f.AMST_FirstName == "0" ? "" : f.AMST_FirstName) + " " + (f.AMST_MiddleName == null || f.AMST_MiddleName == "0" ? "" : f.AMST_MiddleName) + " " + (f.AMST_LastName == null || f.AMST_LastName == "0" ? "" : f.AMST_LastName)).Trim(),
            //                                              ASMCL_Id = c.ASMCL_Id,
            //                                              ASMCL_ClassName = a.ASMCL_ClassName,
            //                                              ASMS_Id = c.ASMS_Id,
            //                                              ASMC_SectionName = b.ASMC_SectionName,
            //                                              ASMAY_Id = c.ASMAY_Id,
            //                                              ASMAY_Year = d.ASMAY_Year,
            //                                              AMST_AdmNo = f.AMST_AdmNo,
            //                                              AMST_DOB = f.AMST_DOB,
            //                                              AMST_emailId = f.AMST_emailId,
            //                                              AMST_MobileNo = f.AMST_MobileNo,
            //                                              AMST_PerStreet = f.AMST_ConStreet,
            //                                              AMST_PerCity = f.AMST_ConCity,
            //                                              AMST_PerArea = f.AMST_ConArea,
            //                                              AMST_PerPincode = f.AMST_ConPincode,
            //                                              IVRMMC_CountryName = g.IVRMMC_CountryName,
            //                                              IVRMMS_Name = h.IVRMMS_Name,
            //                                              AMST_FatherName = f.AMST_FatherName,
            //                                              AMST_MotherName = f.AMST_MotherName,
            //                                              AMST_FatherMobleNo = k.ASTA_FatherMobileNo,
            //                                              AMST_MotherMobileNo = k.ASTA_MotherMobileNo,
            //                                              AMST_FatherOfficeAdd = f.AMST_FatherOfficeAdd,
            //                                              AMST_BloodGroup = f.AMST_BloodGroup,
            //                                              ASTA_AreaZoneName = k.ASTA_AreaZoneName,
            //                                              ASTA_PickUp_TRML_Id = k.ASTA_PickUp_TRML_Id,
            //                                              ASTA_Drop_TRML_Id = k.ASTA_Drop_TRML_Id,
            //                                              ASTA_PickUp_TRMR_Id = k.ASTA_PickUp_TRMR_Id,
            //                                              ASTA_Drop_TRMR_Id = k.ASTA_Drop_TRMR_Id,
            //                                              ASTA_Landmark = k.ASTA_Landmark,
            //                                              ASTA_Phoneoff = k.ASTA_Phoneoff,
            //                                              ASTA_PhoneRes = k.ASTA_PhoneRes
            //                                          }
            //                               ).Distinct().ToArray();

            //                    //  data.stutransapp = _buspasscontext.Adm_Student_Transport_ApplicationDMO.Where(r => r.MI_Id == data.MI_Id).ToArray();

            //                    data.studentconstate = (from b in _buspasscontext.state
            //                                            from c in _buspasscontext.country
            //                                            where (c.IVRMMC_Id == b.IVRMMC_Id && c.IVRMMC_CountryName == studentdetails[0].IVRMMC_CountryName)
            //                                            select new CLGStudentBuspassFormDTO
            //                                            {
            //                                                IVRMMS_Id = b.IVRMMS_Id,
            //                                                IVRMMS_Name = b.IVRMMS_Name
            //                                            }).Distinct().ToArray();
            //                }
            //                else
            //                {
            //                    data.studentDetails = (from d in _buspasscontext.AcademicYearDMO
            //                                           from a in _buspasscontext.School_M_Class
            //                                           from b in _buspasscontext.School_M_Section
            //                                           from c in _buspasscontext.School_Adm_Y_StudentDMO
            //                                           from f in _buspasscontext.Adm_M_Student
            //                                           from g in _buspasscontext.country
            //                                           from h in _buspasscontext.state

            //                                           where (f.AMST_Id == c.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id
            //                                           && g.IVRMMC_Id == f.AMST_ConCountry && h.IVRMMS_Id == f.AMST_ConState && g.IVRMMC_Id == h.IVRMMC_Id &&

            //                                        a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id &&
            //                                       c.AMST_Id == data.AMST_Id)
            //                                           select new CLGStudentBuspassFormDTO
            //                                           {
            //                                               AMST_FirstName = ((f.AMST_FirstName == null || f.AMST_FirstName == "0" ? "" : f.AMST_FirstName) + " " + (f.AMST_MiddleName == null || f.AMST_MiddleName == "0" ? "" : f.AMST_MiddleName) + " " + (f.AMST_LastName == null || f.AMST_LastName == "0" ? "" : f.AMST_LastName)).Trim(),
            //                                               ASMCL_Id = c.ASMCL_Id,
            //                                               ASMCL_ClassName = a.ASMCL_ClassName,
            //                                               ASMS_Id = c.ASMS_Id,
            //                                               ASMC_SectionName = b.ASMC_SectionName,
            //                                               ASMAY_Id = c.ASMAY_Id,
            //                                               ASMAY_Year = d.ASMAY_Year,
            //                                               AMST_AdmNo = f.AMST_AdmNo,
            //                                               AMST_DOB = f.AMST_DOB,
            //                                               AMST_emailId = f.AMST_emailId,
            //                                               AMST_MobileNo = f.AMST_MobileNo,
            //                                               AMST_PerStreet = f.AMST_ConStreet,
            //                                               AMST_PerCity = f.AMST_ConCity,
            //                                               AMST_PerArea = f.AMST_ConArea,
            //                                               AMST_PerPincode = f.AMST_ConPincode,
            //                                               IVRMMC_CountryName = g.IVRMMC_CountryName,
            //                                               IVRMMS_Name = h.IVRMMS_Name,
            //                                               AMST_FatherName = f.AMST_FatherName,
            //                                               AMST_MotherName = f.AMST_MotherName,

            //                                               AMST_FatherOfficeAdd = f.AMST_FatherOfficeAdd,
            //                                               AMST_BloodGroup = f.AMST_BloodGroup,
            //                                           }
            //        ).Distinct().ToArray();

            //                    var studentdetails = (from d in _buspasscontext.AcademicYearDMO
            //                                          from a in _buspasscontext.School_M_Class
            //                                          from b in _buspasscontext.School_M_Section
            //                                          from c in _buspasscontext.School_Adm_Y_StudentDMO
            //                                          from f in _buspasscontext.Adm_M_Student
            //                                          from g in _buspasscontext.country
            //                                          from h in _buspasscontext.state

            //                                          where (f.AMST_Id == c.AMST_Id && a.ASMCL_Id == c.ASMCL_Id && b.ASMS_Id == c.ASMS_Id && d.ASMAY_Id == c.ASMAY_Id
            //                                          && g.IVRMMC_Id == f.AMST_ConCountry && h.IVRMMS_Id == f.AMST_ConState && g.IVRMMC_Id == h.IVRMMC_Id &&

            //                                       a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && d.MI_Id == data.MI_Id && d.ASMAY_Id == data.ASMAY_Id && f.MI_Id == data.MI_Id &&
            //                                      c.AMST_Id == data.AMST_Id)
            //                                          select new CLGStudentBuspassFormDTO
            //                                          {
            //                                              AMST_FirstName = ((f.AMST_FirstName == null || f.AMST_FirstName == "0" ? "" : f.AMST_FirstName) + " " + (f.AMST_MiddleName == null || f.AMST_MiddleName == "0" ? "" : f.AMST_MiddleName) + " " + (f.AMST_LastName == null || f.AMST_LastName == "0" ? "" : f.AMST_LastName)).Trim(),
            //                                              ASMCL_Id = c.ASMCL_Id,
            //                                              ASMCL_ClassName = a.ASMCL_ClassName,
            //                                              ASMS_Id = c.ASMS_Id,
            //                                              ASMC_SectionName = b.ASMC_SectionName,
            //                                              ASMAY_Id = c.ASMAY_Id,
            //                                              ASMAY_Year = d.ASMAY_Year,
            //                                              AMST_AdmNo = f.AMST_AdmNo,
            //                                              AMST_DOB = f.AMST_DOB,
            //                                              AMST_emailId = f.AMST_emailId,
            //                                              AMST_MobileNo = f.AMST_MobileNo,
            //                                              AMST_PerStreet = f.AMST_ConStreet,
            //                                              AMST_PerCity = f.AMST_ConCity,
            //                                              AMST_PerArea = f.AMST_ConArea,
            //                                              AMST_PerPincode = f.AMST_ConPincode,
            //                                              IVRMMC_CountryName = g.IVRMMC_CountryName,
            //                                              IVRMMS_Name = h.IVRMMS_Name,
            //                                              AMST_FatherName = f.AMST_FatherName,
            //                                              AMST_MotherName = f.AMST_MotherName,
            //                                              AMST_FatherOfficeAdd = f.AMST_FatherOfficeAdd,
            //                                              AMST_BloodGroup = f.AMST_BloodGroup,
            //                                          }
            //                               ).Distinct().ToArray();

            //                    //  data.stutransapp = _buspasscontext.Adm_Student_Transport_ApplicationDMO.Where(r => r.MI_Id == data.MI_Id).ToArray();

            //                    data.studentconstate = (from b in _buspasscontext.state
            //                                            from c in _buspasscontext.country
            //                                            where (c.IVRMMC_Id == b.IVRMMC_Id && c.IVRMMC_CountryName == studentdetails[0].IVRMMC_CountryName)
            //                                            select new CLGStudentBuspassFormDTO
            //                                            {
            //                                                IVRMMS_Id = b.IVRMMS_Id,
            //                                                IVRMMS_Name = b.IVRMMS_Name
            //                                            }).Distinct().ToArray();
            //                }
            //            }

            //            data.routeDetails = (from a in _buspasscontext.Adm_Student_Transport_ApplicationDMO
            //                                 from d in _buspasscontext.Adm_M_Student
            //                                 where (a.AMST_Id == d.AMST_Id && d.MI_Id == data.MI_Id && d.MI_Id == a.MI_Id && d.AMST_ActiveFlag == 1 && d.AMST_SOL == "S" && a.AMST_Id == data.AMST_Id)
            //                                 select new CLGStudentBuspassFormDTO
            //                                 {
            //                                     AMST_Id = a.AMST_Id,
            //                                     ASTA_Id = a.ASTA_Id,
            //                                     AMST_FirstName = ((d.AMST_FirstName == null || d.AMST_FirstName == "0" ? "" : d.AMST_FirstName) + " " + (d.AMST_MiddleName == null || d.AMST_MiddleName == "0" ? "" : d.AMST_MiddleName) + " " + (d.AMST_LastName == null || d.AMST_LastName == "0" ? "" : d.AMST_LastName)).Trim(),
            //                                     ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
            //                                     TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _buspasscontext.MasterLocationDMO.Single(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).TRML_LocationName : "--",

            //                                     ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
            //                                     TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _buspasscontext.MasterLocationDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).TRML_LocationName : "--",

            //                                     TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
            //                                     TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _buspasscontext.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).TRMR_RouteName : "--",

            //                                     TRMR_Idd = a.ASTA_Drop_TRMR_Id,
            //                                     TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _buspasscontext.MasterRouteDMO.Single(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).TRMR_RouteName : "--"
            //                                 }
            //                   ).Distinct().ToArray();
            //        }
            //        catch (Exception ex)
            //        {
            //            Console.WriteLine(ex.Message);
            //        }
            return data;
        }
        public CLGStudentBuspassFormDTO getroutedata(CLGStudentBuspassFormDTO data)
        {
            try
            {
                data.routelist = (from a in _buspasscontext.MasterAreaDMO
                                  from b in _buspasscontext.MasterRouteDMO
                                  where (a.TRMA_Id == b.TRMA_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && a.TRMA_ActiveFlg == true && b.TRMR_ActiveFlg == true)
                                  select new CLGStudentBuspassFormDTO
                                  {
                                      TRMR_Id = b.TRMR_Id,
                                      TRMR_RouteName = b.TRMR_RouteName,
                                      TRMR_RouteNo = b.TRMR_RouteNo,
                                      TRMR_order = b.TRMR_order
                                  }
                 ).ToList().OrderBy(f => f.TRMR_order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CLGStudentBuspassFormDTO getlocationdata(CLGStudentBuspassFormDTO data)
        {
            try
            {
                data.locationlist = (from a in _buspasscontext.Route_Location
                                     from b in _buspasscontext.MasterRouteDMO
                                     from c in _buspasscontext.MasterLocationDMO
                                     where (a.TRMR_Id == b.TRMR_Id && a.TRML_Id == c.TRML_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id && a.TRMRL_ActiveFlag == true && b.TRMR_ActiveFlg == true && c.TRML_ActiveFlg == true)
                                     select new CLGStudentBuspassFormDTO
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

        public CLGStudentBuspassFormDTO getlocationdataonly(CLGStudentBuspassFormDTO data)
        {
            try
            {
                data.locationlist = (from a in _buspasscontext.Route_Location
                                     from b in _buspasscontext.MasterRouteDMO
                                     from c in _buspasscontext.MasterLocationDMO
                                     where (a.TRMR_Id == b.TRMR_Id && a.TRML_Id == c.TRML_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id && a.TRMR_Id == data.TRMR_Id && a.TRMRL_ActiveFlag == true && b.TRMR_ActiveFlg == true && c.TRML_ActiveFlg == true)
                                     select new CLGStudentBuspassFormDTO
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

        public async Task<CLGStudentBuspassFormDTO> getbuspassdata(CLGStudentBuspassFormDTO data)
        {
            try
            {
                var studentcurrentyear = (from a in _buspasscontext.Adm_College_Yearly_StudentDMO
                                          where (a.AMCST_Id == data.AMCST_Id)
                                          select a
        ).ToList().OrderByDescending(d => d.ACYST_Id).ToArray();

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
                    var studentadmityear = (from a in _buspasscontext.Adm_Master_College_StudentDMO
                                            where (a.AMCST_Id == data.AMCST_Id)
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
                // data.studentaccyear = data.ASMAY_Id;
                using (var cmd = _buspasscontext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "BUSPASS_FORM_DETAILS_COLLAGE";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@amst",
                SqlDbType.VarChar)
                    {
                        Value = data.AMCST_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@asta",
               SqlDbType.VarChar)
                    {
                        Value = data.ASTACO_Id
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


                var regularnewff = (from f in _buspasscontext.CLGAdm_Std_Transport_ApplicationDMO
                                    where (f.AMCST_Id == data.AMCST_Id && f.ASTACO_CurrentAY == data.ASMAY_Id)
                                    select new CLGStudentBuspassFormDTO
                                    {
                                        classnextid = f.ASTACO_ForAY,
                                        ASTA_ApplStatus = f.ASTACO_ApplStatus
                                    }
        ).ToList();



                List<AcademicYear> acayr = new List<AcademicYear>();
                acayr = _buspasscontext.AcademicYearDMO.Where(r => r.MI_Id == data.MI_Id && r.Is_Active == true).ToList();
                data.fillyear = acayr.ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CLGStudentBuspassFormDTO savedata(CLGStudentBuspassFormDTO data)
        {
            try
            {
                long courseid = 0;
                long branchid = 0;

                if (data.TRMR_Idd == null)
                {
                    data.TRMR_Idd = 0;
                }
                if (data.TRMR_Idp == null)
                {
                    data.TRMR_Idp = 0;
                }

                var updaterecordcount = _buspasscontext.CLGAdm_Std_Transport_ApplicationDMO.Where(t => t.MI_Id == data.MI_Id && t.ASTACO_Id == data.ASTACO_Id && t.AMCST_Id == data.AMCST_Id && t.ASTACO_ForAY == data.ASMAY_Id).Count();
                if (updaterecordcount > 0)
                {
                    var updaterecord = _buspasscontext.CLGAdm_Std_Transport_ApplicationDMO.Single(t => t.MI_Id == data.MI_Id && t.ASTACO_Id == data.ASTACO_Id && t.AMCST_Id == data.AMCST_Id && t.ASTACO_ForAY == data.ASMAY_Id);

                    updaterecord.MI_Id = data.MI_Id;
                    updaterecord.AMCST_Id = data.AMCST_Id;

                    updaterecord.ASTACO_ForAY = data.transportyear;
                    updaterecord.ASTACO_AreaZoneName = data.TRMA_AreaName;
                    updaterecord.TRMA_Id = Convert.ToInt64(data.TRMA_Id);
                    updaterecord.ASTACO_PickUp_TRMR_Id = Convert.ToInt64(data.TRMR_Idp);
                    updaterecord.ASTACO_PickUp_TRML_Id = Convert.ToInt64(data.TRML_Idp);
                    updaterecord.ASTACO_PickUp_TRMS_Id = 0;
                    updaterecord.ASTACO_Drop_TRMR_Id = Convert.ToInt64(data.TRMR_Idd);
                    updaterecord.ASTACO_Drop_TRML_Id = Convert.ToInt64(data.TRML_Idd);
                    updaterecord.ASTACO_Drop_TRMS_Id = 0;
                    updaterecord.ASTACO_PickupSMSMobileNo = data.ASTA_FatherMobileNo;
                    updaterecord.ASTACO_DropSMSMobileNo = data.ASTA_MotherMobileNo;
                    updaterecord.ASTACO_ApplStatus = "Waiting";
                    updaterecord.ASTACO_ActiveFlag = true;
                    updaterecord.ASTACO_Landmark = data.ASTA_Landmark;
                    updaterecord.ASTACO_Phoneoff = data.ASTA_Phoneoff;
                    updaterecord.ASTACO_PhoneRes = data.ASTA_PhoneRes;
                    updaterecord.UpdatedDate = DateTime.Now;
                    _buspasscontext.Update(updaterecord);
                    var flag = _buspasscontext.SaveChanges();
                    if (flag > 0)
                    {
                        data.returnval = "Update";
                    }
                    else
                    {
                        data.returnval = "NotUpdate";
                    }
                }
                else
                {


                    var studentcurrentyear = (from a in _buspasscontext.Adm_College_Yearly_StudentDMO
                                              where (a.AMCST_Id == data.AMCST_Id)
                                              select a
                          ).ToList().OrderByDescending(d => d.ACYST_Id).ToArray();

                    if (studentcurrentyear.Length > 0)
                    {
                        courseid = studentcurrentyear[0].AMCO_Id;
                        branchid = studentcurrentyear[0].AMB_Id;


                        if (studentcurrentyear.FirstOrDefault().ASMAY_Id != data.transportyear)
                        {
                            data.studentaccyear = studentcurrentyear.FirstOrDefault().ASMAY_Id;
                            data.studentsem = studentcurrentyear.FirstOrDefault().AMSE_Id;

                            var cls_orderid = (from a in _buspasscontext.CLG_Adm_Master_SemesterDMO
                                               where (a.AMSE_Id == data.studentsem && a.MI_Id == data.MI_Id)
                                               select new CLGStudentBuspassFormDTO
                                               {
                                                   cls_Order = a.AMSE_SEMOrder + 1
                                               }
                 ).ToList().ToArray();

                            var class_Id = (from a in _buspasscontext.CLG_Adm_Master_SemesterDMO
                                            where (a.AMSE_SEMOrder == cls_orderid[0].cls_Order && a.MI_Id == data.MI_Id)
                                            select new CLGStudentBuspassFormDTO
                                            {
                                                cls_Id = a.AMSE_Id
                                            }
                             ).ToList().ToArray();

                            data.studentfuturesem = class_Id.FirstOrDefault().cls_Id;
                        }
                        else
                        {
                            data.studentaccyear = data.transportyear;
                            data.studentsem = studentcurrentyear.FirstOrDefault().AMSE_Id;
                            data.studentfuturesem = studentcurrentyear.FirstOrDefault().AMSE_Id;
                        }

                    }

                    else
                    {
                        var studentadmityear = (from a in _buspasscontext.Adm_Master_College_StudentDMO
                                                where (a.AMCST_Id == data.AMCST_Id)
                                                select a
                     ).ToList().ToArray();


                        courseid = Convert.ToInt64(studentadmityear[0].AMCO_Id);
                        branchid = studentadmityear[0].AMB_Id;

                        if (studentadmityear.FirstOrDefault().ASMAY_Id != data.transportyear)
                        {
                            data.studentaccyear = studentadmityear.FirstOrDefault().ASMAY_Id;
                            data.studentsem = studentadmityear.FirstOrDefault().AMSE_Id;
                            var cls_orderid = (from a in _buspasscontext.CLG_Adm_Master_SemesterDMO
                                               where (a.AMSE_Id == data.studentsem && a.MI_Id == data.MI_Id)
                                               select new CLGStudentBuspassFormDTO
                                               {
                                                   cls_Order = a.AMSE_SEMOrder + 1
                                               }
                   ).ToList().ToArray();

                            var class_Id = (from a in _buspasscontext.CLG_Adm_Master_SemesterDMO
                                            where (a.AMSE_SEMOrder == cls_orderid[0].cls_Order && a.MI_Id == data.MI_Id)
                                            select new CLGStudentBuspassFormDTO
                                            {
                                                cls_Id = a.AMSE_Id
                                            }
                             ).ToList().ToArray();

                            data.studentfuturesem = class_Id.FirstOrDefault().cls_Id;
                        }
                        else
                        {
                            data.studentaccyear = data.transportyear;
                            data.studentsem = studentadmityear.FirstOrDefault().AMSE_Id;
                            data.studentfuturesem = studentadmityear.FirstOrDefault().AMSE_Id;
                        }
                    }





                    var duplicatecount = _buspasscontext.CLGAdm_Std_Transport_ApplicationDMO.Where(t => t.MI_Id == data.MI_Id && t.AMCST_Id == data.AMCST_Id
                    && t.ASTACO_ForAY == data.ASMAY_Id).Count();
                    if (duplicatecount == 0)
                    {
                        if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                        {
                            GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
                            data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                            data.transnumbconfigurationsettingsss.ASMAY_Id = data.transportyear;
                            data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);
                        }


                        //praveen added
                        var check_regnew = _buspasscontext.Adm_Master_College_StudentDMO.Where(t => t.MI_Id == data.MI_Id && t.AMCST_Id == data.AMCST_Id && t.AMCST_ActiveFlag == true && t.AMCST_SOL == "S" && t.ASMAY_Id == data.transportyear).ToList();
                        if (check_regnew.Count > 0)
                        {
                            data.newregular = "New";
                        }
                        else
                        {
                            var year_order = (from a in _buspasscontext.AcademicYear
                                              where (a.MI_Id == data.MI_Id && a.ASMAY_Id == data.transportyear && a.Is_Active == true)
                                              select new CLGStudentBuspassFormDTO
                                              {
                                                  ASMAY_Order = a.ASMAY_Order - 1
                                              }).ToList();
                            var year_order_id = (from a in _buspasscontext.AcademicYear
                                                 where (a.MI_Id == data.MI_Id && a.ASMAY_Order == year_order[0].ASMAY_Order && a.Is_Active == true)
                                                 select new CLGStudentBuspassFormDTO
                                                 {
                                                     ASMAY_Id = a.ASMAY_Id
                                                 }).ToList();


                            data.regularnew = (from a in _buspasscontext.FeeGroupDMO
                                               from b in _buspasscontext.FeeYearlygroupHeadMappingDMO
                                               from c in _buspasscontext.Fee_College_Student_StatusDMO
                                               from d in _buspasscontext.FeeHeadDMO
                                               where (b.FMG_Id == a.FMG_Id && b.FMH_Id == d.FMH_Id && c.FMG_Id == b.FMG_Id && c.FMH_Id == b.FMH_Id && c.FMG_Id == a.FMG_Id && c.FMH_Id == d.FMH_Id && c.ASMAY_Id == b.ASMAY_Id && d.FMH_Flag == "T" && c.MI_Id == data.MI_Id && c.ASMAY_Id == year_order_id[0].ASMAY_Id && c.FCSS_CurrentYrCharges > 0 && c.AMCST_Id == data.AMCST_Id)
                                               select new CLGStudentBuspassFormDTO
                                               {
                                                   AMCST_Id = c.AMCST_Id
                                               }).ToList().ToArray();

                            if (data.regularnew.Length > 0)
                            {
                                data.newregular = "Regular";
                            }
                            else
                            {
                                data.newregular = "New";
                            }
                        }



                        //  Adm_Student_Transport_ApplicationDMO admiss = Mapper.Map<Adm_Student_Transport_ApplicationDMO>(data);

                        CLGAdm_Std_Transport_ApplicationDMO admiss = new CLGAdm_Std_Transport_ApplicationDMO();
                        admiss.MI_Id = data.MI_Id;
                        admiss.AMCST_Id = data.AMCST_Id;
                        admiss.ASTACO_CurrentAY = Convert.ToInt64(data.studentaccyear);
                        admiss.ASTACO_CurrentCourse = courseid;
                        admiss.ASTACO_CurrentBranch = branchid;
                        admiss.ASTACO_CurrentSemester = Convert.ToInt64(data.studentsem);
                        admiss.ASTACO_ForAY = data.transportyear;
                        admiss.ASTACO_ForSemester = Convert.ToInt64(data.studentfuturesem);
                        admiss.ASTACO_ApplicationNo = data.trans_id;
                        admiss.ASTACO_ApplicationDate = DateTime.Now;
                        admiss.ASTACO_AreaZoneName = data.TRMA_AreaName;
                        admiss.TRMA_Id = Convert.ToInt64(data.TRMA_Id);
                        admiss.ASTACO_PickUp_TRMR_Id = Convert.ToInt64(data.TRMR_Idp);
                        admiss.ASTACO_PickUp_TRML_Id = Convert.ToInt64(data.TRML_Idp);
                        admiss.ASTACO_PickUp_TRMS_Id = 0;
                        admiss.ASTACO_Drop_TRMR_Id = Convert.ToInt64(data.TRMR_Idd);
                        admiss.ASTACO_Drop_TRML_Id = Convert.ToInt64(data.TRML_Idd);
                        admiss.ASTACO_Drop_TRMS_Id = 0;
                        admiss.ASTACO_PickupSMSMobileNo = data.ASTA_FatherMobileNo;
                        admiss.ASTACO_DropSMSMobileNo = data.ASTA_MotherMobileNo;
                        admiss.ASTACO_ApplStatus = "Waiting";
                        admiss.ASTACO_ActiveFlag = true;
                        admiss.ASTACO_Regnew = data.newregular;
                        admiss.ASTACO_Landmark = data.ASTA_Landmark;
                        admiss.ASTACO_Phoneoff = data.ASTA_Phoneoff;
                        admiss.ASTACO_PhoneRes = data.ASTA_PhoneRes;
                        admiss.CreatedDate = DateTime.Now;
                        admiss.UpdatedDate = DateTime.Now;
                        _buspasscontext.Add(admiss);
                        List<Adm_Master_College_StudentDMO> studentdetails = new List<Adm_Master_College_StudentDMO>();
                        studentdetails = _buspasscontext.Adm_Master_College_StudentDMO.Where(t => t.AMCST_Id == data.AMCST_Id).ToList();


                        if (studentdetails.Count() > 0)
                        {

                            data.ASMAY_Id = studentdetails.FirstOrDefault().ASMAY_Id;
                            data.AMST_FatherName = studentdetails.FirstOrDefault().AMCST_FirstName;
                            data.AMST_MobileNo = studentdetails.FirstOrDefault().AMCST_MobileNo;
                            data.AMST_emailId = studentdetails.FirstOrDefault().AMCST_emailId;

                            data.paymentapplicable = "Pay";
                            data.payementcheck = (from a in _buspasscontext.Clg_Fee_AmountEntry_DMO
                                                  from b in _buspasscontext.Fee_T_College_PaymentDMO
                                                  from d in _buspasscontext.FeeGroupDMO
                                                  from f in _buspasscontext.FeeHeadDMO
                                                  from g in _buspasscontext.CLG_Fee_College_Master_Amount_Semesterwise
                                                      // from g in _buspasscontext.feeYCCC
                                                      //  from h in _buspasscontext.feeYCC
                                                  from i in _buspasscontext.Fee_Y_Payment_College_Student
                                                  where (a.FMG_Id == d.FMG_Id && a.FMH_Id == f.FMH_Id && g.FCMA_Id == a.FCMA_Id && a.FCMA_Id == g.FCMA_Id && a.ASMAY_Id == a.ASMAY_Id && i.FYP_Id == b.FYP_Id && f.FMH_Flag == "T" && d.FMG_CompulsoryFlag == "T" && i.AMCST_Id == data.AMCST_Id)
                                                  select new CLGStudentBuspassFormDTO
                                                  {
                                                      FCMA_Id = g.FCMA_Id,
                                                      FMA_Amount = g.FCMAS_Amount
                                                  }
               ).Count();

                            //if (data.payementcheck == 0)
                            //{
                            //    data.paydet = paymentPart(data);
                            //}

                        }

                        int contactExists = _buspasscontext.SaveChanges();
                        //int contactExists = 0;
                        if (contactExists > 0)
                        {
                            data.returnval = "true";
                            var admissphoto = _buspasscontext.Adm_Master_College_StudentDMO.Single(t => t.MI_Id == data.MI_Id && t.AMCST_Id == data.AMCST_Id);

                            admissphoto.AMCST_StudentPhoto = data.AMST_Photoname;
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

                        var admissstatus = _buspasscontext.CLGAdm_Std_Transport_ApplicationDMO.Where(t => t.MI_Id == data.MI_Id && t.AMCST_Id == data.AMCST_Id && t.ASTACO_ApplStatus == "Approved" && t.ASTACO_ForAY == data.ASMAY_Id).Count();
                        if (admissstatus == 0)
                        {

                            //data.message = "Duplicate";
                            ////Adm_Student_Transport_ApplicationDMO admiss = Mapper.Map<Adm_Student_Transport_ApplicationDMO>(data);
                            //var admiss = _buspasscontext.Adm_Student_Transport_ApplicationDMO.Single(t => t.MI_Id == data.MI_Id && t.AMST_Id == data.AMST_Id && t.ASTA_FutureAY == data.ASMAY_Id);

                            //data.ASTA_ApplStatus = admiss.ASTA_ApplStatus;
                            //data.ASTA_ActiveFlag = admiss.ASTA_ActiveFlag;
                            //admiss.ASTA_FutureAY = data.transportyear;
                            //admiss.ASTA_CurrentAY = Convert.ToInt64(data.studentaccyear);
                            //admiss.ASTA_CurrentClass = Convert.ToInt64(data.studentclass);
                            //admiss.ASTA_FutureClass = Convert.ToInt64(data.studentfutureclass);
                            //admiss.ASTA_ApplicationDate = DateTime.Now;
                            //admiss.ASTA_ActiveFlag = data.ASTA_ActiveFlag;
                            //admiss.ASTA_Landmark = data.ASTA_Landmark;
                            //admiss.ASTA_ApplStatus = data.ASTA_ApplStatus;
                            //admiss.ASTA_AreaZoneName = data.TRMA_AreaName;
                            //admiss.ASTA_FatherMobileNo = data.ASTA_FatherMobileNo;
                            //admiss.ASTA_MotherMobileNo = data.ASTA_MotherMobileNo;
                            //admiss.ASTA_PickUp_TRML_Id = Convert.ToInt64(data.TRML_Idp);
                            //admiss.ASTA_Drop_TRML_Id = Convert.ToInt64(data.TRML_Idd);
                            //admiss.ASTA_Drop_TRMR_Id = Convert.ToInt64(data.TRMR_Idd);
                            //admiss.ASTA_PickUp_TRMR_Id = Convert.ToInt64(data.TRMR_Idp);
                            //admiss.ASTA_PhoneRes = data.ASTA_PhoneRes;
                            //admiss.ASTA_Phoneoff = data.ASTA_Phoneoff;
                            //// admiss.CreatedDate = DateTime.Now;
                            //admiss.UpdatedDate = DateTime.Now;

                            //_buspasscontext.Update(admiss);
                            List<Adm_M_Student> studentdetails = new List<Adm_M_Student>();
                            studentdetails = _buspasscontext.Adm_M_Student.Where(t => t.AMST_Id == data.AMCST_Id).ToList();




                            if (studentdetails.Count() > 0)
                            {

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
                                //                                   where (a.FMG_Id == d.FMG_Id && a.FMH_Id == f.FMH_Id && a.FMCC_Id == h.FMCC_Id && g.FYCC_Id == h.FYCC_Id && b.FMA_Id == a.FMA_Id && h.ASMAY_Id == a.ASMAY_Id && i.FYP_Id == b.FYP_Id && f.FMH_Flag == "NT" && i.AMST_Id == data.AMST_Id)
                                //                                   select new FeeAmountEntryDMO
                                //                                   {
                                //                                       FMA_Id = a.FMA_Id,
                                //                                       FMA_Amount = a.FMA_Amount
                                //                                   }
                                //).Count();

                                //if (data.payementcheck == 0)
                                //{
                                //    data.paydet = paymentPart(data);
                                //}

                            }


                            int contactExists = _buspasscontext.SaveChanges();
                            if (contactExists > 0)
                            {
                                //data.returnval = "true";
                                //var admissphoto = _buspasscontext.Adm_M_Student.Single(t => t.MI_Id == data.MI_Id && t.AMST_Id == data.AMST_Id);

                                //admissphoto.AMST_Photoname = data.AMST_Photoname;
                                //_buspasscontext.Update(admissphoto);
                                //int contactExistsphoto = _buspasscontext.SaveChanges();
                            }
                            else
                            {
                                data.returnval = "false";
                            }


                        }
                        else
                        {
                            data.returnval = "Y";
                        }

                    }

                }
                // }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }
        //public Array paymentPart(CLGStudentBuspassFormDTO enq)
        //{
        //    Payment pay = new Payment(_db);
        //    ProspectusDTO data = new ProspectusDTO();
        //    List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();
        //    PaymentDetails PaymentDetailsDto = new PaymentDetails();
        //    int autoinc = 1, totpayableamount = 0;

        //    List<FeeSlplitOnlinePayment> result = new List<FeeSlplitOnlinePayment>();
        //    //enq.ASMAY_Id = 7;
        //    try
        //    {
        //        //  paymentdetails = _ProspectusContext.Fee_PaymentGateway_DetailsDMO.Where(t => t.MI_Id == enq.MI_Id).ToList();

        //        paymentdetails = _buspasscontext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == enq.MI_Id && t.FPGD_PGName == enq.paytype).Distinct().ToList();

        //        // ProspectusDTO ProspectusDTO = new ProspectusDTO();
        //        var FeeAmountresult = (
        //                               from d in _buspasscontext.Clg_Fee_AmountEntry_DMO
        //                               from a in _buspasscontext.CLG_Fee_College_Master_Amount_Semesterwise
        //                               from g in _buspasscontext.FeeHeadDMO
        //                               from e in _buspasscontext.FeeGroupDMO
        //                               where (d.FMH_Id == g.FMH_Id && a.FCMA_Id==d.FCMA_Id  && d.FMG_Id == e.FMG_Id && d.ASMAY_Id == enq.ASMAY_Id && d.MI_Id == enq.MI_Id && d.FMG_Id == e.FMG_Id && g.FMH_Flag == "NT" && d.AMB_Id == enq.AMB_Id && d.AMCO_Id==enq.AMCO_Id && a.AMSE_Id==enq.AMSE_Id && a.MI_Id==d.MI_Id)
        //                               select new CLGStudentBuspassFormDTO
        //                               {
        //                                   FCMA_Id = d.FCMA_Id,
        //                                   FMA_Amount = a.FCMAS_Amount
        //                               }
        //    ).FirstOrDefault();

        //        try
        //        {
        //            // string ids = enq.ftiidss;

        //            using (var cmd1 = _buspasscontext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd1.CommandText = "Admission_Transport_Split_Payment_Registration_college";
        //                cmd1.CommandType = CommandType.StoredProcedure;

        //                cmd1.Parameters.Add(new SqlParameter("@MI_Id",
        //                 SqlDbType.BigInt)
        //                {
        //                    Value = enq.MI_Id
        //                });

        //                cmd1.Parameters.Add(new SqlParameter("@Asmay_Id",
        //                SqlDbType.BigInt)
        //                {
        //                    Value = enq.ASMAY_Id
        //                });

        //                cmd1.Parameters.Add(new SqlParameter("@AMCST_Id",
        //                SqlDbType.VarChar)
        //                {
        //                    Value = enq.AMCST_Id
        //                });
        //                cmd1.Parameters.Add(new SqlParameter("@paygateway",
        //               SqlDbType.VarChar)
        //                {
        //                    Value = enq.paytype
        //                });


        //                if (cmd1.Connection.State != ConnectionState.Open)
        //                    cmd1.Connection.Open();

        //                try
        //                {
        //                    using (var dataReader = cmd1.ExecuteReader())
        //                    {
        //                        while (dataReader.Read())
        //                        {
        //                            result.Add(new FeeSlplitOnlinePayment
        //                            {
        //                                name = "splitId" + autoinc.ToString(),
        //                                merchantId = dataReader["FPGD_MerchantId"].ToString(),
        //                                value = dataReader["balance"].ToString(),
        //                                commission = "0",
        //                                description = "Online Payment",
        //                            });

        //                            autoinc = autoinc + 1;
        //                        }
        //                    }
        //                }

        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                }
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex.Message);
        //        }


        //        if (enq.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
        //        {
        //            GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
        //            enq.transnumbconfigurationsettingsss.MI_Id = Convert.ToInt64(enq.MI_Id);
        //            enq.transnumbconfigurationsettingsss.ASMAY_Id = Convert.ToInt64(enq.ASMAY_Id);
        //            PaymentDetailsDto.trans_id = a.GenerateNumber(enq.transnumbconfigurationsettingsss);
        //        }

        //        if (FeeAmountresult != null)
        //        {


        //            PaymentDetailsDto.Seq = "key|txnid|amount|productinfo|firstname|email|udf1|udf2|udf3|udf4|udf5|udf6|udf7|udf8|udf9|udf10";

        //            foreach (FeeSlplitOnlinePayment x in result)
        //            {
        //                totpayableamount = totpayableamount + Convert.ToInt32(x.value);
        //            }

        //            var item = new
        //            {
        //                paymentParts = result
        //            };

        //            string payinfo = JsonConvert.SerializeObject(item);

        //            if (enq.paytype == "PAYU")
        //            {

        //                PaymentDetailsDto.productinfo = payinfo;
        //                PaymentDetailsDto.amount = Convert.ToDecimal(totpayableamount);
        //                PaymentDetailsDto.MARCHANT_ID = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;
        //                PaymentDetailsDto.firstname = enq.AMCST_FirstName;


        //                PaymentDetailsDto.email = enq.AMST_emailId;

        //                PaymentDetailsDto.SaltKey = paymentdetails.FirstOrDefault().FPGD_SaltKey;
        //                PaymentDetailsDto.payu_URL = paymentdetails.FirstOrDefault().FPGD_URL;
        //                PaymentDetailsDto.phone = Convert.ToInt64(enq.AMST_MobileNo);
        //                PaymentDetailsDto.udf1 = Convert.ToString(enq.ASMAY_Id);
        //                PaymentDetailsDto.udf2 = Convert.ToString(enq.AMCST_Id);
        //                PaymentDetailsDto.udf3 = enq.MI_Id.ToString();
        //                PaymentDetailsDto.udf4 = enq.ASMCL_Id.ToString();
        //                PaymentDetailsDto.udf5 = enq.ASMAY_Id.ToString();
        //                PaymentDetailsDto.udf6 = enq.ASMCL_Id.ToString();
        //                // PaymentDetailsDto.transaction_response_url = "";
        //                PaymentDetailsDto.transaction_response_url = "http://localhost:57606/api/CLGStudentBuspassForm/paymentresponse/";
        //                PaymentDetailsDto.status = "success";
        //                PaymentDetailsDto.service_provider = "payu_paisa";

        //                PaymentDetailsDto.PaymentDetailsList = pay.OnlinePayment(PaymentDetailsDto);

        //            }
        //            else
        //            {
        //                List<MOBILE_INSTITUTION> instidet = new List<MOBILE_INSTITUTION>();
        //                instidet = _buspasscontext.MOBILE_INSTITUTION.Where(t => t.MI_ID == enq.MI_Id).ToList();

        //                enq.instidet = instidet.ToArray();
        //                //string orderId;

        //                //Dictionary<string, object> input = new Dictionary<string, object>();
        //                ////input.Add("amount", 1 * 100);
        //                //input.Add("amount", totpayableamount * 100); // this amount should be same as transaction amount
        //                //input.Add("currency", "INR");
        //                //input.Add("receipt", PaymentDetailsDto.trans_id);
        //                //input.Add("payment_capture", 1);

        //                //string key = paymentdetails.FirstOrDefault().FPGD_SaltKey;
        //                //string secret = paymentdetails.FirstOrDefault().FPGD_MerchantId;

        //                //RazorpayClient client = new RazorpayClient(key, secret);

        //                //Razorpay.Api.Order order = client.Order.Create(input);
        //                //orderId = order["id"].ToString();

        //                //enq.trans_id = orderId;
        //                //enq.merchantkey = paymentdetails.FirstOrDefault().FPGD_MerchantId;
        //                //enq.FMA_Amount = totpayableamount;
        //                //enq.splitpayinformation = payinfo;


        //                string orderId;

        //                Dictionary<string, object> input = new Dictionary<string, object>();
        //                //input.Add("amount", 1 * 100);
        //                input.Add("amount", totpayableamount * 100); // this amount should be same as transaction amount
        //                input.Add("currency", "INR");
        //                input.Add("receipt", PaymentDetailsDto.trans_id);
        //                input.Add("payment_capture", 1);

        //                string key = paymentdetails.FirstOrDefault().FPGD_SaltKey;
        //                string secret = paymentdetails.FirstOrDefault().FPGD_AuthorisationKey;

        //                RazorpayClient client = new RazorpayClient(key, secret);

        //                Razorpay.Api.Order order = client.Order.Create(input);
        //                orderId = order["id"].ToString();

        //                enq.trans_id = orderId;
        //                enq.merchantkey = paymentdetails.FirstOrDefault().FPGD_SaltKey;
        //                enq.FMA_Amount = totpayableamount;
        //                enq.splitpayinformation = payinfo;
        //                PaymentDetailsDto.amount = Convert.ToDecimal(totpayableamount);

        //                PaymentDetailsDto.trans_id = orderId;
        //            }

        //            //FeePaymentDetailsDMO feepaydet = new FeePaymentDetailsDMO();
        //            //feepaydet.MI_Id = Convert.ToInt64(enq.MI_Id);
        //            //feepaydet.ASMAY_ID = Convert.ToInt64(enq.ASMAY_Id);

        //            //feepaydet.FTCU_Id = 1;
        //            //feepaydet.FYP_Receipt_No = PaymentDetailsDto.trans_id;
        //            //feepaydet.FYP_Bank_Name = "";
        //            //feepaydet.FYP_Bank_Or_Cash = "O";
        //            //feepaydet.FYP_DD_Cheque_No = "";
        //            //feepaydet.FYP_DD_Cheque_Date = DateTime.Now;
        //            //feepaydet.FYP_Date = DateTime.Now;
        //            //feepaydet.FYP_Tot_Amount = PaymentDetailsDto.amount;
        //            //feepaydet.FYP_Tot_Waived_Amt = 0;
        //            //feepaydet.FYP_Tot_Fine_Amt = 0;
        //            //feepaydet.FYP_Tot_Concession_Amt = 0;
        //            //feepaydet.FYP_Remarks = "Transport Registration";
        //            //feepaydet.FYP_Chq_Bounce = "CL";
        //            //feepaydet.DOE = DateTime.Now;
        //            //feepaydet.CreatedDate = DateTime.Now;
        //            //feepaydet.UpdatedDate = DateTime.Now;
        //            //feepaydet.user_id = Convert.ToInt64(enq.Id);
        //            //feepaydet.fyp_transaction_id = PaymentDetailsDto.trans_id;
        //            //feepaydet.FYP_OnlineChallanStatusFlag = "Payment Initiated";
        //            //feepaydet.FYP_PaymentReference_Id = "";

        //            //_feecontext.FeePaymentDetailsDMO.Add(feepaydet);
        //            //_feecontext.SaveChanges();



        //            Fee_M_Online_TransactionDMO onlinemtrans = new Fee_M_Online_TransactionDMO();

        //            onlinemtrans.FMOT_Trans_Id = PaymentDetailsDto.trans_id;
        //            onlinemtrans.FMOT_Amount = PaymentDetailsDto.amount;
        //            onlinemtrans.FMOT_Date = DateTime.Now;
        //            onlinemtrans.FMOT_Flag = "T";
        //            onlinemtrans.PASR_Id = 0;
        //            onlinemtrans.AMST_Id = enq.AMST_Id;
        //            onlinemtrans.FMOT_Receipt_no = "";
        //            onlinemtrans.ASMAY_ID = Convert.ToInt64(enq.ASMAY_Id);
        //            onlinemtrans.MI_Id = Convert.ToInt64(enq.MI_Id);
        //            onlinemtrans.FYP_PayModeType = "APP";
        //            onlinemtrans.FMOT_PayGatewayType = enq.paytype;

        //            _feecontext.Fee_M_Online_TransactionDMO.Add(onlinemtrans);


        //            Fee_T_Online_TransactionDMO onlinettrans = new Fee_T_Online_TransactionDMO();
        //            onlinettrans.FMOT_Id = onlinemtrans.FMOT_Id;
        //            onlinettrans.FMA_Id = FeeAmountresult.FMA_Id;
        //            onlinettrans.FTOT_Amount = PaymentDetailsDto.amount;
        //            onlinettrans.FTOT_Created_date = DateTime.Now;
        //            onlinettrans.FTOT_Updated_date = DateTime.Now;
        //            onlinettrans.FTOT_Concession = 0;
        //            onlinettrans.FTOT_Fine = 0;

        //            _feecontext.Fee_T_Online_TransactionDMO.Add(onlinettrans);


        //            var contactexisttransaction = 0;

        //            using (var dbCtxTxn = _feecontext.Database.BeginTransaction())
        //            {
        //                try
        //                {
        //                    contactexisttransaction = _feecontext.SaveChanges();
        //                    dbCtxTxn.Commit();
        //                }
        //                catch (Exception ex)
        //                {
        //                    Console.WriteLine(ex.Message);
        //                    dbCtxTxn.Rollback();
        //                }
        //            }








        //            PaymentDetailsDto.paymentdetails = "True";

        //        }
        //        else
        //        {
        //            PaymentDetailsDto.paymentdetails = "false";
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //    }

        //    return PaymentDetailsDto.PaymentDetailsList;

        //}
        //public PaymentDetails payuresponse(PaymentDetails response)
        //{
        //    PaymentDetails dto = new PaymentDetails();
        //    StudentApplicationDTO stu = new StudentApplicationDTO();
        //    FeeStudentTransactionDTO data = new FeeStudentTransactionDTO();
        //    //   FeePaymentDetailsDMO feeypayment = Mapper.Map<FeePaymentDetailsDMO>(response);
        //    if (response.status == "success")
        //    {
        //        string termid = "1";


        //        //   var orderid = (from a in _buspasscontext.AcademicYearDMO
        //        //                  where (a.ASMAY_Id ==Convert.ToInt64(response.udf5) && a.MI_Id ==Convert.ToInt64(response.udf3))
        //        //                  select new CLGStudentBuspassFormDTO
        //        //                  {
        //        //                      year_Order = a.ASMAY_Order + 1
        //        //                  }
        //        //).ToList().ToArray();

        //        //   var asmay_Id = (from a in _buspasscontext.AcademicYearDMO
        //        //                   where (a.ASMAY_Order == orderid[0].year_Order && a.MI_Id == Convert.ToInt64(response.udf3))
        //        //                   select new CLGStudentBuspassFormDTO
        //        //                   {
        //        //                       year_Id = a.ASMAY_Id
        //        //                   }
        //        //    ).ToList().ToArray();
        //        //   string yutherid = asmay_Id.FirstOrDefault().year_Id.ToString();

        //        stu.MI_Id = Convert.ToInt64(response.udf3);
        //        stu.PASR_MobileNo = response.phone;
        //        stu.pasR_Id = Convert.ToInt64(response.udf2);
        //        stu.PASR_emailId = response.email;
        //        stu.ASMAY_Id = Convert.ToInt64(response.udf5);


        //        data.MI_Id = Convert.ToInt64(response.udf3);
        //        data.ASMCL_ID = Convert.ToInt64(response.udf4);
        //        data.ASMAY_Id = Convert.ToInt64(response.udf5);

        //        try
        //        {
        //            var mgs = insertdatainfeetables(response.udf3, termid, response.udf2, response.udf4, response.amount, response.txnid, response.mihpayid.ToString(), response.udf5);
        //            if (Convert.ToInt32(mgs) > 0)
        //            {

        //                List<MasterConfiguration> mstConfig = new List<MasterConfiguration>();
        //                mstConfig = _db.mstConfig.Where(d => d.MI_Id.Equals(stu.MI_Id) && d.ASMAY_Id.Equals(stu.ASMAY_Id)).ToList();

        //                var studDet = _db.Adm_Student_Transport_ApplicationDMO.Where(t => t.MI_Id == data.MI_Id && t.ASTA_FutureAY == data.ASMAY_Id && t.AMST_Id == stu.pasR_Id).ToList();

        //                SMS sms = new SMS(_db);
        //                string s = sms.sendSms(data.MI_Id, Convert.ToInt64(studDet.FirstOrDefault().ASTA_FatherMobileNo), "TRANSPORT_REG", stu.pasR_Id).Result;

        //                var studDett = _db.Adm_M_Student.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == stu.pasR_Id).ToList();

        //                Email Email = new Email(_db);
        //                string m = Email.sendmail(data.MI_Id, studDett.FirstOrDefault().AMST_emailId, "TRANSPORT_REG", stu.pasR_Id);

        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Buspasss.LogError("Error in " + ex.InnerException);
        //            Buspasss.LogInformation("Buspass Exception" + ex.Message);
        //        }






        //    }
        //    else
        //    {
        //        dto.status = response.status;
        //    }

        //    return response;
        //}

        public string get_grp_reptno(FeeStudentTransactionDTO data)
        {
            // try
            // {

            //     List<FeeMasterConfigurationDMO> feemasnum = new List<FeeMasterConfigurationDMO>();
            //     feemasnum = _db.FeeMasterConfigurationDMO.Where(t => t.MI_Id == data.MI_Id).ToList();
            //     data.feeconfiglist = feemasnum.ToArray();

            //     List<long> temparr = new List<long>();
            //     for (int i = 0; i < feemasnum.Count; i++)
            //     {
            //         data.auto_receipt_flag = feemasnum[i].FMC_AutoReceiptFeeGroupFlag;
            //     }

            //     if (data.auto_receipt_flag == 1)
            //     {

            //         var FeeAmountresult = (from a in _feecontext.feeYCC

            //                                from c in _feecontext.feeYCCC
            //                                from d in _feecontext.FeeAmountEntryDMO

            //                                from g in _feecontext.FeeHeadDMO
            //                                from e in _feecontext.FeeGroupDMO
            //                                where (d.FMH_Id == g.FMH_Id && d.FMCC_Id == a.FMCC_Id && a.ASMAY_Id == d.ASMAY_Id && a.FYCC_Id == c.FYCC_Id && d.ASMAY_Id == data.ASMAY_Id && d.MI_Id == data.MI_Id && d.FMG_Id == e.FMG_Id && g.FMH_Flag == "NT" && c.ASMCL_Id == data.ASMCL_ID)
            //                                select new FeeStudentTransactionDTO
            //                                {
            //                                    FMH_Id = d.FMH_Id,
            //                                }
            //).ToList();

            //         List<long> HeadId = new List<long>();
            //         foreach (var item in FeeAmountresult)
            //         {
            //             HeadId.Add(item.FMH_Id);
            //         }

            //         List<FeeStudentTransactionDTO> grps = new List<FeeStudentTransactionDTO>();
            //         grps = (from b in _feecontext.FeeYearlygroupHeadMappingDMO

            //                 where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.FYGHM_ActiveFlag == "1" && HeadId.Contains(b.FMH_Id))

            //                 select new FeeStudentTransactionDTO
            //                 {
            //                     FMG_Id = b.FMG_Id
            //                 }
            //                ).Distinct().ToList();

            //         List<long> grpid = new List<long>();
            //         string groupidss = "0";
            //         foreach (var item in grps)
            //         {
            //             grpid.Add(item.FMG_Id);
            //         }

            //         for (int r = 0; r < grpid.Count(); r++)
            //         {
            //             groupidss = groupidss + ',' + grpid[r];
            //         }

            //         var final_rept_no = "";
            //         List<FeeStudentTransactionDTO> list_all = new List<FeeStudentTransactionDTO>();
            //         List<FeeStudentTransactionDTO> list_repts = new List<FeeStudentTransactionDTO>();

            //         list_all = (from b in _feecontext.Fee_Groupwise_AutoReceiptDMO
            //                     from c in _feecontext.Fee_Groupwise_AutoReceipt_GroupsDMO
            //                     where (b.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && grpid.Contains(c.FMG_Id) && b.FGAR_Id == c.FGAR_Id)

            //                     select new FeeStudentTransactionDTO
            //                     {
            //                         FGAR_PrefixName = b.FGAR_PrefixName,
            //                         FGAR_SuffixName = b.FGAR_SuffixName,
            //                         //FGAR_Name = b.FGAR_Name,
            //                         //FMG_Id = c.FMG_Id
            //                     }
            //              ).Distinct().ToList();

            //         data.grp_count = list_all.Count();

            //         if (data.grp_count == 1)
            //         {


            //             using (var cmd = _feecontext.Database.GetDbConnection().CreateCommand())
            //             {
            //                 cmd.CommandText = "receiptnogeneration";
            //                 cmd.CommandType = CommandType.StoredProcedure;
            //                 cmd.Parameters.Add(new SqlParameter("@mi_id",
            //                     SqlDbType.VarChar, 100)
            //                 {
            //                     Value = data.MI_Id
            //                 });

            //                 cmd.Parameters.Add(new SqlParameter("@asmayid",
            //                    SqlDbType.NVarChar, 100)
            //                 {
            //                     Value = data.ASMAY_Id
            //                 });
            //                 cmd.Parameters.Add(new SqlParameter("@fmgid",
            //                SqlDbType.NVarChar, 100)
            //                 {
            //                     Value = groupidss
            //                 });

            //                 cmd.Parameters.Add(new SqlParameter("@receiptno",
            //     SqlDbType.NVarChar, 500)
            //                 {
            //                     Direction = ParameterDirection.Output
            //                 });

            //                 if (cmd.Connection.State != ConnectionState.Open)
            //                     cmd.Connection.Open();

            //                 var data1 = cmd.ExecuteNonQuery();

            //                 data.FYP_Receipt_No = cmd.Parameters["@receiptno"].Value.ToString();

            //             }

            //             //data.auto_FYP_Receipt_No = final_rept_no;

            //             //data.FYP_Receipt_No = final_rept_no;
            //         }
            //     }
            //     else
            //     {
            //         data.FYP_Receipt_No = "0";
            //     }

            //     //else if (data.automanualreceiptno == "Auto")
            //     //{
            //     //    GenerateTransactionNumbering a = new GenerateTransactionNumbering(_db);
            //     //    data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
            //     //    data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
            //     //    data.FYP_Receipt_No = a.GenerateNumber(data.transnumbconfigurationsettingsss);
            //     //}

            // }
            // catch (Exception ee)
            // {
            //     Console.WriteLine(ee.Message);
            // }
            return data.FYP_Receipt_No;
        }
        public CLGStudentBuspassFormDTO paynow(CLGStudentBuspassFormDTO dt)
        {

            //            try
            //            {
            //                var alreadyExistEmailId = _buspasscontext.Adm_Student_Transport_ApplicationDMO.Where(d => d.ASTA_Id == dt.ASTA_Id).ToList();

            //                var studentdetails = _buspasscontext.Adm_M_Student.Where(d => d.AMST_Id == dt.PASR_Id).ToList();


            //                dt.ASMCL_Id = alreadyExistEmailId.FirstOrDefault().ASTA_FutureClass;
            //                dt.ASMAY_Id = alreadyExistEmailId.FirstOrDefault().ASTA_FutureAY;
            //                //dt.AMST_FirstName = studentdetails.FirstOrDefault().AMST_FirstName;

            //                dt.AMST_FirstName = ((studentdetails.FirstOrDefault().AMST_FirstName == null || studentdetails.FirstOrDefault().AMST_FirstName == "0" ? "" : studentdetails.FirstOrDefault().AMST_FirstName) + " " + (studentdetails.FirstOrDefault().AMST_MiddleName == null || studentdetails.FirstOrDefault().AMST_MiddleName == "0" ? "" : studentdetails.FirstOrDefault().AMST_MiddleName) + " " + (studentdetails.FirstOrDefault().AMST_LastName == null || studentdetails.FirstOrDefault().AMST_LastName == "0" ? "" : studentdetails.FirstOrDefault().AMST_LastName)).Trim();

            //                dt.AMST_emailId = studentdetails.FirstOrDefault().AMST_emailId;
            //                dt.AMST_MobileNo = studentdetails.FirstOrDefault().AMST_MobileNo;
            //                dt.AMST_Id = dt.PASR_Id;


            //                //dt.payementcheck = _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYPPA_Type == "T" && t.PASA_Id == dt.PASR_Id).Count();

            //                var payementcheck = (from j in _feecontext.Adm_Student_Transport_ApplicationDMO
            //                                     where (j.AMST_Id == dt.AMST_Id && j.ASTA_Id == dt.ASTA_Id && j.ASTA_Amount == 0)
            //                                     select new CLGStudentBuspassFormDTO
            //                                     {
            //                                         AMST_Id = j.AMST_Id,
            //                                         ASTA_Id = j.ASTA_Id,
            //                                         PASTA_Amount = j.ASTA_Amount
            //                                     }
            // ).ToList();

            //                if (payementcheck.Count > 0)
            //                {
            //                    //dt.paydet = paymentPart(dt);
            //                }


            //                dt.fillpaymentgateway = (from a in _feecontext.PAYUDETAILS
            //                                         from b in _feecontext.Fee_PaymentGateway_Details
            //                                         where (a.IMPG_ActiveFlg == true && a.IMPG_Id == b.IMPG_Id && b.MI_Id == dt.MI_Id && b.FPGD_PGActiveFlag == "1")
            //                                         select new FeeStudentTransactionDTO
            //                                         {
            //                                             FPGD_Id = a.IMPG_Id,
            //                                             FPGD_PGName = a.IMPG_PGFlag,
            //                                             FPGD_Image = b.FPGD_Image
            //                                         }
            //).Distinct().ToArray();
            //            }
            //            catch (Exception ex)
            //            {
            //                string msg = ex.Message;
            //            }

            return dt;
        }

        public CLGStudentBuspassFormDTO paynow1(CLGStudentBuspassFormDTO dt)
        {

            //            try
            //            {
            //                var alreadyExistEmailId = _buspasscontext.Adm_Student_Transport_ApplicationDMO.Where(d => d.ASTA_Id == dt.ASTA_Id).ToList();

            //                var studentdetails = _buspasscontext.Adm_M_Student.Where(d => d.AMST_Id == dt.PASR_Id).ToList();


            //                dt.ASMCL_Id = alreadyExistEmailId.FirstOrDefault().ASTA_FutureClass;
            //                dt.ASMAY_Id = alreadyExistEmailId.FirstOrDefault().ASTA_FutureAY;
            //                //dt.AMST_FirstName = studentdetails.FirstOrDefault().AMST_FirstName;

            //                dt.AMST_FirstName = ((studentdetails.FirstOrDefault().AMST_FirstName == null || studentdetails.FirstOrDefault().AMST_FirstName == "0" ? "" : studentdetails.FirstOrDefault().AMST_FirstName) + " " + (studentdetails.FirstOrDefault().AMST_MiddleName == null || studentdetails.FirstOrDefault().AMST_MiddleName == "0" ? "" : studentdetails.FirstOrDefault().AMST_MiddleName) + " " + (studentdetails.FirstOrDefault().AMST_LastName == null || studentdetails.FirstOrDefault().AMST_LastName == "0" ? "" : studentdetails.FirstOrDefault().AMST_LastName)).Trim();

            //                dt.AMST_emailId = studentdetails.FirstOrDefault().AMST_emailId;
            //                dt.AMST_MobileNo = studentdetails.FirstOrDefault().AMST_MobileNo;

            //                dt.AMST_AdmNo = studentdetails.FirstOrDefault().AMST_AdmNo;
            //                dt.AMST_Id = dt.PASR_Id;


            //                //dt.payementcheck = _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYPPA_Type == "T" && t.PASA_Id == dt.PASR_Id).Count();

            //                var payementcheck = (from j in _feecontext.Adm_Student_Transport_ApplicationDMO
            //                                     where (j.AMST_Id == dt.AMST_Id && j.ASTA_Id == dt.ASTA_Id && j.ASTA_Amount == 0)
            //                                     select new CLGStudentBuspassFormDTO
            //                                     {
            //                                         AMST_Id = j.AMST_Id,
            //                                         ASTA_Id = j.ASTA_Id,
            //                                         PASTA_Amount = j.ASTA_Amount
            //                                     }
            // ).ToList();

            //                if (payementcheck.Count > 0)
            //                {
            //                    dt.paydet = paymentPart(dt);
            //                }


            //                dt.fillpaymentgateway = (from a in _feecontext.PAYUDETAILS
            //                                         from b in _feecontext.Fee_PaymentGateway_Details
            //                                         where (a.IMPG_ActiveFlg == true && a.IMPG_Id == b.IMPG_Id && b.MI_Id == dt.MI_Id && b.FPGD_PGActiveFlag == "1")
            //                                         select new FeeStudentTransactionDTO
            //                                         {
            //                                             FPGD_Id = a.IMPG_Id,
            //                                             FPGD_PGName = a.IMPG_PGFlag,
            //                                             FPGD_Image = b.FPGD_Image
            //                                         }
            //).Distinct().ToArray();
            //            }
            //            catch (Exception ex)
            //            {
            //                string msg = ex.Message;
            //            }

            return dt;
        }

        public CLGStudentBuspassFormDTO paynow2(CLGStudentBuspassFormDTO dt)
        {

            //            try
            //            {
            //                var alreadyExistEmailId = _buspasscontext.Adm_Student_Transport_ApplicationDMO.Where(d => d.ASTA_Id == dt.ASTA_Id).ToList();

            //                var studentdetails = _buspasscontext.Adm_M_Student.Where(d => d.AMST_Id == dt.PASR_Id).ToList();


            //                dt.ASMCL_Id = alreadyExistEmailId.FirstOrDefault().ASTA_FutureClass;
            //                dt.ASMAY_Id = alreadyExistEmailId.FirstOrDefault().ASTA_FutureAY;
            //                //dt.AMST_FirstName = studentdetails.FirstOrDefault().AMST_FirstName;

            //                dt.AMST_FirstName = ((studentdetails.FirstOrDefault().AMST_FirstName == null || studentdetails.FirstOrDefault().AMST_FirstName == "0" ? "" : studentdetails.FirstOrDefault().AMST_FirstName) + " " + (studentdetails.FirstOrDefault().AMST_MiddleName == null || studentdetails.FirstOrDefault().AMST_MiddleName == "0" ? "" : studentdetails.FirstOrDefault().AMST_MiddleName) + " " + (studentdetails.FirstOrDefault().AMST_LastName == null || studentdetails.FirstOrDefault().AMST_LastName == "0" ? "" : studentdetails.FirstOrDefault().AMST_LastName)).Trim();

            //                dt.AMST_emailId = studentdetails.FirstOrDefault().AMST_emailId;
            //                dt.AMST_MobileNo = studentdetails.FirstOrDefault().AMST_MobileNo;
            //                dt.AMST_Id = dt.PASR_Id;


            //                //dt.payementcheck = _feecontext.Fee_Y_Payment_Preadmission_ApplicationDMO.Where(t => t.FYPPA_Type == "T" && t.PASA_Id == dt.PASR_Id).Count();

            //                var payementcheck = (from j in _feecontext.Adm_Student_Transport_ApplicationDMO
            //                                     where (j.AMST_Id == dt.AMST_Id && j.ASTA_Id == dt.ASTA_Id && j.ASTA_Amount == 0)
            //                                     select new CLGStudentBuspassFormDTO
            //                                     {
            //                                         AMST_Id = j.AMST_Id,
            //                                         ASTA_Id = j.ASTA_Id,
            //                                         PASTA_Amount = j.ASTA_Amount
            //                                     }
            // ).ToList();

            //                if (payementcheck.Count > 0)
            //                {
            //                    dt.paydet = paymentPart(dt);
            //                }


            //                dt.fillpaymentgateway = (from a in _feecontext.PAYUDETAILS
            //                                         from b in _feecontext.Fee_PaymentGateway_Details
            //                                         where (a.IMPG_ActiveFlg == true && a.IMPG_Id == b.IMPG_Id && b.MI_Id == dt.MI_Id && b.FPGD_PGActiveFlag == "1")
            //                                         select new FeeStudentTransactionDTO
            //                                         {
            //                                             FPGD_Id = a.IMPG_Id,
            //                                             FPGD_PGName = a.IMPG_PGFlag,
            //                                             FPGD_Image = b.FPGD_Image
            //                                         }
            //).Distinct().ToArray();
            //            }
            //            catch (Exception ex)
            //            {
            //                string msg = ex.Message;
            //            }

            return dt;
        }

        public PaymentDetails Razorpaypaymentresponse(PaymentDetails response)
        {
            try
            {
                //FeeStudentTransactionDTO pgmod = new FeeStudentTransactionDTO();
                //PaymentDetails dto = new PaymentDetails();

                //TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                //DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                ////TRANSFER API

                //string url = "https://api.razorpay.com/v1/payments/" + response.razorpay_payment_id + "/transfers";

                //List<Fee_PaymentGateway_DetailsDMO> paymentdetails = new List<Fee_PaymentGateway_DetailsDMO>();

                //paymentdetails = _feecontext.Fee_PaymentGateway_Details.Where(t => t.MI_Id == response.IVRMOP_MIID && t.FPGD_PGName == "RAZORPAY").Distinct().ToList();

                //RazorpayClient client = new RazorpayClient(paymentdetails.FirstOrDefault().FPGD_SaltKey, paymentdetails.FirstOrDefault().FPGD_AuthorisationKey);
                //Razorpay.Api.Payment payment = client.Payment.Fetch(response.razorpay_payment_id);

                //response.order_id = payment.Attributes["order_id"];

                ////FETCH SUBMERCHANT IDS

                //var fetchfmhotid = (from a in _feecontext.Fee_M_Online_TransactionDMO
                //                    where (a.FMOT_Trans_Id == response.order_id.ToString() && a.FMOT_Amount > 0)
                //                    select new FeeStudentTransactionDTO
                //                    {
                //                        FMHOT_Id = a.FMOT_Id,
                //                        FMA_Amount = a.FMOT_Amount,
                //                        MI_Id = a.MI_Id,
                //                        ASMAY_Id = a.ASMAY_ID,
                //                        Amst_Id = a.AMST_Id,
                //                    }).ToArray();

                //var fetchstudentdeatils = (from a in _feecontext.AdmissionStudentDMO
                //                           from b in _feecontext.School_Adm_Y_StudentDMO
                //                           where (a.AMST_Id == b.AMST_Id && a.AMST_Id == Convert.ToInt64(fetchfmhotid[0].Amst_Id) && b.ASMAY_Id == fetchfmhotid[0].ASMAY_Id)
                //                           select new FeeStudentTransactionDTO
                //                           {
                //                               amst_mobile = a.AMST_MobileNo,
                //                               amst_email_id = a.AMST_emailId,
                //                               ASMCL_ID = b.ASMCL_Id
                //                           }).ToArray();

                //Dictionary<String, object> transfers = new Dictionary<String, object>();

                //for (int r = 0; r < fetchfmhotid.Count(); r++)
                //{
                //    transfers.Clear();
                //    var fetchaccountid = (from a in _feecontext.Fee_T_Online_TransactionDMO
                //                          from b in _feecontext.FeeAmountEntryDMO
                //                          from c in _feecontext.Fee_OnlinePayment_Mapping
                //                          from d in _feecontext.Fee_PaymentGateway_Details
                //                          from e in _feecontext.PAYUDETAILS
                //                          where (a.FMA_Id == b.FMA_Id && b.FMG_Id == c.fmg_id && b.FMH_Id == c.FMH_Id && b.FTI_Id == c.fti_id && c.fpgd_id == d.FPGD_Id && d.IMPG_Id == e.IMPG_Id && e.IMPG_PGFlag == "RAZORPAY" && a.FMOT_Id == fetchfmhotid[r].FMHOT_Id && b.MI_Id == Convert.ToInt64(fetchfmhotid[r].MI_Id) && b.ASMAY_Id == Convert.ToInt64(fetchfmhotid[r].ASMAY_Id))
                //                          select new FeeStudentTransactionDTO
                //                          {
                //                              FPGD_SubMerchantId = d.FPGD_SubMerchantId
                //                          }).Distinct().ToArray();

                //    var fetchamount = (from a in _feecontext.Fee_M_Online_TransactionDMO
                //                       where (a.AMST_Id == Convert.ToInt64(fetchfmhotid[0].Amst_Id) && a.FMOT_Trans_Id == response.order_id.ToString() && a.FMOT_Id == fetchfmhotid[r].FMHOT_Id)
                //                       select new FeeStudentTransactionDTO
                //                       {
                //                           FMA_Amount = a.FMOT_Amount
                //                       }).ToArray();

                //    transfers.Add("account", (fetchaccountid.FirstOrDefault().FPGD_SubMerchantId));
                //    transfers.Add("amount", (Convert.ToInt32(fetchamount.FirstOrDefault().FMA_Amount.ToString()) * 100).ToString());
                //    transfers.Add("currency", "INR");

                //    Razorpay.Api.Transfer payment1 = client.Transfer.Create(transfers);

                //    transferAPI trapay = new transferAPI();

                //    if (payment1.Attributes["id"] != "")
                //    {
                //        trapay.transfer_id = payment1.Attributes["id"];
                //        trapay.entity = payment1.Attributes["entity"];
                //        trapay.source = payment1.Attributes["source"];
                //        trapay.recipient = payment1.Attributes["recipient"];
                //        trapay.amount = payment1.Attributes["amount"];
                //        trapay.created_at = payment1.Attributes["created_at"];

                //        FEE_RAZOR_TRANSFER_API_DETAILS fet = new FEE_RAZOR_TRANSFER_API_DETAILS();
                //        fet.TRANSFER_ID = trapay.transfer_id;
                //        fet.ENTITY = trapay.entity;
                //        fet.SOURCE = trapay.source;
                //        fet.RECIPIENT = trapay.recipient;
                //        fet.AMOUNT = (Convert.ToInt32(trapay.amount) / 100).ToString();
                //        fet.CREATED_AT = trapay.created_at;
                //        fet.ORDER_ID = response.order_id;

                //        fet.PAYMENT_ID = response.razorpay_payment_id;
                //        fet.MI_ID = Convert.ToInt32(fetchfmhotid[0].MI_Id);
                //        fet.SETTLEMENT_FLAG = "0";

                //        fet.CREATED_BY = indianTime;
                //        fet.UPDATED_BY = indianTime;
                //        _feecontext.Add(fet);
                //        var contactExists = _feecontext.SaveChanges();
                //        if (contactExists == 1)
                //        {
                //            response.status = "success";
                //        }
                //        else
                //        {
                //            response.status = "Failure";
                //        }
                //    }
                //}

                //if (response.status == "success")
                //{
                //    var groups = (from a in _feecontext.FeePaymentDetailsDMO
                //                  where (a.MI_Id == Convert.ToInt32(fetchfmhotid[0].MI_Id) && a.ASMAY_ID == Convert.ToInt32(fetchfmhotid[0].ASMAY_Id) && a.fyp_transaction_id == response.order_id)
                //                  select new FeeStudentTransactionDTO
                //                  {
                //                      FYP_Receipt_No = a.FYP_Receipt_No
                //                  }
                //          ).Distinct().ToList();

                //    if (groups.Count == 0)
                //    {
                //        var confirmstatus = insertdatainfeetables(fetchfmhotid[0].MI_Id.ToString(), "0", fetchfmhotid[0].Amst_Id.ToString(), fetchstudentdeatils[0].ASMCL_ID.ToString(), fetchfmhotid[0].FMA_Amount, response.order_id, response.razorpay_payment_id, fetchfmhotid[0].ASMAY_Id.ToString());
                //    }
                //}
                //else
                //{

                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;
        }

        //public string insertdatainfeetables(string miid, string termid, string studentid, string classid, decimal amount, string transid, string refid, string yearid)
        //{
        //    TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
        //    DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

        //    var contactexisttransaction = 0;
        //    try
        //    {
        //        var confirmstatus = _feecontext.Database.ExecuteSqlCommand("Transport_Application_online_Registration_Mapping @p0,@p1,@p2,@p3", miid, yearid, studentid, classid);

        //        string recnoen = "";
        //        var fetchfmhotid = (from a in _feecontext.Fee_M_Online_TransactionDMO
        //                            where (a.AMST_Id == Convert.ToInt64(studentid) && a.FMOT_Trans_Id == transid.ToString())
        //                            select new FeeStudentTransactionDTO
        //                            {
        //                                FMHOT_Id = a.FMOT_Id,
        //                                FMA_Amount = a.FMOT_Amount,
        //                                FMOT_PayGatewayType = a.FMOT_PayGatewayType
        //                            }).ToArray();

        //        for (int r = 0; r < fetchfmhotid.Count(); r++)
        //        {
        //            var fethchfmgids = (from a in _feecontext.Fee_T_Online_TransactionDMO
        //                                from b in _feecontext.FeeAmountEntryDMO
        //                                from c in _feecontext.Fee_OnlinePayment_Mapping
        //                                where (c.FMH_Id == b.FMH_Id && c.fti_id == b.FTI_Id && c.fmg_id == b.FMG_Id && a.FMA_Id == b.FMA_Id && a.FMOT_Id == fetchfmhotid[r].FMHOT_Id && b.MI_Id == Convert.ToInt64(miid) && b.ASMAY_Id == Convert.ToInt64(yearid))
        //                                select new FeeStudentTransactionDTO
        //                                {
        //                                    FMG_Id = b.FMG_Id
        //                                }).Distinct().ToArray();

        //            List<long> grpid = new List<long>();
        //            string groupidss = "0";
        //            foreach (var item in fethchfmgids)
        //            {
        //                grpid.Add(item.FMG_Id);
        //            }

        //            for (int d = 0; d < fethchfmgids.Count(); d++)
        //            {
        //                groupidss = groupidss + ',' + fethchfmgids[d].FMG_Id;
        //            }


        //            List<FeeStudentTransactionDTO> list_all = new List<FeeStudentTransactionDTO>();
        //            List<FeeStudentTransactionDTO> list_repts = new List<FeeStudentTransactionDTO>();
        //            list_all = (from b in _feecontext.Fee_Groupwise_AutoReceiptDMO
        //                        from c in _feecontext.Fee_Groupwise_AutoReceipt_GroupsDMO
        //                        where (b.MI_Id == Convert.ToInt64(miid) && b.ASMAY_Id == Convert.ToInt64(yearid) && grpid.Contains(c.FMG_Id) && b.FGAR_Id == c.FGAR_Id)

        //                        select new FeeStudentTransactionDTO
        //                        {
        //                            FGAR_PrefixName = b.FGAR_PrefixName,
        //                            FGAR_SuffixName = b.FGAR_SuffixName,
        //                            FGAR_Id = c.FGAR_Id,
        //                        }
        //                 ).Distinct().ToList();

        //            using (var cmd = _feecontext.Database.GetDbConnection().CreateCommand())
        //            {
        //                cmd.CommandText = "receiptnogeneration";
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add(new SqlParameter("@mi_id",
        //                    SqlDbType.VarChar, 100)
        //                {
        //                    Value = Convert.ToInt32(miid)
        //                });

        //                cmd.Parameters.Add(new SqlParameter("@asmayid",
        //                   SqlDbType.NVarChar, 100)
        //                {
        //                    Value = Convert.ToInt32(yearid)
        //                });
        //                cmd.Parameters.Add(new SqlParameter("@fmgid",
        //               SqlDbType.NVarChar, 100)
        //                {
        //                    Value = groupidss
        //                });

        //                cmd.Parameters.Add(new SqlParameter("@receiptno",
        //    SqlDbType.NVarChar, 500)
        //                {
        //                    Direction = ParameterDirection.Output
        //                });

        //                if (cmd.Connection.State != ConnectionState.Open)
        //                    cmd.Connection.Open();

        //                var data1 = cmd.ExecuteNonQuery();

        //                recnoen = cmd.Parameters["@receiptno"].Value.ToString();

        //                var groupwisefmaids = (from a in _feecontext.Fee_T_Online_TransactionDMO
        //                                       from c in _feecontext.Fee_M_Online_TransactionDMO
        //                                       where (a.FMOT_Id == c.FMOT_Id && a.FMOT_Id == fetchfmhotid[r].FMHOT_Id && c.AMST_Id == Convert.ToInt64(studentid))
        //                                       select new FeeStudentTransactionDTO
        //                                       {
        //                                           FMA_Id = a.FMA_Id,
        //                                           FSS_ToBePaid = Convert.ToInt64(a.FTOT_Amount)
        //                                       }
        //                     ).ToArray();

        //                FeePaymentDetailsDMO onlinemtrans = new FeePaymentDetailsDMO();

        //                onlinemtrans.ASMAY_ID = Convert.ToInt64(yearid);
        //                onlinemtrans.FTCU_Id = 1;
        //                onlinemtrans.FYP_Receipt_No = recnoen;
        //                onlinemtrans.FYP_Bank_Name = "";
        //                onlinemtrans.FYP_Bank_Or_Cash = "O";
        //                onlinemtrans.FYP_DD_Cheque_No = "";
        //                onlinemtrans.FYP_DD_Cheque_Date = DateTime.Now;

        //                onlinemtrans.FYP_Date = DateTime.Now;
        //                onlinemtrans.FYP_Tot_Amount = Convert.ToDecimal(fetchfmhotid[r].FMA_Amount);
        //                onlinemtrans.FYP_Tot_Waived_Amt = 0;
        //                onlinemtrans.FYP_Tot_Fine_Amt = 0;
        //                onlinemtrans.FYP_Tot_Concession_Amt = 0;
        //                onlinemtrans.FYP_Remarks = "Online Transport Payment";
        //                // onlinemtrans.IVRMSTAUL_ID = Convert.ToInt64(studentid);

        //                onlinemtrans.FYP_Chq_Bounce = "CL";
        //                onlinemtrans.MI_Id = Convert.ToInt64(miid);
        //                onlinemtrans.DOE = DateTime.Now;
        //                onlinemtrans.CreatedDate = DateTime.Now;
        //                onlinemtrans.UpdatedDate = DateTime.Now;
        //                onlinemtrans.user_id = Convert.ToInt64(studentid);
        //                onlinemtrans.fyp_transaction_id = transid;

        //                onlinemtrans.FYP_OnlineChallanStatusFlag = "Sucessfull";
        //                onlinemtrans.FYP_PaymentReference_Id = refid.ToString();
        //                onlinemtrans.FYP_ChallanNo = "";
        //                onlinemtrans.FYP_PayModeType = "APP";
        //                onlinemtrans.FYP_PayGatewayType = fetchfmhotid[r].FMOT_PayGatewayType;

        //                _feecontext.FeePaymentDetailsDMO.Add(onlinemtrans);

        //                //multimode of payment
        //                Fee_Y_Payment_PaymentModeSchool onlinemulti = new Fee_Y_Payment_PaymentModeSchool();
        //                onlinemulti.FYP_Id = onlinemtrans.FYP_Id;
        //                onlinemulti.FYP_TransactionTypeFlag = "O";
        //                onlinemulti.FYPPM_TotalPaidAmount = Convert.ToDecimal(fetchfmhotid[r].FMA_Amount);
        //                onlinemulti.FYPPM_LedgerId = 0;
        //                onlinemulti.FYPPM_BankName = "";
        //                onlinemulti.FYPPM_DDChequeNo = "";
        //                onlinemulti.FYPPM_DDChequeDate = indianTime;
        //                onlinemulti.FYPPM_TransactionId = transid;
        //                onlinemulti.FYPPM_PaymentReferenceId = refid.ToString();
        //                onlinemulti.FYPPM_ClearanceStatusFlag = "0";
        //                onlinemulti.FYPPM_ClearanceDate = indianTime;
        //                _feecontext.Add(onlinemulti);
        //                //multimode of payment

        //                Fee_Y_Payment_School_StudentDMO onlinestuapp = new Fee_Y_Payment_School_StudentDMO();

        //                onlinestuapp.FYP_Id = onlinemtrans.FYP_Id;
        //                onlinestuapp.AMST_Id = Convert.ToInt64(studentid);
        //                onlinestuapp.ASMAY_Id = Convert.ToInt64(yearid);
        //                onlinestuapp.FTP_TotalPaidAmount = Convert.ToDecimal(fetchfmhotid[r].FMA_Amount);
        //                onlinestuapp.FTP_TotalConcessionAmount = 0;
        //                onlinestuapp.FTP_TotalFineAmount = 0;
        //                onlinestuapp.FTP_TotalWaivedAmount = 0;

        //                _feecontext.Fee_Y_Payment_School_StudentDMO.Add(onlinestuapp);

        //                //var obj_status_stftrans = _feecontext.Adm_Student_Transport_ApplicationDMO.Where(t => t.MI_Id == Convert.ToInt64(miid) && t.ASTA_FutureAY == Convert.ToInt64(yearid) && t.AMST_Id == Convert.ToInt64(studentid)).FirstOrDefault();
        //                //obj_status_stftrans.ASTA_PaymentDate = DateTime.Now;
        //                //obj_status_stftrans.ASTA_Amount = Convert.ToDecimal(fetchfmhotid[r].FMA_Amount);
        //                //obj_status_stftrans.ASTA_ReceiptNo = recnoen;
        //                //_feecontext.Update(obj_status_stftrans);



        //                for (int s = 0; s < groupwisefmaids.Count(); s++)
        //                {
        //                    FeeTransactionPaymentDMO onlinettrans = new FeeTransactionPaymentDMO();
        //                    onlinettrans.FYP_Id = onlinemtrans.FYP_Id;
        //                    onlinettrans.FMA_Id = groupwisefmaids[s].FMA_Id;
        //                    onlinettrans.FTP_Paid_Amt = groupwisefmaids[s].FSS_ToBePaid;
        //                    onlinettrans.FTP_Fine_Amt = 0;
        //                    onlinettrans.FTP_Concession_Amt = 0;
        //                    onlinettrans.FTP_Waived_Amt = 0;
        //                    onlinettrans.ftp_remarks = "Online Transport Payment";

        //                    _feecontext.FeeTransactionPaymentDMO.Add(onlinettrans);

        //                    var obj_status_stf = _feecontext.FeeStudentTransactionDMO.Where(t => t.MI_Id == Convert.ToInt64(miid) && t.ASMAY_Id == Convert.ToInt64(yearid) && t.AMST_Id == Convert.ToInt64(studentid) && t.FMA_Id == groupwisefmaids[s].FMA_Id && t.FSS_ActiveFlag == true).FirstOrDefault();

        //                    obj_status_stf.FSS_PaidAmount = obj_status_stf.FSS_PaidAmount + groupwisefmaids[s].FSS_ToBePaid;

        //                    if (obj_status_stf.FSS_NetAmount != 0)
        //                    {
        //                        obj_status_stf.FSS_ToBePaid = obj_status_stf.FSS_ToBePaid - groupwisefmaids[s].FSS_ToBePaid;
        //                    }
        //                    else
        //                    {
        //                        obj_status_stf.FSS_ToBePaid = 0;
        //                    }

        //                    _feecontext.Update(obj_status_stf);



        //                }

        //                groupidss = "0";
        //            }

        //        }

        //        using (var dbCtxTxn = _feecontext.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                contactexisttransaction = _feecontext.SaveChanges();
        //                dbCtxTxn.Commit();
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine(ex.Message);
        //                dbCtxTxn.Rollback();
        //            }
        //        }

        //        var confirmstatusss = _feecontext.Database.ExecuteSqlCommand("Transport_Application_online_Registration_update @p0,@p1,@p2,@p3,@p4", miid, yearid, studentid, classid, recnoen);


        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        Buspasss.LogError("Error in " + ex.InnerException);
        //        Buspasss.LogInformation("Buspass Exception" + ex.Message);
        //    }

        //    return contactexisttransaction.ToString();
        //}
    }
}
