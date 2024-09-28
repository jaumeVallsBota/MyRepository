//En este ejemplo:

//    intSeq retorna una función anónima.
//    La función anónima captura la variable i de intSeq.
//    Cada vez que se llama a la función retornada, i se incrementa y se devuelve su valor.

package main

import (
	"reflect"
	"testing"
)

func Test_makeAdder(t *testing.T) {
	type args struct {
		x int
	}
	tests := []struct {
		name string
		args args
		want func(int) int
	}{
		// TODO: Add test cases.
	}
	for _, tt := range tests {
		t.Run(tt.name, func(t *testing.T) {
			if got := makeAdder(tt.args.x); !reflect.DeepEqual(got, tt.want) {
				t.Errorf("makeAdder() = %v, want %v", got, tt.want)
			}
		})
	}
}
