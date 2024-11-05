using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Transport;
using DomainModel.Model.com.vapstech.Transport;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Transport;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace TransportServiceHub.Services
{
    public class TransportApprovedImpl : Interfaces.TransportApprovedInterface
    {
        public TransportContext _TransportContext;
        public ILogger<TransportApprovedDTO> _log;
        private readonly DomainModelMsSqlServerContext _db;
        public TransportApprovedImpl(TransportContext _context, ILogger<TransportApprovedDTO> log, DomainModelMsSqlServerContext db)
        {
            _TransportContext = _context;
            _log = log;
            _db = db;
        }

        public TransportApprovedDTO getdata(TransportApprovedDTO data)
        {
            try
            {
                data.getaccyear = _TransportContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active == true ).OrderByDescending(t=>t.ASMAY_Order).ToArray();
                data.getclass = _TransportContext.School_M_Class.Where(a => a.MI_Id == data.MI_Id && a.ASMCL_ActiveFlag == true).ToArray();
                data.routename = _TransportContext.MasterRouteDMO.Where(a => a.MI_Id == data.MI_Id && a.TRMR_ActiveFlg == true).OrderBy(x=>x.TRMR_order).ToArray();

                //added Praveen(01/18/2019)

                data.picsesslist = _TransportContext.MsterSessionDMO.Where(f => f.MI_Id == data.MI_Id && f.TRMS_ActiveFlg == true && f.TRMS_Flag == "Pick Up").Distinct().ToArray();
                data.drpsesslist = _TransportContext.MsterSessionDMO.Where(f => f.MI_Id == data.MI_Id && f.TRMS_ActiveFlg == true && f.TRMS_Flag == "Drop").Distinct().ToArray();


                //End Praveen(01/18/2019)




                data.getdetails = (from a in _TransportContext.Adm_M_Student
                                   from b in _TransportContext.Adm_Student_Transport_ApplicationDMO
                                   from c in _TransportContext.MasterAreaDMO
                                   from d in _TransportContext.AcademicYearDMO
                                   where (a.AMST_Id == b.AMST_Id && b.TRMA_Id == c.TRMA_Id && b.MI_Id == data.MI_Id
                                   && b.ASTA_Amount > 0 && b.ASTA_ApplStatus != "Waiting" && b.ASTA_FutureAY==d.ASMAY_Id && d.Is_Active==true && d.ASMAY_Id== data.ASMAY_Id && a.AMST_SOL=="S" && a.AMST_ActiveFlag==1)
                                   select new TransportApprovedDTO
                                   {
                                       studentname = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "" : " " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "" : " " + a.AMST_LastName)).Trim(),
                                       areaname = c.TRMA_AreaName,
                                       AMST_Id = b.AMST_Id,
                                       ASTA_Id = b.ASTA_Id,
                                       FASMAY_Id = b.ASTA_FutureAY,
                                       applicationno = b.ASTA_ApplicationNo,
                                       pickuproute = b.ASTA_PickUp_TRMR_Id != 0 ? _TransportContext.MasterRouteDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMR_Id == b.ASTA_PickUp_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",
                                       pickuplocation = b.ASTA_PickUp_TRML_Id != 0 ? _TransportContext.MasterLocationDMO.Where(l => l.MI_Id == data.MI_Id && l.TRML_Id == b.ASTA_PickUp_TRML_Id).FirstOrDefault().TRML_LocationName : "--",
                                       drouproute = b.ASTA_Drop_TRMR_Id != 0 ? _TransportContext.MasterRouteDMO.Where(td => td.MI_Id == data.MI_Id && td.TRMR_Id == b.ASTA_PickUp_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",
                                       drouplocation = b.ASTA_Drop_TRML_Id != 0 ? _TransportContext.MasterLocationDMO.Where(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == b.ASTA_Drop_TRML_Id).FirstOrDefault().TRML_LocationName : "--",
                                       ASTA_ApplStatus = b.ASTA_ApplStatus,
                                       ASMAY_Year = d.ASMAY_Year,
                                       ASMAY_Order = d.ASMAY_Order,

                                   }).Distinct().OrderByDescending(m=>m.ASMAY_Order).ToArray();

                data.logoheader = (from a in _TransportContext.FeeMasterConfigurationDMO
                                   where (a.MI_Id == data.MI_Id && a.userid == 364)
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


        public TransportApprovedDTO gridaconchange(TransportApprovedDTO data)
        {

            try
            {
                data.getdetails = (from a in _TransportContext.Adm_M_Student
                                   from b in _TransportContext.Adm_Student_Transport_ApplicationDMO
                                   from c in _TransportContext.MasterAreaDMO
                                   from d in _TransportContext.AcademicYearDMO
                                   where (a.AMST_Id == b.AMST_Id && b.TRMA_Id == c.TRMA_Id && b.MI_Id == data.MI_Id
                                   && b.ASTA_Amount > 0 && b.ASTA_ApplStatus != "Waiting" && b.ASTA_FutureAY == d.ASMAY_Id && d.Is_Active == true && d.ASMAY_Id == data.ASMAY_Id && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1)
                                   select new TransportApprovedDTO
                                   {
                                       studentname = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "" : " " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "" : " " + a.AMST_LastName)).Trim(),
                                       areaname = c.TRMA_AreaName,
                                       AMST_Id = b.AMST_Id,
                                       ASTA_Id = b.ASTA_Id,
                                       FASMAY_Id = b.ASTA_FutureAY,
                                       applicationno = b.ASTA_ApplicationNo,
                                       pickuproute = b.ASTA_PickUp_TRMR_Id != 0 ? _TransportContext.MasterRouteDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMR_Id == b.ASTA_PickUp_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",
                                       pickuplocation = b.ASTA_PickUp_TRML_Id != 0 ? _TransportContext.MasterLocationDMO.Where(l => l.MI_Id == data.MI_Id && l.TRML_Id == b.ASTA_PickUp_TRML_Id).FirstOrDefault().TRML_LocationName : "--",
                                       drouproute = b.ASTA_Drop_TRMR_Id != 0 ? _TransportContext.MasterRouteDMO.Where(td => td.MI_Id == data.MI_Id && td.TRMR_Id == b.ASTA_PickUp_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",
                                       drouplocation = b.ASTA_Drop_TRML_Id != 0 ? _TransportContext.MasterLocationDMO.Where(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == b.ASTA_Drop_TRML_Id).FirstOrDefault().TRML_LocationName : "--",
                                       ASTA_ApplStatus = b.ASTA_ApplStatus,
                                       ASMAY_Year = d.ASMAY_Year,
                                       ASMAY_Order = d.ASMAY_Order,

                                   }).Distinct().OrderByDescending(m => m.ASMAY_Order).ToArray();
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return data;
        }
        public TransportApprovedDTO editapprove(TransportApprovedDTO data)
        {
            try
            {

                if (data.Temp_Save_List.Length>0)
                {
        
                    int sucesscount = 0;
                    int failedcount = 0;
                    foreach (var st  in data.Temp_Save_List)
                    {
                        var lst = _TransportContext.Adm_Student_Transport_ApplicationDMO.Where(w => w.MI_Id == data.MI_Id && w.ASTA_Id == st.ASTA_Id && w.ASTA_ApplStatus == "Approved").ToList();
                        if (lst.Count>0)
                        {
                            var regstatus = _TransportContext.Database.ExecuteSqlCommand("rejectedlist @p0,@p1,@p2,@p3", st.AMST_Id, data.MI_Id, st.FASMAY_Id, data.userId
                                                            );
                            if (Convert.ToInt32(regstatus) > 0)
                            {
                                sucesscount += 1;
                            }
                            else
                            {
                                failedcount += 1;
                            }

                            if (sucesscount > 0)
                            {
                                if (sucesscount == data.Temp_Save_List.Length)
                                {
                                    data.message = "Record Updated Sucessfully";
                                }
                                else
                                {
                                    data.message = "Record Updated Sucessfully  Sucess Count " + sucesscount + "And Failed Count " + failedcount + "";
                                }
                            }
                            else
                            {
                                data.message = "Record Not Updated Sucessfully";
                            }
                        }
                        
                        
                       
                    }
                  
                }
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }
        public TransportApprovedDTO CancelRejection(TransportApprovedDTO data)
        {
            try
            {

                if (data.Temp_Save_List.Length>0)
                {
        
                    int sucesscount = 0;
                    int failedcount = 0;
                    foreach (var st  in data.Temp_Save_List)
                    {

                        var rejlist = _TransportContext.Adm_Student_Transport_ApplicationDMO.Where(e => e.MI_Id == data.MI_Id && e.ASTA_Id == st.ASTA_Id && e.ASTA_ApplStatus == "Rejected").ToList();
                        if (rejlist.Count>0)
                        {
                            var rejlist1 = _TransportContext.Adm_Student_Transport_ApplicationDMO.Single(e => e.MI_Id == data.MI_Id && e.ASTA_Id == st.ASTA_Id && e.ASTA_ApplStatus == "Rejected");
                            rejlist1.UpdatedDate = DateTime.Now;
                            rejlist1.ASTA_ApplStatus = "Waiting";
                            rejlist1.ASTA_ActiveFlag = true;
                            _TransportContext.Update(rejlist1);

                            int regstatus = _TransportContext.SaveChanges();

                            if (Convert.ToInt32(regstatus) > 0)
                            {
                                sucesscount += 1;
                            }
                            else
                            {
                                failedcount += 1;
                            }

                        }

                       
                        
                    }
                    if (sucesscount > 0)
                    {
                        if (sucesscount == data.Temp_Save_List.Length)
                        {
                            data.message = "Record Updated Sucessfully";
                        }
                        else
                        {
                            data.message = "Record Updated Sucessfully  Sucess Count " + sucesscount + "And Failed Count " + failedcount + "";
                        }
                    }
                    else
                    {
                        data.message = "Record Not Updated Sucessfully";
                    }
                }
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return data;
        }

        public TransportApprovedDTO searchdetails(TransportApprovedDTO data)
        {
            try
            {
                List<TransportApprovedDTO> details = new List<TransportApprovedDTO>();
                if (data.ASMCL_Id == 0)
                {
                    if(data.TRMR_Id==0)
                    {
                        if (data.RegularNew == "New" || data.RegularNew == "Regular")
                        {
                            details = (from a in _TransportContext.Adm_M_Student
                                       from b in _TransportContext.Adm_Student_Transport_ApplicationDMO
                                       from c in _TransportContext.MasterAreaDMO
                                       from d in _TransportContext.School_M_Class
                                    //   from f in _TransportContext.School_M_Section
                                       from g in _TransportContext.School_Adm_Y_StudentDMO
                                       where (a.AMST_Id == b.AMST_Id && a.AMST_Id == g.AMST_Id 
                                       //&& g.ASMCL_Id == d.ASMCL_Id 
                                       //&& g.ASMS_Id == f.ASMS_Id 
                                       &&
                                       b.TRMA_Id == c.TRMA_Id && b.MI_Id == data.MI_Id && b.ASTA_FutureClass == d.ASMCL_Id && b.ASTA_FutureAY == data.ASMAY_Id
                                       && b.ASTA_ActiveFlag == true && b.ASTA_Amount > 0 && b.ASTA_ApplStatus.Equals("Waiting") && b.ASTA_Regnew == data.RegularNew && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1)
                                       select new TransportApprovedDTO
                                       {
                                           studentname = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "" : " " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "" : " " + a.AMST_LastName)).Trim(),
                                           areaname = c.TRMA_AreaName,
                                           AMST_Id = b.AMST_Id,
                                           ASTA_Id = b.ASTA_Id,
                                           FASMAY_Id = b.ASTA_FutureAY,
                                           applicationno = b.ASTA_ApplicationNo,
                                           pickuproute = b.ASTA_PickUp_TRMR_Id != 0 ? _TransportContext.MasterRouteDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMR_Id == b.ASTA_PickUp_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",
                                           pickuplocation = b.ASTA_PickUp_TRML_Id != 0 ? _TransportContext.MasterLocationDMO.Where(l => l.MI_Id == data.MI_Id && l.TRML_Id == b.ASTA_PickUp_TRML_Id).FirstOrDefault().TRML_LocationName : "--",
                                           drouproute = b.ASTA_Drop_TRMR_Id != 0 ? _TransportContext.MasterRouteDMO.Where(td => td.MI_Id == data.MI_Id && td.TRMR_Id == b.ASTA_Drop_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",
                                           drouplocation = b.ASTA_Drop_TRML_Id != 0 ? _TransportContext.MasterLocationDMO.Where(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == b.ASTA_Drop_TRML_Id).FirstOrDefault().TRML_LocationName : "--",
                                           ASTA_ApplStatus = b.ASTA_ApplStatus,
                                           neworreguular = b.ASTA_Regnew,
                                           ASMCL_ClassName = d.ASMCL_ClassName,
                                         //  ASMC_SectionName = f.ASMC_SectionName,
                                           AMST_AdmNo = a.AMST_AdmNo,
                                           AMST_Photoname=a.AMST_Photoname
                                       }).Distinct().ToList();
                        }
                        else
                        {
                            details = (from a in _TransportContext.Adm_M_Student
                                       from b in _TransportContext.Adm_Student_Transport_ApplicationDMO
                                       from c in _TransportContext.MasterAreaDMO
                                       from d in _TransportContext.School_M_Class
                                    //   from f in _TransportContext.School_M_Section
                                       from g in _TransportContext.School_Adm_Y_StudentDMO
                                       where (a.AMST_Id == b.AMST_Id && a.AMST_Id == g.AMST_Id
                                       //&& g.ASMCL_Id == d.ASMCL_Id 
                                     //  && g.ASMS_Id == f.ASMS_Id 
                                       && b.TRMA_Id == c.TRMA_Id && b.MI_Id == data.MI_Id && b.ASTA_FutureClass == d.ASMCL_Id && b.ASTA_FutureAY == data.ASMAY_Id
                                       && b.ASTA_ActiveFlag == true  && b.ASTA_Amount > 0 && b.ASTA_ApplStatus.Equals("Waiting") && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1)
                                       select new TransportApprovedDTO
                                       {

                                           studentname = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "" : " " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "" : " " + a.AMST_LastName)).Trim(),
                                           areaname = c.TRMA_AreaName,
                                           AMST_Id = b.AMST_Id,
                                           ASTA_Id = b.ASTA_Id,
                                           FASMAY_Id = b.ASTA_FutureAY,
                                           applicationno = b.ASTA_ApplicationNo,
                                           pickuproute = b.ASTA_PickUp_TRMR_Id != 0 ? _TransportContext.MasterRouteDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMR_Id == b.ASTA_PickUp_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",
                                           pickuplocation = b.ASTA_PickUp_TRML_Id != 0 ? _TransportContext.MasterLocationDMO.Where(l => l.MI_Id == data.MI_Id && l.TRML_Id == b.ASTA_PickUp_TRML_Id).FirstOrDefault().TRML_LocationName : "--",
                                           drouproute = b.ASTA_Drop_TRMR_Id != 0 ? _TransportContext.MasterRouteDMO.Where(td => td.MI_Id == data.MI_Id && td.TRMR_Id == b.ASTA_Drop_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",
                                           drouplocation = b.ASTA_Drop_TRML_Id != 0 ? _TransportContext.MasterLocationDMO.Where(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == b.ASTA_Drop_TRML_Id).FirstOrDefault().TRML_LocationName : "--",
                                           ASTA_ApplStatus = b.ASTA_ApplStatus,
                                           neworreguular = b.ASTA_Regnew,
                                           ASMCL_ClassName = d.ASMCL_ClassName,
                                          // ASMC_SectionName = f.ASMC_SectionName,
                                           AMST_AdmNo = a.AMST_AdmNo,
                                           AMST_Photoname = a.AMST_Photoname

                                       }).Distinct().ToList();
                        }

                    }
                    else
                    {
                        if (data.RegularNew == "New" || data.RegularNew == "Regular")
                        {
                            details = (from a in _TransportContext.Adm_M_Student
                                       from b in _TransportContext.Adm_Student_Transport_ApplicationDMO
                                       from c in _TransportContext.MasterAreaDMO
                                       from d in _TransportContext.School_M_Class
                                     //  from f in _TransportContext.School_M_Section
                                       from g in _TransportContext.School_Adm_Y_StudentDMO
                                       where (a.AMST_Id == b.AMST_Id && a.AMST_Id == g.AMST_Id 
                                      // && g.ASMCL_Id == d.ASMCL_Id 
                                      // && g.ASMS_Id == f.ASMS_Id 
                                       && b.TRMA_Id == c.TRMA_Id && b.MI_Id == data.MI_Id && b.ASTA_FutureClass == d.ASMCL_Id && b.ASTA_FutureAY == data.ASMAY_Id
                                       && b.ASTA_ActiveFlag == true && ((b.ASTA_PickUp_TRMR_Id == data.TRMR_Id || b.ASTA_Drop_TRMR_Id == data.TRMR_Id)) && b.ASTA_Amount > 0 && b.ASTA_ApplStatus.Equals("Waiting") && b.ASTA_Regnew == data.RegularNew && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1)
                                       select new TransportApprovedDTO
                                       {
                                           studentname = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "" : " " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "" : " " + a.AMST_LastName)).Trim(),
                                           areaname = c.TRMA_AreaName,
                                           AMST_Id = b.AMST_Id,
                                           ASTA_Id = b.ASTA_Id,
                                           FASMAY_Id = b.ASTA_FutureAY,
                                           applicationno = b.ASTA_ApplicationNo,
                                           pickuproute = b.ASTA_PickUp_TRMR_Id != 0 ? _TransportContext.MasterRouteDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMR_Id == b.ASTA_PickUp_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",
                                           pickuplocation = b.ASTA_PickUp_TRML_Id != 0 ? _TransportContext.MasterLocationDMO.Where(l => l.MI_Id == data.MI_Id && l.TRML_Id == b.ASTA_PickUp_TRML_Id).FirstOrDefault().TRML_LocationName : "--",
                                           drouproute = b.ASTA_Drop_TRMR_Id != 0 ? _TransportContext.MasterRouteDMO.Where(td => td.MI_Id == data.MI_Id && td.TRMR_Id == b.ASTA_Drop_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",
                                           drouplocation = b.ASTA_Drop_TRML_Id != 0 ? _TransportContext.MasterLocationDMO.Where(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == b.ASTA_Drop_TRML_Id).FirstOrDefault().TRML_LocationName : "--",
                                           ASTA_ApplStatus = b.ASTA_ApplStatus,
                                           neworreguular = b.ASTA_Regnew,
                                           ASMCL_ClassName = d.ASMCL_ClassName,
                                          // ASMC_SectionName = f.ASMC_SectionName,
                                           AMST_AdmNo = a.AMST_AdmNo,
                                           AMST_Photoname = a.AMST_Photoname

                                       }).Distinct().ToList();
                        }
                        else
                        {
                            details = (from a in _TransportContext.Adm_M_Student
                                       from b in _TransportContext.Adm_Student_Transport_ApplicationDMO
                                       from c in _TransportContext.MasterAreaDMO
                                       from d in _TransportContext.School_M_Class
                                   //    from f in _TransportContext.School_M_Section
                                       from g in _TransportContext.School_Adm_Y_StudentDMO
                                       where (a.AMST_Id == b.AMST_Id && a.AMST_Id == g.AMST_Id
                                      // && g.ASMCL_Id == d.ASMCL_Id 
                                   //    && g.ASMS_Id == f.ASMS_Id 
                                       && b.TRMA_Id == c.TRMA_Id && b.MI_Id == data.MI_Id && b.ASTA_FutureClass == d.ASMCL_Id && b.ASTA_FutureAY == data.ASMAY_Id
                                       && b.ASTA_ActiveFlag == true && ((b.ASTA_PickUp_TRMR_Id == data.TRMR_Id || b.ASTA_Drop_TRMR_Id == data.TRMR_Id)) && b.ASTA_Amount > 0 && b.ASTA_ApplStatus.Equals("Waiting") && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1)
                                       select new TransportApprovedDTO
                                       {

                                           studentname = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "" : " " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "" : " " + a.AMST_LastName)).Trim(),
                                           areaname = c.TRMA_AreaName,
                                           AMST_Id = b.AMST_Id,
                                           ASTA_Id = b.ASTA_Id,
                                           FASMAY_Id = b.ASTA_FutureAY,
                                           applicationno = b.ASTA_ApplicationNo,
                                           pickuproute = b.ASTA_PickUp_TRMR_Id != 0 ? _TransportContext.MasterRouteDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMR_Id == b.ASTA_PickUp_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",
                                           pickuplocation = b.ASTA_PickUp_TRML_Id != 0 ? _TransportContext.MasterLocationDMO.Where(l => l.MI_Id == data.MI_Id && l.TRML_Id == b.ASTA_PickUp_TRML_Id).FirstOrDefault().TRML_LocationName : "--",
                                           drouproute = b.ASTA_Drop_TRMR_Id != 0 ? _TransportContext.MasterRouteDMO.Where(td => td.MI_Id == data.MI_Id && td.TRMR_Id == b.ASTA_Drop_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",
                                           drouplocation = b.ASTA_Drop_TRML_Id != 0 ? _TransportContext.MasterLocationDMO.Where(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == b.ASTA_Drop_TRML_Id).FirstOrDefault().TRML_LocationName : "--",
                                           ASTA_ApplStatus = b.ASTA_ApplStatus,
                                           neworreguular = b.ASTA_Regnew,
                                           ASMCL_ClassName = d.ASMCL_ClassName,
                                       //    ASMC_SectionName = f.ASMC_SectionName,
                                           AMST_AdmNo = a.AMST_AdmNo,
                                           AMST_Photoname = a.AMST_Photoname

                                       }).Distinct().ToList();
                        }

                    }


                }
                else
                {
                    if (data.TRMR_Id == 0)
                    {
                        if (data.RegularNew == "New" || data.RegularNew == "Regular")
                        {
                            details = (from a in _TransportContext.Adm_M_Student
                                       from b in _TransportContext.Adm_Student_Transport_ApplicationDMO
                                       from c in _TransportContext.MasterAreaDMO
                                       from d in _TransportContext.School_M_Class
                                     //  from f in _TransportContext.School_M_Section
                                       from g in _TransportContext.School_Adm_Y_StudentDMO
                                       where (a.AMST_Id == b.AMST_Id && a.AMST_Id == g.AMST_Id 
                                       //&& g.ASMCL_Id == d.ASMCL_Id
                                      // && g.ASMS_Id == f.ASMS_Id 
                                       && b.TRMA_Id == c.TRMA_Id && b.MI_Id == data.MI_Id && b.ASTA_FutureClass == d.ASMCL_Id && b.ASTA_FutureAY == data.ASMAY_Id
                                       && b.ASTA_FutureClass == data.ASMCL_Id && b.ASTA_ActiveFlag == true  && b.ASTA_Amount > 0 && b.ASTA_ApplStatus.Equals("Waiting") && b.ASTA_Regnew == data.RegularNew && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1)
                                       select new TransportApprovedDTO
                                       {

                                           studentname = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "" : " " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "" : " " + a.AMST_LastName)).Trim(),
                                           areaname = c.TRMA_AreaName,
                                           AMST_Id = b.AMST_Id,
                                           ASTA_Id = b.ASTA_Id,
                                           FASMAY_Id = b.ASTA_FutureAY,
                                           applicationno = b.ASTA_ApplicationNo,
                                           pickuproute = b.ASTA_PickUp_TRMR_Id != 0 ? _TransportContext.MasterRouteDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMR_Id == b.ASTA_PickUp_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",
                                           pickuplocation = b.ASTA_PickUp_TRML_Id != 0 ? _TransportContext.MasterLocationDMO.Where(l => l.MI_Id == data.MI_Id && l.TRML_Id == b.ASTA_PickUp_TRML_Id).FirstOrDefault().TRML_LocationName : "--",
                                           drouproute = b.ASTA_Drop_TRMR_Id != 0 ? _TransportContext.MasterRouteDMO.Where(td => td.MI_Id == data.MI_Id && td.TRMR_Id == b.ASTA_Drop_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",
                                           drouplocation = b.ASTA_Drop_TRML_Id != 0 ? _TransportContext.MasterLocationDMO.Where(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == b.ASTA_Drop_TRML_Id).FirstOrDefault().TRML_LocationName : "--",
                                           ASTA_ApplStatus = b.ASTA_ApplStatus,
                                           neworreguular = b.ASTA_Regnew,
                                           ASMCL_ClassName = d.ASMCL_ClassName,
                                          // ASMC_SectionName = f.ASMC_SectionName,
                                           AMST_AdmNo = a.AMST_AdmNo,
                                           AMST_Photoname = a.AMST_Photoname

                                       }).Distinct().ToList();
                        }
                        else
                        {
                            details = (from a in _TransportContext.Adm_M_Student
                                       from b in _TransportContext.Adm_Student_Transport_ApplicationDMO
                                       from c in _TransportContext.MasterAreaDMO
                                       from d in _TransportContext.School_M_Class
                                    //   from f in _TransportContext.School_M_Section
                                       from g in _TransportContext.School_Adm_Y_StudentDMO
                                       where (a.AMST_Id == b.AMST_Id && a.AMST_Id == g.AMST_Id 
                                      // && g.ASMCL_Id == d.ASMCL_Id 
                                      // && g.ASMS_Id == f.ASMS_Id 
                                       && b.TRMA_Id == c.TRMA_Id && b.MI_Id == data.MI_Id && b.ASTA_FutureClass == d.ASMCL_Id && b.ASTA_FutureAY == data.ASMAY_Id
                                       && b.ASTA_FutureClass == data.ASMCL_Id && b.ASTA_ActiveFlag == true  && b.ASTA_Amount > 0 && b.ASTA_ApplStatus.Equals("Waiting") && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1)
                                       select new TransportApprovedDTO
                                       {

                                           studentname = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "" : " " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "" : " " + a.AMST_LastName)).Trim(),
                                           areaname = c.TRMA_AreaName,
                                           AMST_Id = b.AMST_Id,
                                           ASTA_Id = b.ASTA_Id,
                                           FASMAY_Id = b.ASTA_FutureAY,
                                           applicationno = b.ASTA_ApplicationNo,
                                           pickuproute = b.ASTA_PickUp_TRMR_Id != 0 ? _TransportContext.MasterRouteDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMR_Id == b.ASTA_PickUp_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",
                                           pickuplocation = b.ASTA_PickUp_TRML_Id != 0 ? _TransportContext.MasterLocationDMO.Where(l => l.MI_Id == data.MI_Id && l.TRML_Id == b.ASTA_PickUp_TRML_Id).FirstOrDefault().TRML_LocationName : "--",
                                           drouproute = b.ASTA_Drop_TRMR_Id != 0 ? _TransportContext.MasterRouteDMO.Where(td => td.MI_Id == data.MI_Id && td.TRMR_Id == b.ASTA_Drop_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",
                                           drouplocation = b.ASTA_Drop_TRML_Id != 0 ? _TransportContext.MasterLocationDMO.Where(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == b.ASTA_Drop_TRML_Id).FirstOrDefault().TRML_LocationName : "--",
                                           ASTA_ApplStatus = b.ASTA_ApplStatus,
                                           neworreguular = b.ASTA_Regnew,
                                           ASMCL_ClassName = d.ASMCL_ClassName,
                                         //  ASMC_SectionName = f.ASMC_SectionName,
                                           AMST_AdmNo = a.AMST_AdmNo,
                                           AMST_Photoname = a.AMST_Photoname

                                       }).Distinct().ToList();
                        }
                    }
                    else
                    {
                        if (data.RegularNew == "New" || data.RegularNew == "Regular")
                        {
                            details = (from a in _TransportContext.Adm_M_Student
                                       from b in _TransportContext.Adm_Student_Transport_ApplicationDMO
                                       from c in _TransportContext.MasterAreaDMO
                                       from d in _TransportContext.School_M_Class
                                      // from f in _TransportContext.School_M_Section
                                       from g in _TransportContext.School_Adm_Y_StudentDMO
                                       where (a.AMST_Id == b.AMST_Id && a.AMST_Id == g.AMST_Id 
                                      // && g.ASMCL_Id == d.ASMCL_Id 
                                      // && g.ASMS_Id == f.ASMS_Id 
                                       && b.TRMA_Id == c.TRMA_Id && b.MI_Id == data.MI_Id && b.ASTA_FutureClass == d.ASMCL_Id && b.ASTA_FutureAY == data.ASMAY_Id
                                       && b.ASTA_FutureClass == data.ASMCL_Id && b.ASTA_ActiveFlag == true && ((b.ASTA_PickUp_TRMR_Id == data.TRMR_Id || b.ASTA_Drop_TRMR_Id == data.TRMR_Id)) && b.ASTA_Amount > 0 && b.ASTA_ApplStatus.Equals("Waiting") && b.ASTA_Regnew == data.RegularNew && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1)
                                       select new TransportApprovedDTO
                                       {

                                           studentname = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "" : " " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "" : " " + a.AMST_LastName)).Trim(),
                                           areaname = c.TRMA_AreaName,
                                           AMST_Id = b.AMST_Id,
                                           ASTA_Id = b.ASTA_Id,
                                           FASMAY_Id = b.ASTA_FutureAY,
                                           applicationno = b.ASTA_ApplicationNo,
                                           pickuproute = b.ASTA_PickUp_TRMR_Id != 0 ? _TransportContext.MasterRouteDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMR_Id == b.ASTA_PickUp_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",
                                           pickuplocation = b.ASTA_PickUp_TRML_Id != 0 ? _TransportContext.MasterLocationDMO.Where(l => l.MI_Id == data.MI_Id && l.TRML_Id == b.ASTA_PickUp_TRML_Id).FirstOrDefault().TRML_LocationName : "--",
                                           drouproute = b.ASTA_Drop_TRMR_Id != 0 ? _TransportContext.MasterRouteDMO.Where(td => td.MI_Id == data.MI_Id && td.TRMR_Id == b.ASTA_Drop_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",
                                           drouplocation = b.ASTA_Drop_TRML_Id != 0 ? _TransportContext.MasterLocationDMO.Where(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == b.ASTA_Drop_TRML_Id).FirstOrDefault().TRML_LocationName : "--",
                                           ASTA_ApplStatus = b.ASTA_ApplStatus,
                                           neworreguular = b.ASTA_Regnew,
                                           ASMCL_ClassName = d.ASMCL_ClassName,
                                          // ASMC_SectionName = f.ASMC_SectionName,
                                           AMST_AdmNo = a.AMST_AdmNo,
                                           AMST_Photoname = a.AMST_Photoname

                                       }).Distinct().ToList();
                        }
                        else
                        {
                            details = (from a in _TransportContext.Adm_M_Student
                                       from b in _TransportContext.Adm_Student_Transport_ApplicationDMO
                                       from c in _TransportContext.MasterAreaDMO
                                       from d in _TransportContext.School_M_Class
                                      // from f in _TransportContext.School_M_Section
                                       from g in _TransportContext.School_Adm_Y_StudentDMO
                                       where (a.AMST_Id == b.AMST_Id && a.AMST_Id == g.AMST_Id 
                                      // && g.ASMCL_Id == d.ASMCL_Id
                                      // && g.ASMS_Id == f.ASMS_Id 
                                       && b.TRMA_Id == c.TRMA_Id && b.MI_Id == data.MI_Id && b.ASTA_FutureClass == d.ASMCL_Id && b.ASTA_FutureAY == data.ASMAY_Id
                                       && b.ASTA_FutureClass == data.ASMCL_Id && b.ASTA_ActiveFlag == true && ((b.ASTA_PickUp_TRMR_Id == data.TRMR_Id || b.ASTA_Drop_TRMR_Id == data.TRMR_Id)) && b.ASTA_Amount > 0 && b.ASTA_ApplStatus.Equals("Waiting") && a.AMST_SOL == "S" && a.AMST_ActiveFlag == 1)
                                       select new TransportApprovedDTO
                                       {

                                           studentname = ((a.AMST_FirstName == null ? "" : a.AMST_FirstName) + (a.AMST_MiddleName == null ? "" : " " + a.AMST_MiddleName) + (a.AMST_LastName == null ? "" : " " + a.AMST_LastName)).Trim(),
                                           areaname = c.TRMA_AreaName,
                                           AMST_Id = b.AMST_Id,
                                           ASTA_Id = b.ASTA_Id,
                                           FASMAY_Id = b.ASTA_FutureAY,
                                           applicationno = b.ASTA_ApplicationNo,
                                           pickuproute = b.ASTA_PickUp_TRMR_Id != 0 ? _TransportContext.MasterRouteDMO.Where(t => t.MI_Id == data.MI_Id && t.TRMR_Id == b.ASTA_PickUp_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",
                                           pickuplocation = b.ASTA_PickUp_TRML_Id != 0 ? _TransportContext.MasterLocationDMO.Where(l => l.MI_Id == data.MI_Id && l.TRML_Id == b.ASTA_PickUp_TRML_Id).FirstOrDefault().TRML_LocationName : "--",
                                           drouproute = b.ASTA_Drop_TRMR_Id != 0 ? _TransportContext.MasterRouteDMO.Where(td => td.MI_Id == data.MI_Id && td.TRMR_Id == b.ASTA_Drop_TRMR_Id).FirstOrDefault().TRMR_RouteName : "--",
                                           drouplocation = b.ASTA_Drop_TRML_Id != 0 ? _TransportContext.MasterLocationDMO.Where(ld => ld.MI_Id == data.MI_Id && ld.TRML_Id == b.ASTA_Drop_TRML_Id).FirstOrDefault().TRML_LocationName : "--",
                                           ASTA_ApplStatus = b.ASTA_ApplStatus,
                                           neworreguular = b.ASTA_Regnew,
                                           ASMCL_ClassName = d.ASMCL_ClassName,
                                         //  ASMC_SectionName = f.ASMC_SectionName,
                                           AMST_AdmNo = a.AMST_AdmNo,
                                           AMST_Photoname = a.AMST_Photoname

                                       }).Distinct().ToList();
                        }
                    }

                    

                }
                
       
                data.getalldetails = details.ToArray();
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
                string changedStudentData1 = "";
                string studentremarkemail1 = "";
                if (data.Flag == "A")
                {

                    for (int i = 0; i < data.Temp_Save_List.Length; i++)
                    {
                        long amstid = data.Temp_Save_List[i].AMST_Id;
                        long fasmayid = data.Temp_Save_List[i].FASMAY_Id;
                        long astaid = data.Temp_Save_List[i].ASTA_Id;
                        long? picksession = data.Temp_Save_List[i].PickUp_Session;
                        long? dropsession = data.Temp_Save_List[i].Drop_Session;
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

                                //added Praveen
                                update.ASTA_PickUp_TRMS_Id = picksession;
                                update.ASTA_Drop_TRMS_Id = dropsession;
                                //End Praveen
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
                                    int y = 0;
                                    int z = 0;
                                    string msg = "";
                                    string msg1 = "";
                                    if (data.emailclick != null)
                                    {
                                        for (int k1 = 0; k1 < data.emailclick.Length; k1++)
                                        {

                                            //// if (data.data_array[j].ASTA_Id == astaid)
                                            //// {
                                            //     if (data.emailclick[k1].remarks1 != null)
                                            //     {
                                            //         if (data.emailclick[k1].remarks1.ToString() != "")
                                            //         {
                                            //             changedStudentData1 = data.data_array[k1].remarks1.ToString();
                                            //             //  studentremarkemail = data.data_array[j].studentremarkemail.ToString();

                                            //         }

                                            //     }
                                            //// }
                                            if (data.data_array[k1].ASTA_Id == astaid)
                                            {
                                                if (data.emailclick[k1].studentremarkemail1.ToString() != "")
                                                {
                                                    // changedStudentData = data.data_array[j].remarks.ToString();
                                                    studentremarkemail1 = data.emailclick[k1].studentremarkemail1.ToString();

                                                }
                                            }

                                            //if (data.emailclick[k1].studentremarkemail1.ToString() != "")
                                            //    {
                                            //        // changedStudentData = data.data_array[j].remarks.ToString();
                                            //        studentremarkemail1 = data.data_array[k1].studentremarkemail1.ToString();

                                            //    }
                                            

                                            try
                                            {
                                                Dictionary<string, string> smsemail1 = new Dictionary<string, string>();
                                                smsemail1.Add("MESSAGE", studentremarkemail1);
                                                data.smsemailarry1 = smsemail1.ToArray();

                                                var studDet1 = _db.Adm_M_Student.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == data.emailclick[k1].AMST_Id).ToList();
                                                if (studDet1.Count > 0)
                                                {
                                                    if (Convert.ToString(studDet1.FirstOrDefault().AMST_emailId) != null)
                                                    {
                                                        y = y + 1;
                                                        try
                                                        {

                                                            //if (data.emailcheck1 == true)
                                                            //{

                                                            //    Email Email1 = new Email(_db);
                                                            //    Email1.sendmailtransreject(data.MI_Id, "TRN-REJECT", smsemail1, studDett1.FirstOrDefault().AMST_emailId, "Transport Status");



                                                            //}


                                                            Email email = new Email(_db);
                                                            //if (data.flagstring == "homework")
                                                            //{
                                                            email.sendmailtransApprove(data.MI_Id, "TRN-Approved", smsemail1,studDet1.FirstOrDefault().AMST_emailId, data.emailclick[k1].AMST_Id, "Transport Status");
                                                            //}
                                                            //else if (dto.flagstring == "classwork")
                                                            //{
                                                            //    // string m = email.sendmail(dto.MI_Id, studDet.FirstOrDefault().AMST_emailId, "classwork", dto.studentemail[k].AMST_Id.Value);
                                                            //}
                                                            //if (data.emailcheck1 == true)
                                                            //{

                                                            //    Email Email1 = new Email(_db);
                                                            //    Email1.sendmailtransreject(data.MI_Id, "TRN-REJECT", smsemail1, studDett1.FirstOrDefault().AMST_emailId, "Transport Status");



                                                            //}


                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            msg = data.emailclick[k1].studentname;
                                                            msg1 += msg;
                                                            Console.WriteLine(ex.Message);
                                                            continue;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        msg = data.emailclick[k1].studentname;
                                                        msg1 += msg;
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);

                                            }
                                        }

                                    }


                                    if (data.smsclick != null)
                                    {
                                        for (int k1 = 0; k1 < data.smsclick.Length; k1++)
                                        {
                                            
                                            if (data.data_array[k1].ASTA_Id == astaid)
                                            {
                                                if (data.smsclick[k1].remarks1.ToString() != "")
                                                {

                                                    changedStudentData1 = data.smsclick[k1].remarks1.ToString();

                                                }
                                            }

                                          

                                            try
                                            {
                                                Dictionary<string, string> sms2 = new Dictionary<string, string>();
                                                sms2.Add("MESSAGE", changedStudentData1);
                                                data.smsemailarry1 = sms2.ToArray();

                                                //   var studDet1 = _db.Adm_M_Student.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == data.emailclick[k1].AMST_Id).ToList();
                                                var studDet1 = _db.Adm_M_Student.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == data.smsclick[k1].AMST_Id).ToList();
                                               // var studDet = _Context.AdmissionStudentDMO.Where(t => t.MI_Id == dto.MI_Id && t.AMST_Id == dto.studentsms[R].AMST_Id).ToList();

                                                if (studDet1.Count > 0)
                                                {
                                                    if (Convert.ToString(studDet1.FirstOrDefault().AMST_MobileNo) != null)
                                                    {
                                                        y = y + 1;
                                                        try
                                                        {
                                                            
                                                            SMS sms1 = new SMS(_db);
                                                            //string s1 = sms1.sendSmsApprove1(data.MI_Id, Convert.ToString(studDet1.FirstOrDefault().AMST_MobileNo), astaid, "TransportStatus", changedStudentData1).Result;
                                                            string s1 = sms1.sendSmsTransport(data.MI_Id, Convert.ToInt64(studDet1.FirstOrDefault().AMST_MobileNo), astaid, "TransportStatus", changedStudentData1).Result;

                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            msg = data.smsclick[k1].studentname;
                                                            msg1 += msg;
                                                            Console.WriteLine(ex.Message);
                                                            continue;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        msg = data.smsclick[k1].studentname;
                                                        msg1 += msg;
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Console.WriteLine(ex.Message);

                                            }
                                        }

                                    }

                                    //if (data.data_array[j].ASTA_Id == astaid)
                                    //{
                                    //    if (data.emailclick[k1].remarks1 != null)
                                    //{
                                    //    if (data.emailclick[k1].remarks1.ToString() != "")
                                    //    {
                                    //        changedStudentData1 = data.data_array[k1].remarks1.ToString();
                                    //        //  studentremarkemail = data.data_array[j].studentremarkemail.ToString();

                                    //    }

                                    //}
                                    //}

                                    //Dictionary<string, string> smsemail1 = new Dictionary<string, string>();
                                    //smsemail1.Add("MESSAGE", studentremarkemail1);
                                    //data.smsemailarry1 = smsemail1.ToArray();

                                    //var studDet1 = _db.Adm_Student_Transport_ApplicationDMO.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == amstid).ToList();
                                    //var studDett1 = _db.Adm_M_Student.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == amstid).ToList();

                                    //if (data.emailcheck1 == true)
                                    //{

                                    //    Email Email1 = new Email(_db);
                                    //    Email1.sendmailtransreject(data.MI_Id, "TRN-REJECT", smsemail1, studDett1.FirstOrDefault().AMST_emailId, "Transport Status");



                                    //}
                                    //if (data.smscheck1 == true)
                                    //{

                                    //    SMS sms1 = new SMS(_db);
                                    //    string s1 = sms1.sendSms(data.MI_Id, Convert.ToInt64(studDet1.FirstOrDefault().ASTA_FatherMobileNo), astaid, "Transport Status", changedStudentData1).Result;
                                    //}
                                    //gayathri






                                    var studDet = _db.Adm_Student_Transport_ApplicationDMO.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == amstid && t.ASTA_Id== astaid).ToList();
                                    var studDett = _db.Adm_M_Student.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == amstid).ToList();


                                    SMS sms = new SMS(_db);
                                    string s = sms.sendSms(data.MI_Id, Convert.ToInt64(studDet.FirstOrDefault().ASTA_FatherMobileNo), "TRN-REJECT", amstid).Result;


                                    Email Email = new Email(_db);
                                    string m = Email.sendmail(data.MI_Id, studDett.FirstOrDefault().AMST_emailId, "TRANSPORT_APPROVED", amstid);

                                    var update1 = _TransportContext.Adm_Student_Transport_ApplicationDMO.Single(a => a.MI_Id == data.MI_Id && a.AMST_Id == amstid && a.ASTA_Id == astaid);

                                    var fmgidlist = (from a in _TransportContext.FeeGroupDMO
                                                     from b in _TransportContext.FeeStudentGroupMappingDMO
                                                     where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.FMG_ActiceFlag == true && a.FMG_CompulsoryFlag == "T" && a.FMG_Id == b.FMG_Id && b.ASMAY_Id == fasmayid && b.AMST_Id == amstid
                                                     select new TransportApprovedDTO
                                                     {
                                                         FMG_Id = a.FMG_Id
                                                     }).Distinct().ToList();



                                    var stu_rec_list = _TransportContext.TR_Student_RouteDMO.Where(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == fasmayid && t.AMST_Id == amstid).ToList();
                                    if (stu_rec_list.Count > 0)
                                    {
                                        var feegrplist = _TransportContext.TR_Student_Route_FeeGroupDMO.Where(t => t.TRSR_Id == stu_rec_list[0].TRSR_Id).ToList();
                                        foreach (var delff in feegrplist)
                                        {
                                            _TransportContext.Remove(delff);
                                        }

                                        foreach (var del_stu in stu_rec_list)
                                        {
                                            _TransportContext.Remove(del_stu);
                                        }

                                        _TransportContext.SaveChanges();
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
                                    _TransportContext.Add(object123);
                                    _TransportContext.SaveChanges();
                                    foreach (var x in fmgidlist)
                                    {
                                        TR_Student_Route_FeeGroupDMO oobj = new TR_Student_Route_FeeGroupDMO();
                                        oobj.TRSR_Id = object123.TRSR_Id;
                                        oobj.FMG_Id = x.FMG_Id;
                                        oobj.TRSRFG_ActiveFlg = true;
                                        _TransportContext.Add(oobj);
                                    }


                                    _TransportContext.SaveChanges();


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
                    string changedStudentData = "";
                    string studentremarkemail = "";
                    long amstid = 0;
                    long astaid = 0;
                    for (int i = 0; i < data.Temp_Save_List.Length; i++)
                    {
                        try
                        {
                             amstid = data.Temp_Save_List[i].AMST_Id;
                            long fasmayid = data.Temp_Save_List[i].FASMAY_Id;
                             astaid = data.Temp_Save_List[i].ASTA_Id;
                           
                            for (int j = 0; j < data.data_array.Count(); j++)
                            {
                                if (data.data_array[j].ASTA_Id== astaid)
                                {
                                    if (data.data_array[j].remarks != null)
                                    {
                                        if (data.data_array[j].remarks.ToString() != "")
                                        {
                                            changedStudentData = data.data_array[j].remarks.ToString();
                                          //  studentremarkemail = data.data_array[j].studentremarkemail.ToString();
                                          
                                        }
                                        if (data.data_array[j].studentremarkemail.ToString() != "")
                                        {
                                           // changedStudentData = data.data_array[j].remarks.ToString();
                                            studentremarkemail = data.data_array[j].studentremarkemail.ToString();

                                        }
                                    }
                                }
                            }

                            var update = _TransportContext.Adm_Student_Transport_ApplicationDMO.Single(a => a.MI_Id == data.MI_Id && a.AMST_Id == amstid && a.ASTA_Id == astaid);
                            update.ASTA_ApplStatus = "Rejected";
                            update.ASTA_ActiveFlag = false;
                            update.ASTA_Remarks = changedStudentData;
                            update.UpdatedDate = DateTime.Now;
                            _TransportContext.Update(update);
                            var kl = _TransportContext.SaveChanges();
                            if (kl > 0)
                            {

                                Dictionary<string, string> smsemail = new Dictionary<string, string>();
                                smsemail.Add("MESSAGE", studentremarkemail);
                                data.smsemailarry = smsemail.ToArray();

                                var studDet = _db.Adm_Student_Transport_ApplicationDMO.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == amstid).ToList();
                                var studDett = _db.Adm_M_Student.Where(t => t.MI_Id == data.MI_Id && t.AMST_Id == amstid).ToList();

                                if (data.emailcheck == true)
                                {
                          
                                    Email Email = new Email(_db);
                                    Email.sendmailtransreject(data.MI_Id, "TRN-REJECT", smsemail, studDett.FirstOrDefault().AMST_emailId, "Transport Status");
                                }
                                if (data.smscheck == true)
                                {
                                   
                                    SMS sms = new SMS(_db);
                                    string s = sms.sendSms(data.MI_Id, Convert.ToInt64(studDet.FirstOrDefault().ASTA_FatherMobileNo), astaid, "Transport Status", changedStudentData).Result;
                                }

                                //SMS sms = new SMS(_db);
                                //string s = sms.sendSms(data.MI_Id, Convert.ToInt64(studDet.FirstOrDefault().ASTA_FatherMobileNo), "TRN-REJECT",astaid).Result;
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
