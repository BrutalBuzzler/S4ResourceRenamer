# S4ResourceRenamer
A little command line tool to batch rename TS4 resources to the S4PE format.

---

## Tech
S4ResourceRenamer is a .NET 4.5 console application.
The solution is created in Visual Studio 2012.

* The main project has no dependencies.
* The test project uses NUnit and Rhino Mocks via NuGet.

## Functionality
The tool renames TS4 resources in the format  
> `{group}!{instance}.{type}`

to the S4PE format
> `S4_{type}_{group}_{instance}`.

So for example  
&nbsp;&nbsp;&nbsp;&nbsp;`0x00001234!0x07a00296298c0116.blueprint`  
becomes  
&nbsp;&nbsp;&nbsp;&nbsp;`S4_0x3924de26_0x00001234_0x07a00296298c0116.blueprint`.

*Note the type name in the end of the s4pe file name. I left it there so it's
easier to see the type in the s4pe format. s4pe doesn't mind the file
extension.*

The app currently supports the following resource types:

* trayitem
* blueprint
* bpi
* household
* hhi
* sgi
* room
* rmi

## Usage
You can start S4ResourceRenamer via console: 
`ResourceBatchRenamer {file name}`

Or you can simply drag&drop multiple files onto the tool using the Windows Explorer.
