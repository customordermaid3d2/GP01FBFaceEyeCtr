﻿using BepInEx.Configuration;
using BepInEx.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GP01FBFaceEyeCtr
{
    class SampleConfig
    {
		//static saving of the main instance. This makes it easier to run stuff like coroutines from static methods or accessing non-static vars.
		//public static SampleConfig @this;

		//Static var for the logger so you can log from other classes.
		public static ManualLogSource logger;

		internal static COM3D2.GUIAPI.ConfigMenu ConfigMenu;

		/// <summary>
		/// private void Awake()
		/// </summary>
		/// <param name="logger"></param>
		public static void Install(ManualLogSource logger)
		{
			//Useful for engaging coroutines or accessing variables non-static variables. Completely optional though.
			//@this = this;

			//pushes the logger to a public static var so you can use the bepinex logger from other classes.
			SampleConfig.logger = logger;

			//Binds the configuration. In other words it sets your ConfigEntry var to your config setup.
			//ExampleConfig = Config.Bind("Section", "Name", false, "Description");
			ConfigMenu = COM3D2.GUIAPI.MenuHandler.CreateConfigMenu("Test");

			var sect = ConfigMenu.AddSection("My Section");

			var switch1 = sect.AddSwitchControl("Switch1");
			sect.AddSwitchControl("Switch2", false);
			sect.AddSwitchControl("Switch3");
			sect.AddSwitchControl("Switch4");
			sect.AddSwitchControl("Switch5");
			sect.AddSwitchControl("Switch6");
			sect.AddSwitchControl("Switch7");
			sect.AddSwitchControl("Switch3");
			sect.AddSwitchControl("Switch4");
			sect.AddSwitchControl("Switch5");
			sect.AddSwitchControl("Switch6");
			sect.AddSwitchControl("Switch7");
			sect.AddSwitchControl("Switch3");
			sect.AddSwitchControl("Switch4");
			sect.AddSwitchControl("Switch5");
			sect.AddSwitchControl("Switch6");
			sect.AddSwitchControl("Switch7");
			sect.AddSwitchControl("Switch3");
			sect.AddSwitchControl("Switch4");
			sect.AddSwitchControl("Switch5");
			sect.AddSwitchControl("Switch6");
			sect.AddSwitchControl("Switch7");
			sect.AddSwitchControl("Switch8");
			var slider = sect.AddSliderControl("Slider 1");
			var dropdown = sect.AddDropDownControl("Dropdown", new List<string>() { "test", "test2", "mustard", "snarf test" }, "test2");

			var sect2 = ConfigMenu.AddSection("My Section 2");
			sect2.AddSliderControl("Slider 5");
			sect2.AddSliderControl("Slider 6");
			sect2.AddSliderControl("Slider 7");
			sect2.AddDropDownControl("Dropdown2 5", new List<string> { "test5", "test9" });

			var sect3 = ConfigMenu.AddSection("My Section 3");
			sect3.AddSwitchControl("Switch 1");

			switch1.ValueChanged += (s, e) =>
			{
				logger.LogInfo($"Switch1's value changed to {switch1.Value}");
			};

			slider.ValueChanged += (s, e) =>
			{
				logger.LogInfo($"Slider's value changed to {slider.Value}");
			};

			dropdown.ValueChanged += (s, e) =>
			{
				logger.LogInfo($"DropDown's value changed to {dropdown.Value}");
			};

			var field = sect2.AddInputFieldControl("Field 1", "InputHere");

			field.ValueChanged += (s, e) =>
			{
				logger.LogInfo($"Field's value changed to {field.Value}");
			};
		}
	}
}
