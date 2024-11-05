using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.admission;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Services
{
    public class MasterQuarterService : Interfaces.MasterQuarterInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public MasterQuarterService(HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
        {
            _HRMSContext = HRMSContext;
            _Context = Context;
        }
        public HR_Master_QuarterDTO getBasicData(HR_Master_QuarterDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                dto = GetAllDropdownAndDatatableDetails(dto);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Master_QuarterDTO SaveUpdate(HR_Master_QuarterDTO dto)
        {
            dto.retrunMsg = "";
            //try
            //{
            //    HR_Master_Quarter dmoObj = Mapper.Map<HR_Master_Quarter>(dto);

            //    var alldata = _HRMSContext.HR_Master_quarter.Where(t => t.MI_Id == dto.MI_Id && t.HRMQ_QuarterName.Equals(dto.HRMQ_QuarterName)).Count();
            //    if (alldata == 0)
            //    {

            //        if (dmoObj.HRMQ_Id > 0)
            //        {

            //                var duplicateHRMBD_BankName = _HRMSContext.HR_Master_quarter.Where(t => t.MI_Id == dto.MI_Id && t.HRMQ_QuarterName.Equals(dto.HRMQ_QuarterName) && t.HRMQ_Id != dmoObj.HRMQ_Id).Count();
            //            var duplicateaccno = _HRMSContext.HR_Master_quarter.Where(t => t.HRMQ_FromDay == dto.HRMQ_FromDay && t.HRMQ_FromDay >= dto.HRMQ_FromDay && t.HRMQ_ToDay <= dto.HRMQ_ToDay).Count();


            //            if (duplicateHRMBD_BankName == 0 && duplicateaccno==0)
            //                {
            //                    var result = _HRMSContext.HR_Master_quarter.Single(t => t.HRMQ_Id == dmoObj.HRMQ_Id);
            //                   // dto.UpdatedDate = DateTime.Now;

            //                    Mapper.Map(dto, result);
            //                    _HRMSContext.Update(result);
            //                    var flag = _HRMSContext.SaveChanges();
            //                    if (flag > 0)
            //                    {
            //                        dto.retrunMsg = "Update";
            //                    }
            //                    else
            //                    {
            //                        dto.retrunMsg = "AllDuplicate";
            //                    }
            //                }



            //        }
            //        else
            //            {
            //            var duplicateHRMBD_BankName = _HRMSContext.HR_Master_quarter.Where(t => t.MI_Id == dto.MI_Id && t.HRMQ_QuarterName.Equals(dto.HRMQ_QuarterName)).Count();//  && t.HRMQ_FromDay >= dto.HRMQ_FromDay && t.HRMQ_ToDay <= dto.HRMQ_ToDay.Count();
            //            var duplicateaccno = _HRMSContext.HR_Master_quarter.Where(t => t.HRMQ_FromDay == dto.HRMQ_FromDay  && t.HRMQ_FromDay >= dto.HRMQ_FromDay && t.HRMQ_ToDay <= dto.HRMQ_ToDay).Count();


            //            if (duplicateHRMBD_BankName == 0 )
            //            {



            //                dmoObj.HRMQ_ActiveFlg = true;
            //                dmoObj.HRMQ_QuarterName = dto.HRMQ_QuarterName;
            //                dmoObj.HRMQ_ToDay = dto.HRMQ_ToDay;
            //               dmoObj.HRMQ_FromDay = dto.HRMQ_FromDay;
            //              dmoObj.HRMQ_FromMonth = dto.HRMQ_FromMonth;

            //                dmoObj.MI_Id = dto.MI_Id;
            //                dmoObj.HRMQ_ToMonth = dto.HRMQ_ToMonth;

            //                _HRMSContext.Add(dmoObj);
            //                var flag = _HRMSContext.SaveChanges();
            //                if (flag == 1)
            //                {
            //                    dto.retrunMsg = "Add";
            //                }
            //                else
            //                {
            //                    dto.retrunMsg = "false";
            //                }
            //                var result = _HRMSContext.HR_Master_quarter.Single(t => t.HRMQ_Id == dmoObj.HRMQ_Id);

            //                HR_Master_Quarter_Month dmoObjs = Mapper.Map<HR_Master_Quarter_Month>(dto);
            //                dmoObjs.HRMQ_Id = result.HRMQ_Id;
            //                dmoObjs.HRMQM_ActiveFlg = result.HRMQ_ActiveFlg;
            //                dmoObjs.HRMQM_CreatedBy = dto.LogInUserId;
            //                dmoObjs.HRMQM_UpdatedBy = dto.LogInUserId;
            //                _HRMSContext.Add(dmoObjs);
            //                var flags = _HRMSContext.SaveChanges();
            //                if (flags == 1)
            //                {
            //                    dto.retrunMsg = "Add";
            //                }
            //                else
            //                {
            //                    dto.retrunMsg = "false";
            //                }




            //            }



            //        }

            //    }else
            //    {
            //        dto.retrunMsg = "AllDuplicate";
            //    }



            //    dto = GetAllDropdownAndDatatableDetails(dto);
            //}

            //catch (Exception ee)
            //{
            //    Console.WriteLine(ee.Message);
            //    dto.retrunMsg = "Error occured";
            //}
            try
            {


                HR_Master_Quarter dmoObj = Mapper.Map<HR_Master_Quarter>(dto);

                var duplicatecountresult = _HRMSContext.HR_Master_quarter.Where(t => t.MI_Id == dto.MI_Id && t.HRMQ_Id == dto.HRMQ_Id && t.HRMQ_FromDay==dto.HRMQ_FromDay && t.HRMQ_FromMonth==dto.HRMQ_FromMonth && t.HRMQ_ToDay==dto.HRMQ_ToDay && t.HRMQ_ToMonth==dto.HRMQ_ToMonth).Count();
                if (duplicatecountresult == 0)
                {

                    if (dmoObj.HRMQ_Id

 > 0)
                    {

                        var duplicatecount = _HRMSContext.HR_Master_quarter.Where(t => t.MI_Id == dto.MI_Id && t.HRMQ_Id

 == dto.HRMQ_Id

&& t.HRMQ_FromDay == dto.HRMQ_FromDay && t.HRMQ_FromMonth == dto.HRMQ_FromMonth && t.HRMQ_ToDay == dto.HRMQ_ToDay && t.HRMQ_ToMonth == dto.HRMQ_ToMonth).Count();
                        if (duplicatecount == 0)
                        {
                            var result = _HRMSContext.HR_Master_quarter.Single(t => t.HRMQ_Id

 == dmoObj.HRMQ_Id

); dmoObj.HRMQ_ActiveFlg = true;
                            //dto.HRETDS_DepositedDate = DateTime.Now;
                            dmoObj.HRMQ_UpdatedBy = dto.LogInUserId;
                            // dmoObj.HRETDS_CreatedBy = dto.LogInUserId;
                            Mapper.Map(dto, result);
                            _HRMSContext.Update(result);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                        else
                        {
                            dto.retrunMsg = "Duplicate";
                        }
                    }
                    else
                    {
                        var duplicatecount = _HRMSContext.HR_Master_quarter.Where(t => t.MI_Id == dto.MI_Id && t.HRMQ_Id == dto.HRMQ_Id).Count();
                        if (duplicatecount == 0)
                        {
                            dmoObj.HRMQ_ActiveFlg = true;
                            dmoObj.HRMQ_QuarterName = dto.HRMQ_QuarterName;
                            dmoObj.HRMQ_ToDay = dto.HRMQ_ToDay;
                            dmoObj.HRMQ_FromDay = dto.HRMQ_FromDay;
                            dmoObj.HRMQ_FromMonth = dto.HRMQ_FromMonth;

                            dmoObj.MI_Id = dto.MI_Id;
                            dmoObj.HRMQ_ToMonth = dto.HRMQ_ToMonth;

                            _HRMSContext.Add(dmoObj);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag == 1)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                            var result = _HRMSContext.HR_Master_quarter.Single(t => t.HRMQ_Id == dmoObj.HRMQ_Id);

                            HR_Master_Quarter_Month dmoObjs = Mapper.Map<HR_Master_Quarter_Month>(dto);
                            dmoObjs.HRMQ_Id = result.HRMQ_Id;
                            dmoObjs.HRMQM_ActiveFlg = result.HRMQ_ActiveFlg;
                            dmoObjs.HRMQM_CreatedBy = dto.LogInUserId;
                            dmoObjs.HRMQM_UpdatedBy = dto.LogInUserId;
                            _HRMSContext.Add(dmoObjs);
                            var flags = _HRMSContext.SaveChanges();
                            if (flags == 1)
                            {
                                dto.retrunMsg = "Add";
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }


                        }
                        else
                        {
                            dto.retrunMsg = "Duplicate";
                        }
                    }
                }
                else
                {
                    dto.retrunMsg = "Duplicate";
                }



                dto = GetAllDropdownAndDatatableDetails(dto);
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        public HR_Master_QuarterDTO editData(int id)
        {

            HR_Master_QuarterDTO dto = new HR_Master_QuarterDTO();
            dto.retrunMsg = "";
            try
            {
               
                var lorg = _HRMSContext.HR_Master_quarter.Where(t => t.HRMQ_Id.Equals(id)).ToList();

                dto.bankdetailList = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Master_QuarterDTO deactivate(HR_Master_QuarterDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRMQ_Id > 0)
                {
                    var result = _HRMSContext.HR_Master_quarter.Single(t => t.HRMQ_Id == dto.HRMQ_Id);

                    if (result.HRMQ_ActiveFlg == true)
                    {
                        result.HRMQ_ActiveFlg = false;
                    }
                    else if (result.HRMQ_ActiveFlg == false)
                    {
                        result.HRMQ_ActiveFlg = true;
                    }
                    //result.UpdatedDate = DateTime.Now;

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRMQ_ActiveFlg == true)
                        {

                            dto.retrunMsg = "Activated";
                        }
                        else
                        {
                            dto.retrunMsg = "Deactivated";
                        }
                    }
                    else
                    {
                        dto.retrunMsg = "Record Not Activated/Deactivated";
                    }

                    dto = GetAllDropdownAndDatatableDetails(dto);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Master_QuarterDTO GetAllDropdownAndDatatableDetails(HR_Master_QuarterDTO dto)
        {
            List<Month> Monthlist = new List<Month>();
            List<HR_Master_Quarter> datalist = new List<HR_Master_Quarter>();
            try
            {
             datalist = _HRMSContext.HR_Master_quarter.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();

                dto.bankdetailList = datalist.ToArray();

                dto.leaveyeardropdown = _HRMSContext.IVRM_Master_FinancialYear.ToArray();

                Monthlist = _Context.month.Where(t => t.Is_Active == true).ToList();
                dto.monthdropdown = Monthlist.ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }
    }
}
