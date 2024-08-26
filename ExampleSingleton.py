class Singleton:
    _instance = None

    def __new__(cls, *args, **kwargs):
        if cls._instance is None:
            cls._instance = super(Singleton, cls).__new__(cls, *args, **kwargs)
        return cls._instance

# Ejemplo de uso de la clase singleton en Python
if __name__ == "__main__":
    s1 = Singleton()
    s2 = Singleton()

    print(s1 == s2)  # True: ambas variables apuntan a la misma instancia
    print(id(s1))    # Muestra la misma dirección de memoria
    print(id(s2))    # Muestra la misma dirección de memoria
