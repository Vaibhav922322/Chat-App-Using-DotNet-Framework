using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using ChatApp.Models;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using System.Web.Http;

namespace ChatApp.Controllers
{
    [System.Web.Mvc.Authorize]
    public class HomeController : Controller
    {

        private readonly HttpClient httpClient;
        public HomeController()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:44363/api/"); // Replace with the API base URL
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }


        // GET: Home
        public ActionResult Home()
        {
            return View();
        }

        public async Task<ActionResult> testChat()
        {
            
            string apiUrl = $"getAllIdsExcept/{User.Identity.Name}/"; // Replace with the actual API endpoint
            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response content to get the employee details
                List<DisplayThings> chats = await response.Content.ReadAsAsync<List<DisplayThings>>();
                
                // Pass the employee details to the view
                return View(chats);
            }

            // Handle unsuccessful requests (e.g., display an error message)
            ViewBag.ErrorMessage = "Failed to get users from API.";
            return View("Error");
        
        }

        [System.Web.Http.HttpGet]
        public async Task<ActionResult> createChat(string id)
        {
            string apiUrl = $"chatapi/createChatId/"; // Replace with the actual API endpoint
            List<string> usersInChat = new List<string>() { User.Identity.Name, id };
            string jsonString = JsonConvert.SerializeObject(usersInChat);

            // Create HttpContent from the serialized JSON string
            HttpContent httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(apiUrl, httpContent);

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response content to get the employee details
                //List<DisplayThings.Chat> chats = await response.Content.ReadAsAsync<List<DisplayThings.Chat>>();

                // Pass the employee details to the view
                //return RedirectToAction(controllerName:"Home", actionName: "getUserChats");
                return Json("OK", JsonRequestBehavior.AllowGet);
            }

            // Handle unsuccessful requests (e.g., display an error message)
            ViewBag.ErrorMessage = "Failed to get users from API.";
            //return View("Error");
            return Json("Error", JsonRequestBehavior.AllowGet);

        }


        [System.Web.Http.HttpGet]
        public async Task<ActionResult> getUserChats()
        {
            string apiUrl = $"chatApi/getUserChatIds/{User.Identity.Name}/";
            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response content to get the chat details
                List<DisplayThings> chats = await response.Content.ReadAsAsync<List<DisplayThings>>();

                // Pass the chat details to the view
                
                return Json(chats, JsonRequestBehavior.AllowGet);
            }

            // Handle unsuccessful requests (e.g., display an error message)
            ViewBag.ErrorMessage = "Failed to get users from API.";
            return Json("Error", JsonRequestBehavior.AllowGet);
        }

        [System.Web.Http.HttpGet]
        /*To get all users except logged In one*/
        public async Task<ActionResult> getUsersExcept()
        {

            string apiUrl = $"getAllIdsExcept/{User.Identity.Name}/"; // Replace with the actual API endpoint
            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response content to get the employee details
                List<DisplayThings> users = await response.Content.ReadAsAsync<List<DisplayThings>>();


                // Pass the employee details to the view
                return Json(users, JsonRequestBehavior.AllowGet);
            }

            // Handle unsuccessful requests (e.g., display an error message)
            ViewBag.ErrorMessage = "Failed to get users from API.";
            return Json("Error", JsonRequestBehavior.AllowGet);

        }

        [System.Web.Http.HttpGet]
        [System.Web.Mvc.Route("Home/getMessagesInChat/{chatId}")]
        public async Task<ActionResult> getMessagesInChat(string chatId)
        {
            string apiUrl = $"messageApi/getChatMessages/{chatId}/";
            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response content to get the chat details
                List<MessageModel> messages = await response.Content.ReadAsAsync<List<MessageModel>>();

                // Pass the chat details to the view

                return Json(messages, JsonRequestBehavior.AllowGet);
            }

            // Handle unsuccessful requests (e.g., display an error message)
            ViewBag.ErrorMessage = "Failed to get messages from API.";
            return Json("Error", JsonRequestBehavior.AllowGet);
        }
    
        [System.Web.Http.HttpPost]
        [System.Web.Mvc.Route("Home/createMessage/")]

        public async Task<ActionResult> createMessage(DisplayThings.Message msg)
        {
            if(msg == null && (msg.receiverId == null || msg.chatId == null || msg.text == null))
                return Json("Error", JsonRequestBehavior.AllowGet);

            string apiUrl = $"messageApi/createMessageThroughWeb/";

            List<string> messageDetails = new List<string>() { msg.chatId, User.Identity.Name, msg.receiverId, msg.text };
            string jsonString = JsonConvert.SerializeObject(messageDetails);
            HttpContent httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            HttpResponseMessage  response = await httpClient.PostAsync(apiUrl, httpContent);

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response content to get the chat details
                List<MessageModel> messages = await response.Content.ReadAsAsync<List<MessageModel>>();

                // Pass the chat details to the view
                return Json(messages, JsonRequestBehavior.AllowGet);
            }
            return Json("Error", JsonRequestBehavior.AllowGet);
        }




















        [System.Web.Http.HttpGet]
        public async Task<ActionResult> testChat1(string id)
        {

            string apiUrl = $"getUserById/{id}/"; // Replace with the actual API endpoint
            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response content to get the employee details
                DisplayThings users = await response.Content.ReadAsAsync<DisplayThings>();

                // Pass the employee details to the view
                return View(users);
            }

            // Handle unsuccessful requests (e.g., display an error message)
            ViewBag.ErrorMessage = "Failed to get users from API.";
            return View("Error");

        }


        public ActionResult updateTest()
        {

            return View();

        }
        [System.Web.Mvc.Route("Home/upd")]
        public async Task<ActionResult> upd()
        {
            // Replace this with your actual logic to get the data from the server
            //string data = "Data from the server at " + DateTime.Now.ToString();
            string apiUrl = $"chatApi/getUserChatIds/{User.Identity.Name}/"; // Replace with the actual API endpoint
            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the response content to get the employee details
                List<DisplayThings> chats = await response.Content.ReadAsAsync<List<DisplayThings>>();

                // Pass the employee details to the view
                //return View(chats);
                return Json(chats, JsonRequestBehavior.AllowGet);
            }

            // Handle unsuccessful requests (e.g., display an error message)
            ViewBag.ErrorMessage = "Failed to get users from API.";
            //return View("Error");
            return Json("Error", JsonRequestBehavior.AllowGet);
        }

        [System.Web.Http.Authorize]
        [System.Web.Http.HttpPost]
        [System.Web.Mvc.Route("Home/testingcreate/")]
        public ActionResult testingcreate(DisplayThings.Message messages)
        {
            return Json(messages, JsonRequestBehavior.AllowGet);

        }


    }
}