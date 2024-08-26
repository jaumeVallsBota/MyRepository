# Producto
class Hamburguesa:
    def __init__(self):
        self.pan = None
        self.carne = None
        self.queso = None
        self.lechuga = None
        self.tomate = None
        self.salsa = None

    def __str__(self):
        ingredientes = []
        if self.pan: ingredientes.append(f"Pan: {self.pan}")
        if self.carne: ingredientes.append(f"Carne: {self.carne}")
        if self.queso: ingredientes.append(f"Queso: {self.queso}")
        if self.lechuga: ingredientes.append(f"Lechuga: {self.lechuga}")
        if self.tomate: ingredientes.append(f"Tomate: {self.tomate}")
        if self.salsa: ingredientes.append(f"Salsa: {self.salsa}")
        return "Hamburguesa con " + ", ".join(ingredientes)

# Builder
class HamburguesaBuilder:
    def __init__(self):
        self.hamburguesa = Hamburguesa()

    def agregar_pan(self, tipo: str):
        self.hamburguesa.pan = tipo
        return self

    def agregar_carne(self, tipo: str):
        self.hamburguesa.carne = tipo
        return self

    def agregar_queso(self, tipo: str):
        self.hamburguesa.queso = tipo
        return self

    def agregar_lechuga(self):
        self.hamburguesa.lechuga = "Lechuga"
        return self

    def agregar_tomate(self):
        self.hamburguesa.tomate = "Tomate"
        return self

    def agregar_salsa(self, tipo: str):
        self.hamburguesa.salsa = tipo
        return self

    def build(self):
        return self.hamburguesa

# Director
class Director:
    def __init__(self, builder: HamburguesaBuilder):
        self._builder = builder

    def construir_hamburguesa_clasica(self):
        return (self._builder
                .agregar_pan("Blanco")
                .agregar_carne("Res")
                .agregar_queso("Cheddar")
                .agregar_lechuga()
                .agregar_tomate()
                .agregar_salsa("Ketchup")
                .build())

    def construir_hamburguesa_vegetariana(self):
        return (self._builder
                .agregar_pan("Integral")
                .agregar_carne("Vegetal")
                .agregar_queso("Cheddar")
                .agregar_lechuga()
                .agregar_tomate()
                .agregar_salsa("Mostaza")
                .build())

# Uso
if __name__ == "__main__":
    builder = HamburguesaBuilder()
    director = Director(builder)

    # Construir una hamburguesa clásica
    hamburguesa_clasica = director.construir_hamburguesa_clasica()
    print(hamburguesa_clasica)  # Salida: Hamburguesa con Pan: Blanco, Carne: Res, Queso: Cheddar, Lechuga, Tomate, Salsa: Ketchup

    # Construir una hamburguesa vegetariana
    hamburguesa_vegetariana = director.construir_hamburguesa_vegetariana()
    print(hamburguesa_vegetariana)  # Salida: Hamburguesa con Pan: Integral, Carne: Vegetal, Queso: Cheddar, Lechuga, Tomate, Salsa: Mostaza
