import {grid} from './input.js'

// since the grid is a square the below formula calculates the count of outer trees
const outerTrees = grid.length * 4 - 4;
let visibleTrees = outerTrees;

function checkVisibility(row,col,fromDirection) {

    if (fromDirection === 'left') {
        let visible = true;
        for (let i=0; i<col; i++) {
            if (grid[row][i] < grid[row][col]) {
                continue;
            }
            visible = false;
            break;
        }

        return (visible);
    }
  
    if (fromDirection === 'right') {
        let visible = true;
        for (let i = grid.length-1; i > col; i--) {
            if (grid[row][i] < grid[row][col]) {
                continue;
            }
            visible = false
            break;
        }
   
        return (visible)
    }

    if (fromDirection === 'top') {
        let visible = true;
        for (let i=0; i<row; i++) {
            if (grid[i][col] < grid[row][col]) {
                continue;
            }
            visible = false;
            break;
        }

        return (visible)
    }

    if (fromDirection === 'bottom') {
        let visible = true;
        for (let i=grid.length-1; i>row; i--) {
            if (grid[i][col] < grid[row][col]) {
                continue;
            }
            visible = false;
            break;
        }

        return (visible)
    }

}

for (let row = 1; row < grid.length - 1; row++) {
    for (let col = 1; col < grid.length - 1; col++) {
        if (checkVisibility(row, col, 'left')) {
            visibleTrees++
        }
        else if (checkVisibility(row, col, 'right')) {
            visibleTrees++
        }
        else if (checkVisibility(row, col, 'top')) {
            visibleTrees++
        }
        else if (checkVisibility(row, col, 'bottom')) {
            visibleTrees++
        }
    }
}

console.log(visibleTrees)

// for debugging: console.log(`row: ${row}, col: ${col}, i: ${i}, input: ${grid[row][col]}, comparing with: ${grid[row][i]}`);

