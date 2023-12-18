package main

import (
	"bytes"
	"fmt"
	"os"
)

func findAsterisk(line []byte) []int {

	var asterisks []int

	for i, symbol := range line {
		if symbol == '*' {
			asterisks = append(asterisks, i)
		}
	}

	return asterisks
}

func extractNumbers(line []byte) []struct {
	Number     int
	StartIndex int
	EndIndex   int
} {
	var numbers []struct {
		Number     int
		StartIndex int
		EndIndex   int
	}

	currentNumber := 0
	startIndex := -1

	for i, symbol := range line {
		if symbol >= '0' && symbol <= '9' {
			if startIndex == -1 {
				startIndex = i
			}
			currentNumber = currentNumber*10 + int(symbol-'0')
		} else {
			if startIndex != -1 {
				numbers = append(numbers, struct {
					Number     int
					StartIndex int
					EndIndex   int
				}{
					Number:     currentNumber,
					StartIndex: startIndex,
					EndIndex:   i - 1,
				})
				currentNumber, startIndex = 0, -1
			}
		}
	}

	if startIndex != -1 {
		numbers = append(numbers, struct {
			Number     int
			StartIndex int
			EndIndex   int
		}{Number: currentNumber, StartIndex: startIndex, EndIndex: len(line) - 1})
	}

	return numbers
}

func checkIfGear(previousLine []byte, line []byte, nextLine []byte, index int) int {

	previousLineNumbers := extractNumbers(previousLine)
	currentLineNumbers := extractNumbers(line)
	nextLineNumbers := extractNumbers(nextLine)

	type Gear struct {
		Ratio                  int
		NumberOfConnectedParts int
	}

	gear := Gear{
		Ratio:                  1,
		NumberOfConnectedParts: 0,
	}

	for _, number := range previousLineNumbers {
		if (number.StartIndex <= index && number.EndIndex >= index) || number.EndIndex == index-1 || number.StartIndex == index+1 {
			gear.Ratio *= number.Number
			gear.NumberOfConnectedParts++
		}
	}

	for _, number := range currentLineNumbers {
		if number.StartIndex == index+1 || number.EndIndex == index-1 {
			gear.Ratio *= number.Number
			gear.NumberOfConnectedParts++
		}
	}

	for _, number := range nextLineNumbers {
		if (number.StartIndex <= index && number.EndIndex >= index) || number.EndIndex == index-1 || number.StartIndex == index+1 {
			gear.Ratio *= number.Number
			gear.NumberOfConnectedParts++
		}
	}

	if gear.NumberOfConnectedParts == 2 {
		return gear.Ratio
	}

	return 0 //returning 0 in order not to change the sum in case the gear is not connected to 2 parts
}

func main() {

	//read file input
	input, err := os.ReadFile("input.txt")
	if err != nil {
		fmt.Println("Error reading from file", err)
		return
	}

	data := bytes.Split(input, []byte("\n"))

	var result int = 0

	for li, line := range data {
		if li == 0 || li == len(data)-1 {
			continue //we know for a fact that first and last line do not have asterisks
		}
		asterisks := findAsterisk(line)
		for _, asteriskIndex := range asterisks {
			result += checkIfGear(data[li-1], line, data[li+1], asteriskIndex)
		}
	}
	fmt.Println(result)
}
