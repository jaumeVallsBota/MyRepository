package main

import "fmt"

func main() {

	//definimos una array vacía de integers
    var a [5]int
    fmt.Println("emp:", a)
	//se asigna una valor a la posición 4
    a[4] = 100
    fmt.Println("set:", a)
    fmt.Println("get:", a[4])

	//comprobamos el tamaño de la array
    fmt.Println("len:", len(a))

	//Le decimos el tamaño de la array y los elementos, en este caso los 5 primeros son los mostrados y los dos siguientes son 0
    b := [7]int{1, 2, 3, 4, 5}
    fmt.Println("dcl:", b)

	//El compilador puede contar directamente el número de elementos de la array si ponemos [...], en este caso será 5
    var c = [...]int{1, 2, 3, 4, 5}
    fmt.Println("dcl:", c)

	//si especificas el indice (en este caso 3), los valores que hay en esta situación serán sustituidos por 0
    var d = [...]int{100, 3: 400, 500}
    fmt.Println("idx:", d)
	
	//crear una matriz de dos dimensiones por for
    var twoD [2][3]int
    for i := 0; i < 2; i++ {
        for j := 0; j < 3; j++ {
            twoD[i][j] = i + j
        }
    }
    fmt.Println("2d: ", twoD)
	//definir una matriz de dos dimensiones directamente
    twoD = [2][3]int{
        {1, 2, 3},
        {1, 2, 3},
    }
    fmt.Println("2d: ", twoD)
}