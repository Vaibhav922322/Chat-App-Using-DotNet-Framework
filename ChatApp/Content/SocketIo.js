const {Server}  = require("socket.io");
const io = new Server({ cors: "https://localhost:44363/" });

let onlineUser = [];


io.on("connection", (socket) => {
    console.log("new connection", socket.id);


    // listen to a connection
    socket.on("newOnlineUser", (userId) => {
        !onlineUser.some(user => user.userId === userId) &&
            onlineUser.push({
                userId,
                socketId: socket.id
            });
        console.log("onlineUsers : ", onlineUser);
        io.emit("getOnlineUsers", onlineUser);
    })

    socket.on("sendMessage", (params) => {
        const user = onlineUser.find(user => user?.userId === params[0]);
        if (user) {
            io.to(user.socketId).emit("refreshMessages", user?.userId);

            /*io.to(user.socketId).emit("getNotification", {
                senderId: message.senderId,
                isRead: false,
                date: new Date()
            })*/
        }
    });





    socket.on("disconnect", () => {
        onlineUser = onlineUser.filter(user => user.socketId !== socket.id);
        console.log("onlineUsers : ", onlineUser);
        io.emit("getOnlineUsers", onlineUser);
        console.log("disconnected");
    })
});
io.listen(3000);