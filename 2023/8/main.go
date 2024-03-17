package main

import (
	"bytes"
	"fmt"
	"os"
)

type Node struct {
	name         []byte
	leftElement  []byte
	rightElement []byte
}

var nodes []Node
var instructions []int

var nodeMap map[string]Node

// Function to calculate the greatest common divisor (GCD) using Euclid's algorithm
func gcd(a, b int) int {
	for b != 0 {
		a, b = b, a%b
	}
	return a
}

// Function to calculate the least common multiple (LCM) of a slice of integers
func lcmSlice(nums []int) int {
	lcm := nums[0] // Initialize the LCM with the first element of the slice
	for i := 1; i < len(nums); i++ {
		lcm = lcm * nums[i] / gcd(lcm, nums[i]) // Update the LCM using the formula: (lcm * nums[i]) / gcd(lcm, nums[i])
	}
	return lcm
}

func findNextNode(name []byte, nextNode int) []byte {
	if node, ok := nodeMap[string(name)]; ok {
		if nextNode != 0 {
			return node.rightElement
		}
		return node.leftElement
	}
	return nil
}

func getInstruction(idx int) int {
	return instructions[idx%len(instructions)]
}

func checkZ(positions []byte) bool {
	return positions[2] == 'Z'
}

func main() {
	// Read file input.txt
	input, err := os.ReadFile("input.txt")
	if err != nil {
		fmt.Println("Error reading from file:", err)
		return
	}

	maps := bytes.Split(input, []byte("\n\n"))

	// Initialize instructions slice directly
	for _, instr := range maps[0] {
		if instr == byte('L') {
			instructions = append(instructions, 0)
		} else {
			instructions = append(instructions, 1)
		}
	}

	coordinates := bytes.Split(maps[1], []byte("\n"))

	// Initialize the map with a reasonable capacity
	nodeMap = make(map[string]Node, len(coordinates))

	// Parse coordinates
	for _, node := range coordinates {
		n := Node{name: node[0:3], leftElement: node[7:10], rightElement: node[12:15]}
		nodes = append(nodes, n)
		nodeMap[string(n.name)] = n
	}

	var positions [][]byte

	// Add all starting positions
	for _, node := range nodes {
		if node.name[2] == 'A' {
			positions = append(positions, node.name)
		}
	}

	cycles := make([]int, len(positions)) //store the cycles required for each position to reach Z

	//how many cycles it takes for each starting position to reach 'Z'
	for idx, position := range positions {
		steps := 0
		for !checkZ(position) {
			instruction := getInstruction(steps)
			position = findNextNode(position, instruction)
			steps++
		}
		cycles[idx] = steps
		fmt.Println("Steps taken:", steps)
	}

	fmt.Println(lcmSlice(cycles))

}
