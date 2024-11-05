using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.HRMS;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.services
{
   public class ModeOfPaymentImpl : interfaces.ModeOfPaymentInterface
    {
        public FeeGroupContext _FeeGroupContext;
        public ModeOfPaymentImpl(FeeGroupContext hh)
        {
            _FeeGroupContext = hh;
        }
  //=========================================load=======================================================//
        public ModeOfPaymentDTO loaddata(ModeOfPaymentDTO data)
        {
            try
            {
                data.get_payment = _FeeGroupContext.IVRM_ModeOfPayment.Where(t => t.MI_Id == data.MI_Id).Distinct().ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
            return data;
        }
   //==========================================save===============================================================//
        public ModeOfPaymentDTO savedata(ModeOfPaymentDTO data)
        {
            try
            {
                if (data.IVRMMOD_Id == 0)
                {
                    var duplicate = _FeeGroupContext.IVRM_ModeOfPayment.Where(t =>  t.IVRMMOD_ModeOfPayment==data.IVRMMOD_ModeOfPayment && t.MI_Id==data.MI_Id ).ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        IVRM_ModeOfPayment ppp = new IVRM_ModeOfPayment();
                        ppp.MI_Id = data.MI_Id;
                        ppp.IVRMMOD_Id = data.IVRMMOD_Id;
                        ppp.IVRMMOD_ModeOfPayment = data.IVRMMOD_ModeOfPayment;
                        ppp.IVRMMOD_ModeOfPayment_Code = data.IVRMMOD_ModeOfPayment_Code;
                        ppp.IVRMMOD_ActiveFlag = true;
                        ppp.CreatedDate = DateTime.Now;
                        ppp.UpdatedDate = DateTime.Now;
                        ppp.IVRMMOD_CreatedBy = data.UserId;
                        ppp.IVRMMOD_UpdatedBy = data.UserId;
                        ppp.UpdatedDate = DateTime.Now;
                        if (data.IVRMMOD_ModeOfPayment=="Cash" || data.IVRMMOD_ModeOfPayment == "CASH")
                        {
                            ppp.IVRMMOD_Flag = "C";
                        }
                        else
                        {
                            ppp.IVRMMOD_Flag = "B";
                        }
                        
                        _FeeGroupContext.Add(ppp);
                        int p = _FeeGroupContext.SaveChanges();
                        if (p > 0)
                        {
                            data.msg = "Saved";
                        }
                        else {
                            data.msg = " Failed";
                    }
                    }
                }
                else if (data.IVRMMOD_Id > 0)
                {
                    var duplicate=_FeeGroupContext.IVRM_ModeOfPayment.Where(t=>t.IVRMMOD_Id==data.IVRMMOD_Id && t.IVRMMOD_ModeOfPayment==data.IVRMMOD_ModeOfPayment &&  t.IVRMMOD_ModeOfPayment_Code==data.IVRMMOD_ModeOfPayment_Code && t.IVRMMOD_Id!=data.IVRMMOD_Id && t.MI_Id==data.MI_Id).ToArray();
                    if(duplicate.Count()>0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var ss = _FeeGroupContext.IVRM_ModeOfPayment.Where(t => t.IVRMMOD_Id == data.IVRMMOD_Id).SingleOrDefault();
                        ss.MI_Id = data.MI_Id;
                        ss.IVRMMOD_Id = data.IVRMMOD_Id;
                        ss.IVRMMOD_ModeOfPayment = data.IVRMMOD_ModeOfPayment;
                        ss.IVRMMOD_ModeOfPayment_Code = data.IVRMMOD_ModeOfPayment_Code;
                        if (data.IVRMMOD_ModeOfPayment == "Cash" || data.IVRMMOD_ModeOfPayment == "CASH")
                        {
                            ss.IVRMMOD_Flag = "C";
                        }
                        else
                        {
                            ss.IVRMMOD_Flag = "B";
                        }
                        ss.UpdatedDate = DateTime.Now;
                        ss.IVRMMOD_UpdatedBy = data.UserId;
                        _FeeGroupContext.Update(ss);
                        var s = _FeeGroupContext.SaveChanges();
                        if (s>0)
                        {
                            data.msg = "Updated";
                        }
                        else
                        {
                            data.msg = "Updation Failed";
                        }
                     }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

            }
            return data;
        }
        //======================================================delete=====================================================//
        public ModeOfPaymentDTO deletedata(ModeOfPaymentDTO data)
        {
            try
            {
                for(int i=0;i<data.listdata07.Length;i++)
                {
                    var temp_id = data.listdata07[i].IVRMMOD_Id;
                    var result = _FeeGroupContext.IVRM_ModeOfPayment.Single(t => t.IVRMMOD_Id == temp_id);
                    if (result.IVRMMOD_ActiveFlag == true)
                    {
                        result.IVRMMOD_ActiveFlag = false;
                    }
                    result.UpdatedDate = DateTime.Now;
                    _FeeGroupContext.Update(result);

                }
                int row = _FeeGroupContext.SaveChanges();
                if (row > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }
            return data;
        }
        //=====================================================paymentDecative=========================================//
        public ModeOfPaymentDTO paymentDecative(ModeOfPaymentDTO data)
        {
            try
            {
                var u = _FeeGroupContext.IVRM_ModeOfPayment.Where(t => t.IVRMMOD_Id == data.IVRMMOD_Id).SingleOrDefault();
                if (u.IVRMMOD_ActiveFlag== true)
                {
                    u.IVRMMOD_ActiveFlag = false;
                }
                else if (u.IVRMMOD_ActiveFlag == false)
                {
                    u.IVRMMOD_ActiveFlag = true;
                }

                _FeeGroupContext.Update(u);
                int o = _FeeGroupContext.SaveChanges();
                if (o > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch (Exception f)
            {
                Console.WriteLine(f.Message);
            }
            return data;
        }
    }
}
