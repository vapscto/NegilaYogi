using DataAccessMsSqlServerProvider.com.vapstech.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Library;
using DomainModel.Model.com.vapstech.Library;

namespace LibraryServiceHub.com.vaps.Services
{
    public class MasterDepartmentImpl:Interfaces.MasterDepartmentInterface
    {
        public LibraryContext _LibContext;
        public MasterDepartmentImpl(LibraryContext para)
        {
            _LibContext = para;
        }


        public MasterDepartmentDTO getdetails(int id)
        {
            MasterDepartmentDTO data = new MasterDepartmentDTO();
            try
            {
                data.deptlist = _LibContext.MasterDepartmentDMO.Where(t => t.MI_Id == id).Distinct().OrderBy(t=>t.LMD_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }



        public MasterDepartmentDTO Savedata(MasterDepartmentDTO data)
        {
            try
            {
                if(data.LMD_Id>0) //Update
                {
                    var duplicate = _LibContext.MasterDepartmentDMO.Where(t => t.LMD_Id != data.LMD_Id && t.LMD_DepartmentName == data.LMD_DepartmentName).ToList();
                    if(duplicate.Count()>0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _LibContext.MasterDepartmentDMO.Single(t => t.MI_Id == data.MI_Id && t.LMD_Id == data.LMD_Id);
                        
                        update.LMD_DepartmentName = data.LMD_DepartmentName;
                        update.LMD_DepartmentCode = data.LMD_DepartmentCode;
                        update.UpdatedDate = DateTime.Now;
                        _LibContext.Update(update);
                        int AftRow=_LibContext.SaveChanges();
                        if(AftRow>0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
                else //Insert
                {
                    var duplicate = _LibContext.MasterDepartmentDMO.Where(t => t.MI_Id == data.MI_Id &&  t.LMD_DepartmentName == data.LMD_DepartmentName ).ToList();
                    if(duplicate.Count()>0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        MasterDepartmentDMO Obj = new MasterDepartmentDMO();
                        Obj.MI_Id = data.MI_Id;
                        Obj.LMD_DepartmentName = data.LMD_DepartmentName;
                        Obj.LMD_DepartmentCode = data.LMD_DepartmentCode;
                        Obj.LMD_ActiveFlg = true;
                        Obj.CreatedDate = DateTime.Now;
                        Obj.UpdatedDate = DateTime.Now;
                        _LibContext.Add(Obj);
                       int rowAffected = _LibContext.SaveChanges();
                        if(rowAffected > 0)
                        {
                            data.returnval = true;
                        }
                        else
                        {
                            data.returnval = false;
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

     
        public MasterDepartmentDTO deactiveY(MasterDepartmentDTO data)
        {
            try
            {
                var result = _LibContext.MasterDepartmentDMO.Single(t => t.MI_Id == data.MI_Id && t.LMD_Id == data.LMD_Id);

                if(result.LMD_ActiveFlg == true)
                {
                    result.LMD_ActiveFlg = false;
                }
                else if(result.LMD_ActiveFlg == false)
                {
                    result.LMD_ActiveFlg = true;
                }
                result.UpdatedDate = DateTime.Now;
                _LibContext.Update(result);
                int rowAffected=_LibContext.SaveChanges();
                if(rowAffected>0)
                {
                    data.returnval = true;
                }
                else
                {
                    data.returnval = false;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
    }
}
