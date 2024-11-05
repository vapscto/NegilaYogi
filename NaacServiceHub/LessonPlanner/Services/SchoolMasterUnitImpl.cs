using DataAccessMsSqlServerProvider.com.vapstech.Exam;
using DomainModel.Model.com.vapstech.Exam.LessonPlanner;
using PreadmissionDTOs.com.vaps.Exam.LessonPlanner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaacServiceHub.com.vaps.LessonPlanner.Services
{
    public class SchoolMasterUnitImpl : Interface.SchoolMasterUnitInterface
    {
        public LessonplannerContext _context;
        public SchoolMasterUnitImpl(LessonplannerContext _conte)
        {
            _context = _conte;
        }
        public SchoolMasterUnitDTO Getdetails(SchoolMasterUnitDTO data)
        {
            try
            {
                data.getdetails = _context.SchoolMasterUnitDMO.Where(a => a.MI_Id == data.MI_Id).OrderBy(a => a.LPMU_Order).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SchoolMasterUnitDTO savedetails(SchoolMasterUnitDTO data)
        {
            try
            {
                if (data.LPMU_Id > 0)
                {
                    var checkduplicate = _context.SchoolMasterUnitDMO.Where(a => a.MI_Id == data.MI_Id && a.LPMU_UnitName.Equals(data.LPMU_UnitName) && a.LPMU_Id != data.LPMU_Id).ToList();
                    if (checkduplicate.Count() == 0)
                    {
                        var checkresult = _context.SchoolMasterUnitDMO.Single(a => a.MI_Id == data.MI_Id && a.LPMU_Id == data.LPMU_Id);
                        checkresult.LPMU_UnitName = data.LPMU_UnitName;
                        checkresult.LPMU_UnitDescription = data.LPMU_UnitDescription;
                        checkresult.LPMU_TotalHrs = data.LPMU_TotalHrs;
                        checkresult.LPMU_TotalPeriods = data.LPMU_TotalPeriods;
                        checkresult.LPMU_UpdatedBy = data.LPMU_CreatedBy;
                        checkresult.UpdatedDate = DateTime.Now;

                        _context.Update(checkresult);
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
                    }
                    else
                    {
                        data.message = "Duplicate";
                    }
                }
                else
                {
                    var checkduplicate = _context.SchoolMasterUnitDMO.Where(a => a.MI_Id == data.MI_Id && a.LPMU_UnitName.Equals(data.LPMU_UnitName)).ToList();
                    if (checkduplicate.Count() == 0)
                    {
                        var checkrowcount = _context.SchoolMasterUnitDMO.Where(a => a.MI_Id == data.MI_Id).Count();

                        checkrowcount = checkrowcount + 1;

                        SchoolMasterUnitDMO dmo = new SchoolMasterUnitDMO();
                        dmo.MI_Id = data.MI_Id;
                        dmo.LPMU_UnitName = data.LPMU_UnitName;
                        dmo.LPMU_UnitDescription = data.LPMU_UnitDescription;
                        dmo.LPMU_TotalHrs = data.LPMU_TotalHrs;
                        dmo.LPMU_TotalPeriods = data.LPMU_TotalPeriods;
                        dmo.LPMU_UpdatedBy = data.LPMU_CreatedBy;
                        dmo.LPMU_Order = checkrowcount;
                        dmo.UpdatedDate = DateTime.Now;
                        dmo.LPMU_CreatedBy = data.LPMU_CreatedBy;
                        dmo.CreatedDate = DateTime.Now;
                        dmo.LPMU_ActiveFlag = true;
                        _context.Add(dmo);
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
                    }
                    else
                    {
                        data.message = "Duplicate";
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SchoolMasterUnitDTO editdeatils(SchoolMasterUnitDTO data)
        {
            try
            {
                data.geteditdetails = _context.SchoolMasterUnitDMO.Where(a => a.MI_Id == data.MI_Id && a.LPMU_Id == data.LPMU_Id).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SchoolMasterUnitDTO deactivate(SchoolMasterUnitDTO data)
        {
            try
            {
                var checkunitused = _context.SchoolMasterTopicUnitDMO.Where(a => a.LPMU_Id == data.LPMU_Id).ToList();
                if (checkunitused.Count() > 0)
                {
                    data.message = "Exists";
                }
                else
                {
                    var checkresult = _context.SchoolMasterUnitDMO.Single(a => a.MI_Id == data.MI_Id && a.LPMU_Id == data.LPMU_Id);
                    if (checkresult.LPMU_ActiveFlag == true)
                    {
                        checkresult.LPMU_ActiveFlag = false;
                    }
                    else
                    {
                        checkresult.LPMU_ActiveFlag = true;
                    }
                    checkresult.UpdatedDate = DateTime.Now;
                    checkresult.LPMU_UpdatedBy = data.LPMU_CreatedBy;
                    _context.Update(checkresult);
                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
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
        public SchoolMasterUnitDTO validateordernumber(SchoolMasterUnitDTO data)
        {
            try
            {
                if (data.SchoolMasterUnitTempDTO.Count() > 0)
                {
                    int count = 0;
                    for (int k = 0; k < data.SchoolMasterUnitTempDTO.Count(); k++)
                    {
                        count = count + 1;
                        var checkresult = _context.SchoolMasterUnitDMO.Single(a => a.MI_Id == data.MI_Id && a.LPMU_Id == data.SchoolMasterUnitTempDTO[k].LPMU_Id);
                        checkresult.LPMU_Order = count;
                        checkresult.LPMU_UpdatedBy = data.LPMU_CreatedBy;
                        checkresult.UpdatedDate = DateTime.Now;
                        _context.Update(checkresult);
                    }
                    var i = _context.SaveChanges();
                    if (i > 0)
                    {
                        data.returnval = true;
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

        // Master Unit Topic Mapping
        public SchoolMasterUnitDTO Getdetailsmapping(SchoolMasterUnitDTO data)
        {
            try
            {
                data.getunitdetails = _context.SchoolMasterUnitDMO.Where(a => a.MI_Id == data.MI_Id).OrderBy(a => a.LPMU_Order).ToArray();

                data.getgriddetails = (from a in _context.SchoolMasterTopicUnitDMO
                                       from b in _context.SchoolSubjectWithMasterTopicMapping
                                       from c in _context.SchoolMasterUnitDMO
                                       where (a.LPMT_Id == b.LPMT_Id && a.LPMU_Id == c.LPMU_Id && b.MI_Id == data.MI_Id && c.MI_Id == data.MI_Id)
                                       select new SchoolMasterUnitDTO
                                       {
                                           LPMU_Id = a.LPMU_Id,
                                           LPMU_UnitName = c.LPMU_UnitName,
                                           LPMU_Order = c.LPMU_Order,
                                           LPMT_Id = a.LPMT_Id,
                                           LPMT_TopicName = b.LPMT_TopicName,
                                           LPMT_TopicOrder = b.LPMT_TopicOrder,
                                           LPMTU_Id = a.LPMTU_Id,
                                           LPMUT_ActiveFlag = a.LPMUT_ActiveFlag

                                       }).Distinct().OrderBy(a => a.LPMU_Order).ThenBy(a => a.LPMT_TopicOrder).ToArray();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SchoolMasterUnitDTO gettopicnames(SchoolMasterUnitDTO data)
        {
            try
            {
                List<long> ids = new List<long>();
                var getsavedids = _context.SchoolMasterTopicUnitDMO.Where(a => a.LPMU_Id == data.LPMU_Id).ToList();
                for (int k = 0; k < getsavedids.Count(); k++)
                {
                    ids.Add(getsavedids[k].LPMT_Id);
                }

                data.gettopicdetails = _context.SchoolSubjectWithMasterTopicMapping.Where(a => a.MI_Id == data.MI_Id && !ids.Contains(a.LPMT_Id)).OrderBy(a => a.LPMT_TopicOrder).ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return data;
        }
        public SchoolMasterUnitDTO savemappingdetails(SchoolMasterUnitDTO data)
        {
            try
            {
                if (data.SchoolMasterUnitTopicMappingTempDTO.Count() > 0)
                {
                    List<long> temparr = new List<long>();
                    List<long> temparr1 = new List<long>();

                    foreach (SchoolMasterUnitTopicMappingTempDTO ph in data.SchoolMasterUnitTopicMappingTempDTO)
                    {
                        temparr.Add(ph.LPMT_Id);
                    }
                    
                    Array Phone_Noresultremove = _context.SchoolMasterTopicUnitDMO.Where(t => !temparr.Contains(t.LPMT_Id) && t.LPMU_Id == data.LPMU_Id).ToArray();
                    foreach (SchoolMasterTopicUnitDMO ph1 in Phone_Noresultremove)
                    {
                        _context.Remove(ph1);
                    }

                    for (int k = 0; k < data.SchoolMasterUnitTopicMappingTempDTO.Count(); k++)
                    {
                        var checkduplicate = _context.SchoolMasterTopicUnitDMO.Where(a => a.LPMU_Id == data.LPMU_Id && a.LPMT_Id == data.SchoolMasterUnitTopicMappingTempDTO[k].LPMT_Id).ToList();

                        if (checkduplicate.Count() > 0)
                        {
                            var result = _context.SchoolMasterTopicUnitDMO.Single(a => a.LPMU_Id == data.LPMU_Id && a.LPMT_Id == data.SchoolMasterUnitTopicMappingTempDTO[k].LPMT_Id);
                            result.LPMTU_UpdatedBy = data.LPMU_CreatedBy;
                            result.UpdatedDate = DateTime.UtcNow;
                            _context.Update(result);
                        }
                        else
                        {
                            SchoolMasterTopicUnitDMO dmo = new SchoolMasterTopicUnitDMO();
                            dmo.LPMU_Id = data.LPMU_Id;
                            dmo.LPMT_Id = data.SchoolMasterUnitTopicMappingTempDTO[k].LPMT_Id;
                            dmo.LPMTU_CreatedBy = data.LPMU_CreatedBy;
                            dmo.CreatedDate = DateTime.UtcNow;
                            dmo.LPMTU_UpdatedBy = data.LPMU_CreatedBy;
                            dmo.UpdatedDate = DateTime.UtcNow;
                            dmo.LPMUT_ActiveFlag = true;
                            _context.Add(dmo);
                        }
                    }

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
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                data.message = "Add";
                data.returnval = false;
            }
            return data;
        }
        public SchoolMasterUnitDTO deactivatemapping(SchoolMasterUnitDTO data)
        {
            try
            {
                var checkresult = _context.SchoolMasterTopicUnitDMO.Single(a => a.LPMT_Id == data.LPMT_Id && a.LPMTU_Id == data.LPMTU_Id && a.LPMU_Id == data.LPMU_Id);
                if (checkresult.LPMUT_ActiveFlag == true)
                {
                    checkresult.LPMUT_ActiveFlag = false;
                }
                else
                {
                    checkresult.LPMUT_ActiveFlag = true;
                }
                checkresult.UpdatedDate = DateTime.Now;
                checkresult.LPMTU_UpdatedBy = data.LPMU_CreatedBy;
                _context.Update(checkresult);
                var i = _context.SaveChanges();
                if (i > 0)
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
