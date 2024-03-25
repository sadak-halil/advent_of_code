package main

import (
	"bufio"
	"fmt"
	"os"
	"strconv"
	"strings"
)

func readFromFile(filename string) ([][]int, error) {
	file, err := os.Open(filename)
	if err != nil {
		return nil, err
	}
	defer file.Close()

	var intSlices [][]int
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		line := scanner.Text()
		intSlice, err := parseIntSlice(line)
		if err != nil {
			return nil, err
		}
		intSlices = append(intSlices, intSlice)
	}
	if err := scanner.Err(); err != nil {
		return nil, err
	}
	return intSlices, nil
}

func parseIntSlice(line string) ([]int, error) {
	var intSlice []int
	fields := strings.Fields(line)
	for _, field := range fields {
		num, err := strconv.Atoi(field)
		if err != nil {
			return nil, err
		}
		intSlice = append(intSlice, num)
	}
	return intSlice, nil
}

func checkIfAllAreEqual(s []int, v int) bool {
	for _, i := range s {
		if i != v {
			return false
		}
	}
	return true
}

func findNextSlice(s []int) []int {
	n := make([]int, len(s)-1) // length of new array is always one member shorter
	for i := len(s) - 1; i > 0; i-- {
		n[i-1] = s[i] - s[i-1]
	}
	return n
}

func main() {
	filename := "input.txt"
	input, err := readFromFile(filename)
	if err != nil {
		fmt.Println("Error:", err)
		return
	}

	result := 0

	for _, line := range input {
		sum := line[len(line)-1]
		for !checkIfAllAreEqual(line, line[0]) {
			line = findNextSlice(line)
			sum += line[len(line)-1]
		}
		result += sum
	}

	fmt.Println(result)

}
