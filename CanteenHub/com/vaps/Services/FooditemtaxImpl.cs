using DataAccessMsSqlServerProvider.com.vapstech.Canteen;
using DomainModel.Model.com.vapstech.Canteen;
using PreadmissionDTOs.com.vaps.Canteen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CanteenHub.com.vaps.Services
{
    
    public class FooditemtaxImpl : Interfaces.FooditemtaxInterface

    {
        public Canteencontext _fmtContext;
        public FooditemtaxImpl(Canteencontext fmtContext)
        {
            _fmtContext = fmtContext;
        }
        public FooditemtaxDTO loaddata(FooditemtaxDTO data)
        {
            try
            {
                // data.foodtax = _fmtContext.FooditemtaxDMO.ToArray();
                data.Fooditeam = _fmtContext.FooditeamDMO.Where(T => T.CMMFI_ActiveFlg==true).ToArray();
                data.invmaster = _fmtContext.INV_Master_TaxDMO.Where(T => T.INVMT_ActiveFlg == true).ToArray();

                data.foodtax = (from a in _fmtContext.FooditemtaxDMO
                                  from b in _fmtContext.FooditeamDMO
                                  from c in _fmtContext.INV_Master_TaxDMO
                                  where (a.CMMFI_Id == b.CMMFI_Id && a.INVMT_Id == c.INVMT_Id )
                                  select new FooditemtaxDTO
                                  {
                                      CMMFI_FoodItemName = b.CMMFI_FoodItemName,
                                      taxpercent = a.CMMFIT_TaxPercent,
                                      CMMFIT_ActiveFlg=a.CMMFIT_ActiveFlg,
                                      CMMFIT_Id=a.CMMFIT_Id,
                                      INVMT_TaxName=c.INVMT_TaxName

                                  }).ToArray();



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public FooditemtaxDTO savedata (FooditemtaxDTO data)
        {
            try
            {
                if (data.CMMFIT_Id > 0)
                {
                    var result = _fmtContext.FooditemtaxDMO.Where(T => T.CMMFIT_Id != data.CMMFIT_Id ).ToList();
                    if (result.Count > 0)
                    {
                        data.returnval = "exit";
                    }
                    else
                    {
                        var resultax = _fmtContext.FooditemtaxDMO.Where(T => T.CMMFIT_Id == data.CMMFIT_Id && T.CMMFI_Id == data.CMMFI_Id).FirstOrDefault();
                        if (resultax.CMMFIT_Id > 0)
                        {
                            resultax.CMMFIT_Id = data.CMMFIT_Id;
                            resultax.CMMFI_Id = data.CMMFI_Id;
                            resultax.INVMT_Id = data.INVMT_Id;
                            resultax.CMMFIT_TaxPercent = data.CMMFIT_TaxPercent;
                            resultax.CMMFIT_ActiveFlg = true;
                            resultax.CMMFIT_CreatedBy = data.UserId;
                            resultax.CMMFIT_UpdatedBy = data.UserId;
                            resultax.CMMFIT_CreatedDate = DateTime.Now;
                            resultax.CMMFIT_Updateddate = DateTime.Now;


                            _fmtContext.Update(resultax);
                            var contactExists = _fmtContext.SaveChanges();
                            if (contactExists > 0)

                            {
                                data.returnval = "update";

                            }
                            else
                            {
                                data.returnval = "Notupdate";
                            }

                        }
                    }
                }
                else
                {
                    var result = _fmtContext.FooditemtaxDMO.Where(x => x.CMMFI_Id == data.CMMFI_Id && x.INVMT_Id ==data.INVMT_Id && x.CMMFIT_TaxPercent ==data.CMMFIT_TaxPercent).ToList();
                    if (result.Count > 0)
                    {
                        data.returnval = "exit";
                    }
                    else
                    {
                        FooditemtaxDMO obj = new FooditemtaxDMO();
                        obj.CMMFI_Id = data.CMMFI_Id;
                        obj. INVMT_Id = data.INVMT_Id;
                        obj.CMMFIT_TaxPercent = data.CMMFIT_TaxPercent;
                        obj.CMMFIT_ActiveFlg = true;
                        obj.CMMFIT_CreatedBy = data.UserId;
                        obj.CMMFIT_UpdatedBy = data.UserId;
                        obj.CMMFIT_CreatedDate = DateTime.Now;
                        obj.CMMFIT_Updateddate = DateTime.Now;
                        _fmtContext.Add(obj);
                        var contactExists = _fmtContext.SaveChanges();
                        if (contactExists > 0)
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }
        public FooditemtaxDTO deactivate(FooditemtaxDTO acd)
        {
            try
            {



                //   var ismapped = _fmtContext.FooditeamDMO.Single(t => t.CMMFI_Id == acd.CMMFI_Id);

                if (acd.CMMFIT_Id > 0)
                {
                    var result = _fmtContext.FooditemtaxDMO.Single(t => t.CMMFIT_Id == acd.CMMFIT_Id);

                    if (acd.CMMFIT_ActiveFlg == true)
                    {
                        result.CMMFIT_ActiveFlg = false;
                    }
                    else if (acd.CMMFIT_ActiveFlg == false)
                    {
                        result.CMMFIT_ActiveFlg = true;
                    }

                    result.CMMFIT_Updateddate = DateTime.Now;

                    _fmtContext.Update(result);
                    var flag = _fmtContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.CMMFIT_ActiveFlg == true)
                        {

                            acd.returnval = "Foodtax Activated Successfully.";
                        }
                        else
                        {
                            acd.returnval = "Foodtax Deactivated Successfully.";
                        }
                    }
                    else
                    {
                        acd.returnval = "Foodtax Not Activated/Deactivated";
                    }


                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
            }
            return acd;
        }

    }
}
