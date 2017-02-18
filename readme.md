# ContosoThings Simulator
This repo represents a website which can be configured to run as a simulator.  It has an admin interface where you can add and control device along with a webapi where you can control devices remotely.  Currently the supported devices are binary switch, light and partial thermostat. A light can be on/off or on/off/dim or on/off/dim/color.

# Setup
Deploy the website to a domain of your choice.  Create an initial hub and add some devices using the admin interface.  Install the contosothings translators and using the cli/reference app begin to control the devices.

# Limitations
Subscriptions/notifications are not supported yet
Not all Thermostat properties are editable in the ui
More devices are in progress