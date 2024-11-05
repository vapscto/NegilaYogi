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
    public class ClgMasterAcademicYearImpl : Interface.ClgMasterAcademicYearInterface
    {
        public ClgAdmissionContext _ClgAdmissionContext;
        ILogger<ClgMasterAcademicYearImpl> _logaccyear;
        public ClgMasterAcademicYearImpl(ClgAdmissionContext ClgAdmissionContext, ILogger<ClgMasterAcademicYearImpl> log)
        {
            _ClgAdmissionContext = ClgAdmissionContext;
            _logaccyear = log;
        }

        public ClgMasterAcademicYearDTO getalldetails(ClgMasterAcademicYearDTO data)
        {
            try
            {
                data.getdetails = _ClgAdmissionContext.ClgMasterAcademicYearDMO.Where(a => a.MI_Id == data.MI_Id).OrderBy(a => a.ACMAY_AYOrder).ToArray();
                data.getyear = _ClgAdmissionContext.ClgMasterAcademicYearDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMAY_Id == data.ACMAY_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logaccyear.LogInformation("College Admission master year getalldetails :" + ex.Message);
            }
            return data;
        }
        public ClgMasterAcademicYearDTO saveaccyear(ClgMasterAcademicYearDTO data)
        {
            try
            {
                if (data.ACMAY_Id > 0)
                {
                    var result = _ClgAdmissionContext.ClgMasterAcademicYearDMO.Single(a => a.MI_Id == data.MI_Id && a.ACMAY_Id == data.ACMAY_Id);
                    result.ACMAY_AYFromDate = data.ACMAY_AYFromDate;
                    result.ACMAY_AYToDate = data.ACMAY_AYToDate;
                    result.ACMAY_AYOrder = data.ACMAY_AYOrder;
                    result.ACMAY_AcademicYearCode = data.ACMAY_AcademicYearCode;
                    result.ACMAB_PAActiveFlg = data.ACMAB_PAActiveFlg;
                    _ClgAdmissionContext.Update(result);
                    var ii = _ClgAdmissionContext.SaveChanges();
                    if (ii > 0)
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
                else
                {
                    ClgMasterAcademicYearDMO year = new ClgMasterAcademicYearDMO();

                    var checkduplicate = _ClgAdmissionContext.ClgMasterAcademicYearDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMAY_AcademicYear == data.ACMAY_AcademicYear).ToList();

                    var checkduplicate1 = _ClgAdmissionContext.ClgMasterAcademicYearDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMAY_AcademicYearCode == data.ACMAY_AcademicYearCode).ToList();

                    var checkduplicate2 = _ClgAdmissionContext.ClgMasterAcademicYearDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMAY_AYOrder == data.ACMAY_AYOrder).ToList();

                    if (checkduplicate.Count == 0 && checkduplicate1.Count == 0 && checkduplicate2.Count == 0)
                    {
                        year.ACMAY_AcademicYear = data.ACMAY_AcademicYear;
                        year.MI_Id = data.MI_Id;
                        year.ACMAY_AcademicYearCode = data.ACMAY_AcademicYearCode;
                        year.ACMAY_AYOrder = data.ACMAY_AYOrder;
                        year.ACMAY_AYFromDate = data.ACMAY_AYFromDate;
                        year.ACMAY_AYToDate = data.ACMAY_AYToDate;
                        year.ACMAB_PAActiveFlg = data.ACMAB_PAActiveFlg;
                        year.Is_Active = true;

                        _ClgAdmissionContext.Add(year);
                        var ii = _ClgAdmissionContext.SaveChanges();

                        if (ii > 0)
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
                    else
                    {
                        data.message = "Duplicate";
                    }

                }
            }
            catch (Exception ex)
            {
                _logaccyear.LogInformation("College Admission master year saveaccyear :" + ex.Message);
            }
            return data;
        }

        public ClgMasterAcademicYearDTO edit(ClgMasterAcademicYearDTO data)
        {
            try
            {
                data.editdetails = _ClgAdmissionContext.ClgMasterAcademicYearDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMAY_Id == data.ACMAY_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logaccyear.LogInformation("College Admission master year edit :" + ex.Message);
            }
            return data;
        }
        
        public ClgMasterAcademicYearDTO deactivate(ClgMasterAcademicYearDTO data)
        {
            try
            {

                var check_used = _ClgAdmissionContext.Adm_College_Yearly_StudentDMO.Where(a => a.ASMAY_Id == data.ACMAY_Id).ToArray();
                if (check_used.Count() > 0)
                {
                    data.message = "Record Is Already Mapped You Can Not Deactive This";
                }
                else
                {
                    var result = _ClgAdmissionContext.ClgMasterAcademicYearDMO.Single(a => a.MI_Id == data.MI_Id && a.ACMAY_Id == data.ACMAY_Id);
                    if (result.Is_Active == true)
                    {
                        result.Is_Active = false;
                    }
                    else
                    {
                        result.Is_Active = true;
                    }
                    result.UpdatedDate = DateTime.Now;
                    _ClgAdmissionContext.Update(result);
                    var ii = _ClgAdmissionContext.SaveChanges();
                    if (ii > 0)
                    {
                        data.message = "";
                        data.returnval = true;
                    }
                    else
                    {
                        data.message = "";
                        data.returnval = false;
                    }
                }

                
            }
            catch(Exception ex)
            {
                _logaccyear.LogInformation("College Admission master year deactivate :" + ex.Message);
            }
            return data;
        }

        public ClgMasterAcademicYearDTO saveorder(ClgMasterAcademicYearDTO data)
        {
            try
            {
                if (data.yearorder.Count() > 0)
                {
                    int id = 0;
                    for(int i=0;i< data.yearorder.Count(); i++)
                    {
                        var reult = _ClgAdmissionContext.ClgMasterAcademicYearDMO.Single(t => t.MI_Id == data.MI_Id && t.ACMAY_Id == data.yearorder[i].ACMAY_Id);
                        id = id + 1;

                        if (i == 0)
                        {
                            reult.ACMAY_AYOrder = id;
                        }
                        else
                        {
                            reult.ACMAY_AYOrder = id;
                        }
                        _ClgAdmissionContext.Update(reult);
                        var flag = _ClgAdmissionContext.SaveChanges();
                        if (flag > 0)
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
                _logaccyear.LogInformation("College Admission master year saveorder :" + ex.Message);
            }
            return data;
        }
    }
}
