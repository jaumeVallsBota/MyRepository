# -*- coding: utf-8 -*-
"""
Created on Wed Jan 17 09:40:02 2024

@author: jeva
"""
import pandas
from sklearn.linear_model import LinearRegression
import matplotlib.pyplot as pyplot

data = pandas.read_excel("C:\\Users\\jeva\\Downloads\\Source FIles\\Source FIles - Excel Automation with Python Data Modeling\\income_to_credit_score.xlsx")

X = data["Income"]
Y = data["Credit Score"]

#Model need to have data in 2D, this command reshapes X, converting in a list of lists with each value [x, ]
reshaped_X = X.values.reshape(-1,1)
#starts the Model
model = LinearRegression()

#Trains the model with the data 
model.fit(reshaped_X,Y)

results = model.predict(reshaped_X)


pyplot.rcParams["figure.figsize"]=(20,10)
pyplot.rcParams.update({"font.size":20})

pyplot.xlabel("Income")
pyplot.ylabel("Credit Score")
pyplot.scatter(X,Y)
pyplot.plot(X,results)