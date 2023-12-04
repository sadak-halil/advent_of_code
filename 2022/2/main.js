import input from "./input.js"
// transform the input into an array of arrays with pairs representing the play of each player
const arrayOfStrings = input.split('\n').map(str =>{
    return (str)
});

// initiate a new array where we can push the converted arrays
const arrayOfArrays = [];

// remove the space between the two entries
arrayOfStrings.forEach(string => {
    let str = string.split('')
    str.splice(1,1)
    arrayOfArrays.push(str)
})

const result = game => {
    switch (game[1]) {
        case 'X':
            return looseAgainst(game[0]);
        case 'Y':
            return drawAgainst(game[0]) + 3;
        case 'Z':
            return winAgainst(game[0]) + 6;
        default:
            break;
    }
}

const looseAgainst = play => {
    switch (play) {
        case 'A':
            return 3;
        case 'B':
            return 1;
        case 'C':
            return 2;
        default:
            break;
    }
}

const drawAgainst = play => {
    switch (play) {
        case 'A':
            return 1;
        case 'B':
            return 2;
        case 'C':
            return 3;
        default:
            break;
    }
}

const winAgainst = play => {
    switch (play) {
        case 'A':
            return 2;
        case 'B':
            return 3;
        case 'C':
            return 1;
        default:
            break;
    }
}

let score = 0;

arrayOfArrays.forEach(game =>{
    score += result(game);
})

console.log(score)