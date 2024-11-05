using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Library;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Library;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.Library;

namespace LibraryServiceHub.com.vaps.Services
{
    public class MasterSubscriptionImpl : Interfaces.MasterSubscriptionInterface
    {
        public LibraryContext _LibContext;
        public DomainModelMsSqlServerContext _context;
        public MasterSubscriptionImpl(LibraryContext para, DomainModelMsSqlServerContext para22)
        {
            _LibContext = para;
            _context = para22;
        }

        public async Task<Master_Subscription_DTO> getdetails(Master_Subscription_DTO data)
        {
            try
            {
                data.periodicitylist = _LibContext.MasterPeriodicityDMO.Where(t => t.MI_Id == data.MI_Id && t.LMPE_ActiveFlg == true).Distinct().ToArray();

                data.publisherlist = _LibContext.MasterPublisherDMO.Where(t => t.MI_Id == data.MI_Id && t.LMP_ActiveFlg == true).Distinct().ToArray();

                data.deptlist = _LibContext.MasterDepartmentDMO.Where(t => t.MI_Id == data.MI_Id && t.LMD_ActiveFlg == true).Distinct().ToArray();

                data.vendorlist = _LibContext.MasterVanderDMO.Where(t => t.MI_Id == data.MI_Id && t.LMV_ActiveFlg == true).Distinct().ToArray();

                data.categorylist = _LibContext.MasterCategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.LMC_ActiveFlag == true).Distinct().ToArray();

                data.languagelist = _LibContext.MasterLanguageDMO.Where(t => t.MI_Id == data.MI_Id && t.LML_ActiveFlg == true).Distinct().ToArray();

                using (var cmd = _LibContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "LIB_M_SUBSCRIPTION";

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 8000000;

                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
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
                        data.alldata = retObject.ToArray();
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

        public Master_Subscription_DTO Savedata(Master_Subscription_DTO data)
        {
            try
            {
                Master_NumberingDTO check = new Master_NumberingDTO();
                data.transnumbconfigurationsettingsss = check;
                List<Master_Numbering> MM = new List<Master_Numbering>();
                MM = _context.Master_Numbering.Where(t => t.MI_Id == data.MI_Id && t.IMN_Flag == "Subscription").ToList();
                if (MM.Count() > 0)
                {
                    data.transnumbconfigurationsettingsss.IMN_AutoManualFlag = MM.FirstOrDefault().IMN_AutoManualFlag;
                    data.transnumbconfigurationsettingsss.IMN_DuplicatesFlag = MM.FirstOrDefault().IMN_DuplicatesFlag;
                    data.transnumbconfigurationsettingsss.IMN_Flag = MM.FirstOrDefault().IMN_Flag;
                    data.transnumbconfigurationsettingsss.IMN_Id = MM.FirstOrDefault().IMN_Id;
                    data.transnumbconfigurationsettingsss.IMN_PrefixAcadYearCode = MM.FirstOrDefault().IMN_PrefixAcadYearCode;
                    data.transnumbconfigurationsettingsss.IMN_PrefixCalYearCode = MM.FirstOrDefault().IMN_PrefixCalYearCode;
                    data.transnumbconfigurationsettingsss.IMN_PrefixFinYearCode = MM.FirstOrDefault().IMN_PrefixFinYearCode;
                    data.transnumbconfigurationsettingsss.IMN_PrefixParticular = MM.FirstOrDefault().IMN_PrefixParticular;
                    data.transnumbconfigurationsettingsss.IMN_RestartNumFlag = MM.FirstOrDefault().IMN_RestartNumFlag;
                    data.transnumbconfigurationsettingsss.IMN_StartingNo = MM.FirstOrDefault().IMN_StartingNo;
                    data.transnumbconfigurationsettingsss.IMN_SuffixAcadYearCode = MM.FirstOrDefault().IMN_SuffixAcadYearCode;
                    data.transnumbconfigurationsettingsss.IMN_SuffixCalYearCode = MM.FirstOrDefault().IMN_SuffixCalYearCode;
                    data.transnumbconfigurationsettingsss.IMN_SuffixFinYearCode = MM.FirstOrDefault().IMN_SuffixFinYearCode;
                    data.transnumbconfigurationsettingsss.IMN_SuffixParticular = MM.FirstOrDefault().IMN_SuffixParticular;
                    data.transnumbconfigurationsettingsss.IMN_WidthNumeric = MM.FirstOrDefault().IMN_WidthNumeric;
                    data.transnumbconfigurationsettingsss.IMN_ZeroPrefixFlag = MM.FirstOrDefault().IMN_ZeroPrefixFlag;
                    data.transnumbconfigurationsettingsss.MI_Id = MM.FirstOrDefault().MI_Id;
                }


                if (data.LMSU_Id == 0)
                {
                    if (data.transnumbconfigurationsettingsss.IMN_AutoManualFlag == "Auto")
                    {
                        GenerateTransactionNumbering a = new GenerateTransactionNumbering(_context);
                        data.transnumbconfigurationsettingsss.MI_Id = data.MI_Id;
                        data.transnumbconfigurationsettingsss.ASMAY_Id = data.ASMAY_Id;
                        data.trans_id = a.GenerateNumber(data.transnumbconfigurationsettingsss);

                    }

                    var Duplicate = _LibContext.LIB_Master_Subscription_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMPE_Id == data.LMPE_Id && t.LMP_Id == data.LMP_Id && t.LMD_Id == data.LMD_Id && t.LMV_Id == data.LMV_Id && t.LMC_Id == data.LMC_Id && t.LML_Id == data.LML_Id && t.LMSU_SubscriptionNo == data.trans_id && t.LMSU_PeriodicalTitle == data.LMSU_PeriodicalTitle && t.LMSU_EntryDate == data.LMSU_EntryDate && t.LMSU_SubscriptionDate == data.LMSU_SubscriptionDate).ToList();
                    if (Duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        LIB_Master_Subscription_DMO obj = new LIB_Master_Subscription_DMO();

                        obj.LMSU_Id = data.LMSU_Id;
                        obj.MI_Id = data.MI_Id;
                        obj.LMPE_Id = data.LMPE_Id;
                        obj.LMP_Id = data.LMP_Id;
                        obj.LMD_Id = data.LMD_Id;
                        obj.LMV_Id = data.LMV_Id;
                        obj.LMC_Id = data.LMC_Id;
                        obj.LML_Id = data.LML_Id;
                        obj.LMSU_PeriodicalTitle = data.LMSU_PeriodicalTitle;
                        obj.LMSU_SubscriptionNo = data.trans_id;
                        obj.LMSU_PeriodicalTypeFlg = data.LMSU_PeriodicalTypeFlg;  
                        obj.LMSU_Price = data.LMSU_Price;
                        obj.LMSU_Discount = data.LMSU_Discount;
                        obj.LMSU_DoscountTypeFlg = data.LMSU_DoscountTypeFlg;
                        obj.LMSU_NetPrice = data.LMSU_NetPrice;
                        obj.LMSU_OrderNo = data.LMSU_OrderNo;                        
                        obj.LMSU_NoOfCopies = data.LMSU_NoOfCopies;
                        obj.LMSU_StartVolumeNo = data.LMSU_StartVolumeNo;
                        obj.LMSU_StartIssueNo = data.LMSU_StartIssueNo;                       
                        obj.LMSU_AutoAccnNoFlg = data.LMSU_AutoAccnNoFlg;
                        obj.LMSU_NoOfIssues = data.LMSU_NoOfIssues;
                        obj.LMSU_CurrencyType = data.LMSU_CurrencyType;
                        obj.LMSU_EntryDate = data.LMSU_EntryDate;
                        obj.LMSU_OrderDate = data.LMSU_OrderDate;                     
                        obj.LMSU_SubscriptionDate = data.LMSU_SubscriptionDate;
                        obj.LMSU_ExpiryDate = data.LMSU_ExpiryDate;

                        obj.LMSU_PreTerminationDate = data.LMSU_PreTerminationDate;
                        if(data.LMSU_PreTerminationDate!=null)
                        {
                            obj.LMSU_PreTerminateFlg = true;
                        }
                        else
                        {
                            obj.LMSU_PreTerminateFlg = false;
                        }
                        obj.LMSU_ExpectedDate = data.LMSU_ExpectedDate;

                        obj.LMSU_ActiveFlg = true;
                        obj.CreatedBy = data.UserId;
                        obj.UpdatedBy = data.UserId;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;

                        _LibContext.Add(obj);

                        int s = _LibContext.SaveChanges();
                        if (s > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }


                }
                else if (data.LMSU_Id > 0)
                {
                    var Duplicate = _LibContext.LIB_Master_Subscription_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMSU_Id != data.LMSU_Id && t.LMPE_Id == data.LMPE_Id && t.LMP_Id == data.LMP_Id && t.LMD_Id == data.LMD_Id && t.LMV_Id == data.LMV_Id && t.LMC_Id == data.LMC_Id && t.LML_Id == data.LML_Id && t.LMSU_SubscriptionNo == data.trans_id && t.LMSU_PeriodicalTitle == data.LMSU_PeriodicalTitle && t.LMSU_EntryDate == data.LMSU_EntryDate && t.LMSU_SubscriptionDate == data.LMSU_SubscriptionDate).ToList();
                    if (Duplicate.Count > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var result = _LibContext.LIB_Master_Subscription_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMSU_Id == data.LMSU_Id).SingleOrDefault();

                        result.LMPE_Id = data.LMPE_Id;
                        result.LMP_Id = data.LMP_Id;
                        result.LMD_Id = data.LMD_Id;
                        result.LMV_Id = data.LMV_Id;
                        result.LMC_Id = data.LMV_Id;
                        result.LML_Id = data.LML_Id;
                        result.LMC_Id = data.LMC_Id;
                        result.LMSU_PeriodicalTitle = data.LMSU_PeriodicalTitle;
                        result.LMSU_PeriodicalTypeFlg = data.LMSU_PeriodicalTypeFlg;
                        result.LMSU_Price = data.LMSU_Price;
                        result.LMSU_Discount = data.LMSU_Discount;
                        result.LMSU_DoscountTypeFlg = data.LMSU_DoscountTypeFlg;
                        result.LMSU_NetPrice = data.LMSU_NetPrice;
                        result.LMSU_OrderNo = data.LMSU_OrderNo;
                        result.LMSU_NoOfCopies = data.LMSU_NoOfCopies;
                        result.LMSU_StartVolumeNo = data.LMSU_StartVolumeNo;
                        result.LMSU_StartIssueNo = data.LMSU_StartIssueNo;
                        result.LMSU_NoOfIssues = data.LMSU_NoOfIssues;
                        result.LMSU_CurrencyType = data.LMSU_CurrencyType;
                      

                        result.LMSU_EntryDate = data.LMSU_EntryDate;
                        result.LMSU_OrderDate = data.LMSU_OrderDate;
                        result.LMSU_SubscriptionDate = data.LMSU_SubscriptionDate;
                        result.LMSU_ExpiryDate = data.LMSU_ExpiryDate;
                        result.LMSU_PreTerminationDate = data.LMSU_PreTerminationDate;
                        
                        if (data.LMSU_PreTerminationDate != null)
                        {
                            result.LMSU_PreTerminateFlg = true;
                        }
                        else
                        {
                            result.LMSU_PreTerminateFlg = false;
                        }
                        result.LMSU_ExpectedDate = data.LMSU_ExpectedDate;

                        result.UpdatedBy = data.UserId;
                        result.UpdatedDate = DateTime.Now;

                        _LibContext.Update(result);

                        int s = _LibContext.SaveChanges();
                        if (s > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
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

        public Master_Subscription_DTO EditData(Master_Subscription_DTO data)
        {
            try
            {
                data.editdata = (from a in _LibContext.LIB_Master_Subscription_DMO
                                from b in _LibContext.MasterPublisherDMO
                                from c in _LibContext.MasterPeriodicityDMO
                                from d in _LibContext.MasterDepartmentDMO
                                from e in _LibContext.MasterVanderDMO
                                from f in _LibContext.MasterLanguageDMO
                                where (a.MI_Id == b.MI_Id && a.LMP_Id == b.LMP_Id && a.LMPE_Id == c.LMPE_Id && a.LMD_Id == d.LMD_Id && a.LMV_Id == e.LMV_Id && a.LML_Id == f.LML_Id && a.MI_Id == data.MI_Id && a.LMSU_Id==data.LMSU_Id)
                                select new Master_Subscription_DTO
                                {
                                    LMSU_Id = a.LMSU_Id,
                                    LMPE_Id = a.LMPE_Id,
                                    LMP_Id = a.LMP_Id,
                                    LMD_Id = a.LMD_Id,
                                    LMV_Id = a.LMV_Id,
                                    LMC_Id = a.LMC_Id,
                                    LML_Id = a.LML_Id,
                                    LMSU_PeriodicalTitle = a.LMSU_PeriodicalTitle,
                                    LMSU_SubscriptionNo = a.LMSU_SubscriptionNo,
                                    LMSU_PeriodicalTypeFlg = a.LMSU_PeriodicalTypeFlg,
                                    LMSU_SubscriptionDate = a.LMSU_SubscriptionDate,
                                    LMSU_ExpiryDate = a.LMSU_ExpiryDate,
                                    LMSU_PreTerminationDate = a.LMSU_PreTerminationDate,
                                    LMSU_Price = a.LMSU_Price,
                                    LMSU_Discount = a.LMSU_Discount,
                                    LMSU_DoscountTypeFlg = a.LMSU_DoscountTypeFlg,
                                    LMSU_NetPrice = a.LMSU_NetPrice,
                                    LMSU_OrderNo = a.LMSU_OrderNo,
                                    LMSU_OrderDate = a.LMSU_OrderDate,
                                    LMSU_NoOfCopies = a.LMSU_NoOfCopies,
                                    LMSU_StartVolumeNo = a.LMSU_StartVolumeNo,
                                    LMSU_StartIssueNo = a.LMSU_StartIssueNo,
                                    LMSU_ExpectedDate = a.LMSU_ExpectedDate,
                                    LMSU_AutoAccnNoFlg = a.LMSU_AutoAccnNoFlg,
                                    LMSU_EntryDate = a.LMSU_EntryDate,
                                    LMSU_NoOfIssues = a.LMSU_NoOfIssues,
                                    LMSU_CurrencyType = a.LMSU_CurrencyType,
                                    LMSU_PreTerminateFlg = a.LMSU_PreTerminateFlg,
                                    LMSU_ActiveFlg = a.LMSU_ActiveFlg,
                                    LMP_PublisherName = b.LMP_PublisherName,
                                    LMPE_PeriodicityName = c.LMPE_PeriodicityName,
                                    LMD_DepartmentName = d.LMD_DepartmentName,
                                    LMV_VendorName = e.LMV_VendorName,
                                    LML_LanguageName = f.LML_LanguageName,

                                }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Master_Subscription_DTO deactiveY(Master_Subscription_DTO data)
        {
            try
            {
                var result = _LibContext.LIB_Master_Subscription_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMSU_Id == data.LMSU_Id).SingleOrDefault();

                if (result.LMSU_ActiveFlg == true)
                {
                    result.LMSU_ActiveFlg = false;
                }
                else if (result.LMSU_ActiveFlg == false)
                {
                    result.LMSU_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _LibContext.Update(result);
                int rowAffected = _LibContext.SaveChanges();
                if (rowAffected > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
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
