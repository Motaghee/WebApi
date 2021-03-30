using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Common.Models.QccasttModels;

namespace QCManagement.Models
{
    public class CarModels
    {
        [Key]
        //[ReadOnly(true)]
        //[RegularExpression(@"NAS\d{6}\w{1}\d{7}", ErrorMessage = "قالب شماره شاسی رعایت نشده است")]
        [MinLength(17, ErrorMessage = "شماره شاسی باید 17 کاراکتر باشد")]
        [MaxLength(17, ErrorMessage = "شماره شاسی باید 17 کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا شماره شاسی را وارد نمایید")]
        public string Vin { get; set; }

        public int id { get; set; } = 1;

        public List<Pcshopt> Lstshop { get; set; }

        public string GetVin()
        {
            return Vin;
        }

        public void SetVin(string _vin)
        {
            Vin = _vin;
        }
        //---
        public object getMyObjectOfThis(CarModels inputModels)
        {
            return (object)inputModels;
        }



    }
}

