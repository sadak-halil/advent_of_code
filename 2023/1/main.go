package main

import (
	"bufio"
	"fmt"
	"os"
	"strconv"
	"strings"
	"unicode"
)

func main() {

	var result int = 0

	file, err := os.Open("input.txt")
	if err != nil {
		fmt.Println("Error opening the file: ", err)
		return
	}

	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		line := scanner.Text()
		processedNumber, err := strconv.Atoi(processLine(line))
		if err != nil {
			fmt.Println("Error converting the number: ", err)
			continue
		}

		result += processedNumber
	}
	fmt.Println(result)
	defer file.Close()
}

func processLine(line string) string {
	digits := strings.Map(func(r rune) rune {
		if unicode.IsDigit(r) {
			return r
		}
		return -1
	}, line)

	if len(digits) == 0 {
		return ""
	}

	if len(digits) == 1 {
		return digits + digits
	}
	return string(digits[0]) + string(digits[len(digits)-1])
}
