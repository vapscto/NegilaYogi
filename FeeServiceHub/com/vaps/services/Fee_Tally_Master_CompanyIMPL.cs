using DataAccessMsSqlServerProvider;
using DomainModel.Model.com.vapstech.Fee.Tally;
using PreadmissionDTOs.com.vaps.Fees.Tally;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FeeServiceHub.com.vaps.services
{
    public class Fee_Tally_Master_CompanyIMPL : interfaces.Fee_Tally_Master_CompanyInterface
    {
        public FeeGroupContext _FeeGroupContext;
        public Fee_Tally_Master_CompanyIMPL(FeeGroupContext frgContext)
        {
            _FeeGroupContext = frgContext;
        }
        public Fee_Tally_Master_CompanyDTO loaddata(Fee_Tally_Master_CompanyDTO data)
        {
            try
            {
                data.getarray = _FeeGroupContext.Fee_Tally_Master_CompanyDMO.Where(R => R.MI_Id == data.MI_Id).Distinct().ToArray();

             

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public Fee_Tally_Master_CompanyDTO savedata(Fee_Tally_Master_CompanyDTO data)
        {
            try
            {                                
                if (data.FTMCOM_Id > 0)
                {
                    var result = _FeeGroupContext.Fee_Tally_Master_CompanyDMO.Where(P => P.MI_Id == data.MI_Id && P.FTMCOM_CompanyName == data.FTMCOM_CompanyName && P.FTMCOM_Id != data.FTMCOM_Id).ToList();
                    if (result.Count > 0)
                    {
                        data.return_val = "RecordExist";
                    }
                    else
                    {
                        var resultwo = _FeeGroupContext.Fee_Tally_Master_CompanyDMO.Where(P => P.MI_Id == data.MI_Id && P.FTMCOM_Id == data.FTMCOM_Id).FirstOrDefault();
                        if (resultwo.FTMCOM_ActiveId == true)
                        {
                            resultwo.FTMCOM_Id = data.FTMCOM_Id;
                            resultwo.FTMCOM_CompanyCode = data.FTMCOM_CompanyCode;
                            resultwo.FTMCOM_CompanyName = data.FTMCOM_CompanyName;
                            resultwo.MI_Id = data.MI_Id;
                            resultwo.FTMCOM_UpdatedDate = DateTime.Now;
                            _FeeGroupContext.Update(resultwo);
                            var i = _FeeGroupContext.SaveChanges();
                            if (i > 0)
                            {
                                data.return_val = "Update";
                            }
                            else
                            {
                                data.return_val = "Notupdate";
                            }

                        }
                    }
                   
                }
                else
                {
                    var result = _FeeGroupContext.Fee_Tally_Master_CompanyDMO.Where(P => P.MI_Id == data.MI_Id && P.FTMCOM_CompanyName == data.FTMCOM_CompanyName).ToList();
                    if (result.Count > 0)
                    {
                        data.return_val = "RecordExist";
                    }
                    else
                    {
                        Fee_Tally_Master_CompanyDMO obj = new Fee_Tally_Master_CompanyDMO();
                        obj.FTMCOM_Id = data.FTMCOM_Id;
                        obj.MI_Id = data.MI_Id;
                        obj.FTMCOM_UpdatedDate = DateTime.Now;
                        obj.FTMCOM_CreatedDate = DateTime.Now;
                        obj.FTMCOM_CompanyName = data.FTMCOM_CompanyName;
                        obj.FTMCOM_CompanyCode = data.FTMCOM_CompanyCode;
                        obj.FTMCOM_ActiveId = true;
                        _FeeGroupContext.Add(obj);
                        var i = _FeeGroupContext.SaveChanges();
                        if (i > 0)
                        {
                            data.return_val = "save";
                        }
                        else
                        {
                            data.return_val = "Notsave";
                        }

                    }
                    
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        //deletedataYYY

        public Fee_Tally_Master_CompanyDTO deletedata(Fee_Tally_Master_CompanyDTO data)
        {
            try
            {
               
                if (data.FTMCOM_Id > 0)
                {
                    var resultwo = _FeeGroupContext.Fee_Tally_Master_CompanyDMO.Where(P => P.FTMCOM_Id == data.FTMCOM_Id).FirstOrDefault();

                    resultwo.FTMCOM_ActiveId = resultwo.FTMCOM_ActiveId == true ? false : true;
                    resultwo.FTMCOM_UpdatedDate = DateTime.Now;
                    _FeeGroupContext.Update(resultwo);
                    var i = _FeeGroupContext.SaveChanges();
                    if (i > 0)
                    {
                        data.return_val = "Delete";
                    }
                    else
                    {
                        data.return_val = "NotDelete";
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
