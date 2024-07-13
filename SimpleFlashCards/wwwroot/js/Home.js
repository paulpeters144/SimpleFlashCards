function moveCardsRight() {
    $("#cardContainer").hide().show("drop", { direction: "left" }, 350);
}

function moveCardsLeft() {
    $("#cardContainer").hide().show("drop", { direction: "right" }, 350);
}

let Decks;
let PageIndex = 0;
function loadDecks() {
    $.ajax({
        type: "GET",
        url: "/api/home/loaddecks",
        success: function (response) {
            const content = [];
            response.forEach(e => {
                content.push({ "title": e.title, "desc": e.desc })
            });
            Decks = new UserDecks(content);
        },
        error: function (response) {
            alert("Err");
        }
    });
}

function createDeck() {
    const title = document.getElementById("titleInput").value;
    const description = document.getElementById("descriptionInput").value;
    const deck = { "Title": title, "Desc": description };
    $.ajax({
        type: "POST",
        url: "api/Home/createdeck",
        data: JSON.stringify(deck),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
            console.log("success", response);
        },
        failure: function (response) {
            console.log("fail", response);
        },
        error: function (response) {
            console.log("err", response);
        }
    });
}

class UserDecks {
    constructor(content) {
        this.Content = content;
        this.Display = null;
        this.PageIndex = 0;
        this.DeckRows = 3;
        this.DeckCol = 3;
        this.MaxIndex =
            Math.floor((this.DeckRows * this.DeckCol) /
                content.length);
    }
    GetPageHTML() {
        let result = "";
        let currentIndex = 0;
        for (var i = 0; i < this.DeckRows; i++) {
            let rowHtml = `<div class="row">`;
            for (var x = 0; x < this.DeckCol; x++) {
                currentIndex++;
                if (currentIndex > this.Content.length) {
                    break;
                }
                const title = this.Content[x + PageIndex].title;
                const desc = this.Content[x + PageIndex].desc;
                rowHtml += `<div class="col">
            <div class="img-container toolttip">
                <img src="/img/cardLogo.png" />
                <div class="text-img-center">${title}</div>
                <span class="tooltiptext mt-3">
                    ${desc}
                </span>
            </div>
        </div>`;
            }
            rowHtml += "</div>";
            result += rowHtml;
        }
        document.getElementById("cardContainer").innerHTML = result;
    }
    PageRight() {
        if (this.PageIndex < this.MaxIndex) {
            this.PageIndex++;
        }
    }
    PageLeft() {
        if (this.PageIndex > 0) {
            this.PageIndex--;
        }
    }
}