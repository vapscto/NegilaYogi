using DataAccessMsSqlServerProvider.com.vapstech.Library;
using DomainModel.Model.com.vapstech.Library;
using PreadmissionDTOs.com.vaps.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryServiceHub.com.vaps.Services
{
    public class UserClassLibraryImpl:Interfaces.UserClassLibraryInterface
    {

        public LibraryContext _LibraryContext;
        public UserClassLibraryImpl(LibraryContext paar)
        {
            _LibraryContext = paar;
        }


        public LIB_Library_Class_DTO getdetails(int id)
        {
            LIB_Library_Class_DTO data = new LIB_Library_Class_DTO();
            try
            {
                data.stafflist = (/*from a in _LibraryContext.LIB_Master_Library_DMO*/
                                  from b in _LibraryContext.LIB_User_Library_DMO
                                  from c in _LibraryContext.MasterEmployee
                                  where (/*a.LMAL_Id == b.LMAL_Id && a.MI_Id == b.MI_Id &&*/ b.IVRMUL_Id == c.HRME_Id && c.HRME_ActiveFlag == true && b.LUL_ActiveFlg == true && b.MI_Id==id)
                                  select new LIB_Library_Class_DTO
                                  {
                                      IVRMUL_Id=b.IVRMUL_Id,
                                      HRME_Id = c.HRME_Id,
                                      HRME_EmployeeFirstName = ((c.HRME_EmployeeFirstName == null ? " " : c.HRME_EmployeeFirstName) + " " + (c.HRME_EmployeeMiddleName == null ? " " : c.HRME_EmployeeMiddleName) + " " + (c.HRME_EmployeeLastName == null ? " " : c.HRME_EmployeeLastName)).Trim(),

                                      HRME_EmployeeMiddleName = c.HRME_EmployeeMiddleName,
                                      HRME_EmployeeLastName = c.HRME_EmployeeLastName,

                                  }).Distinct().OrderBy(t => t.IVRMUL_Id).ToArray();


                var clsslst = _LibraryContext.School_M_Class.Where(t => t.MI_Id == id && t.ASMCL_ActiveFlag == true).Distinct().OrderBy(t => t.ASMCL_Id).ToArray();
                if (clsslst.Length > 0)
                {
                    data.classlist = clsslst;
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



        public LIB_Library_Class_DTO Savedata(LIB_Library_Class_DTO data)
        {
            try
            {
                //if (data.LMAL_Id > 0)
                //{
                //    var Duplicate = _LibraryContext.LIB_Library_Class_DMO.Where(t => t.MI_Id == data.MI_Id && t.LLC_Id != data.LLC_Id ).ToList();

                //    if (Duplicate.Count() > 0)
                //    {
                //        data.duplicate = true;
                //    }
                //    else
                //    {
                //        var update = _LibraryContext.LIB_Library_Class_DMO.Single(t => t.LLC_Id == data.LLC_Id && t.MI_Id == data.MI_Id);

                        
                //        update.UpdatedDate = DateTime.Now;
                //        _LibraryContext.Update(update);
                        

                //        int rowAffected = _LibraryContext.SaveChanges();
                //        if (rowAffected > 0)
                //        {
                //            data.returnval = true;
                //        }
                //        else
                //        {
                //            data.returnval = false;
                //        }
                //    }
                //}
                //else
                //{
                //    var Duplicate = _LibraryContext.LIB_Master_Library_DMO.Where(t => t.MI_Id == data.MI_Id ).ToList();

                //    if (Duplicate.Count() > 0)
                //    {
                //        data.duplicate = true;
                //    }
                //    else
                //    {
                //        LIB_Library_Class_DMO Obj = new LIB_Library_Class_DMO();

                //        Obj.MI_Id = data.MI_Id;
                //        Obj.ASMCL_Id = data.ASMCL_Id;
                //        Obj.LLC_ActiveFlg = true;
                //        Obj.CreatedDate = DateTime.Now;
                //        Obj.UpdatedDate = DateTime.Now;

                //        _LibraryContext.Add(Obj);

                //        int rowAffected = _LibraryContext.SaveChanges();
                //        if (rowAffected > 0)
                //        {
                //            data.returnval = true;
                //        }
                //        else
                //        {
                //            data.returnval = false;
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }


        public LIB_Library_Class_DTO deactiveY(LIB_Library_Class_DTO data)
        {
            try
            {
                var result = _LibraryContext.LIB_Library_Class_DMO.Single(t => t.MI_Id == data.MI_Id && t.LLC_Id == data.LLC_Id);

                if (result.LLC_ActiveFlg == true)
                {
                    result.LLC_ActiveFlg = false;
                }
                else if (result.LLC_ActiveFlg == false)
                {
                    result.LLC_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _LibraryContext.Update(result);
                int rowAffected = _LibraryContext.SaveChanges();
                if (rowAffected > 0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
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
