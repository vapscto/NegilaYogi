using DataAccessMsSqlServerProvider.FeedBack;
using DomainModel.Model.FeedBack;
using PreadmissionDTOs.FeedBack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.FeedBack.Services
{
    public class FeedBackMasterTypeImpl : Interface.FeedBackMasterTypeInterface
    {
        public FeedBackContext _context;

        // Feedback Master Type
        public FeedBackMasterTypeImpl(FeedBackContext context)
        {
            _context = context;
        }
        public FeedBackMasterTypeDTO getdetails(FeedBackMasterTypeDTO data)
        {
            try
            {
                data.getdetails = _context.FeedBackMasterTypeDMO.Where(a => a.MI_Id == data.MI_Id).OrderBy(a => a.FMTY_FTOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeedBackMasterTypeDTO savedata(FeedBackMasterTypeDTO data)
        {
            try
            {
                if (data.FMTY_Id > 0)
                {
                    var checkname = _context.FeedBackMasterTypeDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id != data.FMTY_Id && a.FMTY_FeedbackTypeName == data.FMTY_FeedbackTypeName).ToList();
                    if (checkname.Count() > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        var checknamed = _context.FeedBackMasterTypeDMO.Single(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id);
                        checknamed.FMTY_FeedbackTypeRemarks = data.FMTY_FeedbackTypeRemarks;
                        checknamed.FMTY_FeedbackTypeName = data.FMTY_FeedbackTypeName;
                        checknamed.FMTY_QuestionwiseOptionFlg = data.FMTY_QuestionwiseOptionFlg;
                        if (data.FMTY_StakeHolderFlag == "Student")
                        {
                            checknamed.FMTY_SubjectSpecificFlag = data.FMTY_SubjectSpecificFlag;
                        }

                        if (data.FMTY_StakeHolderFlag == "Student")
                        {
                            checknamed.FMTY_NOFPerYearByStudent = data.nooftimesfeedback;
                            checknamed.FMTY_NOFPerYearByStaff = 0;
                            checknamed.FMTY_NOFPerYearByParent = 0;
                            checknamed.FMTY_NOFPerYearByAlumni = 0;
                            checknamed.FMTY_StudentFlag = true;
                            checknamed.FMTY_StaffFlag = false;
                            checknamed.FMTY_ParentFlag = false;
                            checknamed.FMTY_AlumniFlag = false;
                        }
                        else if (data.FMTY_StakeHolderFlag == "Parents")
                        {
                            checknamed.FMTY_NOFPerYearByStudent = 0;
                            checknamed.FMTY_NOFPerYearByStaff = 0;
                            checknamed.FMTY_NOFPerYearByParent = data.nooftimesfeedback;
                            checknamed.FMTY_NOFPerYearByAlumni = 0;
                            checknamed.FMTY_StudentFlag = false;
                            checknamed.FMTY_StaffFlag = false;
                            checknamed.FMTY_ParentFlag = true;
                            checknamed.FMTY_AlumniFlag = false;
                        }
                        else if (data.FMTY_StakeHolderFlag == "Alumni")
                        {
                            checknamed.FMTY_NOFPerYearByStudent = 0;
                            checknamed.FMTY_NOFPerYearByStaff = 0;
                            checknamed.FMTY_NOFPerYearByParent = 0;
                            checknamed.FMTY_NOFPerYearByAlumni = data.nooftimesfeedback;
                            checknamed.FMTY_StudentFlag = false;
                            checknamed.FMTY_StaffFlag = false;
                            checknamed.FMTY_ParentFlag = false;
                            checknamed.FMTY_AlumniFlag = true;
                        }
                        else if (data.FMTY_StakeHolderFlag == "Staff")
                        {
                            checknamed.FMTY_NOFPerYearByStudent = 0;
                            checknamed.FMTY_NOFPerYearByStaff = data.nooftimesfeedback;
                            checknamed.FMTY_NOFPerYearByParent = 0;
                            checknamed.FMTY_NOFPerYearByAlumni = 0;
                            checknamed.FMTY_StudentFlag = false;
                            checknamed.FMTY_StaffFlag = true;
                            checknamed.FMTY_ParentFlag = false;
                            checknamed.FMTY_AlumniFlag = false;
                        }

                        checknamed.FMTY_UpdatedBy = data.userid;
                        checknamed.UpdatedDate = DateTime.Now;
                        _context.Update(checknamed);
                        int i = _context.SaveChanges();
                        if (i > 0)
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
                    var checkname = _context.FeedBackMasterTypeDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_FeedbackTypeName == data.FMTY_FeedbackTypeName).ToList();
                    if (checkname.Count() > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        var getrowcount = _context.FeedBackMasterTypeDMO.Where(a => a.MI_Id == data.MI_Id).Count();

                        int rowucount = getrowcount + 1;

                        FeedBackMasterTypeDMO dmo = new FeedBackMasterTypeDMO();
                        dmo.MI_Id = data.MI_Id;
                        dmo.FMTY_FeedbackTypeName = data.FMTY_FeedbackTypeName;
                        dmo.FMTY_FeedbackTypeRemarks = data.FMTY_FeedbackTypeRemarks;
                        dmo.FMTY_StakeHolderFlag = data.FMTY_StakeHolderFlag;
                        dmo.FMTY_SubjectSpecificFlag = data.FMTY_SubjectSpecificFlag;
                        dmo.FMTY_QuestionwiseOptionFlg = data.FMTY_QuestionwiseOptionFlg;
                        dmo.FMTY_FTOrder = rowucount;

                        if (data.FMTY_StakeHolderFlag == "Student")
                        {
                            dmo.FMTY_NOFPerYearByStudent = data.nooftimesfeedback;
                            dmo.FMTY_NOFPerYearByStaff = 0;
                            dmo.FMTY_NOFPerYearByParent = 0;
                            dmo.FMTY_NOFPerYearByAlumni = 0;
                            dmo.FMTY_StudentFlag = true;
                            dmo.FMTY_StaffFlag = false;
                            dmo.FMTY_ParentFlag = false;
                            dmo.FMTY_AlumniFlag = false;
                        }
                        else if (data.FMTY_StakeHolderFlag == "Parents")
                        {
                            dmo.FMTY_NOFPerYearByStudent = 0;
                            dmo.FMTY_NOFPerYearByStaff = 0;
                            dmo.FMTY_NOFPerYearByParent = data.nooftimesfeedback;
                            dmo.FMTY_NOFPerYearByAlumni = 0;
                            dmo.FMTY_StudentFlag = false;
                            dmo.FMTY_StaffFlag = false;
                            dmo.FMTY_ParentFlag = true;
                            dmo.FMTY_AlumniFlag = false;
                        }
                        else if (data.FMTY_StakeHolderFlag == "Alumni")
                        {
                            dmo.FMTY_NOFPerYearByStudent = 0;
                            dmo.FMTY_NOFPerYearByStaff = 0;
                            dmo.FMTY_NOFPerYearByParent = 0;
                            dmo.FMTY_NOFPerYearByAlumni = data.nooftimesfeedback;
                            dmo.FMTY_StudentFlag = false;
                            dmo.FMTY_StaffFlag = false;
                            dmo.FMTY_ParentFlag = false;
                            dmo.FMTY_AlumniFlag = true;
                        }
                        else if (data.FMTY_StakeHolderFlag == "Staff")
                        {
                            dmo.FMTY_NOFPerYearByStudent = 0;
                            dmo.FMTY_NOFPerYearByStaff = data.nooftimesfeedback;
                            dmo.FMTY_NOFPerYearByParent = 0;
                            dmo.FMTY_NOFPerYearByAlumni = 0;
                            dmo.FMTY_StudentFlag = false;
                            dmo.FMTY_StaffFlag = true;
                            dmo.FMTY_ParentFlag = false;
                            dmo.FMTY_AlumniFlag = false;
                        }


                        dmo.FMTY_CreatedBy = data.userid;
                        dmo.CreatedDate = DateTime.Now;
                        dmo.FMTY_UpdatedBy = data.userid;
                        dmo.UpdatedDate = DateTime.Now;
                        dmo.FMTY_ActiveFlag = true;
                        _context.Add(dmo);
                        int i = _context.SaveChanges();
                        if (i > 0)
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
                Console.WriteLine(ex.Message);
                data.returnval = false;
            }
            return data;
        }
        public FeedBackMasterTypeDTO editdata(FeedBackMasterTypeDTO data)
        {
            try
            {
                data.editdetails = _context.FeedBackMasterTypeDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeedBackMasterTypeDTO activedeactive(FeedBackMasterTypeDTO data)
        {
            try
            {
                var result12 = _context.FeedBackMasterTypeDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).ToList();
                if (result12.FirstOrDefault().FMTY_ActiveFlag == true)
                {
                    var checkidused = _context.Feedback_Type_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).Count();
                    var checkidused1 = _context.Feedback_Type_OptionsDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).Count();
                    var checkidused2 = _context.Feedback_College_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).Count();
                    var checkidused3 = _context.Feedback_School_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).Count();
                    var checkidused4 = _context.Feedback_Staff_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).Count();
                    var checkidused5 = _context.Feedback_Alumni_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).Count();
                    var checkidused6 = _context.Feedback_College_StudentToStaffDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).Count();
                    var checkidused7 = _context.Feedback_School_StudentToStaffDMO.Where(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id).Count();

                    int k = checkidused + checkidused1 + checkidused2 + checkidused3 + checkidused4 + checkidused5 + checkidused6 + checkidused7;

                    if (k > 0)
                    {
                        data.message = "Mapped";
                    }
                    else
                    {
                        var result = _context.FeedBackMasterTypeDMO.Single(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id);
                        if (result.FMTY_ActiveFlag == true)
                        {
                            result.FMTY_ActiveFlag = false;
                        }
                        else
                        {
                            result.FMTY_ActiveFlag = true;
                        }
                        result.UpdatedDate = DateTime.Now;
                        result.FMTY_UpdatedBy = data.userid;
                        _context.Update(result);
                        int i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                    var result = _context.FeedBackMasterTypeDMO.Single(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.FMTY_Id);
                    if (result.FMTY_ActiveFlag == true)
                    {
                        result.FMTY_ActiveFlag = false;
                    }
                    else
                    {
                        result.FMTY_ActiveFlag = true;
                    }
                    result.UpdatedDate = DateTime.Now;
                    result.FMTY_UpdatedBy = data.userid;
                    _context.Update(result);
                    int i = _context.SaveChanges();
                    if (i > 0)
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
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public FeedBackMasterTypeDTO getOrder(FeedBackMasterTypeDTO data)
        {
            try
            {
                int i = 0;
                if (data.Type_Master_TempDTO.Length > 0)
                {
                    for (int k = 0; k < data.Type_Master_TempDTO.Length; k++)
                    {
                        i = i + 1;
                        var result = _context.FeedBackMasterTypeDMO.Single(a => a.MI_Id == data.MI_Id && a.FMTY_Id == data.Type_Master_TempDTO[k].FMTY_Id);
                        result.FMTY_FTOrder = i;
                        result.UpdatedDate = DateTime.Now;
                        result.FMTY_UpdatedBy = data.userid;
                        _context.Update(result);
                    }
                    int kl = _context.SaveChanges();
                    if (kl > 0)
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
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Feedback Master Questions
        public Feedback_Master_QuestionDTO getquestiondetails(Feedback_Master_QuestionDTO data)
        {
            try
            {
                data.getdetails = _context.Feedback_Master_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id).OrderBy(a => a.FMQE_FQOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Feedback_Master_QuestionDTO questionssavedata(Feedback_Master_QuestionDTO data)
        {
            try
            {
                if (data.FMQE_Id > 0)
                {
                    var checkname = _context.Feedback_Master_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id && a.FMQE_Id != data.FMQE_Id && a.FMQE_FeedbackQuestions == data.FMQE_FeedbackQuestions).ToList();
                    if (checkname.Count() > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        var checknamed = _context.Feedback_Master_QuestionsDMO.Single(a => a.MI_Id == data.MI_Id && a.FMQE_Id == data.FMQE_Id);
                        checknamed.FMQE_FeedbackQRemarks = data.FMQE_FeedbackQRemarks;
                        checknamed.FMQE_FeedbackQuestions = data.FMQE_FeedbackQuestions;
                        checknamed.FMQE_UpdatedBy = data.userid;
                        checknamed.FMQE_ManualEntryFlg = data.FMQE_ManualEntryFlg;
                        checknamed.UpdatedDate = DateTime.Now;
                        _context.Update(checknamed);
                        int i = _context.SaveChanges();
                        if (i > 0)
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
                    var checkname = _context.Feedback_Master_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id && a.FMQE_FeedbackQuestions == data.FMQE_FeedbackQuestions).ToList();
                    if (checkname.Count() > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        var getrowcount = _context.Feedback_Master_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id).Count();

                        int rowucount = getrowcount + 1;

                        Feedback_Master_QuestionsDMO dmo = new Feedback_Master_QuestionsDMO();
                        dmo.MI_Id = data.MI_Id;
                        dmo.FMQE_FeedbackQuestions = data.FMQE_FeedbackQuestions;
                        dmo.FMQE_FeedbackQRemarks = data.FMQE_FeedbackQRemarks;
                        dmo.FMQE_FQOrder = rowucount;
                        dmo.FMQE_CreatedBy = data.userid;
                        dmo.CreatedDate = DateTime.Now;
                        dmo.FMQE_UpdatedBy = data.userid;
                        dmo.UpdatedDate = DateTime.Now;
                        dmo.FMQE_ActiveFlag = true;
                        dmo.FMQE_ManualEntryFlg = data.FMQE_ManualEntryFlg;
                        _context.Add(dmo);
                        int i = _context.SaveChanges();
                        if (i > 0)
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
                Console.WriteLine(ex.Message);
                data.returnval = false;
            }
            return data;
        }
        public Feedback_Master_QuestionDTO questionseditdata(Feedback_Master_QuestionDTO data)
        {
            try
            {

                data.editdetails = _context.Feedback_Master_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id && a.FMQE_Id == data.FMQE_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Feedback_Master_QuestionDTO questionsactivedeactive(Feedback_Master_QuestionDTO data)
        {
            try
            {
                var result12 = _context.Feedback_Master_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id && a.FMQE_Id == data.FMQE_Id).ToList();
                if (result12.FirstOrDefault().FMQE_ActiveFlag == true)
                {
                    var checkidused = _context.Feedback_Type_QuestionsDMO.Where(a => a.MI_Id == data.MI_Id && a.FMQE_Id == data.FMQE_Id).Count();
                    // var checkidused1 = _context.Feedback_Type_OptionsDMO.Where(a => a.MI_Id == data.MI_Id && a.FMQE_Id == data.FMQE_Id).Count();
                    var checkidused2 = _context.Feedback_College_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMQE_Id == data.FMQE_Id).Count();
                    var checkidused3 = _context.Feedback_School_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMQE_Id == data.FMQE_Id).Count();
                    var checkidused4 = _context.Feedback_Staff_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMQE_Id == data.FMQE_Id).Count();
                    var checkidused5 = _context.Feedback_Alumni_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMQE_Id == data.FMQE_Id).Count();
                    var checkidused6 = _context.Feedback_College_StudentToStaffDMO.Where(a => a.MI_Id == data.MI_Id && a.FMQE_Id == data.FMQE_Id).Count();
                    var checkidused7 = _context.Feedback_School_StudentToStaffDMO.Where(a => a.MI_Id == data.MI_Id && a.FMQE_Id == data.FMQE_Id).Count();

                    int k = checkidused + checkidused2 + checkidused3 + checkidused4 + checkidused5 + checkidused6 + checkidused7;

                    if (k > 0)
                    {
                        data.message = "Mapped";
                    }
                    else
                    {
                        var result = _context.Feedback_Master_QuestionsDMO.Single(a => a.MI_Id == data.MI_Id && a.FMQE_Id == data.FMQE_Id);
                        if (result.FMQE_ActiveFlag == true)
                        {
                            result.FMQE_ActiveFlag = false;
                        }
                        else
                        {
                            result.FMQE_ActiveFlag = true;
                        }
                        result.UpdatedDate = DateTime.Now;
                        result.FMQE_UpdatedBy = data.userid;
                        _context.Update(result);
                        int i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                    var result = _context.Feedback_Master_QuestionsDMO.Single(a => a.MI_Id == data.MI_Id && a.FMQE_Id == data.FMQE_Id);
                    if (result.FMQE_ActiveFlag == true)
                    {
                        result.FMQE_ActiveFlag = false;
                    }
                    else
                    {
                        result.FMQE_ActiveFlag = true;
                    }
                    result.UpdatedDate = DateTime.Now;
                    result.FMQE_UpdatedBy = data.userid;
                    _context.Update(result);
                    int i = _context.SaveChanges();
                    if (i > 0)
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
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Feedback_Master_QuestionDTO questionsgetOrder(Feedback_Master_QuestionDTO data)
        {
            try
            {
                int i = 0;
                if (data.Questions_Master_TempDTO.Length > 0)
                {
                    for (int k = 0; k < data.Questions_Master_TempDTO.Length; k++)
                    {
                        i = i + 1;
                        var result = _context.Feedback_Master_QuestionsDMO.Single(a => a.MI_Id == data.MI_Id && a.FMQE_Id == data.Questions_Master_TempDTO[k].FMQE_Id);
                        result.FMQE_FQOrder = i;
                        result.UpdatedDate = DateTime.Now;
                        result.FMQE_UpdatedBy = data.userid;
                        _context.Update(result);
                    }
                    int kl = _context.SaveChanges();
                    if (kl > 0)
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
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        // Feedback Master Options
        public Feedback_Master_OptionDTO getoptiondetails(Feedback_Master_OptionDTO data)
        {
            try
            {
                data.getdetails = _context.Feedback_Master_OptionsDMO.Where(a => a.MI_Id == data.MI_Id).OrderBy(a => a.FMOP_FOOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Feedback_Master_OptionDTO optionsavedata(Feedback_Master_OptionDTO data)
        {
            try
            {
                if (data.FMOP_Id > 0)
                {
                    var checkname = _context.Feedback_Master_OptionsDMO.Where(a => a.MI_Id == data.MI_Id && a.FMOP_Id != data.FMOP_Id && a.FMOP_FeedbackOptions == data.FMOP_FeedbackOptions).ToList();

                    //var checkname1 = _context.Feedback_Master_OptionsDMO.Where(a => a.MI_Id == data.MI_Id && a.FMOP_Id != data.FMOP_Id && a.FMOP_OptionsValue == data.FMOP_OptionsValue).ToList();

                    if (checkname.Count() > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        var checknamed = _context.Feedback_Master_OptionsDMO.Single(a => a.MI_Id == data.MI_Id && a.FMOP_Id == data.FMOP_Id);
                        checknamed.FMOP_FeedbackOptions = data.FMOP_FeedbackOptions;
                        checknamed.FMOP_OptionsValue = data.FMOP_OptionsValue;
                        checknamed.FMOP_FeedbackORemarks = data.FMOP_FeedbackORemarks;
                        checknamed.FMOP_UpdatedBy = data.userid;
                        checknamed.UpdatedDate = DateTime.Now;
                        _context.Update(checknamed);
                        int i = _context.SaveChanges();
                        if (i > 0)
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
                    var checkname = _context.Feedback_Master_OptionsDMO.Where(a => a.MI_Id == data.MI_Id && a.FMOP_FeedbackOptions == data.FMOP_FeedbackOptions).ToList();
                    //var checkname1 = _context.Feedback_Master_OptionsDMO.Where(a => a.MI_Id == data.MI_Id && a.FMOP_OptionsValue == data.FMOP_OptionsValue).ToList();
                    if (checkname.Count() > 0)
                    {
                        data.message = "Duplicate";
                    }
                    else
                    {
                        var getrowcount = _context.Feedback_Master_OptionsDMO.Where(a => a.MI_Id == data.MI_Id).Count();

                        int rowucount = getrowcount + 1;


                        Feedback_Master_OptionsDMO dmo = new Feedback_Master_OptionsDMO();
                        dmo.MI_Id = data.MI_Id;
                        dmo.FMOP_FeedbackOptions = data.FMOP_FeedbackOptions;
                        dmo.FMOP_OptionsValue = data.FMOP_OptionsValue;
                        dmo.FMOP_FeedbackORemarks = data.FMOP_FeedbackORemarks;
                        dmo.FMOP_FOOrder = rowucount;
                        dmo.FMOP_CreatedBy = data.userid;
                        dmo.CreatedDate = DateTime.Now;
                        dmo.FMOP_UpdatedBy = data.userid;
                        dmo.UpdatedDate = DateTime.Now;
                        dmo.FMOP_ActiveFlag = true;
                        _context.Add(dmo);
                        int i = _context.SaveChanges();
                        if (i > 0)
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
                Console.WriteLine(ex.Message);
                data.returnval = false;
            }
            return data;
        }
        public Feedback_Master_OptionDTO optioneditdata(Feedback_Master_OptionDTO data)
        {
            try
            {

                data.editdetails = _context.Feedback_Master_OptionsDMO.Where(a => a.MI_Id == data.MI_Id && a.FMOP_Id == data.FMOP_Id).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Feedback_Master_OptionDTO optionactivedeactive(Feedback_Master_OptionDTO data)
        {
            try
            {
                var result12 = _context.Feedback_Master_OptionsDMO.Where(a => a.MI_Id == data.MI_Id && a.FMOP_Id == data.FMOP_Id).ToList();
                if (result12.FirstOrDefault().FMOP_ActiveFlag == true)
                {
                    var checkidused1 = _context.Feedback_Type_OptionsDMO.Where(a => a.MI_Id == data.MI_Id && a.FMOP_Id == data.FMOP_Id).Count();
                    var checkidused2 = _context.Feedback_College_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMOP_Id == data.FMOP_Id).Count();
                    var checkidused3 = _context.Feedback_School_Student_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMOP_Id == data.FMOP_Id).Count();
                    var checkidused4 = _context.Feedback_Staff_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMOP_Id == data.FMOP_Id).Count();
                    var checkidused5 = _context.Feedback_Alumni_TransactionDMO.Where(a => a.MI_Id == data.MI_Id && a.FMOP_Id == data.FMOP_Id).Count();
                    var checkidused6 = _context.Feedback_College_StudentToStaffDMO.Where(a => a.MI_Id == data.MI_Id && a.FMOP_Id == data.FMOP_Id).Count();
                    var checkidused7 = _context.Feedback_School_StudentToStaffDMO.Where(a => a.MI_Id == data.MI_Id && a.FMOP_Id == data.FMOP_Id).Count();

                    int k = checkidused1 + checkidused2 + checkidused3 + checkidused4 + checkidused5 + checkidused6 + checkidused7;

                    if (k > 0)
                    {
                        data.message = "Mapped";
                    }
                    else
                    {
                        var result = _context.Feedback_Master_OptionsDMO.Single(a => a.MI_Id == data.MI_Id && a.FMOP_Id == data.FMOP_Id);
                        if (result.FMOP_ActiveFlag == true)
                        {
                            result.FMOP_ActiveFlag = false;
                        }
                        else
                        {
                            result.FMOP_ActiveFlag = true;
                        }
                        result.UpdatedDate = DateTime.Now;
                        result.FMOP_UpdatedBy = data.userid;
                        _context.Update(result);
                        int i = _context.SaveChanges();
                        if (i > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else
                {
                    var result = _context.Feedback_Master_OptionsDMO.Single(a => a.MI_Id == data.MI_Id && a.FMOP_Id == data.FMOP_Id);
                    if (result.FMOP_ActiveFlag == true)
                    {
                        result.FMOP_ActiveFlag = false;
                    }
                    else
                    {
                        result.FMOP_ActiveFlag = true;
                    }
                    result.UpdatedDate = DateTime.Now;
                    result.FMOP_UpdatedBy = data.userid;
                    _context.Update(result);
                    int i = _context.SaveChanges();
                    if (i > 0)
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
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public Feedback_Master_OptionDTO optiongetOrder(Feedback_Master_OptionDTO data)
        {
            try
            {
                int i = 0;
                if (data.Option_Master_TempDTO.Length > 0)
                {
                    for (int k = 0; k < data.Option_Master_TempDTO.Length; k++)
                    {
                        i = i + 1;
                        var result = _context.Feedback_Master_OptionsDMO.Single(a => a.MI_Id == data.MI_Id && a.FMOP_Id == data.Option_Master_TempDTO[k].FMOP_Id);
                        result.FMOP_FOOrder = i;
                        result.UpdatedDate = DateTime.Now;
                        result.FMOP_UpdatedBy = data.userid;
                        _context.Update(result);
                    }
                    int kl = _context.SaveChanges();
                    if (kl > 0)
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
                data.returnval = false;
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
