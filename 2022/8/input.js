import * as fs from 'fs'

export const grid = fs.readFileSync('./input.txt').toString().split('\n').map(row =>{
    return row.split("")
});
