using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.Fee.FinancialAccounting;
using FeeServiceHub.com.vaps.interfaces.FinancialAccounting;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.Fees.FinancialAccounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.services.FinancialAccounting
{
    public class FAMasterCompanyImpl : interfaces.FinancialAccounting.FAMasterCompanyInterface

    {
        public FeeGroupContext _FeeGroupContext;
        public DomainModelMsSqlServerContext _db;


        readonly ILogger<FAMasterCompanyImpl> _logger;
        public FAMasterCompanyImpl(FeeGroupContext frgContext, DomainModelMsSqlServerContext db)
        {
            //_logger = log;
            _FeeGroupContext = frgContext;
            _db = db;
        }

        public FAMasterCompanyDTO saveDetails(FAMasterCompanyDTO data)
        {

            try
            {


                if (data.FAMCOMP_Id > 0)

                {
                    var duplicate = _FeeGroupContext.FACompanyMasterDMO.Where(r => r.FAMCOMP_PhoneNo == data.FAMCOMP_PhoneNo && r.FAMCOMP_CompanyName == data.FAMCOMP_CompanyName && r.FAMCOMP_EMailId==data.FAMCOMP_EMailId && r.FAMCOMP_Id!=data.FAMCOMP_Id && r.MI_Id==data.MI_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "Duplicate";
                    }
                    else
                    {
                        var result = _FeeGroupContext.FACompanyMasterDMO.Where(d => d.FAMCOMP_Id == data.FAMCOMP_Id && d.FAMCOMP_StatusFlg == "open").FirstOrDefault();



                        if (result.FAMCOMP_Id > 0)
                        {
                            // var result = _FeeGroupContext.FACompanyMasterDMO.Single(d => d.FAMCOMP_Id == data.FAMCOMP_Id);

                            //  result.MI_Id = data.MI_Id;
                            result.FAMCOMP_CompanyName = data.FAMCOMP_CompanyName;
                            result.FAMCOMP_Description = data.FAMCOMP_Description;
                            result.FAMCOMP_CompanyAddress = data.FAMCOMP_CompanyAddress;
                            result.FAMCOMP_EMailId = data.FAMCOMP_EMailId;
                            result.FAMCOMP_PhoneNo = data.FAMCOMP_PhoneNo;
                            result.FAMCOMP_Password = data.FAMCOMP_Password;
                            result.FAMCOMP_SalesTaxNo = data.FAMCOMP_SalesTaxNo;
                            result.FAMCOMP_IncomeTaxNo = data.FAMCOMP_IncomeTaxNo;
                            result.FAMCOMP_CMPTypeFlg = data.FAMCOMP_CMPTypeFlg;
                            result.FAMCOMP_DuplicateVoucherFlg = data.FAMCOMP_DuplicateVoucherFlg;
                            result.FAMCOMP_PrintReceiptFlg = data.FAMCOMP_PrintReceiptFlg;
                            result.FAMCOMP_SetDispFlg = data.FAMCOMP_SetDispFlg;
                            result.FAMCOMP_SetLedgerBalanceFlg = data.FAMCOMP_SetLedgerBalanceFlg;
                            result.FAMCOMP_SetNegBalanceFlg = data.FAMCOMP_SetNegBalanceFlg;
                            result.FAMCOMP_SetTypeFlg = data.FAMCOMP_SetTypeFlg;
                            result.FAMCOMP_UseBillWiseDetailsFlg = data.FAMCOMP_UseBillWiseDetailsFlg;
                            result.FAMCOMP_UseDebitCreditFlg = data.FAMCOMP_UseDebitCreditFlg;
                            result.FAMCOMP_BookBeginingDate = data.FAMCOMP_BookBeginingDate;

                            result.FAMCOMP_UpdatedDate = DateTime.Now;

                            result.FAMCOMP_UpdatedBy = data.user_id;

                            _FeeGroupContext.Update(result);
                            var flag = _FeeGroupContext.SaveChanges();
                            if (flag > 0)
                            {
                                data.returnval = "update";
                            }
                            else
                            {
                                data.returnval = "Notupdate";
                            }
                        }
                        else
                        {
                            data.returnval = "admin";
                        }
                    }


                }
                else
                {
                    var duplicate = _FeeGroupContext.FACompanyMasterDMO.Where(r => r.FAMCOMP_PhoneNo == data.FAMCOMP_PhoneNo && r.FAMCOMP_CompanyName == data.FAMCOMP_CompanyName).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "Duplicate";
                    }
                    else
                    {
                        FACompanyMasterDMO obj = new FACompanyMasterDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.FAMCOMP_CompanyName = data.FAMCOMP_CompanyName;
                        obj.FAMCOMP_Description = data.FAMCOMP_Description;
                        obj.FAMCOMP_CompanyAddress = data.FAMCOMP_CompanyAddress;
                        obj.FAMCOMP_EMailId = data.FAMCOMP_EMailId;
                        obj.FAMCOMP_Password = data.FAMCOMP_Password;
                        obj.FAMCOMP_PhoneNo = data.FAMCOMP_PhoneNo;
                        obj.FAMCOMP_SalesTaxNo = data.FAMCOMP_SalesTaxNo;
                        obj.FAMCOMP_IncomeTaxNo = data.FAMCOMP_IncomeTaxNo;


                        obj.FAMCOMP_CMPTypeFlg = data.FAMCOMP_CMPTypeFlg;
                        obj.FAMCOMP_DuplicateVoucherFlg = data.FAMCOMP_DuplicateVoucherFlg;
                        obj.FAMCOMP_PrintReceiptFlg = data.FAMCOMP_PrintReceiptFlg;
                        obj.FAMCOMP_SetDispFlg = data.FAMCOMP_SetDispFlg;
                        obj.FAMCOMP_SetLedgerBalanceFlg = data.FAMCOMP_SetLedgerBalanceFlg;
                        obj.FAMCOMP_SetNegBalanceFlg = data.FAMCOMP_SetNegBalanceFlg;
                        obj.FAMCOMP_SetTypeFlg = data.FAMCOMP_SetTypeFlg;
                        obj.FAMCOMP_UseBillWiseDetailsFlg = data.FAMCOMP_UseBillWiseDetailsFlg;
                        obj.FAMCOMP_UseDebitCreditFlg = data.FAMCOMP_UseDebitCreditFlg;
                        obj.FAMCOMP_BookBeginingDate = data.FAMCOMP_BookBeginingDate;
                        obj.FAMCOMP_CreatedDate = DateTime.Now;
                        obj.FAMCOMP_UpdatedDate = DateTime.Now;
                        obj.FAMCOMP_CreatedBy = data.user_id;
                        obj.FAMCOMP_UpdatedBy = data.user_id;
                        obj.FAMCOMP_ActiveFlg = true;
                        obj.FAMCOMP_StatusFlg = "open";
                        _FeeGroupContext.Add(obj);
                        var contactExists = _FeeGroupContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            data.returnval = "save";

                        }
                        else
                        {
                            data.returnval = "Notsave";
                        }
                    }

                }



            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
                data.returnval = "Admin";
            }
            return data;
        }
        public FAMasterCompanyDTO editDetails(int id)
        {
            FAMasterCompanyDTO obj = new FAMasterCompanyDTO();
            try
            {
                var edit = _FeeGroupContext.FACompanyMasterDMO.Where(R => R.FAMCOMP_Id == id && R.FAMCOMP_ActiveFlg == true).ToList();
                if (edit.Count > 0)
                {
                   
                        obj.masterCompanyDetails = edit.Distinct().ToArray();
                    
                   
                }
                else
                {
                    obj.returnval = "admin";
                }


            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
            }
            return obj;
        }
        public FAMasterCompanyDTO deleteDetails(FAMasterCompanyDTO data)
        {
           
            try
            {
                if (data.FAMCOMP_Id > 0)
                {
                    var update = _FeeGroupContext.FACompanyMasterDMO.Where(R => R.FAMCOMP_Id == data.FAMCOMP_Id && R.MI_Id == data.MI_Id).FirstOrDefault();
                    if (update.FAMCOMP_ActiveFlg == true)
                    {
                        update.FAMCOMP_ActiveFlg = false;
                    }
                    else
                    {
                        update.FAMCOMP_ActiveFlg = true;
                    }
                    update.FAMCOMP_UpdatedDate = DateTime.Now;
                    update.FAMCOMP_UpdatedBy = data.user_id;
                    _FeeGroupContext.Update(update);
                    var contactExists = _FeeGroupContext.SaveChanges();
                    if (contactExists == 1)
                    {
                        data.returnval = "active";

                    }
                    else
                    {
                        data.returnval = "notactive";
                    }

                }
            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
                data.returnval = "failed";
            }
            return data;
        }


        public FAMasterCompanyDTO Getdetails(FAMasterCompanyDTO data)
        {

            try
            {
                // FAMasterCompanyDTO dto = new FAMasterCompanyDTO();
               
                data.masterCompanyDetails = (_FeeGroupContext.FACompanyMasterDMO.Where(P => P.MI_Id == data.MI_Id)).Distinct().OrderByDescending(P=>P.FAMCOMP_CreatedDate).ToArray();
               

            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
            }
            return data;
        }

        public FAUserComapnyMappingDTO GetCompany(FAUserComapnyMappingDTO data)
        {

            try
            {


                data.fillcompany = _FeeGroupContext.FACompanyMasterDMO.Where(t => t.MI_Id == data.MI_Id && t.FAMCOMP_ActiveFlg == true).Distinct().ToArray();

                data.fillfinacialUser = (from a in _FeeGroupContext.ApplRole
                                         from b in _FeeGroupContext.IVRM_Role_Type
                                         from c in _FeeGroupContext.ApplicationUserRole
                                         from d in _FeeGroupContext.applicationUser
                                         where (b.IVRMR_Id == a.Id && c.RoleTypeId == b.IVRMRT_Id && c.UserId == d.Id )
                                         select new FAUserComapnyMappingDTO
                                         {
                                             UserName = d.UserName,
                                            muser_Id = d.Id
                                         }
                                         ).Distinct().ToArray();


                data.UserCompanyDetails = (from a in _FeeGroupContext.FACompanyMasterDMO
                                  from b in _FeeGroupContext.FAUserCompanyMappingDMO
                                  from c in _FeeGroupContext.applicationUser
                                  where (a.FAMCOMP_Id == b.FAMCOMP_Id && a.FAMCOMP_ActiveFlg == true && b.MI_Id == data.MI_Id && c.Id==b.User_Id)
                                  select new FAUserComapnyMappingDTO
                                  {
                                      FAMCOMP_CompanyName = a.FAMCOMP_CompanyName,
                                      FAUCM_Password = b.FAUCM_Password,
                                      FAUCM_Id = b.FAUCM_Id,
                                      FAUCM_ActiveFlg = b.FAUCM_ActiveFlg,
                                      UserName=c.UserName,
                                      muser_Id=b.User_Id,
                                      FAUCM_CreatedDate=b.FAUCM_CreatedDate

                                  }
                                ).Distinct().OrderByDescending(b=>b.FAUCM_CreatedDate).ToArray();



            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
            }
            return data;
        }

        public FAUserComapnyMappingDTO saveUserDetails(FAUserComapnyMappingDTO data)
        {

            try
            {


                if (data.FAUCM_Id > 0)

                {
                    var duplicate = _FeeGroupContext.FAUserCompanyMappingDMO.Where(r => r.FAMCOMP_Id == data.FAMCOMP_Id && r.User_Id==data.muser_Id && r.FAUCM_Password == data.FAUCM_Password ).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "Duplicate";
                    }
                    else
                    {
                        var result = _FeeGroupContext.FAUserCompanyMappingDMO.Where(d => d.FAUCM_Id == data.FAUCM_Id && d.MI_Id==data.MI_Id ).FirstOrDefault();



                        if (result.FAUCM_Id > 0)
                        {
                            // var result = _FeeGroupContext.FACompanyMasterDMO.Single(d => d.FAMCOMP_Id == data.FAMCOMP_Id);

                            //  result.MI_Id = data.MI_Id;
                            result.FAMCOMP_Id = data.FAMCOMP_Id;
                            result.FAUCM_Password = data.FAUCM_Password;

                            result.User_Id = data.muser_Id;
                            result.FAUCM_UpdatedDate = DateTime.Now;

                            result.FAUCM_UpdatedBY = data.user_id;

                            _FeeGroupContext.Update(result);
                            var flag = _FeeGroupContext.SaveChanges();
                            if (flag > 0)
                            {
                                data.returnval = "update";
                            }
                            else
                            {
                                data.returnval = "Notupdate";
                            }
                        }
                        else
                        {
                            data.returnval = "admin";
                        }
                    }


                }
                else
                {
                    var duplicate = _FeeGroupContext.FAUserCompanyMappingDMO.Where(r => r.FAMCOMP_Id == data.FAMCOMP_Id && r.User_Id==data.muser_Id && r.FAUCM_Password == data.FAUCM_Password).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "Duplicate";
                    }
                    else
                    {
                        FAUserCompanyMappingDMO obj = new FAUserCompanyMappingDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.FAMCOMP_Id = data.FAMCOMP_Id;
                        obj.FAUCM_Password = data.FAUCM_Password;
                        obj.User_Id = data.muser_Id;
                        obj.FAUCM_CreatedDate = DateTime.Now;
                        obj.FAUCM_UpdatedDate = DateTime.Now;
                        obj.FAUCM_CreatedBy = data.user_id;
                        obj.FAUCM_UpdatedBY = data.user_id;
                       // obj.User_Id = data.user_id;
                        obj.FAUCM_ActiveFlg = true;

                        _FeeGroupContext.Add(obj);
                        var contactExists = _FeeGroupContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            data.returnval = "save";

                        }
                        else
                        {
                            data.returnval = "Notsave";
                        }
                    }

                }



            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
                data.returnval = "Admin";
            }
            return data;
        }

        public FAUserComapnyMappingDTO editUserDetails(int id)
        {
            FAUserComapnyMappingDTO obj = new FAUserComapnyMappingDTO();
            try
            {
                var edit = _FeeGroupContext.FAUserCompanyMappingDMO.Where(R => R.FAUCM_Id == id && R.FAUCM_ActiveFlg == true ).ToList();
                if (edit.Count > 0)
                {
                    obj.UserCompanyDetails = edit.Distinct().ToArray();
                }
                else
                {
                    obj.returnval = "admin";
                }


            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
            }
            return obj;
        }


        public FAUserComapnyMappingDTO deleteUserDetails(FAUserComapnyMappingDTO data)
        {
          //  FAUserComapnyMappingDTO obj = new FAUserComapnyMappingDTO();
            try
            {
                
                if (data.FAUCM_Id > 0)
                {
                    var update = _FeeGroupContext.FAUserCompanyMappingDMO.Where(R => R.FAUCM_Id == data.FAUCM_Id && R.MI_Id == data.MI_Id).FirstOrDefault();
                    if (update.FAUCM_ActiveFlg == true)
                    {
                        update.FAUCM_ActiveFlg = false;
                    }
                    else
                    {
                        update.FAUCM_ActiveFlg = true;
                    }
                    update.FAUCM_UpdatedDate = DateTime.Now;
                    update.FAUCM_UpdatedBY = data.user_id;
                    _FeeGroupContext.Update(update);
                    var contactExists = _FeeGroupContext.SaveChanges();
                    if (contactExists == 1)
                    {
                        data.returnval = "active";

                    }
                    else
                    {
                        data.returnval = "notactive";
                    }

                }
            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
                data.returnval = "failed";
            }
            return data;
        }

        
        public FACompanyFYMappingDTO saveFYDetails(FACompanyFYMappingDTO data)
        {

            try
            {


                if (data.FACFYM_Id > 0)

                {
                    var duplicate = _FeeGroupContext.FACompanyFYMappingDMO.Where(r => r.FAMCOMP_Id == data.FAMCOMP_Id && r.IMFY_Id == data.IMFY_Id && r.FACFYM_Id!=data.FACFYM_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "Duplicate";
                    }
                    else
                    {
                        var result = _FeeGroupContext.FACompanyFYMappingDMO.Where(d => d.FACFYM_Id == data.FACFYM_Id).FirstOrDefault();



                        if (result.FACFYM_Id > 0)
                        {
                            // var result = _FeeGroupContext.FACompanyMasterDMO.Single(d => d.FAMCOMP_Id == data.FAMCOMP_Id);

                            //  result.MI_Id = data.MI_Id;
                            result.FAMCOMP_Id = data.FAMCOMP_Id;
                            result.FACFYM_RefNo = data.FACFYM_RefNo;
                            result.IMFY_Id = data.IMFY_Id;
                            result.FACFYM_FinancialYearCloseFlg = data.FACFYM_FinancialYearCloseFlg;
                            result.FACFYM_BBDate = data.FACFYM_BBDate;
                            result.FACFYM_Budget = data.FACFYM_Budget;


                            result.FACFYM_UpdatedDate = DateTime.Now;

                            result.FACFYM_UpdatedBY = data.user_id;

                            _FeeGroupContext.Update(result);
                            var flag = _FeeGroupContext.SaveChanges();
                            if (flag > 0)
                            {
                                data.returnval = "update";
                            }
                            else
                            {
                                data.returnval = "Notupdate";
                            }
                        }
                        else
                        {
                            data.returnval = "admin";
                        }
                    }


                }
                else
                {
                    var duplicate = _FeeGroupContext.FACompanyFYMappingDMO.Where(r => r.FAMCOMP_Id == data.FAMCOMP_Id  && r.MI_Id==data.MI_Id && r.FACFYM_RefNo==data.FACFYM_RefNo).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "Duplicate";
                    }
                    else
                    {
                        FACompanyFYMappingDMO obj = new FACompanyFYMappingDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.FAMCOMP_Id = data.FAMCOMP_Id;
                        obj.IMFY_Id = data.IMFY_Id;
                        obj.FACFYM_FinancialYearCloseFlg = data.FACFYM_FinancialYearCloseFlg;
                        obj.FACFYM_RefNo = data.FACFYM_RefNo;
                        obj.FACFYM_CreatedDate = DateTime.Now;
                        obj.FACFYM_UpdatedDate = DateTime.Now;
                        obj.FACFYM_CreatedBy = data.user_id;
                        obj.FACFYM_UpdatedBY = data.user_id;
                        obj.FACFYM_BBDate = data.FACFYM_BBDate;
                        obj.FACFYM_Budget = data.FACFYM_Budget;

                        obj.FACFYM_ActiveFlg = true;

                        _FeeGroupContext.Add(obj);
                        var contactExists = _FeeGroupContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            data.returnval = "save";

                        }
                        else
                        {
                            data.returnval = "Notsave";
                        }
                    }

                }



            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
                data.returnval = "Admin";
            }
            return data;
        }

        public FACompanyFYMappingDTO GetInitialData(FACompanyFYMappingDTO data)
        {

            try
            {


                data.fillcompany = _FeeGroupContext.FACompanyMasterDMO.Where(t => t.MI_Id == data.MI_Id && t.FAMCOMP_ActiveFlg== true).Distinct().ToArray();

               data.fillfinacialyear = _FeeGroupContext.IVRM_Master_FinancialYear.Distinct().ToArray();

               

                  data.FYDetails = (from a in _FeeGroupContext.FACompanyMasterDMO
                                    from b in _FeeGroupContext.FACompanyFYMappingDMO
                                    from c in _FeeGroupContext.IVRM_Master_FinancialYear
                                    where (a.FAMCOMP_Id == b.FAMCOMP_Id && a.FAMCOMP_ActiveFlg == true && b.MI_Id == data.MI_Id && b.IMFY_Id == c.IMFY_Id)
                                    select new FACompanyFYMappingDTO
                                    {
                                        FAMCOMP_CompanyName = a.FAMCOMP_CompanyName,

                                        IMFY_Id = b.IMFY_Id,
                                        IMFY_FinancialYear = c.IMFY_FinancialYear,
                                        FACFYM_RefNo = b.FACFYM_RefNo,
                                        FACFYM_Budget = b.FACFYM_Budget,
                                        FACFYM_BBDate = b.FACFYM_BBDate,

                                        FACFYM_Id = b.FACFYM_Id,
                                        FACFYM_ActiveFlg = b.FACFYM_ActiveFlg,
                                        FACFYM_CreatedDate = b.FACFYM_CreatedDate

                                    }
                                  ).Distinct().OrderByDescending(b => b.FACFYM_CreatedDate).ToArray();



            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
            }
            return data;
        }

        public FACompanyFYMappingDTO editFYDetails(int id)
        {
            FACompanyFYMappingDTO obj = new FACompanyFYMappingDTO();
            try
            {
                var edit = _FeeGroupContext.FACompanyFYMappingDMO.Where(R => R.FACFYM_Id == id ).ToList();
                if (edit.Count > 0)
                {
                    obj.FYDetails = edit.Distinct().ToArray();
                }
                else
                {
                    obj.returnval = "admin";
                }


            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
            }
            return obj;
        }

        public FACompanyFYMappingDTO deleteFYDetails(FACompanyFYMappingDTO data)
        {
            //  FAUserComapnyMappingDTO obj = new FAUserComapnyMappingDTO();
            try
            {

                if (data.FACFYM_Id > 0)
                {
                    var update = _FeeGroupContext.FACompanyFYMappingDMO.Where(R => R.FACFYM_Id == data.FACFYM_Id).FirstOrDefault();
                    if (update.FACFYM_ActiveFlg == true)
                    {
                        update.FACFYM_ActiveFlg = false;
                    }
                    else
                    {
                        update.FACFYM_ActiveFlg = true;
                    }
                    update.FACFYM_UpdatedDate = DateTime.Now;
                   // update.FACFYM_UpdatedBY = DateTime.Now;
                    _FeeGroupContext.Update(update);
                    var contactExists = _FeeGroupContext.SaveChanges();
                    if (contactExists == 1)
                    {
                        data.returnval = "active";

                    }
                    else
                    {
                        data.returnval = "notactive";
                    }

                }
            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
                data.returnval = "failed";
            }
            return data;
        }

    }
}
