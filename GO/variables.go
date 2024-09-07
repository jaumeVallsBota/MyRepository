package main

import ("fmt"
 "reflect")

func main() {

    var a = "initial"
    fmt.Printf("The type of the variable is %T\n", a)

    variable := 42
	fmt.Println("El tipo de la variable es:", reflect.TypeOf(variable))

    const b = 20
    fmt.Println(b)

}