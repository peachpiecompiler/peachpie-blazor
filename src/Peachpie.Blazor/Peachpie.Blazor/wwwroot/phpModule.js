// PHP Context management

var callPHPCommand = "CallPHP";

var PHPContext = undefined;

export function setContext(ctx) { PHPContext = ctx; }

export function initialize() {
	window.php = {
		callPHP: function (method, data) { PHPContext.invokeMethod(callPHPCommand, method, JSON.stringify(data)); }
	}
}

// Forms management

var postRequest = false;
var filesPresented = false;

export function isPostRequested() { return postRequest; }

export function isFilesPresented() { return filesPresented; }

var postData = { };
var getData = { };
var filesData = {};

function turnFormToClientSide (form) {
    form.addEventListener("submit", (event) => {
        postData = {};
        getData = {};
        filesData = {};
        postRequest = false;
        filesPresented = false;

        let url = event.target.getAttribute("action");

        let formData = new FormData(form);

        processFormsData(formData, form.getAttribute("method"));

        if (form.getAttribute("method") == "get") {
            if (Object.keys(getData).length > 0)
                url = url + "?" + new URLSearchParams(getData);;
        }
        else if (form.getAttribute("method") == "post") {
            if (postData != undefined)
                postRequest = true;
        }
        else {
            return;
        }

        if (filesData != undefined)
            filesPresented = true;

        navigateFormTo(url);
        event.preventDefault();
        event.stopPropagation();
    });
}

export function turnFormsToClientSide() {
    let forms = document.getElementsByTagName("form");

    for (var i = 0; i < forms.length; i++) {
        turnFormToClientSide(forms[i]);
    }
}

function processFormsData(formData, method) {
    for (var pair of formData.entries()) {
        if (pair[1] instanceof File) {
            filesData[pair[0]] = files.addFile(pair[1]);
        }
        else {
            if (method == "get") {
                getData[pair[0]] = pair[1];
            }
            else if (method == "post")
                postData[pair[0]] = pair[1];
        }
    }
}

export function getPostData() {
    return postData;
}

export function getFilesData() {
    let result = [];

    for (var key in filesData) {
        if (filesData.hasOwnProperty(key)) {
            let struct = files.createStructure(filesData[key])
            struct["fieldName"] = key;
            result.push(struct);
        }
    }

    filesData = {};
    filesPresented = false;

    return result;
}

//File management

var files = {};
var urlObjects = {};
var nextFileId = 0;

function addFile(file) {
    files[nextFileId] = file;
    return nextFileId++;
}

function createStructure(fileId) {
    let file = files[fileId];
    return {
        "id": fileId,
        "name": file.name,
        "size": file.size,
        "type": file.type
    };
}

export function readAllFileAsBase64(fileId) {
    return new Promise((resolve, reject) => {

        const reader = new FileReader();
        reader.onload = function (e) {
            resolve(arrayBufferToBase64(e.target.result));
        };

        reader.onerror = reject;

        reader.readAsArrayBuffer(files[fileId]);
    });
}

export function createUrlObject(fileId) {
    if (!(fileId in urlObjects))
        urlObjects[fileId] = URL.createObjectURL(files[fileId]);

    return urlObjects[fileId];
}

export function downloadFile(fileId) {
    var a = document.createElement('a');
    a.href = createUrlObject(fileId);
    a.download = files[fileId].name;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
}

export function createFile(data, type, name) {
    let file = new File([data], name, {
        type: type,
    });

    return createStructure(addFile(file));
}

function navigateFormTo(url) {
    var a = document.createElement('a');
    a.href = url;
    document.body.appendChild(a);
    a.click();
    document.body.removeChild(a);
}

//https://stackoverflow.com/questions/9267899/arraybuffer-to-base64-encoded-string
function arrayBufferToBase64(buffer) {
    var binary = '';
    var bytes = new Uint8Array(buffer);
    var len = bytes.byteLength;
    for (var i = 0; i < len; i++) {
        binary += String.fromCharCode(bytes[i]);
    }
    return window.btoa(binary);
}



