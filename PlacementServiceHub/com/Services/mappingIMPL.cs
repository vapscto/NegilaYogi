using DataAccessMsSqlServerProvider.com.vapstech.Placement;
using DomainModel.Model.com.vapstech.Placement;
using PreadmissionDTOs.com.vaps.Placement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlacementServiceHub.com.Services
{
    public class mappingIMPL : Interfaces.mappingInterface
    {
        public PlacementContext _PlacementContext;
        public mappingIMPL(PlacementContext PlacementContext)
        {
            _PlacementContext = PlacementContext;
        }
        //load data
        public mappingDTO loaddata(int id)
        {

            mappingDTO dto = new mappingDTO();
            try
            {
                dto.getsavedata = _PlacementContext.MasterCourseDMO.Distinct().ToArray();

                dto.save = (from a in _PlacementContext.mappingDMO
                            from b in _PlacementContext.MasterCourseDMO


                            where (a.AMCO_Id == b.AMCO_Id)
                            select new mappingDTO
                            {
                                PLMCLSMAP_Id=a.PLMCLSMAP_Id,
                                MI_Id = a.MI_Id,
                                AMCO_Id = a.AMCO_Id,
                                AMCO_CourseName = b.AMCO_CourseName,
                                PLMCLSMAP_ClassName = a.PLMCLSMAP_ClassName,
                                PLMCLSMAP_ClassFlg = a.PLMCLSMAP_ClassFlg,
                                PLMCLSMAP_Remarks = a.PLMCLSMAP_Remarks,
                                PLMCLSMAP_ActiveFlag=a.PLMCLSMAP_ActiveFlag

                            }).Distinct().ToArray();

            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
               
            }
            return dto;

        }

        //save data
        public mappingDTO savedata(mappingDTO data)

        
        {
            try
            {
                if (data.PLMCLSMAP_Id != 0)
                {
                    var res = _PlacementContext.mappingDMO.Where(t => t.PLMCLSMAP_ClassName == data.PLMCLSMAP_ClassName && t.PLMCLSMAP_ClassFlg == data.PLMCLSMAP_ClassFlg && t.PLMCLSMAP_Remarks == data.PLMCLSMAP_Remarks).ToList();
                    if (res.Count > 0)
                    {
                        data.returnduplicatestatus = "Duplicate";
                    }
                    else
                    {
                        var result = _PlacementContext.mappingDMO.Single(t => t.PLMCLSMAP_Id == data.PLMCLSMAP_Id);

                        result.MI_Id = data.MI_Id;
                        result.AMCO_Id = data.AMCO_Id;
                        result.PLMCLSMAP_ClassName = data.PLMCLSMAP_ClassName;
                        result.PLMCLSMAP_ClassFlg = data.PLMCLSMAP_ClassFlg;
                        result.PLMCLSMAP_Remarks = data.PLMCLSMAP_Remarks;
                        result.PLMCLSMAP_CreatedDate = DateTime.Now;
                        result.PLMCLSMAP_UpdatedDate = DateTime.Now;

                        result.PLMCLSMAP_CreatedBy = data.User_Id;
                        result.PLMCLSMAP_UpdatedBy = data.User_Id;
                        result.PLMCLSMAP_ActiveFlag = true;
                        _PlacementContext.Update(result);

                        var contactExists = _PlacementContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = "update";
                        }
                        else
                        {
                            data.returnval = "Notupdate";
                        }
                    }
                }
                else
                {
                    var res = _PlacementContext.mappingDMO.Where(t => t.PLMCLSMAP_ClassName == data.PLMCLSMAP_ClassName && t.PLMCLSMAP_ClassFlg == data.PLMCLSMAP_ClassFlg && t.PLMCLSMAP_Remarks == data.PLMCLSMAP_Remarks).ToList();

                    
if (res.Count > 0)
                    {
                        data.returnval = "Duplicate";
                    }
                    else
                    {
                        mappingDMO an = new mappingDMO();



                        an.MI_Id = data.MI_Id;
                        an.AMCO_Id = data.AMCO_Id;
                        an.PLMCLSMAP_ClassName = data.PLMCLSMAP_ClassName;
                        an.PLMCLSMAP_ClassFlg = data.PLMCLSMAP_ClassFlg;
                        an.PLMCLSMAP_Remarks = data.PLMCLSMAP_Remarks;
                        an.PLMCLSMAP_CreatedDate = DateTime.Now;
                        an.PLMCLSMAP_UpdatedDate = DateTime.Now;
                        an.PLMCLSMAP_CreatedBy = data.User_Id;
                        an.PLMCLSMAP_UpdatedBy = data.User_Id;
                        an.PLMCLSMAP_ActiveFlag = true;
                        _PlacementContext.Add(an);

                        var contactExists = _PlacementContext.SaveChanges();
                        if (contactExists > 0)
                        {
                            data.returnval = "saved";
                        }
                        else
                        {
                            data.returnval = "notsaved";
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.returnval = "admin";
            }
            return data;
        }


        //edit
        public mappingDTO edit(mappingDTO dto)        {            try            {
                dto.editdata = _PlacementContext.mappingDMO.Where(t => t.PLMCLSMAP_Id == dto.PLMCLSMAP_Id).ToArray();


            }            catch (Exception ee)            {                Console.WriteLine(ee.Message);            }            return dto;        }

        //deactive
        public mappingDTO deactive(mappingDTO data)
        {
            try
            {
                var result = _PlacementContext.mappingDMO.Single(t => t.PLMCLSMAP_Id == data.PLMCLSMAP_Id);

                if (result.PLMCLSMAP_ActiveFlag == true)
                {
                    result.PLMCLSMAP_ActiveFlag = false;
                }
                else if (result.PLMCLSMAP_ActiveFlag == false)
                {
                    result.PLMCLSMAP_ActiveFlag = true;
                }

                _PlacementContext.Update(result);
                int returnval = _PlacementContext.SaveChanges();
                if (returnval > 0)
                {
                    data.returnval = "true";
                }
                else
                {
                    data.returnval = "false";
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
            }

            return data;
        }




    }
}
