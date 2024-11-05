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
using PreadmissionDTOs.com.vaps.admission;
using DomainModel.Model.com.vaps.admission;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using DomainModel.Model.com.vaps.Fee;


namespace FeeServiceHub.com.vaps.services
{
    public class FeeMasterGroupwiseAutoReceiptImpl : interfaces.FeeMasterGroupwiseAutoReceiptInterface
    {
        public FeeGroupContext _FeeGroupContext;

        public FeeMasterGroupwiseAutoReceiptImpl(FeeGroupContext frgContext)
        {
            _FeeGroupContext = frgContext;
            //   _AdmissionFormContext = _admc;
        }

        public Fee_Groupwise_AutoReceiptDTO deletedta(Fee_Groupwise_AutoReceiptDTO data)
        {
            try
            {
                var autoreceiptflag = _FeeGroupContext.FeeMasterConfiguration.Where(t => t.MI_Id == data.MI_Id);

                if (autoreceiptflag.FirstOrDefault().FMC_AutoReceiptFeeGroupFlag == 1)
                {
                    var groupdata = _FeeGroupContext.Fee_Groupwise_AutoReceipt_GroupsDMO.Where(t => t.FGAR_Id == data.FGAR_Id).Select(t => t.FMG_Id).ToList();

                    var check_validation = (from a in _FeeGroupContext.FeePaymentDetailsDMO
                                            from b in _FeeGroupContext.FeeTransactionPaymentDMO
                                            from c in _FeeGroupContext.FeeAmountEntryDMO
                                            where (a.FYP_Id == b.FYP_Id && b.FMA_Id == c.FMA_Id && a.ASMAY_ID == c.ASMAY_Id && a.MI_Id == c.MI_Id && a.MI_Id == data.MI_Id && a.ASMAY_ID == data.ASMAY_Id && groupdata.Contains(c.FMG_Id))
                                            select new Fee_Groupwise_AutoReceiptDTO
                                            {
                                                FGAR_PrefixName = a.FYP_Receipt_No
                                            }
              ).OrderByDescending(t => t.FGAR_Id).ToList();

                    if (check_validation.Count == 0)
                    {
                        using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.CommandText = "Delete_Auto_Receipt_Group";

                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@FGAR_Id",
                                SqlDbType.BigInt)
                            {
                                Value = data.FGAR_Id
                            });

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

                            if (cmd.Connection.State != ConnectionState.Open)
                                cmd.Connection.Open();

                            var data1 = cmd.ExecuteNonQuery();

                            if (data1 > 0)
                            {
                                data.returnvalue = "1";
                            }
                            else
                            {
                                data.returnvalue = "2";
                            }
                        }
                    }
                    else
                    {
                        data.returnvalue = "3";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Fee_Groupwise_AutoReceiptDTO editdta(Fee_Groupwise_AutoReceiptDTO data)
        {
            try
            {
                data.filldata = (from a in _FeeGroupContext.Fee_Groupwise_AutoReceiptDMO
                                 from b in _FeeGroupContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                                 from c in _FeeGroupContext.FeeGroupDMO
                                 from d in _FeeGroupContext.AcademicYear
                                 where (a.FGAR_Id == b.FGAR_Id && a.MI_Id == data.MI_Id && c.FMG_Id == b.FMG_Id && d.ASMAY_Id == a.ASMAY_Id && a.FGAR_Id == data.FGAR_Id)
                                 select new Fee_Groupwise_AutoReceiptDTO
                                 {
                                     FGAR_PrefixName = a.FGAR_PrefixName,
                                     FGAR_SuffixName = a.FGAR_SuffixName,
                                     FGAR_PrefixFlag = a.FGAR_PrefixFlag,
                                     FGAR_SuffixFlag = a.FGAR_SuffixFlag,
                                     FMG_GroupName = c.FMG_GroupName,
                                     FGAR_Name = a.FGAR_Name,
                                     FGAR_Address = a.FGAR_Address,
                                     academicyear = d.ASMAY_Year,
                                     FGARG_Id = b.FGARG_Id,
                                     ASMAY_Id = a.ASMAY_Id,
                                     FMG_Id = b.FMG_Id,
                                     FGAR_Starting_No = a.FGAR_Starting_No,
                                     FGAR_Id = a.FGAR_Id,
                                     FGAR_Template_Name = a.FGAR_Template_Name
                                 }
            ).ToArray();

                data.fillgroup = (from a in _FeeGroupContext.FeeGroupDMO
                                  from b in _FeeGroupContext.Yearlygroups
                                  where (a.FMG_Id == b.FMG_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.FMG_ActiceFlag == true)
                                  select new Fee_Groupwise_AutoReceiptDTO
                                  {
                                      FMG_Id = a.FMG_Id,
                                      FMG_GroupName = a.FMG_GroupName,
                                  }
            ).OrderByDescending(t => t.FGAR_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Fee_Groupwise_AutoReceiptDTO getacademicyear(Fee_Groupwise_AutoReceiptDTO data)
        {
            try
            {

                var semid = _FeeGroupContext.Fee_Groupwise_AutoReceipt_GroupsDMO.ToList();

                List<long> FMGId = new List<long>();
                if (semid.Count > 0)
                {
                    foreach (var i in semid)
                    {
                        FMGId.Add(i.FMG_Id);
                    }
                }
                else
                {
                    FMGId.Add(0);
                }

                //&& !FMGId.Contains(a.FMG_Id)
                data.fillgroup = (from a in _FeeGroupContext.FeeGroupDMO
                                  from b in _FeeGroupContext.Yearlygroups
                                  where (a.FMG_Id == b.FMG_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.FMG_ActiceFlag == true )
                                  select new Fee_Groupwise_AutoReceiptDTO
                                  {
                                      FMG_Id = a.FMG_Id,
                                      FMG_GroupName = a.FMG_GroupName,
                                  }
             ).OrderByDescending(t => t.FGAR_Id).ToArray();

                data.filldata = (from a in _FeeGroupContext.Fee_Groupwise_AutoReceiptDMO
                                 from d in _FeeGroupContext.AcademicYear
                                 where (a.MI_Id == data.MI_Id && d.ASMAY_Id == a.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id)
                                 select new Fee_Groupwise_AutoReceiptDTO
                                 {
                                     FGAR_PrefixName = a.FGAR_PrefixName,
                                     FGAR_SuffixName = a.FGAR_SuffixName,
                                     academicyear = d.ASMAY_Year,
                                     FGAR_Id = a.FGAR_Id,
                                     FGAR_Starting_No = a.FGAR_Starting_No
                                 }
             ).OrderByDescending(t => t.FGAR_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Fee_Groupwise_AutoReceiptDTO getinitialdata(Fee_Groupwise_AutoReceiptDTO data)
        {
            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).OrderByDescending(t => t.ASMAY_Order).ToList();
                data.acayear = year.Distinct().ToArray();
      

                data.fillgroup = (from a in _FeeGroupContext.FeeGroupDMO
                                  from b in _FeeGroupContext.Yearlygroups
                                  where (a.FMG_Id == b.FMG_Id && a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && a.FMG_ActiceFlag == true)
                                  select new Fee_Groupwise_AutoReceiptDTO
                                  {
                                      FMG_Id = a.FMG_Id,
                                      FMG_GroupName = a.FMG_GroupName,
                                  }
           ).OrderByDescending(t => t.FGAR_Id).ToArray();

                data.filldata = (from a in _FeeGroupContext.Fee_Groupwise_AutoReceiptDMO
                                 from d in _FeeGroupContext.AcademicYear
                                 where (a.MI_Id == data.MI_Id && d.ASMAY_Id == a.ASMAY_Id && a.ASMAY_Id == data.ASMAY_Id)
                                 select new Fee_Groupwise_AutoReceiptDTO
                                 {
                                     FGAR_PrefixName = a.FGAR_PrefixName,
                                     FGAR_SuffixName = a.FGAR_SuffixName,
                                     academicyear = d.ASMAY_Year,
                                     FGAR_Id = a.FGAR_Id,
                                     FGAR_Starting_No = a.FGAR_Starting_No
                                 }
             ).OrderByDescending(t => t.FGAR_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }

        public Fee_Groupwise_AutoReceiptDTO svedata(Fee_Groupwise_AutoReceiptDTO data)
        {
            try
            {
                TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

                if (data.FGAR_Id > 0)
                {
                    var res = _FeeGroupContext.Fee_Groupwise_AutoReceiptDMO.Where(t => t.MI_Id == data.MI_Id && t.FGAR_Id == data.FGAR_Id).FirstOrDefault();
                    res.FGAR_Address = data.FGAR_Address;
                    res.FGAR_Name = data.FGAR_Name;

                    if (data.FGAR_SuffixFlag == "true")
                    {
                        res.FGAR_SuffixFlag = "1";
                    }
                    else if (data.FGAR_SuffixFlag == "false")
                    {
                        res.FGAR_SuffixFlag = "0";
                    }

                    res.FGAR_PrefixName = data.FGAR_PrefixName;
                    res.FGAR_SuffixName = data.FGAR_SuffixName;
                    res.FGAR_Starting_No = data.FGAR_Starting_No;
                    res.FGAR_Template_Name = data.FGAR_Template_Name;
                    res.FGAR_UpdatedDate = indianTime;
                    res.FGAR_UpdatedBy = data.userid;
                    _FeeGroupContext.Update(res);

                    var result = _FeeGroupContext.Fee_Groupwise_AutoReceipt_GroupsDMO.Where(t => t.FGAR_Id == data.FGAR_Id).ToList();
                    if (result.Count > 0)
                    {
                        _FeeGroupContext.Database.ExecuteSqlCommand("Delete_existing_groups @p0,@p1,@p2", data.MI_Id, data.ASMAY_Id, data.FGAR_Id);

                        for (int b = 0; b < data.savegroup.Length; b++)
                        {
                            Fee_Groupwise_AutoReceipt_GroupsDMO grp = new Fee_Groupwise_AutoReceipt_GroupsDMO();
                            grp.FGAR_Id = data.FGAR_Id;
                            grp.FMG_Id = data.savegroup[b].FMG_Id;
                            _FeeGroupContext.Add(grp);
                        }
                    }

                    //List<long> FGARId = new List<long>();
                    //for (int s = 0; s < data.savegroup.Length; s++)
                    //{
                    //    var cnt = _FeeGroupContext.Fee_Groupwise_AutoReceipt_GroupsDMO.Where(t => t.FMG_Id == data.savegroup[s].FMG_Id && t.FGAR_Id == data.FGAR_Id).ToList();

                    //    if (cnt.Count() > 0)
                    //    {

                    //    }
                    //    else
                    //    {
                    //        FGARId.Add(data.savegroup[s].FMG_Id);
                    //    }

                    //}
                    //for (int r = 0; r < FGARId.Count(); r++)
                    //{

                    //    Fee_Groupwise_AutoReceipt_GroupsDMO fgag = new Fee_Groupwise_AutoReceipt_GroupsDMO();

                    //    fgag.FGAR_Id = data.FGAR_Id;
                    //    fgag.FMG_Id = FGARId[r];
                    //    _FeeGroupContext.Add(fgag);

                    //}

                    var contactexisttransaction = _FeeGroupContext.SaveChanges();
                    if (contactexisttransaction > 0)
                    {
                        data.returnvalue = "updated";
                    }
                    else
                    {
                        data.returnvalue = "updatefailed";
                    }
                }
                else
                {

                    //extra
                    var checkduplicatwo = _FeeGroupContext.Fee_Groupwise_AutoReceiptDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.FGAR_PrefixName == data.FGAR_PrefixName && t.FGAR_SuffixName == data.FGAR_SuffixName).ToList();
                    if (checkduplicatwo.Count > 0)
                    {
                        data.returnvalue = "duplicate";
                    }
                    else
                    {
                        Fee_Groupwise_AutoReceiptDMO FAGR = new Fee_Groupwise_AutoReceiptDMO();
                        FAGR.MI_Id = data.MI_Id;
                        FAGR.ASMAY_Id = data.ASMAY_Id;
                        if (data.FGAR_PrefixFlag == "true")
                        {
                            data.FGAR_PrefixFlag = "1";
                        }
                        else if (data.FGAR_PrefixFlag == "false")
                        {
                            data.FGAR_PrefixFlag = "0";
                        }

                        FAGR.FGAR_PrefixFlag = data.FGAR_PrefixFlag;
                        FAGR.FGAR_PrefixName = data.FGAR_PrefixName;

                        FAGR.FGAR_Starting_No = data.FGAR_Starting_No;
                        FAGR.FGAR_Template_Name = data.FGAR_Template_Name;

                        if (data.FGAR_SuffixFlag == "true")
                        {
                            data.FGAR_SuffixFlag = "1";
                        }
                        else if (data.FGAR_SuffixFlag == "false")
                        {
                            data.FGAR_SuffixFlag = "0";
                        }
                        FAGR.FGAR_SuffixFlag = data.FGAR_SuffixFlag;
                        FAGR.FGAR_SuffixName = data.FGAR_SuffixName;
                        FAGR.FGAR_Name = data.FGAR_Name;
                        FAGR.FGAR_Address = data.FGAR_Address;

                        FAGR.FGAR_CreatedBy = data.userid;
                        FAGR.FGAR_UpdatedBy = data.userid;
                        FAGR.FGAR_CreatedDate = indianTime;
                        FAGR.FGAR_UpdatedDate = indianTime;

                        _FeeGroupContext.Add(FAGR);
                        var check_exist = 0;
                        for (int a = 0; a < data.savegroup.Length; a++)
                        {
                            var res = (from x in _FeeGroupContext.Fee_Groupwise_AutoReceiptDMO
                                       from y in _FeeGroupContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                                       where (x.MI_Id == data.MI_Id && x.ASMAY_Id == data.ASMAY_Id && x.FGAR_Id == y.FGAR_Id && x.FGAR_PrefixName == data.FGAR_PrefixName && x.FGAR_SuffixName == data.FGAR_SuffixName && y.FMG_Id == data.savegroup[a].FMG_Id)
                                       select y).ToList();
                            if (res.Count == 0)
                            {
                                check_exist += 1;
                                Fee_Groupwise_AutoReceipt_GroupsDMO fgag = new Fee_Groupwise_AutoReceipt_GroupsDMO();

                                fgag.FGAR_Id = FAGR.FGAR_Id;
                                fgag.FMG_Id = data.savegroup[a].FMG_Id;

                                _FeeGroupContext.Add(fgag);
                            }

                        }
                        var contactexisttransaction = 0;
                        using (var dbCtxTxn = _FeeGroupContext.Database.BeginTransaction())
                        {
                            if (check_exist > 0)
                            {
                                try
                                {
                                    contactexisttransaction = _FeeGroupContext.SaveChanges();
                                    dbCtxTxn.Commit();
                                    data.returnvalue = "true";

                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex.Message);
                                    dbCtxTxn.Rollback();
                                    data.returnvalue = "false";
                                }
                            }
                            else
                            {
                                data.returnvalue = "duplicate";
                            }
                        }
                    }
                }
                // }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public async Task<Fee_Groupwise_AutoReceiptDTO> printreceipt([FromBody] Fee_Groupwise_AutoReceiptDTO data)
        {

            try
            {
                var htmldata = _FeeGroupContext.Fee_Groupwise_AutoReceiptDMO.Where(t => t.FGAR_Id == data.FGAR_Id).Select(t => t.FGAR_Template_Name).FirstOrDefault();
                data.htmldata = htmldata;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public async Task<Fee_Groupwise_AutoReceiptDTO> get_groupdetails([FromBody] Fee_Groupwise_AutoReceiptDTO data)        {            try            {                data.get_grpDetail = (from a in _FeeGroupContext.Fee_Groupwise_AutoReceiptDMO                                      from b in _FeeGroupContext.Fee_Groupwise_AutoReceipt_GroupsDMO                                      from c in _FeeGroupContext.FeeGroupDMO
                                      where (a.FGAR_Id == b.FGAR_Id && a.FGAR_Id == data.FGAR_Id && c.FMG_Id == b.FMG_Id && a.MI_Id == data.MI_Id && a.ASMAY_Id == data.ASMAY_Id)                                      select new Fee_Groupwise_AutoReceiptDTO                                      {                                          FGAR_Id = a.FGAR_Id,                                          FMG_Id = c.FMG_Id,                                          FMG_GroupName = c.FMG_GroupName,                                      }           ).Distinct().OrderBy(t => t.FGAR_Id).ToArray();            }            catch (Exception ex)            {                Console.WriteLine(ex.Message);            }            return data;        }
    }
}

