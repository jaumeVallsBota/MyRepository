import os
import openai


openai.api_key = "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"

# account for deprecation of LLM model
import datetime
# Get the current date
current_date = datetime.datetime.now().date()

# Define the date after which the model should be set to "gpt-3.5-turbo"
target_date = datetime.date(2024, 6, 12)

# Set the model variable based on the current date
if current_date > target_date:
    llm_model = "gpt-3.5-turbo"
else:
    llm_model = "gpt-3.5-turbo-0301"

from langchain.chains import RetrievalQA
from langchain.chat_models import ChatOpenAI
from langchain.document_loaders import PyPDFLoader
from langchain.vectorstores import DocArrayInMemorySearch
from IPython.display import display, Markdown
from sqlalchemy import create_engine, Column, Integer, String
from sqlalchemy.engine.url import URL


file = 'C:\\Users\\jeva\\Downloads\\GuiaEgipto.pdf'
loader = PyPDFLoader(file_path=file)

from langchain.indexes import VectorstoreIndexCreator

index = VectorstoreIndexCreator(
    vectorstore_cls=DocArrayInMemorySearch
).from_loaders([loader])

query ="Muestra que se dice sobre el templo de Luxor en esta guía"

response = index.query(query)

display(Markdown(response))