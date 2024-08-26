from abc import ABC, abstractmethod

# Producto
class Vehiculo(ABC):
    @abstractmethod
    def crear(self):
        pass

# Productos concretos
class Auto(Vehiculo):
    def crear(self):
        return "Se ha creado un Auto"

class Bicicleta(Vehiculo):
    def crear(self):
        return "Se ha creado una Bicicleta"

# Creador
class CreadorVehiculo(ABC):
    @abstractmethod
    def crear_vehiculo(self):
        pass

    def some_operation(self):
        vehiculo = self.crear_vehiculo()
        result = vehiculo.crear()
        return result

# Creadores concretos
class CreadorAuto(CreadorVehiculo):
    def crear_vehiculo(self):
        return Auto()

class CreadorBicicleta(CreadorVehiculo):
    def crear_vehiculo(self):
        return Bicicleta()

# Uso
if __name__ == "__main__":
    creador1 = CreadorAuto()
    print(creador1.some_operation())  # Salida: Se ha creado un Auto

    creador2 = CreadorBicicleta()
    print(creador2.some_operation())  # Salida: Se ha creado una Bicicleta
