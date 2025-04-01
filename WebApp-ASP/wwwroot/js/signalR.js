const connection = new SignalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();

connection.on("ReceiveMessage", (username, message) => {
    const div = document.createElement("div");
    div.textContent =
        `
        <div class="content">
            <div class="username">
                ${username}
            </div>
            <div class="chat-message">${message}</div>
        </div>

        `;
    document.getElementById("chat-messages").appendChild(div);
});

connection.start().catch(err => console.error(err.toString()));

function sendMessage(event) {
    const username = document.getElementById("username").value;
    const message = document.getElementById("message").value;
    connection.invoke("SendMessage", username, message).catch(err => console.error(err.toString()));

    document.getElementById("message").value = "";

    event.preventDefault();
 }