import { instructions, stacks } from "./input.js";

// regex pattern to get only numbers
const numberPattern = /\d+/g;

// transform the instructions into an array of strings
const arrayOfStrings = instructions.split('\n').map(str =>{
    return (str)
});

// simplify the instructions down to numbers only
let simplifiedInstructions = [];

arrayOfStrings.forEach(line => {
    const instruction = line.match(numberPattern);
    simplifiedInstructions.push(instruction);
})

// function to execute the move/s
const move = (count, stack1, stack2) => {
    for(let i=0; i<+count; i++){
        stacks[+stack2].push(stacks[+stack1].pop())
    }
}

// iteratte through the instructions and call the move function on each line
simplifiedInstructions.forEach(instruction => {
    move(instruction[0],instruction[1],instruction[2])
})

// remove the first array from the list of arrays which was there in order to compensate for the 0 based indexing of arrays
stacks.shift();

stacks.forEach(stack => {
    console.log(stack.pop())
})