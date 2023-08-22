var l = [];
var chats = [];
var users = [];
const loggedInUserId = document.getElementById("userIdofLoggedIn").getAttribute("data-razor-data");
var currentChatId = "";
var currentReceiverId = "";

function updateData() {
    $.ajax({
        url: 'https://localhost:44363/Home/upd',
        type: 'GET',
        dataType: 'json',
        success: function (data) {

            createList(users, 'all-users', 'user');
            createList(chats, 'messages-box', 'chat');
 
        },
        error: function () {
            alert("Error occurred while updating data.");
        }
    });
}

async function  getChats() {
    $.ajax( {
        url: 'https://localhost:44363/Home/getUserChats',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            chats = data
        },
        error: function () {
            alert("Error occurred while updating data.");
        }
    });
}

async function getUsers() {
    $.ajax({
        url: 'https://localhost:44363/Home/getUsersExcept',
        type: 'GET',
        dataType: 'json',
        success: function (data) {
            users = data
        },
        error: function () {
            alert("Error occurred while updating data.");
        }
    });
}

async function createList(arr, element_id, model) {
    if (arr !== null || arr.length === 0) {
        var listHtml = "";
        if (model === 'user' && arr.user !== null) {
            arr.forEach(function (item) {
                listHtml += `<div class="single-user" onclick="createChat('${item.user.userId}')">
                                    ${item.user.userName}
                             </div>`;
            });
            if (listHtml === "")
                document.getElementByClassName(element_id)[0].innerHTML = `<h3> no chats </h3>`;
            else
                document.getElementsByClassName(element_id)[0].innerHTML = listHtml;
        }
        else if (model === 'chat' && arr.chat !== null ) {
            arr.forEach(function (item) {
                listHtml += `<div role="button" onclick ="getSelectedChatMessages('${item.chat.chatId}', '${item.chat.receiverId}','${item.chat.personName}')">
                                    <div  class="user-card align-items-center p-2 justify-content-between hstack gap-3">
                                        <div class = "d-flex">
                                            <div class = "me-2" >
                                                <img src="/Content/avtar.svg" height="35px">
                                            </div>
                                            <div class="text-content">
                                                <div class="name">
                                                    ${item.chat.personName}
                                                </div>
                                                <div class="text">
                                                    jknjknjkgsf
                                                </div>
                                            </div>
                                        </div>
                                        <div class="d-flex flex-column align-items-end">
                                            <div class="date">
                                                07/28/2023
                                            </div>
                                            <div class="this-user-notifications">1
                                            </div>
                                            <span class="user-online">
                                            </span>
                                        </div>
                                    </div>
                                </div>`
            });
            if (listHtml === "")
                document.getElementByClassName(element_id)[0].innerHTML = `<h3> no chats </h3>`;
            else
                document.getElementsByClassName(element_id)[0].innerHTML = listHtml;
        }

    }
}

async function createChat(userId) {

    $.ajax({
        url: `https://localhost:44363/Home/createChat/${userId}/`,
        type: 'GET',
        dataType: 'json',
        success: async function (data) {
            getChats().then(createList(chats, 'chatDiv', 'chat')).then(updateData());
        },
        error: function () {
            alert("Error occurred while updating data.");
        }
    });
}

function displayMessages(messages) {
    if (messages !== null || messages.length < 0)
    {
        if (messages.length === 0) {
            document.getElementsByClassName("chat-box")[0].innerHTML = `<div class="message align-self-center flex-grow-0 vstack">
                    <span> No Messages</span>
                </div>`;
        }
        else
        {
            var listHtml = "";
            messages.forEach(function (msg) {
                listHtml += `<div class="message ${(loggedInUserId !== msg.senderId) ? "align-self-start" : "self align-self-end"}  flex-grow-0 vstack">
                                <span> ${msg.text}</span>
                                <span class="message-footer">07/28/2023</span>
                             </div>`;
            });
            document.getElementsByClassName("chat-box")[0].innerHTML = listHtml;
        } 
    }
    var myDiv = document.getElementsByClassName("chat-box")[0]

    // Scroll the div to the bottom
    myDiv.scrollTop = myDiv.scrollHeight;
}

async function getSelectedChatMessages(chatId, receiverId, receiverName) {
    if (chatId === "") {
        document.getElementById("displayName").innerHTML = "----";
        document.getElementsByClassName("chat-box")[0].innerHTML = `
            <div class="message align-self-center flex-grow-0 vstack">
                    <span> No Conversations selected yet</span>
                </div>
        `;
    }
    else {
        document.getElementById("displayName").innerHTML = receiverName;

        currentChatId = chatId;
        currentReceiverId = receiverId;
        $.ajax({
            url: `https://localhost:44363/Home/getMessagesInChat/${chatId}`,
            type: 'GET',
            dataType: 'json',
            
            success: async function (response) {
                console.log("data in method : ", response);
                displayMessages(response);
                //getChats().then(createList(chats, 'chatDiv', 'chat')).then(updateData());
            },
            error: function () {
                alert("Error occurred while updating data.");
            }
        });
    }
}

async function createMessage(txt) {
    
    console.log(txt);
    if (txt !== "" || txt !== null) {
        $.ajax({
            url: `https://localhost:44363/Home/createMessage/`,
            type: 'POST',
            dataType: 'json',
            data: {
                chatId: currentChatId,
                receiverId: currentReceiverId,
                text: txt
            },
            success: async function (response) {
                getSelectedChatMessages(currentChatId, currentReceiverId);
            },
            error: function () {
                alert("Error occurred while updating data.");
            }
        });
    }
}

function sendMessage() {
    const txt = document.getElementById("msgInpbox").value;
    if (txt !== null) {
        createMessage(txt);
    }
    document.getElementById("msgInpbox").value = "";
}

getUsers().then(console.log(1))
    .then(console.log(users, users.length))
    .then(getChats())
    .then(console.log(2))
    .then(console.log(chats, chats.length))
    .then(getSelectedChatMessages("","",""));



updateData();

function lprintfgh(txt) {
    console.log(txt);
}


