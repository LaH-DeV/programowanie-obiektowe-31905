export function showError(error, dom) {
    if (Array.isArray(dom?.toHide)) {
        dom.toHide.forEach(element => element.style.display = "none")
    }
    if (dom?.alert) {
        dom.alert.innerHTML = `
            <h2>Wystąpił błąd</h2>
            <p>${error}</p>
            <a href="${location}">Odśwież stronę</a>
        `;
        dom.alert.removeAttribute("style");
    }
}

export async function request(url, options) {
    try {
        const res = await fetch(url, options);
        if (res.status === 400) {
            const err = await tryToReadError(res);
            if (err != null) return err;
            else throw new Error(res.statusText);
        } else if (!res.ok) throw new Error(res.statusText);
        const contentType = res.headers.get("Content-Type");
        if (typeof contentType === "string" && contentType.includes("application/json")) {
            return { data: await res.json() };
        }
        if (typeof contentType === "string" && contentType.includes("text/")) {
            return { data: await res.text() };
        }
        return { data: null };
    } catch (err) {
        console.error(err);
        return { error: err };
    }
}

async function tryToReadError(res) {
    try {
        return await res.json(); // { error: string }
    } catch {
        return null;
    }
}
