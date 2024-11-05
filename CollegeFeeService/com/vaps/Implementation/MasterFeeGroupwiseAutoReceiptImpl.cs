using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.College.Fees;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using DomainModel.Model.com.vapstech.College.Fees;
using DomainModel.Model;
using DomainModel.Model.com.vaps.Fee;

namespace CollegeFeeService.com.vaps.Implementation
{
    public class MasterFeeGroupwiseAutoReceiptImpl : Interfaces.MasterFeeGroupwiseAutoReceiptInterface
    {
        public CollFeeGroupContext _FeeGroupContext;

        public MasterFeeGroupwiseAutoReceiptImpl(CollFeeGroupContext frgContext)
        {
            _FeeGroupContext = frgContext;

        }
        public MasterFeeGroupwiseAutoReceiptDTO deletedta(MasterFeeGroupwiseAutoReceiptDTO data)
        {
            try
            {
                var query = _FeeGroupContext.Fee_Groupwise_AutoReceipt_GroupsDMO.Where(d => d.FGARG_Id == data.FGARG_Id).ToList();
                if (query.Any())
                {
                    _FeeGroupContext.Remove(query.ElementAt(0));
                    var del = _FeeGroupContext.SaveChanges();

                    if (del > 0)
                    {
                        data.returnvalue = "deleted";
                    }
                    else
                    {
                        data.returnvalue = "failed";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MasterFeeGroupwiseAutoReceiptDTO editdta(MasterFeeGroupwiseAutoReceiptDTO data)
        {
            try
            {
                data.filldata = (from a in _FeeGroupContext.Fee_Groupwise_AutoReceiptDMO
                                 from b in _FeeGroupContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                                 from c in _FeeGroupContext.FeeGroupClgDMO
                                 from d in _FeeGroupContext.AcademicYear
                                 where (a.FGAR_Id == b.FGAR_Id && c.FMG_Id == b.FMG_Id && d.ASMAY_Id == a.ASMAY_Id && a.MI_Id == data.MI_Id && b.FGARG_Id == data.FGARG_Id && a.FGAR_Id == data.FGAR_Id)//Added && a.FGAR_Id==data.FGAR_Id
                                 select new MasterFeeGroupwiseAutoReceiptDTO
                                 {
                                     FGAR_Id = a.FGAR_Id,
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
                                     FGAR_Template_Name = a.FGAR_Template_Name

                                 }
            ).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MasterFeeGroupwiseAutoReceiptDTO getinitialdata(MasterFeeGroupwiseAutoReceiptDTO data)
        {
            try
            {
                List<MasterAcademic> year = new List<MasterAcademic>();
                year = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == data.MI_Id && t.Is_Active == true).ToList();
                data.acayear = year.Distinct().ToArray();
                List<FeeGroupClgDMO> feegroup = new List<FeeGroupClgDMO>();
                feegroup = _FeeGroupContext.FeeGroupClgDMO.Where(t => t.MI_Id == data.MI_Id && t.FMG_ActiceFlag == true).ToList();
                data.fillgroup = feegroup.Distinct().ToArray();
                data.filldata = (from a in _FeeGroupContext.Fee_Groupwise_AutoReceiptDMO
                                 from b in _FeeGroupContext.Fee_Groupwise_AutoReceipt_GroupsDMO
                                 from c in _FeeGroupContext.FeeGroupClgDMO
                                 from d in _FeeGroupContext.AcademicYear
                                 where (a.FGAR_Id == b.FGAR_Id && a.MI_Id == data.MI_Id && c.FMG_Id == b.FMG_Id && d.ASMAY_Id == a.ASMAY_Id)
                                 select new MasterFeeGroupwiseAutoReceiptDTO
                                 {
                                     FMG_Id = c.FMG_Id,
                                     FGAR_Id = a.FGAR_Id,
                                     FGARG_Id = b.FGARG_Id,
                                     FGAR_PrefixName = a.FGAR_PrefixName,
                                     FGAR_SuffixName = a.FGAR_SuffixName,
                                     FMG_GroupName = c.FMG_GroupName,
                                     academicyear = d.ASMAY_Year,
                                     FGAR_Starting_No = a.FGAR_Starting_No
                                 }).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public MasterFeeGroupwiseAutoReceiptDTO svedata(MasterFeeGroupwiseAutoReceiptDTO data)
        {
            try
            {
                //For Update
                if (data.FGAR_Id > 0)
                {
                    var res = _FeeGroupContext.Fee_Groupwise_AutoReceiptDMO.Where(t => t.MI_Id == data.MI_Id && t.FGAR_Id == data.FGAR_Id).ToList(); res.FirstOrDefault().FGAR_Address = data.FGAR_Address;
                    res.FirstOrDefault().FGAR_Name = data.FGAR_Name;
                    res.FirstOrDefault().FGAR_PrefixName = data.FGAR_PrefixName;
                    res.FirstOrDefault().FGAR_SuffixName = data.FGAR_SuffixName;

                    res.FirstOrDefault().FGAR_Starting_No = data.FGAR_Starting_No;
                    res.FirstOrDefault().FGAR_Template_Name = data.FGAR_Template_Name;
                    _FeeGroupContext.Update(res.FirstOrDefault());
                    List<long> FGARId = new List<long>();
                    for (int s = 0; s < data.savegroup.Length; s++)
                    {
                        var cnt = _FeeGroupContext.Fee_Groupwise_AutoReceipt_GroupsDMO.Where(t => t.FMG_Id == data.savegroup[s].FMG_Id && t.FGAR_Id == data.FGAR_Id).ToList();

                        if (cnt.Count() > 0)
                        {

                        }
                        else
                        {
                            FGARId.Add(data.savegroup[s].FMG_Id);
                        }

                    }
                    for (int r = 0; r < FGARId.Count(); r++)
                    {

                        Fee_Groupwise_AutoReceipt_GroupsDMO fgag = new Fee_Groupwise_AutoReceipt_GroupsDMO();

                        fgag.FGAR_Id = data.FGAR_Id;
                        fgag.FMG_Id = FGARId[r];
                        _FeeGroupContext.Add(fgag);
             
                    }
                    

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
                else //For Insert
                {
                    var checkduplicate = _FeeGroupContext.Fee_Groupwise_AutoReceiptDMO.Where(t => t.ASMAY_Id == data.ASMAY_Id && t.FGAR_PrefixName == data.FGAR_PrefixName && t.FGAR_SuffixName == data.FGAR_SuffixName).ToList();

                    if (checkduplicate.Count() > 0)
                    {
                        data.returnvalue = "duplicate";
                        return data;
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
                        FAGR.FGAR_CreatedDate = DateTime.Now; 
                        FAGR.FGAR_UpdatedDate = DateTime.Now;

                        _FeeGroupContext.Add(FAGR);
                        for (int a = 0; a < data.savegroup.Length; a++)
                        {
                            Fee_Groupwise_AutoReceipt_GroupsDMO fgag = new Fee_Groupwise_AutoReceipt_GroupsDMO();

                            fgag.FGAR_Id = FAGR.FGAR_Id;
                            fgag.FMG_Id = data.savegroup[a].FMG_Id;
                            _FeeGroupContext.Add(fgag);
                        }
                        var contactexisttransaction = 0;
                        using (var dbCtxTxn = _FeeGroupContext.Database.BeginTransaction())
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
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

    }
}
