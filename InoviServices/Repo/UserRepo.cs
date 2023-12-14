using DTO.DTO;
using DTO.Interface;
using InoviDataAccessLayer.EntityModel;
using InoviDataTransferObject.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

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
                    TblUser tblReq = new TblUser
                    {
                        Name = req.Name,
                        Username = req.Username,
                        Password = req.UserPassword,
                        EmailAddress = req.UserEmail

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
        public async Task<bool> LoginUser(LoginDTO req)
        {
            try
            {
                var isExist = _context.TblUsers.Where(x => x.EmailAddress == req.UserEmail).FirstOrDefault();
                if (isExist != null)
                {
                    if (isExist.Password == req.UserPassword)
                    {
                        if (isExist.UserRoleId != null && isExist.UserRoleId != 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
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


    }


}