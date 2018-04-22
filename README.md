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
The config file - .SBC (SeeBeeConfig) is a simple XML file and can be edited with any text editor.  The format is:
<style type="text/css">
span {
	font-family: 'Courier New';
	font-size: 10pt;
	color: #000000;
}
.sc0 {
	font-weight: bold;
}
.sc1 {
	color: #0000FF;
}
.sc3 {
	color: #FF0000;
}
.sc6 {
	font-weight: bold;
	color: #8000FF;
}
.sc8 {
}
.sc12 {
	color: #FF0000;
	background: #FFFF00;
}
.sc13 {
	color: #FF0000;
	background: #FFFF00;
}
</style>
<div style="float: left; white-space: pre; line-height: 1; background: #FFFFFF; "><span class="sc12">&lt;?</span><span class="sc1">xml</span><span class="sc8"> </span><span class="sc3">version</span><span class="sc8">=</span><span class="sc6">"1.0"</span><span class="sc8"> </span><span class="sc3">encoding</span><span class="sc8">=</span><span class="sc6">"UTF-8"</span><span class="sc13">?&gt;</span><span class="sc0">
</span><span class="sc1">&lt;SeeBee&gt;</span><span class="sc0">
    </span><span class="sc1">&lt;Filters&gt;</span><span class="sc0">
        </span><span class="sc1">&lt;Create&gt;</span><span class="sc0">
            </span><span class="sc1">&lt;Filter</span><span class="sc8"> </span><span class="sc3">name</span><span class="sc8">=</span><span class="sc6">"DefaultUserProcesses"</span><span class="sc8"> </span><span class="sc3">appliesOn</span><span class="sc8">=</span><span class="sc6">"Processes"</span><span class="sc8"> </span><span class="sc3">property</span><span class="sc8">=</span><span class="sc6">"ProcessName"</span><span class="sc8"> </span><span class="sc3">operator</span><span class="sc8">=</span><span class="sc6">"Equals"</span><span class="sc1">&gt;</span><span class="sc0">
                notepad.exe, explorer.exe, iexplore.exe, firefox.exe, chrome.exe
            </span><span class="sc1">&lt;/Filter&gt;</span><span class="sc0">
            </span><span class="sc1">&lt;Filter</span><span class="sc8"> </span><span class="sc3">name</span><span class="sc8">=</span><span class="sc6">"DefaultSystemProcesses"</span><span class="sc8"> </span><span class="sc3">appliesOn</span><span class="sc8">=</span><span class="sc6">"Processes"</span><span class="sc8"> </span><span class="sc3">property</span><span class="sc8">=</span><span class="sc6">"ImagePath"</span><span class="sc8"> </span><span class="sc3">operator</span><span class="sc8">=</span><span class="sc6">"StartsWith"</span><span class="sc1">&gt;</span><span class="sc0">
                gpupdate.exe, conhost.exe, svchost.exe, winlogon.exe, csrss.exe, lsass.exe, WUDFHost.exe, spoolsv.exe, unsecapp.exe
            </span><span class="sc1">&lt;/Filter&gt;</span><span class="sc0">
            </span><span class="sc1">&lt;Filter</span><span class="sc8"> </span><span class="sc3">name</span><span class="sc8">=</span><span class="sc6">"RunningProcesses"</span><span class="sc8"> </span><span class="sc3">appliesOn</span><span class="sc8">=</span><span class="sc6">"Processes"</span><span class="sc8"> </span><span class="sc3">property</span><span class="sc8">=</span><span class="sc6">"FinishTime"</span><span class="sc8"> </span><span class="sc3">operator</span><span class="sc8">=</span><span class="sc6">"Equals"</span><span class="sc1">&gt;</span><span class="sc0">
                0
            </span><span class="sc1">&lt;/Filter&gt;</span><span class="sc0">
            </span><span class="sc1">&lt;Filter</span><span class="sc8"> </span><span class="sc3">name</span><span class="sc8">=</span><span class="sc6">"CompletedProcesses"</span><span class="sc8"> </span><span class="sc3">appliesOn</span><span class="sc8">=</span><span class="sc6">"Processes"</span><span class="sc8"> </span><span class="sc3">property</span><span class="sc8">=</span><span class="sc6">"FinishTime"</span><span class="sc8"> </span><span class="sc3">operator</span><span class="sc8">=</span><span class="sc6">"NotEquals"</span><span class="sc1">&gt;</span><span class="sc0">
                0
            </span><span class="sc1">&lt;/Filter&gt;</span><span class="sc0">
            </span><span class="sc1">&lt;Filter</span><span class="sc8"> </span><span class="sc3">name</span><span class="sc8">=</span><span class="sc6">"HasLoadedWindowsDlls"</span><span class="sc8"> </span><span class="sc3">appliesOn</span><span class="sc8">=</span><span class="sc6">"Processes"</span><span class="sc8"> </span><span class="sc3">property</span><span class="sc8">=</span><span class="sc6">"Modules"</span><span class="sc8"> </span><span class="sc3">operator</span><span class="sc8">=</span><span class="sc6">"Contains"</span><span class="sc1">&gt;</span><span class="sc0">
                user32.dll, advapi32.dll
            </span><span class="sc1">&lt;/Filter&gt;</span><span class="sc0">
            </span><span class="sc1">&lt;Filter</span><span class="sc8"> </span><span class="sc3">name</span><span class="sc8">=</span><span class="sc6">"SomeGenericProcesses"</span><span class="sc8"> </span><span class="sc3">appliesOn</span><span class="sc8">=</span><span class="sc6">"Events"</span><span class="sc8"> </span><span class="sc3">property</span><span class="sc8">=</span><span class="sc6">"Operation"</span><span class="sc8"> </span><span class="sc3">operator</span><span class="sc8">=</span><span class="sc6">"Equals"</span><span class="sc1">&gt;</span><span class="sc0">
                "Load Image", QueryBasicInformationFile
            </span><span class="sc1">&lt;/Filter&gt;</span><span class="sc0">
            </span><span class="sc1">&lt;Filter</span><span class="sc8"> </span><span class="sc3">name</span><span class="sc8">=</span><span class="sc6">"SomeGenericProcesses"</span><span class="sc8"> </span><span class="sc3">appliesOn</span><span class="sc8">=</span><span class="sc6">"Events"</span><span class="sc8"> </span><span class="sc3">property</span><span class="sc8">=</span><span class="sc6">"Operation"</span><span class="sc8"> </span><span class="sc3">operator</span><span class="sc8">=</span><span class="sc6">"Equals"</span><span class="sc1">&gt;</span><span class="sc0">
                "Load Image", QueryBasicInformationFile
            </span><span class="sc1">&lt;/Filter&gt;</span><span class="sc0">
        </span><span class="sc1">&lt;/Create&gt;</span><span class="sc0">
        </span><span class="sc1">&lt;Conditions&gt;</span><span class="sc0">
            </span><span class="sc1">&lt;Condition</span><span class="sc8"> </span><span class="sc3">action</span><span class="sc8">=</span><span class="sc6">"Exclude"</span><span class="sc8"> </span><span class="sc3">operator</span><span class="sc8">=</span><span class="sc6">"And"</span><span class="sc1">&gt;</span><span class="sc0">
                DefaultUserProcesses, DefaultSystemProcesses
            </span><span class="sc1">&lt;/Condition&gt;</span><span class="sc0">
            </span><span class="sc1">&lt;Condition</span><span class="sc8"> </span><span class="sc3">action</span><span class="sc8">=</span><span class="sc6">"Include"</span><span class="sc8"> </span><span class="sc3">operator</span><span class="sc8">=</span><span class="sc6">"Or"</span><span class="sc1">&gt;</span><span class="sc0">
                RunningProcesses, HasLoadedWindowsDlls
            </span><span class="sc1">&lt;/Condition&gt;</span><span class="sc0">
            </span><span class="sc1">&lt;Condition</span><span class="sc8"> </span><span class="sc3">action</span><span class="sc8">=</span><span class="sc6">"Include"</span><span class="sc8"> </span><span class="sc3">operator</span><span class="sc8">=</span><span class="sc6">"Only"</span><span class="sc1">&gt;</span><span class="sc0">
                HasLoadedWindowsDlls
            </span><span class="sc1">&lt;/Condition&gt;</span><span class="sc0">
        </span><span class="sc1">&lt;/Conditions&gt;</span><span class="sc0">
    </span><span class="sc1">&lt;/Filters&gt;</span><span class="sc0">
</span><span class="sc1">&lt;/SeeBee&gt;</span>
</div>