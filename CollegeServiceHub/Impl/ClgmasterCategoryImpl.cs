using AutoMapper;
using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Admission;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Impl
{
    public class ClgmasterCategoryImpl : Interface.ClgMasterCategoryInterface
    {
        private static ConcurrentDictionary<string, ClgMasterCategoryDTO> MsCadm =
             new ConcurrentDictionary<string, ClgMasterCategoryDTO>();

        public ClgAdmissionContext mastercourse;
        public ClgmasterCategoryImpl(ClgAdmissionContext mscadm)
        {
            mastercourse = mscadm;
        }

        public ClgMasterCategoryDTO Savedetails(ClgMasterCategoryDTO id)
        {
            try
            {
                if (id.AMCOC_Id > 0)
                {
                    var checkvalidation = mastercourse.mastercategory.Where(a => a.MI_Id == id.MI_Id && a.AMCOC_Name.Equals(id.AMCOC_Name) && a.AMCOC_Id != id.AMCOC_Id).ToList();

                    if (checkvalidation.Count > 0)
                    {
                        id.message = "Duplicate";
                    }
                    else
                    {
                        var result = mastercourse.mastercategory.Single(a => a.MI_Id == id.MI_Id && a.AMCOC_Id == id.AMCOC_Id);
                        result.AMCOC_Name = id.AMCOC_Name;
                        result.AMCOC_Address = id.AMCOC_Address;
                        result.AMCOC_Details = id.AMCOC_Details;
                        result.AMCOC_Type = id.AMCOC_Type;
                        result.ACMC_RegNoPrefix = id.ACMC_RegNoPrefix;

                        result.UpdatedDate = DateTime.Now;

                        mastercourse.Update(result);
                        var ii = mastercourse.SaveChanges();
                        if (ii > 0)
                        {
                            id.returnval = true;
                            id.message = "Update";
                        }
                        else
                        {
                            id.returnval = false;
                            id.message = "Update";
                        }
                    }
                }
                else
                {

                    var checkvalidation = mastercourse.mastercategory.Where(a => a.MI_Id == id.MI_Id && a.AMCOC_Name.Equals(id.AMCOC_Name)).ToList();

                    if (checkvalidation.Count > 0)
                    {
                        id.message = "Duplicate";
                    }
                    else
                    {
                        ClgMasterCategoryDMO branchdmo = new ClgMasterCategoryDMO();
                        branchdmo.MI_Id = id.MI_Id;
                        branchdmo.AMCOC_Name = id.AMCOC_Name;
                        branchdmo.AMCOC_Address = id.AMCOC_Address;
                        branchdmo.AMCOC_Details = id.AMCOC_Details;
                        branchdmo.AMCOC_Type = id.AMCOC_Type;
                        branchdmo.ACMC_RegNoPrefix = id.ACMC_RegNoPrefix;
                        branchdmo.ACMC_ActiveFlag = true;

                        branchdmo.CreatedDate = DateTime.Now;
                        branchdmo.UpdatedDate = DateTime.Now;


                        mastercourse.Add(branchdmo);
                        var ii = mastercourse.SaveChanges();
                        if (ii > 0)
                        {
                            id.returnval = true;
                            id.message = "Add";
                        }
                        else
                        {
                            id.returnval = false;
                            id.message = "Add";
                        }

                    }
                }

            }
            catch (Exception msg)
            {
                Console.WriteLine(msg.Message);
            }
            return id;
        }

        public ClgMasterCategoryDTO getalldetails(int id)
        {
            ClgMasterCategoryDTO data = new ClgMasterCategoryDTO();
            try
            {
                data.masterCategoryList = mastercourse.mastercategory.Where(t => t.MI_Id == Convert.ToInt64(id)).Distinct().ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }

        public ClgMasterCategoryDTO Deletedetails(ClgMasterCategoryDTO id)
        {
            try
            {


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return id;
        }

        public ClgMasterCategoryDTO deactivate(ClgMasterCategoryDTO data)
        {
            try
            {
                var check1 = (from a in mastercourse.mastercategory
                              from b in mastercourse.ClgMasterCourseCategorycategoryMap
                              where (a.AMCOC_Id == b.AMCOC_Id && a.MI_Id == data.MI_Id && b.AMCOC_Id == data.AMCOC_Id && b.AMCOCM_ActiveFlg == true)
                              select new ClgMasterCategoryDTO
                              {
                                  AMCOC_Id = b.AMCOC_Id
                              }).ToList();

                var check2 = (from a in mastercourse.mastercategory
                              from b in mastercourse.Adm_Master_College_StudentDMO
                              where (a.AMCOC_Id == b.AMCOC_Id && a.MI_Id == data.MI_Id && b.AMCOC_Id == data.AMCOC_Id)
                              select new ClgMasterCategoryDTO
                              {
                                  AMCOC_Id =Convert.ToInt64(b.AMCOC_Id)
                              }).ToList();

                if(check1.Count==0 && check2.Count == 0)
                {
                    var result = mastercourse.mastercategory.Single(a => a.MI_Id == data.MI_Id && a.AMCOC_Id == data.AMCOC_Id);
                    if (result.ACMC_ActiveFlag == true)
                    {
                        result.ACMC_ActiveFlag = false;
                    }
                    else
                    {
                        result.ACMC_ActiveFlag = true;
                    }
                    mastercourse.Update(result);
                    var i = mastercourse.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }else
                {
                    data.message = "Mapped";                  
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
