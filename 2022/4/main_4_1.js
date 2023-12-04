import input from "./input.js"

// transform the input into an array of strings
const arrayOfStrings = input.split('\n').map(str =>{
    return (str)
});

// regex pattern to get only numbers
const numberPattern = /\d+/g;

// overlap counter
let fullOverlaps = 0;

// logic to check wheteher one areas is contained entirely within the other and turn the strings into number with +
arrayOfStrings.forEach(pair => {
    const areas = pair.match(numberPattern);
    if ((+areas[0] === +areas[2]) || (+areas[1] === +areas[3]) || ((+areas[0] < +areas[2]) && (+areas[1] > +areas[3])) || ((+areas[0] > +areas[2]) && (+areas[1] < +areas [3])))
    {
        console.log (areas)
        fullOverlaps += 1
    }
})

console.log(fullOverlaps)

