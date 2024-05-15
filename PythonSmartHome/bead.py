import random
import time
import tkinter as tk


class Device:
    def __init__(self,ID,status):
        self._ID = ID
        self._status = status
        self._available = True # network tagja e vagy sem
    
    #getters, setters
    @property
    def ID(self):
        return self._ID
    # @property
    # def status(self):
    #     return self._status
    @property
    def available(self):
        return self._available
    @ID.setter
    def ID(self, value):
        print(f"{self.__class__.__name__} {self.ID} ID changed to {value}")
        self._ID = value
    @available.setter
    def available(self,value):
        if isinstance(value,bool):
            self._available = value
    
   
    def __str__(self):
        if self._status:
            return f"{str(self.ID)} is ONLINE"
        else:
            return f"{str(self.ID)} is OFFLINE"

class SmartLight(Device):
    def __init__(self, ID, status, brightness):
        super().__init__(ID, status)
        self._brightness = brightness if self._status else 0
    
    @property #getter
    def brightness(self):
        return self._brightness
    @property
    def status(self):
        return self._status
    
    @brightness.setter 
    def brightness(self,value):
        
        if(isinstance(value,int)):
            if value > 100 or value < 0:
                raise Exception("The lights\' functionality is between 0 and 100%")
            else:
                print(f"{self.__class__.__name__} {self.ID} set to {value}% brightness")
                self._brightness = value
        else:
            raise Exception("Brightness must be a number between 0 and 100")
    
    @status.setter
    def status(self, value):
        if isinstance(value, bool):
            if value == True:
                print(f"{self.__class__.__name__} {self.ID} has been turned on")
            else:
                print(f"{self.__class__.__name__} {self.ID} has been turned off")
                self._brightness = 0  # Set brightness to 0 when turning off
            self._status = value
        else:
            raise Exception("Status can only be True or False")
    
    def __str__(self):
        return super().__str__() + f", device type: {self.__class__.__name__}, brightness: {str(self._brightness)}%"
    
    def gradualDim(self):
        if self._status:
            if self._brightness == 0:
                print(f"{self.__class__.__name__} {self.ID} already at 0%")
            else:
                print(f"{self.__class__.__name__} {self.ID} has been dimmed")
                self.brightness = random.randint(0,self.brightness) # random gyorsasaggal elalszik eddig, marmint ez csak egy lepes
        else:
            print(f"{self.__class__.__name__} {self.ID} cannot be dimmed as it is OFFLINE")
    def gradualBrighten(self):
        if self._status:
            if self._brightness == 100:
                print(f"{self.__class__.__name__} {self.ID} already at 100%")
            else:
                print(f"{self.__class__.__name__} {self.ID} has been brightened")
                self.brightness = random.randint(self.brightness,100) 
        else:
            print(f"{self.__class__.__name__} {self.ID} cannot be brightened as it is OFFLINE")
    
class Thermostat(Device):
    def __init__(self, ID, status, temp):
        super().__init__(ID, status)
        if self._status == False:
            self._temp = 0
        else:
            self._temp = temp
            
    @property
    def temp(self):
        return self._temp
    @property
    def status(self):
        return self._status
    
    @temp.setter #temperature range simulation???
    def temp(self,value):
        if isinstance(value,int):
            print(f"{self.__class__.__name__} {self.ID} set to {value}°C temperature")
            self._temp = value
        else:
            raise Exception("Temperature must be a number")
    
    @status.setter
    def status(self, value):
        if isinstance(value,bool):
            if value == True:
                print(f"{self.__class__.__name__} {self.ID} has been turned on")
            else:
                print(f"{self.__class__.__name__} {self.ID} has been turned off")
            self._status = value
        else:
            raise Exception("Status can only be True or False")
    
    def __str__(self):
        if self._status:
            return super().__str__() + f", device type: {self.__class__.__name__}, temperature: {str(self.temp)}°C"
        else:
            return super().__str__() + f", device type: {self.__class__.__name__}, temperature: -"

    def simulateChange(self):
        if self._status:
            self.temp = int(self.temp + random.uniform(-1,2)) # -1 0 1 kozott random number
        else:
            print(f"{self.__class__.__name__} {self.ID} is turned off, cannot display temperature change")
         
class SecurityCamera(Device):
    def __init__(self, ID, status, recording):
        super().__init__(ID, status)
        if self._status == False:#kikapcsolva
            self._recording = False
        elif self._status and recording == False:#bekapcsolva de nem videoz
            self._recording = False
        else:
            self._recording = True
    
    @property
    def recording(self):
        return self._recording
    @property
    def status(self):
        return self._status
    
    @recording.setter
    def recording(self,value):
        if isinstance(value,bool):
            if value == True:
                print(f"{self.__class__.__name__} {self.ID} sensed movement and is now armed")
            else:
                print(f"{self.__class__.__name__} {self.ID} is now disarmed")
            self._recording = value
        else:
            raise Exception("Recording status can only be a bool")
        
    @status.setter
    def status(self, value):
        if isinstance(value,bool):
            if value == True:
                print(f"{self.__class__.__name__} {self.ID} has been turned on")
            else:
                print(f"{self.__class__.__name__} {self.ID} has been turned off")
                self._recording = False
            self._status = value
        else:
            raise Exception("Status can only be True or False")
        
    def __str__(self):
        if(self.recording):
            return super().__str__() + f", device type: {self.__class__.__name__}, recording turned ON"
        else:
            return super().__str__() + f", device type: {self.__class__.__name__}, recording turned OFF"
            
    def simulateMovement(self):
        self.recording = random.choice([True,False])

class AutomationSystem:
    def __init__(self):
        self._dl = []

    @property
    def dl(self):
        return self._dl
        
    def __str__(self):
        printedList = []
        for device in self._dl:
            printedList.append(f"{device.__class__.__name__} {device.ID}")
        return f"Devices in the system: "+str(printedList)
    
    def AddDevice(self,obj):
            if obj not in self._dl:
                print(f"{obj.__class__.__name__} {obj.ID} added to list of devices")
                obj.available = False #mar nem elerheto masok szamara
                self._dl.append(obj)
            else:
                print(f"{obj.__class__.__name__} {obj.ID} already in list of devices")
    
    def RemoveDevice(self,obj):
            if obj in self._dl:
                print(f"{obj.__class__.__name__} {obj.ID} removed from list of devices")
                obj.available = True
                self._dl.remove(obj)
            else:
                print(f"{obj.__class__.__name__} {obj.ID} not found in list of devices")
    
    def DiscoverDevice(self,obj):
        if isinstance(obj,(SmartLight,Thermostat,SecurityCamera)):
            if obj.available:
                print(f"{obj.__class__.__name__} {obj.ID} is available and ready to be added")
            else:
                print(f"{obj.__class__.__name__} {obj.ID} is not available")
            return obj.available
        else:
            raise Exception("Invalid device type")
    
    def Automation(self):
        print('')
        print('some time later...')
        print('')
        for device in self._dl:
            if isinstance(device,SmartLight):
                action = random.choice([device.gradualDim, device.gradualBrighten])
                action()
            elif isinstance(device,Thermostat):
                device.simulateChange()
            elif isinstance(device,SecurityCamera):
                device.simulateMovement()


def update_display():
    for widget in label_frame.winfo_children():
        widget.destroy()
    for device in automation_system.dl:
        if device.status:
            tk.Label(label_frame, text=f"{device.__class__.__name__}: {device.ID} is ONLINE").pack(padx=10)
        else:
            tk.Label(label_frame, text=f"{device.__class__.__name__}: {device.ID} is OFFLINE").pack(padx=10)
            
    sliderLight.set(light1.brightness)
    sliderLight2.set(light2.brightness)
    sliderThermo.set(thermostat1.temp)
    
    if camera1.recording: #ALARM
        labelLight.config(text=f"{light1.__class__.__name__}: {light1.ID} brightness: {light1.brightness}%", fg="red")
        labelLight2.config(text=f"{light2.__class__.__name__}: {light2.ID} brightness: {light2.brightness}%", fg="red")

        labelThermo.config(text=f"{thermostat1.__class__.__name__}: {thermostat1.ID} temperature: {thermostat1.temp}°C", fg="red")
        labelSecurityCam.config(text=f"{camera1.__class__.__name__}: {camera1.ID} is armed" if camera1.recording else f"{camera1.__class__.__name__}: {camera1.ID} is disarmed", fg="red")
        
       
        
        
        for device in automation_system.dl:
            if isinstance(device,SmartLight):
                if not device.status:# force turn on
                    device.status = True
                device.brightness = 100
    else:
        labelLight.config(text=f"{light1.__class__.__name__}: {light1.ID} brightness: {light1.brightness}%", fg='black')
        labelLight2.config(text=f"{light2.__class__.__name__}: {light2.ID} brightness: {light2.brightness}%", fg='black')

        labelThermo.config(text=f"{thermostat1.__class__.__name__}: {thermostat1.ID} temperature: {thermostat1.temp}°C", fg='black')
        labelSecurityCam.config(text=f"{camera1.__class__.__name__}: {camera1.ID} is armed" if camera1.recording else f"{camera1.__class__.__name__}: {camera1.ID} is disarmed", fg='black')
    
        
    
    root.after(350, update_display)



def turn_all_on():
    for device in automation_system.dl:
        device.status = True

def turn_all_off():
    for device in automation_system.dl:
        device.status = False



def on_sliderLight_change(value):
    if light1.status:
        light1.brightness = int(value)
    
def on_sliderLight_change2(value):
    if light2.status:
        light2.brightness = int(value)

def on_sliderThermo_change(value):
    if thermostat1.status:
        thermostat1.temp = int(value)


    
def toggle_light():
    light1.status = not light1.status  # Toggle the status (on/off)

def on_dim_light():
    if light1.status and light1.brightness > 0:
        light1.gradualDim()
        sliderLight.set(light1.brightness)

def on_brighten_light():
    if light1.status and light1.brightness < 100:
        light1.gradualBrighten()
        sliderLight.set(light1.brightness)
        
def toggle_light2():
    light2.status = not light2.status  # Toggle the status (on/off)

def on_dim_light2():
    if light2.status and light2.brightness > 0:
        light2.gradualDim()
        sliderLight2.set(light2.brightness)

def on_brighten_light2():
    if light2.status and light2.brightness < 100:
        light2.gradualBrighten()
        sliderLight2.set(light2.brightness)
        
        
        
        
def toggle_thermo():
    thermostat1.status = not thermostat1.status  # Toggle the status (on/off)

def on_thermo_timepassed():
    if thermostat1.status:
        thermostat1.simulateChange()
        sliderThermo.set(thermostat1.temp)
    
def toggle_camera():
    camera1.status = not camera1.status  # Toggle the status (on/off)

def toggle_camera_armed():
    if camera1.status:
        camera1.recording = not camera1.recording
        
def on_movement():
    if camera1.status:
        camera1.simulateMovement()

automation_system = AutomationSystem()
light1 = SmartLight("Living Room",True,78)
light2 = SmartLight("Hall",True,35)
thermostat1 = Thermostat("Kitchen", True,23)
camera1 = SecurityCamera("Front door", False, True)
camera2 = SecurityCamera("Garden",True,False)
camera2.available = False
space = [light1,light2,thermostat1,camera1,camera2]

for device in space:
    if automation_system.DiscoverDevice(device):
        automation_system.AddDevice(device)

root = tk.Tk()
root.geometry("450x850")
root.resizable(False, False)
root.title("Smart Home IoT sim")



label_frame = tk.LabelFrame(root, text="Device status:",font=('Arial',18))
label_frame.pack(padx=10, pady=10)

on_button = tk.Button(root, text="Turn All On", command=turn_all_on)
on_button.pack(padx=10)

off_button = tk.Button(root, text="Turn All Off", command=turn_all_off)
off_button.pack(padx=10)




labelLight = tk.Label(root, text=f"{light1.__class__.__name__}: {light1.ID} brightness: {light1.brightness}%")
labelLight.pack(padx=10)

sliderLight = tk.Scale(root, from_=0, to=100, orient="horizontal", command=on_sliderLight_change)
sliderLight.set(light1.brightness)
sliderLight.pack(padx=10)

button_dim_light = tk.Button(root, text="Dim Light", command=on_dim_light)
button_dim_light.pack(padx=10)

button_brighten_light = tk.Button(root, text="Brighten Light", command=on_brighten_light)
button_brighten_light.pack(padx=10)

button_toggle_light = tk.Button(root, text="Toggle Light", command=toggle_light)
button_toggle_light.pack(padx=10)





labelLight2 = tk.Label(root, text=f"{light2.__class__.__name__}: {light2.ID} brightness: {light2.brightness}%")
labelLight2.pack(padx=10)

sliderLight2 = tk.Scale(root, from_=0, to=100, orient="horizontal", command=on_sliderLight_change2)
sliderLight2.set(light2.brightness)
sliderLight2.pack(padx=10)

button_dim_light2 = tk.Button(root, text="Dim Light", command=on_dim_light2)
button_dim_light2.pack(padx=10)

button_brighten_light2 = tk.Button(root, text="Brighten Light", command=on_brighten_light2)
button_brighten_light2.pack(padx=10)

button_toggle_light2 = tk.Button(root, text="Toggle Light", command=toggle_light2)
button_toggle_light2.pack(padx=10)




labelThermo = tk.Label(root, text=f"{thermostat1.__class__.__name__}: {thermostat1.ID} temperature: {thermostat1.temp}°C")
labelThermo.pack(padx=10,pady=(30,0))

sliderThermo = tk.Scale(root, from_=-20, to=40, orient="horizontal", command=on_sliderThermo_change)
sliderThermo.set(thermostat1.temp)
sliderThermo.pack(padx=10)

button_toggle_thermo = tk.Button(root, text="Toggle Thermostat", command=toggle_thermo)
button_toggle_thermo.pack(padx=10)

button_simulate_thermo = tk.Button(root, text="Simulate Time Passing\nClick many times", command=on_thermo_timepassed)
button_simulate_thermo.pack(padx=10)



labelSecurityCam = tk.Label(root, text=f"{camera1.__class__.__name__}: {camera1.ID} is armed" if camera1.recording else f"{camera1.__class__.__name__}: {camera1.ID} is disarmed")
labelSecurityCam.pack(padx=10,pady=(30,0))

button_cam_recording = tk.Button(root, text="Arm/Disarm Camera", command=toggle_camera_armed)
button_cam_recording.pack(padx=10)

button_toggle_camera = tk.Button(root, text="Toggle Camera", command=toggle_camera)
button_toggle_camera.pack(padx=10)

button_simulate_movement = tk.Button(root, text="Simulate Random Movement\n50% chance to trigger camera", command=on_movement)
button_simulate_movement.pack(padx=10)





labelAuttomationRule = tk.Label(root, text="Automation rule: if camera is armed, all lights will operate at 100% capacity")
labelAuttomationRule.pack(padx=10)



update_display()
root.mainloop()
