using AutoMapper;
using CommonLibrary;
using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.HRMS;
using Microsoft.EntityFrameworkCore;
using PreadmissionDTOs;
using PreadmissionDTOs.com.vaps.HRMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HRMSServicesHub.com.vaps.Services
{
    public class HRMasterchapterVIService : Interfaces.MasterChapterVIInterface
    {

        public HRMSContext _HRMSContext;
        public DomainModelMsSqlServerContext _Context;
        private string[] ThouLetters = { "", "M", "MM", "MMM" };
        private string[] HundLetters =
            { "", "C", "CC", "CCC", "CD", "D", "DC", "DCC", "DCCC", "CM" };
        private string[] TensLetters =
            { "", "x", "xx", "xxx", "xl", "l", "lx", "lxx", "lxxx", "xc" };
        private string[] OnesLetters =
            { "", "i", "ii", "iii", "iv", "v", "vi", "vii", "viii", "ix" };
        public string roman = "";
        public HRMasterchapterVIService(HRMSContext HRMSContext, DomainModelMsSqlServerContext Context)
        {
            _HRMSContext = HRMSContext;
            _Context = Context;
        }

        public MasterChapterVIDTO getBasicData(MasterChapterVIDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                dto = GetAllDropdownAndDatatableDetails(dto);
                // int id = 7;
                // ArabicToRoman(id);

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }
            return dto;
        }

        private string ArabicToRoman(long arabic, string roman)
        {
            //   HR_Master_ChapterVI dmoObj = Mapper.Map<HR_Master_ChapterVI>(dto);

            MasterChapterVIDTO ddd = new MasterChapterVIDTO();
            // See if it's >= 4000.
            if (arabic >= 4000)
            {
                // Use parentheses.
                long thou = arabic / 1000;
                arabic %= 1000;
                return "(" + ArabicToRoman(thou, roman) + ")" +
                    ArabicToRoman(arabic, roman);
            }

            // Otherwise process the letters.
            string result = "";

            // Pull out thousands.
            long num;
            num = arabic / 1000;
            result += ThouLetters[num];
            arabic %= 1000;

            // Handle hundreds.
            num = arabic / 100;
            result += HundLetters[num];
            arabic %= 100;

            // Handle tens.
            num = arabic / 10;
            result += TensLetters[num];
            arabic %= 10;

            // Handle ones.
            result += OnesLetters[arabic];
            string resultdto = result;
            ddd.roman = result;

            return roman;

        }

        public MasterChapterVIDTO SaveUpdate(MasterChapterVIDTO dto)
        {
            dto.retrunMsg = "";

            try
            {
                HR_Master_ChapterVI dmoObj = Mapper.Map<HR_Master_ChapterVI>(dto);

                var alldata = _HRMSContext.HR_master_ChapterVI.Where(t => t.MI_Id == dto.MI_Id && t.HRMCVIA_SectionName.Equals(dto.HRMCVIA_SectionName) && t.HRMCVIA_MaxLimit == dto.HRMCVIA_MaxLimit).Count();
                //  var alldata1 = _HRMSContext.HR_Master_Allowance.Where(t => t.MI_Id == dto.MI_Id && t.HRMAL_MaxLimit == dto.HRMAL_MaxLimit).Count();
                long id = dto.HRMCVIA_Id;
                ArabicToRoman(id, roman);
                // dmoObj.HRMCVIA_ORDER = resultdto;
                if (alldata == 0)
                {
                    if (dmoObj.HRMCVIA_Id > 0)
                    {
                        var duplicateHRMBD_BankName = _HRMSContext.HR_master_ChapterVI.Where(t => t.MI_Id == dto.MI_Id && t.HRMCVIA_SectionName.Equals(dto.HRMCVIA_SectionName) && t.HRMCVIA_MaxLimit != dmoObj.HRMCVIA_MaxLimit && t.HRMCVIA_Id != dmoObj.HRMCVIA_Id).Count();

                        if (duplicateHRMBD_BankName == 0)
                        {
                            var result = _HRMSContext.HR_master_ChapterVI.Single(t => t.HRMCVIA_Id == dmoObj.HRMCVIA_Id);

                            dto.HRMCVIA_ActiveFlg = true;
                            Mapper.Map(dto, result);
                            _HRMSContext.Update(result);
                            var flag = _HRMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "AllDuplicate";
                            }
                        }
                    }
                    else
                    {
                        var duplicateHRMBD_BankName = _HRMSContext.HR_master_ChapterVI.Where(t => t.MI_Id == dto.MI_Id && t.HRMCVIA_SectionName.Equals(dto.HRMCVIA_SectionName)).Count();

                        var getrowcout = _HRMSContext.HR_master_ChapterVI.Where(a => a.MI_Id == dto.MI_Id && a.HRMCVIA_SectionCode == "80C").Count();
                        long coutd = getrowcout + 1;

                        // ArabicToRoman(coutd , roman);
                        //coutd;
                        long num;
                        string result = "";
                        num = coutd / 10;
                        result += TensLetters[num];
                        coutd %= 10;

                        // Handle ones.
                        result += OnesLetters[coutd];
                        string resultdto = result;
                        // ddd.roman = result;



                        if (duplicateHRMBD_BankName == 0)
                        {
                            dmoObj.HRMCVIA_SectionName = dto.HRMCVIA_SectionName;
                            dmoObj.HRMCVIA_SubSectionAplFlg = dto.HRMCVIA_SubSectionAplFlg;
                            dmoObj.HRMCVIA_MaxLimitAplFlg = dto.HRMCVIA_MaxLimitAplFlg;
                            dmoObj.HRMCVIA_SectionCode = dto.HRMCVIA_SectionCode;
                            dmoObj.HRMCVIA_PartFlg = dto.HRMCVIA_PartFlg.TrimEnd().ToString();
                            dmoObj.MI_Id = dto.MI_Id;
                            dmoObj.HRMCVIA_MaxLimit = dto.HRMCVIA_MaxLimit;
                            dmoObj.HRMCVIA_ActiveFlg = true;
                            dmoObj.HRMCVIA_CreatedBy = dto.HRMCVIA_CreatedBy;
                            dmoObj.HRMCVIA_UpdatedBy = dto.HRMCVIA_UpdatedBy;
                           dmoObj.HRMCVIA_ORDER = coutd;

                            //

                            long nums;
                            string results = "";
                            nums = coutd / 10;
                            results += TensLetters[nums];
                            coutd %= 10;

                            // Handle ones.
                            results += OnesLetters[coutd];
                            string resultdtos = results;

                            dmoObj.HRMCIA_ROMANORDER = resultdtos;

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

        public MasterChapterVIDTO editData(int id)
        {

            MasterChapterVIDTO dto = new MasterChapterVIDTO();
            dto.retrunMsg = "";
            try
            {
                List<HR_Master_ChapterVI> lorg = new List<HR_Master_ChapterVI>();
                lorg = _HRMSContext.HR_master_ChapterVI.Where(t => t.HRMCVIA_Id
        .Equals(id)).ToList();
                dto.emploanList = lorg.ToArray();

                dto = Mapper.Map<MasterChapterVIDTO>(lorg.FirstOrDefault());
                dto.emploanList = lorg.ToArray();

                var empearnDed = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                  from mas in _HRMSContext.HR_Master_EarningsDeductions
                                  from empsalsry in _HRMSContext.HR_Employee_Salary

                                  from salarydetails in _HRMSContext.HR_Employee_Salary_Details
                                  where (emp.HRMED_Id == mas.HRMED_Id && emp.HREED_ActiveFlag == true && emp.HRME_Id == dto.HRME_Id && mas.HRMED_EarnDedFlag == "Deduction" && mas.HRMED_EDTypeFlag == "IT" && emp.HRME_Id == empsalsry.HRME_Id && salarydetails.HRES_Id == empsalsry.HRES_Id)
                                  select new HR_Employee_Salary_DetailsDTO
                                  {

                                      HRMED_Id = emp.HRMED_Id,
                                      HRMED_Name = mas.HRMED_Name,
                                      HRESD_Amount = emp.HREED_Amount,
                                      HRMED_EarnDedFlag = mas.HRMED_EarnDedFlag
                                  }).ToList();

                dto.empGrossSal = Convert.ToDecimal(empearnDed.Sum(t => t.HRESD_Amount));
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                dto.retrunMsg = "Error occured";
            }

            return dto;
        }

        public MasterChapterVIDTO getDetailsByEmployee(MasterChapterVIDTO dto)
        {
            dto.empGrossSal = 0;
            // dto.totalAppliedAmount = 0;


            try
            {
                //  var empearnDed = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                //                    from mas in _HRMSContext.HR_Master_EarningsDeductions
                //                    where (emp.HRMED_Id == mas.HRMED_Id && emp.HREED_ActiveFlag == true && emp.HRME_Id == dto.HRME_Id && mas.HRMED_EarnDedFlag == "Earning")
                //                    select new HR_Employee_Salary_DetailsDTO
                //                        {

                //                        HRMED_Id = emp.HRMED_Id,
                //                        HRMED_Name = mas.HRMED_Name,
                //                        HRESD_Amount = emp.HREED_Amount,
                //                        HRMED_EarnDedFlag = mas.HRMED_EarnDedFlag
                //                        }).ToList();

                //  dto.empGrossSal = Convert.ToDecimal(empearnDed.Sum(t => t.HRESD_Amount));
                ////  GetTotalAppliedAmount(dto);




            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }


            return dto;
        }


        public MasterChapterVIDTO deactivate(MasterChapterVIDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.HRMCVIA_Id
 > 0)
                {
                    var result = _HRMSContext.HR_master_ChapterVI.Single(t => t.HRMCVIA_Id
 == dto.HRMCVIA_Id
);

                    if (result.HRMCVIA_ActiveFlg
 == true)
                    {
                        result.HRMCVIA_ActiveFlg
 = false;
                    }
                    else if (result.HRMCVIA_ActiveFlg
 == false)
                    {
                        result.HRMCVIA_ActiveFlg
 = true;
                    }
                    //result.UpdatedDate = DateTime.Now;

                    _HRMSContext.Update(result);
                    var flag = _HRMSContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.HRMCVIA_ActiveFlg
 == true)
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

        public MasterChapterVIDTO validateordernumber(MasterChapterVIDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.MasterChapterVIDTOO.Count() > 0)
                {
                    foreach (MasterChapterVIDTO mob in dto.MasterChapterVIDTOO)
                    {
                        if (mob.HRMCVIA_Id > 0)
                        {
                            var result = _HRMSContext.HR_master_ChapterVI.Single(t => t.HRMCVIA_Id.Equals(mob.HRMCVIA_Id));
                            // ArabicToRoman(coutd , roman);
                            long num;
                            string strresult = "";
                            num = Convert.ToInt32(result.HRMCVIA_ORDER) / 10;
                            strresult += TensLetters[num];
                            result.HRMCVIA_ORDER = (Convert.ToInt32(result.HRMCVIA_ORDER) % 10);
                            strresult += OnesLetters[Convert.ToInt32(result.HRMCVIA_ORDER)];
                            string resultdto = strresult;
                            // ddd.roman = result;
                            result.HRMCIA_ROMANORDER = resultdto;
                            Mapper.Map(mob, result);
                            _HRMSContext.Update(result);
                           // _HRMSContext.SaveChanges();
                            var flag = _HRMSContext.SaveChanges();
                            if (flag > 0)
                            {
                                dto.retrunMsg = "Update";
                            }
                            else
                            {
                                dto.retrunMsg = "AllDuplicate";
                            }
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
        public MasterChapterVIDTO GetAllDropdownAndDatatableDetails(MasterChapterVIDTO dto)
        {
            List<HR_Master_ChapterVI> datalist = new List<HR_Master_ChapterVI>();

            List<MasterEmployee> employe = new List<MasterEmployee>();
            List<HRMasterLoan> masteloan = new List<HRMasterLoan>();
            try
            {

                //var IVRM_ModeOfPayment = _HRMSContext.IVRM_ModeOfPayment.Where(t => t.IVRMMOD_ActiveFlag == true).ToList();
                //dto.modeOfPaymentdropdown = IVRM_ModeOfPayment.ToArray();

                //HR_Configuration PayrollStandard = _HRMSContext.HR_Configuration.FirstOrDefault(t => t.MI_Id.Equals(dto.MI_Id));

                //HR_ConfigurationDTO dmoObj = Mapper.Map<HR_ConfigurationDTO>(PayrollStandard);
                //dto.configurationDetails = dmoObj;


                var employees = (from emp in _HRMSContext.HR_Employee_EarningsDeductions
                                 from mm in _HRMSContext.MasterEmployee
                                 from med in _HRMSContext.HR_Master_EarningsDeductions
                                 where mm.MI_Id.Equals(dto.MI_Id)
                                 && emp.HRMED_Id == med.HRMED_Id

                                 && mm.HRME_ActiveFlag == true && emp.HRME_Id == mm.HRME_Id
                                 orderby mm.HRME_EmployeeOrder
                                 select mm).Distinct().OrderBy(t => t.HRME_EmployeeOrder).ToList();



                dto.employeedropdown = employees.ToArray();
                dto.leaveyeardropdown = _HRMSContext.IVRM_Master_FinancialYear.OrderBy(t => t.IMFY_OrderBy).ToArray();

                // dto.leaveyeardropdown = _HRMSContext.IVRM_Master_FinancialYear.ToArray();
                if (employees.Count() > 0)
                {

                    var empIds = employees.Select(t => t.HRME_Id);

                    datalist = _HRMSContext.HR_master_ChapterVI.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
                    dto.emploanList = datalist.ToArray();
                }

                dto.allowance = _HRMSContext.HR_Master_Allowance.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToArray();

                //masteloan = _HRMSContext.HRMasterLoan.Where(t => t.MI_Id.Equals(dto.MI_Id) && t.HRMLN_ActiveFlag == true).ToList();
                //dto.masterloandropdown = masteloan.ToArray();


                var ordering = _HRMSContext.HR_master_ChapterVI.Where(t => t.MI_Id.Equals(dto.MI_Id)).ToList();
                dto.ordrlist = ordering.ToArray();


            }


            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return dto;
        }


    }
}
