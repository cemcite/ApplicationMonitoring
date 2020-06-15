using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NuevoSoftware.ApplicationMonitoring.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class UserModel : MainModel
    {
        /// <summary>
        /// 
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Username { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Password { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ReEnterPassword { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Mail { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<AppMonModel> ApplicationList { get; set; }
    }
}
