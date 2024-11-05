using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTableServiceHub.com.vaps.Services
{
    public class TTCategoryImpl : Interfaces.TTCategoryInterface
    {
        private static ConcurrentDictionary<string, TTMasterCategoryDTO> _login =
          new ConcurrentDictionary<string, TTMasterCategoryDTO>();


        public TTContext _ttcategorycontext;
        public TTCategoryImpl(TTContext ttcategory)
        {
            _ttcategorycontext = ttcategory;
        }

        public TTMasterCategoryDTO savedetail(TTMasterCategoryDTO _category)
        {
            TTMasterCategoryDMO objpge = Mapper.Map<TTMasterCategoryDMO>(_category);
            try
            {
                if (objpge.TTMC_Id > 0)
                {
                        var result = _ttcategorycontext.TTMasterCategoryDMO.Single(t => t.TTMC_Id.Equals(objpge.TTMC_Id) && t.MI_Id.Equals(objpge.MI_Id));
                        result.TTMC_CategoryName = objpge.TTMC_CategoryName;                     
                        result.UpdatedDate = DateTime.Now;
                        _ttcategorycontext.Update(result);
                        var contactExists = _ttcategorycontext.SaveChanges();
                        if (contactExists.Equals(1))
                        {
                            _category.returnval = true;
                        }
                        else
                        {
                            _category.returnval = false;
                        }
                }
                else
                {
                    var result = _ttcategorycontext.TTMasterCategoryDMO.Where(t => t.TTMC_CategoryName.Equals(objpge.TTMC_CategoryName) && t.MI_Id.Equals(objpge.MI_Id));
                    if (result.Count() > 0)
                    {
                        _category.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        objpge.CreatedDate = DateTime.Now;
                        objpge.UpdatedDate = DateTime.Now;
                        _ttcategorycontext.Add(objpge);
                        var contactExists = _ttcategorycontext.SaveChanges();
                        if (contactExists.Equals(1))
                        {
                            _category.returnval = true;
                        }
                        else
                        {
                            _category.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                _category.returnval = false;
            }
            return _category;
        }

        public TTMasterCategoryDTO getdetails(int id)
        {
            TTMasterCategoryDTO TTMC = new TTMasterCategoryDTO();
            try
            {
              
                TTMC.Categorylist = (from TT_Master_Category in _ttcategorycontext.TTMasterCategoryDMO
                                     where (TT_Master_Category.MI_Id.Equals(id))
                                     select new TTMasterCategoryDTO
                                     {
                                         TTMC_Id= TT_Master_Category.TTMC_Id,                                      
                                         TTMC_CategoryName = TT_Master_Category.TTMC_CategoryName,
                                        TTMC_ActiveFlag= TT_Master_Category.TTMC_ActiveFlag
                                     }
                                      ).ToArray();
            }
            catch (Exception ee)
            {
               Console.WriteLine(ee.Message);
            }
            return TTMC;

        }

        public TTMasterCategoryDTO getpageedit(int id)
        {
            TTMasterCategoryDTO page = new TTMasterCategoryDTO();
            try
            {
                List<TTMasterCategoryDMO> lorg = new List<TTMasterCategoryDMO>();
                lorg = _ttcategorycontext.TTMasterCategoryDMO.AsNoTracking().Where(t => t.TTMC_Id.Equals(id)).ToList();
                page.Categorylistedit = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public TTMasterCategoryDTO deleterec(int id)
        {          
            TTMasterCategoryDTO page = new TTMasterCategoryDTO();
            try
            {
                List<TTMasterCategoryDMO> lorg = new List<TTMasterCategoryDMO>();
                lorg = _ttcategorycontext.TTMasterCategoryDMO.Where(t => t.TTMC_Id.Equals(id)).ToList();
                if (lorg.Any())
                {
                    _ttcategorycontext.Remove(lorg.ElementAt(0));
                    var contactExists = _ttcategorycontext.SaveChanges();
                    if (contactExists == 1)
                    {
                        page.returnval = true;
                    }
                    else
                    {
                        page.returnval = false;
                    }
                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return page;
        }

        public TTMasterCategoryDTO deactivate(TTMasterCategoryDTO acd)
        {
            try
            {
                TTMasterCategoryDMO pge = Mapper.Map<TTMasterCategoryDMO>(acd);
                if (pge.TTMC_Id > 0)
                {
                    var result = _ttcategorycontext.TTMasterCategoryDMO.Single(t => t.TTMC_Id.Equals(pge.TTMC_Id));
                    if (result.TTMC_ActiveFlag.Equals(true))
                    {
                        result.TTMC_ActiveFlag = false;
                    }
                    else
                    {
                        result.TTMC_ActiveFlag = true;
                    }
                    _ttcategorycontext.Update(result);
                    var flag = _ttcategorycontext.SaveChanges();
                    if (flag.Equals(1))
                    {
                        acd.returnval = true;
                    }
                    else
                    {
                        acd.returnval = false;
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
