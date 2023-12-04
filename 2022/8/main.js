import {grid} from './input.js'

// scenic score for all outer trees is 0

function countVisibileTrees(row,col) {

    let scenicScore = 1;
    let count = 0;

    // left
    for (let i = col-1; i>=0; i--) {
        count++
        if (grid[row][i] >= grid[row][col]) break;
    }
    scenicScore *= count;
    
    // right
    count = 0;
    for (let i = col+1; i < grid.length; i++) {
        count++;
        if (grid[row][i] >= grid[row][col]) break;
    }
    scenicScore *= count;

    // up
    count = 0;
    for (let i = row - 1; i>=0; i--) {
        count++;
        if (grid[i][col] >= grid[row][col]) break;
    }
    scenicScore *= count;

    // down
    count = 0;
    for (let i = row + 1; i < grid.length; i++) {
        count++;
        if (grid[i][col] >= grid[row][col]) break;
    }
    scenicScore *= count;

    return scenicScore;
}

let bestScenicScore = 0;

for (let row = 1; row < grid.length - 1; row++) {
    for (let col = 1; col < grid.length - 1; col++) {
        let scenicScore = countVisibileTrees(row,col);
        if (scenicScore > bestScenicScore) {
            bestScenicScore = scenicScore;
        }
    }
}

console.log(bestScenicScore)

