var firstInput = document.querySelector('.first-input');
var secondInput = document.querySelector('.second-input');
var functionalInps = document.querySelectorAll('.functional');
var show = document.querySelector('.result');

console.log(show)
function calc() {
    firstInput = Number(document.getElementById('first-input').value);
    secondInput = Number(document.getElementById('second-input').value);
    functionalInps = document.getElementsByClassName('functional').value;
    var result;
    switch (functionalInps) {
        case '+':
            result = (Number(firstInput) + Number(secondInput));
            break;
        case '-':
            result = Number(firstInput) - Number(secondInput);
            break;
        case '*':
            result = Number(firstInput) * Number(secondInput);
            break;
        case '/':
            result = Number(firstInput) / Number(secondInput);
            break;

    }

    show.innerHTML += result;

}
functionalInps.forEach(function (functionalInp) {
    functionalInp.addEventListener("click", function () {
        calc();
    });
});