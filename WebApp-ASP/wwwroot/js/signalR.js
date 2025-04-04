document.addEventListener('DOMContentLoaded', () => {
    updateNotificationTime();
    setInterval(updateNotificationTime, 8000);
});

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub")
    .build();

connection.on("AllReceiveNotification", (notification) => {
    notifications = notification
    printNotifications(notification);
});
connection.on("AdminReceiveNotification", (notification) => {
    console.log("Admin message received")
    // If user is admin
    GetRole(notification);
});

connection.on("AllNotificationsDismissed", () => {
    updateNotificationTime();
    updateNotificationCount();
});

connection.on("NotificationDismissed", (notificationId) => {
    removeNotification(notificationId);
    updateNotificationTime();
    updateNotificationCount();
});

// Start signalr server
connection.start().catch(error => console.error(error));

// Get user role
async function GetRole(notification) {
    console.log("Checking admin role")
    try {
        const res = await fetch(`/api/notifications/isuseradmin`, {
            method: 'POST'
        });
        if (res.ok) {
            const data = await res.json();
            if (data.success) {
                notifications = notification
                printNotifications(notification);
                return true
            }
            return false
        }
    }
    catch (error) {
        return false;
    }
}

// Print notifications to screen
function printNotifications(notification) {
    const container = document.querySelector('.notifications');

    const item = document.createElement('div');
    item.classList.add('notification-item');
    item.setAttribute('data-id', notification.id);
    updateNotificationTime();
    item.innerHTML =
        `
            <img class="image" src="${notification.image}"/>
            <div class="message">${notification.message}</div>
            <div class="notification-time" data-created="${new Date(notification.created).toISOString()}">${notification.created}</div>
            <button class="btn-close" onclick="dismissNotification('${notification.id}')">&#10006;</button>
        `;

    container.insertBefore(item, container.firstChild);

    updateNotificationCount();
    updateNotificationTime();
    document.querySelector('.dot-red').classList.add('show');
}

// Call Dismiss a notification in controller
async function dismissNotification(notificationId) {
    try {
        const res = await fetch(`/api/notifications/dismiss/${notificationId}`, {
            method: 'POST'
        });
        if (res.ok) {
            removeNotification(notificationId);
        } else {
            console.error(`Failed to dismiss notification`);
        }
    }
    catch (error) {
        console.error(`Error when removing notification: `, error);
    }
}
// Call Dismiss all notifications in controller
async function dismissAllNotifications(userName) {
    try {
        const res = await fetch(`/api/notifications/dismissall/${userName}`, {
            method: 'POST'
        });
        if (res.ok) {
            removeAllNotifications();
        } else {
            console.error(`Failed to dismiss all notifications`);
        }
    }
    catch (error) {
        console.error(`Error when removing notification: `, error);
    }
}

// Remove a notification from the UI
function removeNotification(notificationId) {
    const element = document.querySelector(`.notification-item[data-id="${notificationId}"]`);
    if (element) {
        element.remove();
        updateNotificationCount();
    }
}
function removeAllNotifications() {
    const elements = document.querySelector('.notifications');
    elements.innerHTML = '';
    updateNotificationCount();
}

// Update the notification counter
function updateNotificationCount() {
    const count = document.querySelectorAll('.notification-item').length;
    document.querySelector('.notification-number').textContent = count;

    if (count === 0) {
        document.querySelector('.dot-red').classList.remove('show');
    }
}

// Notification time caluclation
function updateNotificationTime() {
    const notificationTime = document.querySelectorAll('.notification-time');
    const now = new Date();

    notificationTime.forEach(notification => {
        const date = new Date(notification.getAttribute('data-created'));
        const diff = Math.floor((now - date) / 1000); // Difference in seconds
        let timeString;
        if (diff < 60) {
            timeString = `${diff} sekunder sedan`;
        } else if (diff < 3600) {
            timeString = `${Math.floor(diff / 60)} minuter sedan`;
        } else if (diff < 86400) {
            timeString = `${Math.floor(diff / 3600)} timmar sedan`;
        } else {
            timeString = `${Math.floor(diff / 86400)} dagar sedan`;
        }
        notification.textContent = timeString;
    });
}