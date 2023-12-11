package main

import (
	"bufio"
	"fmt"
	"os"
	"strconv"
	"unicode"
)

func processLine(line string) string {

	var firstDigit, secondDigit rune

	//can left to right
	for _, char := range line {
		if unicode.IsDigit(char) {
			firstDigit = char
			break
		}
	}

	//scan right to left
	for i := len(line) - 1; i >= 0; i-- {
		if unicode.IsDigit(rune(line[i])) {
			secondDigit = rune(line[i])
			break
		}
	}

	return string(firstDigit) + string(secondDigit)

}

func main() {

	var answer int = 0

	file, err := os.Open("input.txt")
	if err != nil {
		fmt.Println("Error opening the input file", err)
		return
	}
	defer file.Close()

	scanner := bufio.NewScanner(file)

	for scanner.Scan() {
		line := scanner.Text()
		number, _ := strconv.Atoi(processLine(line))
		answer += number
	}

	if err := scanner.Err(); err != nil {
		fmt.Println("Error reading from file", err)
	}

	fmt.Println(answer)

}
