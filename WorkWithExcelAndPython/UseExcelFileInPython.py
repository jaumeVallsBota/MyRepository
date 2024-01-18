# -*- coding: utf-8 -*-
"""
Created on Wed Jan 17 12:42:23 2024

@author: jeva
"""
import pandas
import numpy
from sklearn.linear_model import LinearRegression
import matplotlib.pyplot as pyplot

data = pandas.read_excel("C:\\Users\jeva\Downloads\Source Files - Use Excel File in Python\\bank.xlsx")

data_dictionary = data.to_dict('records')

#convert data from CSV to excel
dataCSV = pandas.read_csv("C:\\Users\jeva\Downloads\Source Files2\Source Files - Excel Sheet Manipulation with Python\iris.data",
                          names=["sepalLength","sepalWidth","petalLength","petalWidth","class"])

#dataCSV.to_excel("C:\\Users\jeva\Downloads\iris.xlsx", index= None)

data_multipleSheets = pandas.read_excel("C:\\Users\jeva\Downloads\Source Files2\Source Files - Excel Sheet Manipulation with Python\\multiple_sheets.xlsx", sheet_name="Sheet2")

data_multipleSheets.iloc[:,[0]] #we get all the rows from the column 0, also can be done for the rows


data_btc_heist_data = pandas.read_excel("C:\\Users\jeva\Downloads\Source Files3\Source Files - Build Excel Filters\BitcoinHeistData_Small.xlsx") 

data_btc_heist_data[data_btc_heist_data['label'].isin(["princetonLocky"]) & data_btc_heist_data["count"].isin([4])]
data_btc_heist_data[data_btc_heist_data["year"] == 2017]
data_btc_heist_data[data_btc_heist_data["length"] > 100]
data_btc_heist_data[data_btc_heist_data["label"].map(lambda x: x.endswith("er"))]

pandas.value_counts(data_btc_heist_data["year"]) #provides the unique values that are in the column year of the data_btc_heist_data dataset
data_btc_heist_data.sum() #it gives the sum of each column 
data_btc_heist_data.sum(axis= 0) #axis= 0, you sum the rows
#data_btc_heist_data.sum(axis= 1) #axis=1, you sum the columns

pandas.pivot_table(data_btc_heist_data, values="income", index = "count", columns="label", aggfunc=numpy.sum, fill_value=0)