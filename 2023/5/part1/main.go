package main

import (
	"bytes"
	"fmt"
	"os"
	"strconv"
)

func getMaps(input []byte) ([]int, [][]int, [][]int, [][]int, [][]int, [][]int, [][]int, [][]int) {

	sections := bytes.Split(input, []byte("\n\n"))

	//seeds
	seedSection := sections[0]
	seedValues := bytes.Fields(bytes.TrimPrefix(seedSection, []byte("seeds: ")))
	seeds := make([]int, 0, len(seedValues))
	for _, s := range seedValues {
		seed, err := strconv.Atoi(string(s))
		if err != nil {
			fmt.Println("Error converting seed to integer:", err)
			continue
		}
		seeds = append(seeds, seed)
	}

	//soil
	soilSection := sections[1]
	soilLines := bytes.Split(bytes.TrimSpace(bytes.TrimPrefix(soilSection, []byte("seed-to-soil map:"))), []byte("\n"))
	soilMap := make([][]int, 0)
	for _, line := range soilLines {
		values := bytes.Fields(line)
		soilRow := make([]int, 0, len(values))
		for _, v := range values {
			val, err := strconv.Atoi(string(v))
			if err != nil {
				fmt.Println("Error converting soil value to integer:", err)
				continue
			}
			soilRow = append(soilRow, val)
		}
		soilMap = append(soilMap, soilRow)
	}

	//fertilizer
	fertilizerSection := sections[2]
	fertilizerLines := bytes.Split(bytes.TrimSpace(bytes.TrimPrefix(fertilizerSection, []byte("soil-to-fertilizer map:"))), []byte("\n"))
	fertilizerMap := make([][]int, 0)
	for _, line := range fertilizerLines {
		values := bytes.Fields(line)
		fertilizerRow := make([]int, 0, len(values))
		for _, v := range values {
			val, err := strconv.Atoi(string(v))
			if err != nil {
				fmt.Println("Error converting fertilizer value to integer:", err)
				continue
			}
			fertilizerRow = append(fertilizerRow, val)
		}
		fertilizerMap = append(fertilizerMap, fertilizerRow)
	}

	//water
	waterSection := sections[3]
	waterLines := bytes.Split(bytes.TrimSpace(bytes.TrimPrefix(waterSection, []byte("fertilizer-to-water map:"))), []byte("\n"))
	waterMap := make([][]int, 0)
	for _, line := range waterLines {
		values := bytes.Fields(line)
		waterRow := make([]int, 0, len(values))
		for _, v := range values {
			val, err := strconv.Atoi(string(v))
			if err != nil {
				fmt.Println("Error converting water value to integer:", err)
				continue
			}
			waterRow = append(waterRow, val)
		}
		waterMap = append(waterMap, waterRow)
	}

	//light
	lightSection := sections[4]
	lightLines := bytes.Split(bytes.TrimSpace(bytes.TrimPrefix(lightSection, []byte("water-to-light map:"))), []byte("\n"))
	lightMap := make([][]int, 0)
	for _, line := range lightLines {
		values := bytes.Fields(line)
		lightRow := make([]int, 0, len(values))
		for _, v := range values {
			val, err := strconv.Atoi(string(v))
			if err != nil {
				fmt.Println("Error converting water value to integer:", err)
				continue
			}
			lightRow = append(lightRow, val)
		}
		lightMap = append(lightMap, lightRow)
	}

	//temperature
	temperatureSection := sections[5]
	temperatureLines := bytes.Split(bytes.TrimSpace(bytes.TrimPrefix(temperatureSection, []byte("light-to-temperature map:"))), []byte("\n"))
	temperatureMap := make([][]int, 0)
	for _, line := range temperatureLines {
		values := bytes.Fields(line)
		temperatureRow := make([]int, 0, len(values))
		for _, v := range values {
			val, err := strconv.Atoi(string(v))
			if err != nil {
				fmt.Println("Error converting temperature value to integer:", err)
				continue
			}
			temperatureRow = append(temperatureRow, val)
		}
		temperatureMap = append(temperatureMap, temperatureRow)
	}

	//humidity
	humiditySection := sections[6]
	humidityLines := bytes.Split(bytes.TrimSpace(bytes.TrimPrefix(humiditySection, []byte("temperature-to-humidity map:"))), []byte("\n"))
	humidityMap := make([][]int, 0)
	for _, line := range humidityLines {
		values := bytes.Fields(line)
		humidityRow := make([]int, 0, len(values))
		for _, v := range values {
			val, err := strconv.Atoi(string(v))
			if err != nil {
				fmt.Println("Error converting humidity value to integer:", err)
				continue
			}
			humidityRow = append(humidityRow, val)
		}
		humidityMap = append(humidityMap, humidityRow)
	}

	//location
	locationSection := sections[7]
	locationLines := bytes.Split(bytes.TrimSpace(bytes.TrimPrefix(locationSection, []byte("humidity-to-location map:"))), []byte("\n"))
	locationMap := make([][]int, 0)
	for _, line := range locationLines {
		values := bytes.Fields(line)
		locationRow := make([]int, 0, len(values))
		for _, v := range values {
			val, err := strconv.Atoi(string(v))
			if err != nil {
				fmt.Println("Error converting location value to integer:", err)
				continue
			}
			locationRow = append(locationRow, val)
		}
		locationMap = append(locationMap, locationRow)
	}

	return seeds, soilMap, fertilizerMap, waterMap, lightMap, temperatureMap, humidityMap, locationMap
}

func checkMapping(mapping [][]int, input int) int {
	for _, line := range mapping {
		if (input >= line[1]) && (input < line[1]+line[2]) {
			return (line[0] + (input - line[1]))
		}
	}
	return input
}

func main() {

	//read file input
	input, err := os.ReadFile("input.txt")
	if err != nil {
		fmt.Println("Error reading from file", err)
		return
	}

	var closestLocation int = 0

	seeds, soilMap, fertilizerMap, waterMap, lightMap, temperatureMap, humidityMap, locationMap := getMaps(input)

	for _, seed := range seeds {
		soil := checkMapping(soilMap, seed)
		fertilzer := checkMapping(fertilizerMap, soil)
		water := checkMapping(waterMap, fertilzer)
		light := checkMapping(lightMap, water)
		temperature := checkMapping(temperatureMap, light)
		humidity := checkMapping(humidityMap, temperature)
		location := checkMapping(locationMap, humidity)
		if closestLocation == 0 {
			closestLocation = location
		}
		if closestLocation > location {
			closestLocation = location
		}
	}
	fmt.Println("Closest Location:", closestLocation)
}
