# SeeBee
A ProcMon Logs (PML) Analyzer.

CI (via [AppVeyor](https://www.appveyor.com/))

[![Build status](https://ci.appveyor.com/api/projects/status/3i92qslii7jlq6t3/branch/master?svg=true)](https://ci.appveyor.com/project/asquigglytwist/seebee/branch/master)

## Disclaimer:
Please note that this is a work-in-progress and most likely will always be.

## What is it?
[SeeBee](https://github.com/asquigglytwist/SeeBee) is a (currently command-line only) tool to parse the ProcMon Logs (PML) which is in a binary format.
Although PMLs can be read by ProcMon itself, the filtering and querying capabilities of the tool is very limited.

## How does it work?
[SeeBee](https://github.com/asquigglytwist/SeeBee) uses ProcMon to convert the PML to XML and then enables filtering on top of the XML data.
Currently, the filtering engine supports a limited subset of properties and filtering mechanisms.

## How to use?
[SeeBee](https://github.com/asquigglytwist/SeeBee) is currently a CMD only version.  Eventually, I plan to add a GUI and improve the filtering engine.

Without further ado, here's how:
[SeeBeeCmd](https://github.com/asquigglytwist/SeeBee/tree/master/Src/SeeBeeCmd) pm "C:\T\SeeBee\Procmon.exe" in "C:\T\SeeBee\Logfile.PML" c "C:\T\SeeBee\SeeBee.sbc"
where,
* pm -> path to the ProcMon executable
* in -> the input PML or XML file
* c  -> the configuration file for SeeBee

## Configuring SeeBee
The config file - .SBC (SeeBeeConfig) is a simple XML file and can be edited with any text editor.  Below is [an example](https://github.com/asquigglytwist/SeeBee/tree/master/Samples/SeeBee.SBC):
```xml
<?xml version="1.0" encoding="UTF-8"?>
<SeeBee>
	<Filters>
		<Create>
			<Filter name="DefaultUserProcesses" appliesOn="Processes" property="ProcessName" operator="Equals">
				notepad.exe, explorer.exe, iexplore.exe, firefox.exe, chrome.exe
			</Filter>
			<Filter name="DefaultSystemProcesses" appliesOn="Processes" property="ImagePath" operator="StartsWith">
				gpupdate.exe, conhost.exe, svchost.exe, winlogon.exe, csrss.exe, lsass.exe, WUDFHost.exe, spoolsv.exe, unsecapp.exe
			</Filter>
			<Filter name="RunningProcesses" appliesOn="Processes" property="FinishTime" operator="Equals">
				0
			</Filter>
			<Filter name="CompletedProcesses" appliesOn="Processes" property="FinishTime" operator="NotEquals">
				0
			</Filter>
			<Filter name="HasLoadedWindowsDlls" appliesOn="Processes" property="Modules" operator="Contains">
				user32.dll, advapi32.dll
			</Filter>
			<Filter name="SomeGenericProcesses" appliesOn="Events" property="Operation" operator="Equals">
				"Load Image", QueryBasicInformationFile
			</Filter>
			<Filter name="SomeGenericProcesses" appliesOn="Events" property="Operation" operator="Equals">
				"Load Image", QueryBasicInformationFile
			</Filter>
		</Create>
		<Conditions>
			<Condition action="Exclude" operator="And">
				DefaultUserProcesses, DefaultSystemProcesses
			</Condition>
			<Condition action="Include" operator="Or">
				RunningProcesses, HasLoadedWindowsDlls
			</Condition>
			<Condition action="Include" operator="Only">
				HasLoadedWindowsDlls
			</Condition>
		</Conditions>
	</Filters>
</SeeBee>
```

## Sample Output
<div class="consoleWindow">
# of Processes read from file:  9.<br />
# of Events that match the criteria:  108.<br />
Press any key to continue...
</div>