using DataAccessMsSqlServerProvider;
using DomainModel.Model;
using DomainModel.Model.com.vaps.admission;
using DomainModel.Model.com.vapstech.Fee;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FeeServiceHub.com.vaps.services
{
    public class StudentFeeEnablePartialPaymentImpl : interfaces.StudentFeeEnablePartialPaymentInterface
    {
        public FeeGroupContext FeeGroupContext;

        public StudentFeeEnablePartialPaymentImpl(FeeGroupContext objDbcontext)
        {
            FeeGroupContext = objDbcontext;
        }
        public StudentFeeEnablePartialPaymentDTO GetYearList(int id)
        {

            StudentFeeEnablePartialPaymentDTO data = new StudentFeeEnablePartialPaymentDTO();
            try
            {
                data.yearlist = FeeGroupContext.AcademicYear.Where(t => t.MI_Id == id && t.Is_Active == true).Distinct().ToArray();
              //  data.sectionlist = FeeGroupContext.school_M_Section.Where(t => t.MI_Id == id).OrderBy(t => t.ASMC_Order).ToArray();
                List<School_M_Class> allclas = new List<School_M_Class>();
                allclas = FeeGroupContext.admissioncls.Where(t => t.MI_Id == id && t.ASMCL_ActiveFlag == true).ToList();
                data.fillclass = allclas.ToArray();          
                data.alldata = (from a in FeeGroupContext.FeeStudentEnablePartialPaymentDMO
                                from b in FeeGroupContext.AdmissionStudentDMO
                                from c in FeeGroupContext.AcademicYear
                                where (a.MI_Id == id && a.AMST_Id == b.AMST_Id && a.ASMAY_Id == c.ASMAY_Id)
                                select new StudentFeeEnablePartialPaymentDTO
                                {
                                    ASMAY_Year = c.ASMAY_Year,
                                    FSEPP_RemarksDate = a.FSEPP_RemarksDate,
                                    FSEPP_ActiveFlag = a.FSEPP_ActiveFlag,
                                    AMST_Id = a.AMST_Id,
                                    AMST_FirstName = ((b.AMST_FirstName == null || b.AMST_FirstName == "" ? "" : " " + b.AMST_FirstName) + (b.AMST_MiddleName == null || b.AMST_MiddleName == "" || b.AMST_MiddleName == "0" ? "" : " " + b.AMST_MiddleName) + (b.AMST_LastName == null || b.AMST_LastName == "" || b.AMST_LastName == "0" ? "" : " " + b.AMST_LastName)).Trim(),
                                    FSEPP_Id = a.FSEPP_Id,
                                    FSEPP_Remarks = a.FSEPP_Remarks
                                }).OrderByDescending(t => t.FSEPP_Id).ToArray();
            }
            catch (Exception e1)
            {
                Console.WriteLine(e1.Message);
            }
            return data;
        }
        public StudentFeeEnablePartialPaymentDTO getsection(StudentFeeEnablePartialPaymentDTO data)
        {
            List<long> GrpId = new List<long>();
            try
            {

                List<School_M_Section> section = new List<School_M_Section>();
                data.fillsection = (from a in FeeGroupContext.School_Adm_Y_StudentDMO
                                    from b in FeeGroupContext.school_M_Section
                                    where (a.ASMS_Id == b.ASMS_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && b.MI_Id == data.MI_ID)
                                    select new FeeRefundDTO
                                    {
                                        AMSC_Id = b.ASMS_Id,
                                        asmc_sectionname = b.ASMC_SectionName
                                    }
                          ).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentFeeEnablePartialPaymentDTO get_student(StudentFeeEnablePartialPaymentDTO data)
        {
            try
            {
                if (data.ASMCL_Id > 0)
                {
                    data.studentlist = (from a in FeeGroupContext.School_Adm_Y_StudentDMO
                                        from b in FeeGroupContext.AdmissionStudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && b.AMST_SOL == "S" && a.AMAY_ActiveFlag == 1 && b.AMST_ActiveFlag == 1)
                                        select new StudentFeeEnablePartialPaymentDTO
                                        {
                                            AMST_Id = a.AMST_Id,
                                            AMAY_RollNo = a.AMAY_RollNo,
                                            AMST_MiddleName = b.AMST_MiddleName,
                                            AMST_LastName = b.AMST_LastName,
                                            AMST_RegistrationNo = b.AMST_RegistrationNo,
                                            AMST_AdmNo = b.AMST_AdmNo,
                                            AMST_FirstName = ((b.AMST_FirstName == null ? " " : b.AMST_FirstName) + (b.AMST_MiddleName == null ? " " : b.AMST_MiddleName) + (b.AMST_LastName == null ? " " : b.AMST_LastName)).Trim(),
                                        }).Distinct().OrderBy(t => t.AMAY_RollNo).ToArray();
                }
                else
                {
                    data.studentlist = (from a in FeeGroupContext.School_Adm_Y_StudentDMO
                                        from b in FeeGroupContext.AdmissionStudentDMO
                                        where (a.AMST_Id == b.AMST_Id && a.ASMAY_Id == data.ASMAY_Id && a.ASMCL_Id == data.ASMCL_Id && b.AMST_SOL == "S" && a.AMAY_ActiveFlag == 1 && b.AMST_ActiveFlag == 1)
                                        select new StudentFeeEnablePartialPaymentDTO
                                        {
                                            AMST_Id = a.AMST_Id,
                                            AMAY_RollNo = a.AMAY_RollNo,
                                            AMST_FirstName=b.AMST_FirstName,
                                            AMST_MiddleName = b.AMST_MiddleName,
                                            AMST_LastName = b.AMST_LastName,
                                            AMST_RegistrationNo = b.AMST_RegistrationNo,
                                            AMST_AdmNo = b.AMST_AdmNo,
                                        }).Distinct().OrderBy(t => t.AMAY_RollNo).ToArray();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public StudentFeeEnablePartialPaymentDTO savedata(StudentFeeEnablePartialPaymentDTO data)
        {
            try
            {
                FeeStudentEnablePartialPaymentDMO obj = new FeeStudentEnablePartialPaymentDMO();

                obj.MI_Id = data.MI_ID;
                obj.FSEPP_Id = data.FSEPP_Id;
                obj.FSEPP_RemarksDate = data.FSEPP_RemarksDate;
                obj.AMST_Id = data.AMST_Id;
                obj.ASMAY_Id = data.ASMAY_Id;
                obj.FSEPP_ActiveFlag = true;
                obj.FSEPP_Remarks = data.FSEPP_Remarks;
                obj.FSEPP_CreatedDate = DateTime.Now;
                FeeGroupContext.Add(obj);

                var contactExists = FeeGroupContext.SaveChanges();
                if (contactExists >= 1)
                {
                    data.retrunMsg = "Add";
                }
                else
                {
                    data.retrunMsg = "false";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public StudentFeeEnablePartialPaymentDTO deactivate(StudentFeeEnablePartialPaymentDTO dto)
        {
            dto.retrunMsg = "";
            try
            {
                if (dto.FSEPP_Id > 0)
                {
                    var result = FeeGroupContext.FeeStudentEnablePartialPaymentDMO.Single(t => t.FSEPP_Id == dto.FSEPP_Id);

                    if (result.FSEPP_ActiveFlag == true)
                    {
                        result.FSEPP_ActiveFlag = false;
                    }
                    else if (result.FSEPP_ActiveFlag == false)
                    {
                        result.FSEPP_ActiveFlag = true;
                    }
                    FeeGroupContext.Update(result);
                    var flag = FeeGroupContext.SaveChanges();
                    if (flag > 0)
                    {
                        if (result.FSEPP_ActiveFlag == true)
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
    }
}
