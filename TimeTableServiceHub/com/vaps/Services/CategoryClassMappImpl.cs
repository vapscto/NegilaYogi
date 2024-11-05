using AutoMapper;
using DataAccessMsSqlServerProvider;
using DataAccessMsSqlServerProvider.com.vapstech.TT;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.TT;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.TT;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TimeTableServiceHub.Services
{
    public class CategoryClassMappImpl : Interfaces.CategoryClassMappInterface
    {
        private static ConcurrentDictionary<string, TT_Category_Class_DTO> _login =
             new ConcurrentDictionary<string, TT_Category_Class_DTO>();

        public TTContext _AcademicContext;
        ILogger<CategoryClassMappImpl> _acdimpl;
        public DomainModelMsSqlServerContext _db;
        public CategoryClassMappImpl(TTContext academiccontext, ILogger<CategoryClassMappImpl> acdimpl, DomainModelMsSqlServerContext db)
        {
            _AcademicContext = academiccontext;
            _acdimpl = acdimpl;
            _db = db;
        }
        public TT_Category_Class_DTO getallDetails(TT_Category_Class_DTO acdmc)
        {
            try
            {

                acdmc.categorylist = _AcademicContext.TTMasterCategoryDMO.AsNoTracking().Where(t => t.MI_Id.Equals(acdmc.MI_Id)).ToList().ToArray();
                acdmc.acdlist = _AcademicContext.AcademicYear.AsNoTracking().Where(t => t.MI_Id.Equals(acdmc.MI_Id) && t.Is_Active == true).OrderByDescending(yy=>yy.ASMAY_Order).ToList().ToArray();
                acdmc.classlist = _AcademicContext.School_M_Class.AsNoTracking().Where(t => t.MI_Id.Equals(acdmc.MI_Id) && t.ASMCL_ActiveFlag == true).ToList().ToArray();

                acdmc.detailslist = (from m in _AcademicContext.TT_Category_Class_DMO
                                     from n in _AcademicContext.TTMasterCategoryDMO
                                     from p in _AcademicContext.School_M_Class
                                     from f in _AcademicContext.AcademicYear
                                     where (m.MI_Id == n.MI_Id && m.MI_Id == acdmc.MI_Id && m.TTMC_Id == n.TTMC_Id && m.ASMCL_Id == p.ASMCL_Id && f.ASMAY_Id == m.ASMAY_Id)
                                     select new TT_Category_Class_DTO
                                     {
                                         academicName = f.ASMAY_Year,
                                         className = p.ASMCL_ClassName,
                                         categoryName = n.TTMC_CategoryName,
                                         UpdatedDate = m.UpdatedDate,
                                         TTCC_Id = m.TTCC_Id

                                     }).OrderByDescending(d => d.UpdatedDate).ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return acdmc;
        }

        public TT_Category_Class_DTO getdetails(TT_Category_Class_DTO id)
        {

            try
            {

                var lorg3 = _AcademicContext.TT_Category_Class_DMO.AsNoTracking().Where(t => t.ASMAY_Id.Equals(id.ASMAY_Id) && t.MI_Id.Equals(id.MI_Id) && t.TTMC_Id.Equals(id.TTMC_Id)).Select(d => d.ASMCL_Id).ToArray();

                var lorg2 = _AcademicContext.TT_Category_Class_DMO.AsNoTracking().Where(t => t.ASMAY_Id.Equals(id.ASMAY_Id) && t.MI_Id.Equals(id.MI_Id) &&
                !lorg3.Contains(t.ASMCL_Id)).Select(d => d.ASMCL_Id).ToArray();

                id.classlist = _AcademicContext.School_M_Class.AsNoTracking().Where(t => t.MI_Id.Equals(id.MI_Id) && !lorg2.Contains(t.ASMCL_Id)).ToList().ToArray();


               var lorg4 = _AcademicContext.TT_Category_Class_DMO.AsNoTracking().Where(t => lorg3.Contains(t.ASMCL_Id)).Select(d => d.ASMCL_Id).ToArray();
                id.binddetails = _AcademicContext.School_M_Class.AsNoTracking().Where(t => t.MI_Id.Equals(id.MI_Id) && lorg4.Contains(t.ASMCL_Id)).ToList().ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return id;
        }

        public TT_Category_Class_DTO saveProsdet(TT_Category_Class_DTO acd)
        {
            try
            {
                var amcl = acd.clssids.Select(t => t.ASMCL_Id).ToArray();

                List<TT_Category_Class_DMO> Allname5 = new List<TT_Category_Class_DMO>();

                Allname5 = _AcademicContext.TT_Category_Class_DMO.Where(t => t.ASMAY_Id.Equals(acd.ASMAY_Id) && t.MI_Id.Equals(acd.MI_Id) && t.TTMC_Id.Equals(acd.TTMC_Id) && !amcl.Contains(t.ASMCL_Id)).ToList().ToList();

                for (int p = 0; p < Allname5.Count; p++)
                {
                    List<TT_Category_Class_DMO> lorg = new List<TT_Category_Class_DMO>();

                    lorg = _AcademicContext.TT_Category_Class_DMO.Where(t => t.TTCC_Id == Allname5[p].TTCC_Id).ToList();

                    if (lorg.Any())
                    {
                        _AcademicContext.Remove(lorg.ElementAt(0));
                        var flag = _AcademicContext.SaveChanges();

                        if (flag == 1)
                        {
                            acd.returnMsg = "update";

                            acd.detailslist = (from m in _AcademicContext.TT_Category_Class_DMO
                                               from n in _AcademicContext.TTMasterCategoryDMO
                                               from l in _AcademicContext.School_M_Class
                                               from f in _AcademicContext.AcademicYear
                                               where (m.MI_Id == n.MI_Id && m.MI_Id == acd.MI_Id && m.TTMC_Id == n.TTMC_Id && m.ASMCL_Id == l.ASMCL_Id && f.ASMAY_Id == m.ASMAY_Id)
                                               select new TT_Category_Class_DTO
                                               {
                                                   academicName = f.ASMAY_Year,
                                                   className = l.ASMCL_ClassName,
                                                   categoryName = n.TTMC_CategoryName,
                                                   UpdatedDate = m.UpdatedDate,
                                                   TTCC_Id = m.TTCC_Id

                                               }).OrderByDescending(d => d.UpdatedDate).ToArray();
                        }
                        else
                        {
                            acd.returnMsg = "";
                        }
                    }

                }

                for (int j = 0; j < acd.clssids.Length; j++)
                {



                    TT_Category_Class_DMO enq = Mapper.Map<TT_Category_Class_DMO>(acd);

                    List<TT_Category_Class_DMO> Allname2 = new List<TT_Category_Class_DMO>();

                    Allname2 = _AcademicContext.TT_Category_Class_DMO.Where(t => t.ASMAY_Id.Equals(acd.ASMAY_Id) && t.MI_Id.Equals(acd.MI_Id) && t.ASMCL_Id.Equals(acd.clssids[j].ASMCL_Id)).ToList().ToList();

                    if (Allname2.Count > 0)
                    {

                    }
                    else
                    {
                        TT_Category_Class_DMO enq1 = Mapper.Map<TT_Category_Class_DMO>(acd);
                        enq1.MI_Id = enq.MI_Id;
                        enq1.TTMC_Id = acd.TTMC_Id;
                        enq1.ASMCL_Id = acd.clssids[j].ASMCL_Id;
                        enq1.ASMAY_Id = enq.ASMAY_Id;
                        enq1.CreatedDate = DateTime.Now;
                        enq1.UpdatedDate = DateTime.Now;
                        enq1.TTCC_ActiveFlag = true;
                        _AcademicContext.Add(enq1);


                        var flag = _AcademicContext.SaveChanges();
                        if (flag == 1)
                        {
                            acd.returnMsg = "add";

                            acd.detailslist = (from m in _AcademicContext.TT_Category_Class_DMO
                                               from n in _AcademicContext.TTMasterCategoryDMO
                                               from p in _AcademicContext.School_M_Class
                                               from f in _AcademicContext.AcademicYear
                                               where (m.MI_Id == n.MI_Id && m.MI_Id == acd.MI_Id && m.TTMC_Id == n.TTMC_Id && m.ASMCL_Id == p.ASMCL_Id && f.ASMAY_Id == m.ASMAY_Id)
                                               select new TT_Category_Class_DTO
                                               {
                                                   academicName = f.ASMAY_Year,
                                                   className = p.ASMCL_ClassName,
                                                   categoryName = n.TTMC_CategoryName,
                                                   UpdatedDate = m.UpdatedDate,
                                                   TTCC_Id = m.TTCC_Id

                                               }).OrderByDescending(d => d.UpdatedDate).ToArray();
                        }
                        else
                        {
                            acd.returnMsg = "";
                        }
                    }
                }


            }
            catch (Exception ee)
            {
                _acdimpl.LogError(ee.Message);
                _acdimpl.LogDebug(ee.Message);
                Console.WriteLine(ee.Message);
            }

            return acd;
        }

        public TT_Category_Class_DTO deleterec(TT_Category_Class_DTO org)
        {

            List<TT_Category_Class_DMO> lorg = new List<TT_Category_Class_DMO>();


            try
            {
                lorg = _AcademicContext.TT_Category_Class_DMO.Where(t => t.TTCC_Id == org.TTCC_Id).ToList();

                if (lorg.Any())
                {
                    _AcademicContext.Remove(lorg.ElementAt(0));
                    var flag = _AcademicContext.SaveChanges();
                    if (flag == 1)
                    {
                        org.returnval = true;
                    }
                    else
                    {
                        org.returnval = false;
                    }
                }


                org.detailslist = (from m in _AcademicContext.TT_Category_Class_DMO
                                   from n in _AcademicContext.TTMasterCategoryDMO
                                   from p in _AcademicContext.School_M_Class
                                   from f in _AcademicContext.AcademicYear
                                   where (m.MI_Id == n.MI_Id && m.MI_Id == org.MI_Id && m.TTMC_Id == n.TTMC_Id && m.ASMCL_Id == p.ASMCL_Id && f.ASMAY_Id == m.ASMAY_Id)
                                   select new TT_Category_Class_DTO
                                   {
                                       academicName = f.ASMAY_Year,
                                       className = p.ASMCL_ClassName,
                                       categoryName = n.TTMC_CategoryName,
                                       UpdatedDate = m.UpdatedDate,
                                       TTCC_Id = m.TTCC_Id

                                   }).OrderByDescending(d => d.UpdatedDate).ToArray();


            }
            catch (Exception ee)
            {
                org.message = "Sorry...You Can't delete this record because it is used in other operation.";
                Console.WriteLine(ee.Message);
            }

            return org;
        }

    }
}
