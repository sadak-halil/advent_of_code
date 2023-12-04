import {data} from './input.js'

// regex to check for repeateing characters in a string. returns 'true' in case there are any
const check = (str) => {
    return /(.).*\1/.test(str);
}

let position = -1; // will serve as a break for the while loop
let i = 0;

while (position === -1) {
    if (check(data.slice(i,i+4))) {
        i++;
        continue;
    } else {
    position = i + 4;
    console.log(`position is: ${position}`);
    }
}

