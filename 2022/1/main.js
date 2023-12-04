import input from "./input.js"

const arrayOfNumbers = input.split('\n').map(str =>{
    return parseInt(str)
});

let sums = [];
let sum = 0;

arrayOfNumbers.forEach(element =>{
    if (Object.is(element, NaN)) {
        sums.push(sum);
        sum = 0
    } else {
        sum += element;
    }
})

sums = sums.sort((a,b)=>b-a);

console.log(sums[0]+sums[1]+sums[2])