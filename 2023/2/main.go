package main

import (
	"bytes"
	"fmt"
	"os"
	"regexp"
	"strconv"
)

func findNumberOfDices(line []byte) map[string]int {

	re := regexp.MustCompile(`(\d+) (red|blue|green)`)

	matches := re.FindAllSubmatch(line, -1)

	numberOfDices := map[string]int{
		"red":   0,
		"green": 0,
		"blue":  0,
	}

	for _, match := range matches {
		diceNumber, err := strconv.Atoi(string(match[1]))
		if err != nil {
			fmt.Println("Error converting to number", err)
			continue
		}
		diceColor := string(match[2])

		if diceNumber > numberOfDices[diceColor] {
			numberOfDices[diceColor] = diceNumber
		}

	}

	return numberOfDices
}

func main() {

	//read file content
	content, err := os.ReadFile("input.txt")
	if err != nil {
		fmt.Println("Error reading from file", err)
		return
	}

	lines := bytes.Split(content, []byte("\n"))

	var answer = 0

	for _, line := range lines {
		power := findNumberOfDices(line)
		answer += (power["red"] * power["green"] * power["blue"])
	}

	fmt.Println(answer)

}
