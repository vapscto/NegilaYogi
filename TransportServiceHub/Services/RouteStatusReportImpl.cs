using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.admission;
using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.Birthday;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Services
{
    public class RouteStatusReportImpl : Interfaces.RouteStatusReportInterface
    {
        private static ConcurrentDictionary<string, RouteStatusReportDTO> _login =
        new ConcurrentDictionary<string, RouteStatusReportDTO>();

        public TransportContext _context;
        ILogger<RouteStatusReportImpl> _areaimpl;
        public RouteStatusReportImpl(ILogger<RouteStatusReportImpl> areaimpl, TransportContext context)
        {

            _areaimpl = areaimpl;
            _context = context;

        }

        public RouteStatusReportDTO getdata(int id)
        {
            RouteStatusReportDTO data = new RouteStatusReportDTO();
            data.MI_Id = id;
            try
            {
                List<AcademicYear> allyear = new List<AcademicYear>();
                allyear = _context.AcademicYear.Where(y => y.Is_Active == true && y.MI_Id == id ).OrderByDescending(t=>t.ASMAY_Order).ToList();
                data.YearList = allyear.Distinct().OrderByDescending(t => t.ASMAY_Order).ToArray();

                data.routename = _context.MasterRouteDMO.Where(a => a.MI_Id == id && a.TRMR_ActiveFlg==true).OrderBy(d=>d.TRMR_order).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Transport Error Driver Char savedata" + ex.Message);
            }

            return data;
        }
        public RouteStatusReportDTO Getreportdetails(RouteStatusReportDTO data)
        {

            try
            {
                List<TransportStatusReportDTO> result3 = new List<TransportStatusReportDTO>();
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "TR_RouteWiseStatus";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                      SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@TRMR_Id",
                     SqlDbType.BigInt)
                    {
                        Value = data.TRMR_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@AppStatus",
                     SqlDbType.VarChar)
                    {
                        Value = data.ASTA_ApplStatus
                    });
                    cmd.Parameters.Add(new SqlParameter("@StudentType",
                   SqlDbType.VarChar)
                    {
                        Value = data.regorname_map
                    });
                    cmd.Parameters.Add(new SqlParameter("@paystatus",
                   SqlDbType.VarChar)
                    {
                        Value = data.Paidnotpaid
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
                                result3.Add(new TransportStatusReportDTO
                                {
                                    //Class_Name = dataReader["class"].ToString(),
                                   // sectionname = dataReader["section"].ToString(),
                                    //stud_count = int.Parse(dataReader["total"].ToString()),
                                   // asmS_Id = int.Parse(dataReader["sectionid"].ToString()),
                                  //  asmcL_Id = int.Parse(dataReader["classid"].ToString())
                                    // ASMCL_Id = dataReader["class"].ToString(),
                                    ASMCL_ClassName = dataReader["ASMCL_ClassName"].ToString(),
                                    ASMC_SectionName = dataReader["ASMC_SectionName"].ToString(),
                                    AMST_AdmNo = dataReader["AMST_AdmNo"].ToString(),
                                    ASTA_ApplStatus = dataReader["ASTA_ApplStatus"].ToString(),
                                    AMST_Id =Convert.ToInt64(dataReader["AMST_Id"].ToString()),
                                    TRMR_RouteName = dataReader["TRMR_RouteName"].ToString(),
                                    ASTA_Regnew = dataReader["ASTA_Regnew"].ToString(),
                                    ASTA_Id = Convert.ToInt64(dataReader["ASTA_Id"].ToString()),
                                    AMST_FirstName = dataReader["StudentName"].ToString(),
                                    ASTA_PickUp_TRML_Id = Convert.ToInt64(dataReader["ASTA_PickUp_TRML_Id"].ToString()),
                                    TRML_PickLocationName = dataReader["TRML_PickupLocationName"].ToString(),

                                    ASTA_Drop_TRML_Id = Convert.ToInt64(dataReader["ASTA_Drop_TRML_Id"].ToString()),
                                    TRML_DropLocationName = dataReader["TRML_DropLocationName"].ToString(),

                                    TRMR_Idp = Convert.ToInt64(dataReader["ASTA_PickUp_TRMR_Id"].ToString()),
                                    TRMR_PickRouteName = dataReader["TRMR_PickRouteName"].ToString(),

                                    TRMR_Idd = Convert.ToInt64(dataReader["ASTA_Drop_TRMR_Id"].ToString()),
                                    TRMR_DropRouteName = dataReader["TRMR_DropRouteName"].ToString(),
                                    ASTA_Amount = Convert.ToDecimal(dataReader["ASTA_Amount"].ToString()),
                                    ASTA_ApplicationNo = dataReader["ApplicationNo"].ToString(),
                                    AMST_BloodGroup = dataReader["AMST_BloodGroup"].ToString(),
                                    ASTA_FatherMobileNo = Convert.ToInt64(dataReader["fatherMobileNo"].ToString()),
                                    AMST_Photoname= dataReader["AMST_Photoname"].ToString(),
                                });
                                data.messagelist = result3.ToArray();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }










//            if (data.ASTA_ApplStatus == "ALL")
//            {
//                if (data.regorname_map == "new")
//                {

//                    if (data.Paidnotpaid == "Paid")
//                    {
//                        data.messagelist = (from a in _context.Adm_Student_Transport_ApplicationDMO
//                                            from b in _context.MasterRouteDMO
//                                            from c in _context.MasterLocationDMO
//                                            from d in _context.Adm_M_Student

//                                            from e in _context.School_M_Class
//                                            from f in _context.School_M_Section
//                                            from g in _context.School_Adm_Y_StudentDMO

//                                            where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.ASTA_ActiveFlag == true && a.AMST_Id == d.AMST_Id && a.TRMA_Id == b.TRMA_Id && (a.ASTA_Regnew == "new") && (a.ASTA_PickUp_TRMR_Id == data.TRMR_Id || a.ASTA_Drop_TRMR_Id == data.TRMR_Id) && b.TRMR_Id == data.TRMR_Id && g.AMST_Id == d.AMST_Id && g.ASMCL_Id == e.ASMCL_Id && g.ASMS_Id == f.ASMS_Id && a.AMST_Id == g.AMST_Id && g.ASMAY_Id == data.ASMAY_Id && a.ASTA_Amount > 0)
//                                            select new TransportStatusReportDTO
//                                            {
//                                                ASMCL_Id = e.ASMCL_Id,
//                                                ASMCL_ClassName = e.ASMCL_ClassName,
//                                                ASMC_SectionName = f.ASMC_SectionName,
//                                                AMST_AdmNo = d.AMST_AdmNo,
//                                                ASTA_ApplStatus = a.ASTA_ApplStatus,
//                                                AMST_Id = a.AMST_Id,
//                                                TRMR_RouteName = b.TRMR_RouteName,
//                                                ASTA_Regnew = a.ASTA_Regnew,
//                                                ASTA_Id = a.ASTA_Id,
//                                                AMST_FirstName = d.AMST_FirstName + ' ' + d.AMST_MiddleName + ' ' + d.AMST_LastName,
//                                                ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
//                                                TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _context.MasterLocationDMO.Single(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).TRML_LocationName : "--",

//                                                ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
//                                                TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _context.MasterLocationDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).TRML_LocationName : "--",

//                                                TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
//                                                TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).TRMR_RouteName : "--",

//                                                TRMR_Idd = a.ASTA_Drop_TRMR_Id,
//                                                TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).TRMR_RouteName : "--",
//                                                ASTA_Amount = a.ASTA_Amount,
//                                                ASTA_ApplicationNo = a.ASTA_ApplicationNo,
//                                                AMST_BloodGroup = d.AMST_BloodGroup,
//                                                ASTA_FatherMobileNo = a.ASTA_FatherMobileNo

//                                                //}).Distinct().OrderByDescending(a => a.ASTA_Id).ToArray();
//                                            }).Distinct().OrderByDescending(a => a.ASMCL_Id).ToArray();
//                    }
//                    else if (data.Paidnotpaid == "NotPaid")
//                    {
//                        data.messagelist = (from a in _context.Adm_Student_Transport_ApplicationDMO
//                                            from b in _context.MasterRouteDMO
//                                            from c in _context.MasterLocationDMO
//                                            from d in _context.Adm_M_Student

//                                            from e in _context.School_M_Class
//                                            from f in _context.School_M_Section
//                                            from g in _context.School_Adm_Y_StudentDMO

//                                            where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.ASTA_ActiveFlag == true && a.AMST_Id == d.AMST_Id && a.TRMA_Id == b.TRMA_Id && (a.ASTA_Regnew == "new") && (a.ASTA_PickUp_TRMR_Id == data.TRMR_Id || a.ASTA_Drop_TRMR_Id == data.TRMR_Id) && b.TRMR_Id == data.TRMR_Id && g.AMST_Id == d.AMST_Id && g.ASMCL_Id == e.ASMCL_Id && g.ASMS_Id == f.ASMS_Id && a.AMST_Id == g.AMST_Id && g.ASMAY_Id == data.ASMAY_Id && a.ASTA_Amount == 0)
//                                            select new TransportStatusReportDTO
//                                            {
//                                                ASMCL_Id = e.ASMCL_Id,
//                                                ASMCL_ClassName = e.ASMCL_ClassName,
//                                                ASMC_SectionName = f.ASMC_SectionName,
//                                                AMST_AdmNo = d.AMST_AdmNo,
//                                                ASTA_ApplStatus = a.ASTA_ApplStatus,
//                                                AMST_Id = a.AMST_Id,
//                                                TRMR_RouteName = b.TRMR_RouteName,
//                                                ASTA_Regnew = a.ASTA_Regnew,
//                                                ASTA_Id = a.ASTA_Id,
//                                                AMST_FirstName = d.AMST_FirstName + ' ' + d.AMST_MiddleName + ' ' + d.AMST_LastName,
//                                                ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
//                                                TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _context.MasterLocationDMO.Single(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).TRML_LocationName : "--",

//                                                ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
//                                                TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _context.MasterLocationDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).TRML_LocationName : "--",

//                                                TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
//                                                TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).TRMR_RouteName : "--",

//                                                TRMR_Idd = a.ASTA_Drop_TRMR_Id,
//                                                TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).TRMR_RouteName : "--",
//                                                ASTA_Amount = a.ASTA_Amount,
//                                                ASTA_ApplicationNo = a.ASTA_ApplicationNo,
//                                                AMST_BloodGroup = d.AMST_BloodGroup,
//                                                ASTA_FatherMobileNo = a.ASTA_FatherMobileNo

//                                                //  }).Distinct().OrderByDescending(a => a.ASTA_Id).ToArray();
//                                            }).Distinct().OrderByDescending(a => a.ASMCL_Id).ToArray();
//                    }
//                    else if (data.Paidnotpaid == "Both")
//                    {
//                        data.messagelist = (from a in _context.Adm_Student_Transport_ApplicationDMO
//                                            from b in _context.MasterRouteDMO
//                                            from c in _context.MasterLocationDMO
//                                            from d in _context.Adm_M_Student

//                                            from e in _context.School_M_Class
//                                            from f in _context.School_M_Section
//                                            from g in _context.School_Adm_Y_StudentDMO

//                                            where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.ASTA_ActiveFlag == true && a.AMST_Id == d.AMST_Id && a.TRMA_Id == b.TRMA_Id && (a.ASTA_Regnew == "new") && (a.ASTA_PickUp_TRMR_Id == data.TRMR_Id || a.ASTA_Drop_TRMR_Id == data.TRMR_Id) && b.TRMR_Id == data.TRMR_Id && g.AMST_Id == d.AMST_Id && g.ASMCL_Id == e.ASMCL_Id && g.ASMS_Id == f.ASMS_Id && a.AMST_Id == g.AMST_Id && g.ASMAY_Id == data.ASMAY_Id)
//                                            select new TransportStatusReportDTO
//                                            {
//                                                ASMCL_Id = e.ASMCL_Id,
//                                                ASMCL_ClassName = e.ASMCL_ClassName,
//                                                ASMC_SectionName = f.ASMC_SectionName,
//                                                AMST_AdmNo = d.AMST_AdmNo,
//                                                ASTA_ApplStatus = a.ASTA_ApplStatus,
//                                                AMST_Id = a.AMST_Id,
//                                                TRMR_RouteName = b.TRMR_RouteName,
//                                                ASTA_Regnew = a.ASTA_Regnew,
//                                                ASTA_Id = a.ASTA_Id,
//                                                AMST_FirstName = d.AMST_FirstName + ' ' + d.AMST_MiddleName + ' ' + d.AMST_LastName,
//                                                ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
//                                                TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _context.MasterLocationDMO.Single(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).TRML_LocationName : "--",

//                                                ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
//                                                TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _context.MasterLocationDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).TRML_LocationName : "--",

//                                                TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
//                                                TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).TRMR_RouteName : "--",

//                                                TRMR_Idd = a.ASTA_Drop_TRMR_Id,
//                                                TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).TRMR_RouteName : "--",
//                                                ASTA_Amount = a.ASTA_Amount,
//                                                ASTA_ApplicationNo = a.ASTA_ApplicationNo,
//                                                AMST_BloodGroup = d.AMST_BloodGroup,
//                                                ASTA_FatherMobileNo = a.ASTA_FatherMobileNo

//                                                // }).Distinct().OrderByDescending(a => a.ASTA_Id).ToArray();
//                                            }).Distinct().OrderByDescending(a => a.ASMCL_Id).ToArray();
//                    }


//                }
//                else if (data.regorname_map == "regular")
//                {
//                    if (data.Paidnotpaid == "Paid")
//                    {
//                        data.messagelist = (from a in _context.Adm_Student_Transport_ApplicationDMO
//                                            from b in _context.MasterRouteDMO
//                                            from c in _context.MasterLocationDMO
//                                            from d in _context.Adm_M_Student
//                                            from e in _context.School_M_Class
//                                            from f in _context.School_M_Section
//                                            from g in _context.School_Adm_Y_StudentDMO

//                                            where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.ASTA_ActiveFlag == true && a.AMST_Id == d.AMST_Id && a.TRMA_Id == b.TRMA_Id && (a.ASTA_Regnew == "regular") && (a.ASTA_PickUp_TRMR_Id == data.TRMR_Id || a.ASTA_Drop_TRMR_Id == data.TRMR_Id) && b.TRMR_Id == data.TRMR_Id && g.AMST_Id == d.AMST_Id && g.ASMCL_Id == e.ASMCL_Id && g.ASMS_Id == f.ASMS_Id && a.AMST_Id == g.AMST_Id && g.ASMAY_Id == data.ASMAY_Id && a.ASTA_Amount > 0)

//                                            select new TransportStatusReportDTO
//                                            {
//                                                ASMCL_Id = e.ASMCL_Id,
//                                                ASMCL_ClassName = e.ASMCL_ClassName,
//                                                ASMC_SectionName = f.ASMC_SectionName,
//                                                AMST_AdmNo = d.AMST_AdmNo,
//                                                ASTA_ApplStatus = a.ASTA_ApplStatus,
//                                                AMST_Id = a.AMST_Id,
//                                                TRMR_RouteName = b.TRMR_RouteName,
//                                                ASTA_Regnew = a.ASTA_Regnew,
//                                                ASTA_Id = a.ASTA_Id,
//                                                AMST_FirstName = d.AMST_FirstName + ' ' + d.AMST_MiddleName + ' ' + d.AMST_LastName,
//                                                ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
//                                                TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _context.MasterLocationDMO.Single(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).TRML_LocationName : "--",

//                                                ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
//                                                TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _context.MasterLocationDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).TRML_LocationName : "--",

//                                                TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
//                                                TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).TRMR_RouteName : "--",

//                                                TRMR_Idd = a.ASTA_Drop_TRMR_Id,
//                                                TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).TRMR_RouteName : "--",
//                                                ASTA_Amount = a.ASTA_Amount,
//                                                ASTA_ApplicationNo = a.ASTA_ApplicationNo,
//                                                AMST_BloodGroup = d.AMST_BloodGroup,
//                                                ASTA_FatherMobileNo = a.ASTA_FatherMobileNo

//                                                // }).Distinct().OrderByDescending(a => a.ASTA_Id).ToArray();
//                                            }).Distinct().OrderByDescending(a => a.ASMCL_Id).ToArray();
//                    }
//                    else if (data.Paidnotpaid == "NotPaid")
//                    {
//                        data.messagelist = (from a in _context.Adm_Student_Transport_ApplicationDMO
//                                            from b in _context.MasterRouteDMO
//                                            from c in _context.MasterLocationDMO
//                                            from d in _context.Adm_M_Student
//                                            from e in _context.School_M_Class
//                                            from f in _context.School_M_Section
//                                            from g in _context.School_Adm_Y_StudentDMO

//                                            where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.ASTA_ActiveFlag == true && a.AMST_Id == d.AMST_Id && a.TRMA_Id == b.TRMA_Id && (a.ASTA_Regnew == "regular") && (a.ASTA_PickUp_TRMR_Id == data.TRMR_Id || a.ASTA_Drop_TRMR_Id == data.TRMR_Id) && b.TRMR_Id == data.TRMR_Id && g.AMST_Id == d.AMST_Id && g.ASMCL_Id == e.ASMCL_Id && g.ASMS_Id == f.ASMS_Id && a.AMST_Id == g.AMST_Id && g.ASMAY_Id == data.ASMAY_Id && a.ASTA_Amount == 0)

//                                            select new TransportStatusReportDTO
//                                            {
//                                                ASMCL_Id = e.ASMCL_Id,
//                                                ASMCL_ClassName = e.ASMCL_ClassName,
//                                                ASMC_SectionName = f.ASMC_SectionName,
//                                                AMST_AdmNo = d.AMST_AdmNo,
//                                                ASTA_ApplStatus = a.ASTA_ApplStatus,
//                                                AMST_Id = a.AMST_Id,
//                                                TRMR_RouteName = b.TRMR_RouteName,
//                                                ASTA_Regnew = a.ASTA_Regnew,
//                                                ASTA_Id = a.ASTA_Id,
//                                                AMST_FirstName = d.AMST_FirstName + ' ' + d.AMST_MiddleName + ' ' + d.AMST_LastName,
//                                                ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
//                                                TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _context.MasterLocationDMO.Single(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).TRML_LocationName : "--",

//                                                ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
//                                                TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _context.MasterLocationDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).TRML_LocationName : "--",

//                                                TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
//                                                TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).TRMR_RouteName : "--",

//                                                TRMR_Idd = a.ASTA_Drop_TRMR_Id,
//                                                TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).TRMR_RouteName : "--",
//                                                ASTA_Amount = a.ASTA_Amount,
//                                                ASTA_ApplicationNo = a.ASTA_ApplicationNo,
//                                                AMST_BloodGroup = d.AMST_BloodGroup,
//                                                ASTA_FatherMobileNo = a.ASTA_FatherMobileNo
//                                            }).Distinct().OrderByDescending(a => a.ASMCL_Id).ToArray();
//                        // }).Distinct().OrderByDescending(a => a.ASTA_Id).ToArray();
//                    }
//                    else if (data.Paidnotpaid == "Both")
//                    {

//                        data.messagelist = (from a in _context.Adm_Student_Transport_ApplicationDMO
//                                            from b in _context.MasterRouteDMO
//                                            from c in _context.MasterLocationDMO
//                                            from d in _context.Adm_M_Student
//                                            from e in _context.School_M_Class
//                                            from f in _context.School_M_Section
//                                            from g in _context.School_Adm_Y_StudentDMO

//                                            where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.ASTA_ActiveFlag == true && a.AMST_Id == d.AMST_Id && a.TRMA_Id == b.TRMA_Id && (a.ASTA_Regnew == "regular") && (a.ASTA_PickUp_TRMR_Id == data.TRMR_Id || a.ASTA_Drop_TRMR_Id == data.TRMR_Id) && b.TRMR_Id == data.TRMR_Id && g.AMST_Id == d.AMST_Id && g.ASMCL_Id == e.ASMCL_Id && g.ASMS_Id == f.ASMS_Id && a.AMST_Id == g.AMST_Id && g.ASMAY_Id == data.ASMAY_Id)

//                                            select new TransportStatusReportDTO
//                                            {
//                                                ASMCL_Id = e.ASMCL_Id,
//                                                ASMCL_ClassName = e.ASMCL_ClassName,
//                                                ASMC_SectionName = f.ASMC_SectionName,
//                                                AMST_AdmNo = d.AMST_AdmNo,
//                                                ASTA_ApplStatus = a.ASTA_ApplStatus,
//                                                AMST_Id = a.AMST_Id,
//                                                TRMR_RouteName = b.TRMR_RouteName,
//                                                ASTA_Regnew = a.ASTA_Regnew,
//                                                ASTA_Id = a.ASTA_Id,
//                                                AMST_FirstName = d.AMST_FirstName + ' ' + d.AMST_MiddleName + ' ' + d.AMST_LastName,
//                                                ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
//                                                TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _context.MasterLocationDMO.Single(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).TRML_LocationName : "--",

//                                                ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
//                                                TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _context.MasterLocationDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).TRML_LocationName : "--",

//                                                TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
//                                                TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).TRMR_RouteName : "--",

//                                                TRMR_Idd = a.ASTA_Drop_TRMR_Id,
//                                                TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).TRMR_RouteName : "--",
//                                                ASTA_Amount = a.ASTA_Amount,
//                                                ASTA_ApplicationNo = a.ASTA_ApplicationNo,
//                                                AMST_BloodGroup = d.AMST_BloodGroup,
//                                                ASTA_FatherMobileNo = a.ASTA_FatherMobileNo

//                                                //  }).Distinct().OrderByDescending(a => a.ASTA_Id).ToArray();
//                                            }).Distinct().OrderByDescending(a => a.ASMCL_Id).ToArray();
//                    }
//                }
//                else if (data.regorname_map == "both")
//                {


//                    if (data.Paidnotpaid == "Paid")
//                    {
//                        data.messagelist = (from a in _context.Adm_Student_Transport_ApplicationDMO
//                                            from b in _context.MasterRouteDMO
//                                            from c in _context.MasterLocationDMO
//                                            from d in _context.Adm_M_Student

//                                            from e in _context.School_M_Class
//                                            from f in _context.School_M_Section
//                                            from g in _context.School_Adm_Y_StudentDMO

//                                            where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.ASTA_ActiveFlag == true && a.AMST_Id == d.AMST_Id && a.TRMA_Id == b.TRMA_Id && (a.ASTA_PickUp_TRMR_Id == data.TRMR_Id || a.ASTA_Drop_TRMR_Id == data.TRMR_Id) && b.TRMR_Id == data.TRMR_Id && g.AMST_Id == d.AMST_Id && g.ASMCL_Id == e.ASMCL_Id && g.ASMS_Id == f.ASMS_Id && a.AMST_Id == g.AMST_Id && g.ASMAY_Id == data.ASMAY_Id && a.ASTA_Amount > 0)
//                                            select new TransportStatusReportDTO
//                                            {
//                                                ASMCL_Id = e.ASMCL_Id,
//                                                ASMCL_ClassName = e.ASMCL_ClassName,
//                                                ASMC_SectionName = f.ASMC_SectionName,
//                                                AMST_AdmNo = d.AMST_AdmNo,

//                                                ASTA_ApplStatus = a.ASTA_ApplStatus,
//                                                AMST_Id = a.AMST_Id,
//                                                TRMR_RouteName = b.TRMR_RouteName,
//                                                ASTA_Regnew = a.ASTA_Regnew,
//                                                ASTA_Id = a.ASTA_Id,
//                                                AMST_FirstName = d.AMST_FirstName + ' ' + d.AMST_MiddleName + ' ' + d.AMST_LastName,
//                                                ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
//                                                TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _context.MasterLocationDMO.Single(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).TRML_LocationName : "--",

//                                                ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
//                                                TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _context.MasterLocationDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).TRML_LocationName : "--",

//                                                TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
//                                                TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).TRMR_RouteName : "--",

//                                                TRMR_Idd = a.ASTA_Drop_TRMR_Id,
//                                                TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).TRMR_RouteName : "--",
//                                                ASTA_Amount = a.ASTA_Amount,
//                                                ASTA_ApplicationNo = a.ASTA_ApplicationNo,
//                                                AMST_BloodGroup = d.AMST_BloodGroup,
//                                                ASTA_FatherMobileNo = a.ASTA_FatherMobileNo

//                                                //}).Distinct().OrderByDescending(a => a.ASTA_Id).ToArray();
//                                            }).Distinct().OrderByDescending(a => a.ASMCL_Id).ToArray();
//                    }
//                    else if (data.Paidnotpaid == "NotPaid")
//                    {
//                        data.messagelist = (from a in _context.Adm_Student_Transport_ApplicationDMO
//                                            from b in _context.MasterRouteDMO
//                                            from c in _context.MasterLocationDMO
//                                            from d in _context.Adm_M_Student

//                                            from e in _context.School_M_Class
//                                            from f in _context.School_M_Section
//                                            from g in _context.School_Adm_Y_StudentDMO

//                                            where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.ASTA_ActiveFlag == true && a.AMST_Id == d.AMST_Id && a.TRMA_Id == b.TRMA_Id && (a.ASTA_PickUp_TRMR_Id == data.TRMR_Id || a.ASTA_Drop_TRMR_Id == data.TRMR_Id) && b.TRMR_Id == data.TRMR_Id && g.AMST_Id == d.AMST_Id && g.ASMCL_Id == e.ASMCL_Id && g.ASMS_Id == f.ASMS_Id && a.AMST_Id == g.AMST_Id && g.ASMAY_Id == data.ASMAY_Id && a.ASTA_Amount == 0)
//                                            select new TransportStatusReportDTO
//                                            {
//                                                ASMCL_Id = e.ASMCL_Id,
//                                                ASMCL_ClassName = e.ASMCL_ClassName,
//                                                ASMC_SectionName = f.ASMC_SectionName,
//                                                AMST_AdmNo = d.AMST_AdmNo,

//                                                ASTA_ApplStatus = a.ASTA_ApplStatus,
//                                                AMST_Id = a.AMST_Id,
//                                                TRMR_RouteName = b.TRMR_RouteName,
//                                                ASTA_Regnew = a.ASTA_Regnew,
//                                                ASTA_Id = a.ASTA_Id,
//                                                AMST_FirstName = d.AMST_FirstName + ' ' + d.AMST_MiddleName + ' ' + d.AMST_LastName,
//                                                ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
//                                                TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _context.MasterLocationDMO.Single(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).TRML_LocationName : "--",

//                                                ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
//                                                TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _context.MasterLocationDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).TRML_LocationName : "--",

//                                                TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
//                                                TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).TRMR_RouteName : "--",

//                                                TRMR_Idd = a.ASTA_Drop_TRMR_Id,
//                                                TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).TRMR_RouteName : "--",
//                                                ASTA_Amount = a.ASTA_Amount,
//                                                ASTA_ApplicationNo = a.ASTA_ApplicationNo,
//                                                AMST_BloodGroup = d.AMST_BloodGroup,
//                                                ASTA_FatherMobileNo = a.ASTA_FatherMobileNo

//                                                //  }).Distinct().OrderByDescending(a => a.ASTA_Id).ToArray();
//                                            }).Distinct().OrderByDescending(a => a.ASMCL_Id).ToArray();
//                    }
//                    else if (data.Paidnotpaid == "Both")
//                    {
//                        data.messagelist = (from a in _context.Adm_Student_Transport_ApplicationDMO
//                                            from b in _context.MasterRouteDMO
//                                            from c in _context.MasterLocationDMO
//                                            from d in _context.Adm_M_Student

//                                            from e in _context.School_M_Class
//                                            from f in _context.School_M_Section
//                                            from g in _context.School_Adm_Y_StudentDMO

//                                            where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.ASTA_ActiveFlag == true && a.AMST_Id == d.AMST_Id && a.TRMA_Id == b.TRMA_Id && (a.ASTA_PickUp_TRMR_Id == data.TRMR_Id || a.ASTA_Drop_TRMR_Id == data.TRMR_Id) && b.TRMR_Id == data.TRMR_Id && g.AMST_Id == d.AMST_Id && g.ASMCL_Id == e.ASMCL_Id && g.ASMS_Id == f.ASMS_Id && a.AMST_Id == g.AMST_Id && g.ASMAY_Id == data.ASMAY_Id
//)
//                                            select new TransportStatusReportDTO
//                                            {
//                                                ASMCL_Id = e.ASMCL_Id,
//                                                ASMCL_ClassName = e.ASMCL_ClassName,
//                                                ASMC_SectionName = f.ASMC_SectionName,
//                                                AMST_AdmNo = d.AMST_AdmNo,

//                                                ASTA_ApplStatus = a.ASTA_ApplStatus,
//                                                AMST_Id = a.AMST_Id,
//                                                TRMR_RouteName = b.TRMR_RouteName,
//                                                ASTA_Regnew = a.ASTA_Regnew,
//                                                ASTA_Id = a.ASTA_Id,
//                                                AMST_FirstName = d.AMST_FirstName + ' ' + d.AMST_MiddleName + ' ' + d.AMST_LastName,
//                                                ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
//                                                TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _context.MasterLocationDMO.Single(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).TRML_LocationName : "--",

//                                                ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
//                                                TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _context.MasterLocationDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).TRML_LocationName : "--",

//                                                TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
//                                                TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).TRMR_RouteName : "--",

//                                                TRMR_Idd = a.ASTA_Drop_TRMR_Id,
//                                                TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).TRMR_RouteName : "--",
//                                                ASTA_Amount = a.ASTA_Amount,
//                                                ASTA_ApplicationNo = a.ASTA_ApplicationNo,
//                                                AMST_BloodGroup = d.AMST_BloodGroup,
//                                                ASTA_FatherMobileNo = a.ASTA_FatherMobileNo

//                                                //  }).Distinct().OrderByDescending(a => a.ASTA_Id).ToArray();
//                                            }).Distinct().OrderByDescending(a => a.ASMCL_Id).ToArray();
//                    }


//                }

//            }
//            else if (data.ASTA_ApplStatus != "ALL")
//            {
//                if (data.regorname_map == "new")
//                {


//                    if (data.Paidnotpaid == "Paid")
//                    {
//                        data.messagelist = (from a in _context.Adm_Student_Transport_ApplicationDMO
//                                            from b in _context.MasterRouteDMO
//                                            from c in _context.MasterLocationDMO
//                                            from d in _context.Adm_M_Student

//                                            from e in _context.School_M_Class
//                                            from f in _context.School_M_Section
//                                            from g in _context.School_Adm_Y_StudentDMO

//                                            where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.ASTA_ActiveFlag == true && a.AMST_Id == d.AMST_Id && a.TRMA_Id == b.TRMA_Id && a.ASTA_Regnew == "new" && a.ASTA_ApplStatus == data.ASTA_ApplStatus && (a.ASTA_PickUp_TRMR_Id == data.TRMR_Id || a.ASTA_Drop_TRMR_Id == data.TRMR_Id) && b.TRMR_Id == data.TRMR_Id && g.AMST_Id == d.AMST_Id && g.ASMCL_Id == e.ASMCL_Id && g.ASMS_Id == f.ASMS_Id && a.AMST_Id == g.AMST_Id && g.ASMAY_Id == data.ASMAY_Id && a.ASTA_Amount > 0)
//                                            select new TransportStatusReportDTO
//                                            {
//                                                ASMCL_Id = e.ASMCL_Id,
//                                                ASMCL_ClassName = e.ASMCL_ClassName,
//                                                ASMC_SectionName = f.ASMC_SectionName,
//                                                AMST_AdmNo = d.AMST_AdmNo,

//                                                ASTA_ApplStatus = a.ASTA_ApplStatus,
//                                                AMST_Id = a.AMST_Id,
//                                                TRMR_RouteName = b.TRMR_RouteName,
//                                                ASTA_Regnew = a.ASTA_Regnew,
//                                                ASTA_Id = a.ASTA_Id,
//                                                AMST_FirstName = d.AMST_FirstName + ' ' + d.AMST_MiddleName + ' ' + d.AMST_LastName,
//                                                ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
//                                                TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _context.MasterLocationDMO.Single(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).TRML_LocationName : "--",

//                                                ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
//                                                TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _context.MasterLocationDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).TRML_LocationName : "--",

//                                                TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
//                                                TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).TRMR_RouteName : "--",

//                                                TRMR_Idd = a.ASTA_Drop_TRMR_Id,
//                                                TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).TRMR_RouteName : "--",
//                                                ASTA_Amount = a.ASTA_Amount,
//                                                ASTA_ApplicationNo = a.ASTA_ApplicationNo,
//                                                AMST_BloodGroup = d.AMST_BloodGroup,
//                                                ASTA_FatherMobileNo = a.ASTA_FatherMobileNo
//                                            }).Distinct().OrderByDescending(a => a.ASMCL_Id).ToArray();
//                        //   }).Distinct().OrderByDescending(a => a.ASTA_Id).ToArray();
//                    }
//                    else if (data.Paidnotpaid == "NotPaid")
//                    {
//                        data.messagelist = (from a in _context.Adm_Student_Transport_ApplicationDMO
//                                            from b in _context.MasterRouteDMO
//                                            from c in _context.MasterLocationDMO
//                                            from d in _context.Adm_M_Student

//                                            from e in _context.School_M_Class
//                                            from f in _context.School_M_Section
//                                            from g in _context.School_Adm_Y_StudentDMO

//                                            where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.ASTA_ActiveFlag == true && a.AMST_Id == d.AMST_Id && a.TRMA_Id == b.TRMA_Id && a.ASTA_Regnew == "new" && a.ASTA_ApplStatus == data.ASTA_ApplStatus && (a.ASTA_PickUp_TRMR_Id == data.TRMR_Id || a.ASTA_Drop_TRMR_Id == data.TRMR_Id) && b.TRMR_Id == data.TRMR_Id && g.AMST_Id == d.AMST_Id && g.ASMCL_Id == e.ASMCL_Id && g.ASMS_Id == f.ASMS_Id && a.AMST_Id == g.AMST_Id && g.ASMAY_Id == data.ASMAY_Id && a.ASTA_Amount == 0)
//                                            select new TransportStatusReportDTO
//                                            {
//                                                ASMCL_Id = e.ASMCL_Id,
//                                                ASMCL_ClassName = e.ASMCL_ClassName,
//                                                ASMC_SectionName = f.ASMC_SectionName,
//                                                AMST_AdmNo = d.AMST_AdmNo,

//                                                ASTA_ApplStatus = a.ASTA_ApplStatus,
//                                                AMST_Id = a.AMST_Id,
//                                                TRMR_RouteName = b.TRMR_RouteName,
//                                                ASTA_Regnew = a.ASTA_Regnew,
//                                                ASTA_Id = a.ASTA_Id,
//                                                AMST_FirstName = d.AMST_FirstName + ' ' + d.AMST_MiddleName + ' ' + d.AMST_LastName,
//                                                ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
//                                                TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _context.MasterLocationDMO.Single(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).TRML_LocationName : "--",

//                                                ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
//                                                TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _context.MasterLocationDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).TRML_LocationName : "--",

//                                                TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
//                                                TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).TRMR_RouteName : "--",

//                                                TRMR_Idd = a.ASTA_Drop_TRMR_Id,
//                                                TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).TRMR_RouteName : "--",
//                                                ASTA_Amount = a.ASTA_Amount,
//                                                ASTA_ApplicationNo = a.ASTA_ApplicationNo,
//                                                AMST_BloodGroup = d.AMST_BloodGroup,
//                                                ASTA_FatherMobileNo = a.ASTA_FatherMobileNo
//                                            }).Distinct().OrderByDescending(a => a.ASMCL_Id).ToArray();
//                        //  }).Distinct().OrderByDescending(a => a.ASTA_Id).ToArray();
//                    }
//                    else if (data.Paidnotpaid == "Both")
//                    {
//                        data.messagelist = (from a in _context.Adm_Student_Transport_ApplicationDMO
//                                            from b in _context.MasterRouteDMO
//                                            from c in _context.MasterLocationDMO
//                                            from d in _context.Adm_M_Student

//                                            from e in _context.School_M_Class
//                                            from f in _context.School_M_Section
//                                            from g in _context.School_Adm_Y_StudentDMO

//                                            where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.ASTA_ActiveFlag == true && a.AMST_Id == d.AMST_Id && a.TRMA_Id == b.TRMA_Id && a.ASTA_Regnew == "new" && a.ASTA_ApplStatus == data.ASTA_ApplStatus && (a.ASTA_PickUp_TRMR_Id == data.TRMR_Id || a.ASTA_Drop_TRMR_Id == data.TRMR_Id) && b.TRMR_Id == data.TRMR_Id && g.AMST_Id == d.AMST_Id && g.ASMCL_Id == e.ASMCL_Id && g.ASMS_Id == f.ASMS_Id && a.AMST_Id == g.AMST_Id && g.ASMAY_Id == data.ASMAY_Id)
//                                            select new TransportStatusReportDTO
//                                            {
//                                                ASMCL_Id = e.ASMCL_Id,
//                                                ASMCL_ClassName = e.ASMCL_ClassName,
//                                                ASMC_SectionName = f.ASMC_SectionName,
//                                                AMST_AdmNo = d.AMST_AdmNo,

//                                                ASTA_ApplStatus = a.ASTA_ApplStatus,
//                                                AMST_Id = a.AMST_Id,
//                                                TRMR_RouteName = b.TRMR_RouteName,
//                                                ASTA_Regnew = a.ASTA_Regnew,
//                                                ASTA_Id = a.ASTA_Id,
//                                                AMST_FirstName = d.AMST_FirstName + ' ' + d.AMST_MiddleName + ' ' + d.AMST_LastName,
//                                                ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
//                                                TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _context.MasterLocationDMO.Single(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).TRML_LocationName : "--",

//                                                ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
//                                                TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _context.MasterLocationDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).TRML_LocationName : "--",

//                                                TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
//                                                TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).TRMR_RouteName : "--",

//                                                TRMR_Idd = a.ASTA_Drop_TRMR_Id,
//                                                TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).TRMR_RouteName : "--",
//                                                ASTA_Amount = a.ASTA_Amount,
//                                                ASTA_ApplicationNo = a.ASTA_ApplicationNo,
//                                                AMST_BloodGroup = d.AMST_BloodGroup,
//                                                ASTA_FatherMobileNo = a.ASTA_FatherMobileNo
//                                            }).Distinct().OrderByDescending(a => a.ASMCL_Id).ToArray();
//                        // }).Distinct().OrderByDescending(a => a.ASTA_Id).ToArray();
//                    }


//                }
//                else if (data.regorname_map == "regular")
//                {


//                    if (data.Paidnotpaid == "Paid")
//                    {
//                        data.messagelist = (from a in _context.Adm_Student_Transport_ApplicationDMO
//                                            from b in _context.MasterRouteDMO
//                                            from c in _context.MasterLocationDMO
//                                            from d in _context.Adm_M_Student

//                                            from e in _context.School_M_Class
//                                            from f in _context.School_M_Section
//                                            from g in _context.School_Adm_Y_StudentDMO

//                                            where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.ASTA_ActiveFlag == true && a.AMST_Id == d.AMST_Id && a.TRMA_Id == b.TRMA_Id && a.ASTA_Regnew == "regular" && a.ASTA_ApplStatus == data.ASTA_ApplStatus && (a.ASTA_PickUp_TRMR_Id == data.TRMR_Id || a.ASTA_Drop_TRMR_Id == data.TRMR_Id) && b.TRMR_Id == data.TRMR_Id && g.AMST_Id == d.AMST_Id && g.ASMCL_Id == e.ASMCL_Id && g.ASMS_Id == f.ASMS_Id && a.AMST_Id == g.AMST_Id && g.ASMAY_Id == data.ASMAY_Id && a.ASTA_Amount > 0)
//                                            select new TransportStatusReportDTO
//                                            {
//                                                ASMCL_Id = e.ASMCL_Id,
//                                                ASMCL_ClassName = e.ASMCL_ClassName,
//                                                ASMC_SectionName = f.ASMC_SectionName,
//                                                AMST_AdmNo = d.AMST_AdmNo,

//                                                ASTA_ApplStatus = a.ASTA_ApplStatus,
//                                                AMST_Id = a.AMST_Id,
//                                                TRMR_RouteName = b.TRMR_RouteName,
//                                                ASTA_Regnew = a.ASTA_Regnew,
//                                                ASTA_Id = a.ASTA_Id,
//                                                AMST_FirstName = d.AMST_FirstName + ' ' + d.AMST_MiddleName + ' ' + d.AMST_LastName,
//                                                ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
//                                                TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _context.MasterLocationDMO.Single(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).TRML_LocationName : "--",

//                                                ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
//                                                TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _context.MasterLocationDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).TRML_LocationName : "--",

//                                                TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
//                                                TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).TRMR_RouteName : "--",

//                                                TRMR_Idd = a.ASTA_Drop_TRMR_Id,
//                                                TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).TRMR_RouteName : "--",
//                                                ASTA_Amount = a.ASTA_Amount,
//                                                ASTA_ApplicationNo = a.ASTA_ApplicationNo,
//                                                AMST_BloodGroup = d.AMST_BloodGroup,
//                                                ASTA_FatherMobileNo = a.ASTA_FatherMobileNo

//                                                //}).Distinct().OrderByDescending(a => a.ASTA_Id).ToArray();
//                                            }).Distinct().OrderByDescending(a => a.ASMCL_Id).ToArray();
//                    }
//                    else if (data.Paidnotpaid == "NotPaid")
//                    {
//                        data.messagelist = (from a in _context.Adm_Student_Transport_ApplicationDMO
//                                            from b in _context.MasterRouteDMO
//                                            from c in _context.MasterLocationDMO
//                                            from d in _context.Adm_M_Student

//                                            from e in _context.School_M_Class
//                                            from f in _context.School_M_Section
//                                            from g in _context.School_Adm_Y_StudentDMO

//                                            where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.ASTA_ActiveFlag == true && a.AMST_Id == d.AMST_Id && a.TRMA_Id == b.TRMA_Id && a.ASTA_Regnew == "regular" && a.ASTA_ApplStatus == data.ASTA_ApplStatus && (a.ASTA_PickUp_TRMR_Id == data.TRMR_Id || a.ASTA_Drop_TRMR_Id == data.TRMR_Id) && b.TRMR_Id == data.TRMR_Id && g.AMST_Id == d.AMST_Id && g.ASMCL_Id == e.ASMCL_Id && g.ASMS_Id == f.ASMS_Id && a.AMST_Id == g.AMST_Id && g.ASMAY_Id == data.ASMAY_Id && a.ASTA_Amount == 0)
//                                            select new TransportStatusReportDTO
//                                            {
//                                                ASMCL_Id = e.ASMCL_Id,
//                                                ASMCL_ClassName = e.ASMCL_ClassName,
//                                                ASMC_SectionName = f.ASMC_SectionName,
//                                                AMST_AdmNo = d.AMST_AdmNo,

//                                                ASTA_ApplStatus = a.ASTA_ApplStatus,
//                                                AMST_Id = a.AMST_Id,
//                                                TRMR_RouteName = b.TRMR_RouteName,
//                                                ASTA_Regnew = a.ASTA_Regnew,
//                                                ASTA_Id = a.ASTA_Id,
//                                                AMST_FirstName = d.AMST_FirstName + ' ' + d.AMST_MiddleName + ' ' + d.AMST_LastName,
//                                                ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
//                                                TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _context.MasterLocationDMO.Single(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).TRML_LocationName : "--",

//                                                ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
//                                                TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _context.MasterLocationDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).TRML_LocationName : "--",

//                                                TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
//                                                TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).TRMR_RouteName : "--",

//                                                TRMR_Idd = a.ASTA_Drop_TRMR_Id,
//                                                TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).TRMR_RouteName : "--",
//                                                ASTA_Amount = a.ASTA_Amount,
//                                                ASTA_ApplicationNo = a.ASTA_ApplicationNo,
//                                                AMST_BloodGroup = d.AMST_BloodGroup,
//                                                ASTA_FatherMobileNo = a.ASTA_FatherMobileNo
//                                            }).Distinct().OrderByDescending(a => a.ASMCL_Id).ToArray();
//                        //  }).Distinct().OrderByDescending(a => a.ASTA_Id).ToArray();
//                    }
//                    else if (data.Paidnotpaid == "Both")
//                    {
//                        data.messagelist = (from a in _context.Adm_Student_Transport_ApplicationDMO
//                                            from b in _context.MasterRouteDMO
//                                            from c in _context.MasterLocationDMO
//                                            from d in _context.Adm_M_Student

//                                            from e in _context.School_M_Class
//                                            from f in _context.School_M_Section
//                                            from g in _context.School_Adm_Y_StudentDMO

//                                            where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.ASTA_ActiveFlag == true && a.AMST_Id == d.AMST_Id && a.TRMA_Id == b.TRMA_Id && a.ASTA_Regnew == "regular" && a.ASTA_ApplStatus == data.ASTA_ApplStatus && (a.ASTA_PickUp_TRMR_Id == data.TRMR_Id || a.ASTA_Drop_TRMR_Id == data.TRMR_Id) && b.TRMR_Id == data.TRMR_Id && g.AMST_Id == d.AMST_Id && g.ASMCL_Id == e.ASMCL_Id && g.ASMS_Id == f.ASMS_Id && a.AMST_Id == g.AMST_Id && g.ASMAY_Id == data.ASMAY_Id)
//                                            select new TransportStatusReportDTO
//                                            {
//                                                ASMCL_Id = e.ASMCL_Id,
//                                                ASMCL_ClassName = e.ASMCL_ClassName,
//                                                ASMC_SectionName = f.ASMC_SectionName,
//                                                AMST_AdmNo = d.AMST_AdmNo,

//                                                ASTA_ApplStatus = a.ASTA_ApplStatus,
//                                                AMST_Id = a.AMST_Id,
//                                                TRMR_RouteName = b.TRMR_RouteName,
//                                                ASTA_Regnew = a.ASTA_Regnew,
//                                                ASTA_Id = a.ASTA_Id,
//                                                AMST_FirstName = d.AMST_FirstName + ' ' + d.AMST_MiddleName + ' ' + d.AMST_LastName,
//                                                ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
//                                                TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _context.MasterLocationDMO.Single(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).TRML_LocationName : "--",

//                                                ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
//                                                TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _context.MasterLocationDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).TRML_LocationName : "--",

//                                                TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
//                                                TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).TRMR_RouteName : "--",

//                                                TRMR_Idd = a.ASTA_Drop_TRMR_Id,
//                                                TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).TRMR_RouteName : "--",
//                                                ASTA_Amount = a.ASTA_Amount,
//                                                ASTA_ApplicationNo = a.ASTA_ApplicationNo,
//                                                AMST_BloodGroup = d.AMST_BloodGroup,
//                                                ASTA_FatherMobileNo = a.ASTA_FatherMobileNo
//                                            }).Distinct().OrderByDescending(a => a.ASMCL_Id).ToArray();
//                        //    }).Distinct().OrderByDescending(a => a.ASTA_Id).ToArray();
//                    }


//                }

//                else if (data.regorname_map == "both")
//                {


//                    if (data.Paidnotpaid == "Paid")
//                    {
//                        data.messagelist = (from a in _context.Adm_Student_Transport_ApplicationDMO
//                                            from b in _context.MasterRouteDMO
//                                            from c in _context.MasterLocationDMO
//                                            from d in _context.Adm_M_Student

//                                            from e in _context.School_M_Class
//                                            from f in _context.School_M_Section
//                                            from g in _context.School_Adm_Y_StudentDMO

//                                            where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.ASTA_ActiveFlag == true && a.AMST_Id == d.AMST_Id && a.TRMA_Id == b.TRMA_Id && a.ASTA_ApplStatus == data.ASTA_ApplStatus && (a.ASTA_PickUp_TRMR_Id == data.TRMR_Id || a.ASTA_Drop_TRMR_Id == data.TRMR_Id) && b.TRMR_Id == data.TRMR_Id && g.AMST_Id == d.AMST_Id && g.ASMCL_Id == e.ASMCL_Id && g.ASMS_Id == f.ASMS_Id && a.AMST_Id == g.AMST_Id && g.ASMAY_Id == data.ASMAY_Id && a.ASTA_Amount > 0)
//                                            select new TransportStatusReportDTO
//                                            {
//                                                ASMCL_Id = e.ASMCL_Id,
//                                                ASMCL_ClassName = e.ASMCL_ClassName,
//                                                ASMC_SectionName = f.ASMC_SectionName,
//                                                AMST_AdmNo = d.AMST_AdmNo,

//                                                ASTA_ApplStatus = a.ASTA_ApplStatus,
//                                                AMST_Id = a.AMST_Id,
//                                                TRMR_RouteName = b.TRMR_RouteName,
//                                                ASTA_Regnew = a.ASTA_Regnew,
//                                                ASTA_Id = a.ASTA_Id,
//                                                AMST_FirstName = d.AMST_FirstName + ' ' + d.AMST_MiddleName + ' ' + d.AMST_LastName,
//                                                ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
//                                                TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _context.MasterLocationDMO.Single(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).TRML_LocationName : "--",

//                                                ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
//                                                TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _context.MasterLocationDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).TRML_LocationName : "--",

//                                                TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
//                                                TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).TRMR_RouteName : "--",

//                                                TRMR_Idd = a.ASTA_Drop_TRMR_Id,
//                                                TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).TRMR_RouteName : "--",
//                                                ASTA_Amount = a.ASTA_Amount,
//                                                ASTA_ApplicationNo = a.ASTA_ApplicationNo,
//                                                AMST_BloodGroup = d.AMST_BloodGroup,
//                                                ASTA_FatherMobileNo = a.ASTA_FatherMobileNo
//                                            }).Distinct().OrderByDescending(a => a.ASMCL_Id).ToArray();
//                        //}).Distinct().OrderByDescending(a => a.ASTA_Id).ToArray();

//                    }
//                    else if (data.Paidnotpaid == "NotPaid")
//                    {
//                        data.messagelist = (from a in _context.Adm_Student_Transport_ApplicationDMO
//                                            from b in _context.MasterRouteDMO
//                                            from c in _context.MasterLocationDMO
//                                            from d in _context.Adm_M_Student

//                                            from e in _context.School_M_Class
//                                            from f in _context.School_M_Section
//                                            from g in _context.School_Adm_Y_StudentDMO

//                                            where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.ASTA_ActiveFlag == true && a.AMST_Id == d.AMST_Id && a.TRMA_Id == b.TRMA_Id && a.ASTA_ApplStatus == data.ASTA_ApplStatus && (a.ASTA_PickUp_TRMR_Id == data.TRMR_Id || a.ASTA_Drop_TRMR_Id == data.TRMR_Id) && b.TRMR_Id == data.TRMR_Id && g.AMST_Id == d.AMST_Id && g.ASMCL_Id == e.ASMCL_Id && g.ASMS_Id == f.ASMS_Id && a.AMST_Id == g.AMST_Id && g.ASMAY_Id == data.ASMAY_Id && a.ASTA_Amount == 0)
//                                            select new TransportStatusReportDTO
//                                            {
//                                                ASMCL_Id = e.ASMCL_Id,
//                                                ASMCL_ClassName = e.ASMCL_ClassName,
//                                                ASMC_SectionName = f.ASMC_SectionName,
//                                                AMST_AdmNo = d.AMST_AdmNo,

//                                                ASTA_ApplStatus = a.ASTA_ApplStatus,
//                                                AMST_Id = a.AMST_Id,
//                                                TRMR_RouteName = b.TRMR_RouteName,
//                                                ASTA_Regnew = a.ASTA_Regnew,
//                                                ASTA_Id = a.ASTA_Id,
//                                                AMST_FirstName = d.AMST_FirstName + ' ' + d.AMST_MiddleName + ' ' + d.AMST_LastName,
//                                                ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
//                                                TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _context.MasterLocationDMO.Single(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).TRML_LocationName : "--",

//                                                ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
//                                                TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _context.MasterLocationDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).TRML_LocationName : "--",

//                                                TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
//                                                TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).TRMR_RouteName : "--",

//                                                TRMR_Idd = a.ASTA_Drop_TRMR_Id,
//                                                TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).TRMR_RouteName : "--",
//                                                ASTA_Amount = a.ASTA_Amount,
//                                                ASTA_ApplicationNo = a.ASTA_ApplicationNo,
//                                                AMST_BloodGroup = d.AMST_BloodGroup,
//                                                ASTA_FatherMobileNo = a.ASTA_FatherMobileNo
//                                            }).Distinct().OrderByDescending(a => a.ASMCL_Id).ToArray();
//                        //  }).Distinct().OrderByDescending(a => a.ASTA_Id).ToArray();

//                    }
//                    else if (data.Paidnotpaid == "Both")
//                    {
//                        data.messagelist = (from a in _context.Adm_Student_Transport_ApplicationDMO
//                                            from b in _context.MasterRouteDMO
//                                            from c in _context.MasterLocationDMO
//                                            from d in _context.Adm_M_Student

//                                            from e in _context.School_M_Class
//                                            from f in _context.School_M_Section
//                                            from g in _context.School_Adm_Y_StudentDMO

//                                            where (a.MI_Id == data.MI_Id && a.MI_Id == b.MI_Id && a.MI_Id == c.MI_Id && a.MI_Id == d.MI_Id && a.ASTA_ActiveFlag == true && a.AMST_Id == d.AMST_Id && a.TRMA_Id == b.TRMA_Id && a.ASTA_ApplStatus == data.ASTA_ApplStatus && (a.ASTA_PickUp_TRMR_Id == data.TRMR_Id || a.ASTA_Drop_TRMR_Id == data.TRMR_Id) && b.TRMR_Id == data.TRMR_Id && g.AMST_Id == d.AMST_Id && g.ASMCL_Id == e.ASMCL_Id && g.ASMS_Id == f.ASMS_Id && a.AMST_Id == g.AMST_Id && g.ASMAY_Id == data.ASMAY_Id)
//                                            select new TransportStatusReportDTO
//                                            {
//                                                ASMCL_Id = e.ASMCL_Id,
//                                                ASMCL_ClassName = e.ASMCL_ClassName,
//                                                ASMC_SectionName = f.ASMC_SectionName,
//                                                AMST_AdmNo = d.AMST_AdmNo,

//                                                ASTA_ApplStatus = a.ASTA_ApplStatus,
//                                                AMST_Id = a.AMST_Id,
//                                                TRMR_RouteName = b.TRMR_RouteName,
//                                                ASTA_Regnew = a.ASTA_Regnew,
//                                                ASTA_Id = a.ASTA_Id,
//                                                AMST_FirstName = d.AMST_FirstName + ' ' + d.AMST_MiddleName + ' ' + d.AMST_LastName,
//                                                ASTA_PickUp_TRML_Id = a.ASTA_PickUp_TRML_Id,
//                                                TRML_PickLocationName = a.ASTA_PickUp_TRML_Id != 0 ? _context.MasterLocationDMO.Single(l => l.MI_Id == data.MI_Id && l.TRML_Id == a.ASTA_PickUp_TRML_Id).TRML_LocationName : "--",

//                                                ASTA_Drop_TRML_Id = a.ASTA_Drop_TRML_Id,
//                                                TRML_DropLocationName = a.ASTA_Drop_TRML_Id != 0 ? _context.MasterLocationDMO.Single(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == a.ASTA_Drop_TRML_Id).TRML_LocationName : "--",

//                                                TRMR_Idp = a.ASTA_PickUp_TRMR_Id,
//                                                TRMR_PickRouteName = a.ASTA_PickUp_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(t => t.MI_Id == data.MI_Id && t.TRMR_Id == a.ASTA_PickUp_TRMR_Id).TRMR_RouteName : "--",

//                                                TRMR_Idd = a.ASTA_Drop_TRMR_Id,
//                                                TRMR_DropRouteName = a.ASTA_Drop_TRMR_Id != 0 ? _context.MasterRouteDMO.Single(td => td.MI_Id == data.MI_Id && td.TRMR_Id == a.ASTA_Drop_TRMR_Id).TRMR_RouteName : "--",
//                                                ASTA_Amount = a.ASTA_Amount,
//                                                ASTA_ApplicationNo = a.ASTA_ApplicationNo,
//                                                AMST_BloodGroup = d.AMST_BloodGroup,
//                                                ASTA_FatherMobileNo = a.ASTA_FatherMobileNo
//                                            }).Distinct().OrderByDescending(a => a.ASMCL_Id).ToArray();
//                        //  }).Distinct().OrderByDescending(a => a.ASTA_Id).ToArray();
//                    }


//                }

//            }
            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "RouteMapScheduleCount";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@MI_Id",
                    SqlDbType.BigInt)
                {
                    Value = data.MI_Id
                });
                cmd.Parameters.Add(new SqlParameter("@ASMAY_Id",
                   SqlDbType.BigInt)
                {
                    Value = data.ASMAY_Id
                });
                cmd.Parameters.Add(new SqlParameter("@TRMR_Id",
                  SqlDbType.BigInt)
                {
                    Value = data.TRMR_Id
                });
                cmd.Parameters.Add(new SqlParameter("@StuRoute",
                  SqlDbType.VarChar)
                {
                    Value = data.regorname_map
                });
                cmd.Parameters.Add(new SqlParameter("@status",
                  SqlDbType.VarChar)
                {
                    Value = data.ASTA_ApplStatus
                });
                cmd.Parameters.Add(new SqlParameter("@Payment",
                  SqlDbType.VarChar)
                {
                    Value = data.Paidnotpaid
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
                    data.scheduledata = retObject.ToArray();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return data;
        }

    }
}
