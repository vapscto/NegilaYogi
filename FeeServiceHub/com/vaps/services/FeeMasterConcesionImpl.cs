using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using FeeServiceHub.com.vaps.interfaces;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.IO;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;
using DomainModel.Model.com.vapstech.Fee;

namespace FeeServiceHub.com.vaps.services
{
    public class FeeMasterConcesionImpl : interfaces.FeeMasterConcessionInterface
    {
         

        public FeeGroupContext _context;
        readonly ILogger<FeeGroupImplimentation> _logger;
        public FeeMasterConcesionImpl(FeeGroupContext frgContext, ILogger<FeeGroupImplimentation> log)
        {
            _context = frgContext;
            _logger = log;
        }

        public FeeMasterConcessionDTO getdata(FeeMasterConcessionDTO data)
        {
            
            try
            {//load 1
                data.savedata = _context.Fee_Master_ConcessionDMO.Where(s => s.MI_Id == data.MI_Id ).ToArray();
               
                //data.savedata2 = _context.Fee_Master_Concession_DetailsDMO.ToArray();
                data.concession = _context.Fee_Master_ConcessionDMO.Where(t => t.MI_Id == data.MI_Id && t.FMCC_ActiveFlag == true).Distinct().ToArray();
                data.concession3 = (from a in _context.Fee_Master_Concession_DetailsDMO
                                    from b in _context.Fee_Master_ConcessionDMO
                                    where (a.FMCC_Id == b.FMCC_Id && b.FMCC_ActiveFlag == true)
                                    select new FeeMasterConcessionDTO
                                    {
                                        FMCC_Id = a.FMCC_Id,
                                        FMCC_ConcessionName = b.FMCC_ConcessionName,

                                    }).Distinct().ToArray();

                //load 2
                data.savedata22 = (from a in _context.Fee_Master_Concession_DetailsDMO
                                  from b in _context.Fee_Master_ConcessionDMO
                                  where (a.FMCC_Id == b.FMCC_Id&&data.MI_Id==b.MI_Id)
                                  select new FeeMasterConcessionDTO
                                  {
                                      FMCC_Id = b.FMCC_Id,
                                      FMCCD_FromNoSibblings = a.FMCCD_FromNoSibblings,
                                      FMCCD_ToNoSibblings = a.FMCCD_ToNoSibblings,
                                      FMCCD_PerOrAmt = a.FMCCD_PerOrAmt,
                                      FMCCD_PerOrAmtFlag = a.FMCCD_PerOrAmtFlag,
                                      FMCC_ConcessionName = b.FMCC_ConcessionName,
                                      FMCCD_ActiveFlg = a.FMCCD_ActiveFlg,
                                      FMCCD_Id = a.FMCCD_Id,
                                  }).Distinct().ToArray();
                //load 3
                data.savedata33 = (from a in _context.Fee_Master_AutoConcession_GroupDMO
                                    from b in _context.Fee_Master_ConcessionDMO
                                    from c in _context.FeeGroupDMO
                                    from d in _context.FeeHeadDMO
                                    where (c.FMG_Id == a.FMG_Id && d.FMH_Id == a.FMH_Id  && b.MI_Id == c.MI_Id && d.MI_Id == data.MI_Id&&a.FMCC_Id==b.FMCC_Id&&d.MI_Id==c.MI_Id)
                                    select new FeeMasterConcessionDTO
                                    {
                                        FMCC_Id = a.FMCC_Id,
                                        FMG_Id = a.FMG_Id,
                                        FMH_Id = a.FMH_Id,
                                        FMACCG_Id = a.FMACCG_Id,
                                        FMCC_ConcessionName = b.FMCC_ConcessionName,
                                        FMG_GroupName = c.FMG_GroupName,
                                        FMH_FeeName = d.FMH_FeeName,
                                        FMACCG_ActiveFlg = a.FMACCG_ActiveFlg,
                                    }).Distinct().ToArray();


                data.group = (from a in _context.FeeGroupDMO
                              from b in _context.Yearlygroups
                              where (a.FMG_Id == b.FMG_Id && a.FMG_ActiceFlag == true && b.FYG_ActiveFlag == true && a.MI_Id == data.MI_Id)
                              select new FeeMasterConcessionDTO
                              {
                                  FMG_Id = a.FMG_Id,
                                  FMG_GroupName = a.FMG_GroupName,
                              }).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                _logger.LogInformation("Transport Error Driver Char savedata" + ex.Message);
            }
            return data;
        }


        public FeeMasterConcessionDTO savedata(FeeMasterConcessionDTO data)
        {
            try
            {
                if (data.FMCC_Id > 0)
                {
                    var check_fee_master_concession_update = _context.Fee_Master_ConcessionDMO.Where(a => a.FMCC_ConcessionName.Equals(data.FMCC_ConcessionName) && a.FMCC_Id != data.FMCC_Id && a.MI_Id==data.MI_Id).ToList();
                   

                    if (check_fee_master_concession_update.Count == 0)
                    {
                        var result = _context.Fee_Master_ConcessionDMO.Single(a => a.FMCC_Id == data.FMCC_Id);
                        result.FMCC_Id = data.FMCC_Id;
                        result.FMCC_ConcessionApplLimit = data.FMCC_ConcessionApplLimit;
                        result.FMCC_ConcessionName = data.FMCC_ConcessionName;
                         result.UpdatedDate = DateTime.Now;
                        //result.FMCC_ConcessionFlag = "";
                        _context.Update(result);
                        int n = _context.SaveChanges();
                        if (n > 0)
                        {
                            data.message = "Update";
                            data.retrunval = true;
                        }
                        else
                        {
                            data.message = "Update";
                            data.retrunval = false;
                        }
                    }
                    else
                    {
                        data.message = "Duplicate";
                    }
                }
                else
                {
                    var check_fee_master_concession= _context.Fee_Master_ConcessionDMO.Where(a => a.FMCC_ConcessionName.Equals(data.FMCC_ConcessionName) && a.MI_Id==data.MI_Id).ToList();

                    if (check_fee_master_concession.Count == 0)
                    {
                        Fee_Master_ConcessionDMO feemastDMO = new Fee_Master_ConcessionDMO();
                        feemastDMO.MI_Id = data.MI_Id;                        
                        feemastDMO.FMCC_ConcessionName = data.FMCC_ConcessionName;
                        feemastDMO.FMCC_ConcessionApplLimit = data.FMCC_ConcessionApplLimit;
                        feemastDMO.CreatedDate = DateTime.Now;
                        feemastDMO.UpdatedDate = DateTime.Now;
                        feemastDMO.FMCC_ActiveFlag = true;
                        feemastDMO.FMCC_ConcessionFlag = "";
                        _context.Add(feemastDMO);

                       

                        int n = _context.SaveChanges();
                        if (n > 0)
                        {
                            data.message = "Add";
                            data.retrunval = true;
                        }
                        else
                        {
                            data.message = "Add";
                            data.retrunval = false;
                        }
                    }
                    else
                    {
                        data.message = "Duplicate";
                    }


                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Transport Error Master Vehicle savedata" + ex.Message);
            }
            return data;
        }
        public FeeMasterConcessionDTO savedata2(FeeMasterConcessionDTO data)
        {
            try
            {
                if (data.FMCCD_Id > 0)
                {
                    var check_fee_master_concession_update2 = _context.Fee_Master_Concession_DetailsDMO.Where(a => a.FMCCD_FromNoSibblings == data.FMCCD_FromNoSibblings &&a.FMCCD_ToNoSibblings==data.FMCCD_ToNoSibblings&&a.FMCCD_PerOrAmt==data.FMCCD_PerOrAmt&& a.FMCCD_Id != data.FMCCD_Id ).ToList();


                    if (check_fee_master_concession_update2.Count == 0)
                    {
                        var result = _context.Fee_Master_Concession_DetailsDMO.Single(a => a.FMCCD_Id == data.FMCCD_Id);
                        
                        result.FMCC_Id = data.FMCC_Id;
                        result.FMCCD_FromNoSibblings = data.FMCCD_FromNoSibblings;
                        result.FMCCD_ToNoSibblings = data.FMCCD_ToNoSibblings;
                        result.FMCCD_PerOrAmt = data.FMCCD_PerOrAmt;
                        result.FMCCD_PerOrAmtFlag = data.FMCCD_PerOrAmtFlag;
                        result.UpdatedDate = DateTime.Now;

                        _context.Update(result);
                        int n = _context.SaveChanges();
                        if (n > 0)
                        {
                            data.message = "Update";
                            data.retrunval = true;
                        }
                        else
                        {
                            data.message = "Update";
                            data.retrunval = false;
                        }
                    }
                    else
                    {
                        data.message = "Duplicate";
                    }
                }
                else
                {
                    var check_fee_master_concession2 = _context.Fee_Master_Concession_DetailsDMO.Where(a => a.FMCCD_FromNoSibblings==data.FMCCD_FromNoSibblings&&a.FMCCD_ToNoSibblings==data.FMCCD_ToNoSibblings&&a.FMCCD_PerOrAmt==data.FMCCD_PerOrAmt&&a.FMCCD_PerOrAmtFlag==data.FMCCD_PerOrAmtFlag).ToList();

                    if (check_fee_master_concession2.Count == 0)
                    {
                        Fee_Master_Concession_DetailsDMO feemastDMO = new Fee_Master_Concession_DetailsDMO();
                      feemastDMO.FMCCD_Id = data.FMCCD_Id;
                        feemastDMO.FMCC_Id = data.FMCC_Id;
                        feemastDMO.FMCCD_FromNoSibblings = data.FMCCD_FromNoSibblings;
                        feemastDMO.FMCCD_ToNoSibblings = data.FMCCD_ToNoSibblings;
                        feemastDMO.FMCCD_PerOrAmt = data.FMCCD_PerOrAmt;
                        feemastDMO.FMCCD_PerOrAmtFlag = data.FMCCD_PerOrAmtFlag;
                        feemastDMO.CreatedDate = DateTime.Now;
                        feemastDMO.UpdatedDate = DateTime.Now;
                        feemastDMO.FMCCD_ActiveFlg = true;
                       
                        _context.Add(feemastDMO);



                        int n = _context.SaveChanges();
                        if (n > 0)
                        {
                            data.message = "Add";
                            data.retrunval = true;
                        }
                        else
                        {
                            data.message = "Add";
                            data.retrunval = false;
                        }
                    }
                    else
                    {
                        data.message = "Duplicate";
                    }


                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeeMasterConcessionDTO savedata3(FeeMasterConcessionDTO data)
        {
            try
            {
                if (data.FMACCG_Id > 0)
                {

                    for (int i = 0; i < data.headlistdata.Length; i++)
                    {
                        var tempdata = data.headlistdata[i].FMH_Id;

                        var check_fee_master_concession_update3 = _context.Fee_Master_AutoConcession_GroupDMO.Where(a => /*a.FMH_Id == data.FMH_Id && */a.FMH_Id == tempdata && a.FMG_Id == data.FMG_Id && a.FMCC_Id == data.FMCC_Id && a.FMACCG_Id != data.FMACCG_Id).ToList();


                    if (check_fee_master_concession_update3.Count == 0)
                    {
                        var result = _context.Fee_Master_AutoConcession_GroupDMO.Single(a => a.FMACCG_Id == data.FMACCG_Id);

                        result.FMCC_Id = data.FMCC_Id;

                        result.FMG_Id = data.FMG_Id;
                            result.FMH_Id = tempdata;

                        result.UpdatedDate = DateTime.Now;

                        _context.Update(result);
                        int n = _context.SaveChanges();
                        if (n > 0)
                        {
                            data.message = "Update";
                            data.retrunval = true;
                        }
                        else
                        {
                            data.message = "Update";
                            data.retrunval = false;
                        }
                    }
                    else
                    {
                        data.message = "Duplicate";
                    }
                }
                }
                else
                {
                    for (int i = 0; i < data.headlistdata.Length; i++)
                    {
                        var tempdata = data.headlistdata[i].FMH_Id;
                        var check_fee_master_concession3 = _context.Fee_Master_AutoConcession_GroupDMO.Where(a =>  a.FMH_Id == tempdata && a.FMG_Id == data.FMG_Id && a.FMCC_Id == data.FMCC_Id).ToList();

                        if (check_fee_master_concession3.Count == 0)
                        {
                            Fee_Master_AutoConcession_GroupDMO feemastDMO = new Fee_Master_AutoConcession_GroupDMO();
                            feemastDMO.FMACCG_Id = data.FMACCG_Id;
                            feemastDMO.FMCC_Id = data.FMCC_Id;
                            feemastDMO.FMH_Id = tempdata;
                            feemastDMO.FMG_Id = data.FMG_Id;

                            feemastDMO.CreatedDate = DateTime.Now;
                            feemastDMO.UpdatedDate = DateTime.Now;
                            feemastDMO.FMACCG_ActiveFlg = true;

                            _context.Add(feemastDMO);



                            int n = _context.SaveChanges();
                            if (n > 0)
                            {
                                data.message = "Add";
                                data.retrunval = true;
                            }
                            else
                            {
                                data.message = "Add";
                                data.retrunval = false;
                            }
                        }
                        else
                        {
                            data.message = "Duplicate";
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeeMasterConcessionDTO gethead(FeeMasterConcessionDTO data)
        {
            try
            {
                data.head = (from a in _context.FeeYearlygroupHeadMappingDMO
                             from b in _context.FeeHeadDMO
                       

                             where (a.FMG_Id == data.FMG_Id && a.MI_Id == data.MI_Id && a.FMH_Id == b.FMH_Id   && b.FMH_Flag != "F" && b.FMH_Flag != "E")
                             select new FeeMasterConcessionDTO
                             {

                                 FMH_Id = b.FMH_Id,
                                 FMH_FeeName = b.FMH_FeeName,

                             }).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }


        public FeeMasterConcessionDTO activedeactive(FeeMasterConcessionDTO data)
        {
            try
            {
                var check_fees_master_concession = (from a in _context.Fee_Master_ConcessionDMO
                                                    where (a.MI_Id==data.MI_Id && a.FMCC_Id == data.FMCC_Id && a.FMCC_ActiveFlag == true)
                                                    select new FeeMasterConcessionDTO
                                                    {
                                                        FMCC_Id = a.FMCC_Id
                                                    }).ToList();

                if (check_fees_master_concession.Count > 0)
                {
                    data.message = "You Can Not Deactivate This Record Its Already Mapped";
                    return data;
                }


                var result = _context.Fee_Master_ConcessionDMO.Single(a => a.MI_Id == data.MI_Id && a.FMCC_Id == data.FMCC_Id);

                    if (result.FMCC_ActiveFlag == false)
                    {
                        result.FMCC_ActiveFlag = true;
                    }
                    else
                    {
                        result.FMCC_ActiveFlag = false;
                    }
                    result.UpdatedDate = DateTime.Now;
                    _context.Update(result);
                    int n = _context.SaveChanges();
                    if (n > 0)
                    {
                        data.retrunval = true;
                    }
                    else
                    {
                        data.retrunval = false;
                    }                
            }
            catch (Exception ex)
            {
                data.message = "You Can Not Deactivate This Record Its Already Mapped";
                _logger.LogInformation("Transport Error Master Area activedeactive" + ex.Message);
            }
            return data;
        }
        public FeeMasterConcessionDTO deactive2(FeeMasterConcessionDTO data)
        {
            try
            {
                var check_fees_master_concession2 = (from a in _context.Adm_M_Student
                                                  
                                                     where ( a.AMST_Concession_Type == data.FMCC_Id && a.MI_Id == data.MI_Id)
                                                    select new FeeMasterConcessionDTO
                                                    {
                                                       
                                                    }).ToList();

                if (check_fees_master_concession2.Count > 0)
                {
                    data.message = "You Can Not Deactivate This Record Its Already Mapped";
                    return data;
                }
                var result = _context.Fee_Master_Concession_DetailsDMO.Single(a => a.FMCCD_Id == data.FMCCD_Id);

                if (result.FMCCD_ActiveFlg == false)
                {
                    result.FMCCD_ActiveFlg = true;

                }
                else
                {
                    result.FMCCD_ActiveFlg = false;

                }
                result.UpdatedDate = DateTime.Now;
                _context.Update(result);
                int n = _context.SaveChanges();
                if (n > 0)
                {
                    data.retrunval = true;
                }
                else
                {
                    data.retrunval = false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



        public FeeMasterConcessionDTO deactive3(FeeMasterConcessionDTO data)
        {
            try
            {
                var check_fees_master_concession3 = (from a in _context.Adm_M_Student

                                                     where (a.AMST_Concession_Type == data.FMCC_Id && a.MI_Id == data.MI_Id)
                                                     select new FeeMasterConcessionDTO
                                                     {

                                                     }).ToList();

                if (check_fees_master_concession3.Count > 0)
                {
                    data.message = "You Can Not Deactivate This Record Its Already Mapped";
                    return data;
                }


                var result = _context.Fee_Master_AutoConcession_GroupDMO.Single(a => a.FMACCG_Id == data.FMACCG_Id);

                if (result.FMACCG_ActiveFlg == false)
                {
                    result.FMACCG_ActiveFlg = true;

                }
                else
                {
                    result.FMACCG_ActiveFlg = false;

                }
                result.UpdatedDate = DateTime.Now;
                _context.Update(result);
                int n = _context.SaveChanges();
                if (n > 0)
                {
                    data.retrunval = true;
                }
                else
                {
                    data.retrunval = false;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



        public FeeMasterConcessionDTO editdata(FeeMasterConcessionDTO data)
        {
            try
            {
                data.editdata = _context.Fee_Master_ConcessionDMO.Where(a =>a.MI_Id==data.MI_Id && a.FMCC_Id == data.FMCC_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Transport Error Master Driver editdata" + ex.Message);
            }
            return data;
        }

        public FeeMasterConcessionDTO edit2(FeeMasterConcessionDTO data)
        {
            try
            {
                //data.editdata2 = _context.Fee_Master_Concession_DetailsDMO.Where(t => t.FMCC_Id == data.FMCC_Id).Distinct().ToArray();
                data.editdata2 = (from a in _context.Fee_Master_Concession_DetailsDMO
                                  from b in _context.Fee_Master_ConcessionDMO
                                  where (a.FMCC_Id == b.FMCC_Id & b.MI_Id == data.MI_Id && a.FMCC_Id == data.FMCC_Id)
                                  select new FeeMasterConcessionDTO
                                  {
                                      FMCCD_Id = a.FMCCD_Id,
                                      FMCC_Id = a.FMCC_Id,
                                      FMCC_ConcessionName = b.FMCC_ConcessionName,
                                      FMCCD_ToNoSibblings = a.FMCCD_ToNoSibblings,
                                      FMCCD_FromNoSibblings = a.FMCCD_FromNoSibblings,
                                      FMCCD_PerOrAmt = a.FMCCD_PerOrAmt,
                                      FMCCD_PerOrAmtFlag = a.FMCCD_PerOrAmtFlag,
                                      FMCCD_ActiveFlg = a.FMCCD_ActiveFlg,
                                  }).Distinct().ToArray();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }







        public FeeMasterConcessionDTO edit3(FeeMasterConcessionDTO data)
        {
            try
            {
                
                //data.editdata3 = ( from a in _context.Fee_Master_Concession_DetailsDMO
                //                  from b in _context.Fee_Master_ConcessionDMO
                //                  where (a.FMCC_Id == b.FMCC_Id & b.MI_Id == data.MI_Id && a.FMCC_Id == data.FMCC_Id)
                //                  select new FeeMasterConcessionDTO
                //                  {
                //                      FMCCD_Id = a.FMCCD_Id,
                //                      FMCC_Id = a.FMCC_Id,
                //                      FMCC_ConcessionName = b.FMCC_ConcessionName,
                //                      FMCCD_ToNoSibblings = a.FMCCD_ToNoSibblings,
                //                      FMCCD_FromNoSibblings = a.FMCCD_FromNoSibblings,
                //                      FMCCD_PerOrAmt = a.FMCCD_PerOrAmt,
                //                      FMCCD_PerOrAmtFlag = a.FMCCD_PerOrAmtFlag,
                //                      FMCCD_ActiveFlg = a.FMCCD_ActiveFlg,
                //                  }).Distinct().ToArray();
                var editlist3 = (from a in _context.AcademicYear
                                from b in _context.Fee_Master_AutoConcession_GroupDMO

                                where (b.FMACCG_Id == data.FMACCG_Id && a.MI_Id == data.MI_Id )
                                select new FeeMasterConcessionDTO
                                {
                                    FMACCG_Id = b.FMACCG_Id,
                                    FMH_Id = b.FMH_Id,
                                   
                                 
                                }).Distinct().ToList();
                data.editlist3 = editlist3.ToArray();
                if (editlist3[0].FMH_Id != 0)
                {
                    var ee = (from a in _context.Fee_Master_AutoConcession_GroupDMO
                              where (a.FMH_Id == editlist3[0].FMH_Id )
                              select new FeeMasterConcessionDTO
                              {

                                 
                                  FMCC_Id = a.FMCC_Id,
                                  FMG_Id = a.FMG_Id,
                                

                              }).Distinct().ToList();
                    data.FMCC_Id = ee[0].FMCC_Id;
                    data.FMG_Id = ee[0].FMG_Id;
                    data.concession3 = (from a in _context.Fee_Master_Concession_DetailsDMO
                                        from b in _context.Fee_Master_ConcessionDMO
                                        where (a.FMCC_Id == b.FMCC_Id && b.FMCC_ActiveFlag == true)
                                        select new FeeMasterConcessionDTO
                                        {
                                            FMCC_Id = a.FMCC_Id,
                                            FMCC_ConcessionName = b.FMCC_ConcessionName,

                                        }).Distinct().ToArray();
                    data.group = (from a in _context.FeeGroupDMO
                                  from b in _context.Yearlygroups
                                  where (a.FMG_Id == b.FMG_Id && a.FMG_ActiceFlag == true && b.FYG_ActiveFlag == true && a.MI_Id == data.MI_Id)
                                  select new FeeMasterConcessionDTO
                                  {
                                      FMG_Id = a.FMG_Id,
                                      FMG_GroupName = a.FMG_GroupName,
                                  }).Distinct().ToArray();
                    data.head = (from a in _context.FeeYearlygroupHeadMappingDMO
                                 from b in _context.FeeHeadDMO


                                 where (a.FMG_Id == data.FMG_Id && a.MI_Id == data.MI_Id && a.FMH_Id == b.FMH_Id && b.FMH_Flag != "F" && b.FMH_Flag != "E")
                                 select new FeeMasterConcessionDTO
                                 {

                                     FMH_Id = b.FMH_Id,
                                     FMH_FeeName = b.FMH_FeeName,

                                 }).Distinct().ToArray();
                    data.savedata33 = (from a in _context.Fee_Master_AutoConcession_GroupDMO
                                       from b in _context.Fee_Master_ConcessionDMO
                                       from c in _context.FeeGroupDMO
                                       from d in _context.FeeHeadDMO
                                       where (c.FMG_Id == a.FMG_Id && d.FMH_Id == a.FMH_Id && b.MI_Id == c.MI_Id && d.MI_Id == data.MI_Id && a.FMCC_Id == b.FMCC_Id && d.MI_Id == c.MI_Id)
                                       select new FeeMasterConcessionDTO
                                       {
                                           FMCC_Id = a.FMCC_Id,
                                           FMG_Id = a.FMG_Id,
                                           FMH_Id = a.FMH_Id,
                                           FMACCG_Id = a.FMACCG_Id,
                                           FMCC_ConcessionName = b.FMCC_ConcessionName,
                                           FMG_GroupName = c.FMG_GroupName,
                                           FMH_FeeName = d.FMH_FeeName,
                                       }).Distinct().ToArray();
                    data.FMCC_Id = ee[0].FMCC_Id;
                    data.FMG_Id = ee[0].FMG_Id;



                }


                    //data.editdata3=(from a in _context.Fee_Master_AutoConcession_GroupDMO
                    //                from b in _context.Fee_Master_ConcessionDMO
                    //                from c in _context.FeeGroupDMO
                    //                from d in _context.FeeHeadDMO
                    //                where()
                    //                )
                }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }



    }
}
