using System;
using System.Collections.Generic;

namespace NuevoSoftware.ApplicationMonitoring.Data
{
    public partial class NsusersT
    {
        public NsusersT()
        {
            NsapplicationsT = new HashSet<NsapplicationsT>();
        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Mail { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<NsapplicationsT> NsapplicationsT { get; set; }
    }
}
