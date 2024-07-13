let Cards = [];
let CurrentCardIndex = 0;
function flip(id) {
    $(id).toggleClass('flipped');
}

$(document).ready(() => {
    addRegisterElements();
    getNewSet();
});


function authId(str) {
    $.ajax({
        type: "POST",
        url: "api/Home/auth",
        data: JSON.stringify({ "token": str }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            window.location.href = window.location.origin + "/home";
        },
        failure: function (response) {
            console.log("fail", response);
        },
        error: function (response) {
            console.log("err",response);
        }
    });
}

function onSignIn(googleUser) {
    const id_token = googleUser.getAuthResponse().id_token;
    console.log("ID Token: " + id_token);
    $.ajax({
        type: "POST",
        url: "api/Home/auth",
        data: JSON.stringify({ "token": id_token }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {

        },
        failure: function (response) {

        },
        error: function (response) {

        }
    });
}

function addRegisterElements() {
    document.getElementById("mainNav").innerHTML =
        `<ul class="navbar-nav mr-auto">
        </ul>
        <div id="rightNav">
            <ul class="navbar-nav mr-auto">
                <li>
                    <a id="regNav" style="cursor: pointer;" class="nav-link active" onclick="regClick()">Register</a>
                </li>
                <li>
                    <a id="loginNav" style="cursor: pointer;" class="nav-link" onclick="logClick()">Login</a>
                </li>
            </ul>
        </div>`;
}

function showDefaultLogin() {
    $(document).ready(() => {
        document.getElementById("mainNav").innerHTML =
            `<ul class="navbar-nav mr-auto">
        </ul>
        <div id="rightNav">
            <ul class="navbar-nav mr-auto">
                <li>
                    <a id="regNav" style="cursor: pointer;" class="nav-link" onclick="regClick()">Register</a>
                </li>
                <li>
                    <a id="loginNav" style="cursor: pointer;" class="nav-link active" onclick="logClick()">Login</a>
                </li>
            </ul>
        </div>`;
        document.getElementById("Reg").style.display = "none";
        document.getElementById("Login").style.display = "block";
        RegSelected = false;
    });
}

let CanSwitch = true;
let RegSelected = true;
const Delay = 350;
function regClick() {
    if (!CanSwitch || RegSelected) return;
    else RegSelected = true;

    CanSwitch = false;
    setTimeout(() => { CanSwitch = true; }, Delay + 10);

    document.getElementById("Reg").style.display = "block";
    document.getElementById("Login").style.display = "none";

    $("#Reg").hide().show("drop", { direction: "left" }, Delay);

    document.getElementById("regNav").setAttribute("class", "nav-link active");
    document.getElementById("loginNav").setAttribute("class", "nav-link");
}
function logClick() {
    if (!CanSwitch || !RegSelected) return;
    else RegSelected = false;

    CanSwitch = false;
    setTimeout(() => { CanSwitch = true; }, Delay + 10);

    document.getElementById("Reg").style.display = "none";
    document.getElementById("Login").style.display = "block";

    $("#Login").hide().show("drop", { direction: "right" }, Delay);

    document.getElementById("regNav").setAttribute("class", "nav-link");
    document.getElementById("loginNav").setAttribute("class", "nav-link active");
}

function next(direction) {
    CurrentCardIndex += direction;
    if (CurrentCardIndex === -1) {
        CurrentCardIndex = Cards.length - 1
    } else if (CurrentCardIndex === Cards.length) {
        CurrentCardIndex = 0;
    }

    if (direction === -1)
        showNextCard("right")
    else showNextCard("left")

}
function getNewSet() {
    $.ajax({
        type: "GET",
        url: "/api/home/randomset",
        success: function (response) {

            response.forEach(element => Cards.push(element));
            
            document.getElementById("q").className = "card-text mt-4";
            document.getElementById("a").innerText = response[0].answer;
            document.getElementsByClassName("card-title")[0].innerHTML = `Question ${1} of ${Cards.length}`;
            document.getElementById("q").innerText = response[0].question;
            document.getElementById("cardLoader").remove();
        },
        error: function (response) {
            alert("Err");
        }
    });
    showNextCard();
}

function showNextCard(direction) {
    if (Cards.length === 0)
        return;

    const index = CurrentCardIndex;
    const cardCount = Cards.length;
    document.getElementById("cardContainer").innerHTML =
        `<div class="row m-auto" data-aos="fade-${direction}" data-aos-delay="100" id="cardDiv">
                    <section class="card-container m-auto">
                        <div class="card" onclick="flip(this)" style="border-radius:25px; font-weight:400;">
                            <div class="front">
                                <div class="card text-dark bg-white mb-3" style="border-radius:25px; box-shadow: 0 0 0 3px black;">
                                    <div class="card-body">
                                        <h4 class="card-title" style="color: black;">Question ${index + 1} of ${cardCount}</h4>
                                        <p class="card-text mt-4" style="color: black" id="q">
                                            ${Cards[index].question}    
                                        </p>
                                    </div>
                                </div>
                            </div>
                            <div class="back">
                                <div class="card text-dark bg-white mb-3" style="border-radius:25px; box-shadow: 0 0 0 3px black;">
                                    <div class="card-body">
                                        <h4 class="card-title" style="color: black">Answer</h4>
                                        <p class="card-text text-justify text-left" id="a" style="overflow-y: scroll; height: 150px; color:black;">
                                            ${Cards[index].answer}    
                                        </p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </section>
                </div>`;
}