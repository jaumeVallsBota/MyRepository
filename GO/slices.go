package main

import (
	"fmt"
	"slices"
)

func main() {
	//Inicializar un slice. Si no le añades nada, como siempre se inicializa con valores por defecto, en este caso al ser string, nil
	var s []string
	fmt.Println("uninit:", s, s == nil, len(s) == 0)

	// Inicializar un slice con una longitud mayor a 0. Para eso se utiliza el comando Make.
	s = make([]string, 3)
	fmt.Println("emp:", s, "len:", len(s), "cap:", cap(s))

	//Para dar valor a cada elemento de la slice se hace así
	s[0] = "a"
	s[1] = "b"
	s[2] = "c"
	//miramos la slice
	fmt.Println("set:", s)
	//obtenemos el valor en la posición 3
	fmt.Println("get:", s[2])

	//para saber la lingitud de la slice
	fmt.Println("len:", len(s))

	//añadir más elementos en una slice
	s = append(s, "d")
	s = append(s, "e", "f")
	fmt.Println("apd:", s)

	//copiar una slice
	c := make([]string, len(s))
	copy(c, s)
	fmt.Println("cpy:", c)
	//DIFERENTES FORMAS DE COGER PARTES DE UNA SLICE
	l := s[2:5]
	fmt.Println("sl1:", l)

	l = s[:5]
	fmt.Println("sl2:", l)

	l = s[2:]
	fmt.Println("sl3:", l)

	//método para comparar si dos slices son iguales
	t := []string{"g", "h", "i"}
	fmt.Println("dcl:", t)

	t2 := []string{"g", "h", "i"}
	if slices.Equal(t, t2) {
		fmt.Println("t == t2")
	}
	//cREAR un slice de dos dimensiones
	twoD := make([][]int, 3)
	for i := 0; i < 3; i++ {
		innerLen := i + 1
		twoD[i] = make([]int, innerLen)
		for j := 0; j < innerLen; j++ {
			twoD[i][j] = i + j
		}
	}
	fmt.Println("2d: ", twoD)
}
