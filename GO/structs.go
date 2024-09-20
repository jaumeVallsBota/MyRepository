package main

import "fmt"
//Definición de struct
type person struct {
    name string
    age  int
}
//constructor
func newPerson(name string) *person {

    p := person{name: name}
    p.age = 42
    return &p
}

func main() {
	//diferente creación de personas, 
	//Bob es poniendo en el mismo orden que se ha definido, 
	//Alice diciendo los campos, 
	//Fred sin edad, por la cuál es 0
	//Ann con un Puntero
	//Jon llamando directamente el constructor

	
    fmt.Println(person{"Bob", 20})

    fmt.Println(person{name: "Alice", age: 30})

    fmt.Println(person{name: "Fred"})

    fmt.Println(&person{name: "Ann", age: 40})

    fmt.Println(newPerson("Jon"))

	// asigno a la variable s una persona, accedo al nombre a través de s.name
    s := person{name: "Sean", age: 50}
    fmt.Println(s.name)

	//Puntero, apuntando a s y saco la edad
    sp := &s
    fmt.Println(sp.age)

	//añado la edad con el puntero
    sp.age = 51
    fmt.Println(sp.age)

	//creo una variable dog donde creo un struct con nombre y behavior, añadiendo el valor en el momento
    dog := struct {
        name   string
        isGood bool
    }{
        "Rex",
        true,
    }
    fmt.Println(dog)
}