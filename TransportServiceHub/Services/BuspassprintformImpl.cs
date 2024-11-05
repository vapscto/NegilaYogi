using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model.com.vapstech.Transport;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Server;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
namespace TransportServiceHub.Services
{
  
    public class BuspassprintformImpl : Interfaces.BuspassprintformInterface
    {
        public TransportContext _TransportContext;
        public ILogger<TransportApprovedDTO> _log;
        public BuspassprintformImpl(TransportContext _context, ILogger<TransportApprovedDTO> log)
        {
            _TransportContext = _context;
            _log = log;
        }

        public TransportApprovedDTO getdata(int id)
        {
            TransportApprovedDTO data = new TransportApprovedDTO();
            try
            {
                data.getaccyear = _TransportContext.AcademicYear.Where(a => a.MI_Id == id && a.Is_Active == true ).Distinct().OrderByDescending(t=>t.ASMAY_Order).ToArray();
                data.getclass = _TransportContext.School_M_Class.Where(a => a.MI_Id == id && a.ASMCL_ActiveFlag == true).ToArray();
                data.routename = _TransportContext.MasterRouteDMO.Where(a => a.MI_Id == id && a.TRMR_ActiveFlg == true).ToArray();


                data.getdetails = (from a in _TransportContext.Adm_M_Student
                                   from b in _TransportContext.Adm_Student_Transport_ApplicationDMO
                                   from c in _TransportContext.MasterAreaDMO
                                   from d in _TransportContext.School_M_Class

                                   where (a.AMST_Id == b.AMST_Id && b.TRMA_Id == c.TRMA_Id && b.MI_Id == id && a.ASMCL_Id == d.ASMCL_Id
                                   && b.ASTA_Amount > 0 && b.ASTA_ApplStatus != "Waiting")
                                   select new TransportApprovedDTO
                                   {
                                       studentname = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "" : " " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "" : " " + a.AMST_LastName)).Trim(),
                                       areaname = c.TRMA_AreaName,
                                       AMST_Id = b.AMST_Id,
                                       ASTA_Id = b.ASTA_Id,
                                       FASMAY_Id = b.ASTA_FutureAY,
                                       applicationno = b.ASTA_ApplicationNo,
                                       pickuproute = b.ASTA_PickUp_TRMR_Id != 0 ? _TransportContext.MasterRouteDMO.Where(t => t.MI_Id == id && t.TRMR_Id == b.ASTA_PickUp_TRMR_Id).FirstOrDefault().TRMR_RouteName : "NULL",
                                       pickuplocation = b.ASTA_PickUp_TRML_Id != 0 ? _TransportContext.MasterLocationDMO.Where(l => l.MI_Id == id && l.TRML_Id == b.ASTA_PickUp_TRML_Id).FirstOrDefault().TRML_LocationName : "NULL",
                                       drouproute = b.ASTA_Drop_TRMR_Id != 0 ? _TransportContext.MasterRouteDMO.Where(td => td.MI_Id == id && td.TRMR_Id == b.ASTA_PickUp_TRMR_Id).FirstOrDefault().TRMR_RouteName : "NULL",
                                       drouplocation = b.ASTA_Drop_TRML_Id != 0 ? _TransportContext.MasterLocationDMO.Where(ld => ld.MI_Id == id && ld.TRML_Id == b.ASTA_PickUp_TRML_Id).FirstOrDefault().TRML_LocationName : "NULL",
                                       ASTA_ApplStatus = b.ASTA_ApplStatus,
                                       ASMCL_ClassName = d.ASMCL_ClassName,
                                       AMST_BloodGroup = a.AMST_BloodGroup,
                                       AMST_AdmNo = a.AMST_AdmNo,
                                       AMST_PerStreet = a.AMST_PerStreet,
                                       AMST_PerArea = a.AMST_PerArea,
                                       AMST_FatherMobleNo = a.AMST_FatherMobleNo,
                                       AMST_MotherMobileNo = a.AMST_MotherMobileNo,
                                       AMST_Photoname = a.AMST_Photoname,
                                       ASTA_Regnew = b.ASTA_Regnew

                                   }).ToArray();

                data.logoheader = (from a in _TransportContext.FeeMasterConfigurationDMO
                                   where (a.MI_Id == id && a.userid == 364)
                                   select new TransportApprovedDTO
                                   {
                                       logopath = a.MI_Logo,
                                   }
       ).ToList().ToArray();

            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Approved Form getdata :" + ex.Message);
            }
            return data;
        }

        public TransportApprovedDTO searchdetails(TransportApprovedDTO data)
        {
            try
            {
                List<TransportApprovedDTO> details = new List<TransportApprovedDTO>();
                // if (data.Flag == "P")
                //{
                if (data.ASMCL_Id == 0)
                {
                    details = (from a in _TransportContext.Adm_M_Student
                               from b in _TransportContext.Adm_Student_Transport_ApplicationDMO
                               from c in _TransportContext.MasterAreaDMO
                               from d in _TransportContext.School_M_Section
                               from e in _TransportContext.School_Adm_Y_StudentDMO
                               from f in _TransportContext.School_M_Class
                               from g in _TransportContext.AcademicYearDMO
                               where (a.AMST_Id == b.AMST_Id && b.AMST_Id == e.AMST_Id && e.ASMCL_Id == f.ASMCL_Id && e.ASMS_Id == d.ASMS_Id
                                      && e.ASMAY_Id == b.ASTA_FutureAY && c.TRMA_Id == b.TRMA_Id && e.ASMAY_Id == g.ASMAY_Id && a.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id
                                       && b.ASTA_ActiveFlag == true && ((b.ASTA_PickUp_TRMR_Id == data.TRMR_Id || b.ASTA_Drop_TRMR_Id == data.TRMR_Id)) && b.ASTA_Amount > 0 && b.ASTA_ApplStatus == "Approved")
                               //a.AMST_Id == b.AMST_Id && b.TRMA_Id == c.TRMA_Id && b.MI_Id == data.MI_Id && b.AMST_Id == e.AMST_Id && d.ASMS_Id == e.ASMS_Id && b.ASTA_FutureAY == data.ASMAY_Id && a.ASMCL_Id==f.ASMCL_Id && e.ASMCL_Id==e.ASMCL_Id
                               //&& b.ASTA_FutureClass == data.ASMCL_Id && b.ASTA_ActiveFlag == true && ((b.ASTA_PickUp_TRMR_Id == data.TRMR_Id || b.ASTA_Drop_TRMR_Id == data.TRMR_Id)) && b.ASTA_Amount > 0 && b.ASTA_ApplStatus == "Approved" && e.ASMAY_Id==data.ASMAY_Id && e.ASMCL_Id==data.ASMCL_Id)
                               select new TransportApprovedDTO
                               {

                                   studentname = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "" : " " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "" : " " + a.AMST_LastName)).Trim(),
                                   areaname = c.TRMA_AreaName,
                                   AMST_Id = b.AMST_Id,
                                   ASTA_Id = b.ASTA_Id,
                                   FASMAY_Id = b.ASTA_FutureAY,
                                   CASMAY_Id = b.ASTA_CurrentAY,
                                   applicationno = b.ASTA_ApplicationNo,
                                   pickuproute = b.ASTA_PickUp_TRMR_Id != 0 ? _TransportContext.MasterRouteDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMR_Id == b.ASTA_PickUp_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",

                                   pickuplocation = b.ASTA_PickUp_TRML_Id != 0 ? _TransportContext.MasterLocationDMO.Where(l => l.MI_Id == data.MI_Id && l.TRML_Id == b.ASTA_PickUp_TRML_Id).FirstOrDefault().TRML_LocationName : "--",

                                   drouproute = b.ASTA_Drop_TRMR_Id != 0 ? _TransportContext.MasterRouteDMO.Where(td => td.MI_Id == data.MI_Id && td.TRMR_Id == b.ASTA_Drop_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",

                                   drouplocation = b.ASTA_Drop_TRML_Id != 0 ? _TransportContext.MasterLocationDMO.Where(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == b.ASTA_Drop_TRML_Id).FirstOrDefault().TRML_LocationName : "--",

                                   pickuprouteno = b.ASTA_PickUp_TRMR_Id != 0 ? _TransportContext.MasterRouteDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMR_Id == b.ASTA_PickUp_TRMR_Id).FirstOrDefault().TRMR_RouteNo : "--",

                                    droprouteno = b.ASTA_Drop_TRMR_Id != 0 ? _TransportContext.MasterRouteDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMR_Id == b.ASTA_Drop_TRMR_Id).FirstOrDefault().TRMR_RouteNo : "--",

                                   ASTA_ApplStatus = b.ASTA_ApplStatus,
                                   ASMCL_ClassName = f.ASMCL_ClassName,
                                   AMST_BloodGroup = a.AMST_BloodGroup,
                                   AMST_AdmNo = a.AMST_AdmNo,
                                   AMST_PerStreet = a.AMST_PerStreet,
                                   AMST_PerArea = a.AMST_PerArea,
                                   AMST_PerCity=a.AMST_PerCity,
                                   AMST_PerPincode=a.AMST_PerPincode,
                                   AMST_FatherMobleNo = b.ASTA_FatherMobileNo,
                                   AMST_MotherMobileNo = b.ASTA_MotherMobileNo,
                                   AMST_Photoname = a.AMST_Photoname,
                                   ASTA_Regnew = b.ASTA_Regnew,
                                   ASMC_SectionName = d.ASMC_SectionName,
                                   AMST_ConArea = a.AMST_ConArea,
                                   AMST_ConStreet = a.AMST_ConStreet,
                                   AMST_ConCity = a.AMST_ConCity,
                                   AMST_ConPincode = a.AMST_ConPincode,
                                   ASMAY_Year = g.ASMAY_Year,
                                   astA_Landmark=b.ASTA_Landmark
                               }).ToList();
                }
                else
                {
                    details = (from a in _TransportContext.Adm_M_Student
                               from b in _TransportContext.Adm_Student_Transport_ApplicationDMO
                               from c in _TransportContext.MasterAreaDMO
                               from d in _TransportContext.School_M_Section
                               from e in _TransportContext.School_Adm_Y_StudentDMO
                               from f in _TransportContext.School_M_Class
                               from g in _TransportContext.AcademicYearDMO
                               where (a.AMST_Id == b.AMST_Id && b.AMST_Id == e.AMST_Id && e.ASMCL_Id == f.ASMCL_Id && e.ASMS_Id == d.ASMS_Id
                                      && e.ASMAY_Id == b.ASTA_FutureAY && c.TRMA_Id == b.TRMA_Id && e.ASMAY_Id == g.ASMAY_Id && a.MI_Id == data.MI_Id && e.ASMAY_Id == data.ASMAY_Id
                                       && e.ASMCL_Id == data.ASMCL_Id && b.ASTA_ActiveFlag == true && ((b.ASTA_PickUp_TRMR_Id == data.TRMR_Id || b.ASTA_Drop_TRMR_Id == data.TRMR_Id)) && b.ASTA_Amount > 0 && b.ASTA_ApplStatus == "Approved")
                               //a.AMST_Id == b.AMST_Id && b.TRMA_Id == c.TRMA_Id && b.MI_Id == data.MI_Id && b.AMST_Id == e.AMST_Id && d.ASMS_Id == e.ASMS_Id && b.ASTA_FutureAY == data.ASMAY_Id && a.ASMCL_Id==f.ASMCL_Id && e.ASMCL_Id==e.ASMCL_Id
                               //&& b.ASTA_FutureClass == data.ASMCL_Id && b.ASTA_ActiveFlag == true && ((b.ASTA_PickUp_TRMR_Id == data.TRMR_Id || b.ASTA_Drop_TRMR_Id == data.TRMR_Id)) && b.ASTA_Amount > 0 && b.ASTA_ApplStatus == "Approved" && e.ASMAY_Id==data.ASMAY_Id && e.ASMCL_Id==data.ASMCL_Id)
                               select new TransportApprovedDTO
                               {

                                   studentname = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "" : " " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "" : " " + a.AMST_LastName)).Trim(),
                                   areaname = c.TRMA_AreaName,
                                   AMST_Id = b.AMST_Id,
                                   ASTA_Id = b.ASTA_Id,
                                   FASMAY_Id = b.ASTA_FutureAY,
                                   CASMAY_Id=b.ASTA_CurrentAY,
                                   applicationno = b.ASTA_ApplicationNo,
                                   pickuproute = b.ASTA_PickUp_TRMR_Id != 0 ? _TransportContext.MasterRouteDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMR_Id == b.ASTA_PickUp_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",

                                   pickuplocation = b.ASTA_PickUp_TRML_Id != 0 ? _TransportContext.MasterLocationDMO.Where(l => l.MI_Id == data.MI_Id && l.TRML_Id == b.ASTA_PickUp_TRML_Id).FirstOrDefault().TRML_LocationName : "--",

                                   drouproute = b.ASTA_Drop_TRMR_Id != 0 ? _TransportContext.MasterRouteDMO.Where(td => td.MI_Id == data.MI_Id && td.TRMR_Id == b.ASTA_Drop_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",

                                   drouplocation = b.ASTA_Drop_TRML_Id != 0 ? _TransportContext.MasterLocationDMO.Where(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == b.ASTA_Drop_TRML_Id).FirstOrDefault().TRML_LocationName : "--",

                                   pickuprouteno = b.ASTA_PickUp_TRMR_Id != 0 ? _TransportContext.MasterRouteDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMR_Id == b.ASTA_PickUp_TRMR_Id).FirstOrDefault().TRMR_RouteNo : "--",

                                   droprouteno = b.ASTA_Drop_TRMR_Id != 0 ? _TransportContext.MasterRouteDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMR_Id == b.ASTA_Drop_TRMR_Id).FirstOrDefault().TRMR_RouteNo : "--",

                                   ASTA_ApplStatus = b.ASTA_ApplStatus,
                                   ASMCL_ClassName = f.ASMCL_ClassName,
                                   AMST_BloodGroup = a.AMST_BloodGroup,
                                   AMST_AdmNo = a.AMST_AdmNo,
                                   AMST_PerStreet = a.AMST_PerStreet,
                                   AMST_PerArea = a.AMST_PerArea,
                                   AMST_PerCity = a.AMST_PerCity,
                                   AMST_PerPincode = a.AMST_PerPincode,
                                   AMST_FatherMobleNo = b.ASTA_FatherMobileNo,
                                   AMST_MotherMobileNo = b.ASTA_MotherMobileNo,
                                   AMST_Photoname = a.AMST_Photoname,
                                   ASTA_Regnew = b.ASTA_Regnew,
                                   ASMC_SectionName = d.ASMC_SectionName,
                                   AMST_ConArea = a.AMST_ConArea,
                                   AMST_ConStreet = a.AMST_ConStreet,
                                   AMST_ConCity = a.AMST_ConCity,
                                   AMST_ConPincode = a.AMST_ConPincode,
                                   ASMAY_Year = g.ASMAY_Year,
                                   astA_Landmark = b.ASTA_Landmark
                               }).ToList();
                }


                var orderid = (from a in _TransportContext.AcademicYearDMO
                               where (a.ASMAY_Id == data.ASMAY_Id && a.MI_Id == data.MI_Id)
                               select new TransportApprovedDTO
                               {
                                   year_Order = a.ASMAY_Order - 1
                               }
             ).ToList().ToArray();
                    

                var asmay_Id = (from a in _TransportContext.AcademicYearDMO
                                where (a.ASMAY_Order == orderid[0].year_Order && a.MI_Id == data.MI_Id)
                                select new TransportApprovedDTO
                                {
                                    year_Id = a.ASMAY_Id
                                }
                 ).ToList().ToArray();

                List<TransportApprovedDTO> temp_group = new List<TransportApprovedDTO>();
                temp_group = (from a in _TransportContext.FeeGroupDMO
                              from b in _TransportContext.FeeHeadDMO
                              from c in _TransportContext.FeeYearlygroupHeadMappingDMO
                              where (a.FMG_Id==c.FMG_Id && b.FMH_Id==c.FMH_Id && c.MI_Id==data.MI_Id && c.ASMAY_Id==data.ASMAY_Id && a.FMG_CompulsoryFlag!="T" && b.FMH_Flag=="T")
                              select new TransportApprovedDTO
                              {
                                  FMG_Id=a.FMG_Id
                              }).Distinct().ToList();

                List<long> grp_ids = new List<long>();
                foreach (var item in temp_group)
                {
                    grp_ids.Add(item.FMG_Id);
                }

                string amstidss = "";

                List<TransportApprovedDTO> lstopebal = new List<TransportApprovedDTO>();
                List<TransportApprovedDTO> lstopebal1 = new List<TransportApprovedDTO>();
                List<long> amstids = new List<long>();
                foreach (var item in details)
                { 
                    amstids.Add(item.AMST_Id);
                    if(amstidss=="")
                    {
                        amstidss = item.AMST_Id.ToString();
                    }
                    else
                    {
                        amstidss = amstidss+"," +item.AMST_Id.ToString();
                    }
                }

                


                using (var cmd = _TransportContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Fee_Buspass_Terms";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@asmayid",
               SqlDbType.BigInt)
                    {
                        Value = asmay_Id.FirstOrDefault().year_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
              SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@amstids",
              SqlDbType.VarChar)
                    {
                        Value = amstidss
                    });


                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    var retObject = new List<dynamic>();
                    try
                    {
                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while ( dataReader.Read())
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
                        data.openingbalance = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }


                //Praveen 
                using (var cmd = _TransportContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Fee_Buspass_Excess_OP_Bal";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter("@asmayid",
               SqlDbType.BigInt)
                    {
                        Value = data.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@mi_id",
              SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@amstids",
              SqlDbType.VarChar)
                    {
                        Value = amstidss
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
                        data.axcess_op_bal = retObject.ToArray();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                //Praveen End

                //data.openingbalance = (from a in _TransportContext.FeeStudentTransactionDMO
                //                       where (a.MI_Id == data.MI_Id && a.ASMAY_Id == asmay_Id.FirstOrDefault().year_Id && grp_ids.Contains(a.FMG_Id) && amstids.Contains(a.AMST_Id))
                //                       select new TransportApprovedDTO
                //                       {
                //                           openingbalance = a.FSS_ToBePaid
                //                       }).Sum(t => t.openingbalance);


                data.transportcharges = (from a in _TransportContext.FeeStudentTransactionDMO
                                         from b in _TransportContext.FeeGroupDMO
                                         from c in _TransportContext.FeeHeadDMO
                                         from d in _TransportContext.FeeMasterTermHeadsDMO
                                         from e in _TransportContext.FeeTermDMO
                                         where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FMH_Id == d.FMH_Id && a.FTI_Id == d.FTI_Id && d.FMT_Id == e.FMT_Id &&  amstids.Contains(a.AMST_Id) && a.ASMAY_Id == data.ASMAY_Id && c.FMH_Flag == "T")
                                         select new TransportApprovedDTO
                                         {
                                             AMST_Id=a.AMST_Id,
                                             payableamount = a.FSS_TotalToBePaid,
                                             termname = e.FMT_Name,
                                             headname = c.FMH_FeeName,
                                             FMT_Id = e.FMT_Id,
                                             FMT_Order = e.FMT_Order,
                                             FMH_RefundFlag=c.FMH_RefundFlag
                                         }).OrderBy(t => t.FMT_Order).ToArray();


                data.transportchargespaid = (from a in _TransportContext.FeeStudentTransactionDMO
                                             from b in _TransportContext.FeeGroupDMO
                                             from c in _TransportContext.FeeHeadDMO
                                             from d in _TransportContext.FeeMasterTermHeadsDMO
                                             from e in _TransportContext.FeeTermDMO
                                             from f in _TransportContext.Fee_Y_Payment_School_StudentDMO
                                             from g in _TransportContext.FeeTransactionPaymentDMO
                                             from h in _TransportContext.FeePaymentDetailsDMO
                                             where (a.FMG_Id == b.FMG_Id && a.FMH_Id == c.FMH_Id && a.FMH_Id == d.FMH_Id && a.FTI_Id == d.FTI_Id && d.FMT_Id == e.FMT_Id && amstids.Contains(a.AMST_Id) && a.ASMAY_Id == data.ASMAY_Id && c.FMH_Flag == "T" && f.AMST_Id == a.AMST_Id && f.FYP_Id == g.FYP_Id && g.FYP_Id == h.FYP_Id && g.FMA_Id == a.FMA_Id)
                                             select new TransportApprovedDTO
                                             {
                                                 AMST_Id = a.AMST_Id,
                                                 paidamount = a.FSS_PaidAmount,
                                                 termname = e.FMT_Name,
                                                 headname = c.FMH_FeeName,
                                                 FMT_Id = e.FMT_Id,
                                                 FMT_Order = e.FMT_Order,
                                                 receiptno = h.FYP_Receipt_No,
                                                 paiddate = h.FYP_Date,
                                                 FMH_RefundFlag = c.FMH_RefundFlag
                                             }).OrderBy(t => t.FMT_Order).ToArray();



                //data.openingbalance = _TransportContext.FeeStudentTransactionDMO.Where(r => r.MI_Id == data.MI_Id).Select(r => r.FSS_ToBePaid).GroupBy(id => id).OrderByDescending(id => id.Sum()).Select(g => new TransportApprovedDTO { AMST_Id = g.Key, openingbalance = g.Sum() });

                data.getalldetails = details.ToArray();
                //string html = "";
                //string path = "E:\\NEWcode(02072018)\\july1\\july1\\IVRMUX\\wwwroot\\buspass1.html";
                /////  html = "";
                //html = File.ReadAllText(path);
                //data.htmldata = html;
            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Aprroved searchdetails :" + ex.Message);
                _log.LogError(ex.Message);
            }
            return data;
        }
        public async Task<TransportApprovedDTO> showmodaldetails(TransportApprovedDTO data)
        {
            try
            {


                var studentcurrentyear = (from a in _TransportContext.School_Adm_Y_StudentDMO
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
                    var studentadmityear = (from a in _TransportContext.Adm_M_Student
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




                using (var cmd = _TransportContext.Database.GetDbConnection().CreateCommand())
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
                        data.studentdetails = retObject.ToArray();
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
            return data;
        }

        public TransportApprovedDTO savelist(TransportApprovedDTO data)
        {
            try
            {
                int sucesscount = 0;
                int failedcount = 0;

                if (data.Flag == "A")
                {

                    for (int i = 0; i < data.Temp_Save_List.Length; i++)
                    {
                        long amstid = data.Temp_Save_List[i].AMST_Id;
                        long fasmayid = data.Temp_Save_List[i].FASMAY_Id;
                        long astaid = data.Temp_Save_List[i].ASTA_Id;
                        TransportApprovedDMO approved = new TransportApprovedDMO();
                        try
                        {
                            var confirmstatus = _TransportContext.Database.ExecuteSqlCommand("Auto_Fee_Group_mapping_Transport @p0,@p1,@p2,@p3",
                                data.MI_Id, fasmayid, amstid, data.userId);
                            if (Convert.ToInt32(confirmstatus) > 0)
                            {
                                var update = _TransportContext.Adm_Student_Transport_ApplicationDMO.Single(a => a.MI_Id == data.MI_Id && a.AMST_Id == amstid && a.ASTA_Id == astaid);
                                update.ASTA_ApplStatus = "Approved";
                                update.UpdatedDate = DateTime.Now;
                                _TransportContext.Update(update);

                                approved.ASTA_Id = astaid;
                                approved.IVRMUL_Id = data.userId;
                                approved.ASTAA_Date = DateTime.Now;
                                approved.CreatedDate = DateTime.Now;
                                approved.UpdatedDate = DateTime.Now;
                                _TransportContext.Add(approved);
                                var ks = _TransportContext.SaveChanges();
                                if (ks > 0)
                                {
                                    sucesscount = sucesscount + 1;
                                }
                                else
                                {
                                    failedcount = failedcount + 1;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _log.LogError(ex.Message);
                        }
                    }
                    if (sucesscount > 0)
                    {
                        if (sucesscount == data.Temp_Save_List.Length)
                        {
                            data.message = "Record Saved Sucessfully";
                        }
                        else
                        {
                            data.message = "Record Saved Sucessfully  Sucess Count " + sucesscount + "And Failed Count " + failedcount + "";
                        }
                    }
                    else
                    {
                        data.message = "Record Not Saved Sucessfully";
                    }


                    //else if (failedcount != 0)
                    //{
                    //    data.message = "Record Saved Sucessfully And Failed Count " + failedcount;
                    //}

                }
                else
                {
                    for (int i = 0; i < data.Temp_Save_List.Length; i++)
                    {
                        try
                        {
                            long amstid = data.Temp_Save_List[i].AMST_Id;
                            long fasmayid = data.Temp_Save_List[i].FASMAY_Id;
                            long astaid = data.Temp_Save_List[i].ASTA_Id;

                            var update = _TransportContext.Adm_Student_Transport_ApplicationDMO.Single(a => a.MI_Id == data.MI_Id && a.AMST_Id == amstid && a.ASTA_Id == astaid);
                            update.ASTA_ApplStatus = "Rejected";
                            update.ASTA_ActiveFlag = false;
                            update.UpdatedDate = DateTime.Now;
                            _TransportContext.Update(update);
                            var kl = _TransportContext.SaveChanges();
                            if (kl > 0)
                            {
                                sucesscount = sucesscount + 1;
                            }
                            else
                            {
                                failedcount = failedcount + 1;
                            }
                        }
                        catch (Exception ex)
                        {
                            _log.LogInformation("Transport form  rejected " + ex.Message);
                        }
                    }
                    if (sucesscount > 0)
                    {
                        if (sucesscount == data.Temp_Save_List.Length)
                        {
                            data.message = "Record Saved Sucessfully";
                        }
                        else
                        {
                            data.message = "Record Saved Sucessfully  Sucess Count " + sucesscount + "And Failed Count " + failedcount + "";
                        }
                    }
                    else
                    {
                        data.message = "Record Not Saved Sucessfully";
                    }
                }

            }
            catch (Exception ex)
            {
                _log.LogInformation("Transport Approved Form Savelist :" + ex.Message);
            }
            return data;
        }
    }
}
