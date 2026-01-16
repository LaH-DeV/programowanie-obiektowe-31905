import { request, showError } from "/js/utils.js";

const params = new URLSearchParams(window.location.search);
const currentListId = params.get("id");
let currentListName = "";
const form = document.querySelector("form");
const nameInput = form.querySelector("#name");
const list = document.getElementById("list");
const alert = document.getElementById("alert");
const suggestions = document.getElementById("suggestions");
const actions = ["deleteList", "delete", "unmark", "mark"];
const items = new Map();

document.body.addEventListener("click", async (event) => {
    const button = event.target?.closest("button");
    if (button?.tagName !== "BUTTON") return;
    const action = button.getAttribute("data-action");
    const id = button.getAttribute("data-id");
    if (actions.includes(action)) event.preventDefault();
    switch (action) {
        case "deleteList":
            deleteList();
            break;
        case "delete":
            await safeRequest(`/items/${id}`, { method: "DELETE" });
            break;
        case "unmark":
            await safeRequest(`/items/${id}/unmark`, { method: "PUT" });
            break;
        case "mark":
            await safeRequest(`/items/${id}/mark`, { method: "PUT" });
            break;
        default:
            break;
    }
});

document.body.addEventListener("focusout", (event) => {
    const element = event.target;
    if (!element?.hasAttribute("contenteditable")) return;
    const attribute = element?.getAttribute("data-attribute");
    if (attribute === "listName") {
        clearTimeout(element._debounceListName);
        element._debounceListName = setTimeout(() => editListName(element.textContent), 500);
    } else {
        const id = element?.getAttribute("data-id");
        if (!id) return;
        if (attribute === "name") {
            clearTimeout(element._debounceName);
            element._debounceName = setTimeout(() => editItem({ id: Number(id), name: element.textContent }), 500);
        } else if (attribute === "quantity") {
            clearTimeout(element._debounceQty);
            element._debounceQty = setTimeout(() => editItem({ id: Number(id), quantity: element.textContent }), 500);
        }
    }
});


form.addEventListener("submit", (e) => {
    e.preventDefault();
    addItem(Object.fromEntries(new FormData(e.target)));
    nameInput.value = "";
});

if (currentListId) {
    loadList();
    bindGetSuggestions(nameInput, suggestions);
} else {
    reportError("Brak identyfikatora listy (id)",);
}

async function deleteList() {
    const { error } = await request(`/lists/${currentListId}`, { method: "DELETE" });
    if (error != null) reportError(error);
    else window.location = "/";
}

async function loadList() {
    const { error, data } = await request(`/lists/${currentListId}`);
    if (error != null) reportError(error);
    else {
        const listName = document.querySelector("h1 span");
        if (listName) listName.textContent = data.name;
        currentListName = data.name;
        renderItems(data.items);
        document.querySelector(".actions.list-actions").classList.remove("d-none");
        list.removeAttribute("style");
        form.removeAttribute("style");
    }
}

async function editListName(name) {
    if (!name) {
        loadList();
        return;
    }
    if (currentListName === name) return;
    await safeRequest(`/lists/${currentListId}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ name })
    });
}

function cacheItems(data) {
    items.clear();
    data.forEach(item => items.set(item.id, item));
}

async function loadItems() {
    const { error, data } = await request(`/lists/${currentListId}/items`);
    if (error != null) reportError(error);
    else renderItems(data);
}

async function addItem(data) {
    await safeRequest(`/lists/${currentListId}/items`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({
            name: data.name,
            quantity: Number(data.quantity)
        })
    });
}

async function editItem(data) {
    if (!data?.id) {
        loadItems();
        return;
    }
    const item = items.get(data.id);
    const name = data.name || item.name;
    let quantity = Number(data.quantity) || item.quantity;
    if (Number.isNaN(quantity)) quantity = item.quantity;
    if (item.name === name && quantity === item.quantity) return;
    await safeRequest(`/items/${data.id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ name, quantity })
    });
}

function renderItems(items) {
    cacheItems(items);
    list.innerHTML = "";
    let newHtml = "";
    const template = document.getElementById("item-template");
    items.forEach(i => {
        const clone = template.content.cloneNode(true);
        const li = clone.querySelector("li");
        const itemDiv = li.querySelector(".item");
        if (i.isBought) itemDiv.classList.add("bought");
        ["name", "quantity"].forEach(property => {
            const span = li.querySelector(`[data-attribute="${property}"]`);
            span.textContent = i[property];
            span.dataset.id = i.id;
            if (i.isBought) span.removeAttribute("contenteditable");
        });
        ["mark", "unmark", "delete"].forEach((action) => {
            const btn = li.querySelector(`button.${action}`);
            btn.dataset.id = i.id;
            if (action === "mark" && i.isBought || action === "unmark" && !i.isBought) btn.style.display = "none";
        });
        newHtml += li.outerHTML;
    });
    list.innerHTML = newHtml;
}


async function safeRequest(url, options) {
    const { error } = await request(url, options);
    if (error == null) loadItems();
    else reportError(error);
}

function reportError(error) {
    showError(error, { alert, toHide: [form, list] });
}



function bindGetSuggestions(input, target, debounce = 300) {
    let debounceTimer, clearingTimer;
    input.addEventListener("input", () => {
        clearTimeout(debounceTimer);
        const query = input.value.trim();
        if (query.length < 1) {
            target.innerHTML = "";
            target.classList.add("d-none");
            return;
        }
        debounceTimer = setTimeout(async () => {
            try {
                const response = await fetch(`/suggestions?q=${encodeURIComponent(query)}`);
                const data = await response.json();
                target.innerHTML = "";
                target.classList.add("d-none");
                data.forEach(name => {
                    const li = document.createElement("li");
                    li.textContent = name;
                    li.addEventListener("click", () => {
                        input.value = name;
                        target.innerHTML = "";
                        target.classList.add("d-none");
                    });
                    target.appendChild(li);
                    target.classList.remove("d-none");
                });
            } catch (err) {
                console.error("Suggestions error", err);
            }
        }, debounce);
    });
    input.addEventListener("blur", () => {
        clearTimeout(debounceTimer);
        clearingTimer = setTimeout(() => {
            if (!target.classList.contains("d-none")) target.classList.add("d-none");
        }, 100);
    });
}