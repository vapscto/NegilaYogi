using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model.com.vaps.Exam;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommonLibrary
{
    public class MarksCalcReset
    {

        public readonly ExamContext _examcontext;

        public MarksCalcReset(ExamContext examcontext)
        {
            _examcontext = examcontext;
        }

        public string MarksCalculationResetFlag(long ASMAY_Id, long ASMCL_Id, long ASMS_Id, long MI_Id, int EME_Id)
        {
            string returnresult = "";

            List<ExmStudentMarksProcessDMO> calculationid = new List<ExmStudentMarksProcessDMO>();
            calculationid = _examcontext.ExmStudentMarksProcessDMO.Where(t => t.ASMAY_Id == ASMAY_Id && t.ASMCL_Id == ASMCL_Id && t.ASMS_Id == ASMS_Id && t.MI_Id == MI_Id && t.EME_Id == EME_Id).ToList();

            if (calculationid.Count > 0)
            {               
               returnresult = "true";
            }
            else
            {
                returnresult = "false";
            }


            List<ExmStudentMarksProcessSubjectwiseDMO> calculationSubWiseid = new List<ExmStudentMarksProcessSubjectwiseDMO>();
            calculationSubWiseid = _examcontext.ExmStudentMarksProcessSubjectwiseDMO.Where(t => t.ASMAY_Id == ASMAY_Id && t.ASMCL_Id == ASMCL_Id && t.ASMS_Id == ASMS_Id && t.MI_Id == MI_Id && t.EME_Id == EME_Id).ToList();


            if (calculationSubWiseid.Count > 0)
            {
                returnresult = "true";
            }
            else
            {
                returnresult = "false";
            }

            return returnresult;
        }

        public string MarksCalculationReset(long ASMAY_Id, long ASMCL_Id, long ASMS_Id, long MI_Id, int EME_Id)
        {
            string returnresult = "";

            List<ExmStudentMarksProcessDMO> calculationid = new List<ExmStudentMarksProcessDMO>();
            calculationid = _examcontext.ExmStudentMarksProcessDMO.Where(t => t.ASMAY_Id == ASMAY_Id && t.ASMCL_Id == ASMCL_Id && t.ASMS_Id == ASMS_Id && t.MI_Id == MI_Id && t.EME_Id == EME_Id).ToList();

            if (calculationid.Count > 0)
            {
                if (calculationid.Any())
                {
                    _examcontext.Remove(calculationid.ElementAt(0));
                    var contactExists = _examcontext.SaveChanges();
                    if (contactExists == 1)
                    {
                        returnresult = "true";
                    }
                    else
                    {
                        returnresult = "false";
                    }
                }
            }

            List<ExmStudentMarksProcessSubjectwiseDMO> calculationSubWiseid = new List<ExmStudentMarksProcessSubjectwiseDMO>();
            calculationSubWiseid = _examcontext.ExmStudentMarksProcessSubjectwiseDMO.Where(t => t.ASMAY_Id == ASMAY_Id && t.ASMCL_Id == ASMCL_Id && t.ASMS_Id == ASMS_Id && t.MI_Id == MI_Id && t.EME_Id == EME_Id).ToList();

            if (calculationSubWiseid.Count > 0)
            {
                if (calculationSubWiseid.Any())
                {
                    _examcontext.Remove(calculationSubWiseid.ElementAt(0));
                    var contactExists = _examcontext.SaveChanges();
                    if (contactExists == 1)
                    {
                        returnresult = "true";
                    }
                    else
                    {
                        returnresult = "false";
                    }
                }
            }

            return returnresult;
        }
    }
}
