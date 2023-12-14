using InoviDataAccessLayer.EntityModel;
using InoviDataTransferObject.DTO;
using InoviDataTransferObject.IRepo;
using Microsoft.EntityFrameworkCore;
//using static System.Net.Mime.MediaTypeNames;
using System.Net.Http;


namespace InoviServices.Repo
{
    public class QueryRepo : IQueryRepo
    {
        private QueryProContext _context;

        public QueryRepo(QueryProContext context)
        {
            _context = context;
        }

        public void AddQuery(AddQueryDTO req)
        {
            try
            {
                TblQuery tblreq = new TblQuery
                {
                    Description = req.Description,

                    //TblAttachments = new TblAttachment
                    //{
                    //    AttachmentLinkId = req.AttachmentLinkId
                    //}
                };
                _context.TblQueries.Add(tblreq);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<GetQueryDTO>> GetQueryList()
        {
            try
            {
                return await _context.TblQueries
                    .Where(s => s.IsActive == true)
                    .Select(s => new GetQueryDTO { QueryId = s.QueryId, Title = s.Title, Description = s.Description ,CurrentStatus = s.CurrentStatus}).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<List<AttachmentLinkList>> UploadImage()
        {
            throw new NotImplementedException();
        }

        //public static bool SaveImage( string ImgStr, string ImgName)
        //{
        //    System.Net.Mime.MediaTypeNames.Image image = Base64ToImage(ImgStr);
        //    String path = HttpContext.Current.Server.MapPath("~/ImageStorage"); //Path
        //                                                                        //Check if directory exist
        //    if (!System.IO.Directory.Exists(path))
        //    {
        //        System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
        //    }
        //    string imageName = ImgName + ".jpg";
        //    //set the image path
        //    string imgPath = Path.Combine(path, imageName);
        //    image.Save(imgPath, System.Drawing.Imaging.ImageFormat.Jpeg);
        //    return true;
        //}
        //public static Image Base64ToImage(string base64String)
        //{
        //    // Convert base 64 string to byte[]
        //    byte[] imageBytes = Convert.FromBase64String(base64String);
        //    // Convert byte[] to Image
        //    using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
        //    {
        //        Image image = Image.FromStream(ms, true);
        //        return image;
        //    }
        //}

        //public Task<List<AttachmentLinkList>> UploadImage(string ImgStr, string ImgName)
        //{
        //    Image image = SAWHelpers.Base64ToImage(ImgStr);
        //    String path = HttpContext.Current.Server.MapPath("~/ImageStorage"); //Path
        //                                                                        //Check if directory exist
        //    if (!System.IO.Directory.Exists(path))
        //    {
        //        System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
        //    }
        //    string imageName = ImgName + ".jpg";
        //    //set the image path
        //    string imgPath = Path.Combine(path, imageName);
        //    image.Save(imgPath, System.Drawing.Imaging.ImageFormat.Jpeg);
        //    return true;
        //}
    }
}
