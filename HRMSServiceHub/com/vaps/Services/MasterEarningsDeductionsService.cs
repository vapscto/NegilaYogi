using AutoMapper;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Services
{
    public class MasterEarningsDeductionsService : Interfaces.MasterEarningsDeductionsInterface
    {
        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        public MasterEarningsDeductionsService(HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
        {
            _HRMSContext = HRMSContext;
            _Context = Context;

        }

        public HR_Master_EarningsDeductionsDTO getBasicData(HR_Master_EarningsDeductionsDTO dto)
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
        //Onchange 
        //Added by Ramesh
        public HR_Master_EarningsDeductionsDTO changeorderData(HR_Master_EarningsDeductionsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.EarningsDeductionsDTO.Count() > 0)
                {
                    foreach (HR_Master_EarningsDeductionsDTO mob in dto.EarningsDeductionsDTO)
                    {
                        if (mob.HRMED_Id > 0)
                        {
                            var result = _HRMSContext.HR_Master_EarningsDeductions.Single(t => t.HRMED_Id.Equals(mob.HRMED_Id));
                            Mapper.Map(mob, result);
                            _HRMSContext.Update(result);
                            _HRMSContext.SaveChanges();
                        }
                    }
                    dto.retrunMsg = "Order Updated sucessfully";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured"; ;
            }
            return dto;
        }
        //type
        public HR_Master_EarningsDeductions_TypeDTO getBasicDatatype(HR_Master_EarningsDeductions_TypeDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                // dto = GetAllDropdownAndDatatableDetails(dto);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        //type
        public HR_Master_EarningsDeductions_TypeDTO SaveUpdatetype(HR_Master_EarningsDeductions_TypeDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                HR_Master_EarningsDeductions_Type dmoObj = Mapper.Map<HR_Master_EarningsDeductions_Type>(dto);

                var duplicateresult = _HRMSContext.HR_Master_EarningsDeductions_Type.Where(t => t.HRMEDT_EarnDedType.Equals(dto.HRMEDT_EarnDedType)).Count();
                if (duplicateresult == 0)
                {
                    if (dmoObj.HRMEDT_Id > 0)
                    {

                        var result = _HRMSContext.HR_Master_EarningsDeductions_Type.Single(t => t.HRMEDT_Id == dmoObj.HRMEDT_Id);

                        dto.UpdatedDate = DateTime.Now;
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

                        dmoObj.HRMEDT_ActiveFlag = true;
                        dmoObj.UpdatedDate = DateTime.Now;
                        dmoObj.CreatedDate = DateTime.Now;
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
                    }

                }
                else
                {
                    dto.retrunMsg = "Duplicate";
                    return dto;
                }

               


                // dto = GetAllDropdownAndDatatableDetailsType(dto);
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Master_EarningsDeductionsDTO SaveUpdate(HR_Master_EarningsDeductionsDTO dto)
        {
            dto.retrunMsg = "";
            int lorg = 0, countchild = 0;
            try
            {
                HR_Master_EarningsDeductions dmoObj = Mapper.Map<HR_Master_EarningsDeductions>(dto);

                var duplicateresult = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id == dto.MI_Id && t.HRMED_Name.Equals(dto.HRMED_Name) && 
                                        t.HRMED_EDTypeFlag.Equals(dto.HRMED_EDTypeFlag) && t.HRMED_RoundOffFlag.Equals(dto.HRMED_RoundOffFlag) &&
                                        t.HRMED_EarnDedFlag.Equals(dto.HRMED_EarnDedFlag) && t.HRMED_AmountPercentFlag.Equals(dto.HRMED_AmountPercentFlag) && t.HRMED_AmountPercent.Equals(dto.HRMED_AmountPercent) && t.HRMED_ReviseFlg.Equals(dto.HRMED_ReviseFlg)).ToList();

                if (dto.HRMED_AmountPercentFlag =="Percentage")
                {
                    foreach (HR_Master_EarningsDeductionsPerDTO ph in dto.perc_OfDTO)
                    {
                        lorg = _HRMSContext.HR_Master_EarningsDeductionsPer.Where(t => t.HRMED_Id.Equals(dto.HRMED_Id) && t.HRMEDP_HRMED_Id== ph.HRMEDP_HRMED_Id && t.MI_Id == dto.MI_Id).Count();
                        if (lorg > 0)
                        {
                            countchild += 1;
                        }
                    }

                }

               
                if (duplicateresult.Count > 0 && countchild == dto.perc_OfDTO.Length)
                {
                    dto.retrunMsg = "AllDuplicate";
                    return dto;
                }
                else
                {

                if (dmoObj.HRMED_Id > 0)
                {
                        var duplicateName = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id == dto.MI_Id && t.HRMED_Name.Equals(dto.HRMED_Name) && t.HRMED_Name != dto.HRMED_Name).Count();
                        if (duplicateName == 0)
                        {
                            var result = _HRMSContext.HR_Master_EarningsDeductions.Single(t => t.HRMED_Id == dmoObj.HRMED_Id);

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
                            return dto;
                        }

                }
                else
                {
                        var duplicateName = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id == dto.MI_Id && t.HRMED_Name.Equals(dto.HRMED_Name)).Count();
                        if (duplicateName == 0)
                        {
                            dmoObj.HRMED_ActiveFlag = true;
                            _HRMSContext.Add(dmoObj);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag == 1)
                            {
                                dto.HRMED_Id = dmoObj.HRMED_Id;
                                dto.retrunMsg = "Add";


                                HR_Master_EarningsDeductionsDTO DTO = Mapper.Map<HR_Master_EarningsDeductionsDTO>(dmoObj);
                                DTO.HRMED_Order = DTO.HRMED_Id;
                                var result = _HRMSContext.HR_Master_EarningsDeductions.Single(t => t.HRMED_Id == DTO.HRMED_Id);
                                Mapper.Map(DTO, result);
                                _HRMSContext.Update(result);
                                _HRMSContext.SaveChanges();
                            }
                            else
                            {
                                dto.retrunMsg = "false";
                            }
                        }
                        else
                        {
                            dto.retrunMsg = "Duplicate";
                            return dto;
                        }
                }
                if (dto.HRMED_Id > 0)
                {
                    dto = AddUpdatePercentOffData(dto);
                }

                dto = GetAllDropdownAndDatatableDetails(dto);
                }
            }

            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Master_EarningsDeductionsDTO AddUpdatePercentOffData(HR_Master_EarningsDeductionsDTO dto)
        {
            try
            {
                if (dto.perc_OfDTO.Count() > 0)
                {

                    //Delete Existing Record 
                    List<HR_Master_EarningsDeductionsPer> lorg = new List<HR_Master_EarningsDeductionsPer>();
                    lorg = _HRMSContext.HR_Master_EarningsDeductionsPer.Where(t => t.HRMED_Id.Equals(dto.HRMED_Id) && t.MI_Id == dto.MI_Id).ToList();

                    foreach (HR_Master_EarningsDeductionsPer ph1 in lorg)
                    {

                        _HRMSContext.Remove(ph1);

                    }
                    _HRMSContext.SaveChanges();


                    if (dto.HRMED_AmountPercentFlag.Equals("Percentage"))
                    {

                        foreach (HR_Master_EarningsDeductionsPerDTO ph in dto.perc_OfDTO)
                        {
                            HR_Master_EarningsDeductionsPerDTO finelDto = new HR_Master_EarningsDeductionsPerDTO();

                            finelDto.MI_Id = dto.MI_Id;
                            finelDto.HRMED_Id = dto.HRMED_Id;
                            finelDto.HRMEDP_HRMED_Id = ph.HRMEDP_HRMED_Id;

                            HR_Master_EarningsDeductionsPer dmoObj = Mapper.Map<HR_Master_EarningsDeductionsPer>(finelDto);

                            _HRMSContext.Add(dmoObj);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag == 1)
                            {
                            }
                            else
                            {
                            }
                        }

                    }

                }

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return dto;
        }

        public HR_Master_EarningsDeductionsDTO editData(int id)
        {

            HR_Master_EarningsDeductionsDTO dto = new HR_Master_EarningsDeductionsDTO();
            dto.retrunMsg = "";
            try
            {
                List<HR_Master_EarningsDeductions> lorg = new List<HR_Master_EarningsDeductions>();
                lorg = _HRMSContext.HR_Master_EarningsDeductions.AsNoTracking().Where(t => t.HRMED_Id.Equals(id)).ToList();
                dto.earningdetectionList = lorg.ToArray();


                List<HR_Master_EarningsDeductionsPer> EarningsDeductionsPer = new List<HR_Master_EarningsDeductionsPer>();
                EarningsDeductionsPer = _HRMSContext.HR_Master_EarningsDeductionsPer.AsNoTracking().Where(t => t.HRMED_Id.Equals(id)).ToList();
                dto.selectedearningdetectionList = EarningsDeductionsPer.ToArray();


            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }
        //type
        public HR_Master_EarningsDeductions_TypeDTO editDatatype(int id)
        {

            HR_Master_EarningsDeductions_TypeDTO dto = new HR_Master_EarningsDeductions_TypeDTO();
            dto.retrunMsg = "";
            try
            {
                List<HR_Master_EarningsDeductions_Type> lorg = new List<HR_Master_EarningsDeductions_Type>();
                lorg = _HRMSContext.HR_Master_EarningsDeductions_Type.AsNoTracking().Where(t => t.HRMEDT_Id.Equals(id)).ToList();
                dto.eardettypelist = lorg.ToArray();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Master_EarningsDeductionsDTO deactivate(HR_Master_EarningsDeductionsDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRMED_Id > 0)
                {
                    var result = _HRMSContext.HR_Master_EarningsDeductions.Single(t => t.HRMED_Id == dto.HRMED_Id);

                    if (result.HRMED_ActiveFlag == true)
                    {
                        result.HRMED_ActiveFlag = false;
                    }
                    else if (result.HRMED_ActiveFlag == false)
                    {
                        result.HRMED_ActiveFlag = true;
                    }

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRMED_ActiveFlag == true)
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

        //type
        public HR_Master_EarningsDeductions_TypeDTO deactivatetype(HR_Master_EarningsDeductions_TypeDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRMEDT_Id > 0)
                {
                    var result = _HRMSContext.HR_Master_EarningsDeductions_Type.Single(t => t.HRMEDT_Id == dto.HRMEDT_Id);

                    if (result.HRMEDT_ActiveFlag == true)
                    {
                        result.HRMEDT_ActiveFlag = false;
                    }
                    else if (result.HRMEDT_ActiveFlag == false)
                    {
                        result.HRMEDT_ActiveFlag = true;
                    }
                    result.UpdatedDate = DateTime.Now;

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRMEDT_ActiveFlag == true)
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

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public HR_Master_EarningsDeductionsDTO GetAllDropdownAndDatatableDetails(HR_Master_EarningsDeductionsDTO dto)
        {
            List<HR_Master_EarningsDeductions> earningdeductiondatalist = new List<HR_Master_EarningsDeductions>();

            List<HR_Master_EarningsDeductions> earningdatalist = new List<HR_Master_EarningsDeductions>();
            List<HR_Master_EarningsDeductions> deductiondatalist = new List<HR_Master_EarningsDeductions>();



            List<HR_Master_EarningsDeductions_Type> datatypelist = new List<HR_Master_EarningsDeductions_Type>();


            List<HR_Master_EarningsDeductionsDTO> DTOdatalistEarning = new List<HR_Master_EarningsDeductionsDTO>();

            List<HR_Master_EarningsDeductionsDTO> DTOdatalistDeduction = new List<HR_Master_EarningsDeductionsDTO>();

            //  List<string> percentOff = new List<string>();
            try
            {

                datatypelist = _HRMSContext.HR_Master_EarningsDeductions_Type.ToList();
                dto.eardettypelist = datatypelist.ToArray();


                datatypelist = _HRMSContext.HR_Master_EarningsDeductions_Type.Where(t => t.HRMEDT_ActiveFlag == true).ToList();
                dto.eardettypeDropdown = datatypelist.ToArray();

                earningdeductiondatalist = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id)  && t.HRMED_ActiveFlag == true).OrderBy(t=>t.HRMED_Order).ToList();
                dto.earningdetectionList = earningdeductiondatalist.ToArray();

                    //Earning list
                    earningdatalist = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_EarnDedFlag.Equals("Earning")).ToList();

                    if (earningdatalist.Count() > 0)
                    {
                        foreach (HR_Master_EarningsDeductions ph in earningdatalist)
                        {
                            HR_Master_EarningsDeductionsDTO phdto = Mapper.Map<HR_Master_EarningsDeductionsDTO>(ph);

                            if (phdto.HRMED_AmountPercentFlag == "Percentage")
                            {
                                var EarningsDeductionsPerlist = _HRMSContext.HR_Master_EarningsDeductionsPer.Where(t => t.MI_Id.Equals(phdto.MI_Id) && t.HRMED_Id == phdto.HRMED_Id).ToList();

                                if (EarningsDeductionsPerlist.Count() > 0)
                                {
                                    List<string> percentOff = new List<string>();

                                    foreach (HR_Master_EarningsDeductionsPer headername in EarningsDeductionsPerlist)
                                    {
                                        var percentOffHRMED_Name = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_Id.Equals(headername.HRMEDP_HRMED_Id)).Select(t => t.HRMED_Name).ToList();

                                        percentOff.Add(percentOffHRMED_Name.FirstOrDefault());
                                    }
                                    phdto.percentOff = String.Join(" + ", percentOff.ToArray());
                                }
                                else
                                {
                                    phdto.percentOff = "";
                                }
                            }
                            else
                            {
                                phdto.percentOff = "";
                            }

                            DTOdatalistEarning.Add(phdto);

                        }

                    }



                    dto.earningList = DTOdatalistEarning.ToArray();

                    //Deduction List
                    deductiondatalist = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_EarnDedFlag.Equals("Deduction")).ToList();


                    if (deductiondatalist.Count() > 0)
                    {
                        foreach (HR_Master_EarningsDeductions ph in deductiondatalist)
                        {
                            HR_Master_EarningsDeductionsDTO phdto = Mapper.Map<HR_Master_EarningsDeductionsDTO>(ph);

                            if (phdto.HRMED_AmountPercentFlag == "Percentage")
                            {
                                var EarningsDeductionsPerlist = _HRMSContext.HR_Master_EarningsDeductionsPer.Where(t => t.MI_Id.Equals(phdto.MI_Id) && t.HRMED_Id == phdto.HRMED_Id).ToList();

                                if (EarningsDeductionsPerlist.Count() > 0)
                                {
                                    List<string> percentOff = new List<string>();

                                    foreach (HR_Master_EarningsDeductionsPer headername in EarningsDeductionsPerlist)
                                    {
                                        var percentOffHRMED_Name = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_Id.Equals(headername.HRMEDP_HRMED_Id)).Select(t => t.HRMED_Name).ToList();

                                        percentOff.Add(percentOffHRMED_Name.FirstOrDefault());
                                    }
                                    phdto.percentOff = String.Join(" + ", percentOff.ToArray());
                                }
                                else
                                {
                                    phdto.percentOff = "";
                                }
                            }
                            else
                            {
                                phdto.percentOff = "";
                            }

                            DTOdatalistDeduction.Add(phdto);

                        }

                    }

                    dto.detectionList = DTOdatalistDeduction.ToArray();

                    //Arrear list
                    dto.arrearlist = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_EarnDedFlag.Equals("Arrear")).ToArray();
                    //Gross list
                    dto.grosslist = _HRMSContext.HR_Master_EarningsDeductions.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMED_EarnDedFlag.Equals("Gross")).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }

    }
}
