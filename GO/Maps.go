package main

import (
	"fmt"
	"maps"
)

func main() {
	//Creamos un map de strings
	m := make(map[string]int)
	//Añadir valores
	m["k1"] = 7
	m["k2"] = 13

	fmt.Println("map:", m)
	//obtener valores
	v1 := m["k1"]
	fmt.Println("v1:", v1)
	//si llamamos a un valor que no existe, nos devuelve el default para ese tipo
	v3 := m["k3"]
	fmt.Println("v3:", v3)
	//len del map
	fmt.Println("len:", len(m))
	//borrar una entrada
	delete(m, "k2")
	fmt.Println("map:", m)
	//borrar todo el map
	clear(m)
	fmt.Println("map:", m)

	_, prs := m["k2"]
	fmt.Println("prs:", prs)
	//Inicializar y llenar el map directamente
	n := map[string]int{"foo": 1, "bar": 2}
	fmt.Println("map:", n)
	//Comparar si un Map es igual a otro con la función Equal
	n2 := map[string]int{"foo": 1, "bar": 2}
	if maps.Equal(n, n2) {
		fmt.Println("n == n2")
	}
}
