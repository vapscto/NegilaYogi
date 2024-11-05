using CollegeFeeService.com.vaps.Interfaces;
using DataAccessMsSqlServerProvider.com.vapstech.College.Fee;
using DomainModel.Model.com.vapstech.College.Fees;
using Microsoft.Extensions.Logging;
using PreadmissionDTOs.com.vaps.College.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeFeeService.com.vaps.Implementation
{
    public class CLGFeeChequeBounceImpl: CLGFeeChequeBounceInterface
    {
        public CollFeeGroupContext _CollFeeGroupContext;
        ILogger<CLGFeeChequeBounceImpl> _logbranch;
        public CLGFeeChequeBounceImpl(CollFeeGroupContext CollFeeGroupContext, ILogger<CLGFeeChequeBounceImpl> log)
        {
            _CollFeeGroupContext = CollFeeGroupContext;
            _logbranch = log;
        }
        public CLGFeeChequeBounceDTO getalldetails(CLGFeeChequeBounceDTO data)
        {
            try
            {
                data.yearlist = _CollFeeGroupContext.AcademicYear.Where(a => a.MI_Id == data.MI_Id && a.Is_Active).Distinct().OrderBy(a => a.ASMAY_Order).ToArray();
                data.courselist = _CollFeeGroupContext.MasterCourseDMO.Where(a => a.MI_Id == data.MI_Id && a.AMCO_ActiveFlag).Distinct().OrderBy(a => a.AMCO_Order).ToArray();
                data.branchlist = _CollFeeGroupContext.ClgMasterBranchDMO.Where(a => a.MI_Id == data.MI_Id && a.AMB_ActiveFlag).Distinct().OrderBy(a => a.AMB_Order).ToArray();
                data.semisterlist = _CollFeeGroupContext.CLG_Adm_Master_SemesterDMO.Where(a => a.MI_Id == data.MI_Id && a.AMSE_ActiveFlg).Distinct().OrderBy(a => a.AMSE_SEMOrder).ToArray();
                data.sectionlist = _CollFeeGroupContext.Adm_College_Master_SectionDMO.Where(a => a.MI_Id == data.MI_Id && a.ACMS_ActiveFlag).Distinct().OrderBy(a => a.ACMS_Order).ToArray();
                data.alldata = (from a in _CollFeeGroupContext.Fee_College_Cheque_BounceDMO
                                from b in _CollFeeGroupContext.Adm_Master_College_StudentDMO
                                from c in _CollFeeGroupContext.AcademicYear
                                from d in _CollFeeGroupContext.Fee_Y_PaymentDMO
                                where (a.MI_Id == data.MI_Id && a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == c.ASMAY_Id && a.FYP_Id == d.FYP_Id && d.FYP_PayModeType == "Single" && d.FYP_TransactionTypeFlag == "B" && a.User_Id == data.UserId)//&& a.ASMAY_Id == data.ASMAY_Id && b.AMCST_SOL == "S" && b.AMCST_ActiveFlag == 1 
                                select new CLGFeeChequeBounceDTO
                                {
                                    ASMAY_Year = c.ASMAY_Year,
                                    FCCB_Date = a.FCCB_Date,
                                    AMCST_Id = a.AMCST_Id,
                                    AMCST_FirstName = ((b.AMCST_FirstName == null || b.AMCST_FirstName == "" ? "" : " " + b.AMCST_FirstName) + (b.AMCST_MiddleName == null || b.AMCST_MiddleName == "" || b.AMCST_MiddleName == "0" ? "" : " " + b.AMCST_MiddleName) + (b.AMCST_LastName == null || b.AMCST_LastName == "" || b.AMCST_LastName == "0" ? "" : " " + b.AMCST_LastName)).Trim(),
                                    FYP_ReceiptNo = d.FYP_ReceiptNo,
                                    FCCB_Id = a.FCCB_Id,
                                    FCCB_Amount = a.FCCB_Amount
                                }
           ).OrderByDescending(t => t.FCCB_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("CLGFeeChequeBounce  getalldetails :" + ex.Message);
            }
            return data;
        }
        public CLGFeeChequeBounceDTO get_students(CLGFeeChequeBounceDTO data)
        {
            try
            {
                var fillstudent = (from a in _CollFeeGroupContext.Adm_Master_College_StudentDMO
                                   from b in _CollFeeGroupContext.Adm_College_Yearly_StudentDMO
                                   from c in _CollFeeGroupContext.MasterCourseDMO
                                   from d in _CollFeeGroupContext.ClgMasterBranchDMO
                                   from e in _CollFeeGroupContext.CLG_Adm_Master_SemesterDMO
                                   from f in _CollFeeGroupContext.Adm_College_Master_SectionDMO
                                   where (b.AMCO_Id == c.AMCO_Id && d.AMB_Id == b.AMB_Id && a.AMCST_Id == b.AMCST_Id && a.MI_Id == data.MI_Id && b.ASMAY_Id == data.ASMAY_Id && b.AMCO_Id == data.AMCO_Id && b.AMB_Id == data.AMB_Id && b.AMSE_Id == data.AMSE_Id && e.AMSE_Id == b.AMSE_Id && f.ACMS_Id == b.ACMS_Id && b.ACYST_ActiveFlag==1)
                                   select new CollegeFeeTransactionDTO
                                   {
                                       AMCST_Id = a.AMCST_Id,
                                      // AMCST_FirstName = a.AMCST_FirstName,
                                       AMCST_FirstName = ((a.AMCST_FirstName == null || a.AMCST_FirstName == "" ? "" : " " + a.AMCST_FirstName) + (a.AMCST_MiddleName == null || a.AMCST_MiddleName == "" || a.AMCST_MiddleName == "0" ? "" : " " + a.AMCST_MiddleName) + (a.AMCST_LastName == null || a.AMCST_LastName == "" || a.AMCST_LastName == "0" ? "" : " " + a.AMCST_LastName)).Trim(),
                                       AMCST_MiddleName = a.AMCST_MiddleName,
                                       AMCST_LastName = a.AMCST_LastName,
                                       AMCST_RegistrationNo = a.AMCST_RegistrationNo,
                                       AMCST_AdmNo = a.AMCST_AdmNo,
                                       ACYST_RollNo = b.ACYST_RollNo,
                                       AMCO_CourseName = c.AMCO_CourseName,
                                       AMB_BranchName = d.AMB_BranchName,
                                       AMSE_SEMName = e.AMSE_SEMName,
                                       ACMS_SectionName = f.ACMS_SectionName,
                                       AMCST_FatherName = a.AMCST_FatherName,
                                       AMCST_DOB = a.AMCST_DOB,
                                       AMCST_MobileNo = Convert.ToInt64(a.AMCST_MobileNo)
                                   }
               ).Distinct().OrderBy(t => t.ACYST_RollNo).ToList();
                data.studentlist = fillstudent.ToArray();
            }
            catch(Exception ex)
            {
                _logbranch.LogInformation("CLGFeeChequeBounce  get_students :" + ex.Message);
            }

            return data;
        }

        public CLGFeeChequeBounceDTO get_receipts(CLGFeeChequeBounceDTO data)
        {
            try
            {
                data.receiptlist = (from a in _CollFeeGroupContext.Fee_Y_PaymentDMO
                                    from b in _CollFeeGroupContext.Fee_Y_Payment_College_StudentDMO
                                    //from c in _CollFeeGroupContext.Fee_Y_Payment_PaymentModeDMO
                                    where (a.FYP_Id == b.FYP_Id && a.ASMAY_Id == b.ASMAY_Id && a.MI_Id == data.MI_Id && b.AMCST_Id == data.AMCST_Id && a.ASMAY_Id == data.ASMAY_Id && a.FYP_PayModeType == "Single" && a.FYP_TransactionTypeFlag=="B" && a.FYP_ChequeBounceFlag == "CL")
                                    select new CLGFeeChequeBounceDTO
                                    {
                                        FYP_Id = a.FYP_Id,
                                        FYP_ReceiptNo = a.FYP_ReceiptNo,
                                    }
               ).Distinct().OrderBy(t=>t.FYP_Id).ToArray();
            }
            catch (Exception ex)
            {
                _logbranch.LogInformation("CLGFeeChequeBounce  get_receipts :" + ex.Message);
            }

            return data;
        }
        public CLGFeeChequeBounceDTO savedata(CLGFeeChequeBounceDTO data)
        {           
            try
            {
                //if (data.FCCB_Id > 0)
                //{
                //    Fee_College_Cheque_BounceDMO fcb = _CollFeeGroupContext.Fee_College_Cheque_BounceDMO.Single(t => t.FCCB_Id == data.FCCB_Id);
                //    fcb.ASMAY_Id = data.ASMAY_Id;
                //    fcb.AMCST_Id = data.AMCST_Id;
                //    fcb.FYP_Id = data.FYP_Id;
                //    fcb.FCCB_Date = data.FCCB_Date;
                //    fcb.FCCB_Remarks = data.FCCB_Remarks;

                //    _CollFeeGroupContext.Update(fcb);

                //    var contactExists = _CollFeeGroupContext.SaveChanges();
                //    if (contactExists == 1)
                //    {
                //        data.returnval = true;
                //    }
                //    else
                //    {
                //        data.returnval = false;
                //    }
                //}
                //else
                //{
                    var result_y_pay = _CollFeeGroupContext.Fee_Y_PaymentDMO.Single(t => t.FYP_Id == data.FYP_Id);
                    var FCB_Amount = result_y_pay.FYP_TotalPaidAmount;

                    Fee_College_Cheque_BounceDMO obj = new Fee_College_Cheque_BounceDMO();

                    obj.MI_Id = data.MI_Id;
                    obj.FYP_Id = data.FYP_Id;
                    obj.FCCB_Date = data.FCCB_Date;
                    obj.AMCST_Id = data.AMCST_Id;
                    obj.ASMAY_Id = data.ASMAY_Id;
                    obj.FCCB_Amount = FCB_Amount;
                    obj.FCCB_Remarks = data.FCCB_Remarks;
                    obj.User_Id = data.UserId;

                    _CollFeeGroupContext.Add(obj);

                    result_y_pay.FYP_ChequeBounceFlag = "CB";
                    result_y_pay.UpdatedDate = DateTime.Now;
                    _CollFeeGroupContext.Update(result_y_pay);

                    var list_fma_ids = _CollFeeGroupContext.Fee_T_College_PaymentDMO.Where(t => t.FYP_Id == data.FYP_Id).ToList();
                    foreach (var r in list_fma_ids)
                    {
                        var result_stu_sta = _CollFeeGroupContext.Fee_College_Student_StatusDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMCST_Id == data.AMCST_Id && t.FCMAS_Id == r.FCMAS_Id);
                        result_stu_sta.FCSS_PaidAmount = result_stu_sta.FCSS_PaidAmount - Convert.ToInt64(r.FTCP_PaidAmount);
                        result_stu_sta.FCSS_ToBePaid = result_stu_sta.FCSS_ToBePaid + Convert.ToInt64(r.FTCP_PaidAmount);
                        result_stu_sta.FCSS_ChequeBounceFlg = true;
                        _CollFeeGroupContext.Update(result_stu_sta);
                    }
                    var contactExists = _CollFeeGroupContext.SaveChanges();
                    if (contactExists >= 1)
                    {
                        data.returnval = true;
                    }
                    else
                    {
                        data.returnval = false;
                    }
                //}
                data.alldata = (from a in _CollFeeGroupContext.Fee_College_Cheque_BounceDMO
                                from b in _CollFeeGroupContext.Adm_Master_College_StudentDMO
                                from c in _CollFeeGroupContext.AcademicYear
                                from d in _CollFeeGroupContext.Fee_Y_PaymentDMO
                                where (a.MI_Id == data.MI_Id  && a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == c.ASMAY_Id && a.FYP_Id == d.FYP_Id && d.FYP_PayModeType=="Single" && d.FYP_TransactionTypeFlag == "B" && a.User_Id == data.UserId)//&& a.ASMAY_Id == data.ASMAY_Id && b.AMCST_SOL == "S" && b.AMCST_ActiveFlag == 1 
                                select new CLGFeeChequeBounceDTO
                                {
                                    ASMAY_Year = c.ASMAY_Year,
                                    FCCB_Date = a.FCCB_Date,
                                    AMCST_Id = a.AMCST_Id,
                                    AMCST_FirstName = ((b.AMCST_FirstName == null || b.AMCST_FirstName == "" ? "" : " " + b.AMCST_FirstName) + (b.AMCST_MiddleName == null || b.AMCST_MiddleName == "" || b.AMCST_MiddleName == "0" ? "" : " " + b.AMCST_MiddleName) + (b.AMCST_LastName == null || b.AMCST_LastName == "" || b.AMCST_LastName == "0" ? "" : " " + b.AMCST_LastName)).Trim(),
                                    FYP_ReceiptNo = d.FYP_ReceiptNo,
                                    FCCB_Id = a.FCCB_Id,
                                    FCCB_Amount=a.FCCB_Amount
                                }
            ).OrderByDescending(t => t.FCCB_Id).ToArray();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public CLGFeeChequeBounceDTO DeletRecord(CLGFeeChequeBounceDTO data)
        {
            try
            {
               
                var result = _CollFeeGroupContext.Fee_College_Cheque_BounceDMO.Single(t => t.FCCB_Id == data.FCCB_Id);               

                data.FYP_Id = result.FYP_Id;
                data.ASMAY_Id = result.ASMAY_Id;
                data.AMCST_Id = result.AMCST_Id;

                var result_y_pay = _CollFeeGroupContext.Fee_Y_PaymentDMO.Single(t => t.FYP_Id == data.FYP_Id);
                result_y_pay.FYP_ChequeBounceFlag = "CL";
                result_y_pay.UpdatedDate = DateTime.Now;
                _CollFeeGroupContext.Update(result_y_pay);

                var list_fma_ids = _CollFeeGroupContext.Fee_T_College_PaymentDMO.Where(t => t.FYP_Id == data.FYP_Id).ToList();
                foreach (var r in list_fma_ids)
                {
                    var result_stu_sta = _CollFeeGroupContext.Fee_College_Student_StatusDMO.Single(t => t.MI_Id == data.MI_Id && t.ASMAY_Id == data.ASMAY_Id && t.AMCST_Id == data.AMCST_Id && t.FCMAS_Id == r.FCMAS_Id);// && t.User_Id == data.UserId

                    result_stu_sta.FCSS_PaidAmount = result_stu_sta.FCSS_PaidAmount + Convert.ToInt64(r.FTCP_PaidAmount);
                    result_stu_sta.FCSS_ToBePaid = result_stu_sta.FCSS_ToBePaid - Convert.ToInt64(r.FTCP_PaidAmount);
                    result_stu_sta.FCSS_ChequeBounceFlg = false;

                    _CollFeeGroupContext.Update(result_stu_sta);
                }
                _CollFeeGroupContext.Remove(result);

                var contactExists = _CollFeeGroupContext.SaveChanges();
                if (contactExists >= 1)
                {

                    data.returnval = true;
                }
                else
                {

                    data.returnval = false;
                }

                data.alldata = (from a in _CollFeeGroupContext.Fee_College_Cheque_BounceDMO
                                from b in _CollFeeGroupContext.Adm_Master_College_StudentDMO
                                from c in _CollFeeGroupContext.AcademicYear
                                from d in _CollFeeGroupContext.Fee_Y_PaymentDMO
                                where (a.MI_Id == data.MI_Id && a.AMCST_Id == b.AMCST_Id && a.ASMAY_Id == c.ASMAY_Id && a.FYP_Id == d.FYP_Id && d.FYP_PayModeType == "Single" && d.FYP_TransactionTypeFlag == "B" && a.User_Id == data.UserId)//&& a.ASMAY_Id == data.ASMAY_Id && b.AMCST_SOL == "S" && b.AMCST_ActiveFlag == 1 
                                select new CLGFeeChequeBounceDTO
                                {
                                    ASMAY_Year = c.ASMAY_Year,
                                    FCCB_Date = a.FCCB_Date,
                                    AMCST_Id = a.AMCST_Id,
                                    AMCST_FirstName = ((b.AMCST_FirstName == null || b.AMCST_FirstName == "" ? "" : " " + b.AMCST_FirstName) + (b.AMCST_MiddleName == null || b.AMCST_MiddleName == "" || b.AMCST_MiddleName == "0" ? "" : " " + b.AMCST_MiddleName) + (b.AMCST_LastName == null || b.AMCST_LastName == "" || b.AMCST_LastName == "0" ? "" : " " + b.AMCST_LastName)).Trim(),
                                    FYP_ReceiptNo = d.FYP_ReceiptNo,
                                    FCCB_Id = a.FCCB_Id,
                                    FCCB_Amount = a.FCCB_Amount
                                }
           ).OrderByDescending(t => t.FCCB_Id).ToArray();

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
    }
}
