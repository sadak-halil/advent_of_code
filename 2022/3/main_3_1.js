import input from "./input.js"
import priorities from "./priorities.js";

// transform the input into an array of strings
const arrayOfStrings = input.split('\n').map(str =>{
    return (str)
});

// initiate a new array where we can push the converted arrays
const arrayOfArrays = [];

// add each backpack as an array of two strings to arrayOfArrays
arrayOfStrings.forEach(string => {
    let backpack = [];
    backpack.push(string.slice(0,string.length/2))
    backpack.push(string.slice((string.length/2),string.length))
    arrayOfArrays.push(backpack)
})

// find the repeated character in two strings and return its priority
const repeatedChar = (string1, string2) => {
    // use Set to remove repeating letters
    const letters1 = [...new Set(string1.split(''))];
    const letters2 = [...new Set(string2.split(''))];
    let repeated;
    letters1.forEach(letter1 =>{
        letters2.forEach(letter2 => {
            if (letter1 === letter2) {repeated = priorities[`${letter1}`]}
        })
    })
    return repeated;
}

let result = 0;

arrayOfArrays.forEach(array => {
    result += repeatedChar(array[0],array[1]);
})

console.log(result)