from abc import ABC, abstractmethod

# Estrategia
class EstrategiaPago(ABC):
    @abstractmethod
    def pagar(self, cantidad: float):
        pass

# Estrategias concretas
class PagoTarjetaCredito(EstrategiaPago):
    def pagar(self, cantidad: float):
        print(f"Pagando {cantidad} usando Tarjeta de Crédito")

class PagoPayPal(EstrategiaPago):
    def pagar(self, cantidad: float):
        print(f"Pagando {cantidad} usando PayPal")

class PagoCripto(EstrategiaPago):
    def pagar(self, cantidad: float):
        print(f"Pagando {cantidad} usando Criptomonedas")

# Contexto
class ProcesadorDePagos:
    def __init__(self, estrategia: EstrategiaPago):
        self._estrategia = estrategia

    def set_estrategia(self, estrategia: EstrategiaPago):
        self._estrategia = estrategia

    def procesar_pago(self, cantidad: float):
        self._estrategia.pagar(cantidad)

# Uso
if __name__ == "__main__":
    # Inicialmente usando Pago con Tarjeta de Crédito
    procesador = ProcesadorDePagos(PagoTarjetaCredito())
    procesador.procesar_pago(100.0)  # Salida: Pagando 100.0 usando Tarjeta de Crédito

    # Cambiando a Pago con PayPal
    procesador.set_estrategia(PagoPayPal())
    procesador.procesar_pago(200.0)  # Salida: Pagando 200.0 usando PayPal

    # Cambiando a Pago con Criptomonedas
    procesador.set_estrategia(PagoCripto())
    procesador.procesar_pago(300.0)  # Salida: Pagando 300.0 usando Criptomonedas
