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

func findNextNode(name []byte, nextNode int) []byte {
	for _, node := range nodes {
		if bytes.Equal(node.name, name) {
			if nextNode != 0 {
				return node.rightElement
			}
			return node.leftElement
		}
	}
	return nil
}

func getInstruction(idx int) int {
	return instructions[idx%len(instructions)]
}

func main() {

	//read file input
	input, err := os.ReadFile("input.txt")
	if err != nil {
		fmt.Println("Error reading from file", err)
		return
	}

	maps := bytes.Split(input, []byte("\n\n"))

	for _, instruction := range maps[0] {
		if instruction == byte('L') {
			instructions = append(instructions, 0)
		} else {
			instructions = append(instructions, 1)
		}
	}

	coordinates := bytes.Split(maps[1], []byte("\n"))

	//AAA = (BBB, CCC)
	for _, node := range coordinates {
		n := Node{name: node[0:3], leftElement: node[7:10], rightElement: node[12:15]}
		nodes = append(nodes, n)
	}

	var step int = 0
	var coordinate []byte = []byte("AAA")

	for !bytes.Equal(coordinate, []byte("ZZZ")) {
		instruction := getInstruction(step)
		step++
		coordinate = findNextNode(coordinate, instruction)
	}

	fmt.Println(step)

}
