function combo(e, value, theinput) {
    e = e || window.event;
    theinput = document.getElementById(theinput);
    theinput.value = value;
    e.preventDefault ? e.preventDefault() : (e.returnValue = false)
}