using DTO.DTO;
using DTO.Interface;
using InoviDataAccessLayer.EntityModel;
using InoviDataTransferObject.DTO;
using Microsoft.EntityFrameworkCore;

namespace InoviServices.Repo
{
    public class UserRepo : IUserRepo
    {

        private QueryProContext _context;
        ResponseDTO resp = new ResponseDTO();
        public UserRepo(QueryProContext context)
        {
            _context = context;
        }

        public async Task<bool> SignupUser(SignupDTO req)
        {
            try
            {
                var isExist = _context.TblUsers.Where(x => x.EmailAddress == req.UserEmail).FirstOrDefault();
                if (isExist != null)
                {
                    return false;
                }
                else
                {
                    string password = req.UserPassword;
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);


                    TblUser tblReq = new TblUser
                    {
                        Name = req.Name,
                        Username = req.Username,
                        Password = hashedPassword,
                        EmailAddress = req.UserEmail,
                        UserRoleId = 3,
                        IsActive = true,
                        CreatedOn = DateTime.Now
                    };
                    _context.TblUsers.Add(tblReq);
                    _context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<JWT> LoginUser(LoginDTO req)
        {
            try
            {
                var isExist = _context.TblUsers.Where(x => x.EmailAddress == req.UserEmail).FirstOrDefault();
                if (isExist != null)
                {
                    string storedHashedPassword = isExist.Password;

                    if (BCrypt.Net.BCrypt.Verify(req.UserPassword, storedHashedPassword))
                    {
                        if (isExist.UserRoleId != null && isExist.UserRoleId != 0)
                        {
                            var result = new JWT
                            {
                                Email = isExist.EmailAddress,
                                Name = isExist.Name,
                                UserID = isExist.UserId.ToString(),
                                RoleID = isExist.UserRoleId.ToString()
                            };
                            return result;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<string> SendOTP(SendOTPDTO req)
        {
            try
            {
                var isExist = _context.TblUsers.Where(x => x.EmailAddress == req.UserEmail).FirstOrDefault();
                if (isExist == null)
                {
                    return null;
                }
                else
                {
                    return isExist.EmailAddress;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> ValidateOTP(ValidateOTPDTO req)
        {
            try
            {
                var isExist = _context.TblUsers.Where(x => x.EmailAddress == req.UserEmail).FirstOrDefault();
                if (isExist != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<bool> UpdateUserPassword(UpdateUserPasswordDTO req)
        {
            try
            {
                var isExist = _context.TblUsers.Where(x => x.EmailAddress == req.UserEmail).FirstOrDefault();
                if (isExist == null)
                {
                    return false;
                }
                else
                {
                    isExist.Password = req.UserPassword;
                    _context.Update<TblUser>(isExist);
                    _context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<GetUserDTO> GetUserInfo(int UserID)
        {
            try
            {
                return await _context.TblUsers
                .Where(s => s.UserId == UserID)
                .Select(
                    s => new GetUserDTO { 
                        UserID = s.UserId, 
                        RoleID = s.UserRoleId, 
                        Name = s.Name, 
                        Email = s.EmailAddress })
                .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public Task<ProfilePicDTO> UploadImage(ProfilePicDTO req)
        //{
        //    try
        //    {
        //        var isExist = _context.TblAttachments.Where(x => x.AttachmentLinkId == req.AttachmentLinkId).FirstOrDefault();
        //        if (isExist != null)
        //        {
        //            return null;
        //        }
        //        else
        //        {
        //            TblAttachment tblReq = new TblAttachment
        //            {
        //                Path = req.Path,
        //                Filename = req.FileName,
        //                AttachmentLink = req.AttachmentLink
        //            };
        //            tblReq = _context.TblAttachments.Add(tblReq).Entity;
        //            _context.SaveChanges();
        //            req.AttachmentLinkId = tblReq.AttachmentLinkId;
        //            return req;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }


}