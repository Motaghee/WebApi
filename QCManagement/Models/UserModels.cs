using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QCManagement.Models
{
    public class UserModels
    {
        [Key]
        //[ReadOnly(true)]
        public Decimal? SRL { get; set; }  // AutoNumber with Seq.
        public Decimal USERID { get; set; }
        [MaxLength(15, ErrorMessage = "حداکثر طول مجاز برای نام 10 کاراکتر می باشد")]
        [StringLength(15)]
        public string FNAME { get; set; }
        public string LNAME { get; set; }
        [Required(ErrorMessage = "لطفا کد کاربری را وارد نمایید")]
        public string USERNAME { get; set; }
        [Required(ErrorMessage = "لطفا کلمه عبور را وارد نمایید")]
        [DataType(DataType.Password)]
        public string PSW { get; set; }
        public Decimal INUSE { get; set; }
        public Decimal SVADEPT_SRL { get; set; }
        public Decimal SVAACST_SRL { get; set; }
        //---
        private string TableName { get; set; } = null;
        public UserModels()
        {
            TableName = "QCUSERT";
        }

        public string GetTableName()
        {
            return TableName;
        }
        //---
        public object getMyObjectOfThis(UserModels inputUserModels)
        {
            return (object)inputUserModels;
        }
    }
}