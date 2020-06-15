using System;
using System.Collections.Generic;

namespace NuevoSoftware.ApplicationMonitoring.Data
{
    public partial class NsapplicationsT
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public int Interval { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual NsusersT User { get; set; }
    }
}
