using DataAccessMsSqlServerProvider.com.vapstech.ClubManagement;
using DomainModel.Model.com.vapstech.ClubManagement;
using PreadmissionDTOs.com.vaps.ClubManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClubManagement.Services
{
    public class CMS_Master_InstallmentsIMPL : Interfaces.CMS_Master_InstallmentInterface
    {
        public ClubManagementContext _CmsContext;
        public CMS_Master_InstallmentsIMPL(ClubManagementContext cmsContext)
        {
            _CmsContext = cmsContext;
        }
        public CMS_Master_InstallmentsDTO loaddata(int id)
        {

            CMS_Master_InstallmentsDTO dto = new CMS_Master_InstallmentsDTO();
            //  dto.pages = _CmsContext.CMS_Master_InstallmentsDMO.Distinct().ToArray();
            dto.pages = (from a in _CmsContext.CMS_Master_InstallmentsDMO
                         from b in _CmsContext.CMS_Master_InstallmentTypeDMO
                         where (a.CMSMINSTTY_Id == b.CMSMINSTTY_Id && b.MI_Id == id)
                         select a).Distinct().ToArray();

            dto.MonthArray = _CmsContext.MonthDMO.Distinct().ToArray();
            dto.InstallArray = _CmsContext.CMS_Master_InstallmentTypeDMO.Where(P => P.CMSMINSTTY_ActiveFlag == true && P.MI_Id==id).Distinct().ToArray();
            return dto;

        }
        public CMS_Master_InstallmentsDTO savedata(CMS_Master_InstallmentsDTO data)
        {
            try
            {
                if(data.CMSMINST_Id > 0)
                {
                    var list = _CmsContext.CMS_Master_InstallmentsDMO.Where(R => R.CMSMINSTTY_Id == data.CMSMINSTTY_Id && R.CMSMINST_InstallmentName == data.CMSMINST_InstallmentName && R.CMSMINST_FromDate == data.CMSMINST_FromDate && R.CMSMINST_FromMonth == data.CMSMINST_FromMonth && R.CMSMINST_ToDate == data.CMSMINST_ToDate && R.CMSMINST_ToMonth == data.CMSMINST_ToMonth && R.CMSMINST_ApplicableDate == data.CMSMINST_ApplicableDate && R.CMSMINST_ApplMonth == data.CMSMINST_ApplMonth && R.CMSMINST_Id !=data.CMSMINST_Id).ToList();
                    if(list.Count  > 0)
                    {
                        data.returnval = "exit";
                    }
                    else
                    {
                        var resultwo = _CmsContext.CMS_Master_InstallmentsDMO.Where(R => R.CMSMINST_Id == data.CMSMINST_Id).FirstOrDefault();
                        if(resultwo.CMSMINST_Id > 0)
                        {
                            resultwo.CMSMINSTTY_Id = data.CMSMINSTTY_Id;
                            resultwo.CMSMINST_InstallmentName = data.CMSMINST_InstallmentName;
                            resultwo.CMSMINST_FromDate = data.CMSMINST_FromDate;
                            resultwo.CMSMINST_FromMonth = data.CMSMINST_FromMonth;
                            resultwo.CMSMINST_ToDate = data.CMSMINST_ToDate;
                            resultwo.CMSMINST_ToMonth = data.CMSMINST_ToMonth;
                            resultwo.CMSMINST_ApplicableDate = data.CMSMINST_ApplicableDate;
                            resultwo.CMSMINST_ApplMonth = data.CMSMINST_ApplMonth;
                            resultwo.CMSMINST_UpdatedDate = DateTime.Now;
                            resultwo.CMSMINST_UpdatedBy = data.UserId;
                            _CmsContext.Update(resultwo);
                            var contactExists = _CmsContext.SaveChanges();
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
                    var list = _CmsContext.CMS_Master_InstallmentsDMO.Where(R => R.CMSMINSTTY_Id == data.CMSMINSTTY_Id && R.CMSMINST_InstallmentName == data.CMSMINST_InstallmentName && R.CMSMINST_FromDate == data.CMSMINST_FromDate && R.CMSMINST_FromMonth == data.CMSMINST_FromMonth && R.CMSMINST_ToDate == data.CMSMINST_ToDate && R.CMSMINST_ToMonth == data.CMSMINST_ToMonth && R.CMSMINST_ApplicableDate == data.CMSMINST_ApplicableDate && R.CMSMINST_ApplMonth == data.CMSMINST_ApplMonth).ToList();
                    if (list.Count > 0)
                    {
                        data.returnval = "exit";
                    }
                    else
                    {
                        CMS_Master_InstallmentsDMO obj = new CMS_Master_InstallmentsDMO();
                        obj.CMSMINSTTY_Id = data.CMSMINSTTY_Id;
                        obj.CMSMINST_InstallmentName = data.CMSMINST_InstallmentName;
                        obj.CMSMINST_FromDate = data.CMSMINST_FromDate;
                        obj.CMSMINST_FromMonth = data.CMSMINST_FromMonth;
                        obj.CMSMINST_ToDate = data.CMSMINST_ToDate;
                        obj.CMSMINST_ToMonth = data.CMSMINST_ToMonth;
                        obj.CMSMINST_ApplicableDate = data.CMSMINST_ApplicableDate;
                        obj.CMSMINST_ApplMonth = data.CMSMINST_ApplMonth;
                        obj.CMSMINST_ActiveFlag = true;
                        obj.CMSMINST_CreatedDate = DateTime.Now;
                        obj.CMSMINST_CreatedBy = data.UserId;
                        obj.CMSMINST_UpdatedDate = DateTime.Now;
                        obj.CMSMINST_UpdatedBy = data.UserId;
                        _CmsContext.Add(obj);
                        var contactExists = _CmsContext.SaveChanges();
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
        //deactive
        public CMS_Master_InstallmentsDTO deactive(CMS_Master_InstallmentsDTO data)
        {
            try
            {
                if(data.CMSMINST_Id > 0)
                {
                    var resultwo = _CmsContext.CMS_Master_InstallmentsDMO.Where(R => R.CMSMINST_Id == data.CMSMINST_Id).FirstOrDefault();
                    if(resultwo.CMSMINST_ActiveFlag==true)
                    {
                        resultwo.CMSMINST_ActiveFlag = false;
                    }
                    else
                    {
                        resultwo.CMSMINST_ActiveFlag = true;
                    }
                    resultwo.CMSMINST_UpdatedBy = data.UserId;
                    resultwo.CMSMINST_UpdatedDate = DateTime.Now;
                    _CmsContext.Update(resultwo);
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
