package main

import (
	"bytes"
	"fmt"
	"os"
	"sort"
	"strconv"
)

var strength = map[byte]int{
	'A': 14,
	'K': 13,
	'Q': 12,
	'J': 11,
	'T': 10,
	'9': 9,
	'8': 8,
	'7': 7,
	'6': 6,
	'5': 5,
	'4': 4,
	'3': 3,
	'2': 2,
	'1': 1,
}

const (
	FIVE_OF_A_KIND  = 7
	FOUR_OF_A_KIND  = 6
	FULL_HOUSE      = 5
	THREE_OF_A_KIND = 4
	TWO_PAIR        = 3
	ONE_PAIR        = 2
	HIGH_CARD       = 1
)

type Hand struct {
	cards    string
	bid      int
	bestHand int
}

func getBestHand(cards string) int {
	m := map[rune]int{}
	for _, c := range cards {
		m[c]++
	}

	if len(m) == 1 {
		return FIVE_OF_A_KIND
	}

	if len(m) == 2 {
		for _, v := range m {
			if v == 4 {
				return FOUR_OF_A_KIND
			}
		}
		return FULL_HOUSE
	}

	for _, v := range m {
		if v == 3 {
			return THREE_OF_A_KIND
		} else if v == 2 {
			if len(m) == 3 {
				return TWO_PAIR
			}
			return ONE_PAIR
		}
	}

	return HIGH_CARD
}

func main() {

	//read file input
	input, err := os.ReadFile("input.txt")
	if err != nil {
		fmt.Println("Error reading from file", err)
		return
	}

	hands := []Hand{}

	//32T3K 765
	lines := bytes.Split(input, []byte("\n"))
	for _, line := range lines {

		split := bytes.Split(line, []byte(" "))

		cards := string(split[0])
		bid, err := strconv.Atoi(string(split[1]))

		if err != nil {
			panic(err)
		}

		h := Hand{cards: cards, bid: bid, bestHand: getBestHand(cards)}

		hands = append(hands, h)

	}

	sort.Slice(hands, func(i, j int) bool {
		if hands[i].bestHand == hands[j].bestHand {
			for k := range hands[i].cards {
				if hands[i].cards[k] == hands[j].cards[k] {
					continue
				}
				return strength[hands[i].cards[k]] < strength[hands[j].cards[k]]
			}
		}
		return hands[i].bestHand < hands[j].bestHand
	})

	var score int = 0

	for i, hand := range hands {
		score += (i + 1) * hand.bid
	}

	fmt.Printf("%v", score)
}
