function warnToast(str) {
    const guid = getUniqueId();
    const html =
        `<div id="${guid}" class="toast row show alert alert-danger mb-0" 
            style="z-index:100; 
                display:block; width:30em; font-size:16px;">
            <button type="button" class="ml-2 mb-1 close" onclick="removeToast('${guid}')">
                <span aria-hidden="true">&times;</span>
            </button>
            <div class="toast-body">${str}
            </div>
        </div>`;
    const toastElement = document.createElement("div");
    toastElement.innerHTML = html;
    document.getElementById("toaster").appendChild(toastElement);
    $(`#${guid}`).hide();
    $(`#${guid}`).fadeIn(200);


    setTimeout(() => {
        document.getElementById(`${guid}`).remove();
    }, 6000)
}

function successToast(str) {
    const guid = getUniqueId();
    const html =
        `<div id="${guid}" class="toast row show alert alert-success mb-0" 
            style="z-index:100; 
                display:block; width:30em; font-size:16px;">
            <button type="button" class="ml-2 mb-1 close" onclick="removeToast('${guid}')">
                <span aria-hidden="true">&times;</span>
            </button>
            <div class="toast-body">${str}
            </div>
        </div>`;
    const toastElement = document.createElement("div");
    toastElement.innerHTML = html;
    document.getElementById("toaster").appendChild(toastElement);
    $(`#${guid}`).hide();
    $(`#${guid}`).fadeIn(200);


    setTimeout(() => {
        document.getElementById(`${guid}`).remove();
    }, 6000)
}

function getUniqueId() {
    return 'xxxxxxxxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

function removeToast(guid) {
    document.getElementById(`${guid}`).remove();
}