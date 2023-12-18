package main

import (
	"bytes"
	"fmt"
	"os"
	"unicode"
)

func checkIfSymbols(r []byte) bool {
	for _, s := range r {
		if !unicode.IsDigit(rune(s)) && s != '.' {
			return true
		}
	}
	return false
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

func checkForSurroundingSymbols(previousLine []byte, line []byte, nextLine []byte, startIndex int, endIndex int) bool {
	if startIndex != 0 {
		startIndex -= 1
	}
	if endIndex != len(line)-1 {
		endIndex += 1
	}

	if startIndex == 0 {
		if checkIfSymbols(previousLine[startIndex:endIndex+1]) || checkIfSymbols([]byte{line[endIndex]}) || checkIfSymbols(nextLine[startIndex:endIndex+1]) {
			return true
		}
		return false
	}

	if endIndex == len(line)-1 {
		if checkIfSymbols(previousLine[startIndex:endIndex+1]) || checkIfSymbols([]byte{line[startIndex]}) || checkIfSymbols(nextLine[startIndex:endIndex+1]) {
			return true
		}
		return false
	}

	if checkIfSymbols(previousLine[startIndex:endIndex+1]) || checkIfSymbols([]byte{line[startIndex]}) || checkIfSymbols([]byte{line[endIndex]}) || checkIfSymbols(nextLine[startIndex:endIndex+1]) {
		return true
	}

	return false

}

func main() {

	//read file input
	input, err := os.ReadFile("input.txt")
	if err != nil {
		fmt.Println("Error reading from file", err)
		return
	}

	data := bytes.Split(input, []byte("\n"))

	periods := make([]byte, len(data[0])-1)
	for i := range periods {
		periods[i] = '.'
	}

	var result int = 0

	for li, line := range data {
		numbers := extractNumbers(line)
		if li == 0 {
			for _, number := range numbers {
				if checkForSurroundingSymbols(periods, line, data[li+1], number.StartIndex, number.EndIndex) {
					result += number.Number
				}
			}
			continue
		} else if li == len(data)-1 {
			for _, number := range numbers {
				if checkForSurroundingSymbols(data[li-1], line, periods, number.StartIndex, number.EndIndex) {
					result += number.Number
				}
			}
			break
		} else {
			for _, number := range numbers {
				if checkForSurroundingSymbols(data[li-1], line, data[li+1], number.StartIndex, number.EndIndex) {
					result += number.Number
				}
			}
		}
	}

	fmt.Println(result)

}
