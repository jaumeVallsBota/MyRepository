from abc import ABC, abstractmethod

# Interface de Observer
class Observador(ABC):
    @abstractmethod
    def actualizar(self, mensaje: str):
        pass

# Clase concreta de Observador
class Suscriptor(Observador):
    def __init__(self, nombre: str):
        self.nombre = nombre

    def actualizar(self, mensaje: str):
        print(f"{self.nombre} ha recibido la noticia: {mensaje}")

# Interface de Sujeto
class Sujeto(ABC):
    @abstractmethod
    def agregar_observador(self, observador: Observador):
        pass

    @abstractmethod
    def eliminar_observador(self, observador: Observador):
        pass

    @abstractmethod
    def notificar_observadores(self):
        pass

# Clase concreta de Sujeto
class CanalDeNoticias(Sujeto):
    def __init__(self):
        self.observadores = []
        self.ultima_noticia = ""

    def agregar_observador(self, observador: Observador):
        self.observadores.append(observador)

    def eliminar_observador(self, observador: Observador):
        self.observadores.remove(observador)

    def notificar_observadores(self):
        for observador in self.observadores:
            observador.actualizar(self.ultima_noticia)

    def publicar_noticia(self, noticia: str):
        self.ultima_noticia = noticia
        print(f"Publicando noticia: {noticia}")
        self.notificar_observadores()

# Uso
if __name__ == "__main__":
    canal = CanalDeNoticias()

    suscriptor1 = Suscriptor("Juan")
    suscriptor2 = Suscriptor("Maria")
    suscriptor3 = Suscriptor("Luis")

    canal.agregar_observador(suscriptor1)
    canal.agregar_observador(suscriptor2)

    canal.publicar_noticia("Nuevo descubrimiento en Marte!") 
    # Juan y Maria recibirán la noticia

    canal.eliminar_observador(suscriptor1)

    canal.publicar_noticia("Actualización: El descubrimiento era un error.") 
    # Solo Maria recibirá la noticia
