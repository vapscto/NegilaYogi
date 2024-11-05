using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Canteen
{

    public class FooditeamDTO
    {

        public long MI_Id { get; set; }

        public long AMST_Id { get; set; }
        public long AMCSTW_Id { get; set; }
        public long CMMFI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long CMTRANS_Id { get; set; }
        public long UserId { get; set; }
        public long ICAI_Id { get; set; }
        public string AMCST_WalletPIN { get; set; }
        public Array Fooditeam { get; set; }

        public Array FooditeamDeatils { get; set; }
        public Array categeorylist { get; set; }
        public Array ImageDetails { get; set; }
        public Array padamountdeatils { get; set; }
        public string CMMFI_FoodItemName { get; set; }
        public string CMMFI_FoodItemDescription { get; set; }
        public string CMMCA_CategoryName { get; set; }
        public decimal CMMFI_UnitRate { get; set; }
        public bool CMMFI_OutofStockFlg { get; set; }
        public string returnval { get; set; }

        public string AMCSTW_WalletPIN { get; set; }

        public string amst_emailid { get; set; }
        public string CMMFI_PathURL { get; set; }
        public Array GridviewDetails { get; set; }
        public long? CMMCA_Id { get; set; }
        public bool CMMFI_ActiveFlg { get; set; }
        public bool CMMFI_FoodItemFlag { get; set; }
        public string ICAI_Attachment { get; set; }
        public string message { get; set; }
        public string ICAI_FileName { get; set; } 
        public Array studentpinlist { get; set; }
        public FooditeamDTO[] attachementlist { get; set; }
        public CMMFI_Id_FilePath_Arrays[] CMMFI_Id_FilePath_Array { get; set; }
        public class CMMFI_Id_FilePath_Arrays
        {
            public string IHW_FilePath { get; set; }
            public string FileName { get; set; }
        }




    }
}
