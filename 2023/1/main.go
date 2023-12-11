package main

import (
	"bytes"
	"fmt"
	"os"
	"strconv"
)

func processLine(line []byte) int {

	var firstDigit, secondDigit byte
	digits := []byte("123456789")
	words := map[string]byte{
		"one":   '1',
		"two":   '2',
		"three": '3',
		"four":  '4',
		"five":  '5',
		"six":   '6',
		"seven": '7',
		"eight": '8',
		"nine":  '9',
	}

	l := len(line)

	for i, r := range line {
		if bytes.Contains(digits, []byte{r}) {
			if firstDigit == 0 {
				firstDigit = r
			}
			secondDigit = r
			continue
		}
		for key, value := range words {
			if r == key[0] {
				if i+len(key)-1 > l-1 {
					continue
				}

				if !bytes.Equal(line[i:i+len(key)], []byte(key)) {
					continue
				}

				if firstDigit == 0 {
					firstDigit = value
				}

				secondDigit = value
				break

			}
		}
	}
	i, _ := strconv.Atoi(string(firstDigit) + string(secondDigit))
	return i
}

func main() {

	var answer int = 0

	file, err := os.ReadFile("input.txt")
	if err != nil {
		fmt.Println("Error reading the file", err)
		return
	}

	for _, line := range bytes.Split(file, []byte("\n")) {
		answer += processLine(line)
	}

	fmt.Println(answer)

}
