import * as fs from 'fs'

export const commands = fs.readFileSync('/Users/sadakhalil/projects/advent-of-code-22/7/input.txt').toString().split('\n').map(str =>{
    return (str)
});
