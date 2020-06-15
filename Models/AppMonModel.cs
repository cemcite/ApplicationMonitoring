using System;
using System.ComponentModel.DataAnnotations;

namespace NuevoSoftware.ApplicationMonitoring.Models
{
    public class AppMonModel : MainModel
    {
        public int ID { get; set; }
        [Required] 
        public string Name { get; set; }
        [Required] 
        public string URL { get; set; }
        [Required] 
        public int Interval { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
