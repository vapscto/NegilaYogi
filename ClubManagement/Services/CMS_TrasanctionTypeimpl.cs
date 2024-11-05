using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.com.vapstech.ClubManagement;
using DomainModel.Model.com.vapstech.ClubManagement;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.ClubManagement;

namespace ClubManagement.Services
{
    public class CMS_TrasanctionTypeimpl : Interfaces.CMS_TrasanctionTypeInterface
    {
        public ClubManagementContext _CmsContext;
        readonly ILogger<CMS_TrasanctionTypeimpl> _logger;
        public CMS_TrasanctionTypeimpl(ClubManagementContext cmsContext)
        {
            _CmsContext = cmsContext;
        }

        public CMS_TrasanctionTypeDTO Getdetails(int id)
        {

            CMS_TrasanctionTypeDTO dto = new CMS_TrasanctionTypeDTO();

            dto.loadDetails = (_CmsContext.CMS_TransactionsTypeDMO.Where(P => P.MI_Id == id)).Distinct().OrderByDescending(P => P.CMSTRANSTY_CreatedDate).ToArray();
            return dto;
        }

        public CMS_TrasanctionTypeDTO saveDetails(CMS_TrasanctionTypeDTO data)
        {

            try
            {


                if (data.CMSTRANSTY_Id > 0)

                {
                    //var duplicate = _CmsContext.FACompanyMasterDMO.Where(r => r.FAMCOMP_PhoneNo == data.FAMCOMP_PhoneNo && r.FAMCOMP_CompanyName == data.FAMCOMP_CompanyName && r.FAMCOMP_EMailId == data.FAMCOMP_EMailId && r.FAMCOMP_Id != data.FAMCOMP_Id && r.MI_Id == data.MI_Id).ToList();
                    //if (duplicate.Count > 0)
                    //{
                    //    data.returnval = "Duplicate";
                    //}
                    //else
                    //{
                        var result = _CmsContext.CMS_TransactionsTypeDMO.Where(d => d.CMSTRANSTY_Id == data.CMSTRANSTY_Id ).FirstOrDefault();



                        if (result.CMSTRANSTY_Id > 0)
                        {
                            // var result = _FeeGroupContext.FACompanyMasterDMO.Single(d => d.FAMCOMP_Id == data.FAMCOMP_Id);

                            //  result.MI_Id = data.MI_Id;
                            result.CMSTRANSTY_TransactionsName = data.CMSTRANSTY_TransactionsName;
                            result.CMSTRANSTY_AliasName = data.CMSTRANSTY_AliasName;
                            result.CMSTRANSTY_Amount = data.CMSTRANSTY_Amount;
                            result.CMSTRANSTY_AllowCreditTransFlg = data.CMSTRANSTY_AllowCreditTransFlg;
                            result.CMSTRANSTY_ConsiderForMinTransFlg = data.CMSTRANSTY_ConsiderForMinTransFlg;
                            result.CMSTRANSTY_CompulsoryFlg = data.CMSTRANSTY_CompulsoryFlg;
                            result.CMSTRANSTY_MemberCanChooseFlg = data.CMSTRANSTY_MemberCanChooseFlg;
                            result.CMSTRANSTY_CoverChargeFlg = data.CMSTRANSTY_CoverChargeFlg;
                            result.CMSTRANSTY_BarTransactionFlg = data.CMSTRANSTY_BarTransactionFlg;
                            result.CMSTRANSTY_FoodTransactionFlg = data.CMSTRANSTY_FoodTransactionFlg;
                            result.CMSTRANSTY_CardTransactionFlg = data.CMSTRANSTY_CardTransactionFlg;
 
                            result.CMSTRANSTY_UpdatedDate = DateTime.Now;

                            result.CMSTRANSTY_UpdatedBy = data.user_id;

                        _CmsContext.Update(result);
                            var flag = _CmsContext.SaveChanges();
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
                  //  }


                }
                else
                {
                    //var duplicate = _FeeGroupContext.FACompanyMasterDMO.Where(r => r.FAMCOMP_PhoneNo == data.FAMCOMP_PhoneNo && r.FAMCOMP_CompanyName == data.FAMCOMP_CompanyName).ToList();
                    //if (duplicate.Count > 0)
                    //{
                    //    data.returnval = "Duplicate";
                    //}
                    //else
                    //{
                    CMS_TransactionsTypeDMO obj = new CMS_TransactionsTypeDMO();
                        obj.MI_Id = data.MI_Id;
                    obj.CMSTRANSTY_TransactionsName = data.CMSTRANSTY_TransactionsName;
                    obj.CMSTRANSTY_AliasName = data.CMSTRANSTY_AliasName;
                    obj.CMSTRANSTY_Amount = data.CMSTRANSTY_Amount;
                    obj.CMSTRANSTY_AllowCreditTransFlg = data.CMSTRANSTY_AllowCreditTransFlg;
                    obj.CMSTRANSTY_ConsiderForMinTransFlg = data.CMSTRANSTY_ConsiderForMinTransFlg;
                    obj.CMSTRANSTY_CompulsoryFlg = data.CMSTRANSTY_CompulsoryFlg;
                    obj.CMSTRANSTY_MemberCanChooseFlg = data.CMSTRANSTY_MemberCanChooseFlg;
                    obj.CMSTRANSTY_CoverChargeFlg = data.CMSTRANSTY_CoverChargeFlg;
                    obj.CMSTRANSTY_BarTransactionFlg = data.CMSTRANSTY_BarTransactionFlg;
                    obj.CMSTRANSTY_FoodTransactionFlg = data.CMSTRANSTY_FoodTransactionFlg;
                    obj.CMSTRANSTY_CardTransactionFlg = data.CMSTRANSTY_CardTransactionFlg;
                  
                        obj.CMSTRANSTY_UpdatedDate = DateTime.Now;
                        obj.CMSTRANSTY_CreatedBy = data.user_id;
                        obj.CMSTRANSTY_UpdatedBy = data.user_id;
                        obj.CMSTRANSTY_ActiveFlag = true;

                    _CmsContext.Add(obj);
                        var contactExists = _CmsContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            data.returnval = "save";

                        }
                        else
                        {
                            data.returnval = "Notsave";
                        }
                    }

               // }



            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
                // data.returnval = "Admin";
            }
            return data;
        }

        public CMS_TrasanctionTypeDTO deleteDetails(CMS_TrasanctionTypeDTO data)
        {

            try
            {
                if (data.CMSTRANSTY_Id > 0)
                {
                    var update = _CmsContext.CMS_TransactionsTypeDMO.Where(R => R.CMSTRANSTY_Id == data.CMSTRANSTY_Id && R.MI_Id == data.MI_Id).FirstOrDefault();
                    if (update.CMSTRANSTY_ActiveFlag == true)
                    {
                        update.CMSTRANSTY_ActiveFlag = false;
                    }
                    else
                    {
                        update.CMSTRANSTY_ActiveFlag = true;
                    }
                    update.CMSTRANSTY_UpdatedDate = DateTime.Now;
                    update.CMSTRANSTY_UpdatedBy = data.user_id;
                    _CmsContext.Update(update);
                    var contactExists = _CmsContext.SaveChanges();
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


        public CMS_TrasanctionTypeDTO editDetails(CMS_TrasanctionTypeDTO dto)
        {

            try
            {
                var edit = _CmsContext.CMS_TransactionsTypeDMO.Where(R => R.CMSTRANSTY_Id == dto.CMSTRANSTY_Id && R.CMSTRANSTY_ActiveFlag == true).ToList();
                if (edit.Count > 0)
                {

                    dto.cmsdetails = edit.Distinct().ToArray();


                }
                else
                {
                    dto.returnval = "admin";
                }


            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
            }
            return dto;
        }

        //inatllment

        public CMS_TransactionsTypeInstallmentsDTO GetInitialData(int id)
        {
            CMS_TransactionsTypeInstallmentsDTO data = new CMS_TransactionsTypeInstallmentsDTO();
            try
            {


                data.fill_Transaction = _CmsContext.CMS_TransactionsTypeDMO.Where(t =>  t.CMSTRANSTY_ActiveFlag == true && t.MI_Id==id).Distinct().ToArray();

                //data.fill_Installment = _CmsContext.CMS_Master_InstallmentsDMO.Distinct().ToArray();
                data.fill_Installment = (from a in _CmsContext.CMS_Master_InstallmentsDMO
                                         from b in _CmsContext.CMS_TransactionsType_InstallmentsDMO
                                         from c in _CmsContext.CMS_TransactionsTypeDMO
                                         where (a.CMSMINST_Id == b.CMSMINST_Id && c.CMSTRANSTY_Id == b.CMSTRANSTY_Id && c.MI_Id == id)
                                         select new CMS_TransactionsTypeInstallmentsDTO
                                         {
                                             CMSMINST_InstallmentName=a.CMSMINST_InstallmentName,
                                             CMSMINST_Id=a.CMSMINST_Id
                                         }
                                         ).Distinct().ToArray();



            data.fill_details = (from a in _CmsContext.CMS_TransactionsType_InstallmentsDMO
                                 from b in _CmsContext.CMS_Master_InstallmentsDMO
                                 from c in _CmsContext.CMS_TransactionsTypeDMO
                                 where (a.CMSMINST_Id == b.CMSMINST_Id &&  c.CMSTRANSTY_Id == a.CMSTRANSTY_Id)
                                 select new CMS_TransactionsTypeInstallmentsDTO
                                 {

                                     CMSMINST_InstallmentName = b.CMSMINST_InstallmentName,

                                     CMSTRANSTY_TransactionsName = c.CMSTRANSTY_TransactionsName,
                                     CMSTRANSTYINT_Amount = a.CMSTRANSTYINT_Amount,


                                     CMSTRANSTYINT_Id = a.CMSTRANSTYINT_Id,
                                     CMSTRANSTYINT_ActiveFlag = a.CMSTRANSTYINT_ActiveFlag,
                                     CMSTRANSTYINT_CreatedDate = a.CMSTRANSTYINT_CreatedDate

                                 }
                            ).Distinct().OrderByDescending(b => b.CMSTRANSTYINT_CreatedDate).ToArray();



        }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
            }
            return data;
        }

        public CMS_TransactionsTypeInstallmentsDTO editInsDetails(CMS_TransactionsTypeInstallmentsDTO dto)
        {
          
            try
            {
                var edit = _CmsContext.CMS_TransactionsType_InstallmentsDMO.Where(R => R.CMSTRANSTYINT_Id == dto.CMSTRANSTYINT_Id).ToList();
                if (edit.Count > 0)
                {
                    dto.fill_details = edit.Distinct().ToArray();
                }
                else
                {
                    dto.returnval = "admin";
                }


            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
            }
            return dto;
        }

        public CMS_TransactionsTypeInstallmentsDTO saveInsDetails(CMS_TransactionsTypeInstallmentsDTO data)
        {

            try
            {


                if (data.CMSTRANSTYINT_Id > 0)

                {
                    var duplicate =_CmsContext.CMS_TransactionsType_InstallmentsDMO.Where(r => r.CMSTRANSTYINT_Id == data.CMSTRANSTYINT_Id && r.CMSTRANSTY_Id == data.CMSTRANSTY_Id && r.CMSMINST_Id != data.CMSMINST_Id).ToList();
                    if (duplicate.Count > 0)
                    {
                        data.returnval = "Duplicate";
                    }
                    else
                    {
                        var result = _CmsContext.CMS_TransactionsType_InstallmentsDMO.Where(d => d.CMSTRANSTYINT_Id == data.CMSTRANSTYINT_Id).FirstOrDefault();



                        if (result.CMSTRANSTYINT_Id > 0)
                        {
                            // var result = _FeeGroupContext.FACompanyMasterDMO.Single(d => d.FAMCOMP_Id == data.FAMCOMP_Id);

                            //  result.MI_Id = data.MI_Id;
                            result.CMSTRANSTYINT_Id = data.CMSTRANSTYINT_Id;
                            result.CMSTRANSTY_Id = data.CMSTRANSTY_Id;
                            result.CMSMINST_Id = data.CMSMINST_Id;
                            result.CMSTRANSTYINT_Amount = data.CMSTRANSTYINT_Amount;
                          


                            result.CMSTRANSTYINT_UpdatedDate = DateTime.Now;

                            result.CMSTRANSTYINT_UpdatedBy = data.user_id;

                            _CmsContext.Update(result);
                            var flag = _CmsContext.SaveChanges();
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
                    //var duplicate = _CmsContext.CMS_TransactionsType_InstallmentsDMO.Where(r => r.CMSTRANSTY_Id == data.CMSTRANSTY_Id &&  r.CMSTRANSTYINT_Amount == data.FACFYM_RefNo).ToList();
                    //if (duplicate.Count > 0)
                    //{
                    //    data.returnval = "Duplicate";
                    //}
                    //else
                    //{
                    CMS_TransactionsType_InstallmentsDMO obj = new CMS_TransactionsType_InstallmentsDMO();
                      
                        obj.CMSTRANSTY_Id = data.CMSTRANSTY_Id;
                        obj.CMSMINST_Id = data.CMSMINST_Id;
                        obj.CMSTRANSTYINT_Amount = data.CMSTRANSTYINT_Amount;
                       
                        obj.CMSTRANSTYINT_CreatedDate = DateTime.Now;
                        obj.CMSTRANSTYINT_UpdatedDate = DateTime.Now;
                        obj.CMSTRANSTYINT_CreatedBy = data.user_id;
                        obj.CMSTRANSTYINT_UpdatedBy = data.user_id;
                        

                        obj.CMSTRANSTYINT_ActiveFlag = true;

                        _CmsContext.Add(obj);
                        var contactExists = _CmsContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            data.returnval = "save";

                        }
                        else
                        {
                            data.returnval = "Notsave";
                        }
                    }

              //  }



            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
                data.returnval = "Admin";
            }
            return data;
        }

        public CMS_TransactionsTypeInstallmentsDTO deleteInsDetails(CMS_TransactionsTypeInstallmentsDTO data)
        {

            try
            {
                if (data.CMSTRANSTYINT_Id > 0)
                {
                    var update = _CmsContext.CMS_TransactionsType_InstallmentsDMO.Where(R => R.CMSTRANSTYINT_Id == data.CMSTRANSTYINT_Id).FirstOrDefault();
                    if (update.CMSTRANSTYINT_ActiveFlag == true)
                    {
                        update.CMSTRANSTYINT_ActiveFlag = false;
                    }
                    else
                    {
                        update.CMSTRANSTYINT_ActiveFlag = true;
                    }
                    update.CMSTRANSTYINT_UpdatedDate = DateTime.Now;
                    update.CMSTRANSTYINT_UpdatedBy = data.user_id;
                    _CmsContext.Update(update);
                    var contactExists = _CmsContext.SaveChanges();
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

        //transaction Tax

        public CMS_TransactionsTypeTaxDTO GetTaxInitialData(int id)
        {
            CMS_TransactionsTypeTaxDTO data = new CMS_TransactionsTypeTaxDTO();
            try
            {


                data.fill_TaxTransaction = _CmsContext.CMS_TransactionsTypeDMO.Where(t => t.CMSTRANSTY_ActiveFlag == true && t.MI_Id == id).Distinct().ToArray();

                data.fill_Taxdetails = (from a in _CmsContext.CMS_TransactionsType_TaxDMO
                                        from b in _CmsContext.CMS_TransactionsTypeDMO
                                   
                                     where (a.CMSTRANSTY_Id == b.CMSTRANSTY_Id && b.MI_Id==id)
                                     select new CMS_TransactionsTypeTaxDTO
                                     {

                                         CMSTRANSTY_TransactionsName = b.CMSTRANSTY_TransactionsName,

                                         CMSTRANSTY_TaxNo = a.CMSTRANSTY_TaxNo,
                                         CMSTRANSTYTAX_TaxPercent = a.CMSTRANSTYTAX_TaxPercent,


                                         CMSTRANSTYTAX_Id = a.CMSTRANSTYTAX_Id,
                                         CMSTRANSTYTAX_ActiveFlag = a.CMSTRANSTYTAX_ActiveFlag,
                                         CMSTRANSTYTAX_CreatedDate = a.CMSTRANSTYTAX_CreatedDate

                                     }
                                ).Distinct().OrderByDescending(b => b.CMSTRANSTYTAX_CreatedDate).ToArray();



            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
            }
            return data;
        }

        public CMS_TransactionsTypeTaxDTO editTaxDetails(CMS_TransactionsTypeTaxDTO dto)
        {
          //  CMS_TransactionsTypeTaxDTO obj = new CMS_TransactionsTypeTaxDTO();
            try
            {
                var edit = _CmsContext.CMS_TransactionsType_TaxDMO.Where(R => R.CMSTRANSTYTAX_Id == dto.CMSTRANSTYTAX_Id).ToList();
                if (edit.Count > 0)
                {
                    dto.fill_Taxdetails = edit.Distinct().ToArray();
                }
                else
                {
                    dto.returnval = "admin";
                }


            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
            }
            return dto;
        }

        public CMS_TransactionsTypeTaxDTO saveTaxDetails(CMS_TransactionsTypeTaxDTO data)
        {

            try
            {


                if (data.CMSTRANSTYTAX_Id > 0)

                {
                    //var duplicate = _CmsContext.CMS_TransactionsType_TaxDMO.Where(r => r.CMSTRANSTYTAX_Id == data.CMSTRANSTYTAX_Id && r.CMSTRANSTY_Id == data.CMSTRANSTY_Id && r.CMSMINST_Id != data.CMSMINST_Id).ToList();
                    //if (duplicate.Count > 0)
                    //{
                    //    data.returnval = "Duplicate";
                    //}
                    //else
                    //{
                        var result = _CmsContext.CMS_TransactionsType_TaxDMO.Where(d => d.CMSTRANSTYTAX_Id == data.CMSTRANSTYTAX_Id).FirstOrDefault();



                        if (result.CMSTRANSTYTAX_Id > 0)
                        {
                            // var result = _FeeGroupContext.FACompanyMasterDMO.Single(d => d.FAMCOMP_Id == data.FAMCOMP_Id);

                            //  result.MI_Id = data.MI_Id;
                            result.CMSTRANSTYTAX_Id = data.CMSTRANSTYTAX_Id;
                            result.CMSTRANSTY_Id = data.CMSTRANSTY_Id;
                            result.CMSTRANSTY_TaxNo = data.CMSTRANSTY_TaxNo;
                            result.CMSTRANSTYTAX_TaxPercent = data.CMSTRANSTYTAX_TaxPercent;



                            result.CMSTRANSTYTAX_UpdatedDate = DateTime.Now;

                            result.CMSTRANSTYTAX_UpdatedBy = data.user_id;

                            _CmsContext.Update(result);
                            var flag = _CmsContext.SaveChanges();
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
                  //  }


                }
                else
                {
                    //var duplicate = _CmsContext.CMS_TransactionsType_InstallmentsDMO.Where(r => r.CMSTRANSTY_Id == data.CMSTRANSTY_Id &&  r.CMSTRANSTYINT_Amount == data.FACFYM_RefNo).ToList();
                    //if (duplicate.Count > 0)
                    //{
                    //    data.returnval = "Duplicate";
                    //}
                    //else
                    //{
                    CMS_TransactionsType_TaxDMO obj = new CMS_TransactionsType_TaxDMO();

                    obj.CMSTRANSTY_Id = data.CMSTRANSTY_Id;
                    obj.CMSTRANSTY_TaxNo = data.CMSTRANSTY_TaxNo;
                    obj.CMSTRANSTYTAX_TaxPercent = data.CMSTRANSTYTAX_TaxPercent;

                    obj.CMSTRANSTYTAX_CreatedDate = DateTime.Now;
                    obj.CMSTRANSTYTAX_UpdatedDate = DateTime.Now;
                    obj.CMSTRANSTYTAX_CreatedBy = data.user_id;
                    obj.CMSTRANSTYTAX_UpdatedBy = data.user_id;


                    obj.CMSTRANSTYTAX_ActiveFlag = true;

                    _CmsContext.Add(obj);
                    var contactExists = _CmsContext.SaveChanges();
                    if (contactExists == 1)
                    {
                        data.returnval = "save";

                    }
                    else
                    {
                        data.returnval = "Notsave";
                    }
                }

                //  }



            }
            catch (Exception e)
            {
                _logger.LogDebug(e.Message);
                data.returnval = "Admin";
            }
            return data;
        }

        public CMS_TransactionsTypeTaxDTO deleteTaxDetails(CMS_TransactionsTypeTaxDTO data)
        {

            try
            {
                if (data.CMSTRANSTYTAX_Id > 0)
                {
                    var update = _CmsContext.CMS_TransactionsType_TaxDMO.Where(R => R.CMSTRANSTYTAX_Id == data.CMSTRANSTYTAX_Id).FirstOrDefault();
                    if (update.CMSTRANSTYTAX_ActiveFlag == true)
                    {
                        update.CMSTRANSTYTAX_ActiveFlag = false;
                    }
                    else
                    {
                        update.CMSTRANSTYTAX_ActiveFlag = true;
                    }
                    update.CMSTRANSTYTAX_UpdatedDate = DateTime.Now;
                    update.CMSTRANSTYTAX_UpdatedBy = data.user_id;
                    _CmsContext.Update(update);
                    var contactExists = _CmsContext.SaveChanges();
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
