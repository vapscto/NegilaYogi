using CommonLibrary;
using PreadmissionDTOs.com.vaps.Fees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVRMUX.Delegates.com.vapstech.Fees
{
    public class CategoryConcessionGroupMappingDelegate
    {

        CommonDelegate<CategoryConcessionGroupMappingDTO, CategoryConcessionGroupMappingDTO> comm = new CommonDelegate<CategoryConcessionGroupMappingDTO, CategoryConcessionGroupMappingDTO>();
        public CategoryConcessionGroupMappingDTO loaddata(CategoryConcessionGroupMappingDTO data)
        {
            return comm.POSTDatafee(data, "CategoryConcessionGroupMappingFacade/loaddata/");
        }
        public CategoryConcessionGroupMappingDTO gethead(CategoryConcessionGroupMappingDTO data)
        {
            return comm.POSTDatafee(data, "CategoryConcessionGroupMappingFacade/gethead");
        }
        public CategoryConcessionGroupMappingDTO getconcession(CategoryConcessionGroupMappingDTO data)
        {
            return comm.POSTDatafee(data, "CategoryConcessionGroupMappingFacade/getconcession");
        }
        public CategoryConcessionGroupMappingDTO save(CategoryConcessionGroupMappingDTO data)
        {
            return comm.POSTDatafee(data, "CategoryConcessionGroupMappingFacade/save");
        }
        public CategoryConcessionGroupMappingDTO deactiveStudent(CategoryConcessionGroupMappingDTO data)
        {
            return comm.POSTDatafee(data, "CategoryConcessionGroupMappingFacade/deactiveStudent");
        }
        public CategoryConcessionGroupMappingDTO EditData(CategoryConcessionGroupMappingDTO data)
        {
            return comm.POSTDatafee(data, "CategoryConcessionGroupMappingFacade/EditData");
        }
          public CategoryConcessionGroupMappingDTO getgroup(CategoryConcessionGroupMappingDTO data)
        {
            return comm.POSTDatafee(data, "CategoryConcessionGroupMappingFacade/getgroup");
        }



    }
}