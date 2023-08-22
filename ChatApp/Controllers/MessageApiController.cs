using ChatApp.Content;
using ChatApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace ChatApp.Controllers
{
    public class MessageApiController : ApiController
    {
        HttpClient httpClient;
        public MessageApiController()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:44363/api/"); // Replace with the API base URL
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        // GET api/<controller>
        [HttpGet]
        [Route("api/messageApi/getAllMessages")]
        public IHttpActionResult getAllMessages()
        {
            List<MessageModel> messages = new List<MessageModel>();
            using (ChatAppDBContext dbContext = new ChatAppDBContext())
            {
                try
                {
                    messages = dbContext.messageDBSet.OrderBy(item => item.timestamp).ToList();
                }
                catch
                {
                    return InternalServerError();
                }
            }
            return Ok(messages);
        }

        // GET api/<controller>/5
        [HttpGet]
        [Route("api/messageApi/getUserMessages/{userId}/")]
        public IHttpActionResult getUserMessages(string userId)
        {
            if (userId == null)
                return BadRequest(message: "User Id is null");

            List<MessageModel> messages = new List<MessageModel>();
            using (ChatAppDBContext dbContext = new ChatAppDBContext())
            {
                try
                {
                    messages = dbContext.messageDBSet.Where(y => y.senderId == userId || y.recieverId == userId).OrderBy(item => item.timestamp).ToList();
                }
                catch
                {
                    return InternalServerError();
                }
            }
            return Ok(messages);
        }

        // GET api/<controller>/5
        [HttpGet]
        [Route("api/messageApi/getChatMessages/{chatId}/")]
        public IHttpActionResult getChatMessages(string chatId)
        {
            if (chatId == null)
                return BadRequest(message: "Chat Id is null");

            List<MessageModel> messages = new List<MessageModel>();
            using (ChatAppDBContext dbContext = new ChatAppDBContext())
            {
                try
                {
                    if(dbContext.chatDBSet.Where(ch => ch.Id == chatId).FirstOrDefault() == null)
                        return BadRequest(message: "Chat doesn't exist");
                    messages = dbContext.messageDBSet.Where(y => y.chatId == chatId).OrderBy(item => item.timestamp).ToList();
                }
                catch
                {
                    return InternalServerError();
                }
            }
            return Ok(messages);
        }

        // POST api/values
        [HttpPost]
        [Route("api/messageApi/createMessage/")]
        /* list string -> [chatId, senderId, receiverId, text] */
        public IHttpActionResult createMessage([FromBody] List<string> strings)
        {

            if (strings == null)
                return BadRequest(message: "Not enough details...");

            foreach (string s in strings)
            {
                if (s == null)
                    return BadRequest(message: "Got an invalid Id or text");

            }
            if (strings[1] == strings[2])
                return BadRequest(message: "sender and receiver ids are same....");


            MessageModel newMessage = new MessageModel();
            using (ChatAppDBContext dbContext = new ChatAppDBContext())
            {
                string res = verifyStrings(strings[0], strings[1], strings[2]);
                if (res != null)
                    return BadRequest(message: res);

                do
                {
                    newMessage.Id = IdGenerator.generate();
                } while (dbContext.messageDBSet.Where(y => y.Id == newMessage.Id).FirstOrDefault() != null);

                newMessage.chatId = strings[0];
                newMessage.senderId = strings[1];
                newMessage.recieverId = strings[2];
                newMessage.text = strings[3];
                newMessage.timestamp = (DateTime)DateTime.Now;

                try
                {
                    dbContext.messageDBSet.Add(newMessage);
                    dbContext.SaveChanges();
                }
                catch
                {
                    return InternalServerError();
                }
            }
            return Redirect($"https://localhost:44363/api/messageApi/getChatMessages/{newMessage.chatId}/");
        }

        [HttpPost]
        [Route("api/messageApi/createMessageThroughWeb/")]
        /* list string -> [chatId, senderId, receiverId, text] */
        public IHttpActionResult createMessageThroughWeb([FromBody] List<string> strings)
        {

            if (strings == null)
                return BadRequest(message: "Not enough details...");

            foreach (string s in strings)
            {
                if (s == null)
                    return BadRequest(message: "Got an invalid Id or text");

            }
            if (strings[1] == strings[2])
                return BadRequest(message: "sender and receiver ids are same....");


            MessageModel newMessage = new MessageModel();
            using (ChatAppDBContext dbContext = new ChatAppDBContext())
            {
                string res = verifyStrings(strings[0], strings[1], strings[2]);
                if (res != null)
                    return BadRequest(message: res);

                do
                {
                    newMessage.Id = IdGenerator.generate();
                } while (dbContext.messageDBSet.Where(y => y.Id == newMessage.Id).FirstOrDefault() != null);

                newMessage.chatId = strings[0];
                newMessage.senderId = strings[1];
                newMessage.recieverId = strings[2];
                newMessage.text = strings[3];
                newMessage.timestamp = (DateTime)DateTime.Now;

                try
                {
                    dbContext.messageDBSet.Add(newMessage);
                    dbContext.SaveChanges();
                }
                catch
                {
                    return InternalServerError();
                }
            }
            return Redirect($"https://localhost:44363/api/messageApi/getChatMessages/{newMessage.chatId}/");
        }





        private string verifyStrings(string chatId, string senderId, string recieverId)
        {
            using (ChatAppDBContext dBContext = new ChatAppDBContext())
            {


                ChatModel c =  dBContext.chatDBSet.Where(ch => ch.Id == chatId).FirstOrDefault(); ;
                UserModel u1 = dBContext.userDBSet.Where(us => us.Id == senderId).FirstOrDefault();  
                UserModel u2 = dBContext.userDBSet.Where(us => us.Id == recieverId).FirstOrDefault();

                if (c == null || u1 == null || u2 == null)
                    return "Either sender or receiver or chat doesn't exist in database";

                if (u1.Id != c.user1 && u1.Id != c.user2)
                    return "user1 is not in chat";

                if (u2.Id != c.user1 && u2.Id != c.user2)
                    return "user2 is not in chat";
            }
            return null;
        }
    }
}
