using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ChatApp.Models;
using ChatApp.Content;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ChatApp.Controllers
{
    public class ChatApiController : ApiController
    {

        private readonly HttpClient httpClient;
        public ChatApiController()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:44363/api/"); // Replace with the API base URL
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        // GET api/<controller>
        [HttpGet]
        [Route("api/chatApi/getAllChats")]
        public IHttpActionResult getAllChats()
        {
            List<ChatModel> chats = new List<ChatModel>();
            using (ChatAppDBContext dbContext = new ChatAppDBContext())
            {
                try
                {
                    chats = dbContext.chatDBSet.OrderBy(item => item.timestamp).ToList();
                }
                catch
                {
                    return InternalServerError();
                }
            }
            return Ok(chats);
        }

        // GET api/<controller>/5
        [HttpGet]
        [Route("api/chatApi/getUserChats/{userId}/")]
        public IHttpActionResult getUserChats(string userId)
        {
            if(userId == null)
                return BadRequest(message: "User Id is null");
            
            List<ChatModel> chats = new List<ChatModel>();
            using (ChatAppDBContext dbContext = new ChatAppDBContext()) {
                try
                {
                    chats = dbContext.chatDBSet.Where(y => y.user1 == userId || y.user2 == userId).OrderBy(item => item.timestamp).ToList();
                }
                catch
                {
                    return InternalServerError();
                }
            }
            return Ok(chats);
        }

        // POST api/values
        [HttpPost]
        [Route("api/chatapi/createChat/")]
        public async Task<IHttpActionResult> createChat([FromBody] List<string> users)
        {
            
            if (users == null)
                return BadRequest(message: "No users in a chat...");
            
            foreach (string user in users)
            {
                if (user == null)
                    return BadRequest(message: "Got an invalid user Id");
                
            }
            if(users[0] == users[1])
                return BadRequest(message: "chat with same user....");
            

            ChatModel chat = new ChatModel();
            using (ChatAppDBContext dbContext = new ChatAppDBContext())
            {
                string s1 = users[0];
                string s2 = users[1];
                ChatModel temp = dbContext.chatDBSet.Where(
                    y=>( (y.user1 == s1 || y.user1 == s2) && 
                         (y.user2 == s1 || y.user2 == s2)
                )).FirstOrDefault();


                if (temp != null)
                    return BadRequest(message: "chat between these users already exists ....");

              
                UserModel user1 = new UserModel();
                UserModel user2 = new UserModel();

                string apiUrl = $"getUserObjectById/{s1}//"; // Replace with the actual API endpoint
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response content to get the employee details
                    user1 = await response.Content.ReadAsAsync<UserModel>();

                }
                else
                {
                    return BadRequest(message: "Either user not found....");
                }

                apiUrl = $"getUserObjectById/{s2}//"; // Replace with the actual API endpoint
                response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response content to get the employee details
                    user2 = await response.Content.ReadAsAsync<UserModel>();

                }
                else
                {
                    return BadRequest(message: "Either user not found....");
                }


                do
                {
                    chat.Id = IdGenerator.generate();
                } while (dbContext.chatDBSet.Where(y => y.Id == chat.Id).FirstOrDefault() != null);
                
                chat.user1 = user1.Id;
                chat.user2 = user2.Id;
                chat.user1name = user1.name;
                chat.user2name = user2.name;
                chat.timestamp = (DateTime)DateTime.Now;

                try
                {
                    dbContext.chatDBSet.Add(chat);
                    dbContext.SaveChanges();
                }
                catch
                {
                    return InternalServerError();
                }
            }
            return Ok(chat);
        }


        // POST api/values
        [HttpPost]
        [Route("api/chatapi/createChatId/")]
        public async Task<IHttpActionResult> createChatId([FromBody] List<string> users)
        {

            if (users == null)
                return BadRequest(message: "No users in a chat...");

            foreach (string user in users)
                if (user == null)
                    return BadRequest(message: "Got an invalid user Id");

            if (users[0] == users[1])
                return BadRequest(message: "chat with same user....");

            
            ChatModel chat = new ChatModel();
            using (ChatAppDBContext dbContext = new ChatAppDBContext())
            {
                string s1 = users[0];
                string s2 = users[1];
                ChatModel temp = dbContext.chatDBSet.Where(
                    y => ((y.user1 == s1 || y.user1 == s2) &&
                         (y.user2 == s1 || y.user2 == s2)
                )).FirstOrDefault();


                if (temp != null)
                    return Redirect($"https://localhost:44363/api/chatApi/getUserChatIds/{users[0]}/");

                UserModel user1 = new UserModel();
                UserModel user2 = new UserModel();

                string apiUrl = $"getUserObjectById/{s1}//"; // Replace with the actual API endpoint
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                    // Deserialize the response content to get the employee details
                    user1 = await response.Content.ReadAsAsync<UserModel>();

                else
                    return BadRequest(message: "Either user not found....");


                apiUrl = $"getUserObjectById/{s2}//"; // Replace with the actual API endpoint
                response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                    // Deserialize the response content to get the employee details
                    user2 = await response.Content.ReadAsAsync<UserModel>();

                else
                    return BadRequest(message: "Either user not found....");


                do
                {
                    chat.Id = IdGenerator.generate();
                } while (dbContext.chatDBSet.Where(y => y.Id == chat.Id).FirstOrDefault() != null);

                chat.user1 = user1.Id;
                chat.user2 = user2.Id;
                chat.user1name = user1.name;
                chat.user2name = user2.name;
                chat.timestamp = (DateTime)DateTime.Now;

                try
                {
                    dbContext.chatDBSet.Add(chat);
                    dbContext.SaveChanges();
                }
                catch
                {
                    return InternalServerError();
                }
            }
            
            return Redirect($"https://localhost:44363/api/chatApi/getUserChatIds/{users[0]}/");
        }


        [HttpGet]
        [Route("api/chatApi/getUserChatIds/{userId}/")]
        public IHttpActionResult getUserChatIds(string userId)
        {
            if (userId == null)
                return BadRequest(message: "User Id is null");

            List<DisplayThings> chatIds = new List<DisplayThings>();
            using (ChatAppDBContext dbContext = new ChatAppDBContext())
            {
                try
                {
                    chatIds = dbContext.chatDBSet.Where(y => y.user1 == userId || y.user2 == userId).Select(
                        ch => new DisplayThings {
                            chat = new DisplayThings.Chat() 
                            { 
                                chatId = ch.Id, 
                                personName = (userId != ch.user1)? ch.user1name : ch.user2name,
                                receiverId = (userId != ch.user1)? ch.user1 : ch.user2,
                                timestamp = ch.timestamp
                            }
                        }
                        ).OrderBy(item => item.chat.timestamp).ToList();
                }
                catch
                {
                    return InternalServerError();
                }
            }
            return Ok(chatIds);
        }

        [HttpGet]
        [Route("api/chatApi/getChatModelFromId/{chatId}/")]
        public IHttpActionResult getChatModelFromId(string chatId)
        {
            if (chatId == null)
                return BadRequest(message: "User Id is null");

            ChatModel chat = new ChatModel();
            using (ChatAppDBContext dbContext = new ChatAppDBContext())
            {
                try
                {
                    chat = dbContext.chatDBSet.Where(y => y.Id == chatId).FirstOrDefault();
                }
                catch
                {
                    return InternalServerError();
                }
            }
            return Ok(chat);
        }


    }
}