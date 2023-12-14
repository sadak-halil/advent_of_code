package main

import (
	"bytes"
	"fmt"
	"os"
	"regexp"
	"strconv"
)

func processGameNumber(line []byte) int {
	gameNumberBytes := line[5:bytes.Index(line, []byte(":"))] //takes the bytes between "Game " and ":"
	gameNumber, err := strconv.Atoi(string(gameNumberBytes))
	if err != nil {
		fmt.Println("Error converting to int", err)
		return 0
	}
	return gameNumber
}

func checkGamesPossibility(line []byte) bool {

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

	for color, number := range numberOfDices {

		if color == "red" && number > 12 {
			return false
		} else if color == "green" && number > 13 {
			return false
		} else if color == "blue" && number > 14 {
			return false
		}
	}

	return true
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
		possible := checkGamesPossibility(line)
		if possible {
			answer += processGameNumber(line)
		}
	}

	fmt.Println(answer)

}
