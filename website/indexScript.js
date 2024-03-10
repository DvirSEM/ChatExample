let archive = document.getElementsByClassName("history")[0];
let messageBox = document.getElementsByClassName("messageBox")[0];
let nameInput = document.getElementsByClassName("nameInput")[0];
let colorInput = document.getElementsByClassName("colorInput")[0];


async function getMessages() {
  let response = await fetch("/getMessages");
  let messages = await response.json();

  archive.innerHTML = null;

  for (let i = 0; i < messages.length; i++) {
    let message = messages[i];

    let entry = document.createElement("div");
    entry.classList.add("entry");
    archive.appendChild(entry);

    let nameCircle = document.createElement("div");
    nameCircle.classList.add("nameCircle");
    nameCircle.innerText = message.Name[0];
    nameCircle.style.backgroundColor = message.Color;
    entry.appendChild(nameCircle);

    let bobble = document.createElement("div");
    bobble.classList.add("bobble");
    bobble.innerText = message.Content;
    entry.appendChild(bobble);
  }
}

async function sendMessage() {
  let name = nameInput.value;
  if (name == "" || name == null) {
    name = "Unknown";
  }

  let message = {
    Name: name,
    Color: colorInput.value,
    Content: messageBox.value
  }

  let messageJson = JSON.stringify(message);

  await fetch("/sendMessage", {
    method: "POST",
    body: messageJson
  });

  messageBox.value = "";

  await getMessages();
}

getMessages();

setInterval(() => {
  getMessages();
}, 5000);