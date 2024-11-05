using DataAccessMsSqlServerProvider;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FeeServiceHub.com.vaps.interfaces;
using PreadmissionDTOs.com.vaps.Fees;
using DomainModel.Model;
using DomainModel.Model.com.vapstech.Fee;

namespace FeeServiceHub.com.vaps.services
{
    public class BankDetailsImpl : interfaces.BankDetailsInterface
    {
        public FeeGroupContext _YearlyFeeGroupMappingContext;
        public DomainModelMsSqlServerContext _context;
        readonly ILogger<FeeStudentTransactionImpl> _logger;
        public BankDetailsImpl(FeeGroupContext YearlyFeeGroupMappingContext, DomainModelMsSqlServerContext context)
        {
            _YearlyFeeGroupMappingContext = YearlyFeeGroupMappingContext;
            _context = context;         
        }
        public BankDetailsDTO getalldetails(BankDetailsDTO data)
         {
            try
            {
                data.alldata = (from a in _context.School_M_Class
                                from b in _context.BankDetailsDMO
                                where ( a.MI_Id == b.MI_Id&&a.ASMCL_Id == b.Class&&a.ASMCL_ActiveFlag==true)
                                
                                select new BankDetailsDTO
                                {
                                    FBD_ID = b.FBD_ID,
                                    Class = a.ASMCL_Id,
                                  classname=a.ASMCL_ClassName,
                                    Class_Category = b.Class_Category,
                                    Bank_Address = b.Bank_Address,
                                    Bank_Name = b.Bank_Name,
                                    ACC_name = b.ACC_name,
                                    IFSC = b.IFSC,
                                    Acc_No = b.Acc_No,
                                    Active_Flag = b.Active_Flag,

                                }).Distinct().ToArray();

                            List<School_M_Class> classlist = new List<School_M_Class>();
                classlist = _context.School_M_Class.Where(t => t.ASMCL_ActiveFlag == true && t.MI_Id == data.MI_Id).ToList();
                data.classlist = classlist.ToArray();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }

        public BankDetailsDTO getdata(BankDetailsDTO data)
        {
            try
            {
                if(data.FBD_ID==0)
                {
                    var duplicate = _context.BankDetailsDMO.Where(t => t.MI_Id == data.MI_Id && t.Class_Category == data.Class_Category && t.Class == data.Class && t.Bank_Name == data.Bank_Name).Distinct().ToArray();
                    if(duplicate.Count()>0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        BankDetailsDMO a = new BankDetailsDMO();
                    
                        a.MI_Id = data.MI_Id;
                        a.Class_Category = data.Class_Category;
                        a.Class = data.Class;

                        a.Bank_Name = data.Bank_Name;
                        a.Bank_Address = data.Bank_Address;
                        a.Acc_No = data.Acc_No;
                        a.IFSC = data.IFSC;
                        a.ACC_name = data.ACC_name;
                        a.Active_Flag = true;
                        _context.Add(a);
                    }
                    var w = _context.SaveChanges();
                    if(w > 0)
                    {
                        data.msg = "Saved";
                    }
                    else
                    {
                        data.msg = "Failed";
                    }
                }
                else if(data.FBD_ID > 0)
                {
                    var duplicate = _context.BankDetailsDMO.Where(t => t.FBD_ID != data.FBD_ID && t.Class_Category == data.Class_Category && t.Class == data.Class && t.Bank_Name == data.Bank_Name).Distinct().ToArray();
                    if (duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var j = _context.BankDetailsDMO.Where(t => t.MI_Id == data.MI_Id & t.FBD_ID == data.FBD_ID).SingleOrDefault();
                        j.FBD_ID = data.FBD_ID;
                        j.MI_Id = data.MI_Id;
                        j.Class_Category = data.Class_Category;
                        j.Class = data.Class;
                        j.Bank_Name = data.Bank_Name;
                        j.Bank_Address = data.Bank_Address;
                        j.Acc_No = data.Acc_No;
                        j.IFSC = data.IFSC;
                        j.ACC_name = data.ACC_name;
                        _context.Update(j);
                        var r = _context.SaveChanges();
                        if (r > 0)
                        {
                            data.msg = "updated";
                        }
                        else
                        {
                            data.msg = "failed";
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

        public BankDetailsDTO edittab1(BankDetailsDTO data)
        {
            try
            {

                //data.Editlist = _context.BankDetailsDMO.Where(t => t.MI_Id == data.MI_Id && t.FBD_ID == data.FBD_ID).Distinct().ToArray();
                data.Editlist = (from c in _context.School_M_Class
                                 from d in _context.BankDetailsDMO
                                 where (c.MI_Id == d.MI_Id && c.ASMCL_Id == d.Class && d.FBD_ID == data.FBD_ID)

                                 select new BankDetailsDTO
                                 {
                                     FBD_ID = d.FBD_ID,
                                     ASMCL_Id = d.Class,
                                     Class = d.Class,
                                     classname = c.ASMCL_ClassName,
                                     Class_Category = d.Class_Category,
                                     Bank_Address = d.Bank_Address,
                                     Bank_Name = d.Bank_Name,
                                     ACC_name = d.ACC_name,
                                     IFSC = d.IFSC,
                                     Acc_No = d.Acc_No,
                                     Active_Flag = d.Active_Flag,

                                 }).Distinct().ToArray();

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
            return data;
        }
        public BankDetailsDTO deactive(BankDetailsDTO data)
        {
            try
            {
                var g = _context.BankDetailsDMO.Where(t => t.FBD_ID == data.FBD_ID).SingleOrDefault();
                if (g.Active_Flag == true)
                {
                    g.Active_Flag = false;
                }
                else if (g.Active_Flag == false)
                {
                    g.Active_Flag = true;
                }

                g.MI_Id = data.MI_Id;
                _context.Update(g);
                int s = _context.SaveChanges();
                if (s > 0)
                {
                    data.ret = true;
                }
                else
                {
                    data.ret = false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return data;
        }



    }
}
