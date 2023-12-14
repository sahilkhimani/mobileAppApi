using DTO.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoviDataTransferObject.DTO
{
    public class GetQueryDTO
    {
        public int QueryId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CurrentStatus { get; set; }
    }

    public class AddQueryDTO
    {

        public string Description { get; set; }
        public int  AttachmentLinkId { get; set; }
    }
    public class AttachmentLinkList
    {
        public int AttachmentLinkId { get; set; }
        public string FileName { get; set; }
        public string AttachmentLink { get; set; }
        public string Path { get; set; }
    }
}
