# -*- coding: utf-8 -*-
"""
Created on Wed Jan 17 08:59:03 2024

@author: jeva
"""
import tkinter as tk
from tkinter import ttk

root = tk.Tk()

root.title("Metric Convertor")

main = ttk.Frame(root)
main.grid()

feet_value = tk.StringVar()
meters_value = tk.StringVar()

feet_label = ttk.Label(main, text = "feet")
feet_label.grid(column=0,row=1)

feet_display = ttk.Label(main, textvariable=feet_value)
feet_display.grid(column=1,row=1)

meters_label = ttk.Label(main, text="meters")
meters_label.grid(column=0,row=0)

meters_input = ttk.Entry(main, textvariable=meters_value)
meters_input.grid(column=1,row=0)
meters_input.focus()

def convert():
    try:
        value = float(meters_value.get())
        feet_value.set("%.3f" % (value*3.28084))
    except ValueError:
        pass
    
    
convert_button = ttk.Button(main, text="Convert", command = convert)
convert_button.grid(column=0, row=2, columnspan=2)

root.mainloop()