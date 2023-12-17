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
	// fmt.Println("1: ", string(previousLine), "\n2: ", string(line), "\n3: ", string(nextLine), "\nstart: ", startIndex, "\nend: ", endIndex)
	// if endIndex == len(line)-1 {
	// 	fmt.Println("previous line: ", string(previousLine[startIndex:endIndex+1]), "\nprevious symbol: ", string([]byte{line[startIndex]}), "\nnext line: ", string(nextLine[startIndex:endIndex+1]))
	// } else if startIndex == 0 {
	// 	fmt.Println("previous line: ", string(previousLine[startIndex:endIndex+1]), "\nnext symbol: ", string([]byte{line[endIndex]}), "\nnext line: ", string(nextLine[startIndex:endIndex+1]))
	// } else {
	// 	fmt.Println("previous line: ", string(previousLine[startIndex:endIndex+1]), "\nprevious symbol: ", string([]byte{line[startIndex]}), "\nnext symbol: ", string([]byte{line[endIndex]}), "\nnext line: ", string(nextLine[startIndex:endIndex+1]))
	// }

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

	// numbers := extractNumbers(data[2])

	// for _, number := range numbers {
	// 	result := checkForSurroundingSymbols(data[1], data[2], data[3], number.StartIndex, number.EndIndex)
	// 	fmt.Println(number.Number, result)
	// }

	periods := make([]byte, len(data[0])-1)
	for i := range periods {
		periods[i] = '.'
	}

	var result int = 0

	for li, line := range data {
		numbers := extractNumbers(line)
		if li == 0 {
			// fmt.Println("0 case", li)
			for _, number := range numbers {
				if checkForSurroundingSymbols(periods, line, data[li+1], number.StartIndex, number.EndIndex) {
					// fmt.Println("found a not surrounded number", number.Number)
					result += number.Number
				}
			}
			continue
		} else if li == len(data)-1 {
			// fmt.Println("len case", li)
			for _, number := range numbers {
				if checkForSurroundingSymbols(data[li-1], line, periods, number.StartIndex, number.EndIndex) {
					// fmt.Println("found a not surrounded number", number.Number)
					result += number.Number
				}
			}
			break
		} else {
			// fmt.Println("regular case", li)
			for _, number := range numbers {
				if checkForSurroundingSymbols(data[li-1], line, data[li+1], number.StartIndex, number.EndIndex) {
					// fmt.Println("found a not surrounded number", number.Number)
					result += number.Number
				}
			}
		}
	}

	fmt.Println(result)

}
