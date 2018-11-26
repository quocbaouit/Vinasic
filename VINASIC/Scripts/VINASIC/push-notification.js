$(document).ready(function () {
    $.ajaxSetup({
        headers: {
            'X-CSRF-Token': $('meta[name="_token"]').attr('content')
        }
    });
    $('#close').on('click',
        function () {
            $('#note').hide();
            //localStorage.setItem("haveFirstTime", true);
        });
});

//object to get browser info.
var infoModule = {
    options: [],
    header: [navigator.platform, navigator.userAgent, navigator.appVersion, navigator.vendor, window.opera],
    dataos: [
        { name: 'Windows Phone', value: 'Windows Phone', version: 'OS' },
        { name: 'Windows', value: 'Win', version: 'NT' },
        { name: 'iPhone', value: 'iPhone', version: 'OS' },
        { name: 'iPad', value: 'iPad', version: 'OS' },
        { name: 'Kindle', value: 'Silk', version: 'Silk' },
        { name: 'Android', value: 'Android', version: 'Android' },
        { name: 'PlayBook', value: 'PlayBook', version: 'OS' },
        { name: 'BlackBerry', value: 'BlackBerry', version: '/' },
        { name: 'Macintosh', value: 'Mac', version: 'OS X' },
        { name: 'Linux', value: 'Linux', version: 'rv' },
        { name: 'Palm', value: 'Palm', version: 'PalmOS' }
    ],
    databrowser: [
        { name: 'Chrome', value: 'Chrome', version: 'Chrome' },
        { name: 'Firefox', value: 'Firefox', version: 'Firefox' },
        { name: 'Safari', value: 'Safari', version: 'Version' },
        { name: 'Internet Explorer', value: 'MSIE', version: 'MSIE' },
        { name: 'Opera', value: 'Opera', version: 'Opera' },
        { name: 'BlackBerry', value: 'CLDC', version: 'CLDC' },
        { name: 'Mozilla', value: 'Mozilla', version: 'Mozilla' }
    ],
    init: function () {
        var agent = this.header.join(' '),
            os = this.matchItem(agent, this.dataos),
            browser = this.matchItem(agent, this.databrowser);

        return { os: os, browser: browser };
    },
    matchItem: function (string, data) {
        var i = 0,
            j = 0,
            html = '',
            regex,
            regexv,
            match,
            matches,
            version;

        for (i = 0; i < data.length; i += 1) {
            regex = new RegExp(data[i].value, 'i');
            match = regex.test(string);
            if (match) {
                regexv = new RegExp(data[i].version + '[- /:;]([\\d._]+)', 'i');
                matches = string.match(regexv);
                version = '';
                if (matches) { if (matches[1]) { matches = matches[1]; } }
                if (matches) {
                    matches = matches.split(/[._]+/);
                    for (j = 0; j < matches.length; j += 1) {
                        if (j === 0) {
                            version += matches[j] + '.';
                        } else {
                            version += matches[j];
                        }
                    }
                } else {
                    version = '0';
                }
                return {
                    name: data[i].name,
                    version: parseFloat(version)
                };
            }
        }
        return { name: 'unknown', version: 0 };
    }
};

$(window).on('load',
    function () {
        $('#subscribe').on('click',
            function () {
                $('#note').hide();
                subscribe(0);
            });
        //when window onload we need register service Worker to handle push event. after register service we need init browser state.
        if ('serviceWorker' in navigator) {
            navigator.serviceWorker.register('/service-worker.js')
                .then(initialiseState);
        } else {
            console.warn('Service workers are not supported in this browser.');
        }
    });

// when user Unsubscribe notification in browser menu the first time when page ready we will delete expired endPoint.
function initialiseState() {
    if (!('showNotification' in ServiceWorkerRegistration.prototype)) {
        console.warn('Notifications are not supported.');
        return;
    }
    if (Notification.permission === 'default') {
        //important: when use delete notification permission by browser menu.when reload page again we need delete endpoint exist. 
       // deleteExpiredEndPoint();
        var haveFirst = localStorage.getItem("haveFirstTime");
        if (!haveFirst) { $('#note').css('display', 'inherit'); }
    }
    if (Notification.permission === 'denied') {
        //important: when use delete notification permission by browser menu.when reload page again we need delete endpoint exist. 
        //deleteExpiredEndPoint();
        console.warn('The user has blocked notifications.');
        return;
    }
    if (!('PushManager' in window)) {
        console.warn('Push messaging is not supported.');
        return;
    }
    //when page ready we need check if enpoint exist and save it to localStorage in case customer delete LocalStorage when delete history.
    navigator.serviceWorker.ready.then(function (serviceWorkerRegistration) {
        serviceWorkerRegistration.pushManager.getSubscription()
        .then(function (subscription) {
            if (!subscription) {
                return;
            }
            localStorage.setItem("oldEndpoint", subscription.endpoint);
            var BrowserInfo = infoModule.init();
            var globalSubscription = subscription.toJSON();
            globalSubscription.BrowserName = BrowserInfo.browser.name;
            globalSubscription.BrowserVersion = BrowserInfo.browser.version;
            globalSubscription.OsName = BrowserInfo.os.name;
            globalSubscription.OsVersion = BrowserInfo.os.version;
            //sendSubscriptionToBackEnd(globalSubscription,0);
            isPushEnabled = true;
        })
        .catch(function (err) {
            $.notify({ message: 'unexcepted error getting subscription info' }, { type: 'warning' });
        });
    });
}

//when user subscribe we need save endPoint to DB and localStorage.
function subscribe(type) {
    navigator.serviceWorker.ready.then(function (serviceWorkerRegistration) {
        serviceWorkerRegistration.pushManager.subscribe({ userVisibleOnly: true })
        .then(function (subscription) {
            localStorage.setItem("oldEndpoint", subscription.endpoint);
            var BrowserInfo = infoModule.init();
            var globalSubscription = subscription.toJSON();
            globalSubscription.BrowserName = BrowserInfo.browser.name;
            globalSubscription.BrowserVersion = BrowserInfo.browser.version;
            globalSubscription.OsName = BrowserInfo.os.name;
            globalSubscription.OsVersion = BrowserInfo.os.version;
            sendSubscriptionToBackEnd(globalSubscription, type);
           // $.notify({ message: 'Thank you for subscribing' }, { type: 'warning' });
        })
        .catch(function (e) {
            if (Notification.permission === 'denied') {
                $.notify({ message: 'Permission for Notifications was denied.' }, { type: 'warning' });
            }
        });
    });
}

//when user Unsubscribe we need save endPoint to DB and localStorage.
function unsubscribe(type) {
    navigator.serviceWorker.ready.then(function (serviceWorkerRegistration) {
        serviceWorkerRegistration.pushManager.getSubscription().then(
            function (pushSubscription) {
                if (!pushSubscription) {
                    return;
                }
                localStorage.setItem("oldEndpoint", pushSubscription.endpoint);
                pushSubscription.unsubscribe().then(function () {
                    //UnsubscribeToBackEnd(pushSubscription.endpoint, type);
                    $.notify({ message: 'This device is unsubscribe' }, { type: 'warning' });
                }).catch(function (e) {
                    console.log('unsubscribe error: ', e);
                });
            }).catch(function (e) {
                $.notify({ message: 'Error thrown while unsubscribing from push messaging.' }, { type: 'warning' });
            });
    });
}

function UnsubscribeToBackEnd(endpoint, type) {
    $.ajax({
        url: '/push-notification-unsubscribe',
        type: 'POST',
        contentType: 'application/json',
        accept: 'application/json',
        dataType: 'json',
        data: JSON.stringify({ 'Endpoint': endpoint, 'Type': type })
    });
}

function sendSubscriptionToBackEnd(globalSubscription, type) {
    //get preview end point.
    var oldEndPoint = localStorage.getItem("oldEndpoint");
    //save endPoint to local storage to track.
    $.ajax({
        url: '/User/UserSubscribe',
        type: 'POST',
        contentType: 'application/json',
        accept: 'application/json',
        dataType: 'json',
        data: JSON.stringify({ 'Subscription': globalSubscription, 'OldEndPoint': oldEndPoint, 'Type': type }),
    });
}

function deleteExpiredEndPoint() {
    //get preview end point.
    var oldEndPoint = localStorage.getItem("oldEndpoint");
    if (oldEndPoint == '') return;
    $.ajax({
        url: '/push-notification-unsub-exp-endpoint',
        type: 'POST',
        contentType: 'application/json',
        accept: 'application/json',
        dataType: 'json',
        data: JSON.stringify({ 'Endpoint': oldEndPoint }),
    });
}


