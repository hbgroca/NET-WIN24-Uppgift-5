document.addEventListener("DOMContentLoaded", function () {
    // If cookie consent is missing? Open modal
    if (!getCookie("cookieConsent")) {
        showCookieModal();
    }
});


function showCookieModal() {
    const modal = document.querySelector("#cookieModal");
    if (modal) {
        modal.style.display = "flex";
    }

    const consent = getCookie("cookieConsent");
    if (consent) {
        const parsedConsent = JSON.parse(consent);
        document.querySelector("#cookieFunctional").checked = parsedConsent.functional;
        //document.querySelector("#cookieAnalytic").checked = parsedConsent.analytics;
        //document.querySelector("#cookieMarketing").checked = parsedConsent.marketing;
    }
};

function hideCookieModal() {
    const modal = document.querySelector("#cookieModal");
    if (modal) {
        modal.style.display = "none";
    }
};

function getCookie(name) {
    // Get the value of a cookie by name
    const nameEQ = name + "=";

    // Split the cookies string into individual cookies
    const cookies = document.cookie.split(';');

    // Loop through the cookies and find the one with the specified name
    for (let i = 0; i < cookies.length; i++) {
        let cookie = cookies[i].trim();
        if (cookie.indexOf(nameEQ) === 0) {
            // Return the cookie value if found
            return decodeURIComponent(cookie.substring(nameEQ.length));
        }
    }

    return null;
};

function setCookie(name, value, days) {
    let expires = "";

    if (days) {
        const date = new Date();
        date.setTime(date.getTime() + days * 24 * 60 * 60 * 1000);
        expires = "; expires=" + date.toUTCString();
    }

    const encodedValue = encodeURIComponent(value || "")
    document.cookie = `${name}=${encodedValue}${expires}; path=/; SamSite=Lax`
}

async function acceptAllCookies() {
    const consent = {
        essential: true,
        functional: true,
        analytics: true,
        marketing: true
    }

    setCookie("cookieConsent", JSON.stringify(consent), 90);
    await setConsent(consent);

    hideCookieModal();
}

async function acceptSelectedCookies() {
    const form = document.querySelector('#cookieConsentForm');
    const formData = new FormData(form);

    const consent = {
        essential: true,
        functional: formData.get("Functional") === "on",
        analytics: formData.get("Analytic") === "on",
        marketing: formData.get("Marketing") === "on",
    }

    setCookie("cookieConsent", JSON.stringify(consent), 90);
    await setConsent(consent);

    hideCookieModal();
}

async function setConsent(consent) {
    try {

        const res = await fetch('/cookies/setcookies', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(consent)
        });

        if (!res.ok) {
            console.error('Failed to set cookie consent', await res.text())
        }

    } catch (error) {
        console.warn("Something went wrong when saving cookie consent: ", error)
    }
}