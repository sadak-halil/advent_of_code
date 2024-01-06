package main

import (
	"bytes"
	"fmt"
	"os"
	"strconv"
)

func extractIntegersFromSection(section []byte) []int {
	fields := bytes.Fields(section)
	var integers []int
	for _, field := range fields {
		if num, err := strconv.Atoi(string(field)); err == nil {
			integers = append(integers, num)
		}
	}
	return integers
}

func getTimeAndDistance(input []byte) ([]int, []int) {

	sections := bytes.Split(input, []byte("\n"))
	var timeSlice, distanceSlice []int

	for _, section := range sections {
		if bytes.HasPrefix(section, []byte("Time:")) {
			timeSlice = extractIntegersFromSection(section)
		} else if bytes.HasPrefix(section, []byte("Distance:")) {
			distanceSlice = extractIntegersFromSection(section)
		}
	}
	return timeSlice, distanceSlice
}

func waysToBeatRecord(time int, distance int) int {
	numberOfWays := 0
	for i := 0; i < time; i++ {
		if i*(time-i) > distance {
			numberOfWays++
		}
	}
	return numberOfWays
}

func main() {

	//read file input
	input, err := os.ReadFile("input.txt")
	if err != nil {
		fmt.Println("Error reading from file", err)
		return
	}

	result := 1

	times, records := getTimeAndDistance(input)

	for i, time := range times {
		result *= waysToBeatRecord(time, records[i])
	}

	fmt.Println(result)

}
