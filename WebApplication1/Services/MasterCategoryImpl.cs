using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public class MasterCategoryImpl : Interfaces.MasterCategoryInterface
    {
        private static ConcurrentDictionary<string, MasterCategoryDTO> _login =
            new ConcurrentDictionary<string, MasterCategoryDTO>();

        public MasterCategoryContext _masterCategoryContext;
        public DomainModelMsSqlServerContext _db;
        public MasterCategoryImpl(MasterCategoryContext masterCategoryContext, DomainModelMsSqlServerContext db)
        {
            _masterCategoryContext = masterCategoryContext;
            _db = db;
        }
        public MasterCategoryDTO getAllDetails(int id)
        {
            MasterCategoryDTO mstcat = new MasterCategoryDTO();
            try
            {
               
                var mo_id = _masterCategoryContext.institution.Where(d => d.MI_Id == id).ToList();
                List<Institution> allInstitution = new List<Institution>();
                allInstitution = _masterCategoryContext.institution.Where(d=>d.MI_Id==id && d.MI_ActiveFlag==1).ToList();
                mstcat.institutionDrpdwn = allInstitution.ToArray();

                mstcat.categoryList = (from a in _masterCategoryContext.category
                                       from b in _masterCategoryContext.institution
                                       where (a.MI_Id == b.MI_Id && a.MI_Id==id)
                                       select new MasterCategoryDTO { AMC_Id = a.AMC_Id, AMC_Name = a.AMC_Name, AMC_Type = a.AMC_Type, AMC_ActiveFlag = a.AMC_ActiveFlag, MI_Name = b.MI_Name }
                                     ).OrderByDescending(a=>a.CreatedDate).ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return mstcat;
        }
        public MasterCategoryDTO getdetails(MasterCategoryDTO id)
        {
            MasterCategoryDTO cate = new MasterCategoryDTO();
            try
            {
                List<MasterCategory> lorg = new List<MasterCategory>();
                lorg = _masterCategoryContext.category.AsNoTracking().Where(t => t.AMC_Id==id.AMC_Id).ToList();
                cate.categoryList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return cate;
        }
        public MasterCategoryDTO saveCategorydet(MasterCategoryDTO catgry)
        {
            try
            {
                MasterCategory ctgry = Mapper.Map<MasterCategory>(catgry);

                if (ctgry.AMC_Id > 0)
                {
                    var lineAdd_chk = catgry.Line1 + ',' + catgry.Line2 + ',' + catgry.Line3 + ',' + catgry.Line4;
                    ctgry.AMC_Address = lineAdd_chk;
   var duplicateresult = _masterCategoryContext.category.Where(t => t.MI_Id == catgry.MI_Id && t.AMC_Name == ctgry.AMC_Name && t.AMC_Id!=catgry.AMC_Id).Count();

                    if (duplicateresult > 0)
                    {
                        catgry.returnval = "Duplicate";
                        return catgry;
                    }
                    else
                    {
                        var result = _masterCategoryContext.category.Single(t => t.AMC_Id == ctgry.AMC_Id);
                        result.AMC_ActiveFlag = 1;
                        var lineAdd = catgry.Line1 + ',' + catgry.Line2 + ',' + catgry.Line3 + ',' + catgry.Line4;
                        result.AMC_Address = lineAdd;
                        result.AMC_Name = ctgry.AMC_Name;
                        result.AMC_RegNoPrefix = ctgry.AMC_RegNoPrefix;
                        result.AMC_Type = ctgry.AMC_Type;
                        result.AMC_PAApplicationName = "Admissionform";
                        result.MI_Id = ctgry.MI_Id;
                        result.AMC_Logo = ctgry.AMC_Logo;
                        result.AMC_FilePath = ctgry.AMC_FilePath;
                        result.UpdatedDate = DateTime.Now;
                        _masterCategoryContext.Update(result);
                        var flag = _masterCategoryContext.SaveChanges();
                        if (flag > 0)
                        {
                            catgry.returnval = "Update";
                        }
                        else
                        {
                            catgry.returnval = "false";
                        }
                    }
                }





                else
                {
                    var duplicatecountresult = _masterCategoryContext.category.Where(t => t.MI_Id == catgry.MI_Id && t.AMC_Name == ctgry.AMC_Name).Count();
                    if (duplicatecountresult == 0)
                    {

                        var lineAdd = catgry.Line1 + ',' + catgry.Line2 + ',' + catgry.Line3 + ',' + catgry.Line4;
                        ctgry.AMC_Address = lineAdd;
                       
                        ctgry.AMC_ActiveFlag = 1;
                        ctgry.CreatedDate = DateTime.Now;
                        ctgry.UpdatedDate = DateTime.Now;
                        _masterCategoryContext.Add(ctgry);
                        var flag = _masterCategoryContext.SaveChanges();
                        if (flag == 1)
                        {
                            catgry.returnval = "Add";
                        }
                        else
                        {
                            catgry.returnval = "false";
                        }
                    }
                    else
                    {
                        catgry.returnval = "Duplicate";
                        return catgry;
                    }

                }

                catgry.categoryList = (from a in _masterCategoryContext.category
                                       from b in _masterCategoryContext.institution
                                       where (a.MI_Id == b.MI_Id && a.MI_Id==catgry.MI_Id)
                                       select new MasterCategoryDTO { AMC_Id = a.AMC_Id, AMC_Name = a.AMC_Name, AMC_Type = a.AMC_Type, AMC_ActiveFlag = a.AMC_ActiveFlag, MI_Name = b.MI_Name }
                                    ).OrderByDescending(a=>a.CreatedDate).ToArray();
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                catgry.returnval = "Error occured";
            }

            return catgry;
        }       

        public MasterCategoryDTO deleterec(int id)
        {
            MasterCategoryDTO org = new MasterCategoryDTO();
            List<MasterCategory> lorg = new List<MasterCategory>();
            try
            {
                lorg = _masterCategoryContext.category.Where(t => t.AMC_Id==id).ToList();

                if (lorg.Any())
                {
                    _masterCategoryContext.Remove(lorg.ElementAt(0));

                    var flag = _masterCategoryContext.SaveChanges();
                    if (flag == 1)
                    {
                        org.returnval = "Category Deleted Successfully.";
                    }
                    else
                    {
                        org.returnval = "Failed To Delete Category.";
                    }
                }

                org.categoryList = (from a in _masterCategoryContext.category
                                    from b in _masterCategoryContext.institution
                                    where (a.MI_Id == b.MI_Id && a.MI_Id==lorg.FirstOrDefault().MI_Id)
                                    select new MasterCategoryDTO { AMC_Id = a.AMC_Id, AMC_Name = a.AMC_Name, AMC_Type = a.AMC_Type, AMC_ActiveFlag = a.AMC_ActiveFlag, MI_Name = b.MI_Name }
                                   ).OrderByDescending(a => a.CreatedDate).ToArray();
            }
            catch (Exception ee)
            {
                org.returnval = "Sorry You Can Not Delete This Record.Because It Is Mapped With Student.";
                Console.WriteLine(ee.Message);
            }

            return org;
        }
        public MasterCategoryDTO deactivate(MasterCategoryDTO acd)
        {
            try
            {
                MasterCategory enq = Mapper.Map<MasterCategory>(acd);

                //Organisation enq = new Organisation();
                // enq.MO_Id = 12;
                var ismapped = _db.Masterclasscategory.Where(t => t.AMC_Id == enq.AMC_Id).ToList();
                if(ismapped.Count > 0)
                {
                    acd.returnval = "You Can Not Activate/Deactivate This Record.Because It Is Mapped To Student.";
                }
                else
                {
                    if (enq.AMC_Id > 0)
                    {
                        var result = _masterCategoryContext.category.Single(t => t.AMC_Id == enq.AMC_Id);

                        if (acd.AMC_ActiveFlag == 1)
                        {
                            result.AMC_ActiveFlag = 0;
                        }
                        else if (acd.AMC_ActiveFlag == 0)
                        {
                            result.AMC_ActiveFlag = 1;
                        }
                        result.UpdatedDate = DateTime.Now;

                        _masterCategoryContext.Update(result);
                        var flag = _masterCategoryContext.SaveChanges();
                        if (flag > 0)
                        {
                            if (result.AMC_ActiveFlag == 1)
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

                        acd.categoryList = (from a in _masterCategoryContext.category
                                            from b in _masterCategoryContext.institution
                                            where (a.MI_Id == b.MI_Id && a.MI_Id == result.MI_Id)
                                            select new MasterCategoryDTO { AMC_Id = a.AMC_Id, AMC_Name = a.AMC_Name, AMC_Type = a.AMC_Type, AMC_ActiveFlag = a.AMC_ActiveFlag, MI_Name = b.MI_Name }
                                       ).OrderByDescending(a => a.CreatedDate).ToArray();
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
