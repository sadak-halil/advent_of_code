package main

import (
	"bytes"
	"fmt"
	"math"
	"os"
)

func cleanLine(line []byte) []byte {
	idx := bytes.Index(line, []byte(": "))
	line = line[idx+2:]
	return line
}

func splitNumbers(line []byte) (winningNumbers []byte, myNumbers []byte) {
	idx := bytes.Index(line, []byte(" | "))
	winningNumbers = line[:idx]
	myNumbers = line[idx+3:]
	return winningNumbers, myNumbers
}

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

	result := 0

	for _, line := range data {
		line = cleanLine(line)

		winningNumbers := winningNumbersCount(line)

		if winningNumbers != 0 {
			lineScore := math.Pow(2, float64(winningNumbers)-1)
			result += int(lineScore)
		}
	}

	fmt.Println(result)

}
