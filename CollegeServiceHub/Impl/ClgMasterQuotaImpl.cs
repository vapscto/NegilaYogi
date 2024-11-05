using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Admission;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Impl
{
    public class ClgMasterQuotaImpl : Interface.ClgMasterQuotaInterface
    {
        public ClgAdmissionContext _ClgAdmissionContext;
        ILogger<ClgMasterQuotaImpl> _logClgAdmission;
        public ClgMasterQuotaImpl(ClgAdmissionContext ClgAdmissionContext, ILogger<ClgMasterQuotaImpl> log)
        {
            _ClgAdmissionContext = ClgAdmissionContext;
            _logClgAdmission = log;
        }

        public ClgQuotaDTO getalldetails(ClgQuotaDTO data)
        {
            try
            {
                data.getdetails = _ClgAdmissionContext.Clg_Adm_College_QuotaDMO.Where(a => a.MI_Id == data.MI_Id).OrderBy(a => a.ACQ_Id).ToArray();
                data.getdetails1 = _ClgAdmissionContext.Clg_Adm_College_Quota_CategoryDMO.Where(a => a.MI_Id == data.MI_Id).OrderBy(a => a.ACQC_Id).ToArray();

                data.getdetails2 = (from a in _ClgAdmissionContext.Clg_Adm_College_QuotaDMO
                                    from b in _ClgAdmissionContext.Clg_Adm_College_Quota_CategoryDMO
                                    from c in _ClgAdmissionContext.Clg_Adm_College_Quota_Category_MappingDMO
                                    where (a.MI_Id == b.MI_Id && a.ACQ_Id == c.ACQ_Id && b.ACQC_Id == c.ACQC_Id && a.MI_Id==data.MI_Id && b.MI_Id == data.MI_Id
                                    && c.MI_Id == data.MI_Id)
                                    select new ClgQuotaDTO
                                    {
                                        ACQCM_Id = c.ACQCM_Id,
                                        ACQ_QuotaName = a.ACQ_QuotaName,
                                        ACQC_CategoryName = b.ACQC_CategoryName,
                                        ACQCM_ActiveFlg = c.ACQCM_ActiveFlg

                                    }).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                _logClgAdmission.LogInformation("Master Branch  getalldetails :" + ex.Message);
            }
            return data;
        }


        //--------------Master Quota--------------//
        public ClgQuotaDTO savedetails(ClgQuotaDTO data)
        {

            try
            {

                if (data.ACQ_Id != 0)
                {
                    var res = _ClgAdmissionContext.Clg_Adm_College_QuotaDMO.Where(t => t.ACQ_QuotaName == data.ACQ_QuotaName && t.MI_Id == data.MI_Id &&
                    t.ACQ_Id != data.ACQ_Id).ToList();

                    var res1 = _ClgAdmissionContext.Clg_Adm_College_QuotaDMO.Where(t => t.MI_Id == data.MI_Id && t.ACQ_QuotaCode == data.ACQ_QuotaCode && t.ACQ_Id != data.ACQ_Id).ToList();


                    if (res.Count > 0 || res1.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _ClgAdmissionContext.Clg_Adm_College_QuotaDMO.Single(t => t.ACQ_Id == data.ACQ_Id);
                        result.MI_Id = data.MI_Id;
                        result.ACQ_QuotaName = data.ACQ_QuotaName;
                        result.ACQ_QuotaCode = data.ACQ_QuotaCode;
                        result.ACQ_QuotaInfo = data.ACQ_QuotaInfo;
                        result.ACQ_ActiveFlg = true;
                        result.UpdatedDate = DateTime.Now;
                        _ClgAdmissionContext.Update(result);
                        var contactExists = _ClgAdmissionContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            data.message = "Update";
                            data.returnval = true;
                        }
                        else
                        {
                            data.message = "Update";
                            data.returnval = false;
                        }

                    }
                }
                else
                {
                    var res = _ClgAdmissionContext.Clg_Adm_College_QuotaDMO.Where(t => (t.ACQ_QuotaName == data.ACQ_QuotaName) && t.MI_Id == data.MI_Id).ToList();
                    var res1 = _ClgAdmissionContext.Clg_Adm_College_QuotaDMO.Where(t => (t.ACQ_QuotaCode == data.ACQ_QuotaCode) && t.MI_Id == data.MI_Id).ToList();
                    if (res.Count > 0 || res1.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var row_cnt = _ClgAdmissionContext.Clg_Adm_College_QuotaDMO.Where(t => t.ACQ_Id == data.ACQ_Id && t.MI_Id == data.MI_Id).ToList().Count;
                        Clg_Adm_College_QuotaDMO quota = new Clg_Adm_College_QuotaDMO();
                        quota.MI_Id = data.MI_Id;
                        quota.ACQ_QuotaName = data.ACQ_QuotaName;
                        quota.ACQ_QuotaCode = data.ACQ_QuotaCode;
                        quota.ACQ_QuotaInfo = data.ACQ_QuotaInfo;
                        quota.ACQ_ActiveFlg = true;
                        quota.CreatedDate = DateTime.Now;
                        quota.UpdatedDate = DateTime.Now;

                        _ClgAdmissionContext.Add(quota);
                        var contactExists = _ClgAdmissionContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            data.message = "Add";
                            data.returnval = true;
                        }
                        else
                        {
                            data.message = "Add";
                            data.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                data.message = "Error";
                _logClgAdmission.LogInformation("Master Quota save data :" + ex.Message);
            }
            return data;
        }
        public ClgQuotaDTO activedeactiveQuota(ClgQuotaDTO data)
        {
            try
            {
                data.already_cnt = false;
                if (data.ACQ_Id > 0)
                {

                    var check_qutaused = (from a in _ClgAdmissionContext.Clg_Adm_College_QuotaDMO
                                          from b in _ClgAdmissionContext.Clg_Adm_College_Quota_Category_MappingDMO
                                          where (a.ACQ_Id == b.ACQ_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id
                                          && b.ACQCM_ActiveFlg == true && b.ACQ_Id == data.ACQ_Id)
                                          select new ClgQuotaDTO
                                          {
                                              ACQ_Id = b.ACQ_Id
                                          }).ToList();


                    var check_qutaused1 = (from a in _ClgAdmissionContext.Clg_Adm_College_QuotaDMO
                                           from b in _ClgAdmissionContext.Adm_Master_College_StudentDMO
                                           where (a.ACQ_Id == b.ACQ_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id
                                           && b.ACQ_Id == data.ACQ_Id)
                                           select new ClgQuotaDTO
                                           {
                                               ACQ_Id = b.ACQ_Id
                                           }).ToList();


                    if (check_qutaused.Count == 0 && check_qutaused1.Count == 0)
                    {
                        var result = _ClgAdmissionContext.Clg_Adm_College_QuotaDMO.Single(t => t.ACQ_Id == data.ACQ_Id && t.MI_Id == data.MI_Id);
                        if (result.ACQ_ActiveFlg == true)
                        {
                            result.ACQ_ActiveFlg = false;
                        }
                        else
                        {
                            result.ACQ_ActiveFlg = true;
                        }
                        result.UpdatedDate = DateTime.Now;
                        _ClgAdmissionContext.Update(result);
                        var flag = _ClgAdmissionContext.SaveChanges();
                        if (flag == 1)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                    else
                    {
                        data.message = "Mapped";
                    }
                }
            }
            catch (Exception ex)
            {
                _logClgAdmission.LogInformation("Master Quota activedeactiveQuota :" + ex.Message);
                data.message = "Error";
            }
            return data;
        }
        public ClgQuotaDTO editdetails(ClgQuotaDTO data)
        {
            try
            {
                data.editdetails = _ClgAdmissionContext.Clg_Adm_College_QuotaDMO.Where(a => a.MI_Id == data.MI_Id && a.ACQ_Id == data.ACQ_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logClgAdmission.LogInformation("Master Quota editQuota :" + ex.Message);
                data.message = "Error";
            }
            return data;
        }

        //---------------Master Quota Category---------------//
        public ClgQuotaDTO savedetails1(ClgQuotaDTO data)
        {
            try
            {
                if (data.ACQC_Id != 0)
                {
                    var res = _ClgAdmissionContext.Clg_Adm_College_Quota_CategoryDMO.Where(t => t.ACQC_CategoryName == data.ACQC_CategoryName && t.ACQC_Id != data.ACQC_Id && t.MI_Id == data.MI_Id).ToList();

                    var res1 = _ClgAdmissionContext.Clg_Adm_College_Quota_CategoryDMO.Where(t => t.ACQC_CategoryCode == data.ACQC_CategoryCode && t.ACQC_Id != data.ACQC_Id && t.MI_Id == data.MI_Id).ToList();


                    if (res.Count > 0 || res1.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _ClgAdmissionContext.Clg_Adm_College_Quota_CategoryDMO.Single(t => t.ACQC_Id == data.ACQC_Id);
                        result.MI_Id = data.MI_Id;
                        result.ACQC_CategoryName = data.ACQC_CategoryName;
                        result.ACQC_CategoryCode = data.ACQC_CategoryCode;
                        result.ACQC_CategoryInfo = data.ACQC_CategoryInfo;
                        result.ACQC_ActiveFlg = true;
                        result.UpdatedDate = DateTime.Now;
                        _ClgAdmissionContext.Update(result);
                        var contactExists = _ClgAdmissionContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            data.message = "Update";
                            data.returnval = true;
                        }
                        else
                        {
                            data.message = "Update";
                            data.returnval = false;
                        }

                    }
                }
                else
                {
                    var res = _ClgAdmissionContext.Clg_Adm_College_Quota_CategoryDMO.Where(t => (t.ACQC_CategoryName == data.ACQC_CategoryName) && t.MI_Id == data.MI_Id).ToList();

                    var res1 = _ClgAdmissionContext.Clg_Adm_College_Quota_CategoryDMO.Where(t => (t.ACQC_CategoryCode == data.ACQC_CategoryCode) && t.MI_Id == data.MI_Id).ToList();


                    if (res.Count > 0 || res1.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var row_cnt = _ClgAdmissionContext.Clg_Adm_College_Quota_CategoryDMO.Where(t => t.ACQC_Id == data.ACQC_Id).ToList().Count;
                        Clg_Adm_College_Quota_CategoryDMO quota = new Clg_Adm_College_Quota_CategoryDMO();
                        quota.MI_Id = data.MI_Id;
                        quota.ACQC_CategoryName = data.ACQC_CategoryName;
                        quota.ACQC_CategoryCode = data.ACQC_CategoryCode;
                        quota.ACQC_CategoryInfo = data.ACQC_CategoryInfo;
                        quota.ACQC_ActiveFlg = true;
                        quota.CreatedDate = DateTime.Now;
                        quota.UpdatedDate = DateTime.Now;

                        _ClgAdmissionContext.Add(quota);
                        var contactExists = _ClgAdmissionContext.SaveChanges();
                        if (contactExists == 1)
                        {
                            data.message = "Add";
                            data.returnval = true;
                        }
                        else
                        {
                            data.message = "Add";
                            data.returnval = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logClgAdmission.LogInformation("Master Quota category save data :" + ex.Message);
                data.message = "Error";
            }
            return data;
        }
        public ClgQuotaDTO activedeactiveQuota1(ClgQuotaDTO data)
        {
            try
            {
                data.already_cnt = false;
                if (data.ACQC_Id > 0)
                {

                    var check_quotacategory = (from a in _ClgAdmissionContext.Clg_Adm_College_Quota_CategoryDMO
                                               from b in _ClgAdmissionContext.Clg_Adm_College_Quota_Category_MappingDMO
                                               where (a.ACQC_Id == b.ACQC_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id
                                               && b.ACQCM_ActiveFlg == true && b.ACQC_Id == data.ACQC_Id)
                                               select new ClgQuotaDTO
                                               {
                                                   ACQC_Id = b.ACQC_Id

                                               }).ToList();

                    var check_quotacategory1 = (from a in _ClgAdmissionContext.Clg_Adm_College_Quota_CategoryDMO
                                                from b in _ClgAdmissionContext.Adm_Master_College_StudentDMO
                                                where (a.ACQC_Id == b.ACQC_Id && a.MI_Id == data.MI_Id && b.MI_Id == data.MI_Id
                                                 && b.ACQC_Id == data.ACQC_Id)
                                                select new ClgQuotaDTO
                                                {
                                                    ACQC_Id = b.ACQC_Id

                                                }).ToList();


                    if (check_quotacategory.Count == 0 && check_quotacategory1.Count == 0)
                    {
                        var result = _ClgAdmissionContext.Clg_Adm_College_Quota_CategoryDMO.Single(t => t.ACQC_Id == data.ACQC_Id && t.MI_Id == data.MI_Id);
                        if (result.ACQC_ActiveFlg == true)
                        {
                            result.ACQC_ActiveFlg = false;
                        }
                        else
                        {
                            result.ACQC_ActiveFlg = true;
                        }
                        result.UpdatedDate = DateTime.Now;
                        _ClgAdmissionContext.Update(result);
                        var flag = _ClgAdmissionContext.SaveChanges();
                        if (flag == 1)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                    else
                    {
                        data.message = "Mapped";
                    }
                }
            }
            catch (Exception ex)
            {
                _logClgAdmission.LogInformation("Master Quota category activedeactiveQuota :" + ex.Message);
                data.message = "Error";
            }
            return data;
        }
        public ClgQuotaDTO editdetails1(ClgQuotaDTO data)
        {
            try
            {
                data.editdetails1 = _ClgAdmissionContext.Clg_Adm_College_Quota_CategoryDMO.Where(a => a.MI_Id == data.MI_Id && a.ACQC_Id == data.ACQC_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logClgAdmission.LogInformation("Master Quota category editQuota :" + ex.Message);
                data.message = "Error";
            }
            return data;
        }

        //--------------Master Quota Category Mapping--------------//
        public ClgQuotaDTO savedetails2(ClgQuotaDTO data)
        {
            try
            {
                List<long> Quotalist = new List<long>();

                foreach (ClgQuotaDTO item in data.QuotaClist)
                {
                    var result = _ClgAdmissionContext.Clg_Adm_College_Quota_Category_MappingDMO.Where(t => t.ACQ_Id == data.ACQ_Id && t.ACQC_Id == item.ACQC_Id && t.MI_Id == data.MI_Id).ToList();
                    if (result.Count > 0)
                    {
                        var res = _ClgAdmissionContext.Clg_Adm_College_Quota_Category_MappingDMO.Single(t => t.ACQCM_Id == result.FirstOrDefault().ACQCM_Id && t.MI_Id == data.MI_Id);
                        res.UpdatedDate = DateTime.Now;
                        _ClgAdmissionContext.Update(res);
                    }
                    else
                    {
                        Quotalist.Add(item.ACQC_Id);
                        Clg_Adm_College_Quota_Category_MappingDMO caqcm = new Clg_Adm_College_Quota_Category_MappingDMO();
                        caqcm.MI_Id = data.MI_Id;
                        caqcm.ACQ_Id = data.ACQ_Id;
                        caqcm.ACQC_Id = item.ACQC_Id;
                        caqcm.ACQCM_ActiveFlg = true;
                        caqcm.CreatedDate = DateTime.Now;
                        caqcm.UpdatedDate = DateTime.Now;
                        _ClgAdmissionContext.Add(caqcm);
                    }
                }
                var contactExists = _ClgAdmissionContext.SaveChanges();
                if (contactExists > 0)
                {
                    data.message = "Add";
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                    data.message = "Add";
                }

            }
            catch (Exception ex)
            {
                _logClgAdmission.LogInformation("Master Quota category save data :" + ex.Message);
                data.message = "Error";
            }
            return data;
        }

        public ClgQuotaDTO activedeactiveQuota2(ClgQuotaDTO data)
        {
            try
            {
                data.already_cnt = false;
                if (data.ACQCM_Id > 0)
                {
                    var result = _ClgAdmissionContext.Clg_Adm_College_Quota_Category_MappingDMO.Single(t => t.ACQCM_Id == data.ACQCM_Id && t.MI_Id == data.MI_Id);
                    if (result.ACQCM_ActiveFlg == true)
                    {
                        result.ACQCM_ActiveFlg = false;
                    }
                    else
                    {
                        result.ACQCM_ActiveFlg = true;
                    }
                    result.UpdatedDate = DateTime.Now;
                    _ClgAdmissionContext.Update(result);
                    var flag = _ClgAdmissionContext.SaveChanges();
                    if (flag == 1)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                }
            }
            catch (Exception ex)
            {
                _logClgAdmission.LogInformation("Master Quota category Mapping activedeactiveQuota :" + ex.Message);
                data.message = "Error";
            }
            return data;
        }

    }
}
