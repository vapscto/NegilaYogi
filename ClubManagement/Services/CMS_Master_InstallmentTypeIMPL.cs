using DataAccessMsSqlServerProvider.com.vapstech.ClubManagement;
using DomainModel.Model.com.vapstech.ClubManagement;
using PreadmissionDTOs.com.vaps.ClubManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClubManagement.Services
{
    public class CMS_Master_InstallmentTypeIMPL : Interfaces.CMS_Master_InstallmentTypeINTERFACE
    {
        public ClubManagementContext _CmsContext;
        public CMS_Master_InstallmentTypeIMPL(ClubManagementContext cmsContext)
        {
            _CmsContext = cmsContext;
        }
        public CMS_Master_InstallmentTypeDTO loaddata(int id)
        {

            CMS_Master_InstallmentTypeDTO dto = new CMS_Master_InstallmentTypeDTO();
             dto.pages = _CmsContext.CMS_Master_InstallmentTypeDMO.Where(R => R.MI_Id == id).Distinct().ToArray();
            // MonthDMO
            dto.MonthArray = _CmsContext.MonthDMO.Distinct().ToArray();
            return dto;

        }
        public CMS_Master_InstallmentTypeDTO savedata(CMS_Master_InstallmentTypeDTO data)
        {
            try
            {
                if(data.CMSMINSTTY_Id > 0)
                {
                    var result = _CmsContext.CMS_Master_InstallmentTypeDMO.Where(R => R.MI_Id == data.MI_Id && R.CMSMINSTTY_InstallmentType == data.CMSMINSTTY_InstallmentType && R.CMSMINSTTY_InstallmentTypeFlg == data.CMSMINSTTY_InstallmentTypeFlg && R.CMSMINSTTY_Duration == data.CMSMINSTTY_Duration && R.CMSMINSTTY_DurationFlg == data.CMSMINSTTY_DurationFlg && R.CMSMINSTTY_Id !=data.CMSMINSTTY_Id).ToList();
                    if (result.Count > 0)
                    {
                        data.returnval = "exist";
                    }
                    else
                    {
                        var resultwo = _CmsContext.CMS_Master_InstallmentTypeDMO.Where(R => R.CMSMINSTTY_Id == data.CMSMINSTTY_Id).FirstOrDefault();
                        if(resultwo.CMSMINSTTY_Id > 0)
                        {
                            resultwo.CMSMINSTTY_InstallmentType = data.CMSMINSTTY_InstallmentType;
                            resultwo.CMSMINSTTY_InstallmentTypeFlg = data.CMSMINSTTY_InstallmentTypeFlg;
                            resultwo.CMSMINSTTY_Duration = data.CMSMINSTTY_Duration;
                            resultwo.CMSMINSTTY_DurationFlg = data.CMSMINSTTY_DurationFlg;
                            _CmsContext.Update(resultwo);
                            var contactExists = _CmsContext.SaveChanges();
                            if (contactExists == 1)

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
                    var result = _CmsContext.CMS_Master_InstallmentTypeDMO.Where(R => R.MI_Id == data.MI_Id && R.CMSMINSTTY_InstallmentType == data.CMSMINSTTY_InstallmentType && R.CMSMINSTTY_InstallmentTypeFlg == data.CMSMINSTTY_InstallmentTypeFlg && R.CMSMINSTTY_Duration == data.CMSMINSTTY_Duration && R.CMSMINSTTY_DurationFlg == data.CMSMINSTTY_DurationFlg).ToList();
                    if (result.Count > 0)
                    {
                        data.returnval = "exist";
                    }
                    else
                    {
                        CMS_Master_InstallmentTypeDMO obj = new CMS_Master_InstallmentTypeDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.CMSMINSTTY_InstallmentType = data.CMSMINSTTY_InstallmentType;
                        obj.CMSMINSTTY_InstallmentTypeFlg = data.CMSMINSTTY_InstallmentTypeFlg;
                        obj.CMSMINSTTY_Duration = data.CMSMINSTTY_Duration;
                        obj.CMSMINSTTY_DurationFlg = data.CMSMINSTTY_DurationFlg;
                        obj.CMSMINSTTY_ActiveFlag = true;
                        obj.CMSMINSTTY_CreatedDate = DateTime.Now;
                        obj.CMSMINSTTY_UpdatedDate = DateTime.Now;
                        obj.CMSMINSTTY_CreatedBy = data.UserId;
                        obj.CMSMINSTTY_UpdatedBy = data.UserId;
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
                }
               



            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }
        //deactive
        public CMS_Master_InstallmentTypeDTO deactive(CMS_Master_InstallmentTypeDTO data)
        {
            try
            {
                var result = _CmsContext.CMS_Master_InstallmentTypeDMO.Where(R => R.MI_Id == data.MI_Id && R.CMSMINSTTY_Id == data.CMSMINSTTY_Id).FirstOrDefault();
                if (data.CMSMINSTTY_Id > 0)
                {
                  
                    if(result.CMSMINSTTY_ActiveFlag==true)
                    {
                        result.CMSMINSTTY_ActiveFlag = false;
                    }
                    else
                    {
                        result.CMSMINSTTY_ActiveFlag = true;
                    }
                    result.CMSMINSTTY_UpdatedBy = data.UserId;
                    result.CMSMINSTTY_UpdatedDate = DateTime.Now;
                    _CmsContext.Update(result);
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
                else
                {
                    data.returnval = "admin";
                }
                    


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }
    }
}
