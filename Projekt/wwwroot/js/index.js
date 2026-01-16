import { request, showError } from "/js/utils.js";

const lists = document.getElementById("lists");
const form = document.querySelector("form");
const alert = document.getElementById("alert");


form.addEventListener("submit", async (e) => {
    e.preventDefault();
    createList(Object.fromEntries(new FormData(e.target)));
    const nameInput = form.querySelector("#name");
    if (nameInput) nameInput.value = "";
});


function renderListElement(data) {
    const li = document.createElement("li");
    li.classList.add("shopping-list");
    const a = document.createElement("a");
    a.href = `/list.html?id=${data.id}`;
    a.textContent = data.name;
    li.appendChild(a);
    const qty = document.createElement("span");
    qty.classList.add("qty");
    qty.textContent = `${data.boughtItemCount} / ${data.itemCount}`;
    li.appendChild(qty);
    return li;
}

async function loadLists() {
    const { error, data } = await request("lists");
    if (error != null) reportError(error);
    else {
        lists.innerHTML = "";
        data.forEach(list => lists.appendChild(renderListElement(list)));
    }
}

async function createList(formData) {
    const name = formData.name.trim();
    if (!name) return;
    const { error, data } = await request("/lists", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ name })
    });
    if (error == null) window.location = `list.html?id=${data.id}`;
    else showError(error);
}


function reportError(error) {
    showError(error, { alert, toHide: [lists] });
}

loadLists();