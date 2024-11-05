using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.Library;
using DomainModel.Model.com.vapstech.Library;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Services
{
    public class MasterNonBookImpl : Interfaces.MasterNonBookInterface
    {
        public LibraryContext _LibraryContext;
        public DomainModelMsSqlServerContext _contaxt;
        readonly ILogger<BookRegisterImpl> _logger;
        public MasterNonBookImpl(LibraryContext context, DomainModelMsSqlServerContext context1, ILogger<BookRegisterImpl> log)
        {
            _LibraryContext = context;
            _contaxt = context1;
            _logger = log;
        }

        public async Task<MatserNonBook_DTO> getdetails(MatserNonBook_DTO data)
        {
            //MatserNonBook_DTO data = new MatserNonBook_DTO();
            var retObject = new List<dynamic>();
            try
            {
                data.subjectlist = _LibraryContext.MasterSubject_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMS_ActiveFlg == true).Distinct().OrderBy(t => t.LMS_Id).ToArray();
                data.deptlist = _LibraryContext.MasterDepartmentDMO.Where(t => t.MI_Id == data.MI_Id && t.LMD_ActiveFlg == true).Distinct().OrderBy(t => t.LMD_Id).ToArray();

                data.racklist = _LibraryContext.RackDetailsDMO.Where(t => t.MI_Id == data.MI_Id && t.LMRA_ActiveFlag == true).Distinct().OrderBy(t => t.LMRA_Id).ToArray();

                data.langlist = _LibraryContext.MasterLanguageDMO.Where(t => t.MI_Id == data.MI_Id && t.LML_ActiveFlg == true).Distinct().OrderBy(t => t.LML_Id).ToArray();

                data.vendorlist = _LibraryContext.MasterVanderDMO.Where(t => t.MI_Id == data.MI_Id && t.LMV_ActiveFlg == true).Distinct().OrderBy(t => t.LMV_Id).ToArray();

                data.publisherlst = _LibraryContext.MasterPublisherDMO.Where(t => t.MI_Id == data.MI_Id && t.LMP_ActiveFlg == true).Distinct().OrderBy(t => t.LMP_Id).ToArray();

                data.accessorieslist = _LibraryContext.LIB_Master_Accessories_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMAC_ActiveFlg == true).Distinct().OrderBy(t => t.LMAC_Id).ToArray();

                //data.librarylist = _LibraryContext.LIB_Master_Library_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMAL_ActiveFlag==true).Distinct().OrderBy(t => t.LMAL_Id).ToArray();

                var librarylist = (from a in _LibraryContext.LIB_Master_Library_DMO
                                   from b in _LibraryContext.LIB_User_Library_DMO
                                   where (a.MI_Id == data.MI_Id && a.LMAL_Id == b.LMAL_Id && b.IVRMUL_Id == data.UserId && a.LMAL_ActiveFlag == true && b.LUL_ActiveFlg == true)
                                   select a).Distinct().OrderBy(t => t.LMAL_Id).ToList();


                data.librarylist = librarylist.ToArray();
                if (librarylist.Count > 0)
                {
                    if (data.LMAL_Id == 0)
                    {
                        data.LMAL_Id = librarylist.FirstOrDefault().LMAL_Id;
                    }

                }

                data.categorylist = _LibraryContext.MasterCategoryDMO.Where(t => t.MI_Id == data.MI_Id && t.LMC_ActiveFlag == true && t.LMC_BNBFlg == "Non-Book").Distinct().ToArray();

                data.subscriptionist = _LibraryContext.LIB_Master_Subscription_DMO.Where(t => t.MI_Id == data.MI_Id && t.LMSU_ActiveFlg == true).Distinct().ToArray();

                data.periodicitylist = _LibraryContext.MasterPeriodicityDMO.Where(t => t.MI_Id == data.MI_Id && t.LMPE_ActiveFlg == true).Distinct().ToArray();

                using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Lib_Master_Non_Book_alldata_New";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.MI_Id)
                    });
                    cmd.Parameters.Add(new SqlParameter("@LMAL_Id",
                        SqlDbType.VarChar)
                    {
                        Value = Convert.ToInt64(data.LMAL_Id)
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

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

        public MatserNonBook_DTO Savedata(MatserNonBook_DTO data)
        {
            try
            {
                if (data.LMNBK_Id > 0)
                {

                    var Duplicate = (from a in _LibraryContext.LIB_Master_NonBook_DMO
                                     from b in _LibraryContext.LIB_Master_NonBook_AccnNo_DMO
                                     where (a.LMNBK_Id == b.LMNBK_Id && a.MI_Id == data.MI_Id && a.LMNBK_Id != data.LMNBK_Id && a.LML_Id == data.LML_Id && a.LMNBK_NonBookTitle == data.LMNBK_NonBookTitle)
                                     select a).ToArray().ToList();

                    if (Duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {

                        var update1 = _LibraryContext.LIB_Master_NonBook_DMO.Where(t => t.LMNBK_Id == data.LMNBK_Id).SingleOrDefault();

                        update1.LMNBK_NonBookTitle = data.LMNBK_NonBookTitle;
                        update1.LMNBK_VolumeNo = data.LMNBK_VolumeNo;
                        update1.LMC_Id = data.LMC_Id;
                        update1.LMNBK_IssueNo = data.LMNBK_IssueNo;
                        update1.LML_Id = data.LML_Id;
                        update1.LMD_Id = data.LMD_Id;
                        update1.LMPE_Id = data.LMPE_Id;
                        update1.LMP_Id = data.LMP_Id;
                        update1.LMV_Id = data.LMV_Id;
                        update1.LMSU_Id = Convert.ToInt64(data.LMSU_Id);
                        update1.LMNBK_PeriodicalTypeFlg = data.LMNBK_PeriodicalTypeFlg;
                        update1.LMNBK_ISBN = data.LMNBK_ISBN;
                        update1.LMNBK_PublishDate = data.LMNBK_PublishDate;
                        update1.LMNBK_PurchaseDate = data.LMNBK_PurchaseDate;
                        update1.LMNBK_BindStatus = data.LMNBK_BindStatus;
                        update1.LMNBK_Price = data.LMNBK_Price;
                        update1.LMNBK_WithAccessoriesFlg = data.LMNBK_WithAccessoriesFlg;
                        update1.LMNBK_Discount = data.LMNBK_Discount;
                        update1.LMNBK_DiscountTypeFlg = data.LMNBK_DiscountTypeFlg;

                        update1.LMNBK_NetPrice = data.LMNBK_NetPrice;
                        update1.LMNBK_BindingType = data.LMNBK_BindingType;
                        update1.LMNBK_BillNo = data.LMNBK_BillNo;
                        update1.LMNBK_VoucherNo = data.LMNBK_VoucherNo;
                        update1.LMNBK_NoOfPages = data.LMNBK_NoOfPages;
                        update1.LMNBK_SourceType = data.LMNBK_SourceType;
                        update1.LMNBK_DonarName = data.LMNBK_DonarName;
                        update1.LMNBK_DonarAddress = data.LMNBK_DonarAddress;
                        update1.LMNBK_Keywords = data.LMNBK_Keywords;
                        update1.LMNBK_BillDate = data.LMNBK_BillDate;
                        update1.LMNBK_NoOfCopies = data.LMNBK_NoOfCopies;
                        update1.LMNBK_ReferenceNo = data.LMNBK_ReferenceNo;
                        update1.LMNBK_Description = data.LMNBK_Description;
                        update1.LMNBK_CurrencyType = data.LMNBK_CurrencyType;
                        update1.UpdatedDate = DateTime.Now;
                        update1.UpdatedBy = data.UserId;

                        _LibraryContext.Update(update1);


                        int c = 0;

                        foreach (var abc in data.savetmpdata)
                        {
                            var accno1 = (from a in _LibraryContext.LIB_Master_NonBook_DMO
                                          from b in _LibraryContext.LIB_Master_NonBook_AccnNo_DMO
                                          where (a.LMNBK_Id == b.LMNBK_Id && a.LMNBK_Id != data.LMNBK_Id && a.MI_Id == data.MI_Id && b.LMNBKANO_AccnNo == abc.LMNBKANO_AccnNo)
                                          select b).ToArray().ToList();


                            if (accno1.Count() > 0)
                            {
                                data.chkaccessionno = true;
                                c += 1;
                            }
                        }

                        if (c == 0)
                        {
                            foreach (var act1 in data.savetmpdata)
                            {

                                var update2 = _LibraryContext.LIB_Master_NonBook_AccnNo_DMO.Where(t => t.LMNBKANO_Id == data.LMNBKANO_Id).SingleOrDefault();

                                update2.LMNBKANO_AccnNo = act1.LMNBKANO_AccnNo;
                                update2.LMNBKANO_AvailableStatus = data.LMNBKANO_AvailableStatus;
                                update2.LMRA_Id = data.LMRA_Id;
                                update2.UpdatedDate = DateTime.Now;
                                _LibraryContext.Update(update2);
                            }
                        }



                        var chekkey = _LibraryContext.LIB_Master_NonBook_KeyFactor_DMO.Where(t => t.LMNBK_Id == data.LMNBK_Id).ToList();
                        if (chekkey.Count > 0)
                        {
                            var updKey = _LibraryContext.LIB_Master_NonBook_KeyFactor_DMO.Single(t => t.LMNBK_Id == data.LMNBK_Id);

                            updKey.LMNBK_Id = data.LMNBK_Id;
                            updKey.LMNBKF_KeyFactor = data.LMNBKF_KeyFactor;
                            updKey.LMNBKF_PageNo = data.LMNBKF_PageNo;
                            updKey.UpdatedBy = data.UserId;
                            updKey.UpdatedDate = DateTime.Now;

                            _LibraryContext.Update(updKey);
                        }
                        else
                        {
                            LIB_Master_NonBook_KeyFactor_DMO newobj = new LIB_Master_NonBook_KeyFactor_DMO();

                            newobj.LMNBK_Id = data.LMNBK_Id;
                            newobj.LMNBKF_KeyFactor = data.LMNBKF_KeyFactor;
                            newobj.LMNBKF_PageNo = data.LMNBKF_PageNo;
                            newobj.LMNBKF_ActiveFlg = true;
                            newobj.CreatedBy = data.UserId;
                            newobj.UpdatedBy = data.UserId;
                            newobj.CreatedDate = DateTime.Now;
                            newobj.UpdatedDate = DateTime.Now;

                            _LibraryContext.Add(newobj);
                        }


                        var check1 = _LibraryContext.LIB_Master_NonBook_Library_DMO.Where(t => t.LMNBK_Id == data.LMNBK_Id).ToArray().ToList();

                        if (check1.Count() > 0)
                        {
                            var update8 = _LibraryContext.LIB_Master_NonBook_Library_DMO.Where(t => t.LMNBK_Id == update1.LMNBK_Id && t.LMNBKL_Id == data.LMNBKL_Id).SingleOrDefault();
                            update8.LMNBK_Id = data.LMNBK_Id;
                            update8.LMAL_Id = Convert.ToInt64(data.LMAL_Id);
                            //update8.LMNBKL_ActiveFlg = true;
                            update8.UpdatedDate = DateTime.Now;
                            update8.LMNBKL_UpdatedBy = data.UserId;
                            _LibraryContext.Update(update8);
                        }
                        else
                        {
                            LIB_Master_NonBook_Library_DMO obj5 = new LIB_Master_NonBook_Library_DMO();
                            obj5.LMNBK_Id = data.LMNBK_Id;
                            obj5.LMAL_Id = Convert.ToInt64(data.LMAL_Id);
                            obj5.LMNBKL_ActiveFlg = true;
                            obj5.CreatedDate = DateTime.Now;
                            obj5.UpdatedDate = DateTime.Now;
                            obj5.LMNBKL_UpdatedBy = data.UserId;
                            obj5.LMNBKL_CreatedBy = data.UserId;
                            _LibraryContext.Add(obj5);
                        }

                        var checkAccs = _LibraryContext.LIB_Master_NonBook_Accessories_DMO.Where(t => t.LMNBK_Id == data.LMNBK_Id).ToList();
                        if (checkAccs.Count() > 0)
                        {
                            var updtAcc = _LibraryContext.LIB_Master_NonBook_Accessories_DMO.Single(t => t.LMNBK_Id == data.LMNBK_Id);

                            updtAcc.LMNBK_Id = data.LMNBK_Id;
                            updtAcc.LMAC_Id = Convert.ToInt64(data.LMAC_Id);
                            updtAcc.LMNBKAC_UpdatedBy = data.UserId;
                            updtAcc.UpdatedDate = DateTime.Now;
                            _LibraryContext.Update(updtAcc);
                        }
                        else
                        {
                            LIB_Master_NonBook_Accessories_DMO newobj3 = new LIB_Master_NonBook_Accessories_DMO();

                            newobj3.LMNBK_Id = data.LMNBK_Id;
                            newobj3.LMAC_Id = Convert.ToInt64(data.LMAC_Id);
                            newobj3.LMNBKAC_ActiveFlg = true;
                            newobj3.LMNBKAC_CreatedBy = data.UserId;
                            newobj3.LMNBKAC_UpdatedBy = data.UserId;
                            newobj3.CreatedDate = DateTime.Now;
                            newobj3.UpdatedDate = DateTime.Now;
                            _LibraryContext.Add(newobj3);
                        }

                        int rowAffected = _LibraryContext.SaveChanges();

                        if (rowAffected > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else
                {

                    var Duplicate = (from a in _LibraryContext.LIB_Master_NonBook_DMO
                                     from b in _LibraryContext.LIB_Master_NonBook_AccnNo_DMO
                                     where (a.LMNBK_Id == b.LMNBK_Id && a.MI_Id == data.MI_Id && a.LMNBK_Id != data.LMNBK_Id && a.LML_Id == data.LML_Id && a.LMNBK_NonBookTitle == data.LMNBK_NonBookTitle)
                                     select a).ToArray().ToList();

                    if (Duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {

                        LIB_Master_NonBook_DMO obj1 = new LIB_Master_NonBook_DMO();

                        obj1.MI_Id = data.MI_Id;
                        obj1.LMNBK_NonBookTitle = data.LMNBK_NonBookTitle;
                        obj1.LMPE_Id = data.LMPE_Id;
                        obj1.LMP_Id = data.LMP_Id;
                        obj1.LMD_Id = data.LMD_Id;
                        obj1.LMV_Id = data.LMV_Id;
                        obj1.LMC_Id = data.LMC_Id;
                        obj1.LML_Id = data.LML_Id;
                        obj1.LMSU_Id = Convert.ToInt64(data.LMSU_Id);
                        obj1.LMNBK_NonBookTitle = data.LMNBK_NonBookTitle;
                        obj1.LMNBK_VolumeNo = data.LMNBK_VolumeNo;
                        obj1.LMNBK_IssueNo = data.LMNBK_IssueNo;
                        obj1.LMNBK_PeriodicalTypeFlg = data.LMNBK_PeriodicalTypeFlg;
                        obj1.LMNBK_ISBN = data.LMNBK_ISBN;
                        obj1.LMNBK_PublishDate = data.LMNBK_PublishDate;
                        // obj1.LMB_EntryDate = Convert.ToDateTime(data.LMB_EntryDate);
                        obj1.LMNBK_PurchaseDate = data.LMNBK_PurchaseDate;
                        obj1.LMNBK_BindStatus = data.LMNBK_BindStatus;
                        obj1.LMNBK_Price = data.LMNBK_Price;
                        obj1.LMNBK_WithAccessoriesFlg = data.LMNBK_WithAccessoriesFlg;
                        obj1.LMNBK_Discount = data.LMNBK_Discount;
                        obj1.LMNBK_DiscountTypeFlg = data.LMNBK_DiscountTypeFlg;
                        obj1.LMNBK_NetPrice = data.LMNBK_NetPrice;
                        obj1.LMNBK_BindingType = data.LMNBK_BindingType;
                        obj1.LMNBK_BillNo = data.LMNBK_BillNo;
                        obj1.LMNBK_NoOfPages = data.LMNBK_NoOfPages;
                        obj1.LMNBK_VoucherNo = data.LMNBK_VoucherNo;
                        obj1.LMNBK_NoOfPages = data.LMNBK_NoOfPages;
                        obj1.LMNBK_SourceType = data.LMNBK_SourceType;
                        obj1.LMNBK_DonarName = data.LMNBK_DonarName;
                        obj1.LMNBK_DonarAddress = data.LMNBK_DonarAddress;
                        obj1.LMNBK_Keywords = data.LMNBK_Keywords;
                        obj1.LMNBK_BillDate = data.LMNBK_BillDate;
                        obj1.LMNBK_NoOfCopies = data.LMNBK_NoOfCopies;
                        obj1.LMNBK_ReferenceNo = data.LMNBK_ReferenceNo;
                        obj1.LMNBK_Description = data.LMNBK_Description;
                        obj1.LMNBK_CurrencyType = data.LMNBK_CurrencyType;
                        obj1.LMNBK_ActiveFlg = true;
                        obj1.CreatedDate = DateTime.Now;
                        obj1.UpdatedDate = DateTime.Now;
                        obj1.CreatedBy = data.UserId;
                        obj1.UpdatedBy = data.UserId;

                        _LibraryContext.Add(obj1);


                        LIB_Master_NonBook_Library_DMO obj5 = new LIB_Master_NonBook_Library_DMO();
                        obj5.LMNBK_Id = obj1.LMNBK_Id;
                        obj5.LMAL_Id = data.LMAL_Id;
                        obj5.LMNBKL_ActiveFlg = true;
                        obj5.CreatedDate = DateTime.Now;
                        obj5.UpdatedDate = DateTime.Now;
                        obj5.LMNBKL_CreatedBy = data.UserId;
                        obj5.LMNBKL_UpdatedBy = data.UserId;
                        _LibraryContext.Add(obj5);


                        LIB_Master_NonBook_Accessories_DMO obj6 = new LIB_Master_NonBook_Accessories_DMO();
                        obj6.LMNBK_Id = obj1.LMNBK_Id;
                        obj6.LMAC_Id = Convert.ToInt64(data.LMAC_Id);
                        obj6.LMNBKAC_ActiveFlg = true;
                        obj6.CreatedDate = DateTime.Now;
                        obj6.UpdatedDate = DateTime.Now;
                        obj6.LMNBKAC_CreatedBy = data.UserId;
                        obj6.LMNBKAC_UpdatedBy = data.UserId;
                        _LibraryContext.Add(obj6);


                        LIB_Master_NonBook_KeyFactor_DMO obj8 = new LIB_Master_NonBook_KeyFactor_DMO();
                        obj8.LMNBK_Id = obj1.LMNBK_Id;
                        obj8.LMNBKF_KeyFactor = data.LMNBKF_KeyFactor;
                        obj8.LMNBKF_PageNo = data.LMNBKF_PageNo;
                        obj8.CreatedDate = DateTime.Now;
                        obj8.UpdatedDate = DateTime.Now;
                        obj8.CreatedBy = data.UserId;
                        obj8.UpdatedBy = data.UserId;
                        obj8.LMNBKF_ActiveFlg = true;
                        _LibraryContext.Add(obj8);

                        int c = 0;

                        foreach (var abc in data.savetmpdata)
                        {
                            var accno1 = (from a in _LibraryContext.LIB_Master_NonBook_DMO
                                          from b in _LibraryContext.LIB_Master_NonBook_AccnNo_DMO
                                          where (a.LMNBK_Id == b.LMNBK_Id && a.MI_Id == data.MI_Id && b.LMNBKANO_AccnNo == abc.LMNBKANO_AccnNo)
                                          select b).ToArray().ToList();


                            if (accno1.Count() > 0)
                            {
                                data.chkaccessionno = true;
                                c += 1;
                            }
                        }

                        if (c == 0)
                        {

                            foreach (var act1 in data.savetmpdata)
                            {
                                LIB_Master_NonBook_AccnNo_DMO obj2 = new LIB_Master_NonBook_AccnNo_DMO();
                                obj2.LMNBK_Id = obj1.LMNBK_Id;
                                obj2.LMNBKANO_AccnNo = act1.LMNBKANO_AccnNo;
                                obj2.LMNBKANO_AvailableStatus = data.LMNBKANO_AvailableStatus;
                                obj2.LMRA_Id = data.LMRA_Id;
                                obj2.LMNBKANO_DeletedLostDate = data.LMNBKANO_DeletedLostDate;
                                obj2.LMNBKANO_DeletedLostReason = "";
                                obj2.LMNBKANO_ModeOfPayment = "";
                                obj2.LMNBKANO_AmountColleceted = 0;
                                obj2.LMNBKANO_DeletedLostFlg = false;
                                obj2.LMNBKANO_ActiveFlg = true;
                                obj2.CreatedDate = DateTime.Now;
                                obj2.UpdatedDate = DateTime.Now;

                                _LibraryContext.Add(obj2);
                            }

                            int rowAffected = _LibraryContext.SaveChanges();

                            if (rowAffected > 0)
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.LogInformation("library");
            }
            return data;
        }

        public async Task<MatserNonBook_DTO> Editdata(MatserNonBook_DTO data)
        {
            try
            {

                var retObject1 = new List<dynamic>();
                using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Lib_Master_Non_Book_Editdata";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@LMNBK_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.LMNBK_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@LMNBKANO_Id",
                       SqlDbType.BigInt)
                    {
                        Value = data.LMNBKANO_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

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

                                retObject1.Add((ExpandoObject)dataRow);
                            }

                        }

                        data.editlis = retObject1.ToArray();
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

        public MatserNonBook_DTO deactiveY(MatserNonBook_DTO data)
        {
            try
            {
                var result = _LibraryContext.LIB_Master_NonBook_AccnNo_DMO.Single(t => t.LMNBKANO_Id == data.LMNBKANO_Id);

                if (result.LMNBKANO_ActiveFlg == true)
                {
                    result.LMNBKANO_ActiveFlg = false;
                }
                else if (result.LMNBKANO_ActiveFlg == false)
                {
                    result.LMNBKANO_ActiveFlg = true;
                }

                result.UpdatedDate = DateTime.Now;
                _LibraryContext.Update(result);

                int rowAffected = _LibraryContext.SaveChanges();
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

        public MatserNonBook_DTO searching(MatserNonBook_DTO data)
        {
            try
            {
                switch (data.Delete_Reason)
                {

                    case "0":
                        string str = "";
                        data.Book_Prefix = data.Book_Prefix.ToUpper();
                        data.alldata = (from a in _LibraryContext.LIB_Master_NonBook_DMO
                                        from c in _LibraryContext.MasterDepartmentDMO
                                        from e in _LibraryContext.MasterLanguageDMO
                                        from m in _LibraryContext.LIB_Master_NonBook_AccnNo_DMO
                                        from p in _LibraryContext.MasterPublisherDMO
                                        from r in _LibraryContext.RackDetailsDMO
                                        from ca in _LibraryContext.MasterCategoryDMO
                                        from lnb in _LibraryContext.LIB_Master_NonBook_Library_DMO
                                        from vdr in _LibraryContext.MasterVanderDMO

                                        where (a.MI_Id == data.MI_Id && a.LMC_Id == ca.LMC_Id && c.LMD_Id == a.LMD_Id && a.LMNBK_Id == m.LMNBK_Id && e.LML_Id == a.LML_Id && p.LMP_Id == a.LMP_Id && r.LMRA_Id == m.LMRA_Id && lnb.LMNBK_Id == a.LMNBK_Id && lnb.LMAL_Id == data.LMAL_Id && vdr.LMV_Id == a.LMV_Id && a.LMNBK_NonBookTitle.Contains(data.Book_Prefix))
                                        select new MatserNonBook_DTO
                                        {
                                            LMNBK_Id = a.LMNBK_Id,
                                            LMNBKANO_Id = m.LMNBKANO_Id,
                                            LMNBK_NonBookTitle = a.LMNBK_NonBookTitle,
                                            LMNBKANO_AccnNo = m.LMNBKANO_AccnNo,
                                            LMNBK_PurchaseDate = a.LMNBK_PurchaseDate,
                                            LMD_DepartmentName = c.LMD_DepartmentName,
                                            LML_LanguageName = e.LML_LanguageName,
                                            LMNBKANO_AvailableStatus = m.LMNBKANO_AvailableStatus,
                                            LMNBK_Price = a.LMNBK_Price,
                                            LMNBK_SourceType = a.LMNBK_SourceType,
                                            LMNBK_PublishDate = a.LMNBK_PublishDate,
                                            LMC_CategoryName = ca.LMC_CategoryName,
                                            LMP_PublisherName = p.LMP_PublisherName,
                                            LMNBKANO_ActiveFlg = m.LMNBKANO_ActiveFlg,
                                            LMRA_RackName = r.LMRA_RackName,
                                            LMV_VendorName = vdr.LMV_VendorName,
                                        }).Distinct().OrderByDescending(t => t.LMNBKF_Id).ToArray();
                        break;
                    case "1":
                        data.alldata = (from a in _LibraryContext.LIB_Master_NonBook_DMO
                                        from c in _LibraryContext.MasterDepartmentDMO
                                        from e in _LibraryContext.MasterLanguageDMO
                                        from m in _LibraryContext.LIB_Master_NonBook_AccnNo_DMO
                                        from p in _LibraryContext.MasterPublisherDMO
                                        from r in _LibraryContext.RackDetailsDMO
                                        from ca in _LibraryContext.MasterCategoryDMO
                                        from lnb in _LibraryContext.LIB_Master_NonBook_Library_DMO
                                        from vdr in _LibraryContext.MasterVanderDMO

                                        where (a.MI_Id == data.MI_Id && a.LMC_Id == ca.LMC_Id && c.LMD_Id == a.LMD_Id && a.LMNBK_Id == m.LMNBK_Id && e.LML_Id == a.LML_Id && p.LMP_Id == a.LMP_Id && r.LMRA_Id == m.LMRA_Id && lnb.LMNBK_Id == a.LMNBK_Id && lnb.LMAL_Id == data.LMAL_Id && vdr.LMV_Id == a.LMV_Id && m.LMNBKANO_AccnNo.Contains(data.Book_Prefix))
                                        select new MatserNonBook_DTO
                                        {
                                            LMNBK_Id = a.LMNBK_Id,
                                            LMNBKANO_Id = m.LMNBKANO_Id,
                                            LMNBK_NonBookTitle = a.LMNBK_NonBookTitle,
                                            LMNBKANO_AccnNo = m.LMNBKANO_AccnNo,
                                            LMNBK_PurchaseDate = a.LMNBK_PurchaseDate,
                                            LMD_DepartmentName = c.LMD_DepartmentName,
                                            LML_LanguageName = e.LML_LanguageName,
                                            LMNBKANO_AvailableStatus = m.LMNBKANO_AvailableStatus,
                                            LMNBK_Price = a.LMNBK_Price,
                                            LMNBK_SourceType = a.LMNBK_SourceType,
                                            LMNBK_PublishDate = a.LMNBK_PublishDate,
                                            LMC_CategoryName = ca.LMC_CategoryName,
                                            LMP_PublisherName = p.LMP_PublisherName,
                                            LMNBKANO_ActiveFlg = m.LMNBKANO_ActiveFlg,
                                            LMRA_RackName = r.LMRA_RackName,
                                            LMV_VendorName = vdr.LMV_VendorName,
                                        }).Distinct().OrderByDescending(t => t.LMNBKF_Id).ToArray();
                        break;
                    case "2":
                        data.alldata = (from a in _LibraryContext.LIB_Master_NonBook_DMO
                                        from c in _LibraryContext.MasterDepartmentDMO
                                        from e in _LibraryContext.MasterLanguageDMO
                                        from m in _LibraryContext.LIB_Master_NonBook_AccnNo_DMO
                                        from p in _LibraryContext.MasterPublisherDMO
                                        from r in _LibraryContext.RackDetailsDMO
                                        from ca in _LibraryContext.MasterCategoryDMO
                                        from lnb in _LibraryContext.LIB_Master_NonBook_Library_DMO
                                        from vdr in _LibraryContext.MasterVanderDMO

                                        where (a.MI_Id == data.MI_Id && a.LMC_Id == ca.LMC_Id && c.LMD_Id == a.LMD_Id && a.LMNBK_Id == m.LMNBK_Id && e.LML_Id == a.LML_Id && p.LMP_Id == a.LMP_Id && r.LMRA_Id == m.LMRA_Id && lnb.LMNBK_Id == a.LMNBK_Id && lnb.LMAL_Id == data.LMAL_Id && vdr.LMV_Id == a.LMV_Id && p.LMP_PublisherName.Contains(data.Book_Prefix))
                                        select new MatserNonBook_DTO
                                        {
                                            LMNBK_Id = a.LMNBK_Id,
                                            LMNBKANO_Id = m.LMNBKANO_Id,
                                            LMNBK_NonBookTitle = a.LMNBK_NonBookTitle,
                                            LMNBKANO_AccnNo = m.LMNBKANO_AccnNo,
                                            LMNBK_PurchaseDate = a.LMNBK_PurchaseDate,
                                            LMD_DepartmentName = c.LMD_DepartmentName,
                                            LML_LanguageName = e.LML_LanguageName,
                                            LMNBKANO_AvailableStatus = m.LMNBKANO_AvailableStatus,
                                            LMNBK_Price = a.LMNBK_Price,
                                            LMNBK_SourceType = a.LMNBK_SourceType,
                                            LMNBK_PublishDate = a.LMNBK_PublishDate,
                                            LMC_CategoryName = ca.LMC_CategoryName,
                                            LMP_PublisherName = p.LMP_PublisherName,
                                            LMNBKANO_ActiveFlg = m.LMNBKANO_ActiveFlg,
                                            LMRA_RackName = r.LMRA_RackName,
                                            LMV_VendorName = vdr.LMV_VendorName,
                                        }).Distinct().OrderByDescending(t => t.LMNBKF_Id).ToArray();
                        break;
                    case "3":
                        data.alldata = (from a in _LibraryContext.LIB_Master_NonBook_DMO
                                        from c in _LibraryContext.MasterDepartmentDMO
                                        from e in _LibraryContext.MasterLanguageDMO
                                        from m in _LibraryContext.LIB_Master_NonBook_AccnNo_DMO
                                        from p in _LibraryContext.MasterPublisherDMO
                                        from r in _LibraryContext.RackDetailsDMO
                                        from ca in _LibraryContext.MasterCategoryDMO
                                        from lnb in _LibraryContext.LIB_Master_NonBook_Library_DMO
                                        from vdr in _LibraryContext.MasterVanderDMO

                                        where (a.MI_Id == data.MI_Id && a.LMC_Id == ca.LMC_Id && c.LMD_Id == a.LMD_Id && a.LMNBK_Id == m.LMNBK_Id && e.LML_Id == a.LML_Id && p.LMP_Id == a.LMP_Id && r.LMRA_Id == m.LMRA_Id && lnb.LMNBK_Id == a.LMNBK_Id && lnb.LMAL_Id == data.LMAL_Id && vdr.LMV_Id == a.LMV_Id && c.LMD_DepartmentName.Contains(data.Book_Prefix))
                                        select new MatserNonBook_DTO
                                        {
                                            LMNBK_Id = a.LMNBK_Id,
                                            LMNBKANO_Id = m.LMNBKANO_Id,
                                            LMNBK_NonBookTitle = a.LMNBK_NonBookTitle,
                                            LMNBKANO_AccnNo = m.LMNBKANO_AccnNo,
                                            LMNBK_PurchaseDate = a.LMNBK_PurchaseDate,
                                            LMD_DepartmentName = c.LMD_DepartmentName,
                                            LML_LanguageName = e.LML_LanguageName,
                                            LMNBKANO_AvailableStatus = m.LMNBKANO_AvailableStatus,
                                            LMNBK_Price = a.LMNBK_Price,
                                            LMNBK_SourceType = a.LMNBK_SourceType,
                                            LMNBK_PublishDate = a.LMNBK_PublishDate,
                                            LMC_CategoryName = ca.LMC_CategoryName,
                                            LMP_PublisherName = p.LMP_PublisherName,
                                            LMNBKANO_ActiveFlg = m.LMNBKANO_ActiveFlg,
                                            LMRA_RackName = r.LMRA_RackName,
                                            LMV_VendorName = vdr.LMV_VendorName,
                                        }).Distinct().OrderByDescending(t => t.LMNBKF_Id).ToArray();
                        break;
                    case "5":
                        data.alldata = (from a in _LibraryContext.LIB_Master_NonBook_DMO
                                        from c in _LibraryContext.MasterDepartmentDMO
                                        from e in _LibraryContext.MasterLanguageDMO
                                        from m in _LibraryContext.LIB_Master_NonBook_AccnNo_DMO
                                        from p in _LibraryContext.MasterPublisherDMO
                                        from r in _LibraryContext.RackDetailsDMO
                                        from ca in _LibraryContext.MasterCategoryDMO
                                        from lnb in _LibraryContext.LIB_Master_NonBook_Library_DMO
                                        from vdr in _LibraryContext.MasterVanderDMO

                                        where (a.MI_Id == data.MI_Id && a.LMC_Id == ca.LMC_Id && c.LMD_Id == a.LMD_Id && a.LMNBK_Id == m.LMNBK_Id && e.LML_Id == a.LML_Id && p.LMP_Id == a.LMP_Id && r.LMRA_Id == m.LMRA_Id && lnb.LMNBK_Id == a.LMNBK_Id && lnb.LMAL_Id == data.LMAL_Id && vdr.LMV_Id == a.LMV_Id && e.LML_LanguageName.Contains(data.Book_Prefix))
                                        select new MatserNonBook_DTO
                                        {
                                            LMNBK_Id = a.LMNBK_Id,
                                            LMNBKANO_Id = m.LMNBKANO_Id,
                                            LMNBK_NonBookTitle = a.LMNBK_NonBookTitle,
                                            LMNBKANO_AccnNo = m.LMNBKANO_AccnNo,
                                            LMNBK_PurchaseDate = a.LMNBK_PurchaseDate,
                                            LMD_DepartmentName = c.LMD_DepartmentName,
                                            LML_LanguageName = e.LML_LanguageName,
                                            LMNBKANO_AvailableStatus = m.LMNBKANO_AvailableStatus,
                                            LMNBK_Price = a.LMNBK_Price,
                                            LMNBK_SourceType = a.LMNBK_SourceType,
                                            LMNBK_PublishDate = a.LMNBK_PublishDate,
                                            LMC_CategoryName = ca.LMC_CategoryName,
                                            LMP_PublisherName = p.LMP_PublisherName,
                                            LMNBKANO_ActiveFlg = m.LMNBKANO_ActiveFlg,
                                            LMRA_RackName = r.LMRA_RackName,
                                            LMV_VendorName = vdr.LMV_VendorName,
                                        }).Distinct().OrderByDescending(t => t.LMNBKF_Id).ToArray();
                        break;
                    case "6":
                        data.alldata = (from a in _LibraryContext.LIB_Master_NonBook_DMO
                                        from c in _LibraryContext.MasterDepartmentDMO
                                        from e in _LibraryContext.MasterLanguageDMO
                                        from m in _LibraryContext.LIB_Master_NonBook_AccnNo_DMO
                                        from p in _LibraryContext.MasterPublisherDMO
                                        from r in _LibraryContext.RackDetailsDMO
                                        from ca in _LibraryContext.MasterCategoryDMO
                                        from lnb in _LibraryContext.LIB_Master_NonBook_Library_DMO
                                        from vdr in _LibraryContext.MasterVanderDMO

                                        where (a.MI_Id == data.MI_Id && a.LMC_Id == ca.LMC_Id && c.LMD_Id == a.LMD_Id && a.LMNBK_Id == m.LMNBK_Id && e.LML_Id == a.LML_Id && p.LMP_Id == a.LMP_Id && r.LMRA_Id == m.LMRA_Id && lnb.LMNBK_Id == a.LMNBK_Id && lnb.LMAL_Id == data.LMAL_Id && vdr.LMV_Id == a.LMV_Id && r.LMRA_RackName.Contains(data.Book_Prefix))
                                        select new MatserNonBook_DTO
                                        {
                                            LMNBK_Id = a.LMNBK_Id,
                                            LMNBKANO_Id = m.LMNBKANO_Id,
                                            LMNBK_NonBookTitle = a.LMNBK_NonBookTitle,
                                            LMNBKANO_AccnNo = m.LMNBKANO_AccnNo,
                                            LMNBK_PurchaseDate = a.LMNBK_PurchaseDate,
                                            LMD_DepartmentName = c.LMD_DepartmentName,
                                            LML_LanguageName = e.LML_LanguageName,
                                            LMNBKANO_AvailableStatus = m.LMNBKANO_AvailableStatus,
                                            LMNBK_Price = a.LMNBK_Price,
                                            LMNBK_SourceType = a.LMNBK_SourceType,
                                            LMNBK_PublishDate = a.LMNBK_PublishDate,
                                            LMC_CategoryName = ca.LMC_CategoryName,
                                            LMP_PublisherName = p.LMP_PublisherName,
                                            LMNBKANO_ActiveFlg = m.LMNBKANO_ActiveFlg,
                                            LMRA_RackName = r.LMRA_RackName,
                                            LMV_VendorName = vdr.LMV_VendorName,
                                        }).Distinct().OrderByDescending(t => t.LMNBKF_Id).ToArray();

                        break;

                }



            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }

        public async Task<MatserNonBook_DTO> changelibrary(MatserNonBook_DTO data)
        {
            try
            {

                var retObject1 = new List<dynamic>();
                using (var cmd = _LibraryContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "Lib_Master_Non_Book_Accor_to_Library";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                        SqlDbType.BigInt)
                    {
                        Value = data.MI_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@LMAL_Id",
                    SqlDbType.BigInt)
                    {
                        Value = data.LMAL_Id
                    });

                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

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

                                retObject1.Add((ExpandoObject)dataRow);
                            }

                        }

                        data.alldata = retObject1.ToArray();
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


    }
}
