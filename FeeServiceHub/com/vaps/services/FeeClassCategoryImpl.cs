using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model.com.vaps.Fee;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;    

namespace FeeServiceHub.com.vaps.services
{
    public class FeeClassCategoryImpl : interfaces.FeeClassCategoryInterface
    {
        private static ConcurrentDictionary<string, FeeClassCategoryDTO> _login =
        new ConcurrentDictionary<string, FeeClassCategoryDTO>();

        private static ConcurrentDictionary<string, FeeYearlyClassCategoryDTO> _login1 =
             new ConcurrentDictionary<string, FeeYearlyClassCategoryDTO>();

        public FeeGroupContext _FeeGroupContext;
        readonly ILogger<FeeGroupImplimentation> _logger;
        public FeeClassCategoryImpl(FeeGroupContext frgContext, ILogger<FeeGroupImplimentation> log)
        {
            _FeeGroupContext = frgContext;
            _logger = log;
        }
        public FeeClassCategoryDTO SaveGroupData(FeeClassCategoryDTO FGpage)
        {
            bool returnresult = false;
            FeeClassCategoryDMO feepge = Mapper.Map<FeeClassCategoryDMO>(FGpage);
            string retval = "";
            try
            {
                if (feepge.FMCC_Id > 0)
                {


                    var duplicateresult = _FeeGroupContext.feeCC.Where(t => t.FMCC_Id != feepge.FMCC_Id && t.FMCC_ClassCategoryName == feepge.FMCC_ClassCategoryName && t.MI_Id==feepge.MI_Id);

                    if (duplicateresult.Count() > 0)
                    {
                        retval = "Duplicate";
                        FGpage.returnduplicatestatus = retval;
                    }
                    else
                    {
                        

                        var result = _FeeGroupContext.feeCC.Single(t => t.FMCC_Id == feepge.FMCC_Id);
                        result.MI_Id = feepge.MI_Id;
                        result.FMCC_ClassCategoryName = feepge.FMCC_ClassCategoryName;
                        result.FMCC_ClassCategoryCode = feepge.FMCC_ClassCategoryCode;
                        result.FMCC_ActiveFlag = feepge.FMCC_ActiveFlag;
                        result.UpdatedDate = DateTime.Now;
                        result.FMCC_UpdatedBy = FGpage.user_id;
                        _FeeGroupContext.Update(result);
                        var contactExists = _FeeGroupContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            returnresult = true;
                            FGpage.returnval = returnresult;
                            FGpage.message = "Update";
                        }
                        else
                        {
                            returnresult = false;
                            FGpage.returnval = returnresult;
                        }
                    }
                }
                else
                {
                    var result = _FeeGroupContext.feeCC.Where(t => t.FMCC_ClassCategoryName == feepge.FMCC_ClassCategoryName && t.FMCC_ClassCategoryCode == feepge.FMCC_ClassCategoryCode && t.MI_Id== feepge.MI_Id).ToList();
                    if (result.Count() > 0)
                    {
                        retval = "Duplicate";
                        FGpage.returnduplicatestatus = retval;
                    }
                    else
                    {
                        feepge.CreatedDate = DateTime.Now;
                        feepge.UpdatedDate = DateTime.Now;

                        feepge.FMCC_CreatedBy = FGpage.user_id;
                        feepge.FMCC_UpdatedBy = FGpage.user_id;

                        _FeeGroupContext.Add(feepge);
                        var contactExists = _FeeGroupContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            returnresult = true;
                            FGpage.returnval = returnresult;
                        }
                        else
                        {
                            returnresult = false;
                            FGpage.returnval = returnresult;
                        }
                    }
                }

                List<FeeClassCategoryDMO> allpages = new List<FeeClassCategoryDMO>();
                allpages = _FeeGroupContext.feeCC.Where(t=>t.MI_Id== feepge.MI_Id && t.FMCC_ActiveFlag==true).OrderByDescending(t => t.CreatedDate).ToList();
                FGpage.ClaSSCategoryArray = allpages.ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FGpage;
        }
        public FeeClassCategoryDTO getdetails(FeeClassCategoryDTO org)
        {
            FeeClassCategoryDTO FGRDT = new FeeClassCategoryDTO();
            FeeYearlyClassCategoryDTO fygdto = new FeeYearlyClassCategoryDTO();
            try
            {
                List<FeeClassCategoryDMO> feegrp = new List<FeeClassCategoryDMO>();
                feegrp = _FeeGroupContext.feeCC.Where(t => t.MI_Id == org.MI_Id).OrderByDescending(t => t.FMCC_Id).ToList();
                FGRDT.ClaSSCategoryArray = feegrp.ToArray();

                List<FeeClassCategoryDMO> feegrp1 = new List<FeeClassCategoryDMO>();
                feegrp1 = _FeeGroupContext.feeCC.Where(t => t.MI_Id == org.MI_Id && t.FMCC_ActiveFlag==true).ToList();
                FGRDT.classcategorydrp = feegrp1.ToArray();

              

                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = _FeeGroupContext.AcademicYear.Where(t => t.MI_Id == org.MI_Id  && t.Is_Active==true).OrderByDescending(t=>t.ASMAY_Order).ToList(); 
                FGRDT.academicdrp = allyear.Distinct().ToArray();

                List<School_M_Class> result = new List<School_M_Class>();
                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FeeClassCategory";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                        SqlDbType.BigInt)
                    {
                        Value = org.ASMAY_ID
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                    {
                        Value = org.MI_Id
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    try
                    {

                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new School_M_Class
                                {
                                    ASMCL_Id = Convert.ToInt64(dataReader["ASMCL_Id"])
                                });
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.Message);
                    }
                }
                List<School_M_Class> cls = new List<School_M_Class>();
                foreach (var item in result)
                {

                    var admcls = _FeeGroupContext.School_M_Class.Where(d => d.ASMCL_Id == item.ASMCL_Id).FirstOrDefault();
                    cls.Add(admcls);
                }
                FGRDT.admclas = cls.ToArray();

                FGRDT.clsYearData = (from a in _FeeGroupContext.feeCC
                                     from b in _FeeGroupContext.feeYCC
                                     from c in _FeeGroupContext.feeYCCC
                                     from d in _FeeGroupContext.admissioncls
                                     from e in _FeeGroupContext.AcademicYear
                                     where (a.FMCC_Id == b.FMCC_Id && b.FYCC_Id == c.FYCC_Id && c.ASMCL_Id == d.ASMCL_Id && a.MI_Id == org.MI_Id && e.ASMAY_Id == b.ASMAY_Id && b.FYCC_ActiveFlag == true)
                                     select new FeeClassCategoryDTO
                                     {
                                         FMCC_ClassCategoryName = a.FMCC_ClassCategoryName,
                                         ASMCL_ClassName = d.ASMCL_ClassName,
                                         ASMAY_Year = e.ASMAY_Year,
                                         FMCC_ActiveFlag = b.FYCC_ActiveFlag,

                                         FYCCC_Id = c.FYCCC_Id,
                                         FYCC_Id = c.FYCC_Id,
                                         FMCC_Id = c.FYCCC_Id,
                                     }
                          ).ToArray();



            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FGRDT;
        }
        public FeeClassCategoryDTO EditgroupDetails(int id)
        {
            FeeClassCategoryDTO FMG = new FeeClassCategoryDTO();
            try
            {
                List<FeeClassCategoryDMO> masterfeegroup = new List<FeeClassCategoryDMO>();
                masterfeegroup = _FeeGroupContext.feeCC.AsNoTracking().Where(t => t.FMCC_Id.Equals(id)).ToList();
                FMG.ClaSSCategoryArray = masterfeegroup.ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FMG;
        }
        public FeeClassCategoryDTO GetGroupSearchData(FeeClassCategoryDTO mas)
        {

            FeeClassCategoryDTO FGRDT = new FeeClassCategoryDTO();
            try
            {
                List<FeeClassCategoryDMO> feegrp = new List<FeeClassCategoryDMO>();
                feegrp = _FeeGroupContext.feeCC.ToList();
                FGRDT.ClaSSCategoryArray = feegrp.ToArray();

            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FGRDT;

        }
        public FeeClassCategoryDTO getpageedit(int id)
        {
            FeeClassCategoryDTO page = new FeeClassCategoryDTO();
            try
            {
                List<FeeClassCategoryDMO> lorg = new List<FeeClassCategoryDMO>();
                lorg = _FeeGroupContext.feeCC.AsNoTracking().Where(t => t.FMCC_Id.Equals(id)).ToList();
                page.ClaSSCategoryArray = lorg.ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public FeeClassCategoryDTO deleterec(int id)
        {
            bool returnresult = false;
            FeeClassCategoryDTO page = new FeeClassCategoryDTO();

            FeeAmountEntryDTO page4 = new FeeAmountEntryDTO();
            List<FeeAmountEntryDMO> lorg4 = new List<FeeAmountEntryDMO>();
            lorg4 = _FeeGroupContext.FeeAmountEntryDMO.Where(t => t.FMCC_Id.Equals(id)).ToList();
            if (lorg4.Count() == 0)
            {

                List<FeeYearlyClassCategoryDMO> lorgcheck = new List<FeeYearlyClassCategoryDMO>();
                lorgcheck = _FeeGroupContext.feeYCC.Where(t => t.FMCC_Id.Equals(id)).ToList();
                if (lorgcheck.Count() == 0)
                {
                    List<FeeClassCategoryDMO> lorg = new List<FeeClassCategoryDMO>();
                    lorg = _FeeGroupContext.feeCC.Where(t => t.FMCC_Id.Equals(id)).ToList();

                    try
                    {
                        if (lorg.Any())
                        {
                            _FeeGroupContext.Remove(lorg.ElementAt(0));
                            var contactExists = _FeeGroupContext.SaveChanges();
                            if (contactExists == 1)
                            {
                                returnresult = true;
                                page.returnval = returnresult;
                            }
                            else
                            {
                                returnresult = false;
                                page.returnval = returnresult;
                            }
                        }

                        List<FeeClassCategoryDMO> allpages = new List<FeeClassCategoryDMO>();
                        allpages = _FeeGroupContext.feeCC.ToList();
                        page.ClaSSCategoryArray = allpages.ToArray();
                    }
                    catch (Exception ee)
                    {
                        _logger.LogError(ee.Message);
                        Console.WriteLine(ee.Message);
                    }
                }
                else
                {
                    page.retvalue = true;
                }
            }
            else
            {
                page.retvalue = true;

            }
            return page;
        }
        //public FeeClassCategoryDTO deactivate(FeeClassCategoryDTO acd)
        //{
        //    try
        //    {
        //        FeeClassCategoryDMO feepge = Mapper.Map<FeeClassCategoryDMO>(acd);
        //        if (feepge.FMCC_Id > 0)
        //        {
        //            var result = _FeeGroupContext.feeCC.Single(t => t.FMCC_Id == feepge.FMCC_Id);
        //            if (result.FMCC_ActiveFlag == true)
        //            {
        //                result.FMCC_ActiveFlag = false;
        //            }
        //            else
        //            {
        //                result.FMCC_ActiveFlag = true;
        //            }
        //            result.UpdatedDate = DateTime.Now;
        //            _FeeGroupContext.Update(result);
        //            var flag = _FeeGroupContext.SaveChanges();
        //            if (flag == 1)
        //            {
        //                acd.returnval = true;
        //            }
        //            else
        //            {
        //                acd.returnval = false;
        //            }



        //            List<FeeClassCategoryDMO> allpages = new List<FeeClassCategoryDMO>();
        //            allpages = _FeeGroupContext.feeCC.Where(t => t.MI_Id == feepge.MI_Id && t.FMCC_ActiveFlag == true).OrderByDescending(t => t.CreatedDate).ToList();
        //            acd.ClaSSCategoryArray = allpages.ToArray();

        //            List<FeeClassCategoryDMO> feegrp = new List<FeeClassCategoryDMO>();
        //            feegrp = _FeeGroupContext.feeCC.Where(t => t.MI_Id == feepge.MI_Id).OrderByDescending(t => t.CreatedDate).ToList();
        //            acd.classcategorydrp = feegrp.ToArray();

        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogError(e.Message);
        //        Console.WriteLine(e.InnerException);
        //    }
        //    return acd;
        //}


            //Added by Praveen gouda 
        public FeeClassCategoryDTO deactivate(FeeClassCategoryDTO acd)
        {
            try
            {
                FeeClassCategoryDMO feepge = Mapper.Map<FeeClassCategoryDMO>(acd);
                if (feepge.FMCC_Id > 0)
                {
                    var result = _FeeGroupContext.feeCC.Single(t => t.FMCC_Id == feepge.FMCC_Id);
                    if (result.FMCC_ActiveFlag == true)
                    {
                        var resultnew = _FeeGroupContext.feeYCC.Where(k => k.FMCC_Id == feepge.FMCC_Id).Select(t => t.FMCC_Id).ToList();

                        if (resultnew.Count > 0)
                        {
                            acd.message = "used";
                            return acd;
                        }
                        else
                        {
                            result.FMCC_ActiveFlag = false;
                        }
                    }
                    else
                    {
                        result.FMCC_ActiveFlag = true;
                    }
                    if (acd.message != "used")
                    {
                        result.UpdatedDate = DateTime.Now;
                        _FeeGroupContext.Update(result);
                        var flag = _FeeGroupContext.SaveChanges();
                        if (flag == 1)
                        {
                            acd.returnval = true;
                        }
                        else
                        {
                            acd.returnval = false;
                        }
                    }
                    List<FeeClassCategoryDMO> allorganisation = new List<FeeClassCategoryDMO>();
                    allorganisation = _FeeGroupContext.feeCC.ToList();
                    acd.classcategorydrp = allorganisation.ToArray();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                Console.WriteLine(e.InnerException);
            }
            return acd;
        }
        //Ended


        // for yearly 

        [Route("years")]
        public async Task<FeeClassCategoryDTO> getIndependentDropDowns(FeeClassCategoryDTO yrs)
        {
            try
            {
                List<MasterAcademic> allyear = new List<MasterAcademic>();
                allyear = await _FeeGroupContext.AcademicYear.ToListAsync();
                yrs.academicdrp = allyear.ToArray();

                List<FeeClassCategoryDMO> allclasscategory = new List<FeeClassCategoryDMO>();
                allclasscategory = await _FeeGroupContext.feeCC.ToListAsync();
                yrs.classcategorydrp = allclasscategory.ToArray();



            }
            catch (Exception ex)
            {
                var str = ex.Message;
            }

            return yrs;
        }
        public FeeYearlyClassCategoryDTO SaveYearlyGroupData(int id, FeeYearlyClassCategoryDTO FGpage)
        {

            bool returnresult = false;
            string dupl = "false";

            TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime indianTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, INDIAN_ZONE);

            FeeYearlyClassCategoryDMO feepge = Mapper.Map<FeeYearlyClassCategoryDMO>(FGpage);
            string retval = "";
            try
            {
                if (feepge.FYCC_Id > 0)
                {
                    var result = _FeeGroupContext.feeYCC.Single(t => t.FYCC_Id == feepge.FYCC_Id);

                    result.MI_Id = feepge.MI_Id;
                    result.ASMAY_Id = feepge.ASMAY_Id;
                    result.FMCC_Id = feepge.FMCC_Id;

                    result.UpdatedDate = indianTime;
                    result.FYCC_UpdatedBy = FGpage.user_id;

                    var conte = _FeeGroupContext.Database.ExecuteSqlCommand("Insert_Fee_yearly_class_category @p0,@p1,@p2,@p3,@p4,@p5", feepge.MI_Id, feepge.ASMAY_Id, feepge.FMCC_Id, id, feepge.FYCC_ActiveFlag, feepge.FYCC_Id, FGpage.user_id);
                }
                else
                {
                    var result = _FeeGroupContext.feeYCC.Where(t => t.FMCC_Id == feepge.FMCC_Id && t.ASMAY_Id == feepge.ASMAY_Id && t.MI_Id== feepge.MI_Id);
                    if (result.Count() > 0)
                    {
                        List<FeeYearlyClassCategoryDMO> allrecords = new List<FeeYearlyClassCategoryDMO>();
                        allrecords = _FeeGroupContext.feeYCC.Where(t => t.FMCC_Id == feepge.FMCC_Id && t.ASMAY_Id == feepge.ASMAY_Id && t.MI_Id==feepge.MI_Id).ToList();
                        if (allrecords.Count > 0)
                        {
                            for (int i = 0; allrecords.Count > i; i++)
                            {
                                List<MasterYearlyClassCategoryClassDMO> allrecordscheck = new List<MasterYearlyClassCategoryClassDMO>();
                                allrecordscheck = _FeeGroupContext.feeYCCC.Where(t => t.ASMCL_Id == FGpage.ASMCL_ID && t.FYCC_Id == allrecords[i].FYCC_Id).ToList();
                                if (allrecordscheck.Count() > 0)
                                {
                                    dupl = "false";
                                }
                                else
                                {
                                    dupl = "true";
                                }
                            }
                            if (dupl == "false")
                            {
                                retval = "Duplicate";
                                FGpage.returnduplicatestatus = retval;
                            }
                            else
                            {
                                var conte = _FeeGroupContext.Database.ExecuteSqlCommand("Insert_Fee_yearly_class_category @p0,@p1,@p2,@p3,@p4,@p5,@p6", feepge.MI_Id, feepge.ASMAY_Id, feepge.FMCC_Id, id, feepge.FYCC_ActiveFlag, feepge.FYCC_Id, FGpage.user_id);
                            }
                        }
                    }
                    else
                    {

                        var conte = _FeeGroupContext.Database.ExecuteSqlCommand("Insert_Fee_yearly_class_category @p0,@p1,@p2,@p3,@p4,@p5,@p6", feepge.MI_Id, feepge.ASMAY_Id, feepge.FMCC_Id, id, feepge.FYCC_ActiveFlag, feepge.FYCC_Id, FGpage.user_id);


                    }
                }

                FGpage.clsYearData = (from a in _FeeGroupContext.feeCC
                                      from b in _FeeGroupContext.feeYCC
                                      from c in _FeeGroupContext.feeYCCC
                                      from d in _FeeGroupContext.admissioncls
                                      from e in _FeeGroupContext.AcademicYear
                                      where (a.FMCC_Id == b.FMCC_Id && b.FYCC_Id == c.FYCC_Id && c.ASMCL_Id == d.ASMCL_Id && a.MI_Id == feepge.MI_Id && b.ASMAY_Id == feepge.ASMAY_Id && e.ASMAY_Id == b.ASMAY_Id)
                                      select new FeeClassCategoryDTO
                                      {
                                          FMCC_ClassCategoryName = a.FMCC_ClassCategoryName,
                                          ASMCL_ClassName = d.ASMCL_ClassName,
                                          ASMAY_Year = e.ASMAY_Year,
                                          FMCC_ActiveFlag = b.FYCC_ActiveFlag,

                                          FYCCC_Id = c.FYCCC_Id,
                                          FYCC_Id = c.FYCC_Id,
                                          FMCC_Id = c.FYCCC_Id,
                                      }
      ).ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            //   }
            return FGpage;
        }
        public FeeYearlyClassCategoryDTO getdetailsY(int id)
        {
            FeeYearlyClassCategoryDTO FGRDT = new FeeYearlyClassCategoryDTO();
            try
            {
                FGRDT.clsYearData = (from a in _FeeGroupContext.feeCC
                                     from b in _FeeGroupContext.feeYCC
                                     from c in _FeeGroupContext.feeYCCC
                                     from d in _FeeGroupContext.admissioncls
                                     from e in _FeeGroupContext.AcademicYear
                                     where (a.FMCC_Id == b.FMCC_Id && b.FYCC_Id == c.FYCC_Id && c.ASMCL_Id == d.ASMCL_Id && a.MI_Id == 2 && b.ASMAY_Id == 10 && e.ASMAY_Id == b.ASMAY_Id)
                                     select new FeeClassCategoryDTO
                                     {
                                         FMCC_ClassCategoryName = a.FMCC_ClassCategoryName,
                                         ASMCL_ClassName = d.ASMCL_ClassName,
                                         ASMAY_Year = e.ASMAY_Year,
                                         FMCC_ActiveFlag = b.FYCC_ActiveFlag,

                                         FYCCC_Id = c.FYCCC_Id,
                                         FYCC_Id = c.FYCC_Id,
                                         FMCC_Id = c.FYCCC_Id,
                                     }
    ).ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return FGRDT;

        }
        public FeeYearlyClassCategoryDTO deactivateY(FeeYearlyClassCategoryDTO acd)
        {
            try
            {
                FeeYearlyClassCategoryDMO feepge = Mapper.Map<FeeYearlyClassCategoryDMO>(acd);
                if (feepge.FYCC_Id > 0)
                {
                    var result = _FeeGroupContext.feeYCC.Single(t => t.FYCC_Id == feepge.FYCC_Id);
                    result.UpdatedDate = DateTime.Now;
                    if (result.FYCC_ActiveFlag == true)
                    {
                        result.FYCC_ActiveFlag = false;
                    }
                    else
                    {
                        result.FYCC_ActiveFlag = true;
                    }
                    _FeeGroupContext.Update(result);
                    var flag = _FeeGroupContext.SaveChanges();
                    if (flag == 1)
                    {
                        acd.returnval = true;
                    }
                    else
                    {
                        acd.returnval = false;
                    }

                    List<FeeYearlyClassCategoryDMO> allorganisation = new List<FeeYearlyClassCategoryDMO>();
                    allorganisation = _FeeGroupContext.feeYCC.ToList();
                    acd.clsYearData = allorganisation.ToArray();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                Console.WriteLine(e.InnerException);
            }
            return acd;
        }
        public FeeYearlyClassCategoryDTO getpageeditY(int id)
        {
            FeeYearlyClassCategoryDTO page = new FeeYearlyClassCategoryDTO();
            try
            {

                page.clsYearData = (from a in _FeeGroupContext.feeYCC
                                    from b in _FeeGroupContext.feeYCCC
                                    where (a.FYCC_Id == b.FYCC_Id && a.FYCC_Id == id && a.ASMAY_Id == 10 && a.MI_Id == 2)
                                    select new FeeClassCategoryDTO
                                    {
                                        ASMAY_ID = a.ASMAY_Id,
                                        FMCC_Id = a.FMCC_Id,
                                        ASMCL_ID = b.ASMCL_Id

                                    }
   ).ToArray();
            }

            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }
            return page;
        }
        public FeeYearlyClassCategoryDTO deleterecY(int id)
        {
            bool returnresult = false;
            FeeYearlyClassCategoryDTO page = new FeeYearlyClassCategoryDTO();
            List<MasterYearlyClassCategoryClassDMO> lorgcheck123 = new List<MasterYearlyClassCategoryClassDMO>();
            lorgcheck123 = _FeeGroupContext.feeYCCC.Where(t => t.FYCC_Id.Equals(id)).ToList();
            if (lorgcheck123.Count() > 0)
            {
                _FeeGroupContext.Remove(lorgcheck123.ElementAt(0));
                var contactExists = _FeeGroupContext.SaveChanges();
                if (contactExists == 1)
                {
                    returnresult = true;
                    page.returnval = returnresult;
                }
                else
                {
                    returnresult = false;
                    page.returnval = returnresult;
                }

            }
            else
            {
                returnresult = false;
                page.returnval = returnresult;
            }

            return page;
        }
        public FeeYearlyClassCategoryDTO loaddata(FeeYearlyClassCategoryDTO dto)
        {
            try
            {

                List<School_M_Class> result = new List<School_M_Class>();
                using (var cmd = _FeeGroupContext.Database.GetDbConnection().CreateCommand())
                {
                    cmd.CommandText = "FeeClassCategory";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ASMAY_ID",
                        SqlDbType.BigInt)
                    {
                        Value = dto.ASMAY_Id
                    });
                    cmd.Parameters.Add(new SqlParameter("@MI_Id",
                       SqlDbType.BigInt)
                    {
                        Value = dto.MI_Id
                    });
                    if (cmd.Connection.State != ConnectionState.Open)
                        cmd.Connection.Open();

                    try
                    {
                        // var data = cmd.ExecuteNonQuery();

                        using (var dataReader = cmd.ExecuteReader())
                        {
                            while (dataReader.Read())
                            {
                                result.Add(new School_M_Class
                                {
                                    ASMCL_Id = Convert.ToInt64(dataReader["ASMCL_Id"])
                                });
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.Message);
                    }
                }
                List<School_M_Class> cls = new List<School_M_Class>();
                foreach (var item in result)
                {

                    var admcls = _FeeGroupContext.School_M_Class.Where(d => d.ASMCL_Id == item.ASMCL_Id).FirstOrDefault();
                    cls.Add(admcls);
                }
                dto.clsYearData = cls.ToArray();

    //            dto.clsYearData = (from a in _FeeGroupContext.feeCC
    //                               from b in _FeeGroupContext.feeYCC
    //                               from c in _FeeGroupContext.feeYCCC
    //                               from d in _FeeGroupContext.admissioncls
    //                               from e in _FeeGroupContext.AcademicYear
    //                               where (a.FMCC_Id == dto.FMCC_Id && a.FMCC_Id == b.FMCC_Id && b.FYCC_Id == c.FYCC_Id && c.ASMCL_Id == d.ASMCL_Id && a.MI_Id == dto.MI_Id && b.ASMAY_Id == dto.ASMAY_Id && e.ASMAY_Id == b.ASMAY_Id)
    //                               select new FeeClassCategoryDTO
    //                               {
    //                                   FMCC_ClassCategoryName = a.FMCC_ClassCategoryName,
    //                                   ASMCL_ClassName = d.ASMCL_ClassName,
    //                                   ASMAY_Year = e.ASMAY_Year,
    //                                   FMCC_ActiveFlag = b.FYCC_ActiveFlag,
    //                                   ASMCL_ID = d.ASMCL_Id,

    //                               }
    //).ToArray();
            }
            catch (Exception ee)
            {
                _logger.LogError(ee.Message);
                Console.WriteLine(ee.Message);
            }

            return dto;
        }
    }
}
