var currentpos, timer;
function initialize() {
    timer = setInterval("scrollwindow()", 10);
}
function clr() {
    clearInterval(timer);
}
function scrollwindow() {
    currentpos = document.body.scrollTop;
    window.scroll(0, ++currentpos);
    if (currentpos != document.body.scrollTop)
        clr();
}
document.onmousedown = clr
document.ondblclick = initialize
function execute(methodName, parameters) {
    wpfObj.execute(methodName, parameters);
}