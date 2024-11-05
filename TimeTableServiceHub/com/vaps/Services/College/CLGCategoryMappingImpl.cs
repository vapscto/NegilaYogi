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
    public class CLGCategoryMappingImpl : Interfaces.CLGCategoryMappingInterface
    {
        private static ConcurrentDictionary<string, CLGCategoryMappingDTO> _login =
             new ConcurrentDictionary<string, CLGCategoryMappingDTO>();

        public TTContext _TTContext;
        ILogger<CategoryClassMappImpl> _dataimpl;
        public DomainModelMsSqlServerContext _db;
        public CLGCategoryMappingImpl(TTContext academiccontext, ILogger<CategoryClassMappImpl> dataimpl, DomainModelMsSqlServerContext db)
        {
            _TTContext = academiccontext;
            _dataimpl = dataimpl;
            _db = db;
        }
        public CLGCategoryMappingDTO getalldetails(CLGCategoryMappingDTO data)
        {
            try
            {

                data.categorylist = _TTContext.TTMasterCategoryDMO.Where(t => t.MI_Id==data.MI_Id&& t.TTMC_ActiveFlag==true).ToList().ToArray();
                data.yearlist = _TTContext.AcademicYear.AsNoTracking().Where(t => t.MI_Id==data.MI_Id && t.Is_Active == true).OrderByDescending(yy=>yy.ASMAY_Order).ToList().ToArray();

                data.courselist = _TTContext.MasterCourseDMO.AsNoTracking().Where(t => t.MI_Id==data.MI_Id && t.AMCO_ActiveFlag == true).ToList().ToArray();

                data.detailslist = (from m in _TTContext.CLGTT_Category_CourseBranchDMO
                                    from n in _TTContext.TTMasterCategoryDMO
                                     from p in _TTContext.MasterCourseDMO
                                     from g in _TTContext.ClgMasterBranchDMO
                                    from f in _TTContext.AcademicYear
                                     where (p.MI_Id == n.MI_Id && p.MI_Id == data.MI_Id && g.MI_Id==p.MI_Id && p.MI_Id==f.MI_Id && m.TTMC_Id == n.TTMC_Id && m.ASMAY_Id==f.ASMAY_Id && m.AMB_Id==g.AMB_Id && m.AMCO_Id==p.AMCO_Id && p.AMCO_ActiveFlag==true && g.AMB_ActiveFlag==true && f.Is_Active==true )
                                     select new CLGCategoryMappingDTO
                                     {
                                         ASMAY_Year = f.ASMAY_Year,
                                         AMCO_CourseName = p.AMCO_CourseName,
                                         AMB_BranchName = g.AMB_BranchName,
                                         TTMC_CategoryName = n.TTMC_CategoryName,
                                         TTCCB_Id = m.TTCCB_Id,
                                         UpdatedDate=m.UpdatedDate

                                     }).Distinct().OrderByDescending(d => d.UpdatedDate).ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }

        public CLGCategoryMappingDTO getBranch(CLGCategoryMappingDTO data)
        {
            try
            {


                data.branchlist = (from a in _TTContext.CLG_Adm_College_AY_CourseDMO
                                   from b in _TTContext.CLG_Adm_College_AY_Course_BranchDMO
                                   from c in _TTContext.ClgMasterBranchDMO
                                   where a.MI_Id == b.MI_Id && a.MI_Id == data.MI_Id && a.MI_Id == c.MI_Id && a.ACAYC_Id == b.ACAYC_Id && a.ASMAY_Id == data.ASMAY_Id && a.AMCO_Id == data.AMCO_Id && b.AMB_Id == c.AMB_Id && a.ACAYC_ActiveFlag == true && b.ACAYCB_ActiveFlag == true
                                   select c
                                 ).Distinct().ToArray();
            

                data.savedbranch = (from m in _TTContext.CLGTT_Category_CourseBranchDMO
                                    from n in _TTContext.TTMasterCategoryDMO
                                     where ( n.MI_Id == data.MI_Id && m.TTMC_Id == n.TTMC_Id && m.ASMAY_Id==data.ASMAY_Id  && m.TTCC_ActiveFlag==true && n.TTMC_ActiveFlag==true && m.AMCO_Id==data.AMCO_Id)
                                     select m).Distinct().ToArray();

            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }



        public CLGCategoryMappingDTO savedetails(CLGCategoryMappingDTO data)
        {
            try
            {
                var amcl = data.clssids.Select(t => t.AMB_Id).ToArray();
                var mappedbranchlist = _TTContext.CLGTT_Category_CourseBranchDMO.Where(t => t.ASMAY_Id.Equals(data.ASMAY_Id) && t.TTMC_Id.Equals(data.TTMC_Id) && !amcl.Contains(t.AMB_Id) && t.AMCO_Id == data.AMCO_Id).ToList().ToList();

                for (int p = 0; p < mappedbranchlist.Count; p++)
                {
                    var branchlist = _TTContext.CLGTT_Category_CourseBranchDMO.Where(t => t.TTCCB_Id == mappedbranchlist[p].TTCCB_Id).ToList();

                    if (branchlist.Any())
                    {
                        _TTContext.Remove(branchlist.ElementAt(0));
                        var flag = _TTContext.SaveChanges();

                        if (flag == 1)
                        {
                            data.returnMsg = "update";
                        }
                        else
                        {
                            data.returnMsg = "";
                        }
                    }

                }

                for (int j = 0; j < data.clssids.Length; j++)
                {

                    var duplist= _TTContext.CLGTT_Category_CourseBranchDMO.Where(t => t.ASMAY_Id.Equals(data.ASMAY_Id) && t.TTMC_Id.Equals(data.TTMC_Id) && t.AMB_Id== data.clssids[j].AMB_Id && t.AMCO_Id == data.AMCO_Id).ToList().ToList();
                    if (duplist.Count==0)
                    {
                        CLGTT_Category_CourseBranchDMO obj = new CLGTT_Category_CourseBranchDMO(); ;
                        obj.TTMC_Id = data.TTMC_Id;
                        obj.ASMAY_Id = data.ASMAY_Id;
                        obj.AMCO_Id = data.AMCO_Id;
                        obj.AMB_Id = data.clssids[j].AMB_Id;
                        obj.TTCC_ActiveFlag = true;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        _TTContext.Add(obj);
                        var flag = _TTContext.SaveChanges();
                        if (flag == 1)
                        {
                            data.returnMsg = "add";

                        }
                        else
                        {
                            data.returnMsg = "";
                        }
                    }
                  
                }
            }
            
            catch (Exception ee)
            {
                _dataimpl.LogError(ee.Message);
                _dataimpl.LogDebug(ee.Message);
            }

            return data;
        }

        public CLGCategoryMappingDTO deleterec(CLGCategoryMappingDTO data)
        {

            List<TT_Category_Class_DMO> ldata = new List<TT_Category_Class_DMO>();


            try
            {
                ldata = _TTContext.TT_Category_Class_DMO.Where(t => t.TTCC_Id == data.TTCC_Id).ToList();

                if (ldata.Any())
                {
                    _TTContext.Remove(ldata.ElementAt(0));
                    var flag = _TTContext.SaveChanges();
                    if (flag == 1)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }


                data.detailslist = (from m in _TTContext.TT_Category_Class_DMO
                                   from n in _TTContext.TTMasterCategoryDMO
                                   from p in _TTContext.School_M_Class
                                   from f in _TTContext.AcademicYear
                                   where (m.MI_Id == n.MI_Id && m.MI_Id == data.MI_Id && m.TTMC_Id == n.TTMC_Id && m.ASMCL_Id == p.ASMCL_Id && f.ASMAY_Id == m.ASMAY_Id)
                                   select new CLGCategoryMappingDTO
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
                data.message = "Sorry...You Can't delete this record because it is used in other operation.";
                Console.WriteLine(ee.Message);
            }

            return data;
        }

    }
}
