using System;
using DataAccessMsSqlServerProvider.com.vapstech.Library;
using LibraryServiceHub.com.vaps.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel.Model.com.vapstech.Library;
using PreadmissionDTOs.com.vaps.Library;

namespace LibraryServiceHub.com.vaps.Services
{
    public class MasterCategoryImpl:Interfaces.MasterCategoryInterface
    {
        public LibraryContext _LibContext;
        public MasterCategoryImpl(LibraryContext para)
        {
            _LibContext = para;
        }


        public MasterCategory_DTO getdetails(int id)
        {

            MasterCategory_DTO data = new MasterCategory_DTO();
            try
            {
                data.categorylist = _LibContext.MasterCategoryDMO.Where(t => t.MI_Id == id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return data;
        }

        public MasterCategory_DTO Savedata(MasterCategory_DTO data)
        {
            try
            {
                if(data.LMC_Id > 0)
                {
                    var Duplicate = _LibContext.MasterCategoryDMO.Where(t => t.LMC_Id != data.LMC_Id && t.LMC_CategoryName == data.LMC_CategoryName 
                    && t.MI_Id == data.MI_Id).ToList();
                    if(Duplicate.Count()>0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var Update = _LibContext.MasterCategoryDMO.Single(t => t.LMC_Id == data.LMC_Id && t.MI_Id == data.MI_Id);

                        Update.LMC_CategoryName = data.LMC_CategoryName;
                        Update.LMC_CategoryCode = data.LMC_CategoryCode;
                        Update.LMC_BNBFlg = data.LMC_BNBFlg;
                        Update.UpdatedDate = DateTime.Now;
                        _LibContext.Update(Update);
                        int rowAffected = _LibContext.SaveChanges();
                        if(rowAffected > 0)
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
                    var Duplicate = _LibContext.MasterCategoryDMO.Where(t=>t.MI_Id==data.MI_Id && t.LMC_CategoryName==data.LMC_CategoryName).ToList();
                    if (Duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        MasterCategoryDMO obj = new MasterCategoryDMO();
                        obj.MI_Id = data.MI_Id;
                        obj.LMC_CategoryName = data.LMC_CategoryName;
                        obj.LMC_CategoryCode = data.LMC_CategoryCode;
                        obj.LMC_BNBFlg = data.LMC_BNBFlg;
                        obj.LMC_ActiveFlag = true;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        _LibContext.Add(obj);
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
                }
                            
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
       
        public MasterCategory_DTO deactiveY(MasterCategory_DTO data)
        {
            try
            {
                var result = _LibContext.MasterCategoryDMO.Single(t => t.LMC_Id == data.LMC_Id && t.MI_Id==data.MI_Id);
                if(result.LMC_ActiveFlag == true)
                {
                    result.LMC_ActiveFlag = false;
                }
                else if (result.LMC_ActiveFlag == false)
                {
                    result.LMC_ActiveFlag = true;
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
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
