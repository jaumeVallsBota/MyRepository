import pandas as pd
import json
listofclubs = pd.read_csv("C:\\Users\\jeva\\Downloads\\Test.csv", delimiter=";")
jsonformat = listofclubs.to_json(orient="records")

listofJson = json.loads(jsonformat)
print(listofJson[1]["City"])
