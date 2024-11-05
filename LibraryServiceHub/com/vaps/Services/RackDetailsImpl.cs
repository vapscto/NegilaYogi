using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessMsSqlServerProvider.com.vapstech.Library;
using DomainModel.Model.com.vapstech.Library;
using PreadmissionDTOs.com.vaps.Library;

namespace LibraryServiceHub.com.vaps.Services
{
    public class RackDetailsImpl : Interfaces.RackDetailsInterface
    {
        public LibraryContext _LibraryContext;
        public RackDetailsImpl(LibraryContext para)
        {
            _LibraryContext = para;
        }       
       
        public RackDetailsDTO Savedata(RackDetailsDTO data)
        {
            try
            {
                if(data.LMRA_Id>0)
                {
                    var Duplicate = _LibraryContext.RackDetailsDMO.Where(t => t.MI_Id == data.MI_Id && t.LMRA_Id!=data.LMRA_Id && t.LMRA_RackName == data.LMRA_RackName).ToList();
                    if (Duplicate.Count() > 0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        var update1 = _LibraryContext.RackDetailsDMO.Single(t => t.LMRA_Id == data.LMRA_Id && t.MI_Id == data.MI_Id);
                     
                        update1.LMRA_RackName = data.LMRA_RackName;
                        update1.LMRA_DisplayColour = data.LMRA_DisplayColour;
                        update1.LMRA_Location = data.LMRA_Location;
                        update1.LMRA_FloorName = data.LMRA_FloorName;
                        update1.LMRA_BuildingName = data.LMRA_BuildingName;
                        update1.UpdatedDate = DateTime.Now;
                        _LibraryContext.Update(update1);

                        var update2 = _LibraryContext.Lib_Rack_SubjectDMO.Single(t => t.LMRAS_Id == data.LMRAS_Id && t.LMRA_Id==data.LMRA_Id);

                        update2.LMS_Id = data.LMS_Id;
                        update2.UpdatedDate = DateTime.Now;
                        _LibraryContext.Update(update2);
                        int rowAffected=_LibraryContext.SaveChanges();
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
                else
                {
                    var Duplicate = _LibraryContext.RackDetailsDMO.Where(t => t.MI_Id == data.MI_Id && t.LMRA_RackName == data.LMRA_RackName).ToList();
                    if(Duplicate.Count()>0)
                    {
                        data.duplicate = true;
                    }
                    else
                    {
                        RackDetailsDMO obj1 = new RackDetailsDMO();
                       // obj1.LMRA_Id = data.Rack_Id;
                        obj1.MI_Id = data.MI_Id;
                        obj1.LMRA_RackName = data.LMRA_RackName;
                        obj1.LMRA_DisplayColour = data.LMRA_DisplayColour;
                        obj1.LMRA_Location = data.LMRA_Location;
                        obj1.LMRA_FloorName = data.LMRA_FloorName;
                        obj1.LMRA_BuildingName = data.LMRA_BuildingName;
                        obj1.LMRA_ActiveFlag = true;
                        obj1.CreatedDate = DateTime.Now;
                        obj1.UpdatedDate = DateTime.Now;
                        _LibraryContext.Add(obj1);

                        Lib_Rack_SubjectDMO obj2 = new Lib_Rack_SubjectDMO();

                       // obj2.LMRAS_Id = data.RS_Id;
                        obj2.LMRA_Id = obj1.LMRA_Id;
                        obj2.LMS_Id = data.LMS_Id;
                        obj2.CreatedDate = DateTime.Now;
                        obj2.UpdatedDate = DateTime.Now;
                        obj2.LMRAS_ActiveFlg = true;
                        _LibraryContext.Add(obj2);

                        int rowAffected=_LibraryContext.SaveChanges();
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

        public RackDetailsDTO getdetails(int id)
        {
            RackDetailsDTO data = new RackDetailsDTO();
            try
            {
                //data.floorlist = _LibraryContext.MasterFloorDMO.Where(t => t.MI_Id == id && t.Floor_ActiveFlag==true).Distinct().ToArray(); 

                data.subjectlist = _LibraryContext.MasterSubject_DMO.Where(t => t.MI_Id == id && t.LMS_ActiveFlg == true).Distinct().ToArray();

                data.alldata = (from a in _LibraryContext.RackDetailsDMO
                                   from b in _LibraryContext.Lib_Rack_SubjectDMO
                                   //from c in _LibraryContext.MasterFloorDMO
                                   from d in _LibraryContext.MasterSubject_DMO
                                   where (a.LMRA_Id == b.LMRA_Id && b.LMS_Id == d.LMS_Id && a.MI_Id == id && d.LMS_ActiveFlg == true)
                                  //&& c.Floor_ActiveFlag == true && a.Rack_Floor_Id == c.Floor_Id  )
                                   select new RackDetailsDTO
                                   {
                                       MI_Id = a.MI_Id,
                                       LMRA_Id = a.LMRA_Id,
                                       LMRA_RackName = a.LMRA_RackName,
                                       LMRA_DisplayColour = a.LMRA_DisplayColour,
                                       LMRA_FloorName = a.LMRA_FloorName,
                                       LMRA_Location = a.LMRA_Location,
                                       LMRA_ActiveFlag = a.LMRA_ActiveFlag,
                                       LMS_Id=d.LMS_Id,
                                       LMRAS_Id = b.LMRAS_Id,
                                       LMRAS_ActiveFlg = b.LMRAS_ActiveFlg,
                                       LMS_SubjectName = d.LMS_SubjectName, 
                                       LMRA_BuildingName=a.LMRA_BuildingName,


                                   }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public RackDetailsDTO EditData(RackDetailsDTO data)
        {
            try
            {
                data.updatelist = (from a in _LibraryContext.RackDetailsDMO
                                   from b in _LibraryContext.Lib_Rack_SubjectDMO
                                   where (a.LMRA_Id == b.LMRA_Id && a.LMRA_Id == data.LMRA_Id && a.MI_Id == data.MI_Id)
                                   select new RackDetailsDTO
                                   {
                                       MI_Id = a.MI_Id,
                                       LMRA_Id = a.LMRA_Id,
                                       LMRA_RackName = a.LMRA_RackName,
                                       LMRA_DisplayColour = a.LMRA_DisplayColour,
                                       LMRA_FloorName = a.LMRA_FloorName,
                                       LMRA_Location = a.LMRA_Location,
                                       LMRA_ActiveFlag = a.LMRA_ActiveFlag,
                                       LMRA_BuildingName = a.LMRA_BuildingName,
                                       CreatedDate = a.CreatedDate,
                                       UpdatedDate = a.UpdatedDate,
                                       LMS_Id = b.LMS_Id,
                                       LMRAS_Id = b.LMRAS_Id,

                                   }).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }

        public RackDetailsDTO deactiveY(RackDetailsDTO data)
        {
            try
            {
                var result = _LibraryContext.RackDetailsDMO.Single(t => t.MI_Id == data.MI_Id && t.LMRA_Id == data.LMRA_Id);

                if (result.LMRA_ActiveFlag == true)
                {
                    result.LMRA_ActiveFlag = false;
                }
                else if (result.LMRA_ActiveFlag == false)
                {
                    result.LMRA_ActiveFlag = true;
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
