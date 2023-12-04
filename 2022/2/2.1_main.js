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

// method to convert the second letter into the same later as the one of the opponent
const convertLetter = letter => {
    switch (letter) {
        case 'X':
            return 'A'
        case 'Y':
            return 'B'
        case 'Z':
            return 'C'
        default:
            break;
    }
}

// call the conversion method on the existing input array
arrayOfArrays.forEach((array) => {
    array[1] = convertLetter(array[1])
})

const result = game => {
    switch (game[1]) {
        case 'A':
            return playRockAgainst(game[0]) + 1;
        case 'B':
            return playPaperAgainst(game[0]) + 2;
        case 'C':
            return playScissorsAgainst(game[0]) + 3;
        default:
            break;
    }
}

const playRockAgainst = play => {
    switch (play) {
        case 'A':
            return 3;
        case 'B':
            return 0;
        case 'C':
            return 6;
        default:
            break;
    }
}

const playPaperAgainst = play => {
    switch (play) {
        case 'A':
            return 6;
        case 'B':
            return 3;
        case 'C':
            return 0;
        default:
            break;
    }
}

const playScissorsAgainst = play => {
    switch (play) {
        case 'A':
            return 0;
        case 'B':
            return 6;
        case 'C':
            return 3;
        default:
            break;
    }
}

let score = 0;

arrayOfArrays.forEach(game =>{
    score += result(game);
})

console.log(score)