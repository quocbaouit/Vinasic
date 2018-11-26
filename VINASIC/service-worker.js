/* eslint-env browser, serviceworker, es6 */
'use strict';
self.addEventListener('push', function (event) {
    var payload = JSON.parse(event.data.text());
    var title = (payload.Title != '') ? payload.Title : 'Notification';
    var message = payload.Message;
    //var icon = (payload.Icon != '') ?payload.Icon : 'Content/images/logo-paypal.jpg';
    var data = {
        url: (payload.Url!='')?payload.Url:'/'
    };
    const options = {
        body: message,
        icon: '',
        data: data
    };
    event.waitUntil(self.registration.showNotification(title, options));
});
self.addEventListener('notificationclick', function (event) {
    console.log('[Service Worker] Notification click Received.');

    event.notification.close();
    var url = event.notification.data.url;
    event.waitUntil(
      clients.openWindow(url)
    );
});
self.addEventListener('pushsubscriptionchange', e => {
    e.waitUntil(registration.pushManager.subscribe(e.oldSubscription.options)
      .then(subscription => {
          // TODO: Send new subscription to application server  
      }));
});
self.addEventListener('install', () => {
    console.log('[sw]', 'Your ServiceWorker is installed');
});
