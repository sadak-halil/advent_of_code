import input from "./input.js"
import priorities from "./priorities.js";

// transform the input into an array of strings
const arrayOfStrings = input.split('\n').map(str =>{
    return (str)
});

// initiate a new array where we can push the converted arrays
let arrayOfArrays = [];

// split the array into arrays with 3 backpacks each
let group;
while (arrayOfStrings.length>0){
    group = arrayOfStrings.splice(0,3)
    arrayOfArrays.push(group);
}


// find the repeated character in three strings and return its priority
const repeatedChar = (string1, string2, string3) => {
    // use Set to remove repeating letters
    const letters1 = [...new Set(string1.split(''))];
    const letters2 = [...new Set(string2.split(''))];
    const letters3 = [...new Set(string3.split(''))];
    const repeated = letters1.filter(letter => letters2.includes(letter) && letters3.includes(letter))
    
    return priorities[`${repeated[0]}`];
}

let result = 0;

arrayOfArrays.forEach(array => {
    result += repeatedChar(array[0],array[1],array[2]);
})

console.log(result)