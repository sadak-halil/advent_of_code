import input from "./input.js"

const arrayOfNumbers = input.split('\n').map(str =>{
    return parseInt(str)
});

let maxCalories = 0;
let calories = 0;

// for (let i=0; i<arrayOfNumbers.length; i++){
//         if (Object.is(arrayOfNumbers[i],NaN)) {console.log(arrayOfNumbers[i])}
// }

const isMaxCalories = cal =>{
    if (maxCalories < cal) { maxCalories = cal}
}

arrayOfNumbers.forEach(element =>{
    if (Object.is(element, NaN)) {
        calories = 0
    } else {
        calories += element;
        isMaxCalories(calories);
    }
})

console.log(maxCalories)