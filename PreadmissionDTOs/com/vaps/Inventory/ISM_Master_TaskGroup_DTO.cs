using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Inventory
{
    public class ISM_Master_TaskGroup_DTO
    {
        public long ISMMTGRP_Id { get; set; }
        public long MI_Id { get; set; }
        public string ISMMTGRP_TaskGroupName { get; set; }
        public long ISMMTGRP_CreatedByDeptId { get; set; }
        public string ISMMTGRP_TGRemarks { get; set; }
        public bool ISMMTGRP_TGFinishedFlg { get; set; }
        public bool ISMMTGRP_ActiveFlag { get; set; }
        public long ISMMTGRP_CreatedBy { get; set; }
        public long ISMMTGRP_UpdatedBy { get; set; }
        public DateTime ISMMTGRP_CreatedDate { get; set; }
        public DateTime ISMMTGRP_UpdatedDate { get; set; }



        public long UserId { get; set; }
        public long HRMD_Id { get; set; }
        public long ISMTCR_Id { get; set; }
        public long ISMMPR_Id { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string ISMTCR_Title { get; set; }
        public string ISMTCR_TaskNo { get; set; }
        public string ISMTCR_Desc { get; set; }
        public string returnval { get; set; }
        public string ISMMPR_ProjectName { get; set; }
        public Array get_groupname { get; set; }
        public Array task_edit { get; set; }
        public Array task_edit_dept { get; set; }
        public Array dept_list { get; set; }
        public Array task_list_temp { get; set; }
        public Array task_list { get; set; }
        public Array task_grop_list { get; set; }
        public Array task_view_list { get; set; }
        public deptarray1[] deptarray { get; set; }
        public task_listarray1[] task_listarray { get; set; }


        public class deptarray1
        {
            public long HRMD_Id { get; set; }
        }
        public class task_listarray1
        {
            public long HRMD_Id { get; set; }
            public long ISMTCR_Id { get; set; }
            public int? ISMTCR_TGOrder { get; set; }
        }
    }
}
