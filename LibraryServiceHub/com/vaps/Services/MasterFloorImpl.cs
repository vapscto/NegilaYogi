using DataAccessMsSqlServerProvider.com.vapstech.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PreadmissionDTOs.com.vaps.Library;
using DomainModel.Model.com.vapstech.Library;

namespace LibraryServiceHub.com.vaps.Services
{
    public class MasterFloorImpl:Interfaces.MasterFloorInterface
    {
        public LibraryContext _LibraryContext;
        public MasterFloorImpl(LibraryContext para)
        {
            _LibraryContext = para;
        }

        public MasterFloorDTO Savedata(MasterFloorDTO data)
        {
            try
            {
                if(data.Floor_Id>0)
                {
                    var Duplicate = _LibraryContext.MasterFloorDMO.Where(t=>t.Floor_Id!=data.Floor_Id && t.FloorName == data.FloorName && t.MI_Id == data.MI_Id).ToList
                        ();
                    if (Duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update = _LibraryContext.MasterFloorDMO.Single(t => t.Floor_Id == data.Floor_Id && t.MI_Id == data.MI_Id);
                       
                        update.MI_Id = data.MI_Id;
                        update.FloorName = data.FloorName;
                        update.UpdatedDate = DateTime.Now;
                        _LibraryContext.Update(update);
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
                }
                else
                {
                    var Duplicate = _LibraryContext.MasterFloorDMO.Where(t=>t.FloorName == data.FloorName && t.MI_Id==data.MI_Id).ToList
                        ();
                    if (Duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        MasterFloorDMO obj = new MasterFloorDMO();
                        obj.Floor_Id = data.Floor_Id;
                        obj.MI_Id = data.MI_Id;
                        obj.FloorName = data.FloorName;
                        obj.Floor_ActiveFlag = true;
                        obj.CreatedDate = DateTime.Now;
                        obj.UpdatedDate = DateTime.Now;
                        _LibraryContext.Add(obj);
                       int rowAffected= _LibraryContext.SaveChanges();
                        if(rowAffected>0)
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

        public MasterFloorDTO getdetails(int id)
        {
            MasterFloorDTO data = new MasterFloorDTO();
            try
            {
                data.floorlist = _LibraryContext.MasterFloorDMO.Where(t => t.MI_Id == id).Distinct().ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public MasterFloorDTO deactiveY(MasterFloorDTO data)
        {
            try
            {
                var result = _LibraryContext.MasterFloorDMO.Single(t => t.MI_Id == data.MI_Id && t.Floor_Id == data.Floor_Id);

                if (result.Floor_ActiveFlag == true)
                {
                    result.Floor_ActiveFlag = false;
                }
                else if (result.Floor_ActiveFlag == false)
                {
                    result.Floor_ActiveFlag = true;
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
