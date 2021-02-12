function log() {
    document.getElementById('results').innerText = '';

    Array.prototype.forEach.call(arguments, function (msg) {
        if (msg instanceof Error) {
            msg = "Error: " + msg.message;
        }
        else if (typeof msg !== 'string') {
            msg = JSON.stringify(msg, null, 2);
        }
        document.getElementById('results').innerHTML += msg + '\r\n';
    });
}

document.getElementById("login").addEventListener("click", login, false);

var config = {
    authority: "https://localhost:5001",
    client_id: "web",
    redirect_uri: "https://localhost:6001/callback.html",
    response_type: "code",
    scope:"openid",
    post_logout_redirect_uri : "https://localhost:6001/index.html",
};
var mgr = new Oidc.UserManager(config);

let i = 0;
setInterval(() => {
    mgr.getUser().then(function (user) {
        if (user) {
            log("User logged in "+i, user.profile);
            i++;
        }
        else {
            log("User not logged in");
        }
    });
}, 1000);

function login() {
    mgr.signinRedirect();
}


