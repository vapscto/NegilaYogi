using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs;
using DataAccessMsSqlServerProvider;
using WebApplication1.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using DomainModel.Model;
using AutoMapper;
using System.Collections.Concurrent;

namespace WebApplication1.Services
{
    public class StudentMasterConfigurationImpl : Interfaces.StudentMasterConfigurationInterface
    {
        private static ConcurrentDictionary<string, StudentApplicationDTO> _login = 
             new ConcurrentDictionary<string, StudentApplicationDTO>();

        public DomainModelMsSqlServerContext _db;

        public StudentMasterConfigurationImpl(DomainModelMsSqlServerContext dmoc)
        {
            _db = dmoc;
        }

        public async Task<CommonDTO> getRecord(CommonDTO data)
        {
            CommonDTO cdto = new CommonDTO();
            try
            {
                var aa1 = await _db.AcademicYear.Where(t=> t.MI_Id==data.IVRM_MI_Id &&  t.Is_Active == true).OrderByDescending(d => d.ASMAY_Order).ToListAsync();
                cdto.AcademicList = aa1.ToArray();

                var rolelist = await _db.MasterRoleType.Where(t=>t.IVRMRT_Id==data.roleId).ToListAsync();

                
                    if (rolelist[0].IVRMRT_Role.Equals("vaps super admin", StringComparison.OrdinalIgnoreCase) || rolelist[0].IVRMRT_Role.Equals("Super Admin", StringComparison.OrdinalIgnoreCase))
                    {

                    var aa2 = await _db.Institute.Where(t => t.MI_ActiveFlag == 1).ToListAsync();

                    cdto.InstituteList = aa2.ToArray();

                    var aa3 = await _db.Organisation.Where(t => t.MO_ActiveFlag == 1).ToListAsync();
                    cdto.TrustList = aa3.ToArray();

                    var aa4 = await _db.mstConfig.ToListAsync();
                    cdto.masterConfigList = aa4.ToArray();


                    var aa5 = await _db.mastercategory.Where(t => t.AMC_ActiveFlag == 1).ToListAsync();

                    cdto.studentlicategorylist = aa5.ToArray();

                  //  cdto.InstituteList = (from a in _db.Institute
                  //                        from b in _db.Organisation
                  //                        where (a.MO_Id == b.MO_Id && b.MO_ActiveFlag == 1 && a.MI_ActiveFlag == 1)
                  //                        select new CommonDTO
                  //                        {
                  //                            MI_Name = a.MI_Name;

                  //                                }
                  //).ToArray();

                    cdto.masterConfigList = (from a in _db.Institute
                                             from b in _db.Organisation
                                             from c in _db.mstConfig
                                             from d in _db.AcademicYear
                                             where (c.MO_Id == b.MO_Id && c.MI_Id == a.MI_Id && c.ASMAY_Id == d.ASMAY_Id && b.MO_ActiveFlag == 1 && d.Is_Active == true)
                                             select new MasterConfigurationDTO
                                             {
                                                 MO_Name = b.MO_Name,
                                                 MI_Name = a.MI_Name,
                                                 ASMAY_Year = d.ASMAY_Year,
                                                 ISPAC_Id=c.ISPAC_Id
                                             }
 ).ToArray();

                }               
                   else if (rolelist[0].IVRMRT_Role.Equals("Multi Admin", StringComparison.OrdinalIgnoreCase))
                    {                        
                    var aa2 = await _db.Institute.Where(t=>t.MO_Id==data.IVRM_MO_Id).ToListAsync();
                    cdto.InstituteList = aa2.ToArray();

                    var aa3 = await _db.Organisation.Where(t=>t.MO_Id==data.IVRM_MO_Id).ToListAsync();
                    cdto.TrustList = aa3.ToArray();

                    var aa5 = await _db.mastercategory.Where(c => c.AMC_ActiveFlag == 1).ToListAsync();
                    cdto.studentlicategorylist = aa5.ToArray();

                    cdto.masterConfigList = (from a in _db.Institute
                                             from b in _db.Organisation
                                             from c in _db.mstConfig
                                             from d in _db.AcademicYear
                                             where (c.MO_Id == b.MO_Id && c.MI_Id == a.MI_Id && c.ASMAY_Id == d.ASMAY_Id && b.MO_Id== data.IVRM_MO_Id && a.MI_Id==data.IVRM_MI_Id && d.Is_Active == true)
                                             select new MasterConfigurationDTO
                                             {
                                                 MO_Name = b.MO_Name,
                                                 MI_Name = a.MI_Name,
                                                 ASMAY_Year = d.ASMAY_Year,
                                                 ISPAC_Id=c.ISPAC_Id
                                             }
 ).ToArray();
                }               
                    else if (rolelist[0].IVRMRT_Role.Equals("ADMIN", StringComparison.OrdinalIgnoreCase) || rolelist[0].IVRMRT_Role.Equals("COORDINATOR", StringComparison.OrdinalIgnoreCase))
                {
                    var aa2 = await _db.Institute.Where(t => t.MI_Id == data.IVRM_MI_Id).ToListAsync();
                    cdto.InstituteList = aa2.ToArray();

                    var aa3 = await _db.Organisation.Where(t => t.MO_Id == data.IVRM_MO_Id).ToListAsync();
                    cdto.TrustList = aa3.ToArray();


                    var clgorschool = _db.Institute.Where(s => s.MI_Id == data.IVRM_MI_Id).Select(r => r.MI_SchoolCollegeFlag).FirstOrDefault();
                    cdto.MI_SchoolCollegeFlag = clgorschool;

                    if (clgorschool=="U" || clgorschool == "C")
                    {
                        var aa5 = await _db.ClgMasterCategoryDMO.Where(c => c.ACMC_ActiveFlag == true && c.MI_Id == data.IVRM_MI_Id).ToListAsync();
                        cdto.studentlicategorylist = aa5.ToArray();
                       
                    }
                    else
                    {
                        var aa5 = await _db.mastercategory.Where(c => c.AMC_ActiveFlag == 1 && c.MI_Id == data.IVRM_MI_Id).ToListAsync();
                        cdto.studentlicategorylist = aa5.ToArray();
                    }
                  

                    cdto.masterConfigList = (from a in _db.Institute
                                             from b in _db.Organisation
                                             from c in _db.mstConfig
                                             from d in _db.AcademicYear
                                             where (c.MO_Id == b.MO_Id && c.MI_Id == a.MI_Id && c.ASMAY_Id == d.ASMAY_Id && a.MI_Id == data.IVRM_MI_Id && d.Is_Active == true)
                                             select new MasterConfigurationDTO
                                             {
                                                 MO_Name = b.MO_Name,
                                                 MI_Name = a.MI_Name,
                                                 ASMAY_Year = d.ASMAY_Year,
                                                 ISPAC_Id = c.ISPAC_Id
                                             }
 ).ToArray();
                }

            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
            }
            return cdto;
        }

        public MasterConfigurationDTO saveMasterConfig(MasterConfigurationDTO mstConfigData)
        {
            try
            {
                var org = _db.Institute.Where(d => d.MI_Id == mstConfigData.MI_Id).ToArray();
                mstConfigData.MO_Id = org.FirstOrDefault().MO_Id;
                MasterConfiguration mstConf = Mapper.Map<MasterConfiguration>(mstConfigData);
               
                if (mstConfigData.ISPAC_Id == 0)
                {
                    // checked already exist data for the institution on 8-11-2016
                    var existedRecord = _db.mstConfig.Where(d=>d.MI_Id == mstConfigData.MI_Id && d.ASMAY_Id== mstConfigData.ASMAY_Id).ToArray();
                    if(existedRecord.Count() > 0 )
                    {
                        var inst = _db.Institute.Where(d=>d.MI_Id == mstConfigData.MI_Id).ToArray();
                        mstConfigData.message = "Record already exist for institution "+ inst.First().MI_Name; // To display the message 
                    }
                    else
                    {

                        //added by 02/02/2017
                        mstConf.CreatedDate = DateTime.Now;
                        mstConf.UpdatedDate = DateTime.Now;
                        // insert if data is not present for the institute on 8-11-2016
                        _db.Add(mstConf);
                        _db.SaveChanges();
                        mstConfigData.message = "Successfully saved the record"; // To display the message 
                    }
                }
                else
                {   // update
                    var stureg = _db.mstConfig.Single(s => s.ISPAC_Id == mstConfigData.ISPAC_Id);


                    //added by 02/02/2017
                
                    stureg.UpdatedDate = DateTime.Now;
                    Mapper.Map(mstConfigData, stureg);
                    _db.SaveChanges();
                    mstConfigData.message = "Successfully updated the record"; // To display the message 
                }
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
            }
            return mstConfigData;
        }

        public MasterConfigurationDTO getMasterEditData(int id)
        {
            MasterConfigurationDTO returnData = new MasterConfigurationDTO();
            try
            {
                var aa1 = _db.mstConfig.Where(d => d.ISPAC_Id == id);
                returnData.mstConfigList = aa1.ToArray();
                var aa11 = _db.AcademicYear.Where(t => t.ASMAY_Id == aa1.FirstOrDefault().ASMAY_Id);
                returnData.AcademicList = aa11.ToArray();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return returnData;
        }

        public MasterConfigurationDTO deleteRecord(MasterConfigurationDTO data)
        {
            try
            {
                List<MasterConfiguration> mstCong = _db.mstConfig.Where(d => d.ISPAC_Id == data.ISPAC_Id).ToList();
                if (mstCong.Count() > 0)
                {
                    for (int i = 0; i < mstCong.Count(); i++)
                    {
                        _db.Remove(mstCong.ElementAt(i));
                        _db.SaveChanges();
                    }
                }
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
            }
            return data;
        }

    }
}
