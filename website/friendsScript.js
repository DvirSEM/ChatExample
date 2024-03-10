let friendsList = document.getElementsByClassName("friendsList")[0];

async function getFriends() {
  let response = await fetch("/getFriends");
  let friends = await response.json();

  for (let i = 0; i < friends.length; i++) {
    let friend = friends[i];

    let entry = document.createElement("div");
    entry.classList.add("entry");
    entry.style.alignItems = "center";
    friendsList.appendChild(entry);

    let space = document.createElement("div");
    space.style.height = "15px";
    friendsList.appendChild(space);

    let nameCircle = document.createElement("div");
    nameCircle.classList.add("nameCircle");
    nameCircle.innerText = friend.Name[0];
    nameCircle.style.backgroundColor = friend.Color;
    nameCircle.style.marginRight = "10px";
    entry.appendChild(nameCircle);

    let fullNameDiv = document.createElement("div");
    fullNameDiv.innerText = friend.Name;
    entry.appendChild(fullNameDiv);
  }
}

getFriends();