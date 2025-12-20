using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Domain.Enum
{
    public class EntityEnum
    {
        public enum Status
        {
            Published,
            Draft
        }
        public enum ReadingDuration
        {
            [Display(Name = "5 Minutes")]
            Minutes5,

            [Display(Name = "10 Minutes")]
            Minutes10,

            [Display(Name = "15 Minutes")]
            Minutes15,

            [Display(Name = "20 Minutes")]
            Minutes20,

            [Display(Name = "25 Minutes")]
            Minutes25,

            [Display(Name = "30 Minutes")]
            Minutes30,

            [Display(Name = "35 Minutes")]
            Minutes35,

            [Display(Name = "40 Minutes")]
            Minutes40,

            [Display(Name = "45 Minutes")]
            Minutes45,

            [Display(Name = "50 Minutes")]
            Minutes50,

            [Display(Name = "55 Minutes")]
            Minutes55,

            [Display(Name = "1 Hour")]
            OneHour,

            [Display(Name = "1 Hour +")]
            OneHourPlus
        }
    }
}
