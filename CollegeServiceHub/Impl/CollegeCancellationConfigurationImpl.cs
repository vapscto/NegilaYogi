using DataAccessMsSqlServerProvider.com.vapstech.College.Admission;
using DomainModel.Model.com.vapstech.College.Admission;
using PreadmissionDTOs.com.vaps.College.Admission;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CollegeServiceHub.Impl
{
    public class CollegeCancellationConfigurationImpl : Interface.CollegeCancellationConfigurationInterface
    {
        public ClgAdmissionContext _context;

        public CollegeCancellationConfigurationImpl(ClgAdmissionContext context)
        {
            _context = context;
        }
        public CollegeCancellationConfigurationDTO getdata(CollegeCancellationConfigurationDTO data)
        {
            try
            {
                data.getdetails = _context.CollegeCancellationConfigurationDMO.Where(a => a.MI_Id == data.MI_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeCancellationConfigurationDTO saveconfig(CollegeCancellationConfigurationDTO data)
        {
            try
            {
                if (data.ACACC_Id > 0)
                {
                    var checkduplicate = _context.CollegeCancellationConfigurationDMO.Where(a => a.MI_Id == data.MI_Id && a.ACACC_Id != data.ACACC_Id
                     && a.ACACC_ToDays > data.ACACC_FromDays).ToList();

                    var checkduplicate1 = _context.CollegeCancellationConfigurationDMO.Where(a => a.MI_Id == data.MI_Id && a.ACACC_Id != data.ACACC_Id
                     && a.ACACC_FromDays >= data.ACACC_ToDays && a.ACACC_ToDays <= data.ACACC_ToDays).ToList();


                    //if (checkduplicate.Count() > 0)
                    //{
                    //    data.message = "FromDays";
                    //}
                    //else
                    //{
                    //    if (checkduplicate1.Count() > 0)
                    //    {
                    //        data.message = "ToDays";
                    //    }
                    //    else
                    //    {
                    var result = _context.CollegeCancellationConfigurationDMO.Single(a => a.MI_Id == data.MI_Id && a.ACACC_Id == data.ACACC_Id);
                    result.ACACC_FromDays = data.ACACC_FromDays;
                    result.ACACC_ToDays = data.ACACC_ToDays;
                    result.ACACC_RefundAmountPer = data.ACACC_RefundAmountPer;
                    result.ACACC_CancellationPer = data.ACACC_CancellationPer;
                    result.UpdatedDate = DateTime.Now;
                    result.ACACC_UpdatedBy = data.ACACC_CreatedBy;

                    _context.Update(result);
                    var i = _context.SaveChanges();
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
                    //    }
                    //}
                }
                else
                {
                    var checkduplicate = _context.CollegeCancellationConfigurationDMO.Where(a => a.MI_Id == data.MI_Id
                   && a.ACACC_ToDays > data.ACACC_FromDays).ToList();

                    var checkduplicate1 = _context.CollegeCancellationConfigurationDMO.Where(a => a.MI_Id == data.MI_Id
                     && a.ACACC_FromDays > data.ACACC_ToDays && a.ACACC_ToDays <= data.ACACC_ToDays).ToList();

                    //if (checkduplicate.Count() > 0)
                    //{
                    //    data.message = "FromDays";
                    //}
                    //else
                    //{
                    //if (checkduplicate1.Count() > 0)
                    //{
                    //    data.message = "ToDays";
                    //}
                    //else
                    //{
                    CollegeCancellationConfigurationDMO result = new CollegeCancellationConfigurationDMO();
                    result.MI_Id = data.MI_Id;
                    result.ACACC_DOAFlg = data.ACACC_DOAFlg;
                    result.ACACC_FromDays = data.ACACC_FromDays;
                    result.ACACC_ToDays = data.ACACC_ToDays;
                    result.ACACC_RefundAmountPer = data.ACACC_RefundAmountPer;
                    result.ACACC_CancellationPer = data.ACACC_CancellationPer;
                    result.UpdatedDate = DateTime.Now;
                    result.CreatedDate = DateTime.Now;
                    result.ACACC_UpdatedBy = data.ACACC_CreatedBy;
                    result.ACACC_CreatedBy = data.ACACC_CreatedBy;
                    result.ACACC_ActiveFlag = true;
                    _context.Add(result);
                    var i = _context.SaveChanges();
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
                    //}
                    // }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeCancellationConfigurationDTO editconfig(CollegeCancellationConfigurationDTO data)
        {
            try
            {
                data.editdetails = _context.CollegeCancellationConfigurationDMO.Where(a => a.MI_Id == data.MI_Id && a.ACACC_Id == data.ACACC_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public CollegeCancellationConfigurationDTO activedeactive(CollegeCancellationConfigurationDTO data)
        {
            try
            {
                if (data.ACACC_Id > 0)
                {
                    var result = _context.CollegeCancellationConfigurationDMO.Where(a => a.MI_Id == data.MI_Id && a.ACACC_Id == data.ACACC_Id).ToList();
                    if (result.Count() > 0)
                    {
                        var result1 = _context.CollegeCancellationConfigurationDMO.Single(a => a.MI_Id == data.MI_Id && a.ACACC_Id == data.ACACC_Id);
                        _context.Remove(result1);

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
                    else
                    {
                        data.returnval = false;
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
