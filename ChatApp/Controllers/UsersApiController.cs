using ChatApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ChatApp.Controllers
{
    
    public class UsersApiController : ApiController
    {
        [HttpGet]
        [Route("api/getAllUsers/")]
        public IHttpActionResult getAllUsers()
        {
            List<UserModel> users = new List<UserModel>();
            using (ChatAppDBContext dbContext = new ChatAppDBContext())
            {
                try
                {
                    users = dbContext.userDBSet.ToList();
                }
                catch
                {
                    return InternalServerError();
                }
            }
            return Ok(users);
        }

        [HttpGet]
        [Route("api/getUser/{email_id}/")]
        public IHttpActionResult getUser(string email_id)
        {

            email_id = email_id.ToUpper();
            UserModel user = new UserModel();
            using (ChatAppDBContext dbContext = new ChatAppDBContext())
            {
                try
                {
                    user = dbContext.userDBSet.Where(u => u.email_Id == email_id).SingleOrDefault();
                }
                catch
                {
                    return InternalServerError();
                }
            return Ok(user);
            }
        }


        [HttpGet]
        [Route("api/getUserObjectById/{id}/")]
        public IHttpActionResult getUserObjectById(string id)
        {

            UserModel user = new UserModel();
            using (ChatAppDBContext dbContext = new ChatAppDBContext())
            {
                try
                {
                    user = dbContext.userDBSet.Where(u => u.Id == id).SingleOrDefault();
                }
                catch
                {
                    return InternalServerError();
                }
                return Ok(user);
            }
        }


        [HttpGet]
        [Route("api/getAllExcept/{id}/")]
        public IHttpActionResult getAllExcept(string id)
        {
            List<UserModel> users = new List<UserModel>();
            using (ChatAppDBContext dbContext = new ChatAppDBContext())
            {
                try
                {
                    users = dbContext.userDBSet.Where(u => u.Id != id).ToList();
                }
                catch
                {
                    return InternalServerError();
                }
                return Ok(users);
            }
        }

        [HttpGet]
        [Route("api/getAllUserIds/")]
        public IHttpActionResult getAllUserIds()
        {
            List<DisplayThings> display;
            
            using (ChatAppDBContext dbContext = new ChatAppDBContext())
            {
                try
                {
                    display = dbContext.userDBSet.Select(u => new DisplayThings{ 
                                user = new DisplayThings.User() { userId = u.Id, userName = u.name }
                        }).ToList();
                }
                catch
                {
                    return InternalServerError();
                }
            }
            
            return Ok(display);
        }

        [HttpGet]
        [Route("api/getUserByEmailId/{email_id}/")]
        public IHttpActionResult getUserByEmailId(string email_id)
        {

            email_id = email_id.ToUpper();
            UserModel user = new UserModel();
            DisplayThings display = new DisplayThings();
            using (ChatAppDBContext dbContext = new ChatAppDBContext())
            {
                try
                {
                    user = dbContext.userDBSet.Where(u => u.email_Id == email_id).SingleOrDefault();
                    display.user.userName = user.name;
                    display.user.userId = user.Id;
                }
                catch
                {
                    return InternalServerError();
                }
                return Ok(display);
            }
        }

        [HttpGet]
        [Route("api/getUserById/{id}/")]
        public IHttpActionResult getUserById(string id)
        {
            UserModel user = new UserModel();
            DisplayThings display = new DisplayThings();
            using (ChatAppDBContext dbContext = new ChatAppDBContext())
            {
                try
                {
                    user = dbContext.userDBSet.Where(u => u.Id== id).SingleOrDefault();
                    display.user.userName = user.name;
                    display.user.userId = user.Id;
                }
                catch
                {
                    return InternalServerError();
                }
                return Ok(display);
            }
        }


        [HttpGet]
        [Route("api/getAllIdsExcept/{id}/")]
        public IHttpActionResult getAllIdsExcept(string id)
        {
            List<DisplayThings> userIds;
            using (ChatAppDBContext dbContext = new ChatAppDBContext())
            {
                try
                {
                    userIds = dbContext.userDBSet.Where(
                        u => u.Id != id).Select(u => new DisplayThings {
                            user = new DisplayThings.User() { userId = u.Id, userName = u.name }
                        }
                        ).ToList();
                }
                catch
                {
                    return InternalServerError();
                }
                return Ok(userIds);
            }
        }

    }
}