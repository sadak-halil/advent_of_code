package main

import (
	"bytes"
	"fmt"
	"os"
)

// remove the Card X: part of each inout line
func cleanLine(line []byte) []byte {
	idx := bytes.Index(line, []byte(": "))
	line = line[idx+2:]
	return line
}

// split the numbers at the | (pipe) sign
func splitNumbers(line []byte) (winningNumbers []byte, myNumbers []byte) {
	idx := bytes.Index(line, []byte(" | "))
	winningNumbers = line[:idx]
	myNumbers = line[idx+3:]
	return winningNumbers, myNumbers
}

// put numbers into a slice of integers
func extractNumbers(slice []byte) []int {
	var numbers []int
	for _, word := range bytes.Fields(slice) {
		var number int
		for i := 0; i < len(word); i++ {
			number = number*10 + int(word[i]-'0')
		}
		numbers = append(numbers, number)
	}
	return numbers
}

// compare numbers
func winningNumbersCount(line []byte) int {
	winningNumbers, myNumbers := splitNumbers(line)
	winningNumbersInt := extractNumbers(winningNumbers)
	myNumbersInt := extractNumbers(myNumbers)

	var winningNumberCount int = 0

	for _, winningNumber := range winningNumbersInt {
		for _, myNumber := range myNumbersInt {
			if winningNumber == myNumber {
				winningNumberCount++
			}

		}
	}
	return winningNumberCount
}

func main() {

	//read file input
	input, err := os.ReadFile("input.txt")
	if err != nil {
		fmt.Println("Error reading from file", err)
		return
	}

	data := bytes.Split(input, []byte("\n"))

	scratchCards := make([]int, len(data))
	for i := range scratchCards {
		scratchCards[i] = 1
	}

	for idx, line := range data {
		line = cleanLine(line)

		winningNumbers := winningNumbersCount(line)
		amountOfCards := scratchCards[idx]

		if winningNumbers != 0 {
			index := idx + 1
			for i := index; i < index+winningNumbers; i++ {
				scratchCards[i] += amountOfCards
			}
		}
	}
	result := 0
	for i := range scratchCards {
		result += scratchCards[i]
	}
	fmt.Println(result)
}
