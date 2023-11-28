import openai
openai.api_key = "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"

def get_completion(prompt, model="gpt-3.5-turbo"):
    messages = [{"role": "user", "content": prompt}]
    response = openai.ChatCompletion.create(
        model=model,
        messages=messages,
        temperature=0, # this is the degree of randomness of the model's output
    )
    return response.choices[0].message["content"]

prompt = f""" Genera una lista de las 10 sagas de fantasía más importantes
            La lista tiene que estar en formato JSON con las siguientes keys:
            Nombre de la saga, author, genero, cuántos libros tiene, y si está completada o no


"""

response = get_completion(prompt)
print(response)