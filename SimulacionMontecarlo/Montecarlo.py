import numpy as np

def montecarlo_simulation(f, a, b, num_samples=10000):
    """
    Realiza una simulación de Montecarlo para estimar la esperanza matemática de f(x)
    en el intervalo [a, b].

    :param f: Función a evaluar.
    :param a: Límite inferior del intervalo.
    :param b: Límite superior del intervalo.
    :param num_samples: Número de muestras aleatorias a generar.
    :return: Estimación del valor esperado de f(x).
    """
    samples = np.random.uniform(a, b, num_samples)  # Genera muestras uniformes en [a, b]
    print(samples)
    function_values = f(samples)  # Evalúa la función en los puntos aleatorios
    integral_estimate = (b - a) * np.mean(function_values)  # Calcula la estimación
    return integral_estimate

# Ejemplo de uso con f(x) = x^2 en [0, 1]
#if __name__ == "__main__":
#    f = lambda x: x**2  # Función de prueba
#    estimated_value = montecarlo_simulation(f, 0, 5, 10000)
#    print(f"Estimación de la integral de f(x) = x^2 en [0,1]: {estimated_value:.5f}")


#simula si el jugador llega a su objetivo apostando de 1 a 1 con una probabilidad del 50% de ganar. 
def gambler_ruin(initial_money=10, goal=50, bet_amount=1, win_prob=0.5, num_simulations=10000):
    """
    Simula la ruina del jugador: gana si alcanza el objetivo, pierde si se queda sin dinero.
    """
    wins = 0
    for _ in range(num_simulations):
        money = initial_money
        while 0 < money < goal:
            money += bet_amount if np.random.rand() < win_prob else -bet_amount
        if money == goal:
            wins += 1
    return wins / num_simulations  # Probabilidad de éxito

#probabilidad_ganar = gambler_ruin(10, 100, 1, 0.49, 10000)
#print(f"Probabilidad de alcanzar el objetivo: {probabilidad_ganar:.4f}")


def weather_simulation(days=365, rain_prob=0.3):
    """
    Simula días de lluvia en un año basado en una probabilidad diaria.
    """
    rain_days = np.random.rand(days) < rain_prob  # Días en los que llueve
    return np.sum(rain_days)  # Número total de días con lluvia

dias_lluviosos = weather_simulation(365, 0.3)
print(f"Días lluviosos en un año: {dias_lluviosos}")
