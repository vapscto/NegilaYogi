using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.Canteen;
using DomainModel.Model.com.vapstech.Canteen;
using PreadmissionDTOs.com.vaps.Canteen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CanteenHub.com.vaps.Services
{
    public class FoodMasterCategoryImpl : Interfaces.FoodMasterCategoryInterface
    {
        public Canteencontext _CmfContext;
        public FoodMasterCategoryImpl(Canteencontext cmfContext)
        {
            _CmfContext = cmfContext;
        }
        public FoodMasterCategoryDTO loaddata(FoodMasterCategoryDTO data)
        {
            try
            {
                data.Foodcategeory = _CmfContext.FoodMasterCategoryDMO.Where(R => R.MI_Id == data.MI_Id).ToArray();
               // data.Foodcategeory = _CmfContext.FoodMasterCategoryDMO.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



        public FoodMasterCategoryDTO savedata(FoodMasterCategoryDTO data)
        {
            try
            {
                if (data.CMMCA_Id > 0)
                {
                    var result = _CmfContext.FoodMasterCategoryDMO.Where(R => R.CMMCA_Id != data.CMMCA_Id && R.MI_Id == data.MI_Id && R.CMMCA_CategoryName == data.CMMCA_CategoryName).ToList();
                    if (result.Count > 0)
                    {
                        data.returnval = "exit";
                    }
                    else
                    {
                        var categeory = _CmfContext.FoodMasterCategoryDMO.Where(R => R.CMMCA_Id == data.CMMCA_Id).FirstOrDefault();
                        if (categeory.CMMCA_Id > 0)
                        {
                            categeory.CMMCA_CategoryName = data.CMMCA_CategoryName;
                            categeory.CMMCA_Remarks = data.CMMCA_Remarks;
                            categeory.CMMCA_ActiveFlag = true;
                            categeory.CMMCA_CreatedDate = DateTime.Now;
                            categeory.CMMCA_UpdatedDate = DateTime.Now;
                            categeory.CreatedBy = data.UserId;
                            categeory.UpdatedBy = data.UserId;

                            _CmfContext.Update(categeory);

                            var contactExists = _CmfContext.SaveChanges();
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
                    var result = _CmfContext.FoodMasterCategoryDMO.Where(R => R.CMMCA_CategoryName == data.CMMCA_CategoryName && R.MI_Id == data.MI_Id).ToList();
                    if (result.Count > 0)
                    {
                        data.returnval = "exit";
                    }
                    else
                    {
                        FoodMasterCategoryDMO obj = new FoodMasterCategoryDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.CMMCA_CategoryName = data.CMMCA_CategoryName;
                        obj.CMMCA_Remarks = data.CMMCA_Remarks;
                        obj.CMMCA_ActiveFlag = true;
                        obj.CMMCA_CreatedDate = DateTime.Now;
                        obj.CMMCA_UpdatedDate = DateTime.Now;
                        obj.CreatedBy = data.UserId;
                        obj.UpdatedBy = data.UserId;

                        _CmfContext.Add(obj);


                        var contactExists = _CmfContext.SaveChanges();
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

        public FoodMasterCategoryDTO GetEditdata(FoodMasterCategoryDTO data)
        {

            try
            {

                data.GridviewDetails = _CmfContext.FoodMasterCategoryDMO.Where(t => t.CMMCA_Id == data.CMMCA_Id).ToArray();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }
        public FoodMasterCategoryDTO deactivate(FoodMasterCategoryDTO acd)
        {
            try
            {
                // FoodMasterCategoryDMO enq = Mapper.Map<FoodMasterCategoryDMO>(acd);

                FoodMasterCategoryDMO enq = new FoodMasterCategoryDMO();

                //Organisation enq = new Organisation();
                // enq.MO_Id = 12;
           
                                 var ismapped = (from a in _CmfContext.FoodMasterCategoryDMO
                                                 from b in _CmfContext.FooditeamDMO
                                                 where (a.CMMCA_Id == b.CMMCA_Id && a.MI_Id == acd.MI_Id && b.CMMFI_ActiveFlg==true && b.CMMCA_Id == acd.CMMCA_Id  && b.CMMFI_ActiveFlg == true) 

                                                 select a).ToArray();

                if (ismapped.Length > 0)
                {
                    acd.returnval = "You Can Not Activate/Deactivate This Record.Because It Is Mapped To Food Item.";
                }
                else
                {
                    if (acd.CMMCA_Id > 0)
                    {
                        var result = _CmfContext.FoodMasterCategoryDMO.Single(t => t.CMMCA_Id == acd.CMMCA_Id );

                        if (acd.CMMCA_ActiveFlag == true)
                        {
                            result.CMMCA_ActiveFlag = false;
                        }
                        else if (acd.CMMCA_ActiveFlag == false)
                        {
                            result.CMMCA_ActiveFlag = true;
                        }
                        result.CMMCA_UpdatedDate = DateTime.Now;

                        _CmfContext.Update(result);
                        var flag = _CmfContext.SaveChanges();
                        if (flag > 0)
                        {
                            if (result.CMMCA_ActiveFlag == true)
                            {

                                acd.returnval = "Category Activated Successfully.";
                            }
                            else
                            {
                                acd.returnval = "Category Deactivated Successfully.";
                            }
                        }
                        else
                        {
                            acd.returnval = "Category Not Activated/Deactivated";
                        }

                        acd.Foodcategeory = _CmfContext.FoodMasterCategoryDMO.ToArray();
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
