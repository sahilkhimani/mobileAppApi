using InoviDataTransferObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InoviDataTransferObject.IRepo
{
    public interface IQueryRepo
    {
        Task<List<GetQueryDTO>> GetQueryList();
        void AddQuery(AddQueryDTO req);
        Task<List<AttachmentLinkList>> UploadImage();
    }
}
