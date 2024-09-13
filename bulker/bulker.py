import tkinter as tk
import json
import datetime
import pandas as pd
import matplotlib.pyplot as plt

#ablak
rootCalculator = tk.Tk()
rootCalculator.title("Protein calculator")
rootCalculator.geometry('1000x400')
rootCalculator.resizable(width=False, height=False)

#global
bodyweight = 0.0

#szovegek
lblWeight = tk.Label(rootCalculator, text = "Body weight (kg)")
lblWeight.grid(column=0,row=0)

lblMeals = tk.Label(rootCalculator, text = "Meals per day")
lblMeals.grid(column=0,row=1)

#inputok
txtWeight = tk.Entry(rootCalculator, width=25)
txtWeight.grid(column=1,row=0)
txtMeals = tk.Entry(rootCalculator, width=25)
txtMeals.grid(column=1,row=1)

#gomb
def clickedCalc():
    global bodyweight
    bodyweight = float(txtWeight.get())
    dailyprotein = bodyweight * 2
    meals = float(txtMeals.get())
    lblRes = tk.Label(rootCalculator, text = f"Results:\nRecommended protein intake: {dailyprotein}g per day\nProtein per meal: {dailyprotein / meals}g")
    lblRes.grid(column=2,row=2)
    return bodyweight

readyButton = tk.Button(rootCalculator, text = "Calculate", command=clickedCalc)
readyButton.grid(column=0,row=2)

#gomb2
def clickedLoad():
    with open("bulk_log.json", "r") as file:
        data = json.load(file)
        if data != None:
            df = pd.DataFrame(data["logs"])
            df['date'] = pd.to_datetime(df['date']) 

            df.plot(x='date', y='bodyweight', kind='line', marker='o', title='Bodyweight Over Time')
            
            plt.xlabel('Date')
            plt.ylabel('Bodyweight (kg)')
            plt.show()
    return

openButton = tk.Button(rootCalculator, text = "Load", command=clickedLoad)
openButton.grid(column=0,row=4)
#gomb3
def clickedSave():
    data = {
        "bodyweight" : bodyweight,
        "date" : datetime.datetime.now().strftime("%Y/%m/%d, %H:%M:%S")
    }
    if bodyweight != 0: 
        write_json(data,"bulk_log.json") 
        lblSaved = tk.Label(rootCalculator, text = "Data has been successfully saved.",fg='green')
        lblSaved.grid(column=1,row=5)
    else:
        lblSaved = tk.Label(rootCalculator, text = "Invalid data.",fg='red')
        lblSaved.grid(column=1,row=5)
    return

saveButton = tk.Button(rootCalculator, text = "Save", command=clickedSave)
saveButton.grid(column=0,row=5)

#mentes help
# function to add to JSON
def write_json(new_data, filename='bulk_log.json'):
    with open(filename,'r+') as file:
        file_data = json.load(file)
        file_data['logs'].append(new_data)
        file.seek(0)
        json.dump(file_data, file, indent = 4)

















rootCalculator.mainloop()
