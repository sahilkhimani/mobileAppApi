using System;
using System.Collections.Generic;

namespace InoviDataAccessLayer.EntityModel
{
    public partial class TblAttachment
    {
        public int AttachmentLinkId { get; set; }
        public int? Path { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedById { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedById { get; set; }

        public virtual TblQuery? PathNavigation { get; set; }
    }
}
