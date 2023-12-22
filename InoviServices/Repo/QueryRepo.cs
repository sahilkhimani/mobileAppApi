using InoviDataAccessLayer.EntityModel;
using InoviDataTransferObject.DTO;
using InoviDataTransferObject.IRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;


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

        public async Task<bool> AddQuery(AddQueryDTO req)
        {
            try
            {
                TblQuery tblreq = new TblQuery
                {
                    Title = req.Title,
                    Description = req.Description,
                    CurrentStatus = EStatus.Pending.ToString(),
                    CreatedBy = req.UserID,
                    CreatedOn = DateTime.Now.ToString("dd/MM/yyyy - hh:mm:ss"),
                    UserId = req.UserID,

                    TblQueryAttachments = req.AttachmentIds.Select(s => new TblQueryAttachment
                    {
                        AttachmentLinkId = s.AttachmentLinkId
                    }).ToList()
                };

                 var result =_context.TblQueries.Add(tblreq).Entity;
                _context.SaveChanges();
                _context.TblQueryStatuses.Add(new TblQueryStatus
                {
                    QueryId = result.QueryId,
                    StatusId = (int)EStatus.Pending,
                    IsActive = true,
                    CreatedBy = req.UserID,
                    CreatedOn = DateTime.Now
                });
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<GetQueryDTO>> GetQueryList(GetUserIDAndRoleID req)
        {
            try
            {
                List<GetQueryDTO> queryResults;

                if (req.RoleID == 3)
                {
                    var query = await _context.TblQueries
                        .Where(s => s.IsActive == true && s.UserId == req.UserID)
                        .OrderByDescending(x => x.QueryId)
                        .ToListAsync();

                    queryResults = query.Select(s => new GetQueryDTO
                    {
                        QueryId = s.QueryId,
                        Title = s.Title,
                        Description = s.Description,
                        CurrentStatus = s.CurrentStatus,
                        AttachmentPaths = _context.TblQueries.Include(x => x.TblQueryAttachments)
                            .Where(x => x.IsActive == true && x.UserId == req.UserID)
                            .Select(x => x.TblQueryAttachments
                                .Select(qa => qa.AttachmentLink.Path)
                                .ToList()).FirstOrDefault()
                    }).OrderByDescending(x=>x.QueryId).ToList();
                }
                else
                {
                    var query = await _context.TblQueries
                        .Where(s => s.IsActive == true)
                        .OrderByDescending(x => x.QueryId)
                        .ToListAsync();

                    queryResults = query.Select(s => new GetQueryDTO
                    {
                        QueryId = s.QueryId,
                        Title = s.Title,
                        Description = s.Description,
                        CurrentStatus = s.CurrentStatus,
                        AttachmentPaths = _context.TblQueryAttachments
                            .Where(qa => qa.QueryId == s.QueryId)
                            .Select(qa => qa.AttachmentLink.Path)
                            .ToList()
                    }).OrderByDescending(x => x.QueryId).ToList();
                }

                return queryResults;
            }
            catch (Exception ex)
            {
                // Log or handle the exception appropriately
                throw;
            }
        }


        public async Task<bool> UpdateQuery(UpdateStatus req)
        {
            try
            {
                var query = _context.TblQueries.Where(x => x.QueryId == req.QueryID).FirstOrDefault();
                query.CurrentStatus = req.Status;
                query.ModifiedBy = req.UserID;
                query.ModifiedOn = DateTime.Now;
                _context.TblQueries.Update(query);
                _context.TblQueryStatuses.Add(new TblQueryStatus { 
                    QueryId = req.QueryID,
                    StatusId= (int)req.StatusID,
                    IsActive = true,
                    CreatedBy = req.UserID,
                    CreatedOn = DateTime.Now
                });
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        public async Task<AttachmentLinkList> UploadImage(AttachmentLinkList req)
        {
            try
            {
                var isExist = _context.TblAttachments.Where(x => x.AttachmentLinkId == req.AttachmentLinkId).FirstOrDefault();
                if (isExist != null)
                {
                    return null;
                }
                else
                {
                    TblAttachment tblReq = new TblAttachment
                    {
                        Path = req.Path,
                        Filename = req.FileName,
                        AttachmentLink = req.AttachmentLink
                    };
                    tblReq = _context.TblAttachments.Add(tblReq).Entity;
                    _context.SaveChanges();
                    req.AttachmentLinkId = tblReq.AttachmentLinkId;
                    return req;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
