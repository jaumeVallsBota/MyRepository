//En este ejemplo:

//    intSeq retorna una función anónima.
//    La función anónima captura la variable i de intSeq.
//    Cada vez que se llama a la función retornada, i se incrementa y se devuelve su valor.

package main

import "fmt"

func intSeq() func() int {
	i := 0
	return func() int {
		i++
		return i
	}
}

func makeAdder(x int) func(int) int {
	return func(y int) int {
		return x + y
	}
}
func main() {
	nextInt := intSeq()

	fmt.Println(nextInt()) // Imprime 1
	fmt.Println(nextInt()) // Imprime 2
	fmt.Println(nextInt()) // Imprime 3

	// En este ejemplo, makeAdder retorna una función que suma un número fijo (en este caso, 5) a cualquier número que se le pase.

	add5 := makeAdder(5)
	fmt.Println(add5(3)) // Imprime 8
}
