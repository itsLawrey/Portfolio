import unittest
from bead import SmartLight, Thermostat, SecurityCamera, AutomationSystem

class TestSmartLight(unittest.TestCase):
    def test_brightness_setter(self):
        light = SmartLight("Living Room", True, 78)
        light.brightness = 50
        self.assertEqual(light.brightness, 50)

    def test_brightness_setter_out_of_range(self):
        light = SmartLight("Living Room", True, 78)
        with self.assertRaises(Exception):
            light.brightness = 110

    def test_status_setter(self):
        light = SmartLight("Living Room", True, 78)
        light.status = False
        self.assertFalse(light.status)

    def test_gradual_dim(self):
        light = SmartLight("Living Room", True, 78)
        initial_brightness = light.brightness
        light.gradualDim()
        self.assertTrue(light.brightness < initial_brightness)

    def test_gradual_brighten(self):
        light = SmartLight("Living Room", True, 78)
        initial_brightness = light.brightness
        light.gradualBrighten()
        self.assertTrue(light.brightness > initial_brightness)

class TestThermostat(unittest.TestCase):
    def test_temp_setter(self):
        thermostat = Thermostat("Kitchen", True, 23)
        thermostat.temp = 25
        self.assertEqual(thermostat.temp, 25)

    def test_status_setter(self):
        thermostat = Thermostat("Kitchen", True, 23)
        thermostat.status = False
        self.assertFalse(thermostat.status)
        
    def test_simulate_change(self):
        thermostat = Thermostat("Kitchen", True, 23)
        initial_temp = thermostat.temp
        thermostat.simulateChange()
        self.assertNotEqual(thermostat.temp, initial_temp)

class TestSecurityCamera(unittest.TestCase):
    def test_recording_setter(self):
        camera = SecurityCamera("Front door", False, True)
        camera.recording = True
        self.assertTrue(camera.recording)

    def test_status_setter(self):
        camera = SecurityCamera("Front door", False, True)
        camera.status = True
        self.assertTrue(camera.status)

    def test_simulate_movement(self):
        camera = SecurityCamera("Front door", True, True)
        initial_recording = camera.recording
        camera.simulateMovement()
        if initial_recording != camera.recording:
            self.assertNotEqual(camera.recording, initial_recording)
        else:
            self.assertEqual(camera.recording, initial_recording)

class TestAutomationSystem(unittest.TestCase):
    def test_add_device(self):
        automation_system = AutomationSystem()
        light = SmartLight("Living Room", True, 78)
        automation_system.AddDevice(light)
        self.assertIn(light, automation_system.dl)

    def test_remove_device(self):
        automation_system = AutomationSystem()
        light = SmartLight("Living Room", True, 78)
        automation_system.AddDevice(light)
        automation_system.RemoveDevice(light)
        self.assertNotIn(light, automation_system.dl)

if __name__ == "__main__":
    unittest.main()